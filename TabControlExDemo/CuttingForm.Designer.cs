namespace TabControlExDemo
{
    partial class CuttingForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CuttingForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControlEx = new CSharpWin.TabControlEx();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(1105, 750);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControlEx);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1101, 746);
            this.panel2.TabIndex = 1;
            // 
            // tabControlEx
            // 
            this.tabControlEx.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx.ImageList = this.imageList1;
            this.tabControlEx.ItemSize = new System.Drawing.Size(200, 26);
            this.tabControlEx.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx.Name = "tabControlEx";
            this.tabControlEx.SelectedIndex = 0;
            this.tabControlEx.Size = new System.Drawing.Size(1101, 746);
            this.tabControlEx.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlEx.TabIndex = 0;
            this.tabControlEx.SelectedIndexChanged += new System.EventHandler(this.tabControlEx_SelectedIndexChanged);
            this.tabControlEx.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlEx_Selecting);
            this.tabControlEx.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tabControlEx_Deselected);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Network Neighborhood.ico");
            this.imageList1.Images.SetKeyName(1, "Open Folder.ico");
            this.imageList1.Images.SetKeyName(2, "Printers2.ico");
            this.imageList1.Images.SetKeyName(3, "Printers.ico");
            this.imageList1.Images.SetKeyName(4, "Program Group.ico");
            this.imageList1.Images.SetKeyName(5, "Programs.ico");
            this.imageList1.Images.SetKeyName(6, "RAM Drive.ico");
            this.imageList1.Images.SetKeyName(7, "Recycle Bin empty.ico");
            this.imageList1.Images.SetKeyName(8, "Recycle Bin Full.ico");
            this.imageList1.Images.SetKeyName(9, "Removable Drive.ico");
            // 
            // CuttingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 750);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "CuttingForm";
            this.Text = "首镭全自动激光打码系统V3.3.2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CuttingForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CuttingForm_FormClosed);
            this.Load += new System.EventHandler(this.CuttingForm_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList1;
        private CSharpWin.TabControlEx tabControlEx;
        private System.Windows.Forms.Panel panel2;
    }
}

