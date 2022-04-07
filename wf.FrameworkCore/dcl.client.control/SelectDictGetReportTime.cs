using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDictGetReportTime : DclPopSelect<EntityDicComReptime>
    {
        public SelectDictGetReportTime()
        {
            InitializeComponent();


        }
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                colPY = "RetId";
                colInCode = "RetCode";
                
            }
            base.OnLoad(e);
        }
        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl1;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView1;
        }

        public override List<EntityDicComReptime> getDataSource()
        {
            return CacheClient.GetCache<EntityDicComReptime>();
        }
    }
}
