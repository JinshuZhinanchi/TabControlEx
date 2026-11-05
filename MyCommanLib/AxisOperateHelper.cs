using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyCommanLib;

namespace MyCommanLib
{
    public class AxisOperateDataHelper
    {
        public DoublePoint safePoint = new DoublePoint();
        public DoublePoint workPoint = new DoublePoint();
        public DoublePoint workPoint2 = new DoublePoint();
        public AxisOperateDataHelper( )
        {

        }

        public Boolean Write( string fileName, int nLaserType )
        {
            Boolean bRet = false;
            string keyName = "";         
            int nType = nLaserType;

            keyName = string.Format("SafePoint-{0}", nType);
            safePoint.Write(fileName, keyName);

            keyName = string.Format("WorkPoint-{0}", nType);
            workPoint.Write(fileName, keyName);

            keyName = string.Format("WorkPoint2-{0}", nType);
            workPoint2.Write(fileName, keyName);

            return true;
        }

        public Boolean Read( string fileName , int nLaserType )
        {
            Boolean bRet = false;
            string keyName = "";
            int nType = nLaserType;

            keyName = string.Format("SafePoint-{0}", nType);
            safePoint.Read(fileName, keyName);

            keyName = string.Format("WorkPoint-{0}", nType);
            workPoint.Read(fileName, keyName);

            keyName = string.Format("WorkPoint2-{0}", nType);
            workPoint2.Read(fileName, keyName);
            return true;
        }

    }
}
