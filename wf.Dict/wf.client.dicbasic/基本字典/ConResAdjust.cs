using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.entity;
using lis.client.control;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConResAdjust : ConDicCommon, IBarActionExt
    {
        #region IBarAction 成员

        public void Add()
        {
            this.bsBscript.EndEdit();
            EntityDicResAdjust dr = (EntityDicResAdjust)bsBscript.AddNew();
            EntityRequest request = new EntityRequest();
            EntityDicInstrument dr1 = (EntityDicInstrument)bsInstrmt.Current;
            dr.ItrId = dr1.ItrId;
            gridControl2.Enabled = false;
            OperationType = true;
        }

        public void Save()
        {
            this.bsBscript.EndEdit();
            if (bsInstrmt.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            var list = (List<EntityDicResAdjust>)bsBscript.DataSource;
            //String type_id = dr.ItrId;

            EntityResponse result = new EntityResponse();
            EntityDicResAdjust entityCurrent = (EntityDicResAdjust)bsBscript.Current;
            if (OperationType)
            {
                if (list.Count >1)
                {
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        if (list[i].SrcRes == entityCurrent.SrcRes && list[i].MitCno==entityCurrent.MitCno)
                        {
                            lis.client.control.MessageDialog.Show("保存失败！通道码相同且原结果存在相同！", "提示信息");
                            bsBscript.Remove(entityCurrent);
                            gridControl1.DataSource = bsBscript;
                            return;
                        }
                    }
                }
                List<EntityDicResAdjust> listCurrent = new List<EntityDicResAdjust>();
                listCurrent.Add(entityCurrent);
                Dictionary<string, object> d = new Dictionary<string, object>();
                EntitySysOperationLog etOperation = CreateOperateInfo("新增");
                d.Add("Operation", etOperation);
                d.Add("ResAdjust", listCurrent);
                request.SetRequestValue(d);
                result = base.New(request);
            }
            else
            {
                //if (list.Count > 1)
                //{
                //    for (int i = 0; i < list.Count - 1; i++)
                //    {
                //        if (list[i].SrcRes == entityCurrent.SrcRes && list[i].MitCno == entityCurrent.MitCno)
                //        {
                //            lis.client.control.MessageDialog.Show("保存失败！通道码相同且原结果存在相同！", "提示信息");
                //            DoRefresh();
                //            return;
                //        }
                //    }
                //}
                Dictionary<string, object> d = new Dictionary<string, object>();
                EntitySysOperationLog etOperation = CreateOperateInfo("修改");
                d.Add("Operation", etOperation);
                d.Add("ResAdjust", list);
                request.SetRequestValue(d);
                result = base.Update(request);
            }

            if (base.isActionSuccess)
            {
                OperationType = false;
                DoRefresh();
            }
            else
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示信息");
            }
        }
        /// <summary>
        /// 生成操作用户信息记录
        /// </summary>
        /// <returns></returns>
        private EntitySysOperationLog CreateOperateInfo(string type)
        {
            EntitySysOperationLog log = new EntitySysOperationLog();
            log.OperatUserId = UserInfo.loginID;
            log.OperatUserName = UserInfo.userName;
            log.OperatAction = type;
            return log;
        }
        public void Delete()
        {
            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();
            EntityDicResAdjust dr = (EntityDicResAdjust)bsBscript.Current;
            String br_id = dr.ItrId;

            //request.SetRequestValue(dr);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                EntitySysOperationLog etOperation = CreateOperateInfo("删除");
                d.Add("Operation", etOperation);
                d.Add("ResAdjust", dr);
                request.SetRequestValue(d);
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
            this.bsInstrmt.EndEdit();
            EntityDicInstrument Itrdr = (EntityDicInstrument)bsInstrmt.Current;
            EntityRequest request = new EntityRequest();
            EntityDicResAdjust dr = new EntityDicResAdjust();
            if (Itrdr != null)
            {
                dr.ItrId = Itrdr.ItrId;
            }
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicResAdjust>;
                this.bsBscript.DataSource = list;
            }
        }
        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl2", true);
            dlist.Add("gridControl1", true);
            dlist.Add(btnCopy.Name, true);
            return dlist;
        }

        #endregion

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private List<EntityDicResAdjust> list = new List<EntityDicResAdjust>();
        private List<EntityDicInstrument> Itrlist = new List<EntityDicInstrument>();
        bool OperationType = false;

        public ConResAdjust()
        {
            InitializeComponent();

            //对FindPanel的英文按钮进行补充汉化
            Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> _gridLocalizer = new Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string>();
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton, "查找");
            _gridLocalizer.Add(DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton, "清空");
            Hans_GridHelper.HansButtonText(gridView2, _gridLocalizer);

            EntityRequest request = new EntityRequest();
            EntityDicInstrument dr = new EntityDicInstrument();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Other(request);
            if (isActionSuccess)
            {
                Itrlist = ds.GetResult() as List<EntityDicInstrument>;
                this.bsInstrmt.DataSource = Itrlist;
            }
        }


        private void ConResAdjust_Load(object sender, EventArgs e)
        {
            this.DoRefresh();
            if (UserInfo.GetSysConfigValue("seq_visible") == "是")
            {
                colSortNo.Visible = true;
                colSortNo.VisibleIndex = 0;
                colitr_Id.Visible = false;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            bsInstrmt.EndEdit();
            if (bsInstrmt.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }
            EntityDicInstrument dr = (EntityDicInstrument)bsInstrmt.Current;
            FrmMitmOrAdjustCopy frmMitm = new FrmMitmOrAdjustCopy();
            frmMitm.TypeName = "结果调整";
            frmMitm.OriItrId = dr.ItrId.ToString();
            frmMitm.ShowDialog();
        }

        

        #region IBarActionExt 成员

        public void Cancel()
        {

        }

        void IBarActionExt.Edit()
        {
            this.gridControl2.Enabled = false;
            OperationType = false;
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

        private void bsInstrmt_CurrentChanged(object sender, EventArgs e)
        {
            DoRefresh();
        }
    }
}
