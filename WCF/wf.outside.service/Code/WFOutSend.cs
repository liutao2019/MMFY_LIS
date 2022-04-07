using System.Text.RegularExpressions;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using O2S.Components.PDFRender4NET;
using dcl.entity;
using dcl.svr.cache;
using dcl.svr.result;
using dcl.svr.sample;
using dcl.svr.dicbasic;

namespace dcl.outside.service
{
    public static class WFOutSend
    {
        private const string XML_TITLE = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        /// <summary>
        /// 用来获取Lis数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSqlConnection_Lis()
        {
            return ConfigurationManager.AppSettings["LisConnectionString"];
        }

        /// <summary>
        /// 获取中间数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSqlConnection_Mid()
        {
            return ConfigurationManager.AppSettings["MidConnectionString"];
        }

        /// <summary>
        /// 上传检验报告的系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetUploadLisRepDataFromSysName()
        {
            //上传检验报告的系统名称(默认金域)
            string strTemp = ConfigurationManager.AppSettings["UploadLisRepDataFromSysName"];

            if (string.IsNullOrEmpty(strTemp))
            {
                strTemp = "金域";
            }

            return strTemp;
        }

        /// <summary>
        /// 从Lis数据库获取项目数据
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetLisSubItems()
        {
            Logger.LogInfo(String.Format("{0}:执行方法：GetLisSubItems", DateTime.Now));
            StringBuilder builder = new StringBuilder();
            List<EntityDicItmItem> listItem = new ItemBIZ().GetLisSubItems();
            builder.Append(XML_TITLE).Append("<LisItems>");
            if (listItem.Count > 0)
            {
                foreach (EntityDicItmItem r in listItem)
                {
                    builder.Append("<Item>");

                    builder.Append("<ItemCode>");
                    builder.Append(r.ComId);
                    builder.Append("</ItemCode>");

                    builder.Append("<SubItemCode>");
                    builder.Append(r.ItmId);
                    builder.Append("</SubItemCode>");

                    builder.Append("<SubItemName>");
                    builder.Append(r.ItmName);
                    builder.Append("</SubItemName>");

                    builder.Append("</Item>");
                }
                builder.Append("</LisItems>");
            }
            else
                return null;
            return builder;
        }



        public static StringBuilder GetLisSubItemsByComId(string comId)
        {

            List<EntityDicItmItem> listitem = new ItemBIZ().GetLisSubItemsByComId(comId);
            StringBuilder builder = new StringBuilder();
            builder.Append("<LisItems>");

            if (listitem.Count > 0)
            {
                builder.Append("<lis_item_code>");
                builder.Append(comId);
                builder.Append("</lis_item_code>");
                builder.Append("<lis_item_name>");
                builder.Append(listitem[0].ComName);
                builder.Append("</lis_item_name>");
                for (int i = 0; i < listitem.Count; i++)
                {
                    EntityDicItmItem item = listitem[i];

                    builder.Append("<SubItems>");
                    builder.Append("<lis_subitem_code>");
                    builder.Append(item.ItmId);
                    builder.Append("</lis_subitem_code>");

                    builder.Append("<lis_subitem_name>");
                    builder.Append(item.ItmName);
                    builder.Append("</lis_subitem_name>");

                    builder.Append("</SubItems>");
                }
                builder.Append("</LisItems>");
            }
            else
            {
                EntityResponse response = new CacheDataBIZ().GetCacheData("EntityDicCombine");
                List<EntityDicCombine> listCombine = response.GetResult() as List<EntityDicCombine>;
                builder = new StringBuilder();
                builder.Append("<LisItems>");

                if (listCombine.Count > 0)
                {
                    builder.Append("<lis_item_code>");
                    builder.Append(comId);
                    builder.Append("</lis_item_code>");
                    builder.Append("<lis_item_name>");
                    builder.Append(listCombine[0].ComName);
                    builder.Append("</lis_item_name>");
                    builder.Append("</LisItems>");
                }
                else
                {
                    return new StringBuilder();
                }
            }

            return builder;
        }

        /*
         * SELECT     com_id AS F_ItemCode, com_name AS F_ItemName, com_code AS F_ShortName, com_code AS F_EnglishName, 
         * com_code AS F_EngShortName,NULL AS F_SimpCode,type_name AS F_SubjectName
         * FROM         dbo.dict_combine WITH (nolock) LEFT OUTER JOIN
         * dict_type WITH (nolock) ON com_ctype = type_id
         * WHERE     (com_del <> '1')
         */
        /// <summary>
        /// 从Lis数据库获取组合数据
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetLisItems()
        {
            EntityResponse response = new CacheDataBIZ().GetCacheData("EntityDicCombine");
            List<EntityDicCombine> listCombine = response.GetResult() as List<EntityDicCombine>;
            StringBuilder result = new StringBuilder();

            result.Append(XML_TITLE).Append("<LisItems>");

            if (listCombine != null && listCombine.Count > 0)
            {
                foreach (EntityDicCombine combine in listCombine)
                {
                    result.Append("<Item>");

                    result.Append("<ItemCode>");
                    result.Append(combine.ComId);
                    result.Append("</ItemCode>");

                    result.Append("<ItemName>");
                    result.Append(combine.ComName);
                    result.Append("</ItemName>");

                    result.Append("<ShortName>");
                    result.Append(combine.ComCode);
                    result.Append("</ShortName>");

                    result.Append("<EnglishName>");
                    result.Append(combine.ComCode);
                    result.Append("</EnglishName>");

                    result.Append("<EngShortName>");
                    result.Append(combine.ComCode);
                    result.Append("</EngShortName>");

                    result.Append("<SimpCode>");
                    result.Append("");
                    result.Append("</SimpCode>");

                    result.Append("<SubjectName>");
                    result.Append(combine.PTypeName);
                    result.Append("</SubjectName>");

                    result.Append("</Item>");
                }
                result.Append("</LisItems>");

            }
            return result;
        }

