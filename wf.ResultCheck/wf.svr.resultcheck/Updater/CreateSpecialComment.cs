using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CreateSpecialComment : AbstractAuditClass, IAuditUpdater
    {
        public CreateSpecialComment(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit
                ||
                (this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport))
            {

                string strComment = string.Empty;

                try
                {
                    foreach (EntityObrResult res in resulto)
                    {
                        //if (res.referenceSam == null)
                        //{
                        //    continue;
                        //}

                        //int int_res_ref_flag = (int)EnumResRefFlag.Unknow;

                        //if (int.TryParse(res.RefFlag, out int_res_ref_flag) && int_res_ref_flag != -1)
                        //{
                        //    EnumResRefFlag flag = (EnumResRefFlag)int_res_ref_flag;

                        //    if (
                        //        (
                        //        (flag & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1
                        //        || (flag & EnumResRefFlag.Positive) == EnumResRefFlag.Positive
                        //        || (flag & EnumResRefFlag.WeaklyPositive) == EnumResRefFlag.WeaklyPositive)
                        //        && !string.IsNullOrEmpty(res.referenceSam.itm_h_info)
                        //        )
                        //    {
                        //        //填充偏高提示
                        //        if (strComment != string.Empty)
                        //        {
                        //            strComment += "\r\n";
                        //        }

                        //        if (strComment.Length <= 1500)
                        //        {
                        //            strComment += res.referenceSam.itm_h_info;
                        //        }
                        //    }
                        //    else if (
                        //            (flag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1
                        //            && !string.IsNullOrEmpty(res.referenceSam.itm_l_info)
                        //            )
                        //    {
                        //        //填充偏低提示
                        //        if (strComment != string.Empty)
                        //        {
                        //            strComment += "\r\n";
                        //        }
                        //        if (strComment.Length <= 1500)
                        //        {
                        //            strComment += res.referenceSam.itm_l_info;
                        //        }
                        //    }
                        //    else if (
                        //            (flag == EnumResRefFlag.Normal)
                        //            && !string.IsNullOrEmpty(res.referenceSam.itm_use_exp)
                        //            )
                        //    {
                        //        //填充正常时的提示(使用方法)
                        //        if (strComment != string.Empty)
                        //        {
                        //            strComment += "\r\n";
                        //        }
                        //        if (strComment.Length <= 1500)
                        //        {
                        //            strComment += res.referenceSam.itm_use_exp;
                        //        }
                        //    }

                        //}

                        ////填充临床意义
                        //if (!string.IsNullOrEmpty(res.referenceSam.itm_sign))
                        //{
                        //    if (strComment != string.Empty)
                        //    {
                        //        strComment += "\r\n";
                        //    }
                        //    if (strComment.Length <= 1500)
                        //    {
                        //        strComment += res.referenceSam.itm_sign;
                        //    }
                        //}
                    }

                    if (strComment != string.Empty)
                    {
                        if (pat_info.RepComment == null || pat_info.RepComment == string.Empty)
                            pat_info.RepComment = strComment;
                        else if (pat_info.RepComment.IndexOf(strComment) < 0)
                            pat_info.RepComment = pat_info.RepComment + "\r\n" + strComment;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "CreateSpecialComment", ex.ToString());
                    chkResult.AddMessage(EnumOperationErrorCode.Exception, ex.StackTrace, EnumOperationErrorLevel.Error);
                    //throw;
                }


            }
        }

        #endregion
    }
}
