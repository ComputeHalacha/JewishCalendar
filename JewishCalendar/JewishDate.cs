using System;

namespace JewishCalendar
{
    /// <summary>
    /// Represents a single day in the Jewish calendar - Month are Nissan based.
    /// </summary>
    /// <remarks>The regular .NET class System.Globalization.HebrewCalendar has Tishrei as month #1.
    /// This becomes confusing, as months after Adar get a different number -
    /// depending on whether the year is a leap year or not.
    /// The Torah also instructs us to call Nissan the first month. (See Ramban in drasha for Rosh Hashana)
    /// Hence this "Nissan first" Jewish Date class -
    /// with all the underlying logic based on System.Globalization.HebrewCalendar (which was found to be very efficient).
    /// This class cannot be used with the .NET micro framework as it does not have access to System.Globalization.HebrewCalendar.
    /// To use this project with the .NET Micro Framework, you will need to remove this file before compiling.
    ///
    /// This class differs from the System.DateTime structure in that it does not directly have a time of day component. 
    /// The GregorianDate property can be used to keep track of the time of day.
    /// The AddMonths, and AddYears, addition and subtraction operator functions all preserve the time of day of the original GregorianDate.
    /// </remarks>
    [Serializable]
    public class JewishDate : IJewishDate
    {
        #region Private Fields
        //The minimum Jewish year that the HebrewCalendar calendar supports
        private const int MIN_HC_YEAR = 5344;
        private int _day, _month, _year;
        private DateTime _gregorianDate;
        
        #endregion Private Fields

        #region Public Constructors

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
        /// Creates a new JewishDate object with the specified Jewish year, Jewish month and Jewish day
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        public JewishDate(int year, int month, int day)
        {
            //If the year is less than the min supported date for Hebrew Calendar
            if (year <= MIN_HC_YEAR)
            {                
                this.GregorianDate = new JewishDateMicro(year, month, day).GregorianDate;
            }
            else
            {
                this.GregorianDate = new DateTime(year, GetTishrieMonth(month, year), day, Utils.HebrewCalendar);
            }
        }

        /// <summary>
        /// Creates a new JewishDate object with the specified Jewish year, Jewish month and Jewish day and time of day
        /// </summary>
        /// <param name="year">The year - counted from the creation of the world</param>
        /// <param name="month">The Jewish month. As it is in the Torah, Nissan is 1.</param>
        /// <param name="day">The day of the month</param>
        /// <param name="timeOfDay"></param>
        public JewishDate(int year, int month, int day, TimeSpan timeOfDay)
        {
            if (year <= MIN_HC_YEAR)
            {                
                this.GregorianDate = new JewishDateMicro(year, month, day).GregorianDate;
                this.GregorianDate.Add(timeOfDay);
            }
            else
            {
                this.GregorianDate = new DateTime(year, GetTishrieMonth(month, year), day,
                    timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds, Utils.HebrewCalendar);
            }
        }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date
        /// Note: as the location is not specified here, we cannot determine what time shkia is.
        /// So if the given time is after shkia, the Jewish date will be a Jewish Day early.
        /// </summary>
        /// <param name="dateTime">The Gregorian date from which to create the Jewish Date</param>
        public JewishDate(System.DateTime dateTime)
        {
            this.GregorianDate = dateTime;
        }

