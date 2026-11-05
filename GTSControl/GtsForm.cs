using EzdDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using gts;

namespace jog
{
    public partial class GtsForm : Form
    {
        public AxisTestForm rotateAxisForm;
        public AxisTestForm strechAxisForm;
        public AxisTestForm upDownAxisForm;
        public AxisTestForm markZAxisForm;

        public GtsControl m_gtsControl;

        SettingForm settingForm;

        public MarkForm markForm;
        public GtsForm()
        {            
            InitializeComponent();
            
            m_gtsControl = new GtsControl();
            m_gtsControl.m_gtsForm = this;

            markZAxisForm = new AxisTestForm();
            markZAxisForm.m_nCurrentAxis = AxisTestForm.UPDN_AXIS;
            markZAxisForm.m_gtsControl = m_gtsControl;

            markZAxisForm.TopLevel = false;
            markZAxisForm.Dock = System.Windows.Forms.DockStyle.Fill;
            markZAxisForm.FormBorderStyle = FormBorderStyle.None;
            panelAxis1.Controls.Add(markZAxisForm);
            markZAxisForm.Show();

            InitialCard();
        }

        public void StartWork()
        {
          
        }

        public void EndWork()
        {
         
        }

        public void RefreshUIState()
        {
           
        }


        public void Stop()
        {
            m_gtsControl.Stop();            
            //m_gtsControl.ResetWork();
        }

        public void Start()
        {
            m_gtsControl.Start();
        }

        public static Boolean showErrorMessage(string command, short error)
        {
            Boolean bRet = false;
            if (error != 0)
            {
                bRet = true;

                string strMessage = string.Format("{0}={1}", command, error);
                MessageBox.Show(strMessage);
            }
            return bRet;
        }

        public Boolean InitialCard()
        {
            Boolean bRet = false;
            short nRet = 0;
            try
            {
                nRet = gts.mc.GT_Open(0, 1);//开卡
            }
            catch (Exception ee)
            {
            }

            if (showErrorMessage("板卡错误", nRet))
            {
                return bRet;
            }

            nRet = gts.mc.GT_Reset();//复位
            if (showErrorMessage("板卡初始化失败", nRet))
            {
                return bRet;
            }


            nRet = gts.mc.GT_LoadConfig("GTS800.cfg");//加载配置文件
            if (showErrorMessage("板卡cfg配置文件调用失败", nRet))
            {
                return bRet;
            }

            nRet = gts.mc.GT_OpenExtMdl("gts.dll");
            if (showErrorMessage("扩展IO模块调用失败", nRet))
            {
                return bRet;
            }

            nRet = gts.mc.GT_LoadExtConfig("IOIN32.cfg");
            if (showErrorMessage("IO模块cfg配置文件调用失败", nRet))
            {
                return bRet;
            }

            nRet = gts.mc.GT_ResetExtMdl();
            if (showErrorMessage("IO模块初始化失败", nRet))
            {
                return bRet;
            }
            bRet = true;
            return bRet;
        }

        private void GtsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rotateAxisForm.WriteGtsSettingIni();
            upDownAxisForm.WriteGtsSettingIni();
            strechAxisForm.WriteGtsSettingIni();
            markZAxisForm.WriteGtsSettingIni();
            m_gtsControl.Close();
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            if( comboBoxSize.SelectedIndex < 0 )
            {
                MessageBox.Show("没有选择尺寸，请选择尺寸！");
                return;
            }

            settingForm.StartWork();
            settingForm.m_nBoxSizeType = comboBoxSize.SelectedIndex;
            
            settingForm.ShowDialog();
            settingForm.EndWork();
            settingForm.WriteGtsSettingParam();
        }

        private void buttonMark_Click(object sender, EventArgs e)
        {
            //markForm.ShowDialog();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            m_gtsControl.initialChipMarkData();
            
            rotateAxisForm.SaveDevicePara();
            upDownAxisForm.SaveDevicePara();
            strechAxisForm.SaveDevicePara();
            markZAxisForm.SaveDevicePara();
        }
    }
}
