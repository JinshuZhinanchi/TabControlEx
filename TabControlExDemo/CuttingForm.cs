using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.Threading;
using System.Xml;
using EquipViewAppNet;
using MyCommanLib;
using jog;
using EzdDemo;
using DemoCom;
using System.Threading.Tasks;
using StyleWindow;
using UserManager;

namespace TabControlExDemo
{
    public partial class CuttingForm : BaseForm
    {

        Task threadWork;
        System.Timers.Timer workTimer;
        private static object autoWorkObject = new object();

        Task threadWorkMark;
        System.Timers.Timer workMarkTimer;
        private static object autoWorkMarkObject = new object();

        Task threadWorkSmif;
        System.Timers.Timer workSmifTimer;
        private static object autoWorkSmifObject = new object();

        Task threadState;
        System.Timers.Timer stateTimer;
        private static object stateWorkObject = new object();

        Task threadReady;
        System.Timers.Timer readyTimer;
        private static object readyWorkObject = new object();

        Task threadUI;
        System.Timers.Timer UITimer;
        private static object UIWorkObject = new object();


        delegate void SetTextCallback(Control control, Color clr, string text);
        delegate void SetBackColorCallback(Control control, Color clr);

        //public  OpcAxisOperateHelper mOpcAxisOperater = new OpcAxisOperateHelper();

        //public IOFeedForm iOForm;
        public IOFeedForm iOForm;

        public UserControl1 userForm;
        public frm_main frm_user;

        public ChipAutoForm chipAutoForm;
        public GtsForm gtsForm;

        public SerialPortForm robotSerialPortForm;
        public SerialPortForm laserPowerForm;
        public SerialPortForm waferSerialPortForm;
        public MainWindow m_LaserMainWindow;

        public TCPForm smifForm;
        public TCPForm smifForm2;
        public TCPForm cameraForm;
        public TCPForm ocrSwitchRecipeForm;

        public WaferManualForm manualForm;

        public SecsAppView secsForm;

        public MarkForm markForm = new MarkForm();

        //public ParametersFeedForm parametersForm = new ParametersFeedForm();

        //public OpcOperateForm opcOperate;

        public GtsParametersFeedForm parametersForm = new GtsParametersFeedForm();

        private Size beforeResizeSize = Size.Empty;


        //CCDStation
        public CCDCheckState ccdCheckZF = new CCDCheckState();

        private static object lockTakePhotoObject = new object();
        private static object lockScanCodeObject = new object();
        private static object lockReadOpcState = new object();

        public string m_strCurrentOP = "123456";

        public string m_strCurrentPageName = "autoForm";
        public Form m_preCurrentForm = null;
        public int m_nEapBoxIndex = -1;

        public CuttingForm()
        {
            InitializeComponent();

            m_nCurrentLevel = CommonDefine.USER_OP;


        }

        public int GetCurrentOp()
        {
            int nOp = MyCommanLib.CommonDefine.USER_OP;
            if (userForm != null)
            {
                nOp = userForm.GetCurrentOp(m_strCurrentOP);
            }
            return nOp;
        }

        private void InitialOpc()
        {
            int nWarningCount = 0;

            //opc connect            
            //使用opcClient 同步opc时 ，从1开始 ；使用opcda.client 时 从 0开始
            //int nStart = 0;
            //PLCOpcFeedParameters.initialValue(nStart);
            //opcOperate = new OpcOperateForm();

            //opcOperate.addAllCuttingItems(PLCOpcFeedParameters.nPLCOpcParametersCount - nStart, nWarningCount);

            //mOpcAxisOperater.opcOperateParent = opcOperate;

            //if (opcOperate.connectOpcServer())
            //{
            //    opcOperate.InitialPreValue();
            //}
        }

