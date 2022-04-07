using System;
using System.Collections.Generic;
using System.ComponentModel;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicSample : DclPopSelect<EntityDicSample>
    {
        public SelectDicSample()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.colSeq = "SamPyCode";
            //if (this._useCustomType)
            //{
            //    this.getGV().Columns["sam_custom_type"].GroupIndex = 1;
            //    this.SelectMode = HopePopSelectMode.DoubleClick;
            //}
            //else
            //{
            //    this.getGV().ClearGrouping();
            //    //this.getGV().Columns["sam_custom_type"].GroupIndex = 0;
            //    this.SelectMode = HopePopSelectMode.SingleClick;
            //}
        }

        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl1;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView1;
        }


        private bool _useCustomType = false;


        /// <summary>
        /// 使用自定义分组
        /// </summary>
        [Browsable(true)]
        [Category("其他")]
        public bool UserCustomType
        {
            get
            {
                return _useCustomType;
            }
            set
            {
                _useCustomType = value;
            }
        }

        public override List<EntityDicSample> getDataSource()
        {
            return CacheClient.GetCache<EntityDicSample>();
        }
    }
}
