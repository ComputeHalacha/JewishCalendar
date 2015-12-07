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

        var location = getLocation();
        $('#divCalendarPage #h2Header').html(jd.toStringHeb() + '<br />' + jd.getDate().toDateString());
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

        while (currJd.Month === jd.Month) {
            currTd = $('#divCalendarPage #tblCal tr').eq(currWeek).find('td').eq(currDOW).jqmData('jd', currJd).off('click').on('click', { jd: currJd }, showZmanim);
            $(currTd).html('<div class="jd">' + Utils.toJNum(currJd.Day) + '</div><div class="sd">' + currJd.getDate().getDate() + '</div>');
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