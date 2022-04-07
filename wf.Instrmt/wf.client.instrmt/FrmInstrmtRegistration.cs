using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;

using dcl.client.common;
using System.Collections;
using DevExpress.XtraEditors.Repository;
using lis.client.control;


using dcl.entity;
using System.Linq;
using DevExpress.XtraReports.UI;
using dcl.client.report;

namespace dcl.client.instrmt
{
    public partial class FrmInstrmtRegistration : ConCommon
    {
        public FrmInstrmtRegistration()
        {
            InitializeComponent();

            this.gvMaintRegis.DoubleClick += new EventHandler(gvMaintRegis_DoubleClick);
            this.radioGroup_Item.SelectedIndexChanged += new EventHandler(radioGroup_Item_SelectedIndexChanged);
        }

        void gvMaintRegis_DoubleClick(object sender, EventArgs e)
        {
            if (this.gvMaintRegis.FocusedRowHandle >= 0)
            {
                this.gvMaintRegis.ExpandMasterRow(this.gvMaintRegis.FocusedRowHandle);
            }
        }


        private void radioGroup_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindingInstrmt();
        }

        List<EntityDicInstrument> originalInstrmt = new List<EntityDicInstrument>();

        //全局变量 调用操作数据的方法
        ProxyItrInstrumentRegistration proxyRegistration = new ProxyItrInstrumentRegistration();
        ProxyItrInstrumentMaintain proxyMaintain = new ProxyItrInstrumentMaintain();
        ProxyItrInstrumentServicing proxyServicing = new ProxyItrInstrumentServicing();
        public string repCode = string.Empty;
        protected selectParameter selPar = new selectParameter();
        private void FrmInstrmtRegistration_Load(object sender, EventArgs e)
        {
            BindingInstrmt();
            toolInstrmt.SetToolButtonStyle(new string[] { toolInstrmt.BtnResultJudge.Name,toolInstrmt.BtnSinglePrint.Name });
            toolInstrmt.BtnSinglePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            toolInstrmt.BtnResultJudge.Caption = "维修上报";
            toolMaintain.SetToolButtonStyle(new string[] { toolMaintain.BtnConfirm.Name, toolMaintain.BtnExport.Name });
            toolMaintain.BtnConfirm.Caption = "登记确认";

            tabRegistration.SelectedTabPageIndex = 1;
            tabRegistration.SelectedTabPageIndex = 0;
            toolServicing.SetToolButtonStyle(new string[] { toolServicing.BtnResultJudge.Name, toolServicing.BtnConfirm.Name, toolInstrmt.BtnExport.Name });
            toolServicing.BtnResultJudge.Caption = "处理";
            toolServicing.BtnConfirm.Caption = "审核";
            //保养登记报表代码
            repCode = ConfigHelper.GetSysConfigValueWithoutLogin("Mai_Registration_RepCode");
            if (!string.IsNullOrEmpty(repCode))
            {
                tabReport.PageVisible = true;
                toolInstrmt.BtnSinglePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            lueType.DataSource = proxyMaintain.Service.SearchDicPubProfession();
        }

        private void BindingInstrmt()
        {
            originalInstrmt = proxyMaintain.Service.GetInstrmts(string.Empty);

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
            //gvInstrmt.ExpandAllGroups();
        }

        private void gvInstrmt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvInstrmt.GetFocusedRow() != null)
                {
                    if (tabRegistration.SelectedTabPage.Name == "tabRegis")
                    {
                        GetRegistation();
                    }
                    if (tabRegistration.SelectedTabPage.Name == "tabServicing")
                    {
                        GetServicing();
                    }
                    if (tabRegistration.SelectedTabPage.Name == "tabRegisInfo")
                    {
                        GetRegistationInfo();
                    }
                    if (tabRegistration.SelectedTabPage.Name == "tabReport")
                    {
                        GetRegistationReport();
                    }
                }
                else
                {
                    gcMaintRegis.DataSource = null;
                    gcServicing.DataSource = null;
                }

