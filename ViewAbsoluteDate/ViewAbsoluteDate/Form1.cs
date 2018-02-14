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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        
        
        private void jdpFrom_ValueChanged(object sender, EventArgs e)
        {
            var jd = this.jdpFrom.Value;
            this.dateTimePicker1.Value = jd.GregorianDate;

            this.tableLayoutPanel1.Controls.Add(new Label { Text = jd.AbsoluteDate.ToString() });
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {            
            this.jdpFrom.Value = new JewishDate(this.dateTimePicker1.Value);
        }
    }
}
