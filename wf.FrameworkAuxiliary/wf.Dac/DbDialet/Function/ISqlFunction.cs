using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.Function
{
    interface ISqlFunction
    {
        string Build(List<object> args);
    }
}
