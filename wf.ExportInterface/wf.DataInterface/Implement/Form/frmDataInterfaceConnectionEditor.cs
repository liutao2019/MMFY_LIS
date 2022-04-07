using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    public partial class frmDataInterfaceConnectionEditor : Form
    {
        public frmDataInterfaceConnectionEditor(EnumDataAccessMode dataAccessMode)
        {
            InitializeComponent();
            ctrlDataInterfaceConnectionEditor1.DataAccessMode = dataAccessMode;
        }

        public frmDataInterfaceConnectionEditor()
            : this(EnumDataAccessMode.DirectToDB)
        {
        }
    }
}
