using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConFeesType : ConDicCommon, IBarActionExt
    {

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;

        List<EntityDicPubInsurance> list = new List<EntityDicPubInsurance>();
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        #region IBarAction 成员

        public void Add()
        {
            //标记为新增事件
            blIsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;

            EntityDicPubInsurance feesType = (EntityDicPubInsurance)bsItem_Prop.AddNew();
            feesType.FeesTypeId = string.Empty;
            feesType.FeesTypeDelFlag = LIS_Const.del_flag.OPEN;

            bsItem_Prop.EndEdit();
            bsItem_Prop.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsItem_Prop.EndEdit();

            if (bsItem_Prop.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicPubInsurance dr = (EntityDicPubInsurance)bsItem_Prop.Current;
            if (dr.FeesTypeName == null || dr.FeesTypeName.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("费用类别不能为空！", "提示");
                return;
            }

            String type_id = dr.FeesTypeId.ToString();

            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();

            if (type_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (type_id == "")
                {
                    dr.FeesTypeId = result.GetResult<EntityDicPubInsurance>().FeesTypeId;

                    
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //新半增事件处理
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
            bsItem_Prop.ResetCurrentItem();
        }

        public void Delete()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }

            EntityDicPubInsurance dr = (EntityDicPubInsurance)bsItem_Prop.Current;

            String br_id = dr.FeesTypeId.ToString();

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult== DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
            else
            {

            }
        }

        public void DoRefresh()
        {
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = result.GetResult() as List<EntityDicPubInsurance>;
                bsItem_Prop.DataSource = list;
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }
        #endregion
       
        public ConFeesType()
        {
            InitializeComponent();
            this.Name = "ConFeesType";
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.DoRefresh();
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsItem_Prop.Current != null)
            {
                EntityDicPubInsurance feesType = (EntityDicPubInsurance)bsItem_Prop.Current;
                feesType.FeesTypePyCode = tookit.GetSpellCode(this.btnName.Text);
                feesType.FeesTypeWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }


        #region IBarActionExt 成员

        public void Cancel()
        {
            //新半增事件处理
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Close()
        {

        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion

        
    }
}
