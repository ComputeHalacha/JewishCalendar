﻿using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmMonthlyHebrew : Form
    {
        #region Events
        public event EventHandler OccasionWasChanged;
        #endregion Events

        #region Private Fields
        private Location _currentLocation;
        private int _currentMonthLength;
        private int _currentMonthWeeks;
        private Font _dayFont;
        private Font _dayHeadersFont;
        private JewishDate _displayedJewishMonth;
        private bool _isFirstOpen = true;
        private bool _isResizing;
        private bool _loading = true;
        private Point _pnlMouseLocation;
        private Font _secularDayFont;
        private JewishDate _selectedDay;
        private List<SingleDateInfo> _singleDateInfoList = new();
        private JewishDate _todayJewishDate;
        private Font _userOccasionFont;
        private Font _userOccasionYearFont;
        private Font _zmanimFont;
        private readonly string _dailyInfoName = "frmDailyInfo";

        #endregion Private Fields

        #region Properties
        public JewishDate DisplayedJewishMonth
        {
            get => this._displayedJewishMonth;
            set
            {
                if (this._displayedJewishMonth == null ||
                    this._displayedJewishMonth.Year != value.Year ||
                    this._displayedJewishMonth.Month != value.Month)
                {
                    //Set _currentJewishDate to first of month
                    this._displayedJewishMonth = new JewishDate(value.Year, value.Month, 1, value.AbsoluteDate - (value.Day - 1));
                    if (this._displayedJewishMonth < this.jewishDatePicker1.MinDate)
                    {
                        this._displayedJewishMonth = this.jewishDatePicker1.MinDate;
                    }
                    else if (this._displayedJewishMonth > this.jewishDatePicker1.MaxDate)
                    {
                        this._displayedJewishMonth = this.jewishDatePicker1.MaxDate;
                    }
                    this._currentMonthLength = JewishDateCalculations.DaysInJewishMonth(this._displayedJewishMonth.Year, this._displayedJewishMonth.Month);
                    this._currentMonthWeeks = (int)this._displayedJewishMonth.DayOfWeek >= 5 && _currentMonthLength > 29 ? 6 : 5;
                    this.SetCaptionText();
                    this.SetShowSecondsLabel();
                    this.llSefirah.Visible = this._displayedJewishMonth.Month.In(1, 2);
                    this.pnlMain.Invalidate();
                }
            }
        }

        public Location LocationForZmanim
        {
            get => this._currentLocation;
            set
            {
                if (this._currentLocation == null || this._currentLocation.Name != value.Name)
                {
                    this._currentLocation = value;
                    Properties.Settings.Default.LocationName = value.Name;
                    Properties.Settings.Default.Save();
                    this.SetToday();
                    if (!this._loading)
                    {
                        //Location was changed, so we need to re-do the zmanim
                        this.pnlMain.Invalidate();
                        if (this.DailyPanelIsShowing)
                        {
                            this.DailyInfoForm.LocationForZmanim = value;
                        }
                    }
                }
            }
        }

        public JewishDate SelectedJewishDate
        {
            get => this._selectedDay;
            set
            {
                if (value != null)
                {
                    if (this._displayedJewishMonth.Month != value.Month || this._displayedJewishMonth.Year != value.Year)
                    {
                        this.DisplayedJewishMonth = value;
                    }
                    this.SelectSingleDay(value);
                }
            }
        }

        public DateTime SelectedDate
        {
            get => this._selectedDay.GregorianDate;
            set
            {
                if (value != null)
                {
                    var jd = new JewishDate(value);
                    if (this._displayedJewishMonth.Month != jd.Month || this._displayedJewishMonth.Year != jd.Year)
                    {
                        this.DisplayedJewishMonth = jd;
                    }
                    this.SelectSingleDay(jd);
                }
            }
        }

        public bool DailyPanelIsShowing => this.splitContainer1.Panel1.Controls.OfType<frmDailyInfoHeb>().Count() > 0;

        #endregion Properties

        #region Constructors
        public frmMonthlyHebrew()
        {
            Properties.Settings.Default.LastLanguage = "Hebrew";
            Properties.Settings.Default.Save();

            InitializeComponent();

            this.ResizeBegin += (s, e) => { this._isResizing = true; };
            this.ResizeEnd += (s, e) => { this._isResizing = false; this.pnlMain.Invalidate(); };

            this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
            this._dayFont = new Font("David", 20, FontStyle.Regular);
            this._zmanimFont = new Font(this.Font.FontFamily, 9, FontStyle.Regular);
            this._secularDayFont = new Font("Century Gothic", 8f);
            this._userOccasionFont = this._zmanimFont;
            this._userOccasionYearFont = new Font(this._userOccasionFont.FontFamily, (this._userOccasionFont.SizeInPoints * 0.6F));
            this.jewishDatePicker1.MinDate = new JewishDate(3761, 11, 1, 13, new DateTime(1, 1, 13));
            this.jewishDatePicker1.MaxDate = new JewishDate(5999, 6, 1, 817656, new DateTime(2239, 9, 1));
            this.jewishDatePicker1.DataBindings.Add("Value",
                this, "SelectedJewishDate", true, DataSourceUpdateMode.OnPropertyChanged, new JewishDate());
        }

        #endregion Constructors

        #region Event Handlers
        private void frmMonthlyHebrew_Load(object sender, EventArgs e)
        {
            Program.SetDoubleBuffered(this.pnlMain);
            this.printDocument1.DefaultPageSettings.Landscape = true;
            this.InitLocation();
            if (!this._currentLocation.IsInIsrael)
            {
                this.rbInChul.Checked = true;
            }
            if (this._todayJewishDate == null)
            {
                this.SetToday();
            }
            if (this._selectedDay == null)
            {
                this._selectedDay = this._todayJewishDate;
            }
            if (this._displayedJewishMonth == null)
            {
                this.DisplayedJewishMonth = this._todayJewishDate;
            }
            else
            {
                this.SetCaptionText();
            }

            this.llSefirah.Visible = this._displayedJewishMonth.Month.In(1, 2);

            this.EnableArrows();
            this._loading = false;
        }

        private void frmMonthlyHebrew_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMonthlyHebrew_Resize(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(this._displayedJewishMonth + 30);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(this._displayedJewishMonth - 28);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sdi = this._singleDateInfoList.FirstOrDefault(d => d.JewishDate == this._todayJewishDate);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
            }
            else
            {
                this._selectedDay = this._todayJewishDate;
                this.DisplayedJewishMonth = this._todayJewishDate;
            }

            this.EnableArrows();
        }

        private void btnPreviousYear_Click(object sender, EventArgs e)
        {
            int prevYear = this._displayedJewishMonth.Year - 1;
            int month = this._displayedJewishMonth.Month;
            if (month == 13)
            {
                //Last year will not be a leap year
                month = 12;
            }
            else if (month == 12 && JewishDateCalculations.IsJewishLeapYear(prevYear))
            {
                month = 13;
            }

            this.NavigateToMonth(
                new JewishDate(prevYear, month, this._displayedJewishMonth.Day));
        }

        private void btnNextYear_Click(object sender, EventArgs e)
        {
            int nextYear = this._displayedJewishMonth.Year + 1;
            int month = this._displayedJewishMonth.Month;
            if (month == 13)
            {
                //Next year will not be a leap year
                month = 12;
            }
            else if (month == 12 && JewishDateCalculations.IsJewishLeapYear(nextYear))
            {
                month = 13;
            }

            this.NavigateToMonth(
                new JewishDate(nextYear, month, this._displayedJewishMonth.Day));
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.LocationForZmanim = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmMonthlyEnglish()
            {
                DisplayedJewishMonth = this._displayedJewishMonth,
                SelectedJewishDate = this._selectedDay,
                StartPosition = this.StartPosition,
                Bounds = this.Bounds,
                WindowState = this.WindowState
            }.Show();
            this.Hide();
        }

        private void llSecularCalendar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmMonthlySecular()
            {
                CurrentDate = this._displayedJewishMonth.GregorianDate.Date,
                SelectedDate = this._selectedDay.GregorianDate.Date,
                StartPosition = this.StartPosition,
                Bounds = this.Bounds,
                WindowState = this.WindowState,
                DisplayHebrew = true
            }.Show();
            this.Hide();
        }

        private void llEmailReminders_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmReminderSettingsHeb().Show(this);
        }

        private void llShowDaily_LinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate == this._selectedDay ||
                                t.JewishDate == this._todayJewishDate ||
                                t.JewishDate == this._displayedJewishMonth);
            if (sdi != null)
            {
                this.ShowSingleDayInfo(sdi);
            }
        }

        private void llSefira_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process a = new();
            string omerAppName = "OmerReminder.exe";
            a.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, omerAppName);
            a.StartInfo.Arguments = "-location \"" + this._currentLocation.Name + "\"" + " -lang heb";
            a.EnableRaisingEvents = true;
            a.Exited += delegate
            {
                try
                {
                    string nusachString = File.ReadAllText("OmerNusach");
                    Nusach nusach = (Nusach)Enum.Parse(typeof(Nusach), nusachString);

                    if (nusach != Properties.Settings.Default.Nusach)
                    {
                        Properties.Settings.Default.Nusach = nusach;
                        Properties.Settings.Default.Save();

                        int jMonth = this._selectedDay.Month,
                        jDay = this._selectedDay.Day;
                        if ((jMonth == 1 && jDay > 15) || jMonth == 2 || (jMonth == 3 && jDay < 6))
                        {
                            if (this.DailyPanelIsShowing)
                            {
                                var f = this.DailyInfoForm;
                                f.Invoke(new Action(() =>
                                {
                                    f.ShowDateData();
                                    f.Invalidate();
                                }));
                            }
                        }
                    }
                }
                catch {/*Nu Nu**/ }

            };
            a.Start();
        }


        private void llSearchOccasion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmOccasionListHeb().Show(this);
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

                if (sdi != null)
                {
                    this.EditSelectedOccasion();
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi == null)
            {
                e.Cancel = true;
            }
        }

        private void goToDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GoToSelectedOccasionDate();
        }

        private void goToUpcomingOccurenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GoToSelectedOccasionUpcoming();
        }

        private void editThisOccasionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditSelectedOccasion();
        }

        private void deleteThisOccasionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null)
                {
                    if (MessageBox.Show("האם אתם בטוחים שברצונכם למחוק האירוע \"" + occ.Name + "\"?",
                            "לוח - מחק אירוע",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Properties.Settings.Default.UserOccasions.Remove(occ);
                        Properties.Settings.Default.Save();
                        this.Reload();
                    }
                }
            }
        }

        private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                if (this.DailyPanelIsShowing)
                {
                    var f = this.DailyInfoForm;
                    f.AddNewOccasion(new Point((int)(sdi.RectangleF.X - f.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                }
            }
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            this._pnlMouseLocation = e.Location;
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);
            if (sdi != null)
            {
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);
                if (occ != null)
                {
                    this.pnlMain.Cursor = Cursors.Hand;

                    string currTText = this.toolTip1.GetToolTip(this.pnlMain),
                        dateDiff = occ.GetAnniversaryString(sdi.JewishDate, true),
                        tot = occ.Name + " - לחץ לעדכן, לשנות או למחוק" +
                            ((!string.IsNullOrWhiteSpace(dateDiff)) ? "\r\n" + dateDiff : "") +
                            ((!string.IsNullOrWhiteSpace(occ.Notes)) ? "\r\nהערות:\r\n" + occ.Notes : "");

                    if (string.IsNullOrEmpty(currTText) || currTText != tot)
                    {
                        this.toolTip1.SetToolTip(this.pnlMain, tot);
                    }
                    return;
                }
            }
            this.pnlMain.Cursor = Cursors.Default;
            this.toolTip1.SetToolTip(this.pnlMain, null);
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (this._isResizing)
            {
                return;
            }

            this.pnlMain.SuspendLayout();
            var currDate = this._displayedJewishMonth;
            float dayWidth = (this.pnlMain.Width / 7f) + 1f;
            var eachDayHeight = (this.pnlMain.Height - 26f) / this._currentMonthWeeks;
            float currX = this.pnlMain.Right - dayWidth;
            float currY = 0f;

            for (int i = 0; i < 7; i++)
            {
                this.DrawDayHeader(e.Graphics, dayWidth, currX, currY, (DayOfWeek)i);
                currX -= dayWidth;
            }

            this._singleDateInfoList.Clear();

            currX = this.pnlMain.Right - dayWidth;
            currY = 25f;

            while (currDate.Month == this._displayedJewishMonth.Month)
            {
                currX = (this.pnlMain.Right - dayWidth) - (dayWidth * (float)currDate.DayOfWeek);
                var sdi = this.DrawSingleDay(e.Graphics, currDate, dayWidth, eachDayHeight, currX, currY);

                if ((!this._isFirstOpen) && (!this.splitContainer1.Panel1Collapsed) && currDate == this._selectedDay)
                {
                    this.ShowSingleDayInfo(sdi);
                }

                currDate += 1;
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currY += eachDayHeight;
                    currX = this.pnlMain.Right - dayWidth;
                }
            }

            if (this._isFirstOpen)
            {
                this._isFirstOpen = false;
                var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate == this._selectedDay ||
                    t.JewishDate == this._todayJewishDate);
                if (sdi != null)
                {
                    this.ShowSingleDayInfo(sdi);
                }
                this.pnlMain.Invalidate();
            }
            this.pnlMain.ResumeLayout();
        }

        private void rbInIsrael_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.SetLocationDataSource();
            }
        }

        private void splitContainer1_Panel2_ClientSizeChanged(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void splitContainer2_KeyDown(object sender, KeyEventArgs e)
        {
            if (this._selectedDay == null || this.cmbLocation.ContainsFocus)
            {
                return;
            }

            switch (e.KeyData)
            {
                case Keys.Right:
                    this.NavigateToDay(this._selectedDay - 1);
                    break;

                case Keys.Left:
                    this.NavigateToDay(this._selectedDay + 1);
                    break;

                case Keys.Up:
                    this.NavigateToDay(this._selectedDay - 7);
                    break;

                case Keys.Down:
                    this.NavigateToDay(this._selectedDay + 7);
                    break;

                case Keys.Enter:
                    if (this.DailyPanelIsShowing)
                    {
                        var f = this.DailyInfoForm;
                        f.AddNewOccasion(null);
                    }
                    break;
            }
        }

        private void llShowSeconds_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var showing = Properties.Settings.Default.ShowSeconds;
            Properties.Settings.Default.ShowSeconds = !showing;
            Properties.Settings.Default.Save();
            this.SetShowSecondsLabel();
            this.Reload();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            RectangleF bounds = e.PageSettings.PrintableArea;
            using var bmp = new Bitmap(pnlMain.ClientSize.Width, pnlMain.ClientSize.Height);
            pnlMain.DrawToBitmap(bmp, pnlMain.ClientRectangle);
            e.Graphics.DrawImage(bmp,
                bounds.Left,
                bounds.Top + this.lblMonthName.Height,
                bounds.Height - 50,
                bounds.Width - (this.lblMonthName.Height) - 30);
            e.Graphics.DrawString(this.lblMonthName.Text,
                this.lblMonthName.Font,
                new SolidBrush(this.lblMonthName.ForeColor),
                (bounds.Width / 2),
                bounds.Top);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = printDialog1.ShowDialog();
            if (result is DialogResult.Yes or DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        #endregion Event Handlers

        #region Private Functions
        private void ClearSelectedDay()
        {
            if (this._selectedDay != null)
            {
                var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate == this._selectedDay);
                this._selectedDay = null;
                if (sdi != null && this._displayedJewishMonth.Year == sdi.JewishDate.Year && this._displayedJewishMonth.Month == sdi.JewishDate.Month)
                {
                    this.RedrawSingleDay(sdi);
                }
            }
        }

        private void DrawDayHeader(Graphics g, float dayWidth, float currX, float currY, DayOfWeek dow)
        {
            var rect = new RectangleF(currX, currY, dayWidth, 25f);
            var text = Utils.JewishDOWNames[(int)dow];

            if (dow == DayOfWeek.Sunday)
            {
                rect.Width = (this.pnlMain.Width - currX) - 1f;
            }

            g.FillRectangle(Program.DayHeadersBGBrush, rect);
            g.DrawRectangle(Program.DayCellBorderPen, rect.X, rect.Y, rect.Width, rect.Height);
            TextRenderer.DrawText(g, text, this._dayHeadersFont, Rectangle.Truncate(rect), Program.DayHeadersTextColor, Program.TextFormatFlags);
        }

        private SingleDateInfo DrawSingleDay(Graphics g, JewishDate currDate, float width, float height, float currX, float currY)
        {
            var zmanim = new Zmanim(currDate, this._currentLocation);
            var rect = new RectangleF(currX, currY, width, height);
            var text = currDate.Day.ToNumberHeb().Replace("'", "");
            var holidays = Zmanim.GetHolidays(currDate, this._currentLocation.IsInIsrael);
            var occasions = UserOccasionColection.FromSettings(currDate);
            SingleDateInfo sdi = new(currDate, new RectangleF(rect.Location, rect.Size));

            this._singleDateInfoList.Add(sdi);

            if (this._isFirstOpen)
            {
                //We will be repainting anyway...
                return sdi;
            }
            string textZmanim = "";

            if (currDate.DayOfWeek == DayOfWeek.Saturday)
            {
                bool noSedra = false;

                foreach (SpecialDay sd in holidays)
                {
                    if (sd.DayType == SpecialDayTypes.Shabbos)
                    {
                        holidays.Remove(sd);
                        break;
                    }
                }

                foreach (SpecialDay sd in holidays)
                {
                    if (sd.DayType.IsSpecialDayType(SpecialDayTypes.MajorYomTov) || sd.DayType.IsSpecialDayType(SpecialDayTypes.CholHamoed))
                    {
                        noSedra = true;
                    }
                    if (sd.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = currDate + 12;
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        sd.NameHebrew += "\nמולד: " + molad.ToStringHeb(zmanim.GetShkia());
                        break;
                    }
                }

                if (noSedra)
                {
                    textZmanim += Zmanim.GetHolidaysText(holidays, "\n", true);
                }
                else
                {
                    textZmanim = string.Join(" - ", Sedra.GetSedra(currDate, this._currentLocation.IsInIsrael).Select(i =>
                       i.nameHebrew)) + "\n" + Zmanim.GetHolidaysText(holidays, "\n", true);
                }

                g.FillRectangle(Program.ShabbosBrush, rect);
            }
            else
            {
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    width = (this.pnlMain.Width - currX) - 1f;
                    rect.Width = width;
                }
                if (holidays.Count > 0)
                {
                    var hlist = holidays.Cast<SpecialDay>();
                    if (hlist.Any(h => h.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting)))
                    {
                        textZmanim += "הדלק\"נ: " +
                            (zmanim.GetShkia() - this._currentLocation.CandleLighting).ToString24H() + "\n";
                    }
                    if (hlist.Any(h => (h.DayType.IsSpecialDayType(SpecialDayTypes.MajorYomTov))))
                    {
                        g.FillRectangle(Program.MajorYomtovBrush, rect);
                    }
                    else if (hlist.Any(h => (h.DayType.IsSpecialDayType(SpecialDayTypes.MinorYomtov))))
                    {
                        g.FillRectangle(Program.MinorYomtovBrush, rect);
                    }

                    textZmanim += Zmanim.GetHolidaysText(holidays, "\n", true);
                }
            }
            this._singleDateInfoList.Add(new SingleDateInfo(currDate, rect));

            if (occasions.Any(bc => bc.BackColor != Color.Empty))
            {
                g.FillRectangle(new SolidBrush(occasions.First(bc => bc.BackColor != Color.Empty).BackColor), rect);
            }
            if (this._selectedDay != null && currDate == this._selectedDay)
            {
                g.FillRectangle(Program.SelectedDayBackgroundBrush, rect);
            }

            g.DrawRectangle(Program.DayCellBorderPen, rect.X, rect.Y, rect.Width, rect.Height);

            if (currDate == this._todayJewishDate)
            {
                var eh = rect.Height / 3.3f;
                var ew = rect.Width / 3.3f;
                g.FillClosedCurve(Program.DayHeadersBGBrush, new PointF[]
                {
                    new(rect.X + ew, rect.Y + eh),
                    new(rect.X + (rect.Width - ew), rect.Y + eh),
                    new(rect.X + (rect.Width - ew), rect.Y + (rect.Height - eh)),
                    new(rect.X + ew, rect.Y + (rect.Height - eh))
                }, System.Drawing.Drawing2D.FillMode.Alternate, 3f);
            }

            //accumulative height of all text rectangles - keeps track of current Y position in box.
            float offsetTop = 0f;

            //Padding top - varies according to what needs to be displayed beneath it
            rect.Y = currY + (rect.Height / (occasions.Count + holidays.Count > 1 ? 20 : 10));
            rect.X = currX + (width / 2);
            rect.Width = (width / 2);
            rect.Height = TextRenderer.MeasureText(g, text, this._dayFont, rect.Size.ToSize(), Program.TextFormatFlags).Height;
            offsetTop += rect.Height;

            TextRenderer.DrawText(g, text, this._dayFont, Rectangle.Truncate(rect), Program.DayTextColor, Program.TextFormatFlags);

            rect.X = currX;

            TextRenderer.DrawText(g, currDate.GregorianDate.Day.ToString(), this._secularDayFont, Rectangle.Truncate(rect), Program.SecularDayColor, Program.TextFormatFlags);

            rect.Width = width;

            offsetTop += rect.Height / (holidays.Count > 1 ? 5 : 3);

            foreach (var o in occasions)
            {
                //Get the text size for this occasions label.
                var mainTextSize = TextRenderer.MeasureText(g, o.Name, this._userOccasionFont, rect.Size.ToSize(), Program.TextFormatFlags);
                var textSize = mainTextSize;
                var numberTextWidth = 0;
                int number = 0;
                if (Properties.Settings.Default.ShowYearBadge &&
                    o.UserOccasionType.In(UserOccasionTypes.HebrewDateRecurringYearly, UserOccasionTypes.SecularDateRecurringYearly))
                {
                    number = o.GetNumberAnniversary(currDate);
                }

                if (number > 0)
                {
                    numberTextWidth = TextRenderer.MeasureText(g, number.ToString(), this._userOccasionYearFont).Width;
                    textSize.Width += numberTextWidth;
                }

                //Move the Y position down to empty space.
                rect.Y = currY + offsetTop;
                rect.Height = textSize.Height;
                //Save the exact position of the occasion label so when the user clicks on it afterwards, we can open the occasion for editing.
                //Note: the occasion labels are centered in the days box, so we need to find the beginning of the centered text.
                o.Rectangle = new RectangleF(rect.X + ((rect.Width / 2) - (textSize.Width / 2)), rect.Y, textSize.Width, textSize.Height);

                if (Properties.Settings.Default.ShowYearBadge && number > 0 && mainTextSize.Width <= (rect.Width * 0.666))
                {
                    var numRectangle = new Rectangle(Point.Truncate(new(rect.X + (mainTextSize.Width * 0.666F), rect.Y)), Size.Truncate(rect.Size));
                    TextRenderer.DrawText(g, number.ToString(), this._userOccasionYearFont, numRectangle,
                        (o.BackColor.Color == o.Color.Color ? Color.White : o.BackColor),
                        (o.BackColor.Color == o.Color.Color ? Color.Black : o.Color));

                }

                TextRenderer.DrawText(g, o.Name, this._userOccasionFont, Rectangle.Truncate(rect), o.Color, Program.TextFormatFlags);
                offsetTop += rect.Height;
            }

            if (!string.IsNullOrWhiteSpace(textZmanim))
            {
                rect.Y = currY + offsetTop;

                rect.Height = height - offsetTop;
                TextRenderer.DrawText(
                    g,
                    textZmanim,
                    this._zmanimFont,
                    Rectangle.Truncate(rect),
                    Program.ZmanimColor,
                    Program.TextFormatFlags);
            }
            return sdi;
        }

        /// <summary>
        /// To enable arrow navigation, the focus must be in the split-container.
        /// </summary>
        private void EnableArrows()
        {
            //For some odd reason, the focus only works if we go traveling around the form a bit.
            this.llSecularCalendar.Focus();

            this.splitContainer2.Focus();
        }

        private SingleDateInfo GetSingleDateInfoFromLocation(Point location)
        {
            return this._singleDateInfoList.FirstOrDefault(t =>
                             t.RectangleF.Left < location.X &&
                             t.RectangleF.Right > location.X &&
                             t.RectangleF.Top < location.Y &&
                             t.RectangleF.Bottom > location.Y);
        }

        private UserOccasion GetUserOccasionFromLocation(Point location, SingleDateInfo sdi)
        {
            return sdi.UserOccasions.FirstOrDefault(t =>
                             t.Rectangle.Left < location.X &&
                             t.Rectangle.Right > location.X &&
                             t.Rectangle.Top < location.Y &&
                             t.Rectangle.Bottom > location.Y);
        }

        private void NavigateToDay(JewishDate jd)
        {
            //Go to the correct month
            this.DisplayedJewishMonth = jd;
            this.SelectSingleDay(jd);
        }

        private void NavigateToMonth(JewishDate jd)
        {
            int day = 0;
            if (this._selectedDay != null)
            {
                day = this._selectedDay.Day;
            }

            this.DisplayedJewishMonth = jd;

            if (day > 0)
            {
                if (day == 30 && JewishDateCalculations.DaysInJewishMonth(
                    this._displayedJewishMonth.Year, this._displayedJewishMonth.Month) == 29)
                {
                    day = 29;
                }

                this.SelectSingleDay(new JewishDate(this._displayedJewishMonth.Year,
                    this._displayedJewishMonth.Month, day));
            }

            this.EnableArrows();
        }

        private void RedrawSingleDay(SingleDateInfo sdi)
        {
            if (sdi == null)
            {
                return;
            }
            using var g = this.pnlMain.CreateGraphics();
            var rect = sdi.RectangleF;
            g.Clip = new Region(rect);
            g.Clear(this.pnlMain.BackColor);
            this.DrawSingleDay(g, sdi.JewishDate, rect.Width, rect.Height, rect.X, rect.Y);
        }

        private void SelectSingleDay(JewishDate jd)
        {
            if (this._selectedDay != jd)
            {
                if (this._displayedJewishMonth.Year == jd.Year && this._displayedJewishMonth.Month == jd.Month)
                {
                    var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate == jd);
                    if (sdi != null)
                    {
                        this.SelectSingleDay(sdi);
                    }
                }
                this.jewishDatePicker1.Value = jd;
                this._selectedDay = jd;
            }
        }

        private void SelectSingleDay(SingleDateInfo sdi)
        {
            if (sdi == null)
            {
                return;
            }

            this.ClearSelectedDay();
            if (this._selectedDay != sdi.JewishDate)
            {
                using var g = this.pnlMain.CreateGraphics();
                g.Clip = new Region(sdi.RectangleF);
                g.FillRectangle(Program.SelectedDayBackgroundBrush, sdi.RectangleF);
                this._selectedDay = sdi.JewishDate;
            }

            this.jewishDatePicker1.Value = sdi.JewishDate;
            if (this.DailyPanelIsShowing)
            {
                this.ShowSingleDayInfo(sdi);
            }
        }

        private void SetCaptionText()
        {
            this.SuspendLayout();
            string caption = Utils.GetProperMonthNameHeb(this._displayedJewishMonth.Year, this._displayedJewishMonth.Month) + " " +
                this._displayedJewishMonth.Year.ToNumberHeb() + "  |  ";
            DateTime firstDayGMonth = this._displayedJewishMonth.GregorianDate;
            DateTime lastDayGMonth = (this._displayedJewishMonth + (this._currentMonthLength - 1)).GregorianDate;

            if (firstDayGMonth.Month == lastDayGMonth.Month)
            {
                caption += firstDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + firstDayGMonth.Month.ToString() + ") " + firstDayGMonth.Year.ToString();
            }
            else if (firstDayGMonth.Year == lastDayGMonth.Year)
            {
                caption += firstDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + firstDayGMonth.Month.ToString() + ") - " +
                    lastDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + lastDayGMonth.Month.ToString() + ") " + lastDayGMonth.Year.ToString();
                ;
            }
            else
            {
                caption += firstDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + firstDayGMonth.Month.ToString() + ") " + firstDayGMonth.Year.ToString() + " - " +
                    lastDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + lastDayGMonth.Month.ToString() + ") " + lastDayGMonth.Year.ToString();
            }

            this.lblMonthName.Text = caption;
            this.Text = "לוח -  " + caption +
                "       [גירסה " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "]";
            this.ResumeLayout();
        }

        private void SetLocationDataSource()
        {
            this.Cursor = Cursors.WaitCursor;
            bool wasLoading = this._loading;
            this._loading = true;
            bool inIsrael = this.rbInIsrael.Checked;
            var list = Program.LocationsList.Where(l => l.IsInIsrael == inIsrael).ToList();
            this.cmbLocation.DataSource = null;
            this.cmbLocation.DataSource = list;
            this.cmbLocation.DisplayMember = "NameHebrew";

            var name = Properties.Settings.Default.LocationName;
            var i = list.FirstOrDefault(l => l.Name == name);
            if (i != null)
            {
                this.cmbLocation.SelectedItem = i;
            }

            this._loading = wasLoading;
            this.Cursor = Cursors.Default;

            this.LocationForZmanim = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
        }

        private void InitLocation()
        {
            var name = Properties.Settings.Default.LocationName;
            var i = Program.LocationsList.FirstOrDefault(l => l.Name == name);
            if (i != null)
            {
                this.rbInIsrael.Checked = i.IsInIsrael;
                this.rbInChul.Checked = !i.IsInIsrael;

            }
            this.SetLocationDataSource();
        }

        private void ShowSingleDayInfo(SingleDateInfo sdi)
        {
            if (sdi == null)
            {
                return;
            }

            var zmanim = new Zmanim(sdi.JewishDate, this._currentLocation);
            frmDailyInfoHeb f;
            if (!this.DailyPanelIsShowing)
            {
                f = new frmDailyInfoHeb((sdi.JewishDate == this._todayJewishDate ? this._todayJewishDate : sdi.JewishDate),
                    this._currentLocation);
                f.TopLevel = false;
                f.Parent = this;
                f.Name = this._dailyInfoName;
                f.OccasionWasChanged += delegate (object sender, JewishDate jd)
                {
                    var sd = this._singleDateInfoList.FirstOrDefault(d => d.JewishDate == jd);
                    if (sd != null)
                    {
                        sd.UpdateOccasions();
                        this.RedrawSingleDay(sd);
                    }
                    this.OccasionWasChanged?.Invoke(f, new EventArgs());
                };
                f.FormClosed += delegate
                {
                    this.splitContainer1.Panel1Collapsed = true;
                    this.llShowDaily.Visible = true;
                };
                f.Dock = DockStyle.Fill;
                this.splitContainer1.Panel1.Controls.Add(f);
                f.Show();
                this.llShowDaily.Visible = false;
            }
            else
            {
                f = this.DailyInfoForm;
                f.JewishDate = (sdi.JewishDate == this._todayJewishDate ? this._todayJewishDate : sdi.JewishDate);
            }
            if (this.splitContainer1.Panel1Collapsed)
            {
                this.splitContainer1.Panel1Collapsed = false;
            }
        }

        private frmDailyInfoHeb DailyInfoForm =>
            this.splitContainer1.Panel1.Controls.Find(this._dailyInfoName, false).First() as frmDailyInfoHeb;


        private void SetToday()
        {
            if (Program.WeAreHere(this._currentLocation))
            {
                this._todayJewishDate = new JewishDate(this._currentLocation);
            }
            else
            {
                this._todayJewishDate = new JewishDate();
            }
        }

        private void SetShowSecondsLabel()
        {
            this.llShowSeconds.Text = (Properties.Settings.Default.ShowSeconds ?
                "הסתר" : "הצג") +
                " שניות";
        }

        private void EditSelectedOccasion()
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null && this.DailyPanelIsShowing)
                {
                    var f = this.DailyInfoForm;
                    f.EditOccasion(occ, new Point((int)(sdi.RectangleF.X - f.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                }
            }

            this.EnableArrows();
        }

        private void GoToSelectedOccasionDate()
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null)
                {
                    this.SelectedDate =
                    (occ.JewishDate != null ? occ.JewishDate.GregorianDate : occ.SecularDate);
                }
            }
        }

        private void GoToSelectedOccasionUpcoming()
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null)
                {
                    this.SelectedDate = occ.GetUpcomingOccurence();
                }
            }
        }
        #endregion Private Functions               

        #region Public Functions
        public void EditOccasion(UserOccasion uo)
        {
            this.SelectedJewishDate = uo.JewishDate;
            var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate == this._selectedDay);
            if (uo != null && this.DailyPanelIsShowing)
            {
                var f = this.DailyInfoForm;
                f.EditOccasion(uo, new Point((int)(sdi.RectangleF.X - f.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
            }
        }

        public void Reload()
        {
            this.pnlMain.Invalidate();
            if (this.DailyPanelIsShowing)
            {
                var f = this.DailyInfoForm;
                f.ShowDateData();
            }
        }

        #endregion

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}