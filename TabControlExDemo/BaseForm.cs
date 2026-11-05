using MyCommanLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class BaseForm : Form
    {
        public int m_nStartTick = 0;
        Thread delayThread;
        System.Timers.Timer delayTimer;
        public int m_nCurrentLevel = CommonDefine.USER_OP;


        public List<EllipseButton> stateButtonList = new List<EllipseButton>();
        public List<MyOpcTextEdit> textEditList = new List<MyOpcTextEdit>();

        public List<MyOpcNumbEdit> numbEditList = new List<MyOpcNumbEdit>();

        public List<TextOpcButton> textButtonList = new List<TextOpcButton>();

        delegate void SetTextCallback(Control control, Color clr, string text);
        delegate void SetBackColorCallback(Control control, Color clr);
        public CuttingForm mainForm;

        public BaseForm()
        {
            InitializeComponent();
        }

        public void StartWork()
        {
            delayThread = new Thread(new ThreadStart(delayWork));
            delayThread.Start();
            m_nStartTick = Environment.TickCount;
        }

        public void EndWork()
        {
            if (delayThread == null)
                return;
            delayTimer.Enabled = false;
            delayThread.Abort();
        }

        private void delayWork()
        {
            delayTimer = new System.Timers.Timer();
            delayTimer.Interval = 5000;
            delayTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_delayWork);
            delayTimer.Enabled = true;
        }

        void timer_delayWork(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_nCurrentLevel == CommonDefine.USER_OP)
            {
                return;
            }

            //if (CommanDefine.DelayedTime(m_nStartTick) > mainForm.setForm.maxDelay)
            //{
            //    mainForm.set2AutoPage();
            //    m_nStartTick = Environment.TickCount;
            //}
        }

        #region 窗体大小自动变化函数
        public new static void AutoScale(Form frm)
        {
            frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
            //frm.SizeChanged += new EventHandler(Form_SizeChanged);
        }

        public void Form_Load(TabControl tabControl)
        {
            AutoScale(this);
            int nWidth = ((this.Width - 30) / tabControl.TabPages.Count);
            if( nWidth < 10 )
            {
                nWidth = 80;
            }
            tabControl.ItemSize = new Size(nWidth, 60);
        }


        public void SetThreadText(Control control, Color clr, string text)
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

        public void SetThreadBackColor(Control control, Color clr)

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

        public string GetThreadTabName(TabControl tabControl)
        {
            string retString = null;

            if (tabControl.InvokeRequired )
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
                if (tabControl != null && tabControl.SelectedTab != null )
                {
                    retString = tabControl.SelectedTab.Name;
                }
            }

            //this.BeginInvoke(dl, "delegate three"); // 异步调用委托,调用后立即返回并立即执行下面的语句
            //this.Invoke(getName, tabControl, retString); // 等待工作线程完成后, 才接着执行下面的语句.

            return retString;
        }

        /*      
        public static void AutoScale_Cancel(Form frm)
        {
           frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
           frm.SizeChanged -= new EventHandler(Form_SizeChanged);
        }
        static private void Form_SizeChanged(object sender, EventArgs e)
        {
           if (((Form)sender).Tag == null)
               return;

           string[] tmp = ((Form)sender).Tag.ToString().Split(',');
           if (tmp.Length < 1)
               return;

           float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
           float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);

           //   Single currentSize = Convert.ToSingle((float)Convert.ToInt16(tmp[2])); 

           ((Form)sender).Tag = ((Form)sender).Width.ToString() + "," + ((Form)sender).Height;

           foreach (Control control in ((Form)sender).Controls)
           {
               control.Scale(new SizeF(width, heigth));
               //  control.Font = new Font(control.Font.Name, currentSize, control.Font.Style, control.Font.Unit);  

           }
        }
        */
        #endregion


        public void Form_SizeChanged(object sender, EventArgs e, TabControl tableControl, int nHeight )
        {
            if (((Form)sender).Tag == null)
                return;

            string[] tmp = ((Form)sender).Tag.ToString().Split(',');
            if (tmp.Length < 1)
                return;

            float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
            float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);

            if (tableControl.TabPages.Count > 0)
            {
                int nWidth = ((this.Width - 30) / tableControl.TabPages.Count);
                if (nWidth < 10)
                    nWidth = 80;
                tableControl.ItemSize = new Size(nWidth, nHeight);
            }
        }

        public void AddTabPage(string tabName, string pageTitle,  Form form , TabControl  tableControl)
        {
            TabPage tab = new TabPage();
            tab.Name = tabName;
            tab.Text = pageTitle;

            form.TopLevel = false;      //设置为非顶级控件
            tab.Controls.Add(form);

            tableControl.TabPages.Add(tab);

            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        public void readOpcData(OpcOperateForm opcOperate)
        {
            if (opcOperate == null)
                return;
                       

            RefreshUI(opcOperate);
        }

        public void RefreshUI(OpcOperateForm opcOperate)
        {
            if (opcOperate == null)
                return;

            //button state 
            int nStateCount = stateButtonList.Count;
            EllipseButton stateButton;
            for (int i = 0; i < nStateCount; i++)
            {
                stateButton = stateButtonList[i];
                stateButton.RefreshOpcValue(opcOperate);

            }

            //Text edit
            int nEditCount = textEditList.Count;
            MyOpcTextEdit opcEdit;
          
            for (int i = 0; i < nEditCount; i++)
            {
                opcEdit = textEditList[i];
                opcEdit.RefreshOpcValue();
            }

            int nNumbEditCount = numbEditList.Count;
            MyOpcNumbEdit opcNumb;
            for(int i = 0; i < nNumbEditCount; i++)
            {
                opcNumb = numbEditList[i];
                opcNumb.RefreshOpcValue();
            }

            int ntextEditCount = textButtonList.Count;
            TextOpcButton textButton;
            for (int i = 0; i < ntextEditCount; i++)
            {
                textButton = textButtonList[i];
                textButton.RefreshOpcValue(opcOperate);
            }
            
        }


    }
}
