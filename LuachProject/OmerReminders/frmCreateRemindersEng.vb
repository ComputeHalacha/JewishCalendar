﻿Imports System.Linq
Imports JewishCalendar
Imports Outlook = Microsoft.Office.Interop.Outlook

Public Class frmCreateRemindersEng
    Private _loaded As Boolean
    Private _todayJD As JewishDate = Nothing

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
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

        My.Settings.English = True

        Me._todayJD = New JewishDate(loc)
        Me.rbIsrael.Checked = loc.IsInIsrael
        Me.rbHebrew.Checked = Not Me.rbEnglish.Checked
        Me.rbSfardi.Checked = My.Settings.Nusach = Nusach.Sefardi
        Me.rbLaOmer.Checked = My.Settings.Nusach = Nusach.Sefard
        Me.rbBaOmer.Checked = My.Settings.Nusach = Nusach.Ashkenaz
        Me.ShowLinkToReminder()
        Me.btnCreateOutlookReminders.Enabled = Me.btnDeleteOutlookReminders.Enabled = Program.IsOutlookInstalled()

        Me._loaded = True
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.rbSfardi.Checked Then
            My.Settings.Nusach = Nusach.Sefardi
        ElseIf Me.rbLaOmer.Checked Then
            My.Settings.Nusach = Nusach.Sefard
        Else
            My.Settings.Nusach = Nusach.Ashkenaz
        End If
        My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
        My.Settings.Save()
    End Sub

    Private Sub btnCreateOutlookReminders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateOutlookReminders.Click
        If Not Me.FieldsAreValid() Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        If Not Program.IsOutlookInstalled() Then
            MessageBox.Show("This option is only available if Microsoft Outlook is installed on this computer.",
                            "Create Outlook Reminders",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Return
        End If

        Dim olApp As Outlook.Application

        Try
            olApp = New Outlook.Application()

            Dim oNameSpace As Outlook.NameSpace = olApp.GetNamespace("MAPI")
            Dim objApt As Outlook.AppointmentItem
            Dim firstDayOfPesach As JewishDate = GetFirstDayOfPesach(Me._todayJD)
            Dim count As Integer

            oNameSpace.Logon()

            For i As Integer = 1 To 49
                Dim jd As JewishDate = firstDayOfPesach + i

                If jd >= Me._todayJD Then
                    Dim dayOfOmer As Integer = jd.GetDayOfOmer()
                    Dim nusach As Nusach = If(Me.rbLaOmer.Checked, Nusach.Sefard,
                        If(Me.rbSfardi.Checked, Nusach.Sefardi, Nusach.Ashkenaz))
                    Dim txt As String = Utils.GetOmerNusach(dayOfOmer, nusach)
                    Dim yesterday As JewishDate = jd - 1
                    Dim alarmTime As TimeSpan = Me.dtpTime.Value.TimeOfDay
                    Dim subs As String

                    If rbEnglish.Checked Then
                        subs = "Sefiras Ha'omer - Day " & dayOfOmer
                    Else
                        subs = "ספירת העומר - יום " & dayOfOmer
                    End If

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
                    objApt.Subject = If(rbEnglish.Checked, "Count ", "לספור ") & subs &
                        " - " & txt
                    objApt.Body = txt
                    objApt.Save()

                    objApt = Nothing
                End If
            Next i
            Me.Cursor = Cursors.Default
            MessageBox.Show("Successfully created " & count.ToString() & " Sefiras Ha'omer reminders.")
        Catch ex As System.Exception
            MessageBox.Show("Failed to  created reminders." & vbCrLf & "Technical error encountered: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            olApp = Nothing
        End Try
    End Sub

    Private Sub btnDeleteOutlookReminders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteOutlookReminders.Click
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
            MessageBox.Show("Successfully deleted all " & count.ToString() & " Sefiras Ha'omer events and reminders.")
        Catch ex As System.Exception
            MessageBox.Show("Failed to  delete reminders." & vbCrLf & "Technical error encountered: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCreateWindowsReminders_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateWindowsReminders.Click
        If Not Me.FieldsAreValid() Then Exit Sub

        Me.Cursor = Cursors.WaitCursor
        Try
            Program.CreateDailyReminders(Me._todayJD, Me.dtpTime.Value.TimeOfDay)
            MessageBox.Show("Reminders were successfully created.",
                "Create Windows Reminder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        Catch nse As Microsoft.Win32.TaskScheduler.TSNotSupportedException
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
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnDeleteWindowsReminders_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteWindowsReminders.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Program.DeleteDailyReminders()
            MessageBox.Show("Reminders were successfully deleted.",
                                "Delete Windows Reminder",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("There was a problem during the removal of the Window Reminders:" & Environment.NewLine & ex.Message,
                            "Delete Windows Reminder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rbLocs_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbWorld.CheckedChanged
        If Me._loaded Then
            Me.FillLocations()
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            Me.ShowLinkToReminder()
        End If
    End Sub

    Private Sub llPreviewToday_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llPreviewToday.LinkClicked
        If Me._loaded Then
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            My.Settings.Save()
            Dim f As New frmRemindEng()
            f.ShowDialog()
            f.Dispose()
        End If
    End Sub

    Private Sub cbLocations_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbLocations.SelectedIndexChanged
        If Me._loaded Then
            My.Settings.LocationName = DirectCast(Me.cbLocations.SelectedItem, Location).Name
            Me.ShowLinkToReminder()
        End If
    End Sub

    Private Sub rbLaOmer_Click(sender As System.Object, e As System.EventArgs) Handles rbLaOmer.Click
        Me.rbLaOmer.Checked = True
    End Sub

    Private Sub rbBaOmer_Click(sender As System.Object, e As System.EventArgs) Handles rbBaOmer.Click
        Me.rbBaOmer.Checked = True
    End Sub

    Private Sub rbSfardi_Click(sender As System.Object, e As System.EventArgs) Handles rbSfardi.Click
        Me.rbSfardi.Checked = True
    End Sub

    Private Function FieldsAreValid() As Boolean
        If Not (Me.rbLaOmer.Checked OrElse Me.rbBaOmer.Checked OrElse Me.rbSfardi.Checked) Then
            MessageBox.Show("Please choose a Nusach.",
                "Create Omer Reminders",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub FillLocations()
        Dim loaded As Boolean = Me._loaded 'will help us get back to loading situation when we started
        Dim isWorld As Boolean = Me.rbWorld.Checked

        Me._loaded = False
        Me.cbLocations.Items.Clear()
        Dim locList As IEnumerable(Of Location) = Program.LocationsList.Where(Function(l) l.IsInIsrael <> isWorld)
        For Each loc As Location In locList
            Me.cbLocations.Items.Add(loc)
        Next

        Me.cbLocations.SelectedIndex = 0

        'I don't know why the above line messes up the check property of the radio buttons - but it does,
        'so we need to reset them.
        Me.rbWorld.Checked = isWorld
        Me.rbIsrael.Checked = Not isWorld
        Me._loaded = loaded
    End Sub

    Private Function GetAlarmDateTime(yesterday As Date, today As Date, alarmTime As TimeSpan) As DateTime
        Dim shkiaTimeYesterday As TimeSpan = New Zmanim(yesterday, Me.cbLocations.SelectedItem).GetShkia()
        Return If(alarmTime >= shkiaTimeYesterday AndAlso alarmTime <= New TimeSpan(23, 59, 59), yesterday, today) + alarmTime
    End Function

    Private Sub ShowLinkToReminder()
        Dim currentDayOfOmer As Integer = Me._todayJD.GetDayOfOmer()
        If currentDayOfOmer > 0 Then
            Me.llPreviewToday.Visible = True
            Me.llPreviewToday.Text = "Today is day " & currentDayOfOmer.ToString() & ". Click to preview reminder."
        Else
            Me.llPreviewToday.Visible = False
        End If
    End Sub
End Class