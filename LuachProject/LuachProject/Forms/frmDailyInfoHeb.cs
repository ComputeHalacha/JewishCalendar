using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmDailyInfoHeb : Form
    {
        #region events

        public event EventHandler<JewishDate> OccasionWasChanged;

        #endregion events

        #region private fields

        private Font _dateDiffDaysFont;
        private Font _dateDiffExpFont;
        private JewishDate _displayingJewishDate;
        private DateTime _displayingSecularDate;
        private frmAddOccasionHeb _frmAddOccasionHeb;
        private IEnumerable<SpecialDay> _holidays;
        private Font _lblOccasionFont;
        private Font _lineValueFont;
        private IEnumerable<UserOccasion> _occasions;
        private Font _sefirahFont;
        private JewishCalendar.Zmanim _zmanim;

        #endregion private fields

        #region constructor

        public frmDailyInfoHeb(JewishDate jd, Location location)
        {
            this._displayingJewishDate = jd;
            this._zmanim = new Zmanim(jd, location);
            this._holidays = Zmanim.GetHolidays(jd, location.IsInIsrael).Cast<SpecialDay>();
            this._occasions = UserOccasionColection.FromSettings(jd);

            InitializeComponent();

            this._lblOccasionFont = new Font(this.tableLayoutPanel1.Font, FontStyle.Bold);
            this._lineValueFont = new Font(this.richTextBox1.Font, FontStyle.Bold);
            this._dateDiffDaysFont = new Font(this.richTextBox1.Font.FontFamily, 9f, FontStyle.Bold);
            this._dateDiffExpFont = new Font(this.richTextBox1.Font.FontFamily, 7.3f, FontStyle.Italic);
            this._sefirahFont = new Font(this.richTextBox1.Font.FontFamily, 9f);
        }

        #endregion constructor

        #region properties

        public JewishDate JewishDate
        {
            get
            {
                return this._displayingJewishDate;
            }
            set
            {
                if (value != this._displayingJewishDate)
                {
                    this._displayingJewishDate = value;
                    //If we are displaying todays date for the current time zone, we will show the "proper" secular date.
                    if (this.IsHereAndNow())
                    {
                        this._displayingSecularDate = this._displayingJewishDate.GregorianDate;                        
                    }
                    else
                    {
                        this._displayingSecularDate = JewishDateCalculations.GetGregorianDateFromJewishDate(
                            this._displayingJewishDate,
                            (HourMinute)DateTime.Now.TimeOfDay,
                            this._zmanim.Location);
                    }
                    this._zmanim.SecularDate = this._displayingSecularDate;
                    this._holidays = Zmanim.GetHolidays(value, this._zmanim.Location.IsInIsrael).Cast<SpecialDay>();
                    this._occasions = UserOccasionColection.FromSettings(this._displayingJewishDate);
                    this.tableLayoutPanel1.Controls.Clear();
                    this.ShowDateData();
                }
            }
        }

        public Location LocationForZmanim
        {
            get
            {
                return this._zmanim.Location;
            }
            set
            {
                this._zmanim = new Zmanim(this._displayingSecularDate, value);
                this._holidays = Zmanim.GetHolidays(this._displayingJewishDate, this._zmanim.Location.IsInIsrael).Cast<SpecialDay>();
                this.ShowDateData();
            }
        }

        #endregion properties

        #region event handlers

        private void button1_Click(object sender, EventArgs e)
        {
            this.AddNewOccasion(null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //If we are displaying todays date for the current time zone, we will show the "proper" secular date.
            if (this.IsHereAndNow())
            {
                this._displayingSecularDate = this._displayingJewishDate.GregorianDate;
            }
            else
            {
                this._displayingSecularDate = JewishDateCalculations.GetGregorianDateFromJewishDate(
                            this._displayingJewishDate,
                            (HourMinute)DateTime.Now.TimeOfDay,
                            this._zmanim.Location);
            }

            this.ShowDateData();
        }

        #endregion event handlers

        #region private functions

        public void AddNewOccasion(Point? parentPoint)
        {
            if (this._frmAddOccasionHeb != null)
            {
                this._frmAddOccasionHeb.CloseStyle = frmAddOccasionHeb.CloseStyles.None;
                this._frmAddOccasionHeb.Close();
            }
            this._frmAddOccasionHeb = new frmAddOccasionHeb { JewishDate = this._displayingJewishDate };
            this.PositionAddOccasion(parentPoint);
            this._frmAddOccasionHeb.OccasionWasChanged += delegate (object sndr, UserOccasion uo)
            {
                if (UserOccasionColection.FromSettings(this.JewishDate).Contains(uo))
                {
                    this.AddOccasion(uo);
                    this.tableLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                }
                OccasionWasChanged?.Invoke(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));
            };
        }

        public void EditOccasion(UserOccasion occ, Point? parentPoint)
        {
            if (this._frmAddOccasionHeb != null)
            {
                this._frmAddOccasionHeb.CloseStyle = frmAddOccasionHeb.CloseStyles.None;
                this._frmAddOccasionHeb.Close();
            }
            this._frmAddOccasionHeb = new frmAddOccasionHeb(occ);
            LinkLabel lnkLbl = this.tableLayoutPanel1.Controls.OfType<LinkLabel>().First(ll => ll.Tag == occ);
            Label lbl = (Label)this.tableLayoutPanel1.Controls[this.tableLayoutPanel1.Controls.IndexOf(lnkLbl) + 1];

            this._frmAddOccasionHeb.OccasionWasChanged += delegate (object sndr, UserOccasion uo)
            {
                OccasionWasChanged?.Invoke(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));

                if (this._frmAddOccasionHeb.UserOccasion == null ||
                (!UserOccasionColection.FromSettings(this._displayingJewishDate).Contains(this._frmAddOccasionHeb.UserOccasion)))
                {
                    this.tableLayoutPanel1.Controls.Remove(lbl);
                    this.tableLayoutPanel1.Controls.Remove(lnkLbl);
                    if (this.tableLayoutPanel1.RowCount > 0)
                    {
                        this.tableLayoutPanel1.RowCount -= 1;
                    }                    
                }
                else
                {
                    lnkLbl.Text = this._frmAddOccasionHeb.UserOccasion.Name;
                    lnkLbl.LinkColor = this._frmAddOccasionHeb.UserOccasion.Color;
                    var dateDiff = this._frmAddOccasionHeb.UserOccasion.GetAnniversaryString(this.JewishDate, false);
                    lbl.Text = ((!string.IsNullOrWhiteSpace(dateDiff)) ? "(" + dateDiff + ") " : "") +
                        (this._frmAddOccasionHeb.UserOccasion.Notes ?? "");
                    this.tableLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                }
            };
            this.PositionAddOccasion(parentPoint);
        }

        private void AddLine(string header, string value)
        {
            this.richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            this.richTextBox1.SelectionFont = this.richTextBox1.Font;            
            this.richTextBox1.SelectedText = header.Trim() + " " +
                new string('.', 15) + " ";
            this.richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            this.richTextBox1.SelectionFont = this._lineValueFont;
            this.richTextBox1.SelectionColor = Color.CornflowerBlue;
            this.richTextBox1.SelectedText = value.Trim() + Environment.NewLine;
        }

        private void AddOccasion(UserOccasion occ)
        {
            var lnkLbl = new LinkLabel
            {
                Text = occ.Name,
                Font = this._lblOccasionFont,
                LinkColor = occ.Color,
                Tag = occ,
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                LinkBehavior = LinkBehavior.HoverUnderline
            };
            string dateDiff = occ.GetAnniversaryString(this.JewishDate, false);
            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = occ.Color,
                Text = ((!string.IsNullOrWhiteSpace(dateDiff)) ? "(" + dateDiff + ") " : "") +
                    (occ.Notes ?? "")
            };

            lnkLbl.MouseClick += delegate
            {
                this.EditOccasion(occ, null);
            };

            lbl.MouseClick += delegate
            {
                this.EditOccasion(occ, null);
            };

            this.tableLayoutPanel1.Controls.Add(lnkLbl);
            this.tableLayoutPanel1.Controls.Add(lbl);
        }

        private bool IsHereAndNow()
        {
            return this._zmanim.Location.TimeZoneInfo != null &&
                TimeZoneInfo.Local.Id == this._zmanim.Location.TimeZoneInfo.Id &&
                new JewishDate(DateTime.Now, this._zmanim.Location) == this._displayingJewishDate;
        }

        private void PositionAddOccasion(Point? parentPoint)
        {
            this._frmAddOccasionHeb.StartPosition = FormStartPosition.Manual;
            this._frmAddOccasionHeb.SuspendLayout();

            if (parentPoint == null)
            {
                var pointZero = new Point(-this._frmAddOccasionHeb.Width, this.ParentForm.Bottom - this._frmAddOccasionHeb.Height - 7);

                this._frmAddOccasionHeb.CloseStyle = frmAddOccasionHeb.CloseStyles.Slide;
                this._frmAddOccasionHeb.Location = pointZero;
                this._frmAddOccasionHeb.Show(this);
                var a = 0;
                while (true)
                {
                    if (this._frmAddOccasionHeb.Width - a < 50)
                    {
                        this._frmAddOccasionHeb.Location = new Point(0, pointZero.Y);
                        break;
                    }
                    else
                    {
                        this._frmAddOccasionHeb.Location = new Point(pointZero.X + a, pointZero.Y);
                        this._frmAddOccasionHeb.Refresh();
                        a += 50;
                    }
                }

                this._frmAddOccasionHeb.BringToFront();
            }
            else
            {
                this._frmAddOccasionHeb.CloseStyle = frmAddOccasionHeb.CloseStyles.Fade;

                //Opacity is not supported on Right-To-Left
                //this._frmAddOccasionHeb.RightToLeft = RightToLeft.No;
                this._frmAddOccasionHeb.Opacity = 0;

                var point = parentPoint.Value;

                if (point.X < 0)
                {
                    point.X = 10;
                }
                else if ((point.X + this._frmAddOccasionHeb.Width) > this.ParentForm.Right)
                {
                    point.X = this.ParentForm.Right - this._frmAddOccasionHeb.Width - 10;
                }

                if ((point.Y + this._frmAddOccasionHeb.Height) > (this.ParentForm.Bottom - 7))
                {
                    point.Y = this.ParentForm.Bottom - this._frmAddOccasionHeb.Height - 7;
                }

                this._frmAddOccasionHeb.Location = point;
                this._frmAddOccasionHeb.Show(this);
                this._frmAddOccasionHeb.BringToFront();

                while (this._frmAddOccasionHeb.Opacity < 1.0)
                {
                    this._frmAddOccasionHeb.Opacity += 0.05;
                }
                //this._frmAddOccasionHeb.RightToLeft = RightToLeft.Yes;
            }

            this._frmAddOccasionHeb.ResumeLayout();
        }

        private void SetDateDiff()
        {
            JewishDate now = new JewishDate(this._zmanim.Location);
            int diffDays = this._displayingJewishDate.AbsoluteDate - now.AbsoluteDate;

            if (diffDays == 0)
            {
                this.richTextBox1.SelectedText = "היום" + Environment.NewLine;
                return;
            }
            else if (diffDays == 1)
            {
                this.richTextBox1.SelectedText = "מחר" + Environment.NewLine;
                return;
            }
            else if (diffDays == 2)
            {
                this.richTextBox1.SelectedText = "מחרתיים" + Environment.NewLine;
                return;
            }
            else if (diffDays == -1)
            {
                this.richTextBox1.SelectedText = "אתמול" + Environment.NewLine;
                return;
            }

            int totalDays = Math.Abs(diffDays);

            if (diffDays < 0)
            {
                this.richTextBox1.SelectedText = "לפני " + totalDays.ToString("N0") + " ימים";
            }
            else
            {
                this.richTextBox1.SelectedText = "בעוד " + totalDays.ToString("N0") + " ימים";
            }

            if (totalDays > 29)
            {
                var dateDiff = new Itenso.TimePeriod.DateDiff(
                    this._displayingJewishDate.GregorianDate, now.GregorianDate);
                int years = Math.Abs(dateDiff.ElapsedYears),
                    months = Math.Abs(dateDiff.ElapsedMonths);

                if (years + months > 0)
                {
                    int singleDays = Math.Abs(dateDiff.ElapsedDays);
                    var diffText = new System.Text.StringBuilder();

                    if (years >= 1)
                    {
                        diffText.AppendFormat("{0:N0} {1}",
                            (int)years, years >= 2 ? "שנים" : "שנה");
                    }
                    if (months >= 1)
                    {
                        diffText.AppendFormat("{0}{1:N0} חודש{2}",

                            diffText.Length > 0 ? " " : "",
                            (int)months,
                            (months >= 2 ? "ים" : ""));
                    }
                    if (singleDays >= 1)
                    {
                        diffText.AppendFormat("{0}{1:N0} {2}",

                            (diffText.Length > 0 ? " " : ""),
                            (int)singleDays,
                            (singleDays >= 2 ? "ימים" : "יום"));
                    }
                    diffText.Insert(0, "    ");

                    this.richTextBox1.SelectionColor = Color.FromArgb(100, 90, 120);
                    this.richTextBox1.SelectionFont = this._dateDiffExpFont;
                    this.richTextBox1.SelectedText = diffText.ToString();
                }
            }

            this.richTextBox1.SelectedText = Environment.NewLine;
        }

        internal void ShowDateData()
        {
            this.Cursor = Cursors.WaitCursor;
            var dy = DafYomi.GetDafYomi(this._displayingJewishDate);
            var netzshkia = this._zmanim.GetNetzShkia();
            var netz = netzshkia[0];
            var shkia = netzshkia[1];
            var chatzos = this._zmanim.GetChatzos();
            var shaaZmanis = this._zmanim.GetShaaZmanis();
            var shaaZmanis90 = this._zmanim.GetShaaZmanis(90);
            var bold = new Font(this.richTextBox1.Font, FontStyle.Bold | FontStyle.Underline);

            this.richTextBox1.Clear();
            this.richTextBox1.SelectionFont = this._lineValueFont;
            this.richTextBox1.SelectionColor = Color.RoyalBlue;
            this.richTextBox1.SelectedText = this._displayingJewishDate.ToLongDateStringHeb() +
                Environment.NewLine;
            this.richTextBox1.SelectionColor = Color.LightSteelBlue;
            this.richTextBox1.SelectedText = this._displayingSecularDate.ToString("D", Program.HebrewCultureInfo) +
                Environment.NewLine;
            //If the secular day is a day behind as day being displayed is todays date and it is after sunset,
            //the user may get confused as the secular date for today and tomorrow will be the same.
            //So we esplain'in it to them...
            if (this._displayingSecularDate.Date != this._displayingJewishDate.GregorianDate.Date)
            {
                this.richTextBox1.SelectionFont = new Font(this._dateDiffExpFont, FontStyle.Regular);
                this.richTextBox1.SelectionColor = Color.RosyBrown;
                this.richTextBox1.SelectedText = "שים לב: תאריך הלועזי מתחיל בשעה 0:00" + Environment.NewLine;
            }
            this.SetDateDiff();
            if (this._holidays.Count() > 0)
            {
                foreach (var h in this._holidays)
                {
                    this.richTextBox1.SelectedText = Environment.NewLine + h.NameHebrew;
                    if (h.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = this._displayingJewishDate + 12;
                        this.richTextBox1.SelectedText = " - חודש " + Utils.GetProperMonthNameHeb(nextMonth.Year, nextMonth.Month);
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        var dim = JewishDateCalculations.DaysInJewishMonth(this._displayingJewishDate.Year, this._displayingJewishDate.Month);
                        var dow = dim - this._displayingJewishDate.Day;
                        if (dim == 30)
                        {
                            dow--;
                        }
                        this.richTextBox1.SelectedText = Environment.NewLine + "המולד: " + molad.ToStringHeb(this._zmanim.GetShkia());
                        this.richTextBox1.SelectedText = Environment.NewLine + "ראש חודש: " +
                            Utils.JewishDOWNames[dow] + (dim == 30 ? ", " +
                                Utils.JewishDOWNames[(dow + 1) % 7] : "");
                    }
                    else if (h.NameEnglish.Contains("Sefiras Ha'omer"))
                    {
                        this.richTextBox1.SelectedText = Environment.NewLine;
                        this.richTextBox1.SelectionFont = this._sefirahFont;
                        this.richTextBox1.SelectionColor = Color.SteelBlue;
                        this.richTextBox1.SelectedText = Utils.GetOmerNusach(this._displayingJewishDate.GetDayOfOmer(), true, false) + Environment.NewLine;
                    }
                    if (h.DayType.IsSpecialDayType(SpecialDayTypes.EruvTavshilin))
                    {
                        this.richTextBox1.SelectedText = Environment.NewLine;
                        this.richTextBox1.SelectionFont = this._lblOccasionFont ;
                        this.richTextBox1.SelectionColor = Color.Crimson;
                        this.richTextBox1.SelectedText = "עירוב תבשילין" + Environment.NewLine;
                    }
                }
                this.richTextBox1.SelectedText = Environment.NewLine;
                if (shkia != HourMinute.NoValue &&
                    this._holidays.Any(h => h.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting)))
                {
                    this.AddLine("הדלקת נרות", (shkia - this._zmanim.Location.CandleLighting).ToString24H());
                }
            }
            this.richTextBox1.SelectedText = Environment.NewLine;
            
            this.AddLine("פרשת השבוע",
                string.Join(" ", Sedra.GetSedra(this._displayingJewishDate, this._zmanim.Location.IsInIsrael).Select(i => i.nameHebrew)));
            if (dy != null)
            {
                this.AddLine("דף יומי", dy.ToStringHeb());
            }

            this.richTextBox1.SelectedText = Environment.NewLine;
            this.richTextBox1.SelectionBackColor = Color.LightSteelBlue;
            this.richTextBox1.SelectionColor = Color.GhostWhite;
            this.richTextBox1.SelectionFont = new Font(bold.FontFamily, 10f, FontStyle.Bold);

            this.richTextBox1.SelectedText = string.Format(" זמני היום ב{0}{1}", this._zmanim.Location.NameHebrew,
                new string(' ', 150 - this._zmanim.Location.Name.Length));

            this.richTextBox1.SelectionBackColor = this.richTextBox1.BackColor;
            this.richTextBox1.SelectedText = Environment.NewLine + Environment.NewLine;

            if (netz == HourMinute.NoValue)
            {
                this.AddLine("הנץ החמה", "השמש אינו עולה");
            }
            else
            {
                if (this._displayingJewishDate.Month == 1 && this._displayingJewishDate.Day == 14)
                {
                    this.AddLine("סו\"ז אכילת חמץ", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString24H());
                    this.AddLine("סו\"ז שריפת חמץ", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 5D)).ToString24H());
                    this.richTextBox1.SelectedText = Environment.NewLine;
                }

                this.AddLine("עלות השחר - 90", (netz - 90).ToString24H());
                this.AddLine("עלות השחר - 72", (netz - 72).ToString24H());
                this.AddLine("הנץ החמה", netz.ToString24H());
                this.AddLine("סוזק\"ש - מג\"א", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 3D)).ToString24H());
                this.AddLine("סוזק\"ש - הגר\"א", (netz + (int)Math.Floor(shaaZmanis * 3D)).ToString24H());
                this.AddLine("סוז\"ת - מג\"א", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString24H());
                this.AddLine("סוז\"ת - הגר\"א", (netz + (int)Math.Floor(shaaZmanis * 4D)).ToString24H());
            }
            if (netz != HourMinute.NoValue && shkia != HourMinute.NoValue)
            {
                this.AddLine("חצות היום והלילה", chatzos.ToString24H());
                this.AddLine("מנחה גדולה", (chatzos + (int)(shaaZmanis * 0.5)).ToString24H());
                this.AddLine("מנחה קטנה", (netz + (int)(shaaZmanis * 9.5)).ToString24H());
                this.AddLine("פלג המנחה", (netz + (int)(shaaZmanis * 10.75)).ToString24H());
            }
            if (shkia == HourMinute.NoValue)
            {
                this.AddLine("שקיעת החמה", "השמש אינו שוקע");
            }
            else
            {
                this.AddLine("שקיעת החמה", shkia.ToString24H());
                this.AddLine("צאת הכוכבים 45", (shkia + 45).ToString24H());
                this.AddLine("רבינו תם", (shkia + 72).ToString24H());
                this.AddLine("72 דקות זמניות", (shkia + (int)(shaaZmanis * 1.2)).ToString24H());
                this.AddLine("72 דקות זמניות לחומרה", (shkia + (int)(shaaZmanis90 * 1.2)).ToString24H());
            }

            this.tableLayoutPanel1.Controls.Clear();
            foreach (UserOccasion occ in this._occasions)
            {
                this.AddOccasion(occ);
            }

            var bg = (from o in this._occasions
                      where o.BackColor != Color.Empty
                      select o.BackColor).FirstOrDefault();

            this.tableLayoutPanel1.BackColor = (bg != Color.Empty ? bg.Color : Color.GhostWhite);

            this.Cursor = Cursors.Default;
        }

        #endregion private functions
    }
}