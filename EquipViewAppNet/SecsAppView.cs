using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SecsEquip;

using System.IO;
using System.Threading;

namespace EquipViewAppNet
{
    public partial class SecsAppView : Form
    {
        public CSecsEquip m_pSecs = new CSecsEquip();

        OnRemoteCommandEventHandler m_remote;//处理交互 先交互 后答复
        OnCommStateEventHandler m_commState;//通讯状态
        OnControlStateEventHandler m_controlState;//控制状态
        OnTerminalEventHandler m_OnterminalInfo;//提示信息

        public OnRemoteCommandExEventHandler m_OnRemoteExCommand;//主动答复 灵活控制


        public OnPPFormatHandleEvent m_pOnPPFormatValueProc = null;      

        public Recipe m_pReceip = new Recipe();

        //回调处理数据
        public event EventHandler SetMarkFormCommuStateHandler;
        public event EventHandler SetMarkFormControlStateHandler;

        public event EventHandler SetMarkFormLotInfoHandler;
        public event EventHandler SetMarkFormSnInfoHandler;

        public event EventHandler SetMarkFormModuleEmpInfoHandler;
        public event EventHandler SetMarkFormSystemEmpInfoHandler;        

        public event EventHandler RunRemoteCommandHandler;//remote command       
        public event EventHandler SwitchReceipHandler;
        public event EventHandler SetProducHandler;
        public event EventHandler SetMarkContentHandler;
        public event EventHandler SetTestPowerHandler;

        public event EventHandler ShowEAPInfoHandler;

        public event EventHandler ReadRecipParamHandler;
        public event EventHandler ReadRFIDContentHandler;
        public event EventHandler LoadFoupHandler;

        public delegate void WarningFormHandler(string msg);
        public event WarningFormHandler WarningHandler;

        public int m_nCommState  = 0; //eap连接状态 0 disable 1 :not connect 2 connecting 
        public int m_nControlState = 0;// 0 offline  1 local online  2 remote online

        bool m_bRequestData = false;//请求数据锁 true  开始请求，false时，返回请求数据

        string m_strRecipeDir="D:\\";
       
        string m_strSystemEmpDir="D:\\";
       
        string m_strModuleEmpDir="D:\\";

        List<string> strList = new List<string>();

        float[] factorCtrl;
        public SecsAppView()
        {
            InitializeComponent();

            m_remote = OnRemoteCommandEventShow;
            m_commState = OnCommStateEventShow;
            m_controlState = OnControlStateEventShow;
            m_OnterminalInfo = OnTerminalInfoShow;

            m_OnRemoteExCommand = OnRemoteCommandExShow;

            m_pOnPPFormatValueProc = OnPPFormatValueShow;
            // 回调
            //m_pSecs.m_pOnRemoteCommand = OnRemoteCommandEventHandler;

            m_pSecs.m_pOnCommState = OnCommStateEventHandler;
            m_pSecs.m_pOnControlState = OnControlStateEventHandler;
            m_pSecs.m_pOnRemoteExCommand = OnRemoteCommandExEventHandler;

            m_pSecs.m_pOnPPFormatValueProc = OnPPFormatValueProcHandler;//s7f25 上传数据回调处理
            //m_pSecs.m_pOnPPFormatChangeProc = OnPPFormatChangeHandleEvent;//s7f26 下载数据回调处理

           
            InitView();

            ReadIni();

            // SECS
            //InitSecs();
        }

        public void WriteIni()
        {
            string fileName = Environment.CurrentDirectory + "\\SECSEAP.ini";

            string strText = textBoxIP.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "IPAddress", strText, fileName);

            strText = textBoxPort.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "IPPort", strText, fileName);

