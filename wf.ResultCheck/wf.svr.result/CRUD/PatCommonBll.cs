using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.svr.cache;
using System.Data.SqlClient;
using dcl.root.dac;
using lis.dto.Entity;
using lis.dto.BarCodeEntity;
using dcl.root.logon;
using Lib.DataInterface.Implement;
using dcl.common;
using dcl.svr.framedic;
using dcl.pub.entities;
using dcl.svr.sample;
using dcl.entity;

namespace dcl.svr.result.CRUD
{
    public class PatCommonBll
    {
        /// <summary>
        /// 调整采样时间
        /// </summary>
        /// <param name="dtPatInfo"></param>
        public void AdjustSampleDate(DataTable dtPatInfo)
        {
            if (dtPatInfo.Rows.Count == 0)
            {
                return;
            }

            DataRow drPat = dtPatInfo.Rows[0];

            //if (Compare.IsEmpty(drPat["pat_sdate"]))
            //{

            //}


            //IF 采血时间 < 送检时间 – 90分钟 THEN              90分钟 < 送检时间 - 采血时间
            //   采血时间 = 送检时间 – 90 分钟
            //ELSEIF 采血时间 > 送检时间 – 20分钟 THEN          20分钟 > 送检时间 - 采血时间
            //   采血时间 = 送检时间 – 20 分钟
            //ELSEIF 采血时间 = 空 THEN
            //   采血时间 =送检时间 – 90 分钟
            //ELSEIF 

            if (!Compare.IsEmpty(drPat["pat_sample_date"]) &&
                !Compare.IsEmpty(drPat["pat_sdate"]))
            {
                DateTime pat_sample_date = Convert.ToDateTime(drPat["pat_sample_date"]);//采样时间
                DateTime pat_sdate = Convert.ToDateTime(drPat["pat_sdate"]);//送检时间

                if ((pat_sdate - pat_sample_date).TotalMinutes > 90)
                {
                    pat_sample_date = pat_sdate.AddMinutes(-90);
                }
                else if ((pat_sdate - pat_sample_date).TotalMinutes < 20)
                {
                    pat_sample_date = pat_sdate.AddMinutes(-20);
                }

                drPat["pat_sample_date"] = pat_sample_date;
                drPat["pat_sdate"] = pat_sdate;

            }
            else if (Compare.IsEmpty(drPat["pat_sample_date"]) && !Compare.IsEmpty(drPat["pat_sdate"]))
            {
                DateTime pat_sdate = Convert.ToDateTime(drPat["pat_sdate"]);//送检时间
                DateTime pat_sample_date = pat_sdate.AddMinutes(-90);

                drPat["pat_sample_date"] = pat_sample_date;
            }
        }


