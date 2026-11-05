using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace jog
{
    public partial class WarningFormJog : Form
    {
        public GtsControl m_gtsControl;

        delegate void SetBackTextCallback(Control control, string strText);

        Thread threadWork;
        System.Timers.Timer workTimer;

        private object refreshObject = new object();

        public bool m_bFindHome = false;

        public short m_nIOAdd = AxisTestForm.RedLightIOOUT;

        public void StartWork()
        {
            threadWork = new Thread(new ThreadStart(work));
            threadWork.Start();
        }

        public void EndWork()
        {
            if (threadWork == null)
                return;
            workTimer.Enabled = false;
            threadWork.Abort();
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
            lock (refreshObject)
            {
                if (m_gtsControl != null )
                {
                    if( m_nIOAdd == AxisTestForm.RedLightIOOUT )
                    {
                        m_gtsControl.SetDo(m_nIOAdd, 1);
                        m_gtsControl.SetDo(AxisTestForm.BeepIOOUT, 1);
                        MyCommanLib.CommonDefine.TickSleep(1000);
                        m_gtsControl.SetDo(m_nIOAdd, 0);
                        m_gtsControl.SetDo(AxisTestForm.BeepIOOUT, 0);
                        MyCommanLib.CommonDefine.TickSleep(1000);
                    }else
                    {
                        m_gtsControl.SetDo(m_nIOAdd, 1);
                        
                        MyCommanLib.CommonDefine.TickSleep(1000);
                        m_gtsControl.SetDo(m_nIOAdd, 0);
                        
                        MyCommanLib.CommonDefine.TickSleep(1000);
                    }
                }
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

        public void SetInfoText(string strInfo)
        {
            SetThreadBackText(labelInfo, strInfo);
        }

        public WarningFormJog(GtsControl gtsControl)
        {
            InitializeComponent();
            m_gtsControl = gtsControl;
        }

        private void WarningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_gtsControl.OperateClearLight();
            m_gtsControl.ClearStates();
            EndWork();
        }

        private void WarningForm_Load(object sender, EventArgs e)
        {
            m_gtsControl.OperateClearLight();

            StartWork();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WarningForm_Shown(object sender, EventArgs e)
        {

        }
    }
}
