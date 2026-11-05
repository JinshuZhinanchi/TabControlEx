using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

using System.IO;
using System.Collections;
using System.Threading.Tasks;

namespace TabControlExDemo
{
    public partial class TCPForm : Form
    {
        public event EventHandler SetMarkFormCCDCheckHandler;// 设置markForm中opc状态
        //tcp/ip listner thread         
        TcpListener serverListener;
        private Thread listenThread;
        public bool keepReading = false;

        //tcp/ip  client thread
        private Thread listenThreadClient;
        public bool keepReadingClient = false;
        public TcpClient tcpListenClient;
        public NetworkStream clientNetStream;
        public int clientPort = 6000;

        public bool showWarning = false;

        public string receiveString="";
        public List<List<string>> receiveStringArray = new List<List<string>>();
               

        public bool m_bUseCCDCheck = true;
        public bool m_bReceiveFinished = false;
        public bool m_bSendCommand = true;
       

        Hashtable m_strCCDType = new Hashtable();
        public int m_nConnectType = 0;

        public bool m_bCheckSencond = false;
        public List<string> checkStringArray = new List<string>();

        public int m_nStartTickCount = Environment.TickCount;

        public byte[] m_ReceiveBytes = new byte[30];

        public string m_strEndFlag = "\r\n";
        public TCPForm()
        {
            InitializeComponent();
            ReadIni();
            clientPort = int.Parse(textBoxLocalPort.Text);
        }

        public void CheckSecond( bool bCheckSecond , string[] checkArray )
        {
            m_bCheckSencond = bCheckSecond;            
            if ( bCheckSecond )
            {
                checkStringArray.Clear();
                foreach (string strc in checkArray )
                {
                    checkStringArray.Add(strc);
                }
            }           
        }

        public void ReadIniInfo()
        {           
            ReadIni();
        }

        public void WriteIni()
        {
            string fileName = Environment.CurrentDirectory + "\\CheckCCD.ini";

            string strText = IPAddressTextBox.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("CCDIni", $"IPAddress{m_nConnectType}", strText, fileName);

            strText = IPPortTextBox.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("CCDIni", $"IPPort{m_nConnectType}", strText, fileName);

            strText = textBoxLocalIP.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("CCDIni", $"LocalIPAddress{m_nConnectType}", strText, fileName);

            strText = textBoxLocalPort.Text;
            MyCommanLib.CommonDefine.WritePrivateProfileString("CCDIni", $"LocalIPPort{m_nConnectType}", strText, fileName);

            strText = m_bUseCCDCheck.ToString();
            MyCommanLib.CommonDefine.WritePrivateProfileString("CCDIni", $"LocalUseCCD{m_nConnectType}", strText, fileName);

          
        }

        public void ReadIni()
        {
            string fileName = Environment.CurrentDirectory + "\\CheckCCD.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }

            StringBuilder temp = new StringBuilder(255);

            MyCommanLib.CommonDefine.GetPrivateProfileString("CCDIni", $"IPAddress{m_nConnectType}", "192.168.1.5", temp, 255, fileName);
            IPAddressTextBox.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("CCDIni", $"IPPort{m_nConnectType}", "9500", temp, 255, fileName);
            IPPortTextBox.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("CCDIni", $"LocalIPAddress{m_nConnectType}", "192.168.1.6", temp, 255, fileName);
            textBoxLocalIP.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("CCDIni", $"LocalIPPort{m_nConnectType}", "6000", temp, 255, fileName);
            textBoxLocalPort.Text = temp.ToString();

            MyCommanLib.CommonDefine.GetPrivateProfileString("CCDIni", $"LocalUseCCD{m_nConnectType}", "True", temp, 255, fileName);
            m_bUseCCDCheck = Convert.ToBoolean( temp.ToString() );
                  
            checkBoxCCD.Checked = m_bUseCCDCheck;

        }       

