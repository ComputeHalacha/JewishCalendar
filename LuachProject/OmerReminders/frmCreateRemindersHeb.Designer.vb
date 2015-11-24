<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateRemindersHeb
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbSfardi = New System.Windows.Forms.RadioButton()
        Me.rbBaOmer = New System.Windows.Forms.RadioButton()
        Me.rbLaOmer = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnDeleteWindowsReminders = New System.Windows.Forms.Button()
        Me.btnCreateWindowsReminders = New System.Windows.Forms.Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.llPreviewToday = New System.Windows.Forms.LinkLabel()
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
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCreateOutlookReminders
        '
        Me.btnCreateOutlookReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateOutlookReminders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateOutlookReminders.ForeColor = System.Drawing.Color.Green
        Me.btnCreateOutlookReminders.Location = New System.Drawing.Point(105, 392)
        Me.btnCreateOutlookReminders.Name = "btnCreateOutlookReminders"
        Me.btnCreateOutlookReminders.Size = New System.Drawing.Size(195, 31)
        Me.btnCreateOutlookReminders.TabIndex = 0
        Me.btnCreateOutlookReminders.Text = "יצר תזכורות אוטלוק"
        Me.btnCreateOutlookReminders.UseVisualStyleBackColor = True
        '
        'btnDeleteOutlookReminders
        '
        Me.btnDeleteOutlookReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteOutlookReminders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteOutlookReminders.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDeleteOutlookReminders.Location = New System.Drawing.Point(105, 423)
        Me.btnDeleteOutlookReminders.Name = "btnDeleteOutlookReminders"
        Me.btnDeleteOutlookReminders.Size = New System.Drawing.Size(195, 31)
        Me.btnDeleteOutlookReminders.TabIndex = 3
        Me.btnDeleteOutlookReminders.Text = "הסר תזכורות אוטלוק"
        Me.btnDeleteOutlookReminders.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(416, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 14)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "שעת התזכורת"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(482, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 14)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "עיר"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.OmerReminder.My.Resources.Resources.OutlookLogo
        Me.PictureBox1.Location = New System.Drawing.Point(31, 393)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(66, 61)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Gray
        Me.Label4.Location = New System.Drawing.Point(141, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(369, 11)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "כדי לחשבן באיזה תאריך עברי נופל שעת התזכורת, צריכים לדעת שעת שקיעת החמה."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(331, 164)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(179, 14)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "הוסף ""אירוע יומית"" באוטלוק"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbSfardi)
        Me.Panel2.Controls.Add(Me.rbBaOmer)
        Me.Panel2.Controls.Add(Me.rbLaOmer)
        Me.Panel2.Location = New System.Drawing.Point(60, 247)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Panel2.Size = New System.Drawing.Size(450, 28)
        Me.Panel2.TabIndex = 21
        '
        'rbSfardi
        '
        Me.rbSfardi.AutoSize = True
        Me.rbSfardi.Checked = Global.OmerReminder.My.MySettings.Default.Sfardi
        Me.rbSfardi.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "sfardi", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.rbSfardi.Location = New System.Drawing.Point(132, 6)
        Me.rbSfardi.Name = "rbSfardi"
        Me.rbSfardi.Size = New System.Drawing.Size(94, 18)
        Me.rbSfardi.TabIndex = 13
        Me.rbSfardi.Text = "עדות המזרח"
        Me.rbSfardi.UseVisualStyleBackColor = True
        '
        'rbBaOmer
        '
        Me.rbBaOmer.AutoSize = True
        Me.rbBaOmer.Location = New System.Drawing.Point(337, 6)
        Me.rbBaOmer.Name = "rbBaOmer"
        Me.rbBaOmer.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.rbBaOmer.Size = New System.Drawing.Size(109, 18)
        Me.rbBaOmer.TabIndex = 12
        Me.rbBaOmer.Text = "אשכנז (בעומר)"
        Me.rbBaOmer.UseVisualStyleBackColor = True
        '
        'rbLaOmer
        '
        Me.rbLaOmer.AutoSize = True
        Me.rbLaOmer.Checked = Global.OmerReminder.My.MySettings.Default.LaOmer
        Me.rbLaOmer.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "LaOmer", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.rbLaOmer.Location = New System.Drawing.Point(230, 6)
        Me.rbLaOmer.Name = "rbLaOmer"
        Me.rbLaOmer.Size = New System.Drawing.Size(104, 18)
        Me.rbLaOmer.TabIndex = 11
        Me.rbLaOmer.TabStop = True
        Me.rbLaOmer.Text = "ספרד (לעומר)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.rbLaOmer.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(422, 225)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 14)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "נוסח הספירה"
        '
        'btnDeleteWindowsReminders
        '
        Me.btnDeleteWindowsReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteWindowsReminders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeleteWindowsReminders.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDeleteWindowsReminders.Location = New System.Drawing.Point(407, 423)
        Me.btnDeleteWindowsReminders.Name = "btnDeleteWindowsReminders"
        Me.btnDeleteWindowsReminders.Size = New System.Drawing.Size(195, 31)
        Me.btnDeleteWindowsReminders.TabIndex = 24
        Me.btnDeleteWindowsReminders.Text = "הסר תזכורות בחלונות"
        Me.btnDeleteWindowsReminders.UseVisualStyleBackColor = True
        '
        'btnCreateWindowsReminders
        '
        Me.btnCreateWindowsReminders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateWindowsReminders.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateWindowsReminders.ForeColor = System.Drawing.Color.Green
        Me.btnCreateWindowsReminders.Location = New System.Drawing.Point(407, 392)
        Me.btnCreateWindowsReminders.Name = "btnCreateWindowsReminders"
        Me.btnCreateWindowsReminders.Size = New System.Drawing.Size(195, 31)
        Me.btnCreateWindowsReminders.TabIndex = 23
        Me.btnCreateWindowsReminders.Text = "יצר תזכורות בחלונות"
        Me.btnCreateWindowsReminders.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = Global.OmerReminder.My.Resources.Resources.WindowsLogo
        Me.PictureBox2.Location = New System.Drawing.Point(334, 392)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(66, 61)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 25
        Me.PictureBox2.TabStop = False
        '
        'llPreviewToday
        '
        Me.llPreviewToday.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llPreviewToday.Location = New System.Drawing.Point(251, 530)
        Me.llPreviewToday.Name = "llPreviewToday"
        Me.llPreviewToday.Size = New System.Drawing.Size(370, 14)
        Me.llPreviewToday.TabIndex = 26
        Me.llPreviewToday.TabStop = True
        Me.llPreviewToday.Text = "הצג התזכורת של היום"
        Me.llPreviewToday.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.cbLocations)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.dtpTime)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.cbAddAlldayEvent)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(31, 85)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(570, 294)
        Me.Panel3.TabIndex = 28
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbIsrael)
        Me.Panel4.Controls.Add(Me.rbWorld)
        Me.Panel4.Location = New System.Drawing.Point(113, 15)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(329, 28)
        Me.Panel4.TabIndex = 28
        '
        'rbIsrael
        '
        Me.rbIsrael.AutoSize = True
        Me.rbIsrael.Checked = True
        Me.rbIsrael.Location = New System.Drawing.Point(194, 6)
        Me.rbIsrael.Name = "rbIsrael"
        Me.rbIsrael.Size = New System.Drawing.Size(99, 18)
        Me.rbIsrael.TabIndex = 1
        Me.rbIsrael.TabStop = True
        Me.rbIsrael.Text = "ערים בישראל"
        Me.rbIsrael.UseVisualStyleBackColor = True
        '
        'rbWorld
        '
        Me.rbWorld.AutoSize = True
        Me.rbWorld.Location = New System.Drawing.Point(101, 6)
        Me.rbWorld.Name = "rbWorld"
        Me.rbWorld.Size = New System.Drawing.Size(87, 18)
        Me.rbWorld.TabIndex = 0
        Me.rbWorld.Text = "ערים בחו""ל"
        Me.rbWorld.UseVisualStyleBackColor = True
        '
        'cbLocations
        '
        Me.cbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLocations.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbLocations.FormattingEnabled = True
        Me.cbLocations.Location = New System.Drawing.Point(112, 44)
        Me.cbLocations.Name = "cbLocations"
        Me.cbLocations.Size = New System.Drawing.Size(398, 26)
        Me.cbLocations.TabIndex = 5
        '
        'dtpTime
        '
        Me.dtpTime.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OmerReminder.My.MySettings.Default, "ReminderTime", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.dtpTime.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(277, 117)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.RightToLeftLayout = True
        Me.dtpTime.ShowUpDown = True
        Me.dtpTime.Size = New System.Drawing.Size(233, 26)
        Me.dtpTime.TabIndex = 9
        Me.dtpTime.Value = Global.OmerReminder.My.MySettings.Default.ReminderTime
        '
        'cbAddAlldayEvent
        '
        Me.cbAddAlldayEvent.AutoSize = True
        Me.cbAddAlldayEvent.Checked = Global.OmerReminder.My.MySettings.Default.AddAllDayEvent
        Me.cbAddAlldayEvent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbAddAlldayEvent.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OmerReminder.My.MySettings.Default, "AddAllDayEvent", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbAddAlldayEvent.Location = New System.Drawing.Point(57, 186)
        Me.cbAddAlldayEvent.Name = "cbAddAlldayEvent"
        Me.cbAddAlldayEvent.Size = New System.Drawing.Size(453, 18)
        Me.cbAddAlldayEvent.TabIndex = 17
        Me.cbAddAlldayEvent.Text = "בנוסף לתזכורת הרגיל, תגדיר יום אירוע באוטלוק לכל אחד מימי ספירת העומר."
        Me.cbAddAlldayEvent.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Narkisim", 22.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label5.Location = New System.Drawing.Point(137, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(359, 30)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "...וספרתם לכם ממחרת השבת"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Narkisim", 22.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label10.Location = New System.Drawing.Point(132, 477)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(368, 30)
        Me.Label10.TabIndex = 30
        Me.Label10.Text = "שבע שבתות תמימות תהיינה..."
        '
        'frmCreateRemindersHeb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(633, 548)
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
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "frmCreateRemindersHeb"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "תזכורת ספירת העומר"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents cbAddAlldayEvent As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnDeleteWindowsReminders As System.Windows.Forms.Button
    Friend WithEvents btnCreateWindowsReminders As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents llPreviewToday As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbIsrael As System.Windows.Forms.RadioButton
    Friend WithEvents rbWorld As System.Windows.Forms.RadioButton
    Friend WithEvents rbSfardi As System.Windows.Forms.RadioButton
    Friend WithEvents rbBaOmer As System.Windows.Forms.RadioButton
    Friend WithEvents rbLaOmer As System.Windows.Forms.RadioButton

End Class
