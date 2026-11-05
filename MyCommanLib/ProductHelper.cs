using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyCommanLib
{
    
    public class ProductHelper
    {
        public List<string> m_ProductList = new List<string>();
        public List<double> m_ProductHeightList = new List<double>();
        

        public ProductHelper()
        {
            ReadProductList();
        }

        public void ReadProductHeightList()
        {
            m_ProductHeightList.Clear();

            double dValue = 1.0;
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ProductSetting\\PNListHeight.ini";
            if (File.Exists(fileName) == false)
            {
                m_ProductHeightList.Add(dValue);

                for (int i = 0; i < m_ProductList.Count; i++)
                {
                    m_ProductHeightList.Add(dValue);
                }
                return;
            }

            StringBuilder temp = new StringBuilder(255);
            string strValue = "1.0";

            string strText = "PDFNHEIGHT";
            CommonDefine.GetPrivateProfileString("Config", strText, "1.0", temp, 255, fileName);
            strValue = temp.ToString();

            double dNormalValue = 1.0;
            try
            {
                if (strValue.Length <= 0)
                {
                    strValue = "1.0";
                }

                dValue = double.Parse(strValue);
            }
            catch
            {
                dValue = 1.0;
            }

            dNormalValue = dValue;

            m_ProductHeightList.Add(dValue);

            for (int i = 0; i < m_ProductList.Count; i++)
            {

                CommonDefine.GetPrivateProfileString("Config", strText, "", temp, 255, fileName);
                strValue = temp.ToString();

                try
                {
                    if (strValue.Length <= 0)
                    {
                        dValue = dNormalValue;
                    }
                    else
                    {
                        dValue = double.Parse(strValue);
                    }
                }
                catch
                {
                    dValue = dNormalValue;
                }

                m_ProductHeightList.Add(dValue);
            }

        }

        public void ReadProductList()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ProductSetting\\PNList.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            m_ProductList.Clear();

            StreamReader reader = new StreamReader(fileName);

            string strText = "";
            //// 一行一行读取
            strText = reader.ReadLine();

            while (strText != null)
            {
                strText.Trim();
                strText.TrimEnd('\r', '\n');

                if (strText.Length > 0)
                {
                    m_ProductList.Add(strText);
                }
                strText = reader.ReadLine();
            }
            reader.Close();

            ReadProductHeightList();
        }
    }


    public class ProductAxisData
    {
        public int nWaferX1 = 0;
        public int nWaferY1 = 0;
        public int nWaferR1 = 0;
        
        public int nWaferX2 = 0;
        public int nWaferY2 = 0;
        public int nWaferR2 = 0;
        
        public int nMarkX1 = 0;
        public int nMarkY1 = 0;
        public int nMarkZ1 = 0;
        
        public int nMarkX2 = 0;
        public int nMarkY2 = 0;
        public int nMarkZ2 = 0;

        public ProductAxisData()
        {
            nWaferX1 = 0;
            nWaferY1 = 0;
            nWaferR1 = 0;

            nWaferX2 = 0;
            nWaferY2 = 0;
            nWaferR2 = 0;

            nMarkX1 = 0;
            nMarkY1 = 0;
            nMarkZ1 = 0;

            nMarkX2 = 0;
            nMarkY2 = 0;
            nMarkZ2 = 0;
        }

        public void Write(string strFileName )
        {           
            string strKey = "";
            string strText = "";

            string filePath = System.Windows.Forms.Application.StartupPath + "\\ProductSetting";
            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }
            string filePathName = filePath + "\\" + strFileName + ".pt";

            strKey = "fWaferX1";
            strText = nWaferX1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fWaferY1";
            strText = nWaferY1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fWaferR1";
            strText = nWaferR1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);

            strKey = "fWaferX2";
            strText = nWaferX2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fWaferY2";
            strText = nWaferY2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fWaferR2";
            strText = nWaferR2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);


            strKey = "fMarkX1";
            strText = nMarkX1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fMarkY1";
            strText = nMarkY1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fMarkZ1";
            strText = nMarkZ1.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);

            strKey = "fMarkX2";
            strText = nMarkX2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fMarkY2";
            strText = nMarkY2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);
            strKey = "fMarkZ2";
            strText = nMarkZ2.ToString();
            CommonDefine.WritePrivateProfileString("PositionData", strKey, strText, filePathName);

        }

        public Boolean Read(string strFileName)
        {
            Boolean bRet = true;

            string strKey = "";
            string strText = "";

            string filePath = System.Windows.Forms.Application.StartupPath + "\\ProductSetting";
            if (Directory.Exists(filePath) == false)
            {
                return false;
            }
            string filePathName = filePath + "\\" + strFileName + ".pt";
            if( File.Exists(filePathName) == false )
            {
                return false;
            }

            StringBuilder temp = new StringBuilder(255);

            strKey = "fWaferX1";
            strText = nWaferX1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferX1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }


            strKey = "fWaferY1";
            strText = nWaferY1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferY1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fWaferR1";
            strText = nWaferR1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferR1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fWaferX2";
            strText = nWaferX2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferX2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fWaferY2";
            strText = nWaferY2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferY2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }


            strKey = "fWaferR2";
            strText = nWaferR2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nWaferR2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fMarkX1";
            strText = nMarkX1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkX1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fMarkY1";
            strText = nMarkY1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkY1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fMarkZ1";
            strText = nMarkZ1.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkZ1 = int.Parse(strText);
            }
            catch (Exception)
            {
            }


            strKey = "fMarkX2";
            strText = nMarkX2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkX2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fMarkY2";
            strText = nMarkY2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkY2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }

            strKey = "fMarkZ2";
            strText = nMarkZ2.ToString();
            CommonDefine.GetPrivateProfileString("PositionData", strKey, strText, temp, 255, filePathName);
            strText = temp.ToString();
            try
            {
                nMarkZ2 = int.Parse(strText);
            }
            catch (Exception)
            {
            }
           
            return bRet;
        }
    }
}
