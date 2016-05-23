using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuachProject
{
    public enum UserOccasionTypes
    {
        OneTime,
        HebrewDateRecurringYearly,
        HebrewDateRecurringMonthly,
        SecularDateRecurringYearly,
        SecularDateRecurringMonthly
    }

    [Serializable]
    /// <summary>
    /// A single User Occasion or Event.
    /// </summary>
    public class UserOccasion
    {
        #region Private Fields

        [NonSerialized]
        private System.Drawing.RectangleF _rectangle;

        #endregion Private Fields

        #region Public Properties

        public ColorXML BackColor { get; set; }
        public ColorXML Color { get; set; }
        public JewishDate JewishDate { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public System.Drawing.RectangleF Rectangle
        {
            get { return this._rectangle; }
            set { this._rectangle = value; }
        }

        public DateTime SecularDate { get; set; }
        public UserOccasionTypes UserOccasionType { get; set; }

        public string ToString(bool heb = false)
        {
            if (heb)
            {
                switch (this.UserOccasionType)
                {
                    case UserOccasionTypes.OneTime:
                        return "אירוע חד פעמי בתאריך " +
                           this.JewishDate.ToLongDateStringHeb() + "  (" +
                           this.JewishDate.GregorianDate.ToString("d", Program.HebrewCultureInfo) + ")";
                    case UserOccasionTypes.HebrewDateRecurringYearly:
                        return "אירוע שנתי בכל " + this.JewishDate.Day.ToNumberHeb() +
                            " " + Utils.GetProperMonthNameHeb(this.JewishDate.Year, this.JewishDate.Month);
                    case UserOccasionTypes.HebrewDateRecurringMonthly:
                        return "אירוע חודשי בכל " + this.JewishDate.Day.ToNumberHeb() +
                            " לחודש העברי";
                    case UserOccasionTypes.SecularDateRecurringYearly:
                        return "אירוע שנתי בכל " + this.SecularDate.Day.ToString() +
                            " לחודש " + Program.HebrewCultureInfo.DateTimeFormat.MonthNames[this.SecularDate.Month];
                    case UserOccasionTypes.SecularDateRecurringMonthly:
                        return "אירוע חודשי בכל " + this.SecularDate.Day.ToString() + " לחודש הלועזי";
                }
            }
            else
            {
                switch (this.UserOccasionType)
                {
                    case UserOccasionTypes.OneTime:
                        return "One time event on " + this.JewishDate.ToLongDateString() + "  (" +
                            this.JewishDate.GregorianDate.ToShortDateString() + ")";
                    case UserOccasionTypes.HebrewDateRecurringYearly:
                        return "Yearly event on the " + this.JewishDate.Day.ToSuffixedString() +
                            " day of " + this.JewishDate.MonthName;
                    case UserOccasionTypes.HebrewDateRecurringMonthly:
                        return "Monthly event on the " + this.JewishDate.Day.ToSuffixedString()
                            + " day of each Jewish month";
                    case UserOccasionTypes.SecularDateRecurringYearly:
                        return "Yearly event on the " + this.SecularDate.Day.ToSuffixedString() +
                            " day of " + this.SecularDate.ToString("MMMM");
                    case UserOccasionTypes.SecularDateRecurringMonthly:
                        return "Monthly event on the " + this.SecularDate.Day.ToSuffixedString() +
                            " day of each Secular month";
                }
            }
            return "";
        }

        /// <summary>
        /// Gets a string describing the number of times this occasions "anniversary" has occurred.
        /// </summary>
        /// <param name="jd">The "current" date</param>
        /// <param name="hebrew">Should the string be in Hebrew?</param>
        /// <returns></returns>
        public string GetAnniversaryString(JewishDate jd, bool hebrew)
        {
            int num = this.GetNumberAnniversary(jd);
            if (num > 0)
            {
                if (this.UserOccasionType.In(UserOccasionTypes.HebrewDateRecurringYearly,
                                             UserOccasionTypes.SecularDateRecurringYearly))
                {
                    return (hebrew ? "שנה מספר " : "Year number ") +
                                num.ToString();
                }
                else if (this.UserOccasionType.In(UserOccasionTypes.HebrewDateRecurringMonthly,
                                                  UserOccasionTypes.SecularDateRecurringMonthly))
                {
                    return (hebrew ? "חודש מספר " : "Month number ") +
                             num.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the number of times this occasions has occurred by the given date.
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        public int GetNumberAnniversary(JewishDate jd)
        {
            switch (this.UserOccasionType)
            {
                case UserOccasionTypes.HebrewDateRecurringYearly:
                    return jd.Year - this.JewishDate.Year;

                case UserOccasionTypes.HebrewDateRecurringMonthly:
                    var months = 0;
                    //Add up all the months for all the intervening years
                    for (var year = this.JewishDate.Year; year < jd.Year; year++)
                    {
                        months += JewishDate.IsLeapYear(year) ? 13 : 12;
                    }
                    //Add or subtract months from the current year
                    months += jd.Month - this.JewishDate.Month;
                    return months;

                case UserOccasionTypes.SecularDateRecurringYearly:
                    return jd.GregorianDate.Year - this.SecularDate.Year;

                case UserOccasionTypes.SecularDateRecurringMonthly:
                    //Add all the months for all the years
                    months = (jd.GregorianDate.Year - this.SecularDate.Year) * 12;
                    //Add or subtract months from the current year
                    months += (jd.GregorianDate.Month - this.SecularDate.Month);
                    return months;
            }
            return 0;
        }

        #endregion Public Properties
    }

    [Serializable]
    /// <summary>
    /// Collection of UserOccasions.
    /// </summary>
    /// <remarks>The reason this was not simply accomplished with a Generic List
    /// is in order to have an easier time saving the list of occasions it to the user settings.</remarks>
    public class UserOccasionColection : List<UserOccasion>
    {
        #region Public Methods

        /// <summary>
        /// Gets all occasions and events for the given Jewish Date
        /// </summary>
        /// <param name="currDate"></param>
        /// <returns></returns>
        public static UserOccasionColection FromSettings(JewishDate currDate)
        {
            var col = new UserOccasionColection();
            col.AddRange(from uo in Properties.Settings.Default.UserOccasions
                         where (uo.UserOccasionType == UserOccasionTypes.OneTime &&
                            (uo.JewishDate == currDate || uo.SecularDate.Date == currDate.GregorianDate.Date)) ||
                               (uo.JewishDate <= currDate && (
                                   (uo.UserOccasionType == UserOccasionTypes.HebrewDateRecurringYearly && (uo.JewishDate.Day == currDate.Day && IsJewishMonthMatch(uo.JewishDate, currDate))) ||
                                   (uo.UserOccasionType == UserOccasionTypes.HebrewDateRecurringMonthly && (uo.JewishDate.Day == currDate.Day)) ||
                                   (uo.UserOccasionType == UserOccasionTypes.SecularDateRecurringYearly && (uo.SecularDate.Day == currDate.GregorianDate.Day && uo.SecularDate.Month == currDate.GregorianDate.Month)) ||
                                   (uo.UserOccasionType == UserOccasionTypes.SecularDateRecurringMonthly && (uo.SecularDate.Day == currDate.GregorianDate.Day))))
                         select uo);
            return col;
        }

        /// <summary>
        /// Determines if two months match  for a yahrtzeit or birthday etc.
        /// </summary>
        /// <param name="occDate"></param>
        /// <param name="currDate"></param>
        /// <returns></returns>
        private static bool IsJewishMonthMatch(JewishDate occDate, JewishDate currDate)
        {
            bool isOccLeap = JewishDate.IsLeapYear(occDate.Year),
                     isCurrLeap = JewishDate.IsLeapYear(currDate.Year);
            int occMonth = occDate.Month,
                currMonth = currDate.Month;

            if (((isOccLeap && !isCurrLeap) &&
                   ((occMonth == 13 && currMonth == 12) || (occMonth == 12 && currMonth == 11))) ||
               (((!isOccLeap) && isCurrLeap) &&
                   ((occMonth == 12 && currMonth == 13) || (occMonth == 11 && currMonth == 12))))
            {
                return true;
            }

            return occDate.Month == currDate.Month;
        }

        #endregion Public Methods
    }
}
