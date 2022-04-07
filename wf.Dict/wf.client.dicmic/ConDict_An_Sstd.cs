using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using DevExpress.XtraGrid.Columns;
using dcl.entity;
using System.Linq;

namespace dcl.client.dicmic
{
    public partial class ConDict_An_Sstd : ConDicCommon, IBarActionExt
    {
        #region IBarActionExt 成员
        List<EntityDicMicAntitype> listAntiType = new List<EntityDicMicAntitype>();
        List<EntityDicMicAntidetail> listAntiDetail = new List<EntityDicMicAntidetail>();
        List<EntityDicMicAntibio> listAntibio = new List<EntityDicMicAntibio>();

        public void Add()
        {
            if (!gridControl2.UseEmbeddedNavigator)
            {
                gridControl2.UseEmbeddedNavigator = true;
            }
            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            if (dsBasicDictBindingSource.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择所要添加的抗生素的药敏卡!", "提示");
                return;
            }
            EntityDicMicAntitype antiType = dsBasicDictBindingSource.Current as EntityDicMicAntitype;
            EntityDicMicAntidetail anSstd = dsBindingSoure.AddNew() as EntityDicMicAntidetail;
            anSstd.AnsDefId = antiType.AtypeId;
            anSstd.AnsDelFlag = "0";
            gridView2.FocusedRowHandle = dsBindingSoure.Count - 1;


            this.gridControl1.Enabled = false;
            setGridEdit(true);
        }

        public void Save()
        {
            this.dsBindingSoure.EndEdit();
            gridView2.CloseEditor();
            if (this.gridView2.FocusedRowHandle < 0)
            {
                return;
            }
            if (dsBasicDictBindingSource.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择所要添加的抗生素的药敏卡!", "提示");
                isActionSuccess = false;
                return;
            }

            List<EntityDicMicAntidetail> rules = dsBindingSoure.DataSource as List<EntityDicMicAntidetail>;
            var listNew = new List<EntityDicMicAntidetail>();
            var listUpdate = new List<EntityDicMicAntidetail>();
            if (rules.Count == 0)
            {
                MessageDialog.Show("没有保存的项");
                return;
            }
            foreach (EntityDicMicAntidetail item in rules)
            {
                if (item.AnsAntiCode == null || item.AnsAntiCode == "")
                {
                    MessageDialog.Show("请选择所要添加的抗生素！", "提示");
                    isActionSuccess = false;
                    return;
                }
                if (item.AnsDefSn == null)
                    listNew.Add(item);
                else
                    listUpdate.Add(item);
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("new", listNew);
            dict.Add("update", listUpdate);

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dict);

            EntityResponse result = new EntityResponse();
            result = base.Other(request);
            if (base.isActionSuccess)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功");
                setGridEdit(false);
                gridControl2.UseEmbeddedNavigator = false;
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
                throw new Exception(result.ErroMsg);
            }
            DoRefresh();
        }

        public void Delete()
        {
            this.dsBindingSoure.EndEdit();
            if (this.gridView2.FocusedRowHandle < 0)
            {
                lis.client.control.MessageDialog.Show("未选择需删除行！", "提示");
                return;
            }
            EntityDicMicAntidetail antiDetail = dsBindingSoure.Current as EntityDicMicAntidetail;
            String br_id = antiDetail.AnsDefSn.ToString();

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(antiDetail);
            if (br_id == "")
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
            doRefresh();
            this.dataBindGR2();
            //if (gridControl2.UseEmbeddedNavigator)
            //{
            //    gridControl2.UseEmbeddedNavigator = false;
            //}
            setGridEdit(false);
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add(gridControl2.Name, true);
            dlist.Add(gridControl1.Name, true);
            return dlist;
        }

        #endregion

