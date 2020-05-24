Imports System.Linq
Imports JewishCalendar
Imports Microsoft.Win32.TaskScheduler

Public Class frmRemindEng
    Public Property DayOfOmer As Integer
    Public Property Nusach As Nusach = My.Settings.Nusach
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub frmRemind_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.Hide()
        Me.SuspendLayout()
        Me.dtpRemindLater.Value = DateTime.Now.AddHours(1)
        If DayOfOmer = 0 Then
            Dim todayJD = New JewishDate(Program.LocationsList.FirstOrDefault(Function(l) l.Name = My.Settings.LocationName))
            DayOfOmer = todayJD.GetDayOfOmer()
        End If

        If DayOfOmer = 0 Then
            MessageBox.Show("Today is not a day during Sefira!",
                            "Sefira Reminder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            Me.Close()
            Exit Sub
        End If

        Try
            Dim bracha As String = "ברוך אתה יי אלוהינו מלך העולם, אשר קדשנו במצותיו וציונו על ספירת העומר:"
            Dim txt As String = Utils.GetOmerNusach(DayOfOmer, Me.Nusach) & ":"
            Dim harachaman As String = If(Me.Nusach = Nusach.Sefardi, "הרחמן הוא יחזיר עבודת בית המקדש למקומה במהרה בימינו. אמן:",
                                           "הרחמן הוא יחזיר לנו עבודת בית המקדש למקומה במהרה בימינו, אמן סלה:")

            lblCaption.Text = "Count Sefiras Ha'omer - Day " & DayOfOmer
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
                .SelectedText = txt & Environment.NewLine & Environment.NewLine
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
        If DayOfOmer = 49 AndAlso My.Application.IsReminderRun Then
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