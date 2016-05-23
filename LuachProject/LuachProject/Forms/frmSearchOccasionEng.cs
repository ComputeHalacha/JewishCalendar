using System;
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
            var list = from UserOccasion u in Properties.Settings.Default.UserOccasions
                       orderby u.SecularDate != DateTime.MinValue ? u.SecularDate : u.JewishDate.GregorianDate
                       select new ListViewItem(new string[] {
                           u.GetSettingDateString(false),
                           u.ToString(),
                            u.Name})
                       {
                           Tag = u,
                           Font = this.Font,
                           ForeColor = u.Color,
                           BackColor = u.BackColor
                       };
            this.listView1.Items.AddRange(list.ToArray());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            var list = from UserOccasion u in Properties.Settings.Default.UserOccasions
                       where u.Name.ToLower().Contains(this.txtName.Text.ToLower())
                       orderby u.SecularDate != DateTime.MinValue ? u.SecularDate : u.JewishDate.GregorianDate
                       select new ListViewItem(new string[] {
                            u.GetSettingDateString(false),
                            u.ToString(),
                            u.Name})
                       {
                           Tag = u,
                           Font = this.Font,
                           ForeColor = u.Color,
                           BackColor = u.BackColor
                       };
            this.listView1.Items.AddRange(list.ToArray());
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
    }
}