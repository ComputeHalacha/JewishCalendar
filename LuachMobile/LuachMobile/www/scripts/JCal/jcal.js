/// <reference path="Zmanim.js" />
"use strict";

//Returns whether or not the array contains the given item
Array.prototype.has = function (item) {
    return !!~this.indexOf(item); //A cute trick: bitwise NOT turns -1 into 0
};

//Calls the given comparer function for each item in the array.
//The first item encountered for which the comparer returns truthy is returned.
Array.prototype.first = function (comparer) {
    for (var i = 0; i < this.length; i++) {
        if (comparer(this[i])) {
            return this[i];
        }
    }
};

//Get first instance of the given item in the given array.
//Search uses strict comparison operator (===) unless we are dealing with strings and caseSensitive is falsey.
//Note: for non-caseSensitive searches, returns the original array item if a match is found.
Array.prototype.getFirst = function (item, caseSensitive) {
    for (var i = 0; i < this.length; i++) {
        if ((!caseSensitive) && Utils.isString(item) && Utils.isString(this[i]) && item.toLowerCase() === this[i].toLowerCase()) {
            return this[i];
        }
        else if (this[i] === item) {
            return this[i];
        }
    }
};

//Checks a Date object if it represents a valid date or not
Date.prototype.isvalid = function () {
    return (!isNaN(this.valueOf()));
};

function Utils() { }
Utils.jMonthsEng = ["", "Nissan", "Iyar", "Sivan", "Tamuz", "Av", "Ellul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shvat", "Adar", "Adar Sheini"];
Utils.jMonthsHeb = ["", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול", "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר שני"];
Utils.dowEng = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Erev Shabbos", "Shabbos Kodesh"];
Utils.dowHeb = ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "ערב שבת קודש", "שבת קודש"];
Utils.jsd = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט'];
Utils.jtd = ['י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ'];
Utils.jhd = ['ק', 'ר', 'ש', 'ת'];
Utils.jsnum = ["", "אחד", "שנים", "שלשה", "ארבעה", "חמשה", "ששה", "שבעה", "שמונה", "תשעה"];
Utils.jtnum = ["", "עשר", "עשרים", "שלושים", "ארבעים"];

//Returns true if thing is an instance of either a string primitive or String object
Utils.isString = function (thing) {
    return (typeof thing === 'string' || thing instanceof String);
};

