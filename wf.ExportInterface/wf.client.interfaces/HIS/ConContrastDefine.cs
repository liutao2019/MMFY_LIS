using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using dcl.client.common;
using dcl.common.extensions;
using dcl.common;
using Lib.DataInterface.Implement;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.XtraEditors;

namespace dcl.client.interfaces
{
    [DesignTimeVisible(false)]
    public partial class ConContrastDefine : ConDicCommon
    {
        public ConContrastDefine()
        {
            InitializeComponent();

            string[] types = Enum.GetNames(typeof(CommonValue.RuleType));
            foreach (string typeItem in types)
            {
                this.txtRule.Properties.Items.Add(typeItem);
            }
            this.txtRule.SelectedIndex = 0;
        }
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }
        private SaveAction saveAction = SaveAction.Unknown;
        ProxySysItfContrast proxy = new ProxySysItfContrast();
        List<EntitySysItfInterface> listInter = new List<EntitySysItfInterface>();
        List<EntitySysItfContrast> listContrast = new List<EntitySysItfContrast>();
        private void ConContrastDefine_Load(object sender, EventArgs e)
        {
            //初始化工具条
            barControl.BtnPageUp.Caption = "上一条";
            barControl.BtnPageDown.Caption = "下一条";
            barControl.SetToolButtonStyle(new string[] { barControl.BtnAdd.Name, barControl.BtnModify.Name, barControl.BtnDelete.Name, barControl.BtnSave.Name, barControl.BtnCancel.Name, barControl.BtnPageUp.Name, barControl.BtnPageDown.Name, barControl.BtnClose.Name });
            DoRefresh();
            EnableBaseInfo(false);
            InitOtherData();
        }

        /// <summary>
        /// 修改基础控件是否可编辑
        /// </summary>
        protected void EnableBaseInfo(bool enable)
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (!(item is Label))
                {
                    if (item is TextEdit)
                        (item as TextEdit).Properties.ReadOnly = !enable;

                    if (item is MemoEdit)
                        (item as MemoEdit).Properties.ReadOnly = !enable;

                }
            }
        }

        protected void InitOtherData()
        {
            //插件数据转换
            List<EntityDictDataInterfaceCommand> listCommand = Lib.DataInterface.Implement.CacheDataDemandDataInterface.Current.GetCommands(null);
            listCommand.Insert(0, new EntityDictDataInterfaceCommand());
            this.txtPluginConvertCommand.Properties.DataSource = listCommand;

            if (txtPluginConvertCommand.DataBindings.Count == 0)
                txtPluginConvertCommand.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsContrast, "ContConverter", true));
        }


        protected void SetDefaultValue(EntitySysItfContrast entity)
        {
            entity.ContColumnRule = CommonValue.RuleType.None.ToString();// "None";
        }

        private void cbReturnTable_SelectedValueChanged(object sender, EventArgs e)
        {
            //如果是条码对照表则显示查询排序码，用于条码下载的查询字段自定义
            if (cbReturnTable.Text == "BarcodeInfo")
            {
                if (txtSearchSeq.Visible == false)
                {
                    txtSearchSeq.Visible = true;
                    label7.Visible = true;
                    txtScript.Visible = true;
                    lblScript.Visible = true;
                }
            }
            else
            {
                txtSearchSeq.Visible = false;
                label7.Visible = false;
                txtScript.Visible = false;
                lblScript.Visible = false;
            }
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
        /// 加载接口参数对照
        /// </summary>
        private void LoadContrast()
        {
            if (bsInterfaces.Position > -1)
            {

                EntitySysItfInterface inter = (EntitySysItfInterface)bsInterfaces.Current;
                string id = "";
                if (inter != null && !string.IsNullOrEmpty(inter.ItfaceId))
                {
                    id = inter.ItfaceId;
                }
                listContrast = proxy.Service.GetSysContrast(id);
                bsContrast.DataSource = listContrast;
            }

        }
        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntitySysItfInterface inter = (EntitySysItfInterface)bsInterfaces.Current;
            if (inter != null)
            {
                LoadContrast();
            }
        }
        public void DoRefresh()
        {
            listInter = proxy.Service.GetSysInterface();
            bsInterfaces.DataSource = listInter;
            LoadContrast();
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barControl_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntitySysItfContrast entity = (EntitySysItfContrast)bsContrast.AddNew();
           lookUpEdit1.EditValue = ((EntitySysItfInterface)bsInterfaces.Current).ItfaceName;
            EnableButtonStatus(false);
            SetDefaultValue(entity);
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
            bsContrast.EndEdit();
            if (this.gridView3.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }
            bool isSuccess = false;
            if (saveAction == SaveAction.Add)
            {
                EntitySysItfContrast contrast = (EntitySysItfContrast)bsContrast.Current;
                EntitySysItfInterface inter = (EntitySysItfInterface)bsInterfaces.Current;
                contrast.ContItfaceId = inter.ItfaceId;
                if (string.IsNullOrEmpty(contrast.ContDataType))
                {
                    contrast.ContDataType = "System.String";
                }
                isSuccess = proxy.Service.SaveSysContrast(contrast);
            }
            else if (saveAction == SaveAction.Edit)
            {
                EntitySysItfContrast contrast = (EntitySysItfContrast)bsContrast.Current;
                isSuccess = proxy.Service.UpdateSysContrast(contrast);
            }
            else
            {
                return;
            }

            if (isSuccess)
            {
                DoRefresh();
                LoadContrast();
                EnableButtonStatus(true);
                EnableBaseInfo(false);
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
                this.bsContrast.EndEdit();
                if (this.gridView3.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                EntitySysItfContrast contrast = (EntitySysItfContrast)bsContrast.Current;
                string id = "";
                if (!string.IsNullOrEmpty(contrast.ContId.ToString()))
                {
                    id = contrast.ContId.ToString();
                }
                bool isSuccess = false;
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        isSuccess = proxy.Service.DeleteSysContrast(id);
                        break;
                    case DialogResult.Cancel:
                        return;
                }

                if (isSuccess)
                {
                    listContrast.Remove(contrast);
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