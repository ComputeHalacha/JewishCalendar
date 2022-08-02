using JewishCalendar;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmReminderSettingsEng : Form
    {

        public frmReminderSettingsEng()
        {
            InitializeComponent();
        }

        private void frmSettingsEng_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ReminderTimeOfDay > DateTime.MinValue)
            {
                this.dateTimePicker1.Value = Properties.Settings.Default.ReminderTimeOfDay;
            }
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

        private void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Program.SetDailyRemindersTask();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Program.SetDailyRemindersTask();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Program.SendUserOccasionEmailReminders();
        }       

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!Program.SendTestEmail(out string outMessage))
            {
                this.lblTestResults.Text = "Sending test email failed. " + outMessage;
                this.lblTestResults.ForeColor = Color.Red;
            }
            else
            {
                this.lblTestResults.Text = "The test email has been successfully sent!";
                this.lblTestResults.ForeColor = Color.Green;
            }
            this.lblTestResults.Visible = true;
        }
    }
}