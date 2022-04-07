using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public abstract class AbstractAuditClass
    {
        protected EntityPidReportMain pat_info = null;
        protected List<EntityObrResult> resulto = null;
        protected List<EntityPidReportDetail> patients_mi = null;
        protected EnumOperationCode auditType;
        protected AuditConfig config = null;

        protected ITransaction innerTransaction = null;

        public EntityRemoteCallClientInfo Caller { get; set; }

        public AbstractAuditClass(EntityPidReportMain pat_info
                                    , List<EntityPidReportDetail> patients_mi
                                    , List<EntityObrResult> resulto
                                    , EnumOperationCode auditType
                                    , AuditConfig config
                                   )
        {
            this.pat_info = pat_info;
            this.patients_mi = patients_mi;
            this.resulto = resulto;
            this.auditType = auditType;
            this.config = config;
            this.Caller = null;
        }
    }
}
