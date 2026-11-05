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
    public partial class FeedTransZForm : BaseForm
    {
        public FeedTransZForm()
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

            AxisOrigControl.opcForm = opcOperate;


            ////y
            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.currentPos;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.rotateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.rotateMinus;

            ////
            AxisSafeValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.safe;
            AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.safe;
            AxisSafeSave.AddReadEdit(AxisPositionValue);
            AxisSafeSave.AddWriteEdit(AxisSafeValue);


            AxisWork1Value.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.work;
            AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.work;
            AxisWork1Save.AddReadEdit(AxisPositionValue);
            AxisWork1Save.AddWriteEdit(AxisWork1Value);


            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.State.baseMotor.home;
            stateButtonList.Add(zAxisOrigState);


            ////list
            ////y
            textEditList.Add(AxisPositionValue);
            textEditList.Add(AxisManualSpeedValue);
            textEditList.Add(AxisAutoSpeedValue);

            textEditList.Add(AxisSafeValue);
            textEditList.Add(AxisWork1Value);

        }

        
    }
}
