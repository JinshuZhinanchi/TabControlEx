using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using gts;

namespace jog
{
    public partial class AxisTestForm : Form
    {
        public GtsControl m_gtsControl;// = new GtsControl();

        public const short BOX_4INCH = 0;
        public const short BOX_6INCH = 1;
        //box chips count
        public const short BOX_COUNT = 2;
        public const short BOX_CHIPSCOUNT = 25;

        //box chip state
        public const short BOXCHIP_WAITSTATE = 0;
        public const short BOXCHIP_RUNNINGSTATE = 1;
        public const short BOXCHIP_RUNSTATE = 2;
        public const short BOXCHIP_NOTSCANSTATE = 3;
        public const short BOXCHIP_FAILSTATE = 4;
        public const short BOXCHIP_SKIPSTATE = 5;
        public const short BOXCHIP_FINISHSTATE = 6;

        public const short EAP_SCAN_VALUE = 8;

        public static int[] Axis_InUse = { 0, 0, 1, 0 };
        //axis pulse equivalent   x 10000/5  10000 / 10   35* 10000 / 360 
        public static readonly double[] AxisPulseEquivalent = { 2000.0, 1000.0, 972.222222 };
        public static readonly double[] AxisPulseSpeedEquivalent = { 2.0, 1.0, 0.9722222 };//ms ---> s  /1000
        public static readonly double[] AxisSafePos = { 50, 10, 1.0 };


        //work flow

        //Robot flow
        public const short SCAN_VALUE = 0;// 1
        public const short ROBOT_TAKE = 1;// 2

        public const short START_PHOTO = 2;
        public const short END_PHOTO = 3;
        public const short START_MARK = 4;
        public const short END_MARK = 5;
        public const short PUT_CHIP = 6;

        public const short READ_RFID = 9;

        public const short PHOTO_VALUE = 2;
        public const short MARK_VALUE = 3;


        public const short PUT_VALUE = 4;
        public const short CHECK_VALUE = 5;

        public const short PUT_SIMF = 6;
        public const short PUT_SIMF_FAIL = 7;


        //双头寻边拍照类型定义
        public const short CCD_FACE_POSITION = 1;
        public const short CCD_FACE_CHECK = 2;
        public const short CCD_SIDE_POSITION = 3;
        public const short CCD_SIDE_CHECK = 4;

        public const short CCD_WAFER_NOTCH = 5;   //找V口角度
        public const short CCD_WAFER_POSITION = 6;


        //料盒取出---放到Wafer---寻边---寻边定位---V口定位---打标---平台取片---放回Smif 
        public const short ROBOT_PUT_MARK = 10;    //放到Wafer      3

        public const short WAFER_NOTCH = 11; // Wafer 找V口      
        public const short WAFER_POSITION = 12; // Wafer 定位       
        public const short PHOTO_VALUE1 = 13; // V口定位          
        public const short PHOTO_VALUE2 = 14; //侧面定位            
        public const short MARK_VALUEALL = 15; //正侧面打标        
        public const short MARK_VALUE1 = 15; //正面打标         
        public const short MARK_VALUE2 = 16; //侧面打标
        public const short CHECK_VALUEALL = 17;//正反面检测
        public const short CHECK_VALUE1 = 17;//正面检测
        public const short CHECK_VALUE2 = 18;//侧面检测
        public const short WAFER_END = 19;//Wafer 结束

        public const short ROBOT_TAKE_MARK = 20;//从Wafer取片        4

        public const short FINISH_VALUE = 100;
        //end work flow

        //MarkFlow
        //打标线程 流程定义
        public const short MARK_NOTREADY = -1;
        public const short MARK_READY = 0;
        public const short MARK_FLOW_START = 1;
        public const short MARK_ALIGN_TAKEPHOTO = 2;
        public const short MARK_NOTCH_TAKEPHOTO = 3;
        public const short MARK_NOTCH_MARK = 4;
        public const short MARK_NOTCH_CHECK = 5;
        public const short MARK_FINISH = 6;

