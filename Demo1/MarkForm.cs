using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using LmcLib;
using System.IO.Ports;

using OpcDa.Client;

using System.IO;
using System.Diagnostics;
using System.Threading;

using System.Net.Sockets;
using System.Net;
using MyCommanLib;
using UserManager;

namespace EzdDemo
{
    public partial class MarkForm : Form
    {
        
        delegate void SetTextCallback(Control control, Color clr, string text);

        public frm_main frm_user;

        public int    m_bTestMode = 0;
        static bool m_bIsMarking = false;
        static bool m_bIsInitialed = false;

        public event EventHandler SetProducInfoHandler;
        public event EventHandler SetManualOperateHandler;
        public event EventHandler SetChipInfoHandler;

        public event EventHandler SetManualOpcOperateHandler;

        public delegate void SetLaserParamDelegate(int prf, int mcs);
        public SetLaserParamDelegate SetLayserParamHandler;

        public  int  m_nMarkType = 0;
        public  int  m_nWaferSize = 0;
        public int m_nProductIndex = 0;
        public int m_nEncryptionType = 0; //0:不加密， 1： semi加密
        public int m_nMarkPos = 0;//打标位置 PM7 -- PM12


        public int m_nTestPowerTimes = 1;//多少次测试一次功率
        public double m_fNormalPower = 5;
        public double m_fMinPower = 0;//下限百分比
        public double m_fMaxPower = 20;//上限百分比

        string m_strWMFPath = "";
        string m_strEzdFileName = "";
        string logString ="";
        List<string> logStringList = new List<string>();

        UInt32 m_nMarkSn = 1;

        public const int MaxCount = 1000000;

        public string m_MoudFilePath = "";
        public int m_nFinishDelay = 200;

        Thread threadWork;
        System.Timers.Timer workTimer;
        private object showRedObject = new object();
        Boolean m_bShowRedLight = false;

        public int m_nStartIndex = 1;
        public int m_nSNLength = 5;

        public string m_strPNPath = "D:\\xmlPath";

        public int m_nAddress0Card = 0;
        public int m_nAddress1Card = 1;

        public MarkHeadForm markHeadForm1 = new MarkHeadForm();
        public MarkHeadForm markHeadForm2 = new MarkHeadForm();

        private void SetControlTextThread(Control control, Color clr, string text)
        {
            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
            SetTextCallback d = new SetTextCallback(SetControlTextThread);
                this.Invoke(d, new object[] { control,  clr,  text });
            }
            else
            {
                control.Text = text;                
            }

        }

        public MarkForm()
        {
            InitializeComponent();
            // update interface state            
            for (int i = 0; i < CommonDefine.productHelper.m_ProductList.Count; i++ )
            {
                comboBoxProduct.Items.Add(CommonDefine.productHelper.m_ProductList[i]);
            }

            comboBoxProduct.SelectedIndex = 0;
            comboBoxMarkType.SelectedIndex = 1;       

            GetPrivateIni();

            //隐藏测试界面
            //tabPageTest.Parent = null;
            sideMark.Parent = null;
        }

        
        public void StartWork()
        {
            threadWork = new Thread(new ThreadStart(work));
            threadWork.Start();
        }

        public void EndWork()
        {
            if (threadWork != null)
            {
                workTimer.Enabled = false;
                threadWork.Abort();
            }
        }

        private void work()
        {
            workTimer = new System.Timers.Timer();
            workTimer.Interval = 200;
            workTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            workTimer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (showRedObject)
            {
                if(m_bShowRedLight)
                {
                    LmcMultiDll.RedMark(m_nAddress0Card);
                }
            }
        }

