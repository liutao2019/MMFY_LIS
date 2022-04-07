using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Data;
using dcl.dao.core;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubIdent>))]
    public class DaoDicPubIdent : IDaoDic<EntityDicPubIdent>
    {

        public bool Delete(EntityDicPubIdent noType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Didt_id", noType.IdtId);

                helper.UpdateOperation("Dict_ident", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicPubIdent noType)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_ident");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Didt_id", id);
                values.Add("Didt_name", noType.IdtName);
                values.Add("Didt_code", noType.IdtCode);
                values.Add("Didt_c_code", noType.IdtCCode);
                values.Add("Didt_Dsorc_id", noType.IdtSrcId);
                values.Add("py_code", noType.IdtPyCode);
                values.Add("wb_code", noType.IdtWbCode);
                values.Add("sort_no", noType.IdtSortNo);
                values.Add("del_flag", noType.IdtDelFlag);
                values.Add("Didt_interface_id", noType.IdtInterfaceId);
                values.Add("Didt_interface_type", noType.IdtInterfaceType);
                values.Add("Didt_notnull", noType.IdtPatinnoNotnull);

                helper.InsertOperation("Dict_ident", values);

                noType.IdtId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubIdent> Search(object obj)
        {
            try
            {
                String sql = @"select  a.*,b.Dsorc_name 
from  Dict_ident a 
left join Dict_source b on a.Didt_Dsorc_id=b.Dsorc_id  where a.del_flag=0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicPubIdent>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubIdent>();
            }
        }

        public bool Update(EntityDicPubIdent noType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Didt_name", noType.IdtName);
                values.Add("Didt_code", noType.IdtCode);
                values.Add("Didt_c_code", noType.IdtCCode);
                values.Add("Didt_Dsorc_id", noType.IdtSrcId);
                values.Add("py_code", noType.IdtPyCode);
                values.Add("wb_code", noType.IdtWbCode);
                values.Add("sort_no", noType.IdtSortNo);
                values.Add("del_flag", noType.IdtDelFlag);
                values.Add("Didt_interface_id", noType.IdtInterfaceId);
                values.Add("Didt_interface_type", noType.IdtInterfaceType);
                values.Add("Didt_notnull", noType.IdtPatinnoNotnull);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Didt_id", noType.IdtId);

                helper.UpdateOperation("Dict_ident", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
