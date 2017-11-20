using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

public enum ChoiceSwitcherChoices
{
    ChoiceOne,
    ChoiceTwo
}

[DefaultEvent("ChoiceSwitched")]
public class ChoiceSwitcher : UserControl
{
    private string _choiceOneText = "First Choice";
    private string _choiceTwoText = "Second Choice";
    private ChoiceSwitcherChoices _choiceChosen = ChoiceSwitcherChoices.ChoiceOne;
    private Color _slotBackColor = Color.Gray;

    [Category("Action")]
    [Description("Fires when the choice is toggled")]
    public event EventHandler ChoiceSwitched;

    public ChoiceSwitcher()
    {
        ChoiceSwitched = delegate (object sender, EventArgs e) { };
        this.Width = 250;
        this.Height = 25;
        this.ForeColor = Color.DimGray;
        this.PerformAutoScale();
    }
    
    public object ChoiceOneValue { get; set; }
    public object ChoiceTwoValue { get; set; }
    public object SelectedValue
    {
        get
        {
            return this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                this.ChoiceOneValue : this.ChoiceTwoValue;
        }
        set
        {
            if (this.SelectedValue != value)
            {
                if (value == ChoiceOneValue)
                {
                    this.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
                }
                else if (value == ChoiceTwoValue)
                {
                    this.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
                }
                else
                {
                    throw new ArgumentException("The supplied object does not equal the value of either choice");
                }
            }
        }
    }
    public string ChoiceOneText
    {
        get
        {
            return this._choiceOneText;
        }
        set
        {
            this._choiceOneText = value;
            this.Invalidate();
        }
    }
    public string ChoiceTwoText
    {
        get
        {
            return this._choiceTwoText;
        }
        set
        {
            this._choiceTwoText = value;
            this.Invalidate();
        }
    }
    public ChoiceSwitcherChoices ChoiceChosen
    {
        get
        {
            return this._choiceChosen;
        }
        set
        {
            if (this._choiceChosen != value)
            {
                this._choiceChosen = value;
                ChoiceSwitched(this, new EventArgs());
                this.Invalidate();
            }
        }
    }
    public bool ChoiceOneSelected
    {
        get
        {
            return this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne;
        }
        set
        {
            this.ChoiceChosen = ChoiceSwitcherChoices.ChoiceOne;
        }
    }
    public bool ChoiceTwoSelected
    {
        get
        {
            return this._choiceChosen == ChoiceSwitcherChoices.ChoiceTwo;
        }
        set
        {
            this.ChoiceChosen = ChoiceSwitcherChoices.ChoiceTwo;
        }
    }
    public Color SlotBackColor
    {
        get
        {
            return this._slotBackColor;
        }
        set
        {
            this._slotBackColor = value;
            this.Invalidate();
        }
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        this._choiceChosen =
            this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
            ChoiceSwitcherChoices.ChoiceTwo : ChoiceSwitcherChoices.ChoiceOne;
        this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.SuspendLayout();
        base.OnPaint(e);
        using (var g = e.Graphics)
        {
            SizeF textOneSize = g.MeasureString(this._choiceOneText, this.Font),
                textTwoSize = g.MeasureString(this._choiceTwoText, this.Font);
            float textWidth = textOneSize.Width + textTwoSize.Width,
                slotWidth = (this.Width - textWidth) * 0.8f,
                slotHeight = this.Height * 0.7f,
                slotTop = (this.Height - slotHeight) / 2f,
                slotLeft = (this.Width / 2) - (slotWidth / 2);
            Font notSelectedFont = new Font(this.Font, FontStyle.Strikeout);
            Brush textBrush = new SolidBrush(this.ForeColor);
            Brush slotBrush = new SolidBrush(this._slotBackColor);
            g.DrawString(
                this._choiceOneText,
                this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                    this.Font : notSelectedFont,
                textBrush,
                0,
                (this.Height / 2) - (textOneSize.Height / 2));

            GraphicsPath graphPath = new GraphicsPath();
            graphPath.FillMode = FillMode.Winding;
            graphPath.AddEllipse(slotLeft, slotTop, slotHeight, slotHeight);
            graphPath.AddRectangle(
                new RectangleF(
                    slotLeft + (slotHeight / 2),
                    slotTop,
                    (slotWidth - slotHeight),
                    slotHeight));
            graphPath.AddEllipse(
                (slotLeft + slotWidth) - slotHeight,
                slotTop,
                slotHeight,
                slotHeight);
            g.FillPath(slotBrush, graphPath);
            g.DrawImage(
                ZmanimChart.Properties.Resources.SwitchHead,
                new RectangleF(
                    (this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                        textOneSize.Width : slotLeft + (slotWidth - slotHeight)),
                    0,
                    this.Height,
                    this.Height));
            g.DrawString(
                this._choiceTwoText,
                this._choiceChosen == ChoiceSwitcherChoices.ChoiceTwo ?
                    this.Font : notSelectedFont,
                textBrush,
                this.Width - textTwoSize.Width,
                (this.Height / 2) - (textOneSize.Height / 2));
        }
        this.ResumeLayout();
    }
}