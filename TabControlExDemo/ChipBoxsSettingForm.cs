using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EzdDemo;
using jog;
using MyCommanLib;

namespace TabControlExDemo
{
    public partial class ChipBoxsSettingForm : Form
    {
        public MarkForm markForm;
        public ChipAutoForm chipAutoForm;

        CheckBox[,] boxCheckBox = new CheckBox[8, 25];

        Label[,] boxMarkContent = new Label[8, 25];


        string m_strMarkFileName = "D:\\";
        int m_nBoxIndex = 0;

        public event EventHandler RefreshAllChipsUIHandler;
        public string m_strBatch = "Batch";
        private int m_nStartSN = 1;

        Boolean m_bUseManual = false;

        Boolean m_bAddEvent = false;

        public ChipBoxsSettingForm()
        {
            InitializeComponent();
         
            int nBoxIndex = 0;
            int nChipIndex = 0;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_1;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_1;
            nChipIndex++;            
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_2;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_2;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_3;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_3;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_4;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_4;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_5;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_5;
            nChipIndex++;//5
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_6;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_6;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_7;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_7;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_8;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_8;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_9;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_9;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_10;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_10;
            nChipIndex++;//10
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_11;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_11;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_12;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_12;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_13;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_13;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_14;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_14;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_15;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_15;
            nChipIndex++;//15
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_16;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_16;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_17;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_17;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_18;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_18;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_19;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_19;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_20;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_20;
            nChipIndex++;//20
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_21;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_21;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_22;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_22;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_23;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_23;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_24;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_24;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox1_25;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_25;

            nBoxIndex = 1;
            nChipIndex = 0;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_1;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_1;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_2;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_2;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_3;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_3;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_4;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_4;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_5;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_5;
            nChipIndex++;//5
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_6;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_6;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_7;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_7;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_8;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_8;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_9;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_9;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_10;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_10;
            nChipIndex++;//10
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_11;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_11;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_12;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_12;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_13;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_13;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_14;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_14;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_15;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_15;
            nChipIndex++;//15
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_16;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_16;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_17;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_17;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_18;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_18;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_19;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_19;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_20;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_20;
            nChipIndex++;//20
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_21;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_21;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_22;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_22;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_23;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_23;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_24;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_24;
            nChipIndex++;
            boxCheckBox[nBoxIndex, nChipIndex] = checkBox2_25;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_25;

            comboBoxShow.SelectedIndex = 0;
        }

        public void AddEventHandler()
        {
            if (m_bAddEvent)
                return;
            m_bAddEvent = true;

            markForm.SetProducInfoHandler += mark_SetProducInfoHandler;

            markForm.SetManualOperateHandler += mark_SetManualOperateHandler;

            markForm.SetChipInfoHandler += mark_SetChipInfoHandler;

        }
     

        private void mark_SetManualOperateHandler(object sender, EventArgs e)
        {
            Boolean bEnable = (Boolean)sender;

            buttonPhoto.Enabled = bEnable;
            buttonMark.Enabled = bEnable;
            buttonFace.Enabled = bEnable;
            buttonBack.Enabled = bEnable;
            buttonClear.Enabled = bEnable;
            buttonOrig.Enabled = bEnable;

        }

        private void mark_SetProducInfoHandler(object sender, EventArgs e)
        {
            string strValue = sender.ToString();
            int nProducIndex = 0;
            try
            {
                if( strValue.Length > 0 )
            {
                nProducIndex = int.Parse(strValue);
            }
            }
            catch (Exception)
            {

            }
            chipAutoForm.SwitchRobotBank(nProducIndex);
            
        }

        public void ReadBoxMarkData(  string fileName , int nBoxIndex )
        {            
            if (File.Exists(fileName) == false)
            {
                return;
            }

            DataTable dataTable = new DataTable();
            MyCommanLib.CommonDefine.OpenCSVFile(ref dataTable, fileName);

            StringBuilder temp = new StringBuilder(255);

            int nWarningCount = dataTable.Rows.Count;
            int nColsCount = dataTable.Columns.Count;
            int nMarkCol = 3;
            if(  nMarkCol >= nColsCount  )
            {
                return;
            }

            int nLaserType = comboBoxShow.SelectedIndex;

            for(int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++ )
            {
                CommonDefine.waferFileInfo.SetMarkContent(nLaserType, nBoxIndex, i, "");

                boxMarkContent[nBoxIndex, i].Text = "";
                boxCheckBox[nBoxIndex, i].Checked = false;
            }

            string strMark = "";
            for (int i = 1; i < nWarningCount && i <= AxisTestForm.BOX_CHIPSCOUNT ; i++)
            {
                DataRow row = dataTable.Rows[i];
                string strValue = row[nMarkCol].ToString();
                strMark = strValue;
                if ( strValue.Length > 0 )
                {
                    
                    string srcString = strMark;
                    if (chipAutoForm.CheckMarkContentLength(nLaserType, srcString) == false)
                    {
                        MessageBox.Show($"生成信息与样本信息长度不一致，出错行{i},请检查后设置");
                        return;
                    }

                    if (CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nEncriptType == 1)
                    {
                        string strNew = CommonDefine.CreateM7Code(strMark);
                        strMark = strMark + strNew;
                    }
                    CommonDefine.waferFileInfo.SetMarkContent(nLaserType,nBoxIndex, i-1, strMark);

                    boxMarkContent[nBoxIndex, i-1].Text = strMark;
                    boxCheckBox[nBoxIndex, i-1].Checked = true;

                }                
            }
        }

