using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    class BatchRunScript
    {
        public void Run(List<string> listPatId, EnumOperationCode auditType)
        {
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid") == "是")
            {
                if (auditType == EnumOperationCode.Report)
                {
                    new Lis.SendDataToMid.TypeStandardDataSender().Send(listPatId);
                }
                else if (auditType == EnumOperationCode.UndoReport)
                {
                    new Lis.SendDataToMid.TypeStandardDataSender().UndoSend(listPatId);
                }
            }
            else if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid") == "HL7")
            {
                if (auditType == EnumOperationCode.Report)
                {
                    Lis.SendDataByHl7.TypeStandardDataSenderByHL7 SenderByHl7 = new Lis.SendDataByHl7.TypeStandardDataSenderByHL7();

                    SenderByHl7.Send(listPatId);

                }
                else if (auditType == EnumOperationCode.UndoReport)
                {
                    Lis.SendDataByHl7.TypeStandardDataSenderByHL7 SenderByHl7 = new Lis.SendDataByHl7.TypeStandardDataSenderByHL7();

                    SenderByHl7.SendDelReport(listPatId);
                }
            }
        }
    }
}
