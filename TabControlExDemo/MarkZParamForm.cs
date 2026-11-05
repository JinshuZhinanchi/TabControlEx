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
    public partial class MarkZParamForm : BaseForm
    {
        public MarkZParamForm()
        {
            InitializeComponent();
        }

        public void InitialOpcButton(OpcOperateForm opcOperate)
        {
            //x
            AxisAddControl.opcForm = opcOperate;
            AxisMinusControl.opcForm = opcOperate;
            AxisPositionValue.opcForm = opcOperate;
            AxisOrigControl.opcForm = opcOperate;

            AxisPositionValue.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.CurrentParam;
            AxisAddControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.RoateAdd;
            AxisMinusControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.RoateMinus;
            AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.OrgControl;
            zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.OrgState;
            stateButtonList.Add(zAxisOrigState);

            tb_sicMarkValue.opcForm = opcOperate;
            btn_sicMarkControl.opcForm = opcOperate;
            btn_sicMarkSave.opcForm = opcOperate;

            tb_sicMarkValue.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.SicParam;
            btn_sicMarkControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.SicControl;
            btn_sicMarkSave.AddReadEdit(AxisPositionValue);
            btn_sicMarkSave.AddWriteEdit(tb_sicMarkValue);

            tb_siMarkValue.opcForm = opcOperate;
            btn_siMarkControl.opcForm = opcOperate;
            btn_siMarkSave.opcForm = opcOperate;

            tb_siMarkValue.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.SiParam;
            btn_siMarkControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.SiControl;
            btn_siMarkSave.AddReadEdit(AxisPositionValue);
            btn_siMarkSave.AddWriteEdit(tb_siMarkValue);

            tb_ocrMarkValue.opcForm = opcOperate;
            btn_ocrControl.opcForm = opcOperate;
            btn_ocrSave.opcForm = opcOperate;

            tb_ocrMarkValue.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.OCRParam;
            btn_ocrControl.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.OCRControl;
            btn_ocrSave.AddReadEdit(AxisPositionValue);
            btn_ocrSave.AddWriteEdit(tb_ocrMarkValue);


            //AxisManualSpeedValue.opcForm = opcOperate;
            //AxisAutoSpeedValue.opcForm = opcOperate;

            //HeightCalibrateSave.opcForm = opcOperate;

            //HeightCurrentValue.opcForm = opcOperate;
            //HeightCurrentSave.opcForm = opcOperate;




            //tb_sicMarkValue.opcForm = opcOperate;

            ////x


            //AxisManualSpeedValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.manuSpeed;
            //AxisAutoSpeedValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.autoSpeed;

            ////
            //HeightCalibrateValue.opcItemIndex = PLCOpcFeedParameters.XINGAN_MarkZAxis.HeightCalibrationValue;
            //AxisSafeControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.safe;
            //HeightCalibrateSave.AddReadEdit(AxisPositionValue);
            //HeightCalibrateSave.AddWriteEdit(HeightCalibrateValue);

            //HeightCurrentValue.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Param.baseMotor.work;
            //AxisWork1Control.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.work;
            //HeightCurrentSave.AddReadEdit(AxisPositionValue);
            //HeightCurrentSave.AddWriteEdit(HeightCurrentValue);

            //AxisOrigControl.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.Control.baseMotor.home;
            //zAxisOrigState.opcItemIndex = PLCOpcFeedParameters.FMarkMotorZ.State.baseMotor.home;
            //stateButtonList.Add(zAxisOrigState);


            ////list
            ////x
            textEditList.Add(AxisPositionValue);
            //textEditList.Add(AxisManualSpeedValue);
            //textEditList.Add(AxisAutoSpeedValue);

            //textEditList.Add(HeightCalibrateValue);
            //textEditList.Add(HeightCurrentValue);
            textEditList.Add(tb_sicMarkValue);
            textEditList.Add(tb_siMarkValue);
            textEditList.Add(tb_ocrMarkValue);

        }
    }
}
