using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DataInterface.Implement;
using Lib.DAC;

namespace Lib.DataInterface
{
    /// <summary>
    /// 数据连接
    /// </summary>
    [Serializable]
    public class DataInterfaceConnection : ICloneable
    {
        public static DataInterfaceConnection FromDTO(EntityDictDataInterfaceConnection dtoConn)
        {
            DataInterfaceConnection conn = new DataInterfaceConnection();
            conn.Catelog = dtoConn.conn_db_catelog;
            conn.ConnectionType = (EnumDataInterfaceConnectionType)Enum.Parse(typeof(EnumDataInterfaceConnectionType), dtoConn.conn_type, true);
            conn.DbDialet = dtoConn.conn_db_dialet;
            conn.DbDriver = dtoConn.conn_db_driver;
            conn.LoginName = dtoConn.conn_login;
            conn.LoginPassword = dtoConn.conn_pass;
            conn.ServerAddress = dtoConn.conn_address;
            return conn;
        }

        /// <summary>
        /// 连接方式
        /// </summary>
        public EnumDataInterfaceConnectionType ConnectionType { get; set; }

        /// <summary>
        /// SQL：数据库地址
        /// WebService：WSDL地址
        /// WCF：WSDL地址
        /// DotNetDll：dll路径
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// SQL:数据库名称
        /// WebService：(忽略)
        /// WCF：(忽略)
        /// DotNetDll：类名
        /// </summary>
        public string Catelog { get; set; }

        /// <summary>
        /// SQL：数据库登录名
        /// WebService：(忽略)
        /// WCF：(忽略)
        /// DotNetDll：(忽略)
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// SQL：数据库登密码
        /// WebService：(忽略)
        /// WCF：(忽略)
        /// DotNetDll：(忽略)
        /// </summary>
        public string LoginPassword { get; set; }

        /// <summary>
        /// SQL：数据库所使用的驱动类型(MSSql\Oracle\Oledb\ODBC)
        /// WebService：(忽略)
        /// WCF：(忽略)
        /// DotNetDll：(忽略)
        /// </summary>
        public string DbDriver { get; set; }

        /// <summary>
        /// SQL：所使用的数据库
        /// WebService：(忽略)
        /// WCF：(忽略)
        /// DotNetDll：(忽略)
        /// </summary>
        public string DbDialet { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public DataInterfaceConnection()
        {
            this.ConnectionType = EnumDataInterfaceConnectionType.SQL;
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="errorMessage">连接失败的话返回错误信息</param>
        /// <returns>成功返回true</returns>
        public bool TestConnection(out string errorMessage)
        {
            IDataInterfaceConnection conn = GetExecuteConnection();
            bool success = conn.TestConnection(out errorMessage);
            return success;
        }

        public SqlHelper GetSqlHelper()
        {
            IDataInterfaceConnection conn = GetExecuteConnection();
            SqlHelper sqlHelper =  conn.GetSqlHelper();
            return sqlHelper;
        }

        internal IDataInterfaceConnection GetExecuteConnection()
        {
            IDataInterfaceConnection conn = ConnectionHelper.GetConnection(this.ConnectionType);
            conn.Catelog = this.Catelog;
            conn.DbDialet = this.DbDialet;
            conn.DbDriver = this.DbDriver;
            conn.LoginName = this.LoginName;
            conn.LoginPassword = this.LoginPassword;
            conn.ServerAddress = this.ServerAddress;
            return conn;
        }

        public override string ToString()
        {
            return this.ServerAddress;
        }
    }
}
