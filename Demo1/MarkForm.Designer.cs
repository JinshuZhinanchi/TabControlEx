namespace EzdDemo
{
    partial class MarkForm
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
            this.buttonModify = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_MarkCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonWafer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxWaferFile = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.comboBoxProduct = new System.Windows.Forms.ComboBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.comboBoxMarkType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxMarkTransp = new System.Windows.Forms.CheckBox();
            this.textBoxPowerMax = new System.Windows.Forms.TextBox();
            this.textBoxPowerMin = new System.Windows.Forms.TextBox();
            this.numericUpDownTimes = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonGasWork = new System.Windows.Forms.Button();
            this.buttonTestPowerResult = new System.Windows.Forms.Button();
            this.buttonPower = new System.Windows.Forms.Button();
            this.buttonGasOrg = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.FaceMark = new System.Windows.Forms.TabPage();
            this.panelMark1 = new System.Windows.Forms.Panel();
            this.sideMark = new System.Windows.Forms.TabPage();
            this.panelMark2 = new System.Windows.Forms.Panel();
            this.tabPageTest = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOCRGas = new System.Windows.Forms.ComboBox();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimes)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.FaceMark.SuspendLayout();
            this.sideMark.SuspendLayout();
            this.tabPageTest.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonModify
            // 
            this.buttonModify.Location = new System.Drawing.Point(539, 10);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(75, 23);
            this.buttonModify.TabIndex = 28;
            this.buttonModify.Text = "手动修改";
            this.buttonModify.UseVisualStyleBackColor = true;
            this.buttonModify.Click += new System.EventHandler(this.buttonModify_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxOCRGas);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.tb_MarkCount);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.buttonSave);
            this.groupBox4.Controls.Add(this.buttonWafer);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.textBoxWaferFile);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.buttonSetting);
            this.groupBox4.Controls.Add(this.comboBoxProduct);
            this.groupBox4.Controls.Add(this.buttonApply);
            this.groupBox4.Controls.Add(this.buttonModify);
            this.groupBox4.Controls.Add(this.comboBoxMarkType);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.comboBoxSize);
            this.groupBox4.Controls.Add(this.textBoxLength);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.checkBoxMarkTransp);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(617, 114);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "基本信息";
            // 
            // tb_MarkCount
            // 
            this.tb_MarkCount.Location = new System.Drawing.Point(319, 83);
            this.tb_MarkCount.Name = "tb_MarkCount";
            this.tb_MarkCount.Size = new System.Drawing.Size(27, 25);
            this.tb_MarkCount.TabIndex = 35;
            this.tb_MarkCount.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "打标次数:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(459, 38);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 33;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonWafer
            // 
            this.buttonWafer.Location = new System.Drawing.Point(457, 9);
            this.buttonWafer.Margin = new System.Windows.Forms.Padding(2);
            this.buttonWafer.Name = "buttonWafer";
            this.buttonWafer.Size = new System.Drawing.Size(77, 24);
            this.buttonWafer.TabIndex = 32;
            this.buttonWafer.Text = "打开晶圆模版";
            this.buttonWafer.UseVisualStyleBackColor = true;
            this.buttonWafer.Click += new System.EventHandler(this.buttonWafer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 30;
            this.label1.Text = "晶圆模版";
            // 
            // textBoxWaferFile
            // 
            this.textBoxWaferFile.Location = new System.Drawing.Point(77, 12);
            this.textBoxWaferFile.Name = "textBoxWaferFile";
            this.textBoxWaferFile.ReadOnly = true;
            this.textBoxWaferFile.Size = new System.Drawing.Size(317, 25);
            this.textBoxWaferFile.TabIndex = 29;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 15);
            this.label13.TabIndex = 27;
            this.label13.Text = "产品类型:";
            // 
            // buttonSetting
            // 
            this.buttonSetting.Enabled = false;
            this.buttonSetting.Location = new System.Drawing.Point(400, 11);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(52, 23);
            this.buttonSetting.TabIndex = 27;
            this.buttonSetting.Text = "EZCAD2";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Visible = false;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // comboBoxProduct
            // 
            this.comboBoxProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProduct.Location = new System.Drawing.Point(82, 46);
            this.comboBoxProduct.Name = "comboBoxProduct";
            this.comboBoxProduct.Size = new System.Drawing.Size(93, 23);
            this.comboBoxProduct.TabIndex = 26;
            this.comboBoxProduct.SelectedIndexChanged += new System.EventHandler(this.comboBoxProduct_SelectedIndexChanged);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(539, 82);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(62, 23);
            this.buttonApply.TabIndex = 28;
            this.buttonApply.Text = "应用";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // comboBoxMarkType
            // 
            this.comboBoxMarkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarkType.Items.AddRange(new object[] {
            "不打标",
            "Si打标(Si.job)",
            "SiC打标(SiC.job)"});
            this.comboBoxMarkType.Location = new System.Drawing.Point(255, 46);
            this.comboBoxMarkType.Name = "comboBoxMarkType";
            this.comboBoxMarkType.Size = new System.Drawing.Size(91, 23);
            this.comboBoxMarkType.TabIndex = 23;
            this.comboBoxMarkType.SelectedIndexChanged += new System.EventHandler(this.comboBoxMarkType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(180, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 15);
            this.label11.TabIndex = 27;
            this.label11.Text = "打标类型:";
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.FormattingEnabled = true;
            this.comboBoxSize.Items.AddRange(new object[] {
            "6 Inch",
            "8 Inch"});
            this.comboBoxSize.Location = new System.Drawing.Point(73, 214);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(78, 23);
            this.comboBoxSize.TabIndex = 23;
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(71, 240);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(80, 25);
            this.textBoxLength.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 20;
            this.label9.Text = "平边长度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 217);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "片尺寸：";
            // 
            // checkBoxMarkTransp
            // 
            this.checkBoxMarkTransp.AutoSize = true;
            this.checkBoxMarkTransp.Location = new System.Drawing.Point(228, 243);
            this.checkBoxMarkTransp.Name = "checkBoxMarkTransp";
            this.checkBoxMarkTransp.Size = new System.Drawing.Size(59, 19);
            this.checkBoxMarkTransp.TabIndex = 21;
            this.checkBoxMarkTransp.Text = "透明";
            this.checkBoxMarkTransp.UseVisualStyleBackColor = true;
            // 
            // textBoxPowerMax
            // 
            this.textBoxPowerMax.Location = new System.Drawing.Point(406, 89);
            this.textBoxPowerMax.Name = "textBoxPowerMax";
            this.textBoxPowerMax.Size = new System.Drawing.Size(53, 25);
            this.textBoxPowerMax.TabIndex = 28;
            this.textBoxPowerMax.Text = "5";
            this.textBoxPowerMax.Visible = false;
            this.textBoxPowerMax.TextChanged += new System.EventHandler(this.textBoxPowerMax_TextChanged);
            // 
            // textBoxPowerMin
            // 
            this.textBoxPowerMin.Location = new System.Drawing.Point(233, 89);
            this.textBoxPowerMin.Name = "textBoxPowerMin";
            this.textBoxPowerMin.Size = new System.Drawing.Size(52, 25);
            this.textBoxPowerMin.TabIndex = 28;
            this.textBoxPowerMin.Text = "5";
            this.textBoxPowerMin.Visible = false;
            this.textBoxPowerMin.TextChanged += new System.EventHandler(this.textBoxPowerMin_TextChanged);
            // 
            // numericUpDownTimes
            // 
            this.numericUpDownTimes.Location = new System.Drawing.Point(102, 148);
            this.numericUpDownTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimes.Name = "numericUpDownTimes";
            this.numericUpDownTimes.Size = new System.Drawing.Size(76, 25);
            this.numericUpDownTimes.TabIndex = 27;
            this.numericUpDownTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimes.Visible = false;
            this.numericUpDownTimes.ValueChanged += new System.EventHandler(this.numericUpDownTimes_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(347, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 26;
            this.label6.Text = "最大值：";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(174, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 26;
            this.label5.Text = "最小值：";
            this.label5.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 26;
            this.label7.Text = "测试频率";
            this.label7.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "功率范围";
            this.label4.Visible = false;
            // 
            // buttonGasWork
            // 
            this.buttonGasWork.Location = new System.Drawing.Point(338, 33);
            this.buttonGasWork.Name = "buttonGasWork";
            this.buttonGasWork.Size = new System.Drawing.Size(75, 23);
            this.buttonGasWork.TabIndex = 24;
            this.buttonGasWork.Text = "气缸动点";
            this.buttonGasWork.UseVisualStyleBackColor = true;
            this.buttonGasWork.Click += new System.EventHandler(this.buttonGasWork_Click);
            // 
            // buttonTestPowerResult
            // 
            this.buttonTestPowerResult.Location = new System.Drawing.Point(176, 33);
            this.buttonTestPowerResult.Name = "buttonTestPowerResult";
            this.buttonTestPowerResult.Size = new System.Drawing.Size(75, 23);
            this.buttonTestPowerResult.TabIndex = 23;
            this.buttonTestPowerResult.Text = "测试结果";
            this.buttonTestPowerResult.UseVisualStyleBackColor = true;
            this.buttonTestPowerResult.Click += new System.EventHandler(this.buttonTestPowerResult_Click);
            // 
            // buttonPower
            // 
            this.buttonPower.Location = new System.Drawing.Point(20, 33);
            this.buttonPower.Name = "buttonPower";
            this.buttonPower.Size = new System.Drawing.Size(75, 23);
            this.buttonPower.TabIndex = 23;
            this.buttonPower.Text = "测试功率";
            this.buttonPower.UseVisualStyleBackColor = true;
            this.buttonPower.Click += new System.EventHandler(this.buttonPower_Click);
            // 
            // buttonGasOrg
            // 
            this.buttonGasOrg.Location = new System.Drawing.Point(494, 33);
            this.buttonGasOrg.Name = "buttonGasOrg";
            this.buttonGasOrg.Size = new System.Drawing.Size(75, 23);
            this.buttonGasOrg.TabIndex = 23;
            this.buttonGasOrg.Text = "气缸原点";
            this.buttonGasOrg.UseVisualStyleBackColor = true;
            this.buttonGasOrg.Click += new System.EventHandler(this.buttonGasOrg_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.FaceMark);
            this.tabControl1.Controls.Add(this.sideMark);
            this.tabControl1.Controls.Add(this.tabPageTest);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 123);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(617, 427);
            this.tabControl1.TabIndex = 28;
            // 
            // FaceMark
            // 
            this.FaceMark.Controls.Add(this.panelMark1);
            this.FaceMark.Location = new System.Drawing.Point(4, 25);
            this.FaceMark.Name = "FaceMark";
            this.FaceMark.Padding = new System.Windows.Forms.Padding(3);
            this.FaceMark.Size = new System.Drawing.Size(609, 398);
            this.FaceMark.TabIndex = 0;
            this.FaceMark.Text = "正面打标";
            this.FaceMark.UseVisualStyleBackColor = true;
            // 
            // panelMark1
            // 
            this.panelMark1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMark1.Location = new System.Drawing.Point(3, 3);
            this.panelMark1.Name = "panelMark1";
            this.panelMark1.Size = new System.Drawing.Size(603, 392);
            this.panelMark1.TabIndex = 0;
            // 
            // sideMark
            // 
            this.sideMark.Controls.Add(this.panelMark2);
            this.sideMark.Location = new System.Drawing.Point(4, 25);
            this.sideMark.Name = "sideMark";
            this.sideMark.Padding = new System.Windows.Forms.Padding(3);
            this.sideMark.Size = new System.Drawing.Size(609, 438);
            this.sideMark.TabIndex = 1;
            this.sideMark.Text = "侧面打标";
            this.sideMark.UseVisualStyleBackColor = true;
            // 
            // panelMark2
            // 
            this.panelMark2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMark2.Location = new System.Drawing.Point(3, 3);
            this.panelMark2.Name = "panelMark2";
            this.panelMark2.Size = new System.Drawing.Size(603, 432);
            this.panelMark2.TabIndex = 0;
            // 
            // tabPageTest
            // 
            this.tabPageTest.Controls.Add(this.numericUpDownTimes);
            this.tabPageTest.Controls.Add(this.buttonTestPowerResult);
            this.tabPageTest.Controls.Add(this.label7);
            this.tabPageTest.Controls.Add(this.textBoxPowerMax);
            this.tabPageTest.Controls.Add(this.buttonPower);
            this.tabPageTest.Controls.Add(this.label4);
            this.tabPageTest.Controls.Add(this.label6);
            this.tabPageTest.Controls.Add(this.buttonGasWork);
            this.tabPageTest.Controls.Add(this.label5);
            this.tabPageTest.Controls.Add(this.textBoxPowerMin);
            this.tabPageTest.Controls.Add(this.buttonGasOrg);
            this.tabPageTest.Location = new System.Drawing.Point(4, 25);
            this.tabPageTest.Name = "tabPageTest";
            this.tabPageTest.Size = new System.Drawing.Size(609, 438);
            this.tabPageTest.TabIndex = 2;
            this.tabPageTest.Text = "测试";
            this.tabPageTest.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(628, 553);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 15);
            this.label3.TabIndex = 36;
            this.label3.Text = "OCR气缸位置：";
            // 
            // comboBoxOCRGas
            // 
            this.comboBoxOCRGas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOCRGas.Items.AddRange(new object[] {
            "不使用气缸",
            "气缸原点",
            "气缸动点"});
            this.comboBoxOCRGas.Location = new System.Drawing.Point(108, 82);
            this.comboBoxOCRGas.Name = "comboBoxOCRGas";
            this.comboBoxOCRGas.Size = new System.Drawing.Size(91, 23);
            this.comboBoxOCRGas.TabIndex = 37;
            this.comboBoxOCRGas.SelectedIndexChanged += new System.EventHandler(this.comboBoxOCRGas_SelectedIndexChanged);
            // 
            // MarkForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(628, 553);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MarkForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "首镭镭雕系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimes)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.FaceMark.ResumeLayout(false);
            this.sideMark.ResumeLayout(false);
            this.tabPageTest.ResumeLayout(false);
            this.tabPageTest.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxMarkTransp;
        private System.Windows.Forms.ComboBox comboBoxSize;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxProduct;
        private System.Windows.Forms.Button buttonModify;
        private System.Windows.Forms.Button buttonGasWork;
        private System.Windows.Forms.Button buttonPower;
        private System.Windows.Forms.Button buttonGasOrg;
        private System.Windows.Forms.NumericUpDown numericUpDownTimes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonTestPowerResult;
        private System.Windows.Forms.TextBox textBoxPowerMax;
        private System.Windows.Forms.TextBox textBoxPowerMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage FaceMark;
        private System.Windows.Forms.TabPage sideMark;
        private System.Windows.Forms.TabPage tabPageTest;
        private System.Windows.Forms.Panel panelMark1;
        private System.Windows.Forms.Panel panelMark2;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonWafer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxWaferFile;
        private System.Windows.Forms.ComboBox comboBoxMarkType;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_MarkCount;
        private System.Windows.Forms.ComboBox comboBoxOCRGas;
        private System.Windows.Forms.Label label3;
    }
}

