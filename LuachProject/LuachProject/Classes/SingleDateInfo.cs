using JewishCalendar;
using System.Collections.Generic;
using System.Drawing;

namespace LuachProject
{
    internal class SingleDateInfo
    {
        public JewishDate_ JewishDate { get; private set; }
        public RectangleF RectangleF { get; private set; }
        public List<UserOccasion> UserOccasions { get; private set; }

        public SingleDateInfo(JewishDate_ jDate, RectangleF rect)
        {
            this.JewishDate = jDate;
            this.RectangleF = rect;
            this.UpdateOccasions();
        }

        public void UpdateOccasions()
        {
            this.UserOccasions = UserOccasionColection.FromSettings(this.JewishDate);
        }
    }
}