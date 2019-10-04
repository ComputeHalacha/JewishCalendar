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
            {
                return;
            }

            PropertyInfo aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);

            aProp.SetValue(this, true, null);

            this.Text = "Zmanim Chart - Version " + "1.2";

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
            this.choiceDayDetails.ChoiceOneSelected = Properties.Settings.Default.ShowDayDetails;
        }

        private void FillDateCombos()
        {
            this.cmbMonth.ValueMember = "Key";
            this.cmbMonth.DisplayMember = "Value";
            this.cmbYear.ValueMember = "Key";
            this.cmbYear.DisplayMember = "Value";

            var now = new JewishDate();
            var monthToDisplay = now.Day > 12 ? now.AddMonths(1) : now;
            KeyValuePair<int, string> month = new KeyValuePair<int, string>();
            KeyValuePair<int, string> year = new KeyValuePair<int, string>();
            for (int i = monthToDisplay.Year - 2; i <= monthToDisplay.Year + 2; i++)
            {
                var kvp = new KeyValuePair<int, string>(i,
                                    Utils.ToNumberHeb(i % 1000));
                this.cmbYear.Items.Add(kvp);
                if (i == monthToDisplay.Year)
                {
                    year = kvp;
                }
            }
            for (int i = 1; i <= 13; i++)
            {
                var kvp = new KeyValuePair<int, string>(i,
                    Utils.JewishMonthNamesHebrew[i]);
                this.cmbMonth.Items.Add(kvp);
                if (i == monthToDisplay.Month)
                {
                    month = kvp;
                }
            }
            this.cmbMonth.SelectedItem = month;
            this.cmbYear.SelectedItem = year;
            this.jdpFrom.Value = now;
            this.jdpTo.Value = monthToDisplay;
        }

        private void FillZmanTypeRows()
        {
            if (Properties.Settings.Default.SelectedZmanColumns != null)
            {
                foreach (SingleZmanColumn zmanRow in (
                    from r in Properties.Settings.Default.SelectedZmanColumns
                    orderby r.ZmanIndex, r.Offset
                    select r))
                {
                    int index = this.dataGridView1.Rows.Add(new object[]
                    {
                        Program.ZmanTypesList[zmanRow.ZmanIndex],
                        zmanRow.DaysOfWeek == null || zmanRow.DaysOfWeek.Length == 8
                            ? "כולם"
                            : String.Join(", ", zmanRow.DaysOfWeek.Select(dow =>
                                Utils.ToNumberHeb(dow+1)).ToArray()),
                        zmanRow.Offset,
                        zmanRow.AlternateOffset,
                        zmanRow.Header,
                        zmanRow.Bold
                    });
                    this.dataGridView1.Rows[index].Cells[1].Tag = zmanRow.DaysOfWeek;
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
            SelectedZmanColumns columns = this.GetSelectedColumns();
            bool showSeconds = this.choiceSeconds.ChoiceOneSelected;

            //Once "Generate" is clicked, we save the columns selected.
            Properties.Settings.Default.SelectedZmanColumns = columns;
            Properties.Settings.Default.ArmyTime = this.choiceArmy.ChoiceOneSelected;
            Properties.Settings.Default.AmPm = this.choiceAmPm.ChoiceOneSelected;
            Properties.Settings.Default.Width100 = (bool)this.choiceWidth100.SelectedValue;
            Properties.Settings.Default.DirectionRight = (bool)this.choiceDirection.SelectedValue;
            Properties.Settings.Default.DateChooseMonth = this.choiceSwitcherDateType.ChoiceOneSelected;
            Properties.Settings.Default.ShowSeconds = this.choiceSeconds.ChoiceOneSelected;
            Properties.Settings.Default.ShowDayDetails = this.choiceDayDetails.ChoiceOneSelected;
            this.SaveDayOfWeek();

            var columnsSorted = from c in columns orderby c.ZmanIndex, c.Offset select c;
            foreach (var zmanColumn in columnsSorted)
            {
                sbHeaderCells.AppendFormat("<th>{0}</th>", zmanColumn.Header);
            }
            while (true)
            {
                sbRows.AppendFormat("<tr{0}>",
                    (jd.DayOfWeek == DayOfWeek.Saturday ||
                    SpecialDay.IsMajorYomTov(jd, location) ? " class='special'" :
                    SpecialDay.IsMinorYomTovOrFast(jd, location) ? " class='special2'" : ""));

                if (Properties.Settings.Default.DOWFormat != DayOfWeekFormat.None)
                {
                    sbRows.AppendFormat("<td class=\"dow {0}\">{1}</td>",
                        this.rbDOWEnglish.Checked ? "left" : "right",
                        this.GetDayOfWeekString(jd, location));
                }

                sbRows.AppendFormat(
                    "<td style=\"direction:rtl;\">{0}</td><td>{1}</td>",
                    Utils.ToNumberHeb(jd.Day),
                    dz.SecularDate.Day);


                foreach (var zmanColumn in columnsSorted)
                {
                    string zmanTime = null;
                    TimeOfDay zman = TimeOfDay.NoValue;
                    if (zmanColumn.ZmanIndex == 17)
                    {
                        zmanTime = DafYomi.GetDafYomi(jd).ToStringHeb();
                    }
                    else if (zmanColumn.DaysOfWeek == null /* All days */||
                        zmanColumn.DaysOfWeek.Contains(jd.DayInWeek) ||
                        zmanColumn.AlternateOffset != 0)
                    {
                        zman = zmanColumn.GetZman(dz);
                        //For netz, if we are not showing seconds, we show the end of the minute
                        if (!showSeconds && zmanColumn.ZmanIndex.In(2, 3) && zman.Seconds > 0)
                        {
                            zman += 1;
                        }
                        zmanTime = zman.ToString(
                            Properties.Settings.Default.ArmyTime,
                            Properties.Settings.Default.AmPm);
                    }
                    if (!string.IsNullOrEmpty(zmanTime))
                    {
                        if (zmanColumn.Bold)
                        {
                            zmanTime = "<strong>" + zmanTime + "</strong>";
                        }
                        if (zman != TimeOfDay.NoValue && Properties.Settings.Default.ShowSeconds)
                        {
                            zmanTime += "<sub>:" +
                                (zman.Seconds < 10 ? "0" : "") +
                                zman.Seconds.ToString() + "</sub>";
                        }
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
                    dow = "ש\"ק";
                    if (!SpecialDay.IsMajorYomTov(jd, location))
                    {
                        dow += " " + string.Join(" - ",
                        Sedra.GetSedra(jd, location.IsInIsrael)
                            .Select(i => i.nameHebrew));
                    }
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
                    dow = "Shabbos";
                    if (!SpecialDay.IsMajorYomTov(jd, location))
                    {
                        dow += " " + string.Join(" - ",
                        Sedra.GetSedra(jd, location.IsInIsrael)
                            .Select(i => i.nameEng));
                    }
                }
                else
                {
                    dow = jd.DayOfWeek.ToString().Substring(0, 3);
                }
            }
            if (this.choiceDayDetails.ChoiceOneSelected)
            {
                string holidayText = Zmanim.GetHolidaysText(
                    Zmanim.GetHolidays(jd, location.IsInIsrael), " - ",
                    !this.rbDOWEnglish.Checked);

                if (!string.IsNullOrWhiteSpace(holidayText))
                {
                    dow += " - " + holidayText;
                }
            }
            return dow;
        }

        private SelectedZmanColumns GetSelectedColumns()
        {
            SelectedZmanColumns columns = new SelectedZmanColumns();
            foreach (DataGridViewRow dgvr in this.dataGridView1.Rows)
            {
                if (!dgvr.IsNewRow)
                {
                    int offset, alternateOffset;
                    int.TryParse(Convert.ToString(dgvr.Cells[2].Value), out offset);
                    int.TryParse(Convert.ToString(dgvr.Cells[3].Value), out alternateOffset);
                    var sr = new SingleZmanColumn
                    {
                        ZmanIndex = Array.IndexOf(Program.ZmanTypesList, Convert.ToString(dgvr.Cells[0].Value)),
                        DaysOfWeek = dgvr.Cells[1].Tag == null
                            ? new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }
                            : (int[])dgvr.Cells[1].Tag,
                        Offset = offset,
                        AlternateOffset = alternateOffset,
                        Header = Convert.ToString(dgvr.Cells[4].Value),
                        Bold = Convert.ToBoolean(dgvr.Cells[5].Value)
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == clmDaysOfWeek.Index)
            {
                var cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var rect = this.dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var dow = cell.Tag == null
                    ? new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }
                    : (int[])cell.Tag;
                using (var fdow = new frmDaysOfWeek(dow) { Top = rect.Top, Left = rect.Left })
                {
                    if (fdow.ShowDialog(this.dataGridView1) == DialogResult.OK)
                    {
                        var sdow = fdow.DaysOfWeekArray;
                        cell.Tag = sdow;
                        cell.Value = sdow.Length == 8
                            ? "כולם"
                            : String.Join(", ", sdow.Select(d => d == 7
                                ? "יו\"ט"
                                : Utils.ToNumberHeb(d + 1)).ToArray());
                    }
                }
            }
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
    public class SelectedZmanColumns : List<SingleZmanColumn> { }
    public class SingleZmanColumn
    {
        public int[] DaysOfWeek { get; set; }
        public int ZmanIndex { get; set; }
        public int Offset { get; set; }
        public int AlternateOffset { get; set; }
        public string Header { get; set; }
        public bool Bold { get; set; }
        public TimeOfDay GetZman(DailyZmanim dz)
        {
            var hm = TimeOfDay.NoValue;
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
                case 16: hm = dz.ShkiaAtElevation + (int)(dz.ShaaZmanisMga * 1.2); break; //Night - 72 Zmaniyos                    
            }
            var isYomTov = SpecialDay.IsShabbosOrYomTov(dz.JewishDate, dz.Location);
            var hasYomTovOffset = !this.DaysOfWeek.Contains(7);
            var showYomTov = isYomTov && hasYomTovOffset;
            if (this.Offset != 0 &&
                (this.DaysOfWeek == null ||
                this.DaysOfWeek.Contains((int)dz.SecularDate.DayOfWeek)) && (!showYomTov))
            {
                hm += this.Offset;
            }
            else if (this.AlternateOffset != 0 &&
                this.DaysOfWeek != null &&
                ((!this.DaysOfWeek.Contains((int)dz.SecularDate.DayOfWeek)) ||
                showYomTov))
            {
                hm += this.AlternateOffset;
            }
            return hm;
        }
    }
}
