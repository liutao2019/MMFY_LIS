using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;
using System.Data;
using dcl.client.common;
using dcl.common;

namespace lis.client.control
{
    public partial class SelectDict_PatCheckType : HopePopSelect
    {

        public SelectDict_PatCheckType()
        {

            InitializeComponent();

        }


        protected override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl2;
        }

        protected override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }

        protected override DataTable getDataSource()
        {
            DataTable dt = CommonValue.GetPatCtype();
            dt.Rows[0].Delete();
            return dt;
        }

        private void SelectDict_Type_onBeforeFilter(ref string strFilter)
        {
            
        }




    }
}
