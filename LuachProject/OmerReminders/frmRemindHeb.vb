Imports System.Linq
Imports JewishCalendar
Imports Microsoft.Win32.TaskScheduler

Public Class frmRemindHeb
    Private _todayJD As JewishDate
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub frmRemind_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.Hide()
        Me.SuspendLayout()
        Me.dtpRemindLater.Value = DateTime.Now.AddHours(1)
        Me._todayJD = New JewishDate(Program.LocationsList.FirstOrDefault(Function(l) l.Name = My.Settings.LocationName))
        Dim dayOfOmer As Integer = Me._todayJD.GetDayOfOmer()

        If dayOfOmer = 0 Then
            MessageBox.Show("היום אינו יום בתוך ימי ספירת העומר!",
                            "תזכורת ספירת העומר",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            Me.Close()
            Exit Sub
        End If

        Try
            Dim laOmer As Boolean = My.Settings.LaOmer
            Dim sfardi = My.Settings.Sfardi
            Dim bracha As String = "ברוך אתה יי אלוהינו מלך העולם, אשר קדשנו במצותיו וציונו על ספירת העומר:"
            Dim nusach As String = Utils.GetOmerNusach(dayOfOmer, laOmer, sfardi) & ":"
            Dim harachaman As String = If(sfardi, "הרחמן הוא יחזיר עבודת בית המקדש למקומה במהרה בימינו. אמן:",
                                           "הרחמן הוא יחזיר לנו עבודת בית המקדש למקומה במהרה בימינו, אמן סלה:")

            lblCaption.Text = "לספור ספירת העומר - יום " & dayOfOmer

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
            MessageBox.Show("ארעה תקלה בעת הצגת התזכורת:" & Environment.NewLine & ex.Message,
                                        "תזכורת ספירת העומר",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
            Me.Close()
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
            MessageBox.Show("התזכורת נרשם בהצלחה.",
                "תזכורת ספירת העומר",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            If My.Application.IsReminderRun Then
                Application.Exit()
            Else
                Me.Close()
            End If
        Catch nse As TSNotSupportedException
            MessageBox.Show("אי אפשר לרשום תזכורות בגירסת חלונות שהותקנה במחשב זה." &
                           Environment.NewLine &
                           nse.Message,
                       "תזכורת ספירת העומר",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("ארעה תקלה בעת הרשמת התזכורות." &
                            Environment.NewLine &
                            If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message),
                        "תזכורת ספירת העומר",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try
    End Sub
End Class