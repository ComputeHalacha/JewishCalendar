namespace LuachProject
{
    partial class frmMonthlyHebrew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonthlyHebrew));
            this.lblMonthName = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.llSecularCalendar = new System.Windows.Forms.LinkLabel();
            this.llSefirah = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.llShowDaily = new System.Windows.Forms.LinkLabel();
            this.llSearchOccasion = new System.Windows.Forms.LinkLabel();
            this.llShowSeconds = new System.Windows.Forms.LinkLabel();
            this.splitContainer1 = new LuachProject.CuelessSplitContainer();
            this.splitContainer2 = new LuachProject.CuelessSplitContainer();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.jewishDatePicker1 = new JewishDatePicker.JewishDatePicker();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbInIsrael = new System.Windows.Forms.RadioButton();
            this.rbInChul = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToUpcomingOccurenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMonthName
            // 
            this.lblMonthName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonthName.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMonthName.Location = new System.Drawing.Point(0, 0);
            this.lblMonthName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblMonthName.Name = "lblMonthName";
            this.lblMonthName.Size = new System.Drawing.Size(1183, 35);
            this.lblMonthName.TabIndex = 0;
            this.lblMonthName.Text = "label1";
            this.lblMonthName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(7, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(66, 21);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "English";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // llSecularCalendar
            // 
            this.llSecularCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSecularCalendar.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llSecularCalendar.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llSecularCalendar.Location = new System.Drawing.Point(1112, 2);
            this.llSecularCalendar.Name = "llSecularCalendar";
            this.llSecularCalendar.Size = new System.Drawing.Size(68, 16);
            this.llSecularCalendar.TabIndex = 0;
            this.llSecularCalendar.TabStop = true;
            this.llSecularCalendar.Text = "לוח לועזי";
            this.llSecularCalendar.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llSecularCalendar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSecularCalendar_LinkClicked);
            // 
            // llSefirah
            // 
            this.llSefirah.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSefirah.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llSefirah.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llSefirah.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.llSefirah.Location = new System.Drawing.Point(751, 2);
            this.llSefirah.Name = "llSefirah";
            this.llSefirah.Size = new System.Drawing.Size(141, 17);
            this.llSefirah.TabIndex = 4;
            this.llSefirah.TabStop = true;
            this.llSefirah.Text = "תזכורת ספירת העומר";
            this.llSefirah.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llSefirah.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSefira_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            this.toolTip1.BackColor = System.Drawing.Color.GhostWhite;
            this.toolTip1.ForeColor = System.Drawing.Color.LightSlateGray;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // llShowDaily
            // 
            this.llShowDaily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowDaily.AutoSize = true;
            this.llShowDaily.Font = new System.Drawing.Font("Tahoma", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llShowDaily.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llShowDaily.LinkColor = System.Drawing.Color.LightSlateGray;
            this.llShowDaily.Location = new System.Drawing.Point(1091, 21);
            this.llShowDaily.Name = "llShowDaily";
            this.llShowDaily.Size = new System.Drawing.Size(96, 13);
            this.llShowDaily.TabIndex = 5;
            this.llShowDaily.TabStop = true;
            this.llShowDaily.Text = "הצג לוח זמנים ˃";
            this.toolTip1.SetToolTip(this.llShowDaily, "הצג פרטי היום");
            this.llShowDaily.Visible = false;
            this.llShowDaily.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowDaily_LinkClick);
            // 
            // llSearchOccasion
            // 
            this.llSearchOccasion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSearchOccasion.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llSearchOccasion.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llSearchOccasion.LinkColor = System.Drawing.Color.ForestGreen;
            this.llSearchOccasion.Location = new System.Drawing.Point(992, 2);
            this.llSearchOccasion.Name = "llSearchOccasion";
            this.llSearchOccasion.Size = new System.Drawing.Size(107, 16);
            this.llSearchOccasion.TabIndex = 6;
            this.llSearchOccasion.TabStop = true;
            this.llSearchOccasion.Text = "רשימת אירועים";
            this.llSearchOccasion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llSearchOccasion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSearchOccasion_LinkClicked);
            // 
            // llShowSeconds
            // 
            this.llShowSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowSeconds.BackColor = System.Drawing.Color.Transparent;
            this.llShowSeconds.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llShowSeconds.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llShowSeconds.LinkColor = System.Drawing.Color.Maroon;
            this.llShowSeconds.Location = new System.Drawing.Point(905, 2);
            this.llShowSeconds.Name = "llShowSeconds";
            this.llShowSeconds.Size = new System.Drawing.Size(74, 16);
            this.llShowSeconds.TabIndex = 1;
            this.llShowSeconds.TabStop = true;
            this.llShowSeconds.Text = "הצג שניות";
            this.llShowSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llShowSeconds.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowSeconds_LinkClicked);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel2.ClientSizeChanged += new System.EventHandler(this.splitContainer1_Panel2_ClientSizeChanged);
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(1183, 701);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 3;
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
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(7, 0, 10, 0);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.cmbLocation);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.label6);
            this.splitContainer2.Panel2.Controls.Add(this.button3);
            this.splitContainer2.Panel2.Controls.Add(this.button5);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer2.Panel2MinSize = 134;
            this.splitContainer2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer2.Size = new System.Drawing.Size(934, 701);
            this.splitContainer2.SplitterDistance = 540;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.splitContainer2_KeyDown);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.ContextMenuStrip = this.contextMenuStrip1;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.pnlMain.Location = new System.Drawing.Point(7, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlMain.Size = new System.Drawing.Size(917, 540);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.TabStop = true;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseClick);
            this.pnlMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDoubleClick);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.jewishDatePicker1);
            this.panel2.Location = new System.Drawing.Point(10, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(467, 20);
            this.panel2.TabIndex = 19;
            // 
            // jewishDatePicker1
            // 
            this.jewishDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jewishDatePicker1.AutoSize = true;
            this.jewishDatePicker1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jewishDatePicker1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.jewishDatePicker1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jewishDatePicker1.ForeColor = System.Drawing.Color.White;
            this.jewishDatePicker1.Location = new System.Drawing.Point(154, -3);
            this.jewishDatePicker1.Margin = new System.Windows.Forms.Padding(0);
            this.jewishDatePicker1.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MaxDate")));
            this.jewishDatePicker1.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MinDate")));
            this.jewishDatePicker1.Name = "jewishDatePicker1";
            this.jewishDatePicker1.Size = new System.Drawing.Size(312, 30);
            this.jewishDatePicker1.TabIndex = 18;
            this.jewishDatePicker1.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.Value")));
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.BackColor = System.Drawing.Color.Lavender;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button4.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button4.Location = new System.Drawing.Point(319, 108);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button4.Size = new System.Drawing.Size(158, 39);
            this.button4.TabIndex = 4;
            this.button4.Text = "שנה הקודמת →";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.btnPreviousYear_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.Lavender;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button2.Location = new System.Drawing.Point(319, 60);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button2.Size = new System.Drawing.Size(158, 39);
            this.button2.TabIndex = 3;
            this.button2.Text = "חודש הקודם →";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmbLocation
            // 
            this.cmbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(573, 121);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbLocation.Size = new System.Drawing.Size(351, 26);
            this.cmbLocation.Sorted = true;
            this.cmbLocation.TabIndex = 5;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.rbInIsrael);
            this.panel1.Controls.Add(this.rbInChul);
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.panel1.Location = new System.Drawing.Point(696, 88);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(228, 29);
            this.panel1.TabIndex = 11;
            // 
            // rbInIsrael
            // 
            this.rbInIsrael.Checked = global::LuachProject.Properties.Settings.Default.rbInIsraelChecked;
            this.rbInIsrael.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "rbInIsraelChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rbInIsrael.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbInIsrael.Location = new System.Drawing.Point(102, 0);
            this.rbInIsrael.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInIsrael.Name = "rbInIsrael";
            this.rbInIsrael.Size = new System.Drawing.Size(126, 29);
            this.rbInIsrael.TabIndex = 0;
            this.rbInIsrael.TabStop = true;
            this.rbInIsrael.Text = "ארץ ישראל";
            this.rbInIsrael.UseVisualStyleBackColor = true;
            this.rbInIsrael.CheckedChanged += new System.EventHandler(this.rbInIsrael_CheckedChanged);
            // 
            // rbInChul
            // 
            this.rbInChul.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbInChul.Location = new System.Drawing.Point(0, 0);
            this.rbInChul.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInChul.Name = "rbInChul";
            this.rbInChul.Size = new System.Drawing.Size(78, 29);
            this.rbInChul.TabIndex = 1;
            this.rbInChul.Text = "חו\"ל";
            this.rbInChul.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(934, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "לחץ פעמיים להוסיף אירוע   |   לנווט בין הימים השתמשו בלחצני החיצים";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.ForeColor = System.Drawing.Color.GhostWhite;
            this.label6.Location = new System.Drawing.Point(573, 58);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(351, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = ":מיקום";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button3.Location = new System.Drawing.Point(172, 60);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button3.Size = new System.Drawing.Size(141, 87);
            this.button3.TabIndex = 3;
            this.button3.Text = "היום";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.BackColor = System.Drawing.Color.Lavender;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button5.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button5.Location = new System.Drawing.Point(10, 108);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button5.Size = new System.Drawing.Size(158, 39);
            this.button5.TabIndex = 2;
            this.button5.Text = "← שנה הבאה";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnNextYear_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.Lavender;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.ForeColor = System.Drawing.Color.LightSlateGray;
            this.button1.Location = new System.Drawing.Point(10, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(158, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "← חודש הבא";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1099, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "|";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(979, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "|";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(892, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "|";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToDateToolStripMenuItem,
            this.goToUpcomingOccurenceToolStripMenuItem,
            this.editThisOccasionToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteThisOccasionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(256, 106);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // goToDateToolStripMenuItem
            // 
            this.goToDateToolStripMenuItem.Name = "goToDateToolStripMenuItem";
            this.goToDateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.goToDateToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.goToDateToolStripMenuItem.Text = "פתח תאריך האירוע";
            this.goToDateToolStripMenuItem.Click += new System.EventHandler(this.goToDateToolStripMenuItem_Click);

            // 
            // goToUpcomingOccurenceToolStripMenuItem
            // 
            this.goToUpcomingOccurenceToolStripMenuItem.Name = "goToUpcomingOccurenceToolStripMenuItem";
            this.goToUpcomingOccurenceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.goToUpcomingOccurenceToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.goToUpcomingOccurenceToolStripMenuItem.Text = "פתח תאריך הבא";
            this.goToUpcomingOccurenceToolStripMenuItem.Click += new System.EventHandler(this.goToUpcomingOccurenceToolStripMenuItem_Click);

            // 
            // editThisOccasionToolStripMenuItem
            // 
            this.editThisOccasionToolStripMenuItem.Name = "editThisOccasionToolStripMenuItem";
            this.editThisOccasionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.editThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.editThisOccasionToolStripMenuItem.Text = "ערוך פרטי האירוע";
            this.editThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.editThisOccasionToolStripMenuItem_Click);

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(252, 6);
            // 
            // deleteThisOccasionToolStripMenuItem
            // 
            this.deleteThisOccasionToolStripMenuItem.Name = "deleteThisOccasionToolStripMenuItem";
            this.deleteThisOccasionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.deleteThisOccasionToolStripMenuItem.Text = "מחק האירוע";
            this.deleteThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.deleteThisOccasionToolStripMenuItem_Click);

            // 
            // frmMonthlyHebrew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1183, 736);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.llShowSeconds);
            this.Controls.Add(this.llSefirah);
            this.Controls.Add(this.llSearchOccasion);
            this.Controls.Add(this.llShowDaily);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.llSecularCalendar);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblMonthName);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMonthlyHebrew";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "לוח";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMonthlyHebrew_FormClosed);
            this.Load += new System.EventHandler(this.frmMonthlyHebrew_Load);
            this.Resize += new System.EventHandler(this.frmMonthlyHebrew_Resize);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private CuelessSplitContainer splitContainer1;
        private CuelessSplitContainer splitContainer2;
        private System.Windows.Forms.LinkLabel llSecularCalendar;
        private System.Windows.Forms.LinkLabel llSefirah;
        private JewishDatePicker.JewishDatePicker jewishDatePicker1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel llShowDaily;
        private System.Windows.Forms.LinkLabel llSearchOccasion;
        private System.Windows.Forms.LinkLabel llShowSeconds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goToDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToUpcomingOccurenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editThisOccasionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteThisOccasionToolStripMenuItem;
    }
}