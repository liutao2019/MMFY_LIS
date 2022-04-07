using dcl.entity;
using System;
using System.Collections.Generic;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicCombine : DclPopSelect<EntityDicCombine>
    {
        public SelectDicCombine()
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

        public override List<EntityDicCombine> getDataSource()
        {
            return CacheClient.GetCache<EntityDicCombine>();
        }

        public void SelectDict_Combine_Leave(object sender, EventArgs e)
        {
            if (this.valueMember == null || this.valueMember == "")
            {
                this.valueMember = "";
                this.displayMember = "";
            }
            else
            {
                if (this.valueMember != "")
                {
                    string s = this.popupContainerEdit1.Text;
                    if (s == "")
                    {
                        this.valueMember = "";
                        this.displayMember = "";
                    }
                    else
                    {
                        this.popupContainerEdit1.Text = this.displayMember;
                    }
                }
            }
        }
    }
}