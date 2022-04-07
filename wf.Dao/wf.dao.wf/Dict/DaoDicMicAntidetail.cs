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
    [Export("wf.plugin.wf", typeof(IDaoDicMicAntidetail))]
    public class DaoDicMicAntidetail : IDaoDicMicAntidetail
    {
        public List<EntityDicMicAntidetail> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicMicAntidetail> list = EntityManager<EntityDicMicAntidetail>.ConvertToList(dtSour);

            return list.OrderBy(i => i.AnsSortNo).ToList();
        }

        public bool Delete(EntityDicMicAntidetail anSstd)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ranti_id", anSstd.AnsDefSn);

                helper.UpdateOperation("Rel_mic_antidetail", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicAntidetail anSstd)
        {
            try
            {
                //int id = IdentityHelper.GetMedIdentity("Def_mic_antidetail");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("def_sn", id);
                values.Add("Ranti_Dant_id", anSstd.AnsAntiCode);
                values.Add("Ranti_std_upper_limit", anSstd.AnsStdUpperLimit);
                values.Add("Ranti_std_middle_limit", anSstd.AnsStdMiddleLimit);
                values.Add("Ranti_std_lower_limit", anSstd.AnsStdLowerLimit);
                values.Add("Ranti_zone_lower_limit", anSstd.AnsZoneLowerLimit);
                values.Add("Ranti_zone_upper_limit", anSstd.AnsZoneUpperLimit);
                values.Add("Ranti_short_name", anSstd.AnsAntiShortName);
                values.Add("Ranti_flag", anSstd.AnsDefFlag);
                values.Add("Ranti_zone_durgfast", anSstd.AnsZoneDurgfast);
                values.Add("Ranti_zone_intermed", anSstd.AnsZoneIntermed);
                values.Add("Ranti_zone_sensitive", anSstd.AnsZoneSensitive);
                values.Add("del_flag", anSstd.AnsDelFlag);
                values.Add("sort_no", anSstd.AnsSortNo);
                values.Add("Ranti_Dantitype_id", anSstd.AnsDefId);

                values.Add("Ranti_concentration", anSstd.Concentration);
                values.Add("Ranti_group", anSstd.Group);
                values.Add("Ranti_notes", anSstd.Notes);
                values.Add("Ranti_std_sdd", anSstd.StdSdd);
                values.Add("Ranti_std_ns", anSstd.StdNs);
                values.Add("Ranti_zone_ns", anSstd.ZoneNs);
                values.Add("Ranti_zone_sdd", anSstd.ZoneSdd);
                values.Add("Ranti_report_flag", anSstd.ReportFlag);
                values.Add("Ranti_sam_type", anSstd.ZoneSamCustomType);

                helper.InsertOperation("Rel_mic_antidetail", values);

                //anSstd.AnsDefId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicAntidetail> Search(object obj)
        {
            try
            {
                String sql = @"select a.*,b.Dant_cname,b.Dant_code ,b.sort_no as anti_sort_no,Dic_mic_antitype.Dantitype_name 
from Rel_mic_antidetail a,Dict_mic_antibio b ,Dic_mic_antitype where a.Ranti_Dant_id=b.Dant_id and A.del_flag=0  and b.del_flag=0 and a.Ranti_Dantitype_id=Dic_mic_antitype.Dantitype_id";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicAntidetail>();
            }
        }

        public bool Update(EntityDicMicAntidetail anSstd)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ranti_Dant_id", anSstd.AnsAntiCode);
                values.Add("Ranti_std_upper_limit", anSstd.AnsStdUpperLimit);
                values.Add("Ranti_std_middle_limit", anSstd.AnsStdMiddleLimit);
                values.Add("Ranti_std_lower_limit", anSstd.AnsStdLowerLimit);
                values.Add("Ranti_zone_lower_limit", anSstd.AnsZoneLowerLimit);
                values.Add("Ranti_zone_upper_limit", anSstd.AnsZoneUpperLimit);
                values.Add("Ranti_short_name", anSstd.AnsAntiShortName);
                values.Add("Ranti_flag", anSstd.AnsDefFlag);
                values.Add("Ranti_zone_durgfast", anSstd.AnsZoneDurgfast);
                values.Add("Ranti_zone_intermed", anSstd.AnsZoneIntermed);
                values.Add("Ranti_zone_sensitive", anSstd.AnsZoneSensitive);
                values.Add("del_flag", anSstd.AnsDelFlag);
                values.Add("sort_no", anSstd.AnsSortNo);
                values.Add("Ranti_Dantitype_id", anSstd.AnsDefId);


                values.Add("Ranti_concentration", anSstd.Concentration);
                values.Add("Ranti_group", anSstd.Group);
                values.Add("Ranti_notes", anSstd.Notes);
                values.Add("Ranti_std_sdd", anSstd.StdSdd);
                values.Add("Ranti_std_ns", anSstd.StdNs);
                values.Add("Ranti_zone_ns", anSstd.ZoneNs);
                values.Add("Ranti_zone_sdd", anSstd.ZoneSdd);
                values.Add("Ranti_report_flag", anSstd.ReportFlag);
                values.Add("Ranti_sam_type", anSstd.ZoneSamCustomType);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ranti_id", anSstd.AnsDefSn.Value);

                helper.UpdateOperation("Rel_mic_antidetail", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateDelFlagByAntiCode(string antiCode)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ranti_Dant_id", antiCode);

                helper.UpdateOperation("Rel_mic_antidetail", values, key);
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
