using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using Lib.DataInterface.Implement;

namespace dcl.client.wcf
{
    public class ProxyDataInterface : ProxyBase<IDataInterfaceServiceContract>, IDataInterfaceServiceContract
    {
        public override string ConfigName
        {
            get { return "svc.DataInterface"; }
        }

        #region IDataInterfaceServiceContract 成员

        public void CommandDelete(EntityDictDataInterfaceCommand obj)
        {
            this.Service.CommandDelete(obj);
        }

        public void CommandSave(EntityDictDataInterfaceCommand cmd, EntityDictDataInterfaceCommandParameter[] listParams)
        {
            this.Service.CommandSave(cmd, listParams);
        }

        public void ConnectionDelete(EntityDictDataInterfaceConnection obj)
        {
            this.Service.ConnectionDelete(obj);
        }

        public void ConnectionSave(EntityDictDataInterfaceConnection obj)
        {
            this.Service.ConnectionSave(obj);
        }

        public void ConverterDelete(EntityDictDataConverter converter)
        {
            this.Service.ConverterDelete(converter);
        }

        public void ConverterSave(EntityDictDataConverter converter, EntityDictDataConvertContrast[] contrasts)
        {
            this.Service.ConverterSave(converter, contrasts);
        }

        public EntityDictDataInterfaceCommand[] GetCommandByGroup(string groupName)
        {
            return this.Service.GetCommandByGroup(groupName);
        }

        public EntityDictDataInterfaceCommand GetCommandByID(string cmd_id)
        {
            return this.Service.GetCommandByID(cmd_id);
        }

        public EntityDictDataInterfaceCommand[] GetCommands(string module)
        {
            return this.Service.GetCommands(module);
        }

        public EntityDictDataInterfaceConnection GetConnectionByID(string conn_id)
        {
            return this.Service.GetConnectionByID(conn_id);
        }

        public EntityDictDataInterfaceConnection[] GetConnections(string module)
        {
            return this.Service.GetConnections(module);
        }

        public EntityDictDataConverter GetConverterByID(string rule_id)
        {
            return this.Service.GetConverterByID(rule_id);
        }

        public EntityDictDataConvertContrast[] GetConverterContrasts()
        {
            return this.Service.GetConverterContrasts();
        }

        public EntityDictDataConvertContrast[] GetConverterContrastsByConverterID(string rule_id)
        {
            return this.Service.GetConverterContrastsByConverterID(rule_id);
        }

        public EntityDictDataConverter[] GetConverters(string module)
        {
            return this.Service.GetConverters(module);
        }

        public EntityDictDataInterfaceCommandParameter[] GetParameters()
        {
            return this.Service.GetParameters();
        }

        public EntityDictDataInterfaceCommandParameter[] GetParametersByCmdID(string cmd_id)
        {
            return this.Service.GetParametersByCmdID(cmd_id);
        }

        public string TestConnection(EntityDictDataInterfaceConnection obj)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string cmd_id, InterfaceDataBindingItem[] dataBindings)
        {
            object ret = this.Service.ExecuteScalar(cmd_id, dataBindings);
            return ret;
        }

        #endregion
    }
}
