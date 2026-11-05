using MyCommanLib;
using QSerialPortComm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoCom
{
    public partial class SerialPortForm : Form
    {
        public QSerialPort serialPort;
        public string m_strReceiveText="";

        public int m_nType = 0;
        public string m_strEndFlag = "\r"; 

        delegate void SetComTextCallback(Control control, string text);

        public SerialPortForm(int nType = 0)
        {
            InitializeComponent();

            try
            {
                string[] strPortNames = SerialPort.GetPortNames();
                for (int i = 0; i < strPortNames.Length; i++)
                {
                    comboBoxPort.Items.Add(strPortNames[i]);
                }

                serialPort = new QSerialPort(strPortNames[0], m_strEndFlag);

                m_nType = nType;

                CreateSerialPort(strPortNames[0]);
                if (serialPort.IsOpen == false)
                {
                    MessageBox.Show("串口打开失败！");
                }


                comboBoxPort.Text = serialPort.PortName;
                comboBoxBaudRate.Text = serialPort.BaudRate.ToString();
                comboBoxDataBits.Text = serialPort.DataBits.ToString();
                comboBoxParity.Text = serialPort.Parity.ToString();
                comboBoxstopBits.Text = serialPort.StopBits.ToString();

                ReadBaseCommand();
            }
            catch (Exception)
            {
            }
            

        }
        #region 初始化 com
        public void CreateSerialPort( string strPortName)
        {
            //最好加上结束符号"\n",否则接收数据有肯被分开几段
            if (strPortName.Length > 0)
            {
                serialPort = new QSerialPort(strPortName, m_strEndFlag);

                string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\ComportParam{0}.xml", m_nType) ;
                serialPort.LoadParamFile(fileName);

                serialPort.ReadLineString += new SerialPortEventHandler(ComDataReceived);

                serialPort.OpenPort();

                ReFlashUI();
            }            
        }
        
        public void ClosePort()
        {
            if (serialPort != null)
            {
                string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\ComportParam{0}.xml", m_nType);
                string filePath = Path.GetDirectoryName(fileName);
                if (Directory.Exists(filePath) == false)
                    return;

                serialPort.SaveParamFile(fileName);
                serialPort.Close();
            }
        }

        public void ReadComPara()
        {
            if (serialPort != null)
            {
                string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\ComportParam{0}.xml", m_nType);

                serialPort.LoadParamFile(fileName);

            }
        }

        public void ReadBaseCommand()
        {
            string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\Command{0}.txt", m_nType);
            if (File.Exists(fileName) == false)
                return;

            string []strComm = File.ReadAllLines(fileName);
            string strInfo = "";
            for(int i = 0; i < strComm.Length; i++ )
            {
                strInfo = strInfo + strComm[i];                
            }
            richTextBoxNormal.Text = strInfo;
        }

        void ComDataReceived(object sender, SerialPortEventArgs e)
        {
            m_strReceiveText = (e.Message);
            SetComReadText(richTextBoxReceive, e.Message);            
        }

        private void SetComReadText(Control control, string text)
        {
            // InvokeRequired会比较线程的ID,如果调用的线程ID和创建的线程ID不一样返回TRUE
            if (this.IsDisposed)
            {
                return;
            }
            if (control.InvokeRequired)
            {//获取一个值，该值指示调用方在对控件进行方法调用时是否必须调用 Invoke 方法，因为调用方位于创建控件所在的线程以外的线程中
                SetComTextCallback d = new SetComTextCallback(SetComReadText);
                this.Invoke(d, new object[] { control, text });
            }
            else
            {
                if(  control.Text.Length > 500 )
                {
                    control.Text = "";
                }
                control.Text += text;
            }

        }

        public void ReFlashUI()
        {
            if (serialPort.IsOpen)
            {
                buttonOpen.Text = "关闭串口";
            }
            else
            {
                buttonOpen.Text = "打开串口";
            }
        }

        public void OperateAction(string strComm)
        {
            serialPort.Close();
            serialPort.OpenPort();
            if(serialPort.IsOpen)
            {
                serialPort.Write(strComm);
                m_strReceiveText = "";
                Task.Factory.StartNew(() =>
                {
                    CommonDefine.SaveComLogs("Send--->" + strComm.Trim() + "  " + "m_strReceiveText=" + m_strReceiveText);
                });
                
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    CommonDefine.SaveComLogs("SendFial--->" + strComm.Trim() + "  " + "m_strReceiveText=" + m_strReceiveText);
                });
            }
            
        }        

        public void EmptyReceiveBuffer()
        {
            m_strReceiveText = "";
        }

        public string GetLastResponseForRobot(bool bOK = true)
        {
            string str = "";
            if (bOK && m_strReceiveText.Length > 2)
            {
                int nStart = m_strReceiveText.Length - 2;
                str = m_strReceiveText.Substring(nStart, 2);
            }
            else if (m_strReceiveText.Length > 1)
            {
                str = m_strReceiveText.Substring(1, m_strReceiveText.Length - 1);
            }
            return str;
        }

        public string GetLastResponseForLoadPort()
        {
            string str = "";
            if(m_strReceiveText.Length > 6 )
            {
                str = m_strReceiveText.Substring(0, 6);
            }

            return str;
        }

        public string GetLastResponse()
        {
            string str = m_strReceiveText;
            Task.Factory.StartNew(() =>
            {
                CommonDefine.SaveComLogs("Recive--->" + str.Trim());
            });
            
            return str;
        }

        #endregion

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {                    
                    serialPort.Close();
                    ReFlashUI();
                    return;
                }
            }

            {

                string strPortName = comboBoxPort.Text;

                serialPort = new QSerialPort(strPortName, m_strEndFlag);
              

                string strValue = comboBoxBaudRate.Text;
                int nValue = 9600;
                try
                {
                    nValue = int.Parse(strValue);
                }catch
                {
                    nValue = 9600;
                }
                serialPort.BaudRate = nValue;


                strValue = comboBoxDataBits.Text;
                nValue = 8;
                try
                {
                    nValue = int.Parse(strValue);
                }
                catch
                {
                    nValue = 8;
                }
                serialPort.DataBits = nValue;


                strValue = comboBoxParity.Text;
                nValue = 8;
                try
                {
                    serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), strValue);
                }
                catch
                {
                   
                }               

                strValue = comboBoxstopBits.Text; 
                try
                {
                    serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), strValue);
                }
                catch
                {
                    
                }
                string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\ComportParam{0}.xml", m_nType);

                serialPort.SaveParamFile(fileName );

                CreateSerialPort(strPortName);
                MessageBox.Show("连接成功");
            }

            ReFlashUI();

            if (serialPort != null)
            {
                string fileName = Environment.CurrentDirectory + string.Format("\\COMParam\\ComportParam{0}.xml", m_nType);
                serialPort.SaveParamFile(fileName);
            }
        }

        private void buttonClearReceive_Click(object sender, EventArgs e)
        {

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string str = richTextBoxSend.Text + m_strEndFlag;
            serialPort.Write(str);
        }

        private void buttonClearSend_Click(object sender, EventArgs e)
        {

        }

        private void SerialPortForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
