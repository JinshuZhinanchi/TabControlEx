using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using gts;

namespace jog
{
    public partial class SettingForm : Form
    {
        public GtsControl m_gtsControl;

        Thread threadWork;
        System.Timers.Timer workTimer;
        delegate void SetTextCallback(Control control, string text);

        delegate void SetColorCallback(Control control, Color bkColor);

        private object showInfoObject = new object();

        public int m_nBoxSizeType = 0;

        public SettingForm()
        {
            InitializeComponent();
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
            workTimer.Interval = 500;
            workTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            workTimer.Enabled = true;
        }

        private void SetThreadBkColor(Control control, Color bkColor   )
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                try
                {
                    SetColorCallback d = new SetColorCallback(SetThreadBkColor);
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

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (showInfoObject)
            {
                //if (m_nReturn == 0)
                {
                    short axis = AxisTestForm.STRECH_AXIS;

                    uint clk = 0;
                    Control control;
                    string strText;                    
                    
                    double encpos = 0;
                    gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);
                    control = this.textBoxXAxisPos;
                    strText = Math.Round(encpos / AxisTestForm.AxisPulseEquivalent[axis - 1], 1).ToString();
                    SetThreadText(control, strText);

                    axis = AxisTestForm.UPDN_AXIS;
                    gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);
                    control = this.textBoxZAxisPos;
                    strText = Math.Round(encpos / AxisTestForm.AxisPulseEquivalent[axis - 1], 1).ToString();
                    SetThreadText(control, strText);

                    //axis = AxisTestForm.ROTATE_AXIS;
                    //gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);
                    //control = this.textBoxRAxisPos;
                    //strText = Math.Round(encpos / AxisTestForm.AxisPulseEquivalent[axis - 1], 1).ToString();

                    //吸料状态
                    if( m_gtsControl.IsXiliao() )
                    {
                        SetThreadBkColor(labelXiliaoState, Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelXiliaoState, Color.Gray);
                    }

                    //翻转状态
                    if( m_gtsControl.IsRevertOrigPos() )
                    {
                        SetThreadBkColor(labelRevertState0, Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelRevertState0, Color.Gray);
                    }

                    //翻转状态
                    if (m_gtsControl.IsRevertWorkPos())
                    {
                        SetThreadBkColor(labelRevertState1, Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelRevertState1, Color.Gray);
                    }

                    //原点
                    gts.mc.THomeStatus homeStatus;
                    gts.mc.GT_GetHomeStatus(AxisTestForm.STRECH_AXIS, out homeStatus);

                    if (homeStatus.run == 0 && homeStatus.error == 0)
                    {
                        SetThreadBkColor(labelXAxisState,Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelXAxisState, Color.Gray);
                    }

                    gts.mc.GT_GetHomeStatus(AxisTestForm.UPDN_AXIS, out homeStatus);

                    if (homeStatus.run == 0 && homeStatus.error == 0)
                    {
                        SetThreadBkColor(labelZAxisState, Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelZAxisState, Color.Gray);
                    }

                    gts.mc.GT_GetHomeStatus(AxisTestForm.ROTATE_AXIS, out homeStatus);

                    if (homeStatus.run == 0 && homeStatus.error == 0)
                    {
                        SetThreadBkColor(labelRAxisOrigState, Color.Green);
                    }
                    else
                    {
                        SetThreadBkColor(labelRAxisOrigState, Color.Gray);
                    }
                }
            }
        }

        private void buttonXAxisMinus_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogMinus(AxisTestForm.STRECH_AXIS);
        }

        private void buttonXAxisMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.STRECH_AXIS);

            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.STRECH_AXIS, out prfpos, 1, out clk);
            textBoxXAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.STRECH_AXIS - 1], 2).ToString();
        }

        private void buttonXAxisAdd_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogAdd(AxisTestForm.STRECH_AXIS);
        }

        private void buttonXAxisAdd_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.STRECH_AXIS);


            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.STRECH_AXIS, out prfpos, 1, out clk);
            textBoxXAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.STRECH_AXIS - 1], 2).ToString();
            
        }

        private void buttonZAxisMinus_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogMinus(AxisTestForm.UPDN_AXIS);
        }

        private void buttonZAxisMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.UPDN_AXIS);

            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.UPDN_AXIS, out prfpos, 1, out clk);
            textBoxZAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS - 1], 2).ToString();
        }

        private void buttonZAxisAdd_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogAdd(AxisTestForm.UPDN_AXIS);
        }

        private void buttonZAxisAdd_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.UPDN_AXIS);

            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.UPDN_AXIS, out prfpos, 1, out clk);
            textBoxZAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS - 1], 2).ToString();
        }

        private void buttonXAxisOrig_Click(object sender, EventArgs e)
        {
            labelXAxisState.BackColor = Color.Yellow;
            m_gtsControl.FindHome(AxisTestForm.STRECH_AXIS);
                        
        }

        private void buttonZAxisOrig_Click(object sender, EventArgs e)
        {
            labelZAxisState.BackColor = Color.Yellow;
            m_gtsControl.FindHome(AxisTestForm.UPDN_AXIS);

            gts.mc.THomeStatus homeStatus;
            gts.mc.GT_GetHomeStatus(AxisTestForm.UPDN_AXIS, out homeStatus);

            if (homeStatus.run == 0 && homeStatus.error == 0)
            {
                labelZAxisState.BackColor = Color.Green;
            }
            else
            {
                labelZAxisState.BackColor = Color.Red;
            }
        }

        private void buttonXZAxisSafePos_Click(object sender, EventArgs e)
        {

            m_gtsControl.MoveToSafePoint(AxisTestForm.STRECH_AXIS);

            m_gtsControl.MoveToSafePoint(AxisTestForm.UPDN_AXIS);
        }

        private void buttonXZAxisStartPos_Click(object sender, EventArgs e)
        {       

            m_gtsControl.MoveToStartPoint(m_nBoxSizeType, 0);
        }

        private void buttonXZAxisScanPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToScanPoint(m_nBoxSizeType, 0);
        }

        private void buttonXZAxisPhotoPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToPhotoPoint(m_nBoxSizeType);
        }

        private void buttonXZAxisMarkPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToMarkPoint(m_nBoxSizeType);
        }


        private void buttonXZAxisMarkRevertPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToMarkRevertPoint(m_nBoxSizeType);
        }

        private void buttonXZAxisSafePosSave_Click(object sender, EventArgs e)
        {
            
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisSafePos.Text = strText;
            double fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_SafePoint.m_fx = fPos;

            strText = textBoxZAxisPos.Text;
            textBoxZAxisSafePos.Text = strText;
            fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_SafePoint.m_fz = fPos;
        }

        private void buttonXZAxisStartPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisStartPos.Text = strText;
            double fPos = double.Parse(strText);
            for(int i = 0; i < 8; i++ )
            {
                m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[i].m_fx = fPos;
            }            

            strText = textBoxZAxisPos.Text;
            textBoxZAxisStartPos.Text = strText;
            fPos = double.Parse(strText);
            for (int i = 0; i < 8; i++)
            {
                m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[i].m_fz = fPos;
            }
            
        }

        private void buttonXZAxisScanPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisScanPos.Text = strText;
            double fPos = double.Parse(strText);
            for (int i = 0; i < 8; i++)
            {
                m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[i].m_fx = fPos;
            }

            strText = textBoxZAxisPos.Text;
            textBoxZAxisScanPos.Text = strText;
            fPos = double.Parse(strText);
            for (int i = 0; i < 8; i++)
            {
                m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[i].m_fz = fPos;
            }
        }

        private void buttonXZAxisPhotoPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisPhotoPos.Text = strText;
            double fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_TakePhotoPoint.m_fx = fPos;

            strText = textBoxZAxisPos.Text;
            textBoxZAxisPhotoPos.Text = strText;
            fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_TakePhotoPoint.m_fz = fPos;
        }

        private void buttonXZAxisMarkPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisMarkPos.Text = strText;
            double fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkPoint.m_fx = fPos;

            strText = textBoxZAxisPos.Text;
            textBoxZAxisMarkPos.Text = strText;
            fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkPoint.m_fz = fPos;
        }

        private void buttonXZAxisMarkRevertPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxXAxisPos.Text;
            textBoxXAxisMarkRevertPos.Text = strText;
            double fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkRevertPoint.m_fx = fPos;

            strText = textBoxZAxisPos.Text;
            textBoxZAxisMarkRevertPos.Text = strText;
            fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkRevertPoint.m_fz = fPos;

        }

        private void buttonZAxisOffset_Click(object sender, EventArgs e)
        {
            string strText = textBoxZAxisLayerOffset.Text;
            double fPos = double.Parse(strText);

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_fzLayerOffset = fPos;

            strText = textBoxXiliaoSleep.Text;
            int nSleep = int.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_nXiliaoSleep = nSleep;

        }

        private void buttonZAxisHeight_Click(object sender, EventArgs e)
        {
            string strText = textBoxZAxisTakeOffset.Text;
            double fPos = double.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_fzTakeOffset = fPos;

            strText = textBoxRevertSleep.Text;
            int nSleep = int.Parse(strText);
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_nRevertSleep = nSleep;
        }

        private void buttonRAxisOrig_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show("请确认旋转轴能否安全回原点，确认回原点吗？", "提示", MessageBoxButtons.YesNo);
            if( dlgResult == DialogResult.Yes )
            {
                if( m_gtsControl.IsXSafePos() && m_gtsControl.IsZSafePos() )
                {                   

                    labelRAxisOrigState.BackColor = Color.Yellow;
                    m_gtsControl.FindHome(AxisTestForm.ROTATE_AXIS);

                    gts.mc.THomeStatus homeStatus;
                    gts.mc.GT_GetHomeStatus(AxisTestForm.ROTATE_AXIS, out homeStatus);

                    if (homeStatus.run == 0 && homeStatus.error == 0)
                    {
                        labelRAxisOrigState.BackColor = Color.Green;
                    }
                    else
                    {
                        labelRAxisOrigState.BackColor = Color.Red;
                    }

                }
                else
                {
                    MessageBox.Show("有轴不在安全位置，请手动检查。");
                }                
            }
        }

        private void buttonRAxisMinus_MouseDown(object sender, MouseEventArgs e)
        {
           // if (m_gtsControl.IsXSafePos() && m_gtsControl.IsZSafePos())
            {
                m_gtsControl.MoveJogMinus(AxisTestForm.ROTATE_AXIS);
            }                
        }

        private void buttonRAxisMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.ROTATE_AXIS);

            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.ROTATE_AXIS, out prfpos, 1, out clk);
            textBoxRAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.ROTATE_AXIS - 1], 2).ToString();
        }

        private void buttonRAxisAdd_MouseDown(object sender, MouseEventArgs e)
        {
           // if (m_gtsControl.IsXSafePos() && m_gtsControl.IsZSafePos())
            {
                m_gtsControl.MoveJogAdd(AxisTestForm.ROTATE_AXIS);
            }
        }

        private void buttonRAxisAdd_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(AxisTestForm.ROTATE_AXIS);

            double prfpos = 0;
            uint clk = 0;
            gts.mc.GT_GetPrfPos(AxisTestForm.ROTATE_AXIS, out prfpos, 1, out clk);
            textBoxRAxisPos.Text = Math.Round(prfpos / AxisTestForm.AxisPulseEquivalent[AxisTestForm.ROTATE_AXIS - 1], 2).ToString();
        }

        private void buttonRAxisSafePos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToSafePoint( m_nBoxSizeType );
        }

        private void buttonRAxisStartPos1_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,0);
        }

        private void buttonRAxisStartPos2_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,1);
        }

        private void buttonRAxisStartPos3_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,2);
        }

        private void buttonRAxisStartPos4_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,3);
        }

        private void RAxisStartPos5_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,4);
        }

        private void buttonRAxisStartPos6_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,5);
        }

        private void buttonRAxisStartPos7_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,6);
        }

        private void buttonRAxisStartPos8_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToStartPoint(m_nBoxSizeType,7);
        }

        private void buttonRAxisPhotoPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToPhotoPoint(m_nBoxSizeType);
        }

        private void buttonRAxisMarkPos_Click(object sender, EventArgs e)
        {
            m_gtsControl.MoveToMarkRevertPoint(m_nBoxSizeType);
        }

        private void buttonRAxisSafePosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisSafePos.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_SafePoint.m_fr = fPos;
        }

        private void buttonRAxisStartPosSave1_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos1.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[0].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[0].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave2_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos2.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[1].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[1].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave3_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos3.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[2].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[2].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave4_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos4.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[3].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[3].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave5_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos5.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[4].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[4].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave6_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos6.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[5].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[5].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave7_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos7.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[6].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[6].m_fr = fPos;
        }

        private void buttonRAxisStartPosSave8_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisStartPos8.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_StartPoint[7].m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_ScanStartPoint[7].m_fr = fPos;
        }

        private void buttonRAxisPhotoPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);
            textBoxRAxisPhotoPos.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_TakePhotoPoint.m_fr = fPos;
        }

        private void buttonRAxisMarkPosSave_Click(object sender, EventArgs e)
        {
            string strText = textBoxRAxisPos.Text;
            double fPos = double.Parse(strText);

            textBoxRAxisMarkPos.Text = strText;

            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkPoint.m_fr = fPos;
            m_gtsControl.m_gtsWorkStation[m_nBoxSizeType].m_MarkRevertPoint.m_fr = fPos;
        }

        public void WriteGtsSettingParam()
        {            

            string fileName = System.Windows.Forms.Application.StartupPath + "\\gtsSettingParam.ini";

            string strText = textBoxXAxisSafePos.Text;
            string keyName = "textBoxXAxisSafePos";

            //X 轴 
            strText = textBoxXAxisSafePos.Text;
            keyName = string.Format("textBoxXAxisSafePos{0}",m_nBoxSizeType );
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXAxisStartPos.Text;            
            keyName = string.Format("textBoxXAxisStartPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXAxisScanPos.Text;
            keyName = "";
            keyName = string.Format("textBoxXAxisScanPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXAxisPhotoPos.Text;
            keyName = "";
            keyName = string.Format("textBoxXAxisPhotoPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXAxisMarkRevertPos.Text;
            keyName = "";
            keyName = string.Format("textBoxXAxisMarkRevertPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXAxisMarkPos.Text;
            keyName = "";
            keyName = string.Format("textBoxXAxisMarkPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            //Z 轴 
            strText = textBoxZAxisSafePos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisSafePos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisStartPos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisStartPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisScanPos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisScanPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisPhotoPos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisPhotoPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisMarkRevertPos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisMarkRevertPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisMarkPos.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisMarkPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisLayerOffset.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisLayerOffset{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxZAxisTakeOffset.Text;
            keyName = "";
            keyName = string.Format("textBoxZAxisTakeOffset{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            //R 轴 
            strText = textBoxRAxisSafePos.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisSafePos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos1.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos1{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos2.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos2{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos3.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos3{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos4.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos4{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos5.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos5{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos6.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos6{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos7.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos7{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisStartPos8.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisStartPos8{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisPhotoPos.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisPhotoPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRAxisMarkPos.Text;
            keyName = "";
            keyName = string.Format("textBoxRAxisMarkPos{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxXiliaoSleep.Text;
            keyName = "";
            keyName = string.Format("textBoxXiliaoSleep{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

            strText = textBoxRevertSleep.Text;
            keyName = "";
            keyName = string.Format("textBoxRevertSleep{0}", m_nBoxSizeType);
            MyCommanLib.CommonDefine.WritePrivateProfileString("gtsSetting", keyName, strText, fileName);

        }

        public void ReadGtsSettingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\gtsSettingParam.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            string strText = "0";
            string keyName = "gtsSetting";

            StringBuilder temp = new StringBuilder(255);

            for (int i = 0; i < 2; i++)
            {
                //X 轴 
                keyName = "";
                keyName = string.Format("textBoxXAxisSafePos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisSafePos.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxXAxisStartPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisStartPos.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxXAxisScanPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisScanPos.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxXAxisPhotoPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisPhotoPos.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxXAxisMarkRevertPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisMarkRevertPos.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxXAxisMarkPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXAxisMarkPos.Text = temp.ToString();

                //Z 轴 
                keyName = "";
                keyName = string.Format("textBoxZAxisSafePos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisSafePos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisStartPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisStartPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisScanPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisScanPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisPhotoPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisPhotoPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisMarkRevertPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisMarkRevertPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisMarkPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisMarkPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisLayerOffset{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisLayerOffset.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxZAxisTakeOffset{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxZAxisTakeOffset.Text = temp.ToString();

                //R 轴 

                keyName = "";
                keyName = string.Format("textBoxRAxisSafePos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisSafePos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos1{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos1.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos2{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos2.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos3{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos3.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos4{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos4.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos5{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos5.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos6{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos6.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos7{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos7.Text = temp.ToString();

                keyName = "";
                keyName = string.Format("textBoxRAxisStartPos8{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisStartPos8.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisPhotoPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisPhotoPos.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRAxisMarkPos{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRAxisMarkPos.Text = temp.ToString();

                strText = "500";
                keyName = "";
                keyName = string.Format("textBoxXiliaoSleep{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxXiliaoSleep.Text = temp.ToString();


                keyName = "";
                keyName = string.Format("textBoxRevertSleep{0}", i);
                MyCommanLib.CommonDefine.GetPrivateProfileString("gtsSetting", keyName, strText, temp, 255, fileName);
                textBoxRevertSleep.Text = temp.ToString();

                //safepos
                double fPos = double.Parse(textBoxXAxisSafePos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_SafePoint.m_fx = fPos;

                fPos = double.Parse(textBoxZAxisSafePos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_SafePoint.m_fz = fPos;

                fPos = double.Parse(textBoxRAxisSafePos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_SafePoint.m_fr = fPos;

                //takepos

                //sleep
                int nSleep = int.Parse(textBoxXiliaoSleep.Text);
                m_gtsControl.m_gtsWorkStation[i].m_nXiliaoSleep = nSleep;

                nSleep = int.Parse(textBoxRevertSleep.Text);
                m_gtsControl.m_gtsWorkStation[i].m_nRevertSleep = nSleep;

                //startPos            
                fPos = double.Parse(textBoxXAxisStartPos.Text);
                for (int ii = 0; ii < 8; ii++)
                {
                    m_gtsControl.m_gtsWorkStation[i].m_StartPoint[ii].m_fx = fPos;
                }

                fPos = double.Parse(textBoxZAxisStartPos.Text);
                for (int ii = 0; ii < 8; ii++)
                {
                    m_gtsControl.m_gtsWorkStation[i].m_StartPoint[ii].m_fz = fPos;
                }

                fPos = double.Parse(textBoxRAxisStartPos1.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[0].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos2.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[1].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos3.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[2].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos4.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[3].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos5.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[4].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos6.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[5].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos7.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[6].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos8.Text);
                m_gtsControl.m_gtsWorkStation[i].m_StartPoint[7].m_fr = fPos;

                //scan pos            
                fPos = double.Parse(textBoxXAxisScanPos.Text);
                for (int ii = 0; ii < 8; ii++)
                {
                    m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[ii].m_fx = fPos;
                }

                fPos = double.Parse(textBoxZAxisScanPos.Text);
                for (int ii = 0; ii < 8; ii++)
                {
                    m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[ii].m_fz = fPos;
                }

                fPos = double.Parse(textBoxRAxisStartPos1.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[0].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos2.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[1].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos3.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[2].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos4.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[3].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos5.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[4].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos6.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[5].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos7.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[6].m_fr = fPos;

                fPos = double.Parse(textBoxRAxisStartPos8.Text);
                m_gtsControl.m_gtsWorkStation[i].m_ScanStartPoint[7].m_fr = fPos;

                //photo            
                fPos = double.Parse(textBoxXAxisPhotoPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_TakePhotoPoint.m_fx = fPos;

                fPos = double.Parse(textBoxZAxisPhotoPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_TakePhotoPoint.m_fz = fPos;

                fPos = double.Parse(textBoxRAxisPhotoPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_TakePhotoPoint.m_fr = fPos;

                //mark 
                fPos = double.Parse(textBoxXAxisMarkPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkPoint.m_fx = fPos;

                fPos = double.Parse(textBoxZAxisMarkPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkPoint.m_fz = fPos;

                fPos = double.Parse(textBoxRAxisMarkPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkPoint.m_fr = fPos;

                //mark revert 
                fPos = double.Parse(textBoxXAxisMarkRevertPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkRevertPoint.m_fx = fPos;

                fPos = double.Parse(textBoxZAxisMarkRevertPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkRevertPoint.m_fz = fPos;

                fPos = double.Parse(textBoxRAxisMarkPos.Text);
                m_gtsControl.m_gtsWorkStation[i].m_MarkRevertPoint.m_fr = fPos;

                //offset
                fPos = double.Parse(textBoxZAxisLayerOffset.Text);
                m_gtsControl.m_gtsWorkStation[i].m_fzLayerOffset = fPos;

                fPos = double.Parse(textBoxZAxisTakeOffset.Text);
                m_gtsControl.m_gtsWorkStation[i].m_fzTakeOffset = fPos;
            }
        }

        private void buttonXiliao_Click(object sender, EventArgs e)
        {
            if( m_gtsControl.IsXiliao() )
            {
                m_gtsControl.OperateXiliao(false);
            }else
            {
                m_gtsControl.OperateXiliao(true);
            }          

            
        }

        private void buttonRevert_Click(object sender, EventArgs e)
        {
            if( m_gtsControl.IsRevertOrigPos() )
            {
                m_gtsControl.OperateRevertGas(false);
            }else   if( m_gtsControl.IsRevertWorkPos() )
            {
                m_gtsControl.OperateRevertGas(true);
            }
        }

        private void labelRAxisOrigState_Click(object sender, EventArgs e)
        {

        }

        private void buttonXAxisMinus_Click(object sender, EventArgs e)
        {
           
        }
    }
}
