using System.Collections.Generic;
using System.Data;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.control
{
    /// <summary>
    /// 物理组
    /// </summary>
    public partial class SelectDicLabProfession : DclPopSelect<EntityDicPubProfession>
    {

        public SelectDicLabProfession()
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

        public override List<EntityDicPubProfession> getDataSource()
        {
            //实验组
            List<EntityDicPubProfession> dt = CacheClient.GetCache<EntityDicPubProfession>();
            dt = dt.Where(w => w.ProType == 1).ToList();
            foreach (EntityDicPubProfession type in dt)
            {
                if (string.IsNullOrEmpty(type.ProSortNo.ToString()))
                {
                    type.ProSortNo = 999;
                }
            }
            return dt;
        }

    }
}
