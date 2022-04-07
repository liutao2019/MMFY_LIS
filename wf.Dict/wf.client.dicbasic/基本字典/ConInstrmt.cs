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

using dcl.common;

namespace dcl.client.dicbasic
{
    public partial class ConInstrmt : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件 
        /// </summary>
        private bool blIsNew = false;

        private List<EntityDicInstrument> list = new List<EntityDicInstrument>();

        /// <summary>
        /// 是否绑定用户可用物理组
        /// </summary>
        private bool IsBindingUserTypes { get; set; }

        #region IBarActionExt

        public void Add()
        {
            blIsNew = true;//标记为新增事件

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.buttonEdit7.Focus();
            buttonEdit5.Properties.ReadOnly = true;
            buttonEdit6.Properties.ReadOnly = true;

            EntityDicInstrument dr = (EntityDicInstrument)bsInstrmt.AddNew();
            dr.ItrId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsInstrmt.EndEdit();
            if (bsInstrmt.Current == null)
            {
                return;
            }

            if (buttonEdit1.Text.Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("仪器代码不能为空", "提示");
                return;
            }

            if (buttonEdit7.Text.Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("仪器名称不能为空", "提示");
                return;
            }

            if (selectZD_Type1.valueMember == null || selectZD_Type1.valueMember.Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("物理组别不能为空", "提示");
                return;
            }

            if (lookUpEdit1.EditValue == null || lookUpEdit1.EditValue.ToString().Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("数据类型不能为空", "提示");
                return;
            }

            if (selectDict_Type1.valueMember == null || selectDict_Type1.valueMember.Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("专业组别不能为空", "提示");
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicInstrument eyInstrm = gridView1.GetFocusedRow() as EntityDicInstrument;
            if (eyInstrm.ItrCommType == null)
            {
                eyInstrm.ItrCommType = 1;//如果通讯类型为空，就赋予单项的值（1：单项；2双向）
            }

            String itrid = eyInstrm.ItrId;

            if (string.IsNullOrEmpty(itrid))
            {
                eyInstrm.ItrId = textEdit1.Text;
            }

            request.SetRequestValue(eyInstrm);

            EntityResponse result = new EntityResponse();
            //if (type_id == "")
            if (blIsNew)
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (itrid == "")
                {
                    eyInstrm.ItrId = result.GetResult<EntityDicInstrument>().ItrId;
                }
                eyInstrm.ItrTypeName = selectZD_Type1.displayMember;
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事件
                this.gridControl1.Enabled = true;
            }
            this.bsInstrmt.ResetCurrentItem();
        }

        public void Delete()
        {
            this.bsInstrmt.EndEdit();
            if (bsInstrmt.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicInstrument dr = gridView1.GetFocusedRow() as EntityDicInstrument;

            String br_id = dr.ItrId;

            request.SetRequestValue(dr);

            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                this.bsInstrmt.Remove(dr);
                this.list.Remove(dr);//检索时避免重现
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    //DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
        }

        public void DoRefresh()
        {
            EntityResponse ds = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicInstrument>;
                bsInstrmt.DataSource = new List<EntityDicInstrument>();
                bsInstrmt.DataSource = FiltrateUserTypes(list);
            }
        }

        /// <summary>
        /// 过滤用户可用物理组
        /// </summary>
        /// <param name="dtObjData"></param>
        /// <returns></returns>
        private List<EntityDicInstrument> FiltrateUserTypes(List<EntityDicInstrument> dtObjData)
        {
            List<EntityDicInstrument> result = new List<EntityDicInstrument>();


            if (dtObjData == null || dtObjData.Count <= 0)
                return dtObjData;//为空,不过滤

            if (IsBindingUserTypes && !UserInfo.isAdmin) //是否绑定用户可用物理组,并且非admin用户
            {
                result = dtObjData.Where(w => UserInfo.sqlUserTypesFilter.Contains(w.ItrTypeName)).ToList();
                return result;
            }
            else
            {
                return dtObjData;//没绑定,不过滤
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

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
                blIsNew = false;//取消新增事件
        }

        public void Close() { }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext() { }

        public void MovePrev() { }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConInstrmt()
        {
            InitializeComponent();
            this.Name = "ConInstrmt";
        }

        private void on_Load(object sender, EventArgs e)
        {
            //[项目字典]是否关联用户可用物理组
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsBindingUserTypes") == "是")
            {
                IsBindingUserTypes = true;
            }
            else
            {
                IsBindingUserTypes = false;//默认不绑定用户可用物理组
            }

            this.bsRep_flag.DataSource = CommonValue.GetRepFlag();
            this.bsHost_flag.DataSource = CommonValue.GetHostFlag();

            this.initData();
            setGridControl();

            EntityResponse reportList = base.Other(new EntityRequest());
            this.lueReport.Properties.DataSource = reportList.GetResult() as List<EntityReport>;
            this.lueReport.Properties.DisplayMember = "RepName";
            this.lueReport.Properties.ValueMember = "RepCode";

            this.lookUpEditNobactType.Properties.DataSource = this.getNobact_type();
            this.lookUpEditNobactType.Properties.DisplayMember = "name";
            this.lookUpEditNobactType.Properties.ValueMember = "id";
            if (UserInfo.GetSysConfigValue("seq_visible") == "是")
            {
                colitr_seq.Visible = true;
                colitr_seq.VisibleIndex = 0;
                colitr_id.Visible = false;
            }
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
        private void setGridControl()
        {
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);
        }
        private void initData()
        {
            this.DoRefresh();
        }

        private void buttonEdit7_Leave(object sender, EventArgs e)
        {
            dcl.client.common.SpellAndWbCodeTookit spellcode = new common.SpellAndWbCodeTookit();
            DevExpress.XtraEditors.ButtonEdit be = sender as DevExpress.XtraEditors.ButtonEdit;

            buttonEdit5.Text = spellcode.GetSpellCode(be.Text); //拼音码
            buttonEdit6.Text = spellcode.GetWBCode(be.Text); //五笔码

            if (bsInstrmt.Current != null)
            {
                EntityDicInstrument dr = (EntityDicInstrument)bsInstrmt.Current;
                dr.PyCode = tookit.GetSpellCode(be.Text);
                dr.WbCode = tookit.GetWBCode(be.Text);
            }
        }

        
        /// <summary>
        /// 过滤物理组下拉框数据
        /// </summary>
        /// <param name="strFilter"></param>
        private void selectZD_Type1_onBeforeFilter(ref string strFilter)
        {
            if (!UserInfo.isAdmin && IsBindingUserTypes)//是否绑定用户可用物理组,是则过滤,否则不过滤
            {
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    strFilter += string.Format(" and type_id in ({0}) ", UserInfo.sqlUserTypesFilter);
                }
                else
                {
                    strFilter += " and 1=2";
                }
            }
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (lookUpEdit1.EditValue != null && lookUpEdit1.EditValue.ToString() == "3")
            {
                layoutControl1.HideItem(layoutControlItem22);
            }
        }
    }
}
