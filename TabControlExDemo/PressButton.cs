using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TabControlExDemo
{
    public class PressButton:Button
    {
        public int opcItemIndex = -1;
        public int opcItemIndex2 = -1;
        public OpcOperateForm opcForm = null;
        private List<MyOpcTextEdit> readOpcEditList = new List<MyOpcTextEdit>();
        private List<MyOpcTextEdit> saveOpcEditList = new List<MyOpcTextEdit>();

        public PressButton()
        {
            InitializeComponent();
        }

        //解决包含自定义控件的刷新问题
        protected override CreateParams CreateParams
        {
            get

            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle &= ~0x02000000; // Turn off WS_EX_COMPOSITED

                return cp;

            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PressButton
            // 
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PressButton_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PressButton_MouseUp);
            this.ResumeLayout(false);
            
        }

        public void AddReadEdit( MyOpcTextEdit inEdit)
        {
            readOpcEditList.Add(inEdit);
        }

        public void AddWriteEdit(MyOpcTextEdit inEdit)
        {
            saveOpcEditList.Add(inEdit);
        }

        private void PressButton_MouseDown(object sender, MouseEventArgs e)
        {
            //writpe opc  1
            if (opcForm == null )
            {
                return;
            }

            if( opcItemIndex < 0 )
            {
                if(readOpcEditList != null  && saveOpcEditList != null )
                {
                    MyOpcTextEdit readOpcEdit;
                    MyOpcTextEdit saveOpcEdit;

                    int nn = readOpcEditList.Count;

                    for (int i = 0; i < nn; i++ )
                    {
                        readOpcEdit = readOpcEditList[i];
                        saveOpcEdit = saveOpcEditList[i];

                        int nOpcIndex = readOpcEdit.opcItemIndex;
                        Object ob = opcForm.GetItemValue(nOpcIndex);
                        if (ob != null)
                        {
                            int opcSaveIndex = saveOpcEdit.opcItemIndex;
                            if (opcSaveIndex >= 0)
                            {
                                saveOpcEdit.Text = readOpcEdit.Text;
                                opcForm.WriteItemValue(opcSaveIndex, ob);
                            }
                        }
                    }                    
                }
            }
            else
            {
                Boolean bValue = true;
                opcForm.WriteItemValue(opcItemIndex, bValue);
                if( opcItemIndex2 > 0 )
                {
                    opcForm.WriteItemValue(opcItemIndex2, bValue);
                }
                this.BackColor = System.Drawing.Color.Red;
                               

                if (readOpcEditList != null )
                {
                    MyOpcTextEdit readOpcEdit;                    
                    for (int i = 0; i < readOpcEditList.Count; i++)
                    {
                        readOpcEdit = readOpcEditList[i];                        

                        int nOpcIndex = readOpcEdit.opcItemIndex;
                        Object ob = opcForm.GetItemValue(nOpcIndex);
                        if (ob != null)
                        {
                            readOpcEdit.Text = ob.ToString();
                        }
                    }
                }
            }            

        }

        private void PressButton_MouseUp(object sender, MouseEventArgs e)
        {
            //write opc 0 
            if (opcForm == null || opcItemIndex < 0)
                return;

            Boolean bValue = false;
            opcForm.WriteItemValue(opcItemIndex, bValue);
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            if (opcItemIndex2 > 0)
            {
                opcForm.WriteItemValue(opcItemIndex2, bValue);
            }
        }
    }
}
