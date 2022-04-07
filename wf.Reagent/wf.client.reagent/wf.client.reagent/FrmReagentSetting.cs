using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.entity;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;

namespace wf.client.reagent
{
    public partial class FrmReagentSetting : FrmCommon
    {
        //全局变量 用于调用各种操作数据的方法
        ProxyReaSetting proxy = new ProxyReaSetting();
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        //增加控制列表的项目
        Dictionary<string, Boolean> controlsList = new Dictionary<string, Boolean>();
        public Boolean defaultEnableStatus = false;

        public FrmReagentSetting()
        {
            InitializeComponent();
            sysToolBar1.OrderCustomer = true;
        }

        private void FrmReagentSetting_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnDelete", "BtnSave", "BtnCancel", sysToolBar1.BtnModify.Name, sysToolBar1.BtnClose.Name, sysToolBar1.BtnRefresh.Name }); //添加刷新按钮
            sysToolBar1.BtnClose.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            this.txtName.Leave += TxtName_Leave;
            this.txtSort.TextChanged += txtSort_TextChanged;

            defaultEnableStatus = false;
            EnableControls(panelControl3);
            Button_Enabled(true);

            bindReaSetting();
        }

        private void TxtName_Leave(object sender, EventArgs e)
        {
            if (bsReaSetting.Current != null)
            {
                EntityReaSetting dr = (EntityReaSetting)bsReaSetting.Current;

                dr.py_code = tookit.GetSpellCode(this.txtName.Text);
                dr.wb_code = tookit.GetWBCode(this.txtName.Text);
            }
        }

