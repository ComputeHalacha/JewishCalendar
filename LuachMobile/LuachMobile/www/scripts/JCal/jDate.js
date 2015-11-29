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
function jDate(arg, month, day) {
    var self = this;

    self.Day = NaN;
    self.Month = NaN;
    self.Year = NaN;
    self.AbsoluteDate = NaN;

    if (arg instanceof Date) {
        setFromAbsolute(jDate.absoluteFromSDate(arg));
    }
    else if (arg instanceof String) {
        setFromAbsolute(jDate.absoluteFromSDate(Date.parse(arg)));
    }
    else if (typeof arg === 'number') {
        //if no month and day was supplied, we assume that the first argument is an absolute date
        if (typeof month === 'undefined') {
            setFromAbsolute(arg);
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

    function setFromAbsolute(absolute) {
        var getDate = jDate.getFromAbsolute(absolute);
        self.Year = getDate.year;
        self.Month = getDate.month;
        self.Day = getDate.day;
        self.AbsoluteDate = absolute;
    };
}

jDate.prototype = {
    getSecularDate: function () {
        var dt = new Date(2000, 0, 1); // 1/1/2000 is absolute date 730120
        dt.setDate((this.AbsoluteDate - 730120) + 1);
        return dt;
    },
    getDayOfWeek: function () {
        return Math.abs(this.AbsoluteDate % 7);
    },
    addDays: function (days) {
        return new jDate(this.AbsoluteDate + days);
    },
    toString: function () {
        return jDate.daysOfWeekEng[this.getDayOfWeek()] + ' ' +
            jDate.jewishMonthsEng[this.Month] + ' ' +
            this.Day.toString() + ' ' +
            this.Year.toString();
    },
    toStringHeb: function () {
        return jDate.daysOfWeekHeb[this.getDayOfWeek()] + ' ' +
           jDate.toJNumber(this.Day) + ' ' +
           jDate.jewishMonthsHeb[this.Month] + ' ' +
           jDate.toJNumber(this.Year % 1000);
    },
    getDiff: function (jd) {
        return this.AbsoluteDate - jd.AbsoluteDate;
    },
    getDayOfOmer: function () {
        var dayOfOmer = 0;
        if ((this.Month == 1 && this.Day > 15) || this.Month == 2 || (this.Month == 3 && this.Day < 6)) {
            dayOfOmer = this.getDiff(new jDate(this.Year, 1, 15));
        }
        return dayOfOmer;
    },
    getHolidays: function (israel, hebrew) {
        return jDate.getHoldidays(this, israel, hebrew);
    },
    getSedra: function (israel) {
        return new Sedra(this, israel);
    }
};

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
jDate.getHoldidays = function (jd, israel, hebrew) {
    var list = [],
        jYear = jd.Year,
        jMonth = jd.Month,
        jDay = jd.Day,
        dayOfWeek = jd.DayOfWeek,
        isLeapYear = jDate.isJewishLeapYear(jYear),
        secDate = jd.getSecularDate();

    if (dayOfWeek === 5) {
        list.push(!hebrew ? "Erev Shabbos" : "ערב שבת");
    }
    else if (dayOfWeek === 6) {
        list.push(!hebrew ? "Shabbos Kodesh" : "שבת קודש");
        if (jMonth != 6 && jDay > 22 && jDay < 30)
            list.push(!hebrew ? "Shabbos Mevarchim" : "מברכים החודש");
    }
    if (jDay === 30) {
        var monthIndex = (jMonth === 12 && !isLeapYear) || jMonth === 13 ? 1 : jMonth + 1;
        list.push(!hebrew ? "Rosh Chodesh " + jDate.jewishMonthsEng[monthIndex] : "ראש חודש " + jDate.jewishMonthsHeb[monthIndex]);
    } else if (jDay === 1 && jMonth != 7) {
        list.push(!hebrew ? "Rosh Chodesh " + jDate.jewishMonthsEng[jMonth] : "ראש חודש " + jDate.jewishMonthsHeb[jMonth]);
    }
    //V'sain Tal U'Matar in Chutz La'aretz is according to the secular date
    if (secDate.getMonth() == 11 && !israel) {
        var sday = secDate.getDate();
        if (sday === 5 || sday === 6) {
            var nextYearIsLeap = jDate.isJewishLeapYear(jYear + 1);
            if (((sday == 5 && !nextYearIsLeap)) || (sday == 6 && nextYearIsLeap))
                list.push(!hebrew ? "V'sain Tal U'Matar" : "ותן טל ומטר");
        }
    }
    switch (jMonth) {
        case 1: //Nissan
            if (dayOfWeek === 6 && jDay > 7 && jDay < 15)
                list.push(!hebrew ? "Shabbos HaGadol" : "שבת הגדול");
            if (jDay === 12 && dayOfWeek === 4)
                list.push(!hebrew ? "Bedikas Chametz" : "בדיקת חמץ");
            else if (jDay === 13 && dayOfWeek != DayOfWeek.Friday)
                list.push(!hebrew ? "Bedikas Chametz" : "בדיקת חמץ");
            else if (jDay === 14)
                list.push(!hebrew ? "Erev Pesach" : "ערב פסח");
            else if (jDay === 15)
                list.push(!hebrew ? "First Day of Pesach" : "פסח - יום ראשון");
            else if (jDay === 16)
                list.push(israel ?
                    (!hebrew ? "Pesach - Chol HaMoed" : "פסח - חול המועד") :
                    (!hebrew ? "Pesach - Second Day" : "פסח - יום שני"));
            else if (jDay.In(17, 18, 19))
                list.push(!hebrew ? "Pesach - Chol Ha'moed - Erev Yomtov" : "פסח - חול המועד");
            else if (jDay === 20)
                list.push(!hebrew ? "Pesach - Chol Ha'moed - Erev Yomtov" : "פסח - חול המועד - ערב יו\"ט");
            else if (jDay === 21)
                list.push(!hebrew ? "7th Day of Pesach" : "שביעי של פסח");
            else if (jDay === 22 && !israel)
                list.push(!hebrew ? "Last Day of Pesach" : "אחרון של פסח");
            break;
        case 2: //Iyar
            if (dayOfWeek === 1 && jDay > 2 && jDay < 12) {
                list.push(!hebrew ? "Baha\"b" : "תענית שני קמא");
            }
            else if (dayOfWeek === 4 && jDay > 5 && jDay < 13) {
                list.push(!hebrew ? "Baha\"b" : "תענית חמישי");
            }
            else if (dayOfWeek === 1 && jDay > 9 && jDay < 17) {
                list.push(!hebrew ? "Baha\"b" : "תענית שני בתרא");
            }
            if (jDay === 14)
                list.push(!hebrew ? "Pesach Sheini" : "פסח שני");
            else if (jDay === 18)
                list.push(!hebrew ? "Lag BaOmer" : "ל\"ג בעומר");
            break;
        case 3: //Sivan
            if (jDay === 5)
                list.push(!hebrew ? "Erev Shavuos" : "ערב שבועות");
            else if (jDay === 6)
                list.push((israel ? (!hebrew ? "Shavuos" : "חג השבועות") :
                    (!hebrew ? "Shavuos - First Day" : "שבועות - יום ראשון")));
            if (jDay === 7 && !israel)
                list.push(!hebrew ? "Shavuos - Second Day" : "שבועות - יום שני");
            break;
        case 4: //Tamuz
            if (jDay === 17 && dayOfWeek !== 6) {
                list.push(!hebrew ? "Fast - 17th of Tammuz" : "צום י\"ז בתמוז");
            }
            else if (jDay === 18 && dayOfWeek === 0) {
                list.push(!hebrew ? "Fast - 17th of Tammuz" : "צום י\"ז בתמוז");
            } break;
        case 5: //Av
            if (jDay === 9 && dayOfWeek !== 6)
                list.push(!hebrew ? "Tisha B'Av" : "תשעה באב");
            else if (jDay === 10 && dayOfWeek === 0)
                list.push(!hebrew ? "Tisha B'Av" : "תשעה באב");
            else if (jDay === 15)
                list.push(!hebrew ? "Tu B'Av" : "ט\"ו באב");
            break;
        case 6: //Ellul
            if (jDay === 29)
                list.push(!hebrew ? "Erev Rosh Hashana" : "ערב ראש השנה");
            break;
        case 7: //Tishrei
            if (jDay === 1)
                list.push(!hebrew ? "Rosh Hashana - First Day" : "ראש השנה");
            else if (jDay === 2)
                list.push(!hebrew ? "Rosh Hashana - Second Day" : "ראש השנה");
            else if (jDay === 3 && dayOfWeek !== 6)
                list.push(!hebrew ? "Tzom Gedalia" : "צום גדליה");
            else if (jDay === 4 && dayOfWeek === 0)
                list.push(!hebrew ? "Tzom Gedalia" : "צום גדליה");
            else if (jDay === 9)
                list.push(!hebrew ? "Erev Yom Kippur" : "ערב יום הכיפורים");
            else if (jDay === 10)
                list.push(!hebrew ? "Yom Kippur" : "יום הכיפורים");
            else if (jDay === 14)
                list.push(!hebrew ? "Erev Sukkos" : "ערב חג הסוכות");
            else if (jDay === 15)
                list.push(!hebrew ? "First Day of Sukkos" : "חג הסוכות");
            else if (jDay === 16)
                list.push(israel ? (!hebrew ? "Sukkos - Chol HaMoed" : "סוכות - חול המועד") : (!hebrew ? "Sukkos - Second Day" : "יום שני - חג הסוכות"));
            else if (jDay.In(17, 18, 19, 20))
                list.push(!hebrew ? "Sukkos - Chol HaMoed" : "סוכות - חול המועד");
            else if (jDay === 21)
                list.push(!hebrew ? "Hoshana Rabba - Erev Yomtov" : "הושענא רבה - ערב יו\"ט");
            else if (jDay === 22) {
                list.push(!hebrew ? "Shmini Atzeres" : "שמיני עצרת");
                if (israel)
                    list.push(!hebrew ? "Simchas Torah" : "שמחת תורה");
            }
            else if (jDay === 23 && !israel)
                list.push(!hebrew ? "Simchas Torah" : "שמחת תורה");
            break;
        case 8: //Cheshvan
            if (dayOfWeek === 1 && jDay > 2 && jDay < 12) {
                list.push(!hebrew ? "Baha\"b" : "תענית שני קמא");
            }
            else if (dayOfWeek === 4 && jDay > 5 && jDay < 13) {
                list.push(!hebrew ? "Baha\"b" : "תענית חמישי");
            }
            else if (dayOfWeek === 1 && jDay > 9 && jDay < 17) {
                list.push(!hebrew ? "Baha\"b" : "תענית שני בתרא");
            }
            if (jDay === 7 && israel)
                list.push(!hebrew ? "V'sain Tal U'Matar" : "ותן טל ומטר");
            break;
        case 9: //Kislev
            if (jDay === 25)
                list.push(!hebrew ? "Chanuka - One Candle" : "'חנוכה - נר א");
            else if (jDay === 26)
                list.push(!hebrew ? "Chanuka - Two Candles" : "'חנוכה - נר ב");
            else if (jDay === 27)
                list.push(!hebrew ? "Chanuka - Three Candles" : "'חנוכה - נר ג");
            else if (jDay === 28)
                list.push(!hebrew ? "Chanuka - Four Candles" : "'חנוכה - נר ד");
            else if (jDay === 29)
                list.push(!hebrew ? "Chanuka - Five Candles" : "'חנוכה - נר ה");
            else if (jDay === 30)
                list.push(!hebrew ? "Chanuka - Six Candles" : "'חנוכה - נר ו");
            break;
        case 10: //Teves
            if (jDate.isShortKislev(jYear)) {
                if (jDay === 1)
                    list.push(!hebrew ? "Chanuka - Six Candles" : "'חנוכה - נר ו");
                else if (jDay === 2)
                    list.push(!hebrew ? "Chanuka - Seven Candles" : "'חנוכה - נר ז");
                else if (jDay === 3)
                    list.push(!hebrew ? "Chanuka - Eight Candles" : "'חנוכה - נר ח");
            }
            else {
                if (jDay === 1)
                    list.push(!hebrew ? "Chanuka - Seven Candles" : "'חנוכה - נר ז");
                else if (jDay === 2)
                    list.push(!hebrew ? "Chanuka - Eight Candles" : "'חנוכה - נר ח");
            }
            if (jDay === 10)
                list.push(!hebrew ? "Fast - 10th of Teves" : "צום עשרה בטבת");
            break;
        case 11: //Shvat
            if (jDay === 15)
                list.push(!hebrew ? "Tu B'Shvat" : "ט\"ו בשבט");
            break;
        case 12: //Adars case 13:
            if (jMonth === 12 && isLeapYear) {
                if (jDay === 14)
                    list.push(!hebrew ? "Purim Katan" : "פורים קטן");
                else if (jDay === 15)
                    list.push(!hebrew ? "Shushan Purim Katan" : "שושן פורים קטן");
            }
            else {
                if (jDay === 11 && dayOfWeek === 4)
                    list.push(!hebrew ? "Fast - Taanis Esther" : "תענית אסתר");
                else if (jDay === 13 && dayOfWeek !== 6)
                    list.push(!hebrew ? "Fast - Taanis Esther" : "תענית אסתר");
                else if (jDay === 14)
                    list.push(!hebrew ? "Purim" : "פורים");
                else if (jDay === 15)
                    list.push(!hebrew ? "Shushan Purim" : "שושן פורים");
            }
            break;
    }
    if ((jMonth === 1 && jDay > 15) || jMonth === 2 || (jMonth === 3 && jDay < 6)) {
        var dayOfSefirah = jd.getDayOfOmer();
        if (dayOfSefirah > 0) {
            list.push(!hebrew ? "Sefiras Ha'omer - Day " + dayOfSefirah.toString() : "ספירת העומר - יום " + dayOfSefirah.toString());
        }
    }

    return list;
}

//Gets an array of sedras for the given jewish date
function Sedra(jd, israel) {
    //If we are between the first day of Sukkos and Simchas Torah, the sedra will always be Vezos Habracha.
    if (jd.Month === 7 && jd.Day >= 15 && jd.Day < (israel ? 23 : 24)) {
        return [Sedra.sedraList[53]];
    }

    var sedraArray = [],
        sedraOrder = Sedra.getSedraOrder(jd.Year, israel),
        absDate = jd.AbsoluteDate,
        index,
        weekNum;

    /* find the first saturday on or after today's date */
    absDate = Sedra.getDayOnOrBefore(6, absDate + 6);

    weekNum = (absDate - sedraOrder.firstSatInYear) / 7;

    if (weekNum >= sedraOrder.sedraArray.length) {
        var indexLast = sedraOrder.sedraArray[sedraOrder.sedraArray.length - 1];
        if (indexLast < 0) {
            /* advance 2 parashiyot ahead after a doubled week */
            index = (-indexLast) + 2;
        }
        else {
            index = indexLast + 1;
        }
    }
    else {
        index = sedraOrder.sedraArray[weekNum];
    }

    if (index >= 0) {
        sedraArray = [Sedra.sedraList[index]];
    }
    else {
        var i = -index;      /* undouble the sedra */
        sedraArray = [Sedra.sedraList[i], Sedra.sedraList[i + 1]];
    }
    return sedraArray;
}

Sedra.lastCalculatedYear = null;

Sedra.sedraList = [{ eng: "Bereshis", heb: "בראשית" }, { eng: "Noach", heb: "נח" }, { eng: "Lech-Lecha", heb: "לך לך" }, { eng: "Vayera", heb: "וירא" }, { eng: "Chayei Sara", heb: "חיי שרה" }, { eng: "Toldos", heb: "תולדות" }, { eng: "Vayetzei", heb: "ויצא" }, { eng: "Vayishlach", heb: "וישלח" }, { eng: "Vayeishev", heb: "וישב" }, { eng: "Mikeitz", heb: "מקץ" }, { eng: "Vayigash", heb: "ויגש" }, { eng: "Vayechi", heb: "ויחי" }, { eng: "Shemos", heb: "שמות" }, { eng: "Va'era", heb: "וארא" }, { eng: "Bo", heb: "בא" }, { eng: "Beshalach", heb: "בשלח" }, { eng: "Yisro", heb: "יתרו" }, { eng: "Mishpatim", heb: "משפטים" }, { eng: "Terumah", heb: "תרומה" }, { eng: "Tetzaveh", heb: "תצוה" }, { eng: "Ki Sisa", heb: "כי תשא" }, { eng: "Vayakhel", heb: "ויקהל" }, { eng: "Pekudei", heb: "פקודי" }, { eng: "Vayikra", heb: "ויקרא" }, { eng: "Tzav", heb: "צו" }, { eng: "Shmini", heb: "שמיני" }, { eng: "Tazria", heb: "תזריע" }, { eng: "Metzora", heb: "מצורע" }, { eng: "Achrei Mos", heb: "אחרי מות" }, { eng: "Kedoshim", heb: "קדושים" }, { eng: "Emor", heb: "אמור" }, { eng: "Behar", heb: "בהר" }, { eng: "Bechukosai", heb: "בחקותי" }, { eng: "Bamidbar", heb: "במדבר" }, { eng: "Nasso", heb: "נשא" }, { eng: "Beha'aloscha", heb: "בהעלתך" }, { eng: "Sh'lach", heb: "שלח" }, { eng: "Korach", heb: "קרח" }, { eng: "Chukas", heb: "חקת" }, { eng: "Balak", heb: "בלק" }, { eng: "Pinchas", heb: "פינחס" }, { eng: "Matos", heb: "מטות" }, { eng: "Masei", heb: "מסעי" }, { eng: "Devarim", heb: "דברים" }, { eng: "Va'eschanan", heb: "ואתחנן" }, { eng: "Eikev", heb: "עקב" }, { eng: "Re'eh", heb: "ראה" }, { eng: "Shoftim", heb: "שופטים" }, { eng: "Ki Seitzei", heb: "כי תצא" }, { eng: "Ki Savo", heb: "כי תבא" }, { eng: "Nitzavim", heb: "נצבים" }, { eng: "Vayeilech", heb: "וילך" }, { eng: "Ha'Azinu", heb: "האזינו" }, { eng: "Vezos Habracha", heb: "וזאת הברכה" }];
Sedra.shabbos_short = [52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26, -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.shabbos_long = [52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26, -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.mon_short = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26, -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.mon_long = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26, -28, 30, -31, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.thu_normal = [52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, 25, -26, -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.thu_normal_Israel = [52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, -21, 23, 24, 25, 25, -26, -28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.thu_long = [52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 25, -26, -28, 30, -31, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.shabbos_short_leap = [52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.shabbos_long_leap = [52, 52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.mon_short_leap = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 29, 30, 31, 32, 33, 34, 34, 35, 36, 37, -38, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.mon_short_leap_Israel = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, -50];
Sedra.mon_long_leap = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, -41, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.mon_long_leap_Israel = [51, 52, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.thu_short_leap = [52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50];
Sedra.thu_long_leap = [52, 53, 53, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, -50];

Sedra.getDayOnOrBefore = function (day_of_week, date) {
    return date - ((date - day_of_week) % 7);
};
Sedra.getSedraOrder = function (year, israel) {
    //If the last call is within the same year as this one, we reuse the data.
    //If memory is an issue, remove these next few lines
    if (Sedra.lastCalculatedYear != null && Sedra.lastCalculatedYear.year === year && Sedra.lastCalculatedYear.israel === israel) {
        return Sedra.lastCalculatedYear;
    }

    var longCheshvon = jDate.isLongCheshvan(year),
        shortKislev = jDate.isShortKislev(year),
        roshHashana = jDate.absoluteFromJDate(year, 7, 1),
        roshHashanaDOW = Math.abs(roshHashana % 7),
        firstSatInYear = Sedra.getDayOnOrBefore(6, roshHashana + 6),
        yearType,
        sArray;

    if (longCheshvon && !shortKislev)
        yearType = 'complete';
    else if (!longCheshvon && shortKislev)
        yearType = 'incomplete';
    else
        yearType = 'regular';

    if (!jDate.isJewishLeapYear(year)) {
        switch (roshHashanaDOW) {
            case 6:
                if (yearType === "incomplete") {
                    sArray = Sedra.shabbos_short;
                }
                else if (yearType === 'complete') {
                    sArray = Sedra.shabbos_long;
                }
                break;

            case 1:
                if (yearType === 'incomplete') {
                    sArray = Sedra.mon_short;
                }
                else if (yearType === 'complete') {
                    sArray = israel ? Sedra.mon_short : Sedra.mon_long;
                }
                break;

            case 2:
                if (yearType === 'regular') {
                    sArray = israel ? Sedra.mon_short : Sedra.mon_long;
                }
                break;

            case 4:
                if (yearType === 'regular') {
                    sArray = israel ? Sedra.thu_normal_Israel : Sedra.thu_normal;
                }
                else if (yearType === 'complete') {
                    sArray = Sedra.thu_long;
                }
                break;

            default:
                throw new Error("improper sedra year type calculated.");
        }
    }
    else  /* leap year */ {
        switch (roshHashanaDOW) {
            case 6:
                if (yearType === 'incomplete') {
                    sArray = Sedra.shabbos_short_leap;
                }
                else if (yearType === 'complete') {
                    sArray = israel ? Sedra.shabbos_short_leap : Sedra.shabbos_long_leap;
                }
                break;

            case 1:
                if (yearType === 'incomplete') {
                    sArray = israel ? Sedra.mon_short_leap_Israel : Sedra.mon_short_leap;
                }
                else if (yearType === 'complete') {
                    sArray = israel ? Sedra.mon_long_leap_Israel : Sedra.mon_long_leap;
                }
                break;

            case 2:
                if (yearType === 'regular') {
                    sArray = israel ? Sedra.mon_long_leap_Israel : Sedra.mon_long_leap;
                }
                break;

            case 4:
                if (yearType === 'incomplete') {
                    sArray = Sedra.thu_short_leap;
                }
                else if (yearType === 'complete') {
                    sArray = Sedra.thu_long_leap;
                }
                break;

            default:
                throw new Error("improper sedra year type calculated.");
        }
    }

    var retobj = {
        firstSatInYear: firstSatInYear,
        sedraArray: sArray,
        year: year,
        israel: israel
    };

    //Save the data in case the next call is for the same year
    Sedra.lastCalculatedYear = retobj;

    return retobj;
};