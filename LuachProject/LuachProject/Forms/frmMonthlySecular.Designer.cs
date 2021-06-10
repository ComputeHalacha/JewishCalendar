namespace LuachProject
{
    partial class frmMonthlySecular
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonthlySecular));
            this.lblMonthName = new System.Windows.Forms.Label();
            this.llChangeLanguage = new System.Windows.Forms.LinkLabel();
            this.llToJewishCalendar = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.llOccasionList = new System.Windows.Forms.LinkLabel();
            this.llShowDaily = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new LuachProject.CuelessSplitContainer();
            this.splitContainer2 = new LuachProject.CuelessSplitContainer();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblLocationHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbInIsrael = new System.Windows.Forms.RadioButton();
            this.rbInChul = new System.Windows.Forms.RadioButton();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.btnPreviousYear = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.lblNavigationHeader = new System.Windows.Forms.Label();
            this.btnNextYear = new System.Windows.Forms.Button();
            this.btnPreviousMonth = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlControls.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMonthName
            // 
            this.lblMonthName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonthName.Font = new System.Drawing.Font("Century Gothic", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMonthName.Location = new System.Drawing.Point(52, 0);
            this.lblMonthName.Name = "lblMonthName";
            this.lblMonthName.Size = new System.Drawing.Size(1127, 45);
            this.lblMonthName.TabIndex = 1;
            this.lblMonthName.Text = "Month";
            this.lblMonthName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llChangeLanguage
            // 
            this.llChangeLanguage.Dock = System.Windows.Forms.DockStyle.Left;
            this.llChangeLanguage.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llChangeLanguage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llChangeLanguage.Location = new System.Drawing.Point(0, 0);
            this.llChangeLanguage.Name = "llChangeLanguage";
            this.llChangeLanguage.Size = new System.Drawing.Size(52, 45);
            this.llChangeLanguage.TabIndex = 19;
            this.llChangeLanguage.TabStop = true;
            this.llChangeLanguage.Text = "עברית";
            this.llChangeLanguage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.llChangeLanguage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // llToJewishCalendar
            // 
            this.llToJewishCalendar.Dock = System.Windows.Forms.DockStyle.Right;
            this.llToJewishCalendar.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llToJewishCalendar.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llToJewishCalendar.Location = new System.Drawing.Point(1179, 0);
            this.llToJewishCalendar.Name = "llToJewishCalendar";
            this.llToJewishCalendar.Size = new System.Drawing.Size(119, 45);
            this.llToJewishCalendar.TabIndex = 21;
            this.llToJewishCalendar.TabStop = true;
            this.llToJewishCalendar.Text = "Jewish Calendar";
            this.llToJewishCalendar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.llToJewishCalendar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llToJewishCalendar_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.llOccasionList);
            this.panel2.Controls.Add(this.llShowDaily);
            this.panel2.Controls.Add(this.lblMonthName);
            this.panel2.Controls.Add(this.llChangeLanguage);
            this.panel2.Controls.Add(this.llToJewishCalendar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1298, 45);
            this.panel2.TabIndex = 22;
            // 
            // llOccasionList
            // 
            this.llOccasionList.Dock = System.Windows.Forms.DockStyle.Right;
            this.llOccasionList.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llOccasionList.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llOccasionList.LinkColor = System.Drawing.Color.Green;
            this.llOccasionList.Location = new System.Drawing.Point(1040, 0);
            this.llOccasionList.Name = "llOccasionList";
            this.llOccasionList.Size = new System.Drawing.Size(139, 45);
            this.llOccasionList.TabIndex = 23;
            this.llOccasionList.TabStop = true;
            this.llOccasionList.Text = "List of Occasions";
            this.llOccasionList.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.llOccasionList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llOccasionList_LinkClicked);
            // 
            // llShowDaily
            // 
            this.llShowDaily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llShowDaily.Font = new System.Drawing.Font("Century Gothic", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llShowDaily.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llShowDaily.LinkColor = System.Drawing.Color.SlateGray;
            this.llShowDaily.Location = new System.Drawing.Point(1177, 28);
            this.llShowDaily.Name = "llShowDaily";
            this.llShowDaily.Size = new System.Drawing.Size(120, 19);
            this.llShowDaily.TabIndex = 22;
            this.llShowDaily.TabStop = true;
            this.llShowDaily.Text = "Show Zmanim ˃";
            this.llShowDaily.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.splitContainer1.Size = new System.Drawing.Size(1298, 690);
            this.splitContainer1.SplitterDistance = 966;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 20;
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
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer2.Panel2.Controls.Add(this.pnlControls);
            this.splitContainer2.Panel2.Controls.Add(this.lblInstructions);
            this.splitContainer2.Panel2MinSize = 100;
            this.splitContainer2.Size = new System.Drawing.Size(966, 690);
            this.splitContainer2.SplitterDistance = 537;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 1;
            this.splitContainer2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.splitContainer2_KeyDown);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(9, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(957, 537);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseClick);
            this.pnlMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDoubleClick);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.dateTimePicker1);
            this.pnlControls.Controls.Add(this.lblLocationHeader);
            this.pnlControls.Controls.Add(this.panel1);
            this.pnlControls.Controls.Add(this.btnNextMonth);
            this.pnlControls.Controls.Add(this.cmbLocation);
            this.pnlControls.Controls.Add(this.btnPreviousYear);
            this.pnlControls.Controls.Add(this.btnToday);
            this.pnlControls.Controls.Add(this.lblNavigationHeader);
            this.pnlControls.Controls.Add(this.btnNextYear);
            this.pnlControls.Controls.Add(this.btnPreviousMonth);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 25);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(966, 127);
            this.pnlControls.TabIndex = 19;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(495, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(371, 22);
            this.dateTimePicker1.TabIndex = 19;
            this.dateTimePicker1.TabStop = false;
            // 
            // lblLocationHeader
            // 
            this.lblLocationHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblLocationHeader.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblLocationHeader.ForeColor = System.Drawing.Color.GhostWhite;
            this.lblLocationHeader.Location = new System.Drawing.Point(3, 0);
            this.lblLocationHeader.Name = "lblLocationHeader";
            this.lblLocationHeader.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblLocationHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLocationHeader.Size = new System.Drawing.Size(351, 30);
            this.lblLocationHeader.TabIndex = 10;
            this.lblLocationHeader.Text = "Location:";
            this.lblLocationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbInIsrael);
            this.panel1.Controls.Add(this.rbInChul);
            this.panel1.Location = new System.Drawing.Point(6, 41);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 36);
            this.panel1.TabIndex = 11;
            // 
            // rbInIsrael
            // 
            this.rbInIsrael.Checked = global::LuachProject.Properties.Settings.Default.rbInIsraelChecked;
            this.rbInIsrael.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::LuachProject.Properties.Settings.Default, "rbInIsraelChecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rbInIsrael.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbInIsrael.Location = new System.Drawing.Point(0, 0);
            this.rbInIsrael.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInIsrael.Name = "rbInIsrael";
            this.rbInIsrael.Size = new System.Drawing.Size(169, 36);
            this.rbInIsrael.TabIndex = 1;
            this.rbInIsrael.TabStop = true;
            this.rbInIsrael.Text = "In Eretz Yisroel";
            this.rbInIsrael.UseVisualStyleBackColor = true;
            this.rbInIsrael.CheckedChanged += new System.EventHandler(this.rbInIsrael_CheckedChanged);
            // 
            // rbInChul
            // 
            this.rbInChul.Dock = System.Windows.Forms.DockStyle.Right;
            this.rbInChul.Location = new System.Drawing.Point(223, 0);
            this.rbInChul.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbInChul.Name = "rbInChul";
            this.rbInChul.Size = new System.Drawing.Size(125, 36);
            this.rbInChul.TabIndex = 0;
            this.rbInChul.Text = "Elsewhere";
            this.rbInChul.UseVisualStyleBackColor = true;
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.BackColor = System.Drawing.Color.Lavender;
            this.btnNextMonth.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNextMonth.FlatAppearance.BorderSize = 0;
            this.btnNextMonth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnNextMonth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnNextMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextMonth.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnNextMonth.ForeColor = System.Drawing.Color.LightSlateGray;
            this.btnNextMonth.Location = new System.Drawing.Point(685, 37);
            this.btnNextMonth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNextMonth.Size = new System.Drawing.Size(182, 33);
            this.btnNextMonth.TabIndex = 12;
            this.btnNextMonth.TabStop = false;
            this.btnNextMonth.Text = "Next Month →";
            this.btnNextMonth.UseVisualStyleBackColor = false;
            this.btnNextMonth.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbLocation
            // 
            this.cmbLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(3, 84);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(351, 21);
            this.cmbLocation.Sorted = true;
            this.cmbLocation.TabIndex = 9;
            this.cmbLocation.TabStop = false;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // btnPreviousYear
            // 
            this.btnPreviousYear.BackColor = System.Drawing.Color.Lavender;
            this.btnPreviousYear.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPreviousYear.FlatAppearance.BorderSize = 0;
            this.btnPreviousYear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPreviousYear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPreviousYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousYear.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnPreviousYear.ForeColor = System.Drawing.Color.LightSlateGray;
            this.btnPreviousYear.Location = new System.Drawing.Point(372, 79);
            this.btnPreviousYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPreviousYear.Name = "btnPreviousYear";
            this.btnPreviousYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPreviousYear.Size = new System.Drawing.Size(182, 33);
            this.btnPreviousYear.TabIndex = 17;
            this.btnPreviousYear.TabStop = false;
            this.btnPreviousYear.Text = "← Previous Year";
            this.btnPreviousYear.UseVisualStyleBackColor = false;
            this.btnPreviousYear.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnToday.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnToday.Location = new System.Drawing.Point(570, 37);
            this.btnToday.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnToday.Name = "btnToday";
            this.btnToday.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnToday.Size = new System.Drawing.Size(101, 75);
            this.btnToday.TabIndex = 14;
            this.btnToday.TabStop = false;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblNavigationHeader
            // 
            this.lblNavigationHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblNavigationHeader.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblNavigationHeader.ForeColor = System.Drawing.Color.GhostWhite;
            this.lblNavigationHeader.Location = new System.Drawing.Point(372, 0);
            this.lblNavigationHeader.Name = "lblNavigationHeader";
            this.lblNavigationHeader.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblNavigationHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblNavigationHeader.Size = new System.Drawing.Size(495, 30);
            this.lblNavigationHeader.TabIndex = 15;
            this.lblNavigationHeader.Text = "Navigation:";
            this.lblNavigationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNextYear
            // 
            this.btnNextYear.BackColor = System.Drawing.Color.Lavender;
            this.btnNextYear.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNextYear.FlatAppearance.BorderSize = 0;
            this.btnNextYear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnNextYear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnNextYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextYear.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnNextYear.ForeColor = System.Drawing.Color.LightSlateGray;
            this.btnNextYear.Location = new System.Drawing.Point(685, 79);
            this.btnNextYear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNextYear.Name = "btnNextYear";
            this.btnNextYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNextYear.Size = new System.Drawing.Size(182, 33);
            this.btnNextYear.TabIndex = 18;
            this.btnNextYear.TabStop = false;
            this.btnNextYear.Text = "Next Year →";
            this.btnNextYear.UseVisualStyleBackColor = false;
            this.btnNextYear.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnPreviousMonth
            // 
            this.btnPreviousMonth.BackColor = System.Drawing.Color.Lavender;
            this.btnPreviousMonth.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPreviousMonth.FlatAppearance.BorderSize = 0;
            this.btnPreviousMonth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPreviousMonth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPreviousMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousMonth.Font = new System.Drawing.Font("Century Gothic", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnPreviousMonth.ForeColor = System.Drawing.Color.LightSlateGray;
            this.btnPreviousMonth.Location = new System.Drawing.Point(372, 37);
            this.btnPreviousMonth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPreviousMonth.Name = "btnPreviousMonth";
            this.btnPreviousMonth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPreviousMonth.Size = new System.Drawing.Size(182, 33);
            this.btnPreviousMonth.TabIndex = 13;
            this.btnPreviousMonth.TabStop = false;
            this.btnPreviousMonth.Text = "← Previous Month";
            this.btnPreviousMonth.UseVisualStyleBackColor = false;
            this.btnPreviousMonth.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInstructions.ForeColor = System.Drawing.Color.DimGray;
            this.lblInstructions.Location = new System.Drawing.Point(0, 0);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(966, 28);
            this.lblInstructions.TabIndex = 16;
            this.lblInstructions.Text = "Click any day to see Zmanim, Occasions and Events   |   Double-click to add an Ev" +
    "ent   |   Use the arrow keys to navigate through the days.";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Image = global::LuachProject.Properties.Resources.print;
            this.pictureBox2.Location = new System.Drawing.Point(52, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 45);
            this.pictureBox2.TabIndex = 28;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // frmMonthlySecular
            // 
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1298, 735);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMonthlySecular";
            this.Text = "Luach";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMonthlyEnglish_FormClosed);
            this.Load += new System.EventHandler(this.frmMonthlySecular_Load);
            this.Resize += new System.EventHandler(this.frmMonthlyEnglish_Resize);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlControls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblMonthName;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbInIsrael;
        private System.Windows.Forms.RadioButton rbInChul;
        private System.Windows.Forms.Label lblLocationHeader;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnPreviousMonth;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Label lblNavigationHeader;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button btnPreviousYear;
        private System.Windows.Forms.Button btnNextYear;
        private System.Windows.Forms.LinkLabel llChangeLanguage;
        private CuelessSplitContainer splitContainer1;
        private CuelessSplitContainer splitContainer2;
        private System.Windows.Forms.LinkLabel llToJewishCalendar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel llShowDaily;
        private System.Windows.Forms.LinkLabel llOccasionList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}