using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmMonthlySecular : Form
    {
        #region Private Fields

        private Location _currentLocation;
        private int _currentMonthLength;
        private int _currentMonthWeeks;
        private DateTime _dateBeingDisplayed;
        private Font _dayFont;
        private Font _dayHeadersFont;
        private bool _displayHebrew;
        private bool _isFirstOpen = true;
        private bool _isResizing;
        private Font _jewishDayFont;
        private bool _loading;
        private Point _pnlMouseLocation;
        private DateTime? _selectedDay;
        private List<SingleDateInfo> _singleDateInfoList = new List<SingleDateInfo>();
        private DateTime _todayDate = DateTime.Now.Date;
        private Font _userOccasionFont;
        private Font _zmanimFont;

        #endregion Private Fields

        #region Properties

        public DateTime CurrentDate
        {
            get
            {
                return this._dateBeingDisplayed;
            }
            set
            {
                if (this._dateBeingDisplayed.Year != value.Year || this._dateBeingDisplayed.Month != value.Month)
                {
                    this.SetCurrentMonth(value);
                    this.SetCaptionText();
                    this.pnlMain.Invalidate();
                }
            }
        }

        public bool DisplayHebrew
        {
            get
            {
                return this._displayHebrew;
            }
            set
            {
                if (this._displayHebrew != value)
                {
                    this._displayHebrew = value;
                    this.SetControlsPerLanguage();
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
                        ((dynamic)this.splitContainer1.Panel2.Controls[0]).LocationForZmanim = value;
                    }
                }
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return this._selectedDay.GetValueOrDefault();
            }
            set
            {
                if (value != null)
                {
                    if (this._dateBeingDisplayed.Month != value.Month || this._dateBeingDisplayed.Year != value.Year)
                    {
                        this.CurrentDate = value;
                    }
                    this.SelectSingleDay(value);
                }
            }
        }

        public bool DailyPanelIsShowing
        {
            get
            {
                return this.splitContainer1.Panel2.Controls.Count > 0;
            }
        }

        #endregion Properties

        #region Constructors

        public frmMonthlySecular()
        {
            Properties.Settings.Default.LastLanguage = "SecularEnglish";
            Properties.Settings.Default.Save();

            InitializeComponent();

            this.ResizeBegin += (s, e) => { this._isResizing = true; };
            this.ResizeEnd += (s, e) => { this._isResizing = false; this.pnlMain.Invalidate(); };

            this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
            this._dayFont = new Font(this.Font.FontFamily, 20f, FontStyle.Bold);
            this._zmanimFont = new Font(this.Font.FontFamily, 8, FontStyle.Regular);
            this._jewishDayFont = new Font("Narkisim", 12, FontStyle.Regular);
            this._userOccasionFont = this._zmanimFont;

            this.SetCurrentMonth(this._todayDate);
            this.dateTimePicker1.DataBindings.Add("Value", this, "SelectedDate", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion Constructors

        #region Event Handlers

        private void button1_Click(object sender, EventArgs e)
        {
            this.NavigateTo(this._dateBeingDisplayed.AddMonths(1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.NavigateTo(this._dateBeingDisplayed.AddMonths(-1));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sdi = this._singleDateInfoList.FirstOrDefault(d => d.JewishDate.GregorianDate.Date == this._todayDate);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
            }
            else
            {
                this._selectedDay = this._todayDate;
                this.CurrentDate = this._todayDate;
            }
            this.EnableArrows();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.NavigateTo(this._dateBeingDisplayed.AddYears(-1));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.NavigateTo(this._dateBeingDisplayed.AddYears(1));
        }

        private void llShowDaily_LinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate.GregorianDate == this._selectedDay ||
                                t.JewishDate.GregorianDate == this._todayDate);
            if (sdi != null)
            {
                this.ShowSingleDayInfo(sdi);
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.LocationForZmanim = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            }
        }

        private void frmMonthlyEnglish_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMonthlyEnglish_Resize(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void frmMonthlySecular_Load(object sender, EventArgs e)
        {
            this.SetLocationDataSource();
            if (!this._currentLocation.IsInIsrael)
            {
                this._loading = true;
                this.rbInChul.Checked = true;
                this._loading = false;
            }

            if (this._todayDate == DateTime.MinValue)
            {
                this._todayDate = DateTime.Now.Date;
            }
            if (this._selectedDay == null)
            {
                this._selectedDay = this._todayDate;
            }
            if (this._dateBeingDisplayed == null)
            {
                this.SetCurrentMonth(this._todayDate);
            }

            this.SetControlsPerLanguage();

            this.EnableArrows();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this._displayHebrew = (!this._displayHebrew);
            this.SetControlsPerLanguage();
            this.EnableArrows();
        }

        private void llToJewishCalendar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this._displayHebrew)
            {
                new frmMonthlyHebrew()
                {
                    DisplayedJewishMonth = new JewishDate(this._dateBeingDisplayed),
                    SelectedJewishDate = new JewishDate(this._selectedDay.GetValueOrDefault()),
                    StartPosition = this.StartPosition,
                    Bounds = this.Bounds,
                    WindowState = this.WindowState
                }.Show();
            }
            else
            {
                new frmMonthlyEnglish()
                {
                    DisplayedJewishMonth = new JewishDate(this._dateBeingDisplayed),
                    SelectedJewishDate = new JewishDate(this._selectedDay.GetValueOrDefault()),
                    StartPosition = this.StartPosition,
                    Bounds = this.Bounds,
                    WindowState = this.WindowState
                }.Show();
                this.Hide();
            }
            this.Hide();
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
                    if (this._displayHebrew)
                    {
                        (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoHeb).EditOccasion(occ, new Point((int)(sdi.RectangleF.X + sdi.RectangleF.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                    }
                    else
                    {
                        (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng).EditOccasion(occ, new Point((int)(sdi.RectangleF.X + sdi.RectangleF.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                    }
                }
            }

            this.EnableArrows();
        }

        private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null && this.DailyPanelIsShowing)
            {
                if (this._displayHebrew)
                {
                    (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoHeb).AddNewOccasion(new Point((int)(sdi.RectangleF.X + sdi.RectangleF.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
                }
                else
                {
                    (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng).AddNewOccasion(new Point((int)(sdi.RectangleF.X + sdi.RectangleF.Width), (int)(sdi.RectangleF.Y + sdi.RectangleF.Height)));
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
                        tot = this._displayHebrew ?
                        occ.Name + " - לחץ לעדכן, לשנות או למחוק" +
                            ((!string.IsNullOrWhiteSpace(occ.Notes)) ? "\r\nהערות:\r\n" + occ.Notes : "")
                        : occ.Name + " - Click to edit" +
                            ((!string.IsNullOrWhiteSpace(occ.Notes)) ? "\r\nNotes:\r\n" + occ.Notes : "");

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
            var currDate = this._dateBeingDisplayed;
            float dayWidth = (this.pnlMain.Width / 7f) + 1f;
            var eachDayHeight = (this.pnlMain.Height - 26f) / this._currentMonthWeeks;
            var currX = 0f;
            var currY = 0f;

            for (int i = 0; i < 7; i++)
            {
                this.DrawDayHeader(e.Graphics, dayWidth, currX, currY, (DayOfWeek)i);
                currX += dayWidth;
            }

            this._singleDateInfoList.Clear();

            currX = 0f;
            currY = 25f;

            while (currDate.Month == this._dateBeingDisplayed.Month)
            {
                currX = (dayWidth * (float)currDate.DayOfWeek);
                var sdi = this.DrawSingleDay(e.Graphics, currDate, dayWidth, eachDayHeight, currX, currY);

                if ((!this._isFirstOpen) && (!this.splitContainer1.Panel2Collapsed) && currDate == this._selectedDay)
                {
                    this.ShowSingleDayInfo(sdi);
                }

                currDate = currDate.AddDays(1);
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currY += eachDayHeight;
                    currX = 0f;
                }
            }

            if (this._isFirstOpen)
            {
                this._isFirstOpen = false;
                var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate.GregorianDate.Date == this._selectedDay ||
                    t.JewishDate.GregorianDate.Date == this._todayDate ||
                    t.JewishDate.GregorianDate.Date == this._dateBeingDisplayed);
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

        private void splitContainer1_Panel1_ClientSizeChanged(object sender, EventArgs e)
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
                    this.NavigateToDay(this._selectedDay.GetValueOrDefault().AddDays(1));
                    break;

                case Keys.Left:
                    this.NavigateToDay(this._selectedDay.GetValueOrDefault().AddDays(-1));
                    break;

                case Keys.Up:
                    this.NavigateToDay(this._selectedDay.GetValueOrDefault().AddDays(-7));
                    break;

                case Keys.Down:
                    this.NavigateToDay(this._selectedDay.GetValueOrDefault().AddDays(7));
                    break;

                case Keys.Enter:
                    if (this.DailyPanelIsShowing)
                    {
                        if (this._displayHebrew)
                        {
                            (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoHeb).AddNewOccasion(null);
                        }
                        else
                        {
                            (this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng).AddNewOccasion(null);
                        }
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
                var sdi = this._singleDateInfoList.FirstOrDefault(t =>
                    t.JewishDate.GregorianDate.Date == this._selectedDay.GetValueOrDefault().Date);
                this._selectedDay = null;
                if (sdi != null &&
                    this._dateBeingDisplayed.Year == sdi.JewishDate.GregorianDate.Year &&
                    this._dateBeingDisplayed.Month == sdi.JewishDate.GregorianDate.Month)
                {
                    this.RedrawSingleDay(sdi);
                }
            }
        }

        private void DrawDayHeader(Graphics g, float dayWidth, float currX, float currY, DayOfWeek dow)
        {
            var rect = new RectangleF(currX, currY, dayWidth, 25f);
            var text = this._displayHebrew ?
                Utils.JewishDOWNames[(int)dow] : Utils.DaysOfWeek[(int)dow];

            if (dow == DayOfWeek.Saturday)
            {
                rect.Width = (this.pnlMain.Width - currX) - 1f;
            }

            g.FillRectangle(Program.DayHeadersBGBrush, rect);
            g.DrawRectangle(Program.DayCellBorderPen, rect.X, rect.Y, rect.Width, rect.Height);
            g.DrawString(text, this._dayHeadersFont, Program.DayHeadersTextBrush, rect, Program.StringFormat);
        }

        private SingleDateInfo DrawSingleDay(Graphics g, DateTime currDate, float width, float height, float currX, float currY)
        {
            var jDate = new JewishDate(currDate);
            var zmanim = new Zmanim(jDate, this._currentLocation);
            var rect = new RectangleF(currX, currY, width, height);
            var text = currDate.Day.ToString();
            var holidays = Zmanim.GetHolidays(jDate, this._currentLocation.IsInIsrael);

            SingleDateInfo sdi = new SingleDateInfo(jDate, new RectangleF(rect.Location, rect.Size));

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

                width = (this.pnlMain.Width - currX) - 1f;
                rect.Width = width;

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
                        var nextMonth = jDate + 12;
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        if (this._displayHebrew)
                        {
                            sd.NameHebrew += "\nמולד: " + molad.ToStringHeb(zmanim.GetShkia());
                        }
                        else
                        {
                            sd.NameEnglish += "\nMolad: " + molad.ToString(zmanim.GetShkia());
                        }
                        break;
                    }
                }

                if (noSedra)
                {
                    textZmanim += Zmanim.GetHolidaysText(holidays, "\n", this._displayHebrew);
                }
                else
                {
                    textZmanim = string.Join(" - ", Sedra.GetSedra(jDate, this._currentLocation.IsInIsrael).Select(i =>
                        (this._displayHebrew ? i.nameHebrew : i.nameEng))) +
                            "\n" + Zmanim.GetHolidaysText(holidays, "\n", this._displayHebrew);
                }

                

                g.FillRectangle(Program.ShabbosBrush, rect);
            }
            else if (holidays.Count > 0)
            {
                var hlist = holidays.Cast<SpecialDay>();
                if (hlist.Any(h => (h.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting))))
                {
                    if (this._displayHebrew)
                    {
                        textZmanim += "הדלק\"נ: " +
                            (zmanim.GetShkia() - this._currentLocation.CandleLighting).ToString24H() + "\n";
                    }
                    else
                    {
                        textZmanim += "Candles: " +
                        (zmanim.GetShkia() - this._currentLocation.CandleLighting).ToString() + "\n";
                    }
                }
                if (hlist.Any(h => (h.DayType.IsSpecialDayType(SpecialDayTypes.MajorYomTov))))
                {
                    g.FillRectangle(Program.MajorYomtovBrush, rect);
                }
                else if (hlist.Any(h => (h.DayType.IsSpecialDayType(SpecialDayTypes.MinorYomtov))))
                {
                    g.FillRectangle(Program.MinorYomtovBrush, rect);
                }
                textZmanim += Zmanim.GetHolidaysText(holidays, "\n", this._displayHebrew);
            }

            if (sdi.UserOccasions.Any(bc => bc.BackColor != Color.Empty))
            {
                g.FillRectangle(new SolidBrush(sdi.UserOccasions.First(bc => bc.BackColor.Color != Color.Empty).BackColor), rect);
            }
            if (this._selectedDay != null && currDate == this._selectedDay)
            {
                g.FillRectangle(Program.SelectedDayBackgroundBrush, rect);
            }

            g.DrawRectangle(Program.DayCellBorderPen, rect.X, rect.Y, rect.Width, rect.Height);

            if (currDate == this._todayDate)
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
            rect.Y = currY + (rect.Height / (sdi.UserOccasions.Count + holidays.Count > 1 ? 20 : 10));

            //Secular day will be on the left, so we cut the rectangle in half.
            rect.Width /= 2;
            rect.Height = g.MeasureString(text, this._dayFont, (int)rect.Width, Program.StringFormat).Height;
            offsetTop += rect.Height;

            g.DrawString(text, this._dayFont, Program.DayTextBrush, rect, Program.StringFormat);

            //Jewish day will be on the right, so we move the rectangle over to the right of the box.
            //No need to resize the width.
            rect.X += rect.Width;
            g.DrawString(jDate.Day.ToNumberHeb().Replace("'", ""), 
                this._jewishDayFont, Program.SecularDayBrush, rect, Program.StringFormat);
            //Move rectangle back over to the left of the box
            rect.X = currX;
            //resize rectangle to fix whole box
            rect.Width = width;

            offsetTop += rect.Height / (holidays.Count > 1 ? 5 : 3);

            foreach (var o in sdi.UserOccasions)
            {
                //Get the text size for this occasions label.
                var textSize = g.MeasureString(o.Name, this._userOccasionFont, (int)rect.Width, Program.StringFormat);

                //Move the Y position down to empty space.
                rect.Y = currY + offsetTop;
                rect.Height = textSize.Height;
                //Save the exact position of the occasion label so when the user clicks on it afterwards, we can open the occasion for editing.
                //Note: the occasion labels are centered in the days box, so we can't use the X position of rect (which is always 0).
                o.Rectangle = new RectangleF((rect.Width / 2) - (textSize.Width / 2), rect.Y, textSize.Width, textSize.Height);
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

        private void EnableArrows()
        {
            //To enable arrow navigation
            this.llToJewishCalendar.Focus();
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

        private void NavigateTo(DateTime sd)
        {
            int day = 0;
            if (this._selectedDay != null)
            {
                day = this._selectedDay.Value.Day;
            }

            this.CurrentDate = sd;

            if (day > 0)
            {
                if (day == 30 && JewishDate.DaysInJewishMonth(
                    this._dateBeingDisplayed.Year, this._dateBeingDisplayed.Month) == 29)
                {
                    day = 29;
                }

                this.SelectSingleDay(new DateTime(this._dateBeingDisplayed.Year,
                    this._dateBeingDisplayed.Month, day));
            }

            this.EnableArrows();
        }

        private void NavigateToDay(DateTime jd)
        {
            //Go to the correct month
            this.CurrentDate = jd;
            this.SelectSingleDay(jd);
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
                this.DrawSingleDay(g, sdi.JewishDate.GregorianDate.Date, rect.Width, rect.Height, rect.X, rect.Y);
            }
        }

        /// <summary>
        /// This function is run after adding or editing a UserOccasion
        /// </summary>
        /// <param name="jd"></param>
        private void RefreshDay(JewishDate jd)
        {
            var sd = this._singleDateInfoList.FirstOrDefault(d => d.JewishDate == jd);
            if (sd != null)
            {
                sd.UpdateOccasions();
                this.RedrawSingleDay(sd);
            }
        }

        private void SelectSingleDay(DateTime sd)
        {
            if (this._selectedDay != sd)
            {
                if (this._dateBeingDisplayed.Year == sd.Year && this._dateBeingDisplayed.Month == sd.Month)
                {
                    var sdi = this._singleDateInfoList.FirstOrDefault(t => t.JewishDate.GregorianDate.Date == sd.Date);
                    if (sdi != null)
                    {
                        this.SelectSingleDay(sdi);
                    }
                }

                this._selectedDay = sd;
                this.dateTimePicker1.Value = this._selectedDay.GetValueOrDefault();
            }
        }

        private void SelectSingleDay(SingleDateInfo sdi)
        {
            if (sdi == null)
            {
                return;
            }

            this.ClearSelectedDay();
            if (this._selectedDay.GetValueOrDefault() != sdi.JewishDate.GregorianDate.Date)
            {
                using (var g = this.pnlMain.CreateGraphics())
                {
                    g.Clip = new Region(sdi.RectangleF);
                    g.FillRectangle(Program.SelectedDayBackgroundBrush, sdi.RectangleF);
                    this._selectedDay = sdi.JewishDate.GregorianDate.Date;
                }
            }

            this.dateTimePicker1.Value = this._selectedDay.GetValueOrDefault();
            if (this.DailyPanelIsShowing)
            {
                this.ShowSingleDayInfo(sdi);
            }
        }

        private void SetCaptionText()
        {
            string caption;
            JewishDate firstDayJMonth = new JewishDate(this._dateBeingDisplayed);
            JewishDate lastDayJMonth = new JewishDate(
                new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, this._currentMonthLength));

            if (DisplayHebrew)
            {
                caption = this._dateBeingDisplayed.ToString("MMMM (M) yyyy", Program.HebrewCultureInfo) + "  |  ";

                if (firstDayJMonth.Month == lastDayJMonth.Month)
                {
                    caption += Utils.GetProperMonthNameHeb(firstDayJMonth.Year, firstDayJMonth.Month) +
                        " " + (firstDayJMonth.Year % 1000).ToNumberHeb();
                }
                else if (firstDayJMonth.Year == lastDayJMonth.Year)
                {
                    caption += Utils.GetProperMonthNameHeb(firstDayJMonth.Year, firstDayJMonth.Month) +
                        " - " + Utils.GetProperMonthNameHeb(lastDayJMonth.Year, lastDayJMonth.Month) +
                        " " + (lastDayJMonth.Year % 1000).ToNumberHeb();
                }
                else
                {
                    caption += Utils.GetProperMonthNameHeb(firstDayJMonth.Year, firstDayJMonth.Month) +
                        " " + (firstDayJMonth.Year % 1000).ToNumberHeb() +
                        " - " + Utils.GetProperMonthNameHeb(lastDayJMonth.Year, lastDayJMonth.Month) +
                        " " + (lastDayJMonth.Year % 1000).ToNumberHeb();
                }
            }
            else
            {
                caption = this._dateBeingDisplayed.ToString("MMMM yyyy") + "  |  ";

                if (firstDayJMonth.Month == lastDayJMonth.Month)
                {
                    caption += firstDayJMonth.MonthName + " " + firstDayJMonth.Year.ToString();
                }
                else if (firstDayJMonth.Year == lastDayJMonth.Year)
                {
                    caption += firstDayJMonth.MonthName + " - " + lastDayJMonth.MonthName +
                        " " + lastDayJMonth.Year.ToString();
                }
                else
                {
                    caption += firstDayJMonth.MonthName + " " + firstDayJMonth.Year.ToString() +
                        " - " + lastDayJMonth.MonthName + " " + lastDayJMonth.Year.ToString();
                }
            }

            this.lblMonthName.Text = caption;
            this.Text = (DisplayHebrew ? "לוח לועזי" : "Secular Calendar") + " - " + caption;
        }

        private void SetControlsPerLanguage()
        {
            this._isResizing = true;
            this._loading = true;
            this.SuspendLayout();
            if (this._displayHebrew)
            {
                Properties.Settings.Default.LastLanguage = "SecularHebrew";
                Properties.Settings.Default.Save();

                this.RightToLeft = this.splitContainer1.RightToLeft = this.splitContainer2.RightToLeft =
                    this.lblInstructions.RightToLeft = this.lblLocationHeader.RightToLeft =
                    this.lblNavigationHeader.RightToLeft = this.panel1.RightToLeft =
                    this.pnlControls.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;
                this.pnlControls.Left = this.splitContainer2.Panel2.Width - this.pnlControls.Width - 5;
                this.Font = new Font("Tahoma", 7.8f);
                this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
                this._zmanimFont = new Font("Tahoma", 8, FontStyle.Regular);
                this.cmbLocation.Font = this.btnNextMonth.Font = this.btnPreviousMonth.Font =
                    this.btnNextYear.Font = this.btnPreviousYear.Font = new Font("Tahoma", 9f);
                this.lblMonthName.Font = new Font("Tahoma", 18f, FontStyle.Bold);
                this.llShowDaily.Font = new Font("Tahoma", 6.5f, FontStyle.Bold);
                this.llShowDaily.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                this.llShowDaily.Left = 0;
                this.llShowDaily.Text = "הצג לוח זמנים ˃";
                this.toolTip1.SetToolTip(this.llShowDaily, "הצג לוח זמנים");
                this.lblInstructions.Text = "לחץ פעמיים להוסיף אירוע   |   לנווט בין הימים השתמשו בלחצני החיצים";
                this.lblLocationHeader.Text = "מיקום:";
                this.lblNavigationHeader.Text = "ניווט:";
                this.rbInChul.Text = "חוץ לארץ";
                this.rbInIsrael.Text = "ארץ ישראל";
                this.btnToday.Text = "היום";
                this.btnNextMonth.Text = "← חודש הבא";
                this.btnNextMonth.Left = this.lblNavigationHeader.Left;
                this.btnPreviousMonth.Text = "חודש הקודם →";
                this.btnPreviousMonth.Left = this.lblNavigationHeader.Right - this.btnPreviousMonth.Width;
                this.btnNextYear.Text = "← שנה הבאה";
                this.btnNextYear.Left = this.btnNextMonth.Left;
                this.btnPreviousYear.Text = "שנה הקודמת →";
                this.btnPreviousYear.Left = this.btnPreviousMonth.Left;
                this.llChangeLanguage.Text = "English";
                this.llToJewishCalendar.Text = "לוח עברי";
                this.cmbLocation.DisplayMember = "NameHebrew";
                this.dateTimePicker1.Left = this.lblNavigationHeader.Left + 15;
                this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
                this.dateTimePicker1.CustomFormat = Program.HebrewCultureInfo.DateTimeFormat.LongDatePattern;
                this.dateTimePicker1.Font = this._zmanimFont;
            }
            else
            {
                Properties.Settings.Default.LastLanguage = "SecularEnglish";
                Properties.Settings.Default.Save();

                this.RightToLeft = this.splitContainer1.RightToLeft = this.splitContainer2.RightToLeft =
                   this.lblInstructions.RightToLeft = this.lblLocationHeader.RightToLeft =
                   this.lblNavigationHeader.RightToLeft = this.panel1.RightToLeft =
                   this.pnlControls.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;
                this.Font = this.btnNextMonth.Font = this.btnPreviousMonth.Font =
                    this.btnNextYear.Font = this.btnPreviousYear.Font = new Font("Century Gothic", 7.8f);
                this.pnlControls.Left = 5;
                this._zmanimFont = new Font(this.Font.FontFamily, 8, FontStyle.Regular);
                this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
                this.cmbLocation.Font = new Font(this.Font.FontFamily, 9f);
                this.lblMonthName.Font = new Font("Century Gothic", 18f, FontStyle.Bold);
                this.llShowDaily.Font = new Font("Century Gothic", 6.5f, FontStyle.Bold);
                this.llShowDaily.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.llShowDaily.Left = this.Width - this.llShowDaily.Width - 20;
                this.llShowDaily.Text = "Show Zmanim ˃";
                this.toolTip1.SetToolTip(this.llShowDaily, "Show Zmanim Panel");
                this.lblInstructions.Text = "Double-click to add an Event   |   Use the arrow keys to navigate through the days";
                this.lblLocationHeader.Text = "Location:";
                this.lblNavigationHeader.Text = "Navigation:";
                this.rbInChul.Text = "Elsewhere";
                this.rbInIsrael.Text = "In Eretz Yisroel";
                this.btnToday.Text = "Today";
                this.btnNextMonth.Text = "Next Month →";
                this.btnNextMonth.Left = this.lblNavigationHeader.Right - this.btnNextMonth.Width;
                this.btnPreviousMonth.Text = "← Previous Month";
                this.btnPreviousMonth.Left = this.lblNavigationHeader.Left;
                this.btnNextYear.Text = "Next Year →";
                this.btnNextYear.Left = this.lblNavigationHeader.Right - this.btnNextYear.Width; ;
                this.btnPreviousYear.Text = "← Previous Year";
                this.btnPreviousYear.Left = this.lblNavigationHeader.Left;
                this.llChangeLanguage.Text = "עברית";
                this.llToJewishCalendar.Text = "Jewish Calendar";
                this.cmbLocation.DisplayMember = "Name";
                this.dateTimePicker1.Left = this.lblNavigationHeader.Right - this.dateTimePicker1.Width - 15;
                this.dateTimePicker1.Format = DateTimePickerFormat.Long;
                this.dateTimePicker1.Font = this._zmanimFont;
            }
            if (this.LocationForZmanim != null)
                this.cmbLocation.SelectedItem = this.LocationForZmanim;
            this.splitContainer1.Panel2.Controls.Clear();
            this.SetCaptionText();
            this.ResumeLayout();
            this._isResizing = false;
            this._loading = false;
        }

        private void SetCurrentMonth(DateTime value)
        {
            //Set _currentDate to first of month
            this._dateBeingDisplayed = new DateTime(value.Year, value.Month, 1);
            this._currentMonthLength = DateTime.DaysInMonth(value.Year, value.Month);
            this._currentMonthWeeks = (int)this._dateBeingDisplayed.DayOfWeek >= 5 && _currentMonthLength > 29 ? 6 : 5;
        }

        private void SetDailyFormEvents(dynamic f)
        {
            if (f is frmDailyInfoEng)
            {
                ((frmDailyInfoEng)f).OccasionWasChanged += delegate (object sender, JewishDate jd)
                {
                    this.RefreshDay(jd);
                };
                ((frmDailyInfoEng)f).FormClosed += delegate
                {
                    this.splitContainer1.Panel2Collapsed = true;
                    this.llShowDaily.Visible = true;
                };
            }
            else
            {
                ((frmDailyInfoHeb)f).OccasionWasChanged += delegate (object sender, JewishDate jd)
                {
                    this.RefreshDay(jd);
                };
                ((frmDailyInfoHeb)f).FormClosed += delegate
                {
                    this.splitContainer1.Panel2Collapsed = true;
                    this.llShowDaily.Visible = true;
                };
            }
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
            this.cmbLocation.DisplayMember = this._displayHebrew ? "NameHebrew" : "Name";

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
            dynamic frmDailyInfo;
            if (!this.DailyPanelIsShowing)
            {
                if (this._displayHebrew)
                    frmDailyInfo = new frmDailyInfoHeb(sdi.JewishDate, this._currentLocation);
                else
                    frmDailyInfo = new frmDailyInfoEng(sdi.JewishDate, this._currentLocation);
                frmDailyInfo.TopLevel = false;
                frmDailyInfo.Parent = this;
                this.SetDailyFormEvents(frmDailyInfo);
                frmDailyInfo.Dock = DockStyle.Fill;
                this.splitContainer1.Panel2.Controls.Add(frmDailyInfo);
                frmDailyInfo.Show();
                this.llShowDaily.Visible = false;
            }
            else
            {
                frmDailyInfo = this.splitContainer1.Panel2.Controls[0];
                frmDailyInfo.JewishDate = sdi.JewishDate;
            }
            if (this.splitContainer1.Panel2Collapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
            }
        }

        #endregion Private Functions
    }
}