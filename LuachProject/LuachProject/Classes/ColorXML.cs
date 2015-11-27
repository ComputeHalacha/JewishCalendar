using System;
using System.Drawing;
using System.Xml.Serialization;

namespace LuachProject
{
    [Serializable]
    public struct ColorXML
    {
        private Color _color;

        public ColorXML(Color color)
        {
            this._color = color;
        }

        [XmlIgnore]
        public Color Color
        {
            get { return this._color; }
            set { this._color = value; }
        }

        [XmlAttribute]
        public string ColorHtml
        {
            get { return ColorTranslator.ToHtml(this.Color); }
            set { this.Color = ColorTranslator.FromHtml(value); }
        }

        public static implicit operator Color(ColorXML colorXML)
        {
            return colorXML.Color;
        }

        public static implicit operator ColorXML(Color color)
        {
            return new ColorXML(color);
        }
    }
}