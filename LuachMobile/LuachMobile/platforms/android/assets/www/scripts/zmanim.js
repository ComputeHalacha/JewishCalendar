/// <reference path="_references.js" />

// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints,
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";
    $(document).one('pagecreate', '#divZmanimPage', function () {
        $('#divZmanimPage #btnNextDay').on('click', function () { goDay(1); });
        $('#divZmanimPage #btnNextWeek').on('click', function () { goDay(7); });
        $('#divZmanimPage #btnPrevDay').on('click', function () { goDay(-1); });        
        $('#divZmanimPage #btnPrevWeek').on('click', function () { goDay(-7); });
        $('#divZmanimPage #aGoCal').on('click', function () { showCalendar(); });
    }).on("swipeup", "#divZmanimPage", function (event) {
        goDay(-1);
    }).on("swipedown", "#divZmanimPage", function (event) {
        goDay(1);
    });

    $(document).off("pagecontainershow").on("pagecontainershow", $.mobile.pageContainer, function (e, ui) {
        if (ui.toPage.attr('id') === 'divZmanimPage') {           
            document.onLocationChanged = function (location) {
                try {
                    var location = location || getLocation();
                    if (location) {
                        $('#divZmanimPage #divCaption').html('Zmanim for ' + location.Name);
                        $('#divZmanimPage #emLocDet').html('lat: ' +
                                location.Latitude.toString() +
                                ' long:' + location.Longitude.toString() +
                                (location.Israel ? ' | Israel' : '') + '  |  ' +
                                (location.IsDST ? 'DST' : 'not DST'));
                    }
                    showDate();                   
                }
                catch (e) {
                    console.error(e);
                }
            };
            
            //Display the location
            document.onLocationChanged(getLocation());
        }
    });    

    function showDate(jd) {
        if (jd) {
            $('#divZmanimPage').jqmData('currentjDate', jd);
        }
        else if ($('#divZmanimPage').jqmData('currentjDate')) {
            jd = $('#divZmanimPage').jqmData('currentjDate');
        }
        else {
            showDate(new jDate(new Date()));
            return;
        }

        var location = getLocation();
        $('#divZmanimPage #h2Header').html(jd.toStringHeb() + '<br />' + jd.getDate().toDateString());
        $('#divZmanimPage #pSpecial').html(getSpecialHtml(jd, location));
        $('#divZmanimPage #ulMain').html(getZmanimHtml(jd, location));
        $('#divZmanimPage #pMain').jqmData('currDate', jd);
        $('#divZmanimPage #ulMain').listview("refresh");
    }

    function goDay(num) {
        var jd = $('#divZmanimPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addDays(num));
        }
    }

    function getSpecialHtml(jd, location) {
        var holidays = location ? jd.getHolidays(location.Israel) : [],
            html = '';

        if (holidays.length) {
            html += getHolidayIcon(holidays) + ' ';
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
            html += addLine("Candle Lighting", jd.getCandleLighting(location), "Candle lighting time ");
        }
        html += addLine("Weekly Sedra",
            jd.getSedra(location.Israel).map(function (s) { return s.eng; }).join(' - '), "Parasha of the week");
        if (dy != null) {
            html += addLine("Daf Yomi", dy, "The Daf Yomi for this day");
        }

        if (isNaN(netz.hour)) {
            html += addLine("Netz Hachama", "The does not rise", "Sunrise");
        }
        else {
            html += addLine("Alos Hashachar - 90", (Utils.addMinutes(netz, -90)), "90 minutes before sunrise");
            html += addLine("Alos Hashachar - 72", (Utils.addMinutes(netz, -72)), "72 minutes before sunrise");
            html += addLine("Netz Hachama", netz, "Sunrise at this location");
            html += addLine("Krias Shma - MG\"A", (Utils.addMinutes(Utils.addMinutes(netz, -90), parseInt(shaaZmanis90 * 3))), "End of time to say <e>Shema</em> according to the <em>Magen Avraham</em>");
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

    function addLine(caption, value, descr) {
        return '<li data-role="list-divider">' + caption + '</li>' +
               '<li><p><h1 class="h1Zman">' + (value.hour ? Utils.getTimeString(value) : value) +
            '</h1></p><p class="ui-li-aside">' + (descr || '')
             + '</p></li>';
    }

    function showCalendar() {
        var jd = $('#divZmanimPage').jqmData('currentjDate');
        $('#divCalendarPage').jqmData('currentjDate', jd)
        $(":mobile-pagecontainer").pagecontainer("change", "#divCalendarPage", { transition: 'flip', reverse: true, showLoadMsg: true });        
    }
})();