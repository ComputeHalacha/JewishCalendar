/// <reference path="Dafyomi.js" />
/// <reference path="Utils.js" />
/// <reference path="Sedra.js" />
"use strict";
/******************************************************************************************************************************
 *  Represents a single day in the Jewish Calendar.
 *
 *  Many of the algorithms were taken by CBS from the C code  which was translated from the Lisp code
 *  in "Calendrical Calculations" by Nachum Dershowitz and Edward M. Reingold
 *  in Software---Practice & Experience, vol. 20, no. 9 (September, 1990), pp. 899--928.
 *
 *  Create a jDate with any of the following:
 *  new jDate(javascriptDateObject) - Sets to the Jewish date on the given Gregorian date
 *  new jDate("January 1 2045") - Accepts any valid javascript Date string (uses javascripts new Date(String))
 *  new jDate(jewishYear, jewishMonth, jewishDay) - Months start at 1. Nissan is month 1 Adara Sheini is 12.
 *  new jDate(jewishYear, jewishMonth) - Same as above, with Day defaulting to 1
 *  new jDate(absoluteDate) - The number of days elapsed since the theoretical date Sunday, December 31, 0001 BCE
 *  new jDate( { year: 5776, month: 4, day: 5 } ) - same as new jDate(jewishYear, jewishMonth, jewishDay)
 *  new jDate( { year: 5776, month: 4 } ) - same as new jDate(jewishYear, jewishMonth)
 *  new jDate( { year: 5776 } ) - sets to the first day of Rosh Hashana on the given year
 *****************************************************************************************************************************/
function jDate(arg, month, day) {
    var self = this;

    //The day of the Jewish Month
    self.Day = NaN;
    //The Jewish Month. As in the torah, Nissan is 1 and Adara Sheini is 13
    self.Month = NaN;
    //The Number of years since the creation of the world
    self.Year = NaN;
    //The number of days since the theoretical date: Dec. 31, 0001 BCE
    self.Abs = NaN;

    if (arg instanceof Date) {
        if (arg.isvalid()) {
            fromAbs(jDate.absSd(arg));
        }
        else {
            throw new Error('jDate constructor: The given Date is not a valid javascript Date');
        }
    }
    else if (Utils.isString(arg)) {
        var d = new Date(arg);
        if (d.isvalid()) {
            fromAbs(jDate.absSd(d));
        }
        else {
            throw new Error('jDate constructor: The given string "' + arg + '" cannot be parsed into a Date');
        }
    }
    else if (Utils.isNumber(arg)) {
        //if no month and day was supplied, we assume that the first argument is an absolute date
        if (typeof month === 'undefined') {
            fromAbs(arg);
        }
        else {
            self.Year = arg;
            self.Month = month;
            self.Day = day || 1; //If no day was supplied, we take the first day of the month
            self.Abs = jDate.absJd(self.Year, self.Month, self.Day);
        }
    }
    else if (typeof arg === 'object' && typeof arg.year === 'number') {
        self.Day = arg.day || 1;
        self.Month = arg.month || 7;
        self.Year = arg.year;
        self.Abs = jDate.absJd(self.Year, self.Month, self.Day);
    }

    //Sets the current Jewish date from the given absolute date
    function fromAbs(absolute) {
        var date = jDate.fromAbs(absolute);
        self.Year = date.year;
        self.Month = date.month;
        self.Day = date.day;
        self.Abs = absolute;
    };
}

