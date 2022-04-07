using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;

using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.report;
using dcl.entity;
using System.Linq;

namespace dcl.client.samstock
{
    public partial class FrmSamSearch : FrmCommon
    {
        #region 全局变量
        List<EntityDicSampStoreArea> listCups;

        List<EntitySampStoreDetail> listSamDetail;
        #endregion


        public FrmSamSearch()
        {
            InitializeComponent();
            InitData();

        }

        #region 初始化数据
        private void InitData()
        {
            dateEditFrom.EditValue = DateTime.Now.AddMonths(-1).Date;
            dateEditTo.EditValue = DateTime.Now.Date;

            PoxySampStock proxySamp = new PoxySampStock();
            //冰箱
            List<EntityDicSampStore> dtIceBox = proxySamp.Service.GetIceBox();

            lueIceBox.Properties.DataSource = dtIceBox;
            lueIceBox.Properties.DisplayMember = "StoreName";
            lueIceBox.Properties.ValueMember = "StoreId";
            lueIceBox.Properties.Columns.AddRange(new[]
                {
                    new LookUpColumnInfo("StoreId", "ID", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Near,
                                                                         ColumnSortOrder.None),
                    new LookUpColumnInfo("StoreCode", "编号", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Near,
                                                                         ColumnSortOrder.None),
                    new LookUpColumnInfo("StoreName", "名称", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Default,
                                                                         ColumnSortOrder.None)
                });
            //柜子
            listCups = proxySamp.Service.GetCups();

            lueCupID.Properties.DataSource = listCups;
            lueCupID.Properties.DisplayMember = "AreaName";
            lueCupID.Properties.ValueMember = "AreaId";
            lueCupID.Properties.Columns.AddRange(new[]
                {
                    new LookUpColumnInfo("AreaId", "ID", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Near,
                                                                         ColumnSortOrder.None),
                    new LookUpColumnInfo("AreaCode", "编号", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Near,
                                                                         ColumnSortOrder.None),
                    new LookUpColumnInfo("AreaName", "名称", 20,
                                                                         FormatType.None, "", true,
                                                                         HorzAlignment.Default,
                                                                         ColumnSortOrder.None)
                });

            List<EntityDicSampStoreStatus> listSSStatus = proxySamp.Service.GetSamManageStatus();
            repositoryItemLookUpEdit1.DataSource = listSSStatus;

        }

        private void FrmSamSearch_Load(object sender, EventArgs e)
        {

            sysToolBar.SetToolButtonStyle(new string[]{
                sysToolBar.BtnSearch.Name,
                sysToolBar.BtnPrintList.Name,
                sysToolBar.BtnClose.Name});
            txtRackBarcode.Focus();
        }

        private void FrmSamSearch_Resize(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = Size.Width - 200;
        }
        #endregion



        #region 左右对应内容显示
        private void gvSamDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntitySampStoreDetail eySSDetail = gvSamDetail.GetFocusedRow() as EntitySampStoreDetail;

            if (eySSDetail != null)
            {
                this.txtLBarCode.Text = eySSDetail.DetBarCode;
                this.txtLCombin.Text = eySSDetail.PidComName;
                txtCtypeName.EditValue = eySSDetail.ProName;
                //this.txtLCuvCode.Text = eySSDetail.SrRackId;
                this.txtLCuvCode.Text = eySSDetail.RackBarcode;
                this.txtLName.Text = eySSDetail.PidName;
                //this.txtLSamID.Text = eySSDetail.PidSamId;
                this.txtLSamID.Text = eySSDetail.RepSid;
                this.txtSamname.EditValue = eySSDetail.SamName;
                this.txtLSeq.Text = eySSDetail.DetXY;
                this.txtLSex.Text = eySSDetail.PidSex;
                this.txtLAge.Text = eySSDetail.PidAge;

                //this.txtIceBox.Text = eySSDetail.SrStoreId;
                this.txtIceBox.Text = eySSDetail.StoreName;
                //this.txtCups.Text = eySSDetail.SrPlace;
                this.txtCups.Text = eySSDetail.AreaName;

                //这个方法已经被废掉了，调用bc_sign中的操作方法
                //using (ProxySamManage proxy = new ProxySamManage())
                //{
                //    gridControlBcSign.DataSource = proxy.Service.GetBcSign(eySSDetail.DetBarCode);
                //}
                ProxySampProcessDetail proxyProDetail = new ProxySampProcessDetail();
                List<EntitySampProcessDetail> listProDetail = proxyProDetail.Service.GetSampProcessDetail(eySSDetail.DetBarCode);
                gridControlBcSign.DataSource = listProDetail;

            }
            else
            {
                this.txtLBarCode.EditValue = null;
                this.txtLCombin.EditValue = null;
                this.txtCtypeName.EditValue = null;
                this.txtLCuvCode.EditValue = null;
                this.txtLName.EditValue = null;
                this.txtLSamID.EditValue = null;
                this.txtSamname.EditValue = null;
                this.txtLSeq.EditValue = null;
                this.txtLSex.EditValue = null;
                this.txtLAge.EditValue = null;
                this.txtIceBox.EditValue = null;
                this.txtCups.EditValue = null;
                gridControlBcSign.DataSource = null;
            }
        }
        #endregion

