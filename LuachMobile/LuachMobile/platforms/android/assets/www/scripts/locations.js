/// <reference path="_references.js" />

(function () {
    "use strict";

    $(document).one('pagecreate', '#divChangeLocation', function () {
    });

    $(document).on("pagecontainershow", $.mobile.pageContainer, function (e, ui) {
        if (ui.toPage.attr('id') === 'divChangeLocation') {
            if (!$('#divChangeLocation').jqmData('locsList')) {
                loadLocationsList(function (nameList) {
                    var html = '';

                    for (var i = 0; i < nameList.length; i++) {
                        html += '<li><a href="#" data-name="' + nameList[i].n + '">' +
                            nameList[i].n +
                            (nameList[i].h ? ' - ' + nameList[i].h : '') +
                            '</a></li>';
                    }
                    $('#divChangeLocation #ulLocsList').html(html).listview("refresh");
                    $('#divChangeLocation #ulLocsList a').on('click', function () {
                        showZmanim($(this).data('name'));
                    });
                });
            }
        }
    });

    function loadLocationsList(callback) {
        $.getJSON('files/LocationsList.json', null, function (data) {
            $('#divChangeLocation').jqmData('locsList', data.locations);
            if (callback) {
                //sort locations by name
                callback(data.locations.sort(function (a, b) {
                    return a.n < b.n ? -1 : 1;
                }));
            }
        });
    }

    function showZmanim(name) {
        var list = $('#divChangeLocation').jqmData('locsList'),
        loc = list.first(function (i) {
            return i.n === name;
        });
        setLocation(new Location(loc.n, !!loc.i, parseFloat(loc.lt),
            parseFloat(loc.ln), parseInt(loc.tz), (loc.el ? parseInt(loc.el) : 0)), true, true);
        $(":mobile-pagecontainer").pagecontainer("change", "#divZmanimPage", { transition: 'flip', reverse: true, showLoadMsg: true });
    }
})();