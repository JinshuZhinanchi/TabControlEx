using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public partial class FeedTransXForm : BaseForm
    {
        OpcOperateForm mOpcOperate;
        public FeedTransXForm()
        {
            InitializeComponent();
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            //x                             
            mOpcOperate = opcOperate;

            AxisAddControl.opcForm = opcOperate;
            AxisMinusControl.opcForm = opcOperate;
            AxisPositionValue.opcForm = opcOperate;

            AxisManualSpeedValue.opcForm = opcOperate;
            AxisAutoSpeedValue.opcForm = opcOperate;            

            AxisSafeControl.opcForm = opcOperate;
            AxisSafeValue.opcForm = opcOperate;          
            AxisSafeSave.opcForm = opcOperate;

            AxisWork1Control.opcForm = opcOperate;
            AxisWork1Value.opcForm = opcOperate;
            AxisWork1Save.opcForm = opcOperate;


            AxisWork2Control.opcForm = opcOperate;
            AxisWork2Value.opcForm = opcOperate;
            AxisWork2Save.opcForm = opcOperate;


            AxisWork3Control.opcForm = opcOperate;
            AxisWork3Value.opcForm = opcOperate;
            AxisWork3Save.opcForm = opcOperate;


            AxisOrigControl.opcForm = opcOperate;


            SelectOCRButton.opcForm = opcOperate;
            SelectSafeDoorButton.opcForm = opcOperate;

            ////x
            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.currentPos;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.rotateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.rotateMinus;

            AxisManualSpeedValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.manuSpeed;
            AxisAutoSpeedValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.autoSpeed;

            ////
            AxisSafeValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.safe;
            AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.safe;
            AxisSafeSave.AddReadEdit(AxisPositionValue);
            AxisSafeSave.AddWriteEdit(AxisSafeValue);


            AxisWork1Value.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.baseMotor.work;
            AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.work;
            AxisWork1Save.AddReadEdit(AxisPositionValue);
            AxisWork1Save.AddWriteEdit(AxisWork1Value);

            AxisWork2Value.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.position2;
            AxisWork2Control.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position2;
            AxisWork2Save.AddReadEdit(AxisPositionValue);
            AxisWork2Save.AddWriteEdit(AxisWork2Value);

            AxisWork3Value.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Param.position3;
            AxisWork3Control.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.position3;
            AxisWork3Save.AddReadEdit(AxisPositionValue);
            AxisWork3Save.AddWriteEdit(AxisWork3Value);


            AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.Control.baseMotor.home;
            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorX.State.baseMotor.home;
            stateButtonList.Add(zAxisOrigState);


            SelectOCRButton.opcItemIndex = PLCOpcFeedParameters.IOOutState.checkReadOCR;
            SelectSafeDoorButton.opcItemIndex = PLCOpcFeedParameters.IOOutState.checkSafeDoor;

            textButtonList.Add(SelectOCRButton);
            textButtonList.Add(SelectSafeDoorButton);

            ////list
            ////x
            textEditList.Add(AxisPositionValue);
            textEditList.Add(AxisManualSpeedValue);
            textEditList.Add(AxisAutoSpeedValue);

            textEditList.Add(AxisSafeValue);
            textEditList.Add(AxisWork1Value);
            textEditList.Add(AxisWork2Value);
            textEditList.Add(AxisWork3Value);

        }

       
    }
}
