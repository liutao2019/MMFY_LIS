using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysItfInterface))]
    public class DaoSysItfInterface : IDaoSysItfInterface
    {
        public bool SaveSysInterface(EntitySysItfInterface inter)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                int id = IdentityHelper.GetMedIdentity("Base_itf_interface");
                values.Add("Bitf_id", id);
                values.Add("Bitf_name", inter.ItfaceName);
                values.Add("Bitf_server", inter.ItfaceServer);
                values.Add("Bitf_database", inter.ItfaceDatabase);
                values.Add("Bitf_logid", inter.ItfaceLogid);
                values.Add("Bitf_password", inter.ItfacePassword);
                values.Add("Bitf_connect_type", inter.ItfaceConnectType);
                values.Add("Bitf_type", inter.ItfaceInterfaceType);
                values.Add("Bitf_execute_sql", inter.ItfaceExecuteSql);
                values.Add("Bitf_return_table", inter.ItfaceReturnTable);
                values.Add("Bitf_fetchtype", inter.ItfaceFetchtype);
                values.Add("Bitf_org_id", inter.ItfaceOrgId);
                helper.InsertOperation("Base_itf_interface", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
       
        }

        public bool UpdateSysInterface(EntitySysItfInterface inter)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bitf_name", inter.ItfaceName);
                values.Add("Bitf_server", inter.ItfaceServer);
                values.Add("Bitf_database", inter.ItfaceDatabase);
                values.Add("Bitf_logid", inter.ItfaceLogid);
                values.Add("Bitf_password", inter.ItfacePassword);
                values.Add("Bitf_connect_type", inter.ItfaceConnectType);
                values.Add("Bitf_type", inter.ItfaceInterfaceType);
                values.Add("Bitf_execute_sql", inter.ItfaceExecuteSql);
                values.Add("Bitf_return_table", inter.ItfaceReturnTable);
                values.Add("Bitf_fetchtype", inter.ItfaceFetchtype);
                values.Add("Bitf_org_id", inter.ItfaceOrgId);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Bitf_id", inter.ItfaceId);
                helper.UpdateOperation("Base_itf_interface", values, keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
       
            return true;
        }

        public bool DeleteSysInterface(string inId)
        {
            try
            {
                if (!string.IsNullOrEmpty(inId))
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format("delete Base_itf_interface where Bitf_id='{0}' ", inId);

                    helper.ExecSql(sql);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysItfInterface> GetSysInterface()
        {
            return GetSysInterface(string.Empty);
        }

        public List<EntitySysItfInterface> GetSysInterface(string par)
        {
            List<EntitySysItfInterface> list = new List<EntitySysItfInterface>();
            try
            {
                String sql = @"SELECT Base_itf_interface.* ,Dict_organize.Dorg_name
FROM Base_itf_interface 
LEFT JOIN Dict_organize ON Dict_organize.Dorg_id=Base_itf_interface.Bitf_org_id";

                if (!string.IsNullOrEmpty(par) && par != "cache")
                {
                    sql += " where Base_itf_interface.Bitf_type='" + par + "'";
                }
                if (!string.IsNullOrEmpty(par) && par == "cache")
                {
                    sql = string.Format(@"SELECT * from Base_itf_interface where 1=1 {0} ", BuildHospitalSqlWhere("Bitf_org_id"));
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                DataSet result = new DataSet();
                result.Tables.Add(dt);
                foreach (DataTable table in result.Tables)
                {
                    foreach (DataRow item in table.Rows)
                    {
                        if (!item["Bitf_server"].ToString().Contains(".") && item["Bitf_database"].ToString().Length >= 12)
                        {
                            item["Bitf_server"] = EncryptClass.Decrypt(item["Bitf_server"].ToString());
                            item["Bitf_database"] = EncryptClass.Decrypt(item["Bitf_database"].ToString());
                            item["Bitf_logid"] = EncryptClass.Decrypt(item["Bitf_logid"].ToString());
                            item["Bitf_password"] = EncryptClass.Decrypt(item["Bitf_password"].ToString());
                        }
                    }
                }
                list = EntityManager<EntitySysItfInterface>.ConvertToList(dt).OrderBy(i => i.ItfaceId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public static string BuildHospitalSqlWhere(string column)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID); 
        }

        #region CardDataConvert

        public EntitySysItfInterface CardDataConvert(string cardData, string interfaceKey)
        {
            EntitySysItfInterface result = new EntitySysItfInterface();
            string cardOutput = string.Empty;
            if (!string.IsNullOrEmpty(interfaceKey))
            {
                DBManager helper = new DBManager();
                DataTable interfaceData = helper.ExecSel(string.Format(@"select * from  dbo.Base_itf_interface where Bitf_name='{0}' ", interfaceKey));
                foreach (DataRow item in interfaceData.Rows)
                {
                    if (!item["Bitf_server"].ToString().Contains(".") && item["Bitf_database"].ToString().Length >= 12)
                    {
                        item["Bitf_server"] = EncryptClass.Decrypt(item["Bitf_server"].ToString());
                        item["Bitf_database"] = EncryptClass.Decrypt(item["Bitf_database"].ToString());
                        item["Bitf_logid"] = EncryptClass.Decrypt(item["Bitf_logid"].ToString());
                        item["Bitf_password"] = EncryptClass.Decrypt(item["Bitf_password"].ToString());
                    }
                }
                if (interfaceData.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return CardCovert(cardData, interfaceData);

                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException("获取接口信息失败", ex);
                        return null;
                    }
                }
            }
            return result;
        }

        private EntitySysItfInterface CardCovert(string cardData, DataTable interfaceData)
        {
            string result = string.Empty;
            DataRow newRow = interfaceData.Rows[0];
            EntitySysItfInterface interfaces = new EntitySysItfInterface
            {
                ItfaceServer = newRow["Bitf_server"].ToString(),
                ItfaceDatabase = newRow["Bitf_database"].ToString(),
                ItfaceLogid = newRow["Bitf_logid"].ToString(),
                ItfacePassword = newRow["Bitf_password"].ToString(),
                ItfaceConnectType = newRow["Bitf_connect_type"].ToString(),
                ItfaceExecuteSql = newRow["Bitf_execute_sql"].ToString()
            };
            return interfaces;
        }
        #endregion
    }
}
