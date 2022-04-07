using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using dcl.root.dac;
using lis.dto;
using IBatisNet.DataMapper;
using lis.dto.Entity;
using lis.dto.BarCodeEntity;
using System.Collections;
using dcl.common.extensions;
using dcl.common;
using Lib.DAC;
using Lib.EntityDAC;
using System.IO;
using Lib.DataInterface.Implement;
using System.Text.RegularExpressions;
using Lib.LogManager;
using Lis.HQHisInterface;
using Lis.HQHisInterface.HISRequest;
using Lis.SZRYHis.Interface;
using System.Threading;
using System.Xml;
using Lis.SendDataToLisCDR;
using dcl.root.dto;
using dcl.servececontract;
using dcl.pub.entities;
using dcl.svr.interfaces;
using dcl.svr.cache;
using dcl.pub.entities.dict;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.sample
{
    public class BarcodeBIZ : IBarcode
    {
        static bool DebugMode = false;
        static BarcodeBIZ()
        {
            string DebugModeStr = System.Configuration.ConfigurationSettings.AppSettings["DebugMode"];
            if (!string.IsNullOrEmpty(DebugModeStr) && (DebugModeStr == "1" || DebugModeStr.ToLower() == "true"))
            {
                showLogger = true;
                DebugMode = true;
            }
        }
        private DbBase dao = DbBase.InConn();
        IBatisSQLHelper sqlHelper = new IBatisSQLHelper();
        BarcodeDictBIZ bcDictBIZ = new BarcodeDictBIZ();
        static bool showLogger = false;
        System.Diagnostics.Stopwatch watch;

        /// <summary>
        /// 转换(三水写死)
        /// </summary>
        /// <param name="strPatId"></param>
        /// <returns></returns>
        public DataSet ConverPatientID(string strPatId)
        {
            DataSet dspatID = new DataSet();
            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder builder = Lib.DAC.ConnectionStringProvider.ConnectionStrBuilderHelper.CreateConnStrBuilder(EnumDbDriver.Oracle, EnumDataBaseDialet.Oracle10g);

            builder.DbName = "ORCL";
            builder.Server = "ORCL";
            builder.LoginName = "spi";
            builder.LoginPassword = "spi";

            SqlHelper helper = new SqlHelper(builder.Build(), EnumDbDriver.Oracle, EnumDataBaseDialet.Oracle10g);

            //Logger.WriteException("门诊卡号转换", "门诊卡号转换_转换前", string.Format("this.DownLoadInfo.PatientID={0}", this.DownLoadInfo.PatientID));

            if (strPatId != null && (strPatId.Length == 8 || strPatId.Length == 19))
            {
                dspatID = helper.GetDataSet(string.Format("select VISIT_NO from spi_v_medi_visit where FCARDNO = '{0}'", strPatId));
                //object obj_VISIT_NO = helper.ExecuteScalar(string.Format("select VISIT_NO from spi_v_medi_visit where FCARDNO = '{0}'", this.DownLoadInfo.PatientID));
                //if (obj_VISIT_NO != null)
                //{
                //   // Logger.WriteException("门诊卡号转换", "门诊卡号转换", string.Format("FCARDNO={0},VISIT_NO={1}", strPatId, obj_VISIT_NO));

                //    //this.DownLoadInfo.PatientID = obj_VISIT_NO.ToString();
                //}
            }
            return dspatID;
        }





        public DataTable FindAllMessage()
        {
            DBHelper helper = new DBHelper();
            DataTable result = helper.GetTable(string.Format("SELECT * FROM {0} ", BarcodeTable.Message.TableName));
            result.TableName = BarcodeTable.Message.TableName;
            return result;
        }

        /// <summary>
        /// 根据条码号获取用户检验组合
        /// </summary>
        /// <param name="bc_bar_code"></param>
        /// <param name="onlyGetUnregisterCombine">是否只获取未登记组合：bc_flag不为1的记录</param>
        /// <returns></returns>
        public DataTable GetPatientCombine(string bc_bar_code, bool onlyGetUnregisterCombine)
        {
            string sqlSelect = string.Format(@"
select
*,
--bc_lis_code,
dict_combine.com_name as bc_lis_code_name
from {0}   
inner join dict_combine on dict_combine.com_id = {0}.bc_lis_code and dict_combine.com_del <> '1'
where bc_del <> '1' and bc_bar_code='{1}'", BarcodeTable.CName.TableName, bc_bar_code);

            if (onlyGetUnregisterCombine)
            {
                sqlSelect += " and (bc_flag = 0 or bc_flag is null) ";
            }

            DBHelper helper = new DBHelper();
            DataTable result = helper.GetTable(sqlSelect);
            result.TableName = BarcodeTable.CName.TableName;
            return result;
        }

        /// <summary>
        /// 根据条码号获取用户检验组合
        /// </summary>
        /// <param name="bc_bar_code"></param>
        /// <returns></returns>
        public DataTable GetPatientCombine(string bc_bar_code)
        {
            return GetPatientCombine(bc_bar_code, false);
        }

        public IList<ReturnMessages> FindAllReturnMessage(ReturnMessages searchInfo)
        {

            IList<ReturnMessages> messages;
            if (searchInfo != null && !string.IsNullOrEmpty(searchInfo.DeptCode)) //住院条码根据科室过滤
            {
                if (searchInfo.DeptCode.Contains("&") && searchInfo.DeptCode.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    IList<string> deptCodes = new List<string>();
                    string[] strDeptCode = searchInfo.DeptCode.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strDeptCode.Length > 0)
                    {
                        foreach (string item in strDeptCode)
                        {
                            deptCodes.Add(item);
                        }
                    }

                    messages = sqlHelper.SqlMapper.QueryForList<ReturnMessages>("FindReturnMessagesByDepCode", deptCodes);
                }
                else
                {
                    messages = sqlHelper.SqlMapper.QueryForList<ReturnMessages>("FindReturnMessages", searchInfo.DeptCode);
                }
            }
            else
                messages = sqlHelper.SqlMapper.QueryForList<ReturnMessages>("SelectReturnMessages", "");

            return messages;
        }

        /// <summary>
        /// 根据查询条件获取回退的条码信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public DataTable GetDataForReturnMessage(string sqlWhere)
        {
            DataTable dt = null;
            try
            {
                DbBase daoTemp = DbBase.InConn();

                string sql = @"select CAST (bc_return_messages.bc_id AS REAL) AS Id,bc_return_messages.bc_bar_no AS BarcodeNumber,bc_return_messages.bc_bar_code AS BarcodeCode,bc_message AS Message,bc_return_messages.bc_d_code AS DeptCode,bc_return_messages.bc_d_name AS DeptName,bc_sender_code AS SenderCode,bc_sender_name AS SenderName,bc_read_flag AS ReadFlag,bc_time AS BcTime
      ,CAST (bc_barcode_id AS REAL)  AS BarcodeId, 
(case when bc_handle_flag=1 then '是' else '否' end) AS HandleFlag 
, bc_return_messages.bc_del AS DelFlag ,bc_return_messages. bc_ori_id as OriID
, bc_patients.bc_name as PatName,bc_patients.bc_his_name as ComName,bc_return_messages.bc_receiver as ReceiverName
 from bc_return_messages
left join bc_patients on bc_patients.bc_bar_no=bc_return_messages.bc_bar_no
where bc_return_messages.bc_del=0
{0} ";

                DataSet dsResult = daoTemp.GetDataSet(string.Format(sql, sqlWhere));
                dsResult.Tables[0].TableName = "dtbc_return_messages";
                dt = dsResult.Tables[0];
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
            return dt;
        }

        public bool AuditFromHIS(AuditInfo userInfo)
        {
            string result = Outlink.VerifyStaff(Outlink.GenerateAuditInfo(userInfo));
            if (result.IndexOf("Error") >= 0)
                return false;
            return true;
        }
        public DataTable GetDataForReachInfo(string p_strStratDate, string p_strEndDate, string p_strStatus)
        {
            DataTable dtbResult = null;
            //List<Lis.Entities.EntityBcPatients> lstBcPatientsInfo = new List<Lis.Entities.EntityBcPatients>();
            try
            {

                //string strParLoginCode = "";
                //string strParCtype = "";



                ////操作人工号条件处理
                //if (!string.IsNullOrEmpty(p_strLoginCode))
                //{
                //    strParLoginCode = "and   ( bc_print_code = '" + p_strLoginCode + "' or  bc_reach_code = '" + p_strLoginCode + "' or bc_receiver_code ='" + p_strLoginCode + "' or bc_send_code = '" + p_strLoginCode + "' or bc_blood_code = '" + p_strLoginCode + "')";
                //}
                ////物理组条件处理
                //if (!string.IsNullOrEmpty(p_strCtype))
                //{
                //    strParCtype = "and bc_ctype in (" + p_strCtype + ") ";
                //}



                string strSQL = string.Format(@"select
bc_id,
	bc_bar_no,
	bc_bar_code,
	bc_frequency,
	bc_no_id,
	bc_in_no,
	bc_bed_no,
	bc_name,
	bc_sex,
	bc_age,
	bc_d_code,
	bc_d_name,
	bc_diag,
	bc_doct_code,
	bc_doct_name,
	bc_his_name,
	bc_sam_id,
	bc_sam_name=sam_name,
	bc_date,
	bc_times,
	bc_print_flag,
	bc_print_date,
	bc_print_code,
	bc_print_name,
	bc_cuv_code,
	bc_cuv_name=cuv_name,
	bc_cap_sum,
	bc_cap_unit,
	bc_urgent_flag,
	bc_ctype=type_name,
	bc_ori_id,
	bc_ori_name,
	bc_receiver_flag,
	bc_receiver_date,
	bc_receiver_code,
	bc_receiver_name,
	bc_blood_flag,
	bc_blood_date,
	bc_blood_code,
	bc_blood_name,
	bc_send_flag,
	bc_send_date,
	bc_send_code,
	bc_send_name,
	bc_reach_flag,
	bc_reach_date,
	bc_reach_code,
	bc_reach_name,
	bc_exp,
	bc_computer,
	bc_social_no,
	bc_emp_id,
	bc_info,
	bc_hospital_id,
	bc_bar_type,
	bc_status,
	bc_status_cname,
	bc_del,
	bc_birthday,
	bc_blood,
	bc_occ_date,
	bc_sam_rem_id,
	bc_sam_rem_name,
	bc_return_flag,
	bc_print_time,
	bc_lastaction_time,
	bc_return_times,
	bc_address,
	bc_tel,
	bc_emp_company,
	bc_pid,
bc_price = 
(
select sum(isnull(dict_combine.com_pri,0)) 
from bc_cname  
left join dict_combine on bc_cname.bc_lis_code = dict_combine.com_id  

where bc_cname.bc_bar_code = bc_patients.bc_bar_code)

from bc_patients with(nolock)  
left join dict_cuvette on dict_cuvette.cuv_code=bc_patients.bc_cuv_code  
left join dict_sample on dict_sample.sam_id=bc_sam_id  
left join dict_type on dict_type.type_id=bc_patients.bc_ctype
 where  bc_status=?
and  bc_del <> '1' 
and   bc_lastaction_time between ? and ?
 order by bc_status 
");

                SqlHelper helper = new SqlHelper();
                Lib.DAC.DbCommandEx cmd = helper.CreateCommandEx(strSQL);
                cmd.AddParameterValue(p_strStatus);
                cmd.AddParameterValue(p_strStratDate);
                cmd.AddParameterValue(p_strEndDate);

                dtbResult = helper.GetTable(cmd);
                dtbResult.TableName = "reachbarcode";
                //if (dtbResult.Rows.Count > 0)
                //{
                //    lstBcPatientsInfo = Lib.EntityCore.EntityConverter.DataTableToEntityList<Lis.Entities.EntityBcPatients>(dtbResult);
                //}
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
            return dtbResult;
        }
        /// <summary>
        /// 修改标本与标本备注
        /// </summary>
        /// <param name="bcSampleInfo"></param>
        /// <returns></returns>
        public bool ChangeSample(BCSampleInfo bcSampleInfo)
        {
            DBHelper helper = new DBHelper();
            int result = 0;
            if (bcSampleInfo.NeedSign)
            {
                string[] barcodeList = bcSampleInfo.BarcodeID.Split(',');

                foreach (string barcode in barcodeList)
                {
                    string strsql =
                        string.Format(
                            "SELECT ISNULL(bc_sam_id,'') bc_sam_id, ISNULL(bc_sam_name,'') bc_sam_name FROM bc_patients  WITH(NOLOCK) WHERE bc_bar_code='{0}'",
                            barcode);
                    DataTable bcpatiens = helper.GetTable(strsql);

                    string oldsamid = string.Empty;
                    string oldsamname = string.Empty;

                    if (bcpatiens.Rows.Count > 0)
                    {
                        oldsamid = bcpatiens.Rows[0]["bc_sam_id"].ToString();
                        oldsamname = bcpatiens.Rows[0]["bc_sam_name"].ToString();
                    }
                    string remark = string.Format("标本ID:{0}-->{1},标本名称：{2}-->{3}", oldsamid, bcSampleInfo.SampleID, oldsamname, bcSampleInfo.SampleName);
                    result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_sam_id = '{1}' ,bc_sam_name = '{2}', bc_sam_rem_id = '{3}', bc_sam_rem_name = '{4}' where bc_bar_no = '{0}' and bc_del <> '1' and bc_status not in ('20','40','60') ", barcode, bcSampleInfo.SampleID, bcSampleInfo.SampleName, bcSampleInfo.SampleRemarkID, bcSampleInfo.SampleRemarkName));

                    if (result > 0)
                    {
                        string sql =
                       string.Format(@"insert into bc_sign( bc_date, bc_login_id, bc_name, bc_status, bc_bar_no, bc_bar_code, bc_place, bc_flow, bc_remark, pat_id)
                                        values('{0}','{1}','{2}','170','{3}','{3}','',1,'{4}','')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bcSampleInfo.OPID,
                                     bcSampleInfo.OPName, barcode, remark);
                        helper.ExecuteNonQuery(sql);
                    }
                }
            }
            else
            {
                result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_sam_id = '{1}' ,bc_sam_name = '{2}', bc_sam_rem_id = '{3}', bc_sam_rem_name = '{4}' where bc_bar_no = '{0}' and bc_del <> '1'", bcSampleInfo.BarcodeID, bcSampleInfo.SampleID, bcSampleInfo.SampleName, bcSampleInfo.SampleRemarkID, bcSampleInfo.SampleRemarkName));

            }

            return result > 0;
        }

        /// <summary>
        /// 修改吸氧浓度
        /// </summary>
        /// <param name="barcodeID"></param>
        /// <param name="strFio2"></param>
        /// <returns></returns>
        public bool ChangeFio2(string barcodeID, string strFio2)
        {
            DBHelper helper = new DBHelper();
            int result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_fio2 = '{1}'  where bc_bar_no = '{0}' and bc_del <> '1'", barcodeID, strFio2));

            return result > 0;
        }

        /// <summary>
        /// 修改体温
        /// </summary>
        /// <param name="barcodeID"></param>
        /// <param name="strTempt"></param>
        /// <returns></returns>
        public bool ChangeTempt(string barcodeID, string strTempt)
        {
            DBHelper helper = new DBHelper();
            int result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_tempt = '{1}'  where bc_bar_no = '{0}' and bc_del <> '1'", barcodeID, strTempt));

            return result > 0;
        }

        /// <summary>
        /// 修改年龄信息
        /// </summary>
        /// <param name="objAgeInfo"></param>
        /// <returns></returns>
        public bool ChangeAge(BarcodeAgeInfo objAgeInfo)
        {
            DBHelper helper = new DBHelper();
            //门诊条码打印界面，同一病人打印条码年龄为空、性别为空时统一录入，不用逐个录入
            int result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_age='{1}' where bc_del <> '1' and bc_in_no='{0}'", objAgeInfo.PatientID, objAgeInfo.Age));


            return result > 0;
        }


        /// <summary>
        /// 修改性别信息
        /// </summary>
        /// <param name="objSexInfo"></param>
        /// <returns></returns>
        public bool ChangeSex(BarcodeSexInfo objSexInfo)
        {
            DBHelper helper = new DBHelper();

            int result = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_sex='{1}' where bc_del <> '1' and bc_in_no='{0}'", objSexInfo.PatientID, objSexInfo.Sex));


            return result > 0;
        }


        /// <summary>
        /// 是否包含条码信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public bool ContainCombineRecode(string orderID)
        {
            IList<BarcodeCombinesRecodes> recodes = sqlHelper.SqlMapper.QueryForList<BarcodeCombinesRecodes>("SelectBarcodeCombinesRecodesByOrderID", orderID);
            return recodes != null && recodes.Count > 0;
        }
        /// <summary>
        /// 从旧his条码获取医嘱信息sql
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        string GetOldLisYZIDInfoByBarcode(string barcode)
        {
            StringBuilder result = new StringBuilder();
            bool addsqm = System.Configuration.ConfigurationSettings.AppSettings["OldLisYZAddSingleQuotationMarks"] == "1";

            if (!string.IsNullOrEmpty(barcode))
            {
                string sql = string.Format(@"
select isnull(bc_cname.bc_yz_id,'') bc_yz_id from bc_patient left join bc_cname on bc_cname.bc_bar_no=bc_patient.bc_bar_no
where bc_patient.bc_bar_code='{0}' and (bc_del_flag='0' or bc_del_flag is null)", barcode);
                string oldlisConnectStr = System.Configuration.ConfigurationSettings.AppSettings["OldLisConnectionString"];

                SqlHelper helper = new SqlHelper(oldlisConnectStr, EnumDbDriver.MSSql);
                DataTable table = helper.GetTable(sql);
                foreach (DataRow item in table.Rows)
                {
                    if (!string.IsNullOrEmpty(item["bc_yz_id"].ToString()))
                    {
                        if (result.Length > 0)
                        {
                            result.Append(",");
                        }
                        if (addsqm)
                        {
                            result.Append(string.Format("'{0}'", item["bc_yz_id"].ToString()));
                        }
                        else
                        {
                            result.Append(string.Format("{0}", item["bc_yz_id"].ToString()));

                        }
                    }


                }
                table.Clear();
            }


            return result.ToString();
        }

        /// <summary>
        /// 合并条码主算法
        /// </summary>
        /// <param name="downloadInfo"></param>
        /// <param name="dsHISData"></param>
        /// <returns></returns>
        public DataSet DownloadBarcodeFromHIS2(BarcodeDownloadInfo downloadInfo, DataSet dsHISData)
        {
            try
            {
                StringBuilder debugMsg = new StringBuilder();
                //日志
                if (showLogger)
                {
                    watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                }

                string type = downloadInfo.GetDownloadTypeId();

                DBHelper helper = new DBHelper();

                //获取条码表与明细表结构
                DataTable dtPatientsStructure = helper.GetTable("select bc_bar_no,bc_bar_code,bc_frequency,bc_no_id,bc_in_no,bc_bed_no,bc_name,bc_sex,bc_age,bc_d_code,bc_d_name,bc_diag,bc_doct_code,bc_doct_name,bc_his_name,bc_sam_id,bc_sam_name,bc_date,bc_times,bc_cuv_code,bc_cuv_name,bc_cap_sum,bc_cap_unit,bc_urgent_flag,bc_ctype,bc_ori_id,bc_ori_name,bc_exp,bc_computer,bc_social_no,bc_emp_id,bc_info,bc_hospital_id,bc_bar_type,bc_status,bc_status_cname,bc_birthday,bc_blood,bc_blood_date,bc_blood_flag,bc_blood_name,bc_blood_code,bc_print_flag,bc_print_date,bc_print_code,bc_print_name,bc_send_flag,bc_send_date,bc_send_code,bc_send_name,bc_reach_flag,bc_reach_date,bc_reach_code,bc_reach_name,bc_receiver_flag,bc_receiver_date,bc_receiver_code,bc_receiver_name,bc_occ_date,bc_sam_rem_id,bc_sam_rem_name,bc_print_time,bc_return_times,bc_lastaction_time,bc_address,bc_tel,bc_pid,bc_emp_company,bc_app_no,bc_sam_dest,bc_fee_type,bc_emp_company_name,bc_emp_company_dept,bc_oldlis_barcode,bc_notice,'' as bc_gourp_id,'' as bc_group_id,'' as bc_upid,'' as bc_merge_comid,0 as bc_identity  from bc_patients where 1<>1");
                DataTable dtCNameStructure = helper.GetTable("select bc_bar_no, bc_bar_code, bc_frequency, bc_his_name, bc_his_code, bc_yz_id,bc_yz_id2, bc_ctype, bc_flag, bc_date, bc_apply_date, bc_occ_date, bc_item_no, bc_confirm_flag, bc_confirm_code, bc_confirm_name, bc_price, bc_unit, bc_modify_flag, bc_modify_date, bc_itr_id, bc_view_flag, bc_exec_code, bc_exec_name, bc_out_time, bc_rep_flag, bc_rep_date, bc_enrol_flag, bc_other_no, bc_lis_code,bc_name,bc_blood_notice,bc_save_notice from bc_cname where 1<>1");
                DataTable dtCombineSeq = helper.GetTable(string.Format(@"SELECT dict_combine_bar.com_his_fee_code,com_name,dict_combine.com_seq
                                                        FROM dict_combine_bar
                                                        LEFT JOIN dict_combine on dict_combine.com_id = dict_combine_bar.com_id
                                                        WHERE dict_combine_bar.com_ori_id='{0}' and
                                                        (dict_combine.com_del = '0' or dict_combine.com_del is null) 
                                                        and dict_combine.com_id is not null 
                                                        group by com_name,dict_combine.com_seq,dict_combine_bar.com_his_fee_code
                                                        order by dict_combine.com_seq", type));
                //改成从缓存获取字典数据
                //DataTable dtCombineSeq = DictCombineBarCache.Current.GetCombineBarSeq(type);
                //接口下载HIS医嘱

                //是否从旧检验根据条码获取信息，再下载医嘱
                string oldlisBarcode = downloadInfo.PatientID;
                bool downloadbyoldlis = downloadInfo.Conn_LisSearchColumn == "bc_oldlis_barcode";
                if (downloadbyoldlis)
                {
                    downloadInfo.PatientID = GetOldLisYZIDInfoByBarcode(downloadInfo.PatientID);
                }

                IBCConnect connecter = BCConnectFactory.Create(downloadInfo);
                if (Extensions.IsEmpty(dsHISData)) //客户端没传数据则由服务器去取
                {
                    try
                    {
                        string strInterMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");
                        string strMzInterMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_MZInterfaceMode");

                        if (downloadInfo.DownloadType == PrintType.Inpatient)//住院
                        {
                            if (strInterMode == "中医大附属第一医院")
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                            else if (strInterMode == "惠侨CDR")
                            {
                                DataSet dsResult = new DataSet();
                                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();

                                DataSet ds = new DataSet();

                                if (downloadInfo.PatientName == "BCBarcode")
                                    ds = cdr.GetZyBCApplyInfoToTable(string.Empty, downloadInfo.StartTime, downloadInfo.EndTime, downloadInfo.DeptID);
                                else
                                    ds = cdr.GetZyApplyInfoToTable(string.Empty, downloadInfo.StartTime, downloadInfo.EndTime, downloadInfo.DeptID);

                                DataTable dtState = ds.Tables["state"];
                                DataTable dtResult = ds.Tables["result"];
                                if (dtState != null && dtState.Rows.Count > 0
                                    && dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    if (dtState.Columns.Contains("result") && dtState.Rows[0]["result"].ToString() == "0")
                                    {
                                        dsResult.Tables.Add(dtResult.Copy());
                                    }
                                }
                                dsHISData = dsResult;
                            }
                            else if (downloadInfo.DownloadType == PrintType.Inpatient &&
                               ConfigurationManager.AppSettings["EnableNewZYHisForLH"] == "Y")
                            {
                                SZRYService ser = new SZRYService();
                                dsHISData = ser.GetZyApplyDataByDept(downloadInfo.DeptID, downloadInfo.StartTime, downloadInfo.EndTime);

                                Logger.LogInfo("龙华分院新住院HIS条码接口", string.Format("调试信息：{0}", ((dsHISData == null || dsHISData.Tables.Count == 0 || dsHISData.Tables[0].Rows.Count == 0) ? "无医嘱数据" : dsHISData.Tables[0].Rows.Count.ToString())));
                            }
                            else
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                        }
                        else if (downloadInfo.DownloadType == PrintType.Outpatient)//门诊
                        {
                            if (strInterMode == "中医大附属第一医院" || strMzInterMode == "惠侨His(Webservice)")
                            {
                                #region 惠侨His(Webservice)
                                HISRequest_MZ_LisBarcode req = new HISRequest_MZ_LisBarcode();
                                req.BeginDate = downloadInfo.StartTime;
                                req.EndDate = downloadInfo.EndTime;
                                req.P_id = downloadInfo.PatientID;
                                //his提供诊疗卡号字段供我们写入调用
                                if (downloadInfo.Conn_LisSearchColumn == "bc_social_no")
                                {
                                    req.Flag = "1";
                                }
                                try
                                {
                                    string reponse = HqServiceHelper.GetProxy().Call(req);

                                    DataTable table = HISInterface.XmlToTable(reponse);

                                    dsHISData = new DataSet();
                                    dsHISData.Tables.Add(table);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogException("中医大门诊条码接口", ex);
                                    throw;
                                }
                                #endregion
                            }
                            else if (strInterMode == "惠侨CDR")
                            {
                                DataSet dsResult = new DataSet();
                                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();

                                string strCarNo = string.Empty;
                                string strInNo = string.Empty;
                                string strInvoNo = string.Empty;

                                if (downloadInfo.Conn_LisSearchColumn == "bc_social_no")
                                    strCarNo = downloadInfo.PatientID;
                                else if (downloadInfo.Conn_LisSearchColumn == "bc_exp")
                                    strInvoNo = downloadInfo.PatientID;
                                else
                                    strInNo = downloadInfo.PatientID;


                                DataSet ds = cdr.GetMzApplyInfoToTable(strCarNo, downloadInfo.StartTime, downloadInfo.EndTime, strInNo, strInvoNo);
                                DataTable dtState = ds.Tables["state"];
                                DataTable dtResult = ds.Tables["result"];
                                if (dtState != null && dtState.Rows.Count > 0
                                    && dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    if (dtState.Columns.Contains("result") && dtState.Rows[0]["result"].ToString() == "0")
                                    {
                                        dsResult.Tables.Add(dtResult.Copy());
                                    }
                                }
                                dsHISData = dsResult;
                            }
                            else if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_LHMZ") == "Webservice")
                            {
                                SZRYService ser = new SZRYService();

                                string strCarNo = string.Empty;
                                string strInNo = string.Empty;

                                if (downloadInfo.Conn_LisSearchColumn == "bc_pid")
                                    strCarNo = downloadInfo.PatientID;
                                else
                                    strInNo = downloadInfo.PatientID;

                                dsHISData = ser.GetPatientInfoToTable(strCarNo, strInNo, downloadInfo.StartTime, downloadInfo.EndTime);
                            }
                            else
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                        }
                        else
                        {
                            if (downloadInfo.DownloadType == PrintType.TJ//不敢用strInterMode，避免清远可能没开放体检下载
                                && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("TJBarcode_DownloadModel") == "惠侨CDR")
                            {
                                DataSet dsResult = new DataSet();
                                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();


                                DataSet ds = cdr.GetTJApplyInfoToTable(downloadInfo.PatientID, downloadInfo.StartTime, downloadInfo.EndTime, downloadInfo.DeptID);
                                DataTable dtResult = ds.Tables["result"];
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    if (dtResult.Columns.Contains("result") && dtResult.Rows[0]["result"].ToString() == "0")
                                        dsResult.Tables.Add(dtResult.Copy());
                                }
                                dsHISData = dsResult;
                            }
                            else
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                        }

                        #region 旧接口
                        /*
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "中医大附属第一医院")
                        {
                            //惠侨his(门诊与住院)接口
                            //中医大使用Webservice接口
                            if (downloadInfo.DownloadType == PrintType.Inpatient)//住院
                            {
                                dsHISData = connecter.DownloadHisOrder();
                                //HISRequest_ZY_LisBarcode req = new HISRequest_ZY_LisBarcode();
                                //req.dept_sn = downloadInfo.DeptID;
                                //req.ward_sn = downloadInfo.DeptID;
                                //req.begindt = downloadInfo.StartTime;
                                //req.enddt = downloadInfo.EndTime;
                                //try
                                //{
                                //    string reponse = HqServiceHelper.GetProxy().Call(req);

                                //    Logger.LogInfo("中医大住院条码接口", string.Format("请求字符\r\n{0}\r\n\r\n返回字符\r\n{1}", req.Build(), reponse));

                                //    DataTable table = HISInterface.XmlToTable(reponse);

                                //    dsHISData = new DataSet();
                                //    dsHISData.Tables.Add(table);
                                //}
                                //catch (Exception ex)
                                //{
                                //    Logger.LogException("中医大住院条码接口", ex);
                                //    throw;
                                //}
                            }
                            else if (downloadInfo.DownloadType == PrintType.Outpatient)//门诊
                            {
                                HISRequest_MZ_LisBarcode req = new HISRequest_MZ_LisBarcode();
                                req.BeginDate = downloadInfo.StartTime;
                                req.EndDate = downloadInfo.EndTime;
                                req.P_id = downloadInfo.PatientID;
                                //his提供诊疗卡号字段供我们写入调用
                                if (downloadInfo.Conn_LisSearchColumn == "bc_social_no")
                                {
                                    req.Flag = "1";
                                }
                                try
                                {
                                    string reponse = HqServiceHelper.GetProxy().Call(req);

                                    Logger.LogInfo("中医大门诊条码接口", string.Format("请求字符\r\n{0}\r\n\r\n返回字符\r\n{1}", req.Build(), reponse));

                                    DataTable table = HISInterface.XmlToTable(reponse);

                                    dsHISData = new DataSet();
                                    dsHISData.Tables.Add(table);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogException("中医大门诊条码接口", ex);
                                    throw;
                                }

                                //dsHISData = connecter.DownloadHisOrder();
                            }
                            else
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                        }
                        else if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "惠侨CDR")
                        {
                            DataSet dsResult = new DataSet();
                            if (downloadInfo.DownloadType == PrintType.Inpatient)//住院
                            {
                                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();

                                DataSet ds = new DataSet();

                                if (downloadInfo.PatientName == "BCBarcode")
                                    ds = cdr.GetZyBCApplyInfoToTable(string.Empty, downloadInfo.StartTime, downloadInfo.EndTime, downloadInfo.DeptID);
                                else
                                    ds = cdr.GetZyApplyInfoToTable(string.Empty, downloadInfo.StartTime, downloadInfo.EndTime, downloadInfo.DeptID);

                                DataTable dtResult = ds.Tables["result"];
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    if (dtResult.Columns.Contains("result") && dtResult.Rows[0]["result"].ToString() == "0")
                                        dsResult.Tables.Add(dtResult.Copy());
                                }
                            }
                            else if (downloadInfo.DownloadType == PrintType.Outpatient)//门诊
                            {
                                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();

                                string strCarNo = string.Empty;
                                string strInNo = string.Empty;
                                string strInvoNo = string.Empty;

                                if (downloadInfo.Conn_LisSearchColumn == "bc_social_no")
                                    strCarNo = downloadInfo.PatientID;
                                else if (downloadInfo.Conn_LisSearchColumn == "bc_exp")
                                    strInvoNo = downloadInfo.PatientID;
                                else
                                    strInNo = downloadInfo.PatientID;


                                DataSet ds = cdr.GetMzApplyInfoToTable(strCarNo, downloadInfo.StartTime, downloadInfo.EndTime, strInNo, strInvoNo);
                                DataTable dtResult = ds.Tables["result"];
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    if (dtResult.Columns.Contains("result") && dtResult.Rows[0]["result"].ToString() == "0")
                                        dsResult.Tables.Add(dtResult.Copy());
                                }
                            }
                            else
                                dsResult = connecter.DownloadHisOrder();
                            dsHISData = dsResult;
                        }
                        else
                        {
                            if (downloadInfo.DownloadType == PrintType.Outpatient &&
                                dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_LHMZ") == "Webservice")
                            {
                                SZRYService ser = new SZRYService();

                                string strCarNo = string.Empty;
                                string strInNo = string.Empty;

                                if (downloadInfo.Conn_LisSearchColumn == "bc_pid")
                                    strCarNo = downloadInfo.PatientID;
                                else
                                    strInNo = downloadInfo.PatientID;

                                dsHISData = ser.GetPatientInfoToTable(strCarNo, strInNo, downloadInfo.StartTime, downloadInfo.EndTime);
                            }
                            else if (downloadInfo.DownloadType == PrintType.Inpatient &&
                               ConfigurationManager.AppSettings["EnableNewZYHisForLH"] == "Y")
                            {
                                SZRYService ser = new SZRYService();
                                dsHISData = ser.GetZyApplyDataByDept(downloadInfo.DeptID, downloadInfo.StartTime, downloadInfo.EndTime);

                                Logger.LogInfo("龙华分院新住院HIS条码接口", string.Format("调试信息：{0}", ((dsHISData == null || dsHISData.Tables.Count == 0 || dsHISData.Tables[0].Rows.Count ==0)? "无医嘱数据" : dsHISData.Tables[0].Rows.Count.ToString())));
                            }
                            else
                            {
                                dsHISData = connecter.DownloadHisOrder();
                            }
                        }
                         */
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("通过院网接口获取数据", ex);
                        throw;
                    }
                }
                //Logger.LogInfo("通过院网接口获取数据", string.Format("通过院网接口获取数据:{0}", dsHISData.Tables.Count > 0 ? dsHISData.Tables[0].Rows.Count : 0));
                if (showLogger)
                {
                    Logger.LogInfo("通过院网接口获取数据", watch.ElapsedMilliseconds.ToString() + "ms");
                }

                #region 允许下载HIS码未匹配的条码

                //系统配置：允许下载HIS码未匹配的条码
                string strBarcodeAllowDownHisNotMatch = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_AllowDown_HisNotMatch");
                //默认为“是”
                if (string.IsNullOrEmpty(strBarcodeAllowDownHisNotMatch)) { strBarcodeAllowDownHisNotMatch = "是"; }
                //如果里面带‘门诊’‘住院’‘体检’,则指定某一来源允许。备注：‘是’代表全部允许,‘否’代表全部不允许。
                if (type != null && type == "107" && strBarcodeAllowDownHisNotMatch != "否" && strBarcodeAllowDownHisNotMatch != "是")
                {
                    if (!strBarcodeAllowDownHisNotMatch.Contains("门诊"))
                    {
                        strBarcodeAllowDownHisNotMatch = "否";
                    }
                }
                else if (type != null && type == "108" && strBarcodeAllowDownHisNotMatch != "否" && strBarcodeAllowDownHisNotMatch != "是")
                {
                    if (!strBarcodeAllowDownHisNotMatch.Contains("住院"))
                    {
                        strBarcodeAllowDownHisNotMatch = "否";
                    }
                }
                else if (type != null && type == "109" && strBarcodeAllowDownHisNotMatch != "否" && strBarcodeAllowDownHisNotMatch != "是")
                {
                    if (!strBarcodeAllowDownHisNotMatch.Contains("体检"))
                    {
                        strBarcodeAllowDownHisNotMatch = "否";
                    }
                }

                if (strBarcodeAllowDownHisNotMatch == "否")
                {
                    //过滤未匹配HIS码的信息，并且生成错误日志
                    filtrateBCPatientsHisNotMatch(dsHISData, dtCombineSeq, downloadInfo);
                }
                #endregion

                //系统配置：允许下载医嘱ID相同的条码
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_AllowDown_SameYZID") == "否")
                {
                    //过滤相同的医嘱ID,并且生成错误日志
                    //filtrateBCPatientsSameYZ(dsHISData, downloadInfo);
                }

                if (dsHISData == null || Extensions.IsEmpty(dsHISData))//没有就返回
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return null;
                }

                //setBCPatientsSeq(dsHISData, dtCombineSeq, downloadInfo);//根据组合排序排列医嘱

                //  Logger
                //按对照表转换成LIS表的数据
                // DataSet dsLisData = ConvertHelper.ConvertToLis(dsHISData, dtPatientsStructure, downloadInfo.FetchDataType == FetchDataType.OutLink);
                DataSet dsLisData = ConvertHelper.ConvertToLis(dsHISData, dtPatientsStructure, downloadInfo);

                //中山需求，根据科室下载，不管接口数据是什么，都更新为下载科室。
                //dsLisData = SetDepInfo(downloadInfo, dsLisData);

                if (showLogger)
                {
                    Logger.LogInfo("条码下载:对照表转换", watch.ElapsedMilliseconds.ToString() + "ms");
                }
                debugMsg.AppendLine(string.Format("按对照表转换成LIS表的数据:{0}", dsLisData.Tables.Count > 0 ? dsLisData.Tables[0].Rows.Count : 0));

                //转换后没有数据
                if (Extensions.IsEmpty(dsHISData) || Extensions.IsEmpty(dsLisData))
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return dsHISData;
                }

                //当根据部门进行下载条码时进行并发控制
                //KeyResult keyReult = BarConcurrencyController.InsertKey(downloadInfo);
                //if (!keyReult.Ok)
                //{
                //    string temp_strEx = "不通过,已有此key在下载条码" + string.Format("Key:{0}", keyReult.Key);
                //    Logger.LogException("下载条码并发控制", new Exception(temp_strEx));
                //    return dsHISData;
                //}

                //string OrderTimeColumn =ConvertHelper.  downloadInfo.FetchDataType == FetchDataType.OutLink ? "chktm" : "医嘱时间";
                //string HISCombineIDColumn = downloadInfo.FetchDataType == FetchDataType.OutLink ? "FEEID" : "项目编码";
                //string HISCombineNameColumn = downloadInfo.FetchDataType == FetchDataType.OutLink ? "FEENM" : "项目名称";


                // 2014年2月21日16:43:54 ye
                //string OrderTimeColumn = ConvertHelper.GetHISColumn("bc_occ_date");
                //string HISCombineIDColumn = ConvertHelper.GetHISColumn("bc_his_code");
                //string HISCombineNameColumn = ConvertHelper.GetHISColumn("bc_his_name");
                dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);
                string OrderTimeColumn = columnConvertHelper.GetHISColumn("bc_occ_date");
                string HISCombineIDColumn = columnConvertHelper.GetHISColumn("bc_his_code");
                string HISCombineNameColumn = columnConvertHelper.GetHISColumn("bc_his_name");

                DataTable dtSource = dsHISData.Tables[0];//获取的数据
                DataTable dtBCPatients = dsLisData.Tables[0]; //转换后Lis表
                DataTable dtBCPatientsForInsert = dtBCPatients.Clone();//要插入的病人资料
                //if (downloadInfo.DownloadType == PrintType.TJ)
                //{
                //    Logger.LogInfo("获取的数据（行数）", dtSource.Rows.Count.ToString());
                //    Logger.LogInfo("转换后Lis表（行数）", dtBCPatients.Rows.Count.ToString());
                //}

                //从旧检验根据条码获取信息时填充旧条码到bc_oldlis_barcode
                if (downloadbyoldlis && dtBCPatients != null)
                {
                    foreach (DataRow item in dtBCPatients.Rows)
                    {
                        item["bc_oldlis_barcode"] = oldlisBarcode;
                    }
                }
                //dsLisData.Tables[0].Columns.Add(colTJPace);


                //判断如果是外部条码打印不写入本数据库，如体检检查条码打印
                if (downloadInfo.DownloadType == PrintType.TJPacs)
                {

                    //如果是外部条码打印，需要添加一列来识别
                    DataColumn colTJPace = new DataColumn(BarcodeTable.Patient.Bc_printtype);
                    colTJPace.DefaultValue = "";
                    dsLisData.Tables[0].Columns.Add(colTJPace);
                    foreach (DataRow dr in dsLisData.Tables[0].Rows)
                    {
                        dr[BarcodeTable.Patient.Bc_printtype] = "TJPacs";
                        dr[BarcodeTable.Patient.StatusCode] = EnumBarcodeOperationCode.BarcodeGenerate;
                        dr[BarcodeTable.Patient.PrintFlag] = 0;

                    }

                }
                else
                {
                    #region 插入条码表操作

                    //插入条码表
                    if (Extensions.IsNotEmpty(dsLisData))
                    {
                        //拆分条码集合
                        SplitCodeCollection splitCodes = new SplitCodeCollection(downloadInfo.DownloadType);

                        DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                        IList<string> hisCombineIDs = new List<string>();

                        IList<string> orderIDs = new List<string>();

                        //HIS组合ID与医嘱ID
                        foreach (DataRow sourceRow in dtSource.Rows)
                        {
                            if (!hisCombineIDs.Contains(sourceRow[HISCombineIDColumn].ToString()))
                                hisCombineIDs.Add(sourceRow[HISCombineIDColumn].ToString());

                            orderIDs.Add(connecter.GenerateOrderID(sourceRow, downloadInfo));
                        }


                        //条码拆分信息
                        List<BarcodeCombines> bcCombinesCache = new List<BarcodeCombines>(FindBarcodeCombineByHISFeeCode(hisCombineIDs));
                        IList<string> comIDs = new List<string>();
                        if (bcCombinesCache != null)
                        {
                            foreach (BarcodeCombines bc in bcCombinesCache)
                            {
                                comIDs.Add(bc.CombineId);
                            }
                        }
                        //LIS组合
                        IList<Combines> combinesCache = FindLisCombinesByIDs(comIDs);
                        bool ShouldOrderIdBindingOriId = SystemConfiguration.GetSystemConfig("OrderIDBindingOriID") == "是";
                        //由于sql参数最大支持2100个：每次获取的医嘱id设定为2000
                        int eachCount = 2000;
                        List<BarcodeCombinesRecodes> bcCombineRecodeCache;
                        if (orderIDs.Count > eachCount)
                        {
                            //条码明细表
                            bcCombineRecodeCache = new List<BarcodeCombinesRecodes>();
                            // FindBCCombineRecode(orderIDs);

                            int orderIdCount = 0;
                            List<string> tempOrderId = new List<string>();
                            for (int i = 0; i < orderIDs.Count; i++)
                            {
                                tempOrderId.Add(orderIDs[i]);

                                if (orderIdCount >= eachCount - 1 || i == orderIDs.Count - 1)
                                {
                                    IList<BarcodeCombinesRecodes> temp_bcCombineRecodeCache = null;
                                    if (ShouldOrderIdBindingOriId)
                                        temp_bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(tempOrderId);
                                    else
                                        temp_bcCombineRecodeCache = FindBCCombineRecode(tempOrderId);
                                    if (temp_bcCombineRecodeCache != null && temp_bcCombineRecodeCache.Count > 0)
                                    {
                                        bcCombineRecodeCache.AddRange(temp_bcCombineRecodeCache);
                                    }
                                    tempOrderId.Clear();
                                    orderIdCount = 0;
                                }
                                else
                                {
                                    orderIdCount++;

                                }
                            }
                        }
                        else
                        {
                            if (ShouldOrderIdBindingOriId)
                                bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(orderIDs).ToList();
                            else
                                bcCombineRecodeCache = FindBCCombineRecode(orderIDs).ToList();
                        }


                        //条码明细表里的医嘱ID
                        IList<string> bcCombineRecodeOrderCache = ToList(bcCombineRecodeCache, downloadInfo, ShouldOrderIdBindingOriId);

                        if (showLogger)
                            Logger.LogInfo("条码下载:下载缓存表", watch.ElapsedMilliseconds.ToString() + "ms");
                        string oriid = GetOriID(downloadInfo.DownloadType);

                        for (int i = 0; i < dtBCPatients.Rows.Count; i++)
                        {
                            string hisCombineID = dtSource.Rows[i][HISCombineIDColumn].ToString(); //HIS组合ID
                            string hisCombineName = dtSource.Rows[i][HISCombineNameColumn].ToString(); //HIS组合名称

                            string orderID = connecter.GenerateOrderID(dtSource.Rows[i], downloadInfo);//医嘱ID
                            string orderID2 = string.Empty;

                            // 2014年2月21日16:44:51 ye
                            //if (dtSource.Columns.Contains(ConvertHelper.GetHISColumn("bc_yz_id2")) == true)
                            //{
                            //    orderID2 = dtSource.Rows[i][ConvertHelper.GetHISColumn("bc_yz_id2")].ToString();
                            //}
                            if (dtSource.Columns.Contains(columnConvertHelper.GetHISColumn("bc_yz_id2")) == true)
                            {
                                orderID2 = dtSource.Rows[i][columnConvertHelper.GetHISColumn("bc_yz_id2")].ToString();
                            }


                            //return dataRow[ConvertHelper.GetHISColumn("bc_order_id")].ToString();

                            if (HasOrderIDInCombinesRecord(bcCombineRecodeOrderCache, FormatOrderId(orderID, downloadInfo, ShouldOrderIdBindingOriId))) //如果明细表有此医嘱ID
                                continue;

                            string newBarcode = "";
                            bool hasPatients = false;
                            IList<BarcodeCombines> barcodeCombines = null;
                            IList<BarcodeCombines> bcCombinesCache2 = new List<BarcodeCombines>(bcCombinesCache);

                            #region old


                            ////在条码拆分信息缓存里找HIS代码的记录
                            //barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache, hisCombineID, ref combinesCache, ref bcCombinesCache);
                            ////在找到的记录里找出对应来源的记录，如住院或门诊或默认
                            //barcodeCombines = FindWantBarcodeCombines(barcodeCombines, downloadInfo);
                            #endregion

                            //解决大相同his码不同来源的大组合的拆分问题

                            //在找到的记录里找出对应来源的记录，如住院或门诊或默认
                            // bcCombinesCache = FindWantBarcodeCombines(bcCombinesCache, downloadInfo);

                            bcCombinesCache2 = bcCombinesCache.FindAll(baritem => (string.IsNullOrEmpty(baritem.Oriid) || baritem.Oriid == oriid));


                            //在条码拆分信息缓存里找HIS代码的记录
                            //  barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache, hisCombineID, ref combinesCache, ref bcCombinesCache);
                            barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache2, hisCombineID, ref combinesCache, ref bcCombinesCache2);

                            barcodeCombines = FindWantBarcodeCombines(barcodeCombines, downloadInfo);



                            //string logMsg = string.Empty;
                            //foreach (BarcodeCombines item_temp2 in barcodeCombines)
                            //{
                            //    logMsg += "," + item_temp2.CombineId;
                            //}
                            //Logger.WriteException("条码拆分日志", "", "barcodeCombines=" + logMsg);


                            int count = MoreThanOne(dtSource.Rows[i], columnConvertHelper);
                            for (int index = 0; index < count; index++)
                            {
                                //生成条码
                                if (barcodeCombines == null || barcodeCombines.Count == 0)
                                    GenereateMainBarcode(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn, dtSource, dtBCPatients, dtBCPatientsForInsert, splitCodes, ref now, combinesCache, i, hisCombineID, hisCombineName, orderID, orderID2, ref newBarcode, ref hasPatients, null, ShouldOrderIdBindingOriId);
                                else
                                {
                                    //Logger.WriteException("插入条码", "", barcodeCombines.Count.ToString());
                                    foreach (BarcodeCombines barcodeCombine in barcodeCombines)
                                    {
                                        GenereateMainBarcode(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn, dtSource, dtBCPatients, dtBCPatientsForInsert, splitCodes, ref now, combinesCache, i, hisCombineID, hisCombineName, orderID, orderID2, ref newBarcode, ref hasPatients, barcodeCombine, ShouldOrderIdBindingOriId);
                                    }
                                }
                            }
                        }
                        if (showLogger)
                            Logger.LogInfo("条码下载:拆分合并完成", watch.ElapsedMilliseconds.ToString() + "ms");


                        if (Insert(downloadInfo, dtCNameStructure, dtBCPatientsForInsert))
                        {
                            //if (dsHISData != null && DateTime.Now <= DateTime.Parse("2012-02-20"))
                            //{
                            //    try
                            //    {
                            //        if (!Directory.Exists("d:\\LisDataLog"))
                            //            Directory.CreateDirectory("d:\\LisDataLog");

                            //        long size = 0;
                            //        foreach (string fileName in Directory.GetFiles("d:\\LisDataLog"))
                            //        {
                            //            FileInfo fi = new FileInfo(fileName);
                            //            size += fi.Length;

                            //            if (size / 1048576 > 200)
                            //            {
                            //                fi.Delete();
                            //            }
                            //        }

                            //        dsHISData.WriteXml(string.Format("d:\\LisDataLog\\{0}.xml", DateTime.Now.ToString("yyyyMMdd_" + Guid.NewGuid().ToString())));
                            //    }
                            //    catch (Exception)
                            //    {
                            //    }
                            //}
                        }

                        #region 医嘱确认

                        try
                        {
                            if (dtCNameStructure.Rows.Count > 0)
                            {
                                //深圳滨海模式
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "深圳滨海医院"
                                    || (downloadInfo.DownloadType == PrintType.Outpatient && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload") == "HL7")
                                    || (downloadInfo.DownloadType == PrintType.Inpatient && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload") == "HL7")
                                    || (downloadInfo.DownloadType == PrintType.TJ && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirmWithDowload") == "HL7"))
                                {
                                    string config_value = string.Empty;

                                    if (downloadInfo.DownloadType == PrintType.Outpatient)
                                    {
                                        config_value = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload");
                                    }
                                    else if (downloadInfo.DownloadType == PrintType.Inpatient)
                                    {
                                        config_value = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload");
                                    }
                                    else if (downloadInfo.DownloadType == PrintType.TJ)
                                    {
                                        config_value = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirmWithDowload");
                                    }

                                    if (config_value == "HL7")//在医嘱下载的时候确认
                                    {
                                        foreach (DataRow row in dtBCPatientsForInsert.Rows)//遍历每一条条码
                                        {
                                            string bc_bar_code = row["bc_bar_code"].ToString();

                                            string bc_in_no = row["bc_in_no"].ToString();
                                            string user_id = string.Empty;
                                            string user_name = string.Empty;

                                            DataRow[] row_advice = dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                                            if (row_advice.Length > 0)
                                            {
                                                foreach (DataRow row_a in row_advice)
                                                {
                                                    string advice_id = row_a["bc_yz_id"].ToString();

                                                    if (downloadInfo.DownloadType == PrintType.Outpatient)
                                                    {
                                                        new AdviceConfirmBIZ().AdviceConfirm_MZ(bc_in_no, advice_id, user_id, user_name, "MZ_AdviceConfirm");
                                                    }
                                                    else if (downloadInfo.DownloadType == PrintType.Inpatient)
                                                    {
                                                        new AdviceConfirmBIZ().AdviceConfirm_ZY(bc_in_no, advice_id, user_id, user_name, "ZY_AdviceConfirm");
                                                    }
                                                    else if (downloadInfo.DownloadType == PrintType.TJ)
                                                    {
                                                        new AdviceConfirmBIZ().AdviceConfirm_TJ(bc_in_no, advice_id, user_id, user_name, "TJ_AdviceConfirm");
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                                else if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "惠侨CDR")
                                {
                                    foreach (DataRow row in dtBCPatientsForInsert.Rows)//遍历每一条条码
                                    {
                                        string bc_bar_code = row["bc_bar_code"].ToString();

                                        string bc_in_no = row["bc_in_no"].ToString();
                                        string user_id = string.Empty;
                                        string user_name = string.Empty;

                                        DataRow[] row_advice = dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                                        if (row_advice.Length > 0)
                                        {
                                            foreach (DataRow row_a in row_advice)
                                            {
                                                string advice_id = row_a["bc_yz_id"].ToString();

                                                if (downloadInfo.DownloadType == PrintType.Outpatient)
                                                {
                                                    //new AdviceConfirmBIZ().AdviceConfirm_MZ(bc_in_no, advice_id, user_id, user_name, "MZ_AdviceConfirm");
                                                    new AdviceConfirmBIZ().AdviceConfirm_Invoke(bc_in_no, advice_id, user_id, user_name, "MZ_AdviceConfirm");
                                                }
                                                else if (downloadInfo.DownloadType == PrintType.Inpatient)
                                                {
                                                    new AdviceConfirmBIZ().AdviceConfirm_Invoke(bc_in_no, advice_id, user_id, user_name, "ZY_AdviceConfirm");
                                                }
                                                //else if (downloadInfo.DownloadType == PrintType.TJ)
                                                //{
                                                //    new AdviceConfirmBIZ().AdviceConfirm_TJ(bc_in_no, advice_id, user_id, user_name, "TJ_AdviceConfirm");
                                                //}
                                            }
                                        }
                                    }
                                }
                                else if ((downloadInfo.DownloadType == PrintType.Inpatient || downloadInfo.DownloadType == PrintType.Outpatient)
                                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "惠州六院")
                                {
                                    foreach (DataRow row in dtBCPatientsForInsert.Rows)//遍历每一条条码
                                    {
                                        string bc_bar_code = row["bc_bar_code"].ToString();

                                        StringBuilder sb = new StringBuilder();

                                        DataRow[] row_advice = dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                                        if (row_advice.Length > 0)
                                        {
                                            foreach (DataRow row_a in row_advice)
                                            {
                                                string advice_id = row_a["bc_yz_id"].ToString();

                                                if (!string.IsNullOrEmpty(advice_id))
                                                {
                                                    if (sb.Length == 0)
                                                    {
                                                        sb.Append(advice_id);
                                                    }
                                                    else
                                                    {
                                                        sb.Append("^" + advice_id);
                                                    }

                                                }
                                            }
                                            try
                                            {
                                                Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();
                                                ser.LabEpisodeNo(bc_bar_code, sb.ToString(), "lis");
                                            }
                                            catch (Exception ex)
                                            {
                                                Lib.LogManager.Logger.LogException("返回病人检验条码号:" + bc_bar_code, ex);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    #region 湖南儿童医院
                                    string opcode = string.Empty;
                                    string opname = string.Empty;
                                    if (CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") ==
                                        "湖南儿童医院")
                                    {
                                        Dictionary<string, string> dic = new Dictionary<string, string>();
                                        List<string> barcodeList = new List<string>();
                                        foreach (DataRow row in dtBCPatientsForInsert.Rows)
                                        {
                                            string bc_in_no = row["bc_in_no"].ToString();
                                            if (bc_in_no != string.Empty)
                                            {
                                                if (barcodeList.Contains(row["bc_bar_code"].ToString()))
                                                {
                                                    continue;
                                                }
                                                barcodeList.Add(row["bc_bar_code"].ToString());
                                                if (!dic.ContainsKey(bc_in_no))
                                                {
                                                    dic.Add(bc_in_no, row["bc_bar_code"].ToString());
                                                }
                                                else
                                                {
                                                    dic[bc_in_no] += ";" + row["bc_bar_code"].ToString();
                                                }
                                            }
                                        }

                                        foreach (string bcinno in dic.Keys)
                                        {
                                            string bc_in_no = bcinno;
                                            string bc_bar_code = dic[bcinno];

                                            List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                                            list.Add(new InterfaceDataBindingItem("bc_in_no", bc_in_no));
                                            list.Add(new InterfaceDataBindingItem("bc_bar_code", bc_bar_code));

                                            DataInterfaceHelper dihelper =
                                                new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                                            if (downloadInfo.DownloadType == PrintType.Outpatient)
                                            {
                                                dihelper.ExecuteNonQueryWithGroup("条码_门诊_下载后2", list.ToArray());
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(downloadInfo.OperationName))
                                        {
                                            DataTable doc = DictDoctorCache.Current.GetDoctors();
                                            if (doc != null && doc.Rows.Count > 0)
                                            {
                                                DataRow[] rows =
                                                    doc.Select(string.Format("doc_name='{0}' ", downloadInfo.OperationName));
                                                if (rows.Length > 0)
                                                {
                                                    opcode = rows[0]["doc_code"] != null
                                                                  ? rows[0]["doc_code"].ToString()
                                                                  : opcode;

                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    foreach (DataRow row in dtBCPatientsForInsert.Rows) //遍历每一条条码
                                    {
                                        string bc_bar_code = row["bc_bar_code"].ToString();
                                        string bc_in_no = row["bc_in_no"].ToString();
                                        string bc_name = row["bc_name"].ToString();
                                        string bc_pid = row["bc_pid"].ToString();
                                        string bc_times = row["bc_times"].ToString();

                                        if (string.IsNullOrEmpty(bc_times))
                                            bc_times = "0";

                                        string user_id = string.Empty;
                                        string user_name = string.Empty;

                                        DataRow[] row_advice =
                                            dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                                        if (row_advice.Length > 0)
                                        {
                                            foreach (DataRow row_a in row_advice)
                                            {
                                                string advice_id = row_a["bc_yz_id"].ToString();
                                                string bc_his_code = row_a["bc_his_code"].ToString();
                                                string bc_his_name = row["bc_his_name"].ToString();
                                                string bc_social_no = row["bc_social_no"].ToString();

                                                List<InterfaceDataBindingItem> list =
                                                    new List<InterfaceDataBindingItem>();
                                                list.Add(new InterfaceDataBindingItem("bc_in_no", bc_in_no));
                                                list.Add(new InterfaceDataBindingItem("bc_yz_id", advice_id));
                                                list.Add(new InterfaceDataBindingItem("bc_bar_code", bc_bar_code));
                                                list.Add(new InterfaceDataBindingItem("bc_his_code", bc_his_code));
                                                list.Add(new InterfaceDataBindingItem("bc_his_name", bc_his_name));
                                                list.Add(new InterfaceDataBindingItem("bc_pid", bc_pid));
                                                list.Add(new InterfaceDataBindingItem("bc_times", bc_times));
                                                list.Add(new InterfaceDataBindingItem("bc_name", bc_name));
                                                list.Add(new InterfaceDataBindingItem("op_time", DateTime.Now));
                                                list.Add(new InterfaceDataBindingItem("bc_social_no", bc_social_no));
                                                list.Add(new InterfaceDataBindingItem("op_code", opcode));
                                                list.Add(new InterfaceDataBindingItem("op_name", downloadInfo.OperationDepId));

                                                DataInterfaceHelper dihelper =
                                                    new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                                                if (downloadInfo.DownloadType == PrintType.Outpatient)
                                                {
                                                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload") == "HL7V3")
                                                    {
                                                        Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                                        eyComfirm.bc_ori_id = "107";
                                                        eyComfirm.bc_order_id = advice_id;
                                                        eyComfirm.OrderStatus = "40";
                                                        eyComfirm.doctor_id = "";
                                                        eyComfirm.doctor_name = "";
                                                        new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                                    }

                                                    string gn = "条码_门诊_下载后";
                                                    if (ConfigurationManager.AppSettings["HospitalType"] == "1")
                                                        gn = "分院_" + gn;

                                                    dihelper.ExecuteNonQueryWithGroup(gn, list.ToArray());
                                                }
                                                else if (downloadInfo.DownloadType == PrintType.Inpatient)
                                                {
                                                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload") == "HL7V3")
                                                    {
                                                        Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                                        eyComfirm.bc_ori_id = "108";
                                                        eyComfirm.bc_order_id = advice_id;
                                                        eyComfirm.OrderStatus = "40";
                                                        eyComfirm.doctor_id = "";
                                                        eyComfirm.doctor_name = "";
                                                        new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                                    }
                                                    dihelper.ExecuteNonQueryWithGroup("条码_住院_下载后", list.ToArray());
                                                }
                                                else if (downloadInfo.DownloadType == PrintType.TJ)
                                                {
                                                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirmWithDowload") == "HL7V3")
                                                    {
                                                        Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                                        eyComfirm.bc_ori_id = "109";
                                                        eyComfirm.bc_order_id = advice_id;
                                                        eyComfirm.OrderStatus = "40";
                                                        eyComfirm.doctor_id = "";
                                                        eyComfirm.doctor_name = "";
                                                        new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                                    }
                                                    dihelper.ExecuteNonQueryWithGroup("条码_体检_下载后", list.ToArray());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException("医嘱确认", ex);
                            throw;
                        }


                        #endregion

                        if (showLogger)
                        {
                            Logger.LogInfo("条码下载:插入两主表", watch.ElapsedMilliseconds.ToString() + "ms");
                            watch.Stop();
                        }
                    }

                    #endregion
                }

                //BarConcurrencyController.RemoveKey(downloadInfo);
                if (DebugMode)
                {
                    Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                }
                dtSource.Clear();
                //dtBCPatients.Clear();
                dtBCPatientsForInsert.Clear();
                dtCNameStructure.Clear();
                return dsLisData;
            }
            catch (Exception ex)
            {
                //BarConcurrencyController.RemoveKey(downloadInfo);
                Logger.LogException("DownloadBarcodeFromHIS2", ex);
                throw;
            }
        }


        public void ThreadUpdateFee(object objdtBCPatientsForInsert)
        {
            try
            {
                DataSet ds = (DataSet)objdtBCPatientsForInsert;
                DataTable dtBCPatientsForInsert = ds.Tables[0];
                Logger.LogInfo("确认费用", "!");
                //if (dtBCPatientsForInsert == null) return;
                Logger.LogInfo("确认费用", "2");

                Logger.LogInfo("确认费用11", dtBCPatientsForInsert.Rows.Count.ToString());
                List<string> barcodelist = new List<string>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (DataRow row in dtBCPatientsForInsert.Rows)//遍历每一条条码
                {
                    Logger.LogInfo("确认费用", row["bc_bar_code"].ToString());
                    string barcode = row["bc_bar_code"].ToString();
                    if (!barcodelist.Contains(barcode))
                    {
                        barcodelist.Add(barcode);
                        SZRYService ser = new SZRYService();
                        string msg = ser.ConfirmZyApplyInfo(barcode);
                        Logger.LogInfo("确认费用", msg);
                        dic.Add(barcode, msg);
                    }
                }
                SetFeeFlag(dic);
            }
            catch (Exception ex)
            {
                Logger.LogException("ThreadUpdateFee", ex);
                throw;
            }
        }


        private DataSet SetDepInfo(BarcodeDownloadInfo downloadInfo, DataSet dsLisData)
        {
            if (downloadInfo.DeptID != null)
            {
                string strSql = string.Format("select dep_name from dict_depart where dep_code='{0}'", downloadInfo.DeptID);

                SqlHelper helper = new SqlHelper();
                DataTable dtDep = helper.GetTable(strSql);
                if (dtDep != null && dtDep.Rows.Count > 0)
                {
                    string strDepName = dtDep.Rows[0]["dep_name"].ToString();

                    foreach (DataRow drDep in dsLisData.Tables[0].Rows)
                    {
                        drDep["bc_d_code"] = downloadInfo.DeptID;
                        drDep["bc_d_name"] = strDepName;
                    }
                    return dsLisData;
                }
                else
                {
                    return dsLisData;
                }
            }
            else
                return dsLisData;
        }


        /// <summary>
        /// 根据组合排序排列医嘱
        /// </summary>
        /// <param name="dtBCPatients"></param>
        /// <param name="bcCombinesCache"></param>
        /// <returns></returns>
        private void setBCPatientsSeq(DataSet dtBCPatients, DataTable bcCombinesCache, BarcodeDownloadInfo downloadInfo)
        {

            //ConvertHelper.InitContrast(downloadInfo);//2014年2月21日15:36:23 ye

            dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);

            DataTable dtResult = dtBCPatients.Tables[0];
            dtResult.Columns.Add("bc_his_seq", typeof(int));
            foreach (DataRow drBCPatients in dtResult.Rows)
            {
                //  DataRow[] bcComSeq = bcCombinesCache.Select("com_his_fee_code='" + drBCPatients[ConvertHelper.GetHISColumn("bc_his_code")] + "'");//2014年2月21日15:36:23 ye
                DataRow[] bcComSeq = bcCombinesCache.Select("com_his_fee_code='" + drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")] + "'");

                if (bcComSeq.Length > 0)
                {
                    drBCPatients["bc_his_seq"] = bcComSeq[0]["com_seq"].ToString().Trim() == "" ? 99999 : bcComSeq[0]["com_seq"];
                }
                else
                    drBCPatients["bc_his_seq"] = 999999;
            }
            //   dtResult.DefaultView.Sort = string.Format("{0},bc_his_seq", ConvertHelper.GetHISColumn("bc_name"));//2014年2月21日15:36:23 ye
            dtResult.DefaultView.Sort = string.Format("{0},bc_his_seq", columnConvertHelper.GetHISColumn("bc_name"));

            DataTable dtNewResult = dtResult.DefaultView.ToTable();
            dtNewResult.Columns.Remove("bc_his_seq");
            dtBCPatients.Tables.Remove(dtResult);
            dtBCPatients.Tables.Add(dtNewResult);
            //DataView dvBCPatients = new DataView(dtBCPatients.Copy());
            //foreach (BarcodeCombines bc in bcCombinesCache)
            //{
            //    dvBCPatients.RowFilter = string.Format("{0} like '%{1}%'", ConvertHelper.GetHISColumn("bc_his_name"), bc.LisCombineName);
            //    dvBCPatients.Sort = ConvertHelper.GetHISColumn("bc_name");
            //    if (dvBCPatients.Count > 0)
            //    {
            //        for (int i = 0; i < dvBCPatients.Count; i++)
            //        {
            //            dtResult.Rows.Add(dvBCPatients[i].Row.ItemArray);
            //            dtBCPatients.Rows.Remove(dvBCPatients[i].Row);
            //        }
            //    }
            //}

            //foreach (DataRow drBcpatients in dtBCPatients.Rows)//把不在BccombinesCache的数据添加到最后
            //{
            //    dtResult.Rows.Add(drBcpatients.ItemArray);
            //}
            //return dtResult;
        }

        /// <summary>
        /// 过滤HIS码未匹配的信息
        /// </summary>
        /// <param name="dtBCPatients">his数据</param>
        /// <param name="bcCombinesCache">组合条码his码信息</param>
        /// <param name="downloadInfo">条码下载参数</param>
        private void filtrateBCPatientsHisNotMatch(DataSet dtBCPatients, DataTable bcCombinesCache, BarcodeDownloadInfo downloadInfo)
        {
            if (dtBCPatients != null && dtBCPatients.Tables.Count > 0 && dtBCPatients.Tables[0] != null
                && dtBCPatients.Tables[0].Rows.Count > 0)
            {
                dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);

                DataTable dtResult = dtBCPatients.Tables[0].DefaultView.ToTable();

                dtBCPatients.Tables[0].Rows.Clear();

                string temp_ori_id_type = downloadInfo.GetDownloadTypeId();//下载来源

                foreach (DataRow drBCPatients in dtResult.Rows)
                {
                    if (bcCombinesCache != null && bcCombinesCache.Rows.Count > 0 && !string.IsNullOrEmpty(string.Format("{0}", drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")].ToString()))
                        && drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")].ToString().Length > 0
                        && drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")].ToString().Trim().Length > 0)
                    {
                        DataRow[] bcComSeq = bcCombinesCache.Select("com_his_fee_code='" + drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")].ToString() + "'");

                        if (bcComSeq.Length > 0)
                        {
                            dtBCPatients.Tables[0].Rows.Add(drBCPatients.ItemArray);
                            continue;
                        }
                    }

                    string str_p_log = string.Format("在病人来源[{0}]中未找到匹配的com_his_fee_code='{1}'\r\n", temp_ori_id_type, drBCPatients[columnConvertHelper.GetHISColumn("bc_his_code")].ToString());
                    Lib.LogManager.Logger.LogInfo("HisCodeNotMatchLisCode_log", str_p_log);
                }

            }
        }

        /// <summary>
        /// 过滤相同的医嘱ID
        /// </summary>
        /// <param name="dtBCPatients">his数据</param>
        /// <param name="downloadInfo">条码下载参数</param>
        private void filtrateBCPatientsSameYZ(DataSet dtBCPatients, BarcodeDownloadInfo downloadInfo)
        {
            if (dtBCPatients != null && dtBCPatients.Tables.Count > 0 && dtBCPatients.Tables[0] != null
                && dtBCPatients.Tables[0].Rows.Count > 0)
            {
                dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);
                //his对应的医嘱id的字段名
                string ColOrderIDforHIS = columnConvertHelper.GetHISColumn("bc_order_id");

                string temp_ori_id_type = downloadInfo.GetDownloadTypeId();//下载来源

                if (!string.IsNullOrEmpty(ColOrderIDforHIS) && dtBCPatients.Tables[0].Columns.Contains(ColOrderIDforHIS))
                {
                    DataTable dtResult = dtBCPatients.Tables[0].DefaultView.ToTable();

                    dtBCPatients.Tables[0].Rows.Clear();

                    foreach (DataRow drBCPatients in dtResult.Rows)
                    {
                        string selectforDt = string.Format("{0}='{1}'", ColOrderIDforHIS, drBCPatients[ColOrderIDforHIS].ToString());

                        if (dtBCPatients.Tables[0].Select(selectforDt).Length <= 0)
                        {
                            dtBCPatients.Tables[0].Rows.Add(drBCPatients.ItemArray);
                            continue;
                        }

                        string str_p_log = string.Format("在来源[{0}]中,同一批医嘱信息里出现相同的医嘱ID[{1}]\r\n", temp_ori_id_type, drBCPatients[ColOrderIDforHIS].ToString());
                        Lib.LogManager.Logger.LogInfo("HisCodeNotMatchLisCode_log", str_p_log);
                    }
                }
            }
        }

        private string FormatOrderId(string orderID, BarcodeDownloadInfo downloadInfo, bool ShouldOrderIdBindingOriId)
        {
            return ShouldOrderIdBindingOriId ? GetOriID(downloadInfo.DownloadType).Trim() + "_" + orderID : orderID;
        }



        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="downloadInfo"></param>
        /// <param name="dtCNameStructure"></param>
        /// <param name="connecter"></param>
        /// <param name="OrderTimeColumn"></param>
        /// <param name="dtSource"></param>
        /// <param name="dtBCPatients"></param>
        /// <param name="dtBCPatientsForInsert"></param>
        /// <param name="splitCodes"></param>
        /// <param name="now"></param>
        /// <param name="combinesCache"></param>
        /// <param name="i"></param>
        /// <param name="hisCombineID"></param>
        /// <param name="hisCombineName"></param>
        /// <param name="orderID"></param>
        /// <param name="newBarcode"></param>
        /// <param name="hasPatients"></param>
        /// <param name="barcodeCombine"></param>
        private void GenereateMainBarcode(BarcodeDownloadInfo downloadInfo, DataTable dtCNameStructure, IBCConnect connecter, string OrderTimeColumn, DataTable dtSource, DataTable dtBCPatients, DataTable dtBCPatientsForInsert, SplitCodeCollection splitCodes, ref DateTime now, IList<Combines> combinesCache, int i, string hisCombineID, string hisCombineName, string orderID, string orderID2, ref string newBarcode, ref bool hasPatients, BarcodeCombines barcodeCombine, bool ShouldOrderIdBindingOriId)
        {
            DataRow newBCPatientRow = dtBCPatients.NewRow();
            newBCPatientRow.ItemArray = dtBCPatients.Rows[i].ItemArray;
            Combines combine = null;
            string bc_merge_comid = null;

            //系统配置：条码号使用校验位
            string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");

            if (barcodeCombine != null) //有合并条码信息
            {
                combine = FindLisCombineCacheById(combinesCache, barcodeCombine.CombineId); //合并条码信息
                SplitCodeInfo codeInfo = new SplitCodeInfo(barcodeCombine.ComSplitCode);
                codeInfo.GetMoreInfo(dtSource.Rows[i], downloadInfo);

                if (codeInfo != null && !string.IsNullOrEmpty(barcodeCombine.HisFeeCode)) //修正HIS收费代码以条码信息为准
                    codeInfo.HisFeeCode = barcodeCombine.HisFeeCode;

                SplitCodeInfo findCodeInfo = null;
                if (barcodeCombine == null) //没有对应的项目
                {
                    //Logger.WriteException("GenereateMainBarcode", "没有对应的项目", "");

                    hasPatients = false;

                    if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                    {
                        //滨海模块-生成条码号
                        newBarcode = CreateBarcodeNumberForBhMode(null, null, null);
                        //newBarcode = CreateBarcodeNumberForBhMode(barcodeCombine.ExecPtypeCode);
                    }
                    else
                    {
                        //生成条码号
                        newBarcode = CreateBarcodeNumber();
                    }
                }
                else if (string.IsNullOrEmpty(barcodeCombine.ComSplitCode)
                    || !splitCodes.Contains(codeInfo, ref findCodeInfo)) //如果合并代码不一样则生成不同的项目和条码
                {
                    //Logger.WriteException("GenereateMainBarcode", "else if (string.IsNullOrEmpty(barcodeCombine.ComSplitCode)", "");
                    hasPatients = false;

                    if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                    {
                        //滨海模块-生成条码号
                        if (barcodeCombine == null)
                        {
                            newBarcode = CreateBarcodeNumberForBhMode(null, null, null);
                        }
                        else
                        {
                            newBarcode = CreateBarcodeNumberForBhMode(barcodeCombine.ExecPtypeCode, null, barcodeCombine.CombineId);
                        }
                    }
                    else
                    {
                        newBarcode = CreateBarcodeNumber();
                    }

                    if (!string.IsNullOrEmpty(barcodeCombine.ComSplitCode)) //没有合并代码则肯定不合并
                    {
                        codeInfo.IncludeHisFeeCodes.Add(codeInfo.HisFeeCode);//试管没有此收费代码的项目则添加上去
                        codeInfo.NewBarcode = newBarcode;
                        splitCodes.Add(codeInfo);
                    }
                }
                else
                {
                    if (findCodeInfo != null)//可以合并的条码
                    {
                        newBarcode = findCodeInfo.NewBarcode;
                        findCodeInfo.IncludeHisFeeCodes.Add(codeInfo.HisFeeCode);//试管没有此收费代码的项目则添加上去
                        hasPatients = true;

                    }
                }
            }
            else //没有合并条码信息
            {
                //Logger.WriteException("GenereateMainBarcode", "barcodeCombine=null", "");
                if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                {
                    //滨海模块-生成条码号
                    if (barcodeCombine == null)
                    {
                        newBarcode = CreateBarcodeNumberForBhMode(null, null, null);
                    }
                    else
                    {
                        newBarcode = CreateBarcodeNumberForBhMode(barcodeCombine.ExecPtypeCode, null, barcodeCombine.CombineId);
                    }
                }
                else
                {
                    newBarcode = CreateBarcodeNumber();
                }
            }

            //暂时不分预置条码和自动条码
            newBCPatientRow[BarcodeTable.Patient.BarcodeNumber] = newBarcode;
            newBCPatientRow[BarcodeTable.Patient.BarcodeDisplayNumber] = newBarcode;
            newBCPatientRow[BarcodeTable.Patient.CreateTime] = now.ToString();
            newBCPatientRow[BarcodeTable.Patient.LastActionTime] = newBCPatientRow[BarcodeTable.Patient.CreateTime];//added by lin : 2010/7/6
            newBCPatientRow[BarcodeTable.Patient.IDType] = GetNoTypeID(downloadInfo.DownloadType); //病人ID类型

            //拆分大组合(特殊合并)ID
            if (newBCPatientRow.Table.Columns.Contains("bc_merge_comid")
                && string.IsNullOrEmpty(newBCPatientRow["bc_merge_comid"].ToString())
                && barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.MergeComID))
            {
                newBCPatientRow["bc_merge_comid"] = barcodeCombine.MergeComID;
            }

            if (combine != null)
                newBCPatientRow["bc_notice"] = combine.Remark;

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("7") == "预置条码" && downloadInfo.IsPrePlaceBarcode)
                newBCPatientRow["bc_bar_type"] = "1";
            else
                newBCPatientRow["bc_bar_type"] = "0";
            if (ShouldOrderIdBindingOriId)
                newBCPatientRow[BarcodeTable.Patient.OriID] = GetOriID(downloadInfo.DownloadType);
            //发票号
            if (!string.IsNullOrEmpty(downloadInfo.InvoiceID))
            {
                newBCPatientRow[BarcodeTable.Patient.SocialNumber] = downloadInfo.InvoiceID;
            }
            //如果项目有"产检男性" 姓名后加"之夫"
            connecter.SpecialPatient(ref newBCPatientRow, hisCombineName);
            //来源
            if (barcodeCombine != null)
            {
                if (!ShouldOrderIdBindingOriId)
                    if (!string.IsNullOrEmpty(barcodeCombine.Oriid))
                        newBCPatientRow[BarcodeTable.Patient.OriID] = barcodeCombine.Oriid; //医嘱
                    else
                        newBCPatientRow[BarcodeTable.Patient.OriID] = GetOriID(downloadInfo.DownloadType);

                //系统配置：下载条码优先取本系统组合条码信息的标本类型
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_BCDW_priorityDictSamID") == "是"
                    && barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.Sampleid))
                {
                    newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                }
                else if (newBCPatientRow[BarcodeTable.Patient.SampleID].ToString() != string.Empty)
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_id == newBCPatientRow[BarcodeTable.Patient.SampleID].ToString());

                    if (dict_sam != null)
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                    else
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别
                }
                else if (newBCPatientRow[BarcodeTable.Patient.SampleName].ToString() != string.Empty)
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_name == newBCPatientRow[BarcodeTable.Patient.SampleName].ToString());

                    if (dict_sam != null
                        && string.IsNullOrEmpty(barcodeCombine.Sampleid)
                        && string.IsNullOrEmpty(newBCPatientRow[BarcodeTable.Patient.SampleID].ToString()))
                    {
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                    }
                    else
                    {
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                    }
                }
                else
                {
                    newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                }

                //newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                newBCPatientRow[BarcodeTable.Patient.CuvetteCode] = barcodeCombine.CuvCode;//试管类别
                newBCPatientRow["bc_sam_dest"] = barcodeCombine.ComSamDest;
                newBCPatientRow[BarcodeTable.Patient.SampleRemarkID] = barcodeCombine.SampleRemarkid;//标本备注
                newBCPatientRow[BarcodeTable.Patient.CTypeID] = barcodeCombine.ExecPtypeCode;//检验系统的科室字典
                newBCPatientRow[BarcodeTable.Patient.PrintCount] = barcodeCombine.PrintCount; //打印次数
            }

            if (dtSource.Columns.Contains("Dspt")) //标本备注或备注
            {
                //新
                //his的注释写入到bc_patients中的bc_sam_rem_name中
                newBCPatientRow[BarcodeTable.Patient.Remark] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkID] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkName] = dtSource.Rows[i]["Dspt"].ToString();

                //注释by lin
                //原:his的注释写入到bc_patients中的bc_exp中
                //newBCPatientRow[BarcodeTable.Patient.Remark] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkName] = dtSource.Rows[i]["Dspt"].ToString();
            }
            //157需求 备注中含X天X时 填充到年龄 格式为0Y0M0D0H
            if (string.IsNullOrEmpty(newBCPatientRow[BarcodeTable.Patient.Remark].ToString())
                && newBCPatientRow[BarcodeTable.Patient.Remark].ToString().Contains("生后"))
            {
                try
                {

                    //生后第1天

                    //获取最后的单位 天或时
                    string dayorhour = newBCPatientRow[BarcodeTable.Patient.Remark].ToString().Substring(newBCPatientRow[BarcodeTable.Patient.Remark].ToString().Length - 1, 1);
                    //获取具体的值
                    string num = newBCPatientRow[BarcodeTable.Patient.Remark].ToString().Replace("生后", "")
                                                                                                                         .Replace("天", "")
                                                                                                                         .Replace("小时", "");
                    int number = 0;

                    if (int.TryParse(num, out number))
                    {
                        if (dayorhour == "天")
                        {
                            newBCPatientRow[BarcodeTable.Patient.Age] = string.Format("0Y0M{0}D0H", number);
                        }
                        else if (dayorhour == "时")
                        {
                            newBCPatientRow[BarcodeTable.Patient.Age] = string.Format("0Y0M0D{0}H", number);
                        }
                    }
                }
                catch
                {

                }

            }
            string showHisCode = "";
            string showHisName = "";

            if (combine != null)
            {
                showHisCode = combine.Id;
                showHisName = combine.Name;
            }

            //条码明细表
            DataRow cnNameRow = dtCNameStructure.NewRow();
            cnNameRow[BarcodeTable.CName.BarcodeNumber] = newBarcode;
            cnNameRow[BarcodeTable.CName.BarcodeDisplayNumber] = newBarcode;
            cnNameRow[BarcodeTable.CName.SignFlag] = 0;
            //未加执行科室编码,执行科室名称             
            if (barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.HisCombineCode)) //如果有条码信息的his code 就取
                cnNameRow[BarcodeTable.CName.HisCode] = barcodeCombine.HisCombineCode;
            else
                cnNameRow[BarcodeTable.CName.HisCode] = hisCombineID;

            if (barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.HisCombineName)) //如果有条码信息的his name 就取
                cnNameRow[BarcodeTable.CName.HisName] = barcodeCombine.HisCombineName;
            else
                cnNameRow[BarcodeTable.CName.HisName] = hisCombineName;
            // cnNameRow[BarcodeTable.CName.HisCode] =barcodeCombine.HisCombineCode hisCombineID;//connecter.GetHisCode(hisCombineID, showHisCode);
            //cnNameRow[BarcodeTable.CName.HisName] = hisCombineName;

            cnNameRow[BarcodeTable.CName.CombineName] = connecter.GetHisName(hisCombineName, showHisName, barcodeCombine);
            //特殊项目名称
            cnNameRow = connecter.SpecialItem(hisCombineName, cnNameRow, combine, barcodeCombine);
            cnNameRow[BarcodeTable.CName.OrderID] = orderID;//医嘱ID
            cnNameRow["bc_yz_id2"] = orderID2;
            cnNameRow = SetOrderTime(OrderTimeColumn, dtSource.Rows[i], cnNameRow);//医嘱申请时间
            cnNameRow[BarcodeTable.CName.EnrolFlag] = 0;
            cnNameRow[BarcodeTable.CName.ViewFlag] = 1;//显示标志(0-不显示 1-显示)

            if (barcodeCombine != null)
            {
                cnNameRow[BarcodeTable.CName.LisCombineCode] = barcodeCombine.CombineId;
                //cnNameRow["bc_combine_seq"]=barcodeCombine.
                cnNameRow[BarcodeTable.CName.Price] = barcodeCombine.Price;
                cnNameRow[BarcodeTable.CName.Unit] = barcodeCombine.Unit;
                //注意事项 保存与采血
                cnNameRow[BarcodeTable.CName.SaveNotice] = barcodeCombine.SaveNotice;
                cnNameRow[BarcodeTable.CName.BloodNotice] = barcodeCombine.BloodNotice;
            }

            newBCPatientRow[BarcodeTable.Patient.Item] = connecter.GetHisName(hisCombineName, showHisName, barcodeCombine);

            if (hasPatients == false)
            {
                dtBCPatientsForInsert.Rows.Add(newBCPatientRow.ItemArray);

                //added by lin 2010/5/28:新条码默认回退次数为0
                if (dtBCPatientsForInsert.Columns.Contains("bc_return_times"))
                {
                    dtBCPatientsForInsert.Rows[dtBCPatientsForInsert.Rows.Count - 1]["bc_return_times"] = 0;
                }
            }
            else
            { //更新项目名称
                DataRow[] drFindBC = dtBCPatientsForInsert.Select(string.Format("bc_bar_no = '{0}'", newBarcode));
                if (drFindBC != null && drFindBC.Length == 1)
                    drFindBC[0][BarcodeTable.Patient.Item] = drFindBC[0][BarcodeTable.Patient.Item].ToString() + "+" + cnNameRow[BarcodeTable.CName.CombineName].ToString();
            }


            dtCNameStructure.Rows.Add(cnNameRow);
        }

        private int MoreThanOne(DataRow row, dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper)
        {
            int count = 1;
            if (row != null && row.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_qty")))
            {
                //  count = int.Parse(row["Qty"].ToString());

                try
                {
                    count = Convert.ToInt32(row[columnConvertHelper.GetHISColumn("bc_qty")]);
                }
                catch { }

                if (count > 1)
                {
                }
                return count;
            }

            return count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public IList<BarcodeCombinesRecodes> FindBarcodeCombinesRecodes(string barcode)
        {
            return sqlHelper.SqlMapper.QueryForList<BarcodeCombinesRecodes>("SelectBarcodeCombinesRecodesByBarcode", barcode);
        }

        /// <summary>
        /// 在找到的记录里找出对应来源的记录，如住院或门诊或默认
        /// </summary>
        /// <param name="barcodeCombines"></param>
        /// <param name="downloadInfo"></param>
        /// <returns></returns>
        private IList<BarcodeCombines> FindWantBarcodeCombines(IList<BarcodeCombines> barcodeCombines, BarcodeDownloadInfo downloadInfo)
        {
            IList<BarcodeCombines> resultFinal = new List<BarcodeCombines>();
            IList<BarcodeCombines> resultTemp = barcodeCombines;
            if (barcodeCombines.Count <= 1)
                return barcodeCombines;
            while (resultTemp.Count > 0)
            {
                #region 新代码
                IList<BarcodeCombines> result = GetGroupBarcodeCombines(resultTemp);

                //2010-09-28解决有相同his编码但在不同来源时取不到lis_code的问题
                //IList<BarcodeCombines> result = resultTemp;// GetGroupBarcodeCombines(resultTemp);
                #endregion


                #region 旧代码
                //找出相同com_id的条码信息
                //IList<BarcodeCombines> result = GetGroupBarcodeCombines(resultTemp);
                //在找到的条码信息中找对应来源的条码信息
                #endregion

                BarcodeCombines choose = ChooseBarcodeCombines(result, downloadInfo);
                if (choose != null)
                {
                    resultFinal.Add(choose);
                    Remove(resultTemp, result);
                    result.Clear();
                }
                //else
                //    break;
                //以下代码解决惠州门诊条码信息不同组合存在不同来源但收费码相同的情况，请测试后再放出
                else
                {
                    Remove(resultTemp, result);
                    result.Clear();
                    continue;
                }
            }

            return resultFinal;
        }




        private void Remove(IList<BarcodeCombines> resultTemp, IList<BarcodeCombines> result)
        {
            for (int i = result.Count - 1; i >= 0; i--)
            {

                if (resultTemp.Contains(result[i]))
                {
                    resultTemp.Remove(result[i]);
                }
            }

            //foreach (BarcodeCombines item in result)
            //{
            //    if (resultTemp.Contains(item))
            //    {
            //        resultTemp.Remove(item);
            //    }
            //}
        }

        /// <summary>
        /// 只拿相同组合的条码信息
        /// </summary>
        /// <param name="barcodeCombines"></param>
        /// <returns></returns>
        private IList<BarcodeCombines> GetGroupBarcodeCombines(IList<BarcodeCombines> barcodeCombines)
        {
            IList<BarcodeCombines> result = new List<BarcodeCombines>();
            foreach (BarcodeCombines item in barcodeCombines)
            {
                if (result.Count == 0)
                {
                    result.Add(item);
                    continue;
                }
                else
                    if (ContainsCombineID(result, item))//com_id相同
                    result.Add(item);
            }
            return result;
        }

        private BarcodeCombines ChooseBarcodeCombines(IList<BarcodeCombines> result, BarcodeDownloadInfo downloadInfo)
        {
            //bool hasFound = false;
            //BarcodeCombines defaultBC = null;
            //foreach (BarcodeCombines com in result)
            //{
            //    if (!string.IsNullOrEmpty(com.Oriid) && com.Oriid == GetOriID(downloadInfo.DownloadType))
            //        return com; //有来源就用来源
            //    else if (string.IsNullOrEmpty(com.Oriid))
            //    {
            //        defaultBC = com; //取默认
            //    }

            //}

            //return defaultBC;
            BarcodeCombines defaultBC = null;
            string oriid = GetOriID(downloadInfo.DownloadType);

            if (result.Any(i => i.Oriid == oriid))
            {
                defaultBC = result.First(i => i.Oriid == oriid);
            }

            if (defaultBC == null)
            {
                if (result.Any(i => string.IsNullOrEmpty(i.Oriid)))
                {
                    defaultBC = result.First(i => string.IsNullOrEmpty(i.Oriid));
                }
            }
            return defaultBC;




        }

        private bool ContainsCombineID(IList<BarcodeCombines> result, BarcodeCombines item)
        {
            foreach (BarcodeCombines oneItem in result)
            {
                if (oneItem.CombineId == item.CombineId)
                {
                    return true;
                }
            }

            return false;
        }

        private IList<BarcodeCombines> FindBarcodeCombineCacheByHISFeeCode(IList<BarcodeCombines> bcCombinesCache, string hisCombineID, ref IList<Combines> combinesCache, ref IList<BarcodeCombines> bcCombinesCache_4)
        {
            bool moreBCCombines = false;
            IList<BarcodeCombines> result = new List<BarcodeCombines>();
            foreach (BarcodeCombines com in bcCombinesCache)
            {
                if (com.HisFeeCode == hisCombineID) //来源
                {
                    if (com.SplitFlag == 1) //如果为大组合的条码信息 
                    {
                        result = FindSubCombines(com.CombineId);

                        #region 生成特殊项目合并大组合ID

                        if (com.ComFatherFlag == 1)
                        {
                            string guid_MergeComID = new Lib.DAC.GlobalSysTableIDGenerator().Generate("bc_patients", "MergeComID", "");
                            com.MergeComID = guid_MergeComID + "," + com.CombineId;

                            if (result != null && result.Count > 0)
                            {
                                for (int i = 0; i < result.Count; i++)
                                {
                                    result[i].MergeComID = com.MergeComID;
                                }
                            }
                        }

                        #endregion

                        //string subIdMessage = string.Empty;
                        //foreach (BarcodeCombines temp_item in result)
                        //{
                        //    subIdMessage += "," + temp_item.CombineId.ToString();
                        //}

                        //Logger.WriteException("条码拆分日志", "拆分", "源组合id" + com.CombineId + "拆分" + subIdMessage);

                        moreBCCombines = true;
                        IList<string> lisCombineIDs = GetLisCombineIDs(result);
                        IList<Combines> combines = FindLisCombinesByIDs(lisCombineIDs);
                        if (combines != null)//edit by sink 2010-10-12
                        {
                            foreach (Combines combine in combines)
                            {
                                combinesCache.Add(combine);
                            }
                        }
                    }
                    else
                        result.Add(com);
                }
            }


            if (moreBCCombines)
                foreach (BarcodeCombines item in result)
                {
                    bcCombinesCache_4.Add(item);
                }
            return result;
        }

        private IList<string> GetLisCombineIDs(IList<BarcodeCombines> barcodeCombines)
        {
            IList<string> result = new List<string>();
            foreach (BarcodeCombines item in barcodeCombines)
            {
                result.Add(item.CombineId);
            }

            return result;
        }

        private string GetOriID(PrintType printType)
        {
            if (printType == PrintType.Inpatient) //住院：108 门诊：107
                return "108";
            else if (printType == PrintType.Outpatient)
                return "107";
            else if (printType == PrintType.TJ || printType == PrintType.SZSYTJ || printType == PrintType.TJSecond)
                return "109";

            return "";
        }

        private string GetNoTypeID(PrintType printType)
        {
            if (printType == PrintType.Inpatient) //住院：106 门诊：107
                return "106";
            else if (printType == PrintType.Outpatient)
                return "107";
            else if (printType == PrintType.TJ || printType == PrintType.SZSYTJ)
                return "110";
            else if (printType == PrintType.TJSecond)
                return "110";

            return "";
        }

        private bool HasOrderIDInCombinesRecord(IList<string> bcCombineRecodeOrderCache, string orderID)
        {
            return bcCombineRecodeOrderCache.Contains(orderID);
        }

        //private IList<string> ToList(IList<BarcodeCombinesRecodes> bcCombineRecodeCache)
        //{

        //}

        private IList<string> ToList(List<BarcodeCombinesRecodes> bcCombineRecodeCache, BarcodeDownloadInfo downloadInfo, bool ShouldOrderIdBindingOriId)
        {
            IList<string> result = new List<string>();
            if (bcCombineRecodeCache == null || bcCombineRecodeCache.Count == 0)
                return result;
            foreach (BarcodeCombinesRecodes item in bcCombineRecodeCache)
            {
                if (ShouldOrderIdBindingOriId)
                {
                    //result.Add((item.OriId == null ? "" : item.OriId) + item.Orderid);
                    result.Add((item.OriId == null ? "_" : item.OriId + "_") + item.Orderid);
                }
                else
                    result.Add(item.Orderid);
            }

            return result;
        }

        private IList<BarcodeCombinesRecodes> FindBCCombineRecode(IList<string> orderIDs)
        {
            if (orderIDs == null || orderIDs.Count == 0)
                return null;
            //return sqlHelper.SqlMapper.QueryForList<BarcodeCombinesRecodes>("FindBCCombineRecodeByOrderIDs", orderIDs);

            if (orderIDs.Count == 0)
            {
                return new List<BarcodeCombinesRecodes>();
            }

            StringBuilder sb = new StringBuilder();
            bool needComma = false;
            foreach (string bc_yz_id in orderIDs)
            {
                if (needComma)
                {
                    sb.Append(",");
                }

                sb.Append(string.Format("'{0}'", bc_yz_id));

                needComma = true;
            }


            string sql = string.Format(@"
SELECT
    bc_id as Id,
    bc_bar_no as BarcodeNumber,
    bc_bar_code as Barcode,
    bc_frequency as Frequency,
    bc_his_name as HisCombineName,
    bc_his_code as HisCombineid,
    bc_yz_id as Orderid,
    bc_ctype as Type,
    bc_flag as Flag,
    bc_date as CreateDate,
    bc_apply_date as OrderRequireDate,
    bc_occ_date as OrderExecuteDate,
    bc_item_no as ItemNumber,
    bc_confirm_flag as ConfirmFlag,
    bc_confirm_code as ConfirmCode,
    bc_confirm_name as ConfirmName,
    bc_price as Price,
    bc_unit as Unit,
    bc_modify_flag as ModifyFlag,
    bc_modify_date as ModifyDate,
    bc_itr_id as Itrid,
    bc_view_flag as ViewFlag,
    bc_exec_code as ExecPtypeCode,
    bc_exec_name as ExecPtypeName,
    bc_out_time as ExpertReportTime,
    bc_rep_flag as ReportFlag,
    bc_rep_date as ReportDate,
    bc_enrol_flag as EnrolFlag,
    bc_other_no as OtherInfo,
    bc_del as DeleteFlag,
    bc_lis_code as LisCombineid,
    bc_name as ComName
FROM bc_cname WHERE bc_del=0 and bc_yz_id in ({0})", sb.ToString());

            EntityHelper dac = new EntityHelper();
            DbCommandEx cmd = dac.CreateCommandEx(sql);
            cmd.CommandTimeout = 600;

            List<BarcodeCombinesRecodes> list = dac.SelectMany<BarcodeCombinesRecodes>(cmd);

            string Interface_HospitalInterfaceMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");


            #region 肿瘤医院配置

            //try
            //{
            //    //肿瘤医院配置
            //    if (Interface_HospitalInterfaceMode == "肿瘤医院" && orderIDs != null && orderIDs.Count > 0)
            //    {
            //        List<BarcodeCombinesRecodes> list2 = ZLYY_FindBCCombineRecodeReturnOriId2(orderIDs);
            //        foreach (BarcodeCombinesRecodes item in list2)
            //        {
            //            list.Add(item);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogException("FindBCCombineRecode", ex);
            //}

            #endregion

            return list;
        }

        private IList<BarcodeCombinesRecodes> FindBCCombineRecodeReturnOriId(IList<string> orderIDs)
        {
            if (orderIDs == null || orderIDs.Count == 0)
                return null;

            IList<BarcodeCombinesRecodes> list = sqlHelper.SqlMapper.QueryForList<BarcodeCombinesRecodes>("FindBCCombineRecodeByOrderIDsWithOriId", orderIDs);

            string Interface_HospitalInterfaceMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");


            #region 肿瘤医院配置

            try
            {
                //肿瘤医院配置
                if (Interface_HospitalInterfaceMode == "肿瘤医院" && orderIDs != null && orderIDs.Count > 0)
                {
                    //List<BarcodeCombinesRecodes> list2 = ZLYY_FindBCCombineRecodeReturnOriId2(orderIDs);
                    //foreach (BarcodeCombinesRecodes item in list2)
                    //{
                    //    list.Add(item);
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("FindBCCombineRecodeReturnOriId 1307", ex);
            }

            #endregion

            return list;
        }


        //        private List<BarcodeCombinesRecodes> ZLYY_FindBCCombineRecodeReturnOriId(IList<string> orderIDs)
        //        {
        //            EntityHelper oldLisHelper = new EntityHelper(@"Data Source=172.16.95.168\lis;Initial Catalog=clab;User ID=sa;Password=admin;min pool size=1", EnumDbDriver.MSSql, EnumDataBaseDialet.SQL2000);

        //            string sqlSelectOldLis = @"
        //select
        //    bc_bar_code as BarcodeNumber,
        //    bc_bar_code as Barcode,
        //    bc_cname as HisCombineName,
        //    bc_c_code as HisCombineid,
        //    View_get_sam_info.OrderId as Orderid,
        //    bc_date as CreateDate
        //from View_get_sam_info
        //where OrderId in  ({0})";

        //            StringBuilder sqlSubSelectOldLis = new StringBuilder();

        //            bool needComma = false;
        //            foreach (string bc_order_exec_code in orderIDs)
        //            {
        //                if (needComma)
        //                {
        //                    sqlSubSelectOldLis.Append(",");
        //                }

        //                sqlSubSelectOldLis.Append(string.Format("'{0}'", bc_order_exec_code));
        //                needComma = true;
        //            }
        //            sqlSelectOldLis = string.Format(sqlSelectOldLis, sqlSubSelectOldLis.ToString());

        //            //Logger.WriteException("ZLYY_FindBCCombineRecodeReturnOriId 1346", "显示sql语句", sqlSelectOldLis);

        //            List<BarcodeCombinesRecodes> list2 = oldLisHelper.SelectMany<BarcodeCombinesRecodes>(sqlSelectOldLis);

        //            //Logger.WriteException("ZLYY_FindBCCombineRecodeReturnOriId 1350", "list2.Count", list2.Count.ToString());

        //            return list2;
        //        }


        private List<BarcodeCombinesRecodes> ZLYY_FindBCCombineRecodeReturnOriId2(IList<string> orderIDs)
        {
            List<BarcodeCombinesRecodes> list = new List<BarcodeCombinesRecodes>();

            EntityHelper oldLisHelper = new EntityHelper(@"Data Source=172.16.95.168\lis;Initial Catalog=clab;User ID=sa;Password=admin;min pool size=1", EnumDbDriver.MSSql, EnumDataBaseDialet.SQL2000);

            List<BarcodeCombinesRecodes> list2 = new List<BarcodeCombinesRecodes>();


            foreach (string bc_yz_id in orderIDs)
            {
                string[] strs = bc_yz_id.Split('&');
                if (strs.Length < 2)
                    continue;

                //有&符号拼接的为住院医嘱，没有拼接的为门诊

                //                if (strs.Length < 2)
                //                {
                //                    //处理门诊
                //                    string sqlSelectOldLis = string.Format(@"select
                //    bc_bar_code as BarcodeNumber,
                //    bc_bar_code as Barcode,
                //    bc_cname as HisCombineName,
                //    bc_c_code as HisCombineid,
                //    View_get_sam_info.OrderId as Orderid,
                //    bc_date as CreateDate
                //from View_get_sam_info
                //where bc_yz_id = '{0}' ", bc_yz_id);

                //                    try
                //                    {
                //                        list2 = oldLisHelper.SelectMany<BarcodeCombinesRecodes>(sqlSelectOldLis);
                //                    }
                //                    catch (Exception ex)
                //                    {
                //                        Logger.WriteException("ZLYY_FindBCCombineRecodeReturnOriId2 1488", "异常", list.Count.ToString());
                //                    }

                //                }
                //                else//处理住院
                //                {
                string sqlSelectOldLis = @"
select
    bc_bar_code as BarcodeNumber,
    bc_bar_code as Barcode,
    bc_cname as HisCombineName,
    bc_c_code as HisCombineid,
    View_get_sam_info.OrderId as Orderid,
    bc_date as CreateDate
from View_get_sam_info
where bc_order_code = '{0}' and bc_order_exec_code = '{1}'";

                string bc_order_code = strs[0];
                string bc_order_exec_code = strs[1];

                string sqlSelect = string.Format(sqlSelectOldLis, bc_order_code, bc_order_exec_code);



                try
                {
                    list2 = oldLisHelper.SelectMany<BarcodeCombinesRecodes>(sqlSelect);
                }
                catch (Exception ex)
                {
                    Logger.LogException("ZLYY_FindBCCombineRecodeReturnOriId2 1518 ->" + list.Count.ToString(), ex);
                }
                //}

                foreach (BarcodeCombinesRecodes item in list2)
                {
                    if (!string.IsNullOrEmpty(item.Orderid))
                        item.Orderid = item.Orderid.Trim();

                    list.Add(item);
                }
            }

            //Logger.WriteException("ZLYY_FindBCCombineRecodeReturnOriId2 1390", "list2.Count", list.Count.ToString());
            return list;
        }
        //private Combines FindLisCombineCacheByHISFeeCode(IList<Combines> combinesCache, string hisCombineID)
        //{
        //    foreach (Combines com in combinesCache)
        //    {
        //        if (com.HisFeeCode == hisCombineID)
        //        {
        //            return com;
        //        }
        //    }

        //    return null;
        //}

        private Combines FindLisCombineCacheById(IList<Combines> combinesCache, string combineID)
        {
            foreach (Combines com in combinesCache)
            {
                if (com.Id == combineID)
                {
                    return com;
                }
            }

            return null;
        }


        //private IList<Combines> FindLisCombineByHISFeeCode(IList<string> hisCombineIDs)
        //{
        //    return bcDictBIZ.SearchCombineByHISID(hisCombineIDs);
        //}

        private IList<BarcodeCombines> FindBarcodeCombineByHISFeeCode(IList<string> hisCombineIDs)
        {
            var list = bcDictBIZ.FindBarcodeCombineByHISFeeCode(hisCombineIDs).OrderBy(i => i.ComSeq).ToList();
            //var list2 = DictCombineBarCache.Current.FindBarcodeCombineByHISFeeCode(hisCombineIDs).OrderBy(i => i.ComSeq).ToList();
            return list;
        }

        private IList<Combines> FindLisCombinesByIDs(IList<string> combineIDs)
        {
            return bcDictBIZ.FindLisCombines(combineIDs);
        }

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="downloadInfo"></param>
        /// <returns></returns>
        public DataSet DownloadBarcodeFromHIS(BarcodeDownloadInfo downloadInfo)
        {
            return DownloadBarcodeFromHIS2(downloadInfo, null);
        }

        /// <summary>
        /// 生成条码号
        /// </summary>
        /// <returns></returns>
        private string CreateBarcodeNumber()
        {
            //获取最大条码号
            BCBarcodeBIZ barcodeBIZ = new BCBarcodeBIZ();
            string barcode = barcodeBIZ.GetNewBarcode();
            //不同医院的条码规则不同
            IBarcodeGenerateRule rule = new DefaultGenerateRule();

            //int checkcodetype = 1;//校验位默认使用2位前缀
            string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");
            //条码自定义前缀
            rule.barcodeCustomHead = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeCustomHead");
            if (chkcodetype == "不使用")
                rule.CheckCodeType = 0;
            else if (chkcodetype == "2位后缀")
                rule.CheckCodeType = 2;
            else if (chkcodetype == "自定义前缀")
                rule.CheckCodeType = 3;
            else
                rule.CheckCodeType = 1;

            bool rechck = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_CheckBarcodeExists") == "是";

            if (!rechck)
            {
                return rule.GenerateBarcode(barcode);
            }
            else
            {
                string newbarcode = rule.GenerateBarcode(barcode);

                if (IsExistsBarcode(newbarcode))
                    return CreateBarcodeNumber();
                return newbarcode;
            }
        }

        bool IsExistsBarcode(string barcode)
        {
            string sql = string.Format("select count(1) from bc_patients with(NOLOCK) where bc_bar_code='{0}'", barcode);

            SqlHelper helper = new SqlHelper();

            object ob = helper.ExecuteScalar(sql);

            if (ob != null && Convert.ToInt32(ob) > 0)
                return true;

            return false;

        }

        /// <summary>
        /// 生成条码号-滨海模式
        /// </summary>
        /// <param name="com_ctype">物理组</param>
        /// <param name="sel_barcode">用条码号查物理组</param>
        /// <param name="sel_comID">组合ID</param>
        /// <returns></returns>
        private string CreateBarcodeNumberForBhMode(string com_ctype, string sel_barcode, string sel_comID)
        {
            //条码生成规1：2位年份+2位分类+7位流水号+1位校验，共12位
            //条码生成规2：2位年份+2位物理组编码+1位专业组编码+7位流水号，共12位
            string NewBarcode = "";

            try
            {
                if (true)
                {
                    //条码生成规2：2位年份+2位物理组编码+1位专业组编码+7位流水号，共12位
                    #region 条码生成规2

                    string id_key = "";
                    string strYear = DateTime.Now.Year.ToString("D4").Substring(2, 2);//获取年份(后2位)
                    string strCtypeSort = "99";//2位分类(默认为99)
                    string strPtypeSort = "0";//1位专业组编码(默认为0)

                    if (!string.IsNullOrEmpty(com_ctype) || !string.IsNullOrEmpty(sel_barcode))
                    {
                        DBHelper helper = new DBHelper();

                        //获取物理组信息（备注信息--作为分类）
                        DataTable dtTempctype = null;

                        if (!string.IsNullOrEmpty(com_ctype))
                        {
                            dtTempctype = helper.GetTable(string.Format("select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id='{0}'", com_ctype));
                        }
                        else if (!string.IsNullOrEmpty(sel_barcode))
                        {
                            dtTempctype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id=(
select top 1 bc_ctype from bc_patients with(NOLOCK) where bc_bar_no='{0}')", sel_barcode));
                        }

                        if (dtTempctype != null && dtTempctype.Rows.Count > 0)
                        {
                            string str_type_exp = dtTempctype.Rows[0]["type_exp"].ToString();
                            if (!string.IsNullOrEmpty(str_type_exp))
                            {
                                int int_type_exp = 99;
                                if (int.TryParse(str_type_exp, out int_type_exp))
                                {
                                    if (int_type_exp <= 99 && int_type_exp > 0)
                                    {
                                        strCtypeSort = int_type_exp.ToString("D2");
                                    }
                                }
                            }
                        }
                    }

                    #region 获取专业组的1位分类码

                    if (!string.IsNullOrEmpty(sel_comID) || !string.IsNullOrEmpty(sel_barcode))
                    {
                        DBHelper helper = new DBHelper();

                        //获取专业组信息（备注信息--作为分类）
                        DataTable dtTempptype = null;

                        if (!string.IsNullOrEmpty(sel_comID))
                        {
                            dtTempptype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='0' and type_id=(
select top 1 com_ptype from dict_combine with(NOLOCK) where com_id='{0}')", sel_comID));
                        }
                        else if (!string.IsNullOrEmpty(sel_barcode))
                        {
                            dtTempptype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='0' and type_id=(
select top 1 com_ptype from dict_combine with(NOLOCK) where com_id=(
select top 1 bc_lis_code from bc_cname with(NOLOCK) where bc_del='0' and bc_bar_no='{0}'
))", sel_barcode));
                        }

                        if (dtTempptype != null && dtTempptype.Rows.Count > 0)
                        {
                            string str_type_exp = dtTempptype.Rows[0]["type_exp"].ToString();
                            if (!string.IsNullOrEmpty(str_type_exp))
                            {
                                int int_type_exp = 0;
                                if (int.TryParse(str_type_exp, out int_type_exp))
                                {
                                    if (int_type_exp <= 9 && int_type_exp >= 0)
                                    {
                                        strPtypeSort = int_type_exp.ToString();
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    id_key = new Lib.DAC.GlobalSysTableIDGenerator().Generate(strYear, strCtypeSort + strPtypeSort, "");
                    for (int i = 0; id_key.Length < 7; i++)
                    {
                        id_key = "0" + id_key;
                    }

                    NewBarcode = strYear + strCtypeSort + strPtypeSort + id_key;
                    #endregion
                }
                else
                {
                    //条码生成规1：2位年份+2位分类+7位流水号+1位校验，共12位
                    #region 条码生成规1

                    string id_key = "";
                    string strYear = DateTime.Now.Year.ToString("D4").Substring(2, 2);//获取年份(后2位)
                    string strCtypeSort = "99";//2位分类(默认为99)

                    if (!string.IsNullOrEmpty(com_ctype) || !string.IsNullOrEmpty(sel_barcode))
                    {
                        DBHelper helper = new DBHelper();

                        //获取物理组信息（备注信息--作为分类）
                        DataTable dtTempctype = null;

                        if (!string.IsNullOrEmpty(com_ctype))
                        {
                            dtTempctype = helper.GetTable(string.Format("select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id='{0}'", com_ctype));
                        }
                        else if (!string.IsNullOrEmpty(sel_barcode))
                        {
                            dtTempctype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id=(
select top 1 bc_ctype from bc_patients with(NOLOCK) where bc_bar_no='{0}')", sel_barcode));
                        }

                        if (dtTempctype != null && dtTempctype.Rows.Count > 0)
                        {
                            string str_type_exp = dtTempctype.Rows[0]["type_exp"].ToString();
                            if (!string.IsNullOrEmpty(str_type_exp))
                            {
                                int int_type_exp = 99;
                                if (int.TryParse(str_type_exp, out int_type_exp))
                                {
                                    if (int_type_exp <= 99 && int_type_exp > 0)
                                    {
                                        strCtypeSort = int_type_exp.ToString("D2");
                                    }
                                }
                            }
                        }
                    }

                    id_key = new Lib.DAC.GlobalSysTableIDGenerator().Generate(strYear, strCtypeSort, "");
                    for (int i = 0; id_key.Length < 7; i++)
                    {
                        id_key = "0" + id_key;
                    }

                    string sSubCode = "";//按需要生成的编码长度，将传入的sCode进行截取
                    string sNcode = "";//返回带校验位的编码
                    int iSsum = 0;//单数位和
                    int iDsum = 0;//偶数位和
                    int iSum = 0;//总和
                    int iX = 0; //校验位

                    sSubCode = strYear + strCtypeSort + id_key;

                    //将字符串倒序组合
                    string sCodeGroup = "";
                    for (int i = sSubCode.Length; i > 0; i--)
                    {
                        sCodeGroup += sSubCode.Substring(i - 1, 1);
                    }
                    sSubCode = sCodeGroup;

                    //计算单数位 和偶数位的和
                    for (int i = 0; i < sSubCode.Length; i++)
                    {
                        if (i % 2 == 0)
                            iDsum += Convert.ToInt32(sSubCode.Substring(i, 1));  //偶数位和
                        if (i % 2 == 1)
                            iSsum += Convert.ToInt32(sSubCode.Substring(i, 1));//单数位和
                    }

                    //编码计算  偶数位和 *  3
                    iSum = iDsum * 3;
                    //偶数和*3 + 奇数位 和
                    iSum += iSsum;
                    //计算校验位,用10 减去 结果的个位数字
                    iX = 10 - (iSum % 10);

                    if (iX == 10) iX = 0;//个位数为0，则校验位为0

                    NewBarcode = strYear + strCtypeSort + id_key + iX.ToString();
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Logger.LogException("CreateBarcodeNumberForNhMode", ex);
                throw ex;
            }
            return NewBarcode;

        }

        private bool IsEmpty(object o)
        {
            return o == null || o.ToString() == "";
        }

        /// <summary>
        /// 插入条码数据
        /// </summary>
        /// <param name="downloadInfo"></param>
        /// <param name="dtCNameStructure"></param>
        /// <param name="dtBCPatientsForInsert"></param>
        private bool Insert(BarcodeDownloadInfo downloadInfo, DataTable dtCNameStructure, DataTable dtBCPatientsForInsert)
        {
            //Logger.WriteException("BarcodeBIZ.Insert", "", "dtBCPatientsForInsert.Count=" + dtBCPatientsForInsert.Rows.Count + ",dtCNameStructure.Count=" + dtCNameStructure.Rows.Count);

            bool logHisOriginData = false;

            //插入两个主表
            dtBCPatientsForInsert.TableName = BarcodeTable.Patient.TableName;
            dtCNameStructure.TableName = BarcodeTable.CName.TableName;

            //插入体检条码时需要区别对待，分批插入防止事务超时
            //if (downloadInfo.DownloadType == PrintType.TJ)
            //{
            #region MyRegion
            //按姓名排序
            DataView tempview_patient = dtBCPatientsForInsert.DefaultView;
            tempview_patient.Sort = "bc_name asc";
            DataTable table_patients = tempview_patient.ToTable();

            string pat_name = "-1";//临时变量

            //待插入数据的临时表
            DataTable tablePatientToInsert = table_patients.Clone();
            DataTable tableCNameToInsert = dtCNameStructure.Clone();

            //生成的插入bc_sign的sql语句
            ArrayList arrSQL_bcsign = new ArrayList();



            //遍历每一个条码
            foreach (DataRow rowPatient in table_patients.Rows)
            {
                if (pat_name != rowPatient["bc_name"].ToString())//如果当前病人姓名跟前一个不相同，则插入本批数据
                {
                    pat_name = rowPatient["bc_name"].ToString();

                    DataTable tablePatientToInsertBak = tablePatientToInsert.Copy();

                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("7") == "预置条码" && tablePatientToInsertBak.Columns.Contains("bc_bar_code") && downloadInfo.IsPrePlaceBarcode)
                        tablePatientToInsertBak.Columns.Remove("bc_bar_code");

                    ArrayList sql = dao.GetInsertSql(tablePatientToInsertBak);
                    if (sql.Count > 0)
                    {
                        if (tableCNameToInsert.Select("bc_yz_id is null or bc_yz_id = ''").Length == 0)
                        {
                            ArrayList sql2 = dao.GetInsertSql(tableCNameToInsert);
                            sql.AddRange(sql2);
                            sql.AddRange(arrSQL_bcsign);

                            int[] results2 = dao.DoTran(sql);
                        }
                        else
                        {
                            logHisOriginData = true;
                            //存在医嘱id为空
                        }
                    }

                    //清空临时表数据
                    tablePatientToInsert.Clear();
                    tableCNameToInsert.Clear();
                    arrSQL_bcsign.Clear();
                }


                //设置当前条码状态为"生成条码"
                rowPatient["bc_status"] = EnumBarcodeOperationCode.BarcodeGenerate.ToString();
                tablePatientToInsert.Rows.Add(rowPatient.ItemArray);

                string bc_bar_code = rowPatient["bc_bar_code"].ToString();

                //Logger.WriteException("BarcodeBIZ.Insert", "dtCNameStructure.Count,1419行", dtCNameStructure.Rows.Count.ToString());

                //foreach (DataRow dr_temp in dtCNameStructure.Rows)
                //{
                //    Logger.WriteException("BarcodeBIZ.Insert", "dtCNameStructure.Rows", dr_temp["bc_bar_code"].ToString());
                //}

                DataRow[] rowsBcCname = dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                //Logger.WriteException("BarcodeBIZ.Insert", "rowsBcCname.Count", rowsBcCname.Length.ToString());

                //根据条码号查找出bc_cname对应的条码数据，并添加到待插入数据临时表
                foreach (DataRow rowBcCname in rowsBcCname)
                {
                    tableCNameToInsert.Rows.Add(rowBcCname.ItemArray);
                }


                //生成bc_sign插入数据
                string sqlInsertBcSign = GenerateBcSignInsertSQL(Convert.ToDateTime(rowPatient["bc_date"])
                                            , rowPatient["bc_status"].ToString()
                                            , rowPatient["bc_bar_no"].ToString()
                                            , rowPatient["bc_bar_code"].ToString()
                                            , downloadInfo.OperationName);

                arrSQL_bcsign.Add(sqlInsertBcSign);
            }

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("7") == "预置条码" && tablePatientToInsert.Columns.Contains("bc_bar_code") && downloadInfo.IsPrePlaceBarcode)
                tablePatientToInsert.Columns.Remove("bc_bar_code");


            if (CacheSysConfig.Current.GetSystemConfig("BC_EnableBatchBarcode") == "是")
            {
                tablePatientToInsert.Columns.Add("bc_batch_barcode");
                string batchbarcode = new BCBarcodeBIZ().GetNewBatchBarcode();
                foreach (DataRow row in tablePatientToInsert.Rows)
                {
                    row["bc_batch_barcode"] = batchbarcode;
                }
            }

            ArrayList sql_ = dao.GetInsertSql(tablePatientToInsert);
            ArrayList sql2_ = dao.GetInsertSql(tableCNameToInsert);

            //Logger.WriteException("BarcodeBIZ.Insert", "tableCNameToInsert.Count", tableCNameToInsert.Rows.Count.ToString());


            string msg_temp2 = string.Empty;
            foreach (string item in sql2_)
            {
                msg_temp2 += item + Environment.NewLine;
            }
            //Logger.WriteException("BarcodeBIZ.Insert", "msg_temp2", msg_temp2);


            sql_.AddRange(sql2_);
            sql_.AddRange(arrSQL_bcsign);

            try
            {
                if (tableCNameToInsert.Select("bc_yz_id is null or bc_yz_id = ''").Length == 0)
                {
                    int[] results2_ = dao.DoTran(sql_);
                }
                else
                {
                    logHisOriginData = true;
                    //存在医嘱id为空
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(string.Format("插入条码数据，插入数据量：{0}条", sql_.Count), ex);
                throw;
            }
            #endregion
            //}
            //else
            //{
            //    #region MyRegion
            //    try
            //    {
            //        ArrayList arrInsertBcSign = new ArrayList();

            //        //added by lin : 2010/7/6
            //        //生成条码同时插入bc_sign记录
            //        //string insertBCSign = "insert into bc_sign(bc_date,bc_status,bc_bar_no,bc_bar_code,bc_flow) values('{0}','{1}','{2}','{3}',1)";
            //        foreach (DataRow rowBcPatient in dtBCPatientsForInsert.Rows)
            //        {
            //            rowBcPatient["bc_status"] = EnumBarcodeOperationCode.BarcodeGenerate.ToString();

            //            //string sqlInsertBcSign = string.Format(insertBCSign, rowBcPatient["bc_date"], rowBcPatient["bc_status"], rowBcPatient["bc_bar_no"], rowBcPatient["bc_bar_code"]);

            //            string sqlInsertBcSign = GenerateBcSignInsertSQL(Convert.ToDateTime(rowBcPatient["bc_date"]), rowBcPatient["bc_status"].ToString(), rowBcPatient["bc_bar_no"].ToString(), rowBcPatient["bc_bar_code"].ToString());

            //            arrInsertBcSign.Add(sqlInsertBcSign);
            //        }


            //        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("7") == "预置条码" && dtBCPatientsForInsert.Columns.Contains("bc_bar_code") && downloadInfo.IsPrePlaceBarcode)
            //            dtBCPatientsForInsert.Columns.Remove("bc_bar_code");
            //        ArrayList sql = dao.GetInsertSql(dtBCPatientsForInsert);
            //        ArrayList sql2 = dao.GetInsertSql(dtCNameStructure);
            //        sql.AddRange(sql2);
            //        sql.AddRange(arrInsertBcSign);

            //        int totalRowsAffact = -1;

            //        int[] results2 = dao.DoTran(sql);
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.WriteException(this.GetType().Name, "插入 非体检条码数据", ex.ToString());
            //        throw;
            //    } 
            //    #endregion
            //}
            return logHisOriginData;
        }


        public string HisConfirm(List<string> ListbarCode)
        {
            string strResult = string.Empty;

            StringBuilder sbBarcode = new StringBuilder();
            foreach (string strBarcode in ListbarCode)
            {
                sbBarcode.Append(string.Format(",'{0}'", strBarcode));
            }
            sbBarcode.Remove(0, 1);

            string strSql = string.Format("select * from bc_cname where bc_bar_no in ({0})", sbBarcode.ToString());

            DBHelper helper = new DBHelper();

            DataTable DtBcCname = helper.GetTable(strSql);


            SqlHelper Hishelper = new SqlHelper("Data Source=icare;user=icare;password=369icare", EnumDbDriver.Oracle, EnumDataBaseDialet.Oracle9i);

            foreach (DataRow drBcCname in DtBcCname.Rows)
            {
                string strYzId = drBcCname["bc_yz_id"].ToString();
                string strHisSQL = string.Format(@"select pstatus_int
                from lis_getyz a, t_opr_bih_register b
                where a.pat_zyh = b.inpatientid_chr
                and a.pat_yzh = '{0}' order by modify_dat desc", strYzId);

                DataTable dtHis = Hishelper.GetTable(strHisSQL);

                if (dtHis.Rows.Count > 0)
                {
                    string strStatus = dtHis.Rows[0]["pstatus_int"].ToString();
                    //if (strStatus == "0")
                    //{
                    //    strResult += "该病人已下床\r\n";
                    //}
                    //else if (strStatus == "2")
                    //{
                    //    strResult += "该病人已预出院\r\n";
                    //}
                    // else 
                    if (strStatus == "3")
                    {
                        strResult += "该病人已出院\r\n";
                    }
                    //else if (strStatus == "4")
                    //{
                    //    strResult += "该病人已请假\r\n";
                    //}
                    else
                    {
                        string str1 = "update lis_getyz t set t.pat_sf_flag = 1 where t.pat_yzh = '" + strYzId + "'";
                        string str2 = "update t_opr_bih_patientcharge t set t.pstatus_int = 1,chargeactive_dat=TO_DATE('" + DateTime.Now + "', 'yyyy-mm-dd hh24:mi:ss') where t.orderexecid_chr = '" + strYzId + "'";

                        Hishelper.ExecuteNonQuery(str1);
                        Hishelper.ExecuteNonQuery(str2);
                    }
                }
                else
                    strResult += "该住院医嘱不存在\r\n";
            }


            return strResult;
        }


        public int UpdateListingFlag(List<string> ListbarCode)
        {
            StringBuilder sbBarCode = new StringBuilder();
            foreach (string strBarcode in ListbarCode)
            {
                sbBarCode.Append(string.Format(",'{0}'", strBarcode));
            }
            sbBarCode.Remove(0, 1);

            String strSql = string.Format("update bc_patients set bc_listing_print_flag='1' where bc_bar_no in ({0})", sbBarCode.ToString());

            DBHelper helper = new DBHelper();

            return helper.ExecuteNonQuery(strSql);
        }

        public bool SaveColumnSort(string type, string sortvalue)
        {

            string flag = "select configid from sysconfig where configcode='{0}'";

            string strUpdate = "update sysconfig set configItemValue = '{0}' where configcode='{1}' ";

            string strInsert = @"if not exists(select configid from sysconfig where configcode='{0}')
                                 insert into sysconfig (configCode, configGroup, configItemName, configItemType, configItemValue,  configType)
                                 values ('{0}','条码','{1}','字符串','{2}','system')";
            try
            {
                if (type == "mz")
                {

                    DBHelper helper = new DBHelper();

                    DataTable dtSort = helper.GetTable(string.Format(flag, "Barcode_MzColumnVisiableIndex"));

                    if (dtSort != null && dtSort.Rows.Count > 0)
                        helper.ExecuteNonQuery(string.Format(strUpdate, sortvalue, "Barcode_MzColumnVisiableIndex"));
                    else
                        helper.ExecuteNonQuery(string.Format(strInsert, "Barcode_MzColumnVisiableIndex", "门诊条码打印界面自定义列顺序", sortvalue));

                    dcl.svr.cache.CacheSysConfig.Current.Refresh();
                    return true;

                }

            }
            catch
            {
                return false;
            }
            return false;
        }


        /// <summary>
        /// 生成插入bc_sign的sql语句
        /// </summary>
        /// <param name="bc_date"></param>
        /// <param name="bc_status"></param>
        /// <param name="bc_bar_no"></param>
        /// <param name="bc_bar_code"></param>
        /// <returns></returns>
        private string GenerateBcSignInsertSQL(DateTime bc_date, string bc_status, string bc_bar_no, string bc_bar_code, string bc_name)
        {
            string insertBCSign = "insert into bc_sign(bc_date,bc_status,bc_bar_no,bc_bar_code,bc_flow,bc_name) values('{0}','{1}','{2}','{3}',1,'{4}')";

            string sqlInsertBcSign = string.Format(insertBCSign, bc_date.ToString("yyyy-MM-dd HH:mm:ss"), bc_status, bc_bar_no, bc_bar_code, bc_name);

            return sqlInsertBcSign;
        }

        private DataRow SetOrderTime(string OrderTimeColumn, DataRow row, DataRow cnNameRow)
        {
            DateTime orderExecuteDate = new DateTime();
            if (DateTime.TryParse(row[OrderTimeColumn].ToString(), out orderExecuteDate))
                cnNameRow[BarcodeTable.CName.OrderRequireDate] = cnNameRow[BarcodeTable.CName.OrderExecuteDate] = orderExecuteDate;
            return cnNameRow;
        }

        /// <summary>
        /// 通过ID获取LIS组合
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Combines FindLisCombineByID(string p)
        {
            return bcDictBIZ.GetLisCombineByID(p);
        }


        //private IList<Combines> FindLisCombineByHISFeeCode(string hisFeeCode)
        //{
        //    IList<Combines> combines = new List<Combines>();
        //    Combines combine = bcDictBIZ.GetLisCombineByHISCode(hisFeeCode);
        //    if (combine.SplitFlag == 1)
        //    {
        //        combines = FindSubCombines(combine.Id);
        //    }
        //    else
        //    {
        //        combines.Add(combine);
        //    }

        //    return combines;
        //}

        private IList<BarcodeCombines> FindSubCombines(string combineID)
        {
            return bcDictBIZ.FindSubCombines(combineID);
        }

        private bool HasOrderIDInCombinesRecord(string orderID)
        {
            return ContainCombineRecode(orderID);
        }

        /// <summary>
        /// 根据HIS组合ID找条码组合
        /// </summary>
        /// <param name="downloadInfo">下载查询信息</param>
        /// <param name="hisCombineID">HIS组合ID</param>
        /// <returns></returns>
        private BarcodeCombines FindBarcodeCombinesByCombineID(BarcodeDownloadInfo downloadInfo, string CombineID)
        {
            BarcodeCombines combine = null;
            IDictionary dictionary = new System.Collections.Hashtable();
            dictionary["CombineId"] = CombineID;
            if (downloadInfo.DownloadType == PrintType.Inpatient) //住院：108 门诊：107
                dictionary["OriID"] = 108;
            else if (downloadInfo.DownloadType == PrintType.Outpatient)
                dictionary["OriID"] = 107;
            else if (downloadInfo.DownloadType == PrintType.TJ)
                dictionary["OriID"] = 109;

            IList<BarcodeCombines> combines = bcDictBIZ.SearchBarcodeCombineByHISID(dictionary);
            //有来源就用来源,没有就取默认的
            if (combines != null && combines.Count > 0)
            {
                foreach (BarcodeCombines com in combines)
                {
                    if (!string.IsNullOrEmpty(com.Oriid))
                        return com; //有来源就用来源

                    combine = com; //取默认
                }
            }
            return combine;
        }



        public IList<BarcodeCombinesRecodes> FindBarcodesBy(BarcodeCombinesRecodes findInfo)
        {
            IList<BarcodeCombinesRecodes> combinesRecodes = sqlHelper.SqlMapper.QueryForList<BarcodeCombinesRecodes>("FindBarcodeCombinesRecodesBy", findInfo);
            return combinesRecodes;
        }

        public int UpdateBCPatients(IDictionary param)
        {
            return sqlHelper.SqlMapper.Update("UpdateBarcodePatientsStatus", param);
        }

        public DataTable GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime)
        {
            BCCuvetteShelfBIZ biz = new BCCuvetteShelfBIZ();
            return biz.GetCuvetteRegisteredBarcodeInfo(deptid, depTime);
        }

        public DataTable GetCuvDetails(long st_id)
        {
            BCCuvetteShelfBIZ biz = new BCCuvetteShelfBIZ();
            return biz.GetCuvDetails(st_id);
        }

        public int SaveShelfBarcode(EntityBCStandTemp data)
        {
            BCCuvetteShelfBIZ biz = new BCCuvetteShelfBIZ();
            return biz.SaveShelfBarcode(data);
        }



        public int UpdateBarcodePatientsItemName(BarcodePatients bcPatients)
        {
            int result = 0;
            try
            {
                SqlHelper helper = new SqlHelper();
                string strSql = string.Format("UPDATE bc_patients SET bc_his_name ='{1}' WHERE bc_id ='{0}'", bcPatients.Id, bcPatients.HisName);

                result = helper.ExecuteNonQuery(strSql);
                //sqlHelper.SqlMapper.BeginTransaction();
                //result = sqlHelper.SqlMapper.Update("UpdateBarcodePatientsItemName", bcPatients);
                //sqlHelper.SqlMapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                //sqlHelper.SqlMapper.RollBackTransaction();
            }

            return result;
        }

        public int UpdateBarcodePatientsItemNameByBarcode(BarcodePatients bcPatients)
        {
            int result = 0;
            try
            {
                sqlHelper.SqlMapper.BeginTransaction();
                result = sqlHelper.SqlMapper.Update("UpdateBarcodePatientsItemNameByBarcode", bcPatients);
                sqlHelper.SqlMapper.CommitTransaction();
            }
            catch (Exception)
            {
                sqlHelper.SqlMapper.RollBackTransaction();
            }

            return result;
        }


        public EntityOperationResult DeleteShelfBarcode(long st_id)
        {
            BCCuvetteShelfBIZ biz = new BCCuvetteShelfBIZ();
            return biz.DeleteShelfBarcode(st_id);
        }



        public int UpdateBCPatientsUrgentFlag(bool urgent, IList<string> ids)
        {
            if (ids == null || ids.Count == 0)
                return 0;
            if (urgent)
                return sqlHelper.SqlMapper.Update("UpdateBCPatientsAddUrgentFlag", ids);
            else
                return sqlHelper.SqlMapper.Update("UpdateBCPatientsRemoveUrgentFlag", ids);
        }


        internal BarcodePatients FindBarcode(string barcodeID)
        {
            if (string.IsNullOrEmpty(barcodeID))
                return null;
            return sqlHelper.SqlMapper.QueryForObject<BarcodePatients>("SelectBarcodePatients", barcodeID);
        }

        internal BarcodeCombinesRecodes FindCName(string itemID)
        {
            if (string.IsNullOrEmpty(itemID))
                return null;
            return sqlHelper.SqlMapper.QueryForObject<BarcodeCombinesRecodes>("SelectBarcodeCombinesRecodes", itemID);
        }

        internal bool AddBarcodePatient(BarcodePatients bcPatient)
        {
            sqlHelper.SqlMapper.BeginTransaction();
            try
            {
                object object1 = sqlHelper.SqlMapper.Insert("InsertBarcodePatients", bcPatient);
                sqlHelper.SqlMapper.CommitTransaction();
                return object1 == null;
            }
            catch (Exception)
            {
                sqlHelper.SqlMapper.RollBackTransaction();
                return false;
            }

            return false;
        }

        public DateTime GetServerDateTime()
        {
            DateTime dtNow = DateTime.Now;
            IDaoSysLog dao = DclDaoFactory.DaoHandler<IDaoSysLog>();
            if (dao == null)
            {
                return dtNow;
            }
            else
            {
                return dao.GetDatabaseServerDateTime();
            }
        }

        public DataSet GetSignInfo(string barcode)
        {
            barcode = ToDBC(barcode);
            return sqlHelper.QueryForDataSet("SelectBarcodeSignRecodesReturnDataSet", barcode);
        }
        String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }


        public bool UpdateBCPatientsReturnFlag(BarcodePatients oneBarcode)
        {
            int result = sqlHelper.SqlMapper.Update("UpdateBCPatientsReturnFlag", oneBarcode);
            return result > 0;
        }

        public bool UpdateReturnMessage(ReturnMessages message)
        {
            message.BcTime = ServerDateTime.GetDatabaseServerDateTime();
            int result = sqlHelper.SqlMapper.Update("UpdateReturnMessages", message);
            return result > 0;
        }

        public bool AddReturnBarcode(ReturnMessages returnMessage)
        {
            //if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_Enable") == "开")
            //{
            //    string strBCLength = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_BarCodeLength");
            //    int CDRBarCodeLength = strBCLength == string.Empty ? 0 : Convert.ToInt32(strBCLength);

            //    if (returnMessage.BarcodeCode.Length == CDRBarCodeLength)
            //    {
            //        CDRService cdr = new CDRService();
            //        cdr.BarodeRefuse(returnMessage.BarcodeCode, returnMessage.SenderCode, returnMessage.SenderName, returnMessage.Message);
            //    }
            //}
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_PDABarCode") == "是")
            {
                Lis.SendDataToCDR.CDRService cdr = new Lis.SendDataToCDR.CDRService();
                List<string> listBarcode = new List<string>();
                listBarcode.Add(returnMessage.BarcodeCode);
                cdr.BackBarCode(listBarcode);
            }

            object obj = sqlHelper.SqlMapper.Insert("InsertReturnMessages", returnMessage);

            try
            {
                new TATRecordHelper().Clear(returnMessage.BarcodeCode);
            }
            catch
            { }
            return obj == null;
        }

        public int UpdateBCPatientsStatus(BarcodePatients bc)
        {
            int result = sqlHelper.SqlMapper.Update("UpdateBCPatientsStatus", bc);
            return result;
        }



        public int UpdateManyBCPatientsReturnFlag(IList<string> returnMessageId)
        {
            if (returnMessageId == null || returnMessageId.Count == 0)
                return -1;
            return sqlHelper.SqlMapper.Update("UpdateManyBCPatientsReturnFlag", returnMessageId);
        }

        public int UpdateReturnMessageFlag(IList<string> returnMessageId)
        {
            if (returnMessageId == null || returnMessageId.Count == 0)
                return -1;

            return sqlHelper.SqlMapper.Update("UpdateManyReturnMessagesFlag", returnMessageId);
        }

        public int UpdateBCPatientsStatusForSecondSend(List<string> barcodeNumbers)
        {
            if (barcodeNumbers == null || barcodeNumbers.Count == 0)
                return -1;

            return sqlHelper.SqlMapper.Update("UpdateBCPatientsStatusForSecondSend", barcodeNumbers);
        }

        public bool ChangeSampleRemark(BCSampleInfo changeInfo)
        {
            DBHelper helper = new DBHelper();
            int result = helper.ExecuteNonQuery(string.Format("update bc_patients set  bc_sam_rem_id = '{1}', bc_sam_rem_name = '{2}' where bc_bar_no = '{0}' and bc_del <> '1'", changeInfo.BarcodeID, changeInfo.SampleRemarkID, changeInfo.SampleRemarkName));

            return result > 0;
        }

        public int UpdateBCPatientsNameAndSex(BarcodePatients bc)
        {
            if (bc == null || bc.Id == null || bc.Id <= 0)
                return 0;
            return sqlHelper.SqlMapper.Update("UpdateBCPatientsNameAndSex", bc);
        }


        public int UpdateBCPatientsBarCode(string barCode, string prePlaceBarcode)
        {
            int result = -1;
            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int result1 = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_bar_no='{0}',bc_bar_code='{0}' where bc_bar_no='{1}'", prePlaceBarcode, barCode));
                    int result2 = helper.ExecuteNonQuery(string.Format("update bc_cname set bc_bar_no='{0}',bc_bar_code='{0}' where bc_bar_no='{1}'", prePlaceBarcode, barCode));
                    int result3 = helper.ExecuteNonQuery(string.Format("update bc_sign set bc_bar_no='{0}',bc_bar_code='{0}' where bc_bar_no='{1}'", prePlaceBarcode, barCode));
                    helper.Commit();
                    result = result1 + result2 + result3;
                    Logger.LogInfo("UpdateBCPatientsBarCode", string.Format("预置条码[{0}]，,影响bc_patients表{1}条数据,影响bc_cname表{2}条数据,被覆盖的自动条码（内部关联bc_bar_no）为[{3}]", prePlaceBarcode, result1, result2, barCode));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("UpdateBCPatientsBarCode[预置条码]", ex);
                throw;
            }

            return result;
        }


        public bool GetPrePlaceBarcode(string prePlaceBarcode)
        {
            DBHelper helper = new DBHelper();

            DataTable dtResulto = helper.GetTable(string.Format("select bc_bar_code from bc_patients where bc_bar_code='{0}' and bc_del='0'", prePlaceBarcode));

            return dtResulto.Rows.Count > 0;
        }


        public DataTable GetBarCodeStatus(string[] barcodes)
        {

            if (barcodes.Length == 2 && barcodes[0] == "S")
            {
                return GetBarCodeStatusWithPatid(barcodes[1]);
            }
            StringBuilder sbWhere = new StringBuilder();

            DataTable table;

            bool needComma = false;
            foreach (string barcode in barcodes)
            {
                if (string.IsNullOrEmpty(barcode))
                    continue;

                if (needComma)
                    sbWhere.Append(",");

                sbWhere.Append(string.Format("'{0}'", barcode));

                needComma = true;
            }

            if (sbWhere.Length > 0)
            {
                string sql = string.Format(@"select bc_bar_code,bc_bar_no
,isnull(bc_patients.bc_status,'0') as bc_status,bc_status.bc_cname as bc_statusname
,isnull(bc_blood_flag,0) as bc_blood_flag
,isnull(bc_send_flag,0) as bc_send_flag
,isnull(bc_reach_flag,0) as bc_reach_flag
,isnull(bc_receiver_flag,0) as bc_receiver_flag
,bc_reg_flag = isnull((select top 1 1 from bc_sign WITH(NOLOCK) where bc_sign.bc_bar_code = bc_patients.bc_bar_code and bc_sign.bc_bar_code<>'' and bc_sign.bc_bar_code is not null and bc_status = '20'),0)
,bc_patients.bc_ctype
from bc_patients left join bc_status on bc_status.bc_name=bc_patients.bc_status
where bc_bar_no in ({0})
order by bc_pid asc,bc_in_no asc
", sbWhere);
                table = new SqlHelper().GetTable(sql);
            }
            else
            {
                table = new DataTable();
                table.Columns.Add("bc_bar_code");
                table.Columns.Add("bc_bar_no");
                table.Columns.Add("bc_status");
                table.Columns.Add("bc_statusname");
            }
            table.TableName = "bc_patients";
            return table;
        }

        DataTable GetBarCodeStatusWithPatid(string barcodes)
        {


            DataTable table;



            string sql = string.Format(@"select bc_bar_code,bc_bar_no
,isnull(bc_patients.bc_status,'0') as bc_status,bc_status.bc_cname as bc_statusname
,isnull(bc_blood_flag,0) as bc_blood_flag
,isnull(bc_send_flag,0) as bc_send_flag
,isnull(bc_reach_flag,0) as bc_reach_flag
,isnull(bc_receiver_flag,0) as bc_receiver_flag,isnull(patients.pat_id,'') pat_id
,bc_reg_flag = isnull((select top 1 1 from bc_sign WITH(NOLOCK) where bc_sign.bc_bar_code = bc_patients.bc_bar_code and bc_sign.bc_bar_code<>'' and bc_sign.bc_bar_code is not null and bc_status = '20'),0)
from bc_patients left join bc_status on bc_status.bc_name=bc_patients.bc_status
LEFT JOIN patients ON patients.pat_bar_code=bc_patients.bc_bar_code
where bc_bar_no ='{0}'
order by bc_pid asc,bc_in_no asc
", barcodes);
            table = new SqlHelper().GetTable(sql);

            table.TableName = "bc_patients";
            return table;
        }


        public DataTable GetMergeBarCode(string barcode)
        {
            DBHelper helper = new DBHelper();

            string strSql = string.Format(@"
                            select bc_patients.bc_bar_code,bc_patients.bc_his_name,bc_patients.bc_status,bc_status.bc_cname from 
                            bc_cname
                            left join bc_patients on bc_cname.bc_bar_no=bc_patients.bc_bar_no 
                            left join bc_status on bc_patients.bc_status =bc_status.bc_name
                            where bc_cname.bc_yz_id in 
                            (select bc_yz_id from bc_cname left join bc_patients t1 on bc_cname.bc_bar_no=t1.bc_bar_no where bc_cname.bc_bar_code='{0}' and bc_cname.bc_yz_id is not null and t1.bc_no_id=bc_patients.bc_no_id)
                            and bc_cname.bc_bar_code <>'{0}' and bc_patients.bc_del='0'", barcode);

            DataTable dtMergeBarCode = helper.GetTable(strSql);
            dtMergeBarCode.TableName = "MergeBarCode";

            return dtMergeBarCode;
        }


        public string UndoBarcode(string strBarCode)
        {

            string newBarCode = string.Empty;
            try
            {
                //系统配置：条码号使用校验位
                string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                    {
                        //滨海模块-生成条码号
                        newBarCode = CreateBarcodeNumberForBhMode(null, strBarCode, null);
                    }
                    else
                    {
                        newBarCode = CreateBarcodeNumber();
                    }

                    int count = helper.ExecuteNonQuery(string.Format("update bc_patients set bc_bar_code='',bc_bar_no = '{1}',bc_status = '0',bc_print_flag = 0 where bc_bar_code='{0}' and bc_del='0'", strBarCode, newBarCode));
                    int count2 = helper.ExecuteNonQuery(string.Format("update bc_cname set bc_bar_no='{0}',bc_bar_code='{0}' where bc_bar_no='{1}'", newBarCode, strBarCode));
                    int count3 = helper.ExecuteNonQuery(string.Format("update bc_sign set bc_bar_no='{0}',bc_bar_code='{0}' where bc_bar_no='{1}'", newBarCode, strBarCode));
                    helper.Commit();
                    Logger.LogInfo("UndoBarcode", string.Format("重置条码[{0}]，,影响bc_patients表{1}条数据,影响bc_cname表{2}条数据,生成的自动条码（内部关联bc_bar_no）为[{3}]", strBarCode, count, count2, newBarCode));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("UndoBarcode", ex);
                newBarCode = string.Empty;
            }

            return newBarCode;
        }

        #region IBarcode 成员

        public DataTable GetPatientsInfoByBcInNo(string bc_in_no)
        {
            return new BCPatientBIZ().GetPatientsInfoByBcInNo(bc_in_no);
        }

        #endregion

        #region IBarcode 成员


        public int UpdatePrintStatus(List<string> lisBarCode)
        {
            StringBuilder sbBcId = new StringBuilder();

            foreach (var item in lisBarCode)
            {
                sbBcId.Append(string.Format(",'{0}'", item));
            }

            sbBcId.Remove(0, 1);

            string strUpdateWhere = string.Format("update bc_patients set bc_print_time='10' where bc_bar_no in ({0})", sbBcId.ToString());

            return new SqlHelper().ExecuteNonQuery(strUpdateWhere);
        }

        #endregion

        #region IBarcode 成员

        //20120920
        public EntityOperationResult insertBcsignShelfBarcode(string bc_login_id, string bc_name, string bc_bar_no, string bc_remark)
        {
            BCCuvetteShelfBIZ biz = new BCCuvetteShelfBIZ();
            return biz.insertBcsignShelfBarcode(bc_login_id, bc_name, bc_bar_no, bc_remark);
        }

        #endregion

        #region IBarcode 成员


        public int GetBarcodefrequency()
        {
            int intResCurID = 0;
            string strDateNow = DateTime.Now.ToString("yyyyMMdd");
            string strSelect = "select curID from SysTableID where tablename ='Barcodefrequency'";
            SqlHelper helper = new SqlHelper();
            DataTable dtbSystableId = helper.GetTable(strSelect);
            if (dtbSystableId != null && dtbSystableId.Rows.Count > 0)
            {
                int intCurID = Convert.ToInt32(dtbSystableId.Rows[0]["curID"]);
                if (intCurID.ToString().IndexOf(strDateNow) >= 0)
                {
                    intResCurID = intCurID + 1;
                }
                else
                {
                    intResCurID = Convert.ToInt32(strDateNow + "1");
                }
                string strUpdate = "update SysTableID set curID=  " + intResCurID.ToString() + " where tablename ='Barcodefrequency'";
                helper.ExecuteNonQuery(strUpdate);
            }

            return intResCurID;
        }

        #endregion

        #region IBarcode 成员

        //肿瘤医院条码签收时进行费用判断
        public string ZLYYCheckHISStatus(string barcode, string ori_ID)
        {
            return ZLYYBarCheckHISStatus.ZLYYCheckHISStatus(barcode, ori_ID);
        }

        #endregion

        #region IBarcode 成员

        /// <summary>
        /// 當回退條碼時，需要將條碼的採集、收取、送達、簽收等時間置為NULL
        /// </summary>
        /// <returns></returns>
        public bool setTimeToNullOnRecturn(string barcode)
        {
            string sql = string.Format(@"update bc_patients set bc_blood_date = null,
             bc_send_date = null,bc_reach_date= null,bc_receiver_date = null
            where bc_id = {0}", barcode);

            try
            {
                new SqlHelper().ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException("條碼回退更改時間出錯", ex);
                return false;
            }
        }
        /// <summary>
        /// 条码回退后删除条码的上机标志
        /// </summary>
        /// <param name="returnBarcode"></param>
        /// <returns></returns>
        public int UpdateBarcodeModifyFlagOnRecturn(string returnBarcode)
        {
            string sql = string.Format(@"update bc_cname set  bc_modify_flag = 0 where bc_bar_code = '{0}'", returnBarcode);

            try
            {
                return new SqlHelper().ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Logger.LogException("条码回退后删除条码的上机标志", ex);
            }
            return -1;
        }
        #endregion

        #region IBarcode 成员


        /// <summary>
        /// 根据条码号获取已登记的组合
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        /// 已增加，在PidReportMainBIZ
        public DataTable getIsSignCombineByBC(string barcode)
        {
            string sql = string.Format(@"
select bc_cname.bc_lis_code,bc_cname.bc_bar_code,dict_instrmt.itr_name,
patients.pat_itr_id,patients.pat_i_code,poweruserinfo.userName,bc_cname.bc_name,
patients.pat_date,patients.pat_sid
from patients 
left join bc_cname on patients.pat_bar_code = bc_cname.bc_bar_code and bc_cname.bc_name = patients.pat_c_name
left join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
left join poweruserinfo on poweruserinfo.loginId = patients.pat_i_code
where bc_cname.bc_bar_no = '{0}' and bc_cname.bc_flag = 1", barcode);
            try
            {
                DataTable result = new DBHelper().GetTable(sql);
                result.TableName = "CombineName";
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException("获取条码已录入组合失败，getIsSignCombineByBC.BarcodeBIZ", ex);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 根据bc_in_no获取条码信息
        /// </summary>
        /// <param name="ori_id"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="bc_in_no"></param>
        /// <returns></returns>
        public DataTable GetBarcodeData_WithQueryField(string ori_id, string queryField, DateTime dateBegin, DateTime dateEnd, string bc_in_no)
        {
            string sql = string.Format(@"
select
    bc_patients.bc_id,
    bc_patients.bc_notice,
	bc_patients.bc_bar_no,
	bc_patients.bc_bar_code,
	bc_patients.bc_no_id,
	bc_patients.bc_in_no,
	bc_patients.bc_bed_no,
	bc_patients.bc_name,
	bc_patients.bc_sex,
    bc_patients.bc_status,
    bc_patients.bc_ctype,
	case bc_patients.bc_sex when '1' then '男'
	                        when '2' then '女'
	                        else '' end as bc_sex_name,
	bc_patients.bc_age,
    dbo.getAge(bc_patients.bc_age) barcode_age,
    bc_patients.bc_diag,
    bc_patients.bc_doct_code,
    bc_patients.bc_doct_name,
    bc_patients.bc_his_name,
    bc_patients.bc_sam_name,
    isnull(dict_type.[type_name],'') barcode_type_name,
    dict_origin.ori_name barcode_ori_name,
    dict_sample.sam_name barcode_sam_name,
    (case isnull(bc_patients.bc_exp,'') when '' then isnull(dict_sample_remarks.rem_cont,'') else bc_patients.bc_exp end ) barcode_sample_remarks,
    isnull(dict_cuvette.cuv_name,'') barcode_cuv_name,
    bc_patients.bc_exp,
	bc_patients.bc_d_name
from bc_patients  with(nolock)
    LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id LEFT OUTER JOIN 
    dict_origin on bc_patients.bc_ori_id = dict_origin.ori_id LEFT OUTER JOIN  
    dict_cuvette ON bc_patients.bc_cuv_code = dict_cuvette.cuv_code  LEFT OUTER JOIN 
    dict_sample ON bc_patients.bc_sam_id = dict_sample.sam_id LEFT OUTER JOIN
    dict_sample_remarks ON bc_patients.bc_sam_rem_id = dict_sample_remarks.rem_id 
 WHERE  ( bc_occ_date BETWEEN ? AND ? )
 AND  {0} = ?  AND  bc_del <> '1'  
 ORDER BY bc_status ", queryField);

            SqlHelper helper = new SqlHelper();
            DbCommandEx cmd = helper.CreateCommandEx(sql);
            cmd.AddParameterValue(dateBegin);
            cmd.AddParameterValue(dateEnd);
            cmd.AddParameterValue(bc_in_no, DbType.AnsiString);
            DataTable table = helper.GetTable(cmd);
            table.TableName = "bc_patients";
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bc_bar_code"></param>
        /// <param name="opCode"></param>
        /// <param name="opName"></param>
        /// <param name="opPlace"></param>
        /// <param name="bc_status"></param>
        public void UpdateBarcodeStatusWithSign(
            long bc_id,
            string opCode,
            string opName,
            string opPlace,
            string bc_status)
        {
            string sql = @"
declare @bc_bar_code varchar(20)
set @bc_bar_code = (select top 1 bc_bar_code from bc_patients where bc_id = ?1)

update bc_patients set bc_status = ?2,bc_lastaction_time=?3 where bc_id = ?1

insert into bc_sign(bc_date, bc_login_id, bc_name,  bc_status, bc_bar_no, bc_bar_code, bc_place)
values(?3,?4,?5,?2,@bc_bar_code,@bc_bar_code,?6)
";
            SqlHelper helper = new SqlHelper();
            DbCommandEx2 cmd = helper.CreateCommandEx2(sql);
            cmd.AddParameterValue(1, bc_id, DbType.Int64);
            cmd.AddParameterValue(2, bc_status, DbType.AnsiString);
            cmd.AddParameterValue(3, ServerDateTime.GetDatabaseServerDateTime());
            cmd.AddParameterValue(4, opCode, DbType.AnsiString);
            cmd.AddParameterValue(5, opName, DbType.AnsiString);
            cmd.AddParameterValue(6, opPlace, DbType.AnsiString);
            helper.ExecuteNonQuery(cmd);
        }

        #region IBarcode 成员

        /// <summary>
        /// 获取平台条码
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns>0-无，1-成功</returns>
        public int GetPlatfromBarcode(string barcode)
        {
            string strSql = string.Format(@"select bc_bar_no, bc_bar_code,  bc_no_id, bc_in_no,  bc_name, bc_sex, bc_age,  bc_d_name, bc_diag, bc_doct_code,
                             bc_doct_name, bc_his_name, bc_sam_id, bc_sam_name, bc_date, bc_times, bc_print_flag, bc_print_date,bc_status ,
                             bc_status_cname,bc_lastaction_time,bc_del,bc_ori_id,bc_cuv_code
                             from bc_patients where bc_bar_no='{0}'", barcode);

            DBHelper db = new DBHelper();
            DataTable dtPatient = db.GetTable(strSql);
            dtPatient.TableName = "bc_patients";
            if (dtPatient.Rows.Count > 0)
                return 1;

            string strInterface = "select * from dict_interfaces where in_interface_type='平台条码'";

            DataTable dtInterface = db.GetTable(strInterface);
            foreach (DataRow item in dtInterface.Rows)
            {
                if (!item["in_db_address"].ToString().Contains(".") && item["in_db_name"].ToString().Length >= 12)
                {
                    item["in_db_address"] = EncryptClass.Decrypt(item["in_db_address"].ToString());
                    item["in_db_name"] = EncryptClass.Decrypt(item["in_db_name"].ToString());
                    item["in_db_username"] = EncryptClass.Decrypt(item["in_db_username"].ToString());
                    item["in_db_password"] = EncryptClass.Decrypt(item["in_db_password"].ToString());
                }
            }
            if (dtInterface.Rows.Count > 0)
            {
                DataRow drInterface = dtInterface.Rows[0];
                HospitalInterface interfaces = new HospitalInterface(
                     drInterface[BarcodeTable.Interfaces.DBAddress].ToString(),
                     drInterface[BarcodeTable.Interfaces.DBName].ToString(),
                     drInterface[BarcodeTable.Interfaces.DBUsername].ToString(),
                     drInterface[BarcodeTable.Interfaces.DBPassword].ToString(),
                     drInterface[BarcodeTable.Interfaces.DBConnnectType].ToString(),
                     drInterface[BarcodeTable.Interfaces.InterfaceName].ToString());

                DataSet dataset = interfaces.Connecter.ExeInterface(barcode);

                if (dataset.Tables.Count > 0)
                {
                    DataTable dtResulto = dataset.Tables[0];

                    DataRow drResulto = dtResulto.Rows[0];

                    DataTable dtBcPatients = dtPatient.Clone();

                    DataRow drBcPatients = dtBcPatients.NewRow();

                    drBcPatients["bc_bar_code"] = drResulto["bc_bar_code"];
                    drBcPatients["bc_bar_no"] = drResulto["bc_bar_no"];
                    drBcPatients["bc_in_no"] = drResulto["bc_in_no"];
                    drBcPatients["bc_name"] = drResulto["bc_name"];
                    drBcPatients["bc_sex"] = drResulto["bc_sex"];

                    if (!Compare.IsNullOrDBNull(drResulto["bc_age"]))
                    {//目前只截取年
                        string age = drResulto["bc_age"].ToString();

                        if (age.ToLower().Contains('y')
                        && age.ToLower().Contains('m')
                        && age.ToLower().Contains('d')
                        && age.ToLower().Contains('h')
                        && age.ToLower().Contains('i')
                        )
                        {
                            drBcPatients["bc_age"] = age;
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
                            drBcPatients["bc_age"] = age;
                        }

                    }

                    drBcPatients["bc_d_name"] = drResulto["bc_d_name"];
                    drBcPatients["bc_diag"] = drResulto["bc_diag"];
                    drBcPatients["bc_doct_code"] = drResulto["bc_doct_code"];
                    drBcPatients["bc_doct_name"] = drResulto["bc_doct_name"];
                    drBcPatients["bc_his_name"] = drResulto["bc_c_name"];
                    drBcPatients["bc_sam_name"] = drResulto["bc_sam_name"];
                    drBcPatients["bc_date"] = drResulto["bc_date"];
                    drBcPatients["bc_times"] = drResulto["bc_times"];
                    drBcPatients["bc_print_date"] = drResulto["bc_print_date"];
                    drBcPatients["bc_print_flag"] = 1;
                    drBcPatients["bc_status"] = 1;
                    drBcPatients["bc_status_cname"] = "已打印";
                    drBcPatients["bc_lastaction_time"] = drResulto["bc_print_date"];
                    drBcPatients["bc_del"] = 0;
                    drBcPatients["bc_ori_id"] = "110";
                    switch (drResulto["bc_ori_name"].ToString())
                    {
                        case "HIS_MZ":
                            drBcPatients["bc_no_id"] = "107";
                            break;
                        case "HIS_TJ":
                            drBcPatients["bc_no_id"] = "106";
                            break;
                        case "HIS_ZY":
                            drBcPatients["bc_no_id"] = "110";
                            break;
                        default:
                            break;
                    }


                    string sqlCName = @"select bc_bar_code,bc_bar_no,bc_his_name,bc_his_code,bc_yz_id,bc_flag,bc_apply_date,bc_occ_date,bc_del,bc_lis_code,bc_name 
                                       from bc_cname where bc_id='-1'";
                    DataTable dtCName = db.GetTable(sqlCName).Clone();
                    dtCName.TableName = "bc_cname";

                    foreach (DataRow drResult in dtResulto.Rows)
                    {
                        DataRow drCName = dtCName.NewRow();
                        drCName["bc_bar_code"] = drResult["bc_bar_code"];
                        drCName["bc_bar_no"] = drResult["bc_bar_no"];
                        drCName["bc_his_name"] = drResult["bc_c_name"];
                        drCName["bc_his_code"] = drResult["bc_c_code"];
                        drCName["bc_yz_id"] = drResult["bc_yz_id"];
                        drCName["bc_flag"] = 0;
                        drCName["bc_apply_date"] = drResult["bc_apply_date"];
                        drCName["bc_occ_date"] = drResult["bc_occ_date"];
                        drCName["bc_del"] = 0;

                        string SQlCombine = string.Format(@"select dict_combine_bar.*,dict_combine.com_name from dict_combine_bar 
                                                            left join dict_combine on dict_combine_bar.com_id=dict_combine.com_id
                                                            where dict_combine_bar.com_his_fee_code='{0}' and com_ori_id='10003' ", drResult["bc_c_code"]);

                        DataTable dtCombine = db.GetTable(SQlCombine);
                        if (dtCombine.Rows.Count > 0)
                        {
                            DataRow drCombine = dtCombine.Rows[0];
                            drCName["bc_lis_code"] = drCombine["com_id"];
                            drCName["bc_name"] = drCombine["com_name"];
                            drBcPatients["bc_cuv_code"] = drCombine["com_cuv_code"];
                            drBcPatients["bc_sam_id"] = drCombine["com_sam_id"];
                        }
                        dtCName.Rows.Add(drCName);
                    }
                    dtBcPatients.Rows.Add(drBcPatients);

                    DataTable dt = new DataTable("bc_sign");
                    dt.Columns.Add("bc_date");
                    dt.Columns.Add("bc_status");
                    dt.Columns.Add("bc_bar_no");
                    dt.Columns.Add("bc_bar_code");
                    dt.Columns.Add("bc_remark");

                    dt.Rows.Add(drResulto["bc_date"], "1", drResulto["bc_bar_no"], drResulto["bc_bar_code"], drResulto["bc_computer"]);

                    DbBase dao = DbBase.InConn();

                    ArrayList lisInsert = dao.GetInsertSql(dtBcPatients);
                    lisInsert.AddRange(dao.GetInsertSql(dtCName));
                    lisInsert.AddRange(dao.GetInsertSql(dt));

                    dao.DoTran(lisInsert);
                    dtBcPatients.Clear();
                    dtCName.Clear();
                    dt.Clear();
                }
            }


            return 0;
        }


        /// <summary>
        /// 获取分院条码
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns>0-无，1-成功</returns>
        public int GetChildHospitalBarcode(string barcode)
        {
            try
            {
                string strSql = string.Format(@"select *
                             from bc_patients where bc_bar_no='{0}' ", barcode);

                DBHelper db = new DBHelper();
                DataTable dtPatient = db.GetTable(strSql);
                dtPatient.TableName = "bc_patients";
                if (dtPatient.Rows.Count > 0)
                    return 1;

                string childHosConnectStr = ConfigurationManager.AppSettings["ChildHosDBConnectStr"];
                if (string.IsNullOrEmpty(childHosConnectStr))
                {
                    Logger.LogException(new Exception("请在中间层Webconfig 添加分院数据连接字符串[ChildHosDBConnectStr]"));
                    return 0;
                }

                DBHelper dbChild = new DBHelper(childHosConnectStr);

                strSql = string.Format(@"select *
                             from bc_patients where bc_bar_no='{0}'   ", barcode);

                DataTable dtBcPatients = dbChild.GetTable(strSql);
                dtBcPatients.TableName = "bc_patients";
                if (dtBcPatients.Rows.Count > 0)
                {
                    strSql = string.Format(@"select *
                             from bc_cname where bc_bar_no='{0}'   ", barcode);

                    DataTable dtCName = dbChild.GetTable(strSql);
                    dtCName.TableName = "bc_cname";

                    strSql = string.Format(@"select *
                             from bc_sign where bc_bar_no='{0}'   ", barcode);

                    DataTable dt = dbChild.GetTable(strSql);
                    dt.TableName = "bc_sign";
                    DbBase dao = DbBase.InConn();


                    dtBcPatients.Columns.Remove("bc_id");
                    dtCName.Columns.Remove("bc_id");
                    dt.Columns.Remove("bc_id");
                    ArrayList lisInsert = dao.GetInsertSql(dtBcPatients);
                    lisInsert.AddRange(dao.GetInsertSql(dtCName));
                    lisInsert.AddRange(dao.GetInsertSql(dt));

                    dao.DoTran(lisInsert);
                    dtBcPatients.Clear();
                    dtCName.Clear();
                    dt.Clear();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return 0;

        }


        public int ChildHospitalBCConfirm(string barcode, string operatorID, string operatorName)
        {
            try
            {
                if (string.IsNullOrEmpty(barcode)) return 0;
                string childHosConnectStr = ConfigurationManager.AppSettings["ChildHosDBConnectStr"];
                if (string.IsNullOrEmpty(childHosConnectStr))
                {
                    Logger.LogException(new Exception("请在中间层Webconfig 添加分院数据连接字符串[ChildHosDBConnectStr]"));
                    return 0;
                }

                DBHelper dbChild = new DBHelper(childHosConnectStr);
                string strSql = string.Format(@" UPDATE bc_patients set bc_status='5',bc_status_cname='已签收',bc_exp=isnull(bc_exp,'')+' 总院已签收' where bc_bar_no='{0}' and bc_del<>'1' and  bc_status not in ('5','20','40','50','60')  ", barcode);


                if (dbChild.ExecuteNonQuery(strSql) > 0)
                {
                    string sql =
                        string.Format(@"insert into bc_sign( bc_date, bc_login_id, bc_name, bc_status, bc_bar_no, bc_bar_code, bc_place, bc_flow, bc_remark, pat_id)
                                        values('{0}','{1}','{2}','5','{3}','{3}','',1,'{4}','')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), operatorID,
                                      operatorName, barcode, "总院已签收");
                    dbChild.ExecuteNonQuery(sql);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return 0;

        }

        #endregion

        /// <summary>
        /// 利用DataInterfaceHelper获取Datatable
        /// 
        /// 输入指定的命令ID获取获取Datatable
        /// </summary>
        /// <param name="cmd_id">命令ID</param>
        /// <param name="dataBindings">绑定项目</param>
        /// <returns></returns>
        [System.ComponentModel.Description("利用DataInterfaceHelper获取Datatable")]
        public DataTable GetDtByDataInterfaceHelper(string cmd_id, Lib.DataInterface.Implement.InterfaceDataBindingItem[] dataBindings)
        {
            try
            {
                //创建数据接口对象
                DataInterfaceHelper helper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, false);
                //定义参数集合
                //List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                //list.Add(new InterfaceDataBindingItem("bc_in_no", "0"));
                DataTable dttemp = helper.GetDataTable(cmd_id, dataBindings);
                if (dttemp != null) dttemp.TableName = "dt";
                return dttemp;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("利用DataInterfaceHelper获取Datatable", ex);
                throw ex;
            }
            return null;
        }

        public DataTable GetTimeRuleWaring(string barcode, string pat_id, string bc_status)
        {
            DataTable ret = new DataTable("timerule");
            if (!string.IsNullOrEmpty(barcode) && bc_status == "5")
            {
                string sql = @"select msg_tat.*
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_d_name,
bc_cname.bc_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
patients.pat_itr_id,
dict_instrmt.itr_mid,
bc_patients.bc_ctype, --物理组 
dict_type.type_name,
bs1.bc_cname+' 至 '+bs2.bc_cname as flowName,
dict_combine_timerule.com_time as time_tat,
(CASE WHEN dict_combine_timerule.com_time_start_type='0' AND bc_patients.bc_date IS NOT null  THEN bc_patients.bc_date
WHEN dict_combine_timerule.com_time_start_type='1' AND bc_patients.bc_print_date IS NOT null  THEN bc_patients.bc_print_date
WHEN dict_combine_timerule.com_time_start_type='2' AND bc_patients.bc_blood_date IS NOT null  THEN bc_patients.bc_blood_date
WHEN dict_combine_timerule.com_time_start_type='3' AND bc_patients.bc_send_date IS NOT null  THEN bc_patients.bc_send_date
WHEN dict_combine_timerule.com_time_start_type='4' AND bc_patients.bc_reach_date IS NOT null  THEN bc_patients.bc_reach_date ELSE getdate() END) AS FromDate,bc_receiver_date,
Datediff(mi,(CASE WHEN dict_combine_timerule.com_time_start_type='0' AND bc_patients.bc_date IS NOT null  THEN bc_patients.bc_date
WHEN dict_combine_timerule.com_time_start_type='1' AND bc_patients.bc_print_date IS NOT null  THEN bc_patients.bc_print_date
WHEN dict_combine_timerule.com_time_start_type='2' AND bc_patients.bc_blood_date IS NOT null  THEN bc_patients.bc_blood_date
WHEN dict_combine_timerule.com_time_start_type='3' AND bc_patients.bc_send_date IS NOT null  THEN bc_patients.bc_send_date
WHEN dict_combine_timerule.com_time_start_type='4' AND bc_patients.bc_reach_date IS NOT null  THEN bc_patients.bc_reach_date ELSE getdate() END),getdate()) as time_mi
from bc_patients with(nolock)
left join patients on bc_patients.bc_bar_code=patients.pat_bar_code
inner join bc_cname on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id AND 
(bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='')
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on bc_patients.bc_ctype=dict_type.type_id
LEFT JOIN bc_status bs1 ON bs1.bc_name = dict_combine_timerule.com_time_start_type
LEFT JOIN bc_status bs2 ON bs2.bc_name = dict_combine_timerule.com_time_end_type
where bc_patients.bc_bar_code='{0}'
and bc_cname.bc_del='0' and bc_patients.bc_del='0'
--and dict_combine_timerule.com_time_start_type='5'
and dict_combine_timerule.com_time_end_type='5'
and dict_combine_timerule.com_time>0
and bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id
and (patients.pat_flag is null or patients.pat_flag=0 or patients.pat_flag=1) AND 
Datediff(mi,(CASE WHEN dict_combine_timerule.com_time_start_type='0' AND bc_patients.bc_date IS NOT null  THEN bc_patients.bc_date
WHEN dict_combine_timerule.com_time_start_type='1' AND bc_patients.bc_print_date IS NOT null  THEN bc_patients.bc_print_date
WHEN dict_combine_timerule.com_time_start_type='2' AND bc_patients.bc_blood_date IS NOT null  THEN bc_patients.bc_blood_date
WHEN dict_combine_timerule.com_time_start_type='3' AND bc_patients.bc_send_date IS NOT null  THEN bc_patients.bc_send_date
WHEN dict_combine_timerule.com_time_start_type='4' AND bc_patients.bc_reach_date IS NOT null  THEN bc_patients.bc_reach_date ELSE getdate() END),getdate())>dict_combine_timerule.com_time

) as msg_tat";

                SqlHelper helper = new SqlHelper();
                ret = helper.GetTable(string.Format(sql, barcode), "timerule");
            }






            return ret;
        }


        public int SaveBarcodeBale(DataTable dtBarCode)
        {
            int res = 0;
            string strSql = "update bc_patients set bc_upid='{0}' where bc_bar_code in ({1})";

            SqlHelper helper = new SqlHelper();

            foreach (DataRow item in dtBarCode.Rows)
            {
                res += helper.ExecuteNonQuery(string.Format(strSql, item["value"].ToString(), item["where"].ToString()));
            }
            return res;
        }
        /// <summary>
        /// 签收时下载条码(深圳市一)
        /// </summary>
        /// <param name="downloadInfo">PatientID ：条码号; DownloadType: 对应来源(未选择来源对应值为PrintType.Manual) </param>
        /// <returns></returns>
        public DataSet DownloadBarcodeFromHISForSZ(BarcodeDownloadInfo downloadInfo)
        {
            try
            {
                StringBuilder debugMsg = new StringBuilder();

                System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();
                watch2.Start();
                //日志
                if (showLogger)
                {
                    watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                }

                string type = downloadInfo.GetDownloadTypeId();

                DBHelper helper = new DBHelper();
                DataSet dsHISData = null;

                string barcode = downloadInfo.PatientID;
                //获取条码表与明细表结构
                DataTable dtPatientsStructure = helper.GetTable(string.Format(@"select bc_bar_no,bc_bar_code,bc_frequency,bc_no_id,bc_in_no
,bc_bed_no,bc_name,bc_sex,bc_age,bc_d_code,bc_d_name,bc_diag,bc_doct_code,bc_doct_name,bc_his_name
,bc_sam_id,bc_sam_name,bc_date,bc_times,bc_cuv_code,bc_cuv_name,bc_cap_sum,bc_cap_unit,bc_urgent_flag
,bc_ctype,bc_ori_id,bc_ori_name,bc_exp,bc_computer,bc_social_no,bc_emp_id,bc_info,bc_hospital_id,bc_bar_type
,bc_status,bc_status_cname,bc_birthday,bc_blood,bc_blood_date,bc_blood_flag,bc_blood_name,bc_blood_code
,bc_print_flag,bc_print_date,bc_print_code,bc_print_name,bc_send_flag,bc_send_date,bc_send_code,bc_send_name
,bc_reach_flag,bc_reach_date,bc_reach_code,bc_reach_name,bc_receiver_flag,bc_receiver_date,bc_receiver_code
,bc_receiver_name,bc_occ_date,bc_sam_rem_id,bc_sam_rem_name,bc_print_time,bc_return_times,bc_lastaction_time
,bc_address,bc_tel,bc_pid,bc_emp_company,bc_app_no,bc_sam_dest,bc_fee_type,bc_emp_company_name
,bc_emp_company_dept,bc_oldlis_barcode,bc_notice,'' as bc_gourp_id,'' as bc_group_id,'' as bc_upid,'' as bc_merge_comid
from bc_patients with(nolock) where bc_bar_no='{0}' and bc_del<>1 ", barcode));

                if (dtPatientsStructure.Rows.Count > 0)
                {
                    BCPatientBIZ bllBCPatient = new BCPatientBIZ();
                    DataSet dsBarCodePatient = bllBCPatient.SearchByBarcode(barcode);
                    return dsBarCodePatient;
                }

                Logger.LogInfo("查询条码", "barcode： " + barcode + ";" + watch2.ElapsedMilliseconds.ToString());
                DataTable dtCNameStructure = helper.GetTable("select bc_bar_no, bc_bar_code, bc_frequency, bc_his_name, bc_his_code, bc_yz_id,bc_yz_id2, bc_ctype, bc_flag, bc_date, bc_apply_date, bc_occ_date, bc_item_no, bc_confirm_flag, bc_confirm_code, bc_confirm_name, bc_price, bc_unit, bc_modify_flag, bc_modify_date, bc_itr_id, bc_view_flag, bc_exec_code, bc_exec_name, bc_out_time, bc_rep_flag, bc_rep_date, bc_enrol_flag, bc_other_no, bc_lis_code,bc_name,bc_blood_notice,bc_save_notice from bc_cname where 1<>1");
                //                DataTable dtCombineSeq = helper.GetTable(string.Format(@"SELECT dict_combine_bar.com_his_fee_code,com_name,dict_combine.com_seq
                //                                                        FROM dict_combine_bar
                //                                                        LEFT JOIN dict_combine on dict_combine.com_id = dict_combine_bar.com_id
                //                                                        WHERE dict_combine_bar.com_ori_id='{0}' and
                //                                                        (dict_combine.com_del = '0' or dict_combine.com_del is null) 
                //                                                        and dict_combine.com_id is not null 
                //                                                        group by com_name,dict_combine.com_seq,dict_combine_bar.com_his_fee_code
                //                                                        order by dict_combine.com_seq", type));

                IBCConnect connecter = BCConnectFactory.Create(downloadInfo);
                if (Extensions.IsEmpty(dsHISData))
                {
                    try
                    {
                        if (downloadInfo.DownloadType == PrintType.Inpatient)//住院
                        {
                            SZRYService ser = new SZRYService();
                            dsHISData = ser.GetApplyInfoToTable(barcode, "zy");

                            //try
                            //{
                            //    Logger.LogInfo("通过院网接口获取数据", "zy barcode " + barcode + " RowsCount " + dsHISData.Tables[0].Rows.Count);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logger.LogException("通过院网接口获取数据" + barcode, ex);
                            //}
                        }
                        else if (downloadInfo.DownloadType == PrintType.Outpatient)//门诊
                        {
                            SZRYService ser = new SZRYService();
                            dsHISData = ser.GetApplyInfoToTable(barcode, "mz");

                            //try
                            //{
                            //    Logger.LogInfo("通过院网接口获取数据", "mz barcode " + barcode + " RowsCount " + dsHISData.Tables[0].Rows.Count);
                            //}
                            //catch(Exception ex)
                            //{
                            //    Logger.LogException("通过院网接口获取数据" + barcode, ex);
                            //}
                        }
                        else
                        {
                            dsHISData = connecter.DownloadHisOrder();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("通过院网接口获取数据", ex);
                        throw;
                    }
                }

                Logger.LogInfo("从院网接口获取条码", watch2.ElapsedMilliseconds.ToString() + "ms");
                debugMsg.AppendLine(string.Format("通过院网接口获取数据:{0}", dsHISData.Tables.Count > 0 ? dsHISData.Tables[0].Rows.Count : 0));
                if (showLogger)
                {
                    Logger.LogInfo("通过院网接口获取数据", watch.ElapsedMilliseconds.ToString() + "ms");
                }


                if (dsHISData == null || Extensions.IsEmpty(dsHISData))//没有就返回
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return null;
                }

                //setBCPatientsSeq(dsHISData, dtCombineSeq, downloadInfo);//根据组合排序排列医嘱

                //  Logger
                //按对照表转换成LIS表的数据
                // DataSet dsLisData = ConvertHelper.ConvertToLis(dsHISData, dtPatientsStructure, downloadInfo.FetchDataType == FetchDataType.OutLink);
                DataSet dsLisData = ConvertHelper.ConvertToLis(dsHISData, dtPatientsStructure, downloadInfo);


                //Logger.LogInfo("对照表转换", watch2.ElapsedMilliseconds.ToString() + "ms");
                //中山需求，根据科室下载，不管接口数据是什么，都更新为下载科室。
                //dsLisData = SetDepInfo(downloadInfo, dsLisData);

                if (showLogger)
                {
                    Logger.LogInfo("条码下载:对照表转换", watch.ElapsedMilliseconds.ToString() + "ms");
                }
                debugMsg.AppendLine(string.Format("按对照表转换成LIS表的数据:{0}", dsLisData.Tables.Count > 0 ? dsLisData.Tables[0].Rows.Count : 0));

                //转换后没有数据
                if (Extensions.IsEmpty(dsHISData) || Extensions.IsEmpty(dsLisData))
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return dsHISData;
                }

                //当根据部门进行下载条码时进行并发控制
                //KeyResult keyReult = BarConcurrencyController.InsertKey(downloadInfo);
                //if (!keyReult.Ok)
                //{
                //    string temp_strEx = "不通过,已有此key在下载条码" + string.Format("Key:{0}", keyReult.Key);
                //    Logger.LogException("下载条码并发控制", new Exception(temp_strEx));
                //    return dsHISData;
                //}

                //string OrderTimeColumn =ConvertHelper.  downloadInfo.FetchDataType == FetchDataType.OutLink ? "chktm" : "医嘱时间";
                //string HISCombineIDColumn = downloadInfo.FetchDataType == FetchDataType.OutLink ? "FEEID" : "项目编码";
                //string HISCombineNameColumn = downloadInfo.FetchDataType == FetchDataType.OutLink ? "FEENM" : "项目名称";


                // 2014年2月21日16:43:54 ye
                //string OrderTimeColumn = ConvertHelper.GetHISColumn("bc_occ_date");
                //string HISCombineIDColumn = ConvertHelper.GetHISColumn("bc_his_code");
                //string HISCombineNameColumn = ConvertHelper.GetHISColumn("bc_his_name");
                dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);
                string OrderTimeColumn = columnConvertHelper.GetHISColumn("bc_occ_date");
                string HISCombineIDColumn = columnConvertHelper.GetHISColumn("bc_his_code");
                string HISCombineNameColumn = columnConvertHelper.GetHISColumn("bc_his_name");

                DataTable dtSource = dsHISData.Tables[0];//获取的数据
                DataTable dtBCPatients = dsLisData.Tables[0]; //转换后Lis表
                DataTable dtBCPatientsForInsert = dtBCPatients.Clone();//要插入的病人资料

                //if (string.IsNullOrEmpty(HISCombineIDColumn) || string.IsNullOrEmpty(HISCombineNameColumn))


                #region 插入条码表操作

                //插入条码表
                if (Extensions.IsNotEmpty(dsLisData))
                {
                    //拆分条码集合

                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                    IList<string> hisCombineIDs = new List<string>();

                    IList<string> orderIDs = new List<string>();

                    //HIS组合ID与医嘱ID
                    foreach (DataRow sourceRow in dtSource.Rows)
                    {
                        if (!string.IsNullOrEmpty(HISCombineIDColumn) && !hisCombineIDs.Contains(sourceRow[HISCombineIDColumn].ToString()))
                            hisCombineIDs.Add(sourceRow[HISCombineIDColumn].ToString());

                        orderIDs.Add(connecter.GenerateOrderID(sourceRow, downloadInfo));
                    }

                    //条码拆分信息
                    List<BarcodeCombines> bcCombinesCache = new List<BarcodeCombines>(FindBarcodeCombineByHISFeeCode(hisCombineIDs));
                    IList<string> comIDs = new List<string>();
                    if (bcCombinesCache != null)
                    {
                        foreach (BarcodeCombines bc in bcCombinesCache)
                        {
                            comIDs.Add(bc.CombineId);
                        }
                    }
                    //LIS组合
                    IList<Combines> combinesCache = FindLisCombinesByIDs(comIDs);
                    bool ShouldOrderIdBindingOriId = SystemConfiguration.GetSystemConfig("OrderIDBindingOriID") == "是";
                    //由于sql参数最大支持2100个：每次获取的医嘱id设定为2000
                    int eachCount = 2000;
                    List<BarcodeCombinesRecodes> bcCombineRecodeCache;
                    if (orderIDs.Count > eachCount)
                    {
                        //条码明细表
                        bcCombineRecodeCache = new List<BarcodeCombinesRecodes>();
                        // FindBCCombineRecode(orderIDs);

                        int orderIdCount = 0;
                        List<string> tempOrderId = new List<string>();
                        for (int i = 0; i < orderIDs.Count; i++)
                        {
                            tempOrderId.Add(orderIDs[i]);

                            if (orderIdCount >= eachCount - 1 || i == orderIDs.Count - 1)
                            {
                                IList<BarcodeCombinesRecodes> temp_bcCombineRecodeCache = null;
                                if (ShouldOrderIdBindingOriId)
                                    temp_bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(tempOrderId);
                                else
                                    temp_bcCombineRecodeCache = FindBCCombineRecode(tempOrderId);
                                if (temp_bcCombineRecodeCache != null && temp_bcCombineRecodeCache.Count > 0)
                                {
                                    bcCombineRecodeCache.AddRange(temp_bcCombineRecodeCache);
                                }
                                tempOrderId.Clear();
                                orderIdCount = 0;
                            }
                            else
                            {
                                orderIdCount++;

                            }
                        }
                    }
                    else
                    {
                        if (ShouldOrderIdBindingOriId)
                            bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(orderIDs).ToList();
                        else
                            bcCombineRecodeCache = FindBCCombineRecode(orderIDs).ToList();
                    }


                    //条码明细表里的医嘱ID
                    IList<string> bcCombineRecodeOrderCache = ToList(bcCombineRecodeCache, downloadInfo, ShouldOrderIdBindingOriId);

                    Logger.LogInfo("下载缓存表", watch2.ElapsedMilliseconds.ToString() + "ms");

                    if (showLogger)
                        Logger.LogInfo("条码下载:下载缓存表", watch.ElapsedMilliseconds.ToString() + "ms");

                    string oriid = GetOriID(downloadInfo.DownloadType);

                    for (int i = 0; i < dtBCPatients.Rows.Count; i++)
                    {
                        string hisCombineID = dtSource.Rows[i][HISCombineIDColumn].ToString(); //HIS组合ID
                        string hisCombineName = dtSource.Rows[i][HISCombineNameColumn].ToString(); //HIS组合名称

                        string orderID = connecter.GenerateOrderID(dtSource.Rows[i], downloadInfo); //医嘱ID
                        string orderID2 = string.Empty;

                        // 2014年2月21日16:44:51 ye
                        //if (dtSource.Columns.Contains(ConvertHelper.GetHISColumn("bc_yz_id2")) == true)
                        //{
                        //    orderID2 = dtSource.Rows[i][ConvertHelper.GetHISColumn("bc_yz_id2")].ToString();
                        //}
                        if (dtSource.Columns.Contains(columnConvertHelper.GetHISColumn("bc_yz_id2")) == true)
                        {
                            orderID2 = dtSource.Rows[i][columnConvertHelper.GetHISColumn("bc_yz_id2")].ToString();
                        }


                        if (!string.IsNullOrEmpty(orderID) && HasOrderIDInCombinesRecord(bcCombineRecodeOrderCache,
                                                       FormatOrderId(orderID, downloadInfo,
                                                                     ShouldOrderIdBindingOriId))) //如果明细表有此医嘱ID
                            continue;

                        string newBarcode = barcode;
                        IList<BarcodeCombines> barcodeCombines = null;
                        IList<BarcodeCombines> bcCombinesCache2 = new List<BarcodeCombines>(bcCombinesCache);

                        #region old


                        ////在条码拆分信息缓存里找HIS代码的记录
                        //barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache, hisCombineID, ref combinesCache, ref bcCombinesCache);
                        ////在找到的记录里找出对应来源的记录，如住院或门诊或默认
                        //barcodeCombines = FindWantBarcodeCombines(barcodeCombines, downloadInfo);

                        #endregion

                        //解决大相同his码不同来源的大组合的拆分问题

                        //在找到的记录里找出对应来源的记录，如住院或门诊或默认
                        // bcCombinesCache = FindWantBarcodeCombines(bcCombinesCache, downloadInfo);

                        bcCombinesCache2 =
                            bcCombinesCache.FindAll(
                                baritem => (string.IsNullOrEmpty(baritem.Oriid) || baritem.Oriid == oriid));


                        //在条码拆分信息缓存里找HIS代码的记录
                        //  barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache, hisCombineID, ref combinesCache, ref bcCombinesCache);
                        barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache2, hisCombineID,
                                                                              ref combinesCache,
                                                                              ref bcCombinesCache2);

                        barcodeCombines = FindWantBarcodeCombines(barcodeCombines, downloadInfo);

                        if (barcodeCombines.Count > 1 && !string.IsNullOrEmpty(downloadInfo.OperationName))
                        {
                            foreach (var bc in barcodeCombines)
                            {
                                if (downloadInfo.OperationName.Contains(bc.ExecPtypeCode))
                                {
                                    IList<BarcodeCombines> resultFinal = new List<BarcodeCombines>();
                                    resultFinal.Add(bc);
                                    barcodeCombines = resultFinal;
                                    break;
                                }
                            }
                        }

                        //生成条码
                        if (barcodeCombines == null || barcodeCombines.Count == 0)
                        {
                            GenereateMainBarcodeForSZ(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn,
                                                      dtSource, dtBCPatients, dtBCPatientsForInsert, ref now,
                                                      combinesCache, 0, hisCombineID, hisCombineName, orderID,
                                                      orderID2, newBarcode, null, ShouldOrderIdBindingOriId);
                        }
                        else
                        {
                            GenereateMainBarcodeForSZ(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn,
                                                      dtSource, dtBCPatients, dtBCPatientsForInsert, ref now,
                                                      combinesCache, 0, hisCombineID, hisCombineName, orderID,
                                                      orderID2, newBarcode, barcodeCombines[0],
                                                      ShouldOrderIdBindingOriId);
                        }

                    }

                    //Logger.LogInfo("拆分合并完成", watch2.ElapsedMilliseconds.ToString() + "ms");
                    if (showLogger)
                        Logger.LogInfo("条码下载:拆分合并完成", watch.ElapsedMilliseconds.ToString() + "ms");

                    downloadInfo.OperationName = "";
                    if (Insert(downloadInfo, dtCNameStructure, dtBCPatientsForInsert))
                    {

                    }
                    //Logger.LogInfo("保存完成", watch2.ElapsedMilliseconds.ToString() + "ms");
                    #region 医嘱确认

                    try
                    {
                        if (dtCNameStructure.Rows.Count > 0)
                        {
                            List<string> barcodelist = new List<string>();
                            foreach (DataRow row in dtBCPatientsForInsert.Rows) //遍历每一条条码
                            {
                                string bc_bar_code = row["bc_bar_code"].ToString();
                                string bc_in_no = row["bc_in_no"].ToString();
                                string bc_name = row["bc_name"].ToString();
                                string bc_pid = row["bc_pid"].ToString();
                                string bc_times = row["bc_times"].ToString();

                                if (string.IsNullOrEmpty(bc_times))
                                    bc_times = "0";

                                DataRow[] row_advice =
                                    dtCNameStructure.Select(string.Format("bc_bar_code = '{0}'", bc_bar_code));

                                if (row_advice.Length > 0)
                                {
                                    foreach (DataRow row_a in row_advice)
                                    {
                                        string advice_id = row_a["bc_yz_id"].ToString();
                                        string bc_his_code = row_a["bc_his_code"].ToString();
                                        string bc_his_name = row["bc_his_name"].ToString();
                                        string bc_social_no = row["bc_social_no"].ToString();

                                        List<InterfaceDataBindingItem> list =
                                            new List<InterfaceDataBindingItem>();
                                        list.Add(new InterfaceDataBindingItem("bc_in_no", bc_in_no));
                                        list.Add(new InterfaceDataBindingItem("bc_yz_id", advice_id));
                                        list.Add(new InterfaceDataBindingItem("bc_bar_code", bc_bar_code));
                                        list.Add(new InterfaceDataBindingItem("bc_his_code", bc_his_code));
                                        list.Add(new InterfaceDataBindingItem("bc_his_name", bc_his_name));
                                        list.Add(new InterfaceDataBindingItem("bc_pid", bc_pid));
                                        list.Add(new InterfaceDataBindingItem("bc_times", bc_times));
                                        list.Add(new InterfaceDataBindingItem("bc_name", bc_name));
                                        list.Add(new InterfaceDataBindingItem("op_time", DateTime.Now));
                                        list.Add(new InterfaceDataBindingItem("bc_social_no", bc_social_no));
                                        list.Add(new InterfaceDataBindingItem("op_code", downloadInfo.OperationDepId));
                                        list.Add(new InterfaceDataBindingItem("op_name", downloadInfo.OperationDepId));

                                        DataInterfaceHelper dihelper =
                                            new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                                        if (downloadInfo.DownloadType == PrintType.Outpatient)
                                        {
                                            //dihelper.ExecuteNonQueryWithGroup("条码_门诊_下载后", list.ToArray());
                                        }
                                        else if (downloadInfo.DownloadType == PrintType.Inpatient)
                                        {
                                            try
                                            {

                                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                                if (!barcodelist.Contains(barcode))
                                                {
                                                    barcodelist.Add(barcode);
                                                    SZRYService ser = new SZRYService();
                                                    string msg = ser.ConfirmZyApplyInfo(barcode);

                                                    dic.Add(barcode, msg);
                                                }

                                                SetFeeFlag(dic);
                                            }
                                            catch (Exception ex1)
                                            {
                                                Logger.LogException("医嘱确认SZRYService", ex1);
                                            }
                                            //dihelper.ExecuteNonQueryWithGroup("条码_住院_下载后", list.ToArray());
                                        }
                                        else if (downloadInfo.DownloadType == PrintType.TJ)
                                        {
                                            //dihelper.ExecuteNonQueryWithGroup("条码_体检_下载后", list.ToArray());
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("医嘱确认", ex);
                        throw;
                    }


                    #endregion

                    if (showLogger)
                    {
                        Logger.LogInfo("条码下载:插入两主表", watch.ElapsedMilliseconds.ToString() + "ms");
                        watch.Stop();
                    }
                }

                #endregion


                //BarConcurrencyController.RemoveKey(downloadInfo);
                if (DebugMode)
                {
                    Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                }
                dtSource.Clear();
                //dtBCPatients.Clear();
                dtBCPatientsForInsert.Clear();
                dtCNameStructure.Clear();

                DataSet dsReturn = new BCPatientBIZ().SearchByBarcode(barcode);

                watch2.Stop();
                Logger.LogInfo("总耗时", watch2.ElapsedMilliseconds.ToString() + "ms");
                return dsReturn;

                //return dsLisData;
            }
            catch (Exception ex)
            {
                //BarConcurrencyController.RemoveKey(downloadInfo);
                Logger.LogException("DownloadBarcodeFromHIS2", ex);
                throw;
            }
        }



        public DataSet DownloadBarcodeFromHISForLisCDR(BarcodeDownloadInfo downloadInfo)
        {
            try
            {
                StringBuilder debugMsg = new StringBuilder();

                string type = downloadInfo.GetDownloadTypeId();

                DBHelper helper = new DBHelper();
                DataSet dsHISData = null;

                string barcode = downloadInfo.PatientID;
                //获取条码表与明细表结构
                DataTable dtPatientsStructure = helper.GetTable(string.Format(@"select bc_bar_no,bc_bar_code,bc_frequency,bc_no_id,bc_in_no
                                                                                ,bc_bed_no,bc_name,bc_sex,bc_age,bc_d_code,bc_d_name,bc_diag,bc_doct_code,bc_doct_name,bc_his_name
                                                                                ,bc_sam_id,bc_sam_name,bc_date,bc_times,bc_cuv_code,bc_cuv_name,bc_cap_sum,bc_cap_unit,bc_urgent_flag
                                                                                ,bc_ctype,bc_ori_id,bc_ori_name,bc_exp,bc_computer,bc_social_no,bc_emp_id,bc_info,bc_hospital_id,bc_bar_type
                                                                                ,bc_status,bc_status_cname,bc_birthday,bc_blood,bc_blood_date,bc_blood_flag,bc_blood_name,bc_blood_code
                                                                                ,bc_print_flag,bc_print_date,bc_print_code,bc_print_name,bc_send_flag,bc_send_date,bc_send_code,bc_send_name
                                                                                ,bc_reach_flag,bc_reach_date,bc_reach_code,bc_reach_name,bc_receiver_flag,bc_receiver_date,bc_receiver_code
                                                                                ,bc_receiver_name,bc_occ_date,bc_sam_rem_id,bc_sam_rem_name,bc_print_time,bc_return_times,bc_lastaction_time
                                                                                ,bc_address,bc_tel,bc_pid,bc_emp_company,bc_app_no,bc_sam_dest,bc_fee_type,bc_emp_company_name
                                                                                ,bc_emp_company_dept,bc_oldlis_barcode,bc_notice,'' as bc_gourp_id,'' as bc_group_id,'' as bc_upid,'' as bc_merge_comid
                                                                                from bc_patients with(nolock) where bc_bar_no='{0}' and bc_del<>1 ", barcode));

                if (dtPatientsStructure.Rows.Count > 0)
                {
                    BCPatientBIZ bllBCPatient = new BCPatientBIZ();
                    DataSet dsBarCodePatient = bllBCPatient.SearchByBarcode(barcode);
                    return dsBarCodePatient;
                }

                DataTable dtCNameStructure = helper.GetTable("select bc_bar_no, bc_bar_code, bc_frequency, bc_his_name, bc_his_code, bc_yz_id,bc_yz_id2, bc_ctype, bc_flag, bc_date, bc_apply_date, bc_occ_date, bc_item_no, bc_confirm_flag, bc_confirm_code, bc_confirm_name, bc_price, bc_unit, bc_modify_flag, bc_modify_date, bc_itr_id, bc_view_flag, bc_exec_code, bc_exec_name, bc_out_time, bc_rep_flag, bc_rep_date, bc_enrol_flag, bc_other_no, bc_lis_code,bc_name,bc_blood_notice,bc_save_notice from bc_cname where 1<>1");

                IBCConnect connecter = BCConnectFactory.Create(downloadInfo);
                CDRService lisCDRService = new CDRService();
                if (Extensions.IsEmpty(dsHISData))
                {
                    try
                    {
                        DataSet ds = lisCDRService.GetBarCodeInfoToTable(barcode);

                        DataSet dsRes = new DataSet();
                        DataTable dtResult = ds.Tables["Result"];
                        if (dtResult != null)
                        {
                            dsRes.Tables.Add(dtResult.Copy());
                        }

                        dsHISData = dsRes;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("通过院网接口获取数据", ex);
                        throw;
                    }
                }

                debugMsg.AppendLine(string.Format("通过院网接口获取数据:{0}", dsHISData.Tables.Count > 0 ? dsHISData.Tables[0].Rows.Count : 0));


                if (dsHISData == null || Extensions.IsEmpty(dsHISData))//没有就返回
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return null;
                }

                DataSet dsLisData = ConvertHelper.ConvertToLis(dsHISData, dtPatientsStructure, downloadInfo);

                debugMsg.AppendLine(string.Format("按对照表转换成LIS表的数据:{0}", dsLisData.Tables.Count > 0 ? dsLisData.Tables[0].Rows.Count : 0));

                //转换后没有数据
                if (Extensions.IsEmpty(dsHISData) || Extensions.IsEmpty(dsLisData))
                {
                    if (DebugMode)
                    {
                        Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                    }
                    return dsHISData;
                }

                //当根据部门进行下载条码时进行并发控制
                //KeyResult keyReult = BarConcurrencyController.InsertKey(downloadInfo);
                //if (!keyReult.Ok)
                //{
                //    string temp_strEx = "不通过,已有此key在下载条码" + string.Format("Key:{0}", keyReult.Key);
                //    Logger.LogException("下载条码并发控制", new Exception(temp_strEx));
                //    return dsHISData;
                //}


                dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);
                string OrderTimeColumn = columnConvertHelper.GetHISColumn("bc_occ_date");
                string HISCombineIDColumn = columnConvertHelper.GetHISColumn("bc_his_code");
                string HISCombineNameColumn = columnConvertHelper.GetHISColumn("bc_his_name");

                DataTable dtSource = dsHISData.Tables[0];//获取的数据
                DataTable dtBCPatients = dsLisData.Tables[0]; //转换后Lis表
                DataTable dtBCPatientsForInsert = dtBCPatients.Clone();//要插入的病人资料

                #region 插入条码表操作

                //插入条码表
                if (Extensions.IsNotEmpty(dsLisData))
                {
                    //拆分条码集合

                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                    IList<string> hisCombineIDs = new List<string>();

                    IList<string> orderIDs = new List<string>();

                    //HIS组合ID与医嘱ID
                    foreach (DataRow sourceRow in dtSource.Rows)
                    {
                        if (!string.IsNullOrEmpty(HISCombineIDColumn) && !hisCombineIDs.Contains(sourceRow[HISCombineIDColumn].ToString()))
                            hisCombineIDs.Add(sourceRow[HISCombineIDColumn].ToString());

                        orderIDs.Add(connecter.GenerateOrderID(sourceRow, downloadInfo));
                    }


                    //条码拆分信息
                    List<BarcodeCombines> bcCombinesCache = new List<BarcodeCombines>(FindBarcodeCombineByHISFeeCode(hisCombineIDs));
                    IList<string> comIDs = new List<string>();
                    if (bcCombinesCache != null)
                    {
                        foreach (BarcodeCombines bc in bcCombinesCache)
                        {
                            comIDs.Add(bc.CombineId);
                        }
                    }
                    //LIS组合
                    IList<Combines> combinesCache = FindLisCombinesByIDs(comIDs);
                    bool ShouldOrderIdBindingOriId = SystemConfiguration.GetSystemConfig("OrderIDBindingOriID") == "是";
                    //由于sql参数最大支持2100个：每次获取的医嘱id设定为2000
                    int eachCount = 2000;
                    List<BarcodeCombinesRecodes> bcCombineRecodeCache;
                    if (orderIDs.Count > eachCount)
                    {
                        //条码明细表
                        bcCombineRecodeCache = new List<BarcodeCombinesRecodes>();

                        int orderIdCount = 0;
                        List<string> tempOrderId = new List<string>();
                        for (int i = 0; i < orderIDs.Count; i++)
                        {
                            tempOrderId.Add(orderIDs[i]);

                            if (orderIdCount >= eachCount - 1 || i == orderIDs.Count - 1)
                            {
                                IList<BarcodeCombinesRecodes> temp_bcCombineRecodeCache = null;
                                if (ShouldOrderIdBindingOriId)
                                    temp_bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(tempOrderId);
                                else
                                    temp_bcCombineRecodeCache = FindBCCombineRecode(tempOrderId);
                                if (temp_bcCombineRecodeCache != null && temp_bcCombineRecodeCache.Count > 0)
                                {
                                    bcCombineRecodeCache.AddRange(temp_bcCombineRecodeCache);
                                }
                                tempOrderId.Clear();
                                orderIdCount = 0;
                            }
                            else
                            {
                                orderIdCount++;

                            }
                        }
                    }
                    else
                    {
                        if (ShouldOrderIdBindingOriId)
                            bcCombineRecodeCache = FindBCCombineRecodeReturnOriId(orderIDs).ToList();
                        else
                            bcCombineRecodeCache = FindBCCombineRecode(orderIDs).ToList();
                    }


                    //条码明细表里的医嘱ID
                    IList<string> bcCombineRecodeOrderCache = ToList(bcCombineRecodeCache, downloadInfo, ShouldOrderIdBindingOriId);

                    string oriid = GetOriID(downloadInfo.DownloadType);

                    for (int i = 0; i < dtBCPatients.Rows.Count; i++)
                    {
                        string hisCombineID = dtSource.Rows[i][HISCombineIDColumn].ToString(); //HIS组合ID
                        string hisCombineName = dtSource.Rows[i][HISCombineNameColumn].ToString(); //HIS组合名称

                        string orderID = connecter.GenerateOrderID(dtSource.Rows[i], downloadInfo); //医嘱ID
                        string orderID2 = string.Empty;

                        if (dtSource.Columns.Contains(columnConvertHelper.GetHISColumn("bc_yz_id2")) == true)
                        {
                            orderID2 = dtSource.Rows[i][columnConvertHelper.GetHISColumn("bc_yz_id2")].ToString();
                        }

                        if (!string.IsNullOrEmpty(orderID) && HasOrderIDInCombinesRecord(bcCombineRecodeOrderCache,
                                                       FormatOrderId(orderID, downloadInfo,
                                                                     ShouldOrderIdBindingOriId))) //如果明细表有此医嘱ID
                            continue;

                        string newBarcode = barcode;
                        IList<BarcodeCombines> barcodeCombines = null;
                        IList<BarcodeCombines> bcCombinesCache2 = new List<BarcodeCombines>(bcCombinesCache);

                        bcCombinesCache2 =
                            bcCombinesCache.FindAll(
                                baritem => (string.IsNullOrEmpty(baritem.Oriid) || baritem.Oriid == oriid));

                        barcodeCombines = FindBarcodeCombineCacheByHISFeeCode(bcCombinesCache2, hisCombineID,
                                                                                                      ref combinesCache,
                                                                                                      ref bcCombinesCache2);

                        barcodeCombines = FindWantBarcodeCombines(barcodeCombines, downloadInfo);

                        if (barcodeCombines.Count > 1 && !string.IsNullOrEmpty(downloadInfo.OperationName))
                        {
                            foreach (var bc in barcodeCombines)
                            {
                                if (downloadInfo.OperationName.Contains(bc.ExecPtypeCode))
                                {
                                    IList<BarcodeCombines> resultFinal = new List<BarcodeCombines>();
                                    resultFinal.Add(bc);
                                    barcodeCombines = resultFinal;
                                    break;
                                }
                            }
                        }

                        //生成条码
                        if (barcodeCombines == null || barcodeCombines.Count == 0)
                        {
                            GenereateMainBarcodeForSZ(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn,
                                                      dtSource, dtBCPatients, dtBCPatientsForInsert, ref now,
                                                      combinesCache, 0, hisCombineID, hisCombineName, orderID,
                                                      orderID2, newBarcode, null, ShouldOrderIdBindingOriId);
                        }
                        else
                        {
                            GenereateMainBarcodeForSZ(downloadInfo, dtCNameStructure, connecter, OrderTimeColumn,
                                                      dtSource, dtBCPatients, dtBCPatientsForInsert, ref now,
                                                      combinesCache, 0, hisCombineID, hisCombineName, orderID,
                                                      orderID2, newBarcode, barcodeCombines[0],
                                                      ShouldOrderIdBindingOriId);
                        }

                    }

                    downloadInfo.OperationName = "";
                    if (Insert(downloadInfo, dtCNameStructure, dtBCPatientsForInsert))
                    {

                    }
                    #region 医嘱确认

                    try
                    {
                        lisCDRService.BarcodeReceive(barcode, downloadInfo.PatientID, downloadInfo.PatientName);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("医嘱确认", ex);
                        throw;
                    }
                    #endregion
                }

                #endregion


                //BarConcurrencyController.RemoveKey(downloadInfo);
                if (DebugMode)
                {
                    Logger.LogInfo("DownloadBarcodeFromHIS_Debug", debugMsg.ToString());
                }
                dtSource.Clear();
                dtBCPatientsForInsert.Clear();
                dtCNameStructure.Clear();

                DataSet dsReturn = new BCPatientBIZ().SearchByBarcode(barcode);

                return dsReturn;
            }
            catch (Exception ex)
            {
                //BarConcurrencyController.RemoveKey(downloadInfo);
                Logger.LogException("DownloadBarcodeFromHIS2", ex);
                throw;
            }
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
        private void SetFeeFlag(Dictionary<string, string> dic)
        {
            foreach (var key in dic.Keys)
            {
                DataSet ds = ConvertXMLToDataSet(dic[key]);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                    && ds.Tables[0].Columns.Contains("Result"))
                {
                    string res = ds.Tables[0].Rows[0]["Result"].ToString() == "100" ? "2" : "1";

                    string sql = string.Format("update bc_patients set bc_upid='{0}' where bc_bar_code='{1}'", res, key);

                    DBHelper hepler = new DBHelper();
                    hepler.ExecuteNonQuery(sql);
                }
            }
        }


        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="downloadInfo"></param>
        /// <param name="dtCNameStructure"></param>
        /// <param name="connecter"></param>
        /// <param name="OrderTimeColumn"></param>
        /// <param name="dtSource"></param>
        /// <param name="dtBCPatients"></param>
        /// <param name="dtBCPatientsForInsert"></param>
        /// <param name="splitCodes"></param>
        /// <param name="now"></param>
        /// <param name="combinesCache"></param>
        /// <param name="i"></param>
        /// <param name="hisCombineID"></param>
        /// <param name="hisCombineName"></param>
        /// <param name="orderID"></param>
        /// <param name="newBarcode"></param>
        /// <param name="hasPatients"></param>
        /// <param name="barcodeCombine"></param>
        private void GenereateMainBarcodeForSZ(BarcodeDownloadInfo downloadInfo, DataTable dtCNameStructure, IBCConnect connecter, string OrderTimeColumn, DataTable dtSource, DataTable dtBCPatients, DataTable dtBCPatientsForInsert, ref DateTime now, IList<Combines> combinesCache, int i, string hisCombineID, string hisCombineName, string orderID, string orderID2, string newBarcode, BarcodeCombines barcodeCombine, bool ShouldOrderIdBindingOriId)
        {
            DataRow newBCPatientRow = dtBCPatients.NewRow();
            newBCPatientRow.ItemArray = dtBCPatients.Rows[i].ItemArray;
            //Combines combine = null;


            //暂时不分预置条码和自动条码
            newBCPatientRow[BarcodeTable.Patient.BarcodeNumber] = newBarcode;
            newBCPatientRow[BarcodeTable.Patient.BarcodeDisplayNumber] = newBarcode;
            newBCPatientRow[BarcodeTable.Patient.CreateTime] = now.ToString();
            newBCPatientRow[BarcodeTable.Patient.LastActionTime] = newBCPatientRow[BarcodeTable.Patient.CreateTime];//added by lin : 2010/7/6
            newBCPatientRow[BarcodeTable.Patient.IDType] = GetNoTypeID(downloadInfo.DownloadType); //病人ID类型

            newBCPatientRow["bc_bar_type"] = "0";
            if (ShouldOrderIdBindingOriId)
                newBCPatientRow[BarcodeTable.Patient.OriID] = GetOriID(downloadInfo.DownloadType);
            //发票号
            if (!string.IsNullOrEmpty(downloadInfo.InvoiceID))
            {
                newBCPatientRow[BarcodeTable.Patient.SocialNumber] = downloadInfo.InvoiceID;
            }
            //如果项目有"产检男性" 姓名后加"之夫"
            connecter.SpecialPatient(ref newBCPatientRow, hisCombineName);

            //Logger.LogInfo("bc_sam_id", newBCPatientRow[BarcodeTable.Patient.SampleID].ToString());
            if (newBCPatientRow[BarcodeTable.Patient.SampleID].ToString() != string.Empty)
            {
                EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_incode == newBCPatientRow[BarcodeTable.Patient.SampleID].ToString());

                if (dict_sam != null)
                    newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
            }
            string samid = newBCPatientRow[BarcodeTable.Patient.SampleID].ToString();
            string samname = newBCPatientRow[BarcodeTable.Patient.SampleName].ToString();
            //来源
            if (barcodeCombine != null)
            {
                if (!ShouldOrderIdBindingOriId)
                    if (!string.IsNullOrEmpty(barcodeCombine.Oriid))
                        newBCPatientRow[BarcodeTable.Patient.OriID] = barcodeCombine.Oriid; //医嘱
                    else
                        newBCPatientRow[BarcodeTable.Patient.OriID] = GetOriID(downloadInfo.DownloadType);

                if (newBCPatientRow[BarcodeTable.Patient.SampleID].ToString() != string.Empty)
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_id == newBCPatientRow[BarcodeTable.Patient.SampleID].ToString());

                    if (dict_sam != null)
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                    else
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别
                }
                else if (newBCPatientRow[BarcodeTable.Patient.SampleName].ToString() != string.Empty)
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_name == newBCPatientRow[BarcodeTable.Patient.SampleName].ToString());

                    if (dict_sam != null
                        && string.IsNullOrEmpty(barcodeCombine.Sampleid)
                        && string.IsNullOrEmpty(newBCPatientRow[BarcodeTable.Patient.SampleID].ToString()))
                    {
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                    }
                    else
                    {
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                    }
                }
                else
                {
                    newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                }

                //newBCPatientRow[BarcodeTable.Patient.SampleID] = barcodeCombine.Sampleid;    //标本类别 
                newBCPatientRow[BarcodeTable.Patient.CuvetteCode] = barcodeCombine.CuvCode;//试管类别
                newBCPatientRow["bc_sam_dest"] = barcodeCombine.ComSamDest;
                newBCPatientRow[BarcodeTable.Patient.SampleRemarkID] = barcodeCombine.SampleRemarkid;//标本备注
                newBCPatientRow[BarcodeTable.Patient.CTypeID] = barcodeCombine.ExecPtypeCode;//检验系统的科室字典
                newBCPatientRow[BarcodeTable.Patient.PrintCount] = barcodeCombine.PrintCount; //打印次数
            }

            try
            {
                if (newBCPatientRow[BarcodeTable.Patient.SampleID].ToString() == string.Empty && !string.IsNullOrEmpty(samid))
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_incode == samid);

                    if (dict_sam != null)
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                }
                if (newBCPatientRow[BarcodeTable.Patient.SampleID].ToString() == string.Empty && !string.IsNullOrEmpty(samname))
                {
                    EntityDictSample dict_sam = dcl.svr.cache.DictSampleCache.Current.Cache.Find(z => z.sam_incode == samname);

                    if (dict_sam != null)
                        newBCPatientRow[BarcodeTable.Patient.SampleID] = dict_sam.sam_id;    //标本类别 
                }
            }
            catch
            {
            }


            if (dtSource.Columns.Contains("Dspt")) //标本备注或备注
            {
                //新
                //his的注释写入到bc_patients中的bc_sam_rem_name中
                newBCPatientRow[BarcodeTable.Patient.Remark] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkID] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkName] = dtSource.Rows[i]["Dspt"].ToString();

                //注释by lin
                //原:his的注释写入到bc_patients中的bc_exp中
                //newBCPatientRow[BarcodeTable.Patient.Remark] = newBCPatientRow[BarcodeTable.Patient.SampleRemarkName] = dtSource.Rows[i]["Dspt"].ToString();
            }

            string showHisCode = "";
            string showHisName = "";



            //条码明细表
            DataRow cnNameRow = dtCNameStructure.NewRow();
            cnNameRow[BarcodeTable.CName.BarcodeNumber] = newBarcode;
            cnNameRow[BarcodeTable.CName.BarcodeDisplayNumber] = newBarcode;
            cnNameRow[BarcodeTable.CName.SignFlag] = 0;
            //未加执行科室编码,执行科室名称             
            if (barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.HisCombineCode)) //如果有条码信息的his code 就取
                cnNameRow[BarcodeTable.CName.HisCode] = barcodeCombine.HisCombineCode;
            else
                cnNameRow[BarcodeTable.CName.HisCode] = hisCombineID;

            if (barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.HisCombineName)) //如果有条码信息的his name 就取
                cnNameRow[BarcodeTable.CName.HisName] = barcodeCombine.HisCombineName;
            else
                cnNameRow[BarcodeTable.CName.HisName] = hisCombineName;
            // cnNameRow[BarcodeTable.CName.HisCode] =barcodeCombine.HisCombineCode hisCombineID;//connecter.GetHisCode(hisCombineID, showHisCode);
            //cnNameRow[BarcodeTable.CName.HisName] = hisCombineName;

            cnNameRow[BarcodeTable.CName.CombineName] = connecter.GetHisName(hisCombineName, showHisName, barcodeCombine);
            //特殊项目名称
            //cnNameRow = connecter.SpecialItem(hisCombineName, cnNameRow, combine, barcodeCombine);
            cnNameRow[BarcodeTable.CName.OrderID] = orderID;//医嘱ID
            cnNameRow["bc_yz_id2"] = orderID2;
            cnNameRow = SetOrderTime(OrderTimeColumn, dtSource.Rows[i], cnNameRow);//医嘱申请时间
            cnNameRow[BarcodeTable.CName.EnrolFlag] = 0;
            cnNameRow[BarcodeTable.CName.ViewFlag] = 1;//显示标志(0-不显示 1-显示)

            if (barcodeCombine != null)
            {
                cnNameRow[BarcodeTable.CName.LisCombineCode] = barcodeCombine.CombineId;
                //cnNameRow["bc_combine_seq"]=barcodeCombine.
                cnNameRow[BarcodeTable.CName.Price] = barcodeCombine.Price;
                cnNameRow[BarcodeTable.CName.Unit] = barcodeCombine.Unit;
                //注意事项 保存与采血
                cnNameRow[BarcodeTable.CName.SaveNotice] = barcodeCombine.SaveNotice;
                cnNameRow[BarcodeTable.CName.BloodNotice] = barcodeCombine.BloodNotice;
            }

            newBCPatientRow[BarcodeTable.Patient.Item] = connecter.GetHisName(hisCombineName, showHisName, barcodeCombine);

            if (dtBCPatientsForInsert.Rows.Count == 0)
            {
                dtBCPatientsForInsert.Rows.Add(newBCPatientRow.ItemArray);

                //added by lin 2010/5/28:新条码默认回退次数为0
                if (dtBCPatientsForInsert.Columns.Contains("bc_return_times"))
                {
                    dtBCPatientsForInsert.Rows[dtBCPatientsForInsert.Rows.Count - 1]["bc_return_times"] = 0;
                }
            }
            else
            { //更新项目名称
                DataRow[] drFindBC = dtBCPatientsForInsert.Select(string.Format("bc_bar_no = '{0}'", newBarcode));
                if (drFindBC != null && drFindBC.Length == 1)
                    drFindBC[0][BarcodeTable.Patient.Item] = drFindBC[0][BarcodeTable.Patient.Item].ToString() + "+" + cnNameRow[BarcodeTable.CName.CombineName].ToString();
            }


            dtCNameStructure.Rows.Add(cnNameRow);
        }

        /// <summary>
        /// 获取相同发票号中未采集的条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public DataTable GetBarcodeReadySample(string barcode)
        {
            DataTable ret = new DataTable("dt");
            if (!string.IsNullOrEmpty(barcode))
            {
                string sql = @"select bc_name,bc_bar_code,bc_his_name,bc_pid from bc_patients with(nolock)
where bc_pid=(select top 1 bc_pid from bc_patients bc with(nolock) where bc.bc_bar_code='{0}') and bc_del='0'
and bc_status in('0','1')
and bc_ori_id='107'
and bc_bar_code<>'{0}'
and bc_pid is not null
and bc_pid<>'' ";

                SqlHelper helper = new SqlHelper();
                ret = helper.GetTable(string.Format(sql, barcode), "dt");
            }
            return ret;
        }


        public string TranferBarcode(string barcode)
        {
            return new BarcodeDictBIZ().TranferBarcode(barcode);
        }

        /// <summary>
        /// 获取条码的死腔体积量与最小采集量信息
        /// </summary>
        /// <param name="sqlInBC"></param>
        /// <returns></returns>
        public DataTable GetComCapSumByBarcode(string sqlInBC)
        {
            DataTable dt = new DataTable("ComCapSum");

            try
            {
                string sql = string.Format(@"select bc_patients.bc_bar_code,
count(1) as SL,
ISNULL(max(dict_combine_bar.com_deadspace_volume),0) as SKTJ,
ISNULL(sum(dict_combine_bar.com_cap_sum),0) as ZJCJL
from 
bc_patients WITH(NOLOCK)
inner join bc_cname WITH(NOLOCK) on bc_cname.bc_bar_code=bc_patients.bc_bar_code 
inner join dict_combine_bar WITH(NOLOCK) on dict_combine_bar.com_id=bc_cname.bc_lis_code
where  
dict_combine_bar.com_ori_id=bc_patients.bc_ori_id
and bc_patients.bc_del='0' 
and bc_cname.bc_del='0'
and bc_patients.bc_bar_code in({0})
group by bc_patients.bc_bar_code
", sqlInBC);
                dt = new SqlHelper().GetTable(sql);

                dt.TableName = "ComCapSum";
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return dt;
        }

        public string HisBarcodeConfirm(EntityBcParam bcparam)
        {
            try
            {
                Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();
                string code = "V";
                if (bcparam.StatusCode == "5")
                    code = "E";
                if (bcparam.StatusCode == "D")//D接口dll有特殊处理
                    code = "D";
                return ser.ChangeOrdItemStatusByBarcode(bcparam.Barcode, bcparam.ConfirmCode, code);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取包数
        /// </summary>
        /// <param name="bc_status"></param>
        /// <returns></returns>
        public DataTable GetPackCount(string bc_status, string bc_d_code)
        {
            string sqlDate = "and bc_blood_date>='" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and bc_blood_date<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            if (string.IsNullOrEmpty(bc_d_code))
            {
                sqlDate += " and (bc_d_code='' or bc_d_code is null)";
            }
            else
                sqlDate += " and bc_d_code='" + bc_d_code + "'";
            string bc_status2 = string.Empty;
            if (bc_status == "3")
                bc_status2 = Convert.ToString(Convert.ToInt16(bc_status) - 1);
            else
                bc_status2 = Convert.ToString(Convert.ToInt16(bc_status) - 2);
            string sql = string.Format(@"select
(SELECT count(distinct bc_upid) from bc_patients where 
bc_upid is not null and bc_upid<>'' and bc_status={0} {1}) as trCount,
(SELECT count(distinct bc_upid) from bc_patients(nolock) where 
bc_upid is not null and bc_upid<>'' and bc_status = {2} {1}) as allCount", bc_status, sqlDate, bc_status2);

            DataTable dt = new SqlHelper().GetTable(sql);
            dt.TableName = "dtPack";
            return dt;

        }

        #region 标本查漏

        /// <summary>
        /// 获取标本信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetBCPatientsBySpecimen(string strWhere)
        {
            string sql = string.Format(@"select bc_patients.bc_bar_code,
		bc_patients.bc_name,
		bc_patients.bc_sex,
		bc_patients.bc_in_no,
		bc_patients.bc_receiver_date,
		bc_patients.bc_status,
		bc_status.bc_cname,
		bc_patients.bc_his_name,
        --patients.pat_flag,  
        isnull(dict_type.type_name,'') barcode_type_name--,
		--hasresult = case when exists(select top 1 res_id from resulto where resulto.res_id = patients.pat_id and resulto.res_flag=1) then 1 else 0 end
		from bc_patients with(nolock)
		LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id 
		LEFT OUTER JOIN bc_status on bc_patients.bc_status = bc_status.bc_name 
		--left join patients with(nolock) on patients.pat_bar_code=bc_patients.bc_bar_code 
        where 1=1 {0}", strWhere);
            DataTable dtPatients = new SqlHelper().GetTable(sql);
            dtPatients.Columns.Add("complete");  //是否完成
            dtPatients.Columns.Add("notcomplete");  //未完成
            dtPatients.Columns.Add("partcomplete");  //部分完成
            dtPatients.Columns.Add("pat_flag");  //检验标志
            dtPatients.Columns.Add("hasresult"); //是否有结果
            dtPatients.Columns.Add("register_count"); //是否有结果

            dtPatients.TableName = "dtPatients";
            dtPatients.DefaultView.Sort = "bc_bar_code";

            return dtPatients;
        }

        /// <summary>
        /// 判断必录项目结果是否完整
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        private bool IsComplete(string barCode, DataTable dtComItems)
        {
            string sqlWhere = string.Format(@"patients.pat_bar_code='{0}'", barCode);
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
            {
                sqlWhere = string.Format(@"patients.pat_bar_code like '{0}%'", barCode);
            }
            //查出组合
            string sqlPatMi = string.Format(@"select patients.pat_id,bc_cname.bc_lis_code,bc_cname.bc_name,dict_instrmt.itr_mid,pat_date,pat_sid,pat_flag
from bc_cname with(nolock)
left join patients with(nolock) on bc_cname.bc_bar_code=patients.pat_bar_code
left join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id
where {0}", sqlWhere);
            DataTable dtPatMi = new SqlHelper().GetTable(sqlPatMi);

            if (dtPatMi != null && dtPatMi.Rows.Count > 0)
            {
                foreach (DataRow row in dtPatMi.Rows)
                {

                    //获取此组合所有必录项目
                    DataRow[] drComItems = dtComItems.Select(string.Format("com_id='{0}'", row["bc_lis_code"]));
                    //获取此条码的所有结果
                    DataTable dtResult = GetResultBySpecimen(barCode);
                    bool complete_flag = false;
                    foreach (DataRow drItem in drComItems)
                    {
                        foreach (DataRow drResult in dtResult.Rows)
                        {
                            if (drResult["res_itm_id"].ToString() == drItem["com_itm_id"].ToString())
                            {
                                complete_flag = true;
                                break;
                            }
                            else
                            {
                                complete_flag = false;
                            }
                        }
                        if (!complete_flag)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取标本组合信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public DataTable GetBCCnameBySpecimen(string barCode)
        {
            //查出组合
            string sqlCname = string.Format(@"select bc_cname.bc_lis_code com_id,bc_cname.bc_name com_name
                from bc_cname with(nolock) WHERE bc_bar_code='{0}'", barCode);
            DataTable dtCname = new SqlHelper().GetTable(sqlCname);
            dtCname.TableName = "bc_cname";

            return dtCname;
        }

        /// <summary>
        /// 获取条码状态
        /// </summary>
        /// <param name="dtPatients"></param>
        /// <returns></returns>
        public DataTable GetBCPatientStatusBySpecimen(DataSet ds)
        {
            DataTable dtPatientsStatus = ds.Tables["patients"];
            DataTable dtBrcode = new DataTable("dtBarCode");
            if (dtPatientsStatus != null && dtPatientsStatus.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPatientsStatus.Rows)
                {
                    if (dr["bc_status"].ToString() == "5")
                    {
                        bool bCup = false;
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
                        {
                            //判断是否有分杯条码
                            DataTable dtCup = GetCupBarcodeBySpecimen(dr["bc_bar_code"].ToString());
                            foreach (DataRow item in dtCup.Rows)
                            {
                                if (item["bc_status"].ToString() != "5")
                                {
                                    bCup = true;
                                    break;
                                }
                            }
                        }
                        if (!bCup)
                        {
                            dr["complete"] = "0";
                            dr["hasresult"] = "0";
                            dr["pat_flag"] = "-1";
                            continue;
                        }
                    }
                    string sqlWhere = string.Format(@"patients.pat_bar_code='{0}'", dr["bc_bar_code"].ToString());
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
                    {
                        sqlWhere = string.Format(@"patients.pat_bar_code like '{0}%'", dr["bc_bar_code"].ToString());
                    }
                    string sqlPatient = string.Format(@"select pat_flag,
                                    hasresult = case when exists(select top 1 res_id from resulto where resulto.res_id = patients.pat_id and resulto.res_flag=1) then 1 else 0 end
                                    from patients with(nolock) where {0}", sqlWhere);
                    DataTable dtTemp = new SqlHelper().GetTable(sqlPatient);
                    DataView dv = dtTemp.DefaultView.ToTable().DefaultView;
                    dv.Sort = "pat_flag desc";
                    DataTable dt = dv.ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int pat_flag_temp = 0;
                        int hasresult_temp = 0;
                        //取最大值
                        foreach (DataRow item in dt.Rows)
                        {
                            if (item["pat_flag"] != null
                                && !string.IsNullOrEmpty(item["pat_flag"].ToString())
                                && item["pat_flag"] != DBNull.Value)
                            {
                                int pat_flag = 0;
                                if (int.TryParse(item["pat_flag"].ToString().Trim(), out pat_flag)
                                    && pat_flag >= pat_flag_temp)
                                {
                                    dr["pat_flag"] = item["pat_flag"].ToString().Trim();
                                    pat_flag_temp = pat_flag_temp + pat_flag;
                                }
                            }
                            if (item["hasresult"] != null
                                && !string.IsNullOrEmpty(item["hasresult"].ToString())
                                && item["hasresult"] != DBNull.Value)
                            {
                                int hasresult = 0;
                                if (int.TryParse(item["hasresult"].ToString().Trim(), out hasresult)
                                    && hasresult >= hasresult_temp)
                                {
                                    dr["hasresult"] = item["hasresult"].ToString().Trim();
                                    hasresult_temp++;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(dr["pat_flag"].ToString())
                            && int.TryParse(dr["pat_flag"].ToString().Trim(), out pat_flag_temp))
                        {
                            if (pat_flag_temp > 1)
                            {
                                dr["register_count"] = GetRegisterCountBySpecimen(dr["bc_bar_code"].ToString());
                                bool blComplete = IsComplete(dr["bc_bar_code"].ToString(), ds.Tables["itm_com_mi"]);
                                dr["complete"] = blComplete ? "1" : "0";
                            }
                            else
                            {
                                dr["complete"] = "0";
                                dr["register_count"] = "0";
                            }
                        }
                    }
                    else
                    {
                        dr["complete"] = "0";
                        dr["hasresult"] = "0";
                        dr["pat_flag"] = "-1";
                        dr["register_count"] = "0";
                    }
                }
            }

            dtPatientsStatus.TableName = "status";
            return dtPatientsStatus;
        }

        /// <summary>
        /// 获取登记次数
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        private string GetRegisterCountBySpecimen(string barCode)
        {
            string sqlWhere = string.Format(@"bc_sign.bc_bar_code='{0}'", barCode);
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
            {
                sqlWhere = string.Format(@"bc_sign.bc_bar_code like '{0}%'", barCode);
            }
            string sqlPatient = string.Format(@"select count(*) from bc_sign where {0} and bc_status='20'", sqlWhere);
            DataTable dtTemp = new SqlHelper().GetTable(sqlPatient);
            string strCount = dtTemp.Rows[0][0].ToString();
            return strCount;
        }

        private DataTable GetCupBarcodeBySpecimen(string barCode)
        {
            string sql = string.Format(@"select bc_status from bc_patients where bc_bar_code like '{0}%'", barCode);
            DataTable dtTemp = new SqlHelper().GetTable(sql);
            return dtTemp;
        }

        /// <summary>
        /// 获取项目结果
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataTable GetResultBySpecimen(string barCode)
        {
            string sqlWhere = string.Format(@"patients.pat_bar_code='{0}'", barCode);
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
            {
                sqlWhere = string.Format(@"patients.pat_bar_code like '{0}%'", barCode);
            }
            string sqlSelect = string.Format(@"
select resulto.res_itm_id,
resulto.res_itm_ecd,
resulto.res_chr,
dict_instrmt.itr_mid,
patients.pat_sid,
resulto.res_date
from patients with(nolock)
left join resulto with(nolock) on patients.pat_id=resulto.res_id
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
where {0}
", sqlWhere);
            DataTable dtResult = new SqlHelper().GetTable(sqlSelect);
            dtResult.TableName = "dtResult";
            return dtResult;
        }

        /// <summary>
        /// 获取项目结果明细
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public DataTable GetResultByBarcodeBySpecimen(string barcode)
        {
            string sqlWhere = string.Format(@"and patients.pat_bar_code='{0}'", barcode);
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_SearchCupBarcode") == "是")
            {
                sqlWhere = string.Format(@"and patients.pat_bar_code like '{0}%'", barcode);
            }
            string sqlSelect = string.Format(@"
                select resulto.res_itm_id,
                resulto.res_itm_ecd,
                resulto.res_chr,
                dict_instrmt.itr_mid,
                patients.pat_sid,
                resulto.res_date
                from patients with(nolock)
                left join resulto on patients.pat_id=resulto.res_id
                left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
                left join bc_patients on bc_patients.bc_bar_code=patients.pat_bar_code
                where 1=1 {0}
                ", sqlWhere);

            DataTable dtResult = new SqlHelper().GetTable(sqlSelect);
            dtResult.TableName = "dtResult";
            return dtResult;
        }

        #endregion

        public int UpdateBcSign(DataTable dt)
        {
            int i = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                string sql = string.Format(@"
                                insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,
                                bc_bar_no,bc_bar_code,bc_remark)
                                values
                                (getdate(),'{0}','{1}','650','{2}','{2}','{3}')"
                    , dt.Rows[0]["bc_login_id"], dt.Rows[0]["bc_name"], dt.Rows[0]["bc_bar_no"], dt.Rows[0]["bc_remark"]);

                DBHelper hepler = new DBHelper();
                i = hepler.ExecuteNonQuery(sql);
            }
            return i;

        }

        public void SysOperationLog(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["bc_old_age"].ToString() != dt.Rows[0]["bc_age"].ToString())
                {
                    dcl.root.logon.OperationLogger logger = new dcl.root.logon.OperationLogger(dt.Rows[0]["loginID"].ToString(), dt.Rows[0]["ip"].ToString(), dcl.root.logon.SysOperationLogModule.PATIENTS, dt.Rows[0]["bc_bar_code"].ToString());
                    logger.Add_ModifyLog(dcl.root.logon.SysOperationLogGroup.PAT_INFO, "年龄", dt.Rows[0]["bc_old_age"].ToString() + "→" + dt.Rows[0]["bc_age"].ToString());
                    logger.Log();
                }
            }
        }

        public void UpdateBcOutFlag(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                string sql = string.Format(@"
                                update bc_patients set bc_receiver_flag=1,bc_reach_flag=1,bc_send_flag=1 where bc_bar_code='{0}'", barcode);
                DBHelper hepler = new DBHelper();
                hepler.ExecuteNonQuery(sql);
            }
        }

    }
}
