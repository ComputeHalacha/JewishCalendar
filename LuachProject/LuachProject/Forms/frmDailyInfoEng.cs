using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmDailyInfoEng : Form
    {
        #region events

        public event EventHandler<JewishDate> OccasionWasChanged;

        #endregion events

        #region private fields

        private Font _dateDiffDaysFont;
        private Font _dateDiffExpFont;
        private JewishDate _displayingJewishDate;
        private DateTime _displayingSecularDate;
        private frmAddOccasionEng _frmAddOccasionEng;
        private IEnumerable<SpecialDay> _holidays;
        private Font _lblOccasionFont;
        private Font _lineValueFont;
        private IEnumerable<UserOccasion> _occasions;
        private Font _sefirahFont;
        private JewishCalendar.Zmanim _zmanim;

        #endregion private fields

        #region constructor

        public frmDailyInfoEng(JewishDate jd, Location location)
        {
            this._displayingJewishDate = jd;
            this._zmanim = new Zmanim(jd, location);
            this._holidays = Zmanim.GetHolidays(jd, location.IsInIsrael).Cast<SpecialDay>();
            this._occasions = UserOccasionColection.FromSettings(jd);

            InitializeComponent();

            this._lblOccasionFont = new Font(this.Font, FontStyle.Bold);
            this._lineValueFont = new Font(this.richTextBox1.Font, FontStyle.Bold);
            this._dateDiffDaysFont = new Font(this.richTextBox1.Font.FontFamily, 9f, FontStyle.Bold);
            this._dateDiffExpFont = new Font(this.richTextBox1.Font.FontFamily, 7.3f, FontStyle.Italic);
            this._sefirahFont = new Font("Tahoma", 9f);
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
            AddNewOccasion(null);
        }

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

        #endregion event handlers

        #region private functions

        public void AddNewOccasion(Point? parentPoint)
        {
            if (this._frmAddOccasionEng != null)
            {
                this._frmAddOccasionEng.CloseStyle = frmAddOccasionEng.CloseStyles.None;
                this._frmAddOccasionEng.Close();
            }
            this._frmAddOccasionEng = new frmAddOccasionEng { JewishDate = this._displayingJewishDate };
            this.PositionAddOccasion(parentPoint);
            this._frmAddOccasionEng.OccasionWasChanged += delegate (object sndr, UserOccasion uo)
            {
                if (UserOccasionColection.FromSettings(this.JewishDate).Contains(uo))
                {
                    this.AddOccasion(uo);
                    this.tableLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                }

                if (OccasionWasChanged != null)
                {
                    OccasionWasChanged(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));
                }
            };
        }

        public void EditOccasion(UserOccasion occ, Point? parentPoint)
        {
            if (this._frmAddOccasionEng != null)
            {
                this._frmAddOccasionEng.CloseStyle = frmAddOccasionEng.CloseStyles.None;
                this._frmAddOccasionEng.Close();
            }
            this._frmAddOccasionEng = new frmAddOccasionEng(occ);

            LinkLabel lnkLbl = this.tableLayoutPanel1.Controls.OfType<LinkLabel>().First(ll => ll.Tag == occ);
            Label lbl = (Label)this.tableLayoutPanel1.Controls[this.tableLayoutPanel1.Controls.IndexOf(lnkLbl) + 1];

            this._frmAddOccasionEng.OccasionWasChanged += delegate (object sndr, UserOccasion uo)
            {
                if (OccasionWasChanged != null)
                {
                    OccasionWasChanged(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));
                }

                if (this._frmAddOccasionEng.UserOccasion == null ||
                    (!UserOccasionColection.FromSettings(this._displayingJewishDate).Contains(this._frmAddOccasionEng.UserOccasion)))
                {
                    this.tableLayoutPanel1.Controls.Remove(lbl);
                    this.tableLayoutPanel1.Controls.Remove(lnkLbl);
                    this.tableLayoutPanel1.RowCount -= 1;
                }
                else
                {
                    lnkLbl.Text = this._frmAddOccasionEng.UserOccasion.Name;
                    lnkLbl.LinkColor = this._frmAddOccasionEng.UserOccasion.Color;
                    var dateDiff = this._frmAddOccasionEng.UserOccasion.GetAnniversaryString(this.JewishDate, false);
                    lbl.Text = ((!string.IsNullOrWhiteSpace(dateDiff)) ? "(" + dateDiff + ") " : "") +
                        (this._frmAddOccasionEng.UserOccasion.Notes ?? "");
                    this.tableLayoutPanel1.BackColor = (uo.BackColor != Color.Empty ? uo.BackColor.Color : Color.GhostWhite);
                }
            };
            this.PositionAddOccasion(parentPoint);
        }

        private void AddLine(string header, string value)
        {
            this.richTextBox1.SelectionFont = this.richTextBox1.Font;
            this.richTextBox1.SelectedText = header.Trim() + " " +
                new string('.', ((header.Length + value.Length) < 35 ? 35 - header.Length : 5)) + " ";
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
                Dock = DockStyle.Fill,
                AutoEllipsis = true,
                LinkBehavior = LinkBehavior.HoverUnderline
            };
            var dateDiff = occ.GetAnniversaryString(this.JewishDate, false);
            var lbl = new Label
            {
                ForeColor = occ.Color,
                Dock = DockStyle.Fill,
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

        private void DisplayDateDiff()
        {
            JewishDate now = new JewishDate(this._zmanim.Location);
            int diffDays = this._displayingJewishDate.AbsoluteDate - now.AbsoluteDate;

            if (diffDays == 0)
            {
                this.richTextBox1.SelectedText = "Today" + Environment.NewLine;
                return;
            }
            else if (diffDays == 1)
            {
                this.richTextBox1.SelectedText = "Tommorrow" + Environment.NewLine;
                return;
            }
            else if (diffDays == -1)
            {
                this.richTextBox1.SelectedText = "Yesterday" + Environment.NewLine;
                return;
            }

            int totalDays = Math.Abs(diffDays);

            if (diffDays < 0)
            {
                this.richTextBox1.SelectedText = totalDays.ToString("N0") + " days ago";
            }
            else
            {
                this.richTextBox1.SelectedText = "In " + totalDays.ToString("N0") + " days";
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
                        diffText.AppendFormat("{0:N0} secular year{1}", years, years > 1 ? "s" : "");
                    }
                    if (months >= 1)
                    {
                        diffText.AppendFormat("{0}{1} secular month{2}",
                            diffText.Length > 0 ? " - " : "", months, months > 1 ? "s" : "");
                    }
                    if (singleDays >= 1)
                    {
                        diffText.AppendFormat("{0}{1} day{2}",
                            (diffText.Length > 0 ? " - " : ""), singleDays, singleDays > 1 ? "s" : "");
                    }
                    diffText.Insert(0, "    ");

                    this.richTextBox1.SelectionColor = Color.FromArgb(100, 90, 120);
                    this.richTextBox1.SelectionFont = this._dateDiffExpFont;
                    this.richTextBox1.SelectedText = diffText.ToString();
                }
            }

            this.richTextBox1.SelectedText = Environment.NewLine;
        }

        private void DisplayHolidays(HourMinute shkia)
        {
            if (this._holidays.Count() > 0)
            {
                foreach (var h in this._holidays)
                {
                    this.richTextBox1.SelectedText = Environment.NewLine + h.NameEnglish;
                    if (h.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = this._displayingJewishDate + 12;
                        this.richTextBox1.SelectedText = " - Chodesh " + nextMonth.MonthName;
                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        var dim = JewishDate.DaysInJewishMonth(this._displayingJewishDate.Year, this._displayingJewishDate.Month);
                        var dow = dim - this._displayingJewishDate.Day;
                        if (dim == 30)
                        {
                            dow--;
                        }
                        this.richTextBox1.SelectedText = Environment.NewLine + "Molad: " + molad.ToString(this._zmanim.GetShkia());
                        this.richTextBox1.SelectedText = Environment.NewLine + "Rosh Chodesh: " +
                            Utils.DaysOfWeek[dow] + (dim == 30 ? ", " + Utils.DaysOfWeek[(dow + 1) % 7] : "");
                    }
                }
                this.richTextBox1.SelectedText = Environment.NewLine;
                if (shkia != HourMinute.NoValue &&
                    this._holidays.Any(h => h.DayType.HasFlag(SpecialDay.SpecialDayTypes.HasCandleLighting)))
                {
                    this.AddLine("Candle Lighting", (shkia - this._zmanim.Location.CandleLighting).ToString());
                }

                if (this._holidays.Any(h => h.NameEnglish.Contains("Sefiras Ha'omer")))
                {
                    this.richTextBox1.SelectedText = Environment.NewLine;
                    this.richTextBox1.SelectionFont = this._sefirahFont;
                    this.richTextBox1.SelectionColor = Color.SteelBlue;
                    this.richTextBox1.SelectedText = Utils.GetOmerNusach(this._displayingJewishDate.GetDayOfOmer(), true, false) + Environment.NewLine;
                }
            }
        }

        private void DisplayToday()
        {
            this.richTextBox1.Clear();
            this.richTextBox1.SelectionFont = this._lineValueFont;
            this.richTextBox1.SelectionColor = Color.RoyalBlue;
            this.richTextBox1.SelectedText = this._displayingSecularDate.ToString("D", System.Threading.Thread.CurrentThread.CurrentCulture) +
                Environment.NewLine;
            //If the secular day is a day behind as day being displayed is todays date and it is after sunset,
            //the user may get confused as the secular date for today and tomorrow will be the same.
            //So we esplain'in it to them...
            if (this._displayingSecularDate.Date != this._displayingJewishDate.GregorianDate.Date)
            {
                this.richTextBox1.SelectionFont = new Font(this._dateDiffExpFont, FontStyle.Regular);
                this.richTextBox1.SelectionColor = Color.RosyBrown;
                this.richTextBox1.SelectedText = "Note: The secular date begins at midnight" + Environment.NewLine;
            }
        }

        private void DisplayZmanim(HourMinute netz, HourMinute shkia)
        {
            var dy = DafYomi.GetDafYomi(this._displayingJewishDate);
            var chatzos = this._zmanim.GetChatzos();
            var shaaZmanis = this._zmanim.GetShaaZmanis();
            var shaaZmanis90 = this._zmanim.GetShaaZmanis(90);
            var bold = new Font(this.richTextBox1.Font, FontStyle.Bold);

            this.richTextBox1.SelectedText = Environment.NewLine;
            this.AddLine("Weekly Sedra",
                string.Join(" ", Sedra.GetSedra(this._displayingJewishDate, this._zmanim.Location.IsInIsrael).Select(i => i.nameEng)));
            if (dy != null)
            {
                this.AddLine("Daf Yomi", dy.ToString());
            }
            this.richTextBox1.SelectedText = Environment.NewLine;
            this.richTextBox1.SelectionBackColor = Color.LightSteelBlue;
            this.richTextBox1.SelectionColor = Color.GhostWhite;
            this.richTextBox1.SelectionFont = new Font(bold.FontFamily, 10f, FontStyle.Bold);
            this.richTextBox1.SelectedText = string.Format(" Zmanim for {0}{1}", this._zmanim.Location.Name, new string(' ', 150 - this._zmanim.Location.Name.Length));
            this.richTextBox1.SelectionBackColor = this.richTextBox1.BackColor;
            this.richTextBox1.SelectedText = Environment.NewLine + Environment.NewLine;

            if (netz == HourMinute.NoValue)
            {
                this.AddLine("Netz Hachama", "The does not rise");
            }
            else
            {
                this.AddLine("Alos Hashachar - 90", (netz - 90).ToString());
                this.AddLine("Alos Hashachar - 72", (netz - 72).ToString());
                this.AddLine("Netz Hachama", netz.ToString());
                this.AddLine("Krias Shma - MG\"A", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 3D)).ToString());
                this.AddLine("Krias Shma - GR\"A", (netz + (int)Math.Floor(shaaZmanis * 3D)).ToString());
                this.AddLine("Zeman Tefillah - MG\"A", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString());
                this.AddLine("Zeman Tefillah - GR\"A", (netz + (int)Math.Floor(shaaZmanis * 4D)).ToString());
            }

            if (netz != HourMinute.NoValue && shkia != HourMinute.NoValue)
            {
                this.AddLine("Chatzos - Day & Night", chatzos.ToString());
                this.AddLine("Mincha Gedolah", (chatzos + (int)(shaaZmanis * 0.5)).ToString());
            }

            if (shkia == HourMinute.NoValue)
            {
                this.AddLine("Shkias Hachama", "The sun does not set");
            }
            else
            {
                this.AddLine("Shkias Hachama", shkia.ToString());
                this.AddLine("Nightfall 45", (shkia + 45).ToString());
                this.AddLine("Rabbeinu Tam", (shkia + 72).ToString());
            }
        }

        private bool IsHereAndNow()
        {
            return this._zmanim.Location.TimeZoneInfo != null &&
                TimeZoneInfo.Local.Id == this._zmanim.Location.TimeZoneInfo.Id &&
                new JewishDate(DateTime.Now, this._zmanim.Location) == this._displayingJewishDate;
        }

        private void PositionAddOccasion(Point? parentPoint)
        {
            this._frmAddOccasionEng.StartPosition = FormStartPosition.Manual;
            this._frmAddOccasionEng.SuspendLayout();

            if (parentPoint == null)
            {
                this._frmAddOccasionEng.CloseStyle = frmAddOccasionEng.CloseStyles.Slide;

                Point pointZero = new Point(this.ParentForm.Right, this.ParentForm.Bottom - this._frmAddOccasionEng.Height - 7);
                int a = 0;

                this._frmAddOccasionEng.Location = pointZero;
                this._frmAddOccasionEng.Show(this);

                while (true)
                {
                    if (this._frmAddOccasionEng.Width - a < 50)
                    {
                        this._frmAddOccasionEng.Location = new Point(pointZero.X - this._frmAddOccasionEng.Width - 10, pointZero.Y);
                        break;
                    }
                    else
                    {
                        this._frmAddOccasionEng.Location = new Point(pointZero.X - a, pointZero.Y);
                        this._frmAddOccasionEng.Refresh();
                        a += 50;
                    }
                }
                this._frmAddOccasionEng.BringToFront();
            }
            else
            {
                this._frmAddOccasionEng.CloseStyle = frmAddOccasionEng.CloseStyles.Fade;

                var point = parentPoint.Value;

                if (point.X < 0)
                {
                    point.X = 10;
                }
                else if ((point.X + this._frmAddOccasionEng.Width) > this.ParentForm.Right)
                {
                    point.X = this.ParentForm.Right - this._frmAddOccasionEng.Width - 10;
                }

                if ((point.Y + this._frmAddOccasionEng.Height) > (this.ParentForm.Bottom - 7))
                {
                    point.Y = this.ParentForm.Bottom - this._frmAddOccasionEng.Height - 7;
                }

                this._frmAddOccasionEng.Location = point;
                this._frmAddOccasionEng.Show(this);
            }

            this._frmAddOccasionEng.ResumeLayout();
        }

        private void ShowDateData()
        {
            this.Cursor = Cursors.WaitCursor;
            var netzshkia = this._zmanim.GetNetzShkia();
            var netz = netzshkia[0];
            var shkia = netzshkia[1];

            this.DisplayToday();
            this.DisplayDateDiff();
            this.DisplayHolidays(shkia);
            this.DisplayZmanim(netz, shkia);

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