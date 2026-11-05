using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MyCommanLib;

namespace TabControlExDemo
{
    public static class OperateSmifHelper
    {
 
        public static Boolean Initial(TCPForm tcpForm)
        {
            Boolean bRet = true;

            string strComm = TDKSmifOperate.AUTOREPLY;

            tcpForm.clientWrite(strComm);
            Thread.Sleep(500);

            strComm = TDKSmifOperate.ONMGV;
            tcpForm.clientWrite(strComm);

            Thread.Sleep(500);

            FindOrig(tcpForm);

            return bRet;
        }

        public static Boolean Reset(TCPForm tcpForm)
        {
            Boolean bRet = true;
            string strComm = TDKSmifOperate.RESET;
            tcpForm.clientWrite(strComm);

            Thread.Sleep(500);
            return bRet;
        }
        

        public static string FindOrig(TCPForm tcpForm)
        {
            string strReturn = "";

            string strComm = TDKSmifOperate.ORGSH;
            tcpForm.clientWrite(strComm);

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;
            string strType = GetReturnType(tcpForm, strReturn);
            if (strType != TDKSmifOperate.RECEIVE_ACK)
            {
                return strReturn;
            }


            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;
            maxDt = 40000;


            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;
            strType = GetReturnType(tcpForm, strReturn);
            if (strType != TDKSmifOperate.FINISH_ACK)
            {
                return strReturn;
            }

            strReturn = tcpForm.receiveString;

            return strReturn;
        }

        public static string  OpenSmif(TCPForm tcpForm)
        {
            string  strReturn = "";

            string strComm = TDKSmifOperate.CLDMP;
            tcpForm.clientWrite(strComm);

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;
            string strType = GetReturnType(tcpForm, strReturn);
            if (strType != TDKSmifOperate.RECEIVE_ACK)
            {
                return strReturn;
            }


            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;
            maxDt = 50000;


            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;
          
            return strReturn;
        }

        public static string CloseSmif(TCPForm tcpForm)
        {
            string  strReturn = "";
            string strComm = TDKSmifOperate.CUDMP;
            tcpForm.clientWrite(strComm);

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;
            string strType = GetReturnType(tcpForm, strReturn);
            if (strType != TDKSmifOperate.RECEIVE_ACK)
            {
                return strReturn;
            }


            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;
            maxDt = 50000;
            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            strReturn = tcpForm.receiveString;

            strType = GetReturnType(tcpForm, strReturn);
            if (strType != TDKSmifOperate.FINISH_ACK)
            {
                return strReturn;
            }

            return "";
           
        }


        public static string GetMappiingContent(TCPForm tcpForm)
        {
            string strMapping = "";

            string strComm = TDKSmifOperate.MAPRD;
            tcpForm.clientWrite(strComm);
            //返回：s00ACK:MAPRD/001010101010101010101010101;

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;
                     
            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
            }

            //s00ACK:MAPRD/1100000000000000000000000;
            strMapping = tcpForm.receiveString;            
            return strMapping;
        }

        public static string GetRFID(TCPForm tcpForm)
        {
            string strRFID = "";

            string strComm = TDKSmifOperate.CSTID;
            tcpForm.clientWrite(strComm);

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
            }

            strRFID = tcpForm.receiveString;

            return strRFID;
        }

        public static string TriggerPhoto(TCPForm tcpForm)
        {
            string strComm = "T2\r";
            tcpForm.clientWrite(strComm);

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
            }

             return tcpForm.receiveString;
        }

        public static string GetState(TCPForm tcpForm)
        {
            string strMapping = "";

            string strComm = TDKSmifOperate.STATE;
            tcpForm.clientWrite(strComm);
            //返回：s00ACK:MAPRD/001010101010101010101010101

            tcpForm.receiveString = "";
            tcpForm.m_bReceiveFinished = false;

            MyCommanLib.CommonDefine.TickSleep(500);

            Boolean bResult = false;
            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 5000;
            int dt = 0;

            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
            }

            strMapping = tcpForm.receiveString;

            return strMapping;
        }


        public static string GetReturnType( TCPForm tcpForm, string strReturn)
        {
            string strType = "";
            if( strReturn.Length > 6 )
            {
                strType = strReturn.Substring(0, 6);
            }

            return strType;
        }

        public static string GetReturnContent(TCPForm tcpForm,  string strReturn)
        {
            string strContent = "";
            if (strReturn.Length > 6)
            {
                //解析loadport返回数据
                string[] sArray = strReturn.Split('/');
                if( sArray.Length > 1 )
                {
                    strContent = sArray[1];
                }                
            }
            return strContent;
        }

    }
}
