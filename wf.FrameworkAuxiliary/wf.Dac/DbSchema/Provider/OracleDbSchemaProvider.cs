using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DAC.DbSchema
{
    internal class OracleDbSchemaProvider : AbstractDbSchemaProvider, IDbSchemaProvider
    {
        public OracleDbSchemaProvider(string connectionString
                                , EnumDbDriver enumDriver
                                , EnumDataBaseDialet enumDbDialet)
            : base(connectionString, enumDriver, enumDbDialet)
        {

        }

        //        public DbSchemaColumnCollection GetColumns(string tableName)
        //        {
        //            SqlHelper helper = new SqlHelper(this.connString, this.driver);

        //            DbCommandEx cmd = helper.CreateCommandEx(@"
        //SELECT
        //    TABLE_NAME,
        //    COLUMN_NAME,
        //    DATA_TYPE,
        //    NULLABLE,
        //    DATA_LENGTH,
        //    DATA_PRECISION,
        //    DATA_SCALE,
        //    COLUMN_ID
        //FROM USER_TAB_COLUMNS WHERE TABLE_NAME = ? ORDER BY COLUMN_ID");

        //            cmd.AddParameterValue(tableName);

        //            DataTable table = helper.GetTable(cmd);

        //            DbSchemaColumnCollection list = new DbSchemaColumnCollection();

        //            foreach (DataRow row in table.Rows)
        //            {
        //                DbSchemaColumn item = DataRowToColumn(row);
        //                list.Add(item);
        //            }

        //            return list;
        //        }

        private DbSchemaColumn DataRowToColumn(DataRow row)
        {
            string tableName = row["TABLE_NAME"].ToString();
            string colName = row["COLUMN_NAME"].ToString();

            DbSchemaColumn item = new DbSchemaColumn(tableName, colName);

            item.DataType = row["DATA_TYPE"].ToString();
            item.Nullable = (row["NULLABLE"].ToString().ToLower() == "y");

            if (row["DATA_LENGTH"] != DBNull.Value)
            {
                item.DataLength = Convert.ToInt32(row["DATA_LENGTH"]);
            }

            if (row["DATA_PRECISION"] != DBNull.Value)
            {
                item.DataPrecision = Convert.ToInt32(row["DATA_PRECISION"]);
            }

            if (row["DATA_SCALE"] != DBNull.Value)
            {
                item.DataScale = Convert.ToInt32(row["DATA_SCALE"]);
            }

            item.OrdinalPosition = Convert.ToInt32(row["COLUMN_ID"]);

            return item;
        }

        public DbSchemaColumnCollection GetTableColumns(string tableName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx(@"
            SELECT
                TABLE_NAME,
                COLUMN_NAME,
                DATA_TYPE,
                NULLABLE,
                DATA_LENGTH,
                DATA_PRECISION,
                DATA_SCALE,
                COLUMN_ID
            FROM USER_TAB_COLUMNS WHERE TABLE_NAME = ? ORDER BY COLUMN_ID");

            cmd.AddParameterValue(tableName);

            DataTable table = helper.GetTable(cmd);

            DbSchemaColumnCollection list = new DbSchemaColumnCollection();

            foreach (DataRow row in table.Rows)
            {
                DbSchemaColumn item = DataRowToColumn(row);
                list.Add(item);
            }

            return list;
        }

        public DbSchemaColumn GetTableColumn(string tableName, string columnName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx(@"
            SELECT
                TABLE_NAME,
                COLUMN_NAME,
                DATA_TYPE,
                NULLABLE,
                DATA_LENGTH,
                DATA_PRECISION,
                DATA_SCALE,
                COLUMN_ID
            FROM USER_TAB_COLUMNS WHERE TABLE_NAME = ? and COLUMN_NAME = ? ORDER BY COLUMN_ID");

            cmd.AddParameterValue(tableName);
            cmd.AddParameterValue(columnName);

            DataTable table = helper.GetTable(cmd);

            DbSchemaColumn item = null;

            if (table.Rows.Count > 0)
            {
                item = DataRowToColumn(table.Rows[0]);
            }

            return item;
        }

        public DbSchemaColumnCollection GetViewColumns(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbSchemaColumn GetViewColumn(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        #region IDbSchemaProvider 成员

        public List<DbSchemaTable> GetTables()
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DataTable table = helper.GetTable("select TABLESPACE_NAME,TABLE_NAME from user_tables");

            List<DbSchemaTable> list = new List<DbSchemaTable>();

            foreach (DataRow row in table.Rows)
            {
                DbSchemaTable item = new DbSchemaTable();

                item.TableName = row["TABLE_NAME"].ToString();
                item.Owner = row["TABLESPACE_NAME"].ToString();

                list.Add(item);
            }

            return list;
        }

        public DbSchemaTable GetTable(string tableName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx("select TABLESPACE_NAME,TABLE_NAME from user_tables where TABLE_NAME = ?");
            cmd.AddParameterValue(tableName);

            DataTable table = helper.GetTable(cmd);

            DbSchemaTable item = null;

            if (table.Rows.Count > 0)
            {
                item = new DbSchemaTable();

                item.TableName = table.Rows[0]["TABLE_NAME"].ToString();
                item.Owner = table.Rows[0]["TABLESPACE_NAME"].ToString();
            }

            return item;
        }

        public List<DbSchemaView> GetViews()
        {
            throw new NotImplementedException();
        }

        DbSchemaView IDbSchemaProvider.GetView(string viewName)
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
