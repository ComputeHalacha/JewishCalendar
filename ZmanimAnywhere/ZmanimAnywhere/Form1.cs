using JewishCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ZmanimAnywhere
{
    public partial class Form1 : Form
    {
        private Coordinates _coordinates;
        private bool _loading = true;

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
            this.jdpFrom.Value = new JewishDate();
            this.FillLocations();
            this._loading = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FillLocations()
        {
            this.cmbLocations.DisplayMember = "NameHebrew";
            this.cmbLocations.Items.Clear();
            foreach (Location location in Program.LocationsList)
            {
                this.cmbLocations.Items.Add(location);
            }
        }
        private void ShowZmanim()
        {
            if (this._loading)
                return;
            this._loading = true;
            //DO STUFFFFFFFFFFFFFFFFF
            this.webBrowser1.DocumentText = this.getHtml();
            this._loading = false;
        }

        private void jdpFrom_ValueChanged(object sender, EventArgs e)
        {
            this._loading = true;
            this.dateTimePicker1.Value = this.jdpFrom.Value.GregorianDate;
            this._loading = false;
            this.ShowZmanim();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this._loading = true;
            this.jdpFrom.Value = new JewishDate(this.dateTimePicker1.Value);
            this._loading = false;
            this.ShowZmanim();
        }

        private void latDeg_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void latMin_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void latSec_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void longDeg_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void longMin_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void longSec_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void nudElevation_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void nudTimeZone_ValueChanged(object sender, EventArgs e)
        {
            this.ShowZmanim();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var m = new frmWorldMap(this._coordinates);
            m.Show();
            m.OnCoordinatesChange += delegate (Coordinates coords)
            {
                this._loading = true;
                this._coordinates = coords;
                this.cmbLocations.SelectedIndex = -1;
                this.SetCoords();
            };
        }

        private void SetCoords()
        {
            this._loading = true;

            this.latDeg.Value = this._coordinates.LatitudeDegrees;
            this.latMin.Value = this._coordinates.LatitudeMinutes;
            this.latSec.Value = (decimal)this._coordinates.LatitudeSeconds;
            this.longDeg.Value = this._coordinates.LongitudeDegrees;
            this.longMin.Value = this._coordinates.LongitudeMinutes;
            this.longSec.Value = (decimal)this._coordinates.LongitudeSeconds;
            this._loading = false;
            this.ShowZmanim();
        }

        private void cmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            this._loading = true;
            var location = ((Location)this.cmbLocations.SelectedItem);
            var sixtieth = 0.016666666666666666;
            this._coordinates = new Coordinates
            {
                Latitude = location.LatitudeDegrees + (location.LatitudeMinutes * sixtieth),
                Longitude = location.LongitudeDegrees + (location.LongitudeMinutes * sixtieth)
            };
            this.nudElevation.Value = location.Elevation;
            this.nudTimeZone.Value = location.TimeZone;
            this.SetCoords();
        }

        private string getHtml()
        {
            StringBuilder html = new StringBuilder();
            Location location = new JewishCalendar.Location
            {
                LatitudeDegrees = (int)this.latDeg.Value,
                LatitudeMinutes = (double)(this.latDeg.Value +
                        (this.latSec.Value / 60M)),
                LongitudeDegrees = (int)this.longDeg.Value,
                LongitudeMinutes = (double)(this.longDeg.Value +
                        (this.longSec.Value / 60M)),
                Elevation = (int)this.nudElevation.Value,
                TimeZone = (int)this.nudTimeZone.Value
            };
            var dz = new DailyZmanim(this.jdpFrom.Value.GregorianDate, location);
            html.AppendFormat("<h2>Zmanim at {0} feet</h2>", location.Elevation);
            html.Append("<ul>");
            if (dz.NetzMishor == TimeOfDay.NoValue)
            {
                html.Append("<li><strong>The sun does not rise at this location</strong></li>");
            }
            else
            {
                html.AppendFormat("<li>Alos - 90........<strong>{0}</strong></li>",
                dz.GetZman(ZmanType.NetzMishor, -90));
                html.AppendFormat("<li>Alos - 72........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.NetzMishor, -72));
                html.AppendFormat("<li>Mishyakir - 36........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.NetzMishor, -36));
                html.AppendFormat("<li>Netz at sea level........<strong>{0}</strong></li>",
                    dz.NetzMishor);
            }
            if (dz.NetzAtElevation == TimeOfDay.NoValue)
            {
                html.AppendFormat("<li><strong>The sun does not rise at {0} feet at this location</strong></li>", location.Elevation);
            }
            else
            {
                html.AppendFormat("<li>Netz at elevation........<strong>{0}</strong></li>",
                    dz.NetzAtElevation);
            }
            if (dz.NetzMishor == TimeOfDay.NoValue)
            {
                html.Append("<li><strong>The sun does not rise at this location</strong></li>");                
            }
            else
            {
                html.AppendFormat("<li>Krias Shma MG\"A........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.KShmMga));
                html.AppendFormat("<li>Krias Shma GR\"A........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.KshmGra));
                html.AppendFormat("<li>Tefilla MG\"A........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.TflMga));
                html.AppendFormat("<li>Tefilla GR\"A........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.TflGra));
            }

            html.AppendFormat("<li>Chatzos........<strong>{0}</strong></li>",
                dz.GetZman(ZmanType.Chatzos));
            html.AppendFormat("<li>Mincha Gedola........<strong>{0}</strong></li>",
                dz.GetZman(ZmanType.MinchaG));
            html.AppendFormat("<li>MIncha Ketana........<strong>{0}</strong></li>",
                dz.GetZman(ZmanType.MinchaK));
            html.AppendFormat("<li>Plag........<strong>{0}</strong></li>",
                dz.GetZman(ZmanType.MinchaPlg));
            if (dz.ShkiaMishor == TimeOfDay.NoValue)
            {
                html.Append("<li>Sunset at sea level........<strong>The sun does not set at this location</strong></li>");
            }
            else
            {
                html.AppendFormat("<li>Sunset at sea level........<strong>{0}</strong></li>",
                dz.ShkiaMishor);
            }
            if (dz.ShkiaAtElevation == TimeOfDay.NoValue)
            {
                html.AppendFormat("<li>Sunset at {0} feet........<strong>The sun does not set at {0} feet at this location</strong></li>", 
                    location.Elevation);
            }
            else
            {
                html.AppendFormat("<li>Sunset at elevation........<strong>{0}</strong></li>",
                    dz.ShkiaAtElevation);
                html.AppendFormat("<li>Night (45 minutes)........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.ShkiaAtElevation, 45));
                html.AppendFormat("<li>Night (72 minutes)........<strong>{0}</strong></li>",
                    dz.GetZman(ZmanType.ShkiaAtElevation, 72));
            }


            html.Append("</ul>");
            return Properties.Resources.template.Replace("#--CONTENT--#",
                html.ToString());
        }

        public class SingleZmanRow
        {
            public int ZmanIndex { get; set; }
            public int Offset { get; set; }
            public string Header { get; set; }
            public bool Bold { get; set; }
            public TimeOfDay GetZman(DailyZmanim dz)
            {
                var hm = TimeOfDay.NoValue;
                switch (this.ZmanIndex)
                {
                    case 0: hm = (dz.NetzMishor - 90); break; //Alos Hashachar - 90
                    case 1: hm = (dz.NetzMishor - 72); break; //Alos Hashachar - 72            
                    case 2: hm = dz.NetzAtElevation; break; //Sunrise
                    case 3: hm = dz.NetzMishor; break; //Sunrise - sea level
                    case 4: hm = dz.GetZman(ZmanType.KShmMga); break; //Shma - MG\A
                    case 5: hm = dz.GetZman(ZmanType.KshmGra); break; //Shma - GR\A
                    case 6: hm = dz.GetZman(ZmanType.TflMga); break; //Tefilla - MG\A
                    case 7: hm = dz.GetZman(ZmanType.TflGra); break; //Tefilla - GR\A
                    case 8: hm = dz.Chatzos; break; //Midday and Midnight
                    case 9: hm = dz.GetZman(ZmanType.MinchaG); break; //Mincha Gedolah
                    case 10: hm = dz.GetZman(ZmanType.MinchaK); break; //Mincha Ketana
                    case 11: hm = dz.GetZman(ZmanType.MinchaPlg); break; //Plag HaMincha
                    case 12: hm = dz.ShkiaAtElevation; break; //Sunset
                    case 13: hm = dz.ShkiaMishor; break; //Sunset - sea level
                    case 14: hm = dz.ShkiaAtElevation + 45; break; //Night - 45
                    case 15: hm = dz.ShkiaAtElevation + 72; break; //Night - Rabbeinu Tam
                    case 16: hm = dz.ShkiaAtElevation + (int)(dz.ShaaZmanis90 * 1.2); break; //Night - 72 Zmaniyos                    
                }
                if (this.Offset != 0)
                {
                    hm += this.Offset;
                }
                return hm;
            }
        }
    }
}
