using JewishCalendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JewishDatePicker
{
    [DefaultEvent("ValueChanged")]
    public partial class JewishDatePicker : UserControl
    {
        public enum Languages { Hebrew, English };
        public event EventHandler ValueChanged;

        private JewishDate _value = new JewishDate();
        private Languages _language;
        private bool _isLoading;

        [Bindable(true)]
        public JewishDate Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (this._value != value)
                {
                    if (value < MinDate)
                    {
                        throw new ArgumentOutOfRangeException("Value", "Value can not be less than the MinDate");
                    }
                    if (value > MaxDate)
                    {
                        throw new ArgumentOutOfRangeException("Value", "Value can not be more than the MaxDate");
                    }
                    this._value = value;
                    this._isLoading = true;
                    this.SetCombos();
                    this._isLoading = false;
                    this.RaiseValueChanged();
                }
            }
        }

       public JewishDate MinDate { get; set; }

       public JewishDate MaxDate { get; set; }

        [DefaultValue(Languages.Hebrew)]
        public Languages Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
                this.RightToLeft = this._language == Languages.Hebrew ? RightToLeft.Yes : RightToLeft.No;

                if (this.cmbJYear.Items.Count > 0)
                {
                    this.cmbJYear.SelectedIndexChanged -= new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
                    this.cmbJMonth.SelectedIndexChanged -= new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
                    this.cmbJDay.SelectedIndexChanged -= new System.EventHandler(this.cmbJDay_SelectedIndexChanged);

                    this.cmbJYear.Items.Clear();
                    this.FillJewishYears();
                    this.FillJewishMonths();
                    this.FillJewishDays();
                    this.SetCombos();

                    this.cmbJYear.SelectedIndexChanged += new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
                    this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
                    this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
                }

            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.cmbJYear.ForeColor = this.cmbJMonth.ForeColor = this.cmbJDay.ForeColor = value;
            }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.cmbJYear.Font = this.cmbJMonth.Font = this.cmbJDay.Font = value;
            }
        }

        [DefaultValue(RightToLeft.Yes)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
                this.flowLayoutPanel1.RightToLeft = 
                    this.cmbJYear.RightToLeft = 
                    this.cmbJMonth.RightToLeft = 
                    this.cmbJDay.RightToLeft = value;
            }                
        }

        [DefaultValue("Color.White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.flowLayoutPanel1.BackColor = value;
                if (value != Color.Transparent)
                    this.cmbJYear.BackColor = this.cmbJMonth.BackColor = this.cmbJDay.BackColor = value;
            }
        }

        [DefaultValue(FlatStyle.Flat)]
        public FlatStyle FlatStyle
        {
            get
            {
                return this.cmbJYear.FlatStyle;
            }
            set
            {
                this.cmbJYear.FlatStyle = this.cmbJMonth.FlatStyle = this.cmbJDay.FlatStyle = value;                
            }
        }

        public JewishDatePicker()
        {
            this.MinDate = JewishDate.MinDate;
            this.MaxDate = JewishDate.MaxDate;
            
            InitializeComponent();
            
            this.cmbJYear.DisplayMember = "Value";
            this.cmbJYear.ValueMember = "Key";
            this.cmbJMonth.DisplayMember = "Value";
            this.cmbJMonth.ValueMember = "Key";
            this.cmbJDay.DisplayMember = "Value";
            this.cmbJDay.ValueMember = "Key";

            this.FillJewishYears();
            this.FillJewishMonths();
            this.FillJewishDays();
            this.SetCombos();
            this.cmbJYear.SelectedIndexChanged += new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
            this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
            this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
        }

        private void JewishDatePicker_Load(object sender, EventArgs e)
        {            
        }

        public virtual void RaiseValueChanged()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }

        private void cmbJYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbJMonth.SelectedIndexChanged -= new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
            if (!this._isLoading)
            {
                this.UpdateValue();
            }
            this.FillJewishMonths();
            this.SetCombos();
            this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
        }

        private void cmbJMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbJDay.SelectedIndexChanged -= new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
            if (!this._isLoading)
            {
                this.UpdateValue();
            }
            this.FillJewishDays();
            this.SetCombos();
            this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
        }

        private void cmbJDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._isLoading)
            {
                this.UpdateValue();
            }
        }

        private void UpdateValue()
        {
            this.Value = new JewishDate(((KeyValuePair<int, string>)this.cmbJYear.SelectedItem).Key,
                ((KeyValuePair<int, string>)this.cmbJMonth.SelectedItem).Key,
                ((KeyValuePair<int, string>)this.cmbJDay.SelectedItem).Key);
        }

        private void FillJewishYears()
        {
            for (int i = this.MinDate.Year; i <= this.MaxDate.Year; i++)
            {
                this.cmbJYear.Items.Add(new KeyValuePair<int, string>(i, this._language == Languages.Hebrew ? i.ToNumberHeb() : i.ToString()));
            }
        }

        private void FillJewishDays()
        {
            if(this._value == null)
            {
                return;
            }

            this.cmbJDay.Items.Clear();

            int d = JewishDateCalculations.DaysInJewishMonth(this._value.Year, this._value.Month);
            for (int i = 1; i <= d; i++)
            {
                this.cmbJDay.Items.Add(new KeyValuePair<int, string>(i, this._language == Languages.Hebrew ? i.ToNumberHeb() : i.ToString()));
            }
        }

        private void FillJewishMonths()
        {
            this.cmbJMonth.Items.Clear();
            bool m = JewishDateCalculations.IsJewishLeapYear(this.Value.Year);
            for (int i = 1; i <= (m ? 13 : 12); i++)
            {
                this.cmbJMonth.Items.Add(new KeyValuePair<int, string>(i, this._language == Languages.Hebrew ? Utils.JewishMonthNamesHebrew[i] : Utils.JewishMonthNamesEnglish[i]));
            }
        }

        private void SetCombos()
        {
            if(this._value == null)
            {
                return;
            }

            if (this.cmbJYear.SelectedItem == null || ((KeyValuePair<int, string>)this.cmbJYear.SelectedItem).Key != this._value.Year)
            {
                foreach (KeyValuePair<int, string> kvp in this.cmbJYear.Items)
                {
                    if (kvp.Key == this._value.Year)
                    {
                        this.cmbJYear.SelectedItem = kvp;
                        break;
                    }
                }
            }
            if (this.cmbJMonth.SelectedItem == null || ((KeyValuePair<int, string>)this.cmbJMonth.SelectedItem).Key != this._value.Month)
            {
                foreach (KeyValuePair<int, string> kvp in this.cmbJMonth.Items)
                {
                    if (kvp.Key == this._value.Month)
                    {
                        this.cmbJMonth.SelectedItem = kvp;
                        break;
                    }
                }
            }
            if (this.cmbJDay.SelectedItem == null || ((KeyValuePair<int, string>)this.cmbJDay.SelectedItem).Key != this._value.Day)
            {
                foreach (KeyValuePair<int, string> kvp in this.cmbJDay.Items)
                {
                    if (kvp.Key == this._value.Day)
                    {
                        this.cmbJDay.SelectedItem = kvp;
                        break;
                    }
                }
            }
        }
    }
}
