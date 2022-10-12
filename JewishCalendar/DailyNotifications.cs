using System;
using System.Collections.Generic;
using System.Linq;

namespace JewishCalendar
{
    /// <summary>
    /// Gets a list of daily notifications for home and Shul
    /// </summary>
    public class DailyNotifications
    {
        private readonly List<string> dayNotes = new List<string>();
        private readonly List<string> tefillahNotes = new List<string>();
        private readonly Location location;
        private readonly bool israel;
        private readonly TimeSpan time = DateTime.Now.TimeOfDay;
        private readonly JewishDate jdate;
        private readonly int month;
        private readonly int day;
        private readonly DayOfWeek dow;
        private readonly TimeOfDay chatzosHayom;
        private readonly TimeOfDay chatzosHalayla;
        private readonly TimeOfDay alos;
        private readonly TimeOfDay shkia;
        private readonly bool isAfterChatzosHayom;
        private readonly bool isAfterChatzosHalayla;
        private readonly bool isAfterAlos;
        private readonly bool isAfterShkia;
        private readonly bool isDaytime;
        private readonly bool isNightTime;
        private readonly bool isMorning;
        private readonly bool isAfternoon;
        private readonly bool isYomTov;
        private readonly bool isLeapYear;
        private bool noTachnun;
        private readonly bool showGaonShirShelYom;
        private readonly Parsha[] sedras;

        private bool showEnglish;
        private bool ignoreTime;


        /// <summary>
        /// Create a new DailyNotifications object for the given date and location
        /// </summary>
        /// <param name="dailyZmanim"></param>
        public DailyNotifications(DailyZmanim dailyZmanim)
        {
            this.location = dailyZmanim.Location;
            this.israel = this.location.IsInIsrael;
            this.time = DateTime.Now.TimeOfDay;
            this.jdate = dailyZmanim.JewishDate;
            this.month = this.jdate.Month;
            this.day = this.jdate.Day;
            this.dow = this.jdate.DayOfWeek;
            this.chatzosHayom = dailyZmanim.Chatzos;
            this.chatzosHalayla = this.chatzosHayom + 720;
            this.alos = dailyZmanim.NetzMishor - 90;
            this.shkia = dailyZmanim.ShkiaAtElevation;
            this.isAfterChatzosHayom = this.chatzosHayom <= this.time;
            this.isAfterChatzosHalayla = this.chatzosHalayla > this.time ||
                (this.chatzosHalayla.Hour > 12 && this.time.Hours < 12); //Chatzos is before 0:00 (eg 23:58) and time is after 0:00
            this.isAfterAlos = this.alos <= this.time;
            this.isAfterShkia = this.shkia <= this.time;
            this.isDaytime = (this.isAfterAlos || this.ignoreTime) && !this.isAfterShkia;
            this.isNightTime = !this.isDaytime;
            this.isMorning = (this.isDaytime || this.ignoreTime) && !this.isAfterChatzosHayom;
            this.isAfternoon = (this.isDaytime || this.ignoreTime) && (this.isAfterChatzosHayom || this.ignoreTime);
            this.isYomTov = this.IsYomTovOrCholHamoed();
            this.isLeapYear = JewishDateCalculations.IsJewishLeapYear(this.jdate.Year);
            this.noTachnun = (this.isAfternoon || this.ignoreTime) && (this.dow == DayOfWeek.Friday || this.day == 29);
            this.showGaonShirShelYom = this.israel;
            this.sedras = Sedra.GetSedra(this.jdate, this.israel);
        }

        /// <summary>
        /// Get the day and tefillah notifications for the given Jewish date and location
        /// </summary>
        /// <param name="dailyZmanim"></param>
        /// <param name="english"></param>
        /// <param name="ignoreTime"></param>
        /// <returns></returns>
        public static (string[] DayNotes, string[] TefillahNotes) GetNotifications(DailyZmanim dailyZmanim, bool english, bool ignoreTime)
        {
            return new DailyNotifications(dailyZmanim).GetNotifications(english, ignoreTime);
        }

        /// <summary>
        /// Get the day and tefillah notifications for this Jewish date and location
        /// </summary>
        /// <param name="english"></param>
        /// <param name="ignoreTime"></param>
        /// <returns></returns>
        public (string[] DayNotes, string[] TefillahNotes) GetNotifications(bool english, bool ignoreTime)
        {
            this.dayNotes.Clear();
            this.tefillahNotes.Clear();

            this.showEnglish = english;
            this.ignoreTime = ignoreTime;

            if (this.dow == DayOfWeek.Saturday)
            {
                this.GetShabbosNotifications();
            }
            else
            {
                this.GetWeekDayNotifications();
            }
            this.GetAroundTheYearNotifications();

            if ((this.noTachnun || this.ignoreTime) && (this.isDaytime || this.ignoreTime) && !this.isYomTov)
            {
                if (this.dow != DayOfWeek.Saturday)
                {
                    this.AddTefillahNote("No Tachnun", "א\"א תחנון");
                }
                else if (this.isAfternoon || this.ignoreTime)
                {
                    this.AddTefillahNote("No Tzidkascha", "א\"א צדקתך");
                }
                else if (
                  !(
                      (this.month == 1 && this.day > 21) ||
                      this.month == 2 ||
                      (this.month == 3 && this.day < 6)
                  )
              )
                {
                    this.AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                }
            }


            //return only unique values
            return (DayNotes: this.dayNotes.Distinct().ToArray(),
                    TefillahNotes: this.tefillahNotes.Distinct().ToArray());

        }

        private bool IsYomTovOrCholHamoed()
        {
            return (
                this.IsYomTov() ||
                (this.month == 1 && (new[] { 16, 17, 18, 19, 20 }).Any(d => d == this.day)) ||
                (this.month == 7 && (new[] { 16, 17, 18, 19, 20, 21 }).Any(d => d == this.day))
            );
        }

        /**
         * Returns true if this day is yomtov
         */
        private bool IsYomTov()
        {
            switch (this.month)
            {
                case 1:
                    if (this.day == 15 || this.day == 21) return true;
                    if (!this.israel && (this.day == 16 || this.day == 22)) return true;
                    break;
                case 3:
                    if (this.day == 6 || (!this.israel && this.day == 7)) return true;
                    break;
                case 7:
                    if ((new[] { 1, 2, 10, 15, 22 }).Any(d => d == this.day))
                    {
                        return true;
                    }
                    if (!this.israel && (this.day == 16 || this.day == 23)) return true;
                    break;
            }
            return false;
        }

