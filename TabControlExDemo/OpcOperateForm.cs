using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPCAutomation;

using System.IO;

using OpcDa.Client;




namespace TabControlExDemo
{
    public partial class OpcOperateForm : Form
    {
        /// <summary>
        /// plc 信号状态变化
        /// </summary>
        public static int NoChange = 0;
        public static int RisingEdge = 1;
        public static int FallingEdge = 2;

        private OpcServer _opcServer;
        private IRequest _request = null;
        private int _handle = 1;

        string ServerAppName = "Kepware.KEPServerEx.V6";
        string ServerAppIP = "127.0.0.1";
        string updateRate = "100";
        string GroupName = "RobotWafer.RobotPLC";//"RobotTCP.RobotTCP1";//opc client 监视时，opcclient的group名称要和这个名称一致。


        List<string> opcNameList = new List<string>();//opc plc数据列表
        List<object> opcPreValueList = new List<object>();//opc 数据前一个值

        //同步opc服务 ip地址要和opc设置一致
        private object syncLock = new object();
        private bool m_bSynServer = true;
        private OPCServer _synOpcServer;
        private bool m_bSynServerConnected = false;
        OPCGroups mygroups;   //OPCGroups是“Server”下面最大概念。它是OPCGroup的集合。
        OPCGroup mygroup;
        OPCItem []myitemArray = new OPCItem[500];
       
        public ItemResult[] m_Items;
        public object[] m_Values;

        public OpcOperateForm()
        {
            InitializeComponent();
        }

        // step 1   设置初始数据
        public void SetOpcBaseInfo(string opcServerAppName, string opcServerAppIP, string opcUpdateRate = "100")
        {
            ServerAppName = opcServerAppName;
            ServerAppIP = opcServerAppIP;
            updateRate = opcUpdateRate;
        }

        public void SetOpcGroupName( string grpName)
        {
            GroupName = grpName;            
        }

        //step2 添加需要监控的内容
        public void AddItem2OpcList(string strItemName)
        {
            opcNameList.Add(strItemName);
        }

