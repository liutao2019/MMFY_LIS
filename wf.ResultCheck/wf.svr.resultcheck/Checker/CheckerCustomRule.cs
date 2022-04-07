using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.pub.entities;
using System.Data;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.resultcheck.Checker
{
    class CheckerCustomRule : AbstractAuditClass, IAuditChecker
    {
        public CheckerCustomRule(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                return;
                string msg;

                //DataTable tbPatient = Lib.EntityCore.EntityConverter.EntityToDataTable<EntityPatients>(this.pat_info);
                //DataTable tbResulto = Lib.EntityCore.EntityConverter.EntityListToDataTable<EntityObrResult>(this.resulto);
                //DataTable tbPatientMi = Lib.EntityCore.EntityConverter.EntityListToDataTable<EntityPidReportDetail>(this.patients_mi);

                //获取审核配置(代码编译ID)
                string strSysConfig = string.Empty;
                //分别获取一审和二审的参数ID配置
                if (auditType == EnumOperationCode.Audit)
                {
                    strSysConfig = "FirstAuditCheck";
                }
                else if (auditType == EnumOperationCode.Report)
                {
                    strSysConfig = "SecondAuditCheck";
                }

                string strCodeId = CacheSysConfig.Current.GetSystemConfig(strSysConfig);

                ////可支持多段代码编译验证
                //if (!string.IsNullOrEmpty(strCodeId))
                //{
                //    string[] strCodeIdArr = strCodeId.Split(';');
                //    foreach (string strId in strCodeIdArr)
                //    {
                //        object obj = dcl.common.DynamicCompilation.ClsDynamicCompilationMemory.Instance.m_objDynamicCompilationMemory(strId, new object[] { this.pat_info, this.patients_mi, this.resulto }, out msg);

                //        EntityOperationResult res = (obj as EntityOperationResult);

                //        chkResult.Merge(res);
                //    }

                //}
            }
        }

        #endregion
    }
}


namespace CustomAuditRule
{
    public class CustomAuditRuleChecker
    {
        public EntityOperationResult Check(DataTable patients, DataTable patients_mi, DataTable resulto)
        {
            EntityOperationResult result = new EntityOperationResult();

            result.AddCustomMessage("1", "", "123", EnumOperationErrorLevel.Warn);

            return result;
        }
    }
}
