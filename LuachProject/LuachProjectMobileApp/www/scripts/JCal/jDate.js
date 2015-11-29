(function () {
    "use strict";

    /*You can create a jDate with any of the following:
     *  new jDate(javascriptDateObject) - Sets to the Jewish date on the given Gregorian date
     *  new Date("January 1 2045") - same as above. Accepts any valid javascript Date string (uses Date.parse)
     *  new jDate(jewishYear, jewishMonth, jewishDay) - Months start at 1 - Nissan is 1
     *  new jDate(jewishYear, jewishMonth) - Same as above, with Day defaulting to 1
     *  new Date(absoluteDate) - The number of days elapsed since the theoretical date Sunday, December 31, 0001 BCE
     *  new Date( { year: 5776, month: 4, day: 5 } ) - same as new jDate(jewishYear, jewishMonth, jewishDay)
     *  new Date( { year: 5776, month: 4 } ) - same as new jDate(jewishYear, jewishMonth)
     *  new Date( { year: 5776 } ) - sets to the first day of Rosh Hashana on the given year
     */
    window.jDate = function (arg, month, day) {
        var self = this;

        self.Day = NaN;
        self.Month = NaN;
        self.Year = NaN;
        self.AbsoluteDate = NaN;

        self.getSecularDate = function () {
            var dt = new Date(2000, 0, 1); // 1/1/2000 is absolute date 730120
            dt.setDate((self.AbsoluteDate - 730120) + 1);
            return dt;
        };
        self.getDayOfWeek = function () {
            return Math.abs(self.AbsoluteDate % 7);
        };
        self.addDays = function (days) {
            return new jDate(self.AbsoluteDate + days);
        }
        self.toString = function () {
            return jDate.daysOfWeekEng[self.getDayOfWeek()] + ' ' +
                jDate.jewishMonthsEng[self.Month] + ' ' +
                self.Day.toString() + ' ' +
                self.Year.toString();
        };
        self.toStringHeb = function () {
            return jDate.daysOfWeekHeb[self.getDayOfWeek()] + ' ' +
               jDate.toJNumber(self.Day) + ' ' +
               jDate.jewishMonthsHeb[self.Month] + ' ' +
               jDate.toJNumber(self.Year % 1000);
        };
        self.getDiff = function (jd) {
            return self.AbsoluteDate - jd.AbsoluteDate;
        };
        self.getDayOfOmer = function () {
            var dayOfOmer = 0;
            if ((self.Month == 1 && self.Day > 15) || self.Month == 2 || (self.Month == 3 && self.Day < 6)) {
                dayOfOmer = self.getDiff(new jDate(self.Year, 1, 15));
            }
            return dayOfOmer;
        };
        self.setFromAbsolute = function () {
            var getDate = jDate.getFromAbsolute(self.AbsoluteDate);
            self.Year = getDate.year;
            self.Month = getDate.month;
            self.Day = getDate.day;
        };

        if (arg instanceof Date) {
            self.AbsoluteDate = jDate.absoluteFromSDate(arg);
            self.setFromAbsolute();
        }
        else if (arg instanceof String) {
            self.AbsoluteDate = jDate.absoluteFromSDate(Date.parse(arg));
            self.setFromAbsolute();
        }
        else if (typeof arg === 'number') {
            //if no month and day was supplied, we assume that the first argument is an absolute date
            if (typeof month === 'undefined') {
                self.AbsoluteDate = arg;
                self.setFromAbsolute();
            }
            else {
                self.Year = arg;
                self.Month = month;
                self.Day = day || 1; //If no day was supplied, we take the first day of the month
                self.AbsoluteDate = jDate.absoluteFromJDate(self.Year, self.Month, self.Day);
            }
        }
        else if (typeof arg === 'object' && typeof arg.year === 'number') {
            self.Day = arg.day || 1;
            self.Month = arg.month || 7;
            self.Year = arg.year;
            self.AbsoluteDate = jDate.absoluteFromJDate(self.Year, self.Month, self.Day);
        }
    }

    jDate.jewishMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini"];
    jDate.jewishMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"];
    jDate.daysOfWeekEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"];
    jDate.daysOfWeekHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"];
    jDate.singleDigits = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט'];
    jDate.tensDigits = ['י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ'];
    jDate.hundredsDigits = ['ק', 'ר', 'ש', 'ת'];
    jDate.singleNumbers = ["", "אחד", "שנים", "שלשה", "ארבעה", "חמשה", "ששה", "שבעה", "שמונה", "תשעה"];
    jDate.tensNumbers = ["", "עשר", "עשרים", "שלושים", "ארבעים"];

    jDate.getFromAbsolute = function (absDay) {
        //To save on calculations, start with a few years before date
        var year = 3761 + parseInt(absDay / (absDay > 0 ? 366 : 300)),
            month,
            day;

        // Search forward for year from the approximation year.
        while (absDay >= jDate.absoluteFromJDate(year + 1, 7, 1)) {
            year++;
        }
        // Search forward for month from either Tishrei or Nissan.
        month = (absDay < jDate.absoluteFromJDate(year, 1, 1) ? 7 : 1);
        while (absDay > jDate.absoluteFromJDate(year, month, jDate.daysInJewishMonth(year, month))) {
            month++;
        }
        // Calculate the day by subtraction.
        day = (absDay - jDate.absoluteFromJDate(year, month, 1) + 1);

        return { year: year, month: month, day: day };
    };

    jDate.absoluteFromSDate = function (date) {
        var year = date.getFullYear(),
            month = date.getMonth() + 1,
            numberOfDays = date.getDate(); // days this month
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
        var dayInYear = day; // Days so far this month.
        if (month < 7) { // Before Tishrei, so add days in prior months
            // this year before and after Nissan.
            var m = 7;
            while (m <= (jDate.monthsInJewishYear(year))) {
                dayInYear += jDate.daysInJewishMonth(year, m);
                m++;
            };
            m = 1;
            while (m < month) {
                dayInYear += jDate.daysInJewishMonth(year, m);
                m++;
            }
        }
        else { // Add days in prior months this year
            var m = 7;
            while (m < month) {
                dayInYear += jDate.daysInJewishMonth(year, m);
                m++;
            }
        }
        // Days elapsed before absolute date 1. -  Days in prior years.
        return dayInYear + (jDate.elapsedDays(year) + (-1373429));
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
        var months = parseInt((235 * parseInt((year - 1) / 19)) + // Leap months this cycle
                (12 * ((year - 1) % 19)) +                        // Regular months in this cycle.
                (7 * ((year - 1) % 19) + 1) / 19),                // Months in complete cycles so far.
            parts = 204 + 793 * (months % 1080),
            hours = 5 + 12 * months + 793 * parseInt(months / 1080) + parseInt(parts / 1080),
            conjDay = parseInt(1 + 29 * months + hours / 24),
            conjParts = 1080 * (hours % 24) + parts % 1080,
            altDay;
        /* at the end of a leap year -  15 hours, 589 parts or later... -
        ... or is on a Monday at... -  ...of a common year, -
        at 9 hours, 204 parts or later... - ...or is on a Tuesday... -
        If new moon is at or after midday,*/
        if ((conjParts >= 19440) ||
            (((conjDay % 7) == 2) && (conjParts >= 9924) && (!jDate.isJewishLeapYear(year))) ||
            (((conjDay % 7) == 1) && (conjParts >= 16789) && (jDate.isJewishLeapYear(year - 1)))) {
            // Then postpone Rosh HaShanah one day
            altDay = (conjDay + 1);
        }
        else {
            altDay = conjDay;
        }
        // or Friday -  or Wednesday, -  If Rosh HaShanah would occur on Sunday,
        if (((altDay % 7) == 0) || ((altDay % 7) == 3) || ((altDay % 7) == 5)) {
            // Then postpone it one (more) day
            return (1 + altDay);
        }
        else {
            return altDay;
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

    jDate.toJNumber = function (number) {
        if (number < 1) {
            throw new Error("Min value is 1");
        }

        if (number > 9999) {
            throw new Error("Max value is 9999");
        }

        var n = number,
            retval = '';

        if (n >= 1000) {
            retval += jDate.singleDigits[parseInt((n - (n % 1000)) / 1000) - 1] + "'";
            n = n % 1000;
        }

        while (n >= 400) {
            retval += 'ת';
            n -= 400;
        }

        if (n >= 100) {
            retval += jDate.hundredsDigits[parseInt((n - (n % 100)) / 100) - 1];
            n = n % 100;
        }

        if (n == 15) {
            retval += "טו";
        }
        else if (n == 16) {
            retval += "טז";
        }
        else {
            if (n > 9) {
                retval += jDate.tensDigits[parseInt((n - (n % 10)) / 10) - 1];
            }
            if ((n % 10) > 0) {
                retval += jDate.singleDigits[(n % 10) - 1];
            }
        }
        if (number > 999 && (number % 1000 < 10)) {
            retval = "'" + retval;
        }
        else if (retval.length > 1) {
            retval = (retval.slice(0, -1) + "\"" + retval[retval.length - 1]);
        }
        return retval;
    };
})();