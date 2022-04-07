using System.Data;
using dcl.pub.entities;
using System;
using dcl.svr.resultcheck;
using System.Collections.Generic;
using dcl.entity;

namespace CustomAuditRuleScript
{
    public class ScriptAfterSecondAudit
    {
        public EntityOperationResult Check(object[] pars)
        {
            EntityOperationResult result = new EntityOperationResult();

            //得到病人资料表
            EntityPidReportMain patients = pars[0] as EntityPidReportMain;

            //病人ID
            string pat_id = patients.RepId;

            //执行上传的脚本

            return result;
        }
    }
}