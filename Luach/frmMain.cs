using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luach
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmDailyEnglish().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new frmDailyHebrew().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new frmMonthlyEnglish().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new frmMonthlyHebrew().Show();
        }
    }
}
