/// <reference path="_references.js" />
/*jshint strict:true, undef:true */
/*global document: true, $: true, jDate:true, getLocation:true, Utils:true,getHolidayIcon:true, console:true */

(function () {
    "use strict";
    console.log('Start index.js');
    $(document).one('pagecreate', '#divCalendarPage', function () {
        console.log('RAN pagecreate on divCalendarPage');

        $('#divCalendarPage #btnNextMonth').on('click', function () { goMonth(1); });
        $('#divCalendarPage #btnNextYear').on('click', function () { goYear(1); });
        $('#divCalendarPage #btnPrevMonth').on('click', function () { goMonth(-1); });
        $('#divCalendarPage #btnPrevYear').on('click', function () { goYear(-1); });
        if (!document.onLocationChanged.first(function (i) { return !!i.divCalendarPage; })) {
            document.onLocationChanged.push({
                'divCalendarPage': locationChanged
            });
        }
        if (!document.onDevicePause.first(function (i) { return !!i.divCalendarPage; })) {
            document.onDevicePause.push({
                'divCalendarPage': function () {
                }
            });
        }
        if (!document.onDeviceResume.first(function (i) { return !!i.divCalendarPage; })) {
            document.onDeviceResume.push({
                'divCalendarPage': function () {
                    showDate();
                }
            });
        }
    })
    .on("swipeup", "#divCalendarPage", function (event) {
        goMonth(-1);
    })
    .on("swipedown", "#divCalendarPage", function (event) {
        goMonth(1);
    });

    $(document).on("pagecontainershow", $.mobile.pageContainer, function (e, ui) {
        if (ui.toPage.attr('id') === 'divCalendarPage') {
            console.log('RAN pagecontainershow for: divCalendarPage');
            //We want the calendar table to fill up the the available height of the area between the header and footer, so we need the container to have it's height set.
            $('#divCalendarPage div[data-role=main]').css({
                'height': ($.mobile.pageContainer.height() -
                    $('#divCalendarPage #divCalPageHeader').height() -
                    $('#divCalendarPage #divCalPageFooter').height()) + 'px'
            });

            //On page show, display the info for the set location
            locationChanged();
        }
    });

    function showDate(jd) {
        console.log('RAN showDate for: ' + (jd ? jd.toString() : 'UNKNOWN DATE'));
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

        setCaption(jd);
        fillCalendar(jd, getLocation());
    }

    function locationChanged(location) {
        location = location || getLocation();
        console.log('RAN index.js/locationChanged for: ' + (location ? location.Name : 'UNKNOWN'));
        if (location) {
            $('#divCalendarPage #divCaption')
                .data('locationName', location.Name)
                .html('Location set to ' + location.Name);
            $('#divCalendarPage #emLocDet').html('lat: ' +
                location.Latitude.toString() +
                ' long:' + location.Longitude.toString() +
                (location.Israel ? ' | Israel' : ''));
        }
        showDate();
    }

    function fillCalendar(jd, location) {
        //first day of current month
        var currJd = new jDate(jd.Year, jd.Month),
            //Keeps track of the d.o.w. for each day
            currDOW = currJd.getDayOfWeek(),
            //Each week gets a row
            html = '<tr>',
            //The secular date for the current day 
            currSd = currJd.getDate(),
            //Today...
            today = new jDate(new Date());

        //Set the global current date to the first day of the month
        $(document).jqmData('currentjDate', currJd);

        //If the first day of the month is not Sunday,
        if (currDOW > 0) {
            //we will fill the blank space before the first with one big blank table cell
            html += '<td colspan="' + currDOW + '"></td>';
        }

        //For each day of the month
        while (currJd.Month === jd.Month) {
            var holidays = getHolidays(currJd, location);

            //Each days td has a data attribute set to its date's absolute date number.
            //This will be used to recreate the date when the user clicks on the day
            //and we want to display the zmanim in the zmanim page.
            html += '<td data-abs="' + currJd.Abs.toString() +
                '" title="' + (holidays && holidays.length ? holidays.join(' - ') : '') + '" class="hasDate';

            //Today gets a special border
            if (currJd.Abs === today.Abs) {
                html += ' today';
            }

            //The special days get a special bg color.
            if (holidays && holidays.length) {
                //add the holiday class
                html += ' holiday';
            }

            html += '"><div class="jd">' +
                        Utils.toJNum(currJd.Day) +
                    '</div><div class="sd' +
                //The special days get a slightly different layout.
                ((holidays && holidays.length) ? ' hh' : '') +
                '">' + currSd.getDate() + '</div>';

            if (holidays && holidays.length) {
                html += '<div class="ht">' + holidays.join('<br />') + '</div>' +
                    getHolidayIcon(holidays);
            }

            html += '</td>';

            currJd = currJd.addDays(1);
            currDOW = currJd.getDayOfWeek();
            currSd = currJd.getDate();
            if (currDOW === 0) {
                html += '</tr><tr>';
            }
        }
        //If the last day of the month was Shabbos, currDOW will be Sunday as
        //we went over to the first day of the next month
        if (currDOW !== 0) {
            //we will fill the blank space after the last day with one big blank table cell
            html += '<td colspan="' + (7 - currDOW) + '"></td>';
        }
        html += '</tr>';

        $('#divCalendarPage #tblCal')
            //fill the calendar table with the html
            .html(html)
            //set the width to fill the screen
            .width($.mobile.pageContainer.width())
            //set the height to fit between the header and footer
            .height($.mobile.pageContainer.width() * 0.88); 

        $('#divCalendarPage #tblCal td.hasDate').on('click', function () {
            //Set the global current date to the clicked day            
            $(document).jqmData('currentjDate', new jDate(parseInt($(this).data('abs'))));
            //Show the zmanim for this day
            $(":mobile-pagecontainer").pagecontainer("change", "#divZmanimPage", { showLoadMsg: true });
        });
    }

    function setCaption(jd) {
        var fsdate = new jDate(jd.Year, jd.Month).getDate(),
            lsdate = new jDate(jd.Year, jd.Month, jDate.daysJMonth(jd.Year, jd.Month)).getDate(),
            html = Utils.jMonthsHeb[jd.Month] + ' ' + Utils.toJNum(jd.Year % 1000) + '<br />';

        if (fsdate.getMonth() === lsdate.getMonth()) {
            html += Utils.sMonthsEng[fsdate.getMonth()] + ' ' + fsdate.getFullYear().toString();
        }
        else if (fsdate.getFullYear() === lsdate.getFullYear()) {
            html += Utils.sMonthsEng[fsdate.getMonth()] + ' - ' + Utils.sMonthsEng[lsdate.getMonth()] +
                ' ' + fsdate.getFullYear().toString();
        }
        else {
            html += Utils.sMonthsEng[fsdate.getMonth()] + ' ' + fsdate.getFullYear().toString() + ' - ' +
                Utils.sMonthsEng[lsdate.getMonth()] + ' ' + lsdate.getFullYear().toString();
        }

        $('#divCalendarPage #h2Header').html(html);
    }

    function goMonth(num) {
        var jd = $(document).jqmData('currentjDate');
        if (jd) {
            showDate(jd.addMonths(num));
        }
    }

    function goYear(num) {
        var jd = $(document).jqmData('currentjDate');
        if (jd) {
            showDate(jd.addYears(num));
        }
    }

    function getHolidays(jd, location) {
        if (!(jd && location)) {
            return;
        }
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