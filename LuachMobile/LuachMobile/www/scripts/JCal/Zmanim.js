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
Zmanim.getDOW = function (year, month, day) {
    var adjustment = (14 - month) / 12,
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