        private void Form_Load(object sender, EventArgs e, TabControl tabControl)
        {
            #region 初始化 Tabpage

            Form formPage;

            //InitialOpc();

            m_LaserMainWindow = new MainWindow();

            chipAutoForm = new ChipAutoForm();
            formPage = chipAutoForm;
            AddTabPage("chipAutoForm", "自动模式", formPage, tabControl);
            chipAutoForm.mainForm = this;
            chipAutoForm.InitialOpcForm();

            manualForm = new WaferManualForm();
            formPage = manualForm;
            AddTabPage("manualForm", "手动调试", formPage, tabControl);
            manualForm.mainForm = this;


            gtsForm = new GtsForm();
            formPage = gtsForm;
            AddTabPage("gtsForm", "伺服调试", formPage, tabControl);
            gtsForm.markForm = markForm;

            int nType = 1;
            robotSerialPortForm = new SerialPortForm(nType);
            //serialPortForm.opcOperateForm = opcOperate;
            formPage = robotSerialPortForm;
            AddTabPage("robotSerialPortForm", "机械手串口设置", formPage, tabControl);

            waferSerialPortForm = new SerialPortForm();
            formPage = waferSerialPortForm;
            AddTabPage("waferSerialPortForm", "寻边器串口设置", formPage, tabControl);


            smifForm = new TCPForm();
            smifForm.m_strEndFlag = "\r";
            smifForm.m_nConnectType = 1;
            smifForm.ReadIniInfo();
            formPage = smifForm;
            AddTabPage("smifForm", "smif调试", formPage, tabControl);

            smifForm2 = new TCPForm();
            smifForm2.m_strEndFlag = "\r";
            smifForm2.m_nConnectType = 2;
            smifForm2.ReadIniInfo();
            formPage = smifForm2;
            AddTabPage("smifForm2", "smif2调试", formPage, tabControl);

            cameraForm = new TCPForm();
            cameraForm.m_strEndFlag = "\r\n";
            cameraForm.m_nConnectType = 13;
            cameraForm.ReadIniInfo();
            formPage = cameraForm;
            AddTabPage("cameraForm", "ocr调试", formPage, tabControl);

            ocrSwitchRecipeForm = new TCPForm();
            ocrSwitchRecipeForm.m_strEndFlag = "\r\n";
            ocrSwitchRecipeForm.m_nConnectType = 14;
            ocrSwitchRecipeForm.ReadIniInfo();
            formPage = ocrSwitchRecipeForm;
            AddTabPage("ocrSwitchRecipeForm", "ocr配方调试", formPage, tabControl);

            secsForm = new SecsAppView();
            formPage = secsForm;
            AddTabPage("secsForm", "EAP调试", formPage, tabControl);

            nType = 2;
            laserPowerForm = new SerialPortForm(nType);
            laserPowerForm.m_strEndFlag = "\r\n";
            //serialPortForm.opcOperateForm = opcOperate;
            formPage = laserPowerForm;
            AddTabPage("laserPowerForm", "功率计串口设置", formPage, tabControl);

            iOForm = new IOFeedForm();
            //iOForm.opcOperateForm = opcOperate;
            formPage = iOForm;
            AddTabPage("iOForm", "IO监控", formPage, tabControl);

            //userForm = new UserControl1();            
            //formPage = userForm;
            //AddTabPage("userForm", "用户管理", formPage, tabControl);

            frm_user = new frm_main();
            formPage = frm_user;
            AddTabPage("frm_user", "用户管理", formPage, tabControl);



            markForm.frm_user = frm_user;
            markForm.SetManualOpcOperateHandler += mark_SetManualOpcOperateHandler;
            markForm.SetLayserParamHandler += markForm_SetLayserParamHandler;
            secsForm.SwitchReceipHandler += SecsForm_SwitchReceipHandler;
            secsForm.SetMarkContentHandler += SecsForm_SetMarkContentHandler;
            secsForm.RunRemoteCommandHandler += SecsForm_RunRemoteCommandHandler;
            secsForm.ReadRFIDContentHandler += SecsForm_ReadRFIDContentHandler;
            secsForm.LoadFoupHandler += SecsForm_LoadFoupHandler;
            secsForm.WarningHandler += SecsForm_WarningHandler;
            secsForm.ReadRecipParamHandler += SecsForm_ReadRecipParamHandler;

            #endregion

            //连接secs            
            Boolean bUseEap = true;
            if (bUseEap)
            {
                secsForm.InitSecs();
            }

            markForm.ReloadLMC();

            AutoScale(this);
            int nWidth = ((this.Width - 30) / tabControlEx.TabPages.Count);
            tabControlEx.ItemSize = new Size(nWidth, 60);

            Boolean bConnected;
            if (smifForm.m_bUseCCDCheck)
            {
                string strInfo = "Smif1连接成功";
                Color clrInfo = Color.Blue;

                bConnected = false;
                for (int i = 0; i < 5; i++)
                {
                    bConnected = smifForm.connectServer(i);
                    if (bConnected && smifForm.tcpListenClient != null)
                    {

                        break;
                    }
                    Thread.Sleep(500);
                }

                if (bConnected && smifForm.tcpListenClient != null)
                {
                    clrInfo = Color.Blue;
                    strInfo = "Smif1连接成功";
                }
                else
                {
                    clrInfo = Color.Red;
                    strInfo = "Smif1连接失败";
                }

                chipAutoForm.AddListViewThread(null, strInfo, clrInfo);
            }

            if (smifForm2.m_bUseCCDCheck)
            {
                string strInfo = "Smif2连接成功";
                Color clrInfo = Color.Blue;

                bConnected = false;
                for (int i = 0; i < 5; i++)
                {
                    bConnected = smifForm2.connectServer(i);
                    if (bConnected && smifForm2.tcpListenClient != null)
                    {

                        break;
                    }
                    Thread.Sleep(500);
                }

                if (bConnected && smifForm2.tcpListenClient != null)
                {
                    clrInfo = Color.Blue;
                    strInfo = "Smif2连接成功";
                }
                else
                {
                    clrInfo = Color.Red;
                    strInfo = "Smif2连接失败";
                }
                chipAutoForm.AddListViewThread(null, strInfo, clrInfo);
            }

            if (cameraForm.m_bUseCCDCheck)
            {
                string strInfo = "OCR连接成功";
                Color clrInfo = Color.Blue;

                bConnected = false;
                for (int i = 0; i < 5; i++)
                {
                    bConnected = cameraForm.connectServer(i);
                    if (bConnected && cameraForm.tcpListenClient != null)
                    {

                        break;
                    }
                    Thread.Sleep(500);
                }

                if (bConnected && cameraForm.tcpListenClient != null)
                {
                    clrInfo = Color.Blue;
                    strInfo = "OCR连接成功";

                    //string cmd = "OF,01\r";
                    //cameraForm.clientWrite(cmd);
                }
                else
                {
                    clrInfo = Color.Red;
                    strInfo = "OCR连接失败";
                }
                chipAutoForm.AddListViewThread(null, strInfo, clrInfo);
            }


            if (ocrSwitchRecipeForm.m_bUseCCDCheck)
            {
                string strInfo = "OCR配方连接成功";
                Color clrInfo = Color.Blue;

                bConnected = false;
                for (int i = 0; i < 5; i++)
                {
                    bConnected = ocrSwitchRecipeForm.connectServer(i);
                    if (bConnected && ocrSwitchRecipeForm.tcpListenClient != null)
                    {

                        break;
                    }
                    Thread.Sleep(500);
                }

                if (bConnected && ocrSwitchRecipeForm.tcpListenClient != null)
                {
                    clrInfo = Color.Blue;
                    strInfo = "OCR配方连接成功";
                }
                else
                {
                    clrInfo = Color.Red;
                    strInfo = "OCR配方接失败";
                }
                chipAutoForm.AddListViewThread(null, strInfo, clrInfo);
            }


            InitialOpcForm();


            ReadCuttingIni();

            chipAutoForm.ReadWorkCount();

            //mOpcAxisOperater.OperateWaferLight(false);
            //Thread.Sleep(1000);

            if (MessageBox.Show("设备准备回原点，确认安全后，按Yes 取消按No", "系统", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                InfoForm infoForm = new InfoForm();
                infoForm.parentForm = this;
                infoForm.m_nFindType = 0;

                infoForm.SetInfoText("初始化中......");
                infoForm.ShowDialog();


                //robotSerialPortForm.serialPort.Write(JelRobotOperate.GetMoveToOrigString());

                //Thread.Sleep(2000);
                //if (mOpcAxisOperater.IntialAllAxis() == false)
                //{
                //    MessageBox.Show("轴初始化失败！");
                //}
            }

            StartWork();


        }

        private void markForm_SetLayserParamHandler(int prf, int mcs)
        {
            return;     // 更换激光器后不支持外控


            StyleWindow.MainWindow.timer.Stop();

            SendAndHandleResponse("MCS=", mcs.ToString(), chipAutoForm, "MCS");

            prf = prf * 100;
            SendAndHandleResponse("PRF=", prf.ToString(), chipAutoForm, "PRF");

            StyleWindow.MainWindow.timer.Start();
        }

        /// <summary>
        /// 处理和激光器重复发送和错误处理逻辑
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <param name="chipAutoForm"></param>
        /// <param name="variableName"></param>
        void SendAndHandleResponse(string command, string parameter, ChipAutoForm chipAutoForm, string variableName)
        {
            int repeatCount = 0;
            string m_strSend = command + parameter + "\r\n";
            string m_strErr;
            bool m_bSend;

            do
            {
                m_strErr = m_LaserMainWindow.ProcCMD(m_strSend, out m_bSend);
                if (m_strErr.Contains("ERR"))
                {
                    Thread.Sleep(200);
                    repeatCount++;
                    if (repeatCount > 5)
                    {
                        break;
                    }
                }
            } while (m_strErr.Contains("ERR"));

            string m_strInfo;
            if (!m_bSend)
            {
                m_strInfo = $"{variableName}={parameter} Fail: {m_strErr}";
                chipAutoForm.AddListViewThread(null, m_strInfo, Color.Red);
            }
            else
            {
                m_strInfo = $"{variableName}={parameter} SUCESS: {m_strErr}";
                chipAutoForm.AddListViewThread(null, m_strInfo, Color.Blue);
            }

            if (m_strErr.Contains("ERR"))
            {
                m_strInfo = $"{variableName}={parameter} Fail: {m_strErr}";
                chipAutoForm.AddListViewThread(null, m_strInfo, Color.Red);
            }
        }


        public void ThreadInitialDevice()
        {
            gtsForm.m_gtsControl.ResetWork();

            ProgressInfoForm m_InfoForm = new ProgressInfoForm();
            m_InfoForm.mainForm = this;
            m_InfoForm.SetInfoText("初始化中......");
            m_InfoForm.ShowDialog();

            m_InfoForm = null;
        }

        public Boolean InitialDevice()
        {
            Boolean bRet = false;
            Boolean bRet1 = false;
            Boolean bRet2 = false;

            //int nItem = PLCOpcFeedParameters.IOOutState.checkSafeDoor;
            //object ob = opcOperate.GetItemValue(nItem);
            Boolean bSelect = true;
            //if (ob != null)
            //{
            //    bSelect = !((Boolean)ob);
            //}

            if (bSelect)
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_IOInState.SafeDoor;
                //int nItem2 = PLCOpcFeedParameters.ExtIOInState.X63;
                //int nItem3 = PLCOpcFeedParameters.ExtIOInState.X64;

                Boolean bOpen1 = true;
                Boolean bOpen2 = true;
                Boolean bOpen3 = true;
                //object ob = opcOperate.GetItemValue(nItem1);
                object ob = gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.SafeDoorIOIN);
                if (ob != null)
                {
                    bOpen1 = (Boolean)ob;
                }

                //ob = opcOperate.GetItemValue(nItem2);
                //if (ob != null)
                //{
                //    bOpen2 = (Boolean)ob;
                //}

                //ob = opcOperate.GetItemValue(nItem3);
                //if (ob != null)
                //{
                //    bOpen3 = (Boolean)ob;
                //}

                //if (bOpen1 == false || bOpen2 == false  || bOpen3 == false ) 
                if (bOpen1 == false)
                {
                    string strInfo = $"安全门已启用，有安全门未关闭：{bOpen1}-{bOpen2}-{bOpen3},不可初始化！";
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                    return false;
                }

                if (chipAutoForm.IsLAarmXifu() || chipAutoForm.IsRAarmXifu())
                {
                    string strInfo = $"机械臂吸附有料,不可初始化！";
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                    return false;
                }

                if (chipAutoForm.IsXunbianExitWafer())
                {
                    string strInfo = $"寻边器有料,不可初始化！";
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                    return false;
                }

                if (chipAutoForm.IsSmifOpened(0) || chipAutoForm.IsSmifOpened(1))
                {
                    string strInfo = $"SMIF门未关闭,不可初始化！";
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                    return false;
                }
            }


