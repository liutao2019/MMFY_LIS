using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicMicAntitype : DclPopSelect<EntityDicMicAntitype>
    {

        public SelectDicMicAntitype()
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

        public override List<EntityDicMicAntitype> getDataSource()
        {
            return CacheClient.GetCache<EntityDicMicAntitype>();
        }
    }
}
