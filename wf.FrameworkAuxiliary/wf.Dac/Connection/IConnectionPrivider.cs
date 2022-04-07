using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Lib.DAC.DbDriver;

namespace Lib.DAC.Connection
{
    internal interface IConnectionPrivider
    {
        IDbConnection GetConnection();
        IDbDriver Driver { get; }
    }
}
