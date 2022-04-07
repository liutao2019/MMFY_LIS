using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;
using DevExpress.XtraEditors;
using dcl.client.report;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmParameter : ConCommon
    {
        public FrmParameter()
        {
            InitializeComponent();
        }

        //全局变量，用来过滤仪器用的
        string userTypes = "";

        //过滤物理组用
        string userQcTypes = string.Empty;

        List<EntityDicInstrument> listInstrmt = new List<EntityDicInstrument>();//存放仪器过滤之后的数据
        List<EntityDicPubProfession> listLueType = new List<EntityDicPubProfession>();//存放实验组过滤之后的数据

        bool isBinding = true;

        //项目可否新增标志
        bool isCanAddList = false;

        //全局变量 用于调用各种操作数据的方法
        PoxyFrmParameter proxy = new PoxyFrmParameter();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmParameterG_Load(object sender, EventArgs e)
        {
            #region 仪器权限过滤
            //设置仪器权限
            //foreach (EntityUserItrQuality itr in UserInfo.entityUserInfo.UserItrsQc)
            //{
            //    userTypes += "'" + itr.ItrId + "',";
            //}
            //if (userTypes != "")
            //{
            //    userTypes = userTypes.TrimEnd(',');
            //}
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserItrsQc.Count > 0)
                {
                    listInstrmt = this.lue_Instrmt.getDataSource().FindAll(w => UserInfo.entityUserInfo.UserItrsQc.FindIndex(i => i.ItrId == w.ItrId) > -1);
                    this.lue_Instrmt.SetFilter(listInstrmt);
                }
                else
                {
                    this.lue_Instrmt.SetFilter(new List<EntityDicInstrument>());
                }
            }
            else
            {
                listInstrmt = this.lue_Instrmt.getDataSource();
                this.lue_Instrmt.SetFilter(listInstrmt);
            }
            #endregion

            #region 实验组权限过滤
            //foreach (EntityUserLabQuality Type in UserInfo.entityUserInfo.UserQcLab)
            //    userQcTypes += ",'" + Type.LabId + "'";
            //if (userQcTypes.Length > 0)
            //{
            //    userQcTypes = userQcTypes.Remove(0, 1);
            //}
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserQcLab.Count > 0)
                {
                    listLueType = this.lueType.getDataSource().FindAll(w => UserInfo.entityUserInfo.UserQcLab.FindIndex(i => i.LabId == w.ProId) > -1);
                    lueType.SetFilter(listLueType);
                }
                else
                    lueType.SetFilter(new List<EntityDicPubProfession>());
            }
            else
            {
                listLueType = this.lueType.getDataSource();
                lueType.SetFilter(listLueType);
            }
            #endregion

            #region 启用时间初始化
            rpscStartTime.Items = new List<dcl.client.control.RoundPanelStatusItem>
            {
                new dcl.client.control.RoundPanelStatusItem { Caption="近3个月",Value="1" },
                new dcl.client.control.RoundPanelStatusItem { Caption="全部",Value="0" }
            };
            rpscStartTime.SetValue("0");
            rpscStartTime.RoundPanelGroupClick += RpscStartTime_RoundPanelGroupClick;
            #endregion

            sysQcItem.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", sysQcItem.BtnQuickEntry.Name, sysQcItem.BtnExport.Name });
            sysLot.SetToolButtonStyle(new string[] { "BtnAdd", "BtnDelete", "BtnSave", "BtnCancel", sysLot.BtnModify.Name, sysLot.BtnCopy.Name, sysLot.BtnPrintList.Name, sysLot.BtnClose.Name, sysLot.BtnRefresh.Name }); //添加刷新按钮
            sysLot.BtnPrintList.Caption = "打印";
            sysLot.BtnCopy.Caption = "复制";
            sysLot.OnBtnPrintListClicked += sysLot_OnBtnPrintClicked;
            sysLot.BtnClose.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            List<EntityDicQcRule> Items = proxy.Service.SearchQcRule();
            System.Object[] ItemObject = new System.Object[Items.Count];
            cklRules.DataSource = Items;
            cklRules.ValueMember = "RulId";
            cklRules.DisplayMember = "RulName";

            SetPnlReadOnly(true, pnlLOT);
            SetPnlReadOnly(true, pnlItem);
            Instrmt_Enabled(true);
            Item_Enabled(true);
            if (UserInfo.GetSysConfigValue("QC_RunTheStartTime") == "是")
            {
                layitemdtSdate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            pnlItem.Enabled = false;
        }

        /// <summary>
        /// 控件启用公用类
        /// </summary>
        /// <param name="read"></param>
        /// <param name="pnlLOT"></param>
        private void SetPnlReadOnly(bool read, PanelControl pnlLOT)
        {
            foreach (Control c in pnlLOT.Controls)
            {
                if (c is TextEdit)
                    ((TextEdit)c).Properties.ReadOnly = read;

                if (c is HopePopSelect)
                    ((HopePopSelect)c).Readonly = read;

                if (c is CheckEdit)
                    ((CheckEdit)c).Properties.ReadOnly = read;


                if (c is CheckedListBoxControl)
                {
                    CheckedListBoxControl check = (CheckedListBoxControl)c;
                    if (read)
                    {
                        check.SelectionMode = SelectionMode.None;
                    }
                    else
                    {
                        check.SelectionMode = SelectionMode.One;
                    }
                }
            }
        }

        private void lue_Instrmt_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //选择仪器后如果物理组为空则填充当前仪器的物理组 2018-01-31 SJC
            if (string.IsNullOrEmpty(this.lueType.valueMember))
            {
                //获取当前仪器的物理组ID
                string ctype_id = string.Empty;
                List<EntityDicInstrument> listItr = CacheClient.GetCache<EntityDicInstrument>().Where(w => w.ItrId == this.lue_Instrmt.valueMember).ToList();
                if (listItr.Count > 0)
                    ctype_id = listItr[0].ItrLabId.ToString();

                if (!string.IsNullOrEmpty(ctype_id))
                {
                    //根据物理组ID获取物理组名称
                    EntityDicPubProfession rowCType = null;
                    List<EntityDicPubProfession> listPro = CacheClient.GetCache<EntityDicPubProfession>().Where(w => w.ProId.ToString() == ctype_id).ToList();
                    if (listPro.Count > 0)
                        rowCType = listPro[0];

                    if (rowCType != null)
                    {
                        this.lueType.valueMember = ctype_id;
                        this.lueType.displayMember = rowCType.ProName;

                        lueType_ValueChanged(null, null);
                    }
                }
            }

            bindLot();
            gv_lot_FocusedRowChanged(null, null);
        }

        /// <summary>
        /// 绑定质控物
        /// </summary>
        private void bindLot()
        {
            string mid;
            string startDate = null;

            if (this.lue_Instrmt.valueMember != null && lue_Instrmt.valueMember != "")
            {
                mid = this.lue_Instrmt.valueMember.ToString();
            }
            else
                mid = "0";

            if (rpscStartTime.GetCurRoundPanel().Tag?.ToString() == "1")
            {
                startDate = DateTime.Now.AddMonths(-3).AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (mid != "")
            {
                ProxyQcMateria proxyQcMateria = new ProxyQcMateria();
                //查询质控物明细数据
                List<EntityDicQcMateria> Items = proxyQcMateria.Service.SearchQcMateriaByMatId(mid, startDate); //对应SearchDetailNew(mid,startDate) 
                List<EntityDicMachineCode> mitmNoList = proxy.Service.SearchMitmNo(lue_Instrmt.valueMember);//对应 SearchMitmNo(itrId) 

                if (mid != "0" && mitmNoList.Count > 0)
                {
                    StringBuilder sbMtId = new StringBuilder();
                    foreach (var drMitm in mitmNoList)
                    {
                        sbMtId.Append(string.Format(",'{0}'", drMitm.MacItmId));
                    }
                    sbMtId.Remove(0, 1);

                    //根据仪器所含的专业组过滤项目，并过滤掉停用的项目
                    lue_Item.SetFilter(lue_Item.getDataSource().FindAll(w => sbMtId.ToString().Contains(w.ItmId) && w.ItmDelFlag == "0"));
                }
                else
                {
                    lue_Item.SetFilter(lue_Item.getDataSource().FindAll(w => w.ItmId == "-1" && w.ItmDelFlag == "0"));
                }

                this.bdqcParDetail.DataSource = Items;
                gv_lot.ExpandAllGroups();
                if (Items.Count == 0) //if (Items.Rows.Count == 0)
                    clearCklRulesCheck();//清空质控规则
            }
        }

        /// <summary>
        /// 绑定项目
        /// </summary>
        private void bindQcItem()
        {
            string mid;

            if (bdqcParDetail.Current != null)
            {
                EntityDicQcMateria dr = bdqcParDetail.Current as EntityDicQcMateria;
                mid = dr.MatSn;
            }
            else
            {
                mid = "0";
            }
            if (mid != "")
            {
                ProxyQcMateriaDetail proxyQMDetail = new ProxyQcMateriaDetail();
                //查询质控项目数据
                List<EntityDicQcMateriaDetail> Items = proxyQMDetail.Service.SearchQcMateriaDetail(mid);//更改为SearchQcMateriaDetail
                this.bdqcpar.DataSource = Items;
            }

            gv_QcItem_FocusedRowChanged(null, null);
        }

        /// <summary>
        /// 按钮启用事件
        /// </summary>
        /// <param name="isTrue"></param>
        private void Instrmt_Enabled(bool isTrue)
        {
            sysLot.BtnAdd.Enabled = isTrue;
            sysLot.BtnModify.Enabled = isTrue;
            sysLot.BtnDelete.Enabled = isTrue;
            sysLot.BtnSave.Enabled = !isTrue;
            sysLot.BtnCancel.Enabled = !isTrue;
            gc_lot.Enabled = isTrue;
            lue_Instrmt.Readonly = !isTrue;
            lueType.Readonly = !isTrue;
        }

        /// <summary>
        /// 质控物新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnAddClicked(object sender, EventArgs e)
        {
            if (this.lue_Instrmt.valueMember != null && lue_Instrmt.valueMember.ToString() != "")
            {
                cmbType.Focus();
                EntityDicQcMateria drQcPar = (EntityDicQcMateria)this.bdqcParDetail.AddNew();
                drQcPar.MatSn = String.Empty;
                drQcPar.MatStaus = 1;//默认为启用状态

                ceState.Checked = true;
                SetPnlReadOnly(false, pnlLOT);
                Instrmt_Enabled(false);
                dtEdate.EditValue = drQcPar.MatDateEnd = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day).Date;
                dtSdate.EditValue = drQcPar.MatDateStart = DateTime.Now.Date;

                //项目清空数据
                //this.bdqcpar.DataSource = new List<EntityDicQcItem>();这个会报错
                //this.gc_QcItem.DataSource = null;

                gc_QcItem.Enabled = false;//质控项目新增时，质控项目不能编辑(新增)
                isCanAddList = true;//项目是否可以新增
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要添加的仪器", "提示");
                return;
            }
        }

        /// <summary>
        /// 质控物放弃事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnCancelClicked(object sender, EventArgs e)
        {
            Instrmt_Enabled(true);
            SetPnlReadOnly(true, pnlLOT);
            bdqcParDetail.EndEdit();
            bindLot();
            gv_lot_FocusedRowChanged(null, null);

            gc_QcItem.Enabled = true;//新增
            isCanAddList = false;//项目可否新增标志
        }

        /// <summary>
        /// 质控物改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_lot_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bdqcParDetail.Current != null)
            {
                EntityDicQcMateria drLot = (EntityDicQcMateria)bdqcParDetail.Current;
                string mid = drLot.MatSn;

                if (mid != null && mid != "")
                {
                    bindQcItem();
                }
            }
            else
            {
                bindQcItem();
            }
        }

        /// <summary>
        /// 质控物修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (bdqcParDetail.Current != null)
            {
                cmbType.Focus();
                SetPnlReadOnly(false, pnlLOT);
                Instrmt_Enabled(false);
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要修改的质控物", "提示");
                return;
            }
        }

        /// <summary>
        /// 质控物删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (bdqcParDetail.Current == null)
            {
                lis.client.control.MessageDialog.Show("未选择需删除的数据", "提示");
                return;
            }
            bdqcParDetail.EndEdit();
            bdqcpar.EndEdit();
            //得到删除的数据
            EntityDicQcMateria dr = (EntityDicQcMateria)bdqcParDetail.Current;

            //得到质控物数据
            List<EntityDicQcMateria> dtQc_Detail = bdqcParDetail.DataSource as List<EntityDicQcMateria>;

            if (dr.MatSn == "")
            {
                dtQc_Detail.Remove(dr);
            }
            else
            {
                DialogResult dresult = MessageBox.Show("确定要删除该质控物并删除下面所有的项目吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        base.isActionSuccess = proxy.Service.DeleteParDetailNew(dr);
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            if (base.isActionSuccess)
            {
                if (dr.MatSn != "")
                {
                    dtQc_Detail.Remove(dr);
                }
                this.gc_lot.RefreshDataSource();
            }
        }

        /// <summary>
        /// 质控物保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.sysLot.Focus();
            bdqcParDetail.EndEdit();
            if (this.cmbType.EditValue.ToString().Trim() == "")
            {
                cmbType.Focus();
                lis.client.control.MessageDialog.Show("质控物水平不能为空！", "提示");
                return;
            }

            if (this.txtLot.Text.ToString().Trim() == "")
            {
                txtLot.Focus();
                lis.client.control.MessageDialog.Show("质控物批号不能为空！", "提示");
                return;
            }

            if (this.dtEdate.EditValue.ToString() == "")
            {
                dtEdate.Focus();
                lis.client.control.MessageDialog.Show("有效日期时间不能为空！", "提示");
                return;
            }

            List<EntityDicQcMateria> dtQc_Detail = bdqcParDetail.DataSource as List<EntityDicQcMateria>;

            EntityDicQcMateria dr = (EntityDicQcMateria)bdqcParDetail.Current;
            dr.MatId = this.lue_Instrmt.valueMember;

            #region 之前就已经注释的代码
            //DataTable dtMark = dsJudge.Tables["qcMark"];
            //if (dtMark.Rows.Count > 0)//查找该改质控物下面是否存在相同表示
            //{
            //    lis.client.control.MessageDialog.Show("该批号下已存在此质控物水平！", "提示");
            //    cmbType.Focus();
            //    return;
            //}
            #endregion

            ProxyQcMateria proxyQcMateria = new ProxyQcMateria();
            List<EntityDicQcMateria> dtDetai = proxyQcMateria.Service.SearchMatSnInQcMateria(dr);

            //查找该时间范围内是否存在该质控物
            if (dtDetai.Count > 0)
            {
                lis.client.control.MessageDialog.Show("该仪器已存在同批号同水平的质控物！", "提示");
                txtLot.Focus();
                return;
            }

            EntityResponse result = new EntityResponse();
            bool isNew = true;
            if (dr.MatSn == "")
            {
                //保存质控物明细数据
                result = proxyQcMateria.Service.SaveQcMateria(dr);
                base.isActionSuccess = result.Scusess;
                isNew = true;
            }
            else
            {
                dr.MatCNo = null;
                base.isActionSuccess = proxyQcMateria.Service.UpdateQcMateria(dr);

                isNew = false;
            }
            if (base.isActionSuccess)
            {
                if (dr.MatSn == "")
                {
                    dr.MatSn = result.GetResult<EntityDicQcMateria>().MatSn;
                }
                string matId = this.lue_Instrmt.valueMember;
                string matLevel = this.cmbType.Text;

                List<EntityDicQcMateria> dsResult = new List<EntityDicQcMateria>();

                if (isNew)
                {
                    if (MessageDialog.Show("保存成功，是否打开质控通道？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //查询该仪器时间范围历史质控批号
                        dsResult = proxyQcMateria.Service.SearchQcMateriaLeftRuleTimeAndInterface(matId, matLevel);

                        FrmQcParameterRuleInstNew frmPRI = new FrmQcParameterRuleInstNew(dsResult);
                        frmPRI = new FrmQcParameterRuleInstNew(dsResult);
                        frmPRI.ShowDialog();
                    }
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("修改成功!");
                    if (MessageDialog.Show("当前质控批号已修改，是否修改质控通道？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        dsResult = proxyQcMateria.Service.SearchQcMateriaLeftRuleTimeAndInterface(matId, matLevel);

                        FrmQcParameterRuleInstNew frmPRI = new FrmQcParameterRuleInstNew(dsResult);
                        frmPRI.ShowDialog();
                    }
                }
            }

            bindQcItem();//相当于刷新数据

            SetPnlReadOnly(true, pnlLOT);
            Instrmt_Enabled(true);
            this.gc_lot.Focus();

            gc_QcItem.Enabled = true;//
            isCanAddList = false;//项目可否新增标志
        }

        /// <summary>
        /// 质控物刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            bindLot();
            gv_lot_FocusedRowChanged(null, null);
        }

        /// <summary>
        /// 项目新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_OnBtnAddClicked(object sender, EventArgs e)
        {
            if (isCanAddList)
            {
                MessageBox.Show("质控物新增时，项目不能新增！");
                return;
            }

            if (bdqcParDetail.Current != null)
            {
                lue_Item.Focus();
                //this.bdqcpar.AddNew();
                EntityDicQcMateriaDetail materiaDet = (EntityDicQcMateriaDetail)this.bdqcpar.AddNew();
                materiaDet.MatDetId = string.Empty;

                //deReag_date.EditValue = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day).Date;
                SetPnlReadOnly(false, pnlItem);
                Item_Enabled(false);
                isBinding = false;
                pnlItem.Enabled = true;
                cmbValueType.SelectedIndex = 0;
                cmbQcrCRule.SelectedIndex = 0;
                //清空质控规则
                clearCklRulesCheck();
                this.bdqcpar.EndEdit();
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要添加的质控物", "提示");
                return;
            }
        }

        /// <summary>
        /// 项目按钮启用事件
        /// </summary>
        /// <param name="isTrue"></param>
        private void Item_Enabled(bool isTrue)
        {
            sysQcItem.BtnAdd.Enabled = isTrue;
            sysQcItem.BtnModify.Enabled = isTrue;
            sysQcItem.BtnDelete.Enabled = isTrue;
            sysQcItem.BtnSave.Enabled = !isTrue;
            sysQcItem.BtnCancel.Enabled = !isTrue;
            gc_lot.Enabled = isTrue;
            gc_QcItem.Enabled = isTrue;
            lue_Instrmt.Readonly = !isTrue;
            sysQcItem.BtnQuickEntry.Enabled = isTrue;
        }

        /// <summary>
        /// 项目放弃事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_OnBtnCancelClicked(object sender, EventArgs e)
        {
            isBinding = true;
            pnlItem.Enabled = false;
            Item_Enabled(true);
            SetPnlReadOnly(true, pnlItem);
            bdqcpar.EndEdit();
            bindQcItem();
            this.gc_QcItem.Focus();

            //bdqcParDetail.EndEdit();
            //bindLot();
            //gv_lot_FocusedRowChanged(null, null);
        }

        int isClear = 0;

        /// <summary>
        /// 项目改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_QcItem_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (isClear == 0)
            {
                if (bdqcpar.Current != null)
                {
                    lue_Item.Focus();

                    //DataRowView drPar = (DataRowView)bdqcpar.Current;
                    //DataRowView drDetail = (DataRowView)bdqcParDetail.Current;
                    //string strItmEcd = drPar["qcr_itm_ecd"].ToString();
                    EntityDicQcMateriaDetail drPar = (EntityDicQcMateriaDetail)bdqcpar.Current;
                    EntityDicQcMateria drDetail = (EntityDicQcMateria)bdqcParDetail.Current;
                    string strItmEcd = drPar.MatItmId;

                    if (strItmEcd != "")
                    {
                        //清空选择
                        for (int j = 0; j < this.cklRules.ItemCount; j++)
                        {
                            cklRules.SetItemChecked(j, false);
                        }


                        //查询质控规则关联数据
                        ProxyQcMateriaRule proxyQMRule = new ProxyQcMateriaRule();
                        List<EntityDicQcMateriaRule> qc_sample = proxyQMRule.Service.SearchQcMateriaRule(drDetail.MatSn, drPar.MatItmId);

                        foreach (var sarow in qc_sample)
                        {
                            for (int i = 0; i < this.cklRules.ItemCount; i++)
                            {
                                EntityDicQcRule dr = (EntityDicQcRule)cklRules.GetItem(i);
                                int itm = Convert.ToInt32(sarow.RulId);
                                int clb = Convert.ToInt32(dr.RulId);
                                if (clb == itm)
                                {
                                    cklRules.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                }
                else
                    clearCklRulesCheck();
            }
        }

        private void clearCklRulesCheck()
        {
            for (int j = 0; j < this.cklRules.ItemCount; j++)
            {
                cklRules.SetItemChecked(j, false);
            }
        }

        /// <summary>
        /// 项目修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (bdqcpar.Current != null)
            {
                SetPnlReadOnly(false, pnlItem);
                Item_Enabled(false);
                isBinding = false;
                pnlItem.Enabled = true;
                isCanAddList = true;//项目可否新增标志
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要修改的项目", "提示");
                return;
            }
        }

        /// <summary>
        /// 项目删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (bdqcpar.Current != null)
            {
                EntityDicQcMateriaDetail dr = (EntityDicQcMateriaDetail)bdqcpar.Current;
                string qcr_id = dr.MatDetId;
                List<EntityDicQcMateriaDetail> dtPar = bdqcpar.DataSource as List<EntityDicQcMateriaDetail>;

                EntityDicQcMateria drParDeta = (EntityDicQcMateria)bdqcParDetail.Current;

                if (qcr_id == "")
                {
                    dtPar.Remove(dr);
                }
                else
                {
                    DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    switch (dresult)
                    {
                        case DialogResult.OK:
                            base.isActionSuccess = proxy.Service.DeleteParNewRule(dr, drParDeta);
                            break;
                        case DialogResult.Cancel:
                            return;

                    }

                }
                if (base.isActionSuccess)
                {
                    if (qcr_id != "")
                    {
                        dtPar.Remove(dr);
                    }
                }

                gc_QcItem.RefreshDataSource(); //控件刷新
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要删除的项目", "提示");
                return;
            }
        }

        /// <summary>
        /// 项目保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysQcItem.Focus();
            isClear = 1;
            bdqcpar.EndEdit();
            isClear = 0;

            #region 验证输入值是否正确

            List<EntityDicQcMateriaDetail> itemSourse = bdqcpar.DataSource as List<EntityDicQcMateriaDetail>;

            if (this.lue_Item.valueMember == null || (this.lue_Item.valueMember != null && this.lue_Item.valueMember.ToString() == ""))
            {
                lis.client.control.MessageDialog.Show("项目不能为空！", "提示");
                lue_Item.Focus();
                return;
            }

            if (itemSourse.Where(w => w.MatItmId == lue_Item.valueMember).ToList().Count > 1)
            {
                lis.client.control.MessageDialog.Show("该批号下已存在此项目，请重新输入！", "提示");
                lue_Item.Focus();
                return;
            }

            bool isEstimateSDandTarget = (cmbValueType.Text.Trim() == "数值" && cmbQcrCRule.Text.Trim() == "普通");

            if (isEstimateSDandTarget && (txtTarget.EditValue == null || txtTarget.EditValue.ToString().Trim() == ""))
            {
                lis.client.control.MessageDialog.Show("靶值不能为空！", "提示");
                txtTarget.Focus();
                return;
            }
            if (isEstimateSDandTarget && (txtSd.EditValue == null || txtSd.EditValue.ToString().Trim() == ""))
            {
                lis.client.control.MessageDialog.Show("标准差不能为空！", "提示");
                txtSd.Focus();
                return;
            }

            if (cmbValueType.Text.Trim() == "半定量" && txtMaxValue.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请输入最大值！", "提示");
                txtMaxValue.Focus();
                return;
            }
            if (cmbValueType.Text.Trim() == "半定量" && txtMinValue.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请输入最小值！", "提示");
                txtMinValue.Focus();
                return;
            }

            List<EntityDicQcMateriaDetail> dtPar = bdqcpar.DataSource as List<EntityDicQcMateriaDetail>;
            int itemCount = dtPar.Where(w => w.MatItmId == lue_Item.valueMember).ToList().Count;

            #endregion

            EntityDicQcMateriaDetail drv = (EntityDicQcMateriaDetail)bdqcpar.Current;
            drv.MatItrId = this.lue_Instrmt.valueMember;

            EntityDicQcMateria drvLot = (EntityDicQcMateria)bdqcParDetail.Current;
            drv.MatId = drvLot.MatSn;
            drv.MatReadValidDate = drvLot.MatDateEnd;//将质控的有效日期赋值给试剂有效日期
            drv.MatReagBatchno = drvLot.MatBatchNo;

            #region 质控规则保存

            List<EntityDicQcMateriaRule> dtSample = new List<EntityDicQcMateriaRule>();

            for (int i = 0; i < cklRules.CheckedItems.Count; i++)
            {
                string drCk = cklRules.CheckedItems[i].ToString();
                EntityDicQcMateriaRule info = new EntityDicQcMateriaRule();
                info.MatSn = drvLot.MatSn;
                info.RulId = drCk;
                info.MatItmId = drv.MatItmId;
                dtSample.Add(info);
            }

            if (dtSample.Count <= 0)
            {
                EntityDicQcMateriaRule infoRule = new EntityDicQcMateriaRule();
                infoRule.MatSn = drvLot.MatSn;
                infoRule.RulId = "0";
                infoRule.MatItmId = drv.MatItmId;
                dtSample.Add(infoRule);
            }

            #endregion

            EntityResponse result = new EntityResponse();
            string id = drv.MatDetId;
            ProxyQcMateriaDetail proxyQMDetail = new ProxyQcMateriaDetail();
            if (id == "")
            {
                result = proxyQMDetail.Service.SaveQcMateriaDetail(drv);
                base.isActionSuccess = result.Scusess;
            }
            else
            {
                base.isActionSuccess = proxyQMDetail.Service.UpdateQcMateriaDetail(drv);
            }
            if (base.isActionSuccess)
            {
                if (id == "")
                {
                    drv.MatDetId = result.GetResult<EntityDicQcMateriaDetail>().MatDetId;
                }

                proxy.Service.ViewQcParRule(dtSample);//保存选中的质控规则信息
            }

            bindQcItem();//相当于刷新数据

            Item_Enabled(true);
            SetPnlReadOnly(true, pnlItem);
            this.gv_QcItem.Focus();
            isBinding = true;
            pnlItem.Enabled = false;
        }

        /// <summary>
        /// 质控规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_OnBtnQualityRuleClicked(object sender, EventArgs e)
        {
            //FrmCriterion fc = new FrmCriterion();
            //fc.ShowDialog();
        }

        /// <summary>
        /// 质控物复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysLot_BtnCopyClick(object sender, EventArgs e)
        {
            FrmQcCopyNew fqc = new FrmQcCopyNew(this.lue_Instrmt.valueMember, this.lue_Instrmt.displayMember);
            fqc.clikcA += new FrmQcCopyNew.ClikeHander(fqc_clikcA);
            fqc.ShowDialog();
        }

        void fqc_clikcA(ClickEventArgs e)
        {
            if (e.State == 1)
            {
                lue_Instrmt_ValueChanged(null, null);
            }
        }

        /// <summary>
        /// 质控项目快速录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysQcItem_BtnQuickEntryClick(object sender, EventArgs e)
        {
            if (bdqcParDetail.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择质控物！", "提示");
                return;
            }

            EntityDicQcMateria drLot = (EntityDicQcMateria)bdqcParDetail.Current;
            string strDetail = drLot.MatSn;
            string strIns = drLot.MatId;

            List<EntityDicQcMateriaDetail> dtPar = bdqcpar.DataSource as List<EntityDicQcMateriaDetail>;

            string strWhere = "";
            FrmAddQcItemNew faqi = new FrmAddQcItemNew(strWhere, strDetail, strIns);
            faqi.listMateriaDetail = dtPar;//给全局变量赋值
            faqi.checkQcItem += new FrmAddQcItemNew.ClikeHander(faqi_checkQcItem);
            faqi.ShowDialog();
        }

        void faqi_checkQcItem(ClickEventArgs e)
        {
            if (e.State == 1)
            {
                gv_lot_FocusedRowChanged(null, null);
            }
        }

        /// <summary>
        /// 项目下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void lue_Item_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            EntityDicQcMateriaDetail dr = (EntityDicQcMateriaDetail)bdqcpar.Current;

            if (dr != null)
            {
                if (dr.MatDetId != null && dr.MatDetId.ToString().Trim() != "")
                {

                }
                else
                {
                    if (lue_Item.valueMember != null && lue_Item.valueMember != "")
                    {
                        EntityDicItmItem drItem = lue_Item.dtSource.Where(w => w.ItmId == lue_Item.valueMember).ToList()[0];
                    }
                }
            }
        }

        //根据实验组来过滤仪器数据  新增方法
        private void lueType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (lueType.valueMember != null && lueType.valueMember != "")
            {
                if (listInstrmt.Count > 0)
                {
                    this.lue_Instrmt.SetFilter(listInstrmt.FindAll(w => w.ItrLabId == lueType.valueMember));
                }
                else
                {
                    this.lue_Instrmt.SetFilter(new List<EntityDicInstrument>());
                }
            }
            else
            {
                if (listInstrmt.Count > 0)
                    this.lue_Instrmt.SetFilter(listInstrmt.FindAll(w => !string.IsNullOrEmpty(w.ItrLabId)));
                else
                    this.lue_Instrmt.SetFilter(new List<EntityDicInstrument>());
            }
        }

        /// <summary>
        /// 标准差->CV%，靶值->CV%
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCV_EditValueChanged(object sender, EventArgs e)
        {
            if (!isBinding)
            {
                if (txtCV.EditValue != null && txtSd.EditValue != null)//&& txtCV.Text.Trim() == string.Empty
                {
                    try
                    {
                        txtCV.EditValueChanged -= new EventHandler(txtSD_EditValueChanged);
                        txtCV.Text = ((Convert.ToDouble(txtSd.Text.Trim()) / Convert.ToDouble(txtTarget.Text.Trim())) * 100).ToString("0.000");
                        txtCV.EditValueChanged += new EventHandler(txtSD_EditValueChanged);

                        this.bdqcpar.EndEdit();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// CV%->标准差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSD_EditValueChanged(object sender, EventArgs e)
        {
            if (!isBinding)
            {
                if (txtTarget.EditValue != null && txtCV.EditValue != null)//&& txtSd.Text.Trim() == string.Empty
                {
                    try
                    {
                        txtSd.EditValueChanged -= new EventHandler(txtCV_EditValueChanged);
                        txtSd.Text = ((Convert.ToDouble(txtCV.Text.Trim()) * Convert.ToDouble(txtTarget.Text.Trim())) / 100).ToString("0.000");
                        txtSd.EditValueChanged += new EventHandler(txtCV_EditValueChanged);

                        this.bdqcpar.EndEdit();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        private void cklRules_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Checked)
            {
                int checkIndex = e.Index;
                string drCk = cklRules.GetItemText(checkIndex).ToString();
                if (drCk == "Westgard")
                {
                    cklRules.ItemCheck -= new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cklRules_ItemCheck);
                    clearCklRulesCheck();
                    cklRules.SetItemChecked(checkIndex, true);
                    cklRules.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cklRules_ItemCheck);
                }
                else
                {
                    for (int j = 0; j < this.cklRules.ItemCount; j++)
                    {
                        if (cklRules.GetItemText(j) == "Westgard")
                        {
                            cklRules.SetItemChecked(j, false);
                        }
                    }
                }
            }
        }

        private void cmbValueType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbValueType.Text == "数值")
                setLabelVasible(true);
            else
                setLabelVasible(false);
        }

        private void setLabelVasible(bool isVasible)
        {
            pnlItem.Visible = isVasible;
            if (isVasible)
            {
                layitemSD.Text = "标准差";
                layitemSD.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;

                layitemMax.Text = "最大值";
                layitemMax.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;

                layitemMin.Text = "最小值";
                layitemMin.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                layitemSD.Text = "*标准差";
                layitemSD.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;

                layitemMax.Text = "*最大值";
                layitemMax.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;

                layitemMin.Text = "*最小值";
                layitemMin.AppearanceItemCaption.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void RpscStartTime_RoundPanelGroupClick(object sender, EventArgs e)
        {
            bindLot();
            gv_lot_FocusedRowChanged(null, null);
        }



        private void sysLot_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (bdqcParDetail.Current != null)
            {
                string prtTemplate = UserInfo.GetSysConfigValue("QCParamReport");
                if (string.IsNullOrEmpty(prtTemplate))
                {
                    MessageDialog.Show("请到[系统参数]--[质控]--[质控参数报表]设置报表代码", "提示");
                    return;
                }
                EntityDicQcMateria drLot = (EntityDicQcMateria)bdqcParDetail.Current;
                string mid = drLot.MatSn;

                if (mid != "")
                {
                    try
                    {
                        #region 报表打印 旧代码
                        //List<EntityPrintData> listPrintData = new List<EntityPrintData>();
                        //string sqlWhere = string.Format(" and qc_par_detail_new.qcr_key = '{0}'", mid);

                        //EntityPrintData printData = new EntityPrintData();

                        //printData.AddParam("&where&", sqlWhere);
                        //printData.ReportCode = prtTemplate;
                        //listPrintData.Add(printData);

                        //FrmReportPrint pForm = new FrmReportPrint();

                        //pForm.Print3(listPrintData);
                        #endregion

                        #region 报表打印 新代码
                        EntityDCLPrintParameter paramter = new EntityDCLPrintParameter();
                        paramter.ReportCode = prtTemplate;

                        Dictionary<String, Object> keyValue = new Dictionary<String, Object>();
                        keyValue.Add("ParameReport", mid);
                        paramter.CustomParameter = keyValue;

                        DCLReportPrint.Print(paramter);
                        #endregion

                    }
                    catch (ReportNotFoundException ex1)
                    {
                        lis.client.control.MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        Lib.LogManager.Logger.LogException(ex2);
                    }
                }
            }
        }

        private void sysQcItem_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gc_QcItem.DataSource != null)
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
                        gc_QcItem.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "导出异常提醒");
                    }
                }

            }
        }

        /// <summary>
        /// 实验组:显示值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void lueType_DisplayTextChanged(object sender, control.ValueChangeEventArgs args)
        {
            lueType_ValueChanged(null, null);
        }
    }
}
