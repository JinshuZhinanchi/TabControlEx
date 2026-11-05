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
    public partial class CutIOOutListViewForm : Form
    {
        delegate void SetStateCallback(Control control, Color clr);
        delegate void RefreshListViewCallback(ListView control, OpcOperateForm opcOperate, int nStartIndex);

        DataTable m_DataTable = new DataTable();

        public int opcItemIndex = 0;
        OpcOperateForm parentOpcOperate;

        public CutIOOutListViewForm()
        {
            InitializeComponent();

            InitialOpcButton();

            {
                string fileName = Environment.CurrentDirectory + "\\CutIOOut.csv";

                MyCommanLib.CommonDefine.OpenCSVFile(ref m_DataTable, fileName);
                int nCount = m_DataTable.Rows.Count - 1;
                int nPageCount = nCount / 4;
                int nLast = nPageCount % 4;

                int nStart = 1;
                int nPageCount1 = nPageCount;
                if (nLast > 0)
                {
                    nPageCount1 = nPageCount + 1;
                }

                SetTableToDgv(m_DataTable, ref listViewIO1, nStart, nPageCount1);

                nStart = nStart + nPageCount1;
                nPageCount1 = nPageCount;
                if (nLast > 1)
                {
                    nPageCount1 = nPageCount + 1;
                }
                SetTableToDgv(m_DataTable, ref listViewIO2, nStart, nPageCount1);

                nStart = nStart + nPageCount1;
                nPageCount1 = nPageCount;
                if (nLast > 2)
                {
                    nPageCount1 = nPageCount + 1;
                }
                SetTableToDgv(m_DataTable, ref listViewIO3, nStart, nPageCount1);

                nStart = nStart + nPageCount1;
                nPageCount1 = nPageCount;
                if (nLast > 2)
                {
                    nPageCount1 = nPageCount + 1;
                }
                SetTableToDgv(m_DataTable, ref listViewIO4, nStart, nPageCount1);
            }
        }

        public static void SetTableToDgv(DataTable dt, ref ListView listView, int nStart, int nCount)
        {
            if (dt.Columns.Count < 3)
                return;

            int ndtColums = dt.Columns.Count;


            //foreach (DataRow row in dt.Rows)//逐个读取单元格的内容；
            for (int i = 1; i < dt.Rows.Count; i++)//忽略dt第一行
            {
                DataRow row = dt.Rows[i];
                if (i >= nStart && i < nStart + nCount)
                {
                    ListViewItem lvItem = new ListViewItem();

                    lvItem.UseItemStyleForSubItems = false;
                    for (int j = 0; j < ndtColums; j++)
                    {

                        if (j == 0)
                        {
                            //lvItem.ForeColor = textColor;
                            lvItem.Text = row[j].ToString();
                            lvItem.StateImageIndex = 0;
                        }
                        else
                        {
                            lvItem.SubItems.Add(row[j].ToString());

                        }
                    }
                    listView.Items.Add(lvItem);
                }
            }
        }

        public void InitialOpcButton()
        {
            opcItemIndex = PLCOpcFeedParameters.ExtIOOutState.Y00;
        }

        private void RefreshListViewThread(ListView listView, OpcOperateForm opcOperate, int nStartIndex)
        {
            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            if (this.IsDisposed)
            {
                return;
            }
            if (listView.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                RefreshListViewCallback d = new RefreshListViewCallback(RefreshListViewThread);
                this.Invoke(d, new object[] { listView, opcOperate, nStartIndex });
            }
            else
            {
                RefreshListViewIoState(listView, opcOperate, nStartIndex);
            }

        }

        public void RefreshThreadIoState(OpcOperateForm opcOperate)
        {
            parentOpcOperate = opcOperate;

            int nStartIndex = opcItemIndex;
            RefreshListViewThread(listViewIO1, opcOperate, nStartIndex);

            nStartIndex = opcItemIndex + listViewIO1.Items.Count;
            RefreshListViewThread(listViewIO2, opcOperate, nStartIndex);

            nStartIndex = opcItemIndex + listViewIO1.Items.Count + listViewIO2.Items.Count;
            RefreshListViewThread(listViewIO3, opcOperate, nStartIndex);

            nStartIndex = opcItemIndex + listViewIO1.Items.Count + listViewIO2.Items.Count + listViewIO3.Items.Count;
            RefreshListViewThread(listViewIO4, opcOperate, nStartIndex);
        }

        public void RefreshListViewIoState(ListView listView, OpcOperateForm opcOperate, int nStartIndex)
        {
            int nIoIndex = 0;
            int nItemsCount1 = listViewIO1.Items.Count;
            int nSubItemsCount = 0;

            for (int i = 0; i < nItemsCount1; i++)
            {
                nIoIndex = nStartIndex + i;
                ListViewItem lvItem = listView.Items[i];
                nSubItemsCount = lvItem.SubItems.Count;
                for (int j = 0; j < nSubItemsCount; j++)
                {
                    if ((j + 1) == 3)
                    {
                        Object ob = opcOperate.GetItemValue(nIoIndex);
                        if (ob == null)
                            return;

                        Boolean bValue = (Boolean)ob;
                        string strText = lvItem.SubItems[j].Text;
                        if (bValue && strText != "1")
                        {
                            lvItem.SubItems[j].Text = "1";
                            lvItem.SubItems[j].ForeColor = Color.Red;
                            lvItem.SubItems[j].BackColor = Color.Green;
                        }

                        if (!bValue && strText != "0")
                        {
                            lvItem.SubItems[j].Text = "0";
                            lvItem.SubItems[j].ForeColor = Color.Black;
                            lvItem.SubItems[j].BackColor = Color.White;
                        }
                    }
                }
            }
        }


        public void RefreshIoState(OpcOperateForm opcOperate)
        {
            int nIoCount = opcItemIndex;
            int nIoIndex = 0;
            int nItemsCount1 = listViewIO1.Items.Count;
            int nSubItemsCount = 0;

            ListView listView = listViewIO1;
            int nStartIndex = opcItemIndex;
            for (int i = 0; i < nItemsCount1; i++)
            {
                nIoIndex = nStartIndex + i;
                ListViewItem lvItem = listView.Items[i];
                nSubItemsCount = lvItem.SubItems.Count;
                for (int j = 0; j < nSubItemsCount; j++)
                {
                    if ((j + 1) == 3)
                    {
                        Object ob = opcOperate.GetItemValue(nIoIndex);
                        if (ob == null)
                            return;

                        Boolean bValue = (Boolean)ob;
                        string strText = lvItem.SubItems[j].Text;
                        if (bValue && strText != "1")
                        {
                            lvItem.SubItems[j].Text = "1";
                            lvItem.SubItems[j].ForeColor = Color.Red;
                            lvItem.SubItems[j].BackColor = Color.Green;
                        }

                        if (!bValue && strText != "0")
                        {
                            lvItem.SubItems[j].Text = "0";
                            lvItem.SubItems[j].ForeColor = Color.Black;
                            lvItem.SubItems[j].BackColor = Color.White;
                        }
                    }
                }
            }
            //listview2
            listView = listViewIO2;
            int nItemsCount2 = listView.Items.Count;
            nStartIndex = opcItemIndex + nItemsCount1;
            for (int i = 0; i < nItemsCount2; i++)
            {
                nIoIndex = nStartIndex + i;
                ListViewItem lvItem = listView.Items[i];
                nSubItemsCount = lvItem.SubItems.Count;
                for (int j = 0; j < nSubItemsCount; j++)
                {
                    if ((j + 1) == 3)
                    {
                        Object ob = opcOperate.GetItemValue(nIoIndex);
                        if (ob == null)
                            return;

                        Boolean bValue = (Boolean)ob;
                        string strText = lvItem.SubItems[j].Text;
                        if (bValue && strText != "1")
                        {
                            lvItem.SubItems[j].Text = "1";
                            lvItem.SubItems[j].ForeColor = Color.Red;
                            lvItem.SubItems[j].BackColor = Color.Green;
                        }

                        if (!bValue && strText != "0")
                        {
                            lvItem.SubItems[j].Text = "0";
                            lvItem.SubItems[j].ForeColor = Color.Black;
                            lvItem.SubItems[j].BackColor = Color.White;
                        }
                    }
                }
            }
            //listview3
            listView = listViewIO3;
            int nItemsCount3 = listView.Items.Count;
            nStartIndex = opcItemIndex + nItemsCount1 + nItemsCount2;
            for (int i = 0; i < nItemsCount3; i++)
            {
                nIoIndex = nStartIndex + i;
                ListViewItem lvItem = listView.Items[i];
                nSubItemsCount = lvItem.SubItems.Count;
                for (int j = 0; j < nSubItemsCount; j++)
                {
                    if ((j + 1) == 3)
                    {
                        Object ob = opcOperate.GetItemValue(nIoIndex);
                        if (ob == null)
                            return;

                        Boolean bValue = (Boolean)ob;
                        string strText = lvItem.SubItems[j].Text;
                        if (bValue && strText != "1")
                        {
                            lvItem.SubItems[j].Text = "1";
                            lvItem.SubItems[j].ForeColor = Color.Red;
                            lvItem.SubItems[j].BackColor = Color.Green;
                        }

                        if (!bValue && strText != "0")
                        {
                            lvItem.SubItems[j].Text = "0";
                            lvItem.SubItems[j].ForeColor = Color.Black;
                            lvItem.SubItems[j].BackColor = Color.White;
                        }
                    }
                }
            }

            //listview 4            
            listView = listViewIO4;
            int nItemsCount4 = listView.Items.Count;
            nStartIndex = opcItemIndex + nItemsCount1 + nItemsCount2;
            for (int i = 0; i < nItemsCount4; i++)
            {
                nIoIndex = nStartIndex + i;
                ListViewItem lvItem = listView.Items[i];
                nSubItemsCount = lvItem.SubItems.Count;
                for (int j = 0; j < nSubItemsCount; j++)
                {
                    if ((j + 1) == 3)
                    {
                        Object ob = opcOperate.GetItemValue(nIoIndex);
                        if (ob == null)
                            return;

                        Boolean bValue = (Boolean)ob;
                        string strText = lvItem.SubItems[j].Text;
                        if (bValue && strText != "1")
                        {
                            lvItem.SubItems[j].Text = "1";
                            lvItem.SubItems[j].ForeColor = Color.Red;
                            lvItem.SubItems[j].BackColor = Color.Green;
                        }

                        if (!bValue && strText != "0")
                        {
                            lvItem.SubItems[j].Text = "0";
                            lvItem.SubItems[j].ForeColor = Color.Black;
                            lvItem.SubItems[j].BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void CutIOOutListViewForm_SizeChanged(object sender, EventArgs e)
        {
            int width = listViewIO1.Width;

            int fixWidth = (listViewIO1.Columns[1].Width + listViewIO1.Columns[2].Width);

            int dWidth = (width - fixWidth) - 5;
            listViewIO1.Columns[0].Width = dWidth;

            listViewIO2.Columns[0].Width = dWidth;

            listViewIO3.Columns[0].Width = dWidth;

            listViewIO4.Columns[0].Width = dWidth;
        }

        private void listViewIO1_DoubleClick(object sender, EventArgs e)
        {
            if (parentOpcOperate == null)
                return;

            if ( listViewIO1.SelectedItems.Count > 0 )
            {
                int row = listViewIO1.SelectedItems[0].Index;

                int nItem = opcItemIndex + row;

                object ob = parentOpcOperate.GetItemValue(nItem);
                Boolean bValue = false;
                try
                {
                    bValue = (Boolean)ob;
                }
                catch (Exception)
                {
                }

                parentOpcOperate.WriteItemValue(nItem, !bValue);

            }
        }

        private void listViewIO2_DoubleClick(object sender, EventArgs e)
        {
            if (parentOpcOperate == null)
                return;

            if (listViewIO2.SelectedItems.Count > 0)
            {
                int row = listViewIO2.SelectedItems[0].Index;

                int nItem = listViewIO1.Items.Count + opcItemIndex + row;

                object ob = parentOpcOperate.GetItemValue(nItem);
                Boolean bValue = false;
                try
                {
                    bValue = (Boolean)ob;
                }
                catch (Exception)
                {
                }

                parentOpcOperate.WriteItemValue(nItem, !bValue);

            }
        }

        private void listViewIO3_DoubleClick(object sender, EventArgs e)
        {
            if (parentOpcOperate == null)
                return;

            if (listViewIO3.SelectedItems.Count > 0)
            {
                int row = listViewIO3.SelectedItems[0].Index;

                int nItem = listViewIO1.Items.Count + listViewIO2.Items.Count + opcItemIndex + row;

                object ob = parentOpcOperate.GetItemValue(nItem);
                Boolean bValue = false;
                try
                {
                    bValue = (Boolean)ob;
                }
                catch (Exception)
                {
                }

                parentOpcOperate.WriteItemValue(nItem, !bValue);

            }
        }

        private void listViewIO4_DoubleClick(object sender, EventArgs e)
        {
            if (parentOpcOperate == null)
                return;

            if (listViewIO4.SelectedItems.Count > 0)
            {
                int row = listViewIO4.SelectedItems[0].Index;

                int nItem = listViewIO1.Items.Count + listViewIO2.Items.Count + listViewIO3.Items.Count + opcItemIndex + row;

                object ob = parentOpcOperate.GetItemValue(nItem);
                Boolean bValue = false;
                try
                {
                    bValue = (Boolean)ob;
                }
                catch (Exception)
                {
                }

                parentOpcOperate.WriteItemValue(nItem, !bValue);

            }
        }

        private void listViewIO1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
