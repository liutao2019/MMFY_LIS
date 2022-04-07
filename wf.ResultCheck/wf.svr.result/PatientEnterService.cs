using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using dcl.root.dac;
using System.Data;
using dcl.root.logon;
//using lis.dto;
using dcl.svr.sample;
using System.Diagnostics;
using lis.dto.Entity;
using dcl.common;
using dcl.servececontract;
using dcl.svr.framedic;
using dcl.svr.result.CRUD;
using dcl.svr.cache;

using dcl.pub.entities;

using dcl.svr.resultcheck;
using lis.dto;
using System.Text.RegularExpressions;
using Lib.DAC;
using Lib.EntityDAC;
using lis.dto.BarCodeEntity;
using dcl.servececontract;
using System.Collections;
using Lis.SZRYHis.Interface;
using System.IO;
using System.Xml;
using dcl.entity;

namespace dcl.svr.result
{
    public class PatientEnterService : IPatientEnterBLL
    {
        #region IPatientEnter 成员

        /// <summary>
        /// 获取病人相关结果
        /// </summary>
        /// <param name="pat_no_id">id</param>
        /// <param name="pat_in_no">id类型</param>
        /// <param name="dtReportDate">报告日期</param>
        /// <returns></returns>
        public DataSet GetPatRelateResult(string pat_id, string pat_no_id, string pat_in_no, DateTime dtSendDate)
        {
            DBHelper helper = new DBHelper();

            //厦门 需配置
            //获取本次检验以外,id类型和id相同的病人
            string sqlSelect = string.Format(@"select pat_id from patients 
                                                        where pat_no_id = '{0}' 
                                                                and pat_in_no='{1}' 
                                                                and pat_id<>'{2}' 
                                                                and pat_flag<>'{3}'
                                                                and pat_date >= DATEADD(DAY,-7,pat_date)"
                //and pat_sdate >= @date1 and pat_sdate < @date2"
                                                , pat_no_id
                                                , pat_in_no
                                                , pat_id
                                                , LIS_Const.PATIENT_FLAG.Natural
                                                );

            //SqlCommand cmd = new SqlCommand(sqlSelect);
            //cmd.Parameters.AddWithValue("date1", dtSendDate.Date);
            //cmd.Parameters.AddWithValue("date2", dtSendDate.AddDays(1).Date);

            DataTable dtPatIDList = helper.GetTable(sqlSelect);//返回所有符合条件的pat_id
            //SqlCommand cmd = new SqlCommand(sqlSelect);
            //cmd.Parameters.AddWithValue("date1", dtSendDate.Date);
            //cmd.Parameters.AddWithValue("date2", dtSendDate.AddDays(1).Date);

            //DataTable dtPatIDList = helper.GetTable(cmd);//返回所有符合条件的pat_id

            StringBuilder sbPatComIN = new StringBuilder();
            sbPatComIN.Append("'-1'");

            //查找对应的组合
            foreach (DataRow drPat in dtPatIDList.Rows)
            {
                string patID = drPat["pat_id"].ToString();
                sbPatComIN.Append(string.Format(",'{0}'", patID));
            }

            sqlSelect = string.Format(@"
        select dict_combine.com_name, patients_mi.pat_com_id,res_id,res_itm_ecd,res_chr ,res_date,
        res_com_id,dict_item.itm_seq,resulto.res_sid,
        res_itm_seq = dict_combine_mi.com_sort,
        res_itm_seq = dict_combine_mi.com_sort,
        res_com_seq = isnull(dict_combine.com_seq,isnull(patients_mi.pat_seq,99999)),
        resulto.res_com_id,isnull(dict_item.itm_seq,0) as itm_seq,
        rtrim(isnull(resulto.res_ref_l,''))+(case when  isnull(resulto.res_ref_l,'') = '' or isnull(resulto.res_ref_h,'') = '' then '' else '--' end)+Rtrim(isnull(resulto.res_ref_h,'')) res_ref_range,
        dict_res_ref_flag.value res_ref_flag
        from resulto
        left join patients_mi on patients_mi.pat_id = resulto.res_id
        left join dict_combine on dict_combine.com_id = patients_mi.pat_com_id
        left join dict_item on dict_item.itm_id = resulto.res_itm_id
        left join dict_combine_mi on  patients_mi.pat_com_id = dict_combine_mi.com_id and resulto.res_itm_id = dict_combine_mi.com_itm_id
        left join dict_res_ref_flag on dict_res_ref_flag.id=resulto.res_ref_flag
        where resulto.res_id is not null
        and resulto.res_id in ({0})", sbPatComIN.ToString());

            DataTable dtRelateResult = helper.GetTable(sqlSelect);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtRelateResult);
            return ds;

        }

        /// <summary>
        /// 获取病人列表(简要信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom">起始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">物理组ID</param>
        /// <param name="itr_id">仪器ID</param>
        /// <returns></returns>
        public DataSet GetPatientsList_Resume(EntityRemoteCallClientInfo caller, DateTime dtFrom, DateTime dtTo, string type_id, string itr_id, bool allowEmptyType, bool allowEmptyItr)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string sqlItrID_IN = string.Empty;

            string sqlSelect;

            DBHelper helper = new DBHelper();
            //没有输入仪器，则查找物理组别中的所有仪器
            if (
                (itr_id == string.Empty || itr_id == null)
                && allowEmptyItr
                )
            {
                sqlItrID_IN = "'-1'";

                sqlSelect = string.Format("select itr_id from dict_instrmt where itr_type='{0}'", type_id);
                DataTable dtItrID_List = helper.GetTable(sqlSelect);

                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dtItrID_List.Rows)
                {
                    sb.Append(string.Format(",'{0}'", dr["itr_id"]));
                }

                sqlItrID_IN = sqlItrID_IN + sb.ToString();
            }
            else
            {
                sqlItrID_IN = string.Format("'{0}'", itr_id);
            }

            string auditWord = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
            if (auditWord == string.Empty)
            {
                auditWord = "审核";
            }

            string reportWord = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
            if (reportWord == string.Empty)
            {
                reportWord = "报告";
            }

            string branchHospital_Search = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BranchHospital_Search");
            if (branchHospital_Search == string.Empty)
            {
                branchHospital_Search = "关";
            }
            /* 2013-09-06 李进 金域接口 第三方检验的报告人，审核人的编号在lis数据库不存在，
             * 因此直接显示第三方的审核人，报告人的姓名.
             * 注释掉下面的语句,对应的为修改后语句。
             --user1.username as pat_check_name,
             --user2.username as pat_report_name,
             --userRec.username as pat_i_name,
             */

            string patientsTableName = "";
            string resultTableNmae = "resulto";

            if (caller != null && caller.BabyFilterFlag)
            {
                patientsTableName = "patients_newborn";
                resultTableNmae = "resulto_newborn";
            }


            #region 旧代码，处理保密的报告单
            //            sqlSelect = string.Format(@"
            //SELECT 
            //    --distinct 
            //    pat_sid,
            //    cast(0 as bit) as pat_select, 
            //    cast(pat_sid as bigint) as pat_sid_int,
            //    pat_sex,
            //    pat_sex_name= case when pat_sex = '1' then '男'
            //                       when pat_sex = '2' then '女'
            //                       else '' end,
            //    pat_date,
            //    pat_name,
            //    pat_age_exp,
            //    pat_bed_no,
            //    pat_id,
            //    pat_itr_id,
            //    dict_sample.sam_name as pat_sam_name,
            //    dict_instrmt.itr_mid,
            //    dict_instrmt.itr_rep_flag,
            //    pat_flag,
            //    pat_flag_name = case when pat_flag = 0 then '未{2}'
            //                         when pat_flag = 1 then '已{2}'
            //                         when pat_flag = 2 then '已{3}'
            //                         when pat_flag = 4 then '已打印'
            //                         else '未{2}' end,
            //    pat_c_name,
            //    pat_bar_code,
            //    pat_host_order,
            //    (case patients.pat_host_order when '' then null when null then null else cast(pat_host_order as bigint) end) pat_host_order_int,
            //    pat_ctype,
            //    pat_ctype_name = case when pat_ctype = '1' then '常规'
            //                          when pat_ctype = '2' then '急查'
            //                          when pat_ctype = '3' then '保密'
            //                          else '' end,
            //    pat_urgent_flag,
            //    pat_in_no,
            //    pat_ori_id,
            //    pat_no_id,
            //    dict_no_type.no_name as  pat_no_id_name,
            //    pat_jy_date,
            //    pat_chk_date,
            //    pat_doc_id,
            //    doc1.doc_name as pat_doc_name,
            //    pat_sdate,
            //    pat_dep_name,
            //    --user1.username as pat_check_name,
            //    --user2.username as pat_report_name,
            //    --userRec.username as pat_i_name,
            //    case 
            //      when user1.username is null then pat_chk_code
            //      else user1.username 
            //    end as pat_check_name,
            //    case 
            //       when user2.username is null then pat_report_code
            //       else user2.username
            //    end as pat_report_name,
            //    case 
            //       when userRec.username is null then pat_i_code
            //       else userRec.username
            //    end as pat_i_name,
            //
            //    pat_i_code,
            //    patients.pat_recheck_flag,
            //    hasresult = 0,
            //    urgent_msg_handle = 0 --是否已录入危急值处理记录
            //    --hasresult = case when exists(select top 1 res_id from resulto where resulto.res_id = pat_id) then 1 else 0 end
            //   ,urgent_count=(select count(res_id) from resulto where 
            //    res_ref_flag in('6','16','24','32','40','48','56','256','384','512','640','768','896')
            //    and res_id=patients.pat_id)--危急值数
            //
            //FROM patients
            //    Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
            //    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
            //    LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id and dict_no_type.no_del = '0'
            //    LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
            //    LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
            //    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
            //    LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
            //    
            //    --left join resulto on patients.pat_id = resulto.res_id--left join resulto on patients.pat_id = resulto.res_id
            //
            //
            //where 
            //    pat_date >= @pat_date_from 
            //    and pat_date < @pat_date_to 
            //    and pat_itr_id in ({0})
            //    and (case when (pat_ctype = '3' and (pat_flag = '0' or pat_flag is null or pat_flag ='')) then 1
            //              when (pat_ctype = '3' and pat_flag <> '0' and pat_flag <> '' and pat_flag is not null and pat_chk_code <> '{1}') then 2
            //              else 1
            //              end
            //	    ) = 1
            //--order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
            //", sqlItrID_IN, caller.LoginID, auditWord, reportWord); 
            #endregion

