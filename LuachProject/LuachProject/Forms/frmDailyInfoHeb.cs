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
        public event EventHandler<JewishDate> OccasionWasChanged;

        #region private fields
        private DateTime _displayingSecularDate;
        private JewishDate _displayingJewishDate;
        private JewishCalendar.Zmanim _zmanim;
        private IEnumerable<SpecialDay> _holidays;
        private IEnumerable<UserOccasion> _occasions;
        private Font _lblOccasionFont;
        private Font _lineValueFont;
        private Font _dateDiffDaysFont;
        private Font _dateDiffExpFont;
        private Font _sefirahFont;
        #endregion

        public frmDailyInfoHeb(JewishDate jd, Location location)
        {
            this._displayingJewishDate = jd;
            this._zmanim = new Zmanim(jd, location);
            this._holidays = Zmanim.GetHolidays(jd, location.IsInIsrael).Cast<SpecialDay>();
            this._occasions = UserOccasionColection.FromSettings(jd);

            InitializeComponent();

            this._lblOccasionFont = this.flowLayoutPanel1.Font;
            this._lineValueFont = new Font(this.richTextBox1.Font, FontStyle.Bold);
            this._dateDiffDaysFont = new Font(this.richTextBox1.Font.FontFamily, 9f, FontStyle.Bold);
            this._dateDiffExpFont = new Font(this.richTextBox1.Font.FontFamily, 7.3f, FontStyle.Italic);
            this._sefirahFont = new Font(this.richTextBox1.Font.FontFamily, 9f);
        }

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
                        this._displayingSecularDate = this._displayingJewishDate.GetSecularDate(
                            (HourMinute)DateTime.Now.TimeOfDay, this._zmanim.Location);
                    }
                    else
                    {
                        this._displayingSecularDate = this._displayingJewishDate.GregorianDate;
                    }
                    this._zmanim.SecularDate = this._displayingSecularDate;
                    this._holidays = Zmanim.GetHolidays(value, this._zmanim.Location.IsInIsrael).Cast<SpecialDay>();
                    this._occasions = UserOccasionColection.FromSettings(this._displayingJewishDate);
                    this.flowLayoutPanel1.Controls.Clear();
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

        #region event handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            //If we are displaying todays date for the current time zone, we will show the "proper" secular date.
            if (this.IsHereAndNow())
            {
                this._displayingSecularDate = this._displayingJewishDate.GetSecularDate(
                    (HourMinute)DateTime.Now.TimeOfDay, this._zmanim.Location);
            }
            else
            {
                this._displayingSecularDate = this._displayingJewishDate.GregorianDate;
            }

            this.ShowDateData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.AddNewOccasion();
        }
        #endregion

        #region private functions
        public void AddNewOccasion()
        {
            var frm = new frmAddOccasionHeb { JewishDate = this._displayingJewishDate };
            this.PositionAddOccasion(frm);
            frm.OccasionWasChanged += delegate(object sndr, UserOccasion uo)
            {
                if (OccasionWasChanged != null)
                {
                    OccasionWasChanged(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));
                }

                if (UserOccasionColection.FromSettings(this.JewishDate).Contains(uo))
                {
                    this.AddOccasion(uo);
                    this.flowLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                }
            };
        }

        private void ShowDateData()
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

            this.Text = Utils.JewishDOWNames[(int)this._displayingJewishDate.DayOfWeek] +
                " " + this._displayingJewishDate.ToShortDateStringHeb();
            this.richTextBox1.Clear();
            this.richTextBox1.SelectionFont = this._lineValueFont;
            this.richTextBox1.SelectionColor = Color.RoyalBlue;
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
                        this.richTextBox1.SelectedText = " - חודש " + Utils.JewishMonthNamesHebrew[nextMonth.Month];
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        var dim = JewishDateCalculations.DaysInJewishMonth(this._displayingJewishDate.Year, this._displayingJewishDate.Month);
                        var dow = dim - this._displayingJewishDate.Day;
                        if(dim == 30)
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
                }
                this.richTextBox1.SelectedText = Environment.NewLine;
                if (shkia != HourMinute.NoValue && 
                    this._holidays.Any(h => h.DayType.HasFlag(SpecialDay.SpecialDayTypes.HasCandleLighting)))
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
            }
            foreach (UserOccasion occ in this._occasions)
            {
                this.AddOccasion(occ);
            }

            var bg = (from o in this._occasions
                      where o.BackColor != Color.Empty
                      select o.BackColor).FirstOrDefault();

            this.flowLayoutPanel1.BackColor = (bg != Color.Empty ? bg.Color : Color.GhostWhite);

            this.Cursor = Cursors.Default;
        }

        private void AddOccasion(UserOccasion occ)
        {
            var l = new LinkLabel
            {
                Text = occ.Name,
                Font = this._lblOccasionFont,
                Width = this.flowLayoutPanel1.Width,
                LinkColor = occ.Color,                
                AutoSize = false,
                AutoEllipsis = true,
                LinkBehavior = LinkBehavior.HoverUnderline
            };
            
            this.toolTip1.SetToolTip(l, occ.Notes);

            l.MouseClick += delegate
            {
                var frmAo = new frmAddOccasionHeb(occ);
                this.PositionAddOccasion(frmAo);
                frmAo.OccasionWasChanged += delegate(object sndr, UserOccasion uo)
               {
                   if (OccasionWasChanged != null)
                   {
                       OccasionWasChanged(this,  (uo != null ? uo.JewishDate : this._displayingJewishDate));
                   }
                   if (frmAo.UserOccasion == null || 
                   (!UserOccasionColection.FromSettings(this._displayingJewishDate).Contains(frmAo.UserOccasion)))
                   {
                       this.toolTip1.SetToolTip(l, null);
                       this.flowLayoutPanel1.Controls.Remove(l);
                   }
                   else
                   {
                       l.Text = frmAo.UserOccasion.Name;
                       l.LinkColor = frmAo.UserOccasion.Color;
                       this.toolTip1.SetToolTip(l, frmAo.UserOccasion.Notes);
                       this.flowLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                   }
               };
            };
            this.flowLayoutPanel1.Controls.Add(l);
        }

        private void AddLine(string header, string value)
        {
            this.richTextBox1.SelectionFont = this.richTextBox1.Font;
            this.richTextBox1.SelectedText = header.Trim() + " " +
                new string('.', ((header.Length + value.Length) < 30 ? 55 - header.Length : 10)) + " ";
            this.richTextBox1.SelectionFont = this._lineValueFont;
            this.richTextBox1.SelectionColor = Color.CornflowerBlue;
            this.richTextBox1.SelectedText = value.Trim() + Environment.NewLine;
        }

        private void PositionAddOccasion(Form frmAo)
        {
            frmAo.Show(this);
            frmAo.StartPosition = FormStartPosition.Manual;
            var pointZero = new Point(-frmAo.Width, this.ParentForm.Bottom - frmAo.Height - 7);

            frmAo.Location = pointZero;

            var a = 0;
            while (true)
            {
                if (frmAo.Width - a < 50)
                {
                    frmAo.Location = new Point(0, pointZero.Y);
                    break;
                }
                else
                {
                    frmAo.Location = new Point(pointZero.X + a, pointZero.Y);
                    frmAo.Refresh();
                    a += 50;
                }
            }

            frmAo.BringToFront();
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

        private bool IsHereAndNow()
        {
            return this._zmanim.Location.TimeZoneInfo != null &&
                TimeZoneInfo.Local.Id == this._zmanim.Location.TimeZoneInfo.Id &&
                new JewishDate(DateTime.Now, this._zmanim.Location) == this._displayingJewishDate;

        }
        #endregion
    }
}
