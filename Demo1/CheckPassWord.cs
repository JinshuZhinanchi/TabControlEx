using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EzdDemo
{
    public partial class CheckPassWord : Form
    {
        public string m_strPassWord;
        public CheckPassWord( string password )
        {
            InitializeComponent();

            m_strPassWord = password;
        }

        private void CheckOK_Click(object sender, EventArgs e)
        {
            string newPass = textBox1.Text.Trim();
            string oldPass = m_strPassWord.Trim();
            if (newPass.Equals(oldPass))
            {

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("密码不正确，请重新输入");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ModifyPasswordDlg dlg = new ModifyPasswordDlg(m_strPassWord);
            if (DialogResult.OK == dlg.ShowDialog())
            {
                m_strPassWord = dlg.m_strPassWord;
                
            }       
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

    }
}
