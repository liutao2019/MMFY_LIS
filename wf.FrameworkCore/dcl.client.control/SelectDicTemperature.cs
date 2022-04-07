using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicTemperature : DclPopSelect<EntityDicTemperature>
    {
        public SelectDicTemperature()
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


        public override List<EntityDicTemperature> getDataSource()
        {
            List<EntityDicTemperature> listTemp =  CacheClient.GetCache<EntityDicTemperature>();
            return listTemp;
        }

    }
}
