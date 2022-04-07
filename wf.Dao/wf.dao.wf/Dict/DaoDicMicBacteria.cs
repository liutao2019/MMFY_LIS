using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using dcl.dao.core;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicBacteria>))]
    public class DaoDicMicBacteria : IDaoDic<EntityDicMicBacteria>
    {
        public List<EntityDicMicBacteria> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicMicBacteria> list = new List<EntityDicMicBacteria>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicMicBacteria bacteria = new EntityDicMicBacteria();
                bacteria.BacId = item["Dbact_id"].ToString();
                bacteria.BacEname = item["Dbact_ename"].ToString();
                bacteria.BacCname = item["Dbact_cname"].ToString();
                bacteria.BacBtId = item["Dbact_Dbactt_id"].ToString();
                bacteria.BacMitNo = item["Dbact_mit_no"].ToString();
                bacteria.BacWhoNo = item["Dbact_who_no"].ToString();
                bacteria.BacCode = item["Dbact_code"].ToString();
                bacteria.BTypeCname = item["Dbactt_cname"].ToString();

                int flag = 0;
                if (item["Dbact_flag"] != null && item["Dbact_flag"] != DBNull.Value)
                    int.TryParse(item["Dbact_flag"].ToString(), out flag);
                bacteria.BacFlag = flag;

                bacteria.BacCCode = item["Dbact_c_code"].ToString();

                int posiFalg = 0;
                if (item["Dbact_positive_flag"] != null && item["Dbact_positive_flag"] != DBNull.Value)
                    int.TryParse(item["Dbact_positive_flag"].ToString(), out posiFalg);
                bacteria.BacPositiveFlag = posiFalg;

                bacteria.BacPyCode = item["py_code"].ToString();
                bacteria.BacWbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out flag);
                bacteria.BacSortNo = sort;

                bacteria.BacDelFlag = item["del_flag"].ToString();

                bacteria.BacRemark = item["Dbact_remark"].ToString();
                bacteria.BacDx = item["Dbact_dx"].ToString();
                bacteria.BacXt = item["Dbact_xt"].ToString();
                bacteria.BacBm = item["Dbact_mb"].ToString();
                bacteria.BacYs = item["Dbact_ys"].ToString();
                bacteria.BacBy = item["Dbact_by"].ToString();
                bacteria.BacRx = item["Dbact_rx"].ToString();
                bacteria.BacTmd = item["Dbact_tmd"].ToString();
                bacteria.BacScfs = item["Dbact_scfs"].ToString();
                bacteria.BacXjjs = item["Dbact_xjjs"].ToString();

                list.Add(bacteria);
            }
            return list.OrderBy(i => i.BacSortNo).ToList();
        }

        public bool Delete(EntityDicMicBacteria bacteria)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dbact_id", bacteria.BacId);

                helper.UpdateOperation("Dict_mic_bacteria", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicBacteria bacteria)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_mic_bacteria");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dbact_id", id);
                values.Add("Dbact_ename", bacteria.BacEname);
                values.Add("Dbact_cname", bacteria.BacCname);
                values.Add("Dbact_Dbactt_id", bacteria.BacBtId);
                values.Add("Dbact_mit_no", bacteria.BacMitNo);
                values.Add("Dbact_who_no", bacteria.BacWhoNo);
                values.Add("Dbact_code", bacteria.BacCode);
                values.Add("Dbact_flag", bacteria.BacFlag);
                values.Add("Dbact_c_code", bacteria.BacCCode);
                values.Add("Dbact_positive_flag", bacteria.BacPositiveFlag);
                values.Add("py_code", bacteria.BacPyCode);
                values.Add("wb_code", bacteria.BacWbCode);
                values.Add("sort_no", bacteria.BacSortNo);
                values.Add("del_flag", bacteria.BacDelFlag);

                values.Add("Dbact_remark", bacteria.BacRemark);
                values.Add("Dbact_dx", bacteria.BacDx);
                values.Add("Dbact_xt", bacteria.BacXt);
                values.Add("Dbact_mb", bacteria.BacBm);
                values.Add("Dbact_ys", bacteria.BacYs);
                values.Add("Dbact_by", bacteria.BacBy);
                values.Add("Dbact_rx", bacteria.BacRx);
                values.Add("Dbact_tmd", bacteria.BacTmd);
                values.Add("Dbact_scfs", bacteria.BacScfs);
                values.Add("Dbact_xjjs", bacteria.BacXjjs);

                helper.InsertOperation("Dict_mic_bacteria", values);

                bacteria.BacId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicBacteria> Search(object obj)
        {
            try
            {
                String sql = @"SELECT  Dict_mic_bacteria.*, 
Dict_mic_bacttype.Dbactt_cname
FROM     
Dict_mic_bacteria left outer JOIN
Dict_mic_bacttype ON Dict_mic_bacteria.Dbact_Dbactt_id = Dict_mic_bacttype.Dbactt_id where 
Dict_mic_bacteria.del_flag=0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicBacteria>();
            }
        }

        public bool Update(EntityDicMicBacteria bacteria)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dbact_ename", bacteria.BacEname);
                values.Add("Dbact_cname", bacteria.BacCname);
                values.Add("Dbact_Dbactt_id", bacteria.BacBtId);
                values.Add("Dbact_mit_no", bacteria.BacMitNo);
                values.Add("Dbact_who_no", bacteria.BacWhoNo);
                values.Add("Dbact_code", bacteria.BacCode);
                values.Add("Dbact_flag", bacteria.BacFlag);
                values.Add("Dbact_c_code", bacteria.BacCCode);
                values.Add("Dbact_positive_flag", bacteria.BacPositiveFlag);
                values.Add("py_code", bacteria.BacPyCode);
                values.Add("wb_code", bacteria.BacWbCode);
                values.Add("sort_no", bacteria.BacSortNo);
                values.Add("del_flag", bacteria.BacDelFlag);


                values.Add("Dbact_remark", bacteria.BacRemark);
                values.Add("Dbact_dx", bacteria.BacDx);
                values.Add("Dbact_xt", bacteria.BacXt);
                values.Add("Dbact_mb", bacteria.BacBm);
                values.Add("Dbact_ys", bacteria.BacYs);
                values.Add("Dbact_by", bacteria.BacBy);
                values.Add("Dbact_rx", bacteria.BacRx);
                values.Add("Dbact_tmd", bacteria.BacTmd);
                values.Add("Dbact_scfs", bacteria.BacScfs);
                values.Add("Dbact_xjjs", bacteria.BacXjjs);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dbact_id", bacteria.BacId);

                helper.UpdateOperation("Dict_mic_bacteria", values, key);
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
