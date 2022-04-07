using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysBarcodeGenerator
    {
        string GetNextMaxBarCode();
    }
}
