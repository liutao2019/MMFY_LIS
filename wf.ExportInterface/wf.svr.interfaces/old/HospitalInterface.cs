using System;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using Lib.LogManager;
using dcl.common;
using Oracle.ManagedDataAccess.Client;
using dcl.entity;

namespace dcl.svr.interfaces
{
    public class HospitalInterface
    {
        public BarcodeDownloadInfo DownLoadInfo { get; set; }

        /// <summary>
        /// 接口信息
        /// </summary>
        public InterfaceInfo InterfaceInfo { get; set; }

        /// <summary> 连接字符串 </summary>
        public string ConnnectString
        {
            get
            {
                if (Connecter == null)
                    return "";
                return Connecter.ConnnectString;
            }
            set
            {
                if (Connecter != null)
                    Connecter.ConnnectString = value;
            }
        }

        /// <summary>
        /// 连接助手
        /// </summary>
        public IConnect Connecter { get; set; }

        public HospitalInterface(string dataBaseAddress, string dataBaseName, string user, string password, string dataBaseType, string sql)
        {
            this.InterfaceInfo = new InterfaceInfo(dataBaseAddress, dataBaseName, user, password, dataBaseType, sql);
            Connecter = IConnect.GenerateInstance(InterfaceInfo.DataBaseType);
            Connecter.InterfaceInfo = this.InterfaceInfo;
        }


        /// <summary>
        /// 生成数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public string GenerateConnString()
        {
            if (Connecter == null)
                return "";

            ConnnectString = Connecter.GenerateConnString();
            return ConnnectString;
        }

        /// <summary>
        /// 执行HIS接口
        /// </summary>
        /// <param name="param">参数</param>
        public DataSet ExeInterface(string param)
        {
            DataSet result = new DataSet();
            if (string.IsNullOrEmpty(param) || Connecter == null)
                return result;
            Connecter.DownLoadInfo = this.DownLoadInfo;
            result = Connecter.ExeInterface(param);


            return result;
        }

        public DataSet ExeInterface(string[] param)
        {
            DataSet result = new DataSet();
            if (param == null || param.Length < 0 || Connecter == null)
                return result;
            Connecter.DownLoadInfo = this.DownLoadInfo;

            result = Connecter.ExeInterface(param);


            return result;
        }

        public bool TestConnect()
        {
            bool result = false;

            result = Connecter.TestConnect();


            return result;
        }

    }

    public class InterfaceInfo
    {
        /// <summary>数据库地址</summary>
        public string DataBaseAddress { get; set; }

        /// <summary>数据库名</summary>
        public string DataBaseName { get; set; }

        /// <summary> 用户名 </summary>
        public string User { get; set; }

        /// <summary>  密码 </summary>
        public string Password { get; set; }

        /// <summary> 连接类型 </summary>
        public string DataBaseType { get; set; }

        /// <summary> 调用语句 </summary>
        public string SQL { get; set; }


        public InterfaceInfo(string dataBaseAddress, string dataBaseName, string user, string password, string dataBaseType, string sql)
        {
            DataBaseAddress = dataBaseAddress;
            DataBaseName = dataBaseName;
            User = user;
            Password = password;
            DataBaseType = dataBaseType;
            SQL = sql;
        }
    }

    /// <summary>
    /// 连接类型接口
    /// </summary>
    public abstract class IConnect
    {
        public BarcodeDownloadInfo DownLoadInfo { get; set; }

        private string connectString = "";
        /// <summary> 连接字符串 </summary>
        public string ConnnectString
        {
            get
            {
                if (connectString == "")
                {
                    connectString = GenerateConnString();
                }

                return connectString;
            }

            set { connectString = value; }
        }

        internal string ReplaceParameters(string[] param, string sql)
        {
            sql = InterfaceInfo.SQL;
            for (int i = 0; i < param.Length; i++)
            {
                sql = sql.Replace(":p" + (i + 1).ToString(), param[i]);
            }
            if (DownLoadInfo != null
                && !string.IsNullOrEmpty(this.DownLoadInfo.Conn_search_para)
               && !string.IsNullOrEmpty(this.DownLoadInfo.Conn_search_para_text)
                && param.Length > 0
                )
            {
                sql = sql.Replace(this.DownLoadInfo.Conn_search_para, string.Format(this.DownLoadInfo.Conn_search_para_text, param[0]));

            }
            for (int i = 0; i < 12; i++)
            {
                sql = sql.Replace(":p" + (i + 1).ToString(), "");
            }
            return sql;
        }

