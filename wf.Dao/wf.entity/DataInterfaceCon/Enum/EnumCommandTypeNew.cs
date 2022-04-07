using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public enum EnumCommandTypeNew
    {
        // 摘要:
        //     An SQL text command. (Default.)
        Text,
        //
        // 摘要:
        //     The name of a stored procedure.
        StoredProcedure,
        //
        // 摘要:
        //     The name of a table.
        TableDirect,
    }
}
