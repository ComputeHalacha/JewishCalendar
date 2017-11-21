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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.choiceSwitcher6 = new ChoiceSwitcher();
            this.choiceSwitcher5 = new ChoiceSwitcher();
            this.choiceSwitcher4 = new ChoiceSwitcher();
            this.choiceSwitcher3 = new ChoiceSwitcher();
            this.choiceSwitcher2 = new ChoiceSwitcher();
            this.choiceNetz = new ChoiceSwitcher();
            this.choiceSwitcher1 = new ChoiceSwitcher();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(78, 457);
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
            this.cmbLocations.Location = new System.Drawing.Point(14, 36);
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
            this.label1.Location = new System.Drawing.Point(109, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = " Location / מיקום";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 104);
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
            this.label3.Location = new System.Drawing.Point(183, 104);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = " Sunrise / הנץ:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(107, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = " Show / הצג";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.choiceNetz);
            this.panel1.Location = new System.Drawing.Point(-1, 202);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 40);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.choiceSwitcher2);
            this.panel2.Location = new System.Drawing.Point(-1, 241);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 40);
            this.panel2.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = " Sunrise / הנץ:";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.choiceSwitcher3);
            this.panel3.Location = new System.Drawing.Point(-1, 280);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(306, 40);
            this.panel3.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = " Sunrise / הנץ:";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.choiceSwitcher4);
            this.panel4.Location = new System.Drawing.Point(-1, 319);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(306, 40);
            this.panel4.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = " Sunrise / הנץ:";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.choiceSwitcher5);
            this.panel5.Location = new System.Drawing.Point(-1, 353);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(306, 40);
            this.panel5.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = " Sunrise / הנץ:";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.choiceSwitcher6);
            this.panel6.Location = new System.Drawing.Point(-1, 392);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(306, 40);
            this.panel6.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 16);
            this.label10.TabIndex = 4;
            this.label10.Text = " Sunrise / הנץ:";
            // 
            // choiceSwitcher6
            // 
            this.choiceSwitcher6.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher6.ChoiceOneSelected = false;
            this.choiceSwitcher6.ChoiceOneText = "No / לא";
            this.choiceSwitcher6.ChoiceOneValue = null;
            this.choiceSwitcher6.ChoiceTwoSelected = true;
            this.choiceSwitcher6.ChoiceTwoText = "Yes / כן";
            this.choiceSwitcher6.ChoiceTwoValue = null;
            this.choiceSwitcher6.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher6.HighlightSelected = false;
            this.choiceSwitcher6.Location = new System.Drawing.Point(156, 7);
            this.choiceSwitcher6.Name = "choiceSwitcher6";
            this.choiceSwitcher6.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceSwitcher6.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher6.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher6.SelectedValue = null;
            this.choiceSwitcher6.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher6.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher6.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceSwitcher6.StrikeNotSelected = false;
            this.choiceSwitcher6.TabIndex = 8;
            this.choiceSwitcher6.Text = "choiceSwitcher2";
            // 
            // choiceSwitcher5
            // 
            this.choiceSwitcher5.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher5.ChoiceOneSelected = false;
            this.choiceSwitcher5.ChoiceOneText = "No / לא";
            this.choiceSwitcher5.ChoiceOneValue = null;
            this.choiceSwitcher5.ChoiceTwoSelected = true;
            this.choiceSwitcher5.ChoiceTwoText = "Yes / כן";
            this.choiceSwitcher5.ChoiceTwoValue = null;
            this.choiceSwitcher5.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher5.HighlightSelected = false;
            this.choiceSwitcher5.Location = new System.Drawing.Point(156, 7);
            this.choiceSwitcher5.Name = "choiceSwitcher5";
            this.choiceSwitcher5.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceSwitcher5.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher5.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher5.SelectedValue = null;
            this.choiceSwitcher5.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher5.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher5.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceSwitcher5.StrikeNotSelected = false;
            this.choiceSwitcher5.TabIndex = 8;
            this.choiceSwitcher5.Text = "choiceSwitcher2";
            // 
            // choiceSwitcher4
            // 
            this.choiceSwitcher4.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher4.ChoiceOneSelected = false;
            this.choiceSwitcher4.ChoiceOneText = "No / לא";
            this.choiceSwitcher4.ChoiceOneValue = null;
            this.choiceSwitcher4.ChoiceTwoSelected = true;
            this.choiceSwitcher4.ChoiceTwoText = "Yes / כן";
            this.choiceSwitcher4.ChoiceTwoValue = null;
            this.choiceSwitcher4.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher4.HighlightSelected = false;
            this.choiceSwitcher4.Location = new System.Drawing.Point(156, 7);
            this.choiceSwitcher4.Name = "choiceSwitcher4";
            this.choiceSwitcher4.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceSwitcher4.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher4.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher4.SelectedValue = null;
            this.choiceSwitcher4.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher4.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher4.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceSwitcher4.StrikeNotSelected = false;
            this.choiceSwitcher4.TabIndex = 8;
            this.choiceSwitcher4.Text = "choiceSwitcher2";
            // 
            // choiceSwitcher3
            // 
            this.choiceSwitcher3.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher3.ChoiceOneSelected = false;
            this.choiceSwitcher3.ChoiceOneText = "No / לא";
            this.choiceSwitcher3.ChoiceOneValue = null;
            this.choiceSwitcher3.ChoiceTwoSelected = true;
            this.choiceSwitcher3.ChoiceTwoText = "Yes / כן";
            this.choiceSwitcher3.ChoiceTwoValue = null;
            this.choiceSwitcher3.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher3.HighlightSelected = false;
            this.choiceSwitcher3.Location = new System.Drawing.Point(156, 7);
            this.choiceSwitcher3.Name = "choiceSwitcher3";
            this.choiceSwitcher3.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceSwitcher3.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher3.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher3.SelectedValue = null;
            this.choiceSwitcher3.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher3.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher3.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceSwitcher3.StrikeNotSelected = false;
            this.choiceSwitcher3.TabIndex = 8;
            this.choiceSwitcher3.Text = "choiceSwitcher2";
            // 
            // choiceSwitcher2
            // 
            this.choiceSwitcher2.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher2.ChoiceOneSelected = false;
            this.choiceSwitcher2.ChoiceOneText = "No / לא";
            this.choiceSwitcher2.ChoiceOneValue = null;
            this.choiceSwitcher2.ChoiceTwoSelected = true;
            this.choiceSwitcher2.ChoiceTwoText = "Yes / כן";
            this.choiceSwitcher2.ChoiceTwoValue = null;
            this.choiceSwitcher2.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher2.HighlightSelected = false;
            this.choiceSwitcher2.Location = new System.Drawing.Point(156, 7);
            this.choiceSwitcher2.Name = "choiceSwitcher2";
            this.choiceSwitcher2.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceSwitcher2.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher2.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher2.SelectedValue = null;
            this.choiceSwitcher2.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher2.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceSwitcher2.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceSwitcher2.StrikeNotSelected = false;
            this.choiceSwitcher2.TabIndex = 8;
            this.choiceSwitcher2.Text = "choiceSwitcher2";
            // 
            // choiceNetz
            // 
            this.choiceNetz.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceNetz.ChoiceOneSelected = false;
            this.choiceNetz.ChoiceOneText = "No / לא";
            this.choiceNetz.ChoiceOneValue = null;
            this.choiceNetz.ChoiceTwoSelected = true;
            this.choiceNetz.ChoiceTwoText = "Yes / כן";
            this.choiceNetz.ChoiceTwoValue = null;
            this.choiceNetz.ForeColor = System.Drawing.Color.Black;
            this.choiceNetz.HighlightSelected = false;
            this.choiceNetz.Location = new System.Drawing.Point(156, 7);
            this.choiceNetz.Name = "choiceNetz";
            this.choiceNetz.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.choiceNetz.SelectedForeColor = System.Drawing.Color.Black;
            this.choiceNetz.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceNetz.SelectedValue = null;
            this.choiceNetz.Size = new System.Drawing.Size(140, 25);
            this.choiceNetz.SlotBackColor = System.Drawing.Color.Gray;
            this.choiceNetz.SlotChoiceTwoBackColor = System.Drawing.Color.Teal;
            this.choiceNetz.StrikeNotSelected = false;
            this.choiceNetz.TabIndex = 8;
            this.choiceNetz.Text = "choiceSwitcher2";
            // 
            // choiceSwitcher1
            // 
            this.choiceSwitcher1.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
            this.choiceSwitcher1.ChoiceOneSelected = false;
            this.choiceSwitcher1.ChoiceOneText = "World";
            this.choiceSwitcher1.ChoiceOneValue = null;
            this.choiceSwitcher1.ChoiceTwoSelected = true;
            this.choiceSwitcher1.ChoiceTwoText = "ישראל";
            this.choiceSwitcher1.ChoiceTwoValue = null;
            this.choiceSwitcher1.ForeColor = System.Drawing.Color.Black;
            this.choiceSwitcher1.HighlightSelected = false;
            this.choiceSwitcher1.Location = new System.Drawing.Point(87, 64);
            this.choiceSwitcher1.Name = "choiceSwitcher1";
            this.choiceSwitcher1.SelectedFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.choiceSwitcher1.SelectedForeColor = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.SelectedHighlightColor = System.Drawing.Color.White;
            this.choiceSwitcher1.SelectedValue = null;
            this.choiceSwitcher1.Size = new System.Drawing.Size(140, 25);
            this.choiceSwitcher1.SlotBackColor = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.SlotChoiceTwoBackColor = System.Drawing.Color.RoyalBlue;
            this.choiceSwitcher1.StrikeNotSelected = false;
            this.choiceSwitcher1.TabIndex = 7;
            this.choiceSwitcher1.Text = "choiceSwitcher1";
            this.choiceSwitcher1.ChoiceSwitched += new System.EventHandler(this.choiceSwitcher1_ChoiceSwitched_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 505);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.choiceSwitcher1);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
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
        private ChoiceSwitcher choiceNetz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private ChoiceSwitcher choiceSwitcher2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private ChoiceSwitcher choiceSwitcher3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private ChoiceSwitcher choiceSwitcher4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label9;
        private ChoiceSwitcher choiceSwitcher5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label10;
        private ChoiceSwitcher choiceSwitcher6;
    }
}

