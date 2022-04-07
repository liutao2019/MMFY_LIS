using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.common;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.samstock
{
    public partial class FrmSamStoreRecord : FrmCommon
    {
        #region 全局变量
        private List<EntityDicTubeRack> dtRackSpec;
        /// <summary>
        /// 试管架内容
        /// </summary>
        private List<EntityDicSampTubeRack> dtRack;

        /// <summary>
        /// 试管架内容
        /// </summary>
        private List<EntityDicSampTubeRack> dtRackBatch;

        /// <summary>
        /// 试管架内容
        /// </summary>
        private List<EntitySampStoreDetail> dtPatiens = null;

        /// <summary>
        /// 样本详细信息表
        /// </summary>
        //private List<EntityDicSampTubeRack> dtRackPat;

        /// <summary>
        /// 当前选中试管架编号
        /// </summary>
        private string strSsid;
        /// <summary>
        /// 试管架字典的ID号
        /// </summary>
        private string strRackID;
        /// <summary>
        /// 当前选中的试管架的状态
        /// </summary>
        private int intSsStauts = -1;

        private int intSeq = 1;

        private bool isHandWork = true;

        private DateTime? dtFristBarCode;
        private string operatorID = "";
        private string operatorName = "";


        private DateTime? dtFristBarCodeForBatch;
        private string operatorIDForBatch = "";
        private string operatorNameForBatch = "";



        private DateTime? dtFristBarCodeForHand;
        private string operatorIDForHand = "";
        private string operatorNameForHand = "";
        #endregion

        #region 初始化数据
        public FrmSamStoreRecord()
        {
            InitializeComponent();
        }

        private void FrmSamStoreRecord_Load(object sender, EventArgs e)
        {
            InitData();
            txtRackID.Focus();
            if (UserInfo.GetSysConfigValue("Sam_EnableDirectOperator") == "是")
            {
                xtraTabControl1.SelectedTabPage = pageBatch;
                isHandWork = false;
            }
            else {
                xtraTabControl1.SelectedTabPage = pageHand;
            }
            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnSearch.Name
                , sysToolBar.BtnDelete.Name
                , sysToolBar.BtnPrint.Name
            , sysToolBar.BtnClose.Name});
            sysToolBar.OnBtnSearchClicked += btnRefresh_Click;
            sysToolBar.OnCloseClicked += btnClosed_Click;
            sysToolBar.OnBtnPrintClicked += btnPrintRackBc_Click;
            sysToolBar.OnBtnDeleteClicked += btnDel_Click;
            if (xtraTabControl1.SelectedTabPage == pageHand)
            {
                sysToolBar.OnBtnPrintClicked -= btnPrintRackBc_Click;
                sysToolBar.OnBtnPrintClicked += btnPrintRackHand_Click;
            }
        }

        private void InitData()
        {
            try
            {
                #region 手工归档

                dateFrom.EditValue = ServerDateTime.GetServerDateTime().AddDays(-1).Date;
                dateEditTo.EditValue = ServerDateTime.GetServerDateTime().Date;
                rpscType.SetValue("0");
                rpscType.RoundPanelGroupClick += RpscType_RoundPanelGroupClick;

                ProxySampStoreRecord proxy = new ProxySampStoreRecord();
                dtRack = proxy.Service.GetDictRackList(Convert.ToDateTime(dateFrom.EditValue), Convert.ToDateTime(dateEditTo.EditValue).AddDays(1).AddSeconds(-1));

                dtRackBatch = proxy.Service.GetDictRackList(ServerDateTime.GetServerDateTime().AddMonths(-6), ServerDateTime.GetServerDateTime());
                FillRackBarcode(dtRackBatch);
                cmbColor.Properties.Items.AddRange(RackColorConsts.GetRackColorList());

                dtRackSpec = proxy.Service.GetCuvShelf();
                dtRackSpec.Insert(0, new EntityDicTubeRack());
                lueCuvShelf.Properties.DataSource = dtRackSpec;
                lueCuvShelfBatch.Properties.DataSource = dtRackSpec;

                List<EntityDicSampStoreStatus> table = proxy.Service.GetSamManageStatus();
                repositoryItemLookUpEdit2.DataSource = table;
                repositoryItemLookUpEdit3.DataSource = table;

                List<EntityDicSampTubeRack> rows = dtRack.Where(i => i.SrStatus == 0 || i.SrStatus == 5).ToList();
                gcRackInfo.DataSource = rows;


                repositoryItemLookUpEdit5.DataSource = repositoryItemLookUpEdit1.DataSource = CacheClient.GetCache<EntityDicSample>();

                //试管架号

                #endregion

                #region 批量归档
                DateJY.EditValue = ServerDateTime.GetServerDateTime();
                rpscTypeBatch.Items = new List<dcl.client.control.RoundPanelStatusItem>
                    {
                        new dcl.client.control.RoundPanelStatusItem { Caption="已归档",Value="2"},
                        new dcl.client.control.RoundPanelStatusItem { Caption="未归档",Value="1"},
                        new dcl.client.control.RoundPanelStatusItem { Caption="全部",Value="0"}
                    };

                rpscTypeBatch.SetValue("0");
                rpscTypeBatch.RoundPanelGroupClick += RpscTypeBatch_RoundPanelGroupClick;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FillRackBarcode(List<EntityDicSampTubeRack> tb)
        {
            cbBatchRackBarcode.Properties.Items.Clear();
            foreach (var row in tb)
            {
                cbBatchRackBarcode.Properties.Items.Add(row.RackBarcode);
            }
        }
        #endregion


        #region 调整左右的比例
        private void FrmSamStoreRecord_Resize(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = Size.Width * 1 / 3;

        }
        #endregion

        #region 手工归档

        #region 扫描条码号，得到响应条码的信息，并加入到以下的详细列表内
        /// <summary>
        /// 扫描条码号，得到响应条码的信息，并加入到以下的详细列表内
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProxySampStoreRecord proxy = new ProxySampStoreRecord();


                if (string.IsNullOrEmpty(strSsid))
                {
                    MessageDialog.Show("试管架不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                //判断试管架的状态，如果是已经审核或者存储的就不能在进行编辑了
                int sstatus = proxy.Service.GetSamRackStatus(strSsid);
                if (sstatus == 10)
                {
                    MessageDialog.Show("试管架已归档审核，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (sstatus == 15)
                {
                    MessageDialog.Show("试管架已归档，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (sstatus == 20)
                {
                    MessageDialog.Show("试管架已销毁，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }

                //判断是否有条码号
                string strBarCode = txtBarCode.Text.Trim();
                if (string.IsNullOrEmpty(strBarCode))
                {
                    return;
                }

                if (dtFristBarCode == null || ServerDateTime.GetServerDateTime().AddMinutes(-3) > dtFristBarCode || string.IsNullOrEmpty(operatorID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCode = ServerDateTime.GetServerDateTime();
                        operatorID = checkPassword.OperatorID;
                        operatorName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }


                //判断此条码信息是否有效
                List<EntityPidReportMain> dtBcPatients = proxy.Service.GetPatientsInfo(strBarCode);
                if (dtBcPatients.Count < 1)
                {
                    MessageDialog.Show("找不到该条码信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtBarCode.Text = "";
                    return;
                }
                ProxySampDetail proxyDetail = new ProxySampDetail();
                #region 判断此条码的所有组合是否全部出结果了
                //判断此条码的所有组合是否全部出结果了
                List<EntitySampDetail> listDetail = proxyDetail.Service.GetSampDetail(strBarCode);
                List<string> listRepId = new List<string>();
                foreach (EntityPidReportMain pid in dtBcPatients)
                {
                    listRepId.Add(pid.RepId);
                }
                EntityResultQC qc = new EntityResultQC();
                qc.ListObrId = listRepId;
                List<EntityObrResult> listResult = new ProxyObrResult().Service.ObrResultQuery(qc);
                List<EntitySampDetail> listDetailTemp = new List<EntitySampDetail>();
                if (listDetail.Count > 0 && listResult.Count > 0)
                {
                    listDetailTemp = listDetail.Where(p => !listResult.Any(w => w.ItmComId == p.ComId)).ToList();
                }
                if (listDetailTemp.Count > 0)
                {
                    string comName = string.Empty;
                    foreach (EntitySampDetail detail in listDetailTemp)
                    {
                        comName += string.Format(",{0}", detail.ComName);
                    }
                    comName = comName.Remove(0, 1);
                    string message = string.Format("此样本还有({0})未出结果，是否强制归档！", comName);
                    DialogResult result = MessageDialog.Show(message, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (result != DialogResult.Yes)
                    {
                        txtBarCode.Text = "";
                        txtBarCode.Focus();
                        return;
                    }
                }
                #endregion
                string patid = dtBcPatients[0].RepId.ToString();
                if (dtBcPatients.Count > 1)
                {
                    List<EntityPidReportMain> pt = dtBcPatients.Where(i => i.RepStatus < 2).ToList();
                    if (pt.Count > 0)
                    {
                        string com = string.Empty;
                        foreach (EntityPidReportMain var in pt)
                        {
                            com += string.Format(",{0}", var.PidComName);
                        }
                        com = com.Remove(0, 1);
                        DialogResult result = MessageDialog.Show(string.Format("此样本还有{0}尚未审核，是否强制归档！", com), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            txtBarCode.Text = "";
                            txtBarCode.Focus();
                            return;
                        }
                    }
                    else
                    {
                        patid = pt[0].RepId.ToString();
                    }
                }
                else
                {
                    object o = dtBcPatients[0].RepStatus;
                    if (o == null || o == DBNull.Value || o.ToString() == "0" || o.ToString() == "1")
                    {
                        DialogResult result = MessageDialog.Show("此样本尚未审核，是否强制归档！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            txtBarCode.Text = "";
                            txtBarCode.Focus();
                            return;
                        }
                    }
                }


                //判断此条码号是否已经被录入          
                List<EntitySampStoreDetail> dtRackDetail = proxy.Service.GetRackDetail(strSsid);

                List<EntitySampStoreDetail> rs = dtRackDetail.Where(i => i.DetSeqno == int.Parse(spSeq.EditValue.ToString()) && i.DetId == strSsid).ToList();

                if (rs.Count > 0)
                {
                    MessageDialog.Show("此顺序号已被占用!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtBarCode.Text = "";
                    return;
                }

                if (proxy.Service.IsBarCodeUsing(strBarCode))
                {
                    MessageDialog.Show("此标本已经归档!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtBarCode.Text = "";
                    return;
                }

                string[] appendBarCodeList = proxy.Service.GetAppendBarCode(strBarCode, patid);
                if (appendBarCodeList.Length > 0)
                {
                    string msg = string.Empty;
                    for (int index = 0; index < appendBarCodeList.Length; index++)
                    {
                        string barcode = appendBarCodeList[index];
                        if (index != appendBarCodeList.Length - 1)
                        {
                            msg += barcode + ",";
                        }
                        else
                        {
                            msg += barcode + " ";
                        }
                    }
                    MessageDialog.Show(msg + "条码已追加到该条码上!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                //向表内加入一条数据
                EntitySampStoreDetail entity = new EntitySampStoreDetail();
                entity.DetId = strSsid;

                entity.DetBarCode = strBarCode;
                entity.DetX = Convert.ToInt32(txtNumX.Text);
                entity.DetY = Convert.ToInt32(txtNumY.Text);
                entity.DetSeqno = intSeq;
                entity.DetStatus = 5;//未审核

                int intRet = proxy.Service.InsertRackDetail(entity);

                if (intRet < 1)
                {
                    MessageDialog.Show("数据归档添加失败！");
                    return;
                }
                string remark = string.Format("归档架子号:{0},孔号:{1}X{2}", strBarCode, txtNumX.Text, txtNumY.Text);
                intRet = proxy.Service.InsertBcSign(operatorName, operatorID, strBarCode, "110", remark, LocalSetting.Current.Setting.Description);

                if (intRet < 1)
                {
                    MessageDialog.Show("标本操作记录添加失败！");
                    return;
                }

                //同时改变试管架的状态
                EntityDicSampTubeRack row = this.gvRackInfo.GetFocusedRow() as EntityDicSampTubeRack;
                row.SrStatus = 5;//未审核
                EntitySampStoreRack entitySamRack = new EntitySampStoreRack();
                entitySamRack.SrId = strSsid;
                entitySamRack.SrStatus = 5;//未审核
                entitySamRack.SrAmount = row.SrAmount == 0 ? 1 : gvSamDetail.RowCount + 1;
                row.SrAmount = entitySamRack.SrAmount;
                row.UseStatus = entitySamRack.SrAmount + "/" + Convert.ToInt32(row.RackXAmount) * Convert.ToInt32(row.RackYAmount);
                entitySamRack.SrRackId = strRackID;
                proxy.Service.ModifySamStoreRack(entitySamRack);

                //修改字典中试管架的状态
                EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                entityRack.RackId = entitySamRack.SrRackId;
                entityRack.RackStatus = entitySamRack.SrStatus;
                proxy.Service.ModifyRackStatus(entityRack);

                //同时向详细表格内加入一行数据
                gcSamDetail.DataSource = proxy.Service.GetRackDetail(strSsid);

                intSeq++;
                spSeq.EditValue = intSeq;
                FocusRowChange(row);
                txtBarCode.Text = "";
                txtBarCode.Focus();
                dtFristBarCode = ServerDateTime.GetServerDateTime();
            }
        }

        #endregion


        #endregion

        #region 扫描架子条码

        private void txtRackID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string rackID = txtRackID.Text;
                if (string.IsNullOrEmpty(rackID))
                {
                    return;
                }

                //试管架号码有问题
                List<EntityDicSampTubeRack> rackDt = gcRackInfo.DataSource as List<EntityDicSampTubeRack>;
                if (rackDt == null) return;
                List<EntityDicSampTubeRack> rows = rackDt.Where(i => i.RackBarcode == rackID).ToList();
                if (rows.Count < 1)
                {
                    MessageDialog.Show("此试管架条码不存在，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtRackID.Text = "";
                    return;
                }

                EntityDicSampTubeRack dr = rows[0];
                int index = rackDt.IndexOf(dr);

                gvRackInfo.FocusedRowHandle = index;
                txtBarCode.Focus();
            }
        }

        #endregion

        #region 物理组
        //private void selectDict_Type1_ValueChanged(object sender, lis.client.control.ValueChangeEventArgs args)
        //{
        //    //ResetGcReckInfo();
        //}
        #endregion

        #region 顺序号对应位置


        private void spSeq_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lueCuvShelf.Text))
            {
                //lis.client.control.MessageDialog.Show("试管架不能为空！请选择试管架的规格！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);               
                return;
            }
            if (!isHandWork) return;
            List<EntityDicTubeRack> rows = dtRackSpec.Where(i => i.RackCode == lueCuvShelf.EditValue.ToString()).ToList();


            int x = Convert.ToInt32(rows[0].RackXAmount);
            int y = Convert.ToInt32(rows[0].RackYAmount);

            intSeq = Convert.ToInt32(spSeq.EditValue);
            if (intSeq > (x * y))
            {
                MessageDialog.Show("请更换试管架！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                intSeq = x * y;
                spSeq.EditValue = x * y;
            }
            if (intSeq < x)
            {
                txtNumX.Text = (intSeq / x + 1).ToString();
            }
            else if (intSeq == x)
            {
                txtNumX.Text = (intSeq / x).ToString();
            }
            else if (intSeq % x == 0)
            {
                txtNumX.Text = (intSeq / x).ToString();
            }
            else
            {
                txtNumX.Text = (intSeq / x + 1).ToString();
            }

            txtNumY.Text = intSeq % x == 0 ? x.ToString() : (intSeq % x).ToString();
        }
        #endregion

        #region 对应左边的试管中详细样本信息
        private void gvSam_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicSampTubeRack dr = this.gvRackInfo.GetFocusedRow() as EntityDicSampTubeRack;

            ProxySampStoreRecord proxy = new ProxySampStoreRecord();

            if (dr != null)
            {
                strSsid = dr.SrId.ToString();
                lueCuvShelf.EditValue = dr.CusCode.ToString();
                strRackID = dr.RackId.ToString();
                intSsStauts = Convert.ToInt32(dr.SrStatus);
                cmbColor.EditValue = dr.RackColour;
                txtRackID.Text = dr.RackBarcode.ToString();
                //根据ss_id来得到bar_code
                List<EntitySampStoreDetail> dtNow = proxy.Service.GetRackDetail(strSsid);
                if (dtNow != null && dtNow.Count > 0)
                {
                    object maxSeq = dtNow.OrderByDescending(i => i.DetSeqno).FirstOrDefault().DetSeqno;
                    if (maxSeq == DBNull.Value)
                    {
                        spSeq.EditValue = 1;
                    }
                    else
                    {
                        spSeq.EditValue = Convert.ToInt32(maxSeq) + 1;
                    }

                    gcSamDetail.DataSource = dtNow;
                }
                else
                {
                    gcSamDetail.DataSource = null;
                    intSsStauts = -1;
                    spSeq.EditValue = 1;
                }

            }
            else
            {
                gcSamDetail.DataSource = null;
                strSsid = string.Empty;
                strRackID = string.Empty;
                intSsStauts = -1;
            }
            FocusRowChange(dr);
            txtBarCode.Focus();
        }

        private void FocusRowChange(EntityDicSampTubeRack innerRow)
        {
            gridViewRack.Columns.Clear();

            if (innerRow == null)
            {
                return;
            }
            int xNum;
            int yNum;
            if (int.TryParse(innerRow.RackXAmount.ToString(), out xNum) &&
                int.TryParse(innerRow.RackYAmount.ToString(), out yNum))
            {
                BindGridRackUseStatusView(xNum, yNum, innerRow.SrId.ToString());
            }
        }

        private void FocusRowChangeBatch(EntityDicSampTubeRack innerRow)
        {
            gridViewBatchInfo.Columns.Clear();

            if (innerRow == null)
            {
                return;
            }
            int xNum;
            int yNum;
            if (int.TryParse(innerRow.RackXAmount.ToString(), out xNum) &&
                int.TryParse(innerRow.RackYAmount.ToString(), out yNum))
            {
                BindGridRackUseStatusViewForBatch(xNum, yNum, innerRow.SrId.ToString());
            }
        }

        #endregion

        #region 架子使用情况

        private void BindGridRackUseStatusView(int xNum, int yNum, string ssId)
        {

            panelControlRackUseStatus.Height = (yNum + 1) * 22 + 5;

            DataTable table = new DataTable();
            for (int i = 1; i <= xNum; i++)
            {
                table.Columns.Add(i.ToString());
            }
            for (int j = 1; j <= yNum; j++)
            {
                table.Rows.Add(table.NewRow());
            }
            ProxySampStoreRecord proxy = new ProxySampStoreRecord();
            List<EntitySampStoreDetail> dtRackDt = proxy.Service.GetRackDetail(ssId);
            foreach (EntitySampStoreDetail row in dtRackDt)
            {
                int x = int.Parse(row.DetX.ToString());
                int y = int.Parse(row.DetY.ToString());
                if (table.Rows.Count - 1 < x - 1 || table.Columns.Count - 1 < y - 1)
                    continue;
                if (row.PatFlagStatus.ToString() == "未审核")
                {
                    table.Rows[x - 1][y - 1] = 1;
                }
                else
                {
                    table.Rows[x - 1][y - 1] = 2;
                }
            }
            gridControlRack.DataSource = table;
        }

        private void BindGridRackUseStatusViewForBatch(int xNum, int yNum, string ssId)
        {
            DataTable table = new DataTable();
            for (int i = 1; i <= xNum; i++)
            {
                table.Columns.Add(i.ToString());
            }
            for (int j = 1; j <= yNum; j++)
            {
                table.Rows.Add(table.NewRow());
            }
            ProxySampStoreRecord proxy = new ProxySampStoreRecord();
            List<EntitySampStoreDetail> dtRackDt = proxy.Service.GetRackDetail(ssId);
            foreach (EntitySampStoreDetail row in dtRackDt)
            {
                int x = int.Parse(row.DetX.ToString());
                int y = int.Parse(row.DetY.ToString());
                if (table.Rows.Count - 1 < x - 1 || table.Columns.Count - 1 < y - 1)
                    continue;
                if (row.PatFlagStatus.ToString() == "未审核")
                {
                    table.Rows[x - 1][y - 1] = 1;
                }
                else
                {
                    table.Rows[x - 1][y - 1] = 2;
                }

            }

            gridUseinfo.DataSource = table;
            try
            {
                bool isFind = false;
                for (int i = 0; i <= table.Rows.Count; i++)
                {
                    if (isFind) break;
                    for (int j = 1; j <= xNum; j++)
                    {
                        if (table.Rows[i][j - 1] == null || table.Rows[i][j - 1] == DBNull.Value)
                        {
                            gridViewBatchInfo.FocusedColumn = gridViewBatchInfo.Columns[j - 1];
                            gridViewBatchInfo.FocusedRowHandle = i;
                            isFind = true;
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void gridViewRack_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowIndex = e.RowHandle;
            if (rowIndex >= 0)
            {
                rowIndex++;
                e.Info.DisplayText = rowIndex.ToString();
            }
        }

        #region 刷新
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (isHandWork)
            {
                RefreshForm();
            }
            else
            {
                RefreshFormForBatch();
            }
        }

        private void RefreshFormForBatch()
        {
            ProxySampStoreRecord proxy = new ProxySampStoreRecord();

            dtRackBatch = proxy.Service.GetDictRackList(ServerDateTime.GetServerDateTime().AddMonths(-12), ServerDateTime.GetServerDateTime());

            string txtbarcode = cbBatchRackBarcode.Text;
            FillRackBarcode(dtRackBatch);
            cbBatchRackBarcode.Text = txtbarcode;
        }

        private void RefreshForm()
        {
            string strSsid = this.strSsid;
            ProxySampStoreRecord proxy = new ProxySampStoreRecord();
            dtRack = proxy.Service.GetDictRackList(Convert.ToDateTime(this.dateFrom.EditValue), Convert.ToDateTime(this.dateEditTo.EditValue).AddDays(1).AddSeconds(-1));
            if (isHandWork)
            {
                FilterChanged();
            }
            else
            {
                FilterBatchChanged();
            }
        }

        #endregion

        #region 关闭

        private void btnClosed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion


        #region 手工录入和批量录入转换
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //lueCType.valueMember = "";
            //lueCuvShelf.EditValue = "";
            if (e.Page.Name.Equals("pageBatch"))
            {
                FilterBatchChanged();
                isHandWork = false;
                sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnSearch.Name
                ,sysToolBar.BtnSave.Name
                , sysToolBar.BtnDelete.Name
                , sysToolBar.BtnPrint.Name
                , sysToolBar.BtnClose.Name});
                this.sysToolBar.BtnSave.Caption = "开始归档";
                this.sysToolBar.BtnSave.Enabled = true;
                sysToolBar.OnBtnSaveClicked -= btnFiled_Click;
                sysToolBar.OnBtnSaveClicked += btnFiled_Click;
                sysToolBar.OnBtnSearchClicked -= btnRefresh_Click;
                sysToolBar.OnBtnSearchClicked += btnSearchSam_Click;
                sysToolBar.OnBtnDeleteClicked -= btnDel_Click;
                sysToolBar.OnBtnDeleteClicked += btnDeleteBatch_Click;
                sysToolBar.OnBtnPrintClicked -= btnPrintRackHand_Click;
                sysToolBar.OnBtnPrintClicked += btnPrintRackBc_Click;
            }
            if (e.Page.Name.Equals("pageHand"))
            {
                FilterChanged();
                isHandWork = true;
                sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnSearch.Name
                , sysToolBar.BtnDelete.Name,sysToolBar.BtnPrint.Name
                , sysToolBar.BtnClose.Name});
                sysToolBar.BtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                sysToolBar.OnBtnSearchClicked -= btnSearchSam_Click;
                sysToolBar.OnBtnSearchClicked += btnRefresh_Click;
                sysToolBar.OnBtnDeleteClicked -= btnDeleteBatch_Click;
                sysToolBar.OnBtnDeleteClicked += btnDel_Click;
                sysToolBar.OnBtnPrintClicked -= btnPrintRackBc_Click;
                sysToolBar.OnBtnPrintClicked += btnPrintRackHand_Click;

            }
        }
        #endregion


        #region 批量归档

        #region 标本检索

        private void btnSearchSam_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(DateJY.Text))
                {
                    MessageDialog.Show("请选择检验样本的时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    DateJY.EditValue = ServerDateTime.GetServerDateTime();
                    return;
                }


                if (string.IsNullOrEmpty(lueBatchItr.valueMember))
                {
                    MessageDialog.Show("请选择检验仪器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);

                    return;
                }

                ProxySampStoreRecord proxy = new ProxySampStoreRecord();

                dtPatiens = proxy.Service.GetBatchHandData(DateJY.DateTime.Date, lueBatchItr.valueMember, txtSamFrom.Text, txtSamTo.Text, radType.SelectedIndex);

                FilterPatients();

                if (dtPatiens.Count == 0)
                {
                    MessageDialog.ShowAutoCloseDialog("找不到符合当前查询条件的数据");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FilterPatients()
        {
            if (dtPatiens == null) return;
            string filter = string.Empty;
            List<EntitySampStoreDetail> patientsInfo = new List<EntitySampStoreDetail>();
            if (rpscTypeBatch.GetCurRoundPanel().Tag?.ToString() == "0")
            {
                patientsInfo = dtPatiens;
            }
            if (rpscTypeBatch.GetCurRoundPanel().Tag?.ToString() == "1")
            {
                patientsInfo = dtPatiens.Where(i => i.DetBarCode == "" || i.DetBarCode == null).ToList();
            }

            if (rpscTypeBatch.GetCurRoundPanel().Tag?.ToString() == "2")
            {
                patientsInfo = dtPatiens.Where(i => i.DetBarCode != "" && i.DetBarCode != null).ToList();
            }
            foreach (var item in patientsInfo)
            {
                if (item.DetStatus == 0)
                {
                    item.DetDate = null;
                    item._DetNumXY = string.Empty;
                    item.DetSeqno = null;
                }
            }
            gcRackDetail.DataSource = patientsInfo;
        }

        #endregion

        #region 顺序号还是样本号
        private void radType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radType.SelectedIndex == 0)
            {
                labStart.Text = "样本号：";
                labStart.ForeColor = Color.RoyalBlue;
            }
            if (radType.SelectedIndex == 1)
            {
                labStart.Text = "顺序号：";
                labStart.ForeColor = Color.Red;
            }
        }


        #region 删除----删除这条记录
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gvSamDetail.RowCount < 1)
            {
                return;
            }
            DialogResult result = MessageDialog.Show("确定要从当前列表删除这些样本？\r\n 注意：只能删除未审核的标本", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
            {
                return;
            }

            List<string> barCodeList = new List<string>();
            EntitySampStoreDetail row = this.gvSamDetail.GetFocusedRow() as EntitySampStoreDetail;

            if (row.DetStatus.ToString() == "5")
            {
                barCodeList.Add(row.DetBarCode.ToString());
            }

            if (barCodeList.Count == 0)
            {

                MessageDialog.Show("请选择未审核的标本进行删除！已归档审核标本只能销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            ProxySampStoreRecord proxy = new ProxySampStoreRecord();
            proxy.Service.DeleteSamDetail(strSsid, strRackID, barCodeList);
            RefreshForm();
        }
        #endregion


        private void btnFiled_Click(object sender, EventArgs e)
        {
            try
            {
                ProxySampStoreRecord proxy = new ProxySampStoreRecord();



                if (string.IsNullOrEmpty(lueCuvShelfBatch.Text))
                {
                    MessageDialog.Show("架子类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    lueCuvShelfBatch.Focus();
                    return;
                }
                gridView3.CloseEditor();
                List<EntitySampStoreDetail> dtDetail = gcRackDetail.DataSource as List<EntitySampStoreDetail>;
                if (dtDetail == null || dtDetail.Count == 0) return;
                List<EntitySampStoreDetail> rows = dtDetail.Where(i => i.Checked == true).ToList();
                if (rows.Count == 0)
                {
                    MessageDialog.Show("请勾选要归档的标本信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                List<EntitySampStoreDetail> tableAllSelect = new List<EntitySampStoreDetail>();
                foreach (EntitySampStoreDetail dataRow in rows)
                {
                    if (dataRow.DetBarCode != null && dataRow.DetBarCode != "" && !string.IsNullOrEmpty(dataRow.DetBarCode.ToString()))
                    {
                        continue;
                    }

                    if (dataRow.PatFlagStatus != null && dataRow.PatFlagStatus != ""
                        && !string.IsNullOrEmpty(dataRow.PatFlagStatus.ToString())
                        && Convert.ToInt32(dataRow.RepStatus) < 2)
                    {
                        sb.Append(string.Format("{0}  ", dataRow.RepBarCode));
                    }
                    tableAllSelect.Add(dataRow);
                }

                if (sb.Length > 0)
                {
                    DialogResult result = MessageDialog.Show("标本条码号为：" + sb + "\r\n样本尚未审核，是否强制归档！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (result != DialogResult.Yes)
                    {
                        gcRackDetail.RefreshDataSource();
                        return;
                    }
                }

                string typeid = string.Empty;
                string typename = string.Empty;
                string rackcode = string.Empty;
                if (dtFristBarCodeForBatch == null || ServerDateTime.GetServerDateTime().AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(operatorIDForBatch))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = ServerDateTime.GetServerDateTime();
                        operatorIDForBatch = checkPassword.OperatorID;
                        operatorNameForBatch = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                //if (string.IsNullOrEmpty(cbBatchRackBarcode.Text)||!proxy.Service.IsRaclBarCodeExists(cbBatchRackBarcode.Text))
                //{
                if (!string.IsNullOrEmpty(cbBatchRackBarcode.Text))
                {
                    EntityDicSampTubeRack rowBc = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (rowBc != null)
                    {
                        rackcode = rowBc.RackId.ToString();
                        typename = rowBc.ProName.ToString();
                        typeid = rowBc.RackType.ToString();
                    }
                }
                FrmPrintTempBarCode code = new FrmPrintTempBarCode(cbBatchRackBarcode.Text, rackcode, lueCuvShelfBatch.EditValue.ToString(),
                    string.IsNullOrEmpty(typename) ? luBatchCtype.displayMember : typename, string.IsNullOrEmpty(typeid) ? luBatchCtype.valueMember : typeid
                    , operatorNameForBatch);
                code.PrintBarCode(cbBatchRackBarcode.Text);
                cbBatchRackBarcode.Text = code.RackBarCode;
                RefreshFormForBatch();
                //}

                EntityDicSampTubeRack row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                if (row == null)
                {
                    MessageDialog.Show("当前架子号未生成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                string ssid = row.SrId.ToString();
                string rackid = row.RackId.ToString();
                //判断试管架的状态，如果是已经审核或者存储的就不能在进行编辑了
                int sstatus = proxy.Service.GetSamRackStatus(ssid);
                if (sstatus == 10)
                {
                    MessageDialog.Show("试管架已归档审核，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (sstatus == 15)
                {
                    MessageDialog.Show("试管架已归档，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (sstatus == 20)
                {
                    MessageDialog.Show("试管架已销毁，不能再使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }



                int rowhander = 0;
                int colHander = 0;
                if (gridViewBatchInfo.RowCount > 0 && gridViewBatchInfo.FocusedColumn != null)
                {
                    rowhander = gridViewBatchInfo.FocusedRowHandle;
                    colHander = Convert.ToInt32(gridViewBatchInfo.FocusedColumn.FieldName);
                }
                string msg = proxy.Service.BatchHandData(tableAllSelect, ssid, rackid, lueCuvShelfBatch.EditValue.ToString(),
                                            operatorNameForBatch, operatorIDForBatch, LocalSetting.Current.Setting.Description, rowhander, colHander, cbBatchRackBarcode.Text);
                int sec = 4;
                dtFristBarCodeForBatch = ServerDateTime.GetServerDateTime();

                if (string.IsNullOrEmpty(msg))
                {
                    sec = 1;
                    msg = "操作成功";
                }
                btnSearchSam_Click(null, null);
                FocusRowChangeBatch(row);
                MessageDialog.ShowAutoCloseDialog(msg, sec);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        #endregion






        #endregion

        #region 过滤条件radioGroupConditionChanged

        private void RpscType_RoundPanelGroupClick(object sender, EventArgs e)
        {
            FilterChanged();
        }


        private void FilterChanged()
        {
            string filter = string.Empty;
            List<EntityDicSampTubeRack> dtRackInfo = new List<EntityDicSampTubeRack>();
            if (rpscType.GetCurRoundPanel().Tag?.ToString() == "0")
            {
                if (string.IsNullOrEmpty(luCtype.valueMember))
                {
                    gcRackInfo.DataSource = dtRack;
                    gvSam_FocusedRowChanged(null, null);
                    return;
                }
                string RackType = luCtype.valueMember.ToString();
                dtRackInfo = dtRack.Where(i => i.RackType == RackType).ToList();
                //filter = string.Format("rack_ctype = '{0}' ", luCtype.valueMember);
            }
            if (rpscType.GetCurRoundPanel().Tag?.ToString() == "1")
            {
                //filter = string.IsNullOrEmpty(luCtype.valueMember) ? string.Format("ss_status = {0} or ss_status = {1} ", 0, 5)
                //   : string.Format("(ss_status = {0} or ss_status = {1}) and rack_ctype = '{2}'  ", 0, 5, luCtype.valueMember);
                if (!string.IsNullOrEmpty(luCtype.valueMember))
                {
                    string RackType = luCtype.valueMember.ToString();
                    dtRackInfo = dtRack.Where(i => i.RackType == RackType && (i.SrStatus == 0 || i.SrStatus == 5)).ToList();
                }
                else
                {
                    dtRackInfo = dtRack.Where(i => i.SrStatus == 0 || i.SrStatus == 5).ToList();
                }
            }

            if (rpscType.GetCurRoundPanel().Tag?.ToString() == "2")
            {
                //filter = string.IsNullOrEmpty(luCtype.valueMember) ? string.Format("ss_status = {0} or ss_status = {1} ", 10, 15)
                //   : string.Format("(ss_status = {0} or ss_status = {1}) and rack_ctype = '{2}'  ", 10, 15, luCtype.valueMember);
                if (!string.IsNullOrEmpty(luCtype.valueMember))
                {
                    string RackType = luCtype.valueMember.ToString();
                    dtRackInfo = dtRack.Where(i => i.RackType == RackType && (i.SrStatus == 10 || i.SrStatus == 15)).ToList();
                }
                else
                {
                    dtRackInfo = dtRack.Where(i => i.SrStatus == 10 || i.SrStatus == 15).ToList();
                }
            }
            this.gcRackInfo.DataSource = dtRackInfo;
            this.bsRackInfo.DataSource = dtRackInfo;
            gcRackInfo.RefreshDataSource();
            gvSam_FocusedRowChanged(null, null);
        }

        private void FilterBatchChanged()
        {
            string filter = string.Empty;

            List<EntityDicSampTubeRack> dtRackBatchInfo = new List<EntityDicSampTubeRack>();
            if (string.IsNullOrEmpty(luBatchCtype.valueMember) && string.IsNullOrEmpty(lueCuvShelfBatch.Text))
            {
                FillRackBarcode(dtRackBatch);
                return;
            }

            if (!string.IsNullOrEmpty(luBatchCtype.valueMember))
            {
                string RackType = luBatchCtype.valueMember.ToString();
                dtRackBatchInfo = dtRackBatch.Where(i => i.RackType == "-1" || i.RackType == RackType).ToList();
            }


            if (!string.IsNullOrEmpty(lueCuvShelfBatch.Text))
            {
                string RackSpec = lueCuvShelfBatch.EditValue.ToString();
                dtRackBatchInfo = dtRackBatch.Where(i => i.RackSpec == RackSpec).ToList();
            }
            //List<EntityDicSampTubeRack> rows = dtRackBatch;
            //List<EntityDicSampTubeRack> newTable = dtRackBatch;
            //foreach (var row in rows)
            //{
            //    newTable.Add(row);
            //}
            FillRackBarcode(dtRackBatchInfo);
        }

        #endregion


        private bool allowFirePatTypeValueChanged = true;
        private void RpscTypeBatch_RoundPanelGroupClick(object sender, EventArgs e)
        {
            FilterBatchChanged();
            FilterPatients();
        }

        private void btnDeleteBatch_Click(object sender, EventArgs e)
        {
            if (gridView3.RowCount < 1)
            {
                return;
            }
            gridView3.CloseEditor();
            List<EntitySampStoreDetail> dtDetail = gcRackDetail.DataSource as List<EntitySampStoreDetail>;
            if (dtDetail == null || dtDetail.Count == 0) return;
            List<EntitySampStoreDetail> rows = dtDetail.Where(I => I.Checked == true).ToList();
            if (rows.Count == 0)
            {
                MessageDialog.Show("请勾选要删除的标本信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1);
                return;
            }
            DialogResult result = MessageDialog.Show("确定要从当前列表删除这些样本？\r\n 注意：只能删除未审核的标本", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
            {
                return;
            }

            List<string> barCodeList = new List<string>();
            string ssid = string.Empty;
            string rackID = string.Empty;
            foreach (EntitySampStoreDetail row in rows)
            {
                if (row.DetStatus.ToString() == "5")
                {
                    ssid = row.DetId.ToString();
                    rackID = row.SrRackId.ToString();
                    barCodeList.Add(row.DetBarCode.ToString());
                }
            }

            if (barCodeList.Count == 0)
            {

                MessageDialog.Show("请选择已归档的标本进行删除！已审核标本只能销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            ProxySampStoreRecord proxy = new ProxySampStoreRecord();
            proxy.Service.DeleteSamDetail(ssid, rackID, barCodeList);

            RefrshBatchDetail();

        }

        EntityDicSampTubeRack GetSsidByBarCode(string barcode)
        {
            if (dtRackBatch != null)
            {
                List<EntityDicSampTubeRack> rows = dtRackBatch.Where(i => i.RackBarcode == barcode).ToList();
                if (rows.Count > 0 && rows[0].SrId != null)
                {
                    return rows[0];
                }
            }
            return null;
        }
        /// <summary>
        /// 批量归档打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintRackBc_Click(object sender, EventArgs e)
        {
            try
            {
                string typeid = string.Empty;
                string typename = string.Empty;
                string rackcode = string.Empty;
                if (string.IsNullOrEmpty(lueCuvShelfBatch.Text))
                {
                    MessageDialog.Show("架子类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    lueCuvShelfBatch.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(cbBatchRackBarcode.Text))
                {
                    EntityDicSampTubeRack row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (row != null)
                    {
                        rackcode = row.RackId.ToString();
                        typename = row.ProName.ToString();
                        typeid = row.RackType.ToString();
                    }
                }
                if (dtFristBarCodeForBatch == null || ServerDateTime.GetServerDateTime().AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(operatorIDForBatch))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = ServerDateTime.GetServerDateTime();
                        operatorIDForBatch = checkPassword.OperatorID;
                        operatorNameForBatch = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                FrmPrintTempBarCode code = new FrmPrintTempBarCode(cbBatchRackBarcode.Text, rackcode, lueCuvShelfBatch.EditValue.ToString(),
                    string.IsNullOrEmpty(typename) ? luBatchCtype.displayMember : typename, string.IsNullOrEmpty(typeid) ? luBatchCtype.valueMember : typeid, operatorNameForBatch);
                code.ShowDialog();

                if (code.DialogResult == DialogResult.OK)
                {

                    RefreshFormForBatch();
                    cbBatchRackBarcode.Text = code.RackBarCode;


                    ProxySampStoreRecord proxy = new ProxySampStoreRecord();
                    EntityDicSampTubeRack row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (row == null)
                    {
                        gcRackDetail.DataSource = null;
                    }
                    else
                    {
                        List<EntitySampStoreDetail> dtNow = proxy.Service.GetRackDetail(row.SrId.ToString());
                        if (dtNow.Count > 0)
                            gcRackDetail.DataSource = dtNow;
                    }
                    FocusRowChangeBatch(row);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///手工归档打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintRackHand_Click(object sender, EventArgs e)
        {
            try
            {
                string typeid = string.Empty;
                string typename = string.Empty;
                string rackcode = string.Empty;
                if (string.IsNullOrEmpty(lueCuvShelf.Text))
                {
                    MessageDialog.Show("架子类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    lueCuvShelfBatch.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(txtRackID.Text))
                {
                    EntityDicSampTubeRack row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (row != null)
                    {
                        rackcode = row.RackId.ToString();
                        typename = row.ProName.ToString();
                        typeid = row.RackType.ToString();
                    }
                }

                if (dtFristBarCodeForHand == null || ServerDateTime.GetServerDateTime().AddMinutes(-3) > dtFristBarCodeForHand || string.IsNullOrEmpty(operatorIDForHand))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForHand = ServerDateTime.GetServerDateTime();
                        operatorIDForHand = checkPassword.OperatorID;
                        operatorNameForHand = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                FrmPrintTempBarCode code = new FrmPrintTempBarCode(txtRackID.Text, rackcode, lueCuvShelf.EditValue.ToString(),
                    string.IsNullOrEmpty(typename) ? luCtype.displayMember : typename, string.IsNullOrEmpty(typeid) ? luCtype.valueMember : typeid, operatorNameForHand);
                code.ShowDialog();

                if (code.DialogResult == DialogResult.OK)
                {
                    RefreshForm();
                    ProxySampStoreRecord proxy = new ProxySampStoreRecord();
                    EntityDicSampTubeRack row = GetSsidByBarCode(txtRackID.Text);
                    if (row == null)
                    {
                        gcSamDetail.DataSource = null;
                    }
                    else
                    {
                        List<EntitySampStoreDetail> dtNow = proxy.Service.GetRackDetail(row.SrId.ToString());
                        if (dtNow.Count > 0)
                            gcSamDetail.DataSource = dtNow;
                    }
                    FocusRowChange(row);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void lueCuvShelfBatch_EditValueChanged(object sender, EventArgs e)
        {
            cbBatchRackBarcode.Text = string.Empty;
            FilterBatchChanged();
            int xNum = 0;
            int yNum = 0;
            if (lueCuvShelfBatch.EditValue != null && !string.IsNullOrEmpty(lueCuvShelfBatch.EditValue.ToString()) && dtRackSpec != null)
            {
                List<EntityDicTubeRack> rows =
                    dtRackSpec.Where(i => i.RackCode == lueCuvShelfBatch.EditValue.ToString()).ToList();

                if (rows.Count > 0)
                {
                    xNum = Convert.ToInt32(rows[0].RackXAmount);
                    yNum = Convert.ToInt32(rows[0].RackYAmount);
                }
            }
            BindGridRackUseStatusViewForBatch(xNum, yNum, "nonebarcode");
        }

        private void lueBatchItr_onBeforeFilter()
        {
            //当前选中的物理组ID
            string currentSelectType = luBatchCtype.valueMember;
            List<EntityDicInstrument> ItrList = new List<EntityDicInstrument>();
            ItrList = lueBatchItr.getDataSource();
            //是否有物理组
            if (currentSelectType != null && currentSelectType.Trim(null) != string.Empty)
            {
                ItrList = ItrList.Where(i => i.ItrLabId == currentSelectType).ToList();
            }


            if (!UserInfo.isAdmin)
            {
                //非管理员：列出有权限的仪器
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    ItrList = ItrList.Where(i => i.ItrLabId.Contains(UserInfo.sqlUserTypesFilter) && i.ItrId.Contains(UserInfo.sqlUserItrs)).ToList();
                }
            }
            lueBatchItr.dtSource = ItrList;
        }

        private void cbBatchRackBarcode_SelectedValueChanged(object sender, EventArgs e)
        {
            RefrshBatchDetail();
        }

        private void RefrshBatchDetail()
        {
            if (!string.IsNullOrEmpty(cbBatchRackBarcode.Text))
            {
                ProxySampStoreRecord proxy = new ProxySampStoreRecord();
                EntityDicSampTubeRack row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                if (row == null)
                {
                    gcRackDetail.DataSource = null;
                }
                else
                {
                    if (row.RackSpec != null && row.RackSpec != "")
                    {
                        lueCuvShelfBatch.EditValue = row.RackSpec.ToString();
                    }
                    List<EntitySampStoreDetail> dtNow = proxy.Service.GetRackDetail(row.SrId.ToString());
                    gcRackDetail.DataSource = dtNow;
                }
                FocusRowChangeBatch(row);
            }
        }

        private void checkAllDetail_CheckedChanged(object sender, EventArgs e)
        {
            gridView3.CloseEditor();
            List<EntitySampStoreDetail> dtDetail = gcRackDetail.DataSource as List<EntitySampStoreDetail>;
            if (dtDetail == null || dtDetail.Count == 0) return;

            foreach (EntitySampStoreDetail row in dtDetail)
            {
                row.Checked = checkAllDetail.Checked ? true : false;
            }
            gcRackDetail.RefreshDataSource();
        }


        private void gridViewBatchInfo_Click(object sender, EventArgs e)
        {
            gridViewBatchInfo.OptionsSelection.EnableAppearanceFocusedCell = true;

            object cellValue = gridViewBatchInfo.GetFocusedRowCellValue(gridViewBatchInfo.FocusedColumn);
            if (cellValue != null && (cellValue.ToString() == "2" || cellValue.ToString() == "1"))
            {
                MessageDialog.ShowAutoCloseDialog("该位置已有标本", 1);
                int rowhander = gridViewBatchInfo.FocusedRowHandle;
                int colHander = Convert.ToInt32(gridViewBatchInfo.FocusedColumn.FieldName);
                if (colHander == gridViewBatchInfo.Columns.Count)
                {
                    colHander = 0;
                    gridViewBatchInfo.FocusedRowHandle = gridViewBatchInfo.FocusedRowHandle + 1;
                }
                bool isFound = false;
                for (int i = colHander; i < gridViewBatchInfo.Columns.Count; i++)
                {
                    cellValue = gridViewBatchInfo.GetRowCellValue(gridViewBatchInfo.FocusedRowHandle, gridViewBatchInfo.Columns[i]);

                    if (cellValue != null && (cellValue.ToString() == "2" || cellValue.ToString() == "1"))
                    {
                        continue;
                    }
                    gridViewBatchInfo.FocusedColumn = gridViewBatchInfo.Columns[i];
                    isFound = true;
                    break;
                }

                if (!isFound)
                {
                    for (int j = 0; j < gridViewBatchInfo.RowCount; j++)
                    {
                        if (isFound)
                            break;
                        for (int i = 0; i < gridViewBatchInfo.Columns.Count; i++)
                        {
                            cellValue = gridViewBatchInfo.GetRowCellValue(j, gridViewBatchInfo.Columns[i]);

                            if (cellValue != null && (cellValue.ToString() == "2" || cellValue.ToString() == "1"))
                            {
                                continue;
                            }
                            gridViewBatchInfo.FocusedRowHandle = j;
                            gridViewBatchInfo.FocusedColumn = gridViewBatchInfo.Columns[i];
                            isFound = true;
                            break;
                        }
                    }
                }


            }
        }


        public List<EntityDicPubProfession> dtDictType
        {
            get
            {
                List<EntityDicPubProfession> ds = CacheClient.GetCache<EntityDicPubProfession>();
                return ds;
            }
        }

        public EntityDicPubProfession GetCType(string type_id)
        {
            List<EntityDicPubProfession> drs = dtDictType.Where(i => i.ProId == type_id).ToList();

            if (drs.Count > 0)
            {
                return drs[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取仪器所属物理组ID
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrCTypeID(string itr_id)
        {
            string ctype_id = string.Empty;

            EntityDicInstrument dr = GetItr(itr_id);
            if (dr != null)
            {
                ctype_id = dr.ItrLabId.ToString();
            }

            return ctype_id;
        }
        public EntityDicInstrument GetItr(string itr_id)
        {
            if (itr_id == null) itr_id = string.Empty;
            List<EntityDicInstrument> drsItr = this.DictItr.Where(i => i.ItrId == itr_id).ToList();
            if (drsItr.Count > 0)
            {
                return drsItr[0];
            }
            else
            {
                return null;
            }
        }
        public List<EntityDicInstrument> DictItr
        {
            get
            {
                List<EntityDicInstrument> ds = CacheClient.GetCache<EntityDicInstrument>();
                return ds;
            }
        }

        private void cbBatchRackBarcode_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbBatchRackBarcode.Text) && dtRackSpec != null)
            {
                FilterBatchChanged();
                int xNum = 0;
                int yNum = 0;
                if (lueCuvShelfBatch.EditValue != null && !string.IsNullOrEmpty(lueCuvShelfBatch.EditValue.ToString()))
                {

                    List<EntityDicTubeRack> rows = dtRackSpec.Where(i => i.RackCode == lueCuvShelfBatch.EditValue.ToString()).ToList();

                    if (rows.Count > 0)
                    {
                        xNum = Convert.ToInt32(rows[0].RackXAmount);
                        yNum = Convert.ToInt32(rows[0].RackYAmount);
                    }
                }
                BindGridRackUseStatusViewForBatch(xNum, yNum, "nonebarcode");
            }
        }

        private void luCtype_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            FilterChanged();
        }

        private void lueBatchItr_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //选择仪器后如果物理组为空则填充当前仪器的物理组
            if (string.IsNullOrEmpty(luBatchCtype.valueMember))
            {
                string ctype_id = GetItrCTypeID(this.lueBatchItr.valueMember);

                if (!string.IsNullOrEmpty(ctype_id))
                {
                    EntityDicPubProfession rowCType = GetCType(ctype_id);

                    if (rowCType != null)
                    {
                        allowFirePatTypeValueChanged = false;
                        this.luBatchCtype.valueMember = ctype_id;
                        this.luBatchCtype.displayMember = rowCType.ProName.ToString();
                        allowFirePatTypeValueChanged = true;
                    }
                }
            }
        }

        private void luBatchCtype_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (!allowFirePatTypeValueChanged) return;
            lueBatchItr.ClearSelect();
            FilterBatchChanged();
            lueBatchItr_onBeforeFilter();
        }

    }

    public class RackColorConsts
    {
        internal static List<string> GetRackColorList()
        {
            return new List<string> { "红色", "紫色", "蓝色", "IT3000专用", "白色", "黑色", "其它", };
        }
    }
}
