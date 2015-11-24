namespace JewishDatePicker
{
    partial class JewishDatePicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbJYear = new System.Windows.Forms.ComboBox();
            this.cmbJDay = new System.Windows.Forms.ComboBox();
            this.cmbJMonth = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbJYear
            // 
            this.cmbJYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbJYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbJYear.BackColor = System.Drawing.Color.White;
            this.cmbJYear.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbJYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbJYear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbJYear.Location = new System.Drawing.Point(1, 2);
            this.cmbJYear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbJYear.Name = "cmbJYear";
            this.cmbJYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbJYear.Size = new System.Drawing.Size(92, 21);
            this.cmbJYear.TabIndex = 2;
            // 
            // cmbJDay
            // 
            this.cmbJDay.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbJDay.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbJDay.BackColor = System.Drawing.Color.White;
            this.cmbJDay.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbJDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbJDay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbJDay.Location = new System.Drawing.Point(198, 2);
            this.cmbJDay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbJDay.Name = "cmbJDay";
            this.cmbJDay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbJDay.Size = new System.Drawing.Size(72, 21);
            this.cmbJDay.TabIndex = 0;
            // 
            // cmbJMonth
            // 
            this.cmbJMonth.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbJMonth.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbJMonth.BackColor = System.Drawing.Color.White;
            this.cmbJMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbJMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbJMonth.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbJMonth.Location = new System.Drawing.Point(97, 2);
            this.cmbJMonth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbJMonth.Name = "cmbJMonth";
            this.cmbJMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbJMonth.Size = new System.Drawing.Size(97, 21);
            this.cmbJMonth.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmbJDay);
            this.flowLayoutPanel1.Controls.Add(this.cmbJMonth);
            this.flowLayoutPanel1.Controls.Add(this.cmbJYear);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(272, 23);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // JewishDatePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "JewishDatePicker";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(272, 23);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbJYear;
        private System.Windows.Forms.ComboBox cmbJDay;
        private System.Windows.Forms.ComboBox cmbJMonth;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
