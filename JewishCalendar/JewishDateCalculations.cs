/*****************************************************************************************************************************
 * Does much of the actual Jewish Date calculations.
 * Does not rely on System.Globalization.HebrewCalendar so can be safely used with .NET micro projects.
 *
 * Most of the Jewish date logic and calculations were translated from the C code by CBS -
 * which in turn were translated from the Lisp code in "Calendrical Calculations" by Nachum Dershowitz and Edward M. Reingold
 * in Software---Practice & Experience, vol. 20, no. 9 (September, 1990), pp. 899--928.
 *****************************************************************************************************************************/

using System;

namespace JewishCalendar
{
    /// <summary>
    /// Static class that contains functions for Jewish calendar calculations.
    /// </summary>
    public static class JewishDateCalculations
    {
        /// <summary>
        /// Absolute date of start of Jewish calendar
        /// </summary>
        public const int HEBREW_EPOCH = -1373429;

        /// <summary>
        /// Determines if the given Jewish Year is a Leap Year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.IsLeapYear(int)">JewishDate.IsLeapYear</see> instead of this function.</remarks>
        public static bool IsJewishLeapYear(int year)
        {
            return (((7 * year) + 1) % 19) < 7;
        }

        /// <summary>
        /// Compute the total number of months for the given Hebrew year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.MonthsInYear(int)">JewishDate.MonthsInYear</see> 
        /// instead of this function.</remarks>
        public static int MonthsInJewishYear(int year)
        {
            if (IsJewishLeapYear(year))
            {
                return 13;
            }
            else
            {
                return 12;
            }
        }        

        /// <summary>
        /// Does Cheshvan have a full 30 days in the given Jewish Year?
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.IsLongCheshvan(int)">JewishDate.IsLongCheshvan</see> 
        /// instead of this function.</remarks>
        public static bool IsLongCheshvan(int year)
        {
            return (DaysInJewishYear(year) % 10) == 5;
        }

        /// <summary>
        /// Does Kislev have only 29 days for the given Jewish year?
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.IsShortKislev(int)">JewishDate.IsShortKislev</see> 
        /// instead of this function.</remarks>
        public static bool IsShortKislev(int year)
        {
            return (DaysInJewishYear(year) % 10) == 3;
        }

        /// <summary>
        /// Compute the number of days in the given Jewish month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.DaysInJewishMonth(int, int)">JewishDate.DaysInJewishMonth</see> 
        /// instead of this function.</remarks>
        public static int DaysInJewishMonth(int year, int month)
        {
            if ((month == 2) || (month == 4) || (month == 6) || ((month == 8) &&
                (!IsLongCheshvan(year))) || ((month == 9) && IsShortKislev(year)) || (month == 10) || ((month == 12) &&
                (!IsJewishLeapYear(year))) || (month == 13))
            {
                return 29;
            }
            else
            {
                return 30;
            }
        }

        /// <summary>
        /// Get the total number of days in the given Hebrew year (From Rosh Hashana to the next Erev Rosh Hashana)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>If you are not using the .NET micro framework, use 
        /// <see cref="JewishDate.DaysInJewishYear(int)">JewishDate.DaysInHebrewYear</see> 
        /// instead of this function.</remarks>
        public static int DaysInJewishYear(int year)
        {
            return ((GetElapsedDays(year + 1)) - (GetElapsedDays(year)));
        }

        /// <summary>
        /// Compares 2 Jewish dates to see if they both represent the same day
        /// </summary>
        /// <param name="jd1"></param>
        /// <param name="jd2"></param>
        /// <returns></returns>
        public static bool IsSameDate(this IJewishDate jd1, IJewishDate jd2)
        {
            if (jd2 == null) return false;
            return jd1.Year == jd2.Year && jd1.Month == jd2.Month && jd1.Day == jd2.Day;
        }        

