using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace lis.client.control
{
    public partial class LISCheckBox : UserControl
    {
        private bool canChange = false;
        public LISCheckBox()
        {
            InitializeComponent();
            // this._valueMember = 0;
        }

        //public String _txt;
        [Browsable(true), Category("Hope")]
        public String txt
        {
            get { return this.checkEdit1.Text; }
            set
            {
                this.checkEdit1.Text = value;
            }
        }

        public Object _valueMember;
        [Browsable(true), Category("Hope"), Bindable(true)]
        public Object valueMember
        {
            get { return _valueMember; }
            set
            {
                canChange = false;
                try
                {
                    if (value != null && value.ToString() == "1")
                    {
                        this.checkEdit1.CheckState = CheckState.Checked;
                        _valueMember = 1;
                    }
                    else
                    {
                        this.checkEdit1.CheckState = CheckState.Unchecked;
                        _valueMember = 0;
                    }
                }
                finally
                {
                    canChange = true;
                }
            }
        }



        private void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            if (!canChange) return;
            if (checkEdit1.Checked)
            {
                _valueMember = 1;
            }
            else
            {
                _valueMember = 0;
            }

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LISCheckBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
