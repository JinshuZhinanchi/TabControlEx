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
    public partial class MarkYParamForm : BaseForm
    {
        public MarkYParamForm()
        {
            InitializeComponent();
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            //x
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

            AxisOrigControl.opcForm = opcOperate;

            ////x
            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.currentPos;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.rotateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.rotateMinus;

            AxisManualSpeedValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.manuSpeed;
            AxisAutoSpeedValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.autoSpeed;

            ////
            AxisSafeValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.safe;
            AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.safe;
            AxisSafeSave.AddReadEdit(AxisPositionValue);
            AxisSafeSave.AddWriteEdit(AxisSafeValue);

            AxisWork1Value.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Param.baseMotor.work;
            AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.work;
            AxisWork1Save.AddReadEdit(AxisPositionValue);
            AxisWork1Save.AddWriteEdit(AxisWork1Value);

            AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.Control.baseMotor.home;
            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FMarkMotorY.State.baseMotor.home;
            stateButtonList.Add(zAxisOrigState);

            
            ////list
            ////x
            textEditList.Add(AxisPositionValue);
            textEditList.Add(AxisManualSpeedValue);
            textEditList.Add(AxisAutoSpeedValue);

            textEditList.Add(AxisSafeValue);
            textEditList.Add(AxisWork1Value);


        }
    }
}
