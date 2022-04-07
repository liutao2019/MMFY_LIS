using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    public partial class frmDataInterfaceCommandEditor : Form
    {
        public frmDataInterfaceCommandEditor(EnumDataAccessMode dataAccessMode)
        {
            InitializeComponent();
            ctrlDataInterfaceCommandEditor1.DataAccessMode = dataAccessMode;
        }

        public frmDataInterfaceCommandEditor()
            : this(EnumDataAccessMode.DirectToDB)
        {
        }

        public void SetGroupFieldStyle(Dictionary<string, string[]> selectItems, string defaultValue, bool allowEdit, bool allowEmpty)
        {
            ctrlDataInterfaceCommandEditor1.SetGroupFieldStyle(selectItems, defaultValue, allowEdit, allowEmpty);
        }
    }
}
