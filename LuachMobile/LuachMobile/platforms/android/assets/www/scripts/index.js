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
        $('#divCalendarPage').jqmData('pageHeight', $.mobile.pageContainer.height());
        $('#divCalendarPage div[data-role=main] .ui-content').css('padding', 0);
        $('#divCalendarPage div[data-role=main] .ui-mini').css('margin', '2em 0');
        $('#tblCal td').width(parseInt($('#divCalendarPage').width() / 7) - 2);
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
        $('#divCalendarPage #divCaption').html('Location set to ' + location.Name);
        $('#divCalendarPage #emLocDet').html('lat: ' +
                location.Latitude.toString() +
                ' long:' + location.Longitude.toString() +
                (location.Israel ? ' | Israel' : '') + '  |  ' +
                (location.IsDST ? 'DST' : 'not DST'));

        var currJd = jd.addDays(-(jd.Day - 1)),
            currDOW = currJd.getDayOfWeek(),
            monthLength = jDate.daysJMonth(jd.Year, jd.Month),
            currWeek = 1,
            availHeight = $('#divCalendarPage').jqmData('pageHeight') -
                          $('#divCalendarPage div[data-role=header]').height() -
                          $('#divCalendarPage div[data-role=footer]').height() -
                          $('#tblDOW').height(),
            eachHeight = parseInt(availHeight / (monthLength === 30 && currDOW === 6 ? 8 : 7)),
            currTd;

        console.log(availHeight);
        //clear
        $('#divCalendarPage #tblCal td').html('').removeClass('hasDate').removeClass('holiday');
        while (currJd.Month === jd.Month) {
            var td = $('#divCalendarPage #tblCal tr').eq(currWeek).find('td').eq(currDOW),
                holidays = currJd.getHolidays(location.Israel),
                txt = holidays.join(' - '),
                html = '<div class="hasDate';

            if ((!!holidays.length) && txt !== 'Erev Shabbos') {
                html += ' holiday';
            }

            html += '" title="' + txt + '"><div class="jd">' +
                        Utils.toJNum(currJd.Day) +
                    '</div><div class="sd">' + currJd.getDate().getDate() + '</div>';

            if (!!holidays.length) {
                html += getHolidayIcon(holidays);
            }

            html += '</div>';

            td.jqmData('jd', currJd)
              .off('click')
              .on('click', { jd: currJd }, showZmanim)
              .html(html);

            currJd = currJd.addDays(1);
            currDOW = currJd.getDayOfWeek();
            if (currDOW === 0) {
                currWeek++;
            }
        }
        $('td > div.hasDate').css('height', eachHeight);
    }

    function showZmanim(event) {
        var jd = event.data.jd;
        $('#divZmanimPage').jqmData('currentjDate', jd);
        $(":mobile-pagecontainer").pagecontainer("change", "#divZmanimPage", { transition: 'flip' });
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
})();