        /// <summary>
        /// Creates a Jewish date that corresponds to the given Gregorian date in the given location.
        /// Cut-off time is sunset.
        /// </summary>
        /// <param name="date">The Gregorian date from which to create the Jewish Date</param>
        /// <param name="location">The location. This will be used to determine the time of sunset.</param>
        public JewishDate(System.DateTime date, Location location)
            : this(date)
        {
            if (location != null)
            {
                Zmanim zman = new Zmanim(date, location);
                if (zman.GetShkia() <= date.TimeOfDay)
                {
                    /* If the current time is after sunset, the correct Jewish day is tomorrow, so we add a day.
                     * You may be wondering why we don't just do "this.Day++" or something like that...
                     * The explanation is, this class is just a wrapper around the functions of the
                     * System.Globalization.HebrewCalendar class and there is no internal representation
                     * of the Jewish Date other then it's underlying DateTime - stored in the GregorianDate property.
                     * You may note that this causes a bit of quirk in this class as the Secular Date for this Jewish Date
                     * can not be absolutely determined by accessing the GregorianDate property.
                     * Hence the GetSecularDate function, which returns the correct Secular DateTime. */
                    this.GregorianDate = this.GregorianDate.AddDays(1);
                }
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The maximum supported date by this class
        /// </summary>
        public static JewishDate MaxDate
        {
            get
            {
                return new JewishDate(Utils.HebrewCalendar.MaxSupportedDateTime);
            }
        }

        /// <summary>
        /// The minimum valid date supported by this class
        /// </summary>
        public static JewishDate MinDate
        {
            get
            {
                return new JewishDate(DateTime.MinValue);
            }
        }

        /// <summary>
        /// The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE.
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE.
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.
        /// </summary>
        public int AbsoluteDate
        {
            get
            {
                return (int)((this.GregorianDate.Subtract(new DateTime(1, 1, 1)).TotalDays + 1));
            }
        }

        /// <summary>
        /// The Day in the month for this Jewish Date.
        /// NOTE: If location was not supplied in the constructor the Jewish Day may not be correct
        /// as from nightfall until midnight should really be the next Jewish day.
        /// </summary>
        public int Day { get { return this._day; } }

        /// <summary>
        /// The day of the week for this Jewish Date
        /// </summary>
        public System.DayOfWeek DayOfWeek { get { return this.GregorianDate.DayOfWeek; } }

        /// <summary>
        /// The secular date at midnight of this Jewish Date.
        /// </summary>
        /// <remarks>
        /// If the current object was constructed with a constructor that takes a "Location"
        /// and the DateTime used by the constructor was after sunset but before midnight,
        /// the GregorianDate will be a day late as a day was added by the constructor.
        /// This anomaly is caused by the following un-mixable elements:
        ///     1. The correct Jewish date changes at sunset - so in Jewish, it is the next day.
        ///     2. The JewishDate class's calculations are based on the System.Globalization.HebrewCalendar functions -
        ///        which use System DateTime and do not consider sunset as a factor....
        ///        So in order to change the Jewish Date to the next day, we need to add a day to the
        ///        GregorianDate property as this class's connection to the functions of
        ///        System.Globalization.HebrewCalendar are all through it's GregorianDate property.
        /// To get the correct and proper Secular Date, use the GetSecularDate(HourMinute, Location) function.
        /// </remarks>
        public DateTime GregorianDate
        {
            get
            {
                return this._gregorianDate;
            }
            set
            {
                this._gregorianDate = value;

                //Calculate the values now and save them. 
                //This will save the need to do the calculations each time one of the Properties are called
                //If the date is less than the Hebrew Calendars min supported date we will work with the algorithms of the JewishDateMicro class.
                if (this._gregorianDate.Year <= 1583)
                {
                    var jdm = new JewishDateMicro(this._gregorianDate);
                    this._year = jdm.Year;
                    this._month = jdm.Month;
                    this._day = jdm.Day;
                }
                else
                {
                    this._year = Utils.HebrewCalendar.GetYear(value);
                    this._month = GetNissanMonth(Utils.HebrewCalendar.GetMonth(value), this._year);
                    this._day = Utils.HebrewCalendar.GetDayOfMonth(value);
                }
            }
        }

        /// <summary>
        /// The Jewish Month. As in the Torah, Nissan is month 1
        /// </summary>
        public int Month { get { return this._month; } }

        /// <summary>
        /// The name of the current Jewish Month (in English)
        /// </summary>
        public string MonthName { get { return Utils.GetProperMonthNameHeb(this.Year, this.Month); } }

        /// <summary>
        /// The number of years since creation
        /// </summary>
        public int Year { get { return this._year; } }

        #endregion Public Properties

        #region Public Instance Methods

        /// <summary>
        /// Gets the difference in months between two JewishDates
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
                    if (month > MonthsInYear(year))
                    {
                        month = 1;
                    }
                    else if (month == 7)
                    {
                        year++;
                    }
                }
                else {
                    months++;
                    month--;
                    if (month < 1)
                    {
                        month = MonthsInYear(year);
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
                miy = MonthsInYear(year);

            for (var i = 0; i < Math.Abs(months); i++)
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
                        miy = MonthsInYear(year);
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
                        miy = MonthsInYear(year);
                    }
                }
            }
            return new JewishDate(year, month, day, this.GregorianDate.TimeOfDay);
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
            if (month == 13 && !IsLeapYear(year))
            {
                month = 12;
            }
            else if (month == 8 && day == 30 && !IsLongCheshvan(year))
            {
                month = 9;
                day = 1;
            }
            else if (month == 9 && day == 30 && IsShortKislev(year))
            {
                month = 10;
                day = 1;
            }
            return new JewishDate(year, month, day, this.GregorianDate.TimeOfDay);
        }

