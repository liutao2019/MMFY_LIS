﻿using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicReaProduct : DclPopSelect<EntityDicReaProduct>
    {

        public SelectDicReaProduct()
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
                colInCode = "Rpdt_id";
                colDisplay = "Rpdt_name";
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

        public override List<EntityDicReaProduct> getDataSource()
        {
            return CacheClient.GetCache<EntityDicReaProduct>();
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
