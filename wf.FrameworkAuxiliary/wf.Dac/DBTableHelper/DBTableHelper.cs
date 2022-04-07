using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Lib.DAC.DbSchema;


namespace Lib.DAC
{
    /// <summary>
    /// datatable操作类（增删改）
    /// </summary>
    public class DbTableHelper
    {
        DbSchemaInfo _dbInfo = null;
        EnumDbDriver _drivertype;
        EnumDataBaseDialet _dialettype;

        #region .ctor
        public DbTableHelper(string connectionString
                        , EnumDbDriver enumDriver
                        , EnumDataBaseDialet enumDialet)
        {
            this._dbInfo = new DbSchemaInfo(connectionString, enumDriver, enumDialet);
            this._drivertype = enumDriver;
            this._dialettype = enumDialet;
        }

        public DbTableHelper()
            : this(DACConfig.Current.ConnectionString
                   , DACConfig.Current.DriverType
                   , DACConfig.Current.DataBaseDialet)
        {
        }
        #endregion

        #region Insert
        /// <summary>
        /// 生成插入命令（如果数据库支持批量插入则请不要使用此方法插入数据）
        /// </summary>
        /// <param name="dbTableName">数据库表明</param>
        /// <param name="excluFields">要排除的插入字段</param>
        /// <param name="dtData">要插入的数据</param>
        /// <returns></returns>
        public DbCommandEx[] GenerateInsertCommand(string dbTableName, string[] excluFields, DataTable dtData)
        {
            DbSchemaTable tbInfo = _dbInfo.GetTable(dbTableName);

            if (tbInfo == null) throw new Exception(string.Format("找不到指定的表{0}", dbTableName));

            List<DbCommandEx> listCMD = new List<DbCommandEx>();

            //获取sql语句
            string sql = GetInsertText(tbInfo, excluFields, dtData);

            if (sql != null)
            {
                //遍历要插入的数据生成插入命令
                foreach (DataRow drData in dtData.Rows)
                {
                    DbCommandEx cmd = new DbCommandEx(this._drivertype, this._dialettype);
                    cmd.CommandText = sql;

                    //遍历数据结构的每一列
                    foreach (DbSchemaColumn col in tbInfo.Columns)
                    {
                        string colName = col.ColumnName;
                        if (drData.Table.Columns.Contains(colName)
                            && (excluFields == null || !ExistedString(excluFields, colName, false))
                            )
                        {
                            cmd.AddParameterValue(drData[colName]);
                        }
                    }

                    listCMD.Add(cmd);
                }
            }
            return listCMD.ToArray();
        }

        /// <summary>
        /// 生成sql文本
        /// </summary>
        /// <param name="tbSchema"></param>
        /// <param name="excluFields"></param>
        /// <param name="tbToInsert"></param>
        /// <returns></returns>
        private string GetInsertText(DbSchemaTable tbSchema, string[] excluFields, DataTable tbToInsert)
        {
            string ret = null;

            if (tbSchema.Columns.Count > 0 && tbToInsert != null)
            {
                //要插入的字段
                StringBuilder sbNamesPart = new StringBuilder();

                //变量占位符
                StringBuilder sbValuesPart = new StringBuilder();

                foreach (DbSchemaColumn col in tbSchema.Columns)
                {
                    if (
                        (excluFields == null || !ExistedString(excluFields, col.ColumnName, false))//当前的数据库列不在排除字段中
                        && (tbToInsert == null || tbToInsert.Columns.Contains(col.ColumnName))
                        )
                    {
                        sbNamesPart.Append(string.Format("{0}{1}", SqlStringConst.CommaSpace, col.ColumnName));
                        sbValuesPart.Append(string.Format("{0}?", SqlStringConst.CommaSpace));
                    }
                }

                sbNamesPart.Remove(0, SqlStringConst.CommaSpace.Length);
                sbValuesPart.Remove(0, SqlStringConst.CommaSpace.Length);
                ret = string.Format("Insert into {0}({1}) Values({2})", tbSchema.TableName, sbNamesPart, sbValuesPart);
            }
            return ret;
        }
        #endregion

