 using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaSubscribe
    {
        [OperationContract]
        EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t);
        [OperationContract]
        bool InsertNewData(List<EntityReaSubscribe> data);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaSubscribe> ReaQuery(EntityReaQC patientCondition);
        [OperationContract]
        List<EntityReaStoreCount> SearchAllReaStoreCount();
        [OperationContract]
        EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t);
        [OperationContract]
        EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t);
        [OperationContract]
        bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaSubscribe> data);
        [OperationContract]
        EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t);
        [OperationContract]
        bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
