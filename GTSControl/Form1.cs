using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace jog
{
    //例程6-2 Jog运动
    public partial class Form1 : Form
    {
        short axis;
        bool[] en=new bool[8];
        uint clk;
        double vel, prfvel, encvel, prfpos, encpos;
        gts.mc.TJogPrm jog;
        public Form1()
        {
            InitializeComponent();
        }
        //
        //加载界面
        //
        private void Form1_Load(object sender, EventArgs e)
        {
            gts.mc.GT_Open(0,1);//打开运动控制卡
            this.comboBox1.SelectedIndex = 0;

        }
        //
        //复位
        //
        private void button6_Click(object sender, EventArgs e)
        {
            gts.mc.GT_Reset();
        }
        //
        //初始化
        //
        private void button1_Click(object sender, EventArgs e)
        {
            gts.mc.GT_LoadConfig("GT800_test.cfg");//下载配置文件
            gts.mc.GT_ClrSts(1,8);//清除各轴报警和限位
            gts.mc.GT_PrfJog(axis);//设置为jog模式
            
        }
        //
        //伺服使能
        //
        private void button5_Click(object sender, EventArgs e)
        {
            axis = Convert.ToInt16(this.comboBox1.SelectedIndex + 1);
                if (!en[axis-1])
                {
                    gts.mc.GT_AxisOn(axis);//上伺服
                    this.button5.Text = "伺服关闭";
                }
                if (en[axis-1])
                {
                    gts.mc.GT_AxisOff(axis);//下伺服
                    this.button5.Text = "伺服使能";
                }
                en[axis - 1] = !en[axis - 1];
          
        }
        //
        //刷新状态
        //
        private void timer1_Tick(object sender, EventArgs e)
        {
            axis = Convert.ToInt16(this.comboBox1.SelectedIndex + 1);//读取轴号

            if (!en[Convert.ToInt32(this.comboBox1.Text)-1])
            {
                this.button5.Text = "伺服使能";
            }
            if (en[Convert.ToInt32(this.comboBox1.Text)-1])
            {
                this.button5.Text = "伺服关闭";
            }
            //读取规划位置并显示
            gts.mc.GT_GetPrfPos(axis,out prfpos,1,out clk);
            this.textBox6.Text = Math.Round(prfpos,1).ToString();
            //读取规划速度并显示
            gts.mc.GT_GetPrfVel(axis,out prfvel,1,out clk);
            this.textBox7.Text = Math.Round(prfvel, 6).ToString();
            //读取实际位置并显示
            gts.mc.GT_GetEncPos(axis,out encpos,1,out clk);
            this.textBox8.Text = Math.Round(encpos, 1).ToString();
            //读取实际速度并显示
            gts.mc.GT_GetEncVel(axis,out encvel,1,out clk);
            this.textBox9.Text = Math.Round(encvel, 6).ToString();
        }
        //
        //位置清零
        //
        private void button4_Click(object sender, EventArgs e)
        {
            gts.mc.GT_ZeroPos(1,8);
        }
        //
        //按键按下启动正向jog运功
        //
        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            ///读取参数
            vel = Convert.ToDouble(this.textBox1.Text);
            jog.acc = Convert.ToDouble(this.textBox3.Text);
            jog.dec = Convert.ToDouble(this.textBox4.Text);

            

            gts.mc.GT_SetJogPrm(axis, ref jog);//设置jog运动参数

            gts.mc.GT_SetVel(axis, -vel);//设置目标速度

            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动
        }
        //
        //按键抬起停止正向jog运功
        //
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            gts.mc.GT_Stop(1 << (axis - 1), 0);
        }
        //
        //按键按下启动负向jog运功
        //
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            vel = Convert.ToDouble(this.textBox1.Text);
            jog.acc = Convert.ToDouble(this.textBox3.Text);
            jog.dec = Convert.ToDouble(this.textBox4.Text);

           

            gts.mc.GT_SetJogPrm(axis, ref jog);//设置jog运动参数

            gts.mc.GT_SetVel(axis, vel);//设置目标速度

            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动
        }
        //
        //按键抬起停止负向jog运功
        //
        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            gts.mc.GT_Stop(1 << (axis - 1), 0);
        }

        
    }
}
