using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.common;
using lis.client.control;

using dcl.entity;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using dcl.client.cache;

namespace dcl.client.samstock
{
    #region 新代码
    public partial class FrmSamSave : FrmCommon
    {
        #region 全局变量
        List<EntityDicSampTubeRack> dtFile;
        List<EntityDicSampTubeRack> dtSave;
        List<EntityDicSampStoreArea> dtCups;

        private bool Sam_EnableDirectOperator = false;
        private string opName = string.Empty;
        string opId = string.Empty;

        private bool gvFileStatus = false;

        private bool gvSaveStatus = false;
        #endregion

        #region 加载

        public FrmSamSave()
        {
            InitializeComponent();
        }

        private void InitData()
        {
            dateFrom.EditValue = DateTime.Now.AddDays(-1).Date;
            dateEditTo.EditValue = DateTime.Now.Date;

            AddCheckColumnToGC();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                PoxySampStock proxy = new PoxySampStock();

                List<EntityDicSampTubeRack> dtRack = proxy.Service.GetDictRackListForSave(checkDirectFlag.Checked ? DateTime.Now.AddYears(1) : Convert.ToDateTime(dateFrom.EditValue),
                                                                 Convert.ToDateTime(dateEditTo.EditValue));

                List<EntityDicSampTubeRack> rows = dtRack.Where(w => w.SrStatus == 5).ToList(); //未审核
                dtFile = rows;
                gcFile.DataSource = dtFile;

                rows = dtRack.Where(w => w.SrStatus == 15 || w.SrStatus == 10).ToList();//已存储
                dtSave = rows;

                gcSave.DataSource = dtSave;

                //架子状态
                repositoryItemLookUpEditStatus.DataSource = proxy.Service.GetSamManageStatus();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("查询出错" + ex.Message);
            }
        }

