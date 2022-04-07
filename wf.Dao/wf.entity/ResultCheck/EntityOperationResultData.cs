using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityOperationResultData
    {
        public EntityPidReportMain Patient { get; set; }

        public EntityOperationResultData()
        {
            this.Patient = new EntityPidReportMain();
        }
    }
}
