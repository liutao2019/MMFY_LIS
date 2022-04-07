using System.Collections.Generic;
using System.Data;
using dcl.client.frame;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicInstrument : DclPopSelect<EntityDicInstrument>
    {
        public SelectDicInstrument()
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

        public override List<EntityDicInstrument> getDataSource()
        {
            var cache = CacheClient.GetCache<EntityDicInstrument>();
            if (!ShowAllInstrmt)
            {
                if (UserInfo.GetSysConfigValue("HideCombineItr") == "是")
                {
                    cache = cache.Where(i => i.ItrReportItrId == null || i.ItrReportItrId == "").ToList();
                }
            }
            return cache;
        }

        private bool _showallitr = false;

        public bool ShowAllInstrmt
        {
            get
            {
                return _showallitr;
            }
            set
            {
                _showallitr = value;
            }
        }

    }
}
