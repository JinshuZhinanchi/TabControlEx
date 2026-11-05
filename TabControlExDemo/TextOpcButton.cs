using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class TextOpcButton : Button
    {
        public int opcItemIndex = -1;
        public OpcOperateForm opcForm = null;
        
        delegate void SetStateCallback(Control control, string strText);

        public TextOpcButton()
        {
            InitializeComponent();
        }
               

        public void SetThreadText(Control control, string strText)
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
                        SetStateCallback d = new SetStateCallback(SetThreadText);
                        this.Invoke(d, new object[] { control, strText });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    control.Text = strText;
                    if (strText == "已屏蔽")
                    {
                        control.ForeColor = System.Drawing.Color.Green;
                    } else
                    {
                        control.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        public void RefreshOpcValue(OpcOperateForm opcOperate)
        {
            //writpe opc  1
            if (opcOperate == null || opcItemIndex < 0)
                return;
            Object ob = opcOperate.GetItemValue(opcItemIndex);
            if (ob == null)
                return;


            Boolean bValue = (Boolean)ob;
            if (bValue)
            {
                SetThreadText(this, "已屏蔽");
            }
            else
            {
                SetThreadText(this, "未屏蔽");
            }

            opcOperate.SetItemPreValue(opcItemIndex, ob);


        }

        private void TextOpcButton_MouseDown(object sender, MouseEventArgs e)
        {
            Boolean bValue = true;


            if (opcForm != null && opcItemIndex > 0)
            {
                bValue = (Boolean)opcForm.GetItemValue(opcItemIndex);
                
                opcForm.WriteItemValue(opcItemIndex, !bValue);
                RefreshOpcValue(opcForm);
            }           
        }
    }
}
