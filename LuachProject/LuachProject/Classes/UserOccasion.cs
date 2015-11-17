using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuachProject
{
    [Serializable]
    public class UserOccasionColection : List<UserOccasion>
    {
        public static UserOccasionColection FromSettings(JewishDate currDate)
        {
            var col = new UserOccasionColection();
            col.AddRange(from uo in Properties.Settings.Default.UserOccasions
                         where (uo.UserOccasionType == UserOccasionTypes.OneTime && (uo.JewishDate == currDate || uo.SecularDate.Date == currDate.GregorianDate.Date)) ||
                               (uo.UserOccasionType == UserOccasionTypes.HebrewDateRecurringYearly && (uo.JewishDate.Month == currDate.Month && uo.JewishDate.Day == currDate.Day)) ||
                               (uo.UserOccasionType == UserOccasionTypes.HebrewDateRecurringMonthly && (uo.JewishDate.Day == currDate.Day)) ||
                               (uo.UserOccasionType == UserOccasionTypes.SecularDateRecurringYearly && (uo.SecularDate.Month == currDate.GregorianDate.Month && uo.SecularDate.Day == currDate.GregorianDate.Day)) ||
                               (uo.UserOccasionType == UserOccasionTypes.SecularDateRecurringMonthly && (uo.SecularDate.Day == currDate.GregorianDate.Day))
                         select uo);  
            return col;
        }
    }

    public enum UserOccasionTypes
    {
        OneTime,
        HebrewDateRecurringYearly,
        HebrewDateRecurringMonthly,
        SecularDateRecurringYearly,
        SecularDateRecurringMonthly
    }

    [Serializable]
    public class UserOccasion
    {
        public UserOccasionTypes UserOccasionType { get; set; }
        public JewishDate JewishDate { get; set; }
        public DateTime SecularDate { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }       
        public ColorXML Color { get; set; }
        public ColorXML BackColor { get; set; }
        public System.Drawing.RectangleF Rectangle { get; set; }
    }
}
