/*jshint strict:true, undef:true */
/*global document: true, $: true, jDate:true, Utils:true, console:true, navigator:true, window:true, localStorage:true, Location:true */

"use strict";

document.addEventListener('deviceready', deviceReady.bind(this), false);
document.onLocationChanged = [];
document.onDeviceReady = [];
document.onDevicePause = [];
document.onDeviceResume = [];

if (!document.onDeviceReady.first(function (i) {
        return !!i.general;
    })) {
    document.onDeviceReady.push({
        'general': function () {
            if (navigator.geolocation) {
                setCurrentLocation();
            } else {
                setDefaultLocation();
            }
        }
    });
}

function deviceReady() {
    console.log('Cordova was recognized device ready has been fired.');

    // Handle the Cordova pause and resume events
    document.addEventListener('pause', onPause.bind(this), false);
    document.addEventListener('resume', onResume.bind(this), false);
    // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.

    document.onDeviceReady.forEach(function (i) {
        for (var p in i) {
            i[p]();
            console.log('RAN onDeviceReady for: ' + p);
        }
    });

}

function onPause() {
    // TODO: This application has been suspended. Save application state here.
    document.onDevicePause.forEach(function (i) {
        for (var p in i) {
            i[p]();
             console.log('RAN onPause for: ' + p);
        }
    });

}

function onResume() {
    // TODO: This application has been reactivated. Restore application state here.
    setCurrentLocation();
    document.onDeviceResume.forEach(function (i) {
        for (var p in i) {
            i[p]();
            console.log('RAN onResume for: ' + p);
        }
    });
}

function showMessage(message, isError, seconds, title, callback, buttonName) {
    /*if (navigator.notification) {
        navigator.notification.alert(message, callback, title, buttonName);
        if (isError) {
            navigator.notification.beep(1);
        }
    }
    else {*/
    toast(message, isError, seconds);
    //}
}

function toast(message, isError, seconds) {
    var removeMe = function () {
        $(this).remove();
    };
    $('<div class="toast">' + message + '</div>')
        .addClass(isError ? 'error' : '')
        .click(removeMe)
        .appendTo($.mobile.pageContainer).delay(seconds ? seconds * 1000 : (isError ? 15000 : 1000))
        .fadeOut(1000, removeMe);
}

function getLocation() {
    if (!$($.mobile.pageContainer).jqmData('location')) {
        //we should at least start with something....
        setDefaultLocation();

        if (!!window.cordova) {
            setCurrentLocation();
        }
    }
    return $($.mobile.pageContainer).jqmData('location');
}

function setDefaultLocation() {
    var loc = localStorage.getItem('location');

    if (loc) {
        loc = JSON.parse(loc);
    } else {
        loc = Location.getJerusalem(); //where else?
    }
    setLocation(loc, false, false);
}

function setCurrentLocation() {
    try {
        console.log('Attempting to acquire device location from Cordova geolocation plugin');
        navigator.geolocation.getCurrentPosition(function (position) {
            var location = new Location('Current Location', //Name
                undefined, //Israel - we have no way of knowing (the constructor will try to figure it out though)
                position.coords.latitude,
                position.coords.longitude,
                Utils.currUtcOffset(),
                position.coords.altitude);
            console.log('Location acquired from Cordova geolocation plugin.');
            setLocation(location, false, false);
        }, function () {
            setDefaultLocation();
        });
    } catch (e) {
        console.error(e.message);
        setDefaultLocation();
    }
}

function setLocation(loc, store, inform) {
    if (!!store) {
        localStorage.setItem('location', JSON.stringify(loc));
    }
    if (!!inform) {
        showMessage('Location set to: ' + loc.Name, false, 2, 'Location set');
    }
    $($.mobile.pageContainer).jqmData('location', loc);

    document.onLocationChanged.forEach(function (i) {
        for (var p in i) {
            i[p]();
            console.log('RAN onLocationChanged for: ' + p);
        }
    });
}

function getHolidayIcon(holidays) {
    var html = '';
    for (var i = 0; i < holidays.length; i++) {
        if (holidays[i].has('Rosh Hashana') && !holidays[i].has('Erev Rosh Hashana')) {
            html += '<i class="fa fa-balance-scale fa-2x"></i>';
        }
        if (holidays[i].has('Yom Kippur') && !holidays[i].has('Erev Yom Kippur')) {
            html += '<i class="fa fa-book fa-2x"></i>';
        }
        if (holidays[i].has('Sukkos') && !holidays[i].has('Erev Sukkos')) {
            html += '<i class="fa fa-inbox fa-2x"></i>';
        }
        if (holidays[i].has('Chanuka')) {
            html += '<i class="fa fa-fire fa-2x"></i>';
        }
        if (holidays[i].has('Tu B\'Shvat')) {
            html += '<i class="fa fa-apple fa-2x"></i>';
        }
        if (holidays[i].has('Purim') && !holidays[i].has('Purim Katan')) {
            html += '<i class="fa  fa-glass fa-2x"></i>';
        }
        if (holidays[i].has('Pesach') && !holidays[i].has('Erev Pesach')) {
            html += '<i class="fa fa-soccer-ball-o fa-2x"></i>';
        }
        if (holidays[i].has('Shavuos') && !holidays[i].has('Erev Shavuos')) {
            html += '<i class="fa fa-pagelines fa-2x"></i>';
        }
        if (holidays[i].has('Fast') || holidays[i].has('Tzom') || holidays[i].has('Tisha B\'Av')) {
            html += '<i class="fa fa-ban fa-2x"></i>';
        }
    }
    return html;
}