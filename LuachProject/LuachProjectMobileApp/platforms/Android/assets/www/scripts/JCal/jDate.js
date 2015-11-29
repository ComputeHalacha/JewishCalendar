(function () {
    "use strict";

    window.jDate = function (arg, month, day) {
        var self = this;

        self.Day = NaN;
        self.Month = NaN;
        self.Year = NaN;
        self.AbsoluteDate = NaN;

        self.getSecularDate = function () {
            var dt = new Date(2000, 0, 1); //absolute date 730120
            dt.setDate((self.AbsoluteDate - 730120) + 1);
            return dt;
        };

        self.getDayOfWeek = function () {
            return Math.abs(self.AbsoluteDate % 7);
        };

        if (arg instanceof Date) {
            self.AbsoluteDate = jDate.absoluteFromSDate(arg);
            jDate.setFromAbsolute(self);
        }
        else if (arg instanceof String) {
            self.AbsoluteDate = jDate.absoluteFromSDate(Date.parse(arg));
            jDate.setFromAbsolute(self);
        }
        else if (typeof arg === 'number') {
            if (typeof month === 'undefined') {
                self.AbsoluteDate = arg;
                jDate.setFromAbsolute(self);
            }
            else {
                self.Year = arg;
                self.Month = month;
                self.Day = day;
                self.AbsoluteDate = jDate.absoluteFromJDate(self.Year, self.Month, self.Day);
            }
        }
        else if (typeof arg === 'object' && typeof arg.year === 'number') {
            self.Day = arg.day;
            self.Month = arg.month;
            self.Year = arg.year;
            self.AbsoluteDate = jDate.absoluteFromJDate(self.Year, self.Month, self.Day);
        }
    }

    jDate.jewishMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini"];
    jDate.jewishMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"];
    jDate.daysOfWeekEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"];
    jDate.daysOfWeekHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"];

    jDate.setFromAbsolute = function (jd) {
        jd.Year = parseInt((jd.AbsoluteDate + (-1373429)) / 366); // Approximation from below.
        // Search forward for year from the approximation.
        while (jd.AbsoluteDate >= new jDate({ year: (jd.Year + 1), month: 7, day: 1 }).AbsoluteDate) {
            jd.Year++;
        }
        // Search forward for month from either Tishrei or Nissan.
        if (jd.AbsoluteDate < new jDate({ year: jd.Year, month: 1, day: 1 }).AbsoluteDate) {
            jd.Month = 7; //  Start at Tishrei
        }
        else {
            jd.Month = 1; //  Start at Nissan
        }
        while (jd.AbsoluteDate > new jDate({ year: jd.Year, month: jd.Month, day: jDate.daysInJewishMonth(jd.Year, jd.Month) }).AbsoluteDate) {
            jd.Month++;
        }
        // Calculate the day by subtraction.
        jd.Day = (jd.AbsoluteDate - new jDate({ year: jd.Year, month: jd.Month, day: 1 }).AbsoluteDate + 1);
    };

    jDate.absoluteFromSDate = function (date) {
        var year = date.getFullYear(),
            month = date.getMonth() + 1,
            numberOfDays = date.getDate();           // days this month
        // add days in prior months this year
        for (var i = month - 1; i > 0; i--) {
            numberOfDays += jDate.daysInGregorianMonth(i, year);
        }

        return (numberOfDays          // days this year
               + 365 * (year - 1)     // days in previous years ignoring leap days
               + parseInt((year - 1) / 4)       // Julian leap days before this year...
               - parseInt((year - 1) / 100)     // ...minus prior century years...
               + parseInt((year - 1) / 400));   // ...plus prior years divisible by 400
    };

    jDate.absoluteFromJDate = function (year, month, day) {
        var DayInYear = day; // Days so far this month.
        if (month < 7) { // Before Tishrei, so add days in prior months
            // this year before and after Nissan.
            var m = 7;
            while (m <= (jDate.monthsInJewishYear(year))) {
                DayInYear = DayInYear + jDate.daysInJewishMonth(year, m);
                m++;
            };
            m = 1;
            while (m < month) {
                DayInYear = DayInYear + jDate.daysInJewishMonth(year, m);
                m++;
            }
        }
        else { // Add days in prior months this year
            var m = 7;
            while (m < month) {
                DayInYear = DayInYear + jDate.daysInJewishMonth(year, m);
                m++;
            }
        }
        // Days elapsed before absolute date 1. -  Days in prior years.
        return DayInYear + (jDate.elapsedDays(year) + (-1373429));
    };

    jDate.daysInGregorianMonth = function (month, year) {
        switch (month) {
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
    };

    jDate.daysInJewishMonth = function (year, month) {
        if ((month == 2) || (month == 4) || (month == 6) || ((month == 8) &&
                    (!jDate.isLongCheshvan(year))) || ((month == 9) && jDate.isShortKislev(year)) || (month == 10) || ((month == 12) &&
                    (!jDate.isJewishLeapYear(year))) || (month == 13)) {
            return 29;
        }
        else {
            return 30;
        }
    };

    jDate.elapsedDays = function (year) {
        var MonthsElapsed = parseInt((235 * parseInt((year - 1) / 19)) + (12 * ((year - 1) % 19)) + (7 * ((year - 1) % 19) + 1) / 19); // Leap months this cycle -  Regular months in this cycle. -  Months in complete cycles so far.
        var PartsElapsed = 204 + 793 * (MonthsElapsed % 1080);
        var HoursElapsed = 5 + 12 * MonthsElapsed + 793 * parseInt(MonthsElapsed / 1080) + parseInt(PartsElapsed / 1080);
        var ConjunctionDay = parseInt(1 + 29 * MonthsElapsed + HoursElapsed / 24);
        var ConjunctionParts = 1080 * (HoursElapsed % 24) + PartsElapsed % 1080;
        var AlternativeDay;
        if ((ConjunctionParts >= 19440) ||
            (((ConjunctionDay % 7) == 2) && (ConjunctionParts >= 9924) && (!jDate.isJewishLeapYear(year))) ||
            (((ConjunctionDay % 7) == 1) && (ConjunctionParts >= 16789) && (jDate.isJewishLeapYear(year - 1)))) // at the end of a leap year -  15 hours, 589 parts or later... -  ...or is on a Monday at... -  ...of a common year, -  at 9 hours, 204 parts or later... -  ...or is on a Tuesday... -  If new moon is at or after midday,
        {
            // Then postpone Rosh HaShanah one day
            AlternativeDay = (ConjunctionDay + 1);
        }
        else {
            AlternativeDay = ConjunctionDay;
        }
        if (((AlternativeDay % 7) == 0) || ((AlternativeDay % 7) == 3) || ((AlternativeDay % 7) == 5)) // or Friday -  or Wednesday, -  If Rosh HaShanah would occur on Sunday,
        {
            // Then postpone it one (more) day
            return (1 + AlternativeDay);
        }
        else {
            return AlternativeDay;
        }
    };

    jDate.daysInJewishYear = function (year) {
        return ((jDate.elapsedDays(year + 1)) - (jDate.elapsedDays(year)));
    };

    jDate.daysInJewishMonth = function (year, month) {
        if ((month == 2) || (month == 4) || (month == 6) || ((month == 8) &&
            (!jDate.isLongCheshvan(year))) || ((month == 9) && jDate.isShortKislev(year)) || (month == 10) || ((month == 12) &&
            (!jDate.isJewishLeapYear(year))) || (month == 13)) {
            return 29;
        }
        else {
            return 30;
        }
    };

    jDate.isLongCheshvan = function (year) {
        return (jDate.daysInJewishYear(year) % 10) == 5;
    };

    jDate.isShortKislev = function (year) {
        return (jDate.daysInJewishYear(year) % 10) == 3;
    };

    jDate.isJewishLeapYear = function (year) {
        return (((7 * year) + 1) % 19) < 7;
    };

    jDate.monthsInJewishYear = function (year) {
        if (jDate.isJewishLeapYear(year)) {
            return 13;
        }
        else {
            return 12;
        }
    };
})();