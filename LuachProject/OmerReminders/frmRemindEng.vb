Imports System.Linq
Imports JewishCalendar
Imports Microsoft.Win32.TaskScheduler

Public Class frmRemindEng
    Private _todayJD As JewishDate_
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub frmRemind_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.Hide()
        Me.SuspendLayout()
        Me.dtpRemindLater.Value = DateTime.Now.AddHours(1)
        Me._todayJD = New JewishDate_(Program.LocationsList.FirstOrDefault(Function(l) l.Name = My.Settings.LocationName))
        Dim dayOfOmer As Integer = Me._todayJD.GetDayOfOmer()

        If dayOfOmer = 0 Then
            MessageBox.Show("Today is not a day during Sefira!",
                            "Sefira Reminder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            Me.Close()
            Exit Sub
        End If

        Try
            Dim laOmer As Boolean = My.Settings.LaOmer
            Dim sfardi As Boolean = My.Settings.Sfardi
            Dim bracha As String = "ברוך אתה יי אלוהינו מלך העולם, אשר קדשנו במצותיו וציונו על ספירת העומר:"
            Dim nusach As String = Utils.GetOmerNusach(dayOfOmer, laOmer, sfardi) & ":"
            Dim harachaman As String = If(sfardi, "הרחמן הוא יחזיר עבודת בית המקדש למקומה במהרה בימינו. אמן:",
                                           "הרחמן הוא יחזיר לנו עבודת בית המקדש למקומה במהרה בימינו, אמן סלה:")

            lblCaption.Text = "Count Sefiras Ha'omer - Day " & dayOfOmer
            Me.Text = Me.lblCaption.Text

            With Me.RichTextBox1
                .Text = ""
                .SelectedText = Environment.NewLine
                .SelectionAlignment = HorizontalAlignment.Center
                .SelectionIndent = 20
                .SelectionRightIndent = 20
                .SelectedText = bracha & Environment.NewLine & Environment.NewLine
                .SelectionFont = New Font(.Font.FontFamily, .Font.Size + 15, FontStyle.Bold)
                .SelectionColor = Color.Maroon
                .SelectedText = nusach & Environment.NewLine & Environment.NewLine
                .SelectionColor = .ForeColor
                .SelectionFont = .Font
                .SelectedText = harachaman
                .HideSelection = True
            End With

        Catch ex As Exception
            MessageBox.Show("There was a problem during the display of the Omer Reminder:" & Environment.NewLine & ex.Message,
                                        "Show Windows Reminder",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
            Me.Close()
            Exit Sub
        End Try

        Me.ResumeLayout()
        Me.Show()

        'If today is the last day of the Omer, try to remove tasks.
        If dayOfOmer = 49 AndAlso My.Application.IsReminderRun Then
            Try
                Program.DeleteDailyReminders()
            Catch
            End Try
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If My.Application.IsReminderRun Then Application.Exit()
    End Sub

    Private Sub btnRemindLater_Click(sender As Object, e As EventArgs) Handles btnRemindLater.Click
        Do While Me.dtpRemindLater.Value < DateTime.Now
            Me.dtpRemindLater.Value = Me.dtpRemindLater.Value.AddDays(1)
        Loop

        Try
            Program.CreateOneTimeReminder(Me.dtpRemindLater.Value)
            MessageBox.Show("Reminder was successfully created.",
                "Create Windows Reminder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            If My.Application.IsReminderRun Then
                Application.Exit()
            Else
                Me.Close()
            End If
        Catch nse As TSNotSupportedException
            MessageBox.Show("This action is not supported on your operating system." &
                           Environment.NewLine &
                           nse.Message,
                       "Create Windows Reminder",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("There was a problem during the registration of the Window Tasks Reminders:" &
                            Environment.NewLine &
                            If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message),
                        "Create Windows Reminder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try
    End Sub
End Class