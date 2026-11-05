using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MyCommanLib;
using jog;
using System.Threading.Tasks;

namespace TabControlExDemo
{
    public partial class WarningForm : Form
    {
        public CuttingForm cuttingForm;

        delegate void SetBackTextCallback(Control control, string strText);

        Task threadWork;
        System.Timers.Timer workTimer;

        private object refreshObject = new object();

        public bool m_bFindHome = false;

        public int  m_nIOAdd = PLCOpcFeedParameters.XINGAN_IOOutState.TowerRedLight;

        public int m_nSleep = 400;

        public Boolean m_nShowContinue = false;

        public int m_nSecsAlarmIndex = -1;

        public void StartWork()
        {
            threadWork = new Task(new Action(work));
            threadWork.Start();
        }

        public void EndWork()
        {
            //if (threadWork == null)
            //    return;
            //workTimer.Enabled = false;
            //threadWork.Dispose();
        }

        private void work()
        {
            workTimer = new System.Timers.Timer();
            workTimer.Interval = 500;
            workTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            workTimer.Enabled = true;
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

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //lock (refreshObject)
            if( MyLock(refreshObject) )
            {
                try
                {
                    if (cuttingForm != null)
                    {
                        if (m_nIOAdd == GLinkIOModule00.YellowLightIOOUT)
                        {
                            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, true);
                            cuttingForm.chipAutoForm.WriteItemValue(GLinkIOModule00.BeepIOOUT, true);
                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, false);
                            cuttingForm.chipAutoForm.WriteItemValue(GLinkIOModule00.BeepIOOUT, false);
                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                        }
                        else if(m_nIOAdd == GLinkIOModule00.RedLightIOOUT)
                        {
                            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, true);
                            cuttingForm.chipAutoForm.WriteItemValue(GLinkIOModule00.BeepIOOUT, true);
                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                            cuttingForm.chipAutoForm.WriteItemValue(GLinkIOModule00.BeepIOOUT, false);
                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                        }
                        else if (m_nIOAdd >= 0)
                        {
                            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, true);

                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, false);

                            MyCommanLib.CommonDefine.TickSleep(m_nSleep);
                        }
                    }
                }
                catch (Exception)
                {

                }

                Monitor.Exit(refreshObject );
            }

        }

        private void SetThreadBackText(Control control, string strInfo)
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
                        SetBackTextCallback d = new SetBackTextCallback(SetThreadBackText);
                        this.Invoke(d, new object[] { control, strInfo });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    control.Text = strInfo;
                    if (strInfo == "end")
                    {
                        this.Close();
                    }
                }
            }
        }

        public void SetInfoText(string strInfo , int nSecsAlarmIndex = -1, int nSleep = 500 , int WarnIndex = 1)
        {
            m_nIOAdd = PLCOpcFeedParameters.XINGAN_IOOutState.TowerRedLight;

            if(WarnIndex == 2)
            {
                m_nIOAdd = PLCOpcFeedParameters.XINGAN_IOOutState.TowerYellowLight;
            }

            m_nSleep = nSleep;
            m_nSecsAlarmIndex = nSecsAlarmIndex;

            if (cuttingForm.IsEAPRunMode())
            {
                cuttingForm.secsForm.AlarmSet(nSecsAlarmIndex, true);
            }

            cuttingForm.chipAutoForm.m_nWorkState = AxisTestForm.WORK_WARNING;
            if (WarnIndex == 2)
                cuttingForm.chipAutoForm.m_nWorkState = AxisTestForm.WORK_MANUAL;

            cuttingForm.chipAutoForm.WriteItemValue(m_nIOAdd, true);
            cuttingForm.chipAutoForm.WriteItemValue(GLinkIOModule00.BeepIOOUT, true);

            
            SetThreadBackText(labelInfo, strInfo);

            if(cuttingForm != null )
            {
                cuttingForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Red);
                CommonDefine.SaveAlarmLogs(strInfo);
            }            
        }

        public WarningForm(CuttingForm gtsControl)
        {
            InitializeComponent();
            cuttingForm = gtsControl;
        }

        private void WarningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            EndWork();

         
            if( m_nSecsAlarmIndex >= 0 )
            {
                cuttingForm.secsForm.AlarmSet(m_nSecsAlarmIndex, false);
            }
            m_nIOAdd = -1;
            //cuttingForm.chipAutoForm.OperateClearLight();
            cuttingForm.ClearStates();            
        }

        private void WarningForm_Load(object sender, EventArgs e)
        {
            buttonContinue.Visible = m_nShowContinue;
            if (m_nShowContinue)
            {
                buttonOK.Text = "否";
            }
            else
            {
                buttonOK.Text = "确定";
            }
            cuttingForm.chipAutoForm.OperateClearLight();

            StartWork();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