        /// <summary>
        /// The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE.
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE.
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.
        /// NOTE: This function should only be used if you don't have access to System.Globalization.HebrewCalendar.
        /// </summary>
        public static int GetAbsoluteFromJewishDate(int year, int month, int day)
        {
            int DayInYear = day; // Days so far this month.
            if (month < 7)
            { // Before Tishrei, so add days in prior months
                // this year before and after Nissan.
                int m = 7;
                while (m <= (MonthsInJewishYear(year)))
                {
                    DayInYear = DayInYear + DaysInJewishMonth(year, m);
                    m++;
                };
                m = 1;
                while (m < month)
                {
                    DayInYear = DayInYear + DaysInJewishMonth(year, m);
                    m++;
                }
            }
            else
            { // Add days in prior months this year
                int m = 7;
                while (m < month)
                {
                    DayInYear = DayInYear + DaysInJewishMonth(year, m);
                    m++;
                }
            }
            // Days elapsed before absolute date 1. -  Days in prior years.
            return DayInYear + (GetElapsedDays(year) + HEBREW_EPOCH);
        }

        /// <summary>
        /// Computed the absolute date for the given Gregorian Year, Month and Day
        /// NOTE: If you are not using the .NET micro framework, do not use this function!
        /// Use the following instead: (int)((YOUR_DATETIME.Subtract(new DateTime(1, 1, 1)).TotalDays + 1));
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetAbsoluteFromGregorianDate(int year, int month, int day)
        {
            int numberOfDays = day;           // days this month
            // add days in prior months this year
            for (int i = month - 1; i > 0; i--)
            {
                numberOfDays += DaysInGregorianMonth(i, year);
            }

            return (numberOfDays          // days this year
                   + 365 * (year - 1)     // days in previous years ignoring leap days
                   + (year - 1) / 4       // Julian leap days before this year...
                   - (year - 1) / 100     // ...minus prior century years...
                   + (year - 1) / 400);   // ...plus prior years divisible by 400
        }

        /// <summary>
        /// Compute the number of days in the given month of the Gregorian calendar.
        /// NOTE: If you are not using the .NET Micro framework, do not use this function!
        /// Use the GregorianCalendar.GetDaysInMonth function instead
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int DaysInGregorianMonth(int month, int year)
        {
            switch (month)
            {
                case 2:
                    if ((((year % 4) == 0) && ((year % 100) != 0))
                        || ((year % 400) == 0))
                        return 29;
                    else
                        return 28;

                case 4:
                case 6:
                case 9:
                case 11: return 30;
                default: return 31;
            }
        }

        /// <summary>
        /// Computes the number of days elapsed from the Sunday prior to the start of the
        /// Jewish calendar to the mean conjunction of Tishrei of the given Jewish year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static int GetElapsedDays(int year)
        {
            int MonthsElapsed = (235 * ((year - 1) / 19)) + (12 * ((year - 1) % 19)) + (7 * ((year - 1) % 19) + 1) / 19; // Leap months this cycle -  Regular months in this cycle. -  Months in complete cycles so far.
            int PartsElapsed = 204 + 793 * (MonthsElapsed % 1080);
            int HoursElapsed = 5 + 12 * MonthsElapsed + 793 * (MonthsElapsed / 1080) + PartsElapsed / 1080;
            int ConjunctionDay = (1 + 29 * MonthsElapsed + HoursElapsed / 24);
            int ConjunctionParts = 1080 * (HoursElapsed % 24) + PartsElapsed % 1080;
            int AlternativeDay;
            if ((ConjunctionParts >= 19440) || (((ConjunctionDay % 7) == 2) && (ConjunctionParts >= 9924) && (!IsJewishLeapYear(year))) || (((ConjunctionDay % 7) == 1) && (ConjunctionParts >= 16789) && (IsJewishLeapYear(year - 1)))) // at the end of a leap year -  15 hours, 589 parts or later... -  ...or is on a Monday at... -  ...of a common year, -  at 9 hours, 204 parts or later... -  ...or is on a Tuesday... -  If new moon is at or after midday,
            {
                // Then postpone Rosh HaShanah one day
                AlternativeDay = (ConjunctionDay + 1);
            }
            else
            {
                AlternativeDay = ConjunctionDay;
            }
            if (((AlternativeDay % 7) == 0) || ((AlternativeDay % 7) == 3) || ((AlternativeDay % 7) == 5)) // or Friday -  or Wednesday, -  If Rosh HaShanah would occur on Sunday,
            {
                // Then postpone it one (more) day
                return (1 + AlternativeDay);
            }
            else
            {
                return AlternativeDay;
            }
        }
    }
}