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
    public partial class ConDoctor : ConDicCommon, IBarActionExt
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        private bool blIsNew = false;
        private List<EntityDicDoctor> list = new List<EntityDicDoctor>();

        #region IBarAction 成员

        public void Add()
        {
            blIsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            this.btnName.Focus();
            btnPYM.Properties.ReadOnly = true;
            btnWBM.Properties.ReadOnly = true;

            EntityDicDoctor dr = (EntityDicDoctor)bsDoctor.AddNew();
            dr.DoctorId = string.Empty;
            dr.DelFlag = LIS_Const.del_flag.OPEN;

            bsDoctor.EndEdit();
            bsDoctor.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            if (this.btnName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("医生名称不能为空", "提示信息");
                this.ActiveControl = this.btnName;
                this.btnName.Focus();
                return;
            }
            if (this.btnGH.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("医生代码不能为空", "提示信息");
                this.ActiveControl = this.btnGH;
                this.btnGH.Focus();

                return;
            }
            this.bsDoctor.EndEdit();
            if (bsDoctor.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDicDoctor dr = (EntityDicDoctor)bsDoctor.Current;
            String doctor_id = dr.DoctorId;

            request.SetRequestValue(dr);

            EntityResponse result = new EntityResponse();

            if (doctor_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                if (doctor_id == "")
                {
                    dr.DoctorId = result.GetResult<EntityDicDoctor>().DoctorId;
                }
                dr.DoctorDeptName = selectDicPubDept1.displayMember;
                MessageDialog.ShowAutoCloseDialog("保存成功");
                //DoRefresh();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridControl1.Enabled = true;
            }

            bsDoctor.ResetCurrentItem();
        }

        public void Delete()
        {
            this.bsDoctor.EndEdit();
            if (bsDoctor.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicDoctor dr = (EntityDicDoctor)bsDoctor.Current;
            String br_id = dr.DoctorId;

            request.SetRequestValue(dr);

            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                bsDoctor.Remove(dr);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    //DoRefresh(); //不刷新数据(Remove就好，目的:为了不跳到第一行,方便查看)
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
                list = ds.GetResult() as List<EntityDicDoctor>;
                bsDoctor.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            return dlist;
        }
        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        public ConDoctor()
        {
            InitializeComponent();
            this.Name = "ConDoctor";
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
            //this.btnPYM.Text = tookit.GetSpellCode(this.btnName.Text);
            //this.btnWBM.Text = tookit.GetWBCode(this.btnName.Text);

            if (bsDoctor.Current != null)
            {
                EntityDicDoctor dr = (EntityDicDoctor)bsDoctor.Current;

                dr.PyCode = tookit.GetSpellCode(this.btnName.Text);
                dr.WbCode = tookit.GetWBCode(this.btnName.Text);
            }
        }
        

        #region IBarActionExt 成员

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
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

        
        /// <summary>
        /// 同步医生资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSynchro_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm frm = new DevExpress.Utils.WaitDialogForm("请稍后......", "正在同步医生数据！");
            frm.Visible = true;
            try
            {
                EntityRequest request = new EntityRequest();
                EntityResponse respone = new EntityResponse();
                respone = base.Other(request);
                frm.Visible = false;
                if (respone.Scusess)
                {
                    MessageDialog.ShowAutoCloseDialog("同步成功");
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("同步失败");
                }
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                if (frm.Visible)
                    frm.Visible = false;
            }
            
        }
    }
}