        public const short SMIF_NOTREADY = -1;
        public const short SMIF_READY = 0;
        public const short OPEN_SMIF = 10;
        public const short CLOSE_SMIF = 11;

        //work state
        public const short WORK_MANUAL = 0;
        public const short WORK_AUTO = 1;
        public const short WORK_WARNING = 2;
        public const short WORK_RESET = 3;
        public const short WORK_Intial = 4;

        //axis define
        public static short STRECH_AXIS = 1;
        public static short UPDN_AXIS = 2;
        public static short ROTATE_AXIS = 3;

        //axis define
        public static short MarkX_AXIS = 1;
        public static short MarkY_AXIS = 2;
        public static short MARKZ_AXIS = 3;
        public static short ROBOT_AXIS = 4;

        //
        //In0 启动按钮
        public static short StartIOIN = 0;
        //In1 停止按钮
        public static short StopIOIN = 1;
        //In1 复位按钮
        public static short ResetIOIN = 2;
        //In3 急停按钮
        public static short EMStopIOIN = 3;

        //In8 真空负压信号（吸料）
        public static short XiLiaoIOIN = 8;
        //In9 有料检测信号
        public static short ScanIOIN = 9;
        //In10 翻转气缸原点
        public static short RevertGasIOIN1 = 10;
        //In11 翻转气缸动点
        public static short RevertGasIOIN2 = 11;


        //In100 料盒确认按钮1
        public static short BoxReadyExIOIN1 = 0;
        //In101 料盒确认按钮2
        public static short BoxReadyExIOIN2 = 1;
        //In102 料盒确认按钮3
        public static short BoxReadyExIOIN3 = 2;
        //In103 料盒确认按钮4
        public static short BoxReadyExIOIN4 = 3;
        // In104 料盒确认按钮5
        public static short BoxReadyExIOIN5 = 4;
        //In105 料盒确认按钮6
        public static short BoxReadyExIOIN6 = 5;
        //In106 料盒确认按钮7
        public static short BoxReadyExIOIN7 = 6;
        //In107 料盒确认按钮8
        public static short BoxReadyExIOIN8 = 7;

        //In200 料盒感应开关1_A型号
        public static short BoxExitExIOIN1A = 0;
        //In201 料盒感应开关2_A型号
        public static short BoxExitExIOIN2A = 1;
        //In202 料盒感应开关3_A型号
        public static short BoxExitExIOIN3A = 2;
        //In203 料盒感应开关4_A型号
        public static short BoxExitExIOIN4A = 3;
        //In204 料盒感应开关5_A型号
        public static short BoxExitExIOIN5A = 4;
        //In205 料盒感应开关6_A型号
        public static short BoxExitExIOIN6A = 5;
        //In206 料盒感应开关7_A型号
        public static short BoxExitExIOIN7A = 6;
        //In207 料盒感应开关8_A型号
        public static short BoxExitExIOIN8A = 7;

        //In208 料盒感应开关1_B型号
        public static short BoxExitExIOIN1B = 8;
        //In209 料盒感应开关2_B型号
        public static short BoxExitExIOIN2B = 9;
        //In210 料盒感应开关3_B型号
        public static short BoxExitExIOIN3B = 10;
        //In211 料盒感应开关4_B型号
        public static short BoxExitExIOIN4B = 11;
        //In212 料盒感应开关5_B型号
        public static short BoxExitExIOIN5B = 12;
        //In213 料盒感应开关6_B型号
        public static short BoxExitExIOIN6B = 13;
        //In214 料盒感应开关7_B型号
        public static short BoxExitExIOIN7B = 14;
        //In215 料盒感应开关8_B型号
        public static short BoxExitExIOIN8B = 15;


