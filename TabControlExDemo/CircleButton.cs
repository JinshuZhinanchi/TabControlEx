using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public class EllipseButton : Button
    {
        public int opcItemIndex = -1;
        delegate void SetStateCallback(Control control, Color clr);

        public void SetThreadState(Control control, Color clr)
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
                        SetStateCallback d = new SetStateCallback(SetThreadState);
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
                SetThreadState(this, PLCOpcFeedParameters.runColor);               
            }
            else
            {
                SetThreadState(this, PLCOpcFeedParameters.disableColor);
            }

            opcOperate.SetItemPreValue(opcItemIndex, ob);                      
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //if(opcItemIndex >= 0)
            {
                GraphicsPath gPath = new GraphicsPath();
                // 绘制椭圆形区域
                gPath.AddEllipse(0, 0, this.ClientSize.Width, this.ClientSize.Height);

                // 将区域赋值给Region
                this.Region = new System.Drawing.Region(gPath);
            }

            base.OnPaint(pevent);
        }
    }

}
