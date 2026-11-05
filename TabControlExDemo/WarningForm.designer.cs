
namespace TabControlExDemo
{
    partial class WarningForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(399, 186);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(84, 31);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(12, 50);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(630, 93);
            this.labelInfo.TabIndex = 1;
            this.labelInfo.Text = "label1";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(545, 186);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(84, 31);
            this.buttonContinue.TabIndex = 3;
            this.buttonContinue.Text = "是";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // WarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 229);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonOK);
            this.Name = "WarningForm";
            this.Text = "WarningForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WarningForm_FormClosing);
            this.Load += new System.EventHandler(this.WarningForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonContinue;
    }
}