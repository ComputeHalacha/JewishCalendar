using System;

namespace JewishCalendar
{
    /// <summary>
    /// Represents a single special day
    /// </summary>
    public class SpecialDay
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Enums

        [FlagsAttribute]
        /// <summary>
        /// Types of special days
        /// </summary>
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

        #endregion Public Enums

        #region Public Properties

        /// <summary>
        /// Type of special day. Can have multiple values.
        /// </summary>
        public SpecialDayTypes DayType { get; set; }

        /// <summary>
        /// Name of this special day in English
        /// </summary>
        public string NameEnglish { get; set; }

        /// <summary>
        /// Name of this special day in Hebrew
        /// </summary>
        public string NameHebrew { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the name of this special day in English.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.NameEnglish;
        }

        #endregion Public Methods
    }
}