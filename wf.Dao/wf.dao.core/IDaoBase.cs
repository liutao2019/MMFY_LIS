using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.core
{
    public interface IDaoBase
    {
        DBManager Dbm { get; set; }
    }
}
