using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using System.Data;
using Lib.DAC;
using dcl.servececontract;
using System.Collections;
using dcl.root.dto;

namespace dcl.svr.result
{
    public class MicMediaBIZ : IMicMedia
    {
        public DataTable GetPatients(DateTime dtStart, DateTime dtEnd, string strFilter, string strFlag)
        {
            string sql = string.Format(@"select
                                        pat_vouno,  
                                        pat_bar_code,
                                        pat_name,
                                        (case pat_sex when 1 then '男' when 2 then '女' else '未知' end) pat_sex,
                                        pat_c_name,
                                        dbo.getage(pat_age_exp) pat_age,
                                        pat_flag,
                                        pat_date
                                        from lis_mic_patients
                                        where pat_date >='{0}' and pat_date <'{1}'", dtStart.Date, dtEnd.AddDays(1).Date);

            string strWhere = string.Empty;


            switch (strFlag)
            {
                case "0":
                    strWhere = " and pat_flag ='0'";
                    break;
                case "1":
                    strWhere = " and pat_flag in ('1','2')";
                    break;
                case "2":
                    strWhere = string.Empty;
                    break;
                default:
                    break;
            }

            if (strFilter != string.Empty)
                strWhere += strFilter;


            sql += strWhere;

            SqlHelper helper = new SqlHelper();

            DataTable dtPatients = helper.GetTable(sql, "lis_mic_patients");

            return dtPatients;
        }

        public DataSet GetPatMediaInfo(string strVouNo)
        {
            DataSet dsResult = new DataSet();
            SqlHelper helper = new SqlHelper();
            string strMedia = string.Format(@"select lis_mic_media.*,lis_dict_media.med_name from  lis_mic_media
                                              left join lis_dict_media on lis_dict_media.med_code=lis_mic_media.med_code
                                              where lis_mic_media.med_pat_vouno='{0}'", strVouNo);
            DataTable dtMedia = helper.GetTable(strMedia, "lis_mic_media");
            dsResult.Tables.Add(dtMedia);

            return dsResult;
        }

        public DataSet GetPatMediaProInfo(string strMedId, string strMedCode)
        {
            DataSet dsResult = new DataSet();
            SqlHelper helper = new SqlHelper();

            string strMediaPro = string.Format(@"select * from  lis_mic_media_pro where pro_med_id='{0}'", strMedId);
            DataTable dtMediaPro = helper.GetTable(strMediaPro, "lis_mic_media_pro");
            dsResult.Tables.Add(dtMediaPro);

            string strWorksheet = string.Format(@"select *,case she_type when '0' then '过程记录' else '报告' end she_type_name 
                                                  from lis_dict_worksheet where she_med_code ='{0}'", strMedCode);

            DataTable dtWorksheet = helper.GetTable(strWorksheet, "lis_dict_worksheet");
            dsResult.Tables.Add(dtWorksheet);

            return dsResult;
        }

        public DataSet GetComMedia(List<String> lisComId, string strSamId, string strBarcode)
        {
            DataSet dsResult = new DataSet();

            string strSql = string.Format("select pat_vouno from lis_mic_patients where pat_bar_code='{0}'", strBarcode);
            SqlHelper helper = new SqlHelper();
            dsResult.Tables.Add(helper.GetTable(strSql, "PatStatus"));


            string strComId = string.Empty;

            foreach (string item in lisComId)
            {
                strComId += string.Format(",'{0}'", item);
            }

            if (strComId.Length > 0)
                strComId = strComId.Remove(0, 1);


            string strGetComMedia = string.Format(@"select lis_dict_media.* from lis_dict_com_media 
                                                    inner join lis_dict_media on lis_dict_media.med_code=lis_dict_com_media.com_med_code
                                                    where lis_dict_media.med_del_flag='0' and  lis_dict_com_media.com_id in ({0}) 
                                                    and lis_dict_com_media.com_sam_id='{1}'
                                                    ", strComId, strSamId);

            dsResult.Tables.Add(helper.GetTable(strGetComMedia, "ComMedia"));




            return dsResult;
        }

        public bool SaveMICPatientInfo(DataTable dtPatInfo, List<String> lisComId, string strSamId)
        {
            List<string> lisVonNo = new List<string>();

            string strComId = string.Empty;

            foreach (string item in lisComId)
            {
                strComId += string.Format(",'{0}'", item);
            }

            if (strComId.Length > 0)
                strComId = strComId.Remove(0, 1);

            //SqlHelper helper = new SqlHelper();
            DbBase dao = DbBase.InConn();

            string strGetComMedia = string.Format(@"select distinct lis_dict_media.* from lis_dict_com_media 
                                                    inner join lis_dict_media on lis_dict_media.med_code=lis_dict_com_media.com_med_code
                                                    where lis_dict_media.med_del_flag='0' and  lis_dict_com_media.com_id in ({0}) 
                                                    and lis_dict_com_media.com_sam_id='{1}'
                                                    ", strComId, strSamId);

            DataTable dtComMedia = dao.GetDataTable(strGetComMedia, "ComMedia");

            string strGetMediaLimit = string.Format(@"select * from lis_dict_culture_limit where cul_com_id in ({0}) and cul_com_sam_id='{1}'", strComId, strSamId);

            DataTable dtMediaLimit = dao.GetDataTable(strGetMediaLimit, "MediaLimit");

            //获取lis_mic_patients表结构
            DataTable dtPatient = dao.GetDataTable("select * from lis_mic_patients where pat_vouno='-1'");

            DataRow drPatInfo = dtPatInfo.Rows[0];

            //获取lis_mic_media表结构
            DataTable dtMedia = dao.GetDataTable("select * from lis_mic_media where med_id='-1'");
            dtMedia.Columns.Remove("med_id");


            DataTable dtVouNo = dao.GetDataTable(@"select isnull(max(pat_vouno),0) vouno from lis_mic_patients 
                                                    where pat_date>= CONVERT(varchar(100), GETDATE(), 111) 
                                                    and pat_date < CONVERT(varchar(100),dateadd( d,1,GETDATE()), 111)");
            int vouNo = Convert.ToInt32(dtVouNo.Rows[0]["vouno"].ToString());

            if (vouNo == 0)
                vouNo = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd") + "0");


            foreach (DataRow drComMedia in dtComMedia.Rows)
            {

                DataRow drPatient = dtPatient.NewRow();
                vouNo = vouNo + 1;
                foreach (DataColumn item in dtPatient.Columns)
                {
                    if (item.ColumnName != "pat_vouno" && item.ColumnName != "pat_flag" && item.ColumnName != "pat_report_id")
                    {
                        drPatient[item.ColumnName] = drPatInfo[item.ColumnName];
                    }
                }

                drPatient["pat_vouno"] = vouNo;
                drPatient["pat_flag"] = "0";
                drPatient["pat_date"] = DateTime.Now;

                dtPatient.Rows.Add(drPatient);


                DataRow drMedia = dtMedia.NewRow();

                drMedia["med_pat_vouno"] = vouNo;
                drMedia["med_code"] = drComMedia["med_code"].ToString();
                drMedia["med_start_date"] = DateTime.Now;
                drMedia["med_flag"] = "0";

                DataRow[] drMedias = dtMediaLimit.Select(string.Format("cul_com_sam_id='{0}'", strSamId));
                drMedia["med_end_date"] = DateTime.Now.Date.AddDays(Convert.ToInt32(drMedias[0]["cul_limit"].ToString()) + 1);

                dtMedia.Rows.Add(drMedia);


            }

            ArrayList lisInsert = dao.GetInsertSql(dtPatient);

            lisInsert.AddRange(dao.GetInsertSql(dtMedia));

            int result = dao.DoNonQuery(lisInsert);

            return result > 0;
        }

        public DataTable GetMediaProPlus(string strMediaProId)
        {
            SqlHelper helper = new SqlHelper();
            string strMediaPro = string.Format("select * from lis_mic_media_pro_plus where pro_id='{0}'", strMediaProId);
            DataTable dtMediaProPlus = helper.GetTable(strMediaPro, "lis_mic_media_pro_plus");
            return dtMediaProPlus;
        }

        public int SaveMedia(DataSet dsMedia)
        {
            DbBase dao = DbBase.InConn();

            DataTable dtMediaPro = dsMedia.Tables["lis_mic_media_pro"];

            ArrayList arrOperate = new ArrayList();

            string strMedId = string.Empty;

            if (dtMediaPro.Rows.Count > 0)
            {
                DataRow drMediaPro = dtMediaPro.Rows[0];

                if (drMediaPro["pro_id"].ToString() == string.Empty)
                {
                    int id = dao.GetID("lis_mic_media_pro", dtMediaPro.Rows.Count);

                    strMedId = id.ToString();

                    drMediaPro["pro_id"] = strMedId;

                    arrOperate = dao.GetInsertSql(dtMediaPro);
                }
                else
                {
                    strMedId = drMediaPro["pro_id"].ToString();
                    arrOperate = dao.GetUpdateSql(dtMediaPro, new string[] { "pro_id" });
                }
            }


            DataTable dtMediaProPlus = dsMedia.Tables["lis_mic_media_pro_plus"];

            DataTable dtInsertMediaProPlus = dtMediaProPlus.Clone();

            if (dtMediaProPlus != null && dtMediaProPlus.Rows.Count > 0)
            {
                foreach (DataRow item in dtMediaProPlus.Rows)
                {
                    item["pro_id"] = strMedId;
                    if (item["pro_char"].ToString().Trim() != string.Empty)
                    {
                        dtInsertMediaProPlus.Rows.Add(item.ItemArray);
                    }
                }

                if (dtInsertMediaProPlus.Columns.Contains("itm_med_code"))
                {
                    dtInsertMediaProPlus.Columns.Remove("itm_med_code");
                }

                if (dtInsertMediaProPlus.Columns.Contains("itm_she_name"))
                {
                    dtInsertMediaProPlus.Columns.Remove("itm_she_name");
                }

                string strDelSql = string.Format("delete lis_mic_media_pro_plus where pro_id='{0}'", strMedId);

                arrOperate.Add(strDelSql);

                arrOperate.AddRange(dao.GetInsertSql(dtInsertMediaProPlus));
            }

            return dao.DoTran(arrOperate)[0];
        }

        public int SaveInterimReport(DataTable dtParameter)
        {
            int result = 0;
            if (dtParameter != null && dtParameter.Rows.Count > 0)
            {
                DataRow drParameter = dtParameter.Rows[0];
                string strBarCode = drParameter["barcode"].ToString();
                string strOperatorId = drParameter["rep_user_code"].ToString();
                string strRes = drParameter["rep_conclusion"].ToString();
                string strItrId = drParameter["rep_itr_id"].ToString();
                result = SavePatInfo(strBarCode, strItrId, strOperatorId, strRes, "2");

                if (result > 0)
                {
                    string strSql = string.Format(@"insert into dbo.lis_mic_report
                                ( rep_pat_vouno, rep_med_id, rep_pro_id, rep_describe, rep_conclusion, rep_date, rep_user_code, rep_user_name, rep_flag)
                                VALUES ('{0}','{1}','{2}','{3}','{4}',getdate(),'{5}','{6}','{7}')",
                                 drParameter["rep_pat_vouno"].ToString(),
                                 drParameter["rep_med_id"].ToString(),
                                 "",
                                 strRes,
                                 "",
                                 drParameter["rep_user_code"].ToString(),
                                 drParameter["rep_user_name"].ToString(),
                                 "2");

                    DbBase dao = DbBase.InConn();
                    dao.DoNonQuery(strSql);
                }

            }
            return result;
        }

        public int SaveFinalReport(DataTable dtParameter)
        {
            int result = 0;
            if (dtParameter != null && dtParameter.Rows.Count > 0)
            {
                DataRow drParameter = dtParameter.Rows[0];
                string strBarCode = drParameter["barcode"].ToString();
                string strOperatorId = drParameter["rep_user_code"].ToString();
                string strRes = drParameter["rep_conclusion"].ToString();
                string strItrId = drParameter["rep_itr_id"].ToString();
                result = SavePatInfo(strBarCode, strItrId, strOperatorId, strRes, "2");

                string strVouno = drParameter["rep_pat_vouno"].ToString();
                UpdateMediaStatus(strVouno);
            }
            return result;
        }

        public int SaveMidReport(DataTable dtParameter)
        {
            int result = 0;
            if (dtParameter != null && dtParameter.Rows.Count > 0)
            {
                DataRow drParameter = dtParameter.Rows[0];
                string strBarCode = drParameter["barcode"].ToString();
                string strOperatorId = drParameter["rep_user_code"].ToString();
                string strRes = drParameter["rep_conclusion"].ToString();
                string strItrId = drParameter["rep_itr_id"].ToString();
                result = SavePatInfo(strBarCode, strItrId, strOperatorId, strRes, "1");

                string strVouno = drParameter["rep_pat_vouno"].ToString();

                UpdateMediaStatus(strVouno);
            }
            return result;
        }

        private int SavePatInfo(string strBarCode, string strInstrmtId, string strOperatorID, string strResult, string strStatus)
        {
            PatientEnterService patService = new PatientEnterService();

            DataSet dsPat = patService.GetPatientsByBarCode(strBarCode);

            string Sid = patService.GetItrSID_MaxPlusOne(DateTime.Now, strInstrmtId, true);

            string patId = strInstrmtId + String.Format("{0:yyyyMMdd}", DateTime.Now)
                   + Sid;

            DataTable dtPatient = dsPat.Tables["patients"].Copy();

            dtPatient.Columns.Remove("pat_key");

            foreach (DataRow drPatient in dtPatient.Rows)
            {
                drPatient["pat_id"] = patId;
                drPatient["pat_sid"] = Sid;
                drPatient["pat_itr_id"] = strInstrmtId;
                drPatient["pat_flag"] = "0";
                if (strStatus == "2")
                {
                    drPatient["pat_flag"] = "2";
                    drPatient["pat_chk_code"] = strOperatorID;
                    drPatient["pat_chk_date"] = DateTime.Now.AddMinutes(-5);
                    drPatient["pat_report_code"] = strOperatorID;
                    drPatient["pat_report_date"] = DateTime.Now;
                }
                drPatient["pat_date"] = DateTime.Now;
            }

            DataTable dtCombine = dsPat.Tables["patients_mi"].Copy();

            foreach (DataRow drCombine in dtCombine.Rows)
            {
                drCombine["pat_id"] = patId;
            }

            DbBase dao = DbBase.InConn();

            ArrayList arr = dao.GetInsertSql(dtPatient);

            arr.AddRange(dao.GetInsertSql(dtCombine));

            if (strStatus == "2")
            {
                string strSql = string.Format(@"insert into cs_rlts 
                            (bsr_id, bsr_mid, bsr_date, bsr_sid, bsr_cname, bsr_describe, bsr_res_flag, bsr_seq, bsr_i_flag)  
                            VALUES ('{0}','{1}',getdate(),'{2}','{3}','','0','1','0')", patId, strInstrmtId, Sid, strResult);

                arr.Add(strSql);
            }

            return dao.DoNonQuery(arr);

        }

        private int UpdateMediaStatus(string strVouon)
        {
            string strUpdate1 = string.Format("update lis_mic_patients set pat_flag='2' where pat_vouno='{0}'", strVouon);

            string strUpdate2 = string.Format("update lis_mic_media set med_flag='3' where med_pat_vouno='{0}'", strVouon);

            string strUpdate3 = string.Format(@"update lis_mic_media_pro set pro_flag='1' where pro_med_id in 
                                                (select med_id from lis_mic_media where med_pat_vouno='{0}')", strVouon);


            ArrayList arrUpdate = new ArrayList();
            arrUpdate.Add(strUpdate1);
            arrUpdate.Add(strUpdate2);
            arrUpdate.Add(strUpdate3);


            DbBase dao = DbBase.InConn();


            return dao.DoTran(arrUpdate)[0];
        }

        public DataTable GetPatPicResult(string strProId)
        {
            string strSql = string.Format("select * from resulto_p where pres_id='{0}'", strProId);
            SqlHelper helper = new SqlHelper();
            return helper.GetTable(strSql, "resulto_p");
        }

        public DataTable GetMicInfo()
        {
            DataTable result = new DataTable();

            string strSql = @"select CONVERT(varchar(100), dateadd(m,-1,pat_date), 23) date,* from lis_mic_patients
                                left join lis_mic_media on lis_mic_media.med_pat_vouno=lis_mic_patients.pat_vouno
                                where pat_date>=CONVERT(varchar(100), dateadd(m,-1,getdate()), 23)";

            SqlHelper helper = new SqlHelper();
            DataTable dtMicRes = helper.GetTable(strSql, "MisRes");

            DateTime now = DateTime.Now.Date;

            DataTable dtDate = new DataTable("MicDate");
            dtDate.Columns.Add("1");
            dtDate.Columns.Add("2");
            dtDate.Columns.Add("3");
            dtDate.Columns.Add("4");
            dtDate.Columns.Add("5");
            dtDate.Columns.Add("6");

            for (int i = 0; i < 30; i++)
            {
                string strInfo = string.Empty;
                DateTime strDate = now.AddDays(-i).Date;
                string strDateWhere = string.Format(" pat_date>='{0}' and pat_date <'{1}'", strDate, strDate.AddDays(1));
                DataRow[] drMicRes = dtMicRes.Select(strDateWhere);
                if (drMicRes.Length > 0)
                {
                    DataRow[] drMicResIn = dtMicRes.Select(strDateWhere + " and pat_flag ='0'");
                    string strIn = drMicResIn.Length.ToString();
                    string strOut = dtMicRes.Select(strDateWhere + " and pat_flag in ('1','2')").Length.ToString();
                    string strCount = drMicRes.Length.ToString();

                    int count = 0;
                    foreach (DataRow item in drMicResIn)
                    {
                        if (Convert.ToDateTime(item["med_end_date"]) >= DateTime.Now.Date.AddDays(1))
                        {
                            count = count + 1;
                        }
                    }

                    strInfo = FormatValue(strDate.ToString("MM-dd"), strIn, strOut, count.ToString(), strCount);
                }
                else
                    strInfo = string.Format(@"{0}
当天无标本", strDate.ToString("MM-dd"));
                if (i == 0 || i % 6 == 0)
                    dtDate.Rows.Add(dtDate.NewRow());

                int intRow = i <= 5 ? 0 : (i - i % 6) / 6;

                dtDate.Rows[intRow][i % 6] = strInfo;
            }


            return dtDate;
        }

        private string FormatValue(string strDate, string strIn, string strOut, string strTime, string strCount)
        {
            return string.Format(@"{0}
标本总数：{4}
培  养 中：{1}
培养结束：{2}
超     时：{3}", strDate, strIn, strOut, strTime, strCount);
        }

        public int SaveMicReport(DataTable dtParameter)
        {
            DataRow drParameter = dtParameter.Rows[0];

            string strRepId = drParameter["rep_id"].ToString();

            string strSql = string.Empty;

            if (strRepId == string.Empty)
            {
                strSql = string.Format(@"insert into lis_mic_report (rep_pat_vouno,rep_conclusion,rep_date,rep_flag) values
                                                      ('{0}','{1}',getdate(),0)",
                                                      drParameter["rep_pat_vouno"].ToString(),
                                                      drParameter["rep_conclusion"].ToString());
            }
            else
            {
                strSql = string.Format("update lis_mic_report set rep_conclusion='{0}' where rep_id='{1}'",
                                                      drParameter["rep_conclusion"].ToString(),
                                                      drParameter["rep_id"].ToString());
            }

            SqlHelper helper = new SqlHelper();

            return helper.ExecuteNonQuery(strSql);
        }

        public DataTable GetMicReport(string strPatId)
        {
            string strSql = string.Format(@"select *,
                              case rep_flag when 1 then '已发布' else '已录入' end [state]
                              from dbo.lis_mic_report
                              where rep_pat_vouno='{0}'", strPatId);

            SqlHelper helper = new SqlHelper();
            return helper.GetTable(strSql, "lis_mic_report");
        }
        public DataTable GetAntiTypeMic(List<string> list_pat_id)
        {
            string strSql = @"SELECT patients.pat_id,patients.pat_name, an_rlts.anr_mic,dict_antibio.anti_at_id FROM an_rlts 
LEFT JOIN dict_antibio ON an_rlts.anr_aid = dict_antibio.anti_id 
LEFT JOIN patients WITH(NOLOCK ) ON patients.pat_id = an_rlts.anr_id 
WHERE patients.pat_id in ('{0}')
";
            strSql = string.Format(strSql, string.Join("','", list_pat_id.ToArray()));
            DataTable dt = new SqlHelper().GetTable(strSql, "an_rlts");
            return dt;
        }
        public int SaveMicInterimReport(string strMicRepId, string strOperatorID, string strOperatorName)
        {
            SqlHelper helper = new SqlHelper();

            string strMicReportSql = string.Format(@"select * from lis_mic_report where rep_id={0}", strMicRepId);

            DataTable dtMicReport = helper.GetTable(strMicReportSql);

            string strPatId = dtMicReport.Rows[0]["rep_pat_vouno"].ToString();

            string strSql = string.Format(@"select top 1 * from patients where pat_id='{0}'", strPatId);

            DataTable dtPat = helper.GetTable(strSql, "patients");

            if (dtPat != null && dtPat.Rows.Count > 0)
            {
                dtPat.Columns.Remove("pat_key");
                DataRow drPatient = dtPat.Rows[0];

                string strInstrmtId = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Mic_InterimReport");

                PatientEnterService patService = new PatientEnterService();

                string Sid = patService.GetItrSID_MaxPlusOne(DateTime.Now, strInstrmtId, true);

                string patId = strInstrmtId + String.Format("{0:yyyyMMdd}", DateTime.Now) + Sid;

                drPatient["pat_id"] = patId;
                drPatient["pat_sid"] = Sid;
                drPatient["pat_itr_id"] = strInstrmtId;
                drPatient["pat_flag"] = "2";
                drPatient["pat_chk_code"] = strOperatorID;
                drPatient["pat_chk_date"] = DateTime.Now.AddMinutes(-5);
                drPatient["pat_report_code"] = strOperatorID;
                drPatient["pat_report_date"] = DateTime.Now;
                drPatient["pat_date"] = DateTime.Now;

                string strPatMiSql = string.Format("select * from patients_mi where pat_id='{0}'", strPatId);

                DataTable dtCombine = helper.GetTable(strPatMiSql, "patients_mi");

                foreach (DataRow drCombine in dtCombine.Rows)
                {
                    drCombine["pat_id"] = patId;
                }

                DbBase dao = DbBase.InConn();

                ArrayList arr = dao.GetInsertSql(dtPat);

                arr.AddRange(dao.GetInsertSql(dtCombine));

                string strCsRltsSql = string.Format(@"insert into cs_rlts 
                            (bsr_id, bsr_mid, bsr_date, bsr_sid, bsr_cname, bsr_describe, bsr_res_flag, bsr_seq, bsr_i_flag)  
                            VALUES ('{0}','{1}',getdate(),'{2}','{3}','','0','1','0')", patId, strInstrmtId, Sid, dtMicReport.Rows[0]["rep_conclusion"].ToString());

                arr.Add(strCsRltsSql);


                int i = dao.DoNonQuery(arr);

                string strUpdateMicReportSql = string.Format(@"update lis_mic_report set rep_pat_id='{0}',rep_chk_date=getdate(),
                                                              rep_chk_code='{1}',rep_chk_name='{2}',
                                                              rep_flag=1 where rep_id={3}",
                                                              patId, strOperatorID, strOperatorName, strMicRepId);

                i += helper.ExecuteNonQuery(strUpdateMicReportSql);

                return i;
            }


            return 0;
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="strMicRepId"></param>
        /// <param name="strOperatorID"></param>
        /// <param name="strOperatorName"></param>
        /// <returns></returns>
        public int SaveMicInterimUndoReport(string strMicRepId)
        {
            SqlHelper helper = new SqlHelper();

            string strMicReportSql = string.Format(@"select * from lis_mic_report where rep_id={0}", strMicRepId);

            DataTable dtMicReport = helper.GetTable(strMicReportSql);

            string strPatId = dtMicReport.Rows[0]["rep_pat_id"].ToString();

            ArrayList arr = new ArrayList();
            string strPatients = string.Format(@"delete from patients where pat_id='{0}'", strPatId);
            arr.Add(strPatients);

            string strCsRltsSql = string.Format(@"delete from cs_rlts where bsr_id='{0}'", strPatId);
            arr.Add(strCsRltsSql);

            string strUpdateMicReportSql = string.Format(@"update lis_mic_report set rep_pat_id=null,rep_chk_date=null,
                                                              rep_chk_code=null,rep_chk_name=null,
                                                              rep_flag=0 where rep_id={0}",
                                                              strMicRepId);
            arr.Add(strUpdateMicReportSql);

            DbBase dao = DbBase.InConn();
            int i = dao.DoNonQuery(arr);

            return i;
        }

        public int DeleteMicInterimReport(List<string> strPatId)
        {
            int result = 0;

            SqlHelper helper = new SqlHelper();

            foreach (string item in strPatId)
            {
                DataTable dtMicReport = helper.GetTable(string.Format("select rep_pat_id from lis_mic_report where rep_pat_vouno='{0}'", item));

                foreach (DataRow drMicReport in dtMicReport.Rows)
                {
                    try
                    {
                        if (drMicReport["rep_pat_id"] != null && drMicReport["rep_pat_id"].ToString().Trim() != string.Empty)
                        {
                            string strInterimId = drMicReport["rep_pat_id"].ToString();

                            string strDeletePat = string.Format("delete patients where pat_id='{0}'", strInterimId);

                            string strDeletePatMi = string.Format("delete patients_mi where pat_id='{0}'", strInterimId);

                            string strDeleteResult = string.Format("delete cs_rlts where bsr_id='{0}'", strInterimId);

                            helper.ExecuteNonQuery(strDeletePat);
                            helper.ExecuteNonQuery(strDeletePatMi);
                            helper.ExecuteNonQuery(strDeleteResult);
                        }
                        result++;
                    }
                    catch
                    {
                    }

                }
            }

            return result;
        }
    }
}
