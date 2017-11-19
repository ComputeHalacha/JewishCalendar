using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;


//data for a change event. the current implementation
//guarantees OldValue and NewValue are always different,
//but the change event is built this way so you could change
//internal behavior if you wanted.

public class ChoiceChangedEventArgs : EventArgs
{
    public bool OldValue { get; set; }
    public bool NewValue { get; set; }

    public ChoiceChangedEventArgs(bool old_value, bool new_value)
    {
        NewValue = new_value;
        OldValue = old_value;
    }
}


//event handler
public delegate void ChoiceChangedEventHandler(object sender, ChoiceChangedEventArgs e);



[DefaultEvent("ChoiceChanged")]
public class ChooserControl : UserControl
{
    private string _textRight = "Right";
    private string _textLeft = "Left";
    private bool _rightSelected = false;

    //public properties that will show in the designer    
    public Color ToggleButtonForeColor { get { return this.btnToggle.ForeColor; } set { this.btnToggle.ForeColor = value; } }
    public Color ToggleButtonBackColor { get { return this.btnToggle.BackColor; } set { this.btnToggle.BackColor = value; } }
    
    public string TextRight
    {
        get
        {
            return _textRight;
        }
        set
        {
            _textRight = value;
            UpdateLook();

        }
    }

    public string TextLeft
    {
        get
        {
            return _textLeft;
        }
        set
        {
            _textLeft = value;
            UpdateLook();
        }
    }

    public bool RightSelected
    {
        get { return _rightSelected; }

        set
        {
            //This is set up so change events don't even occur unless the value is really changing.
            //This behavior could be modified if for some reason you want a change event without
            //  the value actually changing; which explains the ChoiceChangedEventArgs having a
            //  old value and a new value, even though the current implementation guarantees
            //  they are actually different.

            if (RightSelected == value)
                return;

            _rightSelected = value;
            UpdateLook();

            ChoiceChanged(this, new ChoiceChangedEventArgs(!RightSelected, RightSelected));
        }
    }

    public bool LeftSelected
    {
        get { return !_rightSelected; }

        set
        {
            this.RightSelected = !value;
        }
    }

    private Label lblSelected;
    private Button btnToggle;

    //ChoiceChanged is the default event for this control, and the only custom one we need.
    [Category("Action")]
    [Description("Fires when the choose is toggled")]
    public event ChoiceChangedEventHandler ChoiceChanged;

    void _ChoiceChangedDoNothing(object sender, ChoiceChangedEventArgs e)
    {
    }

    public ChooserControl()
    {
        ChoiceChanged = new ChoiceChangedEventHandler(_ChoiceChangedDoNothing);

        this.BackColor = SystemColors.ControlLightLight;
        this.ForeColor = SystemColors.ControlText;
        this.BorderStyle = BorderStyle.FixedSingle;

        this.Width = 250;

        lblSelected = new Label
        {
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent,
            ForeColor = this.ForeColor

        };
        lblSelected.MouseDown += new MouseEventHandler(label1_MouseDown);
        this.ForeColorChanged += ChooserControl_ForeColorChanged;

        btnToggle = new Button();        

        this.ToggleButtonForeColor = SystemColors.ControlDark;
        this.ToggleButtonBackColor = SystemColors.Control;

        btnToggle.MouseDown += new MouseEventHandler(label1_MouseDown);

        this.Height = Math.Max(this.lblSelected.Height, this.btnToggle.Height);

        this.Controls.Add(lblSelected);
        this.Controls.Add(btnToggle);


        UpdateLook();
        this.PerformAutoScale();
    }

    private void ChooserControl_ForeColorChanged(object sender, EventArgs e)
    {
        this.lblSelected.ForeColor = this.ForeColor;
    }

    void label1_MouseDown(object sender, EventArgs e)
    {
        Clicked();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Clicked();
    }

    private void Clicked()
    {
        RightSelected = !RightSelected;
    }

    private void UpdateLook()
    {
        if (RightSelected)
        {
            this.lblSelected.Dock = DockStyle.Right;
            this.btnToggle.Dock = DockStyle.Left;
            this.lblSelected.Text = TextRight;
            this.btnToggle.Text = TextLeft;
        }
        else
        {
            this.btnToggle.Dock = DockStyle.Right;
            this.lblSelected.Dock = DockStyle.Left;
            this.lblSelected.Text = TextLeft;
            this.btnToggle.Text = TextRight;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
    }
}