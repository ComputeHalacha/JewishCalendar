/// <reference path="_references.js" />

// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints,
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";
    $(document).on('pagecreate', '#divMainPage', function () {
        $('#btnNextDay').on('click', function () { goDay(1); });
        $('#btnNextWeek').on('click', function () { goDay(7); });
        $('#btnNextMonth').on('click', function () { goMonth(1); });
        $('#btnNextYear').on('click', function () { goYear(1); });
        $('#btnPrevWeek').on('click', function () { goDay(-7); });
        $('#btnPrevMonth').on('click', function () { goMonth(-1); });
        $('#btnPrevYear').on('click', function () { goYear(-1); })
            .on("swipeup", "#divMainPage", function (event) {
                goDay(-1);
            }).on("swipedown", "#divMainPage", function (event) {
                goDay(1);
            });
        if (!window.cordova) {
            console.log('Cordova not recognized, showing default location and date.');
            showDate();
        }
    });

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
                $('#divMainPage').jqmData('location', location);
                console.log('Acquired location from geolocation plugin');
                console.info(position);
                showDate();
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
        if (!$('#divMainPage').jqmData('location')) {
            !!window.cordova ? setCurrentLocation() : setDefaultLocation();
        }
        return $('#divMainPage').jqmData('location');
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
        $('#divMainPage').jqmData('location', loc);
        showDate();
    }

    function showDate(jd) {
        if (jd) {
            $('#divMainPage').jqmData('currentjDate', jd);
        }
        else if ($('#divMainPage').jqmData('currentjDate')) {
            jd = $('#divMainPage').jqmData('currentjDate');
        }
        else {
            showDate(new jDate(new Date()));
            return;
        }

        var location = getLocation();
        $('#h2Header').html(jd.toStringHeb() + '<br />' + jd.getDate().toDateString());
        $('#pSpecial').html(getSpecialHtml(jd, location));
        $('#divCaption').html('Zmanim for ' + location.Name);
        $('#emLocDet').html('lat: ' +
                location.Latitude.toString() +
                ' long:' + location.Longitude.toString() +
                (location.Israel ? ' | Israel' : '') + '  |  ' +
                (location.IsDST ? 'DST' : 'not DST'));
        $('#pMain').html(getZmanimHtml(jd, location));
        $('#pMain').jqmData('currDate', jd);
    }

    function goDay(num) {
        var jd = $('#divMainPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addDays(num));
        }
    }

    function goMonth(num) {
        var jd = $('#divMainPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addMonths(num));
        }
    }

    function goYear(num) {
        var jd = $('#divMainPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addYears(num));
        }
    }

    function getSpecialHtml(jd, location) {
        var holidays = jd.getHolidays(location.Israel),
            html = '';

        if (holidays.length) {
            holidays.forEach(function (h) {
                if (~h.indexOf('Mevarchim')) {
                    var nextMonth = jd.addMonths(1);
                    h += '<br />Molad: ' +
                        Molad.getString(nextMonth.Year, nextMonth.Month, location);
                }
                html += h + '<br />';
            });
        }
        return html;
    }

    function getZmanimHtml(jd, location) {
        var html = '',
            ns = jd.getSunriseSunset(location),
            netz = ns.sunrise,
            shkia = ns.sunset,
        dy = jd.getDafyomi(),
        chatzos = jd.getChatzos(location),
        shaaZmanis = jd.getShaaZmanis(location),
        shaaZmanis90 = jd.getShaaZmanis(location, 90);

        if (jd.hasCandleLighting()) {
            html += "<strong>Candle Lighting: " + Utils.getTimeString(jd.getCandleLighting(location)) + '</strong><br /><br />';
        }
        html += addLine("Weekly Sedra",
            jd.getSedra(location.Israel).map(function (s) { return s.eng; }).join(' - '));
        if (dy != null) {
            html += addLine("Daf Yomi", dy);
        }

        if (isNaN(netz.hour)) {
            html += addLine("Netz Hachama", "The does not rise");
        }
        else {
            html += addLine("Alos Hashachar - 90", (Utils.addMinutes(netz, -90)));
            html += addLine("Alos Hashachar - 72", (Utils.addMinutes(netz, -72)));
            html += addLine("Netz Hachama", netz);
            html += addLine("Krias Shma - MG\"A", (Utils.addMinutes(Utils.addMinutes(netz, -90), parseInt(shaaZmanis90 * 3))));
            html += addLine("Krias Shma - GR\"A", (Utils.addMinutes(netz, parseInt(shaaZmanis * 3))));
            html += addLine("Zeman Tefillah - MG\"A", (Utils.addMinutes(Utils.addMinutes(netz, -90), parseInt(shaaZmanis90 * 4))));
            html += addLine("Zeman Tefillah - GR\"A", Utils.addMinutes(netz, parseInt(shaaZmanis * 4)));
        }

        if (!(isNaN(netz.hour) || isNaN(shkia.hour))) {
            html += addLine("Chatzos - Day & Night", chatzos);
            html += addLine("Mincha Gedolah", Utils.addMinutes(chatzos, parseInt(shaaZmanis * 0.5)));
        }

        if (isNaN(shkia.hour)) {
            html += addLine("Shkias Hachama", "The sun does not set");
        }
        else {
            html += addLine("Shkias Hachama", shkia);
            html += addLine("Nightfall 45", Utils.addMinutes(shkia, 45));
            html += addLine("Rabbeinu Tam", Utils.addMinutes(shkia, 72));
        }

        return html;
    }

    function addLine(caption, value) {
        return caption + '...............<strong>' +
            (value.hour ? Utils.getTimeString(value) : value) + '</strong><br />';
    }
})();