        public void OpenBoxCsvFile(int nBoxIndex)
        {
            m_nBoxIndex = nBoxIndex;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(m_strMarkFileName);
            dlg.Filter = "Ezd files (*.csv)|*.csv";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_strMarkFileName = dlg.FileName;
                ReadBoxMarkData(m_strMarkFileName, m_nBoxIndex);

            }
        }

        private void buttonLoad1_Click(object sender, EventArgs e)
        {
            buttonLoad1.Text = "";
            OpenBoxCsvFile(0);
            buttonLoad1.Text = System.IO.Path.GetFileName(m_strMarkFileName); 
        }

        private void buttonLoad2_Click(object sender, EventArgs e)
        {
            buttonLoad2.Text = "";
            OpenBoxCsvFile(1);
            buttonLoad2.Text = System.IO.Path.GetFileName(m_strMarkFileName);
        }

        public void SelectAllChips(int nBoxIndex, Boolean bSelect)
        {
            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {
                boxCheckBox[nBoxIndex, i].Checked = bSelect;
            }
        }

        private void checkBox1All_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllChips(0, checkBox1All.Checked);
        }

        private void checkBox2All_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllChips(1, checkBox2All.Checked);
        }

        private void buttonMoudleEdit_Click(object sender, EventArgs e)
        {
            markForm.StartWork();
            markForm.ShowDialog();
            markForm.EndWork();
                        
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if( chipAutoForm != null )
            {
                chipAutoForm.AddListViewThread(null, "料仓配置取消退出", Color.Blue);
            }
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < AxisTestForm.BOX_COUNT; i++ )
            {
                for(int j = 0; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
                {
                    chipAutoForm.m_MarkChipsData[i, j].bSelect = boxCheckBox[i, j].Checked ;
                    chipAutoForm.m_MarkChipsData[i, j].strMarkContent = boxMarkContent[i, j].Text;                   
                }
            }

            Point pp = new Point();
            RefreshAllChipsUIHandler(pp,e);

            if( markForm != null )
            {
                markForm.WritePrivateIni();
            }

            if (chipAutoForm != null)
            {
                chipAutoForm.AddListViewThread(null, "料仓配置确认退出", Color.Blue);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public Boolean IsUseManual()
        {            
            return m_bUseManual;
        }

        public string  GetBatchText()
        {            
            return m_strBatch;            
        }

        public int GetStartSN()
        {
            return m_nStartSN;
        }

        private void mark_SetChipInfoHandler(object sender, EventArgs e)
        {
            string strValue = sender.ToString();

            SetChipInfoText(strValue);

        }

        public void SetChipInfoText(string strText)
        {
            textBoxBatch1.Text = strText;
        }
        public void SetEAPChipsText(int nBoxIndex, string mapContent, string[] markContent)
        {
            string strValue = "";
            int nLaserType = 0;

            for (int i = 0; i < mapContent.Length; i++)
            {
                if(mapContent[i].ToString() == "1" && markContent[i].Length > 0)
                {
                    strValue = markContent[i];
                    chipAutoForm.m_MarkChipsData[nBoxIndex, i].bSelect = true;
                    chipAutoForm.m_MarkChipsData[nBoxIndex, i].strMarkContent = strValue;

                    CommonDefine.waferFileInfo.SetMarkContent(nLaserType, nBoxIndex, i, strValue);
                }
                else
                {
                    chipAutoForm.m_MarkChipsData[nBoxIndex, i].bSelect = false;
                    chipAutoForm.m_MarkChipsData[nBoxIndex, i].strMarkContent = "";
                }
            }
        }

        public void SetChipsText(int nLaserType )
        {
            int nChipIndex = 0;
            int nStartIndex = 1;
            string strText = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].fixedCode;
            string snText = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].snCode;
            int nLen = snText.Length;

            string srcString = strText + snText;
            if (chipAutoForm.CheckMarkContentLength(nLaserType, srcString) == false)
            {
                MessageBox.Show("生成信息与样本信息长度不一致，请检查后设置");
                return;
            }

            string strStartSn = textBoxNo1.Text;
            if( nLaserType == 1)
            {
                strStartSn = textBoxNo2.Text;
            }
            try
            {               
                if (strStartSn.Length > 0)
                {
                    nStartIndex = int.Parse(strStartSn);
                }                      
            }
            catch
            {
            }
            finally
            {
                string strW = string.Format("D{0}", nLen);
                string formatString = "{0}{1:" + strW + "}";
                string strMark = string.Format(formatString, strText, nStartIndex);
            
                for (int i = 0; i < AxisTestForm.BOX_COUNT; i++)
                {                    
                    for (int j = nChipIndex; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
                    {                        
                        if (boxCheckBox[i, j].Checked)
                        {
                            if (nLen > 0)
                            {
                                nStartIndex = j + 1;
                                strMark = string.Format(formatString, strText, nStartIndex);
                            }
                            else
                            {
                                strMark = strText;
                            }

                            if (CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nEncriptType == 1)
                            {
                                string strNew = CommonDefine.CreateM7Code(strMark);
                                strMark = strMark + strNew;
                            }
                                                        
                            CommonDefine.waferFileInfo.SetMarkContent(nLaserType,i,j,strMark);

                            nStartIndex++;
                        }                        
                    }
                }            

            }
        }

        public void ClearChipsText(int nLaserType)
        {
            int nBoxCount = AxisTestForm.BOX_COUNT;
            int nChipsCount = AxisTestForm.BOX_CHIPSCOUNT;              

            for (int i = 0; i < nBoxCount; i++)
            {
                for (int j = 0; j < nChipsCount; j++)
                {
                    CommonDefine.waferFileInfo.SetMarkContent(nLaserType, i, j, "");
                }
            }

        }

        private void buttonMoudleCreate1_Click(object sender, EventArgs e)
        {
            int nIndex = 0;
            comboBoxShow.SelectedIndex = nIndex;                       

            CommonDefine.waferFileInfo.AllLaserMarkContent[nIndex].fixedCode = textBoxBatch1.Text;            

            SetChipsText(nIndex);
            reflashBoxMarkContent(nIndex);
        }

        private void buttonMoudleClear1_Click(object sender, EventArgs e)
        {
            int nIndex = 0;
            comboBoxShow.SelectedIndex = nIndex;

            ClearChipsText(nIndex);
            reflashBoxMarkContent(nIndex);
        }

        private void ChipBoxsSettingForm_Load(object sender, EventArgs e)
        {           
            markForm.TopLevel = false;
            markForm.Dock = DockStyle.Fill;
            panelMark.Controls.Add(markForm);
            markForm.Show();
        }

        private void buttonPhoto_Click(object sender, EventArgs e)
        {

            chipAutoForm.TakePhoto(AxisTestForm.CCD_SIDE_POSITION);
            //int bMarkBack = chipAutoForm.mainForm.markForm.m_bMarkBack;
            //if (bMarkBack > 0)
            //{
            //    chipAutoForm.mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_PHOTO));
            //}
            //else
            //{
            //    chipAutoForm.mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_PHOTO_FACE));
            //}

        }

        private void buttonMark_Click(object sender, EventArgs e)
        {
            chipAutoForm.TakePhoto(AxisTestForm.CCD_FACE_POSITION);

            //chipAutoForm.mainForm.robotSerialPortForm.EmptyReceiveBuffer();
            //int bMarkBack = chipAutoForm.mainForm.markForm.m_bMarkBack;
            //int nMarkPos = chipAutoForm.mainForm.markForm.GetMarkPos();
            //if (bMarkBack > 0)
            //{
            //    chipAutoForm.mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_MARK + nMarkPos));
            //}
            //else
            //{
            //    chipAutoForm.mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_MARK_FACE + nMarkPos));
            //}            
        }

        private void buttonFace_Click(object sender, EventArgs e)
        {
            chipAutoForm.TakePhoto(AxisTestForm.CCD_SIDE_CHECK);

            //chipAutoForm.mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetICommString(JelRobotOperate.F_MOVETO_FACE));
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            //chipAutoForm.TakePhoto(AxisTestForm.CCD_FACE_CHECK);
            //string info = chipAutoForm.TriggerPhoto();

            //string info = OperateSmifHelper.TriggerPhoto(chipAutoForm.mainForm.cameraForm);
            //chipAutoForm.AddListViewThread(null, info, Color.Blue);

            Task tk = new Task(new Action(TriggerPhoto));
            tk.Start();
            
            //chipAutoForm.mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetICommString(JelRobotOperate.F_MOVETO_BACK));
        }

        public void TriggerPhoto()
        {
            string info = OperateSmifHelper.TriggerPhoto(chipAutoForm.mainForm.cameraForm);
            string[] arr = info.Trim().Split(',');
            chipAutoForm.AddListViewThread(null, "OCR：" + arr[arr.Length - 1], Color.Blue);
        }

        private void buttonOrig_Click(object sender, EventArgs e)
        {
            chipAutoForm.TakePhoto(AxisTestForm.CCD_WAFER_NOTCH);
            //chipAutoForm.mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetICommString(JelRobotOperate.F_MOVETO_ORIG));
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            chipAutoForm.TakePhoto(AxisTestForm.CCD_WAFER_POSITION);
            //chipAutoForm.mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetClearString());
        }

        private void textBoxBatch1_TextChanged(object sender, EventArgs e)
        {
            m_strBatch = textBoxBatch1.Text;
        }

        private void textBoxNo1_TextChanged(object sender, EventArgs e)
        {         
            try
            {
                if (textBoxNo1.Text.Length > 0)
                {
                    m_nStartSN = int.Parse(textBoxNo1.Text);
                }else
                {
                    m_nStartSN = 1;
                }

            }
            catch
            {
                m_nStartSN = 1;
            }
            if( m_nStartSN <= 0)
            {
                m_nStartSN = 1;
            }
        }
             

        private void buttonSettingParam_Click(object sender, EventArgs e)
        {

            // chipAutoForm.mainForm.waferInfo2OPC();

            //chipAutoForm.mainForm.parametersForm.InitialOpcButton(null);

            //chipAutoForm.mainForm.parametersForm.StartWork();

            //chipAutoForm.mainForm.parametersForm.ShowDialog();

            //chipAutoForm.mainForm.OPC2WaferInfo();

            //chipAutoForm.mainForm.parametersForm.EndWork();

            chipAutoForm.mainForm.parametersForm = new GtsParametersFeedForm(chipAutoForm.mainForm.gtsForm.m_gtsControl);
            chipAutoForm.mainForm.parametersForm.ShowDialog();
        }

        private void comboBoxShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nLaserIndex = comboBoxShow.SelectedIndex;
            if (nLaserIndex < 0)
                return;

            reflashBoxMarkContent(nLaserIndex);
        }

        public void reflashBoxMarkContent( int nLaserIndex )
        {
            string strMark = "";

            for (int i = 0; i < AxisTestForm.BOX_COUNT; i++)
            {
                for (int j = 0; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
                {
                    if (boxCheckBox[i, j].Checked)
                    {
                        strMark = CommonDefine.waferFileInfo.GetMarkContent(nLaserIndex, i, j);
                        boxMarkContent[i, j].Text = strMark;
                    }
                }
            }
        }

        private void buttonMoudleCreate2_Click(object sender, EventArgs e)
        {
            int nIndex = 1;
            comboBoxShow.SelectedIndex = nIndex;
            CommonDefine.waferFileInfo.AllLaserMarkContent[nIndex].fixedCode = textBoxBatch2.Text;

            SetChipsText(nIndex);
            reflashBoxMarkContent(nIndex);
        }               

        private void buttonMoudleClear2_Click(object sender, EventArgs e)
        {
            int nIndex = 1;
            comboBoxShow.SelectedIndex = nIndex;

            ClearChipsText(nIndex);
            reflashBoxMarkContent(nIndex);
        }

        private void buttonSettingParam_Click_1(object sender, EventArgs e)
        {
            //chipAutoForm.mainForm.waferInfo2OPC();

            //chipAutoForm.mainForm.parametersForm = new ParametersFeedForm();

            //chipAutoForm.mainForm.parametersForm.InitialOpcButton(null);

            //chipAutoForm.mainForm.parametersForm.StartWork();

            //chipAutoForm.mainForm.parametersForm.ShowDialog();

            //chipAutoForm.mainForm.OPC2WaferInfo();

            //chipAutoForm.mainForm.parametersForm.EndWork();

            chipAutoForm.mainForm.parametersForm = new GtsParametersFeedForm(chipAutoForm.mainForm.gtsForm.m_gtsControl);
            chipAutoForm.mainForm.parametersForm.ShowDialog();
        }

        private void ChipBoxsSettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(chipAutoForm!= null )
            {
                chipAutoForm.AddListViewThread(null, "关闭料仓配置", Color.Blue);
            }
        }
    }
}
