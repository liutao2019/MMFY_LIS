using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.control;
using dcl.entity;

namespace dcl.client.sample.Controls
{
    public partial class SelectIdentifierType : DclPopSelect<EntityKeyValue>
    {
        public SelectIdentifierType()
        {
            InitializeComponent();
        }

        public override DevExpress.XtraGrid.GridControl getGC()
        {
            return this.gridControl1;
        }

        public override DevExpress.XtraGrid.Views.Grid.GridView getGV()
        {
            return this.gridView1;
        }

        public override List<EntityKeyValue> getDataSource()
        {
            List<EntityKeyValue> cmbList = new List<EntityKeyValue>();
            cmbList.Add(new EntityKeyValue() { key = 1, name = "居民身份证" });
            cmbList.Add(new EntityKeyValue() { key = 2, name = "居民户口簿" });
            cmbList.Add(new EntityKeyValue() { key = 3, name = "护照" });
            cmbList.Add(new EntityKeyValue() { key = 4, name = "军官证（士兵证）" });
            cmbList.Add(new EntityKeyValue() { key = 5, name = "驾驶执照" });
            cmbList.Add(new EntityKeyValue() { key = 6, name = "港澳台通行证" });
            cmbList.Add(new EntityKeyValue() { key = 7, name = "其他" });

            return cmbList;
        }

        private void SelectDicPubIdent_Leave(object sender, EventArgs e)
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
