using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace MyCommanLib
{
    public partial class UserControl1: Form
    {
        public int m_nStartTick = 0;
        Thread delayThread;
        System.Timers.Timer delayTimer;

        public int m_nCurrentLevel = CommonDefine.USER_ADMIN;

        public event EventHandler SwitchTabPageHandler;

        public string m_strOperater = "123";
        public string m_strTechnician = "1234";
        public string m_strEngineer = "12345";
        public string m_strAdmin = "123456";

        public UserControl1()
        {
            InitializeComponent();
            ReadCuttingIni();

            WriteCuttingIni();
        }

        public void StartWork()
        {
            delayThread = new Thread(new ThreadStart(delayWork));
            delayThread.Start();
            m_nStartTick = Environment.TickCount;
        }

        public void EndWork()
        {
            if (delayThread == null)
                return;
            delayTimer.Enabled = false;
            delayThread.Abort();
        }

        private void delayWork()
        {
            delayTimer = new System.Timers.Timer();
            delayTimer.Interval = 5000;
            delayTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_delayWork);
            delayTimer.Enabled = true;
        }

        void timer_delayWork(object sender, System.Timers.ElapsedEventArgs e)
        {
            SwitchTabPageHandler(m_nStartTick, e);

        }

        public Boolean CheckPassword(string pw, int nType)
        {
            Boolean bRet = false;
            if (nType == MyCommanLib.CommonDefine.USER_OP)
            {
                if (pw == m_strOperater)
                {
                    bRet = true;
                }
            }
            else if (nType == MyCommanLib.CommonDefine.USER_TP)
            {
                if (pw == m_strTechnician)
                {
                    bRet = true;
                }
            }
            else if (nType == MyCommanLib.CommonDefine.USER_EP)
            {
                if (pw == m_strEngineer)
                {
                    bRet = true;
                }
            }
            else if (nType == MyCommanLib.CommonDefine.USER_ADMIN)
            {
                if (pw == m_strAdmin)
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        public int GetCurrentOp(string strOP)
        {
            int nOp = MyCommanLib.CommonDefine.USER_OP;
            if (strOP == m_strOperater)
            {
                nOp = MyCommanLib.CommonDefine.USER_OP;
            }
            else if (strOP == m_strTechnician)
            {
                nOp = MyCommanLib.CommonDefine.USER_TP;
            }
            else if (strOP == m_strEngineer)
            {
                nOp = MyCommanLib.CommonDefine.USER_EP;
            }
            else if (strOP == m_strAdmin)
            {
                nOp = MyCommanLib.CommonDefine.USER_ADMIN;
            }

            return nOp;
        }
        public void WriteCuttingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\User.ini";

            string strText = m_strOperater;
            string keyName = "m_strOperater";

            CommonDefine.WritePrivateProfileString("UserInfo", keyName, strText, fileName);

            keyName = "m_strTechnician";
            strText = m_strTechnician;
            CommonDefine.WritePrivateProfileString("UserInfo", keyName, strText, fileName);


            strText = m_strEngineer;
            keyName = "m_strEngineer";
            CommonDefine.WritePrivateProfileString("UserInfo", keyName, strText, fileName);

            strText = m_strAdmin;
            keyName = "m_strAdmin";
            CommonDefine.WritePrivateProfileString("UserInfo", keyName, strText, fileName);

        }

        public void ReadCuttingIni()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\User.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            string strText = "";
            string keyName = "m_strOperater";

            StringBuilder temp = new StringBuilder(255);

            keyName = "m_strOperater";
            CommonDefine.GetPrivateProfileString("UserInfo", keyName, strText, temp, 255, fileName);
            m_strOperater = temp.ToString();

            keyName = "m_strTechnician";
            CommonDefine.GetPrivateProfileString("UserInfo", keyName, strText, temp, 255, fileName);
            m_strTechnician = temp.ToString();

            keyName = "m_strEngineer";
            CommonDefine.GetPrivateProfileString("UserInfo", keyName, strText, temp, 255, fileName);
            m_strEngineer = temp.ToString();

            keyName = "m_strAdmin";
            CommonDefine.GetPrivateProfileString("UserInfo", keyName, strText, temp, 255, fileName);
            m_strAdmin = temp.ToString();

            textBoxOperater.Text = m_strOperater;
            textBoxTechnician.Text = m_strTechnician;
            textBoxEngineer.Text = m_strEngineer;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;

            m_strOperater = textBoxOperater.Text;
            m_strTechnician = textBoxTechnician.Text;
            m_strEngineer = textBoxEngineer.Text;
            m_strAdmin = textBoxAdmin.Text;

            WriteCuttingIni();
        }

        private void textBoxOperater_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        private void textBoxTechnician_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        private void textBoxEngineer_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }

        private void textBoxAdmin_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }
    }
}