        /*
            SELECT     
            bc_patients.bc_bar_code AS F_HospSampleID, NULL AS F_HospSampleNumber, bc_patients.bc_in_no AS F_PatientNumber, 
            bc_patients.bc_bed_no AS F_BedNumber, bc_patients.bc_blood_date AS F_SamplingDate, 
            bc_patients.bc_name AS F_NAME, bc_patients.bc_in_no AS F_PatientID, (CASE WHEN bc_sex = '男' OR
            bc_sex = '1' THEN 1 WHEN bc_sex = '女' OR
            bc_sex = '2' THEN 0 ELSE 2 END) AS F_SEX, bc_patients.bc_birthday AS F_birthday, (CASE WHEN bc_patients.bc_age IS NOT NULL 
            THEN dbo.GET_NUMBER2(dbo.getAge(bc_patients.bc_age)) ELSE NULL END) AS F_AGE, (CASE WHEN bc_patients.bc_age IS NOT NULL 
            THEN dbo.CHINA_AND_CHAR_STR(dbo.getAge(bc_patients.bc_age)) ELSE NULL END) AS F_AGEUNIT, bc_patients.bc_tel AS F_PatientTel, 
            dict_depart.dep_name AS F_SectionOffice, dict_doctor.doc_name AS F_Doctor, NULL AS F_DoctorTel, 
            bc_patients.bc_diag AS F_Diagnostication, bc_cname.bc_lis_code AS F_ItemCode, 
            bc_cname.bc_his_name AS F_ItemName, 
            (CASE bc_patients.bc_ori_id WHEN '108' THEN '住院' WHEN '107' THEN '门诊' WHEN '109' THEN '体检' WHEN '110' THEN '外院' END) AS F_PatientSource, 
            (CASE bc_patients.bc_ori_id WHEN '108' THEN '住院' WHEN '107' THEN '门诊' WHEN '109' THEN '体检' WHEN '110' THEN '外院' END) AS F_Ori, 
            bc_patients.bc_sam_name AS F_SamName, '' AS F_Remark
            FROM bc_patients LEFT OUTER JOIN
            bc_cname ON bc_patients.bc_bar_no = bc_cname.bc_bar_no LEFT OUTER JOIN
            dict_doctor ON bc_patients.bc_doct_code = dict_doctor.doc_code LEFT OUTER JOIN
            dict_depart ON bc_patients.bc_d_code = dict_depart.dep_code
            WHERE     (bc_patients.bc_del <> '1') AND (bc_cname.bc_del <> '1')
         */
        //如果参数为空，结果也为空
        public static StringBuilder GetLisRequest(string hospSampleID)
        {
            if (String.IsNullOrEmpty(hospSampleID))
                return null;
            string barcode = hospSampleID.Replace("'", "''");
            StringBuilder result = new StringBuilder();
            string strTemp = ConfigurationManager.AppSettings["UploadLisRepDataFromSysName"];
            bool isBHHispital = ConfigurationManager.AppSettings["HospitalName"] == "深圳市龙岗宝荷医院";
            string strHospitalName = ConfigurationManager.AppSettings["HospitalName"];//医院名称
            EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(barcode);


            string strPatId = "";//pat_id


            EntityPatientQC qc = new EntityPatientQC();
            qc.RepBarCode = barcode;
            List<EntityPidReportMain> listPidReportMain = new PidReportMainBIZ().PatientQuery(qc);
            if (listPidReportMain != null && listPidReportMain.Count > 0)
            {
                strPatId = listPidReportMain[0].RepId;
            }

            result.Append(XML_TITLE).Append("<Data>");
            if (sampMain.ListSampDetail.Count > 0)
            {
                for (int i = 0; i < sampMain.ListSampDetail.Count; i++)
                {
                    if (sampMain != null && i == 0)
                    {
                        result.Append("<Data_Row>");
                        //医院条码
                        result.Append("<lis_Barcode>");
                        result.Append(sampMain.SampBarCode);
                        result.Append("</lis_Barcode>");

                        if (!string.IsNullOrEmpty(strHospitalName) && strHospitalName == "清远市中医院"
                             && sampMain.SrcName.ToString().Length > 0
                             && sampMain.SrcName == "门诊")
                        {
                            //清远市中医院 要求 门诊号那个字段不要填充
                            //病人id(住院号/门诊号/体检号)
                            result.Append("<pat_no>");
                            result.Append("");
                            result.Append("</pat_no>");
                        }
                        else
                        {
                            //病人id(住院号/门诊号/体检号)
                            result.Append("<pat_no>");
                            result.Append(sampMain.PidInNo);
                            result.Append("</pat_no>");
                            //病人id(住院号/门诊号/体检号)
                            result.Append("<pat_id>");
                            result.Append(sampMain.PidInNo);
                            result.Append("</pat_id>");
                        }

                        //病人姓名
                        result.Append("<pat_name>");
                        result.Append(sampMain.PidName);
                        result.Append("</pat_name>");


                        result.Append("<pat_bedno>");
                        result.Append(sampMain.PidBedNo);
                        result.Append("</pat_bedno>");


                        //采样时间（2013-08-29 15:56:56)
                        result.Append("<blood_time>");
                        result.Append(sampMain.CollectionDate);
                        result.Append("</blood_time>");
                        string sex = string.Empty;
                        if (sampMain.PidSex == "男" || sampMain.PidSex == "1")
                        {
                            sex = "1";
                        }
                        else if (sampMain.PidSex == "女" || sampMain.PidSex == "2")
                        {
                            sex = "2";
                        }
                        else
                        {
                            sex = "0";
                        }
                        //性别
                        result.Append("<pat_sex>");
                        result.Append(sex);
                        result.Append("</pat_sex>");

                        //出生日期
                        result.Append("<pat_birthday>");
                        result.Append(FormatDateTime(sampMain.PidBirthday));
                        result.Append("</pat_birthday>");

                        //年龄
                        string strAge = Convert.ToString(sampMain.PidAge);
                        if (!string.IsNullOrEmpty(strAge))
                        {
                            String pattern = @"(.*?)Y(.*?)M(.*?)D(.*?)H(.*?)I";
                            if (Regex.IsMatch(strAge, pattern))
                            {
                                Match match = Regex.Match(strAge, pattern);
                                if (!string.IsNullOrEmpty(match.Groups[1].Value) && match.Groups[1].Value != "0")
                                {
                                    AppendAge(result, match.Groups[1].Value, "0");

                                    if (!string.IsNullOrEmpty(match.Groups[2].Value) && match.Groups[2].Value != "0")
                                    {
                                        AppendAge2(result, match.Groups[2].Value, "1");
                                    }
                                }
                                else if (!string.IsNullOrEmpty(match.Groups[2].Value) && match.Groups[2].Value != "0")
                                {
                                    AppendAge(result, match.Groups[2].Value, "1");

                                    if (!string.IsNullOrEmpty(match.Groups[3].Value) && match.Groups[3].Value != "0")
                                    {
                                        AppendAge2(result, match.Groups[3].Value, "2");
                                    }
                                }
                                else if (!string.IsNullOrEmpty(match.Groups[3].Value) && match.Groups[3].Value != "0")
                                {
                                    AppendAge(result, match.Groups[3].Value, "2");

                                    if (!string.IsNullOrEmpty(match.Groups[4].Value) && match.Groups[4].Value != "0")
                                    {
                                        AppendAge2(result, match.Groups[4].Value, "3");
                                    }
                                }
                                else if (!string.IsNullOrEmpty(match.Groups[4].Value) && match.Groups[4].Value != "0")
                                {
                                    AppendAge(result, match.Groups[4].Value, "3");

                                    if (!string.IsNullOrEmpty(match.Groups[5].Value) && match.Groups[5].Value != "0")
                                    {
                                        AppendAge2(result, match.Groups[5].Value, "4");
                                    }
                                }
                                else if (!string.IsNullOrEmpty(match.Groups[5].Value) && match.Groups[5].Value != "0")
                                {
                                    AppendAge(result, match.Groups[5].Value, "4");
                                }
                                else
                                {
                                    result.Append("<pat_age/>");
                                    result.Append("<pat_ageunit/>");
                                }
                            }
                            else
                            {
                                result.Append("<pat_age/>");
                                result.Append("<pat_ageunit/>");
                            }
                        }
                        else
                        {
                            result.Append("<pat_age/>");
                            result.Append("<pat_ageunit/>");
                        }
                        ////年龄
                        //result.Append("<pat_age>");
                        //result.Append(r["F_AGE"]);
                        //result.Append("</pat_age>");

                        ////年龄单位
                        //result.Append("<pat_ageunit>");
                        //result.Append(r["F_AGEUNIT"]);
                        //result.Append("</pat_ageunit>");

                        //病人电话
                        result.Append("<pat_tel>");
                        result.Append(sampMain.PidTel);
                        result.Append("</pat_tel>");

                        //送检科室
                        result.Append("<dept_name>");
                        result.Append(sampMain.PidDeptName);
                        result.Append("</dept_name>");

                        //送检医生
                        result.Append("<doctor_name>");
                        result.Append(sampMain.PidDoctorName);
                        result.Append("</doctor_name>");

                        //送检医生电话
                        result.Append("<doctor_tel>");
                        result.Append("");
                        result.Append("</doctor_tel>");

                        //临床诊断
                        result.Append("<clinical_diag>");
                        result.Append(sampMain.PidDiag);
                        result.Append("</clinical_diag>");

                        ////医院项目名称
                        //result.Append("<lis_item_name>");
                        //result.Append(r["F_ItemName"]);
                        //result.Append("</lis_item_name>");

                        ////医院项目代码
                        //result.Append("<lis_item_code>");
                        //result.Append(r["F_ItemCode"]);
                        //result.Append("</lis_item_code>");

                        //样本名称??
                        result.Append("<samp_name>");
                        result.Append(sampMain.SampSamName);
                        result.Append("</samp_name>");

                        //病人来源
                        result.Append("<Pat_From>");
                        result.Append(sampMain.SrcName);
                        result.Append("</Pat_From>");

                        //报告id
                        result.Append("<Pat_ID>");
                        result.Append(strPatId);
                        result.Append("</Pat_ID>");
                    }
                    if (sampMain.ListSampDetail[i].ComId != null)
                    {
                        result.Append(GetLisSubItemsByComId(sampMain.ListSampDetail[i].ComId));
                    }
                }
                result.Append("</Data_Row>");
                result.Append("</Data>");
            }
            else
                return null;

            return result;
        }

