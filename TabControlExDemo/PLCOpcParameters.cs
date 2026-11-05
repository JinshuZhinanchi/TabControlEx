using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace TabControlExDemo
{
    #region 基础结构
    /// <summary>
    /// Motor Base 
    /// </summary>
    public class motorBaseControl
    {
        public int rotateAdd;
        public int rotateMinus;

        public int home;

        public int safe;
        public int work;

        public void ResetIndexValue(ref int nIndex)
        {
            rotateAdd = nIndex++;
            rotateMinus = nIndex++;

            home = nIndex++;
            safe = nIndex++;
            work = nIndex++;
        }
    }

    public class motorBaseParam
    {

        public int manuSpeed;
        public int autoSpeed;
        public int currentPos;

        public int safe;
        public int work;

        public void ResetIndexValue(ref int nIndex)
        {
            manuSpeed = nIndex++;
            autoSpeed = nIndex++;
            currentPos = nIndex++;

            safe = nIndex++;
            work = nIndex++;

        }
    }

    public class motorBaseState
    {
        public int home;
        public int homming;
        public int safe;
        public int work;

        public void ResetIndexValue(ref int nIndex)
        {
            home = nIndex++;
            homming = nIndex++;
            safe = nIndex++;
            work = nIndex++;
        }
    }
    #endregion

    public static class PLCOpcFeedParameters
    {
        
        public static class PLCConstDefine
        {
            //Robot X 位置
            public static short ROBOT_MBOX_POS = 1;
            public static short ROBOT_SMIF1_POS = 2;
            public static short ROBOT_SMIF2_POS = 3;
            public static short ROBOT_SBOX_POS = 4;
        }
            


        public static class IOInState
        {
            public static int nIndex = 0;
            public static int StartX0 = nIndex++;
            public static int StopX2 = nIndex++;
            public static int ResetX4 = nIndex++;
            public static int EStopX6 = nIndex++;
            //4
            public static int MBoxConfirm1X10 = nIndex++;
            public static int MBoxConfirm2X12 = nIndex++;
            //6
            public static int MBoxExit1_AX14 = nIndex++;
            public static int MBoxExit2_AX16 = nIndex++;
            public static int MBoxExit1_BX20 = nIndex++;
            public static int MBoxExit2_BX22 = nIndex++;
            //10

            public static int RobotRunningX24 = nIndex++;
            public static int RobotNGX26 = nIndex++;
            public static int AR_RobotXiFu27 = nIndex++;
                        
            public static int StopState = nIndex++;//停止按钮：该值为1 代表常闭  为0 代表常开
            public static int TestPower = nIndex++;//检测功率：该值为1 开启检测功率  为0 不开启检测功率

            //功率测试气缸            
            public static int gasOrig = nIndex++; //气缸原点位
            public static int gasWork = nIndex++; //气缸工作位

            //17

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                StartX0 = nIndex++;
                StopX2 = nIndex++;
                ResetX4 = nIndex++;
                EStopX6 = nIndex++;

                MBoxConfirm1X10 = nIndex++;
                MBoxConfirm2X12 = nIndex++;

                MBoxExit1_AX14 = nIndex++;
                MBoxExit2_AX16 = nIndex++;
                MBoxExit1_BX20 = nIndex++;
                MBoxExit2_BX22 = nIndex++;


                RobotRunningX24 = nIndex++;
                RobotNGX26 = nIndex++;
                AR_RobotXiFu27 = nIndex++;
                
                StopState = nIndex++;
                TestPower = nIndex++;//检测功率：该值为1 开启检测功率  为0 不开启检测功率                
                gasOrig = nIndex++;
                gasWork = nIndex++;

                

            }

        }//17

        public static class IOOutState
        {
            public static int nIndex = 0;

            public static int TowerYellowLight = nIndex++;            
            public static int TowerRedLight = nIndex++;
            public static int TowerGreenLight = nIndex++;
            public static int TowerBuzzer = nIndex++;
            //4
            public static int MBoxconfirmLight1 = nIndex++;
            public static int MBoxconfirmLight2 = nIndex++;
            //6       
            public static int gasOperate = nIndex++;
            public static int checkReadOCR = nIndex++;

            public static int checkSafeDoor = nIndex++;
            public static int reserved1 = nIndex++;
            public static int reserved2 = nIndex++;
            public static int reserved3 = nIndex++;

            //门上升延时
            public static int doorOpenSleep = nIndex++;
            public static int doorCloseSleep = nIndex++;
            public static int vacuumOperate = nIndex++;

            public static int MBoxExit1_C = nIndex++;
            public static int MBoxExit2_C = nIndex++;

            //13
            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;

                TowerYellowLight = nIndex++;                
                TowerRedLight = nIndex++;
                TowerGreenLight = nIndex++;
                TowerBuzzer = nIndex++;

                MBoxconfirmLight1 = nIndex++;
                MBoxconfirmLight2 = nIndex++;

                gasOperate = nIndex++; 
                checkReadOCR = nIndex++;

                checkSafeDoor = nIndex++;
                reserved1 = nIndex++;
                reserved2 = nIndex++;
                reserved3 = nIndex++;

                doorOpenSleep = nIndex++;
                doorCloseSleep = nIndex++;
                vacuumOperate = nIndex++;

                MBoxExit1_C = nIndex++;
                MBoxExit2_C = nIndex++;
            }
        }//30

        public static class CustomDefine
        {
            public static int nIndex = 0;

            public static int initialPLC = nIndex++;
            public static int initialPLC_Ing = nIndex++;
            public static int initialPLCOK = nIndex++;

            public static int waferState = nIndex++;
            public static int initialWafer = nIndex++;
            public static int initialWaferOK = nIndex++;

            public static int startFindEdge = nIndex++;
            public static int isVacuumXifu = nIndex++;
            public static int waferExitMaterial = nIndex++;

            //5
            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;

                initialPLC = nIndex++;
                initialPLC_Ing = nIndex++;
                initialPLCOK = nIndex++;

                waferState = nIndex++;
                initialWafer = nIndex++;
                initialWaferOK = nIndex++;

                startFindEdge = nIndex++;
                isVacuumXifu = nIndex++;
                waferExitMaterial = nIndex++;
            }

        }//35

        //寻边X轴
        public static class FFindEdgeMotorX
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();

                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);

                    position2 = nIndex++;
                    position3 = nIndex++;
                    //5                  
                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                //4

                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //4                    

                    position2 = nIndex++;
                    position3 = nIndex++;

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();

                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5
                    position2 = nIndex++;
                    position3 = nIndex++;

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//49

        //寻边Y轴
        public static class FFindEdgeMotorY
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();
                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5                  
                    position2 = nIndex++;
                    position3 = nIndex++;

                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                //4
                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //4                    
                    position2 = nIndex++;
                    position3 = nIndex++;

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();
                public static int position2 = nIndex++;
                public static int position3 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5
                    position2 = nIndex++;
                    position3 = nIndex++;

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//63

        //寻边R轴
        public static class FFindEdgeMotorR
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5                  
                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                //4

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //4                    

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//77

        //机械手X轴 
        public static class FRobotMotorX
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();

                public static int position2 = nIndex++;
                public static int position3 = nIndex++;
                public static int position4 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    position2 = nIndex++;
                    position3 = nIndex++;
                    position4 = nIndex++;

                    //5                  
                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                public static int position2 = nIndex++;
                public static int position3 = nIndex++;
                public static int position4 = nIndex++;
                //4

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    position2 = nIndex++;
                    position3 = nIndex++;
                    position4 = nIndex++;
                    //4                    

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();
                public static int position2 = nIndex++;
                public static int position3 = nIndex++;
                public static int position4 = nIndex++;

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    position2 = nIndex++;
                    position3 = nIndex++;
                    position4 = nIndex++;
                    //5

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//91

        //打标Z轴 打
        public static class FMarkMotorZ
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5                  
                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                //4

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //4                    

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//119

        //打标Y轴 正打
        public static class FMarkMotorY
        {
            public static class Control
            {
                public static int nIndex = 0;
                public static motorBaseControl baseMotor = new motorBaseControl();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5                  
                    return nIndex;
                }
            }

            //5
            public static class State
            {
                public static int nIndex = 0;
                public static motorBaseState baseMotor = new motorBaseState();
                //4

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //4                    

                    return nIndex;
                }
            }
            //4

            public static class Param
            {
                public static int nIndex = 0;
                public static motorBaseParam baseMotor = new motorBaseParam();

                public static int ResetIndexValue(int nStart)
                {
                    nIndex = nStart;
                    baseMotor.ResetIndexValue(ref nIndex);
                    //5

                    return nIndex;
                }
            }
            //5

            public static int nIndex = 0;
            public static int ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                nIndex = Control.ResetIndexValue(nIndex);
                //5
                nIndex = State.ResetIndexValue(nIndex);
                //9
                nIndex = Param.ResetIndexValue(nIndex);
                //14
                return nIndex;
            }
        }//105
                
        public static class ExtIOInState
        {
            public static int nIndex = 0;
            public static int X00 = nIndex++;
            public static int X01 = nIndex++;
            public static int X02 = nIndex++;
            public static int X03 = nIndex++;
            public static int X04 = nIndex++;
            public static int X05 = nIndex++;
            public static int X06 = nIndex++;
            public static int X07 = nIndex++;

            public static int X10 = nIndex++;
            public static int X11 = nIndex++;
            public static int X12 = nIndex++;
            public static int X13 = nIndex++;
            public static int X14 = nIndex++;
            public static int X15 = nIndex++;
            public static int X16 = nIndex++;
            public static int X17 = nIndex++;

            public static int X20 = nIndex++;
            public static int X21 = nIndex++;
            public static int X22 = nIndex++;
            public static int X23 = nIndex++;
            public static int X24 = nIndex++;
            public static int X25 = nIndex++;
            public static int X26 = nIndex++;
            public static int X27 = nIndex++;

            public static int X30 = nIndex++;
            public static int X31 = nIndex++;
            public static int X32 = nIndex++;
            public static int X33 = nIndex++;
            public static int X34 = nIndex++;
            public static int X35 = nIndex++;   //上手臂原点位
            public static int X36 = nIndex++;
            public static int X37 = nIndex++;

            public static int X40 = nIndex++;
            public static int X41 = nIndex++;
            public static int X42 = nIndex++;
            public static int X43 = nIndex++;//下手臂原点位
            public static int X44 = nIndex++;//下手臂真空吸附
            public static int X45 = nIndex++;//平台吸附
            public static int X46 = nIndex++;
            public static int X47 = nIndex++;

            public static int X50 = nIndex++;
            public static int X51 = nIndex++;
            public static int X52 = nIndex++;
            public static int X53 = nIndex++;
            public static int X54 = nIndex++;
            public static int X55 = nIndex++;
            public static int X56 = nIndex++;
            public static int X57 = nIndex++;

            public static int X60 = nIndex++;
            public static int X61 = nIndex++;
            public static int X62 = nIndex++;
            public static int X63 = nIndex++;
            public static int X64 = nIndex++;
            public static int X65 = nIndex++;
            public static int X66 = nIndex++;
            public static int X67 = nIndex++;

            public static int X70 = nIndex++;
            public static int X71 = nIndex++;
            public static int X72 = nIndex++;
            public static int X73 = nIndex++;
            public static int X74 = nIndex++;
            public static int X75 = nIndex++;
            public static int X76 = nIndex++;
            public static int X77 = nIndex++;


            public static int X100 = nIndex++;
            public static int X101 = nIndex++;
            public static int X102 = nIndex++;
            public static int X103 = nIndex++;
            public static int X104 = nIndex++;
            public static int X105 = nIndex++;
            public static int X106 = nIndex++;
            public static int X107 = nIndex++;

            //48

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;

                X00 = nIndex++;
                X01 = nIndex++;
                X02 = nIndex++;
                X03 = nIndex++;
                X04 = nIndex++;
                X05 = nIndex++;
                X06 = nIndex++;
                X07 = nIndex++;

                X10 = nIndex++;
                X11 = nIndex++;
                X12 = nIndex++;
                X13 = nIndex++;
                X14 = nIndex++;
                X15 = nIndex++;
                X16 = nIndex++;
                X17 = nIndex++;

                X20 = nIndex++;
                X21 = nIndex++;
                X22 = nIndex++;
                X23 = nIndex++;
                X24 = nIndex++;
                X25 = nIndex++;
                X26 = nIndex++;
                X27 = nIndex++;

                X30 = nIndex++;
                X31 = nIndex++;
                X32 = nIndex++;
                X33 = nIndex++;
                X34 = nIndex++;
                X35 = nIndex++;
                X36 = nIndex++;
                X37 = nIndex++;

                X40 = nIndex++;
                X41 = nIndex++;
                X42 = nIndex++;
                X43 = nIndex++;
                X44 = nIndex++;
                X45 = nIndex++;
                X46 = nIndex++;
                X47 = nIndex++;

                X50 = nIndex++;
                X51 = nIndex++;
                X52 = nIndex++;
                X53 = nIndex++;
                X54 = nIndex++;
                X55 = nIndex++;
                X56 = nIndex++;
                X57 = nIndex++;

                X60 = nIndex++;
                X61 = nIndex++;
                X62 = nIndex++;
                X63 = nIndex++;
                X64 = nIndex++;
                X65 = nIndex++;
                X66 = nIndex++;
                X67 = nIndex++;

                X70 = nIndex++;
                X71 = nIndex++;
                X72 = nIndex++;
                X73 = nIndex++;
                X74 = nIndex++;
                X75 = nIndex++;
                X76 = nIndex++;
                X77 = nIndex++;

                X100 = nIndex++;
                X101 = nIndex++;
                X102 = nIndex++;
                X103 = nIndex++;
                X104 = nIndex++;
                X105 = nIndex++;
                X106 = nIndex++;
                X107 = nIndex++;

            }

        }//17

        public static class ExtIOOutState
        {
            public static int nIndex = 0;

            public static int Y00 = nIndex++;
            public static int Y01 = nIndex++;
            public static int Y02 = nIndex++;
            public static int Y03 = nIndex++;
            public static int Y04 = nIndex++;
            public static int Y05 = nIndex++;
            public static int Y06 = nIndex++;
            public static int Y07 = nIndex++;

            public static int Y10 = nIndex++;
            public static int Y11 = nIndex++;
            public static int Y12 = nIndex++;
            public static int Y13 = nIndex++;
            public static int Y14 = nIndex++;
            public static int Y15 = nIndex++;
            public static int Y16 = nIndex++;
            public static int Y17 = nIndex++;

            public static int Y20 = nIndex++;
            public static int Y21 = nIndex++;
            public static int Y22 = nIndex++;
            public static int Y23 = nIndex++;
            public static int Y24 = nIndex++;
            public static int Y25 = nIndex++;
            public static int Y26 = nIndex++;
            public static int Y27 = nIndex++;

            public static int Y30 = nIndex++;
            public static int Y31 = nIndex++;
            public static int Y32 = nIndex++;
            public static int Y33 = nIndex++;
            public static int Y34 = nIndex++;
            public static int Y35 = nIndex++;
            public static int Y36 = nIndex++;
            public static int Y37 = nIndex++;

            public static int Y40 = nIndex++;
            public static int Y41 = nIndex++;
            public static int Y42 = nIndex++;
            public static int Y43 = nIndex++;
            public static int Y44 = nIndex++;
            public static int Y45 = nIndex++;
            public static int Y46 = nIndex++;
            public static int Y47 = nIndex++;

            public static int Y50 = nIndex++;
            public static int Y51 = nIndex++;
            public static int Y52 = nIndex++;
            public static int Y53 = nIndex++;
            public static int Y54 = nIndex++;
            public static int Y55 = nIndex++;
            public static int Y56 = nIndex++;
            public static int Y57 = nIndex++;

            public static int Y60 = nIndex++;
            public static int Y61 = nIndex++;
            public static int Y62 = nIndex++;
            public static int Y63 = nIndex++;
            public static int Y64 = nIndex++;
            public static int Y65 = nIndex++;
            public static int Y66 = nIndex++;
            public static int Y67 = nIndex++;

            //13
            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;

                Y00 = nIndex++;
                Y01 = nIndex++;
                Y02 = nIndex++;
                Y03 = nIndex++;
                Y04 = nIndex++;
                Y05 = nIndex++;
                Y06 = nIndex++;
                Y07 = nIndex++;

                Y10 = nIndex++;
                Y11 = nIndex++;
                Y12 = nIndex++;
                Y13 = nIndex++;
                Y14 = nIndex++;
                Y15 = nIndex++;
                Y16 = nIndex++;
                Y17 = nIndex++;

                Y20 = nIndex++;
                Y21 = nIndex++;
                Y22 = nIndex++;
                Y23 = nIndex++;
                Y24 = nIndex++;
                Y25 = nIndex++;
                Y26 = nIndex++;
                Y27 = nIndex++;

                Y30 = nIndex++;
                Y31 = nIndex++;
                Y32 = nIndex++;
                Y33 = nIndex++;
                Y34 = nIndex++;
                Y35 = nIndex++;
                Y36 = nIndex++;
                Y37 = nIndex++;

                Y40 = nIndex++;
                Y41 = nIndex++;
                Y42 = nIndex++;
                Y43 = nIndex++;
                Y44 = nIndex++;
                Y45 = nIndex++;
                Y46 = nIndex++;
                Y47 = nIndex++;

                Y50 = nIndex++;
                Y51 = nIndex++;
                Y52 = nIndex++;
                Y53 = nIndex++;
                Y54 = nIndex++;
                Y55 = nIndex++;
                Y56 = nIndex++;
                Y57 = nIndex++;

                Y60 = nIndex++;
                Y61 = nIndex++;
                Y62 = nIndex++;
                Y63 = nIndex++;
                Y64 = nIndex++;
                Y65 = nIndex++;
                Y66 = nIndex++;
                Y67 = nIndex++;
            }
        }//30

        public static class ActDelayTimes
        {
            public static int nIndex = 0;
            public static int upFlowExitMaterial = nIndex++;
            public static int dnFlowExitMaterial = nIndex++;
            public static int dnFlowFullMaterial = nIndex++;
            public static int workpieceStation = nIndex++;
            public static int trayExitMaterial = nIndex++;
            public static int NGtrayExitMaterial = nIndex++;
            public static int roateVacuumSucker = nIndex++;

            public static int transVacuumSucker = nIndex++;

            //8
            public static int positionGasOrig = nIndex++;
            public static int positionGasStation = nIndex++;
            public static int jiaJinGasOrig = nIndex++;
            public static int jiaJinGasStation = nIndex++;
            public static int tuiLiaoGasOrig = nIndex++;
            public static int tuiLiaoGasStation = nIndex++;
            public static int roateGasOrig = nIndex++;
            public static int rotateGasStation = nIndex++;
            public static int NGGasRetract = nIndex++;
            public static int NGGasExtend = nIndex++;
            public static int flowExitMaterial = nIndex++;
            public static int takePhoto = nIndex++;
            //20

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                upFlowExitMaterial = nIndex++;
                dnFlowExitMaterial = nIndex++;
                dnFlowFullMaterial = nIndex++;
                workpieceStation = nIndex++;
                trayExitMaterial = nIndex++;
                NGtrayExitMaterial = nIndex++;
                roateVacuumSucker = nIndex++;

                transVacuumSucker = nIndex++;

                positionGasOrig = nIndex++;
                positionGasStation = nIndex++;
                jiaJinGasOrig = nIndex++;
                jiaJinGasStation = nIndex++;
                tuiLiaoGasOrig = nIndex++;
                tuiLiaoGasStation = nIndex++;
                roateGasOrig = nIndex++;
                rotateGasStation = nIndex++;
                NGGasRetract = nIndex++;
                NGGasExtend = nIndex++;
                flowExitMaterial = nIndex++;
                takePhoto = nIndex++;
            }

        }//248

        public static class WorkSate
        {
            public static int nIndex = 0;

            public static int workState = nIndex++;//1:手动中 2：设备初始化中 3：准备完成，可自动  4：自动运行中   5： 停止    6：故障中

            public static int manualValue = 1;
            public static int initialValue = 2;
            public static int readyValue = 3;
            public static int runValue = 4;
            public static int stoping = 5;
            public static int warning = 6;

            public static int operateLevel = nIndex++;
            public static int operateDelay = nIndex++;

            //6
            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                workState = nIndex++;

                operateLevel = nIndex++;
                operateDelay = nIndex++;
            }
        }//249
                       
        public static class WarningState
        {

            public static int nStartIndex = 0;
            public static int nIndex = 0;

            public static List<string> warningList = new List<string>();

            public static int warningCode1 = nIndex++;
            public static int warningCode2 = nIndex++;
            public static int warningCode3 = nIndex++;

            public static void ResetIndexValue(int nStart)
            {
                //nStartIndex = nStart;
                ReadWarningData();
                //nIndex = nStart + warningList.Count;
                nIndex = nStart;

                warningCode1 = nIndex++;
                warningCode2 = nIndex++;
                warningCode3 = nIndex++;
            }

            public static void ReadWarningData()
            {
                warningList.Clear();

                string fileName = System.Windows.Forms.Application.StartupPath + "\\feedWarnings.csv";
                if (File.Exists(fileName) == false)
                {
                    return;
                }

                DataTable dataTable = new DataTable();
                MyCommanLib.CommonDefine.OpenCSVFile(ref dataTable, fileName);

                StringBuilder temp = new StringBuilder(255);

                int nWarningCount = dataTable.Rows.Count ;

                for (int i = 1; i < nWarningCount; i++)
                {
                    DataRow row = dataTable.Rows[i];

                    string strValue = row[1].ToString();
                    if (strValue.Length > 0)
                    {
                        warningList.Add(strValue);
                    }
                }

            }
        }

        public static class PartOpcParams
        {
            public static int nIndex = 0;
            public static int startTakePhotoZF = nIndex++;
            public static int startScanBarCode = nIndex++;
            public static int takePhotoOK = nIndex++;
            public static int takePhotoNG = nIndex++;
            public static int scanBarCodeOK = nIndex++;
            public static int scanBarCodeNG = nIndex++;
            

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                startTakePhotoZF = nIndex++;
                startScanBarCode = nIndex++;
                takePhotoOK = nIndex++;
                takePhotoNG = nIndex++;
                scanBarCodeOK = nIndex++;
                scanBarCodeNG = nIndex++;
                
            }
        }

        //button state color
        public static Color runColor = Color.Green;
        public static Color pauseColor = Color.Yellow;
        public static Color disableColor = Color.Gray;     
        public static Color warningColor = Color.Red;

        public static int nPLCOpcParametersCount = 0;

        //TickCount 0 --- 32位正的最大，瞬变为32位最小-----0
        public static int DelayedTime(int startTick)
        {
            int nDelay = 0;
            int startTime = startTick;
            int endTime = System.Environment.TickCount;

            if (endTime > startTime)
            {
                nDelay = endTime - startTime;
            }
            else
            {
                nDelay = (Int32.MaxValue - startTime) + (Int32.MaxValue - Math.Abs(endTime));
            }

            return nDelay;
        }

        public static void TickSleep(int msSecond)
        {
            if (msSecond <= 0)
                msSecond = 1;

            int startTime = System.Environment.TickCount;
            int endTime = System.Environment.TickCount + msSecond;
            int ticks = System.Environment.TickCount;

            Boolean outMax = false;
            if (endTime < startTime)
            {
                outMax = true;
            }

            Boolean timeOut = false;
            while (timeOut == false)
            {
                ticks = System.Environment.TickCount;
                if (outMax)
                {
                    if (ticks < startTime)
                        outMax = false;
                }

                if (!outMax)
                {
                    if (ticks > endTime || ticks <= 0 && endTime > 0 && startTime > 0)
                    {
                        timeOut = true;
                    }
                }
            }
        }


        public static void initialPartOpcParams(int nStart)
        {
            nPLCOpcParametersCount = nStart;
            IOInState.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = IOInState.nIndex;

            IOOutState.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = IOOutState.nIndex;

        }


        public static class XINGAN_IOInState
        {
            public static int nIndex = 0;
            public static int Start = nIndex++;
            public static int Stop = nIndex++;
            public static int Reset = nIndex++;
            public static int EStop = nIndex++;

            public static int Positive_Limit = nIndex++;
            public static int Negative_Limit = nIndex++;
            public static int Home_Limit = nIndex++;

            public static int Light_Cylinder_Origin = nIndex++;
            public static int Light_Cylinder_Work = nIndex++;
            public static int Power_Cylinder_Origin = nIndex++;
            public static int Power_Cylinder_Work = nIndex++;
            public static int SafeDoor = nIndex++;
            public static int AirSupply = nIndex++;
            public static int AlignerVacuum = nIndex++;

            public static int RightArmHome = nIndex++;
            public static int RightArmVacuum = nIndex++;
            public static int LeftArmHome = nIndex++;
            public static int LeftArmVacuum = nIndex++;
            public static int ArmRunning = nIndex++;
            public static int ArmError = nIndex++;

            public static int SmifPlaceMent1 = nIndex++;
            public static int SmifDoorOpen1 = nIndex++;
            public static int SmifOperate1 = nIndex++;
            public static int SmifPresence1 = nIndex++;
            public static int SmifError1 = nIndex++;
            
            public static int SmifPlaceMent2 = nIndex++;
            public static int SmifDoorOpen2 = nIndex++;
            public static int SmifOperate2 = nIndex++;
            public static int SmifPresence2 = nIndex++;
            public static int SmifError2 = nIndex++;
            
            public static int VisionCheck = nIndex++;
            public static int CycleMode = nIndex++;
            public static int BoxExist2A = nIndex++;
            public static int BoxExist2B = nIndex++;

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                Start = nIndex++;
                Stop = nIndex++;
                Reset = nIndex++;
                EStop = nIndex++;

                Positive_Limit = nIndex++;
                Negative_Limit = nIndex++;
                Home_Limit = nIndex++;

                Light_Cylinder_Origin = nIndex++;
                Light_Cylinder_Work = nIndex++;
                Power_Cylinder_Origin = nIndex++;
                Power_Cylinder_Work = nIndex++;
                SafeDoor = nIndex++;
                AirSupply = nIndex++;
                AlignerVacuum = nIndex++;

                RightArmHome = nIndex++;
                RightArmVacuum = nIndex++;
                LeftArmHome = nIndex++;
                LeftArmVacuum = nIndex++;
                ArmRunning = nIndex++;
                ArmError = nIndex++;

                SmifPlaceMent1 = nIndex++;
                SmifDoorOpen1 = nIndex++;
                SmifOperate1 = nIndex++;
                SmifPresence1 = nIndex++;
                SmifError1 = nIndex++;

                SmifPlaceMent2 = nIndex++;
                SmifDoorOpen2 = nIndex++;
                SmifOperate2 = nIndex++;
                SmifPresence2 = nIndex++;
                SmifError2 = nIndex++;

                VisionCheck = nIndex++;
                CycleMode = nIndex++;
                BoxExist2A = nIndex++;
                BoxExist2B = nIndex++;
            }

        }

        public static class XINGAN_IOOutState
        {
            public static int nIndex = 0;
            public static int TowerBuzzer = nIndex++;
            public static int TowerYellowLight = nIndex++;
            public static int TowerGreenLight = nIndex++;
            public static int TowerRedLight = nIndex++;

            public static int StartLight = nIndex++;
            public static int StopLight = nIndex++;
            public static int ResetLight = nIndex++;

            public static int LightVacuum = nIndex++;
            public static int PowerVacuum = nIndex++;

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                TowerBuzzer = nIndex++;
                TowerYellowLight = nIndex++;
                TowerGreenLight = nIndex++;
                TowerRedLight = nIndex++;

                StartLight = nIndex++;
                StopLight = nIndex++;
                ResetLight = nIndex++;

                LightVacuum = nIndex++;
                PowerVacuum = nIndex++;
            }

        }

        public static class XINGAN_MarkZAxis
        {
            public static int nIndex = 0;
            public static int RoateAdd = nIndex++;
            public static int RoateMinus = nIndex++;
            public static int CurrentParam = nIndex++;
            public static int MoveSafe = nIndex++;
            public static int MoveWork = nIndex++;

            public static int HeightCalibrationValue = nIndex++;
            public static int HeightCurrentValue = nIndex++;
            public static int StartMeasurement = nIndex++;
            public static int FinishMeasurement = nIndex++;

            public static int MoveHome = nIndex++;
            public static int Alarm1 = nIndex++;
            public static int Alarm2 = nIndex++;

            public static int OrgControl = nIndex;
            public static int OrgState = nIndex;

            public static int SicParam = nIndex;
            public static int SicControl = nIndex;
            public static int SicState = nIndex;

            public static int SiParam = nIndex;
            public static int SiControl = nIndex;
            public static int SiState = nIndex;

            public static int OCRParam = nIndex;
            public static int OCRControl = nIndex;
            public static int OCRState = nIndex;

            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;

                RoateAdd = nIndex++;
                RoateMinus = nIndex++;
                CurrentParam = nIndex++;
                MoveSafe = nIndex++;
                MoveWork = nIndex++;

                HeightCalibrationValue = nIndex++;
                HeightCurrentValue = nIndex++;
                StartMeasurement = nIndex++;
                FinishMeasurement = nIndex++;

                MoveHome = nIndex++;

                Alarm1 = nIndex++;
                Alarm2 = nIndex++;

                OrgControl = nIndex++;
                OrgState = nIndex++;

                SicParam = nIndex++;
                SicControl = nIndex++;
                SicState = nIndex++;

                SiParam = nIndex++;
                SiControl = nIndex++;
                SiState = nIndex++;

                OCRParam = nIndex++;
                OCRControl = nIndex++;
                OCRState = nIndex++;
            }
        }

        public static void initialValue(int nStart)
        {
            // 初始化顺序： control  --》 station ----》 position

            //control
            nPLCOpcParametersCount = nStart;
           
            //IOInstate ---> IOOutState ---> time delay
            //IOInState.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = IOInState.nIndex;

            //IOOutState.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = IOOutState.nIndex;

            //CustomDefine.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = CustomDefine.nIndex;

            //FFindEdgeMotorX.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FFindEdgeMotorX.nIndex;

            //FFindEdgeMotorY.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FFindEdgeMotorY.nIndex;

            //FFindEdgeMotorR.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FFindEdgeMotorR.nIndex;

            //FRobotMotorX.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FRobotMotorX.nIndex;

            //FMarkMotorZ.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FMarkMotorZ.nIndex;

            //FMarkMotorY.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = FMarkMotorY.nIndex;                       

            //ExtIOInState.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = ExtIOInState.nIndex;

            //ExtIOOutState.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = ExtIOOutState.nIndex;

            //WarningState.ResetIndexValue(nPLCOpcParametersCount);
            //nPLCOpcParametersCount = WarningState.nIndex;

            XINGAN_IOInState.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = XINGAN_IOInState.nIndex;

            XINGAN_IOOutState.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = XINGAN_IOOutState.nIndex;

            XINGAN_MarkZAxis.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = XINGAN_MarkZAxis.nIndex;
        }

    }

    public static class PLCOpcCutParameters
    {
        public static int nPLCOpcParametersCount = 0;

        public static class WorkSate
        {
            public static int nIndex = 0;

            public static int workState = nIndex++;//1:手动中 2：设备初始化中 3：准备完成，可自动  4：自动运行中   5： 停止    6：故障中

            public static int manualValue = 1;
            public static int initialValue = 2;
            public static int readyValue = 3;
            public static int runValue = 4;
            public static int stoping = 5;
            public static int warning = 6;

            public static int operateLevel = nIndex++;
            public static int operateDelay = nIndex++;

            //6
            public static void ResetIndexValue(int nStart)
            {
                nIndex = nStart;
                workState = nIndex++;

                operateLevel = nIndex++;
                operateDelay = nIndex++;
            }
        }//249

        #region 报警信息定义
        public static class WarningState
        {

            public static int nStartIndex = 0;
            public static int nIndex = 0;

            public static List<string> warningList = new List<string>();


            public static void ResetIndexValue(int nStart)
            {
                nStartIndex = nStart;
                ReadWarningData();
                nIndex = nStart + warningList.Count;
            }

            public static void ReadWarningData()
            {
                warningList.Clear();

                string fileName = System.Windows.Forms.Application.StartupPath + "\\cutWarnings.csv";
                if (File.Exists(fileName) == false)
                {
                    return;
                }

                DataTable dataTable = new DataTable();
                MyCommanLib.CommonDefine.OpenCSVFile(ref dataTable, fileName);


                int nWarningCount = dataTable.Rows.Count;

                for (int i = 1; i < nWarningCount; i++)
                {
                    DataRow row = dataTable.Rows[i];

                    string strValue = row[1].ToString();
                    if (strValue.Length > 0)
                    {
                        warningList.Add(strValue);
                    }
                }
            }
        }
        #endregion

        public static void initialValue(int nStart)
        {
            nPLCOpcParametersCount = nStart;
            WorkSate.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = WorkSate.nIndex;

            WarningState.ResetIndexValue(nPLCOpcParametersCount);
            nPLCOpcParametersCount = WarningState.nIndex;
        }
    }
}
