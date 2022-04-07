using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.DataConverter
{
    class NoConverter : IInterfaceDataConverter
    {
        public object ConvertTo(object input)
        {
            return input;
        }
    }
}
