namespace ZmanimAnywhere
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.jdpFrom = new JewishDatePicker.JewishDatePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label3 = new System.Windows.Forms.Label();
            this.latDeg = new System.Windows.Forms.NumericUpDown();
            this.latMin = new System.Windows.Forms.NumericUpDown();
            this.latSec = new System.Windows.Forms.NumericUpDown();
            this.longSec = new System.Windows.Forms.NumericUpDown();
            this.longMin = new System.Windows.Forms.NumericUpDown();
            this.longDeg = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.nudElevation = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudTimeZone = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.latDeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.latMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.latSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.longSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.longMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.longDeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudElevation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeZone)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLocations
            // 
            this.cmbLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FormattingEnabled = true;
            this.cmbLocations.Location = new System.Drawing.Point(18, 146);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.Size = new System.Drawing.Size(260, 24);
            this.cmbLocations.Sorted = true;
            this.cmbLocations.TabIndex = 1;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.cmbLocations_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = " Location";
            // 
            // jdpFrom
            // 
            this.jdpFrom.AutoSize = true;
            this.jdpFrom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.jdpFrom.BackColor = System.Drawing.Color.White;
            this.jdpFrom.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.jdpFrom.ForeColor = System.Drawing.Color.Black;
            this.jdpFrom.Location = new System.Drawing.Point(18, 32);
            this.jdpFrom.Margin = new System.Windows.Forms.Padding(0);
            this.jdpFrom.MaxDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.MaxDate")));
            this.jdpFrom.MinDate = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.MinDate")));
            this.jdpFrom.Name = "jdpFrom";
            this.jdpFrom.Size = new System.Drawing.Size(245, 28);
            this.jdpFrom.TabIndex = 27;
            this.jdpFrom.Value = ((JewishCalendar.JewishDate)(resources.GetObject("jdpFrom.Value")));
            this.jdpFrom.ValueChanged += new System.EventHandler(this.jdpFrom_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 16);
            this.label11.TabIndex = 30;
            this.label11.Text = "Jewish Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 31;
            this.label2.Text = "Secular Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 94);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(242, 24);
            this.dateTimePicker1.TabIndex = 32;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(329, 16);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 21);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(690, 623);
            this.webBrowser1.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = " Latitude";
            // 
            // latDeg
            // 
            this.latDeg.Location = new System.Drawing.Point(18, 247);
            this.latDeg.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.latDeg.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.latDeg.Name = "latDeg";
            this.latDeg.Size = new System.Drawing.Size(72, 24);
            this.latDeg.TabIndex = 35;
            this.latDeg.ValueChanged += new System.EventHandler(this.latDeg_ValueChanged);
            // 
            // latMin
            // 
            this.latMin.Location = new System.Drawing.Point(96, 247);
            this.latMin.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.latMin.Name = "latMin";
            this.latMin.Size = new System.Drawing.Size(72, 24);
            this.latMin.TabIndex = 36;
            this.latMin.ValueChanged += new System.EventHandler(this.latMin_ValueChanged);
            // 
            // latSec
            // 
            this.latSec.DecimalPlaces = 2;
            this.latSec.Location = new System.Drawing.Point(174, 247);
            this.latSec.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.latSec.Name = "latSec";
            this.latSec.Size = new System.Drawing.Size(72, 24);
            this.latSec.TabIndex = 37;
            this.latSec.ValueChanged += new System.EventHandler(this.latSec_ValueChanged);
            // 
            // longSec
            // 
            this.longSec.DecimalPlaces = 2;
            this.longSec.Location = new System.Drawing.Point(174, 306);
            this.longSec.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.longSec.Name = "longSec";
            this.longSec.Size = new System.Drawing.Size(72, 24);
            this.longSec.TabIndex = 41;
            this.longSec.ValueChanged += new System.EventHandler(this.longSec_ValueChanged);
            // 
            // longMin
            // 
            this.longMin.Location = new System.Drawing.Point(96, 306);
            this.longMin.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.longMin.Name = "longMin";
            this.longMin.Size = new System.Drawing.Size(72, 24);
            this.longMin.TabIndex = 40;
            this.longMin.ValueChanged += new System.EventHandler(this.longMin_ValueChanged);
            // 
            // longDeg
            // 
            this.longDeg.Location = new System.Drawing.Point(18, 306);
            this.longDeg.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.longDeg.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.longDeg.Name = "longDeg";
            this.longDeg.Size = new System.Drawing.Size(72, 24);
            this.longDeg.TabIndex = 39;
            this.longDeg.ValueChanged += new System.EventHandler(this.longDeg_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 38;
            this.label4.Text = " Longitude";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 25);
            this.button1.TabIndex = 42;
            this.button1.Text = "Open Map";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nudElevation
            // 
            this.nudElevation.Location = new System.Drawing.Point(18, 365);
            this.nudElevation.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudElevation.Name = "nudElevation";
            this.nudElevation.Size = new System.Drawing.Size(106, 24);
            this.nudElevation.TabIndex = 44;
            this.nudElevation.Value = new decimal(new int[] {
            35000,
            0,
            0,
            0});
            this.nudElevation.ValueChanged += new System.EventHandler(this.nudElevation_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 345);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 16);
            this.label5.TabIndex = 43;
            this.label5.Text = "Altitude / Elevation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(130, 367);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 16);
            this.label6.TabIndex = 45;
            this.label6.Text = "feet";
            // 
            // nudTimeZone
            // 
            this.nudTimeZone.Location = new System.Drawing.Point(18, 422);
            this.nudTimeZone.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudTimeZone.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            -2147483648});
            this.nudTimeZone.Name = "nudTimeZone";
            this.nudTimeZone.Size = new System.Drawing.Size(106, 24);
            this.nudTimeZone.TabIndex = 47;
            this.nudTimeZone.ValueChanged += new System.EventHandler(this.nudTimeZone_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 402);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 16);
            this.label7.TabIndex = 46;
            this.label7.Text = "Time Zone - UTC Offset";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 652);
            this.Controls.Add(this.nudTimeZone);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudElevation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.longSec);
            this.Controls.Add(this.longMin);
            this.Controls.Add(this.longDeg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.latSec);
            this.Controls.Add(this.latMin);
            this.Controls.Add(this.latDeg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.jdpFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLocations);
            this.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Zmanim Anywhere";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.latDeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.latMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.latSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.longSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.longMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.longDeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudElevation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeZone)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbLocations;
        private System.Windows.Forms.Label label1;
        private JewishDatePicker.JewishDatePicker jdpFrom;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown latDeg;
        private System.Windows.Forms.NumericUpDown latMin;
        private System.Windows.Forms.NumericUpDown latSec;
        private System.Windows.Forms.NumericUpDown longSec;
        private System.Windows.Forms.NumericUpDown longMin;
        private System.Windows.Forms.NumericUpDown longDeg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown nudElevation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudTimeZone;
        private System.Windows.Forms.Label label7;
    }
}

