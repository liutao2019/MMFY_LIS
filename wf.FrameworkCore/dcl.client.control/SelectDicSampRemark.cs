using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicSampRemark : DclPopSelect<EntityDicSampRemark>
    {

        public SelectDicSampRemark()
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
            return this.gridControl2;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }

        public override List<EntityDicSampRemark> getDataSource()
        {
            return CacheClient.GetCache<EntityDicSampRemark>();
        
        }

    }
}
