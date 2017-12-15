using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZmanimChart
{
    public partial class frmBrowser : Form
    {
        public frmBrowser()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.webBrowser1.ShowPrintDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.webBrowser1.ShowSaveAsDialog();
        }
    }
}
