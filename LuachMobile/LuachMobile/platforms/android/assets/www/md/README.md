# JewishCalendar - The Luach Project #

A repository of projects containing:

1.  ***The Jewish Calendar Library*** -  a code libabry for integrating Jewish calendar capabilities into an application, app or site.
2.  ***The Luach Project***  -  Luach Windows applications
3.  ***The Luach Project Mobile App Mobile***  - Luach Mobile application

## **The Jewish Calendar Library** ##

There are two libraries:

##### Jewish Calendar .NET Library ###

* Written in C#.
* Classes for Jewish date calculations,
* Conversion back and forth from Secular dates
* Zmanim - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date
* Parsha of the week, day
* Daf Yomi for any day since it was started
* Molad of any month

##### jCal.js - Jewish Calendar javascript Library ###

* Written in pure javascript. No outside libraries needed.
* Algorithms were optimized for the slower scripting engine and target platform.
  The functions (seem to) work quickly and efficiently on any device.
* Conversion back and forth from regular javascript Date objects
* Zmanim - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date
* Parsha of the week, day
* Daf Yomi for any day since it was started
* Molad of any month

## **The Luach Project** ##

Two Luach Applications. One for the desktop and  another for mobile.

##### The Luach Project Desktop Application ###

* Code is mostly C# with some components in VB.NET.
* Based on the JewishCalendar .net library and Jewish Date Picker controls library.
* Calendar view by either Secular or Jewish Month.
* Full Zmanim for any day in history or the future for anywhere in the world. (contains a huge database of locations)
* Allows the user to add custom occasions and events with calendar color scheme.
* Integrates the **Sefiras Haomer Reminder** application which creates Windows and Outlook daily reminders to count sefira (any nusach)
* Both Hebrew or English versions of the GUI.

##### The Luach Project Mobile App ###

* Cordova 3.5 application - can be installed on almost any device (IOS, Android, Blackberry etc.)
* Uses jQuery Mobile as a GUI framework.
* Based on **jCal.js** the JewishCalendar javascript library
* Basic functionality is in place. The app is still in the process of being built.

### How do I get set up? ###

In the repository, there are 7 projects:

1. *\JewishCalendar\JewishCalendar.csproj* - code for **JewishCalendar.dll**. Contains  The Jewish Calendar .NET Library
2. *\JewishDatePicker\JewishDatePicker.cs* - code for **JewishDatePicker.dll**. This is a windows forms control for picking Jewish dates (Similar to the regular DateTimePicker control.)
3. *\Luach\Luach.csproj - Creates Luach.exe* - A watered down version of The Luach Project application. Is included mainly to demonstrate how to use the JewishCalendar objects.
4. *\LuachMobile\LuachMobile\LuachMobile.jsproj* - Contains both the Jewish Calendar Javascript library and "The Luach Project" Cordova mobile app project.
5. *\LuachProject\LuachProject\LuachProject.csproj* - Creates **LuachProject.exe**. This is the main "Luach Project" Windows application with the full functionality.
6. *\LuachProject\OmerReminders\OmerReminders.vbproj* - Creates **OmerReminder.exe**. This Windows application creates daily Windows tasks and Outlook reminders to count Sefira. The code was written in vb.net for some reason.
7. \LuachProject\SetupLuachProject\SetupLuachProject.vdproj - This is a Visual studio installer project for The Luach Project .NET application.

**Please Note:**
The installer project for the The Luach Project Windows application can only be opened in VS 2013 and VS 2015 if you add the [Windows Installer Extention](https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9)

### Contribution guidelines ###

* The code for the libraries are fairly well documented and commented, but the applications are less so. If you don't understand some code at first, ask me.(cb@compute.co.il) beforer assuming that it is a mistake.
* Tests - yes, please! The project grew in very small increments created as they were needed for other projects - it was never expected to become a full library. Unfortunately, no tests were created.

### Who do I talk to? ###

* Chaim B. Sommers - cb@compute.co.il