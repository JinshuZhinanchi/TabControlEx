using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class ScanForm : Form
    {

        public int m_nBoxIndex = 0;
        public string m_strFeedCastleID = "";
        public string m_strCutCastleID = "";
        public string m_strSmifPodID = "";

        public ScanForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if( textBoxFeedCastleID.Text.Length <= 0 )
            {
                MessageBox.Show("上料CastleID内容为空，请重新扫码！");
                return;
            }

            if (textBoxCutCastleID.Text.Length <= 0)
            {
                MessageBox.Show("下料CastleID内容为空，请重新扫码！");
                return;
            }

            if (textBoxSmifPodID.Text.Length <= 0)
            {
                MessageBox.Show("下料SmifPodID内容为空，请重新扫码！");
                return;
            }

            m_strFeedCastleID = textBoxFeedCastleID.Text;
            m_strCutCastleID = textBoxCutCastleID.Text;
            m_strSmifPodID = textBoxSmifPodID.Text;

            m_nBoxIndex = comboBoxBoxIndex.SelectedIndex;

            this.DialogResult = DialogResult.OK;


            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
