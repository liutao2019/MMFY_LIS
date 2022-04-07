using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    interface IDataInterfaceParameter
    {
        string Name { get; set; }
        int Length { get; set; }
        EnumDataInterfaceParameterDirection Direction { get; set; }
        string DataType { get; set; }
        object Value { get; set; }
        int GetHashCode();
    }
}
