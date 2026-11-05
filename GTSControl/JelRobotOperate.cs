using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jog
{
    public class JelRobotOperate
    {
        //机械手 指令 序号
        public static int FIND_ORIG = 1;
        public static int MOVETO_ORIG = 2;
        public static int A_MOVETO_ORIG = 3;//上手臂
        public static int R_MOVETO_ORIG = 4;//旋转轴        
        public static int F_MOVETO_ORIG = 5;//下手臂
        public static int Z_MOVETO_ORIG = 6;//Z轴
        

        //public static int F_MOVETO_FACE = 11;
        //public static int F_MOVETO_BACK = 12;
                                
        //public static int MOVETO_PHOTO_FACE = 13;  
        //public static int MOVETO_MARK_FACE = 14;   //14--19 正面打标位置   PM7--PM12

        //public static int MOVETO_MARK_FACE45 = 15;
        //public static int MOVETO_MARK_FACE90 = 16;
        //public static int MOVETO_MARK_FACE180 = 17;
        //public static int MOVETO_MARK_FACE_90 = 18;
        //public static int MOVETO_MARK_FACE_45 = 19;

        public static int R_PUTWAFER_ALIGN  = 16;           //上手臂 放料
        public static int R_TAKEWAFER_ALIGN = 17;           //上手臂 取料
        public static int L_PUTWAFER_ALIGN  = 19;           //下手臂 放料
        public static int L_TAKEWAFER_ALIGN = 18;           //下手臂 取料


        public static int START_MAPPING_C1 = 21;
        public static int START_MAPPING_C2 = 22;

      
        public static int MOVETO_OCR_CHECK = 24;

        
        //public static int MOVETO_MARK = 130; //24;  130-135  反面打标位置  PM15- PM20
                
        public static int TAKE_CASS1_FIRSTCHIP = 26;
        public static int PUT_CASS1_FIRSTCHIP = 56;
               
        public static int TAKE_CASS2_FIRSTCHIP = 86;
        public static int PUT_CASS2_FIRSTCHIP = 116;



        //下手臂
        public static int PUT_CASS1_FIRSTCHIP2 = 146;      //smif1 156 --- 180  下手臂放料
        public static int PUT_CASS2_FIRSTCHIP2 = 175;      //smif2 181 --- 206  下手臂放料


        public static string EXCUTE_COMMAND = "$1G{0}\r";
        public static string EXCUTE_CLEAR = "$1RD\r";
        public static string EXCUTE_READMAP = "$1WFK\r";

        public static string EXCUTE_VACUUMUPOPEN = "$1DS11\r";
        public static string EXCUTE_VACUUMUPCLOSE = "$1DS10\r";

        public static string EXCUTE_VACUUMDNOPEN = "$1DS21\r";
        public static string EXCUTE_VACUUMDNCLOSE = "$1DS20\r";


        public static string ReadPowerStart = "SO 1\r\n";
        public static string ReadPowerStop = "SO 0\r\n";
        public static string ReadPowerCommand = "GD\r\n";
        public static string ReadNormalPowerCommand = "GSR\r\n";
        public static string SET_BANK = "$1BC1\r";


        public static string GetICommString(int nIndex)
        {
            string str;
            str = string.Format(EXCUTE_COMMAND, nIndex);
            return str;
        }

        public static string GetClearString()
        {
            return EXCUTE_CLEAR;
        }

        public static string GetMappingString()
        {
            return EXCUTE_READMAP;
        }

        //16 进制
        public static string GetSwitchRobotBankString(int nBank)
        {        
            string strBank = Convert.ToString(nBank, 16);
            //$1BC1
            string swBank = string.Format("$1BC{0}\r", strBank);

            
            return swBank;
        }

    }
}
