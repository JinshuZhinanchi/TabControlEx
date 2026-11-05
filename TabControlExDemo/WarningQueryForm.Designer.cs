
namespace TabControlExDemo
{
    partial class WarningQueryForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewWarns = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelPageInfo = new System.Windows.Forms.Label();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.buttonEmpty = new System.Windows.Forms.Button();
            this.columnHeaderCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pulseButtonPre = new TabControlExDemo.PulseButton();
            this.pulseButtonNext = new TabControlExDemo.PulseButton();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(796, 585);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "报警查询";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.listViewWarns, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.30435F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.69566F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(790, 565);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listViewWarns
            // 
            this.listViewWarns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderContent,
            this.columnHeaderCount});
            this.listViewWarns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewWarns.FullRowSelect = true;
            this.listViewWarns.GridLines = true;
            this.listViewWarns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewWarns.HideSelection = false;
            this.listViewWarns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listViewWarns.LabelWrap = false;
            this.listViewWarns.Location = new System.Drawing.Point(3, 123);
            this.listViewWarns.MultiSelect = false;
            this.listViewWarns.Name = "listViewWarns";
            this.listViewWarns.Size = new System.Drawing.Size(784, 439);
            this.listViewWarns.SmallImageList = this.imageList1;
            this.listViewWarns.TabIndex = 1;
            this.listViewWarns.UseCompatibleStateImageBehavior = false;
            this.listViewWarns.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "时间";
            this.columnHeaderTime.Width = 140;
            // 
            // columnHeaderContent
            // 
            this.columnHeaderContent.Text = "内容";
            this.columnHeaderContent.Width = 562;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.dateTimePickerStart, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.dateTimePickerEnd, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pulseButtonPre, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.pulseButtonNext, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelPageInfo, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonQuery, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonEmpty, 4, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 114);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "报警名称：";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.Location = new System.Drawing.Point(253, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "报警时间：";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateTimePickerStart.Location = new System.Drawing.Point(353, 18);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(144, 21);
            this.dateTimePickerStart.TabIndex = 2;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(503, 18);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(144, 21);
            this.dateTimePickerEnd.TabIndex = 2;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxName.Location = new System.Drawing.Point(103, 18);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(128, 21);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.Visible = false;
            // 
            // labelPageInfo
            // 
            this.labelPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPageInfo.Location = new System.Drawing.Point(353, 69);
            this.labelPageInfo.Name = "labelPageInfo";
            this.labelPageInfo.Size = new System.Drawing.Size(144, 32);
            this.labelPageInfo.TabIndex = 4;
            this.labelPageInfo.Text = "label3";
            this.labelPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonQuery.BackColor = System.Drawing.Color.Green;
            this.buttonQuery.Location = new System.Drawing.Point(653, 8);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(94, 41);
            this.buttonQuery.TabIndex = 1;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = false;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // buttonEmpty
            // 
            this.buttonEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEmpty.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonEmpty.Location = new System.Drawing.Point(503, 66);
            this.buttonEmpty.Name = "buttonEmpty";
            this.buttonEmpty.Size = new System.Drawing.Size(144, 39);
            this.buttonEmpty.TabIndex = 6;
            this.buttonEmpty.Text = "清空报警";
            this.buttonEmpty.UseVisualStyleBackColor = false;
            this.buttonEmpty.Click += new System.EventHandler(this.buttonEmpty_Click);
            // 
            // columnHeaderCount
            // 
            this.columnHeaderCount.Text = "次数";
            this.columnHeaderCount.Width = 122;
            // 
            // pulseButtonPre
            // 
            this.pulseButtonPre.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButtonPre.Location = new System.Drawing.Point(103, 60);
            this.pulseButtonPre.Name = "pulseButtonPre";
            this.pulseButtonPre.PulseSpeed = 0.3F;
            this.pulseButtonPre.Size = new System.Drawing.Size(94, 51);
            this.pulseButtonPre.TabIndex = 5;
            this.pulseButtonPre.Text = "上一页";
            this.pulseButtonPre.UseVisualStyleBackColor = true;
            this.pulseButtonPre.Click += new System.EventHandler(this.pulseButtonPre_Click);
            // 
            // pulseButtonNext
            // 
            this.pulseButtonNext.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButtonNext.Location = new System.Drawing.Point(253, 60);
            this.pulseButtonNext.Name = "pulseButtonNext";
            this.pulseButtonNext.PulseSpeed = 0.3F;
            this.pulseButtonNext.Size = new System.Drawing.Size(94, 51);
            this.pulseButtonNext.TabIndex = 5;
            this.pulseButtonNext.Text = "下一页";
            this.pulseButtonNext.UseVisualStyleBackColor = true;
            this.pulseButtonNext.Click += new System.EventHandler(this.pulseButtonNext_Click);
            // 
            // WarningQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 585);
            this.Controls.Add(this.groupBox1);
            this.Name = "WarningQueryForm";
            this.Text = "WarningQueryForm";
            this.SizeChanged += new System.EventHandler(this.WarningQueryForm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.ListView listViewWarns;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderContent;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBoxName;
        private PulseButton pulseButtonPre;
        private PulseButton pulseButtonNext;
        private System.Windows.Forms.Label labelPageInfo;
        private System.Windows.Forms.Button buttonEmpty;
        private System.Windows.Forms.ColumnHeader columnHeaderCount;
    }
}