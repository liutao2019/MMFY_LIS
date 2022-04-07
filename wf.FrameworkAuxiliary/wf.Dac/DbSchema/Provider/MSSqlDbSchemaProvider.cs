using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC.DbDriver;

namespace Lib.DAC.DbSchema
{
    internal class MSSqlDbSchemaProvider : AbstractDbSchemaProvider, IDbSchemaProvider
    {
        public MSSqlDbSchemaProvider(string connectionString
                                , EnumDbDriver enumDriver
                                , EnumDataBaseDialet enumDbDialet)
            : base(connectionString, enumDriver, enumDbDialet)
        {

        }

        private DbSchemaColumn DataRowToColumn(DataRow row)
        {
            string tableName = row["TABLE_NAME"].ToString();
            string colName = row["COLUMN_NAME"].ToString();

            DbSchemaColumn item = new DbSchemaColumn(tableName, colName);

            item.DataType = row["DATA_TYPE"].ToString();
            item.Nullable = (row["IS_NULLABLE"].ToString().ToUpper() == "YES");

            item.IsPrimaryKey = (row["IS_PRIMARYKEY"].ToString().ToUpper() == "YES");

            if (row["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            {
                item.DataLength = Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]);
            }

            if (row["NUMERIC_PRECISION"] != DBNull.Value)
            {
                item.DataPrecision = Convert.ToInt32(row["NUMERIC_PRECISION"]);
            }

            if (row["NUMERIC_SCALE"] != DBNull.Value)
            {
                item.DataScale = Convert.ToInt32(row["NUMERIC_SCALE"]);
            }

            item.OrdinalPosition = Convert.ToInt32(row["ORDINAL_POSITION"]);

            return item;
        }

        public DbSchemaColumnCollection GetTableColumns(string tableName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx(@"
            select
                col.TABLE_NAME,
                col.COLUMN_NAME,
                col.DATA_TYPE,
                col.IS_NULLABLE,
                col.ORDINAL_POSITION,
                col.CHARACTER_MAXIMUM_LENGTH,
                col.NUMERIC_PRECISION,
                col.NUMERIC_SCALE,
            	case when keycol.COLUMN_NAME is null then 'NO'
                     else 'YES' end IS_PRIMARYKEY
            from information_schema.columns col
            left join information_schema.key_column_usage keycol 
            	on col.TABLE_NAME = keycol.TABLE_NAME 
            	and col.COLUMN_NAME = keycol.COLUMN_NAME
            	and col.TABLE_CATALOG = keycol.TABLE_CATALOG
            where col.TABLE_NAME = ?
            order by col.ORDINAL_POSITION asc
            ");

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
            select
                col.TABLE_NAME,
                col.COLUMN_NAME,
                col.DATA_TYPE,
                col.IS_NULLABLE,
                col.ORDINAL_POSITION,
                col.CHARACTER_MAXIMUM_LENGTH,
                col.NUMERIC_PRECISION,
                col.NUMERIC_SCALE,
            	case when keycol.COLUMN_NAME is null then 'NO'
                     else 'YES' end IS_PRIMARYKEY
            from information_schema.columns col
            left join information_schema.key_column_usage keycol 
            	on col.TABLE_NAME = keycol.TABLE_NAME 
            	and col.COLUMN_NAME = keycol.COLUMN_NAME
            	and col.TABLE_CATALOG = keycol.TABLE_CATALOG
            where col.TABLE_NAME = ? and col.COLUMN_NAME = ?
            order by col.ORDINAL_POSITION asc
            ");
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

        public DbSchemaColumnCollection GetViewColumns(string viewName)
        {
            return GetTableColumns(viewName);
        }

        public DbSchemaColumn GetViewColumn(string viewName, string columnName)
        {
            return GetTableColumn(viewName, columnName);
        }

        /// <summary>
        /// 获取所有表信息
        /// </summary>
        /// <returns></returns>
        public List<DbSchemaTable> GetTables()
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DataTable table = helper.GetTable("select TABLE_SCHEMA,TABLE_NAME from information_schema.tables where TABLE_TYPE='BASE TABLE' order by TABLE_NAME");

            List<DbSchemaTable> list = new List<DbSchemaTable>();

            foreach (DataRow row in table.Rows)
            {
                DbSchemaTable item = new DbSchemaTable();

                item.TableName = row["TABLE_NAME"].ToString();
                item.Owner = row["TABLE_SCHEMA"].ToString();

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// 获取单个表信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DbSchemaTable GetTable(string tableName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx("select TABLE_SCHEMA,TABLE_NAME from information_schema.tables where TABLE_TYPE=? and TABLE_NAME = ?");
            cmd.AddParameterValue("BASE TABLE");
            cmd.AddParameterValue(tableName);

            DataTable table = helper.GetTable(cmd);

            DbSchemaTable item = null;

            if (table.Rows.Count > 0)
            {
                item = new DbSchemaTable();

                item.TableName = table.Rows[0]["TABLE_NAME"].ToString();
                item.Owner = table.Rows[0]["TABLE_SCHEMA"].ToString();
            }

            return item;
        }

        /// <summary>
        /// 获取所有视图信息
        /// </summary>
        /// <returns></returns>
        public List<DbSchemaView> GetViews()
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DataTable table = helper.GetTable("select TABLE_SCHEMA,TABLE_NAME,VIEW_DEFINITION from information_schema.views order by TABLE_NAME");

            List<DbSchemaView> list = new List<DbSchemaView>();

            foreach (DataRow row in table.Rows)
            {
                DbSchemaView item = new DbSchemaView();

                item.ViewName = row["TABLE_NAME"].ToString();
                item.Owner = row["TABLE_SCHEMA"].ToString();
                item.Difinition = row["VIEW_DEFINITION"].ToString();

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// 获取单个视图信息
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public DbSchemaView GetView(string viewName)
        {
            SqlHelper helper = new SqlHelper(this.connString, this.driver);

            DbCommandEx cmd = helper.CreateCommandEx("select TABLE_SCHEMA,TABLE_NAME,VIEW_DEFINITION from information_schema.views where TABLE_NAME = ? order by TABLE_NAME");
            cmd.AddParameterValue(viewName);

            DataTable table = helper.GetTable(cmd);

            DbSchemaView item = null;

            if (table.Rows.Count > 0)
            {
                item = new DbSchemaView();

                item.ViewName = table.Rows[0]["TABLE_NAME"].ToString();
                item.Owner = table.Rows[0]["TABLE_SCHEMA"].ToString();
                item.Difinition = table.Rows[0]["VIEW_DEFINITION"].ToString();
            }

            return item;
        }

        public DbSchemaInfo GetDataBaseInfo()
        {
            throw new NotImplementedException();
        }
    }
}