        #region 控件事件

        //打印清单：按钮事件
        private void sysToolBar_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            try
            {
                //DataTable rackDt = gcSamDetail.DataSource as DataTable;
                List<EntitySampStoreDetail> listSStoreDetail = gcSamDetail.DataSource as List<EntitySampStoreDetail>;
                if (listSStoreDetail == null || listSStoreDetail.Count == 0)
                {
                    MessageDialog.Show("没打印数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                        MessageBoxDefaultButton.Button1);
                    return;
                }

                StringBuilder sbBarcode = new StringBuilder();
                foreach (var infoSStoreDetail in listSStoreDetail)
                {
                    if (!string.IsNullOrEmpty(infoSStoreDetail.DetBarCode))
                    {
                        sbBarcode.Append(string.Format(",'{0}'", infoSStoreDetail.DetBarCode.ToString().Trim()));
                    }
                }
                if (sbBarcode.Length == 0)
                {
                    MessageDialog.Show("该标本无条码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button1);
                    return;
                }

                sbBarcode.Remove(0, 1);

                #region 打印清单报表 旧代码
                //List<EntityPrintParameter> listPrint = new List<EntityPrintParameter>();
                //EntityPrintParameter printParmeter = new EntityPrintParameter();
                //printParmeter.Name = "&where&";
                //printParmeter.Value = string.Format(" and SamStore_RackDetail.ssd_bar_code in  ({0})", sbBarcode);
                //listPrint.Add(printParmeter);

                //EntityPrintData printData = new EntityPrintData();
                //printData.Parameters = listPrint;
                //printData.ReportCode = "RackSampListReport";

                //List<EntityPrintData> listPrintData = new List<EntityPrintData>();
                //listPrintData.Add(printData);

                //FrmReportPrint pForm = new FrmReportPrint();
                //pForm.Print3(listPrintData);
                #endregion

                #region 打印清单报表 新代码
                EntityDCLPrintParameter paramter = new EntityDCLPrintParameter();
                paramter.ReportCode = "RackSampListReport";

                Dictionary<String, Object> keyValue = new Dictionary<String, Object>();
                keyValue.Add("SamBarcode", sbBarcode.ToString());
                paramter.CustomParameter = keyValue;

                DCLReportPrint.Print(paramter);
                #endregion

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("打印清单异常信息:", ex);
                MessageDialog.Show("打印清单出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void sysToolBar_OnBtnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                EntityDicSamSearchParamter samSearchParam = new EntityDicSamSearchParamter();
                samSearchParam.DateTimeFrom = Convert.ToDateTime(dateEditFrom.EditValue);
                samSearchParam.DateTimeTo = Convert.ToDateTime(dateEditTo.EditValue);

                if (!string.IsNullOrEmpty(luCtype.valueMember))
                {
                    samSearchParam.RackCtype = luCtype.valueMember;
                }

                if (lueIceBox.EditValue != null && !string.IsNullOrEmpty(lueIceBox.Text))
                {
                    samSearchParam.IceID = lueIceBox.EditValue.ToString();
                }

                if (lueCupID.EditValue != null && !string.IsNullOrEmpty(lueCupID.Text))
                {
                    samSearchParam.CupID = lueCupID.EditValue.ToString();
                }

                if (!string.IsNullOrEmpty(txtRackBarcode.Text))
                {
                    samSearchParam.RackBarcode = txtRackBarcode.Text;
                }

                if (!string.IsNullOrEmpty(txtPatInNo.Text))
                {
                    samSearchParam.PatInNo = txtPatInNo.Text;
                }

                if (!string.IsNullOrEmpty(txtPatName.Text))
                {
                    samSearchParam.PatName = txtPatName.Text;
                }

                if (!string.IsNullOrEmpty(txtStoreMan.Text))
                {
                    samSearchParam.StoreMan = txtStoreMan.Text;
                }

                if (!string.IsNullOrEmpty(txtSamBarcode.Text))
                {
                    samSearchParam.SamBarcode = txtSamBarcode.Text;
                }
                //using (ProxySamManage proxy = new ProxySamManage())
                //{

                //    dtSamDetail = proxy.Service.GetRackQueryData(dFrom, dTo, patInNo, patName, storeMan
                //                                                 , rackCtype, iceID, cupID, rackBarcode, samBarcode);
                //}
                ProxySamSearch proxySamSearch = new ProxySamSearch();
                listSamDetail = proxySamSearch.Service.GetRackQueryData(samSearchParam);
                BindGridView();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("查询出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        private void BindGridView()
        {
            if (listSamDetail != null)
            {
                if (radioGroupFilter.EditValue == null || radioGroupFilter.EditValue.ToString() == "0")
                {
                    gcSamDetail.DataSource = listSamDetail;
                    return;
                }

                if (radioGroupFilter.EditValue.ToString() == "1")
                {
                    //DataRow[] rows = dtSamDetail.Select(string.Format("ssd_satus <> {0}  ", 20));
                    List<EntitySampStoreDetail> listSSD = listSamDetail.Where(w => w.DetStatus != 20).ToList();

                    gcSamDetail.DataSource = listSSD;
                    return;
                }
                if (radioGroupFilter.EditValue.ToString() == "2")
                {
                    //DataRow[] rows = dtSamDetail.Select(string.Format("ssd_satus = {0}  ", 20));
                    List<EntitySampStoreDetail> listSSDetail = listSamDetail.Where(w => w.DetStatus == 20).ToList();

                    gcSamDetail.DataSource = listSSDetail;
                }
            }
        }


        private void lueIceBox_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIceBox.EditValue == null)
            {
                //DataTable dtcupCopy = dtCups.Copy();
                //dtcupCopy.Clear();
                List<EntityDicSampStoreArea> listCupCopy = new List<EntityDicSampStoreArea>();

                lueCupID.Properties.DataSource = listCupCopy;
                return;
            }
            //DataRow[] rows = dtCups.Select(string.Format("cup_ice_id = '{0}'", lueIceBox.EditValue));
            List<EntityDicSampStoreArea> listSSArea = listCups.Where(w => w.StoreId == lueIceBox.EditValue.ToString()).ToList();

            lueCupID.Properties.DataSource = listSSArea;
        }

        private void radioGroupFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
        }

        #endregion

        private void txtRackBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string rackBarcode = txtRackBarcode.Text;
                    if (string.IsNullOrEmpty(rackBarcode))
                    {
                        txtRackBarcode.Focus();
                        return;
                    }
                    //using (ProxySamManage proxy = new ProxySamManage())
                    //{
                    //    dtSamDetail = proxy.Service.GetRackQueryDataByBarcode(rackBarcode, string.Empty);
                    //}
                    ProxySamSearch proxySamSearch = new ProxySamSearch();
                    listSamDetail = proxySamSearch.Service.GetRackQueryDataByBarcode(rackBarcode, string.Empty);

                    BindGridView();
                }
            }
            catch (Exception ex)
            {

                MessageDialog.Show("查询出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                 MessageBoxDefaultButton.Button1);
                txtRackBarcode.Focus();
            }
        }

        private void txtSamBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string samBarcode = txtSamBarcode.Text;
                    if (string.IsNullOrEmpty(samBarcode))
                    {
                        txtSamBarcode.Focus();
                        return;
                    }
                    //using (ProxySamManage proxy = new ProxySamManage())
                    //{
                    //    dtSamDetail = proxy.Service.GetRackQueryDataByBarcode(string.Empty, samBarcode);
                    //}
                    ProxySamSearch proxySamSearch = new ProxySamSearch();
                    listSamDetail = proxySamSearch.Service.GetRackQueryDataByBarcode(string.Empty, samBarcode);
                    BindGridView();
                }
            }
            catch (Exception ex)
            {

                MessageDialog.Show("查询出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                 MessageBoxDefaultButton.Button1);
                txtSamBarcode.Focus();
            }
        }

    }
}
