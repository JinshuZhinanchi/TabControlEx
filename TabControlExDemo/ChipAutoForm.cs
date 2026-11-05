using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using jog;
using MyCommanLib;
using DemoCom;
using System.IO;
using System.Threading.Tasks;
using UserManager;

namespace TabControlExDemo
{
    public partial class ChipAutoForm : BaseForm
    {        

        delegate void AddListViewCallback(string strTime, string strContent, Color textColor);

        delegate void SetTextCallback(Control control, Color clr, string text);
        delegate void SetStateCallback(Control control, Boolean bRunning);

        delegate void WorkCountAddCallback();

        Label[,] boxMarkContent = new Label[8, 25];

        public ChipBoxsSettingForm m_chipSettingForm = new ChipBoxsSettingForm();
        //流程控制
        public GtsWorkStation[] m_gtsWorkStation = new GtsWorkStation[2];
        public MarkChipsStruct[,] m_MarkChipsData = new MarkChipsStruct[8, 25];
        public short m_nWorkState;
        public short m_nPreWorkState;

        public short m_nRobotFlow = 0;//扫描  -- 取料 -- 放到平台 -- 取料 -- 放到smif... 
        public short m_nMarkFlow = 0;//准备接片-- 寻边 -- 寻边定位 -- 打标 -- 准备接片
        private short m_nSmifFlow = 0;//准备接片-- 寻边 -- 寻边定位 -- 打标 -- 准备接片

        int m_nMarkLayerIndex = -1;
        int m_nRArmLayerIndex = -1;
        int m_nLArmLayerIndex = -1;

        private int m_nWorkBoxIndex = 0;

        public ChipBoxStruct m_WorkChipList = new ChipBoxStruct();
        public ChipBoxStruct m_WorkChipListTemp = new ChipBoxStruct();
        public ChipBoxList m_WorkReadyBoxList = new ChipBoxList();
        public int[] m_WorkChipState = new int[25];       //ui 0: 无  1：有  2：完成
        public int m_nWorkLayerIndex = 0;//ui
        public int m_nWorkBoxSize = 0;
        private int m_nWorkTestMode =0;
        private Boolean m_bPatchMark = false;

        private int m_nTakePhotoNG = 0;
        private int m_nCheckOCRNG = 0;

        private const int m_RobotCommdSleep = 15000;
        public static WarningForm m_warnForm;

        public int m_nStartLightTick = 0;

        DateTime m_StartTime = new DateTime();
        int m_nOneChipTime = 10000;
        int m_nFinishCount = 0;

        Boolean m_bTestFlow = false;

        public MarkStruct m_PosStructZ = new MarkStruct();
        public MarkStruct m_PosStructF = new MarkStruct();

        public string[] m_strMarkContentArray = new string[25];

        public Boolean m_bEAPLockMarkContent = false;
        public Boolean m_bEAPLockLoadFoup = false;

        public ChipAutoForm()
        {
            InitializeComponent();
            
            m_chipSettingForm.RefreshAllChipsUIHandler += RefreshAllChipsUI;

            int nBoxIndex = 0;
            int nChipIndex = 0;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_1;
            nChipIndex++;
            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_2;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_3;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_4;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_5;
            nChipIndex++;//5

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_6;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_7;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_8;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_9;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_10;
            nChipIndex++;//10

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_11;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_12;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_13;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_14;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_15;
            nChipIndex++;//15

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_16;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_17;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_18;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_19;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_20;
            nChipIndex++;//20

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_21;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_22;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_23;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_24;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox1_25;

            nBoxIndex = 1;
            nChipIndex = 0;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_1;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_2;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_3;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_4;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_5;
            nChipIndex++;//5

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_6;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_7;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_8;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_9;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_10;
            nChipIndex++;//10

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_11;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_12;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_13;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_14;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_15;
            nChipIndex++;//15

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_16;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_17;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_18;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_19;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_20;
            nChipIndex++;//20

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_21;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_22;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_23;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_24;
            nChipIndex++;

            boxMarkContent[nBoxIndex, nChipIndex] = labelBox2_25;

            ReadCuttingIniValue();

            initialChipMarkData();

        }
              

        //0--4   尺寸bank  4  ，  6， 8 ，12   // A
        public void SwitchRobotBank(int nProductID )
        {
            int nBank = nProductID;
            if (nProductID < 0)
                return;
            if (mainForm == null)
                return;
            if (mainForm.robotSerialPortForm == null)
                return;

            if ( mainForm.robotSerialPortForm.serialPort.IsOpen )
            {
                //MessageBox.Show(JelRobotOperate.GetSwitchRobotBankString(nBank));
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetSwitchRobotBankString(nBank));
            }            

        }

        public void WriteCuttingIniValue()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";
                        
