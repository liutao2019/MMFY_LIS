using Lib.DAC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    abstract class AbstractDataInterfaceConnection : IDataInterfaceConnection
    {
        public abstract IDataInterfaceCommand CreateCommand();

        public abstract bool TestConnection(out string errorMessage);

        public abstract SqlHelper GetSqlHelper();

        public string ServerAddress { get; set; }

        public string Catelog { get; set; }

        public string LoginName { get; set; }

        public string LoginPassword { get; set; }

        public string DbDriver { get; set; }

        public string DbDialet { get; set; }

        public override int GetHashCode()
        {
            string str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}"
             , this.ServerAddress
             , this.Catelog
             , this.LoginName
             , this.LoginPassword
             , this.DbDriver
             , this.DbDialet);

            return str.GetHashCode();
        }
    }
}
