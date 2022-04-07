using System;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface
{
    class SqlDataInterfaceConnection : AbstractDataInterfaceConnection
    {
        private Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder GetConnectionStringBuilder()
        {
            EnumDataBaseDialet dialet = Lib.DAC.DbDialetHelper.GetDialetTypeByName(this.DbDialet);
            EnumDbDriver driver = Lib.DAC.DbDriverHelper.GetDriverTypeByName(this.DbDriver);

            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder connbuilder =
            Lib.DAC.ConnectionStringProvider.ConnectionStrBuilderHelper.CreateConnStrBuilder(driver, dialet);
            connbuilder.Server = this.ServerAddress;
            connbuilder.DbName = this.Catelog;
            connbuilder.LoginName = this.LoginName;
            connbuilder.LoginPassword = this.LoginPassword;
            return connbuilder;
        }

        internal SqlHelper CreateSqlHelper()
        {
            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder connbuilder = GetConnectionStringBuilder();

            SqlHelper helper = new SqlHelper(connbuilder.Build(), connbuilder.Driver, connbuilder.Dialet);
            return helper;
        }


        public override SqlHelper GetSqlHelper()
        {
            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder connbuilder = GetConnectionStringBuilder();
            SqlHelper helper = new SqlHelper(connbuilder.Build(), connbuilder.Driver, connbuilder.Dialet);
            return helper;
        }


        internal EntityHelper CreateEntityHelper()
        {
            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder connbuilder = GetConnectionStringBuilder();

            EntityHelper helper = new EntityHelper(connbuilder.Build(), connbuilder.DriverType, connbuilder.DbType);
            return helper;
        }

        public override IDataInterfaceCommand CreateCommand()
        {
            SqlDataInterfaceCommand cmd = new SqlDataInterfaceCommand();
            return cmd;
        }

        public override bool TestConnection(out string errorMessage)
        {
            bool success = false;
            errorMessage = null;
            try
            {
                success = Lib.DAC.Connection.ConnectionPrivider.TestConnection(true, GetConnectionStringBuilder());
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //throw;
            }
            return success;
        }
    }
}