        /// <summary>
        /// Compare this JewishDate to another. If the day, month and year are the same, will return true.
        /// </summary>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public override bool Equals(object jd2)
        {
            if (!(jd2 is IJewishDate))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, jd2))
            {
                return true;
            }
            return this.IsSameDate((IJewishDate)jd2);
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
        /// Return the HashCode for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Year.GetHashCode() ^ this.Month.GetHashCode() ^ this.Day.GetHashCode();
        }

        /// <summary>
        /// Returns the correct Secular Date for this JewishDate at the given Time and Location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="timeOfDay"></param>
        /// <returns></returns>
        /// <remarks>
        /// When using a JewishDate constructor that takes a "Location" object,
        /// if the initializing DateTime was after sunset, the date was set to the next day.
        /// The GregorianDate property will therefore not properly reflect the true Secular date until midnight.
        /// This function returns the correct GregorianDate for this JewishDate at the given time and place.
        /// </remarks>
        public System.DateTime GetSecularDate(HourMinute timeOfDay, Location location)
        {
            //Sunset is never, ever before mid-day (not even at the North and South Poles)
            if (timeOfDay.Hour > 12 && timeOfDay >= new Zmanim(this.GregorianDate, location).GetShkia())
            {
                // From sunset to midnight:
                // Jewish today is Secular tomorrow and Secular Today is Jewish Yesterday
                // (please sir, keep your yarmulka on!) [double meanings all around]
                return this.GregorianDate.AddDays(-1);
            }
            else
            {
                return this.GregorianDate;
            }
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
            sb.Append(Utils.JewishDOWNames[(int)this.DayOfWeek]);
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

        #endregion Public Instance Methods

        #region Public Static Methods

        /// <summary>
        /// Determines if the given Jewish Year is a Leap Year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.IsJewishLeapYear(int)">JewishDateCalculations.IsJewishLeapYear</see>,
        /// but internally uses System.Globalization.HebrewCalendar to retrieve its value.
        /// The algorithm for both functions are identical with the single difference being, that the 
        /// HebrewCalendar version does a check to make sure that the year is within the range of years
        /// that it can represent.</remarks>
        public static bool IsLeapYear(int year)
        {
            return Utils.HebrewCalendar.IsLeapYear(year);
        }

        /// <summary>
        /// Does Cheshvan have a full 30 days in the given Jewish Year?
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.IsLongCheshvan(int)">JewishDateCalculations.IsLongCheshvan</see>,
        /// but internally uses System.Globalization.HebrewCalendar to retrieve its value. 
        /// When using the JewishDate class which itself is based on System.Globalization.HebrewCalendar, 
        /// using the class specific version of the function is more efficient.</remarks>
        public static bool IsLongCheshvan(int year)
        {
            if (year <= MIN_HC_YEAR)
            {
                return JewishDateCalculations.IsLongCheshvan(year);
            }
            else
            {
                return Utils.HebrewCalendar.GetDaysInMonth(year, 2) == 30;
            }
        }

        /// <summary>
        /// Does Kislev have only 29 days for the given Jewish year?
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.IsShortKislev(int)">JewishDateCalculations.IsShortKislev</see>,
        /// but internally uses System.Globalization.HebrewCalendar to retrieve its value. 
        /// When using the JewishDate class which itself is based on System.Globalization.HebrewCalendar, 
        /// using the class specific version of the function is more efficient.</remarks>
        public static bool IsShortKislev(int year)
        {
            if (year <= MIN_HC_YEAR)
            {
                return JewishDateCalculations.IsShortKislev(year);
            }
            else
            {
                return Utils.HebrewCalendar.GetDaysInMonth(year, 3) == 29;
            }
        }

        /// <summary>
        /// Compute the total number of months for the given Hebrew year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.MonthsInJewishYear(int)">JewishDateCalculations.MonthsInJewishYear</see>,
        /// but internally uses HebrewCalendar.GetMonthsInYear which in turn uses HebrewCalendar.IsLeapYear 
        /// to determine if the year is a leap year or not.</remarks>
        public static int MonthsInYear(int year)
        {
            if (year <= MIN_HC_YEAR)
            {
                return JewishDateCalculations.MonthsInJewishYear(year);
            }
            else
            {
                return Utils.HebrewCalendar.GetMonthsInYear(year);
            }
        }

        /// <summary>
        /// Get the total number of days in the given Jewish year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.DaysInJewishYear(int)">JewishDateCalculations.DaysInJewishYear</see>,
        /// but internally uses System.Globalization.HebrewCalendar to retrieve its value. 
        /// When using the JewishDate class which itself is based on System.Globalization.HebrewCalendar, 
        /// using the class specific version of the function is more efficient.</remarks>
        public static int DaysInJewishYear(int year)
        {
            if (year <= MIN_HC_YEAR)
            {
                return JewishDateCalculations.DaysInJewishYear(year);
            }
            else
            {
                return Utils.HebrewCalendar.GetDaysInYear(year);
            }
        }

        /// <summary>
        /// Get the total number of days in the given Jewish Month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month">The Nissan based Jewish month</param>
        /// <returns></returns>
        /// <remarks>This function will return the same value as 
        /// <see cref="JewishDateCalculations.DaysInJewishMonth(int, int)">JewishDateCalculations.DaysInJewishMonth</see>,
        /// but internally uses System.Globalization.HebrewCalendar to retrieve its value. 
        /// When using the JewishDate class which itself is based on System.Globalization.HebrewCalendar, 
        /// using the class specific version of the function is more efficient.</remarks>
        public static int DaysInJewishMonth(int year, int month)
        {
            if (year <= MIN_HC_YEAR)
            {
                return JewishDateCalculations.DaysInJewishMonth(year, month);
            }
            else
            {
                return Utils.HebrewCalendar.GetDaysInMonth(year, GetTishrieMonth(month, year));
            }
        }
        #endregion Public Static Methods

        #region Private Methods        
        private static int GetNissanMonth(int tishrieMonth, int year)
        {
            if (tishrieMonth <= 6)
            {
                return tishrieMonth + 6;
            }
            else if (IsLeapYear(year))
            {
                if (tishrieMonth == 7)
                {
                    return 13;
                }
                else
                {
                    return tishrieMonth - 7;
                }
            }
            else
            {
                return tishrieMonth - 6;
            }
        }

        private static int GetTishrieMonth(int nissanMonth, int year)
        {
            if (nissanMonth >= 7)
            {
                return nissanMonth - 6;
            }
            else
            {
                return nissanMonth + (IsLeapYear(year) ? 7 : 6);
            }
        }

        #endregion Private Methods

        #region Operator Overloads
        /// <summary>
        /// Explicitly cast a JewishDateMicro to a JewishDate
        /// </summary>
        /// <param name="jd"></param>
        public static explicit operator JewishDate(JewishDateMicro jd)
        {
            return new JewishDate(jd.Year, jd.Month, jd.Day);
        }

        /// <summary>
        /// Subtract days from a Jewish date.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static JewishDate operator -(JewishDate hd, int days)
        {
            return new JewishDate(hd.GregorianDate.AddDays(-days));
        }

        /// <summary>
        /// Gets the difference in days between two Jewish dates.
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="hd2"></param>
        /// <returns></returns>
        public static int operator -(JewishDate hd, JewishDate hd2)
        {
            return hd.GregorianDate.Subtract(hd2.GregorianDate).Days;
        }

        /// <summary>
        /// Returns true if both objects do not have the same day, month and year
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator !=(JewishDate jd1, IJewishDate jd2)
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
            return new JewishDate(hd.GregorianDate.AddDays(days));
        }

        /// <summary>
        /// Returns true if the current JewishDate object is chronologically before the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <(JewishDate jd1, IJewishDate jd2)
        {
            return (jd1 != null && jd2 != null &&
                jd1.GregorianDate.Date < jd2.GregorianDate.Date);
        }

        /// <summary>
        /// Returns true if the current JewishDate object is not chronologically later than the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator <=(JewishDate jd1, IJewishDate jd2)
        {
            return (jd1 != null && jd2 != null &&
                (jd1 == jd2 || jd1 < jd2));
        }

        /// <summary>
        /// Returns true if both objects have the same day, month and year. You can also use the Equals function or the extension method IsSameDate(iJewishDate js) for the same purpose.
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator ==(JewishDate jd1, IJewishDate jd2)
        {
            if (Object.ReferenceEquals(jd1, null))
            {
                return Object.ReferenceEquals(jd2, null);
            }
            return jd1.Equals(jd2);
        }

        /// <summary>
        /// Returns true if the current JewishDate object is chronologically after the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >(JewishDate jd1, IJewishDate jd2)
        {
            return (jd1 != null && jd2 != null &&
                jd1.GregorianDate.Date > jd2.GregorianDate.Date);
        }

        /// <summary>
        /// Returns true if the current JewishDate object is not chronologically earlier than the second iJewishDate object
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool operator >=(JewishDate jd1, IJewishDate jd2)
        {
            return (jd1 != null && jd2 != null &&
                (jd1 > jd2 || jd1 == jd2));
        }
        #endregion

    }
}