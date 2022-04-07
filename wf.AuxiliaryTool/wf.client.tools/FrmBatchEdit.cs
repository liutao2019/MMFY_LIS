using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common;
using lis.client.control;
using dcl.client.sample;
using dcl.client.wcf;
using dcl.client.common;
using dcl.client.result;
using dcl.entity;
using dcl.client.cache;


namespace dcl.client.tools
{
    public partial class FrmBatchEdit : FrmCommon
    {
        List<EntityDicCombine> listComb = new List<EntityDicCombine>();
        FrmCombineManager f;

        /// <summary>
        /// 数据源查询时条件记录--用于日记记录
        /// </summary>
        private string SourceLogMsg { get; set; }

        /// <summary>
        /// 目标修改条件记录--用于日记记录
        /// </summary>
        private string TargetLogMsg { get; set; }

        #region 初始化
        public FrmBatchEdit()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnSingleAuditClicked += new System.EventHandler(this.sysToolBar1_OnBtnSingleAuditClicked);
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);

            //***********************************************************
            //仪器过滤为当前登录用户有权的仪器
            string filter = string.Empty;
            if (UserInfo.UserItrs != null)
            {
                for (int i = 0; i < UserInfo.UserItrs.Length - 1; i++)
                {
                    filter += "'" + UserInfo.UserItrs[i] + "',";
                }
                filter += "'" + UserInfo.UserItrs[UserInfo.UserItrs.Length - 1] + "'";
                fPat_itr_id.SelectFilter = string.Format("itr_id in ({0})", filter);
                gPat_itr_id.SelectFilter = string.Format("itr_id in ({0})", filter);
                //***********************************************************
            }
            Initialize();
        }

        public void Initialize()
        {
            dateReportSource.EditValue = DateTime.Now;
            dateReportTarget.EditValue = DateTime.Now;
        }

        /// <summary>
        /// 初始化按钮
        /// </summary>
        private void InitButtons()
        {
            sysToolBar1.BtnSearch.Caption = "源数据";
            sysToolBar1.BtnSingleAudit.Caption = "目标数据";
            sysToolBar1.SetToolButtonStyle(
                new string[] {
                    sysToolBar1.BtnSearch.Name
                    ,sysToolBar1.BtnSingleAudit.Name
                    ,sysToolBar1.BtnCopy.Name
                    ,sysToolBar1.BtnModify.Name
                    ,sysToolBar1.BtnClose.Name
                            },
                    new string[] { "F3", "F4", "F5", "F6", "F7" }
                    );
        }

        #endregion

        #region 事件

        private void FrmBatchEdit_Load(object sender, EventArgs e)
        {
            InitButtons();

            List<EntityUserRole> listUserRole = CacheClient.GetCache<EntityUserRole>();
            int powerCount = listUserRole.FindAll(i => i.UserLoginId == UserInfo.loginID && i.RoleRemark.Contains("资料批量修改")).Count;

            //是否有权限可以修改审核时间
            if (powerCount > 0 || UserInfo.loginID == "admin")
            {
                VisibleDataControl(true);
            }

            sysToolBar1.NotWriteLogButtonNameList.Add(sysToolBar1.BtnCopy.Name);//复制按钮-去掉默认日志
            sysToolBar1.NotWriteLogButtonNameList.Add(sysToolBar1.BtnModify.Name);//修改按钮-去掉默认日志
            sysToolBar1.NotWriteLogButtonNameList.Add(sysToolBar1.BtnSearch.Name);//去掉默认日志
            sysToolBar1.NotWriteLogButtonNameList.Add(sysToolBar1.BtnSingleAudit.Name);//去掉默认日志

            fpat_chk_id.SetFilter(fpat_chk_id.getDataSource().FindAll(i => i.UserType == "检验组"));
            gpat_chk_id.SetFilter(gpat_chk_id.getDataSource().FindAll(i => i.UserType == "检验组"));
        }

        /// <summary>
        /// 目标数据
        /// </summary>
        private void sysToolBar1_OnBtnSingleAuditClicked(object sender, EventArgs e)
        {
            try
            {
                SearchTarget();
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }

        }

        /// <summary>
        /// 源数据
        /// </summary>    
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                SearchSource();
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            try
            {
                HandleData("复制");
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取源目标条件
        /// </summary>
        /// <returns></returns>
        private BatchEditSrc GetSourceParam()
        {
            BatchEditSrc entitySrc = new BatchEditSrc();
            entitySrc.pat_date = (DateTime)this.dateReportSource.EditValue;//日期
            entitySrc.pat_itr_id = this.fPat_itr_id.valueMember;//仪器
            entitySrc.pat_dep_id = fpat_dep_id.valueMember;
            entitySrc.pat_dep_name = fpat_dep_id.displayMember;
            entitySrc.pat_i_code = fpat_chk_id.valueMember;
            entitySrc.pat_sam_id = fPat_sam_id.valueMember;

            entitySrc.pat_sid_begin = Convert.ToInt64(txtStartNum.Text);
            entitySrc.pat_sid_end = Convert.ToInt64(txtEndNum.Text);
            entitySrc.MatchMode = this.rdoSourceMatchType.EditValue.ToString();
            return entitySrc;
        }

        /// <summary>
        /// 获取目标条件
        /// </summary>
        /// <returns></returns>
        private BatchEditDest GetDestParam()
        {
            BatchEditDest entityDest = new BatchEditDest();
            entityDest.pat_date = (DateTime)this.dateReportTarget.EditValue;//日期
            entityDest.pat_itr_id = this.gPat_itr_id.valueMember;//仪器
            entityDest.pat_dep_id = gpat_dep_id.valueMember;
            entityDest.pat_dep_name = gpat_dep_id.displayMember;
            entityDest.pat_i_code = gpat_chk_id.valueMember;
            entityDest.pat_sam_id = gPat_sam_id.valueMember;

            entityDest.pat_sid_begin = Convert.ToInt64(txtStartNumgoal.Text);
            entityDest.pat_sid_end = Convert.ToInt64(txtEndNumgoal.Text);

            entityDest.pat_ori_id = fpat_ori_id.valueMember;
            entityDest.pat_unit = fpat_unit.Text;
            entityDest.pat_doc_id = fpat_doc_id.valueMember;
            entityDest.pat_rem = fpat_rem.valueMember;

            entityDest.pat_age_exp = txtAge.AgeValueText;
            entityDest.pat_age = txtAge.AgeToMinute;
            entityDest.pat_sex = txtPatSex.valueMember;


            entityDest.pat_exp = txtPatExp.Text;
            entityDest.pat_diag = gpat_dialog.Text;
            if (!Compare.IsEmpty(fPat_sample_date.EditValue))//采样时间
            {
                entityDest.pat_sample_date = (DateTime)fPat_sample_date.EditValue;
            }

            if (!Compare.IsEmpty(gpat_sdate.EditValue))//送检时间
            {
                entityDest.pat_sdate = (DateTime)gpat_sdate.EditValue;
            }

            if (!Compare.IsEmpty(fPat_rec_date.EditValue))//接收时间
            {
                entityDest.pat_apply_date = (DateTime)fPat_rec_date.EditValue;
            }

            if (!Compare.IsEmpty(fPat_jy_date.EditValue))//检验时间
            {
                entityDest.pat_jy_date = (DateTime)fPat_jy_date.EditValue;
            }

            if (!Compare.IsEmpty(gpat_chkDate.EditValue))//一审时间
            {
                entityDest.pat_chk_date = (DateTime)gpat_chkDate.EditValue;
            }

            if (!Compare.IsEmpty(gpat_reportDate.EditValue))//二审时间
            {
                entityDest.pat_report_date = (DateTime)gpat_reportDate.EditValue;
            }

            entityDest.MatchMode = this.rdoDestMatchType.EditValue.ToString();

            if (f != null && f.dtCombine != null && f.dtCombine.Count > 0)
            {
                int com_seq = 0;
                foreach (EntityDicCombine drCombine in f.dtCombine)
                {
                    EntityPatientsMi_4Barcode entityCombine = new EntityPatientsMi_4Barcode();
                    entityCombine.pat_com_name = drCombine.ComName.ToString();
                    entityCombine.pat_com_id = drCombine.ComId.ToString();
                    entityCombine.pat_seq = com_seq;

                    entityDest.PatientsMi.Add(entityCombine);

                    com_seq++;
                }

            }
            return entityDest;
        }

        /// <summary>
        /// 修改
        /// </summary>
        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            this.gcError.DataSource = null;
            this.sysToolBar1.Focus();

            if (CheckTxtContent())
            {
                if (MessageDialog.Show("是否继续修改？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                ProxyBatchPatData proxy = new ProxyBatchPatData();
                try
                {
                    #region 源条件

                    //源条件
                    BatchEditSrc entitySrc = GetSourceParam();

                    #region 源条件日记记录

                    //如果源数据有数据,则记录查询条件
                    if (true)
                    {
                        SourceLogMsg = "";
                        if (entitySrc != null && entitySrc.pat_date != null)
                        {
                            SourceLogMsg += "报告日期:" + entitySrc.pat_date.ToString("yyyy-MM-dd");
                        }
                        if (entitySrc != null && entitySrc.pat_itr_id != null && entitySrc.pat_itr_id.Length > 0)
                        {
                            SourceLogMsg += "仪器编码:" + entitySrc.pat_itr_id;
                        }
                        if (entitySrc != null)
                        {
                            SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_begin.ToString();
                        }
                        if (entitySrc != null)
                        {
                            SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_end.ToString();
                        }
                        if (entitySrc != null && entitySrc.pat_sam_id != null && entitySrc.pat_sam_id.Length > 0)
                        {
                            SourceLogMsg += "标本编码:" + entitySrc.pat_sam_id;
                        }
                        if (entitySrc != null && entitySrc.pat_i_code != null && entitySrc.pat_i_code.Length > 0)
                        {
                            SourceLogMsg += "检验者工号:" + entitySrc.pat_i_code;
                        }
                        if (entitySrc != null && entitySrc.pat_dep_id != null && entitySrc.pat_dep_id.Length > 0)
                        {
                            SourceLogMsg += "送检科室代码:" + entitySrc.pat_dep_id;
                        }

                        if (!string.IsNullOrEmpty(SourceLogMsg))
                        {
                            SourceLogMsg = "[" + SourceLogMsg + "]";
                        }
                    }

                    #endregion

                    #endregion

                    #region 目标条件

                    BatchEditDest entityDest = GetDestParam();

                    #region 目标条件日记记录

                    if (true && entityDest != null)
                    {
                        TargetLogMsg = "";
                        if (entityDest.MatchMode != null && entityDest.MatchMode.Length > 0)
                        {
                            TargetLogMsg += "MatchMode:" + entityDest.MatchMode + "|";
                        }
                        if (entityDest.pat_date != null)
                        {
                            TargetLogMsg += "pat_date:" + entityDest.pat_date.ToString("yyyy-MM-dd") + "|";
                        }
                        if (entityDest.pat_age_exp != null && entityDest.pat_age_exp.Length > 0)
                        {
                            TargetLogMsg += "pat_age_exp:" + entityDest.pat_age_exp + "|";
                        }
                        if (entityDest.pat_apply_date != null)
                        {
                            TargetLogMsg += "pat_apply_date:" + entityDest.pat_apply_date.ToString() + "|";
                        }
                        if (entityDest.pat_chk_date != null)
                        {
                            TargetLogMsg += "pat_chk_date:" + entityDest.pat_chk_date.ToString() + "|";
                        }
                        if (entityDest.pat_dep_id != null && entityDest.pat_dep_id.Length > 0)
                        {
                            TargetLogMsg += "pat_dep_id:" + entityDest.pat_dep_id + "|";
                        }
                        if (entityDest.pat_doc_id != null && entityDest.pat_doc_id.Length > 0)
                        {
                            TargetLogMsg += "pat_doc_id:" + entityDest.pat_doc_id + "|";
                        }
                        if (entityDest.pat_exp != null && entityDest.pat_exp.Length > 0)
                        {
                            TargetLogMsg += "pat_exp:" + entityDest.pat_exp + "|";
                        }
                        if (entityDest.pat_i_code != null && entityDest.pat_i_code.Length > 0)
                        {
                            TargetLogMsg += "pat_i_code:" + entityDest.pat_i_code + "|";
                        }
                        if (entityDest.pat_itr_id != null && entityDest.pat_itr_id.Length > 0)
                        {
                            TargetLogMsg += "pat_itr_id:" + entityDest.pat_itr_id + "|";
                        }
                        if (entityDest.pat_jy_date != null)
                        {
                            TargetLogMsg += "pat_jy_date:" + entityDest.pat_jy_date.ToString() + "|";
                        }

                        if (entityDest.pat_ori_id != null && entityDest.pat_ori_id.Length > 0)
                        {
                            TargetLogMsg += "pat_ori_id:" + entityDest.pat_ori_id + "|";
                        }

                        if (entityDest.pat_rem != null && entityDest.pat_rem.Length > 0)
                        {
                            TargetLogMsg += "pat_rem:" + entityDest.pat_rem + "|";
                        }

                        if (entityDest.pat_report_date != null)
                        {
                            TargetLogMsg += "pat_report_date:" + entityDest.pat_report_date.ToString() + "|";
                        }

                        if (entityDest.pat_sam_id != null && entityDest.pat_sam_id.Length > 0)
                        {
                            TargetLogMsg += "pat_sam_id:" + entityDest.pat_sam_id + "|";
                        }
                        if (entityDest.pat_sample_date != null)
                        {
                            TargetLogMsg += "pat_sample_date:" + entityDest.pat_sample_date.ToString() + "|";
                        }
                        if (entityDest.pat_sdate != null)
                        {
                            TargetLogMsg += "pat_sdate:" + entityDest.pat_sdate.ToString() + "|";
                        }
                        if (entityDest.pat_sex != null && entityDest.pat_sex.Length > 0)
                        {
                            TargetLogMsg += "pat_sex:" + entityDest.pat_sex + "|";
                        }
                        if (entityDest.pat_sid_begin > 0)
                        {
                            TargetLogMsg += "pat_sid_begin:" + entityDest.pat_sid_begin + "|";
                        }
                        if (entityDest.pat_sid_end > 0)
                        {
                            TargetLogMsg += "pat_sid_end:" + entityDest.pat_sid_end + "|";
                        }
                        if (entityDest.pat_unit != null && entityDest.pat_unit.Length > 0)
                        {
                            TargetLogMsg += "pat_unit:" + entityDest.pat_unit + "|";
                        }
                        if (entityDest.PatientsMi != null && entityDest.PatientsMi.Count > 0)
                        {
                            string tempMi = "";
                            foreach (EntityPatientsMi_4Barcode sMi in entityDest.PatientsMi)
                            {
                                if (!string.IsNullOrEmpty(sMi.pat_com_id))
                                    if (string.IsNullOrEmpty(tempMi))
                                    {
                                        tempMi = sMi.pat_com_id;
                                    }
                                    else
                                    {
                                        tempMi += "," + sMi.pat_com_id;
                                    }
                            }
                            TargetLogMsg += "PatientsMi(" + tempMi + ")" + "|";
                        }

                        if (!string.IsNullOrEmpty(TargetLogMsg))
                        {
                            TargetLogMsg = "[" + TargetLogMsg.TrimEnd(new char[] { '|' }) + "]";
                        }
                    }

                    #endregion

                    #endregion

                    if ((!string.IsNullOrEmpty(SourceLogMsg)) && (!string.IsNullOrEmpty(TargetLogMsg)))
                    {
                        WriteLogByBatchEdit(SourceLogMsg + "=>" + TargetLogMsg, "修改");
                    }

                    List<EntityOperateResult> listResult = proxy.Service.BatchUpdatePatientData(entitySrc, entityDest);//返回的信息
                    List<EntityPidReportMain> listMessage = new List<EntityPidReportMain>();
                    int count = 0;
                    if (listResult.Count > 0)
                    {
                        foreach (EntityOperateResult opResult in listResult)
                        {
                            if (!opResult.Success)
                            {
                                EntityPidReportMain patient = opResult.Data.Patient.Clone() as EntityPidReportMain;
                                patient.ErrorMessage = OperationMessageHelper.GetErrorMessage(opResult.Message);
                                listMessage.Add(patient);
                            }
                        }

                        if (listMessage.Count > 0)
                        {
                            this.gcError.DataSource = listMessage;
                            gcError.RefreshDataSource();
                            MessageDialog.Show("修改失败！");
                            count = listMessage.Count;
                            return;
                        }
                    }

                    //***************************************************************************
                    //如果勾选了则进行以下操作
                    if (this.chkClearNotContainItem.Checked)
                    {
                        List<string> sourceListPatId = getCurrentPatBySID(Convert.ToInt32(txtStartNum.Text), Convert.ToInt32(txtEndNum.Text), fPat_itr_id.valueMember, dateReportSource.DateTime);

                        //如果有修改成功的话，则进行判断新增加的那些目标patId项目是否与选中的组合不同属
                        if (sourceListPatId.Count > count)
                        {
                            List<string> TargetListPatId = getCurrentPatBySID(Convert.ToInt32(txtStartNumgoal.Text), Convert.ToInt32(txtEndNumgoal.Text), gPat_itr_id.valueMember, dateReportTarget.DateTime);
                            List<string> com_id = new List<string>();
                            for (int i = 0; i < entityDest.PatientsMi.Count; i++)
                            {
                                com_id.Add(entityDest.PatientsMi[i].pat_com_id);
                            }
                            ProxyBatchPatData dataProxy = new ProxyBatchPatData();
                            bool result = dataProxy.Service.SetResultoBcFlag(TargetListPatId, com_id);
                            if (!result)
                            {
                                MessageDialog.Show("修改失败！");
                                return;
                            }
                        }
                    }
                    //***************************************************************************

                    lis.client.control.MessageDialog.Show("修改成功！");
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 表单验证
        /// </summary>
        /// <returns></returns>
        private bool CheckTxtContent()
        {
            bool success = true;
            String message = String.Empty;

            if (txtEndNum.Text.Trim().Length <= 0)
            {
                message += "请输入源样本始号!\r\n";
                success = false;
            }
            if (txtEndNumgoal.Text.Trim().Length <= 0)
            {
                message += "请输入目标样本始号!\r\n";
                success = false;
            }
            if (txtEndNum.Text.Trim().Length <= 0)
            {
                message += "请输入源样本终号!\r\n";
                success = false;
            }
            if (txtEndNumgoal.Text.Trim().Length <= 0)
            {
                message += "请输入目标样本终号!\r\n";
                success = false;
            }

            if (fPat_itr_id.valueMember == null)
            {
                message += "请输入源仪器号!\r\n";
                success = false;
            }

            if (gPat_itr_id.valueMember == null)
            {
                message += "请输入目标仪器号!\r\n";
                success = false;
            }



            if (txtStartNum.Text.Length > 0 && txtEndNum.Text.Length > 0)
            {
                if (int.Parse(txtStartNum.Text) > int.Parse(txtEndNum.Text))
                {
                    message += "源样本始号不能大于源样本终号!\r\n";
                    success = false;
                }
            }
            if (txtStartNumgoal.Text.Length > 0 && txtEndNumgoal.Text.Length > 0)
            {
                if (int.Parse(txtStartNumgoal.Text) > int.Parse(txtEndNumgoal.Text))
                {
                    message += "目标样本始号不能大于目标样本终号!\r\n";
                    success = false;
                }
            }
            if (txtStartNum.Text.Length > 0 && txtEndNum.Text.Length > 0 && txtStartNumgoal.Text.Length > 0 && txtEndNumgoal.Text.Length > 0)
            {
                if ((int.Parse(txtEndNum.Text) - int.Parse(txtStartNum.Text)) != (int.Parse(txtEndNumgoal.Text) - int.Parse(txtStartNumgoal.Text)))
                {
                    message += "源样本号数与目标样本号数不一致!\r\n";
                    success = false;
                }
            }
            if (message.Length > 0)
            {
                MessageDialog.Show("操作被取消,原因是:\r\n" + message, "提示!");
            }
            return success;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        private void DoData(string actType)
        {
            if (!CheckTxtContent())
            {
                return;
            }

            if (gPat_itr_id.valueMember != null && gPat_itr_id.valueMember == fPat_itr_id.valueMember)
            {
                if (DialogResult.Yes != lis.client.control.MessageDialog.Show("你将进行同仪器操作，请务必先确认数据正确！是否继续此操作？", "提示", MessageBoxButtons.YesNo))
                {
                    lis.client.control.MessageDialog.Show("操作已取消！", "提示!");
                    return;
                }

            }

            List<EntityPidReportMain> dtSoure = Search(true);
            if (dtSoure == null || dtSoure.Count == 0)
            {
                MessageBox.Show("没有数据符合当前查询条件! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<EntityPidReportMain> dtData;

            if (actType == LIS_Const.action_type.Update && IncludeAuditedData(dtSoure))
            {
                List<EntityPidReportMain> dtChecked = new List<EntityPidReportMain>();
                List<EntityPidReportMain> dtError = FindNotEditData(dtSoure, ref dtChecked);
                bsPatients.DataSource = dtError;
                dtData = dtChecked;
            }
            else
                dtData = dtSoure;

            List<EntityPidReportMain> dtDoData = GetData(dtData, actType);


            //if (actType == LIS_Const.action_type.New)
            //{
            //    if (isExist(dtDoData))
            //    {
            //        lis.client.control.MessageDialog.Show("数据已存在数据库! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //}

            if ((!string.IsNullOrEmpty(SourceLogMsg)) && (!string.IsNullOrEmpty(TargetLogMsg)))
            {
                WriteLogByBatchEdit(SourceLogMsg + "=>" + TargetLogMsg, "复制");//写入日志记录
            }
            ProxyBatchPatData dataProxy = new ProxyBatchPatData();
            bool result = false;
            if (actType == LIS_Const.action_type.New)
            {
                result = dataProxy.Service.BatchCopyPatientData(dcl.client.common.Util.ToCallerInfo(), dtDoData, listComb);
            }
            else
            {
                ProxyPidReportMain mainProxy = new ProxyPidReportMain();
                result = mainProxy.Service.UpdatePatientData(dtDoData);
            }
            if (result)
                MessageBox.Show("复制成功");
            else
                MessageBox.Show("复制失败");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtSoure"></param>
        /// <param name="dtCheckedData"></param>
        /// <returns></returns>
        private List<EntityPidReportMain> FindNotEditData(List<EntityPidReportMain> dtSoure, ref List<EntityPidReportMain> dtCheckedData)
        {
            List<EntityPidReportMain> error = new List<EntityPidReportMain>();

            if (dtCheckedData == null)
                dtCheckedData = new List<EntityPidReportMain>();

            foreach (EntityPidReportMain row in dtSoure)
            {
                if (row.RepStatus != null && PatientsHelper.HasAudited(row.RepStatus.Value.ToString()))
                {
                    row.ErrorMessage = "已审核数据";
                    error.Add(row);
                }
                else
                    dtCheckedData.Add(row);
            }

            return error;
        }


        /// <summary>
        /// 检查是否存在于数据库
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool isExist(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;

            bool _is = false;
            //string pat_id = "";
            //foreach (DataRow dr in dt.Rows)
            //{
            //    pat_id += "'" + dr["pat_id"].ToString() + "',";
            //}

            //DataSet dsCondtion = CommonClient.CreateDS(new String[] { "tablename", "columns", "where" }, "dtCondtion");
            //DataRow drt = dsCondtion.Tables[0].NewRow();
            //drt["tablename"] = "patients";
            //drt["columns"] = " count(*)";
            //drt["where"] = "pat_id in (" + pat_id.TrimEnd(',') + " )";
            //dsCondtion.Tables[0].Rows.Add(drt);

            //DataSet result = base.doOther(dsCondtion);
            //if (int.Parse(result.Tables[0].Rows[0][0].ToString()) > 0)
            //{
            //    _is = true;
            //}
            return _is;
        }

        /// <summary>
        /// 是否包含已经审核数据
        /// </summary>
        private bool IncludeAuditedData(List<EntityPidReportMain> dt)
        {
            if (dt == null)
                return false;

            bool result = false;

            foreach (EntityPidReportMain dr in dt)
            {
                if (dr.RepStatus != null && dcl.common.PatientsHelper.HasAudited(dr.RepStatus.Value.ToString()))
                {
                    return true;
                }
            }

            return result;
        }


        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <returns></returns>
        private List<EntityPidReportMain> Search(bool searchSource)
        {
            string selectInstrmt = this.fPat_itr_id.valueMember == null ? ("") : this.fPat_itr_id.valueMember.ToString();

            string startNum = txtStartNum.Text;
            string endNum = txtEndNum.Text;
            string date = this.dateReportSource.DateTime.Date == DateTime.MinValue ? ("") : this.dateReportSource.DateTime.Date.ToString();
            string inspectValue = this.fpat_chk_id.valueMember == null ? ("") : this.fpat_chk_id.valueMember.ToString();
            string sample = this.fPat_sam_id.valueMember == null ? ("") : this.fPat_sam_id.valueMember.ToString();
            string depart = this.fpat_dep_id.valueMember == null ? ("") : this.fpat_dep_id.valueMember.ToString();
            string match_mode = this.rdoSourceMatchType.EditValue.ToString();

            if (!searchSource)
            {
                selectInstrmt = this.gPat_itr_id.valueMember == null ? ("") : this.gPat_itr_id.valueMember.ToString();

                startNum = txtStartNumgoal.Text;
                endNum = txtEndNumgoal.Text;
                date = this.dateReportTarget.DateTime == DateTime.MinValue ? ("") : this.dateReportTarget.DateTime.ToString();


                inspectValue = "";
                sample = "";
                depart = "";

                match_mode = this.rdoDestMatchType.EditValue.ToString();
            }

            ProxyPidReportMain proxyMain = new ProxyPidReportMain();

            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.ListItrId.Add(selectInstrmt);
            patientQc.DateStart = Convert.ToDateTime(date);
            patientQc.DateEnd = Convert.ToDateTime(date).AddDays(1).AddMilliseconds(-1);
            patientQc.SamId = sample;
            patientQc.DepId = depart;
            patientQc.PatDepName = fpat_dep_id.displayMember;
            patientQc.PidCheckUserId = inspectValue;
            if (match_mode == "0")
            {
                EntitySid sid = new EntitySid();
                sid.StartSid = Convert.ToInt32(startNum);
                sid.EndSid = Convert.ToInt32(endNum);
                patientQc.ListSidRange.Add(sid);
            }
            else
            {
                EntitySortNo sortNo = new EntitySortNo();
                sortNo.StartNo = Convert.ToInt32(startNum);
                sortNo.EndNo = Convert.ToInt32(endNum);
                patientQc.ListSortRange.Add(sortNo);
            }

            List<EntityPidReportMain> listPatients = proxyMain.Service.PatientQuery(patientQc);

            if (listPatients.Count != 0)
            {
                #region 日记记录

                //如果源数据有数据,则记录查询条件
                SourceLogMsg = "";
                BatchEditSrc entitySrc = GetSourceParam();
                if (entitySrc != null && entitySrc.pat_date != null)
                {
                    SourceLogMsg += "报告日期:" + entitySrc.pat_date.ToString("yyyy-MM-dd");
                }
                if (entitySrc != null && entitySrc.pat_itr_id != null && entitySrc.pat_itr_id.Length > 0)
                {
                    SourceLogMsg += "仪器编码:" + entitySrc.pat_itr_id;
                }
                if (entitySrc != null)
                {
                    SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_begin.ToString();
                }
                if (entitySrc != null)
                {
                    SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_end.ToString();
                }
                if (entitySrc != null && entitySrc.pat_sam_id != null && entitySrc.pat_sam_id.Length > 0)
                {
                    SourceLogMsg += "标本编码:" + entitySrc.pat_sam_id;
                }
                if (entitySrc != null && entitySrc.pat_i_code != null && entitySrc.pat_i_code.Length > 0)
                {
                    SourceLogMsg += "检验者工号:" + entitySrc.pat_i_code;
                }
                if (entitySrc != null && entitySrc.pat_dep_id != null && entitySrc.pat_dep_id.Length > 0)
                {
                    SourceLogMsg += "送检科室代码:" + entitySrc.pat_dep_id;
                }

                if (!string.IsNullOrEmpty(SourceLogMsg))
                {
                    SourceLogMsg = "[" + SourceLogMsg + "]";
                }

                #endregion
            }
            return listPatients;
        }


        private List<EntityPidReportMain> GetData(List<EntityPidReportMain> dt, string acttype)
        {
            if (string.IsNullOrEmpty(txtStartNumgoal.Text) && string.IsNullOrEmpty(txtStartNum.Text))
                return new List<EntityPidReportMain>();
            int patSampeIdDiffer = int.Parse(txtStartNumgoal.Text) - int.Parse(txtStartNum.Text);  //当前操作用户输入起始号
            bool isChange = !chkCopySourceRemark.Checked && acttype == LIS_Const.action_type.New;

            //给需要复制到目标数据的实体复制
            foreach (EntityPidReportMain row in dt)
            {
                if (isChange)
                {
                    row.RepRemark = null;
                }
                row.RepCheckUserId = gpat_chk_id.valueMember;
                row.RepItrId = gPat_itr_id.valueMember;
                if (ExistsDateTime(dateReportTarget.DateTime.Date))
                    row.RepInDate = dateReportTarget.DateTime.Date;
                row.PidDeptId = gpat_dep_id.valueMember;
                row.PidSamId = gPat_sam_id.valueMember;
                row.PidRemark = fpat_rem.valueMember;
                row.PidSrcId = fpat_ori_id.valueMember;
                row.PidDoctorCode = fpat_doc_id.valueMember;
                row.PidUnit = fpat_unit.Text;
                row.PidSex = txtPatSex.valueMember;
                if (string.IsNullOrEmpty(txtAge.AgeValueText))
                    row.PidAge = null;
                else
                    row.PidAge = txtAge.AgeToMinute;
                row.PidAgeExp = txtAge.AgeValueText;
                row.RepRemark = txtPatExp.Text;
                //替换组合项目名称
                row.PidComName = txtCombineEdit.Text;

                if (ExistsDateTime(fPat_sample_date.DateTime))
                    row.SampCollectionDate = fPat_sample_date.DateTime;//采样时间
                if (ExistsDateTime(gpat_sdate.DateTime))
                    row.SampSendDate = gpat_sdate.DateTime;//送检时间
                if (ExistsDateTime(fPat_rec_date.DateTime))
                    row.SampApplyDate = fPat_rec_date.DateTime;//接收时间
                if (ExistsDateTime(fPat_jy_date.DateTime))
                    row.SampCheckDate = fPat_jy_date.DateTime;//检验时间
                if (ExistsDateTime(gpat_chkDate.DateTime))
                    row.RepAuditDate = gpat_chkDate.DateTime;//一审时间
                if (ExistsDateTime(gpat_reportDate.DateTime))
                    row.RepReportDate = gpat_reportDate.DateTime;//二审时间
                row.IsCopyCombine = chkCopyCombine.Checked ? "1" : "0";

                //修改pat_id
                string source_itr_id = row.RepItrId;
                if (acttype == "New" && this.rdoSourceMatchType.EditValue.ToString() == "1"
                    && this.rdoDestMatchType.EditValue.ToString() == "1"
                    )
                {
                    row.RepSid = (int.Parse(row.RepSerialNum) + patSampeIdDiffer).ToString();
                    row.RepSerialNum = row.RepSid;
                }
                else if (acttype == "New" && this.rdoSourceMatchType.EditValue.ToString() == "0"
                    && this.rdoDestMatchType.EditValue.ToString() == "1")
                {
                    row.RepSid = (int.Parse(row.RepSid) + patSampeIdDiffer).ToString();
                    row.RepSerialNum = row.RepSid;
                }
                else
                {
                    //更新样本号
                    if (this.rdoSourceMatchType.EditValue.ToString() == "0" && rdoDestMatchType.EditValue.ToString() == "0") //
                    {
                        row.RepSid = (int.Parse(row.RepSid) + patSampeIdDiffer).ToString();
                    }
                    else //序号方式修改
                    {
                        row.RepSid = (int.Parse(row.RepSerialNum) + patSampeIdDiffer).ToString();
                        row.RepSerialNum = null;
                    }
                }

                //为旧病人ID和仪器ID赋值
                row.RepIdOld = row.RepId;
                row.RepItrIdOld = fPat_itr_id.valueMember;
                if (ExistsDateTime(dateReportTarget.DateTime.Date))
                {
                    row.RepId = ResultoHelper.GenerateResID(gPat_itr_id.valueMember, dateReportTarget.DateTime.Date, row.RepSid.ToString());
                }
                row.RepUrgentFlag = null;
            }

            #region 目标日记记录

            #region 为日志字符串赋值
            TargetLogMsg += "RepCheckUserId" + ":" + gpat_chk_id.valueMember + "|";
            TargetLogMsg += "RepItrId" + ":" + gPat_itr_id.valueMember + "|";
            if (ExistsDateTime(dateReportTarget.DateTime.Date))
                TargetLogMsg += "RepInDate" + ":" + dateReportTarget.DateTime + "|";
            TargetLogMsg += "PidDeptId" + ":" + gpat_dep_id.valueMember + "|";
            TargetLogMsg += "PidSamId" + ":" + gPat_sam_id.valueMember + "|";
            TargetLogMsg += "PidRemark" + ":" + fpat_rem.valueMember + "|";
            TargetLogMsg += "PidSrcId" + ":" + fpat_ori_id.valueMember + "|";
            TargetLogMsg += "PidDoctorCode" + ":" + fpat_doc_id.valueMember + "|";
            TargetLogMsg += "PidUnit" + ":" + fpat_unit.Text + "|";
            TargetLogMsg += "PidSex" + ":" + txtPatSex.valueMember + "|";
            if (string.IsNullOrEmpty(txtAge.AgeValueText))
                TargetLogMsg += "PidAge" + ":" + "" + "|";
            else
                TargetLogMsg += "PidAge" + ":" + txtAge.AgeToMinute + "|";
            TargetLogMsg += "PidAgeExp" + ":" + txtAge.AgeValueText + "|";
            TargetLogMsg += "RepRemark" + ":" + txtPatExp.Text + "|";
            TargetLogMsg += "PidComName" + ":" + txtCombineEdit.Text + "|";
            if (ExistsDateTime(gpat_sdate.DateTime))
                TargetLogMsg += "SampSendDate" + ":" + gpat_sdate.DateTime + "|";
            if (ExistsDateTime(fPat_rec_date.DateTime))
                TargetLogMsg += "SampReciveDate" + ":" + fPat_rec_date.DateTime + "|";
            if (ExistsDateTime(fPat_jy_date.DateTime))
                TargetLogMsg += "SampCheckDate" + ":" + fPat_jy_date.DateTime + "|";
            if (ExistsDateTime(gpat_chkDate.DateTime))
                TargetLogMsg += "RepAuditDate" + ":" + gpat_chkDate.DateTime + "|";
            if (ExistsDateTime(gpat_reportDate.DateTime))
                TargetLogMsg += "RepReportDate" + ":" + gpat_reportDate.DateTime + "|";
            string copy = chkCopyCombine.Checked ? "1" : "0";
            TargetLogMsg += "IsCopyCombine" + ":" + copy + "|";
            #endregion

            if (!string.IsNullOrEmpty(TargetLogMsg))
            {
                TargetLogMsg = "[" + TargetLogMsg.TrimEnd(new char[] { '|' }) + "]";
            }

            #endregion

            return dt;
        }

        /// <summary>
        /// 时间是否有效
        /// </summary>
        /// <param name="dateTime">时间</param>
        private bool ExistsDateTime(DateTime dateTime)
        {
            return dateTime != null && dateTime != DateTime.MinValue;
        }

        #endregion


        /// <summary>
        /// 复制 / 修改
        /// </summary>
        /// <param name="command"></param>
        private void HandleData(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            if (gcError.DataSource != null)
                gcError.DataSource = null;

            DialogResult dResult = lis.client.control.MessageDialog.Show("是否" + command + " ? ", "提示", MessageBoxButtons.YesNo);
            if (dResult == DialogResult.Yes)
            {
                if (command.Trim() == "复制")
                    DoData(LIS_Const.action_type.New);
                else
                    DoData(LIS_Const.action_type.Update);
            }
        }

        /// <summary>
        /// 目标数据
        /// </summary>
        private void SearchTarget()
        {
            if(string.IsNullOrEmpty(txtStartNumgoal.Text.Trim()))
            {
                MessageBox.Show("请填写目标内容和范围中的样本始号! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int i;
            if(!int.TryParse(txtStartNumgoal.Text.Trim(),out i))
            {
                MessageBox.Show("目标内容和范围中的样本始号必须为整数! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtEndNumgoal.Text.Trim()))
            {
                MessageBox.Show("请填写目标内容和范围中的样本终号! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!int.TryParse(txtEndNumgoal.Text.Trim(), out i))
            {
                MessageBox.Show("目标内容和范围中的样本终号必须为整数! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<EntityPidReportMain> dtSoure = Search(false);
            if (dtSoure.Count <= 0)
            {
                MessageBox.Show("没有数据符合当前查询条件! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FrmTarget ft = new FrmTarget(dtSoure);
            ft.Show();
        }

        /// <summary>
        /// 源数据
        /// </summary>
        private void SearchSource()
        {
            List<EntityPidReportMain> listTable = Search(true);
            if (listTable.Count <= 0)
            {
                MessageBox.Show("没有数据符合当前查询条件! ", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //如果源数据有数据,则记录查询条件
            if (listTable != null && listTable.Count > 0)
            {
                SourceLogMsg = "";
                BatchEditSrc entitySrc = GetSourceParam();
                if (entitySrc != null && entitySrc.pat_date != null)
                {
                    SourceLogMsg += "报告日期:" + entitySrc.pat_date.ToString("yyyy-MM-dd");
                }
                if (entitySrc != null && entitySrc.pat_itr_id != null && entitySrc.pat_itr_id.Length > 0)
                {
                    SourceLogMsg += "仪器编码:" + entitySrc.pat_itr_id;
                }
                if (entitySrc != null)
                {
                    SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_begin.ToString();
                }
                if (entitySrc != null)
                {
                    SourceLogMsg += (entitySrc.MatchMode == "0" ? "样本" : "序号") + "始号:" + entitySrc.pat_sid_end.ToString();
                }
                if (entitySrc != null && entitySrc.pat_sam_id != null && entitySrc.pat_sam_id.Length > 0)
                {
                    SourceLogMsg += "标本编码:" + entitySrc.pat_sam_id;
                }
                if (entitySrc != null && entitySrc.pat_i_code != null && entitySrc.pat_i_code.Length > 0)
                {
                    SourceLogMsg += "检验者工号:" + entitySrc.pat_i_code;
                }
                if (entitySrc != null && entitySrc.pat_dep_id != null && entitySrc.pat_dep_id.Length > 0)
                {
                    SourceLogMsg += "送检科室代码:" + entitySrc.pat_dep_id;
                }

                if (!string.IsNullOrEmpty(SourceLogMsg))
                {
                    SourceLogMsg = "[" + SourceLogMsg + "]";
                }
            }

            FrmSource ft = new FrmSource(listTable);
            ft.Show();
        }



        private void txtCombineEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                //根据当前物理组合仪器专业组过滤

                //当前物理组
                string ctype = string.Empty;
                f = new FrmCombineManager(listComb, ctype, "");
                f.RefreshCombineTextDemanded += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_RefreshCombineTextDemanded);
                f.Location = this.txtCombineEdit.Location;
                f.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X + this.txtCombineEdit.Width - f.Width;
                f.Top = this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;
                f.ShowDialog();
            }
        }


        void f_RefreshCombineTextDemanded(object sender, EventArgs args)
        {
            if (f.dtCombine == null || f.dtCombine.Count == 0)
            {
                txtCombineEdit.Text = "";
                return;
            }

            txtCombineEdit.Text = "";

            foreach (EntityDicCombine row in f.dtCombine)
            {
                txtCombineEdit.Text += "+" + row.ComName.ToString();
            }

            txtCombineEdit.Text = txtCombineEdit.Text.TrimStart('+');
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            FrmBscripeSelectV2 fb = new FrmBscripeSelectV2("0");
            fb.GetExp += new FrmBscripeSelectV2.SelectExp(fb_GetExp);
            fb.ShowDialog();
        }

        void fb_GetExp(string exp)
        {
            txtPatExp.Text = exp;
        }


        private void rdoSourceMatchType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.rdoSourceMatchType.EditValue.ToString() == "0")
            {
                this.layitemBegin.Text = "起始样本";
                this.layitemEnd.Text = "终止样本";

                this.layitemBegin.AppearanceItemCaption.ForeColor = Color.Blue;
                this.layitemEnd.AppearanceItemCaption.ForeColor = Color.Blue;
            }
            else
            {
                this.layitemBegin.Text = "起始序号";
                this.layitemEnd.Text = "终止序号";

                this.layitemBegin.AppearanceItemCaption.ForeColor = Color.Red;
                this.layitemEnd.AppearanceItemCaption.ForeColor = Color.Red;
            }
        }

        private void rdoDestMatchType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.rdoDestMatchType.EditValue.ToString() == "0")
            {
                this.layitemBeginT.Text = "起始样本";
                this.layitemEndT.Text = "终止样本";

                this.layitemBeginT.AppearanceItemCaption.ForeColor = Color.Blue;
                this.layitemEndT.AppearanceItemCaption.ForeColor = Color.Blue;
            }
            else
            {
                this.layitemBeginT.Text = "起始序号";
                this.layitemEndT.Text = "终止序号";

                this.layitemBeginT.AppearanceItemCaption.ForeColor = Color.Red;
                this.layitemEndT.AppearanceItemCaption.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 获取PatID
        /// </summary>
        /// <returns></returns>
        private List<string> getCurrentPatBySID(int start, int end, string itrCode, DateTime dateTime)
        {
            List<string> patId = new List<string>();
            string date = dateTime.ToString("yyyyMMdd");
            for (int i = start; i <= end; i++)
            {
                patId.Add(itrCode + date + i.ToString());
            }

            return patId;
        }

        /// <summary>
        /// 目标起始样本号改变时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStartNumgoal_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtStartNum.Text) && !string.IsNullOrEmpty(txtEndNum.Text) && !string.IsNullOrEmpty(txtStartNumgoal.Text))
            {
                try
                {
                    int minu = Convert.ToInt32(txtEndNum.Text) - Convert.ToInt32(txtStartNum.Text);
                    this.txtEndNumgoal.Text = (Convert.ToInt32(txtStartNumgoal.Text) + minu).ToString();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void chkCopySourceRemark_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopySourceRemark.Checked)
            {
                txtPatExp.Text = string.Empty;
            }
        }

        /// <summary>
        /// 写入日志记录
        /// </summary>
        /// <param name="msg">详细信息</param>
        /// <param name="ope">操作类型</param>
        private void WriteLogByBatchEdit(string msg, string ope)
        {
            ProxySysLog logProxy = new ProxySysLog();
            EntityLogLogin logMessage = new EntityLogLogin();
            logMessage.LogModule = this.Text;
            logMessage.LogLoginID = UserInfo.loginID;
            logMessage.LogIP = UserInfo.ip;
            logMessage.LogMAC = UserInfo.mac;
            logMessage.LogMessage = msg;
            logMessage.LogType = ope;
            logMessage.LogTime = DateTime.Now;

            logProxy.Service.NewSysLog(logMessage);
        }

        /// <summary>
        /// 是否显示时间修改控件
        /// </summary>
        /// <param name="bln"></param>
        private void VisibleDataControl(bool bln)
        {
            //检验时间
            layitemJYDate.Visibility = bln == true ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //一审时间
            layitemchkDate.Visibility = bln == true ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //二审时间
            layitemreportDate.Visibility = bln == true ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }
    }
}
