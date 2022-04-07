using dcl.servececontract;
using System.Collections.Generic;
using dcl.entity;

namespace dcl.svr.result
{
    public class ObrRelateResultBIZ : IObrRelateResult
    {
        public List<EntityObrResult> GetRelateResult(EntityPidReportMain patient)
        {
            return new ObrResultBIZ().GetObrRelateResult(patient);
        }
    }
}