                gvMaintRegis_FocusedRowChanged(sender, e);//相当于刷新数据

            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }

        }

        //查询保养登记数据
        private void GetRegistation()
        {
            if (gvInstrmt.GetFocusedRow() == null)
                return;

            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

            List<EntityDicInstrmtMaintainRegistration> lisRegis = proxyRegistration.Service.GetRegistration(instrmt.ItrId);

            foreach (var regis in lisRegis)
            {
                switch (regis.RegOperateType.ToString())
                {
                    case "勾选":
                        {
                            RepositoryItemCheckEdit txtEdit = new RepositoryItemCheckEdit();
                            txtEdit.PictureChecked = global::dcl.client.instrmt.Properties.Resources._checked;
                            txtEdit.PictureUnchecked = global::dcl.client.instrmt.Properties.Resources.uncheck;
                            txtEdit.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
                            txtEdit.ValueChecked = "完成";
                            txtEdit.ValueUnchecked = "";
                            regis.RegOperateType = txtEdit;
                            regis.RegExp = string.Format(regis.MaiAstrict, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                            break;
                        }
                    case "枚举":
                        {
                            RepositoryItemComboBox txtEdit = new RepositoryItemComboBox();
                            txtEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                            string[] dict = regis.MaiAstrict.Split(',');

                            for (int i = 0; i < dict.Length; i++)
                            {
                                txtEdit.Items.Add(dict[i]);
                            }
                            regis.RegOperateType = txtEdit;
                            break;
                        }
                    case "数值":
                        {
                            RepositoryItemSpinEdit txtEdit = new RepositoryItemSpinEdit();
                            txtEdit.IsFloatValue = true;
                            try
                            {
                                string[] dict = regis.MaiAstrict.Split(',');
                                txtEdit.MinValue = decimal.Parse(dict[0]);
                                txtEdit.MaxValue = decimal.Parse(dict[1]);
                            }
                            catch
                            { }
                            regis.RegOperateType = txtEdit;
                            break;
                        }
                    default:
                        {
                            RepositoryItemTextEdit txtEdit = new RepositoryItemTextEdit();
                            regis.RegOperateType = txtEdit;
                            break;
                        }
                }
            }
            gcMaintRegis.DataSource = lisRegis; //保养登记
        }

        //查询保养记录数据
        private void GetRegistationInfo()
        {
            if (gvInstrmt.GetFocusedRow() == null)
                return;

            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();
            List<EntityDicInstrmtMaintainRegistration> lisRegis = proxyRegistration.Service.GetRegistrationByDate(instrmt.ItrId);

            gcMaintRegisInfo.DataSource = lisRegis; //保养记录
        }

        //查询维修记录数据
        private void GetServicing()
        {
            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();
            if (instrmt != null)
            {
                List<EntityDicItrInstrumentServicing> lisRegis = proxyServicing.Service.GetServicing(instrmt.ItrId);

                gcServicing.DataSource = lisRegis;//维修记录
            }
        }


        //查询保养记录报表
        private void GetRegistationReport()
        {
            if (gvInstrmt.GetFocusedRow() == null || string.IsNullOrEmpty(repCode))
                return;

            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

            EntityDCLPrintParameter par = new EntityDCLPrintParameter();
            par.ReportCode = repCode;
            par.CustomParameter.Add("ItrId", instrmt.ItrId);
            selPar.Xr = DCLReportPrint.GetXtraReportByPrintData(par);
            if (selPar.Xr == null)
                return;
            prpPar.printControl1.PrintingSystem = selPar.Xr.PrintingSystem;
            prpPar.printPreviewStaticItem3.Caption = "100%";
            prpPar.zoomBarEditItem1.EditValue = "100%";
            prpPar.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing);
            prpPar.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous);
        }
        private void gvMaintRegis_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column == colContent)
            {
                EntityDicInstrmtMaintainRegistration item = gvMaintRegis.GetRow(e.RowHandle) as EntityDicInstrmtMaintainRegistration;
                if (item != null) e.RepositoryItem = (RepositoryItem)item.RegOperateType;
            }
        }

        private void gvMaintRegis_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvMaintRegis.GetFocusedRow() != null)
            {
                EntityDicInstrmtMaintainRegistration operate = (EntityDicInstrmtMaintainRegistration)gvMaintRegis.GetFocusedRow();
                txtOperateTips.EditValue = operate.MaiOperateTips;
                //保养记录
                List<EntityDicInstrmtMaintainRegistration> lisRegisInfo = proxyRegistration.Service.GetRegistrationByDate(operate.RegMaiId.ToString());

                pnlLog.Controls.Clear();
                string content = operate.MaiContent;

                ItrRegistraionLog control = new ItrRegistraionLog();
                control.Init(content, lisRegisInfo);
                control.Dock = DockStyle.Fill;

                pnlLog.Controls.Add(control);
            }
        }

        private void gvMaintRegis_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                EntityDicInstrmtMaintainRegistration operate = (EntityDicInstrmtMaintainRegistration)gvMaintRegis.GetRow(e.RowHandle);

                if (operate.IsOverrunWaringTime)
                    e.Appearance.BackColor = Color.Yellow;
                if (operate.NextMaintainTime != null && operate.NextMaintainTime != string.Empty && !string.IsNullOrEmpty(operate.OverrunIntervalTime))
                    e.Appearance.BackColor = Color.Orange;
                if (operate.IsOverrunAuditTime)
                    e.Appearance.BackColor = Color.Red;
            }
        }

        private void toolInstrmt_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            if (gvInstrmt.GetFocusedRow() != null)
            {
                EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

                FrmInstrmtServicing frmSer = new FrmInstrmtServicing(instrmt.ItrId, instrmt.ItrName);
                frmSer.ShowDialog();
                if (frmSer.DialogResult == DialogResult.OK)
                {
                    GetServicing();
                    instrmt.ItrStatus = "故障";
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要上报的仪器");
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolInstrm_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (gvInstrmt.GetFocusedRow() == null && !string.IsNullOrEmpty(repCode))
            {
                lis.client.control.MessageDialog.Show("请选择要打印的数据！");
                return;
            }
    
            EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

            EntityDCLPrintParameter par = new EntityDCLPrintParameter();
            par.ReportCode = repCode;
            par.CustomParameter.Add("ItrId", instrmt.ItrId);
            DCLReportPrint.Print(par);

        }

        //(保养登记)登记确认按钮
        private void toolMaintain_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            toolInstrmt.Focus();

            List<EntityDicInstrmtMaintainRegistration> lisOperate = (List<EntityDicInstrmtMaintainRegistration>)gcMaintRegis.DataSource;

            if (lisOperate.Count > 0)
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();

                bool isOperate = false;
                foreach (EntityDicInstrmtMaintainRegistration operate in lisOperate)
                {
                    if (operate.RegContent != string.Empty)
                    {
                        isOperate = true;
                        txtOperateTips.EditValue = operate.MaiOperateTips;
                        List<EntityDicInstrmtMaintainRegistration> lisRegisInfo = proxyRegistration.Service.GetRegistrationByDate(operate.RegMaiId.ToString());
                        if (lisRegisInfo.Count>0 && lisRegisInfo[0].RegRegisterDate.Value.Date == operate.RegRegisterDate.Value.Date)
                        {
                            if (lis.client.control.MessageDialog.Show(string.Format("{0}本{1}已执行保养,是否继续？", operate.MaiContent, operate.MaiContent.Substring(0, 1)), MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        break;
                    }
                }
                if (!isOperate)
                {
                    lis.client.control.MessageDialog.Show("请执行保养操作！");
                    return;
                }

                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    List<EntityDicInstrmtMaintainRegistration> lisRegis = new List<EntityDicInstrmtMaintainRegistration>();

                    foreach (var operate in lisOperate)
                    {
                        if (operate.RegContent != string.Empty)
                        {
                            EntityDicInstrmtMaintainRegistration regis = new EntityDicInstrmtMaintainRegistration();

                            if (operate.RegOperateType is RepositoryItemCheckEdit)
                            {
                                regis.RegExp = string.Format(operate.MaiAstrict, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                            }
                            regis.RegContent = operate.RegContent;
                            regis.RegItrId = operate.RegItrId;
                            //regis.RegMaiId = operate.RegMaiId;
                            regis.RegMaiId = operate.MaiId;

                            regis.RegRegisterCode = frmCheck.OperatorID;

                            if (operate.RegRegisterDate != null)
                                regis.LastOperateTime = operate.RegRegisterDate.Value;

                            regis.RegInterval = operate.RegInterval;
                            regis.RegExp = operate.RegExp;

                            regis.OverrunIntervalTime = operate.OverrunIntervalTime;

                            lisRegis.Add(regis);
                        }
                    }
                    if (lisRegis.Count > 0)
                    {
                        if (lisRegis.Count == proxyRegistration.Service.MaintainRegistration(lisRegis))
                        {
                            lis.client.control.MessageDialog.Show("操作成功");
                            gvInstrmt_FocusedRowChanged(null, null);
                        }
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("此仪器无保养计划！");
        }

        private void tabRegistration_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tabRegistration.SelectedTabPage.Name == "tabRegis")
            {
                GetRegistation();
            }
            if (tabRegistration.SelectedTabPage.Name == "tabServicing")
            {
                GetServicing();
            }
            if (tabRegistration.SelectedTabPage.Name == "tabRegisInfo")
            {
                GetRegistationInfo();
            }
            if (tabRegistration.SelectedTabPage.Name == "tabReport")
            {
                GetRegistationReport();
            }
        }

        private void toolServicing_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            if (gvServicing.GetFocusedRow() != null)
            {
                EntityDicItrInstrumentServicing servicing = (EntityDicItrInstrumentServicing)gvServicing.GetFocusedRow();

                if (servicing.SerHandleDate != null) //if (servicing.ser_handle_date != null)
                {
                    lis.client.control.MessageDialog.Show("此维修已处理！");
                    return;
                }

                EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();

                FrmInstrmtServicing frmSer = new FrmInstrmtServicing(instrmt.ItrId, instrmt.ItrName, servicing.SerId, servicing.SerContent);

                frmSer.ShowDialog();
                if (frmSer.DialogResult == DialogResult.OK)
                {
                    GetServicing();
                }
            }
            else
                lis.client.control.MessageDialog.Show("请选择要处理的维修内容！");
        }

        private void toolServicing_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            if (gvServicing.GetFocusedRow() != null)
            {
                EntityDicItrInstrumentServicing servicing = (EntityDicItrInstrumentServicing)gvServicing.GetFocusedRow();

                if (servicing.SerChkDate != null)
                {
                    lis.client.control.MessageDialog.Show("此维修已审核！");
                    return;
                }

                if (servicing.SerHandleDate == null)
                {
                    lis.client.control.MessageDialog.Show("此维修还未处理！");
                    return;
                }

                EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetFocusedRow();
                FrmInstrmtServicing frmSer = new FrmInstrmtServicing(instrmt.ItrId, instrmt.ItrName, servicing.SerId, servicing.SerContent, servicing.SerHandleResult, servicing.SerPrice, servicing.SerInterval);
                frmSer.ShowDialog();

                if (frmSer.DialogResult == DialogResult.OK)
                {
                    GetServicing();
                    instrmt.ItrStatus = "正常";
                }
            }
            else
                lis.client.control.MessageDialog.Show("请选择要审核的数据！");
        }

        private void gvInstrmt_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                EntityDicInstrument instrmt = (EntityDicInstrument)gvInstrmt.GetRow(e.RowHandle);
                if (instrmt.ItrStatus == "停用" || instrmt.ItrStatus == "故障")
                    e.Appearance.ForeColor = Color.Red;
            }
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
                string filter =ToDBC(txtSort.Text.Trim());

                listInstrmt = originalInstrmt.Where(w => w.ItrId.Contains(filter.ToUpper()) ||
                                                         w.ItrName.Contains(filter.ToUpper()) ||
                                                         w.ItrEname.Contains(filter.ToUpper()) ||
                                                         w.WbCode.Contains(filter.ToUpper()) ||
                                                         w.PyCode.Contains(filter.ToUpper())
                                                     ).ToList();
                #region 老版本的过滤方法，摒弃
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

            gvInstrmt_FocusedRowChanged(null, null);
        }
        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ToDBC(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }
        private void toolServicing_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcServicing.DataSource != null)
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
                        //gcServicing.ExportToExcelOld(ofd.FileName.Trim());
                        gcServicing.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(ex.ToString(), "导出异常提示");
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("无导出数据！", "提示");
        }

        private void toolMaintain_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcMaintRegis.DataSource != null)
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
                        //gcMaintRegis.ExportToExcelOld(ofd.FileName.Trim());
                        gcMaintRegis.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(ex.ToString(), "导出异常提示");
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("无导出数据！", "提示");
        }

        public partial class selectParameter
        {
            public selectParameter()
            {
            }
            XtraReport xr;

            /// <summary>
            /// 报表对象
            /// </summary>
            public XtraReport Xr
            {
                get { return xr; }
                set { xr = value; }
            }
            string path = PathManager.SettingPath + @"reportparam.ini";
 
        }
    }
}
