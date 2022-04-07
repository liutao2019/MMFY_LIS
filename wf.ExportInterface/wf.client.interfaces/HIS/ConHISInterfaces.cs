using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.common;
using lis.client.control;

using dcl.common.extensions;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;
using dcl.common;

namespace dcl.client.interfaces
{
    [DesignTimeVisible(false)]
    public partial class ConHISInterfaces : ConDicCommon
    {
        public ConHISInterfaces()
        {
            InitializeComponent();
        }
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }
        private SaveAction saveAction = SaveAction.Unknown;
        ProxySysItfInterface proxy = new ProxySysItfInterface();
        List<EntitySysItfInterface> listInter = new List<EntitySysItfInterface>();
        private void ConHISInterfaces_Load(object sender, EventArgs e)
        {
            //初始化工具条
            barControl.BtnPageUp.Caption = "上一条";
            barControl.BtnPageDown.Caption = "下一条";
            barControl.SetToolButtonStyle(new string[] { barControl.BtnAdd.Name, barControl.BtnModify.Name, barControl.BtnDelete.Name, barControl.BtnSave.Name, barControl.BtnCancel.Name, barControl.BtnPageUp.Name, barControl.BtnPageDown.Name, barControl.BtnClose.Name });
            DoRefresh();
            EnableBaseInfo(false);
        }
        //public  void InitParamters()
        //{
        //    this.subTable = BarcodeTable.Interfaces.TableName;
        //    this.gcSub = gridControl1;
        //    this.gvSub = gridView3;
        //    this.primaryKeyOfSubTable = BarcodeTable.Interfaces.ID;
        //    this.bsSub = bsHISInterfaces;
        //    this.barControl1.BarManager = this;
        //    this.BaseInfoContainer = groupBox1;
        //}

        protected void EnableBaseInfo(bool enable)
        {
            foreach (Control item in layControl.Controls)
            {
                if (!(item is Label))
                {
                    if (item is TextEdit)
                        (item as TextEdit).Properties.ReadOnly = !enable;

                    if (item is MemoEdit)
                        (item as MemoEdit).Properties.ReadOnly = !enable;

                    if (item is HopePopSelect)
                        (item as HopePopSelect).Readonly = !enable;

                }
            }

            //if (BaseInfoContainerExt != null)
            //    foreach (Control item in BaseInfoContainerExt.Controls)
            //    {
            //        if (!(item is Label))
            //        {
            //            if (item is SimpleButton)
            //                (item as SimpleButton).Enabled = enable;
            //        }
            //    }

            //ControlsEnable = enable;
        }

        ///// <summary>
        ///// 清除右边主编辑的所有控件文本
        ///// </summary>
        //protected  void ClearBaseInfo()
        //{
        //    //foreach (Control item in BaseInfoContainer.Controls)
        //    {
        //        if (!(item is Label))
        //        {

        //            if (item is TextEdit)
        //                (item as TextEdit).Text = "";

        //            if (item is MemoEdit)
        //                (item as MemoEdit).Text = "";

        //            if (item is HopePopSelect)
        //            {
        //                (item as HopePopSelect).valueMember = "";
        //                (item as HopePopSelect).displayMember = "";
        //            }
        //        }
        //    }
        //}

        protected void InitOtherData()
        {

            BindingText(txtName, BarcodeTable.Interfaces.Name);
            BindingText(txtDBAddress, BarcodeTable.Interfaces.DBAddress);
            BindingText(cbConnectType, BarcodeTable.Interfaces.DBConnnectType);
            BindingText(txtDBName, BarcodeTable.Interfaces.DBName);
            BindingText(txtDBPassword, BarcodeTable.Interfaces.DBPassword);
            BindingText(txtDBUsername, BarcodeTable.Interfaces.DBUsername);
            BindingText(txtSql, BarcodeTable.Interfaces.InterfaceName);
            BindingText(cbInterfaceType, BarcodeTable.Interfaces.InterfaceFetchType);
            BindingText(cbUseType, BarcodeTable.Interfaces.InterfaceType);

            if (cmbHospital.DataBindings.Count == 5)
            {
                cmbHospital.DataBindings.Add(new Binding("valueMember", this.bsHISInterfaces,
                                                         BarcodeTable.Interfaces.InterfaceHospital, true));
                cmbHospital.DataBindings.Add(new Binding("displayMember", this.bsHISInterfaces, "hos_name", true));
            }
        }

