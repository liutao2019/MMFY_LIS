using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.Connection;
using System.Data;
using Lib.DAC.DbSchema;

namespace Lib.DAC
{
    internal class DbSchemaHelper
    {
        public static IDbSchemaProvider GetProvider(string connectionString, EnumDbDriver enumDriver, EnumDataBaseDialet enumDBType)
        {
            IDbSchemaProvider provider;
            if (enumDBType == EnumDataBaseDialet.SQL2000
                || enumDBType == EnumDataBaseDialet.SQL2005
                || enumDBType == EnumDataBaseDialet.SQL2008)
            {
                provider = new MSSqlDbSchemaProvider(connectionString, enumDriver, enumDBType);
            }
            else if (enumDBType == EnumDataBaseDialet.Oracle8i
                || enumDBType == EnumDataBaseDialet.Oracle9i
                || enumDBType == EnumDataBaseDialet.Oracle10g
                || enumDBType == EnumDataBaseDialet.Oracle11g)
            {
                provider = new OracleDbSchemaProvider(connectionString, enumDriver, enumDBType);
            }
            else if (enumDBType == EnumDataBaseDialet.Access
                || enumDBType == EnumDataBaseDialet.Access2007)
            {
                provider = new AccessDbSchemaProvidercs(connectionString, enumDriver, enumDBType);
            }
            else
            {
                throw new ArgumentException();
            }
            return provider;
        }
    }
}
