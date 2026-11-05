namespace TabControlExDemo
{
    partial class TCPForm
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
            this.conButton = new System.Windows.Forms.Button();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.IPPortTextBox = new System.Windows.Forms.TextBox();
            this.IPAddressTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stateLabel = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.Label();
            this.sendButton = new System.Windows.Forms.Button();
            this.sendTextBox = new System.Windows.Forms.TextBox();
            this.checkBoxCCD = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLocalIP = new System.Windows.Forms.TextBox();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // conButton
            // 
            this.conButton.Location = new System.Drawing.Point(693, 349);
            this.conButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.conButton.Name = "conButton";
            this.conButton.Size = new System.Drawing.Size(107, 26);
            this.conButton.TabIndex = 14;
            this.conButton.Text = "连接";
            this.conButton.UseVisualStyleBackColor = true;
            this.conButton.Click += new System.EventHandler(this.conButton_Click);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(24, 139);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.resultTextBox.Multiline = true;
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(775, 197);
            this.resultTextBox.TabIndex = 13;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(585, 22);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 29);
            this.startButton.TabIndex = 12;
            this.startButton.Text = "开始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Visible = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // IPPortTextBox
            // 
            this.IPPortTextBox.Location = new System.Drawing.Point(365, 25);
            this.IPPortTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IPPortTextBox.Name = "IPPortTextBox";
            this.IPPortTextBox.Size = new System.Drawing.Size(132, 25);
            this.IPPortTextBox.TabIndex = 11;
            this.IPPortTextBox.Text = "9500";
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.Location = new System.Drawing.Point(95, 21);
            this.IPAddressTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(164, 25);
            this.IPAddressTextBox.TabIndex = 10;
            this.IPAddressTextBox.Text = "192.168.1.5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(303, 29);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "端口：";
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(21, 105);
            this.stateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(97, 15);
            this.stateLabel.TabIndex = 8;
            this.stateLabel.Text = "空闲中。。。";
            // 
            // ipAddress
            // 
            this.ipAddress.AutoSize = true;
            this.ipAddress.Location = new System.Drawing.Point(19, 25);
            this.ipAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(67, 15);
            this.ipAddress.TabIndex = 9;
            this.ipAddress.Text = "服务器：";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(693, 636);
            this.sendButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(100, 29);
            this.sendButton.TabIndex = 16;
            this.sendButton.Text = "发送";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // sendTextBox
            // 
            this.sendTextBox.Location = new System.Drawing.Point(24, 419);
            this.sendTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sendTextBox.Multiline = true;
            this.sendTextBox.Name = "sendTextBox";
            this.sendTextBox.Size = new System.Drawing.Size(775, 197);
            this.sendTextBox.TabIndex = 15;
            // 
            // checkBoxCCD
            // 
            this.checkBoxCCD.AutoSize = true;
            this.checkBoxCCD.Checked = true;
            this.checkBoxCCD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCCD.Location = new System.Drawing.Point(405, 104);
            this.checkBoxCCD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxCCD.Name = "checkBoxCCD";
            this.checkBoxCCD.Size = new System.Drawing.Size(119, 19);
            this.checkBoxCCD.TabIndex = 17;
            this.checkBoxCCD.Text = "使用该项功能";
            this.checkBoxCCD.UseVisualStyleBackColor = true;
            this.checkBoxCCD.CheckedChanged += new System.EventHandler(this.checkBoxCCD_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "本机：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(303, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "端口：";
            // 
            // textBoxLocalIP
            // 
            this.textBoxLocalIP.Location = new System.Drawing.Point(95, 59);
            this.textBoxLocalIP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLocalIP.Name = "textBoxLocalIP";
            this.textBoxLocalIP.Size = new System.Drawing.Size(164, 25);
            this.textBoxLocalIP.TabIndex = 10;
            this.textBoxLocalIP.Text = "192.168.1.6";
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.Location = new System.Drawing.Point(365, 62);
            this.textBoxLocalPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(132, 25);
            this.textBoxLocalPort.TabIndex = 11;
            this.textBoxLocalPort.Text = "6000";
            // 
            // TCPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 688);
            this.Controls.Add(this.checkBoxCCD);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.sendTextBox);
            this.Controls.Add(this.conButton);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.textBoxLocalPort);
            this.Controls.Add(this.IPPortTextBox);
            this.Controls.Add(this.textBoxLocalIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IPAddressTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.ipAddress);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TCPForm";
            this.Text = "TcpIpCS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TcpIpCS_FormClosing);
            this.SizeChanged += new System.EventHandler(this.TCPForm_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button conButton;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox IPPortTextBox;
        private System.Windows.Forms.TextBox IPAddressTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label ipAddress;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox sendTextBox;
        private System.Windows.Forms.CheckBox checkBoxCCD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLocalIP;
        private System.Windows.Forms.TextBox textBoxLocalPort;
    }
}