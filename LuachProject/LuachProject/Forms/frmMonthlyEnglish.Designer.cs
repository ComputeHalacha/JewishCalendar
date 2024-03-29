﻿namespace LuachProject
{
    partial class frmMonthlyEnglish
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonthlyEnglish));
            this.lblMonthName = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.llSecularCalendar = new System.Windows.Forms.LinkLabel();
            this.llSefira = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.llShowDaily = new System.Windows.Forms.LinkLabel();
            this.splitContainer1 = new LuachProject.CuelessSplitContainer();
            this.splitContainer2 = new LuachProject.CuelessSplitContainer();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.jewishDatePicker1 = new JewishDatePicker.JewishDatePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbInIsrael = new System.Windows.Forms.RadioButton();
            this.rbInChul = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.llOpenOccasionList = new System.Windows.Forms.LinkLabel();
            this.llShowSeconds = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.llEmailReminders = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMonthName
            // 
            this.lblMonthName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonthName.Font = new System.Drawing.Font("Century Gothic", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMonthName.Location = new System.Drawing.Point(0, 0);
            this.lblMonthName.Name = "lblMonthName";
            this.lblMonthName.Size = new System.Drawing.Size(1408, 45);
            this.lblMonthName.TabIndex = 1;
            this.lblMonthName.Text = "label1";
            this.lblMonthName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(41, 11);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 12);
            this.linkLabel1.TabIndex = 19;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "עברית";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // llSecularCalendar
            // 
            this.llSecularCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSecularCalendar.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llSecularCalendar.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llSecularCalendar.Location = new System.Drawing.Point(1270, 5);
            this.llSecularCalendar.Name = "llSecularCalendar";
            this.llSecularCalendar.Size = new System.Drawing.Size(134, 19);
            this.llSecularCalendar.TabIndex = 1;
            this.llSecularCalendar.TabStop = true;
            this.llSecularCalendar.Text = "Secular Calendar";
            this.llSecularCalendar.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llSecularCalendar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSecularCalendar_LinkClicked);
            // 
            // llSefira
            // 
            this.llSefira.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSefira.AutoSize = true;
            this.llSefira.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llSefira.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llSefira.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.llSefira.Location = new System.Drawing.Point(807, 6);
            this.llSefira.Name = "llSefira";
            this.llSefira.Size = new System.Drawing.Size(90, 14);
            this.llSefira.TabIndex = 20;
            this.llSefira.TabStop = true;
            this.llSefira.Text = "Sefira Reminders";
            this.llSefira.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llSefira.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSefira_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // llShowDaily
            // 
            this.llShowDaily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowDaily.AutoSize = true;
            this.llShowDaily.Font = new System.Drawing.Font("Century Gothic", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llShowDaily.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llShowDaily.LinkColor = System.Drawing.Color.SlateGray;
            this.llShowDaily.Location = new System.Drawing.Point(1329, 30);
            this.llShowDaily.Name = "llShowDaily";
            this.llShowDaily.Size = new System.Drawing.Size(77, 11);
            this.llShowDaily.TabIndex = 21;
            this.llShowDaily.TabStop = true;
            this.llShowDaily.Text = "Show Zmanim ˃";
            this.llShowDaily.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.toolTip1.SetToolTip(this.llShowDaily, "Show Zmanim Panel");
            this.llShowDaily.Visible = false;
            this.llShowDaily.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowDaily_LinkClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 45);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.ClientSizeChanged += new System.EventHandler(this.splitContainer1_Panel1_ClientSizeChanged);
            this.splitContainer1.Panel1MinSize = 500;
            this.splitContainer1.Size = new System.Drawing.Size(1408, 690);
            this.splitContainer1.SplitterDistance = 1081;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pnlMain);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel3);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2MinSize = 155;
            this.splitContainer2.Size = new System.Drawing.Size(1081, 690);
            this.splitContainer2.SplitterDistance = 502;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 19;
            this.splitContainer2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.splitContainer2_KeyDown);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(9, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1072, 502);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseClick);
            this.pnlMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDoubleClick);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbLocation);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 67);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1081, 120);
            this.panel3.TabIndex = 19;
            // 
            // cmbLocation
            // 
            this.cmbLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(3, 85);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(351, 21);
            this.cmbLocation.Sorted = true;
            this.cmbLocation.TabIndex = 5;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.jewishDatePicker1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(374, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(469, 32);
            this.panel2.TabIndex = 18;
            // 
            // jewishDatePicker1
            // 
            this.jewishDatePicker1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.jewishDatePicker1.AutoSize = true;
            this.jewishDatePicker1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jewishDatePicker1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.jewishDatePicker1.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jewishDatePicker1.ForeColor = System.Drawing.Color.White;
            this.jewishDatePicker1.Language = JewishDatePicker.JewishDatePicker.Languages.English;
            this.jewishDatePicker1.Location = new System.Drawing.Point(142, 2);
            this.jewishDatePicker1.Margin = new System.Windows.Forms.Padding(0);
            this.jewishDatePicker1.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MaxDate")));
            this.jewishDatePicker1.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MinDate")));
            this.jewishDatePicker1.Name = "jewishDatePicker1";
            this.jewishDatePicker1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.jewishDatePicker1.Size = new System.Drawing.Size(323, 27);
            this.jewishDatePicker1.TabIndex = 17;
            this.jewishDatePicker1.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.Value")));
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.Color.GhostWhite;
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(96, 30);
            this.label1.TabIndex = 18;
            this.label1.Text = "Navigate:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbInIsrael);
            this.panel1.Controls.Add(this.rbInChul);
            this.panel1.Location = new System.Drawing.Point(6, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 36);
            this.panel1.TabIndex = 11;
            // 
            // rbInIsrael
            // 
            this.rbInIsrael.Checked = global::LuachProject.Properties.Settings.Default.rbInIsraelChecked;
            this.rbInIsrael.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "rbInIsraelChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rbInIsrael.Location = new System.Drawing.Point(3, 4);
            this.rbInIsrael.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInIsrael.Name = "rbInIsrael";
            this.rbInIsrael.Size = new System.Drawing.Size(67, 26);
            this.rbInIsrael.TabIndex = 1;
            this.rbInIsrael.TabStop = true;
            this.rbInIsrael.Text = "Israel";
            this.rbInIsrael.UseVisualStyleBackColor = true;
            this.rbInIsrael.CheckedChanged += new System.EventHandler(this.rbInIsrael_CheckedChanged);
            // 
            // rbInChul
            // 
            this.rbInChul.Location = new System.Drawing.Point(84, 5);
            this.rbInChul.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInChul.Name = "rbInChul";
            this.rbInChul.Size = new System.Drawing.Size(102, 25);
            this.rbInChul.TabIndex = 0;
            this.rbInChul.Text = "Elsewhere";
            this.rbInChul.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lavender;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button1.Location = new System.Drawing.Point(661, 38);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(182, 33);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next Month →";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.ForeColor = System.Drawing.Color.GhostWhite;
            this.label6.Location = new System.Drawing.Point(3, 1);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(351, 30);
            this.label6.TabIndex = 10;
            this.label6.Text = "Location:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Lavender;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button4.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button4.Location = new System.Drawing.Point(374, 80);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button4.Size = new System.Drawing.Size(177, 33);
            this.button4.TabIndex = 2;
            this.button4.Text = "← Previous Year";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Lavender;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button2.Location = new System.Drawing.Point(374, 38);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button2.Size = new System.Drawing.Size(177, 33);
            this.button2.TabIndex = 1;
            this.button2.Text = "← Previous Month";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button3.Location = new System.Drawing.Point(556, 38);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button3.Size = new System.Drawing.Size(101, 75);
            this.button3.TabIndex = 0;
            this.button3.Text = "Today";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Lavender;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button5.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button5.Location = new System.Drawing.Point(661, 80);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button5.Size = new System.Drawing.Size(182, 33);
            this.button5.TabIndex = 4;
            this.button5.Text = "Next Year →";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1081, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Click any day to see Zmanim, Occasions and Events   |   Double-click to add an Ev" +
    "ent   |   Use the arrow keys to navigate through the days.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // llOpenOccasionList
            // 
            this.llOpenOccasionList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llOpenOccasionList.AutoSize = true;
            this.llOpenOccasionList.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llOpenOccasionList.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llOpenOccasionList.LinkColor = System.Drawing.Color.Green;
            this.llOpenOccasionList.Location = new System.Drawing.Point(1163, 6);
            this.llOpenOccasionList.Name = "llOpenOccasionList";
            this.llOpenOccasionList.Size = new System.Drawing.Size(92, 14);
            this.llOpenOccasionList.TabIndex = 22;
            this.llOpenOccasionList.TabStop = true;
            this.llOpenOccasionList.Text = "List of Occasions";
            this.llOpenOccasionList.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llOpenOccasionList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llOpenOccasionList_LinkClicked);
            // 
            // llShowSeconds
            // 
            this.llShowSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowSeconds.AutoSize = true;
            this.llShowSeconds.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llShowSeconds.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llShowSeconds.LinkColor = System.Drawing.Color.Maroon;
            this.llShowSeconds.Location = new System.Drawing.Point(1048, 6);
            this.llShowSeconds.Name = "llShowSeconds";
            this.llShowSeconds.Size = new System.Drawing.Size(82, 14);
            this.llShowSeconds.TabIndex = 23;
            this.llShowSeconds.TabStop = true;
            this.llShowSeconds.Text = "Show Seconds";
            this.llShowSeconds.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llShowSeconds.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowSeconds_LinkClicked);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(909, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(9, 14);
            this.label3.TabIndex = 24;
            this.label3.Text = "|";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1027, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 14);
            this.label4.TabIndex = 25;
            this.label4.Text = "|";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1267, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(9, 14);
            this.label5.TabIndex = 26;
            this.label5.Text = "|";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.AllowCurrentPage = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::LuachProject.Properties.Resources.print;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 25);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1142, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 14);
            this.label7.TabIndex = 29;
            this.label7.Text = "|";
            // 
            // llEmailReminders
            // 
            this.llEmailReminders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llEmailReminders.AutoSize = true;
            this.llEmailReminders.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llEmailReminders.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llEmailReminders.LinkColor = System.Drawing.Color.Goldenrod;
            this.llEmailReminders.Location = new System.Drawing.Point(930, 6);
            this.llEmailReminders.Name = "llEmailReminders";
            this.llEmailReminders.Size = new System.Drawing.Size(85, 14);
            this.llEmailReminders.TabIndex = 28;
            this.llEmailReminders.TabStop = true;
            this.llEmailReminders.Text = "Email Reminders";
            this.llEmailReminders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llEmailReminders.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEmailReminders_LinkClicked);
            // 
            // frmMonthlyEnglish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1408, 735);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.llEmailReminders);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.llShowSeconds);
            this.Controls.Add(this.llOpenOccasionList);
            this.Controls.Add(this.llShowDaily);
            this.Controls.Add(this.llSefira);
            this.Controls.Add(this.llSecularCalendar);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblMonthName);
            this.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMonthlyEnglish";
            this.Text = "Luach";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMonthlyEnglish_FormClosed);
            this.Load += new System.EventHandler(this.frmMonthlyEnglish_Load);
            this.Resize += new System.EventHandler(this.frmMonthlyEnglish_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblMonthName;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbInIsrael;
        private System.Windows.Forms.RadioButton rbInChul;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private CuelessSplitContainer splitContainer1;
        private CuelessSplitContainer splitContainer2;
        private System.Windows.Forms.LinkLabel llSecularCalendar;
        private System.Windows.Forms.LinkLabel llSefira;
        private JewishDatePicker.JewishDatePicker jewishDatePicker1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel llShowDaily;
        private System.Windows.Forms.LinkLabel llOpenOccasionList;
        private System.Windows.Forms.LinkLabel llShowSeconds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel llEmailReminders;
    }
}