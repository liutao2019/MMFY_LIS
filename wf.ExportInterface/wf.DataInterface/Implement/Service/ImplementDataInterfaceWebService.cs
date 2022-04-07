using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.Implement
{
    public class ImplementDataInterfaceWebService : IDataInterfaceServiceContract
    {
        #region connection
        public EntityDictDataInterfaceConnection[] GetConnections(string module)
        {
            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            List<EntityDictDataInterfaceConnection> list = biz.SelectAll(module);
            return list.ToArray();
        }

        public void ConnectionDelete(EntityDictDataInterfaceConnection obj)
        {
            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            biz.Delete(obj);
        }

        public void ConnectionSave(EntityDictDataInterfaceConnection obj)
        {
            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            biz.Save(obj);
        }

        public EntityDictDataInterfaceConnection GetConnectionByID(string conn_id)
        {
            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            return biz.SelectByID(conn_id);
        }
        #endregion

        public string TestConnection(EntityDictDataInterfaceConnection obj)
        {
            string errMsg;
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(obj);
            conn.TestConnection(out errMsg);
            return errMsg;
        }

        #region Command
        public EntityDictDataInterfaceCommand[] GetCommands(string module)
        {
            CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
            List<EntityDictDataInterfaceCommand> list = biz.SelectAll(module);
            return list.ToArray();
        }

        public EntityDictDataInterfaceCommand[] GetCommandByGroup(string groupName)
        {
            CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
            List<EntityDictDataInterfaceCommand> ret = biz.SelectByGroupName(groupName);
            return ret.ToArray();
        }

        public EntityDictDataInterfaceCommand GetCommandByID(string cmd_id)
        {
            CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
            EntityDictDataInterfaceCommand cmd = biz.SelectByID(cmd_id);
            return cmd;
        }

        public void CommandDelete(EntityDictDataInterfaceCommand obj)
        {
            CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
            biz.Delete(obj);
        }

        public EntityDictDataInterfaceCommandParameter[] GetParametersByCmdID(string cmd_id)
        {
            CRUDDataInterfaceCommandParameter biz = new CRUDDataInterfaceCommandParameter();
            List<EntityDictDataInterfaceCommandParameter> list;
            if (string.IsNullOrEmpty(cmd_id))
            {
                list = new List<EntityDictDataInterfaceCommandParameter>();
            }
            else
            {
                list = biz.SelectAll(cmd_id);
            }
            return list.ToArray();
        }

        public void CommandSave(EntityDictDataInterfaceCommand cmd, EntityDictDataInterfaceCommandParameter[] listParams)
        {
            CRUDDataInterfaceCommand biz = new CRUDDataInterfaceCommand();
            biz.Save(cmd, new List<EntityDictDataInterfaceCommandParameter>(listParams));
        }

        public EntityDictDataInterfaceCommandParameter[] GetParameters()
        {
            CRUDDataInterfaceCommandParameter biz = new CRUDDataInterfaceCommandParameter();
            return biz.SelectAll().ToArray();
        }
        #endregion

        #region Converter
        public EntityDictDataConverter[] GetConverters(string module)
        {
            CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
            List<EntityDictDataConverter> list = biz.SelectAll(module);
            return list.ToArray();
        }

        public EntityDictDataConverter GetConverterByID(string rule_id)
        {
            CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
            EntityDictDataConverter ret = biz.SelectByID(rule_id);
            return ret;
        }

        public EntityDictDataConvertContrast[] GetConverterContrastsByConverterID(string rule_id)
        {
            CRUDDataInterfaceConvertContrast biz = new CRUDDataInterfaceConvertContrast();
            List<EntityDictDataConvertContrast> list = biz.SelectAll(rule_id);
            return list.ToArray();
        }

        public EntityDictDataConvertContrast[] GetConverterContrasts()
        {
            CRUDDataInterfaceConvertContrast biz = new CRUDDataInterfaceConvertContrast();
            List<EntityDictDataConvertContrast> list = biz.SelectAll();
            return list.ToArray();
        }

        public void ConverterDelete(EntityDictDataConverter converter)
        {
            CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
            biz.Delete(converter);
        }

        public void ConverterSave(EntityDictDataConverter converter, EntityDictDataConvertContrast[] contrasts)
        {
            CRUDDataInterfaceConverter biz = new CRUDDataInterfaceConverter();
            biz.Save(converter, new List<EntityDictDataConvertContrast>(contrasts));
        }
        #endregion

        #region DataInterfaceHelper

        public object ExecuteScalar(string cmd_id, InterfaceDataBindingItem[] dataBindings)
        {
            DataInterfaceHelper helper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
            object ret = helper.ExecuteScalar(cmd_id, dataBindings);
            return ret;
        }

        #endregion
    }
}