        static void AppendAge(StringBuilder builder, string age, string unit)
        {
            //年龄
            builder.Append("<pat_age>");
            builder.Append(age);
            builder.Append("</pat_age>");
            //年龄单位
            builder.Append("<pat_ageunit>");
            builder.Append(unit);
            builder.Append("</pat_ageunit>");
        }

        static void AppendAge2(StringBuilder builder, string age, string unit)
        {
            //年龄
            builder.Append("<pat_age2>");
            builder.Append(age);
            builder.Append("</pat_age2>");
            //年龄单位
            builder.Append("<pat_ageunit2>");
            builder.Append(unit);
            builder.Append("</pat_ageunit2>");
        }

        private static string FormatDateTime(object obj)
        {

            if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()))
            {
                try
                {
                    return Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 更新数据到Mid数据库
        /// </summary>
        /// <param name="reportResult"></param>
        /// <returns></returns>
        public static string UploadLisRepData(string reportResult)
        {

            try
            {
                if (String.IsNullOrEmpty(reportResult))
                    return "error参数不能为空！";

                if (ConfigurationManager.AppSettings["EnableLog"] != null &&
                    ConfigurationManager.AppSettings["EnableLog"].ToUpper() == "Y")
                {
                    Logger.LogInfo(String.Format("金域发送文件原文：\r\n{0}", reportResult));
                }

                if (ConfigurationManager.AppSettings["EnableBase64Encode"] != null &&
                    ConfigurationManager.AppSettings["EnableBase64Encode"].ToUpper() == "Y")
                {
                    byte[] outputb = Convert.FromBase64String(reportResult);
                    string byteToEncoding = ConfigurationManager.AppSettings["byteToEncoding"];
                    if (!string.IsNullOrEmpty(byteToEncoding) && byteToEncoding == "1")
                    {
                        reportResult = Encoding.GetEncoding("gb2312").GetString(outputb);
                    }
                    else if (!string.IsNullOrEmpty(byteToEncoding) && byteToEncoding == "2")
                    {
                        //GBK
                        reportResult = Encoding.GetEncoding("GBK").GetString(outputb);
                    }
                    else if (!string.IsNullOrEmpty(byteToEncoding) && byteToEncoding == "3")
                    {
                        //GB18030
                        reportResult = Encoding.GetEncoding("GB18030").GetString(outputb);
                    }
                    else if (!string.IsNullOrEmpty(byteToEncoding) && byteToEncoding == "4")
                    {
                        //Default
                        reportResult = Encoding.Default.GetString(outputb);
                    }
                    else
                    {
                        reportResult = Encoding.GetEncoding("utf-8").GetString(outputb);
                    }

                    if (ConfigurationManager.AppSettings["EnableEncodeLog"] != null &&
                        ConfigurationManager.AppSettings["EnableEncodeLog"].ToUpper() == "Y")
                    {
                        Logger.LogInfo(String.Format("金域发送文件解码后数据：\r\n{0}", reportResult));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(String.Format("{1} UploadLisRepData 金域发送文件密文数据不对,异常详情:{0}", ex.Message, DateTime.Now));
                if (ConfigurationManager.AppSettings["EnableErrorEncodeLog"] != null &&
                    ConfigurationManager.AppSettings["EnableErrorEncodeLog"].ToUpper() == "Y")
                {
                    Logger.LogInfo(String.Format("金域发送文件解码后参数格式不对数据：\r\n{0}", reportResult));
                }
                return "error" + ex.Message;
            }


            if (String.IsNullOrEmpty(reportResult))
                return "error参数不能为空！";
            string errorMessage = "";
            XmlDocument document = new XmlDocument();
            //document.CreateXmlDeclaration("1.0", "utf-8", "yes");
            try
            {
                document.LoadXml(reportResult);
            }
            catch (Exception ee)
            {
                Logger.LogInfo(String.Format("{1} UploadLisRepData 参数格式不对,异常详情:{0}", ee.Message, DateTime.Now));
                if (ConfigurationManager.AppSettings["EnableErrorEncodeLog"] != null &&
                    ConfigurationManager.AppSettings["EnableErrorEncodeLog"].ToUpper() == "Y")
                {
                    Logger.LogInfo(String.Format("金域发送文件解码后参数格式不对数据：\r\n{0}", reportResult));
                }
                return "error" + ee.Message;
            }

            
            //根据配置节点InsertToLis来确定是要同步到lis数据库

            try
            {
                foreach (XmlNode n in document.SelectNodes("Report_Result/Report_Info"))
                {
                    try
                    {
                        AssemblyLisCommand alc = new AssemblyLisCommand(document);
                        alc.Start();
                    }
                    catch (Exception ee)
                    {
                        Logger.LogException(ee);
                        errorMessage = errorMessage + ee.Message;
                    }
                }
            }
            catch (Exception ae)
            {
                Logger.LogException(ae);
                errorMessage = errorMessage + ae.Message;
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                return "success";
            }
            else
            {
                return "error" + errorMessage;
            }
        }


        #region
        //        public static string InsertReportInfo
        //            = @"INSERT INTO lis_report_info
        //           ([report_info_id]
        //           ,[lis_Barcode]
        //           ,[ext_lab_code]
        //           ,[ext_Barcode]
        //           ,[ext_checkItem]
        //           ,[pat_name]
        //           ,[pat_age]
        //           ,[pat_height]
        //           ,[pat_wight]
        //           ,[pat_pre_week]
        //           ,[pat_id]
        //           ,[pat_bedNo]
        //           ,[pat_tel]
        //           ,[pat_sex]
        //           ,[pat_birthday]
        //           ,[pat_ori_name]
        //           ,[sam_name]
        //           ,[sam_state]
        //           ,[doctor_name]
        //           ,[dept_name]
        //           ,[clinical_diag]
        //           ,[blood_time]
        //           ,[ext_check_ID]
        //           ,[ext_receive_time]
        //           ,[ext_check_time]
        //           ,[ext_first_audit_time]
        //           ,[ext_ second_audit _time]
        //           ,[ext_upload_time]
        //           ,[ext_report_suggestion]
        //           ,[ext_report_remark]
        //           ,[ext_checker]
        //           ,[ext_first_audit]
        //           ,[ext_second_audit]
        //           ,[ext_intstrmt_name]
        //           ,[ext_lab_name]
        //           ,[ext_report_type]
        //           ,[ext_report_code])
        //     VALUES
        //           (<report_info_id, int,>
        //           ,<lis_Barcode, varchar(100),>
        //           ,<ext_lab_code, varchar(20),>
        //           ,<ext_Barcode, varchar(100),>
        //           ,<ext_checkItem, varchar(300),>
        //           ,<pat_name, varchar(20),>
        //           ,<pat_age, varchar(20),>
        //           ,<pat_height, varchar(20),>
        //           ,<pat_wight, varchar(20),>
        //           ,<pat_pre_week, varchar(20),>
        //           ,<pat_id, varchar(20),>
        //           ,<pat_bedNo, varchar(20),>
        //           ,<pat_tel, varchar(20),>
        //           ,<pat_sex, varchar(4),>
        //           ,<pat_birthday, datetime,>
        //           ,<pat_ori_name, varchar(50),>
        //           ,<sam_name, varchar(20),>
        //           ,<sam_state, varchar(100),>
        //           ,<doctor_name, varchar(20),>
        //           ,<dept_name, varchar(20),>
        //           ,<clinical_diag, varchar(200),>
        //           ,<blood_time, datetime,>
        //           ,<ext_check_ID, varchar(400),>
        //           ,<ext_receive_time, datetime,>
        //           ,<ext_check_time, datetime,>
        //           ,<ext_first_audit_time, datetime,>
        //           ,<ext_ second_audit _time, datetime,>
        //           ,<ext_upload_time, datetime,>
        //           ,<ext_report_suggestion, varchar(500),>
        //           ,<ext_report_remark, varchar(500),>
        //           ,<ext_checker, varchar(20),>
        //           ,<ext_first_audit, varchar(20),>
        //           ,<ext_second_audit, varchar(20),>
        //           ,<ext_intstrmt_name, varchar(50),>
        //           ,<ext_lab_name, varchar(50),>
        //           ,<ext_report_type, varchar(5),>
        //           ,<ext_report_code, varchar(50),>)";
        #endregion


        /// <summary>
        /// ReportInfo的相关命令,包含ResultInfo在内的三个表，共五个表。
        /// </summary>
        sealed class ReportInfoCommandAssemble
        {
            public ReportInfoCommandAssemble()
            {
                ResultInfoCommands = new List<ResultInfoCommandAssemble>();
                Report_pic = new List<SqlCommand>();
            }

            SqlCommand _Report_Info;
            public SqlCommand Report_Info
            {
                get { return this._Report_Info; }
                set { this._Report_Info = value; }
            }
            List<SqlCommand> _Report_pic;
            public List<SqlCommand> Report_pic
            {
                get
                {
                    return this._Report_pic;
                }
                set
                {
                    this._Report_pic = value;
                }
            }

            List<ResultInfoCommandAssemble> _ResultInfoCommands;
            public List<ResultInfoCommandAssemble> ResultInfoCommands
            {
                get
                {
                    return _ResultInfoCommands;
                }
                set
                {
                    _ResultInfoCommands = value;
                }
            }

            public bool IsOK = true;
        }

        /// <summary>
        /// ResultInfo的相关数据，包含三个表
        /// </summary>
        sealed class ResultInfoCommandAssemble
        {
            public ResultInfoCommandAssemble()
            {
                Result_pic = new List<SqlCommand>();
                Result_detail_info = new List<SqlCommand>();
            }
            SqlCommand _Result_info;
            public SqlCommand Result_info
            {
                get
                {
                    return this._Result_info;
                }
                set
                {
                    this._Result_info = value;
                }
            }

            List<SqlCommand> _Result_detail_info;
            public List<SqlCommand> Result_detail_info
            {
                get
                {
                    return this._Result_detail_info;
                }
                set
                {
                    this._Result_detail_info = value;
                }
            }

            List<SqlCommand> _Result_pic;
            public List<SqlCommand> Result_pic
            {
                get
                {
                    return this._Result_pic;
                }
                set
                {
                    this._Result_pic = value;
                }
            }
        }

        public static string AffirmRequest(string hospSampleID)
        {
            if (string.IsNullOrEmpty(hospSampleID))
            {
                return "参数不能为空";
            }
            bool result = false;
            try
            {
                EntitySampQC qc = new EntitySampQC();
                qc.ListSampBarId.Add(hospSampleID);
                List<EntitySampMain> list = new SampMainBIZ().SampMainQuery(qc).FindAll(w => w.SampStatusId == "0" || w.SampStatusId == "1" || w.SampStatusId == "2"
                                                                                                                                               || w.SampStatusId == "3" || w.SampStatusId == "4" || w.SampStatusId == "8" || w.SampStatusId == "9");
                if (list.Count > 0)
                {
                    EntitySampOperation operation = new EntitySampOperation();
                    operation.OperationStatus = "5";
                    operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                    result = new SampMainBIZ().UpdateSampMainStatus(operation, list);
                }
                if (result)
                {
                    EntitySampProcessDetail sampProcessDetail = new EntitySampProcessDetail();
                    sampProcessDetail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                    sampProcessDetail.ProcBarno = hospSampleID;
                    sampProcessDetail.ProcBarcode = hospSampleID;
                    sampProcessDetail.ProcStatus = "5";
                    sampProcessDetail.ProcContent = GetUploadLisRepDataFromSysName() + "已接收";
                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetail);
                }

            }
            catch (Exception ee)
            {
                Logger.LogException(ee);
                return ee.Message;
            }
            return string.Empty;
        }

        public static string RecordSampleStatus(string hospSampleID, string statusCode)
        {
            if (string.IsNullOrEmpty(hospSampleID) || string.IsNullOrEmpty(statusCode))
            {
                return "参数不能为空";
            }
            try
            {
                EntitySampProcessDetail sampProcessDetail = new EntitySampProcessDetail();
                sampProcessDetail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                sampProcessDetail.ProcBarno = hospSampleID;
                sampProcessDetail.ProcBarcode = hospSampleID;
                sampProcessDetail.ProcStatus = statusCode;
                sampProcessDetail.ProcContent = GetUploadLisRepDataFromSysName() + "操作";
                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetail);
            }
            catch (Exception ee)
            {
                Logger.LogException(ee);
                return ee.Message;
            }
            return string.Empty;
        }
    }

    public class AssemblyLisCommand
    {
        //当前的Report_Info节点
        XmlNode reportInfoNode;
        //整个XML文件
        XmlDocument document;
        List<EntityDicCombine> listCombine = new List<EntityDicCombine>();
        List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
        List<EntityDicResultTips> listTips = new List<EntityDicResultTips>();
        List<EntityDicDoctor> listDoc = new List<EntityDicDoctor>();
        //Patients表中的插入命令
        private EntityPidReportMain PidReportMainInsertEntity;
        //对应的多个Resulto表的插入命令
        private List<ResultCommadsAssembly> ResultCmdAssembly = new List<ResultCommadsAssembly>();
        //第三方编码
        private string lab;
        //转换后的lis组合码
        string comID;
        private string barcode = string.Empty;
        string pat_suggest = string.Empty;
        string pat_remark = string.Empty;
        string pat_first_audit_code = string.Empty;
        string pat_second_audit_code = string.Empty;
        DateTime pat_first_audit_time;
        DateTime pat_second_audit_time;


        public AssemblyLisCommand(XmlDocument document)
        {
            this.document = document;
            //AssemblyCommand();
        }

        public AssemblyLisCommand()
        { }


        public void Start()
        {
            PrepareData();
            foreach (XmlNode n in document.SelectNodes("Report_Result/Report_Info"))
            {
                this.AssemblyCommand(n);
                this.Execute();
            }
        }

        private void AssemblyCommand(XmlNode reportInfoNode)
        {
            if (reportInfoNode == null)
            {
                throw new Exception("传输数据格式不对！");
            }
            PidReportMainInsertEntity = new EntityPidReportMain();
            ResultCmdAssembly = new List<ResultCommadsAssembly>();
            lab = reportInfoNode.SelectSingleNode("ext_lab_code").InnerText;
            this.reportInfoNode = reportInfoNode;
            PidReportMainInsertEntity = GetPatientInsertEntity();

            bool isLoad = false;
            foreach (XmlNode n in reportInfoNode.SelectNodes("result_info"))
            {
                ResultCommadsAssembly rma = new ResultCommadsAssembly();
                if (String.IsNullOrEmpty(comID))
                    comID = n.SelectSingleNode("ext_combine_code").InnerText;
                rma.ResultCmd = GetResultoEntity(n);
                DateTime date = DateTime.MinValue;
                XmlNode dn = n.SelectSingleNode("result_date");
                if (dn != null)
                    DateTime.TryParse(dn.InnerText, out date);
                if (!isLoad)
                {
                    isLoad = true;
                }
                ResultCmdAssembly.Add(rma);
            }
            foreach (XmlNode nn in reportInfoNode.SelectNodes("report_pic"))
            {
                //rma.ResultPCmds.Add(GetResultPCmd(nn));
                //rma.ResultPCmds = GetResultPCmd(nn);
                ResultCommadsAssembly rma = new ResultCommadsAssembly();
                List<EntityObrResultImage> LsSqlcom_ResultP = GetResultPCmd(nn);
                if (LsSqlcom_ResultP != null && LsSqlcom_ResultP.Count > 0)
                {
                    foreach (EntityObrResultImage sqlcom_ResultP in LsSqlcom_ResultP)
                    {
                        rma.ResultPCmds.Add(sqlcom_ResultP);
                    }
                }
                ResultCmdAssembly.Add(rma);
            }
        }

        /// <summary>
        /// 需要对参数pat_id计算赋值
        /// </summary>
        /// <returns></returns>
        private EntityPidReportMain GetPatientInsertEntity()
        {
            pat_suggest = string.Empty;
            pat_remark = string.Empty;

            #region 接口注册

            ////<!--是否启用接口注册-->
            //if (ConfigurationManager.AppSettings["interface_regedit"] != null &&
            //    ConfigurationManager.AppSettings["interface_regedit"].ToUpper() == "Y")
            //{
            //    if (reportInfoNode.SelectSingleNode("interface_regedit") == null)
            //    {
            //        string int_regno = reportInfoNode.SelectSingleNode("interface_regedit").InnerText;
            //        DataRow drinterface_regedit = GetSysInterfaceRegeditInfo(int_regno);
            //        if (drinterface_regedit != null && drinterface_regedit["int_regno"].ToString().Length > 0
            //            && drinterface_regedit["int_flag"].ToString().Length > 0
            //            && drinterface_regedit["int_flag"].ToString() == "1")
            //        {
            //        }
            //        else
            //        {
            //            Logger.LogException(new Exception("节点(interface_regedit)的接口注册码无效,请重新注册"));
            //            throw new Exception("节点(interface_regedit)的接口注册码无效,请重新注册");
            //        }
            //    }
            //    else
            //    {
            //        Logger.LogException(new Exception("缺漏节点(接口注册码)interface_regedit"));
            //        throw new Exception("缺漏节点(接口注册码)interface_regedit");
            //    }
            //}

            #endregion


            pat_first_audit_code = string.Empty;
            pat_second_audit_code = string.Empty;
            pat_first_audit_time = DateTime.Now;
            pat_second_audit_time = DateTime.Now;
            bool DirectAuditFlag = System.Configuration.ConfigurationManager.AppSettings["DirectAuditFlag"] == "N";
            // Logger.LogException(new Exception("条码号为：" + reportInfoNode.SelectSingleNode("lis_Barcode").InnerText));
            EntitySampMain sampMain = GetPatientInfo(reportInfoNode.SelectSingleNode("lis_Barcode").InnerText);
            if (sampMain == null || string.IsNullOrEmpty(sampMain.SampBarCode))
            {
                Logger.LogException(new Exception("当前数据的医院条码在LIS数据库中不存在！条码号为：" + reportInfoNode.SelectSingleNode("lis_Barcode").InnerText));
                throw new Exception("当前数据的医院条码在LIS数据库中不存在！条码号为：" + reportInfoNode.SelectSingleNode("lis_Barcode").InnerText);
            }
            XmlNode n;
            EntityPidReportMain pidReportMian = new EntityPidReportMain();

            pidReportMian.PidComName = !string.IsNullOrEmpty(sampMain.SampComName)
                             ? sampMain.SampComName : string.Empty;
            pidReportMian.PidName = sampMain.PidName;
            string age = !string.IsNullOrEmpty(sampMain.PidAge)
                             ? sampMain.PidAge : string.Empty;
            pidReportMian.PidAgeExp = GetAge(age);
            object sexObj = sampMain.PidSex;
            if (sexObj != null && sexObj != DBNull.Value)
            {
                if (!String.IsNullOrEmpty(sexObj as string))
                {
                    string sex = sexObj as string;
                    if (sex.Contains("男") || sex == "1")
                        pidReportMian.PidSex = "1";
                    else if (sex.Contains("女") || sex == "2")
                        pidReportMian.PidSex = "2";
                    else
                        pidReportMian.PidSex = "0";
                }
            }
            List<EntityDicDoctor> rows = listDoc.FindAll(w => w.DoctorCode == sampMain.PidDoctorCode);
            if (rows != null && rows.Count > 0)
                pidReportMian.PidDoctorCode = rows[0].DoctorCode;
            pidReportMian.PidSrcId = sampMain.PidSrcId;
            pidReportMian.SampCollectionDate = sampMain.CollectionDate;
            n = reportInfoNode.SelectSingleNode("pat_height");
            if (n != null)
                pidReportMian.PidHeight = n.InnerText;
            n = reportInfoNode.SelectSingleNode("pat_wight");
            if (n != null)
                pidReportMian.PidWeight = n.InnerText;

            n = reportInfoNode.SelectSingleNode("pat_pre_week");
            if (n != null)
                pidReportMian.PidPreWeek = n.InnerText;

            pidReportMian.RepInputId = sampMain.PidPatno;

            n = reportInfoNode.SelectSingleNode("pat_id");
            if (n != null && !string.IsNullOrEmpty(n.InnerText))
            {
                pidReportMian.PidInNo = n.InnerText;
            }
            else
            {
                pidReportMian.PidInNo = sampMain.PidInNo;
            }

            n = reportInfoNode.SelectSingleNode("pat_bedNo");
            if (n != null)
                pidReportMian.PidBedNo = n.InnerText;

            n = reportInfoNode.SelectSingleNode("pat_tel");
            if (n != null)
                pidReportMian.PidTel = n.InnerText;

            n = reportInfoNode.SelectSingleNode("sam_state");
            if (n != null)
                pidReportMian.PidRemark = n.InnerText;

            n = reportInfoNode.SelectSingleNode("clinical_diag");
            if (n != null)
                pidReportMian.PidDiag = n.InnerText;


            DateTime date;
            n = reportInfoNode.SelectSingleNode("ext_check_time");
            if (n != null)
            {
                if (DateTime.TryParse(n.InnerText, out date))
                    pidReportMian.SampCheckDate = date;
            }

            if (!DirectAuditFlag)
            {
                n = reportInfoNode.SelectSingleNode("ext_first_audit_time");
                if (n != null)
                {
                    if (DateTime.TryParse(n.InnerText, out date))
                    {
                        pat_first_audit_time = date;
                        pidReportMian.RepAuditDate = date; ;
                    }
                }
            }
            if (!DirectAuditFlag)
            {
                n = reportInfoNode.SelectSingleNode("ext_second_audit_time");
                if (n != null)
                {
                    if (DateTime.TryParse(n.InnerText, out date))
                    {
                        pat_second_audit_time = date;
                        pidReportMian.RepReportDate = date;
                    }
                }
            }

            string cmm;
            n = reportInfoNode.SelectSingleNode("ext_report_suggestion");
            cmm = n == null ? "" : n.InnerText;
            n = reportInfoNode.SelectSingleNode("ext_report_remark");
            cmm = cmm + (n == null ? "" : n.InnerText);

            XmlNodeList xmlNodeList = reportInfoNode.SelectNodes("result_info");
            if (xmlNodeList != null)
            {
                foreach (XmlNode ns in xmlNodeList)
                {
                    XmlNode n1 = ns.SelectSingleNode("result_suggestion");
                    if (n1 != null && !cmm.Contains(n1.InnerText))
                    {
                        cmm = cmm + n1.InnerText + " ";
                    }
                }
            }
            pat_suggest = cmm;
            pidReportMian.RepComment = cmm;

            string exp = string.Empty;
            xmlNodeList = reportInfoNode.SelectNodes("result_info");
            if (xmlNodeList != null)
            {
                foreach (XmlNode ns in xmlNodeList)
                {
                    XmlNode n1 = ns.SelectSingleNode("result_remark");
                    if (n1 != null && !exp.Contains(n1.InnerText))
                    {
                        exp = exp + n1.InnerText + " ";
                    }
                }
            }
            pat_remark = exp;
            pidReportMian.RepRemark = exp;

            bool directReportFlag = System.Configuration.ConfigurationManager.AppSettings["DirectReportFlag"] == "Y";
            bool DirectPrintFlag = System.Configuration.ConfigurationManager.AppSettings["DirectPrintFlag"] == "Y";
            if (DirectPrintFlag)
            {
                n = reportInfoNode.SelectSingleNode("ext_first_audit");
                if (n != null)
                {
                    pat_first_audit_code = n.InnerText;
                    pidReportMian.RepAuditUserId = n.InnerText;
                }

                n = reportInfoNode.SelectSingleNode("ext_second_audit");
                if (n != null)
                {
                    pat_second_audit_code = n.InnerText;
                    pidReportMian.RepReportUserId = n.InnerText;
                }

                pidReportMian.RepStatus = 4;
            }
            else if (directReportFlag)
            {
                n = reportInfoNode.SelectSingleNode("ext_first_audit");
                if (n != null)
                {
                    pat_first_audit_code = n.InnerText;
                    pidReportMian.RepAuditUserId = n.InnerText;
                }
                n = reportInfoNode.SelectSingleNode("ext_second_audit");
                if (n != null)
                {
                    pat_second_audit_code = n.InnerText;
                    pidReportMian.RepReportUserId = n.InnerText;

                }
                pidReportMian.RepStatus = 2;
            }
            else if (DirectAuditFlag)
            {
                pidReportMian.RepAuditUserId = "";
                pidReportMian.RepReportUserId = "";
                pidReportMian.RepStatus = 0;
            }
            else
            {
                n = reportInfoNode.SelectSingleNode("ext_second_audit");
                if (n != null)
                    pidReportMian.RepAuditUserId = n.InnerText;

                pidReportMian.RepReportUserId = null;

                pidReportMian.RepStatus = 1;
            }

            pidReportMian.RepInDate = DateTime.Now;

            pidReportMian.PidSamId = sampMain.SampSamId;
            n = reportInfoNode.SelectSingleNode("ext_checker");
            if (n != null)
            {
                pidReportMian.RepCheckUserId = n.InnerText;
            }
            else
            {
                n = reportInfoNode.SelectSingleNode("ext_first_audit");
                if (n != null)
                    pidReportMian.RepCheckUserId = n.InnerText;
            }
            n = reportInfoNode.SelectSingleNode("lis_Barcode");
            if (n != null)
            {
                pidReportMian.RepBarCode = n.InnerText;
                barcode = n.InnerText;
            }

            pidReportMian.SampReceiveDate = sampMain.SampOccDate;

            pidReportMian.SampSendDate = sampMain.ReachDate;

            pidReportMian.SampApplyDate = sampMain.ReceiverDate;

            pidReportMian.PidDeptId = sampMain.PidDeptCode;

            pidReportMian.PidDeptName = sampMain.PidDeptName;

            object obj = sampMain.SampUrgentFlag;
            pidReportMian.RepCtype = "1";
            if (obj != null && obj != DBNull.Value)
            {
                if ((bool)obj)
                {
                    pidReportMian.RepCtype = "2";
                }
            }

            pidReportMian.PidIdtId = sampMain.PidIdtId;

            pidReportMian.PidOrgId = "outreport"; //此值表明当前检验结果来自第三方

            pidReportMian.PidAddmissTimes = string.IsNullOrEmpty(sampMain.PidAdmissTimes.ToString()) ? 0 : Convert.ToInt16(sampMain.PidAdmissTimes.ToString());

            return pidReportMian;
        }

        private EntityObrResult GetResultoEntity(XmlNode node)
        {
            XmlNode n;
            EntityObrResult obrResult = new EntityObrResult();
            n = node.SelectSingleNode("result");
            if (n != null)
                obrResult.ObrValue = n.InnerText;
            n = node.SelectSingleNode("result_reference");
            if (n != null)
            {
                string strRef = n.InnerText.Replace("[", "").Replace("]", "").TrimStart().TrimEnd();
                string[] str = strRef.Split('-');
                if (str != null && str.Length > 1)
                {
                    obrResult.RefLowerLimit = str[0];
                    obrResult.RefUpperLimit = str[1];
                }
            }
            n = node.SelectSingleNode("result_unit");
            if (n != null)
                obrResult.ObrUnit = n.InnerText;
            n = node.SelectSingleNode("result_date");
            DateTime date;
            if (n != null && !String.IsNullOrEmpty(n.InnerText))
            {
                if (DateTime.TryParse(n.InnerText, out date))
                    obrResult.ObrDate = date;
            }

            obrResult.ObrFlag = 1;

            n = node.SelectSingleNode("result_test_method");
            if (n != null)
                obrResult.ObrItmMethod = n.InnerText;
            n = node.SelectSingleNode("lis_combine_code");
            if (n != null)
            {
                obrResult.ItmComId = n.InnerText;
            }
            n = node.SelectSingleNode("lis_item_code");
            if (n != null)
            {
                obrResult.ItmId = n.InnerText;
            }

            List<EntityDicItmItem> listItm = listItem.FindAll(w => w.ItmId == obrResult.ItmId);
            string ecd = string.Empty;
            if (listItm != null && listItm.Count > 0 && !string.IsNullOrEmpty(listItm[0].ItmEcode))
                ecd = listItm[0].ItmEcode;
            obrResult.ItmEname = ecd;

            obrResult.ObrType = 1;

            SqlParameter res_ref_flag = new SqlParameter("@res_ref_flag", SqlDbType.VarChar);
            List<EntityDicResultTips> rows = listTips.FindAll(w => w.TipValue == node.SelectSingleNode("result_flag").InnerText);
            if (rows != null && rows.Count > 0)
                res_ref_flag.Value = rows[0].TipId;


            return obrResult;
        }

        private List<EntityObrResultImage> GetResultPCmd(XmlNode node)
        {
            XmlNode n;
            n = node.SelectSingleNode("pic_content");
            XmlNode n_pic_name;
            n_pic_name = node.SelectSingleNode("pic_name");
            List<EntityObrResultImage> listResultImage = new List<EntityObrResultImage>();
            List<byte[]> bList = new List<byte[]>();
            if (n != null && !string.IsNullOrEmpty(n.InnerText))
            {
                if (lab.Contains("达安") && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UndoReportNotDeleteData"])
                && System.Configuration.ConfigurationManager.AppSettings["EnablePDFEncode"].ToUpper() == "Y")
                    bList = ConvertPDF2Image(Convert.FromBase64String(n.InnerText));
                else if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["EnablePDFEncode"])
                    && System.Configuration.ConfigurationManager.AppSettings["EnablePDFEncode"].ToUpper() == "Y")
                    bList = ConvertPDF2Image(Convert.FromBase64String(n.InnerText));
                else
                    bList.Add(Convert.FromBase64String(n.InnerText));
            }
            if (bList != null && bList.Count > 0)
            {
                foreach (byte[] bItem in bList)
                {
                    EntityObrResultImage resultImage = new EntityObrResultImage();

                    if (n_pic_name != null && !string.IsNullOrEmpty(n_pic_name.InnerText))
                    {
                        resultImage.ObrItmEname = n_pic_name.InnerText;
                    }
                    resultImage.ObrImage = bItem;
                    resultImage.ObrFlag = 1;
                    listResultImage.Add(resultImage);
                }
            }
            else
            {
                EntityObrResultImage resultImage = new EntityObrResultImage();

                resultImage.ObrFlag = 1;

                listResultImage.Add(resultImage);
            }
            return listResultImage;
        }

