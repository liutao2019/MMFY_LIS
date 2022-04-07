using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoTouchPrint
    {
        List<EntityPidReportMain> PatientPrintQuery(string pidInNo, string pidSrcId);

        List<EntitySampMain> SampMainPrintQuery(string pidInNo, string pidSrcId);
    }
}
