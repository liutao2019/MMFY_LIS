using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    public class DbSchemaProcedure
    {
        List<DbSchemaParameter> Prameters { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 存储过程定义
        /// </summary>
        public string Difinition { get; set; }

        public DbSchemaProcedure()
        {
            this.Prameters = new List<DbSchemaParameter>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
