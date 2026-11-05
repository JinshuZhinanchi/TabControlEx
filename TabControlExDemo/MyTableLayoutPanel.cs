using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyCommanLib;

namespace TabControlExDemo
{
    public partial class MyTableLayoutPanel : TableLayoutPanel
    {
        public MyTableLayoutPanel()
        {
            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.CacheText, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= CommonDefine.WS_EX_COMPOSITED;
                return cp;
            }
        }

        public void BeginUpdate()
        {
            CommonDefine.SendMessage(this.Handle, CommonDefine.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        public void EndUpdate()
        {
            CommonDefine.SendMessage(this.Handle, CommonDefine.WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            Parent.Invalidate(true);
        }

    }
}
