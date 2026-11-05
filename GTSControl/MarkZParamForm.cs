using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyCommanLib;

namespace jog
{
    public partial class MarkZParamForm : Form
    {
        delegate void SetColorCallback(Control control, Color bkColor);

        public GtsControl m_gtsControl;
        int m_nLaserType = 0;
        short m_nCurrentAxis = AxisTestForm.MARKZ_AXIS;


        public MarkZParamForm( GtsControl gtsControl)
        {
            InitializeComponent();
            m_gtsControl = gtsControl;            
        }     
        internal void AxisData2UI()
        {          
            textBoxSafePos.Text = CommonDefine.waferFileInfo.axisData.safePoint.fz.ToString("F4");
            textBoxMarkPos.Text = CommonDefine.waferFileInfo.axisData.workPoint.fz.ToString("F4");
            textBoxPhotoPos.Text = CommonDefine.waferFileInfo.axisData.workPoint2.fz.ToString("F4");

        }

        internal void UI2AxisData()
        {
            int nLaserType = m_nLaserType;
            double fValue = 0;
            double.TryParse(textBoxSafePos.Text, out fValue);
            CommonDefine.waferFileInfo.axisData.safePoint.fz = fValue;

            double.TryParse(textBoxMarkPos.Text, out fValue);
            CommonDefine.waferFileInfo.axisData.workPoint.fz = fValue;

            double.TryParse(textBoxPhotoPos.Text, out fValue);
            CommonDefine.waferFileInfo.axisData.workPoint2.fz = fValue;
        }

        public string SetThreadTextBox(TextBox tabControl , string  strText)
        {
            string retString = null;

            if (tabControl.InvokeRequired)
            {
                return (string)tabControl.Invoke(
                  new Func<String>(() => SetThreadTextBox(tabControl , strText))
                );
            }
            else
            {
                if (tabControl != null)
                {
                    tabControl.Text = strText;
                    retString = strText;
                }
            }
            //this.BeginInvoke(dl, "delegate three"); // 异步调用委托,调用后立即返回并立即执行下面的语句
            //this.Invoke(getName, tabControl, retString); // 等待工作线程完成后, 才接着执行下面的语句.
            return retString;
        }

        public Boolean SetThreadStateColor(TextBox tabControl, Color clr)
        {
            Boolean bState = false;

            if (tabControl.InvokeRequired)
            {
                return (Boolean)tabControl.Invoke(
                  new Func<Boolean>(() => SetThreadStateColor(tabControl, clr))
                );
            }
            else
            {
                if (tabControl != null)
                {
                    tabControl.BackColor = clr;
                    bState = true;
                }
            }            
            return bState;
        }

        private void buttonJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogAdd(m_nCurrentAxis);
        }

        private void buttonJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(m_nCurrentAxis);
        }

        private void buttonJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveJogMinus(m_nCurrentAxis);
        }

        private void buttonJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            m_gtsControl.MoveStop(m_nCurrentAxis);
        }

        private void buttonOrig_Click(object sender, EventArgs e)
        {
            m_gtsControl.FindHome(m_nCurrentAxis, true);


        }

        private void buttonSafePos_Click(object sender, EventArgs e)
        {
            double fValue = 0;
            double.TryParse(textBoxSafePos.Text, out fValue);
            m_gtsControl.P2PMove(m_nCurrentAxis, fValue);
        }

        private void buttonPhotoPos_Click(object sender, EventArgs e)
        {
            double fValue = 0;
            double.TryParse(textBoxPhotoPos.Text, out fValue);
            m_gtsControl.P2PMove(m_nCurrentAxis, fValue);
        }

        private void buttonMarkPos_Click(object sender, EventArgs e)
        {
            double fValue = 0;
            double.TryParse(textBoxMarkPos.Text, out fValue);
            m_gtsControl.P2PMove(m_nCurrentAxis, fValue);
        }

        private void buttonSafePosSave_Click(object sender, EventArgs e)
        {                        
            textBoxSafePos.Text = textBoxCurrentPos.Text;
        }

        private void buttonPhotoPosSave_Click(object sender, EventArgs e)
        {
            textBoxPhotoPos.Text = textBoxCurrentPos.Text;
        }

        private void buttonMarkPosSave_Click(object sender, EventArgs e)
        {
            textBoxMarkPos.Text = textBoxCurrentPos.Text;
        }
        gts.mc.THomeStatus homeStatus;
        internal void RefreshUI()
        {
            double fValue = m_gtsControl.GetCurrentPos(m_nCurrentAxis);
            SetThreadTextBox(textBoxCurrentPos, fValue.ToString("F4"));

            gts.mc.GT_GetHomeStatus(m_nCurrentAxis, out homeStatus);

            if (homeStatus.run == 0 && homeStatus.error == 0)
            {
                SetThreadBkColor(stateOrig, Color.Green);
            }
            else
            {
                SetThreadBkColor(stateOrig, Color.Gray);
            }
        }
        private void SetThreadBkColor(Control control, Color bkColor)
        {
            if (control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                try
                {
                    SetColorCallback d = new SetColorCallback(SetThreadBkColor);
                    this.Invoke(d, new object[] { control, bkColor });
                }
                catch
                {
                }
            }
            else
            {
                {
                    control.BackColor = bkColor;
                }
            }
        }
        
    }
}
