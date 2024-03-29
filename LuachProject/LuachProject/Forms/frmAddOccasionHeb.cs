﻿using JewishCalendar;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmAddOccasionHeb : Form
    {
        public enum CloseStyles
        {
            Fade,
            Slide,
            None
        }

        public event EventHandler<UserOccasion> OccasionWasChanged;

        private bool _loading;
        private Color _selectedForeColor = Color.Maroon;
        private Color _selectedBackColor = Color.Empty;

        public CloseStyles CloseStyle { get; set; }

        public JewishDate JewishDate
        {
            get
            {
                return this.jewishDatePicker1.Value;
            }
            set
            {
                this.jewishDatePicker1.Value = value;
            }
        }

        public DateTime SecularDate
        {
            get
            {
                return this.dateTimePicker1.Value;
            }
            set
            {
                this.dateTimePicker1.Value = value;
            }
        }

        public UserOccasion UserOccasion { get; private set; }

        public frmAddOccasionHeb()
        {
            InitializeComponent();
            this.jewishDatePicker1.MinDate = new JewishDate(3761, 11, 1, 13, new DateTime(1, 1, 13));
            this.jewishDatePicker1.MaxDate = new JewishDate(5999, 6, 1, 817656, new DateTime(2239, 9, 1));
        }

        public frmAddOccasionHeb(UserOccasion uoToEdit)
            : this()
        {
            this.UserOccasion = uoToEdit;
            this.Text = "עדכן - " + this.UserOccasion.Name;
        }

        private void frmAddOccasionHeb_Load(object sender, EventArgs e)
        {
            if (this.UserOccasion != null)
            {
                this.txtName.Text = this.UserOccasion.Name;
                this.txtNotes.Text = this.UserOccasion.Notes;
                this.txtName.TabIndex = 1000;
                switch (this.UserOccasion.UserOccasionType)
                {
                    case UserOccasionTypes.OneTime:
                        this.JewishDate = this.UserOccasion.JewishDate;
                        this.rbOneTime.Checked = true;
                        break;

                    case UserOccasionTypes.HebrewDateRecurringYearly:
                        this.JewishDate = this.UserOccasion.JewishDate;
                        this.rbJewishYearly.Checked = true;
                        break;

                    case UserOccasionTypes.HebrewDateRecurringMonthly:
                        this.JewishDate = this.UserOccasion.JewishDate;
                        this.rbJewishMonthly.Checked = true;
                        break;

                    case UserOccasionTypes.SecularDateRecurringYearly:
                        this.SecularDate = this.UserOccasion.SecularDate;
                        this.rbSecularYearly.Checked = true;
                        break;

                    case UserOccasionTypes.SecularDateRecurringMonthly:
                        this.SecularDate = this.UserOccasion.SecularDate;
                        this.rbSecularMonthly.Checked = true;
                        break;
                }
                this.toggleSendReminder.Checked = this.UserOccasion.SendEmailReminders;
                this.btnAdd.Text = "עדכן";
                this.btnDelete.Visible = true;
                this._selectedForeColor = this.UserOccasion.Color;
            }
            else
            {
                this.btnDelete.Visible = false;
                this.btnCancel.Location = this.btnDelete.Location;
            }

            this.txtName.ForeColor = this.btnColor.BackColor = this._selectedForeColor;

            //As each day can only have a single background color,
            //we get this occasions back color from any occasion in the list for the day.
            //What exactly is this "Day" is determined by the occasion type.
            this._selectedBackColor = (from o in UserOccasionColection.FromSettings(this.JewishDate)
                                       where o.BackColor != Color.Empty
                                       select o.BackColor).FirstOrDefault();
            this.btnBGColor.BackColor = this.txtName.BackColor = this._selectedBackColor;
            this.llClearBackColor.Visible = (this._selectedBackColor != Color.Empty);
            this.SetLabels();
            //Repaint the background
            this.panel2.Invalidate();
        }

        /// <summary>
        /// Draws the fancy gradient background colors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //I'm Not sure why 0, 0 doesn't work (not enough time to investigate and it works fine this way :))
            RectangleF r = new(-23, -22, this.Width + 30, this.Height + 22);
            GraphicsPath path = new();

            path.AddRectangle(r);
            e.Graphics.FillRectangle(new PathGradientBrush(path)
            {
                CenterColor = Color.FromArgb(50, this._selectedForeColor.R, this._selectedForeColor.G, this._selectedForeColor.B) /*Color.FromArgb(255, 192, 212, 238)*/,
                SurroundColors = new Color[] { this._selectedBackColor == Color.Empty ? Color.GhostWhite : this._selectedBackColor }
            }, r);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("מחק אירוע \"" +
                this.UserOccasion.Name + "\"?", "האם אתה בטוח?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Properties.Settings.Default.UserOccasions.Remove(this.UserOccasion);
                Properties.Settings.Default.Save();
                this.UserOccasion = null;

                if (OccasionWasChanged != null)
                {
                    OccasionWasChanged(this, this.UserOccasion);
                }

                this.Close();
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this._selectedForeColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.btnColor.BackColor = this.txtName.ForeColor = this._selectedForeColor = this.colorDialog1.Color;
                //Repaint the background
                this.panel2.Invalidate();
            }
        }

        private void btnBGColor_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this._selectedBackColor;
            if (this.colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.btnBGColor.BackColor = this.txtName.BackColor = this._selectedBackColor = this.colorDialog1.Color;
                this.llClearBackColor.Visible = (this._selectedBackColor != Color.Empty);
                //Repaint the background
                this.panel2.Invalidate();
            }
        }

        private void llClearBackColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.btnBGColor.BackColor = this.txtName.BackColor = this._selectedBackColor = Color.Empty;
            this.llClearBackColor.Visible = (this._selectedBackColor != Color.Empty);
            //Repaint the background
            this.panel2.Invalidate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                MessageBox.Show("חסר שם האירוע.",
                    "אירוע", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtName.Focus();
                return;
            }

            if (this.UserOccasion == null)
            {
                this.UserOccasion = new UserOccasion();
            }

            this.UserOccasion.Name = this.txtName.Text;
            this.UserOccasion.Notes = this.txtNotes.Text;
            this.UserOccasion.Color = this._selectedForeColor;

            if (rbOneTime.Checked)
            {
                this.UserOccasion.JewishDate = this.JewishDate;
                this.UserOccasion.UserOccasionType = UserOccasionTypes.OneTime;
            }
            else if (this.rbJewishYearly.Checked)
            {
                this.UserOccasion.JewishDate = this.JewishDate;
                this.UserOccasion.UserOccasionType = UserOccasionTypes.HebrewDateRecurringYearly;
            }
            else if (this.rbJewishMonthly.Checked)
            {
                this.UserOccasion.JewishDate = this.JewishDate;
                this.UserOccasion.UserOccasionType = UserOccasionTypes.HebrewDateRecurringMonthly;
            }
            else if (this.rbSecularYearly.Checked)
            {
                this.UserOccasion.SecularDate = this.SecularDate;
                this.UserOccasion.UserOccasionType = UserOccasionTypes.SecularDateRecurringYearly;
            }
            else if (this.rbSecularMonthly.Checked)
            {
                this.UserOccasion.SecularDate = this.SecularDate;
                this.UserOccasion.UserOccasionType = UserOccasionTypes.SecularDateRecurringMonthly;
            }

            this.UserOccasion.SendEmailReminders = this.toggleSendReminder.Checked;

            if (!Properties.Settings.Default.UserOccasions.Contains(this.UserOccasion))
            {
                Properties.Settings.Default.UserOccasions.Add(this.UserOccasion);
            }

            //Set the back-color for all occasions on that same day (Jewish/Secular according to occasion type)
            UserOccasionColection.FromSettings(this.JewishDate).ForEach(uo =>
                uo.BackColor = this._selectedBackColor);

            Properties.Settings.Default.Save();

            if (OccasionWasChanged != null)
            {
                OccasionWasChanged(this, this.UserOccasion);
            }

            this.Close();
        }

        private void jewishDatePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this._loading = true;
                this.SecularDate = this.JewishDate.GregorianDate;
                this.SetLabels();
                this._loading = false;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.Focus();
                this._loading = true;
                this.JewishDate = new JewishCalendar.JewishDate(this.SecularDate);
                this.SetLabels();
                this._loading = false;
                this.dateTimePicker1.Focus();
            }
        }

        private void frmAddOccasionEng_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.CloseStyle == CloseStyles.Fade)
                {
                    while (this.Opacity > 0)
                    {
                        this.Opacity -= 0.3;
                        this.Refresh();
                    }
                }
                else if (this.CloseStyle == CloseStyles.Slide)
                {
                    while (this.Left > -this.Width)
                    {
                        this.Location = new Point(this.Left - 50, this.Top);
                        this.Refresh();
                    }
                }
            }
        }

        private void SetLabels()
        {
            var jdMonth = this.JewishDate.Month;
            var jdDay = this.JewishDate.Day;
            var sdMonth = this.SecularDate.Month;
            var sdDay = this.SecularDate.Day;

            this.rbOneTime.Text = "אירוע חד פעמי בתאריך " +
                this.JewishDate.ToLongDateStringHeb() + "  (" +
                this.SecularDate.ToString("d", Program.HebrewCultureInfo) + ")";
            this.rbJewishYearly.Text = "אירוע שנתי בכל " + jdDay.ToNumberHeb() +
                " " + Utils.GetProperMonthNameHeb(this.JewishDate.Year, jdMonth);
            this.rbJewishMonthly.Text = "אירוע חודשי בכל " + jdDay.ToNumberHeb() +
                " לחודש העברי";
            this.rbSecularYearly.Text = "אירוע שנתי בכל " + sdDay.ToString() +
                " לחודש " + Program.HebrewCultureInfo.DateTimeFormat.MonthNames[sdMonth];
            this.rbSecularMonthly.Text = "אירוע חודשי בכל " + sdDay.ToString() +
                " לחודש הלועזי";
        }        
    }
}