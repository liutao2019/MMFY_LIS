using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDictSysUser : DclPopSelect<EntitySysUser>
    {

        public SelectDictSysUser()
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

        public override List<EntitySysUser> getDataSource()
        {
            List<EntitySysUser> listUser = CacheClient.GetCache<EntitySysUser>();
            foreach (EntitySysUser item in listUser)
            {
                if (string.IsNullOrEmpty(item.SortNo))
                {
                    item.SortNo = "999";
                }
            }
            return listUser;
        }

        private void SelectDict_inspect_Leave(object sender, EventArgs e)
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
