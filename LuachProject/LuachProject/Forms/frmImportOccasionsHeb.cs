using System;
using System.Linq;
using System.Windows.Forms;

namespace LuachProject
{
    public partial class frmImportOccasionsHeb : Form
    {
        public UserOccasionColection OcassionList { get; private set; }

        public frmImportOccasionsHeb(UserOccasionColection uoc)
        {
            InitializeComponent();
            this.OcassionList = uoc;
        }

        private void frmImportOccasionsHeb_Load(object sender, EventArgs e)
        {
            this.LoadList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void LoadList()
        {
            this.listView1.SuspendLayout();
            this.listView1.Items.Clear();
            var list = from UserOccasion u in this.OcassionList
                       orderby u.SecularDate != DateTime.MinValue ? u.SecularDate : u.JewishDate.GregorianDate
                       select new ListViewItem(new string[] {
                           u.Name,
                           u.GetSettingDateString(true),
                           u.ToString(true),
                           u.Notes})
                       {
                           Tag = u,
                           Font = this.Font,
                           ForeColor = u.Color,
                           BackColor = u.BackColor
                       };
            this.listView1.Items.AddRange(list.ToArray());
            this.listView1.ResumeLayout();
        }

        private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var lvi in this.listView1.Items.OfType<ListViewItem>())
            {
                lvi.Checked = this.cbCheckAll.Checked;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.listView1.Items.OfType<ListViewItem>().Where(l => l.Checked).Count() == 0)
            {
                MessageBox.Show("לא נבחרו שום אירועים", "לוח - ייבוא אירועם",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (var lvi in this.listView1.Items.OfType<ListViewItem>().Where(l => !l.Checked))
            {
                this.OcassionList.Remove((UserOccasion)lvi.Tag);
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}