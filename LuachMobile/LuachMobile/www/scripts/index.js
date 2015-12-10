/// <reference path="_references.js" />

(function () {
    "use strict";
    document.onDeviceReady = function () {
        showDate();
    };

    document.onDevicePause = function () {
    };

    document.onDeviceResume = function () {
        showDate();
    };

    document.onLocationChanged = function () {
        showDate();
    };

    $(document).on('pagecreate', '#divCalendarPage', function () {
        $('#divCalendarPage div[data-role=main] .ui-content').css({ 'padding': '0', 'height': '100%' });
        $('#divCalendarPage div[data-role=main] .ui-mini').css('margin', '2em 0');
        $('#divCalendarPage #btnNextMonth').on('click', function () { goMonth(1); });
        $('#divCalendarPage #btnNextYear').on('click', function () { goYear(1); });
        $('#divCalendarPage #btnPrevMonth').on('click', function () { goMonth(-1); });
        $('#divCalendarPage #btnPrevYear').on('click', function () { goYear(-1); })
            .on("swipeup", "#divCalendarPage", function (event) {
                goMonth(-1);
            }).on("swipedown", "#divCalendarPage", function (event) {
                goMonth(1);
            });
        if (!window.cordova) {
            showDate();
        }
    });

    $(document).on("pagecontainershow", $.mobile.pageContainer, function (e, ui) {
        if (ui.toPage.attr('id') === 'divCalendarPage') {
            //We want the calendar table to fill up the the available height of the area between the header and footer, so we need the container to have it's height set.
            $('#divCalendarPage div[data-role=main]').css({
                'height': ($.mobile.pageContainer.height() - $('#divCalendarPage #divCalPageHeader').height() - $('#divCalendarPage #divCalPageFooter').height()) + 'px'
            });

            //Redraw the calendar if the calendar is empty or if the location was changed from another page
            if (!$('#divCalendarPage #tblCal').html() ||
                $('#divCalendarPage #divCaption').data('locationName') !== getLocation()) {
                showDate();
            }
        }
    });

    function showDate(jd) {
        if (jd) {
            $('#divCalendarPage').jqmData('currentjDate', jd);
        }
        else if ($('#divCalendarPage').jqmData('currentjDate')) {
            jd = $('#divCalendarPage').jqmData('currentjDate');
        }
        else {
            showDate(new jDate(new Date()));
            return;
        }

        var location = getLocation(),
            sdate = jd.getDate();

        $('#divCalendarPage #h2Header').html(Utils.jMonthsHeb[jd.Month] + ' ' +
            Utils.toJNum(jd.Year % 1000) + '<br />' +
            Utils.sMonthsEng[sdate.getMonth()] + ' ' + sdate.getFullYear().toString());
        $('#divCalendarPage #divCaption').data('locationName', location.Name).html('Location set to ' + location.Name);
        $('#divCalendarPage #emLocDet').html('lat: ' +
                location.Latitude.toString() +
                ' long:' + location.Longitude.toString() +
                (location.Israel ? ' | Israel' : '') + '  |  ' +
                (location.IsDST ? 'DST' : 'not DST'));

        fillCalendar(jd, location);
    }

    function showZmanim(jd) {
        $('#divZmanimPage').jqmData('currentjDate', jd);
        $(":mobile-pagecontainer").pagecontainer("change", "#divZmanimPage", { transition: 'flip' });
    }

    function fillCalendar(jd, location) {
        //first day of current month
        var currJd = new jDate(jd.Year, jd.Month),
            //Keeps track of the d.o.w. for each day
            currDOW = currJd.getDayOfWeek(),
            //Each week gets a row
            html = '<tr>';

        //If the first day of the month is not Sunday,
        if (currDOW > 0) {
            //we will fill the blank space before the first with one big blank table cell
            html += '<td colspan="' + currDOW + '"></td>'
        }

        //For each day of the month
        while (currJd.Month === jd.Month) {
            var holidays = getHolidays(currJd, location);

            //Each days td has a data attribute set to its date's absolute date number.
            //This will be used to recreate the date when the user clicks on the day
            //and we want to display the zmanim in the zmanim page.
            html += '<td data-abs="' + currJd.Abs.toString() +
                '" title="' + holidays.join(' - ') + '" class="hasDate';

            //The special days get a special bg color.
            if (!!holidays.length) {
                //add the holiday class
                html += ' holiday';
            }

            html += '"><div class="jd">' +
                        Utils.toJNum(currJd.Day) +
                    '</div><div class="sd">' + currJd.getDate().getDate() + '</div>';

            if (!!holidays.length) {
                html += '<div class="ht">' + holidays.join('<br />') + '</div>' +
                    getHolidayIcon(holidays);
            }

            html += '</td>';

            currJd = currJd.addDays(1);
            currDOW = currJd.getDayOfWeek();
            if (currDOW === 0) {
                html += '</tr><tr>';
            }
        }
        //If the last day of the month was Shabbos, currDOW will be Sunday as
        //we went over to the first day of the next month
        if (currDOW !== 0) {
            //we will fill the blank space after the last day with one big blank table cell
            html += '<td colspan="' + (7 - currDOW) + '"></td>'
        }
        html += '</tr>';

        $('#divCalendarPage #tblCal').html(html).width($.mobile.pageContainer.width());

        $('#divCalendarPage #tblCal td.hasDate').on('click', function () {
            showZmanim(new jDate(parseInt($(this).data('abs'))));
        });
    }

    function goMonth(num) {
        var jd = $('#divCalendarPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addMonths(num));
        }
    }

    function goYear(num) {
        var jd = $('#divCalendarPage').jqmData('currentjDate');
        if (jd) {
            showDate(jd.addYears(num));
        }
    }

    function getHolidays(jd, location) {
        var holidays = jd.getHolidays(location.Israel);
        for (var i = holidays.length; i >= 0; i--) {
            if (holidays[i] === 'Shabbos Kodesh') {
                //Instead of Shabbos Kodesh, we just show the parsha
                holidays[i] = jd.getSedra(location.Israel).map(function (s) {
                    return s.eng;
                }).join(' - ');
            }
            else if (holidays[i] === 'Erev Shabbos') {
                //No room on a mobile to show Erev Shabbos - it's obviously Friday in any case....
                holidays.splice(i, 1);
            }
        }
        return holidays;
    }
})();