            try
            {
                if (chipAutoForm.InitialRobot() == false)
                {
                    chipAutoForm.AddListViewThread(null, "Robot初始化失败，请检查！", Color.Red);
                }

                //设置Bank始终为1，防止断电后切换为默认Bank
                chipAutoForm.mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.SET_BANK);
            }
            catch (Exception)
            {
            }

            try
            {
                bRet1 = OperateSmifHelper.Initial(smifForm);
                gtsForm.m_gtsControl.ResetWork();
                if (bRet1 == false)
                {
                    chipAutoForm.AddListViewThread(null, "smif1初始化失败，请检查！", Color.Red);
                }
                else
                {
                    bRet = true;
                }

                bRet2 = OperateSmifHelper.Initial(smifForm2);
                if (bRet2 == false)
                {
                    chipAutoForm.AddListViewThread(null, "smif2初始化失败，请检查！", Color.Red);
                }
                else
                {
                    bRet = true;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                gtsForm.m_gtsControl.ResetWork();

                //bRet = mOpcAxisOperater.IntialAllAxis();
                //if(bRet == false)
                //{
                //    chipAutoForm.AddListViewThread(null, "打标Z轴回原点失败，请检查！", Color.Red);
                //}
            }
            catch (Exception)
            {
            }

            try
            {
                bRet = OperateAlignerHelper.OperateFindOrig(waferSerialPortForm);
                if (bRet == false)
                {
                    chipAutoForm.AddListViewThread(null, "寻边器回原点失败，请检查！", Color.Red);
                }
            }
            catch (Exception)
            {
            }

            chipAutoForm.initialData();

            return bRet;

        }

        public void InitialOpcForm()
        {
            //iOForm.InitialOpcButton(opcOperate);
            //iOForm.opcOperateForm = opcOperate;
            //parametersForm.InitialOpcButton(opcOperate);
        }

