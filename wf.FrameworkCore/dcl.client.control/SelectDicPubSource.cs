using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicPubSource : DclPopSelect<EntityDicOrigin>
    {

        public SelectDicPubSource()
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

        public override List<EntityDicOrigin> getDataSource()
        {
            return CacheClient.GetCache<EntityDicOrigin>();
        }
    }
}
