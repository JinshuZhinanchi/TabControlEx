using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyCommanLib;

namespace TabControlExDemo
{
    public class OpcAxisOperateHelper
    {        
        public  OpcOperateForm opcOperateParent;
        
        public OpcAxisOperateHelper( )
        {

        }

        public OpcAxisOperateHelper(OpcOperateForm opcOperate)
        {
            opcOperateParent = opcOperate;
        }

        public  Boolean IntialAllAxis()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.XINGAN_MarkZAxis.OrgControl;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 300 * 1000;
                int nSleep = 0;

                int nItem1 = PLCOpcFeedParameters.XINGAN_MarkZAxis.OrgState;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
                while (bRet == false)
                {
                    ob = opcOperateParent.GetItemValue(nItem1);
                    bRet = (Boolean)ob;
                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);

                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            
            return bRet;
        }


        public  Boolean IsXunbianExitWafer()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_IOInState.AlignerVacuum;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }

            return bRet;
        }

        public Boolean OperateVacummDevice(Boolean bValue)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.ExtIOOutState.Y50;
                opcOperateParent.WriteItemValue(nItem, bValue);
                bRet = true;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferXifu(Boolean bValue )
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.IOOutState.vacuumOperate;
                opcOperateParent.WriteItemValue(nItem1, bValue);
                bRet = true;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IsWaferXifu()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_IOInState.AlignerVacuum;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferLight(Boolean bOrig)
        {
            Boolean bRet = false;
            try
            {
                if( bOrig )
                {
                    int nItem1 = PLCOpcFeedParameters.XINGAN_IOOutState.LightVacuum;

                    opcOperateParent.WriteItemValue(nItem1, true);

                }else
                {
                    int nItem1 = PLCOpcFeedParameters.XINGAN_IOOutState.LightVacuum;
                    opcOperateParent.WriteItemValue(nItem1, false);


                }
                bRet = true;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IsWaferLightOrig()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X57;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IsWaferLightWork()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X04;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWafer_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_WorkPos()&& IS_OperateWaferY_WorkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_WorkPos() && IS_OperateWaferY_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 运动到SIC打标位
        /// </summary>
        /// <returns></returns>
        public Boolean OperateSIC_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.XINGAN_MarkZAxis.SicControl;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateSIC_WorkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateSIC_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 运动到SI打标位
        /// </summary>
        /// <returns></returns>
        public Boolean OperateSI_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.XINGAN_MarkZAxis.SiControl;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateSI_WorkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateSI_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 运动到OCR打标位
        /// </summary>
        /// <returns></returns>
        public Boolean OperateOCR_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.XINGAN_MarkZAxis.OCRControl;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateOCR_WorkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateOCR_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferX_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;
                                
                bRet = IS_OperateWaferX_WorkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_WorkPos();
                   
                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferY_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferY_WorkPos();

                while (bRet == false)
                {
                    bRet = IS_OperateWaferY_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);

                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferR_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferR_WorkPos();

                while (bRet == false)
                {
                    bRet = IS_OperateWaferR_WorkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);

                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public void waferInfo2OPC()
        {
            int nProductIndex = CommonDefine.waferFileInfo.nProductIndex;
            string strProductName = CommonDefine.productHelper.m_ProductList[nProductIndex];

            Boolean bRead = CommonDefine.waferFileInfo.productAxisData.Read(strProductName);
            if( bRead == false )
            {
                return;
            }

            if (opcOperateParent != null)
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
                int nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX1;

                //寻边点位1 动态位
                nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX1;
                opcOperateParent.WriteItemValue(nItem, nPos );

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferY1;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferR1;
                opcOperateParent.WriteItemValue(nItem, nPos);

                //寻边点位2 接片位
                nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX2;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferY2;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferR2;
                opcOperateParent.WriteItemValue(nItem, nPos);

                //打标位1  动态位
                nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkX1;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkY1;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkZ1;
                opcOperateParent.WriteItemValue(nItem, nPos);

                //打标位2 打标位
                nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkX2;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkY2;
                opcOperateParent.WriteItemValue(nItem, nPos);

                nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkZ2;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
        }

        public void OPC2WaferInfo()
        {
            int nProductIndex = CommonDefine.waferFileInfo.nProductIndex;
            string strProductName = CommonDefine.productHelper.m_ProductList[nProductIndex];                        

            if (opcOperateParent != null)
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
                int nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX1;
                object ob;

                //寻边点位1 动态位
                nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX1;
                ob = opcOperateParent.GetItemValue(nItem);
                if( ob != null )
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferX1 = nPos;
                

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferY1;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferY1 = nPos;

              
                nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferR1;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferR1 = nPos;

                //寻边点位2 接片位
                nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferX2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferX2 = nPos;

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferY2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferY2 = nPos;

                nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nWaferR2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nWaferR2 = nPos;

                //打标位1  动态位
                nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkX1;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkX1 = nPos;

                nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkY1;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkY1 = nPos;

                nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.safe;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkZ1;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkZ1 = nPos;

                //打标位2 打标位
                nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkX2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkX2 = nPos;

                nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkY2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkY2 = nPos;

                nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.work;
                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkZ2;
                ob = opcOperateParent.GetItemValue(nItem);
                if (ob != null)
                {
                    try
                    {
                        nPos = (int)ob;
                    }
                    catch (Exception)
                    {
                    }
                }
                CommonDefine.waferFileInfo.productAxisData.nMarkZ2 = nPos;

                CommonDefine.waferFileInfo.productAxisData.Write(strProductName);
            }
        }

        public  Boolean IS_OperateWaferX_WorkPos()
        {
            Boolean bRet = false;

            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorX.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }           

            return bRet;
        }

        public  Boolean IS_OperateWaferY_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorY.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateWaferR_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorR.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 是否在SIC的打标位
        /// </summary>
        /// <returns></returns>
        public bool IS_OperateSIC_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_MarkZAxis.SicState;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 是否在SI的打标位
        /// </summary>
        /// <returns></returns>
        public bool IS_OperateSI_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_MarkZAxis.SiState;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        /// <summary>
        /// 是否在OCR检测位置
        /// </summary>
        /// <returns></returns>
        public bool IS_OperateOCR_WorkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.XINGAN_MarkZAxis.OCRState;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateWaferX_WorkPos(int nOffset)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem);
                int nPos = 0;
                try
                {
                    if (ob != null)
                    {
                        nPos = (int)ob;
                    }
                }
                catch (Exception)
                {
                }

                nPos =  nOffset;
                
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            
            return bRet;
        }

        public  Boolean Set_OperateWaferY_WorkPos(int nOffset)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.safe;

                object ob = opcOperateParent.GetItemValue(nItem);
                int nPos = 0;
                try
                {
                    if (ob != null)
                    {
                        nPos = (int)ob;
                    }
                }
                catch (Exception)
                {
                }

                nPos =  nOffset;

                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateWaferR_WorkPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        #region 接片位
        public Boolean OperateWafer_MPos( Boolean bMoveR = true )
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                if(bMoveR)
                {
                    nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.work;
                    opcOperateParent.WriteItemValue(nItem, true);
                } 
                    
                

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                Boolean bRfet3 = true;
                if( bMoveR)
                {
                    bRfet3 = IS_OperateWaferR_MPos();
                }

                bRet = IS_OperateWaferX_MPos() && IS_OperateWaferY_MPos() && bRfet3;
                while (bRet == false)
                {
                    if (bMoveR)
                    {
                        bRfet3 = IS_OperateWaferR_MPos();
                    }

                    bRet = IS_OperateWaferX_MPos() && IS_OperateWaferY_MPos() && bRfet3;

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }             

            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferX_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferY_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferY_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferY_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateWaferR_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferR_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferR_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateWaferX_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorX.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateWaferY_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorY.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IsWafer_ROrig()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X02;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateWaferR_MPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorR.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateWaferX_MPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateWaferY_MPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateWaferR_MPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }
        #endregion 


        #region 寻边拍照位

        public double GetCurrentPosX()
        {
            double fValue = 0;
            int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.currentPos;
            object ob = opcOperateParent.GetItemValue(nItem );
            if( ob != null )
            {
                int nValue = (int)ob;
                fValue = (double)(nValue);
                fValue = fValue / 1000.0;
            }    
            return fValue;
        }

        public  double GetCurrentPosY()
        {
            double fValue = 0;
            int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Param.baseMotor.currentPos;
            object ob = opcOperateParent.GetItemValue(nItem);
            if (ob != null)
            {
                int nValue = (int)ob;
                fValue = (double)(nValue);
                fValue = fValue / 1000.0;
            }
            return fValue;
        }

        public double GetCurrentPosR()
        {
            double fValue = 0;
            int nItem = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.currentPos;
            object ob = opcOperateParent.GetItemValue(nItem);
            if (ob != null)
            {
                int nValue = (int)ob;
                fValue = (double)(nValue);
                fValue = fValue / 1000.0;
            }
            return fValue;
        }


        public Boolean OperateWafer_FindVPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position2;
                opcOperateParent.WriteItemValue(nItem, true);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.position2;
                opcOperateParent.WriteItemValue(nItem, true);

               
                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_FindVPos() && IS_OperateWaferY_FindVPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_FindVPos() && IS_OperateWaferY_FindVPos() ;

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }

            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferX_FindVPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position2;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferY_FindVPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.position2;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferY_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferY_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }
        public Boolean IS_OperateWaferX_FindVPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorX.State.position2;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IS_OperateWaferY_FindVPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorY.State.position2;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }
        #endregion

        #region 定位拍照位
        public Boolean OperateWafer_VPhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position3;
                opcOperateParent.WriteItemValue(nItem, true);

                nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.position3;
                opcOperateParent.WriteItemValue(nItem, true);


                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_VPhotoPos() && IS_OperateWaferY_VPhotoPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_VPhotoPos() && IS_OperateWaferY_VPhotoPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }

            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferX_VPhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position3;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferX_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferX_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateWaferY_VPhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FFindEdgeMotorY.Control.position3;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateWaferY_MPos();
                while (bRet == false)
                {
                    bRet = IS_OperateWaferY_MPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }
        public Boolean IS_OperateWaferX_VPhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorX.State.position3;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IS_OperateWaferY_VPhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FFindEdgeMotorY.State.position3;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }
        #endregion

        //Mark
        public Boolean OperateMarkX_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkX_PhotoPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkX_PhotoPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateMarkY_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkY_PhotoPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkY_PhotoPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateMarkZ_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkZ_PhotoPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkZ_PhotoPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateMarkY_SafePos()
        {
            Boolean bRet = false;
            try
            {
                int nPos = 0;
                Set_OperateMarkY_PhotoPos(nPos);
                bRet = OperateMarkY_PhotoPos();

                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkY1;
                Set_OperateMarkY_PhotoPos(nPos);

                

            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IS_OperateMarkY_SafePos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X45;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean OperateMarkZ_SafePos()
        {
            Boolean bRet = false;
            try
            {
                int nPos = 0;
                Set_OperateMarkZ_PhotoPos(nPos);
                bRet = OperateMarkZ_PhotoPos();

                nPos = CommonDefine.waferFileInfo.productAxisData.nMarkZ1;
                Set_OperateMarkZ_PhotoPos(nPos);


            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IS_OperateMarkZ_SafePos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X42;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateMarkX_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FRobotMotorX.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateMarkY_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FMarkMotorY.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }


        public  Boolean IS_OperateMarkZ_PhotoPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FMarkMotorZ.State.baseMotor.safe;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkX_PhotoPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkY_PhotoPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkZ_PhotoPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.safe;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }


        public  Boolean OperateMarkX_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkX_MarkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkX_MarkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateMarkY_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkY_MarkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkY_MarkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean OperateMarkZ_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 60 * 1000;
                int nSleep = 0;

                bRet = IS_OperateMarkZ_MarkPos();
                while (bRet == false)
                {
                    bRet = IS_OperateMarkZ_MarkPos();

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateMarkX_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FRobotMotorX.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateMarkY_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FMarkMotorY.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean IS_OperateMarkZ_MarkPos()
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.FMarkMotorZ.State.baseMotor.work;
                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkX_MarkPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkY_MarkPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public  Boolean Set_OperateMarkZ_MarkPos(int nPos)
        {
            Boolean bRet = false;
            try
            {
                int nItem = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.work;
                opcOperateParent.WriteItemValue(nItem, nPos);
            }
            catch (Exception)
            {
            }
            return bRet;
        }


        //robotX        
        public Boolean OperateRobotX_WorkPos(int nPosIndex)
        {
            Boolean bRet = false;
            try
            {

                int nItem1 = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.safe;
                int nItem2 = PLCOpcFeedParameters.FRobotMotorX.State.baseMotor.safe;
                if( nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS )
                {
                    nItem1 = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.work;
                    nItem2 = PLCOpcFeedParameters.FRobotMotorX.State.baseMotor.work;
                }else if( nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SMIF1_POS)
                {
                    nItem1 = PLCOpcFeedParameters.FRobotMotorX.Control.position2;
                    nItem2 = PLCOpcFeedParameters.FRobotMotorX.State.position2;
                }
                else if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SMIF2_POS)
                {
                    nItem1 = PLCOpcFeedParameters.FRobotMotorX.Control.position3;
                    nItem2 = PLCOpcFeedParameters.FRobotMotorX.State.position3;
                }
                else if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SBOX_POS)
                {
                    nItem1 = PLCOpcFeedParameters.FRobotMotorX.Control.position4;
                    nItem2 = PLCOpcFeedParameters.FRobotMotorX.State.position4;
                }


                opcOperateParent.WriteItemValue(nItem1, true);

                Thread.Sleep(1000);

                int nStartTickCount = Environment.TickCount;
                int nOverTime = 30 * 1000;
                int nSleep = 0;

                object ob = opcOperateParent.GetItemValue(nItem2);
                bRet = (Boolean)ob;
                while (bRet == false)
                {
                    ob = opcOperateParent.GetItemValue(nItem2);
                    bRet = (Boolean)ob;

                    nSleep = MyCommanLib.CommonDefine.DelayedTime(nStartTickCount);
                    if (nSleep > nOverTime)
                    {
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public Boolean IS_OperateRobotX_WorkPos(int nPosIndex)
        {
            Boolean bRet = false;
            try
            {
                int nItem1 = PLCOpcFeedParameters.ExtIOInState.X34;
                
                if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_MBOX_POS)
                {
                    nItem1 = PLCOpcFeedParameters.ExtIOInState.X34;
                }
                else if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SMIF1_POS)
                {
                    nItem1 = PLCOpcFeedParameters.ExtIOInState.X36;
                }
                else if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SMIF2_POS)
                {
                    nItem1 = PLCOpcFeedParameters.ExtIOInState.X37;
                }
                else if (nPosIndex == PLCOpcFeedParameters.PLCConstDefine.ROBOT_SBOX_POS)
                {                    
                    nItem1 = PLCOpcFeedParameters.FRobotMotorX.State.position4;
                }

                object ob = opcOperateParent.GetItemValue(nItem1);
                bRet = (Boolean)ob;
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        //robot 
        public Boolean IsRobotRuning()
        {
            Boolean bRunning = false;
            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.ArmRunning);
            if (ob != null)
            {
                bRunning = (Boolean)ob;
            }
            return bRunning;
        }

        public Boolean IsRAarmXifu()
        {
            Boolean bReady = false;

            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.RightArmVacuum);
            if (ob != null)
            {
                bReady = (Boolean)ob;
            }

            return bReady;
        }

        public Boolean IsLAarmXifu()
        {
            Boolean bReady = false;

            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.LeftArmVacuum);
            if (ob != null)
            {
                bReady = (Boolean)ob;
            }

            return bReady;
        }


        public Boolean IsRAarmOrig()
        {
            Boolean bReady = false;

            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.RightArmHome);
            if (ob != null)
            {
                bReady = (Boolean)ob;
            }

            return bReady;
        }

        public Boolean IsLAarmOrig()
        {
            Boolean bReady = false;

            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.LeftArmHome);
            if (ob != null)
            {
                bReady = (Boolean)ob;
            }

            return bReady;
        }

        public Boolean IsRobotReady()
        {
            Boolean bReady = false;

            Boolean bEmError = false;
            Boolean bRunning = false;

            object ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.ArmError);
            if (ob != null)
            {
                bEmError = (Boolean)ob;
            }

            ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.ArmRunning);
            if (ob != null)
            {
                bRunning = (Boolean)ob;
            }

            if (bRunning == false)
            {
                CommonDefine.TickSleep(100);
                ob = opcOperateParent.GetItemValue(PLCOpcFeedParameters.XINGAN_IOInState.ArmRunning);
                if (ob != null)
                {
                    bRunning = (Boolean)ob;
                }
            }

            if (bEmError == false && bRunning == false)
            {
                bReady = true;
            }

            return bReady;
        }

        
    }
}
