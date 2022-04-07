using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;
using System.Data;
using dcl.client.common;

namespace lis.client.control
{
    public partial class SelectDict_Sex : HopePopSelect
    {

        public SelectDict_Sex()
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
            //return new FuncLibClient().getDictFromCache(new String[] { "dict_type" }).Tables[0];

            DataTable dt = new DataTable();
            
            dt.Columns.Add("type_name");
            dt.Columns.Add("type_py");
            dt.Columns.Add("type_id");
            dt.Columns.Add("type_wb");

            DataRow dr;

            dr = dt.NewRow();
            dr["type_name"] = "男";
            dr["type_py"] = "N";
            dr["type_id"] = "1";
            dr["type_wb"] = string.Empty;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["type_name"] = "女";
            dr["type_py"] = "L";
            dr["type_id"] = "2";
            dr["type_wb"] = string.Empty;
            dt.Rows.Add(dr);
            return dt;

        }

        private void SelectDict_Type_onBeforeFilter(ref string strFilter)
        {
            
        }




    }
}