        private List<byte[]> ConvertPDF2Image(byte[] p)
        {
            //Image[] images = new Image[0];
            List<byte[]> b = new List<byte[]>();
            try
            {
                MemoryStream ms = new MemoryStream(p);
                //File.WriteAllBytes(@"c:\test.pdf", p);
                ms.Position = 0;

                //if (System.Configuration.ConfigurationManager.AppSettings["HospitalName"] == "石岩医院")
                //{
                //    Aspose.Pdf.Document document = new Aspose.Pdf.Document(ms);
                //    var device = new Aspose.Pdf.Devices.JpegDevice(96);
                //    //遍历每一页转为jpg
                //    for (var i = 1; i <= document.Pages.Count; i++)
                //    {
                //        //string filePathOutPut = Path.Combine(directoryPath, string.Format("{0}.jpg", i));
                //        MemoryStream jepgms = new MemoryStream();
                //        try
                //        {
                //            device.Process(document.Pages[i], jepgms);
                //            b.Add(jepgms.GetBuffer());
                //            //File.WriteAllBytes(@"c:\test1.png", jepgms.GetBuffer());
                //            jepgms.Close();
                //        }
                //        catch (Exception ex)
                //        {
                //            jepgms.Close();
                //            //File.Delete(filePathOutPut);
                //        }
                //    }
                //    document.Dispose();
                //}
                //else
                //{
                PDFFile file = PDFFile.Open(ms);
                //images = new Image[file.PageCount];
                for (int i = 0; i < file.PageCount; i++)
                {
                    // Convert each page to bitmap and save it.
                    MemoryStream jepgms = new MemoryStream();
                    Bitmap pageImage = file.GetPageImage(i, 200);
                    pageImage.Save(jepgms, ImageFormat.Png);
                    //File.WriteAllBytes(@"c:\a.Png", jepgms.GetBuffer());
                    b.Add(jepgms.GetBuffer());
                    //images[i] = Image.FromStream(jepgms);
                    jepgms.Close();
                }
                //ms.Close();
                file.Dispose();
                //}
                ms.Close();
                //file.Dispose();
                return b;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("PDF转图片出错", ex);
                return null;
            }


        }

