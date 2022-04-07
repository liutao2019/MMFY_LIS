using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using lis.client.control;
using dcl.client.wcf;
using dcl.common;
using dcl.client.frame;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmSecondSign : DevExpress.XtraEditors.XtraForm
    {
        public FrmSecondSign()
        {
            InitializeComponent();
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridControlCombineList.MouseDown += new MouseEventHandler(gridControlCombineList_MouseDown);
            this.gridViewCombineList.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(gridViewCombineList_CustomDrawColumnHeader);
        }
        private bool CheckReceiveTimeAndPatdate = false;
        #region CheckBoxOnGridHeader 全选按钮

        public void gridViewCombineList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "col_selected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {


            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;

            info = gridheadercheckbox.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = gridheadercheckbox.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }
        private bool bGridHeaderCheckBoxState = false;
        protected virtual void gridControlCombineList_MouseDown(object sender, MouseEventArgs e)
        {

            Point pt = this.gridViewCombineList.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewCombineList.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "col_selected")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridViewCombineList.InvalidateColumnHeader(this.gridViewCombineList.Columns["col_selected"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            List<EntitySampRegister> SampRegister = this.bsSampRegister.DataSource as List<EntitySampRegister>;
            foreach (var item in SampRegister)
            {
                item.Checked = bGridHeaderCheckBoxState;
            }
            this.bsSampRegister.DataSource = SampRegister;
            this.gridControlCombineList.RefreshDataSource();
        }
        #endregion

        private void FrmSecondSign_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnRefresh.Name,
                sysToolBar1.BtnPageDown.Name,
                sysToolBar1.BtnStat.Name,
                sysToolBar1.BtnClose.Name
            });
            sysToolBar1.BtnPageDown.Caption = "跳空";
            sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            sysToolBar1.BtnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BtnRefresh_Click);

            deRegisterDate.EditValue = ServerDateTime.GetServerDateTime().Date;

            CheckReceiveTimeAndPatdate = UserInfo.GetSysConfigValue("Other_CheckReceiveTimeAndPatdate") == "是";

            col_selected.Caption = string.Empty;
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        void BtnRefresh_Click(object sender, EventArgs e)
        {
            SearchRegisteredBarcode(false);
        }

        private void txt_pos_y_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_pos_x_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            ProxySecondSign proxy = new ProxySecondSign();

            if (e.KeyValue == 13 && this.txt_barcode.Text.Trim(null) != string.Empty)
            {
                if (this.txt_dept.valueMember == null || this.txt_dept.valueMember.Trim() == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("请选择[接收室]");
                    this.ActiveControl = this.txt_dept;
                    this.txt_dept.Focus();
                    return;
                }

                if (this.selectDicTubeRack1.valueMember == null)
                {
                    lis.client.control.MessageDialog.Show("请选择[试管架类型]");
                    this.selectDicTubeRack1.Focus();
                    this.ActiveControl = this.selectDicTubeRack1;
                    return;
                }

                EntityPidReportMain dsPat = GetPatientDataByBarCode(SQLFormater.FormatSQL(this.txt_barcode.Text));
                EntityPidReportMain dtPatInfo = dsPat;

                if (dtPatInfo == null)
                {
                    lis.client.control.MessageDialog.Show("无此条形码的信息,请与临床科联系");

                    this.txt_barcode.Text = string.Empty;
                    this.ActiveControl = this.txt_barcode;
                    this.txt_barcode.Focus();
                }
                else
                {
                    if (dtPatInfo.BcStatus != null &&
                        (dtPatInfo.BcStatus.ToString() == "9"
                        || dtPatInfo.BcStatus.ToString() == "20" || dtPatInfo.BcStatus.ToString() == "40"
                        || dtPatInfo.BcStatus.ToString() == "60" || dtPatInfo.BcStatus.ToString() == "70"))
                    {
                        /***************增加最后一次操作签名的操作人和时间************************/
                        string name = string.Empty;
                        string time = string.Empty;
                        string remark = string.Empty;

                        proxy.Service.GetLastBarcodeAction(this.txt_barcode.Text, out name, out time, out remark, Convert.ToInt32(dtPatInfo.BcStatus.ToString()));
                        //PatientControl.GetLastBarcodeAction(this.txt_barcode.Text, out name, out time, out remark, Convert.ToInt32(dtPatInfo.BcStatus.ToString()));

                        ClearAndFocusBarcodeInput();
                        return;
                    }
                    if (CheckReceiveTimeAndPatdate)
                    {
                        DateTime recvTime;
                        DateTime jyTime;
                        if (dtPatInfo.SampApplyDate != null &&
                            DateTime.TryParse(dtPatInfo.SampApplyDate.ToString(), out recvTime)
                            && dtPatInfo.SampCheckDate != null &&
                            DateTime.TryParse(dtPatInfo.SampCheckDate.ToString(), out jyTime) &&
                            recvTime > jyTime)
                        {
                            MessageDialog.Show("签收时间大于检验时间，请检查！", "提示");
                            return;
                        }

                    }
                    if (dtPatInfo.BcStatus == null || dtPatInfo.BcStatus.ToString() != "5")
                    {
                        //MessageDialog.ShowAutoCloseDialog("此条码号未签收，请核对");
                        //ClearAndFocusBarcodeInput();
                        EntitySampOperation oper = new EntitySampOperation();
                        oper.OperationID = UserInfo.userInfoId;
                        oper.OperationName = UserInfo.userName;
                        EntitySampMain SampMain = new EntitySampMain();
                        SampMain.SampBarCode = this.txt_barcode.Text;
                        SampMain.SampBarId = this.txt_barcode.Text;
                        List<EntitySampMain> SampMainList = new List<EntitySampMain>();
                        SampMainList.Add(SampMain);
                        bool success = new dcl.client.sample.ReceiveStep().UpdateStatus(oper, SampMainList); //new dcl.client.sample.ReceiveStep().UpdateStatus(this.txt_barcode.Text, UserInfo.userInfoId, UserInfo.userName);
                        if (!success)
                        {
                            MessageDialog.Show("签收失败！", "提示");
                            return;
                        }
                    }

                    List<EntityPidReportDetail> dtPatientsMi = dsPat.ListPidReportDetail;

                    List<string> combineIDs = new List<string>();
                    foreach (EntityPidReportDetail row in dtPatientsMi)
                    {
                        combineIDs.Add(row.ComId.ToString());
                    }

                    if (!string.IsNullOrEmpty(dtPatInfo.SampType.ToString()))
                    {
                        List<EntityDicInstrument> dsInstrments = proxy.Service.GetInstrumentByComIds(combineIDs);
                        if (dsInstrments != null && dsInstrments.Count > 0)
                        {
                            List<EntityDicInstrument> dtInstrumentType = new List<EntityDicInstrument>(); //用于存放过滤物理组的仪器
                            string typeId = txt_dept.valueMember; //物理组别
                            foreach (EntityDicInstrument drIns in dsInstrments)
                            {
                                if (drIns.ItrLabId.ToString() == typeId) //判断仪器是否属于该物理组
                                {
                                    dtInstrumentType.Add(drIns);
                                }
                            }
                            if (dtInstrumentType.Count == 0)
                            {
                                if (MessageDialog.Show("当前条码组合不属于此物理组,是否继续", "提示", MessageBoxButtons.YesNo) ==
                                    DialogResult.No)
                                    return;
                            }
                        }

                    }
                    EntitySampRegister entity = ControlToEntity(dtPatientsMi);
                    entity.PidName = dtPatInfo.PidName.ToString();
                    entity.RegBarCode = this.txt_barcode.Text;
                    entity.RegDate = Convert.ToDateTime(deRegisterDate.EditValue);


                    int result = proxy.Service.SaveShelfBarcode(entity);

                    if (result == -1)
                    {
                        //lis.client.control.MessageDialog.Show("此条码号已录入，请核对");

                        /***************增加最后一次操作签名的操作人和时间************************/
                        string name = string.Empty;
                        string time = string.Empty;
                        string remark = string.Empty;

                        proxy.Service.GetLastBarcodeAction(this.txt_barcode.Text, out name, out time, out remark, 20);
                        //PatientControl.GetLastBarcodeAction(this.txt_barcode.Text, out name, out time, out remark, 20);
                        //if (remark.Contains("；"))
                        //{
                        //    lis.client.control.MessageDialog.Show(string.Format("此条码号已录入，请核对!\r\n操作人：{0}，时间：{1},\r\n备注：{2}", name, time, remark.Substring(0, remark.IndexOf('；'))));
                        //}
                        //else
                        //    lis.client.control.MessageDialog.Show(string.Format("此条码号已录入，请核对!\r\n操作人：{0}，时间：{1},\r\n备注：{2}", name, time, remark));
                        /********************************************************************/

                        ClearAndFocusBarcodeInput();
                    }
                    else if (result == -2)
                    {
                        lis.client.control.MessageDialog.Show("编号已存在，请核对");

                        ClearAndFocusBarcodeInput();
                    }

                    else if (result > 0)
                    {
                        if (this.gridControlCombineList.DataSource != null)
                        {
                            //*********写入bc_sign表中记录条码登记************************************************** 20120920
                            string remark = string.Format(@"接收室：{0}；编号：{1}；登记日期：{2}；", this.txt_dept.displayMember, this.txt_sequence.Text.ToString(), Convert.ToDateTime(deRegisterDate.EditValue).ToString());
                            EntitySampOperation sampOperation = new EntitySampOperation();
                            sampOperation.OperationTime = ServerDateTime.GetServerDateTime();
                            sampOperation.OperationID = UserInfo.userInfoId.ToString();
                            sampOperation.OperationName = UserInfo.userName.ToString();
                            sampOperation.OperationStatus = "20";
                            sampOperation.Remark = remark;
                            EntitySampMain sampMain = new EntitySampMain();
                            sampMain.SampBarId = this.txt_barcode.Text.ToString();
                            sampMain.SampBarCode = this.txt_barcode.Text.ToString();
                            //插入bc_sign表行，跟踪记录 20120920
                            ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                            proxyProcess.Service.SaveSampProcessDetail(sampOperation, sampMain);

                            /***************************************************************************************/

                            List<EntitySampRegister> data = this.gridControlCombineList.DataSource as List<EntitySampRegister>;

                            EntitySampRegister row = new EntitySampRegister();
                            int RegSn = proxy.Service.GetSampRegisterMaxId();
                            row.RegSn = RegSn;
                            row.PidName = entity.PidName;
                            row.RegNumber = entity.RegNumber;
                            row.RegComName = entity.RegComName;
                            row.RegBarCode = entity.RegBarCode;
                            row.Checked = false;

                            data.Add(row);

                            this.gridControlCombineList.DataSource = data;

                            this.gridControlCombineList.RefreshDataSource();

                            this.gridViewCombineList.MoveLast();

                            gridViewCombineList_FocusedRowChanged(this.gridViewCombineList, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(data.Count - 1, data.Count - 1));
                        }
                        else
                        {
                            SearchRegisteredBarcode(false);
                        }


                        if (IncreaseXYNumber())
                        {
                            IncreaseSeqNumber();
                            ClearAndFocusBarcodeInput();
                        }
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("保存出错！请联系管理员");
                    }
                }
            }
        }

        private void ClearAndFocusBarcodeInput()
        {
            this.txt_barcode.Text = string.Empty;
            this.ActiveControl = this.txt_barcode;
            this.txt_barcode.Focus();
        }

        private void sysToolBar1_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            if (IncreaseXYNumber())
            {
                IncreaseSeqNumber();
            }
        }

        private bool IncreaseXYNumber()
        {
            if (this.selectDicTubeRack1.valueMember == null)
            {
                lis.client.control.MessageDialog.Show("请选择[试管架类型]");
                this.selectDicTubeRack1.Focus();
                this.ActiveControl = this.selectDicTubeRack1;
                return false;
            }

            int y = Convert.ToInt32(this.txt_pos_y.EditValue);
            if (y < (int)this.txt_pos_y.Properties.MaxValue)
            {
                y = y + 1;
                this.txt_pos_y.EditValue = y;
            }
            else
            {
                int x = Convert.ToInt32(this.txt_pos_x.EditValue);

                if (x < (int)this.txt_pos_x.Properties.MaxValue)
                {
                    x = x + 1;
                    this.txt_pos_x.EditValue = x;
                    this.txt_pos_y.EditValue = 1;
                }
                else
                {
                    if (lis.client.control.MessageDialog.Show("此架子已满，是否自动更新架子号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {

                        int shelfno = Convert.ToInt32(this.txt_shelf_number.EditValue);
                        shelfno = shelfno + 1;
                        this.txt_shelf_number.EditValue = shelfno;

                        this.txt_pos_x.EditValue = 1;
                        this.txt_pos_y.EditValue = 1;

                    }
                    else
                    {
                        txt_sequence.Focus();
                        this.txt_barcode.Text = string.Empty;
                        return false;
                    }
                }
            }
            return true;
        }

        private void IncreaseSeqNumber()
        {
            int seq = Convert.ToInt32(this.txt_sequence.EditValue);
            seq = seq + 1;
            this.txt_sequence.EditValue = seq;
        }

        /// <summary>
        /// 根据条码获取病人信息
        /// </summary>
        /// <param name="barCode"></param>
        private EntityPidReportMain GetPatientDataByBarCode(string barCode)
        {
            ProxyPidReportMain proxy = new ProxyPidReportMain();
            EntityPidReportMain patients = new EntityPidReportMain();
            patients = proxy.Service.GetPatientsByBarCode(barCode);
            return patients;
        }

        private void SearchRegisteredBarcode(bool getMaxSeqNo)
        {
            ProxySecondSign proxy = new ProxySecondSign();
            if (this.txt_dept.valueMember != null
                && this.txt_dept.valueMember.ToString() != string.Empty)
            {
                List<EntitySampRegister> sampRegister = proxy.Service.GetCuvetteRegisteredBarcodeInfo(this.txt_dept.valueMember.ToString(), Convert.ToDateTime(deRegisterDate.EditValue));
                foreach (EntitySampRegister item in sampRegister)
                {
                    item.Checked = false;
                }

                this.gridControlCombineList.DataSource = sampRegister;
                this.bsSampRegister.DataSource = sampRegister;

                if (getMaxSeqNo)
                {
                    EntitySampRegister objMaxNo = sampRegister.OrderByDescending(i => i.RegNumber).FirstOrDefault();

                    if (objMaxNo == null)
                    {
                        this.txt_sequence.EditValue = 1;
                    }
                    else
                    {
                        this.txt_sequence.EditValue = Convert.ToInt32(objMaxNo.RegNumber) + 1;
                    }
                }

                this.gridViewCombineList.MoveLast();


            }
            else
            {
                this.gridControlCombineList.DataSource = null;
            }
        }

        private EntitySampRegister ControlToEntity(List<EntityPidReportDetail> dtPatientsMi)
        {
            EntitySampRegister entity = new EntitySampRegister();
            entity.RegLabId = this.txt_dept.valueMember;
            entity.RegYPlace = Convert.ToInt32(this.txt_pos_y.EditValue);
            entity.RegXPlace = Convert.ToInt32(this.txt_pos_x.EditValue);
            entity.RegNumber = Convert.ToInt32(this.txt_sequence.EditValue);
            entity.RegUserId = UserInfo.userInfoId;
            entity.RegRackNo = Convert.ToInt32(this.txt_shelf_number.EditValue);
            entity.RegRackCode = selectDicTubeRack1.valueMember;

            bool needplus = false;
            foreach (EntityPidReportDetail dr in dtPatientsMi)
            {
                if (!string.IsNullOrEmpty(dr.PatComName))
                {
                    if (needplus)
                    {
                        entity.RegComName += "+";
                    }

                    entity.RegComName += dr.PatComName.ToString();

                    needplus = true;
                }
            }
            return entity;
        }

        private void gridViewCombineList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ProxySecondSign proxy = new ProxySecondSign();
            EntitySampRegister drCuv = this.gridViewCombineList.GetFocusedRow() as EntitySampRegister;
            if (drCuv != null)
            {
                long st_id = Convert.ToInt64(drCuv.RegSn);
                List<EntitySampRegister> sampRegister = new List<EntitySampRegister>();
                sampRegister = proxy.Service.GetSampRegister(st_id);
                if (sampRegister.Count > 0)
                {
                    EntitySampRegister dr = sampRegister[0];

                    txt_bc_barcode.EditValue = dr.RegBarCode;
                    txt_bc_receivedept.EditValue = dr.ProName;
                    txt_bc_receivetime.EditValue = dr.RegDate;
                    txt_bc_sequence.EditValue = dr.RegNumber;
                    txt_bc_shelfnumber.EditValue = dr.RegRackNo;
                    txt_bc_pos_x.EditValue = dr.RegXPlace;
                    txt_bc_pos_y.EditValue = dr.RegYPlace;


                    txt_pat_inno.EditValue = dr.PidInNo;
                    dr.PidAge = AgeConverter.TrimZeroValue(dr.PidAge);
                    dr.PidAge = AgeConverter.ValueToText(dr.PidAge);
                    txt_pat_name.EditValue = dr.PidName;
                    txt_pat_sex.EditValue = dr.PidSex;
                    txt_pat_age.EditValue = dr.PidAge;
                    txt_pat_dept.EditValue = dr.PidDeptName;
                    txt_pat_bedno.EditValue = dr.PidBedNo;
                    txt_pat_senddoc.EditValue = dr.PidDoctorName;
                    txt_pat_iptimes.EditValue = dr.PidAdmissTimes;
                    txt_pat_diag.EditValue = dr.PidDiag;
                    txt_pat_combine.EditValue = dr.RegComName;

                    if (!Compare.IsEmpty(dr.PidSrcId))
                    {
                        txt_bc_sampleori.SelectByID(dr.PidSrcId.ToString());
                    }
                    else
                    {
                        txt_bc_sampleori.SelectByDispaly(dr.SrcName.ToString());
                    }

                    if (!Compare.IsEmpty(dr.SampSamId))
                    {
                        txt_bc_sample.SelectByID(dr.SampSamId.ToString());
                    }
                    else
                    {
                        txt_bc_sample.SelectByDispaly(dr.SampSamName.ToString());
                    }

                    if (!string.IsNullOrEmpty(dr.StBcOccDate))
                    {
                        txt_pat_senddate.EditValue = dr.StBcOccDate;
                    }
                }
            }
        }

        /// <summary>
        /// 删除登记资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            StringBuilder delBarMsg = new StringBuilder();
            ProxySecondSign proxy = new ProxySecondSign();
            this.gridViewCombineList.CloseEditor();
            //***************************************************************
            //设置一个已登记组合的条码号信息
            StringBuilder canNotDel = new StringBuilder();
            string havedRecord = string.Empty;
            //***************************************************************

            List<EntitySampRegister> rows = new List<EntitySampRegister>();
            List<EntitySampRegister> SampRegisterList = this.gridViewCombineList.DataSource as List<EntitySampRegister>;

            ProxySampProcessDetail proxyxxx = new ProxySampProcessDetail();

            foreach (EntitySampRegister row in SampRegisterList)
            {
                if (row.Checked)
                {
                    //*******************************************************************
                    //判断条码号是否已有项目已登记，有则不能删除

                    List<EntitySampProcessDetail> listxx = proxyxxx.Service.GetSampProcessDetail(row.RegBarCode.ToString());

                    int x = listxx.FindIndex(w => w.ProcStatus == "20");
                    if (x >= 0)
                    {
                        canNotDel.AppendLine(listxx[x].ProcContent);
                        havedRecord += row.RegBarCode.ToString() + " ";
                    }
                    //*******************************************************************
                    rows.Add(row);
                    delBarMsg.AppendLine(string.Format("条码号：{0} 姓名：{1}", row.RegBarCode, row.PidName));
                    continue;
                }

            }


            if (rows.Count == 0)
            {
                EntitySampRegister dr = this.bsSampRegister.Current as EntitySampRegister;
                if (dr != null)
                {
                    //*******************************************************************
                    //判断条码号是否已有项目已登记，有则不能删除

                    List<EntitySampProcessDetail> listxx = proxyxxx.Service.GetSampProcessDetail(dr.RegBarCode.ToString());

                    int x = listxx.FindIndex(w => w.ProcStatus == "20");

                    if (x >= 0)
                    {
                        canNotDel.AppendLine(listxx[x].ProcContent);
                        havedRecord += dr.RegBarCode.ToString() + " ";
                    }
                    //*******************************************************************
                    else
                    {
                        rows.Add(dr);
                        delBarMsg.AppendLine(string.Format("条码号：{0} 姓名：{1}", dr.RegBarCode, dr.PidName));
                    }

                }
            }

            //*******************************************************************
            //显示不能删除的条码号
            if (canNotDel.Length > 1)
            {
                MessageDialog.Show(canNotDel.ToString() + "\n\r已有项目已录入，不能删除！");
            }

            //*******************************************************************
            if (rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请选择需要删除的数据！");
                return;
            }

            //进行身份验证 20120920



            if (lis.client.control.MessageDialog.Show(string.Format("您是否确定要删除选择的数据？\r\n{0}", delBarMsg.ToString()), "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //身份认证
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");

                DialogResult dig = frmCheck.ShowDialog();


                if (dig == DialogResult.OK)
                {
                    bool delResult = false;
                    foreach (EntitySampRegister dr in rows)
                    {


                        Int64 st_id = Convert.ToInt64(dr.RegSn);
                        string bc_cname = dr.PidName.ToString();
                        string st_bar_code = dr.RegBarCode.ToString();//条码号
                        string st_bc_cname = dr.RegComName.ToString();//组合项目


                        bool result = proxy.Service.DeleteShelfBarcode(st_id);
                        delResult = result;
                        if (result)
                        {
                            EntitySampOperation sampOperation = new EntitySampOperation();
                            sampOperation.OperationTime = ServerDateTime.GetServerDateTime();
                            sampOperation.OperationID = frmCheck.OperatorID;
                            sampOperation.OperationName = frmCheck.OperatorName;
                            sampOperation.OperationStatus = "530";
                            sampOperation.Remark = "组合:" + st_bc_cname;
                            EntitySampMain sampMain = new EntitySampMain();
                            sampMain.SampBarId = st_bar_code;
                            sampMain.SampBarCode = st_bar_code;
                            //插入bc_sign表行，跟踪记录 20120920
                            ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                            proxyProcess.Service.SaveSampProcessDetail(sampOperation, sampMain);

                        }



                    }
                    if (delResult)
                    {
                        MessageDialog.ShowAutoCloseDialog("删除成功！");

                    }
                    else
                    {
                        MessageDialog.ShowAutoCloseDialog("删除失败！");
                    }
                    SearchRegisteredBarcode(true);
                }
                else if (dig == DialogResult.No)
                {
                    lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                    return;
                }
            }
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            if (txt_dept.valueMember != null && txt_dept.displayMember != null)
            {
                FrmSecondStat fss = new FrmSecondStat(txt_dept.valueMember, txt_dept.displayMember);
                fss.ShowDialog();
            }
            else
            {
                FrmSecondStat fss = new FrmSecondStat();
                fss.ShowDialog();
            }
        }

        private void deRegisterDate_EditValueChanged(object sender, EventArgs e)
        {
            SearchRegisteredBarcode(false);
        }

        private void txt_dept_DisplayTextChanged(object sender, control.ValueChangeEventArgs args)
        {
            SearchRegisteredBarcode(true);
        }

        private void txt_dept_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            SearchRegisteredBarcode(true);
        }

        private void selectDicTubeRack1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (selectDicTubeRack1.valueMember != null)
            {
                EntityDicTubeRack dr = selectDicTubeRack1.dtSource.Where(i => i.RackCode == selectDicTubeRack1.valueMember.ToString()).FirstOrDefault();
                if (dr != null)
                {
                    int x = int.MaxValue;
                    int y = int.MaxValue;
                    if (dr != null)
                    {
                        x = Convert.ToInt32(dr.RackXAmount);
                        y = Convert.ToInt32(dr.RackYAmount);
                    }

                    this.txt_pos_x.Properties.MaxValue = x;
                    this.txt_pos_y.Properties.MaxValue = y;
                }
            }
        }
    }
}