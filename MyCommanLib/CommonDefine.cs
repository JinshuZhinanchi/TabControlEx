using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MyCommanLib
{
    public static class CommonDefine
    {

        public static SecsData m_SecsData = new SecsData();

        public static int USER_OP = 0;
        public static int USER_TP = 1;
        public static int USER_EP = 2;
        public static int USER_ADMIN = 3;

        public static WaferFileOperate waferFileInfo = new WaferFileOperate();
        public static ProductHelper    productHelper = new ProductHelper();

        public static MarkStruct m_PosStruct = new MarkStruct();
        public static string m_posMemeryFile = "posMemeryFile";

        public static MarkStruct m_CheckStruct = new MarkStruct();
        public static string m_checkMemeryFile = "checkMemeryFile"; //检测


        public static MarkStruct m_CCD2MarkPos = new MarkStruct();//值为：CCD Pos - Mark Pos

        public static MarkStruct m_RobotPos = new MarkStruct();// fx, LP1 位置  fx2 LP2 位置 fy Aigner位置

        /// <summary>
        /// 是否接收远程消息
        /// </summary>
        public static bool bAccept = true;

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

        public static int WM_SETREDRAW = 0x000B; //uint WM_SETREDRAW
        public static int WS_EX_COMPOSITED = 0x02000000;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); //UInt32 Msg

        //TickCount 0 --- 32位正的最大，瞬变为32位最小-----0
        public static int DelayedTime(int startTick)
        {
            int nDelay = 0;
            int startTime = startTick;
            int endTime = System.Environment.TickCount;

            if (endTime >= startTime)
            {
                nDelay = endTime - startTime;
            }
            else
            {
                nDelay = (Int32.MaxValue - startTime) + (Int32.MaxValue - Math.Abs(endTime));
            }

            return nDelay;
        }

        public static void TickSleep(int msSecond)
        {
            if (msSecond <= 0)
                msSecond = 1;

            int startTime = System.Environment.TickCount;
            int endTime = System.Environment.TickCount + msSecond;
            int ticks = System.Environment.TickCount;

            Boolean outMax = false;
            if (endTime < startTime)
            {
                outMax = true;
            }

            Boolean timeOut = false;
            while (timeOut == false)
            {
                ticks = System.Environment.TickCount;
                if (outMax)
                {
                    if (ticks < startTime)
                        outMax = false;
                }

                if (!outMax)
                {
                    if (ticks > endTime || ticks <= 0 && endTime > 0 && startTime > 0)
                    {
                        timeOut = true;
                    }
                }
            }
        }

        // mycsvdt 用来装csv文件中数据的datatable,filepath是指csv文件的路径
        public static bool OpenCSVFile(ref DataTable mycsvdt, string filepath)
        {
            string strpath = filepath; //csv文件的路径
            if (File.Exists(strpath) == false)
            {
                return false;
            }
            try
            {
                mycsvdt.Clear();
                mycsvdt.Columns.Clear();

                int intColCount = 0;
                bool blnFlag = true;

                DataColumn mydc;
                DataRow mydr;

                string strline;
                string[] aryline;
                StreamReader mysr = new StreamReader(strpath, System.Text.Encoding.Default);

                while ((strline = mysr.ReadLine()) != null)
                {
                    aryline = strline.Split(',');

                    //给datatable加上列名
                    if (blnFlag)
                    {
                        blnFlag = false;
                        intColCount = aryline.Length;

                        int col = 0;
                        for (int i = 0; i < intColCount; i++)
                        {
                            col = i + 1;
                            mydc = new DataColumn(col.ToString());
                            mycsvdt.Columns.Add(mydc);
                        }
                    }

                    //填充数据并加入到datatable中
                    mydr = mycsvdt.NewRow();
                    for (int i = 0; i < intColCount; i++)
                    {
                        mydr[i] = aryline[i];
                    }
                    mycsvdt.Rows.Add(mydr);
                }

                mysr.Close();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void SetTableToDgv(DataTable dt, ref ListView listView)
        {
            if (dt.Columns.Count < 3)
                return;

            int ndtColums = dt.Columns.Count;

            int i = -1;
            foreach (DataRow row in dt.Rows)//逐个读取单元格的内容；
            {
                if (i >= 0)//忽略dt第一行
                {
                    ListViewItem lvItem = new ListViewItem();

                    lvItem.UseItemStyleForSubItems = false;
                    for (int j = 0; j < ndtColums; j++)
                    {

                        if (j == 0)
                        {
                            //lvItem.ForeColor = textColor;
                            lvItem.Text = row[j].ToString();
                            lvItem.StateImageIndex = 0;
                        }
                        else
                        {
                            lvItem.SubItems.Add(row[j].ToString());

                        }
                    }
                    listView.Items.Add(lvItem);
                }
                i++;
            }
        }

        public static void SaveWorkLogs(string strText)
        {
            DateTime tt = DateTime.Now;
            string strKey = string.Format("{0:D2}:{1:D2}:{2:D2}-{3:D3}", tt.Hour, tt.Minute, tt.Second, tt.Millisecond);
            string fileName = string.Format("{0}-{1:D2}-{2:D2}", tt.Year, tt.Month, tt.Day);

            string filePath = System.Windows.Forms.Application.StartupPath + "\\Worklogs";
            if( Directory.Exists(filePath) == false )
            {
                Directory.CreateDirectory(filePath);
            }
            string filePathName = filePath + "\\" + fileName + ".log";


            WritePrivateProfileString("WorkLogs", strKey, strText, filePathName);

        }

        public static void SaveComLogs(string strText)
        {
            DateTime tt = DateTime.Now;
            string strKey = string.Format("{0:D2}:{1:D2}:{2:D2}-{3}", tt.Hour, tt.Minute, tt.Second, tt.Millisecond);
            string fileName = string.Format("{0}-{1:D2}-{2:D2}-{3:D2}", tt.Year, tt.Month, tt.Day, tt.Hour);

            string filePath = System.Windows.Forms.Application.StartupPath + "\\Comlogs";
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }
            string filePathName = filePath + "\\" + fileName + ".log";

            WritePrivateProfileString("ComLogs", strKey, strText, filePathName);

        }

        public static void SaveAlarmLogs(string strText)
        {
            DateTime tt = DateTime.Now;
            string strKey = string.Format("{0:D2}:{1:D2}:{2:D2}-{3:D3}", tt.Hour, tt.Minute, tt.Second, tt.Millisecond);
            string fileName = string.Format("{0}-{1:D2}-{2:D2}", tt.Year, tt.Month, tt.Day);

            string filePath = System.Windows.Forms.Application.StartupPath + "\\AlarmLogs";
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }
            string filePathName = filePath + "\\" + fileName + ".log";


            WritePrivateProfileString("AlarmLogs", strKey, strText, filePathName);

        }

        public static void WriteRobotPosIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezd.ini";
            string strText = "";

            strText = CommonDefine.m_RobotPos.fx.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", "RobotPosX", strText, fileName);

            strText = CommonDefine.m_RobotPos.fx2.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", "RobotPosX2", strText, fileName);

            strText = CommonDefine.m_RobotPos.fy.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", "RobotPosY", strText, fileName);
        }

        public static void GetRobotPosIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezd.ini";

            StringBuilder temp = new StringBuilder(255);
            string strValue = "0";
            double fValue = 0;

            CommonDefine.GetPrivateProfileString("EzdIni", "RobotPosX", strValue, temp, 255, fileName);
            strValue = temp.ToString();
            double.TryParse(strValue, out fValue);
            CommonDefine.m_RobotPos.fx = fValue;

            CommonDefine.GetPrivateProfileString("EzdIni", "RobotPosX2", strValue, temp, 255, fileName);
            strValue = temp.ToString();
            double.TryParse(strValue, out fValue);
            CommonDefine.m_RobotPos.fx2 = fValue;

            CommonDefine.GetPrivateProfileString("EzdIni", "RobotPosY", strValue, temp, 255, fileName);
            strValue = temp.ToString();
            double.TryParse(strValue, out fValue);
            CommonDefine.m_RobotPos.fy = fValue;

        }

        public static Boolean WriteMemeryFile(string fileName, MarkStruct markStruct)
        {
            Boolean bRet = true;
            try
            {
                //定义内存大小
                int size = 1024;

                //创建共享内存
                MemoryMappedFile shareMemory = MemoryMappedFile.CreateOrOpen(fileName, size);

                var stream = shareMemory.CreateViewStream(0, size);

                byte[] rawdatas = rawSerialize(markStruct);

                stream.Write(rawdatas, 0, rawdatas.Length);
                stream.Close();
            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }

        private static byte[] rawSerialize(MarkStruct markStruct)
        {
            int rawsize = Marshal.SizeOf(markStruct);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            Marshal.StructureToPtr(markStruct, buffer, false);

            byte[] rawdatas = new byte[rawsize];

            Marshal.Copy(buffer, rawdatas, 0, rawsize);

            Marshal.FreeHGlobal(buffer);

            return rawdatas;
        }

        private static MarkStruct rawDeserialize(byte[] rawdatas)
        {
            Type anytype = typeof(MarkStruct);

            int rawsize = Marshal.SizeOf(anytype);

            if (rawsize > rawdatas.Length) return new MarkStruct();

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            Marshal.Copy(rawdatas, 0, buffer, rawsize);

            object retobj = Marshal.PtrToStructure(buffer, anytype);

            Marshal.FreeHGlobal(buffer);

            return (MarkStruct)retobj;
        }

        public static Boolean ReadMemeryFile(string fileName, ref MarkStruct markStruct)
        {
            Boolean bRet = true;
            try
            {
                //定义内存大小
                int size = 1024;

                //创建共享内存
                MemoryMappedFile shareMemory = MemoryMappedFile.OpenExisting(fileName);

                var stream = shareMemory.CreateViewStream(0, size);

                int rawsize = Marshal.SizeOf(markStruct);

                byte[] rawdatas = new byte[rawsize];

                stream.Read(rawdatas, 0, rawsize);

                markStruct = rawDeserialize(rawdatas);

                stream.Close();
            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }

        public static void WriteMaterialNumb(string iniFileName, string strMaterNumb, string pnType, string pnName)
        {
            string fileName = iniFileName;

            string strText = "PN";
            WritePrivateProfileString("Config", strText, strMaterNumb, fileName);
            strText = "PNType";
            WritePrivateProfileString("Config", strText, pnType, fileName);
            strText = "PNName";
            WritePrivateProfileString("Config", strText, pnName, fileName);
        }

        public static void InitialMarkStruct(ref MarkStruct myStruct)
        {
            myStruct.m_nPaizhaoStart = 1;

            myStruct.m_nResult = 0;

            myStruct.fx = 0;
            myStruct.fy = 0;
            myStruct.fr = 0;

            myStruct.m_nPaizhaoEnd = 0;
        }

        public static string CreateM7Code(string strCode)
        {

            int nMode = 59;
            strCode = strCode + "A0";

            int nValue = 0;
            int x = 8;
            int a = 0;
            for (int i = 0; i < strCode.Length; i++)
            {
                a = strCode[i] - 32;
                nValue = a + x * nValue;
                nValue = nValue % nMode;
            }

            string strR = "A0";
            if (nValue > 0)
            {
                nValue = nMode - nValue;

                //高低位互换            
                int nValue1 = nValue & 0x07;//后3位
                int nVa = nValue & 0x38;//111000
                int nValue2 = (nVa >> 3); //前3位

                Char a11 = (Char)(nValue2 + 'A');
                Char a12 = (Char)(nValue1 + '0');

                Char[] aR = new Char[2];
                aR[0] = a11;
                aR[1] = a12;

                strR = new string(aR);
            }

            //MessageBox.Show( strR );

            return strR;
        }        
    }

    public class DoublePoint
    {
        public double fx = 0;
        public double fy = 0;
        public double fz = 0;
        public double fr = 0;
        public DoublePoint()
        {
            fx = 0;
            fy = 0;
            fz = 0;
            fr = 0;
        }

        public DoublePoint(DoublePoint dp)
        {
            fx = dp.fx;
            fy = dp.fy;
            fz = dp.fz;
            fr = dp.fr;
        }

        public Boolean Write(string fileName, string parentKeyName)
        {
            Boolean bRet = false;

            string keyName = parentKeyName;
            string keyValue = "0";

            keyName = string.Format("{0}-X", parentKeyName);
            keyValue = fx.ToString("F4");
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("{0}-Y", parentKeyName);
            keyValue = fy.ToString("F4");
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("{0}-Z", parentKeyName);
            keyValue = fz.ToString("F4");
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("{0}-R", parentKeyName);
            keyValue = fr.ToString("F4");
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);
            return false;
        }

        public Boolean Read(string fileName, string parentKeyName)
        {
            Boolean bRet = false;

            StringBuilder temp = new StringBuilder(255);
            string keyName = parentKeyName;
            string keyValue = "0";


            keyName = string.Format("{0}-X", parentKeyName);
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            double.TryParse(keyValue, out fx);


            keyName = string.Format("{0}-Y", parentKeyName);
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            double.TryParse(keyValue, out fy);

            keyName = string.Format("{0}-Z", parentKeyName);
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            double.TryParse(keyValue, out fz);

            keyName = string.Format("{0}-R", parentKeyName);
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            double.TryParse(keyValue, out fr);

            return true;
        }
    }

    public class SecsData
    {
        public string recipeDir = "";

        public string[] strMarkContent = new string[25];
        /// <summary>
        /// 料盒ID的recipe
        /// </summary>
        //public string recipeName = "";
        public string[] recipeNameArray = new string[2];

        /// <summary>
        /// 料盒的打标内容
        /// </summary>
        public string strMappingContent = "";

        /// <summary>
        /// 当前工作料盒索引
        /// </summary>
        public int nBoxIndexWork = 0;

        /// <summary>
        /// 料盒索引
        /// </summary>
        public int nBoxIndexOther = 0;

        public string strCurrentMapContent = "";


        public void InitialData()
        {
            //strLotID = "";
            //strMappingContent = "";
            //recipeName = "";
            for (int i = 0; i < 8; i++)
            {
                strMarkContent[i] = "";
                recipeNameArray[i] = "";
                //strMappingContent[i] = "";
            }
        }
    };

    public class MarkChipsStruct
    {
        public Boolean bSelect;
        public string strMarkContent;

        public MarkChipsStruct()
        {
            bSelect = false;
            strMarkContent = "";
        }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MarkStruct
    {
        public int m_nPaizhaoStart;// 0: 无  1：拍照 (正面) 2; 检测（正面）
                                   // 3：拍照 (侧面) 4; 检测（侧面） 5：寻边定位  6; 寻边打标定位
        public int m_nResult;//0:NG   1: OK

        public double CurPosX;
        public double CurPosY;

        public double fx;
        public double fy;
        public double fr;

        public double fx2;
        public double fy2;
        public double fr2;

        public int m_nPaizhaoEnd;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string strContent;
    };

    
    //---------防止程序多次运行----------//
    public abstract class OneInStance
    {
        /// <summary>
        /// 判断程序是否正在运行
        /// </summary>
        /// <param name="appld">程序名称</param>
        /// <returns>如果程序第一次运行返回TRUE否则返回False</returns>
        public static bool IsFirst(string appId)
        {
            bool ret = false;

            if (OpenMutex(0x1F0001, 0, appId) == IntPtr.Zero)
            {
                CreateMutex(IntPtr.Zero, 0, appId);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 为现有的一个已命名互斥体对象创建一个新句柄
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle">如希望子进程能够继承句柄，则为TRUE</param>
        /// <param name="lpName">要打开对象的名字</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr OpenMutex(
             uint dwDesiredAccess, // access
             int bInheritHandle, // inheritance option
             string lpName // object name
         );

        /// <summary>
        /// 找出当前系统是否已经存在指定进程的实例。
        /// 如果没有则创建一个互斥体。
        /// CreateMutex()函数可用来创建一个有名或无名的互斥量对象：
        /// </summary>
        /// <param name="lpMutexAttributes">指向安全属性的指针</param>
        /// <param name="bInitialOwner">初始化互斥对象的所有者</param>
        /// <param name="lpName">指向互斥对象名的指针</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateMutex(
         IntPtr lpMutexAttributes, // SD
         int bInitialOwner, // initial owner
         string lpName // object name
         );
    }
}
