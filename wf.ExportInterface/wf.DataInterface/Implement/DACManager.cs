using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    class DACManager : ICloneable
    {
        /// <summary>
        /// 数据获取方式
        /// </summary>
        public EnumDataAccessMode DataAccessMode { get; set; }

        /// <summary>
        /// 是否使用cache
        /// </summary>
        public bool UseCache { get; set; }

        public DACManager(EnumDataAccessMode accessMode, bool useCache)
        {
            this.DataAccessMode = accessMode;
            this.UseCache = useCache;
        }


        #region IDataInterfaceServiceContract 成员

        public List<EntityDictDataInterfaceConnection> GetConnections(string module_name)
        {
            List<EntityDictDataInterfaceConnection> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConnections(module_name);
                }
                else
                {
                    ret = new CRUDDataInterfaceConnection().SelectAll(module_name);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConnections(module_name);
                }
                else
                {
                    ret = new List<EntityDictDataInterfaceConnection>(DIEnviorment.Proxy.GetConnections(module_name));
                }
            }
            return ret;
        }

        public EntityDictDataInterfaceConnection GetConnectionByID(string conn_id)
        {
            EntityDictDataInterfaceConnection ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConnectionByID(conn_id);
                }
                else
                {
                    ret = new CRUDDataInterfaceConnection().SelectByID(conn_id);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConnectionByID(conn_id);
                }
                else
                {
                    ret = DIEnviorment.Proxy.GetConnectionByID(conn_id);
                }
            }
            return ret;
        }

        public string TestConnection(EntityDictDataInterfaceConnection obj)
        {
            string msg;
            if (obj.conn_running_side.ToString() == EnumDeploymentMode.Server.ToString())
            {
                DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(obj);
                conn.TestConnection(out msg);
            }
            else if (obj.conn_running_side.ToString() == EnumDeploymentMode.Client.ToString())
            {
                msg = DIEnviorment.Proxy.TestConnection(obj);
            }
            else
            {
                throw new NotSupportedException("未指定执行端");
            }
            return msg;
        }

        public void ConnectionDelete(EntityDictDataInterfaceConnection obj)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                //直连数据库
                CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
                biz.Delete(obj);
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                //自定义处理方式
                DIEnviorment.Proxy.ConnectionDelete(obj);
                CacheDataDemandDataInterface.Current.RefreshConnection();
            }
        }

        public void ConnectionSave(EntityDictDataInterfaceConnection obj)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
                biz.Save(obj);
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                DIEnviorment.Proxy.ConnectionSave(obj);
                CacheDataDemandDataInterface.Current.RefreshConnection();
            }
        }

        public List<EntityDictDataInterfaceCommand> GetCommands(string module_name)
        {
            List<EntityDictDataInterfaceCommand> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetCommands(module_name);
                }
                else
                {
                    ret = new CRUDDataInterfaceCommand().SelectAll(module_name);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetCommands(module_name);
                }
                else
                {
                    ret = new List<EntityDictDataInterfaceCommand>(DIEnviorment.Proxy.GetCommands(module_name));
                }
            }

            return ret;
        }

        public EntityDictDataInterfaceCommand GetCommandByID(string cmd_id)
        {
            EntityDictDataInterfaceCommand ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetCommandByID(cmd_id);
                }
                else
                {
                    ret = new CRUDDataInterfaceCommand().SelectByID(cmd_id);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetCommandByID(cmd_id);
                }
                else
                {
                    ret = DIEnviorment.Proxy.GetCommandByID(cmd_id);
                }
            }
            return ret;
        }

        public List<EntityDictDataInterfaceCommand> GetCommandByGroup(string cmd_group)
        {
            List<EntityDictDataInterfaceCommand> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetCommandsByGroup(cmd_group);
                }
                else
                {
                    ret = new CRUDDataInterfaceCommand().SelectByGroupName(cmd_group);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetCommandsByGroup(cmd_group);
                }
                else
                {
                    ret = new List<EntityDictDataInterfaceCommand>(DIEnviorment.Proxy.GetCommandByGroup(cmd_group));
                }
            }

            return ret;
        }

        public List<EntityDictDataInterfaceCommandParameter> GetParametersByCmdID(string cmd_id)
        {
            List<EntityDictDataInterfaceCommandParameter> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetParameterByCmdID(cmd_id);
                }
                else
                {
                    ret = new CRUDDataInterfaceCommandParameter().SelectAll(cmd_id);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetParameterByCmdID(cmd_id);
                }
                else
                {

                    ret = new List<EntityDictDataInterfaceCommandParameter>(DIEnviorment.Proxy.GetParametersByCmdID(cmd_id));
                }
            }

            return ret;
        }

        public List<EntityDictDataInterfaceCommandParameter> GetParameters()
        {
            List<EntityDictDataInterfaceCommandParameter> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetParameters();
                }
                else
                {
                    ret = new CRUDDataInterfaceCommandParameter().SelectAll();
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetParameters();
                }
                else
                {

                    ret = new List<EntityDictDataInterfaceCommandParameter>(DIEnviorment.Proxy.GetParameters());
                }
            }

            return ret;
        }

        public void CommandDelete(EntityDictDataInterfaceCommand obj)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
                biz.Delete(obj);
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                DIEnviorment.Proxy.CommandDelete(obj);
                CacheDataDemandDataInterface.Current.RefreshCommand();
                CacheDataDemandDataInterface.Current.RefreshParameter();
            }
        }

        public void CommandSave(EntityDictDataInterfaceCommand cmd, EntityDictDataInterfaceCommandParameter[] listParams)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
                biz.Save(cmd, new List<EntityDictDataInterfaceCommandParameter>(listParams));
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                DIEnviorment.Proxy.CommandSave(cmd, listParams);
                CacheDataDemandDataInterface.Current.RefreshCommand();
                CacheDataDemandDataInterface.Current.RefreshParameter();
            }
        }

        public List<EntityDictDataConverter> GetConverters(string module)
        {
            List<EntityDictDataConverter> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConverters(module);
                }
                else
                {
                    ret = new CRUDDataInterfaceConverter().SelectAll(module);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConverters(module);
                }
                else
                {
                    ret = new List<EntityDictDataConverter>(DIEnviorment.Proxy.GetConverters(module));
                }
            }
            return ret;
        }

        public EntityDictDataConverter GetConverterByID(string rule_id)
        {
            EntityDictDataConverter ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConverterByID(rule_id);
                }
                else
                {
                    ret = new CRUDDataInterfaceConverter().SelectByID(rule_id);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConverterByID(rule_id);
                }
                else
                {
                    DIEnviorment.Proxy.GetConverterByID(rule_id);
                }
            }
            return ret;
        }

        public List<EntityDictDataConvertContrast> GetConverterContrastsByConverterID(string rule_id)
        {
            List<EntityDictDataConvertContrast> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConvertContrastByConverterID(rule_id);
                }
                else
                {
                    ret = new CRUDDataInterfaceConvertContrast().SelectAll(rule_id);
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConvertContrastByConverterID(rule_id);
                }
                else
                {
                    ret = new List<EntityDictDataConvertContrast>(DIEnviorment.Proxy.GetConverterContrastsByConverterID(rule_id));
                }
            }
            return ret;
        }

        public List<EntityDictDataConvertContrast> GetConverterContrasts()
        {
            List<EntityDictDataConvertContrast> ret = null;
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                if (UseCache)
                {
                    ret = CacheDirectDBDataInterface.Current.GetConverterContrasts();
                }
                else
                {
                    ret = new CRUDDataInterfaceConvertContrast().SelectAll();
                }
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                if (UseCache)
                {
                    ret = CacheDataDemandDataInterface.Current.GetConverterContrasts();
                }
                else
                {
                    ret = new List<EntityDictDataConvertContrast>(DIEnviorment.Proxy.GetConverterContrasts());
                }
            }
            return ret;
        }

        public void ConverterDelete(EntityDictDataConverter obj)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
                biz.Delete(obj);
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                DIEnviorment.Proxy.ConverterDelete(obj);
                CacheDataDemandDataInterface.Current.RefreshConverter();
            }
        }

        public void ConverterSave(EntityDictDataConverter converter, EntityDictDataConvertContrast[] contrasts)
        {
            if (this.DataAccessMode == EnumDataAccessMode.DirectToDB)
            {
                CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
                biz.Save(converter, new List<EntityDictDataConvertContrast>(contrasts));
            }
            else if (this.DataAccessMode == EnumDataAccessMode.Custom)
            {
                DIEnviorment.Proxy.ConverterSave(converter, contrasts);
                CacheDataDemandDataInterface.Current.RefreshConverter();
            }
        }

        #endregion

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}