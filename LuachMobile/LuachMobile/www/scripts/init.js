/*jshint strict:true, undef:true */
/*global document: true, $: true */
(function () {
    "use strict";
    $( document ).on( "mobileinit", function() {
       //Cache all pages
        $.mobile.page.prototype.options.domCache = true;
        $.mobile.defaultPageTransition = 'flip';
        //preload calendar page
        $(":mobile-pagecontainer").pagecontainer("load", "#divCalendarPage", { showLoadMsg: false }); 
    });
})();