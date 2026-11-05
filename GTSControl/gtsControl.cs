using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using gts;
using gts;
using MyCommanLib;
using System.Threading;

namespace jog
{
    public class GtsControl
    {
        public gts.mc.TTrapPrm []m_TtrapPrm = new gts.mc.TTrapPrm[3];
        public double[] m_fSpeed = new double[3];

        public gts.mc.TJogPrm[] m_TjogPrm = new gts.mc.TJogPrm[3];
        public double[] m_fManualSpeed = new double[3];

        public event EventHandler RefreshChipListUIHandler;
        public event EventHandler RefreshBoxListUIHandler;

        public int m_nWorkState;

        //data struct


        //public GtsPoint m_SafePoint = new GtsPoint();
        //public GtsPoint m_TakeMaterialPoint = new GtsPoint();
        //public GtsPoint m_TakePhotoPoint = new GtsPoint();
        //public GtsPoint m_MarkPoint = new GtsPoint();
        //public GtsPoint m_MarkRevertPoint = new GtsPoint();

        //public GtsPoint[] m_StartPoint = new GtsPoint[8];
        //public GtsPoint[] m_ScanStartPoint = new GtsPoint[8];

        //public double m_fzLayerOffset;
        //public double m_fzTakeOffset;

        //public int m_nXiliaoSleep = 500;
        //public int m_nRevertSleep = 500;

        public GtsWorkStation[] m_gtsWorkStation = new GtsWorkStation[2];

        public MarkChipsStruct[,] m_MarkChipsData = new MarkChipsStruct[8,25];        

        //flow control
        public int m_nWorkFlow;//扫描  -- 取料 -- 扫码 -- 打标 -- 放料--取料...  
        public ChipBoxStruct m_WorkChipList = new ChipBoxStruct();
        public ChipBoxList   m_WorkReadyBoxList = new ChipBoxList();
        public int[]         m_WorkChipState = new int[25];       //ui 0: 无  1：有  2：完成
        public int m_nWorkBoxIndex = 0;//ui
        public int m_nWorkLayerIndex = 0;//ui
        public int m_nWorkBoxSize = 0;

        public GtsForm m_gtsForm = null;
        public int m_nWorkTestMode = 1;
        public int m_nTestFun = 0;//功能按钮测试

        public static InfoForm m_InfoForm = new InfoForm();
        public static WarningFormJog m_warnForm;

        public GtsControl()
        {            
 
            for(int i = 0; i < m_TtrapPrm.Length; i++)
            {
                m_TtrapPrm[i].acc = 1;
                m_TtrapPrm[i].dec = 1;
                m_TtrapPrm[i].smoothTime = 0;
                m_TtrapPrm[i].velStart = 0;

                m_fSpeed[i] = 5;
            }

            initialChipMarkData();

            m_nWorkState = AxisTestForm.WORK_MANUAL;

            //test 
            //m_WorkReadyBoxList.m_nCurrentChipIndex = 0;
            //Point pp = new Point(0, AxisTestForm.BOXCHIP_WAITSTATE);
            //m_WorkReadyBoxList.m_BoxList.Add(pp);
            //pp = new Point(1, AxisTestForm.BOXCHIP_WAITSTATE);
            //m_WorkReadyBoxList.m_BoxList.Add(pp);
            //pp = new Point(2, AxisTestForm.BOXCHIP_WAITSTATE);
            //m_WorkReadyBoxList.m_BoxList.Add(pp);
            //pp = new Point(3, AxisTestForm.BOXCHIP_WAITSTATE);
            //m_WorkReadyBoxList.m_BoxList.Add(pp);

            m_warnForm  = new WarningFormJog(this);
        }
        public void Close()
        {
            gts.mc.GT_Close();
        }

        public void ResetWork()
        {
            //gts.mc.GT_Reset();

            //gts.mc.GT_LoadConfig("GTS800.cfg");//加载配置文件            

            gts.mc.GT_ClrSts(AxisTestForm.UPDN_AXIS, 8);//清除限位和报警
           
            EnableAllDevice();
            Thread thr = new Thread(new ThreadStart(ShowInfo));
            thr.Start();

            //FindAllHome();
           
        }

        public void ShowInfo()
        {
            m_InfoForm = new InfoForm();
            m_InfoForm.m_gtsControl = this;
            m_InfoForm.SetInfoText("复位中......");
            m_InfoForm.ShowDialog();

            OperateRevertGas(true);
            OperateXiliao(false);
            OperateClearLight();
            SetDo(AxisTestForm.YellowLightIOOUT, 1);

        }

        public short OperateDo(int ioIndex)
        {
            short sRtn = 0;
            int lpValue = 0;
            sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lpValue);

            if ((lpValue & (1 << ioIndex)) == 0)
            {
                lpValue = (lpValue | (1 << ioIndex));
            }
            else
            {
                lpValue = (lpValue & ~(1 << ioIndex));
            }

            sRtn = gts.mc.GT_SetDo(gts.mc.MC_GPO, lpValue);

            return sRtn;
        }
        public void SetStopIO( short nIndex )
        {
            //gts.mc.GT_SetStopIo(AxisTestForm.STRECH_AXIS, 0, gts.mc.MC_GPI, nIndex);
            gts.mc.GT_SetStopIo(AxisTestForm.UPDN_AXIS, 0, gts.mc.MC_GPI, nIndex);
            //gts.mc.GT_SetStopIo(AxisTestForm.ROTATE_AXIS, 0, gts.mc.MC_GPI, nIndex);
        }

        public void SetEStopIO(short nIndex)
        {
            //gts.mc.GT_SetStopIo(AxisTestForm.STRECH_AXIS, 1, gts.mc.MC_GPI, nIndex);
            gts.mc.GT_SetStopIo(AxisTestForm.UPDN_AXIS, 1, gts.mc.MC_GPI, nIndex);
            //gts.mc.GT_SetStopIo(AxisTestForm.ROTATE_AXIS, 1, gts.mc.MC_GPI, nIndex);
        }


        public void SetManualJogPrm(int axis, gts.mc.TJogPrm jogPrm, double fSpeed)
        {
            m_TjogPrm[axis - 1].acc = jogPrm.acc ;
            m_TjogPrm[axis - 1].dec = jogPrm.dec ;
            

            m_fManualSpeed[axis - 1] = fSpeed;
        }

        public void MoveJogMinus(short axis)
        {
            gts.mc.GT_PrfJog(axis);//设置为jog模式

            gts.mc.TJogPrm jog;
            gts.mc.GT_GetJogPrm(axis, out jog);

            jog.acc = m_TjogPrm[axis - 1].acc;
            jog.dec = m_TjogPrm[axis - 1].dec;
            jog.smooth = m_TjogPrm[axis - 1].smooth;        

            double vel = m_fManualSpeed[axis-1];          

            gts.mc.GT_SetJogPrm(axis, ref jog);//设置jog运动参数
            gts.mc.GT_SetVel(axis, -vel);//设置目标速度
            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动
        }

