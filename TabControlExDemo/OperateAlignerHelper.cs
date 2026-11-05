#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2024  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：DESKTOP-DHCL5FV
 * 公司名称：苏州首镭激光科技有限公司
 * 命名空间：TabControlExDemo
 * 唯一标识：42a69269-4740-4eeb-9f46-4a763d02733b
 * 文件名：OperateAlignerHelper
 * 
 * 创建者：卢磊
 * 电子邮箱：480357408@qq.com
 * 创建时间：2024/8/6 17:20:29
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using DemoCom;
using MyCommanLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TabControlExDemo
{
    /// <summary>
    /// OperateAlignerHelper 的摘要说明
    /// </summary>
    public class OperateAlignerHelper
    {
        #region <常量>
        #endregion <常量>

        #region <变量>

        public static string GZ_ORIG = "#1W0\r";
        public static string GZ_SETSIZE = "#1WAS6\r";
        public static string GZ_SETPOS0 = "#1WOP0\r";

        public static string GZ_TAKEPOS = "#1G12\r";
        public static string GZ_XIFU = "#1WUC\r";
        public static string GZ_FINDPOS = "#1G13\r";
        public static string GZ_WA = "#1WA\r";


        public static string GZ_ALIGN = "#1G17\r";

        public static string GZ_GTPOS = "#1WOP9000\r";
        public static string GZ_MTPOS = "#1G15\r";

        public static string GZ_TAKEPOS2 = "#1G14\r";
        public static string GZ_WDF = "#1WDF\r";

        public static string GZ_RD = "#1RD\r";

        public static string RECEIVE_ACK = "#1W0ACK\r";
        public static string FINISH_ACK = "s00INF";

        #endregion <变量>

        #region <属性>
        #endregion <属性>

        #region <构造方法和析构方法>
        #endregion <构造方法和析构方法>

        #region <方法>

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static Boolean OperateFindOrig(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            Reset(serialForm);

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;
            serialForm.OperateAction(GZ_ORIG);


            RECEIVE_ACK = "#1W0ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();
            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"没有收到命令响应,请准备好后按开始！{strRet}");

                Reset(serialForm);

                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 60000;//60s

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1W0INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"完成命令没有收到返回值,请准备好后按开始！{strRet}");

                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            SelectWaferPos(serialForm, 18000);

            return bRet;
        }

        /// <summary>
        /// 移动到接收晶圆的位置
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static Boolean OperateAlignerReady(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            Reset(serialForm);

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;
            serialForm.OperateAction(GZ_TAKEPOS);


            RECEIVE_ACK = "#1G12ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();
            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{GZ_TAKEPOS}命令返回值不正确, 请准备好后按开始！{strRet}");

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 60000;//60s

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1G12INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{GZ_TAKEPOS}完成命令返回值不正确, 请准备好后按开始！{strRet}");

                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 开始寻边 复合指令
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static Boolean OperateFindNotch(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            Reset(serialForm);

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;
            serialForm.OperateAction(GZ_ALIGN);


            RECEIVE_ACK = "#1G17ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                string info = $"{GZ_ALIGN.Trim()}命令返回值不正确,请准备好后按开始！{strRet}";
                //MessageBox.Show($"{GZ_ALIGN.Trim()}命令返回值不正确,请准备好后按开始！{strRet}");
                CommonDefine.SaveWorkLogs(info);

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 1000 * 60;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1G17INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                string info = $"{GZ_ALIGN.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}";
                //MessageBox.Show($"{GZ_ALIGN.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}");
                CommonDefine.SaveWorkLogs(info);
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 移动到取走晶圆位置
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static Boolean OperateAlignerEnd(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            Reset(serialForm);

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;
            serialForm.OperateAction(GZ_TAKEPOS2);


            RECEIVE_ACK = "#1G14ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{GZ_TAKEPOS2.Trim()}命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 60000;//60s

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1G14INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }
            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{GZ_TAKEPOS2.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 清除错误
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static Boolean Reset(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;

            string strCommond = GZ_RD;
            serialForm.OperateAction(strCommond);

            RECEIVE_ACK = "#1RDACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{strCommond.Trim()}命令返回值不正确,请准备好后按开始！{strRet.Trim()}");

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 1000 * 60;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1RDINF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{strCommond.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 选择寻边角度
        /// </summary>
        /// <param name="serialForm"></param>
        /// <param name="nPos"></param>
        /// <returns></returns>
        public static Boolean SelectWaferPos(SerialPortForm serialForm, int nPos)
        {
            Boolean bRet = true;

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;

            string strCommond = $"#1WOP{nPos}\r";
            serialForm.OperateAction(strCommond);

            RECEIVE_ACK = $"#1WOP{nPos}ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{strCommond.Trim()}ACK命令返回值不正确,请准备好后按开始！{strRet.Trim()}");

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 1000 * 60;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = $"#1WOP{nPos}INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{strCommond.Trim()}INF完成命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;


            return bRet;
        }

        /// <summary>
        /// 寻边器吸附
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static bool XIFU(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;

            string strCommond = GZ_XIFU;
            serialForm.OperateAction(strCommond);

            RECEIVE_ACK = "#1WUCACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{strCommond.Trim()}命令返回值不正确,请准备好后按开始！{strRet.Trim()}");

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 1000 * 60;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1WUCINF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{strCommond.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 寻边器关闭吸附
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static bool CloseXIFU(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            serialForm.EmptyReceiveBuffer();
            serialForm.serialPort.Synchronous = true;

            string strCommond = GZ_WDF;
            serialForm.OperateAction(strCommond);

            RECEIVE_ACK = "#1WDFACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"{strCommond.Trim()}命令返回值不正确,请准备好后按开始！{strRet.Trim()}");

                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 1000 * 60;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1WDFINF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"{strCommond.Trim()}完成命令返回值不正确,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 执行校准-旋转
        /// </summary>
        /// <param name="serialPortForm"></param>
        /// <returns></returns>
        public static bool OperateAlignerWA(SerialPortForm serialPortForm, int nPos)
        {
            if (Reset(serialPortForm) == false)
                return false;

            if (XIFU(serialPortForm) == false)
                return false;

            if (SelectWaferPos(serialPortForm, nPos) == false)
                return false;

            if (OperateMovePos(serialPortForm) == false)
                return false;

            serialPortForm.EmptyReceiveBuffer();
            serialPortForm.OperateAction(GZ_WA);

            serialPortForm.serialPort.Synchronous = true;

            RECEIVE_ACK = "#1WAACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialPortForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialPortForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialPortForm.GetLastResponse();
            serialPortForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialPortForm.serialPort.Synchronous = false;

                MessageBox.Show($"没有收到WA命令响应,请准备好后按开始！{strRet}");

                Reset(serialPortForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 60000;//60s

            while (serialPortForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialPortForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1WAINF";
            strRet = serialPortForm.GetLastResponse();
            serialPortForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }

            if (strRet2 != FINISH_ACK)
            {
                serialPortForm.serialPort.Synchronous = false;
                MessageBox.Show($"WA完成命令没有收到返回值,请准备好后按开始！{strRet}");
                Reset(serialPortForm);
                return false;
            }

            return true;

            serialPortForm.serialPort.Synchronous = false;
        }

        /// <summary>
        /// 执行校准-移动到传感器位置
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns></returns>
        public static bool OperateMovePos(SerialPortForm serialForm)
        {
            Boolean bRet = true;

            serialForm.EmptyReceiveBuffer();

            serialForm.OperateAction(GZ_FINDPOS);

            serialForm.serialPort.Synchronous = true;
            RECEIVE_ACK = "#1G13ACK";
            int nStartTick = Environment.TickCount;
            Boolean bReadFinish = false;
            int nSleep = 2000;

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            string strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            string strRet2 = strRet;
            if (strRet.Length >= RECEIVE_ACK.Length)
            {
                strRet2 = strRet.Substring(0, RECEIVE_ACK.Length);
            }

            if (strRet2 != RECEIVE_ACK)
            {
                serialForm.serialPort.Synchronous = false;

                MessageBox.Show($"没有收到#1G13命令响应,请准备好后按开始！{strRet}");
                Reset(serialForm);
                return false;
            }


            nStartTick = Environment.TickCount;
            bReadFinish = false;
            nSleep = 60000;//60s

            while (serialForm.GetLastResponseForLoadPort().Length <= 0)
            {
                serialForm.serialPort.synReadAllData();

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nSleep)
                {
                    bReadFinish = false;
                    break;
                }
            }

            FINISH_ACK = "#1G13INF";
            strRet = serialForm.GetLastResponse();
            serialForm.EmptyReceiveBuffer();

            strRet2 = strRet;
            if (strRet.Length >= FINISH_ACK.Length)
            {
                strRet2 = strRet.Substring(0, FINISH_ACK.Length);
            }
            if (strRet2 != FINISH_ACK)
            {
                serialForm.serialPort.Synchronous = false;
                MessageBox.Show($"#1G13完成命令没有收到返回值,请准备好后按开始！{strRet}");
                Reset(serialForm);

                return false;
            }

            serialForm.serialPort.Synchronous = false;

            return bRet;
        }

        /// <summary>
        /// 旋转-移动到下料位置
        /// </summary>
        /// <param name="serialPortForm"></param>
        /// <returns></returns>
        public static bool OperateFindNotch2(SerialPortForm serialPortForm, int nPos)
        {
            if (XIFU(serialPortForm) == false)
                return false;

            if (SelectWaferPos(serialPortForm, nPos) == false)
                return false;

            if (OperateAlignerEnd(serialPortForm) == false)
                return false;

            return true;
        }

        #endregion <方法>

        #region <事件>
        #endregion <事件>
    }
}