        private void FrmSamSave_Load(object sender, EventArgs e)
        {
            PoxySampStock proxy = new PoxySampStock();
            //冰箱
            List<EntityDicSampStore> dtIceBox = proxy.Service.GetIceBox();

            lueIceBox.Properties.DataSource = dtIceBox;
            repositoryItemLookUpEditIce.DataSource = dtIceBox;
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
            dtCups = proxy.Service.GetCups();

            lueCupID.Properties.DataSource = dtCups;
            repositoryItemLookUpEditCup.DataSource = dtCups;
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

            Sam_EnableDirectOperator = UserInfo.GetSysConfigValue("Sam_EnableDirectOperator") == "是";

            if (Sam_EnableDirectOperator)
            {
                gcSaveJZ.Visible = false;
                checkDirectFlag.Checked = true;
            }

            InitData();

            sysToolBar.BtnConfirm.Caption = "完成";
            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnSearch.Name
                    , sysToolBar.BtnAudit.Name
                    , sysToolBar.BtnUndoAudit2.Name
                    ,sysToolBar.BtnConfirm.Name  //添加完成按钮
                , sysToolBar.BtnClose.Name});

            //默认不显示完成按钮，勾选直接扫描存储时显示
            sysToolBar.BtnConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            sysToolBar.OnBtnSearchClicked += btnRefresh_Click;
            sysToolBar.OnCloseClicked += btnClosed_Click;
            sysToolBar.OnAuditClicked += btnAudit_Click;
            sysToolBar.OnUndoAuditClicked += btnAntiAudit_Click;

            sysToolBar.OnBtnConfirmClicked += btnFinish_Click;//完成按钮事件
        }
        #endregion

        #region 选择后检索出待确定存储地址的架子
        /// <summary>
        /// 选择后检索出待确定存储地址的架子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void lueCType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            string strType = luCtype.valueMember;
            if (string.IsNullOrEmpty(strType))
            {
                gcFile.DataSource = dtFile;
                gcSave.DataSource = dtSave;
                return;
            }

            List<EntityDicSampTubeRack> rows2 = dtSave.Where(w => w.RackType == strType).ToList();
            List<EntityDicSampTubeRack> dtnew2 = rows2;

            gcSave.DataSource = dtnew2;
            this.gcSave.RefreshDataSource();//刷新数据
        }

        private void lueIceBox_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIceBox.EditValue == null)
            {
                List<EntityDicSampStoreArea> dtcupCopy = new List<EntityDicSampStoreArea>();
                lueCupID.Properties.DataSource = dtcupCopy;
                return;
            }
            List<EntityDicSampStoreArea> rows = dtCups.Where(w => w.StoreId == lueIceBox.EditValue.ToString()).ToList();
            List<EntityDicSampStoreArea> dtnew = rows;

            lueCupID.Properties.DataSource = dtnew;
        }
        #endregion

        #region 开始扫描需存储的试管架
        private void txtRackID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string strRackID = txtBarcode.Text;
                if (string.IsNullOrEmpty(strRackID))
                {
                    return;
                }
                if (checkDirectFlag.Checked)
                {

                    if (string.IsNullOrEmpty(lueIceBox.Text))
                    {
                        MessageDialog.ShowAutoCloseDialog("请选择冰箱型号！");
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(lueCupID.Text))
                    {
                        MessageDialog.ShowAutoCloseDialog("请选择冰箱柜子号！");
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        return;
                    }

                    PoxySampStock proxy = new PoxySampStock();
                    List<EntityDicSampTubeRack> table = proxy.Service.GetDictRackInfo(strRackID);

                    if (table == null || table.Count == 0)
                    {
                        MessageDialog.ShowAutoCloseDialog("此试管架条码不存在，请重新输入");
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        return;
                    }
                    string status = table[0].SrStatus.ToString();

                    if (status == "15" || status == "10")
                    {
                        MessageDialog.ShowAutoCloseDialog("该试管架已存储审核");
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        return;
                    }

                    if (status == "20")
                    {
                        MessageDialog.ShowAutoCloseDialog("改试管架已销毁");
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(opId))
                    {
                        FrmCheckPassword checkPassword = new FrmCheckPassword();
                        if (checkPassword.ShowDialog() == DialogResult.OK)
                        {
                            opId = checkPassword.OperatorID;
                            opName = checkPassword.OperatorName;
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (this.layitemTimeCountDown.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                    {
                        this.ctlTimeCountDown1.ReCount();
                    }

                    EntitySampStoreRack entitySamRack = new EntitySampStoreRack();
                    EntityDicSampTubeRack row = table[0];

                    List<EntitySampStoreDetail> dt = proxy.Service.GetRackDetail(row.SrId.ToString());

                    EntitySampStoreDetail entityRackDetail = new EntitySampStoreDetail();// 试管架字典表

                    foreach (var rw in dt)
                    {
                        entityRackDetail.DetId = rw.DetId.ToString();
                        entityRackDetail.DetStatus = 10;
                        entityRackDetail.DetBarCode = rw.DetBarCode.ToString();

                        proxy.Service.ModifySamDetail(entityRackDetail);

                        string remark = string.Format("存储冰箱:{0},存储柜子:{1}", lueIceBox.Text, lueCupID.Text);



                        #region 调用已经写好的bc_sign插入方法

                        EntitySampOperation sampOperation = new EntitySampOperation();
                        sampOperation.OperationTime = ServerDateTime.GetServerDateTime();//获取服务器时间
                        sampOperation.OperationID = opId;
                        sampOperation.OperationName = opName;
                        sampOperation.OperationStatus = "120";
                        sampOperation.OperationPlace = LocalSetting.Current.Setting.Description;
                        sampOperation.Remark = remark;

                        EntitySampMain sampMain = new EntitySampMain();
                        sampMain.SampBarId = entityRackDetail.DetBarCode;
                        sampMain.SampBarCode = entityRackDetail.DetBarCode;

                        //插入bc_sign表行，跟踪记录 20120920
                        ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                        proxyProcess.Service.SaveSampProcessDetail(sampOperation, sampMain);

                        #endregion
                    }

                    entitySamRack.SrId = row.SrId.ToString();
                    entitySamRack.SrStatus = 10;
                    entitySamRack.SrRackId = row.RackId.ToString();
                    entitySamRack.SrPlace = lueCupID.EditValue.ToString();
                    entitySamRack.SrAmount = row.SrAmount;
                    entitySamRack.SrStoreId = lueIceBox.EditValue.ToString();
                    entitySamRack.SrAuditUserId = opId;
                    entitySamRack.SrAuditUserName = opName;
                    entitySamRack.SrAuditDate = DateTime.Now;
                    row.SrStatus = 10;
                    row.SrAuditDate = DateTime.Now;

                    proxy.Service.AuditSamStoreRack(entitySamRack);

                    EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                    entityRack.RackId = entitySamRack.SrRackId;
                    entityRack.RackStatus = entitySamRack.SrStatus;
                    //修改字典中试管架的状态 
                    proxy.Service.ModifyRackStatus(entityRack);

                    if (dtSave == null)
                    {
                        dtSave = new List<EntityDicSampTubeRack>();
                    }
                    dtSave.Add(row);

                    gcSave.RefreshDataSource();

                    if (dtFile != null && dtFile.Count > 0)
                    {
                        List<EntityDicSampTubeRack> rows = dtFile.Where(w => w.RackBarcode == strRackID).ToList();
                        if (rows.Count > 0)
                        {
                            dtFile.Remove(rows[0]);
                            gcFile.RefreshDataSource();
                        }
                    }

                }
                else
                {
                    //试管架已经被存储
                    List<EntityDicSampTubeRack> rows = dtSave.Where(w => w.RackBarcode == strRackID).ToList();
                    if (rows.Count > 0)
                    {
                        MessageDialog.Show("此试管架号已经存储，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                           MessageBoxDefaultButton.Button1);
                        txtBarcode.Text = "";
                        return;
                    }
                    //试管架号码有问题
                    rows = dtFile.Where(w => w.RackBarcode == strRackID).ToList();
                    if (rows.Count < 1)
                    {
                        MessageDialog.Show("此试管架条码不存在，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information,
                                           MessageBoxDefaultButton.Button1);
                        txtBarcode.Text = "";
                        return;
                    }

                    EntityDicSampTubeRack dr = rows[0];
                    dr.IsSelected = true;
                    int index = dtFile.IndexOf(dr);

                    gvFile.FocusedRowHandle = index;
                }

                txtBarcode.Text = string.Empty;
                txtBarcode.Focus();
            }

        }

        #endregion

        #region 将左边的添加到右边
        private void btnAdd_Click(object sender, EventArgs e)
        {
            gvFile.CloseEditor();
            //dtFile.AcceptChanges();
            List<EntityDicSampTubeRack> rows = dtFile.Where(w => w.IsSelected == true).ToList();
            if (rows.Count == 0)
            {
                MessageDialog.Show("请勾选要归档存储的试管架！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            if (string.IsNullOrEmpty(lueIceBox.Text))
            {
                MessageDialog.Show("请选择冰箱型号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            if (string.IsNullOrEmpty(lueCupID.Text))
            {
                MessageDialog.Show("请选择冰箱柜子号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            foreach (var dataRow in rows)
            {
                dataRow.SrStatus = 5;
                dataRow.SrStoreId = lueIceBox.EditValue.ToString();
                dataRow.SrPlace = lueCupID.EditValue.ToString();
                dtSave.Add(dataRow);
                dtFile.Remove(dataRow);
            }

            gcFile.RefreshDataSource();
            gcSave.RefreshDataSource();
        }
        #endregion


        #region 审核
        private void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                gvSave.CloseEditor();
                List<EntityDicSampTubeRack> dtSaveDe = gvSave.DataSource as List<EntityDicSampTubeRack>;
                List<EntityDicSampTubeRack> rows = dtSave.Where(w => w.IsSelected == true && (w.SrStatus == 5 || w.SrStatus == 15)).ToList();
                if (rows.Count == 0)
                {
                    MessageDialog.Show("请勾选[未审核]的试管架！");
                    return;
                }
                if (string.IsNullOrEmpty(opId))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {

                        opId = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }

                string operatorID = opId;
                string operatorName = opName;
                if (this.layitemTimeCountDown.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    this.ctlTimeCountDown1.ReCount();
                }
                //dtFristBarCodeForBatch = DateTime.Now;

                EntitySampStoreRack entitySamRack = new EntitySampStoreRack();

                PoxySampStock proxy = new PoxySampStock();

                foreach (EntityDicSampTubeRack row in rows)
                {
                    List<EntitySampStoreDetail> dt = proxy.Service.GetRackDetail(row.SrId.ToString());
                    EntitySampStoreDetail entityRackDetail = new EntitySampStoreDetail();

                    foreach (EntitySampStoreDetail rw in dt)
                    {
                        entityRackDetail.DetId = rw.DetId.ToString();
                        entityRackDetail.DetStatus = 10;
                        entityRackDetail.DetBarCode = rw.DetBarCode.ToString();

                        proxy.Service.ModifySamDetail(entityRackDetail);

                        string remark = string.Format("存储冰箱:{0},存储柜子:{1}", lueIceBox.Text, lueCupID.Text);

                        #region 调用已经写好的bc_sign插入方法
                        EntitySampOperation sampOperation = new EntitySampOperation();
                        sampOperation.OperationTime = ServerDateTime.GetServerDateTime();//获取服务器时间
                        sampOperation.OperationID = operatorID;
                        sampOperation.OperationName = operatorName;
                        sampOperation.OperationStatus = "120";
                        sampOperation.OperationPlace = LocalSetting.Current.Setting.Description;
                        sampOperation.Remark = remark;

                        EntitySampMain sampMain = new EntitySampMain();
                        sampMain.SampBarId = entityRackDetail.DetBarCode;
                        sampMain.SampBarCode = entityRackDetail.DetBarCode;

                        //插入bc_sign表行，跟踪记录 20120920
                        ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                        proxyProcess.Service.SaveSampProcessDetail(sampOperation, sampMain);

                        //老方法注释掉
                        //proxy.Service.InsertBcSign(operatorName, operatorID, entityRackDetail.DetBarCode, "120",remark, LocalSetting.Current.Setting.Description);
                        #endregion
                    }

                    entitySamRack.SrId = row.SrId.ToString();
                    entitySamRack.SrStatus = 10;
                    entitySamRack.SrRackId = row.RackId.ToString();
                    entitySamRack.SrPlace = row.SrPlace.ToString();
                    entitySamRack.SrAmount = row.SrAmount;
                    entitySamRack.SrStoreId = row.SrStoreId.ToString();
                    entitySamRack.SrAuditUserId = UserInfo.loginID;
                    entitySamRack.SrAuditUserName = UserInfo.userName;
                    entitySamRack.SrAuditDate = DateTime.Now;
                    row.SrStatus = 10;
                    row.SrAuditDate = DateTime.Now;

                    proxy.Service.AuditSamStoreRack(entitySamRack);

                    EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                    entityRack.RackId = entitySamRack.SrRackId;
                    entityRack.RackStatus = entitySamRack.SrStatus;
                    //修改字典中试管架的状态
                    proxy.Service.ModifyRackStatus(entityRack);
                }

                MessageDialog.ShowAutoCloseDialog("审核成功!");
                LoadData(); //重新查询数据,即刷新数据
            }
            catch (Exception ex)
            {
                MessageDialog.Show("异常:" + ex.Message, "信息");
            }
        }

        #endregion

        #region 反审
        /// <summary>
        /// 反审修改架子的状态为‘已归档’
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAntiAudit_Click(object sender, EventArgs e)
        {
            try
            {
                gvSave.CloseEditor();

                List<EntityDicSampTubeRack> rows = dtSave.Where(w => w.IsSelected == true && w.SrStatus == 10).ToList();
                if (rows.Count == 0) //if (rows.Length == 0)
                {
                    MessageDialog.Show("请勾选[已审核]的试管架！");
                    return;
                }
                if (string.IsNullOrEmpty(opId))
                {
                    FrmCheckPassword checkPassword = new FrmCheckPassword();
                    if (checkPassword.ShowDialog() == DialogResult.OK)
                    {
                        opId = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                    else
                    {
                        return;
                    }
                }
                PoxySampStock proxy = new PoxySampStock();
                EntitySampStoreRack entitySamRack = new EntitySampStoreRack();

                foreach (var row in rows)
                {
                    entitySamRack.SrId = row.SrId.ToString();
                    entitySamRack.SrStatus = 5;
                    entitySamRack.SrRackId = row.RackId.ToString();
                    entitySamRack.SrPlace = row.SrPlace.ToString();
                    entitySamRack.SrAmount = row.SrAmount;
                    entitySamRack.SrStoreId = row.SrStoreId.ToString();
                    row.SrStatus = 5;
                    row.SrAuditDate = DateTime.Now; ;// DBNull.Value;

                    proxy.Service.AuditSamStoreRack(entitySamRack);

                    List<EntitySampStoreDetail> dt = proxy.Service.GetRackDetail(row.SrId.ToString());

                    EntitySampStoreDetail entityRackDetail = new EntitySampStoreDetail();

                    foreach (var rw in dt)
                    {
                        entityRackDetail.DetId = rw.DetId.ToString();
                        entityRackDetail.DetStatus = 5;
                        entityRackDetail.DetBarCode = rw.DetBarCode.ToString();

                        proxy.Service.ModifySamDetail(entityRackDetail);
                        proxy.Service.DeleteBcSign(entityRackDetail.DetBarCode, "120");
                    }

                    EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                    entityRack.RackId = entitySamRack.SrRackId;
                    entityRack.RackStatus = entitySamRack.SrStatus;
                    //修改字典中试管架的状态
                    proxy.Service.ModifyRackStatus(entityRack);
                }
                if (this.layitemTimeCountDown.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    this.ctlTimeCountDown1.ReCount();
                }

                MessageDialog.ShowAutoCloseDialog("反审成功!");
                LoadData(); //重新查询数据,即刷新数据
            }
            catch (Exception ex)
            {
                MessageDialog.Show("异常:" + ex.Message, "信息");
            }

        }
        #endregion

        #region 获得选中的试管架内容
        private void gvFile_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                //FocusRowChange();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("异常:" + ex.Message, "信息");
            }
        }
        #endregion

        #region 关闭
        private void btnClosed_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 撤回
        private void btnBack_Click(object sender, EventArgs e)
        {
            gvSave.CloseEditor();
            List<EntityDicSampTubeRack> rows = dtSave.Where(w => w.IsSelected == true).ToList();
            if (rows.Count == 0) //if (rows.Length == 0)
            {
                MessageDialog.Show("请勾选需要撤回的试管架！");
                return;
            }
            PoxySampStock proxy = new PoxySampStock();

            foreach (var row in rows)
            {
                if (row.SrStatus.Equals(10))
                {
                    MessageDialog.Show("请先反审核！反审核之后才能被撤回！");
                    return;
                }
                if (proxy.Service.GetSamRackStatus(row.SrId.ToString()) == 20)
                {
                    MessageDialog.Show("该批次试管架已经销毁,请刷新界面！");
                    return;
                }

                row.SrStatus = 5;

                dtFile.Add(row);
                dtSave.Remove(row);
            }
            gcFile.RefreshDataSource();
            gcSave.RefreshDataSource();
        }
        #endregion

        #region 刷新

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        //全选事件：存放的架子
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            gvSave.CloseEditor();
            if (dtSave == null || dtSave.Count == 0) return;

            foreach (var row in dtSave)
            {
               //// row.IsSelected = chkAudit.Checked ? 1 : 0;
            }
            gcSave.DataSource = dtSave;
            gcSave.RefreshDataSource();
        }

        //全选事件：没有存放的架子
        private void checkUnAudit_CheckedChanged(object sender, EventArgs e)
        {
            gvFile.CloseEditor();
            if (dtFile == null || dtFile.Count == 0) return;

            foreach (var row in dtFile)
            {
               /// row.IsSelected = checkUnAudit.Checked ? 1 : 0;
            }
            gcFile.DataSource = dtFile;
            gcFile.RefreshDataSource();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            opId = string.Empty;
            opName = string.Empty;
            ctlTimeCountDown1.Reset();
            dtSave.Clear();
            this.gcSave.RefreshDataSource(); //刷新控件数据
        }

        private void checkDirectFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDirectFlag.Checked)
            {
                gcSaveJZ.Visible =  false;
                
                layitemAdd.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layitemBack.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layitemTimeCountDown.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //当勾选时显示完成按钮
                sysToolBar.BtnConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                gcSaveJZ.Visible = true;
                layitemAdd.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layitemBack.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                layitemTimeCountDown.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //当不勾选时显示完成按钮
                sysToolBar.BtnConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        private void ctlTimeCountDown1_TimeOut(object sender, EventArgs args)
        {
            if (this.layitemTimeCountDown.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
            {
                opId = string.Empty;
                opName = string.Empty;
                ctlTimeCountDown1.Reset();
                dtSave.Clear();
            }
        }


        #region 为GridControl添加列头勾选框
        /// <summary>
        /// 为GridControl添加列头勾选框
        /// </summary>
        private void AddCheckColumnToGC()
        {
            this.gvFile.Click += new System.EventHandler(this.gvFile_Click);
            this.gvFile.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gvFile_CustomDrawColumnHeader);
            this.gvFile.DataSourceChanged += new EventHandler(gvFile_DataSourceChanged);

            this.gvSave.Click += new System.EventHandler(this.gvSave_Click);
            this.gvSave.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gvSave_CustomDrawColumnHeader);
            this.gvSave.DataSourceChanged += new EventHandler(gvSave_DataSourceChanged);
        }

        #region gvSave
        private void gvSave_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.FieldName == "IsSelected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                GridControlCheckHeader.DrawCheckBox(e, gvSaveStatus);
                e.Handled = true;
            }
        }

        private void gvSave_DataSourceChanged(object sender, EventArgs e)
        {
            GridColumn column = this.gvSave.Columns.ColumnByFieldName("IsSelected");
            if (column != null)
            {
                column.Width = 80;
                column.OptionsColumn.ShowCaption = false;
                column.ColumnEdit = new RepositoryItemCheckEdit();
            }
        }

        private void gvSave_Click(object sender, EventArgs e)
        {
            if (GridControlCheckHeader.ClickGridCheckBox(this.gvSave, "IsSelected", gvSaveStatus))
            {
                gvSaveStatus = !gvSaveStatus;
                gcSave.RefreshDataSource();
            }
        }
        #endregion

        #region gvFile
        private void gvFile_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.FieldName == "IsSelected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                GridControlCheckHeader.DrawCheckBox(e, gvFileStatus);
                e.Handled = true;
            }
        }

        private void gvFile_DataSourceChanged(object sender, EventArgs e)
        {
            GridColumn column = this.gvFile.Columns.ColumnByFieldName("IsSelected");
            if (column != null)
            {
                column.Width = 80;
                column.OptionsColumn.ShowCaption = false;
                column.ColumnEdit = new RepositoryItemCheckEdit();
            }
        }

        private void gvFile_Click(object sender, EventArgs e)
        {
            if (GridControlCheckHeader.ClickGridCheckBox(this.gvFile, "IsSelected", gvFileStatus))
            {
                gvFileStatus = !gvFileStatus;
                gcFile.RefreshDataSource();
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
