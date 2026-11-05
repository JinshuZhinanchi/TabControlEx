using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using MyCommanLib;

namespace TabControlExDemo
{
    static class Program
    {        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //防止程序多次运行
            if (!OneInStance.IsFirst("TabControlExDemo"))
            {
                MessageBox.Show("警告:程序正在运行中! 请不要重复打开程序!", "程序错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CuttingForm());
        }

        /// <summary>
        /// write ini file
        /// PathName 是MarkEzd.dll所在的目录
        /// </summary>     
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);
        /// <summary>
        /// load ini file
        /// PathName 是MarkEzd.dll所在的目录
        /// </summary>     
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            int nSize, string lpFileName);

    }
}