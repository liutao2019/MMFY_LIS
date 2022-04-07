using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicHarvester : DclPopSelect<EntityDictHarvester>
    {
        public SelectDicHarvester()
        {
            if (DesignMode)
                return;
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


        public override List<EntityDictHarvester> getDataSource()
        {
            List<EntityDictHarvester> listHar = CacheClient.GetCache<EntityDictHarvester>();
            return listHar;
        }
    }
}
