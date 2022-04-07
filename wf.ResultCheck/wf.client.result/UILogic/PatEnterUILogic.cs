using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using Lib.ProxyFactory;
using System.Windows.Forms;
using lis.client.control;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.Interface;
using dcl.root.logon;
using dcl.client.report;
using dcl.client.wcf;
using dcl.client.common;



using dcl.client.frame;
using dcl.entity;
using dcl.client.cache;
using System.Diagnostics;

namespace dcl.client.result.UILogic
{
    /// <summary>
    /// 审核/报告
    /// 病人资料、描述报告、细菌报告界面逻辑层
    /// </summary>
    public class PatEnterUILogic
    {
        string ItrDataType;
        public string ItrName { get; set; }
        public string Itr_ID { get; set; }


        /// <summary>
        /// 上一次操作ID
        /// </summary>
        public string strLastOperationID = string.Empty;

        /// <summary>
        /// 上一次操作密码
        /// </summary>
        public string strLastOperationPw = string.Empty;//密码

        /// <summary>
        /// 审核、删除默认前一个人工号
        /// </summary>
        private bool IsRecordLastOperationID = false;
        /// <summary>
        /// 报告(二审)是否记录前一个人的密码
        /// </summary>
        private bool IsRecordLastReportOperationPw = false;

        public string PatIdAndInNoAndName { get; set; }

        IPatientList parentForm = null;

        public PatEnterUILogic()
        {
            IsRecordLastOperationID = UserInfo.GetSysConfigValue("lab_default_id") == "是";
            IsRecordLastReportOperationPw = UserInfo.GetSysConfigValue("Lab_report_DefaultPw") == "是";
        }

        public PatEnterUILogic(IPatientList parent, string itr_datatype)
        {
            ItrDataType = itr_datatype;
            parentForm = parent;
            IsRecordLastOperationID = UserInfo.GetSysConfigValue("lab_default_id") == "是";
            IsRecordLastReportOperationPw = UserInfo.GetSysConfigValue("Lab_report_DefaultPw") == "是";
        }


        #region 审核、取消审核


