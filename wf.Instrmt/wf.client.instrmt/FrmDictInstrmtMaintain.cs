using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.instrmt
{
    public partial class FrmDictInstrmtMaintain : ConCommon
    {
        public FrmDictInstrmtMaintain()
        {
            InitializeComponent();
            this.cbFWarnningTime.Visible = false;
            this.radioGroup_Item.SelectedIndexChanged += new EventHandler(radioGroup_Item_SelectedIndexChanged);
        }
        private void radioGroup_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindingInstrmt();
        }

        bool isNewAdd = false;
        
        List<EntityDicInstrument> originalInstrmt = new List<EntityDicInstrument>();

        //全局变量 用来调用数据的操作方法
        private ProxyItrInstrumentMaintain proxy = new ProxyItrInstrumentMaintain();

        private void FrmDictInstrmtMaintain_Load(object sender, EventArgs e)
        {
            BindingInstrmt();
            toolInstrmt.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh", toolInstrmt.BtnExport.Name });
            EnableOperation(true);
            lueType.DataSource = proxy.Service.SearchDicPubProfession();
            List<EntityDicPubEvaluate> listDesc = CacheClient.GetCache<EntityDicPubEvaluate>().FindAll(w=>w.EvaFlag=="23");
            if (listDesc.Count > 0)
            {
                foreach (EntityDicPubEvaluate pub in listDesc)
                {
                    cmbDesc.Properties.Items.Add(pub.EvaContent);
                }
            }
        }


        private void EnableOperation(bool isEnable)
        {
            toolInstrmt.BtnAdd.Enabled = isEnable;
            toolInstrmt.BtnModify.Enabled = isEnable;
            toolInstrmt.BtnDelete.Enabled = isEnable;
            toolInstrmt.BtnSave.Enabled = !isEnable;
            toolInstrmt.BtnCancel.Enabled = !isEnable;
            toolInstrmt.BtnRefresh.Enabled = isEnable;

            gcInstrmt.Enabled = isEnable;
            gcInsMaintain.Enabled = isEnable;
            EnableControls(plOperate, !isEnable);
        }

        /// <summary>
        /// 绑定仪器数据源
        /// </summary>
        private void BindingInstrmt()
        {
            originalInstrmt = proxy.Service.GetInstrmts(string.Empty);

            if (radioGroup_Item.EditValue.ToString() == "1")
            {
                originalInstrmt = originalInstrmt.Where(w => w.DelFlag == "0" && w.ItrStatus != "停用").ToList();
            }
            else if (radioGroup_Item.EditValue.ToString() == "2")
            {
                originalInstrmt = originalInstrmt.Where(w => w.DelFlag == "0" && w.ItrStatus == "停用").ToList();
            }
            else
            {
                originalInstrmt = originalInstrmt.Where(w => w.DelFlag == "0").ToList();
            }
            gcInstrmt.DataSource = originalInstrmt;
            gvInstrmt.ExpandAllGroups();

            BindingMaintain();
        }

        private void BindingMaintain()
        {
            if (gvInstrmt.GetFocusedRow() != null)
            {
                EntityDicInstrument instrmt = gvInstrmt.GetFocusedRow() as EntityDicInstrument;
                string itr_id = instrmt.ItrId;
                
                gcInsMaintain.DataSource = proxy.Service.GetInstrmtMaintains(itr_id);
            }

            gvInsMaintain_FocusedRowChanged(null, null);

        }

        /// <summary>
        /// 遍历所有控件，以控制所有控件显示是否可编辑状态
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="isTrue"></param>
        private void EnableControls(Control parentControl, bool isTrue)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c.Name != "txtMaiId" && !(c is DevExpress.XtraEditors.LabelControl))
                {
                    if (c is lis.client.control.HopePopSelect)
                        ((lis.client.control.HopePopSelect)c).Readonly = !isTrue;
                    else if (c is DevExpress.XtraEditors.BaseEdit)
                    {
                        ((DevExpress.XtraEditors.BaseEdit)c).Properties.ReadOnly = !isTrue;
                    }
                    else
                    {
                        c.Enabled = isTrue;
                    }
                }
            }
        }

        private void gvInstrmt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            BindingMaintain();

        }

        private void gvInsMaintain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvInsMaintain.GetFocusedRow() != null)
            {
                EntityDicItrInstrumentMaintain instrmtMaintain = (EntityDicItrInstrumentMaintain)gvInsMaintain.GetFocusedRow();
                SetControlsValue(instrmtMaintain);
            }
            else
                SetControlsValue(new EntityDicItrInstrumentMaintain());
        }

        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="instrmt"></param>
        private void SetControlsValue(EntityDicItrInstrumentMaintain maintain)
        {
            if (maintain != null)
            {
                txtMaiId.EditValue = maintain.MaiId;
                txtMaiContent.EditValue = maintain.MaiContent;
                cmbMaiType.EditValue = maintain.MaiType != null && maintain.MaiType.Trim() != string.Empty ? maintain.MaiType : "勾选";
                txtMaiAstrict.EditValue = maintain.MaiAstrict;

                txtMaiIntervalDay.EditValue = maintain.MaiIntervalDay;
                txtMaiIntervalMonth.EditValue = maintain.MaiIntervalMonth;
                txtMaiIntervalYear.EditValue = maintain.MaiIntervalYear;
                txtMaiIntervalHour.EditValue =
                    maintain.MaiInterval - maintain.MaiIntervalDay * 24 - maintain.MaiIntervalMonth * 30 * 24 - maintain.MaiIntervalYear * 365 * 24;

                txtMaiWarningTimeDay.EditValue = maintain.MaiWarningTimeDay;
                txtMaiWarningTimeMonth.EditValue = maintain.MaiWarningTimeMonth;
                txtMaiWarningTimeYear.EditValue = maintain.MaiWarningTimeYear;
                txtMaiWarningTimeHour.EditValue =
                maintain.MaiWarningTime - maintain.MaiWarningTimeDay * 24 - maintain.MaiWarningTimeMonth * 30 * 24 - maintain.MaiWarningTimeYear * 365 * 24;

                txtMaiCloseAuditTimeDay.EditValue = maintain.MaiCloseAuditTimeDay;
                txtMaiCloseAuditTimeMonth.EditValue = maintain.MaiCloseAuditTimeMonth;
                txtMaiCloseAuditTimeYear.EditValue = maintain.MaiCloseAuditTimeYear;
                txtMaiCloseAuditTimeHour.EditValue =
                maintain.MaiCloseAuditTime - maintain.MaiCloseAuditTimeDay * 24 - maintain.MaiCloseAuditTimeMonth * 30 * 24 - maintain.MaiCloseAuditTimeYear * 365 * 24;
                txtmaiOperateTips.EditValue = maintain.MaiOperateTips;

                this.cbFCloseAuTime.Checked = maintain.FuzzyCloseAuditTime == "1";
                this.cbFInvTime.Checked = maintain.FuzzyIntervalTime == "1";
                this.cbFWarnningTime.Checked = maintain.FuzzyWarningTime == "1";
                cmbDesc.Text = maintain.MaiDesc;
            }
        }

        private void SetTime(DevExpress.XtraEditors.CheckEdit ck, DevExpress.XtraEditors.TextEdit txt, int hour)
        {
            if (ck.Checked)
            {
                txt.EditValue = hour;
            }
        }

        //新增按钮事件
        private void toolInstrmt_OnBtnAddClicked(object sender, EventArgs e)
        {
            EnableControls(plOperate, true);
            isNewAdd = true;
            SetControlsValue(new EntityDicItrInstrumentMaintain());
            EnableOperation(false);
        }

        //修改按钮事件
        private void toolInstrmt_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (gvInsMaintain.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请选择要修改的数据！", "提示");
                return;
            }
            isNewAdd = false;
            EnableControls(plOperate, true);
            EnableOperation(false);
        }

        //保存按钮事件
        private void toolInstrmt_OnBtnSaveClicked(object sender, EventArgs e)
        {
            bool isPassVerification = true;

            if (txtMaiContent.EditValue == null || txtMaiContent.EditValue.ToString().Trim() == string.Empty)
            {
                isPassVerification = false;
                txtMaiContent.Focus();
            }
            if (cmbMaiType.EditValue == null || cmbMaiType.EditValue.ToString().Trim() == string.Empty)
            {
                isPassVerification = false;
                cmbMaiType.Focus();
            }
            if (!isPassVerification)
            {
                lis.client.control.MessageDialog.Show("带*号的为必填项！", "提示");
                return;
            }
            
            EntityDicItrInstrumentMaintain maintain = new EntityDicItrInstrumentMaintain();

            if (!isNewAdd)
            {
                maintain = (EntityDicItrInstrumentMaintain)gvInsMaintain.GetFocusedRow();
            }
            
            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

            maintain.MaiItrId = instrmt.ItrId;
            maintain.MaiId = txtMaiId.Text != null && txtMaiId.Text != string.Empty ? Convert.ToInt32(txtMaiId.Text) : 0;
            maintain.MaiContent = txtMaiContent.Text;
            maintain.MaiType = cmbMaiType.Text;
            maintain.MaiAstrict = txtMaiAstrict.Text;

            maintain.MaiIntervalDay = Convert.ToInt32(txtMaiIntervalDay.EditValue);
            maintain.MaiIntervalMonth = Convert.ToInt32(txtMaiIntervalMonth.EditValue);
            maintain.MaiIntervalYear = Convert.ToInt32(txtMaiIntervalYear.EditValue);

            maintain.MaiWarningTimeDay = Convert.ToInt32(txtMaiWarningTimeDay.EditValue);
            maintain.MaiWarningTimeMonth = Convert.ToInt32(txtMaiWarningTimeMonth.EditValue);
            maintain.MaiWarningTimeYear = Convert.ToInt32(txtMaiWarningTimeYear.EditValue);

            maintain.MaiCloseAuditTimeDay = Convert.ToInt32(txtMaiCloseAuditTimeDay.EditValue);
            maintain.MaiCloseAuditTimeMonth = Convert.ToInt32(txtMaiCloseAuditTimeMonth.EditValue);
            maintain.MaiCloseAuditTimeYear = Convert.ToInt32(txtMaiCloseAuditTimeYear.EditValue);

            maintain.MaiInterval =
            maintain.MaiIntervalDay * 24 + maintain.MaiIntervalMonth * 30 * 24 + maintain.MaiIntervalYear * 365 * 24 + Convert.ToInt32(txtMaiIntervalHour.EditValue);
            maintain.MaiWarningTime =
            maintain.MaiWarningTimeDay * 24 + maintain.MaiWarningTimeMonth * 30 * 24 + maintain.MaiWarningTimeYear * 365 * 24 + Convert.ToInt32(txtMaiWarningTimeHour.EditValue);
            maintain.MaiCloseAuditTime =
            maintain.MaiCloseAuditTimeDay * 24 + maintain.MaiCloseAuditTimeMonth * 30 * 24 + maintain.MaiCloseAuditTimeYear * 365 * 24 + Convert.ToInt32(txtMaiCloseAuditTimeHour.EditValue);
            maintain.MaiOperateTips = txtmaiOperateTips.Text;

            maintain.FuzzyCloseAuditTime = this.cbFCloseAuTime.Checked ? "1" : "0";
            maintain.FuzzyIntervalTime = this.cbFInvTime.Checked ? "1" : "0";
            maintain.FuzzyWarningTime = this.cbFWarnningTime.Checked ? "1" : "0";
            maintain.MaiDesc = cmbDesc.Text;
            bool isSuccess = false;
            if (isNewAdd)
            {
                isSuccess = proxy.Service.AddInstrmtMaintain(maintain); 
            }
            else
            {
                isSuccess = proxy.Service.UpdateInstrmtMaintain(maintain);
            }
            if (isSuccess)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("操作成功!");

                EnableOperation(true);
                BindingMaintain();
            }
        }

        //删除按钮事件
        private void toolInstrmt_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (gvInstrmt.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }

            if (lis.client.control.MessageDialog.Show("是否要删除所选仪器？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EntityDicItrInstrumentMaintain instrmtMaintain = (EntityDicItrInstrumentMaintain)gvInsMaintain.GetFocusedRow();
                
                if (proxy.Service.DeleteInstrmtMaintainByID(instrmtMaintain.MaiId))
                {
                    lis.client.control.MessageDialog.Show("操作成功！", "提示");
                    EnableOperation(true);
                    BindingMaintain();
                }
            }
        }

        //刷新按钮事件
        private void toolInstrmt_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            BindingMaintain();
        }

        //取消按钮事件
        private void toolInstrmt_OnBtnCancelClicked(object sender, EventArgs e)
        {
            BindingMaintain();
            EnableOperation(true);
        }

        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            if (txtSort.Text == null || txtSort.Text.Trim() == string.Empty)
            {
                gcInstrmt.DataSource = originalInstrmt;
                gvInstrmt.ExpandAllGroups();
            }
            else
            {
                List<EntityDicInstrument> listInstrmt = new List<EntityDicInstrument>();
                string filter = txtSort.Text.Trim();

                listInstrmt = originalInstrmt.Where(w => w.ItrId.Contains(filter.ToUpper()) ||
                                                         w.ItrName.Contains(filter.ToUpper()) ||
                                                         w.ItrEname.Contains(filter.ToUpper()) ||
                                                         w.WbCode.Contains(filter.ToUpper()) ||
                                                         w.PyCode.Contains(filter.ToUpper())
                                                     ).ToList();
                #region 老代码过滤的方法，摒弃
                //foreach (var instrmt in originalInstrmt)
                //{
                //    if (instrmt.ItrId.IndexOf(txtSort.Text.Trim()) >= 0)
                //        listInstrmt.Add(instrmt);
                //    else if (instrmt.ItrName.IndexOf(txtSort.Text.Trim()) >= 0)
                //        listInstrmt.Add(instrmt);
                //    else if (instrmt.ItrEname.IndexOf(txtSort.Text.Trim()) >= 0)
                //        listInstrmt.Add(instrmt);
                //    else if (instrmt.WbCode.IndexOf(txtSort.Text.Trim()) >= 0)
                //        listInstrmt.Add(instrmt);
                //    else if (instrmt.PyCode.IndexOf(txtSort.Text.Trim()) >= 0)
                //        listInstrmt.Add(instrmt);
                //}
                #endregion

                gcInstrmt.DataSource = listInstrmt;
                gvInstrmt.ExpandAllGroups();
            }
        }

        //导出按钮事件
        private void toolInstrmt_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcInsMaintain.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcInsMaintain.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(),"导出异常提醒");
                    }
                }

            }
        }

    }
}
