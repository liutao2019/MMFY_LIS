using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    abstract class AbstractDataInterfaceCommand : IDataInterfaceCommand
    {
        public string CommandText { get; set; }
        public EnumCommandType CommandType { get; set; }

        public IDataInterfaceConnection Connection { get; set; }

        public IDataInterfaceParameterCollection Parameters { get; set; }

        public abstract IDataInterfaceParameter CreateParameter(string paramName);

        public abstract int ExecuteNonQuery();

        public abstract DataTable ExecuteGetDataTable();

        public abstract DataSet ExecuteGetDataSet();

        public abstract object ExecuteScalar();

        public override int GetHashCode()
        {
            string str = string.Format("{0}_{1}_{2}_{3}"
                 , this.CommandText
                 , this.CommandType
                 , this.Connection == null ? 0 : this.Connection.GetHashCode()
                 , this.Parameters == null ? 0 : this.Parameters.GetHashCode());

            return str.GetHashCode();
        }
    }
}
