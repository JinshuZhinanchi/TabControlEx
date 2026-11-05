
namespace jog
{
    partial class GtsForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelAxis1 = new System.Windows.Forms.Panel();
            this.panelAxis2 = new System.Windows.Forms.Panel();
            this.panelAxis3 = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.panelAxis1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelAxis2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelAxis3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonApply, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(912, 580);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panelAxis1
            // 
            this.panelAxis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAxis1.Location = new System.Drawing.Point(3, 3);
            this.panelAxis1.Name = "panelAxis1";
            this.panelAxis1.Size = new System.Drawing.Size(298, 534);
            this.panelAxis1.TabIndex = 0;
            // 
            // panelAxis2
            // 
            this.panelAxis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAxis2.Location = new System.Drawing.Point(307, 3);
            this.panelAxis2.Name = "panelAxis2";
            this.panelAxis2.Size = new System.Drawing.Size(298, 534);
            this.panelAxis2.TabIndex = 0;
            // 
            // panelAxis3
            // 
            this.panelAxis3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAxis3.Location = new System.Drawing.Point(611, 3);
            this.panelAxis3.Name = "panelAxis3";
            this.panelAxis3.Size = new System.Drawing.Size(298, 534);
            this.panelAxis3.TabIndex = 0;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonApply.Location = new System.Drawing.Point(611, 546);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 28);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "应用";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonSetting
            // 
            this.buttonSetting.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonSetting.Location = new System.Drawing.Point(223, 6);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(75, 28);
            this.buttonSetting.TabIndex = 1;
            this.buttonSetting.Text = "手动设置";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonSetting);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(307, 543);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 34);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "尺寸：";
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.FormattingEnabled = true;
            this.comboBoxSize.Items.AddRange(new object[] {
            "6 Inch",
            "8 Inch"});
            this.comboBoxSize.Location = new System.Drawing.Point(69, 11);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(121, 20);
            this.comboBoxSize.TabIndex = 3;
            // 
            // GtsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 580);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GtsForm";
            this.Text = "GtsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GtsForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelAxis1;
        private System.Windows.Forms.Panel panelAxis2;
        private System.Windows.Forms.Panel panelAxis3;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxSize;
        private System.Windows.Forms.Label label1;
    }
}