        private void mark_SetManualOpcOperateHandler(object sender, EventArgs e)
        {
            string strFunction = (string)sender;
            if (strFunction == "GasOrig")
            {
                chipAutoForm.WriteItemValue(GLinkIOModule00.PowerGasIOOUT, false);

            }
            else if (strFunction == "GasWork")
            {
                chipAutoForm.WriteItemValue(GLinkIOModule00.PowerGasIOOUT, true);
            }
            else if (strFunction == "TestPowerMark")
            {
                Task.Factory.StartNew(() =>
                {
                    TestMarkPower(markForm.m_nAddress0Card);
                    try
                    {
                        //chipAutoForm.AddListViewThread(null, CommonDefine.waferFileInfo.AllLaserMarkContent.Length.ToString(), Color.Red);
                        if (CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMax < markForm.m_fNormalPower
                        || CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMin > markForm.m_fNormalPower)
                        {
                            string strInfo = $"功率测试值{markForm.m_fNormalPower}超范围";
                            chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                        }
                        else
                        {
                            string strInfo = $"功率测试值{markForm.m_fNormalPower}正常";
                            chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                });

            }
            else if (strFunction == "TestPowerResult")
            {
                double fPower = markForm.m_fNormalPower;
                MessageBox.Show(fPower.ToString("F3"));
            }
            else if (strFunction == "WriteOPCData")
            {
                // waferInfo2OPC();
            }
            else if (strFunction == "ReadOPCData")
            {
                // OPC2WaferInfo();
            }

        }


        private void RefreshChipListUI(object sender, EventArgs e)
        {
            chipAutoForm.RefreshUI();
        }

        private void RefreashBoxListUI(object sender, EventArgs e)
        {
            chipAutoForm.RefreshUI();
        }

        private void SecsForm_RunRemoteCommandHandler(object sender, EventArgs e)//step 5 
        {
            switch (sender.ToString())
            {
                case "START":

                    //int nID = SECSEventData.WorkStart;
                    //secsForm.SetEventPos(nID);

                    chipAutoForm.OperateStart();

                    break;
                case "STOP":

                    //nID = SECSEventData.WorkStop;
                    //secsForm.SetEventPos(nID);

                    chipAutoForm.RemoteStop();

                    break;
                case "LoadFoup":

                    //chipAutoForm.RemoteLoadFoup();

                    break;
                case "Cancel":

                    chipAutoForm.RemoteCancle();

                    break;
                default:
                    break;
            }
        }

        private void SecsForm_ReadRecipParamHandler(object sender, EventArgs e)//eap请求数据
        {
            {
                string strRecipeEzd = sender.ToString();

                string strInfo = string.Format("EAP Recipe 参数请求消息：{0}", strRecipeEzd);
                CommonDefine.SaveWorkLogs(strInfo);

                string recipeFileName = secsForm.GetRecipeDir() + "\\" + strRecipeEzd;
                strRecipeEzd = strRecipeEzd.ToUpper();
                if (markForm != null)
                {
                    Thread.Sleep(1000);

                    //TODO:修改此函数
                    WriteSecsMarkParamData2(strRecipeEzd);

                    //光笔参数
                    for (int i = 0; i < 10; i++)
                    {
                        WriteSecsMarkParamData(i);
                    }
                    //WriteSecsMarkParamDataToSvid(SECSWorkFlowParam.nMarkLoop0);
                    secsForm.m_pReceip.nLoadType = 1;

                    strInfo = string.Format("EAP Recipe参数生成成功：{0}", strRecipeEzd);

                    CommonDefine.SaveWorkLogs(strInfo);

                }
            }
        }
        public void WriteSecsMarkParamData2(string strRecipeName)
        {
            secsForm.m_pReceip.RecipeName = strRecipeName;
            secsForm.m_pReceip.PowerRange = "0,100";
        }

        public void WriteSecsMarkParamData(int nPenNo)
        {
            if (markForm == null)
                return;
            int nMarkLoop = 1;//加工次数
            double dMarkSpeed = 100;//加工速度mm/s
            double dPowerRatio = 50;//功率百分比(0-100%)	
            double dCurrent = 0.5;//电流A
            int nFreq = 500;//频率HZ
            double dQPulseWidth = 10;//Q脉冲宽度us	
            int nStartTC = 200;//开始延时us
            int nLaserOffTC = 200;//激光关闭延时us 
            int nEndTC = 200;//结束延时us
            int nPolyTC = 200;//拐角延时us   //	
            double dJumpSpeed = 200; //跳转速度mm/s
            int nJumpPosTC = 200; //跳转位置延时us
            int nJumpDistTC = 200;//跳转距离延时us	
            double dEndComp = 20;//末点补偿mm
            double dAccDist = 20;//加速距离mm	
            double dPointTime = 20;//打点延时 ms						 
            bool bPulsePointMode = false;//脉冲点模式 
            int nPulseNum = 10;//脉冲点数目
            double dFlySpeed = 200;


            //光笔参数
            markForm.GetRecipePenParam(markForm.m_nAddress0Card, nPenNo, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth, ref nStartTC,
                ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp, ref dAccDist, ref dPointTime,
                ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);


            if (nPenNo < 10)
            {
                secsForm.m_pReceip.markParams[nPenNo].PenNO = nPenNo.ToString();
                secsForm.m_pReceip.markParams[nPenNo].MarkTimes = nMarkLoop.ToString();
                secsForm.m_pReceip.markParams[nPenNo].MarkSpeed = dMarkSpeed.ToString("F2");
                secsForm.m_pReceip.markParams[nPenNo].MarkFreq = nFreq.ToString();
                secsForm.m_pReceip.markParams[nPenNo].MarkPulse = dQPulseWidth.ToString("F2");
                secsForm.m_pReceip.markParams[nPenNo].LaserOffTC = nLaserOffTC.ToString();

            }

        }


        public void WriteCuttingIniValue(string keyName, string strText)
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";
            Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);
        }

        public string ReadCuttingIniValue(string keyName)
        {
            string strText = "";
            string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";
            if (File.Exists(fileName) == false)
            {
                return strText;
            }

            StringBuilder temp = new StringBuilder(255);
            Program.GetPrivateProfileString("CuttingIni", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();

            return strText;
        }

        public void WriteCuttingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";

            //string strText = setForm.GetCuttingPath();
            //string keyName = "CuttingPath";

            //Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);

            //keyName = "CuttingPath2";
            //strText = setForm.GetCuttingPath2();
            //Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);


            //strText = setForm.GetXmlPath();
            //keyName = "XmlPath";
            //Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);

            //strText = setForm.GetDelay();
            //keyName = "levelDelay";            
            //Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);

            if (chipAutoForm != null)
            {
                chipAutoForm.WriteCuttingIniValue();
            }

        }

        public void ReadCuttingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            string strText = "";
            string keyName = "CuttingPath";

            StringBuilder temp = new StringBuilder(255);

            //Program.GetPrivateProfileString("CuttingIni", keyName , strText, temp, 255, fileName);
            //strText = temp.ToString();
            //setForm.SetCuttingPath(strText);

            //keyName = "CuttingPath2";
            //Program.GetPrivateProfileString("CuttingIni", keyName, strText, temp, 255, fileName);
            //strText = temp.ToString();
            //setForm.SetCuttingPath2(strText);

            //keyName = "XmlPath";
            //Program.GetPrivateProfileString("CuttingIni", keyName , strText, temp, 255, fileName);
            //strText = temp.ToString();
            //setForm.SetXmlPath(strText);

