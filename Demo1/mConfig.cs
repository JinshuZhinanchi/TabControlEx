using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzdDemo
{
    public class mConfig
    {
        string SocketIP;
        int SocketPort;

        int SocketNum;
        string Barcode;

        bool enableReader;
        string ReaderIP_1;
        int ReaderPort_1;
        bool ReaderConnect;
        string CheckItem;
        bool CheckResult;
        bool LaserAgain;
        string LogPath;

        int REQtime;
        int ReaderTime;

        int ReaderNum;

        double MoveXpos;
        double MoveYpos;

        public string socketip
        {
            get { return SocketIP; }
            set { SocketIP = value; }
        }
        public int socketport
        {
            get { return SocketPort; }
            set { SocketPort = value; }
        }

        public int socketnum
        {
            get { return SocketNum; }
            set { SocketNum = value; }
        }

        public string barcode
        {
            get { return Barcode; }
            set { Barcode = value; }
        }

        public bool enablereader
        {
            get { return enableReader; }
            set { enableReader = value; }
        }

        public string readerip
        {
            get { return ReaderIP_1; }
            set { ReaderIP_1 = value; }
        }
        public int readerport
        {
            get { return ReaderPort_1; }
            set { ReaderPort_1 = value; }
        }

        public bool readeconnect
        {
            get { return ReaderConnect; }
            set { ReaderConnect = value; }
        }

        public string checkitem
        {
            get { return CheckItem; }
            set { CheckItem = value; }
        }
        public bool checkresult
        {
            get { return CheckResult; }
            set { CheckResult = value; }
        }

        public bool laseragain
        {
            get { return LaserAgain; }
            set { LaserAgain = value; }
        }

        public string logpath
        {
            get { return LogPath; }
            set { LogPath = value; }
        }


        public int reqtime
        {
            get { return REQtime; }
            set { REQtime = value; }
        }


        public int readertime
        {
            get { return ReaderTime; }
            set { ReaderTime = value; }
        }

        public int readernum
        {
            get { return ReaderNum; }
            set { ReaderNum = value; }
        }

        public double xpos
        {
            get { return MoveXpos; }
            set { MoveXpos = value; }
        }

        public double ypos
        {
            get { return MoveYpos; }
            set { MoveYpos = value; }
        }
    }
}
