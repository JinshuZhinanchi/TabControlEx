namespace MyCommanLib
{
    partial class UserControl1
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOperater = new System.Windows.Forms.TextBox();
            this.textBoxTechnician = new System.Windows.Forms.TextBox();
            this.textBoxEngineer = new System.Windows.Forms.TextBox();
            this.textBoxAdmin = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(73, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "操作员密码：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(73, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "技术员密码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(73, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "工程师密码：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(73, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "管理员密码：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOperater
            // 
            this.textBoxOperater.Location = new System.Drawing.Point(189, 53);
            this.textBoxOperater.Name = "textBoxOperater";
            this.textBoxOperater.PasswordChar = '*';
            this.textBoxOperater.Size = new System.Drawing.Size(337, 21);
            this.textBoxOperater.TabIndex = 1;
            this.textBoxOperater.Text = "123456";
            this.textBoxOperater.TextChanged += new System.EventHandler(this.textBoxOperater_TextChanged);
            // 
            // textBoxTechnician
            // 
            this.textBoxTechnician.Location = new System.Drawing.Point(189, 114);
            this.textBoxTechnician.Name = "textBoxTechnician";
            this.textBoxTechnician.PasswordChar = '*';
            this.textBoxTechnician.Size = new System.Drawing.Size(337, 21);
            this.textBoxTechnician.TabIndex = 1;
            this.textBoxTechnician.Text = "123456";
            this.textBoxTechnician.TextChanged += new System.EventHandler(this.textBoxTechnician_TextChanged);
            // 
            // textBoxEngineer
            // 
            this.textBoxEngineer.Location = new System.Drawing.Point(189, 177);
            this.textBoxEngineer.Name = "textBoxEngineer";
            this.textBoxEngineer.PasswordChar = '*';
            this.textBoxEngineer.Size = new System.Drawing.Size(337, 21);
            this.textBoxEngineer.TabIndex = 1;
            this.textBoxEngineer.Text = "123456";
            this.textBoxEngineer.TextChanged += new System.EventHandler(this.textBoxEngineer_TextChanged);
            // 
            // textBoxAdmin
            // 
            this.textBoxAdmin.Location = new System.Drawing.Point(189, 240);
            this.textBoxAdmin.Name = "textBoxAdmin";
            this.textBoxAdmin.PasswordChar = '*';
            this.textBoxAdmin.Size = new System.Drawing.Size(337, 21);
            this.textBoxAdmin.TabIndex = 1;
            this.textBoxAdmin.Text = "123456";
            this.textBoxAdmin.TextChanged += new System.EventHandler(this.textBoxAdmin_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(584, 321);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(138, 50);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxAdmin);
            this.Controls.Add(this.textBoxEngineer);
            this.Controls.Add(this.textBoxTechnician);
            this.Controls.Add(this.textBoxOperater);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UserControl1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxOperater;
        private System.Windows.Forms.TextBox textBoxTechnician;
        private System.Windows.Forms.TextBox textBoxEngineer;
        private System.Windows.Forms.TextBox textBoxAdmin;
        private System.Windows.Forms.Button buttonSave;
    }
}
