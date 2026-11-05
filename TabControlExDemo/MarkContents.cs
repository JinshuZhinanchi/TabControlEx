using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading;

namespace TabControlExDemo
{
    public static class MarkStateData
    {
        //DM50=0设备未就绪    DM50=1待机中     DM50=2手动中    DM50	=3自动运行中     DM50	=4故障中
        public const int Ready = 0;//
        public const int Waiting = 1;//    Bool True    False 运行中
        public const int ManualMode = 2;// Bool    True False       待料
        public const int AutoMode = 3;//    Bool True    False 待处理
        public const int Warning = 4;// Bool    True False       结束
    }

    public static class UserTypeData
    {
        public const int Admin = 0;//管理员
        public const int PE = 1;//工程师
        public const int ME = 2;//技术员
        public const int OP = 3;//操作员
    }

    //工作流
    public static class WorkFlowState
    {
        public const int mInitial = 0;//初始化
        public const int mReady = 1;//就绪
        public const int mStart = 2;//plc start
        public const int mCCDState = 3;//ccd station1
        public const int mCCDPosition = 4;//ccd station2
        public const int mWorkStation1 = 5;//work station1
        public const int mWorkStation2 = 6;//work station2
        public const int mCCDCheckStart = 7;//work station2
        public const int mCCDCheckEnd = 8;//work station2
        public const int mWorkFlowEnd = 9;// workEnd
    }

    //admin ME  PE  OP
    public class UserData
    {
        public string userName = "admin";
        public string userPw = "123456";
        public int userType = UserTypeData.OP;

        public UserData()
        {
            userName = "OP";
            userType = UserTypeData.OP;
            userPw = "123456";
        }

        public UserData(string uName,int uType)
        {
            userName = uName;
            userType = uType;
            userPw = "123456";
        }

        public bool check(UserData uData)
        {
            bool bRet = false;
            if( uData.userName == userName && uData.userPw == userPw )
            {
                bRet = true;
            }
            return bRet;
        }
    }

    public class UserListData
    {
        public List<UserData> userDataList = new List<UserData>();
        public UserListData()
        {

        }

        public int CheckUserData(UserData uData)// 0 成功  1：用户密码错误 2：权限不足
        {
            int nRet = 2;
            UserData tempUser;
            for (int i = 0; i < userDataList.Count; i++)
            {
                tempUser = userDataList[i];
                if( uData.check(tempUser))
                {
                    if( tempUser.userType > uData.userType )
                    {
                        nRet = 1;
                    }
                    else
                    {
                        nRet = 0;
                        uData.userType = tempUser.userType;
                    }                    
                    break;
                }
            }
            return nRet;
        }

        public string GetPassword(UserData uData)
        {
            string pass = null ;
            UserData userData;

            for (int i = 0; i < userDataList.Count; i++)
            {
                userData = userDataList[i];
                if (userData.userName == uData.userName)
                {
                    uData.userType = userDataList[i].userType;
                    uData.userPw = userDataList[i].userPw;
                    pass =   userDataList[i].userPw;                  
                    break;
                }
            }
            return pass;
        }
        public void ModifyPassword(UserData uData )
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\UserData\\UserList.ini";

            string tagName;
            UserData userData;

            for (int i = 0; i < userDataList.Count; i++)
            {
                userData = userDataList[i];
                if( userData.userName == uData.userName )
                {
                    userDataList[i].userPw = uData.userPw;
               
                    tagName = string.Format("userPw{0}", i);
                    Program.WritePrivateProfileString("UserData", tagName, userData.userPw, fileName);

                    break;
                }                
            }
        }

        public void SaveUserListData()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\UserData\\UserList.ini";
           
            string tagName;
            UserData userData;                      

            tagName = string.Format("userCount");
            Program.WritePrivateProfileString("UserData", tagName, userDataList.Count.ToString(), fileName);

