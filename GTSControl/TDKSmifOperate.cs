using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotControl
{
    public class TDKSmifOperate
    {
        //打开软件--ONMGV（Online模式） -- ORGSH（初始化）-- IO判断状态 --- CLDMP --- CUDMP   
        /// <summary>
        ///自动流程用到的 指令======================================================================== 
        /// </summary>
        public static string READY      = "s00MOV:READY;\r";//Ready指令 报告Ready 信息   ？？？ 

        //上电执行
        public static string ORGSH = "s00MOV:ORGSH;\r";//回到初始位置

        public static string CLDMP = "s00MOV:CLDMP;\r";//功能:打开SMIFPod并MAP扫描晶圆状态.在执行该指令前要先完
                                                       // 成map参数设置，和map种类的选择.该指令执行的动作包含，打
                                                       //开SMIFPod 底部Base，Z轴带着装载的Cassette从Home位下移
                                                       //至Z轴原点处即Stage位（晶圆可以进行取放工位），并在下移的过程中进行MAP扫描，计算扫描结果

        public static string CUDMP = "s00MOV:CUDMP;\r"; //功能:关闭SMIFPod并MAP扫描晶圆状态.在执行该指令前要先完
                                                        //成map参数设置，和map种类的选择.该指令执行的动作包含，从
                                                        //stage 上移到home位置(即SMIFPod初始被放置的位置)，并关闭 SMIFPod 底部Base.

        //end 自动流程调用指令=====================================================================
        //MOV 指令
        public static string ORGJZ      = "s00MOV:ORGJZ;\r";//仅执行Z轴搜索原点。务必保证SMIFPod底部base已经打开即DOROP或ORGJL指令已执行完,
        public static string ORGJL      = "s00MOV:ORGJL;\r";//仅执行开关盒电机L轴搜索原点，务必保证平台移动到最顶上位置

        public static string DOROP      = "s00MOV:DOROP;\r";//仅执行SMIFPod底部base打开.在Z轴运动前都需要完成SMIFPod 底部base打开，否则Z轴无法正常运动。此命令在online       模式下要求SMIFPod必须在位才可以进行。
        public static string DORCL      = "s00MOV:DOROP;\r";//功能:仅执行SMIFPod底部base关闭.执行此命令时，要保证初始       化完成，且Z轴处于home位置

        public static string HOME_      = "s00MOV:HOME_;\r";//执行Z轴上移到home位置。从stage上移到home位置(即SMIFPod初始被放置的位置).此位置可以通过SET:H####指令设置.

        public static string STAGE      = "s00MOV:STAGE;\r";//功能:执行Z轴下移到stage位置。从home位置下移到stage位置(即晶圆可以进行取放工位)

        //public static string TEACH      = "s00MOV:TEACH;\r";//执行晶圆和Cassette示教功能.当搬运的晶圆出现新的类型
                                                            // 时(如厚度/材质/Cassette规格)变化时,需要设定新的晶圆种类编号,
                                                            //并对此类型进行示教.示教前需先设置Map种类编号(SET:MAP##)

        public static string MAPDO = "s00MOV:MAPDO;\r";//功能:执行 Mapping 动作.需先选择设置与此类晶圆相对应的
                                                       // Mapping 种类(SET:MTY##),才可以正常进行,否则Mapping 结果可
                                                       //能有误差.务必保证所扫描的晶圆与选择的种类的一致.
                                                               

        public static string DZJOG      = "s00MOV:DZJOG;\r";//Z轴电机向下做JOG运动
        public static string UZJOG      = "s00MOV:UZJOG;\r";//Z轴电机向上做JOG运动


        //SET 指令
        public static string RESET      = "s00SET:RESET;\r";//清楚报警
        public static string SetHomeDistance(int nDis)
        {
            string strComm = string.Format("s00SET:H{0:D4}\r", nDis);//0.1mm
            return strComm;
        }

        public static string SetWaferType(int nType)
        {
            string strComm = string.Format("s00SET:WTY{0:D2}\r", nType);//
            return strComm;
        }

        public static string SetWaferMaxThick(int nThick)
        {
            string strComm = string.Format("s00SET:M{0:D4}\r", nThick);//um
            return strComm;
        }

        public static string SetWaferMinThick(int nThick)
        {
            string strComm = string.Format("s00SET:N{0:D4}\r", nThick);//um
            return strComm;
        }

        public static string SetWafeRange(int nThick)
        {
            string strComm = string.Format("s00SET:G{0:D4}\r", nThick);//um
            return strComm;
        }

        public static string SetWafeRFID(int nThick)
        {
            string strComm = string.Format("s00SET:CSTID\r");//um
            return strComm;
        }

        //MOD 指令
        public static string ONMGV = "s00MOD:ONMGV;\r";//切换为Online模式
        public static string MENTE = "s00MOD:MENTE;\r";//切换为强制维护模式
        public static string TEACH = "s00MOD:TEACH;\r";//切换为强制维护模式


        //GET 指令
        public static string STATE = "s00GET:STATE;\r";//查询loadport状态，第5、6位为错误代码
        public static string MAPRD = "s00GET:MAPRD;\r";//查询Wafer search 升序结果  采用升序
                                                       //返回结果 s00ACK:MAPRD/001010101010101010101010101 
                                                       //上面数据分成13组，第一个0表示无，1为正常，其它异常 ，后面00为无，01为正常 其它位异常

        public static string CSTID = "s00GET:CSTID;\r";//功能:读取指定RFID端口上的ID
                     


     
        public static string GetCommond(string strComm, int nType )
        {
            if (nType != 0)           
            {
                strComm = strComm + "\r";
            }
            return strComm;
        }


        //返回结果
        public static string RECEIVE_ACK = "s00ACK";
        public static string FINISH_ACK = "s00INF";

    }
}
