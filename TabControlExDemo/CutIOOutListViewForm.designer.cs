
namespace TabControlExDemo
{
    partial class CutIOOutListViewForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewIO1 = new System.Windows.Forms.ListView();
            this.IOName1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IOAddress1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IOState1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listViewIO2 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewIO3 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewIO4 = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.listViewIO1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewIO2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewIO3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewIO4, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 761);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // listViewIO1
            // 
            this.listViewIO1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IOName1,
            this.IOAddress1,
            this.IOState1});
            this.listViewIO1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewIO1.GridLines = true;
            this.listViewIO1.HideSelection = false;
            this.listViewIO1.Location = new System.Drawing.Point(3, 3);
            this.listViewIO1.MultiSelect = false;
            this.listViewIO1.Name = "listViewIO1";
            this.listViewIO1.Size = new System.Drawing.Size(240, 755);
            this.listViewIO1.SmallImageList = this.imageList1;
            this.listViewIO1.TabIndex = 0;
            this.listViewIO1.UseCompatibleStateImageBehavior = false;
            this.listViewIO1.View = System.Windows.Forms.View.Details;
            this.listViewIO1.SelectedIndexChanged += new System.EventHandler(this.listViewIO1_SelectedIndexChanged);
            this.listViewIO1.DoubleClick += new System.EventHandler(this.listViewIO1_DoubleClick);
            // 
            // IOName1
            // 
            this.IOName1.Text = "名称";
            this.IOName1.Width = 139;
            // 
            // IOAddress1
            // 
            this.IOAddress1.Text = "地址";
            this.IOAddress1.Width = 64;
            // 
            // IOState1
            // 
            this.IOState1.Text = "状态";
            this.IOState1.Width = 30;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(5, 30);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listViewIO2
            // 
            this.listViewIO2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewIO2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewIO2.GridLines = true;
            this.listViewIO2.HideSelection = false;
            this.listViewIO2.Location = new System.Drawing.Point(249, 3);
            this.listViewIO2.MultiSelect = false;
            this.listViewIO2.Name = "listViewIO2";
            this.listViewIO2.Size = new System.Drawing.Size(240, 755);
            this.listViewIO2.SmallImageList = this.imageList1;
            this.listViewIO2.TabIndex = 0;
            this.listViewIO2.UseCompatibleStateImageBehavior = false;
            this.listViewIO2.View = System.Windows.Forms.View.Details;
            this.listViewIO2.DoubleClick += new System.EventHandler(this.listViewIO2_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "地址";
            this.columnHeader2.Width = 64;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "状态";
            this.columnHeader3.Width = 30;
            // 
            // listViewIO3
            // 
            this.listViewIO3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listViewIO3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewIO3.GridLines = true;
            this.listViewIO3.HideSelection = false;
            this.listViewIO3.Location = new System.Drawing.Point(495, 3);
            this.listViewIO3.MultiSelect = false;
            this.listViewIO3.Name = "listViewIO3";
            this.listViewIO3.Size = new System.Drawing.Size(240, 755);
            this.listViewIO3.SmallImageList = this.imageList1;
            this.listViewIO3.TabIndex = 0;
            this.listViewIO3.UseCompatibleStateImageBehavior = false;
            this.listViewIO3.View = System.Windows.Forms.View.Details;
            this.listViewIO3.DoubleClick += new System.EventHandler(this.listViewIO3_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "名称";
            this.columnHeader4.Width = 142;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "地址";
            this.columnHeader5.Width = 64;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "状态";
            this.columnHeader6.Width = 30;
            // 
            // listViewIO4
            // 
            this.listViewIO4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listViewIO4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewIO4.GridLines = true;
            this.listViewIO4.HideSelection = false;
            this.listViewIO4.Location = new System.Drawing.Point(741, 3);
            this.listViewIO4.MultiSelect = false;
            this.listViewIO4.Name = "listViewIO4";
            this.listViewIO4.Size = new System.Drawing.Size(240, 755);
            this.listViewIO4.SmallImageList = this.imageList1;
            this.listViewIO4.TabIndex = 0;
            this.listViewIO4.UseCompatibleStateImageBehavior = false;
            this.listViewIO4.View = System.Windows.Forms.View.Details;
            this.listViewIO4.DoubleClick += new System.EventHandler(this.listViewIO4_DoubleClick);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "名称";
            this.columnHeader7.Width = 140;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "地址";
            this.columnHeader8.Width = 64;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "状态";
            this.columnHeader9.Width = 30;
            // 
            // CutIOOutListViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CutIOOutListViewForm";
            this.Text = "CutIOOutListViewForm";
            this.SizeChanged += new System.EventHandler(this.CutIOOutListViewForm_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView listViewIO1;
        private System.Windows.Forms.ColumnHeader IOName1;
        private System.Windows.Forms.ColumnHeader IOAddress1;
        private System.Windows.Forms.ColumnHeader IOState1;
        private System.Windows.Forms.ListView listViewIO2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView listViewIO3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ListView listViewIO4;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ImageList imageList1;
    }
}