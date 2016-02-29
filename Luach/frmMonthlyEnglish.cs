using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Luach
{
    public partial class frmMonthlyEnglish : Form
    {
        #region Private Fields
        private JewishDate _currentJewishDate;
        private JewishDate _todayJewishDate = new JewishDate();
        private bool _loading;
        private Location _currentLocation;
        private int _currentMonthLength;
        private int _currentMonthWeeks;
        private List<Tuple<JewishDate, string, RectangleF>> _dateList = new List<Tuple<JewishDate, string, RectangleF>>();
        private Font _dayHeadersFont;
        private Font _dayFont;
        private Font _zmanimFont;
        private Font _secularDayFont;
        private Point _pnlMouseLocation;
        private JewishDate _selectedDay;
        private bool _isResizing;
        #endregion

        #region Properties
        public JewishDate CurrentJewishDate
        {
            get
            {
                return this._currentJewishDate;
            }
            set
            {
                //Set _currentJewishDate to first of month
                this._currentJewishDate = value - (value.Day - 1);
                this._currentMonthLength = JewishDateCalculations.DaysInJewishMonth(this._currentJewishDate.Year, this._currentJewishDate.Month);
                this._currentMonthWeeks = (int)this._currentJewishDate.DayOfWeek >= 5 && _currentMonthLength > 29 ? 6 : 5;
                this.SetCaptionText();
                this.pnlMain.Invalidate();
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
                if (this._currentJewishDate.Month != value.Month || this._currentJewishDate.Year != value.Year)
                {
                    this.CurrentJewishDate = value;
                }
                this.SelectSingleDay(value);
            }
        }
        #endregion

        #region Constructors
        public frmMonthlyEnglish()
        {
            InitializeComponent();
            this.ResizeBegin += (s, e) => { this._isResizing = true; };
            this.ResizeEnd += (s, e) => { this._isResizing = false; this.pnlMain.Invalidate(); };

            this._dayHeadersFont = new Font(this.pnlMain.Font.FontFamily, 10, FontStyle.Regular);
            this._dayFont = new Font(this.pnlMain.Font.FontFamily, 20, FontStyle.Regular);
            this._zmanimFont = new Font(this.Font.FontFamily, 7, FontStyle.Regular);
            this._secularDayFont = new Font("San Serif", 7.5f);
        }
        #endregion

        #region Event Handlers
        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            if (this._isResizing)
            {
                return;
            }

            var currDate = this._currentJewishDate;
            float dayWidth = (this.pnlMain.Width / 7f) + 1f;
            var eachDayHeight = (this.pnlMain.Height - 26f) / this._currentMonthWeeks;
            var currX = 0f;
            var currY = 0f;

            for (int i = 0; i < 7; i++)
            {
                this.DrawDayHeader(e.Graphics, dayWidth, currX, currY, (DayOfWeek)i);
                currX += dayWidth;
            }

            this._dateList.Clear();

            currX = 0f;
            currY = 25f;

            while (currDate.Month == this._currentJewishDate.Month)
            {
                currX = (dayWidth * (float)currDate.DayOfWeek);
                this.DrawSingleDay(e.Graphics, currDate, dayWidth, eachDayHeight, currX, currY);
                currDate += 1;
                if (currDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    currY += eachDayHeight;
                    currX = 0f;
                }
            }
        }

        private void pnlMain_Leave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(this.pnlMain);
        }

        private void pnlMain_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(this.pnlMain);
        }

        private void frmMonthlyEnglish_Resize(object sender, EventArgs e)
        {
            this.pnlMain.Invalidate();
        }

        private void frmMonthlyEnglish_Load(object sender, EventArgs e)
        {
            this.SetLocationDataSource();
            if (!this._currentLocation.IsInIsrael)
            {
                this._loading = true;
                this.rbInChul.Checked = true;
                this._loading = false;
            }
            if (_currentJewishDate == null)
                this.CurrentJewishDate = new JewishCalendar.JewishDate();

            this.SetCaptionText();
        }

        private void rbInIsrael_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.SetLocationDataSource();
            }
            //Location was changed, so we need to re-do the zmanim
            this.pnlMain.Invalidate();
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this._currentLocation = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
                //Location was changed, so we need to re-do the zmanim
                this.pnlMain.Invalidate();
                Properties.Settings.Default.LocationName = this._currentLocation.Name;
                Properties.Settings.Default.Save();
            }
        }

        private void pnlMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var tuple = GetTupleFromLocation(e.Location);
            if (tuple != null)
            {
                var f = new frmDailyEnglish { CurrentJewishDate = tuple.Item1, Owner = this };
                f.Show(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.CurrentJewishDate += 30;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this._currentJewishDate.Year != this._todayJewishDate.Year ||
                this._currentJewishDate.Month != this._todayJewishDate.Month)
            {
                this.CurrentJewishDate = this._selectedDay = this._todayJewishDate;
            }
            else
            {
                this.SelectToday();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.CurrentJewishDate -= 28;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            this.CurrentJewishDate = new JewishDate(this.CurrentJewishDate.Year - 1,
                this.CurrentJewishDate.Month, this.CurrentJewishDate.Day);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.CurrentJewishDate = new JewishDate(this.CurrentJewishDate.Year + 1,
                this.CurrentJewishDate.Month, this.CurrentJewishDate.Day);
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            this._pnlMouseLocation = e.Location;
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 1)
                return;

            var tuple = this.GetTupleFromLocation(this._pnlMouseLocation);

            if (tuple == null)
                return;

            if (this._selectedDay != tuple.Item1)
            {
                this.SelectSingleDay(tuple, true);
            }
            else if (this._selectedDay == tuple.Item1)
            {
                this.ClearSelectedDay();
            }
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

        private void DrawSingleDay(Graphics g, JewishDate currDate, float width, float height, float currX, float currY)
        {
            var zmanim = new Zmanim(currDate, this._currentLocation);
            var rect = new RectangleF(currX, currY, width, height);
            var text = currDate.Day.ToNumberHeb();
            var holidays = Zmanim.GetHolidays(currDate, this._currentLocation.IsInIsrael);
            string textZmanim = "";

            if (currDate.DayOfWeek == DayOfWeek.Saturday)
            {
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
                    if (sd.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = currDate + 12;
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        sd.NameEnglish += "\nMolad: " + molad.ToString(zmanim.GetShkia());
                        break;
                    }
                }

                textZmanim = string.Join(" - ", Sedra.GetSedra(currDate, false).Select(i => i.nameEng)) +
                        "\n" + Zmanim.GetHolidaysText(holidays, "\n", false);

                g.FillRectangle(Program.ShabbosBrush, rect);
            }
            else if (holidays.Count > 0)
            {
                var hlist = holidays.Cast<SpecialDay>();
                if (hlist.Any(h =>
                    (h.DayType & SpecialDayTypes.HasCandleLighting) == SpecialDayTypes.HasCandleLighting))
                {
                    textZmanim += "Candles: " +
                        (zmanim.GetShkia() - this._currentLocation.CandleLighting).ToString() + "\n";
                }
                if (hlist.Any(h =>
                    (h.DayType & SpecialDayTypes.MajorYomTov) == SpecialDayTypes.MajorYomTov ||
                    (h.DayType & SpecialDayTypes.MinorYomtov) == SpecialDayTypes.MinorYomtov))
                {
                    g.FillRectangle(Program.YomtovBrush, rect);
                }
                textZmanim += Zmanim.GetHolidaysText(holidays, "\n", false);
            }

            this._dateList.Add(Tuple.Create(currDate, textZmanim, rect));

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

            rect.Height /= 2;
            rect.Width /= 2;
            g.DrawString(text, this._dayFont, Program.DayTextBrush, rect, Program.StringFormat);
            rect.X += rect.Width;
            g.DrawString(currDate.GregorianDate.Day.ToString(), this._secularDayFont, Program.SecularDayBrush, rect, Program.StringFormat);
            rect.X -= rect.Width;
            rect.Width *= 2;
            if (!string.IsNullOrWhiteSpace(textZmanim))
            {
                rect.Y += rect.Height;
                rect.Height = height - rect.Height;
                g.DrawString(textZmanim, this._zmanimFont, Program.ZmanimBrush, rect, Program.StringFormat);
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
            this.cmbLocation.DisplayMember = "Name";

            var name = Properties.Settings.Default.LocationName;
            var i = list.FirstOrDefault(l => l.Name == name);
            if (i != null)
            {
                this.cmbLocation.SelectedItem = i;
            }
            this._currentLocation = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            this._loading = wasLoading;
            this.Cursor = Cursors.Default;
        }

        private Tuple<JewishDate, string, RectangleF> GetTupleFromLocation(Point location)
        {
            return this._dateList.FirstOrDefault(t =>
                t.Item3.Left < location.X &&
                t.Item3.Right > location.X &&
                t.Item3.Top < location.Y &&
                t.Item3.Bottom > location.Y);
        }

        private void ShowSingleDayToolTip(Tuple<JewishDate, string, RectangleF> tuple)
        {
            var zmanim = new Zmanim(tuple.Item1, this._currentLocation);
            var shkiaNetz = zmanim.GetNetzShkia();
            var text = tuple.Item1.GregorianDate.ToString("MMMM d, yyyy") + "\n" +
                (string.IsNullOrWhiteSpace(tuple.Item2) ? "" : tuple.Item2 + "\n") +
                "Sunrise: " + shkiaNetz[0].ToString() + "\n" +
                "Sunset: " + shkiaNetz[1].ToString();
            this.toolTip1.ToolTipTitle = tuple.Item1.DayOfWeek.ToString() + " " + tuple.Item1.ToLongDateString();
            this.toolTip1.Show(text, this.pnlMain, this._pnlMouseLocation.X + 5, this._pnlMouseLocation.Y + 5, 10000);
            this.pnlMain.Focus();
        }

        private void SelectSingleDay(JewishDate jd)
        {
            if (this._selectedDay != jd)
            {
                if (this._currentJewishDate.Year == jd.Year && this._currentJewishDate.Month == jd.Month)
                {
                    var tuple = this._dateList.FirstOrDefault(t => t.Item1 == jd);
                    if (tuple != null)
                    {
                        this.SelectSingleDay(tuple, false);
                    }
                }
            }
            if (this._selectedDay != jd)
            {
                this._selectedDay = jd;
            }
        }

        private void SelectSingleDay(Tuple<JewishDate, string, RectangleF> tuple, bool showToolTip)
        {
            if (showToolTip)
                this.ShowSingleDayToolTip(tuple);

            if (this._selectedDay != tuple.Item1)
            {
                this.ClearSelectedDay();
                using (var g = this.pnlMain.CreateGraphics())
                {
                    g.Clip = new Region(tuple.Item3);
                    g.FillRectangle(Program.SelectedDayBackgroundBrush, tuple.Item3);
                    this._selectedDay = tuple.Item1;
                }
            }
        }

        private void ClearSelectedDay()
        {
            if (this._selectedDay != null)
            {
                var tuple = this._dateList.FirstOrDefault(t => t.Item1 == this._selectedDay);
                this._selectedDay = null;
                if (tuple != null && this._currentJewishDate.Year == tuple.Item1.Year && this._currentJewishDate.Month == tuple.Item1.Month)
                {
                    using (var g = this.pnlMain.CreateGraphics())
                    {
                        var rect = tuple.Item3;
                        g.Clip = new Region(rect);
                        g.Clear(this.pnlMain.BackColor);
                        DrawSingleDay(g, tuple.Item1, rect.Width, rect.Height, rect.X, rect.Y);
                    }
                }
            }
        }

        private void SelectToday()
        {
            var tuple = this._dateList.FirstOrDefault(t => t.Item1 == this._todayJewishDate);
            if (tuple != null)
            {
                this._pnlMouseLocation = new Point(
                    (int)(tuple.Item3.X + (tuple.Item3.Width / 2)),
                    (int)(tuple.Item3.Y + (tuple.Item3.Height / 2)));

                this.SelectSingleDay(tuple, false);
            }
        }

        private void SetCaptionText()
        {
            string caption = this._currentJewishDate.MonthName + " " +
                this._currentJewishDate.Year.ToString() + "  |  ";
            DateTime firstDayGMonth = this._currentJewishDate.GregorianDate;
            DateTime lastDayGMonth = (this._currentJewishDate + (this._currentMonthLength - 1)).GregorianDate;

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
        #endregion
    }
}
