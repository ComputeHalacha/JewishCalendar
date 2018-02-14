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
            this.jdpFrom.Value = new JewishDate();            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        
        
        private void jdpFrom_ValueChanged(object sender, EventArgs e)
        {
            var jd = this.jdpFrom.Value;
            this.dateTimePicker1.Value = jd.GregorianDate;

            this.textBox1.Text = jd.AbsoluteDate.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {            
            this.jdpFrom.Value = new JewishDate(this.dateTimePicker1.Value);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.jdpFrom.Value = new JewishDate(Convert.ToInt32(this.textBox1.Text));
        }
    }
}
