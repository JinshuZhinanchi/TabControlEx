using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace jog
{
    public partial class InfoForm : Form
    {
        public GtsControl m_gtsControl;

        delegate void SetBackTextCallback(Control control, string strText);

        Task threadWork;
        System.Timers.Timer workTimer;

        private object refreshObject = new object();

        public bool m_bFindHome = false;

        public void StartWork()
        {
            threadWork = new Task(  new Action( work ) );
            threadWork.Start();
        }

        public void EndWork()
        {
            if (threadWork == null)
                return;
            workTimer.Enabled = false;
            threadWork.Dispose();
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
            //lock( refreshObject )
            if( MyLock(refreshObject) )
            {
                try
                {
                    if (m_gtsControl != null && m_bFindHome == false)
                    {
                        m_bFindHome = true;
                        m_gtsControl.FindAllHome();
                        m_gtsControl.m_nWorkState = AxisTestForm.WORK_MANUAL;
                        EndWork();
                        SetThreadBackText(labelInfo, "end");

                    }
                }
                catch (Exception)
                {                    
                }

                Monitor.Exit(refreshObject);
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
                    if( strInfo == "end")
                    {
                        this.Close();                        
                    }
                }
            }
        }

        public InfoForm()
        {
            InitializeComponent();
        }

        public void SetInfoText(string strInfo)
        {
            SetThreadBackText(labelInfo, strInfo);
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            //if( m_nOperateType == 0 )
            {
                Thread.Sleep(100);
                //StartWork();
            }            
        }
    }
}
