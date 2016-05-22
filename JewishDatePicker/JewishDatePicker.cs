using JewishCalendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace JewishDatePicker
{
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public partial class JewishDatePicker : UserControl
    {
        #region Private Fields

        private bool _isLoading;

        private Languages _language;

        private JewishDate _value = new JewishDate();

        #endregion Private Fields
        private JewishDate _minDate = JewishDate.MinDate;
        private JewishDate _maxDate = JewishDate.MaxDate;

        #region Public Constructors

        public JewishDatePicker()
        {
            InitializeComponent();

            if (this._value == null) this._value = new JewishDate();

            this.cmbJYear.DisplayMember = "Value";
            this.cmbJYear.ValueMember = "Key";
            this.cmbJMonth.DisplayMember = "Value";
            this.cmbJMonth.ValueMember = "Key";
            this.cmbJDay.DisplayMember = "Value";
            this.cmbJDay.ValueMember = "Key";

            this.FillJewishYearsCombo();
            this.FillJewishMonthsCombo();
            this.FillJewishDaysCombo();
            this.SetCombosToShowValue();
            this.cmbJYear.SelectedIndexChanged += new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
            this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
            this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler ValueChanged;
        public event EventHandler MaxValueChanged;
        public event EventHandler MinValueChanged;

        #endregion Public Events

        #region Public Enums

        public enum Languages { Hebrew, English };

        #endregion Public Enums

        #region Public Properties

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
                this.RefillCombos();
            }
        }

        public JewishDate MaxDate
        {
            get
            {
                return this._maxDate;
            }
            set
            {
                if (value != this._maxDate)
                {
                    this._maxDate = value;
                    this.RefillCombos();
                    this.RaiseMaxValueChanged();
                }
            }
        }

        public JewishDate MinDate
        {
            get
            {
                return this._minDate;
            }
            set
            {
                if (this._minDate != value)
                {
                    this._minDate = value;
                    this.RefillCombos();
                    this.RaiseMinValueChanged();
                }
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

        [Bindable(true)]
        public JewishDate Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (this._value == null || this._value != value)
                {
                    if (value < this._minDate)
                    {
                        this._value = this._minDate;
                    }
                    else if (value > this._maxDate)
                    {
                        this._value = this._maxDate;
                    }
                    else
                    {
                        this._value = value;
                    }
                    this._isLoading = true;
                    this.SetCombosToShowValue();
                    this._isLoading = false;
                    this.RaiseValueChanged();
                }
            }
        }

        #endregion Public Properties

        #region Public Methods
        public virtual void RaiseValueChanged()
        {
            this.ValueChanged?.Invoke(this, new EventArgs());
        }

        public virtual void RaiseMaxValueChanged()
        {
            this.MaxValueChanged?.Invoke(this, new EventArgs());
        }

        public virtual void RaiseMinValueChanged()
        {
            this.MinValueChanged?.Invoke(this, new EventArgs());
        }

        #endregion Public Methods

        #region Private Methods

        private void cmbJDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._isLoading)
            {
                this.SetValueFromCombos();
            }
        }

        private void cmbJMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbJDay.SelectedIndexChanged -= new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
            if (!this._isLoading)
            {
                this.SetValueFromCombos();
            }
            this.FillJewishDaysCombo();
            this.SetCombosToShowValue();
            this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
        }

        private void cmbJYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbJMonth.SelectedIndexChanged -= new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
            if (!this._isLoading)
            {
                this.SetValueFromCombos();
            }
            this.FillJewishMonthsCombo();
            this.SetCombosToShowValue();
            this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
        }

        private void FillJewishDaysCombo()
        {
            if (this._value == null)
            {
                return;
            }

            this.cmbJDay.Items.Clear();

            int d = JewishDateCalculations.DaysInJewishMonth(this._value.Year, this._value.Month);
            for (int i = 1; i <= d; i++)
            {
                this.cmbJDay.Items.Add(new KeyValuePair<int, string>(i,
                    this._language == Languages.Hebrew ? i.ToNumberHeb() : i.ToString()));
            }
        }

        private void FillJewishMonthsCombo()
        {
            if (this._value == null)
            {
                return;
            }

            this.cmbJMonth.Items.Clear();
            bool m = JewishDateCalculations.IsJewishLeapYear(this._value.Year);
            for (int i = 1; i <= (m ? 13 : 12); i++)
            {
                this.cmbJMonth.Items.Add(new KeyValuePair<int, string>(i,
                    this._language == Languages.Hebrew ? Utils.GetProperMonthNameHeb(this.Value.Year, i) : Utils.GetProperMonthName(this.Value.Year, i)));
            }
        }

        private void FillJewishYearsCombo()
        {
            for (int i = this._minDate.Year; i <= this._maxDate.Year; i++)
            {
                this.cmbJYear.Items.Add(new KeyValuePair<int, string>(i,
                    this._language == Languages.Hebrew ? i.ToNumberHeb() : i.ToString()));
            }
        }

        private void JewishDatePicker_Load(object sender, EventArgs e)
        {
        }

        private void SetCombosToShowValue()
        {
            if (this._value == null)
            {
                return;
            }

            if (this.cmbJYear.SelectedItem == null ||
                ((KeyValuePair<int, string>)this.cmbJYear.SelectedItem).Key != this._value.Year)
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
            if (this.cmbJMonth.SelectedItem == null ||
                ((KeyValuePair<int, string>)this.cmbJMonth.SelectedItem).Key != this._value.Month)
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
            if (this.cmbJDay.SelectedItem == null ||
                ((KeyValuePair<int, string>)this.cmbJDay.SelectedItem).Key != this._value.Day)
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

        private void SetValueFromCombos()
        {
            int year = ((KeyValuePair<int, string>)this.cmbJYear.SelectedItem).Key,
                month = ((KeyValuePair<int, string>)this.cmbJMonth.SelectedItem).Key,
                day = ((KeyValuePair<int, string>)this.cmbJDay.SelectedItem).Key;

            if (day == 30 && JewishDateCalculations.DaysInJewishMonth(year, month) == 29)
            {
                day = 29;
            }

            this.Value = new JewishDate(year, month, day);
        }

        private void RefillCombos()
        {
            if (this.cmbJYear.Items.Count > 0)
            {
                this.cmbJYear.SelectedIndexChanged -= new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
                this.cmbJMonth.SelectedIndexChanged -= new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
                this.cmbJDay.SelectedIndexChanged -= new System.EventHandler(this.cmbJDay_SelectedIndexChanged);

                this.cmbJYear.Items.Clear();
                this.FillJewishYearsCombo();
                this.FillJewishMonthsCombo();
                this.FillJewishDaysCombo();
                this.SetCombosToShowValue();

                this.cmbJYear.SelectedIndexChanged += new System.EventHandler(this.cmbJYear_SelectedIndexChanged);
                this.cmbJMonth.SelectedIndexChanged += new System.EventHandler(this.cmbJMonth_SelectedIndexChanged);
                this.cmbJDay.SelectedIndexChanged += new System.EventHandler(this.cmbJDay_SelectedIndexChanged);
            }
        }
        #endregion Private Methods
    }
}