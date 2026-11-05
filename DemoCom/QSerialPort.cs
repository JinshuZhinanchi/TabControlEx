using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;

namespace QSerialPortComm
{
    /// <summary>
    /// SerialPort事件处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SerialPortEventHandler(object sender, SerialPortEventArgs e);

    /// <summary>
    /// 事件参数
    /// </summary>
    public class SerialPortEventArgs : EventArgs
    {
        #region 定义字段
        
        private string _Message;        
        private string _PortName;

        #endregion 
        
        #region 定义属性

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            set { _Message = value; }
            get { return _Message; }
        }

        /// <summary>
        /// 端口名
        /// </summary>
        public string PortName
        {
            set { _PortName = value; }
            get { return _PortName; }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">端口名称</param>
        /// <param name="message">消息内容</param>
        public SerialPortEventArgs(string portName, string message)
        {
            this.PortName = portName;
            this.Message = message;
        }
    }

    /// <summary>
    /// QSerialPort串口通讯类,从SerialPort继承的类
    /// </summary>
    public class QSerialPort : SerialPort
    {
        #region 定义字段

        private string endStr = "";
        private byte[] strReadBuf = null;
        private Boolean synchronous = false;

        #endregion

        #region 事件定义

        /// <summary>
        /// 接收到一行数据
        /// </summary>
        public event SerialPortEventHandler ReadLineString;

        #endregion

        #region 定义属性
        
        public string EndStr
        {            
            set { endStr = value; }
            get { return endStr; }
        }

