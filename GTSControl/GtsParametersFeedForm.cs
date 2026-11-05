using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jog
{
    public partial class GtsParametersFeedForm : Form
    {
        GtsControl m_gtsControl;

        //MarkXParamForm      markX;        
        public  MarkZParamForm      markZ;
        //public MarkRParamForm markR;//对应Y
        public MarkRParamForm robotX;
                
        public string strProductName;

        private object lockReadOpcState = new object();

        Task    threadWork;
        System.Timers.Timer workTimer;

        public GtsParametersFeedForm()
        {
            InitializeComponent();          
        }

        public GtsParametersFeedForm(GtsControl gtsControl)
        {
            InitializeComponent();
            m_gtsControl = gtsControl;

            CreateForm();                       
        }

        public void CreateForm()
        {

            //markX = new MarkXParamForm();
            robotX = new MarkRParamForm(m_gtsControl);
            markZ = new MarkZParamForm(m_gtsControl);
            //robotX = new RobotXParamForm(m_gtsControl);

            InitialForm(tabControlMotor);
        }

        public void InitialForm(TabControl tabControl)
        {
            #region 初始化 Tabpage

            Form formPage;           
           
            formPage = markZ;
            AddTabPage("markZ", "打标Z轴位置参数", formPage, tabControl);

            formPage = robotX;
            AddTabPage("robotX", "机械手X轴位置参数", formPage, tabControl);

            //formPage = markR;
            //AddTabPage("markR", "打标R轴位置参数", formPage, tabControl);
                                
            #endregion
                        
        }        

        public void readAxisData()
        {           
            //refresh autoForm            
            string tabPaeName = GetThreadTabName(tabControlMotor);
            if (tabPaeName != null)
            {
                if (tabPaeName == "robotX")
                {
                    robotX.RefreshUI();
                }
                else if (tabPaeName == "markR")
                {
                    //markR.RefreshUI();
                }
                else if (tabPaeName == "markZ")
                {
                    markZ.RefreshUI();
                }
            }
        }

        public string GetThreadTabName(TabControl tabControl)
        {
            string retString = null;
            if (tabControl.InvokeRequired)
            {
                return (string)tabControl.Invoke(
                  new Func<String>(() => GetThreadTabName(tabControl))
                );
            }
            else
            {
                if (tabControl != null)
                {
                    retString = tabControl.SelectedTab.Name;
                }
            }

            //this.BeginInvoke(dl, "delegate three"); // 异步调用委托,调用后立即返回并立即执行下面的语句
            //this.Invoke(getName, tabControl, retString); // 等待工作线程完成后, 才接着执行下面的语句.
            return retString;
        }

        public void AddTabPage(string tabName, string pageTitle, Form form, TabControl tableControl)
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

        public void RefreshUI()
        {
            string tabPaeName = GetThreadTabName(tabControlMotor);
            if (tabPaeName != null)
            {
            }
        }

        private void tabControlMotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        public void AxisData2UI()
        {
            markZ.AxisData2UI();
            robotX.AxisData2UI();
        }

        public void UI2AxisData()
        {
            markZ.UI2AxisData();
            robotX.UI2AxisData();
        }

        private void ParametersFeedForm_Load(object sender, EventArgs e)
        {
            //当前数据显示到界面

            MyCommanLib.CommonDefine.GetRobotPosIni();
            AxisData2UI();
            StartWork();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            UI2AxisData();
            EndWork();

            MyCommanLib.CommonDefine.WriteRobotPosIni();

            //GC.Collect();
            Close();
        }

        public void StartWork()
        {
            threadWork = new Task(new Action(work));
            threadWork.Start();
        }

        public void EndWork()
        {
            if (threadWork != null)
            {
                workTimer.Enabled = false;
                threadWork.Dispose();
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
            //lock (lockReadOpcState)
            {
                readAxisData();
            }
        }

        private void ParametersFeedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
