using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    internal class HOOK
    {
        private static int hHook = 0;
        private static Form HookControlForm;
        private static HookProc MouseHookProcedure = new HookProc(HOOK.HOOKProcReturn); // 不要定义在方法里面。执行几次后会被回收；切记！！！！

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// GetForegroundWindow
        ///  　函数功能：该函数返回前台窗口（用户当前工作的窗口）。系统分配给产生前台窗口的线程一个稍高一点的优先级。
        ///  　函数原型：HWND GetForegroundWindow（VOID）
        ///  　参数：无。
        ///  　返回值：函数返回前台窗回的句柄。
        ///  　速查：Windows NT：3.1以上版本；Windows：95以上版本：Windows CE：1.0以上版本：头文件：Winuser.h；库文件：user32.lib。
        ///  　摘要：
        ///  　函数功能：该函数返回前台窗口（用户当前工作的窗口）。系统分配给产生前台窗口的线程一个稍高一点的优先级。
        ///  　函数原型：HWND GetForegroundWindow（VOID）
        ///  　参数：无。
        ///  　返回值：函数返回前台窗回的句柄。
        ///  　速查：Windows NT：3.1以上版本；Windows：95以上版本：Windows CE：1.0以上版本：头文件：Winuser.h；库文件：user32.lib。
        ///
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// GetWindowThreadProcessId这个函数来获得窗口所属进程ID和线程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, int ID);


        /// <summary>
        /// 消息处理中心
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static int HOOKProcReturn(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //鼠标右键
                if (wParam.ToInt32() == 0x205)
                {
                    IntPtr intpr = GetForegroundWindow();
                    if ((intpr == HookControlForm.Handle))
                    {
                        MessageBox.Show("ok- 0x205");

                    }
                }
                else if (wParam.ToInt32() == 0x203)
                {
                    if (GetForegroundWindow() == HookControlForm.Handle)
                    {
                        MessageBox.Show("ok- 0x203");
                    }
                }//鼠标左键
                else if (wParam.ToInt32() == 0x201)
                {
                    if (GetForegroundWindow() == HookControlForm.Handle)
                        MessageBox.Show("ok- 0x201");
                    //结束监听
                    HOOK.StopHook();

                }
                else if (wParam.ToInt32() == 0xa1)
                {
                    if ((GetForegroundWindow() == HookControlForm.Handle))
                    {
                        MessageBox.Show("ok- 0xa1");
                    }
                }
                else if (wParam.ToInt32() == 0x202)
                {
                    //MessageBox.Show("ok- 0x202");
                }
                else if (wParam.ToInt32() == 0x200)
                {
                    //  MessageBox.Show("ok-0x200");
                }
            }
            //监听下一次事件
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        public static bool StartHook(Form HookControl)
        {
            HookControlForm = HookControl;
             MouseHookProcedure = new HookProc(HOOK.HOOKProcReturn);
            //监听 事件 发送消息
            hHook = SetWindowsHookEx((int)HookType.WH_MOUSE, MouseHookProcedure, IntPtr.Zero, GetWindowThreadProcessId(HookControlForm.Handle, 0));
            if (hHook == 0)
            {
                return false;
            }
            return true;
        }

        public static bool StopHook()
        {
            return UnhookWindowsHookEx(hHook);
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public HOOK.POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// 钩子类型舰艇鼠标键盘等事件
        /// </summary>
        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,//局部 线程级别
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14 //全局
        }

    }
}
