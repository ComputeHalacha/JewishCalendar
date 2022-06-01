using JewishCalendar;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmSettingsEng : Form
    {

        public frmSettingsEng()
        {
            InitializeComponent();
        }

        private void frmAddOccasionEng_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                while (this.Opacity > 0)
                {
                    this.Opacity -= 0.1;
                    this.Refresh();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Program.SetDailyRemindersTask();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Program.SendUserOccasionEmailReminders();
        }
    }
}