using dcl.entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.dao.core
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public class DBManager
    {
        private static string dbName = ConfigurationManager.AppSettings["DCL.ExtDataInterface"];

        MD5Helper mdHelper;
        public DBManager()
        {
            mdHelper = new MD5Helper();
            IDbConnection dbc = GetDBConnection("ConnectionString");
            _DBconnection = dbc;
        }

        public DBManager(string Key)
        {
            mdHelper = new MD5Helper();
            IDbConnection dbc = GetDBConnection(Key);
            _DBconnection = dbc;
        }

        private IDbConnection _DBconnection = null;
        public IDbConnection DBConnection
        {
            get { return _DBconnection; }
            set
            {
                _DBconnection = value;
            }
        }


        private IDbTransaction _DbTransaction = null;
        public IDbTransaction DbTransaction
        {
            get { return _DbTransaction; }
            set
            {
                _DbTransaction = value;
            }
        }


        public IDbTransaction BeginTrans()
        {
            if (DBConnection.State == ConnectionState.Closed)
            {
                DBConnection.Open();
            }
            _DbTransaction = DBConnection.BeginTransaction();
            return _DbTransaction;
        }
        public void CommitTrans()
        {
            try
            {
                _DbTransaction.Commit();

                if (_DBconnection != null && _DBconnection.State == ConnectionState.Open)
                {
                    _DBconnection.Close();
                }
            }
            finally
            {
                _DbTransaction.Dispose();
                _DbTransaction = null;
            }
        }

        public void RollbackTrans()
        {
            try
            {
                _DbTransaction.Rollback();
                if (_DBconnection != null && _DBconnection.State == ConnectionState.Open)
                {
                    _DBconnection.Close();
                }
            }
            finally
            {
                _DbTransaction.Dispose();
                _DbTransaction = null;
            }
        }

        private void CloseSetNullConnection()
        {
            _DBconnection.Close();
            _DBconnection.Dispose();
            _DBconnection = null;

        }
        private static void PrepareCommand(IDbCommand cmd, List<DbParameter> paramHt)
        {
            if (paramHt != null)
            {
                foreach (DbParameter deTemp in paramHt)
                {
                    DbParameter param = (DbParameter)cmd.CreateParameter();　//强转为DbParameter 
                    param.ParameterName = deTemp.ParameterName;
                    //获取参数值，MSDN:Parameter 对象的 Value 属性值会自动推断出 Parameter 的数据提供程序“类型”。
                    if (deTemp.Value == null)
                        deTemp.Value = DBNull.Value;
                    param.Value = deTemp.Value;
                    cmd.Parameters.Add(param);　//添加到cmd
                }
            }
        }

        private List<DbParameter> KillDoubleParameter(List<DbParameter> paramHt)
        {


            var OutPutParamList = from data in paramHt group data by data.ParameterName into g select g.First();
            return OutPutParamList.ToList();
        }

        private bool CheckDoubleParameter(List<DbParameter> paramHt, string ParamName)
        {
            foreach (DbParameter Parameter in paramHt)
            {
                if (ParamName.Equals(Parameter.ParameterName))
                    return true;
            }
            return false;
            //todo kim检查是否有重复饿参数
        }

        /// <summary>
        /// 执行一条SQL 语句 </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns>DataTable 类型的返回是这个语句的所筛选出来的数据</returns>
        public DataTable ExecSel(string sql)
        {
            DataTable Result = null;
            try
            {
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    Result = new DataTable();
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    using (IDataReader sdr = cmd.ExecuteReader())
                    {
                        Result.Load(sdr);

                    }
                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + sql);
                if (this._DbTransaction == null)
                    _DBconnection.Close();

                throw e;
            }
            return Result;
        }

        public void ExecSql(string sql)
        {
            try
            {
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    cmd.ExecuteNonQuery();
                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + sql);
                if (this._DbTransaction == null)
                    _DBconnection.Close();
                throw ex;
            }
        }

        public void ExecSql(string[] sql)
        {
            try
            {
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandType = CommandType.Text;

                    foreach (string item in sql)
                    {
                        cmd.CommandText = item;
                        cmd.ExecuteNonQuery();
                    }

                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + sql);
                if (this._DbTransaction == null)
                    _DBconnection.Close();
                throw ex;
            }
        }

        public IDataReader GetDataReader(string sql)
        {
            try
            {
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }

                    IDataReader da = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    _DBconnection.Close();
                    return da;
                }
            }
            catch (Exception ex)
            {
                _DBconnection.Close();
                throw ex;
            }
        }

        public int ExecCommand(string sql)
        {
            try
            {
                int Result;
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    Result = cmd.ExecuteNonQuery();
                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();
                    }
                    return Result;
                }
            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + sql);
                _DBconnection.Close();
                throw ex;
            }
        }

        public int ExecCommand(string sql, List<DbParameter> paramHt)
        {
            try
            {
                int Result = 0;
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    List<DbParameter> ParamList = KillDoubleParameter(paramHt);

                    PrepareCommand(cmd, ParamList);
                    paramHt.Clear();
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    Result = cmd.ExecuteNonQuery();

                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();

                    }
                    return Result;
                }
            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + sql);
                if (this._DbTransaction == null)
                    _DBconnection.Close();

                throw ex;
            }
        }

        public object SelOne(string sql, List<DbParameter> paramHt)
        {
            try
            {
                object Result = null;
                using (IDbCommand com = _DBconnection.CreateCommand())
                {
                    com.Transaction = this._DbTransaction;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    if (paramHt != null)
                    {
                        PrepareCommand(com, paramHt);
                        paramHt.Clear();
                    }
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }
                    Result = com.ExecuteScalar();
                    if (com.Transaction == null)
                    {
                        _DBconnection.Close();

                    }
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Execute_sp_DtSql(string Sqlstr, List<DbParameter> paramHt)
        {
            try
            {
                DataTable Result = new DataTable();
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = Sqlstr;
                    cmd.CommandType = CommandType.StoredProcedure;
                    PrepareCommand(cmd, paramHt);
                    if (paramHt != null)
                        paramHt.Clear();

                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        _DBconnection.Open();
                    }

                    using (IDataReader sdr = cmd.ExecuteReader())
                    {
                        Result.Load(sdr);
                        cmd.Parameters.Clear();
                    }
                    if (cmd.Transaction == null)
                    {
                        _DBconnection.Close();

                    }
                    return Result;
                }

            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + Sqlstr);
                _DBconnection.Close();
                throw ex;
            }
        }

        public DataTable ExecuteDtSql(string Sqlstr, List<DbParameter> paramHt)
        {
            try
            {
                DataTable Result = new DataTable();
                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = Sqlstr;
                    cmd.CommandType = CommandType.Text;

                    PrepareCommand(cmd, paramHt);
                    if (paramHt != null)
                        paramHt.Clear();

                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        lock (_DBconnection)
                        {
                            _DBconnection.Open();
                        }
                    }


                    using (IDataReader sdr = cmd.ExecuteReader())
                    {

                        Result.Load(sdr);
                        cmd.Parameters.Clear();
                    }
                    lock (_DBconnection)
                    {
                        if (cmd.Transaction == null)
                        {
                            _DBconnection.Close();

                        }
                    }
                    return Result;
                }

            }
            catch (Exception ex)
            {
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + Sqlstr);
                _DBconnection.Close();
                throw ex;

            }
        }

        public DataTable ExecuteDtSql(string Sqlstr)
        {
            try
            {
                DataTable Result = new DataTable();

                using (IDbCommand cmd = _DBconnection.CreateCommand())
                {
                    cmd.Transaction = this._DbTransaction;
                    cmd.CommandText = Sqlstr;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;
                    if (_DBconnection.State == ConnectionState.Closed)
                    {
                        lock (_DBconnection)
                        {
                            _DBconnection.Open();
                        }
                    }
                    using (IDataReader sdr = cmd.ExecuteReader())
                    {
                        Result.Load(sdr);
                        cmd.Parameters.Clear();
                    }
                    lock (_DBconnection)
                    {
                        if (cmd.Transaction == null)
                        {
                            _DBconnection.Close();

                        }
                    }
                    return Result;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                if (_DBconnection != null)
                    Lib.LogManager.Logger.LogInfo("DBconnection.State:" + _DBconnection.State.ToString() + "\r\n" + Sqlstr);
                _DBconnection.Close();
                throw ex;
            }
        }

        public void CloseConnection()
        {
            _DBconnection.Close();
        }

        public IDbConnection GetDBConnection(string Key)
        {
            string con = ConfigurationManager.AppSettings[Key];
            //支持constr
            if (con == null)
            {
                con = Key;
            }
            con = mdHelper.DecryptString(con);
            IDbConnection FDBconnection = new SqlConnection(con);
            return FDBconnection as SqlConnection;
        }

        ConnectionStringSettings GetConnectionSetting(string Key)
        {
            ConnectionStringSettings Result = ConfigurationManager.ConnectionStrings[Key];
            if (Result == null)
            {
                Result = new ConnectionStringSettings();
                Result.ConnectionString = Key;
            }
            return Result;

        }

        private string GetRemoteConnectionKey(string ProviderName)
        {
            string Result = string.Empty;
            try
            {
                Result = ProviderName.Split('.')[1];
            }
            catch (Exception)
            {
            }

            return Result;
        }

        public string GetInsertSQL(string TableName, List<DbParameter> paramHt)
        {
            string Fields = "";
            string value = "";
            string insertSQL = "insert into {0} ({1}) values ({2})";
            foreach (var v in paramHt)
            {
                if (v.ParameterName != paramHt.Last().ParameterName)
                {
                    Fields += v.ParameterName + ",";
                    value += "@" + v.ParameterName + ",";
                }
                else
                {
                    Fields += v.ParameterName;
                    value += "@" + v.ParameterName;
                }
            }
            return string.Format(insertSQL, TableName, Fields, value);

        }

        /// <summary>
        /// 动态构造Sql语句，实现数据库插入操作
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="values">值字典</param>
        public int InsertOperation(string TableName, Dictionary<string, object> values)
        {
            string Fields = string.Empty;
            string value = string.Empty;
            string insertSQL = "insert into {0} ({1}) values ({2})";
            List<DbParameter> paraList = new List<DbParameter>();
            foreach (var v in values)
            {
                Fields += ",[" + v.Key + "] ";
                value += ",@" + v.Key + " ";
                paraList.Add(new SqlParameter("@" + v.Key, v.Value));
            }
            if (values.Count > 0)
            {
                Fields = Fields.Substring(1);
                value = value.Substring(1);
            }
            string sql = string.Format(insertSQL, TableName, Fields, value);
            return ExecCommand(sql, paraList);//调用数据库处理类
        }

        /// <summary>
        /// 动态构造Sql语句，实现数据库删除操作
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Keys">键值</param>
        public int DeleteOperation(string TableName, Dictionary<string, object> Keys)
        {
            string Fields = string.Empty;
            string DeleteSql = "delete {0} where  ({1})";
            List<DbParameter> paraList = new List<DbParameter>();
            foreach (var v in Keys)
            {
                Fields += " and [" + v.Key + "] = @" + v.Key;
                paraList.Add(new SqlParameter("@" + v.Key, v.Value));
            }
            if (Keys.Count > 0)
                Fields = Fields.Substring(5);
            string sql = string.Format(DeleteSql, TableName, Fields);
            return ExecCommand(sql, paraList);//调用数据库处理类
        }


        /// <summary>
        /// 动态构造Sql语句，实现数据库删除操作
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Keys">键值</param>
        public int UpdateOperation(string TableName, Dictionary<string, object> values, Dictionary<string, object> Keys)
        {
            string UpdateFields = string.Empty;
            string WhereFields = string.Empty;
            string UpdateSQL = "update {0} set {1} where ({2})";
            List<DbParameter> paraList = new List<DbParameter>();
            foreach (var v in values)
            {
                UpdateFields += ", [" + v.Key + "] = @" + v.Key;
                paraList.Add(new SqlParameter("@" + v.Key, v.Value));
            }
            UpdateFields = UpdateFields.Substring(1);


            foreach (var v in Keys)
            {
                string KeyParamName = v.Key;
                if (CheckDoubleParameter(paraList, "@" + KeyParamName))
                {
                    KeyParamName += KeyParamName;
                }
                WhereFields += " and [" + v.Key + "] = @" + KeyParamName;
                paraList.Add(new SqlParameter("@" + KeyParamName, v.Value));
            }

            if (Keys.Count > 0)
                WhereFields = WhereFields.Substring(4);

            string sql = string.Format(UpdateSQL, TableName, UpdateFields, WhereFields);
            return ExecCommand(sql, paraList);//调用数据库处理类
        }

        /// <summary>
        /// 实体转换成保存数据参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dictionary<string, object> ConverToDBSaveParameter<T>(T entity)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            PropertyInfo[] propertys = entity.GetType().GetProperties();

            DateTime standareTime = Convert.ToDateTime("1800-01-01");
            DateTime tempTime;
            foreach (PropertyInfo item in propertys)
            {
                var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    FieldMapAttribute map = (FieldMapAttribute)attribute;

                    string columnName = string.Empty;
                    if (dbName.Contains("clab"))
                        columnName = map.ClabName;
                    if (dbName.Contains("med"))
                        columnName = map.MedName;
                    if (dbName.Contains("wf"))
                        columnName = map.WFName;

                    if (!string.IsNullOrEmpty(columnName) && map.DBColumn && !map.DBIdentity)
                    {
                        //if (result.ContainsKey(columnName))
                        //    continue;

                        object value = item.GetValue(entity, null);
                        if (value != null)
                        {
                            if (value.GetType() == typeof(string))
                            {
                                if (value.ToString() != string.Empty)
                                    result.Add(columnName, value);
                            }
                            else if (value.GetType() == typeof(DateTime))
                            {
                                tempTime = Convert.ToDateTime(value);
                                if (tempTime > standareTime)
                                {
                                    result.Add(columnName, tempTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else
                                {
                                    //result.Add(columnName, value);
                                    result.Add(columnName, new DateTime(1900, 1, 1).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                            }
                            else if (value.GetType() == typeof(Boolean))
                            {
                                bool bl = (bool)value;

                                if (bl)
                                    result.Add(columnName, 1);
                                else
                                    result.Add(columnName, 0);
                            }
                            else
                                result.Add(columnName, value);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 实体转换成更新数据参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dictionary<string, object> ConverToDBUpdateParameter<T>(T entity)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            PropertyInfo[] propertys = entity.GetType().GetProperties();
            DateTime standareTime = Convert.ToDateTime("1800-01-01");
            DateTime tempTime;
            foreach (PropertyInfo item in propertys)
            {
                var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    FieldMapAttribute map = (FieldMapAttribute)attribute;

                    string columnName = string.Empty;
                    if (dbName.Contains("clab"))
                        columnName = map.ClabName;
                    if (dbName.Contains("med"))
                        columnName = map.MedName;
                    if (dbName.Contains("wf"))
                        columnName = map.WFName;

                    if (!string.IsNullOrEmpty(columnName) && map.DBColumn && !map.DBIdentity)
                    {
                        //if (result.ContainsKey(columnName))
                        //    continue;

                        object value = item.GetValue(entity, null);
                        if (value != null && value.GetType() == typeof(DateTime))
                        {
                            tempTime = Convert.ToDateTime(value);
                            if (tempTime > standareTime)
                            {
                                result.Add(columnName, tempTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                //result.Add(columnName, value);
                                result.Add(columnName, new DateTime(1900, 1, 1).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                        else if (value != null && value.GetType() == typeof(Boolean))
                        {
                            bool bl = (bool)value;

                            if (bl)
                                result.Add(columnName, 1);
                            else
                                result.Add(columnName, 0);
                        }
                        else
                            result.Add(columnName, value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 实体转换成SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertys"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ConverToDBSQL<T>(T entity, PropertyInfo[] propertys, string name)
        {
            string result = string.Empty;

            string strColumn = string.Empty;

            string strValue = string.Empty;
            DateTime standareTime = Convert.ToDateTime("1800-01-01");
            DateTime tempTime;
            foreach (PropertyInfo item in propertys)
            {
                var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    FieldMapAttribute map = (FieldMapAttribute)attribute;
                    string columnName = string.Empty;
                    if (dbName.Contains("clab"))
                        columnName = map.ClabName;
                    if (dbName.Contains("med"))
                        columnName = map.MedName;
                    if (dbName.Contains("wf"))
                        columnName = map.WFName;

                    if (!string.IsNullOrEmpty(columnName) && map.DBColumn && !map.DBIdentity)
                    {
                        object value = item.GetValue(entity, null);
                        if (value != null)
                        {
                            if (value.GetType() == typeof(string))
                            {
                                if (value.ToString() != string.Empty)
                                {
                                    strColumn += string.Format(",{0}", columnName);
                                    strValue += string.Format(",'{0}'", value.ToString().Replace("'", "''"));
                                }
                            }
                            else if (value.GetType() == typeof(DateTime))
                            {
                                tempTime = Convert.ToDateTime(value);
                                if (tempTime > standareTime)
                                {
                                    strColumn += string.Format(",{0}", columnName);
                                    strValue += string.Format(",'{0}'",tempTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                            }
                            else if (value.GetType() == typeof(Boolean))
                            {
                                strColumn += string.Format(",{0}", columnName);

                                bool bl = (bool)value;

                                if (bl)
                                    strValue += string.Format(",{0}", 1);
                                else
                                    strValue += string.Format(",{0}", 0);
                            }
                            else
                            {
                                strColumn += string.Format(",{0}", columnName);
                                strValue += string.Format(",{0}", value.ToString());
                            }
                        }
                    }
                }
            }

            strColumn = strColumn.Remove(0, 1);
            strValue = strValue.Remove(0, 1);

            result = string.Format("insert into {0} ({1}) values ({2})", name, strColumn, strValue);

            return result;
        }

        /// <summary>
        /// add this method to GetInsertSqlBag
        /// 2017.09.15 Tiger
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public string GetInsertSQL(string TableName, Dictionary<String, object> parm)
        {
            string Fields = "";
            string value = "";
            string insertSQL = "insert into {0} ({1}) values ({2})";
            foreach (var v in parm)
            {
                Fields += ",[" + v.Key + "]";
                value += ",'" + v.Value + "'";
            }
            if (parm.Count > 0)
            {
                Fields = Fields.Substring(1);
                value = value.Substring(1);
            }
            string sql = string.Format(insertSQL, TableName, Fields, value);
            return sql;
        }

        public string GetDeleteSQL(string TableName, Dictionary<String, object> parm)
        {
            string Fields = "";
            string value = "";
            string whereStr = "";
            string insertSQL = "delete from {0}  where 1 = 1 {1} ";
            foreach (var v in parm)
            {
                Fields = "[" + v.Key + "]";
                value = "'" + v.Value + "'";
                whereStr += " and " + Fields + " = " + value;
            }

            string sql = string.Format(insertSQL, TableName, whereStr);
            return sql;
        }

        /// <summary>
        /// add this method to GetUpdateSqlBag
        /// 2017.09.15 Tiger
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Values"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public string GetUpdateSql(string TableName, Dictionary<String, object> Values,
            Dictionary<String, object> Keys, bool outPutInsert = false)
        {
            string updateSQL = string.Empty;
            if (outPutInsert)
                updateSQL = "update {0} set {1} where {2}  OUTPUT INSERTED.* ";
            else
                updateSQL = "update {0} set {1} where {2} ";

            string setPart = "";
            string wherePart = "";

            foreach (var v in Values)
            {
                if (v.Value == null)
                    setPart += ",[" + v.Key + "] = NULL ";
                else
                    setPart += ",[" + v.Key + "] = " + "'" + v.Value + "'";

            }
            if (Values.Count > 0)
            {
                setPart = setPart.Substring(1);
            }

            foreach (var v in Keys)
            {
                if (v.Value == null)
                    wherePart += " and [" + v.Key + "] IS NULL ";
                else
                    wherePart += " and [" + v.Key + "] = " + "'" + v.Value + "'";
            }
            if (Keys.Count > 0)
            {
                wherePart = wherePart.Substring(5);
            }
            return string.Format(updateSQL, TableName, setPart, wherePart);
        }

        /// <summary>
        /// 找出实体中值不为空的字段，保存到Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Dictionary<string, object> EntityToDictionary<T>(T entity,
            List<string> ignoreList, bool ignoreNull = true)
        {
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                PropertyInfo[] propertys = entity.GetType().GetProperties();
                DateTime standareTime = Convert.ToDateTime("1800-01-01");
                DateTime tempTime;
                foreach (PropertyInfo item in propertys)
                {
                    var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                    if (attribute != null)
                    {
                        FieldMapAttribute map = (FieldMapAttribute)attribute;

                        string columnName = string.Empty;
                        if (dbName.Contains("clab"))
                            columnName = map.ClabName;
                        if (dbName.Contains("med"))
                            columnName = map.MedName;
                        if (dbName.Contains("wf"))
                            columnName = map.WFName;

                        if (ignoreList != null && ignoreList.Contains(columnName))
                            continue;

                        if (!string.IsNullOrEmpty(columnName) && map.DBColumn && !map.DBIdentity)
                        {
                            //if (result.ContainsKey(columnName))
                            //    continue;

                            object value = item.GetValue(entity, null);
                            if (ignoreNull)//忽略为空的字段
                            {
                                if (value == null || string.IsNullOrEmpty(value.ToString()))
                                    continue;
                            }

                            if (value != null)
                            {
                                if (value.GetType() == typeof(string))
                                {
                                    if (value.ToString() != string.Empty)
                                        result.Add(columnName, value);
                                }
                                else if (value.GetType() == typeof(DateTime))
                                {
                                    tempTime = Convert.ToDateTime(value);
                                    if (tempTime > standareTime)
                                    {
                                        result.Add(columnName, tempTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    else
                                    {
                                        //result.Add(columnName, value);
                                        result.Add(columnName, new DateTime(1900, 1, 1).ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                }
                                else if (value.GetType() == typeof(Boolean))
                                {
                                    bool bl = (bool)value;

                                    if (bl)
                                        result.Add(columnName, 1);
                                    else
                                        result.Add(columnName, 0);
                                }
                                else
                                    result.Add(columnName, value);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
