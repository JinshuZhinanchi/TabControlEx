using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ClientEngine;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;

namespace EzdDemo
{
    public class TCPReader
    {
        public bool IsConnected
        {
            get
            {
                return _Client.IsConnected;
            }
        }
        public SuperSocket.ClientEngine.AsyncTcpSession _Client;
        private string IP_Address;
        private int IP_Port;
        public string LogPath;

        public mConfig mconfig = new mConfig();
        #region Event

        public MachineParam.Tmsgevent OnEvent;
        public MachineParam.Ttcpsocket OnOutReciveData;

        public void DoOutEvent(int msg, int val)
        {
            if (OnEvent != null)
                OnEvent(msg, val);
        }
        public void DoOutReciveData(int deviceName, string msg, int val)
        {
            if (OnOutReciveData != null) 
                OnOutReciveData(deviceName, msg, val);
        }
        #endregion

        ~TCPReader()
        {
            try
            {
                _Client.Close();
            }
            catch(System.Exception ex)
            {
            }
        }
        public void DoSetIpAddress(string ipaddress,int ipport)
        {
            IP_Address = ipaddress;
            IP_Port = ipport;
        }

        public void DoInit(string IpAddress,int PortNo)
        {
            IP_Address = IpAddress;
            IP_Port = PortNo;
            if (IpAddress != "" && PortNo >= 5)
            {
                this._Client = new SuperSocket.ClientEngine.AsyncTcpSession();
                //System.Net 
                this._Client.Connected += new EventHandler(_Client_Connected);
                this._Client.DataReceived += new EventHandler<DataEventArgs>(_Client_DataReceived);
                this._Client.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(_Client_Error);
                this._Client.Closed += new EventHandler(_Client_Closed);
                this._Client.ReceiveBufferSize = 4096;
                bool res = DoConnect(IpAddress, PortNo);

                mconfig.readeconnect = res;
            }
        }
        public void DoClose()
        {
            try
            {
                this._Client.Close();
            }
            catch (System.Exception ex)
            {

            }
        }
        public bool DoConnect(string hostName, int portNumber)
        {
            try
            {
                EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(hostName), portNumber);
                this._Client.Connect(serverAddress);
                Thread.Sleep(50);
                if (!_Client.IsConnected)
                {
                    DoOutEvent(1, 0);
                    return false;
                }

            }
            catch (System.Exception ex)
            {

                return false;
            }
            return true;
        }
        public bool DoReConnect()
        {
            bool result = true;
            try
            {
                string IpAddress = this.IP_Address;
                int PortNo = this.IP_Port;
                EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IpAddress), PortNo);
                _Client.Close();
                Thread.Sleep(50);
                DoInit(IpAddress, PortNo);
                if (!_Client.IsConnected)
                {
                    DoOutEvent(2, 0);
                    result = false;
                }
                else
                {
                    DoOutEvent(3, 1);
                }
            }
            catch (System.Exception ex)
            { 
                result = false;
                //DoOutEvent(2, 0);
            }
            finally
            {
                if (!result)
                {
                    DoOutEvent(2, 0);
                }
            }
            return result;
        }
        void _Client_Closed(object sender, EventArgs e)
        {

        }
        void _Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            
        }
        void _Client_DataReceived(object sender, DataEventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
                try
                {
                    int findng = 0;
                    string msg = Encoding.ASCII.GetString(e.Data, e.Offset, e.Length);
                    if (msg=="NoRead")
                    {
                        findng++;
                        mconfig.checkresult = false;
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            if (mconfig.checkitem == msg)
                            {
                                mconfig.checkresult = true;
                            }
                        }
                        else
                        {
                            mconfig.checkresult = false;

                        }

                    }
                    DoSend("E");
                    string mPrintString = msg;

                    DoOutReciveData(1, msg, 0);

                    AddReciveData("读码器:" + mPrintString);
                }
                catch (Exception ex)
                {
                    mconfig.checkresult = false;
                }
            //});
        }
        void _Client_Connected(object sender, EventArgs e)
        {
        }
        public void DoSend(string msg)
        {
            try
            {
                byte[] arr_1 = Encoding.ASCII.GetBytes(msg);
                this._Client.Send(arr_1, 0, arr_1.Length);
            }
            catch (System.Exception ex)
            {

            }
        }
        public void DoStartRead()
        {
            try
            {
                DoSend("T");
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        #region log record
        private List<string> mLoging = new List<string>();
        public void AddReciveData(string msg)
        {
            string strTime = DateTime.Now.ToString("yyyy-MM-dd/HH-mm-ss  ") + DateTime.Now.Millisecond.ToString("d3") + ": ";
            string logText = "";
            logText = "  Time: " + strTime + "  " + "data: " + msg;
            mLoging.Add(logText);
            if (mLoging.Count > 10)
            {
                SaveLog();
            }
        }
        private void SaveLog()
        {
            if (mLoging.Count == 0) return;
            string fullname = LogPath + DateTime.Now.ToString("yyyy-MM-dd-hh") + "   Barcode_Recive.ini";
            using (FileStream fs = new FileStream(fullname, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    try
                    {
                        for (int i = 0; i < mLoging.Count; i++)
                            sw.WriteLine(mLoging[i]);
                    }
                    finally
                    {
                        mLoging.Clear();
                        sw.Close();
                        fs.Close();
                    }
                }
            }
        }
        #endregion
    }
}