        public void ShowEntityCenter(int nCard )
        {
            double px = 0, py = 0, pa = 0;
            double dMinx = 0,dMiny = 0,dMaxx = 0,dMaxy=0,dz=0;
            string strEnt = "code";
            LmcMultiDll.GetEntSize(nCard, strEnt, ref dMinx, ref dMiny, ref dMaxx, ref dMaxy, ref dz);

            px = (dMinx + dMaxx) / 2.0;
            py = (dMiny + dMaxy) / 2.0;

            //textBoxPosx.Text = px.ToString();
            //textBoxPosy.Text = py.ToString();
            //textBoxPosz.Text = pa.ToString();
        }

        public bool loadEzdFile(string filename , int nCard)
        {
            bool bRet = true;
            LmcErrCode errorCode = LmcMultiDll.LoadEzdFile(nCard,filename);

            if (errorCode != LmcErrCode.LMC1_ERR_SUCCESS)
            {
                //MessageBox.Show("打开Ezd文件" + filename + "失败!");
                return false;
            }

            ShowEntityCenter(nCard);
                        
            UpdatePrevBmp(nCard);
       
            return bRet;
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadLMC();
        }

        public void ReloadLMC()
        {
            if (m_bIsInitialed)
                return;

            String dir = System.Windows.Forms.Application.StartupPath + "\\";// Environment.CurrentDirectory;
            LmcErrCode nErr = LmcErrCode.LMC1_ERR_SUCCESS;
#if _DEBUG
            nErr = LmcMultiDll.Initialize(dir, true, this.Handle);
#else
            if (m_bTestMode != 0)
            {
                nErr = LmcMultiDll.Initialize(dir, true, this.Handle);
                MessageBox.Show("运行在演示模式！");
            }
            else
            {
                nErr = LmcMultiDll.Initialize(dir, false,this.Handle);
            }
#endif
            if (nErr != LmcErrCode.LMC1_ERR_SUCCESS)
            {
                m_bIsInitialed = false;
                MessageBox.Show("初始化LmcDll失败\n错误原因:" + LmcMultiDll.GetErrorText(nErr));
               // return;
            }

            m_bIsInitialed = true;
            GetCardAddress();


            markHeadForm1.markForm = this;
            markHeadForm2.markForm = this;

            markHeadForm1.InitialHeadForm(0,m_nAddress0Card);
            markHeadForm2.InitialHeadForm(1, m_nAddress1Card);

            panelMark1.Controls.Clear();
            markHeadForm1.TopLevel = false;
            markHeadForm1.Dock = DockStyle.Fill;
            panelMark1.Controls.Add(markHeadForm1);
            markHeadForm1.Show();

            panelMark2.Controls.Clear();
            markHeadForm2.TopLevel = false;
            markHeadForm2.Dock = DockStyle.Fill;
            panelMark2.Controls.Add(markHeadForm2);
            markHeadForm2.Show();


        }


        public void GetCardAddress()
        {
            int[] sn = new int[8];
            for (int i = 0; i < 8; i++)
            {
                sn[i] = -1;
            }
            LmcMultiDll.GetAllCardSN(sn);

            for (int i = 0; i < 8; i++)
            {
                if (sn[i] == 0)
                {
                    m_nAddress0Card = i;
                }

                if (sn[i] == 1)
                {
                    m_nAddress1Card = i;
                }
            }
        }


        public void SaveMarkRecords(string strText)
        {
            DateTime tt = DateTime.Now;
            string strKey = string.Format("{0:D2}:{1:D2}:{2:D2}", tt.Hour, tt.Minute, tt.Second);
            string fileName = string.Format("{0}-{1:D2}-{2:D2}", tt.Year, tt.Month, tt.Day);

            string filePathName = System.Windows.Forms.Application.StartupPath + "\\MarkRecords\\" + fileName +".ini";    
          

            CommonDefine.WritePrivateProfileString("EzdIni", strKey, strText, filePathName);

        }

        private void SaveLogFile(string filename)
        {            
            StreamWriter sw = File.CreateText(filename);

            string strText = "";
            
            for(int i = 0; i < logStringList.Count; i++ )
            {
                strText = logStringList[i];
                if (strText != null)
                {
                    sw.WriteLine(strText);
                }
            }
            sw.Flush();
            sw.Close();
            
        }


