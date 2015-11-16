using JewishCalendar;
using System.Collections.Generic;
using System.Drawing;

namespace LuachProject
{
    class SingleDateInfo
    {
        public JewishDate JewishDate { get; private set; }                       
        public RectangleF RectangleF { get; private set; }

        public SingleDateInfo(JewishDate jDate, RectangleF rect)
        {
            this.JewishDate = jDate;           
            this.RectangleF = rect;
        }
    }
}
