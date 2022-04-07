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
    public partial class SelectPatType : DclPopSelect<EntityKeyValue>
    {
        public SelectPatType()
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
            cmbList.Add(new EntityKeyValue() { key = 1, name = "外国籍留学生" });
            cmbList.Add(new EntityKeyValue() { key = 2, name = "复工复产人员" });
            cmbList.Add(new EntityKeyValue() { key = 3, name = "返校老师" });
            cmbList.Add(new EntityKeyValue() { key = 4, name = "返校学生" });
            cmbList.Add(new EntityKeyValue() { key = 5, name = "医疗机构工作人员" });
            cmbList.Add(new EntityKeyValue() { key = 6, name = "口岸检疫和边防检查人员" });
            cmbList.Add(new EntityKeyValue() { key = 7, name = "监所工作人员" });
            cmbList.Add(new EntityKeyValue() { key = 8, name = "社会福利养老机构工作人员" });
            cmbList.Add(new EntityKeyValue() { key = 9, name = "孕产妇" });
            cmbList.Add(new EntityKeyValue() { key = 20, name = "新生儿" });
            cmbList.Add(new EntityKeyValue() { key = 99, name = "其他人群" });

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
