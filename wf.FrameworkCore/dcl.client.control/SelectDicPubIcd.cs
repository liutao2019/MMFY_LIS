using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicPubIcd : DclPopSelect<EntityDicPubIcd>
    {
        public SelectDicPubIcd()
        {
            InitializeComponent();
        }

        protected override bool AlllowCustomText
        {
            get
            {
                return true;
            }
        }
        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl1;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView1;
        }


        public override List<EntityDicPubIcd> getDataSource()
        {
            return CacheClient.GetCache<EntityDicPubIcd>();
        }

    }
}
