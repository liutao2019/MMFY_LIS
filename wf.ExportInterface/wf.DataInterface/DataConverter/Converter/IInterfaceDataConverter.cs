using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.DataConverter
{
    public interface IInterfaceDataConverter
    {
        object ConvertTo(object input);
    }
}
