using System;
namespace JewishCalendar
{
    /// <summary>
    /// Interface for the Jewish Date classes
    /// </summary>
    public interface IJewishDate
    {
        /// <summary>
        /// The Day in the month for this Jewish Date. 
        /// NOTE: If location was not supplied in the constructor the Jewish Day may not be correct 
        /// as from nightfall until midnight should really be the next Jewish day.       
        /// </summary>
        int Day { get; }
        /// <summary>
        /// The Jewish Month. As in the Torah, Nissan is month 1
        /// </summary>
        int Month { get; }
        /// <summary>
        /// The number of years since creation
        /// </summary>
        int Year { get; }
        /// <summary>
        /// The number of days elapsed since the theoretical Gregorian date Sunday, December 31, 1 BCE. 
        /// Since there is no year 0 in the calendar, the year following 1 BCE is 1 CE. 
        /// So, the Gregorian date January 1, 1 CE is absolute date number 1.
        /// </summary>
        int AbsoluteDate { get; }
        /// <summary>
        /// The day of the week for this Jewish Date
        /// </summary>
        DayOfWeek DayOfWeek { get; }
        /// <summary>
        /// The secular date on this Jewish Date
        /// </summary>
        DateTime GregorianDate { get; set; }    
        /// <summary>
        /// The name of the current Jewish Month (in English)
        /// </summary>
        string MonthName { get; }
        /// <summary>
        /// Returns the Jewish date in the format: The 14th day of Adar, 5775
        /// </summary>
        /// <returns></returns>
        string ToLongDateString();
        /// <summary>
        /// Returns the day of the Omer for this Jewish date. If it is not during Sefirah, 0 is returned
        /// </summary>        
        /// <returns></returns>
        int GetDayOfOmer();
        /// <summary>
        /// Returns the Jewish date in the format: Adar 14, 5775
        /// </summary>
        /// <returns></returns>
        string ToShortDateString();
        /// <summary>
        /// Returns the Jewish date in the format: יום חמישי כ"ט תשרי תשע"ה
        /// </summary>
        /// <returns></returns>
        string ToLongDateStringHeb();
        /// <summary>
        /// Returns the Jewish date in the format: כ"ו אלול תשע"ה 
        /// </summary>
        /// <returns></returns>
        string ToShortDateStringHeb();
        /// <summary>
        /// Returns the Jewish date in the format: Adar 14, 5775
        /// </summary>
        /// <returns></returns>
        string ToString();          
    }
}
