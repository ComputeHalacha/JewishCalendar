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
        private double _latitude;
        private double _longitude;

        public frmWorldMap()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.SetCoords(e);
            OnCoordinatesChange(new Coordinates { Latitude= this._latitude, Longitude= this._longitude });
        }

        private void SetCoords(MouseEventArgs e)
        {
            this._longitude = ((360d / this.pictureBox1.Width) * e.X) - 180;
            this._latitude = 90 - ((180d / this.pictureBox1.Height) * e.Y);
            this.label1.Text = String.Format("Latitude: {0:N4} Longitude: {1:N4}", this._latitude, this._longitude);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.panel1.SuspendLayout();
            this.pictureBox1.Anchor = AnchorStyles.None;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.SetBounds(0, 0,  this.pictureBox1.Image.Width,
            this.pictureBox1.Image.Height);            
            this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.panel1.Invalidate();
            this.panel1.ResumeLayout();
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollPosition = new Point(this.pictureBox1.Width / 2, this.pictureBox1.Height / 2);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.SetCoords(e);
        }
    }
}
