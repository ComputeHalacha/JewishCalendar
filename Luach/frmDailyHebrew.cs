using JewishCalendar;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Luach
{
    public partial class frmDailyHebrew : Form
    {
        #region private fields
        private DateTime _currentDate;
        private JewishDate _currentJewishDate;
        private bool _loading = false;
        #endregion

        #region public properties
        public DateTime CurrentDate
        {
            get
            {
                return this._currentDate;
            }
            set
            {
                if (this._currentDate != value)
                {
                    this._currentDate = value;
                    var jdate = new JewishDate(this._currentDate, 
                        (JewishCalendar.Location)this.cmbLocation.SelectedItem);
                    if (jdate != this._currentJewishDate)
                    {
                        this._currentJewishDate = jdate;
                    }
                    this.ShowDateData();
                    if (this.Owner is frmMonthlyHebrew)
                    {
                        ((frmMonthlyHebrew)this.Owner).SelectedJewishDate = this._currentJewishDate;
                    }
                }
            }
        }

        public JewishDate CurrentJewishDate
        {
            get
            {
                return this._currentJewishDate;
            }
            set
            {
                if (this._currentJewishDate != value)
                {
                    this._currentJewishDate = value;
                    var gdate = this._currentJewishDate.GregorianDate;
                    if (gdate != this._currentDate)
                    {
                        this._currentDate = this._currentJewishDate.GregorianDate;
                    }
                }
                this.ShowDateData();
                if (this.Owner is frmMonthlyHebrew)
                {
                    ((frmMonthlyHebrew)this.Owner).SelectedJewishDate = this._currentJewishDate;
                }
            }
        }
        #endregion

        public frmDailyHebrew()
        {
            InitializeComponent();
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SetLocationDataSource();
            var location = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            if (!location.IsInIsrael)
            {
                this._loading = true;
                this.rbInChul.Checked = true;
                this._loading = false;
            }
        }

        #region event handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            this._loading = true;
            if (this._currentDate == DateTime.MinValue)
                this.CurrentDate = DateTime.Now;

            this.dateTimePicker1.Value = this._currentDate;
            this.jewishDatePicker1.Value = this._currentJewishDate;
            
            this.dateTimePicker1.DataBindings.Add("Value", this, "CurrentDate", false, DataSourceUpdateMode.OnPropertyChanged);
            this.jewishDatePicker1.DataBindings.Add("Value", this, "CurrentJewishDate", false, DataSourceUpdateMode.OnPropertyChanged);
          
            this._loading = false;
            this.ShowDateData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = DateTime.Now;
        }

        private void rbInIsrael_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                this.SetLocationDataSource();
            }
            //Location was changed, so we may need to re-do the zmanim
            this.CurrentJewishDate = new JewishDate(this._currentDate, 
                (JewishCalendar.Location)this.cmbLocation.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker1.Value.AddDays(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker1.Value.AddDays(-1);
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._loading)
            {
                var location = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
                //If the location changed, the Jewish date may change as well
                this.CurrentJewishDate = new JewishDate(this._currentDate, location);                
                Properties.Settings.Default.LocationName = location.Name;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region private functions
        private void SetLocationDataSource()
        {
            this.Cursor = Cursors.WaitCursor;
            bool wasLoading = this._loading;
            this._loading = true;
            bool inIsrael = this.rbInIsrael.Checked;
            var list = Program.LocationsList.Where(l => l.IsInIsrael == inIsrael).ToList();
            this.cmbLocation.DataSource = null;
            this.cmbLocation.DataSource = list;
            this.cmbLocation.DisplayMember = "NameHebrew";

            var name = Properties.Settings.Default.LocationName;
            var i = list.FirstOrDefault(l => l.Name == name);
            if (i != null)
            {
                this.cmbLocation.SelectedItem = i;
            }
            else
            {
                this.cmbLocation.SelectedIndex = 0;
            }
            this._loading = wasLoading;
            this.Cursor = Cursors.Default;
        }

        private void ShowDateData()
        {
            this.Cursor = Cursors.WaitCursor;
            var location = (JewishCalendar.Location)this.cmbLocation.SelectedItem;
            var zmanim = new Zmanim(this._currentJewishDate, location);
            var dy = DafYomi.GetDafYomi(this._currentJewishDate);
            var holidays = Zmanim.GetHolidays(this._currentJewishDate, location.IsInIsrael);
            var holidayList = holidays.Cast<SpecialDay>();
            var netzshkia = zmanim.GetNetzShkia();
            var netz = netzshkia != null ? netzshkia[0] : new HourMinute();
            var shkia = netzshkia != null ? netzshkia[1] : netz;
            var chatzos = zmanim.GetChatzos();
            var shaaZmanis = zmanim.GetShaaZmanis();
            var shaaZmanis90 = zmanim.GetShaaZmanis(90);
            var bold = new Font(this.richTextBox1.Font, FontStyle.Bold | FontStyle.Underline);

            this.richTextBox1.Clear();
            this.richTextBox1.SelectionColor = Color.DarkBlue;
            this.richTextBox1.SelectionFont = bold;
            this.richTextBox1.SelectedText = Utils.JewishDOWNames[(int)this._currentJewishDate.DayOfWeek].ToString() +
                " - " + this._currentJewishDate.ToShortDateStringHeb() + Environment.NewLine;
            if (holidayList.Count() > 0)
            {
                this.richTextBox1.SelectedText = Environment.NewLine + Zmanim.GetHolidaysText(holidays, " - ", true) +
                    Environment.NewLine;
                if (holidayList.Any(h => h.DayType.HasFlag(SpecialDay.SpecialDayTypes.HasCandleLighting)))
                {
                    this.AddLine("הדלקת נרות", (shkia - location.CandleLighting).ToString24H());
                }
            }
            this.richTextBox1.SelectionColor = Color.Black;
            this.richTextBox1.SelectedText = Environment.NewLine;
            this.AddLine("פרשת השבוע",
                string.Join(" ", Sedra.GetSedra(this._currentJewishDate, location.IsInIsrael).Select(i => i.nameHebrew)));
            this.AddLine("תאריך לועזי", this._currentDate.ToString("D", Program.HebrewCultureInfo));
            if (dy != null)
            {
                this.AddLine("דף היומי", dy.ToStringHeb());
            }
            this.richTextBox1.SelectionFont = this.richTextBox1.Font;
            this.richTextBox1.SelectedText = string.Format("{0}----------------------------------------------{0}" +
                "זמני היום עבור {1}{0}-----------------------------------------------{0}",
                Environment.NewLine, location.Name);
            this.AddLine("עלות השחר - 90", (netz - 90).ToString24H());
            this.AddLine("עלות השחר - 72", (netz - 72).ToString24H());
            this.AddLine("נץ החמה", netz.ToString24H());
            this.AddLine("סזק\"ש מג\"א", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 3D)).ToString24H());
            this.AddLine("סזק\"ש גר\"א", (netz + (int)Math.Floor(shaaZmanis * 3D)).ToString24H());
            this.AddLine("סז\"ת מג\"א", ((netz - 90) + (int)Math.Floor(shaaZmanis90 * 4D)).ToString24H());
            this.AddLine("סז\"ת גר\"א", (netz + (int)Math.Floor(shaaZmanis * 4D)).ToString24H());
            this.AddLine("חצות היום והלילה", chatzos.ToString24H());
            this.AddLine("מנחה גדולה", (chatzos + (int)(shaaZmanis * 0.5)).ToString24H());
            this.AddLine("שקיעת החמה", shkia.ToString24H());
            this.AddLine("צה\"כ 45", (shkia + 45).ToString24H());
            this.AddLine("צה\"כ ר\"ת", (shkia + 72).ToString24H());
            this.Cursor = Cursors.Default;
        }

        private void AddLine(string header, string value)
        {
            this.richTextBox1.SelectionFont = this.richTextBox1.Font;
            this.richTextBox1.SelectedText = header.Trim() + " " + new string('.', 30 - header.Length) + " ";
            this.richTextBox1.SelectionFont = new Font(this.richTextBox1.Font, FontStyle.Bold);
            this.richTextBox1.SelectedText = value.Trim() + Environment.NewLine;
        }
        #endregion
    }
}
