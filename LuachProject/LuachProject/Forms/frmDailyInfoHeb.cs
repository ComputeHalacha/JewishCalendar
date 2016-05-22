using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace LuachProject
{
    public partial class frmDailyInfoHeb : Form
    {
        #region events

        public event EventHandler<JewishDate> OccasionWasChanged;

        #endregion events

        #region private fields                
        private JewishDate _displayingJewishDate;
        private DateTime _displayingSecularDate;
        private DateTime _secularDateAtMidnight;
        private frmAddOccasionHeb _frmAddOccasionHeb;
        private IEnumerable<SpecialDay> _holidays;
        private Font _lblOccasionFont;
        private IEnumerable<UserOccasion> _occasions;
        private JewishCalendar.Zmanim _zmanim;
        #endregion private fields

        #region constructor
        public frmDailyInfoHeb(JewishDate jd, Location location)
        {
            this._displayingJewishDate = jd;
            this.SetSecularDate();
            this._zmanim = new Zmanim(this._secularDateAtMidnight, location);
            this._holidays = Zmanim.GetHolidays(jd, location.IsInIsrael).Cast<SpecialDay>();
            this._occasions = UserOccasionColection.FromSettings(jd);

            InitializeComponent();

            this._lblOccasionFont = new Font(this.tableLayoutPanel1.Font, FontStyle.Bold);
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
                    this._zmanim.SecularDate = this._secularDateAtMidnight;
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
            this.ShowDateData();
        }
        #endregion event handlers

        #region public and internal functions
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

        internal void ShowDateData()
        {
            this.Cursor = Cursors.WaitCursor;
            var dy = DafYomi.GetDafYomi(this._displayingJewishDate);
            var netzshkia = this._zmanim.GetNetzShkia(true);
            var netzshkiaMishor = this._zmanim.GetNetzShkia(false);
            var netz = netzshkia[0];
            var shkia = netzshkia[1];
            var netzMishor = netzshkiaMishor[0];
            var shkiaMishor = netzshkiaMishor[1];
            var chatzos = this._zmanim.GetChatzos();
            var shaaZmanis = this._zmanim.GetShaaZmanis();
            var shaaZmanis90 = this._zmanim.GetShaaZmanis(90);
            var html = new StringBuilder();

            html.AppendFormat("<div class=\"padWidth royalBlue bold\">{0}</div>",
                this._displayingJewishDate.ToLongDateStringHeb());
            html.AppendFormat("<div class=\"padWidth lightSteelBlue\">{0}</div>",
                this._displayingSecularDate.ToString("D", Program.HebrewCultureInfo));

            //If the secular day is a day behind as day being displayed is todays date and it is after sunset,
            //the user may get confused as the secular date for today and tomorrow will be the same.
            //So we esplain'in it to them...
            if (this._secularDateAtMidnight.Date != this._displayingSecularDate.Date)
            {
                html.Append("<div class=\"padWidth rosyBrown seven italic\">שים לב: תאריך הלועזי מתחיל בשעה 0:00</div>");
            }

            this.DisplayDateDiff(html);

            html.Append("<br />");
            if (this._holidays.Count() > 0)
            {
                foreach (var h in this._holidays)
                {
                    html.AppendFormat("<div class=\"padWidth\">{0}", h.NameHebrew);
                    if (h.NameEnglish == "Shabbos Mevarchim")
                    {
                        var nextMonth = this._displayingJewishDate + 12;
                        html.AppendFormat(" - חודש {0}", Utils.GetProperMonthNameHeb(nextMonth.Year, nextMonth.Month));

                        var molad = Molad.GetMolad(nextMonth.Month, nextMonth.Year);
                        var dim = JewishDateCalculations.DaysInJewishMonth(this._displayingJewishDate.Year, this._displayingJewishDate.Month);
                        var dow = dim - this._displayingJewishDate.Day;
                        if (dim == 30)
                        {
                            dow--;
                        }
                        html.AppendFormat("<div>המולד: {0}</div>", molad.ToStringHeb(this._zmanim.GetShkia()));
                        html.AppendFormat("<div>ראש חודש: {0}{1}</div>",
                            Utils.JewishDOWNames[dow], (dim == 30 ? ", " + Utils.JewishDOWNames[(dow + 1) % 7] : ""));
                    }
                    html.Append("</div>");
                    if (h.NameEnglish.Contains("Sefiras Ha'omer"))
                    {
                        html.AppendFormat("<div class=\"nine bluoid\">{0}</div>",
                            Utils.GetOmerNusach(this._displayingJewishDate.GetDayOfOmer(), true, false));
                    }

                    if (h.DayType.IsSpecialDayType(SpecialDayTypes.EruvTavshilin))
                    {
                        html.Append("<div class=\"padWidth crimson bold\">עירוב תבשילין</div>");
                    }
                }
            }

            html.Append("<table>");

            if (shkia != HourMinute.NoValue &&
                    this._holidays.Any(h => h.DayType.IsSpecialDayType(SpecialDayTypes.HasCandleLighting)))
            {
                this.AddLine(html, "הדלקת נרות", (shkia - this._zmanim.Location.CandleLighting).ToString24H(),
                    wideDescription: false);
                html.Append("<tr><td class=\"nobg\" colspan=\"3\">&nbsp;</td></tr>");
            }

            this.AddLine(html, "פרשת השבוע",
                string.Join(" ", Sedra.GetSedra(this._displayingJewishDate, this._zmanim.Location.IsInIsrael).Select(i => i.nameHebrew)),
                wideDescription: false);
            if (dy != null)
            {
                this.AddLine(html, "דף יומי", dy.ToStringHeb(), wideDescription: false);
            }

            html.Append("</table><br />");
            html.AppendFormat("<div class=\"padBoth lightSteelBlueBG ghostWhite nine bold clear\">זמני היום ב{0}</div>",
                this._zmanim.Location.NameHebrew);
            html.Append("<table>");

            if (netz == HourMinute.NoValue)
            {
                this.AddLine(html, "הנץ החמה", "השמש אינו עולה", bold: true);
            }
            else
            {
                if (this._displayingJewishDate.Month == 1 && this._displayingJewishDate.Day == 14)
                {
                    this.AddLine(html, "סו\"ז אכילת חמץ", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString24H(),
                        bold: true);
                    this.AddLine(html, "סו\"ז שריפת חמץ", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 5D)).ToString24H(),
                        bold: true);
                    html.Append("<br />");
                }

                this.AddLine(html, "עלות השחר - 90", (netzMishor - 90).ToString24H());
                this.AddLine(html, "עלות השחר - 72", (netzMishor - 72).ToString24H());
                this.AddLine(html, "הנה\"ח - מ " + this._zmanim.Location.Elevation.ToString() + " מטר",
                    netz.ToString24H(), bold: true);
                if (netz != netzMishor)
                {
                    this.AddLine(html, "הנה\"ח -  גובה פני הים",
                        netzMishor.ToString24H());
                }
                this.AddLine(html, "סוזק\"ש - מג\"א", ((netzMishor - 90) + (int)Math.Floor(shaaZmanis90 * 3D)).ToString24H());
                this.AddLine(html, "סוזק\"ש - הגר\"א", (netzMishor + (int)Math.Floor(shaaZmanis * 3D)).ToString24H());
                this.AddLine(html, "סוז\"ת - מג\"א", ((netzMishor - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString24H());
                this.AddLine(html, "סוז\"ת - הגר\"א", (netzMishor + (int)Math.Floor(shaaZmanis * 4D)).ToString24H());
            }
            if (netz != HourMinute.NoValue && shkia != HourMinute.NoValue)
            {
                this.AddLine(html, "חצות היום והלילה", chatzos.ToString24H());
                this.AddLine(html, "מנחה גדולה", (chatzos + (int)(shaaZmanis * 0.5)).ToString24H());
                this.AddLine(html, "מנחה קטנה", (netzMishor + (int)(shaaZmanis * 9.5)).ToString24H());
                this.AddLine(html, "פלג המנחה", (netzMishor + (int)(shaaZmanis * 10.75)).ToString24H());
            }
            if (shkia == HourMinute.NoValue)
            {
                this.AddLine(html, "שקיעת החמה", "השמש אינו שוקע", bold: true);
            }
            else
            {
                if (shkia != shkiaMishor)
                {
                    this.AddLine(html, "שקה\"ח - מ " + this._zmanim.Location.Elevation.ToString() + " מטר",
                        shkiaMishor.ToString24H());
                }
                this.AddLine(html, "שקה\"ח - גובה פני הים", shkia.ToString24H(), bold: true);
                this.AddLine(html, "צאת הכוכבים 45", (shkia + 45).ToString24H());
                this.AddLine(html, "רבינו תם", (shkia + 72).ToString24H());
                this.AddLine(html, "72 דקות זמניות", (shkia + (int)(shaaZmanis * 1.2)).ToString24H());
                this.AddLine(html, "72 דקות זמניות לחומרה", (shkia + (int)(shaaZmanis90 * 1.2)).ToString24H());
            }
            html.Append("</table>");
            this.webBrowser1.DocumentText = Properties.Resources.InfoHTMLHeb
                .Replace("{{BODY}}", html.ToString());

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
        private void AddLine(StringBuilder sb, string header, string value, bool wideDescription = true, bool bold = false)
        {
            sb.Append("<tr>");
            sb.AppendFormat("<td class=\"{0}{1}\"><span>{2}</span></td><td>&nbsp;</td>",
                (wideDescription ? "wide" : "medium"),
                (bold ? " bold" : ""), header);
            sb.AppendFormat("<td class=\"{0} cornFlowerBlue bold nobg\">{1}</td>",
                (wideDescription ? "narrow" : "medium"),
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

        private void DisplayDateDiff(StringBuilder html)
        {
            JewishDate now = new JewishDate(this._zmanim.Location);
            int diffDays = this._displayingJewishDate.AbsoluteDate - now.AbsoluteDate;

            html.Append("<div class=\"padWidth\">");

            if (diffDays == 0)
            {
                html.Append("היום");
            }
            else if (diffDays == 1)
            {
                html.Append("מחר");
            }
            else if (diffDays == 2)
            {
                html.Append("מחרתיים");
            }
            else if (diffDays == -1)
            {
                html.Append("אתמול");
            }
            else
            {
                int totalDays = Math.Abs(diffDays);

                if (diffDays < 0)
                {
                    html.AppendFormat("לפני {0:N0} ימים", totalDays);
                }
                else
                {
                    html.AppendFormat("בעוד {0:N0} ימים", totalDays);
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
                            html.AppendFormat("{0:N0} {1}", years, years >= 2 ? "שנים" : "שנה");
                        }
                        if (months >= 1)
                        {
                            html.AppendFormat(" {0:N0} {1}", months, (months >= 2 ? "חודשים" : "חודש"));
                        }
                        if (singleDays >= 1)
                        {
                            html.AppendFormat(" {0:N0} {1}", singleDays, (singleDays >= 2 ? "ימים" : "יום"));
                        }

                        html.Append("</span>");
                    }
                }
            }

            html.Append("</div>");
        }
        #endregion private functions        
    }
}