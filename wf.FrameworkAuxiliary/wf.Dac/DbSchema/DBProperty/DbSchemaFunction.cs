using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    public class DbSchemaFunc
    {
        public string Name { get; internal set; }
        public string Owner { get; internal set; }

        List<DbSchemaParameter> Prameters { get; set; }

        public DbSchemaFunc()
        {
            this.Prameters = new List<DbSchemaParameter>();
        }
    }
}