        /**Is the current Jewish Date the day before a yomtov that contains a Friday?*/
        private bool HasEiruvTavshilin()
        {
            return (
                //Eiruv Tavshilin is only on Wednesday or Thursday
                (this.dow == DayOfWeek.Thursday || this.dow == DayOfWeek.Wednesday) &&
                //today is Erev Yomtov
                this.IsErevYomTov() &&
                //Thursday OR Wednesday when in Chu"l or Erev Rosh Hashana anywhere
                (this.dow == DayOfWeek.Thursday || (this.dow == DayOfWeek.Wednesday && (!this.israel || this.jdate.Month == 6))) &&
                //No Eiruv Tavshilin on Erev yom kippur
                this.jdate.Day != 9
            );
        }


        /**Is today Erev Yom Tov? (includes Erev second days of Sukkos and Pesach) */
        private bool IsErevYomTov()
        {
            return (
                (this.month == 1 && (this.day == 14 || this.day == 20)) ||
                (this.month == 3 && this.day == 5) ||
                (this.month == 6 && this.day == 29) ||
                ((this.month == 7 && (this.day == 9 || this.day == 14 || this.day == 21)))
            );
        }


        private void GetShabbosNotifications()
        {
            if (this.month == 1 && this.day > 7 && this.day < 15)
            {
                this.AddDayNote("Shabbos Hagadol", "שבת הגדול");
            }
            else if (this.month == 7 && this.day > 2 && this.day < 10)
            {
                this.AddDayNote("Shabbos Shuva", "שבת שובה");
            }
            else if (this.month == 5 && this.day > 2 && this.day < 10)
            {
                this.AddDayNote("Shabbos Chazon", "שבת חזון");
            }
            else if ((this.month == (this.isLeapYear ? 12 : 11) && this.day > 24) ||
              (this.month == (this.isLeapYear ? 13 : 12) && this.day == 1))
            {
                this.AddDayNote("Parshas Shkalim", "פרשת שקלים");
            }
            else if (this.month == (this.isLeapYear ? 13 : 12) && this.day > 7 && this.day < 14)
            {
                this.AddDayNote("Parshas Zachor", "פרשת זכור");
            }
            else if (this.month == (this.isLeapYear ? 13 : 12) && this.day > 16 && this.day < 24)
            {
                this.AddDayNote("Parshas Parah", "פרשת פרה");
            }
            else if (
              (this.month == (this.isLeapYear ? 13 : 12) && this.day > 23 && this.day < 30) ||
              (this.month == 1 && this.day == 1)
          )
            {
                this.AddDayNote("Parshas Hachodesh", "פרשת החודש");
            }
            if ((this.isMorning || this.ignoreTime) && !this.isYomTov)
            {
                if (this.sedras.Length > 0)
                {
                    this.AddTefillahNote(
                        $"Kriyas Hatorah Parshas {string.Join(" - ", this.sedras.Select(i => i.nameEng))}",
                        $"קה\"ת פרשת { string.Join(" - ", this.sedras.Select(i => i.nameHebrew))}");
                }
                //All months but Tishrei have Shabbos Mevarchim on the Shabbos before Rosh Chodesh
                if (this.month != 6 && this.day > 22 && this.day < 30)
                {
                    var nextMonth = this.jdate.AddMonths(1);
                    this.AddTefillahNote(
                        "The molad will be " +
                            Molad.GetMolad(nextMonth.Year, nextMonth.Month).ToString(),
                        "המולד יהיה ב" +
                            Molad.GetMolad(nextMonth.Year, nextMonth.Month).ToStringHeb(this.shkia));
                    this.AddTefillahNote("Bircas Hachodesh", "מברכים החודש");
                    if (this.month != 1 && this.month != 2)
                    {
                        this.AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                    }
                }
            }

            //Rosh Chodesh
            if (this.month != 7 && (this.day == 1 || this.day == 30))
            {
                this.AddDayNote("Rosh Chodesh", "ראש חודש");
                this.AddTefillahNote("Ya`aleh Viyavo", "יעלה ויבא");
                if ((this.showGaonShirShelYom) && (this.isDaytime || this.ignoreTime))
                {
                    this.AddTefillahNote("Barchi Nafshi", "שיר של יום - קי\"ד - ברכי נפשי");
                }
                //Rosh Chodesh Teves is during Chanuka
                if ((this.isDaytime || this.ignoreTime) &&
                    this.month != 10 && !(this.month == 9 && this.day == 30))
                {
                    this.AddTefillahNote("Chatzi Hallel", "חצי הלל");
                }
                this.AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
            }
            else if (this.isYomTov || this.ignoreTime)
            {
                this.AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                if ((this.showGaonShirShelYom) && (this.isDaytime || this.ignoreTime))
                {
                    this.AddTefillahNote("שיר של יום - מזמור שיר ליום השבת");
                }
            }
            //Kriyas Hatora - Shabbos by mincha - besides for Yom Kippur
            if ((this.isAfternoon || this.ignoreTime) && !(this.month == 7 && this.day == 10))
            {
                var sedra = Sedra.GetSedra(this.jdate + 1, this.israel);
                this.AddTefillahNote(
                    "Kriyas Hatorah Mincha Parshas " + sedra[0].nameEng,
                    "קה\"ת במנחה פרשת " + sedra[0].nameHebrew);
            }
            if (
                (this.isAfternoon || this.ignoreTime) &&
                ((this.month == 1 && this.day > 21) ||
                    (this.month <= 6 && !(this.month == 5 && (this.day == 8 || this.day == 9))))
            )
            {
                var prakim = PirkeiAvos.GetPirkeiAvos(this.jdate, this.israel);
                if (prakim.Length > 0)
                {
                    this.AddDayNote(
                        "Pirkei Avos - " + string.Join(" and ",
                            prakim.Select(s => $"Perek { Utils.ToNumberHeb(s)}")),
                        "פרקי אבות - " + string.Join(" ו ",
                            prakim.Select(s => $"פרק { Utils.ToNumberHeb(s)}")));
                }
            }
        }

