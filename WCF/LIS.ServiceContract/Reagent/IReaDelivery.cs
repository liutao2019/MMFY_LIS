using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaDelivery
    {
        [OperationContract]
        EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t);
        [OperationContract]
        bool InsertNewData(List<EntityReaDelivery> data);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaDelivery> ReaQuery(EntityReaQC patientCondition);
        [OperationContract]
        List<EntityReaStoreCount> SearchAllReaStoreCount();
        [OperationContract]
        EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t);
        [OperationContract]
        EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t);
        [OperationContract]
        bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaDelivery> data);
        [OperationContract]
        EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t);
        [OperationContract]
        bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
