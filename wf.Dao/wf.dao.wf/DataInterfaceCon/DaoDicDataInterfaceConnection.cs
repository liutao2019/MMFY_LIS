using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;
using dcl.common;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicDataInterfaceConnection))]
    public class DaoDicDataInterfaceConnection : IDaoDicDataInterfaceConnection
    {
        public bool DeleteDataInterfaceConnection(string connId)
        {
            bool isDelete = false;
            if (!string.IsNullOrEmpty(connId))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"delete from Dict_datainterfaceconnection where conn_id='{0}' ", connId);
                    isDelete = helper.ExecCommand(sql) > 0;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isDelete;
        }

        public bool SaveDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            bool isSave = false;
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_datainterfaceconnection"); //获取主键值

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("conn_id", id);
                values.Add("conn_name", dtInterCon.ConnName);
                values.Add("conn_type", dtInterCon.ConnType);
                values.Add("conn_address", dtInterCon.ConnAddress);
                values.Add("conn_db_driver", dtInterCon.ConnDbDriver);
                values.Add("conn_db_dialet", dtInterCon.ConnDbDialet);
                values.Add("conn_db_catelog", dtInterCon.ConnDbCatelog);
                values.Add("conn_desc", dtInterCon.ConnDesc);
                values.Add("conn_login", dtInterCon.ConnLogin);
                values.Add("conn_pass", dtInterCon.ConnPass);
                values.Add("sys_module", dtInterCon.SysModule);
                values.Add("sys_default", dtInterCon.SysDefault);
                values.Add("conn_running_side", dtInterCon.ConnRunningSide);
                values.Add("conn_code", dtInterCon.ConnCode);

                helper.InsertOperation("Dict_datainterfaceconnection ", values);

                dtInterCon.ConnId = id.ToString();
                isSave = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceConnection> SearchDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            List<EntityDicDataInterfaceConnection> listDataInterConn = new List<EntityDicDataInterfaceConnection>();
            try
            {
                DBManager helper = new DBManager();
                string sqlStr = "select * from Dict_datainterfaceconnection";
                DataTable dt = helper.ExecuteDtSql(sqlStr);
                listDataInterConn = EntityManager<EntityDicDataInterfaceConnection>.ConvertToList(dt).OrderBy(i => i.ConnId.Length).ThenBy(i => i.ConnId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listDataInterConn;
        }

        public bool UpdateDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("conn_id", id);
                values.Add("conn_name", dtInterCon.ConnName);
                values.Add("conn_type", dtInterCon.ConnType);
                values.Add("conn_address", dtInterCon.ConnAddress);
                values.Add("conn_db_driver", dtInterCon.ConnDbDriver);
                values.Add("conn_db_dialet", dtInterCon.ConnDbDialet);
                values.Add("conn_db_catelog", dtInterCon.ConnDbCatelog);
                values.Add("conn_desc", dtInterCon.ConnDesc);
                values.Add("conn_login", dtInterCon.ConnLogin);
                values.Add("conn_pass", dtInterCon.ConnPass);
                values.Add("sys_module", dtInterCon.SysModule);
                values.Add("conn_default", dtInterCon.SysDefault);
                values.Add("conn_running_side", dtInterCon.ConnRunningSide);
                values.Add("conn_code", dtInterCon.ConnCode);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("conn_id", dtInterCon.ConnId);

                isUpdate = helper.UpdateOperation("Dict_datainterfaceconnection ", values, keys) > 0;
            }
            catch (Exception ex) 
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }
    }
}