        private void GetWeekDayNotifications()
        {
            //מוצאי שבת
            if ((this.isNightTime || this.ignoreTime) && this.dow == DayOfWeek.Sunday)
            {
                //הבדלה בתפילה for מוצאי שבת
                this.AddTefillahNote(
                    (this.month == 1 && this.day == 15) || (this.month == 3 && this.day == 6)
                        ? "ותודיעינו"
                        : "אתה חוננתנו");
                //Motzai Shabbos before Yom Tov - no ויהי נועם
                if (
                    (this.month == 6 && this.day > 22) ||
                    (this.month == 7 && this.day < 22 && this.day != 3) ||
                    (this.month == 1 && this.day > 8 && this.day < 15) ||
                    (this.month == 3 && this.day < 6)
                )
                {
                    this.AddTefillahNote("No Vihi Noam", "א\"א ויהי נועם");
                }
            }
            //אתה חוננתנו for מוצאי יו"ט
            else if (
                (this.isNightTime || this.ignoreTime) &&
                ((this.month == 1 && (this.day == 16 || this.day == 22)) ||
                    (this.month == 3 && this.day == 7) ||
                    (this.month == 7 && ((new[] { 3, 11, 16, 23 }).Any(d => d == this.day))))
            )
            {
                this.AddTefillahNote("Ata Chonantanu", "אתה חוננתנו");
            }
            //Kriyas hatorah for Monday and Thursday
            //when it"s not chol hamoed, chanuka, purim, a fast day or rosh chodesh
            if (
                (this.isMorning || this.ignoreTime) &&
                !this.isYomTov &&
                (this.dow == DayOfWeek.Monday || this.dow == DayOfWeek.Thursday) &&
                !this.HasOwnKriyasHatorah()
            )
            {
                if (this.sedras.Length > 0)
                {
                    this.AddTefillahNote(
                        $"Kriyas Hatorah Parshas {this.sedras[0].nameEng}",
                        $"קה\"ת פרשת {this.sedras[0].nameHebrew}");
                }
            }
            //Rosh Chodesh
            if ((this.month != 7 && this.day == 1) || this.day == 30)
            {
                this.noTachnun = true;
                this.AddDayNote("Rosh Chodesh", "ראש חודש");
                this.AddTefillahNote("Ya`aleh Viyavo", "יעלה ויבא");
                if ((this.showGaonShirShelYom) && (this.isDaytime || this.ignoreTime))
                {
                    this.AddTefillahNote("Barchi Nafshi", "שיר של יום - קי\"ד - ברכי נפשי");
                }
                //Rosh Chodesh Teves is during Chanuka
                if ((this.isDaytime || this.ignoreTime) && this.month != 10 && !(this.month == 9 && this.day == 30))
                {
                    this.AddTefillahNote("Chatzi Hallel", "חצי הלל");
                    if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                    {
                        this.NoLaminatzeach();
                    }
                }
            }
            //Yom Kippur Kattan
            else if (
                this.month != 6 &&
                ((this.dow < DayOfWeek.Friday && this.day == 29) ||
                    (this.dow == DayOfWeek.Thursday && this.day == 28)) &&
                (this.isAfternoon || this.ignoreTime)
            )
            {
                this.AddTefillahNote("Yom Kippur Kattan", "יו\"כ קטן");
            }
            if (this.HasEiruvTavshilin())
            {
                this.AddDayNote("Eruv Tavshilin", "עירוב תבשילין");
            }
        }

