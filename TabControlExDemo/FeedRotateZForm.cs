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
    public partial class FeedRotateZForm : BaseForm
    {
        public FeedRotateZForm()
        {
            InitializeComponent();
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            //z

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


            ////y
            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.currentPos;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.rotateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.rotateMinus;

            AxisManualSpeedValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.manuSpeed;
            AxisAutoSpeedValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.autoSpeed;

            ////
            AxisSafeValue.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.safe;
            AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.safe;
            AxisSafeSave.AddReadEdit(AxisPositionValue);
            AxisSafeSave.AddWriteEdit(AxisSafeValue);


            AxisWork1Value.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Param.baseMotor.work;
            AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.work;
            AxisWork1Save.AddReadEdit(AxisPositionValue);
            AxisWork1Save.AddWriteEdit(AxisWork1Value);

            AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.Control.baseMotor.home;
            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FFindEdgeMotorR.State.baseMotor.home;
            stateButtonList.Add(zAxisOrigState);


            ////list
            ////y
            textEditList.Add(AxisPositionValue);
            textEditList.Add(AxisManualSpeedValue);
            textEditList.Add(AxisAutoSpeedValue);

            textEditList.Add(AxisSafeValue);
            textEditList.Add(AxisWork1Value);

        }

        private void buttonVPosOffset_Click(object sender, EventArgs e)
        {
           
        }
    }
}
