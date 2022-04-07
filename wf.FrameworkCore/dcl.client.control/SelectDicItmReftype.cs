using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicItmReftype : DclPopSelect<EntityDicItmReftype>
    {

        public SelectDicItmReftype()
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

        public override List<EntityDicItmReftype> getDataSource()
        {
            return CacheClient.GetCache<EntityDicItmReftype>();
        }

    }
}
