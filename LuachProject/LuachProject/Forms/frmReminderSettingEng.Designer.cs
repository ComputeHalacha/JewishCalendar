namespace LuachProject
{
    partial class frmReminderSettingsEng
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveAndExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSendToEmail = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.toggleSendOnDay = new LuachProject.Classes.Toggle();
            this.toggleSendDayBefore = new LuachProject.Classes.Toggle();
            this.toggle1 = new LuachProject.Classes.Toggle();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(539, 493);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 33);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveAndExit
            // 
            this.btnSaveAndExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveAndExit.Location = new System.Drawing.Point(775, 493);
            this.btnSaveAndExit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSaveAndExit.Name = "btnSaveAndExit";
            this.btnSaveAndExit.Size = new System.Drawing.Size(110, 33);
            this.btnSaveAndExit.TabIndex = 11;
            this.btnSaveAndExit.Text = "Save && Exit";
            this.btnSaveAndExit.UseVisualStyleBackColor = false;
            this.btnSaveAndExit.Click += new System.EventHandler(this.btnSaveAndExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(22, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Email server";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(143, 89);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(268, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "Send reminder on day before Occasion or Event";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(147, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Send reminder on day of Occasion or Event";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(291, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(11, 91);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "Email \"From\" Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(217, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 27;
            this.label5.Text = "Email \"From\" Address";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(183, 142);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(22, 145);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 15);
            this.label7.TabIndex = 29;
            this.label7.Text = "User Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(184, 27);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 15);
            this.label9.TabIndex = 33;
            this.label9.Text = "Send Encrypted";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(58, 138);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(201, 15);
            this.label10.TabIndex = 7;
            this.label10.Text = "Time of day to send reminder email";
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.White;
            this.btnSend.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSend.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnSend.Location = new System.Drawing.Point(521, 296);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(285, 57);
            this.btnSend.TabIndex = 35;
            this.btnSend.Text = "Send a Reminder Email for today now";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(10, 27);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(242, 15);
            this.label11.TabIndex = 37;
            this.label11.Text = "Send reminder email to the email address:";
            // 
            // txtSendToEmail
            // 
            this.txtSendToEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "SendToEmailAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtSendToEmail.Font = new System.Drawing.Font("Century Gothic", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtSendToEmail.Location = new System.Drawing.Point(10, 49);
            this.txtSendToEmail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSendToEmail.Name = "txtSendToEmail";
            this.txtSendToEmail.Size = new System.Drawing.Size(406, 25);
            this.txtSendToEmail.TabIndex = 36;
            this.txtSendToEmail.Text = global::LuachProject.Properties.Settings.Default.SendToEmailAddress;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::LuachProject.Properties.Settings.Default, "ReminderTimeOfDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker1.Location = new System.Drawing.Point(58, 166);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(165, 22);
            this.dateTimePicker1.TabIndex = 34;
            this.dateTimePicker1.Value = global::LuachProject.Properties.Settings.Default.ReminderTimeOfDay;
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "EmailPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(187, 163);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(182, 22);
            this.textBox3.TabIndex = 30;
            this.textBox3.Text = global::LuachProject.Properties.Settings.Default.EmailPassword;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "EmailUserName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(22, 166);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(145, 22);
            this.textBox4.TabIndex = 28;
            this.textBox4.Text = global::LuachProject.Properties.Settings.Default.EmailUserName;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "EmailFromAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(221, 112);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(198, 22);
            this.textBox2.TabIndex = 26;
            this.textBox2.Text = global::LuachProject.Properties.Settings.Default.EmailFromAddress;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "EmailFromName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(11, 112);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 22);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = global::LuachProject.Properties.Settings.Default.EmailFromName;
            // 
            // txtPort
            // 
            this.txtPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "SmtpPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtPort.Location = new System.Drawing.Point(294, 48);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(75, 22);
            this.txtPort.TabIndex = 22;
            this.txtPort.Text = global::LuachProject.Properties.Settings.Default.SmtpPort;
            // 
            // txtServer
            // 
            this.txtServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LuachProject.Properties.Settings.Default, "EmailServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtServer.Location = new System.Drawing.Point(22, 48);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(145, 22);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = global::LuachProject.Properties.Settings.Default.EmailServer;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label12.Location = new System.Drawing.Point(258, 27);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 15);
            this.label12.TabIndex = 38;
            this.label12.Text = "* required";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label13.Location = new System.Drawing.Point(106, 27);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 15);
            this.label13.TabIndex = 39;
            this.label13.Text = "* required";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label14.Location = new System.Drawing.Point(103, 145);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 15);
            this.label14.TabIndex = 40;
            this.label14.Text = "* required";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label15.Location = new System.Drawing.Point(254, 145);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 15);
            this.label15.TabIndex = 41;
            this.label15.Text = "* required";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label16.Location = new System.Drawing.Point(355, 91);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 15);
            this.label16.TabIndex = 42;
            this.label16.Text = "* required";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.toggleSendOnDay);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.toggleSendDayBefore);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 223);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reminder settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtServer);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPort);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.toggle1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(457, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 223);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SMTP server details";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSendToEmail);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(14, 247);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 156);
            this.groupBox3.TabIndex = 44;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Email details";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(17, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(868, 419);
            this.panel1.TabIndex = 45;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(657, 493);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 33);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toggleSendOnDay
            // 
            this.toggleSendOnDay.AutoSize = true;
            this.toggleSendOnDay.Checked = global::LuachProject.Properties.Settings.Default.SendReminderOnEventDay;
            this.toggleSendOnDay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleSendOnDay.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "SendReminderOnEventDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.toggleSendOnDay.Location = new System.Drawing.Point(58, 43);
            this.toggleSendOnDay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toggleSendOnDay.Name = "toggleSendOnDay";
            this.toggleSendOnDay.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.toggleSendOnDay.Size = new System.Drawing.Size(81, 31);
            this.toggleSendOnDay.TabIndex = 20;
            this.toggleSendOnDay.Text = "toggle1";
            this.toggleSendOnDay.UseVisualStyleBackColor = true;
            // 
            // toggleSendDayBefore
            // 
            this.toggleSendDayBefore.AutoSize = true;
            this.toggleSendDayBefore.Checked = global::LuachProject.Properties.Settings.Default.SendReminderOnDayBeforeEventDay;
            this.toggleSendDayBefore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleSendDayBefore.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "SendReminderOnDayBeforeEventDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.toggleSendDayBefore.Location = new System.Drawing.Point(58, 82);
            this.toggleSendDayBefore.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toggleSendDayBefore.Name = "toggleSendDayBefore";
            this.toggleSendDayBefore.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.toggleSendDayBefore.Size = new System.Drawing.Size(81, 31);
            this.toggleSendDayBefore.TabIndex = 18;
            this.toggleSendDayBefore.Text = "toggle1";
            this.toggleSendDayBefore.UseVisualStyleBackColor = true;
            // 
            // toggle1
            // 
            this.toggle1.AutoSize = true;
            this.toggle1.Checked = global::LuachProject.Properties.Settings.Default.SendEmailEncrypted;
            this.toggle1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggle1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "SendEmailEncrypted", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.toggle1.Location = new System.Drawing.Point(188, 44);
            this.toggle1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.toggle1.Name = "toggle1";
            this.toggle1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.toggle1.Size = new System.Drawing.Size(81, 31);
            this.toggle1.TabIndex = 32;
            this.toggle1.Text = "toggle1";
            this.toggle1.UseVisualStyleBackColor = true;
            // 
            // frmSettingsEng
            // 
            this.AcceptButton = this.btnSaveAndExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(898, 538);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSaveAndExit);
            this.Controls.Add(this.btnCancel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettingsEng";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email Reminder Settings";
            this.TransparencyKey = System.Drawing.Color.GhostWhite;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddOccasionEng_FormClosing);
            this.Load += new System.EventHandler(this.frmSettingsEng_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveAndExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private Classes.Toggle toggleSendDayBefore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private Classes.Toggle toggleSendOnDay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private Classes.Toggle toggle1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSendToEmail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
    }
}