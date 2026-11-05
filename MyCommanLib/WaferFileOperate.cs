using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyCommanLib
{        

    public class WaferFileOperate
    {
        public OneLaserMarkContent[] AllLaserMarkContent = new OneLaserMarkContent[2];

        public string strWaferCCDModuleName = "";        
        public int nProductIndex = 0;//0-16 对应机械手16个bank        
        public int nMarkType;//0:都不打    1：正面打标    2：侧面打标    3：两面都打
        public int nOCRGasPos = 0;//0:原点  1：动点

        /// <summary>
        /// 打标次数
        /// </summary>
        public int m_nMarkCount = 1;

        public int[] encryptDataWi = {3,7,9,10,5,8,4,2,1,6 };//对应为数字：0 - 9 的权重
        public Dictionary<string, int> encryptCharDataWi = new Dictionary<string, int>();//A,B,C,D...对应1，2，3，4...

        public ProductAxisData productAxisData = new ProductAxisData();

        public AxisOperateDataHelper axisData = new AxisOperateDataHelper();

        public WaferFileOperate()
        {
            
            AllLaserMarkContent[0] = new OneLaserMarkContent();
            AllLaserMarkContent[1] = new OneLaserMarkContent();

         
            nProductIndex = 0;
            nMarkType = 1;
            nOCRGasPos = 0;

            strWaferCCDModuleName = "";
         

            encryptCharDataWi.Add("A", 1);
            encryptCharDataWi.Add("B", 2);
            encryptCharDataWi.Add("C", 3);
            encryptCharDataWi.Add("D", 4);
            encryptCharDataWi.Add("E", 5);
            encryptCharDataWi.Add("F", 6);
            encryptCharDataWi.Add("G", 7);
            encryptCharDataWi.Add("H", 8);
            encryptCharDataWi.Add("I", 9);
        }
        
        public void InitialData()
        {                      
            nMarkType = 0;
            nOCRGasPos = 0;
            strWaferCCDModuleName = "";
            nProductIndex = 0;
        }

        public void ClearMarkContent(int nLaserType, int nBoxIndex)
        {           
            for (int j = 0; j < 25; j++)
            {
                SetMarkContent(nLaserType, nBoxIndex, j, "");
            }            
        }

        public Boolean IsMarkContentEmpty(int nLaserType, int nBoxIndex)
        {
            Boolean bEmpty = true;
            string strMark = "";
            for (int j = 0; j < 25; j++)
            {
                strMark = GetMarkContent(nLaserType, nBoxIndex, j);

                if( strMark.Length > 0 )
                {
                    bEmpty = false;
                }
            }
            return bEmpty;
        }

        public void SetMarkContent(int nLaserType, int i, int j, string strMark)
        {
            AllLaserMarkContent[nLaserType].boxMarkContent[i, j] = strMark;
        }

        public string GetMarkContent(int nLaserType, int i, int j)
        {
            string strMark = AllLaserMarkContent[nLaserType].boxMarkContent[i, j];
            return strMark;
        }

        public string GetEncryptVerifyCode(string strText)
        {
            string strVerifyCode = "";
            string strValue = "";
            int nAddValue = 0;
            int nValue = -1;
            for (int i = 0; i < strText.Length; i++)
            {
                strValue = strText.Substring(i, 1);
                if( string.Compare( strValue , "0") >=0 && string.Compare(strValue, "9") <= 0)
                {
                    nValue = int.Parse(strValue);
                    nAddValue = nAddValue + nValue * encryptDataWi[nValue];
                }else if( string.Compare(strValue, "A") >= 0 && string.Compare(strValue,"I") <= 0 )
                {
                    encryptCharDataWi.TryGetValue(strValue, out nValue);
                    nAddValue = nAddValue + nValue * encryptDataWi[nValue];
                }
            }

            if( nAddValue >= 0)
            {
                int t = nAddValue % 11;
                int r = (12 - t) % 11;
                if( r == 10)
                {
                    strValue = "X";
                }else
                {
                    strValue = r.ToString();
                }

                strVerifyCode = strValue;
            }

            return strVerifyCode;
        }

        public Boolean WriteFile(string strWaferFileName )
        {
            Boolean bRet = true;
            string fileName = strWaferFileName;

            string keyName;
            string keyValue;

            for (int i = 0; i < AllLaserMarkContent.Length; i++)
            {

                AllLaserMarkContent[i].WriteFile(strWaferFileName, i);
            }

            keyName = string.Format("nProductIndex");
            keyValue = nProductIndex.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);
          
            keyName = string.Format("nMarkType");
            keyValue = nMarkType.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("strWaferCCDModuleName");
            keyValue = strWaferCCDModuleName;
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("nOCRGasPos");
            keyValue = nOCRGasPos.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("nMarkCount");
            keyValue = m_nMarkCount.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            return bRet;
        }

        public Boolean ReadFile(string strWaferFileName)
        {
            Boolean bRet = true;
            string fileName = strWaferFileName;
            if( File.Exists(fileName) == false)
            {
                return false;
            }
            string keyName;
            string keyValue;

            StringBuilder temp = new StringBuilder(255);
         
            for (int i = 0; i < AllLaserMarkContent.Length; i++)
            {

                AllLaserMarkContent[i].ReadFile(strWaferFileName, i);
            }

            keyName = string.Format("nProductIndex");
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                nProductIndex = int.Parse(keyValue);
            }
            catch (Exception)
            {

            }            

            keyName = string.Format("nMarkType");
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                nMarkType = int.Parse(keyValue);
            }
            catch (Exception)
            {
            }

            keyName = string.Format("nOCRGasPos");
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            int.TryParse(keyValue, out nOCRGasPos);

            keyName = string.Format("strWaferCCDModuleName");
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            strWaferCCDModuleName = temp.ToString();

            keyName = string.Format("nMarkCount");
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            m_nMarkCount = int.Parse(keyValue);

            return bRet;
        }
    }

    public class OneLaserMarkContent
    {
        public string strEzdName = "";

        public string sampleCode = "JR49BG00001";
        public string fixedCode = "JR49BG";
        public string snCode = "00001";

        public Boolean bCCDCheck = true;

        public string fVPosOffset = "90";

        public int nEncriptType = 0;//0:不加密  1：天通 17位身份证加密算法

        /// <summary>
        /// 打标寻边器角度；
        /// </summary>
        public int nMarkAngle = 0;

        /// <summary>
        /// 检测寻边器角度；
        /// </summary>
        public int nCheckAngle = 0;

        /// <summary>
        /// 晶圆种类
        /// </summary>
        public int nWaferType = 0;

        /// <summary>
        /// 激光器重复频率
        /// </summary>
        public int nPRF = 0;

        /// <summary>
        /// 激光器功率系数
        /// </summary>
        public int nMCS = 0;
        /// <summary>
        /// 最小功率
        /// </summary>
        public double dPowerMin = 0;
        /// <summary>
        /// 最小功率
        /// </summary>
        public double dPowerMax = 20;


        public Boolean bPreventRepeat = false;

        public string[,] boxMarkContent = new string[8, 25];

        public OneLaserMarkContent()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    boxMarkContent[i, j] = "";
                }
            }
        }

        public Boolean WriteFile(string strWaferFileName, int nLaserType )
        {
            Boolean bRet = true;
            string fileName = strWaferFileName;

            string keyName;
            string keyValue;

            keyName = string.Format("strEzdName{0}", nLaserType);
            keyValue = strEzdName;
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("sampleCode{0}", nLaserType);
            keyValue = sampleCode;
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("fixedCode{0}", nLaserType);
            keyValue = fixedCode;
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("snCode{0}", nLaserType);
            keyValue = snCode;
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("nEncriptType{0}", nLaserType);
            keyValue = nEncriptType.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);
            

            keyName = string.Format("bPreventRepeat{0}", nLaserType);
            keyValue = bPreventRepeat.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            keyName = string.Format("bCCDCheck{0}", nLaserType);
            keyValue = bCCDCheck.ToString();
            CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

            if(nLaserType == 0)
            {
                keyName = string.Format("nMarkAngle");
                keyValue = nMarkAngle.ToString();
                bool b = CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("nCheckAngle");
                keyValue = nCheckAngle.ToString();
                bool b2 = CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("nWaferType");
                keyValue = nWaferType.ToString();
                CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("nPRF");
                keyValue = nPRF.ToString();
                CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("nMCS");
                keyValue = nMCS.ToString();
                CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("dPowerMin");
                keyValue = dPowerMin.ToString();
                CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);

                keyName = string.Format("dPowerMax");
                keyValue = dPowerMax.ToString();
                CommonDefine.WritePrivateProfileString("WaferData", keyName, keyValue, fileName);
            }

            return bRet;
        }

        public Boolean ReadFile(string strWaferFileName, int nLaserType )
        {
            Boolean bRet = true;
            string fileName = strWaferFileName;
            if (File.Exists(fileName) == false)
            {
                return false;
            }

            string keyName;
            string keyValue;

            StringBuilder temp = new StringBuilder(255);

            keyName = string.Format("strEzdName{0}", nLaserType );
            CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                strEzdName = keyValue;
            }
            catch (Exception)
            {
            }

           
            keyName = string.Format("sampleCode{0}", nLaserType);
            int i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                sampleCode = keyValue;
            }
            catch (Exception)
            {
            }

            keyName = string.Format("fixedCode{0}", nLaserType );
            i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                fixedCode = keyValue;
            }
            catch (Exception)
            {
            }

            keyName = string.Format("snCode{0}", nLaserType);
            i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                snCode = keyValue;
            }
            catch (Exception)
            {

            }

            keyName = string.Format("nEncriptType{0}", nLaserType);
            i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                nEncriptType = int.Parse( keyValue );
            }
            catch (Exception)
            {

            }

            
            keyName = string.Format("bPreventRepeat{0}", nLaserType);
            i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                bPreventRepeat = Boolean.Parse( keyValue );
            }
            catch (Exception)
            {
            }

            keyName = string.Format("bCCDCheck{0}", nLaserType);
            i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "0", temp, 255, fileName);
            keyValue = temp.ToString();
            try
            {
                bCCDCheck = Boolean.Parse(keyValue);
            }
            catch (Exception)
            {

            }

            if(nLaserType == 0)
            {
                keyName = string.Format("nMarkAngle");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    nMarkAngle = int.Parse(keyValue);
                }
                catch (Exception)
                {

                }

                keyName = string.Format("nCheckAngle");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    nCheckAngle = int.Parse(keyValue);
                }
                catch (Exception)
                {

                }

                keyName = string.Format("nWaferType");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    nWaferType = int.Parse(keyValue);
                }
                catch (Exception)
                {

                }

                keyName = string.Format("nPRF");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    nPRF = int.Parse(keyValue);
                }
                catch (Exception)
                {

                }


                keyName = string.Format("nMCS");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    nMCS = int.Parse(keyValue);
                }
                catch (Exception)
                {

                }

                keyName = string.Format("dPowerMin");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    dPowerMin = double.Parse(keyValue);
                }
                catch (Exception)
                {

                }

                keyName = string.Format("dPowerMax");
                i = CommonDefine.GetPrivateProfileString("WaferData", keyName, "", temp, 255, fileName);
                keyValue = temp.ToString();
                try
                {
                    dPowerMax = double.Parse(keyValue);
                }
                catch (Exception)
                {

                }
            }
            

            return bRet;
        }
    }
}
