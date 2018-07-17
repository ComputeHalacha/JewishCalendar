using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ZmanimChart
{
    public partial class frmDaysOfWeek : Form
    {
        public int[] DaysOfWeekArray { get; private set; }
        public frmDaysOfWeek(int[] daysOfWeek)
        {
            InitializeComponent();
            this.DaysOfWeekArray = daysOfWeek;
            foreach (int i in this.DaysOfWeekArray)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<int> selected = new List<int>();
            foreach (int cb in this.checkedListBox1.CheckedIndices)
            {
                selected.Add(cb);
            }
            this.DaysOfWeekArray = selected.ToArray();
            this.DialogResult = DialogResult.OK;
        }        
    }
}
