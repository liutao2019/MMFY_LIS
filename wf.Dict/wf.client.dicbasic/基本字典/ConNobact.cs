using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;

using DevExpress.XtraGrid.Columns;
using dcl.entity;
using System.Linq;
using lis.client.control;

namespace dcl.client.dicbasic
{
    public partial class ConNobact : ConDicCommon, IBarActionExt
    {

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;

        List<EntityDicMicSmear> smearList = new List<EntityDicMicSmear>();
        List<EntityDicCombine> combineList = new List<EntityDicCombine>();
        List<EntityDicNobactCom> nobactComList = new List<EntityDicNobactCom>();
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();



        public ConNobact()
        {
            InitializeComponent();
            this.Name = "ConNobact";
        }
        public DataTable getNobact_type()
        {
            DataTable result = new DataTable("Nobact_type");
            result.Columns.Add("id", typeof(int));
            result.Columns.Add("name");
            result.Rows.Add(new Object[] { 0, "全部" });
            result.Rows.Add(new Object[] { 1, "无菌" });
            result.Rows.Add(new Object[] { 2, "涂片" });
            return result;
        }


        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);

            return dlist;
        }

        private void ConNobact_Load(object sender, EventArgs e)
        {
            EntityRequest request = new EntityRequest();
            string type = "GetCombine";
            request.SetRequestValue(type);
            combineList = base.Other(request).GetResult() as List<EntityDicCombine>;
            bindingSourceCom.DataSource = combineList;

            this.initData();
            setGridControl();
        }

        private void initData()
        {
            this.DoRefresh();
            setGridControl();

            this.lookUpEdit_reg_flag.Properties.DataSource = this.Get_RegFlagTable();
            this.lookUpEdit_reg_flag.Properties.DisplayMember = "name";
            this.lookUpEdit_reg_flag.Properties.ValueMember = "id";

            this.lookUpEditNobactType.Properties.DataSource = this.getNobact_type();
            this.lookUpEditNobactType.Properties.DisplayMember = "name";
            this.lookUpEditNobactType.Properties.ValueMember = "id";

        }

        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
        }

        /// <summary>
        /// 取消组合勾选
        /// </summary>
        private void NoCheckComIDs()
        {
            if (bindingSourceCom.DataSource != null && bindingSourceCom.DataSource is List<EntityDicCombine>)
            {
                foreach (EntityDicCombine combine in combineList)
                {
                    combine.Checked = false;
                }

                gvApparatus.CloseEditor();
                bindingSourceCom.ResetBindings(true);
                bindingSourceCom.EndEdit();
            }
        }


        private DataTable Get_RegFlagTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));

            dt.Rows.Add(new object[] { 0, "正常" });
            dt.Rows.Add(new object[] { 1, "阳性" });

            return dt;
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Return")
            {
                SendKeys.Send("{tab}");
            }
        }



        #region IBarAction 成员

        public void Add()
        {
            //标记为新增事件
            this.blIsNew = true;

            NoCheckComIDs();

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.txtNob_cname.Focus();
            txt_py.Properties.ReadOnly = true;
            txt_wb.Properties.ReadOnly = true;

            EntityDicMicSmear dr = (EntityDicMicSmear)bsNobact.AddNew();
            dr.SmeId = string.Empty;
            dr.SmeDelFlag = "0";

            this.gridControl1.Enabled = false;
            this.txtNob_cname.Focus();
            txt_py.Properties.ReadOnly = true;
            txt_wb.Properties.ReadOnly = true;
        }

        public void Save()
        {
            //if (this.txtNobCname.Text.Trim() == string.Empty)
            //{
            //    lis.client.control.MessageDialog.Show("涂片结果不能为空", "提示信息");
            //    return;
            //}
            //else if (this.txtNobtype.Text.Trim() == string.Empty)
            //{
            //    lis.client.control.MessageDialog.Show("组别不能为空", "提示信息");
            //    return;
            //}

            this.bsNobact.EndEdit();
            if (bsNobact.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicMicSmear dr = (bsNobact.Current) as EntityDicMicSmear;
            String nob_id = dr.SmeId;
            Dictionary<string, object> item = new Dictionary<string, object>();
            item.Add("newSmear", dr);

            if (bindingSourceCom.DataSource != null && bindingSourceCom.DataSource is List<EntityDicCombine>)
            {
                List<EntityDicCombine> changeCombineList = combineList.Where(i => i.Checked == true).ToList();
                if (changeCombineList.Count > 0)
                    item.Add("combines", changeCombineList);
                else
                    item.Add("combines", new List<EntityDicCombine>());
            }
            request.SetRequestValue(item);

            EntityResponse result = new EntityResponse();
            if (nob_id == string.Empty)
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (nob_id == string.Empty)
                {
                    dr.SmeId = result.GetResult<EntityDicMicSmear>().SmeId;
                    dr.SmeProName = result.GetResult<EntityDicMicSmear>().SmeProName;
                }
                MessageDialog.ShowAutoCloseDialog("保存成功");
                bsNobact.ResetCurrentItem();
                DoRefresh();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            //为新增事件的处理方式重新赋值为过滤条件
            if (blIsNew)
            {
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsNobact.EndEdit();
            if (bsNobact.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicMicSmear dr = (bsNobact.Current) as EntityDicMicSmear;
            String nob_id = dr.SmeId;

            request.SetRequestValue(dr);
            if (nob_id == "")
            {
                return;
            }
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
            NoCheckComIDs();
            EntityResponse result = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                smearList = result.GetResult() as List<EntityDicMicSmear>;
                bsNobact.DataSource = smearList;
            }
        }
        #endregion

        #region IBarActionExt 成员

        public void Cancel()
        {
            //为新增事件的处理方式重新赋值为过滤条件
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

        private void bsNobact_PositionChanged(object sender, EventArgs e)
        {
            if (bsNobact != null && bsNobact.Position >= 0 && bsNobact.Current != null)
            {
                EntityDicMicSmear dr = bsNobact.Current as EntityDicMicSmear;
                NoCheckComIDs();

                EntityRequest request = new EntityRequest();
                string nobId = dr.SmeId;

                request.SetRequestValue(nobId);

                nobactComList = (base.Other(request)).GetResult() as List<EntityDicNobactCom>;
                if (string.IsNullOrEmpty(dr.SmePublic.ToString()))
                {
                    cbIsPublic.Checked = false;
                }

                if (bindingSourceCom.DataSource != null && bindingSourceCom.DataSource is List<EntityDicCombine>)
                {
                    foreach (EntityDicNobactCom nobactCom in nobactComList)
                    {
                        EntityDicCombine comb = combineList.Find(i => i.ComId == nobactCom.ComId);
                        if (comb != null)
                            comb.Checked = true;
                    }
                    gvApparatus.CloseEditor();
                    bindingSourceCom.DataSource = combineList;
                    bindingSourceCom.ResetBindings(true);
                    bindingSourceCom.EndEdit();
                }

            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = txtFilter.Text.Trim();

            if (filter == "")
                bsNobact.DataSource = smearList;
            else
            {
                bsNobact.DataSource = smearList.Where(w => w.SmeId.Contains(filter) ||
                                                       w.SmeName != null && w.SmeName.Contains(filter) ||
                                                        w.SmePyCode != null &&
                                                        w.SmePyCode.Contains(filter.ToUpper()) ||
                                                       w.SmeWbCode != null && w.SmeWbCode.Contains(filter.ToUpper()) ||
                                                       w.SmeCCode != null && w.SmeCCode.Contains(filter)
                                                             ).ToList();
            }
        }

        private void txtNob_cname_Leave(object sender, EventArgs e)
        {
            //这个不需要拼音码和五笔码
            //if (bsNobact.Current != null)
            //{
            //    EntityDicMicSmear smear = (EntityDicMicSmear)bsNobact.Current;
            //    smear.SmePyCode = tookit.GetSpellCode(this.txtNob_cname.Text);
            //    smear.SmeWbCode = tookit.GetWBCode(this.txtNob_cname.Text);
            //}
        }
    }
}
