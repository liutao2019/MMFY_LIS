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
    public partial class ConReferenceName : ConDicCommon, IBarActionExt
    {
        #region 保存数据表临时过滤信息
        /// <summary>
        /// 过滤编码信息
        /// </summary>
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        /// <summary>
        /// 过滤项目信息
        /// </summary>
        ColumnFilterInfo cfiName = new ColumnFilterInfo();
        /// <summary>
        /// 过滤计算项目信息
        /// </summary>
        ColumnFilterInfo cfiAgeL = new ColumnFilterInfo();
        /// <summary>
        /// 过滤类型项目信息
        /// </summary>
        ColumnFilterInfo cfiAgeH = new ColumnFilterInfo();
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion

        #region IBarAction 成员

        public void Add()
        {

            blIsNew = true;//标记为新增事件
            #region 数据表的过滤信息保存到新建临时过滤信息里
            cfiId = colref_id.FilterInfo;
            cfiName = colref_name.FilterInfo;
            cfiAgeL = colref_age_l.FilterInfo;
            cfiAgeH = colref_age_h.FilterInfo;

            #endregion

            #region 重新清空数据表的过滤信息
            //重新清空数据表的过滤信息
            colref_id.FilterInfo = new ColumnFilterInfo();
            colref_name.FilterInfo = new ColumnFilterInfo();
            colref_age_l.FilterInfo = new ColumnFilterInfo();
            colref_age_h.FilterInfo = new ColumnFilterInfo();
            #endregion

            EntityDicItmReftype dr = (EntityDicItmReftype)bsItem_Prop.AddNew();
            dr.RefId = string.Empty;
            dr.RefDelFlag = LIS_Const.del_flag.OPEN;

            cmbAge_l_unit.EditValue = "岁";
            cmbAge_h_unit.EditValue = "岁";
            this.btnName.Focus();
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
            EntityDicItmReftype dr = (EntityDicItmReftype)bsItem_Prop.Current;
            if (dr.RefName == null || dr.RefName.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("参考值名称不能为空！", "提示");
                btnName.Focus();
                return;
            }
            if (dr.RefAgeLower.ToString() == null || dr.RefAgeLower.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("年龄下限不能为空！", "提示");
                txtage_l.Focus();
                return;
            }
            if (dr.RefAgeLower.ToString() == null || dr.RefAgeLower.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("年龄上限不能为空！", "提示");
                txtage_h.Focus();
                return;
            }
            try
            {
                Convert.ToInt32(dr.RefAgeLower);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("请输入正确年龄下限值！", "提示");
                txtage_l.Focus();
                return;
            }
            try
            {
                Convert.ToInt32(dr.RefAgeHigh);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("请输入正确年龄上限值！", "提示");
                txtage_h.Focus();
                return;
            }

            EntityRequest request = new EntityRequest();
            dr.RefName = btnName.Text;
            dr.RefAgeHigh = Convert.ToInt32(txtage_h.Text);
            dr.RefAgeLower = Convert.ToInt32(txtage_l.Text);
            dr.RefAgeHighUnit = cmbAge_h_unit.Text;
            dr.RefAgeLowerUnit = cmbAge_l_unit.Text;
            //五笔与拼音
            dr.RefPyCode = btnPYM.Text;
            dr.RefWbCode = btnWBM.Text;
            dr.RefDelFlag = "0";
            String type_id = dr.RefId;
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
                //if (type_id == "")
                //{
                //    dtType.Rows[bsItem_Prop.Position]["ref_id"] =

                //        result.Tables["dict_referenceName"].Rows[0]["ref_id"];
                //}
                //this.dtType.AcceptChanges();
                //searchControl1.Initialize(gridView1, bsItem_Prop, dtType, dtAll);
                DoRefresh();
            }
            else
            {
                throw new Exception();
            }

            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                colref_id.FilterInfo = cfiId;
                colref_name.FilterInfo = cfiName;
                colref_age_l.FilterInfo = cfiAgeL;
                colref_age_h.FilterInfo = cfiAgeH;
                blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {
            this.bsItem_Prop.EndEdit();
            if (bsItem_Prop.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicItmReftype dr = (EntityDicItmReftype)bsItem_Prop.Current;
            String br_id = dr.RefId;

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
            EntityDicItmReftype dr = new EntityDicItmReftype();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicItmReftype>;
                this.bsItem_Prop.DataSource = list;
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

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicItmReftype> list = new List<EntityDicItmReftype>();
        public ConReferenceName()
        {

            InitializeComponent();
        }

        private void on_Load(object sender, EventArgs e)
        {
            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView1, _gridLocalizer);

            this.initData();
        }
        private void initData()
        {
            this.DoRefresh();
        }


        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsItem_Prop.Current != null)
            {
                EntityDicItmReftype dr = (EntityDicItmReftype)bsItem_Prop.Current;
                dr.RefPyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.RefWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }


        #region IBarActionExt 成员
        /// <summary>
        /// 放弃
        /// </summary>
        public void Cancel()
        {

            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                colref_id.FilterInfo = cfiId;
                colref_name.FilterInfo = cfiName;
                colref_age_l.FilterInfo = cfiAgeL;
                colref_age_h.FilterInfo = cfiAgeH;
                blIsNew = false;
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