        private void BindingText(Control textbox, string column)
        {
            if (textbox.DataBindings.Count == 0)
                textbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsHISInterfaces, column, true));
        }

        private void btnTestConnenct_Click(object sender, EventArgs e)
        {
            EntitySysItfInterface inter = (EntitySysItfInterface)bsHISInterfaces.Current;
            DataSet dsResult = proxy.Service.TestConnection(inter);
            if (Extensions.IsNotEmpty(dsResult) && Extensions.IsNotEmpty(dsResult.Tables["return"]) && dsResult.Tables["return"].Rows[0]["value"].ToString() == "True")
                lis.client.control.MessageDialog.Show("连接成功!", "提示");
            else
            {
                lis.client.control.MessageDialog.Show("连接失败!", "提示");
            }
        }

        private void cbInterfaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblRemark.Visible = (cbInterfaceType.Text == "SQL");
        }

        public void DoRefresh()
        {
            listInter = proxy.Service.GetSysInterface();
            bsHISInterfaces.DataSource = listInter;
        }
        /// <summary>
        /// 可编辑
        /// </summary>
        public virtual bool EnableGridView(bool enable)
        {
            return gridView3.OptionsBehavior.Editable = enable;
        }
        private void EnableButtonStatus(bool enable)
        {
            barControl.BtnAdd.Enabled = enable;
            barControl.BtnModify.Enabled = enable;
            barControl.BtnDelete.Enabled = enable;
            barControl.BtnSave.Enabled = !enable;
            barControl.BtnCancel.Enabled = !enable;
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntitySysItfInterface entity = (EntitySysItfInterface)bsHISInterfaces.AddNew();
            EnableButtonStatus(false);
            saveAction = SaveAction.Add;
            EnableBaseInfo(true);
        }

        /// <summary>
        /// 放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnCancelClicked(object sender, EventArgs e)
        {
            EnableButtonStatus(true);
            EnableBaseInfo(false);
            DoRefresh();
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnModifyClicked(object sender, EventArgs e)
        {
            EnableButtonStatus(false);
            saveAction = SaveAction.Edit;
            EnableBaseInfo(true);
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnSaveClicked(object sender, EventArgs e)
        {
            bsHISInterfaces.EndEdit();
            if (this.gridView3.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }
            EntityResponse result = new EntityResponse();
            if (saveAction == SaveAction.Add)
            {
                EntitySysItfInterface inter = (EntitySysItfInterface)bsHISInterfaces.Current;
                EntityRequest request = new EntityRequest();
                encryption(inter);
                request.SetRequestValue(inter);
                result = proxy.Service.SaveSysInterface(request);
            }
            else if (saveAction == SaveAction.Edit)
            {
                EntitySysItfInterface inter = (EntitySysItfInterface)bsHISInterfaces.Current;
                EntityRequest request = new EntityRequest();
                encryption(inter);
                request.SetRequestValue(inter);
                result = proxy.Service.UpdateSysInterface(request);
            }
            else
            {
                return;
            }

            if (result.Scusess)
            {
                DoRefresh();
                EnableButtonStatus(true);
                EnableBaseInfo(true);
            }
        }
        private void encryption(EntitySysItfInterface inter)
        {
            if (!string.IsNullOrEmpty(inter.ItfaceServer))
            {
                inter.ItfaceServer = EncryptClass.Encrypt(inter.ItfaceServer);
            }
            if (!string.IsNullOrEmpty(inter.ItfaceDatabase))
            {
                inter.ItfaceDatabase = EncryptClass.Encrypt(inter.ItfaceDatabase);
            }
            if (!string.IsNullOrEmpty(inter.ItfaceLogid))
            {
                inter.ItfaceLogid = EncryptClass.Encrypt(inter.ItfaceLogid);
            }
            if (!string.IsNullOrEmpty(inter.ItfacePassword))
            {
                inter.ItfacePassword = EncryptClass.Encrypt(inter.ItfacePassword);
            }
           
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                this.bsHISInterfaces.EndEdit();
                if (this.gridView3.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }
                EntityResponse response = new EntityResponse();
                EntitySysItfInterface inter = (EntitySysItfInterface)bsHISInterfaces.Current;
                string id = "";
                if (!string.IsNullOrEmpty(inter.ItfaceId))
                {
                    id = inter.ItfaceId;
                }
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        response = proxy.Service.DeleteSysInterface(id);
                        break;
                    case DialogResult.Cancel:
                        return;
                }

                if (response.Scusess)
                {
                    listInter.Remove(inter);
                    gridView3.RefreshData();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException("删除出错", ex);
            }
        }

        /// <summary>
        /// 下移本行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            gridView3.MoveNext();
        }

        /// <summary>
        /// 上移本行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            gridView3.MovePrev();
        }

        private void barControl_OnCloseClicked(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}

