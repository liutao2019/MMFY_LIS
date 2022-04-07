using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Collections;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.oa
{
    public partial class FrmOrderType : FrmCommon
    {
        public FrmOrderType()
        {
            InitializeComponent();
        }
        ProxyOrderTable proxy = new ProxyOrderTable();
        List<EntityOaTable> listTable = new List<EntityOaTable>();
        List<EntityOaTableField> listField = new List<EntityOaTableField>();
        /// <summary>
        /// 单证字段_判断点击保存按钮时是Insert还是Update的标志位
        /// </summary>
        public enum OptionStatus
        {
            Insert,
            Update
        }
        OptionStatus optionStatus = OptionStatus.Update;

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOrderType_Load(object sender, EventArgs e)
        {
            //初始化工具条
            barOrderType.SetToolButtonStyle(new string[] { barOrderType.BtnAdd.Name,barOrderType.BtnDelete.Name });
            barOrderItem.BtnPageUp.Caption = "上移";
            barOrderItem.BtnPageDown.Caption = "下移";
            barOrderItem.SetToolButtonStyle(new string[] { barOrderItem.BtnAdd.Name, barOrderItem.BtnModify.Name, barOrderItem.BtnDelete.Name, barOrderItem.BtnSave.Name, barOrderItem.BtnCancel.Name, barOrderItem.BtnPageUp.Name, barOrderItem.BtnPageDown.Name });

            EnterEditingState(false);

            LoadData();
        }
        private void EnableButtonStatus(bool enable) {
            barOrderItem.BtnAdd.Enabled = enable;
            barOrderItem.BtnModify.Enabled = enable;
            barOrderItem.BtnDelete.Enabled = enable;
            barOrderItem.BtnSave.Enabled = !enable;
            barOrderItem.BtnCancel.Enabled = !enable;
        }
        /// <summary>
        /// 读取单证类型
        /// </summary>
        private void LoadData()
        {
            EnterEditingState(false);
            EntityRequest request = new EntityRequest();
            EntityResponse response = proxy.Service.GetOrderTable(request);
            if (response.Scusess)
            {
                listTable = response.GetResult()as List<EntityOaTable>;
            }
            tvOrderType.DataSource = listTable;
        }

        /// <summary>
        /// 新增单证类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderType_OnBtnAddClicked(object sender, EventArgs e)
        {
            if (txtOrderTypeName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_ORDERTYPENAME_NULL, OfficeMessage.BASE_TITLE);
                txtOrderTypeName.Focus();
                return;
            }
            EntityOaTable table = new EntityOaTable();

            table.TabName = txtOrderTypeName.Text.Trim();
            EntityRequest request= new EntityRequest();
            request.SetRequestValue(table);
            proxy.Service.NewOrderTable(request);
            if (this.isActionSuccess)
            {
                barOrderType.LogMessage = string.Format("保存成功,单证类型名称: {0}", txtOrderTypeName.Text.Trim());
            }

            LoadData();
        }

        /// <summary>
        /// 删除单证类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderType_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (tvOrderType.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }

            EntityOaTable table = new EntityOaTable();
            table.TabCode = tvOrderType.Selection[0].GetValue(colOrderTypeCode).ToString();
            string orderTypeName = tvOrderType.Selection[0].GetValue(colOrderTypeName).ToString();
            EntityOaTableField field = new EntityOaTableField();
            field.TabCode = table.TabCode;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Table", table);
            dict.Add("Field", field);
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dict);
            DialogResult dresult = MessageBox.Show("本操作将删除单证类型[" + orderTypeName + "]和其下的所有单证,非开发人员请勿擅自修改,是否确认?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    proxy.Service.DeleteOrder(request);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (this.isActionSuccess)
            {
                barOrderType.LogMessage = string.Format("删除成功,单证类型名称: {0}", orderTypeName);
            }

            //删除记录后重新加载界面
            LoadData();   
        }

        /// <summary>
        /// 显示单证字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvOrderType_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            txtOrderTypeName.Text = "";

            LoadOrderItem();
        }

        /// <summary>
        /// 显示单证字段信息
        /// </summary>
        private void LoadOrderItem()
        {
            EnterEditingState(false);

            if (tvOrderType.Selection.Count > 0)
            {
                string orderTypeCode = tvOrderType.Selection[0].GetValue(colOrderTypeCode).ToString();
                EntityResponse response = proxy.Service.GetOrderTableField(orderTypeCode.ToString());
                if (response.Scusess)
                {
                    listField = response.GetResult() as List<EntityOaTableField>;
                }
                tvOrderItem.DataSource = listField;
            }
        }

        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(Boolean enable)
        {
            txtOrderItemName.Properties.ReadOnly = !enable;
            cboOrderItemType.Properties.ReadOnly = !enable;
            txtDictName.Properties.ReadOnly = !enable;
            txtDictFilter.Properties.ReadOnly = !enable;
            ckeShowInList.Properties.ReadOnly = !enable;
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnAddClicked(object sender, EventArgs e)
        {
            optionStatus = OptionStatus.Insert;
            EnterEditingState(true);

            txtOrderItemName.Text = "";
            cboOrderItemType.Text = "字符串";
            txtDictName.Text = "";
            txtDictFilter.Text = "";
            txtDictName.Text = "";
            ckeShowInList.Checked = false;

            txtOrderItemName.Focus();
        }

        /// <summary>
        /// 放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnCancelClicked(object sender, EventArgs e)
        {
            tvOrderItem_FocusedNodeChanged(null, null);
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (tvOrderItem.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(true);

            txtOrderItemName.Focus();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.isActionSuccess = false;

            if (tvOrderType.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }

            string orderTypeCode = tvOrderType.Selection[0].GetValue(colOrderTypeCode).ToString();

            if (txtOrderItemName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_ORDERITEMNAME_NULL, OfficeMessage.BASE_TITLE);
                txtOrderItemName.Focus();
                return;
            }

            if (cboOrderItemType.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_ORDERITEMTYPE_NULL, OfficeMessage.BASE_TITLE);
                cboOrderItemType.Focus();
                return;
            }

            EntityOaTableField field = new EntityOaTableField();
            field.TabCode = orderTypeCode;
            field.FieldName = txtOrderItemName.Text.Trim();
            field.FieldType = cboOrderItemType.Text.Trim();
            if (optionStatus == OptionStatus.Update)
            {
                field.FieldIndex = int.Parse(tvOrderItem.Selection[0].GetValue(colOrderItemIndex).ToString());
            }
            else
            {
                field.FieldIndex = int.Parse(DateTime.Now.ToString("yyMMddmmss"));
            }
            field.FieldDictList = txtDictName.Text.Trim();
            field.FieldDictSql = txtDictFilter.Text.Trim();
            field.FieldListDisplay = "0";
            if (ckeShowInList.Checked)
            {
                field.FieldListDisplay = "1";
            }

            if (optionStatus == OptionStatus.Update)
            {
                field.FieldCode = tvOrderItem.Selection[0].GetValue(colOrderItemCode).ToString();
            }
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(field);
            EntityResponse response = new EntityResponse();
            if (optionStatus == OptionStatus.Insert)
            {
                response= proxy.Service.NewOrderTableField(request);
            }
            else
            {
               response= proxy.Service.UpdateOrderTableField(request);
            }

            if (response.Scusess)
            {
                barOrderItem.LogMessage = string.Format("保存成功,单证字段名称: {0}", txtOrderItemName.Text.Trim());
                MessageBox.Show("操作成功");
                EnableButtonStatus(true);
            }

            tvOrderType_FocusedNodeChanged(null, null);
        }

        /// <summary>
        /// 删除单证字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (tvOrderItem.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }
            EntityOaTableField field = new EntityOaTableField();
            field.FieldCode = tvOrderItem.Selection[0].GetValue(colOrderItemCode).ToString();
            string orderItemName = tvOrderItem.Selection[0].GetValue(colOrderItemName).ToString();
            EntityRequest request = new EntityRequest();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Field", field);
            request.SetRequestValue(dict);
            DialogResult dresult = MessageBox.Show("本操作将删除单证字段[" + orderItemName + "]并影响其下的所有单证,是否确认?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    proxy.Service.DeleteOrder(request);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (this.isActionSuccess)
            {
                barOrderType.LogMessage = string.Format("删除成功,单证字段名称: {0}", orderItemName);
            }

            //删除记录后重新加载界面
            tvOrderType_FocusedNodeChanged(null, null);  
        }

        /// <summary>
        /// 下移本行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            if (tvOrderItem.Selection.Count > 0)
            {
                int lastIndex = 0;

                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvOrderItem.Selection[0];
                List<EntityOaTableField> list = new List<EntityOaTableField>();
                for (int i = tvOrderItem.AllNodesCount-1; i >=0; i--)
                {
                    EntityOaTableField field = new EntityOaTableField();
                    DevExpress.XtraTreeList.Nodes.TreeListNode thisTn = tvOrderItem.FindNodeByID(i);

                    field.FieldCode = thisTn.GetValue(colOrderItemCode).ToString();

                    if (thisTn == tn)
                    {
                        if (i == tvOrderItem.AllNodesCount -1)
                        {
                            return;
                        }
                        else
                        {
                            field.FieldIndex = i + 1;
                            lastIndex = i + 1;
                            list[list.Count - 1].FieldIndex = i;
                        }
                    }
                    else
                    {
                        field.FieldIndex = i;
                    }
                    list.Add(field);
                }

                EntityRequest request = new EntityRequest();
                request.SetRequestValue(list);
                proxy.Service.UpdateOrderTableFieldIndex(request);
                tvOrderType_FocusedNodeChanged(null, null);
                tvOrderItem.SetFocusedNode(tvOrderItem.FindNodeByID(lastIndex));
            }
        }

        /// <summary>
        /// 上移本行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderItem_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            if (tvOrderItem.Selection.Count > 0)
            {
                int lastIndex = 0;

                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvOrderItem.Selection[0];
                List<EntityOaTableField> list = new List<EntityOaTableField>();

                for (int i = 0; i < tvOrderItem.AllNodesCount; i++)
                {
                    EntityOaTableField field = new EntityOaTableField();
                    DevExpress.XtraTreeList.Nodes.TreeListNode thisTn = tvOrderItem.FindNodeByID(i);

                    field.FieldCode = thisTn.GetValue(colOrderItemCode).ToString();

                    if (thisTn == tn)
                    {
                        if (i == 0)
                        {
                            return;
                        }
                        else
                        {
                            field.FieldIndex = i - 1;
                            lastIndex = i - 1;
                            list[list.Count - 1].FieldIndex = i;
                        }
                    }
                    else
                    {
                        field.FieldIndex = i;
                    }
                    list.Add(field);
                }
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(list);
                proxy.Service.UpdateOrderTableFieldIndex(request);
                tvOrderType_FocusedNodeChanged(null, null);
                tvOrderItem.SetFocusedNode(tvOrderItem.FindNodeByID(lastIndex));
            }
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvOrderItem_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            EnterEditingState(false);

            if (tvOrderItem.Selection.Count > 0)
            {
                optionStatus = OptionStatus.Update;

                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvOrderItem.Selection[0];
                txtOrderItemName.Text = tn.GetValue(colOrderItemName).ToString();
                cboOrderItemType.Text = tn.GetValue(colOrderItemType).ToString();
                txtDictName.Text = tn.GetValue(colDictName).ToString();
                txtDictFilter.Text = tn.GetValue(colDictFilter).ToString();
                if (tn.GetValue(colInlist).ToString() == "是")
                {
                    ckeShowInList.Checked = true;
                }
                else
                {
                    ckeShowInList.Checked = false;
                }
            }
        }
    }
}
