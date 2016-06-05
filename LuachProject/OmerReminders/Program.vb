Imports JewishCalendar
Imports Microsoft.Win32
Imports Microsoft.Win32.TaskScheduler

Module Program
    Friend Const RemindersTaskName As String = "Omer Reminders"

    ''' <summary>
    ''' Holds the list of locations
    ''' </summary>
    ''' <remarks></remarks>
    Friend LocationsList As List(Of Location)

    Sub New()
        LoadLocations()
    End Sub

    ''' <summary>
    ''' Gets the upcoming first day of Pesach
    ''' </summary>
    ''' <param name="today"></param>
    ''' <returns></returns>
    Friend Function GetFirstDayOfPesach(today As JewishDate) As JewishDate
        Return New JewishDate(today.Year +
                        If((today.Month > 3 AndAlso today.Month < 7) OrElse
                        (today.Month = 3 AndAlso today.Day > 5), 1, 0), 1, 15)
    End Function

    Friend Sub CreateDailyReminders(todayJD As JewishDate, alarmTime As TimeSpan)
        Dim secondDayOfPesach As JewishDate = GetFirstDayOfPesach(todayJD) + 1
        Dim isVistaPlus As Boolean = Convert.ToInt32(My.Computer.Info.OSVersion.Split(".")(0)) >= 6
        Dim path As String = Application.ExecutablePath
        Dim folder As String = Application.StartupPath
        Dim ts As New TaskService()
        Dim td As TaskDefinition = ts.NewTask()
        Dim dt As DailyTrigger = New DailyTrigger(1)
        Dim action As New ExecAction(path, "-remind" & If(My.Settings.English, "", " -lang heb"), folder)

        Try
            td.Actions.Add(action)
            dt.StartBoundary = (secondDayOfPesach.GregorianDate.Date + alarmTime)
            dt.EndBoundary = (((secondDayOfPesach + 49).GregorianDate.Date) + alarmTime).AddHours(1)
            td.Triggers.Add(dt)

            If isVistaPlus Then
                td.Principal.LogonType = TaskLogonType.InteractiveToken
                td.Principal.UserId = My.User.CurrentPrincipal.Identity.Name
                td.RegistrationInfo.Date = DateTime.Now
                td.RegistrationInfo.Author = "CBS - Compute.co.il"
                td.RegistrationInfo.Version = New Version("6.1.3")
                td.RegistrationInfo.Description = "This task was created by the Omer Reminder application. " &
                            "It runs each day at the time specified and shows a reminder to count Sefiras Ha'omer. " &
                            "After the 49th day of the Omer, the task will be automatically deleted."
                td.Settings.AllowDemandStart = True
                td.Settings.AllowHardTerminate = True
                td.Settings.StartWhenAvailable = True
                td.Settings.DeleteExpiredTaskAfter = New TimeSpan(0, 0, 0, 1)
                td.Settings.DisallowStartIfOnBatteries = False
                td.Settings.DisallowStartOnRemoteAppSession = False
                td.Settings.ExecutionTimeLimit = New TimeSpan(1, 0, 0, 0, 0)
                td.Settings.StopIfGoingOnBatteries = False
                td.Settings.WakeToRun = True
            End If
            ts.RootFolder.RegisterTaskDefinition(Program.RemindersTaskName, td)
        Catch nse As TSNotSupportedException
            Throw nse
        Catch ex As Exception
            Throw ex
        Finally
            action.Dispose()
            ts.Dispose()
            dt.Dispose()
            ts.Dispose()
        End Try
    End Sub

    Friend Sub DeleteDailyReminders()
        Using ts As New TaskService()
            ts.RootFolder.DeleteTask(Program.RemindersTaskName)
        End Using
    End Sub

    Friend Sub CreateOneTimeReminder(dt As DateTime)
        Dim isVistaPlus As Boolean = Convert.ToInt32(My.Computer.Info.OSVersion.Split(".")(0)) >= 6
        Dim path As String = Application.ExecutablePath
        Dim folder As String = Application.StartupPath
        Dim ts As New TaskService()
        Dim td As TaskDefinition = ts.NewTask()
        Dim trg As TimeTrigger = New TimeTrigger(dt) With {.EndBoundary = dt.AddDays(1)}
        Dim taskName As String = "Omer_Reminder_Temporary_" & Guid.NewGuid().ToString()
        Dim action As New ExecAction(path, "-remind -taskname " & taskName &
                                     If(My.Settings.English, "", " -lang heb"), folder)
        Try
            td.Actions.Add(action)
            td.Triggers.Add(trg)

            If isVistaPlus Then
                td.Principal.LogonType = TaskLogonType.InteractiveToken
                td.Principal.UserId = My.User.CurrentPrincipal.Identity.Name
                td.RegistrationInfo.Date = DateTime.Now
                td.RegistrationInfo.Author = "CBS - Compute.co.il"
                td.RegistrationInfo.Version = New Version("6.1.3")
                td.RegistrationInfo.Description = "This task was created by the Omer Reminder application. "
                td.Settings.StartWhenAvailable = True
                td.Settings.DeleteExpiredTaskAfter = New TimeSpan(0, 0, 0, 1)
                td.Settings.DisallowStartIfOnBatteries = False
                td.Settings.DisallowStartOnRemoteAppSession = False
                td.Settings.ExecutionTimeLimit = New TimeSpan(1, 0, 0, 0, 0)
                td.Settings.StopIfGoingOnBatteries = False
                td.Settings.WakeToRun = True
            End If
            ts.RootFolder.RegisterTaskDefinition(taskName, td)
        Catch nse As TSNotSupportedException
            Throw nse
        Catch ex As Exception
            Throw ex
        Finally
            action.Dispose()
            ts.Dispose()
            trg.Dispose()
            ts.Dispose()
        End Try
    End Sub

    Friend Function IsOutlookInstalled() As Boolean
        'Does the following line work? Or does it just show that the reference to the interop assembly exists in HKEY_CLASSES_ROOT?
        'Return Type.GetTypeFromProgID("Outlook.Application") IsNot Nothing

        Dim path As String = "Software\Microsoft\Windows\CurrentVersion\App Paths\outlook.exe"
        Dim rk As RegistryKey

        rk = Registry.CurrentUser
        rk = rk.OpenSubKey(path, False)

        If rk Is Nothing Then
            rk = Registry.LocalMachine.OpenSubKey(path, False)
        End If

        If rk IsNot Nothing Then
            rk.Close()
            rk.Dispose()
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Loads the locations from the settings.
    ''' Note: the xml format for each locations is:
    '''     <L N="Ofakim" H="אופקים" I="Y">    <!--N = name of location, H = hebrew name (optional), I = is the location in Israel? [Y = yes] (optional)-->
    '''         <T>2</T>    <!--Time zone: hour offset from UTC (AKA GMT) -->
    '''         <E>170</E>    <!--Elevation in meters (optional)-->
    '''         <LT>31.32</LT>    <!--Latitude-->
    '''         <LN>-34.62</LN>   <!--Longitude-->
    '''         <CL>30</CL>    <!--Candle-lighting: minutes before sunset (optional)-->
    '''         <TZN>Israel Standard Time</TZN>    <!--Time zone name (optional)-->
    '''     </L>
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLocations()
        LocationsList = New List(Of Location)()
        Using ms = New System.IO.StringReader(My.Resources.LocationsList)
            Dim settings = New System.Xml.XmlReaderSettings() With {.IgnoreWhitespace = True}
            Using xr = System.Xml.XmlReader.Create(ms, settings)
                While xr.ReadToFollowing("L")
                    Dim name As String = xr.GetAttribute("N").Trim()
                    Dim heb As String = If(xr.GetAttribute("H"), name)
                    Dim inIsrael As Boolean = xr.GetAttribute("I") = "Y"
                    Dim timeZone As Integer
                    Dim elevation As Integer = 0
                    Dim latitude As Double
                    Dim longitute As Double
                    Dim candleLighting As Integer
                    Dim timeZoneName As String = Nothing

                    xr.ReadToDescendant("T")
                    timeZone = xr.ReadElementContentAsInt("T", "")
                    If xr.Name = "E" Then
                        elevation = xr.ReadElementContentAsInt("E", "")
                    End If

                    latitude = xr.ReadElementContentAsDouble("LT", "")
                    longitute = xr.ReadElementContentAsDouble("LN", "")

                    If xr.Name = "CL" Then
                        candleLighting = xr.ReadElementContentAsInt("CL", "")
                    Else
                        candleLighting = If(inIsrael, 30, 18)
                    End If

                    If xr.Name = "TZN" Then
                        timeZoneName = xr.ReadElementContentAsString("TZN", "")
                    ElseIf inIsrael Then
                        timeZoneName = "Israel Standard Time"
                    End If

                    LocationsList.Add(New Location(name, timeZone, latitude, longitute) With {
                         .NameHebrew = heb,
                         .Elevation = elevation,
                         .IsInIsrael = inIsrael,
                         .TimeZoneName = timeZoneName,
                         .CandleLighting = candleLighting
                    })
                End While
                xr.Close()
            End Using
            ms.Close()
        End Using
    End Sub
End Module