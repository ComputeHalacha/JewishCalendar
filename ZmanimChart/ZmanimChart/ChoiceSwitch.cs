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
public class ChoiceSwitcher : Control
{
    private string _choiceOneText = "First Choice";
    private string _choiceTwoText = "Second Choice";
    private ChoiceSwitcherChoices _choiceChosen = ChoiceSwitcherChoices.ChoiceOne;
    private Color _slotBackColor = Color.Gray;
    private Color _slotChoiceTwoBackColor = Color.Teal;
    private bool _strikeNotSelected = false;
    private bool _highlightSelected = false;
    private Color _selectedHighlightColor = Color.White;
    private Font _selectedFont;
    private Color _selectedForeColor = Color.Blue;

    [Category("Action")]
    [Description("Fires when the choice is toggled")]
    public event EventHandler ChoiceSwitched;

    public ChoiceSwitcher()
    {
        ChoiceSwitched = delegate (object sender, EventArgs e) { };
        this.Width = 250;
        this.Height = 25;
        this.ForeColor = Color.DimGray;
        this.SelectedFont = this.Font;
        this.SelectedForeColor = this.ForeColor;
    }
    public object ChoiceOneValue { get; set; }
    public object ChoiceTwoValue { get; set; }
    public bool StrikeNotSelected
    {
        get { return this._strikeNotSelected; }
        set
        {
            if (this._strikeNotSelected != value)
            {
                this._strikeNotSelected = value;
                this.Invalidate();
            }
        }
    }
    public bool HighlightSelected
    {
        get { return this._highlightSelected; }
        set
        {
            if (this._highlightSelected != value)
            {
                this._highlightSelected = value;
                this.Invalidate();
            }
        }
    }
    public Color SelectedHighlightColor
    {
        get { return this._selectedHighlightColor; }
        set
        {
            if (this._selectedHighlightColor != value)
            {
                this._selectedHighlightColor = value;
                this.Invalidate();
            }
        }
    }
    public Font SelectedFont
    {
        get { return this._selectedFont; }
        set
        {
            if (this._selectedFont != value)
            {
                this._selectedFont = value;
                this.Invalidate();
            }
        }
    }
    public Color SelectedForeColor
    {
        get { return this._selectedForeColor; }
        set
        {
            if (this._selectedForeColor != value)
            {
                this._selectedForeColor = value;
                this.Invalidate();
            }
        }
    }
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
            if (this._slotBackColor != value)
            {
                this._slotBackColor = value;
                this.Invalidate();
            }
        }
    }
    public Color SlotChoiceTwoBackColor
    {
        get
        {
            return this._slotChoiceTwoBackColor;
        }
        set
        {
            if (this._slotChoiceTwoBackColor != value)
            {
                this._slotChoiceTwoBackColor = value;
                this.Invalidate();
            }
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        this.ChoiceChosen =
            this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
            ChoiceSwitcherChoices.ChoiceTwo : ChoiceSwitcherChoices.ChoiceOne;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.SuspendLayout();
        base.OnPaint(e);
        using (var g = e.Graphics)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            Font notSelectedFont = this._strikeNotSelected ?
                new Font(this.Font, FontStyle.Strikeout | FontStyle.Regular) : this.Font;

            SizeF textOneSize = TextRenderer.MeasureText(this._choiceOneText,
                    this._selectedFont),
                textTwoSize = TextRenderer.MeasureText(this._choiceTwoText,
                    this._selectedFont);
            float textWidth = textOneSize.Width + textTwoSize.Width + 2,
                slotWidth = ((this.Width - textWidth) * 0.8f) - 2,
                slotHeight = this.Height * 0.7f,
                slotTop = (this.Height - slotHeight) / 2f,
                slotLeft = textOneSize.Width + 5f;
            Brush slotBrush = new SolidBrush(
                this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                    this._slotBackColor : this._slotChoiceTwoBackColor),
                highlightBrush = new SolidBrush(this._selectedHighlightColor);

            if (this._highlightSelected && this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne)
            {
                g.FillRectangle(
                    highlightBrush,
                    0,
                    0,
                    textOneSize.Width,
                    this.Height);
            }
            TextRenderer.DrawText(g,
                this._choiceOneText,
                this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                    this._selectedFont : notSelectedFont,
                new Point(0, (int)((this.Height / 2) - (textOneSize.Height / 2))),
                 this._choiceChosen == ChoiceSwitcherChoices.ChoiceOne ?
                    this.SelectedForeColor : this.ForeColor);
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
                        slotLeft - 2 : slotLeft + (slotWidth - slotHeight)) - 2,
                    0,
                    this.Height,
                    this.Height));
            if (this._highlightSelected && this._choiceChosen == ChoiceSwitcherChoices.ChoiceTwo)
            {
                g.FillRectangle(
                    highlightBrush,
                    (slotLeft + slotWidth) + 5,
                    0,
                    textTwoSize.Width,
                    this.Height);
            }
            TextRenderer.DrawText(g,
            this._choiceTwoText,
             this._choiceChosen == ChoiceSwitcherChoices.ChoiceTwo ?
                 this._selectedFont : notSelectedFont,
             new Point((int)((slotLeft + slotWidth) + 5),
                (int)((this.Height / 2) - (textOneSize.Height / 2))),
              this._choiceChosen == ChoiceSwitcherChoices.ChoiceTwo ?
                 this.SelectedForeColor : this.ForeColor);

            notSelectedFont.Dispose();
            slotBrush.Dispose();
            highlightBrush.Dispose();
        }
        this.ResumeLayout();
    }
}