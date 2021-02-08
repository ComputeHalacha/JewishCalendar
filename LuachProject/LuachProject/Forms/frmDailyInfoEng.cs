using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace LuachProject
{
    public partial class frmDailyInfoEng : Form
    {
        #region events

        public event EventHandler<JewishDate> OccasionWasChanged;

        #endregion events

        #region private fields
        private JewishDate _displayingJewishDate;
        private DateTime _displayingSecularDate;
        private DateTime _secularDateAtMidnight;
        private frmAddOccasionEng _frmAddOccasionEng;
        private IEnumerable<SpecialDay> _holidays;
        private readonly Font _lblOccasionFont;
        private IEnumerable<UserOccasion> _occasions;
        private readonly DailyZmanim _dailyZmanim;
        private string _html;
        #endregion private fields

        #region constructor

        public frmDailyInfoEng(JewishDate jd, Location location)
        {
            this._displayingJewishDate = jd;
            this._dailyZmanim = new DailyZmanim(jd.GregorianDate, location);
            this._holidays = Zmanim.GetHolidays(jd, location.IsInIsrael).Cast<SpecialDay>();
            this._occasions = UserOccasionColection.FromSettings(jd);

            InitializeComponent();

            this._lblOccasionFont = new Font(this.Font, FontStyle.Bold);
            this.webBrowser1.ObjectForScripting = new ScriptingObject();
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
                    this.SetSecularDate();
                    this._dailyZmanim.SecularDate = this._secularDateAtMidnight;
                    this._holidays = Zmanim.GetHolidays(value, this._dailyZmanim.Location.IsInIsrael).Cast<SpecialDay>();
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
                return this._dailyZmanim.Location;
            }
            set
            {
                this._dailyZmanim.SecularDate = this._secularDateAtMidnight;
                this._dailyZmanim.Location = value;
                this._holidays = Zmanim.GetHolidays(this._displayingJewishDate,
                    this._dailyZmanim.Location.IsInIsrael).Cast<SpecialDay>();
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
            this.SetSecularDate();
            this.ShowDateData();
        }
        #endregion event handlers

        #region public and internal functions
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

                OccasionWasChanged?.Invoke(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));
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
                OccasionWasChanged?.Invoke(this, (uo != null ? uo.JewishDate : this._displayingJewishDate));

                if (this._frmAddOccasionEng.UserOccasion == null ||
                    (!UserOccasionColection.FromSettings(this._displayingJewishDate).Contains(this._frmAddOccasionEng.UserOccasion)))
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

        internal void ShowDateData()
        {
            this.Cursor = Cursors.WaitCursor;
            var html = new StringBuilder();

            this.DisplayToday(html);
            this.DisplayDateDiff(html);
            html.Append("<br />");
            this.DisplayHolidays(html);
            this.DisplayZmanim(html);
            this._html = Properties.Resources.InfoHTMLEng
                .Replace("{{BODY}}", html.ToString());
            this.webBrowser1.DocumentText = this._html;

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
        #endregion

        #region private functions       
        private void AddLine(StringBuilder sb, string header, string value, bool wideDescription = true, bool bold = false, bool emphasizeValue = false)
        {
            sb.Append("<tr>");
            sb.AppendFormat("<td class=\"{0}{1}\"><span>{2}</span></td><td>&nbsp;</td>",
                (wideDescription ? "wide" : "medium"),
                (bold ? " bold" : ""), header);
            sb.AppendFormat("<td class=\"{0} {1} bold nobg\">{2}</td>",
                (wideDescription ? "narrow" : "medium"),
                (emphasizeValue ? "crimson" : "cornFlowerBlue"),
                value);
            sb.Append("</tr>");
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

        private void DisplayDateDiff(StringBuilder html)
        {
            JewishDate now = new JewishDate(this._dailyZmanim.Location);
            int diffDays = this._displayingJewishDate.AbsoluteDate - now.AbsoluteDate;

            html.Append("<div class=\"padWidth\">");

            if (diffDays == 0)
            {
                html.Append("Today");
            }
            else if (diffDays == 1)
            {
                html.Append("Tommorrow");
            }
            else if (diffDays == -1)
            {
                html.Append("Yesterday");
            }
            else
            {
                int totalDays = Math.Abs(diffDays);

                if (diffDays < 0)
                {
                    html.AppendFormat("{0:N0} days ago", totalDays);
                }
                else
                {
                    html.AppendFormat("In {0:N0} days", totalDays);
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

                        html.Append("&nbsp;&nbsp;<span class=\"purpleoid seven italic\">");

                        if (years >= 1)
                        {
                            html.AppendFormat("{0:N0} secular year{1}", years, years > 1 ? "s" : "");
                        }
                        if (months >= 1)
                        {
                            html.AppendFormat(" {0:N0} secular month{1}", months, months > 1 ? "s" : "");
                        }
                        if (singleDays >= 1)
                        {
                            html.AppendFormat(" {0:N0} day{1}", singleDays, singleDays > 1 ? "s" : "");
                        }

                        html.Append("</span>");
                    }
                }
            }
            html.Append("</div>");
        }

        private void DisplayHolidays(StringBuilder html)
        {
            var shkia = this._dailyZmanim.ShkiaAtElevation;
            if (this._holidays.Count() > 0)
            {
                foreach (var h in this._holidays)
                {
                    html.AppendFormat("<div class=\"padWidth\">{0}", h.NameEnglish);
                    if (h.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = this._displayingJewishDate + 12;
                        html.AppendFormat(" - Chodesh {0}", nextMonth.MonthName);

                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        var dim = JewishDateCalculations.DaysInJewishMonth(this._displayingJewishDate.Year, this._displayingJewishDate.Month);
                        var dow = dim - this._displayingJewishDate.Day;
                        if (dim == 30)
                        {
                            dow--;
                        }
                        html.AppendFormat("<div>Molad: {0}</div>", molad.ToString(this._dailyZmanim.ShkiaAtElevation));
                        html.AppendFormat("<div>Rosh Chodesh: {0}{1}</div>",
                             Utils.DaysOfWeek[dow], (dim == 30 ? ", " + Utils.DaysOfWeek[(dow + 1) % 7] : ""));
                    }
                    html.Append("</div>");

                    if (h.NameEnglish.Contains("Sefiras Ha'omer"))
                    {
                        var dayOfOmer = this._displayingJewishDate.GetDayOfOmer();
                        html.AppendFormat("<div><a onclick=\"javacript:window.external.showSefirah({0}, false);return false;\" class=\"tahoma nine steelBlue pointer\">{1}</a></div>",
                          dayOfOmer, Utils.GetOmerNusach(dayOfOmer, Properties.Settings.Default.Nusach));
                    }

                    if (h.DayType.IsSpecialDayType(SpecialDayTypes.EruvTavshilin))
                    {
                        html.Append("<div class=\"padWidth eight bold crimson\">Eiruv Tavshilin</div>");
                    }
                }
            }
            html.Append("<table>");
            if (shkia != TimeOfDay.NoValue &&
                   this._holidays.Any(h => h.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting)))
            {
                this.AddLine(html, "Candle Lighting", (shkia - this._dailyZmanim.Location.CandleLighting).ToString(), wideDescription: false);
                html.Append("<tr><td class=\"nobg\" colspan=\"3\">&nbsp;</td></tr>");
            }
        }

        private void DisplayToday(StringBuilder html)
        {
            html.AppendFormat("<div class=\"padWidth royalBlue bold\">{0}, {1}</div>",
                this._displayingJewishDate.DayOfWeek,
                this._displayingJewishDate.ToLongDateString());
            html.AppendFormat("<div class=\"padWidth lightSteelBlue\">{0}</div>",
                this._displayingSecularDate.ToString("D", System.Threading.Thread.CurrentThread.CurrentCulture));


            //If the secular day is a day behind as day being displayed is todays date and it is after sunset,
            //the user may get confused as the secular date for today and tomorrow will be the same.
            //So we esplain'in it to them...
            if (this._secularDateAtMidnight.Date != this._displayingSecularDate.Date)
            {
                html.Append("<div class=\"padWidth rosyBrown seven italic\">Note: The secular date begins at midnight</div>");
            }
        }

        private void DisplayZmanim(StringBuilder html)
        {
            var showSeconds = Properties.Settings.Default.ShowSeconds;
            var dy = DafYomi.GetDafYomi(this._displayingJewishDate);
            var netz = this._dailyZmanim.NetzAtElevation;
            var shkia = this._dailyZmanim.ShkiaAtElevation;
            var netzMishor = this._dailyZmanim.NetzMishor;
            var shkiaMishor = this._dailyZmanim.ShkiaMishor;
            var chatzos = this._dailyZmanim.Chatzos;
            var shaaZmanis = this._dailyZmanim.ShaaZmanis;
            var shaaZmanis90 = this._dailyZmanim.ShaaZmanisMga;
            var feet = "...at " + (this._dailyZmanim.Location.Elevation * 3.28084).ToString("N0") +
                " ft.";

            this.AddLine(html, "Weekly Sedra",
                string.Join(" ", Sedra.GetSedra(this._displayingJewishDate, this._dailyZmanim.Location.IsInIsrael).Select(i => i.nameEng)),
                wideDescription: false);
            if (dy != null)
            {
                this.AddLine(html, "Daf Yomi", dy.ToString(), wideDescription: false);
            }

            html.Append("</table><br />");
            html.AppendFormat("<div class=\"padBoth lightSteelBlueBG ghostWhite nine bold clear\">Zmanim for {0}</div>",
                this._dailyZmanim.Location.Name);
            html.Append("<table>");

            if (netz == TimeOfDay.NoValue)
            {
                this.AddLine(html, "Netz Hachama", "The does not rise", bold: true, emphasizeValue: true);
            }
            else
            {
                if (this._displayingJewishDate.Month == 1 && this._displayingJewishDate.Day == 14)
                {
                    this.AddLine(html, "Stop eating Chometz", ((netz - 90d) + (shaaZmanis90 * 4d)).ToString24H(showSeconds),
                        bold: true);
                    this.AddLine(html, "Burn Chometz before", ((netz - 90d) + (shaaZmanis90 * 5d)).ToString24H(showSeconds),
                        bold: true);
                    html.Append("<br />");
                }

                this.AddLine(html, "Alos Hashachar - 90", (netzMishor - 90d).ToString(showSeconds));
                this.AddLine(html, "Alos Hashachar - 72", (netzMishor - 72d).ToString(showSeconds));
                if (netz != netzMishor)
                {
                    this.AddLine(html, "Sunrise <em class=\"reg lightSteelBlue\">" + feet + "</em>", netz.ToString(showSeconds));
                }
                this.AddLine(html, "Netz Hachama", netzMishor.ToString(showSeconds), bold: true, emphasizeValue: true);

                this.AddLine(html, "Krias Shma - MG\"A", this._dailyZmanim.GetZman(ZmanType.KShmMga).ToString(showSeconds));
                this.AddLine(html, "Krias Shma - GR\"A", this._dailyZmanim.GetZman(ZmanType.KshmGra).ToString(showSeconds));
                this.AddLine(html, "Zeman Tefillah - MG\"A", this._dailyZmanim.GetZman(ZmanType.TflMga).ToString(showSeconds));
                this.AddLine(html, "Zeman Tefillah - GR\"A", this._dailyZmanim.GetZman(ZmanType.TflGra).ToString(showSeconds));
            }

            if (netz != TimeOfDay.NoValue && shkia != TimeOfDay.NoValue)
            {
                this.AddLine(html, "Chatzos - Day & Night", chatzos.ToString(showSeconds));
                this.AddLine(html, "Mincha Gedolah", this._dailyZmanim.GetZman(ZmanType.MinchaG).ToString(showSeconds));
                this.AddLine(html, "Mincha Ktanah", this._dailyZmanim.GetZman(ZmanType.MinchaK).ToString(showSeconds));
                this.AddLine(html, "Plag Hamincha", this._dailyZmanim.GetZman(ZmanType.MinchaPlg).ToString(showSeconds));
            }

            if (shkia == TimeOfDay.NoValue)
            {
                this.AddLine(html, "Shkias Hachama", "The sun does not set", bold: true, emphasizeValue: true);
            }
            else
            {
                if (shkia == shkiaMishor)
                {
                    this.AddLine(html, "Shkias Hachama", shkia.ToString(showSeconds), bold: true, emphasizeValue: true);
                }
                else
                {
                    this.AddLine(html, "Sunset <em class=\"reg lightSteelBlue\">...at Sea Level</em>", shkiaMishor.ToString(showSeconds));
                    this.AddLine(html, "Shkiah <em class=\"reg lightSteelBlue\"> " + feet + "</em>",
                        shkia.ToString(showSeconds), bold: true, emphasizeValue: true);
                }
                this.AddLine(html, "Nightfall 45", (shkia + 45d).ToString(showSeconds));
                this.AddLine(html, "Rabbeinu Tam", (shkia + 72d).ToString(showSeconds));
                this.AddLine(html, "72 \"Zmaniot\"", (shkia + (shaaZmanis * 1.2)).ToString(showSeconds));
                this.AddLine(html, "72 \"Zmaniot MA\"", (shkia + (shaaZmanis90 * 1.2)).ToString(showSeconds));
            }
            html.Append("</table>");
        }

        private void SetSecularDate()
        {
            this._displayingSecularDate = this._displayingJewishDate.GregorianDate;
            /*-------------------------------------------------------------------------------------------------------------------------------
             * The zmanim shown will always be for the Gregorian Date that starts at midnight of the current Jewish Date.
             * We use the JewishDateCalculations.GetGregorianDateFromJewishDate function 
             * which gets the Gregorian Date that will be at midnight of the given Jewish day.  
            ----------------------------------------------------------------------------------------------------------------------------------*/
            this._secularDateAtMidnight = JewishDateCalculations.GetGregorianDateFromJewishDate(this._displayingJewishDate);
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
        #endregion private functions
    }
}