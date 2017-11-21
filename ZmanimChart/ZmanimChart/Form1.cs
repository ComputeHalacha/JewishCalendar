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

            this.cmbLocations.DisplayMember =  "NameHebrew";
            this.cmbMonth.ValueMember = "Key";
            this.cmbMonth.DisplayMember = "Value";
            this.cmbYear.ValueMember = "Key";
            this.cmbYear.DisplayMember = "Value";

            this.FillLocations();
            var nextMonth = new JewishDate().AddMonths(1);
            KeyValuePair<int, string> month = new KeyValuePair<int, string>();
            KeyValuePair<int, string> year = new KeyValuePair<int, string>();
            for (int i = nextMonth.Year-2; i <= nextMonth.Year + 2; i++)
            {
                var kvp = new KeyValuePair<int, string>(i,
                                    Utils.ToNumberHeb(i % 1000));
                this.cmbYear.Items.Add(kvp);
                if(i==nextMonth.Year)
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
            this.cmbMonth.SelectedItem= month;
            this.cmbYear.SelectedItem = year;
        }

        private void FillLocations()
        {
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

        private void button1_Click(object sender, EventArgs e)
        {
            var location = this.cmbLocations.SelectedItem;
            if (location == null)
            {
                return;
            }
            var b = new frmBrowser();
            b.webBrowser1.DocumentText = GetHtml(
                (Location)location, 
                this.GetSelectedMonth(),
                this.GetSelectedYear());
            b.Show();
        }

        private string GetHtml(Location location, int month, int year)
        {
            StringBuilder sb = new StringBuilder();
            JewishDate jd = new JewishDate(year, month, 1);
            while (jd.Month == month)
            {
                var sd = jd.GregorianDate;
                var sunrise = Zmanim.GetNetzShkia(sd, location, false)[0];
                var sunset = Zmanim.GetShkia(sd, location, true);
                sb.AppendFormat(
                    "<tr{9}><td>{0}</td><td style='direction:rtl;'>{1}</td><td>{2}</td><td>{3}</td><td><strong>{4}</strong></td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>",
                    jd.DayInWeek + 1,
                    Utils.ToNumberHeb(jd.Day),
                    sd.Day,
                    (sunrise - 50).ToString24H(),
                    (sunrise - 25).ToString24H(),
                    (sunrise - 10).ToString24H(),
                    sunrise.ToString24H(),
                    sunset.ToString24H(),
                    (sunset + 72).ToString24H(),
                    jd.DayOfWeek == DayOfWeek.Saturday ? " class='special'" : "");
                jd = jd + 1;
            }
            return Properties.Resources.template
                .Replace("#--LOCATION--#", location.NameHebrew)
                .Replace("#--MONTH--#", Utils.JewishMonthNamesHebrew[month] + " " + Utils.ToNumberHeb(year % 1000))
                .Replace("#--PLACE_HOLDER--#", sb.ToString());
        }

        private void cmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocationName = ((Location)cmbLocations.SelectedItem).Name;
            Properties.Settings.Default.Save();
        }               
        
        private void choiceSwitcher1_ChoiceSwitched_1(object sender, EventArgs e)
        {
            this.FillLocations();
        }
    }
}
