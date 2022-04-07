using System.Collections.Generic;
using dcl.client.control;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.msgclient
{
    /// <summary>
    /// 用来替换SelectDic_Doctor缓存控件控件
    /// </summary>
    public partial class SelectDicPubDoctor : DclPopSelect<EntityDicDoctor>
    {

        public SelectDicPubDoctor()
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

        public override List<EntityDicDoctor> getDataSource()
        {
            return CacheClient.GetCache<EntityDicDoctor>();
        }
    }
}
