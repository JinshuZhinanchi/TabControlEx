//#define MultiDllState

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;



namespace LmcLib
{
    public enum LmcErrCode
    {
        LMC1_ERR_SUCCESS       =  0 , //成功
        LMC1_ERR_EZCADRUN      =  1 , //发现EZCAD已经在运行
        LMC1_ERR_NOFINDCFGFILE =  2 , //找不到EZCAD.CFG
        LMC1_ERR_FAILEDOPEN    =  3 , //打开LMC失败
        LMC1_ERR_NODEVICE      =  4 , //没有LMC设备
        LMC1_ERR_HARDVER       =  5 , //lmc设备版本错误
        LMC1_ERR_DEVCFG        =  6,  //找不到设备配置文件MarkCfg
        LMC1_ERR_STOPSIGNAL    =  7 , //有报警信号
        LMC1_ERR_USERSTOP      =  8 , //用户中断
        LMC1_ERR_UNKNOW        =  9 , //不明错误
        LMC1_ERR_OUTTIME       =  10, //超时
        LMC1_ERR_NOINITIAL     =  11, //未初始化
        LMC1_ERR_READFILE      =  12, //读文件失败
        LMC1_ERR_OWENWNDNULL   =  13, //窗口为空
        LMC1_ERR_NOFINDFONT    =  14, //找不到字体
        LMC1_ERR_PENNO         =  15, //笔号错误
        LMC1_ERR_NOTTEXT       =  16, //指定对象不是文本对象
        LMC1_ERR_SAVEFILE      =  17, //保存文件失败
        LMC1_ERR_NOFINDENT     =  18, //保存文件失败找不到指定对象
        LMC1_ERR_STATUE        =  19, //当前状态不能执行此操作
        LMC1_ERR_PARAM         =  20, //参数输入错误
    }

#if MultiDllState
    public class LmcDll
    {
        public static string GetErrorText(LmcErrCode error)
        {
            switch (error)
            {
                case LmcErrCode.LMC1_ERR_SUCCESS: return "成功";
                case LmcErrCode.LMC1_ERR_EZCADRUN: return "发现EZCAD已经在运行";
                case LmcErrCode.LMC1_ERR_NOFINDCFGFILE: return "找不到EZCAD.CFG";
                case LmcErrCode.LMC1_ERR_FAILEDOPEN: return "打开LMC失败";
                case LmcErrCode.LMC1_ERR_NODEVICE: return "没有LMC设备";
                case LmcErrCode.LMC1_ERR_HARDVER: return "lmc设备版本错误";
                case LmcErrCode.LMC1_ERR_DEVCFG: return "找不到设备配置文件MarkCfg";
                case LmcErrCode.LMC1_ERR_STOPSIGNAL: return "有报警信号";
                case LmcErrCode.LMC1_ERR_USERSTOP: return "用户中断";
                case LmcErrCode.LMC1_ERR_UNKNOW: return "不明错误";
                case LmcErrCode.LMC1_ERR_OUTTIME: return "超时";
                case LmcErrCode.LMC1_ERR_NOINITIAL: return "未初始化";
                case LmcErrCode.LMC1_ERR_READFILE: return "读文件失败";
                case LmcErrCode.LMC1_ERR_OWENWNDNULL: return "窗口为空";
                case LmcErrCode.LMC1_ERR_NOFINDFONT: return "找不到字体";
                case LmcErrCode.LMC1_ERR_PENNO: return "笔号错误";
                case LmcErrCode.LMC1_ERR_NOTTEXT: return "指定对象不是文本对象";
                case LmcErrCode.LMC1_ERR_SAVEFILE: return "保存文件失败";
                case LmcErrCode.LMC1_ERR_NOFINDENT: return "保存文件失败找不到指定对象";
                case LmcErrCode.LMC1_ERR_STATUE: return "当前状态不能执行此操作";
                case LmcErrCode.LMC1_ERR_PARAM: return "参数输入错误";
                default:
                    break;
            }
            return "参数输入错误";
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

        /// <summary>
        /// 初始化函数库
        /// PathName 是MarkEzd.dll所在的目录
        /// </summary>     
        [DllImport("MarkEzd", EntryPoint = "lmc1_Initial2", CharSet= CharSet.Unicode,CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Initialize(string PathName,bool bTestMode);

        /// <summary>
        /// 释放函数库
        /// </summary>     
        [DllImport("MarkEzd", EntryPoint = "lmc1_Close",CharSet= CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Close();        

        /// <summary>
        /// 载入ezd文件到当前数据库里面,并清除旧的数据库
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_LoadEzdFile", CharSet= CharSet.Unicode,CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode LoadEzdFile(string FileName);

        /// <summary>
        /// 标刻当前数据库里面所有数据一次
        /// Fly 表示进行飞动标刻
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_Mark",CharSet= CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Mark(bool Fly);

        [DllImport("MarkEzd", EntryPoint = "lmc1_IsMarking", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsMarking();

        /// <summary>
        /// 飞行标刻指定对象一次
        /// EntName 指定对象的名字
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_MarkEntityFly", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkEntityFly(string EntName);

        /// <summary>
        /// 标刻指定对象一次
        /// EntName 指定对象的名字
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_MarkEntity", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkEntity(string EntName);

        /// <summary>
        /// 红光预览当前数据库里面所有数据一次
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_RedLightMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode RedMark();


        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("MarkEzd", EntryPoint = "lmc1_GetEzdFilePrevBitmap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetEzdFilePrevBitmap(string FileName);

        /// <summary>
        /// 得到指定ezd文件里面数据的预览图片
        /// </summary>   
        public static Image GetEzdFilePreviewImage(string FileName)
        {
            IntPtr pBmp = GetEzdFilePrevBitmap(FileName);
            Image img = Image.FromHbitmap(pBmp);
            LmcDll.DeleteObject(pBmp);
            return img;
        }


        [DllImport("MarkEzd", EntryPoint = "lmc1_GetPrevBitmap2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurPrevBitmap(int bmpwidth, int bmpheight);
      
        /// <summary>
        /// 得到当前数据库里面数据的预览图片
        /// </summary>  
        public static Image GetCurPreviewImage(int bmpwidth, int bmpheight)
        {
            IntPtr pBmp = GetCurPrevBitmap(bmpwidth, bmpheight);
            Image img = Image.FromHbitmap(pBmp);
            LmcDll.DeleteObject(pBmp);
            return img;
        }

        //调用设置设备参数的对话框
        [DllImport("MarkEzd", EntryPoint = "lmc1_SetDevCfg", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_SetDevCfg();

        public static void SetDevCfg()
        {
            lmc1_SetDevCfg();
        }


        [DllImport("MarkEzd", EntryPoint = "lmc1_GetEntityName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_GetEntityNameByIndex(int nEntityIndex, StringBuilder entname);

        /// <summary>
        /// 得到指定索引号的对象的名称。
        /// </summary>     
        public static string GetEntityNameByIndex(int nEntityIndex)
        {   
             StringBuilder str = new StringBuilder("",255);
             lmc1_GetEntityNameByIndex(nEntityIndex, str);            
             return str.ToString();
        }


        [DllImport("MarkEzd", EntryPoint = "lmc1_GetTextByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_GetTextByName(string  strTextName, StringBuilder entname);

        /// <summary>
        /// 得到指定对象的文本
        /// </summary>     
        public static string GetTextByName(string strTextName)
        {   
             StringBuilder str = new StringBuilder("",255);
             lmc1_GetTextByName(strTextName, str);            
             return str.ToString();
        }

        /// <summary>
        /// 得到对象总数
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetEntityCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort GetEntityCount();

        /// <summary>
        /// 读当前输入端口
        /// data 为当前输入端口的值,
        /// Bit0是In0的状态,Bit0=1表示In0为高电平,Bit0=0表示In0为低电平
        /// Bit1是In1的状态,Bit1=1表示In1为高电平,Bit1=0表示In1为低电平
        /// ........
        /// Bit15是In15的状态,Bit15=1表示In15为高电平,Bit15=0表示In15为低电平
        /// </summary>   
        [DllImport("MarkEzd", EntryPoint = "lmc1_ReadPort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ReadPort(ref ushort data);

        /// <summary>
        /// 设置当前输出端口输出
        /// data 为当前输口端口要设置的值,
        /// Bit0是Out0的状态,Bit0=1表示Out0为高电平,Bit0=0表示Out0为低电平
        /// Bit1是Out1的状态,Bit1=1表示Out1为高电平,Bit1=0表示Out1为低电平
        /// ........
        /// Bit15是Out15的状态,Bit15=1表示Out15为高电平,Bit15=0表示Out15为低电平
        /// </summary>  
        [DllImport("MarkEzd", EntryPoint = "lmc1_WritePort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode WritePort(ushort data);


   
        /// <summary>
        /// 更改数据库中指定名称的文本对象的内容为指定文本
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_ChangeTextByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ChangeTextByName(string EntName, string NewText);
        

        public const uint LMC1_FONT_JSF = 1; //JczSingleLine字体
        public const uint LMC1_FONT_TTF = 2; //TrueType字体
        public const uint LMC1_FONT_DMF = 4; //DotMatrix字体
        public const uint LMC1_FONT_BCF = 8; //Barcode字体

        public struct FontRecord
        {
            public string fontname;//字体名称
            public uint fontattrib;//字体属性
        }


        /// <summary>
        /// 得到数据库中字体记录的总数
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetFontRecordCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetFontRecordCount(ref int fontCount);

        /// <summary>
        /// 得到系统中指定序号的字体记录的名称和属性
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetFontRecord", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetFontRecordByIndex(int fontIndex, StringBuilder fontName, ref uint fontAttrib);


         /// <summary>
         /// 得到系统中所有可以使用的字体名称和属性
         /// </summary> 
         public static bool GetAllFontRecord(ref FontRecord[] fonts)
         {
             int fontnum=0;
             if(GetFontRecordCount(ref fontnum)!=LmcErrCode.LMC1_ERR_SUCCESS)
             {
                 return false;
             }
             if(fontnum==0)
             {
                 return true;
             }
           
             fonts = new FontRecord[fontnum];
             StringBuilder str = new StringBuilder("",255);
             uint fontAttrib=0;
             for (int i = 0; i < fontnum; i++)
             {         
                 GetFontRecordByIndex(i,str,ref fontAttrib);
                 fonts[i].fontname = str.ToString();;
                 fonts[i].fontattrib = fontAttrib;
             }               
             return true;
         }

         /// <summary>
         /// 清除数据库里所有对象
         /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_ClearEntLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ClearLibAllEntity();

         /// <summary>
         /// 设置当前字体的属性,当向数据库里添加文本时要使用这些参数
         /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_SetFontParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetFontParam(string strFontName,double dCharHeight,double dCharWidth,double dCharAngle,double dCharSpace,double dLineSpace,bool bEqualCharWidth);


        /// <summary>
        /// 得到指定笔号的参数
        /// nPenNo要设置的笔号0-255
        /// nMarkLoop标刻次数
        /// dMarkSpeed 加工速度 mm/s或者inch/mm,取决于markdll.dll的当前单位
        /// dPowerRatio功率百分比 0-100%
        /// dCurrent电流A
        /// nFreq频率Hz
        /// nQPulseWidth Q脉冲宽度 us
        /// nStartTC 开光延时 us
        /// nLaserOffTC 关光延时 us
        /// nEndTC  结束延时 us
        /// nPolyTC 多边形拐角延时us
        /// dJumpSpeed 跳转速度 mm/s或者inch/mm,取决于markdll.dll的当前单位 
        /// nJumpPosTC 跳转位置延时 us
        /// nJumpDistTC 跳转距离延时 us
        /// dEndComp 末点补偿距离 mm或者inch,取决于markdll.dll的当前单位 
        /// dAccDist 加速距离 mm或者inch
        /// dPointTime 打点时间ms
        /// bPulsePointMode 打点模式 true使能
        /// nPulseNum 打点个数
        /// dFlySpeed 流水线速度 mm/s或者inch/mm
         /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetPenParam(int nPenNo,
                     ref int nMarkLoop,
                     ref double dMarkSpeed,
					 ref double   dPowerRatio,
					 ref double   dCurrent,
					 ref int      nFreq,
					 ref double   nQPulseWidth,
					 ref int      nStartTC,
					 ref int      nLaserOffTC,
				     ref int      nEndTC,
					 ref int      nPolyTC,
					 ref double   dJumpSpeed,
					 ref int      nJumpPosTC,
					 ref int      nJumpDistTC,
					 ref double   dEndComp,
					 ref double   dAccDist,
					 ref double   dPointTime,
					 ref bool     bPulsePointMode,
					 ref int      nPulseNum,
					 ref double   dFlySpeed);
        /// <summary>
        /// 设置指定笔号的参数
        /// nPenNo要设置的笔号0-255
        /// nMarkLoop标刻次数
        /// dMarkSpeed 加工速度 mm/s或者inch/mm,取决于markdll.dll的当前单位
        /// dPowerRatio功率百分比 0-100%
        /// dCurrent电流A
        /// nFreq频率Hz
        /// nQPulseWidth Q脉冲宽度 us
        /// nStartTC 开光延时 us
        /// nLaserOffTC 关光延时 us
        /// nEndTC  结束延时 us
        /// nPolyTC 多边形拐角延时us
        /// dJumpSpeed 跳转速度 mm/s或者inch/mm,取决于markdll.dll的当前单位 
        /// nJumpPosTC 跳转位置延时 us
        /// nJumpDistTC 跳转距离延时 us
        /// dEndComp 末点补偿距离 mm或者inch,取决于markdll.dll的当前单位 
        /// dAccDist 加速距离 mm或者inch
        /// dPointTime 打点时间ms
        /// bPulsePointMode 打点模式 true使能
        /// nPulseNum 打点个数
        /// dFlySpeed 流水线速度 mm/s或者inch/mm
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_SetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetPenParam(int nPenNo,
                             int nMarkLoop,
                             double dMarkSpeed,
                             double dPowerRatio,
                             double dCurrent,
                             int nFreq,
                             double nQPulseWidth,
                             int nStartTC,
                             int nLaserOffTC,
                             int nEndTC,
                             int nPolyTC,
                             double dJumpSpeed,
                             int nJumpPosTC,
                             int nJumpDistTC,
                             double dEndComp,
                             double dAccDist,
                             double dPointTime,
                             bool bPulsePointMode,
                             int nPulseNum,
                             double dFlySpeed);

         /// <summary>
         /// 向数据库里添加文本
         /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AddTextToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddTextToLib(string text, string EntName, double dPosX, double dPosY, double dPosZ, int nAlign, double dTextRotateAngle, int nPenNo, int bHatchText);

        /// <summary>
        /// 保存当前数据到指定文件
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_SaveEntLibToFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SaveEntLibToFile(string strFileName);


        [DllImport("MarkEzd", EntryPoint = "lmc1_MarkPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkPoint(double x,double y,double delay,int pen);


        /// <summary>
        /// 载入指定数据文件
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AddFileToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddFileToLib(string strFileName,string strEntName,double dPosX,double dPosY,double dPosZ,int nAlign,double dRatio,int nPenNo,int bHatchFile);

        public enum BARCODETYPE
        {
            BARCODETYPE_39  =    0,
            BARCODETYPE_93      =1,
            BARCODETYPE_128A    =2,
            BARCODETYPE_128B    =3,
            BARCODETYPE_128C    =4,
            BARCODETYPE_128OPT  =5,
            BARCODETYPE_EAN128A =6,
            BARCODETYPE_EAN128B =7,
            BARCODETYPE_EAN128C =8,
            BARCODETYPE_EAN13   =9,
            BARCODETYPE_EAN8    =10,
            BARCODETYPE_UPCA    =11,
            BARCODETYPE_UPCE    =12,
            BARCODETYPE_25      =13,
            BARCODETYPE_INTER25 =14,
            BARCODETYPE_CODABAR =15,
            BARCODETYPE_PDF417  =16,
            BARCODETYPE_DATAMTX =17,
            BARCODETYPE_USERDEF =18,
         };
        public const ushort BARCODEATTRIB_REVERSE         = 0x0008;
        public const ushort BARCODEATTRIB_HUMANREAD       = 0x1000;
        public const ushort BARCODEATTRIB_CHECKNUM        = 0x0004;
        public const ushort BARCODEATTRIB_PDF417_SHORTMODE= 0x0040;
        public const ushort BARCODEATTRIB_DATAMTX_DOTMODE = 0x0080;
        public const ushort BARCODEATTRIB_CIRCLEMODE      = 0x0100;

        public enum DATAMTX_SIZEMODE
        {
            DATAMTX_SIZEMODE_SMALLEST  =0,
            DATAMTX_SIZEMODE_10X10     =1,
            DATAMTX_SIZEMODE_12X12     =2,
            DATAMTX_SIZEMODE_14X14     =3,
            DATAMTX_SIZEMODE_16X16     =4,
            DATAMTX_SIZEMODE_18X18     =5,
            DATAMTX_SIZEMODE_20X20     =6,
            DATAMTX_SIZEMODE_22X22     =7,
            DATAMTX_SIZEMODE_24X24     =8,
            DATAMTX_SIZEMODE_26X26     =9,
            DATAMTX_SIZEMODE_32X32     =10,
            DATAMTX_SIZEMODE_36X36     =11,
            DATAMTX_SIZEMODE_40X40     =12,
            DATAMTX_SIZEMODE_44X44     =13,
            DATAMTX_SIZEMODE_48X48     =14,
            DATAMTX_SIZEMODE_52X52     =15,
            DATAMTX_SIZEMODE_64X64     =16,
            DATAMTX_SIZEMODE_72X72     =17,
            DATAMTX_SIZEMODE_80X80     =18,
            DATAMTX_SIZEMODE_88X88     =19,
            DATAMTX_SIZEMODE_96X96     =20,
            DATAMTX_SIZEMODE_104X104   =21,
            DATAMTX_SIZEMODE_120X120   =22,
            DATAMTX_SIZEMODE_132X132   =23,
            DATAMTX_SIZEMODE_144X144   =24,
            DATAMTX_SIZEMODE_8X18     =25,
            DATAMTX_SIZEMODE_8X32     =26,
            DATAMTX_SIZEMODE_12X26     =27,
            DATAMTX_SIZEMODE_12X36     =28,
            DATAMTX_SIZEMODE_16X36     =29,
            DATAMTX_SIZEMODE_16X48     =30,
        }
         /// <summary>
         /// 向数据库里添加条码文本
        /// 注意 double[] dBarWidthScale 和dSpaceWidthScale大小必须为4
         /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AddBarCodeToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddBarCodeToLib(string strText,
            string strEntName,
            double dPosX,
            double dPosY,
            double dPosZ,
            int nAlign,
            int nPenNo,
            int bHatchText,
            BARCODETYPE nBarcodeType,
            ushort wBarCodeAttrib,
            double dHeight,
            double dNarrowWidth,  
            [MarshalAs(UnmanagedType.LPArray)] double[] dBarWidthScale,
            [MarshalAs(UnmanagedType.LPArray)] double[] dSpaceWidthScale,
          	double dMidCharSpaceScale,
			double dQuietLeftScale,
			double dQuietMidScale,
			double dQuietRightScale,
			double dQuietTopScale,
			double dQuietBottomScale,
			int    nRow,
			int    nCol,
			int    nCheckLevel,
           DATAMTX_SIZEMODE nSizeMode,
			double dTextHeight,
			double dTextWidth,
			double dTextOffsetX,
			double dTextOffsetY,
			double dTextSpace,
			double dDiameter,
			string TextFontName);

        public const int HATCHATTRIB_ALLCALC = 0x01;//全部对象整体计算
        public const int HATCHATTRIB_EDGE = 0x02;//绕边走一次
        public const int HATCHATTRIB_MINUP = 0x04;//最少起笔
        public const int HATCHATTRIB_BIDIR = 0x08;//双向填充
        public const int HATCHATTRIB_LOOP = 0x10;//环行填充
        public const int HATCHATTRIB_OUT = 0x20;//环行由内向外
        public const int HATCHATTRIB_AUTOROT = 0x40;//自动角度旋转
        public const int HATCHATTRIB_AVERAGELINE = 0x80;//自动分布填充线

        /// <summary>
        /// 设置当前的填充参数,如果向数据库添加对象时使能填充,系统会用此函数设置的参数来填充
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_SetHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetHatchParam(bool bEnableContour,
                          int bEnableHatch1,
                          int nPenNo1,
                          int nHatchAttrib1,
                          double dHatchEdgeDist1,
                          double dHatchLineDist1,
                          double dHatchStartOffset1,
                          double dHatchEndOffset1,
                          double dHatchAngle1,
                          int bEnableHatch2,
                          int nPenNo2,
                          int nHatchAttrib2,
                          double dHatchEdgeDist2,
                          double dHatchLineDist2,
                          double dHatchStartOffset2,
                          double dHatchEndOffset2,
                          double dHatchAngle2);

        /// <summary>
        /// 向数据库添加一条曲线对象
        /// 注意PtBuf必须为2维数组,且第一维为2,如 double[5,2],double[n,2],
        /// ptNum为PtBuf数组的第2维,如PtBuf为double[5,2]数组,则ptNum=5
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AddCurveToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddCurveToLib([MarshalAs(UnmanagedType.LPArray)] double[,] PtBuf,
        int ptNum,string strEntName,int nPenNo,int bHatch);

        /// <summary>
        /// 设置数据库的所有对象的旋转参数,不影响数据的显示,只是加工时才对对象进行旋转
        /// dMoveX x方向移动距离
        /// dMoveY y方向移动距离
        /// dCenterX旋转中心的x坐标
        /// dCenterY旋转中心的y坐标      
        /// dRotateAng为旋转角度,单位为弧度值
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_SetRotateMoveParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetRotateMoveParam(double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng);



        /// <summary>
        /// 移动数据库中指定名称的对象
        /// dMoveX x方向移动距离
        /// dMoveY y方向移动距离
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_MoveEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MoveEnt(string strEntName,double dMovex,double dMovey);
            
        /// <summary>
        /// 对数据库中指定名称的对象进行比例变换
        /// dCenx,dCeny变换中心的坐标
        /// dScaleX x方向变换比例
        /// dScaleY y方向变换比例
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_ScaleEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ScaleEnt(string strEntName,double dCenx,double dCeny,double dScaleX,double dScaleY);

        /// <summary>
        /// 对数据库中指定名称的对象进行镜像变换
        /// dCenx,dCeny镜像中心的坐标
        /// bMirrorX= true X镜像 
        /// bMirrorY= true Y镜像
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_MirrorEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MirrorEnt(string strEntName,double dCenx,double dCeny,bool bMirrorX,bool bMirrorY);

        /// <summary>
        /// 对数据库中指定名称的对象进行旋转变换
        /// dCenx旋转中心的x坐标
        /// dCeny旋转中心的y坐标      
        /// dAngle为旋转角度,单位为弧度值
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_RotateEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode RotateEnt(string strEntName, double dCenx,double dCeny,double dAngle);

        /// <summary>
        /// 得到用户加密狗的客户ID号
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetClientID", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort GetClientID();

        /// <summary>
        /// 得到指定对象的尺寸信息
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetEntSize", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetEntSize(string strEntName, ref double dMinx,ref double dMiny,ref double dMaxx,ref double dMaxy,ref double dz);


#region 扩展轴的函数
        /// <summary>
        /// 下面是关于扩展轴的函数
        /// </summary> 

        /// <summary>
        /// 扩展轴移动到目标位置
        /// axis=0或1
        /// GoalPos目标位置,单位mm或inch
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AxisMoveTo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisMoveTo(int axis, double GoalPos);


        /// <summary>
        /// 扩展轴回原点(校正原点)
        /// axis=0或1
        /// GoalPos目标位置
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AxisCorrectOrigin", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisGoHome(int axis);

        /// <summary>
        /// 得到扩展轴的当前坐标
        /// axis=0或1
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_GetAxisCoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern double GetAxisCoor(int axis);


        /// <summary>
        /// 扩展轴移动到脉冲目标位置
        /// axis=0或1
        /// nGoalPos目标位置,单位:脉冲
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AxisMoveToPulse", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisMoveToPulse(int axis, int nGoalPos);

        /// <summary>
        /// 得到扩展轴的当前脉冲坐标
        /// axis=0或1
        /// </summary> 
        [DllImport("MarkEzd", EntryPoint = "lmc1_AxisMoveToPulse", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetAxisCoorPulse(int axis);

        
#endregion
        [DllImport("MarkEzd", EntryPoint = "lmc1_StopMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode StopMark();

    }
#endif

    public class LmcMultiDll
    {
        public static string GetErrorText(LmcErrCode error)
        {
            switch (error)
            {
                case LmcErrCode.LMC1_ERR_SUCCESS: return "成功";
                case LmcErrCode.LMC1_ERR_EZCADRUN: return "发现EZCAD已经在运行";
                case LmcErrCode.LMC1_ERR_NOFINDCFGFILE: return "找不到EZCAD.CFG";
                case LmcErrCode.LMC1_ERR_FAILEDOPEN: return "打开LMC失败";
                case LmcErrCode.LMC1_ERR_NODEVICE: return "没有LMC设备";
                case LmcErrCode.LMC1_ERR_HARDVER: return "lmc设备版本错误";
                case LmcErrCode.LMC1_ERR_DEVCFG: return "找不到设备配置文件MarkCfg";
                case LmcErrCode.LMC1_ERR_STOPSIGNAL: return "有报警信号";
                case LmcErrCode.LMC1_ERR_USERSTOP: return "用户中断";
                case LmcErrCode.LMC1_ERR_UNKNOW: return "不明错误";
                case LmcErrCode.LMC1_ERR_OUTTIME: return "超时";
                case LmcErrCode.LMC1_ERR_NOINITIAL: return "未初始化";
                case LmcErrCode.LMC1_ERR_READFILE: return "读文件失败";
                case LmcErrCode.LMC1_ERR_OWENWNDNULL: return "窗口为空";
                case LmcErrCode.LMC1_ERR_NOFINDFONT: return "找不到字体";
                case LmcErrCode.LMC1_ERR_PENNO: return "笔号错误";
                case LmcErrCode.LMC1_ERR_NOTTEXT: return "指定对象不是文本对象";
                case LmcErrCode.LMC1_ERR_SAVEFILE: return "保存文件失败";
                case LmcErrCode.LMC1_ERR_NOFINDENT: return "保存文件失败找不到指定对象";
                case LmcErrCode.LMC1_ERR_STATUE: return "当前状态不能执行此操作";
                case LmcErrCode.LMC1_ERR_PARAM: return "参数输入错误";
                default:
                    break;
            }
            return "参数输入错误";
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

        /// <summary>
        /// 初始化函数库
        /// PathName 是MarkEzd.dll所在的目录
        /// </summary>     
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_Initial", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Initialize( string PathName, bool bTestMode , IntPtr hOwenWnd);//父窗口

        /// <summary>
        /// 释放函数库
        /// </summary>     
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_Close", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Close();

        /// <summary>
        ////得到所有卡的序号 , 卡序号=-1表示没有卡  0－7表示有效的序号  动态库nCard调用的是sn的序号
        /// </summary>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetAllCardSN", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetAllCardSN(int[] sn);

        /// <summary>
        /// 载入ezd文件到当前数据库里面,并清除旧的数据库
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_LoadEzdFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode LoadEzdFile(int nCard, string FileName);

        /// <summary>
        //是否当前打标已经结束
        //返回参数 0 表示正在加工 1打标已经结束
        /// </summary>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_IsMarkFinish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsMarkingFinish(int nCard);

        /// <summary>
        /// 停止打标工作
        /// axis=0或1
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_StopMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode StopMark(int nCard);

        /// <summary>
        /// 标刻当前数据库里面所有数据一次
        /// Fly 表示进行飞动标刻
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_Mark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode Mark(int nCard, bool Fly);

        /// <summary>
        /// 飞行标刻指定对象一次
        /// EntName 指定对象的名字
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MarkEntityFly", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkEntityFly(int nCard, string EntName);

        /// <summary>
        /// 标刻指定对象一次
        /// EntName 指定对象的名字
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MarkEntity", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkEntity(int nCard, string EntName);

        /// <summary>
        /// 红光预览当前数据库里面所有数据一次
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_RedLightMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode RedMark(int nCard);


        /// <summary>
        /// 控制振镜直接运动到指定坐标
        /// </summary>
        /// <param name="nCard"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GotoPos", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int GotoPos(int nCard, double x, double y);


        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetEzdFilePrevBitmap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetEzdFilePrevBitmap(int nCard, string FileName);

        /// <summary>
        /// 得到指定ezd文件里面数据的预览图片
        /// </summary>   
        public static Image GetEzdFilePreviewImage(int nCard,string FileName)
        {
            IntPtr pBmp = GetEzdFilePrevBitmap(nCard, FileName);
            Image img = Image.FromHbitmap(pBmp);
            LmcMultiDll.DeleteObject(pBmp);
            return img;
        }


        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetPrevBitmap2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurPrevBitmap(int nCard, int nhandle, int bmpwidth, int bmpheight);

        /// <summary>
        /// 得到当前数据库里面数据的预览图片
        /// </summary>  
        public static Image GetCurPreviewImage(int nCard,int bmpwidth, int bmpheight)
        {
            IntPtr pBmp = GetCurPrevBitmap(nCard, 0, bmpwidth, bmpheight);
            Image img = Image.FromHbitmap(pBmp);
            LmcMultiDll.DeleteObject(pBmp);
            return img;
        }

        //调用设置设备参数的对话框
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetDevCfg", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_SetDevCfg(int nCard);

        public static void SetDevCfg(int nCard)
        {
            lmc1_SetDevCfg(nCard);
        }


        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetEntityName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_GetEntityNameByIndex(int nCard,int nEntityIndex, StringBuilder entname);

        /// <summary>
        /// 得到指定索引号的对象的名称。
        /// </summary>     
        public static string GetEntityNameByIndex(int nCard,int nEntityIndex)
        {
            StringBuilder str = new StringBuilder("", 255);
            lmc1_GetEntityNameByIndex(nCard,nEntityIndex, str);
            return str.ToString();
        }


        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetTextByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        internal static extern LmcErrCode lmc1_GetTextByName(int nCard, string strTextName, StringBuilder entname);

        /// <summary>
        /// 得到指定对象的文本
        /// </summary>     
        public static string GetTextByName(int nCard,string strTextName)
        {
            StringBuilder str = new StringBuilder("", 255);
            lmc1_GetTextByName(nCard,strTextName, str);
            return str.ToString();
        }

        /// <summary>
        /// 得到对象总数
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetEntityCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort GetEntityCount(int nCard);

        /// <summary>
        /// 读当前输入端口
        /// data 为当前输入端口的值,
        /// Bit0是In0的状态,Bit0=1表示In0为高电平,Bit0=0表示In0为低电平
        /// Bit1是In1的状态,Bit1=1表示In1为高电平,Bit1=0表示In1为低电平
        /// ........
        /// Bit15是In15的状态,Bit15=1表示In15为高电平,Bit15=0表示In15为低电平
        /// </summary>   
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_ReadPort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ReadPort(int nCard,ref ushort data);

        /// <summary>
        /// 设置当前输出端口输出
        /// data 为当前输口端口要设置的值,
        /// Bit0是Out0的状态,Bit0=1表示Out0为高电平,Bit0=0表示Out0为低电平
        /// Bit1是Out1的状态,Bit1=1表示Out1为高电平,Bit1=0表示Out1为低电平
        /// ........
        /// Bit15是Out15的状态,Bit15=1表示Out15为高电平,Bit15=0表示Out15为低电平
        /// </summary>  
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_WritePort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode WritePort(int nCard,ushort data);



        /// <summary>
        /// 更改数据库中指定名称的文本对象的内容为指定文本
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_ChangeTextByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ChangeTextByName(int nCard, string EntName, string NewText);


        /// <summary>
        /// 设定指定卡中指定索引号的对象的名称
        /// </summary>  nCard 卡序号        nEntIndex 对象索引号号    strEntName对象名称
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetEntityName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetEntityName(int nCard, int nEntIndex, string strEntName);

        public const uint LMC1_FONT_JSF = 1; //JczSingleLine字体
        public const uint LMC1_FONT_TTF = 2; //TrueType字体
        public const uint LMC1_FONT_DMF = 4; //DotMatrix字体
        public const uint LMC1_FONT_BCF = 8; //Barcode字体

        public struct FontRecord
        {
            public string fontname;//字体名称
            public uint fontattrib;//字体属性
        }


        /// <summary>
        /// 得到数据库中字体记录的总数
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetFontRecordCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetFontRecordCount(int nCard, ref int fontCount);

        /// <summary>
        /// 得到系统中指定序号的字体记录的名称和属性
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetFontRecord", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetFontRecordByIndex(int nCard,int fontIndex, StringBuilder fontName, ref uint fontAttrib);


        /// <summary>
        /// 得到系统中所有可以使用的字体名称和属性
        /// </summary> 
        public static bool GetAllFontRecord(int nCard,ref FontRecord[] fonts)
        {
            int fontnum = 0;
            if (GetFontRecordCount(nCard,ref fontnum) != LmcErrCode.LMC1_ERR_SUCCESS)
            {
                return false;
            }
            if (fontnum == 0)
            {
                return true;
            }

            fonts = new FontRecord[fontnum];
            StringBuilder str = new StringBuilder("", 255);
            uint fontAttrib = 0;
            for (int i = 0; i < fontnum; i++)
            {
                GetFontRecordByIndex(nCard,i, str, ref fontAttrib);
                fonts[i].fontname = str.ToString(); ;
                fonts[i].fontattrib = fontAttrib;
            }
            return true;
        }

        /// <summary>
        /// 清除数据库里所有对象
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_ClearEntLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ClearLibAllEntity(int nCard);

        /// <summary>
        /// 设置当前字体的属性,当向数据库里添加文本时要使用这些参数
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetFontParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetFontParam(string strFontName, double dCharHeight, double dCharWidth, double dCharAngle, double dCharSpace, double dLineSpace, bool bEqualCharWidth);


        /// <summary>
        /// 得到指定笔号的参数
        /// nPenNo要设置的笔号0-255
        /// nMarkLoop标刻次数
        /// dMarkSpeed 加工速度 mm/s或者inch/mm,取决于markdll.dll的当前单位
        /// dPowerRatio功率百分比 0-100%
        /// dCurrent电流A
        /// nFreq频率Hz
        /// nQPulseWidth Q脉冲宽度 us
        /// nStartTC 开光延时 us
        /// nLaserOffTC 关光延时 us
        /// nEndTC  结束延时 us
        /// nPolyTC 多边形拐角延时us
        /// dJumpSpeed 跳转速度 mm/s或者inch/mm,取决于markdll.dll的当前单位 
        /// nJumpPosTC 跳转位置延时 us
        /// nJumpDistTC 跳转距离延时 us
        /// dEndComp 末点补偿距离 mm或者inch,取决于markdll.dll的当前单位 
        /// dAccDist 加速距离 mm或者inch
        /// dPointTime 打点时间ms
        /// bPulsePointMode 打点模式 true使能
        /// nPulseNum 打点个数
        /// dFlySpeed 流水线速度 mm/s或者inch/mm
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetPenParam(int nCard, int nPenNo,
                     ref int nMarkLoop,
                     ref double dMarkSpeed,
                     ref double dPowerRatio,
                     ref double dCurrent,
                     ref int nFreq,
                     ref double nQPulseWidth,
                     ref int nStartTC,
                     ref int nLastoffTC,
                     ref int nEndTC,
                     ref int nPolyTC,
                     ref double dJumpSpeed,
                     ref int nJumpPosTC,
                     ref int nJumpDistTC,
                     ref double dEndComp,
                     ref double dAccDist,
                     ref double dPointTime,
                     ref bool bPulsePointMode,
                     ref int nPulseNum,
                     ref double dFlySpeed);       
        /// <summary>
        /// 设置指定笔号的参数
        /// nPenNo要设置的笔号0-255
        /// nMarkLoop标刻次数
        /// dMarkSpeed 加工速度 mm/s或者inch/mm,取决于markdll.dll的当前单位
        /// dPowerRatio功率百分比 0-100%
        /// dCurrent电流A
        /// nFreq频率Hz
        /// nQPulseWidth Q脉冲宽度 us
        /// nStartTC 开光延时 us
        /// nEndTC  结束延时 us
        /// nPolyTC 多边形拐角延时us
        /// dJumpSpeed 跳转速度 mm/s或者inch/mm,取决于markdll.dll的当前单位 
        /// nJumpPosTC 跳转位置延时 us
        /// nJumpDistTC 跳转距离延时 us
        /// dEndComp 末点补偿距离 mm或者inch,取决于markdll.dll的当前单位 
        /// dAccDist 加速距离 mm或者inch
        /// dPointTime 打点时间ms
        /// bPulsePointMode 打点模式 true使能
        /// nPulseNum 打点个数
        /// dFlySpeed 流水线速度 mm/s或者inch/mm
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetPenParam(int nCard, int nPenNo,
                             int nMarkLoop,
                             double dMarkSpeed,
                             double dPowerRatio,
                             double dCurrent,
                             int nFreq,
                             double nQPulseWidth,
                             int nStartTC,
                             int nLastoffTC,
                             int nEndTC,
                             int nPolyTC,
                             double dJumpSpeed,
                             int nJumpPosTC,
                             int nJumpDistTC,
                             double dEndComp,
                             double dAccDist,
                             double dPointTime,
                             bool bPulsePointMode,
                             int nPulseNum,
                             double dFlySpeed);

        /// <summary>
        /// 向数据库里添加文本
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AddTextToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddTextToLib(int nCard, string text, string EntName, double dPosX, double dPosY, double dPosZ, int nAlign, double dTextRotateAngle, int nPenNo, int bHatchText);

        /// <summary>
        /// 保存当前数据到指定文件
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SaveEntLibToFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SaveEntLibToFile(int nCard,string strFileName);


        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MarkPoint", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MarkPoint(int nCard,double x, double y, double delay, int pen);


        /// <summary>
        /// 载入指定数据文件
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AddFileToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddFileToLib(int nCard,string strFileName, string strEntName, double dPosX, double dPosY, double dPosZ, int nAlign, double dRatio, int nPenNo, int bHatchFile);

        public enum BARCODETYPE
        {
            BARCODETYPE_39 = 0,
            BARCODETYPE_93 = 1,
            BARCODETYPE_128A = 2,
            BARCODETYPE_128B = 3,
            BARCODETYPE_128C = 4,
            BARCODETYPE_128OPT = 5,
            BARCODETYPE_EAN128A = 6,
            BARCODETYPE_EAN128B = 7,
            BARCODETYPE_EAN128C = 8,
            BARCODETYPE_EAN13 = 9,
            BARCODETYPE_EAN8 = 10,
            BARCODETYPE_UPCA = 11,
            BARCODETYPE_UPCE = 12,
            BARCODETYPE_25 = 13,
            BARCODETYPE_INTER25 = 14,
            BARCODETYPE_CODABAR = 15,
            BARCODETYPE_PDF417 = 16,
            BARCODETYPE_DATAMTX = 17,
            BARCODETYPE_USERDEF = 18,
        };
        public const ushort BARCODEATTRIB_REVERSE = 0x0008;
        public const ushort BARCODEATTRIB_HUMANREAD = 0x1000;
        public const ushort BARCODEATTRIB_CHECKNUM = 0x0004;
        public const ushort BARCODEATTRIB_PDF417_SHORTMODE = 0x0040;
        public const ushort BARCODEATTRIB_DATAMTX_DOTMODE = 0x0080;
        public const ushort BARCODEATTRIB_CIRCLEMODE = 0x0100;

        public enum DATAMTX_SIZEMODE
        {
            DATAMTX_SIZEMODE_SMALLEST = 0,
            DATAMTX_SIZEMODE_10X10 = 1,
            DATAMTX_SIZEMODE_12X12 = 2,
            DATAMTX_SIZEMODE_14X14 = 3,
            DATAMTX_SIZEMODE_16X16 = 4,
            DATAMTX_SIZEMODE_18X18 = 5,
            DATAMTX_SIZEMODE_20X20 = 6,
            DATAMTX_SIZEMODE_22X22 = 7,
            DATAMTX_SIZEMODE_24X24 = 8,
            DATAMTX_SIZEMODE_26X26 = 9,
            DATAMTX_SIZEMODE_32X32 = 10,
            DATAMTX_SIZEMODE_36X36 = 11,
            DATAMTX_SIZEMODE_40X40 = 12,
            DATAMTX_SIZEMODE_44X44 = 13,
            DATAMTX_SIZEMODE_48X48 = 14,
            DATAMTX_SIZEMODE_52X52 = 15,
            DATAMTX_SIZEMODE_64X64 = 16,
            DATAMTX_SIZEMODE_72X72 = 17,
            DATAMTX_SIZEMODE_80X80 = 18,
            DATAMTX_SIZEMODE_88X88 = 19,
            DATAMTX_SIZEMODE_96X96 = 20,
            DATAMTX_SIZEMODE_104X104 = 21,
            DATAMTX_SIZEMODE_120X120 = 22,
            DATAMTX_SIZEMODE_132X132 = 23,
            DATAMTX_SIZEMODE_144X144 = 24,
            DATAMTX_SIZEMODE_8X18 = 25,
            DATAMTX_SIZEMODE_8X32 = 26,
            DATAMTX_SIZEMODE_12X26 = 27,
            DATAMTX_SIZEMODE_12X36 = 28,
            DATAMTX_SIZEMODE_16X36 = 29,
            DATAMTX_SIZEMODE_16X48 = 30,
        }
        /// <summary>
        /// 向数据库里添加条码文本
        /// 注意 double[] dBarWidthScale 和dSpaceWidthScale大小必须为4
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AddBarCodeToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddBarCodeToLib(int nCard,string strText,
            string strEntName,
            double dPosX,
            double dPosY,
            double dPosZ,
            int nAlign,
            int nPenNo,
            int bHatchText,
            BARCODETYPE nBarcodeType,
            ushort wBarCodeAttrib,
            double dHeight,
            double dNarrowWidth,
            [MarshalAs(UnmanagedType.LPArray)] double[] dBarWidthScale,
            [MarshalAs(UnmanagedType.LPArray)] double[] dSpaceWidthScale,
              double dMidCharSpaceScale,
            double dQuietLeftScale,
            double dQuietMidScale,
            double dQuietRightScale,
            double dQuietTopScale,
            double dQuietBottomScale,
            int nRow,
            int nCol,
            int nCheckLevel,
           DATAMTX_SIZEMODE nSizeMode,
            double dTextHeight,
            double dTextWidth,
            double dTextOffsetX,
            double dTextOffsetY,
            double dTextSpace,
            double dDiameter,
            string TextFontName);

        public const int HATCHATTRIB_ALLCALC = 0x01;//全部对象整体计算
        public const int HATCHATTRIB_EDGE = 0x02;//绕边走一次
        public const int HATCHATTRIB_MINUP = 0x04;//最少起笔
        public const int HATCHATTRIB_BIDIR = 0x08;//双向填充
        public const int HATCHATTRIB_LOOP = 0x10;//环行填充
        public const int HATCHATTRIB_OUT = 0x20;//环行由内向外
        public const int HATCHATTRIB_AUTOROT = 0x40;//自动角度旋转
        public const int HATCHATTRIB_AVERAGELINE = 0x80;//自动分布填充线

        /// <summary>
        /// 设置当前的填充参数,如果向数据库添加对象时使能填充,系统会用此函数设置的参数来填充
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode SetHatchParam(bool bEnableContour,
                          int bEnableHatch1,
                          int nPenNo1,
                          int nHatchAttrib1,
                          double dHatchEdgeDist1,
                          double dHatchLineDist1,
                          double dHatchStartOffset1,
                          double dHatchEndOffset1,
                          double dHatchAngle1,
                          int bEnableHatch2,
                          int nPenNo2,
                          int nHatchAttrib2,
                          double dHatchEdgeDist2,
                          double dHatchLineDist2,
                          double dHatchStartOffset2,
                          double dHatchEndOffset2,
                          double dHatchAngle2);

        /// <summary>
        /// 向数据库添加一条曲线对象
        /// 注意PtBuf必须为2维数组,且第一维为2,如 double[5,2],double[n,2],
        /// ptNum为PtBuf数组的第2维,如PtBuf为double[5,2]数组,则ptNum=5
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AddCurveToLib", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AddCurveToLib(int nCard,[MarshalAs(UnmanagedType.LPArray)] double[,] PtBuf,
        int ptNum, string strEntName, int nPenNo, int bHatch);

        /// <summary>
        /// 设置数据库的所有对象的旋转参数,不影响数据的显示,只是加工时才对对象进行旋转
        /// dMoveX x方向移动距离
        /// dMoveY y方向移动距离
        /// dCenterX旋转中心的x坐标
        /// dCenterY旋转中心的y坐标      
        /// dRotateAng为旋转角度,单位为弧度值
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_SetRotateMoveParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetRotateMoveParam(int nCard,double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng);



        /// <summary>
        /// 移动数据库中指定名称的对象
        /// dMoveX x方向移动距离
        /// dMoveY y方向移动距离
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MoveEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MoveEnt(int nCard,string strEntName, ref double dMovex, ref double dMovey);

        ///<summary>
        /// 复制指定名称对象，并命名
        ///<summary>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_CopyEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int CopyEnt(int nCard, string strEntName, string strNewEntName);

        ///<summary>
        /// 删除指定名称对象
        ///<summary>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_DeleteEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DeleteEnt(int nCard, string strEntName);


        /// <summary>
        /// 对数据库中指定名称的对象进行比例变换
        /// dCenx,dCeny变换中心的坐标
        /// dScaleX x方向变换比例
        /// dScaleY y方向变换比例
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_ScaleEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode ScaleEnt(int nCard,string strEntName, double dCenx, double dCeny, double dScaleX, double dScaleY);

        /// <summary>
        /// 对数据库中指定名称的对象进行镜像变换
        /// dCenx,dCeny镜像中心的坐标
        /// bMirrorX= true X镜像 
        /// bMirrorY= true Y镜像
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MirrorEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode MirrorEnt(int nCard,string strEntName, double dCenx, double dCeny, bool bMirrorX, bool bMirrorY);

        /// <summary>
        /// 对数据库中指定名称的对象进行旋转变换
        /// dCenx旋转中心的x坐标
        /// dCeny旋转中心的y坐标      
        /// dAngle为旋转角度,单位为弧度值
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_RotateEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode RotateEnt(int nCard,string strEntName, double dCenx, double dCeny, double dAngle);

        /// <summary>
        /// 得到用户加密狗的客户ID号
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetClientID", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort GetClientID(int nCard);

        /// <summary>
        /// 得到指定对象的尺寸信息
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetEntSize", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode GetEntSize(int nCard, string strEntName, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dz);

        /// <summary>
        /// 将指定对象移动到特对对象前面  多头没有此功能
        /// </summary>
        /// <param name="nMoveEnt"></param>
        /// <param name="GoalEnt"></param>
        /// <returns></returns>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MoveEntityBefore", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int MoveEntityBefore(int nCard, int nMoveEnt, int GoalEnt);


        /// <summary>
        /// 将指定对象移动到特定对象后面  多头没有此功能
        /// </summary>
        /// <param name="nMoveEnt"></param>
        /// <param name="GoalEnt"></param>
        /// <returns></returns>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_MoveEntityAfter", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int MoveEntityAfter(int nCard,int nMoveEnt, int GoalEnt);


        /// <summary>
        /// 20140228 将所有对象顺序颠倒  多头没有此功能
        /// </summary>
        /// <param name="nMoveEnt"></param>
        /// <param name="GoalEnt"></param>
        /// <returns></returns>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_ReverseAllEntOrder", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int ReverseAllEntOrder();

        /// <summary>
        /// 对数据库中指定名称的对象群组
        /// strEntName1被群组的对象1
        /// strEntName2被群组的对象2
        /// strGroupName群组后对象名      
        /// nGroupPen群组后对象笔号
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GroupEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int GroupEnt(int nCard,string strEntName1, string strEntName2, string strGroupName, int nGroupPen);

        /// <summary>
        /// 对数据库中指定名称的对象解散群组
        /// strEntName1被解散群组的对象
        /// </summary>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_UnGroupEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int UnGroupEnt(int nCard,string strGroupName);

        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GroupEnt2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int lmc1_GroupEnt2(int nCard,string[] strEntName, int nEntCount, string strGroupName, int nGroupPen);

        /// <summary>
        /// 彻底解散群组对象为曲线
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="nFlag"></param>
        /// <returns></returns>
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_UnGroupEnt2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int UnGroupEnt2(int nCard,string GroupName, int nFlag);

        #region 扩展轴的函数
        /// <summary>
        /// 下面是关于扩展轴的函数
        /// </summary> 

        /// <summary>
        /// 扩展轴移动到目标位置
        /// axis=0或1
        /// GoalPos目标位置,单位mm或inch
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AxisMoveTo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisMoveTo(int nCard,int axis, double GoalPos);


        /// <summary>
        /// 扩展轴回原点(校正原点)
        /// axis=0或1
        /// GoalPos目标位置
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AxisCorrectOrigin", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisGoHome(int nCard,int axis);

        /// <summary>
        /// 得到扩展轴的当前坐标
        /// axis=0或1
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_GetAxisCoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern double GetAxisCoor(int nCard,int axis);


        /// <summary>
        /// 扩展轴移动到脉冲目标位置
        /// axis=0或1
        /// nGoalPos目标位置,单位:脉冲
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AxisMoveToPulse", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern LmcErrCode AxisMoveToPulse(int nCard,int axis, int nGoalPos);

        /// <summary>
        /// 得到扩展轴的当前脉冲坐标
        /// axis=0或1
        /// </summary> 
        [DllImport("MultiMarkEzd", EntryPoint = "lmc1_AxisMoveToPulse", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetAxisCoorPulse(int nCard,int axis);
#endregion
    }
}
