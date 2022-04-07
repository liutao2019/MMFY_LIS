using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.root.dac;
using lis.dto;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using lis.dto.Entity;
using lis.dto.BarCodeEntity;
using System.Collections;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using dcl.servececontract;
using dcl.svr.frame;
using dcl.svr.cache;
using dcl.pub.entities.dict;

namespace dcl.svr.sample
{
    public class BarcodeDictBIZ : IBarcodeDict
    {

        IBatisSQLHelper sqlHelper = new IBatisSQLHelper();

        public IList<BarcodeCombines> SearchBarcodeCombineByHISID(IDictionary select)
        {
            return sqlHelper.SqlMapper.QueryForList<BarcodeCombines>("SelectBarcodeCombinesByHisCombineCode", select);
        }

        public IList<Combines> FindLisCombines(IList<string> ids)
        {
            if (ids == null || ids.Count <= 0)
                return null;
            return sqlHelper.SqlMapper.QueryForList<Combines>("FindLisCombineByIDs", ids);
        }

        public bool AddBarcodeCombine(BarcodeCombines barcodeCombines)
        {
            sqlHelper.SqlMapper.BeginTransaction();
            Object o = sqlHelper.SqlMapper.Insert("InsertBarcodeCombines", barcodeCombines);
            sqlHelper.SqlMapper.CommitTransaction(true);

            return o != null;
        }

        public bool UpdateBarcodeCombine(BarcodeCombines barcodeCombines)
        {
            sqlHelper.SqlMapper.BeginTransaction();
            int i = sqlHelper.SqlMapper.Update("UpdateBarcodeCombines", barcodeCombines);
            sqlHelper.SqlMapper.CommitTransaction(true);

            return i > 0;
        }

        public IList<BarcodeCombines> SearchBarcodeCombines(string combineID)
        {
            return sqlHelper.SqlMapper.QueryForList<BarcodeCombines>("SelectBarcodeCombines", combineID);
        }

        public Combines GetLisCombineByHISCode(string hisCode)
        {
            IList<Combines> combines = sqlHelper.SqlMapper.QueryForList<Combines>("SelectCombinesByHISCode", hisCode);
            if (combines != null && combines.Count > 0)
                return combines[0];
            else
                return null;
        }

        public Combines GetLisCombineByID(string id)
        {
            return sqlHelper.SqlMapper.QueryForObject<Combines>("SelectCombines", id);
        }

        public DataSet FindInstrmentBy(IList<string> combinesID, string typeID)
        {

            return sqlHelper.QueryForDataSet("SelectDictInstrmtsByCombinesID", combinesID);
        }

        internal IList<BarcodeCombines> FindSubCombines(string combineID)
        {
            return sqlHelper.SqlMapper.QueryForList<BarcodeCombines>("FindSubCombines", combineID);
        }

        internal IList<Combines> SearchCombineByHISID(IList<string> hisCombineIDs)
        {
            if (hisCombineIDs == null || hisCombineIDs.Count == 0)
                return null;
            return sqlHelper.SqlMapper.QueryForList<Combines>("SearchCombineByManyHISID", hisCombineIDs);
        }

        public IList<BarcodeCombines> FindBarcodeCombineByHISFeeCode(IList<string> hisCombineIDs)
        {
            if (hisCombineIDs == null || hisCombineIDs.Count == 0)
                return null;
            IList<BarcodeCombines> ret = sqlHelper.SqlMapper.QueryForList<BarcodeCombines>("SearchBarcodeCombineByManyHISID", hisCombineIDs);

            if (!string.IsNullOrEmpty(ServerConfig.GetHospitalID()))
            {
                IList<BarcodeCombines> retList = new List<BarcodeCombines>();

                foreach (BarcodeCombines barcodeCombinese in ret)
                {
                    EntityDictCombine com = DictCombineCache.Current.GetCombineByID(barcodeCombinese.CombineId, false);
                    if (com != null)
                    {
                        retList.Add(barcodeCombinese);
                    }
                }
                retList = retList.OrderBy(i => i.ComSeq).ToList();
                return retList;
            }
            ret = ret.OrderBy(i => i.ComSeq).ToList();
            return ret;
        }

        #region IBarcodeDict 成员