        /// <summary>
        /// 插入缺省结果
        /// </summary>
        /// <param name="transHelper"></param>
        public void InsertDefaultResult(string pat_id, string pat_sam_id, string pat_itr_id, string pat_sid, DataTable dtCombine, DBHelper transHelper)
        {
            try
            {
                if (dtCombine != null && dtCombine.Rows.Count > 0)
                {
                   

                    if (transHelper == null)
                    {
                        transHelper = new DBHelper();
                    }
                      DateTime dtToday = DateTime.Now;
                    //细菌报告管理涂片结果允许自动保存默认值
                      if (!string.IsNullOrEmpty(pat_itr_id) && CacheSysConfig.Current.GetSystemConfig("AntiLab_AutoSaveDefValue") == "是")
                      {
                          var itrcache = DictInstrmtCache.Current.GetInstructmentByID(pat_itr_id);

                          if (itrcache != null && itrcache.ItrReportType == "3")
                          {
                              string sqlcs = string.Format("select * from cs_rlts with(nolock) where bsr_id = '{0}' ", pat_id);
                              DataTable dtCs = transHelper.GetTable(sqlcs);
                              if (dtCs.Rows.Count > 0) return;
                              DataTable dtCsInsert = dtCs.Clone();
                              int i = 0;
                              foreach (DataRow drCom in dtCombine.Rows)
                              {
                                  //组合ID
                                  string com_id = drCom["pat_com_id"].ToString();

                                  //根据 组合ID和标本 获取具有缺省值的项目
                                  List<string> list = GetCombineDefData(pat_itr_id,com_id );

                                  if (list.Count == 0) continue;

                                  foreach (string defvalue in list)
                                  {

                                      DataRow drCs = dtCsInsert.NewRow();
                                      drCs["bsr_id"] = pat_id;
                                      drCs["bsr_mid"] = pat_itr_id;
                                      drCs["bsr_date"] = dtToday;
                                      drCs["bsr_sid"] = pat_sid;
                                      drCs["bsr_cname"] = defvalue;
                                      drCs["bsr_seq"] = i;
                                      drCs["bsr_i_flag"] = 0;
                                      i++;
                                      dtCsInsert.Rows.Add(drCs);

                                  }
                              }

                              if (dtCsInsert.Rows.Count > 0)
                              {
                                  List<SqlCommand> listCmdResultoInsert = DBTableHelper.GenerateInsertCommand("cs_rlts", new string[] { "" }, dtCsInsert);

                                  foreach (SqlCommand cmd in listCmdResultoInsert)
                                  {
                                      transHelper.ExecuteNonQuery(cmd);
                                  }
                              }

                              return;
                          }
                      }

                    //获取当前病人结果记录
                    string sqlSelectResulto = string.Format("select * from resulto with(nolock) where res_id = '{0}' and res_flag = 1", pat_id);
                    DataTable dtResulto = transHelper.GetTable(sqlSelectResulto);


                    DataTable dtResultoInsert = dtResulto.Clone();

                  

                    //遍历当前病人的组合
                    foreach (DataRow drCom in dtCombine.Rows)
                    {
                        //组合ID
                        string com_id = drCom["pat_com_id"].ToString();

                        //根据 组合ID和标本 获取具有缺省值的项目
                        DataTable dtItem = DictCombineMi.NewInstance.GetCombineMiWdthDefault(com_id, pat_sam_id, pat_itr_id);

                        foreach (DataRow drItem in dtItem.Rows)
                        {
                            string itm_id = drItem["itm_id"].ToString();

                            if (dtResultoInsert.Select(string.Format("res_itm_id = '{0}' ", itm_id)).Length == 0
                                && dtResulto.Select(string.Format("res_itm_id = '{0}' ", itm_id)).Length == 0
                                )
                            {

                                DataRow drResulto = dtResultoInsert.NewRow();
                                drResulto["res_id"] = pat_id;
                                drResulto["res_itr_id"] = pat_itr_id;
                                drResulto["res_sid"] = pat_sid;
                                drResulto["res_itm_id"] = itm_id;
                                drResulto["res_itm_ecd"] = drItem["itm_ecd"];
                                drResulto["res_chr"] = drItem["itm_defa"];
                                drResulto["res_date"] = dtToday;
                                drResulto["res_flag"] = 1;
                                drResulto["res_type"] = 0;
                                drResulto["res_com_id"] = com_id;
                                drResulto["res_itm_rep_ecd"] = drItem["itm_rep_ecd"];

                                dtResultoInsert.Rows.Add(drResulto);
                            }
                        }
                    }

                    if (dtResultoInsert.Rows.Count > 0)
                    {
                        List<SqlCommand> listCmdResultoInsert = DBTableHelper.GenerateInsertCommand("resulto", new string[] { "res_key" }, dtResultoInsert);

                        foreach (SqlCommand cmd in listCmdResultoInsert)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "InsertDefaultResult", ex.ToString());
                throw;
            }
        }


        public  List<string> GetCombineDefData(string itrid, string comID)
        {

            try
            {
                Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

                string sql = string.Format(@"
select com_id,isnull(itm_defa,'') as def_data,dict_item_sam.itm_itr_id from dict_combine_mi 
left join dict_item on dict_combine_mi.com_itm_id=dict_item.itm_id 
left join dict_item_sam on dict_item_sam.itm_id=dict_item.itm_id
where isnull(itm_defa,'')<>''  and dict_combine_mi.com_id='{1}'
group by com_id,isnull(itm_defa,''),dict_item_sam.itm_itr_id,dict_combine_mi.com_sort 
order by dict_combine_mi.com_sort
", itrid, comID);

                DBHelper helper = new DBHelper();
                DataTable dt = helper.GetTable(sql);
                dt.TableName = "CombineDefData";

                DataRow[] rowAlls = dt.Select(string.Format("itm_itr_id='{0}'", itrid));

                if (rowAlls.Length == 0)
                {
                    rowAlls = dt.Select(string.Format("com_id='{0}'", comID));
                }

                foreach (DataRow item in rowAlls)
                {
                    string defData = item["def_data"].ToString();


                    if (defData.StartsWith("[")
                        && defData.EndsWith("]"))
                    {
                        StringBuilder itemDefData = new StringBuilder();
                        string[] codeList = defData.Replace("[", "").Replace("]", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DataTable desInfo = helper.GetTable(@"select br_id,isnull(br_scripe,'') as br_scripe from dict_bscripe");
                        foreach (string code in codeList)
                        {
                            DataRow[] rows = desInfo.Select(string.Format("br_id='{0}'", code));
                            if (rows.Length > 0 && !string.IsNullOrEmpty(rows[0]["br_scripe"].ToString()))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0]["br_scripe"].ToString());

                            }

                        }
                        item["def_data"] = itemDefData.ToString();
                    }
                    else if (defData.StartsWith("^")
                        && defData.EndsWith("^"))
                    {

                        StringBuilder itemDefData = new StringBuilder();


                        string[] codeList = defData.Replace("^", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DataTable desInfo = helper.GetTable(@"select nob_id,nob_cname from dict_nobact where nob_del<>'1'");
                        foreach (string code in codeList)
                        {
                            DataRow[] rows = desInfo.Select(string.Format("nob_id='{0}'", code));
                            if (rows.Length > 0 && !string.IsNullOrEmpty(rows[0]["nob_cname"].ToString()))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0]["nob_cname"].ToString());

                            }

                        }
                        item["def_data"] = itemDefData.ToString();
                    }
                }


                foreach (DataRow row in rowAlls)
                {
            
                    string defData = row["def_data"].ToString();
             

                    if (!result.ContainsKey(comID + itrid))
                    {
                        result.Add(comID + itrid, new List<string>());
                    }
                    if (defData.Contains("^|"))
                    {
                        string[] defDataList = defData.Split(new string[] { "^|" }, StringSplitOptions.RemoveEmptyEntries);

                        result[comID + itrid].AddRange(defDataList);
                    }
                    else
                    {
                        result[comID + itrid].Add(defData);
                    }

                }

                if (result.ContainsKey(comID + itrid))
                {
                    return result[comID + itrid];
                }
                if (result.ContainsKey(comID + "-1"))
                {
                    return result[comID + "-1"];
                }
                else
                {
                    return new List<string>();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetCombineDefData", ex.ToString());

                return new List<string>();
            }
        }

