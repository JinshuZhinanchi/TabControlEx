using jog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MyCommanLib;
using System.Threading.Tasks;

namespace TabControlExDemo
{
    public partial class WaferManualForm : BaseForm
    {        
        public WaferManualForm()
        {
            InitializeComponent();
        }

    
        public Boolean IsDeviceRunning(Boolean bCheckEAP = true)
        {
            Boolean bRunning = false;           
            return bRunning;
        }


        #region loadPort
        //=====================loadPort start =====================================
        
        public void InitialSmif()
        {
            string strInfo = "";

            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {

                strInfo = "料盒索引错误,请准备好后按开始";
                MessageBox.Show(strInfo);
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            OperateSmifHelper.Initial(tcpForm);

            MessageBox.Show("初始化成功！");
        }

       

        private void OpenSmif()
        {
            string strInfo = "开始打开Smif";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);


            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {
               
                strInfo = "料盒索引错误,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }


            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {
                mainForm.chipAutoForm.ResetSmif(nBoxIndex);
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {                
                strInfo = "smif错误报警,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            string strRet = OperateSmifHelper.OpenSmif(tcpForm);
            if (strRet.Length <= 0 || mainForm.chipAutoForm.IsSmifOpened(nBoxIndex) == false)
            {
                strInfo = $"打开Smif失败--{strRet}";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                warnForm.ShowDialog();
                warnForm = null;
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                return;
            }

            strInfo = $"打开Smif成功--{strRet}";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
            //MessageBox.Show(strInfo);
        }

        private void CloseSmif()
        {
            string strInfo = "开始关闭Smif";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

           
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {
               
                strInfo = "料盒索引错误,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {
                mainForm.chipAutoForm.ResetSmif(nBoxIndex);
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {
               
                strInfo = "smif错误报警,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            string strRet = OperateSmifHelper.CloseSmif(tcpForm);
            if (strRet.Length > 0)
            {
                strInfo = $"关闭Smif失败请检查--{strRet}";

                MessageBox.Show(strInfo);

                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                return;
            }

            strInfo = $"关闭Smif成功--{strRet}";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
            MessageBox.Show(strInfo);
        }

        public void GetState()
        {
            string strInfo;

            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {

                strInfo = "料盒索引错误,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {
                mainForm.chipAutoForm.ResetSmif(nBoxIndex);
            }

            if (mainForm.chipAutoForm.IsSmifError(nBoxIndex))
            {

                strInfo = "smif错误报警,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            string strRet = OperateSmifHelper.GetState(tcpForm);
            if (strRet.Length <= 0)
            {
                strInfo = $"获取SMIF状态失败--{strRet}";

                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                return;
            }

            strInfo = $"SMIF状态--{strRet}";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }

        private void buttonBoxIn_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }                      

            if( mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig() )
            { }
            else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            Task smifTask = new Task(new Action(OpenSmif));
            smifTask.Start();

            if (mainForm.IsEAPRunMode())
            {
                CommonDefine.bAccept = false;
            }
           
        }

        private void buttonBoxOut_Click(object sender, EventArgs e)
        {

            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            Task smifTask = new Task(new Action(CloseSmif));
            smifTask.Start();

            if (mainForm.IsEAPRunMode())
            {
                CommonDefine.bAccept = true;
            }
        }

        private void buttonLoadPortInitial_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            { }
            else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}Smif1初始化", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            Task smifTask = new Task(new Action(InitialSmif));
            smifTask.Start();
        }

        private void buttonBoxReset_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}Smif1复位", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            OperateSmifHelper.Reset(mainForm.smifForm);
        }

        private void buttonLoadPortMapping_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            Task smifTask = new Task(new Action(ReadSmif));
            smifTask.Start();
        }

        private void ReadSmif()
        {
            string strInfo;
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {

                strInfo = "料盒索引错误,请准备好后按开始";
                MessageBox.Show(strInfo);
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }
                      
            string strMap = OperateSmifHelper.GetMappiingContent(tcpForm);

            strInfo = string.Format("料盒1Mapping信息:{0}", strMap);
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }

