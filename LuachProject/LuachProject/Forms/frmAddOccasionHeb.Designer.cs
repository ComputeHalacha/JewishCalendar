namespace LuachProject
{
    partial class frmAddOccasionHeb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddOccasionHeb));
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbSecularMonthly = new System.Windows.Forms.RadioButton();
            this.rbSecularYearly = new System.Windows.Forms.RadioButton();
            this.rbJewishMonthly = new System.Windows.Forms.RadioButton();
            this.rbJewishYearly = new System.Windows.Forms.RadioButton();
            this.rbOneTime = new System.Windows.Forms.RadioButton();
            this.jewishDatePicker1 = new JewishDatePicker.JewishDatePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnBGColor = new System.Windows.Forms.Button();
            this.llClearBackColor = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.rbSecularMonthly);
            this.panel1.Controls.Add(this.rbSecularYearly);
            this.panel1.Controls.Add(this.rbJewishMonthly);
            this.panel1.Controls.Add(this.rbJewishYearly);
            this.panel1.Controls.Add(this.rbOneTime);
            this.panel1.Location = new System.Drawing.Point(44, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 150);
            this.panel1.TabIndex = 2;
            // 
            // rbSecularMonthly
            // 
            this.rbSecularMonthly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbSecularMonthly.AutoSize = true;
            this.rbSecularMonthly.Location = new System.Drawing.Point(314, 119);
            this.rbSecularMonthly.Name = "rbSecularMonthly";
            this.rbSecularMonthly.Size = new System.Drawing.Size(126, 21);
            this.rbSecularMonthly.TabIndex = 4;
            this.rbSecularMonthly.Text = "Secular Monthly";
            this.rbSecularMonthly.UseVisualStyleBackColor = true;
            // 
            // rbSecularYearly
            // 
            this.rbSecularYearly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbSecularYearly.AutoSize = true;
            this.rbSecularYearly.Location = new System.Drawing.Point(326, 90);
            this.rbSecularYearly.Name = "rbSecularYearly";
            this.rbSecularYearly.Size = new System.Drawing.Size(114, 21);
            this.rbSecularYearly.TabIndex = 3;
            this.rbSecularYearly.Text = "Secular Yearly";
            this.rbSecularYearly.UseVisualStyleBackColor = true;
            // 
            // rbJewishMonthly
            // 
            this.rbJewishMonthly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbJewishMonthly.AutoSize = true;
            this.rbJewishMonthly.Location = new System.Drawing.Point(319, 63);
            this.rbJewishMonthly.Name = "rbJewishMonthly";
            this.rbJewishMonthly.Size = new System.Drawing.Size(121, 21);
            this.rbJewishMonthly.TabIndex = 2;
            this.rbJewishMonthly.Text = "Jewish Monthly";
            this.rbJewishMonthly.UseVisualStyleBackColor = true;
            // 
            // rbJewishYearly
            // 
            this.rbJewishYearly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbJewishYearly.AutoSize = true;
            this.rbJewishYearly.Location = new System.Drawing.Point(332, 34);
            this.rbJewishYearly.Name = "rbJewishYearly";
            this.rbJewishYearly.Size = new System.Drawing.Size(109, 21);
            this.rbJewishYearly.TabIndex = 1;
            this.rbJewishYearly.Text = "Jewish Yearly";
            this.rbJewishYearly.UseVisualStyleBackColor = true;
            // 
            // rbOneTime
            // 
            this.rbOneTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbOneTime.AutoSize = true;
            this.rbOneTime.Checked = true;
            this.rbOneTime.Location = new System.Drawing.Point(299, 5);
            this.rbOneTime.Name = "rbOneTime";
            this.rbOneTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbOneTime.Size = new System.Drawing.Size(141, 21);
            this.rbOneTime.TabIndex = 0;
            this.rbOneTime.TabStop = true;
            this.rbOneTime.Text = "One time occasion";
            this.rbOneTime.UseVisualStyleBackColor = true;
            // 
            // jewishDatePicker1
            // 
            this.jewishDatePicker1.AutoSize = true;
            this.jewishDatePicker1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jewishDatePicker1.BackColor = System.Drawing.Color.White;
            this.jewishDatePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jewishDatePicker1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jewishDatePicker1.ForeColor = System.Drawing.Color.Black;
            this.jewishDatePicker1.Location = new System.Drawing.Point(512, 55);
            this.jewishDatePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.jewishDatePicker1.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MaxDate")));
            this.jewishDatePicker1.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MinDate")));
            this.jewishDatePicker1.Name = "jewishDatePicker1";
            this.jewishDatePicker1.Size = new System.Drawing.Size(247, 30);
            this.jewishDatePicker1.TabIndex = 7;
            this.jewishDatePicker1.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.Value")));
            this.jewishDatePicker1.ValueChanged += new System.EventHandler(this.jewishDatePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(512, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "תאריך עברי:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(512, 152);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(247, 23);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(512, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "תאריך לועזי:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(512, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 31);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "בטל";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.Location = new System.Drawing.Point(719, 354);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 31);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "הוסף";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(41, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "שם אירוע:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(46, 56);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(427, 23);
            this.txtName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(43, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "סוג אירוע:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.Location = new System.Drawing.Point(620, 354);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 31);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "מחק";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(512, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "צבע הכיתוב:";
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Red;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnColor.Location = new System.Drawing.Point(512, 240);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(120, 37);
            this.btnColor.TabIndex = 9;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnBGColor
            // 
            this.btnBGColor.BackColor = System.Drawing.Color.Transparent;
            this.btnBGColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBGColor.Location = new System.Drawing.Point(657, 240);
            this.btnBGColor.Name = "btnBGColor";
            this.btnBGColor.Size = new System.Drawing.Size(117, 37);
            this.btnBGColor.TabIndex = 10;
            this.btnBGColor.UseVisualStyleBackColor = false;
            this.btnBGColor.Click += new System.EventHandler(this.btnBGColor_Click);
            // 
            // llClearBackColor
            // 
            this.llClearBackColor.AutoSize = true;
            this.llClearBackColor.BackColor = System.Drawing.Color.Transparent;
            this.llClearBackColor.Location = new System.Drawing.Point(698, 280);
            this.llClearBackColor.Name = "llClearBackColor";
            this.llClearBackColor.Size = new System.Drawing.Size(33, 17);
            this.llClearBackColor.TabIndex = 18;
            this.llClearBackColor.TabStop = true;
            this.llClearBackColor.Text = "נקה";
            this.llClearBackColor.Visible = false;
            this.llClearBackColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llClearBackColor_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(654, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "צבע רקע היום בלוח:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(46, 114);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(427, 94);
            this.txtNotes.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(41, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "הערות ופרטי האירוע:";
            // 
            // frmAddOccasionHeb
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(848, 421);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnBGColor);
            this.Controls.Add(this.llClearBackColor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.jewishDatePicker1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAddOccasionHeb";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "אירוע";
            this.TransparencyKey = System.Drawing.Color.GhostWhite;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddOccasionEng_FormClosing);
            this.Load += new System.EventHandler(this.frmAddOccasionHeb_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmAddOccasionHeb_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbSecularMonthly;
        private System.Windows.Forms.RadioButton rbSecularYearly;
        private System.Windows.Forms.RadioButton rbJewishMonthly;
        private System.Windows.Forms.RadioButton rbJewishYearly;
        private System.Windows.Forms.RadioButton rbOneTime;
        private JewishDatePicker.JewishDatePicker jewishDatePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Button btnBGColor;
        private System.Windows.Forms.LinkLabel llClearBackColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label7;
    }
}