        private void LoadLogFile(string filename)
        {           
            if (File.Exists(filename)== false)
            {                
                return;
            }            

            StreamReader reader = new StreamReader(filename);                   

	        //// 一行一行读取
            string strText = reader.ReadLine();
            while (strText  != null)
	        {             
                strText = reader.ReadLine();
                logStringList.Add(strText);
	        }
            reader.Close();

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //LmcDll.Close();
        }

       
        public Boolean TransMarkPostion(int nCard)
        {
            Boolean bRet = true;
            double dMoveX = MyCommanLib.CommonDefine.m_PosStruct.fx ;/// dMoveX x方向移动距离
            double dMoveY = MyCommanLib.CommonDefine.m_PosStruct.fy;/// dMoveY y方向移动距离
            double dCenterX = 0;/// dCenterX旋转中心的x坐标
            double dCenterY = 0;/// dCenterY旋转中心的y坐标                            
            double dHue = MyCommanLib.CommonDefine.m_PosStruct.fr ;  /// dRotateAng为旋转角度,单位为弧度值

            LmcMultiDll.SetRotateMoveParam(nCard, dMoveX, dMoveY, dCenterX, dCenterY, dHue);

            //MarkEntity("");
            return bRet;
        }

        public Boolean Mark(int nCard,string strText)
        {
            if (m_bIsMarking )
            {                
                return false;
            }

            if( strText.Length > 0)
            {
                ReplaceByName(nCard, strText);
            }

            //  TransMarkPostion(nCard);


            m_bIsMarking = true;

            int nCount = LmcMultiDll.GetEntityCount(nCard);
            string  strEnt;
            for(int i = 0; i < nCount; i++ )
            {
                strEnt = LmcMultiDll.GetEntityNameByIndex(nCard,i);
                if( strEnt == "Power")
                {
                    continue;
                }

                //循环打标
                for(int j = 0;j < CommonDefine.waferFileInfo.m_nMarkCount; j++)
                {
                    LmcMultiDll.MarkEntity(nCard, strEnt);

                    while (LmcMultiDll.IsMarkingFinish(nCard) == false)
                    {
                        Thread.Sleep(100);
                    }
                }
                
            }

            SaveMarkRecords(strText);

            m_bIsMarking = false;

            return true;
        }

        public Boolean MarkEntity(int nCard ,string strEnt )
        {
            Boolean bRet = false;          

            LmcErrCode err = LmcMultiDll.MarkEntity( nCard, strEnt);

            if (err == LmcErrCode.LMC1_ERR_SUCCESS)
            {
                bRet = true;
            }

            return bRet;
        }

       public bool IsMarkFinish(int nCard)
        {
            return LmcMultiDll.IsMarkingFinish(nCard);

        }

        void UpdatePrevBmp(int nCard )
        {
            ShowPreviewBmp(nCard);
        }

        public string GetStringFromEzd(int nCard)
        {
            string retString = "";
            int nCount = LmcMultiDll.GetEntityCount(nCard);
            string strName;
            for (int i = 0; i < nCount; i++)
            {
                strName = LmcMultiDll.GetEntityNameByIndex(nCard,i);
                retString = retString + LmcMultiDll.GetTextByName(nCard,strName);
            }
            return retString;
        }

        public void ShowPreviewBmp(int nCard)
        {
            if( nCard == markHeadForm1.m_nCardAddress )
            {
                markHeadForm1.ShowPreviewBmp( nCard);
            }else if(nCard == markHeadForm2.m_nCardAddress )
            {
                markHeadForm2.ShowPreviewBmp( nCard);
            }
        }

