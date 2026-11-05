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
    public partial class IOCutForm : BaseForm
    {
        CutIOOutListViewForm ioOutListViewForm;
        CutIOInListViewForm ioInListViewForm;

        Task threadWork;
        System.Timers.Timer workTimer;

        private object refreshObject = new object();

        OpcOperateForm parentOpcOperate;

        public IOCutForm()
        {
            InitializeComponent();


            InitialForm(tabControlIO);

            int nWidth = ((this.Width - 30) / tabControlIO.TabPages.Count);
            if (nWidth < 10)
            {
                nWidth = 80;
            }
            tabControlIO.ItemSize = new Size(nWidth, 40);
            AutoScale(this);
        }

        public void InitialForm(TabControl tabControl)
        {
            #region 初始化 Tabpage

            Form formPage;

            ioInListViewForm = new CutIOInListViewForm();
            formPage = ioInListViewForm;
            AddTabPage("ioInListViewForm", "输入端口状态", formPage, tabControl);

            ioOutListViewForm = new CutIOOutListViewForm();
            formPage = ioOutListViewForm;
            AddTabPage("ioOutListViewForm", "输出端口状态", formPage, tabControl);            

            #endregion
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            parentOpcOperate = opcOperate;

            ioInListViewForm.InitialOpcButton();
            ioOutListViewForm.InitialOpcButton();
        }

        public void RefreshUIState()
        {
            readOpcAllData();
        }

        public void readOpcAllData( )
        {
            //refresh autoForm
            //TabPage tabpage = manualTabControl.SelectedTab;
            string tabPaeName = GetThreadTabName(tabControlIO);
            if (tabPaeName != null)
            {
                if (tabPaeName == "ioInListViewForm")
                {
                    ioInListViewForm.RefreshThreadIoState(parentOpcOperate);
                }
                else if (tabPaeName == "ioOutListViewForm")
                {                                                   
                    ioOutListViewForm.RefreshThreadIoState(parentOpcOperate);
                }
            }

        }

        public void StartWork()
        {
            threadWork = new Task(new Action(work));
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

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (refreshObject)
            {
                readOpcAllData();
            }
        }


        private void IOCutForm_SizeChanged(object sender, EventArgs e)
        {
            Form_SizeChanged(sender, e, tabControlIO, 40);
        }
    }
}
