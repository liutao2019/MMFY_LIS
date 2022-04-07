using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using System.Collections;
using dcl.root.dac;
using Lib.DataInterface.Implement;
using Lis.SendDataToCDR;
using dcl.svr.frame;
using dcl.svr.root.com;
using dcl.svr.interfaces;
using System.Configuration;

namespace dcl.svr.sample
{
    public class BCCNameBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.CName.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.CName.ID; }
        }

        public override DataSet Search(string where)
        {
            //modified by corossol 2011-01-28
            //return doSearch(new DataSet(), SearchSQL + string.Format(" Where {0} AND bc_del <> '1' " , where));

            DataSet ds = doSearch(new DataSet(), string.Format(@"
select bc_cname.*,dict_combine.com_pri,dict_combine.com_code,isnull(dict_combine.com_urgent_flag,0) com_urgent_flag
from bc_cname
left join dict_combine on bc_cname.bc_lis_code = dict_combine.com_id
where {0} and bc_del <> '1'", where));

            return ds;
        }

        public override System.Data.DataSet doNew(System.Data.DataSet dsData)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable currentTable = dsData.Tables[MainTable];

                ArrayList arrNew = dao.GetInsertSql(currentTable);
                dao.DoTran(arrNew);
                result.Tables.Add(currentTable.Copy());
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
            }
            return result;
        }

        public override System.Data.DataSet doOther(System.Data.DataSet ds)
        {
            DataSet result = new DataSet();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("bc_cname_del")
                && ds.Tables["bc_cname_del"] != null && ds.Tables["bc_cname_del"].Rows.Count > 0)
            {
                DataTable dt = ds.Tables["bc_cname_del"];
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    try
                    {
                        string strID = dt.Rows[i]["bc_id"].ToString();
                        string strDeleteSql = string.Format("delete bc_cname where bc_id={0}", strID);
                        DBHelper helper = new DBHelper();
                        helper.ExecuteNonQuery(strDeleteSql);
                    }
                    catch (Exception ex)
                    {
                        result.Tables.Add(CommonBIZ.createErrorInfo("删除数据出错!", ex.ToString())); ;
                    }

                }
                return result;
            }

            string sql = ds.Tables["sql"].Rows[0]["sql"].ToString();
            
            try
            {
                DataTable dt = dao.GetDataTable(sql, "sql");
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return result;
        }

        public override int Update(string set, string where)
        {

            int i = base.Update(set, where);
            if (set.ToLower().Replace(" ", "").Contains("bc_del=1"))//by lin:悲剧，只能用字符串来判断是否为删除条码
            {
                DBHelper helper = new DBHelper();
                #region 撤销时发送HL7消息

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid") == "HL7" ||
                    dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "惠侨CDR") //撤销时发送HL7消息
                {
                    if (!string.IsNullOrEmpty(where))
                    {
                        string sql = string.Format(@"
select pa.bc_in_no,cn.bc_yz_id,pa.bc_ori_id 
from bc_cname as cn,bc_patients as pa with(nolock) 
where pa.bc_bar_code=cn.bc_bar_code and cn.{0}", where);

                        DataTable table = helper.GetTable(sql);
                        if (table.Rows.Count != 0)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                string yz_id = row["bc_yz_id"].ToString(); //医嘱ID
                                string bc_in_no = row["bc_in_no"].ToString(); //就诊号码
                                string bc_ori_id = row["bc_ori_id"].ToString(); //病人来源

                                if (!string.IsNullOrEmpty(yz_id))
                                {
                                    if (bc_ori_id == "108")
                                    {
                                        new AdviceConfirmBIZ().AdviceCancelConfirm_ZY(bc_in_no, yz_id, string.Empty, string.Empty);
                                    }
                                    else if (bc_ori_id == "107")
                                    {
                                        new AdviceConfirmBIZ().AdviceCancelConfirm_MZ(bc_in_no, yz_id, string.Empty, string.Empty);
                                    }
                                    else if (bc_ori_id == "109")
                                    {
                                        new AdviceConfirmBIZ().AdviceCancelConfirm_TJ(bc_in_no, yz_id, string.Empty, string.Empty);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion


                string sqlBcCname = @"
                select
                bc_patients.bc_bar_code,
                bc_patients.bc_ori_id,
                bc_patients.bc_pid,
                bc_patients.bc_times,
                bc_patients.bc_in_no,
                bc_patients.bc_emp_id,
                bc_patients.bc_social_no,
                bc_cname.bc_yz_id,
                bc_cname.bc_del,
                bc_cname.bc_his_code
                from bc_patients with(nolock)
                inner join bc_cname with(nolock) on bc_cname.bc_bar_code = bc_patients.bc_bar_no
                where bc_cname." + where;

                DataTable tableCname = helper.GetTable(sqlBcCname);
                if (tableCname.Rows.Count > 0)
                {
                    string ori_id = tableCname.Rows[0]["bc_ori_id"].ToString();
                    foreach (DataRow row in tableCname.Rows)
                    {
                        if (where != null && (row["bc_del"] != null && row["bc_del"].ToString() == "1"
                                              && where.ToLower().Replace(" ", "").Contains("bc_bar_no")))
                            continue;
                        string advice_id = row["bc_yz_id"].ToString();
                        if (advice_id.Trim() != string.Empty)
                        {
                            string bc_in_no = row["bc_in_no"].ToString();
                            string bc_bar_code = row["bc_bar_code"].ToString();
                            string bc_his_code = row["bc_his_code"].ToString();
                            string bc_times = row["bc_times"].ToString();
                            string bc_emp_id = row["bc_emp_id"].ToString();
                            string bc_social_no = row["bc_social_no"].ToString();

                            if (string.IsNullOrEmpty(bc_times))
                                bc_times = "0";
                            string bc_pid = row["bc_pid"].ToString();

                            List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                            list.Add(new InterfaceDataBindingItem("bc_in_no", bc_in_no));
                            list.Add(new InterfaceDataBindingItem("bc_yz_id", advice_id));
                            list.Add(new InterfaceDataBindingItem("bc_bar_code", bc_bar_code));
                            list.Add(new InterfaceDataBindingItem("bc_his_code", bc_his_code));
                            list.Add(new InterfaceDataBindingItem("bc_times", bc_times));
                            list.Add(new InterfaceDataBindingItem("bc_pid", bc_pid));
                            list.Add(new InterfaceDataBindingItem("bc_emp_id", bc_pid));
                            list.Add(new InterfaceDataBindingItem("bc_social_no", bc_social_no));

                            DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                            if (ori_id == "107")
                            {
                                string gn = "条码_门诊_删除后";
                                if (ConfigurationManager.AppSettings["HospitalType"] == "1")
                                    gn = "分院_" + gn;
                                dihelper.ExecuteNonQueryWithGroup(gn, list.ToArray());
                            }
                            else if (ori_id == "108")
                            {
                                dihelper.ExecuteNonQueryWithGroup("条码_住院_删除后", list.ToArray());
                            }
                            else if (ori_id == "109")
                            {
                                dihelper.ExecuteNonQueryWithGroup("条码_体检_删除后", list.ToArray());
                            }
                        }
                    }
 
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "惠侨CDR")
                    {
                        DataTable dtBarCode = new DataView(tableCname).ToTable(true, new string[] { "bc_bar_code", "bc_ori_id" });
                        CDRService send = new CDRService();
                        foreach (DataRow item in dtBarCode.Rows)
                        {
                            if (item["bc_ori_id"].ToString() == "108")
                                send.CancelZyCuvCharge(item["bc_bar_code"].ToString());
                        }

                    }
                }

            }
            return i;
        }
    }
}