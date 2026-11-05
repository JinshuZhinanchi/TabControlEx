
namespace TabControlExDemo
{
    partial class IOCutForm
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
            this.tabControlIO = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlIO
            // 
            this.tabControlIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlIO.Location = new System.Drawing.Point(0, 0);
            this.tabControlIO.Name = "tabControlIO";
            this.tabControlIO.SelectedIndex = 0;
            this.tabControlIO.Size = new System.Drawing.Size(984, 761);
            this.tabControlIO.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlIO.TabIndex = 0;
            // 
            // IOCutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tabControlIO);
            this.Name = "IOCutForm";
            this.Text = "IOCutForm";
            this.SizeChanged += new System.EventHandler(this.IOCutForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlIO;
    }
}