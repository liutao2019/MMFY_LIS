using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC.ConnectionStringProvider;
using Lib.DAC.Connection;

namespace Lib.DAC
{
    public class DACHelper
    {
        #region 启动事务
        /// <summary>
        /// 使用config配置的连接串与数据库配置 启动事务，默认隔离级别ReadCommitted
        /// </summary>
        /// <returns></returns>
        public static ITransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 使用指定的隔离级别，使用config配置的连接串与数据库配置 启动事务
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public static ITransaction BeginTransaction(IsolationLevel il)
        {
            return BeginTransaction(il, DACConfig.Current.ConnectionString, DACConfig.Current.DriverType, DACConfig.Current.DataBaseDialet);
        }

        /// <summary>
        /// 使用指定的连接串与指定的数据库配置 启动事务，默认隔离级别ReadCommitted
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="driver"></param>
        /// <param name="dialet"></param>
        /// <returns></returns>
        public static ITransaction BeginTransaction(string connStr, EnumDbDriver driver, EnumDataBaseDialet dialet)
        {
            return BeginTransaction(IsolationLevel.ReadCommitted, connStr, driver, dialet);
        }

        /// <summary>
        /// 使用指定的隔离级别，使用指定的连接串与指定的数据库配置 启动事务
        /// </summary>
        /// <param name="il"></param>
        /// <param name="connStr"></param>
        /// <param name="driver"></param>
        /// <param name="dialet"></param>
        /// <returns></returns>
        public static ITransaction BeginTransaction(IsolationLevel il, string connStr, EnumDbDriver driver, EnumDataBaseDialet dialet)
        {
            IDbConnection conn = GetConnection(connStr, driver);
            conn.Open();
            IDbTransaction transaction = conn.BeginTransaction(il);

            AdoTransaction tran = new AdoTransaction(transaction, DbDriverHelper.GetDriver(driver), DbDialetHelper.GetDialet(dialet));

            return tran;
        }
        #endregion

        /// <summary>
        /// 获取当前配置的数据库连接串
        /// </summary>
        /// <returns></returns>
        public static string CurrentConnectionString
        {
            get
            {
                return DACConfig.Current.ConnectionString;
            }
        }

        public static EnumDataBaseDialet CurrentDialet
        {
            get
            {
                return DACConfig.Current.DataBaseDialet;
            }
        }

        public static EnumDbDriver CurrentDriver
        {
            get
            {
                return DACConfig.Current.DriverType;
            }
        }

        /// <summary>
        /// 创建数据库连接串
        /// </summary>
        /// <param name="driver">驱动方式</param>
        /// <param name="dialet">数据库类型</param>
        /// <param name="serverAddress">地址</param>
        /// <param name="userID">登录用户</param>
        /// <param name="password">登录密码</param>
        /// <param name="catelog"></param>
        /// <returns></returns>
        public static string CreateConnectionString(
            EnumDbDriver driver
            , EnumDataBaseDialet dialet
            , string serverAddress
            , string userID
            , string password
            , string catelog
            )
        {
            ConnectionStringBuilder builder = ConnectionStrBuilderHelper.CreateConnStrBuilder(driver, dialet);
            builder.Server = serverAddress;
            builder.LoginName = userID;
            builder.LoginPassword = password;
            builder.DbName = catelog;
            string connStr = builder.Build();
            return connStr;
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="driver"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool TestConnection(string connStr, EnumDbDriver driver, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            try
            {
                success = ConnectionPrivider.TestConnection(true, connStr, driver);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return success;
        }

        /// <summary>
        /// 测试当前配置的数据库连接
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool TestCurrentConnection(out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            try
            {
                success = ConnectionPrivider.Current.TestCurrentConnection(true);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return success;
        }

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static IDbConnection GetConnection(string connStr, EnumDbDriver driver)
        {
            return DbDriverHelper.GetDriver(driver).CreateConnection(connStr);
        }

        /// <summary>
        /// 获取当前配置的数据库连接对象
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetCurrentConnection()
        {
            return ConnectionPrivider.Current.GetConnection();
        }
    }
}
