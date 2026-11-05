using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyCommanLib;

namespace TabControlExDemo
{
    public partial class SettingForm : BaseForm
    {        
        public string m_strCuttingPath = "";
        public string m_strCuttingPath2 = "";
        public string m_strXmlPath = "";
        public string m_strDelay = "300000";
        public int maxDelay = 10000;

        public SettingForm()
        {
            InitializeComponent();
            m_nCurrentLevel = CommonDefine.USER_ADMIN;
        }
                

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;

            m_strDelay = textBoxDelay.Text;
            SetDelay(m_strDelay);
        }

        private void buttonWorkOrder_Click(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SetXmlPath(dialog.SelectedPath);                
            }
        }

        private void buttonCuttingPath_Click(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SetCuttingPath(dialog.SelectedPath);
            }
        }

        private void buttonCuttingPath2_Click(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SetCuttingPath2(dialog.SelectedPath);
            }
        }

        public void SetCuttingPath(string strText)
        {
            //m_strCuttingPath = strText;
            //textBoxCuttingPath.Text = strText;
            //mainForm.autoForm.SetCuttingPath(strText);
        }

        public void SetCuttingPath2(string strText)
        {
            //m_strCuttingPath2 = strText;
            //textBoxCuttingPath2.Text = strText;
            //mainForm.autoForm.SetCuttingPath2(strText);
        }

        public void SetXmlPath(string strText)
        {
            //m_strXmlPath = strText;
            //textBoxWorkOrder.Text = strText;
            //mainForm.autoForm.SetXmlPath(strText);

        }

        public void SetDelay(string strText)
        {
            //m_strDelay = strText;
            //textBoxDelay.Text = strText;

            //try
            //{
            //    maxDelay = (int.Parse(m_strDelay)) * 1000;
            //}
            //catch
            //{
            //    maxDelay = 60000;
            //    m_strDelay = maxDelay.ToString();
            //}

            //mainForm.autoForm.setDelay(m_strDelay);
            //mainForm.WriteOpcDelay(maxDelay);
        }

        public string GetCuttingPath()
        {
            return textBoxCuttingPath.Text;
        }

        public string GetCuttingPath2()
        {
            return textBoxCuttingPath2.Text;
        }

        public string GetXmlPath()
        {
            return textBoxWorkOrder.Text;
        }

        public string GetDelay()
        {
            return m_strDelay;
        }

        private void textBoxDelay_TextChanged(object sender, EventArgs e)
        {
            m_nStartTick = Environment.TickCount;
        }
    }
}
