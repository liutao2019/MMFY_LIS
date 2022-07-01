using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.svr.interfaces;

namespace dcl.svr.resultcheck
{
    public class SendDataToMid
    {
        public void Run(List<string> listPatId, EnumOperationCode auditType)
        {
            if (auditType == EnumOperationCode.Report)
            {
                DCLExtInterfaceFactory.DCLExtInterface.UploadDCLReportAsync(listPatId);
            }
            else if (auditType == EnumOperationCode.UndoReport)
            {
                DCLExtInterfaceFactory.DCLExtInterface.UndoUploadDCLReportAsync(listPatId);
            }
            else if (auditType == EnumOperationCode.MidReport)
            {
                DCLExtInterfaceFactory.DCLExtInterface.UploadDCLMidReportAsync(listPatId);
            }
            return;
        }

        public void SendYssReport(List<string> listPatId, EnumOperationCode auditType)
        {
            if (auditType == EnumOperationCode.Report || auditType == EnumOperationCode.Audit)
            {
                DCLExtInterfaceFactory.DCLExtInterface.UploadYssReportAsync(listPatId);
            }
            else if (auditType == EnumOperationCode.UndoReport || auditType == EnumOperationCode.UndoAudit)
            {
                DCLExtInterfaceFactory.DCLExtInterface.UndoUploadYssReportAsync(listPatId);
            }
            return;
        }
        
    }
}