        public Boolean Synchronous
        {
            set { synchronous = value; }
            get { return synchronous; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// QSerialPort构造函数
        /// portName是端口号"COM1,COM2.."
        /// EngStr是字符结束符号
        /// </summary>
        /// <param name="portName"></param> 
        /// <param name="EngStr"></param> 
        /// <returns></returns> 
        public QSerialPort(string portName,string EngStr)
        {
            Encoding = Encoding.Default;//编码
            endStr = EngStr;
            PortName = portName;
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;

            WriteTimeout = 100;
            ReadTimeout = 100;

            DataReceived += new SerialDataReceivedEventHandler(QSerialPort_DataReceived);           
        }

        #endregion

        /// <summary>
        /// 设置参数
        /// </summary>
        public void SetParam(string PortName,int BaudRate, int DataBits, Parity parity, StopBits stopBits)
        {
            this.PortName = PortName;
            this.BaudRate = BaudRate;
            this.DataBits = DataBits;
            this.Parity = parity;
            try
            { 
                this.StopBits = stopBits;
            }
            catch { }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        public bool OpenPort()
        {           
            if (IsOpen)
            {
                Close();
            }

            try
            {
                
                Open();
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show("串口打开失败，请检查！");
                return false;
            }

            return true;
        }

        // <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            if (IsOpen)
            {
                Close();
            }
            strReadBuf = null;            
        }
        
        /// <summary>
        /// 得到解码的字符串
        /// </summary>
        private string GetDecodeString(byte[] buf)
        {
            return Encoding.Default.GetString(buf, 0, buf.Length);
        }


        /// <summary>
        /// 从字符缓冲区中读出第一条字符串,然后把读出的第一条从缓冲区中取出
        /// </summary>        
        private string GetALineString()
        {
            if (strReadBuf == null)
            {
                return null;
            }
            string str = GetDecodeString(strReadBuf);
            int index = str.IndexOf(endStr);
           
            if (index >= 0)
            {//有字符串结束符号  
                string strCode = str.Substring(0, index);//注意汉字是两个字节,所以这样写
                byte[] buf = Encoding.Default.GetBytes(strCode);

                Array.Copy(strReadBuf,buf,buf.Length);
                string strLine = GetDecodeString(buf);

                if ((buf.Length + endStr.Length ) >= strReadBuf.Length)
                {
                    strReadBuf = null;
                }
                else
                {
                    buf = new byte[strReadBuf.Length - buf.Length - endStr.Length];
                    Array.Copy(strReadBuf, index + endStr.Length, buf, 0, buf.Length);
                    strReadBuf = buf;
                }
                return strLine;
            }
            return null;
        }

        public void synReadAllData()
        {
            QSerialPort sp = (QSerialPort)this;
            int nReadBytes = sp.BytesToRead;
            if (nReadBytes > 0)
            {
                try
                {
                    ///从缓冲区中读出的新的数据
                    byte[] buf = new byte[nReadBytes];
                    sp.Read(buf, 0, nReadBytes);

                    if (endStr == null || endStr.Length == 0)
                    {//没有结束符号,有数据就发出去
                        string strGet = GetDecodeString(buf);
                        if (ReadLineString != null && strGet != null && strGet.Length > 0)
                        {
                            ReadLineString(this, new SerialPortEventArgs(sp.PortName, strGet));
                        }
                        return;
                    }

                    ///把新的数据放到数据缓冲区中
                    if (strReadBuf == null)
                    {
                        strReadBuf = buf;
                        buf = null;
                    }
                    else
                    {
                        byte[] newbuf = new byte[strReadBuf.Length + nReadBytes];

                        Array.Copy(strReadBuf, newbuf, strReadBuf.Length);
                        Array.Copy(buf, 0, newbuf, strReadBuf.Length, nReadBytes);

                        sp.strReadBuf = newbuf;
                    }

                    /// 从字符缓冲区中读出第一条字符串,然后把读出的第一条从缓冲区中取出
                    string strLine = sp.GetALineString();
                    while (strLine != null)
                    {
                        if (ReadLineString != null && strLine != null && strLine.Length > 0)
                        {
                            ReadLineString(this, new SerialPortEventArgs(sp.PortName, strLine));
                        }
                        strLine = sp.GetALineString();
                    }
                }
                catch
                {
                }
            }
        }

     
        /// <summary>
        /// 数据接收事件处理
        /// </summary>
        void QSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if( synchronous )
                return;

            QSerialPort sp = (QSerialPort)sender;
            int nReadBytes =sp.BytesToRead;
            if (nReadBytes > 0)
            {
                try
                {
                    ///从缓冲区中读出的新的数据
                    byte[] buf = new byte[nReadBytes];
                    sp.Read(buf, 0, nReadBytes);

                    if (endStr == null || endStr.Length == 0)
                    {//没有结束符号,有数据就发出去
                        string strGet = GetDecodeString(buf);
                        if (ReadLineString != null && strGet != null && strGet.Length > 0)
                        {
                            ReadLineString(this, new SerialPortEventArgs(sp.PortName, strGet));
                        }
                        return;
                    }

                    ///把新的数据放到数据缓冲区中
                    if(strReadBuf==null)
                    {
                        strReadBuf = buf;
                        buf = null;
                    }
                    else
                    {
                        byte[] newbuf = new byte[strReadBuf.Length + nReadBytes];
                        
                        Array.Copy(strReadBuf,newbuf,strReadBuf.Length);
                        Array.Copy(buf,0,newbuf,strReadBuf.Length,nReadBytes);

                        sp.strReadBuf = newbuf;                    
                    }

                    /// 从字符缓冲区中读出第一条字符串,然后把读出的第一条从缓冲区中取出
                    string strLine = sp.GetALineString();
                    while (strLine != null)
                    {
                        if (ReadLineString != null && strLine != null && strLine.Length > 0)
                        {
                            ReadLineString(this, new SerialPortEventArgs(sp.PortName, strLine));
                        }
                        strLine = sp.GetALineString();
                    }                                                       
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// 保存当前参数到文件中
        /// </summary>
        public bool SaveParamFile(string file)
        {
            XmlDocument xDom = new XmlDocument();
            if (!File.Exists(file))
            {//文件不存在
                
                XmlElement xmlRoot = xDom.CreateElement("SerialPorts");
                xDom.AppendChild(xmlRoot);
                xDom.Save(file);
            }

            xDom.Load(file);
           
            XmlNode root = xDom.SelectSingleNode("SerialPorts");
            root.RemoveAll();          
           
            XmlElement xParamNode = xDom.CreateElement("Param");        
            XmlElement xSubNode;

            xSubNode = xDom.CreateElement("PortName");
            xSubNode.InnerText = PortName;
            xParamNode.AppendChild(xSubNode);

            xSubNode = xDom.CreateElement("BaudRate");
            xSubNode.InnerText = BaudRate.ToString();
            xParamNode.AppendChild(xSubNode);

            xSubNode = xDom.CreateElement("DataBits");
            xSubNode.InnerText = DataBits.ToString();
            xParamNode.AppendChild(xSubNode);

            xSubNode = xDom.CreateElement("Parity");
            xSubNode.InnerText = Enum.GetName(typeof(Parity),Parity);
            xParamNode.AppendChild(xSubNode);

            xSubNode = xDom.CreateElement("StopBits");
            xSubNode.InnerText = Enum.GetName(typeof(StopBits), StopBits);
            xParamNode.AppendChild(xSubNode);


            root.AppendChild(xParamNode);//增加端口配置

            xDom.Save(file);
            return true;
        }

        /// <summary>
        /// 载入参数文件中的设置参数
        /// </summary>
        public bool LoadParamFile(string file)
        {
            if (!File.Exists(file))
            {//文件不存在
                return false;
            }
            XmlDocument xDom = new XmlDocument();
            xDom.Load(file);
           
            XmlNode root = xDom.SelectSingleNode("SerialPorts");
            if (root == null)
            {
                return false;
            }
            XmlNode xParamNode = root.SelectSingleNode("Param");
            if (xParamNode == null)
            {
                return false;
            }

            XmlNodeList xmlNodeList = xParamNode.ChildNodes;
            if (xmlNodeList == null)
            {
                return false;
            }
            foreach (XmlNode xn in xmlNodeList)
            {
                switch (xn.Name)
                {
                    case "PortName":
                        PortName = xn.InnerText;
                        break;
                    case "BaudRate":
                        BaudRate = int.Parse(xn.InnerText);
                        break;
                    case "DataBits":
                        DataBits = int.Parse(xn.InnerText);
                        break;
                    case "Parity":
                        Parity = (Parity)Enum.Parse(typeof(Parity), xn.InnerText);
                        break;
                    case "StopBits":
                        StopBits = (StopBits)Enum.Parse(typeof(StopBits), xn.InnerText);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