        /// <summary>
        /// 插入缺省结果
        /// </summary>
        /// <param name="transHelper"></param>
        public void InsertDefaultResultForBf(string pat_id, string pat_sam_id, string pat_itr_id, string pat_sid, DataTable dtCombine, DBHelper transHelper)
        {
            try
            {
                if (dtCombine != null && dtCombine.Rows.Count > 0)
                {
                    //获取当前病人结果记录
                    string sqlSelectResulto = string.Format("select * from resulto_newborn with(nolock) where res_id = '{0}' and res_flag = 1", pat_id);

                    if (transHelper == null)
                    {
                        transHelper = new DBHelper();
                    }

                    DataTable dtResulto = transHelper.GetTable(sqlSelectResulto);


                    DataTable dtResultoInsert = dtResulto.Clone();

                    DateTime dtToday = ServerDateTime.GetDatabaseServerDateTime();

                    //遍历当前病人的组合
                    foreach (DataRow drCom in dtCombine.Rows)
                    {
                        //组合ID
                        string com_id = drCom["pat_com_id"].ToString();

                        //根据 组合ID和标本 获取具有缺省值的项目
                        DataTable dtItem = DictCombineMi.NewInstance.GetCombineMiWdthDefault(com_id, pat_sam_id, pat_itr_id);

                        foreach (DataRow drItem in dtItem.Rows)
                        {
                            string itm_id = drItem["itm_id"].ToString();

                            if (dtResultoInsert.Select(string.Format("res_itm_id = '{0}' ", itm_id)).Length == 0
                                && dtResulto.Select(string.Format("res_itm_id = '{0}' ", itm_id)).Length == 0
                                )
                            {

                                DataRow drResulto = dtResultoInsert.NewRow();
                                drResulto["res_id"] = pat_id;
                                drResulto["res_itr_id"] = pat_itr_id;
                                drResulto["res_sid"] = pat_sid;
                                drResulto["res_itm_id"] = itm_id;
                                drResulto["res_itm_ecd"] = drItem["itm_ecd"];
                                drResulto["res_chr"] = drItem["itm_defa"];
                                drResulto["res_date"] = dtToday;
                                drResulto["res_flag"] = 1;
                                drResulto["res_type"] = 0;
                                drResulto["res_com_id"] = com_id;
                                drResulto["res_itm_rep_ecd"] = drItem["itm_rep_ecd"];

                                dtResultoInsert.Rows.Add(drResulto);
                            }
                        }
                    }

                    if (dtResultoInsert.Rows.Count > 0)
                    {
                        List<SqlCommand> listCmdResultoInsert = DBTableHelper.GenerateInsertCommand("resulto_newborn", new string[] { "res_key" }, dtResultoInsert);

                        foreach (SqlCommand cmd in listCmdResultoInsert)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "InsertDefaultResult", ex.ToString());
                throw;
            }
        }

        public void InsertDefaultResult(string pat_id, string pat_sam_id, string pat_itr_id, string pat_sid, DBHelper transHelper)
        {
            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }
            DataTable dtCombine = transHelper.GetTable(string.Format("select * from patients_mi with(nolock) where pat_id = '{0}'", pat_id));

            InsertDefaultResult(pat_id, pat_sam_id, pat_itr_id, pat_sid, dtCombine, transHelper);
        }


        /// <summary>
        /// 检查样本号是否已存在
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public static bool ExistSID(string pat_sid, string itr_id, DateTime pat_date)
        {
            return ExistSID(pat_sid, itr_id, pat_date, null);
        }

        public static bool ExistSID(string pat_sid, string itr_id, DateTime pat_date, DBHelper transHelper)
        {
            //DBHelper helper = new DBHelper();

            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }

