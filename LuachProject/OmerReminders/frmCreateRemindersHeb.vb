Imports System.Linq
Imports JewishCalendar
Imports Outlook = Microsoft.Office.Interop.Outlook

Public Class frmCreateRemindersHeb
    Private _loaded As Boolean
    Private _todayJD As JewishDate_ = Nothing

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FillLocations()

        Dim loc As Location = Nothing
        If Not String.IsNullOrEmpty(My.Settings.LocationName) Then
            loc = Program.LocationsList.FirstOrDefault(Function(l) l.Name = My.Settings.LocationName)
        End If

        If loc IsNot Nothing Then
            Me.cbLocations.SelectedItem = loc
        Else
            'First item in the list
            loc = DirectCast(Me.cbLocations.SelectedItem, Location)
        End If

        My.Settings.English = False

        Me._todayJD = New JewishDate_(loc)
        Me.rbIsrael.Checked = loc.IsInIsrael
        Me.rbBaOmer.Checked = (Not Me.rbLaOmer.Checked) AndAlso (Not rbSfardi.Checked)
        Me.ShowLinkToReminder()
        Me._loaded = True
        Me.btnCreateOutlookReminders.Enabled = Me.btnDeleteOutlookReminders.Enabled = Program.IsOutlookInstalled()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
        My.Settings.Save()
    End Sub

    Private Sub btnCreateOutlookReminders_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreateOutlookReminders.Click
        If Not Me.FieldsAreValid() Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        If Not Program.IsOutlookInstalled() Then
            MessageBox.Show("תוכנת אוטלוק לא נמצא במחשב זה.",
                            "תזכורת ספירת העומר",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Return
        End If

        Dim olApp As Outlook.Application

        Try
            olApp = New Outlook.Application()

            Dim oNameSpace As Outlook.NameSpace = olApp.GetNamespace("MAPI")
            Dim objApt As Outlook.AppointmentItem
            Dim firstDayOfPesach As JewishDate_ = Program.GetFirstDayOfPesach(Me._todayJD)
            Dim count As Integer

            oNameSpace.Logon()

            For i As Integer = 1 To 49
                Dim jd As JewishDate_ = firstDayOfPesach + i

                If jd >= Me._todayJD Then
                    Dim dayOfOmer As Integer = jd.GetDayOfOmer()
                    Dim nusach As String = Utils.GetOmerNusach(dayOfOmer, Me.rbLaOmer.Checked, Me.rbSfardi.Checked)
                    Dim yesterday As JewishDate_ = jd - 1
                    Dim alarmTime As TimeSpan = Me.dtpTime.Value.TimeOfDay
                    Dim subs As String = "ספירת העומר - יום " & dayOfOmer

                    count += 1

                    If cbAddAlldayEvent.Checked Then
                        'All day event
                        objApt = olApp.CreateItem(Outlook.OlItemType.olAppointmentItem)
                        objApt.ReminderSet = False
                        objApt.Importance = Outlook.OlImportance.olImportanceHigh
                        objApt.Start = jd.GregorianDate
                        objApt.AllDayEvent = True
                        objApt.Subject = subs
                        objApt.Save()
                    End If

                    'Actual Reminder
                    objApt = olApp.CreateItem(Outlook.OlItemType.olAppointmentItem)
                    objApt.ReminderSet = True
                    objApt.Importance = Outlook.OlImportance.olImportanceHigh
                    objApt.AllDayEvent = False
                    objApt.Start = Me.GetAlarmDateTime(yesterday.GregorianDate, jd.GregorianDate, alarmTime)
                    objApt.ReminderMinutesBeforeStart = 0
                    objApt.Subject = "לספור " & subs &
                        " - " & nusach
                    objApt.Body = nusach
                    objApt.Save()

                    objApt = Nothing
                End If
            Next i
            Me.Cursor = Cursors.Default
            MessageBox.Show(count.ToString() & " תזכורות יוצרו בהצלחה.")
        Catch ex As Exception
            MessageBox.Show("ארעה תקלה בעת יצירת התזכורות." & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            olApp = Nothing
        End Try
    End Sub

    Private Sub btnDeleteOutlookReminders_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteOutlookReminders.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim olApp As Outlook.Application = New Outlook.Application()
            Dim oNameSpace As Outlook.NameSpace = olApp.GetNamespace("MAPI")
            Dim objApt As Outlook.AppointmentItem
            Dim count As Integer

            'I have no idea why it all doesn't get deleted on the first run
            For i As Integer = 0 To 5
                For Each objApt In oNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar).Items
                    If objApt.Start.Year = DateTime.Now.Year AndAlso
                        (objApt.Subject.Contains("Sefiras Ha'omer") OrElse objApt.Subject.Contains("ספירת העומר")) Then
                        objApt.Delete()
                        count += 1
                    End If
                Next objApt
            Next

            olApp = Nothing
            oNameSpace = Nothing
            objApt = Nothing
            Me.Cursor = Cursors.Default
            MessageBox.Show("נמחקו " & count.ToString() & " תזכורות או אירועי יום של ספירת העומר.")
        Catch ex As Exception
            MessageBox.Show("ארעה תקלה בעת הסרת התזכורות." & vbCrLf & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCreateWindowsReminders_Click(sender As Object, e As EventArgs) Handles btnCreateWindowsReminders.Click
        If Not Me.FieldsAreValid() Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        Try
            Program.CreateDailyReminders(Me._todayJD, Me.dtpTime.Value.TimeOfDay)
            MessageBox.Show("התזכורות הורשמו בהצלחה.",
                "תזכורת ספירת העומר",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        Catch nse As Microsoft.Win32.TaskScheduler.TSNotSupportedException
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
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnDeleteWindowsReminders_Click(sender As Object, e As EventArgs) Handles btnDeleteWindowsReminders.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Program.DeleteDailyReminders()
            MessageBox.Show("התזכורות הוסרו בהצלחה.",
                                "תזכורת ספירת העומר",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("ארעה תקלה בעת הסרת התזכורות." & Environment.NewLine & ex.Message,
                            "תזכורת ספירת העומר",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbLocs_CheckedChanged(sender As Object, e As EventArgs) Handles rbWorld.CheckedChanged
        If Me._loaded Then
            Me.FillLocations()
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            Me.ShowLinkToReminder()
        End If
    End Sub

    Private Sub llPreviewToday_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llPreviewToday.LinkClicked
        If Me._loaded Then
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            My.Settings.Save()
            Dim f As New frmRemindHeb()
            f.ShowDialog()
            f.Dispose()
        End If
    End Sub

    Private Sub cbLocations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbLocations.SelectedIndexChanged
        If Me._loaded Then
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            Me.ShowLinkToReminder()
        End If
    End Sub

    Private Sub cbLocations_Format(sender As Object, e As ListControlConvertEventArgs) Handles cbLocations.Format
        Dim l As Location = e.ListItem
        e.Value = If(Not String.IsNullOrEmpty(l.NameHebrew), l.NameHebrew, l.Name)
    End Sub

    Private Sub FillLocations()
        Dim loaded = Me._loaded 'will get us back to where we started
        Me._loaded = False

        Dim isWorld As Boolean = Me.rbWorld.Checked

        Me.cbLocations.Items.Clear()
        Dim locList As IEnumerable(Of Location) = Program.LocationsList.Where(Function(l) l.IsInIsrael <> isWorld)

        'First show those with Hebrew names
        For Each loc As Location In (From l In locList Where Not String.IsNullOrEmpty(l.NameHebrew) Order By l.NameHebrew)
            Me.cbLocations.Items.Add(loc)
        Next

        'Next show those that don't have it...
        For Each loc As Location In (From l In locList Where String.IsNullOrEmpty(l.NameHebrew) Order By l.Name)
            Me.cbLocations.Items.Add(loc)
        Next

        Me.cbLocations.SelectedIndex = 0

        'I don't know why the above line messes up the check property of the radio buttons - but it does,
        'so we need to reset them.
        Me.rbWorld.Checked = isWorld
        Me.rbIsrael.Checked = Not isWorld
        Me._loaded = loaded
    End Sub

    Private Sub rbLaOmer_Click(sender As Object, e As EventArgs) Handles rbLaOmer.Click
        Me.rbLaOmer.Checked = True
    End Sub

    Private Sub rbBaOmer_Click(sender As Object, e As EventArgs) Handles rbBaOmer.Click
        Me.rbBaOmer.Checked = True
    End Sub

    Private Sub rbSfardi_Click(sender As Object, e As EventArgs) Handles rbSfardi.Click
        Me.rbSfardi.Checked = True
    End Sub

    Private Function GetAlarmDateTime(yesterday As Date, today As Date, alarmTime As TimeSpan) As DateTime
        Dim shkiaTimeYesterday As TimeSpan = New Zmanim(yesterday, Me.cbLocations.SelectedItem).GetShkia()
        Return If(alarmTime >= shkiaTimeYesterday AndAlso alarmTime <= New TimeSpan(23, 59, 59), yesterday, today) + alarmTime
    End Function

    Private Sub ShowLinkToReminder()
        Dim currentDayOfOmer As Integer = Me._todayJD.GetDayOfOmer()
        If currentDayOfOmer > 0 Then
            Me.llPreviewToday.Visible = True
            Me.llPreviewToday.Text = "היום יום מספר " & currentDayOfOmer.ToString() & ". הקלק כאן להצגת התזכורת."
        Else
            Me.llPreviewToday.Visible = False
        End If
    End Sub

    Private Function FieldsAreValid() As Boolean
        If Not (Me.rbLaOmer.Checked OrElse Me.rbBaOmer.Checked OrElse Me.rbSfardi.Checked) Then
            MessageBox.Show("אנא בחרו נוסח.",
                "תזכורת ספירת העומר",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function
End Class