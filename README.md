# JewishCalendar - The Luach Project - jCal.js #

A repository of Jewish Calendar projects containing:

1. ***Jewish Calendar .NET Library*** -  A .NET code library for integrating Jewish calendar capabilities into any .NET application, app or site.
1. ***jCal.js*** - A javascript stand-alone library to add full Jewish Date functionality to any Javascript code.
1. ***Luach Project***  - A Luach Windows applications
1. ***Luach Project Mobile App***  - A Luach Mobile application

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
* Compatible with the .NET micro framework.
* Code is written in C#

### jCal.js ###

* Javascript library for Jewish date calculations
* Written in pure javascript. No outside libraries needed.
* Algorithms were optimized for the slower scripting engine and target platform. The functions work quickly and efficiently on any device.
* Conversion back and forth from javascript Date objects
* Zmanim calculations for any date and location - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date and location
* Parsha of the week for any date and location
* Daf Yomi for any day since daf yomi was initiated
* Molad of any month
* Day of Sefirah - including function to get nusach of counting
* Jewish date calculation functions such as calculation of interval between dates etc.

### The Luach Project ###

* A .NET desktop full-featured Luach application
* Code is mostly C# with some components in VB.NET.
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
* As the code is pure HTML5/css/javascript, it can also be displayed in a regular browser
* jQuery Mobile as a GUI framework
* Based on **jCal.js** the JewishCalendar javascript library
* Full Zmanim for any day in history or the future for anywhere in the world. (contains a large database of locations)

### How do I get set up? ###

In the repository, there are 8  projects.

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