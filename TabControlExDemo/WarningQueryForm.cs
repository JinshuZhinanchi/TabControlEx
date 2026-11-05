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
    public partial class WarningQueryForm : Form
    {
        public AutoForm parentForm;

        public  int m_nPageIndex = 0;
        private int m_nCountOfPage = 10;
        private int m_nPageCount = 0;
        private int m_nTotalCount = 0;
        private DateTime m_dateTime1;
        private DateTime m_dateTime2;


        public WarningQueryForm()
        {
            InitializeComponent();
            if(m_nPageCount == 0)
            {
                labelPageInfo.Text = "页面：0/0";

                pulseButtonPre.Enabled = false;
                pulseButtonNext.Enabled = false;
            }
        }

        private void ReFreshCurrentPage()
        {
            int nStart = m_nPageIndex * m_nCountOfPage;

            int nShowCount = Math.Min((m_nTotalCount - nStart),m_nCountOfPage);


            DataTable dt = parentForm.m_productWarningDbOp.GetDataByTime(m_dateTime1, m_dateTime2, nStart, nShowCount);

            this.listViewWarns.Items.Clear();

            int length = dt.Rows.Count;

            for (int i = 0; i < length; i++)
            {
                ListViewItem lvi = new ListViewItem(dt.Rows[i][0].ToString());  //ListView的第一个Item作为主项需要单独添加

                //后面添加的Item都为SubItems ，即为子项
                DeviceWarnings wDt = parentForm.m_dviceWarningDbOP.GetbyName(dt.Rows[i][1].ToString());

                lvi.SubItems.Add(dt.Rows[i][2].ToString());

                lvi.SubItems.Add(wDt.Count.ToString());

                listViewWarns.Items.Add(lvi);//最后进行添加
            }

            if (m_nPageCount == 0)
            {
                labelPageInfo.Text = "页面：0/0";

                pulseButtonPre.Enabled = false;
                pulseButtonNext.Enabled = false;
            }
            else
            {
                labelPageInfo.Text = string.Format("页面：{0}/{1}", m_nPageIndex+1, m_nPageCount);

                if( m_nPageIndex == 0 )
                {
                    pulseButtonPre.Enabled = false;
                }
                else
                {
                    pulseButtonPre.Enabled = true;
                }

                if( m_nPageIndex == m_nPageCount-1)
                {
                    pulseButtonNext.Enabled = false;
                }
                else
                {
                    pulseButtonNext.Enabled = true;
                }
                
            }
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            string wName = textBoxName.Text;
           
            m_dateTime1 = dateTimePickerStart.Value;
            m_dateTime2 = dateTimePickerEnd.Value;

            int nCount = parentForm.m_productWarningDbOp.GetCountByTime(m_dateTime1, m_dateTime2);
            m_nTotalCount = nCount;

            m_nPageCount = nCount / m_nCountOfPage;
            if( nCount%m_nCountOfPage != 0 )
            {
                m_nPageCount++;
            }           

            m_nPageIndex = 0;

            ReFreshCurrentPage();            

        }

        private void WarningQueryForm_SizeChanged(object sender, EventArgs e)
        {
            int width = listViewWarns.Width;
            listViewWarns.Columns[2].Width = 30;
            listViewWarns.Columns[1].Width = width - listViewWarns.Columns[0].Width - listViewWarns.Columns[2].Width;
            
        }

        private void pulseButtonPre_Click(object sender, EventArgs e)
        {
            if (m_nPageIndex > 0)            
            {              
                m_nPageIndex--;
                ReFreshCurrentPage();

            }
        }

        private void pulseButtonNext_Click(object sender, EventArgs e)
        {
            if (m_nPageIndex < m_nPageCount -1 )
            {               
                m_nPageIndex++;
                ReFreshCurrentPage();

            }
        }

        private void buttonEmpty_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("你确定要清空吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if ( dr == DialogResult.Yes )
            {
                parentForm.m_productWarningDbOp.ClearAll();
                parentForm.m_dviceWarningDbOP.ClearCount();
                m_nPageIndex= 0;
                m_nPageCount = 0;
                ReFreshCurrentPage();
            }
        }
    }
}
