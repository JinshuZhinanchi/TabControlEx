
namespace TabControlExDemo
{
    partial class ScanForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFeedCastleID = new System.Windows.Forms.TextBox();
            this.textBoxCutCastleID = new System.Windows.Forms.TextBox();
            this.textBoxSmifPodID = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBoxIndex = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "上料CastleID：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "下料CastleID：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "SmifPodID：";
            // 
            // textBoxFeedCastleID
            // 
            this.textBoxFeedCastleID.Location = new System.Drawing.Point(228, 106);
            this.textBoxFeedCastleID.Name = "textBoxFeedCastleID";
            this.textBoxFeedCastleID.Size = new System.Drawing.Size(345, 25);
            this.textBoxFeedCastleID.TabIndex = 3;
            // 
            // textBoxCutCastleID
            // 
            this.textBoxCutCastleID.Location = new System.Drawing.Point(228, 179);
            this.textBoxCutCastleID.Name = "textBoxCutCastleID";
            this.textBoxCutCastleID.Size = new System.Drawing.Size(345, 25);
            this.textBoxCutCastleID.TabIndex = 3;
            // 
            // textBoxSmifPodID
            // 
            this.textBoxSmifPodID.Location = new System.Drawing.Point(228, 248);
            this.textBoxSmifPodID.Name = "textBoxSmifPodID";
            this.textBoxSmifPodID.Size = new System.Drawing.Size(345, 25);
            this.textBoxSmifPodID.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(249, 310);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(147, 43);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(426, 310);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(147, 43);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "上料料盒：";
            // 
            // comboBoxBoxIndex
            // 
            this.comboBoxBoxIndex.FormattingEnabled = true;
            this.comboBoxBoxIndex.Items.AddRange(new object[] {
            "料盒1",
            "料盒2"});
            this.comboBoxBoxIndex.Location = new System.Drawing.Point(228, 55);
            this.comboBoxBoxIndex.Name = "comboBoxBoxIndex";
            this.comboBoxBoxIndex.Size = new System.Drawing.Size(345, 23);
            this.comboBoxBoxIndex.TabIndex = 5;
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 388);
            this.Controls.Add(this.comboBoxBoxIndex);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxSmifPodID);
            this.Controls.Add(this.textBoxCutCastleID);
            this.Controls.Add(this.textBoxFeedCastleID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "ScanForm";
            this.Text = "ScanForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFeedCastleID;
        private System.Windows.Forms.TextBox textBoxCutCastleID;
        private System.Windows.Forms.TextBox textBoxSmifPodID;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxBoxIndex;
    }
}