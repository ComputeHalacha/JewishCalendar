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
            this.button1 = new System.Windows.Forms.Button();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.chooserControl1 = new ChooserControl();
            this.choiceSwitcher1 = new ChoiceSwitcher();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate / יצר";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbLocations
            // 
            this.cmbLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FormattingEnabled = true;
            this.cmbLocations.Location = new System.Drawing.Point(15, 36);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbLocations.Size = new System.Drawing.Size(276, 24);
            this.cmbLocations.Sorted = true;
            this.cmbLocations.TabIndex = 1;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.cmbLocations_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = " Location / מיקום";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = " Month / חודש";
            // 
            // cmbMonth
            // 
            this.cmbMonth.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMonth.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(15, 125);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbMonth.Size = new System.Drawing.Size(133, 24);
            this.cmbMonth.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = " Year / שנה";
            // 
            // cmbYear
            // 
            this.cmbYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(154, 125);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbYear.Size = new System.Drawing.Size(136, 24);
            this.cmbYear.TabIndex = 5;
            // 
            // chooserControl1
            // 
            this.chooserControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chooserControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chooserControl1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chooserControl1.LeftSelected = true;
            this.chooserControl1.Location = new System.Drawing.Point(15, 63);
            this.chooserControl1.Name = "chooserControl1";
            this.chooserControl1.RightSelected = false;
            this.chooserControl1.Size = new System.Drawing.Size(275, 24);
            this.chooserControl1.TabIndex = 7;
            this.chooserControl1.TextLeft = "World";
            this.chooserControl1.TextRight = "ישראל";
            this.chooserControl1.ToggleButtonBackColor = System.Drawing.SystemColors.Control;
            this.chooserControl1.ToggleButtonForeColor = System.Drawing.SystemColors.ControlDark;
            this.chooserControl1.ChoiceChanged += new ChoiceChangedEventHandler(this.chooserControl1_ChoiceChanged);
            // 
            // choiceSwitcher1
            // 
            this.choiceSwitcher1.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
            this.choiceSwitcher1.ChoiceOneSelected = true;
            this.choiceSwitcher1.ChoiceOneText = "First Choice";
            this.choiceSwitcher1.ChoiceOneValue = null;
            this.choiceSwitcher1.ChoiceTwoSelected = false;
            this.choiceSwitcher1.ChoiceTwoText = "Second Choice";
            this.choiceSwitcher1.ChoiceTwoValue = null;
            this.choiceSwitcher1.ForeColor = System.Drawing.Color.DimGray;
            this.choiceSwitcher1.Location = new System.Drawing.Point(269, 192);
            this.choiceSwitcher1.Name = "choiceSwitcher1";
            this.choiceSwitcher1.SelectedValue = null;
            this.choiceSwitcher1.Size = new System.Drawing.Size(250, 25);
            this.choiceSwitcher1.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 446);
            this.Controls.Add(this.choiceSwitcher1);
            this.Controls.Add(this.chooserControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLocations);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Create Calendar / יצר רשימה";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private ChooserControl chooserControl1;
        private ChoiceSwitcher choiceSwitcher1;
    }
}

