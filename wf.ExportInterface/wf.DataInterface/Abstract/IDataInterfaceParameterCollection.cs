using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    interface IDataInterfaceParameterCollection : IDataParameterCollection
    {
        AbstractDataInterfaceParameter GetReturnValueParameter();

        int GetHashCode();
    }
}
