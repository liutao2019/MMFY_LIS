using dcl.client.cache;
using dcl.client.common;
using dcl.entity;
using System.Collections.Generic;

namespace dcl.client.control
{
    public partial class SelectDicAntibioType : DclPopSelect<EntityDicAntibioType>
    {
        public SelectDicAntibioType()
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

        public override List<EntityDicAntibioType> getDataSource()
        {
            return CacheClient.GetCache<EntityDicAntibioType>();
        }
    }
}