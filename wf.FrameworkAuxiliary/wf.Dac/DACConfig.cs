using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    /// <summary>
    /// 数据访问配置类
    /// </summary>
    internal class DACConfig
    {
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        public string ConnectionString
        {
            get
            {
                return this._connString;
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public EnumDbDriver DriverType
        {
            get
            {
                return this._databasedriver;
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public EnumDataBaseDialet DataBaseDialet
        {
            get
            {
                return this._databasedialet;
            }
        }


        static DACConfig _current = new DACConfig();

        public IDbDriver Driver { get; private set; }
        public IDialet Dialet { get; private set; }

        public static DACConfig Current
        {
            get
            {
                return _current;
            }
        }



        private string _connString = null;

        private EnumDbDriver _databasedriver = EnumDbDriver.MSSql;

        private EnumDataBaseDialet _databasedialet = EnumDataBaseDialet.SQL2005;

        private DACConfig()
        {
            try
            {
                InitParams();
            }
            catch
            {

                throw;
            }

        }

        void Current_AppSettingChanged(object sender, EventArgs args)
        {
            InitParams();
        }

        private void InitParams()
        {

            this._connString = new MD5Helper().DecryptString(ConfigurationManager.AppSettings["ConnectionString"]);

            #region driver
            string drivername;

            drivername = ConfigurationManager.AppSettings["DBDriver"];
            //}

            if (drivername != null)
            {
                this._databasedriver = DbDriverHelper.GetDriverTypeByName(drivername);
            }
            else
            {
                this._databasedriver = EnumDbDriver.MSSql;
            }
            #endregion
            this.Driver = DbDriverHelper.GetDriver(this._databasedriver);


            #region Dialet
            string dbtype;

            dbtype = ConfigurationManager.AppSettings["DataBaseType"];

            if (dbtype != null)
            {
                this._databasedialet = DbDialetHelper.GetDialetTypeByName(dbtype);
            }
            else
            {
                this._databasedialet = EnumDataBaseDialet.SQL2005;
            }
            #endregion

            this.Dialet = DbDialetHelper.GetDialet(this._databasedialet);
        }
    }
}
