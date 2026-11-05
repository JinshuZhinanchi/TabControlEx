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
    public partial class MarkXParamForm : BaseForm
    {
        public MarkXParamForm()
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

            AxisWork2Control.opcForm = opcOperate;
            AxisWork2Value.opcForm = opcOperate;
            AxisWork2Save.opcForm = opcOperate;

            AxisWork3Control.opcForm = opcOperate;
            AxisWork3Value.opcForm = opcOperate;
            AxisWork3Save.opcForm = opcOperate;

            AxisWork4Control.opcForm = opcOperate;
            AxisWork4Value.opcForm = opcOperate;
            AxisWork4Save.opcForm = opcOperate;

            AxisOrigControl.opcForm = opcOperate;

            ////x
            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.currentPos;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.rotateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.rotateMinus;

            AxisManualSpeedValue.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.manuSpeed;
            AxisAutoSpeedValue.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.autoSpeed;

            ////
            AxisSafeValue.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.safe;
            AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.safe;
            AxisSafeSave.AddReadEdit(AxisPositionValue);
            AxisSafeSave.AddWriteEdit(AxisSafeValue);

            AxisWork1Value.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.baseMotor.work;
            AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.work;
            AxisWork1Save.AddReadEdit(AxisPositionValue);
            AxisWork1Save.AddWriteEdit(AxisWork1Value);

            AxisWork2Value.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.position2;
            AxisWork2Control.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.position2;
            AxisWork2Save.AddReadEdit(AxisPositionValue);
            AxisWork2Save.AddWriteEdit(AxisWork2Value);

            AxisWork3Value.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.position3;
            AxisWork3Control.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.position3;
            AxisWork3Save.AddReadEdit(AxisPositionValue);
            AxisWork3Save.AddWriteEdit(AxisWork3Value);

            AxisWork4Value.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Param.position4;
            AxisWork4Control.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.position4;
            AxisWork4Save.AddReadEdit(AxisPositionValue);
            AxisWork4Save.AddWriteEdit(AxisWork4Value);

            AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.Control.baseMotor.home;
            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FRobotMotorX.State.baseMotor.home;
            stateButtonList.Add(zAxisOrigState);

            ////list
            ////x
            textEditList.Add(AxisPositionValue);
            textEditList.Add(AxisManualSpeedValue);
            textEditList.Add(AxisAutoSpeedValue);

            textEditList.Add(AxisSafeValue);
            textEditList.Add(AxisWork1Value);
            textEditList.Add(AxisWork2Value);
            textEditList.Add(AxisWork3Value);
            textEditList.Add(AxisWork4Value);


        }

        private void AxisWork1Value_TextChanged(object sender, EventArgs e)
        {

        }

        private void AxisSafeSave_Click(object sender, EventArgs e)
        {

        }

        private void AxisWork3Control_Click(object sender, EventArgs e)
        {

        }
    }
}