        public void GetRecipePenParam(int nAddress0Card, int nPenNo, ref int nMarkLoop, ref double dMarkSpeed, ref double dPowerRatio, ref double dCurrent, ref int nFreq, ref double dQPulseWidth, ref int nStartTC, ref int nLaserOffTC, ref int nEndTC, ref int nPolyTC, ref double dJumpSpeed, ref int nJumpPosTC, ref int nJumpDistTC, ref double dEndComp, ref double dAccDist, ref double dPointTime, ref bool bPulsePointMode, ref int nPulseNum, ref double dFlySpeed)
        {
            LmcMultiDll.GetPenParam(nAddress0Card, nPenNo, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth, ref nStartTC,
                ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp, ref dAccDist, ref dPointTime,
                ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);
        }

        public Boolean ReplaceByName( int nCard, string strText ) 
        {
            Boolean bRet = false;

            string strEnt = "code";           
            LmcErrCode err =  LmcMultiDll.ChangeTextByName(nCard, strEnt, strText);
            if( err == LmcErrCode.LMC1_ERR_SUCCESS )
            {
                bRet = true;
            }
            UpdatePrevBmp(nCard);

            return bRet;
        }

        public void WritePrivateIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezd.ini";
            string strText = "";

            strText = m_strWMFPath;
            CommonDefine.WritePrivateProfileString("EzdIni", "BoxWaferFile", strText, fileName);
            

            strText = m_nMarkType.ToString();           
            CommonDefine.WritePrivateProfileString("EzdIni", "MarkBack", strText, fileName);

           
            strText = this.textBoxPowerMax.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", "m_fMaxPower", strText, fileName);

            strText = this.numericUpDownTimes.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", "m_nTestPowerTimes", strText, fileName);
                        
            string keyName = "ProductIndex";
            strText = m_nProductIndex.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", keyName, strText, fileName);

