using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    public abstract class AbstractDICache
    {
        protected abstract EnumDataAccessMode DataAccessMode { get; }

        DACManager dac;
        protected AbstractDICache()
        {
            _cacheConnection = new List<EntityDictDataInterfaceConnection>();
            _cacheCommand = new List<EntityDictDataInterfaceCommand>();
            _cacheParameter = new List<EntityDictDataInterfaceCommandParameter>();
            _cacheConverter = new List<EntityDictDataConverter>();
            _cacheConvertContrast = new List<EntityDictDataConvertContrast>();
            dac = new DACManager(DataAccessMode, false);
        }

        #region prop&field

        bool connectionInited = false;
        bool commandInited = false;
        bool parameterInited = false;
        bool converterInited = false;

        #endregion

        protected List<EntityDictDataInterfaceConnection> _cacheConnection { get; set; }
        protected List<EntityDictDataInterfaceCommand> _cacheCommand { get; set; }
        protected List<EntityDictDataInterfaceCommandParameter> _cacheParameter { get; set; }
        protected List<EntityDictDataConverter> _cacheConverter { get; set; }
        protected List<EntityDictDataConvertContrast> _cacheConvertContrast { get; set; }

        #region Init

        private void InitConnection()
        {
            if (!connectionInited)
            {
                RefreshConnection();
                connectionInited = true;
            }
        }

        private void InitCommand()
        {
            if (!commandInited)
            {
                RefreshCommand();
                commandInited = true;
            }
        }

        private void InitParameter()
        {
            if (!parameterInited)
            {
                RefreshParameter();
                parameterInited = true;
            }
        }

        private void InitConverter()
        {
            if (!converterInited)
            {
                RefreshConverter();
                converterInited = true;
            }
        }

        #endregion

        #region Refresh
        /// <summary>
        /// 刷新连接缓存
        /// </summary>
        public void RefreshConnection()
        {
            lock (this._cacheConnection)
            {
                this._cacheConnection = dac.GetConnections(null);
            }
        }

        /// <summary>
        /// 刷新命令缓存
        /// </summary>
        public void RefreshCommand()
        {
            lock (this._cacheCommand)
            {
                this._cacheCommand = dac.GetCommands(null);
            }
        }

        /// <summary>
        /// 刷新参数缓存
        /// </summary>
        public void RefreshParameter()
        {
            lock (this._cacheParameter)
            {
                this._cacheParameter = dac.GetParameters();
            }
        }

        /// <summary>
        /// 刷新数据转换缓存
        /// </summary>
        public void RefreshConverter()
        {
            lock (this._cacheConverter)
            {
                this._cacheConverter = dac.GetConverters(null);
                this._cacheConvertContrast = dac.GetConverterContrasts();
            }
        }

        /// <summary>
        /// 刷新所有缓存
        /// </summary>
        public void RefreshAll()
        {
            RefreshConnection();
            RefreshCommand();
            RefreshParameter();
            RefreshConverter();
        }
        #endregion

        #region connection相关
        public List<EntityDictDataInterfaceConnection> GetConnections(string moduleName)
        {
            InitConnection();

            if (string.IsNullOrEmpty(moduleName))
                return this._cacheConnection;

            List<EntityDictDataInterfaceConnection> list = new List<EntityDictDataInterfaceConnection>();
            foreach (EntityDictDataInterfaceConnection item in this._cacheConnection)
            {
                if (item.sys_module == moduleName)
                    list.Add(item);
            }

            return list;
        }

        public EntityDictDataInterfaceConnection GetConnectionByID(string conn_id)
        {
            InitConnection();
            foreach (EntityDictDataInterfaceConnection item in this._cacheConnection)
            {
                if (item.conn_id == conn_id)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 根据连接串代码(不区分大小写)获取连接串配置
        /// </summary>
        /// <param name="conn_code"></param>
        /// <returns></returns>
        public EntityDictDataInterfaceConnection GetConnectionByCode(string conn_code)
        {
            InitConnection();
            foreach (EntityDictDataInterfaceConnection item in this._cacheConnection)
            {
                if (string.IsNullOrEmpty(item.conn_code)
                    && string.IsNullOrEmpty(conn_code))
                {
                    return item;
                }
                else if (
                    item.conn_code != null && conn_code != null &&
                    item.conn_code.ToLower() == conn_code.ToLower())
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region command相关

        public EntityDictDataInterfaceCommand GetCommandByID(string cmd_id)
        {
            InitCommand();

            foreach (EntityDictDataInterfaceCommand item in this._cacheCommand)
            {
                if (item.cmd_id == cmd_id)
                    return item;
            }
            return null;
        }

        public List<EntityDictDataInterfaceCommand> GetCommandsByGroup(string group_name)
        {
            InitCommand();

            List<EntityDictDataInterfaceCommand> list = new List<EntityDictDataInterfaceCommand>();
            foreach (EntityDictDataInterfaceCommand item in this._cacheCommand)
            {
                if (item.cmd_group == group_name)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<EntityDictDataInterfaceCommand> GetCommands(string moduleName)
        {
            InitCommand();

            if (string.IsNullOrEmpty(moduleName))
                return this._cacheCommand;

            List<EntityDictDataInterfaceCommand> list = new List<EntityDictDataInterfaceCommand>();
            foreach (EntityDictDataInterfaceCommand item in this._cacheCommand)
            {
                if (item.sys_module == moduleName)
                    list.Add(item);
            }

            return list;
        }
        #endregion

        #region parameter相关
        public List<EntityDictDataInterfaceCommandParameter> GetParameterByCmdID(string cmd_id)
        {
            InitParameter();
            List<EntityDictDataInterfaceCommandParameter> list = new List<EntityDictDataInterfaceCommandParameter>();
            foreach (EntityDictDataInterfaceCommandParameter item in this._cacheParameter)
            {
                if (item.cmd_id == cmd_id)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<EntityDictDataInterfaceCommandParameter> GetParameters()
        {
            InitParameter();
            return this._cacheParameter;
        }

        #endregion

        #region 转换规则相关

        public EntityDictDataConverter GetConverterByID(string rule_id)
        {
            this.InitConverter();
            foreach (EntityDictDataConverter item in this._cacheConverter)
            {
                if (item.rule_id == rule_id)
                    return item;
            }
            return null;
        }

        public List<EntityDictDataConvertContrast> GetConvertContrastByConverterID(string rule_id)
        {
            this.InitConverter();
            List<EntityDictDataConvertContrast> list = new List<EntityDictDataConvertContrast>();
            foreach (EntityDictDataConvertContrast item in this._cacheConvertContrast)
            {
                if (item.rule_id == rule_id)
                    list.Add(item);
            }

            return list;
        }

        public List<EntityDictDataConvertContrast> GetConverterContrasts()
        {
            this.InitConverter();
            return this._cacheConvertContrast;
        }

        public List<EntityDictDataConverter> GetConverters(string module_name)
        {
            this.InitConverter();

            if (string.IsNullOrEmpty(module_name))
            {
                return this._cacheConverter;
            }
            else
            {
                List<EntityDictDataConverter> tmp = new List<EntityDictDataConverter>();
                foreach (EntityDictDataConverter item in this._cacheConverter)
                {
                    if (item.sys_module == module_name)
                    {
                        tmp.Add(item);
                    }
                }
                return tmp;
            }
        }
        #endregion
    }
}
