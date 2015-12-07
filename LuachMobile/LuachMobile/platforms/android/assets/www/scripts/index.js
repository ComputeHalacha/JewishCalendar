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
        $('#divCaption').html('Location set to ' + location.Name);
        $('#emLocDet').html('lat: ' +
                location.Latitude.toString() +
                ' long:' + location.Longitude.toString() +
                (location.Israel ? ' | Israel' : '') + '  |  ' +
                (location.IsDST ? 'DST' : 'not DST'));

        var currJd = jd.addDays(jd.Day - 1),
            currDOW = currJd.getDayOfWeek(),
            monthLength = jDate.daysJMonth(jd.Year, jd.Month),
            currWeek = 1,
        currTd;

        while (currJd.Month === jd.Month && currJd.Day < monthLength) {
            currTd = $('#tblCal tr').eq(currWeek).find('td').eq(currDOW);
            $(currTd).append('<strong>' + currJd.Day + '&nbsp;' + Utils.toJNum(currJd.Day) + '</strong>');
            console.log('Did day: ' + currJd.Day.toString());
            currJd = currJd.addDays(1);
            currDOW = currJd.getDayOfWeek();
            if (currDOW === 6) {
                currWeek++;
            }
        }
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
})();