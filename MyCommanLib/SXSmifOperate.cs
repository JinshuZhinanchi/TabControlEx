using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommanLib
{
    class SXSmifOperate
    {
        //打开软件-- RESET -- INITIAL -- ORGSH（初始化）-- IO判断状态 --- CLDMP --- CUDMP   
        /// <summary>
        ///自动流程用到的 指令======================================================================== 
        /// </summary>
        /// 

        //上电执行
        public static string START = "00MOV:START;";//LP START
        public static string ORGSH = "00MOV:ORGSH;";//回到初始位置
        public static string CLOAD = "00MOV:CLOAD;";//功能:打开SMIFPod并MAP扫描晶圆状态.在执行该指令前要先完
                                                       // 成map参数设置，和map种类的选择.该指令执行的动作包含，打
                                                       //开SMIFPod 底部Base，Z轴带着装载的Cassette从Home位下移
                                                       //至Z轴原点处即Stage位（晶圆可以进行取放工位），并在下移的过程中进行MAP扫描，计算扫描结果

        public static string CULOD = "00MOV:CULOD;"; //功能:关闭SMIFPod并MAP扫描晶圆状态.在执行该指令前要先完
                                                        //成map参数设置，和map种类的选择.该指令执行的动作包含，从
                                                        //stage 上移到home位置(即SMIFPod初始被放置的位置)，并关闭 SMIFPod 底部Base.

        //end 自动流程调用指令=====================================================================


        //MOV 指令        
        public static string PODOP = "00MOV:PODOP;";//LP Locked
        public static string PODCL = "00MOV:PODCL;";//LP UnLocked
        public static string RETRY = "00MOV:RETRY;";//Retry during recoverable error.               

        //SET 指令
        public static string RESET = "00SET:RESET;";//清楚报警
        public static string INITL = "00SET:INITL;";//初始化状态

        //GET 指令
        public static string STATE = "00SET:STATE;";
        public static string LEDST = "00SET:LEDST;";


        public static string RFID1 = "00SET:RFID1;";//Reports the POD_ID.
        public static string RFID2 = "00SET:RFID2;";//Reports the Cassette_ID.

        public static string MAPRD = "00SET:MAPRD;";//Wafer data (ascending order).
                
        public static string GetCheckSum(string strComm)
        {
            string strSum = "00";
            int nLen = 2 + strComm.Length + strSum.Length;

            int sum = nLen;
            Char ch;
            Char[] array = strComm.ToArray();
            for(int i = 0; i < array.Length; i++)
            {
                sum = sum + array[i];
            }

            int checkSum = sum & 0xff;

            int hs = checkSum & 0xf0;
            int ls = checkSum & 0x0f;


            strSum = hs.ToString("X") + ls.ToString("X");

            return strSum;
        }

        public static Char[] GetCommond(string strComm, int nType)
        {
            string checkSum = GetCheckSum(strComm);

            int nLen = 1 +  2 + strComm.Length + checkSum.Length + 1;

            Char[] commBuf = new Char[nLen];

            string strComm2 = strComm + checkSum;

            Char[] commArray = strComm2.ToArray();

            commBuf[0] =  (Char)(0x01);
            commBuf[1] = (Char)(nLen & 0xff);
            commBuf[2] = (Char)(nLen & 0xff00);

            for (int i = 0; i < commArray.Length; i++ )
            {
                commBuf[i + 2] = commArray[i];
            }

            commBuf[nLen] = (Char)(0x03);

            return commArray;
        }


        //返回结果
        public static string RECEIVE_ACK = "00ACK";
        public static string FINISH_ACK = "00INF";
    }
}
