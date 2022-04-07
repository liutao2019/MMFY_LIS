using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicPubOrganize : DclPopSelect<EntityDicPubOrganize>
    {

        public SelectDicPubOrganize()
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

        public override List<EntityDicPubOrganize> getDataSource()
        {
            return CacheClient.GetCache<EntityDicPubOrganize>();
        }

    }
}