        //step3 链接opc服务器
        public bool connectOpcServer()
        {
            bool bRet = false;
            
            if (string.IsNullOrEmpty(ServerAppName))
            {
                MessageBox.Show("ProgId can not be empty.");
                return bRet;
            }
            if (m_bSynServer)
            {
                if (this._synOpcServer != null)
                {
                    this._synOpcServer.Disconnect();
                    this._synOpcServer = null;
                }

                this._synOpcServer = new OPCServer();
                try
                {
                    this._synOpcServer.Connect(ServerAppName, ServerAppIP);
                    if( _synOpcServer.ServerState != (int)OPCServerState.OPCRunning)
                    {
                        MessageBox.Show("OPC 服务连接失败，请检查OPC相应设置");
                        return bRet;
                    }

                    m_bSynServerConnected = true;

                    //add group
                    mygroups = _synOpcServer.OPCGroups;//获取组集合
                    
                    if (mygroups != null )
                    {

                        //mygroups = myOPCsvr.OPCGroups;//获取组集合
                        //                              //mygroups.DefaultGroupIsActive = true;//设置组集合默认为激活
                        //mygroups.DefaultGroupDeadband = 0;//设置死区
                        //                                  //mygroups.DefaultGroupUpdateRate = 200;//更新为200ms              

                        //mygroup = mygroups.Add(grpName);
                        //mygroup.IsActive = isActive;
                        //mygroup.IsSubscribed = true;
                        //mygroup.UpdateRate = upDateRate;

                        //myitems = mygroup.OPCItems;
                        //myitems.AddItem("Channel_1.Device_1.Tag1", 1);  
                        //mygroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(Group_DataChange);
                        //mygroup.AsyncWriteComplete += new DIOPCGroupEvent_AsyncWriteCompleteEventHandler(Group_AsyncWriteComplete);

                        //add item
                        //List<string> items = new List<string>();

                        //mygroups.DefaultGroupIsActive = true;
                        //mygroups.DefaultGroupDeadband = 0;//设置死区
                        //mygroups.DefaultGroupUpdateRate = 100;//更新为200ms  
                                                                   
                        
                        mygroup = mygroups.Add(GroupName);
                        mygroup.IsActive = true;
                        mygroup.UpdateRate = 100;//和opc client 测试时，一定要一样的更新频率
                        mygroup.IsSubscribed = true;
                        //mygroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(Group_DataChange);
                        
                        OPCItems myitems = mygroup.OPCItems;

                        if (myitems != null )
                        {
                            string strItem = "";
                            for (int i = 0; i < opcNameList.Count; i++)
                            {
                                strItem = string.Format("{0}.{1}", GroupName, opcNameList[i]);

                                myitemArray[i] = myitems.AddItem(strItem, i);

                            }
                            
                            m_Values = new object[opcNameList.Count];
                        }
                        
                        bRet = true;
                    }
                    bRet = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return bRet;
            }

            if (this._opcServer != null)
            {
                this._opcServer.Disconnect();
                this._opcServer = null;
            }

            Type tp = Type.GetTypeFromProgID(ServerAppName);
            this._opcServer = new OpcServer(tp.GUID.ToString(), ServerAppIP);

            try
            {
                this._opcServer.Connect();

                //add group
                OpcGroup grp = this._opcServer.AddGroup(GroupName, 100, true);
                if (grp != null)
                {
                    grp.DataChanged += OnDataChange;
                    

                    //add item
                    List<string> items = new List<string>();

                    string strItem = "";
                    for (int i = 0; i < opcNameList.Count; i++)
                    {
                        strItem = string.Format("{0}.{1}", GroupName, opcNameList[i]);
                        items.Add(strItem);
                    }

                    m_Items = grp.AddItems(items.ToArray());
                    m_Values = new object[m_Items.Length];
                    
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return bRet;
        }

        #region 同步读取opc       
        /// <summary>
        /// 同步取数/一个一个读取
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private object GetOpcValueOne(int nItem)
        {
            OPCItem item;
            object itemValue = null;           
            try
            {
                try
                {

                    item = myitemArray[nItem];
                    //item = myitems.GetOPCItem( nItem );
                }
                catch
                {
                    
                    return itemValue ;
                }

                Object value = null ;
                Object quality;
                Object timestamp;
                //直接从设备上取数
                if( item != null )
                    item.Read((short)OPCDataSource.OPCCache, out value, out quality, out timestamp);

                return value;
            }
            catch (Exception es)
            {
                
                return itemValue;
            }
        }
        

        /// <summary>
        /// 断开OPC连接
        /// </summary>
        public void DisSynConnect()
        {
            if (!m_bSynServerConnected)
            {
                return;
            }

            //加锁 
            lock (syncLock)
            {
                if (_synOpcServer != null)
                {                   
                    try
                    {
                        GC.Collect();
                        _synOpcServer.OPCGroups.RemoveAll();                        
                        _synOpcServer.Disconnect();
                    }
                    catch
                    {                        
                          GC.Collect();
                    }
                }
            }

            m_bSynServerConnected = false;
        }
        #endregion

        public void  DisConnect()
        {
            if( _opcServer != null && _opcServer.IsConnected )
            {
                _opcServer.Disconnect();
                _opcServer = null;
            }

            DisSynConnect();

        }

        public void InitialPreValue()
        {
            object rObject = null;

            if (m_Values != null && m_Values.Length > 0)
            {
                for(int i = 0; i < m_Values.Length && i < opcPreValueList.Count; i++ )
                {
                    rObject = m_Values[i];
                    opcPreValueList[i] = rObject;
                }                
            }

        }

        /// <summary>
        /// return value 0 没有变化   1：上升沿    2： 下降沿
        /// </summary>
        /// <param name="nItem"></param>
        /// <returns></returns>
        public int GetOpcState(int nItem)
        {
            int nRet = NoChange;

            Boolean bValue = false;
            Boolean bPreValue = false;
            object preOb;
            object rob;
            rob = GetItemValue(nItem);
            if (rob != null)
            {
                bValue = (Boolean)rob;
                preOb = GetItemPreValue(nItem);
                SetItemPreValue(nItem, bValue);
                bPreValue = false;
                if (preOb != null)
                {
                    bPreValue = (Boolean)preOb;
                }

                if (bPreValue == false && bValue)//上升沿
                {
                    nRet = RisingEdge;
                }

                if (bPreValue && bValue == false)
                {
                    nRet = FallingEdge;
                }

                
            }
            return nRet;
        }

        //获取对应item的内容
        public object GetValue(ItemValue itemValue, Object strValue)
        {
            if (itemValue == null)
            {
                if (strValue == "") return null;
                return strValue;
            }

            //if (_type == typeof(DateTime))
            //{
            //    DateTime datetime = DateTimeCTRL.Value;
            //    if (datetime == DateTimeCTRL.MinDate) return DateTime.MinValue;
            //    return datetime;
            //}

            //if (_type != null && _type.IsArray) 
            //    return OpcDa.Client.Com.Convert.ChangeType(m_value, _type);

            // convert empty string to null for all types other than strings.
            Type type = itemValue.Value.GetType();
            if (type != typeof(string) && strValue != null && strValue == "")
                strValue = null;

            // convert string to type (null creates a default value for the type).
            return OpcDa.Client.Com.Convert.ChangeType(strValue, type);
        }

        //写入对应Item的内容
        public void WriteItemValue(int nItem, Object strValue)
        {
            if (m_bSynServer)
            {
                if (_synOpcServer == null)
                    return;
                if( nItem < myitemArray.Length && nItem >= 0)
                {
                    if(myitemArray[nItem] != null)
                        myitemArray[nItem].Write(strValue);
                }

                return;
            }
                
            if (_opcServer == null)
                return;
            OpcGroup grp = this._opcServer.FindGroupByName(GroupName);
            if (grp != null)
            {
                try
                {
                    ItemResult item = m_Items[nItem];
                    if (item != null)
                    {
                        ItemValue itemValue = new ItemValue(item);
                        itemValue.Value = strValue;
                        itemValue.Value = GetValue(itemValue, strValue);
                        
                        IdentifiedResult[] results = grp.AsyncWrite(new ItemValue[] { itemValue }, ++_handle, new WriteCompleteEventHandler(this.OnWriteComplete), out _request);
                        for (int i = 0; i < results.Length; i++)
                        {
                            if (results[i].ResultID.Failed())
                            {
                                string message = "Failed to write item \'" + results[i].ItemName + "\' error: " + results[i].ResultID.ToString();
                                MessageBox.Show(message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnDataChange(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new DataChangedEventHandler(OnDataChange), new object[] { subscriptionHandle, requestHandle, values });
                return;
            }


            foreach (ItemValueResult value in values)
            {
                // item value should never have a null client handle.
                if (value.ClientHandle == null)
                {
                    continue;
                }
                UpdateListViewItem(value);            

            }
        }

        /// <summary>
        /// Called when an asynchronous read request completes.
        /// </summary>
        private void OnReadComplete(object clientHandle, ItemValueResult[] values)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new ReadCompleteEventHandler(OnReadComplete), new object[] { clientHandle, values });
                    return;
                }

                if (!_handle.Equals(clientHandle))
                {
                    return;
                }

                foreach (ItemValueResult value in values)
                {
                    // item value should never have a null client handle.
                    if (value.ClientHandle == null)
                    {
                        continue;
                    }

                    if (value.ResultID.Failed())
                    {
                        string message = "Failed to read item \'" + value.ItemName + "\' error: " + value.ResultID.ToString();
                        MessageBox.Show(message);
                    }
                    else
                    {
                        UpdateListViewItem(value);
                    }
                }
            }
            catch
            {
            }
        }

        private void OnWriteComplete(object clientHandle, IdentifiedResult[] results)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new WriteCompleteEventHandler(OnWriteComplete), new object[] { clientHandle, results });
                return;
            }

            if (!_handle.Equals(clientHandle))
            {
                return;
            }

            foreach (IdentifiedResult result in results)
            {
                // item value should never have a null client handle.
                if (result.ClientHandle == null)
                {
                    continue;
                }

                if (result.ResultID.Failed())
                {
                    string message = "Failed to write item \'" + result.ItemName + "\' error: " + result.ResultID.ToString();
                    MessageBox.Show(message);
                }
            }
        }

        private void UpdateListViewItem(ItemValueResult value)
        {
            for (int i = 0; i < m_Items.Length; i++)
            {
                ItemResult result = this.m_Items[i];
                if (result != null && value != null &&
                    Guid.Equals(result.ClientHandle, value.ClientHandle))
                {                   
                    m_Values[i] = value.Value;
                }
            }
        }

        public object GetItemValue(int nItem)
        {
            object rObject = null;
            if (m_bSynServer)
            {
                //rObject = _synOpcServer.ReadItem((short)nItem);

                rObject = GetOpcValueOne(nItem);
            }
            else
            {
                if( nItem >= 0 && nItem < m_Values.Length )
                {
                    rObject = m_Values[nItem];
                }
                
            }    
            return rObject;
        }

        public object GetItemPreValue(int nItem)
        {
            object  ob = null;

            if ( nItem >= 0 && nItem < opcPreValueList.Count)
            {
                ob = opcPreValueList[nItem];
            }                
            return ob;
        }

        public void SetItemPreValue(int nItem, object ob)
        {           
            if (nItem >= 0 && nItem < opcPreValueList.Count)
            {
                opcPreValueList[nItem] = ob;
            }          
        }

        public void addAllCuttingItems(int nItemCount,int nWarningCount)
        {
            opcNameList.Clear();
            opcPreValueList.Clear();

            //int nWarningCount = PLCOpcFeedParameters.WarningState.warningList.Count;
            object ob = (object)false;

            string strName = "1";
            for (int i = 0; i < nItemCount - nWarningCount; i++)
            {                
                strName = (i+1).ToString();

                AddItem2OpcList(strName);
                opcPreValueList.Add(ob);
            }

            //warning
            addWarningCuttingItems(nWarningCount);
        }

        public void addWarningCuttingItems(int nItemCount)
        {
            
            object ob = (object)false;

            string strName = "1";
            for (int i = 0; i < nItemCount; i++)
            {
                strName = string.Format("WARN{0:D3}", i + 1);
                AddItem2OpcList(strName);
                opcPreValueList.Add(ob);
            }
        }
                
    }
}