            //keyName = "levelDelay";
            //Program.GetPrivateProfileString("CuttingIni", keyName, strText, temp, 255, fileName);
            //strText = temp.ToString();
            //setForm.SetDelay(strText);

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Form_SizeChanged(sender, e, tabControlEx, 40);
        }


        private void CuttingForm_Load(object sender, EventArgs e)
        {

            Form_Load(sender, e, tabControlEx);

            //threadWork = new Thread(new ThreadStart(work));
            //threadWork.Start();

            //threadSecs = new Thread(new ThreadStart(secsWork));
            //threadSecs.Start();

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

        public void StartWork()
        {
            threadWork = new Task(new Action(work));
            threadWork.Start();

            threadWorkMark = new Task(new Action(workMark));
            threadWorkMark.Start();

            threadWorkSmif = new Task(new Action(workSmif));
            //threadWorkSmif.Start();

            threadState = new Task(new Action(stateThread));
            threadState.Start();


            threadReady = new Task(new Action(readyThread));
            threadReady.Start();

            threadUI = new Task(new Action(UIThread));
            threadUI.Start();

        }

        public Boolean MyLock(object lockObject)
        {
            Boolean bLock = true;
            try
            {
                bLock = Monitor.TryEnter(lockObject);
            }
            catch (Exception)
            {
                bLock = false;
            }

            return bLock;
        }

        private void work()
        {
            workTimer = new System.Timers.Timer();
            workTimer.Interval = 100;
            workTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            workTimer.Enabled = true;
        }

        private void workMark()
        {
            workMarkTimer = new System.Timers.Timer();
            workMarkTimer.Interval = 100;
            workMarkTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_ElapsedMark);
            workMarkTimer.Enabled = true;
        }

        private void workSmif()
        {
            workSmifTimer = new System.Timers.Timer();
            workSmifTimer.Interval = 100;
            workSmifTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_ElapsedSmif);
            workSmifTimer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            //lock (autoWorkObject)
            if (MyLock(autoWorkObject))
            {
                try
                {
                    if (chipAutoForm != null)
                    {
                        chipAutoForm.OperateWorkFlow();
                    }
                }
                catch (Exception)
                {
                }
                Monitor.Exit(autoWorkObject);
            }
        }


        public Boolean SwitchRecipeForEap(string recipeName, ref string strInfo)//gts 切换 wmf recipe
        {
            Boolean bRet = false;

            if (recipeName.Length > 0)
            {
                string wmfFileName = secsForm.GetRecipeDir() + "\\" + recipeName + ".wmf";
                if (File.Exists(wmfFileName) == false)
                {
                    strInfo = string.Format("EAP Recipe 文件 {0}切换失败，文件不存在，请检查本地文件", wmfFileName);
                    //CommonDefine.SaveWorkLogs(strInfo);

                    //secsForm.SetEventPos(SECSEventData.ReadProductReecipeError);


                    //gtsForm.m_gtsControl.OperateClearLight();
                    //gtsForm.m_gtsControl.SetDo(AxisTestForm.RedLightIOOUT, 1);
                    //gtsForm.m_gtsControl.SetDo(AxisTestForm.BeepIOOUT, 1);


                    //SECSEventHandlerData evHandler = new SECSEventHandlerData();
                    //evHandler.nSVIndex = SECSWorkFlowParam.CancelCassetteID;
                    //evHandler.strSVContent = nBoxIndex.ToString();

                    //evHandler.nSVTipInfoIndex = SECSWorkFlowParam.CancelInfo;
                    //evHandler.strTipInfo = "Recipe 文件不存在";

                    //evHandler.nEventIndex = SECSEventData.DeviceCancel;
                    //gts_SetEAPDataHandler(evHandler, null);

                    //MessageBox.Show(strInfo);
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                    //chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                    return false;
                }

                if (markForm.openWMFile(wmfFileName))
                {
                    strInfo = string.Format("EAP Recipe 信息{0}切换成功", recipeName);
                    //CommonDefine.SaveWorkLogs(strInfo);
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
                    //secsForm.SetEventPos(SECSEventData.Recipe);

                    //gtsForm.m_gtsControl.m_nWorkFlow = AxisTestForm.SCAN_VALUE;
                    //gtsForm.m_gtsControl.m_nWorkState = AxisTestForm.WORK_AUTO;

                    //gtsForm.m_gtsControl.OperateClearLight();
                    //gtsForm.m_gtsControl.SetDo(AxisTestForm.GreenLightIOOUT, 1);

                    bRet = true;
                }
                else
                {
                    strInfo = string.Format("EAP Recipe 文件 {0}切换打开失败，请检查本地文件", wmfFileName);
                    //CommonDefine.SaveWorkLogs(strInfo);

                    //secsForm.SetEventPos(SECSEventData.ReadProductReecipeError);

                    //gtsForm.m_gtsControl.OperateClearLight();
                    //gtsForm.m_gtsControl.SetDo(AxisTestForm.RedLightIOOUT, 1);
                    //gtsForm.m_gtsControl.SetDo(AxisTestForm.BeepIOOUT, 1);
                    chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                    //SECSEventHandlerData evHandler = new SECSEventHandlerData();
                    //evHandler.nSVIndex = SECSWorkFlowParam.CancelCassetteID;
                    //evHandler.strSVContent = nBoxIndex.ToString();

                    //evHandler.nSVTipInfoIndex = SECSWorkFlowParam.CancelInfo;
                    //evHandler.strTipInfo = "Recipe 文件打开失败";

                    //evHandler.nEventIndex = SECSEventData.DeviceCancel;
                    //gts_SetEAPDataHandler(evHandler, null);

                    //gtsForm.m_gtsControl.ShowWarningDialog(strInfo);

                    bRet = false;
                }

            }

            return bRet;
        }

        void timer_ElapsedMark(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            //lock (autoWorkObject)
            if (MyLock(autoWorkMarkObject))
            {
                try
                {
                    if (chipAutoForm != null)
                    {
                        chipAutoForm.OperateMarkFlow();
                    }
                }
                catch (Exception)
                {
                }
                Monitor.Exit(autoWorkMarkObject);
            }
        }

        void timer_ElapsedSmif(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            //lock (autoWorkObject)
            if (MyLock(autoWorkSmifObject))
            {
                try
                {
                    if (chipAutoForm != null)
                    {
                        chipAutoForm.OperateSmifFlow();
                    }

                }
                catch (Exception)
                {
                }
                Monitor.Exit(autoWorkSmifObject);
            }
        }

        private void stateThread()
        {
            stateTimer = new System.Timers.Timer();
            stateTimer.Interval = 100;
            stateTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_State);
            stateTimer.Enabled = true;
        }


        void timer_State(object sender, System.Timers.ElapsedEventArgs e)
        {

            //
            //lock (stateWorkObject)
            //if ( MyLock(stateWorkObject) )
            //{
            //    try
            //    {
            //        OperateWorkState();                                        
            //    }
            //    catch (Exception)
            //    {

            //    }                

            //    Monitor.Exit(stateWorkObject);

            //}

            refreshTabpage();
        }

        private void readyThread()
        {
            readyTimer = new System.Timers.Timer();
            readyTimer.Interval = 100;
            readyTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Ready);
            readyTimer.Enabled = true;
        }

        void timer_Ready(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            //lock (readyWorkObject)
            if (MyLock(readyWorkObject))
            {
                try
                {
                    if (chipAutoForm != null)
                    {
                        chipAutoForm.OperateReadyMaterials();
                    }

                }
                catch (Exception)
                {

                }
                Monitor.Exit(readyWorkObject);
            }
        }

        private void UIThread()
        {
            UITimer = new System.Timers.Timer();
            UITimer.Interval = 200;
            UITimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_UI);
            UITimer.Enabled = true;
        }

        void timer_UI(object sender, System.Timers.ElapsedEventArgs e)
        {
            //
            //lock (UIWorkObject)
            if (MyLock(UIWorkObject))
            {
                try
                {
                    if (chipAutoForm != null)
                    {
                        chipAutoForm.RefreshBoxLight();
                    }

                }
                catch (Exception)
                {

                }
                Monitor.Exit(UIWorkObject);
            }
        }


        public string readCuttingFileName(string xmlFileName)
        {
            string llsName = "";
            XmlDocument doc = new XmlDocument();
            //xmlFileName = "d:\\1.xml";
            doc.Load(xmlFileName);       //加载Xml文件
            XmlElement rootElem = doc.DocumentElement;    //获取根节点

            XmlNodeList personNodes = rootElem.GetElementsByTagName("FrameVLM"); //获取person子节点集合

            foreach (XmlNode node in personNodes)
            {
                llsName = ((XmlElement)node).InnerText;
            }

            return llsName;
        }

        Boolean checkCodeRepeat(string barCode)
        {
            Boolean bRet = false;
            //string strValue = autoForm.ReadScanIni(barCode);
            //strValue.Trim();
            //if( strValue.Length > 0   )
            //{
            //    bRet = true;
            //}

            return bRet;
        }

        public void IntitalOpcOut()
        {
            //if (opcOperate != null)
            //{
            //    opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerYellowLight,false);
            //    opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerRedLight, false);
            //    opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, false);
            //    opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerBuzzer, false);
            //}
        }

        private void CuttingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteCuttingIni();

            if (markForm != null)
            {
                markForm.CloseMark();
            }

            if (secsForm != null)
            {
                secsForm.EndSecs();
            }

            if (robotSerialPortForm != null)
            {
                robotSerialPortForm.m_nType = 1;
                robotSerialPortForm.ClosePort();
            }

            if (waferSerialPortForm != null)
            {
                waferSerialPortForm.m_nType = 0;
                waferSerialPortForm.ClosePort();
            }

            if (laserPowerForm != null)
            {
                laserPowerForm.m_nType = 2;
                laserPowerForm.ClosePort();
            }

            if (smifForm != null)
            {
                smifForm.CloseClient();
            }

            if (smifForm2 != null)
            {
                smifForm2.CloseClient();
            }

            if (cameraForm != null)
            {
                cameraForm.CloseClient();
            }

            if (ocrSwitchRecipeForm != null)
            {
                ocrSwitchRecipeForm.CloseClient();
            }

            //if ( opcOperate != null )
            //{
            //    IntitalOpcOut();
            //    opcOperate.DisConnect();
            //}
        }

        private void CuttingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //tcpScanForm.CloseClient();
        }

        public string set2AutoPage()
        {
            string retString = "";
            return retString;
        }

        public void EndAllWork()
        {
            //autoForm.EndWork();
            //tcpScanForm.EndWork();
            //userForm.EndWork();
            //setForm.EndWork();
            //secsForm.EndWork();

        }

        public string GetThreadTabName(TabControl tabControl)
        {
            string retString = null;

            if (tabControl.InvokeRequired)
            {
                try
                {
                    retString = (string)tabControl.Invoke(new Func<String>(() => GetThreadTabName(tabControl)));
                }
                catch
                {

                }
            }
            else
            {
                if (tabControl != null && tabControl.SelectedTab != null)
                {
                    retString = tabControl.SelectedTab.Name;
                }
            }

            //this.BeginInvoke(dl, "delegate three"); // 异步调用委托,调用后立即返回并立即执行下面的语句
            //this.Invoke(getName, tabControl, retString); // 等待工作线程完成后, 才接着执行下面的语句.

            return retString;
        }

        public void refreshTabpage()
        {
            string sourceName = GetThreadTabName(tabControlEx);// tabControlEx.SelectedTab.Name;
            if (sourceName == "chipAutoForm")
            {
                if (chipAutoForm != null)
                {
                    chipAutoForm.RefreshUIState();
                }
            }
            else if (sourceName == "gtsForm")
            {
                if (gtsForm != null)
                {
                    gtsForm.RefreshUIState();
                }
            }
            else if (sourceName == "iOForm")
            {
                if (iOForm != null)
                {
                    iOForm.RefreshState(); 
                }
            }
            //else if (sourceName == "manualForm")
            //{
            //    manualForm.Initial();
            //}
        }
        private void tabControlEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshTabpage();
        }

        private void tabControlEx_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (chipAutoForm.m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                e.Cancel = true;
                return;
            }

            string tabPaeName = e.TabPage.Name;

            m_strCurrentPageName = tabPaeName;
            if (tabPaeName != null)
            {
                if (tabPaeName == "laserPowerForm" || tabPaeName == "robotSerialPortForm" || tabPaeName == "waferSerialPortForm")
                {
                    frm_register.mainUser = frm_user;

                    frm_register frm_Register1 = new frm_register();
                    if (frm_Register1.ShowDialog() == DialogResult.OK)
                    {

                    }
                    else
                    {
                        e.Cancel = true;
                        m_strCurrentPageName = "chipAutoForm";
                        return;
                    }
                }
                else if (tabPaeName == "frm_user")
                {
                    frm_register.mainUser = frm_user;

                    frm_register frm_Register1 = new frm_register();
                    if (frm_Register1.ShowDialog() == DialogResult.OK)
                    {

                    }
                    else
                    {
                        e.Cancel = true;
                        m_strCurrentPageName = "chipAutoForm";
                        return;
                    }
                }
            }
            else if (tabPaeName == "secsForm")
            {
                frm_register.mainUser = frm_user;

                frm_register frm_Register1 = new frm_register();
                if (frm_Register1.ShowDialog() == DialogResult.OK)
                {

                }
                else
                {
                    e.Cancel = true;
                    m_strCurrentPageName = "chipAutoForm";
                    return;
                }

            }

            //string sourceName = sender.ToString();

            //EndAllWork();

            //string tabPaeName = e.TabPage.Name;

            //m_strCurrentPageName = tabPaeName;
            //if (tabPaeName != null)
            //{
            //    if (tabPaeName == "autoForm")
            //    {
            //        autoForm.StartWork();
            //    }
            //    else if (tabPaeName == "tcpScanForm")
            //    {
            //        if (this.GetCurrentOp() < tcpScanForm.m_nCurrentLevel)
            //        {

            //            m_strCurrentPageName = "autoForm";
            //            MessageBox.Show("权限不足，不可操作");
            //            autoForm.StartWork();
            //            return;
            //        }

            //        tcpScanForm.StartWork();

            //    }
            //    else if (tabPaeName == "userForm")
            //    {
            //        if (this.GetCurrentOp() < userForm.m_nCurrentLevel)
            //        {                        
            //            m_strCurrentPageName = "autoForm";
            //            MessageBox.Show("权限不足，不可操作");
            //            autoForm.StartWork();
            //            return;
            //        }

            //        userForm.StartWork();
            //    }
            //    else if (tabPaeName == "setForm")
            //    {
            //        if (this.GetCurrentOp() < setForm.m_nCurrentLevel)
            //        {                        
            //            m_strCurrentPageName = "autoForm";
            //            MessageBox.Show("权限不足，不可操作");
            //            autoForm.StartWork();
            //            return;
            //        }

            //        setForm.StartWork();
            //    }

        }
        //Deselecting → Deselected → SelectedIndexChanged → Selecting → Selected → SelectedIndexChanged。
        private void tabControlEx_Deselected(object sender, TabControlEventArgs e)
        {
            //string tabPaeName = e.TabPage.Name;
            //m_preCurrentForm = autoForm;
            //if ( tabPaeName == "autoForm")
            //{
            //    m_preCurrentForm = autoForm;
            //}else if(tabPaeName == "tcpScanForm")
            //{
            //    m_preCurrentForm = tcpScanForm;
            //}
            //else if (tabPaeName == "userForm")
            //{
            //    m_preCurrentForm = userForm;
            //}
            //else if (tabPaeName == "setForm")
            //{
            //    m_preCurrentForm = setForm;

            //}
            //else if (tabPaeName == "secsForm")
            //{
            //    m_preCurrentForm = secsForm;
            //}

        }

        public void ClearStates(Boolean bOnlyClear = false)
        {
            if (bOnlyClear)
            {
                robotSerialPortForm.serialPort.Write(JelRobotOperate.EXCUTE_CLEAR);
                Thread.Sleep(500);
            }
            else
            {
                if (chipAutoForm.IsNormalWarning(false))
                {
                    robotSerialPortForm.serialPort.Write(JelRobotOperate.EXCUTE_CLEAR);
                    Thread.Sleep(500);

                }
            }


        }


        public Boolean IsNeedReadOCR(int nLaserType)
        {
            Boolean bResult = false;

            //object ob = opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.checkReadOCR);          
            //if (ob != null)
            //{
            //    bResult = (Boolean)ob;
            //}

            bResult = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].bCCDCheck;

            //if (nLaserType > 0)
            //{
            //    bResult = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].bCCDCheck;
            //}
            //else
            //{
            //    bResult = CommonDefine.waferFileInfo.AllLaserMarkContent[0].bCCDCheck &&
            //              CommonDefine.waferFileInfo.AllLaserMarkContent[1].bCCDCheck;
            //}

            return bResult;
        }

        public Boolean TestMarkPower(int nCard)
        {
            Boolean bResult = false;
            if (markForm == null)
            {
                return false;
            }

            //object ob = opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.PowerVacuum);

            //气缸出
            chipAutoForm.WriteItemValue(GLinkIOModule00.PowerGasIOOUT, true);
            Thread.Sleep(500);
            Boolean bEnd = false;
            object ob = gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.PowerGasOutIOIN);
            //ob = opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.Power_Cylinder_Work);
            if (ob != null)
            {
                bEnd = (Boolean)ob;
            }

            int nStartTick = Environment.TickCount;
            int nSleepMax = 3000;

            while (bEnd == false)
            {
                ob = gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.PowerGasOutIOIN);
                if (ob != null)
                {
                    bEnd = (Boolean)ob;
                }

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleepMax)
                    break;
            }


            if (bEnd)
            {
                string strEnt = "Power";
                if (true)
                {
                    int nShowCount = 0;
                    if (markForm.MarkEntity(nCard, strEnt))
                    {
                        do
                        {
                            markForm.m_fNormalPower = LyinkTestPower();
                            nShowCount++;
                            if (nShowCount >= 5 && nShowCount < 10)
                                chipAutoForm.AddListViewThread(null, $"功率测试结果：{markForm.m_fNormalPower}", Color.Blue);
                            Thread.Sleep(1000);
                        }
                        while (markForm.IsMarkFinish(nCard) == false);
                        chipAutoForm.AddListViewThread(null, $"功率测试结束", Color.Blue);
                    }
                }
            }
            else
            {
                chipAutoForm.AddListViewThread(null, "功率测试气缸未到工作位", Color.Red);
                return false;
            }

            chipAutoForm.WriteItemValue(GLinkIOModule00.PowerGasIOOUT, false);
            Thread.Sleep(500);

            bEnd = false;
            ob = gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.PowerGasOrigIOIN);
            if (ob != null)
            {
                bEnd = (Boolean)ob;
            }
            nStartTick = Environment.TickCount;

            while (bEnd == false)
            {
                ob = gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.PowerGasOrigIOIN);
                if (ob != null)
                {
                    bEnd = (Boolean)ob;
                }

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleepMax)
                    break;

            }

            bResult = bEnd;

            return bResult;
        }

        public void OperateLaserPower(string strCommand)
        {
            laserPowerForm.OperateAction(strCommand);
        }

        public double LyinkTestPower()
        {
            double fValue = 0;
            laserPowerForm.EmptyReceiveBuffer();
            laserPowerForm.serialPort.Synchronous = true;
            OperateLaserPower(JelRobotOperate.ReadPowerCommand);

            int nStartTick = Environment.TickCount;
            int nMaxSleep = 2000;
            while (laserPowerForm.GetLastResponseForRobot(false).Length <= 0)
            {
                laserPowerForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nMaxSleep)
                    break;
            }

            laserPowerForm.serialPort.Synchronous = false;
            string strRet = laserPowerForm.GetLastResponseForRobot(false);


            if (strRet.Length > 3)
            {
                try
                {

                    fValue = double.Parse(strRet);
                }
                catch
                {

                }
            }

            return fValue;
        }

        public Boolean CheckTestPower(ref double fPower)
        {
            Boolean bRet = false;
            double fValue = 0;
            double fMinValue = 0;
            double fMaxValue = 0;
            if (markForm == null)
            {
                return false;
            }

            fValue = markForm.m_fNormalPower;
            if (fValue == 0)
            {
                return bRet;
            }

            if (fValue < markForm.m_fMinPower)
            {
                return bRet;
            }

            if (fValue > markForm.m_fMaxPower)
            {
                return bRet;
            }

            bRet = true;

            return bRet;
        }

        /// <summary>
        /// 获取recipe信息
        /// </summary>
        /// <param name="sender">RecipeName</param>
        /// <param name="e"></param>
        private void SecsForm_SwitchReceipHandler(object sender, EventArgs e)
        {
            if (chipAutoForm.m_nWorkState == AxisTestForm.WORK_AUTO)
                return;

            GtsControl gtsControl = gtsForm.m_gtsControl;
            if (gtsControl == null)
            {
                return;
            }

            List<string> strList = (List<string>)sender;

            string strRecipe = strList[0];
            int nBoxIndex = int.Parse(strList[1]);


            //int nBoxIndex = m_nEapBoxIndex;
            //if (strList.Count >= 3)
            //{
            //    try
            //    {
            //        nBoxIndex = int.Parse(strList[2]);
            //    }
            //    catch (Exception)
            //    {
            //        nBoxIndex = -1;
            //    }
            //}

            //string strLotID = "";
            //if( strList.Count >= 1 )
            //{
            //    strLotID = strList[0];
            //}           


            string strInfo = "";

            if (strRecipe.Length <= 0)
            {

                //strInfo = $"料盒ID验证完成，Recipe信息获取失败！请移走料盒{nBoxIndex + 1}检查";
                strInfo = $"EAP Recipe信息获取失败: strRecipe.Lenght <= 0";
                CommonDefine.SaveWorkLogs(strInfo);

                secsForm.SetEventPos(SECSEventData.ReadProductionRecipeError);

                chipAutoForm.ShowWarningDialog(strInfo);
            }
            else if (nBoxIndex < 0)
            {
                strInfo = "EAP Recipe返回料盒ID错误: nBoxIndex < 0";
                CommonDefine.SaveWorkLogs(strInfo);

                secsForm.SetEventPos(SECSEventData.ReadProductionRecipeError);

                chipAutoForm.ShowWarningDialog(strInfo);
            }
            else
            {
                //CommonDefine.m_SecsData.strLotID = strLotID;
                CommonDefine.m_SecsData.recipeNameArray[nBoxIndex] = strRecipe;
                CommonDefine.m_SecsData.nBoxIndexOther = nBoxIndex;

                strInfo = $"EAP---收到PPSELECT指令：{strRecipe}";
                chipAutoForm.AddListViewThread(null, strInfo, Color.Green);

                strInfo = string.Format("料盒：{0} Receip：{1}", nBoxIndex, strRecipe);
                CommonDefine.SaveWorkLogs(strInfo);

                Boolean bSwitch = true;
                string fileNmae = secsForm.GetRecipeDir() + "\\" + strRecipe + ".wmf";
                if (File.Exists(fileNmae))
                {

                    secsForm.SetValue(SECSWorkFlowParam.PPexeName, strRecipe);

                    secsForm.SetEventPos(SECSEventData.Recipe);

                    strInfo = "recepe校验成功！";
                    //CommonDefine.SaveWorkLogs(strInfo);

                    chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
                    //chipAutoForm.m_nRobotFlow = AxisTestForm.EAP_SCAN_VALUE;
                    //chipAutoForm.m_nWorkState = AxisTestForm.WORK_AUTO;

                    //MessageBox.Show(gtsControl.m_nWorkFlow.ToString());
                    //if (gtsControl.m_nWorkFlow == AxisTestForm.EAP_SCAN_VALUE )
                    //{
                    //    gtsControl.OperateClearLight();
                    //    gtsControl.SetDo(AxisTestForm.GreenLightIOOUT, 1);

                    //    CommonDefine.SaveWorkLogs("EAP验证成功，自动开始");
                    //    gtsControl.m_nWorkState = AxisTestForm.WORK_AUTO;

                    //}
                    //else
                    //{
                    //    CommonDefine.SaveWorkLogs($"EAP验证成功，当前状态不正确{gtsControl.m_nWorkFlow}");
                    //}
                }
                else
                {
                    strInfo = $"Recipe校验: File.Exists == False";
                    CommonDefine.SaveWorkLogs(strInfo);

                    secsForm.SetEventPos(SECSEventData.ReadProductionRecipeError);

                    chipAutoForm.ShowWarningDialog(strInfo);
                }
            }

        }

        /// <summary>
        /// 获取打标信息
        /// </summary>
        /// <param name="sender">BoxIndex、MapContent、MarkContent</param>
        /// <param name="e"></param>
        private void SecsForm_SetMarkContentHandler(object sender, EventArgs e)
        {
            if (chipAutoForm.m_nWorkState == AxisTestForm.WORK_AUTO)
                return;

            GtsControl gtsControl = gtsForm.m_gtsControl;
            if (gtsControl == null)
            {
                return;
            }

            List<string> strList = (List<string>)sender;

            string mapContent = strList[0];
            string markContent = strList[1];
            int boxIndex = int.Parse(strList[2]);

            string strInfo = $"EAP---收到Content指令：{mapContent}---{markContent}---{boxIndex}";
            chipAutoForm.AddListViewThread(null, strInfo, Color.Green);

            string strWarn = "";
            if (mapContent.Length != AxisTestForm.BOX_CHIPSCOUNT || markContent.Length <= 0 || !mapContent.Contains("1"))
            {
                strInfo = string.Format("EAP_CONTENT数据异常请检查：mapContent != AxisTestForm.BOX_CHIPSCOUNT || markContent.Length <= 0");
                chipAutoForm.AddListViewThread(null, strInfo, Color.Red);

                MessageBox.Show(strInfo, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                secsForm.SetEventPos(SECSEventData.MarkContentFailed);

            }
            else
            {
                try
                {
                    string[] markContents = markContent.Split(',');
                    CommonDefine.m_SecsData.strMappingContent = mapContent;
                    CommonDefine.m_SecsData.strMarkContent = markContents;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                secsForm.SetEventPos(SECSEventData.MarkContentOK);
            }


            //if (markContent.Length > 0)
            //{
            //    CommonDefine.m_SecsData.strMappingContent = markContent;

            //    strInfo = string.Format("EAP打标数据：{0}", markContent);

            //    string[] markContents = markContent.Split(',');
            //    CommonDefine.m_SecsData.strMarkContent = markContents;


            //    int nCount = markContents.Length;
            //    if (nCount != AxisTestForm.BOX_CHIPSCOUNT)
            //    {
            //        报警
            //        secsForm.SetEventPos(SECSEventData.MarkContentFailed);

            //        strWarn = string.Format("EAP打标数据数量错误-{0},请检查", markContent);

            //        chipAutoForm.ShowWarningDialog(strWarn);

            //    }
            //    else
            //    {
            //        chipAutoForm.m_chipSettingForm.SetEAPChipsText(CommonDefine.m_SecsData.nBoxIndexOther, mapContent, markContents);

            //        chipAutoForm.RefreshUI();
            //        secsForm.SetEventPos(SECSEventData.MarkContentOK);

            //        chipAutoForm.AddListViewThread(null, "打标数据正确，可以EAP远程启动或机台启动开始", Color.Blue);

            //        chipAutoForm.m_bEAPLockMarkContent = false;
            //        chipAutoForm.m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
            //    }
            //}
            //else
            //{
            //    secsForm.SetEventPos(SECSEventData.MarkContentFailed);
            //    报警

            //    strWarn = string.Format("EAP打标数据空：{0}，请检查", markContent);
            //    chipAutoForm.ShowWarningDialog(strWarn);
            //}

        }

        /// <summary>
        /// 获取读取RFID的BoxIndex
        /// </summary>
        /// <param name="sender">BoxIndex</param>
        /// <param name="e"></param>
        private void SecsForm_ReadRFIDContentHandler(object sender, EventArgs e)
        {
            //if (chipAutoForm.m_nWorkState == AxisTestForm.WORK_AUTO)
            //    return;

            List<string> strList = (List<string>)sender;

            int nBoxIndex = int.Parse(strList[0]);

            CommonDefine.m_SecsData.nBoxIndexOther = nBoxIndex;

            string strInfo = $"EAP---收到ReadRFID指令：{nBoxIndex}";
            chipAutoForm.AddListViewThread(null, strInfo, Color.Green);

            chipAutoForm.m_nRobotFlow = AxisTestForm.READ_RFID;
            chipAutoForm.m_nWorkState = AxisTestForm.WORK_AUTO;
        }

        private void SecsForm_LoadFoupHandler(object sender, EventArgs e)
        {
            if (chipAutoForm.m_nWorkState == AxisTestForm.WORK_AUTO)
                return;

            List<string> strList = (List<string>)sender;

            int nBoxIndex = int.Parse(strList[0]);

            CommonDefine.m_SecsData.nBoxIndexWork = nBoxIndex;

            string strInfo = $"EAP---收到LoadFoup指令：{nBoxIndex}";
            chipAutoForm.AddListViewThread(null, strInfo, Color.Green);

            //chipAutoForm.m_nRobotFlow = AxisTestForm.EAP_SCAN_VALUE;
            chipAutoForm.m_nRobotFlow = AxisTestForm.SCAN_VALUE;
            chipAutoForm.m_nWorkState = AxisTestForm.WORK_AUTO;
        }

        private void SecsForm_WarningHandler(string msg)
        {
            WarningForm form = new WarningForm(this);
            form.SetInfoText(msg);
            form.ShowDialog();
            form = null;
        }

        public Boolean IsEAPRunMode()
        {
            Boolean isEAP = false;
            if (secsForm.m_nCommState == 2 && secsForm.m_nControlState == 2)
            {
                isEAP = true;
            }
            return isEAP;
        }

    }
}