        /// <summary>
        /// 接口信息
        /// </summary>
        public InterfaceInfo InterfaceInfo { get; set; }

        /// <summary>
        /// 测试连接
        /// </summary>
        public abstract bool TestConnect();
        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns>多表组成的结果</returns>
        public abstract DataSet ExeInterface(string param);
        /// <summary>
        /// 生成连接字符串
        /// </summary>
        public abstract string GenerateConnString();

        public const string SQLServer = "sql server";
        public const string Oracle = "oracle";
        public const string ODBC = "odbc";
        public const string SybaseODBC = "sybaseodbc";
        public const string OleDB = "oledb";
        public const string OutlinkZY = "outlinkzy";
        public const string OutlinkMZ = "outlinkmy";

        public const string ParamString = ":p1";

        public static IConnect GenerateInstance(string connectType)
        {
            IConnect connect = null;
            switch (connectType.ToLower().Trim())
            {
                case SQLServer:
                    connect = new SqlServerConnect();
                    break;
                case Oracle:
                    connect = new OracleClientConnect();
                    break;
                case ODBC:
                    connect = new ODBCConnect();
                    break;

                case SybaseODBC:
                    connect = new SybaseODBCConnect();
                    break;

                case OleDB:
                    connect = new OleConnect();
                    break;
                case OutlinkZY:
                    connect = new OutlinkZYConnect();
                    break;
                case OutlinkMZ:
                    connect = new OutlinkMZConnect();
                    break;
                default:
                    break;
            }

            return connect;
        }

        public abstract DataSet ExeInterface(string[] param);

    }

    /// <summary>
    /// 佛山市一OUTLINK
    /// </summary>
    public abstract class OutlinkConnect : IConnect
    {