            strText = textBoxSoft.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "SoftNum", strText, fileName);

            strText = textBoxDevice.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "DeviceID", strText, fileName);

            strText = textBoxRecipeDir.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "Directory", strText, fileName);

            strText = textBoxRarDir.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "RarDirectory", strText, fileName);

            strText = textBoxSystemEmpDir.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "SystemEmpDirectory", strText, fileName);

            strText = textBoxModuleEmpDir.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "ModuleEmpDirectory", strText, fileName);

            strText = textBoxT1.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T1", strText, fileName);

            strText = textBoxT2.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T2", strText, fileName);

            strText = textBoxT3.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T3", strText, fileName);

            strText = textBoxT4.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T4", strText, fileName);

            strText = textBoxT5.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T5", strText, fileName);

            strText = textBoxT6.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T6", strText, fileName);

            strText = textBoxT7.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "T7", strText, fileName);


            string strValue = "0";            
            if (radioButtonOffline.Checked )
            {
                strValue  = "0";
            }
            else if (radioButtonLocal.Checked)
            {
                strValue = "1";
            }
            else
            {
                strValue = "2";
            }
            strText = strValue;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "CommuType", strText, fileName);

            
            strValue = "0";
            if (checkBoxEnableComm.Checked)
            {
                strValue = "1";
            }
            else
            {
                strValue = "0";
            }
            strText = strValue;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "CommuEnable", strText, fileName);

            strValue = "0";
            if (checkBoxReceipDir.Checked)
            {
                strValue = "1";
            }
            else
            {
                strValue = "0";
            }
            strText = strValue;
            MyCommanLib.CommonDefine.WritePrivateProfileString("SECSIni", "checkBoxReceipDir", strText, fileName);

        }

        public void ReadIni( bool bConnect = false )
        {
            string fileName = Environment.CurrentDirectory + "\\SECSEAP.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            StringBuilder temp = new StringBuilder(255);

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "IPAddress", "192.168.1.5", temp, 255, fileName);
            textBoxIP.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "IPPort", "6000", temp, 255, fileName);
            textBoxPort.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "SoftNum", "JING_DMS", temp, 255, fileName);
            textBoxSoft.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "DeviceID", "0", temp, 255, fileName);
            textBoxDevice.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "Directory", "D:\\", temp, 255, fileName);
            textBoxRecipeDir.Text = temp.ToString();
            m_strRecipeDir = textBoxRecipeDir.Text;

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "RarDirectory", "D:\\", temp, 255, fileName);
            textBoxRarDir.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "SystemEmpDirectory", "D:\\", temp, 255, fileName);
            textBoxSystemEmpDir.Text = temp.ToString();
            m_strSystemEmpDir = textBoxSystemEmpDir.Text;

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "ModuleEmpDirectory", "D:\\", temp, 255, fileName);
            textBoxModuleEmpDir.Text = temp.ToString();
            m_strModuleEmpDir = textBoxModuleEmpDir.Text;

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T1", "500", temp, 255, fileName);
            textBoxT1.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T2", "10000", temp, 255, fileName);
            textBoxT2.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T3", "45000", temp, 255, fileName);
            textBoxT3.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T4", "45000", temp, 255, fileName);
            textBoxT4.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T5", "10000", temp, 255, fileName);
            textBoxT5.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T6", "5000", temp, 255, fileName);
            textBoxT6.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "T7", "10000", temp, 255, fileName);
            textBoxT7.Text = temp.ToString();
            

            string strValue = "";
            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "CommuType", "0", temp, 255, fileName);
            strValue = temp.ToString();

            if( bConnect )
            {
                if (strValue == "0")
                {
                    radioButtonOffline.Checked = true;
                }
                else if (strValue == "1")
                {
                    radioButtonLocal.Checked = true;
                }
                else
                {
                    radioButtonRemote.Checked = true;
                }
            }


            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "CommuEnable", "0", temp, 255, fileName);
            strValue = temp.ToString();

            if( bConnect )
            {
                if (strValue == "0")
                {
                    checkBoxEnableComm.Checked = false;
                }
                else
                {
                    checkBoxEnableComm.Checked = true;
                }
            }


            MyCommanLib.CommonDefine.GetPrivateProfileString("SECSIni", "checkBoxReceipDir", "0", temp, 255, fileName);
            strValue = temp.ToString();

            if( bConnect )
            {
                if (strValue == "0")
                {
                    checkBoxReceipDir.Checked = false;
                }
                else
                {
                    checkBoxReceipDir.Checked = true;
                }
            }
            
       
        }

        void InitView()
        {
            listViewParam.Columns.Add("名称", 100);
            listViewParam.Columns.Add("值", 100);
            listViewParam.View = View.Details;
            listViewParam.GridLines = true;

            labelCommState.Text = CSecsAssist.GetCommunicationText(COMM_STATE.eCOMM_ENABLED_NOT_COMMUNICATING);
            labelControlState.Text = CSecsAssist.GetControlText(CONTROL_STATE.eCONTROL_OFFLINE_HOST);
        }


        // SECS
        public Boolean InitSecs()
        {
            Boolean bRet = true;
            RcResult rc = m_pSecs.start();
            if (rc.rc != 0)
            {
                //MessageBox.Show("连接SECS错误:" + rc.ToString());
                bRet = false;
                return bRet;
            }

            ReadIni(true);

            string IP = textBoxIP.Text;
            long nPort = int.Parse(textBoxPort.Text);
            bool bPassive = true;
            int nDeviceID = int.Parse(textBoxDevice.Text);
            string direct = textBoxRecipeDir.Text;

            string rarDir = textBoxRarDir.Text;

            m_pSecs.SetIP(textBoxIP.Text);
            m_pSecs.SetPort(int.Parse(textBoxPort.Text));
            m_pSecs.SetPassive(bPassive);
            m_pSecs.SetDeviceID(nDeviceID);

            m_pSecs.SetSOFTREV(textBoxSoft.Text);
            m_pSecs.SetT1(int.Parse(textBoxT1.Text));
            m_pSecs.SetT2(int.Parse(textBoxT2.Text));
            m_pSecs.SetT3(int.Parse(textBoxT3.Text));
            m_pSecs.SetT4(int.Parse(textBoxT4.Text));
            m_pSecs.SetT5(int.Parse(textBoxT5.Text));
            m_pSecs.SetT6(int.Parse(textBoxT6.Text));
            m_pSecs.SetT7(int.Parse(textBoxT7.Text));

            m_pSecs.LoadDataByCsvFile();
            m_pSecs.SetRecipeDirectory(direct);
            m_pSecs.SetWinrarPath(rarDir);

            m_pSecs.SetEnableLog(true);

            if (radioButtonOffline.Checked)
            {
                m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_OFFLINE);
            }
            else if (radioButtonLocal.Checked)
            {
                m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_ONLINE_LOCAL);
            }
            else if (radioButtonRemote.Checked)
            {
                m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_ONLINE_REMOTE);
            }

            

            if (checkBoxEnableComm.Checked)
            {
                m_pSecs.CommEnable();
            }
            else
            {
                m_pSecs.CommDisable();
            }

            return bRet;
        }

        public void EndSecs()
        {
            if( m_pSecs != null )
            {
                m_pSecs.Abort();
            }
        }
         
        // 启用/禁用通讯
        private void checkBoxEnableComm_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxEnableComm.Checked)
                {
                    m_pSecs.CommEnable();
                }
                else
                {
                    m_pSecs.CommDisable();
                }
            }
            catch (Exception)
            {

            }
            
        }

        // 在线模式控制
        private void radioButtonControl_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonOffline.Checked)
                {
                    m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_OFFLINE);
                }
                else if (radioButtonLocal.Checked)
                {
                    m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_ONLINE_LOCAL);
                }
                else if (radioButtonRemote.Checked)
                {
                    m_pSecs.SetControlMode(CONTROL_MODE.eCONTROL_MODE_ONLINE_REMOTE);
                }
            }
            catch (Exception)
            {
            }
            
        }
                
        #region   设置值、设置事件、设置报警 函数

        /// <summary>
        /// 设置变量内容
        /// </summary>
        /// <param name="nID">ID</param>
        /// <param name="value">内容</param>
        public void SetValue(long nID, string value)
        {
            m_pSecs.VariableSet(nID, value);
        }

        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="nID">ID</param>
        public void  SetEventPos(long nID)            
        {
            m_pSecs.EventPos(nID);
        }

        /// <summary>
        /// 设置报警
        /// </summary>
        /// <param name="nID">ID</param>
        /// <param name="result">1代表报警，0代表解除，设置报警后，一定解除后再工作</param>
        public void AlarmSet(int nID , bool result )
        {
            m_pSecs.AlarmSet(nID, result);
        }

        #endregion

        // 设置值
        void buttonVarSet_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxVarName.Text.ToString());
            string pValue = textBoxVarValue.Text;

            m_pSecs.VariableSet(nID, pValue);            
        }

        // 事件发送
        void buttonEventSend_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxEventID.Text.ToString());
       
            m_pSecs.EventPos(nID);
        }

        // 警报set
        void buttonAlarmSet_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxAlarmID.Text.ToString());
            
            RcResult rc = m_pSecs.AlarmSet(nID, true);
        }

        // 警报clear
        void buttonAlarmClear_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxAlarmID.Text.ToString());

            m_pSecs.AlarmSet(nID, false);
        }

        public void RunRemoteCommand (object sender, RemoteCommandHandlerEventArgs e)
        {
            RunRemoteCommandHandler(sender,  e);            
        }


        //// Format 下载
        //public void OnPPFormatChangeHandleEvent(object sender, PPFormatEventArgs e)
        //{
        //    // 新建或者打开已有的一个recipe        
        //    if (e.nHandleType == PPFormatHandle.SetPpidVlaueBegin)
        //    {
        //        m_pReceip.initialData();
        //    }
        //    // 向打开的recipe写入数据          
        //    else if (e.nHandleType == PPFormatHandle.SetPpidVlaue)
        //    {
        //        //List<string> listValue = e.listParamValue;
        //        //switch (e.pCmdCode)
        //        //{
        //        //    case "1":
        //        //        {
        //        //            // 如果只有一个数据
        //        //            if (listValue.Count == 1)
        //        //            {
        //        //                m_pTmp.param1 = listValue[0];

        //        //            }
        //        //        }
        //        //        break;

        //        //    case "2":
        //        //        // 如果有2个数据
        //        //        if (listValue.Count == 2)
        //        //        {
        //        //            m_pTmp.param2 = listValue[0];
        //        //            m_pTmp.param2 = listValue[1];
        //        //        }
        //        //        break;
        //        //}
        //    }
        //    // 保存recipe      
        //    else if (e.nHandleType == PPFormatHandle.SetPpidVlaueFinsh)
        //    {

        //    }
        //}


        //上传 Receip参数  S7F25
        void OnPPFormatValueProcHandler(object sender, PPFormatEventArgs e)
        {           
            if (e.nHandleType == PPFormatHandle.LoadPpid)
            {
                // 加载PP到临时对象中
                string ppid = e.ppid;
                //m_pReceip = new Recipe();

                //MessageBox.Show("开始加载recipe参数");

                m_pReceip.initialData();
                //ppid = "ppid.ezd";
                ReadRecipParamHandler(ppid, e);
                //需要阻塞下

                while (m_pReceip.nLoadType <= 0)
                {                    
                    Thread.Sleep(100);
                }
                
            }
            else if (e.nHandleType == PPFormatHandle.GetPpidValue)
            {               

                if (m_pReceip == null)
                    return;


                List<string> listValue = new List<string>();
                                            
                string pValue = "";

                int nID = int.Parse(e.pCmdCode);
                if( nID < 4 )
                {
                    switch (nID)
                    {
                        case 1: pValue = m_pReceip.RecipeName; break;
                        case 2: pValue = m_pReceip.PowerRange; break;
                        case 3: pValue = m_pReceip.MarkStringType; break;
                        default: pValue = "-1"; break;
                    }
                }else
                {
                    int nIndex = (nID - 4)/7;
                    int nParaID = (nID - 4) % 7;
                    if( nIndex < 10 && nParaID < 7 )
                    {
                        switch (nParaID)
                        {
                            case 0: pValue = m_pReceip.markParams[nIndex].PenNO; break;
                            case 1: pValue = m_pReceip.markParams[nIndex].MarkTimes; break;
                            case 2: pValue = m_pReceip.markParams[nIndex].MarkSpeed; break;
                            case 3: pValue = m_pReceip.markParams[nIndex].MarkFreq; break;
                            case 4: pValue = m_pReceip.markParams[nIndex].MarkPulse; break;
                            case 5: pValue = m_pReceip.markParams[nIndex].StartTC; break;
                            case 6: pValue = m_pReceip.markParams[nIndex].LaserOffTC; break;
                            default: pValue = "-1"; break;
                        }
                    }                                        
                }

                listValue.Add(pValue);
                e.listParamValue = listValue;
                
            }
        }



        // 远程控制
        REMOTE_REPLY OnRemoteCommandEventHandler(object sender, RemoteCommandHandlerEventArgs e)
        {
            switch (e.pCMD)
            {
                case "START":                    
                case "STOP":
                case "Recipe":
                    // 调用作业命令

                    RunRemoteCommand(e.pCMD, e);
                    break;
                case "PP-SELECT":
                case "SET-PRODUC-PARA":
                case "SET-PRODUC-BATCH":
                    {
                        REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);
                        if( rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                        {
                            SetProducHandler(strList, e);
                        }
                        return rReply;
                    }                
                // 命令不存在
                default:
                    return REMOTE_REPLY.eCOMMAND_NOT_EXIST;
            }
          
            return REMOTE_REPLY.eCOMMAND_PERFORMED;
        }

       
        void OnRemoteCommandExEventHandler(object sender, RemoteCommandHandlerEventArgs e)
        {
            if (e.pCMD == "Cancel")
            {
                MyCommanLib.CommonDefine.bAccept = true;
            }

            switch (e.pCMD)
            {
                case "START":
                case "STOP":
                case "Recipe":
                case "Cancel":                    
                    {
                        try
                        {
                            // 调用作业命令
                            RunRemoteCommand(e.pCMD, e);

                            REMOTE_REPLY rReply = REMOTE_REPLY.eCOMMAND_PERFORMED;
                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);

                            if(e.pCMD == "START")
                            {
                                Thread.Sleep(500);
                                //ProcessStart Event
                                SetEventPos(21002);
                            }

                            if(e.pCMD == "STOP")
                            {
                                SetEventPos(21022);
                            }
                            
                        }
                        catch (Exception ex )
                        {
                            MessageBox.Show($"EAP 相应异常--{ex.Message}");

                        }                       
                        
                    }                    
                    break;
                case "PP-SELECT":
                    {
                        try
                        {
                            REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);

                            if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                            {
                                SwitchReceipHandler(strList, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"EAP 相应异常--{ex.Message}");
                        }
                        
                    }
                    break;
                case "MarkContent":
                    {
                        try
                        {
                            REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);

                            if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                            {
                                SetMarkContentHandler(strList, e);
                            }
                            else
                            {
                                WarningHandler("EAP---下发打标数据异常请确认!");
                            }
                        }
                        catch (Exception ex )
                        {
                            MessageBox.Show($"EAP 相应异常--{ex.Message}");
                        }
                        
                    }
                    break;
                case "TestPowerResult":
                    {
                        try
                        {
                            REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);
                            if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                            {
                                SetTestPowerHandler(strList, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"EAP 相应异常--{ex.Message}");
                        }
                        
                    }
                    break;
                case "SET-PRODUC-PARA":
                case "SET-PRODUC-BATCH":
                    {
                        try
                        {
                            REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);
                            if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                            {
                                SetProducHandler(strList, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"EAP 相应异常--{ex.Message}");
                        }
                        
                    }
                    break;
                case "ReadRFID":
                    try
                    {
                        REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                        m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);

                        if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                        {
                            ReadRFIDContentHandler(strList, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"EAP 相应异常--{ex.Message}");
                    }

                    break;
                case "LoadFoup":
                    try
                    {
                        if (!MyCommanLib.CommonDefine.bAccept)
                        {
                            REMOTE_REPLY rReply1 = REMOTE_REPLY.eCOMMAND_CANNOT_PERFORM_NOW;
                            m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply1);
                            return;
                        }

                        REMOTE_REPLY rReply = HandleSelectDataExample(e.pCMD, e);

                        m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);

                        if (rReply == REMOTE_REPLY.eCOMMAND_PERFORMED)
                        {
                            LoadFoupHandler(strList, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"EAP 相应异常--{ex.Message}");
                    }
                    break;
                // 命令不存在
                default:
                    {
                        REMOTE_REPLY rReply = REMOTE_REPLY.eCOMMAND_NOT_EXIST;

                        m_pSecs.RemoteCmdReply(e.pReplyMailbox, rReply);
                    }
                    break;
            }
        }
        // PP-SLECT例子
        REMOTE_REPLY HandleSelectDataExample(string strName,RemoteCommandHandlerEventArgs e)
        {
            try
            {
                if (strName == "PP-SELECT")
                {
                    strList.Clear();
                    string pRecipeName = e.mapParament["RecipeName"];   // 提取名称

                    strList.Add(pRecipeName);

                    string strBoxIndex = "";
                    try
                    {
                        strBoxIndex = e.mapParament["BoxIndex"];
                        strList.Add(strBoxIndex);
                    }
                    catch (Exception)
                    {
                    }


                } else if(strName == "MarkContent")
                {
                    strList.Clear();
                    string mapContent = e.mapParament["MapContent"];   // 提取名称

                    strList.Add(mapContent);

                    string markContent = e.mapParament["MarkContent"];
                    strList.Add(markContent);

                    string boxIndex = e.mapParament["BoxIndex"];
                    strList.Add(boxIndex);

                    string[] markContents = markContent.Split(',');
                    
                    if(markContents.Length < 25)
                    {
                        return REMOTE_REPLY.eCOMMAND_PARAMETER_IS_INVALID;
                    }

                    for(int i = 0; i < 25; i++)
                    {
                        if(MyCommanLib.CommonDefine.m_SecsData.strCurrentMapContent[i] == '0')
                        {
                            if(mapContent[i] == '1')
                            {
                                return REMOTE_REPLY.eCOMMAND_PARAMETER_IS_INVALID;
                            }
                        }

                        if(mapContent[i] == '1')
                        {
                            if (markContents[i].Length <= 0)
                            {
                                return REMOTE_REPLY.eCOMMAND_PARAMETER_IS_INVALID;
                            }
                        }
                    }
                    
                }
                else if (strName == "SET-PRODUC-PARA")
                {
                    strList.Clear();

                    string pProducID = e.mapParament["ProducID"];   // 提取名称       
                    strList.Add(pProducID);

                    string pProducCount = e.mapParament["ProducCount"];   // 提取名称
                    strList.Add(pProducCount);

                    string pProductBatch = e.mapParament["ProducBatch"];   // 提取名称
                    strList.Add(pProductBatch);

                    string pReceipName = e.mapParament["RecipeName"]; // Receip
                    strList.Add(pReceipName);                                        
                } else if( strName == "TestPowerResult" )
                {
                    strList.Clear();

                    string pProducID = e.mapParament["TestPowerResult"];   // 提取名称       
                    strList.Add(pProducID);
                    
                }
                else if(strName == "ReadRFID")
                {
                    strList.Clear();

                    string pFoupID = e.mapParament["BoxIndex"];
                    strList.Add(pFoupID);
                }
                else if(strName == "LoadFoup")
                {
                    strList.Clear();

                    string pFoupID = e.mapParament["BoxIndex"];
                    strList.Add(pFoupID);
                }
                
            }
            catch (System.Exception)
            {
                // 不存在Recipe存在，Host没有发送过来
                return REMOTE_REPLY.eCOMMAND_PARAMETER_IS_INVALID;
            }

            return REMOTE_REPLY.eCOMMAND_PERFORMED;
        }

        public void OnPPFormatValueShow( object sender, PPFormatEventArgs e )
        {
            if (this.IsHandleCreated)
            {
                BeginInvoke(m_pOnPPFormatValueProc, sender, e);
            }
        }

        public void OnRemoteCommandExShow(object sender, RemoteCommandHandlerEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                BeginInvoke(m_OnRemoteExCommand, sender, e);
            }
        }

        // 远程控制命令显示到软件，用于观察demo
        void RemoteCommandEventShow(object sender, RemoteCommandHandlerEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                BeginInvoke(m_remote, sender, e);
            }
        }

        //public void RemoteCmdReply(string pReplyMailbox, REMOTE_REPLY nHCACK, Dictionary<string,string> mapParam )
        //{

        //}

        // 远程控制
        REMOTE_REPLY OnRemoteCommandEventShow(object sender, RemoteCommandHandlerEventArgs e)
        {            
            textBoxRemoteCmd.Text = e.pCMD;         

            listViewParam.Items.Clear();
            foreach(string key in e.mapParament.Keys)            
            {
                try
                {
                    listViewParam.BeginUpdate();

                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = key.ToString();
                    lvi.SubItems.Add(e.mapParament[key]);
                    listViewParam.Items.Add(lvi);

                    listViewParam.EndUpdate();
                }
                catch 
                {
                	
                }
                
            }
            return REMOTE_REPLY.eCOMMAND_PERFORMED; ;
        }

        // 选择目录
        private void buttonSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string pDir = dialog.SelectedPath;
                if (pDir != null)
                {
                    m_pSecs.SetRecipeDirectory(pDir);
                    textBoxRecipeDir.Text = pDir;
                    m_strRecipeDir = pDir;
                }
            }
        }        


        // 状态
        [Description("控制状态改变")]
        void OnCommStateEventHandler(object sender, CommStateEventArgs e)
        {            
            BeginInvoke(m_commState, sender, e);            
            
        }

        public int GetCommState()
        {
            return m_nCommState;
        }

        public int GetControlState()
        {
            return m_nControlState;
        }

        public string GetRecipeDir()
        {
            return m_strRecipeDir;
        }

        public string GetSystemEmpDir()
        {
            return m_strSystemEmpDir;
        }

        public string GetModuleEmpDir()
        {
            return m_strModuleEmpDir;
        }

        void OnCommStateEventShow(object sender, CommStateEventArgs e)
        {
            if (e.nState == COMM_STATE.eCOMM_DISABLED)
                m_nCommState = 0;
            else if (e.nState == COMM_STATE.eCOMM_ENABLED_NOT_COMMUNICATING)
                m_nCommState = 1;
            else
                m_nCommState = 2;

            labelCommState.Text = CSecsAssist.GetCommunicationText(e.nState);

            if( SetMarkFormCommuStateHandler != null  )
            {
                SetMarkFormCommuStateHandler(labelCommState.Text, e);//设置markform控件状态
            }            
        }

        [Description("通讯状态改变")]
        void OnControlStateEventHandler(object sender, ControlStateEventArgs e)
        {
            BeginInvoke(m_controlState, sender, e);
            
        }
        void OnControlStateEventShow(object sender, ControlStateEventArgs e)
        {           
            if (e.nState == CONTROL_STATE.eCONTROL_ONLINE_LOCAL)
                m_nControlState = 1;
            else if (e.nState == CONTROL_STATE.eCONTROL_ONLINE_REMOTE)
            {
                m_nControlState = 2;                
            }                
            else
                m_nControlState = 0;

            labelControlState.Text = CSecsAssist.GetControlText(e.nState);

            if(SetMarkFormControlStateHandler != null )
            {
                SetMarkFormControlStateHandler(labelControlState.Text, e);//设置markform控件状态
            }
            
        }

        //提示信息
        TERMINAL_REPLY OnTerminalInfoShow(object sender, TerminalEventArgs e)
        {
            //labelControlState.Text = e.pText;
            //MessageBox.Show(e.pText);

            ShowEAPInfoHandler(e.pText, e);
            return 0;
        }
            

        private void button1_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxEventID.Text.ToString());

            m_pSecs.EventDiscreteVarPos(nID);
        }

        private void SecsAppView_SizeChanged(object sender, EventArgs e)
        {
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            string IP = textBoxIP.Text;
            long nPort = int.Parse(textBoxPort.Text);
            bool bPassive = true;
            int nDeviceID = int.Parse(textBoxDevice.Text);
            string direct = textBoxRecipeDir.Text;

            m_pSecs.SetIP(textBoxIP.Text);
            m_pSecs.SetPort(int.Parse(textBoxPort.Text));
            m_pSecs.SetPassive(bPassive);
            m_pSecs.SetDeviceID(nDeviceID);

            m_pSecs.SetSOFTREV(textBoxSoft.Text);
            m_pSecs.SetT1(int.Parse(textBoxT1.Text));
            m_pSecs.SetT2(int.Parse(textBoxT2.Text));
            m_pSecs.SetT3(int.Parse(textBoxT3.Text));
            m_pSecs.SetT4(int.Parse(textBoxT4.Text));
            m_pSecs.SetT5(int.Parse(textBoxT5.Text));
            m_pSecs.SetT6(int.Parse(textBoxT6.Text));
            m_pSecs.SetT7(int.Parse(textBoxT7.Text));

            m_pSecs.LoadDataByCsvFile();
            m_pSecs.SetRecipeDirectory(direct);

            bool bCheck = checkBoxReceipDir.Checked;
            m_pSecs.PPIsFolderDir(bCheck);

            // 下面这个是文件夹的前提
            m_pSecs.SetWinrarPath(textBoxRarDir.Text);       
            

            WriteIni();

        }

        public void SetAllowHostDownload( bool bCheck )
        {
            m_pSecs.PPAllowHostDowmload(bCheck);
        }   
        

        private void buttonMark_Click(object sender, EventArgs e)
        {

        }

        private void buttonMN_Click(object sender, EventArgs e)
        {
            int nID = StringToNum.atoi(textBoxEventID.Text.ToString());
            m_pSecs.EventPos(nID);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // 结果构建 demo
            List<List<string>> listResult = new List<List<string>>();
            List<string> aa = new List<string>();
            aa.Add("A1");
            aa.Add("A2");
            listResult.Add(aa);

            List<string> bb = new List<string>();
            bb.Add("B1");
            bb.Add("B2");
            bb.Add("B3");
            listResult.Add(bb);

            List<string> cc = new List<string>();
            cc.Add("C1");
            listResult.Add(cc);

            string pTmp = MakeCheckResult(listResult);

            m_pSecs.VariableSet(1500, pTmp);

        }

        // 参数的类型
        // listParament参数的列表
        string RecipeJoin(List<string> listParament, string pDataFormat)
        {
            string pTmp;
            List<string> listSML = new List<string>();
            listSML.Add("L");

            for (int i = 0; i < listParament.Count; i++)
            {
                pTmp = CListOperate.listJoin(pDataFormat, listParament[i]);
                listSML.Add(pTmp);
            }

            pTmp = CListOperate.listJoin(listSML);
            return pTmp;
        }

        // 构建数据的代码，返回值是用于设置的变量
        public string MakeCheckResult(List<List<string>> listResult)
        {
            string pTmp;
            string[] listDataX = new string[listResult.Count + 1];
            listDataX[0] = "L";
            for (int i = 0; i < listResult.Count; i++)
            {
                string[] listDataY = new string[listResult[i].Count + 1];
                listDataY[0] = "L";
                for (int j = 0; j < listResult[i].Count; j++)
                {
                    pTmp = CListOperate.listJoin("A", listResult[i][j]);
                    listDataY[j + 1] = pTmp;
                }
                pTmp = CListOperate.listJoin(listDataY);
                listDataX[i + 1] = pTmp;
            }
            pTmp = CListOperate.listJoin(listDataX);
            return pTmp;

        }

        //构建数据的代码，将list转换为字符串，同一记录用逗号分开；不用记录之间用；分开
        public string MakeMarkResult(List<string> listResult)
        {
            List<string> oneList;
            string pTmp = "";

            for (int i = 0; i < listResult.Count; i++)
            {
                pTmp = pTmp + listResult[i] + ";";

            }
            return pTmp;

        }

        private void buttonRar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(textBoxRarDir.Text);// Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.Filter = "Ezd files (*.exe)|*.exe";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxRarDir.Text = dlg.FileName;
                //m_pSecs.SetWinrarPath(textBoxRarDir.Text);
            }
        }

        public void SetLotPath(string pathName)
        {
            textBoxLotDir.Text = pathName;
            EventArgs e = new EventArgs();
            SetMarkFormLotInfoHandler(pathName, e);
        }

        public void SetSnPath(string pathName)
        {
            textBoxSnDir.Text = pathName;

            EventArgs e = new EventArgs();
            SetMarkFormSnInfoHandler(pathName, e);
        }

        public void SetSystemEmpPath(string pathName)
        {
            textBoxSystemEmpDir.Text = pathName;            
        }

        public void SetModuleEmpPath(string pathName)
        {
            textBoxModuleEmpDir.Text = pathName;
        }

        private void buttonLot_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog path = new FolderBrowserDialog();

            if (path.ShowDialog() == DialogResult.OK)
            {
                string pDir = path.SelectedPath;
                if (pDir != null)
                {
                    SetMarkFormLotInfoHandler(path.SelectedPath, e);

                    textBoxLotDir.Text = path.SelectedPath;
                }
            }
            
        }

        private void buttonSn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();

            if (path.ShowDialog() == DialogResult.OK)
            {
                string pDir = path.SelectedPath;
                if (pDir != null)
                {
                    SetMarkFormSnInfoHandler(path.SelectedPath, e);
                    textBoxSnDir.Text = path.SelectedPath;
                }
            }

        }

        private void buttonSystemEmp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();

            if (path.ShowDialog() == DialogResult.OK)
            {
                string pDir = path.SelectedPath;
                if (pDir != null)
                {
                    SetMarkFormSystemEmpInfoHandler(path.SelectedPath, e);
                    textBoxSystemEmpDir.Text = path.SelectedPath;
                    m_strSystemEmpDir = path.SelectedPath;
                }
            }
            
        }

        private void buttonModuleEmp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();

            if (path.ShowDialog() == DialogResult.OK)
            {
                string pDir = path.SelectedPath;
                if (pDir != null)
                {
                    SetMarkFormModuleEmpInfoHandler(path.SelectedPath, e);
                    textBoxModuleEmpDir.Text = path.SelectedPath;
                    m_strModuleEmpDir = path.SelectedPath;
                }
            }            
        }

        private void SecsAppView_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void SecsAppView_Load(object sender, EventArgs e)
        {

        }
    }

    public class MarkParam
    {
        public string PenNO = "";
        public string MarkTimes = "";
        public string MarkSpeed = "";

        public string MarkFreq = "";
        public string MarkPulse = "";

        public string StartTC = "";
        public string LaserOffTC = "";

        public MarkParam()
        {
            initialData();
        }

        public void initialData()
        {
            PenNO = "";
            MarkTimes = "";
            MarkSpeed = "";

            MarkFreq = "";
            MarkPulse = "";

            StartTC = "";
            LaserOffTC = "";
        }
    }

    public class Recipe
    {
        public string RecipeName    = "";
        public string PowerRange    = "";
        public string MarkStringType = "";

        public MarkParam[] markParams = new MarkParam[10];
        
        
        public int nLoadType = 0;// 0:没有load结束， 1：load 成功  2：load失败

        public Recipe()
        {
            crateData();
        }

        public void crateData()
        {
            RecipeName = "";
            PowerRange = "";
            MarkStringType = "";

            for (int i = 0; i < 10; i++)
            {
                markParams[i] = new MarkParam();
            }
            nLoadType = 0;

        }

        public void initialData()
        {
            RecipeName = "";
            PowerRange = "";
            MarkStringType = "";

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    markParams[i].initialData();
                }
            }
            catch
            {

            }
            

            nLoadType = 0;

        }
    }
}