            //string keyName = "ProductIndex";
            //string strText = m_nProductIndex.ToString();            
            //Program.WritePrivateProfileString("CuttingIni", keyName, strText, fileName);

        }

        public string ReadCuttingIniValue()
        {
            string strText = "0";
            //string fileName = System.Windows.Forms.Application.StartupPath + "\\Cutting.ini";
           
            //StringBuilder temp = new StringBuilder(255);
            //string keyName = "ProductIndex";
            //Program.GetPrivateProfileString("CuttingIni", keyName, strText, temp, 255, fileName);
            //strText = temp.ToString();

            //int nSelect = 0;
            //try
            //{
            //    nSelect = int.Parse(strText);
            //}catch
            //{

            //}
          
            //SwitchRobotBank(nSelect);
            return strText;
        }

        public void AddListViewThread(string strTime, string strContent, Color textColor)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.listViewWorkLogs.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                AddListViewCallback d = new AddListViewCallback(AddListViewThread); 
                this.Invoke(d, new object[] { strTime, strContent, textColor });
            }
            else
            {
                AddContent2ListView(strTime, strContent, textColor);
            }
        }

        public void AddContent2ListView(string strTime, string strContent, Color textColor)
        {
            try
            {
                if (strTime == null)
                {
                    strTime = string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                int nCount = listViewWorkLogs.Items.Count;
                if (nCount > 600)
                {
                    for (int i = nCount - 1; i > 100; i--)
                    {
                        listViewWorkLogs.Items.RemoveAt(i);
                    }
                }
            }
            catch
            {
                int nCount = listViewWorkLogs.Items.Count;
                if (nCount > 600)
                {
                    listViewWorkLogs.Clear();
                }
            }


            ListViewItem lvItem = new ListViewItem();

            lvItem.ForeColor = textColor;
            lvItem.Text = strTime;
            lvItem.StateImageIndex = 0;

            lvItem.SubItems.Add(strContent);

            this.listViewWorkLogs.Items.Insert(0, lvItem);

            MyCommanLib.CommonDefine.SaveWorkLogs(strContent);
        }

        public void initialMarkContentArray()
        {
            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {                
                m_strMarkContentArray[i] = "";
            }
        }

        internal void initialChipMarkData()
        {
            m_gtsWorkStation[0] = new GtsWorkStation();
            m_gtsWorkStation[1] = new GtsWorkStation();
            for (int i = 0; i < AxisTestForm.BOX_COUNT; i++)
            {
                for (int j = 0; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
                {
                    m_MarkChipsData[i, j] = new MarkChipsStruct();
                }
            }

            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {
                m_WorkChipState[i] = AxisTestForm.BOXCHIP_NOTSCANSTATE;
                m_strMarkContentArray[i] = "";
            }
        }

        internal void ResetChipMarkData(int nBoxIndex )
        { 
            for (int j = 0; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
            {
                m_MarkChipsData[nBoxIndex, j].bSelect = false;
                m_MarkChipsData[nBoxIndex, j].strMarkContent = "";
            }            
        }

        internal void initialData()
        {
            m_nRobotFlow = 0;//扫描  -- 取料 -- 放到平台 -- 取料 -- 放到smif... 
            m_nMarkFlow = 0;//准备接片-- 寻边 -- 寻边定位 -- 打标 -- 准备接片
            m_nSmifFlow = 0;//准备接片-- 寻边 -- 寻边定位 -- 打标 -- 准备接片

            m_nMarkLayerIndex = -1;
            m_nRArmLayerIndex = -1;
            m_nLArmLayerIndex = -1;

            m_WorkChipList.m_LayerList.Clear();
            m_WorkReadyBoxList.m_BoxList.Clear();
    }

        public void ResetChipState()
        {
            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {
                m_WorkChipState[i] = AxisTestForm.BOXCHIP_NOTSCANSTATE;
            }
        }

        public Boolean CheckScanData()
        {
            int nCount = m_WorkChipList.m_LayerList.Count;
            int nBoxIndex = m_WorkChipList.m_nBoxIndex;

            Boolean bFind = false;
            Boolean bRet = true;

            for(int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++ )
            {
                bFind = true;

                if ( m_MarkChipsData[nBoxIndex, i].bSelect )
                {
                    bFind = false;
                    for(int j = 0; j < nCount; j++ )
                    {
                        int iLayer = m_WorkChipList.m_LayerList[j].X;
                        if( iLayer == i )
                        {
                            bFind = true;
                            break;
                        }
                    }
                    
                }

                if (bFind == false)
                {
                    bRet = false;
                    break;
                }
            }

            return bRet;
        }

        internal void CreateChipMarkData()
        {           
            int nCount = m_WorkChipList.m_LayerList.Count;
            int nBoxIndex = m_WorkChipList.m_nBoxIndex;
            if (nCount <= 0)
                return;
            if (m_chipSettingForm.markForm == null)
                return;
                    
            int nLen = m_chipSettingForm.markForm.GetSnLength(m_chipSettingForm.markForm.m_nAddress0Card);

            string strText = m_chipSettingForm.GetBatchText();
            string strW = string.Format("D{0}", nLen);
            string formatString = "{0}{1:" + strW + "}";
            string strMark = "";

            ResetChipMarkData(nBoxIndex);

            int nStartIndex = m_chipSettingForm.GetStartSN();                       
            int nLaserType = 0;

            for (int i = 0; i < nCount; i++ )
            {
                int iLayer = m_WorkChipList.m_LayerList[i].X;
                m_MarkChipsData[nBoxIndex, iLayer].bSelect = true;

                strMark = CommonDefine.waferFileInfo.GetMarkContent(nLaserType, nBoxIndex, iLayer);
                
                m_MarkChipsData[nBoxIndex, iLayer].strMarkContent = strMark;
                nStartIndex++;
                //MessageBox.Show(strMark);

            }            
        }

        void SetWorkSate(int nState)
        {
            m_nWorkState = (short)nState;
        }

        public void InitialOpcForm()
        {
            m_warnForm = new WarningForm(mainForm);
        }
        
        
        private void SetThreadText(Control control, Color clr, string text)
        {
            if (control.IsDisposed)
            {
                return;
            }

            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            //if (ob is TextBox)
            {
                if (control.InvokeRequired)
                {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                    try
                    {
                        SetTextCallback d = new SetTextCallback(SetThreadText);
                        this.Invoke(d, new object[] { control, clr, text });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    if (clr.ToArgb() > 0)
                    {
                        control.ForeColor = clr;
                    }
                    control.Text = text;
                }
            }
        }

        private void SetThreadBkColorAndText(Control control, Color clr, string text)
        {
            if (control.IsDisposed)
            {
                return;
            }

            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            //if (ob is TextBox)
            {
                if (control.InvokeRequired)
                {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                    try
                    {
                        SetTextCallback d = new SetTextCallback(SetThreadBkColorAndText);
                        this.Invoke(d, new object[] { control, clr, text });
                    }
                    catch
                    {

                    }

                }
                else
                {
                    if (clr != null)
                    {
                        control.BackColor = clr;
                    }
                    if( text != null)
                    {
                        control.Text = text;
                    }
                    
                }
            }
        }
        
        private void WorkCountAdd()
        {
            if (labelWorkCount.IsDisposed)
            {
                return;
            }
                
            if (labelWorkCount.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                try
                {
                    WorkCountAddCallback d = new WorkCountAddCallback(WorkCountAdd);
                    this.Invoke(d, new object[] { });
                }
                catch{ }
            }
            else
            {
                string strText = labelWorkCount.Text;
                if( strText.Length <= 0  )
                {
                    strText = "0";
                }
                int nCount = 0;
                try
                {
                    nCount = int.Parse(strText);                        
                }catch { }
                nCount++;

                labelWorkCount.Text = nCount.ToString();
                mainForm.WriteCuttingIniValue("WorkCount",labelWorkCount.Text);
            }
            
        }
                
        internal void RefreshUI()
        {
            string strInfo = "";
            Color bkColor = Color.Black;

            int nBoxIndex = m_nWorkBoxIndex;
            int nLayerIndex = m_nWorkLayerIndex;
            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {
                int nType = m_WorkChipState[i];

                nLayerIndex = i;

                if (nType == AxisTestForm.BOXCHIP_WAITSTATE)
                {
                    strInfo = m_MarkChipsData[nBoxIndex, i].strMarkContent;
                    bkColor = Color.Green;                    
                }
                else if (nType == AxisTestForm.BOXCHIP_RUNSTATE || nType == AxisTestForm.BOXCHIP_RUNNINGSTATE)
                {                                        
                    strInfo = null;
                    bkColor = Color.Yellow;
                }
                else if (nType == AxisTestForm.BOXCHIP_FINISHSTATE)
                {
                    strInfo = null;
                    bkColor = Color.Blue;
                    
                }
                else if (nType == AxisTestForm.BOXCHIP_FAILSTATE)
                {
                    strInfo = null;
                    bkColor = Color.Red;
                }
                else
                {
                    strInfo = null;
                    bkColor = Color.Black;                    
                }

                SetThreadBkColorAndText(boxMarkContent[nBoxIndex, i], bkColor, strInfo);
            }
            strInfo = (nBoxIndex + 1).ToString();
            bkColor = Color.Black;
            SetThreadText(labelBoxIndex, bkColor, strInfo);

            strInfo = (nLayerIndex + 1).ToString();
            bkColor = Color.Black;
            SetThreadText(labelLayerIndex, bkColor, strInfo);

        }
        
        /// <summary>
        /// 双臂机械手工作流程
        /// </summary>
        /// <returns></returns>
        public Boolean OperateWorkFlow()
        {
            Boolean bRet = true;

            if (m_nWorkState != AxisTestForm.WORK_AUTO)
                return bRet;

            int nSecsAlarmIndex = -1;

            // 伺服报警标志
            if (IsRobotEmStop())
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;                               
                return bRet;
            }

            if (IsNormalWarning())
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;             
                return bRet;
            }           

            //if (IsOPCNormalWarning())
            //{
            //    m_nWorkState = AxisTestForm.WORK_MANUAL;              
            //    return bRet;
            //}          


            SetOperateTime();

            string strInfo = "";

            if (m_nRobotFlow == AxisTestForm.EAP_SCAN_VALUE)
            {
                nSecsAlarmIndex = SECSAlarmData.RecipeSwitchFail;

                m_chipSettingForm.chipAutoForm = this;

                if (m_WorkReadyBoxList.m_BoxList.Count <= 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "扫描的料已完成1,请准备好后按开始！";
                    ShowWarningDialog(strInfo);

                    return false;
                }


                int nListIndex = FindFirstWorkIndex();
                if (nListIndex >= m_WorkReadyBoxList.m_BoxList.Count || nListIndex < 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "扫描的料已完成2,请准备好后按开始！";
                    ShowWarningDialog(strInfo);
                    return false;
                }

                //MessageBox.Show(m_WorkReadyBoxList.m_BoxList.Count.ToString() );

                int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;

                nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;


                //切换料盒后，切换recipe                
                if (mainForm.IsEAPRunMode())
                {
                    string recipeName = CommonDefine.m_SecsData.recipeNameArray[nBoxIndex];

                    try
                    {
                        if (mainForm.SwitchRecipeForEap(recipeName, ref strInfo) == false)
                        {
                            m_nWorkState = AxisTestForm.WORK_MANUAL;

                            strInfo = "recipe切换失败: "+recipeName;
                            ShowWarningDialog(strInfo, nSecsAlarmIndex);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                                        
                    int nPenNO = 0;
                    mainForm.WriteSecsMarkParamData(nPenNO);
                    strInfo = $"Recipe切换成功！打标次数：{mainForm.secsForm.m_pReceip.markParams[nPenNO].MarkTimes}, " +
                                               $"打标速度：{mainForm.secsForm.m_pReceip.markParams[nPenNO].MarkSpeed}, " +
                                               $"打标频率：{mainForm.secsForm.m_pReceip.markParams[nPenNO].MarkFreq}, " +
                                               $"打标脉宽：{mainForm.secsForm.m_pReceip.markParams[nPenNO].MarkPulse}, ";

                    AddListViewThread(null, strInfo, Color.Blue);

                    Action ac = new Action(() => {
                        mainForm.TestMarkPower(mainForm.markForm.m_nAddress0Card);
                    });

                    Task t = new Task(ac);
                    t.Start();
                    Task.WaitAll(t);
                    
                    try
                    {
                        if (CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMax < mainForm.markForm.m_fNormalPower
                        || CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMin > mainForm.markForm.m_fNormalPower)
                        {
                            m_nWorkState = AxisTestForm.WORK_MANUAL;

                            strInfo = $"功率测试值{mainForm.markForm.m_fNormalPower}超范围";
                            ShowWarningDialog(strInfo, nSecsAlarmIndex);
                            return false;

                        }
                        else
                        {
                            strInfo = $"功率测试值{mainForm.markForm.m_fNormalPower}正常";
                            AddListViewThread(null, strInfo, Color.Blue);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                                                          
                    //AddListViewThread(null, strInfo, Color.Blue);
                    //AddListViewThread(null, mainForm.markForm.markHeadForm1.GetParam(), Color.Blue);

                    try
                    {
                        m_chipSettingForm.SetEAPChipsText(nBoxIndex, CommonDefine.m_SecsData.strMappingContent,CommonDefine.m_SecsData.strMarkContent);

                        string strMap = nBoxIndex > 0 ? OperateSmifHelper.GetMappiingContent(mainForm.smifForm2) : OperateSmifHelper.GetMappiingContent(mainForm.smifForm);

                        //分割Map
                        string[] strArray = strMap.Split('/');
                        string strResult = strArray[1];
                        string strMapping = strResult.Substring(0, strResult.Length - 2);

                        string strShowInfo = "";
                        string strEapInfo = "";

                        Point tempPoint;

                        int nResult = 1;

                        if (nResult == 1)
                        {
                            string[] sArray = new string[25];
                            for (int i = 0; i < strMapping.Length; i++)
                            {
                                sArray[i] = strMapping[i].ToString();
                            }

                            strEapInfo = strShowInfo = strMapping;

                            m_nWorkBoxIndex = nBoxIndex;
                            int layerIndex = 0;

                            try
                            {
                                foreach (string str in sArray)
                                {
                                    if (layerIndex >= AxisTestForm.BOX_CHIPSCOUNT)
                                    {
                                        break;
                                    }
                                    string strText = m_MarkChipsData[nBoxIndex, layerIndex].strMarkContent;
                                    Boolean bCheck = m_MarkChipsData[nBoxIndex, layerIndex].bSelect;

                                    string sss = str;
                                    if (layerIndex == 0)
                                    {
                                        sss = str.Substring(str.Length - 1, 1);
                                    }

                                    if (sss == "1" && bCheck)
                                    {
                                        //等到扫描结果
                                        Point pp = new Point(layerIndex, AxisTestForm.BOXCHIP_WAITSTATE);
                                        m_WorkChipList.m_LayerList.Add(pp);
                                        m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_WAITSTATE;

                                    }
                                    else if (sss == "1")
                                    {
                                        m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_WAITSTATE;
                                    }
                                    else if (str == "E" || str == "W")
                                    {
                                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                                        m_WorkChipList.m_LayerList.Clear();

                                        strInfo = string.Format("返回数据有误，请检查后重新扫描！-{0}", strShowInfo);

                                        WarningForm warnForm = new WarningForm(mainForm);
                                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                                        warnForm.ShowDialog();
                                        warnForm = null;
                                        return false;
                                    }
                                    else
                                    {
                                        m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_NOTSCANSTATE;
                                    }
                                    layerIndex++;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            try
                            {
                                //刷新料盒状态
                                tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_RUNSTATE);
                                m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;
                                m_WorkReadyBoxList.m_nCurrentChipIndex = 0;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                            this.RefreshAllChipsUI(null, null);

                            if (m_WorkChipList.m_LayerList.Count <= 0)
                            {

                                m_nWorkState = AxisTestForm.WORK_MANUAL;

                                strInfo = string.Format("合法的数量为空,请检查后重新扫描！-{0}", "");


                                WarningForm warnForm = new WarningForm(mainForm);
                                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                                warnForm.ShowDialog();
                                warnForm = null;
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                }
                else
                {
                    //initialMarkContentArray();
                    //m_nRobotFlow = AxisTestForm.READ_RFID;
                }
            }
            else if(m_nRobotFlow == AxisTestForm.READ_RFID)
            {
                nSecsAlarmIndex = SECSAlarmData.RFIDReadFail;

                int nListIndex = FindFirstWorkIndex();
                int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;

                if (nBoxIndex < 0 || nBoxIndex > 1)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "料盒索引错误,请准备好后按开始！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (mainForm.IsEAPRunMode())
                {
                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexOther;
                }

                string strRFID = nBoxIndex == 0 ? OperateSmifHelper.GetRFID(mainForm.smifForm) : OperateSmifHelper.GetRFID(mainForm.smifForm2);

                string[] arr = strRFID.Split('/');
                strRFID = arr[2].Substring(0, arr[2].Length - 2);
                strInfo = string.Format("RFID:{0}", strRFID);
                mainForm.chipAutoForm.AddListViewThread(null, strInfo, Color.Blue);

                if(strRFID.Length <= 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"strRFID读取失败！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (mainForm.IsEAPRunMode())
                {
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupRFIDReadContent, strRFID);
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupRFIDReadResult, "0");
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupID, nBoxIndex.ToString());
                    mainForm.secsForm.SetEventPos(SECSEventData.ReadRFIDComplete);

                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                }
                else
                {
                    m_nRobotFlow = AxisTestForm.SCAN_VALUE;
                }
                
            }
            else if (m_nRobotFlow == AxisTestForm.SCAN_VALUE)
            {
                nSecsAlarmIndex = SECSAlarmData.FoupLoadFail;

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "扫料测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                    return false;
                }

                if (m_WorkReadyBoxList.m_BoxList.Count <= 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "料盒已生产完成";
                    
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo);                 
                    warnForm.ShowDialog();

                    OperateClearLight(GLinkIOModule00.YellowLightIOOUT);

                    warnForm = null;
                    return false;
                }

                int nListIndex = FindFirstWorkIndex();

                if (nListIndex >= m_WorkReadyBoxList.m_BoxList.Count || nListIndex < 0)
                {
                    if(ckb_cycleMode.Checked == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        // if ( CloseOneSmif(m_nWorkBoxIndex) )
                        {
                            strInfo = $"扫描的料已完成--料盒索引:{m_WorkReadyBoxList.m_BoxList.Count}-料盒：{nListIndex}";
                            //mainForm.mOpcAxisOperater.OperateVacummDevice(false);

                            WarningForm warnForm = new WarningForm(mainForm);
                            warnForm.SetInfoText(strInfo, nSecsAlarmIndex, 1500, 2);
                            warnForm.ShowDialog();
                            warnForm = null;

                            OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
                           
                        }
                        return false;
                    }
                    
                }

                if (mainForm.IsEAPRunMode() == false)
                {
                    Action ac = new Action(() => {
                        mainForm.TestMarkPower(mainForm.markForm.m_nAddress0Card);
                    });

                    Task t = new Task(ac);
                    t.Start();
                    Task.WaitAll(t);

                    try
                    {
                        if (CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMax < mainForm.markForm.m_fNormalPower
                        || CommonDefine.waferFileInfo.AllLaserMarkContent[0].dPowerMin > mainForm.markForm.m_fNormalPower)
                        {
                            m_nWorkState = AxisTestForm.WORK_MANUAL;

                            strInfo = $"功率测试值{mainForm.markForm.m_fNormalPower}超范围";
                            ShowWarningDialog(strInfo, nSecsAlarmIndex);
                            return false;

                        }
                        else
                        {
                            strInfo = $"功率测试值{mainForm.markForm.m_fNormalPower}正常";
                            AddListViewThread(null, strInfo, Color.Blue);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                AddListViewThread(null, mainForm.markForm.markHeadForm1.GetParam(), Color.Blue);

                strInfo = "扫料开始！";
                AddListViewThread(null, strInfo, Color.Blue);

                int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;

                if (nBoxIndex < 0 || nBoxIndex > 1)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "料盒索引错误,请准备好后按开始！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                bool bEAP = mainForm.IsEAPRunMode();
                if (bEAP)
                {
                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;
                }

                if ( mainForm.IsEAPRunMode() == false )
                {
                    //if (CommonDefine.waferFileInfo.IsMarkContentEmpty(0, nBoxIndex))
                    //{
                    //    strInfo = $"当前料盒：{nBoxIndex}没有打标内容，请检查料仓配置！";
                    //    WarningForm warnForm = new WarningForm(mainForm);
                    //    warnForm.SetInfoText(strInfo);
                    //    warnForm.ShowDialog();
                    //    warnForm = null;
                    //    return bRet;
                    //}
                }

                ClearRobotError();

                Point tempPoint;

                if (m_WorkReadyBoxList.m_BoxList[nListIndex].Y == AxisTestForm.BOXCHIP_WAITSTATE)
                {
                    m_WorkChipList.m_LayerList.Clear();
                    m_WorkChipList.m_nBoxIndex = nBoxIndex;
                }

                Boolean bEapMode = mainForm.IsEAPRunMode();

                string strRet = nBoxIndex == 0 ? OperateSmifHelper.OpenSmif(mainForm.smifForm) : OperateSmifHelper.OpenSmif(mainForm.smifForm2);
                Thread.Sleep(500);
                Boolean bOpened = IsSmifOpened(nBoxIndex);
                if (strRet.Length <= 0 || bOpened == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"打开Smif失败--{strRet}--{bOpened}--{nBoxIndex}";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    AddListViewThread(null, strInfo, Color.Red);
                    return false;
                }

                nSecsAlarmIndex = SECSAlarmData.FoupMappingFail;

                string strMap = nBoxIndex > 0 ? OperateSmifHelper.GetMappiingContent(mainForm.smifForm2) : OperateSmifHelper.GetMappiingContent(mainForm.smifForm);

                //分割Map
                string[] strArray = strMap.Split('/');
                string strResult = strArray[1];
                string strMapping = strResult.Substring(0, strResult.Length - 2);

                string strShowInfo = "";
                string strEapInfo = "";

                int nResult = 1;
                if (bEAP)
                {
                    nResult = 0;
                }

                if (nResult == 1)
                {
                    string[] sArray = new string[25];
                    for (int i = 0; i < strMapping.Length; i++)
                    {
                        sArray[i] = strMapping[i].ToString();
                    }

                    strEapInfo = strShowInfo = strMapping;

                    m_nWorkBoxIndex = nBoxIndex;
                    int layerIndex = 0;

                    try
                    {
                        foreach (string str in sArray)
                        {
                            if (layerIndex >= AxisTestForm.BOX_CHIPSCOUNT)
                            {
                                break;
                            }
                            string strText = m_MarkChipsData[nBoxIndex, layerIndex].strMarkContent;
                            Boolean bCheck = m_MarkChipsData[nBoxIndex, layerIndex].bSelect;

                            string sss = str;
                            if (layerIndex == 0)
                            {
                                sss = str.Substring(str.Length - 1, 1);
                            }

                            if (sss == "1" && bCheck)
                            {
                                //等到扫描结果
                                Point pp = new Point(layerIndex, AxisTestForm.BOXCHIP_WAITSTATE);
                                m_WorkChipList.m_LayerList.Add(pp);
                                m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_WAITSTATE;

                            }
                            else if(sss == "1")
                            {
                                m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_WAITSTATE;
                            }
                            else if (str == "E" || str == "W")
                            {
                                m_nWorkState = AxisTestForm.WORK_MANUAL;

                                m_WorkChipList.m_LayerList.Clear();

                                strInfo = string.Format("返回数据有误，请检查后重新扫描！-{0}", strShowInfo);

                                WarningForm warnForm = new WarningForm(mainForm);
                                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                                warnForm.ShowDialog();
                                warnForm = null;
                                return false;
                            }
                            else
                            {
                                m_WorkChipState[layerIndex] = AxisTestForm.BOXCHIP_NOTSCANSTATE;
                            }
                            layerIndex++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    try
                    {
                        //刷新料盒状态
                        tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_RUNSTATE);
                        m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;
                        m_WorkReadyBoxList.m_nCurrentChipIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    

                    Boolean bNext = true;
                    AddListViewThread(null, $"CheckScanData(): {CheckScanData()}", Color.Blue);
                    //检测扫码内容
                    if (CheckScanData() == false && bEapMode == false)
                    {
                        //CreateChipMarkData();
                        this.RefreshAllChipsUI(null, null);

                        DialogResult result;

                        strInfo = string.Format("选择产品和扫到产品不一致，是否继续 -{0}", strShowInfo);
                        result = MessageBox.Show(strInfo, "信息提示", MessageBoxButtons.YesNo);

                        AddListViewThread(null, strInfo, Color.Red);

                        if (result == DialogResult.Yes)
                        {
                        }
                        else
                        {
                            bNext = false;
                        }
                    }
                    else
                    {
                        //CreateChipMarkData();
                        this.RefreshAllChipsUI(null, null);
                    }
                    //生成打标数据
                    if (bNext)
                    {
                        //CreateChipMarkData();
                    }
                    else
                    {
                        m_WorkChipList.m_LayerList.Clear();

                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        return false;
                    }

                    //shuaxin 
                    //this.RefreshAllChipsUI(null, null);

                    if (m_WorkChipList.m_LayerList.Count <= 0)
                    {

                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        strInfo = string.Format("合法的数量为空,请检查后重新扫描！-{0}", "");


                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                        return false;
                    }
                }

                ResetWork();    

                m_nLArmLayerIndex = -1;
                m_nRArmLayerIndex = -1;
                m_nMarkLayerIndex = -1;
                m_nWorkLayerIndex = -1;

                //mainForm.mOpcAxisOperater.OperateVacummDevice(true);

                strInfo = $"扫料完成！扫码内容：{strMapping}";
                AddListViewThread(null, strInfo, Color.Blue);

                CommonDefine.m_SecsData.strCurrentMapContent = strMapping;

                if(bEAP)
                {
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupScanWaferContent, strMapping);
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupScanWaferResult, "0");
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupID, nBoxIndex.ToString());

                    mainForm.secsForm.SetEventPos(SECSEventData.LotStart);
                    mainForm.secsForm.SetEventPos(SECSEventData.ScanFoupWafers);

                    //m_bEAPLockMarkContent = true;

                    CommonDefine.bAccept = false;

                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                }
                else
                {
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                } 
            }
            else if (m_nRobotFlow == AxisTestForm.ROBOT_TAKE)
            {
                nSecsAlarmIndex = SECSAlarmData.UpArmFoupTakeFail;

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "机械手取料测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nRobotFlow = AxisTestForm.ROBOT_PUT_MARK;
                    return false;
                }

                if (m_WorkChipList.m_LayerList.Count <= 0)
                {
                    //m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //m_nRobotFlow = AxisTestForm.LOADPORT_READY;
                    //m_WorkReadyBoxList.m_BoxList.Clear();

                    //strInfo = "没有可取的料,请准备好后按开始！";
                    //AddListViewThread(null, strInfo, Color.Blue);
                    //ShowWarning(strInfo);

                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE_MARK;
                    return false;
                }

                mainForm.ClearStates();               

                strInfo = "机械手取料开始！";
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_WorkChipList.m_LayerList.Count <= 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nRobotFlow = AxisTestForm.SCAN_VALUE;

                    strInfo = "没有可取的料,请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo);
                    WriteItemValue(GLinkIOModule00.YellowLightIOOUT, true);
                    //mainForm.mOpcAxisOperater.OperateVacummDevice(false);

                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                int nBoxIndex = m_WorkChipList.m_nBoxIndex;

                bool bEap = mainForm.IsEAPRunMode();
                if (bEap)
                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;

                if (IsMaterialExit(nBoxIndex) == false)
                {
                    m_WorkChipList.m_LayerList.Clear();
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nRobotFlow = AxisTestForm.SCAN_VALUE;

                    strInfo = "没有可取的料,请准备好后按开始！";
                    //mainForm.mOpcAxisOperater.OperateVacummDevice(false);

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (IsSmifOpened(nBoxIndex) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "Smif料盒未打开,不可放片，请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }


                Boolean bXifu = IsRAarmXifu();
                if (bXifu)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "上手臂吸料有效，请检查！";
                    
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                ClearRobotError();

                int nStartCommandIndex = 0;
                if (nBoxIndex == 0)
                {
                    nStartCommandIndex = JelRobotOperate.TAKE_CASS1_FIRSTCHIP;
                }
                else if (nBoxIndex == 1)
                {
                    nStartCommandIndex = JelRobotOperate.TAKE_CASS2_FIRSTCHIP;
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nRobotFlow = AxisTestForm.SCAN_VALUE;

                    strInfo = "料盒信息错误,请准备好后按开始！";
                    
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                int nLayerIndex = m_WorkChipList.m_LayerList[0].X;
                int nSizeType = m_WorkChipList.m_nSizeType;
                m_nWorkBoxSize = nSizeType;

                if (nLayerIndex < 0 || nLayerIndex >= AxisTestForm.BOX_CHIPSCOUNT)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"层号信息不正确：{nLayerIndex}，请检查！";
                    //MessageBox.Show(strInfo);
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;

                    return false;
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex + nLayerIndex));

                //等待结束
                int nResult = 1;//WaitRobotResult();
                if (nResult == 1)
                {
                    int nStartTick = Environment.TickCount;

                    while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                    {
                        mainForm.robotSerialPortForm.serialPort.synReadAllData();
                        int dt = CommonDefine.DelayedTime(nStartTick);
                        if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                            break;
                    }

                    mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                    string strResult = mainForm.robotSerialPortForm.GetLastResponse();

                    if (strResult.Contains("OK") == false || IsNormalWarning())
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                        strInfo = string.Format("取料失败，请检查！返回内容：{0}", err);

                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                        return false;
                    }

                }

                m_nWorkLayerIndex = nLayerIndex;
                m_nRArmLayerIndex = nLayerIndex;

                m_WorkChipList.m_LayerList.RemoveAt(0);
                m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_RUNNINGSTATE;

               // markForm.secsForm.SetEventPos(SECSEventData.WaferStart);

                //this.RefreshAllChipsUI(null, null);
                strInfo = string.Format("料盒位上手臂取料成功：{0}", nLayerIndex + 1);
                AddListViewThread(null, strInfo, Color.Blue);
       
                if( mainForm.IsEAPRunMode() )
                {

                    //mainForm.secsForm.SetEventPos(SECSEventData.WaferEnd);
           
                    mainForm.secsForm.SetEventPos(SECSEventData.FoupTakeMaterial);
                }

                this.RefreshAllChipsUI(null, null);

                m_nTakePhotoNG = 0;

                if ( m_nMarkLayerIndex >= 0)
                {
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE_MARK;                 
                }else
                {
                    m_nRobotFlow = AxisTestForm.ROBOT_PUT_MARK;
                }
            }            
            else if (m_nRobotFlow == AxisTestForm.ROBOT_PUT_MARK)
            {
                if (m_nMarkFlow != AxisTestForm.MARK_READY)
                {
                    return bRet;
                }
                nSecsAlarmIndex = SECSAlarmData.UpArmAlignPutFail;

                // 伺服报警标志
                if (IsRobotEmStop())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    return bRet;
                }

                strInfo = "寻边器放料开始！";
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "机械手放料测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);

                    m_nMarkLayerIndex = 1;

                    m_nRobotFlow = AxisTestForm.PUT_SIMF;
                    m_nMarkFlow = AxisTestForm.MARK_FLOW_START;
                    return false;
                }

                int nBoxIndex = m_WorkChipList.m_nBoxIndex;

                bool bEAP = mainForm.IsEAPRunMode();
                if (bEAP)
                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;

                int nLayerIndex = m_nRArmLayerIndex;

                if (nBoxIndex == 0 || nBoxIndex == 1)
                {                   
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"料盒位置信息错误：{nBoxIndex},请准备好后按开始！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (nLayerIndex < 0 || nLayerIndex >= AxisTestForm.BOX_CHIPSCOUNT )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"层号信息不正确：{nLayerIndex}，请检查！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;

                    return false;
                }

                if ( IsXunbianExitWafer())                
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "寻边器上已有料，请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                Boolean bXifu = IsRAarmXifu();
                if (bXifu == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "上手臂没有吸料，请检查！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;

                    return false;
                }

                ClearRobotError();                          
                                
                int nStartCommandIndex = JelRobotOperate.R_PUTWAFER_ALIGN;
                if (nBoxIndex == 0 || nBoxIndex == 1)
                {
                    nStartCommandIndex = JelRobotOperate.R_PUTWAFER_ALIGN;
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "料盒信息错误,请准备好后按开始！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (OperateAlignerHelper.OperateAlignerReady(mainForm.waferSerialPortForm))
                {

                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "寻边器未到放料位,请准备好后按开始！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex));

                int nStartTick = Environment.TickCount;

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                        break;
                }
                mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                string strResult = mainForm.robotSerialPortForm.GetLastResponse();
                if (strResult.Contains("OK") == false || IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                    strInfo = string.Format("放料失败，请检查！返回内容：{0}", err);

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.A_MOVETO_ORIG));

                nStartTick = Environment.TickCount;
                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                        break;
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                strResult = mainForm.robotSerialPortForm.GetLastResponse();
                if (strResult.Contains("OK") == false || IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                    strInfo = string.Format("手臂退出失败，请检查！返回内容：{0}", err);

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (bEAP)
                {
                    mainForm.secsForm.SetEventPos(SECSEventData.WaferAlignPutMaterial);
                }

                m_nMarkLayerIndex = nLayerIndex;
                m_nRArmLayerIndex = -1;
                            
                if (m_nLArmLayerIndex >= 0)
                {
                    if (m_nTakePhotoNG > 0)
                    {
                        m_nRobotFlow = AxisTestForm.PUT_SIMF;
                    }
                    else
                    {
                        m_nRobotFlow = AxisTestForm.PUT_SIMF;
                    }
                }
                else
                {
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                }

                m_nTakePhotoNG = 0;

                strInfo = "寻边器放料完成！";
                AddListViewThread(null, strInfo, Color.Blue);

                m_nMarkFlow = AxisTestForm.MARK_FLOW_START;
            }            
            else if (m_nRobotFlow == AxisTestForm.ROBOT_TAKE_MARK)
            {

                if (m_nMarkFlow != AxisTestForm.MARK_READY)
                {
                    return bRet;
                }

                nSecsAlarmIndex = SECSAlarmData.UpArmAlignTakeFail;

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "上手臂寻边取料测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nRobotFlow = AxisTestForm.ROBOT_PUT_MARK;
                    return false;
                }

                if (IsXunbianExitWafer() == false)
                {
                    //m_WorkChipList.m_LayerList.Clear();
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                  
                    strInfo = "寻边位没有可取的料,请准备好后按开始,重新取料！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                ClearRobotError();                       
               
                //mainForm.mOpcAxisOperater.OperateWaferXifu(false);
                //Thread.Sleep(500);

                //Boolean bXifu = mainForm.mOpcAxisOperater.IsWaferXifu();
                //if (!bXifu)
                //{
                //    m_nWorkState = AxisTestForm.WORK_MANUAL;

                //    strInfo = "寻边吸料有效，请检查！";

                //    WarningForm warnForm = new WarningForm(mainForm);
                //    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                //    warnForm.ShowDialog();
                //    warnForm = null;
                //    return false;
                //}

                strInfo = "寻边器取料开始！";
                AddListViewThread(null, strInfo, Color.Blue);
               
                int nBoxIndex = m_WorkChipList.m_nBoxIndex;
                int nStartCommandIndex = 0;
                if (nBoxIndex == 0)
                {
                    nStartCommandIndex = JelRobotOperate.L_TAKEWAFER_ALIGN;
                }
                else if (nBoxIndex == 1)
                {
                    nStartCommandIndex = JelRobotOperate.L_TAKEWAFER_ALIGN;
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nRobotFlow = AxisTestForm.SCAN_VALUE;

                    strInfo = "料盒信息错误,请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                int nLayerIndex = m_nMarkLayerIndex;
                int nSizeType = m_WorkChipList.m_nSizeType;
                m_nWorkBoxSize = nSizeType;
                if (nLayerIndex < 0 || nLayerIndex >= AxisTestForm.BOX_CHIPSCOUNT)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"层号信息不正确：{nLayerIndex}，请检查！";
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;

                    return false;
                }

                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex));

                //等待结束
                int nResult = 1;// WaitRobotResult();
                if (nResult == 1)
                {
                    int nStartTick = Environment.TickCount;

                    while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                    {
                        mainForm.robotSerialPortForm.serialPort.synReadAllData();
                        int dt = CommonDefine.DelayedTime(nStartTick);
                        if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                            break;
                    }

                    mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                    string strResult = mainForm.robotSerialPortForm.GetLastResponse();

                    if (strResult.Contains("OK") == false || IsNormalWarning())
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                        strInfo = string.Format("取料失败，请检查！返回内容：{0}", err);

                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                        return false;
                    }

                }

                if (mainForm.IsEAPRunMode())
                {
                    mainForm.secsForm.SetEventPos(SECSEventData.WaferAlignTakeMaterial);
                }

                m_nWorkLayerIndex = nLayerIndex;
                m_nMarkLayerIndex = -1;

                m_nLArmLayerIndex = nLayerIndex;

                m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_RUNNINGSTATE;

                this.RefreshAllChipsUI(null, null);

                strInfo = "寻边器取料完成！";
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_nRArmLayerIndex >= 0)
                {                   
                    m_nRobotFlow = AxisTestForm.ROBOT_PUT_MARK;                    
                }
                else
                {
                    m_nRobotFlow = AxisTestForm.PUT_SIMF;
                }

            }
            else if (m_nRobotFlow == AxisTestForm.PUT_SIMF)
            {

                nSecsAlarmIndex = SECSAlarmData.DnArmFoupPutFail;

                strInfo = "Smif料盒放料开始";
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "Smif料盒放料测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                    return false;
                }

                // 伺服报警标志
                if (IsRobotEmStop())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    return bRet;
                }


                int nBoxIndex = m_WorkChipList.m_nBoxIndex;

                bool bEAP = mainForm.IsEAPRunMode();
                if (bEAP)
                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;

                int nLayerIndex = m_nLArmLayerIndex;
                if (nLayerIndex < 0 || nLayerIndex >= AxisTestForm.BOX_CHIPSCOUNT)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = $"层号信息不正确：{nLayerIndex}，请检查！";
                    //MessageBox.Show(strInfo);
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo,nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;

                    return false;
                }

                if (IsSmifExit(nBoxIndex) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "没有Smif料盒可放,请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                if (IsSmifOpened(nBoxIndex) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "Smif料盒未打开,不可放片，请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                ClearRobotError();

                int nStartCommandIndex = 0;
                if (nBoxIndex == 0)
                {
                    nStartCommandIndex = JelRobotOperate.PUT_CASS1_FIRSTCHIP2;
                }
                else if (nBoxIndex == 1)
                {
                    nStartCommandIndex = JelRobotOperate.PUT_CASS2_FIRSTCHIP2;
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "料盒信息错误,请准备好后按开始！";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                Boolean bXifu = IsLAarmXifu();
                if (bXifu == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = "没有吸料，请检查！，复位后将取下一片料";

                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;                                    
                
                    return false;
                }
                                
                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(nStartCommandIndex + nLayerIndex));

                int nResult = 1;
                if (nResult == 1)
                {
                    int nStartTick = Environment.TickCount;

                    while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                    {
                        mainForm.robotSerialPortForm.serialPort.synReadAllData();
                        int dt = CommonDefine.DelayedTime(nStartTick);
                        if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                            break;
                    }

                    mainForm.robotSerialPortForm.serialPort.Synchronous = false;
                    string strResult = mainForm.robotSerialPortForm.GetLastResponse();
                    if (strResult.Contains("OK") == false || IsNormalWarning())
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                        strInfo = string.Format("放料失败，请检查！返回内容：{0}", err);

                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                        return false;
                    }

                    mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                    mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                    mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.F_MOVETO_ORIG));
                    nResult = 1;

                    if (nResult == 1)
                    {
                        nStartTick = Environment.TickCount;

                        while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                        {
                            mainForm.robotSerialPortForm.serialPort.synReadAllData();
                            int dt = CommonDefine.DelayedTime(nStartTick);
                            if (dt > m_RobotCommdSleep && IsRobotRuning() == false)
                                break;
                        }

                        mainForm.robotSerialPortForm.serialPort.Synchronous = false;

                        strResult = mainForm.robotSerialPortForm.GetLastResponse();
                        if (strResult.Contains("OK") == false || IsNormalWarning(false))
                        {
                            m_nWorkState = AxisTestForm.WORK_MANUAL;

                            string err = mainForm.robotSerialPortForm.GetLastResponseForRobot(false);
                            strInfo = string.Format("手臂退出失败，请检查！返回内容：{0}", err);

                            WarningForm warnForm = new WarningForm(mainForm);
                            warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                            warnForm.ShowDialog();
                            warnForm = null;
                            return false;
                        }
                    }

                    m_nFinishCount++;
                    WorkCountAdd();
                    SetOperateTime();

                    if (m_nTakePhotoNG > 0 || m_nCheckOCRNG > 0)
                    {
                        m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_FAILSTATE;
                    }
                    else
                    {
                        m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_FINISHSTATE;

                    }
                }

                RefreshAllChipsUI(null, null);

                strInfo = $"smif放料完成-料盒：{nBoxIndex+1}--层号：{nLayerIndex+1}！";
                AddListViewThread(null, strInfo, Color.Blue);

                if (mainForm.IsEAPRunMode())
                {
                    //mainForm.secsForm.SetValue(SECSWorkFlowParam.WaferEndSlot, (nLayerIndex + 1).ToString());
                    //mainForm.secsForm.SetEventPos(SECSEventData.WaferEnd);

                    mainForm.secsForm.SetEventPos(SECSEventData.FoupOKPutMaterial);
                    mainForm.secsForm.SetEventPos(SECSEventData.WaferEnd);
                }

                m_nLArmLayerIndex = -1;

                int nListIndex = FindWorkIndexByBoxIndex(nBoxIndex);

                if (m_WorkChipList.m_LayerList.Count > 0 )
                {
                    m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
                }
                else
                {
                    if( m_nMarkLayerIndex  < 0 )
                    {
                        try
                        {
                            mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_ORIG));
                            //mainForm.mOpcAxisOperater.OperateRobotX_WorkPos(PLCOpcFeedParameters.PLCConstDefine.ROBOT_SBOX_POS);
                        }
                        catch (Exception)
                        {
                        }

                        CloseOneSmif(nBoxIndex);

                        int nLaserType = 0;
                        CommonDefine.waferFileInfo.ClearMarkContent(nLaserType, nBoxIndex);

                        string strContent = m_strMarkContentArray[0];
                        for(int i = 1; i < AxisTestForm.BOX_CHIPSCOUNT; i++ )
                        {
                            strContent = strContent +","+ m_strMarkContentArray[i];
                        }

                        if (mainForm.IsEAPRunMode())
                        {
                            //mainForm.secsForm.SetValue(SECSWorkFlowParam.MarkContent, strContent);
                            mainForm.secsForm.SetEventPos(SECSEventData.LotEnd);

                            m_nWorkState = AxisTestForm.WORK_MANUAL;

                            strInfo = $"扫描的料已完成---料盒：{nBoxIndex}";

                            //WarningForm warnForm = new WarningForm(mainForm);
                            //warnForm.SetInfoText(strInfo, 1500,500, 2);
                            //warnForm.ShowDialog();
                            //warnForm = null;

                            AddListViewThread(null, strInfo, Color.Green);

                            OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
                            
                            WriteItemValue(GLinkIOModule00.BeepIOOUT, true);
                            Thread.Sleep(5000);
                            WriteItemValue(GLinkIOModule00.BeepIOOUT, false);

                        }
                        else
                        {
                            m_nRobotFlow = AxisTestForm.SCAN_VALUE;
                        }

                        CommonDefine.bAccept = true;

                        int count = 0;
                        if (ckb_cycleMode.Checked)
                        {
                            count = int.Parse(tb_CycleNumber.Text);
                            count--;
                            if (tb_CycleNumber.InvokeRequired)
                            {
                                tb_CycleNumber.Invoke(new Action(() => { tb_CycleNumber.Text = count.ToString(); }));
                            }
                            else
                            {
                                tb_CycleNumber.Text = count.ToString();
                            }

                            if (count <= 0)
                            {
                                m_nWorkState = AxisTestForm.WORK_MANUAL;
                                strInfo = "循环测试已完成！";
                                AddListViewThread(null, strInfo, Color.Blue);
                            }
                        }
                    }
                    else
                    {
                        m_nRobotFlow = AxisTestForm.ROBOT_TAKE_MARK;
                    }

                    if (ckb_cycleMode.Checked == false)
                    {
                        //刷新料盒工作状态
                        Point tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_FINISHSTATE);
                        m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;
                    }

                    m_nStartLightTick = Environment.TickCount;
                }
            }                        
            return bRet;
        }

        public void ShowWarningDialog(string strWarningInfo, int nSecsAlarmIndex = -1)
        {
            WarningForm warnForm = new WarningForm(mainForm);
            warnForm.SetInfoText(strWarningInfo, nSecsAlarmIndex);
            warnForm.ShowDialog();
            warnForm = null;
        }

        /// <summary>
        /// nIndex  0 打标焦距    1：检测OCR 焦距
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public Boolean MarkZMoveToWorkPoint(int nIndex)
        {
            Boolean bRet = true;
            string strInfo = "";
            GtsControl gtsControl = mainForm.gtsForm.m_gtsControl;

            DoublePoint robotXPoint = CommonDefine.waferFileInfo.axisData.workPoint;
            if (nIndex == 0)
            {
                robotXPoint = CommonDefine.waferFileInfo.axisData.workPoint;
                gtsControl.P2PMove(AxisTestForm.MARKZ_AXIS, robotXPoint.fz, true);

            }
            else if (nIndex == 1)
            {
                robotXPoint = CommonDefine.waferFileInfo.axisData.workPoint2;
                gtsControl.P2PMove(AxisTestForm.MARKZ_AXIS, robotXPoint.fz, true);

            }
            else
            {
                bRet = false;
            }

            return bRet;
        }

        public Boolean RobotMoveToWorkPoint(int nIndex)
        {
            Boolean bRet = true;
            string strInfo = "";
            GtsControl gtsControl = mainForm.gtsForm.m_gtsControl;

            double fValue = CommonDefine.m_RobotPos.fx;
            if (nIndex == 0)
            {
                fValue = CommonDefine.m_RobotPos.fx;
                gtsControl.P2PMove(AxisTestForm.ROBOT_AXIS, fValue, true);

                //object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotXAligner_IN);
                //bRet = (Boolean)ob;
                bRet = mainForm.gtsForm.m_gtsControl.IsAxisOrig(AxisTestForm.ROBOT_AXIS);
            }
            else if (nIndex == 1)
            {
                fValue = CommonDefine.m_RobotPos.fx2;
                gtsControl.P2PMove(AxisTestForm.ROBOT_AXIS, fValue, true);

                object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotXSmif2_IN);
                bRet = (Boolean)ob;
            }
            else if (nIndex == 2)
            {
                fValue = CommonDefine.m_RobotPos.fy;
                gtsControl.P2PMove(AxisTestForm.ROBOT_AXIS, fValue, true);

                object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotXAligner_IN);
                bRet = (Boolean)ob;
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }
        /// <summary>
        /// 寻边打标平台工作流程
        /// </summary>
        /// <returns></returns>
        public Boolean OperateMarkFlow()
        {
            Boolean bRet = true;

            if (m_nWorkState != AxisTestForm.WORK_AUTO)
                return bRet;

            int nSecsAlarmIndex = -1;
            // 伺服报警标志
            if (IsRobotEmStop(false))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                return bRet;
            }

            if (IsNormalWarning(false))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                return bRet;
            }

            //if (IsOPCNormalWarning(false))
            //{
            //    m_nWorkState = AxisTestForm.WORK_MANUAL;

            //    return bRet;
            //}

            string strInfo = "";
         
            if (m_nMarkFlow == AxisTestForm.MARK_FLOW_START)
            {
                nSecsAlarmIndex = SECSAlarmData.WaferAlignFail;

                int nMarkAngle = CommonDefine.waferFileInfo.AllLaserMarkContent[0].nMarkAngle;

                strInfo = string.Format("自动寻边开始：" + nMarkAngle);               
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "载台位准备测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nMarkFlow = AxisTestForm.MARK_NOTCH_MARK;
                    return false; 
                }

                //OperateAlignerHelper.SelectWaferPos(mainForm.waferSerialPortForm, nMarkAngle);
                if (/*OperateAlignerHelper.OperateFindNotch(mainForm.waferSerialPortForm) == false*/
                    OperateAlignerHelper.OperateAlignerWA(mainForm.waferSerialPortForm, nMarkAngle) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    OperateAlignerHelper.Reset(mainForm.waferSerialPortForm);

                    strInfo = string.Format("寻边器寻边失败，请检查！是否重试");
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.m_nShowContinue = true;
                    warnForm.ShowDialog();

                    if (warnForm.DialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        SetWorkSate(AxisTestForm.WORK_AUTO);

                        OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                       
                        warnForm = null;
                        return false;
                    }
                    else
                    {
                        m_nTakePhotoNG++;
                        SetWorkSate(AxisTestForm.WORK_MANUAL);

                        int nLayerIndex = m_nMarkLayerIndex;
                        m_strMarkContentArray[nLayerIndex] = "0";

                        OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, true);
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerYellowLight, true);

                        m_nMarkFlow = AxisTestForm.MARK_FINISH;
                        warnForm = null;

                        MessageBox.Show("请手动取料和初始化！", "系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    
                }

                if (mainForm.IsEAPRunMode())
                {
                    mainForm.secsForm.SetEventPos(SECSEventData.WaferPosition);
                }

                strInfo = string.Format("自动寻边结束");
                AddListViewThread(null, strInfo, Color.Blue);

                m_nMarkFlow = AxisTestForm.MARK_NOTCH_MARK;

                if (ckb_cycleMode.Checked)
                {
                    m_nMarkFlow = AxisTestForm.MARK_FINISH;
                    // m_nMarkFlow = AxisTestForm.MARK_NOTCH_CHECK;
                    OperateAlignerHelper.OperateFindNotch2(mainForm.waferSerialPortForm, nMarkAngle);
                }
            }            
            else if (m_nMarkFlow == AxisTestForm.MARK_NOTCH_MARK)
            {
                //nSecsAlarmIndex = SECSAlarmData.WaferMarkFail;

                // 伺服报警标志
                if (IsRobotEmStop())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }        
                
                //if (bMarkBack == 1 || bMarkBack == 3)
                {                    
                    AddListViewThread(null, "打标开始", Color.Blue);

                    bool bEap = mainForm.IsEAPRunMode();


                    if (m_bTestFlow)
                    {
                        Thread.Sleep(3000);
                        strInfo = "打标测试完成";
                        AddListViewThread(null, strInfo, Color.Blue);
                        m_nMarkFlow = AxisTestForm.MARK_NOTCH_CHECK;
                        return false;
                    }

                    int nMarkType = 0;
                    if (MarkZMoveToWorkPoint(nMarkType) == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        strInfo = "打标Z轴到位错误,请准备好后按开始！";
                                                                  
                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                       
                        return false;
                    }
                                      
                    int nMarkAngle = CommonDefine.waferFileInfo.AllLaserMarkContent[0].nMarkAngle;
                    if(OperateAlignerHelper.OperateFindNotch2(mainForm.waferSerialPortForm, nMarkAngle) == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        strInfo = string.Format("寻边器打标位校准失败，请检查后重试！");
                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.ShowDialog();
                        warnForm = null;
                        return false;
                    }

                    //发送打标信号                    
                    try
                    {
                        if (m_nWorkTestMode == 0)
                        {
                            //mark
                            if (mainForm.markForm != null)
                            {
                                CommonDefine.TickSleep(500);

                                //if( m_gtsForm.markForm.MarkPostion() )
                                int nBoxIndex = m_WorkChipList.m_nBoxIndex;

                                bool bEAP = mainForm.IsEAPRunMode();
                                if (bEAP)
                                {
                                    nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;
                                }

                                int nLayerIndex = m_nMarkLayerIndex;

                                if (bEap)
                                {
                                    mainForm.secsForm.SetValue(SECSWorkFlowParam.WaferOderID, (nLayerIndex + 1).ToString());
                                    mainForm.secsForm.SetEventPos(SECSEventData.LaserStart);
                                }

                                string strText = CommonDefine.waferFileInfo.GetMarkContent(0, nBoxIndex, nLayerIndex);

                                m_strMarkContentArray[nLayerIndex] = strText;
                                // string strText = CommonDefine.waferFileInfo.nProductIndex//m_MarkChipsData[nBoxIndex, nLayerIndex].strMarkContent;
                                Boolean bSucess = false;
                                if( strText.Length > 0 )
                                {
                                    try
                                    {
                                        int nCard = mainForm.markForm.m_nAddress0Card;

                                        CommonDefine.m_PosStruct.fx = m_PosStructZ.fx;
                                        CommonDefine.m_PosStruct.fy = m_PosStructZ.fy;
                                        CommonDefine.m_PosStruct.fr = m_PosStructZ.fr;

                                        //MessageBox.Show("开始正面打标");
                                        bSucess = mainForm.markForm.Mark(nCard, strText);

                                        AddListViewThread(null, $"料盒：{nBoxIndex}，层号：{nLayerIndex},打标内容：{strText}", Color.Blue);
                                    }
                                    catch
                                    {
                                        //bSucess = true;
                                    }

                                }

                                if (bSucess)
                                {
                                    if (bEap)
                                    {
                                        mainForm.secsForm.SetValue(SECSWorkFlowParam.MarkContent, strText);
                                        mainForm.secsForm.SetValue(SECSWorkFlowParam.MarkResult, "0");
                                        mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupID, nBoxIndex.ToString());
                                    }
                                }
                                else
                                {
                                    //m_nWorkState = AxisTestForm.WORK_MANUAL;

                                    if (bEap)
                                    {
                                        mainForm.secsForm.SetValue(SECSWorkFlowParam.MarkResult, "1");
                                    }

                                    m_nTakePhotoNG = 5;
                                    strInfo = $"打标失败,打标内容：{strText},请准备好后按开始！";
                                    //MessageBox.Show(strInfo);
                                    WarningForm warnForm = new WarningForm(mainForm);
                                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                                    //warnForm.ShowDialog();
                                    warnForm = null;
                                    //return false;
                                }

                                if (bEap)
                                {
                                    mainForm.secsForm.SetEventPos(SECSEventData.LaserComplete);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                AddListViewThread(null, "打标结束",Color.Blue);
                m_nMarkFlow = AxisTestForm.MARK_NOTCH_CHECK;

            }
            else if (m_nMarkFlow == AxisTestForm.MARK_NOTCH_CHECK)
            {
                nSecsAlarmIndex = SECSAlarmData.AlignOCRReadFail;

                //Boolean bNeedOCR1 = mainForm.IsNeedReadOCR(0);         
                //if (bNeedOCR1 == false )
                //{
                //    AddListViewThread(null, "Recipe检测已屏蔽", Color.Blue);
                //    m_nMarkFlow = AxisTestForm.MARK_FINISH;
                //    return bRet;
                //}
                
                if(ckb_CameraEnble.Checked == false)
                {
                    AddListViewThread(null, "主页检测已屏蔽", Color.Blue);
                    m_nMarkFlow = AxisTestForm.MARK_FINISH;
                    return bRet;
                }
               
                // 伺服报警标志
                if (IsRobotEmStop())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    return bRet;
                }

                int nCheckAngle = CommonDefine.waferFileInfo.AllLaserMarkContent[0].nCheckAngle;

                strInfo = "检测开始: " + nCheckAngle;
                AddListViewThread(null, strInfo, Color.Blue);

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "检测测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nMarkFlow = AxisTestForm.MARK_NOTCH_CHECK;
                    return false;
                }

                int nBoxIndex = m_nWorkBoxIndex;
                int nLayerIndex = m_nMarkLayerIndex;
                int nSizeType = m_WorkChipList.m_nSizeType;

                int nOCRGas = CommonDefine.waferFileInfo.nOCRGasPos;
                if (nOCRGas > 0)
                {
                    Boolean bValue = false;
                    short nIoIndex = GLinkIOModule01.OCRGasOrig_IN;
                    object ob = false;
                    if (CommonDefine.waferFileInfo.nOCRGasPos == 2)
                    {
                        bValue = true;
                        nIoIndex = GLinkIOModule01.OCRGasOut_IN;
                    }
                    ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, nIoIndex);

                    Boolean bEnd = (Boolean)ob;
                    if (bEnd == false)
                    {
                        WriteItemValue(GLinkIOModule00.OCRGasIOOUT, bValue);
                        Thread.Sleep(1000);
                        for (int i = 0; i < 10; i++)
                        {
                            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, nIoIndex);
                            bEnd = (Boolean)ob;
                            if (bEnd)
                            {
                                break;
                            }
                            Thread.Sleep(500);
                        }

                        Thread.Sleep(1000);
                    }
                }


                //if (mainForm.mOpcAxisOperater.OperateOCR_WorkPos() == false)
                //{
                //    m_nWorkState = AxisTestForm.WORK_MANUAL;

                //    strInfo = string.Format("到检测位失败，请检查后重试！");
                //    //MessageBox.Show(strInfo);
                //    WarningForm warnForm = new WarningForm(mainForm);
                //    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                //    warnForm.ShowDialog();
                //    warnForm = null;
                //    return false;
                //}

                if(OperateAlignerHelper.OperateFindNotch2(mainForm.waferSerialPortForm, nCheckAngle) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = string.Format("寻边器到检测位失败，请检查后重试！");
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                //if ((bMarkBack == 1 || bMarkBack == 3) && bNeedOCR1)
                {
                                      
                    bool bEap = mainForm.IsEAPRunMode();
                    if (bEap)
                    {
                        mainForm.secsForm.SetEventPos(SECSEventData.CheckLaserResultStart);
                    }
                                     
                    int nType = 0;//检测
                    Boolean bOCR = OCRScan(nType);
                    if (bOCR == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        strInfo = string.Format("检测失败，请检查！选择继续还是重试:OCR内容:{0}", CommonDefine.m_PosStruct.strContent);

                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.m_nShowContinue = true;
                        warnForm.ShowDialog();

                        if (warnForm.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            SetWorkSate(AxisTestForm.WORK_AUTO);

                            OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, true);
                        }
                        else
                        {
                            m_nTakePhotoNG++;
                            SetWorkSate(AxisTestForm.WORK_AUTO);

                            m_strMarkContentArray[nLayerIndex] = "1";

                            OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, true);

                            m_nMarkFlow = AxisTestForm.MARK_FINISH;
                        }

                        warnForm = null;
                        return false;
                    }

                    if (CheckMarkContent(0, nBoxIndex, nLayerIndex) == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;

                        string strMark = CommonDefine.waferFileInfo.GetMarkContent(0, nBoxIndex, nLayerIndex);

                        strInfo = string.Format("检测比对失败，请检查！选择继续还是重试:打标内容：{0},OCR内容:{1}", strMark, CommonDefine.m_PosStruct.strContent);
                        //MessageBox.Show(strInfo);

                        WarningForm warnForm = new WarningForm(mainForm);
                        warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                        warnForm.m_nShowContinue = true;
                        warnForm.ShowDialog();


                        if (warnForm.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            SetWorkSate(AxisTestForm.WORK_AUTO);

                            OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, true);
                        }
                        else
                        {
                            m_nTakePhotoNG++;
                            SetWorkSate(AxisTestForm.WORK_AUTO);

                            m_strMarkContentArray[nLayerIndex] = "1";

                            OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerGreenLight, true);

                            m_nMarkFlow = AxisTestForm.MARK_FINISH;
                        }

                        warnForm = null;
                        return false;
                    }

                    if (bEap)
                    {
                        mainForm.secsForm.SetValue(SECSWorkFlowParam.CheckResultContent, CommonDefine.m_PosStruct.strContent);
                        mainForm.secsForm.SetValue(SECSWorkFlowParam.CheckResult, "0");
                        mainForm.secsForm.SetEventPos(SECSEventData.CheckLaserResultComplete);
                    }
                }
               
                m_nCheckOCRNG = 0;

                AddListViewThread(null, "检测结束: " + CommonDefine.m_PosStruct.strContent, Color.Blue);
                m_nMarkFlow = AxisTestForm.MARK_FINISH;
            }
            else if (m_nMarkFlow == AxisTestForm.MARK_FINISH)
            {
                // 伺服报警标志
                if (IsRobotEmStop())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    return bRet;
                }

       
                //AddListViewThread(null, "到接片位开始", Color.Blue);

                if (m_bTestFlow)
                {
                    Thread.Sleep(3000);
                    strInfo = "到接片位测试完成";
                    AddListViewThread(null, strInfo, Color.Blue);
                    m_nMarkFlow = AxisTestForm.MARK_READY;
                    return false;
                }

                if(OperateAlignerHelper.CloseXIFU(mainForm.waferSerialPortForm) == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;

                    strInfo = string.Format("寻边器破真空失败，请检查后重试！");
                    //MessageBox.Show(strInfo);
                    WarningForm warnForm = new WarningForm(mainForm);
                    warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                    warnForm.ShowDialog();
                    warnForm = null;
                    return false;
                }

                m_nMarkFlow = AxisTestForm.MARK_READY; 
            }
            
            return bRet;
        }


        public Boolean ResetSmif(int nBoxIndex )
        {
            Boolean bRet = true;
            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            OperateSmifHelper.Reset(tcpForm);

            return bRet;
        }

        public Boolean IsSmifError(int nBoxIndex)
        {
            Boolean bRet = true;
            short nItem = GLinkIOModule01.Smif1Error_IN;
            if(nBoxIndex == 1)
            {
                nItem = GLinkIOModule01.Smif2Error_IN;
            }

            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, nItem);

            
            if (ob == null)
            {
                return bRet;
            }

            bRet = (Boolean)ob;

            return bRet;
        }

        public Boolean IsSmifOpened(int nBoxIndex)
        {
            Boolean bRet = true;
            short nItem = GLinkIOModule01.Smif1Open_IN;
            if (nBoxIndex == 1)
            {
                nItem = GLinkIOModule01.Smif2Open_IN;
            }

            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, nItem);
            
            if (ob == null)
            {
                return false;
            }

            bRet = (Boolean)ob;

            return bRet;
        }

        /// <summary>
        /// Smif工作流程
        /// </summary>
        /// <returns></returns>
        public Boolean OperateSmifFlow()
        {
            Boolean bRet = true;

            if (m_nWorkState != AxisTestForm.WORK_AUTO)
                return bRet;


            // 伺服报警标志
            if (IsRobotEmStop(false))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                return bRet;
            }

            if (IsNormalWarning(false))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                return bRet;
            }

            //if (IsOPCNormalWarning(false))
            //{
            //    m_nWorkState = AxisTestForm.WORK_MANUAL;

            //    return bRet;
            //}

            string strInfo = "";

            if (m_nSmifFlow == AxisTestForm.OPEN_SMIF)
            {
                ThreadOpenSmif();

                m_nSmifFlow = AxisTestForm.SMIF_READY;

            }
            else if (m_nSmifFlow == AxisTestForm.CLOSE_SMIF)
            {

                ThreadCloseSmif();
                m_nSmifFlow = AxisTestForm.SMIF_READY;
            }           

            return bRet;
        }


        public void ThreadOpenSmif()
        {
            Task smifTask = new Task( new Action(OpenSmif) );

            smifTask.Start();
        }

        public void ThreadCloseSmif()
        {
            Task smifTask = new Task(new Action(CloseSmif));

            smifTask.Start();
        }

        private void OpenSmif()
        {
            string strInfo = "开始打开Smif";
            AddListViewThread(null, strInfo, Color.Blue);

            int nSecsAlarmIndex = SECSAlarmData.FoupLoadFail;
            if (m_bTestFlow)
            {
                Thread.Sleep(3000);
                strInfo = "打开Smif测试完成";
                AddListViewThread(null, strInfo, Color.Blue);
                return ;
            }

            int nListIndex = FindFirstWorkIndex();            

            int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "料盒索引错误,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                return ;
            }

            
            TCPForm tcpForm = mainForm.smifForm;
            if( nBoxIndex > 0 )
            {
                tcpForm = mainForm.smifForm2;
            }

            if ( IsSmifError(nBoxIndex) )
            {
                ResetSmif(nBoxIndex);
            }

            if (IsSmifError(nBoxIndex))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "smif错误报警,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                return;
            }

            string strRet = OperateSmifHelper.OpenSmif(tcpForm);
            Thread.Sleep(500);
            Boolean bOpened = IsSmifOpened(nBoxIndex);
            if ( strRet.Length <= 0 || bOpened == false )
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = $"打开Smif失败--{strRet}--{bOpened}--{nBoxIndex}";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                AddListViewThread(null, strInfo, Color.Red);
                return;
            }

            strInfo = $"打开Smif成功--{strRet}";
            AddListViewThread(null, strInfo, Color.Blue);

            string strMap = OperateSmifHelper.GetMappiingContent(tcpForm);
            if( strMap.Contains("1") )
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = $"Smif有料--{strMap}--{bOpened}--{nBoxIndex}";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                AddListViewThread(null, strInfo, Color.Red);
                return;
            }

        }

        private void CloseSmif()
        {
            int nSecsAlarmIndex = SECSAlarmData.FoupUnLoadFail;

            string strInfo = "开始关闭Smif";
            AddListViewThread(null, strInfo, Color.Blue);

            if (m_bTestFlow)
            {
                Thread.Sleep(3000);
                strInfo = "关闭Smif测试完成";
                AddListViewThread(null, strInfo, Color.Blue);
                return;
            }

            int nListIndex = FindFirstWorkIndex();
           
            int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;

            if (mainForm.IsEAPRunMode())
            {
                nBoxIndex = CommonDefine.m_SecsData.nBoxIndexWork;
            }

            if (nBoxIndex < 0 || nBoxIndex > 1)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "料盒索引错误,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                return;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            if (IsSmifError(nBoxIndex))
            {
                ResetSmif(nBoxIndex);
            }

            if (IsSmifError(nBoxIndex))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "smif错误报警,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;
                return;
            }

            string strRet = OperateSmifHelper.CloseSmif(tcpForm);
            if (strRet.Length > 0)
            {
                strInfo = $"关闭Smif失败--{strRet}";

                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo, nSecsAlarmIndex);
                warnForm.ShowDialog();
                warnForm = null;

                AddListViewThread(null, strInfo, Color.Red);
                return;
            }

            strInfo = $"关闭Smif成功--{strRet}";
            AddListViewThread(null, strInfo, Color.Blue);
        }

        private Boolean CloseOneSmif( int nBoxIndex )
        {
            Boolean bRet = false;

            string strInfo = $"开始关闭Smif:{nBoxIndex}";
            AddListViewThread(null, strInfo, Color.Blue);

            if (m_bTestFlow)
            {
                Thread.Sleep(3000);
                strInfo = "关闭Smif测试完成";
                AddListViewThread(null, strInfo, Color.Blue);
                return true;
            }

          
            if (nBoxIndex < 0 || nBoxIndex > 1)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "料盒索引错误,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                warnForm.ShowDialog();
                warnForm = null;
                return bRet;
            }

            TCPForm tcpForm = mainForm.smifForm;
            if (nBoxIndex > 0)
            {
                tcpForm = mainForm.smifForm2;
            }

            if (IsSmifError(nBoxIndex))
            {
                ResetSmif(nBoxIndex);
            }

            if (IsSmifError(nBoxIndex))
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                strInfo = "smif错误报警,请准备好后按开始！";
                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                warnForm.ShowDialog();
                warnForm = null;
                return bRet;
            }

            string strRet = OperateSmifHelper.CloseSmif(tcpForm);
            if (strRet.Length > 0)
            {
                strInfo = $"关闭Smif失败请检查！是否重试--{strRet}";

                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                AddListViewThread(null, strInfo, Color.Red);
                DialogResult dlr = warnForm.ShowDialog();

                if(dlr == DialogResult.Yes)
                {
                    CloseOneSmif(nBoxIndex);
                    warnForm = null;
                }
                else
                {
                    warnForm = null;
                    return bRet;
                }
            }

            strInfo = $"关闭Smif成功--{strRet}";
            AddListViewThread(null, strInfo, Color.Blue);

            return true;
        }

        public void WriteItemValue(int nIndex, Boolean bValue, short mdl = -1)
        {
            short ioIndex = (short)nIndex;
            int nValue = 0;
            if (bValue)
            {
                nValue = 1;
            }
            mainForm.gtsForm.m_gtsControl.SetDoValue(mdl, ioIndex, nValue);
        }

        public void OperateClearLight(int nLightIO = -1)
        {
            Boolean bOpcMode = false;
            if (bOpcMode)
            {
                WriteItemValue(PLCOpcFeedParameters.IOOutState.TowerYellowLight, false);
                WriteItemValue(PLCOpcFeedParameters.IOOutState.TowerGreenLight, false);
                WriteItemValue(PLCOpcFeedParameters.IOOutState.TowerRedLight, false);
                WriteItemValue(PLCOpcFeedParameters.IOOutState.TowerBuzzer, false);

            }
            else
            {
                WriteItemValue(GLinkIOModule00.YellowLightIOOUT, false);
                WriteItemValue(GLinkIOModule00.GreenLightIOOUT, false);
                WriteItemValue(GLinkIOModule00.RedLightIOOUT, false);
                WriteItemValue(GLinkIOModule00.BeepIOOUT, false);

            }

            if (nLightIO >= 0)
            {
                WriteItemValue(nLightIO, true);
            }
            else
            {
                if (bOpcMode)
                {
                    WriteItemValue(PLCOpcFeedParameters.IOOutState.TowerYellowLight, true);

                }
                else
                {
                    WriteItemValue(GLinkIOModule00.YellowLightIOOUT, true);

                }

            }
        }

        public void SetOperateTime()
        {
            string strInfo = string.Format("{0}:{1}:{2}",m_StartTime.Hour,m_StartTime.Minute,m_StartTime.Second);
            Color bkColor = Color.Black;
            SetThreadText(labelStartTime, bkColor, strInfo);

            int nTotalCount = m_WorkChipList.m_LayerList.Count + m_nFinishCount;

            DateTime endTime = DateTime.Now;
            TimeSpan dTime = endTime - m_StartTime;
            int h = 0;
            int m = 0;
            int s = 0;            

            if ( m_nFinishCount == 0 )
            {
                m_StartTime = DateTime.Now;

                strInfo = string.Format("{0}:{1}:{2}", m_StartTime.Hour, m_StartTime.Minute, m_StartTime.Second);
                SetThreadText(labelStartTime, bkColor, strInfo);

                SetThreadText(labelRunTime, bkColor, "");
                SetThreadText(labelEndTime, bkColor, "");
                SetThreadText(labelCacuTime, bkColor, "");
                SetThreadText(labelLastTime, bkColor, "");
            }else
            {

                h = dTime.Hours;
                m = dTime.Minutes;
                s = dTime.Seconds;
                strInfo = string.Format("{0}:{1}:{2}", h, m, s);
                SetThreadText(labelRunTime, bkColor, strInfo);


                m_nOneChipTime = (int)(dTime.TotalSeconds / m_nFinishCount);

                int nTotalSeconds = nTotalCount * m_nOneChipTime;

                h = (int)(nTotalSeconds/3600);
                m = (int)( (nTotalSeconds - h*3600)/ 60 );
                s = nTotalSeconds - h*3600 - m*60;

                strInfo = string.Format("{0}:{1}:{2}", h, m, s);
                SetThreadText(labelCacuTime, bkColor, strInfo);

                int nLeftSeconds = (nTotalCount - m_nFinishCount) * m_nOneChipTime;
                h = (int)(nLeftSeconds / 3600);
                m = (int)((nLeftSeconds - h * 3600) / 60);
                s = nLeftSeconds - h * 3600 - m * 60;

                strInfo = string.Format("{0}:{1}:{2}", h, m, s);
                SetThreadText(labelLastTime, bkColor, strInfo);
            }
            
        }
        public Boolean OperateWorkState()
        {
            Boolean bRet = false;
            Boolean bValue = false;

            if (IsRobotEmStop())
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;
                return bRet;
            }

         
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.StopIOIN);
           
            Boolean bStopClose = false;
            if( ob != null )
            {
                //if (obStop != null)
                //{
                //    bStopClose = (Boolean)obStop;
                //}
                bValue = (Boolean)ob;
                //if (bStopClose)
                //{
                //    bValue = !bValue;
                //}
            }
            
            if (bValue)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;
                OperateClearLight( GLinkIOModule00.YellowLightIOOUT);
                           
                return bRet;
            }

            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.StartIOIN);
            if (ob != null)
            {
                bValue = (Boolean)ob;
            }
            
            if (bValue)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                if (CommonDefine.waferFileInfo.IsMarkContentEmpty(0, 0) && CommonDefine.waferFileInfo.IsMarkContentEmpty(0, 1))
                {
                    string strInfo = "没有打标内容，请检查料仓配置！";
                    AddListViewThread(null, strInfo, Color.Red);
                    MessageBox.Show(strInfo); ;
                    return bRet;
                }

                OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
                GC.Collect();                        

                m_nWorkState = AxisTestForm.WORK_AUTO;
                return false;
            }

            if (m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                return bRet;
            }
    
            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule00.slaveNO, GLinkIOModule00.ResetIOIN);
            if (ob != null)
            {
                bValue = (Boolean)ob;
            }

            if (m_nWorkState != AxisTestForm.WORK_RESET)
            {
                
                if (bValue)
                {
                    OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
                    ResetWork();
                    return false;
                }
            }
       
            return bRet;

        }

        public  void ClearRobotError()
        {                
            if (IsRobotWarning())
            {
                mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetClearString());
                Thread.Sleep(500);               
            }           
        }

        /// <summary>
        /// 机械手回原点
        /// </summary>
        public void ResetWork()
        {
            ClearRobotError();

            mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetICommString( JelRobotOperate.A_MOVETO_ORIG) );

            int nMaxSleep = 8000;
            int nStartTick = Environment.TickCount;

            while (IsLAarmOrig() == false || IsRAarmOrig() == false  )
            {
                Thread.Sleep(500);

                int dt = CommonDefine.DelayedTime(nStartTick);
                if (dt > nMaxSleep && mainForm.chipAutoForm.IsRobotRuning() == false)
                    break;
            }
        }

        internal Boolean InitialRobot()
        {
            Boolean bRet = true;

            try
            {
                ClearRobotError();
                Thread.Sleep(1000);


                int nMaxSleep = 20000;
                int nStartTick = Environment.TickCount;

                mainForm.robotSerialPortForm.EmptyReceiveBuffer();
                mainForm.robotSerialPortForm.serialPort.Synchronous = true;
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.GetICommString(JelRobotOperate.MOVETO_ORIG));
                Thread.Sleep(1000);

                while (mainForm.robotSerialPortForm.GetLastResponseForRobot(false).Length <= 0)
                {
                    mainForm.robotSerialPortForm.serialPort.synReadAllData();
                    int dt = CommonDefine.DelayedTime(nStartTick);
                    if (dt > nMaxSleep && IsRobotRuning() == false)
                    {
                        bRet = false;
                        break;
                    }
                        
                }

                mainForm.robotSerialPortForm.serialPort.Synchronous = false;

              
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        public void RefreshBoxLight()
        {
            Boolean bRet = false;
            object ob = null;

            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif1Place_IN);
                
            Boolean isExit1 = false;
            if (ob != null)
            {
                isExit1 = (Boolean)ob;
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit1_BX20);
            //Boolean isExit2 = false;
            //if (ob != null)
            //{
            //    isExit2 = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit1_C);
            //Boolean isExit3 = false;
            //if (ob != null)
            //{
            //    isExit3 = (Boolean)ob;
            //}

            if (isExit1 == false)
            {
               // mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, false);
                RemoveWorkIndex(0);
            }

            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif2Place_IN);
           // ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence2);

            isExit1 = false;
            if (ob != null)
            {
                isExit1 = (Boolean)ob;
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit2_BX22);
            //isExit2 = false;
            //if (ob != null)
            //{
            //    isExit2 = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit2_C);
            //isExit3 = false;
            //if (ob != null)
            //{
            //    isExit3 = (Boolean)ob;
            //}

            if (isExit1 == false)
            {
                //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, false);
                RemoveWorkIndex(1);
            }                        
            
            for(int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++ )
            {
                if(m_WorkReadyBoxList.m_BoxList[i].X == 0 )
                {
                    if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_WAITSTATE || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNSTATE
                    || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNNINGSTATE)
                    {
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, true);
                    }
                    else if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_FINISHSTATE)
                    {                        
                        int dt = MyCommanLib.CommonDefine.DelayedTime(m_nStartLightTick);
                        int ddt = dt / 1000;
                        if( ddt%2 == 0 )
                        {
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, true);
                        }else
                        {
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, false);
                        }
                        
                        //MyCommanLib.CommonDefine.TickSleep(1000);
                      
                        //MyCommanLib.CommonDefine.TickSleep(1000);
                    }
                    else
                    {                        
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, false);
                    }
                }
                

                if(m_WorkReadyBoxList.m_BoxList[i].X == 1 )
                {
                    if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_WAITSTATE || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNSTATE
                    || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNNINGSTATE)
                    {
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, true);
                    }
                    else if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_FINISHSTATE)
                    {
                        int dt = MyCommanLib.CommonDefine.DelayedTime(m_nStartLightTick);
                        int ddt = dt / 1000;
                        if (ddt % 2 == 0)
                        {
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, true);
                        }
                        else
                        {
                            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, false);
                        }

                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, true);
                        ////MyCommanLib.CommonDefine.TickSleep(1000);
                        //System.Threading.Thread.Sleep(1000);
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, false);
                        ////MyCommanLib.CommonDefine.TickSleep(1000);
                        //System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, false);
                    }
                }
                

            }
        }
        public Boolean IsRobotWarning()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotRunningNG_IN);
            Boolean bReady = (Boolean)ob;
            return bReady;
        }

        public Boolean IsRobotRuning()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotRunning_IN);
            Boolean bReady = (Boolean)ob;
            return bReady;
        }

        public Boolean IsLAarmOrig()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotDnArmOrig_IN);
            Boolean bReady = (Boolean)ob;
            return bReady;
        }

        public Boolean IsRAarmOrig()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotUpArmOrig_IN);
            Boolean bReady = (Boolean)ob;
            return bReady;
        }
              
        public Boolean IsRAarmXifu()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotRArmXifu_IN);
            Boolean bReady = (Boolean)ob;

            return bReady;
        }

        public Boolean IsLAarmXifu()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.RobotLArmXifu_IN);
            Boolean bReady = (Boolean)ob;

            return bReady;
        }

        public int WaitRobotResult()
        {
            int nType = 1;//0:NG 1:OK  2:超时
                          //等待结束
           
            return nType;
        }

        public Boolean IsMaterialExit(int nBoxIndex)
        {
            Boolean bExit = true;
            object ob1= null,ob2 = null,ob3 = null;

            Boolean bExit1=false, bExit2=false,bExit3 = false;
            if( nBoxIndex == 0)
            {
                //ob1 = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence1);
                ob1 = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif1Place_IN);
                
            }
            else if( nBoxIndex == 1)
            {
                //ob1 = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence2);
                ob1 = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif2Place_IN);
                //ob2 = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.BoxExist2B);
                //ob3 = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit2_C);
            }
            

            if (ob1 != null)
            {
                bExit1 = (Boolean)ob1;
            }

            if (ob2 != null)
            {
                bExit2 = (Boolean)ob2;
            }

            if (ob3 != null)
            {
                bExit3 = (Boolean)ob3;
            }

            if (bExit1 == false && bExit2 == false /*&& bExit3 == false*/)
            {
                bExit = false;
            }
            return bExit;
        }

        public Boolean IsRAarmXiLiao()
        {
            Boolean bReady = IsRAarmXifu();
                        
            if( bReady == false )
            {
                mainForm.robotSerialPortForm.OperateAction( JelRobotOperate.EXCUTE_VACUUMUPOPEN );
           
                Thread.Sleep(500);
                if (IsRAarmXifu())
                {
                    bReady = true;
                }
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMUPCLOSE);
            }

            return bReady;
        }

        public Boolean IsLAarmXiLiao()
        {
            Boolean bReady = IsLAarmXifu();

            if (bReady == false)
            {
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMDNOPEN);

                Thread.Sleep(500);
                if (IsLAarmXifu())
                {
                    bReady = true;
                }
                mainForm.robotSerialPortForm.OperateAction(JelRobotOperate.EXCUTE_VACUUMDNCLOSE);
            }

            return bReady;
        }

        public Boolean IsXunbianExitWafer()
        {
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.AlignerExitMaterial_IN);
            Boolean bReady = (Boolean)ob;

            return bReady;
        }
        public int RemoveWorkIndex(int nBoxIndex)
        {
            int nRet = 0;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (m_WorkReadyBoxList.m_BoxList[i].X == nBoxIndex)
                {
                    m_WorkReadyBoxList.m_BoxList.RemoveAt(i);

                    AddListViewThread(null, $"料盒{nBoxIndex + 1}已移走", Color.Blue);
                    nRet = nBoxIndex;

                    if (mainForm.IsEAPRunMode())
                    {
                        mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupID, nBoxIndex.ToString());
                        mainForm.secsForm.SetEventPos(SECSEventData.FoupRemoved);
                    }

                    break;
                }
            }

            //刷新
            ResetChipMarkData(nBoxIndex);
            return nRet;
        }

        public int FindFirstWorkIndex()
        {
            int nListIndex = -1;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {               
                if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_WAITSTATE || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNSTATE
                    || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNNINGSTATE)
                {
                    nListIndex = i;
                    break;
                }
            }
            return nListIndex;
        }

        public int FindWorkIndexByBoxIndex( int nBoxIndex )
        {
            int nListIndex = -1;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (m_WorkReadyBoxList.m_BoxList[i].X == nBoxIndex)
                {
                    nListIndex = i;
                    break;
                }
            }
            return nListIndex;
        }

        public void AddBoxIndex2ReadyList(int nBoxIndex)
        {
            Boolean bFind = false;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (nBoxIndex == m_WorkReadyBoxList.m_BoxList[i].X)
                {
                    bFind = true;
                    break;
                }
            }

            if (bFind == false)
            {
                Point pp = new Point(nBoxIndex, AxisTestForm.BOXCHIP_WAITSTATE);
                m_WorkReadyBoxList.m_BoxList.Add(pp);

                AddListViewThread(null, $"料盒{nBoxIndex+1}已放好", Color.Blue);

                if (mainForm.IsEAPRunMode())
                {
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.FoupID, nBoxIndex.ToString());
                    mainForm.secsForm.SetEventPos(SECSEventData.FoupArrived);
                }
            }
        }

        //确认按钮===============================
        public Boolean OperateReadyMaterials()
        {
            Boolean bRet = false;           

           
            //object ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxConfirm1X10);

            //Boolean isConfirm = false;
            //if( ob != null )
            //{
            //    isConfirm = (Boolean)ob;
            //}

            //object ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence1);
            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif1Place_IN);

            Boolean isExit1 = false;
            if (ob != null)
            {
                isExit1 = (Boolean)ob;
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit1_BX20);

            //Boolean isExit2 = false;
            //if (ob != null)
            //{
            //    isExit2 = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit1_C);

            //Boolean isExit3 = false;
            //if (ob != null)
            //{
            //    isExit3 = (Boolean)ob;
            //}


            //if (isConfirm  && (isExit1 || isExit2 || isExit3 ) )
            if(isExit1)
            {
                AddBoxIndex2ReadyList(0);
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxConfirm2X12);

            //isConfirm = false;
            //if (ob != null)
            //{
            //    isConfirm = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence2);
            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif2Place_IN);

            isExit1 = false;
            if (ob != null)
            {
                isExit1 = (Boolean)ob;
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit2_BX22);

            //isExit2 = false;
            //if (ob != null)
            //{
            //    isExit2 = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit2_C);

            //isExit3 = false;
            //if (ob != null)
            //{
            //    isExit3 = (Boolean)ob;
            //}

            //if (isConfirm && (isExit1 || isExit2 || isExit3 ) )
            if(isExit1)
            {
                AddBoxIndex2ReadyList(1);
            }

            return bRet;
        }

        //ui checkbox
        public Boolean OperateReadyMaterialsManual( int nIndex )
        {
            Boolean bRet = false;


            object ob = null;
            Boolean isExit1 = false;
            Boolean isExit2 = false;
            Boolean isExit3 = false;

            if ( nIndex == 0 )
            {
                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence1);
                ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif1Place_IN);

                if (ob != null)
                {
                    isExit1 = (Boolean)ob;
                }

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit1_BX20);

                //if (ob != null)
                //{
                //    isExit2 = (Boolean)ob;
                //}

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit1_C);

                //if (ob != null)
                //{
                //    isExit3 = (Boolean)ob;
                //}

            }
            else
            {
                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence2);

                ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif2Place_IN);
                if (ob != null)
                {
                    isExit1 = (Boolean)ob;
                }

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit2_BX22);

                //if (ob != null)
                //{
                //    isExit2 = (Boolean)ob;
                //}

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit2_C);

                //if (ob != null)
                //{
                //    isExit3 = (Boolean)ob;
                //}
            }
                       

            if (isExit1)
            {
                AddBoxIndex2ReadyList(nIndex);
            }else
            {

            }
                        
            return bRet;
        }

        public void SetThreadState(Control control, Boolean bEnable)
        {
            if (control.IsDisposed)
            {
                return;
            }
            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            //if (ob is TextBox)
            {
                if (control.InvokeRequired)
                {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                    try
                    {
                        SetStateCallback d = new SetStateCallback(SetThreadState);
                        this.Invoke(d, new object[] { control, bEnable });
                    }
                    catch
                    {
                    }
                }
                else
                {
                    control.Enabled = bEnable;
                }
            }
        }

        public void OperateControlState(Boolean bRunning)
        {
            Boolean bEnable = !bRunning;          
            {
                SetThreadState(buttonChipsSetting, !bRunning);

                SetThreadState(buttonInitial, !bRunning);
                SetThreadState(buttonRun, !bRunning);
                SetThreadState(buttonClear, !bRunning);
                SetThreadState(tb_CycleNumber, !bRunning);
                SetThreadState(ckb_cycleMode, !bRunning);
            }
        }

        public void RefreshUIState()
        {
            try
            {               
                ReflashWorkState();              
            }
            catch (Exception)
            {
            }

            try
            {
               OperateWorkState();               
            }
            catch (Exception)
            {
            }
        }

        public void ReflashWorkState()
        {
            short nWorkState = 0;
            nWorkState = m_nWorkState;

            Color bkColor = Color.FromArgb(0);
            // 0 : 初始化中  1： 待机  2：运行 3：故障
            if (nWorkState == AxisTestForm.WORK_AUTO)
            {
                SetThreadText(workStateState, bkColor, "运行");
                SetThreadBackColor(workStateState, PLCOpcFeedParameters.runColor);
                SetThreadText(warningInfoState, PLCOpcFeedParameters.runColor, "运行中......");

                this.OperateControlState(true);
                if (m_nPreWorkState != nWorkState)
                {
                    m_nPreWorkState = nWorkState;
                    string strState = "2";

                   mainForm.secsForm.SetValue(SECSWorkFlowParam.ProcessState, strState);
                    mainForm.secsForm.SetEventPos(SECSEventData.PorcessStateChanged); //机器状态变化
                }
            }
            else if (nWorkState == AxisTestForm.WORK_Intial)
            {
                SetThreadText(workStateState, bkColor, "初始化中");
                SetThreadBackColor(workStateState, PLCOpcFeedParameters.pauseColor);

                SetThreadText(warningInfoState, PLCOpcFeedParameters.pauseColor, "初始化中......");

                this.OperateControlState(false);

                if (m_nPreWorkState != nWorkState)
                {
                    m_nPreWorkState = nWorkState;
                    string strState = "0";
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.ProcessState, strState);
                    mainForm.secsForm.SetEventPos(SECSEventData.PorcessStateChanged); //机器状态变化
                }
            }
            else if (nWorkState == AxisTestForm.WORK_WARNING)
            {
                SetThreadText(workStateState, bkColor, "报警");
                SetThreadBackColor(workStateState, PLCOpcFeedParameters.warningColor);

                SetThreadText(warningInfoState, PLCOpcFeedParameters.warningColor, "报警中......");
                this.OperateControlState(false);

                if (m_nPreWorkState != nWorkState)
                {
                    m_nPreWorkState = nWorkState;
                    string strState = "3";
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.ProcessState, strState);

                    mainForm.secsForm.SetEventPos(SECSEventData.PorcessStateChanged); //机器状态变化
                }
            }
            else
            {
                SetThreadText(workStateState, bkColor, "待机");
                SetThreadBackColor(workStateState, PLCOpcFeedParameters.pauseColor);

                SetThreadText(warningInfoState, PLCOpcFeedParameters.pauseColor, "待机......");

                this.OperateControlState(false);

                //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.TowerYellowLight, true);

                if (m_nPreWorkState != nWorkState)
                {
                    m_nPreWorkState = nWorkState;
                    string strState = "1";
                    mainForm.secsForm.SetValue(SECSWorkFlowParam.ProcessState, strState);

                    mainForm.secsForm.SetEventPos(SECSEventData.PorcessStateChanged); //机器状态变化
                }
            }
            //end work state =======================================            
        }

        //ui checkbox false
        public void RefreshBoxLightManual(int nIndex )
        {
            Boolean bRet = false;
            object ob = null;

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence1);
            ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif1Place_IN);
            Boolean isExit1 = false;
            if (ob != null)
            {
                isExit1 = (Boolean)ob;
            }

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit1_BX20);
            //Boolean isExit2 = false;
            //if (ob != null)
            //{
            //    isExit2 = (Boolean)ob;
            //}

            //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit1_C);
            //Boolean isExit3 = false;
            //if (ob != null)
            //{
            //    isExit3 = (Boolean)ob;
            //}

            if( nIndex != 0 )
            {
                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.SmifPresence2);
                ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule01.Smif2Place_IN);

                isExit1 = false;
                if (ob != null)
                {
                    isExit1 = (Boolean)ob;
                }

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOInState.MBoxExit2_BX22);
                //isExit2 = false;
                //if (ob != null)
                //{
                //    isExit2 = (Boolean)ob;
                //}

                //ob = mainForm.opcOperate.GetItemValue(PLCOpcFeedParameters.IOOutState.MBoxExit2_C);
                //isExit3 = false;
                //if (ob != null)
                //{
                //    isExit3 = (Boolean)ob;
                //}
            }

            if (isExit1 == false)
            {
                if( nIndex == 0 )
                {
                    //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight1, false);
                }else
                {
                    //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.IOOutState.MBoxconfirmLight2, false);
                }
                
                RemoveWorkIndex(nIndex);
            }

        }

        //
        public Boolean TakePhoto(int nPhotoType )
        {
            Boolean bRet = true;

            MyCommanLib.CommonDefine.InitialMarkStruct(ref MyCommanLib.CommonDefine.m_PosStruct);
            MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoStart = nPhotoType;
            //double fx = mainForm.mOpcAxisOperater.GetCurrentPosX();
            //double fy = mainForm.mOpcAxisOperater.GetCurrentPosY();

            //MyCommanLib.CommonDefine.m_PosStruct.CurPosX = fx;
            //MyCommanLib.CommonDefine.m_PosStruct.CurPosY = fy;

            MyCommanLib.CommonDefine.WriteMemeryFile(MyCommanLib.CommonDefine.m_posMemeryFile, MyCommanLib.CommonDefine.m_PosStruct);

            int nStartTickCount = System.Environment.TickCount;

            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.LightVacuum, true);

            int dt = 0;
            while (MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd == 0)
            {
                MyCommanLib.CommonDefine.ReadMemeryFile(MyCommanLib.CommonDefine.m_posMemeryFile, ref MyCommanLib.CommonDefine.m_PosStruct);

                dt = CommonDefine.DelayedTime(nStartTickCount);

                if (dt > 10000)
                {
                    break;
                }
            }


            if (MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd != 0)
            {
                if (MyCommanLib.CommonDefine.m_PosStruct.m_nResult != 0)
                {
                    bRet = true;
                }
                else
                {
                    bRet = false;
                }
            }
            else
            {
                bRet = false;
            }

            //mainForm.opcOperate.WriteItemValue(PLCOpcFeedParameters.XINGAN_IOOutState.LightVacuum, false);

            //屏蔽检测结果
            //return true;

            return bRet;
        }

        public string TriggerPhoto()
        {
            mainForm.cameraForm.receiveString = "";
            mainForm.cameraForm.m_bReceiveFinished = false;

            string cmd = "T2\r";
            mainForm.cameraForm.clientWrite(cmd);

            MyCommanLib.CommonDefine.TickSleep(500);

            int nStartTickCount = System.Environment.TickCount;
            int maxDt = 50000;
            int dt = 0;

            while (mainForm.cameraForm.m_bReceiveFinished == false)
            {
                dt = CommonDefine.DelayedTime(nStartTickCount);
                if (dt > maxDt)
                {
                    break;
                }
                Thread.Sleep(300);
            }

            return mainForm.cameraForm.receiveString;
        }

        public Boolean OCRScan(int nType)
        {
            Boolean bRet = true;

            string strInfo = "OCR 扫码识别开始";
            TCPForm tcpForm = mainForm.cameraForm;
            if( nType == 0 )
            {
                strInfo = "OCR 扫码检测开始";
            }            

            CommonDefine.m_PosStruct.strContent = "";
           
            tcpForm.m_bReceiveFinished = false;
            tcpForm.receiveString = "";
            tcpForm.SendOCRCommand(0);

            AddListViewThread(null, strInfo, Color.Blue);

            int nStartTickCount = Environment.TickCount;
            int dt = 0;
            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                if (dt > 20000)
                {
                    break;
                }
                Thread.Sleep(100);
            }

            if (tcpForm.m_bReceiveFinished)
            {
                string strText = tcpForm.receiveString.Trim('\n');
                string strResult = strText.Trim('\r');
                string[] strArray = strResult.Split(',');

                if (strResult.Contains("*") || strResult.Contains("?"))
                {
                    strInfo = $"接收检测数据完成--NG:{strResult}";
                    if (nType > 0)
                    {
                        strInfo = $"接收OCR识别数据完成--NG:{strResult}";
                    }
                    bRet = false;
                }
                else
                {
                    strInfo = $"接收检测数据完成--OK:{strResult}";
                    if (nType > 0)
                    {
                        strInfo = $"接收OCR识别数据完成--OK:{strResult}";
                    }
                }

                if (nType == 0)
                {
                    //N049D079XYA6,390.000,1.000,0.076
                    CommonDefine.m_PosStruct.strContent = strArray[0];
                    if( strArray.Length > 3 )
                    {
                        if( strArray[2].Contains("1.0") == false )
                        {
                            strInfo = $"检测数据判断结果NG：{strResult}";
                            bRet = false;
                        }
                    }
                }
                else
                {                   
                    CommonDefine.m_PosStruct.strContent = strArray[0].Trim('[');
                }
                AddListViewThread(null, strInfo, Color.Blue);
                tcpForm.m_bSendCommand = true;
            }
            else
            {
                strInfo = "等待检测数据超时";
                if (nType > 0)
                {
                    strInfo = "等待OCR识别数据超时";
                }
                tcpForm.m_bSendCommand = true;
                bRet = false;
            }
            return bRet;
        }


        public Boolean OCRSwRecipeScan(int nType)
        {
            Boolean bRet = true;

            string strInfo = "OCR 切换配方开始";
            TCPForm tcpForm = mainForm.ocrSwitchRecipeForm;
            
            CommonDefine.m_PosStruct.strContent = "";

            tcpForm.m_bReceiveFinished = false;
            tcpForm.receiveString = "";

            string strCommand = "LF";
            tcpForm.SendOCRCommand(0);

            AddListViewThread(null, strInfo, Color.Blue);

            int nStartTickCount = Environment.TickCount;
            int dt = 0;
            while (tcpForm.m_bReceiveFinished == false)
            {
                dt = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                if (dt > 20000)
                {
                    break;
                }
                Thread.Sleep(100);
            }

            if (tcpForm.m_bReceiveFinished)
            {
                string strText = tcpForm.receiveString.Trim('\n');
                string strResult = strText.Trim('\r');
                string[] strArray = strResult.Split(',');

                if (strResult.Contains("*") || strResult.Contains("?"))
                {
                    strInfo = $"接收检测数据完成--NG:{strResult}";
                    if (nType > 0)
                    {
                        strInfo = $"接收OCR识别数据完成--NG:{strResult}";
                    }
                    bRet = false;
                }
                else
                {
                    strInfo = $"接收检测数据完成--OK:{strResult}";
                    if (nType > 0)
                    {
                        strInfo = $"接收OCR识别数据完成--OK:{strResult}";
                    }
                }

                if (nType == 0)
                {
                    //N049D079XYA6,390.000,1.000,0.076
                    CommonDefine.m_PosStruct.strContent = strArray[0];
                    if (strArray.Length > 3)
                    {
                        if (strArray[2].Contains("1.0") == false)
                        {
                            strInfo = $"检测数据判断结果NG：{strResult}";
                            bRet = false;
                        }
                    }
                }
                else
                {
                    CommonDefine.m_PosStruct.strContent = strArray[0].Trim('[');
                }
                AddListViewThread(null, strInfo, Color.Blue);
                tcpForm.m_bSendCommand = true;
            }
            else
            {
                strInfo = "等待检测数据超时";
                if (nType > 0)
                {
                    strInfo = "等待OCR识别数据超时";
                }
                tcpForm.m_bSendCommand = true;
                bRet = false;
            }
            return bRet;
        }

        public Boolean CheckMarkContent( int nLaserType ,int nBoxIndex , int nLayerIndex )
        {
            Boolean bCheckResult = true;
            string strOCRSrc = CommonDefine.m_PosStruct.strContent;
            string strMarkSrc = CommonDefine.waferFileInfo.GetMarkContent(nLaserType, nBoxIndex, nLayerIndex);
            string strOCR = strOCRSrc.Replace(" ","");//strOCRSrc.Trim(); 
            string strMark = strMarkSrc.Replace(" ", "");//strMarkSrc.Trim();

          
            string strNewMark = strMark;
            int nCodeLen = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].sampleCode.Length;
            if (CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].nEncriptType == 1)
            {
                nCodeLen = nCodeLen + 2;
            }

            if ( strOCR.CompareTo(strNewMark) != 0 || strMark.Length != nCodeLen)
            {
                bCheckResult = false;
            }

            return bCheckResult;
        }

        public Boolean CheckMarkContentLength(int nLaserType, string srcString)
        {
            Boolean bCheckResult = true;
         
            string strMark = srcString;
            int nCodeLen = CommonDefine.waferFileInfo.AllLaserMarkContent[nLaserType].sampleCode.Length;
            
            if (strMark.Length != nCodeLen )
            {
                bCheckResult = false;
            }
            return bCheckResult;
        }

        public bool IsNormalWarning(Boolean bShowInfo = true)
        {
            if (m_bTestFlow)
            {
                return false;
            }

            Boolean bEmStop = false;
            bEmStop = mainForm.gtsForm.m_gtsControl.IsNormalWarning();

            if (bEmStop && bShowInfo)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                string strInfo = "机械手运行出错,请复位后，再开始";

                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                warnForm.ShowDialog();
                warnForm = null;
            }

            return bEmStop;
        }

        private Boolean IsRobotEmStop( Boolean bShowInfo = true)
        {            
            Boolean bEmStop = true;//需要改回去

            object ob = mainForm.gtsForm.m_gtsControl.GetDiValue(GLinkIOModule01.slaveNO, GLinkIOModule00.EMStopIOIN);
            Boolean bRunning = (Boolean)ob;

            if (bEmStop == false &&  bShowInfo )
            {
                string strInfo = "机械手急停,请检查安全后，按复位";

                WarningForm warnForm = new WarningForm(mainForm);
                warnForm.SetInfoText(strInfo);
                warnForm.ShowDialog();
                warnForm = null;
            }
            return !bEmStop;
        }

        public void OperateStart()
        {
            if (m_bEAPLockMarkContent)
            {
                //string strinfo = "没有打标内容，请等待eap发送打标内容！";
                //AddListViewThread(null, strinfo, Color.Red);
                //MessageBox.Show(strinfo); ;
                //return;
            }

            if (m_nWorkState == AxisTestForm.WORK_AUTO)
                return;

           
            OperateClearLight( GLinkIOModule00.GreenLightIOOUT);
          

            string strinfo = "EAP---收到START指令";
            AddListViewThread(null, strinfo, Color.Green);

            //m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
            m_nRobotFlow = AxisTestForm.EAP_SCAN_VALUE;
            m_nWorkState = AxisTestForm.WORK_AUTO;
        }

        public void RemoteLoadFoup()
        {
            
            OperateClearLight(GLinkIOModule00.GreenLightIOOUT);


            string strinfo = "EAP---收到LoadFoup指令";
            AddListViewThread(null, strinfo, Color.Green);

            m_nRobotFlow = AxisTestForm.SCAN_VALUE;
            m_nWorkState = AxisTestForm.WORK_AUTO;
        }

        public void RemoteCancle()
        {
            string strinfo = "EAP---收到Cancel指令";
            AddListViewThread(null, strinfo, Color.Green);

            //if (m_nWorkBoxIndex == AxisTestForm.WORK_AUTO)
            //    return;

            m_nWorkState = AxisTestForm.WORK_MANUAL;

            CloseSmif();
            CommonDefine.bAccept = true;
        }

        public void RemoteStop()
        {
            m_nWorkState = AxisTestForm.WORK_MANUAL;

            string strinfo = "EAP---收到STOP指令";
            AddListViewThread(null, strinfo, Color.Green);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                return;
            }

            if( mainForm.IsEAPRunMode() )
            {
                OperateStart();
            }
            else
            {
                //if (CommonDefine.waferFileInfo.IsMarkContentEmpty(0, 0) && CommonDefine.waferFileInfo.IsMarkContentEmpty(0, 1))
                //{
                //    string strInfo = "没有打标内容，请检查料仓配置！";
                //    AddListViewThread(null, strInfo, Color.Red);
                //    MessageBox.Show(strInfo); ;
                //    return;
                //}

                //CuttingForm parentForm = this.mainForm;
                //parentForm.gtsForm.Start();
                m_nWorkState = AxisTestForm.WORK_AUTO;
                OperateClearLight(GLinkIOModule00.GreenLightIOOUT);
               
            }

            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {            
            //CuttingForm parentForm = this.mainForm;
            //parentForm.gtsForm.Stop();
            m_nWorkState = AxisTestForm.WORK_MANUAL;
            OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
            
        }

        private void buttonChipsSetting_Click(object sender, EventArgs e)
        {
            if( m_nWorkState == AxisTestForm.WORK_AUTO )
            {
                return;
            }
            
            m_chipSettingForm.markForm = this.mainForm.markForm;
            m_chipSettingForm.chipAutoForm = this;
            m_chipSettingForm.AddEventHandler();                       

            m_chipSettingForm.markForm.OperateButtonState(false);                 

            m_chipSettingForm.ShowDialog();
            if( m_chipSettingForm.DialogResult == DialogResult.OK)
            {
                m_chipSettingForm.markForm.WritePrivateIni();

                RefreshUiInfo();
            }
            else
            {

            }
            
            m_nWorkTestMode = m_chipSettingForm.markForm.m_bTestMode;

            RefreshAllChipsUI(null,null);
                        
        }

        public void  RefreshUiInfo()
        {
            int nWaferSize = mainForm.markForm.m_nWaferSize;
            if (nWaferSize == 0)
            {
                labelWaferSize.Text = "4 Inch";
            }
            else if (nWaferSize == 1)
            {
                labelWaferSize.Text = "6 Inch";
            }
            else
            {
                labelWaferSize.Text = "8 Inch";
            }
            //处理
            int nMarkType = CommonDefine.waferFileInfo.nMarkType;
           if(nMarkType == 2)
            {
                labelWaferType.Text = "SiC打标";
            }
            else if (nMarkType == 1)
            {
                labelWaferType.Text = "Sic打标";
            }else
            {
                labelWaferType.Text = "无打标";
            }

            int nProductIndex = CommonDefine.waferFileInfo.nProductIndex;            

            if ( nProductIndex >= 0 )
            {
                string strProductName = CommonDefine.productHelper.m_ProductList[nProductIndex];
                labelProductName.Text = strProductName;
            }else
            {
                labelProductName.Text = "非法产品";
            }
        }
        private void labelBox1_21_Click(object sender, EventArgs e)
        {

        }

        private void ChipAutoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        private void RefreshAllChipsUI(object sender, EventArgs e)
        {
            RefreshUI();

            for (int i = 0; i < AxisTestForm.BOX_COUNT; i++)
            {
                for (int j = 0; j < AxisTestForm.BOX_CHIPSCOUNT; j++)
                {
                    if (m_MarkChipsData[i, j].bSelect == false)
                    {
                        if (boxMarkContent[i, j].BackColor == Color.Green)
                        {
                            boxMarkContent[i, j].BackColor = Color.Cyan;
                        }
                    }
                    else
                    {

                    }
                }

            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {           
            if (m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                return;
            }

            m_nWorkState = AxisTestForm.WORK_MANUAL;
            mainForm.robotSerialPortForm.serialPort.Write(JelRobotOperate.GetClearString());
            OperateAlignerHelper.Reset(mainForm.waferSerialPortForm);
            ResetSmif(0);
            ResetSmif(1);
            OperateClearLight(GLinkIOModule00.YellowLightIOOUT);
           

            if (mainForm.IsEAPRunMode())
            {
                CommonDefine.bAccept = true;
            }
        }

        private void ChipAutoForm_Load(object sender, EventArgs e)
        {
            
        }

        public void ReadWorkCount()
        {
            string strText = mainForm.ReadCuttingIniValue("WorkCount");
            labelWorkCount.Text = strText;
        }

        private void buttonZero_Click(object sender, EventArgs e)
        {
            labelWorkCount.Text = "0";
            //WorkCountAdd();
        }

        public void CreateBoxPatchData(int nBoxIndex )
        {
            if (m_WorkChipList.m_LayerList.Count > 0)
            {
                MessageBox.Show("当前工作数据没有完成，当前料盒不可生成补打数据");
                return;
            }

            //生成补打工作内容            
            m_WorkChipList.m_nBoxIndex = nBoxIndex;
            for (int i = 0; i < AxisTestForm.BOX_CHIPSCOUNT; i++)
            {
                if (boxMarkContent[nBoxIndex, i].BackColor == Color.Red)
                {
                    //等到扫描结果
                    Point pp = new Point(i, AxisTestForm.BOXCHIP_WAITSTATE);
                    m_WorkChipList.m_LayerList.Add(pp);
                }
            }

            if (m_WorkChipList.m_LayerList.Count > 0)
            {
                MessageBox.Show("当前料盒有补打数据，按开始可补打");
                m_nRobotFlow = AxisTestForm.ROBOT_TAKE;
            }
            else
            {
                MessageBox.Show("当前料盒无失败数据，不需补打");
            }

        }
   
        private void buttonPatch1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm(CommonDefine.USER_TP);
            loginForm.ShowDialog();
            if (loginForm.DialogResult == DialogResult.OK)
            {
                CreateBoxPatchData(0);
            }            
        }

        private void buttonPach2_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm(CommonDefine.USER_TP);
            loginForm.ShowDialog();
            if (loginForm.DialogResult == DialogResult.OK)
            {
                CreateBoxPatchData(1);
            }
        }

        private void checkBoxReady1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxReady1.Checked)
            {
                checkBoxReady1.BackColor = Color.Green;
                OperateReadyMaterialsManual(0);
            }
            else
            {
                checkBoxReady1.BackColor = Color.LightGray;
                RefreshBoxLightManual(0);
            }
        }

        private void checkBoxReady2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxReady2.Checked)
            {
                checkBoxReady2.BackColor = Color.Green;
                OperateReadyMaterialsManual(1);
            }
            else
            {
                checkBoxReady2.BackColor = Color.LightGray;
                RefreshBoxLightManual(1);
            }
        }

        public void SetWorkInfo(string strInfo)
        {
            SetThreadText(labelEAPWorkInfo, Color.Blue, strInfo);
        }

        private void buttonInitial_Click(object sender, EventArgs e)
        {
            if (m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                return;
            }

            if (MessageBox.Show("确认设备轴要回原点，确认安全后，按Yes 取消按No", "系统", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                mainForm.ThreadInitialDevice();

            }            
        }

        private void listViewWorkLogs_SizeChanged(object sender, EventArgs e)
        {
            int width = listViewWorkLogs.Width - 5;
            listViewWorkLogs.Columns[1].Width = width - listViewWorkLogs.Columns[0].Width;
        }

        public Boolean IsSmifExit(int nBoxIndex)
        {
            Boolean bExit = true;

            return bExit;
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            ScanForm scanForm = new ScanForm();
            scanForm.ShowDialog();
            if( scanForm.DialogResult == DialogResult.OK)
            {
                //mainForm.secsForm.SetValue(SECSWorkFlowParam.ScanConternt, scanForm.m_strFeedCastleID);
                //mainForm.secsForm.SetValue(SECSWorkFlowParam.ScanConterntCST, scanForm.m_strCutCastleID);
                //mainForm.secsForm.SetValue(SECSWorkFlowParam.ScanConterntPOD, scanForm.m_strSmifPodID);
                //string strValue = (scanForm.m_nBoxIndex + 1).ToString();
                //mainForm.secsForm.SetValue(SECSWorkFlowParam.StartBoxNumb,strValue );

                //mainForm.secsForm.SetEventPos(SECSEventData.SendLotInfo);

                //mainForm.m_nEapBoxIndex = scanForm.m_nBoxIndex;
            }
        }

        private void btn_CheckLog_Click(object sender, EventArgs e)
        {
            string LogPath = System.Windows.Forms.Application.StartupPath + @"\WorkLogs";
            System.Diagnostics.Process.Start("explorer.exe", LogPath);
        }

        private void ckb_CameraEnble_Click(object sender, EventArgs e)
        {
            //LoginForm loginForm = new LoginForm(CommonDefine.USER_TP);
            //loginForm.ShowDialog();
            //if (loginForm.DialogResult != DialogResult.OK)
            //{
            //    ckb_CameraEnble.Checked = !ckb_CameraEnble.Checked;
            //}

            frm_register.mainUser = mainForm.frm_user;
            frm_register frm_Register1 = new frm_register();
            if (frm_Register1.ShowDialog() != DialogResult.OK)
            {
                ckb_CameraEnble.Checked = !ckb_CameraEnble.Checked;
            }
        }
    }
}
