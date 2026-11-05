using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyCommanLib;

namespace TabControlExDemo
{
    public partial class AutoForm : BaseForm
    {
        delegate void SetTextCallback(Control control, Color clr, string text);
        delegate void SetBackColorCallback(Control control, Color clr);        

        public CuttingForm mainForm;

        delegate void AddListViewCallback(string strTime, string strContent, Color textColor);

        public short m_nWorkState = 0;

        string m_strScanContent = "";
        
        public WarningTableOP m_productWarningDbOp = new WarningTableOP();
        public DeviceWarningsOP m_dviceWarningDbOP = new DeviceWarningsOP();

        string m_strDelay;

        public AutoForm()
        {
            InitializeComponent();

            buttonRunState.BackColor = PLCOpcFeedParameters.disableColor;
            buttonPauseState.BackColor = PLCOpcFeedParameters.disableColor;
            buttonWarnState.BackColor = PLCOpcFeedParameters.disableColor;

            //AddContent2ListView("2021051015", "测试报警1", Color.Red);
            //AddContent2ListView("2021051015", "测试提示1", Color.Black);

            SQLiteHelper.CreateDBFile("EQDataBase1.db");
            //WarningTableData wTableData = new WarningTableData();
            //wTableData.WarningID = 1;
            //wTableData.MachineName = "TH001";
            //wTableData.Name = "离线";
            //wTableData.Descript = "请检查网络连接";
            //m_productDbOp.Insert(wTableData);

            //AddWarning2Table("WARN001");

            m_nCurrentLevel = CommonDefine.USER_OP;

            //开始监听
            //HOOK.StartHook(this);
        }

        public void AddWarning2Table( string warningName )
        {
            DeviceWarnings deviceWarnings = new DeviceWarnings();
            deviceWarnings = m_dviceWarningDbOP.GetbyName(warningName);
            deviceWarnings.Count = deviceWarnings.Count + 1;
            m_dviceWarningDbOP.Update(deviceWarnings);

            WarningTableData wTableData = new WarningTableData();            
            wTableData.MachineName = "TH001";
            wTableData.Name = warningName;
            wTableData.Time = DateTime.Now;
            wTableData.Descript = deviceWarnings.Descript;

            m_productWarningDbOp.Insert(wTableData);
        }

