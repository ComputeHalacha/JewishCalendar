namespace LuachProject
{
    partial class frmOccasionList
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
            this.editThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteThisOccasionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.llExportList = new System.Windows.Forms.LinkLabel();
            this.llImportList = new System.Windows.Forms.LinkLabel();
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
            this.btnClose.Location = new System.Drawing.Point(882, 583);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(107, 33);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(655, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search for an occasion:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(14, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(975, 23);
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
            this.listView1.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Bold);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(14, 75);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(975, 474);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Occasion Name";
            this.columnHeader3.Width = 288;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date";
            this.columnHeader1.Width = 340;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Occasion Type";
            this.columnHeader2.Width = 330;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToDateToolStripMenuItem,
            this.editThisOccasionToolStripMenuItem,
            this.deleteThisOccasionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 110);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // goToDateToolStripMenuItem
            // 
            this.goToDateToolStripMenuItem.Name = "goToDateToolStripMenuItem";
            this.goToDateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.goToDateToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.goToDateToolStripMenuItem.Text = "Go to &Date";
            this.goToDateToolStripMenuItem.Click += new System.EventHandler(this.goToDateToolStripMenuItem_Click);
            // 
            // editThisOccasionToolStripMenuItem
            // 
            this.editThisOccasionToolStripMenuItem.Name = "editThisOccasionToolStripMenuItem";
            this.editThisOccasionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.editThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.editThisOccasionToolStripMenuItem.Text = "&Edit this Occasion";
            this.editThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.editThisOccasionToolStripMenuItem_Click);
            // 
            // deleteThisOccasionToolStripMenuItem
            // 
            this.deleteThisOccasionToolStripMenuItem.Name = "deleteThisOccasionToolStripMenuItem";
            this.deleteThisOccasionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteThisOccasionToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.deleteThisOccasionToolStripMenuItem.Text = "&Delete this Occasion";
            this.deleteThisOccasionToolStripMenuItem.Click += new System.EventHandler(this.deleteThisOccasionToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(14, 552);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(523, 27);
            this.label2.TabIndex = 18;
            this.label2.Text = "Select an occasion to navigate to that Date | Double click an occasion to open it" +
    " for editing";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // llExportList
            // 
            this.llExportList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llExportList.AutoSize = true;
            this.llExportList.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llExportList.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llExportList.Location = new System.Drawing.Point(833, 553);
            this.llExportList.Name = "llExportList";
            this.llExportList.Size = new System.Drawing.Size(64, 16);
            this.llExportList.TabIndex = 19;
            this.llExportList.TabStop = true;
            this.llExportList.Text = "EXPORT";
            this.llExportList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.llExportList, "Export list of occasions to a file");
            this.llExportList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExportList_LinkClicked);
            // 
            // llImportList
            // 
            this.llImportList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llImportList.AutoSize = true;
            this.llImportList.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.llImportList.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llImportList.Location = new System.Drawing.Point(925, 553);
            this.llImportList.Name = "llImportList";
            this.llImportList.Size = new System.Drawing.Size(61, 16);
            this.llImportList.TabIndex = 20;
            this.llImportList.TabStop = true;
            this.llImportList.Text = "IMPORT";
            this.llImportList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.llImportList, "Import occasions from a previously exported file");
            this.llImportList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llImportList_LinkClicked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(902, 553);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 19);
            this.label1.TabIndex = 21;
            this.label1.Text = "|";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmOccasionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1003, 628);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.llImportList);
            this.Controls.Add(this.llExportList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Century Gothic", 7.8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmOccasionList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List of Occasions";
            this.Load += new System.EventHandler(this.frmOccasionListEng_Load);
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
        private System.Windows.Forms.LinkLabel llExportList;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel llImportList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem goToDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editThisOccasionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteThisOccasionToolStripMenuItem;
    }
}