        private void ReadRFID()
        {
            string strInfo;
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {

                strInfo = "料盒索引错误,请准备好后按开始";
                MessageBox.Show(strInfo);
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            string strRFID = OperateSmifHelper.GetRFID(tcpForm);

            string[] arr = strRFID.Split('/');
            strRFID = arr[2].Substring(0, arr[2].Length - 2);
            strInfo = string.Format("RFID:{0}", strRFID);
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }


        private void buttonBoxIn2_Click(object sender, EventArgs e)
        {

            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}打开Smif2", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            Task smifTask = new Task(new Action(OpenSmif));
            smifTask.Start();

        }

        private void buttonBoxOut2_Click(object sender, EventArgs e)
        {

            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}关闭Smif2", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            Task smifTask = new Task(new Action(CloseSmif));
            smifTask.Start();
        }

        private void buttonLoadPortInitial2_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}初始化Smif2", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            Task smifTask = new Task(new Action(InitialSmif));
            smifTask.Start();
        }

        private void buttonBoxReset2_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}复位Smif2", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            OperateSmifHelper.Reset(mainForm.smifForm2);
        }

        private void buttonLoadPortMapping2_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if (mainForm.chipAutoForm.IsRAarmOrig() && mainForm.chipAutoForm.IsLAarmOrig())
            {

            }else
            {
                MessageBox.Show("机械手不在原点位，请检查");
                return;
            }

            string strInfo = string.Format("{0}料盒2Mapping", "");
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

        }      
        //=====================loadPort end =======================================
        #endregion loadPort

        #region Robot
        //=====================Robot Start =============================
        private void buttonRobotReset_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            string strInfo = "";

            if(mainForm.chipAutoForm.IsRAarmXiLiao()  )
            {
                strInfo = "上手臂有料，不可复位";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);

                return;
            }

            if (mainForm.chipAutoForm.IsLAarmXiLiao())
            {
                strInfo = "下手臂有料，不可复位";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);
                return;
            }

            strInfo = string.Format("{0}机械手复位", "");            
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
            mainForm.chipAutoForm.ResetWork();
        }

        private void buttonRobotMap_Click(object sender, EventArgs e)
        {
            Task smifTask = new Task(new Action(GetState));
            smifTask.Start();

            return;

            if (IsDeviceRunning())
            {
                return;
            }

            string strInfo = "";

            if (mainForm.chipAutoForm.IsRAarmXiLiao())
            {
                strInfo = "上手臂有料，不可操作";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);

                return;
            }

            if (mainForm.chipAutoForm.IsLAarmXiLiao())
            {
                strInfo = "下手臂有料，不可操作";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);
                return;
            }

            strInfo = "机械手扫料开始！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);


            int nBoxIndex = comboBoxLoadPort.SelectedIndex;

            if (nBoxIndex < 0 || nBoxIndex > 1)
            {              
                strInfo = "料盒索引错误,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return ;
            }

            //if (mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_SBOX_POS) == false)
            //{              
            //    strInfo = "机械手X轴到位错误,请准备好后按开始！";
            //    MessageBox.Show(strInfo);
            //    return ;
            //}

            Point tempPoint;           
            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;

            if (nBoxIndex == 0)
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.START_MAPPING_C1));
            }
            else
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.START_MAPPING_C2));
            }

            //等待结束
            int nResult = 1;// WaitRobotResult();
            if (nResult == 1)
            {
                int nStartTick = Environment.TickCount;

                Boolean bUseHandler = false;

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();

                    int dt = CommonDefine.DelayedTime(nStartTick);
                    //if (dt > 2000 && dt < 3000)
                    //{
                    //    object ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.RobotRunningX24);
                    //    if (ob != null)
                    //    {
                    //        bUseHandler = !((Boolean)ob);
                    //    }
                    //    if (bUseHandler)
                    //    {
                    //        break;
                    //    }
                    //}
                    //else
                    //{
                    //    bUseHandler = false;
                    //}
                    if (dt > 50000)
                        break;
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                if (bUseHandler)
                {                  ;

                    strInfo = "正在使用手柄或者机械手出错，请关闭手柄后再试！";
                    MessageBox.Show(strInfo);
                    return ;
                }

                string strResult = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);

                if (strResult.Length < 27)
                {                 
                    strInfo = string.Format("返回数据有误,请检查后重新扫描！-{0}", strResult);
                    MessageBox.Show(strInfo);
                    return ;
                }

                string strMapping = strResult.Substring(2, strResult.Length - 2);
                string[] sArray = strMapping.Split(',');

                string strShowInfo = "";
                for (int i = 0; i < sArray.Length; i++)
                {
                    if (i == 0)
                    {
                        int nnn = sArray[i].Length;
                        strShowInfo = (i + 1).ToString() + ":" + sArray[i].Substring(nnn - 1, 1) + ",";
                    }
                    else
                    {
                        strShowInfo = strShowInfo + (i + 1).ToString() + ":" + sArray[i] + ",";
                    }
                }

                mainForm.chipAutoForm.ResetWork();

                MessageBox.Show(strShowInfo);
            }
            
        }

        private void buttonUpTake_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }
         
            string strInfo = "机械手取料开始！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

          
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;

            if (mainForm.chipAutoForm.IsSmifOpened(nBoxIndex) == false)
            {
                strInfo = "Smif料盒未打开,不可放片，请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            if (mainForm.chipAutoForm.IsMaterialExit(nBoxIndex) == false)
            {              
                strInfo = "没有可取的料,请准备好后按开始！";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);
                return ;
            }




            if (mainForm.chipAutoForm.IsRAarmXiLiao())
            {
                strInfo = "上手臂有料，不可取料";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);
                return;
            }

            //Boolean bRet1 = mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);
            //Boolean bRet2 = mainForm.mOpcAxisOperater.IS_OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);

            //if (bRet1  == false  || bRet2 == false)
            //{
             
            //    strInfo = $"机械手X轴到位错误或者位置感应器位置不正确{bRet1} - {bRet2},请准备好后按开始！";
            //    mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
            //    MessageBox.Show(strInfo);
            //    return;
            //}

            int nStartCommandIndex = 0;
            if (nBoxIndex == 0)
            {
                nStartCommandIndex = JelRobotOperate.TAKE_CASS1_FIRSTCHIP;
            }
            else if (nBoxIndex == 1)
            {
                nStartCommandIndex = JelRobotOperate.TAKE_CASS2_FIRSTCHIP;
            }
            else
            {             
                strInfo = "料盒信息错误,请准备好后按开始！";
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                MessageBox.Show(strInfo);
                return;
            }

            int nLayerIndex = (int)( numericUpDownLayer.Value -1);
            
            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;
            mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex + nLayerIndex));

            int nRobotSleep = 8000;
            //等待结束
            int nResult = 1;// WaitRobotResult();
            if (nResult == 1)
            {
                int nStartTick = Environment.TickCount;

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > nRobotSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                        break;
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                string strResult = mainForm.robotSerialPortForm.GetLastResponse();

                if (strResult.Contains("OK") == false)
                {                    
                    string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                    strInfo = string.Format("取料失败，请检查！返回内容：{0}", err);

                    MessageBox.Show(strInfo);
                    return ;
                }
            }                  

        }

        private void buttonUpPut_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            DialogResult dr = MessageBox.Show("请确认放料对应的槽位是否有料，然后选择是否继续 ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {

            }
            else
            {
                return;
            }
                        

            int nLoadPort = comboBoxLoadPort.SelectedIndex;           

            string strInfo = "";          

            strInfo = string.Format("{0}上手臂放片", "");            
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);


            int nLayerIndex = (int)numericUpDownLayer.Value -1;
            int nStartCommandIndex = JelRobotOperate.PUT_CASS1_FIRSTCHIP;

         
        }
                
              

        private void buttonPlatePut_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            string strInfo = "平台放片开始";
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            if (mainForm.chipAutoForm.IsXunbianExitWafer())
            {
                strInfo = "平台上已有料，请检查！";
                MessageBox.Show(strInfo);
                return;
            }

            //Boolean bRet1 = mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);
            //Boolean bRet2 = mainForm.mOpcAxisOperater.IS_OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);

            //if (bRet1 == false || bRet2 == false)               
            //{

            //    strInfo = $"机械手X轴到位错误或者位置感应器位置不正确{bRet1}-{bRet2},请准备好后按开始！";
            //    MessageBox.Show(strInfo);
            //    return;
            //}

            //if (mainForm.mOpcAxisOperater.OperateWafer_MPos() == false)
            //{                
            //    strInfo = "寻边器没有到接片位，请准备好后按开始！";

            //    MessageBox.Show(strInfo);
            //    return;
            //}


            //Boolean bXifu = mainForm.mOpcAxisOperater.IsRAarmXifu();
            //if (bXifu)
            //{

            //    strInfo = "寻边器感应有料，请检查!";
            //    MessageBox.Show(strInfo);                       
            //    return ;
            //}


            OperateAlignerHelper.OperateAlignerReady(mainForm.waferSerialPortForm);

            int nStartCommandIndex = JelRobotOperate.R_PUTWAFER_ALIGN;        

            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;
            mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex));

            int nStartTick = Environment.TickCount;
            int m_RobotCommdSleep = 8000;
            while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
            {
                mainForm.robotSerialPortForm.serialPort.synReadAllData();
                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > m_RobotCommdSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                    break;
            }
            mainForm.robotSerialPortForm.serialPort.Synchronous = false;

            string strResult = mainForm.robotSerialPortForm.GetLastResponse();
            if (strResult.Contains("OK") == false || mainForm.chipAutoForm.IsNormalWarning(false))
            {
                
                string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                strInfo = string.Format("放料失败，请检查！返回内容：{0}", strResult);

                MessageBox.Show(strInfo);
                return;
            }

            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;
            mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.A_MOVETO_ORIG));

            nStartTick = Environment.TickCount;
            while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
            {
                mainForm.robotSerialPortForm.serialPort.synReadAllData();
                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > m_RobotCommdSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                    break;
            }

            mainForm.robotSerialPortForm.serialPort.Synchronous = false;

            strResult = mainForm.robotSerialPortForm.GetLastResponse();
            if (strResult.Contains("OK") == false || mainForm.chipAutoForm.IsNormalWarning(false))
            {               
                string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                strInfo = string.Format("手臂退出失败，请检查！返回内容：{0}", err);
                MessageBox.Show(strInfo);
                return;
            }
                        
            strInfo = "上手臂寻边放料完成！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }

        private void buttonPlateTake_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            string strInfo = "平台取片";
           
            int nLoadPort = comboBoxLoadPort.SelectedIndex;

            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
                     
            //if (mainForm.mOpcAxisOperater.IsXunbianExitWafer() == false)
            //{

            //    strInfo = "寻边位没有可取的料,请准备好后按开始,重新取料！";

            //    MessageBox.Show(strInfo);
            //    return;
            //}

            //Boolean bRet1 = mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);
            //Boolean bRet2 = mainForm.mOpcAxisOperater.IS_OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS);

            //if (bRet1 == false || bRet2 == false)
            //{

            //    strInfo = $"机械手X轴到位错误或者位置感应器位置不正确{bRet1}-{bRet2},请准备好后按开始！";
            //    MessageBox.Show(strInfo);
            //    return;
            //}


            //if (mainForm.mOpcAxisOperater.OperateWafer_MPos(false) == false)
            //{                
            //    strInfo = "wafer到接片位失败,请准备好后按开始！";

            //    MessageBox.Show(strInfo);
            //    return;
            //}

            //mainForm.mOpcAxisOperater.OperateWaferXifu(false);
            //Thread.Sleep(500);

            //Boolean bXifu = mainForm.mOpcAxisOperater.IsWaferXifu();
            //if (bXifu == false)
            //{                
            //    strInfo = "寻边器感应无料，请检查！";
            //    MessageBox.Show(strInfo);
            //    return;
            //}


            strInfo = "寻边取料开始！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            int nStartCommandIndex = 0;
            if (nBoxIndex == 0)
            {
                nStartCommandIndex = JelRobotOperate.L_TAKEWAFER_ALIGN;
            }
            else if (nBoxIndex == 1)
            {
                nStartCommandIndex = JelRobotOperate.L_TAKEWAFER_ALIGN;
            }
            else
            {
                
                strInfo = "料盒信息错误,请准备好后按开始！";

                MessageBox.Show(strInfo);
                return;
            }

            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;
            mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex));

            int m_RobotCommdSleep = 8000;
            //等待结束
            int nResult = 1;// WaitRobotResult();
            if (nResult == 1)
            {
                int nStartTick = Environment.TickCount;

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > m_RobotCommdSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                        break;
                }


                mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                string strResult = mainForm.robotSerialPortForm.GetLastResponse();

                if (strResult.Contains("OK") == false || mainForm.chipAutoForm.IsNormalWarning(false))
                {
                   
                    string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                    strInfo = string.Format("取料失败，请检查！返回内容：{0}", strResult);

                    MessageBox.Show(strInfo);
                    return;
                }
            }

            strInfo = "下手臂寻边取料完成！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }


        private void buttonDnPut_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            DialogResult dr = MessageBox.Show("请确认放料对应的槽位是否有料，然后选择是否继续 ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {

            }
            else
            {
                return;
            }

            int nLoadPort = comboBoxLoadPort.SelectedIndex;
                    
            string strInfo = "";
            strInfo = "Smif料盒放料开始";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

            
            int nBoxIndex = comboBoxLoadPort.SelectedIndex;
            int nLayerIndex = (int)numericUpDownLayer.Value - 1;
            int nStartCommandIndex = JelRobotOperate.PUT_CASS1_FIRSTCHIP2;

            if (mainForm.chipAutoForm.IsSmifExit(nBoxIndex) == false)
            {
                
                strInfo = "没有Smif料盒可放,请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }


            if (mainForm.chipAutoForm.IsSmifOpened(nBoxIndex) == false)
            {                
                strInfo = "Smif料盒未打开,不可放片，请准备好后按开始！";
                MessageBox.Show(strInfo);
                return;
            }

            if (nBoxIndex == 0)
            {
                nStartCommandIndex = JelRobotOperate.PUT_CASS1_FIRSTCHIP2;
            }
            else if (nBoxIndex == 1)
            {
                nStartCommandIndex = JelRobotOperate.PUT_CASS2_FIRSTCHIP2;
            }
            else
            {
                
                strInfo = "料盒信息错误,请准备好后按开始！";

                MessageBox.Show(strInfo);
                return;
            }

            int nPosIndex = PLCOpcFeedParameters.PLCConstDefine.ROBOT_SMIF1_POS + nBoxIndex;
         

            //Boolean bRet1 = mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(nPosIndex);
            //Boolean bRet2 = mainForm.mOpcAxisOperater.IS_OperateRobotX_WorkPos(nPosIndex);

            //if (bRet1 == false || bRet2 == false)
            //{

            //    strInfo = $"机械手X轴到位错误或者位置感应器位置不正确{bRet1}-{bRet2},请准备好后按开始！";
            //    MessageBox.Show(strInfo);
            //    return;
            //}


            Boolean bXifu = mainForm.chipAutoForm.IsLAarmXifu();
            if (bXifu == false)
            {
                
                strInfo = "没有吸料，请检查！";

                MessageBox.Show(strInfo);
                return;                               
            }

            mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            mainForm.robotSerialPortForm.serialPort.Synchronous = true;
            mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex + nLayerIndex));

            int m_RobotCommdSleep = 8000;
            int nResult = 1;// WaitRobotResult();
            if (nResult == 1)
            {
                int nStartTick = Environment.TickCount;

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > m_RobotCommdSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                        break;
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                string strResult = mainForm.robotSerialPortForm.GetLastResponse();
                if (strResult.Contains("OK") == false || mainForm.chipAutoForm.IsNormalWarning(false))
                {                    
                    string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                    strInfo = string.Format("放料失败，请检查！返回内容：{0}", strResult);

                    MessageBox.Show(strInfo);
                    return;
                }

                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.A_MOVETO_ORIG));
                nResult = 1;// WaitRobotResult();


                if (nResult == 1)
                {

                    nStartTick = Environment.TickCount;

                    while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                    {
                        mainForm.robotSerialPortForm.serialPort.synReadAllData();
                        int dt = CommonDefine.DelayedTime(nStartTick);
                        if (dt > m_RobotCommdSleep )
                            break;
                    }

                    mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                    strResult = mainForm.robotSerialPortForm.GetLastResponse();
                    if (strResult.Contains("OK") == false || mainForm.chipAutoForm.IsNormalWarning(false))
                    {
                       
                        string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                        strInfo = string.Format("手臂退出失败，请检查！返回内容：{0}", err);

                        MessageBox.Show(strInfo);
                        return;
                    }
                }                               
            }

            strInfo = "下手臂smif放料完成！";
            mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }

        private void buttonAlignVacuum_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if(buttonAlignVacuum.Text == "平台真空")
            {
                OperateAlignerHelper.XIFU(mainForm.waferSerialPortForm);
                Thread.Sleep(500);
                buttonAlignVacuum.Text = "关闭平台真空";
            }
            else
            {
                OperateAlignerHelper.CloseXIFU(mainForm.waferSerialPortForm);
                Thread.Sleep(500);
                buttonAlignVacuum.Text = "平台真空";
            }
            string strInfo = string.Format("{0}寻边真空", "");
                         
        }

        private void buttonUpVacuum_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if(buttonUpVacuum.Text == "上手臂真空")
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMUPOPEN);
                buttonUpVacuum.Text = "关闭上手臂真空";
            }
            else
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMUPCLOSE);
                buttonUpVacuum.Text = "上手臂真空";
            }

            string strInfo = string.Format("{0}上手臂真空", "");

            //mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

        }

        private void buttonDnVacuum_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            if(buttonDnVacuum.Text == "下手臂真空")
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMDNOPEN);
                buttonDnVacuum.Text = "关闭下手臂真空";
            }
            else
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMDNCLOSE);
                buttonDnVacuum.Text = "下手臂真空";
            }

            string strInfo = string.Format("{0}下手臂真空", "");
            //mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);
        }



        //=====================Robot end   =============================
        #endregion robot

        private void button_startAliger_Click(object sender, EventArgs e)
        {
            //if (OperateAlignerHelper.OperateFindNotch(mainForm.waferSerialPortForm))
            //{

            //}

            int nPos = int.Parse(tb_Angle.Text.Trim());
            if(OperateAlignerHelper.OperateAlignerWA(mainForm.waferSerialPortForm, nPos) == false)
            {
                OperateAlignerHelper.OperateFindNotch2(mainForm.waferSerialPortForm, nPos);
            }
        }

        private void btn_readRFID_Click(object sender, EventArgs e)
        {
            if (IsDeviceRunning())
            {
                return;
            }

            Task smifTask = new Task(new Action(ReadRFID));
            smifTask.Start();
        }

        private void btn_OpenLaserWindow_Click(object sender, EventArgs e)
        {
            if(mainForm.m_LaserMainWindow == null)
            {
                string info = "mainForm.m_LaserMainWindow == null";
                MessageBox.Show(info, "系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mainForm.m_LaserMainWindow.Show();
        }

        private void btn_CloseLaserWindow_Click(object sender, EventArgs e)
        {
            if (mainForm.m_LaserMainWindow == null)
            {
                string info = "mainForm.m_LaserMainWindow == null";
                MessageBox.Show(info, "系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mainForm.m_LaserMainWindow.Hide();
        }
    }
}
