<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateRemindersEng
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnCreateOutlookReminders = New System.Windows.Forms.Button()
        Me.btnDeleteOutlookReminders = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbHebrew = New System.Windows.Forms.RadioButton()
        Me.rbEnglish = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbSfardi = New System.Windows.Forms.RadioButton()
        Me.rbBaOmer = New System.Windows.Forms.RadioButton()
        Me.rbLaOmer = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnDeleteWindowsReminders = New System.Windows.Forms.Button()
        Me.btnCreateWindowsReminders = New System.Windows.Forms.Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.llPreviewToday = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.rbIsrael = New System.Windows.Forms.RadioButton()
        Me.rbWorld = New System.Windows.Forms.RadioButton()
        Me.cbLocations = New System.Windows.Forms.ComboBox()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.cbAddAlldayEvent = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCreateOutlookReminders
        '
        Me.btnCreateOutlookReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateOutlookReminders.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateOutlookReminders.ForeColor = System.Drawing.Color.Green
        Me.btnCreateOutlookReminders.Location = New System.Drawing.Point(192, 381)
        Me.btnCreateOutlookReminders.Name = "btnCreateOutlookReminders"
        Me.btnCreateOutlookReminders.Size = New System.Drawing.Size(173, 32)
        Me.btnCreateOutlookReminders.TabIndex = 0
        Me.btnCreateOutlookReminders.Text = "Create Outlook Reminders"
        Me.btnCreateOutlookReminders.UseVisualStyleBackColor = True
        '
        'btnDeleteOutlookReminders
        '
        Me.btnDeleteOutlookReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteOutlookReminders.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteOutlookReminders.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDeleteOutlookReminders.Location = New System.Drawing.Point(192, 414)
        Me.btnDeleteOutlookReminders.Name = "btnDeleteOutlookReminders"
        Me.btnDeleteOutlookReminders.Size = New System.Drawing.Size(173, 32)
        Me.btnDeleteOutlookReminders.TabIndex = 3
        Me.btnDeleteOutlookReminders.Text = "Delete Outlook Reminders"
        Me.btnDeleteOutlookReminders.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(34, 154)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(165, 14)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Reminder, Time Of Day:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(34, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 14)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Location:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.OmerReminder.My.Resources.Resources.OutlookLogo
        Me.PictureBox1.Location = New System.Drawing.Point(127, 382)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(60, 64)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Gray
        Me.Label4.Location = New System.Drawing.Point(34, 83)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(397, 36)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Used to determine if the alarm time is after ""Shkiah""." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If the day of the Omer sh" & _
    "owed in the alarm is to be the correct day of the omer," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "we need to know the Jew" & _
    "ish date at the time of the alarm."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(486, 34)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 14)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "All Day Event:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Gray
        Me.Label7.Location = New System.Drawing.Point(486, 79)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 12)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "For Outlook reminders."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Navy
        Me.Label8.Location = New System.Drawing.Point(486, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(143, 14)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Reminder Language:"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbHebrew)
        Me.Panel1.Controls.Add(Me.rbEnglish)
        Me.Panel1.Location = New System.Drawing.Point(486, 131)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(150, 29)
        Me.Panel1.TabIndex = 20
        '
        'rbHebrew
        '
        Me.rbHebrew.AutoSize = True
        Me.rbHebrew.Location = New System.Drawing.Point(79, 6)
        Me.rbHebrew.Name = "rbHebrew"
        Me.rbHebrew.Size = New System.Drawing.Size(69, 20)
        Me.rbHebrew.TabIndex = 9
        Me.rbHebrew.Text = "Hebrew"
        Me.rbHebrew.UseVisualStyleBackColor = True
        '
        'rbEnglish
        '
        Me.rbEnglish.AutoSize = True
        Me.rbEnglish.Checked = Global.OmerReminder.My.MySettings.Default.English
        Me.rbEnglish.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "English", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.rbEnglish.Location = New System.Drawing.Point(9, 6)
        Me.rbEnglish.Name = "rbEnglish"
        Me.rbEnglish.Size = New System.Drawing.Size(61, 20)
        Me.rbEnglish.TabIndex = 8
        Me.rbEnglish.TabStop = True
        Me.rbEnglish.Text = "English"
        Me.rbEnglish.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbSfardi)
        Me.Panel2.Controls.Add(Me.rbBaOmer)
        Me.Panel2.Controls.Add(Me.rbLaOmer)
        Me.Panel2.Location = New System.Drawing.Point(486, 205)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(228, 73)
        Me.Panel2.TabIndex = 21
        '
        'rbSfardi
        '
        Me.rbSfardi.AutoSize = True
        Me.rbSfardi.Checked = Global.OmerReminder.My.MySettings.Default.Sfardi
        Me.rbSfardi.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "Sfardi", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.rbSfardi.Location = New System.Drawing.Point(9, 50)
        Me.rbSfardi.Name = "rbSfardi"
        Me.rbSfardi.Size = New System.Drawing.Size(89, 20)
        Me.rbSfardi.TabIndex = 10
        Me.rbSfardi.Text = "עדות המזרח"
        Me.rbSfardi.UseVisualStyleBackColor = True
        '
        'rbBaOmer
        '
        Me.rbBaOmer.AutoSize = True
        Me.rbBaOmer.Location = New System.Drawing.Point(9, 28)
        Me.rbBaOmer.Name = "rbBaOmer"
        Me.rbBaOmer.Size = New System.Drawing.Size(102, 20)
        Me.rbBaOmer.TabIndex = 9
        Me.rbBaOmer.Text = "(אשכנז (בעומר"
        Me.rbBaOmer.UseVisualStyleBackColor = True
        '
        'rbLaOmer
        '
        Me.rbLaOmer.AutoSize = True
        Me.rbLaOmer.Checked = Global.OmerReminder.My.MySettings.Default.LaOmer
        Me.rbLaOmer.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "LaOmer", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.rbLaOmer.Location = New System.Drawing.Point(9, 6)
        Me.rbLaOmer.Name = "rbLaOmer"
        Me.rbLaOmer.Size = New System.Drawing.Size(98, 20)
        Me.rbLaOmer.TabIndex = 8
        Me.rbLaOmer.TabStop = True
        Me.rbLaOmer.Text = "(ספרד (לעומר" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.rbLaOmer.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(486, 186)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(122, 14)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Counting Nusach:"
        '
        'btnDeleteWindowsReminders
        '
        Me.btnDeleteWindowsReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteWindowsReminders.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteWindowsReminders.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDeleteWindowsReminders.Location = New System.Drawing.Point(460, 414)
        Me.btnDeleteWindowsReminders.Name = "btnDeleteWindowsReminders"
        Me.btnDeleteWindowsReminders.Size = New System.Drawing.Size(173, 32)
        Me.btnDeleteWindowsReminders.TabIndex = 24
        Me.btnDeleteWindowsReminders.Text = "Delete Windows Reminders"
        Me.btnDeleteWindowsReminders.UseVisualStyleBackColor = True
        '
        'btnCreateWindowsReminders
        '
        Me.btnCreateWindowsReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateWindowsReminders.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateWindowsReminders.ForeColor = System.Drawing.Color.Green
        Me.btnCreateWindowsReminders.Location = New System.Drawing.Point(460, 381)
        Me.btnCreateWindowsReminders.Name = "btnCreateWindowsReminders"
        Me.btnCreateWindowsReminders.Size = New System.Drawing.Size(173, 32)
        Me.btnCreateWindowsReminders.TabIndex = 23
        Me.btnCreateWindowsReminders.Text = "Create Windows Reminders"
        Me.btnCreateWindowsReminders.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = Global.OmerReminder.My.Resources.Resources.WindowsLogo
        Me.PictureBox2.Location = New System.Drawing.Point(396, 381)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(60, 64)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 25
        Me.PictureBox2.TabStop = False
        '
        'llPreviewToday
        '
        Me.llPreviewToday.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llPreviewToday.Location = New System.Drawing.Point(421, 544)
        Me.llPreviewToday.Name = "llPreviewToday"
        Me.llPreviewToday.Size = New System.Drawing.Size(329, 15)
        Me.llPreviewToday.TabIndex = 26
        Me.llPreviewToday.TabStop = True
        Me.llPreviewToday.Text = "Preview todays reminder"
        Me.llPreviewToday.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Gray
        Me.Label1.Location = New System.Drawing.Point(34, 205)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(364, 48)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Time of day reminder ""pops up""." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "NOTE: If this time is after ""Shkiah"" (sunset) fo" & _
    "r your selected location, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the day of the Omer displayed will be  the next day;" & _
    "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "as Shkiah starts the next Jewish day."
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.cbLocations)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.dtpTime)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Panel1)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.cbAddAlldayEvent)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(9, 76)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(738, 285)
        Me.Panel3.TabIndex = 28
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbIsrael)
        Me.Panel4.Controls.Add(Me.rbWorld)
        Me.Panel4.Location = New System.Drawing.Point(95, 20)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(292, 29)
        Me.Panel4.TabIndex = 28
        '
        'rbIsrael
        '
        Me.rbIsrael.AutoSize = True
        Me.rbIsrael.Checked = True
        Me.rbIsrael.Location = New System.Drawing.Point(172, 7)
        Me.rbIsrael.Name = "rbIsrael"
        Me.rbIsrael.Size = New System.Drawing.Size(120, 20)
        Me.rbIsrael.TabIndex = 1
        Me.rbIsrael.TabStop = True
        Me.rbIsrael.Text = "Locations In Israel"
        Me.rbIsrael.UseVisualStyleBackColor = True
        '
        'rbWorld
        '
        Me.rbWorld.AutoSize = True
        Me.rbWorld.Location = New System.Drawing.Point(37, 7)
        Me.rbWorld.Name = "rbWorld"
        Me.rbWorld.Size = New System.Drawing.Size(138, 20)
        Me.rbWorld.TabIndex = 0
        Me.rbWorld.Text = "Worldwide Locations"
        Me.rbWorld.UseVisualStyleBackColor = True
        '
        'cbLocations
        '
        Me.cbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLocations.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbLocations.FormattingEnabled = True
        Me.cbLocations.Location = New System.Drawing.Point(34, 50)
        Me.cbLocations.Name = "cbLocations"
        Me.cbLocations.Size = New System.Drawing.Size(354, 26)
        Me.cbLocations.Sorted = True
        Me.cbLocations.TabIndex = 5
        '
        'dtpTime
        '
        Me.dtpTime.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OmerReminder.My.MySettings.Default, "ReminderTime", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.dtpTime.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(34, 172)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.ShowUpDown = True
        Me.dtpTime.Size = New System.Drawing.Size(208, 26)
        Me.dtpTime.TabIndex = 9
        Me.dtpTime.Value = Global.OmerReminder.My.MySettings.Default.ReminderTime
        '
        'cbAddAlldayEvent
        '
        Me.cbAddAlldayEvent.AutoSize = True
        Me.cbAddAlldayEvent.Checked = Global.OmerReminder.My.MySettings.Default.AddAllDayEvent
        Me.cbAddAlldayEvent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbAddAlldayEvent.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "AddAllDayEvent", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbAddAlldayEvent.Location = New System.Drawing.Point(486, 57)
        Me.cbAddAlldayEvent.Name = "cbAddAlldayEvent"
        Me.cbAddAlldayEvent.Size = New System.Drawing.Size(227, 20)
        Me.cbAddAlldayEvent.TabIndex = 17
        Me.cbAddAlldayEvent.Text = "Also add ""All Day Event"" for each day"
        Me.cbAddAlldayEvent.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Narkisim", 23.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label5.Location = New System.Drawing.Point(339, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(375, 31)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "...וספרתם לכם ממחרת השבת"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Narkisim", 23.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label10.Location = New System.Drawing.Point(5, 485)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(384, 31)
        Me.Label10.TabIndex = 30
        Me.Label10.Text = "שבע שבתות תמימות תהיינה..."
        '
        'frmCreateRemindersEng
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(761, 569)
        Me.Controls.Add(Me.btnDeleteWindowsReminders)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnCreateOutlookReminders)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnCreateWindowsReminders)
        Me.Controls.Add(Me.btnDeleteOutlookReminders)
        Me.Controls.Add(Me.llPreviewToday)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Century Gothic", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmCreateRemindersEng"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Omer Reminders"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCreateOutlookReminders As System.Windows.Forms.Button
    Friend WithEvents btnDeleteOutlookReminders As System.Windows.Forms.Button
    Friend WithEvents cbLocations As System.Windows.Forms.ComboBox
    Friend WithEvents dtpTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbAddAlldayEvent As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbHebrew As System.Windows.Forms.RadioButton
    Friend WithEvents rbEnglish As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbBaOmer As System.Windows.Forms.RadioButton
    Friend WithEvents rbLaOmer As System.Windows.Forms.RadioButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnDeleteWindowsReminders As System.Windows.Forms.Button
    Friend WithEvents btnCreateWindowsReminders As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents llPreviewToday As System.Windows.Forms.LinkLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbIsrael As System.Windows.Forms.RadioButton
    Friend WithEvents rbWorld As System.Windows.Forms.RadioButton
    Friend WithEvents rbSfardi As System.Windows.Forms.RadioButton

End Class
