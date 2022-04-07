using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicCheckPurpose : DclPopSelect<EntityDicCheckPurpose>
    {

        public SelectDicCheckPurpose()
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

        public override List<EntityDicCheckPurpose> getDataSource()
        {
            return CacheClient.GetCache<EntityDicCheckPurpose>();
        }
    }
}