            for (int i = 0; i < userDataList.Count; i++)
            {
                userData = userDataList[i];
                tagName = string.Format("userName{0}", i);
                Program.WritePrivateProfileString("UserData", tagName, userData.userName, fileName);

                tagName = string.Format("userPw{0}", i);
                Program.WritePrivateProfileString("UserData", tagName, userData.userPw, fileName);

                tagName = string.Format("userType{0}", i);
                Program.WritePrivateProfileString("UserData", tagName, userData.userType.ToString(), fileName);
            }            
            
        }

        public void ReadUserListData()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\UserData\\UserList.ini";
            string tagName;
            string strValue;

            userDataList.Clear();

            StringBuilder temp = new StringBuilder(255);

            tagName = string.Format("userCount");
            Program.GetPrivateProfileString("UserData", tagName, "4", temp, 255, fileName);
            strValue = temp.ToString();
            int nCount = int.Parse(strValue);

            for (int i = 0; i < nCount; i++)
            {
                UserData userData = new UserData();

                tagName = string.Format("userName{0}", i);
                Program.GetPrivateProfileString("UserData", tagName, "OP", temp, 255, fileName);
                userData.userName  = temp.ToString();

                tagName = string.Format("userPw{0}", i);
                Program.GetPrivateProfileString("UserData", tagName, "12345", temp, 255, fileName);
                userData.userPw = temp.ToString();

                tagName = string.Format("userType{0}", i);
                Program.GetPrivateProfileString("UserData", tagName, "3", temp, 255, fileName);
                strValue = temp.ToString();
                userData.userType = int.Parse(strValue);

                userDataList.Add(userData);
            }
            
        }
    }

    public class MarkContentData
    {
        public string strWorkNumber = "";
        public string strLiaoHao;
        public string strCurrentSn;
        public string strName1;
        public string strName2;
    }

    public class ProduceCountData
    {
        public string strWorkTime;
        public string strFreeTime;
        public string strUPH;
        public string strGoodCount;
        public string strStripCount;
    }

    
    public class CCDCheckState
    {        
        public bool bHaveCheckState = false;
        public bool bHaveCheckPos = false;
        public string BoolProResult;//OK  NG
        public string BoolReadBarcode;// True False
        public string StrBarcodeContent;
        public double PX = 0;
        public double PY = 0;
        public double PA = 0;

        public bool isReadingZF = false;
        public bool isReadingPOS = false;

        public bool m_bPaizhaoEnable;
        public int m_nStartTickCount;
        public bool m_bStartEnable;


        public CCDCheckState()
        {
            initialData();

            isReadingZF = false;
            isReadingPOS = false;
        }
        public void initialData()
        {
            bHaveCheckState = false;
            bHaveCheckPos = false;
            BoolProResult = "NG";//OK  NG
            BoolReadBarcode = "False";// True False
            StrBarcodeContent = "";
            PX = 0;
            PY = 0;
            PA = 0;
        }

        public void WriteMaterialNumb(string strMaterNumb,string pnType ,string pnName)
        {
            string fileNamePZ = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\PN.ini";
            string strText = "PN";
            Program.WritePrivateProfileString("Config", strText, strMaterNumb, fileNamePZ);
            strText = "PNType";
            Program.WritePrivateProfileString("Config", strText, pnType, fileNamePZ);
            strText = "PNName";
            Program.WritePrivateProfileString("Config", strText, pnName, fileNamePZ);
        }

        public void WritePaiZhao1()
        {
            string fileNameOK = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\1OK.ini";
            if (File.Exists(fileNameOK) )
            {
                File.Delete(fileNameOK);
            }

            string fileName = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\Station1.ini";
            if (File.Exists(fileName) )
            {
                File.Delete(fileName);
            }

            //Thread.Sleep(200);
            PLCOpcFeedParameters.TickSleep(200);

            string fileNamePZ = System.Windows.Forms.Application.StartupPath  + "\\CCDStation\\1PZ.ini";
            string strText = "PZ";
            Program.WritePrivateProfileString("EzdIni", "EzdFileName", strText, fileNamePZ);

            PLCOpcFeedParameters.TickSleep(100);
        }

        public void WritePaiZhao2()
        {
            string fileNameOK = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\2OK.ini";
            if (File.Exists(fileNameOK))
            {
                File.Delete(fileNameOK);
            }

            string fileName = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\Station2.ini";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            Thread.Sleep(200);

            string fileNamePZ = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\2PZ.ini";
            string strText = "PZ";
            Program.WritePrivateProfileString("EzdIni", "EzdFileName", strText, fileNamePZ);
        }

        public void ReadCheckStateZF()
        {
            initialData();           

            string fileNameOK = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\1OK.ini";
            if (File.Exists(fileNameOK) == false)
            {
                return;
            }

            string fileName = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\Station1.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }
            isReadingZF = true;

            
            Thread.Sleep(200);

            string fileNamePZ = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\1PZ.ini";

            StringBuilder temp = new StringBuilder(255);

            Program.GetPrivateProfileString("Config", "BoolProResult", "NG", temp, 255, fileName);
            BoolProResult = temp.ToString();

            Program.GetPrivateProfileString("Config", "BoolReadBarcode", "False", temp, 255, fileName);
            BoolReadBarcode = temp.ToString();

            Program.GetPrivateProfileString("Config", "StrBarcodeContent", "", temp, 255, fileName);
            StrBarcodeContent = temp.ToString();

            File.Delete(fileNameOK);
            File.Delete(fileName);
            //File.Delete(fileNamePZ);

            bHaveCheckState = true;
        }

        public void ReadStation2()
        {
            initialData();

            string fileNameOK = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\2OK.ini";
            if (File.Exists(fileNameOK) == false)
            {
                return;
            }

            string fileName = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\Station2.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            string dstFileName = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\Station2Copy.ini";
            if (File.Exists(dstFileName) )
            {
                File.Delete(dstFileName);                
            }

            isReadingPOS = true;
            

            Thread.Sleep(200);

            File.Copy(fileName, dstFileName);

            string fileNamePZ = System.Windows.Forms.Application.StartupPath + "\\CCDStation\\1PZ.ini";

            StringBuilder temp = new StringBuilder(255);

            Program.GetPrivateProfileString("Config", "BoolProResult", "NG", temp, 255, fileName);
            BoolProResult = temp.ToString();

           // LmcDll.GetPrivateProfileString("Config", "BoolReadBarcode", "False", temp, 255, fileName);
           // BoolReadBarcode = temp.ToString();

           // LmcDll.GetPrivateProfileString("Config", "StrBarcodeContent", "", temp, 255, fileName);
           // StrBarcodeContent = temp.ToString();

            string strTemp;

            Program.GetPrivateProfileString("Config", "PX", "0", temp, 255, fileName);
            strTemp = temp.ToString();
            PX = Convert.ToDouble(strTemp);

            Program.GetPrivateProfileString("Config", "PY", "0", temp, 255, fileName);
            strTemp = temp.ToString();
            PY = Convert.ToDouble(strTemp);

            Program.GetPrivateProfileString("Config", "PA", "0", temp, 255, fileName);
            strTemp = temp.ToString();
            PA = Convert.ToDouble(strTemp);

            File.Delete(fileNameOK);
            File.Delete(fileName);

            //File.Delete(fileNamePZ);

            bHaveCheckPos = true;
        }
    }       

    public static class SECSWorkFlowParam
    {
        public  static int mStartIndex = 14001;//起始索引

        //public static int ProcessState = mStartIndex++;//当前工作状态
        //public static int PPexeName = mStartIndex++;//工艺名称
        //public static int ScanConternt = mStartIndex++;//上料盒ID
        //public static int ScanConterntCST = mStartIndex++;//收料盒ID
        //public static int ScanConterntPOD = mStartIndex++;//收料盒PortID   
        //public static int LotID = mStartIndex++;//批次名称        
        //public static int StartBoxNumb = mStartIndex++;
        //public static int MarkContent = mStartIndex++;//打标内容
        //public static int WaferStartSlot = mStartIndex++;//取料开始对应槽位
        //public static int WaferEndSlot = mStartIndex++;//取料结束对应槽位
        //public static int WaferWorkSlot = mStartIndex++;//工作位对应槽位
        //public static int ScanChipsContent = mStartIndex++;

        /// <summary>
        /// 运行状态
        /// </summary>
        public static int ProcessState = 14001;

        /// <summary>
        /// recipe名称
        /// </summary>
        public static int PPexeName = 14002;

        /// <summary>
        /// 打标内容
        /// </summary>
        public static int MarkContent = 14007;

        /// <summary>
        /// 打标结果
        /// </summary>
        public static int MarkResult = 14008;

        /// <summary>
        /// OCR检测内容
        /// </summary>
        public static int CheckResultContent = 14009;

        /// <summary>
        /// OCR检测结果
        /// </summary>
        public static int CheckResult = 14010;

        /// <summary>
        /// LOT ID
        /// </summary>
        public static int BatchName = 14012;

        /// <summary>
        /// MAP结果
        /// </summary>
        public static int FoupScanWaferResult = 14014;

        /// <summary>
        /// MAP 内容
        /// </summary>
        public static int FoupScanWaferContent = 14015;

        /// <summary>
        /// RFID结果
        /// </summary>
        public static int FoupRFIDReadResult = 14016;

        /// <summary>
        /// RFID结果
        /// </summary>
        public static int FoupRFIDReadContent = 14017;

        /// <summary>
        /// wafer在Foup中的位置，从1开始
        /// </summary>
        public static int WaferOderID = 14018;

        /// <summary>
        /// 料盒的位置
        /// </summary>
        public static int FoupID = 14019;

    }

    public static class SECSMarkCountData
    {
        public static int mStartIndex = 14011;//起始索引

        //public static int WorkTime = mStartIndex++;//s 工作时间
        //public static int FreeTime = mStartIndex++;//s 空闲时间
        //public static int UPH      = mStartIndex++;// 产能，每小时数量
        //public static int MTBA     = mStartIndex++;//小时/人工干预
        //public static int MTBF     = mStartIndex++;//小时/失败

    }

    public static class SECSMarkParam
    {
        public static int mStartIndex = 14020;//起始索引

        //public static int PenNO = mStartIndex++;//笔号
        //public static int MarkSpeed = mStartIndex++;//打标速度
        //public static int PowerRatio = mStartIndex++;//打标功率
        //public static int StartTC = mStartIndex++;//开光延时
        //public static int LaserOffTC = mStartIndex++;//关光延时
        //public static int MarkFreq = mStartIndex++;//打标频率

    }

    public class SECSAlarmData
    {
        // 1 duan 33001 -- 33006   6  2duan: 34000 -- 34110  111 (117) 3 duan:35000 -- 35028  29 (146)  4; 36000 -- 36036  37(183)  5: 36100 - 36171 172 (255)
        public const int WorkStateStartIndex         = 33001;

        public const int MRStartIndex = 34000;

        public const int MR3StartIndex = 35000;

        public const int MR5StartIndex = 36000;

        public const int RStartIndex = 36100;

        public static int nStartIndex = WorkStateStartIndex;

        //public static int RecipeSwitchFail            = nStartIndex++;    
        //public static int SmifLoadFail                = nStartIndex++;
        //public static int SmifUnLoadFail              = nStartIndex++;
        //public static int OpenCstMappingFail          = nStartIndex++;  
        //public static int UpArmCstTakeFail            = nStartIndex++;
        //public static int UpArmPlatePutFail           = nStartIndex++;
        //public static int DnArmPlateTakeFail          = nStartIndex++;  
        //public static int DnArmSmifPutFail            = nStartIndex++;
        //public static int WaferAlignFail              = nStartIndex++;
        //public static int PlateNotchPositionFail      = nStartIndex++;
        //public static int WaferMarkFail               = nStartIndex++;
        //public static int WaferCheckFail              = nStartIndex++;
        //public static int ServoAlarm                  = nStartIndex++;
        //public static int ServoFindHomeFail           = nStartIndex++;      
        //public static int RobotInitialFail            = nStartIndex++;          
        //public static int SmifInitialFail             = nStartIndex++;
        //public static int PlateInitialFail            = nStartIndex++;
        //public static int RobotRunningFail            = nStartIndex++;

        public static int FoupLoadFail = nStartIndex++;
        public static int FoupUnLoadFail = nStartIndex++;
        public static int FoupMappingFail = nStartIndex++;

        /// <summary>
        /// 上手臂取料失败报警
        /// </summary>
        public static int UpArmFoupTakeFail = nStartIndex++;
        public static int UpArmFoupPutFail = nStartIndex++;

        /// <summary>
        /// 下手臂料盒放料失败报警
        /// </summary>
        public static int DnArmFoupPutFail = nStartIndex++;

        /// <summary>
        /// 上手臂寻边器放料失败报警
        /// </summary>
        public static int UpArmAlignPutFail = nStartIndex++;

        /// <summary>
        /// OCR检测失败报警
        /// </summary>
        public static int AlignOCRReadFail = nStartIndex++;

        /// <summary>
        /// 寻边器定位失败报警
        /// </summary>
        public static int WaferAlignFail = nStartIndex++;

        /// <summary>
        /// 下手臂寻边器取料失败报警
        /// </summary>
        public static int UpArmAlignTakeFail = nStartIndex++;
        public static int UpArmPlatePutFail = nStartIndex++;
        public static int UpArmPlateTakeFail = nStartIndex++;
        public static int PlateNotchPositionFail = nStartIndex++;
        public static int PlateCirclePositionFail = nStartIndex++;
        public static int PlateTestHeightOver = nStartIndex++;
        public static int PlateTestPowerOver = nStartIndex++;
        public static int ServoAlarm = nStartIndex++;
        public static int LimitPosAlarm = nStartIndex++;
        public static int FindHomeFail = nStartIndex++;
        public static int RobotInitialFail = nStartIndex++;
        public static int FoupInitialFail = nStartIndex++;
        public static int AlignInitialFail = nStartIndex++;
        public static int PlateInitialFail = nStartIndex++;
        public static int RecipeSwitchFail = nStartIndex++;
        public static int RFIDReadFail = nStartIndex++;
    }

    public static class SECSEventData
    {
        public static int StartIndex     = 21001;//工序状态

        //public static int PorcessStateChanged = StartIndex++;
        //public static int WorkStart = StartIndex++;

        //public static int SendLotInfo = StartIndex++;//读取产品信息完成
        //public static int Recipe = StartIndex++;//切换Recipe完成

        //public static int LotStart = StartIndex++;//料盒开始

        //public static int ScanFoupWafers = StartIndex++;//扫片完成
        //public static int WaferStart = StartIndex++;//取片开始

        //public static int WaferAlignStart = StartIndex++;
        //public static int WaferAlignEnd = StartIndex++;

        //public static int LaserStart = StartIndex++;//打标开始
        //public static int LaserComplete = StartIndex++;//打标结束

        //public static int CheckLaserResultStart = StartIndex++;// 检测效果开始
        //public static int CheckLaserResultComplete = StartIndex++;//检测效果结束

        //public static int LotEnd = StartIndex++;//全部生产完成

        //public static int RecipeFail = StartIndex++;
        //public static int MarkContentOK = StartIndex++;
        //public static int MarkContentFailed = StartIndex++;
        //public static int WaferEnd = StartIndex++;


        public static int PorcessStateChanged = 21001;
        public static int WorkStart = 21002;

        /// <summary>
        /// RFID读取完成
        /// </summary>
        public static int ReadRFIDComplete = 21003;

        /// <summary>
        /// 切换Recipe完成
        /// </summary>
        public static int Recipe = 21004;

        /// <summary>
        /// LoadFoup 完成
        /// </summary>
        public static int LotStart = 21005;

        /// <summary>
        /// Foup 扫晶圆完成
        /// </summary>
        public static int ScanFoupWafers = 21006;

        /// <summary>
        /// Foup 取料完成
        /// </summary>
        public static int FoupTakeMaterial = 21007;
        
        /// <summary>
        /// 寻边器放料完成
        /// </summary>
        public static int WaferAlignPutMaterial = 21008;

        /// <summary>
        /// 寻边器取料完成
        /// </summary>
        public static int WaferAlignTakeMaterial = 21010;

        /// <summary>
        /// 寻边器定位完成
        /// </summary>
        public static int WaferPosition = 21013;

        /// <summary>
        /// 打标开始
        /// </summary>
        public static int LaserStart = 21014;

        /// <summary>
        /// 打标结束
        /// </summary>
        public static int LaserComplete = 21015;

        /// <summary>
        /// OCR检测开始
        /// </summary>
        public static int CheckLaserResultStart = 21016; 

        /// <summary>
        /// OCR检测结束
        /// </summary>
        public static int CheckLaserResultComplete = 21017;

        /// <summary>
        /// 下手臂放料完成
        /// </summary>
        public static int FoupOKPutMaterial = 21018;

        /// <summary>
        /// 一个晶圆生产完成
        /// </summary>
        public static int WaferEnd = 21020;

        /// <summary>
        /// 料盒生产完成
        /// </summary>
        public static int LotEnd = 21021;

        public static int WorkStop = 21022;

        /// <summary>
        /// 切换recipe失败
        /// </summary>
        public static int ReadProductionRecipeError = 21023;

        /// <summary>
        /// Foup 放料完成
        /// </summary>
        public static int FoupArrived = 21025;

        /// <summary>
        /// Foup 取料完成
        /// </summary>
        public static int FoupRemoved = 21026;

        /// <summary>
        /// 获取打标内容成功
        /// </summary>
        public static int MarkContentOK = 21027;

        /// <summary>
        /// 获取打标内容失败
        /// </summary>
        public static int MarkContentFailed = 21028;



    }


}
