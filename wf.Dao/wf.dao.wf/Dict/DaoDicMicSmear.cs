using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicMicSmear))]
    public class DaoDicMicSmear : IDaoDicMicSmear
    {
       

        public bool Delete(EntityDicMicSmear smear)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dsme_id", smear.SmeId);

                helper.UpdateOperation("Dict_mic_smear", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicSmear smear)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_mic_smear");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsme_id", id);
                values.Add("Dsme_name", smear.SmeName);
                values.Add("Dsme_Dpro_id", smear.SmeType);
                values.Add("Dsme_c_code", smear.SmeCCode);
                values.Add("Dsme_Dpurp_id", smear.SmePurpId);
                values.Add("Dsme_com_flag", smear.SmeComFlag);
                values.Add("Dsme_positive_flag", smear.SmePositiveFlag);
                values.Add("sort_no", smear.SmeSortNo);
                values.Add("py_code", smear.SmePyCode);
                values.Add("wb_code", smear.SmeWbCode);
                values.Add("del_flag", smear.SmeDelFlag);
                values.Add("Dsme_prop", smear.SmeProp);
                values.Add("Dsme_aen", smear.SmeAen);
                values.Add("Dsme_class", smear.SmeClass);
                values.Add("Dsme_public", smear.SmePublic);
                

                helper.InsertOperation("Dict_mic_smear", values);

                smear.SmeId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicSmear> Search(object obj)
        {
            try
            {
                String sql = @"SELECT Dict_mic_smear.*,Dict_profession.Dpro_name, 
Dict_check_purpose.Dpurp_name FROM Dict_mic_smear 
Left OUTER JOIN Dict_check_purpose ON Dict_mic_smear.Dsme_Dpurp_id = Dict_check_purpose.Dpurp_id 
LEFT OUTER JOIN Dict_profession ON Dict_mic_smear.Dsme_Dpro_id = Dict_profession.Dpro_id
where Dict_mic_smear.del_flag =0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicMicSmear>.ConvertToList(dt).OrderBy(i => i.SmeSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicSmear>();
            }
        }

        public List<EntityDicMicSmear> SearchMicSmear()
        {
            try
            {
                String sql = @"select Dsme_id,Dsme_name from Dict_mic_smear where del_flag<>'1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicMicSmear>.ConvertToList(dt).OrderBy(i => i.SmeSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicSmear>();
            }
        }

        public bool Update(EntityDicMicSmear smear)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsme_name", smear.SmeName);
                values.Add("Dsme_Dpro_id", smear.SmeType);
                values.Add("Dsme_c_code", smear.SmeCCode);
                values.Add("Dsme_Dpurp_id", smear.SmePurpId);
                values.Add("Dsme_com_flag", smear.SmeComFlag);
                values.Add("Dsme_positive_flag", smear.SmePositiveFlag);
                values.Add("sort_no", smear.SmeSortNo);
                values.Add("py_code", smear.SmePyCode);
                values.Add("wb_code", smear.SmeWbCode);
                values.Add("del_flag", smear.SmeDelFlag);
                values.Add("Dsme_prop", smear.SmeProp);
                values.Add("Dsme_aen", smear.SmeAen);
                values.Add("Dsme_class", smear.SmeClass);
                values.Add("Dsme_public", smear.SmePublic);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dsme_id", smear.SmeId);

                helper.UpdateOperation("Dict_mic_smear", values, key);
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
