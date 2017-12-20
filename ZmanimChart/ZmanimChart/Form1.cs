using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ZmanimChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            PropertyInfo aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);

            aProp.SetValue(this, true, null);

            this.FillLocations();
            this.FillDateCombos();
            this.clmZman.DataSource = Program.ZmanTypesList;
            this.SetDOWFormat();
            this.FillZmanTypeRows();
            this.choiceArmy.ChoiceOneSelected = Properties.Settings.Default.ArmyTime;
            this.choiceAmPm.ChoiceOneSelected = Properties.Settings.Default.AmPm;
            this.choiceWidth100.SelectedValue = Properties.Settings.Default.Width100;
            this.choiceDirection.SelectedValue = Properties.Settings.Default.DirectionRight;
            this.choiceSwitcherDateType.ChoiceOneSelected = Properties.Settings.Default.DateChooseMonth;
        }

        private void FillDateCombos()
        {
            this.cmbMonth.ValueMember = "Key";
            this.cmbMonth.DisplayMember = "Value";
            this.cmbYear.ValueMember = "Key";
            this.cmbYear.DisplayMember = "Value";

            var now = new JewishDate();
            var nextMonth = now.AddMonths(1);
            KeyValuePair<int, string> month = new KeyValuePair<int, string>();
            KeyValuePair<int, string> year = new KeyValuePair<int, string>();
            for (int i = nextMonth.Year - 2; i <= nextMonth.Year + 2; i++)
            {
                var kvp = new KeyValuePair<int, string>(i,
                                    Utils.ToNumberHeb(i % 1000));
                this.cmbYear.Items.Add(kvp);
                if (i == nextMonth.Year)
                {
                    year = kvp;
                }
            }
            for (int i = 1; i <= 13; i++)
            {
                var kvp = new KeyValuePair<int, string>(i,
                    Utils.JewishMonthNamesHebrew[i]);
                this.cmbMonth.Items.Add(kvp);
                if (i == nextMonth.Month)
                {
                    month = kvp;
                }
            }
            this.cmbMonth.SelectedItem = month;
            this.cmbYear.SelectedItem = year;
            this.jdpFrom.Value = now;
            this.jdpTo.Value = nextMonth;
        }

        private void FillZmanTypeRows()
        {
            if (Properties.Settings.Default.SelectedZmanRows != null)
            {
                foreach (SingleZmanRow r in Properties.Settings.Default.SelectedZmanRows.OrderBy(sr => sr.ZmanIndex))
                {
                    this.dataGridView1.Rows.Add(new object[]
                    {
                        Program.ZmanTypesList[r.ZmanIndex],
                        r.Offset,
                        r.Header,
                        r.Bold
                    });
                }
            }
        }

        private void SetDOWFormat()
        {
            switch (Properties.Settings.Default.DOWFormat)
            {
                case DayOfWeekFormat.JewishNum:
                    this.rbDOWJewishNum.Checked = true;
                    break;
                case DayOfWeekFormat.Number:
                    this.rbDowNum.Checked = true;
                    break;
                case DayOfWeekFormat.Full:
                    rbDayOfWeekFull.Checked = true;
                    break;
                case DayOfWeekFormat.English:
                    rbDOWEnglish.Checked = true;
                    break;
                case DayOfWeekFormat.None:
                    rbDOWNone.Checked = true;
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void FillLocations()
        {
            this.cmbLocations.DisplayMember = "NameHebrew";
            this.cmbLocations.Items.Clear();
            bool israel = this.choiceSwitcher1.ChoiceTwoSelected;
            var list = Program.LocationsList.Where(l => l.IsInIsrael == israel);
            foreach (Location location in list)
            {
                this.cmbLocations.Items.Add(location);
                if (location.Name == Properties.Settings.Default.LocationName)
                {
                    this.cmbLocations.SelectedItem = location;
                }
            }
        }

        private int GetSelectedMonth()
        {
            return ((KeyValuePair<int, string>)this.cmbMonth.SelectedItem).Key;
        }

        private int GetSelectedYear()
        {
            return ((KeyValuePair<int, string>)this.cmbYear.SelectedItem).Key;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (this.cmbLocations.SelectedItem != null)
            {
                var html = this.GetHtml();
                if (!string.IsNullOrWhiteSpace(html))
                {
                    var b = new frmBrowser();
                    b.webBrowser1.DocumentText = html;
                    b.Show();
                }
            }
        }

        private string GetHtml()
        {
            Location location = (Location)this.cmbLocations.SelectedItem;
            int month = this.GetSelectedMonth();
            int year = this.GetSelectedYear();
            StringBuilder sbHeaderCells = new StringBuilder();
            StringBuilder sbRows = new StringBuilder();
            bool showMonth = this.choiceSwitcherDateType.ChoiceOneSelected;
            JewishDate jd = showMonth ? new JewishDate(year, month, 1) : this.jdpFrom.Value;
            DailyZmanim dz = new DailyZmanim(jd.GregorianDate, location);
            string startSMonth = dz.SecularDate.ToString("MM yyyy");
            SelectedZmanRows columns = this.GetSelectedColumns();

            //Once "Generate" is clicked, we save the columns selected.
            Properties.Settings.Default.SelectedZmanRows = columns;
            Properties.Settings.Default.ArmyTime = this.choiceArmy.ChoiceOneSelected;
            Properties.Settings.Default.AmPm = this.choiceAmPm.ChoiceOneSelected;
            Properties.Settings.Default.Width100 = (bool)this.choiceWidth100.SelectedValue;
            Properties.Settings.Default.DirectionRight = (bool)this.choiceDirection.SelectedValue;
            Properties.Settings.Default.DateChooseMonth = this.choiceSwitcherDateType.ChoiceOneSelected;
            this.SaveDayOfWeek();

            foreach (var r in columns.OrderBy(sr => sr.ZmanIndex))
            {
                sbHeaderCells.AppendFormat("<th>{0}</th>", r.Header);
            }
            while (true)
            {
                sbRows.AppendFormat("<tr{0}>",
                    (jd.DayOfWeek == DayOfWeek.Saturday ||
                    SpecialDay.IsMajorYomTov(jd, location) ? " class='special'" :
                    SpecialDay.IsMinorYomTovOrFast(jd, location) ? " class='special2'" : ""));

                if (Properties.Settings.Default.DOWFormat != DayOfWeekFormat.None)
                {
                    sbRows.AppendFormat("<td>{0}</td>", this.GetDayOfWeekString(jd, location));
                }

                sbRows.AppendFormat(
                    "<td style=\"direction:rtl;\">{0}</td><td>{1}</td>",
                    Utils.ToNumberHeb(jd.Day),
                    dz.SecularDate.Day);


                foreach (var s in columns.OrderBy(sr => sr.ZmanIndex))
                {
                    string zmanTime = null;
                    if (s.ZmanIndex == 17)
                    {
                        zmanTime = DafYomi.GetDafYomi(jd).ToStringHeb();
                    }
                    else
                    {
                        zmanTime = s.GetZman(dz).ToString(
                            Properties.Settings.Default.ArmyTime,
                            Properties.Settings.Default.AmPm);
                    }
                    if (s.Bold)
                    {
                        zmanTime = "<strong>" + zmanTime + "</strong>";
                    }
                    sbRows.AppendFormat("<td>{0}</td>", zmanTime);
                }

                jd = jd + 1;
                if (showMonth ? jd.Month == month : jd <= this.jdpTo.Value)
                {
                    dz.SecularDate = jd.GregorianDate;
                }
                else
                {
                    sbRows.Append("</tr>");
                    break;
                }
            }
            string endSMonth = dz.SecularDate.ToString("MM yyyy");
            string monthHeader = showMonth ?
                Utils.JewishMonthNamesHebrew[month] + " " + Utils.ToNumberHeb(year % 1000) :
                this.getFromToHeaderText();
            int inBuiltColsCount = Properties.Settings.Default.DOWFormat == DayOfWeekFormat.None ? 2 : 3;

            return Properties.Resources.template
                .Replace("#--DIRECTION--#", (Properties.Settings.Default.DirectionRight ? "direction:rtl;" : ""))
                .Replace("#--TOTAL_CELLS--#", (columns.Count + inBuiltColsCount).ToString())
                .Replace("#--LOCATION--#", location.NameHebrew)
                .Replace("#--TABLE_WIDTH--#", Properties.Settings.Default.Width100 ? "width: 100%;" : "")
                .Replace("#--MONTH--#", "<strong>" + monthHeader + "</strong> (" +
                    startSMonth +
                    (startSMonth != endSMonth ? " - " + endSMonth : "") + ")")
                .Replace("#--HEADER_CELLS--#", sbHeaderCells.ToString())
                .Replace("#--LOCATION_COL_SPAN--#", inBuiltColsCount.ToString())
                .Replace("#--VALUE_ROWS--#", sbRows.ToString());
        }

        private void SaveDayOfWeek()
        {
            if (this.rbDOWJewishNum.Checked)
            {
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.JewishNum;
            }
            else if (this.rbDowNum.Checked)
            {
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.Number;
            }
            else if (this.rbDayOfWeekFull.Checked)
            {
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.Full;
            }
            else if (this.rbDOWEnglish.Checked)
            {
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.English;
            }
            else
            {
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.None;
            }
        }

        private string getFromToHeaderText()
        {
            string text = "";
            JewishDate from = this.jdpFrom.Value,
                to = this.jdpTo.Value;
            text =
            Utils.ToNumberHeb(from.Day) + " " +
            Utils.JewishMonthNamesHebrew[from.Month] + " " +
            Utils.ToNumberHeb(from.Year % 1000) +
            " - " +
            Utils.ToNumberHeb(to.Day) + " " +
            Utils.JewishMonthNamesHebrew[to.Month] + " " +
            Utils.ToNumberHeb(to.Year % 1000);

            return text;
        }

        private string GetDayOfWeekString(JewishDate jd, Location location)
        {
            string dow = "";

            if (this.rbDOWJewishNum.Checked)
            {
                dow = (jd.DayInWeek + 1).ToNumberHeb();
            }
            else if (this.rbDowNum.Checked)
            {
                dow = (jd.DayInWeek + 1).ToString();
            }
            else if (this.rbDayOfWeekFull.Checked)
            {
                if (jd.DayOfWeek == DayOfWeek.Saturday)
                {
                    var sedras = Sedra.GetSedra(jd, location.IsInIsrael);
                    dow = "ש\"ק " + string.Join(" - ", sedras.Select(i => i.nameHebrew));
                }
                else
                {
                    dow = Utils.JewishDOWNamesShort[jd.DayInWeek];
                }
            }
            else if (this.rbDOWEnglish.Checked)
            {
                if (jd.DayOfWeek == DayOfWeek.Saturday)
                {
                    var sedras = Sedra.GetSedra(jd, location.IsInIsrael);
                    dow = string.Join(" - ", sedras.Select(i => i.nameEng));
                }
                else
                {
                    dow = jd.DayOfWeek.ToString().Substring(0, 3);
                }
            }

            return dow;
        }

        private SelectedZmanRows GetSelectedColumns()
        {
            SelectedZmanRows columns = new SelectedZmanRows();
            foreach (DataGridViewRow dgvr in this.dataGridView1.Rows)
            {
                if (!dgvr.IsNewRow)
                {
                    int offset;
                    int.TryParse(Convert.ToString(dgvr.Cells[1].Value), out offset);
                    var sr = new SingleZmanRow
                    {
                        ZmanIndex = Array.IndexOf(Program.ZmanTypesList, Convert.ToString(dgvr.Cells[0].Value)),
                        Offset = offset,
                        Header = Convert.ToString(dgvr.Cells[2].Value),
                        Bold = Convert.ToBoolean(dgvr.Cells[3].Value)
                    };
                    columns.Add(sr);
                }
            }

            return columns;
        }

        private void CmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocationName = ((Location)cmbLocations.SelectedItem).Name;
            Properties.Settings.Default.Save();
        }

        private void ChoiceSwitcher1_ChoiceSwitched_1(object sender, EventArgs e)
        {
            this.FillLocations();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == clmDelete.Index)
            {
                this.dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void choiceSwitcherDateType_ChoiceSwitched(object sender, EventArgs e)
        {
            if (choiceSwitcherDateType.ChoiceOneSelected)
            {
                this.pnlDateTypeFromTo.Visible = false;
                this.pnlDateTypeMonth.Visible = true;
            }
            else
            {
                this.pnlDateTypeFromTo.Visible = true;
                this.pnlDateTypeMonth.Visible = false;
            }
        }
    }
    public class SelectedZmanRows : List<SingleZmanRow> { }
    public class SingleZmanRow
    {
        public int ZmanIndex { get; set; }
        public int Offset { get; set; }
        public string Header { get; set; }
        public bool Bold { get; set; }

        public HourMinute GetZman(DailyZmanim dz)
        {
            var hm = HourMinute.NoValue;
            switch (this.ZmanIndex)
            {
                case 0: hm = (dz.NetzMishor - 90); break; //Alos Hashachar - 90
                case 1: hm = (dz.NetzMishor - 72); break; //Alos Hashachar - 72            
                case 2: hm = dz.NetzAtElevation; break; //Sunrise
                case 3: hm = dz.NetzMishor; break; //Sunrise - sea level
                case 4: hm = dz.GetZman(ZmanType.KShmMga); break; //Shma - MG\A
                case 5: hm = dz.GetZman(ZmanType.KshmGra); break; //Shma - GR\A
                case 6: hm = dz.GetZman(ZmanType.TflMga); break; //Tefilla - MG\A
                case 7: hm = dz.GetZman(ZmanType.TflGra); break; //Tefilla - GR\A
                case 8: hm = dz.Chatzos; break; //Midday and Midnight
                case 9: hm = dz.GetZman(ZmanType.MinchaG); break; //Mincha Gedolah
                case 10: hm = dz.GetZman(ZmanType.MinchaK); break; //Mincha Ketana
                case 11: hm = dz.GetZman(ZmanType.MinchaPlg); break; //Plag HaMincha
                case 12: hm = dz.ShkiaAtElevation; break; //Sunset
                case 13: hm = dz.ShkiaMishor; break; //Sunset - sea level
                case 14: hm = dz.ShkiaAtElevation + 45; break; //Night - 45
                case 15: hm = dz.ShkiaAtElevation + 72; break; //Night - Rabbeinu Tam
                case 16: hm = dz.ShkiaAtElevation + (int)(dz.ShaaZmanis90 * 1.2); break; //Night - 72 Zmaniyos                    
            }
            if (this.Offset != 0)
            {
                hm += this.Offset;
            }
            return hm;
        }
    }
}
