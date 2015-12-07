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
        $('#tblCal, #divTblCal').height(parseInt($(document).height() * 0.63)).offset({ left: -8 }).width($(document).width() - 7);
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
        currTd;
        //clear
        $('#divCalendarPage #tblCal td').html('');
        while (currJd.Month === jd.Month) {
            var td = $('#divCalendarPage #tblCal tr').eq(currWeek).find('td').eq(currDOW),
                holidays = currJd.getHolidays(location.Israel),
                html = '<div class="haDate';

            if (!!holidays.length) {
                html += ' holiday';
            }

            html += '"><div class="jd">' +
                        Utils.toJNum(currJd.Day) +
                    '</div><div class="sd">' + currJd.getDate().getDate() + '</div>';

            if (!!holidays.length) {
                var txt = holidays.join(' - ');
                if (txt.has('Rosh Hashana') && !txt.has('Erev Rosh Hashana')) {
                    html += '<i class="fa fa-balance-scale"></i>';
                }
                if (txt.has('Yom Kippur') && !txt.has('Erev Yom Kippur')) {
                    html += '<i class="fa fa-book"></i>';
                }
                if (txt.has('Succos') && !txt.has('Erev Succos')) {
                    html += '<i class="fa fa-inbox"></i>';
                }
                if (txt.has('Chanuka')) {
                    html += '<i class="fa fa-fire"></i>';
                }
                if (txt.has('Tu B\'Shvat')) {
                    html += '<i class="fa fa-apple"></i>';
                }
                if (txt.has('Purim') && !txt.has('Purim Katan')) {
                    html += '<i class="fa fa-beer"></i>';
                }
                if (txt.has('Pesach') && !txt.has('Erev Pesach')) {
                    html += '<i class="fa fa-soccer-ball-o"></i>';
                }
                if (txt.has('Shavuos') && !txt.has('Erev Shavuos')) {
                    html += '<i class="fa fa-pagelines"></i>';
                }
                if (txt.has('Fast') || txt.has('Tzom') || txt.has('Tisha B\'Av')) {
                    html += '<i class="fa fa-ban"></i>';
                }
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