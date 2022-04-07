using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;

namespace Lib.DataInterface
{
    class SqlDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public SqlDataInterfaceCommand()
        {
            base.Parameters = (new SqlDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;
        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            SqlDataInterfaceParameter p = new SqlDataInterfaceParameter();
            p.Name = paramName;
            return p;
        }

        internal IDataInterfaceParameter CreateParameter(DataInterfaceParameter p)
        {
            SqlDataInterfaceParameter par = new SqlDataInterfaceParameter();
            par.Name = p.Name;
            par._p = p;
            return par;
        }


        public override int ExecuteNonQuery()
        {
            SqlHelper helper = CreateSqlHelper();
            IDbCommand cmd = PrepareSqlCommand();
            int i = helper.ExecuteNonQuery(cmd);
            AfterExecute(cmd);
            return i;
        }

        public override DataTable ExecuteGetDataTable()
        {
            SqlHelper helper = CreateSqlHelper();
            IDbCommand cmd = PrepareSqlCommand();
            DataTable table = helper.GetTable(cmd);

            AfterExecute(cmd);

            return table;
        }

        void PrepareParameters(IDbCommand cmd)
        {
            foreach (SqlDataInterfaceParameter item in this.Parameters)
            {
                IDbDataParameter par = cmd.CreateParameter();
                par.ParameterName = item.Name;

                switch (item.Direction)
                {
                    case EnumDataInterfaceParameterDirection.Input:
                        par.Direction = ParameterDirection.Input;
                        break;

                    case EnumDataInterfaceParameterDirection.Output:
                        par.Direction = ParameterDirection.Output;
                        break;

                    case EnumDataInterfaceParameterDirection.InputOutput:
                        par.Direction = ParameterDirection.InputOutput;
                        break;

                    case EnumDataInterfaceParameterDirection.ReturnValue:
                        par.Direction = ParameterDirection.ReturnValue;
                        break;

                    default:
                        par.Direction = ParameterDirection.Input;
                        break;
                }

                par.DbType = (DbType)Enum.Parse(typeof(DbType), item.DataType, true);

                par.Size = item.Length;

                if (item.Value == null)
                {
                    par.Value = DBNull.Value;
                }
                else
                {
                    par.Value = item.Value;
                }

                cmd.Parameters.Add(par);

                if (string.IsNullOrEmpty(item.Name))
                {
                    item.Name = par.ParameterName;
                    if (item._p != null)
                    {
                        item._p.Name = par.ParameterName;
                    }
                }
            }
        }

        IDbCommand PrepareSqlCommand()
        {
            SqlHelper helper = CreateSqlHelper();
            IDbCommand cmd = helper.CreateCommand(this.CommandText);
            PrepareParameters(cmd);

            cmd.CommandType = (System.Data.CommandType)Enum.Parse(typeof(System.Data.CommandType), this.CommandType.ToString(), true);
            return cmd;
        }

        private SqlHelper CreateSqlHelper()
        {
            if (this.Connection == null)
            {
                return new SqlHelper();
            }
            else
            {
                return (this.Connection as SqlDataInterfaceConnection).CreateSqlHelper();
            }
        }

        void AfterExecute(IDbCommand cmd)
        {
            foreach (IDbDataParameter sqlpar in cmd.Parameters)
            {
                if (sqlpar.Direction == ParameterDirection.Output
                    || sqlpar.Direction == ParameterDirection.ReturnValue)
                {
                    string pName = sqlpar.ParameterName;
                    SqlDataInterfaceParameter p = this.Parameters[pName];
                    if (p != null)
                    {
                        p.Value = sqlpar.Value;
                    }
                }
            }
        }

        SqlDataInterfaceParameterCollection _parameters = new SqlDataInterfaceParameterCollection();

        public new SqlDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as SqlDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as IDataInterfaceParameterCollection;
            }
        }

        public override DataSet ExecuteGetDataSet()
        {
            SqlHelper helper = CreateSqlHelper();
            IDbCommand cmd = PrepareSqlCommand();
            DataSet ds = helper.GetDataSet(cmd);

            AfterExecute(cmd);

            return ds;
        }

        public override object ExecuteScalar()
        {
            SqlHelper helper = CreateSqlHelper();
            IDbCommand cmd = PrepareSqlCommand();
            object obj = helper.ExecuteScalar(cmd);

            AfterExecute(cmd);

            return obj;
        }
    }
}
