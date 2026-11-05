using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCommanLib
{
    public partial class LoginForm : Form
    {
        public int m_nUserLevel = CommonDefine.USER_OP;

        public LoginForm()
        {
            InitializeComponent();

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }

        public LoginForm(int nUserLevel)
        {
            InitializeComponent();

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            m_nUserLevel = nUserLevel;

            comboBoxUser.SelectedIndex = nUserLevel;
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            UserControl1 userForm = new UserControl1();
            string pw = GetPassword();
            int nType = GetUserType();
            if( nType < m_nUserLevel )
            {
                MessageBox.Show("权限不足，需要更高权限");
                return;
            }

            Boolean bRet = userForm.CheckPassword(pw, nType);
           
            if(bRet )
            {
                MessageBox.Show("登录成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误，请重新输入！");
            }
        }

        public int GetUserType()
        {
            int nValue = 0;
            nValue = comboBoxUser.SelectedIndex;
            return nValue;
        }

        public string GetPassword()
        {
            string pw = "";
            pw = textBoxUser.Text;
            return pw;
        }
    }
}
