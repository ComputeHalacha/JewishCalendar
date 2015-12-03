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
            showDate();
        }
    });

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {
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
            navigator.geolocation.getCurrentPosition(function (position) {
                var location = new Location('Current Location', //Name
                                            undefined, //Israel - don't set, the constructor will try to figure it out
                                            position.coords.latitude,
                                            position.coords.longitude,
                                            Utils.currUtcOffset(),
                                            position.coords.altitude);
                $('#divMainPage').jqmData('location', location);
                console.log('Acquired location from geolocation plugin');
                console.info(position);
                showDate();
                toast('Loaded location from current position');
            }, function () {
                setDefaultLocation();
            });
        }
        catch (e) {
            console.error(e);
            setDefaultLocation();
        }
    }

    function showMessage(message, isError, seconds) {
        if (navigator.notification) {
            navigator.notification.alert(message);
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
            loc = new Location("Modi'in Illit", true, 31.933, -35.0426, 2, 300);
            localStorage.setItem('location', JSON.stringify(loc));
        }
        showMessage('Location set to: ' + loc.Name);
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
        dy = null,// DafYomi.GetDafYomi(this._displayingJewishDate);
        chatzos = jd.getChatzos(location),
        shaaZmanis = jd.getShaaZmanis(location),
        shaaZmanis90 = jd.getShaaZmanis(location, 90);

        if (jd.hasCandleLighting()) {
            html += "<strong>Candle Lighting: " + Zmanim.getTimeString(jd.getCandleLighting(location)) + '</strong><br /><br />';
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
            html += addLine("Alos Hashachar - 90", (Zmanim.addMinutes(netz, -90)));
            html += addLine("Alos Hashachar - 72", (Zmanim.addMinutes(netz, -72)));
            html += addLine("Netz Hachama", netz);
            html += addLine("Krias Shma - MG\"A", (Zmanim.addMinutes(Zmanim.addMinutes(netz, -90), parseInt(shaaZmanis90 * 3))));
            html += addLine("Krias Shma - GR\"A", (Zmanim.addMinutes(netz, parseInt(shaaZmanis * 3))));
            html += addLine("Zeman Tefillah - MG\"A", (Zmanim.addMinutes(Zmanim.addMinutes(netz, -90), parseInt(shaaZmanis90 * 4))));
            html += addLine("Zeman Tefillah - GR\"A", Zmanim.addMinutes(netz, parseInt(shaaZmanis * 4)));
        }

        if (!(isNaN(netz.hour) || isNaN(shkia.hour))) {
            html += addLine("Chatzos - Day & Night", chatzos);
            html += addLine("Mincha Gedolah", Zmanim.addMinutes(chatzos, parseInt(shaaZmanis * 0.5)));
        }

        if (isNaN(shkia.hour)) {
            html += addLine("Shkias Hachama", "The sun does not set");
        }
        else {
            html += addLine("Shkias Hachama", shkia);
            html += addLine("Nightfall 45", Zmanim.addMinutes(shkia, 45));
            html += addLine("Rabbeinu Tam", Zmanim.addMinutes(shkia, 72));
        }

        return html;
    }

    function addLine(caption, value) {
        return caption + '...............<strong>' +
            (value.hour ? Zmanim.getTimeString(value) : value) + '</strong><br />';
    }
})();