        public int GetCCDProductID(string strKey)
        {
            int nValue = -1;
            object ob =null;
            string strValue;
            if ( m_strCCDType.ContainsKey(strKey) )
            {
                ob = m_strCCDType[strKey];
            }
            if( ob != null)
            {
                strValue = ob.ToString();
                nValue = int.Parse(strValue);
            }           
            return nValue;
        }

        public void ReadCCDModuleType( string strDir)
        {
            m_strCCDType.Clear();

            string fileName = strDir  + "\\CCDModuleType.ini";
            if (File.Exists(fileName) == false)
            {
                return;
            }         

            StreamReader reader = new StreamReader(fileName);

            string strText = "";
            string strKey = "";
            string strValue="";
            int nPos = 0;
            int nLen = 0;
            //// 一行一行读取
            strText = reader.ReadLine();

            while (strText != null)
            {
                strText.Trim();
                nPos = strText.IndexOf('=');
                if( nPos > 0 )
                {
                    strKey = strText.Substring(0, nPos);
                    nLen = strText.Length - nPos - 1;
                    if( nLen > 0 )
                    {
                        strValue = strText.Substring(nPos + 1, nLen);
                        m_strCCDType.Add(strKey,strValue);
                    }                    
                }
                strText = reader.ReadLine();
            }
            reader.Close();

          
        }

        #region  服务端监听
        //TCP/IP Listener   监听
        private void startButton_Click(object sender, EventArgs e)
        {
            //startListen();
        }

        private void startListen()
        {
            if (!keepReading)
            {
                string address = IPAddressTextBox.Text;
                string port = IPPortTextBox.Text;
                string addressRegex = @"^\d+\.\d+\.\d+\.\d+$";
                string portRegx = @"^\d+$";
                if (Regex.IsMatch(address, addressRegex) && Regex.IsMatch(port, portRegx))
                {
                    keepReading = true;
                    //监听的是本地的IP地址，因为电脑可以同时链接到多个网络，所以一台电脑可以同时有不同的IP地址，这里需要指明要监听哪个IP的哪个端口
                    listenThread = new Thread(() => ipListen(IPAddress.Parse(address), int.Parse(port)));
                    listenThread.Start();
                    //this.stateLabel.Text = "正在监听";
                    this.startButton.Text = "停止监听";
                }
                else
                {
                    MessageBox.Show("检测CCD，请填写正确的IP地址和端口");
                }
            }
            else
            {
                keepReading = false;
                //this.stateLabel.Text = "空闲";
                this.startButton.Text = "开始监听";
            }
        }
        private void ipListen(IPAddress address, int port)
        {
            serverListener = new TcpListener(address, port);
            serverListener.Start();
            OneString oneString = new OneString(changeReceiveTextBox);
            while (keepReading)//这是一个阀，来控制是否脱离线程，因为要把listener停掉，所以不能用abort来关闭线程
            {
                if (serverListener.Pending())//这是判断是否有输入的，必须要加这个判断，不然线程就会在下面的AcceptTcpClient卡住
                {
                    using (TcpClient client = serverListener.AcceptTcpClient())
                    {
                        //接收
                        NetworkStream netStream = client.GetStream();
                        byte[] receiveBytes = new byte[client.ReceiveBufferSize];
                        netStream.Read(receiveBytes, 0, client.ReceiveBufferSize);
                        string str = Encoding.Default.GetString(receiveBytes).TrimEnd('\0');

                        //oneString(str);
                        //返回
                        byte[] sendBytes = Encoding.Default.GetBytes("receive!");
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        netStream.Close();
                    }
                }
            }
            serverListener.Stop();
        }

        private delegate void OneString(string content);

        private void changeReceiveTextBox(string content)
        {
            if (resultTextBox.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                OneString d = new OneString(changeReceiveTextBox);
                this.Invoke(d, new object[] { content });
            }
            else
            {
                resultTextBox.Text += content;               
            }

        }

        public  void setReceiveTextBox(string content)
        {
            if (resultTextBox.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                OneString d = new OneString(setReceiveTextBox);
                this.Invoke(d, new object[] { content });
            }
            else
            {
                resultTextBox.Text = content;              
            }

        }
        #endregion

