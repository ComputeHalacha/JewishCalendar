using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JewishCalendar;

namespace LuachProject
{
    public partial class frmOccasionListHeb : Form
    {

        public frmOccasionListHeb()
        {
            InitializeComponent();
        }

        private void frmOccasionListHeb_Load(object sender, EventArgs e)
        {
            if (this.Owner is frmMonthlyHebrew)
            {
                ((frmMonthlyHebrew)this.Owner).OccasionWasChanged += delegate
                {
                    this.LoadList();
                };
            }
            else if (this.Owner is frmMonthlySecular)
            {
                ((frmMonthlySecular)this.Owner).OccasionWasChanged += delegate
                {
                    this.LoadList();
                };
            }

            this.LoadList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.LoadList();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.EditSelectedOccasion();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GoToSelectedOccasionDate();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.listView1.SelectedItems.Count <= 0)
            {
                e.Cancel = true;
            }
        }

        private void goToDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                this.GoToSelectedOccasionDate();
                this.Close();
            }
        }

        private void goToUpcomingOccurenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                this.GoToSelectedOccasionUpcoming();
                this.Close();
            }
        }

        private void editThisOccasionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                this.EditSelectedOccasion();
                this.Close();
            }
        }

        private void deleteThisOccasionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                var uo = (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag;
                if (MessageBox.Show("האם אתם בטוחים שברצונכם למחוק האירוע \"" + uo.Name + "\"?",
                        "לוח - מחק אירוע",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Properties.Settings.Default.UserOccasions.Remove(uo);
                    Properties.Settings.Default.Save();
                    ((dynamic)this.Owner).Reload();
                    this.LoadList();
                }
            }
        }

        private void llExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Properties.Settings.Default.UserOccasions.Count == 0)
            {
                MessageBox.Show("אין אירועים ברשימה.", "לוח - ייצוא אירועים",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var sfd = new SaveFileDialog()
            {
                DefaultExt = "xml",
                Filter = "קובצי XML (*.xml)|*.xml|כל סוגי הקבצים (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                CreatePrompt = false,
                OverwritePrompt = true,
                FileName = "לוח_ייצוא_אירועים_" + (Environment.UserName ?? "") + ".xml"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    var formatter = new System.Xml.Serialization.XmlSerializer(Properties.Settings.Default.UserOccasions.GetType());
                    using (var stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        formatter.Serialize(stream, Properties.Settings.Default.UserOccasions);
                    }
                    MessageBox.Show("רשימת האירועים ייוצאו בהצלחה אל " + sfd.FileName,
                        "לוח - ייצוא אירועים");
                }
            }
        }

        private void llImport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var sfd = new OpenFileDialog()
            {
                DefaultExt = "xml",
                Filter = "קובצי XML (*.xml)|*.xml|כל סוגי הקבצים (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                FileName = "לוח_ייצוא_אירועים_" + (Environment.UserName ?? "") + ".xml"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK && (!string.IsNullOrWhiteSpace(sfd.FileName)) && File.Exists(sfd.FileName))
                {
                    UserOccasionColection uoc = null;
                    var formatter = new System.Xml.Serialization.XmlSerializer(Properties.Settings.Default.UserOccasions.GetType());
                    using (var stream = new FileStream(sfd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            uoc = (UserOccasionColection)formatter.Deserialize(stream);
                        }
                        catch
                        {
                            MessageBox.Show("ייבוא אירועים מקובץ " + sfd.FileName +
                                "נכשלה.\nאנא וודאו שהקובץ הוא רשימת אירועים שייוצאו מתוכנת לוח ושלא נעשו בו שינויים לא נכונים",
                                "לוח - ייבוא אירועים", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //Let the user try again...
                            llImport_LinkClicked(sender, e);
                            return;
                        }
                    }

                    if (uoc != null && uoc.Count > 0)
                    {
                        using (var fi = new frmImportOccasionsHeb(uoc))
                        {
                            if (fi.ShowDialog() == DialogResult.OK)
                            {
                                Properties.Settings.Default.UserOccasions.AddRange(fi.OcassionList);
                                Properties.Settings.Default.Save();
                                ((dynamic)this.Owner).Reload();
                                this.LoadList();
                                MessageBox.Show(fi.OcassionList.Count.ToString() + " אירועים הובאו בהצלחה מקובץ " + sfd.FileName,
                                    "לוח - ייבוא אירועים");
                            }
                            else
                            {
                                MessageBox.Show("לא הובאו שום אירועים מקובץ " + sfd.FileName,
                                    "לוח - ייבוא אירועים",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        return;                        
                    }
                    else
                    {
                        MessageBox.Show("לא הצלחנו להביא שום אירועים מקובץ " + sfd.FileName,
                            "לוח - ייבוא אירועים", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
        }

        private void LoadList()
        {
            this.listView1.SuspendLayout();
            this.listView1.Items.Clear();
            var search = this.txtName.Text.ToLower();
            var list = from UserOccasion u in Properties.Settings.Default.UserOccasions
                       orderby u.SecularDate != DateTime.MinValue ? u.SecularDate : u.JewishDate.GregorianDate
                       where string.IsNullOrWhiteSpace(search) || u.Name.ToLower().Contains(search)
                       select new ListViewItem(new string[] {
                           u.Name,
                           u.GetSettingDateString(true),
                           u.ToString(true) })
                       {
                           Tag = u,
                           Font = this.Font,
                           ForeColor = u.Color,
                           BackColor = u.BackColor
                       };
            this.listView1.Items.AddRange(list.ToArray());
            this.listView1.ResumeLayout();
        }

        private void EditSelectedOccasion()
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ((dynamic)this.Owner).EditOccasion(
                    (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag);
            }
        }

        private void GoToSelectedOccasionDate()
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                var uo = (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag;
                ((dynamic)this.Owner).SelectedDate = 
                    (uo.JewishDate != null ? uo.JewishDate.GregorianDate : uo.SecularDate);
            }
        }

        private void GoToSelectedOccasionUpcoming()
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                //The selected occasion
                UserOccasion uo = (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag;
                ((dynamic)this.Owner).SelectedDate = uo.GetUpcomingOccurence();
            }
        }
    }
}