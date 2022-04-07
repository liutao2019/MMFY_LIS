using System.Collections.Generic;
using System.Data;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.control
{
    /// <summary>
    /// 专业组
    /// </summary>
    public partial class SelectDicPubProfession : DclPopSelect<EntityDicPubProfession>
    {

        public SelectDicPubProfession()
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
            return CacheClient.GetCache<EntityDicPubProfession>().Where(w => w.ProType == 0).ToList();
        }

        public void SelectDict_P_Type_onBeforeFilter(ref string strFilter)
        {
            strFilter += " and type_flag=0 ";
        }




    }
}
