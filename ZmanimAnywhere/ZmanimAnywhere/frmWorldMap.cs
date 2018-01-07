using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZmanimAnywhere
{
    public partial class frmWorldMap : Form
    {
        public delegate void CoordinatesChange(Coordinates coordinates);
        public event CoordinatesChange OnCoordinatesChange;
        private Coordinates _coordinates = new Coordinates() { Latitude = 0, Longitude = 0 };
        private Rectangle _recLabel;

        public frmWorldMap()
        {
            InitializeComponent();
        }

        public frmWorldMap(Coordinates coordinates) : this()
        {
            if (coordinates != null)
            {
                this._coordinates = coordinates;
            }
        }

        private void frmWorldMap_Load(object sender, EventArgs e)
        {
            this.OnCoordinatesChange += delegate (Coordinates coords)
            {

            };
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.SetCoords(e);
            OnCoordinatesChange(this._coordinates);
            this.pictureBox1.Invalidate();
        }

        private void SetCoords(MouseEventArgs e)
        {
            this._coordinates = this.GetMousePositionCoordinates(e.Location);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.panel1.SuspendLayout();
            this.pictureBox1.Anchor = AnchorStyles.None;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.SetBounds(0, 0, (int)(this.pictureBox1.Width * 1.25),
                (int)(this.pictureBox1.Height * 1.25));
            this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.panel1.Invalidate();
            this.panel1.ResumeLayout();
            this.ScrollToSelectedPoint();
            this.panel1.AutoScroll = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Coordinates c = this.GetMousePositionCoordinates(e.Location);
            string s = String.Format("{0}\n{1}",
                c.LatitudeLongString,
                c.LongitudeLongString);
            this.pictureBox1.SuspendLayout();
            if (this._recLabel != null)
            {
                this.pictureBox1.Invalidate(this._recLabel);
            }
            this._recLabel = new Rectangle(e.Location.X - 50, e.Location.Y + 10, 200, 100);
            using (var g = this.pictureBox1.CreateGraphics())
            {
                g.DrawString(s, this.Font, Brushes.YellowGreen, this._recLabel);
            }
            this.pictureBox1.Invalidate(this._recLabel);
            this.pictureBox1.ResumeLayout();
        }

        private Coordinates GetMousePositionCoordinates(Point point)
        {
            return new Coordinates()
            {
                Longitude = ((360d / this.pictureBox1.Width) * point.X) - 180,
                Latitude = 90 - ((180d / this.pictureBox1.Height) * point.Y)
            };
        }

        private PointF GetSelectedPoint()
        {
            var x = (this._coordinates.Longitude + 180d) * (this.pictureBox1.Width / 360d);
            var y = (this.pictureBox1.Height / 180d) * (90d - this._coordinates.Latitude);
            return new PointF((float)x, (float)y);
        }

        private void ScrollToSelectedPoint()
        {
            var sel = GetSelectedPoint();
            this.panel1.AutoScrollPosition = new Point(
              (int)(sel.X - (this.panel1.Width / 2) - 17.5),
              (int)(sel.Y - (this.panel1.Height / 2) - 17.5));
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var sel = GetSelectedPoint();
            using (Bitmap b = Properties.Resources.x)
            {
                e.Graphics.DrawImage(
                    b,
                    sel.X - (b.Width / 2),
                    sel.Y - (b.Height / 2),
                    35,
                    35);
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            this.ScrollToSelectedPoint();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {

        }
    }
}
