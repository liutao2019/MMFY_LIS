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
    public partial class ConBCCuvette : ConDicCommon, IBarActionExt
    {

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        List<EntityDicTestTube> list = new List<EntityDicTestTube>();
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConBCCuvette()
        {
            InitializeComponent();
            this.bsCuvette.CurrentChanged += new EventHandler(bsCuvette_CurrentChanged);
        }

        private void on_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.DoRefresh();
        }

        public void Add()
        {
            blIsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.txtName.Focus();
            txtPY.Properties.ReadOnly = true;
            txtWB.Properties.ReadOnly = true;

            EntityDicTestTube feesType = (EntityDicTestTube)bsCuvette.AddNew();
            feesType.TubCode = string.Empty;
            feesType.TubDelFlag = "0";

            bsCuvette.EndEdit();
            bsCuvette.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsCuvette.EndEdit();

            if (bsCuvette.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicTestTube dr = (EntityDicTestTube)bsCuvette.Current;
            if (dr.TubName == null || dr.TubName.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("试管名不能为空！", "提示");
                return;
            }

            String type_id = dr.TubCode.ToString();

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
                    dr.TubCode = result.GetResult<EntityDicTestTube>().TubCode;


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
        }

        public void Delete()
        {
            this.bsCuvette.EndEdit();
            if (bsCuvette.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }

            EntityDicTestTube dr = (EntityDicTestTube)bsCuvette.Current;

            String br_id = dr.TubCode.ToString();

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult == DialogResult.OK)
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
                list = result.GetResult() as List<EntityDicTestTube>;
                bsCuvette.DataSource = list; 
            }
        }

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
            throw new NotImplementedException();
        }





        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);
            dlist.Add("simpleButton1", true);

            return dlist;
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }



        void bsCuvette_CurrentChanged(object sender, EventArgs e)
        {
            DataRowView row = this.bsCuvette.Current as DataRowView;

            if (row != null)
            {

                //this.txtcuv_fee_code.EditValue = row["cuv_fee_code"];
                //this.txtPri.EditValue = row["cuv_pri"];
                //this.txtColor.EditValue = row["cuv_color"];
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (bsCuvette.Current != null)
            {
                EntityDicTestTube tube = (EntityDicTestTube)bsCuvette.Current;
                tube.TubPyCode = tookit.GetSpellCode(this.txtName.Text);
                tube.TubWbCode = tookit.GetWBCode(this.txtName.Text);
            }
        }

    }
}
