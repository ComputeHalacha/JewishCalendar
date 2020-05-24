Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    '
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private _isReminderRun As Boolean

        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Dim args As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs()
            If args.Contains("-location") Then
                My.Settings.LocationName = args(args.IndexOf("-location") + 1)
            End If
            If args.Contains("-remind") OrElse args.Contains("/remind") Then
                Me._isReminderRun = True
                If args.Contains("-lang") AndAlso args(args.IndexOf("-lang") + 1) = "heb" Then
                    Dim frmReminder As New frmRemindHeb()
                    If args.Contains("-omerDay") Then
                        frmReminder.DayOfOmer = Convert.ToInt32(args(args.IndexOf("-omerDay") + 1))
                    End If
                    If args.Contains("-nusach") Then
                        frmReminder.Nusach = DirectCast([Enum].Parse(GetType(JewishCalendar.Nusach), args(args.IndexOf("-nusach") + 1)), JewishCalendar.Nusach)
                    End If
                    My.Application.MainForm = frmReminder
                Else
                    Dim frmReminder As New frmRemindEng()
                    If args.Contains("-omerDay") Then
                        frmReminder.DayOfOmer = Convert.ToInt32(args(args.IndexOf("-omerDay") + 1))
                    End If
                    If args.Contains("-nusach") Then
                        frmReminder.Nusach = DirectCast([Enum].Parse(GetType(JewishCalendar.Nusach), args(args.IndexOf("-nusach") + 1)), JewishCalendar.Nusach)
                    End If
                    My.Application.MainForm = frmReminder
                End If
                'If the -taskname flag is supplied, the task is a one time reminder and should be deleted.
                If args.Contains("-taskname") Then
                    Using ts As New Microsoft.Win32.TaskScheduler.TaskService()
                        ts.RootFolder.DeleteTask(args(args.IndexOf("-taskname") + 1))
                    End Using
                End If
            Else
                If args.Contains("-lang") AndAlso args(args.IndexOf("-lang") + 1) = "heb" Then
                    My.Application.MainForm = New frmCreateRemindersHeb()
                Else
                    My.Application.MainForm = New frmCreateRemindersEng()
                End If
            End If
        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
            FileIO.FileSystem.WriteAllText("OmerNusach", Settings.Nusach.ToString(), False)
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(e.Exception.Message)
        End Sub

        Public ReadOnly Property IsReminderRun() As Boolean
            Get
                Return _isReminderRun
            End Get
        End Property
    End Class
End Namespace