        public ConDict_An_Sstd()
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
            setGridEdit(false);
            if (UserInfo.GetSysConfigValue("BacLab_AntiGroup") == "是")
                colss_aen.Caption = "分组";
            gridControl2.UseEmbeddedNavigator = false;
        }

        private void initData()
        {
            this.doRefresh();
        }

        private void doRefresh()
        {
            ProxyCommonDic proxy = new ProxyCommonDic("svc.ConDict_An_Stype");
            ProxyCommonDic proxy1 = new ProxyCommonDic("svc.ConDict_Antibio");
            EntityResponse result = base.Search(new EntityRequest());
            EntityResponse sType = proxy.Search(new EntityRequest());
            EntityResponse antibios = proxy1.Search(new EntityRequest());
            if (isActionSuccess)
            {
                listAntiDetail = result.GetResult() as List<EntityDicMicAntidetail>;
                dsBindingSoure.DataSource = listAntiDetail;
                listAntibio = antibios.GetResult() as List<EntityDicMicAntibio>;
                btItem.DataSource = listAntibio;
                listAntiType = sType.GetResult() as List<EntityDicMicAntitype>;
                dsBasicDictBindingSource.DataSource = listAntiType;
                btCustomType.DataSource = getSamType();
            }
        }

        //绑定gridview2数据
        private void dataBindGR2()
        {
            this.gridControl2.Focus();
            if (dsBasicDictBindingSource.Current != null)
            {
                EntityDicMicAntitype dr = dsBasicDictBindingSource.Current as EntityDicMicAntitype;
                dsBindingSoure.DataSource = listAntiDetail.Where(x => x.AnsDefId == dr.AtypeId).ToList();
            }
            else
            {
                dsBindingSoure.DataSource = listAntiDetail;
            }
        }

        private List<EntitySampCustomType> getSamType()
        {
            List<EntitySampCustomType> customTypeList = new List<EntitySampCustomType>();
            for (int i = 0; i < 8; i++)
            {
                EntitySampCustomType customType = new EntitySampCustomType();
                customType.ZoneSamCustomType = i.ToString();
                #region 给标本组名称赋值
                if (i == 0)
                {
                    customType.ZoneSamCustomName = "BLD";
                }
                else if (i == 1)
                {
                    customType.ZoneSamCustomName = "MISC";
                }
                else if (i == 2)
                {
                    customType.ZoneSamCustomName = "UR";
                }
                else if (i == 3)
                {
                    customType.ZoneSamCustomName = "RES";
                }
                else if (i == 4)
                {
                    customType.ZoneSamCustomName = "CSF";
                }
                else if (i == 5)
                {
                    customType.ZoneSamCustomName = "SBF";
                }
                else if (i == 6)
                {
                    customType.ZoneSamCustomName = "STL";
                }
                else
                {
                    customType.ZoneSamCustomName = "SV";
                }
                #endregion
                customTypeList.Add(customType);
            }
            return customTypeList;
        }
        private void gridControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Add();
            }
        }


        private void setGridEdit(bool isTrue)
        {
            foreach (DevExpress.XtraGrid.Columns.GridColumn item in gridView2.Columns)
            {
                item.OptionsColumn.AllowEdit = isTrue;
            }
        }


        #region IBarActionExt 成员

        public void Cancel()
        {
            //setGridEdit(false);
            this.gridControl2.UseEmbeddedNavigator = false;
        }

        public void Edit()
        {
            if (this.dsBasicDictBindingSource.Current == null)
            {
                lis.client.control.MessageDialog.Show("无修改数据！", "提示");
                if (this.ParentForm is FrmCommon)
                {
                    ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
                }
                return;
            }
            setGridEdit(true);
            if (!gridControl2.UseEmbeddedNavigator)
            {
                gridControl2.UseEmbeddedNavigator = true;
            }
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

        /// <summary>
        /// 前期方便修改增加修改立马改变数据库方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void gridControl2_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.Tag != null && e.Button.Tag.ToString() == "add")
            {
                Add();
            }
            if (e.Button.Tag != null && e.Button.Tag.ToString() == "delete")
            {
                Delete();
            }
        }

        private void ctlRepositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ctlRepositoryItemLookUpEdit1.EndUpdate();
            gridView2.CloseEditor();
        }
        

        private void dsBasicDictBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.dataBindGR2();
        }
    }
}