            //检查样本号是否已存在
            string sqlSelect = string.Format(@"
                    select top 1 pat_sid from
                    patients with(nolock)  where pat_sid='{0}' 
                    and pat_itr_id ='{1}' 
                    and pat_date >= '{2}' 
                    and pat_date<'{3}'", pat_sid, itr_id, pat_date.Date.ToString("yyyy-MM-dd HH:mm:ss"), pat_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            //object objSID = transHelper.ExecuteScalar(sqlSelect);

            object objSID = transHelper.ExecuteScalar(sqlSelect);



            if (objSID != null && objSID != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查样本号是否已存在
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public static bool ExistSIDForBabyFilter(string pat_sid, string itr_id, DateTime pat_date)
        {
            return ExistSIDForBabyFilter(pat_sid, itr_id, pat_date, null);
        }

        public static bool ExistSIDForBabyFilter(string pat_sid, string itr_id, DateTime pat_date, DBHelper transHelper)
        {
            //DBHelper helper = new DBHelper();

            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }

            //检查样本号是否已存在
            string sqlSelect = string.Format(@"
                    select top 1 pat_sid from
                    patients_newborn with(nolock)  where pat_sid='{0}' 
                    and pat_itr_id ='{1}' 
                    and pat_date >= '{2}' 
                    and pat_date<'{3}'", pat_sid, itr_id, pat_date.Date.ToString("yyyy-MM-dd HH:mm:ss"), pat_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            //object objSID = transHelper.ExecuteScalar(sqlSelect);

            object objSID = transHelper.ExecuteScalar(sqlSelect);



            if (objSID != null && objSID != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查序号是否已存在
        /// </summary>
        /// <param name="pat_host_order"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public static bool ExistHostOrder(int pat_host_order, string itr_id, DateTime pat_date)
        {
            return ExistHostOrder(pat_host_order, itr_id, pat_date, null);
        }

        public static bool ExistHostOrder(int pat_host_order, string itr_id, DateTime pat_date, DBHelper transHelper)
        {
            //DBHelper helper = new DBHelper();
            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }

            //检查序号是否已存在
            string sqlSelect = string.Format(@"
                    select top 1 pat_host_order from 
                    patients with(nolock)   where pat_host_order={0}
                    and pat_itr_id ='{1}' 
                    and pat_date >= '{2}' 
                    and pat_date<'{3}'", pat_host_order, itr_id, pat_date.Date.ToString("yyyy-MM-dd HH:mm:ss"), pat_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            //object objHostOrder = transHelper.ExecuteScalar(sqlSelect);

            object objHostOrder = transHelper.ExecuteScalar(sqlSelect);

            if (objHostOrder != null && objHostOrder != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查序号是否已存在
        /// </summary>
        /// <param name="pat_host_order"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public static bool ExistHostOrderForBabyFilter(int pat_host_order, string itr_id, DateTime pat_date)
        {
            return ExistHostOrderForBabyFilter(pat_host_order, itr_id, pat_date, null);
        }

        public static bool ExistHostOrderForBabyFilter(int pat_host_order, string itr_id, DateTime pat_date, DBHelper transHelper)
        {
            //DBHelper helper = new DBHelper();
            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }

            //检查序号是否已存在
            string sqlSelect = string.Format(@"
                    select top 1 pat_host_order from 
                    patients_newborn  where pat_host_order={0}
                    and pat_itr_id ='{1}' 
                    and pat_date >= '{2}' 
                    and pat_date<'{3}'", pat_host_order, itr_id, pat_date.Date.ToString("yyyy-MM-dd HH:mm:ss"), pat_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            //object objHostOrder = transHelper.ExecuteScalar(sqlSelect);

            object objHostOrder = transHelper.ExecuteScalar(sqlSelect);

            if (objHostOrder != null && objHostOrder != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 检查样本号是否已存在
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public static bool ExistNewSIDForCxh(string pat_upid, string itr_id, DateTime pat_date)
        {
            return ExistNewSIDForCxh(pat_upid, itr_id, pat_date, null);
        }

        public static bool ExistNewSIDForCxh(string pat_upid, string itr_id, DateTime pat_date, DBHelper transHelper)
        {
            //DBHelper helper = new DBHelper();

            if (transHelper == null)
            {
                transHelper = new DBHelper();
            }

            //检查样本号是否已存在
            string sqlSelect = string.Format(@"
                    select top 1 pat_upid from
                    patients with(nolock)  where pat_upid='{0}' 
                    and pat_itr_id ='{1}' 
                    and pat_date >= '{2}' 
                    and pat_date<'{3}'", pat_upid, itr_id, pat_date.Date.ToString("yyyy-MM-dd HH:mm:ss"), pat_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            //object objSID = transHelper.ExecuteScalar(sqlSelect);

            object objSID = transHelper.ExecuteScalar(sqlSelect);



            if (objSID != null && objSID != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 生成病人ID
        /// </summary>
        /// <param name="pat_date"></param>
        /// <param name="pat_itr_id"></param>
        /// <param name="pat_sid"></param>
        /// <returns></returns>
        public static string CreatePatID(DateTime pat_date, string pat_itr_id, string pat_sid)
        {
            //string pat_id = drPatient["pat_itr_id"].ToString() + pat_date.ToString("yyyyMMdd") + drPatient["pat_sid"].ToString();

            string pat_id = pat_itr_id + pat_date.ToString("yyyyMMdd") + pat_sid;
            return pat_id;
        }


        /// <summary>
        /// 获取病人资料状态
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetPatientState(string pat_id)
        {
            //pat_flag
            string sqlSelect = string.Format("select top 1 (CASE WHEN ISNULL(pat_pre_flag,0)<>0 AND ISNULL(pat_flag,0)=0 THEN 1 ELSE pat_flag end  ) as pat_flag,pat_bar_code,pat_pre_flag from patients with(nolock) where pat_id = '{0}' ", pat_id);

            DBHelper helper = new DBHelper();
            DataTable dtPatient = helper.GetTable(sqlSelect);
            //if (objValue != null && objValue != DBNull.Value)
            //{
            //    return objValue.ToString();
            //}
            //else
            //{
            //    return string.Empty;
            //}
            return dtPatient;
        }

        /// <summary>
        /// 获取病人资料状态(多个)
        /// </summary>
        /// <param name="InPat_ids">IN的pat_id</param>
        /// <returns></returns>
        public DataTable GetPatientStateForIn(string InPat_ids)
        {
            //pat_flag
            string sqlSelect = string.Format("select pat_flag,pat_bar_code,pat_id from patients with(nolock) where pat_id in({0})", InPat_ids);

            DBHelper helper = new DBHelper();
            DataTable dtPatient = helper.GetTable(sqlSelect);

            return dtPatient;
        }

        /// <summary>
        /// 获取病人资料状态
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetPatientStateForBabyFilter(string pat_id)
        {
            //pat_flag
            string sqlSelect = string.Format("select top 1 pat_flag,pat_bar_code from patients_newborn with(nolock) where pat_id = '{0}'", pat_id);

            DBHelper helper = new DBHelper();
            DataTable dtPatient = helper.GetTable(sqlSelect);
            //if (objValue != null && objValue != DBNull.Value)
            //{
            //    return objValue.ToString();
            //}
            //else
            //{
            //    return string.Empty;
            //}
            return dtPatient;
        }
        /// <summary>
        /// 更新条码状态并插入条码操作日志
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="barcode"></param>
        /// <param name="remark"></param>
        /// <param name="transHelper"></param>
        public void UpdateBarcodeStatus(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday, DBHelper transHelper)
        {
            DBHelper helper = new DBHelper();

            List<SqlCommand> list = GetUpdateBarcodeStatusCommand(caller, statusCode, barcode, remark, dtToday);
            foreach (SqlCommand cmd in list)
            {
                //transHelper.ExecuteNonQuery(cmd);
                helper.ExecuteNonQuery(cmd);
            }

           

            //            if (string.IsNullOrEmpty(barcode))
            //            {
            //                return;
            //            }

            //            if (dtToday == null)
            //            {
            //                dtToday = DateTime.Now;
            //            }

            //            string sqlSelectBarcodeInfo = string.Format("select bc_return_times,bc_bar_no from bc_patients where bc_bar_code = '{0}'", barcode);
            //            DataTable dtBarcode = new DBHelper().GetTable(sqlSelectBarcodeInfo);

            //            if (dtBarcode.Rows.Count == 0)
            //            {
            //                return;
            //            }

            //            //更新bc_patients表标志
            //            string sqlUpdate = string.Format(@"
            //update bc_patients
            //set bc_status = @bc_status,bc_lastaction_time = @bc_lastaction_time
            //where bc_bar_code = @bc_bar_code");
            //            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate);
            //            cmdUpdate.Parameters.AddWithValue("bc_status", statusCode);
            //            cmdUpdate.Parameters.AddWithValue("bc_bar_code", barcode);
            //            cmdUpdate.Parameters.AddWithValue("bc_lastaction_time", dtToday.Value);
            //            transHelper.ExecuteNonQuery(cmdUpdate);

            //            //插入条码日志记录
            //            string sqlInsert = string.Format(@"
            //insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_flow,bc_remark) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_flow,@bc_remark)");

            //            SqlCommand cmdInsert = new SqlCommand(sqlInsert);
            //            cmdInsert.Parameters.AddWithValue("bc_date", dtToday.Value);
            //            cmdInsert.Parameters.AddWithValue("bc_login_id", caller.LoginID == null ? string.Empty : caller.LoginID);
            //            cmdInsert.Parameters.AddWithValue("bc_name", caller.LoginName == null ? string.Empty : caller.LoginName);
            //            cmdInsert.Parameters.AddWithValue("bc_status", statusCode);
            //            cmdInsert.Parameters.AddWithValue("bc_bar_no", dtBarcode.Rows[0]["bc_bar_no"]);
            //            cmdInsert.Parameters.AddWithValue("bc_bar_code", barcode);
            //            cmdInsert.Parameters.AddWithValue("bc_place", caller.Location == null ? string.Empty : caller.Location);

            //            int currentFlow = 1;
            //            if (!Compare.IsEmpty(dtBarcode.Rows[0]["bc_return_times"]))
            //            {
            //                currentFlow = Convert.ToInt32(dtBarcode.Rows[0]["bc_return_times"]) + 1;
            //            }

            //            cmdInsert.Parameters.AddWithValue("bc_flow", currentFlow);
            //            cmdInsert.Parameters.AddWithValue("bc_remark", remark);

            //            transHelper.ExecuteNonQuery(cmdInsert);
        }

        /// <summary>
        /// 获取更新条码状态并插入条码操作日志的sqlcommand
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="statusCode"></param>
        /// <param name="barcode"></param>
        /// <param name="remark"></param>
        /// <param name="dtToday"></param>
        /// <returns></returns>
        public List<SqlCommand> GetUpdateBarcodeStatusCommand(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday)
        {
            List<SqlCommand> list = new List<SqlCommand>();

            if (string.IsNullOrEmpty(barcode))
            {
                return list;
            }

            if (dtToday == null)
            {
                dtToday = ServerDateTime.GetDatabaseServerDateTime();
            }

            string sqlSelectBarcodeInfo = string.Format("select bc_return_times,bc_bar_no from bc_patients where bc_bar_code = '{0}'", barcode);
            DataTable dtBarcode = new DBHelper().GetTable(sqlSelectBarcodeInfo);

            if (dtBarcode.Rows.Count == 0)
            {
                return list;
            }

            //资料登记的时候要更新 bc_patients.bc_receiver_flag = 1
            string subUpdate = string.Empty;
            if (statusCode == EnumBarcodeOperationCode.SampleRegister.ToString())
            {
                subUpdate = ",bc_patients.bc_receiver_flag = 1";
            }

            //更新bc_patients表标志
            string sqlUpdate = string.Format(@"
update bc_patients
set
bc_status = @bc_status,
bc_lastaction_time = @bc_lastaction_time
{0}
where bc_bar_code = @bc_bar_code", subUpdate);
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate);
            cmdUpdate.Parameters.AddWithValue("bc_status", statusCode);
            SqlParameter pBarcode = cmdUpdate.Parameters.AddWithValue("bc_bar_code", barcode);
            pBarcode.DbType = DbType.AnsiString;

            cmdUpdate.Parameters.AddWithValue("bc_lastaction_time", dtToday.Value);

            list.Add(cmdUpdate);
            //transHelper.ExecuteNonQuery(cmdUpdate);

            //插入条码日志记录
            string sqlInsert = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_flow,bc_remark) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_flow,@bc_remark)");

            SqlCommand cmdInsert = new SqlCommand(sqlInsert);
            cmdInsert.Parameters.AddWithValue("bc_date", dtToday.Value);
            cmdInsert.Parameters.AddWithValue("bc_login_id", caller.LoginID == null ? string.Empty : caller.LoginID);
            cmdInsert.Parameters.AddWithValue("bc_name", caller.LoginName == null ? string.Empty : caller.LoginName);
            cmdInsert.Parameters.AddWithValue("bc_status", statusCode);
            cmdInsert.Parameters.AddWithValue("bc_bar_no", dtBarcode.Rows[0]["bc_bar_no"]);
            cmdInsert.Parameters.AddWithValue("bc_bar_code", barcode);
            cmdInsert.Parameters.AddWithValue("bc_place", caller.Location == null ? string.Empty : caller.Location);

            int currentFlow = 1;
            if (!Compare.IsEmpty(dtBarcode.Rows[0]["bc_return_times"]))
            {
                currentFlow = Convert.ToInt32(dtBarcode.Rows[0]["bc_return_times"]) + 1;
            }

            cmdInsert.Parameters.AddWithValue("bc_flow", currentFlow);
            cmdInsert.Parameters.AddWithValue("bc_remark", remark);

            list.Add(cmdInsert);
            //transHelper.ExecuteNonQuery(cmdInsert);

            return list;
        }

        /// <summary>
        /// 根据配置转换获取年龄分钟(当年龄为空值时)
        /// </summary>
        /// <param name="ageInput">输入(分钟)</param>
        /// <returns></returns>
        public static int GetConfigAge(object ageMinuteInput)
        {
            if (!Compare.IsEmpty(ageMinuteInput))
            {
                int ret = -1;

                if (int.TryParse(ageMinuteInput.ToString(), out ret))
                {
                    if (ret >= 0)
                    {
                        return ret;
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            string configCalAge = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetRefOnNullAge");

            int calage = -1;

            if (!string.IsNullOrEmpty(configCalAge)
                && configCalAge != "不计算参考值")
            {
                calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
            }
            return calage;
        }

        public static string GetConfigSex(object sexInput)
        {
            string ret = string.Empty;
            if (Compare.IsEmpty(sexInput) || (sexInput.ToString() != "1" && sexInput.ToString() != "2" && sexInput.ToString() != "0"))//年龄为空
            {
                string configCalSex = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetRefOnNullSex");

                if (configCalSex == "不计算参考值")
                {
                    ret = "9";
                }
                else if (configCalSex == "默认")
                {
                    ret = "0";
                }
                else if (configCalSex == "男")
                {
                    ret = "1";
                }
                else if (configCalSex == "女")
                {
                    ret = "2";
                }
            }
            else
            {
                ret = sexInput.ToString();
            }

            return ret;
        }

        /***************************************************************************************/
        /**************************************************************************************/
        //此处做了重载方法，以达到不动其他的设置,保证程序的稳定运行

        public List<SqlCommand> GetUpdateBarcodeStatusCommand(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday, string pat_id)
        {
            List<SqlCommand> list = new List<SqlCommand>();

            if (dtToday == null)
            {
                dtToday = ServerDateTime.GetDatabaseServerDateTime();
            }
            SqlParameter parVarchar;
            //当没有条码时，则不更新bc_patients表，只更新bc_sign表
            if (string.IsNullOrEmpty(barcode))
            {
                string sqlInsert1 = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_remark,pat_id) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_remark,@pat_id)");

                SqlCommand cmdInsert1 = new SqlCommand(sqlInsert1);
                SqlParameter parDate = cmdInsert1.Parameters.AddWithValue("bc_date", dtToday.Value);
                parDate.DbType = DbType.DateTime;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_login_id", caller.LoginID == null ? string.Empty : caller.LoginID);
                parVarchar.DbType = DbType.AnsiString;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_name", caller.LoginName == null ? string.Empty : caller.LoginName);
                parVarchar.DbType = DbType.AnsiString;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_status", statusCode);
                parVarchar.DbType = DbType.AnsiString;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_bar_no", string.Empty);
                parVarchar.DbType = DbType.AnsiString;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_bar_code", string.Empty);
                parVarchar.DbType = DbType.AnsiString;
                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_place", caller.Location == null ? string.Empty : caller.Location);
                parVarchar.DbType = DbType.AnsiString;

                parVarchar = cmdInsert1.Parameters.AddWithValue("bc_remark", remark ?? "");
                parVarchar.DbType = DbType.AnsiString;
                //增加pat_id号======================================

                parVarchar = cmdInsert1.Parameters.AddWithValue("pat_id", pat_id);
                parVarchar.DbType = DbType.AnsiString;
                //==================================================

                list.Add(cmdInsert1);
                return list;
            }



            string sqlSelectBarcodeInfo = string.Format("select bc_return_times,bc_bar_no from bc_patients WITH(NOLOCK) where bc_bar_code = '{0}'", barcode);
            DataTable dtBarcode = new DBHelper().GetTable(sqlSelectBarcodeInfo);

            if (dtBarcode.Rows.Count == 0)
            {
                return list;
            }

            //更新bc_patients表标志
            string sqlUpdate = string.Format(@"
update bc_patients
set bc_status = @bc_status,bc_lastaction_time = @bc_lastaction_time
where bc_bar_code = @bc_bar_code");
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate);
            cmdUpdate.Parameters.AddWithValue("bc_status", statusCode);
            cmdUpdate.Parameters.AddWithValue("bc_bar_code", barcode);
            SqlParameter parlastaction = cmdUpdate.Parameters.AddWithValue("bc_lastaction_time", dtToday.Value);
            parlastaction.DbType = DbType.DateTime;
            list.Add(cmdUpdate);
            //transHelper.ExecuteNonQuery(cmdUpdate);

            //插入条码日志记录
            string sqlInsert = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_flow,bc_remark,pat_id) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_flow,@bc_remark,@pat_id)");

            SqlCommand cmdInsert = new SqlCommand(sqlInsert);
            SqlParameter parDate2 = cmdInsert.Parameters.AddWithValue("bc_date", dtToday.Value);
            parDate2.DbType = DbType.DateTime;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_login_id", caller.LoginID == null ? string.Empty : caller.LoginID);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_name", caller.LoginName == null ? string.Empty : caller.LoginName);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_status", statusCode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_no", dtBarcode.Rows[0]["bc_bar_no"]);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_code", barcode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_place", caller.Location == null ? string.Empty : caller.Location);
            parVarchar.DbType = DbType.AnsiString;
            //增加pat_id号======================================

            parVarchar = cmdInsert.Parameters.AddWithValue("pat_id", pat_id);
            parVarchar.DbType = DbType.AnsiString;
            //==================================================

            int currentFlow = 1;
            if (!Compare.IsEmpty(dtBarcode.Rows[0]["bc_return_times"]))
            {
                currentFlow = Convert.ToInt32(dtBarcode.Rows[0]["bc_return_times"]) + 1;
            }

            SqlParameter parInt = cmdInsert.Parameters.AddWithValue("bc_flow", currentFlow);
            parInt.DbType = DbType.Int32;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_remark", remark ?? "");
            parVarchar.DbType = DbType.AnsiString;
            list.Add(cmdInsert);
            //transHelper.ExecuteNonQuery(cmdInsert);

            return list;
        }

        public List<SqlCommand> GetUpdateBarcodeStatusCommandForSZ(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday, string pat_id)
        {
            List<SqlCommand> list = new List<SqlCommand>();

            if (dtToday == null)
            {
                dtToday = ServerDateTime.GetDatabaseServerDateTime();
            }
            SqlParameter parVarchar;


            //更新bc_patients表标志
            string sqlUpdate = string.Format(@"
update bc_patients
set bc_status = @bc_status,bc_lastaction_time = @bc_lastaction_time,bc_receiver_date=@bc_receiver_date,
bc_receiver_code=@bc_receiver_code,bc_receiver_name=@bc_receiver_name,bc_receiver_flag=1
where bc_bar_code = @bc_bar_code");
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate);
            cmdUpdate.Parameters.AddWithValue("bc_status", statusCode);
            cmdUpdate.Parameters.AddWithValue("bc_bar_code", barcode);
            SqlParameter parlastaction = cmdUpdate.Parameters.AddWithValue("bc_lastaction_time", dtToday.Value);
            parlastaction.DbType = DbType.DateTime;
            SqlParameter parlasreceive = cmdUpdate.Parameters.AddWithValue("bc_receiver_date", dtToday.Value);
            parlasreceive.DbType = DbType.DateTime;
            cmdUpdate.Parameters.AddWithValue("bc_receiver_code", caller.LoginID ?? string.Empty);
            cmdUpdate.Parameters.AddWithValue("bc_receiver_name", caller.LoginName ?? string.Empty);
            list.Add(cmdUpdate);
            //transHelper.ExecuteNonQuery(cmdUpdate);

            //插入条码日志记录
            string sqlInsert = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_flow,bc_remark,pat_id) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_flow,@bc_remark,@pat_id)");

            SqlCommand cmdInsert = new SqlCommand(sqlInsert);
            SqlParameter parDate2 = cmdInsert.Parameters.AddWithValue("bc_date", dtToday.Value);
            parDate2.DbType = DbType.DateTime;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_login_id", caller.LoginID ?? string.Empty);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_name", caller.LoginName ?? string.Empty);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_status", "5");
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_no", barcode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_code", barcode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_place", caller.Location ?? string.Empty);
            parVarchar.DbType = DbType.AnsiString;
            //增加pat_id号======================================

            parVarchar = cmdInsert.Parameters.AddWithValue("pat_id", pat_id);
            parVarchar.DbType = DbType.AnsiString;
            //==================================================

            int currentFlow = 1;
            //if (!Compare.IsEmpty(dtBarcode.Rows[0]["bc_return_times"]))
            //{
            //    currentFlow = Convert.ToInt32(dtBarcode.Rows[0]["bc_return_times"]) + 1;
            //}

            SqlParameter parInt = cmdInsert.Parameters.AddWithValue("bc_flow", currentFlow);
            parInt.DbType = DbType.Int32;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_remark", "签收并同步登记");
            parVarchar.DbType = DbType.AnsiString;
            list.Add(cmdInsert);


            sqlInsert = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,bc_flow,bc_remark,pat_id) values(@bc_date,@bc_login_id,@bc_name,@bc_status,@bc_bar_no,@bc_bar_code,@bc_place,@bc_flow,@bc_remark,@pat_id)");

            cmdInsert = new SqlCommand(sqlInsert);
            parDate2 = cmdInsert.Parameters.AddWithValue("bc_date", dtToday.Value);
            parDate2.DbType = DbType.DateTime;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_login_id", caller.LoginID == null ? string.Empty : caller.LoginID);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_name", caller.LoginName == null ? string.Empty : caller.LoginName);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_status", statusCode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_no", barcode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_bar_code", barcode);
            parVarchar.DbType = DbType.AnsiString;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_place", caller.Location == null ? string.Empty : caller.Location);
            parVarchar.DbType = DbType.AnsiString;
            //增加pat_id号======================================

            parVarchar = cmdInsert.Parameters.AddWithValue("pat_id", pat_id);
            parVarchar.DbType = DbType.AnsiString;
            //==================================================


            //if (!Compare.IsEmpty(dtBarcode.Rows[0]["bc_return_times"]))
            //{
            //    currentFlow = Convert.ToInt32(dtBarcode.Rows[0]["bc_return_times"]) + 1;
            //}

            parInt = cmdInsert.Parameters.AddWithValue("bc_flow", currentFlow);
            parInt.DbType = DbType.Int32;
            parVarchar = cmdInsert.Parameters.AddWithValue("bc_remark", remark ?? "");
            parVarchar.DbType = DbType.AnsiString;
            list.Add(cmdInsert);

            //transHelper.ExecuteNonQuery(cmdInsert);

            return list;
        }