        public DbCommandEx[] GenerateUpdateCommand(string tablename, string[] keys, string[] excluseFields, DataTable tbToUpdate)
        {
            if (keys == null || keys.Length == 0)
                throw new Exception("没有指定主键");

            DbSchemaTable tbSchema = _dbInfo.GetTable(tablename);

            if (tbSchema == null) throw new Exception(string.Format("找不到指定的表{0}", tablename));

            List<DbCommandEx> listCMD = new List<DbCommandEx>();

            //获取sql语句
            string sql = GetUpdateText(tbSchema, keys, excluseFields, tbToUpdate);

            if (sql != null)
            {
                //遍历要插入的数据生成更新命令
                foreach (DataRow drData in tbToUpdate.Rows)
                {
                    DbCommandEx cmd = new DbCommandEx(this._drivertype, this._dialettype);
                    cmd.CommandText = sql;

                    List<object> fieldValue = new List<object>();
                    List<object> keyValue = new List<object>();

                    foreach (DbSchemaColumn col in tbSchema.Columns)
                    {
                        if (tbToUpdate.Columns.Contains(col.ColumnName))
                        {
                            if (ExistedString(keys, col.ColumnName, false))
                            {
                                object val = drData[col.ColumnName];
                                keyValue.Add(val);
                            }
                            else if (excluseFields == null || !ExistedString(excluseFields, col.ColumnName, false))
                            {
                                object val = drData[col.ColumnName];
                                fieldValue.Add(val);
                            }
                        }
                    }

                    foreach (object val in fieldValue)
                    {
                        cmd.AddParameterValue(val);
                    }

                    foreach (object val in keyValue)
                    {
                        cmd.AddParameterValue(val);
                    }

                    listCMD.Add(cmd);
                }
            }

            return listCMD.ToArray();
        }

        private string GetUpdateText(DbSchemaTable tbSchema, string[] keys, string[] excluseFields, DataTable tbToUpdate)
        {
            string ret = null;

            if (tbSchema.Columns.Count > 0 && tbToUpdate != null)
            {
                //要更新的字段
                StringBuilder sbFieldPart = new StringBuilder();

                //主键列
                StringBuilder sbKeyPart = new StringBuilder();

                foreach (DbSchemaColumn col in tbSchema.Columns)
                {
                    if (tbToUpdate.Columns.Contains(col.ColumnName))
                    {
                        if (ExistedString(keys, col.ColumnName, false))
                        {
                            sbKeyPart.Append(string.Format("and {1} = ? ", SqlStringConst.CommaSpace, col.ColumnName));
                        }
                        else if (excluseFields == null || !ExistedString(excluseFields, col.ColumnName, false))
                        {
                            sbFieldPart.Append(string.Format("{0}{1} = ? ", SqlStringConst.CommaSpace, col.ColumnName));
                        }
                    }
                }

                if (sbFieldPart.Length > 0)
                    sbFieldPart.Remove(0, SqlStringConst.CommaSpace.Length);

                if (sbKeyPart.Length > 0)
                    sbKeyPart.Remove(0, 3);

                ret = string.Format("update {0} set {1} where {2}", tbSchema.TableName, sbFieldPart, sbKeyPart);
            }
            return ret;
        }

        //private static string GetInsertText(DbSchemaTable tableInfo, List<string> excluFields, DataTable dtData)
        //{
        //    string ret = null;

        //    if (tableInfo.Columns.Count > 0)
        //    {
        //        StringBuilder sbNamesPart = new StringBuilder();
        //        StringBuilder sbValuesPart = new StringBuilder();

        //        foreach (DbSchemaColumn col in tableInfo.Columns)
        //        {
        //            if (
        //                (excluFields == null || !excluFields.Exists(i => i == col.ColumnName))
        //                && (dtData == null || dtData.Columns.Contains(col.ColumnName))
        //                )
        //            {
        //                sbNamesPart.Append(string.Format("{0}[{1}]", SqlStringConst.CommaSpace, col.ColumnName));
        //                sbValuesPart.Append(string.Format("{0}?", SqlStringConst.CommaSpace));
        //            }
        //        }

        //        sbNamesPart.Remove(0, SqlStringConst.CommaSpace.Length);
        //        sbValuesPart.Remove(0, SqlStringConst.CommaSpace.Length);
        //        ret = string.Format("Insert into [{0}]({1}) Values({2})", tableInfo.TableName, sbNamesPart, sbValuesPart);

        //        //BasicSqlFormatter formatter = new BasicSqlFormatter();
        //        //ret = formatter.Format(ret);
        //    }

        //    return ret;
        //}

        private bool ExistedString(string[] collection, string val, bool caseSensitve)
        {
            foreach (string item in collection)
            {
                if (caseSensitve)
                {
                    if (item == val)
                        return true;
                }
                else
                {
                    if (item != null
                        && val != null
                        && item.ToLower() == val.ToLower()
                        )
                        return true;
                }
            }
            return false;
        }
    }
}
