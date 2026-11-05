using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EzdDemo
{
    public class MachineParam
    {
        #region 常量--窗口信息
        private const int WM_User = 0x0400;
        public const int wm_event = WM_User + 100;
        public const int wm_alarm = WM_User + 200;
        public const int wm_image = WM_User + 300;
        #endregion

        //IO 常量 In
        //public const UInt16 IOinLBRES = 0x00000001;  //0    打标机初始化
        //public const UInt16 IOinLBSTART = 0x00000002; //1	打标机启动
        //public const UInt16 IOinLBSTOP = 0x00000004; //2	打标机急停


        //public const UInt16 IOOutLBREADY = 0x00000001; //0	打标机就绪
        //public const UInt16 IOOutLBALM = 0x00000002; //1	打标机异常 
        //public const UInt16 IOOutLBOK = 0x00000004; //2	打标OK
        //public const UInt16 IOOutLBNG = 0x00000008;//3	打标NG

        //IO-In
        public const int IOinLBRES = 1;  //1    打标机初始化
        public const int IOinLBSTART = 2; //2	打标机启动
        public const int IOinLBSTOP = 3; //3	打标机急停

        //IO-Out
        public const int IOOutLBREADY = 1; //1	打标机就绪
        public const int IOOutLBALM = 2; //2	打标机异常 
        public const int IOOutLBOK = 3; //3	打标OK
        public const int IOOutLBNG = 4;//4	打标NG



        public delegate void Tmsgevent(int msg, int val);
        public delegate void Ttcpsocket(int device, string barcode, int errorcode);
        public delegate void Tdevevent(int msg, int val);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);
    }
}