        internal void initialChipMarkData()
        {
            m_gtsWorkStation[0] = new GtsWorkStation();
            m_gtsWorkStation[1] = new GtsWorkStation();
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 25; j++ )
                {
                    m_MarkChipsData[i, j] = new MarkChipsStruct();
                }                
            }
        }

        internal void ResetChipMarkData()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    m_MarkChipsData[i, j].bSelect = false;
                    m_MarkChipsData[i, j].strMarkContent = "";
                }
            }
        }

        public void MoveJogAdd(short axis)
        {
            gts.mc.GT_PrfJog(axis);//设置为jog模式

            gts.mc.TJogPrm jog;
            gts.mc.GT_GetJogPrm(axis, out jog);

            jog.acc = m_TjogPrm[axis - 1].acc;
            jog.dec = m_TjogPrm[axis - 1].dec;
            jog.smooth = m_TjogPrm[axis - 1].smooth;

            double vel = m_fManualSpeed[axis - 1];

            gts.mc.GT_SetJogPrm(axis, ref jog );//设置jog运动参数
            gts.mc.GT_SetVel(axis, vel);//设置目标速度
            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动
        }

        public void MoveStop(short axis )
        {
            gts.mc.GT_Stop(1 << (axis - 1), 0);
            System.Threading.Thread.Sleep(500);
            //gts.mc.GT_PrfTrap(AxisTestForm.STRECH_AXIS);
            gts.mc.GT_PrfTrap(AxisTestForm.UPDN_AXIS);
            //gts.mc.GT_PrfTrap(AxisTestForm.ROTATE_AXIS);
        }

        public void Start()
        {
            if (m_nWorkState == AxisTestForm.WORK_AUTO)
                return;

            //gts.mc.GT_PrfTrap(AxisTestForm.STRECH_AXIS);
            gts.mc.GT_PrfTrap(AxisTestForm.UPDN_AXIS);
            //gts.mc.GT_PrfTrap(AxisTestForm.ROTATE_AXIS);
           
            m_nWorkState = AxisTestForm.WORK_AUTO;
        }

        public void Stop()
        {
            //MoveStop(AxisTestForm.STRECH_AXIS);
            MoveStop(AxisTestForm.UPDN_AXIS);
            //MoveStop(AxisTestForm.ROTATE_AXIS);
           

            m_nWorkState = AxisTestForm.WORK_MANUAL;
        }

        public void ClearStates()
        {
            //gts.mc.GT_ClrSts(AxisTestForm.STRECH_AXIS, 8);
            gts.mc.GT_ClrSts(AxisTestForm.UPDN_AXIS, 8);
            //gts.mc.GT_ClrSts(AxisTestForm.ROTATE_AXIS, 8);
            
        }
        
        public void EnableAllDevice()
        {
            //gts.mc.GT_AxisOn(AxisTestForm.STRECH_AXIS);
            gts.mc.GT_AxisOn(AxisTestForm.UPDN_AXIS);
            //gts.mc.GT_AxisOn(AxisTestForm.ROTATE_AXIS);
            
        }
        public void FindAllHome()
        {        
            //FindHome(AxisTestForm.STRECH_AXIS,true);
            //FindHome(AxisTestForm.ROTATE_AXIS, true);
            FindHome(AxisTestForm.UPDN_AXIS,true);            

        }

        public void FindHome(short axis ,bool bWait = false ,Boolean bFindSecond = true)
        {
            #region //智能查找原点
            //设置 Smart Home 回原点参数
            gts.mc.THomePrm tHomePrm;
            short nRet = 0;
            nRet = gts.mc.GT_GetHomePrm(axis, out tHomePrm);
            tHomePrm.mode = 22;//12;
            tHomePrm.moveDir = -1;
            tHomePrm.indexDir = 1;
            tHomePrm.edge = 0;
            tHomePrm.velHigh = 50;
            tHomePrm.velLow = 5;
            tHomePrm.acc = 0.5;
            tHomePrm.dec = 0.5;
            if (axis == AxisTestForm.ROTATE_AXIS)
            {
                tHomePrm.velHigh = 40;
                tHomePrm.velLow = 2;
            }
            
            
            tHomePrm.searchHomeDistance  = 2000000;
            tHomePrm.searchIndexDistance = 2000000;
            tHomePrm.escapeStep = 20000;


            nRet = gts.mc.GT_GoHome(axis, ref tHomePrm); //启动 Smart Home 回原点
                        
            if( bWait )
            {
                gts.mc.THomeStatus homeStatus;
                gts.mc.GT_GetHomeStatus(axis, out homeStatus);
               
                while( homeStatus.run != 0 && homeStatus.error == 0 )
                {                    
                    MyCommanLib.CommonDefine.TickSleep(100);
                    gts.mc.GT_GetHomeStatus(axis, out homeStatus);                    
                }
            }

            gts.mc.GT_ZeroPos(axis, 8);   

           if( bFindSecond )
           {
               int lAxisStatus = 0;
               uint clk;
               gts.mc.GT_GetSts(axis, out lAxisStatus, 1, out clk);
               // 正向限位 负向限位    紧急停止 未使能
               if ((lAxisStatus & 0x40) != 0)
               {
                   gts.mc.GT_ClrSts(axis, 8);//清除各轴报警和限位
                                             //移动到正向,再次找原点
                    GtsPoint gtsPoint = GetSafePoint(0);
                   double fPos = gtsPoint.m_fx;
                   if (axis == AxisTestForm.STRECH_AXIS)
                   {
                       fPos = gtsPoint.m_fx;
                   }

                   if (axis == AxisTestForm.UPDN_AXIS)
                   {
                       //move z
                       fPos = gtsPoint.m_fz;
                   }

                   if (axis == AxisTestForm.ROTATE_AXIS)
                   {
                       //move r            
                       fPos = gtsPoint.m_fr;
                   }

                   P2PMoveSlow(axis, fPos, true);
                   FindHome(axis, true, false);

               }                    
           }
                     

            #endregion
        }


        internal bool IsXSafePos( )
        {
            bool bRet = true;
            if( GetCurrentPos(AxisTestForm.STRECH_AXIS) > AxisTestForm.AxisSafePos[AxisTestForm.STRECH_AXIS - 1] )
            {
                bRet = false;
            }
            return bRet;
        }

        internal bool IsZSafePos()
        {
            bool bRet = true;
            if (GetCurrentPos(AxisTestForm.UPDN_AXIS) < AxisTestForm.AxisSafePos[AxisTestForm.UPDN_AXIS - 1])
            {
                bRet = false;
            }
            return bRet;
        }

        public double GetCurrentPos(short axis )
        {
            double fValue = 0;
            double prfpos = 0;
            uint clk = 0;
            
            gts.mc.GT_GetPrfPos(axis, out prfpos, 1, out clk);                       

            double encpos = 0;
            gts.mc.GT_GetEncPos(axis, out encpos, 1, out clk);

            fValue = prfpos / AxisTestForm.AxisPulseEquivalent[axis - 1];

            return fValue;
        }

        public void SetWorkTrapPrm(int axis, gts.mc.TTrapPrm trapPrm, double fSpeed)
        {
            m_TtrapPrm[axis-1].acc = trapPrm.acc;
            m_TtrapPrm[axis-1].dec = trapPrm.dec;
            m_TtrapPrm[axis-1].smoothTime = trapPrm.smoothTime;
            m_TtrapPrm[axis-1].velStart = trapPrm.velStart;

            m_fSpeed[axis-1] = fSpeed;
        }

        public GtsPoint GetSafePoint( int nSizeType )
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_SafePoint);                       
            return gtsPoint;
        }

        public GtsPoint GetTakePhotoPoint(int nSizeType)
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_TakePhotoPoint);
            return gtsPoint;
        }

        public GtsPoint GetReverseMarkPoint(int nSizeType )
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_MarkRevertPoint);
            return gtsPoint;
        }

        public GtsPoint GetMarkPoint(int nSizeType)
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_MarkPoint);
            return gtsPoint;
        }

        public GtsPoint GetWorkPoint(int nSizeType ,int nBoxIndex, int nLayerIndex)
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_StartPoint[nBoxIndex] );

            gtsPoint.m_fz = gtsPoint.m_fz + m_gtsWorkStation[nSizeType].m_fzLayerOffset * nLayerIndex;
            gtsPoint.m_nz = (int)(gtsPoint.m_fz * AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS-1]);

            return gtsPoint;
        }

        public GtsPoint GetScanPoint(int nSizeType, int nBoxIndex, int nLayerIndex)
        {
            GtsPoint gtsPoint = new GtsPoint(m_gtsWorkStation[nSizeType].m_ScanStartPoint[nBoxIndex]);

            gtsPoint.m_fz = gtsPoint.m_fz + m_gtsWorkStation[nSizeType].m_fzLayerOffset * nLayerIndex;
            gtsPoint.m_nz = (int)(gtsPoint.m_fz * AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS - 1]);

            return gtsPoint;
        }

        internal void P2PMoveSlow(short axis, double fPos, Boolean bWait = false)
        {
            gts.mc.TTrapPrm trapPrm = new gts.mc.TTrapPrm();

            gts.mc.GT_PrfTrap(axis);

            gts.mc.GT_GetTrapPrm(axis, out trapPrm);


            trapPrm.acc = m_TtrapPrm[axis - 1].acc/2;
            trapPrm.dec = m_TtrapPrm[axis - 1].dec/2;
            trapPrm.smoothTime = m_TtrapPrm[axis - 1].smoothTime;
            trapPrm.velStart = 0;

            int nPos = (int)(fPos * AxisTestForm.AxisPulseEquivalent[axis - 1]);

            gts.mc.GT_SetTrapPrm(axis, ref trapPrm);//设置点位运动参数
            gts.mc.GT_SetVel(axis, m_fSpeed[axis - 1]/4);//设置目标速度
            gts.mc.GT_SetPos(axis, nPos);//设置目标位置

            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动

            if (bWait)
            {
                while (IsMoveFinished(axis) == false)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

        }

        public void P2PMove(short axis, double fPos, Boolean bWait = false)
        {
            gts.mc.TTrapPrm trapPrm = new gts.mc.TTrapPrm();

            gts.mc.GT_PrfTrap(axis);

            gts.mc.GT_GetTrapPrm(axis, out trapPrm);

          
            trapPrm.acc = m_TtrapPrm[axis - 1].acc;
            trapPrm.dec = m_TtrapPrm[axis - 1].dec;
            trapPrm.smoothTime = m_TtrapPrm[axis - 1].smoothTime;
            trapPrm.velStart = 0;         

            int nPos = (int)(fPos * AxisTestForm.AxisPulseEquivalent[axis - 1]);

            gts.mc.GT_SetTrapPrm(axis, ref trapPrm);//设置点位运动参数
            gts.mc.GT_SetVel(axis, m_fSpeed[axis-1]);//设置目标速度
            gts.mc.GT_SetPos(axis, nPos);//设置目标位置

            gts.mc.GT_Update(1 << (axis - 1));//更新轴运动

            if( bWait )
            {
                while (IsMoveFinished(axis) == false)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            
        }

        public void MoveToSafePoint(int nSizeType , short axis )
        {
            //move x
            GtsPoint gtsPoint = GetSafePoint(nSizeType);

            double fPos = gtsPoint.m_fx;
            if(axis == AxisTestForm.STRECH_AXIS )
            {
                P2PMove(axis, fPos, true);
            }
            else if (axis == AxisTestForm.UPDN_AXIS)
            {
                fPos = gtsPoint.m_fz;
                P2PMove(axis, fPos, true);
            }
            else if (axis == AxisTestForm.ROTATE_AXIS)
            {
                fPos = gtsPoint.m_fr;
                P2PMove(axis, fPos, true);
            }         

        }

        public void MoveToSafePoint(int nSizeType)
        {
            //move x
            GtsPoint gtsPoint = GetSafePoint(nSizeType);
            double fPos = gtsPoint.m_fx;
            P2PMove(AxisTestForm.STRECH_AXIS, fPos, true);

            //move z
            fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);

            //move r            
            fPos = gtsPoint.m_fr;
            P2PMove(AxisTestForm.ROTATE_AXIS, fPos, true);

        }

        internal void MoveToStartPoint(int nSizeType , int nIndex )
        {
            // move x to safe 
            MoveToSafePoint(AxisTestForm.STRECH_AXIS);

            //move z to safe 
            MoveToSafePoint(AxisTestForm.UPDN_AXIS);

            GtsPoint gtsPoint = m_gtsWorkStation[nSizeType].m_StartPoint[nIndex];
            //move r            
            double fPos = gtsPoint.m_fr;
            P2PMove(AxisTestForm.ROTATE_AXIS, fPos, true);

            //move z
            fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);

            //move x
            fPos = gtsPoint.m_fx;
            //P2PMove(AxisTestForm.STRECH_AXIS, fPos, true);
        }

        

        internal void MoveToScanPoint(int nSizeType,  int nIndex )
        {
            // move x to safe 
            MoveToSafePoint(AxisTestForm.STRECH_AXIS);

            //move z to safe 
            MoveToSafePoint(AxisTestForm.UPDN_AXIS);

            GtsPoint gtsPoint = m_gtsWorkStation[nSizeType].m_ScanStartPoint[nIndex];
            //move r            
            double fPos = gtsPoint.m_fr;
            //P2PMove(AxisTestForm.ROTATE_AXIS, fPos, true);

            //move z
            fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);

            //move x
            fPos = gtsPoint.m_fx;
            P2PMove(AxisTestForm.STRECH_AXIS, fPos, true);
            
        }

        internal void MoveToMarkRevertPoint(int nSizeType)
        {
            // move x to safe 
            MoveToSafePoint(AxisTestForm.STRECH_AXIS);

            //move z to safe 
            MoveToSafePoint(AxisTestForm.UPDN_AXIS);

            GtsPoint gtsPoint = m_gtsWorkStation[nSizeType].m_MarkRevertPoint;

            //move r            
            double fPos = gtsPoint.m_fr;
            P2PMove(AxisTestForm.ROTATE_AXIS, fPos, true);

            //move z
            fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);

            //move x
            fPos = gtsPoint.m_fx;
            P2PMove(AxisTestForm.STRECH_AXIS, fPos, true);
        }

        internal void MoveToMarkPoint(int nSizeType )
        {
            MoveToMarkRevertPoint(nSizeType);
            GtsPoint gtsPoint = m_gtsWorkStation[nSizeType].m_MarkPoint;
            //move z
            double fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);
           
        }

        internal void MoveToPhotoPoint(int nSizeType )
        {
            // move x to safe 
            MoveToSafePoint(AxisTestForm.STRECH_AXIS);

            //move z to safe 
            MoveToSafePoint(AxisTestForm.UPDN_AXIS);

            GtsPoint gtsPoint = m_gtsWorkStation[nSizeType].m_TakePhotoPoint;
            //move r            
            double fPos = gtsPoint.m_fr;
            P2PMove(AxisTestForm.ROTATE_AXIS, fPos, true);

            //move z
            fPos = gtsPoint.m_fz;
            P2PMove(AxisTestForm.UPDN_AXIS, fPos, true);

            //move x
            fPos = gtsPoint.m_fx;
            P2PMove(AxisTestForm.STRECH_AXIS, fPos, true);
        }


        public Boolean IsMoveFinished(short axis)
        {
            Boolean bRet = false;
            // 读取轴状态
            uint clk = 0;
            int lAxisStatus = 0;
            gts.mc.GT_GetSts(axis, out lAxisStatus, 1, out clk);

            // 规划器正在运动标志
            if ((lAxisStatus & 0x400) != 0)
            {
                bRet = false;
            }
            else
            {
                bRet = true;
            }
            return bRet;

        }        

        public void MoveZ(double fy)
        {

        }

        public void MoveR(double fr)
        {

        }

        public object SetDoValue(short mdl, short ioIndex, int nValue)
        {
            short nRet = 0;
            if (mdl == -1)
            {
                nRet = SetDo(ioIndex, nValue);
            }
            else
            {
                nRet = SetExtDo(mdl, ioIndex, nValue);
            }

            object ob = false;
            if (nRet > 0)
            {
                ob = true;
            }
            return ob;
        }

        public short GetHomeDi(short axis)
        {
            short sRtn = 0;
            int lGpiValue = 0;

            int ioIndex = axis - 1;

            sRtn = gts.mc.GT_GetDi(gts.mc.MC_HOME, out lGpiValue);

            // MessageBox.Show( lGpiValue.ToString() + "," + ioIndex.ToString() );

            if ((lGpiValue & (1 << ioIndex)) != 0)//高电平
            {
                sRtn = 1;
            }
            return sRtn;
        }

        public Boolean IsAxisOrig(short axis)
        {
            Boolean bOrig = false;

            if (GetHomeDi(axis) > 0)
            {
                bOrig = true;
            }

            return bOrig;
        }

        public object GetDiValue(short mdl, short ioIndex)
        {
            short nRet = 0;
            if (mdl == -1)
            {
                nRet = GetDi(ioIndex);
            }
            else
            {
                nRet = GetExtDi(mdl, ioIndex);
            }

            object ob = false;
            if (nRet > 0)
            {
                ob = true;
            }

            return ob;
        }

        public short GetAllDi(out int lGpiValue)
        {
            short sRtn = 0;
            sRtn = gts.mc.GT_GetDi(gts.mc.MC_GPI, out lGpiValue);         
            return sRtn;
        }

        public short GetDi( int ioIndex)
        {
            short sRtn = 0;
            int lGpiValue = 0;
            sRtn = gts.mc.GT_GetDi(gts.mc.MC_GPI, out lGpiValue);
            if ((lGpiValue & (1 << ioIndex)) != 0)//高电平
            {
                sRtn = 1;
                //Console.WriteLine("按键已经按下，不检测\n");
            }                
            return sRtn;
        }

        public void OperateClearLight()
        {
            SetDo(AxisTestForm.YellowLightIOOUT, 0);
            SetDo(AxisTestForm.GreenLightIOOUT, 0);
            SetDo(AxisTestForm.RedLightIOOUT, 0);
            SetDo(AxisTestForm.BeepIOOUT, 0);
        }

        public short SetDo(int ioIndex , int nValue)
        {
            short sRtn = 0;
            int lpValue = 0;
            sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lpValue);
            if( nValue == 0 )
            {
                lpValue = (lpValue | (1 << ioIndex));
            }
            else
            {
                lpValue = (lpValue & ~(1 << ioIndex));
            }
            
            sRtn = gts.mc.GT_SetDo(gts.mc.MC_GPO,  lpValue);
         
            return sRtn;
        }

        public short SetExtDo(short mdl, int ioIndex, int nValue)
        {
            short sRtn = 0;
            ushort lpValue = 0;
            short io = (short)ioIndex;

            if (nValue > 0)
            {
                lpValue = 0;
            }
            else
            {
                lpValue = 1;
            }

            sRtn = gts.mc.GT_SetExtIoBit(mdl, io, lpValue);

            return sRtn;
        }
        public short GetAllExtDi(short mdl, out ushort lGpiValue)
        {
            short sRtn = 0;
            sRtn = gts.mc.GT_GetExtIoValue(mdl, out lGpiValue);
            return sRtn;
        }

        public short GetExtDi(short mdl, int ioIndex)
        {
            short sRtn = 0;
            ushort lGpiValue = 0;
            sRtn = gts.mc.GT_GetExtIoValue(mdl, out lGpiValue);
            if ((lGpiValue & (1 << ioIndex)) == 0)//高电平
            {
                sRtn = 1;
                //Console.WriteLine("按键已经按下，不检测\n");
            }
            return sRtn;
        }

        
        public Boolean OperateWorkState()
        {
            Boolean bRet = false;
            short nRet = 0;

            nRet = GetDi(AxisTestForm.StopIOIN);
            if (nRet > 0)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;
                OperateClearLight();
                SetDo(AxisTestForm.YellowLightIOOUT, 1);
                return bRet;
            }


            nRet = GetDi(AxisTestForm.StartIOIN);
            if (nRet > 0)
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;
                OperateClearLight();
                SetDo(AxisTestForm.GreenLightIOOUT, 1);

                m_nWorkState = AxisTestForm.WORK_AUTO;
                return false;
            }        

            if(m_nWorkState == AxisTestForm.WORK_AUTO)
            {
                return bRet;
            }

            if ( m_nWorkState != AxisTestForm.WORK_RESET)
            {
                nRet = GetDi(AxisTestForm.ResetIOIN);
                if (nRet > 0 )
                {
                    m_nTestFun = 0;
                    
                    ResetWork();
                    return false;
                }
            }

            // 伺服报警标志
            if (IsServoWarning() )
            {
                m_nWorkTestMode = 0;
                string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                m_warnForm.SetInfoText(strInfo);
                m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                m_warnForm.ShowDialog();
                return bRet;
            }

            if (IsNormalWarning())
            {
                string strInfo = "限位或急停报警,确定后请重新复位！";
                m_warnForm.SetInfoText(strInfo);
                m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                m_warnForm.ShowDialog();
                return bRet;
            }            
            return bRet;            
        }

        public Boolean IsRevertOrigPos()
        {
            Boolean bRet = false;
            short nRet = GetDi(AxisTestForm.RevertGasIOIN1);

            if (nRet > 0)
            {
                bRet = true;
            }
            return bRet;
        }

        public Boolean IsRevertWorkPos()
        {
            Boolean bRet = false;
            short nRet = GetDi(AxisTestForm.RevertGasIOIN2);

            if (nRet > 0)
            {
                bRet = true;
            }
            return bRet;
        }

        public Boolean IsExitChip()
        {
            Boolean bRet = false;
            short nValue = GetDi(AxisTestForm.ScanIOIN);
            if (nValue > 0 )
            {
                //等到扫描结果
                bRet = true;
            }            
            return bRet;
        }
        
        public Boolean IsXiliao()
        {
            Boolean bRet = false;
            short nRet = GetDi(AxisTestForm.XiLiaoIOIN);

            int lGpiValue;
            short sRtn = gts.mc.GT_GetDo(gts.mc.MC_GPO, out lGpiValue);

            int nEnableDo = 0;
            int ioIndex = AxisTestForm.XiLiaoIOOUT;
      
            //16 IO
            if ((lGpiValue & (1 << ioIndex)) == nEnableDo)//高电平
            {
                if (nRet > 0)
                {
                    bRet = true;
                }
            }           
            
            return bRet;
        }

        public void OperateXiliao( Boolean bXiliao)
        {            
            if ( bXiliao )
            {
                SetDo(AxisTestForm.XiLiaoIOOUT, 1);

            }
            else
            {
                SetDo(AxisTestForm.XiLiaoIOOUT, 0);
            }
        }

        public void OperateRevertGas(Boolean bOrigPos)//双向气缸
        {

            if( bOrigPos )
            {
                SetDo(AxisTestForm.RevertGasIOOUTOrig, 0);
                SetDo(AxisTestForm.RevertGasIOOUTWork, 1);
            }
            else
            {
                SetDo(AxisTestForm.RevertGasIOOUTWork, 0);
                SetDo(AxisTestForm.RevertGasIOOUTOrig, 1);                
            }
        }

        public Boolean IsMaterialExit(int nBoxIndex)
        {
            Boolean bExit = true;
            int nRet1 = GetExtDi(1, AxisTestForm.BoxExitExIOIN1A + nBoxIndex);
            int nRet2 = GetExtDi(1, AxisTestForm.BoxExitExIOIN1B + nBoxIndex);
            if (nRet1 == 0 && nRet2 == 0)
            {
                bExit = false;
            }
            return bExit;
        }
        public void RefreshBoxLight()
        {
            for(int i = 0; i < 8; i++ )
            {
                int nRet1 = GetExtDi(1, AxisTestForm.BoxExitExIOIN1A + i);
                int nRet2 = GetExtDi(1, AxisTestForm.BoxExitExIOIN1B + i);
                if( nRet1 == 0 && nRet2 == 0 )
                {
                    RemoveWorkIndex(i);
                }
            }

            for (int i = 0; i < AxisTestForm.BOX_COUNT; i++)
            {                
                 SetDo(AxisTestForm.READY_LIGHTSB1 + i, 0);                
            }

            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_WAITSTATE || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNSTATE
                    || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNNINGSTATE )
                {
                    int nBoxIndex = m_WorkReadyBoxList.m_BoxList[i].X;
                    SetDo(AxisTestForm.READY_LIGHTSB1 + nBoxIndex, 1);
                }else if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_FINISHSTATE)
                {
                    int nBoxIndex = m_WorkReadyBoxList.m_BoxList[i].X;

                    SetDo(AxisTestForm.READY_LIGHTSB1 + nBoxIndex, 1);
                    MyCommanLib.CommonDefine.TickSleep(1000);
                    SetDo(AxisTestForm.READY_LIGHTSB1 + nBoxIndex, 0);
                    MyCommanLib.CommonDefine.TickSleep(1000);
                }
            }
        }

        public int RemoveWorkIndex(int nBoxIndex)
        {
            int nRet = 0;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (m_WorkReadyBoxList.m_BoxList[i].X == nBoxIndex )
                {
                    m_WorkReadyBoxList.m_BoxList.RemoveAt(i);
                    nRet = nBoxIndex;
                    break;
                }
            }
            return nRet;
        }

        public int FindFirstWorkIndex()
        {
            int nBoxIndex = -1;
            for (int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if (m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_WAITSTATE || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNSTATE
                    || m_WorkReadyBoxList.m_BoxList[i].Y == AxisTestForm.BOXCHIP_RUNNINGSTATE )
                {
                    nBoxIndex = i;
                    break;
                }
            }
            return nBoxIndex;
        }

        public void AddBoxIndex2ReadyList(int nBoxIndex)
        {
            Boolean bFind = false;
            for(int i = 0; i < m_WorkReadyBoxList.m_BoxList.Count; i++)
            {
                if( nBoxIndex == m_WorkReadyBoxList.m_BoxList[i].X )
                {
                    bFind = true;
                    break;
                }
            }

            if( bFind == false )
            {
                Point pp = new Point(nBoxIndex, AxisTestForm.BOXCHIP_WAITSTATE);
                m_WorkReadyBoxList.m_BoxList.Add(pp);
            }
        }

        public Boolean OperateReadyMaterials()
        {
            Boolean bRet = false;
            //if (m_nWorkState != gts.mc.WORK_AUTO)
            //    return bRet;

            //int lAxisStatus = 0;
            //uint clk;
            //gts.mc.GT_GetSts(AxisTestForm.STRECH_AXIS, out lAxisStatus, 1, out clk);
            //// 伺服报警标志  正向限位 负向限位   平滑停止  紧急停止
            //if ((lAxisStatus & 0x2) != 0 || (lAxisStatus & 0x20) != 0 || (lAxisStatus & 0x40) != 0
            //    || (lAxisStatus & 0x80) != 0 || (lAxisStatus & 0x100) != 0 || (lAxisStatus & 0x200) != 0)
            //{
            //    return bRet;
            //}

            for(int i = 0; i < 8; i++)
            {
                short nRet = GetExtDi(0,AxisTestForm.BoxReadyExIOIN1 + i);
                short nRet2 = GetExtDi(1,AxisTestForm.BoxExitExIOIN1A + i);
                short nRet3 = GetExtDi(1,AxisTestForm.BoxExitExIOIN1B + i);

                if (nRet > 0 && ( nRet2 > 0 || nRet3 > 0 ))
                {
                    AddBoxIndex2ReadyList(i);                    
                }
            }            

            return bRet;
        }

        public Boolean TakePhoto()
        {
            Boolean bRet = true;      

            MyCommanLib.CommonDefine.InitialMarkStruct(ref MyCommanLib.CommonDefine.m_PosStruct);
            MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoStart = 1;
            MyCommanLib.CommonDefine.WriteMemeryFile(MyCommanLib.CommonDefine.m_posMemeryFile, MyCommanLib.CommonDefine.m_PosStruct);
            
            int nStartTickCount = System.Environment.TickCount;


            int dt = 0;
            while (MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd == 0)
            {
                MyCommanLib.CommonDefine.ReadMemeryFile(MyCommanLib.CommonDefine.m_posMemeryFile, ref MyCommanLib.CommonDefine.m_PosStruct);

                dt = CommonDefine.DelayedTime(nStartTickCount);

                if (dt > 5000)
                {
                    break;
                }
            }
            

            if (MyCommanLib.CommonDefine.m_PosStruct.m_nPaizhaoEnd != 0)
            {
                if (MyCommanLib.CommonDefine.m_PosStruct.m_nResult == 1)
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
            return bRet;
        }

        public Boolean IsServoWarning()
        {
            Boolean bRet = false;
            int lAxisStatus = 0;
            uint clk;
            for (short i = 1; i <= 3; i++  )
            {
                gts.mc.GT_GetSts(i, out lAxisStatus, 1, out clk);
                // 伺服报警标志 
                if ((lAxisStatus & 0x2) != 0)
                {
                    bRet = true;
                    break;
                }
            }
                
            return bRet;
        }
        public Boolean IsNormalWarning()
        {
            Boolean bRet = false;

            int lAxisStatus = 0;
            uint clk;
            for (short i = 1; i <= 3; i++)
            {
                if (AxisTestForm.Axis_InUse[i] == 0)
                    continue;
                gts.mc.GT_GetSts(i, out lAxisStatus, 1, out clk);
                // 正向限位 负向限位    紧急停止 未使能
                if (((lAxisStatus & 0x20) != 0 || (lAxisStatus & 0x40) != 0 || (lAxisStatus & 0x100) != 0))
                {
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }

        public Boolean OperateWorkFlow()
        {
            Boolean bRet = true;
           
            if (m_nWorkState != AxisTestForm.WORK_AUTO )
                return bRet;
                       

            // 伺服报警标志
            if (IsServoWarning())
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                //MessageBox.Show("伺服报警,请重新启动伺服驱动器,！");

                string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                m_warnForm.SetInfoText(strInfo);
                m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                m_warnForm.ShowDialog();
                return bRet;
            }

            if (IsNormalWarning())
            {
                m_nWorkState = AxisTestForm.WORK_MANUAL;

                //MessageBox.Show("限位或急停报警,请重新复位！");

                string strInfo = "限位或急停报警,确定后请重新复位！";
                m_warnForm.SetInfoText(strInfo);
                m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                m_warnForm.ShowDialog();
                return bRet;
            }
                        

            short nRet = 0;
            Point tempPoint = new Point(0,0);

            if( m_nWorkFlow == AxisTestForm.SCAN_VALUE )
            {
                if(m_WorkReadyBoxList.m_BoxList.Count <= 0 )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有可扫描的料！");
                    string strInfo = "没有可扫描的料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }
                

                int nListIndex = FindFirstWorkIndex();
                if (nListIndex >= m_WorkReadyBoxList.m_BoxList.Count || nListIndex < 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("扫描的料已完成！");
                    string strInfo = "扫描的料已完成,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                if (IsXiliao())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("本身有料，不可扫料！");
                    string strInfo = "本身有料，不可扫料！,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                int nBoxIndex = m_WorkReadyBoxList.m_BoxList[nListIndex].X;

                if (m_WorkReadyBoxList.m_BoxList[nListIndex].Y == AxisTestForm.BOXCHIP_WAITSTATE)
                {
                    tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_RUNNINGSTATE);
                    m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;

                    m_WorkChipList.m_LayerList.Clear();
                    m_WorkChipList.m_nBoxIndex = nBoxIndex;
                }

                nBoxIndex = m_WorkChipList.m_nBoxIndex;
                int nSizeType = m_WorkChipList.m_nSizeType;

                GtsPoint safePoint = GetSafePoint(nSizeType);
                //是否在安全位，不在，移动安全位
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                P2PMove(AxisTestForm.UPDN_AXIS, safePoint.m_fz, true);
                                
                
                int nStartLayer = m_WorkReadyBoxList.m_nCurrentChipIndex;
                //移动到新的点
                GtsPoint fPoint = GetScanPoint(nSizeType, nBoxIndex, nStartLayer);
                P2PMove(AxisTestForm.ROTATE_AXIS, fPoint.m_fr, true);

                P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);

                P2PMove(AxisTestForm.STRECH_AXIS, fPoint.m_fx, true);

                m_nWorkBoxIndex = nBoxIndex;

                for (int i = nStartLayer; i < AxisTestForm.BOX_CHIPSCOUNT; i++ )
                {
                    //检测急停
                    // 读取轴状态                    
                    // 伺服报警标志

                    if (IsServoWarning())
                    {
                        EventArgs ee = new EventArgs();
                        RefreshChipListUIHandler(new Point(), ee);
                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        //MessageBox.Show("伺服报警,请重新启动伺服驱动器！");

                        string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                        m_warnForm.SetInfoText(strInfo);
                        m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                        m_warnForm.ShowDialog();

                        return bRet;
                    }

                    if (IsNormalWarning())
                    {
                        EventArgs ee = new EventArgs();
                        RefreshChipListUIHandler(new Point(), ee);
                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        //MessageBox.Show("限位或急停报警,请重新复位！");

                        string strInfo = "限位或急停报警,确定后请重新复位！";
                        m_warnForm.SetInfoText(strInfo);
                        m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                        m_warnForm.ShowDialog();
                        return bRet;
                    }

                    fPoint = GetScanPoint(nSizeType, nBoxIndex, i);

                    //P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);
                    P2PMoveSlow(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);

                    //发送扫描信号
                    //SetDo(gts.mc.START_SCAN_OUT,1);
                    //MyCommanLib.CommonDefine.TickSleep(m_nXiliaoSleep);
                    //SetDo(gts.mc.START_SCAN_OUT, 0);

                    string strText = m_MarkChipsData[nBoxIndex, i].strMarkContent;
                    Boolean bCheck = m_MarkChipsData[nBoxIndex, i].bSelect;
                   
                    if ( bCheck && IsExitChip() )
                    {
                        //等到扫描结果
                        Point pp = new Point(i,AxisTestForm.BOXCHIP_WAITSTATE);
                        m_WorkChipList.m_LayerList.Add(pp);
                        m_WorkChipState[i] = AxisTestForm.BOXCHIP_WAITSTATE;
                    }
                    else
                    {
                        m_WorkChipState[i] = AxisTestForm.BOXCHIP_NOTSCANSTATE;
                        //MessageBox.Show("扫描超时，请检查");
                    }
                    
                    m_WorkReadyBoxList.m_nCurrentChipIndex = i + 1;                   

                }
                

                //存储扫码结果                
                tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_RUNSTATE);
                m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;
                m_WorkReadyBoxList.m_nCurrentChipIndex = 0;

                EventArgs e = new EventArgs();
                RefreshChipListUIHandler(new Point(), e);

                m_nWorkFlow = AxisTestForm.ROBOT_TAKE;

            }
            else if(m_nWorkFlow == AxisTestForm.ROBOT_TAKE)
            {
                if (m_WorkChipList.m_LayerList.Count <= 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.SCAN_VALUE;
                    //MessageBox.Show("没有可取的料！");

                    string strInfo = "没有可取的料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }
                

                int nBoxIndex = m_WorkChipList.m_nBoxIndex;
                if( IsMaterialExit( nBoxIndex) == false )
                {
                    m_WorkChipList.m_LayerList.Clear();
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.SCAN_VALUE;
                    //MessageBox.Show("没有可取的料！");

                    string strInfo = "没有可取的料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                if (IsXiliao())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("本身有料，不可吸料！");
                    string strInfo = "本身有料，不可吸料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }

                if (IsRevertOrigPos() == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到初始位,不可放料！");
                    string strInfo = "气缸没有到初始位,不可放料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                int nLayerIndex = m_WorkChipList.m_LayerList[0].X;
                int nSizeType = m_WorkChipList.m_nSizeType;
                m_nWorkBoxSize = nSizeType;

                GtsPoint fPoint = GetWorkPoint(nSizeType,nBoxIndex, nLayerIndex);

                m_nWorkLayerIndex = nLayerIndex;
                //是否在安全位，不在，移动安全位
                GtsPoint safePoint = m_gtsWorkStation[nSizeType].m_SafePoint;
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                //P2PMove(AxisTestForm.UPDN_AXIS, m_SafePoint.m_fz, true);

                m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_RUNNINGSTATE;


                // 伺服报警标志
                if (IsServoWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("伺服报警,请重新启动伺服驱动器！");
                    string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("限位或急停报警,请重新复位！");
                    string strInfo = "限位或急停报警,确定后请重新复位！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                //移动到新的点
                P2PMove(AxisTestForm.ROTATE_AXIS, fPoint.m_fr,true);

                P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);
                //伸出
                P2PMove(AxisTestForm.STRECH_AXIS, fPoint.m_fx, true);
                //抬起
                P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz + m_gtsWorkStation[nSizeType].m_fzTakeOffset, true);

                //取料，吸料
                //发送吸附信号
                SetDo(AxisTestForm.XiLiaoIOOUT, 1);
                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nXiliaoSleep);

               
                if( IsXiliao() == false && m_nWorkTestMode == 0 )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);

                    //MessageBox.Show("没有吸取到料！");
                    string strInfo = "没有吸取到料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                //SetDo(gts.mc.START_SUCTION_OUT, 0);
                //移动出来
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                EventArgs e = new EventArgs();
                Point pp = m_WorkChipList.m_LayerList[0];
                pp.Y = AxisTestForm.BOXCHIP_RUNNINGSTATE;
                RefreshChipListUIHandler(pp, e);                               

                m_nWorkFlow = AxisTestForm.PHOTO_VALUE;
            }
            else if (m_nWorkFlow == AxisTestForm.PHOTO_VALUE)
            {
                if (IsXiliao() == false && m_nWorkTestMode == 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.ROBOT_TAKE;
                    //MessageBox.Show("本身无料，不可拍照，按开始重新取料！");
                    string strInfo = "本身无料，不可拍照,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
     
                    return false;
                }


                //是否在安全位，不在，移动安全位
                int nSizeType = m_nWorkBoxSize;
                GtsPoint safePoint = m_gtsWorkStation[nSizeType].m_SafePoint;
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                //P2PMove(AxisTestForm.UPDN_AXIS, m_SafePoint.m_fz, true);


                // 伺服报警标志
                if (IsServoWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("伺服报警,请重新启动伺服驱动器！");
                    string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("限位或急停报警,请重新复位！");
                    string strInfo = "限位或急停报警,确定后请重新复位！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                //移动到新的点的过度点
                GtsPoint takePoint = m_gtsWorkStation[nSizeType].m_TakePhotoPoint;
                P2PMove(AxisTestForm.UPDN_AXIS, takePoint.m_fz, true);

                //移动到新的点的过度点
                P2PMove(AxisTestForm.ROTATE_AXIS, takePoint.m_fr, true);

                //移动到新点 伸进 
                P2PMove(AxisTestForm.STRECH_AXIS, takePoint.m_fx, true);

                //发送翻转信号
                OperateRevertGas(false);

                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nRevertSleep);

           
                if (IsRevertWorkPos() == false   )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到翻转位！");

                    string strInfo = "没有到翻转位,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                //发送拍照信号
                if( m_nWorkTestMode == 0 )
                {
                    if (TakePhoto() == false)
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        //MessageBox.Show("拍照失败，请检查！");
                        string strInfo = "拍照失败，请检查！,请准备好后按开始！";
                        m_warnForm.SetInfoText(strInfo);
                        m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                        m_warnForm.ShowDialog();
                        return false;
                    }
                }

                //翻转回来
                OperateRevertGas(true);
                
                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nRevertSleep);
                                
                if (IsRevertOrigPos()==false  )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到初始位！");
                    string strInfo = "没有到初始位,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }

                //移动出来
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                m_nWorkFlow = AxisTestForm.MARK_VALUE;
            }
            else if (m_nWorkFlow == AxisTestForm.MARK_VALUE)
            {
                if (IsXiliao() == false && m_nWorkTestMode == 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.ROBOT_TAKE;
                    //MessageBox.Show("本身无料，不可打标，按开始重新取料！");
                    string strInfo = "本身无料，不可打标,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }

                //是否在安全位，不在，移动安全位
                int nSizeType = m_nWorkBoxSize;
                GtsPoint safePoint = m_gtsWorkStation[nSizeType].m_SafePoint;
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                // 伺服报警标志
                if (IsServoWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("伺服报警,请重新启动伺服驱动器！");
                    string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("限位或急停报警,请重新复位！");
                    string strInfo = "限位或急停报警,确定后请重新复位！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                if (IsRevertOrigPos() == false)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到初始位,不可放料！");
                    string strInfo = "没有到初始位,不可放料,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    OperateRevertGas(true);
                    MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nRevertSleep);

                    return false;
                }

                //移动到新的点的过度点                
                GtsPoint markRevertPoint = GetReverseMarkPoint(nSizeType);

                P2PMove(AxisTestForm.UPDN_AXIS, markRevertPoint.m_fz, true);
                P2PMove(AxisTestForm.ROTATE_AXIS, markRevertPoint.m_fr, true);


                //移动到新点 伸进 
                GtsPoint markPoint = GetReverseMarkPoint(nSizeType);
                P2PMove(AxisTestForm.STRECH_AXIS, markPoint.m_fx, true);


                //发送翻转信号
                OperateRevertGas(false);
                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nRevertSleep);

                
                if (IsRevertWorkPos() == false  )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到翻转位！");
                    string strInfo = "没有到翻转位,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }

                //抬起
                P2PMove(AxisTestForm.UPDN_AXIS, markPoint.m_fz, true);

                //mark
                if( m_gtsForm != null )
                {
                    //if( m_gtsForm.markForm.MarkPostion() )
                    int nBoxIndex = m_WorkChipList.m_nBoxIndex;
                    int nLayerIndex = m_WorkChipList.m_LayerList[0].X;
                    string strText = m_MarkChipsData[nBoxIndex,nLayerIndex].strMarkContent;
                    Boolean bSucess = false;
                    try
                    {
                        int nCard = m_gtsForm.markForm.m_nAddress0Card;
                        bSucess = m_gtsForm.markForm.Mark(nCard, strText);
                    }catch
                    {
                        //bSucess = true;
                    }
                    
                    if (bSucess)
                    {
                        
                    }
                    else
                    {
                        m_nWorkState = AxisTestForm.WORK_MANUAL;
                        //MessageBox.Show("打标失败！");
                        string strInfo = "打标失败,请准备好后按开始！";
                        m_warnForm.SetInfoText(strInfo);
                        m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                        m_warnForm.ShowDialog();
                        return false;
                    }
                }
                else
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("打标失败！");
                    string strInfo = "打标失败,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

                //降低
                //P2PMove(AxisTestForm.UPDN_AXIS, m_SafePoint.m_fz, true);

                //翻转回来
                OperateRevertGas(true);
                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nRevertSleep);


                if (IsRevertOrigPos()==false )
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("没有到初始位！");
                    string strInfo = "没有到初始位,请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

               
                //移动出来
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);
                m_nWorkFlow = AxisTestForm.PUT_VALUE;
            }
            else if (m_nWorkFlow == AxisTestForm.PUT_VALUE)
            {
                if (IsXiliao() == false && m_nWorkTestMode == 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.ROBOT_TAKE;
                    //MessageBox.Show("本身无料，不可放料，按开始重新取料！");
                    string strInfo = "本身无料，不可放料，请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();

                    return false;
                }



                //是否在安全位，不在，移动安全位
                int nSizeType = this.m_nWorkBoxSize;
                GtsPoint safePoint = m_gtsWorkStation[nSizeType].m_SafePoint;
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                //P2PMove(AxisTestForm.UPDN_AXIS, m_SafePoint.m_fz, true);

                int nBoxIndex = m_WorkChipList.m_nBoxIndex;
                int nLayerIndex = m_WorkChipList.m_LayerList[0].X;
                GtsPoint fPoint = GetWorkPoint(nSizeType,nBoxIndex, nLayerIndex);


                // 伺服报警标志
                if (IsServoWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("伺服报警,请重新启动伺服驱动器！");
                    string strInfo = "伺服报警,请重新启动伺服驱动器,然后按确定按钮后按复位";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                if (IsNormalWarning())
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    //MessageBox.Show("限位或急停报警,请重新复位！");
                    string strInfo = "限位或急停报警,确定后请重新复位！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return bRet;
                }

                //移动到新的点的过度点
                P2PMove(AxisTestForm.ROTATE_AXIS, fPoint.m_fr, true);
                //抬起
                P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz + m_gtsWorkStation[nSizeType].m_fzTakeOffset, true);

                //移动到新点 伸进 
                P2PMove(AxisTestForm.STRECH_AXIS, fPoint.m_fx, true);

                //发送松开
                SetDo(AxisTestForm.XiLiaoIOOUT, 0);
                MyCommanLib.CommonDefine.TickSleep(m_gtsWorkStation[nSizeType].m_nXiliaoSleep);

                if (IsXiliao() && m_nWorkTestMode == 0)
                {
                    m_nWorkState = AxisTestForm.WORK_MANUAL;
                    m_nWorkFlow = AxisTestForm.ROBOT_TAKE;
                    //MessageBox.Show("没有释放掉料！");
                    string strInfo = "没有释放掉料，请准备好后按开始！";
                    m_warnForm.SetInfoText(strInfo);
                    m_warnForm.m_nIOAdd = AxisTestForm.RedLightIOOUT;
                    m_warnForm.ShowDialog();
                    return false;
                }

               

                //下降
                P2PMove(AxisTestForm.UPDN_AXIS, fPoint.m_fz, true);

                //移动出来                
                P2PMove(AxisTestForm.STRECH_AXIS, safePoint.m_fx, true);

                Point pp = m_WorkChipList.m_LayerList[0];                
                m_WorkChipList.m_LayerList.RemoveAt(0);

                m_WorkChipState[nLayerIndex] = AxisTestForm.BOXCHIP_FINISHSTATE;

                EventArgs e = new EventArgs();
                pp.Y = AxisTestForm.BOXCHIP_FINISHSTATE;
                RefreshChipListUIHandler(pp, e);

                int nListIndex = FindFirstWorkIndex();

                if (m_WorkChipList.m_LayerList.Count > 0 )
                {
                    m_nWorkFlow = AxisTestForm.ROBOT_TAKE;
                }else
                {
                    m_nWorkFlow = AxisTestForm.SCAN_VALUE;
                    
                    //存储扫码结果                
                    tempPoint = new Point(m_WorkReadyBoxList.m_BoxList[nListIndex].X, AxisTestForm.BOXCHIP_FINISHSTATE);
                    m_WorkReadyBoxList.m_BoxList[nListIndex] = tempPoint;
                }
                
            }
            return bRet;
        }
        
    }

    public class ChipBoxStruct
    {
        public int m_nSizeType = 0; // 0-1 0: 6-Inch  1:4-Inch
        public int m_nBoxIndex = 0;//0 - 7        
        public List<Point> m_LayerList = new List<Point>();//.X layerindex Y 状态
    }

    public class ChipBoxList
    {
        public int m_nSizeType = 0; //0 - 1 0: 6-Inch 1: 4-Inch
        public int m_nCurrentChipIndex = 0;
        public List<Point> m_BoxList = new List<Point>();//.X boxindex Y 状态
    }

    public class GtsPoint
    {
        //距离
        public double m_fx { get; set; }
        public double m_fy { get; set; }
        public double m_fz { get; set; }
        public double m_fr { get; set; }
        //脉冲
        public long m_nx { get; set; }
        public long m_ny { get; set; }
        public long m_nz { get; set; }
        public long m_nr { get; set; }

        public GtsPoint()
        {
            m_fx = 0;
            m_fy = 0;
            m_fz = 0;
            m_fr = 0;
        }

        public GtsPoint(GtsPoint srcPoint)
        {
            m_fx = srcPoint.m_fx;
            m_fy = srcPoint.m_fy;
            m_fz = srcPoint.m_fz;
            m_fr = srcPoint.m_fr;

            m_nx = (int)(m_fx * AxisTestForm.AxisPulseEquivalent[AxisTestForm.STRECH_AXIS - 1]);

            m_nz = (int)(m_fz * AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS - 1]);
            m_nr = (int)(m_fr * AxisTestForm.AxisPulseEquivalent[AxisTestForm.ROTATE_AXIS - 1]);
        }

        public void SetGtsPoint( double fx, double fy , double fz , double fr)
        {
            m_fx = fx;
            m_fy = fy;
            m_fz = fz;
            m_fr = fr;

            m_nx = (int)(m_fx * AxisTestForm.AxisPulseEquivalent[AxisTestForm.STRECH_AXIS - 1]);

            m_nz = (int)(m_fz * AxisTestForm.AxisPulseEquivalent[AxisTestForm.UPDN_AXIS - 1]);
            m_nr = (int)(m_fr * AxisTestForm.AxisPulseEquivalent[AxisTestForm.ROTATE_AXIS - 1]);
        }
    }

    public class GtsWorkStation
    {
        public GtsPoint m_SafePoint = new GtsPoint();
        public GtsPoint m_TakeMaterialPoint = new GtsPoint();
        public GtsPoint m_TakePhotoPoint = new GtsPoint();
        public GtsPoint m_MarkPoint = new GtsPoint();
        public GtsPoint m_MarkRevertPoint = new GtsPoint();

        public GtsPoint[] m_StartPoint = new GtsPoint[8];
        public GtsPoint[] m_ScanStartPoint = new GtsPoint[8];

        public double m_fzLayerOffset;
        public double m_fzTakeOffset;

        public int m_nXiliaoSleep = 500;
        public int m_nRevertSleep = 500;

        public GtsWorkStation()
        {
            for (int i = 0; i < 8; i++)
            {
                m_StartPoint[i] = new GtsPoint();
                m_ScanStartPoint[i] = new GtsPoint();
            }
        }

        public void InitialData()
        {

        }
    }
}
