﻿/* **********************************************************************************************
 * Computes the daily Zmanim and Yomim Tovim for a single date.
 * Most of the astronomical mathematical calculations were directly adapted from the excellent
 * Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005)
 * **********************************************************************************************/

using System;
using System.Collections;

namespace JewishCalendar
{
    /// <summary>
    /// Computes the daily Zmanim and Yomim Tovim for a single Jewish date.
    /// </summary>
    /// <remarks>Most of the astronomical mathematical calculations were directly adapted from the excellent
    /// Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005)</remarks>
    public class Zmanim
    {
        #region properties

        /// <summary>
        /// The Location to cheshbon the zmanim for
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Zmanim are by the secular date
        /// </summary>
        public DateTime SecularDate { get; set; }

        #endregion properties

        #region constructors

        /// <summary>
        /// Create a new Zmanim instance for the given secular day and Location
        /// </summary>
        /// <param name="d"></param>
        /// <param name="loc"></param>
        public Zmanim(DateTime d, Location loc)
        {
            this.SecularDate = d;
            this.Location = loc;
        }

        /// <summary>
        /// Create a new Zmanim instance for the given Jewish day and Location
        /// </summary>
        /// <param name="hd"></param>
        /// <param name="loc"></param>
        public Zmanim(IJewishDate hd, Location loc)
            : this(hd.GregorianDate, loc)
        { }

        #endregion constructors

        #region public instance functions

        /// <summary>
        /// Gets chatzos of both day and night for current location.
        /// Configured from netz to shkia at sea level
        /// </summary>
        /// <returns></returns>
        public HourMinute GetChatzos()
        {
            return GetChatzos(this.SecularDate, this.Location);
        }

        /// <summary>
        /// Gets sunrise for current location  (at the locations altitude)
        /// </summary>
        /// <returns></returns>
        public HourMinute GetNetz()
        {
            var netzShkia = this.GetNetzShkia();
            if (netzShkia == null) { return new HourMinute(); }
            return netzShkia[0];
        }

        /// <summary>
        /// Gets an array of two HourMinute structures. The first is the time of Netz for the current date and location and the second is the time of shkia.
        /// </summary>
        /// <param name="considerElevation"></param>
        /// <returns></returns>
        public HourMinute[] GetNetzShkia(bool considerElevation = true)
        {
            return GetNetzShkia(this.SecularDate, this.Location, considerElevation);
        }

        /// <summary>
        /// Gets length of Shaa zmanis in minutes for current location.
        /// Configured from netz to shkia at sea level.
        /// </summary>
        /// <param name="offset">Number of minutes before/after shkia/netz to cheshbon</param>
        /// <returns></returns>
        public double GetShaaZmanis(int offset = 0)
        {
            return GetShaaZmanis(this.SecularDate, this.Location, offset);
        }

        /// <summary>
        /// Gets sunset for current location  (at the locations altitude)
        /// </summary>
        /// <returns></returns>
        public HourMinute GetShkia()
        {
            var netzShkia = this.GetNetzShkia();
            if (netzShkia == null) { return new HourMinute(); }
            return netzShkia[1];
        }

        #endregion public instance functions

        #region public static functions

        /// <summary>
        /// Gets chatzos of both day and night for given date and location.
        /// Configured from netz to shkia at sea level
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static HourMinute GetChatzos(DateTime date, Location location)
        {
            HourMinute[] netzShkia = GetNetzShkia(date, location, false);
            HourMinute netz = netzShkia[0],
                       shkia = netzShkia[1];

            if (netz == HourMinute.NoValue || shkia == HourMinute.NoValue)
            {
                return HourMinute.NoValue;
            }

            var chatz = (int)((shkia.TotalMinutes - netz.TotalMinutes) / 2);
            return netz + chatz;
        }

