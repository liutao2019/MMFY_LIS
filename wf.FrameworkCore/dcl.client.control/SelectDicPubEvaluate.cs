using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicPubEvaluate : DclPopSelect<EntityDicPubEvaluate>
    {
        public SelectDicPubEvaluate()
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




        public override List<EntityDicPubEvaluate> getDataSource()
        {
            return CacheClient.GetCache<EntityDicPubEvaluate>();
        }

    }
}
