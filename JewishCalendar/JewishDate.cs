namespace JewishCalendar
{
    /// <summary>
    /// Represents a single day in the Jewish calendar - Month are Nissan based.
    /// </summary>
    /// <remarks>
    /// The calculations and functions used by this class's representation of the Jewish Date are based on open source algorithms.
    /// 
    /// <list type="numeric">
    /// <listheader>This class has 3 main advantages over System.Globalization.HebrewCalendar:</listheader>
    /// <item>
    /// The Months are numbered from Nissan as opposed to Tishrei. The regular .NET class System.Globalization.HebrewCalendar has Tishrei as month #1.
    /// This becomes confusing, as months after Adar get a different number -
    /// depending on whether the year is a leap year or not.
    /// The Torah also instructs us to call Nissan the first month. (See Ramban in drasha for Rosh Hashana)
    /// Hence this "Nissan first" Jewish Date class.
    /// </item>
    /// <item>
    /// It can represent Jewish dates before the Common Era
    /// </item>
    /// <item>
    /// It can be used for .NET Micro Framework projects which do not have access to System.Globalization.HebrewCalendar
    /// </item>
    /// </list>     
    /// </remarks>

    [System.Serializable]
    public class JewishDate
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

        /// <summary>
        /// Represents the time of day for this JewishDate
        /// </summary>
        public System.TimeSpan TimeOfDay { get; set; }
        /// <summary>
        /// Get the Gregorian Date for the current Hebrew Date
        /// </summary>
        /// <returns></returns>
        public System.DateTime GregorianDate { get; private set; }

        /// <summary>
        /// The Hour component of the time of day represented by this Jewish Date
        /// </summary>
        public int Hour { get { return this.TimeOfDay.Hours; } }

        /// <summary>
        /// The Minute component of the time of day represented by this Jewish Date
        /// </summary>
        public int Minute { get { return this.TimeOfDay.Minutes; } }

        /// <summary>
        /// The Second component of the time of day represented by this Jewish Date
        /// </summary>
        public int Second { get { return this.TimeOfDay.Seconds; } }

        /// <summary>
        /// The Millisecond component of the time of day represented by this Jewish Date
        /// </summary>
        public int Millisecond { get { return this.TimeOfDay.Milliseconds; } }

        /// <summary>
        /// Minimum valid date that can be represented by this class
        /// </summary>
        public static JewishDate MinDate { get; private set; }
        /// <summary>
        /// Maximum valid date that can be represented by this class
        /// </summary>
        public static JewishDate MaxDate { get; private set; }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// static constructor
        /// </summary>
        static JewishDate()
        {
            MinDate = new JewishDate(1, 7, 1);
            MaxDate = new JewishDate(5999, 6, 29);
        }

        /// <summary>
        /// Empty constructor. Sets the date to the current system date.
        /// </summary>
        public JewishDate() : this(System.DateTime.Now) { }

        /// <summary>
        /// Get the current Jewish date in the given location. Cut-off time is sunset.
        /// </summary>
        /// <param name="location">The location. This will be used to determine the time of sunset.</param>
        public JewishDate(Location location) : this(System.DateTime.Now, location) { }

        /// <summary>
        /// Creates a new JewishDate with the specified Hebrew year, month, day and absolute day.
        /// This is the quickest constructor as it does no calculations at all. 
        /// Caution: If the absolute day doesn't correctly match the given year/month/day, weird things will happen.
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        /// <param name="absoluteDay">The "absolute day"</param>
        public JewishDate(int year, int month, int day, int absoluteDay)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.AbsoluteDate = absoluteDay;
            this.GregorianDate = JewishDateCalculations.GetGregorianDateFromJewishDate(this);
        }

        /// <summary>
        /// Creates a new JewishDate with the specified Hebrew year, month and day
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        public JewishDate(int year, int month, int day) :
            this(year, month, day, JewishDateCalculations.GetAbsoluteFromJewishDate(year, month, day))
        { }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date
        /// </summary>
        /// <param name="date">The Gregorian date from which to create the Jewish Date</param>
        public JewishDate(System.DateTime date)            
        {
            this.SetFromAbsoluteDate(JewishDateCalculations.GetAbsoluteFromGregorianDate(date));
            this.GregorianDate = date;
            this.TimeOfDay = date.TimeOfDay;
        }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date in the given location. Cut-off time is sunset.
        /// </summary>
        /// <param name="date">The Gregorian date from which to create the Jewish Date</param>
        /// <param name="location">The location. This will be used to determine the time of sunset.</param>
        public JewishDate(System.DateTime date, Location location)
        {
            int abs = JewishDateCalculations.GetAbsoluteFromGregorianDate(date);
            Zmanim zman = new Zmanim(date, location);

            if (zman.GetShkia() <= date.TimeOfDay)
            {
                abs++;
            }
            
            this.SetFromAbsoluteDate(abs);
            this.GregorianDate = date;
            this.TimeOfDay = date.TimeOfDay;
        }

        /// <summary>
        /// Creates a Hebrew date from the "absolute date".
        /// In other words, the Hebrew date on the day that is the given number of days after/before December 31st, 1 BCE
        /// </summary>
        /// <param name="absoluteDate">The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE.
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE.
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.</param>
        public JewishDate(int absoluteDate)
        {
            this.SetFromAbsoluteDate(absoluteDate);
            this.GregorianDate = JewishDateCalculations.GetGregorianDateFromJewishDate(this);
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
            while (absoluteDate >= JewishDateCalculations.GetAbsoluteFromJewishDate((this.Year + 1), 7, 1))
            {
                this.Year++;
            }
            // Search forward for month from either Tishrei or Nissan.
            if (absoluteDate < JewishDateCalculations.GetAbsoluteFromJewishDate(this.Year, 1, 1))
            {
                this.Month = 7; //  Start at Tishrei
            }
            else
            {
                this.Month = 1; //  Start at Nissan
            }
            while (absoluteDate > JewishDateCalculations.GetAbsoluteFromJewishDate(
                this.Year, 
                this.Month, 
                (JewishDateCalculations.DaysInJewishMonth(this.Year, this.Month))))
            {
                this.Month++;
            }
            // Calculate the day by subtraction.
            this.Day = (absoluteDate - JewishDateCalculations.GetAbsoluteFromJewishDate(this.Year, this.Month, 1) + 1);
        }

        #endregion Constructors

        #region Public Functions
        /// <summary>
        /// Returns true if both objects have the same day, month and year. You can also use the == operator or the extension method IsSameDate(JewishDate js) for the same purpose.
        /// </summary>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public override bool Equals(object jd2)
        {
            if (!(jd2 is JewishDate))
            {
                return false;
            }
            if (object.ReferenceEquals(this, jd2))
            {
                return true;
            }
            return this.IsSameDate((JewishDate)jd2);
        }

        /// <summary>
        /// Gets the difference in months between two JewishDates. 
        /// If the second date is before this one, the number will be negative.
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        /// <remarks>Ignores Day part. For example, from 29 Kislev to 1 Teves will 
        /// return 1 even though they are only a day or two apart</remarks>
        public int DateDiffMonth(JewishDate jd)
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
        public JewishDate AddMonths(int months)
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
            return new JewishDate(year, month, day);
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
        public JewishDate AddYears(int years)
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
            return new JewishDate(year, month, day);
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
                dayOfOmer = (this - new JewishDate(this.Year, 1, 15));
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
        public static JewishDate operator -(JewishDate hd, int days)
        {
            return new JewishDate(hd.AbsoluteDate - days);
        }

        /// <summary>
        /// Gets the difference in days between two Jewish dates.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="hd2"></param>
        /// <returns></returns>
        public static int operator -(JewishDate hd, JewishDate hd2)
        {
            return hd.AbsoluteDate - hd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if both objects do not have the same day, month and year
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator !=(JewishDate jd1, JewishDate jd2)
        {
            return !(jd1 == jd2);
        }

        /// <summary>
        /// Add days to a Jewish date.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static JewishDate operator +(JewishDate hd, int days)
        {
            return new JewishDate(hd.AbsoluteDate + days);
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is chronologically before the second JewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <(JewishDate jd1, JewishDate jd2)
        {
            if (jd1 == null || jd2 == null)
            {
                return false;
            }
            return jd1.AbsoluteDate < jd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is not chronologically later than the second JewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <=(JewishDate jd1, JewishDate jd2)
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
        /// Returns true if both objects have the same day, month and year. You can also use the Equals function or the extension method IsSameDate(JewishDate js) for the same purpose.
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator ==(JewishDate jd1, JewishDate jd2)
        {
            if (object.ReferenceEquals(jd1, null))
            {
                return object.ReferenceEquals(jd2, null);
            }
            return jd1.Equals(jd2);
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is chronologically after the second JewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >(JewishDate jd1, JewishDate jd2)
        {
            if (jd1 == null || jd2 == null || jd1 == jd2)
            {
                return false;
            }
            return jd1.AbsoluteDate > jd2.AbsoluteDate;
        }

        /// <summary>
        /// Returns true if the current JewishDateMicro object is not chronologically earlier than the second JewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >=(JewishDate jd1, JewishDate jd2)
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