        public void AddListViewThread(string strTime, string strContent, Color textColor)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.listViewWarns.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                AddListViewCallback d = new AddListViewCallback(AddListViewThread);
                this.Invoke(d, new object[] { strTime, strContent ,textColor});
            }
            else
            {
                AddContent2ListView(strTime, strContent,textColor);
            }
        }

        
        public void AddContent2ListView(string strTime, string strContent, Color textColor )
        {
            if (strTime == null)
            {
                strTime = String.Format("{0}：{1}", DateTime.Now.Day.ToString(), DateTime.Now.ToLongTimeString().ToString());
            }

            ListViewItem lvItem = new ListViewItem();

            lvItem.ForeColor = textColor;
            lvItem.Text = strTime;
            lvItem.StateImageIndex = 0;

            lvItem.SubItems.Add(strContent);
            
            this.listViewWarns.Items.Insert(0,lvItem);
        }

        private void AutoForm_SizeChanged(object sender, EventArgs e)
        {
            int width = tableLayoutPanel11.Width;
            listViewWarns.Columns[1].Width = width - listViewWarns.Columns[0].Width;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
        }

        private void SetThreadText(Control control, Color clr, string text)
        {
            if (control.IsDisposed)
            {
                return;
            }

            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            //if (ob is TextBox)
            {
                if (control.InvokeRequired)
                {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                    try
                    {
                        SetTextCallback d = new SetTextCallback(SetThreadText);
                        this.Invoke(d, new object[] { control, clr, text });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    if (clr.ToArgb() > 0)
                    {
                        control.ForeColor = clr;
                    }
                    control.Text = text;
                }
            }
        }

        private void SetThreadBackColor(Control control, Color clr)

        {
            if (control.IsDisposed)
            {
                return;
            }

            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            //if (ob is TextBox)
            {
                if (control.InvokeRequired)
                {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                    try
                    {
                        SetBackColorCallback d = new SetBackColorCallback(SetThreadBackColor);
                        this.Invoke(d, new object[] { control, clr });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    control.BackColor = clr;
                }
            }
        }


        

        public string GetLogTimeString()
        {
            string strTime = string.Format("{0}月{1}日{2}:{3}:{4}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            return strTime;
        }

        public void WriteScanIni( string barCode )
        {
            if(m_strScanContent.Length <= 0 )
            {
                return;
            }

            string fileName = System.Windows.Forms.Application.StartupPath + "\\ScanRecords\\" + m_strScanContent + ".ini";

            string strText = "1";
            string keyName = barCode;
            Program.WritePrivateProfileString("OrderIni", keyName , strText, fileName);

        }

        public string ReadScanIni(string barCode)
        {
            string strValue = "";

            string fileName = System.Windows.Forms.Application.StartupPath + "\\ScanRecords\\" + m_strScanContent + ".ini";
                        
            string keyName = barCode;
            
            StringBuilder temp = new StringBuilder(255);

            Program.GetPrivateProfileString("OrderIni", keyName , "", temp, 255, fileName);

            strValue = temp.ToString();

            return strValue;
        }

        public void WriteCuttingIni(string cutFileName)
        {
            /*
            string fileName = m_strCuttingPath + "\\" + cutFileName + ".txt";

            string strText = "1";
            string keyName = cutFileName;
            Program.WritePrivateProfileString("OrderIni", keyName, strText, fileName);
            */
            // Create an instance of StreamWriter to write text to a file.
            // The using statement also closes the StreamWriter.
            //string fileName = mainForm.setForm.GetCuttingPath() + "\\ProcessFile.txt";
            //using (StreamWriter sw = new StreamWriter(fileName))
            //{
            //    sw.WriteLine(cutFileName);
            //    sw.Close();
            //}

            //string fileName2 = mainForm.setForm.GetCuttingPath2() + "\\ProcessFile.txt";
            //using (StreamWriter sw2 = new StreamWriter(fileName2))
            //{
            //    sw2.WriteLine(cutFileName);
            //    sw2.Close();
            //}

        }

        private void textBoxScan_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.KeyCode == Keys.Enter )
            {
                m_strScanContent = textBoxScan.Text;

                //m_strScanContent = mainForm.readCuttingFileName("");

                textBoxContent.Text = m_strScanContent;

                WriteScanIni(m_strScanContent );

                //m_strScanContent = ReadScanIni(m_strScanContent);

                //获取切割文档名称

                //string xmlName = mainForm.setForm.GetXmlPath() + "\\" + m_strScanContent + ".xml";
                //if(System.IO.File.Exists(xmlName) )
                //{
                //    string cutFileName = mainForm.readCuttingFileName(xmlName);
                //    if (cutFileName.Length > 0)
                //    {
                //        WriteCuttingIni(cutFileName);
                //        textBoxCutting.Text = cutFileName;
                //    }
                //    else
                //    {
                //        MessageBox.Show("没有读取到切割文档名称，请检查！");
                //    }
                //}else
                //{
                //    MessageBox.Show("没有读取到切割文档xml文件，请检查！");
                //}
                

                textBoxScan.Text = "";
                string strTime = GetLogTimeString();
                string strContent = string.Format("工单号：{0}", m_strScanContent);
                AddListViewThread(strTime, strContent , Color.Blue);
            }
        }

        private void buttonPhoto_Click(object sender, EventArgs e)
        {
            mainForm.ccdCheckZF.WritePaiZhao1();
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            WarningQueryForm warningQuery = new WarningQueryForm();
            warningQuery.parentForm = this;
            warningQuery.ShowDialog();
        }

        private void textBoxLevel_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter )
            {
                textBoxLevel.Text = "";

                StartWork();
            }
        }

        public void SetCuttingPath(string strText)
        {
            textBoxCuttingPath.Text = strText;
        }

        public void SetCuttingPath2(string strText)
        {
            textBoxCuttingPath2.Text = strText;
        }

        public void SetXmlPath(string strText)
        {
            textBoxWorkOrder.Text = strText;
        }

        internal void setDelay(string strDelay)
        {
            m_strDelay = strDelay;
        }


        private void AutoForm_DragOver(object sender, DragEventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        private void AutoForm_MouseMove(object sender, MouseEventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        private void textBoxLevel_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        public void SetLevelInfo(int nLevel)
        {
            if( nLevel == 0 )
            {
                labelLevel.Text = "操作员";
            }else if( nLevel == 1 )
            {
                labelLevel.Text = "技术员";
            }
            else if (nLevel == 2)
            {
                labelLevel.Text = "工程师";
            }
            else if (nLevel == 3)
            {
                labelLevel.Text = "管理员";
            }
        }
    }
}
