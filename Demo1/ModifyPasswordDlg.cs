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
    public partial class ModifyPasswordDlg : Form
    {
        public string m_strPassWord;
        public ModifyPasswordDlg(string password)
        {
            InitializeComponent();
            m_strPassWord = password;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            string oldPass1 = m_strPassWord.Trim();
            string oldPass2 = textOldPass.Text.Trim();
            string newPass = textNewPass.Text.Trim();

            if (oldPass1.Equals(oldPass2) )
            {
                if( newPass.Length > 0 )
                {
                    m_strPassWord = newPass;
                    this.DialogResult = DialogResult.OK;
                
                    MessageBox.Show("密码修改成功,马上登陆后生效");
                }else
                {
                    MessageBox.Show("新的密码不能为空");
                }
            }
            else
            {            
                MessageBox.Show("原始密码错误，请重新输入");
               
            }
        }
    }
}
