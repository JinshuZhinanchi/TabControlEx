using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EzdDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DateTime dTime1 = new DateTime(2019,01,27);
            DateTime dTime2 = new DateTime(2039, 8, 17);
            DateTime wTime3 = new DateTime(2039, 9, 25);
            DateTime cTime = DateTime.Now;
            if (cTime < dTime1 || cTime > dTime2)
            {
               // MessageBox.Show("发生系统问题，请联系开发商！");
               // return;
            }
            if (cTime > wTime3)
            {
               // MessageBox.Show("发生系统问题，请联系开发商！");                
            }
            Application.Run(new MarkForm());
        }
    }
}