using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class ParametersFeedForm : BaseForm
    {                      
        FeedTransXForm      findEdgeX;        
        FeedTransYForm      findEdgeY;
        FeedRotateZForm     findEdgeR;

        MarkXParamForm      robotX;
       // MarkYParamForm      markY;
        MarkZParamForm      markZ;

        public Boolean m_bFormInitialed = false;
        OpcOperateForm parentOpcOperate;
        public string strProductName;

        private object lockReadOpcState = new object();
        Task    threadWork;
        System.Timers.Timer workTimer;

        public ParametersFeedForm()
        {
            InitializeComponent();
            CreateForm();
                       
        }

        public void CreateForm()
        {          
            findEdgeX = new FeedTransXForm();
            findEdgeY = new FeedTransYForm();
            findEdgeR = new FeedRotateZForm();

            robotX = new MarkXParamForm();
            //markY = new MarkYParamForm();
            markZ = new MarkZParamForm();

            InitialForm(tabControlMotor);
        }

        public void InitialForm(TabControl tabControl)
        {
            #region 初始化 Tabpage

            Form formPage;
            
            //formPage = findEdgeX;
            //AddTabPage("findEdgeX", "寻边X轴位置参数", formPage, tabControl);

            //formPage = findEdgeY;
            //AddTabPage("findEdgeY", "寻边Y轴位置参数", formPage, tabControl);

            //formPage = findEdgeR;
            //AddTabPage("findEdgeR", "寻边R轴位置参数", formPage, tabControl);

            formPage = markZ;
            AddTabPage("markZ", "Z轴位置参数", formPage, tabControl);

            //formPage = robotX;
            //AddTabPage("robotX", "机械手X轴位置参数", formPage, tabControl);

            //formPage = markY;
            //AddTabPage("markY", "正打Y轴位置参数", formPage, tabControl);

            
         
            #endregion

            m_bFormInitialed = true;
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            //parentOpcOperate = opcOperate;

            //findEdgeX.InitialOpcButton( opcOperate );
            //findEdgeY.InitialOpcButton(opcOperate);
            //findEdgeR.InitialOpcButton(opcOperate);

            //robotX.InitialOpcButton(opcOperate);
            //markY.InitialOpcButton(opcOperate);
            markZ.InitialOpcButton(opcOperate);
        }

        public void readOpcData()
        {
            if (m_bFormInitialed == false)
                return;

            if(parentOpcOperate == null )
            {
                return;
            }

            //refresh autoForm            
            string tabPaeName = GetThreadTabName(tabControlMotor);
            if (tabPaeName != null)
            {
                if (tabPaeName == "findEdgeX")
                {
                    findEdgeX.readOpcData(parentOpcOperate);
                }
                else if (tabPaeName == "findEdgeY")
                {
                    findEdgeY.readOpcData(parentOpcOperate);
                }                
                else if (tabPaeName == "findEdgeR")
                {
                    findEdgeR.readOpcData(parentOpcOperate);
                }
                else if (tabPaeName == "robotX")
                {
                    robotX.readOpcData(parentOpcOperate);
                }
                else if (tabPaeName == "markY")
                {
                    //markY.readOpcData(parentOpcOperate);
                }
                else if (tabPaeName == "markZ")
                {
                    markZ.readOpcData(parentOpcOperate);
                }
            }
        }     

        private void ParametersFeedForm_SizeChanged(object sender, EventArgs e)
        {
              Form_SizeChanged(sender, e, tabControlMotor, 40);
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

        private void tabControlMotor_SizeChanged(object sender, EventArgs e)
        {
        }

        private void ParametersFeedForm_Load(object sender, EventArgs e)
        {
            

            int nWidth = ((this.Width - 30) / tabControlMotor.TabPages.Count);
            if (nWidth < 10)
            {
                nWidth = 80;
            }
            tabControlMotor.ItemSize = new Size(nWidth, 40);
            //AutoScale(this);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
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
            lock (lockReadOpcState)
            {
                readOpcData();
            }
        }
    }
}