jDate.prototype = {
    //Returns a valid javascript Date object that represents the Gregorian date that starts at midnight of the current Jewish date
    getDate: function () {
        var dt = new Date(2000, 0, 1); // 1/1/2000 is absolute date 730120
        dt.setDate((this.Abs - 730120) + 1);
        return dt;
    },

    //The day of the week for the current Jewish date. Sunday is 0 and Shabbos is 6
    getDayOfWeek: function () {
        return Math.abs(this.Abs % 7);
    },

    //Returns a new Jewish date represented by adding the given number of days to the current Jewish date
    addDays: function (days) {
        return new jDate(this.Abs + days);
    },

    //Returns a new Jewish date represented by adding the given number of Jewish Months to the current Jewish date
    addMonths: function (months) {
        var year = this.Year,
            month = this.Month,
            day = this.Day,
            miy = jDate.monthsJYear(year);

        for (var i = 0; i < Math.abs(months) ; i++) {
            if (months > 0) {
                month += 1;
                if (month > miy) {
                    month = 1;
                }
                if (month === 7) {
                    year += 1;
                    miy = jDate.monthsJYear(year);
                }
            }
            else if (months < 0) {
                month -= 1;
                if (month === 0) {
                    month = miy;
                }
                if (month === 6) {
                    year -= 1;
                    miy = jDate.monthsJYear(year);
                }
            }
        }
        return new jDate(year, month, day);
    },

    //Returns a new Jewish date represented by adding the given number of Jewish Years to the current Jewish date
    addYears: function (years) {
        return new jDate(this.Year + years, this.Month, this.Day);
    },

    //Returns the current Jewish date in the format: Thursday Kislev 3 5776
    toString: function () {
        return Utils.dowEng[this.getDayOfWeek()] + ' ' +
            Utils.jMonthsEng[this.Month] + ' ' +
            this.Day.toString() + ' ' +
            this.Year.toString();
    },

    //Returns the current Jewish date in the format: יום חמישי כ"א כסלו תשע"ו
    toStringHeb: function () {
        return Utils.dowHeb[this.getDayOfWeek()] + ' ' +
           Utils.toJNum(this.Day) + ' ' +
           Utils.jMonthsHeb[this.Month] + ' ' +
           Utils.toJNum(this.Year % 1000);
    },

    //Gets the difference in days between the current Jewish date and the given one.
    //If the given date is earlier, it will be a negative number.
    getDiff: function (jd) {
        return this.Abs - jd.Abs;
    },
    //Gets the day of the omer for the current Jewish date. If the date is not during sefira, 0 is returned.
    getDayOfOmer: function () {
        var dayOfOmer = 0;
        if ((this.Month == 1 && this.Day > 15) || this.Month == 2 || (this.Month == 3 && this.Day < 6)) {
            dayOfOmer = this.getDiff(new jDate(this.Year, 1, 15));
        }
        return dayOfOmer;
    },

    //Gets an array[string] of holidays, fasts and any other special specifications for the current Jewish date.
    getHolidays: function (israel, hebrew) {
        return jDate.getHoldidays(this, israel, hebrew);
    },

    //Does the current Jewish date need candle lighting before sunset?
    hasCandleLighting: function () {
        var dow = this.getDayOfWeek();

        if (dow === 5) {
            return true;
        }
        else if (dow === 6) {
            //there is no "candle lighting time" - even if yom tov is on Motzai Shabbos
            return false;
        }

        return (this.Month === 1 && [14, 20].has(this.Day)) ||
               (this.Month === 3 && this.Day === 5) ||
               (this.Month === 6 && this.Day === 29) ||
               (this.Month === 7 && [9, 14, 21].has(this.Day));
    },

    //Gets the candle lighting time for the current Jewish date for the given Location object.
    getCandleLighting: function (location) {
        if (!location) {
            throw new Error('To get sunrise and sunset, the location needs to be supplied');
        }
        if (this.hasCandleLighting()) {
            return Zmanim.getCandleLighting(this, location);
        }
        else {
            throw new Error("No candle lighting on " + jd.toString());
        }
    },

    //Get the sedra of the week for the current Jewish date
    getSedra: function (israel) {
        return new Sedra(this, israel);
    },

    //gets sunrise and sunset time for the current Jewish date at the given Location.
    //Return format: { sunrise: { hour: 6, minute: 18 }, sunset: { hour: 19, minute: 41 } }
    getSunriseSunset: function (location) {
        if (!location) {
            throw new Error('To get sunrise and sunset, the location needs to be supplied');
        }
        return Zmanim.getSunTimes(this, location);
    },

    //Gets Chatzos for both the day and the night for the current Jewish date at the given Location.
    //Return format: { hour: 11, minute: 48 }
    getChatzos: function (location) {
        if (!location) {
            throw new Error('To get Chatzos, the location needs to be supplied');
        }
        return Zmanim.getChatzos(this, location);
    },

    //Gets the length of a single Sha'a Zmanis for the current Jewish date at the given Location.
    getShaaZmanis: function (location, offset) {
        if (!location) {
            throw new Error('To get the Shaa Zmanis, the location needs to be supplied');
        }
        return Zmanim.getShaaZmanis(this, location, offset);
    },

    // Returns the daily daf in English. For example: Sukkah, Daf 3
    getDafyomi: function () {
        return Dafyomi.toString(this);
    },

    //Gets the daily daf in Hebrew. For example: 'סוכה דף כ.
    getDafyomiHeb: function () {
        return Dafyomi.toStringHeb(this);
    }
};

