using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using Lib.DAC.DbDriver;

namespace Lib.DAC.Connection
{
    /// <summary>
    /// 数据库连接提供对象
    /// </summary>
    public class ConnectionPrivider
    {
        #region singleton
        private static ConnectionPrivider _current = null;

        public static ConnectionPrivider Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ConnectionPrivider();
                }
                return _current;
            }
        }
        #endregion

        /// <summary>
        /// .ctor
        /// </summary>
        private ConnectionPrivider()
        {
            this.currentdriver = DbDriverHelper.GetCurrentDriver();
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="enumDriver"></param>
        /// <returns></returns>
        public static IDbConnection GetConnection(string connectionString, EnumDbDriver enumDriver)
        {
            IDbDriver driver = DbDriverHelper.GetDriver(enumDriver);
            IDbConnection conn = driver.CreateConnection(connectionString);
            return conn;
        }
        public static IDbConnection GetConnection(ConnectionStringProvider.ConnectionStringBuilder builder)
        {
            IDbConnection conn = builder.Driver.CreateConnection(builder.Build());
            return conn;
        }
        ///// <summary>
        ///// 获取数据库连接
        ///// </summary>
        ///// <param name="connectionString"></param>
        ///// <param name="driver"></param>
        ///// <returns></returns>
        //public static IDbConnection GetConnection(string connectionString, IDbDriver driver)
        //{
        //    IDbConnection conn = driver.CreateConnection(connectionString);
        //    return conn;
        //}

        /// <summary>
        /// 获取数据库连接(由配置制定)
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            //if (this.currentdriver == null)
            //{
            //    this.currentdriver = DBDriverProvider.GetCurrentDriver();
            //}
            IDbConnection conn = currentdriver.CreateConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        /// <summary>
        /// 获取当前数据库驱动
        /// </summary>
        internal Lib.DAC.IDbDriver CurrentDriver
        {
            get { return currentdriver; }
        }

        /// <summary>
        /// 获取当前配置的数据库驱动类型
        /// </summary>
        public EnumDbDriver CurrentDriverType
        {
            get { return DACConfig.Current.DriverType; }
        }

        /// <summary>
        /// 测试当前连接
        /// </summary>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public bool TestCurrentConnection(bool throwException)
        {
            bool success = false;
            IDbConnection conn = this.GetConnection();
            try
            {
                conn.Open();
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return success;
        }

        public static bool TestConnection(bool throwException, ConnectionStringProvider.ConnectionStringBuilder builder)
        {
            bool success = false;
            IDbConnection conn = GetConnection(builder);
            try
            {
                conn.Open();
                success = true;
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return success;
        }

        public static bool TestConnection(bool throwException, string connectionString, EnumDbDriver driverType)
        {
            bool success = false;
            IDbConnection conn = GetConnection(connectionString, driverType);
            try
            {
                conn.Open();
                success = true;
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return success;
        }

        private Lib.DAC.IDbDriver currentdriver = null;
        //private EnumDBDriver _driverType;

        /// <summary>
        /// 当前配置的数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return DACConfig.Current.ConnectionString;
            }
        }
    }
}
