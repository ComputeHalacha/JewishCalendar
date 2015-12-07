/// <reference path="_references.js" />

// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints,
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {
        console.log('Cordova was recognized device ready has been fired.');

        // Handle the Cordova pause and resume events
        document.addEventListener('pause', onPause.bind(this), false);
        document.addEventListener('resume', onResume.bind(this), false);
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        setCurrentLocation();
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
        setCurrentLocation();
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
                var a = getHarvey();
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
    }
})();