/***************************************************************************************************************
*  Converts to a Jewish Date. Sample of use - to get the current Jewish Date: jDate.toJDate(new Date()). 
*  To print out the current date in English: jDate.toJDate(new Date()).toString() and in Hebrew: jDate.toJDate(new Date()).toStringHeb()
*  Arguments to the jDate.toJDate function can be any of the following:
*  jDate.toJDate(Date) - Sets to the Jewish date on the given Javascript Date object
*  jDate.toJDate("January 1 2045") - Accepts any valid Javascript Date string (uses string constructor of Date object)
*  jDate.toJDate(jewishYear, jewishMonth, jewishDay) - Months start at 1. Nissan is month 1 Adara Sheini is 13.
*  jDate.toJDate(jewishYear, jewishMonth) - Same as above, with Day defaulting to 1
*  jDate.toJDate(jewishYear) - sets to the first day of Rosh Hashana on the given year 
*  jDate.toJDate( { year: 5776, month: 4, day: 5 } ) - Months start at 1. Nissan is month 1 Adara Sheini is 13.
*  jDate.toJDate( { year: 5776, month: 4 } ) - Same as above, with Day defaulting to 1
*  jDate.toJDate( { year: 5776 } ) - sets to the first day of Rosh Hashana on the given year 
****************************************************************************************************************/
jDate.toJDate = function (arg, month, year) {
    if (Utils.isNumber(arg) && typeof month === 'undefined' && typeof day === 'undefined') {
        return new jDate(1, 1, year);
    }
    else {
        return new jDate(arg, month, year);
    }
};

//Calulate the Jewish date at the given absolute date
jDate.fromAbs = function (absDay) {
    //To save on calculations, start with a few years before date
    var year = 3761 + parseInt(absDay / (absDay > 0 ? 366 : 300)),
        month,
        day;

    // Search forward for year from the approximation year.
    while (absDay >= jDate.absJd(year + 1, 7, 1)) {
        year++;
    }
    // Search forward for month from either Tishrei or Nissan.
    month = (absDay < jDate.absJd(year, 1, 1) ? 7 : 1);
    while (absDay > jDate.absJd(year, month, jDate.daysJMonth(year, month))) {
        month++;
    }
    // Calculate the day by subtraction.
    day = (absDay - jDate.absJd(year, month, 1) + 1);

    return { year: year, month: month, day: day };
};

//Gets the absolute date of the given javascript Date object
jDate.absSd = function (date) {
    var year = date.getFullYear(),
        month = date.getMonth() + 1,
        numberOfDays = date.getDate(); // days this month
    // add days in prior months this year
    for (var i = month - 1; i > 0; i--) {
        numberOfDays += jDate.daysSMonth(i, year);
    }

    return (numberOfDays          // days this year
           + 365 * (year - 1)     // days in previous years ignoring leap days
           + parseInt((year - 1) / 4)       // Julian leap days before this year...
           - parseInt((year - 1) / 100)     // ...minus prior century years...
           + parseInt((year - 1) / 400));   // ...plus prior years divisible by 400
};

//Calculate the absolute date for the given Jewish Date
jDate.absJd = function (year, month, day) {
    var dayInYear = day; // Days so far this month.
    if (month < 7) { // Before Tishrei, so add days in prior months
        // this year before and after Nissan.
        var m = 7;
        while (m <= (jDate.monthsJYear(year))) {
            dayInYear += jDate.daysJMonth(year, m);
            m++;
        };
        m = 1;
        while (m < month) {
            dayInYear += jDate.daysJMonth(year, m);
            m++;
        }
    }
    else { // Add days in prior months this year
        var m = 7;
        while (m < month) {
            dayInYear += jDate.daysJMonth(year, m);
            m++;
        }
    }
    // Days elapsed before absolute date 1. -  Days in prior years.
    return dayInYear + (jDate.tDays(year) + (-1373429));
};

