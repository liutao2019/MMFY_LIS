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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicAntibio>))]
    public class DaoDicMicAntibio : IDaoDic<EntityDicMicAntibio>
    {
        public List<EntityDicMicAntibio> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicMicAntibio> list = new List<EntityDicMicAntibio>();
            foreach(DataRow item in dtSour.Rows)
            {
                EntityDicMicAntibio antibio = new EntityDicMicAntibio();
                antibio.AntId = item["Dant_id"].ToString();
                antibio.AntEname = item["Dant_ename"].ToString();
                antibio.AntCname = item["Dant_cname"].ToString();
                antibio.AntCode = item["Dant_code"].ToString();
                antibio.AntStdUpperLimit = item["Dant_std_upper_limit"].ToString();
                antibio.AntStdMiddleLimit = item["Dant_std_middle_limit"].ToString();
                antibio.AntStdLowerLimit = item["Dant_std_lower_limit"].ToString();

                decimal zoneLower = 0M;
                if (item["Dant_zone_lower_limit"] != null && item["Dant_zone_lower_limit"] != DBNull.Value)
                    decimal.TryParse(item["Dant_zone_lower_limit"].ToString(), out zoneLower);
                antibio.AntZoneLowerLimit = zoneLower;

                decimal zoneUpper = 0M;
                if (item["Dant_zone_upper_limit"] != null && item["Dant_zone_upper_limit"] != DBNull.Value)
                    decimal.TryParse(item["Dant_zone_upper_limit"].ToString(), out zoneLower);
                antibio.AntZoneUpperLimit = zoneUpper;

                int flag = 0;
                if (item["Dant_flag"] != null && item["Dant_flag"] != DBNull.Value)
                    int.TryParse(item["Dant_flag"].ToString(), out flag);
                antibio.AntFlag = flag;

                antibio.AntZoneDurgfast = item["Dant_zone_durgfast"].ToString();
                antibio.AntZoneIntermed = item["Dant_zone_intermed"].ToString();
                antibio.AntZoneSensitive = item["Dant_zone_sensitive"].ToString();

                int printFlag = 0;
                if (item["Dant_pirnt_flag"] != null && item["Dant_pirnt_flag"] != DBNull.Value)
                    int.TryParse(item["Dant_pirnt_flag"].ToString(), out flag);
                antibio.AntPirntFlag = printFlag;

                antibio.AntMethod = item["Dant_method"].ToString();
                antibio.AntSerum = item["Dant_serum"].ToString();
                antibio.AntUrine = item["Dant_urine"].ToString();
                antibio.AntMitNo = item["Dant_mit_no"].ToString();
                antibio.AntWhoNo = item["Dant_who_no"].ToString();
                antibio.AntCCode = item["Dant_c_code"].ToString();
                antibio.AntPyCode = item["py_code"].ToString();
                antibio.AntWbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                antibio.AntSortNo = sort;

                antibio.AntDelFlag = item["del_flag"].ToString();
                antibio.AntKbWhoNo = item["Dant_kb_who_no"].ToString();
                antibio.AntClsiComment = item["Dant_clsi_comment"].ToString();
                //antibio.AntNotes = item["ant_notes"].ToString();
                //antibio.AntWhoEtestNo = item["ant_who_etest_no"].ToString();
                antibio.AntNotes = item["Dant_notes"].ToString();
                antibio.AntTypeId = item["Dant_type_id"].ToString();
                antibio.AntUnitName = item["Dant_unit_name"].ToString();

                list.Add(antibio);

            }
            return list.OrderBy(i => i.AntSortNo).ToList();
        }

        public bool Delete(EntityDicMicAntibio antibio)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dant_id", antibio.AntId);

                helper.UpdateOperation("Dict_mic_antibio", values, key);
                //删除抗生素后  还需更新药敏卡中抗生素的删除标志
                new DaoDicMicAntidetail().UpdateDelFlagByAntiCode(antibio.AntId);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicAntibio antibio)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_mic_antibio");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dant_id", id);
                values.Add("Dant_ename", antibio.AntEname);
                values.Add("Dant_cname", antibio.AntCname);
                values.Add("Dant_code", antibio.AntCode);
                values.Add("Dant_std_upper_limit", antibio.AntStdUpperLimit);
                values.Add("Dant_std_middle_limit", antibio.AntStdMiddleLimit);
                values.Add("Dant_std_lower_limit", antibio.AntStdLowerLimit);
                values.Add("Dant_zone_lower_limit", antibio.AntZoneLowerLimit);
                values.Add("Dant_zone_upper_limit", antibio.AntZoneUpperLimit);
                values.Add("Dant_flag", antibio.AntFlag);
                values.Add("Dant_zone_durgfast", antibio.AntZoneDurgfast);
                values.Add("Dant_zone_intermed", antibio.AntZoneIntermed);
                values.Add("Dant_zone_sensitive", antibio.AntZoneSensitive);
                values.Add("Dant_pirnt_flag", antibio.AntPirntFlag);
                values.Add("Dant_method", antibio.AntMethod);
                values.Add("Dant_serum", antibio.AntSerum);
                values.Add("Dant_urine", antibio.AntUrine);
                values.Add("Dant_mit_no", antibio.AntMitNo);
                values.Add("Dant_who_no", antibio.AntWhoNo);
                values.Add("Dant_c_code", antibio.AntCCode);
                values.Add("py_code", antibio.AntPyCode);
                values.Add("wb_code", antibio.AntWbCode);
                values.Add("sort_no", antibio.AntSortNo);
                values.Add("del_flag", antibio.AntDelFlag);
                values.Add("Dant_kb_who_no", antibio.AntKbWhoNo);
                values.Add("Dant_clsi_comment", antibio.AntClsiComment);
                //values.Add("ant_notes", antibio.AntNotes);
                //values.Add("ant_who_etest_no", antibio.AntWhoEtestNo);

                values.Add("Dant_notes", antibio.AntNotes);
                values.Add("Dant_type_id", antibio.AntTypeId);
                values.Add("Dant_unit_name", antibio.AntUnitName);

                helper.InsertOperation("Dict_mic_antibio", values);

                antibio.AntId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicAntibio> Search(object obj)
        {
            try
            {
                String sql = @"SELECT * FROM Dict_mic_antibio WHERE del_flag = 0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicAntibio>();
            }
        }

        public bool Update(EntityDicMicAntibio antibio)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dant_ename", antibio.AntEname);
                values.Add("Dant_cname", antibio.AntCname);
                values.Add("Dant_code", antibio.AntCode);
                values.Add("Dant_std_upper_limit", antibio.AntStdUpperLimit);
                values.Add("Dant_std_middle_limit", antibio.AntStdMiddleLimit);
                values.Add("Dant_std_lower_limit", antibio.AntStdLowerLimit);
                values.Add("Dant_zone_lower_limit", antibio.AntZoneLowerLimit);
                values.Add("Dant_zone_upper_limit", antibio.AntZoneUpperLimit);
                values.Add("Dant_flag", antibio.AntFlag);
                values.Add("Dant_zone_durgfast", antibio.AntZoneDurgfast);
                values.Add("Dant_zone_intermed", antibio.AntZoneIntermed);
                values.Add("Dant_zone_sensitive", antibio.AntZoneSensitive);
                values.Add("Dant_pirnt_flag", antibio.AntPirntFlag);
                values.Add("Dant_method", antibio.AntMethod);
                values.Add("Dant_serum", antibio.AntSerum);
                values.Add("Dant_urine", antibio.AntUrine);
                values.Add("Dant_mit_no", antibio.AntMitNo);
                values.Add("Dant_who_no", antibio.AntWhoNo);
                values.Add("Dant_c_code", antibio.AntCCode);
                values.Add("py_code", antibio.AntPyCode);
                values.Add("wb_code", antibio.AntWbCode);
                values.Add("sort_no", antibio.AntSortNo);
                values.Add("del_flag", antibio.AntDelFlag);
                values.Add("Dant_kb_who_no", antibio.AntKbWhoNo);
                values.Add("Dant_clsi_comment", antibio.AntClsiComment);
                //values.Add("ant_notes", antibio.AntNotes);
                //values.Add("ant_who_etest_no", antibio.AntWhoEtestNo);

                values.Add("Dant_notes", antibio.AntNotes);
                values.Add("Dant_type_id", antibio.AntTypeId);
                values.Add("Dant_unit_name", antibio.AntUnitName);


                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dant_id", antibio.AntId);

                helper.UpdateOperation("Dict_mic_antibio", values, key);
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
