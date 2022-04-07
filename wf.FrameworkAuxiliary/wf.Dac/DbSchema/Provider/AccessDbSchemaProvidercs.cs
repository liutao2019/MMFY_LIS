using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    internal class AccessDbSchemaProvidercs : AbstractDbSchemaProvider, IDbSchemaProvider
    {
        public AccessDbSchemaProvidercs(string connectionString
                                , EnumDbDriver enumDriver
                                , EnumDataBaseDialet enumDbDialet)
            : base(connectionString, enumDriver, enumDbDialet)
        {

        }

        #region IDbSchemaProvider 成员

        public List<DbSchemaTable> GetTables()
        {
            throw new NotImplementedException();
        }

        public DbSchemaTable GetTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public List<DbSchemaView> GetViews()
        {
            throw new NotImplementedException();
        }

        public DbSchemaView GetView(string viewName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaColumnCollection GetTableColumns(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaColumn GetTableColumn(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaColumnCollection GetViewColumns(string viewName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaColumn GetViewColumn(string viewName, string columnName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaInfo GetDataBaseInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
