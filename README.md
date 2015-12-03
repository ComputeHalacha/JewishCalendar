# README - JewishCalendar - The Luach Project #

**Jewish Calendar** is a repository of open source projects for:

1. "The Luach Project" applications 
2. The Jewish Calendar Library which is for integrating Jewish calendar capabilities into any application, app or site.

# JewishCalendar - The Luach Project #

## **The Luach Project** ##

The Luach Project is a pair of Luach applications:

### Windows Forms Application ###

* Code is mostly C# with some components in VB.NET.
* Based on the JewishCalendar .net library and Jewish Date Picker controls library.
* Calendar view by either Secular or Jewish Month.
* Full Zmanim for any day in history or the future for anywhere in the world. (contains a huge database of locations)
* Allows the user to add custom occasions and events with calendar color scheme.
* Integrates the **Sefiras Haomer Reminder** application which creates Windows and Outlook daily reminders to count sefira (any nusach)
* Both Hebrew or English versions of the GUI.

### Mobile App ###

* Cordova 3.5 application - can be installed on almost any device (IOS, Android, Blackberry etc.)
* GUI uses jQuery Mobile
* Based on the JewishCalendar javascript library
* Basic functionality is in place. The app is still in the process of being built.

## **Jewish Calendar Libraries** ##

There are two versions of the libraries:

### Jewish Calendar .NET Library ###

* Written in C#.
* Classes for Jewish date calculations, 
* Conversion back and forth from Secular dates
* Zmanim - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date
* Parsha of the week, day
* Daf Yomi for any day since it was started
* Molad of any month

### Jewish Calendar javascript Library ###

* Classes for Jewish date calculations. Algorithms were optimized for the slower engine. They functions seem to work quickly and efficiently on any device.
* Conversion back and forth from regular javascript Date objects
* Zmanim - sunrise/sunset, chatzos, sha'a zmanis etc. for any date and location.
* Jewish Holidays/Fasts etc, for any date
* Parsha of the week, day
* Daf Yomi for any day since it was started
* Molad of any month

### How do I get set up? ###

There are three Visual Studio solutions containing a total of 7 projects:

1. \JewishCalendar\JewishCalendar.csproj - creates JewishCalendar.dll. Contains the heart of the Jewish Calendar .NET library
2. \JewishDatePicker\JewishDatePicker.cs - creates JewishDatePicker.dll. This is a windows forms control for picking Jewish dates (Similar to the regular DateTimePicker control.)
3. \Luach\Luach.csproj - Creates Luach.exe. A watered down version of the Luach Project application. Is included mainly to demonstrate how to use the JewishCalendar objects.
4. \LuachMobile\LuachMobile\LuachMobile.jsproj - Contains both the Jewish Calendar Javascript library and "The Luach Project" Cordova mobile app project.
5. \LuachProject\LuachProject\LuachProject.csproj - Creates LuachProject.exe. This is the main "Luach Project" Windows application with the full functionality.
6. \LuachProject\OmerReminders\OmerReminders.vbproj - Creates OmerReminder.exe. This Windows application creates daily Windows tasks and Outlook reminders to count Sefira. The code was written in vb.net for some reason.
7. \LuachProject\SetupLuachProject\SetupLuachProject.vdproj - This is a Visual studio installer project for The Luach Project .NET application.


The installer project for the The Luach Project Windows application can only be opened in VS 2013 and VS 2015 if you add the [Windows Installer Extention](https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9)

### Contribution guidelines ###

* Writing tests - yes, please! The project grew in very small increments created as I needed them for other projects - I never expected it to become a full library. Unfortunately, no tests were created.
* Please do not commit any revision to the main branch that does not compile without any errors or warnings whatsoever.

### Who do I talk to? ###

* CB Sommers - cb@compute.co.il