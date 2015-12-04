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
    totalMonths = monthAdj + 235 * ((year - 1) / 19) + 12 * ((year - 1) % 19) +
        ((((year - 1) % 19) * 7) + 1) / 19;
    partsElapsed = 204 + (793 * (totalMonths % 1080));
    hoursElapsed = 5 + (12 * totalMonths) + 793 * (totalMonths / 1080) +
        partsElapsed / 1080 - 6;
    parts = (partsElapsed % 1080) + 1080 * (hoursElapsed % 24);

    return {
        jDate: new jDate((1 + (29 * parseInt(totalMonths))) + parseInt((hoursElapsed / 24))),
        time: { hour: parseInt(hoursElapsed) % 24, minute: parseInt((parts % 1080) / 18) },
        chalakim: parts % 18
    };
};

// Returns the time of the molad as a string in the format: Monday Night, 8:33 PM and 12 Chalakim
// The location is used to determine when to display "Night" or "Motzai Shabbos" etc.
// If location is not supplied, the cutoff time is 8 PM.
Molad.getString = function (year, month, location) {
    var molad = Molad.getMolad(month, year),
        nightfall = { hour: 20, minute: 0 },
        dow = molad.jDate.getDayOfWeek(),
        str = '';
    if (location) {
        nightfall = molad.jDate.getSunriseSunset(location).sunset;
    }
    var isNight = Utils.totalMinutes(Utils.timeDiff(molad.time, nightfall)) >= 0;

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
// The location is used to determine when to display "ליל/יום" or "מוצאי שב"ק" etc.
// If location is not supplied, the cutoff time is 8 PM.
Molad.getStringHeb = function (year, month, location) {
    var molad = Molad.getMolad(month, year),
        nightfall = { hour: 20, minute: 0 },
        dow = molad.jDate.getDayOfWeek(),
        str = '';
    if (location) {
        nightfall = molad.jDate.getSunriseSunset(location).sunset;
    }
    var isNight = Utils.totalMinutes(Utils.timeDiff(molad.time, nightfall)) >= 0;

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