        //Out0 三色灯黄灯
        public static short YellowLightIOOUT = 0;
        //Out1 三色灯红灯
        public static short RedLightIOOUT = 1;
        //Out2 三色灯绿灯
        public static short GreenLightIOOUT = 2;
        //Out3 蜂鸣器
        public static short BeepIOOUT = 3;
        //Out4    真空吸
        public static short XiLiaoIOOUT = 4;
        //Out5    翻转气缸
        public static short RevertGasIOOUTOrig = 5;
        //Out6
        public static short PumpGasIOOUT = 6;
        //Out7
        public static short RevertGasIOOUTWork = 7;
        //Out8---Out15
        public const short READY_LIGHTSB1 = 8;
        public const short READY_LIGHTSB2 = 9;
        public const short READY_LIGHTSB3 = 10;
        public const short READY_LIGHTSB4 = 11;
        public const short READY_LIGHTSB5 = 12;
        public const short READY_LIGHTSB6 = 13;
        public const short READY_LIGHTSB7 = 14;
        public const short READY_LIGHTSB8 = 15;

        // out end 

        Thread threadWork;
        System.Timers.Timer workTimer;


        private object showInfoObject = new object();

        delegate void SetTextCallback(Control control, string text);
        delegate void SetColorCallback(Control control, Color bkColor);

        short m_nInitial = 0;
        short m_nReturn = 0;
        ushort m_nStatus = 0;

        public short m_nCurrentAxis = 0;

        Boolean m_bAxisEnable = false;

        private int m_nSearchHome = -2000000;

        public AxisTestForm()
        {
            InitializeComponent();
        }

        public static void commandhandler(string command, short error)
        {
            if (error != 0)
            {
                Console.WriteLine("{0}={1}\n", command, error);
            }
        }

        public static Boolean showErrorMessage(string command, short error)
        {
            Boolean bRet = false;
            if (error != 0)
            {
                bRet = true;

                string strMessage = string.Format("{0}={1}", command, error);
                MessageBox.Show(strMessage);
            }
            return bRet;
        }

        public void WriteGtsSettingIni()
        {
            int axis = m_nCurrentAxis;

            string fileName = System.Windows.Forms.Application.StartupPath + "\\gtsSetting.ini";

            string strText = textBoxAxis1Mspeed.Text;
            string keyName = "textBoxAxis1Mspeed";

            //X 轴 0 Z轴 1   R轴 2
            strText = textBoxAxis1Mspeed.Text;
            keyName = string.Format("textBoxAxis1Mspeed{0}", axis);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxAxis1Aspeed.Text;
            keyName = string.Format("textBoxAxis1Aspeed{0}", axis);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxAxis1Acc.Text;
            keyName = string.Format("textBoxAxis1Acc{0}", axis);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxAxis1Stime.Text;
            keyName = string.Format("textBoxAxis1Stime{0}", axis);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxAxis1Step.Text;
            keyName = string.Format("textBoxAxis1Step{0}", axis);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);


        }

        public void ReadGtsSettingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\gtsSetting.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            short axis = m_nCurrentAxis;

            string strText = "";
            string keyName = "gtsSetting";

            StringBuilder temp = new StringBuilder(255);

            keyName = string.Format("textBoxAxis1Mspeed{0}", axis);
            MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();
            textBoxAxis1Mspeed.Text = strText;

            keyName = string.Format("textBoxAxis1Aspeed{0}", axis);
            MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();
            textBoxAxis1Aspeed.Text = strText;


            keyName = string.Format("textBoxAxis1Acc{0}", axis);
            MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();
            textBoxAxis1Acc.Text = strText;

            keyName = string.Format("textBoxAxis1Stime{0}", axis);
            MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();
            textBoxAxis1Stime.Text = strText;


            keyName = string.Format("textBoxAxis1Step{0}", axis);
            MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
            strText = temp.ToString();
            textBoxAxis1Step.Text = strText;

