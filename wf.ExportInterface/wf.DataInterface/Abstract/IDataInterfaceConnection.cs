using Lib.DAC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    interface IDataInterfaceConnection
    {
        IDataInterfaceCommand CreateCommand();
        bool TestConnection(out string errorMessage);
        SqlHelper GetSqlHelper();
        string ServerAddress { get; set; }
        string DbDriver { get; set; }
        string DbDialet { get; set; }
        string Catelog { get; set; }
        string LoginName { get; set; }
        string LoginPassword { get; set; }

        int GetHashCode();
    }
}
