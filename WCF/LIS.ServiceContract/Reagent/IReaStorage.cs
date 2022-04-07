using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaStorage
    {
        [OperationContract]
        EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t);
        [OperationContract]
        bool InsertNewData(List<EntityReaStorage> data);
        [OperationContract]
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitSid(string pat_sid, DateTime pat_date);
        [OperationContract]
        string GetReaBarcode_MaxPlusOne(DateTime date, string stepCode);
        [OperationContract]
        bool ExsitBarcode(string pat_sid, DateTime pat_date);
        [OperationContract]
        List<EntityReaStorage> ReaQuery(EntityReaQC patientCondition);
        [OperationContract]
        List<EntityReaStoreCount> SearchAllReaStoreCount();
        [OperationContract]
        EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t);
        [OperationContract]
        EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t);
        [OperationContract]
        bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaStorage> data);
        [OperationContract]
        EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t);
        [OperationContract]
        bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t);
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace);
    }
}
