using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicItmItem : DclPopSelect<EntityDicItmItem>
    {

        public SelectDicItmItem()
        {

            InitializeComponent();


        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.colSeq = "ItmSortNo";
        }


        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl2;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }


        public override List<EntityDicItmItem> getDataSource()
        {
            return CacheClient.GetCache<EntityDicItmItem>();

        }
    }
}
