/// <reference path="_references.js" />

"use strict";

document.addEventListener('deviceready', onDeviceReady.bind(this), false);

function onDeviceReady() {
    console.log('Cordova was recognized device ready has been fired.');

    // Handle the Cordova pause and resume events
    document.addEventListener('pause', onPause.bind(this), false);
    document.addEventListener('resume', onResume.bind(this), false);
    // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
    setCurrentLocation();

    if (document.onDeviceReady) {
        document.onDeviceReady();
    }
};

function onPause() {
    // TODO: This application has been suspended. Save application state here.
    if (document.onDevicePause) {
        document.onDevicePause();
    }
};

function onResume() {
    // TODO: This application has been reactivated. Restore application state here.
    setCurrentLocation();
    if (document.onDeviceResume) {
        document.onDeviceResume();
    }
};

function setCurrentLocation() {
    try {
        console.log('Attempting to acquire device location from Cordova geolocation plugin');
        navigator.geolocation.getCurrentPosition(function (position) {
            var location = new Location('Current Location', //Name
                                        undefined, //Israel - we have no way of knowing (the constructor will try to figure it out though)
                                        position.coords.latitude,
                                        position.coords.longitude,
                                        Utils.currUtcOffset(),
                                        position.coords.altitude,
                                        Utils.isDST());
            console.log('Location acquired from Cordova geolocation plugin.');
            $($.mobile.pageContainer).jqmData('location', location);
            console.log('Acquired location from geolocation plugin');
            console.info(position);
            showMessage('Location set to Current position', false, 2, 'Location set');
            if (document.onLocationChanged) {
                document.onLocationChanged();
            }
        }, function () {
            setDefaultLocation();
        });
    }
    catch (e) {
        console.error(e.message);
        setDefaultLocation();
    }
}

function showMessage(message, isError, seconds, title, callback, buttonName) {
    if (navigator.notification) {
        navigator.notification.alert(message, callback, title, buttonName);
        if (isError) {
            navigator.notification.beep(1);
        }
    }
    else {
        toast(message, isError, seconds);
    }
}

function toast(message, isError, seconds) {
    var removeMe = function () { $(this).remove(); };
    $('<div class="toast">' + message + '</div>')
        .addClass(isError ? 'error' : '')
        .click(removeMe)
        .appendTo($.mobile.pageContainer).delay(seconds ? seconds * 1000 : (isError ? 15000 : 1000))
        .fadeOut(1000, removeMe);
}

function getLocation() {
    if (!$($.mobile.pageContainer).jqmData('location')) {
        !!window.cordova ? setCurrentLocation() : setDefaultLocation();
    }
    return $($.mobile.pageContainer).jqmData('location');
}

function setDefaultLocation() {
    var loc = localStorage.getItem('location');

    if (loc) {
        loc = JSON.parse(loc);
    }
    else {
        loc = Location.getJerusalem(); //where else?
        localStorage.setItem('location', JSON.stringify(loc));
    }
    showMessage('Location set to: ' + loc.Name, false, 2, 'Location set');
    $($.mobile.pageContainer).jqmData('location', loc);
    if (document.onLocationChanged) {
        document.onLocationChanged();
    }
}
function getHolidayIcon(holidays) {
    var html = '';
    for (var i = 0; i < holidays.length; i++) {
        if (holidays[i].has('Rosh Hashana') && !holidays[i].has('Erev Rosh Hashana')) {
            html += '<i class="fa fa-balance-scale fa-lg"></i>';
        }
        if (holidays[i].has('Yom Kippur') && !holidays[i].has('Erev Yom Kippur')) {
            html += '<i class="fa fa-book fa-lg"></i>';
        }
        if (holidays[i].has('Sukkos') && !holidays[i].has('Erev Sukkos')) {
            html += '<i class="fa fa-inbox fa-lg"></i>';
        }
        if (holidays[i].has('Chanuka')) {
            html += '<i class="fa fa-fire fa-lg"></i>';
        }
        if (holidays[i].has('Tu B\'Shvat')) {
            html += '<i class="fa fa-apple fa-lg"></i>';
        }
        if (holidays[i].has('Purim') && !holidays[i].has('Purim Katan')) {
            html += '<i class="fa  fa-glass fa-lg"></i>';
        }
        if (holidays[i].has('Pesach') && !holidays[i].has('Erev Pesach')) {
            html += '<i class="fa fa-soccer-ball-o fa-lg"></i>';
        }
        if (holidays[i].has('Shavuos') && !holidays[i].has('Erev Shavuos')) {
            html += '<i class="fa fa-pagelines fa-lg"></i>';
        }
        if (holidays[i].has('Fast') || holidays[i].has('Tzom') || holidays[i].has('Tisha B\'Av')) {
            html += '<i class="fa fa-ban fa-lg"></i>';
        }
    }
    return html;
}