        public override bool TestConnect()
        {
            try
            {
                string result = Outlink.GetClinPat("Mzno=\"1180017\";");
                if (string.IsNullOrEmpty(result))
                    return true;
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public override abstract DataSet ExeInterface(string param);


        public override string GenerateConnString()
        {
            return "";
        }

        public override DataSet ExeInterface(string[] param)
        {
            throw new NotImplementedException();
        }
    }

    public class OutlinkMZConnect : OutlinkConnect
    {

        public override DataSet ExeInterface(string param)
        {
            string result = Outlink.GetClinPat(param);
            ConvertHelper helper = new ConvertHelper();
            return helper.ConvertToDataSet(result, SplitType.MzInfo);
        }
    }

    public class OutlinkZYConnect : OutlinkConnect
    {
        public override DataSet ExeInterface(string param)
        {
            string result = Outlink.GetWardPat(param);
            ConvertHelper helper = new ConvertHelper();
            return helper.ConvertToDataSet(result, SplitType.PatInfo);
        }
    }

    /// <summary>
    /// SQL Server连接方式
    /// </summary>
    public class SqlServerConnect : IConnect
    {
        public override bool TestConnect()
        {
            bool result = false;
            SqlConnection conn = new SqlConnection(ConnnectString);
            try
            {
                conn.Open();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.LogException("数据库连接", ex);
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override DataSet ExeInterface(string param)
        {
            DataSet result = new DataSet();
            SqlConnection conn = new SqlConnection(ConnnectString);
            try
            {
                //生成执行语句
                string sql = InterfaceInfo.SQL.Replace(ParamString, param);
                SqlCommand cmd = new SqlCommand(sql, conn);
                Lib.LogManager.Logger.LogInfo(sql);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override string GenerateConnString()
        {
            string result = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.UserID = InterfaceInfo.User;
            builder.Password = InterfaceInfo.Password;
            builder.DataSource = InterfaceInfo.DataBaseAddress;
            builder.InitialCatalog = InterfaceInfo.DataBaseName;
            result = builder.ConnectionString;

            return result;
        }

        public override DataSet ExeInterface(string[] param)
        {
            DataSet result = new DataSet();
            SqlConnection conn = new SqlConnection(ConnnectString);
            try
            {
                //生成执行语句
                string sql = "";
                sql = ReplaceParameters(param, sql);
                SqlCommand cmd = new SqlCommand(sql, conn);
                Lib.LogManager.Logger.LogInfo(sql);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }

    /// <summary>
    /// Oracle Client连接方式 
    /// </summary>
    public class OracleClientConnect : IConnect
    {
        public override bool TestConnect()
        {
            bool result = false;
            OracleConnection conn = new OracleConnection(ConnnectString);
            try
            {
                conn.Open();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.LogException("数据库连接", ex);
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override DataSet ExeInterface(string param)
        {
            DataSet result = new DataSet();
            OracleConnection conn = new OracleConnection(ConnnectString);
            try
            {

                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                string sql = InterfaceInfo.SQL.Replace(ParamString, param);
                cmd.CommandText = sql;
                Lib.LogManager.Logger.LogInfo(sql);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override string GenerateConnString()
        {
            return string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));Persist Security Info=True;User ID={2};Password={3};", InterfaceInfo.DataBaseAddress, InterfaceInfo.DataBaseName, InterfaceInfo.User, InterfaceInfo.Password);
            //return string.Format("Data Source={0};user={1};password={2};", InterfaceInfo.DataBaseName, InterfaceInfo.User, InterfaceInfo.Password);
        }

        public override DataSet ExeInterface(string[] param)
        {
            DataSet result = new DataSet();
            OracleConnection conn = new OracleConnection(ConnnectString);
            try
            {

                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                string sql = "";
                sql = ReplaceParameters(param, sql);
                cmd.CommandText = sql;
                Lib.LogManager.Logger.LogInfo(sql);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }

    /// <summary>
    /// ODBC连接方式 
    /// </summary>
    public class ODBCConnect : IConnect
    {
        public override bool TestConnect()
        {
            bool result = false;

            OdbcConnection conn = new OdbcConnection(ConnnectString);
            try
            {
                conn.Open();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.LogException("数据库连接", ex);
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override DataSet ExeInterface(string param)
        {
            DataSet result = new DataSet();
            OdbcConnection conn = new OdbcConnection(ConnnectString);
            try
            {

                conn.Open();
                OdbcCommand cmd = conn.CreateCommand();
                string sql = InterfaceInfo.SQL.Replace(ParamString, param);
                cmd.CommandText = sql;
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Logger.LogException("HospitalInterface", ex);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override string GenerateConnString()
        {
            // return "Driver={Microsoft ODBC for oracle};" + string.Format(" SERVER={0};UID={1};PWD={2}", InterfaceInfo.DataBaseAddress, InterfaceInfo.User, InterfaceInfo.Password);
            //return string.Format("DSN={0};UID={1};PWD={2}", InterfaceInfo.DataBaseAddress, InterfaceInfo.User, InterfaceInfo.Password);
            return string.Format("DSN={0};UID={1};PWD={2}", InterfaceInfo.DataBaseAddress, InterfaceInfo.User, InterfaceInfo.Password);

            // return string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));Persist Security Info=True;User ID={2};Password={3};", InterfaceInfo.DataBaseAddress, InterfaceInfo.DataBaseName, InterfaceInfo.User, InterfaceInfo.Password);
        }

        public override DataSet ExeInterface(string[] param)
        {
            DataSet result = new DataSet();
            OdbcConnection conn = new OdbcConnection(ConnnectString);
            try
            {

                conn.Open();
                OdbcCommand cmd = conn.CreateCommand();
                string sql = "";
                sql = ReplaceParameters(param, sql);


                cmd.CommandText = sql;
                OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Logger.LogException("HospitalInterface", ex);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


    }

    public class SybaseODBCConnect : ODBCConnect
    {
        public override string GenerateConnString()
        {
            return "provider={sybase ASE ODBC Driver};" + string.Format("DSN={0};UID={1};PWD={2}", InterfaceInfo.DataBaseAddress, InterfaceInfo.User, InterfaceInfo.Password);
        }
    }

    public class OleConnect : IConnect
    {

        public override bool TestConnect()
        {
            bool result = false;

            OleDbConnection conn = new OleDbConnection(ConnnectString);
            try
            {
                conn.Open();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.LogException("数据库连接", ex);
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override DataSet ExeInterface(string param)
        {
            DataSet result = new DataSet();
            OleDbConnection conn = new OleDbConnection(ConnnectString);
            try
            {

                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                string sql = InterfaceInfo.SQL.Replace(ParamString, param);
                Logger.LogInfo("Ole", sql);
                cmd.CommandText = sql;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public override string GenerateConnString()
        {
            return string.Format("Provider=msdaora;Data Source={0};User Id={1};Password={2};", InterfaceInfo.DataBaseAddress, InterfaceInfo.User, InterfaceInfo.Password);
        }

        public override DataSet ExeInterface(string[] param)
        {
            DataSet result = new DataSet();
            OleDbConnection conn = new OleDbConnection(ConnnectString);
            try
            {

                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                string sql = "";
                sql = ReplaceParameters(param, sql);
                Logger.LogInfo("Ole", sql);
                cmd.CommandText = sql;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(result);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }


}