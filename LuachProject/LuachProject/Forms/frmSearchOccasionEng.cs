using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmSearchOccasionEng : Form
    {

        public frmSearchOccasionEng()
        {
            InitializeComponent();
        }       

        private void frmSearchOccasionEng_Load(object sender, EventArgs e)
        {
            this.LoadList();
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.LoadList();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ((dynamic)this.Owner).EditOccasion(
                    (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                var uo = (UserOccasion)this.listView1.SelectedItems.OfType<ListViewItem>().First().Tag;
                ((dynamic)this.Owner).SelectedDate = uo.JewishDate.GregorianDate;
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
                CreatePrompt = false,
                OverwritePrompt = true,
                FileName = "Luach Project Occasions_" + (Environment.UserName ?? "") +".xml"
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
                FileName = "Luach Project Occasions" + (Environment.UserName ?? "") + ".xml"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    UserOccasionColection uoc = null;
                    var formatter = new System.Xml.Serialization.XmlSerializer(Properties.Settings.Default.UserOccasions.GetType());
                    using (var stream = new FileStream(sfd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        uoc = (UserOccasionColection)formatter.Deserialize(stream);
                    }

                    if (uoc != null)
                    {
                        Properties.Settings.Default.UserOccasions.AddRange(uoc);
                        Properties.Settings.Default.Save();
                        ((dynamic)this.Owner).Reload();
                        this.LoadList();
                    }
                }
                MessageBox.Show("Your occasions have been successfully imported from " + sfd.FileName,
                        "Luach Project - Import Occasions");
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
    }
}