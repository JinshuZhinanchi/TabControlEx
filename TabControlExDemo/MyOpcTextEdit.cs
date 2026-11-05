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
    public partial class MyOpcTextEdit : TextBox
    {
        delegate void SetTextCallback(Control control, Color clr, string text);

        public int opcItemIndex = -1;
        public OpcOperateForm opcForm = null;
        public Boolean myFocused = false;

        public Boolean bLockUnchange = false;

        public int m_nOpcValueType = 1;//0:short     1: long
        public int m_nTextValueType = 1;//0:int  1;double

        public double m_fRate = 1000.0;

        public MyOpcTextEdit()
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
                    if( control.Text != text)
                        control.Text = text;
                }
            }
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

        public void RefreshOpcValue()
        {
            if (myFocused)
                return;
            //writpe opc  1
            if (opcForm == null || opcItemIndex < 0)
                return;
            Object ob = opcForm.GetItemValue(opcItemIndex);
            if (ob == null)
                return;

            Boolean block = bLockUnchange;
            bLockUnchange = true;

            Color clr = Color.Black;
            long nValue = 0;
            if(m_nOpcValueType == 1 )
            {
                nValue = (int)ob;
                double fValue = nValue;
                fValue = fValue / m_fRate;
                //this.Text = string.Format("{0:F2}", fValue);
                string text = string.Format("{0:F3}", fValue);
                SetThreadText(this, clr, text);

            }
            else
            {
                nValue = (short)ob;
                //this.Text = nValue.ToString();
                string text = nValue.ToString();
                SetThreadText(this, clr, text);
            }
            
            bLockUnchange = block;
        }

        private void MyOpcTextEdit_TextChanged(object sender, EventArgs e)
        {
            //writpe opc  1
            if (opcForm == null || opcItemIndex < 0)
               return;
            if (bLockUnchange)
                return;

            double fValue = Convert.ToDouble(this.Text);
            int nValue = (int)(fValue * m_fRate);

            object ob;
            if(m_nOpcValueType == 1 )
            {
                ob = (long)nValue;
            }else
            {
                ob = (short)nValue;
            }
            opcForm.WriteItemValue(opcItemIndex, ob);
        }

    }
}
