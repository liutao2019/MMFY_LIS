using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaPurchase
    {
        [OperationContract]
        EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t);
        [OperationContract]
        bool InsertNewData(List<EntityReaPurchase> data);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaPurchase> ReaQuery(EntityReaQC patientCondition);
        [OperationContract]
        List<EntityReaStoreCount> SearchAllReaStoreCount();
        [OperationContract]
        EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t);
        [OperationContract]
        EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t);
        [OperationContract]
        bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaPurchase> data);
        [OperationContract]
        EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t);
        [OperationContract]
        bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
