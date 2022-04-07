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
using System.Linq;
using lis.client.control;

namespace dcl.client.dicbasic
{
    public partial class ConUGR_Type : ConDicCommon, IBarActionExt
    {

        #region 保存数据表临时过滤信息
        /// <summary>
        /// 过滤编码信息
        /// </summary>
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        /// <summary>
        /// 过滤镜检类型信息
        /// </summary>
        ColumnFilterInfo cfiName = new ColumnFilterInfo();
        /// <summary>
        /// 过滤五笔码信息
        /// </summary>
        ColumnFilterInfo cfiWBcode = new ColumnFilterInfo();
        /// <summary>
        /// 过滤拼音码信息
        /// </summary>
        ColumnFilterInfo cfiPYcode = new ColumnFilterInfo();
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion

        #region IBarAction 成员

        public void Add()
        {
            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;
            blIsNew = true;//新增事件标志
            #region 保存过滤信息到临时变量里

            cfiId = colmicro_id.FilterInfo;
            cfiName = colmicro_name.FilterInfo;
            cfiPYcode = colmicro_py.FilterInfo;
            cfiWBcode = colmicro_wb.FilterInfo;


            #endregion

            #region 清空gridview里的过滤信息
            colmicro_id.FilterInfo = new ColumnFilterInfo();

            colmicro_name.FilterInfo = new ColumnFilterInfo();
            colmicro_py.FilterInfo = new ColumnFilterInfo();
            colmicro_wb.FilterInfo = new ColumnFilterInfo();
            #endregion
            EntityDicMicroscope dr = (EntityDicMicroscope)bsUGRType.AddNew();
            dr.MicroId = string.Empty;
            dr.MicroDelFlag = LIS_Const.del_flag.OPEN;
            this.gridControl1.Enabled = false;
            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;
        }

        public void Save()
        {
            this.bsUGRType.EndEdit();
            if (bsUGRType.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();

            EntityDicMicroscope dr = (EntityDicMicroscope)bsUGRType.Current;

            //五笔与拼音
            dr.MicroPyCode = btnPYM.Text;
            dr.MicroWbCode = btnWBM.Text;
            String type_id = dr.MicroId;
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            if (type_id == "null")
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
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }
        }

        public void Delete()
        {
            this.bsUGRType.EndEdit();
            if (bsUGRType.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicMicroscope dr = (EntityDicMicroscope)bsUGRType.Current;
            String br_id = dr.MicroId;

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
            EntityDicMicroscope dr = new EntityDicMicroscope();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicMicroscope>;
                this.bsUGRType.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);

            return dlist;
        }
        #endregion
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicMicroscope> list = new List<EntityDicMicroscope>();
        public ConUGR_Type()
        {

            InitializeComponent();
            this.Name = "ConUGR_Type";
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
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
        private void btnName_Leave(object sender, EventArgs e)
        {
            if (bsUGRType.Current != null)
            {
                EntityDicMicroscope dr = (EntityDicMicroscope)bsUGRType.Current;
                dr.MicroPyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.MicroWbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }


        #region IBarActionExt 成员

        public void Cancel()
        {
            //新增事件时保存的处理方式
            if (blIsNew)
            {
                colmicro_id.FilterInfo = cfiId;

                colmicro_name.FilterInfo = cfiName;
                colmicro_py.FilterInfo = cfiPYcode;
                colmicro_wb.FilterInfo = cfiWBcode;
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }
        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }
        public void Close()
        {

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
