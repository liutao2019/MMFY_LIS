using dcl.entity;
using System.Collections.Generic;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicMicTemplate : DclPopSelect<EntityDicMicTemplate>
    {
        public int TmpType
        {
            get;
            set;
        }
        public SelectDicMicTemplate()
        {
            InitializeComponent();
            gridView2.OptionsView.ShowDetailButtons = false;
            this.TmpType = -1;
        }

        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl2;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }

        public override List<EntityDicMicTemplate> getDataSource()
        {
            List<EntityDicMicTemplate> listTemp = CacheClient.GetCache<EntityDicMicTemplate>();
            if (TmpType != -1)
                listTemp = listTemp.FindAll(i => i.TmpType == TmpType);
            return listTemp;
        }
    }
}