using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using dcl.svr.cache;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 更新病人信息
    /// </summary>
    public class UpdatePatientsInfo : AbstractAuditClass, IAuditUpdater
    {
        public UpdatePatientsInfo(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {
        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit
                ||
                 (this.auditType == EnumOperationCode.Report && this.config.bAllowStepAuditToReport)
                )
            {
                //检验项目勾上传染病标识检验报告审核的时候，如果发现项目为传染病
                //，并且结果为阳性，修改检查类型为隐私触摸屏打印
                if (config.IllReportNotAllowPrintMz)
                {
                    foreach (EntityObrResult res in this.resulto)
                    {
                        string res_chr = res.ObrValue;
                        if (
                        (res_chr.Contains("阳性")
                        || res_chr.StartsWith("+")
                        || res_chr.EndsWith("+")
                        || res_chr.ToLower() == "pos"
                        )
                        && !res_chr.Contains("弱阳性")
                        && !res_chr.Contains("±")
                       && !(res_chr.Length > 1 && res_chr.Replace("+", "").Trim() == string.Empty)
                    )
                        {
                            var refItem = DictItemCache.Current.DclCache.Find(i => i.ItmId == res.ItmId && i.ItmDelFlag != "1");
                            if (refItem == null || refItem.ItmInfectionFlag != 1)
                            {
                                continue;
                            }

                            pat_info.RepCtype = "3";
                        }
                    }
                }


            }


            if (this.auditType == EnumOperationCode.Report)
            {
                #region 填充危急值标志
                bool hasCriticalValues = false;
                foreach (EntityObrResult res in resulto)
                {
                    int int_res_ref_flag;
                    if (int.TryParse(res.RefFlag, out int_res_ref_flag))
                    {
                        EnumResRefFlag res_ref_flag = (EnumResRefFlag)int_res_ref_flag;

                        if (res_ref_flag != EnumResRefFlag.Unknow)
                        {
                            //根据res_ref_flag标志判断
                            if (
                                (res_ref_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2//高于危急值上限
                                ||
                                (res_ref_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2//低于危急值下限
                                ||
                                (res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue//自定义危急值
                               )
                            {
                                this.pat_info.RepUrgentFlag = 1;

                                hasCriticalValues = true;
                                break;
                            }
                        }
                    }
                }

                if (!hasCriticalValues)
                {
                    this.pat_info.RepUrgentFlag = 0;
                }
                this.pat_info.RepReadUserId = null;
                this.pat_info.RepReadDate = null;
                #endregion

            }
            else if (this.auditType == EnumOperationCode.UndoReport)
            {
                this.pat_info.RepUrgentFlag = 0;
            }
            if (!string.IsNullOrEmpty(pat_info.RepBarCode))
            {
                EntitySampQC sampQc = new EntitySampQC();
                sampQc.ListSampBarId.Add(pat_info.RepBarCode);


                IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
                List<EntitySampMain> listBcPat = new List<EntitySampMain>();
                if (dao != null)
                {
                    listBcPat = dao.GetSampMain(sampQc);
                    if (listBcPat != null && listBcPat.Count > 0)
                    {
                        pat_info.RepInputId = listBcPat[0].PidPatno.ToString();
                        if (!string.IsNullOrEmpty(listBcPat[0].PidAdmissTimes.ToString()))
                            pat_info.PidAddmissTimes = Convert.ToInt32(listBcPat[0].PidAdmissTimes);
                        pat_info.PidExamNo = listBcPat[0].PidExamNo;
                        pat_info.PidExamCompany = listBcPat[0].PidExamCompany;
                    }
                }
            }
        }

        #endregion
    }
}
