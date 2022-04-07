using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    internal abstract class AbstractDbSchemaProvider
    {
        protected EnumDataBaseDialet dbtype;
        protected string connString;
        protected EnumDbDriver driver;

        public AbstractDbSchemaProvider(string connectionString
                                , EnumDbDriver enumDbDriver
                                , EnumDataBaseDialet enumDbDialet)
        {
            connString = connectionString;
            dbtype = enumDbDialet;
            driver = enumDbDriver;
        }


       
    }
}
