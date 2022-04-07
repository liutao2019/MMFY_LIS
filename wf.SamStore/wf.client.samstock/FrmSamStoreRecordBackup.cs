using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.pub.entities.dict;
using dcl.client.common;
using lis.client.control;

namespace dcl.client.samstock
{
    public partial class FrmSamStoreRecordBackup : FrmCommon
    {

        #region 全局变量
        private DataTable dtRackSpec;
        /// <summary>
        /// 试管架内容
        /// </summary>
        private DataTable dtRack;

        /// <summary>
        /// 试管架内容
        /// </summary>
        private DataTable dtRackBatch;

        /// <summary>
        /// 试管架内容
        /// </summary>
        private DataTable dtPatiens=null;

        /// <summary>
        /// 样本详细信息表
        /// </summary>
        private DataTable dtRackPat;
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
        #endregion


        #region 初始化数据
        public FrmSamStoreRecordBackup()
        {
            InitializeComponent();
        }

        private void FrmSamStoreRecord_Load(object sender, EventArgs e)
        {
            InitData();
            txtRackID.Focus();
        }

        private void InitData()
        {

            try
            {
                #region 手工归档

                dateFrom.EditValue = DateTime.Now.AddDays(-1).Date;
                dateEditTo.EditValue = DateTime.Now.Date;

                dateBatchFrom.EditValue = DateTime.Now.AddDays(-1).Date;
                dateEditBatchTo.EditValue = DateTime.Now.Date;

                ProxySamManage proxy = new ProxySamManage();
                 dtRack = proxy.Service.GetDictRackList(Convert.ToDateTime(dateFrom.EditValue), Convert.ToDateTime(dateEditTo.EditValue));
                dtRack.PrimaryKey = new[]{ dtRack.Columns["rack_id"]};

                dtRackBatch = proxy.Service.GetDictRackList(Convert.ToDateTime(dateBatchFrom.EditValue), Convert.ToDateTime(dateEditBatchTo.EditValue));
                FillRackBarcode(dtRackBatch);
                cmbColor.Properties.Items.AddRange(RackColorConsts.GetRackColorList());
                //lueCuvNo.Properties.DataSource = dtRack;
                //lueCuvNo.Properties.DisplayMember = "rack_name";
                //lueCuvNo.Properties.ValueMember = "rack_id";
                //lueCuvNo.Properties.Columns.AddRange(new[] {
                //new DevExpress.XtraEditors.Controls.LookUpColumnInfo("rack_id", "ID", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None),
                //new DevExpress.XtraEditors.Controls.LookUpColumnInfo("rack_code", "编号", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None),
                //new DevExpress.XtraEditors.Controls.LookUpColumnInfo("rack_name", "名称", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
              
                

                dtRackSpec = proxy.Service.GetCuvShelf();
                dtRackSpec.Rows.InsertAt(dtRackSpec.NewRow(), 0);
                lueCuvShelf.Properties.DataSource = dtRackSpec;
                lueCuvShelfBatch.Properties.DataSource = dtRackSpec;

                DataTable table = proxy.Service.GetSamManageStatus();
                repositoryItemLookUpEdit2.DataSource = table;
                repositoryItemLookUpEdit3.DataSource = table;

                //dtRackPat = proxy.Service.GetRackDetail("");
                //gcSamDetail.DataSource = dtRackPat;

                DataRow[] rows = dtRack.Select(string.Format("ss_status = {0} or ss_status = {1} ", 0,5));
                DataTable newTable = dtRack.Clone();
                foreach (DataRow row in rows)
                {
                    newTable.Rows.Add(row.ItemArray);
                }
                gcRackInfo.DataSource = newTable;

                
                repositoryItemLookUpEdit1.DataSource = proxy.Service.GetSample();

                //试管架号

               

                #endregion
                

                #region 批量归档
                DateJY.EditValue = DateTime.Now;

                #endregion
                proxy.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        void FillRackBarcode(DataTable tb)
        {
            cbBatchRackBarcode.Properties.Items.Clear();
            foreach (DataRow row in tb.Rows)
            {
                cbBatchRackBarcode.Properties.Items.Add(row["rack_barcode"]);
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
                 ProxySamManage proxy = new ProxySamManage();


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

                if (dtFristBarCode == null || DateTime.Now.AddMinutes(-3) > dtFristBarCode || string.IsNullOrEmpty(operatorID))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCode = DateTime.Now;
                        operatorID = checkPassword.OperatorID;
                        operatorName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                

                //判断此条码信息是否有效
                DataTable dtBcPatients = proxy.Service.GetPatientsInfo(strBarCode);
                if (dtBcPatients.Rows.Count < 1)
                {
                    MessageDialog.Show("找不到该条码信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtBarCode.Text = "";
                    return;
                }
                string patid=dtBcPatients.Rows[0]["pat_id"].ToString();
                if (dtBcPatients.Rows.Count > 1)
                {
                    DataRow[] pt = dtBcPatients.Select(string.Format("pat_flag >0"));
                    if (pt.Length == 0)
                    {
                        DialogResult result = MessageDialog.Show("此样本尚未审核，是否强制归档！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            txtBarCode.Text = "";
                            txtBarCode.Focus();
                            return;
                        }
                    }
                    else
                    {
                        patid=pt[0]["pat_id"].ToString();
                    }
                }
                else
                {
                    object o = dtBcPatients.Rows[0]["pat_flag"];
                    if (o == null || o == DBNull.Value || o.ToString() == "0")
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
                DataTable dtRackDetail = proxy.Service.GetRackDetail(strSsid);

                DataRow[] rs = dtRackDetail.Select(string.Format("ssd_seq = '{0}' and ssd_id = '{1}'", spSeq.EditValue,strSsid));               

                if (rs.Length > 0)
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
                    MessageDialog.Show(msg+"条码已追加到该条码上!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                //向表内加入一条数据
                EntityRackDetail entity = new EntityRackDetail();
                entity.ssd_id = strSsid;

                entity.ssd_bar_code = strBarCode;
                entity.ssd_num_x = Convert.ToInt32(txtNumX.Text);
                entity.ssd_num_y = Convert.ToInt32(txtNumY.Text);
                entity.ssd_seq = intSeq;
                entity.ssd_status = 5;//未审核

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
                DataRow row = gvRackInfo.GetFocusedDataRow();
                row["ss_status"] = 5;//未审核
                EntitySamRack entitySamRack = new EntitySamRack();
                entitySamRack.ss_id = strSsid;
                entitySamRack.ss_status = 5;//未审核
                entitySamRack.ss_num = row["ss_num"] == null || row["ss_num"] == DBNull.Value ? 1 : gvSamDetail.RowCount + 1;
                row["ss_num"] = entitySamRack.ss_num;
                row["usestatus"] = entitySamRack.ss_num + "/" + Convert.ToInt32(row["cus_x_num"]) * Convert.ToInt32(row["cus_y_num"]);
                entitySamRack.ss_rack_id = strRackID;
                proxy.Service.ModifySamStoreRack(entitySamRack);

                //修改字典中试管架的状态
                EntityDictRack entityRack = new EntityDictRack();
                entityRack.rack_id = entitySamRack.ss_rack_id;
                entityRack.rack_status = entitySamRack.ss_status;
                proxy.Service.ModifyRackStatus(entityRack);

                //同时向详细表格内加入一行数据
                gcSamDetail.DataSource = proxy.Service.GetRackDetail(strSsid);

                intSeq++;
                spSeq.EditValue = intSeq;
                FocusRowChange(row);
                txtBarCode.Text = "";
                txtBarCode.Focus();
                dtFristBarCode = DateTime.Now;
                proxy.Dispose();
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
                DataTable rackDt = gcRackInfo.DataSource as DataTable;
                if (rackDt == null) return;
                DataRow[] rows = rackDt.Select(string.Format("rack_barcode = '{0}'", rackID));
                if (rows.Length < 1)
                {
                    MessageDialog.Show("此试管架条码不存在，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    txtRackID.Text = "";
                    return;
                }

                DataRow dr = rows[0];
                int index = rackDt.Rows.IndexOf(dr);

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
            DataRow[] rows = dtRackSpec.Select(string.Format("cus_code = '{0}'", lueCuvShelf.EditValue.ToString()));


            int x = Convert.ToInt32(rows[0]["cus_x_num"]);
            int y = Convert.ToInt32(rows[0]["cus_y_num"]);

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
            DataRow dr = gvRackInfo.GetFocusedDataRow();
                        
            ProxySamManage proxy = new ProxySamManage();
            
            if (dr != null)
            {
                strSsid = dr["ss_id"].ToString();
                lueCuvShelf.EditValue = dr["rack_spec"].ToString();
                strRackID = dr["rack_id"].ToString();
                intSsStauts = Convert.ToInt32(dr["ss_status"]);
                cmbColor.EditValue = dr["rack_color"];
              
                //根据ss_id来得到bar_code
                DataTable dtNow = proxy.Service.GetRackDetail(strSsid);
                if (dtNow != null)
                {                   
                    object maxSeq = dtNow.Compute("max(ssd_seq)", "");
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

        private void FocusRowChange(DataRow innerRow)
        {
            gridViewRack.Columns.Clear();

            if (innerRow == null)
            {
                return;
            }
            int xNum;
            int yNum;
            if (innerRow["cus_x_num"] != null && int.TryParse(innerRow["cus_x_num"].ToString(), out xNum) &&
                innerRow["cus_y_num"] != null && int.TryParse(innerRow["cus_y_num"].ToString(), out yNum))
            {
                BindGridRackUseStatusView(xNum, yNum, innerRow["ss_id"].ToString());
            }
        }

        private void FocusRowChangeBatch(DataRow innerRow)
        {
            gridViewBatchInfo.Columns.Clear();

            if (innerRow == null)
            {
                return;
            }
            int xNum;
            int yNum;
            if (innerRow["cus_x_num"] != null && int.TryParse(innerRow["cus_x_num"].ToString(), out xNum) &&
                innerRow["cus_y_num"] != null && int.TryParse(innerRow["cus_y_num"].ToString(), out yNum))
            {
                BindGridRackUseStatusViewForBatch(xNum, yNum, innerRow["ss_id"].ToString());
            }
        }

        #endregion

        #region 架子使用情况

        private void BindGridRackUseStatusView(int xNum, int yNum, string ssId)
        {

            panelControlRackUseStatus.Height = (yNum + 1) * 22+5;

            DataTable table = new DataTable();
            for (int i = 1; i <= xNum; i++)
            {
                table.Columns.Add(i.ToString());
            }
            for (int j = 1; j <= yNum; j++)
            {
                table.Rows.Add(table.NewRow());
            }
            ProxySamManage proxy = new ProxySamManage();
            DataTable dtRackDt = proxy.Service.GetRackDetail(ssId);
            foreach (DataRow row in dtRackDt.Rows)
            {
                int x = int.Parse(row["ssd_num_x"].ToString());
                int y = int.Parse(row["ssd_num_y"].ToString());
                if (table.Rows.Count - 1 < x - 1 || table.Columns.Count - 1 < y - 1)
                    continue;
                if (row["patflagstatus"].ToString() == "未审核" )
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

            panelControl7.Height = (yNum + 1) * 22 + 5;

            DataTable table = new DataTable();
            for (int i = 1; i <= xNum; i++)
            {
                table.Columns.Add(i.ToString());
            }
            for (int j = 1; j <= yNum; j++)
            {
                table.Rows.Add(table.NewRow());
            }
            ProxySamManage proxy = new ProxySamManage();
            DataTable dtRackDt = proxy.Service.GetRackDetail(ssId);
            foreach (DataRow row in dtRackDt.Rows)
            {
                int x = int.Parse(row["ssd_num_x"].ToString());
                int y = int.Parse(row["ssd_num_y"].ToString());
                if (table.Rows.Count - 1 < x - 1 || table.Columns.Count - 1 < y - 1)
                    continue;
                if (row["patflagstatus"].ToString() == "未审核")
                {
                    table.Rows[x - 1][y - 1] = 1;
                }
                else
                {
                    table.Rows[x - 1][y - 1] = 2;
                }

            }

            gridUseinfo.DataSource = table;
            proxy.Dispose();
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
            
            int[] handlers = gvSamDetail.GetSelectedRows();
            
            List<string> barCodeList=new List<string>();
            foreach (int handler in handlers)
            {
                DataRow row = gvSamDetail.GetDataRow(handler);

                if (row["ssd_satus"] != null && row["ssd_satus"].ToString() == "5")
                {
                    barCodeList.Add(row["ssd_bar_code"].ToString());
                }
            }

            if (barCodeList.Count == 0)
            {
             
              MessageDialog.Show("请选择未审核的标本进行删除！已归档审核标本只能销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            using (ProxySamManage proxy = new ProxySamManage())
            {
                proxy.Service.DeleteSamDetail(strSsid, strRackID, barCodeList);
            }

            RefreshForm();
           
        }
        #endregion

        #region 审核----审核这些记录，改变他们的状态到 （10 审核）
        private void btnVerify_Click(object sender, EventArgs e)
        {
            int[] handlers = gvSamDetail.GetSelectedRows();
            EntityRackDetail entity = new EntityRackDetail();
            ProxySamManage proxy = new ProxySamManage();
            foreach (int handler in handlers)
            {
                DataRow row = gvSamDetail.GetDataRow(handler);
                entity.ssd_bar_code = row["ssd_bar_code"].ToString();
                entity.ssd_id = row["ssd_id"].ToString();
                entity.ssd_status = 10;
                row["ssd_satus"] = 10;//审核

                //数据库中进行跟新
                proxy.Service.ModifySamDetail(entity);
            }


        }
        #endregion

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
            ProxySamManage proxy = new ProxySamManage();

            dtRackBatch = proxy.Service.GetDictRackList(Convert.ToDateTime(dateBatchFrom.EditValue), Convert.ToDateTime(dateEditBatchTo.EditValue));

            proxy.Dispose();
            string txtbarcode = cbBatchRackBarcode.Text;
            FillRackBarcode(dtRackBatch);
            cbBatchRackBarcode.Text = txtbarcode;
        }

        private void RefreshForm()
        {
            string strSsid = this.strSsid;
            ProxySamManage proxy = new ProxySamManage();
            dtRack = proxy.Service.GetDictRackList(Convert.ToDateTime(dateFrom.EditValue), Convert.ToDateTime(dateEditTo.EditValue));
            proxy.Dispose();

            dtRack.PrimaryKey = new[] {dtRack.Columns["rack_id"]};
            if (isHandWork)
            {
                FilterChanged();
            }
            else
            {
                FilterBatchChanged();
            }

            DataRow[] rowArry = dtRack.Select(string.Format("ss_id = '{0}'", strSsid));
            if (rowArry.Length == 0)
            {
                return;
            }

            DataRow drow = rowArry[0];
            int index = dtRack.Rows.IndexOf(drow);

            gvRackInfo.FocusedRowHandle = index;
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
                btnDeleteBatch.Visible = true;
                btnDel.Visible = false;
                //MessageBox.Show("pageBatch");
            }
            if (e.Page.Name.Equals("pageHand"))
            {
                FilterChanged();
                isHandWork = true;
                btnDeleteBatch.Visible = false;
                btnDel.Visible = true;
                //MessageBox.Show("pageHand");
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
                    DateJY.EditValue = DateTime.Now;
                    return;
                }


                if (string.IsNullOrEmpty(lueBatchItr.valueMember))
                {
                    MessageDialog.Show("请选择检验仪器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);

                    return;
                }

                ProxySamManage proxy = new ProxySamManage();

                dtPatiens = proxy.Service.GetBatchHandData(GetWhere());

                FilterPatients();

                if (dtPatiens.Rows.Count == 0)
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

            if ((radioGroupBatchCondition.EditValue == null || radioGroupBatchCondition.EditValue.ToString() == "0"))
            {
                gcPatients.DataSource = dtPatiens;
            }
            if (radioGroupBatchCondition.EditValue != null && radioGroupBatchCondition.EditValue.ToString() == "1")
            {
                filter = string.Format("ssd_bar_code is null or ssd_bar_code='' ");
            }

            if (radioGroupBatchCondition.EditValue != null && radioGroupBatchCondition.EditValue.ToString() == "2")
            {
                filter =  string.Format("ssd_bar_code is not null  ");
            }
           
            DataRow[] rows = dtPatiens.Select(filter);
            DataTable newTable = dtPatiens.Clone();
            foreach (DataRow row in rows)
            {
                newTable.Rows.Add(row.ItemArray);
            }
            gcPatients.DataSource = newTable;
        }

        #endregion
        
        #region 搜索查询相应条件的数据

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string strWhere = string.Format(
                "  patients.pat_date >= DateAdd(d,0,'{0}') and patients.pat_date < DateAdd(d,1,'{0}')",
                DateJY.DateTime.Date);

            strWhere += string.Format(" and pat_itr_id = '{0}' ", lueBatchItr.valueMember);
            if (radType.SelectedIndex == 0)
            {
                strWhere += " and dbo.convertSidToNumeric(patients.pat_sid) >= " + txtSamFrom.Text + " ";
                strWhere += " and dbo.convertSidToNumeric(patients.pat_sid) <= " + txtSamTo.Text + " ";
            }
            if (radType.SelectedIndex == 1)
            {
                strWhere += " and dbo.convertSidToNumeric(patients.pat_host_order) >= " + txtSamFrom.Text + " ";
                strWhere += " and dbo.convertSidToNumeric(patients.pat_host_order) <= " + txtSamTo.Text + " ";
            }

            return strWhere;
        }

        #endregion

        #region 顺序号还是样本号
        private void radType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radType.SelectedIndex == 0)
            {
                //MessageBox.Show("起始按样本号");
                labStart.Text = "样本号：";
                labStart.ForeColor = Color.RoyalBlue;
                //labEnd.Text = "终止样本号：";
                //labEnd.ForeColor = Color.RoyalBlue;
            }
            if (radType.SelectedIndex == 1)
            {
                //MessageBox.Show("起始按顺序号");
                labStart.Text = "顺序号：";
                labStart.ForeColor = Color.Red;
                //labEnd.Text = "终止顺序号：";
                //labEnd.ForeColor = Color.Red;
            }
        }
        
        private void btnFiled_Click(object sender, EventArgs e)
        {
            try
            {
                ProxySamManage proxy = new ProxySamManage();


                if (string.IsNullOrEmpty(cbBatchRackBarcode.Text))
                {
                    MessageDialog.Show("试管架不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    cbBatchRackBarcode.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(lueCuvShelfBatch.Text))
                {
                    MessageDialog.Show("架子类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    lueCuvShelfBatch.Focus();
                    return;
                }
               if (!proxy.Service.IsRaclBarCodeExists(cbBatchRackBarcode.Text))
               {
                   DialogResult result = MessageDialog.Show("确认生成架子号["+cbBatchRackBarcode.Text+"]，并开始归档？", "提示", MessageBoxButtons.YesNo,
                                                                 MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                   if (result != DialogResult.Yes)
                   {
                       cbBatchRackBarcode.Focus();
                       return;
                   }
                   string maxRackCode = proxy.Service.GetMaxRackCode();
                   EntityDictRack entity = new EntityDictRack();
                   entity.rack_code = (int.Parse(maxRackCode) + 1).ToString();
                   entity.rack_spec = lueCuvShelfBatch.EditValue.ToString();
                   entity.rack_ctype = string.IsNullOrEmpty(luBatchCtype.valueMember) ? "-1" : luBatchCtype.valueMember;
                   entity.rack_status = 0;
                   entity.rack_color = "无";
                   entity.rack_Barcode = cbBatchRackBarcode.Text;

                   entity.rack_id = proxy.Service.GetMaxRack();

                   int intRet = proxy.Service.InsertIntoRack(entity);

                   if (intRet > 0)
                   {
                       EntitySamRack SamEntity = new EntitySamRack();
                       SamEntity.ss_id = proxy.Service.GetMaxSamRackID();
                       SamEntity.ss_rack_id = entity.rack_id;
                       SamEntity.ss_status = entity.rack_status;
                       SamEntity.ss_num = 0;
                       intRet = proxy.Service.InsertSamRack(SamEntity);

                       if (intRet < 1)
                       {
                           MessageDialog.Show("添加一条记录到对应的SamStore_Rack表失败！", "提示", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                           return;
                       }
                   }
                   RefreshFormForBatch();
               }

                DataRow row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                if (row == null)
                {
                    MessageDialog.Show("当前架子号未生成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                string ssid = row["ss_id"].ToString();
                string rackid = row["rack_id"].ToString();
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

                gridView3.CloseEditor();
                DataTable dtDetail = gcRackDetail.DataSource as DataTable;
                if (dtDetail == null || dtDetail.Rows.Count == 0) return;
                DataRow[] rows = dtDetail.Select("isselected =1");
                if (rows.Length == 0)
                {
                    MessageDialog.Show("请勾选要归档的标本信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    return;
                }

                if (dtFristBarCodeForBatch == null || DateTime.Now.AddMinutes(-3) > dtFristBarCodeForBatch || string.IsNullOrEmpty(operatorIDForBatch))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        dtFristBarCodeForBatch = DateTime.Now;
                        operatorIDForBatch = checkPassword.OperatorID;
                        operatorNameForBatch = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }

                //FrmCheckPassword checkPassword = new FrmCheckPassword();
                //if (checkPassword.ShowDialog() == DialogResult.OK)
                //{
                //    operatorID = checkPassword.OperatorID;
                //    operatorName = checkPassword.OperatorName;
                //}
                //else
                //{
                //    return;
                //}
                DataTable table = dtDetail.Clone();
                foreach (DataRow dataRow in rows)
                {
                    if(dataRow["ssd_bar_code"]!=null&&dataRow["ssd_bar_code"]!=DBNull.Value&&!string.IsNullOrEmpty(dataRow["ssd_bar_code"].ToString()))
                    {
                        continue;
                    }
                    table.Rows.Add(dataRow.ItemArray);
                }
                int rowhander = 0;
                int colHander = 0;
                if (gridViewBatchInfo.RowCount > 0 && gridViewBatchInfo.FocusedColumn!=null)
                {
                     rowhander = gridViewBatchInfo.FocusedRowHandle;
                     colHander = Convert.ToInt32(gridViewBatchInfo.FocusedColumn.FieldName);
                }
                string msg = proxy.Service.BatchHandData(table, ssid, rackid, lueCuvShelfBatch.EditValue.ToString(),
                                            operatorNameForBatch, operatorIDForBatch, LocalSetting.Current.Setting.Description, rowhander, colHander, cbBatchRackBarcode.Text);

                proxy.Dispose();
                int sec =4;
                dtFristBarCodeForBatch = DateTime.Now;

                if (string.IsNullOrEmpty(msg))
                {
                    sec = 1;
                    msg = "操作成功";
                }
                RefrshBatchDetail();
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

        private void radioGroupCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterChanged();
        }

        private void FilterChanged()
        {
            string filter = string.Empty;

            if ((radioGroupCondition.EditValue == null || radioGroupCondition.EditValue.ToString() == "0") )
            {
                if (string.IsNullOrEmpty(luCtype.valueMember))
                {
                    gcRackInfo.DataSource = dtRack;
                    gvSam_FocusedRowChanged(null, null);
                    return;
                }
                filter=string.Format("rack_ctype = '{0}' ", luCtype.valueMember);
            }
            if (radioGroupCondition.EditValue != null && radioGroupCondition.EditValue.ToString() == "1")
            {
                 filter = string.IsNullOrEmpty(luCtype.valueMember)?string.Format("ss_status = {0} or ss_status = {1} ", 0, 5)
                    : string.Format("(ss_status = {0} or ss_status = {1}) and rack_ctype = '{2}'  ", 0, 5, luCtype.valueMember);
            }

            if (radioGroupCondition.EditValue != null && radioGroupCondition.EditValue.ToString() == "2")
            {
                filter = string.IsNullOrEmpty(luCtype.valueMember) ? string.Format("ss_status = {0} or ss_status = {1} ", 10, 15)
                   : string.Format("(ss_status = {0} or ss_status = {1}) and rack_ctype = '{2}'  ",10, 15, luCtype.valueMember);
            }
            DataRow[] rows = dtRack.Select(filter);
            DataTable newTable = dtRack.Clone();
            foreach (DataRow row in rows)
            {
                newTable.Rows.Add(row.ItemArray);
            }
            gcRackInfo.DataSource = newTable;
            gvSam_FocusedRowChanged(null, null);
        }

        private void FilterBatchChanged()
        {
            string filter = string.Empty;


            if (string.IsNullOrEmpty(luBatchCtype.valueMember) && string.IsNullOrEmpty(lueCuvShelfBatch.Text))
            {
                FillRackBarcode(dtRackBatch);
                return;
            }

            if (!string.IsNullOrEmpty(luBatchCtype.valueMember))
            {
                filter = string.Format(" (rack_ctype = '{0}' or rack_ctype='-1')  ", luBatchCtype.valueMember);
            }

          
            if (!string.IsNullOrEmpty(lueCuvShelfBatch.Text))
            {
                if (string.IsNullOrEmpty(filter))
                    filter = string.Format("  rack_spec = '{0}'  ", lueCuvShelfBatch.EditValue);
                else
                    filter = filter + string.Format(" and rack_spec = '{0}'  ", lueCuvShelfBatch.EditValue);
            }
            DataRow[] rows = dtRackBatch.Select(filter);
            DataTable newTable = dtRackBatch.Clone();
            foreach (DataRow row in rows)
            {
                newTable.Rows.Add(row.ItemArray);
            }
            FillRackBarcode(newTable);
        }

        private void luCtype_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            FilterChanged();
        }

        #endregion

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void luBatchCtype_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            lueBatchItr.ClearSelect();
            FilterBatchChanged();
        }

        private void radioGroupBatchCondition_SelectedIndexChanged(object sender, EventArgs e)
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
            DataTable dtDetail = gcRackDetail.DataSource as DataTable;
            if (dtDetail == null || dtDetail.Rows.Count == 0) return;
            DataRow[] rows = dtDetail.Select("isselected =1");
            if (rows.Length == 0)
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

            //int[] handlers = gridView3.GetSelectedRows();

            List<string> barCodeList = new List<string>();
            string ssid = string.Empty;
            string rackID = string.Empty;
            foreach (DataRow row in rows)
            {
                //DataRow row = gridView3.GetDataRow(handler);

                if (row["ssd_satus"] != null && row["ssd_satus"].ToString() == "5")
                {
                    ssid = row["ssd_id"].ToString();
                    rackID = row["ss_rack_id"].ToString();
                    barCodeList.Add(row["ssd_bar_code"].ToString());
                }
            }

            if (barCodeList.Count == 0)
            {

                MessageDialog.Show("请选择已归档的标本进行删除！已审核标本只能销毁", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            using (ProxySamManage proxy = new ProxySamManage())
            {
                proxy.Service.DeleteSamDetail(ssid, rackID, barCodeList);
            }

            RefrshBatchDetail();
        }

        DataRow GetSsidByBarCode(string barcode)
        {
            if (dtRackBatch != null)
            {
                DataRow[] rows = dtRackBatch.Select(string.Format("Rack_barcode='{0}'", barcode));
                if (rows.Length > 0 && rows[0]["ss_id"]!=null)
                {
                    return rows[0];
                }
            }
            return null;
        }
        private void btnPrintRackBc_Click(object sender, EventArgs e)
        {
            try
            {
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
                    DataRow row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (row != null)
                    {
                        rackcode = row["rack_id"].ToString();
                        typename = row["type_name"].ToString();
                    }
                }
                FrmPrintTempBarCode code = new FrmPrintTempBarCode(cbBatchRackBarcode.Text, rackcode, lueCuvShelfBatch.EditValue.ToString(), string.IsNullOrEmpty(typename) ? luBatchCtype.displayMember : typename, null, null);
                code.ShowDialog();

                if (code.DialogResult == DialogResult.OK)
                {

                    RefreshFormForBatch();
                    cbBatchRackBarcode.Text = code.RackBarCode;


                    ProxySamManage proxy = new ProxySamManage();
                    DataRow row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                    if (row == null)
                    {
                        gcRackDetail.DataSource = null;
                    }
                    else
                    {
                        DataTable dtNow = proxy.Service.GetRackDetail(row["ss_id"].ToString());
                        if (dtNow.Rows.Count > 0)
                            gcRackDetail.DataSource = dtNow;
                    }
                    FocusRowChangeBatch(row);
                    proxy.Dispose();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                gridView1.CloseEditor();
                DataTable dtFile = gcPatients.DataSource as DataTable;
                DataTable dtDetail = gcRackDetail.DataSource as DataTable;
                if (dtFile == null || dtFile.Rows.Count == 0) return;
                DataRow[] rowReports = dtFile.Select("isselected =1 and (patflagstatus='已打印' or patflagstatus='已报告')  ");
                DataRow[] rowNotReport = dtFile.Select("isselected =1 and patflagstatus<>'已打印' and patflagstatus<>'已报告'  ");
                DataRow[] rowAll = dtFile.Select("isselected =1");
                if (rowAll.Length == 0)
                {
                    MessageDialog.Show("请勾选要归档的标本信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    return;
                }
                bool useAllData = false;
                if (rowNotReport.Length > 0)
                {
                    DialogResult result = MessageDialog.Show("您勾选的某些标本未报告!是否允许归档", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        useAllData = true;
                    }
                }

                if (dtDetail == null)
                {
                    dtDetail = dtFile.Clone();
                }
                if (useAllData)
                {
                    foreach (DataRow dataRow in rowAll)
                    {
                        if (dtDetail.Select(string.Format("pat_bar_code='{0}' ",dataRow["pat_bar_code"])).Length > 0)
                            continue;
                        dtDetail.Rows.Add(dataRow.ItemArray);
                        dtFile.Rows.Remove(dataRow);
                    }
                }
                else
                {
                    foreach (DataRow dataRow in rowReports)
                    {
                        if (
                            dtDetail.Select(
                                string.Format("pat_bar_code='{0}' and (patflagstatus='已打印' or patflagstatus='已报告') ",
                                              dataRow["pat_bar_code"])).Length > 0)
                            continue;
                        dtDetail.Rows.Add(dataRow.ItemArray);
                        dtFile.Rows.Remove(dataRow);
                        //dtPatiens.Rows.Remove(dataRow);
                    }
                }
                gcRackDetail.DataSource = dtDetail;
                gcPatients.RefreshDataSource();
                gcRackDetail.RefreshDataSource();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

                gridView3.CloseEditor();
                DataTable dtDetail = gcRackDetail.DataSource as DataTable;
                DataTable dtFile = gcPatients.DataSource as DataTable;
                if (dtDetail == null || dtDetail.Rows.Count == 0) return;
                DataRow[] rows = dtDetail.Select("isselected =1");
                if (rows.Length == 0)
                {
                    MessageDialog.Show("请勾选要还原归档的标本信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                       MessageBoxDefaultButton.Button1);
                    return;
                }

                if (dtFile == null)
                {
                    dtFile = dtDetail.Clone();
                }
                foreach (DataRow dataRow in rows)
                {
                    if (dataRow["ssd_createtime"] != null && dataRow["ssd_createtime"] != DBNull.Value)
                    {
                        dataRow["isselected"] = 0;
                        continue;
                    }
                    if (dtFile.Select(string.Format("pat_bar_code='{0}'", dataRow["pat_bar_code"])).Length > 0)
                    {
                        dtDetail.Rows.Remove(dataRow);
                        continue;
                    }
                    dtFile.Rows.Add(dataRow.ItemArray);
                    //dtPatiens.Rows.Add(dataRow.ItemArray);
                    dtDetail.Rows.Remove(dataRow);
                }
                gcPatients.RefreshDataSource();
                gcRackDetail.RefreshDataSource();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void lueCuvShelfBatch_EditValueChanged(object sender, EventArgs e)
        {
            FilterBatchChanged();
        }

        private void lueBatchItr_onBeforeFilter(ref string strFilter)
        {
            //当前选中的物理组ID
            string currentSelectType = luBatchCtype.valueMember;

            //是否有物理组
            if (currentSelectType != null && currentSelectType.Trim(null) != string.Empty)
            {
                ////根据仪器数据类型过滤出当前物理组的仪器
                //if (PatEnter.ItrDataType == LIS_Const.InstmtDataType.Normal || PatEnter.ItrDataType == LIS_Const.InstmtDataType.Eiasa)
                //{
                //    strFilter += string.Format(" and itr_type='{0}' and (itr_rep_flag='{1}' or  itr_rep_flag='{2}')", currentSelectType, LIS_Const.InstmtDataType.Normal, LIS_Const.InstmtDataType.Eiasa);
                //}
                //else
                //{
                strFilter += string.Format(" and itr_type='{0}'", currentSelectType);
                //}
            }
         

            if (!UserInfo.isAdmin)
            {
                //非管理员：列出有权限的仪器
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    strFilter += string.Format(" and itr_type in ({0}) and itr_id in ({1})", UserInfo.sqlUserTypesFilter, UserInfo.sqlUserItrs);
                }
                else
                {
                    strFilter += " and 1=2";
                }
            }
        }

        private void cbBatchRackBarcode_SelectedValueChanged(object sender, EventArgs e)
        {
            RefrshBatchDetail();
        }

        private void RefrshBatchDetail()
        {
            if (!string.IsNullOrEmpty(cbBatchRackBarcode.Text))
            {
                ProxySamManage proxy = new ProxySamManage();
                DataRow row = GetSsidByBarCode(cbBatchRackBarcode.Text);
                if (row == null)
                {
                    gcRackDetail.DataSource = null;
                }
                else
                {
                    if (row["rack_spec"] != null && row["rack_spec"] != DBNull.Value)
                    {
                        lueCuvShelfBatch.EditValue = row["rack_spec"].ToString();
                    }
                    DataTable dtNow = proxy.Service.GetRackDetail(row["ss_id"].ToString());
                    gcRackDetail.DataSource = dtNow;
                }
                FocusRowChangeBatch(row);
                proxy.Dispose();
            }
        }

        private void checkPatients_CheckedChanged(object sender, EventArgs e)
        {
            gridView1.CloseEditor();
            DataTable dtDetail = gcPatients.DataSource as DataTable;
            if (dtDetail == null || dtDetail.Rows.Count == 0) return;

            foreach (DataRow row in dtDetail.Rows)
            {
                row["isselected"] = checkPatients.Checked ? 1 : 0;
            }
        }

        private void checkAllDetail_CheckedChanged(object sender, EventArgs e)
        {
            gridView3.CloseEditor();
            DataTable dtDetail = gcRackDetail.DataSource as DataTable;
            if (dtDetail == null || dtDetail.Rows.Count == 0) return;

            foreach (DataRow row in dtDetail.Rows)
            {
                row["isselected"] = checkAllDetail.Checked ? 1 : 0;
            }
        }

        private void gridViewBatchInfo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
           
        }

        private void gridControlRack_Click(object sender, EventArgs e)
        {
            //
        }

        private void gridViewBatchInfo_Click(object sender, EventArgs e)
        {
            gridViewBatchInfo.OptionsSelection.EnableAppearanceFocusedCell = true;
        }

 

  

       


   

      
        

        
        

        

        

        
        
        
        
        
        




        


        








    }
}
