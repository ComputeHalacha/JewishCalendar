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

Utils.isIsrael_DST = function () {
    var date = new Date(),
        israelTimeOffset = (2 - Utils.currUtcOffset());
    //This will give us the current correct date and time in Israel
    date.setHours(date.getHours + israelTimeOffset);

    //TODO: add correct logic here!
    return ![11, 12, 1, 2, 3].has(date.getMonth());
}

//Gets the time difference between two times of day
Utils.timeDiff = function (time1, time2) {
    return Zmanim.fixHourMinute(Zmanim.addMinutes(time1, Utils.totalMinutes(time2)));
};

//Gets the total number of minutes in the given time
Utils.totalMinutes = function (time) {
    return time.hour * 60 + time.minutes;
};