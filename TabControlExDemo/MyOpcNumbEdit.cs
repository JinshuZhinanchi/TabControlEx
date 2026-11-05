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
    public partial class MyOpcNumbEdit : NumericUpDown
    {
        public int opcItemIndex = -1;
        public OpcOperateForm opcForm = null;
        public Boolean myFocused = false;

        delegate void SetValueCallback(NumericUpDown control, int nValue);

        

        public Boolean bLockUnchange = false;

        public MyOpcNumbEdit()
        {
            InitializeComponent();

            this.Enter += new EventHandler(textBox1_Enter);//获得焦点事件
            this.Leave += new EventHandler(textBox1_Leave);//失去焦点事件。
        }

        void textBox1_Enter(object sender, EventArgs e)
        {
            myFocused = true;
            //MessageBox.Show("获得了焦点");
        }
        void textBox1_Leave(object sender, EventArgs e)
        {
            myFocused = false;
            //MessageBox.Show("失去了焦点");
        }


        //解决包含自定义控件的刷新问题
        protected override CreateParams CreateParams
        {
            get

            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle &= ~0x02000000; // Turn off WS_EX_COMPOSITED

                return cp;

            }

        }

        public void SetThreadValue(NumericUpDown control, int nValue)
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
                        SetValueCallback d = new SetValueCallback(SetThreadValue);
                        this.Invoke(d, new object[] { control, nValue });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    if (control.Value != nValue)
                        control.Value = nValue;
                }
            }
        }

        public void RefreshOpcValue()
        {
            if (myFocused)
                return;
            //writpe opc  1
            if (opcForm == null || opcItemIndex < 0)
                return;
            Object ob = opcForm.GetItemValue(opcItemIndex);
            if( ob == null )
            {
                return;
            }

            Boolean block = bLockUnchange;
            bLockUnchange = true;
            
            int nValue = (Int32)ob;
            nValue = (int)(nValue * 100);
            //this.Value = nValue;
            SetThreadValue(this, nValue);

            bLockUnchange = block;
        }

        private void MyNumbEdit_ValueChanged(object sender, EventArgs e)
        {
            //writpe opc  1
            if (opcForm == null || opcItemIndex < 0)
                return;
            if (bLockUnchange)
                return;

            Int32 nValue  = (Int32)(this.Value/100);
            
            opcForm.WriteItemValue(opcItemIndex, nValue);
        }
    }
}
