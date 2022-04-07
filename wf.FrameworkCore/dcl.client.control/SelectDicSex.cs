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
    public partial class SelectDicSex : DclPopSelect<EntitySex>
    {
        /// <summary>
        /// 性别列表
        /// </summary>
        public SelectDicSex()
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

        public override List<EntitySex> getDataSource()
        {
            List<EntitySex> list = new List<EntitySex>();
            list.Add(GenEntitySex("-1", "未知", "WZ","WZ"));
            list.Add(GenEntitySex("1", "男", "M", "M"));
            list.Add(GenEntitySex("2", "女", "W", "W"));
            return list;
        }


        private EntitySex GenEntitySex(string SpId, string SexName, string SexPy, string SexWb)
        {
            EntitySex item = new EntitySex();
            item.SpId = SpId;
            item.SexName = SexName;
            item.SexPy = SexPy;
            item.SexWb = SexWb;
            return item;
        }


    }
}
