# Jewish Calendar #

A repository of Jewish Calendar projects containing:

1. ***Jewish Calendar .NET Library*** -  A .NET code library for integrating Jewish calendar capabilities into any .NET application, app or site.
1. ***jCal.js*** - A javascript library to add full Jewish Date functionality to any Javascript code.
1. ***pyluach*** - A Python stand-alone library to add full Jewish Date functionality to any Python code.
1. ***Luach Project***  - A Luach Windows applications
1. ***Luach Project Mobile App***  - A Luach Mobile app.

### Jewish Calendar .NET Library ###

* Class library for Jewish date calculations
* Conversion back and forth from System.DateTime dates
* Zmanim calculations for any date and location - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date and location
* Parsha of the week for any date and location
* Daf Yomi for any day since daf yomi was initiated
* Molad of any month
* Day of Sefirah - including function to get nusach of counting
* Jewish date calculation functions such as calculation of interval between dates etc.
* Compatible with other .NET frameworks - such as the .Net Micro Framework etc.
* Code is written in C#

### jCal.js ###

* Javascript library for Jewish date calculations
* Written in pure javascript. No outside libraries needed.
* Algorithms were optimized for the scripting engine and target platform. The functions work quickly and efficiently on any device.
* Conversion back and forth from javascript Date objects
* Zmanim calculations for any date and location - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date and location
* Parsha of the week for any date and location
* Daf Yomi for any day since daf yomi was initiated
* Molad of any month
* Day of Sefirah - including function to get nusach of counting
* Jewish date calculation functions such as calculation of interval between dates etc.


### pyluach ###

* Python 3 library for Jewish date calculations
* Written in pure python. The only outside package dependency is tzlocal.
* Conversion back and forth from Python date and datetime objects
* Zmanim calculations for any date and location - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date and location
* Parsha of the week for any date and location
* Daf Yomi for any day since daf yomi was initiated
* Molad of any month
* Day of Sefirah - including function to get nusach of counting
* Jewish date calculation functions such as calculation of interval between dates etc.
* Calendar formatting for the Jewish Calendar - the Jewish Date equivalent to the built-in calendar module.

### The Luach Project ###

* A .NET desktop full-featured Luach application
* Code is C#. One satellite component (*Sefiras Haomer Reminder*) is written in VB.NET.
* Based on the JewishCalendar .net library and Jewish Date Picker controls library.
* Calendar view by either Secular or Jewish Month.
* Full Zmanim for any day in history or the future for anywhere in the world. (contains a large database of locations)
* Allows the user to add custom occasions and events with calendar color scheme.
* Integrates the **Sefiras Haomer Reminder** application which creates Windows and Outlook daily reminders to count sefira (any nusach)
* Full Hebrew or English versions of the GUI.

### The Luach Project Mobile App ###

