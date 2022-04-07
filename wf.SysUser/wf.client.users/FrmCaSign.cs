using System.Collections.Generic;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.users
{
    public partial class FrmCaSign : FrmCommon
    {
        public FrmCaSign()
        {
            InitializeComponent();
        }

        public FrmCaSign(List<EntityCaSign> source) : this()
        {
            gridControl1.DataSource = source;
        }
    }
}
