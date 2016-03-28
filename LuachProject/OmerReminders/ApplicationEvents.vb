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
                    My.Application.MainForm = New frmRemindHeb()
                Else
                    My.Application.MainForm = New frmRemindEng()
                End If
            Else
                If args.Contains("-lang") AndAlso args(args.IndexOf("-lang") + 1) = "heb" Then
                    My.Application.MainForm = New frmCreateRemindersHeb()
                Else
                    My.Application.MainForm = New frmCreateRemindersEng()
                End If
            End If
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