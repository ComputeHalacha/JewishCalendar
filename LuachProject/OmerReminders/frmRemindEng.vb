Imports System.Linq
Imports JewishCalendar
Imports Microsoft.Win32.TaskScheduler

Public Class frmRemindEng
    Private _todayJD As JewishDate
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If My.Settings.English Then
            Me.RightToLeft = Windows.Forms.RightToLeft.No
            Me.RightToLeftLayout = False
        Else
            Me.RightToLeft = Windows.Forms.RightToLeft.Yes
            Me.RightToLeftLayout = True
        End If
    End Sub

    Private Sub frmRemind_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.Hide()
        Me.SuspendLayout()

        Me._todayJD = New JewishDate(Program.LocationsList.FirstOrDefault(Function(l) l.Name = My.Settings.LocationName))
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
            Dim english As Boolean = My.Settings.English
            Dim laOmer As Boolean = My.Settings.LaOmer
            Dim sfardi As Boolean = My.Settings.Sfardi
            Dim bracha As String = "ברור אתה יי אלוהינו מלך העולם, אשר קדשנו במצותיו וציונו על ספירת העומר:"
            Dim nusach As String = Utils.GetOmerNusach(dayOfOmer, laOmer, sfardi) & ":"
            Dim harachaman As String = If(sfardi, "הרחמן הוא יחזיר עבודת בית המקדש למקומה במהרה בימינו. אמן:",
                                           "הרחמן הוא יחזיר לנו עבודת בית המקדש למקומה במהרה בימינו, אמן סלה:")

            If english Then
                lblCaption.Text = "Count Sefiras Ha'omer - Day " & dayOfOmer
            Else
                lblCaption.Text = "לספור ספירת העומר - יום " & dayOfOmer
            End If

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
                Dim ts As New TaskService()
                ts.RootFolder.DeleteTask("Omer Reminders")
            Catch
            End Try
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If My.Application.IsReminderRun Then Application.Exit()
    End Sub
End Class