* The HTML5 equivalent of The Luach Project
* Code is pure HTML5/css/javascript
* Cordova 3.5 application - can be installed on almost any device (IOS, Android, Blackberry etc.)
* As the code is pure HTML5/css/javascript, it can also be displayed in a regular browser. For example, see [online Luach](http://www.compute.co.il/Luach/).
* jQuery Mobile as a GUI framework
* Based on **jCal.js** the JewishCalendar javascript library
* Full Zmanim for any day in history or the future for anywhere in the world. (contains a large database of locations)

### How do I get set up? ###

In the repository, there are 9 projects.

1. ***\JewishCalendar\JewishCalendar.csproj***    
    The code for the JewishCalendar .NET library. Compiles to **JewishCalendar.dll**.  
    This is a stand-alone project. It does not reference any other project in this repository. 

1. ***\LuachProject\LuachProject\LuachProject.csproj***     
	Creates **LuachProject.exe**.  
	This is the main "Luach Project" Windows application with the full functionality.  
	This project references *JewishCalendar.dll*, JewishDatePicker.dll* and *OmerReminder.exe*. 
	
1. ***\LuachMobile\LuachMobile\***  
	This folder contains both jCal.js and The Luach Project Mobile app project. 
	
	The project can be opened and compliled to native mobile apps using Microsoft Visual Studio with the project ***\LuachMobile\LuachMobile\LuachMobile.jsproj***.   
	This is Visual Studio Javascript Apache Cordova project.  
	As such, it requires that the **Javascript Apache Cordova Apps** project type is installed in Visual Studio.  
	Additionally, the project requires that the [Bundler & Minifier Extension](https://visualstudiogallery.msdn.microsoft.com/9ec27da7-e24b-4d56-8064-fd7e88ac1c40)  be installed to group and minify the javascript and CSS files together. 
	
	The project can also be opened and compiled using [Intel® XDK](https://software.intel.com/en-us/intel-xdk) with the project file ***\LuachMobile\LuachMobile\LuachMobile.xdk***.  
   
	Intels XDK has a few advantages over Visual Studio in that:
	* 	You can test on IOS without an OSX computer or an Apple Developer Account by using the XDK IOS app.  
		In VS, you can only install on an IOS devive for testing if you have a computer running OSX and an Apple Developer Account (cost $99 a year). 
	* 	The XDK emulator tools are much easier to work with. 
	* 	Cross platform development. The XDK has versions for Windows, Mac and Linux.
	
  	On the other hand, code editing and debugging is somewhat smoother in VS and all the js/css minifying and bundling is taken care of for you automatically with the [Bundler & Minifier Extension](https://visualstudiogallery.msdn.microsoft.com/9ec27da7-e24b-4d56-8064-fd7e88ac1c40).

1. ***\pyluach\***    
	The code for the **pyluach** Python library.    	 
    All the pure python source code is contained in this folder.

    To use the package from the console, there are two callable modules:
    
    #### luach.py ####

    To use a python console to display zmanim for anywhere/anytime use the /pyluach/luach.py file.

    usage: luach.py [-h] [-convertdate JEWISHDATE] [-d DAYS] [-heb] [-a] [-l] location

    Outputs a formatted list of Zmanim for anywhere in the world for any Jewish Date and for any number of days.

    positional arguments:
      location              The city or location name. Doesn't need the full name,
                            the beginning of the name or a regular expression
                            search can be used. The search is not case sensitive.
                            For locations in Israel, the Hebrew name can be used as well as the English name.
                            If the supplied value matches more than one location,
                            the displayed Zmanim will be repeated for each match.
                            For example, if the supplied value is ".+wood", the
                            Zmanim of both Lakewood NJ and Hollywood California
                            will be displayed.

    optional arguments:
      -h, --help            show the help message and exit

      -convertdate JEWISHDATE, --jewishdate JEWISHDATE
                            The Jewish Date to display the Zmanim for.
                            If this argument is not supplied, the current system date is converted to a Jewish Date and used.
                            The Jewish Date should be formatted: DAY-MONTH-YEAR.
                            DAY is the day of the month.
                            MONTH is the Jewish month number, Nissan is month number 1 and Adar Sheini is 13.
                            YEAR is the full 4 digit Jewish year.
                            For example, "1-7-5778" will get the first day of Rosh Hashana 5778.
                            "13-4-5777" will get the 13th day of Tammuz 5777.
                            Alternatively, the date parts can be separated with a forward-slash (/), comma or period.

      -d DAYS, --days DAYS  The number of days forward to display.
                            If this is not supplied, a single day will be displayed.

      -heb, --hebrew        Display the Zmanim in Hebrew.

      -a, --army            Display the Zmanim time in army-time/24 hour format.
                            For example, for a Zman of 10:10 PM, 22:10 will be displayed.

      -l, --locations       Instead of displaying the Zmanim, display the list of
                            locations returned from the "location" argument search.
                            Shows each locations name, latitude, longitude, elevation, utcoffset and hebrew name.
                            To show the full list of all the locations, use: luach.py .+ -l

    For example, to show all the Zmanim for all the days of Sukkos 5777 for both Lakewood NJ and Brooklyn NY,
    Use: luach.py "lakewood|brooklyn" -convertdate 15-7-5777 -d 9

    To show the Zmanim in Hebrew for Tisha B'av in Jerusalem in the year 3248 (the year the Beis Hamikdash was destroyed),
     Use: luach.py "ירושלים" -convertdate 9-5-3248 -h

    #### jcalendar.py ####

    Calendar printing functions for Jewish Dates.
    Most functions are pretty-close equivalents to the built-in calendar functions.

    For this module the first day of the week is always Sunday.

    Usage: jcalendar.py [options] [year [month]]

    Options:
      -h, --help            show this help message and exit
      -w WIDTH, --width=WIDTH
                            width of date column (default 2, text only)
      -l LINES, --lines=LINES
                            number of lines for each week (default 1, text only)
      -s SPACING, --spacing=SPACING
                            spacing between months (default 6, text only)
      -m MONTHS, --months=MONTHS
                            months per row (default 3, text only)
      -c CSS, --css=CSS     CSS to use for page (html only)
      -e ENCODING, --encoding=ENCODING
                            Encoding to use for output.
      -t TYPE, --type=TYPE  output type (text or html)
	
1. ***\JewishDatePicker\JewishDatePicker.cs***    
	 The code for the **JewishDatePicker.dll**.    
	 This is a windows forms control for picking Jewish dates (Similar to the regular DateTimePicker control.). This project references the  *JewishCalendar.dll*. 
	 
1. ***\Luach\Luach.csproj***    
	Creates **Luach.exe** - A watered down version of The Luach Project application.   
	Is included mainly to demonstrate how to use the JewishCalendar objects.  
	This project references both *JewishCalendar.dll* and *JewishDatePicker.dll*. 
	
1. ***\LuachProject\OmerReminders\OmerReminders.vbproj***    
	Creates **OmerReminder.exe**.  
	This Windows application creates daily Windows tasks and Outlook reminders to count Sefira.  
	The code was written in vb.net for some reason.  
	This project references *JewishCalendar.dll*. 
	
1. ***\LuachProject\SetupLuachProject\SetupLuachProject.vdproj***    
	This is a Visual studio installer project for creating a Windows Installer for **The Luach Project** .NET application.  
	This project is dependent on *\LuachMobile\LuachMobile\LuachMobile.jsproj*.  
	**PLEASE NOTE:**  
	A Visual Studio installer project cannot be opened in VS 2013 or VS 2015 unless you add the [Windows Installer Extention](https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9)
	
1. ***\JewishCalendar\TestBenchmarks\TestBenchmarks.csproj***    
	A console application for benchmarking.  
	Used primarily to compare the different implementations of the Jewish Date calculations.  
	This project references *JewishCalendar.dll*.

### Contribution guidelines ###

* The code for the libraries are fairly well documented and commented, but the applications are less so. If you find an obvious mistake or bug, please don't hesitate to change. But if you just don't understand the reasoning behind some logic etc., it would be great if I could be contacted (cb@compute.co.il) before changing. There is that minuscule chance that there is a sensible explanation for the weirdness. 
* Tests - yes, please! The project grew in very small increments created as they were needed for other projects - it was never expected to become a full library. Unfortunately, no tests were created.

### Who do I talk to? ###

* Chaim B. Sommers - cb@compute.co.il