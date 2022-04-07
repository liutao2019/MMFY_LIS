using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.samstock
{
    public partial class FrmSamDestory : FrmCommon
    {
        #region 全局变量

        List<EntityDicSampStoreArea> listCups;
        //DataTable dtRackSam;
        List<EntitySampStoreRack> listRackSam;
        DateTime? beginDate = null;
        DateTime? endDate = null;

        private string opName = string.Empty;
        private string opID = string.Empty;
        private DateTime? dtFristBarCodeForBatch;
        private bool Sam_EnableDirectOperator = false;
        private int destoryJudgeDay = 7;
        #endregion

        #region 窗体加载

        public FrmSamDestory()
        {
            InitializeComponent();
            gridheadercheckbox = new RepositoryItemCheckEdit();
        }


        private void FrmSamDestory_Load(object sender, EventArgs e)
        {
            Sam_EnableDirectOperator = UserInfo.GetSysConfigValue("Sam_EnableDirectOperator") == "是";
            string days = UserInfo.GetSysConfigValue("Sam_SaveToDestoryDaysTip");
            int day;
            if (!string.IsNullOrEmpty(days) && int.TryParse(days, out day))
            {
                destoryJudgeDay = day;
            }
            if (Sam_EnableDirectOperator)
            {
                labelControl1.Visible = false;
                labelControl2.Visible = false;
                labelControl3.Visible = false;
                labelControl4.Visible = false;
                labelControl5.Visible = false;
                labelControl6.Visible = false;
                labelControl7.Visible = false;
                checkDirectFlag.Checked = true;
                btnFinish.Visible = true;
                dateFrom.Visible = false;
                dateEditTo.Visible = false;
                luCtype.Visible = false;
                lueCupID.Visible = false;
                lueIceBox.Visible = false;
                radioGroupCondition.Visible = false;
                txtBarCode.Visible = false;
            }

            InitData();
            txtRackID.Focus();

            if (Sam_EnableDirectOperator)
            {
                QueryData();
            }
            //给日期范围赋当前时间值
            dateFrom.EditValue = DateTime.Now.AddDays(-1);
            dateEditTo.EditValue = DateTime.Now;

            //sysToolBar.BtnUndoAudit2.Caption = "取消销毁";
            sysToolBar.BtnClear.Caption = "销毁";
            sysToolBar.btnDeleteBatch.Caption = "批量销毁";
            sysToolBar.BtnUndo.Caption = "取消销毁";
            sysToolBar.btnReturn.Caption = "批量取消销毁";

            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnSearch.Name
                , sysToolBar.BtnClear.Name
                ,sysToolBar.btnDeleteBatch.Name
                ,sysToolBar.BtnUndo.Name
                ,sysToolBar.btnReturn.Name
               // , sysToolBar.BtnUndoAudit2.Name
            , sysToolBar.BtnClose.Name});

            sysToolBar.OnBtnSearchClicked += btnQuery_Click;  //查询事件

            sysToolBar.BtnClearClick += btnDestory_Click; //销毁事件
            sysToolBar.OnBtnDeleteBatchClicked += btnBatchDestory_Click; //批量销毁事件
            sysToolBar.BtnUndoClick += btnCancelDestory_Click; //取消销毁事件
            sysToolBar.OnBtnReturnClicked += btnBatchCancelDestroy_Click; //批量取消销毁
            //sysToolBar.OnUndoAuditClicked += btnCancelDestory_Click;

        }

        private void InitData()
        {
            PoxySampStock proxySamp = new PoxySampStock();
            //冰箱
            List<EntityDicSampStore> listIceBox = proxySamp.Service.GetIceBox();

            lueIceBox.Properties.DataSource = listIceBox;
            lueIceBox.Properties.DisplayMember = "StoreName";
            lueIceBox.Properties.ValueMember = "StoreId";
            lueIceBox.Properties.Columns.AddRange(new LookUpColumnInfo[]
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
            lueCupID.Properties.Columns.AddRange(new LookUpColumnInfo[]
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

            List<EntityDicSampStoreStatus> listStoreStatus = proxySamp.Service.GetSamManageStatus();
            repositoryItemLookUpEdit2.DataSource = listStoreStatus;
            repositoryItemLookUpEdit3.DataSource = listStoreStatus;

            repositoryItemLookUpEdit1.DataSource = CacheClient.GetCache<EntityDicSample>();

        }


        private void lueIceBox_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIceBox.EditValue == null)
            {
                List<EntityDicSampStoreArea> listcupCopy = new List<EntityDicSampStoreArea>();

                lueCupID.Properties.DataSource = listcupCopy;
                return;
            }

            List<EntityDicSampStoreArea> listStoreArea = listCups.Where(w => w.StoreId == lueIceBox.EditValue.ToString()).ToList();

            lueCupID.Properties.DataSource = listStoreArea;
        }

        #endregion


        private void radioGroupCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView();
            BindDetail();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                QueryData();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("检索出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

        }

        private void QueryData()
        {
            DateTime? dFrom = null;
            DateTime? dTo = null;
            if (dateFrom.EditValue != null && !string.IsNullOrEmpty(dateFrom.Text))
            {
                dFrom = Convert.ToDateTime(dateFrom.EditValue);
                beginDate = dFrom;
            }
            if (dateEditTo.EditValue != null && !string.IsNullOrEmpty(dateEditTo.Text))
            {
                dTo = Convert.ToDateTime(dateEditTo.EditValue);
                endDate = dTo;
            }
            string iceID = string.Empty;
            string cupID = string.Empty;
            string rackID = string.Empty;
            string barcode = string.Empty;
            string rackCtype = string.Empty;

            if (!string.IsNullOrEmpty(luCtype.valueMember))
            {
                rackCtype = luCtype.valueMember;
            }

            if (lueIceBox.EditValue != null && !string.IsNullOrEmpty(lueIceBox.Text))
            {
                iceID = lueIceBox.EditValue.ToString();
            }

            if (lueCupID.EditValue != null && !string.IsNullOrEmpty(lueCupID.Text))
            {
                cupID = lueCupID.EditValue.ToString();
            }

            if (!string.IsNullOrEmpty(txtRackID.Text))
            {
                rackID = txtRackID.Text;
            }


            if (!string.IsNullOrEmpty(txtBarCode.Text))
            {
                barcode = txtBarCode.Text;
            }

            ProxySamDestory proxyDestory = new ProxySamDestory();
            listRackSam = proxyDestory.Service.GetRackDataForDestory(dFrom, dTo, rackCtype, iceID, cupID, rackID, barcode);

            BindGridView();
        }

        private void BindGridView()
        {
            if (listRackSam != null)
            {
                if (radioGroupCondition.EditValue == null || radioGroupCondition.EditValue.ToString() == "0")//全部
                {
                    gcRackInfo.DataSource = listRackSam;
                    return;
                }

                if (radioGroupCondition.EditValue.ToString() == "1") //未销毁
                {
                    List<EntitySampStoreRack> listStoreRack = listRackSam.Where(w => w.SrStatus == 10 || w.SrStatus == 15).ToList();

                    gcRackInfo.DataSource = listStoreRack;
                    return;
                }
                if (radioGroupCondition.EditValue.ToString() == "2") //已销毁
                {
                    List<EntitySampStoreRack> listRack = listRackSam.Where(w => w.SrStatus == 20).ToList();

                    gcRackInfo.DataSource = listRack;
                }
            }

        }

        //销毁按钮
        private void btnDestory_Click(object sender, EventArgs e)
        {
            try
            {
                EntitySampStoreRack entiityStoreRack = gvRackInfo.GetFocusedRow() as EntitySampStoreRack;

                if (entiityStoreRack == null) return;

                if (entiityStoreRack.SrStatus.ToString() != "10")
                {
                    MessageDialog.Show("该试管架未审核，不能销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                 MessageBoxDefaultButton.Button1);
                    return;
                }

                DialogResult result = MessageDialog.Show("确定要销毁当前试管架所选样本？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
                ProxySamDestory proxySamDestory = new ProxySamDestory();

                string strSsid = entiityStoreRack.SrId;
                string rackID = entiityStoreRack.RackId;
                string iceid = !string.IsNullOrEmpty(entiityStoreRack.SrStoreId) ? entiityStoreRack.SrStoreId : "";
                string cupid = !string.IsNullOrEmpty(entiityStoreRack.SrPlace) ? entiityStoreRack.SrPlace : "";
                gvSamDetail.CloseEditor();

                List<EntitySampStoreDetail> listStoreDetail = gcSamDetail.DataSource as List<EntitySampStoreDetail>;

                if (listStoreDetail.Count == 0)
                {
                    return;
                }
                List<EntitySampStoreDetail> listDetail = listStoreDetail.Where(w => w.DetId == strSsid && w.IsSelected == "1" && w.DetStatus != 20).ToList();

                if (listStoreDetail.Count > 0 && listDetail.Count == 0)
                {
                    MessageDialog.Show("请勾选要销毁的标本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                    return;
                }

                if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = DateTime.Now;
                        opID = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                List<string> barcodeList = new List<string>();
                foreach (var dataDetail in listDetail)
                {
                    barcodeList.Add(dataDetail.DetBarCode);
                }

                proxySamDestory.Service.DestoryRackSam(barcodeList, strSsid, rackID, opName, opID, iceid, cupid, LocalSetting.Current.Setting.Description);

                dtFristBarCodeForBatch = DateTime.Now;

                MessageDialog.Show("销毁成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);

                QueryData();

                SetFocusRow(strSsid);
            }
            catch (Exception ex)
            {

                MessageDialog.Show("检索出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        //取消销毁按钮事件
        private void btnCancelDestory_Click(object sender, EventArgs e)
        {
            try
            {
                //DataRow dr = gvRackInfo.GetFocusedDataRow();
                EntitySampStoreRack eyStoreRack = gvRackInfo.GetFocusedRow() as EntitySampStoreRack;
                if (eyStoreRack == null) return;

                DialogResult result = MessageDialog.Show("确定要取消销毁这些样本？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
                //ProxySamManage proxy = new ProxySamManage();
                ProxySamDestory proxySamDestory = new ProxySamDestory();

                string strSsid = eyStoreRack.SrId;
                string rackID = eyStoreRack.RackId;

                if (!proxySamDestory.Service.CanRollBackDestory(strSsid, rackID))
                {
                    MessageDialog.Show("当前试管架已经重新使用，不能取消销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                    return;
                }

                gvSamDetail.CloseEditor();

                //DataTable detailTb = gcSamDetail.DataSource as DataTable;
                List<EntitySampStoreDetail> listDetail = gcSamDetail.DataSource as List<EntitySampStoreDetail>;

                if (listDetail == null || listDetail.Where(w => w.DetId == strSsid && w.DetStatus == 20).ToList().Count == 0)
                {
                    MessageDialog.Show("无需要取消销毁的标本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                    return;
                }

                //DataRow[] rows = detailTb.Select(string.Format("ssd_id = '{0}' and isselected='{1}' and ssd_satus={2} ", strSsid, "1", 20));
                List<EntitySampStoreDetail> listSSD = listDetail.Where(w => w.DetId == strSsid && w.IsSelected == "1" && w.DetStatus == 20).ToList();

                if (listDetail.Count > 0 && listSSD.Count == 0)
                {
                    MessageDialog.Show("请勾选要取消销毁的标本", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                  MessageBoxDefaultButton.Button1);
                    return;
                }
                if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = DateTime.Now;
                        opID = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                List<string> barcodeList = new List<string>();
                foreach (var infoSSD in listSSD)
                {
                    barcodeList.Add(infoSSD.DetBarCode);
                }

                proxySamDestory.Service.RollBackDestory(barcodeList, strSsid, rackID);

                dtFristBarCodeForBatch = DateTime.Now;

                MessageDialog.Show("取消销毁成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
                QueryData();
                SetFocusRow(strSsid);
            }
            catch (Exception ex)
            {

                MessageDialog.Show("检索出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        private void SetFocusRow(string strSsid)
        {
            //DataRow[] rowArry = dtRackSam.Select(string.Format("ss_id = '{0}'", strSsid));
            List<EntitySampStoreRack> listArry = listRackSam.Where(w => w.SrId == strSsid).ToList();
            if (listArry.Count == 0)
            {
                return;
            }
            EntitySampStoreRack drow = listArry[0];
            int index = listRackSam.IndexOf(drow);

            gvRackInfo.FocusedRowHandle = index;
        }

        private void gvRackInfo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            BindDetail();
        }

        private void BindDetail()
        {
            //DataRow dr = gvRackInfo.GetFocusedDataRow();
            EntitySampStoreRack eySSRack = gvRackInfo.GetFocusedRow() as EntitySampStoreRack;

            //ProxySamManage proxy = new ProxySamManage();
            ProxySamDestory proxySamDestory = new ProxySamDestory();

            if (eySSRack != null)
            {
                string strSsid = eySSRack.SrId;

                //根据ss_id来得到bar_code
                List<EntitySampStoreDetail> listSampSDetail = proxySamDestory.Service.GetRackDetailForDestory(strSsid);
                gcSamDetail.DataSource = listSampSDetail;
            }
            else
            {
                gcSamDetail.DataSource = null;
            }
        }

        #region Grid全选控件CheckBoxOnGridHeader

        void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "colSelect")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        RepositoryItemCheckEdit gridheadercheckbox;
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {
            CheckEditViewInfo info = gridheadercheckbox.CreateViewInfo() as CheckEditViewInfo;
            CheckEditPainter painter = gridheadercheckbox.CreatePainter() as CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            ControlGraphicsInfoArgs args = new ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }
        private bool bGridHeaderCheckBoxState;
        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = gvSamDetail.GridControl.PointToClient(MousePosition);
            GridHitInfo info = gvSamDetail.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "colSelect")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                gvSamDetail.InvalidateColumnHeader(gvSamDetail.Columns["colSelect"]);
                SelectAllPatientInGrid();
            }
        }

        //gcSamDetail控件全选事件
        private void SelectAllPatientInGrid()
        {
            gvSamDetail.CloseEditor();
            List<EntitySampStoreDetail> listSStoreDetail = gcSamDetail.DataSource as List<EntitySampStoreDetail>;
            if (listSStoreDetail == null) return;
            foreach (var infoSStoreDetail in listSStoreDetail)
            {
                infoSStoreDetail.IsSelected = bGridHeaderCheckBoxState ? "1" : "0";
            }
            gcSamDetail.DataSource = listSStoreDetail; //从新给控件赋值
            gcSamDetail.RefreshDataSource();//需要刷新DataSource数据才能实现全选或非全选

        }
        #endregion

        #region 扫描架子条码or样本条码

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string barcode = txtBarCode.Text;
                    if (string.IsNullOrEmpty(barcode))
                    {
                        txtBarCode.Focus();
                        return;
                    }
                    string rackBarCode = "";

                    //DataTable table = gcRackInfo.DataSource as DataTable;
                    List<EntitySampStoreRack> listSSRack = gcRackInfo.DataSource as List<EntitySampStoreRack>;

                    DateTime dFrom = DateTime.Now.AddMonths(-12);
                    DateTime dTo = DateTime.Now;

                    //ProxySamManage proxy = new ProxySamManage();
                    ProxySamDestory proxySamDestory = new ProxySamDestory();
                    //DataTable newdt = proxy.Service.GetRackDataForDestory(dFrom, dTo, null, null, null, null, barcode );
                    List<EntitySampStoreRack> listSamStoreRack = proxySamDestory.Service.GetRackDataForDestory(dFrom, dTo, null, null, null, null, barcode);

                    if (listSamStoreRack == null || listSamStoreRack.Count == 0)
                    {
                        MessageDialog.ShowAutoCloseDialog("找不到相关的信息,该样本可能为归档存储！");
                        txtBarCode.Text = string.Empty;
                        txtBarCode.Focus();
                        return;
                    }

                    if (checkDirectFlag.Checked)
                    {
                        //DataRow dr = newdt.Rows[0];
                        EntitySampStoreRack eySSRack = listSamStoreRack[0];
                        if (eySSRack.SrStatus.ToString() != "10")
                        {
                            MessageDialog.ShowAutoCloseDialog("该样本所属架子:" + eySSRack.RackBarcode + "不是审核状态，不能销毁;");
                            txtBarCode.Text = string.Empty;
                            txtBarCode.Focus();
                            return;
                        }

                        string strSsid = eySSRack.SrId;
                        string rackID = eySSRack.RackId;
                        rackBarCode = eySSRack.RackBarcode;

                        string iceid = !string.IsNullOrEmpty(eySSRack.SrStoreId) ? eySSRack.SrStoreId : "";
                        string cupid = !string.IsNullOrEmpty(eySSRack.SrPlace) ? eySSRack.SrPlace : "";

                        List<EntitySampStoreDetail> detailTb = proxySamDestory.Service.GetRackDetailForDestory(strSsid);

                        List<EntitySampStoreDetail> listSSD = detailTb.Where(w => w.DetId == strSsid && w.DetBarCode == barcode && w.DetStatus == 20).ToList();
                        //DataRow[] rows =
                        //    detailTb.Select(string.Format("ssd_id = '{0}' and ssd_bar_code='{2}'  and ssd_satus<>{1} ", strSsid, 20, barcode));

                        if (detailTb.Count > 0 && listSSD.Count == 0)
                        {
                            MessageDialog.ShowAutoCloseDialog("该架子里的样本已是销毁状态，不能销毁;");
                            txtBarCode.Text = string.Empty;
                            txtBarCode.Focus();
                            return;
                        }
                        if (!string.IsNullOrEmpty(eySSRack.SrAuditDate.ToString()))
                        {
                            DateTime checkdate = eySSRack.SrAuditDate;
                            if (DateTime.Now.Subtract(checkdate).TotalDays < destoryJudgeDay + 1)
                            {
                                DialogResult result = MessageDialog.Show(string.Format("该试管标本没有超过归档时间{0}天，是否销毁？", destoryJudgeDay), "提示", MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                if (result == DialogResult.No)
                                {
                                    txtRackID.Text = string.Empty;
                                    txtRackID.Focus();
                                    return;
                                }
                            }
                        }


                        List<string> barcodeList = new List<string>();

                        barcodeList.Add(barcode);

                        if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                        {
                            FrmCheckPassword checkPassword = new FrmCheckPassword();
                            if (checkPassword.ShowDialog() == DialogResult.OK)
                            {
                                dtFristBarCodeForBatch = DateTime.Now;
                                opID = checkPassword.OperatorID;
                                opName = checkPassword.OperatorName;
                            }
                            else
                            {
                                return;
                            }
                        }

                        proxySamDestory.Service.DestoryRackSam(barcodeList, strSsid, rackID, opName, opID, LocalSetting.Current.Setting.Description, iceid, cupid);
                        dtFristBarCodeForBatch = DateTime.Now;

                    }

                    if (listSSRack == null || listSSRack.Count == 0)
                    {
                        gcRackInfo.DataSource = listSamStoreRack;
                    }
                    else
                    {
                        //DataRow[] rows = table.Select(string.Format("rack_barcode = '{0}'", rackBarCode));
                        List<EntitySampStoreRack> listSSR = listSSRack.Where(w => w.RackBarcode == rackBarCode).ToList();
                        if (listSSR.Count > 0)
                        {
                            listSSRack.Remove(listSSR[0]);
                        }
                        listSSRack.Add(listSamStoreRack[0]);
                        gcRackInfo.RefreshDataSource();
                    }
                    if (listRackSam == null)
                    {
                        listRackSam = listSamStoreRack;
                    }
                    else
                    {
                        //DataRow[] rows = dtRackSam.Select(string.Format("rack_barcode = '{0}'", rackBarCode));
                        List<EntitySampStoreRack> listSampSR = listRackSam.Where(w => w.RackBarcode == rackBarCode).ToList();
                        if (listSampSR.Count > 0)
                        {
                            listRackSam.Remove(listSampSR[0]);
                        }
                        listRackSam.Add(listSamStoreRack[0]);
                    }

                    if (checkDirectFlag.Checked)
                    {
                        radioGroupCondition.EditValue = "2";
                    }
                    txtBarCode.Text = string.Empty;
                    txtBarCode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button1);
            }
        }

        private void txtRackID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string rackBarcode = txtRackID.Text;
                    if (string.IsNullOrEmpty(rackBarcode))
                    {
                        txtRackID.Focus();
                        return;
                    }

                    //DataTable table = gcRackInfo.DataSource as DataTable;
                    List<EntitySampStoreRack> listSStoreR = gcRackInfo.DataSource as List<EntitySampStoreRack>;

                    if (!checkDirectFlag.Checked && listSStoreR.Count > 0)
                    {
                        //试管架已经被存储
                        //DataRow[] rows = table.Select(string.Format("rack_barcode = '{0}'", rackBarcode));
                        List<EntitySampStoreRack> listSSR = listSStoreR.Where(w => w.RackBarcode == rackBarcode).ToList();
                        if (listSSR.Count > 0)
                        {
                            EntitySampStoreRack eySSR = listSSR[0];
                            int index = listSStoreR.IndexOf(eySSR);

                            gvRackInfo.FocusedRowHandle = index;
                            txtBarCode.Focus();
                            return;
                        }
                    }

                    DateTime dFrom = DateTime.Now.AddMonths(-12);
                    DateTime dTo = DateTime.Now;


                    //ProxySamManage proxy = new ProxySamManage();
                    ProxySamDestory proxySamDestory = new ProxySamDestory();
                    //DataTable newdt = proxy.Service.GetRackDataForDestory(dFrom, dTo, null, null, null, rackBarcode,null);
                    List<EntitySampStoreRack> listNewSSR = proxySamDestory.Service.GetRackDataForDestory(dFrom, dTo, null, null, null, rackBarcode, null);

                    if (listNewSSR == null || listNewSSR.Count == 0)
                    {
                        MessageDialog.ShowAutoCloseDialog("找不到相关的信息,该试管可能未有样本归档存储！");
                        txtRackID.Text = string.Empty;
                        txtRackID.Focus();
                        return;
                    }

                    if (checkDirectFlag.Checked)
                    {
                        //DataRow dr = newdt.Rows[0];
                        EntitySampStoreRack eySampSR = listNewSSR[0];
                        if (eySampSR.SrStatus.ToString() != "10")
                        {
                            MessageDialog.ShowAutoCloseDialog("架子:" + eySampSR.RackBarcode + "不是审核状态，不能销毁;");
                            txtRackID.Text = string.Empty;
                            txtRackID.Focus();
                            return;
                        }

                        //string strSsid = dr["ss_id"].ToString();
                        //string rackID = dr["rack_id"].ToString();

                        //string iceid = dr["ss_icebox"] != null && dr["ss_icebox"] != DBNull.Value ? dr["ss_icebox"].ToString() : "";
                        //string cupid = dr["ss_place"] != null && dr["ss_place"] != DBNull.Value ? dr["ss_place"].ToString() : "";
                        string strSsid = eySampSR.SrId;
                        string rackID = eySampSR.RackId;

                        string iceid = !string.IsNullOrEmpty(eySampSR.SrStoreId) ? eySampSR.SrStoreId : "";
                        string cupid = !string.IsNullOrEmpty(eySampSR.SrPlace) ? eySampSR.SrPlace : "";

                        //DataTable detailTb = proxy.Service.GetRackDetailForDestory(strSsid);
                        List<EntitySampStoreDetail> listDetailTb = proxySamDestory.Service.GetRackDetailForDestory(strSsid);

                        if (listDetailTb == null || listDetailTb.Count == 0)
                        {
                            MessageDialog.ShowAutoCloseDialog("该架子无样本，不能销毁;");
                            txtRackID.Text = string.Empty;
                            txtRackID.Focus();
                            return;
                        }
                        //DataRow[] rows =detailTb.Select(string.Format("ssd_id = '{0}'  and ssd_satus<>{1} ", strSsid, 20));
                        List<EntitySampStoreDetail> listSSDetail = listDetailTb.Where(w => w.DetId == strSsid && w.DetStatus != 20).ToList();

                        if (listDetailTb.Count > 0 && listSSDetail.Count == 0)
                        {
                            MessageDialog.ShowAutoCloseDialog("该架子里的样本已是销毁状态，不能销毁;");
                            txtRackID.Text = string.Empty;
                            txtRackID.Focus();
                            return;
                        }
                        if (!string.IsNullOrEmpty(eySampSR.SrAuditDate.ToString()))
                        {
                            DateTime checkdate = Convert.ToDateTime(eySampSR.SrAuditDate);
                            if (DateTime.Now.Subtract(checkdate).TotalDays < destoryJudgeDay + 1)
                            {
                                DialogResult result = MessageDialog.Show(string.Format("该试管标本没有超过归档时间{0}天，是否销毁？", destoryJudgeDay), "提示", MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                if (result == DialogResult.No)
                                {
                                    txtRackID.Text = string.Empty;
                                    txtRackID.Focus();
                                    return;
                                }
                            }
                        }

                        List<string> barcodeList = new List<string>();
                        foreach (var dataDetail in listSSDetail)
                        {
                            barcodeList.Add(dataDetail.DetBarCode);
                        }
                        if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                        {
                            FrmCheckPassword checkPassword = new FrmCheckPassword();
                            if (checkPassword.ShowDialog() == DialogResult.OK)
                            {
                                dtFristBarCodeForBatch = DateTime.Now;
                                opID = checkPassword.OperatorID;
                                opName = checkPassword.OperatorName;
                            }
                            else
                            {
                                return;
                            }
                        }

                        proxySamDestory.Service.DestoryRackSam(barcodeList, strSsid, rackID, opName, opID, LocalSetting.Current.Setting.Description, iceid, cupid);
                        dtFristBarCodeForBatch = DateTime.Now;
                        eySampSR.SrStatus = 20;

                    }

                    if (listSStoreR == null || listSStoreR.Count == 0)
                    {
                        gcRackInfo.DataSource = listNewSSR;
                    }
                    else
                    {
                        //DataRow[] rows = table.Select(string.Format("rack_barcode = '{0}'", rackBarcode));
                        List<EntitySampStoreRack> listSSRack = listSStoreR.Where(w => w.RackBarcode == rackBarcode).ToList();

                        if (listSSRack.Count > 0)
                        {
                            listSStoreR.Remove(listSSRack[0]);
                        }
                        listSStoreR.Add(listNewSSR[0]);
                        gcRackInfo.RefreshDataSource();
                    }
                    if (listRackSam == null)
                    {
                        listRackSam = listNewSSR;
                    }
                    else
                    {
                        //DataRow[] rows = dtRackSam.Select(string.Format("rack_barcode = '{0}'", rackBarcode));
                        List<EntitySampStoreRack> listSSR = listRackSam.Where(w => w.RackBarcode == rackBarcode).ToList();
                        if (listSSR.Count > 0)
                        {
                            listRackSam.Remove(listSSR[0]);
                        }
                        listRackSam.Add(listNewSSR[0]);
                    }

                    if (checkDirectFlag.Checked)
                    {
                        radioGroupCondition.EditValue = "0";

                    }

                    txtRackID.Text = string.Empty;
                    txtRackID.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                     MessageBoxDefaultButton.Button1);
            }

        }

        #endregion

        //gcRackInfo控件全选事件
        private void checkUnAudit_CheckedChanged(object sender, EventArgs e)
        {
            gvRackInfo.CloseEditor();

            List<EntitySampStoreRack> listSampSR = gcRackInfo.DataSource as List<EntitySampStoreRack>;

            if (listSampSR == null || listSampSR.Count == 0) return;

            foreach (var infoSamSR in listSampSR)
            {
                infoSamSR.IsSelected = checkUnAudit.Checked ? 1 : 0;
            }
            gcRackInfo.DataSource = listSampSR;//将勾选状态从新赋值给gcRackInfo
            gcRackInfo.RefreshDataSource();//刷新DataSource数据
        }

        //批量销毁按钮和SystoolBar中的批量销毁按钮事件
        private void btnBatchDestory_Click(object sender, EventArgs e)
        {
            try
            {
                gvRackInfo.CloseEditor();

                //DataTable dt = gcRackInfo.DataSource as DataTable;
                List<EntitySampStoreRack> listSSRack = gcRackInfo.DataSource as List<EntitySampStoreRack>;

                if (listSSRack == null || listSSRack.Count == 0) return;

                //DataRow[] rowSams = dt.Select(string.Format("isselected=1 "));
                List<EntitySampStoreRack> listSSR = listSSRack.Where(w => w.IsSelected == 1).ToList();

                if (listSSR.Count == 0)
                {
                    MessageDialog.Show("请勾选要销毁的架子", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                         MessageBoxDefaultButton.Button1);
                    return;
                }
                DialogResult result = MessageDialog.Show("确定要销毁所勾选试管架中的样本？", "提示", MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }

                if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = DateTime.Now;
                        opID = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                string message = string.Empty;

                //ProxySamManage proxy = new ProxySamManage();
                ProxySamDestory proxySamDestore = new ProxySamDestory();

                foreach (var infoSSR in listSSR)
                {

                    if (infoSSR.SrStatus.ToString() != "10")
                    {
                        message += "架子:" + infoSSR.RackBarcode + "不是审核状态，不能销毁;\r\n";
                        continue;
                    }

                    //string strSsid = dr["ss_id"].ToString();
                    //string rackID = dr["rack_id"].ToString();

                    //string iceid = dr["ss_icebox"] != null && dr["ss_icebox"] != DBNull.Value ? dr["ss_icebox"].ToString() : "";
                    //string cupid = dr["ss_place"] != null && dr["ss_place"] != DBNull.Value ? dr["ss_place"].ToString() : "";
                    string strSsid = infoSSR.SrId;
                    string rackID = infoSSR.RackId;

                    string iceid = !string.IsNullOrEmpty(infoSSR.SrStoreId) ? infoSSR.SrStoreId : "";
                    string cupid = !string.IsNullOrEmpty(infoSSR.SrPlace) ? infoSSR.SrPlace : "";

                    //DataTable detailTb = proxySamDestore.Service.GetRackDetailForDestory(strSsid);
                    List<EntitySampStoreDetail> listDetailTb = proxySamDestore.Service.GetRackDetailForDestory(strSsid);

                    if (listDetailTb == null || listDetailTb.Count == 0)
                    {
                        continue;
                    }
                    //DataRow[] rows =detailTb.Select(string.Format("ssd_id = '{0}'  and ssd_satus<>{1} ", strSsid, 20));
                    List<EntitySampStoreDetail> listDetail = listDetailTb.Where(w => w.DetId == strSsid && w.DetStatus != 20).ToList();

                    if (listDetailTb.Count > 0 && listDetail.Count == 0)
                    {
                        continue;
                    }

                    List<string> barcodeList = new List<string>();
                    foreach (var infoDetail in listDetail)
                    {
                        barcodeList.Add(infoDetail.DetBarCode);
                    }

                    proxySamDestore.Service.DestoryRackSam(barcodeList, strSsid, rackID, opName,
                                                 opID, LocalSetting.Current.Setting.Description, iceid, cupid);
                    dtFristBarCodeForBatch = DateTime.Now;
                }


                MessageDialog.Show("操作成功！\r\n" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);

                QueryData();


            }
            catch (Exception ex)
            {

                MessageDialog.Show("检索出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        //批量取消销毁按钮
        private void btnBatchCancelDestroy_Click(object sender, EventArgs e)
        {
            try
            {
                this.gvRackInfo.CloseEditor();

                //DataTable dt = gcRackInfo.DataSource as DataTable;
                List<EntitySampStoreRack> listSSR = gcRackInfo.DataSource as List<EntitySampStoreRack>;

                if (listSSR == null || listSSR.Count == 0) return;

                //DataRow[] rowSams = dt.Select(string.Format("isselected=1 "));
                List<EntitySampStoreRack> listSSRack = listSSR.Where(w => w.IsSelected == 1).ToList();

                if (listSSRack.Count == 0)
                {
                    MessageDialog.Show("请勾选要取消销毁的架子", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                         MessageBoxDefaultButton.Button1);
                    return;
                }
                DialogResult result = MessageDialog.Show("确定要批量取消销毁所勾选试管架中的所有样本？", "提示", MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }

                if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(opID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = DateTime.Now;
                        opID = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                string message = string.Empty;

                //ProxySamManage proxy = new ProxySamManage();
                ProxySamDestory proxySamDestory = new ProxySamDestory();

                foreach (var infoSSRack in listSSRack)
                {
                    string strSsid = infoSSRack.SrId;
                    string rackID = infoSSRack.RackId;

                    if (!proxySamDestory.Service.CanRollBackDestory(strSsid, rackID))
                    {
                        //zhe
                        message += "架子:" + infoSSRack.RackBarcode + "已经重新使用，不能取消销毁;\r\n";
                        continue;
                    }


                    //DataTable detailTb = proxySamDestory.Service.GetRackDetailForDestory(strSsid);
                    List<EntitySampStoreDetail> listSampSDetail = proxySamDestory.Service.GetRackDetailForDestory(strSsid);

                    if (listSampSDetail == null || listSampSDetail.Count == 0)
                    {
                        continue;
                    }
                    //if (detailTb.Select(string.Format("ssd_id = '{0}'  and ssd_satus={1} ", strSsid, 20)).Length == 0)
                    if (listSampSDetail.Where(w => w.DetId == strSsid && w.DetStatus == 20).ToList().Count == 0)
                    {
                        continue;
                    }
                    //DataRow[] rows = detailTb.Select(string.Format("ssd_id = '{0}'  and ssd_satus={1} ", strSsid, 20));
                    List<EntitySampStoreDetail> listSSD = listSampSDetail.Where(w => w.DetId == strSsid && w.DetStatus == 20).ToList();

                    if (listSampSDetail.Count > 0 && listSSD.Count == 0)
                    {
                        continue;
                    }
                    List<string> barcodeList = new List<string>();
                    foreach (var infoSSD in listSSD)
                    {
                        barcodeList.Add(infoSSD.DetBarCode);
                    }

                    proxySamDestory.Service.RollBackDestory(barcodeList, strSsid, rackID);
                }

                MessageDialog.Show("操作成功！\r\n" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);

                QueryData();

            }
            catch (Exception ex)
            {
                MessageDialog.Show("检索出错" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
            }
        }

        private void checkDirectFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDirectFlag.Checked)
            {
                dateFrom.EditValue = null;
                dateEditTo.EditValue = null;
                labelControl1.Visible = false;
                labelControl2.Visible = false;
                labelControl3.Visible = false;
                labelControl4.Visible = false;
                labelControl5.Visible = false;
                labelControl6.Visible = false;
                labelControl7.Visible = false;
                btnFinish.Visible = true;
                dateFrom.Visible = false;
                dateEditTo.Visible = false;
                luCtype.Visible = false;
                lueCupID.Visible = false;
                lueIceBox.Visible = false;
                radioGroupCondition.Visible = false;
                txtBarCode.Visible = false;
            }
            else
            {
                if (beginDate != null && endDate != null)
                {
                    dateFrom.EditValue = beginDate;
                    dateEditTo.EditValue = endDate;
                }
                else
                {
                    dateFrom.EditValue = DateTime.Now.AddDays(-7);
                    dateEditTo.EditValue = DateTime.Now;
                }

                labelControl1.Visible = true;
                labelControl2.Visible = true;
                labelControl3.Visible = true;
                labelControl4.Visible = true;
                labelControl5.Visible = true;
                labelControl6.Visible = true;
                labelControl7.Visible = true;
                btnFinish.Visible = false;
                dateFrom.Visible = true;
                dateEditTo.Visible = true;
                luCtype.Visible = true;
                lueCupID.Visible = true;
                lueIceBox.Visible = true;
                radioGroupCondition.Visible = true;
                txtBarCode.Visible = true;
            }
        }

        //事件按钮：完成
        private void btnFinish_Click(object sender, EventArgs e)
        {
            List<EntitySampStoreRack> listSSR = gcRackInfo.DataSource as List<EntitySampStoreRack>;
            if (listSSR != null && listSSR.Count > 0)
            {
                listSSR.Clear();
                if (listRackSam != null)
                    listRackSam.Clear();

                this.gcRackInfo.RefreshDataSource();
            }
        }
    }
}
