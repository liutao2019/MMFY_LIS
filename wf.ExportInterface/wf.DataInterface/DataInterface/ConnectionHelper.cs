using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    internal class ConnectionHelper
    {
        //public int ExecuteNonQuery(InterfaceCommandConfig cmd)
        //{
        //    IDataInterfaceConnection iconn = CreateConnection(cmd.ConnectionConfig);
        //    IDataInterfaceCommand icmd = CreateCommand(iconn, cmd);
        //    int i = icmd.ExecuteNonQuery();
        //    return i;
        //}

        //public DataTable ExecuteGetDataTable(InterfaceCommandConfig cmd)
        //{
        //    IDataInterfaceConnection iconn = CreateConnection(cmd.ConnectionConfig);
        //    IDataInterfaceCommand icmd = CreateCommand(iconn, cmd);
        //    DataTable table = icmd.ExecuteGetDataTable();
        //    return table;
        //}

        //public DataSet ExecuteGetDataSet(InterfaceCommandConfig cmd)
        //{
        //    IDataInterfaceConnection iconn = CreateConnection(cmd.ConnectionConfig);
        //    IDataInterfaceCommand icmd = CreateCommand(iconn, cmd);
        //    DataSet ds = icmd.ExecuteGetDataSet();
        //    return ds;
        //}

        //private IDataInterfaceConnection CreateConnection(InterfaceConnectionConfig connConfig)
        //{
        //    IDataInterfaceConnection conn = GetConnection(connConfig.ConnectionType);
        //    conn.ServerAddress = connConfig.ServerAddress;
        //    if (connConfig.ConnectionType == EnumDataInterfaceConnectionType.SQL)
        //    {
        //        conn.Catelog = connConfig.Catelog;
        //        conn.DbDialet = connConfig.DbDialet;
        //        conn.DbDriver = connConfig.DbDriver;
        //    }
        //    conn.LoginName = connConfig.LoginName;
        //    conn.LoginPassword = connConfig.LoginPassword;
        //    return conn;
        //}

        //private IDataInterfaceCommand CreateCommand(IDataInterfaceConnection conn, InterfaceCommandConfig cmdcfg)
        //{
        //    IDataInterfaceCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = cmdcfg.CommandText;
        //    cmd.CommandType = cmdcfg.CommandType;

        //    foreach (InterfaceParameterConfig parcfg in cmdcfg.Parameters)
        //    {
        //        IDataInterfaceParameter par = cmd.CreateParameter(parcfg.Name);
        //        par.DataType = parcfg.DataType;
        //        par.Direction = parcfg.Direction;
        //        par.Length = parcfg.Length;
        //        par.Value = parcfg.Value;
        //        par.ValueExpression = parcfg.ValueExpression;
        //        //par.Config = parcfg;
        //        cmd.Parameters.Add(par);
        //    }
        //    return cmd;
        //}

        /// <summary>
        /// 根据连接类型获取连接对象
        /// </summary>
        /// <param name="connType"></param>
        /// <returns></returns>
        public static IDataInterfaceConnection GetConnection(EnumDataInterfaceConnectionType connType)
        {
            IDataInterfaceConnection conn;
            switch (connType)
            {
                case EnumDataInterfaceConnectionType.SQL:
                    conn = new SqlDataInterfaceConnection();
                    break;

                case EnumDataInterfaceConnectionType.WebService:
                    conn = new WSDataInterfaceConnection();
                    break;

                case EnumDataInterfaceConnectionType.WCF:
                    throw new NotSupportedException();
                    conn = new WCFDataInterfaceConnection();
                    break;

                case EnumDataInterfaceConnectionType.DotNetDll:
                    conn = new NetDllDataInterfaceConnection();
                    break;

                case EnumDataInterfaceConnectionType.BiniaryDll:
                    conn = new BinDllDataInterfaceConnection();
                    break;

                case EnumDataInterfaceConnectionType.DOSCommand:
                    conn = new DOSCmdDataInterfaceConnection();
                    break;

                default:
                    throw new NotSupportedException(connType.ToString());
            }
            return conn;
        }
    }
}