            //set gtscontrol
            //读取数据           
            double vel = Convert.ToDouble(this.textBoxAxis1Aspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            gts.mc.TTrapPrm trapPrm = new gts.mc.TTrapPrm();

            gts.mc.GT_GetTrapPrm(axis, out trapPrm);

            trapPrm.acc = Convert.ToDouble(this.textBoxAxis1Acc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.dec = Convert.ToDouble(this.textBoxAxis1Dec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.smoothTime = (short)(1000 * Convert.ToDouble(this.textBoxAxis1Stime.Text));
            trapPrm.velStart = 0;

            m_gtsControl.SetWorkTrapPrm(axis, trapPrm, vel);


            vel = Convert.ToDouble(this.textBoxAxis1Mspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            gts.mc.TJogPrm jogPrm = new gts.mc.TJogPrm();

            jogPrm.acc = Convert.ToDouble(this.textBoxAxis1MAcc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jogPrm.dec = Convert.ToDouble(this.textBoxAxis1MDec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            m_gtsControl.SetManualJogPrm(axis, jogPrm, vel);

        }
        public void InitialGtsControl()
        {

        }

        internal void SaveDevicePara()
        {
            short axis = m_nCurrentAxis;

            gts.mc.TJogPrm jog;

            gts.mc.GT_GetJogPrm(axis, out jog);

            double vel = Convert.ToDouble(this.textBoxAxis1Mspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.acc = Convert.ToDouble(this.textBoxAxis1MAcc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.dec = Convert.ToDouble(this.textBoxAxis1MDec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            m_gtsControl.SetManualJogPrm(axis, jog, vel);


            vel = Convert.ToDouble(this.textBoxAxis1Aspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            gts.mc.TTrapPrm trapPrm = new gts.mc.TTrapPrm();

            m_nReturn = gts.mc.GT_GetTrapPrm(axis, out trapPrm);

            trapPrm.acc = Convert.ToDouble(this.textBoxAxis1Acc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.dec = Convert.ToDouble(this.textBoxAxis1Dec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.smoothTime = (short)(1000 * Convert.ToDouble(this.textBoxAxis1Stime.Text));
            trapPrm.velStart = 0;

            m_gtsControl.SetWorkTrapPrm(axis, trapPrm, vel);

            WriteGtsSettingIni();
        }

        public void InitialAxis()
        {
            ReadGtsSettingIni();

            //if (m_nCurrentAxis == ROTATE_AXIS)
            //{
            //    groupBoxName.Text = "旋转轴";
            //}
            //else if (m_nCurrentAxis == UPDN_AXIS)
            //{
            //    groupBoxName.Text = "上下轴";
            //}
            //else if (m_nCurrentAxis == STRECH_AXIS)
            //{
            //    groupBoxName.Text = "伸缩轴";
            //}
            if (m_nCurrentAxis == MARKZ_AXIS)
            {
                groupBoxName.Text = $"轴{m_nCurrentAxis} 打标Z轴";
            }
            //else if (m_nCurrentAxis == MarkX_AXIS)
            //{
            //    groupBoxName.Text = $"轴{m_nCurrentAxis} 打标X轴";
            //}
            //else if (m_nCurrentAxis == ROBOT_AXIS)
            //{
            //    groupBoxName.Text = $"轴{m_nCurrentAxis} 机械手托运轴";
            //}

            m_nInitial = 1;


            m_nReturn = gts.mc.GT_ClrSts(m_nCurrentAxis, 8);//清除限位和报警
            string strMessage = string.Format("轴{0}{1}", m_nCurrentAxis, "清除限位和报警失败");
            if (showErrorMessage(strMessage, m_nReturn))
            {
                return;
            }

            //// 轴1伺服使能
            //strMessage = string.Format("轴{0}{1}", nAxis, "伺服使能失败");
            //m_nReturn = GT_AxisOn(nAxis);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //// 位置清零
            //strMessage = string.Format("轴{0}{1}", nAxis, "位置清零失败");
            //m_nReturn = GT_ZeroPos(nAxis,0);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //// AXIS轴规划位置清零
            //strMessage = string.Format("轴{0}{1}", nAxis, "规划位置清零失败");
            //m_nReturn = GT_SetPrfPos(nAxis, 0);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //// 将 AXIS 轴设为点位模式
            //strMessage = string.Format("轴{0}{1}", nAxis, "点位模式失败");
            //m_nReturn = GT_PrfTrap(nAxis);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //// 读取点位运动参数(需要读取所有运动参数到上位机变量)
            //strMessage = string.Format("轴{0}{1}", nAxis, "点位模式失败");
            //m_nReturn = GT_PrfTrap(nAxis);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //TTrapPrm trapPrm = new TTrapPrm();
            //m_nReturn = GT_GetTrapPrm(nAxis, out trapPrm);
            //if (showErrorMessage(strMessage, m_nReturn))
            //{
            //    return;
            //}

            //// 设置需要修改的运动参数            
            //trapPrm.acc = 0.25;
            //trapPrm.dec = 0.125;
            //trapPrm.smoothTime = 25;
            //// 设置点位运动参数
            //m_nReturn = GT_SetTrapPrm(nAxis, ref trapPrm);

            //// 设置 AXIS 轴的目标位置
            //sRtn = GT_SetPos(AXIS, 50000L);
            //commandhandler("GT_SetPos", sRtn);
            //// 设置AXIS轴的目标速度
            //sRtn = GT_SetVel(AXIS, 50);
            //commandhandler("GT_SetVel", sRtn);
            //// 启动AXIS轴的运动
            //sRtn = GT_Update(1 << (AXIS - 1));
            //commandhandler("GT_Update", sRtn)

            m_nInitial = 0;
        }

        public Boolean MoveTo()
        {
            Boolean bRet = false;

            return bRet;
        }

        private void buttonJog_Click(object sender, EventArgs e)
        {

        }

        private void buttondp2p_Click(object sender, EventArgs e)
        {
            //读取数据          
            short axis = m_nCurrentAxis;
            double vel = Convert.ToDouble(this.textBoxAxis1Aspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            double pos = Convert.ToDouble(this.textBoxAxis1Step.Text);

            gts.mc.TTrapPrm trapPrm = new gts.mc.TTrapPrm();

            trapPrm.acc = Convert.ToDouble(this.textBoxAxis1Acc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.dec = Convert.ToDouble(this.textBoxAxis1Dec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            trapPrm.smoothTime = (short)(1000 * Convert.ToDouble(this.textBoxAxis1Stime.Text));
            trapPrm.velStart = 0;

            m_gtsControl.SetWorkTrapPrm(axis, trapPrm, vel);

            m_gtsControl.P2PMove(axis, pos);

        }

        private void buttonClearAxis1_Click(object sender, EventArgs e)
        {
            gts.mc.GT_ClrSts(m_nCurrentAxis, 8);//清除各轴报警和限位
        }

        private void buttonAxis1ServoOn_Click(object sender, EventArgs e)
        {
            if (m_nReturn > 0)
            {
                showErrorMessage("板卡Error", m_nReturn);
                return;
            }

            if (!m_bAxisEnable)
            {
                gts.mc.GT_AxisOn(m_nCurrentAxis);//上伺服
                buttonAxis1ServoOn.Text = "关闭使能";
                m_bAxisEnable = true;
            }
            else
            {
                gts.mc.GT_AxisOff(m_nCurrentAxis);//下伺服
                buttonAxis1ServoOn.Text = "伺服使能";
                m_bAxisEnable = false;
            }


        }

        private void buttonAxis1Zero_Click(object sender, EventArgs e)
        {
            //if (m_gtsControl.IsExitChip())
            //{
            //    MessageBox.Show("检测到精圆");
            //}
            //else
            //{
            //    MessageBox.Show("没检测到精圆");
            //}
            /*
            if (m_nReturn > 0)
            {
                showErrorMessage("板卡Error", m_nReturn);
                return;
            }

            gts.mc.GT_ZeroPos(m_nCurrentAxis, 8);   
             * */
            gts.mc.GT_ZeroPos(m_nCurrentAxis, 1);
        }

        private void buttonAxis1Stop_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveStop(m_nCurrentAxis);
        }

        private void buttonAxis1EStop_Click(object sender, EventArgs e)
        {

        }

        private void buttonAxis1Home_Click(object sender, EventArgs e)
        {
            //#region //手动查找原点
            //m_nReturn = gts.mc.GT_HomeInit();					// 初始化自动回原点功能
            //m_nReturn = gts.mc.GT_AxisOn(m_nCurrentAxis);		// 使能轴1
            //m_nReturn = gts.mc.GT_Index(m_nCurrentAxis, 20000, 2000);			// 轴1为Home+Index回零模式
            //m_nReturn = gts.mc.GT_Home(m_nCurrentAxis, 200000, 10, 0.5, 2000);


            ////Home搜索距离
            ////m_nSearchHome

            //short sRtn;
            //// 启动Home捕获
            //sRtn = gts.mc.GT_SetCaptureMode(m_nCurrentAxis, gts.mc.CAPTURE_HOME);
            //// 切换到点位运动模式
            //sRtn = gts.mc.GT_PrfTrap(m_nCurrentAxis);
            //// 读取点位模式运动参数
            //TTrapPrm trapPrm = new TTrapPrm();          
            //sRtn = gts.mc.GT_GetTrapPrm(m_nCurrentAxis, out trapPrm);

            //trapPrm.acc = 0.25;
            //trapPrm.dec = 0.25;
            //// 设置点位模式运动参数
            //sRtn = gts.mc.GT_SetTrapPrm(m_nCurrentAxis, ref trapPrm);
            //// 设置点位模式目标速度，即回原点速度
            //sRtn = gts.mc.GT_SetVel(m_nCurrentAxis, 10);
            //// 设置点位模式目标位置，即原点搜索距离
            //sRtn = gts.mc.GT_SetPos(m_nCurrentAxis, m_nSearchHome);
            //// 启动运动
            //sRtn = gts.mc.GT_Update(1 << (m_nCurrentAxis - 1));

            //m_nReturn = sRtn;
            //#endregion //查找原点结束

            m_gtsControl.FindHome(m_nCurrentAxis, true);
        }

        public string GetThreadContolText(Control control)
        {
            string retString = null;

            if (control.InvokeRequired)
            {
                return (string)control.Invoke(
                  new Func<String>(() => GetThreadContolText(control))
                );
            }
            else
            {
                if (control != null)
                {
                    retString = control.Text;
                }
            }


            return retString;
        }

        private void SetThreadText(Control control, string text)
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                try
                {
                    SetTextCallback d = new SetTextCallback(SetThreadText);
                    this.Invoke(d, new object[] { control, text });
                }
                catch
                {
                }
            }
            else
            {
                {
                    control.Text = text;
                }
            }

        }

        private void SetThreadColor(Control control, Color bkColor)
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                try
                {
                    SetColorCallback d = new SetColorCallback(SetThreadColor);
                    this.Invoke(d, new object[] { control, bkColor });
                }
                catch
                {
                }
            }
            else
            {
                {
                    control.BackColor = bkColor;
                }
            }
        }
        private void AxisTestForm_Load(object sender, EventArgs e)
        {
            StartWork();
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
            workTimer.Interval = 500;
            workTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            workTimer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MyLock(showInfoObject))
            {
                try
                {
                    //if (m_nReturn == 0)
                    {

                        short axis = m_nCurrentAxis;

                        double prfpos = 0;
                        uint clk = 0;

                        Control control;
                        string strText;
                        gts.mc.GT_GetPrfPos(axis, out prfpos, 1, out clk);
                        control = this.textBoxPlaningPos;
                        strText = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[axis - 1], 1).ToString();
                        SetThreadText(control, strText);


                        double prfvel = 0;
                        gts.mc.GT_GetPrfVel(axis, out prfvel, 1, out clk);
                        control = this.textBoxPlaningSpeed;
                        strText = Math.Round(prfvel, 1).ToString();
                        SetThreadText(control, strText);


                        double encpos = 0;
                        gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);
                        control = this.textBoxRuningPos;
                        strText = Math.Round(encpos / AxisTestForm.AxisPulseEquivalent[axis - 1], 1).ToString();
                        SetThreadText(control, strText);


                        double encvel = 0;
                        gts.mc.GT_GetEncVel(axis, out encvel, 1, out clk);
                        control = this.textBoxRuningSpeed;
                        strText = Math.Round(encvel, 1).ToString();
                        SetThreadText(control, strText);

                        // 读取轴状态
                        int lAxisStatus = 0;
                        gts.mc.GT_GetSts(axis, out lAxisStatus, 1, out clk);

                        Color bkColor;
                        Control clrControl;

                        clrControl = labelAxis1DeviceAlarm;
                        // 伺服报警标志
                        if ((lAxisStatus & 0x2) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1RunError;
                        // 跟随误差越限标志
                        if ((lAxisStatus & 0x10) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1PLimit;
                        // 正向限位
                        if ((lAxisStatus & 0x20) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1NLimit;
                        // 负向限位
                        if ((lAxisStatus & 0x40) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1Stop;
                        // 平滑停止
                        if ((lAxisStatus & 0x80) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1EStop;
                        // 急停标志
                        if ((lAxisStatus & 0x100) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1ServoOn;
                        // 伺服使能标志
                        if ((lAxisStatus & 0x200) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        clrControl = labelAxis1Run;
                        // 规划器正在运动标志
                        if ((lAxisStatus & 0x400) != 0)
                        {
                            bkColor = Color.Red;
                        }
                        else
                        {
                            bkColor = Color.Green;
                        }
                        SetThreadColor(clrControl, bkColor);

                        // 读取捕获状态
                        short capture = 0;
                        int pos = 0;
                        gts.mc.GT_GetCaptureStatus(axis, out capture, out pos, 1, out clk);
                        // 读取规划位置
                        //gts.mc.GT_GetPrfPos(axis, out prfpos, 1, out clk);
                        // 读取编码器位置
                        //gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);


                        //Home回零阶段的状态

                        //有按键输入退出循环
                        //if (System.Console.KeyAvailable)
                        //    break;

                        //ushort hState = 0;
                        //m_nReturn = gts.mc.GT_HomeSts(axis, out hState); // 查询返回状态

                    }
                }
                catch
                {

                }
                Monitor.Exit(showInfoObject);
            }
        }

        private void buttonJog_MouseDown(object sender, MouseEventArgs e)
        {

            ///读取参数            
            short axis = m_nCurrentAxis;

            gts.mc.TJogPrm jog = new mc.TJogPrm();


            double vel = Convert.ToDouble(this.textBoxAxis1Mspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.acc = Convert.ToDouble(this.textBoxAxis1MAcc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.dec = Convert.ToDouble(this.textBoxAxis1MDec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            m_gtsControl.SetManualJogPrm(axis, jog, vel);

            m_gtsControl.MoveJogMinus(axis);
        }

        private void buttonJog_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(m_nCurrentAxis);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {

            gts.mc.GT_Reset();
        }

        private void buttonInitial_Click(object sender, EventArgs e)
        {
            m_nReturn = gts.mc.GT_LoadConfig("GTS800.cfg");//加载配置文件
            if (showErrorMessage("cfg配置文件调用失败", m_nReturn))
            {
                return;
            }

            m_nReturn = gts.mc.GT_ClrSts(m_nCurrentAxis, 8);//清除限位和报警
            string strMessage = string.Format("轴{0}{1}", m_nCurrentAxis, "清除限位和报警失败");
            if (showErrorMessage(strMessage, m_nReturn))
            {
                return;
            }
        }

        private void buttonJogAdd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            ///读取参数
            short axis = m_nCurrentAxis;

            gts.mc.TJogPrm jog = new mc.TJogPrm();

            double vel = Convert.ToDouble(this.textBoxAxis1Mspeed.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.acc = Convert.ToDouble(this.textBoxAxis1MAcc.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];
            jog.dec = Convert.ToDouble(this.textBoxAxis1MDec.Text) * AxisTestForm.AxisPulseSpeedEquivalent[axis - 1];

            m_gtsControl.SetManualJogPrm(axis, jog, vel);

            m_gtsControl.MoveJogAdd(axis);

        }

        private void buttonJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(m_nCurrentAxis);
        }

        private void AxisTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }

    public class GLinkIOModule00
    {
        //标准模块1 16X16  输入 //EXT 0
        public static short slaveNO = -1;

        //In0 启动按钮
        public static short StartIOIN = 0;
        //In1 停止按钮
        public static short StopIOIN = 1;
        //In1 复位按钮
        public static short ResetIOIN = 2;
        //In3 急停按钮
        public static short EMStopIOIN = 3;

        public static short SafeDoorIOIN = 4;

        //In100 料盒确认按钮1
        public static short BoxReadyExIOIN1 = 5;
        //In101 料盒确认按钮2
        public static short BoxReadyExIOIN2 = 6;

        //In200 料盒感应开关1_A型号
        public static short BoxExitExIOIN1A = 7;
        //In201 料盒感应开关1_B型号
        public static short BoxExitExIOIN1B = 8;
        //In208 料盒感应开关1_C型号
        public static short BoxExitExIOIN1C = 9;
        //In209 料盒感应开关2_A型号
        public static short BoxExitExIOIN2A = 10;
        //In208 料盒感应开关2_B型号
        public static short BoxExitExIOIN2B = 11;
        //In209 料盒感应开关2_C型号
        public static short BoxExitExIOIN2C = 12;

        //In9 离子风报警
        public static short IconWarningIOIN = 13;
        //In10 功率气缸原点
        public static short PowerGasOrigIOIN = 14;
        //In11 功率气缸动点
        public static short PowerGasOutIOIN = 15;


        //输出
        //Out0 三色灯黄灯
        public static short YellowLightIOOUT = 0;
        //Out1 三色灯红灯
        public static short RedLightIOOUT = 1;
        //Out2 三色灯绿灯
        public static short GreenLightIOOUT = 2;
        //Out3 蜂鸣器
        public static short BeepIOOUT = 3;
        //Out4 蓝灯
        public static short BlueLightIOOUT = 4;

        //Out5 电子锁
        public static short ElectLockIOOUT = 5;
        //Out6 离子风1
        public static short Iconic1IOOUT = 6;
        //Out7 离子风2
        public static short Iconic2IOOUT = 7;

        //Out8 抽风机
        public static short BlowWindIOOUT = 8;
        //Out9 离子风复位
        public static short IconicResetIOOUT = 9;
        //Out10 料盒确认灯1
        public static short READY_LIGHTSB1 = 10;
        //Out11 料盒确认灯2
        public static short READY_LIGHTSB2 = 11;

        //功率气缸
        public static short PowerGasIOOUT = 14;
        public static short OCRGasIOOUT = 15;

    }

    public class GLinkIOModule01
    {
        //扩展模块1 32X  输入 //EXT 0
        public static short slaveNO = 0;

        //In00 上手臂真空吸
        public static short RobotRArmXifu_IN = 0;
        //In01 下手臂真空吸
        public static short RobotLArmXifu_IN = 1;
        //In02 机械手动作中
        public static short RobotRunning_IN = 2;
        //In03 机械手和寻边器动作NG
        public static short RobotRunningNG_IN = 3;
        //In04 寻边器有料感应
        public static short AlignerExitMaterial_IN = 4;
        //In05 寻边器准备信号
        public static short AlignerReady_IN = 5;
        //In06 机械手下手臂原点
        public static short AlignerRunning_IN = 6;
        //In07 机械手上手臂原点
        public static short AlignerXifu_IN = 7;

        //In00 机械手 Orig1
        public static short RobotUpArmOrig_IN = 5;
        //In01 机械手 Orig2
        public static short RobotDnArmOrig_IN = 6;

        //OCR 气缸
        public static short OCRGasOrig_IN = 7;
        public static short OCRGasOut_IN = 8;


        //In08 Smif1 料盒放好
        public static short Smif1Place_IN = 10;
        //In08 Smif1 料盒打开
        public static short Smif1Open_IN = 11;
        //In08 Smif1 报警
        public static short Smif1Error_IN = 12;


        //In08 Smif2 料盒放好
        public static short Smif2Place_IN = 13;
        //In08 Smif2 料盒打开
        public static short Smif2Open_IN = 14;
        //In08 Smif2 报警
        public static short Smif2Error_IN = 15;




        //In00 RobotX Smif1 LP1 修改为感应原点
        public static short RobotXAligner_IN = 12;
        //In01 RobotX Smif2
        public static short RobotXSmif2_IN = 13;
    }
}
