
namespace DemoCom
{
    partial class SerialPortForm
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
            this.richTextBoxReceive = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.comboBoxstopBits = new System.Windows.Forms.ComboBox();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxNormal = new System.Windows.Forms.RichTextBox();
            this.buttonClearSend = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.richTextBoxSend = new System.Windows.Forms.RichTextBox();
            this.buttonClearReceive = new System.Windows.Forms.Button();
            this.checkBoxHex = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_TestSerialPort = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxReceive
            // 
            this.richTextBoxReceive.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.richTextBoxReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxReceive.ForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxReceive.Location = new System.Drawing.Point(4, 22);
            this.richTextBoxReceive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxReceive.Name = "richTextBoxReceive";
            this.richTextBoxReceive.Size = new System.Drawing.Size(736, 557);
            this.richTextBoxReceive.TabIndex = 0;
            this.richTextBoxReceive.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxReceive);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(744, 583);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据接收区";
            // 
            // buttonOpen
            // 
            this.buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpen.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonOpen.Location = new System.Drawing.Point(156, 410);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(144, 48);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "打开串口";
            this.buttonOpen.UseVisualStyleBackColor = false;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // comboBoxstopBits
            // 
            this.comboBoxstopBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxstopBits.FormattingEnabled = true;
            this.comboBoxstopBits.Items.AddRange(new object[] {
            "None",
            "One",
            "Two"});
            this.comboBoxstopBits.Location = new System.Drawing.Point(156, 344);
            this.comboBoxstopBits.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxstopBits.Name = "comboBoxstopBits";
            this.comboBoxstopBits.Size = new System.Drawing.Size(144, 23);
            this.comboBoxstopBits.TabIndex = 1;
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.comboBoxParity.Location = new System.Drawing.Point(156, 265);
            this.comboBoxParity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(144, 23);
            this.comboBoxParity.TabIndex = 1;
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.comboBoxDataBits.Location = new System.Drawing.Point(156, 186);
            this.comboBoxDataBits.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(144, 23);
            this.comboBoxDataBits.TabIndex = 1;
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(156, 107);
            this.comboBoxBaudRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(144, 23);
            this.comboBoxBaudRate.TabIndex = 1;
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(156, 28);
            this.comboBoxPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(144, 23);
            this.comboBoxPort.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 269);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "校验位：";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 190);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "数据位：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 348);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "停止位：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 111);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "波特率：";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号：";
            // 
            // richTextBoxNormal
            // 
            this.richTextBoxNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxNormal.Location = new System.Drawing.Point(4, 62);
            this.richTextBoxNormal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxNormal.Name = "richTextBoxNormal";
            this.richTextBoxNormal.ReadOnly = true;
            this.richTextBoxNormal.Size = new System.Drawing.Size(721, 50);
            this.richTextBoxNormal.TabIndex = 3;
            this.richTextBoxNormal.Text = "111111111111111111111111111111111111111111111111111111111111111111111111111111111" +
    "11111111111111111";
            // 
            // buttonClearSend
            // 
            this.buttonClearSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearSend.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonClearSend.Location = new System.Drawing.Point(4, 82);
            this.buttonClearSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClearSend.Name = "buttonClearSend";
            this.buttonClearSend.Size = new System.Drawing.Size(148, 48);
            this.buttonClearSend.TabIndex = 2;
            this.buttonClearSend.Text = "清空发送";
            this.buttonClearSend.UseVisualStyleBackColor = false;
            this.buttonClearSend.Click += new System.EventHandler(this.buttonClearSend_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonSend.Location = new System.Drawing.Point(4, 11);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(148, 48);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // richTextBoxSend
            // 
            this.richTextBoxSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxSend.Location = new System.Drawing.Point(4, 4);
            this.richTextBoxSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxSend.Name = "richTextBoxSend";
            this.richTextBoxSend.Size = new System.Drawing.Size(721, 50);
            this.richTextBoxSend.TabIndex = 0;
            this.richTextBoxSend.Text = "";
            // 
            // buttonClearReceive
            // 
            this.buttonClearReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearReceive.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonClearReceive.Location = new System.Drawing.Point(156, 491);
            this.buttonClearReceive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClearReceive.Name = "buttonClearReceive";
            this.buttonClearReceive.Size = new System.Drawing.Size(144, 48);
            this.buttonClearReceive.TabIndex = 0;
            this.buttonClearReceive.Text = "清空接收";
            this.buttonClearReceive.UseVisualStyleBackColor = false;
            this.buttonClearReceive.Click += new System.EventHandler(this.buttonClearReceive_Click);
            // 
            // checkBoxHex
            // 
            this.checkBoxHex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxHex.AutoSize = true;
            this.checkBoxHex.Location = new System.Drawing.Point(4, 506);
            this.checkBoxHex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHex.Name = "checkBoxHex";
            this.checkBoxHex.Size = new System.Drawing.Size(75, 19);
            this.checkBoxHex.TabIndex = 4;
            this.checkBoxHex.Text = "16进制";
            this.checkBoxHex.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1099, 766);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(4, 595);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(744, 142);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "数据发送区";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel2.Controls.Add(this.richTextBoxNormal, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.richTextBoxSend, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 22);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(736, 116);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonClearSend, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonSend, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn_TestSerialPort, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(756, 595);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(312, 142);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(756, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(312, 583);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "参数设置";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.buttonOpen, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxPort, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxHex, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.buttonClearReceive, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxstopBits, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxBaudRate, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxParity, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.comboBoxDataBits, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 22);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 7;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(304, 557);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // btn_TestSerialPort
            // 
            this.btn_TestSerialPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_TestSerialPort.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_TestSerialPort.Location = new System.Drawing.Point(160, 11);
            this.btn_TestSerialPort.Margin = new System.Windows.Forms.Padding(4);
            this.btn_TestSerialPort.Name = "btn_TestSerialPort";
            this.btn_TestSerialPort.Size = new System.Drawing.Size(148, 48);
            this.btn_TestSerialPort.TabIndex = 5;
            this.btn_TestSerialPort.Text = "串口测试";
            this.btn_TestSerialPort.UseVisualStyleBackColor = false;
            // 
            // SerialPortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 766);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SerialPortForm";
            this.Text = "SerialPortForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxReceive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.ComboBox comboBoxstopBits;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonClearSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.RichTextBox richTextBoxSend;
        private System.Windows.Forms.Button buttonClearReceive;
        private System.Windows.Forms.CheckBox checkBoxHex;
        private System.Windows.Forms.RichTextBox richTextBoxNormal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btn_TestSerialPort;
    }
}