        #region  客户端监听

        private void sendButton_Click(object sender, EventArgs e)
        {
           
              //  string strContent = sendTextBox.Text + "\r\n";
            //this.clientWrite(strContent);         
            //string strCommand = "";
            if(tcpListenClient == null )
            {
                MessageBox.Show("请连接服务");
            }

            string strContent = sendTextBox.Text + m_strEndFlag;
            this.clientWrite(strContent);    
            
            //SendT2CCD(0);
            // LoadPortModbusOperate.ReadLPState3(clientNetStream);
        }

        public  bool connectServer(int nTimes )
        {
            bool bConnected = false;
            if (!keepReadingClient)
            {
                string address = IPAddressTextBox.Text;
                string port = IPPortTextBox.Text;
                string addressRegex = @"^\d+\.\d+\.\d+\.\d+$";
                string portRegx = @"^\d+$";
                if (Regex.IsMatch(address, addressRegex) && Regex.IsMatch(port, portRegx))
                {

                    //监听的是本地的IP地址，因为电脑可以同时链接到多个网络，所以一台电脑可以同时有不同的IP地址，这里需要指明要监听哪个IP的哪个端口

                    //找到服务器的IP地址
                    //IPAddress ipAddress = IPAddress.Parse(address);
                    //创建TcpClient对象实现与服务器的连接
                    try
                    {
                        clientPort = int.Parse(textBoxLocalPort.Text);
                    }
                    catch (Exception)
                    {
                    }
                    
                    string addressLoc = textBoxLocalIP.Text;
                    int portLoc = clientPort + nTimes; //int.Parse(textBoxLocalPort.Text) + nTimes;
                    IPAddress ipAddressL1 = IPAddress.Parse(addressLoc);
                    //IPAddress ipAddressL2 = IPAddress.Parse("192.168.1.19");


                    IPEndPoint iPEndPoint1 = new IPEndPoint(ipAddressL1, portLoc);
                    //IPEndPoint iPEndPoint2  = new IPEndPoint(ipAddressL2, 8500);


                    //连接服务器
                    try
                    {
                        tcpListenClient = new TcpClient(iPEndPoint1);

                        tcpListenClient.Connect(address, int.Parse(port));                                       

                        clientNetStream = tcpListenClient.GetStream();

                        keepReadingClient = true;

                        listenThreadClient = new Thread(() => ipClientListen());
                        listenThreadClient.Start();

                        
                        bConnected = true;
                        showWarning = false;
                        //this.stateLabel.Text = "正在监听";
                        this.conButton.Text = "停止监听";

                    }
                    catch (Exception)
                    {
                        this.conButton.Text = "连接";
                        bConnected = false;
                        tcpListenClient = null;
                        keepReadingClient = false;
                        showWarning = true;
                        clientPort++;                        
                        //MessageBox.Show("检测CCD，请填写正确的IP地址和端口1");
                        
                    }
                }
                else
                {
                    this.conButton.Text = "连接";
                    bConnected = false;
                    showWarning = true;
                    //MessageBox.Show("检测CCD，请填写正确的IP地址和端口");
                }
            }
            else
            {
                bConnected = false;
                showWarning = false;
                keepReadingClient = false;
                this.conButton.Text = "连接";
                CloseClient();      

            }

            WriteIni();
            return bConnected;
        }

        private void conButton_Click(object sender, EventArgs e)
        {
            connectServer(0);
            if (showWarning ) 
            {
                MessageBox.Show("检测CCD，请填写正确的IP地址和端口");
            }
        }

        public void clearReceiveData()
        {
            int nCount = receiveStringArray.Count;
            for(int i = 0; i < nCount; i++ )
            {
                List<string> recData = receiveStringArray[i];
                recData.Clear();
            }

            receiveStringArray.Clear();
        }

