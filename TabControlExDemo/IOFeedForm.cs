using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using gts;
using jog;

namespace TabControlExDemo
{
    public partial class IOFeedForm : Form
    {
        delegate void SetBackColorCallback(Control control, Color clr);

      
        EllipseButton[] IOInButtons = new EllipseButton[16];
        EllipseButton[] IOOutButtons = new EllipseButton[16];

        EllipseButton[] IOEXTInButtons1 = new EllipseButton[16];

        public IOFeedForm()
        {
            InitializeComponent();

            int nIndex = 0;
            IOInButtons[0] = buttonIOIn1;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn2;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn3;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn4;

            nIndex++;
            IOInButtons[nIndex] = buttonIOIn5;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn6;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn7;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn8;

            nIndex++;
            IOInButtons[nIndex] = buttonIOIn9;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn10;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn11;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn11;

            nIndex++;
            IOInButtons[nIndex] = buttonIOIn13;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn14;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn15;
            nIndex++;
            IOInButtons[nIndex] = buttonIOIn16;


            nIndex = 0;
            IOOutButtons[0] = buttonIOOut1;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut2;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut3;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut4;

            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut5;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut6;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut7;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut8;

            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut9;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut10;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut11;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut12;

            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut13;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut14;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut15;
            nIndex++;
            IOOutButtons[nIndex] = buttonIOOut16;


            nIndex = 0;
            IOEXTInButtons1[0] = buttonExtIOIN1;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN2;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN3;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN4;

            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN5;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN6;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN7;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN8;

            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN9;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN10;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN11;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN12;

            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN13;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN14;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN15;
            nIndex++;
            IOEXTInButtons1[nIndex] = buttonExtIOIN16;
        }


        private void SetThreadBackColor(Control control, Color clr)

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
                        SetBackColorCallback d = new SetBackColorCallback(SetThreadBackColor);
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

        public void RefreshState()
        {
            short sRtn = 0;
            int lGpiValue = 0;
            int nEnableDi = 0;

            for (int i = 0; i < 16; i++)
            {
                sRtn = gts.mc.GT_GetDi(gts.mc.MC_GPI, out lGpiValue);

                Color bkColor;
                //16 IO  开始
                if ((lGpiValue & (1 << i)) > nEnableDi)//高电平
                {
                    bkColor = Color.Green;
                }
                else
                {
                    bkColor = Color.Gray;
                }
                SetThreadBackColor(IOInButtons[i], bkColor);
            }

            int nEnableDo = 0;
            for (int i = 0; i < 16; i++)
            {
                //输出
                sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lGpiValue);

                Color bkColor;
                if ((lGpiValue & (1 << i)) == nEnableDo)//高电平
                {
                    bkColor = Color.Green;
                    //Console.WriteLine("按键已经按下，不检测\n");
                }
                else
                {
                    bkColor = Color.Gray;
                }
                SetThreadBackColor(IOOutButtons[i], bkColor);
            }

            short mdl = GLinkIOModule01.slaveNO;
            ushort extValue;
            for (int i = 0; i < 16; i++)
            {
                sRtn = gts.mc.GT_GetExtIoValue(mdl, out extValue);

                Color bkColor;
                //16 IO  开始
                if ((extValue & (1 << i)) == 0)//高电平
                {
                    bkColor = Color.Green;
                }
                else
                {
                    bkColor = Color.Gray;
                }
                SetThreadBackColor(IOEXTInButtons1[i], bkColor);
            }

        }

        public short SetDo(int ioIndex)
        {
            short sRtn = 0;
            int lpValue = 0;
            sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lpValue);

            if ((lpValue & (1 << ioIndex)) == 0)
            {
                lpValue = (lpValue | (1 << ioIndex));
            }
            else
            {
                lpValue = (lpValue & ~(1 << ioIndex));
            }

            sRtn = gts.mc.GT_SetDo(gts.mc.MC_GPO, lpValue);

            return sRtn;
        }
      

        private void buttonYellowOut_Click(object sender, EventArgs e)
        {
            int ioIndex = 0;
            SetDo( ioIndex);                  
        }

        private void buttonRedOut_Click(object sender, EventArgs e)
        {
            int ioIndex = 1;
            SetDo(ioIndex);    
        }

        private void buttonGreenOut_Click(object sender, EventArgs e)
        {
            int ioIndex = 2;
            SetDo(ioIndex);    
        }

        private void buttonBellingOut_Click(object sender, EventArgs e)
        {
            int ioIndex = 3;
            SetDo(ioIndex);    
        }
      

        private void buttonBlueOut_Click(object sender, EventArgs e)
        {
            int ioIndex = 4;
            SetDo(ioIndex);
        }

        private void buttonIOOut6_Click(object sender, EventArgs e)
        {
            int ioIndex = 5;
            SetDo(ioIndex);
        }

        private void buttonIOOut7_Click(object sender, EventArgs e)
        {
            int ioIndex = 6;
            SetDo(ioIndex);
        }

        private void buttonIOOut8_Click(object sender, EventArgs e)
        {
            int ioIndex = 7;
            SetDo(ioIndex);
        }

        private void buttonIOOut9_Click(object sender, EventArgs e)
        {
            int ioIndex = 8;
            SetDo(ioIndex);
        }

        private void buttonIOOut10_Click(object sender, EventArgs e)
        {
            int ioIndex = 9;
            SetDo(ioIndex);
        }

        private void buttonIOOut11_Click(object sender, EventArgs e)
        {
            int ioIndex = 10;
            SetDo(ioIndex);
        }

        private void buttonIOOut12_Click(object sender, EventArgs e)
        {
            int ioIndex = 11;
            SetDo(ioIndex);
        }

        private void buttonIOOut13_Click(object sender, EventArgs e)
        {
            int ioIndex = 12;
            SetDo(ioIndex);
        }

        private void buttonIOOut14_Click(object sender, EventArgs e)
        {
            int ioIndex = 13;
            SetDo(ioIndex);
        }

        private void buttonIOOut15_Click(object sender, EventArgs e)
        {
            int ioIndex = 14;
            SetDo(ioIndex);
        }

        private void buttonIOOut16_Click(object sender, EventArgs e)
        {
            int ioIndex = 15;
            SetDo(ioIndex);
        }
    }
}