        /// <summary>
        /// 批量审核
        /// </summary>
        public void AuditBatch(List<EntityPidReportMain> drs)
        {
            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - " + LocalSetting.Current.Setting.AuditWord, GetAuditPopedomCode(), "", "");
            frmCheck.operationCode = EnumOperationCode.Audit;

            List<string> patList = new List<string>();

            foreach (EntityPidReportMain dr in drs)
            {
                patList.Add(dr.RepId.ToString());
            }

            string currentPatID = string.Empty;
            if (!string.IsNullOrEmpty(PatIdAndInNoAndName) && PatIdAndInNoAndName.Split(':').Length > 0)
            {
                currentPatID = PatIdAndInNoAndName.Split(':')[0];
                if (patList.Contains(currentPatID))
                {
                    patList.Remove(currentPatID);
                    patList.Add(PatIdAndInNoAndName);
                }
                else
                {
                    currentPatID = string.Empty;
                }
            }

            EntityOperationResultList message = null;
            if (this.ItrDataType != "-1")
            {
                //根据仪器不同的数据类型选择不同的审核检查类型
                if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                    || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
                {
                    //系统配置：审核---双盲法AB仪器ID(A仪器id,B仪器id)
                    string DoubleBlind_AorB_ItrID = ConfigHelper.GetSysConfigValueWithoutLogin("DoubleBlind_AorB_ItrID");
                    //系统配置：审核---双盲法C仪器ID(C仪器id)
                    string DoubleBlind_C_ItrID = ConfigHelper.GetSysConfigValueWithoutLogin("DoubleBlind_C_ItrID");
                    //只有双盲法AB仪器输入一审检查前,验证身份
                    if (!string.IsNullOrEmpty(Itr_ID) && !string.IsNullOrEmpty(DoubleBlind_AorB_ItrID) && DoubleBlind_AorB_ItrID.Contains(this.Itr_ID)
                        || (!string.IsNullOrEmpty(Itr_ID) && !string.IsNullOrEmpty(DoubleBlind_C_ItrID) && DoubleBlind_C_ItrID == this.Itr_ID))
                    {
                        if (IsRecordLastOperationID)
                            frmCheck.txtLoginid.Text = strLastOperationID;

                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig == DialogResult.OK)
                        {
                            if (IsRecordLastOperationID)
                            {
                                strLastOperationID = frmCheck.OperatorID;
                            }

                            message = AuditCheck_CommonResult(patList, EnumOperationCode.Audit, dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName));
                            //如果有错误,则要重新身份验证,否则直接一审
                            if (message != null && message.FailedCount > 0)
                            {
                                frmCheck = new FrmCheckPassword("身份验证 - " + LocalSetting.Current.Setting.AuditWord, GetAuditPopedomCode(), "", "");
                                frmCheck.operationCode = EnumOperationCode.Audit;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        message = AuditCheck_CommonResult(patList, EnumOperationCode.Audit, null);
                    }
                }
                else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
                {
                    message = AuditCheck_BacResult(patList, EnumOperationCode.Audit);
                }
                else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
                {
                    message = AuditCheck_DescResult(patList, EnumOperationCode.Audit);
                }
                //else if (this.ItrDataType == LIS_Const.InstmtDataType.BabyFilter)
                //{
                //    message = AuditCheck_BabyFilter(patList, EnumOperationCode.Audit);
                //}
            }
            else//如果选中的病人资料中混合有细菌药敏和普通报告的
            {
                message = new EntityOperationResultList();

                Dictionary<string, List<EntityPidReportMain>> listItems = new Dictionary<string, List<EntityPidReportMain>>();

                foreach (EntityPidReportMain rowPat in drs)
                {
                    string itr_rep_flag = rowPat.ItrReportType.ToString();

                    bool existKey = false;
                    foreach (string key in listItems.Keys)
                    {
                        if (key == itr_rep_flag)
                        {
                            existKey = true;
                        }
                    }

                    if (!existKey)
                    {
                        listItems.Add(itr_rep_flag, new List<EntityPidReportMain>());
                    }
                    listItems[itr_rep_flag].Add(rowPat);
                }

                foreach (string key in listItems.Keys)
                {
                    if (key == LIS_Const.InstmtDataType.Eiasa
                        || key == LIS_Const.InstmtDataType.Normal)
                    {
                        List<string> patListTemp = new List<string>();
                        foreach (EntityPidReportMain dr in listItems[key])
                        {
                            patListTemp.Add(dr.RepId.ToString());
                        }
                        EntityOperationResultList messageTemp = AuditCheck_CommonResult(patListTemp, EnumOperationCode.Audit, null);

                        if (messageTemp != null && messageTemp.Count > 0)
                        {
                            message.AddRange(messageTemp);
                        }
                    }
                    else if (key == LIS_Const.InstmtDataType.Bacteria)
                    {
                        List<string> patListTemp = new List<string>();
                        foreach (EntityPidReportMain dr in listItems[key])
                        {
                            patListTemp.Add(dr.RepId.ToString());
                        }
                        EntityOperationResultList messageTemp = AuditCheck_BacResult(patListTemp, EnumOperationCode.Audit);
                        if (messageTemp != null && messageTemp.Count > 0)
                        {
                            message.AddRange(messageTemp);
                        }
                    }
                    else if (key == LIS_Const.InstmtDataType.Description)
                    {
                        List<string> patListTemp = new List<string>();
                        foreach (EntityPidReportMain dr in listItems[key])
                        {
                            patListTemp.Add(dr.RepId.ToString());
                        }
                        EntityOperationResultList messageTemp = AuditCheck_DescResult(patListTemp, EnumOperationCode.Audit);
                        if (messageTemp != null && messageTemp.Count > 0)
                        {
                            message.AddRange(messageTemp);
                        }
                    }
                }
            }

            //强制去除一审前检查到的超出参考值范围提示。
            if (UserInfo.GetSysConfigValue("Audit_EnforceOverRefNocheck") == "是")
            {
                if (message != null && message.FailedCount > 0)
                {
                    for (int i = 0; i < message.Count; i++)
                    {
                        EntityOperationResult OperationResult = message[i];
                        for (int i2 = 0; i2 < OperationResult.Message.Count; i2++)
                        {
                            EntityOperationError item = OperationResult.Message[i2];
                            if (item.ErrorCode == EnumOperationErrorCode.OverRef)
                            {
                                OperationResult.Message.Remove(item);
                                i2--;
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(PatIdAndInNoAndName) && !string.IsNullOrEmpty(currentPatID))
            {
                patList.Remove(PatIdAndInNoAndName);
                patList.Add(currentPatID);
            }

            if (message != null && message.FailedCount > 0)//存在审核检查失败的记录
            {
                //显示提示窗口并询问用户是否继续
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(message);
                if (resultviwer.ShowDialog() == DialogResult.OK)
                {
                    //获取再次被选中的记录
                    patList = resultviwer.GetSelectedID();
                    if (IsRecordLastOperationID)
                        frmCheck.txtLoginid.Text = strLastOperationID;

                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        if (!UserInfo.isAdmin && UserInfo.GetSysConfigValue("Audit_CanAuditWhenNotItrAuth") == "否")
                        {
                            bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(drs[0].RepItrId.ToString(), EnumOperationCode.Audit, frmCheck.OperatorID);
                            if (!ok)
                            {
                                lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                                return;
                            }
                        }

                        if (IsRecordLastOperationID)
                        {
                            strLastOperationID = frmCheck.OperatorID;
                        }

                        //更新再次选中的记录状态
                        new ProxyPidReportMainAudit().Service.Audit(patList, dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, frmCheck.Pat_i_code, frmCheck.OperatorName));

                        //记录CA使用情况
                        if ((frmCheck.strCASignMode == "河池市人民医院" || frmCheck.strCASignMode == "广东医学院附属医院") && frmCheck.CAUserInfo != null && !string.IsNullOrEmpty(frmCheck.CAUserInfo.strCerId))
                        {
                            ProxyUserManage proxy = new ProxyUserManage();
                            List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                            foreach (string pat_id in patList)
                            {
                                EntityCaSign drCaSign = new EntityCaSign();

                                drCaSign.CaCerId = frmCheck.CAUserInfo.strCerId;
                                if (frmCheck.strCASignMode == "河池市人民医院" && !frmCheck.CAUserInfo.strUserList.Contains(frmCheck.CAUserInfo.strCerId))
                                {
                                    drCaSign.CaEntityId = frmCheck.CAUserInfo.strUserList;
                                }
                                drCaSign.CaDate = ServerDateTime.GetServerDateTime();
                                drCaSign.CaLoginId = frmCheck.OperatorID;
                                drCaSign.CaName = frmCheck.OperatorName;
                                drCaSign.CaEvent = "检验报告";
                                drCaSign.CaRemark = string.Format("{0} - {1} 检验报告[{2}]", frmCheck.OperatorID, frmCheck.OperatorName, pat_id);
                                dtCaSign.Add(drCaSign);
                            }
                            proxy.Service.InsertCaSign(dtCaSign);
                        }

                        //更新再次选中的记录在列表中的状态
                        foreach (string pat_id in patList)
                        {
                            EntityPidReportMain dr = GetRowByPatId(drs, pat_id);
                            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited);
                            dr.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                        }


                        if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                        {
                            parentForm.RefreshPatients();
                            parentForm.SelectAllPatient(false);
                        }
                        lis.client.control.MessageDialog.ShowAutoCloseDialog(LocalSetting.Current.Setting.AuditWord + "成功");
                    }
                }
            }
            else//全部检查通过,弹出身份验证窗
            {
                DialogResult dig = DialogResult.Cancel;
                if (string.IsNullOrEmpty(frmCheck.OperatorID))
                {
                    if (IsRecordLastOperationID)
                        frmCheck.txtLoginid.Text = strLastOperationID;
                    dig = frmCheck.ShowDialog();

                }
                else
                {
                    //如果一审检验前，有身份验证过,并且没有提示信息等,则不需要再身份验证
                    dig = DialogResult.OK;
                }

                if (dig == DialogResult.OK)
                {
                    if (IsRecordLastOperationID)
                    {
                        strLastOperationID = frmCheck.OperatorID;
                    }
                    if (!UserInfo.isAdmin && UserInfo.GetSysConfigValue("Audit_CanAuditWhenNotItrAuth") == "否")
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(drs[0].RepItrId.ToString(), EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return;
                        }

                    }
                    new ProxyPidReportMainAudit().Service.Audit(patList, dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, frmCheck.Pat_i_code, frmCheck.OperatorName));

                    //记录CA使用情况
                    if ((frmCheck.strCASignMode == "河池市人民医院" || frmCheck.strCASignMode == "广东医学院附属医院") && frmCheck.CAUserInfo != null && !string.IsNullOrEmpty(frmCheck.CAUserInfo.strCerId))
                    {
                        ProxyUserManage proxy = new ProxyUserManage();
                        List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                        foreach (string pat_id in patList)
                        {
                            EntityCaSign drCaSign = new EntityCaSign();

                            drCaSign.CaCerId = frmCheck.CAUserInfo.strCerId;
                            if (frmCheck.strCASignMode == "河池市人民医院" && !frmCheck.CAUserInfo.strUserList.Contains(frmCheck.CAUserInfo.strCerId))
                            {
                                drCaSign.CaEntityId = frmCheck.CAUserInfo.strUserList;
                            }
                            drCaSign.CaLoginId = frmCheck.OperatorID;
                            drCaSign.CaName = frmCheck.OperatorName;
                            drCaSign.CaEvent = "检验报告";
                            drCaSign.CaRemark = string.Format("{0} - {1} 检验报告[{2}]", frmCheck.OperatorID, frmCheck.OperatorName, pat_id);
                            dtCaSign.Add(drCaSign);
                        }
                        proxy.Service.InsertCaSign(dtCaSign);
                    }


                    foreach (EntityPidReportMain drPat in drs)
                    {
                        drPat.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited);
                        drPat.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                    }

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        parentForm.RefreshPatients();
                        parentForm.SelectAllPatient(false);
                    }
                    lis.client.control.MessageDialog.ShowAutoCloseDialog(LocalSetting.Current.Setting.AuditWord + "成功");
                }
            }
        }

        /// <summary>
        /// 批量发送中期报告
        /// </summary>
        /// <param name="drs"></param>
        public void MidReportBatch(List<EntityPidReportMain> drs)
        {
            FrmCheckPassword frm = new FrmCheckPassword();

            List<string> patList = new List<string>();

            foreach (EntityPidReportMain dr in drs)
            {
                patList.Add(dr.RepId.ToString());
            }

            if (frm.ShowDialog() == DialogResult.OK)//身份验证
            {
                EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();

                caller.LoginName = frm.OperatorName;
                caller.LoginID = frm.OperatorID;
                caller.Remarks = "ip:" + dcl.common.IPUtility.GetIP();//获取本机ip;

                EntityOperationResultList message = new EntityOperationResultList();//new ProxyPidReportMainAudit().Service.CommonMidReport(patList.ToArray(), EnumOperationCode.MidReport, caller, true);

                if (message != null && message.FailedCount > 0)//存在检查失败的记录
                {
                    List<string> patList2 = new List<string>();
                    //显示提示窗口并询问用户是否继续
                    AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(message, EnumOperationCode.MidReport);
                    if (resultviwer.ShowDialog() == DialogResult.OK)
                    {
                        //获取再次被选中的记录
                        patList2 = resultviwer.GetSelectedID();
                        if (patList2 != null && patList2.Count > 0)
                        {
                            //new ProxyPidReportMainAudit().Service.CommonMidReport(patList2.ToArray(), EnumOperationCode.MidReport, caller, false);//不检查,直接操作
                        }
                    }

                    if (patList.Count != message.FailedCount || (patList2 != null && patList2.Count > 0))
                    {
                        //if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                        //{
                        parentForm.RefreshPatients();
                        parentForm.SelectAllPatient(false);
                        //}
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("发送成功");
                    }
                }
                else//如果全部通过
                {

                    parentForm.RefreshPatients();
                    parentForm.SelectAllPatient(false);

                    lis.client.control.MessageDialog.ShowAutoCloseDialog("发送成功");
                }
            }
        }


        /// <summary>
        /// 批量取消审核
        /// </summary>
        /// <param name="drs"></param>
        public void UndoAuditBatch(List<EntityPidReportMain> drs, string userLoginID, string userName)
        {
            DialogResult dig = DialogResult.Abort;
            FrmCheckPassword frmCheck = null;

            if (userLoginID == null)
            {
                //身份验证
                frmCheck = new FrmCheckPassword("身份验证 - 取消" + LocalSetting.Current.Setting.AuditWord, GetUndoAuditPopedomCode(), "", "");
                frmCheck.operationCode = EnumOperationCode.UndoAudit;
                if (IsRecordLastOperationID)
                    frmCheck.txtLoginid.Text = strLastOperationID;
                dig = frmCheck.ShowDialog();

            }

            if (dig == DialogResult.OK || userLoginID != null)
            {
                if (IsRecordLastOperationID && frmCheck != null)
                {
                    strLastOperationID = frmCheck.OperatorID;
                }

                if (userLoginID == null)
                {
                    userLoginID = frmCheck.OperatorID;
                    userName = frmCheck.OperatorName;
                }

                List<string> patList = new List<string>();
                foreach (EntityPidReportMain dr in drs)//只有状态为已审核的病人资料才加入到反审列表中
                {
                    patList.Add(dr.RepId.ToString());
                }

                EntityOperationResultList result = new ProxyPidReportMainAudit().Service.UndoAudit(patList, dcl.client.common.Util.ToCallerInfo(userLoginID, string.Empty, userName));

                if (result.FailedCount == 0)
                {
                    foreach (string pat_id in patList)
                    {
                        EntityPidReportMain dr = GetRowByPatId(drs, pat_id);
                        dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural);
                        dr.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                    }

                    if (dcl.client.frame.UserInfo.GetSysConfigValue("OneStepCancelReport") != "是")
                    {
                        if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                        {
                            parentForm.RefreshPatients();
                            parentForm.SelectAllPatient(false);
                        }
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("取消" + LocalSetting.Current.Setting.AuditWord + "成功");
                    }
                }
                else
                {
                    EntityOperationResult[] results = result.GetSuccessOperations();
                    foreach (EntityOperationResult opr in results)
                    {
                        EntityPidReportMain dr = GetRowByPatId(drs, opr.Data.Patient.RepId);
                        if (dr != null)
                        {
                            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural);
                            dr.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                        }
                    }

                    if (dcl.client.frame.UserInfo.GetSysConfigValue("OneStepCancelReport") != "是")
                    {
                        if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                        {
                            parentForm.RefreshPatients();
                            parentForm.SelectAllPatient(false);
                        }

                        FrmOpResultViwer viewer = new FrmOpResultViwer(result);
                        viewer.ShowDialog();
                    }
                }
            }
        }

        #endregion

        #region 审核前检查

        /// <summary>
        /// 审核检查:普通病人资料
        /// </summary>
        /// <param name="patIDList"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        private EntityOperationResultList AuditCheck_CommonResult(IEnumerable<string> patIDList, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            if (caller == null)
            {
                caller = dcl.client.common.Util.ToCallerInfo();
                caller.OperationName = UserInfo.HaveFunctionByCode("PatInput_SaveHistoryResult").ToString();
            }
            else
            {
                //角色权限：检验报告允许保存历史结果差异
                caller.OperationName = UserInfo.HaveFunctionByCode("PatInput_SaveHistoryResult").ToString();
            }

            caller.UseAuditRule = false;


            EntityOperationResultList message = new ProxyPidReportMainAudit().Service.CommonAuditCheck(patIDList, checkType, caller);
            return message;
        }

        /// <summary>
        /// 审核检查:描述报告病人资料
        /// </summary>
        /// <param name="patIDList"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        private EntityOperationResultList AuditCheck_DescResult(IEnumerable<string> patIDList, EnumOperationCode checkType)
        {
            EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo();
            caller.OperationName = UserInfo.userName;
            caller.UserID = UserInfo.loginID;
            EntityOperationResultList message = new ProxyPidReportMainAudit().Service.DesctAuditCheck(patIDList, checkType, caller);
            return message;
        }

        /// <summary>
        /// 审核检查:细菌报告病人资料
        /// </summary>
        /// <param name="patIDList"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        private EntityOperationResultList AuditCheck_BacResult(IEnumerable<string> patIDList, EnumOperationCode checkType)
        {
            return null;
        }


        #endregion

        #region 报告、取消报告

        /// <summary>
        /// 批量报告
        /// </summary>
        /// <param name="drs"></param>
        public bool ReoprtBatch(List<EntityPidReportMain> drs)
        {
            bool success = false;

            //身份验证
            string titleMessage = "身份验证 - " + LocalSetting.Current.Setting.ReportWord;
            if (UserInfo.GetSysConfigValue("Audit_UKeyType") != "不使用")
            {
                titleMessage += "(需要密钥验证)";
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword(titleMessage, GetReportPopedomCode(), "", "");
            frmCheck.operationCode = EnumOperationCode.Report;
            List<string> patlist = new List<string>();
            foreach (EntityPidReportMain dr in drs)
            {
                patlist.Add(dr.RepId.ToString());
            }

            //根据仪器不同的数据类型选择不同的审核检查类型
            EntityOperationResultList message = null;
            if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
            {
                message = AuditCheck_CommonResult(patlist, EnumOperationCode.Report, null);
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
            {
                message = AuditCheck_BacResult(patlist, EnumOperationCode.Report);
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
            {
                message = AuditCheck_DescResult(patlist, EnumOperationCode.Report);
            }

            bool blnRes = true;
            if (message != null && message.FailedCount > 0)
            {
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(message, EnumOperationCode.Report);
                if (resultviwer.ShowDialog() == DialogResult.OK)
                {
                    patlist = resultviwer.GetSelectedID();
                    List<string> unSendUrgPatIDlist = resultviwer.GetSelectedunSendUrgID();//获取不发送危急提示的id

                    if (UserInfo.GetSysConfigValue("Audit_HintRowCount") == "是")//审核报告时是否提示审核条数
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("当次将审核[" + patlist.Count.ToString() + "]条记录", 2);

                    DialogResult dig = DialogResult.Cancel;
                    if (IsRecordLastOperationID 
                        && IsRecordLastReportOperationPw
                        && strLastOperationID != string.Empty 
                        && strLastOperationPw != string.Empty)
                    {
                        bool flag = frmCheck.Valid(strLastOperationID, strLastOperationPw);
                        if (!flag)
                            dig = frmCheck.ShowDialog();
                        else
                            dig = DialogResult.OK;
                    }
                    else
                    {
                        frmCheck.txtLoginid.Text = strLastOperationID;
                        frmCheck.ActiveControl = frmCheck.txtPassword;
                        dig = frmCheck.ShowDialog();
                    }

                    if (dig == DialogResult.OK)
                    {
                        if (!UserInfo.isAdmin && UserInfo.GetSysConfigValue("Audit_CanAuditWhenNotItrAuth") == "否")
                        {
                            bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(drs[0].RepItrId.ToString(), EnumOperationCode.Audit, frmCheck.OperatorID);
                            if (!ok)
                            {
                                lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                                return false;
                            }
                        }

                        if (IsRecordLastOperationID)
                        {
                            strLastOperationID = frmCheck.OperatorID;
                        }

                        if (IsRecordLastReportOperationPw)
                        {
                            strLastOperationPw = frmCheck.PassWord;
                        }

                        //*******************************************************************
                        //判断是否开启没有条码号的的检验报告不能审核报告打印
                        bool isNeedCheckNoBarcode = false;
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarcodeNeedAuditCheek") == "是")
                        {
                            string itrExList =
                                ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeAuditCheckItrExList");

                            if (string.IsNullOrEmpty(itrExList) || !itrExList.Contains(Itr_ID))
                            {
                                isNeedCheckNoBarcode = true;
                            }

                        }

                        if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotAuditReportPrintOnNoBC") == "是" || isNeedCheckNoBarcode)
                        {
                            int count = 0;
                            if (!canReportOnNoBarCode(frmCheck, isNeedCheckNoBarcode ? "NoBarcode_CanAudit" : "CanNotAuditReportPrintInNoBC"))
                            {
                                for (int i = 0; i < drs.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(drs[i].RepBarCode.ToString()))
                                    {
                                        patlist.Remove(patlist[i]);
                                        count++;
                                    }
                                }
                                lis.client.control.MessageDialog.Show(string.Format("共有{1}条记录没有条码号，用户名：[{0}]无权报告没有条码号的报告！", frmCheck.OperatorID, count));
                            }
                        }

                        //*******************************************************************
                        message = new ProxyPidReportMainAudit().Service.Report(patlist, dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, frmCheck.Pat_i_code, frmCheck.OperatorName, unSendUrgPatIDlist, frmCheck.OperatorSftId));

                        if (message != null && message.FailedCount > 0)
                        {
                            AuditCheckResultViwer resultviwerShow = new AuditCheckResultViwer(message, EnumOperationCode.Report);
                            if (resultviwerShow.ShowDialog() != DialogResult.OK)
                            {
                                blnRes = false;
                            }
                        
                        }

                        #region 旧CA

                        //DataTable dtbCASignContent = null;
                        //if (String.Equals(UserInfo.GetSysConfigValue("Audit_UKeyType"), "NETCA", StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    if (String.Equals(UserInfo.GetSysConfigValue("CASignatureMode"), GDNetCA.CA_FLAG, StringComparison.CurrentCultureIgnoreCase))
                        //    {
                        //    }
                        //    else if (String.Equals(UserInfo.GetSysConfigValue("CASignatureMode"), GDCAReader.CA_FLAG, StringComparison.CurrentCultureIgnoreCase))
                        //    {
                        //        GDCASign(drs, patlist, message, new StringBuilder(""), frmCheck);
                        //    }
                        //    else
                        //    {
                        //        foreach (EntityOperationResult operResult in message)
                        //        {
                        //            if (operResult.Success == true)
                        //            {
                        //                EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                        //                dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                        //                dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                        //                //CA电子签名内容
                        //                if (operResult.ReturnResult != null && operResult.ReturnResult.Tables.Count > 0 && operResult.ReturnResult.Tables.Contains("CASignContent"))
                        //                {
                        //                    if (dtbCASignContent == null)
                        //                    {
                        //                        dtbCASignContent = operResult.ReturnResult.Tables["CASignContent"];
                        //                    }
                        //                    else
                        //                    {
                        //                        dtbCASignContent.Merge(operResult.ReturnResult.Tables["CASignContent"]);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        //CA电子签名
                        //        this.CASignature(dtbCASignContent);

                        //        //记录CA使用情况
                        //        if ((frmCheck.strCASignMode == "河池市人民医院" || frmCheck.strCASignMode == "广东医学院附属医院") && frmCheck.CAUserInfo != null && !string.IsNullOrEmpty(frmCheck.CAUserInfo.strCerId))
                        //        {
                        //            ProxyUserManage proxy = new ProxyUserManage();
                        //            List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                        //            foreach (EntityOperationResult operResult in message)
                        //            {
                        //                EntityCaSign drCaSign = new EntityCaSign();

                        //                drCaSign.CaCerId = frmCheck.CAUserInfo.strCerId;
                        //                if (frmCheck.strCASignMode == "河池市人民医院" && !frmCheck.CAUserInfo.strUserList.Contains(frmCheck.CAUserInfo.strCerId))
                        //                {
                        //                    drCaSign.CaEntityId = frmCheck.CAUserInfo.strUserList;
                        //                }
                        //                drCaSign.CaLoginId = frmCheck.OperatorID;
                        //                drCaSign.CaName = frmCheck.OperatorName;
                        //                drCaSign.CaEvent = "发布报告";
                        //                drCaSign.CaRemark = string.Format("{0} - {1} 发布报告[{2}]", frmCheck.OperatorID, frmCheck.OperatorName, operResult.Data.Patient.RepId);
                        //                dtCaSign.Add(drCaSign);
                        //            }
                        //            proxy.Service.InsertCaSign(dtCaSign);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    foreach (EntityOperationResult operResult in message)
                        //    {
                        //        if (operResult.Success == true)
                        //        {
                        //            EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                        //            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                        //            dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                        //        }
                        //    }
                        //}
                        #endregion

                        SaveCASignInfo(frmCheck, message);

                        foreach (EntityOperationResult operResult in message)
                        {
                            if (operResult.Success == true)
                            {
                                EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                                dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                                dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                            }
                        }
                        success = true;
                        if (dcl.client.frame.UserInfo.GetSysConfigValue("PrintOnReport") != "是")
                        {
                            if (blnRes)
                            {
                                lis.client.control.MessageDialog.ShowAutoCloseDialog(LocalSetting.Current.Setting.ReportWord + "成功");
                            }

                        }
                    }
                }
            }
            else
            {
                if (UserInfo.GetSysConfigValue("Audit_HintRowCount") == "是")//审核报告时是否提示审核条数
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("当次将审核[" + drs.Count.ToString() + "]条记录", 2);

                DialogResult dig = DialogResult.Cancel;
                if (IsRecordLastOperationID
                    && IsRecordLastReportOperationPw
                    && strLastOperationID != string.Empty
                    && strLastOperationPw != string.Empty)
                {
                    bool flag = frmCheck.Valid(strLastOperationID, strLastOperationPw);
                    if (!flag)
                        dig = frmCheck.ShowDialog();
                    else
                        dig = DialogResult.OK;
                }
                else
                {
                    frmCheck.txtLoginid.Text = strLastOperationID;
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                    dig = frmCheck.ShowDialog();
                }

                if (dig == DialogResult.OK)
                {
                    if (!UserInfo.isAdmin && UserInfo.GetSysConfigValue("Audit_CanAuditWhenNotItrAuth") == "否")
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(drs[0].RepItrId.ToString(), EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return false;
                        }
                    }


                    if (IsRecordLastOperationID)
                    {
                        strLastOperationID = frmCheck.OperatorID;
                    }

                    if (IsRecordLastReportOperationPw)
                    {
                        strLastOperationPw = frmCheck.PassWord;
                    }

                    //*******************************************************************
                    //判断是否开启没有条码号的的检验报告不能审核报告打印
                    bool isNeedCheckNoBarcode = false;
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarcodeNeedAuditCheek") == "是")
                    {
                        string itrExList =
                            ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeAuditCheckItrExList");

                        if (string.IsNullOrEmpty(itrExList) || !itrExList.Contains(Itr_ID))
                        {
                            isNeedCheckNoBarcode = true;
                        }
                    }

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotAuditReportPrintOnNoBC") == "是" || isNeedCheckNoBarcode)
                    {
                        int count = 0;
                        if (!canReportOnNoBarCode(frmCheck, isNeedCheckNoBarcode ? "NoBarcode_CanAudit" : "CanNotAuditReportPrintInNoBC"))
                        {
                            for (int i = 0; i < drs.Count; i++)
                            {
                                if (string.IsNullOrEmpty(drs[i].RepBarCode.ToString()))
                                {
                                    //drs.Remove(drs[i]);
                                    patlist.Remove(patlist[i]);
                                    count++;
                                }
                            }
                            lis.client.control.MessageDialog.Show(string.Format("共有{1}条记录没有条码号，用户名：[{0}]无权报告没有条码号的报告！", frmCheck.OperatorID, count));
                            return false;
                        }
                    }

                    //报告(二审)时允许修改一审人
                    string userid = string.Empty;
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("report_Allowedit_auditercode") == "是")
                    {
                        userid = frmCheck.Pat_i_code;
                    }

                    message = new ProxyPidReportMainAudit().Service.Report(patlist,
                        dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, userid,
                        frmCheck.OperatorName, frmCheck.OperatorSftId));


                    if (message != null && message.FailedCount > 0)
                    {
                        AuditCheckResultViwer resultviwerShow = new AuditCheckResultViwer(message, EnumOperationCode.Report);
                        if (resultviwerShow.ShowDialog() != DialogResult.OK)
                        {
                            blnRes = false;
                        }

                        patlist = resultviwerShow.GetSelectedID();
                    }

                    #region 旧CA
                    //DataTable dtbCASignContent = null;
                    ////增加新的判断分支如果为网证通，执行下面的代码，不是网证通，按以前的方法走。
                    //if (String.Equals(UserInfo.GetSysConfigValue("Audit_UKeyType"), "NETCA", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    if (String.Equals(UserInfo.GetSysConfigValue("CASignatureMode"), GDNetCA.CA_FLAG, StringComparison.CurrentCultureIgnoreCase))
                    //    {
                    //    }
                    //    else if (String.Equals(UserInfo.GetSysConfigValue("CASignatureMode"), GDCAReader.CA_FLAG, StringComparison.CurrentCultureIgnoreCase))
                    //    {
                    //        GDCASign(drs, patlist, message, new StringBuilder(""), frmCheck);
                    //    }
                    //    //结束 2013-09-16
                    //    else
                    //    {
                    //        foreach (EntityOperationResult operResult in message)
                    //        {
                    //            if (operResult.Success == true)
                    //            {
                    //                EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                    //                dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                    //                dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;

                    //                //CA电子签名内容
                    //                if (operResult.ReturnResult != null && operResult.ReturnResult.Tables.Count > 0 && operResult.ReturnResult.Tables.Contains("CASignContent"))
                    //                {
                    //                    if (dtbCASignContent == null)
                    //                    {
                    //                        dtbCASignContent = operResult.ReturnResult.Tables["CASignContent"];
                    //                    }
                    //                    else
                    //                    {
                    //                        dtbCASignContent.Merge(operResult.ReturnResult.Tables["CASignContent"]);
                    //                    }
                    //                }

                    //            }
                    //        }
                    //        //CA电子签名报告单内容
                    //        this.CASignature(dtbCASignContent);

                    //        //记录CA使用情况
                    //        if ((frmCheck.strCASignMode == "河池市人民医院" || frmCheck.strCASignMode == "广东医学院附属医院") && frmCheck.CAUserInfo != null && !string.IsNullOrEmpty(frmCheck.CAUserInfo.strCerId))
                    //        {
                    //            ProxyUserManage proxy = new ProxyUserManage();
                    //            List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                    //            foreach (EntityOperationResult operResult in message)
                    //            {
                    //                EntityCaSign drCaSign = new EntityCaSign();
                    //                drCaSign.CaCerId = frmCheck.CAUserInfo.strCerId;
                    //                if (frmCheck.strCASignMode == "河池市人民医院" && !frmCheck.CAUserInfo.strUserList.Contains(frmCheck.CAUserInfo.strCerId))
                    //                {
                    //                    drCaSign.CaEntityId = frmCheck.CAUserInfo.strUserList;
                    //                }
                    //                drCaSign.CaLoginId = frmCheck.OperatorID;
                    //                drCaSign.CaName = frmCheck.OperatorName;
                    //                drCaSign.CaEvent = "发布报告";
                    //                drCaSign.CaRemark = string.Format("{0} - {1} 发布报告[{2}]", frmCheck.OperatorID, frmCheck.OperatorName, operResult.Data.Patient.RepId);

                    //                dtCaSign.Add(drCaSign);
                    //            }
                    //            proxy.Service.InsertCaSign(dtCaSign);
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //    foreach (EntityOperationResult operResult in message)
                    //    {
                    //        if (operResult.Success == true)
                    //        {
                    //            EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                    //            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                    //            dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                    //        }
                    //    }
                    //}

                    #endregion

                    this.SaveCASignInfo(frmCheck, message);

                    foreach (EntityOperationResult operResult in message)
                    {
                        if (operResult.Success == true)
                        {
                            EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                            dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                        }
                    }
                    success = true;
                    if (dcl.client.frame.UserInfo.GetSysConfigValue("PrintOnReport") != "是")
                    {
                        if (blnRes)
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog(LocalSetting.Current.Setting.ReportWord + "成功");
                        }

                    }
                }
            }

            return success;
        }


        private void GDCASign(List<EntityPidReportMain> drs, List<string> patlist, EntityOperationResultList message, StringBuilder logMsg,
                               FrmCheckPassword frmCheck)
        {
            try
            {

                DataTable dtbCASignContent;
                dtbCASignContent = new DataTable();
                dtbCASignContent.Columns.Add("pat_id");
                dtbCASignContent.Columns.Add("sourceContent");
                dtbCASignContent.Columns.Add("SignContent");
                dtbCASignContent.Columns.Add("SignerImage");
                dtbCASignContent.Columns.Add("signerType");
                dtbCASignContent.Columns.Add("msg_checker_name");
                dtbCASignContent.Columns.Add("msg_checker_id");
                dtbCASignContent.Columns.Add("msg_content");

                Dictionary<string, bool> needSignPatList = new Dictionary<string, bool>();

                foreach (string patid in patlist)
                {
                    if (!needSignPatList.ContainsKey(patid))
                    {
                        needSignPatList.Add(patid, false);
                    }
                }

                string userCert = string.Empty;

                foreach (EntityOperationResult operResult in message)
                {
                    if (operResult.Success)
                    {
                        EntityPidReportMain dr = GetRowByPatId(drs, operResult.Data.Patient.RepId);
                        dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported);
                        dr.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                        //电子签名内容
                        DataRow r = dtbCASignContent.NewRow();
                        r["sourceContent"] = "";
                        r["pat_id"] = operResult.Data.Patient.RepId;
                        string signData = new GDCAReader().GDCASign(frmCheck.PassWord, r["sourceContent"] as string, ref userCert);

                        if (!string.IsNullOrEmpty(signData))
                        {
                            r["signContent"] = signData;
                            r["msg_content"] = userCert;
                        }
                        else
                        {
                            Logger.WriteException(this.GetType().Name, "CA操作失败", operResult.Data.Patient.RepId);
                            continue;
                        }
                        r["msg_checker_name"] = frmCheck.OperatorName;
                        r["msg_checker_id"] = frmCheck.OperatorID;
                        r["signerType"] = "GDCA_PATIENTS";
                        dtbCASignContent.Rows.Add(r);
                    }
                }
                dtbCASignContent.TableName = "patients_ext";
                new ProxyPidReportMainAudit().Service.InsertReportCASignature(dtbCASignContent);


            }
            catch (Exception EX)
            {
                Logger.WriteException(this.GetType().Name, "CA操作失败", EX.ToString());
            }
        }

        List<EntityPrintData> PrintData { get; set; }
        string OperatorID { get; set; }
        private void SendPDFData()
        {
            try
            {
                if (PrintData != null && PrintData.Count > 0)
                {
                    FrmReportPrint pForm = new FrmReportPrint();
                    Dictionary<string, MemoryStream> dic = pForm.ExportToPDF(PrintData);
                    Dictionary<string, byte[]> dicPdfSign = new NetcaSign().PdfSign(dic, OperatorID);
                    List<string> patlist = new List<string>();
                    List<byte[]> bytes = new List<byte[]>();
                    foreach (string key in dicPdfSign.Keys)
                    {
                        patlist.Add(key);
                        bytes.Add(dicPdfSign[key]);
                    }
                    if (patlist.Count == 0)
                    {
                        Logger.WriteException(this.GetType().Name, "签章PDF失败", "请确保USBKEY有签章信息与网政通客户端版本为2.3.12或以上");
                        return;
                    }
                    string wsdlAddress = ConfigHelper.GetSysConfigValueWithoutLogin("PDFUpLoadAddress");

                    WSProxyFactory factory = new WSProxyFactory(wsdlAddress);
                    object ret = factory.Invoke("UploadMutliReport",
                        new object[]
                            {
                                patlist.ToArray()
                                ,bytes.ToArray()
                                ,"PDF"
                                ,"Lis"
                            });
                    if (!(bool)ret)
                    {
                        Logger.WriteException(this.GetType().Name, "上传PDF失败", "请查看归档web服务日志");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "上传PDF失败", ex.ToString());
            }

        }


        /// <summary>
        /// CA电子签名报告单内容
        /// </summary>
        /// <param name="p_dtbCASignContent">报告单内容</param>
        private void CASignature(DataTable p_dtbCASignContent)
        {
            //CA电子签名报告单内容
            if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("CASignatureMode")) && UserInfo.GetSysConfigValue("CASignatureMode") != "无" && UserInfo.CASignMode)
            {
                Lis.Client.CASign.ICASignature CASign = Lis.Client.CASign.CASignatureFactory.CreateCASignature(UserInfo.GetSysConfigValue("CASignatureMode"));
                bool blnRes = CASign.Sign(ref p_dtbCASignContent);
                if (blnRes)
                {
                    new ProxyPidReportMainAudit().Service.InsertReportCASignature(p_dtbCASignContent);
                }

            }
        }

        /// <summary>
        /// 批量取消报告
        /// </summary>
        /// <param name="drs"></param>
        public void UndoReoprtBatch(List<EntityPidReportMain> drs)
        {
            bool CancelPrintReportPower = UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是";

            string powerName = CancelPrintReportPower ? "FrmPatEnter_CancelPrintReportPower" : string.Empty;

            string undoReportRemark = string.Empty;
            if (UserInfo.GetSysConfigValue("Lab_UndoReportRemark") == "是")
            {
                FrmUndoRepotReson frmReson = new FrmUndoRepotReson();
                if (frmReson.ShowDialog() == DialogResult.OK)
                {
                    undoReportRemark = frmReson.strRemark;
                }
                else
                    return;
            }

            //身份验证
            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消" + LocalSetting.Current.Setting.ReportWord, GetUndoReportPopedomCode(), "", "", powerName);
            frmCheck.operationCode = EnumOperationCode.UndoReport;

            List<string> patList = new List<string>();
            List<string> patReportList = new List<string>();

            foreach (EntityPidReportMain dr in drs)//只有状态为已审核的病人资料才加入到反审列表中
            {
                if (dr.RepStatus != null && dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                {
                    patReportList.Add(dr.RepId.ToString());
                }
                patList.Add(dr.RepId.ToString());
            }


            #region 取消报告前检查

            if (this.ItrDataType == LIS_Const.InstmtDataType.Normal
                || this.ItrDataType == LIS_Const.InstmtDataType.Eiasa)//若为普通或酶标报告,则取消前检查
            {
                EntityOperationResultList resultCheck;
                //系统配置：取消审核最大小时
                string StrUndoAudit_Second_UndoExpiredHours = UserInfo.GetSysConfigValue("UndoAudit_Second_UndoExpiredHours");

                //取消二审前检查
                if (!string.IsNullOrEmpty(StrUndoAudit_Second_UndoExpiredHours) && StrUndoAudit_Second_UndoExpiredHours != "0")
                {
                    if (IsRecordLastOperationID && strLastOperationID != string.Empty)
                    {
                        frmCheck.txtLoginid.Text = strLastOperationID;
                        frmCheck.ActiveControl = frmCheck.txtPassword;
                    }
                    //如果有开(系统配置--取消审核最大小时),则检查前要身份验证
                    DialogResult digTemp = frmCheck.ShowDialog();

                    if (digTemp == DialogResult.OK)
                    {
                        if (IsRecordLastOperationID)
                        {
                            strLastOperationID = frmCheck.OperatorID;
                        }

                        resultCheck = new ProxyPidReportMainAudit().Service.CommonUndoReoprtCheck(patList, EnumOperationCode.UndoReport, dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName));
                        //如果检查有返回信息,则清除操作者验证信息,需再一次身份验证
                        if (resultCheck != null && resultCheck.FailedCount > 0)
                        {
                            frmCheck = new FrmCheckPassword("身份验证 - 取消" + LocalSetting.Current.Setting.ReportWord, GetUndoReportPopedomCode(), "", "", powerName);
                            frmCheck.operationCode = EnumOperationCode.UndoReport;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    resultCheck = new ProxyPidReportMainAudit().Service.CommonUndoReoprtCheck(patList, EnumOperationCode.UndoReport, null);
                }

                //若存在检查不通过的,则重新选择项再继续
                if (resultCheck != null && resultCheck.FailedCount > 0)
                {
                    //显示审核检查提示窗口
                    AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(resultCheck, EnumOperationCode.UndoReport);

                    if (resultviwer.ShowDialog() == DialogResult.OK)//点击了"继续"
                    {
                        patList = resultviwer.GetSelectedID();//重新获取勾选的项
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)//如果是描述报告,取消前检查
            {
                //取消报告前检查
                EntityOperationResultList resultCheck = new ProxyPidReportMainAudit().Service.BatchUndoReportCheck(patList, "UndoReport");
                //若存在检查不通过的,则重新选择项再继续
                if (resultCheck != null && resultCheck.FailedCount > 0)
                {
                    //显示审核检查提示窗口
                    AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(resultCheck, EnumOperationCode.UndoReport);

                    if (resultviwer.ShowDialog() == DialogResult.OK)//点击了"继续"
                    {
                        patList = resultviwer.GetSelectedID();//重新获取勾选的项
                    }
                    else
                    {
                        return;
                    }
                }
            }

            #endregion

            DialogResult dig = DialogResult.OK;
            //如果操作者验证信息为空,则要再次验证身份
            if (string.IsNullOrEmpty(frmCheck.OperatorID))
            {
                if (IsRecordLastOperationID && strLastOperationID != string.Empty)
                {
                    frmCheck.txtLoginid.Text = strLastOperationID;
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                dig = frmCheck.ShowDialog();
            }

            if (dig == DialogResult.OK)
            {
                if (CancelPrintReportPower && !frmCheck.Power)
                {
                    if (patReportList.Count == 0)
                    {
                        lis.client.control.MessageDialog.Show("您没有权限反审已打印的标本！");
                        return;
                    }
                    if (patReportList.Count < drs.Count)
                    {
                        lis.client.control.MessageDialog.Show("您没有权限反审已打印的标本！将过滤掉已打印过的标本！");
                    }
                    patList = patReportList;
                }
                bool OneStepCancelReport = dcl.client.frame.UserInfo.GetSysConfigValue("OneStepCancelReport") == "是";
                EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                caller.Remarks = undoReportRemark;
                caller.OperatorSftId = frmCheck.OperatorSftId;
                EntityOperationResultList result = new ProxyPidReportMainAudit().Service.UndoReport(patList, caller);

                if (result.FailedCount == 0)
                {
                    SaveCASignInfo(frmCheck, result);

                    #region 记录CA使用情况
                    //记录CA使用情况
                    if ((frmCheck.strCASignMode == "河池市人民医院" || frmCheck.strCASignMode == "广东医学院附属医院") && frmCheck.CAUserInfo != null && !string.IsNullOrEmpty(frmCheck.CAUserInfo.strCerId))
                    {
                        ProxyUserManage proxy = new ProxyUserManage();
                        List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                        foreach (string pat_id in patList)
                        {
                            EntityCaSign drCaSign = new EntityCaSign();
                            dtCaSign.Add(drCaSign);
                            drCaSign.CaCerId = frmCheck.CAUserInfo.strCerId;
                            if (frmCheck.strCASignMode == "河池市人民医院" && !frmCheck.CAUserInfo.strUserList.Contains(frmCheck.CAUserInfo.strCerId))
                            {
                                drCaSign.CaEntityId = frmCheck.CAUserInfo.strUserList;
                            }
                            drCaSign.CaLoginId = frmCheck.OperatorID;
                            drCaSign.CaName = frmCheck.OperatorName;
                            drCaSign.CaEvent = "取消报告";
                            drCaSign.CaRemark = string.Format("{0} - {1} 取消报告[{2}]", frmCheck.OperatorID, frmCheck.OperatorName, pat_id);
                        }
                        proxy.Service.InsertCaSign(dtCaSign);
                    }
                    #endregion

                    foreach (string pat_id in patList)
                    {

                        EntityPidReportMain dr = GetRowByPatId(drs, pat_id);
                        if (OneStepCancelReport)
                        {
                            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural);
                            dr.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                        }
                        else
                        {
                            dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited);
                            dr.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                        }
                    }

                    if (OneStepCancelReport)
                    {
                        this.UndoAuditBatch(drs, frmCheck.OperatorID, frmCheck.OperatorName);
                    }
                    else
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("取消" + LocalSetting.Current.Setting.ReportWord + "成功");
                    }

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        parentForm.RefreshPatients();
                        parentForm.SelectAllPatient(false);
                    }
                }
                else
                {
                    EntityOperationResult[] results = result.GetSuccessOperations();

                    foreach (EntityOperationResult opr in results)
                    {
                        EntityPidReportMain dr = GetRowByPatId(drs, opr.Data.Patient.RepId);

                        if (dr != null)
                        {
                            if (OneStepCancelReport)
                            {
                                if (OneStepCancelReport)
                                {
                                    dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural);
                                    dr.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                                }
                                else
                                {
                                    dr.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited);
                                    dr.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                                }
                            }
                        }

                        if (dcl.client.frame.UserInfo.GetSysConfigValue("OneStepCancelReport") != "是")
                        {
                            if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                            {
                                parentForm.RefreshPatients();
                                parentForm.SelectAllPatient(false);
                            }
                        }
                        else
                        {
                            this.UndoAuditBatch(drs, frmCheck.OperatorID, frmCheck.OperatorName);

                            if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                            {
                                parentForm.RefreshPatients();
                                parentForm.SelectAllPatient(false);
                            }
                        }
                        FrmOpResultViwer viewer = new FrmOpResultViwer(result);
                        viewer.ShowDialog();
                    }
                }
            }
        }


        #endregion

        #region 签名相关
        /// <summary>
        /// 保存CA签名
        /// </summary>
        /// <param name="frmCheck"></param>
        /// <param name="result"></param>
        private void SaveCASignInfo(FrmCheckPassword frmCheck, EntityOperationResultList result)
        {
            if (frmCheck.caPKI != null)
            {
                //wf.client.frame.PubWaitForm waitForm = new PubWaitForm();
                //waitForm.StartPosition = FormStartPosition.CenterScreen;
                //waitForm.SetCaption("正在签名");
                //waitForm.SetDescription("");
                //waitForm.Show();
                Stopwatch s = new Stopwatch();
                s.Start();

                ProxyUserManage proxyUserManage = new ProxyUserManage();

                List<EntityCaSign> caSignList = new List<EntityCaSign>();

                string plainSignData = string.Empty;
                string signRes = string.Empty;
                string plainStampData = string.Empty;
                string timeStampRes = string.Empty;

                bool caRes = true;

                foreach (var res in result)
                {
                    if (frmCheck.operationCode == EnumOperationCode.Report)
                    {
                        //签名原文
                        plainSignData = res.OperationResultData.ToString();

                        //数字签名结果
                        signRes = frmCheck.caPKI.CASignature(plainSignData);

                        if (string.IsNullOrEmpty(signRes))
                        {
                            MessageDialog.Show("数字签名出错：" + frmCheck.caPKI.ErrorInfo);
                            caRes = false;
                        }

                        //时间戳原文
                        plainStampData = "Ukey证书签名原文：" + plainSignData + "Ukey证书签名结果：" + signRes;
                        //时间戳结果
                        timeStampRes = frmCheck.caPKI.CATimeStamp(plainStampData);

                        if (string.IsNullOrEmpty(timeStampRes))
                        {
                            MessageDialog.Show("打时间戳出错：" + frmCheck.caPKI.ErrorInfo);
                            caRes = false;
                        }
                    }

                    EntityCaSign caSign = new EntityCaSign();

                    caSign.CaDate = ServerDateTime.GetServerDateTime();
                    caSign.CaLoginId = frmCheck.OperatorID;
                    caSign.CaName = frmCheck.OperatorName;
                    caSign.CaEvent = frmCheck.operationCode == EnumOperationCode.Report ? "发布报告" : "取消报告";
                    caSign.CaRemark = string.Format("{0}[{1}]{2}", caSign.CaEvent, res.Data.Patient.RepId, (caRes ? "成功" : "失败:" + frmCheck.caPKI.ErrorInfo));
                    //caSign.CaEntityId = frmCheck.caPKI.GetIdentityID();

                    caSign.CaSourceContent = plainSignData;
                    caSign.CaSignContent = signRes;
                    caSign.CaSourceTimestamp = plainStampData;
                    caSign.CaSignTimestamp = timeStampRes;
                    caSignList.Add(caSign);
                }

                proxyUserManage.Service.InsertCaSign(caSignList);

                s.Stop();
                Lib.LogManager.Logger.LogInfo("签名完成，用时:" + s.Elapsed);
                //waitForm.SetCaption("签名完成，用时" + s.Elapsed);
                //waitForm.Close();
            }
        }

        #endregion

        /// <summary>
        /// 根据PatID返回DataRow
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="patId"></param>
        /// <returns></returns>
        private EntityPidReportMain GetRowByPatId(List<EntityPidReportMain> drs, string patId)
        {
            foreach (EntityPidReportMain dr in drs)
            {
                if (dr.RepId.ToString() == patId)
                {
                    return dr;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据实体返回datarow
        /// </summary>
        /// <param name="entityPatInfo"></param>
        /// <returns></returns>
        private EntityPidReportMain GetNewRowByMsg(EntityPidReportMain entityPatInfo)
        {
            EntityPidReportMain dtNewRow = new EntityPidReportMain();
            dtNewRow.RepSid = entityPatInfo.RepSid;
            dtNewRow.PidName = entityPatInfo.PidName;
            dtNewRow.PidComName = entityPatInfo.PidComName;

            return dtNewRow;
        }

        #region 获取权限代码
        /// <summary>
        /// 获取审核的权限代码
        /// </summary>
        /// <returns></returns>
        private string GetAuditPopedomCode()
        {
            if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
            {
                return LIS_Const.BillPopedomCode.Audit;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
            {
                return LIS_Const.BillPopedomCode.Audit;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
            {
                return LIS_Const.BillPopedomCode.Audit;
            }
            return "-1";
        }

        /// <summary>
        /// 获取取消审核的权限代码
        /// </summary>
        /// <returns></returns>
        private string GetUndoAuditPopedomCode()
        {
            if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
            {
                return LIS_Const.BillPopedomCode.UndoAudit;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
            {
                return LIS_Const.BillPopedomCode.UndoAudit;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
            {
                return LIS_Const.BillPopedomCode.UndoAudit;
            }
            return "-1";
        }

        /// <summary>
        /// 获取报告限代码
        /// </summary>
        /// <returns></returns>
        private string GetReportPopedomCode()
        {
            if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
            {
                return LIS_Const.BillPopedomCode.Report;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
            {
                return LIS_Const.BillPopedomCode.Report;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
            {
                return LIS_Const.BillPopedomCode.Report;
            }
            return "-1";
        }

        /// <summary>
        /// 获取取消报告限代码
        /// </summary>
        /// <returns></returns>
        private string GetUndoReportPopedomCode()
        {
            if (this.ItrDataType == LIS_Const.InstmtDataType.Eiasa
                || this.ItrDataType == LIS_Const.InstmtDataType.Normal)
            {
                return LIS_Const.BillPopedomCode.UndoReport;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Bacteria)
            {
                return LIS_Const.BillPopedomCode.UndoReport;
            }
            else if (this.ItrDataType == LIS_Const.InstmtDataType.Description)
            {
                return LIS_Const.BillPopedomCode.UndoReport;
            }
            return "-1";
        }

        #endregion

        public bool canReportOnNoBarCode(FrmCheckPassword check, string funcCode)
        {
            if (check.OperatorID != "admin")
            {
                EntityUserQc userQc = new EntityUserQc();
                userQc.LoginId = check.OperatorID;
                List<EntitySysUser> listUser = new ProxySysUserInfo().Service.SysUserQuery(userQc);
                if (listUser.Count > 0)
                {
                    List<EntitySysUser> drUser = listUser.FindAll(i => i.FuncCode == funcCode);
                    if (drUser.Count <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
