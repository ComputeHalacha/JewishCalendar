namespace LuachProject
{
    partial class frmOccasionListHeb
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToUpcomingOccurenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.llExport = new System.Windows.Forms.LinkLabel();
            this.llImport = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(748, 577);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 31);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "סגור";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "חפש ע\"פ שם אירוע או חלק ממנה:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(12, 42);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(830, 23);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.Color.Lavender;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(12, 71);
            this.listView1.Name = "listView1";
            this.listView1.RightToLeftLayout = true;
            this.listView1.Size = new System.Drawing.Size(830, 447);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "תיאור האירוע";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "תאריך";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "סוג אירוע";
            this.columnHeader2.Width = 320;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToDateToolStripMenuItem,
            this.goToUpcomingOccurenceToolStripMenuItem,
            this.editThisOccasionToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteThisOccasionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(262, 114);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // goToDateToolStripMenuItem
            // 
            this.goToDateToolStripMenuItem.Name = "goToDateToolStripMenuItem";
            this.goToDateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.goToDateToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.goToDateToolStripMenuItem.Text = "פתח תאריך האירוע";
            this.goToDateToolStripMenuItem.Click += new System.EventHandler(this.goToDateToolStripMenuItem_Click);
            // 
            // goToUpcomingOccurenceToolStripMenuItem
            // 
            this.goToUpcomingOccurenceToolStripMenuItem.Name = "goToUpcomingOccurenceToolStripMenuItem";
            this.goToUpcomingOccurenceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.goToUpcomingOccurenceToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.goToUpcomingOccurenceToolStripMenuItem.Text = "פתח תאריך הבא";
            this.goToUpcomingOccurenceToolStripMenuItem.Click += new System.EventHandler(this.goToUpcomingOccurenceToolStripMenuItem_Click);
            // 
            // editThisOccasionToolStripMenuItem
            // 
            this.editThisOccasionToolStripMenuItem.Name = "editThisOccasionToolStripMenuItem";
            this.editThisOccasionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.editThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.editThisOccasionToolStripMenuItem.Text = "ערוך פרטי האירוע";
            this.editThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.editThisOccasionToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(258, 6);
            // 
            // deleteThisOccasionToolStripMenuItem
            // 
            this.deleteThisOccasionToolStripMenuItem.Name = "deleteThisOccasionToolStripMenuItem";
            this.deleteThisOccasionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.deleteThisOccasionToolStripMenuItem.Text = "מחק האירוע";
            this.deleteThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.deleteThisOccasionToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(9, 521);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(537, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "בחר שורה כדי ללכת לתאריך האירוע | לחץ פעמיים כדי לפתוח האירוע לעדכון";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // llExport
            // 
            this.llExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llExport.AutoSize = true;
            this.llExport.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llExport.Location = new System.Drawing.Point(803, 523);
            this.llExport.Name = "llExport";
            this.llExport.Size = new System.Drawing.Size(38, 17);
            this.llExport.TabIndex = 19;
            this.llExport.TabStop = true;
            this.llExport.Text = "ייצוא";
            this.llExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExport_LinkClicked);
            // 
            // llImport
            // 
            this.llImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llImport.AutoSize = true;
            this.llImport.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llImport.Location = new System.Drawing.Point(745, 523);
            this.llImport.Name = "llImport";
            this.llImport.Size = new System.Drawing.Size(39, 17);
            this.llImport.TabIndex = 20;
            this.llImport.TabStop = true;
            this.llImport.Text = "ייבוא";
            this.llImport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llImport_LinkClicked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(787, 523);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "|";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmOccasionListHeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(854, 620);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.llImport);
            this.Controls.Add(this.llExport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmOccasionListHeb";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "רשימת אירועים";
            this.Load += new System.EventHandler(this.frmOccasionListHeb_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.LinkLabel llExport;
        private System.Windows.Forms.LinkLabel llImport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goToDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToUpcomingOccurenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editThisOccasionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteThisOccasionToolStripMenuItem;
    }
}