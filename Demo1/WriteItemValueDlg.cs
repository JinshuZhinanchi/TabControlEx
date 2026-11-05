using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpcDa.Client;

namespace OpcTestClient
{
    public partial class WriteItemValueDlg : Form
    {
        public ItemValue _itemValue;
        private Type _type;
        private Type[] m_types = null;

        public WriteItemValueDlg()
        {
            InitializeComponent();
        }

        public WriteItemValueDlg(ItemValue itemValue)
        {
            InitializeComponent();
            this._itemValue = (ItemValue)itemValue.Clone();
            this.txtItemName.Text = itemValue.ItemName;

            if (itemValue.Value != null)
            {
                this._type = itemValue.Value.GetType();
                this.txtValue.Text = itemValue.Value.ToString();
            }


            m_types = OpcDa.Client.Com.Type.Enumerate();

            foreach (System.Type type in m_types)
            {
                this.cbType.Items.Add(type.Name);
            }

            this.SelectedType = this._type;
        }
        private object GetValue()
        {
            if (_type == null)
            {
                if (this.txtValue.Text == "") return null;
                return this.txtValue.Text;
            }

            //if (_type == typeof(DateTime))
            //{
            //    DateTime datetime = DateTimeCTRL.Value;
            //    if (datetime == DateTimeCTRL.MinDate) return DateTime.MinValue;
            //    return datetime;
            //}

            //if (_type != null && _type.IsArray) 
            //    return OpcDa.Client.Com.Convert.ChangeType(m_value, _type);

            // convert empty string to null for all types other than strings.
            string value = this.txtValue.Text;
            if (_type != typeof(string) && value != null && value == "") value = null;

            // convert string to type (null creates a default value for the type).
            return OpcDa.Client.Com.Convert.ChangeType(value, _type);
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this._type = this.SelectedType;

            this._itemValue.Value = GetValue();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        public System.Type SelectedType
        {
            get
            {
                if (this.cbType.SelectedIndex >= 0)
                {
                    return m_types[this.cbType.SelectedIndex];
                }

                return null;
            }

            set
            {
                for (int ii = 0; ii < m_types.Length; ii++)
                {
                    if (value == m_types[ii])
                    {
                        this.cbType.SelectedIndex = ii;
                        return;
                    }
                }

                this.cbType.SelectedIndex = -1;
            }
        }
    }
}
