using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmOccasionList : Form
    {

        public frmOccasionList()
        {
            InitializeComponent();
        }

        private void frmOccasionListEng_Load(object sender, EventArgs e)
        {
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
                if (MessageBox.Show("Are you sure that you wish to delete the Occasion \"" + uo.Name + "\"?",
                        "Luach Project - Delete Occasion",
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

        private void llExportList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Properties.Settings.Default.UserOccasions.Count == 0)
            {
                MessageBox.Show("There are no Occasions in your list.", "Luach Project - Export Occasions",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var sfd = new SaveFileDialog()
            {
                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                CreatePrompt = false,
                OverwritePrompt = true,
                FileName = "Luach Project Occasions_" + (Environment.UserName ?? "") + ".xml"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    var formatter = new System.Xml.Serialization.XmlSerializer(Properties.Settings.Default.UserOccasions.GetType());
                    using (var stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        formatter.Serialize(stream, Properties.Settings.Default.UserOccasions);
                    }
                    MessageBox.Show("Your occasions have been successfully exported to " + sfd.FileName,
                        "Luach Project - Export Occasions");
                }
            }
        }

        private void llImportList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var sfd = new OpenFileDialog()
            {
                DefaultExt = "xml",
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                RestoreDirectory = false,
                FileName = "Luach Project Occasions" + (Environment.UserName ?? "") + ".xml"
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
                            MessageBox.Show("We were not able to import any Occasions from " + sfd.FileName + 
                                ".\nPlease assure that the selected file is a valid Occasion file, and that it was not edited incorrectly.",
                                "Luach Project - Import Occasions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            //Let the user try again...
                            llImportList_LinkClicked(sender, e);
                            return;
                        }                       
                    }

                    if (uoc != null && uoc.Count > 0)
                    {
                        Properties.Settings.Default.UserOccasions.AddRange(uoc);
                        Properties.Settings.Default.Save();
                        ((dynamic)this.Owner).Reload();
                        this.LoadList();
                        MessageBox.Show(uoc.Count.ToString() + " Occasions have been successfully imported from " + sfd.FileName,
                            "Luach Project - Import Occasions");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("There weren't any occasions that we were able to import from " + sfd.FileName,
                            "Luach Project - Import Occasions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
        }

        private void LoadList()
        {
            this.listView1.Items.Clear();
            var search = this.txtName.Text.ToLower();
            var list = from UserOccasion u in Properties.Settings.Default.UserOccasions
                       orderby u.SecularDate != DateTime.MinValue ? u.SecularDate : u.JewishDate.GregorianDate
                       where string.IsNullOrWhiteSpace(search) || u.Name.ToLower().Contains(search)
                       select new ListViewItem(new string[] {
                           u.Name,
                           u.GetSettingDateString(false),
                           u.ToString() })
                       {
                           Tag = u,
                           Font = this.Font,
                           ForeColor = u.Color,
                           BackColor = u.BackColor
                       };
            this.listView1.Items.AddRange(list.ToArray());
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
                ((dynamic)this.Owner).SelectedDate = uo.JewishDate.GregorianDate;
            }
        }
    }
}