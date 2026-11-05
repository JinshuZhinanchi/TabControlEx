using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class IOForm : BaseForm
    {
        CutIOInListViewForm iOInForm;
        CutIOOutListViewForm iOOutForm;
        
        public IOForm()
        {
            InitializeComponent();
        }


        private void Form_Load(object sender, EventArgs e, TabControl tabControl)
        {
            #region 初始化 Tabpage

            Form formPage;

            iOInForm = new CutIOInListViewForm();
            formPage = iOInForm;
            AddTabPage("iOInForm", "输入端口", formPage, tabControl);


            iOOutForm = new CutIOOutListViewForm();
            formPage = iOOutForm;
            AddTabPage("iOOutForm", "输出端口", formPage, tabControl);
            #endregion

            AutoScale(this);
            int nWidth = ((this.Width - 30) / ioTabControl.TabPages.Count);
            ioTabControl.ItemSize = new Size(nWidth, 60);
        }

        private void IOForm_Load(object sender, EventArgs e)
        {
            Form_Load(sender, e, ioTabControl);
        }

        private void IOForm_SizeChanged(object sender, EventArgs e)
        {
            Form_SizeChanged(sender, e, ioTabControl,40);
        }
    }
}
