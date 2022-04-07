using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicReaUnit : DclPopSelect<EntityDicReaUnit>
    {

        public SelectDicReaUnit()
        {

            InitializeComponent();


        }
        protected override bool AlllowCustomText
        {
            get
            {
                return true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                colInCode = "Runit_id";
                colDisplay = "Runit_name";
            }
            base.OnLoad(e);
        }

        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl2;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView2;
        }

        public override List<EntityDicReaUnit> getDataSource()
        {
            return CacheClient.GetCache<EntityDicReaUnit>();
        }

        private void SelectDicPubDept_Leave(object sender, EventArgs e)
        {
            //if (this.valueMember == null || this.valueMember == "")
            //{
            //    this.valueMember = "";
            //    this.displayMember = "";
            //}
            //else
            //{
            //    if (this.valueMember != "")
            //    {
            //        string s = this.popupContainerEdit1.Text;
            //        if (s == "")
            //        {
            //            this.valueMember = "";
            //            this.displayMember = "";
            //        }
            //        else
            //        {
            //            this.popupContainerEdit1.Text = this.displayMember;
            //        }
            //    }
            //}
        }




    }
}
