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
4. \LuachMobile\LuachMobile\LuachMobile.jsproj - Contains both the Jewish Calendar Jacascript library and the cordova mobile app project.

* Configuration
* The repository does not contain any projects that rely on any outside dependencies. Many of the projects depend on each other internally...
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests - yes, please! the project grew in small pieces as I needed them for other projects - I never expected it to become a full library. No tests was included.
* Please do not commit any revision to the main branch that does not compile without any errors or warnings whatsoever.

### Who do I talk to? ###

* CB Sommers - cb@compute.co.il