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
        }

        private void FillDateCombos()
        {
            this.cmbMonth.ValueMember = "Key";
            this.cmbMonth.DisplayMember = "Value";
            this.cmbYear.ValueMember = "Key";
            this.cmbYear.DisplayMember = "Value";

            var nextMonth = new JewishDate().AddMonths(1);
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
                default:
                    rbDayOfWeekFull.Checked = true;
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
            JewishDate jd = new JewishDate(year, month, 1);
            DailyZmanim dz = new DailyZmanim(jd.GregorianDate, location);
            string startSMonth = dz.SecularDate.ToString("MM yyyy");
            SelectedZmanRows columns = this.GetSelectedColumns();

            //Once "Generate" is clicked, we save the columns selected.
            Properties.Settings.Default.SelectedZmanRows = columns;

            foreach (var r in columns.OrderBy(sr => sr.ZmanIndex))
            {
                sbHeaderCells.AppendFormat("<th>{0}</th>", r.Header);
            }
            while (true)
            {
                sbRows.AppendFormat(
                    "<tr{0}><td>{1}</td><td>{2}</td><td>{3}</td>",
                    (jd.DayOfWeek == DayOfWeek.Saturday ? " class='special'" : ""),
                    this.GetDayOfWeekString(jd, location),
                    Utils.ToNumberHeb(jd.Day),
                    dz.SecularDate.Day);

                foreach (var s in columns.OrderBy(sr => sr.ZmanIndex))
                {
                    var zmanTime = s.GetZman(dz).ToString24H();
                    if (s.Bold)
                    {
                        zmanTime = "<strong>" + zmanTime + "</strong>";
                    }
                    sbRows.AppendFormat("<td>{0}</td>", zmanTime);
                }

                jd = jd + 1;
                if (jd.Month == month)
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

            return Properties.Resources.template
                .Replace("#--TOTAL_CELLS--#", (columns.Count + 3).ToString())
                .Replace("#--LOCATION--#", location.NameHebrew)
                .Replace("#--MONTH--#", "<strong>" +
                    Utils.JewishMonthNamesHebrew[month] + " " +
                    Utils.ToNumberHeb(year % 1000) +
                    "</strong> (" +
                    startSMonth +
                    (startSMonth != endSMonth ? " - " + endSMonth : "") + ")")
                .Replace("#--HEADER_CELLS--#", sbHeaderCells.ToString())
                .Replace("#--VALUE_ROWS--#", sbRows.ToString());
        }

        private string GetDayOfWeekString(JewishDate jd, Location location)
        {
            string dow;
            if (this.rbDOWJewishNum.Checked)
            {
                dow = (jd.DayInWeek + 1).ToNumberHeb();
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.JewishNum;
            }
            else if (this.rbDowNum.Checked)
            {
                dow = (jd.DayInWeek + 1).ToString();
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.Number;
            }
            else
            {                
                dow = Utils.JewishDOWNamesShort[jd.DayInWeek];
                if(jd.DayOfWeek == DayOfWeek.Saturday)
                {
                    var sedras = Sedra.GetSedra(jd, location.IsInIsrael);                    
                    dow += " " + string.Join(" - ", sedras.Select(i => i.nameHebrew));
                }
                Properties.Settings.Default.DOWFormat = DayOfWeekFormat.Full;
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
                case 15: hm = dz.ShkiaAtElevation + 45; break; //Night - 45
                case 16: hm = dz.ShkiaAtElevation + 72; break; //Night - Rabbeinu Tam
                case 17: hm = dz.ShkiaAtElevation + (int)(dz.ShaaZmanis90 * 1.2); break; //Night - 72 Zmaniyos
            }
            if (this.Offset != 0)
            {
                hm += this.Offset;
            }
            return hm;
        }
    }
}