//Gets the Jewish representation of a number (365 = שס"ה)
//Minimum number is 1 and maximum is 9999.
Utils.toJNum = function (number) {
    if (number < 1) {
        throw new Error("Min value is 1");
    }

    if (number > 9999) {
        throw new Error("Max value is 9999");
    }

    var n = number,
        retval = '';

    if (n >= 1000) {
        retval += Utils.jsd[parseInt((n - (n % 1000)) / 1000) - 1] + "'";
        n = n % 1000;
    }

    while (n >= 400) {
        retval += 'ת';
        n -= 400;
    }

    if (n >= 100) {
        retval += Utils.jhd[parseInt((n - (n % 100)) / 100) - 1];
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
            retval += Utils.jtd[parseInt((n - (n % 10)) / 10) - 1];
        }
        if ((n % 10) > 0) {
            retval += Utils.jsd[(n % 10) - 1];
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

//gets the "real" system UTC offset in hours (not affected by DST)
Utils.currUtcOffset = function () {
    var date = new Date(),
        jan = new Date(date.getFullYear(), 0, 1),
        jul = new Date(date.getFullYear(), 6, 1);
    return parseInt(Math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset()) / 60);
};

//Determines if date (or now) is DST for the current system time zone
Utils.isDST = function (date) {
    date = date || new Date();
    return parseInt(date.getTimezoneOffset() / 60) < Utils.currUtcOffset();
};

//Determines if the given date and hour are during DST (using USA rules)
Utils.isUSA_DST = function (date, hour) {
    var year = date.getYear(),
        month = date.getMonth() + 1,
        day = date.getDate();

    if (month < 3 || month == 12) {
        return false;
    }
    else if (month > 3 && month < 11) {
        return true;
    }

        //DST starts at 2:00 AM on the second Sunday in March
    else if (month == 3) {
        //Gets day of week on March 1st
        var firstDOW = Zmanim.getDOW(year, 3, 1),
        //Gets date of second Sunday
            targetDate = firstDOW == 0 ? 8 : ((7 - (firstDOW + 7) % 7)) + 8;

        return (day > targetDate || (day == targetDate && hour >= 2));
    }
        //DST ends at 2:00 AM on the first Sunday in November
    else //dt.Month == 11
    {
        //Gets day of week on November 1st
        var firstDOW = Zmanim.getDOW(year, 11, 1),
        //Gets date of first Sunday
            targetDate = firstDOW == 0 ? 1 : ((7 - (firstDOW + 7) % 7)) + 1;

        return (day < targetDate || (day == targetDate && hour < 2));
    }
};

Utils.isIsrael_DST = function (date) {
    date = (date || new Date());

    //get the number of hours the current users time zone is off from Israels
    var israelTimeOffset = (2 - -(date.getTimezoneOffset() / 60));

    //This will give us the current correct date and time in Israel
    date.setHours(date.getHours + israelTimeOffset);

    var month = date.getMonth() + 1,
        day = date.getDate();
    if ([11, 12, 1, 2].has(month)) {
        return false;
    }
    else if ([4, 5, 6, 7, 8, 9].has(month)) {
        return true;
    }
        //DST starts at midnight on the Friday before the last Sunday in March
    else if (month == 3) {
        //Gets day of week on March 31st
        var lastDOW = Zmanim.getDOW(year, 3, 31),
        //Gets date of last Sunday
            lastSunday = lastDOW === 0 ? 31 : ((7 - (lastDOW + 7) % 7)) + 8;

        return day >= (lastSunday - 2);
    }
        //DST ends at Midnight on the last Sunday in October
    else //dt.Month == 10
    {
        //Gets day of week on November 30th
        var lastDOW = Zmanim.getDOW(year, 11, 30),
        //Gets date of last Sunday
            lastSunday = lastDOW === 0 ? 30 : ((7 - (lastDOW + 7) % 7)) + 8;

        return day < targetDate;
    }
}

//Gets the time difference between two times of day
Utils.timeDiff = function (time1, time2) {
    return Zmanim.fixHourMinute(Zmanim.addMinutes(time1, Utils.totalMinutes(time2)));
};

//Gets the total number of minutes in the given time
Utils.totalMinutes = function (time) {
    return time.hour * 60 + time.minutes;
};
/// <reference path="Utils.js" />
"use strict";

//Represents a geographic Location. Needed for calculating Zmanim.
//If Israel is undefined, if the location is in the very near vicinity of Israel it will be assumed that it is in Israel.
//UTCOffset is the time zone. Israel is always 2 and the US East coast is -5. England is 0 of course.
//If UTCOffset is not specifically supplied, the longitude will be used to get an educated guess.
function Location(name, israel, latitude, longitude, utcOffset, elevation, isDST) {
    if (typeof israel === 'undefined') {
        //Israel general coordinates (we are pretty safe even if we are off by a few miles,
        //where else is the (99.99% Jewish) user? Sinai, Lebanon, Syria, Jordan ...
        israel = (latitude > 29.45 && latitude < 33 && longitude < -34.23 && longitude > -35.9);
    }
    if (israel) {
        //Israel has only one immutable time zone
        utcOffset = 2;
    }
    else if (typeof utcOffset === 'undefined') {
        //Determine the "correct" time zone using the simple fact that Greenwich is both TZ 0 and longitude 0
        //Even though technically this is the way it should be, it will be often incorrect as time zones are almost always moved to the closest border.
        utcOffset = -parseInt(longitude / 15);
        if (typeof isDST === 'undefined') {
            //It's bad enough that we needed to guess the time zone
            isDST = false;
        }
    }
    //If "isDST" was not defined
    if (typeof isDST === 'undefined') {
        isDST = israel ? Utils.isIsrael_DST() : Utils.isDST();
    }

    return {
        Name: name || 'Unknown Location',
        Israel: !!israel,
        Latitude: latitude,
        Longitude: longitude,
        UTCOffset: utcOffset || 0,
        Elevation: elevation || 0,
        IsDST: !!isDST
    };
}

Location.getJerusalem = function () {
    return new Location("Jerusalem", true, 31.78, -35.22, 2, 800);
};
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
 *  new Date("January 1 2045") - Accepts any valid javascript Date string (uses javascripts new Date(String))
 *  new jDate(jewishYear, jewishMonth, jewishDay) - Months start at 1. Nissan is month 1 Adara Sheini is 12.
 *  new jDate(jewishYear, jewishMonth) - Same as above, with Day defaulting to 1
 *  new Date(absoluteDate) - The number of days elapsed since the theoretical date Sunday, December 31, 0001 BCE
 *  new Date( { year: 5776, month: 4, day: 5 } ) - same as new jDate(jewishYear, jewishMonth, jewishDay)
 *  new Date( { year: 5776, month: 4 } ) - same as new jDate(jewishYear, jewishMonth)
 *  new Date( { year: 5776 } ) - sets to the first day of Rosh Hashana on the given year *
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
    else if (typeof arg === 'number') {
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
                ((sday === 6 || (sday === 7 && dayofweek === 0)) && nextYearIsLeap))
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
/// <reference path="Utils.js" />
/// <reference path="jDate.js" />
"use strict";

/* Computes the daily Zmanim for any single date at any location.
 * The astronomical and mathematical calculations were directly adapted from the excellent
 * Jewish calendar calculation in C# Copyright © by Ulrich and Ziporah Greve (2005) */
function Zmanim(sd, location) { }

//Gets sunrise and sunset time for given date.
//Accepts a javascript Date object, a string for creating a javascript date object or a jDate object.
//Returns { sunrise: { hour: 6, minute: 18 }, sunset: { hour: 19, minute: 41 } }
//Location object is required.
Zmanim.getSunTimes = function (date, location, considerElevation) {
    if (date instanceof jDate) {
        date = date.getDate();
    }
    else if (date instanceof String) {
        date = new Date(date);
    }
    if (!(date instanceof Date)) {
        throw new Error('Zmanim.getSunTimes: supplied date parameter cannot be converted to a Date');
    }
    //Set the undefined value to true
    considerElevation = (typeof considerElevation === 'undefined' || considerElevation);

    var sunrise, sunset, day = Zmanim.dayOfYear(date),
        zeninthDeg = 90, zenithMin = 50, lonHour = 0, longitude = 0, latitude = 0,
        cosLat = 0, sinLat = 0, cosZen = 0, sinDec = 0, cosDec = 0,
        xmRise = 0, xmSet = 0, xlRise = 0, xlSet = 0, aRise = 0, aSet = 0, ahrRise = 0, ahrSet = 0,
        hRise = 0, hSet = 0, tRise = 0, tSet = 0, utRise = 0, utSet = 0, earthRadius = 6356900,
        zenithAtElevation = Zmanim.degToDec(zeninthDeg, zenithMin) +
                            Zmanim.radToDeg(Math.acos(earthRadius / (earthRadius + (considerElevation ? (location.Elevation || 0) : 0))));

    zeninthDeg = Math.floor(zenithAtElevation);
    zenithMin = (zenithAtElevation - Math.floor(zenithAtElevation)) * 60;
    cosZen = Math.cos(0.01745 * Zmanim.degToDec(zeninthDeg, zenithMin));
    longitude = location.Longitude;
    lonHour = longitude / 15;
    latitude = location.Latitude;
    cosLat = Math.cos(0.01745 * latitude);
    sinLat = Math.sin(0.01745 * latitude);
    tRise = day + (6 + lonHour) / 24;
    tSet = day + (18 + lonHour) / 24;
    xmRise = Zmanim.M(tRise);
    xlRise = Zmanim.L(xmRise);
    xmSet = Zmanim.M(tSet);
    xlSet = Zmanim.L(xmSet);
    aRise = 57.29578 * Math.atan(0.91746 * Math.tan(0.01745 * xlRise));
    aSet = 57.29578 * Math.atan(0.91746 * Math.tan(0.01745 * xlSet));
    if (Math.abs(aRise + 360 - xlRise) > 90) {
        aRise += 180;
    }
    if (aRise > 360) {
        aRise -= 360;
    }
    if (Math.abs(aSet + 360 - xlSet) > 90) {
        aSet += 180;
    }
    if (aSet > 360) {
        aSet -= 360;
    }
    ahrRise = aRise / 15;
    sinDec = 0.39782 * Math.sin(0.01745 * xlRise);
    cosDec = Math.sqrt(1 - sinDec * sinDec);
    hRise = (cosZen - sinDec * sinLat) / (cosDec * cosLat);
    ahrSet = aSet / 15;
    sinDec = 0.39782 * Math.sin(0.01745 * xlSet);
    cosDec = Math.sqrt(1 - sinDec * sinDec);
    hSet = (cosZen - sinDec * sinLat) / (cosDec * cosLat);
    if (Math.abs(hRise) <= 1) {
        hRise = 57.29578 * Math.acos(hRise);
        utRise = ((360 - hRise) / 15) + ahrRise + Zmanim.adj(tRise) + lonHour;
        sunrise = Zmanim.timeAdj(utRise + location.UTCOffset, date, location);
        while (sunrise.hour > 12) {
            sunrise.hour -= 12;
        }
    }

    if (Math.abs(hSet) <= 1) {
        hSet = 57.29578 * Math.acos(hSet);
        utSet = (hRise / 15) + ahrSet + Zmanim.adj(tSet) + lonHour;
        sunset = Zmanim.timeAdj(utSet + location.UTCOffset, date, location);
        while (sunset.hour < 12) {
            sunset.hour += 12;
        }
    }

    return { sunrise: sunrise, sunset: sunset };
};

Zmanim.getChatzos = function (date, location) {
    var sunTimes = Zmanim.getSunTimes(date, location, false),
        rise = sunTimes.sunrise,
        set = sunTimes.sunset;

    if (isNaN(rise.hour) || isNaN(set.hour)) {
        return { hour: NaN, minute: NaN };
    }

    var riseMinutes = (rise.hour * 60) + rise.minute,
        setMinutes = (set.hour * 60) + set.minute,
        chatz = parseInt((setMinutes - riseMinutes) / 2);

    return Zmanim.addMinutes(rise, chatz);
};

Zmanim.getShaaZmanis = function (date, location, offset) {
    var sunTimes = Zmanim.getSunTimes(date, location, false),
        rise = sunTimes.sunrise,
        set = sunTimes.sunset;

    if (isNaN(rise.hour) || isNaN(set.hour)) {
        return NaN;
    }

    if (offset) {
        rise = Zmanim.addMinutes(rise, -offset);
        set = Zmanim.addMinutes(set, offset);
    }

    var riseMinutes = (rise.hour * 60) + rise.minute,
        setMinutes = (set.hour * 60) + set.minute;

    return (setMinutes - riseMinutes) / 12;
}

Zmanim.getCandleLighting = function (date, location) {
    var set = Zmanim.getSunTimes(date, location).sunset;

    if (!location.Israel) {
        return Zmanim.addMinutes(set, -18);
    }

    var special = [{ names: ['jerusalem', 'yerush', 'petach', 'petah', 'petak'], min: 40 },
                   { names: ['haifa', 'chaifa', 'be\'er sheva', 'beersheba'], min: 22 }],
        loclc = location.Name.toLowerCase(),
        city = special.first(function (sp) {
            return sp.names.first(function (spi) {
                return loclc.indexOf(spi) > -1;
            });
        });
    if (city) {
        return Zmanim.addMinutes(set, -city.min);
    }
    else {
        return Zmanim.addMinutes(set, -30);
    }
}

Zmanim.isSecularLeapYear = function (year) {
    if (year % 400 == 0) {
        return true;
    }
    if (year % 100 != 0) {
        if (year % 4 == 0) {
            return true;
        }
    }
    return false;
}

Zmanim.dayOfYear = function (date) {
    var monCount = [0, 1, 32, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366];
    if ((date.getMonth() + 1 > 2) && (Zmanim.isSecularLeapYear(date.getYear()))) {
        return monCount[date.getMonth() + 1] + date.getDate() + 1;
    }
    else {
        return monCount[date.getMonth() + 1] + date.getDate();
    }
};

Zmanim.degToDec = function (deg, min) {
    return (deg + min / 60);
};

Zmanim.M = function (x) {
    return (0.9856 * x - 3.251);
};

Zmanim.L = function (x) {
    return (x + 1.916 * Math.sin(0.01745 * x) + 0.02 * Math.sin(2 * 0.01745 * x) + 282.565);
};

Zmanim.adj = function (x) {
    return (-0.06571 * x - 6.62);
};

Zmanim.radToDeg = function (rad) {
    return 57.29578 * rad;
};

Zmanim.timeAdj = function (time, date, location) {
    var hour, min;

    if (time < 0) {
        time += 24;
    }
    hour = parseInt(time);
    min = parseInt(parseInt((time - hour) * 60 + 0.5));

    if (location.IsDST) {
        hour++;
    }
    else if (location.isDST != false) {
        var inCurrTZ = location.UTCOffset === Utils.currUtcOffset();
        if (inCurrTZ && Utils.isDST(date)) {
            hour++;
        }
        else if ((!inCurrTZ) && Utils.isUSA_DST(date, hour)) {
            hour++;
        }
    }

    return Zmanim.fixHourMinute({ hour: hour, minute: min });
};

// Get day of week using Zellers algorithm.
//Important note: months starts at 1 not 0 like javascript
Zmanim.getDOW = function (year, month, day) {
    var adjustment = parseInt((14 - month) / 12),
        mm = month + 12 * adjustment - 2,
        yy = year - adjustment;
    return (day + (13 * mm - 1) / 5 + yy + yy / 4 - yy / 100 + yy / 400) % 7;
};

//Makes sure hour is between 0 and 23 and minute is between 0 and 59
//Overlaps get added/subtracted.
Zmanim.fixHourMinute = function (hm) {
    //make a copy - javascript sends object parameters by reference
    var result = { hour: hm.hour, minute: hm.minute };
    while (result.minute < 0) {
        result.minute += 60;
        result.hour--;
    }
    while (result.minute >= 60) {
        result.minute -= 60;
        result.hour++;
    }
    if (result.hour < 0) {
        result.hour = 24 + (result.hour % 24);
    }
    if (result.hour > 23) {
        result.hour = result.hour % 24;
    }
    return result;
};

//Add the given number of minutes to the given time
Zmanim.addMinutes = function (hm, minutes) {
    return Zmanim.fixHourMinute({ hour: hm.hour, minute: hm.minute + minutes });
};

Zmanim.getTimeString = function (hm, army) {
    if (!!army) {
        return (hm.hour.toString() + ":" +
                (hm.minute < 10 ? "0" + hm.minute.toString() : hm.minute.toString()));
    }
    else {
        return (hm.hour <= 12 ? (hm.hour == 0 ? 12 : hm.hour) : hm.hour - 12).toString() +
                ":" +
                (hm.minute < 10 ? "0" + hm.minute.toString() : hm.minute.toString()) +
                (hm.hour < 12 ? " AM" : " PM");
    }
};
/// <reference path="utils.js" />
/// <reference path="jDate.js" />
"use strict";

/****************************************************************************************************************
 * Computes the Sedra/Sedras of the week for the given day.
 * Returns an array of sedras (either one or two) for the given Jewish Date
 * Sample of use to get todays sedra in Israel:
 *     var sedras = new Sedra(new jDate(new Date(), true));
 *     var str = sedras.map(function (s) { return s.eng; }).join(' - ');
 * The code was converted to javascript and tweaked by CBS.
 * It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
 * Portions of that code are Copyright (c) 2002 Michael J. Radwin. All Rights Reserved.
 * Many of the algorithms were taken from hebrew calendar routines implemented by Nachum Dershowitz
 * ***************************************************************************************************************/
function Sedra(jd, israel) {
    //If we are between the first day of Sukkos and Simchas Torah, the sedra will always be Vezos Habracha.
    if (jd.Month === 7 && jd.Day >= 15 && jd.Day < (israel ? 23 : 24)) {
        return [Sedra.sedraList[53]];
    }

    var sedraArray = [],
        sedraOrder = Sedra.getSedraOrder(jd.Year, israel),
        absDate = jd.Abs,
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
        roshHashana = jDate.absJd(year, 7, 1),
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

    if (!jDate.isJdLeapY(year)) {
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
/// <reference path="Utils.js" />
/// <reference path="Zmanim.js" />
/// <reference path="jDate.js" />
"use strict";

/* Returns the molad for the given jewish month and year.
 * Algorithm was adapted from Hebcal by Danny Sadinoff
 *
 * Example of use:
 * var moladString = Molad.getString(5776, 10);
 */
function Molad() { }

Molad.getMolad = function (month, year) {
    var totalMonths, partsElapsed, hoursElapsed, parts, monthAdj = month - 7;

    if (monthAdj < 0) {
        monthAdj += jDate.monthsJYear(year);
    }
    totalMonths = parseInt(monthAdj + 235 * parseInt((year - 1) / 19) + 12 * ((year - 1) % 19) +
        ((((year - 1) % 19) * 7) + 1) / 19);
    partsElapsed = 204 + (793 * (totalMonths % 1080));
    hoursElapsed = 5 + (12 * totalMonths) + 793 * parseInt(totalMonths / 1080) +
        parseInt(partsElapsed / 1080) - 6;
    parts = parseInt((partsElapsed % 1080) + 1080 * (hoursElapsed % 24));

    return {
        jDate: new jDate((1 + (29 * parseInt(totalMonths))) + parseInt((hoursElapsed / 24))),
        time: { hour: parseInt(hoursElapsed) % 24, minute: parseInt((parts % 1080) / 18) },
        chalakim: parts % 18
    };
};

// Returns the time of the molad as a string in the format: Monday Night, 8:33 PM and 12 Chalakim
// The molad is always in Jerusalem so we use the Jerusalem sunset times
// to determine whether to display "Night" or "Motzai Shabbos" etc.
Molad.getString = function (year, month) {
    var molad = Molad.getMolad(month, year),
        nightfall = molad.jDate.getSunriseSunset(Location.getJerusalem()).sunset,
        isNight = Utils.totalMinutes(Utils.timeDiff(molad.time, nightfall)) >= 0,
        dow = molad.jDate.getDayOfWeek(),
        str = '';

    if (isNaN(nightfall.hour)) {
        str += Utils.dowEng[dow];
    }
    else if (dow === 6 && isNight) {
        str += "Motzai Shabbos,";
    }
    else if (dow === 5 && isNight) {
        str += "Shabbos Night,";
    }
    else {
        str += Utils.dowEng[dow] + (isNight ? " Night" : "");
    }
    str += " " + Zmanim.getTimeString(molad.time) + " and " +
        molad.chalakim.toString() + " Chalakim";

    return str;
};

// Returns the time of the molad as a string in the format: ליל שני 20:33 12 חלקים
// The molad is always in Jerusalem so we use the Jerusalem sunset times
// to determine whether to display "ליל/יום" or "מוצאי שב"ק" etc.
Molad.getStringHeb = function (year, month) {
    var molad = Molad.getMolad(month, year),
        nightfall = molad.jDate.getSunriseSunset(Location.getJerusalem()).sunset,
        isNight = Utils.totalMinutes(Utils.timeDiff(molad.time, nightfall)) >= 0,
        dow = molad.jDate.getDayOfWeek(),
        str = '';

    if (dow === 6) {
        str += (isNight ? "מוצאי שב\"ק" : "יום שב\"ק");
    }
    else if (dow === 5) {
        str += (isNight ? "ליל שב\"ק" : "ערב שב\"ק");
    }
    else {
        str += (isNight ? "ליל" : "יום") +
            Utils.dowHeb[dow].replace("יום", "");
    }
    str += " " + Zmanim.getTimeString(molad.time, true) + " " +
        molad.chalakim.toString() + " חלקים";

    return str;
};
/// <reference path="Utils.js" />
/// <reference path="jDate.js" />
"use strict";
/***********************************************************************************************************
 * Computes the Day Yomi for the given day.
 * Sample of use - to get todays daf:
 *     var dafEng = Dafyomi.toString(new jDate(new Date()));
 *     var dafHeb = Dafyomi.toStringHeb(new jDate(new Date()));
 * The code was converted to javascript and tweaked by CBS.
 * It is directly based on the C code in Danny Sadinoff's HebCal - Copyright (C) 1994.
 * The HebCal code for dafyomi was adapted by Aaron Peromsik from Bob Newell's public domain daf.el.
***********************************************************************************************************/
function Dafyomi() { }

Dafyomi.masechtaList = [
    { eng: 'Berachos', heb: 'ברכות', daf: 64 },
    { eng: 'Shabbos', heb: 'שבת', daf: 157 },
    { eng: 'Eruvin', heb: 'ערובין', daf: 105 },
    { eng: 'Pesachim', heb: 'פסחים', daf: 121 },
    { eng: 'Shekalim', heb: 'שקלים', daf: 22 },
    { eng: 'Yoma', heb: 'יומא', daf: 88 },
    { eng: 'Sukkah', heb: 'סוכה', daf: 56 },
    { eng: 'Beitzah', heb: 'ביצה', daf: 40 },
    { eng: 'Rosh Hashana', heb: 'ראש השנה', daf: 35 },
    { eng: 'Taanis', heb: 'תענית', daf: 31 },
    { eng: 'Megillah', heb: 'מגילה', daf: 32 },
    { eng: 'Moed Katan', heb: 'מועד קטן', daf: 29 },
    { eng: 'Chagigah', heb: 'חגיגה', daf: 27 },
    { eng: 'Yevamos', heb: 'יבמות', daf: 122 },
    { eng: 'Kesubos', heb: 'כתובות', daf: 112 },
    { eng: 'Nedarim', heb: 'נדרים', daf: 91 },
    { eng: 'Nazir', heb: 'נזיר', daf: 66 },
    { eng: 'Sotah', heb: 'סוטה', daf: 49 },
    { eng: 'Gitin', heb: 'גיטין', daf: 90 },
    { eng: 'Kiddushin', heb: 'קדושין', daf: 82 },
    { eng: 'Baba Kamma', heb: 'בבא קמא', daf: 119 },
    { eng: 'Baba Metzia', heb: 'בבא מציעא', daf: 119 },
    { eng: 'Baba Batra', heb: 'בבא בתרא', daf: 176 },
    { eng: 'Sanhedrin', heb: 'סנהדרין', daf: 113 },
    { eng: 'Makkot', heb: 'מכות', daf: 24 },
    { eng: 'Shevuot', heb: 'שבועות', daf: 49 },
    { eng: 'Avodah Zarah', heb: 'עבודה זרה', daf: 76 },
    { eng: 'Horayot', heb: 'הוריות', daf: 14 },
    { eng: 'Zevachim', heb: 'זבחים', daf: 120 },
    { eng: 'Menachos', heb: 'מנחות', daf: 110 },
    { eng: 'Chullin', heb: 'חולין', daf: 142 },
    { eng: 'Bechoros', heb: 'בכורות', daf: 61 },
    { eng: 'Arachin', heb: 'ערכין', daf: 34 },
    { eng: 'Temurah', heb: 'תמורה', daf: 34 },
    { eng: 'Kerisos', heb: 'כריתות', daf: 28 },
    { eng: 'Meilah', heb: 'מעילה', daf: 22 },
    { eng: 'Kinnim', heb: 'קנים', daf: 4 },
    { eng: 'Tamid', heb: 'תמיד', daf: 10 },
    { eng: 'Midos', heb: 'מדות', daf: 4 },
    { eng: 'Niddah', heb: 'נדה', daf: 73 }];

Dafyomi.getDaf = function (jd) {
    var absoluteDate = jd.Abs,
        dafcnt = 40, cno, dno, osday, nsday, total, count, j, blatt;

    osday = jDate.absSd(new Date(1923, 8, 11));
    nsday = jDate.absSd(new Date(1975, 5, 24));

    /*  No cycle, new cycle, old cycle */
    if (absoluteDate < osday)
        return null; /* daf yomi hadn't started yet */
    if (absoluteDate >= nsday) {
        cno = 8 + parseInt(((absoluteDate - nsday) / 2711));
        dno = (absoluteDate - nsday) % 2711;
    }
    else {
        cno = 1 + parseInt((absoluteDate - osday) / 2702);
        dno = parseInt((absoluteDate - osday) / 2702);
    }

    /* Find the daf taking note that the cycle changed slightly after cycle 7. */
    total = blatt = 0;
    count = -1;

    /* Fix Shekalim for old cycles */
    if (cno <= 7)
        Dafyomi.masechtaList[4].daf = 13;
    else
        Dafyomi.masechtaList[4].daf = 22;

    /* Find the daf */
    j = 0;
    while (j < dafcnt) {
        count++;
        total = total + Dafyomi.masechtaList[j].daf - 1;
        if (dno < total) {
            blatt = (Dafyomi.masechtaList[j].daf + 1) - (total - dno);
            /* fiddle with the weird ones near the end */
            switch (count) {
                case 36:
                    blatt = blatt + 21;
                    break;
                case 37:
                    blatt = blatt + 24;
                    break;
                case 38:
                    blatt = blatt + 33;
                    break;
                default:
                    break;
            }
            /* Bailout */
            j = 1 + dafcnt;
        }
        j++;
    }

    return { masechet: Dafyomi.masechtaList[count], daf: blatt };
};

// Returns the name of the Masechta and daf number in English, For example: Sukkah, Daf 3
Dafyomi.toString = function (jd) {
    var d = Dafyomi.getDaf(jd);
    return d.masechet.eng + ", Daf " + d.daf.toString();
};

//Returns the name of the Masechta and daf number in Hebrew. For example: 'סוכה דף כ.
Dafyomi.toStringHeb = function (jd) {
    var d = Dafyomi.getDaf(jd);
    return d.masechet.heb + " דף " + Utils.toJNum(d.masechet.daf);
}