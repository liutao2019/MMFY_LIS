using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicPubInsurance : DclPopSelect<EntityDicPubInsurance>
    {

        public SelectDicPubInsurance()
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

        public override List<EntityDicPubInsurance> getDataSource()
        {
            return CacheClient.GetCache<EntityDicPubInsurance>();
        }

    }
}
