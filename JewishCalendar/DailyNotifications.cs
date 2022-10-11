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
        private TimeSpan time = DateTime.Now.TimeOfDay;
        private readonly JewishDate jdate;
        private readonly int month;
        private readonly int day;
        private readonly DayOfWeek dow;
        private TimeOfDay chatzosHayom;
        private TimeOfDay chatzosHalayla;
        private TimeOfDay alos;
        private TimeOfDay shkia;
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
        private readonly bool showEnglish;
        private readonly Parsha[] sedras;

        /// <summary>
        /// Generate day and tefillah notifications for the given date and location
        /// </summary>
        /// <param name="dailyZmanim"></param>
        /// <param name="english"></param>
        public DailyNotifications(DailyZmanim dailyZmanim, bool english)
        {
            location = dailyZmanim.Location;
            israel = location.IsInIsrael;
            time = DateTime.Now.TimeOfDay;
            jdate = dailyZmanim.JewishDate;
            month = jdate.Month;
            day = jdate.Day;
            dow = jdate.DayOfWeek;
            chatzosHayom = dailyZmanim.Chatzos;
            chatzosHalayla = chatzosHayom + 720;
            alos = dailyZmanim.NetzMishor - 90;
            shkia = dailyZmanim.ShkiaAtElevation;
            isAfterChatzosHayom = chatzosHayom <= time;
            isAfterChatzosHalayla = chatzosHalayla > time ||
                (chatzosHalayla.Hour > 12 && time.Hours < 12); //Chatzos is before 0:00 (eg 23:58) and time is after 0:00
            isAfterAlos = alos <= time;
            isAfterShkia = shkia <= time;
            isDaytime = isAfterAlos && !isAfterShkia;
            isNightTime = !isDaytime;
            isMorning = isDaytime && !isAfterChatzosHayom;
            isAfternoon = isDaytime && isAfterChatzosHayom;
            isYomTov = isYomTovOrCholHamoed();
            isLeapYear = JewishDateCalculations.IsJewishLeapYear(jdate.Year);
            noTachnun = isAfternoon && (dow == DayOfWeek.Friday || day == 29);
            showGaonShirShelYom = israel;
            showEnglish = english;
            sedras = Sedra.GetSedra(jdate, israel);
        }

        /// <summary>
        /// Get the day and tefillah notifications
        /// </summary>
        /// <returns></returns>
        public (string[] DayNotes, string[] TefillahNotes) GetNotifications()
        {
            if (dow == DayOfWeek.Saturday)
            {
                getShabbosNotifications();
            }
            else
            {
                getWeekDayNotifications();
            }
            getAroundTheYearNotifications();

            if (noTachnun && isDaytime && !isYomTov)
            {
                if (dow != DayOfWeek.Saturday)
                {
                    AddTefillahNote("No Tachnun", "א\"א תחנון");
                }
                else if (isAfternoon)
                {
                    AddTefillahNote("No Tzidkascha", "א\"א צדקתך");
                }
                else if (
                  !(
                      (month == 1 && day > 21) ||
                      month == 2 ||
                      (month == 3 && day < 6)
                  )
              )
                {
                    AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                }
            }


            //return only unique values
            return (DayNotes: dayNotes.Distinct().ToArray(),
                    TefillahNotes: tefillahNotes.Distinct().ToArray());

        }

        private bool isYomTovOrCholHamoed()
        {
            return (
                IsYomTov() ||
                (jdate.Month == 1 && (new[] { 16, 17, 18, 19, 20 }).Any(d => d == jdate.Day)) ||
                (jdate.Month == 7 && (new[] { 16, 17, 18, 19, 20, 21 }).Any(d => d == jdate.Day))
            );
        }

        /**
         * Returns true if this day is yomtov
         * @param {Boolean} israel
         */
        private bool IsYomTov()
        {
            var day = jdate.Day;
            switch (jdate.Month)
            {
                case 1:
                    if (day == 15 || day == 21) return true;
                    if (!israel && (day == 16 || day == 22)) return true;
                    break;
                case 3:
                    if (day == 6 || (!israel && day == 7)) return true;
                    break;
                case 7:
                    if ((new[] { 1, 2, 10, 15, 22 }).Any(d => d == day))
                    {
                        return true;
                    }
                    if (!israel && (day == 16 || day == 23)) return true;
                    break;
            }
            return false;
        }

        /**Is the current Jewish Date the day before a yomtov that contains a Friday?*/
        private bool hasEiruvTavshilin()
        {
            return (
                //Eiruv Tavshilin is only on Wednesday or Thursday
                (dow == DayOfWeek.Thursday || dow == DayOfWeek.Wednesday) &&
                //today is Erev Yomtov
                isErevYomTov(jdate, israel) &&
                //Thursday OR Wednesday when in Chu"l or Erev Rosh Hashana anywhere
                (dow == DayOfWeek.Thursday || (dow == DayOfWeek.Wednesday && (!israel || jdate.Month == 6))) &&
                //No Eiruv Tavshilin on Erev yom kippur
                jdate.Day != 9
            );
        }


        /**Is today Erev Yom Tov? (includes Erev second days of Sukkos and Pesach) */
        private static bool isErevYomTov(JewishDate jdate, bool israel)
        {
            return (
                (jdate.Month == 1 && (jdate.Day == 14 || jdate.Day == 20)) ||
                (jdate.Month == 3 && jdate.Day == 5) ||
                (jdate.Month == 6 && jdate.Day == 29) ||
                ((jdate.Month == 7 && (jdate.Day == 9 || jdate.Day == 14 || jdate.Day == 21)))
            );
        }


        private void getShabbosNotifications()
        {
            if (month == 1 && day > 7 && day < 15)
            {
                AddDayNote("Shabbos Hagadol", "שבת הגדול");
            }
            else if (month == 7 && day > 2 && day < 10)
            {
                AddDayNote("Shabbos Shuva", "שבת שובה");
            }
            else if (month == 5 && day > 2 && day < 10)
            {
                AddDayNote("Shabbos Chazon", "שבת חזון");
            }
            else if ((month == (isLeapYear ? 12 : 11) && day > 24) ||
              (month == (isLeapYear ? 13 : 12) && day == 1))
            {
                AddDayNote("Parshas Shkalim", "פרשת שקלים");
            }
            else if (month == (isLeapYear ? 13 : 12) && day > 7 && day < 14)
            {
                AddDayNote("Parshas Zachor", "פרשת זכור");
            }
            else if (month == (isLeapYear ? 13 : 12) && day > 16 && day < 24)
            {
                AddDayNote("Parshas Parah", "פרשת פרה");
            }
            else if (
              (month == (isLeapYear ? 13 : 12) && day > 23 && day < 30) ||
              (month == 1 && day == 1)
          )
            {
                AddDayNote("Parshas Hachodesh", "פרשת החודש");
            }
            if (isMorning && !isYomTov)
            {
                if (sedras.Length > 0)
                {
                    AddTefillahNote(
                        $"Kriyas Hatorah Parshas {string.Join(" - ", sedras.Select(i => i.nameEng))}",
                        $"קה\"ת פרשת { string.Join(" - ", sedras.Select(i => i.nameHebrew))}");
                }
                //All months but Tishrei have Shabbos Mevarchim on the Shabbos before Rosh Chodesh
                if (month != 6 && day > 22 && day < 30)
                {
                    var nextMonth = jdate.AddMonths(1);
                    AddTefillahNote(
                        "The molad will be " +
                            Molad.GetMolad(nextMonth.Year, nextMonth.Month).ToString(),
                        "המולד יהיה ב" +
                            Molad.GetMolad(nextMonth.Year, nextMonth.Month).ToStringHeb(shkia));
                    AddTefillahNote("Bircas Hachodesh", "מברכים החודש");
                    if (month != 1 && month != 2)
                    {
                        AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                    }
                }
            }

            //Rosh Chodesh
            if (month != 7 && (day == 1 || day == 30))
            {
                AddDayNote("Rosh Chodesh", "ראש חודש");
                AddTefillahNote("Ya`aleh Viyavo", "יעלה ויבא");
                if (showGaonShirShelYom && isDaytime)
                {
                    AddTefillahNote("Barchi Nafshi", "שיר של יום - קי\"ד - ברכי נפשי");
                }
                //Rosh Chodesh Teves is during Chanuka
                if (isDaytime && month != 10 && !(month == 9 && day == 30))
                {
                    AddTefillahNote("Chatzi Hallel", "חצי הלל");
                }
                AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
            }
            else if (isYomTov)
            {
                AddTefillahNote("No Av Harachamim", "א\"א אב הרחמים");
                if (showGaonShirShelYom && isDaytime)
                {
                    AddTefillahNote("שיר של יום - מזמור שיר ליום השבת");
                }
            }
            //Kriyas Hatora - Shabbos by mincha - besides for Yom Kippur
            if (isAfternoon && !(month == 7 && day == 10))
            {
                var sedra = Sedra.GetSedra(jdate + 1, israel);
                AddTefillahNote(
                    "Kriyas Hatorah Mincha Parshas " + sedra[0].nameEng,
                    "קה\"ת במנחה פרשת " + sedra[0].nameHebrew);
            }
            if (
                isAfternoon &&
                ((month == 1 && day > 21) ||
                    (month <= 6 && !(month == 5 && (day == 8 || day == 9))))
            )
            {
                var prakim = PirkeiAvos.GetPirkeiAvos(jdate, israel);
                if (prakim.Length > 0)
                {
                    AddDayNote(
                        "Pirkei Avos - " + string.Join(" and ",
                            prakim.Select(s => $"Perek { Utils.ToNumberHeb(s)}")),
                        "פרקי אבות - " + string.Join(" ו ",
                            prakim.Select(s => $"פרק { Utils.ToNumberHeb(s)}")));
                }
            }
        }

        private void getWeekDayNotifications()
        {
            //מוצאי שבת
            if (isNightTime && dow == DayOfWeek.Sunday)
            {
                //הבדלה בתפילה for מוצאי שבת
                AddTefillahNote(
                    (month == 1 && day == 15) || (month == 3 && day == 6)
                        ? "ותודיעינו"
                        : "אתה חוננתנו");
                //Motzai Shabbos before Yom Tov - no ויהי נועם
                if (
                    (month == 6 && day > 22) ||
                    (month == 7 && day < 22 && day != 3) ||
                    (month == 1 && day > 8 && day < 15) ||
                    (month == 3 && day < 6)
                )
                {
                    AddTefillahNote("No Vihi Noam", "א\"א ויהי נועם");
                }
            }
            //אתה חוננתנו for מוצאי יו"ט
            else if (
                isNightTime &&
                ((month == 1 && (day == 16 || day == 22)) ||
                    (month == 3 && day == 7) ||
                    (month == 7 && ((new[] { 3, 11, 16, 23 }).Any(d => d == day))))
            )
            {
                AddTefillahNote("Ata Chonantanu", "אתה חוננתנו");
            }
            //Kriyas hatorah for Monday and Thursday
            //when it"s not chol hamoed, chanuka, purim, a fast day or rosh chodesh
            if (
                isMorning &&
                !isYomTov &&
                (dow == DayOfWeek.Monday || dow == DayOfWeek.Thursday) &&
                !hasOwnKriyasHatorah()
            )
            {
                if (sedras.Length > 0)
                {
                    AddTefillahNote(
                        $"Kriyas Hatorah Parshas {sedras[0].nameEng}",
                        $"קה\"ת פרשת {sedras[0].nameHebrew}");
                }
            }
            //Rosh Chodesh
            if ((month != 7 && day == 1) || day == 30)
            {
                noTachnun = true;
                AddDayNote("Rosh Chodesh", "ראש חודש");
                AddTefillahNote("Ya`aleh Viyavo", "יעלה ויבא");
                if (showGaonShirShelYom && isDaytime)
                {
                    AddTefillahNote("Barchi Nafshi", "שיר של יום - קי\"ד - ברכי נפשי");
                }
                //Rosh Chodesh Teves is during Chanuka
                if (isDaytime && month != 10 && !(month == 9 && day == 30))
                {
                    AddTefillahNote("Chatzi Hallel", "חצי הלל");
                    if (isMorning && dow != DayOfWeek.Saturday)
                    {
                        noLaminatzeach();
                    }
                }
            }
            //Yom Kippur Kattan
            else if (
                month != 6 &&
                ((dow < DayOfWeek.Friday && day == 29) ||
                    (dow == DayOfWeek.Thursday && day == 28)) &&
                isAfternoon
            )
            {
                AddTefillahNote("Yom Kippur Kattan", "יו\"כ קטן");
            }
            if (hasEiruvTavshilin())
            {
                AddDayNote("Eruv Tavshilin", "עירוב תבשילין");
            }
        }

        private void getAroundTheYearNotifications()
        {
            switch (month)
            {
                case 1: //Nissan
                    noTachnun = true;
                    if (day > 15)
                    {
                        AddTefillahNote("Morid Hatal", "מוריד הטל");
                    }
                    if (dow != DayOfWeek.Saturday && day > 15 && day != 21)
                    {
                        AddTefillahNote("Vesain Bracha", "ותן ברכה");
                    }
                    if (
                        isMorning &&
                        dow != DayOfWeek.Saturday &&
                        (new[] { 14, 16, 17, 18, 19, 20 }).Any(d => d == day)
                    )
                    {
                        AddTefillahNote("No Mizmor Lesodah", "א\"א מזמור לתודה");
                        if (dow != DayOfWeek.Saturday)
                        {
                            noLaminatzeach();
                        }
                    }
                    if (day == 15)
                    {
                        AddDayNote("First Day of Pesach", "יו\"ט ראשון של פסח");
                        AddTefillahNote("Full Hallel", "הלל השלם");
                        if (isAfternoon)
                        {
                            AddTefillahNote("Morid Hatal", "מוריד הטל");
                        }
                        if (
                            showGaonShirShelYom &&
                            isDaytime &&
                            dow != DayOfWeek.Saturday
                        )
                        {
                            AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                        }
                    }
                    else if (day == 16 && !israel)
                    {
                        AddDayNote("Second Day of Pesach", "יו\"ט שני של פסח");
                        AddTefillahNote("Full Hallel", "הלל השלם");
                        AddTefillahNote("Morid Hatal", "מוריד הטל");
                        if (
                            showGaonShirShelYom &&
                            isDaytime &&
                            dow != DayOfWeek.Saturday
                        )
                        {
                            AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                        }
                    }
                    else if ((new[] { 16, 17, 18, 19, 20, 21 }).Any(d => d == day))
                    {
                        if (day == 21)
                        {
                            AddDayNote("Shvi`i Shel Pesach", "שביעי של פםח");
                            if (isDaytime)
                            {
                                if (israel)
                                {
                                    AddTefillahNote("Yizkor", "יזכור");
                                }
                                if (showGaonShirShelYom && dow != DayOfWeek.Saturday)
                                {
                                    AddTefillahNote(
                                        "שיר של יום - י\"ח - למנצח לעבד ה\"");
                                }
                            }
                        }
                        else
                        {
                            AddDayNote("Chol Ha`moed Pesach", "פסח - חול המועד");
                            AddTefillahNote("Ya`aleh Viyavo", "יעלה ויבא");
                            if (isMorning && dow != DayOfWeek.Saturday)
                                noLaminatzeach();
                            if (
                                showGaonShirShelYom &&
                                isDaytime &&
                                dow != DayOfWeek.Saturday
                            )
                            {
                                switch (day)
                                {
                                    case 16:
                                        if (dow == DayOfWeek.Sunday)
                                        {
                                            AddTefillahNote("שיר של יום - קי\"ד - בצאת ישראל");
                                        }
                                        else
                                        {
                                            AddTefillahNote("שיר של יום - ע\"ח - משכיל לאסף");
                                        }
                                        break;
                                    case 17:
                                        if (dow == DayOfWeek.Monday)
                                        {
                                            AddTefillahNote("שיר של יום - ע\"ח - משכיל לאסף");
                                        }
                                        else
                                        {
                                            AddTefillahNote("שיר של יום - פ\" - למנצח אל שושנים");
                                        }
                                        break;
                                    case 18:
                                        if (
                                            dow == DayOfWeek.Tuesday ||
                                            dow == DayOfWeek.Sunday
                                        )
                                        {
                                            AddTefillahNote("שיר של יום - פ\" - למנצח אל שושנים");
                                        }
                                        else
                                        {
                                            AddTefillahNote("שיר של יום - ק\"ה - הודו לה\"");
                                        }
                                        break;
                                    case 19:
                                        if (dow == DayOfWeek.Thursday)
                                        {
                                            AddTefillahNote("שיר של יום - קל\"ה - הללוי - ה הללו את שם");
                                        }
                                        else
                                        {
                                            AddTefillahNote("שיר של יום - ק\"ה - הודו לה\"");
                                        }
                                        break;
                                    case 20:
                                        if (dow == DayOfWeek.Friday)
                                        {
                                            AddTefillahNote("שיר של יום - ס\"ו - למנצח שיר מזמור");
                                        }
                                        else
                                        {
                                            AddTefillahNote("שיר של יום - קל\"ה - הללוי - ה הללו את שם");
                                        }
                                        break;
                                }
                            }
                        }
                        if (isDaytime) AddTefillahNote("Half Hallel", "חצי הלל");
                    }
                    if (day == 22)
                    {
                        if (israel)
                        {
                            AddDayNote("Isru Chag", "איסרו חג");
                        }
                        else
                        {
                            AddDayNote("Acharon Shel Pesach", "אחרון של פסח");
                            if (isDaytime)
                            {
                                AddTefillahNote("Yizkor", "יזכור");
                                AddTefillahNote("Half Hallel", "חצי הלל");
                            }
                        }
                        if (dow != DayOfWeek.Saturday && isMorning)
                        {
                            noLaminatzeach();
                        }
                    }
                    else if (
                      dow == DayOfWeek.Saturday &&
                      ((new[] { 15, 16, 17, 18, 19, 20, 21 }).Any(d => d == day) ||
                          (!israel && day == 22))
                  )
                    {
                        AddTefillahNote("Shir Hashirim", "מגילת שיר השירים");
                    }
                    break;
                case 2: //Iyar
                    if (day <= 15)
                    {
                        AddTefillahNote("Morid Hatal", "מוריד הטל");
                        if (dow != DayOfWeek.Saturday)
                        {
                            AddTefillahNote("V`sain Bracha", "ותן ברכה");
                        }
                    }
                    //Pesach Sheini and Lag Ba"Omer
                    if (
                        day == 14 ||
                        (day == 13 && isAfternoon) ||
                        day == 18 ||
                        (day == 17 && isAfternoon)
                    )
                    {
                        noTachnun = true;
                        if (day == 14)
                        {
                            AddDayNote("Pesach Sheini", "פסח שני");
                        }
                    }
                    //Baha"b
                    if (
                        isMorning &&
                        ((dow == DayOfWeek.Monday && day > 3 && day < 13) ||
                            (dow == DayOfWeek.Thursday && day > 6 && day < 14) ||
                            (dow == DayOfWeek.Monday &&
                                day > 10 &&
                                day < 18 &&
                                day != 14))
                    )
                    {
                        AddTefillahNote("Ba`hab", "סליחות בה\"ב");
                        AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                    }
                    break;
                case 3: //Sivan
                    if (day < 13)
                    {
                        noTachnun = true;
                    }
                    if (day == 6 && isMorning)
                    {
                        AddDayNote("Shavuos", "יום טוב של שבועות");
                        AddTefillahNote("Full Hallel", "הלל השלם");
                        AddTefillahNote("Megilas Rus", "מגילת רות");
                        AddTefillahNote("Akdamus", "אקדמות");
                        if (israel) AddTefillahNote("Yizkor", "יזכור");
                        if (showGaonShirShelYom)
                        {
                            AddTefillahNote("שיר של יום - י\"ט - ..השמים מספרים..");
                        }
                    }
                    else if (day == 7)
                    {
                        if (israel)
                        {
                            AddDayNote("Issru Chag", "איסרו חג");
                            if (isMorning && dow != DayOfWeek.Saturday)
                            {
                                noLaminatzeach();
                            }
                        }
                        else
                        {
                            AddDayNote("Shavuos Second Day", "יום טוב של שבועות");
                            AddTefillahNote("Full Hallel", "הלל השלם");
                            AddTefillahNote("Yizkor", "יזכור");
                        }
                    }
                    break;
                case 4: //Tammuz
                    if (
                        isDaytime &&
                        ((day == 17 && dow != DayOfWeek.Saturday) ||
                            (day == 18 && dow == DayOfWeek.Sunday))
                    )
                    {
                        if (isDaytime)
                        {
                            AddDayNote("Shiva Asar B`Tamuz", "י\"ז בתמוז");
                            AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                            AddTefillahNote("Aneinu", "עננו");
                        }
                        if (isMorning)
                        {
                            AddTefillahNote("Selichos", "סליחות");
                        }
                    }
                    break;
                case 5: //Av
                    if (isAfternoon && day == 8 && dow != DayOfWeek.Friday)
                    {
                        noTachnun = true;
                    }
                    else if (
                      (day == 9 && dow != DayOfWeek.Saturday) ||
                      (day == 10 && dow == DayOfWeek.Sunday)
                  )
                    {
                        AddDayNote("Tish B`Av", "תשעה באב");
                        if (isDaytime)
                        {
                            AddTefillahNote("Kinos", "קינות");
                            AddTefillahNote("Aneinu", "עננו");
                            if (isMorning && dow != DayOfWeek.Saturday)
                            {
                                noLaminatzeach();
                            }
                        }
                        else
                        {
                            AddTefillahNote("Megilas Eicha", "מגילת איכה");
                            if (isNightTime && dow == DayOfWeek.Sunday)
                            {
                                AddTefillahNote("No Vihi Noam", "א\"א ויהי נועם");
                            }
                        }
                        noTachnun = true;
                    }
                    else if (isAfternoon && day == 14)
                    {
                        noTachnun = true;
                    }
                    else if (day == 15)
                    {
                        AddDayNote("Tu B`Av", "ט\"ו באב");
                        noTachnun = true;
                    }
                    break;
                case 6: //Ellul
                    AddTefillahNote("L`Dovid Hashem Ori", "לדוד ה");
                    if (
                        day > 20 &&
                        dow != DayOfWeek.Saturday &&
                        (isAfterChatzosHalayla || isMorning)
                    )
                    {
                        var startedSelichos = day >= 26;
                        if (day < 26)
                        {
                            var daysToRH = 30 - day;
                            var dowRH = (DayOfWeek)((daysToRH + (int)dow) % 7);
                            switch (dowRH)
                            {
                                case DayOfWeek.Monday:
                                    startedSelichos = day >= 22;
                                    break;
                                case DayOfWeek.Tuesday:
                                    startedSelichos = day >= 21;
                                    break;
                                case DayOfWeek.Saturday:
                                    startedSelichos = day >= 24;
                                    break;
                            }
                        }
                        if (startedSelichos)
                        {
                            AddTefillahNote("Selichos", "סליחות");
                        }
                    }
                    if (day == 29)
                    {
                        noTachnun = true;
                    }
                    break;
                case 7: //Tishrei
                    if (day < 11)
                    {
                        AddTefillahNote("Hamelech Hakadosh", "המלך הקדוש");
                        if (dow != DayOfWeek.Saturday && day != 9)
                        {
                            AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                        }
                    }
                    //Days of Rosh Hashana, Tzom Gedaliah and Yom Kippur are dealt with individually below.
                    if (day > 4 && day < 10 && dow != DayOfWeek.Saturday)
                    {
                        AddTefillahNote("Selichos", "סליחות");
                        AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                    }
                    if (dow == DayOfWeek.Saturday && day > 2 && day < 10)
                    {
                        AddDayNote("Shabbos Shuva", "שבת שובה");
                    }
                    if (day >= 10)
                    {
                        noTachnun = true;
                    }
                    switch (day)
                    {
                        case 1:
                            AddDayNote("Rosh Hashana", "ראש השנה");
                            if (dow != DayOfWeek.Saturday && isDaytime)
                            {
                                AddTefillahNote("Tekias Shofar", "תקיעת שופר");
                                if (showGaonShirShelYom)
                                {
                                    AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                }
                                if (isAfternoon)
                                {
                                    AddDayNote("Tashlich", "תשליך");
                                }
                            }
                            break;
                        case 2:
                            AddDayNote("Rosh Hashana", "ראש השנה");
                            if (isDaytime)
                            {
                                AddTefillahNote("Tekias Shofar", "תקיעת שופר");
                                if (showGaonShirShelYom)
                                {
                                    AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                }
                                if (dow == DayOfWeek.Sunday && isAfternoon)
                                {
                                    AddDayNote("Tashlich", "תשליך");
                                }
                            }
                            break;
                        case 3:
                            if (dow != DayOfWeek.Saturday)
                            {
                                if (isDaytime)
                                {
                                    AddDayNote("Fast of Tzom Gedalya", "צום גדליה");
                                    AddTefillahNote("Aneinu", "עננו");
                                }
                                if (isAfterChatzosHalayla || isMorning)
                                {
                                    AddTefillahNote("Selichos", "סליחות");
                                }
                                AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                            }
                            break;
                        case 4:
                            if (dow == DayOfWeek.Sunday)
                            {
                                if (isDaytime)
                                {
                                    AddDayNote("Fast of Tzom Gedalya", "צום גדליה");
                                    AddTefillahNote("Aneinu", "עננו");
                                }
                                if (isAfterChatzosHalayla || isMorning)
                                {
                                    AddTefillahNote("Selichos", "סליחות");
                                }
                                AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                            }
                            else if (dow != DayOfWeek.Saturday)
                            {
                                AddTefillahNote("Hamelech Hamishpat", "המלך המשפט");
                                if (isAfterChatzosHalayla || isMorning)
                                {
                                    AddTefillahNote("Selichos", "סליחות");
                                }
                            }
                            break;
                        case 9:
                            AddDayNote("Erev Yom Kippur", "ערב יום כיפור");
                            if (isMorning)
                            {
                                AddTefillahNote("No Mizmor L`Sodah", "א\"א מזמור לתודה");
                                if (dow != DayOfWeek.Saturday)
                                {
                                    noLaminatzeach();
                                }
                                if (dow == DayOfWeek.Friday)
                                {
                                    AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                                }
                            }
                            else if (isAfternoon)
                            {
                                AddTefillahNote("Vidduy", "ודוי בעמידה");
                            }
                            if (isDaytime && dow != DayOfWeek.Friday)
                            {
                                AddTefillahNote("No Avinu Malkeinu", "א\"א אבינו מלכנו");
                            }
                            noTachnun = true;
                            break;
                        case 10:
                            AddDayNote("Yom Kippur", "יום הכיפורים");
                            AddDayNote("לפני ה\" תטהרו");
                            if (isDaytime)
                            {
                                AddTefillahNote("Yizkor", "יזכור");
                                if (showGaonShirShelYom && dow != DayOfWeek.Saturday)
                                {
                                    AddTefillahNote("שיר של יום - ל\"ב - לדוד משכיל");
                                }
                            }
                            if (isAfternoon)
                            {
                                //only Yom Kippur has its own Kriyas Hatorah
                                AddTefillahNote("קה\"ת במנחה סוף פרשת אח\"מ");
                            }
                            break;
                        case 15:
                            AddDayNote("First day of Sukkos", "יו\"ט ראשון של סוכות");
                            AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (isDaytime)
                            {
                                AddTefillahNote("Full Hallel", "הלל השלם");
                                if (dow != DayOfWeek.Saturday)
                                {
                                    AddTefillahNote("Hoshanos - למען אמתך", "הושענות - למען אמתך");
                                    AddTefillahNote("Kah Keli", "קה קלי");
                                    if (showGaonShirShelYom)
                                    {
                                        AddTefillahNote("שיר של יום - ע\"ו - למנצח בנגינות מזמור");
                                    }
                                }
                                else
                                {
                                    AddTefillahNote("Hoshanos - אום נצורה", "הושענות - אום נצורה");
                                }
                            }
                            break;
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (day == 16 && !israel)
                            {
                                AddDayNote(
                                    "Second day of Sukkos",
                                    "סוכות - יום טוב שני"


                                );
                            }
                            else if (!israel)
                            {
                                AddDayNote("Chol Hamoed Sukkos", "סוכות - חול המועד");
                                AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            }
                            if (isDaytime)
                            {
                                AddTefillahNote("Full Hallel", "הלל השלם");
                                switch (day)
                                {
                                    case 16:
                                        AddTefillahNote(
                                            "הושענות - " +
                                                (dow == DayOfWeek.Sunday
                                                    ? "למען אמתך"
                                                    : "אבן שתיה"));
                                        if (
                                            showGaonShirShelYom &&
                                            dow != DayOfWeek.Saturday
                                        )
                                        {
                                            AddTefillahNote("שיר של יום - כ\"ט - ..הבו לה\" בני אלים");
                                        }
                                        break;
                                    case 17:
                                        AddTefillahNote(
                                            (showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "אערוך שועי"));
                                        if (
                                            showGaonShirShelYom &&
                                            dow != DayOfWeek.Saturday
                                        )
                                        {
                                            AddTefillahNote("שיר של יום - נ\" - מזמור לאסף");
                                        }
                                        break;
                                    case 18:
                                        if (dow == DayOfWeek.Sunday)
                                        {
                                            AddTefillahNote("Hoshanos", "הושענות");
                                            if (
                                                showGaonShirShelYom &&
                                                dow != DayOfWeek.Saturday
                                            )
                                            {
                                                AddTefillahNote("שיר של יום - נ\" - מזמור לאסף");
                                            }
                                        }
                                        else
                                        {
                                            if (dow == DayOfWeek.Tuesday)
                                            {
                                                AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            else if (dow == DayOfWeek.Thursday)
                                            {
                                                AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            else if (dow == DayOfWeek.Friday)
                                            {
                                                AddTefillahNote("Hoshanos", "הושענות");
                                            }
                                            if (
                                                showGaonShirShelYom &&
                                                dow != DayOfWeek.Saturday
                                            )
                                            {
                                                AddTefillahNote("שיר של יום - צ\"ד - ..מי יקום לי..");
                                            }
                                        }
                                        break;
                                    case 19:
                                        AddTefillahNote(
                                            (showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "א-ל למושעות"));
                                        if (
                                            showGaonShirShelYom &&
                                            dow != DayOfWeek.Saturday
                                        )
                                        {
                                            if (dow == DayOfWeek.Monday)
                                            {
                                                AddTefillahNote("שיר של יום - צ\"ד - ..מי יקום לי..");
                                            }
                                            else
                                            {
                                                AddTefillahNote("שיר של יום - צ\"ד - א - ל נקמות..ישרי לב");
                                            }
                                        }
                                        break;
                                    case 20:
                                        AddTefillahNote(
                                            (showEnglish ? "Hoshanos" : "הושענות") +
                                                " - " +
                                                (dow == DayOfWeek.Saturday
                                                    ? "אום נצורה"
                                                    : "אדון המושיע"));
                                        if (
                                            showGaonShirShelYom &&
                                            dow != DayOfWeek.Saturday
                                        )
                                        {
                                            if (dow == DayOfWeek.Thursday)
                                            {
                                                AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                            }
                                            else
                                            {
                                                AddTefillahNote("שיר של יום - צ\"ד - א - ל נקמות..ישרי לב");
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case 21:
                            AddDayNote("Hoshana Raba", "הושעה רבה");
                            AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (isNightTime)
                            {
                                AddTefillahNote("Mishneh Torah", "משנה תורה");
                            }
                            else
                            {
                                AddTefillahNote("Hoshanos", "הושענות");
                                AddTefillahNote("Full Hallel", "הלל השלם");
                                if (showGaonShirShelYom)
                                {
                                    if (dow == DayOfWeek.Friday)
                                    {
                                        AddTefillahNote("שיר של יום - פ\"ב - מזמור לאסף");
                                    }
                                    else
                                    {
                                        AddTefillahNote("שיר של יום - פ\"א - למנצח על הגתית");
                                    }
                                }
                            }
                            break;
                        case 22:
                            AddDayNote("Shmini Atzeres", "שמיני עצרת");
                            AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                            if (israel)
                            {
                                AddDayNote("Simchas Torah", "שמחת תורה");
                                AddTefillahNote("Hakafos", "הקפות");
                            }
                            if (isDaytime)
                            {
                                AddTefillahNote("Full Hallel", "הלל השלם");
                                AddTefillahNote("Yizkor", "יזכור");
                                AddTefillahNote("Tefilas Geshem", "תפילת גשם");
                                AddTefillahNote("משיב הרוח ומוריד הגשם");
                                if (showGaonShirShelYom && dow != DayOfWeek.Saturday)
                                {
                                    AddTefillahNote("שיר של יום - י\"ב - למנצח על השמינית");
                                }
                            }
                            break;
                    }
                    if (day == 23)
                    {
                        if (!israel)
                        {
                            AddDayNote("Simchas Torah", "שמחת תורה");
                            AddTefillahNote("Hakafos", "הקפות");
                            AddTefillahNote("Full Hallel", "הלל השלם");
                            AddTefillahNote("Ya`aleh V`yavo", "יעלה ויבא");
                        }
                        else
                        {
                            AddDayNote("Issru Chag", "איסרו חג");
                            if (isNightTime)
                            {
                                AddDayNote("א גוטען ווינטער", "חורף טוב");
                            }
                            else if (dow != DayOfWeek.Saturday && isMorning)
                            {
                                noLaminatzeach();
                            }
                        }
                    }
                    else if (
                      dow == DayOfWeek.Saturday &&
                      (new[] { 15, 17, 18, 19, 20 }).Any(d => d == day))
                    {
                        AddTefillahNote("Megilas Koheles", "מגילת קהלת");
                    }
                    if (day < 22)
                    {
                        AddTefillahNote("L`Dovid Hashem Ori", "לדוד ה");
                    }
                    else if (day > 22)
                    {
                        AddTefillahNote(
                            "Mashiv Haruach U`Morid Hageshem",
                            "משיב הרוח ומוריד הגשם");
                    }
                    break;
                case 8: //Cheshvan
                    if (
                        isDaytime &&
                        ((dow == DayOfWeek.Monday && day > 3 && day < 13) ||
                            (dow == DayOfWeek.Thursday && day > 6 && day < 14) ||
                            (dow == DayOfWeek.Monday && day > 10 && day < 18))
                    )
                    {
                        AddTefillahNote("Ba`Hab", "סליחות בה\"ב");
                        AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                    }
                    if (day <= 22)
                    {
                        AddTefillahNote(
                            "Mashiv Haruach U`Morid Hageshem",
                            "משיב הרוח ומוריד הגשם");
                    }
                    if (day >= 7 && dow != DayOfWeek.Saturday)
                    {
                        AddTefillahNote("V`sain Tal U`matar", "ותן טל ומטר");
                    }
                    break;
                case 9: //Kislev
                    if (day <= 7 && dow != DayOfWeek.Saturday)
                    {
                        AddTefillahNote("V`sain Tal U`matar", "ותן טל ומטר");
                    }
                    else if (
                      day == 24 &&
                      dow != DayOfWeek.Saturday &&
                      isAfternoon
                  )
                    {
                        noTachnun = true;
                    }
                    else if (day >= 25)
                    {
                        noTachnun = true;
                        AddDayNote("Chanukah", "חנוכה");
                        AddTefillahNote("Al Hanissim", "על הניסים");
                        if (isDaytime)
                        {
                            AddTefillahNote("Full Hallel", "הלל השלם");
                            if (isMorning && dow != DayOfWeek.Saturday)
                                noLaminatzeach();
                            if (
                                showGaonShirShelYom &&
                                day != 30 &&
                                dow != DayOfWeek.Saturday
                            )
                            {
                                AddTefillahNote("שיר של יום - ל\" - מזמור שיר חנוכת הבית");
                            }
                        }
                    }
                    break;
                case 10: //Teves
                    if (day <= (JewishDateCalculations.IsShortKislev(jdate.Year) ? 3 : 2))
                    {
                        noTachnun = true;
                        AddDayNote("Chanukah", "חנוכה");
                        AddTefillahNote("Al Hanissim", "על הניסים");
                        if (isDaytime)
                        {
                            AddTefillahNote("Full Hallel", "הלל השלם");
                            if (isMorning && dow != DayOfWeek.Saturday)
                            {
                                noLaminatzeach();
                                if (day != 1 && showGaonShirShelYom)
                                {
                                    AddTefillahNote("שיר של יום - ל\" - מזמור שיר חנוכת הבית");
                                }
                            }
                        }
                    }
                    else if (day == 10 && isDaytime)
                    {
                        AddDayNote("Fast of Asara B`Teves", "עשרה בטבת");
                        if (isMorning)
                        {
                            AddTefillahNote("Selichos", "סליחות");
                        }
                        AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                        AddTefillahNote("Aneinu", "עננו");
                    }
                    break;
                case 11: //Shvat
                    if (day == 14 && isAfternoon) noTachnun = true;
                    if (day == 15)
                    {
                        noTachnun = true;
                        AddDayNote("Tu B`Shvat", "ט\"ו בשבט");
                    }
                    break;
                case 12:
                case 13:
                    if (month == 12 && isLeapYear)
                    {
                        //Adar Rishon in a leap year
                        if (
                            ((day == 13 && isAfternoon) || (day == 14 || day == 14)) &&
                            isDaytime
                        )
                        {
                            AddDayNote(
                                day == 14
                                    ? showEnglish
                                        ? "Purim Katan"
                                        : "פורים קטן"
                                    : showEnglish
                                    ? "Shushan Purim Katan"
                                    : "שושן פורים קטן"


                            );
                            noTachnun = true;
                            if (isMorning && dow != DayOfWeek.Saturday)
                            {
                                noLaminatzeach();
                            }
                        }
                    }
                    else
                    {
                        //The "real" Adar: the only one in a non-leap-year or Adar Sheini
                        if (
                            isDaytime &&
                            ((day == 11 && dow == DayOfWeek.Thursday) ||
                                (day == 13 && dow != DayOfWeek.Saturday))
                        )
                        {
                            if (isMorning)
                            {
                                AddDayNote("Fast of Ta`anis Esther", "תענית אסתר");
                                AddTefillahNote("Selichos", "סליחות");
                            }
                            AddTefillahNote("Avinu Malkeinu", "אבינו מלכנו");
                            AddTefillahNote("Aneinu", "עננו");
                        }
                        else
                        {
                            //Only ירושלים says על הניסים on ט"ו
                            bool isYerushalayim = location.Name == "ירושלים";
                            if (day == 14)
                            {
                                noTachnun = true;
                                if (isMorning && dow != DayOfWeek.Saturday)
                                {
                                    noLaminatzeach();
                                }
                                //On a Purim Meshulash in Yerushalayim, מגילת אסתר is on י"ד
                                if (!isYerushalayim || dow == DayOfWeek.Friday)
                                {
                                    AddDayNote("Megilas Esther", "מגילת אסתר");
                                    if (!isYerushalayim)
                                    {
                                        AddDayNote("Purim", "פורים");
                                        AddTefillahNote("Al Hanissim", "על הניסים");
                                        if (showGaonShirShelYom)
                                        {
                                            AddTefillahNote("שיר של יום - כ\"ב - למנצח על אילת השחר");
                                        }
                                    }
                                    else
                                    {
                                        //On a Purim Meshulash in Yerushalayim, מתנות לאביונים is on י"ד
                                        AddDayNote("Matanos LeEvyonim", "מתנות לאביונים");
                                    }
                                }
                                else
                                {
                                    AddDayNote("Purim D`Prazim", "פורים דפרזים");
                                }
                            }
                            else if (day == 15)
                            {
                                noTachnun = true;
                                if (isMorning && dow != DayOfWeek.Saturday)
                                {
                                    noLaminatzeach();
                                }
                                if (isYerushalayim)
                                {
                                    AddDayNote("Purim", "פורים");
                                    AddTefillahNote("Al Hanissim", "על הניסים");
                                    if (dow != DayOfWeek.Saturday)
                                    {
                                        AddDayNote("Megilas Esther", "מגילת אסתר");
                                    }
                                    if (
                                        showGaonShirShelYom &&
                                        isDaytime &&
                                        dow != DayOfWeek.Saturday
                                    )
                                    {
                                        AddTefillahNote("שיר של יום - כ\"ב - למנצח על אילת השחר");
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


                                }).Any(l => l == location.Name)
                              )
                                {
                                    AddDayNote("Purim D`Mukafin", "פורים דמוקפין");
                                    if (dow != DayOfWeek.Saturday)
                                    {
                                        AddDayNote("(Megilas Esther)", "(מגילת אסתר)");
                                    }
                                }
                                else
                                {
                                    AddDayNote("Shushan Purim", "שושן פורים");
                                }
                            }
                            else if (
                              day == 16 &&
                              isYerushalayim &&
                              dow == DayOfWeek.Sunday
                          )
                            {
                                AddDayNote(
                                    "Purim Seuda and Mishloach Manos",
                                    "סעודת פורים ומשלוח מנות");
                            }
                        }
                    }
                    break;
            }
        }

        private void noLaminatzeach()
        {
            AddTefillahNote("No Laminatzeach", "א\"א למנצח");
        }

        private void AddDayNote(string englishOrDefaultText, string hebrewText = null)
        {
            var note = showEnglish || string.IsNullOrWhiteSpace(hebrewText) ? englishOrDefaultText : hebrewText;
            if (!dayNotes.Contains(note))
            {
                dayNotes.Add(note);
            }
        }

        private void AddTefillahNote(string englishOrDefaultText, string hebrewText = null)
        {
            var note = showEnglish || string.IsNullOrWhiteSpace(hebrewText) ? englishOrDefaultText : hebrewText;
            if (!tefillahNotes.Contains(note))
            {
                tefillahNotes.Add(note);
            }
        }

        private bool hasOwnKriyasHatorah()
        {
            //Rosh chodesh
            if (day == 1 || day == 30)
            {
                return true;
            }
            switch (month)
            {
                case 1:
                    return day > 14 && day < 22;
                case 4:
                    return day == 17 || (dow == DayOfWeek.Sunday && day == 18);
                case 5:
                    return day == 9 || (dow == DayOfWeek.Sunday && day == 10);
                case 7:
                    return (

                        (new[] { 3, 16, 17, 18, 19, 20, 21 }).Any(d => d == day) ||
                        (dow == DayOfWeek.Sunday && day == 4)
                    );
                case 9:
                    return day >= 25;
                case 10:
                    return (
                        day == 10 ||
                        day < 3 ||
                        (day == 3 && JewishDateCalculations.IsShortKislev(jdate.Year))
                    );
                case 12:
                case 13:
                    return (
                        month == (JewishDateCalculations.IsJewishLeapYear(jdate.Year) ? 13 : 12) &&
                        (day == 13 || day == (location.Name == "ירושלים" ? 15 : 14))
                    );
                default:
                    return false;
            }
        }

    }
}
