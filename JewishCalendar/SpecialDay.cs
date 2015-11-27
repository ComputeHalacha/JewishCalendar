using System;

namespace JewishCalendar
{
    /// <summary>
    /// Represents a single special day
    /// </summary>
    public class SpecialDay
    {
        /// <summary>
        /// Types of special days
        /// </summary>
        [FlagsAttribute]
        public enum SpecialDayTypes
        {
            /// <summary>
            /// Shabbos
            /// </summary>
            Shabbos = 2,

            /// <summary>
            /// Major Yom Tov - where melacha is prohibited
            /// </summary>
            MajorYomTov = 4,

            /// <summary>
            /// Minor Yom Tov - where Melacha is permitted
            /// </summary>
            MinorYomtov = 16,

            /// <summary>
            /// A fast day
            /// </summary>
            FastDay = 32,

            /// <summary>
            /// Extra day information
            /// </summary>
            Information = 64,

            /// <summary>
            /// Erev shabbos or yomtov
            /// </summary>
            HasCandleLighting = 128
        };

        /// <summary>
        /// Name of this special day in English
        /// </summary>
        public string NameEnglish { get; set; }

        /// <summary>
        /// Name of this special day in Hebrew
        /// </summary>
        public string NameHebrew { get; set; }

        /// <summary>
        /// Type of special day. Can have multiple values.
        /// </summary>
        public SpecialDayTypes DayType { get; set; }

        /// <summary>
        /// Create a new SpecialDay instance.
        /// </summary>
        /// <param name="nameEnglish"></param>
        /// <param name="nameHebrew"></param>
        /// <param name="dayTypes"></param>
        public SpecialDay(string nameEnglish, string nameHebrew, SpecialDayTypes dayTypes)
        {
            this.NameEnglish = nameEnglish;
            this.NameHebrew = nameHebrew;
            this.DayType = dayTypes;
        }

        /// <summary>
        /// Returns the name of this special day in English.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.NameEnglish;
        }
    }
}