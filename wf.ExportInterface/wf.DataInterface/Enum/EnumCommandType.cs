using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    public enum EnumCommandType
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