        public int FindPatientsSid(string combinesID, string itr_id, DateTime pat_date)
        {
            string strSql = string.Format("select * from dict_instrmt_com where itr_id='{0}' and com_id='{1}'", itr_id, combinesID);
            DBHelper db = new DBHelper();
            DataTable dtInsCom = db.GetTable(strSql);
            if (dtInsCom != null && dtInsCom.Rows.Count > 0 &&
                dtInsCom.Rows[0]["itrcom_start_sid"] != DBNull.Value && dtInsCom.Rows[0]["itrcom_start_sid"] != null &&
                dtInsCom.Rows[0]["itrcom_end_sid"] != DBNull.Value && dtInsCom.Rows[0]["itrcom_end_sid"] != null &&
                dtInsCom.Rows[0]["itrcom_start_sid"].ToString().Trim() != "0" &&
                dtInsCom.Rows[0]["itrcom_end_sid"].ToString().Trim() != "0")
            {
                int startSid = Convert.ToInt32(dtInsCom.Rows[0]["itrcom_start_sid"]);
                int endSid = Convert.ToInt32(dtInsCom.Rows[0]["itrcom_end_sid"]);

                string patientsname = "patients";

                //if (dtInsCom.Rows[0]["itr_rep_flag"] != null && dtInsCom.Rows[0]["itr_rep_flag"].ToString() == "6")
                //    patientsname = "patients_newborn";

                string sqlSid = string.Format(@"select max(cast(pat_sid as bigint)) pat_sid from {5} where pat_itr_id='{0}' and 
                                            cast(pat_sid as bigint)>={1} and cast(pat_sid as bigint)<={2} 
                                            and pat_date>='{3}' and pat_date<'{4}'", itr_id, startSid, endSid, pat_date, pat_date.AddDays(1),patientsname);
                DataTable dtPatients = db.GetTable(sqlSid);
                if (dtPatients != null && dtPatients.Rows.Count > 0
                    && dtPatients.Rows[0]["pat_sid"] != DBNull.Value
                    && dtPatients.Rows[0]["pat_sid"] != null)
                {
                    return Convert.ToInt32(dtPatients.Rows[0]["pat_sid"]) + 1;
                }
                else
                    return startSid;
            }
            else
                return -1;
        }

        #endregion


        public string TranferBarcode(string barCode)
        {
            try
            {
                BCPatientBIZ bllBcPatient = new BCPatientBIZ();
                DataSet dsBarCodePatient = bllBcPatient.SearchByBarcode(barCode);

                if (dsBarCodePatient == null || dsBarCodePatient.Tables.Count == 0 ||
                    dsBarCodePatient.Tables[0].Rows.Count == 0)
                    return "无数据";


                DataTable dtBarCodePat = dsBarCodePatient.Tables[0];

                DataRow row = dtBarCodePat.Rows[0];


                string bc_ori_id = row["bc_ori_id"].ToString();

                DataTable dtBCCombine = new BarcodeBIZ().GetPatientCombine(barCode);


                StringBuilder hisFeeCodes=new StringBuilder();
                StringBuilder lisCodes = new StringBuilder();

                int i = 0;
                int j = 0;
                foreach (DataRow cRow in dtBCCombine.Rows)
                {
                    if(cRow["bc_yz_id"]==null||string.IsNullOrEmpty(cRow["bc_yz_id"].ToString()))
                        continue;

                    if (i == 0)
                    {
                        hisFeeCodes.Append(string.Format("'{0}'", cRow["bc_yz_id"]));
                    }
                    else
                    {
                        hisFeeCodes.Append(string.Format(",'{0}'", cRow["bc_yz_id"]));
                    }
                    i++;

                    if (cRow["bc_lis_code"] == null || string.IsNullOrEmpty(cRow["bc_lis_code"].ToString()))
                        continue;

                    if (j == 0)
                    {
                        lisCodes.Append(string.Format("'{0}'", cRow["bc_lis_code"]));
                    }
                    else
                    {
                        lisCodes.Append(string.Format(",'{0}'", cRow["bc_lis_code"]));
                    }
                   j++;
                }


                if (hisFeeCodes.Length > 0)
                {


                    string strSql = string.Format(@" SELECT
      com_key AS Id,dict_combine.com_ptype,
      dict_combine_bar.com_id AS CombineId,
      dict_combine.com_name as LisCombineName,
      dict_combine.com_seq as ComSeq,com_his_fee_code
      FROM dict_combine_bar
      inner JOIN dict_combine on dict_combine.com_id = dict_combine_bar.com_id
      WHERE
      (dict_combine.com_del = '0' or dict_combine.com_del is null) and dict_combine.com_id is NOT NULL
      and dict_combine_bar.com_his_fee_code IN ({0}) and dict_combine_bar.com_id not in ({1}) and com_combine_source='{2}' ",
                                                  hisFeeCodes, (lisCodes.Length == 0 ? "'1'" : lisCodes.ToString()),
                                                  bc_ori_id);

                    DBHelper db = new DBHelper();
                    DataTable dtInsCom = db.GetTable(strSql);



                    if (dtInsCom.Rows.Count == 0)
                    {
                        return "当前医院未设置当前条码组合的收费码";
                    }
                    string ptype = string.Empty;
                    string updatesql;
                    List<string> updateSqlList = new List<string>();
                    foreach (DataRow cRow in dtBCCombine.Rows)
                    {
                        if (cRow["bc_yz_id"] == null || string.IsNullOrEmpty(cRow["bc_yz_id"].ToString()))
                            continue;

                        DataRow[] rows = dtInsCom.Select(string.Format("com_his_fee_code='{0}'", cRow["bc_yz_id"]));

                        if (rows.Length > 0)
                        {

                            if (string.IsNullOrEmpty(ptype) && rows[0]["com_ptype"] != null &&
                                !string.IsNullOrEmpty(rows[0]["com_ptype"].ToString()))
                            {
                                ptype = rows[0]["com_ptype"].ToString();
                                updatesql =
                              string.Format(
                                  @" update bc_patients set bc_ctype='{0}' where  bc_bar_code='{1}' ",ptype,  barCode);

                                updateSqlList.Add(updatesql);
                                
                            }
                             updatesql =
                                string.Format(
                                    @" update bc_cname set bc_lis_code='{0}' where bc_yz_id='{1}' and bc_bar_code='{2}' ",
                                    rows[0]["CombineId"], cRow["bc_yz_id"], barCode);

                            updateSqlList.Add(updatesql);
                        }
                        else
                        {
                            return string.Format("本院无医嘱ID:{0} 对应的组合", cRow["bc_yz_id"]);
                        }

                    }

                    using (DBHelper tran=DBHelper.BeginTransaction())
                    {
                        try
                        {
                            foreach (string sql in updateSqlList)
                            {
                                tran.ExecuteNonQuery(sql);
                            }
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.RollBack();
                            throw;
                        }
                       
                    }
                }
                else
                {
                    return "该条码无医嘱ID";
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return ex.Message;
            }
            return string.Empty;
        }
    }
}