        public void UpdateBarcodeStatus(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday, DBHelper transHelper, string pat_id)
        {
            DBHelper helper = new DBHelper();

            //List<SqlCommand> list = GetUpdateBarcodeStatusCommand(caller, statusCode, barcode, remark, dtToday);
            List<SqlCommand> list = GetUpdateBarcodeStatusCommand(caller, statusCode, barcode, remark, dtToday, pat_id);

            foreach (SqlCommand cmd in list)
            {
                //transHelper.ExecuteNonQuery(cmd);
                helper.ExecuteNonQuery(cmd);
            }
            try
            {
                if (dtToday == null)
                {
                    dtToday = ServerDateTime.GetDatabaseServerDateTime();
                }
                //new TATRecordHelper().Record(barcode, "20", "", dtToday.ToString());
            }
            catch
            { }

        }

        public void UpdateBarcodeStatusForSZ(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string remark, DateTime? dtToday, DBHelper transHelper, string pat_id)
        {
            DBHelper helper = new DBHelper();


            //List<SqlCommand> list = GetUpdateBarcodeStatusCommand(caller, statusCode, barcode, remark, dtToday);
            List<SqlCommand> list = GetUpdateBarcodeStatusCommandForSZ(caller, statusCode, barcode, remark, dtToday, pat_id);

            foreach (SqlCommand cmd in list)
            {
                //transHelper.ExecuteNonQuery(cmd);
                helper.ExecuteNonQuery(cmd);
            }
        }

