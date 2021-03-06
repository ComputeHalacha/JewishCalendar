/// <reference path="_references.js" />

// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints,
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";
    $(document).one('pagecreate', '#divZmanimPage', function () {
        console.log('RAN pagecreate on divZmanimPage');
        $('body').css({ 'visibility': 'visible' });
        $('#divZmanimPage #btnNextDay').on('click', function () { goDay(1); });
        $('#divZmanimPage #btnNextWeek').on('click', function () { goDay(7); });
        $('#divZmanimPage #btnZmanimToday').on('click', function () { goDay(); });
        $('#divZmanimPage #btnPrevDay').on('click', function () { goDay(-1); });
        $('#divZmanimPage #btnPrevWeek').on('click', function () { goDay(-7); });
        if (!document.onLocationChanged.first(function (i) { return i['divZmanimPage']; })) {
            document.onLocationChanged.push({ 'divZmanimPage': locationChanged });
        }
    }).on("swipeup", "#divZmanimPage", function (event) {
        goDay(-1);
    }).on("swipedown", "#divZmanimPage", function (event) {
        goDay(1);
    });

    $(document).on("pagecontainershow", $.mobile.pageContainer, function (e, ui) {
        if (ui.toPage.attr('id') === 'divZmanimPage') {
            console.log('RAN pagecontainershow for: divZmanimPage');
            //Display the location
            locationChanged();
        }
    });

    function locationChanged(location) {
        try {
            var location = location || getLocation();
            console.log('RAN zmanim.js/locationChanged for: ' + (location ? location.Name : 'UNKNOWN'));
            if (location) {
                $('#divZmanimPage #divZmanimCaption').html('Zmanim for ' + location.Name);
                $('#divZmanimPage #emLocDet').html('lat: ' +
                        location.Latitude.toString() +
                        ' long:' + location.Longitude.toString() +
                        (location.Israel ? ' | Israel' : ''));
            }
            showDate();
        }
        catch (e) {
            console.error(e);
        }
    }

    function showDate(jd) {
        if (jd) {
            $(document).jqmData('currentjDate', jd);
        }
        else if ($(document).jqmData('currentjDate')) {
            jd = $(document).jqmData('currentjDate');
        }
        else {
            showDate(new jDate(new Date()));
            return;
        }

        var location = getLocation();
        $('#divZmanimPage #h2Header').html(jd.toStringHeb() + '<br />' + jd.getDate().toDateString());
        $('#divZmanimPage #pSpecial').html(getSpecialHtml(jd, location));
        $('#divZmanimPage #ulMain').html(getZmanimHtml(jd, location));
        $('#divZmanimPage #ulMain').listview("refresh");
    }

    function goDay(num) {
        if (!num) {
            showDate(new jDate());
        }
        else {
            var jd = $(document).jqmData('currentjDate');
            if (jd) {
                showDate(jd.addDays(num));
            }
        }
    }

    function getSpecialHtml(jd, location) {
        var holidays = location ? jd.getHolidays(location.Israel) : [],
            html = '';
        if (holidays.length) {
            html += '<tr><td>' + getHolidayIcon(holidays) + '</td><td>';
            holidays.forEach(function (h) {
                if (~h.indexOf('Mevarchim')) {
                    var nextMonth = jd.addMonths(1);
                    h += '<br />Molad: ' +
                        Molad.getString(nextMonth.Year, nextMonth.Month, location);
                }
                html += h + '<br />';
            });
            html += '</td></tr>';
        }
        if (jd.hasEiruvTavshilin(location.Israel)) {
            html += "<tr><td><strong>Eiruv Tavshilin</strong></td></tr>";
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
            html += addLine("Candle Lighting", jd.getCandleLighting(location), "Candle lighting time ");
        }
        html += addLine("Weekly Sedra",
            jd.getSedra(location.Israel).map(function (s) { return s.eng; }).join(' - '), "Parasha of the week");
        if (dy != null) {
            html += addLine("Daf Yomi", dy, "The Daf Yomi for this day");
        }
        if (jd.Month < 7 && jd.getDayOfWeek() === 6) {
            var pa = jd.getPirkeiAvos(location.Israel);
            if (pa.length) {
                html += addLine("Pirkei Avos", pa.map(function (s) {
                    return Utils.toSuffixed(s) + ' Perek';
                }).join(' and '), "The Perek/Prakim of Pirkei Avos");
            }
        }
        if (isNaN(netz.hour)) {
            html += addLine("Netz Hachama", "The does not rise", "Sunrise");
        }
        else {
            html += addLine("Alos Hashachar - 90", (Utils.addMinutes(netz, -90)), "90 minutes before sunrise");
            html += addLine("Alos Hashachar - 72", (Utils.addMinutes(netz, -72)), "72 minutes before sunrise");
            html += addLine("Netz Hachama", netz, "Sunrise at this location");
            html += addLine("Krias Shma - MG\"A", (Utils.addMinutes(Utils.addMinutes(netz, -90), parseInt(shaaZmanis90 * 3))), "Zman Krias Shema - Magen Avraham");
            html += addLine("Krias Shma - GR\"A", (Utils.addMinutes(netz, parseInt(shaaZmanis * 3))), "Zman Krias Shema -  Gr\"a");
            html += addLine("Zeman Tefillah - MG\"A", (Utils.addMinutes(Utils.addMinutes(netz, -90), parseInt(shaaZmanis90 * 4))), "Zman Tefillah - Magen Avraham");
            html += addLine("Zeman Tefillah - GR\"A", Utils.addMinutes(netz, parseInt(shaaZmanis * 4)), "Zman Tefillah -  Gr\"a");
        }

        if (!(isNaN(netz.hour) || isNaN(shkia.hour))) {
            html += addLine("Chatzos - Day & Night", chatzos, "Time of Chatzos - both day and night");
            html += addLine("Mincha Gedolah", Utils.addMinutes(chatzos, parseInt(shaaZmanis * 0.5)));
            html += addLine("Mincha Ktana", Utils.addMinutes(netz, parseInt(shaaZmanis * 9.5)));
            html += addLine("Plag HaMincha", Utils.addMinutes(netz, parseInt(shaaZmanis * 10.75)));
        }

        if (isNaN(shkia.hour)) {
            html += addLine("Shkias Hachama", "The sun does not set");
        }
        else {
            html += addLine("Shkias Hachama", shkia, "Sunset");
            html += addLine("Nightfall 45", Utils.addMinutes(shkia, 45), "45 minutes after sunset");
            html += addLine("Rabbeinu Tam", Utils.addMinutes(shkia, 72), "72 minutes after sunset");
            html += addLine("72 \"Dakot Zmaniot\"", Utils.addMinutes(shkia, parseInt(shaaZmanis * 1.2)), "72 proportional minutes after sunset");
            html += addLine("72 \"Dakot Zmaniot MA\"", Utils.addMinutes(shkia, parseInt(shaaZmanis90 * 1.2)), "72 strict proportional minutes after sunset");
        }

        return html;
    }

    function addLine(caption, value, descr) {
        return '<li data-role="list-divider">' + caption + '</li>' +
               '<li class="liZman"><p>' + (descr || '') +
               '</p><em>' + (value.hour ? Utils.getTimeString(value) : value) +
               '</em></li>';
    }

    function showCalendar() {
        $(":mobile-pagecontainer").pagecontainer("change", "#divCalendarPage", { showLoadMsg: true });
    }
})();