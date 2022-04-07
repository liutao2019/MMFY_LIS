using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.root.com;
using System.Data;
using dcl.root.dto;
using System.Collections;
using lis.dto;

namespace dcl.svr.tools
{
    public class EfficacyManageBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtUpdate = ds.Tables["resultoUpdate"];
                ArrayList arrUpdate = new ArrayList();

                List<string> notepatIDs = new List<string>();//记录pat_id 用于更新计算项目

                if (dtUpdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtUpdate.Rows.Count; i++)
                    {
                        string sql = @"update resulto set res_chr =" + dtUpdate.Rows[i]["res_chr"] + ",res_cast_chr=" + dtUpdate.Rows[i]["res_cast_chr"] + " where res_id='" + dtUpdate.Rows[i]["res_id"] +
                    "' and res_itm_ecd='" + dtUpdate.Rows[i]["res_itm_ecd"] + "' and res_sid=" + dtUpdate.Rows[i]["res_sid"];
                        arrUpdate.Add(sql);

                        if (!string.IsNullOrEmpty(dtUpdate.Rows[i]["res_id"].ToString()))
                        {
                            if (!notepatIDs.Contains(dtUpdate.Rows[i]["res_id"].ToString()))
                            {
                                notepatIDs.Add(dtUpdate.Rows[i]["res_id"].ToString());
                            }
                        }
                    }
                    dao.DoTran(arrUpdate);
                    result.Tables.Add(dtUpdate.Copy());

                    if (notepatIDs.Count > 0)
                    {
                        for (int j = 0; j < notepatIDs.Count; j++)
                        {
                            //更新计算项目
                            updateGenerateAutoCalItem(notepatIDs[j]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("计算错误！", ex.ToString()));
            }
            return result;
        }

        /// <summary>
        /// 更新计算项目
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        private bool updateGenerateAutoCalItem(string patID)
        {
            bool bln = false;

            dcl.root.dac.DBHelper helper = new dcl.root.dac.DBHelper();

            string selectPatState = string.Format("select top 1 pat_id,pat_sid,pat_flag,pat_sam_id,pat_itr_id,pat_sam_rem from patients where pat_id = '{0}'", patID);

            DataTable dtPat = helper.GetTable(selectPatState);

            dcl.pub.entities.EntityPatient2 entityPatient = new dcl.pub.entities.EntityPatient2();

            if (dtPat.Rows.Count > 0)
            {
                entityPatient.pat_id = dtPat.Rows[0]["pat_id"].ToString();
                entityPatient.pat_sid = dtPat.Rows[0]["pat_sid"].ToString();
                entityPatient.pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();

                if (!dcl.common.Compare.IsEmpty(dtPat.Rows[0]["pat_flag"]))
                {
                    entityPatient.pat_flag = Convert.ToInt32(dtPat.Rows[0]["pat_flag"]);
                }
                else
                {
                    entityPatient.pat_flag = 0;
                }


                if (entityPatient.pat_flag == 0)
                {
                    string sampRem = string.Empty;
                    if (!dcl.common.Compare.IsEmpty(dtPat.Rows[0]["pat_sam_rem"]))
                    {
                        sampRem = dtPat.Rows[0]["pat_sam_rem"].ToString();
                    }
                    //关联计算项目
                    dcl.svr.result.PatReadBLL.NewInstance.GenerateAutoCalItemToOut(entityPatient, entityPatient.pat_sam_id, sampRem);
                    bln = true;
                }
            }

            return bln;
        }

        public DataSet doOther(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                /*
              String sql =
                @"SELECT * FROM dict_checkb where chk_del = '" + dto.LIS_Const.del_flag.OPEN + "'";
                 */
                String sql = "select itr_id,itr_mid,itr_name from dict_instrmt where itr_del=0";

                DataTable dt = dao.GetDataTable(sql);
                dt.TableName = "dict_instrmt";
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                String sql = @"select *,isnull((select max(dict_item_sam.itm_max_digit) from dict_item_sam where dict_item_sam.itm_id=dict_item.itm_id),0) as itm_max_digit 
from dict_item where itm_del=0";

                DataTable dt = dao.GetDataTable(sql);
                dt.TableName = "dict_item";
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtNew = ds.Tables["resulto"];
                string sql = @"update resulto set res_chr =" + dtNew.Rows[0]["res_chr"] + " where res_id='" + dtNew.Rows[0]["res_id"] +
                    "' and res_itm_ecd='" + dtNew.Rows[0]["res_itm_ecd"] + "' and res_sid=" + dtNew.Rows[0]["res_sid"];
                ArrayList arrNew = new ArrayList();
                arrNew.Add(sql);
                dao.DoTran(arrNew);
                result.Tables.Add(dtNew.Copy());
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("计算错误！", ex.ToString()));
            }
            return result;
        }

        public DataSet doView(DataSet ds)
        {
            DataSet res = new DataSet();
            try
            {
                String itm_ced = ds.Tables["res"].Rows[0]["sql"].ToString();
                string sql = @"select
res_key,
res_id,
res_itr_id,
cast(res_sid as bigint) res_sid,
res_itm_id,
res_itm_ecd,
res_chr,
res_od_chr,
res_cast_chr,
res_unit,
res_price,
res_ref_l,
res_ref_h,
res_ref_exp,
res_ref_flag,
res_meams,
res_date,
res_flag,
res_type,
res_rep_type,
res_com_id,
res_itm_rep_ecd,
pat_flag,isnull(pat_host_order,'')  pat_host_order,
itm_max_digit = (select top 1 isnull(itm_max_digit,0) from dict_item_sam where itm_id = resulto.res_itm_id)
from patients
left join resulto on resulto.res_id = patients.pat_id " + itm_ced + " order by res_sid";

                DataTable dt = dao.GetDataTable(sql);
                dt.TableName = "resulto";
                res.Tables.Add(dt);
                return res;
            }
            catch (Exception ex)
            {
                res.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return res;
        }

        #endregion
    }
}
