using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;
using System.Data;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.control
{
    public partial class SelectDicCheckType : DclPopSelect<EntityDicCheckType>
    {
        /// <summary>
        /// 报告类别
        /// </summary>
        public SelectDicCheckType()
        {
            InitializeComponent();
        }

        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl2;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }

        public override List<EntityDicCheckType> getDataSource()
        {
            List<EntityDicCheckType> list = new List<EntityDicCheckType>();
            list.Add(GenEntityDicCheckType("-1", "全部", "QB","QB"));
            list.Add(GenEntityDicCheckType("1", "常规", "CG", "CG"));
            list.Add(GenEntityDicCheckType("2", "急查", "JC", "JC"));
            list.Add(GenEntityDicCheckType("3", "保密", "BM", "BM"));
            list.Add(GenEntityDicCheckType("4", "溶栓", " RS", "RS"));
            return list;
        }


        private EntityDicCheckType GenEntityDicCheckType(string CheckTypeId, string CheckTypeName, string CheckTypePyCode, string CheckTypeWbCode)
        {
            EntityDicCheckType item = new EntityDicCheckType();
            item.CheckTypeId = CheckTypeId;
            item.CheckTypeName = CheckTypeName;
            item.CheckTypePyCode = CheckTypePyCode;
            item.CheckTypeWbCode = CheckTypeWbCode;
            return item;
        }


    }
}