            if (branchHospital_Search == "关")
            {
                sqlSelect = string.Format(@"
            SELECT 
                --distinct 
                pat_sid,
                cast(0 as bit) as pat_select, 
                cast(pat_sid as bigint) as pat_sid_int,
                pat_sex,
                pat_sex_name= case when pat_sex = '1' then '男'
                                   when pat_sex = '2' then '女'
                                   else '' end,
                pat_date,
                pat_name,
                pat_age_exp,
                pat_birthday,
                pat_bed_no,
                pat_id,
                pat_itr_id,
                dict_sample.sam_name as pat_sam_name,
                dict_instrmt.itr_mid,
                dict_instrmt.itr_rep_flag,
                pat_flag,
                pat_flag_name = case when pat_flag = 0 then '未{1}'
                                     when pat_flag = 1 then '已{2}'
                                     when pat_flag = 2 then '已{2}'
                                     when pat_flag = 4 then '已打印'
                                     else '未{1}' end,
                pat_c_name,
                pat_bar_code,
                pat_host_order,
                (case patients.pat_host_order when '' then null when null then null else cast(pat_host_order as bigint) end) pat_host_order_int,
                pat_ctype,
                pat_ctype_name = case when pat_ctype = '1' then '常规'
                                      when pat_ctype = '2' then '急查'
                                      when pat_ctype = '3' then '保密'
                                      when pat_ctype = '4' then '溶栓'
                                      else '' end,
                pat_urgent_flag,
                pat_in_no,
                pat_ori_id,
                pat_no_id,
                pat_upid,pat_diag,
                dict_no_type.no_name as  pat_no_id_name,
                pat_jy_date,pat_apply_date,
                pat_chk_date,
                pat_doc_id,
                doc1.doc_name as pat_doc_name,
                pat_sdate,
                pat_dep_name,pat_dep_id,
                --user1.username as pat_check_name,
                --user2.username as pat_report_name,
                --userRec.username as pat_i_name,
                case 
                  when user1.username is null then pat_chk_code
                  else user1.username 
                end as pat_check_name,
                case 
                   when user2.username is null then pat_report_code
                   else user2.username
                end as pat_report_name,
                case 
                   when userRec.username is null then pat_i_code
                   else userRec.username
                end as pat_i_name,
            
                pat_i_code,
                patients.pat_recheck_flag,PowerUserInfo5.Username as pat_look_name,
                 (case when pat_look_date is null or pat_look_date='' then '' else CONVERT(varchar(25),pat_look_date,120) end) as pat_look_date,
                hasresult = 0,hasresult2 = -1,
                urgent_msg_handle = 0 --是否已录入危急值处理记录
                --hasresult = case when exists(select top 1 res_id from {4} where {4}.res_id = pat_id) then 1 else 0 end
               ,  0 as urgent_count
                ,'' as modify_flag
                ,isnull(pat_itr_audit_flag,0) pat_itr_audit_flag,pat_prt_flag,pat_mid_flag,patients.pat_sam_rem,
               pat_pre_flag,pat_pre_date,pat_pre_code
            FROM {3} patients
                Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
                LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
                LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id and dict_no_type.no_del = '0'
                LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
                LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
                LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
                LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
                LEFT OUTER JOIN PowerUserInfo AS PowerUserInfo5 ON patients.pat_look_code = PowerUserInfo5.loginId
                --left join resulto on patients.pat_id = resulto.res_id--left join resulto on patients.pat_id = resulto.res_id
            
            
            where 
                pat_date >= @pat_date_from 
                and pat_date < @pat_date_to 
                and pat_itr_id in ({0})
                --and (case when (pat_ctype = '3' and (pat_flag = '0' or pat_flag is null or pat_flag ='')) then 1
                  --        when (pat_ctype = '3' and pat_flag <> '0' and pat_flag <> '' and pat_flag is not null and pat_chk_code <> '{1}') then 2
                    --      else 1
                      --    end
            	    --) = 1
            --order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
            ", sqlItrID_IN, auditWord, reportWord, patientsTableName, resultTableNmae);
            }
            else
            {
                sqlSelect = string.Format(@"
            SELECT 
                --distinct 
                pat_sid,
                cast(0 as bit) as pat_select, 
                cast(pat_sid as bigint) as pat_sid_int,
                pat_sex,
                pat_sex_name= case when pat_sex = '1' then '男'
                                   when pat_sex = '2' then '女'
                                   else '' end,
                pat_date,
                pat_name,
                pat_age_exp,
                pat_birthday,
                pat_bed_no,
                pat_id,
                pat_itr_id,pat_diag,
                dict_sample.sam_name as pat_sam_name,
                dict_instrmt.itr_mid,
                dict_instrmt.itr_rep_flag,
                pat_flag,
                pat_flag_name = case when pat_flag = 0 then '未{1}'
                                     when pat_flag = 1 then '已{2}'
                                     when pat_flag = 2 then '已{2}'
                                     when pat_flag = 4 then '已打印'
                                     else '未{1}' end,
                pat_c_name,
                pat_bar_code,
                pat_host_order,
                (case patients.pat_host_order when '' then null when null then null else cast(pat_host_order as bigint) end) pat_host_order_int,
                pat_ctype,
                pat_ctype_name = case when pat_ctype = '1' then '常规'
                                      when pat_ctype = '2' then '急查'
                                      when pat_ctype = '3' then '保密'
                                      when pat_ctype = '4' then '溶栓'
                                      else '' end,
                pat_urgent_flag,
                pat_in_no,
                pat_ori_id,
                pat_no_id,
                pat_upid,
                dict_no_type.no_name as  pat_no_id_name,
                pat_jy_date,pat_apply_date,
                pat_chk_date,
                pat_doc_id,
                doc1.doc_name as pat_doc_name,
                pat_sdate,
                pat_dep_name,pat_dep_id,
                --user1.username as pat_check_name,
                --user2.username as pat_report_name,
                --userRec.username as pat_i_name,
                case 
                  when user1.username is null then pat_chk_code
                  else user1.username 
                end as pat_check_name,
                case 
                   when user2.username is null then pat_report_code
                   else user2.username
                end as pat_report_name,
                case 
                   when userRec.username is null then pat_i_code
                   else userRec.username
                end as pat_i_name,
            
                pat_i_code,
                patients.pat_recheck_flag,PowerUserInfo5.Username as pat_look_name,
                 (case when pat_look_date is null or pat_look_date='' then '' else CONVERT(varchar(25),pat_look_date,120) end) as pat_look_date,
                hasresult = 0,hasresult2 = -1,
                urgent_msg_handle = 0 --是否已录入危急值处理记录
                --hasresult = case when exists(select top 1 res_id from {4} where {4}.res_id = pat_id) then 1 else 0 end
               ,  0 as urgent_count
                ,'' as modify_flag
                ,isnull(pat_itr_audit_flag,0) pat_itr_audit_flag,pat_prt_flag,pat_mid_flag,patients.pat_sam_rem
                ,dict_hospital.hos_id
                ,dict_hospital.hos_name,pat_pre_flag,pat_pre_date,pat_pre_code
            FROM {3} patients
                Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
                LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
                LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id and dict_no_type.no_del = '0'
                LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
                LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
                LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
                LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
                LEFT OUTER JOIN PowerUserInfo AS PowerUserInfo5 ON patients.pat_look_code = PowerUserInfo5.loginId
                left join dict_depart on patients.pat_dep_id = dict_depart.dep_code
				left join dict_hospital on dict_depart.dep_hospital = dict_hospital.hos_id
                --left join resulto on patients.pat_id = resulto.res_id--left join resulto on patients.pat_id = resulto.res_id
            
            
            where 
                pat_date >= @pat_date_from 
                and pat_date < @pat_date_to 
                and pat_itr_id in ({0})
                --and (case when (pat_ctype = '3' and (pat_flag = '0' or pat_flag is null or pat_flag ='')) then 1
                  --        when (pat_ctype = '3' and pat_flag <> '0' and pat_flag <> '' and pat_flag is not null and pat_chk_code <> '{1}') then 2
                    --      else 1
                      --    end
            	    --) = 1
            --order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
            ", sqlItrID_IN, auditWord, reportWord, patientsTableName, resultTableNmae);
            }



            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("pat_date_from", dtFrom.Date);
            cmdSelect.Parameters.AddWithValue("pat_date_to", dtTo.Date.AddDays(1));

            DataTable dt = helper.GetTable(cmdSelect);

            //去掉sql中的排序，排序用代码实现 2014年4月10日11:27:40
            dt = OrderTableByHostorderAndSid(dt, dtFrom, dtTo);


            dt.TableName = "patients";
            DataTable returnDT = dt.Clone();
            foreach (DataRow drPat in dt.Rows)
            {
                if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                {
                    string patage = drPat["pat_age_exp"].ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    drPat["pat_age_exp"] = patage;
                }


                if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value
                    && drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value && drPat["pat_flag"].ToString() != "" && drPat["pat_flag"].ToString() != "0"
                    && drPat["pat_ctype"].ToString() == "3" && caller != null && !string.IsNullOrEmpty(caller.LoginID)
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_illReportNotAllowPrintMZ") == "是")
                {
                    bool hasFunc = CacheUserInfo.Current.HasFunctionByLoginID(caller.LoginID, 214);
                    if (!hasFunc)
                        continue;
                }
                DataRow row = returnDT.NewRow();
                row.ItemArray = drPat.ItemArray;
                returnDT.Rows.Add(row);
               
            }
            //dt.AcceptChanges();
            DataSet ds = new DataSet();
            ds.Tables.Add(returnDT);
            dt.Clear();
            sw.Stop();
            Debug.WriteLine(string.Format("GetPatientsList_Resume {0}ms", sw.ElapsedMilliseconds));

            return ds;
        }
        private DataTable OrderTableByHostorderAndSid(DataTable input,DateTime dtFrom,DateTime dtTo)
        {
            if (input != null && input.Rows.Count > 1)
            {
                if (!input.Columns.Contains("lenOrder"))
                {
                    input.Columns.Add("lenOrder", typeof(int));
                }
                if (!input.Columns.Contains("lenSid"))
                {
                    input.Columns.Add("lenSid", typeof(int));
                }
                foreach (DataRow row in input.Rows)
                {
                    if (row["pat_host_order"] != null && row["pat_host_order"] != DBNull.Value
                        && row["pat_host_order"].ToString().Length > 0)
                    {
                        row["lenOrder"] = row["pat_host_order"].ToString().Length;
                    }
                    else
                    {
                        row["lenOrder"] = 0;

                        //把空字符串也统一转为DBNull.Value
                        if (row.Table.Columns.Contains("pat_host_order"))
                        {
                            row["pat_host_order"] = DBNull.Value;
                        }
                    }
                    if (row["pat_sid"] != null && row["pat_sid"] != DBNull.Value)
                    {
                        row["lenSid"] = row["pat_sid"].ToString().Length;
                    }
                    else
                    {
                        row["lenSid"] = 0;
                    }
                }
                if(dtTo.Date!=dtFrom.Date)
                input.DefaultView.Sort = "pat_date,lenOrder,pat_host_order,lenSid,pat_sid";
                else
                    input.DefaultView.Sort = "lenOrder,pat_host_order,lenSid,pat_sid";
                input.Columns.Remove("lenOrder");
                input.Columns.Remove("lenSid");
                return input.DefaultView.ToTable();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// 获取病人列表(带序号并未审核未报告的)
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器</param>
        /// <returns></returns>
        public DataTable GetPatientsList_WithHostOrder(DateTime date, string itr_id)
        {
            string sqlSelect = "";

            DBHelper helper = new DBHelper();


            sqlSelect = string.Format(@"
select pat_id,pat_itr_id,pat_sid,pat_date,pat_host_order,pat_flag 
from patients 
where pat_date >= @pat_date_from 
and pat_date < @pat_date_to
and pat_itr_id='{0}' and pat_flag in(0) and pat_host_order is not null
--order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
", itr_id);


            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("pat_date_from", date.Date);
            cmdSelect.Parameters.AddWithValue("pat_date_to", date.Date.AddDays(1));

            DataTable dt = helper.GetTable(cmdSelect);


            //去掉sql中的排序，排序用代码实现 2014年4月10日11:27:40
            dt = OrderTableByHostorderAndSid(dt, date, date);
            dt.TableName = "GetPatientsList_WithHostOrder";

            return dt;
        }

        /// <summary>
        /// 获取病人资料状态（必录项目已齐，超时报告等）
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pat_itr_id"></param>
        /// <returns></returns>
        public DataTable GetPatientStatus(DateTime date, string pat_itr_id)
        {
            try
            {
                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                #region 查找是否有结果
                string sql1 = @"
select
patients.pat_id,
patients.pat_ctype,
patients.pat_recheck_flag,
0 as com_line_color,
cast(null as datetime) rep_date,-1 as resstatus,
case when patients.pat_flag is null then 0
     else patients.pat_flag end as pat_flag,
status = case when exists(select top 1 res_id from resulto where resulto.res_id = patients.pat_id and resulto.res_flag=1) then 1 else 0 end,
status2 =(select count(res_id) from resulto where res_id=patients.pat_id
               and res_ref_flag in('6','16','24','32','40','48','56','256','384','512','640','768','896')
                 ),--危急值数
'' as msg_content 
,'' as modify_flag
,0 as hasWarningMsg
,'0' AS ResMergeFlag
from patients with(nolock)
--left join patients_ext with(nolock) on patients_ext.pat_id = patients.pat_id
--left join bc_cname with(nolock) on patients.pat_bar_code=bc_cname.bc_bar_code
where
pat_date >= @date1 and pat_date < @date2
";
                if (!string.IsNullOrEmpty(pat_itr_id))
                {
                    sql1 += string.Format("and pat_itr_id = '{0}'", pat_itr_id);
                }

                SqlCommand cmd1 = new SqlCommand(sql1);
                cmd1.Parameters.AddWithValue("date1", date.Date);
                cmd1.Parameters.AddWithValue("date2", date.Date.AddDays(1));

                DataTable table = helper.GetTable(cmd1);
                #endregion

                #region 查找必录项目是否已齐全
                bool Lab_ShowColorForOverTime = CacheSysConfig.Current.GetSystemConfig("Lab_ShowColorForOverTime") == "是";



                string appsql = string.Empty;

                if (CacheSysConfig.Current.GetSystemConfig("Lab_FlagTipNoInClitem") == "是")
                    appsql = " AND com_itm_id not in (select cal_itm_ecd from dict_cl_item ) ";

                foreach (DataRow row in table.Rows)
                {
                    string pat_id = row["pat_id"].ToString();
                    //查找是否有超时
                    string sql3 = string.Empty;
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ShowComEmergentStatus") == "是")
                    {
                        //惠州三院添加组合紧急状态颜色列
                        string sqlStatus = string.Format(@"select com_line_color = (select top 1 (isnull(dict_combine.com_line_color,0))
			    from patients with(nolock)
                left join patients_mi with(nolock) on patients.pat_id = patients_mi.pat_id
			    left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id
			    where patients.pat_id='{0}' and (com_line_status is not null or com_line_status<>''))", pat_id);
                        DataTable dtStatus = helper.GetTable(sqlStatus);
                        if (dtStatus != null && dtStatus.Rows.Count > 0)
                            row["com_line_color"] = dtStatus.Rows[0]["com_line_color"];
                    }

                    string pat_ctype = row["pat_ctype"].ToString();

                    if (row["status"].ToString() == "0")
                    {
                        row["resstatus"] = 0;
                    }
                    if (row["status"].ToString() == "1" && row["pat_flag"].ToString() == "0")
                    {



                        //                        string sql2 = string.Format(@"select top 2 1 from dict_combine_mi
                        //                                left join patients_mi with(nolock) on patients_mi.pat_com_id=dict_combine_mi.com_id
                        //                                where com_popedom ='1' and pat_id='{0}'
                        //                                and  com_itm_id not in (select res_itm_id from resulto with(nolock) where res_id='{0}' and res_flag='1' and res_chr is not null and res_chr<>'') {1} ",
                        //                                                    row["pat_id"], appsql);
                        //                        if (helper.GetTable(sql2).Rows.Count > 0)
                        //                            row["status"] = "2";

                        //由于上面语句会导致数据库堵塞,因此改为下面方式,分两次查询,程序计算。

                        //项目名称相同的其中一个项目有结果则默认另外的项目也有结果
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_SameItmName_OneItmHasResAnotherHasRes") == "是")
                        {
                            string sql2_1 = string.Format(@"select dict_item.itm_name from dict_combine_mi
                                left join patients_mi with(nolock) on patients_mi.pat_com_id=dict_combine_mi.com_id
                                left join dict_item on dict_item.itm_id=dict_combine_mi.com_itm_id
                                where com_popedom ='1' and pat_id='{0}'
                                {1} ",
                                                          row["pat_id"], appsql);

                            string sql2_2 = string.Format(@"select dict_item.itm_name from resulto with(nolock) 
                                left join dict_item on dict_item.itm_id=resulto.res_itm_id
                                where res_id='{0}' and res_flag='1' and res_chr is not null and res_chr<>''  ",
                                                        row["pat_id"]);

                            DataTable dtsql2_1 = helper.GetTable(sql2_1);
                            DataTable dtsql2_2 = helper.GetTable(sql2_2);

                            if (dtsql2_1 != null && dtsql2_1.Rows.Count > 0)
                            {
                                if (dtsql2_2 != null && dtsql2_2.Rows.Count > 0)
                                {
                                    foreach (DataRow drsql2_1 in dtsql2_1.Rows)
                                    {
                                        if (dtsql2_2.Select(string.Format("itm_name='{0}'", dcl.common.SQLFormater.FormatSQL(drsql2_1["itm_name"].ToString()))).Length <= 0)
                                        {
                                            row["status"] = "2";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    row["status"] = "2";
                                }
                            }
                        }
                        else
                        {
                            #region  判断是否缺少必录项目
                            string sql2_1 = string.Format(@"select com_itm_id from dict_combine_mi
                                left join patients_mi with(nolock) on patients_mi.pat_com_id=dict_combine_mi.com_id
                                where com_popedom ='1' and pat_id='{0}'
                                 {1} ",
                                                                                        row["pat_id"], appsql);
                            string sql2_2 = string.Format(@"select res_itm_id from resulto with(nolock) where res_id='{0}' and res_flag='1' and res_chr is not null and res_chr<>'' ",
                                                        row["pat_id"]);
                            DataTable dtsql2_1 = helper.GetTable(sql2_1);
                            DataTable dtsql2_2 = helper.GetTable(sql2_2);

                            if (dtsql2_1 != null && dtsql2_1.Rows.Count > 0)
                            {
                                if (dtsql2_2 != null && dtsql2_2.Rows.Count > 0)
                                {
                                    foreach (DataRow drsql2_1 in dtsql2_1.Rows)
                                    {
                                        if (dtsql2_2.Select(string.Format("res_itm_id='{0}'", drsql2_1["com_itm_id"].ToString())).Length <= 0)
                                        {
                                            row["status"] = "2";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    row["status"] = "2";
                                }
                            }
                            #endregion
                        }


                        row["resstatus"] = row["status"];
                        if (pat_ctype != "2")
                        {
                            sql3 = string.Format(@"

select
patients.pat_id,
patients_mi.pat_com_id,
patients.pat_apply_date,
dict_combine.com_reptimes,
datediff(minute,patients.pat_apply_date,getdate()) as timepassed,
dict_combine.com_reptimes - dict_combine.com_reptimes * 0.2 as timelimited

from patients with(nolock)
left join patients_mi with(nolock) on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
where
patients.pat_id = '{0}' and dict_combine.com_reptimes is not null
and (datediff(minute,patients.pat_apply_date,getdate()) + dict_combine.com_reptimes * 0.2) >= dict_combine.com_reptimes
and dict_combine.com_reptimes>0", pat_id);
                        }
                        else
                        {
                            sql3 = string.Format(@"
select
patients.pat_id,
patients_mi.pat_com_id,
patients.pat_apply_date,
cast(dict_combine.com_urgent_times as int) com_reptimes,
datediff(minute,patients.pat_apply_date,getdate()) as timepassed,
cast(cast(dict_combine.com_urgent_times as int) as int) - cast(dict_combine.com_urgent_times as int) * 0.2 as timelimited
from patients with(nolock)
left join patients_mi with(nolock) on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
where
patients.pat_id = '{0}' and cast(dict_combine.com_urgent_times as int) is not null
and (datediff(minute,patients.pat_apply_date,getdate()) + cast(dict_combine.com_urgent_times as int) * 0.2) >= cast(dict_combine.com_urgent_times as int)
and cast(dict_combine.com_urgent_times as int)>0
", pat_id);
                        }
                        if (helper.GetTable(sql3).Rows.Count > 0)
                            row["status"] = "3";
                    }

                    if ((row["pat_flag"].ToString() == "1" || row["pat_flag"].ToString() == "0") &&
                        Lab_ShowColorForOverTime)
                    {
                        if (pat_ctype != "2")
                        {
                            sql3 = string.Format(@"

select
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients_mi.pat_com_id,
patients.pat_apply_date,
datediff(minute,pat_apply_date,getdate()) as timepassed,
CASE WHEN datediff(minute,pat_apply_date,getdate()) >= dict_combine_timerule.com_time THEN 1 ELSE 0 END isovertime,
CASE WHEN dict_combine_timerule.com_rea_time>0 and datediff(minute,pat_apply_date,getdate()) > (dict_combine_timerule.com_time-dict_combine_timerule.com_rea_time) THEN 1 ELSE 0 END isovertime2
from patients with(nolock)
left join patients_mi with(nolock) on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
LEFT JOIN dict_combine_timerule_related ON dict_combine_timerule_related.com_id=dict_combine.com_id
LEFT JOIN dict_combine_timerule ON dict_combine_timerule.com_time_id=dict_combine_timerule_related.com_time_id
AND (patients.pat_ori_id=dict_combine_timerule.com_time_ori_id  or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='') 
AND (dict_combine_timerule.com_time_start_type='5' and dict_combine_timerule.com_time_end_type='60')
AND dict_combine_timerule.com_time_type='常规'
where
patients.pat_id = '{0}' and dict_combine_timerule.com_time is not null and isnull(pat_flag,0)<>(case when dict_combine_timerule.com_time_end_type='40' then 1 else 2 end )
and ((datediff(minute,pat_apply_date,getdate())) >= dict_combine_timerule.com_time OR (dict_combine_timerule.com_rea_time>0 and datediff(minute,pat_apply_date,getdate()) > (dict_combine_timerule.com_time-dict_combine_timerule.com_rea_time)))
and dict_combine_timerule.com_time>0", pat_id);
                        }
                        else
                        {
                            sql3 = string.Format(@"
select
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients_mi.pat_com_id,
patients.pat_apply_date,
datediff(minute,pat_apply_date,getdate()) as timepassed,
CASE WHEN datediff(minute,pat_apply_date,getdate()) >= dict_combine_timerule.com_time THEN 1 ELSE 0 END isovertime,
CASE WHEN dict_combine_timerule.com_rea_time>0 and datediff(minute,pat_apply_date,getdate()) >(dict_combine_timerule.com_time-dict_combine_timerule.com_rea_time) THEN 1 ELSE 0 END isovertime2
from patients with(nolock)
left join patients_mi with(nolock) on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
LEFT JOIN dict_combine_timerule_related ON dict_combine_timerule_related.com_id=dict_combine.com_id
LEFT JOIN dict_combine_timerule ON dict_combine_timerule.com_time_id=dict_combine_timerule_related.com_time_id
AND (patients.pat_ori_id=dict_combine_timerule.com_time_ori_id  or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='') 
AND (dict_combine_timerule.com_time_start_type='5' and dict_combine_timerule.com_time_end_type='60')
AND dict_combine_timerule.com_time_type='急查'
where
patients.pat_id = '{0}' and dict_combine_timerule.com_time is not null and isnull(pat_flag,0)<>(case when dict_combine_timerule.com_time_end_type='40' then 1 else 2 end )
and ((datediff(minute,pat_apply_date,getdate())) >= dict_combine_timerule.com_time OR (dict_combine_timerule.com_rea_time>0 and datediff(minute,pat_apply_date,getdate()) > (dict_combine_timerule.com_time-dict_combine_timerule.com_rea_time)))
and dict_combine_timerule.com_time>0
", pat_id);
                        }
                        DataTable rTable = helper.GetTable(sql3);
                        if (rTable.Rows.Count > 0)
                        {
                            row["status"] = rTable.Rows[0]["isovertime"].ToString() == "1" ? "4" : "5";
                        }
                    }
                }

                #endregion




                table.TableName = "patientstatus";
                return table;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }

        }

        /// <summary>
        /// 获取病人资料状态（必录项目已齐，超时报告等）
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pat_itr_id"></param>
        /// <returns></returns>
        public DataTable GetPatientStatusForOverTime(DateTime date, string pat_itr_id)
        {
            try
            {
                DBHelper helper = new DBHelper();

                #region 查找是否有结果
                string sql1 = @"
select
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients.pat_ctype,
pat_c_name,
patients.pat_name,
patients.pat_apply_date,
case when patients.pat_flag is null then 0
     else patients.pat_flag end as pat_flag,
'0' as status,0 as timepassed,dict_instrmt.itr_mid,'' as overtype
from patients left join dict_instrmt on dict_instrmt.itr_id=patients.pat_itr_id
where
pat_date >= @date1 and pat_date < @date2  
";
                if (!string.IsNullOrEmpty(pat_itr_id))
                {
                    if (pat_itr_id.Contains("'"))
                    {
                        sql1 += string.Format("and pat_itr_id in({0})", pat_itr_id);
                    }
                    else
                    {
                        sql1 += string.Format("and pat_itr_id = '{0}'", pat_itr_id);
                    }
                }

                SqlCommand cmd1 = new SqlCommand(sql1);
                cmd1.Parameters.AddWithValue("date1", date.Date);
                cmd1.Parameters.AddWithValue("date2", date.Date.AddDays(1));

                DataTable table = helper.GetTable(cmd1);

                DataTable ret = table.Clone();
                #endregion


                bool EnableOvertimeMessage = CacheSysConfig.Current.GetSystemConfig("BC_EnableOvertimeMessage") == "是";

                #region 查找必录项目是否已齐全
                foreach (DataRow row in table.Rows)
                {
                    if (row["pat_flag"].ToString() != "2" && row["pat_flag"].ToString() != "4")
                    {


                        string pat_id = row["pat_id"].ToString();

                        string pat_ctype = row["pat_ctype"].ToString();


                        //查找是否有超时
                        string sql3 = string.Empty;

                        string addsql = " dict_combine_timerule.com_time_end_type='60' ";
                        string timecolumn = "patients.pat_apply_date";


                        if (EnableOvertimeMessage)
                        {
                            addsql =
                                " (dict_combine_timerule.com_time_end_type='40' or dict_combine_timerule.com_time_end_type='60') ";
                            timecolumn = "patients.pat_jy_date";
                        }

                        if (pat_ctype != "2")
                        {
                            sql3 = string.Format(@"

select distinct
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients_mi.pat_com_id,
patients.pat_apply_date,
datediff(minute,{1},getdate()) as timepassed,case when dict_combine_timerule.com_time_end_type='40' then '登记-一审' when dict_combine_timerule.com_time_end_type='60' then '登记-二审' else '' end as overtype
from patients
left join patients_mi on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
LEFT JOIN dict_combine_timerule_related ON dict_combine_timerule_related.com_id=dict_combine.com_id
LEFT JOIN dict_combine_timerule ON dict_combine_timerule.com_time_id=dict_combine_timerule_related.com_time_id
AND (patients.pat_ori_id=dict_combine_timerule.com_time_ori_id  or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='') AND {2}
AND dict_combine_timerule.com_time_type='常规'
where
patients.pat_id = '{0}' and dict_combine_timerule.com_time is not null and isnull(pat_flag,0)<>(case when dict_combine_timerule.com_time_end_type='40' then 1 else 2 end )
and (datediff(minute,{1},getdate())) >= dict_combine_timerule.com_time
and dict_combine_timerule.com_time>0", pat_id, timecolumn, addsql);
                        }
                        else
                        {
                            sql3 = string.Format(@"
select distinct
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients_mi.pat_com_id,
patients.pat_apply_date,
datediff(minute,{1},getdate()) as timepassed,case when dict_combine_timerule.com_time_end_type='40' then '登记-一审' when dict_combine_timerule.com_time_end_type='60' then '登记-二审' else '' end as overtype
from patients
left join patients_mi on patients_mi.pat_id = patients.pat_id
left join dict_combine on patients_mi.pat_com_id = dict_combine.com_id 
LEFT JOIN dict_combine_timerule_related ON dict_combine_timerule_related.com_id=dict_combine.com_id
LEFT JOIN dict_combine_timerule ON dict_combine_timerule.com_time_id=dict_combine_timerule_related.com_time_id
AND (patients.pat_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='') AND {2}
AND dict_combine_timerule.com_time_type='急查'
where
patients.pat_id = '{0}' and dict_combine_timerule.com_time is not null and isnull(pat_flag,0)<>(case when dict_combine_timerule.com_time_end_type='40' then 1 else 2 end )
and (datediff(minute,{1},getdate())) >= dict_combine_timerule.com_time
and dict_combine_timerule.com_time>0
", pat_id, timecolumn, addsql);
                        }
                        DataTable rTable = helper.GetTable(sql3);
                        if (rTable.Rows.Count > 0)
                        {
                            foreach (DataRow tRow in rTable.Rows)
                            {
                                row["status"] = "1";
                                row["timepassed"] = tRow["timepassed"];
                                row["overtype"] = tRow["overtype"];
                                DataRow newRow = ret.NewRow();
                                newRow.ItemArray = row.ItemArray;
                                ret.Rows.Add(newRow);
                            }

                        }
                    }
                }
                #endregion

                table.Dispose();


                ret.TableName = "patientovertime";
                return ret;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatientStatusForOverTime", ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// 获取病人列表(详细信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom">起始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">物理组ID</param>
        /// <param name="itr_id">仪器ID</param>
        /// <returns></returns>
        public DataTable GetPatientsList_Details(DateTime dtFrom, DateTime dtTo, string type_id, string itr_id)
        {
            string sqlItrID_IN = string.Empty;

            string sqlSelect;

            DBHelper helper = new DBHelper();
            //没有输入仪器，则查找物理组别中的所有仪器
            if (itr_id == string.Empty || itr_id == null)
            {
                sqlItrID_IN = "'-1'";

                sqlSelect = string.Format("select itr_id from dict_instrmt where itr_type='{0}'", type_id);
                DataTable dtItrID_List = helper.GetTable(sqlSelect);

                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dtItrID_List.Rows)
                {
                    sb.Append(string.Format(",'{0}'", dr["itr_id"]));
                }

                sqlItrID_IN = sqlItrID_IN + sb.ToString();
            }
            else
            {
                sqlItrID_IN = string.Format("'{0}'", itr_id);
            }

            sqlSelect = string.Format(@"
select 
distinct pat_sid,
cast(0 as bit) as pat_select, 
cast(pat_sid as bigint) as pat_sid_int,
pat_sex,
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end,
pat_date,
pat_name,
pat_age_exp,
pat_bed_no,
pat_id,
pat_itr_id,
dict_sample.sam_name as pat_sam_name,
dict_instrmt.itr_mid,

pat_flag,
pat_flag_name = '',

pat_ctype,
patients.pat_recheck_flag,
hasresult = case when resulto.res_id is not null then 1
                 else 0 end
--,resulto.res_id as hasresult
from patients
Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
left join resulto on patients.pat_id = resulto.res_id


where 
pat_date >= @pat_date_from 
and pat_date < @pat_date_to 
and pat_itr_id in ({0})
order by dict_instrmt.itr_mid asc, cast(pat_sid as bigint) asc
", sqlItrID_IN);


            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("pat_date_from", dtFrom.Date);
            cmdSelect.Parameters.AddWithValue("pat_date_to", dtTo.Date.AddDays(1));

            DataTable dt = helper.GetTable(cmdSelect);
            dt.TableName = "GetPatientsList_Details";

            return dt;
        }

        /// <summary>
        /// 获取病人简要信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetPatientResume(string pat_id)
        {
            string sqlSelect = string.Format(@"
select top 1
pat_id,
pat_ori_id,
pat_date,
pat_name,
pat_sid,
pat_sex,
pat_age_exp,
pat_age,
pat_itr_id,
pat_flag,
pat_sam_id,
itr_qc_flag
from patients
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
where pat_id ='{0}'", pat_id);
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);
            return dt;
        }


        public DataTable GetQcRuleMes(string pat_id)
        {
            string sqlGetRuleMes = string.Format(@"
select res_itm_ecd,qc_rule_mes.* from qc_rule_mes
inner join resulto 
on resulto.res_id='{0}' 
and qc_rule_mes.qrm_item_id=resulto.res_itm_id
and qc_rule_mes.qcm_itr_id =resulto.res_itr_id
", pat_id);

            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlGetRuleMes);
            return dt;
        }


        ///// <summary>
        ///// 获取病人基本信息
        ///// </summary>
        ///// <param name="patID"></param>
        ///// <returns></returns>
        //public DataSet GetPatientInfo(string patID)
        //{
        //    throw new NotImplementedException();
        //try
        //{
        //String pat_id = patID;
        //                string sqlSelect =
        //string.Format(@"
        //SELECT
        //      patients.pat_id, patients.pat_itr_id, patients.pat_sid, patients.pat_name, 
        //      patients.pat_sex, patients.pat_age, patients.pat_age_exp, patients.pat_age_unit, 
        //      patients.pat_dep_id, patients.pat_no_id, patients.pat_in_no, 
        //      patients.pat_admiss_times, patients.pat_bed_no, patients.pat_c_name, 
        //      patients.pat_diag, patients.pat_rem, patients.pat_work, patients.pat_tel, 
        //      patients.pat_email, patients.pat_unit, patients.pat_address, patients.pat_pre_week, 
        //      patients.pat_height, patients.pat_weight, patients.pat_sam_id, patients.pat_chk_id, 
        //      patients.pat_doc_id, patients.pat_i_code, patients.pat_chk_code, 
        //      patients.pat_send_code, patients.pat_report_code, patients.pat_ctype, 
        //      patients.pat_send_flag, patients.pat_prt_flag, patients.pat_flag, patients.pat_reg_flag, 
        //      patients.pat_urgent_flag, patients.pat_look_code, patients.pat_exp, patients.pat_pid, 
        //      patients.pat_date, patients.pat_sdate, patients.pat_rec_date, patients.pat_chk_date, 
        //      patients.pat_report_date, patients.pat_send_date, patients.pat_look_date, 
        //      patients.pat_social_no, patients.pat_emp_id, patients.pat_bar_code, 
        //      patients.pat_host_order, patients.Pat_etagere, patients.Pat_place, 
        //      patients.pat_sample_date, patients.pat_apply_date, patients.pat_jy_date, 
        //      patients.Pat_prt_date, patients.pat_sample_part, patients.pat_ori_id, 
        //      patients.pat_mid_info, patients.pat_comment, patients.pat_hospital_id, 
        //      dict_instrmt.itr_name, dict_depart.dep_name, dict_sample.sam_name, 
        //      dict_checkb.chk_cname, dict_doctor.doc_name, 
        //      PowerUserInfo.userName AS pat_chk_name, dict_no_type.no_name, 
        //      dict_origin.ori_name
        //FROM patients LEFT OUTER JOIN
        //      dict_origin ON patients.pat_ori_id = dict_origin.ori_id LEFT OUTER JOIN
        //      dict_no_type ON patients.pat_no_id = dict_no_type.no_id LEFT OUTER JOIN
        //      PowerUserInfo ON 
        //      patients.pat_chk_code = PowerUserInfo.userInfoId LEFT OUTER JOIN
        //      dict_doctor ON patients.pat_doc_id = dict_doctor.doc_id LEFT OUTER JOIN
        //      dict_checkb ON patients.pat_chk_id = dict_checkb.chk_id LEFT OUTER JOIN
        //      dict_sample ON patients.pat_sam_id = dict_sample.sam_id LEFT OUTER JOIN
        //      dict_depart ON patients.pat_dep_id = dict_depart.dep_id LEFT OUTER JOIN
        //      dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
        //WHERE patients.pat_id ='{0}'
        //                ", patID);

        //                string sqlSelect =
        //string.Format(@"
        //SELECT
        //top 1
        //patients.*,
        //dict_instrmt.itr_ptype, 
        //dict_instrmt.itr_mid,
        //dict_instrmt.itr_name,
        //dict_depart.dep_name, 
        //dict_sample.sam_name,
        //dict_checkb.chk_cname,
        //dict_doctor.doc_name, 
        //PowerUserInfo.userName AS pat_i_code_name,
        //dict_no_type.no_name, 
        //dict_origin.ori_name,
        //pat_ctype_name = '',--检查类型名称
        //pat_sex_name = case when pat_sex = '1' then '男'
        //                    when pat_sex = '2' then '女'
        //                    else '' end
        //FROM patients 
        //    LEFT OUTER JOIN dict_origin ON patients.pat_ori_id = dict_origin.ori_id 
        //    LEFT OUTER JOIN dict_no_type ON patients.pat_no_id = dict_no_type.no_id
        //    LEFT OUTER JOIN PowerUserInfo ON patients.pat_i_code = PowerUserInfo.loginId
        //    LEFT OUTER JOIN dict_doctor ON patients.pat_doc_id = dict_doctor.doc_id
        //    LEFT OUTER JOIN dict_checkb ON patients.pat_chk_id = dict_checkb.chk_id
        //    LEFT OUTER JOIN dict_sample ON patients.pat_sam_id = dict_sample.sam_id
        //    LEFT OUTER JOIN dict_depart ON patients.pat_dep_id = dict_depart.dep_id
        //    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
        //    
        //WHERE patients.pat_id ='{0}'", patID);

        //                DBHelper helper = new DBHelper();

        //                Logger.Trace("pat_sid=" + patID);
        //                DataTable dtPat = helper.GetTable(sqlSelect);

        //                FuncLibBIZ bllFuncLibBIZ = new FuncLibBIZ();
        //                DataTable dtPatCType = bllFuncLibBIZ.getPat_ctype();

        //                foreach (DataRow drPat in dtPat.Rows)
        //                {
        //                    if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value && drPat["pat_ctype"].ToString().Trim(null) != string.Empty)
        //                    {
        //                        string pat_ctype = drPat["pat_ctype"].ToString();
        //                        DataRow[] drs = dtPatCType.Select(string.Format("id='{0}'", pat_ctype));
        //                        if (drs.Length > 0)
        //                        {
        //                            drPat["pat_ctype_name"] = drs[0]["value"];
        //                        }
        //                    }
        //                }

        //                dtPat.TableName = "PatientInfo";

        //                DataSet ds = new DataSet();
        //                ds.Tables.Add(dtPat);

        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriteException(this.GetType().Name, "获取病人信息出错,病人ID:" + patID, ex.ToString());
        //                throw;
        //            }
        //}

        ///// <summary>
        ///// 获取病人组合明细
        ///// </summary>
        ///// <param name="patID"></param>
        ///// <returns></returns>
        //public DataSet GetPatientCombine(string patID)
        //{
        //    throw new NotImplementedException();
        //            try
        //            {
        //                string sql = string.Format(@"
        //SELECT dict_combine.com_name, patients_mi.*
        //FROM patients_mi INNER JOIN
        //    dict_combine ON patients_mi.pat_com_id = dict_combine.com_id
        //WHERE (patients_mi.pat_id = '{0}')
        //", patID);
        //                DBHelper helper = new DBHelper();

        //                Logger.Debug(string.Format("获取病人组合信息patID={0}", patID));
        //                DataTable dt = helper.GetTable(sql);
        //                dt.TableName = "PatientCombine";

        //                DataSet ds = new DataSet();

        //                ds.Tables.Add(dt);
        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,patID=" + patID, ex.ToString());
        //                throw;
        //            }
        //}

        /// <summary>
        /// 获取病人组合对应的所有项目
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCombineItems(string patID, string pat_sex)
        {
            string sql = string.Format(@"
select
distinct com_itm_id,
dict_item.itm_ecd as com_itm_ecd ,
dict_item.itm_sex_limit,
dict_combine_mi.com_popedom,
dict_combine_mi.com_id,
com_sort
from dict_combine_mi
inner join dict_item on dict_item.itm_id = dict_combine_mi.com_itm_id
where 
com_id in (select pat_com_id from patients_mi where pat_id = '{0}')

", patID);

            if (pat_sex != "0")
            {
                sql += string.Format("and (dict_item.itm_sex_limit = '{0}' or dict_item.itm_sex_limit='0' or dict_item.itm_sex_limit ='' or dict_item.itm_sex_limit is null)", pat_sex);
            }

            sql += " order by com_id, com_sort asc";

            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sql);
            dt.TableName = "GetPatientCombineItems";
            return dt;
        }

        /// <summary>
        /// 获取病人结果
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patSampleTypeID"></param>
        /// <param name="itr_ptype"></param>
        /// <returns></returns>
        public DataSet GetPatientResult(string patID)
        {
            string sql = string.Format(@"SELECT 
    pat_sid,
    cast(0 as bit) as pat_select, 
    cast(pat_sid as bigint) as pat_sid_int,
    pat_sex,
    pat_sex_name='', 
    pat_date,
    pat_name,
    pat_age_exp,
    pat_birthday,
    pat_bed_no,
    pat_ori_id,
    pat_id,
    pat_itr_id,
    dict_sample.sam_name as pat_sam_name,
    dict_instrmt.itr_mid,
    pat_flag,
    pat_flag_name = '',
    pat_c_name,
    pat_bar_code,
    pat_host_order,
    cast(pat_host_order as bigint) as pat_host_order_int,
    pat_ctype,
    pat_ctype_name = '',
    pat_in_no,
    pat_no_id,
    dict_no_type.no_name as  pat_no_id_name,
    pat_jy_date,
    pat_doc_id,
    doc1.doc_name as pat_doc_name,
    pat_sdate,
    pat_dep_name,
    user1.username as pat_check_name,
    user2.username as pat_report_name,
    userRec.username as pat_i_name,
    hasresult = case when exists(select top 1 res_id from resulto where resulto.res_id = pat_id and resulto.res_flag=1) then 1
                     else 0 end
    --hasresult = case when resulto.res_id is not null then 1
    --                 else 0 end
FROM patients
    Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id
    LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
    LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
    LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
    --left join resulto on patients.pat_id = resulto.res_id--left join resulto on patients.pat_id = resulto.res_id


where  pat_id='{0}'
        
", patID);
            DBHelper db = new DBHelper();
            DataTable dtPat = db.GetTable(sql);

            #region MyRegion
            foreach (DataRow drPat in dtPat.Rows)
            {
                if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                {
                    string patage = drPat["pat_age_exp"].ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    drPat["pat_age_exp"] = patage;
                }

                if (drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value)
                {
                    string patflag = drPat["pat_flag"].ToString();
                    if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        drPat["pat_flag_name"] = "已" + CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        drPat["pat_flag_name"] = "已打印";
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        drPat["pat_flag_name"] = "已" + CacheSysConfig.Current.GetSystemConfig("ReportWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                    {
                        drPat["pat_flag_name"] = "未" + CacheSysConfig.Current.GetSystemConfig("AuditWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                }
                else
                {
                    drPat["pat_flag_name"] = "未" + CacheSysConfig.Current.GetSystemConfig("AuditWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                }

                if (drPat["pat_sex"] != null && drPat["pat_sex"] != DBNull.Value)
                {
                    string pat_sex = drPat["pat_sex"].ToString();
                    if (pat_sex == "1")
                    {
                        drPat["pat_sex_name"] = "男";
                    }
                    else if (pat_sex == "2")
                    {
                        drPat["pat_sex_name"] = "女";
                    }
                }

                if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value)
                {
                    string pat_ctype = drPat["pat_ctype"].ToString();

                    if (pat_ctype == "1")
                    {
                        drPat["pat_ctype_name"] = "常规";
                    }
                    else if (pat_ctype == "2")
                    {
                        drPat["pat_ctype_name"] = "急查";
                    }
                    else if (pat_ctype == "3")
                    {
                        drPat["pat_ctype_name"] = "保密";
                    }
                    else if (pat_ctype == "4")
                    {
                        drPat["pat_ctype_name"] = "溶栓";
                    }
                }
            }
            #endregion

            dtPat.TableName = "patients";
            DataSet ds = new DataSet();
            ds.Tables.Add(dtPat);

            return ds;
        }
        /// <summary>
        /// 获取病人结果
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="patSampleTypeID"></param>
        /// <param name="itr_ptype"></param>
        /// <returns></returns>
        public DataSet GetPatientResultWithBabyFilter(string patID, bool isBabyFilter)
        {
            if (!isBabyFilter) return GetPatientResult(patID);
            string sql = string.Format(@"SELECT 
    pat_sid,
    cast(0 as bit) as pat_select, 
    cast(pat_sid as bigint) as pat_sid_int,
    pat_sex,
    pat_sex_name='', 
    pat_date,
    pat_name,
    pat_age_exp,
    pat_bed_no,
    pat_ori_id,
    pat_id,
    pat_itr_id,
    dict_sample.sam_name as pat_sam_name,
    dict_instrmt.itr_mid,
    pat_flag,
    pat_flag_name = '',
    pat_c_name,
    pat_bar_code,
    pat_host_order,
    cast(pat_host_order as bigint) as pat_host_order_int,
    pat_ctype,
    pat_ctype_name = '',
    pat_in_no,
    pat_no_id,
    dict_no_type.no_name as  pat_no_id_name,
    pat_jy_date,
    pat_doc_id,
    doc1.doc_name as pat_doc_name,
    pat_sdate,
    pat_dep_name,
    user1.username as pat_check_name,
    user2.username as pat_report_name,
    userRec.username as pat_i_name,
    hasresult = case when exists(select top 1 res_id from resulto_newborn where resulto_newborn.res_id = pat_id and resulto_newborn.res_flag=1) then 1
                     else 0 end
    --hasresult = case when resulto.res_id is not null then 1
    --                 else 0 end
FROM patients_newborn patients
    Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id
    LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
    LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
    LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
    --left join resulto on patients.pat_id = resulto.res_id--left join resulto on patients.pat_id = resulto.res_id


where  pat_id='{0}'
        
", patID);
            DBHelper db = new DBHelper();
            DataTable dtPat = db.GetTable(sql);

            #region MyRegion
            foreach (DataRow drPat in dtPat.Rows)
            {
                if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                {
                    string patage = drPat["pat_age_exp"].ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    drPat["pat_age_exp"] = patage;
                }

                if (drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value)
                {
                    string patflag = drPat["pat_flag"].ToString();
                    if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        drPat["pat_flag_name"] = "已" + CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        drPat["pat_flag_name"] = "已打印";
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        drPat["pat_flag_name"] = "已" + CacheSysConfig.Current.GetSystemConfig("ReportWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                    {
                        drPat["pat_flag_name"] = "未" + CacheSysConfig.Current.GetSystemConfig("AuditWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                }
                else
                {
                    drPat["pat_flag_name"] = "未" + CacheSysConfig.Current.GetSystemConfig("AuditWord");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                }

                if (drPat["pat_sex"] != null && drPat["pat_sex"] != DBNull.Value)
                {
                    string pat_sex = drPat["pat_sex"].ToString();
                    if (pat_sex == "1")
                    {
                        drPat["pat_sex_name"] = "男";
                    }
                    else if (pat_sex == "2")
                    {
                        drPat["pat_sex_name"] = "女";
                    }
                }

                if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value)
                {
                    string pat_ctype = drPat["pat_ctype"].ToString();

                    if (pat_ctype == "1")
                    {
                        drPat["pat_ctype_name"] = "常规";
                    }
                    else if (pat_ctype == "2")
                    {
                        drPat["pat_ctype_name"] = "急查";
                    }
                    else if (pat_ctype == "3")
                    {
                        drPat["pat_ctype_name"] = "保密";
                    }
                    else if (pat_ctype == "4")
                    {
                        drPat["pat_ctype_name"] = "溶栓";
                    }
                }
            }
            #endregion

            dtPat.TableName = "patients";
            DataSet ds = new DataSet();
            ds.Tables.Add(dtPat);

            return ds;
        }
        //        /// <summary>
        //        /// 获取仪器的下一个样本号
        //        /// </summary>
        //        /// <param name="date"></param>
        //        /// <param name="itr_id"></param>
        //        /// <returns></returns>
        //        public string GetItrNextSID(DateTime date, string itr_id)
        //        {
        //            try
        //            {
        //                int iSID = 0;

        //                string sqlSelect = string.Format(@"
        //select max(cast(pat_sid as int)) from patients
        //where 
        //pat_itr_id = '{0}'
        //and pat_date >= @pat_date_from 
        //and pat_date < @pat_date_to 
        //", itr_id);
        //                SqlCommand cmdSelect = new SqlCommand(sqlSelect);
        //                cmdSelect.Parameters.AddWithValue("pat_date_from", date.Date);
        //                cmdSelect.Parameters.AddWithValue("pat_date_to", date.Date.AddDays(1));

        //                DBHelper helper = new DBHelper();

        //                object objSID = helper.ExecuteScalar(cmdSelect);
        //                if (objSID == null || objSID == DBNull.Value)
        //                {
        //                    iSID = 1;
        //                }
        //                else
        //                {
        //                    iSID = Convert.ToInt32(objSID);
        //                    iSID++;
        //                }
        //                return iSID.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.WriteException(this.GetType().Name, string.Format("仪器日期的下一个样本ID出错:GetNextSID({0},{1})", itr_id, date.ToString("yyyy-MM-dd")), ex.ToString());
        //                throw;
        //            }
        //        }

        /// <summary>
        /// 获取仪器的下一个可用样本号
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        public string GetItrNextValidateSID(DateTime date, string itr_id, string currentSID)
        {
            //检查样本号是否可用
            if (!string.IsNullOrEmpty(currentSID) && currentSID.Split('$').Length == 2)
            {
                return PatCommonBll.ExistSID(currentSID.Split('$')[0], itr_id, date).ToString();
            }
            int iCurrSID;

            if (!int.TryParse(currentSID, out iCurrSID))
            {
                iCurrSID = 1;
            }

            string sqlSelect = string.Format(@"
select 
pat_sid
from patients
where 
pat_itr_id = '{0}'
and pat_date >= @pat_date_from 
and pat_date < @pat_date_to 
and cast(pat_sid as bigint) >= {1}
", itr_id, iCurrSID);

            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("pat_date_from", date.Date);
            cmdSelect.Parameters.AddWithValue("pat_date_to", date.Date.AddDays(1));

            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(cmdSelect);

            if (dt.Rows.Count > 0)
            {
                List<Int64> sidList = new List<Int64>();
                foreach (DataRow dr in dt.Rows)
                {
                    sidList.Add(Convert.ToInt64(dr["pat_sid"]));
                }

                //if (iCurrSID > sidList.Max())
                //{
                //    return (sidList.Max() + 1).ToString(); 
                //}

                Int64 temp = iCurrSID < sidList.Min() ? iCurrSID : sidList.Min();

                for (Int64 i = temp; i < sidList.Max(); i++)
                {
                    if (!sidList.Any(o => o == i))
                    {
                        return i.ToString();
                    }
                }
                return (sidList.Max() + 1).ToString();
            }
            else
            {
                return iCurrSID.ToString();
            }

            //if (dt.Rows.Count > 0)
            //{
            //List<int> sidList = new List<int>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    sidList.Add(Convert.ToInt32(dr["pat_sid"]));
            //}

            //int currentMaxValue = iCurrSID + 1;
            //if (sidList.Count > 0)
            //{
            //    currentMaxValue = sidList.Max() + 1;
            //}

            //int iNextSID = iCurrSID + 1;
            //for (int i = iNextSID; i < currentMaxValue; i++)
            //{
            //    if (!sidList.Any(o => o == i))
            //    {
            //        return i.ToString();
            //    }
            //}

            //return currentMaxValue.ToString();
            //}
            //else
            //{
            //    return currentSID;
            //}
        }

        /// <summary>
        /// 根据当前仪器和样本号、年份获取满足条件的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        public string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID)
        {
            string strRv = "";
            try
            {
                if ((!string.IsNullOrEmpty(itr_id)) && (!string.IsNullOrEmpty(currentSID)))
                {
                    string sqlSelect = string.Format(@"
select top 1 pat_id,pat_itr_id,pat_sid,pat_date from patients
where pat_itr_id='{0}' 
and pat_sid='{1}' 
and pat_date>='{2}-1-1' 
and pat_date<='{2}-12-31 23:59:59.000'
", itr_id, currentSID, date.Year.ToString());

                    SqlCommand cmdSelect = new SqlCommand(sqlSelect);

                    DBHelper helper = new DBHelper();
                    DataTable dt = helper.GetTable(cmdSelect);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["pat_date"].ToString()))
                        {
                            strRv = dt.Rows[0]["pat_date"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatDate_ByItrSID", ex.ToString());
            }
            return strRv;
        }

        /// <summary>
        /// 获取一个新的筛查号
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.Description("获取一个新的筛查号")]
        public string GetPatFiltercodeNewNo()
        {
            string newFiltercode = "";
            try
            {
                DBHelper helper = new DBHelper();

                DataSet dsResult = helper.ExecuteSql("sp_get_filtercode");
                if (dsResult != null)
                {
                    DataTable dtResult = dsResult.Tables[0];
                    newFiltercode = dtResult.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatFiltercodeNewNo", ex.ToString());
            }
            return newFiltercode;
        }

        /// <summary>
        /// 获取仪器最大样本号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrSID_MaxPlusOne(DateTime date, string itr_id, bool excluseSeqRecord)
        {
            DictInstructmentBLL bll = new DictInstructmentBLL();
            return bll.GetItrSID_MaxPlusOne(date, itr_id, excluseSeqRecord);
        }

        /// <summary>
        /// 获取仪器最大样本号+1(陈星海实验号)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrCxhNewSID_MaxPlusOne(DateTime date, string itr_id)
        {
            DictInstructmentBLL bll = new DictInstructmentBLL();
            return bll.GetItrCxhNewSID_MaxPlusOne(date, itr_id);
        }

        /// <summary>
        /// 获取仪器最大样本号+1新生儿筛查
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrSID_MaxPlusOneForBF(DateTime date, string itr_id, bool excluseSeqRecord)
        {
            DictInstructmentBLL bll = new DictInstructmentBLL();
            return bll.GetItrSID_MaxPlusOneForBF(date, itr_id, excluseSeqRecord);
        }
        /// <summary>
        /// 获取仪器最大序号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrHostOrder_MaxPlusOne(DateTime date, string itr_id)
        {
            DictInstructmentBLL bll = new DictInstructmentBLL();
            return bll.GetItrHostOrder_MaxPlusOne(date, itr_id);
        }


        /// <summary>
        /// 获取指定病人的所有复查备份项目
        /// </summary>
        /// <param name="patID">病人ID</param>
        /// <returns></returns>
        public DataTable GetBackupResult(string patID)
        {
            try
            {
                if (patID == null)
                {
                    patID = string.Empty;
                }

                DBHelper helper = new DBHelper();
                string sqlSelectBak = string.Format(@"
select
    res_itm_id,
    res_itm_ecd,
    res_chr,
    res_od_chr,
    res_cast_chr,
    res_date,
    res_bak_type = '备份结果'
from resulto_bak
where
res_id = '{0}'
", patID);
                DataTable dt = helper.GetTable(sqlSelectBak);
                dt.TableName = "GetBackupResult";

                string sqlSelectMid = string.Format(@"
select
    dict_item.itm_id as res_itm_id,
    dict_item.itm_ecd as res_itm_ecd,
    resulto_mid.res_chr_a as res_chr,
    resulto_mid.res_chr_b as res_od_chr,
    res_cast_chr = null,
    resulto_mid.res_date,
    res_bak_type = '仪器结果'
    
from resulto_mid
    inner join dict_mitm_no on dict_mitm_no.mit_itr_id = resulto_mid.res_itr_id and dict_mitm_no.mit_cno = resulto_mid.res_cno
    inner join dict_item on dict_item.itm_id = dict_mitm_no.mit_itm_id
where 
(resulto_mid.res_itr_id + convert(varchar(8),resulto_mid.res_date,112) + resulto_mid.res_sid) = '{0}'
and (res_data_type = 0 or res_data_type = 1)
", patID);

                DataTable dt2 = helper.GetTable(sqlSelectMid);


                foreach (DataRow row in dt2.Rows)
                {
                    decimal decCastChr = 0;
                    if (decimal.TryParse(row["res_chr"].ToString(), out decCastChr))
                    {
                        row["res_cast_chr"] = decCastChr;
                    }
                    dt.Rows.Add(row.ItemArray);
                }

                dt.DefaultView.Sort = "res_itm_ecd asc,res_date desc";

                return dt;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("获取备份项目出错:GetBackupResult({0})", patID), ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 批量发送中期报告
        /// </summary>
        /// <param name="listPatientsID">patID集</param>
        /// <param name="checkType">操作类型</param>
        /// <param name="caller">操作信息</param>
        /// <param name="IsCheckData">发送时是否要检查</param>
        /// <returns></returns>
        public EntityOperationResultList CommonMidReport(string[] listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller, bool IsCheckData)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.CommonMidReport(listPatientsID.ToArray(), checkType, caller, IsCheckData);
        }

        /// <summary>
        /// 获取指定病人、指定项目的复查备份数据
        /// </summary>
        /// <param name="patID">病人ID</param>
        /// <param name="itm_ecd">项目代码</param>
        /// <returns></returns>
        public DataSet GetItemBackupResult(string patID, string itm_ecd)
        {
            try
            {
                if (patID == null)
                {
                    patID = string.Empty;
                }

                if (itm_ecd == null)
                {
                    itm_ecd = string.Empty;
                }

                DBHelper helper = new DBHelper();
                string sqlSelect = string.Format(@"
select
res_itm_id,
res_itm_ecd,
res_chr,
res_od_chr,
--res_cast_chr,
res_date,
res_date_string = ''
from resulto_bak
where
res_id = '{0}'
and res_itm_ecd = '{1}'
order by res_itm_ecd desc,res_date desc
", patID, itm_ecd);
                DataTable dt = helper.GetTable(sqlSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["res_date"] != null && dr["res_date"] != DBNull.Value)
                    {
                        dr["res_date_string"] = Convert.ToDateTime(dr["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("获取指定病人、指定项目的复查备份数据出错:GetItemBackupResult({0},{1})", patID, itm_ecd), ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 备份项目
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtResultItems"></param>
        public EntityOperationResult BackupResult(string pat_id, DataTable dtResultItems)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("BackupResult");

            DateTime today = ServerDateTime.GetDatabaseServerDateTime();

            List<string> itemsID = new List<string>();
            foreach (DataRow drResItem in dtResultItems.Rows)
            {
                drResItem["res_id"] = pat_id;
                string res_itm_id = drResItem["res_itm_id"].ToString();
                itemsID.Add(res_itm_id);

                if (drResItem["res_date"] == null || drResItem["res_date"] == DBNull.Value)
                {
                    drResItem["res_date"] = today;
                }
            }

            SqlCommand cmdDelete = new PatDeleteBLL().GetDelPatCommonResultItemsCMD(pat_id, itemsID);
            List<SqlCommand> cmdInsert = DBTableHelper.GenerateInsertCommand("resulto_bak", new string[] { "res_key" }, dtResultItems);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    helper.ExecuteNonQuery(cmdDelete);
                    foreach (SqlCommand cmdInst in cmdInsert)
                    {
                        helper.ExecuteNonQuery(cmdInst);
                    }
                    helper.Commit();
                }

            }
            catch (Exception ex)
            {
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                Logger.WriteException(this.GetType().Name, "BackupResult", ex.ToString());
            }

            return opResult;
        }

        /// <summary>
        /// 根据病人ID类型和ID号获取病人资料
        /// </summary>
        /// <param name="pat_no_id"></param>
        /// <param name="pat_in_no"></param>
        /// <returns></returns>
        public DataSet GetPatientInfoByID(string pat_no_id, string pat_in_no)
        {
            try
            {
                DBHelper helper = new DBHelper();
                DataSet ds = new DataSet();
                DataTable dt = null;

                if (pat_no_id == null)
                {
                    pat_no_id = "-1";
                }

                if (pat_in_no == null)
                {
                    pat_in_no = "-1";
                }

                string no_type = GetPatNoType(pat_no_id);

                if (no_type != "barcode")
                {
                    string sqlSelect = string.Format(@"
                                                select top 1 
                                                pat_name,
                                                pat_sex,
                                                pat_age_exp,
                                                pat_bed_no,
                                                pat_work,
                                                pat_tel,
                                                pat_email,
                                                pat_unit,
                                                pat_address,
                                                pat_height,
                                                pat_weight,
                                                pat_ori_id,
                                                dict_origin.ori_name as pat_ori_name
                                                --,pat_sample_date
                                                --,pat_sdate
                                                --,pat_apply_date
                                                from patients 
                                                left join dict_origin on patients.pat_ori_id = dict_origin.ori_id
						                        where	
                                                pat_no_id = '{0}'
                                                and pat_in_no ='{1}'
                                                and pat_date <= @pat_date
                                                order by pat_date desc
                                                ", pat_no_id, pat_in_no);


                    SqlCommand cmd = new SqlCommand(sqlSelect);
                    cmd.Parameters.AddWithValue("pat_date", DateTime.Now);
                    dt = helper.GetTable(cmd);
                    dt.TableName = PatientTable.PatientInfoTableName;
                    ds.Tables.Add(dt);
                }
                else
                {
                    ds = GetPatientsByBarCode(pat_in_no);
                }
                return ds;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("根据病人ID类型和ID号获取病人资料出错:GetPatientInfoByID(pat_no_id={0},pat_in_no={1})", pat_no_id, pat_in_no), ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 根据条码号获取病人资料
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        /// 已增加 
        public DataSet GetPatientsByBarCode(string barCode)
        {
            return null;
            //获取lis病人资料表结构
        }

        /// <summary>
        /// 条码病人表资料转换为lis病人资料表
        /// </summary>
        /// <param name="dtBarCodePat"></param>
        /// <param name="dtLisPat"></param>
        private void ConvertBarCodePatientToLisPatient(DataTable dtBarCodePat, DataTable dtLisPat)
        {
            dtLisPat.Clear();
            DataRow drBarcode = dtBarCodePat.Rows[0];
            DataRow drPatInfo = dtLisPat.NewRow();

            //姓名
            drPatInfo[PatientTable.Name] = drBarcode[BarcodeTable.Patient.Name];

            //性别
            string sex = "0";
            if (drBarcode[BarcodeTable.Patient.Sex] != DBNull.Value)
            {
                if (drBarcode[BarcodeTable.Patient.Sex].ToString() == "男"
                    || drBarcode[BarcodeTable.Patient.Sex].ToString() == "1")
                {
                    sex = "1";
                }
                else if (drBarcode[BarcodeTable.Patient.Sex].ToString() == "女"
                    || drBarcode[BarcodeTable.Patient.Sex].ToString() == "2"
                    )
                {
                    sex = "2";
                }
                else
                {
                    sex = string.Empty;
                }
            }

            drPatInfo[PatientTable.Sex] = sex;

            string pat_sex_name = string.Empty;
            if (sex == "1")
            {
                pat_sex_name = "男";
            }
            else if (sex == "2")
            {
                pat_sex_name = "女";
            }

            drPatInfo["pat_sex_name"] = pat_sex_name;

            //年龄
            if (drBarcode[BarcodeTable.Patient.Age] != null && drBarcode[BarcodeTable.Patient.Age] != DBNull.Value)
            {
                //目前只截取年
                string age = drBarcode[BarcodeTable.Patient.Age].ToString();

                if (age.ToLower().Contains('y')
                && age.ToLower().Contains('m')
                && age.ToLower().Contains('d')
                && age.ToLower().Contains('h')
                && age.ToLower().Contains('i')
                )
                {
                    drPatInfo["pat_age_exp"] = age;
                }
                else
                {
                    int intAge;
                    age = age.Trim().Split('.')[0];
                    if (age != null && age.Length > 0)
                    {
                        if (
                            age.Contains("Y")
                            && age.Contains("M")
                            && age.Contains("D")
                            && age.Contains("H")
                            && age.Contains("I")
                            )
                        {

                        }
                        else if (int.TryParse(age, out intAge))
                        {
                            age = age + "Y0M0D0H0I";
                        }
                        else//老outlink
                        {
                            age = age.ToUpper().Replace('年', 'Y')
                                   .Replace('岁', 'Y')
                                   .Replace("个月", "M")
                                   .Replace('月', 'M')
                                   .Replace('日', 'D')
                                   .Replace('天', 'D')
                                   .Replace("小时", "H")
                                   .Replace('时', 'H')
                                   .Replace("分钟", "I")
                                   .Replace('分', 'I');

                            string patten = "(Y|D|M|H|I)";
                            string[] tmp = Regex.Split(age, patten);
                            string[] tmp2 = new string[tmp.Length];
                            int count = 0;
                            for (int i = 0; i < tmp.Length; i = i + 2)
                            {
                                if (i + 1 >= tmp.Length)
                                    continue;
                                tmp2[count] = tmp[i] + tmp[i + 1];
                                count++;
                            }
                            string year = null;
                            string month = null;
                            string day = null;
                            string hour = null;
                            string minute = null;
                            foreach (string s in tmp2)
                            {
                                if (string.IsNullOrEmpty(s))
                                    continue;

                                if (s.Contains("Y") && year == null)
                                    year = s;

                                if (s.Contains("M") && month == null)
                                    month = s;

                                if (s.Contains("D") && day == null)
                                    day = s;

                                if (s.Contains("H") && hour == null)
                                    hour = s;

                                if (s.Contains("I") && minute == null)
                                    minute = s;
                            }
                            if (year == null) year = "0Y";
                            if (month == null) month = "0M";
                            if (day == null) day = "0D";
                            if (hour == null) hour = "0H";
                            if (minute == null) minute = "0I";
                            age = year + month + day + hour + minute;
                        }
                    }
                    drPatInfo["pat_age_exp"] = age;
                }
                ////目前只截取年
                //string age = drBarcode[BarcodeTable.Patient.Age].ToString();
                //age = age.Trim().Split('.')[0];
                //if (age != null && age.Length > 0)
                //{
                //    if (age.IndexOf("年") >= 0) //if (age.Length > 3)
                //    {
                //        if (age.EndsWith("年") || age.EndsWith("岁"))
                //        {
                //            age = age.Replace('年', 'Y').Replace('岁', 'Y') + "0M0D0H0I";
                //        }
                //        else
                //        {
                //            age = age.Replace('年', 'Y').Replace('月', 'M').Replace('日', 'D').Replace('时', 'H');//新outlink修改了返回年龄格式    2010-6-18 by li
                //            age = age + "0I";
                //        }
                //    }
                //    else//老outlink
                //    {
                //        if (age.EndsWith("年") || age.EndsWith("岁"))
                //        {
                //            age = age.Replace('年', 'Y').Replace('岁', 'Y') + "0M0D0H0I";
                //        }
                //        else if (age.EndsWith("月"))
                //        {
                //            age = "0Y" + age.Replace('月', 'M') + "0D0H0I";
                //        }
                //        else if (age.EndsWith("天") || age.EndsWith("日"))
                //        {
                //            age = "0Y0M" + age.Replace('天', 'D').Replace('日', 'D') + "0H0I";
                //        }
                //        else
                //        {
                //            age = age + "Y0M0D0H0I";
                //        }
                //    }

                //    drPatInfo[PatientTable.Age_text] = age;
                //}
            }

            //病人来源
            drPatInfo["ori_name"] = drBarcode["bc_ori_name"];
            drPatInfo["pat_ori_id"] = drBarcode["bc_ori_id"];
            //所属院区
            if (drBarcode["bc_hospital_id"] != DBNull.Value)
            {
                drPatInfo["pat_hospital_id"] = drBarcode["bc_hospital_id"];
            }
            drPatInfo["pat_age"] = AgeConverter.AgeValueTextToMinute(drPatInfo[PatientTable.Age_text].ToString());

            if (drBarcode["bc_reach_date"] != DBNull.Value)//送达时间
            {
                drPatInfo["pat_reach_date"] = Convert.ToDateTime(drBarcode["bc_reach_date"]);
            }

            DateTime now = ServerDateTime.GetDatabaseServerDateTime();
            string Lab_BarcodeTimeCal = "佛山市一";
            Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");

            if (Lab_BarcodeTimeCal == "佛山市一")
            {
                #region 佛山市一
                if (drBarcode["bc_print_date"] == DBNull.Value)
                {
                    drBarcode["bc_print_date"] = drBarcode["bc_date"];
                }

                //送检时间
                if (!Compare.IsEmpty(drBarcode["bc_reach_date"]))//送达时间 不为空
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_reach_date"]);
                }
                else if (!Compare.IsEmpty(drBarcode["bc_receiver_date"]))//签收时间 不为空
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_receiver_date"]);
                }
                else
                {
                    drPatInfo[PatientTable.SampleSendDate] = now;
                }

                ////采样时间
                if (!Compare.IsEmpty(drPatInfo["pat_ori_id"]))
                {
                    string ori_id = drPatInfo["pat_ori_id"].ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo[PatientTable.SampleSendDate]);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(drBarcode["bc_blood_date"]))
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (drBarcode["bc_blood_date"] != DBNull.Value)
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                        }
                        else
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                        }
                    }
                    else
                    {
                        if (drBarcode["bc_blood_date"] != DBNull.Value)
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                        }
                        else
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                        }
                    }
                }
                else
                {
                    if (drBarcode["bc_blood_date"] != DBNull.Value)
                    {
                        drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                    }
                    else
                    {
                        drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                    }
                }

                if (drBarcode[BarcodeTable.Patient.DateApply] == DBNull.Value)
                {
                    drBarcode[BarcodeTable.Patient.DateApply] = drPatInfo[PatientTable.SampleCollectDate];
                }

                //医嘱执行时间(申请时间)
                drPatInfo["pat_sample_receive_date"] = drBarcode[BarcodeTable.Patient.DateApply];
                if (Convert.ToDateTime(drPatInfo[PatientTable.SampleCollectDate]) < Convert.ToDateTime(drPatInfo["pat_sample_receive_date"])
                    && Convert.ToDateTime(drPatInfo["pat_sample_receive_date"]) <= Convert.ToDateTime(drPatInfo[PatientTable.SampleSendDate])
                    )
                {
                    drPatInfo[PatientTable.SampleCollectDate] = drPatInfo["pat_sample_receive_date"];
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "清远人医")
            {
                #region 清远人医
                if (drBarcode["bc_print_date"] == DBNull.Value)
                {
                    drBarcode["bc_print_date"] = drBarcode["bc_date"];
                }

                //送检时间
                if (!Compare.IsEmpty(drBarcode["bc_send_date"]))//送检时间 不为空
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_send_date"]);
                }
                else if (!Compare.IsEmpty(drBarcode["bc_reach_date"]))//送达时间 不为空
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_reach_date"]);
                }
                else if (!Compare.IsEmpty(drBarcode["bc_receiver_date"]))//签收时间 不为空
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_receiver_date"]);
                }
                else
                {
                    drPatInfo[PatientTable.SampleSendDate] = now;
                }


                ////采样时间
                if (!Compare.IsEmpty(drPatInfo["pat_ori_id"]))
                {
                    string ori_id = drPatInfo["pat_ori_id"].ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo[PatientTable.SampleSendDate]);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(drBarcode["bc_blood_date"]))
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo[PatientTable.SampleCollectDate] = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (drBarcode["bc_blood_date"] != DBNull.Value)
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                        }
                        else
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                        }
                    }
                    else
                    {
                        if (drBarcode["bc_blood_date"] != DBNull.Value)
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                        }
                        else
                        {
                            drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                        }
                    }
                }
                else
                {
                    if (drBarcode["bc_blood_date"] != DBNull.Value)
                    {
                        drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                    }
                    else
                    {
                        drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_print_date"]);
                    }
                }

                if (drBarcode[BarcodeTable.Patient.DateApply] == DBNull.Value)
                {
                    drBarcode[BarcodeTable.Patient.DateApply] = drPatInfo[PatientTable.SampleCollectDate];
                }

                //医嘱执行时间(申请时间)
                drPatInfo["pat_sample_receive_date"] = drBarcode[BarcodeTable.Patient.DateApply];
                if (Convert.ToDateTime(drPatInfo[PatientTable.SampleCollectDate]) < Convert.ToDateTime(drPatInfo["pat_sample_receive_date"])
                    && Convert.ToDateTime(drPatInfo["pat_sample_receive_date"]) <= Convert.ToDateTime(drPatInfo[PatientTable.SampleSendDate])
                    )
                {
                    drPatInfo[PatientTable.SampleCollectDate] = drPatInfo["pat_sample_receive_date"];
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "中山人医")
            {
                #region 中山人医
                if (drBarcode["bc_print_date"] == DBNull.Value)
                {
                    drBarcode["bc_print_date"] = drBarcode["bc_date"];
                }

                if (drBarcode["bc_blood_date"] != DBNull.Value)//采样时间
                {
                    drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                }

                //采集时间为空，则标本收取时间作为采集时间
                if (string.IsNullOrEmpty(drBarcode["bc_blood_date"].ToString())
                    && !string.IsNullOrEmpty(drBarcode["bc_send_date"].ToString()))
                {
                    drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_send_date"]).AddMinutes(-new Random().Next(3, 9));
                }

                if (drBarcode["bc_send_date"] != DBNull.Value)//送检时间(收取)
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_send_date"]);
                }

                if (drBarcode["bc_receiver_date"] != DBNull.Value)//接收时间
                {
                    drPatInfo[PatientTable.SampleReceiveDate] = Convert.ToDateTime(drBarcode["bc_receiver_date"]);
                }

                if (drBarcode["bc_occ_date"] != DBNull.Value)//申请时间
                {
                    drPatInfo["pat_sample_receive_date"] = Convert.ToDateTime(drBarcode["bc_occ_date"]);
                }

                if (drBarcode["bc_reach_date"] != DBNull.Value)//送达时间
                {
                    drPatInfo["pat_reach_date"] = Convert.ToDateTime(drBarcode["bc_reach_date"]);
                }

                #endregion
            }
            else
            {
                if (drBarcode["bc_blood_date"] != DBNull.Value)//采样时间
                {
                    drPatInfo[PatientTable.SampleCollectDate] = Convert.ToDateTime(drBarcode["bc_blood_date"]);
                }

                if (drBarcode["bc_send_date"] != DBNull.Value)//送检时间(收取)
                {
                    drPatInfo[PatientTable.SampleSendDate] = Convert.ToDateTime(drBarcode["bc_send_date"]);
                }

                if (drBarcode["bc_receiver_date"] != DBNull.Value)//接收时间
                {
                    drPatInfo[PatientTable.SampleReceiveDate] = Convert.ToDateTime(drBarcode["bc_receiver_date"]);
                }

                if (drBarcode["bc_occ_date"] != DBNull.Value)//申请时间
                {
                    drPatInfo["pat_sample_receive_date"] = Convert.ToDateTime(drBarcode["bc_occ_date"]);
                }

                if (drBarcode["bc_reach_date"] != DBNull.Value)//送达时间
                {
                    drPatInfo["pat_reach_date"] = Convert.ToDateTime(drBarcode["bc_reach_date"]);
                }

            }

            if (drBarcode.Table.Columns.Contains("bc_times"))
            {
                int iAdmissTimes = 0;

                if (int.TryParse(drBarcode["bc_times"].ToString(), out iAdmissTimes))
                {
                    drPatInfo["pat_admiss_times"] = iAdmissTimes;
                }
                else
                {
                    drPatInfo["pat_admiss_times"] = 0;
                }
            }

            //标本备注
            drPatInfo["pat_sam_rem"] = drBarcode[BarcodeTable.Patient.SampleRemarkName];

            //ID类型
            drPatInfo[PatientTable.IDType] = drBarcode[BarcodeTable.Patient.IDType];

            //接收时间
            drPatInfo[PatientTable.SampleReceiveDate] = drBarcode[BarcodeTable.Patient.SampleReceiveDate];

            drPatInfo["pat_jy_date"] = now;

            //条码
            drPatInfo[PatientTable.BarCode] = drBarcode[BarcodeTable.Patient.BarcodeDisplayNumber];

            //病床号
            drPatInfo[PatientTable.BedNumber] = drBarcode[BarcodeTable.Patient.BedNumber];

            //ID
            drPatInfo[PatientTable.Number] = drBarcode[BarcodeTable.Patient.PatientID];

            //病区code
            drPatInfo["pat_ward_id"] = drBarcode[BarcodeTable.Patient.DepartmentCode];

            //病区名称
            drPatInfo["pat_ward_name"] = string.Empty;

            //送检科室名称
            drPatInfo["pat_dep_name"] = drBarcode[BarcodeTable.Patient.Department];

            drPatInfo["pat_social_no"] = drBarcode["bc_social_no"];

            //送检科室code
            drPatInfo["pat_dep_id"] = string.Empty;

            if ((CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "通用"
                || CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "outlink")
                // && SystemConfiguration.GetSystemConfig("GetPatientsInfoType") == "通用"
                && !Compare.IsNullOrDBNull(drBarcode[BarcodeTable.Patient.DepartmentCode]))
            {
                drPatInfo["pat_dep_id"] = drBarcode[BarcodeTable.Patient.DepartmentCode].ToString();
            }
            //联系地址
            drPatInfo[PatientTable.Address] = drBarcode[BarcodeTable.Patient.Address];

            //联系电话
            drPatInfo[PatientTable.Tel] = drBarcode[BarcodeTable.Patient.Tel];


            //if (Compare.IsEmpty(drBarcode[BarcodeTable.Patient.DoctorID]))
            //{
            //    if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.Doctor]))
            //    {
            //        drPatInfo[PatientTable.DoctorID] = new DictDoctor().GetDocByName(drBarcode[BarcodeTable.Patient.Doctor].ToString());
            //    }
            //}
            //else
            //{
            //    //开单医生ID
            //    drPatInfo[PatientTable.DoctorID] = drBarcode[BarcodeTable.Patient.DoctorID];
            //}


            //if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.DoctorID]))//如果医生工号不为空
            //{
            //    //用医生工号获取医生ID
            //    drPatInfo[PatientTable.DoctorID] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(drBarcode[BarcodeTable.Patient.DoctorID].ToString());
            //    //new DictDoctor().GetDocIDByCode(drBarcode[BarcodeTable.Patient.DoctorID].ToString());
            //}
            //else
            //{
            //    if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.Doctor]))//如果医生姓名不为空,则用医生姓名查找出医生ID
            //    {
            //        drPatInfo[PatientTable.DoctorID] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(drBarcode[BarcodeTable.Patient.Doctor].ToString());
            //        //new DictDoctor().GetDocByName(drBarcode[BarcodeTable.Patient.Doctor].ToString());
            //    }
            //}

            if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.DoctorID]))//如果医生工号不为空
            {
                //string doc_id = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(drBarcode[BarcodeTable.Patient.DoctorID].ToString());

                //用医生工号获取医生ID
                //drPatInfo[PatientTable.DoctorID] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(drBarcode[BarcodeTable.Patient.DoctorID].ToString());
                //new DictDoctor().GetDocIDByCode(drBarcode[BarcodeTable.Patient.DoctorID].ToString());

                drPatInfo[PatientTable.DoctorID] = drBarcode[BarcodeTable.Patient.DoctorID].ToString();
            }
            else
            {
                if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.Doctor]))//如果医生姓名不为空,则用医生姓名查找出医生code
                {
                    drPatInfo[PatientTable.DoctorID] = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(drBarcode[BarcodeTable.Patient.Doctor].ToString());
                    //new DictDoctor().GetDocByName(drBarcode[BarcodeTable.Patient.Doctor].ToString());
                }
            }

            //开单医生姓名
            drPatInfo["pat_doc_name"] = drBarcode[BarcodeTable.Patient.Doctor];
            drPatInfo["doc_name"] = drBarcode[BarcodeTable.Patient.Doctor];

            //临床诊断
            drPatInfo[PatientTable.Diag] = drBarcode[BarcodeTable.Patient.Diagnosis];

            if (drBarcode["bc_birthday"] != DBNull.Value)//出生日期
            {
                drPatInfo["pat_birthday"] = Convert.ToDateTime(drBarcode["bc_birthday"]);
            }

            ////标本送检日期
            //if (!Compare.IsEmpty(drBarcode[BarcodeTable.Patient.SampleSendDate]))
            //{
            //    drPatInfo[PatientTable.SampleSendDate] = drBarcode[BarcodeTable.Patient.SampleSendDate];
            //}
            //else
            //{
            //    drPatInfo[PatientTable.SampleSendDate] = DateTime.Now;
            //}

            //标本状态
            //drPatInfo["pat_rem"] = '6';//TO-DO:写死 状态：6为检验中

            //条码状态
            drPatInfo["bc_status"] = drBarcode[BarcodeTable.Patient.StatusCode];

            if (!drPatInfo.Table.Columns.Contains("bc_ctype"))
            {
                drPatInfo.Table.Columns.Add("bc_ctype");
            }
            drPatInfo["bc_ctype"] = drBarcode["bc_ctype"];
            //打印标志
            drPatInfo["bc_print_flag"] = drBarcode["bc_print_flag"];




            //标本类别
            drPatInfo["sam_name"] = drBarcode["bc_sam_name"];
            drPatInfo["pat_sam_id"] = drBarcode["bc_sam_id"];


            //检查类型
            if (drBarcode["bc_urgent_flag"] != DBNull.Value && Convert.ToBoolean(drBarcode["bc_urgent_flag"]) == true)
            {
                drPatInfo["pat_ctype"] = "2";
            }
            else
            {
                drPatInfo["pat_ctype"] = "1";
            }

            //检查类型
            if (drBarcode.Table.Columns.Contains("bc_urgent_status") && drBarcode["bc_urgent_status"] != DBNull.Value && drBarcode["bc_urgent_status"].ToString() == "2")
            {
                drPatInfo["pat_ctype"] = "4";
            }




            //+++++++++edit by sink 2010-9-26 ++++++++++++
            //自定义ID
            if (!Compare.IsNullOrDBNull(drBarcode["bc_pid"]))
            {
                drPatInfo["pat_pid"] = drBarcode["bc_pid"].ToString();
            }

            //唯一号UPID 目前滨海使用
            if (!Compare.IsNullOrDBNull(drBarcode["bc_upid"]))
            {
                drPatInfo["pat_upid"] = drBarcode["bc_upid"].ToString();
            }


            //人员身份
            if (!Compare.IsNullOrDBNull(drBarcode["bc_identity"]))
            {
                drPatInfo["pat_identity"] = drBarcode["bc_identity"];
            }

            //保存拆分大组合(特殊合并)ID
            if (!Compare.IsNullOrDBNull(drBarcode["bc_merge_comid"]))
            {
                drPatInfo["bc_merge_comid"] = drBarcode["bc_merge_comid"].ToString();
            }

            //申请单号
            if (
                drPatInfo.Table.Columns.Contains("pat_app_no")
                && drBarcode.Table.Columns.Contains("bc_app_no")
                && !Compare.IsNullOrDBNull(drBarcode["bc_app_no"])
                )
            {
                drPatInfo["pat_app_no"] = drBarcode["bc_app_no"].ToString();
            }

            //费用类别
            drPatInfo["pat_fee_type"] = drBarcode["bc_fee_type"];

            //体检id
            drPatInfo["pat_emp_id"] = drBarcode["bc_emp_id"];

            if (
                drPatInfo.Table.Columns.Contains("pat_emp_company_name")
                && drBarcode.Table.Columns.Contains("bc_emp_company_name"))
            {
                //体检id
                drPatInfo["pat_emp_company_name"] = drBarcode["bc_emp_company_name"];
            }

            //如果体检ID不为空，则更新病人来源为体检
            if (!Compare.IsEmpty(drBarcode["bc_emp_id"]))
            {
                drPatInfo["pat_ori_id"] = "109";
                drPatInfo["ori_name"] = "体检";
            }


            dtLisPat.Rows.Add(drPatInfo);
        }

        /// <summary>
        /// 根据病人ID类型id获取病人ID类型
        /// </summary>
        /// <param name="pat_no_id"></param>
        /// <returns></returns>
        public string GetPatNoType(string pat_no_id)
        {
            string sqlSelect = string.Format("select top 1 no_code from dict_no_type where no_id='{0}'", pat_no_id);
            DBHelper helper = new DBHelper();
            object objResult = helper.ExecuteScalar(sqlSelect);
            if (objResult != null && objResult != DBNull.Value)
            {
                return objResult.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据登陆号与权限代码判断是否有此权限
        /// </summary>
        /// <param name="loginId">登陆号</param>
        /// <param name="funcCode">用户权限代码</param>
        /// <returns></returns>
        public bool GetUserHaveFunctionByCode(string loginId, string funcCode)
        {
            bool blnRv = false;

            if (!string.IsNullOrEmpty(loginId) && !string.IsNullOrEmpty(funcCode))
            {
                if (loginId != "admin")//非admin用户,验证权限
                {
                    string sqlSelect = string.Format(@"select count(powerfuncinfo.funcInfoId) as funcCount
from poweruserinfo
inner join poweruserrole on poweruserinfo.userInfoId=poweruserrole.userInfoId
inner join powerrolefunc on powerrolefunc.roleInfoId=poweruserrole.roleInfoId
inner join powerfuncinfo on powerfuncinfo.funcInfoId=powerrolefunc.funcInfoId
where poweruserinfo.loginId='{0}'
and powerfuncinfo.funcCode='{1}'", loginId, funcCode);
                    DBHelper helper = new DBHelper();
                    object objResult = helper.ExecuteScalar(sqlSelect);
                    if (objResult != null && objResult != DBNull.Value && (!objResult.Equals(0)))
                    {
                        blnRv = true;
                    }
                }
                else
                {
                    blnRv = true;
                }
            }

            return blnRv;
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="pat_id">病人ID</param>
        /// <param name="item_ecd">项目编号</param>
        /// <returns></returns>
        public int DeleteItem(string pat_id, string item_ecd)
        {
            string sqlDelItem = string.Format("delete from resulto where res_id='{0}' and res_itm_ecd='{1}'", pat_id, item_ecd);

            DBHelper helper = new DBHelper();
            int iRowAffact = helper.ExecuteNonQuery(sqlDelItem);
            return iRowAffact;
        }

        /// <summary>
        /// 批量删除检验项目
        /// </summary>
        /// <param name="ListPatIDs"></param>
        /// <param name="listDelItmIDs"></param>
        /// <returns></returns>
        public int DeleteResultoItemBatch(List<string> ListPatIDs, List<string> listDelItmIDs)
        {
            int iRowAffact = 0;

            try
            {
                string sqlWhereItmIDs = "";
                for (int i = 0; i < listDelItmIDs.Count; i++)
                {
                    if (string.IsNullOrEmpty(sqlWhereItmIDs))
                    {
                        sqlWhereItmIDs = "'" + listDelItmIDs[i] + "'";
                    }
                    else
                    {
                        sqlWhereItmIDs += ",'" + listDelItmIDs[i] + "'";
                    }
                }

                //如果项目ID集为空,则返回-2
                if (string.IsNullOrEmpty(sqlWhereItmIDs)) { return -2; }

                for (int j = 0; j < ListPatIDs.Count; j++)
                {
                    string sqlDelBatch = string.Format(@"update resulto set res_flag=0 
where res_id='{0}'
and res_flag=1 and res_itm_id in({1})
and exists(select top 1 1 from patients where pat_id=res_id and pat_flag=0)", ListPatIDs[j], sqlWhereItmIDs);

                    DBHelper helper = new DBHelper();
                    int iRowAffact_p = helper.ExecuteNonQuery(sqlDelBatch);
                    if (iRowAffact_p > 0)
                    {
                        iRowAffact += iRowAffact_p;
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "批量删除检验项目", ex.ToString());
                throw ex;
            }
            return iRowAffact;
        }

        /// <summary>
        /// 获取指定仪器和日期的所有样本和结果
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="includeNonItrResult">是否包含非仪器结果</param>
        /// <returns></returns>
        public DataTable GetInstructmentResult(DateTime date, string itr_id, string filter, bool includeNonItrResult)
        {
            string res_type_where = string.Empty;
            if (includeNonItrResult == false)
            {
                res_type_where = " and res_type='1' ";
            }

            if (filter == null || filter == string.Empty)
            {
                filter = " 1=1 ";
            }

            filter = filter.Replace("res_sid_int", "cast(res_sid as bigint)");

            string sql = string.Format(@"
select
res_key,
res_id, 
res_sid, 
cast(res_sid as bigint) as res_sid_int,
res_itm_id,
res_itm_ecd, 
dict_item.itm_name as res_itm_name,
res_chr, 
res_date, 
dict_instrmt.itr_name,
dict_instrmt.itr_mid,
pat_flag = case when patients.pat_flag is null then 0
                else patients.pat_flag end
FROM resulto 
left join dict_instrmt on dict_instrmt.itr_id = resulto.res_itr_id
left join dict_item on resulto.res_itm_id = dict_item.itm_id
left join patients on patients.pat_id = resulto.res_id
where 
resulto.res_itr_id = '{0}'
and (res_date >=@res_date and res_date <@res_date1  )
and ({1}) and res_flag = 1
{2}
order by cast(res_sid as bigint) asc,dict_item.itm_seq
", itr_id, filter, res_type_where);

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("res_date", date.Date);
            cmd.Parameters.AddWithValue("res_date1", date.Date.AddDays(1));

            DBHelper helper = new DBHelper();
            DataTable dtResult = helper.GetTable(cmd);

            dtResult.TableName = "GetInstructmentResult";

            return dtResult;
        }

        /// <summary>
        /// 获取指定仪器和日期的所有样本和结果(从中间表获取)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public DataTable GetInstructmentResult2(DateTime date, string itr_id, int result_type, string filter)
        {
            DateTime dtSearchDate = date.Date;
            DataTable dtResult;

            if (string.IsNullOrEmpty(filter))
            {
                filter = " 1=1 ";
            }

            string sqlSelect;
            if (result_type == 0)
            {
                sqlSelect = string.Format(@"
select
    resulto_mid.*,
    dict_mitm_no.mit_itm_id as itm_id,
    dict_item.itm_ecd,
    msg = case when dict_mitm_no.mit_itm_id is null then '通道码设置错误'
               else '' end

from resulto_mid
    left outer join dict_mitm_no on (dict_mitm_no.mit_cno = resulto_mid.res_cno and resulto_mid.res_itr_id = dict_mitm_no.mit_itr_id and mit_del='0')
    left outer join dict_item on dict_mitm_no.mit_itm_id = dict_item.itm_id
where
    (res_date >=@res_date and res_date <@res_date1)
    and res_itr_id = '{0}'
    and {1}
--order by res_date desc
", itr_id, filter);

            }
            else if (result_type == 1)
            {
                sqlSelect = string.Format(@"
select
    res_key,
    res_itr_id,
    res_itr_ori_id,
    res_sid,
    res_cno = '',
    res_chr as res_chr_a,
    res_od_chr as res_chr_b,
    res_chr_c = '',
    res_chr_d = '',
    res_date,
    res_id,
    res_itm_id as itm_id,
    res_itm_ecd as itm_ecd,
    msg = ''
from resulto
    left join patients on patients.pat_id = resulto.res_id
where
    --res_flag = 1 and
    res_date >=@res_date and res_date <@res_date1
    and res_itr_id = '{0}'
    and {1}
--order by res_date desc
", itr_id, filter);


                #region MyRegion
                //需要优化一下语句
                //select
                //res_key,
                //res_itr_id,
                //res_itr_ori_id,
                //res_sid,
                //res_cno = '',
                //res_chr as res_chr_a,
                //res_od_chr as res_chr_b,
                //res_chr_c = '',
                //res_chr_d = '',
                //res_date,
                //res_id,
                //res_itm_id as itm_id,
                //res_itm_ecd as itm_ecd,
                //msg = ''
                //from resulto
                //    left join patients on patients.pat_id = resulto.res_id
                //where
                //    res_flag = 1 
                //    and ((res_date >='2012-03-06' and res_date <'2012-03-07') or (pat_date >='2012-03-06' and pat_date <'2012-03-07'))
                //    and res_itr_id = '10022'
                //    and res_sid like '%1123%'
                //order by res_date desc 
                #endregion

            }

            else
            {
                sqlSelect = string.Format(@"
select
    resulto_mid.*,
    dict_mitm_no.mit_itm_id as itm_id,
    dict_item.itm_ecd,
    msg = (case when dict_mitm_no.mit_itm_id is null or dict_mitm_no.mit_del = '1' then '通道码设置错误'
           when dict_mitm_no.mit_del = '1' then '通道码已失效'
               else '' end)

from resulto_mid
    left outer join dict_mitm_no on (dict_mitm_no.mit_cno = resulto_mid.res_cno and resulto_mid.res_itr_id = dict_mitm_no.mit_itr_id)
    left outer join dict_item on dict_mitm_no.mit_itm_id = dict_item.itm_id
where
    (res_date >=@res_date and res_date <@res_date1)
    and res_itr_id = '{0}'
    and {1}
--order by res_date desc
", itr_id, filter);
            }

            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.AddWithValue("res_date", date.Date);
            cmd.Parameters.AddWithValue("res_date1", date.Date.AddDays(1));

            DBHelper helper = new DBHelper();
            dtResult = helper.GetTable(cmd);

            if (!dtResult.Columns.Contains("res_sid_int"))
            {
                dtResult.Columns.Add("res_sid_int", typeof(double));
            }
            foreach (DataRow item in dtResult.Rows)
            {
                if (item["res_sid"] != null
                    && item["res_sid"] != DBNull.Value
                   && !string.IsNullOrEmpty(item["res_sid"].ToString())
                    )
                {
                    string sid = item["res_sid"].ToString();
                    double pat_sid_int = 0;
                    if (double.TryParse(sid, out pat_sid_int))
                    {
                        item["res_sid_int"] = pat_sid_int;
                    }
                }
            }

            dtResult.TableName = "GetInstructmentResult2";

            return dtResult;
        }

        /// <summary>
        /// 病人结果视窗
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public DataTable GetPatientsResultView(DateTime date, string itr_id)
        {
            //string sqlSelect = 
            return null;
        }
        #endregion

        #region 审核\报告相关

        /// <summary>
        /// 普通结果审核检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public EntityOperationResultList CommonAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.CommonAuditCheck(listPatientsID.ToArray(), checkType, caller);
        }

        /// <summary>
        /// 预报告检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public EntityOperationResultList EnableAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.EnableAuditCheck(listPatientsID.ToArray(), checkType, caller);
        }

        /// <summary>
        /// 描述结果审核检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public EntityOperationResultList DesctAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.DesctAuditCheck(listPatientsID.ToArray(), checkType, caller);
        }


        /// <summary>
        /// 取消二审前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList CommonUndoReoprtCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.CommonUndoReoprtCheck(listPatientsID.ToArray(), checkType, caller);
        }

        /// <summary>
        /// 取消二审前检查ForBabyFilter
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList CommonUndoReoprtCheckForBabyFilter(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.CommonUndoReoprtCheckForBabyFilter(listPatientsID.ToArray(), checkType, caller);
        }

        public void Audit(string PatientID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.Audit(new string[] { PatientID }, caller);
        }

        public void AuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.Audit(listPatientsID.ToArray(), caller);
        }

        public void UndoAudit(string PatientID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.UndoAudit(new string[] { PatientID }, caller);
        }

        public EntityOperationResultList UndoAuditBatch(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            EntityOperationResultList result = bll.UndoAudit(listPatientsID.ToArray(), caller);
            return result;
        }

        public void PreAuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.PreAuditBatch(listPatientsID.ToArray(), caller);
        }

        public EntityOperationResultList UndoPreAuditBatch(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            EntityOperationResultList result = bll.UndoPreAuditBatch(listPatientsID.ToArray(), caller);
            return result;
        }

        public void Report(string PatientID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.Report(new string[] { PatientID }, caller);
        }

        public EntityOperationResultList ReportBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            EntityOperationResultList ret = bll.Report(listPatientsID.ToArray(), caller);
            return ret;
        }

        public void UndoReport(string PatientID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            bll.UndoReport(new string[] { PatientID }, caller);
        }

        public EntityOperationResultList UndoReportBatch(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            EntityOperationResultList result = bll.UndoReport(listPatientsID.ToArray(), caller);
            return result;
        }
        public EntityOperationResultList UndoReportBatchForBabyFilter(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            EntityOperationResultList result = bll.UndoReportForBabyFilter(listPatientsID.ToArray(), caller);
            return result;
        }
        public EntityOperationResultList BabyFilterAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType,
                                                              EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            return bll.BabyFilterAuditCheck(listPatientsID.ToArray(), checkType, caller);
        }

        public void AuditBatchForBabyFilter(IEnumerable<string> listPatientsID, IEnumerable<string> recheckID, DateTime checkDate,
                                            EntityRemoteCallClientInfo caller)
        {
            LabAuditBiz bll = new LabAuditBiz();
            //bll.AuditForBabyFilter(listPatientsID.ToArray(), recheckID, checkDate, caller);
        }



        public EntityOperationResultList UndoAuditBatchForBabyFilter(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            throw new NotImplementedException();
        }


        public EntityOperationResultList ReportBatchForBabyFilter(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 获取描述报告内容
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetPatDescResult(string pat_id)
        {
            string sqlSelect = string.Format(@"select top 1 * from {0} where bsr_res_flag=1 and bsr_id='{1}'", PatientTable.PatientDescResultTableName, pat_id);
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);
            dt.TableName = PatientTable.PatientDescResultTableName;
            return dt;
        }

        /// <summary>
        /// 更新描述报告
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatDescResult(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.UpdatePatDescResult(caller, dsData);
        }

        public EntityOperationResult UpdatePatCommonResult(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.UpdatePatCommonResult(caller, dsData);
        }

        public EntityOperationResult UpdatePatCommonResultForBf(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            PatUpdateBLL bll = new PatUpdateBLL();
            return bll.UpdatePatCommonResultForBf(caller, dsData);
        }


        /// <summary>
        /// 获取病人资料表结构
        /// </summary>
        /// <returns></returns>
        public DataTable GetPatientInfoStruct()
        {
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable("select top 0 * from patients");
            dt.TableName = PatientTable.PatientInfoTableName;
            return dt;
        }

        /// <summary>
        /// 获取病人结果表结构
        /// </summary>
        /// <returns></returns>
        public DataTable GetPatientResultStruct()
        {
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable("select top 0 * from resulto");
            dt.TableName = PatientTable.PatientResultTableName;
            return dt;
        }

        /// <summary>
        /// 获取病人检验组合表结构
        /// </summary>
        /// <returns></returns>
        public DataTable GetPatientCombineStruct()
        {
            DataTable dt = DataTableStructCache.Current.GetTableStruct(PatientTable.PatientCombineTableName);
            if (dt == null)
            {
                DBHelper helper = new DBHelper();
                dt = helper.GetTable("select top 0 *,pat_com_name='', cast(null as int) as com_seq from patients_mi");
                dt.TableName = PatientTable.PatientCombineTableName;

                if (dt != null && dt.Columns.Contains("pat_key"))//排除pat_key字段
                {
                    dt.Columns.Remove("pat_key");
                    dt.AcceptChanges();
                }

                DataTableStructCache.Current.AddTableStruct(dt);
            }
            return dt;
        }

        /// <summary>
        /// 根据大组合(特殊合并)ID获取已上机病人信息
        /// </summary>
        /// <param name="bc_merge_comid"></param>
        /// <returns></returns>
        private DataTable GetPatientByMergeComid(string bc_merge_comid)
        {
            DataTable dt = null;

            string sql = string.Format(@"select top 1 pat_id,pat_itr_id,pat_sid,pat_date,dict_instrmt.itr_mid from patients with(NOLOCK)
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
where pat_bar_code=(
select top 1 bc_cname.bc_bar_code from bc_patients
inner join bc_cname on bc_cname.bc_bar_no=bc_patients.bc_bar_code
where bc_patients.bc_del='0' and bc_cname.bc_del='0' and bc_cname.bc_flag=1
and bc_patients.bc_merge_comid='{0}'
)", bc_merge_comid);

            DBHelper helper = new DBHelper();
            dt = helper.GetTable(sql);
            if (dt != null)
            {
                dt.TableName = "PatMergeComid";
            }

            return dt;
        }

        #region IPatientEnter 成员


        public IEnumerable<lis.dto.BarCodeEntity.BCSignEntity> GetBarCodeStatus(string barCode)
        {
            string sqlSelect = string.Format(@"
select
bc_sign.*,
bc_status.bc_cname as bc_status_name
from bc_sign WITH(NOLOCK)
left join bc_status on bc_status.bc_name = bc_sign.bc_status
where bc_sign.bc_bar_code = '{0}'
order by bc_date asc
", barCode);

            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);

            return lis.dto.BarCodeEntity.BCSignEntity.FromDataTable(dt);
        }

        #endregion

        #region IPatientEnter Delete

        ///// <summary>
        /////  删除普通病人资料
        ///// </summary>
        ///// <param name="pat_id"></param>
        ///// <param name="delWithResult"></param>
        ///// <returns></returns>
        //public EntityOperationResult DelPatCommonResult(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult)
        //{
        //    PatDeleteBLL bll = new PatDeleteBLL();
        //    return bll.DelPatCommonResult(caller, pat_id, delWithResult);
        //}

        /// <summary>
        /// 删除描述报告病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatDescResult(EntityRemoteCallClientInfo caller, string pat_id, bool delWithResult)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.DelPatDescResult(caller, pat_id, delWithResult);
        }

        /// <summary>
        /// 批量删除普通病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatCommonResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult, bool canDeleteAudited)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.BatchDelPatCommonResult(caller, listPatID, delWithResult, canDeleteAudited);
        }

        /// <summary>
        /// 批量删除描述报告病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatDescResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.BatchDelPatDescResult(caller, listPatID, delWithResult);
        }

        /// <summary>
        /// 删除细菌病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatBacResult(string pat_id, bool delWithResult)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.DelPatBacResult(pat_id, delWithResult);
        }

        /// <summary>
        /// 批量删除细菌病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BacthDelPatBacResult(List<string> listPatID, bool delWithResult)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.BacthDelPatBacResult(listPatID, delWithResult);
        }

        /// <summary>
        /// 批量删除普通病人资料
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatCommonResultForBabyFilter(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult, bool canDeleteAudited)
        {
            PatDeleteBLL bll = new PatDeleteBLL();
            return bll.BatchDelPatCommonResultForBabyFilter(caller, listPatID, delWithResult, canDeleteAudited);
        }
        #endregion


        /// <summary>
        /// 样本进程
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="patDate"></param>
        /// <returns></returns>
        public DataSet Pat_Monitor(string type_id, string itr_id, DateTime patDate)
        {
            try
            {
                DataSet dsResult = new DataSet();

                DBHelper helper = new DBHelper();

                //仪器的查询条件
                string sql_where_itr = string.Empty;// "'" + itr_id + "'";

                if (string.IsNullOrEmpty(itr_id))
                {
                    DataTable dtItr = DictInstructmentBLL.NewInstance.GetCTypeItr(type_id);

                    bool needComma = false;
                    foreach (DataRow rowItr in dtItr.Rows)
                    {
                        if (needComma)
                        {
                            sql_where_itr += ",";
                        }

                        sql_where_itr += string.Format("'{0}'", rowItr["itr_id"]);

                        needComma = true;
                    }

                    if (sql_where_itr.Length == 0)
                    {
                        sql_where_itr = " -1 ";
                    }
                }
                else
                {
                    sql_where_itr = "'" + itr_id + "'";
                }

                DataTable dtNormal;

                //获取有结果的病人资料
                string sqlNormal = string.Format(@"
SELECT distinct
    pat_id,
    pat_itr_id,
    dict_instrmt.itr_mid,
    pat_sid,
    cast(pat_sid as bigint) as pat_sid_int 
    ,pat_name,
    pat_sex,
    pat_dep_name,
    pat_flag,
    pat_apply_date,
    pat_flag_name = case when pat_flag = '0' or pat_flag is null or pat_flag = '' then '未审核'
                         when pat_flag = '1' then '已审核'
                         when pat_flag = '2' then '已报告'
                         when pat_flag = '4' then '已打印' end,
                    
    pat_state_name = case when pat_flag = '0' or pat_flag is null or pat_flag = '' then '未审核'
                         when pat_flag = '1' then '未报告'
                         when pat_flag = '2' then '已报告'
                         when pat_flag = '4' then '已打印' end
FROM patients 
    inner join resulto on patients.pat_id = resulto.res_id and resulto.res_flag = 1
    inner join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
WHERE
    pat_itr_id in ({0})
    and (pat_date >='{1}' and pat_date <'{2}')
    order by pat_itr_id,cast(pat_sid as bigint)
    ", sql_where_itr, patDate.ToString("yyyy-MM-dd"), patDate.AddDays(1).ToString("yyyy-MM-dd"));

                dtNormal = helper.GetTable(sqlNormal);
                dtNormal.TableName = "normal";
                dsResult.Tables.Add(dtNormal);


                if (!string.IsNullOrEmpty(itr_id))
                {
                    sql_where_itr = "";
                    DataTable dtItr = DictInstructmentBLL.NewInstance.GetCTypeItr(type_id);

                    bool needComma = false;
                    foreach (DataRow rowItr in dtItr.Rows)
                    {
                        if (needComma)
                        {
                            sql_where_itr += ",";
                        }

                        sql_where_itr += string.Format("'{0}'", rowItr["itr_id"]);

                        needComma = true;
                    }

                    if (sql_where_itr.Length == 0)
                    {
                        sql_where_itr = " -1 ";
                    }
                }


//                string sqlNoResult = string.Format(@"


//select sum(recvQty) as recvQty,sum(noResQty) as noResQty 
//,sum(noauditQty) as noauditQty 
//,sum(noreportQty) as noreportQty 
//,sum(reportQty) as reportQty 
//from
//(
//select COUNT(1) as recvQty,0 as noResQty
//,0 as noauditQty
//,0 as noreportQty
//,0 as reportQty
//  from bc_patients 
//where  (bc_date >='{1}' and bc_date <'{2}') 
//and bc_ctype='{3}' and bc_receiver_flag=1 and bc_del = '0'

//union all 

//SELECT 0 as recvQty,COUNT(1) as noResQty
//,0 as noauditQty
//,0 as noreportQty
//,0 as reportQty
//FROM patients 
//left join resulto on patients.pat_id = resulto.res_id
//where 
//resulto.res_id is null
//and pat_itr_id in ({0})
//and (pat_date >='{1}' and pat_date <'{2}')


//union all 

//SELECT 0 as recvQty,0 as noResQty
//,COUNT(1) as noauditQty
//,0 as noreportQty
//,0 as reportQty
//FROM patients 
//where 
//(patients.pat_flag is null or patients.pat_flag=0)
//and pat_itr_id in ({0})
//and (pat_date >='{1}' and pat_date <'{2}')

//union all 

//SELECT 0 as recvQty,0 as noResQty
//,0 as noauditQty
//,COUNT(1) as noreportQty
//,0 as reportQty
//FROM patients 
//where 
// patients.pat_flag =1
//and pat_itr_id in ({0})
//and (pat_date >='{1}' and pat_date <'{2}')

//union all 

//SELECT 0 as recvQty,0 as noResQty
//,0 as noauditQty
//,0 as noreportQty
//,COUNT(1) as reportQty
//FROM patients 
//where 
// patients.pat_flag in (2,4)
//and pat_itr_id in ({0})
//and (pat_date >='{1}' and pat_date <'{2}')

//)qt

//", sql_where_itr, patDate.ToString("yyyy-MM-dd"), patDate.AddDays(1).ToString("yyyy-MM-dd"), type_id);
//                                DataTable dtNoResult = helper.GetTable(sqlNoResult);
//                                dtNoResult.TableName = "noresult";
//                                dsResult.Tables.Add(dtNoResult);


                //获取没有结果的病人资料
                string sqlNoResult = string.Format(@"
                SELECT pat_id,pat_sid,cast(pat_sid as bigint) as pat_sid_int,pat_name,pat_sex,pat_dep_name,pat_flag ,dict_instrmt.itr_mid
                FROM patients 
                left join resulto on patients.pat_id = resulto.res_id
                inner join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
                where 
                resulto.res_id is null
                and pat_itr_id in ({0})
                and (pat_date >='{1}' and pat_date <'{2}')
                order by cast(pat_sid as bigint)
                ", sql_where_itr, patDate.ToString("yyyy-MM-dd"), patDate.AddDays(1).ToString("yyyy-MM-dd"));
                DataTable dtNoResult = helper.GetTable(sqlNoResult);
                dtNoResult.TableName = "noresult";
                dsResult.Tables.Add(dtNoResult);

                //获取没有病人的结果
                string sqlNoPat = string.Format(@"
                SELECT res_id,  res_sid 
                ,res_chr ,res_itm_ecd ,dict_instrmt.itr_mid
                FROM resulto 
                left join patients on patients.pat_id = resulto.res_id
                inner join dict_instrmt on dict_instrmt.itr_id = resulto.res_itr_id
                where patients.pat_id is null and resulto.res_id is not null
                and resulto.res_itr_id in ({0})
                and (resulto.res_date >='{1}' and resulto.res_date <'{2}') and resulto.res_flag = 1
                order by res_itr_id, len(res_sid),res_sid
                ", sql_where_itr, patDate.ToString("yyyy-MM-dd"), patDate.AddDays(1).ToString("yyyy-MM-dd"));
                DataTable dtNoPat = helper.GetTable(sqlNoPat);
                dtNoPat.TableName = "nopat";
                dsResult.Tables.Add(dtNoPat);

                string sqlAllType = string.Format(@"(SELECT distinct
    pat_id,
    pat_itr_id,
    dict_instrmt.itr_mid,
    pat_sid,
    cast(pat_sid as bigint) as pat_sid_int 
    ,pat_name,
    pat_sex,
    pat_dep_name,
    pat_flag,
    pat_apply_date,
    patients.pat_c_name,
    pat_flag_name = case when pat_flag = '0' or pat_flag is null or pat_flag = '' then '未审核'
                         when pat_flag = '1' then '已审核'
                         when pat_flag = '2' then '已报告'
                         when pat_flag = '4' then '已打印' end,
                    
    pat_state_name = case when pat_flag = '0' or pat_flag is null or pat_flag = '' then '未审核'
                         when pat_flag = '1' then '未报告'
                         when pat_flag = '2' then '未打印'
                         when pat_flag = '4' then '已打印' end
FROM patients
    inner join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
    left join dict_type on dict_instrmt.itr_type=dict_type.type_id
WHERE
    dict_instrmt.itr_type = '{0}'
    and (pat_date >='{1}' and pat_date <'{2}')
    )
union
  (SELECT distinct
    '' as pat_id,
    '' as pat_itr_id,
    '' as itr_mid,
    '' as pat_sid,
    '' as pat_sid_int 
    ,bc_name as pat_name,
    bc_sex as pat_sex,
    bc_d_name as pat_dep_name,
    '' as pat_flag,
    bc_receiver_date as pat_apply_date,
    bc_his_name as pat_c_name,
    pat_flag_name = '已签收',
                    
    pat_state_name = '已签收未登记'
FROM bc_patients
WHERE
    bc_ctype = '{0}'
    and (bc_receiver_date >='{1}' and bc_receiver_date <'{2}') and bc_status='5')", type_id, patDate.ToString("yyyy-MM-dd"), patDate.AddDays(1).ToString("yyyy-MM-dd"));
                DataTable dtAllType = helper.GetTable(sqlAllType);
                dtAllType.TableName = "AllTypepat";
                dsResult.Tables.Add(dtAllType);

                return dsResult;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取样本进程信息出错", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 更新病人状态为已打印,更新打印时间
        /// </summary>
        /// <param name="patIDs"></param>
        public void UpdatePrintState(IEnumerable<string> patIDs)
        {
            try
            {
                string sqlUpdate = "update patients set pat_flag = @pat_flag , pat_prt_date = getdate() where pat_id = @pat_id";

                string sqlUpdateBarcode = @"insert into bc_sign 
                                            (bc_date, bc_login_id, bc_name,  bc_status, bc_bar_no, bc_bar_code, pat_id)
                                            (
                                            select 
                                            getdate() bc_date,
                                            '' bc_login_id,--pat_report_code bc_login_id,
                                            '' bc_name,--PowerUserInfo.username bc_name,
                                            '100' bc_status,
                                            pat_bar_code bc_bar_no,
                                            pat_bar_code bc_bar_code,
                                            pat_id 
                                            from 
                                            patients 
                                            --LEFT OUTER JOIN PowerUserInfo  on patients.pat_report_code=PowerUserInfo.loginId
                                            where pat_id='{0}'
                                            )";

                SqlCommand cmd = new SqlCommand(sqlUpdate);

                cmd.Parameters.AddWithValue("pat_flag", LIS_Const.PATIENT_FLAG.Printed);
                SqlParameter pPatID = cmd.Parameters.AddWithValue("pat_id", -1);
                pPatID.DbType = DbType.AnsiString;

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    foreach (string patID in patIDs)
                    {
                        cmd.Parameters["pat_id"].Value = patID;
                        helper.ExecuteNonQuery(cmd);
                        helper.ExecuteNonQuery(string.Format(sqlUpdateBarcode, patID));
                    }

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "UpdatePrintState", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 更新病人状态为已打印,更新打印时间(带操作者)
        /// </summary>
        /// <param name="patIDs"></param>
        /// <param name="OperatorID">操作者ID</param>
        /// <param name="OperatorName">操作者名称</param>
        public void UpdatePrintState_whitOperator(IEnumerable<string> patIDs, string OperatorID, string OperatorName, string strPlace)
        {
            try
            {
                string sqlUpdate = "update patients set pat_flag = @pat_flag , pat_prt_date = getdate() where pat_id = @pat_id and   (pat_flag=2 or pat_flag=4) ";

                string sqlUpdateBarcode = @"insert into bc_sign 
                                            (bc_date, bc_login_id, bc_name,  bc_status, bc_bar_no, bc_bar_code,bc_place,bc_remark, pat_id)
                                            (
                                            select 
                                            getdate() bc_date,
                                            '{1}' bc_login_id,
                                            '{2}' bc_name,
                                            '100' bc_status,
                                            pat_bar_code bc_bar_no,
                                            pat_bar_code bc_bar_code,
                                            '{3}' bc_place,
                                            '检验报告管理模块' bc_remark,
                                            pat_id 
                                            from 
                                            patients 
                                            where pat_id='{0}'
                                            )";

                SqlCommand cmd = new SqlCommand(sqlUpdate);

                cmd.Parameters.AddWithValue("pat_flag", LIS_Const.PATIENT_FLAG.Printed);
                SqlParameter pPatID = cmd.Parameters.AddWithValue("pat_id", -1);
                pPatID.DbType = DbType.AnsiString;

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    foreach (string patID in patIDs)
                    {
                        cmd.Parameters["pat_id"].Value = patID;
                        helper.ExecuteNonQuery(cmd);

                        //注释by林志宏2013-11-20：下面这句话会报错：未将对象引用到对象的实例
                        //c#：string.Format内的参数不需要再ToString()，string.Format系统自动会将null转换成""
                        //请检查是哪个参数为空值
                        helper.ExecuteNonQuery(string.Format(sqlUpdateBarcode, patID, OperatorID, OperatorName, strPlace));
                    }

                    helper.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "UpdatePrintState_whitOperator", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 保存描述模板
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="flag">类型</param>
        /// <param name="owner">拥有者(用户登录ID)</param>
        public void SaveDescriptionTemplete(string content, string flag, string owner)
        {
            string tableName = "dict_bscripe";
            string sqlInsert = "insert into dict_bscripe(br_id,br_scripe,br_flag,br_owner) values(@br_id,@br_scripe,@br_flag,@br_owner)";
            SqlCommand cmdInsert = new SqlCommand(sqlInsert);

            //using (DBHelper helper = DBHelper.BeginTransaction())
            //{
            DBHelper helper = new DBHelper();
            string id = SaveTableNextID(tableName, helper);

            cmdInsert.Parameters.AddWithValue("br_id", id);
            cmdInsert.Parameters.AddWithValue("br_scripe", content);
            cmdInsert.Parameters.AddWithValue("br_flag", flag);
            cmdInsert.Parameters.AddWithValue("br_owner", owner);

            helper.ExecuteNonQuery(cmdInsert);

            //    helper.Commit();
            //}
        }

        /// <summary>
        /// 获取描述模板
        /// </summary>
        /// <param name="type"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public DataTable GetDescriptionTemplete(string type, string owner)
        {
            string sql = string.Format(@"
select 
* 
from dict_bscripe 
where 
(br_owner ='{0}' or br_owner is null or br_owner='')
and br_flag='{1}' order by cast(br_seq as int) asc
", owner, type);

            DataTable dt = new DBHelper().GetTable(sql);
            dt.TableName = "GetDescriptionTemplete";
            return dt;
        }

        /// <summary>
        /// 从SysTableId表中获取指定表的下一个ID,并更新为新的ID,返回新ID
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private string SaveTableNextID(string tableName, DBHelper transHelper)
        {
            string sqlGetCurrID = string.Format("select top 1 curId from SysTableId where tableName='{0}'", tableName);

            object objCurrID = transHelper.ExecuteScalar(sqlGetCurrID);

            string nextID = "1";

            string sql = string.Empty;
            if (objCurrID == null)
            {
                sql = string.Format("insert into SysTableId(tableName,curId) values('{0}'.'{1}')", tableName, nextID);
            }
            else
            {
                if (objCurrID != DBNull.Value)
                {
                    nextID = (Convert.ToInt32(objCurrID) + 1).ToString();
                }

                sql = string.Format("update SysTableId set curId='{0}' where tableName='{1}'", nextID, tableName);
            }

            transHelper.ExecuteNonQuery(sql);

            return nextID;
        }

        /// <summary>
        /// 保存-批量添加默认阴性结果(微生物)
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SaveBatchAddBaRlts(DataSet ds)
        {
            bool btn = false;
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt_btype = ds.Tables["ba_rlts"];
                    if (dt_btype != null && dt_btype.Rows.Count > 0)
                    {
                        SqlHelper sqlhelper = new SqlHelper();
                        foreach (DataRow dr in dt_btype.Rows)
                        {
                            string sql = @"if(exists(select top 1 1 from patients where pat_id='{0}' and pat_flag=0)
and not exists(select top 1 1 from dbo.cs_rlts where bsr_id='{0}')
and not exists(select top 1 1 from ba_rlts where bar_id='{0}' and bar_bid='{3}'))
begin
INSERT INTO ba_rlts
           ([bar_id]
           ,[bar_mid]
           ,[bar_date]
           ,[bar_sid]
           ,[bar_bid]
           ,[bar_bcnt]
           ,[bar_scripe])
     VALUES
           ('{0}'
           ,'{1}'
           ,getdate()
           ,'{2}'
           ,'{3}'
           ,''
           ,''
           )
end";

                            sql = string.Format(sql, dr["bar_id"], dr["bar_mid"], dr["bar_sid"], dr["bar_bid"]);

                            sqlhelper.ExecuteNonQuery(sql);
                        }
                        btn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
            return btn;


        }


        #region IPatientEnterBLL 成员

        /// <summary>
        /// 根据条码号查询住院号
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public DataSet GetPatientsRecordByBarCode(string barCode)
        {
            DataSet dsResult = new DataSet();
            string sqlPat = string.Format("select dict_instrmt.itr_id,dict_instrmt.itr_name,pat_date,pat_sid,pat_c_name from patients left join  dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id where pat_bar_code='{0}' ", barCode);
            DataTable dtNoPat = new DBHelper().GetTable(sqlPat);
            dtNoPat.TableName = "patients";
            dsResult.Tables.Add(dtNoPat);
            return dsResult;
        }

        /// <summary>
        /// 细菌审核前检查 
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="isAudit"></param>
        /// <returns></returns>
        public EntityOperationResultList BacAuditCheck(IEnumerable<string> listPatientsID, string isAudit)
        {
            return new BacAuditBll().BatchAuditCheck(listPatientsID, isAudit);
        }

        /// <summary>
        /// 细菌取消报告前检查 
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type"></param>
        /// <returns></returns>
        public EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> listPatientsID, string oper_type)
        {
            return new BacAuditBll().BatchUndoReportCheck(listPatientsID, oper_type);
        }


        public string GetPatientState(string pat_id)
        {
            string ret = string.Empty;
            DataTable table = new PatCommonBll().GetPatientState(pat_id);

            if (table.Rows.Count > 0)
            {
                ret = table.Rows[0]["pat_flag"].ToString();
            }

            return ret;
        }

        /// <summary>
        /// 获取病人资料状态(多个)
        /// </summary>
        /// <param name="InPat_ids">IN的pat_id</param>
        /// <returns></returns>
        public DataTable GetPatientStateForIn(string InPat_ids)
        {
            DataTable table = new PatCommonBll().GetPatientStateForIn(InPat_ids);
            if (table != null)
            {
                table.TableName = "dt";
            }
            return table;
        }

        public string GetPatientStateForBabyFilter(string pat_id)
        {
            string ret = string.Empty;
            DataTable table = new PatCommonBll().GetPatientStateForBabyFilter(pat_id);

            if (table.Rows.Count > 0)
            {
                ret = table.Rows[0]["pat_flag"].ToString();
            }

            return ret;
        }


        public IEnumerable<BarcodePatientsExtent> GetBarCodeExtend(string barCode)
        {
            string sqlSelect = string.Format(@"
select *
from bc_patients_extend
where bc_bar_code = '{0}'
", barCode);

            EntityHelper helper = new EntityHelper();

            return helper.SelectMany<BarcodePatientsExtent>(sqlSelect);
        }
        public bool CheckCurUserCanAudit(string itr_ID, EnumOperationCode auditType, string loginID)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                return CacheUserInstrmtInfo.Current.CheckUserCanMgrIInstrmt2(loginID, itr_ID);

            }
            return true;
        }
        #endregion

        #region IPatientEnterBLL 成员


        /// <summary>
        /// 将报告单电子签章写入数据库
        /// </summary>
        /// <param name="p_dtbCASigncontent"></param>
        /// <returns></returns>
        public bool InsertReportCASignature(DataTable p_dtbCASigncontent)
        {

            return new dcl.svr.resultcheck.Updater.CASignature().InsertReportCASignature(p_dtbCASigncontent);

        }

        #endregion

        #region IPatientEnterBLL 成员


        public IEnumerable<BCSignEntity> GetReportStatus(string pat_id)
        {
            //            string sqlSelect = string.Format(@"
            //select
            //bc_sign.*,
            //bc_status.bc_cname as bc_status_name
            //from bc_sign WITH(NOLOCK)
            //left join bc_status on bc_status.bc_name = bc_sign.bc_status
            //where bc_sign.pat_id = '{0}'
            //or bc_sign.bc_bar_code=(select CASE WHEN  pat_bar_code is null or pat_bar_code ='' then '0x0xa' ELSE pat_bar_code end from patients  where pat_id='{0}')
            //order by bc_date asc
            //", pat_id);


            string sqlSelect = string.Format(@"
select
bc_sign.*,
bc_status.bc_cname as bc_status_name
from bc_sign WITH(NOLOCK)
inner join bc_status on bc_status.bc_name = bc_sign.bc_status
where bc_sign.pat_id = '{0}'

union

select
bc_sign.*,
bc_status.bc_cname as bc_status_name
from bc_sign WITH(NOLOCK)
inner join bc_status on bc_status.bc_name = bc_sign.bc_status
where bc_sign.bc_bar_code=(select CASE WHEN  pat_bar_code is null or pat_bar_code ='' then '0x0xa' ELSE pat_bar_code end from patients nolock  where pat_id='{0}')

order by bc_date asc
", pat_id);


            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);

            return lis.dto.BarCodeEntity.BCSignEntity.FromDataTable(dt);
        }

        #endregion

        #region IPatientEnterBLL 成员


        public void PatientAdditionalBarcode(
                                string pat_id
                                , DateTime pat_date
                                , string pat_bar_code
                                , string barcode_to_append
                                , string pat_itr_name
                                , string pat_sid
                                , string loginId
                                , string loginName)
        {
            PatUpdateBLL update = new PatUpdateBLL();
            update.PatientAdditionalBarcode(pat_id, pat_date, pat_bar_code, barcode_to_append, pat_itr_name, pat_sid, loginId, loginName);
        }

        #endregion

        #region IPatientEnterBLL 成员

        /// <summary>
        /// 获取这台仪器所有有权限的用户
        /// </summary>
        /// <param name="itrID"></param>
        /// <returns></returns>
        public DataSet GetUserCanMgrIInstrmt(string itrID)
        {

            return CacheUserInstrmtInfo.Current.GetUserCanMgrIInstrmt(itrID);


        }

        #endregion

        #region IPatientEnterBLL 成员


        public bool IsTheItrSoleSid(string itr_id, string Sid)
        {
            DictInstructmentBLL bll = new DictInstructmentBLL();
            return bll.IsTheItrSoleSid(itr_id, Sid);
        }

        #endregion

        #region IPatientEnterBLL 成员


        public DataTable GetItrWarningMsg(string patid)
        {
            string sql = string.Format(@"
select 
warn_id, warn_pat_id, warn_item_id, warn_msg_code, warn_msg, warn_recheck_type, warn_date
,dict_item.itm_ecd from instrmt_warning_msg
left join dict_item on warn_item_id=dict_item.itm_id
where warn_pat_id='{0}'
", patid);

            DataTable dt = new DBHelper().GetTable(sql);
            dt.TableName = "GetItrWarningMsg";
            return dt;
        }

        public IEnumerable<EntityAuditRule> GetAuditRuleList()
        {
            List<EntityAuditRule> list = new List<EntityAuditRule>();
            string sql = string.Format(@"select * from dict_auditrule ");

            DataTable dt = new DBHelper().GetTable(sql);
            dt.TableName = "dict_auditrule";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EntityAuditRule rule = new EntityAuditRule();
                rule.ar_id = (int)dt.Rows[i]["ar_id"];
                rule.ar_type = dt.Rows[i]["ar_type"].ToString();
                if (dt.Rows[i]["ar_item_id"] != null && dt.Rows[i]["ar_item_id"] != DBNull.Value)
                {
                    rule.ar_item_id = dt.Rows[i]["ar_item_id"].ToString();
                }
                if (dt.Rows[i]["ar_nohis_min"] != null && dt.Rows[i]["ar_nohis_min"] != DBNull.Value)
                {
                    rule.ar_nohis_min = (decimal)dt.Rows[i]["ar_nohis_min"];
                }
                if (dt.Rows[i]["ar_nohis_max"] != null && dt.Rows[i]["ar_nohis_max"] != DBNull.Value)
                {
                    rule.ar_nohis_max = (decimal)dt.Rows[i]["ar_nohis_max"];
                }
                if (dt.Rows[i]["ar_result_value"] != null && dt.Rows[i]["ar_result_value"] != DBNull.Value)
                {
                    rule.ar_result_value = (decimal)dt.Rows[i]["ar_result_value"];
                }
                if (dt.Rows[i]["ar_result_days"] != null && dt.Rows[i]["ar_result_days"] != DBNull.Value)
                {
                    rule.ar_result_days = (int)dt.Rows[i]["ar_result_days"];
                }
                list.Add(rule);
            }
            dt.Clear();
            return list;
        }

        public bool UpdateAuditRuleList(IEnumerable<EntityAuditRule> auditRules)
        {
            try
            {
                SqlHelper helper = new SqlHelper();
                foreach (EntityAuditRule entity in auditRules)
                {
                    if (entity.ar_id == null || entity.ar_id <= 0)
                    {
                        string strSql = @"insert into dict_auditrule
                                  (ar_item_id,ar_nohis_min,ar_nohis_max,ar_result_value,ar_type,ar_result_days)
                                  values (?,?,?,?,?,?)";
                        DbCommandEx com = helper.CreateCommandEx(strSql);
                        com.AddParameterValue(entity.ar_item_id);
                        com.AddParameterValue(entity.ar_nohis_min);
                        com.AddParameterValue(entity.ar_nohis_max);
                        com.AddParameterValue(entity.ar_result_value);
                        com.AddParameterValue(entity.ar_type);
                        com.AddParameterValue(entity.ar_result_days);
                        helper.ExecuteNonQuery(com);
                    }
                    else
                    {
                        string strSql = @"update dict_auditrule set ar_nohis_min=?,ar_nohis_max=?,ar_result_value=?,ar_result_days=?
                                  where ar_id=? ";
                        DbCommandEx com = helper.CreateCommandEx(strSql);
                        com.AddParameterValue(entity.ar_nohis_min);
                        com.AddParameterValue(entity.ar_nohis_max);
                        com.AddParameterValue(entity.ar_result_value);
                        com.AddParameterValue(entity.ar_result_days);
                        com.AddParameterValue(entity.ar_id);
                        helper.ExecuteNonQuery(com);
                    }
                }
                DictAuditRuleCache.Current.Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteAuditRule(int ar_id)
        {
            try
            {
                SqlHelper helper = new SqlHelper();

                string strSql = @"delete from dict_auditrule where ar_id=?";
                DbCommandEx com = helper.CreateCommandEx(strSql);
                com.AddParameterValue(ar_id);
                helper.ExecuteNonQuery(com);
                DictAuditRuleCache.Current.Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<EntityAutoAuditItr> GetAutoAuditItrList()
        {
            List<EntityAutoAuditItr> list = new List<EntityAutoAuditItr>();
            string sql = string.Format(@"SELECT dai.*,di.itr_mid,di.itr_name FROM dict_autoauditItr dai 
                                                    LEFT JOIN dict_instrmt di ON di.itr_id=dai.ai_itr_id WHERE dai.ai_del_flag<>1  ");

            DataTable dt = new DBHelper().GetTable(sql);
            dt.TableName = "dict_autoauditItr";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EntityAutoAuditItr rule = new EntityAutoAuditItr();
                rule.ai_itr_id = dt.Rows[i]["ai_itr_id"].ToString();
                if (dt.Rows[i]["ai_chk_code"] != null && dt.Rows[i]["ai_chk_code"] != DBNull.Value)
                {
                    rule.ai_chk_code = dt.Rows[i]["ai_chk_code"].ToString();
                }
                if (dt.Rows[i]["ai_report_code"] != null && dt.Rows[i]["ai_report_code"] != DBNull.Value)
                {
                    rule.ai_report_code = dt.Rows[i]["ai_report_code"].ToString();
                }
                if (dt.Rows[i]["ai_audit_timefrom"] != null && dt.Rows[i]["ai_audit_timefrom"] != DBNull.Value)
                {
                    rule.ai_audit_timefrom = dt.Rows[i]["ai_audit_timefrom"].ToString();
                }
                if (dt.Rows[i]["ai_audit_timeTo"] != null && dt.Rows[i]["ai_audit_timeTo"] != DBNull.Value)
                {
                    rule.ai_audit_timeTo = dt.Rows[i]["ai_audit_timeTo"].ToString();
                }
                if (dt.Rows[i]["ai_remark"] != null && dt.Rows[i]["ai_remark"] != DBNull.Value)
                {
                    rule.ai_remark = dt.Rows[i]["ai_remark"].ToString();
                }
                if (dt.Rows[i]["ai_allow_fristaudit"] != null && dt.Rows[i]["ai_allow_fristaudit"] != DBNull.Value)
                {
                    rule.ai_allow_fristaudit = (int)dt.Rows[i]["ai_allow_fristaudit"];
                }
                if (dt.Rows[i]["ai_allow_secondaudit"] != null && dt.Rows[i]["ai_allow_secondaudit"] != DBNull.Value)
                {
                    rule.ai_allow_secondaudit = (int)dt.Rows[i]["ai_allow_secondaudit"];
                }
                if (dt.Rows[i]["itr_mid"] != null && dt.Rows[i]["itr_mid"] != DBNull.Value)
                {
                    rule.itr_mid = dt.Rows[i]["itr_mid"].ToString();
                }
                rule.ai_del_flag = (int)dt.Rows[i]["ai_del_flag"];
                list.Add(rule);
            }
            dt.Clear();
            return list;
        }


        public bool UpdateAutoAuditItrList(IEnumerable<EntityAutoAuditItr> auditRules)
        {
            try
            {
                SqlHelper helper = new SqlHelper();
                foreach (EntityAutoAuditItr entity in auditRules)
                {
                    if (entity.IsNew)
                    {
                        string strSql = @"insert into dict_autoauditItr
                                  (ai_itr_id,ai_chk_code,ai_report_code,ai_audit_timefrom,ai_audit_timeTo,ai_allow_fristaudit,ai_allow_secondaudit,ai_remark)
                                  values (?,?,?,?,?,?,?,?)";
                        DbCommandEx com = helper.CreateCommandEx(strSql);
                        com.AddParameterValue(entity.ai_itr_id);
                        com.AddParameterValue(entity.ai_chk_code);
                        com.AddParameterValue(entity.ai_report_code);
                        com.AddParameterValue(entity.ai_audit_timefrom);
                        com.AddParameterValue(entity.ai_audit_timeTo);
                        com.AddParameterValue(entity.ai_allow_fristaudit);
                        com.AddParameterValue(entity.ai_allow_secondaudit);
                        com.AddParameterValue(entity.ai_remark);
                        helper.ExecuteNonQuery(com);
                    }
                    else
                    {
                        string strSql =
                            @"update dict_autoauditItr set ai_chk_code=?,ai_report_code=?,ai_audit_timefrom=?
                                  ,ai_audit_timeTo=?,ai_allow_fristaudit=?,ai_allow_secondaudit=?,ai_remark=?
                                  where ai_itr_id=? ";
                        DbCommandEx com = helper.CreateCommandEx(strSql);
                        com.AddParameterValue(entity.ai_chk_code);
                        com.AddParameterValue(entity.ai_report_code);
                        com.AddParameterValue(entity.ai_audit_timefrom);
                        com.AddParameterValue(entity.ai_audit_timeTo);
                        com.AddParameterValue(entity.ai_allow_fristaudit);
                        com.AddParameterValue(entity.ai_allow_secondaudit);
                        com.AddParameterValue(entity.ai_remark);
                        com.AddParameterValue(entity.ai_itr_id);
                        helper.ExecuteNonQuery(com);
                    }
                }
                //DictAuditRuleCache.Current.Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteAutoAuditItr(string ai_itr_id)
        {
            try
            {
                SqlHelper helper = new SqlHelper();

                string strSql = @"delete from dict_autoauditItr where ai_itr_id=?";
                DbCommandEx com = helper.CreateCommandEx(strSql);
                com.AddParameterValue(ai_itr_id);
                helper.ExecuteNonQuery(com);
                //DictAuditRuleCache.Current.Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool SaveNoBarCodeItrExList(List<string> itrList)
        {

            string flag = "select configid from sysconfig where configcode='{0}'";

            string strUpdate = "update sysconfig set configItemValue = '{0}' where configcode='{1}' ";

            string strInsert = @"if not exists(select configid from sysconfig where configcode='{0}')
                                 insert into sysconfig (configCode, configGroup, configItemName, configItemType, configItemValue,  configType)
                                 values ('{0}','条码','{1}','字符串','{2}','system')";
            try
            {
                string sortvalue = "";

                foreach (string itrid in itrList)
                {
                    sortvalue += "," + itrid;
                }
                if (sortvalue.Length > 0)
                    sortvalue = sortvalue.Remove(0, 1);
                DBHelper helper = new DBHelper();

                DataTable dtSort = helper.GetTable(string.Format(flag, "Lab_NoBarCodeCheckItrExpectList"));

                if (dtSort != null && dtSort.Rows.Count > 0)
                    helper.ExecuteNonQuery(string.Format(strUpdate, sortvalue, "Lab_NoBarCodeCheckItrExpectList"));
                else
                    helper.ExecuteNonQuery(string.Format(strInsert, "Lab_NoBarCodeCheckItrExpectList",
                                                         "不需要无条码打印确认的仪器(编码逗号隔开)", sortvalue));

                CacheSysConfig.Current.Refresh();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool UpdateNoBarComfirmFlag(List<string> patList)
        {


            string strUpdate = "update patients set pat_prt_flag =1 where pat_id='{0}' ";

            try
            {
                DBHelper helper = new DBHelper();

                foreach (string patid in patList)
                {
                    helper.ExecuteNonQuery(string.Format(strUpdate, patid));
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新阅读者信息
        /// </summary>
        /// <param name="strOpType">nw:记录阅读信息;un:取消阅读</param>
        /// <param name="strLookcode">阅读者信息</param>
        /// <param name="patList">pat_id集</param>
        /// <returns></returns>
        public bool UpdatePatLookcodeData(string strOpType, string strLookcode, List<string> patList)
        {
            if (string.IsNullOrEmpty(strOpType)) return false;

            if (string.IsNullOrEmpty(strLookcode)) return false;

            string strUpdate = "";

            if (strOpType.ToLower() == "nw")//记录阅读者
            {
                strUpdate = string.Format("update patients set pat_look_code='{0}',pat_look_date='{1}' ", strLookcode, DateTime.Now);
                strUpdate += " where pat_id='{0}' and (pat_flag=2 or pat_flag=4) and pat_look_code is null and pat_look_date is null ";
            }
            else if (strOpType.ToLower() == "un")//取消阅读者
            {
                //当前用户是否有权限取消所有阅读信息,加上是否有科室权限
                if (GetUserHaveFunctionByCode(strLookcode, "FrmCombineModeSel_UnAllLookcode"))
                {
                    if (strLookcode != "admin")
                    {
                        strUpdate = @"update patients set pat_look_code=null where pat_id='{0}' and pat_look_code is not null and pat_dep_id in (
  select [dict_depart].[dep_code]
from poweruserinfo
inner join [poweruserdepart] on poweruserinfo.userInfoId=[poweruserdepart].userInfoId
inner join dict_depart on dict_depart.dep_id=poweruserdepart.departId
where poweruserinfo.loginId='" + strLookcode + "')  ";
                    }
                    else
                    {
                        strUpdate = @"update patients set pat_look_code=null where pat_id='{0}' and pat_look_code is not null  ";
                    }
                }
                else
                {
                    strUpdate = string.Format("update patients set pat_look_code=null where pat_look_code='{0}' ", strLookcode);
                    strUpdate += " and pat_id='{0}' ";
                }
            }
            else if (strOpType == "unForOwnLooker")//自己取消自己
            {
                strUpdate = string.Format("update patients set pat_look_code=null where pat_look_code='{0}' ", strLookcode);
                strUpdate += " and pat_id='{0}' ";
            }
            else
            {
                return false;
            }

            try
            {
                DBHelper helper = new DBHelper();

                foreach (string patid in patList)
                {
                    helper.ExecuteNonQuery(string.Format(strUpdate, patid));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFilterCodeByTel(string tel)
        {

            if (string.IsNullOrEmpty(tel)) return string.Empty;

            string strUpdate = string.Format("select top 1 pat_upid From patients_newborn where pat_tel='{0}' and pat_upid is not null and pat_upid<>'' ", tel);

            try
            {
                SqlHelper helper = new SqlHelper();

                object obj = helper.ExecuteScalar(strUpdate);

                if (obj != null && obj != DBNull.Value)
                {
                    return obj.ToString();
                }

            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }


        /// <summary>
        /// 获取无条码病人资料状态
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pat_itr_id"></param>
        /// <returns></returns>
        public DataTable GetNoBarCodePatientList(DateTime date, string pat_itr_id)
        {
            try
            {
                DBHelper helper = new DBHelper();


                string sql1 = @"
select 0 as isselected,
patients.pat_id,
patients.pat_sid,
patients.pat_in_no,
patients.pat_host_order,
patients.pat_ctype,
patients.pat_name,
patients.pat_apply_date,
case when patients.pat_flag is null then 0
else patients.pat_flag end as pat_flag,
case 
when userRec.username is null then pat_i_code
 else userRec.username
end as pat_i_name
from patients
 LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code
where
pat_date >= @date1 and pat_date < @date2  and pat_itr_id =@itrid 
and pat_prt_flag is null and (pat_bar_code is null or pat_bar_code='') 
order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
";

                SqlCommand cmd1 = new SqlCommand(sql1);
                cmd1.Parameters.AddWithValue("date1", date.Date);
                cmd1.Parameters.AddWithValue("date2", date.Date.AddDays(1));
                cmd1.Parameters.AddWithValue("itrid", pat_itr_id);

                DataTable table = helper.GetTable(cmd1);



                table.TableName = "patientss";
                return table;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetNoBarCodePatientList", ex.ToString());
                throw;
            }

        }

        public DataTable BatchAddCombine(DataTable source)
        {
            try
            {
                DataTable dtSource = GetSourceData(source);
                DataTable dtDataToUpdate = dtSource.Clone();
                DataTable dtResult = new DataTable("result");
                DataTable dtPatientsMi = new DataTable();
                dtResult.Columns.Add("message");
                dtResult.Columns.Add("resultFlag");
                DataRow drNew = dtResult.NewRow();
                bool comHas = false;
                int updateFlag = 0;

                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    using (DBHelper transHelper = DBHelper.BeginTransaction())
                    {
                        foreach (DataRow drSource in dtSource.Rows)
                        {
                            //未审核的才能修改
                            if (Compare.IsEmpty(drSource["pat_flag"]) || drSource["pat_flag"].ToString() == "0")
                            {
                                int com_seq = 0;
                                string pat_c_name = string.Empty;
                                if (!string.IsNullOrEmpty(drSource["pat_c_name"].ToString()))
                                {
                                    dtPatientsMi = GetPatientsMiCount(drSource["pat_id"].ToString());
                                    com_seq = dtPatientsMi.Rows.Count;
                                    pat_c_name += drSource["pat_c_name"].ToString();
                                }

                                foreach (DataRow rowCombine in source.Rows)
                                {
                                    foreach (DataRow item in dtPatientsMi.Rows)
                                    {
                                        if (item["pat_com_id"].ToString() == rowCombine["com_id"].ToString())
                                        {
                                            comHas = true;
                                            break;
                                        }
                                        else
                                            comHas = false;
                                    }
                                    if (comHas)
                                        continue;
                                    if (string.IsNullOrEmpty(pat_c_name))
                                        pat_c_name = rowCombine["com_name"].ToString();
                                    else
                                        pat_c_name += "+" + rowCombine["com_name"].ToString();

                                    string sqlInsertPatMi = string.Format(@"
insert into patients_mi(pat_id,pat_com_id,pat_his_code,pat_com_price,pat_yz_id,pat_seq)
values(@pat_id,@pat_com_id,@pat_his_code,@pat_com_price,@pat_yz_id,@pat_seq)
");
                                    SqlCommand cmdInsert = new SqlCommand(sqlInsertPatMi);
                                    cmdInsert.Parameters.AddWithValue("pat_id", drSource["pat_id"].ToString());
                                    cmdInsert.Parameters.AddWithValue("pat_com_id", rowCombine["com_id"].ToString());
                                    cmdInsert.Parameters.AddWithValue("pat_his_code", DBNull.Value);

                                    cmdInsert.Parameters.AddWithValue("pat_com_price", DBNull.Value);

                                    cmdInsert.Parameters.AddWithValue("pat_yz_id", DBNull.Value);
                                    cmdInsert.Parameters.AddWithValue("pat_seq", com_seq);

                                    updateFlag = transHelper.ExecuteNonQuery(cmdInsert);

                                    com_seq++;
                                }
                                if (updateFlag > 0)
                                {
                                    drSource["pat_c_name"] = pat_c_name;
                                    dtDataToUpdate.Rows.Add(drSource.ItemArray);
                                }
                            }
                            else
                                continue;
                        }
                        List<SqlCommand> cmdPatient = DBTableHelper.GenerateUpdateCommand("patients", new string[] { "pat_key" }, new string[] { }, dtDataToUpdate);

                        foreach (SqlCommand cmd in cmdPatient)
                        {
                            updateFlag += transHelper.ExecuteNonQuery(cmd);
                        }

                        transHelper.Commit();
                    }
                    if (updateFlag > 0)
                    {
                        drNew["message"] = "";
                        drNew["resultFlag"] = "1";
                        dtResult.Rows.Add(drNew);
                    }
                }
                else
                {
                    drNew["message"] = "未找到样本号";
                    drNew["resultFlag"] = "0";
                    dtResult.Rows.Add(drNew);
                }
                return dtResult;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "BatchAddCombine", ex.ToString());
                return null;
            }

        }

        #endregion

        private DataTable GetSourceData(DataTable source)
        {
            DateTime date = Convert.ToDateTime(source.Rows[0]["pat_date"].ToString());
            string dateS = Convert.ToString(date.ToString("yyyy-MM-dd 00:00:00"));
            string dateE = Convert.ToString(date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            string sqlSelectSource = string.Format(@"
select
*
from patients
where
pat_itr_id = '{0}'
and pat_date >= '{3}' and pat_date < '{4}'
and cast(pat_sid as bigint) >= {1}
and cast(pat_sid as bigint) <= {2}
",
 source.Rows[0]["pat_itr_id"].ToString(),
 source.Rows[0]["pat_sid_begin"].ToString(),
 source.Rows[0]["pat_sid_end"].ToString(),
 dateS,
 dateE
 );
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelectSource);
            dt.TableName = "GetSourceData";
            return dt;
        }

        private DataTable GetPatientsMiCount(string pat_id)
        {
            string sqlSelectSource = string.Format(@"
select
pat_com_id
from patients_mi
where
pat_id = '{0}'
",
pat_id
 );
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelectSource);
            dt.TableName = "GetSourceData";
            //int count = Convert.ToInt16(dt.Rows[0][0].ToString());
            return dt;
        }

        /// <summary>
        /// 手工确认费用
        /// </summary>
        /// <returns></returns>
        public string ManuaConfirmFee(string pat_id, string barcode, List<string> comIDs)
        {
            try
            {

                bool iscancel = false;
                SZRYService ser = new SZRYService();
                string xml = string.Empty;
                if (pat_id == "barcode")
                {
                    xml = ser.ConfirmZyApplyInfo(barcode);
                }
                if (pat_id == null && comIDs == null)
                {
                    iscancel = true;
                    xml = ser.CancelZyApplyInfo(barcode);
                }
                if (pat_id == "sign")
                {
                    xml = ser.ConfirmZyApplyInfoWithFeeCode(barcode, comIDs);
                }

                if (pat_id == "return")
                {
                    iscancel = true;

                    xml = ser.CancelZyApplyInfoWithFeeCode(barcode, comIDs);
                }
                if (pat_id != "sign" && pat_id != "return" && comIDs != null && comIDs.Count > 0)
                {
                    xml = ser.ConfirmZyApplyInfo(barcode, comIDs);
                }
                if (!string.IsNullOrEmpty(xml))
                {
                    DataSet ds = ConvertXMLToDataSet(xml);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                        && ds.Tables[0].Columns.Contains("Result"))
                    {
                        string flag = iscancel ? "0" : "2";
                        string res = ds.Tables[0].Rows[0]["Result"].ToString() == "100" ? flag : "1";

                        string sql = string.Format("update bc_patients set bc_upid='{0}' where bc_bar_code='{1}'", res, barcode);

                        DBHelper hepler = new DBHelper();

                        hepler.ExecuteNonQuery(sql);

                        sql = string.Format("update patients set pat_upid='{0}' where pat_bar_code='{1}'", res, barcode);
                        hepler.ExecuteNonQuery(sql);

                        res = ds.Tables[0].Rows[0]["Result"].ToString() == "100" ? "" : ds.Tables[0].Rows[0]["ErrorMsg"].ToString();

                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ManuaConfirmFee", ex.ToString());
                return ex.Message;
            }
            return string.Empty;
        }

        private DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


        public void SendCriticalMsg(string pat_id)
        {
            new SendCrotocalMsgForZY6Y(pat_id).Excute();
        }

        public void SendCriticalMsg(DataTable data)
        {
            new SendCrotocalMsgForZY6Y(data).ExcuteData();
        }

        /// <summary>
        /// 通过医院获取仪器
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public string GetInstrmtByHospital(string hospital)
        {
            string sqlSelect = string.Format(@"
select dict_instrmt.itr_id from dict_instrmt 
left join dict_type on dict_instrmt.itr_type=dict_type.type_id
where dict_type.tyep_hospitalName='{0}'",
hospital
 );
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);
            string strInstrmt = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    strInstrmt += "," + dr[0].ToString();
                }
                if (!string.IsNullOrEmpty(strInstrmt))
                {
                    strInstrmt = strInstrmt.Remove(0, 1);
                }
            }
            return strInstrmt;
        }

        public DataSet GetPatientsRecordByBarCodeWithBabyFilter(string barCode, bool isBabyFilter)
        {
            throw new NotImplementedException();
        }
    }
}
