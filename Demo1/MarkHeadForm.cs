using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LmcLib;
using MyCommanLib;

namespace EzdDemo
{
    public partial class MarkHeadForm : Form
    {
        public int m_nCardAddress = 0;
        public string m_MoudFilePath = "";

        public MarkForm markForm;
        public Boolean m_bShowRedLight = false;
        public int m_nLaserType = 0;
        public int m_nEncryptionType = 0;
        public int m_nSNLength = 12;

        public MarkHeadForm( )
        {
            InitializeComponent();                       

            comboBoxEncriptType.SelectedIndex = 0;            
        }

        public void InitialHeadForm(int nLaserType ,int nCard)
        {
            m_nCardAddress = nCard;

            GetPrivateIni(nLaserType);

            m_nLaserType = nLaserType;
            if (m_nLaserType == 0)
            {
                //labelPowerPulse.Text = "功率：";
            }
            else
            {
                labelPowerPulse.Text = "脉宽：";
            }

            
        }

        private void buttonOpenEzd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(EzdFiletextBox.Text);// Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.Filter = "Ezd files (*.ezd)|*.ezd";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int nCard = m_nCardAddress;
                
                EzdFiletextBox.Text = dlg.FileName;
                m_MoudFilePath = System.IO.Path.GetDirectoryName(EzdFiletextBox.Text);
                if (markForm.loadEzdFile(EzdFiletextBox.Text, nCard))
                {
                    string strEnt = "code";
                    string strText = LmcMultiDll.GetTextByName(nCard, strEnt);

                    textBoxSample.Text = strText;

                    int nStart = textBoxSnFixed.Text.Length;

                    string strFixed = strText;
                    if (nStart < strText.Length && nStart > 0)
                    {
                        strFixed = strText.Substring(0, nStart);
                    }
                    //SetChipInfoHandler(strFixed, null);
                }
            }
        }

        private void buttonLaserParam_Click(object sender, EventArgs e)
        {
            int nCard = m_nCardAddress;
            LmcMultiDll.SetDevCfg(nCard);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int nCard = m_nCardAddress;

            int nPenNo = comboBoxPenNO.SelectedIndex;
            if( nPenNo < 0 )
            {
                return;
            }


            if (nPenNo >= 0)
            {
               
                int nMarkLoop = 1;//加工次数
                double dMarkSpeed = 100;//标刻次数mm/s
                double dPowerRatio = 50;//功率百分比(0-100%)	
                double dCurrent = 0.5;//电流A
                int nFreq = 500;//频率HZ
                double dQPulseWidth = 10;//Q脉冲宽度us	
                int nStartTC = 200;//开始延时us
                int nLaserOffTC = 200;//激光关闭延时us 
                int nEndTC = 200;//结束延时us
                int nPolyTC = 200;//拐角延时us   //	
                double dJumpSpeed = 200; //跳转速度mm/s
                int nJumpPosTC = 200; //跳转位置延时us
                int nJumpDistTC = 200;//跳转距离延时us	
                double dEndComp = 20;//末点补偿mm
                double dAccDist = 20;//加速距离mm	
                double dPointTime = 20;//打点延时 ms						 
                bool bPulsePointMode = false;//脉冲点模式 
                int nPulseNum = 10;//脉冲点数目
                double dFlySpeed = 200;

                LmcMultiDll.GetPenParam(nCard, nPenNo, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth, ref nStartTC,
                    ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp, ref dAccDist, ref dPointTime,
                    ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);


                try
                {
                    nMarkLoop = int.Parse( textBoxMarkTimes.Text );
                    dMarkSpeed = double.Parse( textBoxMarkSpeed.Text );
                    nFreq = int.Parse( textBoxMarkFreq.Text );
                    dPowerRatio = double.Parse( textBoxMarkPowerPulse.Text);

                    if (m_nLaserType == 0)
                    {
                        dPowerRatio = double.Parse( textBoxMarkPowerPulse.Text );
                    }
                    else
                    {
                        dQPulseWidth =  double.Parse( textBoxMarkPowerPulse.Text);
                    }

                    nStartTC = int.Parse( textBoxMarkOpenDelay.Text );
                    nLaserOffTC = int.Parse( textBoxMarkCloseDelay.Text);
                    nEndTC = int.Parse( textBoxMarkEndDelay.Text );
                    nPolyTC = int.Parse( textBoxMarkPolyDelay.Text);

                    LmcMultiDll.SetPenParam(nCard, nPenNo, nMarkLoop, dMarkSpeed,
                      dPowerRatio, dCurrent, nFreq, dQPulseWidth,
                      nStartTC, nLaserOffTC, nEndTC, nPolyTC,
                      dJumpSpeed, nJumpPosTC, nJumpDistTC, dEndComp,
                      dAccDist, dPointTime, bPulsePointMode, nPulseNum,
                      dFlySpeed);


                }
                catch (Exception)
                {
                }

            }

            LmcMultiDll.SaveEntLibToFile(nCard, EzdFiletextBox.Text);
        }

        private void buttonManuel_Click(object sender, EventArgs e)
        {
            MyCommanLib.CommonDefine.ReadMemeryFile(MyCommanLib.CommonDefine.m_posMemeryFile, ref MyCommanLib.CommonDefine.m_PosStruct);
            string strInfo = string.Format("E:{0}R:{1}X1:{2}Y1:{3}A1:{4}", MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd, MyCommanLib.CommonDefine.m_PosStruct.m_nResult,
                                         MyCommanLib.CommonDefine.m_PosStruct.fx, MyCommanLib.CommonDefine.m_PosStruct.fy, MyCommanLib.CommonDefine.m_PosStruct.fr);
            MessageBox.Show(strInfo);

            DialogResult nRet = MessageBox.Show("是否打标", "提示", MessageBoxButtons.YesNo);
            if (nRet != DialogResult.Yes)
            {
                return;
            }

            int nCard = m_nCardAddress;

            if (MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd == 1)
            {
                if (MyCommanLib.CommonDefine.m_PosStruct.m_nResult != 0)
                {
                    markForm.TransMarkPostion(nCard);

                    string strText = textBoxSample.Text;
                    //MarkEntity(strText);
                    markForm.Mark(nCard, strText);
                }
                else
                {
                    string strText = textBoxSample.Text;
                    //MarkEntity(strText);
                    markForm.Mark(nCard, strText);
                }

            }
            else
            {
                string strText = textBoxSample.Text;
                //MarkEntity(strText);
                markForm.Mark(nCard, strText);
            }
        }

        private void redcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            m_bShowRedLight = redcheckbox.Checked;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            string strMark = textBoxSnFixed.Text + textBoxSnCode.Text;
            textBoxSample.Text = strMark;

            CommonDefine.waferFileInfo.AllLaserMarkContent[m_nLaserType].snCode = textBoxSnCode.Text;
            markForm.ReplaceByName(m_nCardAddress, strMark);

            UI2WaferInfo(m_nLaserType);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        public void comboBoxPenNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nPenNo = comboBoxPenNO.SelectedIndex;
            if( nPenNo >= 0 )
            {
                int nCard = m_nCardAddress;

                int nMarkLoop = 1;//加工次数
                double dMarkSpeed = 100;//标刻次数mm/s
                double dPowerRatio = 50;//功率百分比(0-100%)	
                double dCurrent = 0.5;//电流A
                int nFreq = 500;//频率HZ
                double dQPulseWidth = 10;//Q脉冲宽度us	
                int nStartTC = 200;//开始延时us
                int nLaserOffTC = 200;//激光关闭延时us 
                int nEndTC = 200;//结束延时us
                int nPolyTC = 200;//拐角延时us   //	
                double dJumpSpeed = 200; //跳转速度mm/s
                int nJumpPosTC = 200; //跳转位置延时us
                int nJumpDistTC = 200;//跳转距离延时us	
                double dEndComp = 20;//末点补偿mm
                double dAccDist = 20;//加速距离mm	
                double dPointTime = 20;//打点延时 ms						 
                bool bPulsePointMode = false;//脉冲点模式 
                int nPulseNum = 10;//脉冲点数目
                double dFlySpeed = 200;

                LmcMultiDll.GetPenParam(nCard, nPenNo, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth, ref nStartTC,
                    ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp, ref dAccDist, ref dPointTime,
                    ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);


                try
                {
                    textBoxMarkTimes.Text = nMarkLoop.ToString();
                    textBoxMarkSpeed.Text = dMarkSpeed.ToString();
                    textBoxMarkFreq.Text = nFreq.ToString();
                    textBoxMarkPowerPulse.Text = dQPulseWidth.ToString();

                    if (m_nLaserType == 0)
                    {
                        //textBoxMarkPowerPulse.Text = dPowerRatio.ToString();
                    }
                    else
                    {
                        textBoxMarkPowerPulse.Text = dQPulseWidth.ToString();
                    }

                    textBoxMarkOpenDelay.Text = nStartTC.ToString();
                    textBoxMarkCloseDelay.Text = nLaserOffTC.ToString();
                    textBoxMarkEndDelay.Text = nEndTC.ToString();
                    textBoxMarkPolyDelay.Text = nPolyTC.ToString();

                }
                catch (Exception)
                {
                }
                
            }
        }

        internal void ShowPreviewBmp( int nCard)
        {
            int w = pictureBox1.Size.Width;
            int h = pictureBox1.Size.Height;
            if (w > h)
            {
                w = h;
            }
            else
            {
                h = w;
            }
            pictureBox1.Image = LmcMultiDll.GetCurPreviewImage(nCard, w, h);
        }

        public void WritePrivateIni(int nLaserType )
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezdMark.ini";
            string strText = this.EzdFiletextBox.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", $"EzdFileName{nLaserType}", strText, fileName);
                       
            strText = this.textBoxSample.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", $"BoxSample{nLaserType}", strText, fileName);

            strText = this.textBoxSnFixed.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", $"BoxSnFixed{nLaserType}", strText, fileName);

            strText = this.textBoxSnCode.Text;
            CommonDefine.WritePrivateProfileString("EzdIni", $"BoxSnCode{nLaserType}", strText, fileName);

            m_nEncryptionType = comboBoxEncriptType.SelectedIndex;
            strText = m_nEncryptionType.ToString();
            CommonDefine.WritePrivateProfileString("EzdIni", $"nEncryptionType{nLaserType}", strText, fileName);

        }

        public void GetPrivateIni(int nLaserType )
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\ezdMark.ini";

            StringBuilder temp = new StringBuilder(255);

            CommonDefine.GetPrivateProfileString("EzdIni", $"EzdFileName{nLaserType}", "c:\\1.ezd", temp, 255, fileName);
            this.EzdFiletextBox.Text = temp.ToString();

            m_MoudFilePath = System.IO.Path.GetDirectoryName(EzdFiletextBox.Text);

            CommonDefine.GetPrivateProfileString("EzdIni", $"BoxSample{nLaserType}", "JR49BG00001", temp, 255, fileName);
            //this.textBoxSample.Text = temp.ToString();

            CommonDefine.GetPrivateProfileString("EzdIni", $"BoxSnFixed{nLaserType}", "JR49BG", temp, 255, fileName);
            this.textBoxSnFixed.Text = temp.ToString();

            CommonDefine.GetPrivateProfileString("EzdIni", $"BoxSnCode{nLaserType}", "00001", temp, 255, fileName);
            this.textBoxSnCode.Text = temp.ToString();

            CommonDefine.GetPrivateProfileString("EzdIni", $"nEncryptionType{nLaserType}", "0", temp, 255, fileName);
            string strText = temp.ToString();

            try
            {
                if (strText.Length > 0)
                {
                    m_nEncryptionType = int.Parse(strText);
                }
            }
            catch (Exception)
            {

            }
            comboBoxEncriptType.SelectedIndex = m_nEncryptionType;
         
        }

        internal int  GetSnLength()
        {
            int nLen = textBoxSnCode.Text.Length;

            m_nSNLength = nLen;
            return m_nSNLength;
        }

        internal int GetVerifyCodeType()
        {
            int nType = comboBoxEncriptType.SelectedIndex;

            if (nType < 0)
            {
                nType = 0;
                comboBoxEncriptType.SelectedIndex = 0;
            }

            m_nEncryptionType = nType;

            return nType;
        }

        internal void OperateButtonState(bool bEnable)
        {
            buttonOpenEzd.Enabled = bEnable;
            buttonLaserParam.Enabled = bEnable;
            buttonManuel.Enabled = bEnable;
            buttonSave.Enabled = bEnable;
            buttonApply.Enabled = bEnable;

            comboBoxEncriptType.Enabled = bEnable;
            textBoxSample.Enabled = bEnable;
            textBoxSnCode.Enabled = bEnable;
            textBoxSnFixed.Enabled = bEnable;

            
            textBoxMarkTimes.Enabled = bEnable;
            textBoxMarkSpeed.Enabled = bEnable;
            textBoxMarkFreq.Enabled = bEnable;
            textBoxMarkPowerPulse.Enabled = bEnable;

            textBoxMarkOpenDelay.Enabled = bEnable;
            textBoxMarkCloseDelay.Enabled = bEnable;
            textBoxMarkEndDelay.Enabled = bEnable;
            textBoxMarkPolyDelay.Enabled = bEnable;

            checkBoxCCDCheck1.Enabled = bEnable;

            tb_markAngle.Enabled = bEnable;
            tb_checkAngle.Enabled = bEnable;
            comboBox1.Enabled = bEnable;
            tb_PRF.Enabled = bEnable;
            tb_MCS.Enabled = bEnable;
            tb_MaxPowerValue.Enabled = bEnable;
            tb_MinPowerValue.Enabled = bEnable;
        }

        /// <summary>
        /// 将 CommonDefine 类中的打标信息显示到页面
        /// </summary>
        /// <param name="nLaserType">料盒index</param>
        internal void waferInfo2UI(int nLaserType )
        {
            //code define
            EzdFiletextBox.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].strEzdName;
            textBoxSample.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].sampleCode;
            textBoxSnFixed.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].fixedCode;
            textBoxSnCode.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].snCode;

            comboBoxEncriptType.SelectedIndex = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nEncriptType;

            markForm.loadEzdFile(EzdFiletextBox.Text, m_nCardAddress );

            comboBoxPenNO.SelectedIndex = 0;

            checkBoxCCDCheck1.Checked = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].bCCDCheck;

            textBoxVPosOffset.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].fVPosOffset;

            tb_markAngle.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nMarkAngle.ToString();

            tb_checkAngle.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nCheckAngle.ToString();

            comboBox1.SelectedIndex = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nWaferType;

            tb_PRF.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nPRF.ToString();

            tb_MCS.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nMCS.ToString();

            tb_MinPowerValue.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].dPowerMin.ToString();
            tb_MaxPowerValue.Text = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].dPowerMax.ToString();
        }

        /// <summary>
        /// 将晶圆打标信息保存到 CommonDefine 类
        /// </summary>
        /// <param name="nLaserType">料盒index</param>
        internal void UI2WaferInfo(int nLaserType)
        {
            //code define
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].strEzdName = EzdFiletextBox.Text;
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].sampleCode = textBoxSample.Text;
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].fixedCode = textBoxSnFixed.Text;
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].snCode = textBoxSnCode.Text;
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nEncriptType = comboBoxEncriptType.SelectedIndex;

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].bCCDCheck = checkBoxCCDCheck1.Checked;

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].fVPosOffset = textBoxVPosOffset.Text;

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nMarkAngle = int.Parse(tb_markAngle.Text);

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nCheckAngle = int.Parse(tb_checkAngle.Text);

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nWaferType = comboBox1.SelectedIndex;

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nPRF = int.Parse(tb_PRF.Text);

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nMCS = int.Parse(tb_MCS.Text);

            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].dPowerMin = double.Parse(tb_MinPowerValue.Text);
            CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].dPowerMax = double.Parse(tb_MaxPowerValue.Text);
        }

        private void checkBoxCCDCheck1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public string GetParam()
        {
            string str = string.Format("频率：{0}，脉宽：{1}，打标角度：{2}，检测角度：{3}",
                textBoxMarkFreq.Text, textBoxMarkPowerPulse.Text, tb_markAngle.Text, tb_checkAngle.Text
                );
            return str;
        }
    }
}
