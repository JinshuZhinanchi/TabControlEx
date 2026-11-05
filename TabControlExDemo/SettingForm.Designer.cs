
namespace TabControlExDemo
{
    partial class SettingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCuttingPath2 = new System.Windows.Forms.Button();
            this.buttonCuttingPath = new System.Windows.Forms.Button();
            this.buttonWorkOrder = new System.Windows.Forms.Button();
            this.textBoxCuttingPath2 = new System.Windows.Forms.TextBox();
            this.textBoxCuttingPath = new System.Windows.Forms.TextBox();
            this.textBoxWorkOrder = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCuttingPath2);
            this.groupBox1.Controls.Add(this.buttonCuttingPath);
            this.groupBox1.Controls.Add(this.buttonWorkOrder);
            this.groupBox1.Controls.Add(this.textBoxCuttingPath2);
            this.groupBox1.Controls.Add(this.textBoxCuttingPath);
            this.groupBox1.Controls.Add(this.textBoxWorkOrder);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(18, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "路径设置";
            // 
            // buttonCuttingPath2
            // 
            this.buttonCuttingPath2.Location = new System.Drawing.Point(444, 125);
            this.buttonCuttingPath2.Name = "buttonCuttingPath2";
            this.buttonCuttingPath2.Size = new System.Drawing.Size(46, 23);
            this.buttonCuttingPath2.TabIndex = 11;
            this.buttonCuttingPath2.Text = "设置";
            this.buttonCuttingPath2.UseVisualStyleBackColor = true;
            this.buttonCuttingPath2.Click += new System.EventHandler(this.buttonCuttingPath2_Click);
            // 
            // buttonCuttingPath
            // 
            this.buttonCuttingPath.Location = new System.Drawing.Point(444, 88);
            this.buttonCuttingPath.Name = "buttonCuttingPath";
            this.buttonCuttingPath.Size = new System.Drawing.Size(46, 23);
            this.buttonCuttingPath.TabIndex = 12;
            this.buttonCuttingPath.Text = "设置";
            this.buttonCuttingPath.UseVisualStyleBackColor = true;
            this.buttonCuttingPath.Click += new System.EventHandler(this.buttonCuttingPath_Click);
            // 
            // buttonWorkOrder
            // 
            this.buttonWorkOrder.Location = new System.Drawing.Point(444, 52);
            this.buttonWorkOrder.Name = "buttonWorkOrder";
            this.buttonWorkOrder.Size = new System.Drawing.Size(46, 23);
            this.buttonWorkOrder.TabIndex = 13;
            this.buttonWorkOrder.Text = "设置";
            this.buttonWorkOrder.UseVisualStyleBackColor = true;
            this.buttonWorkOrder.Click += new System.EventHandler(this.buttonWorkOrder_Click);
            // 
            // textBoxCuttingPath2
            // 
            this.textBoxCuttingPath2.Location = new System.Drawing.Point(141, 127);
            this.textBoxCuttingPath2.Name = "textBoxCuttingPath2";
            this.textBoxCuttingPath2.ReadOnly = true;
            this.textBoxCuttingPath2.Size = new System.Drawing.Size(297, 21);
            this.textBoxCuttingPath2.TabIndex = 8;
            // 
            // textBoxCuttingPath
            // 
            this.textBoxCuttingPath.Location = new System.Drawing.Point(141, 90);
            this.textBoxCuttingPath.Name = "textBoxCuttingPath";
            this.textBoxCuttingPath.ReadOnly = true;
            this.textBoxCuttingPath.Size = new System.Drawing.Size(297, 21);
            this.textBoxCuttingPath.TabIndex = 9;
            // 
            // textBoxWorkOrder
            // 
            this.textBoxWorkOrder.Location = new System.Drawing.Point(141, 52);
            this.textBoxWorkOrder.Name = "textBoxWorkOrder";
            this.textBoxWorkOrder.ReadOnly = true;
            this.textBoxWorkOrder.Size = new System.Drawing.Size(297, 21);
            this.textBoxWorkOrder.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(58, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 18);
            this.label9.TabIndex = 5;
            this.label9.Text = "切割路径2：";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(58, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "切割路径1：";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(58, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "工单路径：";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(155, 289);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(301, 21);
            this.textBoxDelay.TabIndex = 3;
            this.textBoxDelay.Text = "300";
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "页面停留时间（s）：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(575, 334);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(121, 47);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 405);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCuttingPath2;
        private System.Windows.Forms.Button buttonCuttingPath;
        private System.Windows.Forms.Button buttonWorkOrder;
        private System.Windows.Forms.TextBox textBoxCuttingPath2;
        private System.Windows.Forms.TextBox textBoxCuttingPath;
        private System.Windows.Forms.TextBox textBoxWorkOrder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonSave;
    }
}