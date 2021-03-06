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
// to determine whether to display "Night" or "Motzai Shabbos" etc. (check this...)
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
    str += " " + Utils.getTimeString(molad.time) + " and " +
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
    str += " " + Utils.getTimeString(molad.time, true) + " " +
        molad.chalakim.toString() + " חלקים";

    return str;
};