        /// <summary>
        /// 按钮启用事件
        /// </summary>
        /// <param name="isTrue"></param>
        private void Button_Enabled(bool isTrue)
        {
            sysToolBar1.BtnAdd.Enabled = isTrue;
            sysToolBar1.BtnModify.Enabled = isTrue;
            sysToolBar1.BtnDelete.Enabled = isTrue;
            sysToolBar1.BtnSave.Enabled = !isTrue;
            sysToolBar1.BtnCancel.Enabled = !isTrue;
            gcReagentSetting.Enabled = isTrue;
        }
        //控制所有编辑按钮状态
        private void EnterEditingState(Boolean enable)
        {
            defaultEnableStatus = enable;
        }
        //遍历所有控件，以控制所有控件显示是否可编辑状态
        private void EnableControls(Control parentControl)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c.Name == "cmbSort" || c.Name == "txtSort" || c.Name == "sysItem"
                    || c.Name == "radioGroup_Item" || c.Name == "btnSynchronous" || c.Name == "radioGroup_split"
                    || c.Name == "txtFilter" || c.Name == "dateFrom" || c.Name == "dateEditTo"
                    || c.Name == "btnModify")
                {
                    c.Enabled = true;
                    if (c is DevExpress.XtraEditors.TextEdit)
                    {
                        ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = false;
                    }
                    continue;
                }

                if (c.Controls.Count > 0)
                {
                    if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                    {
                        if (c is DevExpress.XtraEditors.TextEdit)
                        {
                            ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                        }

                        if (c is lis.client.control.HopePopSelect)
                        {
                            ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                        }

                    }
                    else
                    {
                        EnableControls(c);
                    }
                }
                else
                {
                    if (controlsList.ContainsKey(c.Name))
                        c.Enabled = controlsList[c.Name];
                    else
                    {
                        if (c is DevExpress.XtraEditors.LabelControl)
                            c.Enabled = true;
                        else
                        {
                            if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                            {
                                if (c is DevExpress.XtraEditors.TextEdit)
                                {
                                    ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                                }

                                if (c is lis.client.control.HopePopSelect)
                                {
                                    ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                                }

                            }
                            else
                            {
                                c.Enabled = defaultEnableStatus;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绑定试剂库
        /// </summary>
        private void bindReaSetting()
        {
            ProxyReaSetting proxyReaSetting = new ProxyReaSetting();
            //查询试剂库数据
            List<EntityReaSetting> Items = proxyReaSetting.Service.SearchReaSettingAll();
            this.bsReaSetting.DataSource = Items;
            gvReagentSetting.ExpandAllGroups();
        }
        /// <summary>
        /// 控件启用公用类
        /// </summary>
        /// <param name="read"></param>
        /// <param name="pnlLOT"></param>
        private void SetPnlReadOnly(bool read, Panel pnlLOT)
        {
            foreach (Control c in pnlLOT.Controls)
            {
                if (c is TextEdit)
                    ((TextEdit)c).Properties.ReadOnly = read;

                if (c is HopePopSelect)
                    ((HopePopSelect)c).Readonly = read;

                if (c is CheckEdit)
                    ((CheckEdit)c).Properties.ReadOnly = read;


                if (c is CheckedListBoxControl)
                {
                    CheckedListBoxControl check = (CheckedListBoxControl)c;
                    if (read)
                    {
                        check.SelectionMode = SelectionMode.None;
                    }
                    else
                    {
                        check.SelectionMode = SelectionMode.One;
                    }
                }
            }
        }

        /// <summary>
        /// 选择框发生改变时,刷新
        /// </summary>
        private void RadioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }



        private void txtSort_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            List<EntityReaSetting> list = this.bsReaSetting.DataSource as List<EntityReaSetting>;

            string filterSort = txtSort.Text.Trim();
            if (!string.IsNullOrEmpty(filterSort))
            {
                list = list.Where(w => w.Drea_id.Contains(filterSort) ||
                                                       w.Drea_name != null && w.Drea_name.Contains(filterSort) ||
                                                       w.py_code != null && w.py_code.Contains(filterSort.ToUpper()) ||
                                                       w.wb_code != null && w.wb_code.Contains(filterSort.ToUpper()) ||
                                                       w.Drea_product != null && w.Drea_product.Contains(filterSort) ||
                                                       w.Drea_group != null && w.Drea_group.Contains(filterSort)).ToList();
            }
            this.bsReaSetting.DataSource = list;
        }

        #region 按钮菜单点击事件
        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            bindReaSetting();
        }

        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            Button_Enabled(true);
            SetPnlReadOnly(true, panel1);
            bsReaSetting.EndEdit();
            bindReaSetting();

            defaultEnableStatus = false;
            EnableControls(panelControl3);
        }

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (bsReaSetting.Current != null)
            {
                EntityReaSetting dr = (EntityReaSetting)bsReaSetting.Current;
                string rea_id = dr.Drea_id;
                List<EntityReaSetting> dtRea = bsReaSetting.DataSource as List<EntityReaSetting>;

                if (rea_id == "")
                {
                    dtRea.Remove(dr);
                }
                else
                {
                    DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    switch (dresult)
                    {
                        case DialogResult.OK:
                            base.isActionSuccess = proxy.Service.DeleteReaSetting(dr);
                            break;
                        case DialogResult.Cancel:
                            return;

                    }

                }
                if (base.isActionSuccess)
                {
                    if (rea_id != "")
                    {
                        proxy.Service.LogLogin("删除试剂信息", "试剂库设置", UserInfo.loginID, UserInfo.ip, UserInfo.mac
                    , $"试剂编码{rea_id} 删除成功");
                    }
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要删除的项目", "提示");
                return;
            }
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            bsReaSetting.EndEdit();

            if (this.txtName.Text.ToString().Trim() == "")
            {
                txtName.Focus();
                lis.client.control.MessageDialog.Show("试剂名称不能为空！", "提示");
                return;
            }
            //if (this.txtPackage.Text.ToString().Trim() == "")
            //{
            //    txtPackage.Focus();
            //    lis.client.control.MessageDialog.Show("包装规格不能为空！", "提示");
            //    return;
            //}
            if (selectDicReaUnit1.valueMember == null || selectDicReaUnit1.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("单位不能为空", "提示");
                selectDicReaUnit1.Focus();
                return;
            }
            //if (selectDicReaProduct1.valueMember == null || selectDicReaProduct1.valueMember == string.Empty)
            //{
            //    lis.client.control.MessageDialog.Show("生产厂商不能为空", "提示");
            //    selectDicReaProduct1.Focus();
            //    return;
            //}
            //if (selectDicReaSupplier1.valueMember == null || selectDicReaSupplier1.valueMember == string.Empty)
            //{
            //    lis.client.control.MessageDialog.Show("供货商不能为空", "提示");
            //    selectDicReaSupplier1.Focus();
            //    return;
            //}
            if (selectDicReaGroup1.valueMember == null || selectDicReaGroup1.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("试剂组别不能为空", "提示");
                selectDicReaGroup1.Focus();
                return;
            }

            EntityReaSetting dr = (EntityReaSetting)bsReaSetting.Current;

            EntityResponse result = new EntityResponse();
            bool isNew = true;
            if (string.IsNullOrEmpty(dr.Drea_id))
            {
                //保存试剂库数据
                result = proxy.Service.SaveReaSetting(dr);
                base.isActionSuccess = result.Scusess;
                isNew = true;
            }
            else
            {
                base.isActionSuccess = proxy.Service.UpdateReaSetting(dr);

                isNew = false;
            }
            if (base.isActionSuccess)
            {
                if (dr.Drea_id == "")
                {
                    dr.Drea_id = result.GetResult<EntityReaSetting>().Drea_id;
                }
                if (isNew)
                {
                    MessageDialog.ShowAutoCloseDialog("保存成功!");
                    proxy.Service.LogLogin("新增试剂", "试剂库设置", UserInfo.loginID, UserInfo.ip, UserInfo.mac
                    , $"试剂编码{dr.Drea_id}，名称{dr.Drea_name},新增成功");
                    this.gcReagentSetting.RefreshDataSource();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("修改成功!");
                    proxy.Service.LogLogin("修改质控物", "质控参数设置", UserInfo.loginID, UserInfo.ip, UserInfo.mac
                    , $"试剂编码{dr.Drea_id}，名称{dr.Drea_name},修改成功");
                    this.gcReagentSetting.RefreshDataSource();
                }
            }

            bindReaSetting();//相当于刷新数据

            SetPnlReadOnly(true, panel1);
            Button_Enabled(true);
            this.gcReagentSetting.Focus();

            defaultEnableStatus = false;
            EnableControls(panelControl3);
        }

        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (bsReaSetting.Current != null)
            {
                defaultEnableStatus = true;
                EnableControls(panelControl3);
                SetPnlReadOnly(false, panel1);
                Button_Enabled(false);
                panel1.Enabled = true;
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要修改的项目", "提示");
                return;
            }
        }

        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntityReaSetting drReaSetting = (EntityReaSetting)this.bsReaSetting.AddNew();
            Button_Enabled(false);
            gcReagentSetting.Enabled = false;
            defaultEnableStatus = true;
            EnableControls(panelControl3);
        }

        
        #endregion
    }
}
