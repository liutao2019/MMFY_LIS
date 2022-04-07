using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.DbDriver;

namespace Lib.DAC.ConnectionStringProvider
{
    public abstract class ConnectionStringBuilder
    {
        internal ConnectionStringBuilder()
        {

        }
        public IDbDriver Driver
        {
            get;
            private set;
        }
        public IDialet Dialet
        {
            get;
            private set;
        }
        public EnumDbDriver DriverType
        {
            get;
            private set;
        }
        public EnumDataBaseDialet DbType
        {
            get;
            private set;
        }
        internal ConnectionStringBuilder(EnumDbDriver driverType)
        {
            Driver = DbDriverHelper.GetDriver(driverType);
            this.DriverType = driverType;
        }
        internal ConnectionStringBuilder(EnumDbDriver driverType, EnumDataBaseDialet dbType)
            : this(driverType)
        {
            Dialet = DbDialetHelper.GetDialet(dbType);
            this.DbType = dbType;

        }

        public string Server { get; set; }
        public string DbName { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }


        public abstract string Build();
    }
}
