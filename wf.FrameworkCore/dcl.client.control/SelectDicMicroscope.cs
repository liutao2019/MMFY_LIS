using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicMicroscope : DclPopSelect<EntityDicMicroscope>
    {

        public SelectDicMicroscope()
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

        public override List<EntityDicMicroscope> getDataSource()
        {
            return CacheClient.GetCache<EntityDicMicroscope>();
        }
    }
}