//The number of days in the given Gregorian Month.
//Note: For the month parameter, January should be 1 and December should be 12
//This is unlike Javascripts getMonth() function which returns 0 for January and 11 for December.
jDate.daysSMonth = function (month, year) {
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

//The number of days in the given Jewish Month
jDate.daysJMonth = function (year, month) {
    if ((month == 2) || (month == 4) || (month == 6) || ((month == 8) &&
                (!jDate.isLongCheshvan(year))) || ((month == 9) && jDate.isShortKislev(year)) || (month == 10) || ((month == 12) &&
                (!jDate.isJdLeapY(year))) || (month == 13)) {
        return 29;
    }
    else {
        return 30;
    }
};

//Elapsed days
jDate.tDays = function (year) {
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
        (((conjDay % 7) == 2) && (conjParts >= 9924) && (!jDate.isJdLeapY(year))) ||
        (((conjDay % 7) == 1) && (conjParts >= 16789) && (jDate.isJdLeapY(year - 1)))) {
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

//Number of days in the given Jewish Year
jDate.daysJYear = function (year) {
    return ((jDate.tDays(year + 1)) - (jDate.tDays(year)));
};

//Number of days in the given Jewish Month. Nissan is 1 and Adar Sheini is 13.
jDate.daysJMonth = function (year, month) {
    if ((month == 2) || (month == 4) || (month == 6) || ((month == 8) &&
        (!jDate.isLongCheshvan(year))) || ((month == 9) && jDate.isShortKislev(year)) || (month == 10) || ((month == 12) &&
        (!jDate.isJdLeapY(year))) || (month == 13)) {
        return 29;
    }
    else {
        return 30;
    }
};

//Does Cheshvan for the given Jewish Year have 30 days?
jDate.isLongCheshvan = function (year) {
    return (jDate.daysJYear(year) % 10) == 5;
};

//Does Kislev for the given Jewish Year have 29 days?
jDate.isShortKislev = function (year) {
    return (jDate.daysJYear(year) % 10) == 3;
};

//Does the given Jewish Year have 13 months?
jDate.isJdLeapY = function (year) {
    return (((7 * year) + 1) % 19) < 7;
};

//Number of months in Jewish Year
jDate.monthsJYear = function (year) {
    return jDate.isJdLeapY(year) ? 13 : 12;
};

//Gets an array[string] of holidays, fasts and any other special specifications for the given Jewish date.
jDate.getHoldidays = function (jd, israel, hebrew) {
    var list = [],
        jYear = jd.Year,
        jMonth = jd.Month,
        jDay = jd.Day,
        dayOfWeek = jd.getDayOfWeek(),
        isLeapYear = jDate.isJdLeapY(jYear),
        secDate = jd.getDate();

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
        list.push(!hebrew ? "Rosh Chodesh " + Utils.jMonthsEng[monthIndex] : "ראש חודש " + Utils.jMonthsHeb[monthIndex]);
    } else if (jDay === 1 && jMonth != 7) {
        list.push(!hebrew ? "Rosh Chodesh " + Utils.jMonthsEng[jMonth] : "ראש חודש " + Utils.jMonthsHeb[jMonth]);
    }
    //V'sain Tal U'Matar in Chutz La'aretz is according to the secular date
    if (dayOfWeek !== 6 && (!israel) && secDate.getMonth() == 11) {
        var sday = secDate.getDate();
        //The three possible dates for starting vt"u are the 5th, 6th and 7th of December
        if ([5, 6, 7].has(sday)) {
            var nextYearIsLeap = jDate.isJdLeapY(jYear + 1);
            //If next year is not a leap year, then vst"u starts on the 5th.
            //If next year is a leap year than vst"u starts on the 6th.
            //If the 5th or 6th were shabbos than vst"u starts on the following day - Sunday.
            if ((((sday === 5 || (sday === 6 && dayOfWeek === 0)) && (!nextYearIsLeap))) ||
                ((sday === 6 || (sday === 7 && dayOfWeek === 0)) && nextYearIsLeap))
                list.push(!hebrew ? "V'sain Tal U'Matar" : "ותן טל ומטר");
        }
    }
    switch (jMonth) {
        case 1: //Nissan
            if (dayOfWeek === 6 && jDay > 7 && jDay < 15)
                list.push(!hebrew ? "Shabbos HaGadol" : "שבת הגדול");
            if (jDay === 12 && dayOfWeek === 4)
                list.push(!hebrew ? "Bedikas Chametz" : "בדיקת חמץ");
            else if (jDay === 13 && dayOfWeek !== 5)
                list.push(!hebrew ? "Bedikas Chametz" : "בדיקת חמץ");
            else if (jDay === 14)
                list.push(!hebrew ? "Erev Pesach" : "ערב פסח");
            else if (jDay === 15)
                list.push(!hebrew ? "First Day of Pesach" : "פסח - יום ראשון");
            else if (jDay === 16)
                list.push(israel ?
                    (!hebrew ? "Pesach - Chol HaMoed" : "פסח - חול המועד") :
                    (!hebrew ? "Pesach - Second Day" : "פסח - יום שני"));
            else if ([17, 18, 19].has(jDay))
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
            else if ([17, 18, 19, 20].has(jDay))
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
        case 12: //Both Adars
        case 13:
            if (jMonth === 12 && isLeapYear) { //Adar Rishon in a leap year
                if (jDay === 14)
                    list.push(!hebrew ? "Purim Katan" : "פורים קטן");
                else if (jDay === 15)
                    list.push(!hebrew ? "Shushan Purim Katan" : "שושן פורים קטן");
            }
            else { //The "real" Adar: the only one in a non-leap-year or Adar Sheini
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
    //If it is during Sefiras Ha'omer
    if ((jMonth === 1 && jDay > 15) || jMonth === 2 || (jMonth === 3 && jDay < 6)) {
        var dayOfSefirah = jd.getDayOfOmer();
        if (dayOfSefirah > 0) {
            list.push(!hebrew ? "Sefiras Ha'omer - Day " + dayOfSefirah.toString() : "ספירת העומר - יום " + dayOfSefirah.toString());
        }
    }

    return list;
};