using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaLossReport
    {
        [OperationContract]
        EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t);
        [OperationContract]
        bool InsertNewData(List<EntityReaLossReport> data);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaLossReport> ReaQuery(EntityReaQC patientCondition);

        [OperationContract]
        EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t);
        [OperationContract]
        EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t);
        [OperationContract]
        bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaLossReport> data);
        [OperationContract]
        EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t);
        [OperationContract]
        bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