        string GetAge(string age)
        {
            try
            {
                string retAge = age;
                if (!string.IsNullOrEmpty(age))
                {
                    //目前只截取年
                    if (age.ToLower().Contains('y')
                        && age.ToLower().Contains('m')
                        && age.ToLower().Contains('d')
                        && age.ToLower().Contains('h')
                        && age.ToLower().Contains('i')
                        )
                    {
                        return retAge;
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
                            else
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
                        retAge = age;
                    }
                }
                return retAge;
            }
            catch (Exception)
            {
                return "0Y0M0D0H0I";
            }

        }

        private EntitySampMain GetPatientInfo(string barCode)
        {
            EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(barCode);
            return sampMain;
        }


        public void Execute()
        {
            string notePatId = "";
            try
            {
                SqlCommand cmd = new SqlCommand();
                DateTime currentDate = DateTime.Now;
                EntityPatientQC qc = new EntityPatientQC();
                qc.DateStart = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd"));
                qc.DateEnd = Convert.ToDateTime(currentDate.AddDays(1).ToString("yyyy-MM-dd"));
                List<EntityPidReportMain> listPidReportMain = new PidReportMainBIZ().GetPatientsCount(qc);
                string index = ((int)listPidReportMain.Count + 1).ToString();
                string pat_id;
                string itr = System.Configuration.ConfigurationManager.AppSettings["InsertItrID"];
                //config配置:组合id关联对应仪器id(格式:组合ID1,仪器ID1;组合ID2,仪器ID2)
                string cf_comIdAndItrIDs = System.Configuration.ConfigurationManager.AppSettings["comIdAndItrIDs"];
                if (string.IsNullOrEmpty(itr))
                {
                    //cmd.CommandText = "select top 1 itr_id from dict_instrmt_com where com_id='" +
                    //                  GetLisComID(comID, lab) + "'";
                    //object obj = cmd.ExecuteScalar();
                    //itr = obj == null ? "" : obj as string;
                }
                if (!string.IsNullOrEmpty(cf_comIdAndItrIDs))
                {
                    #region 特定组合id转用特定仪器id


                    List<string> ltComID = new List<string>();//组合
                    List<string> ltItrID = new List<string>();//仪器

                    //格式:组合ID1,仪器ID1;组合ID2,仪器ID2
                    string[] splLtComAndItr = cf_comIdAndItrIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string fcstrComAndItr in splLtComAndItr)
                    {
                        string[] splComAndItr = fcstrComAndItr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (splComAndItr.Length >= 2
                            && !string.IsNullOrEmpty(splComAndItr[0])
                            && !string.IsNullOrEmpty(splComAndItr[1]))
                        {
                            ltComID.Add(splComAndItr[0]);
                            ltItrID.Add(splComAndItr[1]);
                        }
                    }

                    if (ltComID.Count > 0 && ResultCmdAssembly != null && ResultCmdAssembly.Count > 0)
                    {
                        foreach (ResultCommadsAssembly c in ResultCmdAssembly)
                        {
                            if (!string.IsNullOrEmpty(c.ResultCmd.ItmComId))
                            {
                                string strTemp_res_com_id = c.ResultCmd.ItmComId;
                                //是否有对应的组合id
                                if (ltComID.Contains(strTemp_res_com_id))
                                {
                                    //有对应的组合id,用转到对应的仪器id
                                    itr = ltItrID[ltComID.FindIndex(i => i == strTemp_res_com_id)];
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                }
                bool isNeedSavePatients = true;
                List<EntityPidReportMain> listPid = new List<EntityPidReportMain>();
                string pat_flag = "0";
                if (string.IsNullOrEmpty(barcode))
                {
                    pat_id = itr + DateTime.Now.ToString("yyyyMMdd") + index;
                }
                else
                {
                    index = barcode;
                    EntityPatientQC patientQc = new EntityPatientQC();
                    listPid = new List<EntityPidReportMain>();
                    qc.RepBarCode = barcode;
                 //   qc.ListItrId.Add(itr);
                    listPid = new PidReportMainBIZ().PatientQuery(qc);

                    if (listPid.Count > 0)
                    {
                        pat_id = listPid[0].RepId;
                        pat_flag = listPid[0].RepStatus == null ? "0" : listPid[0].RepStatus.ToString();
                        isNeedSavePatients = false;
                    }
                    else
                    {
                        pat_id = itr + DateTime.Now.ToString("yyyyMMdd") + index;
                        EntityPidReportMain pidReport = new PidReportMainBIZ().GetPatientByPatId(pat_id);
                        if (pidReport != null && !string.IsNullOrEmpty(pidReport.RepId))
                        {
                            pat_id = pidReport.RepId;
                            pat_flag = pidReport.RepStatus == null ? "0" : pidReport.RepStatus.ToString();
                            isNeedSavePatients = false;
                        }
                        else
                        {
                            isNeedSavePatients = true;
                        }
                    }
                }
                notePatId = pat_id;
                if (isNeedSavePatients)
                {
                    //Logger.LogException(new Exception("保存病人信息前"));
                    PidReportMainInsertEntity.RepId = pat_id;
                    PidReportMainInsertEntity.RepSid = index;

                    PidReportMainInsertEntity.RepItrId = itr;
                    List<EntityPidReportMain> listMain = new List<EntityPidReportMain>();
                    listMain.Add(PidReportMainInsertEntity);
                    new PidReportMainBIZ().InsertNewPatient(listMain);
                    if (string.IsNullOrEmpty(PidReportMainInsertEntity.RepReportUserId))
                    {
                        PidReportMainInsertEntity.RepReportUserId =
                            PidReportMainInsertEntity.RepAuditUserId;
                    }
                    EntitySampProcessDetail sampProcessDetail = new EntitySampProcessDetail();
                    sampProcessDetail.ProcDate = PidReportMainInsertEntity.RepReportDate ?? DateTime.Now;
                    sampProcessDetail.ProcBarno = barcode;
                    sampProcessDetail.ProcBarcode = barcode;
                    sampProcessDetail.ProcStatus = "60";
                    sampProcessDetail.ProcUsername = PidReportMainInsertEntity.RepReportUserId;
                    sampProcessDetail.ProcContent = WFOutSend.GetUploadLisRepDataFromSysName() + "操作";
                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetail);
                }
                else
                {
                    bool NotAllowEditReportData = System.Configuration.ConfigurationManager.AppSettings["NotAllowEditReportData"] == "Y";

                    if (NotAllowEditReportData && (pat_flag == "2" || pat_flag == "4"))
                    {
                        Logger.LogInfo(String.Format("【已报告，过滤，pat_id = '{0}'】", pat_id));
                        return;
                    }

                    string appendSql = string.Empty;
                    EntityPidReportMain updateReportMain = new EntityPidReportMain();
                    if (listPid.Count > 0)
                    {
                        updateReportMain = listPid[0];
                    }
                    if (!string.IsNullOrEmpty(pat_suggest))
                    {
                        appendSql = "update";
                        updateReportMain.RepComment = pat_suggest;
                    }
                    if (!string.IsNullOrEmpty(pat_remark))
                    {
                        appendSql = "update";
                        updateReportMain.RepRemark = pat_remark;
                    }
                    bool DirectPrintFlag = System.Configuration.ConfigurationManager.AppSettings["DirectPrintFlag"] == "Y";
                    bool directReportFlag = System.Configuration.ConfigurationManager.AppSettings["DirectReportFlag"] == "Y";
                    if (directReportFlag || DirectPrintFlag)
                    {
                        appendSql = "update";
                        int patflag = DirectPrintFlag ? 4 : 2;
                        updateReportMain.RepAuditUserId = pat_first_audit_code;
                        updateReportMain.RepReportUserId = pat_second_audit_code;
                        updateReportMain.RepAuditDate = Convert.ToDateTime(pat_first_audit_time.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateReportMain.RepReportDate = Convert.ToDateTime(pat_second_audit_time.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateReportMain.RepStatus = patflag;
                    }
                    if (!string.IsNullOrEmpty(appendSql))
                    {
                        List<EntityPidReportMain> listUpdatePidReport = new List<EntityPidReportMain>();
                        listUpdatePidReport.Add(updateReportMain);
                        new PidReportMainBIZ().UpdatePatientData(listUpdatePidReport);
                    }

                    string bRecord = System.Configuration.ConfigurationManager.AppSettings["RegisterBCAddBcSignRecord"];
                    if (!string.IsNullOrEmpty(bRecord) && bRecord.ToUpper() == "Y")
                    {
                        EntitySampProcessDetail sampProcessDetail = new EntitySampProcessDetail();
                        sampProcessDetail.ProcDate = PidReportMainInsertEntity.RepReportDate ?? DateTime.Now;
                        sampProcessDetail.ProcBarno = barcode;
                        sampProcessDetail.ProcBarcode = barcode;
                        sampProcessDetail.ProcStatus = "60";
                        sampProcessDetail.ProcUsername = PidReportMainInsertEntity.RepReportUserId;
                        sampProcessDetail.ProcContent = WFOutSend.GetUploadLisRepDataFromSysName() + "操作";
                        new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetail);
                    }
                }

                int count = 1;
                List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);
                if (listDetail.Count > 0)
                {
                    count = listDetail.Count + 1;
                }
                List<string> list = new List<string>();
                List<EntityObrResult> listRes = new ObrResultBIZ().ObrResultQueryByObrId(pat_id);
                List<EntitySampDetail> listMi = new SampDetailBIZ().GetSampDetail(barcode).FindAll(w => w.DelFlag != "1" && w.ComId != null || w.ComId != "");

                List<EntityObrResultImage> listResPic = new ObrResultImageBIZ().GetObrResultImage(pat_id);

                bool isexitpic = false;

                foreach (ResultCommadsAssembly c in ResultCmdAssembly)
                {
                    string ecd = string.Empty;
                    if (c.ResultCmd != null)
                    {
                        if (listRes.Count > 0)
                        {
                            List<EntityObrResult> listResult = listRes.FindAll(w => w.ItmId == c.ResultCmd.ItmId);
                            if (listResult.Count > 0)
                            {
                                //EntityResultQC resultQc = new EntityResultQC();
                                //resultQc.ListObrId.Add(pat_id);
                                //resultQc.listItmIds.Add(c.ResultCmd.ItmId);
                                //new ObrResultBIZ().UpdateObrFlagByCondition(resultQc);
                                new ObrResultBIZ().DeleteObrResultByObrSn(listResult[0].ObrSn.ToString());
                            }
                        }
                        c.ResultCmd.ObrItrId = itr;
                        c.ResultCmd.ObrSid = index;
                        ecd = c.ResultCmd.ItmEname;
                        c.ResultCmd.ItmEname = ecd ?? "";
                        c.ResultCmd.ObrId = pat_id;

                        List<EntityDicItmItem> rr = listItem.FindAll(w => w.ItmId == c.ResultCmd.ItmId);
                        if (rr != null && rr.Count > 0)
                            c.ResultCmd.ItmReportCode = rr[0].ItmRepCode;
                        new ObrResultBIZ().InsertObrResult(c.ResultCmd);
                        bool isNeedSaveMi = true;
                        if (listDetail.Count > 0 && !string.IsNullOrEmpty(c.ResultCmd.ItmComId))
                        {
                            List<EntityPidReportDetail> rows = listDetail.FindAll(w => w.RepId == pat_id && w.ComId == c.ResultCmd.ItmComId);
                            if (rows.Count > 0) isNeedSaveMi = false;
                        }
                        if (isNeedSaveMi && !string.IsNullOrEmpty(c.ResultCmd.ItmComId)
                            && !list.Contains((c.ResultCmd.ItmComId)))
                        {
                            list.Add(c.ResultCmd.ItmComId);
                            string yzid = string.Empty;

                            List<EntitySampDetail> rows = listMi.FindAll(w => w.ComId == c.ResultCmd.ItmComId);

                            if (rows.Count > 0)
                            {
                                yzid = rows[0].ComId;
                            }
                            EntityPidReportDetail entityReportDetail = new EntityPidReportDetail();
                            entityReportDetail.RepId = pat_id;
                            entityReportDetail.ComId = c.ResultCmd.ItmComId;
                            entityReportDetail.OrderSn = yzid;
                            entityReportDetail.SortNo = count;
                            entityReportDetail.RepBarCode = reportInfoNode.SelectSingleNode("lis_Barcode").InnerText;
                            List<EntityPidReportDetail> listInsertDetail = new List<EntityPidReportDetail>();
                            listInsertDetail.Add(entityReportDetail);
                            new PidReportDetailBIZ().InsertNewReportDetail(listInsertDetail);
                        }
                    }
                    //存在重传报告情况，重传的报告无法覆盖
                    bool UsingLastPic = System.Configuration.ConfigurationManager.AppSettings["UsingLastPic"] == "Y";
                    bool isDeletePic = false;
                    foreach (EntityObrResultImage cm in c.ResultPCmds)
                    {
                        if (!isDeletePic && UsingLastPic)
                        {
                            new ObrResultImageBIZ().DeletePatPhotoResultByObrId(pat_id);
                            isDeletePic = true;
                        }

                        if (!UsingLastPic)
                        {
                            bool isbreak = false;
                            if (listResPic.Count > 0
                                && cm.ObrImage != null)
                            {
                                try
                                {
                                    byte[] newpic = (byte[])cm.ObrImage;
                                    foreach (EntityObrResultImage row in listResPic)
                                    {
                                        if (row.ObrImage == null) continue;
                                        byte[] pic = (byte[])row.ObrImage;

                                        if (newpic.Length == pic.Length && pic[0] == newpic[0] && pic[1] == newpic[1]
                                            && pic[pic.Length - 1] == newpic[pic.Length - 1])
                                        {
                                            isbreak = true;
                                            break;
                                        }
                                    }
                                }
                                catch
                                { continue; }
                            }
                            if (isbreak) continue;
                        }
                        if (ecd.Length > 12)
                        {
                            ecd = "jyreprot";
                        }
                        cm.ObrId = pat_id;
                        if (cm.ObrItmEname == null || string.IsNullOrEmpty(cm.ObrItmEname.Trim()))
                        {
                            cm.ObrItmEname = ecd;
                        }
                        cm.ObrDate = currentDate;
                        cm.ObrItrId = itr;
                        cm.ObrSid = Convert.ToDecimal(index); ;
                        new ObrResultImageBIZ().SaveObrResultImage(cm);
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.LogException(new Exception(ee.ToString() + "\r\n[pat_id=" + notePatId + "]"));
                throw ee;
            }
        }

        public delegate void UploadFileAndDataDelegate(string patId);


        private void PrepareData()
        {
            EntityResponse responseCom = new CacheDataBIZ().GetCacheData("EntityDicCombine");
            listCombine = responseCom.GetResult() as List<EntityDicCombine>;

            EntityResponse responseItem = new CacheDataBIZ().GetCacheData("EntityDicItmItem");
            listItem = responseItem.GetResult() as List<EntityDicItmItem>;

            EntityResponse responseTips = new CacheDataBIZ().GetCacheData("EntityDicResultTips");
            listTips = responseTips.GetResult() as List<EntityDicResultTips>;

            EntityResponse responseDoc = new CacheDataBIZ().GetCacheData("EntityDicDoctor");
            listDoc = responseDoc.GetResult() as List<EntityDicDoctor>;

        }
        class ResultCommadsAssembly
        {
            public EntityObrResult ResultCmd;

            public List<EntityObrResultImage> ResultPCmds;

            public ResultCommadsAssembly()
            {
                ResultPCmds = new List<EntityObrResultImage>();
            }
        }
    }
}