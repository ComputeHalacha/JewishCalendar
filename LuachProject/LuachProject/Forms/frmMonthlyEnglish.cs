using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmMonthlyEnglish : Form
    {
        #region Private Fields
        private bool _isFirstOpen = true;
        private JewishDate _displayedJewishMonth;
        private JewishDate _todayJewishDate;
        private bool _loading;
        private Location _currentLocation;
        private int _currentMonthLength;
        private int _currentMonthWeeks;
        private List<SingleDateInfo> _singleDateInfoList = new List<SingleDateInfo>();
        private Font _dayHeadersFont;
        private Font _dayFont;
        private Font _zmanimFont;
        private Font _secularDayFont;
        private Font _userOccasionFont;
        private Point _pnlMouseLocation;
        private JewishDate _selectedDay;
        private bool _isResizing;
        #endregion

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
                    this._currentMonthLength = JewishDateCalculations.DaysInJewishMonth(this._displayedJewishMonth.Year, this._displayedJewishMonth.Month);
                    this._currentMonthWeeks = (int)this._displayedJewishMonth.DayOfWeek >= 5 && _currentMonthLength > 29 ? 6 : 5;
                    this.SetCaptionText();
                    this.llSefira.Visible = this._displayedJewishMonth.Month.In(1, 2);
                    this.pnlMain.Invalidate();
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

                    if (this.splitContainer1.Panel2.Controls.Count > 0)
                    {
                        ((frmDailyInfoEng)this.splitContainer1.Panel2.Controls[0]).LocationForZmanim = value;
                    }
                }
            }
        }
        #endregion

        #region Constructors
        public frmMonthlyEnglish()
        {
            Properties.Settings.Default.LastLanguage = "English";
            Properties.Settings.Default.Save();

            InitializeComponent();

            this.ResizeBegin += (s, e) => { this._isResizing = true; };
            this.ResizeEnd += (s, e) => { this._isResizing = false; this.pnlMain.Invalidate(); };

            this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
            this._dayFont = new Font("Narkisim", 20, FontStyle.Regular);
            this._zmanimFont = new Font(this.Font.FontFamily, 8, FontStyle.Regular);
            this._secularDayFont = new Font(this.Font.FontFamily, 8.5f);
            this._userOccasionFont = this._zmanimFont;
            this.jewishDatePicker1.DataBindings.Add("Value", this, "SelectedJewishDate", true, DataSourceUpdateMode.OnPropertyChanged);
        }
        #endregion

        #region Event Handlers
        private void frmMonthlyEnglish_Load(object sender, EventArgs e)
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

            this.llSefira.Visible = this._displayedJewishMonth.Month.In(1, 2);

            this.EnableArrows();
        }

        private void frmMonthlyEnglish_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (this._isResizing)
            {
                return;
            }

            var currDate = this._displayedJewishMonth;
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

            while (currDate.Month == this._displayedJewishMonth.Month)
            {
                currX = (dayWidth * (float)currDate.DayOfWeek);
                var sdi = this.DrawSingleDay(e.Graphics, currDate, dayWidth, eachDayHeight, currX, currY);

                if ((!this._isFirstOpen) && (!this.splitContainer1.Panel2Collapsed) && currDate == this._selectedDay)
                {
                    this.ShowSingleDayInfo(sdi);
                }

                currDate += 1;
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currY += eachDayHeight;
                    currX = 0f;
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
        }

        private void frmMonthlyEnglish_Resize(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void rbInIsrael_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.SetLocationDataSource();
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.LocationForZmanim = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(this._displayedJewishMonth + 31);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(this._displayedJewishMonth - 28);
        }

        private void splitContainer2_KeyDown(object sender, KeyEventArgs e)
        {
            if (this._selectedDay == null || this.cmbLocation.ContainsFocus)
                return;
            switch (e.KeyData)
            {
                case Keys.Right:
                    this.NavigateToDay(this._selectedDay + 1);
                    break;
                case Keys.Left:
                    this.NavigateToDay(this._selectedDay - 1);
                    break;
                case Keys.Up:
                    this.NavigateToDay(this._selectedDay - 7);
                    break;
                case Keys.Down:
                    this.NavigateToDay(this._selectedDay + 7);
                    break;
                case Keys.Enter:
                    if (this.splitContainer1.Panel2.Controls.Count > 0)
                    {
                        var f = this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng;
                        f.AddNewOccasion();
                    }
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(new JewishDate(this.DisplayedJewishMonth.Year - 1,
                this.DisplayedJewishMonth.Month, this.DisplayedJewishMonth.Day));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.NavigateToMonth(new JewishDate(this._displayedJewishMonth.Year + 1,
                this._displayedJewishMonth.Month, this._displayedJewishMonth.Day));
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
                        tot = occ.Name + " - Click to edit" + 
                            ((!string.IsNullOrWhiteSpace(occ.Notes)) ? "\r\nNotes:\r\n" + occ.Notes : "");
                    
                    if(string.IsNullOrEmpty(currTText) || currTText != tot)
                    {
                        this.toolTip1.SetToolTip(this.pnlMain, tot);                                            
                    }
                    return;
                }
            }
            this.pnlMain.Cursor = Cursors.Default;
            this.toolTip1.SetToolTip(this.pnlMain, null);
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                this.SelectSingleDay(sdi);
                var occ = this.GetUserOccasionFromLocation(this._pnlMouseLocation, sdi);

                if (occ != null && this.splitContainer1.Panel2.Controls.Count > 0)
                {
                    var f = this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng;
                    f.EditOccasion(occ);
                }
            }

            this.EnableArrows();
        }

        private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var sdi = this.GetSingleDateInfoFromLocation(this._pnlMouseLocation);

            if (sdi != null)
            {
                if (this.splitContainer1.Panel2.Controls.Count > 0)
                {
                    var f = this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng;
                    f.AddNewOccasion();
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmMonthlyHebrew()
            {
                DisplayedJewishMonth = this._displayedJewishMonth,
                SelectedJewishDate = this._selectedDay,
                StartPosition = this.StartPosition,
                Bounds = this.Bounds,
                WindowState = this.WindowState
            }.Show();
            this.Hide();
        }

        private void llSefira_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process a = new Process();
            a.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, @"OmerReminder.exe");
            a.StartInfo.Arguments = "-location \"" + this._currentLocation.Name + "\"" + " -lang eng";
            a.Start();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void splitContainer1_Panel1_ClientSizeChanged(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void llSecularCalendar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmMonthlySecular()
            {
                CurrentDate = this._displayedJewishMonth.GregorianDate.Date,
                SelectedDate = this._selectedDay.GregorianDate.Date,
                StartPosition = this.StartPosition,
                Bounds = this.Bounds,
                WindowState = this.WindowState
            }.Show();
            this.Hide();
        }
        #endregion

        #region Private Functions
        private void DrawDayHeader(Graphics g, float dayWidth, float currX, float currY, DayOfWeek dow)
        {
            var rect = new RectangleF(currX, currY, dayWidth, 25f);
            var text = Utils.DaysOfWeek[(int)dow];

            if (dow == DayOfWeek.Saturday)
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
            var text = currDate.Day.ToNumberHeb();
            var holidays = Zmanim.GetHolidays(currDate, this._currentLocation.IsInIsrael);

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
                width = (this.pnlMain.Width - currX) - 1f;
                rect.Width = width;

                foreach (SpecialDay sd in holidays)
                {
                    if (sd.DayType == SpecialDay.SpecialDayTypes.Shabbos)
                    {
                        holidays.Remove(sd);
                        break;
                    }
                }

                foreach (SpecialDay sd in holidays)
                {
                    if (sd.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = currDate + 12;
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        sd.NameEnglish += "\nMolad: " + molad.ToString(zmanim.GetShkia());
                        break;
                    }
                }

                textZmanim = string.Join(" - ", Sedra.GetSedra(currDate, this._currentLocation.IsInIsrael).Select(i => i.nameEng)) +
                        "\n" + Zmanim.GetHolidaysText(holidays, "\n", false);

                g.FillRectangle(Program.ShabbosBrush, rect);
            }
            else if (holidays.Count > 0)
            {
                var hlist = holidays.Cast<SpecialDay>();
                if (hlist.Any(h =>
                    (h.DayType & SpecialDay.SpecialDayTypes.HasCandleLighting) == SpecialDay.SpecialDayTypes.HasCandleLighting))
                {
                    textZmanim += "Candles: " +

                        (zmanim.GetShkia() - this._currentLocation.CandleLighting).ToString() + "\n";
                }
                if (hlist.Any(h =>
                    (h.DayType & SpecialDay.SpecialDayTypes.MajorYomTov) == SpecialDay.SpecialDayTypes.MajorYomTov ||
                    (h.DayType & SpecialDay.SpecialDayTypes.MinorYomtov) == SpecialDay.SpecialDayTypes.MinorYomtov))
                {
                    g.FillRectangle(Program.YomtovBrush, rect);
                }
                textZmanim += Zmanim.GetHolidaysText(holidays, "\n", false);
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
            rect.Y = currY + (rect.Height / (sdi.UserOccasions.Count + holidays.Count > 1 ? 20 : 10));

            //Hebrew day will be on the left, so we cut the rectangle in half.
            rect.Width /= 2;
            rect.Height = g.MeasureString(text, this._dayFont, (int)rect.Width, Program.StringFormat).Height;
            offsetTop += rect.Height;

            g.DrawString(text, this._dayFont, Program.DayTextBrush, rect, Program.StringFormat);

            //Secular day will be on the right, so we move the rectangle over to the right of the box.
            //No need to resize the width.
            rect.X += rect.Width;
            g.DrawString(currDate.GregorianDate.Day.ToString(), this._secularDayFont, Program.SecularDayBrush, rect, Program.StringFormat);
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

        private void SetLocationDataSource()
        {
            this.Cursor = Cursors.WaitCursor;
            bool wasLoading = this._loading;
            this._loading = true;
            bool inIsrael = this.rbInIsrael.Checked;
            var list = Program.LocationsList.Where(l => l.IsInIsrael == inIsrael).ToList();
            this.cmbLocation.DataSource = null;
            this.cmbLocation.DataSource = list;
            this.cmbLocation.DisplayMember = "Name";

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

        private void ShowSingleDayInfo(SingleDateInfo sdi)
        {
            if (sdi == null)
                return;

            var zmanim = new Zmanim(sdi.JewishDate, this._currentLocation);
            frmDailyInfoEng f;
            if (this.splitContainer1.Panel2.Controls.Count == 0)
            {
                f = new frmDailyInfoEng(sdi.JewishDate, this._currentLocation);
                f.TopLevel = false;
                f.Parent = this;
                f.OccasionWasChanged += delegate(object sender, JewishDate jd)
                {
                    this.RedrawSingleDay(this._singleDateInfoList.FirstOrDefault(d =>
                        d.JewishDate == jd));
                };
                f.FormClosed += delegate
                {
                    this.splitContainer1.Panel2Collapsed = true;
                };
                f.Dock = DockStyle.Fill;
                this.splitContainer1.Panel2.Controls.Add(f);
                f.Show();
            }
            else
            {
                f = this.splitContainer1.Panel2.Controls[0] as frmDailyInfoEng;
                f.JewishDate = sdi.JewishDate;
            }
            if (this.splitContainer1.Panel2Collapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
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
            this.ShowSingleDayInfo(sdi);
        }

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

        private void SetCaptionText()
        {
            string caption = this._displayedJewishMonth.MonthName + " " +
                this._displayedJewishMonth.Year.ToString() + "  |  ";
            DateTime firstDayGMonth = this._displayedJewishMonth.GregorianDate;
            DateTime lastDayGMonth = (this._displayedJewishMonth + (this._currentMonthLength - 1)).GregorianDate;

            if (firstDayGMonth.Month == lastDayGMonth.Month)
            {
                caption += firstDayGMonth.ToString("MMMM yyyy");

            }
            else if (firstDayGMonth.Year == lastDayGMonth.Year)
            {
                caption += firstDayGMonth.ToString("MMMM") + " - " + lastDayGMonth.ToString("MMMM yyyy");
            }
            else
            {
                caption += firstDayGMonth.ToString("MMMM yyyy") + " - " + lastDayGMonth.ToString("MMMM yyyy");
            }

            this.lblMonthName.Text = caption;
            this.Text = "Luach - " + caption;
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

        /// <summary>
        /// To enable arrow navigation, the focus must be in the split-container.
        /// </summary>
        private void EnableArrows()
        {
            //For some odd reason, the focus only works if we go traveling around the form a bit.
            this.llSecularCalendar.Focus();

            this.splitContainer2.Focus();
        }

        #endregion
    }
}
