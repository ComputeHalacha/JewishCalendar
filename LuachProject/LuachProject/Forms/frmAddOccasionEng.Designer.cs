namespace LuachProject
{
    partial class frmAddOccasionEng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddOccasionEng));
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
            this.label6 = new System.Windows.Forms.Label();
            this.llClearBackColor = new System.Windows.Forms.LinkLabel();
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
            this.panel1.Location = new System.Drawing.Point(31, 205);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 151);
            this.panel1.TabIndex = 2;
            // 
            // rbSecularMonthly
            // 
            this.rbSecularMonthly.AutoSize = true;
            this.rbSecularMonthly.Location = new System.Drawing.Point(3, 123);
            this.rbSecularMonthly.Name = "rbSecularMonthly";
            this.rbSecularMonthly.Size = new System.Drawing.Size(130, 20);
            this.rbSecularMonthly.TabIndex = 4;
            this.rbSecularMonthly.Text = "Secular Monthly";
            this.rbSecularMonthly.UseVisualStyleBackColor = true;
            // 
            // rbSecularYearly
            // 
            this.rbSecularYearly.AutoSize = true;
            this.rbSecularYearly.Location = new System.Drawing.Point(3, 94);
            this.rbSecularYearly.Name = "rbSecularYearly";
            this.rbSecularYearly.Size = new System.Drawing.Size(120, 20);
            this.rbSecularYearly.TabIndex = 3;
            this.rbSecularYearly.Text = "Secular Yearly";
            this.rbSecularYearly.UseVisualStyleBackColor = true;
            // 
            // rbJewishMonthly
            // 
            this.rbJewishMonthly.AutoSize = true;
            this.rbJewishMonthly.Location = new System.Drawing.Point(3, 67);
            this.rbJewishMonthly.Name = "rbJewishMonthly";
            this.rbJewishMonthly.Size = new System.Drawing.Size(123, 20);
            this.rbJewishMonthly.TabIndex = 2;
            this.rbJewishMonthly.Text = "Jewish Monthly";
            this.rbJewishMonthly.UseVisualStyleBackColor = true;
            // 
            // rbJewishYearly
            // 
            this.rbJewishYearly.AutoSize = true;
            this.rbJewishYearly.Location = new System.Drawing.Point(3, 38);
            this.rbJewishYearly.Name = "rbJewishYearly";
            this.rbJewishYearly.Size = new System.Drawing.Size(113, 20);
            this.rbJewishYearly.TabIndex = 1;
            this.rbJewishYearly.Text = "Jewish Yearly";
            this.rbJewishYearly.UseVisualStyleBackColor = true;
            // 
            // rbOneTime
            // 
            this.rbOneTime.AutoSize = true;
            this.rbOneTime.Checked = true;
            this.rbOneTime.Location = new System.Drawing.Point(3, 9);
            this.rbOneTime.Name = "rbOneTime";
            this.rbOneTime.Size = new System.Drawing.Size(146, 20);
            this.rbOneTime.TabIndex = 0;
            this.rbOneTime.TabStop = true;
            this.rbOneTime.Text = "One time occasion";
            this.rbOneTime.UseVisualStyleBackColor = true;
            // 
            // jewishDatePicker1
            // 
            this.jewishDatePicker1.AutoSize = true;
            this.jewishDatePicker1.BackColor = System.Drawing.Color.White;
            this.jewishDatePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jewishDatePicker1.Language = JewishDatePicker.JewishDatePicker.Languages.English;
            this.jewishDatePicker1.Location = new System.Drawing.Point(515, 55);
            this.jewishDatePicker1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.jewishDatePicker1.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MaxDate")));
            this.jewishDatePicker1.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.MinDate")));
            this.jewishDatePicker1.Name = "jewishDatePicker1";
            this.jewishDatePicker1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.jewishDatePicker1.Size = new System.Drawing.Size(301, 29);
            this.jewishDatePicker1.TabIndex = 7;
            this.jewishDatePicker1.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jewishDatePicker1.Value")));
            this.jewishDatePicker1.ValueChanged += new System.EventHandler(this.jewishDatePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(515, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Jewish Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(515, 144);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(302, 24);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(515, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Secular Date";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(515, 321);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 31);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.Location = new System.Drawing.Point(714, 321);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 31);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(31, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Occasion or Event description:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(31, 56);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(427, 24);
            this.txtName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(31, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Occasion/event type:";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.Location = new System.Drawing.Point(615, 321);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 31);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(515, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Color of text";
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.SteelBlue;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnColor.Location = new System.Drawing.Point(515, 240);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(145, 37);
            this.btnColor.TabIndex = 9;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnBGColor
            // 
            this.btnBGColor.BackColor = System.Drawing.Color.Transparent;
            this.btnBGColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBGColor.Location = new System.Drawing.Point(666, 240);
            this.btnBGColor.Name = "btnBGColor";
            this.btnBGColor.Size = new System.Drawing.Size(150, 37);
            this.btnBGColor.TabIndex = 10;
            this.btnBGColor.UseVisualStyleBackColor = false;
            this.btnBGColor.Click += new System.EventHandler(this.btnBGColor_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(666, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Day Background Color";
            // 
            // llClearBackColor
            // 
            this.llClearBackColor.AutoSize = true;
            this.llClearBackColor.BackColor = System.Drawing.Color.Transparent;
            this.llClearBackColor.Location = new System.Drawing.Point(720, 280);
            this.llClearBackColor.Name = "llClearBackColor";
            this.llClearBackColor.Size = new System.Drawing.Size(42, 16);
            this.llClearBackColor.TabIndex = 15;
            this.llClearBackColor.TabStop = true;
            this.llClearBackColor.Text = "Clear";
            this.llClearBackColor.Visible = false;
            this.llClearBackColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llClearBackColor_LinkClicked);
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(31, 114);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(427, 65);
            this.txtNotes.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(31, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(186, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "Occasion Details and Notes:";
            // 
            // frmAddOccasionEng
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(848, 380);
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
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Century Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddOccasionEng";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add New Occasion or Event";
            this.TransparencyKey = System.Drawing.Color.GhostWhite;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddOccasionEng_FormClosing);
            this.Load += new System.EventHandler(this.frmAddOccasionEng_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmAddOccasionEng_Paint);
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel llClearBackColor;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label7;
    }
}