            keyName = "nWaferSize";
            strText = m_nWaferSize.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", keyName, strText, fileName);

       
            keyName = "nMarkPos";
            strText = GetMarkPos().ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", keyName, strText, fileName);
            
        }

        public void GetPrivateIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezd.ini";

            StringBuilder temp = new StringBuilder(255);

            CommonDefine.GetPrivateProfileString("EzdIni", "BoxWaferFile", "0", temp, 255, fileName);
            m_strWMFPath = temp.ToString();
       
            CommonDefine.GetPrivateProfileString("EzdIni", "m_bTestMode", "0", temp, 255, fileName);
            m_bTestMode = int.Parse(temp.ToString());

            CommonDefine.GetPrivateProfileString("EzdIni", "MarkBack", "1", temp, 255, fileName);
            try
            {
                m_nMarkType = int.Parse(temp.ToString());
            }
            catch (Exception)
            {
            }
            comboBoxMarkType.SelectedIndex = m_nMarkType;
            CommonDefine.waferFileInfo.nMarkType = m_nMarkType;


            CommonDefine.GetPrivateProfileString("EzdIni", "m_fMinPower", "0", temp, 255, fileName);
            this.textBoxPowerMin.Text = temp.ToString();

            CommonDefine.GetPrivateProfileString("EzdIni", "m_fMaxPower", "20", temp, 255, fileName);
            this.textBoxPowerMax.Text = temp.ToString();

            CommonDefine.GetPrivateProfileString("EzdIni", "m_nTestPowerTimes", "20", temp, 255, fileName);
            this.numericUpDownTimes.Text = temp.ToString();

            try
            {
                m_nTestPowerTimes = int.Parse(this.numericUpDownTimes.Text);
            }
            catch (Exception)
            {

            }

            try
            {
                m_fMinPower = double.Parse(this.textBoxPowerMin.Text);
                m_fMaxPower = double.Parse(this.textBoxPowerMax.Text);
            }
            catch (Exception)
            {

            }

            CommonDefine.GetPrivateProfileString("EzdIni", "ProductIndex", "0", temp, 255, fileName);
            string strText = temp.ToString();

            try
            {
                if (strText.Length > 0)
                {
                    m_nProductIndex = int.Parse(strText);
                }
            }
            catch (Exception)
            {

            }
            comboBoxProduct.SelectedIndex = m_nProductIndex;         

            CommonDefine.GetPrivateProfileString("EzdIni", "PNFilePath", "D:\\xmlPath", temp, 255, fileName);
            m_strPNPath = temp.ToString();


            CommonDefine.GetPrivateProfileString("EzdIni", "CCD_MARK_POSX", "0", temp, 255, fileName);
            strText = temp.ToString();

            try
            {
                if (strText.Length > 0)
                {
                    CommonDefine.m_CCD2MarkPos.fx = double.Parse(strText);
                }
            }
            catch (Exception)
            {

            }

            CommonDefine.GetPrivateProfileString("EzdIni", "CCD_MARK_POSY", "0", temp, 255, fileName);
            strText = temp.ToString();

            try
            {
                if (strText.Length > 0)
                {
                    CommonDefine.m_CCD2MarkPos.fy = double.Parse(strText);
                }
            }
            catch (Exception)
            {

            }

            
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (m_bIsMarking )
            {
                if (MessageBox.Show("确实要退出系统吗?", "系统", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {         
                    WritePrivateIni();
                    
                    //LmcDll.Close();
                    System.Environment.Exit(0);
                }
            }
            else
            {
               
                WritePrivateIni();
                //LmcDll.Close();
                
            }
        }

        public void CloseMark()
        {
            LmcMultiDll.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }              

        
        public int GetSnLength(int nLaserType )
        {
            int nLen = markHeadForm1.GetSnLength();
            if (nLaserType == markHeadForm1.m_nLaserType)
            {
                nLen = markHeadForm1.GetSnLength();
            }
            else if (nLaserType == markHeadForm2.m_nLaserType)
            {
                nLen = markHeadForm2.GetSnLength();
            }

            return nLen;
        }

        public int GetVerifyCodeType(int nLaserType)
        {
            int nType = markHeadForm1.GetVerifyCodeType();
            if (nLaserType == markHeadForm1.m_nLaserType)
            {
                nType = markHeadForm1.GetVerifyCodeType();
            }
            else if (nLaserType == markHeadForm2.m_nLaserType)
            {
                nType = markHeadForm2.GetVerifyCodeType();
            }

            return nType;
            
        }

        public int GetMarkPos()
        {
            int nType = 0;           

            return nType;
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            LmcMultiDll.Close();

            string exeFileName = Environment.CurrentDirectory + "\\EzCad2.exe";
            if (File.Exists(exeFileName))
            {
                Process process = new Process();
                process.StartInfo.FileName = exeFileName;
                process.StartInfo.Arguments = "";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.WaitForExit();
                process.Close();

                //Process p = Process.Start(exeFileName);
                // p.WaitForExit();//关键，等待外部程序退出后才能往下执行
            }

            m_bIsInitialed = false;
            ReloadLMC();
        }       
    
        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nProductIndex = comboBoxProduct.SelectedIndex;
            CommonDefine.waferFileInfo.nProductIndex = m_nProductIndex;

            //切换bank
            int nBankIndex = m_nProductIndex;
            SwitchRobotBank(nBankIndex);
        }

        private void comboBoxMarkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nMarkType = comboBoxMarkType.SelectedIndex;
            CommonDefine.waferFileInfo.nMarkType = m_nMarkType;
            //读取数据 覆盖PLC

        }

        public void SwitchRobotBank(int nBankIndex)
        {
            int nBank = nBankIndex;
            if (nBankIndex < 0)
                return;
            if (SetProducInfoHandler == null)
                return;

            SetProducInfoHandler(nBankIndex.ToString(), null);

        }

        public void OperateButtonState(Boolean bEnable )
        {
            
            comboBoxMarkType.Enabled = bEnable;            
            comboBoxProduct.Enabled = bEnable;

            markHeadForm1.OperateButtonState(bEnable);
            markHeadForm2.OperateButtonState(bEnable);

            if (SetManualOperateHandler != null)
            {
                SetManualOperateHandler(bEnable, null);
            }                      
            
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            //LoginForm loginForm = new LoginForm(CommonDefine.USER_TP);
            //loginForm.ShowDialog();
            //if (loginForm.DialogResult == DialogResult.OK)
            //{
            //    OperateButtonState(true);
            //}

            frm_register.mainUser = frm_user;

            frm_register frm_Register1 = new frm_register();
            if (frm_Register1.ShowDialog() == DialogResult.OK)
            {
                OperateButtonState(true);
            }
        }

        private void buttonGasOrg_Click(object sender, EventArgs e)
        {
            if (SetManualOpcOperateHandler != null)
            {
                string str = "GasOrig";
                SetManualOpcOperateHandler(str, null);
            }
        }

        private void buttonGasWork_Click(object sender, EventArgs e)
        {
            if (SetManualOpcOperateHandler != null)
            {
                string str = "GasWork";
                SetManualOpcOperateHandler(str, null);
            }
        }

        private void buttonPower_Click(object sender, EventArgs e)
        {
            if (SetManualOpcOperateHandler != null)
            {
                string str = "TestPowerMark";
                SetManualOpcOperateHandler(str, null);
            }
        }

        private void buttonTestPowerResult_Click(object sender, EventArgs e)
        {
            if (SetManualOpcOperateHandler != null)
            {
                string str = "TestPowerResult";
                SetManualOpcOperateHandler(str, null);
            }
        }

        private void numericUpDownTimes_ValueChanged(object sender, EventArgs e)
        {
            m_nTestPowerTimes = (int)numericUpDownTimes.Value;
        }

        private void textBoxPowerMin_TextChanged(object sender, EventArgs e)
        {
            string strText = textBoxPowerMin.Text;
            try
            {
                m_fMinPower = double.Parse(strText);
            }
            catch (Exception)
            {

            }
        }

        private void textBoxPowerMax_TextChanged(object sender, EventArgs e)
        {
            string strText = textBoxPowerMax.Text;
            try
            {
                m_fMaxPower = double.Parse(strText);
            }
            catch (Exception)
            {

            }
        }
          
        public void waferInfo2UI()
        {            
            //waferinfo            
            comboBoxProduct.SelectedIndex = CommonDefine.waferFileInfo.nProductIndex;
            comboBoxMarkType.SelectedIndex = CommonDefine.waferFileInfo.nMarkType;
            comboBoxOCRGas.SelectedIndex = CommonDefine.waferFileInfo.nOCRGasPos;

            tb_MarkCount.Text = CommonDefine.waferFileInfo.m_nMarkCount.ToString();

            markHeadForm1.waferInfo2UI(0);
            markHeadForm1.comboBoxPenNO_SelectedIndexChanged(null, null);
            markHeadForm2.waferInfo2UI(1);
        }

        public void UI2WaferInfo()
        {
            //waferinfo           
            CommonDefine.waferFileInfo.nProductIndex = comboBoxProduct.SelectedIndex;
            CommonDefine.waferFileInfo.nMarkType = comboBoxMarkType.SelectedIndex;
            CommonDefine.waferFileInfo.nOCRGasPos = comboBoxOCRGas.SelectedIndex;

            CommonDefine.waferFileInfo.m_nMarkCount = int.Parse(tb_MarkCount.Text);
            string strFileName = textBoxWaferFile.Text;
            try
            {
                CommonDefine.waferFileInfo.strWaferCCDModuleName = System.IO.Path.GetDirectoryName(strFileName);
            }
            catch (Exception)
            {
            }
            

            markHeadForm1.UI2WaferInfo(0);
            markHeadForm2.UI2WaferInfo(1);

            strFileName = Path.GetFileNameWithoutExtension(strFileName);
            ManualSwitchMaterialNub(1, /*CommonDefine.waferFileInfo.strWaferCCDModuleName*/strFileName);
        }


        public void waferInfo2OPC()
        {            
            if (SetManualOpcOperateHandler != null)
            {
                string str = "WriteOPCData";
                SetManualOpcOperateHandler(str, null);
            }
        }

        public void OPC2WaferInfo()
        {
            if (SetManualOpcOperateHandler != null)
            {
                string str = "ReadOPCData";
                SetManualOpcOperateHandler(str, null);
            }
        }

        private void buttonWafer_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            
            if (Directory.Exists(m_strWMFPath))
            {
                dlg.InitialDirectory = m_strWMFPath;   // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                dlg.InitialDirectory = Environment.CurrentDirectory;
            }

            dlg.Filter = "Ezd files (*.wmf)|*.wmf";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_strWMFPath = System.IO.Path.GetDirectoryName( dlg.FileName );

                openWMFile(dlg.FileName);
            }
        }

        public void ManualSwitchMaterialNub(int nIndex, string pnName)
        {
            string iniPnFileName = m_strPNPath + "\\PN.ini";

            string strMater = "";
            string strPN = nIndex.ToString();
            string strPnName = pnName;
            CommonDefine.WriteMaterialNumb(iniPnFileName, strMater, strPN, strPnName);

        }

        public Boolean openWMFile(string strFileName)
        {

            Boolean bRet = true;
            SetControlTextThread(textBoxWaferFile, Color.Black, strFileName);


            if (CommonDefine.waferFileInfo.ReadFile(strFileName))
            {
                //reflash ui
                string pnFileName = Path.GetFileNameWithoutExtension(strFileName);
                ManualSwitchMaterialNub(1, /*CommonDefine.waferFileInfo.strWaferCCDModuleName*/pnFileName);

                loadEzdFile(CommonDefine.waferFileInfo.AllLaserMarkContent[0].strEzdName, m_nAddress0Card);
                loadEzdFile(CommonDefine.waferFileInfo.AllLaserMarkContent[1].strEzdName, m_nAddress1Card);

                SetLayserParamHandler(CommonDefine.waferFileInfo.AllLaserMarkContent[0].nPRF,
                    CommonDefine.waferFileInfo.AllLaserMarkContent[0].nMCS);

                waferInfo2UI();



               // waferInfo2OPC();
            }
            else
            {
                return false;
            }
            return bRet;
        }
                

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog oDlg = new SaveFileDialog();
                      
            if (Directory.Exists(m_strWMFPath))
            {
                oDlg.InitialDirectory = m_strWMFPath; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            }
            else
            {
                oDlg.InitialDirectory = Environment.CurrentDirectory;
            }

            oDlg.Filter = "Ezd files (*.wmf)|*.wmf";
            if (oDlg.ShowDialog() == DialogResult.OK)
            {
                string strFileName = oDlg.FileName;
                textBoxWaferFile.Text = strFileName;

                m_strWMFPath = System.IO.Path.GetDirectoryName(strFileName);

                SaveWMFile(strFileName);                
            }

            //this.Close();
        }

        public Boolean SaveWMFile(string strFileName)
        {
            Boolean bRet = true;

            UI2WaferInfo();

            if (CommonDefine.waferFileInfo.WriteFile( strFileName ))
            {
                
            }
            else
            {
                bRet = false;
            }            
         
            return bRet;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            UI2WaferInfo();          
        }

        private void comboBoxOCRGas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonDefine.waferFileInfo.nOCRGasPos = comboBoxOCRGas.SelectedIndex;

            string str = "OCRGasOrig";
            if (CommonDefine.waferFileInfo.nOCRGasPos == 1)
            {
                SetManualOpcOperateHandler(str, null);
            }
            else if (CommonDefine.waferFileInfo.nOCRGasPos == 2)
            {
                str = "OCRGasWork";
                SetManualOpcOperateHandler(str, null);
            }
        }
    }
}