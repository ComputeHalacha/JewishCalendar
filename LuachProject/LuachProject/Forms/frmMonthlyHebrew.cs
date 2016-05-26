using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private bool _loading;
        private Point _pnlMouseLocation;
        private Font _secularDayFont;
        private JewishDate _selectedDay;
        private List<SingleDateInfo> _singleDateInfoList = new List<SingleDateInfo>();
        private JewishDate _todayJewishDate;
        private Font _userOccasionFont;
        private Font _zmanimFont;

        #endregion Private Fields

        #region Properties

        public JewishDate DisplayedJewishMonth
        {
            get
            {
                return this._displayedJewishMonth;
            }
            set
            {
                if (this._displayedJewishMonth == null || this._displayedJewishMonth.Year != value.Year || this._displayedJewishMonth.Month != value.Month)
                {
                    //Set _currentJewishDate to first of month
                    this._displayedJewishMonth = value - (value.Day - 1);
                    this._currentMonthLength = JewishDate.DaysInJewishMonth(this._displayedJewishMonth.Year, this._displayedJewishMonth.Month);
                    this._currentMonthWeeks = (int)this._displayedJewishMonth.DayOfWeek >= 5 && _currentMonthLength > 29 ? 6 : 5;
                    this.SetCaptionText();
                    this.llSefirah.Visible = this._displayedJewishMonth.Month.In(1, 2);
                    this.pnlMain.Invalidate();
                }
            }
        }

        public Location LocationForZmanim
        {
            get
            {
                return this._currentLocation;
            }
            set
            {
                this._currentLocation = value;
                if (!this._loading)
                {
                    //Location was changed, so we need to re-do the zmanim
                    this.pnlMain.Invalidate();
                    Properties.Settings.Default.LocationName = value.Name;
                    Properties.Settings.Default.Save();

                    if (this.DailyPanelIsShowing)
                    {
                        ((frmDailyInfoHeb)this.splitContainer1.Panel1.Controls[0]).LocationForZmanim = value;
                    }
                }
            }
        }

        public JewishDate SelectedJewishDate
        {
            get
            {
                return this._selectedDay;
            }
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
            get
            {
                return this._selectedDay.GregorianDate;
            }
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

        public bool DailyPanelIsShowing
        {
            get
            {
                return this.splitContainer1.Panel1.Controls.OfType<frmDailyInfoHeb>().Count() > 0;
            }
        }

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
            this.jewishDatePicker1.DataBindings.Add("Value",
                this, "SelectedJewishDate", true, DataSourceUpdateMode.OnPropertyChanged, new JewishDate());
        }

        #endregion Constructors

        #region Event Handlers

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

        private void button4_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(new JewishDate(
                this._displayedJewishMonth.Year - 1,
                (this._displayedJewishMonth.Month == 13 ? 12 : this._displayedJewishMonth.Month),
                this._displayedJewishMonth.Day));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(new JewishDate(
                this._displayedJewishMonth.Year + 1,
                (this._displayedJewishMonth.Month == 13 ? 12 : this._displayedJewishMonth.Month),
                this._displayedJewishMonth.Day));
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.LocationForZmanim = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            }
        }

        private void frmMonthlyHebrew_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMonthlyHebrew_Load(object sender, EventArgs e)
        {
            this.SetLocationDataSource();
            if (!this._currentLocation.IsInIsrael)
            {
                this._loading = true;
                this.rbInChul.Checked = true;
                this._loading = false;
            }
            if (this._todayJewishDate == null)
            {
                this._todayJewishDate = new JewishCalendar.JewishDate(this._currentLocation);
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
        }

        private void frmMonthlyHebrew_Resize(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
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
            Process a = new Process();
            a.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, @"OmerReminder.exe");
            a.StartInfo.Arguments = "-location \"" + this._currentLocation.Name + "\"" + " -lang heb";
            a.Start();
        }

        private void llSearchOccasion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmOccasionListHeb().Show(this);
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null && this.DailyPanelIsShowing)
                {
                    var f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
                    f.EditOccasion(occ, new Point((int)(sdi.RectangleF.X - f.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                }
            }

            this.EnableArrows();
        }

        private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                if (this.DailyPanelIsShowing)
                {
                    var f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
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
                    t.JewishDate == this._todayJewishDate ||
                    t.JewishDate == this._displayedJewishMonth);
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
                return;
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
                        var f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
                        f.AddNewOccasion(null);
                    }
                    break;
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
            g.DrawString(text, this._dayHeadersFont, Program.DayHeadersTextBrush, rect, Program.StringFormat);
        }

        private SingleDateInfo DrawSingleDay(Graphics g, JewishDate currDate, float width, float height, float currX, float currY)
        {
            var zmanim = new Zmanim(currDate, this._currentLocation);
            var rect = new RectangleF(currX, currY, width, height);
            var text = currDate.Day.ToNumberHeb().Replace("'", "");
            var holidays = Zmanim.GetHolidays(currDate, this._currentLocation.IsInIsrael);
            var occasions = UserOccasionColection.FromSettings(currDate);
            SingleDateInfo sdi = new SingleDateInfo(currDate, new RectangleF(rect.Location, rect.Size));

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
                    new PointF(rect.X + ew, rect.Y + eh),
                    new PointF(rect.X + (rect.Width - ew), rect.Y + eh),
                    new PointF(rect.X + (rect.Width - ew), rect.Y + (rect.Height - eh)),
                    new PointF(rect.X + ew, rect.Y + (rect.Height - eh))
                }, System.Drawing.Drawing2D.FillMode.Alternate, 3f);
            }

            //accumulative height of all text rectangles - keeps track of current Y position in box.
            float offsetTop = 0f;

            //Padding top - varies according to what needs to be displayed beneath it
            rect.Y = currY + (rect.Height / (occasions.Count + holidays.Count > 1 ? 20 : 10));
            rect.X = currX + (width / 2);
            rect.Width = (width / 2);
            rect.Height = g.MeasureString(text, this._dayFont, (int)rect.Width, Program.StringFormat).Height;
            offsetTop += rect.Height;

            g.DrawString(text, this._dayFont, Program.DayTextBrush, rect, Program.StringFormat);
            rect.X = currX;
            g.DrawString(currDate.GregorianDate.Day.ToString(), this._secularDayFont, Program.SecularDayBrush, rect, Program.StringFormat);
            rect.Width = width;

            offsetTop += rect.Height / (holidays.Count > 1 ? 5 : 3);

            foreach (var o in occasions)
            {
                //Get the text size for this occasions label.
                var textSize = g.MeasureString(o.Name, this._userOccasionFont, (int)rect.Width, Program.StringFormat);

                //Move the Y position down to empty space.
                rect.Y = currY + offsetTop;
                rect.Height = textSize.Height;
                //Save the exact position of the occasion label so when the user clicks on it afterwards, we can open the occasion for editing.
                //Note: the occasion labels are centered in the days box, so we need to find the beginning of the centered text.
                o.Rectangle = new RectangleF(rect.X + ((rect.Width / 2) - (textSize.Width / 2)), rect.Y, textSize.Width, textSize.Height);
                g.DrawString(o.Name, this._userOccasionFont, new SolidBrush(o.Color), rect, Program.StringFormat);
                offsetTop += rect.Height;
            }

            if (!string.IsNullOrWhiteSpace(textZmanim))
            {
                rect.Y = currY + offsetTop;

                rect.Height = height - offsetTop;
                g.DrawString(textZmanim, this._zmanimFont, Program.ZmanimBrush, rect, Program.StringFormat);
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
                if (day == 30 && JewishDate.DaysInJewishMonth(
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
            using (var g = this.pnlMain.CreateGraphics())
            {
                var rect = sdi.RectangleF;
                g.Clip = new Region(rect);
                g.Clear(this.pnlMain.BackColor);
                this.DrawSingleDay(g, sdi.JewishDate, rect.Width, rect.Height, rect.X, rect.Y);
            }
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
                using (var g = this.pnlMain.CreateGraphics())
                {
                    g.Clip = new Region(sdi.RectangleF);
                    g.FillRectangle(Program.SelectedDayBackgroundBrush, sdi.RectangleF);
                    this._selectedDay = sdi.JewishDate;
                }
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
                    " (" + lastDayGMonth.Month.ToString() + ") " + lastDayGMonth.Year.ToString(); ;
            }
            else
            {
                caption += firstDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + firstDayGMonth.Month.ToString() + ") " + firstDayGMonth.Year.ToString() + " - " +
                    lastDayGMonth.ToString("MMMM", Program.HebrewCultureInfo) +
                    " (" + lastDayGMonth.Month.ToString() + ") " + lastDayGMonth.Year.ToString();
            }

            this.lblMonthName.Text = caption;
            this.Text = "לוח - " + caption;
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

        private void ShowSingleDayInfo(SingleDateInfo sdi)
        {
            if (sdi == null)
                return;

            var zmanim = new Zmanim(sdi.JewishDate, this._currentLocation);
            frmDailyInfoHeb f;
            if (!this.DailyPanelIsShowing)
            {
                f = new frmDailyInfoHeb(sdi.JewishDate, this._currentLocation);
                f.TopLevel = false;
                f.Parent = this;
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
                f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
                f.JewishDate = sdi.JewishDate;
            }
            if (this.splitContainer1.Panel1Collapsed)
            {
                this.splitContainer1.Panel1Collapsed = false;
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
                var f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
                f.EditOccasion(uo, new Point((int)(sdi.RectangleF.X - f.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
            }
        }

        public void Reload()
        {
            this.pnlMain.Invalidate();
            if (this.DailyPanelIsShowing)
            {
                var f = this.splitContainer1.Panel1.Controls[0] as frmDailyInfoHeb;
                f.ShowDateData();
            }
        }
        #endregion
    }
}