        /// <summary>
        /// Gets a list of special days and information about the given Jewish Date
        /// </summary>
        /// <param name="jDate"></param>
        /// <param name="inIsrael"></param>
        /// <returns></returns>
        /// <remarks>We use an ArrayList rather than a generic List to accommodate
        /// the .NET Micro framework which does not support generic lists.
        /// For regular projects just use as follows: GetHolidays(jDate, inIsrael).Cast&lt;JewishCalendar.SpecialDay&gt;()
        /// </remarks>
        public static ArrayList GetHolidays(IJewishDate jDate, bool inIsrael)
        {
            ArrayList list = new ArrayList();
            int jYear = jDate.Year;
            int jMonth = jDate.Month;
            int jDay = jDate.Day;
            DayOfWeek dayOfWeek = jDate.DayOfWeek;
            bool isLeapYear = JewishDateCalculations.IsJewishLeapYear(jYear);
            DateTime secDate = jDate.GregorianDate;

            if (dayOfWeek == DayOfWeek.Friday)
            {
                list.Add(new SpecialDay("Erev Shabbos", "ערב שבת",
                    SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                list.Add(new SpecialDay("Shabbos Kodesh", "שבת קודש", SpecialDayTypes.Shabbos));

                if (jMonth == 1 && jDay > 7 && jDay < 15)
                {
                    list.Add(new SpecialDay("Shabbos HaGadol", "שבת הגדול"));
                }
                else if (jMonth == 7 && jDay > 2 && jDay < 10)
                {
                    list.Add(new SpecialDay("Shabbos Shuva", "שבת שובה"));
                }
                else if (jMonth == 5 && jDay > 2 && jDay < 10)
                {
                    list.Add(new SpecialDay("Shabbos Chazon", "שבת חזון"));
                }
                else if ((jMonth == (isLeapYear ? 12 : 11) && jDay > 23 && jDay < 30) ||
                        (jMonth == (isLeapYear ? 13 : 12) && jDay == 1))
                {
                    list.Add(new SpecialDay("Parshas Shkalim", "פרשת שקלים"));
                }
                else if (jMonth == (isLeapYear ? 13 : 12) && jDay > 7 && jDay < 14)
                {
                    list.Add(new SpecialDay("Parshas Zachor", "פרשת זכור"));
                }
                else if (jMonth == (isLeapYear ? 13 : 12) && jDay > 16 && jDay < 24)
                {
                    list.Add(new SpecialDay("Parshas Parah", "פרשת פרה"));
                }
                else if ((jMonth == (isLeapYear ? 13 : 12) && jDay > 23 && jDay < 30) ||
                        (jMonth == 1 && jDay == 1))
                {
                    list.Add(new SpecialDay("Parshas Hachodesh", "פרשת החודש"));
                }

                if (jMonth != 6 && jDay > 22 && jDay < 30)
                {
                    list.Add(new SpecialDay("Shabbos Mevarchim", "מברכים החודש"));
                }
                //Perkei Avos. In Nissan from after the last day of Pesach
                if ((jMonth == 1 && jDay > (inIsrael ? 21 : 22)) ||
                    //All Shabbosim through Iyar, Sivan, Tamuz, Av and Ellul - besides for the day/s of Shavuos
                    ((jMonth > 1 && jMonth < 7 && !((jMonth == 3 && jDay == 6) || (!inIsrael && jMonth == 3 && jDay == 7))) ||
                    //In Tishrei until Sukkos besides for Rosh HAshana and Yom Kippur
                    (jMonth == 7 && jDay < 15 && jDay > 2 && jDay != 10)))
                {
                    //Get the week number of since Pesach
                    var perekAvos = ((jDate.AbsoluteDate - (new JewishDateMicro(jYear, 1, (inIsrael ? 22 : 23))).AbsoluteDate) % 6) + 1;
                    //If shavuos was on Shabbos, we miss a week
                    if(jMonth > 3 || (jMonth == 3 && jDay > 6) && new JewishDateMicro(jYear, 3, 6).DayOfWeek == DayOfWeek.Saturday )
                    {
                        perekAvos--;
                    }
                    //If Rosh Hashana was on Shabbos, we miss a week
                    else if (jMonth == 7 && new JewishDateMicro(jYear, 1, 1).DayOfWeek == DayOfWeek.Saturday)
                    {
                        perekAvos--;
                    }
                    //If Yom Kippur was on Shabbos, we miss a week
                    else if (jMonth == 7 &&  jDay > 10 && new JewishDateMicro(jYear, 1, 10).DayOfWeek == DayOfWeek.Saturday)
                    {
                        perekAvos--;
                    }

                    if (perekAvos > 0)
                    {
                        list.Add(new SpecialDay(perekAvos.ToSuffixedString() + 
                            "Pirkei Avos - Perek", "פרקי אבות - פרק " + perekAvos.ToNumberHeb()));
                    }
                }
            }
            if (jDay == 30)
            {
                int monthIndex = (jMonth == 12 && !isLeapYear) || jMonth == 13 ? 1 : jMonth + 1;
                list.Add(new SpecialDay("Rosh Chodesh " + Utils.GetProperMonthName(jYear, jMonth),
                   "ראש חודש " + Utils.GetProperMonthNameHeb(jYear, monthIndex),
                   SpecialDayTypes.MinorYomtov));
            }
            else if (jDay == 1 && jMonth != 7)
            {
                list.Add(new SpecialDay("Rosh Chodesh " + Utils.GetProperMonthName(jYear, jMonth),
                   "ראש חודש " + Utils.GetProperMonthNameHeb(jYear, jMonth),
                   SpecialDayTypes.MinorYomtov));
            }

            //V'sain Tal U'Matar in Chutz La'aretz is according to the secular date
            if (secDate.Month == 12 && secDate.Day.In(5, 6) && !inIsrael)
            {
                var nextYearIsLeap = JewishDateCalculations.IsJewishLeapYear(jYear + 1);
                if (((secDate.Day == 5 && !nextYearIsLeap)) || (secDate.Day == 6 && nextYearIsLeap))
                    list.Add(new SpecialDay("V'sain Tal U'Matar", "ותן טל ומטר"));
            }

            switch (jMonth)
            {
                case 1: //Nissan
                    if (jDay == 12 && dayOfWeek == DayOfWeek.Thursday)
                        list.Add(new SpecialDay("Bedikas Chametz", "בדיקת חמץ"));
                    else if (jDay == 13 && dayOfWeek != DayOfWeek.Friday)
                        list.Add(new SpecialDay("Bedikas Chametz", "בדיקת חמץ"));
                    else if (jDay == 14)
                    {
                        if ((!inIsrael) && dayOfWeek == DayOfWeek.Wednesday)
                        {
                            list.Add(new SpecialDay("Erev Pesach", "ערב פסח",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Erev Pesach", "ערב פסח",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    else if (jDay == 15)
                        list.Add(new SpecialDay("First Day of Pesach", "פסח - יום ראשון", SpecialDayTypes.MajorYomTov));
                    else if (jDay == 16)
                        list.Add(inIsrael ?
                           (new SpecialDay("Pesach - Chol HaMoed", "פסח - חול המועד", SpecialDayTypes.MinorYomtov)) :
                           (new SpecialDay("Pesach - Second Day", "פסח - יום שני", SpecialDayTypes.MajorYomTov)));
                    else if (jDay.In(17, 18, 19))
                        list.Add(new SpecialDay("Pesach - Chol Ha'moed", "פסח - חול המועד", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 20)
                    {
                        if (dayOfWeek == DayOfWeek.Thursday || ((!inIsrael) && dayOfWeek == DayOfWeek.Wednesday))
                        {
                            list.Add(new SpecialDay("Pesach - Chol Ha'moed - Erev Yomtov", "פסח - חול המועד - ערב יו\"ט",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Pesach - Chol Ha'moed - Erev Yomtov", "פסח - חול המועד - ערב יו\"ט",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    else if (jDay == 21)
                        list.Add(new SpecialDay("7th Day of Pesach", "שביעי של פסח", SpecialDayTypes.MajorYomTov));
                    else if (jDay == 22 && !inIsrael)
                        list.Add(new SpecialDay("Last Day of Pesach", "אחרון של פסח", SpecialDayTypes.MajorYomTov));
                    break;

                case 2: //Iyar
                    if (dayOfWeek == DayOfWeek.Monday && jDay > 2 && jDay < 12)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית שני קמא", SpecialDayTypes.FastDay));
                    }
                    else if (dayOfWeek == DayOfWeek.Thursday && jDay > 5 && jDay < 13)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית חמישי", SpecialDayTypes.FastDay));
                    }
                    else if (dayOfWeek == DayOfWeek.Monday && jDay > 9 && jDay < 17)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית שני בתרא", SpecialDayTypes.FastDay));
                    }
                    if (jDay == 14)
                        list.Add(new SpecialDay("Pesach Sheini", "פסח שני", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 18)
                        list.Add(new SpecialDay("Lag BaOmer", "ל\"ג בעומר", SpecialDayTypes.MinorYomtov));
                    break;

                case 3: //Sivan
                    if (jDay == 5)
                    {
                        if (dayOfWeek == DayOfWeek.Thursday || ((!inIsrael) && dayOfWeek == DayOfWeek.Wednesday))
                        {
                            list.Add(new SpecialDay("Erev Shavuos", "ערב שבועות",
                                SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Erev Shavuos", "ערב שבועות", SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    else if (jDay == 6)
                        list.Add((inIsrael ?
                            new SpecialDay("Shavuos", "חג השבועות", SpecialDayTypes.MajorYomTov) :
                            new SpecialDay("Shavuos - First Day", "שבועות - יום ראשון", SpecialDayTypes.MajorYomTov)));
                    if (jDay == 7 && !inIsrael)
                        list.Add(new SpecialDay("Shavuos - Second Day", "שבועות - יום שני"));
                    break;

                case 4: //Tamuz
                    if (jDay == 17 && dayOfWeek != DayOfWeek.Saturday)
                    {
                        list.Add(new SpecialDay("Fast - 17th of Tammuz", "צום י\"ז בתמוז", SpecialDayTypes.FastDay));
                    }
                    else if (jDay == 18 && dayOfWeek == DayOfWeek.Sunday)
                    {
                        list.Add(new SpecialDay("Fast - 17th of Tammuz", "צום י\"ז בתמוז", SpecialDayTypes.FastDay));
                    }
                    break;

                case 5: //Av
                    if (jDay == 9 && dayOfWeek != DayOfWeek.Saturday)
                        list.Add(new SpecialDay("Tisha B'Av", "תשעה באב", SpecialDayTypes.FastDay));
                    else if (jDay == 10 && dayOfWeek == DayOfWeek.Sunday)
                        list.Add(new SpecialDay("Tisha B'Av", "תשעה באב", SpecialDayTypes.FastDay));
                    else if (jDay == 15)
                        list.Add(new SpecialDay("Tu B'Av", "ט\"ו באב", SpecialDayTypes.MinorYomtov));
                    break;

                case 6: //Ellul
                    if (jDay == 29)
                    {
                        if (dayOfWeek == DayOfWeek.Wednesday)
                        {
                            list.Add(new SpecialDay("Erev Rosh Hashana", "ערב ראש השנה",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Erev Rosh Hashana", "ערב ראש השנה",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    break;

                case 7: //Tishrei
                    if (jDay == 1)
                        list.Add(new SpecialDay("Rosh Hashana - First Day", "ראש השנה", SpecialDayTypes.MajorYomTov));
                    else if (jDay == 2)
                        list.Add(new SpecialDay("Rosh Hashana - Second Day", "ראש השנה", SpecialDayTypes.MajorYomTov));
                    else if (jDay == 3 && dayOfWeek != DayOfWeek.Saturday)
                        list.Add(new SpecialDay("Tzom Gedalia", "צום גדליה", SpecialDayTypes.FastDay));
                    else if (jDay == 4 && dayOfWeek == DayOfWeek.Sunday)
                        list.Add(new SpecialDay("Tzom Gedalia", "צום גדליה", SpecialDayTypes.FastDay));
                    else if (jDay == 9)
                        list.Add(new SpecialDay("Erev Yom Kippur", "ערב יום הכיפורים", SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                    else if (jDay == 10)
                        list.Add(new SpecialDay("Yom Kippur", "יום הכיפורים", SpecialDayTypes.MajorYomTov | SpecialDayTypes.FastDay));
                    else if (jDay == 14)
                    {
                        if ((!inIsrael) && dayOfWeek == DayOfWeek.Wednesday)
                        {
                            list.Add(new SpecialDay("Erev Sukkos", "ערב חג הסוכות",
                                SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Erev Sukkos", "ערב חג הסוכות",
                                SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    else if (jDay == 15)
                        list.Add(new SpecialDay("First Day of Sukkos", "חג הסוכות", SpecialDayTypes.MajorYomTov));
                    else if (jDay == 16)
                        list.Add(inIsrael ? (
                           new SpecialDay("Sukkos - Chol HaMoed", "סוכות - חול המועד", SpecialDayTypes.MinorYomtov)) : (
                           new SpecialDay("Sukkos - Second Day", "יום שני - חג הסוכות", SpecialDayTypes.MajorYomTov)));
                    else if (jDay.In(17, 18, 19, 20))
                        list.Add(new SpecialDay("Sukkos - Chol HaMoed", "סוכות - חול המועד", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 21)
                    {
                        if ((!inIsrael) && dayOfWeek == DayOfWeek.Wednesday)
                        {
                            list.Add(new SpecialDay("Hoshana Rabba - Erev Yomtov", "הושענא רבה - ערב יו\"ט",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting | SpecialDayTypes.EruvTavshilin));
                        }
                        else
                        {
                            list.Add(new SpecialDay("Hoshana Rabba - Erev Yomtov", "הושענא רבה - ערב יו\"ט",
                                SpecialDayTypes.MinorYomtov | SpecialDayTypes.Information | SpecialDayTypes.HasCandleLighting));
                        }
                    }
                    else if (jDay == 22)
                    {
                        list.Add(new SpecialDay("Shmini Atzeres", "שמיני עצרת", SpecialDayTypes.MajorYomTov));
                        if (inIsrael)
                            list.Add(new SpecialDay("Simchas Torah", "שמחת תורה", SpecialDayTypes.MajorYomTov));
                    }
                    else if (jDay == 23 && !inIsrael)
                        list.Add(new SpecialDay("Simchas Torah", "שמחת תורה", SpecialDayTypes.MajorYomTov));
                    break;

                case 8: //Cheshvan
                    if (dayOfWeek == DayOfWeek.Monday && jDay > 2 && jDay < 12)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית שני קמא", SpecialDayTypes.FastDay));
                    }
                    else if (dayOfWeek == DayOfWeek.Thursday && jDay > 5 && jDay < 13)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית חמישי", SpecialDayTypes.FastDay));
                    }
                    else if (dayOfWeek == DayOfWeek.Monday && jDay > 9 && jDay < 17)
                    {
                        list.Add(new SpecialDay("Baha\"b", "תענית שני בתרא", SpecialDayTypes.FastDay));
                    }
                    if (jDay == 7 && inIsrael)
                        list.Add(new SpecialDay("V'sain Tal U'Matar", "ותן טל ומטר"));
                    break;

                case 9: //Kislev
                    if (jDay == 25)
                        list.Add(new SpecialDay("Chanuka - One Candle", "'חנוכה - נר א", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 26)
                        list.Add(new SpecialDay("Chanuka - Two Candles", "'חנוכה - נר ב", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 27)
                        list.Add(new SpecialDay("Chanuka - Three Candles", "'חנוכה - נר ג", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 28)
                        list.Add(new SpecialDay("Chanuka - Four Candles", "'חנוכה - נר ד", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 29)
                        list.Add(new SpecialDay("Chanuka - Five Candles", "'חנוכה - נר ה", SpecialDayTypes.MinorYomtov));
                    else if (jDay == 30)
                        list.Add(new SpecialDay("Chanuka - Six Candles", "'חנוכה - נר ו", SpecialDayTypes.MinorYomtov));
                    break;

                case 10: //Teves
                    if (JewishDateCalculations.IsShortKislev(jYear))
                    {
                        if (jDay == 1)
                            list.Add(new SpecialDay("Chanuka - Six Candles", "'חנוכה - נר ו", SpecialDayTypes.MinorYomtov));
                        else if (jDay == 2)
                            list.Add(new SpecialDay("Chanuka - Seven Candles", "'חנוכה - נר ז", SpecialDayTypes.MinorYomtov));
                        else if (jDay == 3)
                            list.Add(new SpecialDay("Chanuka - Eight Candles", "'חנוכה - נר ח", SpecialDayTypes.MinorYomtov));
                    }
                    else
                    {
                        if (jDay == 1)
                            list.Add(new SpecialDay("Chanuka - Seven Candles", "'חנוכה - נר ז", SpecialDayTypes.MinorYomtov));
                        else if (jDay == 2)
                            list.Add(new SpecialDay("Chanuka - Eight Candles", "'חנוכה - נר ח", SpecialDayTypes.MinorYomtov));
                    }
                    if (jDay == 10)
                        list.Add(new SpecialDay("Fast - 10th of Teves", "צום עשרה בטבת", SpecialDayTypes.FastDay));
                    break;

                case 11: //Shvat
                    if (jDay == 15)
                        list.Add(new SpecialDay("Tu B'Shvat", "ט\"ו בשבט", SpecialDayTypes.MinorYomtov));
                    break;
                case 12: //Adars
                case 13:
                    if (jMonth == 12 && isLeapYear)
                    {
                        if (jDay == 14)
                            list.Add(new SpecialDay("Purim Katan", "פורים קטן", SpecialDayTypes.MinorYomtov));
                        else if (jDay == 15)
                            list.Add(new SpecialDay("Shushan Purim Katan", "שושן פורים קטן", SpecialDayTypes.MinorYomtov));
                    }
                    else
                    {
                        if (jDay == 11 && dayOfWeek == DayOfWeek.Thursday)
                            list.Add(new SpecialDay("Fast - Taanis Esther", "תענית אסתר", SpecialDayTypes.FastDay));
                        else if (jDay == 13 && dayOfWeek != DayOfWeek.Saturday)
                            list.Add(new SpecialDay("Fast - Taanis Esther", "תענית אסתר", SpecialDayTypes.FastDay));
                        else if (jDay == 14)
                            list.Add(new SpecialDay("Purim", "פורים", SpecialDayTypes.MinorYomtov));
                        else if (jDay == 15)
                            list.Add(new SpecialDay("Shushan Purim", "שושן פורים", SpecialDayTypes.MinorYomtov));
                    }
                    break;
            }

            if ((jMonth == 1 && jDay > 15) || jMonth == 2 || (jMonth == 3 && jDay < 6))
            {
                int dayOfSefirah = jDate.GetDayOfOmer();
                if (dayOfSefirah > 0)
                {
                    list.Add(new SpecialDay("Sefiras Ha'omer - Day " + dayOfSefirah.ToString(), "ספירת העומר - יום " + dayOfSefirah.ToString()));
                }
            }
            //Remove any candle lighting from shabbos....
            if (dayOfWeek == DayOfWeek.Saturday)
            {
                foreach (SpecialDay sd in list)
                {
                    if (sd.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting))
                    {
                        sd.DayType = (SpecialDayTypes)(sd.DayType - SpecialDayTypes.HasCandleLighting);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Gets a dash delimited list of holidays for the given Jewish Day
        /// </summary>
        ///<param name="holidayList"></param>
        ///<param name="delimiter"></param>
        ///<param name="hebrew"></param>
        /// <returns></returns>
        public static string GetHolidaysText(SpecialDay[] holidayList, string delimiter, bool hebrew)
        {
            string holidays = "";
            foreach (SpecialDay yt in holidayList)
            {
                if (holidays.Length > 0)
                {
                    holidays += delimiter;
                }
                holidays += (hebrew ? yt.NameHebrew : yt.NameEnglish);
            }
            return holidays;
        }

        /// <summary>
        /// Gets a dash delimited list of holidays for the given Jewish Day
        /// </summary>
        /// <param name="jdate"></param>
        /// <param name="inIsrael"></param>
        /// <param name="hebrew"></param>
        /// <returns></returns>
        public static string GetHolidaysText(IJewishDate jdate, bool inIsrael, bool hebrew)
        {
            return GetHolidaysText(GetHolidays(jdate, inIsrael), " - ", hebrew);
        }

        /// <summary>
        /// Gets a dash delimited list of holidays for the given Jewish Day
        /// </summary>
        ///<param name="holidayList"></param>
        ///<param name="delimiter"></param>
        ///<param name="hebrew"></param>
        /// <returns></returns>
        public static string GetHolidaysText(ArrayList holidayList, string delimiter, bool hebrew)
        {
            var list = new SpecialDay[holidayList.Count];
            for (int i = 0; i < holidayList.Count; i++)
            {
                list[i] = (SpecialDay)holidayList[i];
            }
            return GetHolidaysText(list, delimiter, hebrew);
        }

        /// <summary>
        /// Get time of sunrise for the given location and date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <param name="considerElevation"></param>
        /// <returns></returns>
        public static HourMinute GetNetz(DateTime date, Location location, bool considerElevation = true)
        {
            var netzShkia = GetNetzShkia(date, location, considerElevation);
            if (netzShkia == null) { return new HourMinute(); }
            return netzShkia[0];
        }

        /// <summary>
        /// Gets length of Shaa zmanis in minutes for given date and location.
        /// Configured from netz to shkia at sea level.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <param name="offset">Number of minutes before/after shkia/netz to cheshbon</param>
        /// <returns></returns>
        public static double GetShaaZmanis(DateTime date, Location location, int offset = 0)
        {
            HourMinute[] netzShkia = GetNetzShkia(date, location, false);
            if (netzShkia[0] == HourMinute.NoValue || netzShkia[1] == HourMinute.NoValue) { return 0; }
            HourMinute netz = netzShkia[0] - offset,
                shkia = netzShkia[1] + offset;

            return (shkia.TotalMinutes - netz.TotalMinutes) / 12;
        }

        /// <summary>
        /// Get time of sunset for the given location and date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <param name="considerElevation"></param>
        /// <returns></returns>
        public static HourMinute GetShkia(DateTime date, Location location, bool considerElevation = true)
        {
            var netzShkia = GetNetzShkia(date, location, considerElevation);
            if (netzShkia == null) { return new HourMinute(); }
            return netzShkia[1];
        }

        #endregion public static functions

        #region Astronomical Calculations

        /// <summary>
        /// Gets an array of two HourMinute structures.
        /// The first is the time of sunrise for the given date and location and the second is the time of sunset.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="location"></param>
        /// <param name="considerElevation"></param>
        /// <returns></returns>
        public static HourMinute[] GetNetzShkia(DateTime date, Location location, bool considerElevation = true)
        {
            HourMinute sunrise = HourMinute.NoValue, sunset = HourMinute.NoValue;
            int day = GetDayOfYear(date);
            double zeninthDeg = 90, zenithMin = 50, lonHour = 0, longitude = 0, latitude = 0, cosLat = 0, sinLat = 0, cosZen = 0, sinDec = 0, cosDec = 0,
                xmRise = 0, xmSet = 0, xlRise = 0, xlSet = 0, aRise = 0, aSet = 0, ahrRise = 0, ahrSet = 0,
                hRise = 0, hSet = 0, tRise = 0, tSet = 0, utRise = 0, utSet = 0, earthRadius = 6356900,
                zenithAtElevation = DegToDec(zeninthDeg, zenithMin) + RadToDeg(Math.Acos(earthRadius / (earthRadius +
                    (considerElevation ? location.Elevation : 0))));

            zeninthDeg = Math.Floor(zenithAtElevation);
            zenithMin = (zenithAtElevation - Math.Floor(zenithAtElevation)) * 60;
            cosZen = Math.Cos(0.01745 * DegToDec(zeninthDeg, zenithMin));
            longitude = DegToDec(location.LongitudeDegrees, location.LongitudeMinutes) *
                (location.LongitudeType == Location.LongitudeTypes.West ? 1 : -1);
            lonHour = longitude / 15;
            latitude = DegToDec(location.LatitudeDegrees, location.LatitudeMinutes) *
                (location.LatitudeType == Location.LatitudeTypes.North ? 1 : -1);
            cosLat = Math.Cos(0.01745 * latitude);
            sinLat = Math.Sin(0.01745 * latitude);
            tRise = day + (6 + lonHour) / 24;
            tSet = day + (18 + lonHour) / 24;
            xmRise = M(tRise);
            xlRise = L(xmRise);
            xmSet = M(tSet);
            xlSet = L(xmSet);
            aRise = 57.29578 * Math.Atan(0.91746 * Math.Tan(0.01745 * xlRise));
            aSet = 57.29578 * Math.Atan(0.91746 * Math.Tan(0.01745 * xlSet));
            if (Math.Abs(aRise + 360 - xlRise) > 90)
            {
                aRise += 180;
            }
            if (aRise > 360)
            {
                aRise -= 360;
            }
            if (Math.Abs(aSet + 360 - xlSet) > 90)
            {
                aSet += 180;
            }
            if (aSet > 360)
            {
                aSet -= 360;
            }
            ahrRise = aRise / 15;
            sinDec = 0.39782 * Math.Sin(0.01745 * xlRise);
            cosDec = Math.Sqrt(1 - sinDec * sinDec);
            hRise = (cosZen - sinDec * sinLat) / (cosDec * cosLat);
            ahrSet = aSet / 15;
            sinDec = 0.39782 * Math.Sin(0.01745 * xlSet);
            cosDec = Math.Sqrt(1 - sinDec * sinDec);
            hSet = (cosZen - sinDec * sinLat) / (cosDec * cosLat);
            if (Math.Abs(hRise) <= 1)
            {
                hRise = 57.29578 * Math.Acos(hRise);
                utRise = ((360 - hRise) / 15) + ahrRise + Adj(tRise) + lonHour;
                sunrise = TimeAdj(utRise + location.TimeZone, date, location);
                while (sunrise.Hour > 12)
                {
                    sunrise.Hour -= 12;
                }
            }

            if (Math.Abs(hSet) <= 1)
            {
                hSet = 57.29578 * Math.Acos(hSet);
                utSet = (hRise / 15) + ahrSet + Adj(tSet) + lonHour;
                sunset = TimeAdj(utSet + location.TimeZone, date, location);
                while (sunset.Hour < 12)
                {
                    sunset.Hour += 12;
                }
            }

            return new HourMinute[] { sunrise, sunset };
        }

        private static double Adj(double x)
        {
            return (-0.06571 * x - 6.62);
        }

        private static double DegToDec(double deg, double min)
        {
            return (deg + min / 60);
        }

        private static int GetDayOfYear(DateTime date)
        {
            int[] monCount = { 0, 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
            if ((date.Month > 2) && (IsSecularLeapYear(date.Year)))
            {
                return monCount[date.Month] + date.Day + 1;
            }
            else
            {
                return monCount[date.Month] + date.Day;
            }
        }

        private static bool IsSecularLeapYear(int year)
        {
            if (year % 400 == 0)
            {
                return true;
            }
            if (year % 100 != 0)
            {
                if (year % 4 == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static double L(double x)
        {
            return (x + 1.916 * Math.Sin(0.01745 * x) + 0.02 * Math.Sin(2 * 0.01745 * x) + 282.565);
        }

        private static double M(double x)
        {
            return (0.9856 * x - 3.251);
        }

        private static double RadToDeg(double rad)
        {
            return 57.29578 * rad;
        }

        private static HourMinute TimeAdj(double time, DateTime date, Location location)
        {
            int hour, min;

            if (time < 0)
            {
                time += 24;
            }

            hour = (int)(Math.Truncate(Math.Floor(time)));
            min = (int)(Math.Truncate(Math.Floor((time - hour) * 60d + 0.5)));

            if (min >= 60)
            {
                hour += 1;
                min -= 60;
            }

            if (hour > 24)
            {
                hour -= 24;
            }

            HourMinute hm = new HourMinute { Hour = hour, Minute = min };

            if (Utils.IsDateTimeDST(date.Date.AddHours(hour), location))
            {
                hm += 60;
            }

            return hm;
        }

        #endregion Astronomical Calculations
    }
}