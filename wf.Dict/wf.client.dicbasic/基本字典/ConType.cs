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
    public partial class ConType : ConDicCommon, IBarActionExt
    {
        #region 保存数据表临时过滤信息
        /// <summary>
        /// 过滤编码信息
        /// </summary>
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        /// <summary>
        /// 过滤名称
        /// </summary>
        ColumnFilterInfo cfiName = new ColumnFilterInfo();
        /// <summary>
        /// 过滤拼音码
        /// </summary>
        ColumnFilterInfo cfiPy = new ColumnFilterInfo();
        /// <summary>
        /// 过滤五笔码
        /// </summary>
        ColumnFilterInfo cfiWb = new ColumnFilterInfo();
        /// <summary>
        /// 过滤备注信息
        /// </summary>
        ColumnFilterInfo cfiExp = new ColumnFilterInfo();
        /// <summary>
        /// 过滤组别
        /// </summary>
        ColumnFilterInfo cfiFlag = new ColumnFilterInfo();
        /// <summary>
        /// 过滤医院
        /// </summary>
        ColumnFilterInfo cfiHos = new ColumnFilterInfo();

        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion

        #region IBarAction 成员
        #region IBarActionExt 成员
        public void Add()
        {
            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;
            blIsNew = true;//新增事件标志
            #region 保存过滤信息到临时变量里

            cfiExp = coltype_exp.FilterInfo;
            cfiFlag = coltype_flag.FilterInfo;
            cfiId = coltype_id.FilterInfo;
            cfiName = coltype_name.FilterInfo;
            cfiPy = coltype_py.FilterInfo;
            cfiWb = coltype_wb.FilterInfo;
            cfiHos = gridColumn1.FilterInfo;


            #endregion

            #region 清空gridview里的过滤信息
            coltype_exp.FilterInfo = new ColumnFilterInfo(); ;
            coltype_flag.FilterInfo = new ColumnFilterInfo();
            coltype_id.FilterInfo = new ColumnFilterInfo();

            coltype_name.FilterInfo = new ColumnFilterInfo();
            coltype_py.FilterInfo = new ColumnFilterInfo();
            coltype_wb.FilterInfo = new ColumnFilterInfo();
            gridColumn1.FilterInfo = new ColumnFilterInfo();
            #endregion
            EntityDicPubProfession dr = (EntityDicPubProfession)bsType.AddNew();
            dr.ProId = string.Empty;
            dr.ProDelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;
        }

        public void Save()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("组别名称不能为空", "提示信息");
                return;
            }
            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }
            lookUpEdit1.EditValue = "0";
            EntityRequest request = new EntityRequest();

            EntityDicPubProfession dr = (EntityDicPubProfession)bsType.Current;
            //五笔与拼音
            dr.ProPyCode = btnPYM.Text;
            dr.ProWbCode = btnWBM.Text;
            dr.ProType = 1;
            String type_id = dr.ProId;
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
                DoRefresh();
            }
            else
            {

                throw new Exception();
            }
            #region 如果是新增事件重新赋值过滤条件
            if (blIsNew)
            {
                coltype_exp.FilterInfo = cfiExp;
                coltype_flag.FilterInfo = cfiFlag;
                coltype_id.FilterInfo = cfiId;

                coltype_name.FilterInfo = cfiName;
                coltype_py.FilterInfo = cfiPy;
                coltype_wb.FilterInfo = cfiWb;
                gridColumn1.FilterInfo = cfiHos;
                blIsNew = false;
                this.gridControl1.Enabled = true;
            }

            #endregion
        }

        public void Delete()
        {
            this.bsType.EndEdit();
            if (bsType.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicPubProfession dr = (EntityDicPubProfession)bsType.Current;
            String br_id = dr.ProId;

            request.SetRequestValue(dr);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    this.DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
        }

        public void DoRefresh()
        {
            EntityRequest request = new EntityRequest();
            EntityDicPubProfession dr = new EntityDicPubProfession();
            dr.ProType = 1;
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = (ds.GetResult() as List<EntityDicPubProfession>).Where(w => w.ProType == 1).ToList();
                this.bsType.DataSource = list;
                if (IsbindingcboPType)//是否要绑定专业组下拉框
                {
                    IsbindingcboPType = false;
                    //cboPType.DataBindings.Add(new System.Windows.Forms.Binding("displayMember", this.bsType, "ProPtypeName", true));
                    //cboPType.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsType, "ProTypeLink", true));
                }

            }
        }
        #endregion
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
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicPubProfession> list = new List<EntityDicPubProfession>();
        public ConType()
        {

            InitializeComponent();
            this.Name = "ConType";
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
            lookUpEdit1.EditValue = "0";
            setGridControl();
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

        /// <summary>
        /// 是否要绑定专业组下拉框
        /// </summary>
        private bool IsbindingcboPType = true;

        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsType.Current != null)
            {
                EntityDicPubProfession dr = (EntityDicPubProfession)bsType.Current;
                dr.ProPyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.ProWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }

        public void Cancel()
        {
            #region 如果是新增事件重新赋值过滤条件
            if (blIsNew)
            {
                coltype_exp.FilterInfo = cfiExp;
                coltype_flag.FilterInfo = cfiFlag;
                coltype_id.FilterInfo = cfiId;

                coltype_name.FilterInfo = cfiName;
                coltype_py.FilterInfo = cfiPy;
                coltype_wb.FilterInfo = cfiWb;
                gridColumn1.FilterInfo = cfiHos;
                blIsNew = false;
                this.gridControl1.Enabled = true;
            }

            #endregion
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
    }
}
