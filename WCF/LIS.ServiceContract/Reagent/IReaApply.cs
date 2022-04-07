using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaApply
    {
        [OperationContract]
        EntityOperateResult SaveReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply);
        [OperationContract]
        bool InsertNewApply(List<EntityReaApply> applies);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaApply> ReaQuery(EntityReaQC patientCondition);
        [OperationContract]
        List<EntityReaStoreCount> SearchAllReaStoreCount();
        [OperationContract]
        EntityOperateResult UpdateReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply);
        [OperationContract]
        EntityOperateResult AuditReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply);
        [OperationContract]
        bool DeleteReaApply(EntityRemoteCallClientInfo caller, List<EntityReaApply> applies);
        [OperationContract]
        EntityOperateResult UndoReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply);
        [OperationContract]
        bool ReturnReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
