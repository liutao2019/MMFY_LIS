using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicDataInterfaceCommandParameter))]
    public class DaoDicDataInterfaceCommandParameter : IDaoDicDataInterfaceCommandParameter
    {
        public bool DeleteDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isDelete = false;
            if (cmdParam != null)
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"delete from dict_DataInterfaceCommandParameter where cmd_id= '{0}' ", cmdParam.CmdId);
                    if (!string.IsNullOrEmpty(cmdParam.ParamName))
                        sql += string.Format(@" and param_name='{0}' ", cmdParam.ParamName);

                    helper.ExecCommand(sql);
                    isDelete = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isDelete;
        }

        public bool SaveDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isSave = false;
            try
            {
                //int id = IdentityHelper.GetMedIdentity("dict_DataInterfaceCommandParameter"); //获取主键值

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("cmd_id", cmdParam.CmdId);
                values.Add("param_name", cmdParam.ParamName);
                values.Add("param_direction", cmdParam.ParamDirection);
                values.Add("param_datatype", cmdParam.ParamDatatype);
                values.Add("param_length", cmdParam.ParamLength);

                values.Add("param_seq", cmdParam.ParamSeq);
                values.Add("param_isbound", cmdParam.ParamIsbound);
                values.Add("param_databind", cmdParam.ParamDatabind);
                values.Add("param_enabledlog", cmdParam.ParamEnabledlog);
                values.Add("param_converter_id", cmdParam.ParamConverterId);

                values.Add("param_desc", cmdParam.ParamDesc);

                helper.InsertOperation("dict_DataInterfaceCommandParameter ", values);

                //cmdParam.CmdId = id.ToString();
                isSave = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceCommandParameter> SearchDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            List<EntityDicDataInterfaceCommandParameter> listInterCommand = new List<EntityDicDataInterfaceCommandParameter>();
            if (cmdParam != null)
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sqlStr = "select * from dict_DataInterfaceCommandParameter where 1=1";
                    if (!string.IsNullOrEmpty(cmdParam.CmdId))
                        sqlStr += string.Format(" and cmd_id ='{0}' ", cmdParam.CmdId);

                    DataTable dt = helper.ExecuteDtSql(sqlStr);
                    listInterCommand = EntityManager<EntityDicDataInterfaceCommandParameter>.ConvertToList(dt).OrderBy(i => i.CmdId.Length).ThenBy(i => i.CmdId).ThenBy(i => i.ParamSeq).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listInterCommand;
        }

        public bool UpdateDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("cmd_id", id);
                values.Add("param_name", cmdParam.ParamName);
                values.Add("param_direction", cmdParam.ParamDirection);
                values.Add("param_datatype", cmdParam.ParamDatatype);
                values.Add("param_length", cmdParam.ParamLength);

                values.Add("param_seq", cmdParam.ParamSeq);
                values.Add("param_isbound", cmdParam.ParamIsbound);
                values.Add("param_databind", cmdParam.ParamDatabind);
                values.Add("param_enabledlog", cmdParam.ParamEnabledlog);
                values.Add("param_converter_id", cmdParam.ParamConverterId);
                values.Add("param_desc", cmdParam.ParamDesc);

                Dictionary<string, object> keys = new Dictionary<string, object>(); 
                keys.Add("cmd_id", cmdParam);

                helper.UpdateOperation("dict_DataInterfaceCommandParameter ", values, keys);

                isUpdate = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }
    }
}