        //nResult  非0的数量； 0:OK 1 NG  2  偏移
        public int CheckReceiveData()
        {
            
            int nResult = 0;
            int nCount = receiveStringArray.Count;
            int nNGCount = 0;
            for (int i = 0; i < nCount; i++)
            {
                List<string> recData = receiveStringArray[i];
                
                if(recData.Count > 0 )
                {
                    string sValue = recData[0];
                    sValue.Trim();
                    string strHead = "EXW";
                    if (sValue.Length > 0)
                    {
                        strHead = sValue.Substring(sValue.Length - 1);
                        try
                        {
                            nResult = int.Parse(strHead);

                            if( nResult == 0 && m_bCheckSencond )
                            {
                                int nSrcCount = checkStringArray.Count;
                                int nDstCount = recData.Count;
                                for(int n = 0; n < nDstCount-1 && n < nSrcCount; n++ )
                                {
                                    string dstStr = recData[n + 1];
                                    string srcStr = checkStringArray[n];
                                    dstStr.Trim();
                                    srcStr.Trim();
                                    dstStr.ToUpper();
                                    srcStr.ToUpper();
                                    if( dstStr.CompareTo(srcStr) != 0)
                                    {
                                        nResult = 6;
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            nResult = 6;
                        }                        
                    }                        
                    else
                        nResult = 5;

                    if (nResult > 0)
                        nNGCount++;
                }
            }
            return nNGCount;
        }

        public void EncodeReceiveStrArray(string strArray )
        {
            //clearReceiveData();

            char startChar = '+';
            //判断及拆分出有效数据    加号“+”开始，分号“;”结束
            int nStartIndex = strArray.IndexOf(startChar);
            if (nStartIndex < 0)
                return;
            if (nStartIndex == strArray.Length - 1)
                return;

            string strContent;
            strContent = strArray.Substring(nStartIndex + 1);
            //拆分结束

            string str = strContent;
            string sss = "";
            string strTemp = str;
            char fenge = ';';
            int nn = str.IndexOf(fenge);
            while (nn > 0)
            {
                sss = strTemp;
                str = sss.Substring(0, nn);

                List<string> recData = new List<string>();

                EncodeReceiveString(recData, str);
                this.receiveStringArray.Add(recData);                

                strTemp = sss.Substring(nn + 1, sss.Length - nn - 1);

                nn = strTemp.IndexOf(fenge);                
            }

            if(strTemp.Length > 1 )
            {
                str = strTemp;
                List<string> recData = new List<string>();
                EncodeReceiveString(recData, str);
                this.receiveStringArray.Add(recData);                
            }
        }

        public void EncodeReceiveString(List<string> recData, string strReceive)
        {
            string strLast = strReceive;
            string strTemp = "";
            string str;
            int nPos;
            string eStr = ",";
            while(strLast.Length > 0 )
            {
                nPos = strLast.IndexOf(eStr);
                if( nPos > 0 )
                {
                    str = strLast.Substring(0, nPos);
                    int nLen = strLast.Length - nPos - 1;
                    if (nLen > 0)
                    {
                        strTemp = strLast.Substring(nPos + 1, nLen);
                    }
                    else
                    {
                        strTemp = "";
                    }

                    strLast = strTemp;
                }
                else
                {
                    str = strLast;
                    strLast = "";
                }
                str.Trim();
                recData.Add(str);
            }
        }

        public void SendT2CCD(int nType )
        {
            if (tcpListenClient == null)
                return;

            //byte[] comm = new byte[5];
            // comm[0] = (int)'T';
            // comm[1] = (int)'1';
            // comm[2] = 13;
            // comm[3] = 10;
            // comm[4] = 0;
            string strCommand = "EXW,0\r\nT1\r\n";
            if( nType != 0 )
            {
                strCommand = "EXW,1\r\nT1\r\n";
            }
            //20210915 绍兴基恩士让去掉模式指令
            strCommand = "T1\r\n";

            if ( m_nConnectType == 1)
            {
                strCommand = "LON\r\n";
            }

            this.sendTextBox.Text = strCommand;

            byte[] sendBytes = Encoding.Default.GetBytes(strCommand);            

            if (tcpListenClient.Connected)
            {
                clientNetStream.Write(sendBytes, 0, sendBytes.Length);             
                //netStream.Close();                
            }
        }


        public void SendLiaoHaoCCD(int nCCdType )
        {
            if (tcpListenClient == null)
                return;

            string strCommand = string.Format("PW,1,{0}\r\n", nCCdType);

            byte[] array = System.Text.Encoding.ASCII.GetBytes(strCommand);

            int nLen = strCommand.Length ;

            byte[] comm = new byte[nLen+1];
            comm[nLen] = 0;
            for(int i = 0; i < nLen; i++ )
            {
                comm[i] = array[i];
            }
        /*    comm[0] = 80;//P
            comm[1] = 87;//PW
            comm[2] = 44;//,
            comm[3] = (byte)(31+nType);
            comm[4] = 44;//,
            comm[5] = 13;
            comm[6] = 10;
            comm[7] = 0;
            */
            if (tcpListenClient.Connected)
            {                
                clientNetStream.Write(comm, 0, comm.Length);
                //netStream.Close();                
            }
        }

        public void clientWrite(string strComm)
        {
            if (tcpListenClient == null)
                return;

            if (tcpListenClient.Connected)
            {                
                byte[] sendBytes = Encoding.Default.GetBytes(strComm);
                clientNetStream.Write(sendBytes, 0, sendBytes.Length);
                //netStream.Close();                
            }
          /*  else
            {
                string address = IPAddressTextBox.Text;
                string port = IPPortTextBox.Text;
                string strContent = sendTextBox.Text + "\r\n";

                //找到服务器的IP地址
                IPAddress ipAddress = IPAddress.Parse(address);
                //创建TcpClient对象实现与服务器的连接
                string addressLoc = textBoxLocalIP.Text;
                int portLoc = int.Parse(textBoxLocalPort.Text) + 500;
                IPAddress ipAddressL1 = IPAddress.Parse(addressLoc);
                //IPAddress ipAddressL2 = IPAddress.Parse("192.168.1.19");

                IPEndPoint iPEndPoint1 = new IPEndPoint(ipAddressL1, portLoc);

                TcpClient client = new TcpClient();
                //连接服务器
                client.Connect(address, int.Parse(port));
                using (client)
                {
                    //连接完服务器后便在客户端和服务端之间产生一个流的通道
                    NetworkStream netStream = client.GetStream();
                    byte[] sendBytes = Encoding.Default.GetBytes(strContent);
                    netStream.Write(comm, 0, comm.Length);
                    netStream.Close();

                }
            }*/
            
        }
        private void ipClientListen()
        {
            if (tcpListenClient == null)
                return;

            if (keepReadingClient)
            {
                while (tcpListenClient!= null && tcpListenClient.Connected)//这是一个阀，来控制是否脱离线程，因为要把listener停掉，所以不能用abort来关闭线程
                {
                    if (keepReadingClient == false)
                    {
                        return;
                    }
                    try
                    {
                        
                        string str = "";
                        NetworkStream netStream = clientNetStream;// tcpListenClient.GetStream();
                        if (keepReadingClient == false)
                        {
                            return;
                        }


                        int nLen = tcpListenClient.ReceiveBufferSize;

                        if (m_nConnectType > 100 && nLen > 30 )//modbus
                        {
                            nLen = 30;
                        }

                        m_ReceiveBytes = new byte[nLen];

                        int i;
                        i = netStream.Read(m_ReceiveBytes, 0, nLen );
                        nLen = i;

                        if( m_nConnectType > 100 && i > 10 )//modbus
                        {
                            checkStringArray.Clear();

                            string str11 = "";
                            int nValue = 0;
                            for(int ii = 0; ii < nLen; ii++ )
                            {
                                nValue = m_ReceiveBytes[ii];
                                str11 = str11 + " " + nValue.ToString();

                                checkStringArray.Add(nValue.ToString());
                            }

                            str11 = str11 + ";";
                            OneString oneString = new OneString(setReceiveTextBox);
                            oneString(str11);
                            receiveString = str11;

                            m_bReceiveFinished = true;
                        }
                        else if (i > 3)
                        {
                           
                            str = Encoding.Default.GetString(m_ReceiveBytes).TrimEnd('\0');
                            if( m_nConnectType >=1 && m_nConnectType < 10)
                            {
                               
                                receiveString = str;

                                string smifStr = "s00INF:READY/COMM;";
                                if ( str.Length >=smifStr.Length  )
                                {
                                    string strSrc = str.Substring(0, smifStr.Length);
                                    if( strSrc == smifStr )
                                    {
                                        clientWrite("s00ACK:READY/COMM;\r");
                                    }
                                }
                                m_bReceiveFinished = true;
                            }
                            else
                            {
                               
                                receiveString = str;
                                this.EncodeReceiveStrArray(str);

                                m_bReceiveFinished = true;
                            }

                            ThreadFlashUI();
                        }

                        Thread.Sleep(10);
                    }
                    catch (Exception ex)
                    {
                       // MessageBox.Show(ex.Message);
                    }
                    

                    /*while ((i = netStream.Read(receiveBytes, 0, receiveBytes.Length)) != 0)
                    {
                        if(receiveStringArray.Count == 0 )
                        {
                            for (int k = 0; k < i; k++)
                            {
                                if (receiveBytes[k] == '\r')
                                {                                    
                                    EncodeReceiveString(receiveString);                                    
                                    receiveString = "";                                    
                                }
                                else
                                {
                                    str = Encoding.Default.GetString(receiveBytes).TrimEnd('\0');
                                    receiveString += str;
                                }
                            }
                        }                        
                        //str = Encoding.Default.GetString(receiveBytes).TrimEnd('\0');
                        //oneString(str);
                    }
                    */

                }
            }
            else
            {
                if(tcpListenClient.Connected )
                {
                    this.conButton.Text = "连接";
                    CloseClient();
                }                
            }

        }
        #endregion

        public void ThreadFlashUI()
        {
            Task task = Task.Run(() =>
            {
                OneString oneString = new OneString(changeReceiveTextBox);
                oneString(receiveString);
            });
        }

        public void CloseClient()
        {
            if (tcpListenClient == null)
                return;

            keepReadingClient = false;
            
            if( tcpListenClient.Connected )
            {
                NetworkStream netStream = clientNetStream;// tcpListenClient.GetStream();
                if(netStream != null )
                {
                    netStream.Close();
                    clientNetStream = null;
                }
                clientPort++;
                tcpListenClient.Close();                
                tcpListenClient = null;
                //MessageBox.Show(clientPort.ToString());
                Thread.Sleep(1000);
            }
                
        }

        private void TcpIpCS_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteIni();

            CloseClient();
            serverListener.Stop();            
        }

        private void TCPForm_SizeChanged(object sender, EventArgs e)
        {
        }

        private void checkBoxCCD_CheckedChanged(object sender, EventArgs e)
        {
            m_bUseCCDCheck = checkBoxCCD.Checked;
            //SetMarkFormCCDCheckHandler(m_bUseCCDCheck.ToString(),e);
        }

        public void SendOCRCommand(int nType)
        {
            
            string strCommand = "READ\r\n";//1741
            if (nType == 1)
            {
                strCommand = "SM\"READ\"0\r\n";//盖泽V1  
            }
            SendStrCommand(strCommand);
        }

        public void SendStrCommand(string strCommand)
        {
            this.sendTextBox.Text = strCommand;

            byte[] sendBytes = Encoding.Default.GetBytes(strCommand);

            if (tcpListenClient.Connected)
            {
                clientNetStream.Write(sendBytes, 0, sendBytes.Length);
                //netStream.Close();                
            }
        }

    }


}
