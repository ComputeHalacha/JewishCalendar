namespace JewishCalendar
{
    /// <summary>
    /// Represents a single day in the Jewish Calendar.
    /// </summary>
    /// <remarks>
    /// System.Globalization.HebrewCalendar is not available in the .NET Micro Framework,
    /// so this class was created for use in MF projects.
    /// The calculations and functions used by this class's representation of the Jewish Date are based on open source algorithms.
    /// 
    /// This class does not have a time of day component. 
    /// The GregorianDate property can not be used to keep track of the time of day as the System.DateTime object
    /// is generated anew for each call to the GregorianDate property and  it will always have a time of day of 12 AM.
    /// 
    /// IMPORTANT NOTE: for all other projects besides for Micro Framework projects, you should use the
    /// JewishCalendar.JewishDate class which being based on System.Globalization.HebrewCalendar, 
    /// is slightly more efficient - in benchmark testing we found it to be slightly quicker than this class.
    /// 
    /// When using the JewishCalendar dll with the .NET Micro Framework, the following items needs to be removed before compiling:
    ///      1. The entire file "JewishDate.cs"
    ///      2. The following line in "Utils.cs": public static HebrewCalendar HebrewCalendar = new HebrewCalendar();
    ///      3. The following line in "Utils.cs": HebrewCultureInfo.DateTimeFormat.Calendar = HebrewCalendar;
    /// </remarks>

    [System.Serializable]
    public class JewishDateMicro : IJewishDate
    {
        #region Public Properties

        /// <summary>
        /// The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE.
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE.
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.
        /// </summary>
        public int AbsoluteDate { get; private set; }

        /// <summary>
        /// The Day in the month for this Jewish Date.
        /// NOTE: Not always correct; from nightfall until midnight should really be the next Jewish day.
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// The index of the day of the week for this Jewish Date. Sunday is 0.
        /// </summary>
        public int DayInWeek { get { return System.Math.Abs(this.AbsoluteDate % 7); } }

        /// <summary>
        /// The day of the week for this Jewish Date (from Midnight to Midnight)
        /// </summary>
        public System.DayOfWeek DayOfWeek { get { return (System.DayOfWeek)this.DayInWeek; } }

        /// <summary>
        /// The Jewish Month. As in the Torah, Nissan is month 1
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// The name of the current Jewish Month (in English)
        /// </summary>
        public string MonthName { get { return Utils.GetProperMonthName(this.Year, this.Month); } }

        /// <summary>
        /// The number of years since creation
        /// </summary>
        public int Year { get; private set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Empty constructor. Sets the date to the current system date.
        /// </summary>
        public JewishDateMicro() : this(System.DateTime.Now) { }

        /// <summary>
        /// Get the current Jewish date in the given location. Cut-off time is sunset.
        /// </summary>
        /// <param name="location">The location. This will be used to determine the time of sunset.</param>
        public JewishDateMicro(Location location) : this(System.DateTime.Now, location) { }

        /// <summary>
        /// Creates a new JewishDate with the specified Hebrew year, month, day and absolute day.
        /// This is the quickest constructor as it does no calculations at all. 
        /// Caution: If the absolute day doesn't correctly match the given year/month/day, weird things will happen.
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        /// <param name="absoluteDay">The "absolute day"</param>
        public JewishDateMicro(int year, int month, int day, int absoluteDay)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.AbsoluteDate = absoluteDay;
        }

        /// <summary>
        /// Creates a new JewishDate with the specified Hebrew year, month and day
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        public JewishDateMicro(int year, int month, int day) :
            this(year, month, day, JewishDateCalculations.GetAbsoluteFromJewishDate(year, month, day))
        { }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date
        /// </summary>
        /// <param name="date">The Gregorian date from which to create the Jewish Date</param>
        public JewishDateMicro(System.DateTime date) :
            this(JewishDateCalculations.GetAbsoluteFromGregorianDate(date.Year, date.Month, date.Day))
        { }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date in the given location. Cut-off time is sunset.
        /// </summary>
        /// <param name="date">The Gregorian date from which to create the Jewish Date</param>
        /// <param name="location">The location. This will be used to determine the time of sunset.</param>
        public JewishDateMicro(System.DateTime date, Location location)
        {
            int absoluteDate = JewishDateCalculations.GetAbsoluteFromGregorianDate(date.Year, date.Month, date.Day);
            Zmanim zman = new Zmanim(date, location);
            if (zman.GetShkia() <= date.TimeOfDay)
            {
                absoluteDate++;
            }
            this.SetFromAbsoluteDate(absoluteDate);
        }

        /// <summary>
        /// Creates a Hebrew date from the "absolute date".
        /// In other words, the Hebrew date on the day that is the given number of days after/before December 31st, 1 BCE
        /// </summary>
        /// <param name="absoluteDate">The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE.
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE.
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.</param>
        public JewishDateMicro(int absoluteDate)
        {
            this.SetFromAbsoluteDate(absoluteDate);
        }

        /// <summary>
        /// Sets the current Jewish date to the date represented by the given "Absolute Date" -
        /// which is the number of days after/before December 31st, 1 BCE.
        /// The logic here was translated from the C code - which in turn were translated
        /// from the Lisp code in ''Calendrical Calculations'' by Nachum Dershowitz and Edward M. Reingold in
        /// Software---Practice &amp; Experience, vol. 20, no. 9 (September, 1990), pp. 899--928.
        /// </summary>
        /// <param name="absoluteDate"></param>
        private void SetFromAbsoluteDate(int absoluteDate)
        {
            this.AbsoluteDate = absoluteDate;

            //To save on calculations, start with an estimation of a few years before date
            this.Year = 3761 + (absoluteDate / (absoluteDate > 0 ? 366 : 300));

            //The following in from the original code; it starts the calculations way back when and takes almost as long to calculate all of them...
            //this.Year = ((absoluteDate + JewishDateCalculations.HEBREW_EPOCH) / 366); // Approximation from below.

            // Search forward for year from the approximation.
            while (absoluteDate >= new JewishDateMicro((this.Year + 1), 7, 1).AbsoluteDate)
            {
                this.Year++;
            }
            // Search forward for month from either Tishrei or Nissan.
            if (absoluteDate < new JewishDateMicro(this.Year, 1, 1).AbsoluteDate)
            {
                this.Month = 7; //  Start at Tishrei
            }
            else
            {
                this.Month = 1; //  Start at Nissan
            }
            while (absoluteDate > new JewishDateMicro(this.Year, this.Month, (JewishDateCalculations.DaysInJewishMonth(this.Year, this.Month))).AbsoluteDate)
            {
                this.Month++;
            }
            // Calculate the day by subtraction.
            this.Day = (absoluteDate - new JewishDateMicro(this.Year, this.Month, 1).AbsoluteDate + 1);
        }

        #endregion Constructors

        #region Public Functions

        /// <summary>
        /// Get the Gregorian Date for the current Hebrew Date
        /// </summary>
        /// <returns></returns>
        public System.DateTime GregorianDate
        {
            get
            {
                int d = this.AbsoluteDate;
                if (d >= 730120) // 1/1/2000
                {
                    return new System.DateTime(2000, 1, 1).AddDays(d - 730120);
                }
                else
                {
                    int day, month, year;

                    // Search forward year by year from approximate year
                    year = d / 366;
                    while (d >= JewishDateCalculations.GetAbsoluteFromGregorianDate(year + 1, 1, 1))
                        year++;
                    // Search forward month by month from January
                    month = 1;
                    while (d > JewishDateCalculations.GetAbsoluteFromGregorianDate(
                        year, month, JewishDateCalculations.DaysInGregorianMonth(month, year)))
                        month++;
                    day = d - JewishDateCalculations.GetAbsoluteFromGregorianDate(year, month, 1) + 1;

                    return new System.DateTime(year, month, day);
                }
            }
            set
            {
                var absoluteDate = JewishDateCalculations.GetAbsoluteFromGregorianDate(value.Year, value.Month, value.Day);
                this.SetFromAbsoluteDate(absoluteDate);
            }
        }

        /// <summary>
        /// Returns true if both objects have the same day, month and year. You can also use the == operator or the extension method IsSameDate(iJewishDate js) for the same purpose.
        /// </summary>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public override bool Equals(object jd2)
        {
            if (!(jd2 is IJewishDate))
            {
                return false;
            }
            if (object.ReferenceEquals(this, jd2))
            {
                return true;
            }
            return this.IsSameDate((IJewishDate)jd2);
        }

        /// <summary>
        /// Gets the difference in months between two JewishDates. 
        /// If the second date is before this one, the number will be negative.
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        /// <remarks>Ignores Day part. For example, from 29 Kislev to 1 Teves will 
        /// return 1 even though they are only a day or two apart</remarks>
        public int DateDiffMonth(IJewishDate jd)
        {
            int month = jd.Month,
             year = jd.Year,
             months = 0;

            while (!(year == this.Year && month == this.Month))
            {
                if (this.AbsoluteDate > jd.AbsoluteDate)
                {
                    months--;
                    month++;
                    if (month > JewishDateCalculations.MonthsInJewishYear(year))
                    {
                        month = 1;
                    }
                    else if (month == 7)
                    {
                        year++;
                    }
                }
                else
                {
                    months++;
                    month--;
                    if (month < 1)
                    {
                        month = JewishDateCalculations.MonthsInJewishYear(year);
                    }
                    else if (month == 6)
                    {
                        year--;
                    }
                }
            }

            return months;
        }

        /// <summary>
        /// Adds the given number of months to the current date and returns the new Jewish Date
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public IJewishDate AddMonths(int months)
        {
            int year = this.Year,
                month = this.Month,
                day = this.Day,
                miy = JewishDateCalculations.MonthsInJewishYear(year);

            for (var i = 0; i < System.Math.Abs(months); i++)
            {
                if (months > 0)
                {
                    month += 1;
                    if (month > miy)
                    {
                        month = 1;
                    }
                    if (month == 7)
                    {
                        year += 1;
                        miy = JewishDateCalculations.MonthsInJewishYear(year);
                    }
                }
                else if (months < 0)
                {
                    month -= 1;
                    if (month == 0)
                    {
                        month = miy;
                    }
                    if (month == 6)
                    {
                        year -= 1;
                        miy = JewishDateCalculations.MonthsInJewishYear(year);
                    }
                }
            }
            return new JewishDateMicro(year, month, day);
        }

        /// <summary>
        /// Adds the given number of years to the current date and returns the new Jewish Date
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        /// <remarks>If the current month is Adar Sheini and the new year is not a leap year, the month is set to Adar.
        /// If the current Day is the 30th of Cheshvan or Kislev and in the new year that month only has 29 days, 
        /// the day is set to the 1st of the following month.
        /// </remarks>
        public IJewishDate AddYears(int years)
        {
            int year = this.Year + years,
                month = this.Month,
                day = this.Day;

            if (month == 13 && !JewishDateCalculations.IsJewishLeapYear(year))
            {
                month = 12;
            }
            else if (month == 8 && day == 30 && !JewishDateCalculations.IsLongCheshvan(year))
            {
                month = 9;
                day = 1;
            }
            else if (month == 9 && day == 30 && JewishDateCalculations.IsShortKislev(year))
            {
                month = 10;
                day = 1;
            }
            return new JewishDateMicro(year, month, day);
        }

        /// <summary>
        /// Returns the day of the Omer for the given Jewish date. If the given day is not during Sefirah, 0 is returned
        /// </summary>
        /// <returns></returns>
        public int GetDayOfOmer()
        {
            int dayOfOmer = 0;
            if ((this.Month == 1 && this.Day > 15) || this.Month == 2 || (this.Month == 3 && this.Day < 6))
            {
                dayOfOmer = (this - new JewishDateMicro(this.Year, 1, 15));
            }
            return dayOfOmer;
        }

        /// <summary>
        /// Returns the HashCode for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Year.GetHashCode() ^ this.Month.GetHashCode() ^ this.Day.GetHashCode();
        }

        /// <summary>
        /// Returns the Jewish date in the format: The 14th day of Adar, 5775
        /// </summary>
        /// <returns></returns>
        public string ToLongDateString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("The ");
            sb.Append(this.Day.ToSuffixedString());
            sb.Append(" day of ");
            sb.Append(this.MonthName);
            sb.Append(", " + this.Year.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Jewish date in the format: יום חמישי כ"ט תשרי תשע"ה
        /// </summary>
        /// <returns></returns>
        public string ToLongDateStringHeb()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(Utils.JewishDOWNames[this.DayInWeek]);
            sb.Append(" ");
            sb.Append(this.ToShortDateStringHeb());
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Jewish date in the format: Adar 14, 5775
        /// </summary>
        /// <returns></returns>
        public string ToShortDateString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(this.MonthName);
            sb.Append(" " + this.Day);
            sb.Append(", " + this.Year.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Jewish date in the format: כ"ו אלול תשע"ה
        /// </summary>
        /// <returns></returns>
        public string ToShortDateStringHeb()
        {
            var sb = new System.Text.StringBuilder();
            //Note for the .net micro framework there are no "format" functions
            sb.Append(this.Day.ToNumberHeb());
            sb.Append(" ");
            sb.Append(Utils.GetProperMonthNameHeb(this.Year, this.Month));
            sb.Append(" ");
            sb.Append(this.Year.ToNumberHeb());
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Jewish date in the format: Adar 14, 5775
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToShortDateString();
        }

        #endregion Public Functions

        #region Operator Functions

        /// <summary>
        /// Subtract days from a Jewish date.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static JewishDateMicro operator -(JewishDateMicro hd, int days)
        {
            return new JewishDateMicro(hd.AbsoluteDate - days);
        }

        /// <summary>
        /// Gets the difference in days between two Jewish dates.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="hd2"></param>
        /// <returns></returns>
        public static int operator -(JewishDateMicro hd, JewishDateMicro hd2)
        {
            return hd.AbsoluteDate - hd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if both objects do not have the same day, month and year
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator !=(JewishDateMicro jd1, IJewishDate jd2)
        {
            return !(jd1 == jd2);
        }

        /// <summary>
        /// Add days to a Jewish date.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static JewishDateMicro operator +(JewishDateMicro hd, int days)
        {
            return new JewishDateMicro(hd.AbsoluteDate + days);
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is chronologically before the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <(JewishDateMicro jd1, IJewishDate jd2)
        {
            if (jd1 == null || jd2 == null)
            {
                return false;
            }
            return jd1.AbsoluteDate < jd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is not chronologically later than the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <=(JewishDateMicro jd1, IJewishDate jd2)
        {
            if (jd1 == null || jd2 == null)
            {
                return false;
            }
            if (jd1 == jd2)
            {
                return true;
            }
            return jd1 < jd2;
        }

        /// <summary>
        /// Returns true if both objects have the same day, month and year. You can also use the Equals function or the extension method IsSameDate(iJewishDate js) for the same purpose.
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator ==(JewishDateMicro jd1, IJewishDate jd2)
        {
            if (object.ReferenceEquals(jd1, null))
            {
                return object.ReferenceEquals(jd2, null);
            }
            return jd1.Equals(jd2);
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is chronologically after the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >(JewishDateMicro jd1, IJewishDate jd2)
        {
            if (jd1 == null || jd2 == null || jd1 == jd2)
            {
                return false;
            }
            return jd1.AbsoluteDate > jd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is not chronologically earlier than the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >=(JewishDateMicro jd1, IJewishDate jd2)
        {
            if (jd1 == null || jd2 == null)
            {
                return false;
            }
            if (jd1 == jd2)
            {
                return true;
            }
            return jd1 > jd2;
        }

        #endregion Operator Functions
    }
}