        /***********************************************************************************************/
        /***********************************************************************************************/

        //将新增的检验报告或者修改检验报告记录登记到bc_sign表中记录
        public void insertCheckReportToBcSign(EntityRemoteCallClientInfo caller, string statusCode, string barcode, string pat_id)
        {
            //DateTime datetime = DateTime.Now;
            string bc_login_id = caller.LoginID == null ? string.Empty : caller.LoginID;
            string bc_name = caller.LoginName == null ? string.Empty : caller.LoginName;
            string bc_status = statusCode;
            string bc_bar_no = barcode == null ? null : barcode;
            string bc_place = caller.Location == null ? string.Empty : caller.Location;

            //插入条码日志记录
            string sqlInsert = string.Format(@"
insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_place,pat_id) 
values(getdate(),'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", "", bc_login_id, bc_name, bc_status, bc_bar_no, bc_bar_no, bc_place, pat_id);


            new DBHelper().ExecuteNonQuery(sqlInsert);
        }

        public DataTable getPatients(string pat_id)
        {
            string sql = string.Format(@"select dict_combine.com_name,patients.pat_id,
patients.pat_name,patients.pat_in_no,dict_instrmt.itr_name,patients.pat_sid from patients
left join patients_mi on patients.pat_id = patients_mi.pat_id
left join dict_combine on dict_combine.com_id = patients_mi.pat_com_id
left join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
where patients.pat_id = '{0}'", pat_id);

            return new DBHelper().GetTable(sql);
        }

        /// <summary>
        /// 根据病人姓名或者ID查找已删除的操作记录
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string getPatIdByPatName(string sql)
        {
            DataTable result = new DataTable("pat_id");
            result = new DBHelper().GetTable(sql);

            if (result != null && result.Rows.Count > 0)
            {
                return result.Rows[0]["pat_id"].ToString();
            }
            else
                return string.Empty;
        }


        /// <summary>
        /// 根据用户ID获取用户功能权限
        /// </summary>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        public DataTable doSearchFuncByLogin(string LoginId)
        {
            string sql = string.Format(@"
select poweruserrole.roleInfoId,powerrolefunc.funcInfoId,[powerfuncinfo].funcName,
[powerfuncinfo].funcCode,poweruserInfo.loginId,poweruserInfo.userName
from poweruserInfo
left join poweruserrole on poweruserrole.userInfoId = poweruserInfo.userInfoId
left join powerrolefunc on powerrolefunc.roleInfoId = poweruserrole.roleInfoId
left join powerfuncinfo on [powerfuncinfo].funcInfoId = powerrolefunc.funcInfoId
where loginId =  '{0}'", LoginId);

            DataTable result = new DBHelper().GetTable(sql);
            result.TableName = "userFunction";
            return result;
        }

        /// <summary>
        /// 判断条码号是否已回退
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public bool Returned(string barCode)
        {
            string sql = string.Format(@"select bc_status from bc_patients where bc_bar_no = '{0}'", barCode);

            DataTable dt = new DBHelper().GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["bc_status"].ToString() == "9")
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// 将结果列顺序保存到系统配置中
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public bool saveColumnSort(string sort)
        {
            string flag = "select configid from sysconfig where configcode='PatResultColumnSort'";

            try
            {
                DataTable dt = new DBHelper().GetTable(flag);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string sql = string.Format(@"
update sysconfig set configItemValue = '{0}' where configcode='PatResultColumnSort' 
", sort);

                    new DBHelper().ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = string.Format(@"
if not exists(select configid from sysconfig where configcode='PatResultColumnSort')
insert into sysconfig values ('PatResultColumnSort','检验报告管理','检验报告管理中结果列顺序','字符串','{0}','','system')
", sort);
                    new DBHelper().ExecuteNonQuery(sql);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteException("新增/修改系统配置出错", "dcl.svr.result.CRUD.PatCommonBll.saveColumnSort", ex.Message);
                return false;
            }
        }

        public bool savePatFunctionSet(string sort, string Visible)
        {
            string flag = "select configid from sysconfig where configcode='{0}'";

            string strUpdate = "update sysconfig set configItemValue = '{0}' where configcode='{1}' ";

            string strInsert = @"if not exists(select configid from sysconfig where configcode='{0}')
                                 insert into sysconfig (configCode, configGroup, configItemName, configItemType, configItemValue,  configType)
                                 values ('{0}','检验报告管理','{1}','字符串','{2}','system')";

            try
            {
                DBHelper helper = new DBHelper();

                DataTable dtSort = helper.GetTable(string.Format(flag, "PatFunctionSort"));

                if (dtSort != null && dtSort.Rows.Count > 0)
                    helper.ExecuteNonQuery(string.Format(strUpdate, sort, "PatFunctionSort"));
                else
                    helper.ExecuteNonQuery(string.Format(strInsert, "PatFunctionSort", "检验报告管理中右列功能顺序", sort));


                DataTable dtVisible = helper.GetTable(string.Format(flag, "PatFunctionVisible"));

                if (dtVisible != null && dtVisible.Rows.Count > 0)
                    helper.ExecuteNonQuery(string.Format(strUpdate, Visible, "PatFunctionVisible"));
                else
                    helper.ExecuteNonQuery(string.Format(strInsert, "PatFunctionVisible", "检验报告管理中右列功能可见", Visible));

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
