namespace ZmanimChart
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.clmZman = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmDaysOfWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAlternateOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBold = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDOWNone = new System.Windows.Forms.RadioButton();
            this.rbDOWEnglish = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbDowNum = new System.Windows.Forms.RadioButton();
            this.rbDOWJewishNum = new System.Windows.Forms.RadioButton();
            this.rbDayOfWeekFull = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.jdpFrom = new JewishDatePicker.JewishDatePicker();
            this.jdpTo = new JewishDatePicker.JewishDatePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pnlDateTypeMonth = new System.Windows.Forms.Panel();
            this.pnlDateTypeFromTo = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.choiceSeconds = new ChoiceSwitcher();
            this.choiceSwitcherDateType = new ChoiceSwitcher();
            this.choiceDirection = new ChoiceSwitcher();
            this.choiceWidth100 = new ChoiceSwitcher();
            this.choiceAmPm = new ChoiceSwitcher();
            this.choiceArmy = new ChoiceSwitcher();
            this.choiceSwitcher1 = new ChoiceSwitcher();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlDateTypeMonth.SuspendLayout();
            this.pnlDateTypeFromTo.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(490, 553);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate / יצר";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // cmbLocations
            // 
            this.cmbLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FormattingEnabled = true;
            this.cmbLocations.Location = new System.Drawing.Point(18, 33);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbLocations.Size = new System.Drawing.Size(260, 23);
            this.cmbLocations.Sorted = true;
            this.cmbLocations.TabIndex = 1;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.CmbLocations_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = " Location / מיקום";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = " Month / חודש";
            // 
            // cmbMonth
            // 
            this.cmbMonth.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMonth.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(3, 21);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbMonth.Size = new System.Drawing.Size(140, 23);
            this.cmbMonth.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = " Year / שנה";
            // 
            // cmbYear
            // 
            this.cmbYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(155, 21);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbYear.Size = new System.Drawing.Size(111, 23);
            this.cmbYear.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmZman,
            this.clmDaysOfWeek,
            this.clmOffset,
            this.clmAlternateOffset,
            this.clmHeader,
            this.clmBold,
            this.clmDelete});
            this.dataGridView1.Location = new System.Drawing.Point(17, 210);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1093, 320);
            this.dataGridView1.TabIndex = 16;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // clmZman
            // 
            this.clmZman.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.clmZman.DropDownWidth = 2;
            this.clmZman.HeaderText = "Zman / זמן";
            this.clmZman.Name = "clmZman";
            this.clmZman.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmZman.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmZman.Width = 450;
            // 
            // clmDaysOfWeek
            // 
            this.clmDaysOfWeek.HeaderText = "Days/ימים";
            this.clmDaysOfWeek.Name = "clmDaysOfWeek";
            this.clmDaysOfWeek.ReadOnly = true;
            // 
            // clmOffset
            // 
            this.clmOffset.HeaderText = "Offset / הוסף דקות";
            this.clmOffset.Name = "clmOffset";
            this.clmOffset.Width = 150;
            // 
            // clmAlternateOffset
            // 
            this.clmAlternateOffset.HeaderText = "Alt. Offset";
            this.clmAlternateOffset.Name = "clmAlternateOffset";
            // 
            // clmHeader
            // 
            this.clmHeader.HeaderText = "Heading / כותרת";
            this.clmHeader.Name = "clmHeader";
            this.clmHeader.Width = 150;
            // 
            // clmBold
            // 
            this.clmBold.HeaderText = "Bold / מודגש";
            this.clmBold.Name = "clmBold";
            this.clmBold.Width = 50;
            // 
            // clmDelete
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.clmDelete.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmDelete.HeaderText = "";
            this.clmDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.clmDelete.LinkColor = System.Drawing.Color.Maroon;
            this.clmDelete.Name = "clmDelete";
            this.clmDelete.Text = "x";
            this.clmDelete.ToolTipText = "Delete / הסר";
            this.clmDelete.UseColumnTextForLinkValue = true;
            this.clmDelete.VisitedLinkColor = System.Drawing.Color.Red;
            this.clmDelete.Width = 30;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(503, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Columns / עמודות";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDOWNone);
            this.groupBox1.Controls.Add(this.rbDOWEnglish);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.rbDowNum);
            this.groupBox1.Controls.Add(this.rbDOWJewishNum);
            this.groupBox1.Controls.Add(this.rbDayOfWeekFull);
            this.groupBox1.Location = new System.Drawing.Point(355, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 141);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Day of Week / יום בשבוע";
            // 
            // rbDOWNone
            // 
            this.rbDOWNone.AutoSize = true;
            this.rbDOWNone.Location = new System.Drawing.Point(9, 112);
            this.rbDOWNone.Name = "rbDOWNone";
            this.rbDOWNone.Size = new System.Drawing.Size(86, 19);
            this.rbDOWNone.TabIndex = 6;
            this.rbDOWNone.Text = "None / ללא";
            this.rbDOWNone.UseVisualStyleBackColor = true;
            // 
            // rbDOWEnglish
            // 
            this.rbDOWEnglish.AutoSize = true;
            this.rbDOWEnglish.Location = new System.Drawing.Point(9, 88);
            this.rbDOWEnglish.Name = "rbDOWEnglish";
            this.rbDOWEnglish.Size = new System.Drawing.Size(79, 19);
            this.rbDOWEnglish.TabIndex = 5;
            this.rbDOWEnglish.Text = "Sun, Mon";
            this.rbDOWEnglish.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 44);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(34, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "א\' / ב\'";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "ראשון / שני";
            // 
            // rbDowNum
            // 
            this.rbDowNum.AutoSize = true;
            this.rbDowNum.Location = new System.Drawing.Point(9, 64);
            this.rbDowNum.Name = "rbDowNum";
            this.rbDowNum.Size = new System.Drawing.Size(48, 19);
            this.rbDowNum.TabIndex = 2;
            this.rbDowNum.Text = "1, 2";
            this.rbDowNum.UseVisualStyleBackColor = true;
            // 
            // rbDOWJewishNum
            // 
            this.rbDOWJewishNum.AutoSize = true;
            this.rbDOWJewishNum.Checked = true;
            this.rbDOWJewishNum.Location = new System.Drawing.Point(9, 43);
            this.rbDOWJewishNum.Name = "rbDOWJewishNum";
            this.rbDOWJewishNum.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbDOWJewishNum.Size = new System.Drawing.Size(17, 16);
            this.rbDOWJewishNum.TabIndex = 1;
            this.rbDOWJewishNum.TabStop = true;
            this.rbDOWJewishNum.UseVisualStyleBackColor = true;
            // 
            // rbDayOfWeekFull
            // 
            this.rbDayOfWeekFull.AutoSize = true;
            this.rbDayOfWeekFull.Location = new System.Drawing.Point(9, 22);
            this.rbDayOfWeekFull.Name = "rbDayOfWeekFull";
            this.rbDayOfWeekFull.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbDayOfWeekFull.Size = new System.Drawing.Size(17, 16);
            this.rbDayOfWeekFull.TabIndex = 0;
            this.rbDayOfWeekFull.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(610, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "AM / PM";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(603, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 15);
            this.label8.TabIndex = 22;
            this.label8.Text = "Time / שעה";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(769, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 15);
            this.label9.TabIndex = 24;
            this.label9.Text = "100% Width / רוחב";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(783, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 15);
            this.label10.TabIndex = 26;
            this.label10.Text = "Direction / כיוון";
            // 
            // jdpFrom
            // 
            this.jdpFrom.AutoSize = true;
            this.jdpFrom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jdpFrom.BackColor = System.Drawing.Color.White;
            this.jdpFrom.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jdpFrom.ForeColor = System.Drawing.Color.Black;
            this.jdpFrom.Location = new System.Drawing.Point(67, 0);
            this.jdpFrom.Margin = new System.Windows.Forms.Padding(0);
            this.jdpFrom.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.MaxDate")));
            this.jdpFrom.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.MinDate")));
            this.jdpFrom.Name = "jdpFrom";
            this.jdpFrom.Size = new System.Drawing.Size(245, 28);
            this.jdpFrom.TabIndex = 27;
            this.jdpFrom.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.Value")));
            // 
            // jdpTo
            // 
            this.jdpTo.AutoSize = true;
            this.jdpTo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jdpTo.BackColor = System.Drawing.Color.White;
            this.jdpTo.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jdpTo.ForeColor = System.Drawing.Color.Black;
            this.jdpTo.Location = new System.Drawing.Point(67, 36);
            this.jdpTo.Margin = new System.Windows.Forms.Padding(0);
            this.jdpTo.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpTo.MaxDate")));
            this.jdpTo.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpTo.MinDate")));
            this.jdpTo.Name = "jdpTo";
            this.jdpTo.Size = new System.Drawing.Size(245, 28);
            this.jdpTo.TabIndex = 28;
            this.jdpTo.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jdpTo.Value")));
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 15);
            this.label11.TabIndex = 30;
            this.label11.Text = " From / מ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 15);
            this.label12.TabIndex = 31;
            this.label12.Text = " Until / עד";
            // 
            // pnlDateTypeMonth
            // 
            this.pnlDateTypeMonth.Controls.Add(this.cmbYear);
            this.pnlDateTypeMonth.Controls.Add(this.cmbMonth);
            this.pnlDateTypeMonth.Controls.Add(this.label2);
            this.pnlDateTypeMonth.Controls.Add(this.label3);
            this.pnlDateTypeMonth.Location = new System.Drawing.Point(18, 94);
            this.pnlDateTypeMonth.Name = "pnlDateTypeMonth";
            this.pnlDateTypeMonth.Size = new System.Drawing.Size(270, 46);
            this.pnlDateTypeMonth.TabIndex = 32;
            // 
            // pnlDateTypeFromTo
            // 
            this.pnlDateTypeFromTo.Controls.Add(this.label11);
            this.pnlDateTypeFromTo.Controls.Add(this.jdpFrom);
            this.pnlDateTypeFromTo.Controls.Add(this.label12);
            this.pnlDateTypeFromTo.Controls.Add(this.jdpTo);
            this.pnlDateTypeFromTo.Location = new System.Drawing.Point(17, 94);
            this.pnlDateTypeFromTo.Name = "pnlDateTypeFromTo";
            this.pnlDateTypeFromTo.Size = new System.Drawing.Size(313, 67);
            this.pnlDateTypeFromTo.TabIndex = 33;
            this.pnlDateTypeFromTo.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(594, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 15);
            this.label13.TabIndex = 35;
            this.label13.Text = "Seconds / שניות";
            // 
            // choiceSeconds
            // 
            this.choiceSeconds.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceSeconds.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceSeconds.BackColorSlot = System.Drawing.Color.Teal;
            this.choiceSeconds.BackColorSlotChoiceTwo = System.Drawing.Color.Gray;
            this.choiceSeconds.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceSeconds.ChoiceOneSelected = true;
            this.choiceSeconds.ChoiceTwoSelected = false;
            this.choiceSeconds.DisplayAsYesNo = false;
            this.choiceSeconds.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceSeconds.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSeconds.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSeconds.FontSize = 7F;
            this.choiceSeconds.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceSeconds.ForeColorSelected = System.Drawing.SystemColors.ControlText;
            this.choiceSeconds.Location = new System.Drawing.Point(564, 130);
            this.choiceSeconds.Name = "choiceSeconds";
            this.choiceSeconds.SelectedValue = false;
            this.choiceSeconds.Size = new System.Drawing.Size(153, 23);
            this.choiceSeconds.TabIndex = 34;
            this.choiceSeconds.Text = "choiceSwitcher2";
            this.choiceSeconds.TextChoiceOne = "Yes / כן";
            this.choiceSeconds.TextChoiceTwo = "No / לא";
            this.choiceSeconds.ValueChoiceOne = false;
            this.choiceSeconds.ValueChoiceTwo = true;
            // 
            // choiceSwitcherDateType
            // 
            this.choiceSwitcherDateType.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceSwitcherDateType.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceSwitcherDateType.BackColorSlot = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.choiceSwitcherDateType.BackColorSlotChoiceTwo = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.choiceSwitcherDateType.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceSwitcherDateType.ChoiceOneSelected = true;
            this.choiceSwitcherDateType.ChoiceTwoSelected = false;
            this.choiceSwitcherDateType.DisplayAsYesNo = false;
            this.choiceSwitcherDateType.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceSwitcherDateType.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSwitcherDateType.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSwitcherDateType.FontSize = 7F;
            this.choiceSwitcherDateType.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceSwitcherDateType.ForeColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.choiceSwitcherDateType.Location = new System.Drawing.Point(24, 65);
            this.choiceSwitcherDateType.Name = "choiceSwitcherDateType";
            this.choiceSwitcherDateType.SelectedValue = null;
            this.choiceSwitcherDateType.Size = new System.Drawing.Size(284, 23);
            this.choiceSwitcherDateType.TabIndex = 29;
            this.choiceSwitcherDateType.Text = "choiceSwitcher2";
            this.choiceSwitcherDateType.TextChoiceOne = "Month / חודש";
            this.choiceSwitcherDateType.TextChoiceTwo = "Choose Dates / לבחור תאריכים";
            this.choiceSwitcherDateType.ValueChoiceOne = null;
            this.choiceSwitcherDateType.ValueChoiceTwo = null;
            this.choiceSwitcherDateType.ChoiceSwitched += new System.EventHandler(this.choiceSwitcherDateType_ChoiceSwitched);
            // 
            // choiceDirection
            // 
            this.choiceDirection.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceDirection.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceDirection.BackColorSlot = System.Drawing.Color.RoyalBlue;
            this.choiceDirection.BackColorSlotChoiceTwo = System.Drawing.Color.RoyalBlue;
            this.choiceDirection.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceDirection.ChoiceOneSelected = true;
            this.choiceDirection.ChoiceTwoSelected = false;
            this.choiceDirection.DisplayAsYesNo = false;
            this.choiceDirection.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceDirection.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceDirection.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceDirection.FontSize = 7F;
            this.choiceDirection.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceDirection.ForeColorSelected = System.Drawing.SystemColors.ControlText;
            this.choiceDirection.Location = new System.Drawing.Point(740, 83);
            this.choiceDirection.Name = "choiceDirection";
            this.choiceDirection.SelectedValue = false;
            this.choiceDirection.Size = new System.Drawing.Size(181, 23);
            this.choiceDirection.TabIndex = 25;
            this.choiceDirection.Text = "choiceSwitcher2";
            this.choiceDirection.TextChoiceOne = "Left / שמאל";
            this.choiceDirection.TextChoiceTwo = "Right / ימין";
            this.choiceDirection.ValueChoiceOne = false;
            this.choiceDirection.ValueChoiceTwo = true;
            // 
            // choiceWidth100
            // 
            this.choiceWidth100.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceWidth100.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceWidth100.BackColorSlot = System.Drawing.Color.Gray;
            this.choiceWidth100.BackColorSlotChoiceTwo = System.Drawing.Color.Teal;
            this.choiceWidth100.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceWidth100.ChoiceOneSelected = false;
            this.choiceWidth100.ChoiceTwoSelected = true;
            this.choiceWidth100.DisplayAsYesNo = false;
            this.choiceWidth100.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceWidth100.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceWidth100.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceWidth100.FontSize = 7F;
            this.choiceWidth100.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceWidth100.ForeColorSelected = System.Drawing.SystemColors.ControlText;
            this.choiceWidth100.Location = new System.Drawing.Point(756, 23);
            this.choiceWidth100.Name = "choiceWidth100";
            this.choiceWidth100.SelectedValue = true;
            this.choiceWidth100.Size = new System.Drawing.Size(149, 23);
            this.choiceWidth100.TabIndex = 23;
            this.choiceWidth100.Text = "choiceSwitcher2";
            this.choiceWidth100.TextChoiceOne = "No / לא";
            this.choiceWidth100.TextChoiceTwo = "Yes / כן";
            this.choiceWidth100.ValueChoiceOne = false;
            this.choiceWidth100.ValueChoiceTwo = true;
            // 
            // choiceAmPm
            // 
            this.choiceAmPm.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceAmPm.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceAmPm.BackColorSlot = System.Drawing.Color.Teal;
            this.choiceAmPm.BackColorSlotChoiceTwo = System.Drawing.Color.Gray;
            this.choiceAmPm.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceAmPm.ChoiceOneSelected = true;
            this.choiceAmPm.ChoiceTwoSelected = false;
            this.choiceAmPm.DisplayAsYesNo = false;
            this.choiceAmPm.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceAmPm.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceAmPm.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceAmPm.FontSize = 7F;
            this.choiceAmPm.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceAmPm.ForeColorSelected = System.Drawing.SystemColors.ControlText;
            this.choiceAmPm.Location = new System.Drawing.Point(564, 77);
            this.choiceAmPm.Name = "choiceAmPm";
            this.choiceAmPm.SelectedValue = false;
            this.choiceAmPm.Size = new System.Drawing.Size(153, 23);
            this.choiceAmPm.TabIndex = 20;
            this.choiceAmPm.Text = "choiceSwitcher2";
            this.choiceAmPm.TextChoiceOne = "Yes / כן";
            this.choiceAmPm.TextChoiceTwo = "No / לא";
            this.choiceAmPm.ValueChoiceOne = false;
            this.choiceAmPm.ValueChoiceTwo = true;
            // 
            // choiceArmy
            // 
            this.choiceArmy.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceArmy.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceArmy.BackColorSlot = System.Drawing.Color.Teal;
            this.choiceArmy.BackColorSlotChoiceTwo = System.Drawing.Color.Teal;
            this.choiceArmy.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceArmy.ChoiceOneSelected = true;
            this.choiceArmy.ChoiceTwoSelected = false;
            this.choiceArmy.DisplayAsYesNo = false;
            this.choiceArmy.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceArmy.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceArmy.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceArmy.FontSize = 7F;
            this.choiceArmy.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceArmy.ForeColorSelected = System.Drawing.SystemColors.ControlText;
            this.choiceArmy.Location = new System.Drawing.Point(540, 23);
            this.choiceArmy.Name = "choiceArmy";
            this.choiceArmy.SelectedValue = false;
            this.choiceArmy.Size = new System.Drawing.Size(200, 23);
            this.choiceArmy.TabIndex = 19;
            this.choiceArmy.Text = "choiceSwitcher2";
            this.choiceArmy.TextChoiceOne = "Army / צבאי";
            this.choiceArmy.TextChoiceTwo = "Regular / רגיל";
            this.choiceArmy.ValueChoiceOne = false;
            this.choiceArmy.ValueChoiceTwo = true;
            // 
            // choiceSwitcher1
            // 
            this.choiceSwitcher1.BackColorNotSelected = System.Drawing.SystemColors.Control;
            this.choiceSwitcher1.BackColorSelected = System.Drawing.SystemColors.Control;
            this.choiceSwitcher1.BackColorSlot = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.BackColorSlotChoiceTwo = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher1.ChoiceOneSelected = false;
            this.choiceSwitcher1.ChoiceTwoSelected = true;
            this.choiceSwitcher1.DisplayAsYesNo = false;
            this.choiceSwitcher1.Font = new System.Drawing.Font("Verdana", 7F);
            this.choiceSwitcher1.FontNotSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSwitcher1.FontSelected = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.choiceSwitcher1.FontSize = 7F;
            this.choiceSwitcher1.ForeColorNotSelected = System.Drawing.SystemColors.ControlText;
            this.choiceSwitcher1.ForeColorSelected = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.Location = new System.Drawing.Point(141, 7);
            this.choiceSwitcher1.Name = "choiceSwitcher1";
            this.choiceSwitcher1.SelectedValue = null;
            this.choiceSwitcher1.Size = new System.Drawing.Size(140, 23);
            this.choiceSwitcher1.TabIndex = 7;
            this.choiceSwitcher1.Text = "choiceSwitcher1";
            this.choiceSwitcher1.TextChoiceOne = "World";
            this.choiceSwitcher1.TextChoiceTwo = "ישראל";
            this.choiceSwitcher1.ValueChoiceOne = null;
            this.choiceSwitcher1.ValueChoiceTwo = null;
            this.choiceSwitcher1.ChoiceSwitched += new System.EventHandler(this.ChoiceSwitcher1_ChoiceSwitched_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 611);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.choiceSeconds);
            this.Controls.Add(this.pnlDateTypeMonth);
            this.Controls.Add(this.choiceSwitcherDateType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.choiceDirection);
            this.Controls.Add(this.pnlDateTypeFromTo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.choiceWidth100);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.choiceAmPm);
            this.Controls.Add(this.choiceArmy);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.choiceSwitcher1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLocations);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Create Calendar / יצר רשימה";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlDateTypeMonth.ResumeLayout(false);
            this.pnlDateTypeMonth.PerformLayout();
            this.pnlDateTypeFromTo.ResumeLayout(false);
            this.pnlDateTypeFromTo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbLocations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbYear;
        private ChoiceSwitcher choiceSwitcher1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDowNum;
        private System.Windows.Forms.RadioButton rbDOWJewishNum;
        private System.Windows.Forms.RadioButton rbDayOfWeekFull;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ChoiceSwitcher choiceArmy;
        private ChoiceSwitcher choiceAmPm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private ChoiceSwitcher choiceWidth100;
        private System.Windows.Forms.Label label10;
        private ChoiceSwitcher choiceDirection;
        private System.Windows.Forms.RadioButton rbDOWEnglish;
        private System.Windows.Forms.RadioButton rbDOWNone;
        private JewishDatePicker.JewishDatePicker jdpFrom;
        private JewishDatePicker.JewishDatePicker jdpTo;
        private ChoiceSwitcher choiceSwitcherDateType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel pnlDateTypeMonth;
        private System.Windows.Forms.Panel pnlDateTypeFromTo;
        private System.Windows.Forms.Label label13;
        private ChoiceSwitcher choiceSeconds;
        private System.Windows.Forms.DataGridViewComboBoxColumn clmZman;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDaysOfWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAlternateOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmHeader;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmBold;
        private System.Windows.Forms.DataGridViewLinkColumn clmDelete;
    }
}

