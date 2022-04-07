using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicTestTube : DclPopSelect<EntityDicTestTube>
    {
        public SelectDicTestTube()
        {
            InitializeComponent();
        }


        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl1;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView1;
        }

        public override List<EntityDicTestTube> getDataSource()
        {
            return CacheClient.GetCache<EntityDicTestTube>();
        }

    }
}