        private void GetAroundTheYearNotifications()
        {
            switch (this.month)
            {
                case 1: //Nissan
                    this.noTachnun = true;
                    if (this.day > 15)
                    {
                        this.AddTefillahNote("Morid Hatal", "מוריד הטל");
                    }
                    if (this.dow != DayOfWeek.Saturday && this.day > 15 && this.day != 21)
                    {
                        this.AddTefillahNote("Vesain Bracha", "ותן ברכה");
                    }
                    if (
                        (this.isMorning || this.ignoreTime) &&
                        this.dow != DayOfWeek.Saturday &&
                        (new[] { 14, 16, 17, 18, 19, 20 }).Any(d => d == this.day)
                    )
                    {
                        this.AddTefillahNote("No Mizmor Lesodah", "א\"א מזמור לתודה");
                        if (this.dow != DayOfWeek.Saturday)
                        {
                            this.NoLaminatzeach();
                        }
                    }
                    if (this.day == 15)
                    {
                        this.AddDayNote("First Day of Pesach", "יו\"ט ראשון של פסח");
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        this.AddTefillahNote("Full Hallel", "הלל השלם");
                        if (this.isAfternoon || this.ignoreTime)
                        {
                            this.AddTefillahNote("Morid Hatal", "מוריד הטל");
                        }
                        if (
                            (this.showGaonShirShelYom) &&
                            (this.isDaytime || this.ignoreTime) &&
                            this.dow != DayOfWeek.Saturday
                        )
                        {
                            this.AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                        }
                    }
                    else if (this.day == 16 && !this.israel)
                    {
                        this.AddDayNote("Second Day of Pesach", "יו\"ט שני של פסח");
                        this.AddTefillahNote("Full Hallel", "הלל השלם");
                        this.AddTefillahNote("Morid Hatal", "מוריד הטל");
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        if (
                            (this.showGaonShirShelYom) &&
                            (this.isDaytime || this.ignoreTime) &&
                            this.dow != DayOfWeek.Saturday
                        )
                        {
                            this.AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                        }
                    }
                    if ((new[] { 16, 17, 18, 19, 20, 21 }).Any(d => d == this.day))
                    {
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        if (this.day == 21)
                        {
                            this.AddDayNote("Shvi`i Shel Pesach", "שביעי של פםח");
                            if (this.isDaytime || this.ignoreTime)
                            {
                                if (this.israel || this.ignoreTime)
                                {
                                    this.AddTefillahNote("Yizkor", "יזכור");
                                }
                                if ((this.showGaonShirShelYom) && this.dow != DayOfWeek.Saturday)
                                {
                                    this.AddTefillahNote(
                                        "שיר של יום - י\"ח - למנצח לעבד ה\"");
                                }
                            }
                        }
                        else
                        {
                            if (this.israel || this.day != 16)
                            {
                                this.AddDayNote("Chol Ha`moed Pesach", "פסח - חול המועד");
                            }
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                                this.NoLaminatzeach();
                            if (
                                this.showGaonShirShelYom &&
                                (this.isDaytime || this.ignoreTime) &&
                                this.dow != DayOfWeek.Saturday
                            )
                            {
                                switch (this.day)
                                {
                                    case 16:
                                        if (this.dow == DayOfWeek.Sunday)
                                        {
                                            this.AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                                        }
                                        else
                                        {
                                            this.AddTefillahNote("שיר של יום - ע\"ח - משכיל לאסף");
                                        }
                                        break;
                                    case 17:
                                        if (this.dow == DayOfWeek.Monday)
                                        {
                                            this.AddTefillahNote("שיר של יום - ע\"ח - משכיל לאסף");
                                        }
                                        else
                                        {
                                            this.AddTefillahNote("שיר של יום - פ\" - למנצח אל שושנים");
                                        }
                                        break;
                                    case 18:
                                        if (
                                            this.dow == DayOfWeek.Tuesday ||
                                            this.dow == DayOfWeek.Sunday
                                        )
                                        {
                                            this.AddTefillahNote("שיר של יום - פ\" - למנצח אל שושנים");
                                        }
                                        else
                                        {
                                            this.AddTefillahNote("שיר של יום - ק\"ה - הודו לה\"");
                                        }
                                        break;
                                    case 19:
                                        if (this.dow == DayOfWeek.Thursday)
                                        {
                                            this.AddTefillahNote("שיר של יום - קל\"ה - הללוי - ה הללו את שם");
                                        }
                                        else
                                        {
                                            this.AddTefillahNote("שיר של יום - ק\"ה - הודו לה\"");
                                        }
                                        break;
                                    case 20:
                                        if (this.dow == DayOfWeek.Friday)
                                        {
                                            this.AddTefillahNote("שיר של יום - ס\"ו - למנצח שיר מזמור");
                                        }
                                        else
                                        {
                                            this.AddTefillahNote("שיר של יום - קל\"ה - הללוי - ה הללו את שם");
                                        }
                                        break;
                                }
                            }
                        }
                        if (this.isDaytime || this.ignoreTime) this.AddTefillahNote("Half Hallel", "חצי הלל");
                    }
                    if (this.day == 22)
                    {
                        if (this.israel || this.ignoreTime)
                        {
                            this.AddDayNote("Isru Chag", "איסרו חג");
                        }
                        else
                        {
                            this.AddDayNote("Acharon Shel Pesach", "אחרון של פסח");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Yizkor", "יזכור");
                                this.AddTefillahNote("Half Hallel", "חצי הלל");
                            }
                        }
                        if (this.dow != DayOfWeek.Saturday && (this.isMorning || this.ignoreTime))
                        {
                            this.NoLaminatzeach();
                        }
                    }
                    else if (
                      this.dow == DayOfWeek.Saturday &&
                      ((new[] { 15, 16, 17, 18, 19, 20, 21 }).Any(d => d == this.day) ||
                          (!this.israel && this.day == 22))
                  )
                    {
                        this.AddTefillahNote("Shir Hashirim", "מגילת שיר השירים");
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                    }
                    break;
                case 2: //Iyar
                    if (this.day <= 15)
                    {
                        this.AddTefillahNote("Morid Hatal", "מוריד הטל");
                        if (this.dow != DayOfWeek.Saturday)
                        {
                            this.AddTefillahNote("V`sain Bracha", "ותן ברכה");
                        }
                    }
                    //Pesach Sheini and Lag Ba"Omer
                    if (
                        this.day == 14 ||
                        (this.day == 13 && (this.isAfternoon || this.ignoreTime)) ||
                        this.day == 18 ||
                        (this.day == 17 && (this.isAfternoon || this.ignoreTime))
                    )
                    {
                        this.noTachnun = true;
                        if (this.day == 14)
                        {
                            this.AddDayNote("Pesach Sheini", "פסח שני");
                        }
                    }
                    //Baha"b
                    if (
                        (this.isMorning || this.ignoreTime) &&
                        ((this.dow == DayOfWeek.Monday && this.day > 3 && this.day < 13) ||
                            (this.dow == DayOfWeek.Thursday && this.day > 6 && this.day < 14) ||
                            (this.dow == DayOfWeek.Monday &&
                                this.day > 10 &&
                                this.day < 18 &&
                                this.day != 14))
                    )
                    {
                        this.AddTefillahNote("Ba`hab", "סליחות בה\"ב");
                        this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                    }
                    break;
                case 3: //Sivan
                    if (this.day < 13)
                    {
                        this.noTachnun = true;
                    }
                    if (this.day == 6)
                    {
                        this.AddDayNote("Shavuos", "יום טוב של שבועות");
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        if (this.isDaytime || this.ignoreTime)
                        {
                            this.AddTefillahNote("Full Hallel", "הלל השלם");
                            this.AddTefillahNote("Megilas Rus", "מגילת רות");
                            this.AddTefillahNote("Akdamus", "אקדמות");
                            if (this.israel || this.ignoreTime) this.AddTefillahNote("Yizkor", "יזכור");
                            if (this.showGaonShirShelYom)
                            {
                                this.AddTefillahNote("שיר של יום - י\"ט - ..השמים מספרים..");
                            }
                        }
                    }
                    else if (this.day == 7)
                    {
                        if (this.israel || this.ignoreTime)
                        {
                            this.AddDayNote("Issru Chag", "איסרו חג");
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                            {
                                this.NoLaminatzeach();
                            }
                        }
                        else
                        {
                            this.AddDayNote("Shavuos Second Day", "יום טוב של שבועות");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            this.AddTefillahNote("Full Hallel", "הלל השלם");
                            this.AddTefillahNote("Yizkor", "יזכור");
                        }
                    }
                    break;
                case 4: //Tammuz
                    if (
                        (this.isDaytime || this.ignoreTime) &&
                        ((this.day == 17 && this.dow != DayOfWeek.Saturday) ||
                            (this.day == 18 && this.dow == DayOfWeek.Sunday))
                    )
                    {
                        if (this.isDaytime || this.ignoreTime)
                        {
                            this.AddDayNote("Shiva Asar B`Tamuz", "י\"ז בתמוז");
                            this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                            this.AddTefillahNote("Aneinu", "עננו");
                        }
                        if (this.isMorning || this.ignoreTime)
                        {
                            this.AddTefillahNote("Selichos", "סליחות");
                        }
                    }
                    break;
                case 5: //Av
                    if ((this.isAfternoon || this.ignoreTime) && this.day == 8 && this.dow != DayOfWeek.Friday)
                    {
                        this.noTachnun = true;
                    }
                    else if (
                      (this.day == 9 && this.dow != DayOfWeek.Saturday) ||
                      (this.day == 10 && this.dow == DayOfWeek.Sunday)
                  )
                    {
                        this.AddDayNote("Tish B`Av", "תשעה באב");
                        if (this.isDaytime || this.ignoreTime)
                        {
                            this.AddTefillahNote("Kinos", "קינות");
                            this.AddTefillahNote("Aneinu", "עננו");
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                            {
                                this.NoLaminatzeach();
                            }
                        }
                        else
                        {
                            this.AddTefillahNote("Megilas Eicha", "מגילת איכה");
                            if ((this.isNightTime || this.ignoreTime) && this.dow == DayOfWeek.Sunday)
                            {
                                this.AddTefillahNote("No Vihi Noam", "א\"א ויהי נועם");
                            }
                        }
                        this.noTachnun = true;
                    }
                    else if ((this.isAfternoon || this.ignoreTime) && this.day == 14)
                    {
                        this.noTachnun = true;
                    }
                    else if (this.day == 15)
                    {
                        this.AddDayNote("Tu B`Av", "ט\"ו באב");
                        this.noTachnun = true;
                    }
                    break;
                case 6: //Ellul
                    this.AddTefillahNote("L`Dovid Hashem Ori", "לדוד ה");
                    if (
                        this.day > 20 &&
                        this.dow != DayOfWeek.Saturday &&
                        (this.isAfterChatzosHalayla || this.isMorning || this.ignoreTime)
                    )
                    {
                        var startedSelichos = this.day >= 26;
                        if (this.day < 26)
                        {
                            var daysToRH = 30 - this.day;
                            var dowRH = (DayOfWeek)((daysToRH + (int)this.dow) % 7);
                            switch (dowRH)
                            {
                                case DayOfWeek.Monday:
                                    startedSelichos = this.day >= 22;
                                    break;
                                case DayOfWeek.Tuesday:
                                    startedSelichos = this.day >= 21;
                                    break;
                                case DayOfWeek.Saturday:
                                    startedSelichos = this.day >= 24;
                                    break;
                            }
                        }
                        if (startedSelichos)
                        {
                            this.AddTefillahNote("Selichos", "סליחות");
                        }
                    }
                    if (this.day == 29)
                    {
                        this.noTachnun = true;
                    }
                    break;
                case 7: //Tishrei
                    if (this.day < 11)
                    {
                        this.AddTefillahNote("Hamelech Hakadosh", "המלך הקדוש");
                        if (this.dow != DayOfWeek.Saturday && this.day != 9)
                        {
                            this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                        }
                    }
                    //Days of Rosh Hashana, Tzom Gedaliah and Yom Kippur are dealt with individually below.
                    if (this.day > 4 && this.day < 10 && this.dow != DayOfWeek.Saturday)
                    {
                        this.AddTefillahNote("Selichos", "סליחות");
                        this.AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                    }
                    if (this.dow == DayOfWeek.Saturday && this.day > 2 && this.day < 10)
                    {
                        this.AddDayNote("Shabbos Shuva", "שבת שובה");
                    }
                    if (this.day >= 10)
                    {
                        this.noTachnun = true;
                    }
                    switch (this.day)
                    {
                        case 1:
                            this.AddDayNote("Rosh Hashana", "ראש השנה");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.dow != DayOfWeek.Saturday && (this.isDaytime || this.ignoreTime))
                            {
                                this.AddTefillahNote("Tekias Shofar", "תקיעת שופר");
                                if (this.showGaonShirShelYom)
                                {
                                    this.AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                }
                                if (this.isAfternoon || this.ignoreTime)
                                {
                                    this.AddDayNote("Tashlich", "תשליך");
                                }
                            }
                            break;
                        case 2:
                            this.AddDayNote("Rosh Hashana", "ראש השנה");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Tekias Shofar", "תקיעת שופר");
                                if (this.showGaonShirShelYom)
                                {
                                    this.AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                }
                                if (this.dow == DayOfWeek.Sunday && (this.isAfternoon || this.ignoreTime))
                                {
                                    this.AddDayNote("Tashlich", "תשליך");
                                }
                            }
                            break;
                        case 3:
                            if (this.dow != DayOfWeek.Saturday)
                            {
                                if (this.isDaytime || this.ignoreTime)
                                {
                                    this.AddDayNote("Fast of Tzom Gedalya", "צום גדליה");
                                    this.AddTefillahNote("Aneinu", "עננו");
                                }
                                if (this.isAfterChatzosHalayla || this.isMorning || this.ignoreTime)
                                {
                                    this.AddTefillahNote("Selichos", "סליחות");
                                }
                                this.AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                            }
                            break;
                        case 4:
                            if (this.dow == DayOfWeek.Sunday)
                            {
                                if (this.isDaytime || this.ignoreTime)
                                {
                                    this.AddDayNote("Fast of Tzom Gedalya", "צום גדליה");
                                    this.AddTefillahNote("Aneinu", "עננו");
                                }
                                if (this.isAfterChatzosHalayla || this.isMorning || this.ignoreTime)
                                {
                                    this.AddTefillahNote("Selichos", "סליחות");
                                }
                                this.AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                            }
                            else if (this.dow != DayOfWeek.Saturday)
                            {
                                this.AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                                if (this.isAfterChatzosHalayla || this.isMorning || this.ignoreTime)
                                {
                                    this.AddTefillahNote("Selichos", "סליחות");
                                }
                            }
                            break;
                        case 9:
                            this.AddDayNote("Erev Yom Kippur", "ערב יום כיפור");
                            if (this.isMorning || this.ignoreTime)
                            {
                                this.AddTefillahNote("No Mizmor L`Sodah", "א\"א מזמור לתודה");
                                if (this.dow != DayOfWeek.Saturday)
                                {
                                    this.NoLaminatzeach();
                                }
                                if (this.dow == DayOfWeek.Friday)
                                {
                                    this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                                }
                            }
                            else if (this.isAfternoon || this.ignoreTime)
                            {
                                this.AddTefillahNote("Vidduy", "ודוי בעמידה");
                            }
                            if ((this.isDaytime || this.ignoreTime) && this.dow != DayOfWeek.Friday)
                            {
                                this.AddTefillahNote("No Avinu Malkeinu", "א\"א אבינו מלכנו");
                            }
                            this.noTachnun = true;
                            break;
                        case 10:
                            this.AddDayNote("Yom Kippur", "יום הכיפורים");
                            this.AddDayNote("לפני ה\" תטהרו");
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Yizkor", "יזכור");
                                if ((this.showGaonShirShelYom) && this.dow != DayOfWeek.Saturday)
                                {
                                    this.AddTefillahNote("שיר של יום - ל\"ב - לדוד משכיל");
                                }
                            }
                            if (this.isAfternoon || this.ignoreTime)
                            {
                                //only Yom Kippur has its own Kriyas Hatorah
                                this.AddTefillahNote("קה\"ת במנחה סוף פרשת אח\"מ");
                            }
                            break;
                        case 15:
                            this.AddDayNote("First day of Sukkos", "יו\"ט ראשון של סוכות");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Full Hallel", "הלל השלם");
                                if (this.dow != DayOfWeek.Saturday)
                                {
                                    this.AddTefillahNote("Hoshanos - למען אמתך", "הושענות - למען אמתך");
                                    this.AddTefillahNote("Kah Keli", "קה קלי");
                                    if (this.showGaonShirShelYom)
                                    {
                                        this.AddTefillahNote("שיר של יום - ע\"ו - למנצח בנגינות מזמור");
                                    }
                                }
                                else
                                {
                                    this.AddTefillahNote("Hoshanos - אום נצורה", "הושענות - אום נצורה");
                                }
                            }
                            break;
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.day == 16 && !this.israel)
                            {
                                this.AddDayNote(
                                    "Second day of Sukkos",
                                    "סוכות - יום טוב שני"


                                );
                            }
                            else if (!this.israel)
                            {
                                this.AddDayNote("Chol Hamoed Sukkos", "סוכות - חול המועד");
                                this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            }
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Full Hallel", "הלל השלם");
                                switch (this.day)
                                {
                                    case 16:
                                        this.AddTefillahNote(
                                            "הושענות - " +
                                                (this.dow == DayOfWeek.Sunday
                                                    ? "למען אמתך"
                                                    : "אבן שתיה"));
                                        if (
                                            this.showGaonShirShelYom &&
                                            this.dow != DayOfWeek.Saturday
                                        )
                                        {
                                            this.AddTefillahNote("שיר של יום - כ\"ט - ..הבו לה\" בני אלים");
                                        }
                                        break;
                                    case 17:
                                        this.AddTefillahNote(
                                            (this.showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (this.dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "אערוך שועי"));
                                        if (
                                            this.showGaonShirShelYom &&
                                            this.dow != DayOfWeek.Saturday
                                        )
                                        {
                                            this.AddTefillahNote("שיר של יום - נ\" - מזמור לאסף");
                                        }
                                        break;
                                    case 18:
                                        if (this.dow == DayOfWeek.Sunday)
                                        {
                                            this.AddTefillahNote("Hoshanos", "הושענות");
                                            if (
                                                this.showGaonShirShelYom &&
                                                this.dow != DayOfWeek.Saturday
                                            )
                                            {
                                                this.AddTefillahNote("שיר של יום - נ\" - מזמור לאסף");
                                            }
                                        }
                                        else
                                        {
                                            if (this.dow == DayOfWeek.Tuesday)
                                            {
                                                this.AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            else if (this.dow == DayOfWeek.Thursday)
                                            {
                                                this.AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            else if (this.dow == DayOfWeek.Friday)
                                            {
                                                this.AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            if (
                                                this.showGaonShirShelYom &&
                                                this.dow != DayOfWeek.Saturday
                                            )
                                            {
                                                this.AddTefillahNote("שיר של יום - צ\"ד - ..מי יקום לי..");
                                            }
                                        }
                                        break;
                                    case 19:
                                        this.AddTefillahNote(
                                            (this.showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (this.dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "א-ל למושעות"));
                                        if (
                                            this.showGaonShirShelYom &&
                                            this.dow != DayOfWeek.Saturday
                                        )
                                        {
                                            if (this.dow == DayOfWeek.Monday)
                                            {
                                                this.AddTefillahNote("שיר של יום - צ\"ד - ..מי יקום לי..");
                                            }
                                            else
                                            {
                                                this.AddTefillahNote("שיר של יום - צ\"ד - א - ל נקמות..ישרי לב");
                                            }
                                        }
                                        break;
                                    case 20:
                                        this.AddTefillahNote(
                                            (this.showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (this.dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "אדון המושיע"));
                                        if (
                                            (this.showGaonShirShelYom) &&
                                            this.dow != DayOfWeek.Saturday
                                        )
                                        {
                                            if (this.dow == DayOfWeek.Thursday)
                                            {
                                                this.AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                            }
                                            else
                                            {
                                                this.AddTefillahNote("שיר של יום - צ\"ד - א - ל נקמות..ישרי לב");
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case 21:
                            this.AddDayNote("Hoshana Raba", "הושעה רבה");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.isNightTime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Mishneh Torah", "משנה תורה");
                            }
                            else
                            {
                                this.AddTefillahNote("Hoshanos", "הושענות");
                                this.AddTefillahNote("Full Hallel", "הלל השלם");
                                if (this.showGaonShirShelYom)
                                {
                                    if (this.dow == DayOfWeek.Friday)
                                    {
                                        this.AddTefillahNote("שיר של יום - פ\"ב - מזמור לאסף");
                                    }
                                    else
                                    {
                                        this.AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                    }
                                }
                            }
                            break;
                        case 22:
                            this.AddDayNote("Shmini Atzeres", "שמיני עצרת");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (this.israel || this.ignoreTime)
                            {
                                this.AddDayNote("Simchas Torah", "שמחת תורה");
                                this.AddTefillahNote("Hakafos", "הקפות");
                            }
                            if (this.isDaytime || this.ignoreTime)
                            {
                                this.AddTefillahNote("Full Hallel", "הלל השלם");
                                this.AddTefillahNote("Yizkor", "יזכור");
                                this.AddTefillahNote("Tefilas Geshem", "תפילת גשם");
                                this.AddTefillahNote("משיב הרוח ומוריד הגשם");
                                if ((this.showGaonShirShelYom) && this.dow != DayOfWeek.Saturday)
                                {
                                    this.AddTefillahNote("שיר של יום - י\"ב - למנצח על השמינית");
                                }
                            }
                            break;
                    }
                    if (this.day == 23)
                    {
                        if (!this.israel)
                        {
                            this.AddDayNote("Simchas Torah", "שמחת תורה");
                            this.AddTefillahNote("Hakafos", "הקפות");
                            this.AddTefillahNote("Full Hallel", "הלל השלם");
                            this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        }
                        else
                        {
                            this.AddDayNote("Issru Chag", "איסרו חג");
                            if (this.isNightTime || this.ignoreTime)
                            {
                                this.AddDayNote("א גוטען ווינטער", "חורף טוב");
                            }
                            else if (this.dow != DayOfWeek.Saturday && (this.isMorning || this.ignoreTime))
                            {
                                this.NoLaminatzeach();
                            }
                        }
                    }
                    else if (
                      this.dow == DayOfWeek.Saturday &&
                      (new[] { 15, 17, 18, 19, 20 }).Any(d => d == this.day))
                    {
                        this.AddTefillahNote("Megilas Koheles", "מגילת קהלת");
                        this.AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                    }
                    if (this.day < 22)
                    {
                        this.AddTefillahNote("L`Dovid Hashem Ori", "לדוד ה");
                    }
                    else if (this.day > 22)
                    {
                        this.AddTefillahNote(
                            "Mashiv Haruach U`Morid Hageshem",
                            "משיב הרוח ומוריד הגשם");
                    }
                    break;
                case 8: //Cheshvan
                    if (
                        (this.isDaytime || this.ignoreTime) &&
                        ((this.dow == DayOfWeek.Monday && this.day > 3 && this.day < 13) ||
                            (this.dow == DayOfWeek.Thursday && this.day > 6 && this.day < 14) ||
                            (this.dow == DayOfWeek.Monday && this.day > 10 && this.day < 18))
                    )
                    {
                        this.AddTefillahNote("Ba`Hab", "סליחות בה\"ב");
                        this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                    }
                    if (this.day <= 22)
                    {
                        this.AddTefillahNote(
                            "Mashiv Haruach U`Morid Hageshem",
                            "משיב הרוח ומוריד הגשם");
                    }
                    if (this.day >= 7 && this.dow != DayOfWeek.Saturday)
                    {
                        this.AddTefillahNote("V`sain Tal U`matar", "ותן טל ומטר");
                    }
                    break;
                case 9: //Kislev
                    if (this.day <= 7 && this.dow != DayOfWeek.Saturday)
                    {
                        this.AddTefillahNote("V`sain Tal U`matar", "ותן טל ומטר");
                    }
                    else if (
                      this.day == 24 &&
                      this.dow != DayOfWeek.Saturday &&
                      (this.isAfternoon || this.ignoreTime)
                  )
                    {
                        this.noTachnun = true;
                    }
                    else if (this.day >= 25)
                    {
                        this.noTachnun = true;
                        this.AddDayNote("Chanukah", "חנוכה");
                        this.AddTefillahNote("Al Hanissim", "על הניסים");
                        if (this.isDaytime || this.ignoreTime)
                        {
                            this.AddTefillahNote("Full Hallel", "הלל השלם");
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                                this.NoLaminatzeach();
                            if (
                                (this.showGaonShirShelYom) &&
                                this.day != 30 &&
                                this.dow != DayOfWeek.Saturday
                            )
                            {
                                this.AddTefillahNote("שיר של יום - ל\" - מזמור שיר חנוכת הבית");
                            }
                        }
                    }
                    break;
                case 10: //Teves
                    if (this.day <= (JewishDateCalculations.IsShortKislev(this.jdate.Year) ? 3 : 2))
                    {
                        this.noTachnun = true;
                        this.AddDayNote("Chanukah", "חנוכה");
                        this.AddTefillahNote("Al Hanissim", "על הניסים");
                        if (this.isDaytime || this.ignoreTime)
                        {
                            this.AddTefillahNote("Full Hallel", "הלל השלם");
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                            {
                                this.NoLaminatzeach();
                                if (this.day != 1 && (this.showGaonShirShelYom))
                                {
                                    this.AddTefillahNote("שיר של יום - ל\" - מזמור שיר חנוכת הבית");
                                }
                            }
                        }
                    }
                    else if (this.day == 10 && (this.isDaytime || this.ignoreTime))
                    {
                        this.AddDayNote("Fast of Asara B`Teves", "עשרה בטבת");
                        if (this.isMorning || this.ignoreTime)
                        {
                            this.AddTefillahNote("Selichos", "סליחות");
                        }
                        this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                        this.AddTefillahNote("Aneinu", "עננו");
                    }
                    break;
                case 11: //Shvat
                    if (this.day == 14 && (this.isAfternoon || this.ignoreTime)) this.noTachnun = true;
                    if (this.day == 15)
                    {
                        this.noTachnun = true;
                        this.AddDayNote("Tu B`Shvat", "ט\"ו בשבט");
                    }
                    break;
                case 12:
                case 13:
                    if (this.month == 12 && (this.isLeapYear || this.ignoreTime))
                    {
                        //Adar Rishon in a leap year
                        if (
                            ((this.day == 13 && (this.isAfternoon || this.ignoreTime)) || (this.day == 14 || this.day == 14)) &&
                            (this.isDaytime || this.ignoreTime)
                        )
                        {
                            this.AddDayNote(
                                this.day == 14
                                    ? this.showEnglish
                                        ? "Purim Katan"
                                        : "פורים קטן"
                                    : this.showEnglish
                                    ? "Shushan Purim Katan"
                                    : "שושן פורים קטן"


                            );
                            this.noTachnun = true;
                            if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                            {
                                this.NoLaminatzeach();
                            }
                        }
                    }
                    else
                    {
                        //The "real" Adar: the only one in a non-leap-year or Adar Sheini
                        if (
                            (this.isDaytime || this.ignoreTime) &&
                            ((this.day == 11 && this.dow == DayOfWeek.Thursday) ||
                                (this.day == 13 && this.dow != DayOfWeek.Saturday))
                        )
                        {
                            if (this.isMorning || this.ignoreTime)
                            {
                                this.AddDayNote("Fast of Ta`anis Esther", "תענית אסתר");
                                this.AddTefillahNote("Selichos", "סליחות");
                            }
                            this.AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                            this.AddTefillahNote("Aneinu", "עננו");
                        }
                        else
                        {
                            //Only ירושלים says על הניסים on ט"ו
                            bool isYerushalayim = this.location.Name == "ירושלים";
                            if (this.day == 14)
                            {
                                this.noTachnun = true;
                                if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                                {
                                    this.NoLaminatzeach();
                                }
                                //On a Purim Meshulash in Yerushalayim, מגילת אסתר is on י"ד
                                if (!isYerushalayim || this.dow == DayOfWeek.Friday)
                                {
                                    this.AddDayNote("Megilas Esther", "מגילת אסתר");
                                    if (!isYerushalayim)
                                    {
                                        this.AddDayNote("Purim", "פורים");
                                        this.AddTefillahNote("Al Hanissim", "על הניסים");
                                        if (this.showGaonShirShelYom)
                                        {
                                            this.AddTefillahNote("שיר של יום - כ\"ב - למנצח על אילת השחר");
                                        }
                                    }
                                    else
                                    {
                                        //On a Purim Meshulash in Yerushalayim, מתנות לאביונים is on י"ד
                                        this.AddDayNote("Matanos LeEvyonim", "מתנות לאביונים");
                                    }
                                }
                                else
                                {
                                    this.AddDayNote("Purim D`Prazim", "פורים דפרזים");
                                }
                            }
                            else if (this.day == 15)
                            {
                                this.noTachnun = true;
                                if ((this.isMorning || this.ignoreTime) && this.dow != DayOfWeek.Saturday)
                                {
                                    this.NoLaminatzeach();
                                }
                                if (isYerushalayim)
                                {
                                    this.AddDayNote("Purim", "פורים");
                                    this.AddTefillahNote("Al Hanissim", "על הניסים");
                                    if (this.dow != DayOfWeek.Saturday)
                                    {
                                        this.AddDayNote("Megilas Esther", "מגילת אסתר");
                                    }
                                    if (
                                        (this.showGaonShirShelYom) &&
                                        (this.isDaytime || this.ignoreTime) &&
                                        this.dow != DayOfWeek.Saturday
                                    )
                                    {
                                        this.AddTefillahNote("שיר של יום - כ\"ב - למנצח על אילת השחר");
                                    }
                                }
                                else if ((new[]
                                  {
                                      "טבריה",
                                      "יפו",
                                      "עכו",
                                      "צפת",
                                      "באר שבע",
                                      "חיפה",
                                      "באר שבע",
                                      "בית שאן",
                                      "לוד",


                                }).Any(l => l == this.location.Name)
                              )
                                {
                                    this.AddDayNote("Purim D`Mukafin", "פורים דמוקפין");
                                    if (this.dow != DayOfWeek.Saturday)
                                    {
                                        this.AddDayNote("(Megilas Esther)", "(מגילת אסתר)");
                                    }
                                }
                                else
                                {
                                    this.AddDayNote("Shushan Purim", "שושן פורים");
                                }
                            }
                            else if (
                              this.day == 16 &&
                              isYerushalayim &&
                              this.dow == DayOfWeek.Sunday
                          )
                            {
                                this.AddDayNote(
                                    "Purim Seuda and Mishloach Manos",
                                    "סעודת פורים ומשלוח מנות");
                            }
                        }
                    }
                    break;
            }
        }

        private void NoLaminatzeach()
        {
            this.AddTefillahNote("No Laminatzeach", "א\"א למנצח");
        }

        private void AddDayNote(string englishOrDefaultText, string hebrewText = null)
        {
            var note = this.showEnglish || string.IsNullOrWhiteSpace(hebrewText) ? englishOrDefaultText : hebrewText;
            if (!this.dayNotes.Contains(note))
            {
                this.dayNotes.Add(note);
            }
        }

        private void AddTefillahNote(string englishOrDefaultText, string hebrewText = null)
        {
            var note = this.showEnglish || string.IsNullOrWhiteSpace(hebrewText) ? englishOrDefaultText : hebrewText;
            if (!this.tefillahNotes.Contains(note))
            {
                this.tefillahNotes.Add(note);
            }
        }

        private bool HasOwnKriyasHatorah()
        {
            //Rosh chodesh
            if (this.day == 1 || this.day == 30)
            {
                return true;
            }
            switch (this.month)
            {
                case 1:
                    return this.day > 14 && this.day < 22;
                case 4:
                    return this.day == 17 || (this.dow == DayOfWeek.Sunday && this.day == 18);
                case 5:
                    return this.day == 9 || (this.dow == DayOfWeek.Sunday && this.day == 10);
                case 7:
                    return (

                        (new[] { 3, 16, 17, 18, 19, 20, 21 }).Any(d => d == this.day) ||
                        (this.dow == DayOfWeek.Sunday && this.day == 4)
                    );
                case 9:
                    return this.day >= 25;
                case 10:
                    return (
                        this.day == 10 ||
                        this.day < 3 ||
                        (this.day == 3 && JewishDateCalculations.IsShortKislev(this.jdate.Year))
                    );
                case 12:
                case 13:
                    return (
                        this.month == (JewishDateCalculations.IsJewishLeapYear(this.jdate.Year) ? 13 : 12)
                        && (this.day == 13 || this.day == (this.location.Name == "ירושלים" ? 15 : 14))
                    );
                default:
                    return false;
            }
        }

    }
}
