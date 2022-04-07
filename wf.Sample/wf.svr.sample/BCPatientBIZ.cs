using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.common.extensions;
using System.Collections;
using dcl.svr.root.com;
using dcl.common;
using lis.dto.Entity;
using dcl.svr.cache;
using Lib.DAC;
using Lib.DataInterface.Implement;
using dcl.root.dac;
using Lib.LogManager;
using Lis.SendDataToCDR;
using dcl.svr.frame;
using dcl.common;
using dcl.pub.entities.dict;
using lis.common.extensions;
using System.Configuration;

namespace dcl.svr.sample
{
    public class BCPatientBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.Patient.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.Patient.ID; }
        }

        public string DeleteFlagSQL
        {
            get { return " bc_del <> '1' "; }
        }

        //排除没有项目的病人资料
        public string CheckNoBcCnameSql
        {
            get
            {
                if (CacheSysConfig.Current.GetSystemConfig("7") == "预置条码")
                {
                    return
                        " AND EXISTS(SELECT BC_ID FROM bc_cname with(nolock)  WHERE bc_cname.bc_bar_no=bc_patients.bc_bar_no) ";
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据条码号获取条码信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public DataSet SearchByBarcode(string barCode)
        {
            if (string.IsNullOrEmpty(barCode))
            {
                return new DataSet();
            }

            DataSet dsBarCodePatient = Search(" WHERE " + DeleteFlagSQL + " AND " + string.Format(" {0} = '{1}' ", BarcodeTable.Patient.BarcodeDisplayNumber, barCode));

            ////根据签名信息填充时间
            //if (dsBarCodePatient.Tables.Count > 0 && dsBarCodePatient.Tables[0].Rows.Count > 0)
            //{
            //    DataTable dtBarCodePat = dsBarCodePatient.Tables[0];

            //    //获取签名信息
            //    DataTable dtSign = new BCSignBIZ().SearchByBarcode(barCode);

            //    //0	未打印	
            //    //1	已打印	
            //    //2	采集	
            //    //-2	撤消采集	
            //    //3	已收取	
            //    //-3	撤消收取	
            //    //4	已送检	
            //    //-4	撤消送检	
            //    //5	签收	
            //    //6	检验中	
            //    //7	已检验	
            //    //8	二次送检	
            //    //9	回退	

            //    //bool bc_print_date_filled = false;
            //    //bool bc_blood_date_filled = false;
            //    //bool bc_send_date_filled = false;
            //    //bool bc_reach_date_filled = false;
            //    //bool bc_receiver_date_filled = false;

            //    if (dtSign.Rows.Count > 0)
            //    {
            //        foreach (DataRow drSign in dtSign.Rows)
            //        {
            //            if (drSign["bc_status"].ToString() == "1")//打印
            //            {
            //                dtBarCodePat.Rows[0]["bc_print_date"] = drSign["bc_date"];
            //            }

            //            if (drSign["bc_status"].ToString() == "2")//采集
            //            {
            //                dtBarCodePat.Rows[0]["bc_blood_date"] = drSign["bc_date"];
            //            }

            //            if (drSign["bc_status"].ToString() == "3")//收取
            //            {
            //                dtBarCodePat.Rows[0]["bc_send_date"] = drSign["bc_date"];
            //            }

            //            if (drSign["bc_status"].ToString() == "4")//送达
            //            {
            //                dtBarCodePat.Rows[0]["bc_reach_date"] = drSign["bc_date"];
            //            }

            //            if (drSign["bc_status"].ToString() == "5")//签收
            //            {

            //                dtBarCodePat.Rows[0]["bc_receiver_date"] = drSign["bc_date"];
            //            }
            //        }
            //    }
            //}

            return dsBarCodePatient;
        }

        public override DataSet Search(string where)
        {
            string sql = SearchSQL + where + " AND " + DeleteFlagSQL + CheckNoBcCnameSql; //+ string.Format(" ORDER BY {0} ", BarcodeTable.Patient.StatusCode);

            try
            {
                DataSet ds = doSearch(new DataSet(), sql);
                SqlHelper helper = new SqlHelper();

                if (where.ToLower().Trim().StartsWith("where  bc_bar_code =")
                    || where.ToLower().Trim().StartsWith("where  bc_bar_no =")
                    )
                {
                    DataTable table = helper.GetTable("select * from bc_sign WITH(NOLOCK) " + where + " order by bc_date asc");
                    table.TableName = "bc_sign";
                    ds.Tables.Add(table);

                    string strRemSql = @"select com_name,com_rem from 
                                bc_cname WITH(NOLOCK)
                                left join dict_combine on com_id=bc_cname.bc_lis_code
                                " + where;
                    DataTable dtComRem = helper.GetTable(strRemSql, "com_rem");
                    ds.Tables.Add(dtComRem);
                }

                if (where.ToLower().Trim().Contains("where bc_upid"))
                {
                    DataTable table = helper.GetTable("select bc_sign.* from bc_sign WITH(NOLOCK) LEFT JOIN bc_patients WITH(NOLOCK)  ON bc_patients.bc_bar_no = bc_sign.bc_bar_no " + where + " ");
                    table.TableName = "bc_sign";
                    ds.Tables.Add(table);

                }

                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(string.Format("Search,sql= {0}", sql), ex);
                throw;
            }
        }

        public override string SearchSQL
        {
            get
            {
                string appsql = " 0 as com_line_color,";
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_ShowComLineStatus") == "是")
                {
                    appsql = @" com_line_color = (select top 1 (isnull(dict_combine.com_line_color,0))
			    from bc_cname 
			    left join dict_combine on bc_cname.bc_lis_code = dict_combine.com_id
			    where bc_cname.bc_bar_code = bc_patients.bc_bar_code and (com_line_status is not null or com_line_status<>'')), ";
                }

                return string.Format(@"
select
      bc_patients.bc_id,
      bc_patients.bc_bar_no,
      bc_patients.bc_bar_code,
      bc_patients.bc_frequency,
      bc_patients.bc_no_id,
      bc_patients.bc_in_no,
      bc_patients.bc_bed_no,
      bc_patients.bc_name,
      bc_patients.bc_sex,
      bc_patients.bc_age,
      bc_patients.bc_d_code,
      bc_patients.bc_d_name,
      bc_patients.bc_diag,
      bc_patients.bc_doct_code,
      bc_patients.bc_doct_name,
      bc_patients.bc_his_name,
      bc_patients.bc_sam_id,
      bc_patients.bc_sam_name,
      bc_patients.bc_date,
      bc_patients.bc_times,
      bc_patients.bc_print_flag,
      bc_patients.bc_print_date,
      bc_patients.bc_print_code,
      bc_patients.bc_print_name,
      bc_patients.bc_cuv_code,
      bc_patients.bc_cuv_name,
      bc_patients.bc_cap_sum,
      bc_patients.bc_cap_unit,
      bc_patients.bc_urgent_flag,
      bc_patients.bc_ctype,
      bc_patients.bc_ori_id,
      bc_patients.bc_ori_name,
      bc_patients.bc_receiver_flag,
      bc_patients.bc_receiver_date,
      bc_patients.bc_receiver_code,
      bc_patients.bc_receiver_name,
      bc_patients.bc_blood_flag,
      bc_patients.bc_blood_date,
      bc_patients.bc_blood_code,
      bc_patients.bc_blood_name,
      bc_patients.bc_send_flag,
      bc_patients.bc_send_date,
      bc_patients.bc_send_code,
      bc_patients.bc_send_name,
      bc_patients.bc_reach_flag,
      bc_patients.bc_reach_date,
      bc_patients.bc_reach_code,
      bc_patients.bc_reach_name,
      bc_patients.bc_exp,
      bc_patients.bc_computer,
      bc_patients.bc_social_no,
      bc_patients.bc_emp_id,
      bc_patients.bc_info,
      bc_patients.bc_hospital_id,
      bc_patients.bc_bar_type,
      bc_patients.bc_status,
      bc_patients.bc_status_cname,
      bc_patients.bc_del,
      bc_patients.bc_birthday,
      bc_patients.bc_blood,
      bc_patients.bc_occ_date,
      bc_patients.bc_sam_rem_id,
      bc_patients.bc_sam_rem_name,
      bc_patients.bc_return_flag,
      bc_patients.bc_print_time,
      bc_patients.bc_lastaction_time,
      bc_patients.bc_return_times,
      bc_patients.bc_address,
      bc_patients.bc_tel,
      bc_patients.bc_pid,
      bc_patients.bc_emp_company,
      bc_patients.bc_secondsign_date,
      bc_patients.bc_secondsign_code,
      bc_patients.bc_secondsign_name,
      bc_patients.bc_secondsign_flag,
      bc_patients.bc_app_no,
      bc_patients.bc_fee_type,
      bc_patients.bc_sam_dest,
      bc_patients.bc_batch,
      bc_patients.bc_emp_company_name,
      bc_patients.bc_lastaction_ope_code,
      bc_patients.bc_lastaction_ope_place,
      bc_patients.bc_secondsend_flag,
      bc_patients.bc_emp_company_dept,
      bc_patients.bc_barcode_type,
      bc_patients.bc_pre_sendflag,
      bc_patients.bc_oldlis_barcode,
      bc_patients.bc_notice,
      bc_patients.bc_upid,
      bc_patients.bc_merge_comid,
      bc_patients.bc_fio2,
      bc_patients.bc_tempt,
      bc_patients.bc_batch_barcode,
      bc_patients.bc_identity,
      bc_patients.bc_urgent_status,
      bc_patients.bc_out_flag,
      dict_type.type_id,
      dict_type.type_name,
      dict_type.type_exp,
      dict_type.type_py,
      dict_type.type_wb,
      dict_type.type_seq,
      dict_type.type_del,
      dict_type.type_flag,
      dict_type.tyep_hospitalName,
      dict_type.type_link,
      dict_origin.ori_id,
      dict_origin.ori_name,
      dict_origin.ori_incode,
      dict_origin.ori_py,
      dict_origin.ori_wb,
      dict_origin.ori_seq,
      dict_origin.ori_del,
      dict_cuvette.cuv_code,
      dict_cuvette.cuv_name,
      dict_cuvette.cuv_flag,
      dict_cuvette.cuv_min_no,
      dict_cuvette.cuv_max_no,
      dict_cuvette.cuv_seq,
      dict_cuvette.cuv_py,
      dict_cuvette.cuv_wb,
      dict_cuvette.cuv_del,
      dict_cuvette.cuv_max_capacity,
      dict_cuvette.cuv_unit,
      dict_cuvette.cuv_max_combine,
      dict_cuvette.cuv_fee_code,
      dict_cuvette.cuv_pri,
      dict_cuvette.cuv_color,
      dict_sample.sam_id,
      dict_sample.sam_name,
      dict_sample.sam_code,
      dict_sample.sam_incode,
      dict_sample.sam_type,
      dict_sample.sam_py,
      dict_sample.sam_wb,
      dict_sample.sam_seq,
      dict_sample.sam_del,
      dict_sample.sam_custom_type,
      dict_sample.sam_trans_code,
      dict_sample_remarks.rem_id,
      dict_sample_remarks.rem_cont,
      dict_sample_remarks.rem_py,
      dict_sample_remarks.rem_wb,
      dict_sample_remarks.rem_incode,
      dict_sample_remarks.res_newborn,
    (case when bc_patients.bc_info='122' then '手工' else '下载' end) as 'barcode_create_type',--条码生成类型
    (case when bc_patients.bc_upid='2' then '成功' when bc_patients.bc_upid='1' then '失败' when bc_patients.bc_upid='0' then '取消' else '' end) as bc_feeflag,
    dbo.getAge(bc_patients.bc_age) barcode_age,
    isnull(dict_type.type_name,'') barcode_type_name,
    isnull(dict_type.type_exp,'') barcode_type_exp,
    dict_origin.ori_name barcode_ori_name,
    dict_sample.sam_name barcode_sam_name,
    '' as ComCapSum,
    dict_type.type_exp,
    {0}
    (case isnull(bc_patients.bc_exp,'') when '' then isnull(dict_sample_remarks.rem_cont,'') else bc_patients.bc_exp end ) barcode_sample_remarks,
    isnull(dict_cuvette.cuv_name,'') barcode_cuv_name,
    bc_price = 0
    --,bc_secondsend_time = (select top 1 bc_date from bc_sign where bc_sign.bc_bar_code = bc_bar_code and bc_status = '8' order by bc_date desc)
from bc_patients  with(nolock)
    LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id LEFT OUTER JOIN 
    dict_origin on bc_patients.bc_ori_id = dict_origin.ori_id LEFT OUTER JOIN  
    dict_cuvette ON bc_patients.bc_cuv_code = dict_cuvette.cuv_code  LEFT OUTER JOIN 
    dict_sample ON bc_patients.bc_sam_id = dict_sample.sam_id LEFT OUTER JOIN
    dict_sample_remarks ON bc_patients.bc_sam_rem_id = dict_sample_remarks.rem_id 
", appsql);

            }
        }

        /// <summary>
        /// 析分条码
        /// </summary>      
        public override DataSet doOther(DataSet dsData)
        {
            //系统配置：条码号使用校验位
            string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");

            DataSet dsResult = new DataSet();
            DataTable dtReturn = new DataTable("result");
            dtReturn.Columns.Add("NewBarcodeID");
            DataTable table = dsData.Tables["Separate"];
            if (table.IsEmpty())
                return new DataSet();

            if (table != null && table.Columns.Contains("isMergeNwBarcode")
                && table.Rows.Count > 0 && table.Select(" isMergeNwBarcode='1' ").Length > 0)
            {
                #region 多项目合并成一个新条码

                bool isPreplace = table.Rows[0]["isPreplace"].ToString() == "1" ? true : false;//是否预置条码
                string barcodeID = table.Rows[0]["BarcodeID"].ToString();

                List<string> itemIDs = new List<string>();//要合并的项目集
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    itemIDs.Add(table.Rows[i]["itemID"].ToString());
                }

                string new_bc_bar_no = "";
                if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                {
                    //滨海模块-生成条码号
                    new_bc_bar_no = CreateBarcodeNumberForBhMode(null, null, barcodeID, null);
                }
                else
                {
                    new_bc_bar_no = GetNewBarcode();
                }

                string new_bc_bar_code = string.Empty;
                if (!isPreplace)
                {
                    new_bc_bar_code = new_bc_bar_no;
                }

                //拆分条码多项目合成一新条码
                if (NewBarcodeForMerge(barcodeID, new_bc_bar_no, new_bc_bar_code, itemIDs) == true)
                {
                    foreach (string itemID in itemIDs)
                    {
                        //更新明细表  要判断是预置条码还是自动条码               
                        int result = new BCCNameBIZ().Update(string.Format(" {0} = '{1}' , {2} = '{3}' ", BarcodeTable.CName.BarcodeNumber, new_bc_bar_no, BarcodeTable.CName.BarcodeDisplayNumber, new_bc_bar_no),
                                string.Format(" {0} = {1} ", BarcodeTable.CName.ID, itemID));

                        if (result > 0)
                        {
                            //如果已经存在此条码号，则不添加
                            if (dtReturn != null && dtReturn.Rows.Count > 0 && dtReturn.Select("NewBarcodeID='" + new_bc_bar_no + "'").Length > 0)
                            {
                                continue;
                            }
                            else
                            {
                                dtReturn.Rows.Add(new_bc_bar_no);
                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region 各自生成新条码

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    bool isPreplace = table.Rows[i]["isPreplace"].ToString() == "1" ? true : false;//是否预置条码
                    string barcodeID = table.Rows[i]["BarcodeID"].ToString();
                    string itemID = table.Rows[i]["itemID"].ToString();

                    string new_bc_bar_no = "";
                    if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                    {
                        //滨海模块-生成条码号
                        new_bc_bar_no = CreateBarcodeNumberForBhMode(null, null, barcodeID, null);
                    }
                    else
                    {
                        new_bc_bar_no = GetNewBarcode();
                    }

                    string new_bc_bar_code = string.Empty;
                    if (!isPreplace)
                    {
                        new_bc_bar_code = new_bc_bar_no;
                    }

                    //根据旧条码数据插入新的条码
                    if (NewBarcode(barcodeID, new_bc_bar_no, new_bc_bar_code, itemID) == false)
                        continue;

                    //更新明细表  要判断是预置条码还是自动条码               
                    int result = new BCCNameBIZ().Update(string.Format(" {0} = '{1}' , {2} = '{3}' ", BarcodeTable.CName.BarcodeNumber, new_bc_bar_no, BarcodeTable.CName.BarcodeDisplayNumber, new_bc_bar_no),
                            string.Format(" {0} = {1} ", BarcodeTable.CName.ID, itemID));

                    if (result > 0)
                    {
                        dtReturn.Rows.Add(new_bc_bar_no);
                    }
                }

                #endregion
            }

            dsResult.Tables.Add(dtReturn);
            return dsResult;

        }

        private bool NewBarcode(string barcodeID, string new_bc_bar_no, string new_bc_bar_code, string itemID)
        {
            string strNwExpression = "";//新备注信息--滨海要更新

            bool isSetNwExpression = false;

            BarcodeBIZ barcodeBIZ = new BarcodeBIZ();

            BarcodePatients bcPatient = barcodeBIZ.FindBarcode(barcodeID);

            BarcodeCombinesRecodes cname = barcodeBIZ.FindCName(itemID);

            #region 获取医嘱信息

            //获取医嘱信息
            if (cname != null
                && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "深圳滨海医院")
            {
                object objExpression = new SqlHelper().ExecuteScalar(string.Format(@"if exists (select TOP 1 * from information_schema.columns where TABLE_NAME='bc_barcode_mid' and COLUMN_NAME='bc_sample_remark')
begin
select top 1 bc_sample_remark from bc_barcode_mid where bc_order_id='{0}'
end 
else
begin
select '' as bc_sample_remark
end", cname.Orderid));

                if (objExpression != null)
                {
                    strNwExpression = objExpression.ToString();
                    isSetNwExpression = true;
                }
            }
            #endregion

            string oldName = bcPatient.HisName;
            float id = bcPatient.Id;
            bcPatient.HisName = cname.ComName; //更新

            bcPatient.Code = new_bc_bar_code;
            //if (bcPatient.BarcodeType == 0)  //如果是预置条码
            bcPatient.Number = new_bc_bar_no;

            bcPatient.Status = "0";
            bcPatient.StatusCname = "未打印";
            bcPatient.PrinterCode = "";
            bcPatient.PrinterName = "";
            bcPatient.PrintFlag = 0;
            bcPatient.ReachCode = "";
            bcPatient.ReachName = "";
            bcPatient.ReachFlag = 0;
            bcPatient.ReceiverCode = "";
            bcPatient.ReceiverName = "";
            bcPatient.ReceiverFlag = 0;
            bcPatient.SendCode = "";
            bcPatient.SendName = "";
            bcPatient.SendFlag = 0;
            bcPatient.BloodCode = "";
            bcPatient.BloodName = "";
            bcPatient.BloodFlag = 0;
            if (isSetNwExpression)
            {
                bcPatient.Expression = strNwExpression;//更新备注信息
            }

            bool updateResult = barcodeBIZ.AddBarcodePatient(bcPatient);

            new SqlHelper().ExecuteNonQuery(string.Format(@"insert into bc_sign(bc_date,bc_status,bc_bar_no,bc_bar_code,bc_remark,bc_flow) values
                                                                               (getdate(),'0','{1}','{1}','拆分生成新条码',1)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), new_bc_bar_no));

            //填充原bc_upid到新条码号
            new SqlHelper().ExecuteNonQuery(string.Format(@"update bc_patients set bc_upid=(select top 1 bc_upid from bc_patients where bc_id={0})
where bc_bar_no='{1}'", barcodeID, new_bc_bar_no));

            string strInterMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");

            if (strInterMode == "惠州六院" && (bcPatient.Originid == "108" || bcPatient.Originid == "107"))
            {
                Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();
                ser.LabEpisodeNo(barcodeID, "", "lis");
                ser.LabEpisodeNo(new_bc_bar_no, "", "lis");
            }

            if (updateResult) //更新原来的条码名称
            {
                string newHisName = BarcodeHelper.RemoveCombineName(oldName, cname.ComName);
                new BCPatientBIZ().Update(string.Format(" {0} = '{1}' ", BarcodeTable.Patient.Item, newHisName),
                  string.Format(" {0} = {1} ", BarcodeTable.Patient.ID, id));
            }

            return updateResult;
        }

        /// <summary>
        /// 拆分条码多项目合成一新条码
        /// </summary>
        /// <param name="barcodeID"></param>
        /// <param name="new_bc_bar_no"></param>
        /// <param name="new_bc_bar_code"></param>
        /// <param name="itemIDs"></param>
        /// <returns></returns>
        private bool NewBarcodeForMerge(string barcodeID, string new_bc_bar_no, string new_bc_bar_code, List<string> itemIDs)
        {
            BarcodeBIZ barcodeBIZ = new BarcodeBIZ();

            BarcodePatients bcPatient = barcodeBIZ.FindBarcode(barcodeID);

            //条码组合集
            List<BarcodeCombinesRecodes> cnames = new List<BarcodeCombinesRecodes>();
            string strNwHisName = "";//新条码的组合名称
            string strNwExpression = "";//新备注信息--滨海要更新
            bool isSetNwExpression = false;

            int tempIndex = 0;

            foreach (string itemID in itemIDs)
            {
                tempIndex++;
                BarcodeCombinesRecodes cname = barcodeBIZ.FindCName(itemID);
                cnames.Add(cname);

                #region 每次获取第一条医嘱信息

                //每次获取第一条医嘱信息
                if (tempIndex == 1
                    && cname != null
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "深圳滨海医院")
                {
                    object objExpression = new SqlHelper().ExecuteScalar(string.Format(@"if exists (select TOP 1 * from information_schema.columns where TABLE_NAME='bc_barcode_mid' and COLUMN_NAME='bc_sample_remark')
begin
select top 1 bc_sample_remark from bc_barcode_mid where bc_order_id='{0}'
end 
else
begin
select '' as bc_sample_remark
end", cname.Orderid));

                    if (objExpression != null)
                    {
                        strNwExpression = objExpression.ToString();
                        isSetNwExpression = true;
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(strNwHisName))
                {
                    strNwHisName = cname.ComName;
                }
                else
                {
                    strNwHisName += "+" + cname.ComName;
                }
            }

            string oldName = bcPatient.HisName;
            float id = bcPatient.Id;
            bcPatient.HisName = strNwHisName; //更新

            bcPatient.Code = new_bc_bar_code;
            //if (bcPatient.BarcodeType == 0)  //如果是预置条码
            bcPatient.Number = new_bc_bar_no;

            bcPatient.Status = "0";
            bcPatient.StatusCname = "未打印";
            bcPatient.PrinterCode = "";
            bcPatient.PrinterName = "";
            bcPatient.PrintFlag = 0;
            bcPatient.ReachCode = "";
            bcPatient.ReachName = "";
            bcPatient.ReachFlag = 0;
            bcPatient.ReceiverCode = "";
            bcPatient.ReceiverName = "";
            bcPatient.ReceiverFlag = 0;
            bcPatient.SendCode = "";
            bcPatient.SendName = "";
            bcPatient.SendFlag = 0;
            bcPatient.BloodCode = "";
            bcPatient.BloodName = "";
            bcPatient.BloodFlag = 0;
            if (isSetNwExpression)
            {
                bcPatient.Expression = strNwExpression;//更新备注信息
            }

            bool updateResult = barcodeBIZ.AddBarcodePatient(bcPatient);

            new SqlHelper().ExecuteNonQuery(string.Format(@"insert into bc_sign(bc_date,bc_status,bc_bar_no,bc_bar_code,bc_remark,bc_flow) values
                                                                               (getdate(),'0','{1}','{1}','拆分生成新条码',1)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), new_bc_bar_no));

            //填充原bc_upid到新条码号
            new SqlHelper().ExecuteNonQuery(string.Format(@"update bc_patients set bc_upid=(select top 1 bc_upid from bc_patients where bc_id={0})
where bc_bar_no='{1}'", barcodeID, new_bc_bar_no));

            string strInterMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");

            if (strInterMode == "惠州六院" && (bcPatient.Originid == "108" || bcPatient.Originid == "107"))
            {
                Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();
                ser.LabEpisodeNo(barcodeID, "", "lis");
                ser.LabEpisodeNo(new_bc_bar_no, "", "lis");
            }

            if (updateResult) //更新原来的条码名称
            {
                string newHisName = oldName;

                foreach (BarcodeCombinesRecodes cname in cnames)
                {
                    //移除新条码的组合信息
                    newHisName = BarcodeHelper.RemoveCombineName(newHisName, cname.ComName);
                }

                new BCPatientBIZ().Update(string.Format(" {0} = '{1}' ", BarcodeTable.Patient.Item, newHisName),
                  string.Format(" {0} = {1} ", BarcodeTable.Patient.ID, id));
            }

            return updateResult;
        }

        private string GetNewBarcode()
        {
            string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");

            string barcode = new BCBarcodeBIZ().GetNewBarcode();
            IBarcodeGenerateRule rule = new DefaultGenerateRule();

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
                    return GetNewBarcode();
                return newbarcode;
            }
        }

        bool IsExistsBarcode(string barcode)
        {
            string sql = string.Format("select count(1) from bc_patients with(NOLOCK) where bc_bar_code='{0}'", barcode);

            SqlHelper helper = new SqlHelper();

            object ob = helper.ExecuteScalar(sql);

            if (ob != null && ob != DBNull.Value && Convert.ToInt32(ob) > 0)
                return true;

            return false;

        }

        /// <summary>
        /// 生成条码号-滨海模式
        /// </summary>
        /// <param name="com_ctype">物理组</param>
        /// <param name="sel_barcode">用条码号查物理组</param>
        /// <param name="sel_barcodeID">bc_patients.bc_id</param>
        /// /// <param name="sel_comID">组合id</param>
        /// <returns></returns>
        private string CreateBarcodeNumberForBhMode(string com_ctype, string sel_barcode, string sel_barcodeID, string sel_comID)
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
                    string strCtypeSort = "99";//2位物理组编码(默认为99)
                    string strPtypeSort = "0";//1位专业组编码(默认为0)

                    if (!string.IsNullOrEmpty(com_ctype) || !string.IsNullOrEmpty(sel_barcode) || !string.IsNullOrEmpty(sel_barcodeID))
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
                        else if (!string.IsNullOrEmpty(sel_barcodeID))
                        {
                            dtTempctype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id=(
select top 1 bc_ctype from bc_patients with(NOLOCK) where bc_id={0} )", sel_barcodeID));
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

                    if (!string.IsNullOrEmpty(sel_comID) || !string.IsNullOrEmpty(sel_barcode) || !string.IsNullOrEmpty(sel_barcodeID))
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
                        else if (!string.IsNullOrEmpty(sel_barcodeID))
                        {
                            dtTempptype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='0' and type_id=(
select top 1 com_ptype from dict_combine with(NOLOCK) where com_id=(
select top 1 bc_lis_code from bc_cname with(NOLOCK) where bc_del='0' and bc_bar_no=(
select top 1 bc_bar_no from bc_patients with(NOLOCK) where bc_id={0}
)))", sel_barcodeID));
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

                    if (!string.IsNullOrEmpty(com_ctype) || !string.IsNullOrEmpty(sel_barcode) || !string.IsNullOrEmpty(sel_barcodeID))
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
                        else if (!string.IsNullOrEmpty(sel_barcodeID))
                        {
                            dtTempctype = helper.GetTable(string.Format(@"select type_id,type_exp,type_seq from dict_type with(NOLOCK) where type_flag='1' and type_id=(
select top 1 bc_ctype from bc_patients with(NOLOCK) where bc_id={0} )", sel_barcodeID));
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

        public override DataSet doNew(DataSet dsData)
        {
            //Dictionary<string, List<EntityDictCombineBar>> splited_combines = new CombineSplitMergeBIZ("107").SplitAndMerge(coms.ToArray());

            DataSet result = new DataSet();
            try
            {
                DataTable tbPatients = dsData.Tables[MainTable];
                DataTable tbCName = dsData.Tables[BarcodeTable.CName.TableName];
                DataTable tbBcSign = dsData.Tables["bc_sign"];

                //是否自动合并与拆分
                bool bAutoSplitCombine = true;
                if (tbPatients.Columns.Contains("AutoSplitCombine"))
                {
                    bAutoSplitCombine = (tbPatients.Rows[0]["AutoSplitCombine"].ToString() == "1");
                }

                if (!tbPatients.Columns.Contains("bc_cap_sum"))
                {
                    tbPatients.Columns.Add("bc_cap_sum", typeof(decimal));
                }

                if (!tbPatients.Columns.Contains("bc_print_time"))
                {
                    tbPatients.Columns.Add("bc_print_time", typeof(int));
                }

                //需要插入的bc_patients与bc_cname
                DataTable tbBcPatientsToInsert = tbPatients.Clone();
                DataTable tbBcCNameToInsert = tbCName.Clone();
                DataTable tbBcSignToInsert = tbBcSign.Clone();

                //获取来源类型
                string bc_ori_id = tbPatients.Rows[0]["bc_ori_id"].ToString();

                List<string> coms_id = new List<string>();
                foreach (DataRow row in tbCName.Rows)//遍历每一个组合得到所有的项目id
                {
                    coms_id.Add(row["bc_lis_code"].ToString());
                }

                DataTable dtErro = new DataTable("erro");
                dtErro.Columns.Add("erro");


                DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                //if (splited_combines.Count == 0)
                //{
                //    dtErro.Rows.Add("组合字典未设置拆分码，请在项目字典中添加对应的信息.");
                //    DataSet dsErro = new DataSet();
                //    dsErro.Tables.Add(dtErro);
                //    return dsErro;
                //}

                //获取条码类型：预置、自动
                string barcode_type = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("7");

                //系统配置：条码号使用校验位
                string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");

                if (bAutoSplitCombine)
                {
                    //根据来源与项目id得到拆分合并后的组合
                    //<拆分码,dict_combine_bar信息>
                    Dictionary<string, List<EntityDictCombineBar>> splited_combines = new CombineSplitMergeBIZ(bc_ori_id).SplitAndMerge(coms_id.ToArray());

                    #region 自动拆分合并条码
                    foreach (string com_split_code in splited_combines.Keys)//遍历每一个合并码生成一个条码
                    {
                        DataRow row = tbPatients.Rows[0];
                        DataRow rowBcSign = tbBcSign.Rows[0];

                        string bc_bar_code = row["bc_bar_code"].ToString().Trim();
                        string bc_bar_no = "";//新条码

                        if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                        {
                            #region 条码生成规则-滨海模式

                            string temp_ctype_id = "";
                            string temp_comID = "";

                            foreach (EntityDictCombineBar com_bar in splited_combines[com_split_code])
                            {
                                temp_ctype_id = com_bar.com_exec_code;//物理组id
                                temp_comID = com_bar.com_id;//组合id
                            }
                            //滨海模块-生成条码号
                            bc_bar_no = CreateBarcodeNumberForBhMode(temp_ctype_id, null, null, temp_comID);

                            #endregion
                        }
                        else
                        {
                            bc_bar_no = GetNewBarcode();
                        }

                        if (!string.IsNullOrEmpty(bc_bar_code))
                        {
                            BarcodeBIZ barcodeBiz = new BarcodeBIZ();
                            if (!barcodeBiz.GetPrePlaceBarcode(row["bc_bar_code"].ToString()))
                            {
                                //bc_barcode = row["bc_bar_code"].ToString().Trim();
                                bc_bar_no = bc_bar_code;
                            }
                            else
                            {
                                dtErro.Rows.Add("此试管已被登记.");
                                DataSet dsErro = new DataSet();
                                dsErro.Tables.Add(dtErro);
                                return dsErro;
                            }
                        }
                        else
                        {
                            bc_bar_code = bc_bar_no;
                        }



                        rowBcSign["bc_bar_no"] = bc_bar_no;
                        rowBcSign["bc_bar_code"] = bc_bar_code;
                        rowBcSign["bc_date"] = now;
                        rowBcSign["bc_status"] = "0";

                        string bc_his_name = string.Empty;//his名
                        //string bc_notice = string.Empty;
                        //string bc_name = string.Empty;//lis名

                        bool needPlus = false;//是否需要加号拼接
                        string urgentflag = "0";

                        //计算此试管所有组合采血量所需数据

                        decimal decDeadspace_volume = 0;//死腔体积
                        decimal decCupSum = 0;//采血量
                        //保存死腔体积容器，每台仪器只计算该仪器最大一个死腔体积
                        Dictionary<string, decimal> dictItrValue = new Dictionary<string, decimal>();

                        foreach (EntityDictCombineBar com_bar in splited_combines[com_split_code])
                        {
                            //遍历拆分码中的每一个组合
                            row["bc_sam_id"] = com_bar.com_sam_id;//设置条码信息的标本id
                            row["bc_cuv_code"] = com_bar.com_cuv_code;//设置条码信息的试管id

                            if (row.Table.Columns.Contains("bc_merge_comid")
                                && string.IsNullOrEmpty(row["bc_merge_comid"].ToString())
                                && !string.IsNullOrEmpty(com_bar.MergeComID))
                            {
                                //如果‘特殊项目合并大组合ID’不为空,则保存
                                row["bc_merge_comid"] = com_bar.MergeComID;
                            }

                            //打印次数
                            if (row.Table.Columns.Contains("bc_print_time"))
                            {
                                if (com_bar.com_print_count != null
                                    && com_bar.com_print_count.Value != null
                                    && com_bar.com_print_count.Value > 0)
                                {
                                    row["bc_print_time"] = com_bar.com_print_count.Value;
                                }
                                else
                                {
                                    row["bc_print_time"] = 1;
                                }
                            }

                            DataRow rowCName = tbBcCNameToInsert.NewRow();

                            rowCName["bc_bar_no"] = bc_bar_no;
                            rowCName["bc_bar_code"] = bc_bar_code;
                            rowCName["bc_lis_code"] = com_bar.com_id;

                            if (urgentflag != "1")
                            {
                                DataRow[] rowsBcLisCode = tbCName.Select(string.Format("bc_lis_code = '{0}'", com_bar.com_id));
                                if (rowsBcLisCode.Length > 0 && Convert.ToBoolean(rowsBcLisCode[0]["urgentflag"]))
                                {
                                    urgentflag = "1";
                                }
                            }

                            EntityDictCombine ett_com = DictCombineCache.Current.GetCombineByID(com_bar.com_id, true);
                            //获取组合默认仪器ID

                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "深圳沙井医院")
                            {
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("barcode_CalculateCupSum") == "是")
                                {
                                    decCupSum += Convert.ToDecimal(com_bar.com_cap_sum);
                                    //以仪器为单位，将组合死腔体积加入容器，
                                    if (dictItrValue.ContainsKey(ett_com.com_itr_id))
                                    {
                                        decimal decVule = dictItrValue[ett_com.com_itr_id];
                                        //同仪器的组合死腔体积大的替换比较小的死腔体积
                                        if (Convert.ToDecimal(com_bar.com_deadspace_volume) > decVule)
                                        {
                                            dictItrValue[ett_com.com_itr_id] = Convert.ToDecimal(com_bar.com_deadspace_volume);
                                        }
                                    }
                                    else
                                    {
                                        dictItrValue.Add(ett_com.com_itr_id, Convert.ToDecimal(com_bar.com_deadspace_volume));
                                    }
                                }
                            }

                            rowCName["bc_name"] = ett_com.com_name;
                            rowCName["bc_his_name"] = ett_com.com_his_name;

                            rowCName["bc_apply_date"] = now.ToString("yyyy-MM-dd HH:mm:ss");
                            rowCName["bc_occ_date"] = now.ToString("yyyy-MM-dd HH:mm:ss");
                            //rowCName["bc_receiver_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //rowCName["bc_blood_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //rowCName["bc_send_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //rowCName["bc_reach_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            tbBcCNameToInsert.Rows.Add(rowCName.ItemArray);


                            //if (bc_notice == string.Empty && ett_com.com_rem != null && ett_com.com_rem.Trim() != string.Empty)
                            //    bc_notice = ett_com.com_rem;

                            if (needPlus)
                            {
                                bc_his_name += "+";
                                //bc_name += "+";
                            }

                            //bc_name += ett_com.com_name;

                            //如果his名为空则取lis名
                            if (1 != 1)//(!string.IsNullOrEmpty(ett_com.com_his_name))
                            {
                                //bc_his_name += ett_com.com_his_name;
                            }
                            else
                            {
                                bc_his_name += ett_com.com_name;
                            }

                            needPlus = true;
                            row["bc_ctype"] = com_bar.com_exec_code;
                        }

                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "深圳沙井医院")
                        {
                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("barcode_CalculateCupSum") == "是")
                            {
                                //将计算出来的采血量统计
                                foreach (string key in dictItrValue.Keys)
                                {
                                    decDeadspace_volume += dictItrValue[key];
                                }
                                decimal decTalCupSum = (decDeadspace_volume + decCupSum) * Convert.ToDecimal(4);
                                string strSamName = dcl.svr.cache.DictSampleCache.Current.GetSamCNameByID(row["bc_sam_id"].ToString());
                                if (!string.IsNullOrEmpty(strSamName) && strSamName.IndexOf("血清") >= 0)
                                {
                                    if (!row.Table.Columns.Contains("bc_cap_sum"))
                                    {
                                        row.Table.Columns.Add("bc_cap_sum", typeof(decimal));
                                    }

                                    //记录具体采血量
                                    row["bc_cap_sum"] = decTalCupSum;
                                    if (decTalCupSum <= 3)
                                    {
                                        //采用3ml试管

                                        row["bc_cuv_code"] = "6";

                                    }
                                    else if (decTalCupSum <= 5)
                                    {
                                        //采用5ml试管

                                        row["bc_cuv_code"] = "10028";
                                    }
                                    else
                                    {
                                        //采用8ml试管

                                        row["bc_cuv_code"] = "10029";
                                    }
                                }
                            }

                        }

                        row["bc_his_name"] = bc_his_name;
                        //Logger.WriteException("bc_notice", "", "设置bc_notice" + bc_notice);
                        //row["bc_notice"] = bc_notice;
                        DataRow rowBcPatientsToInsert = tbBcPatientsToInsert.Rows.Add(row.ItemArray);

                        rowBcPatientsToInsert["bc_bar_type"] = 0;
                        rowBcPatientsToInsert["bc_bar_no"] = bc_bar_no;
                        rowBcPatientsToInsert["bc_bar_code"] = bc_bar_code;
                        rowBcPatientsToInsert["bc_urgent_flag"] = urgentflag;

                        foreach (DataRow rowCName in tbBcCNameToInsert.Rows)
                        {
                            if (rowCName["urgentflag"].ToString() != string.Empty
                                && Convert.ToBoolean(rowCName["urgentflag"]))
                            {
                                rowBcPatientsToInsert["bc_urgent_flag"] = "1";
                                break;
                            }
                        }

                        tbBcSignToInsert.Rows.Add(rowBcSign.ItemArray);
                    }
                    #endregion
                }
                else
                {
                    #region 不拆分和合并条码
                    DataRow row = tbPatients.Rows[0];
                    DataRow rowBcSign = tbBcSign.Rows[0];

                    string bc_bar_code = row["bc_bar_code"].ToString().Trim();
                    string bc_bar_no = "";//新条码

                    if (!string.IsNullOrEmpty(chkcodetype) && chkcodetype == "滨海模式")
                    {
                        #region 生成条码规则-滨海模式

                        string temp_ctype_id = "";
                        string temp_comID = "";//组合id
                        foreach (string comid in coms_id)
                        {
                            EntityDictCombineBar com_bar = DictCombineBarCache.Current.GetCombineBarWithComID(comid, bc_ori_id);
                            if (com_bar != null)
                            {
                                temp_ctype_id = com_bar.com_exec_code;
                                temp_comID = com_bar.com_id;//组合id
                            }
                        }

                        //滨海模块-生成条码号
                        bc_bar_no = CreateBarcodeNumberForBhMode(temp_ctype_id, null, null, temp_comID);

                        #endregion
                    }
                    else
                    {
                        bc_bar_no = GetNewBarcode();
                    }

                    if (!string.IsNullOrEmpty(bc_bar_code))
                    {
                        BarcodeBIZ barcodeBiz = new BarcodeBIZ();
                        if (!barcodeBiz.GetPrePlaceBarcode(row["bc_bar_code"].ToString()))
                        {
                            //bc_barcode = row["bc_bar_code"].ToString().Trim();
                        }
                        else
                        {
                            dtErro.Rows.Add("此试管已被登记.");
                            DataSet dsErro = new DataSet();
                            dsErro.Tables.Add(dtErro);
                            return dsErro;
                        }
                    }
                    else
                    {
                        bc_bar_code = bc_bar_no;
                    }

                    row["bc_bar_no"] = bc_bar_no;
                    row["bc_bar_code"] = bc_bar_code;

                    rowBcSign["bc_bar_no"] = bc_bar_no;
                    rowBcSign["bc_bar_code"] = bc_bar_code;
                    rowBcSign["bc_date"] = now;
                    rowBcSign["bc_status"] = "0";

                    string bc_his_name = string.Empty;//his名
                    //string bc_name = string.Empty;//lis名

                    bool needPlus = false;//是否需要加号拼接

                    string urgentflag = "0";
                    foreach (string comid in coms_id)
                    {
                        EntityDictCombineBar com_bar = DictCombineBarCache.Current.GetCombineBarWithComID(comid, bc_ori_id);
                        EntityDictCombine ett_com = DictCombineCache.Current.GetCombineByID(comid, true);
                        if (com_bar != null)
                        {
                            row["bc_sam_id"] = com_bar.com_sam_id;//设置条码信息的标本id
                            row["bc_cuv_code"] = com_bar.com_cuv_code;//设置条码信息的试管id
                            row["bc_ctype"] = com_bar.com_exec_code;

                            if (row.Table.Columns.Contains("bc_print_time"))
                            {
                                if (com_bar.com_print_count != null
                                    && com_bar.com_print_count.Value != null
                                    && com_bar.com_print_count.Value > 0)
                                {
                                    row["bc_print_time"] = com_bar.com_print_count.Value;
                                }
                                else
                                {
                                    row["bc_print_time"] = 1;
                                }
                            }
                        }
                        //else
                        //{
                        //}

                        //comid tbCName.sele
                        if (urgentflag != "1")
                        {
                            DataRow[] rowsBcLisCode = tbCName.Select(string.Format("bc_lis_code = '{0}'", comid));
                            if (rowsBcLisCode.Length > 0 && Convert.ToBoolean(rowsBcLisCode[0]["urgentflag"]))
                            {
                                urgentflag = "1";
                            }
                        }

                        DataRow rowCName = tbBcCNameToInsert.NewRow();

                        rowCName["bc_bar_no"] = bc_bar_no;
                        rowCName["bc_bar_code"] = bc_bar_code;
                        rowCName["bc_lis_code"] = comid;



                        rowCName["bc_name"] = ett_com.com_name;
                        rowCName["bc_his_name"] = ett_com.com_his_name;

                        rowCName["bc_apply_date"] = now.ToString("yyyy-MM-dd HH:mm:ss");
                        rowCName["bc_occ_date"] = now.ToString("yyyy-MM-dd HH:mm:ss");
                        //rowCName["bc_receiver_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //rowCName["bc_blood_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //rowCName["bc_send_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //rowCName["bc_reach_date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        tbBcCNameToInsert.Rows.Add(rowCName.ItemArray);

                        if (needPlus)
                        {
                            bc_his_name += "+";
                            //bc_name += "+";
                        }

                        //bc_name += ett_com.com_name;

                        //如果his名为空则取lis名
                        if (!string.IsNullOrEmpty(ett_com.com_his_name))
                        {
                            bc_his_name += ett_com.com_his_name;
                        }
                        else
                        {
                            bc_his_name += ett_com.com_name;
                        }

                        needPlus = true;
                    }
                    row["bc_his_name"] = bc_his_name;

                    DataRow rowBcPatientsToInsert = tbBcPatientsToInsert.Rows.Add(row.ItemArray);

                    rowBcPatientsToInsert["bc_bar_type"] = 0;
                    rowBcPatientsToInsert["bc_urgent_flag"] = urgentflag;
                    //foreach (DataRow rowCName in tbBcCNameToInsert.Rows)
                    //{
                    //if (rowCName["urgentflag"].ToString() != string.Empty
                    //        && Convert.ToBoolean(rowCName["urgentflag"]))
                    //{
                    //    rowBcPatientsToInsert["bc_urgent_flag"] = "1";
                    //    break;
                    //}
                    //}

                    tbBcSignToInsert.Rows.Add(rowBcSign.ItemArray);
                    #endregion
                }

                DataSet dsCName = new DataSet();
                dsCName.Tables.Add(tbBcCNameToInsert);

                //插入bc_patients表
                ArrayList arrNew = dao.GetInsertSql(tbBcPatientsToInsert);
                dao.DoTran(arrNew);

                //插入bc_cname表
                if (dsCName.IsNotEmpty() && dsCName.Tables[0].IsNotEmpty())
                    new BCCNameBIZ().doNew(dsCName);

                //插入bc_sign
                ArrayList arrBcSign = dao.GetInsertSql(tbBcSignToInsert);
                dao.DoTran(arrBcSign);

                result.Tables.Add(tbBcPatientsToInsert.Copy());
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
            }
            return result;
        }


        public override int Update(string set, string where)
        {
            string opname = string.Empty;

            string stepcode = string.Empty;
            string barcodeList = string.Empty;
            string dt = string.Empty;
            string loginid = string.Empty;
            if (where.Contains("|") && where.Split('|').Length == 5)
            {
                stepcode = where.Split('|')[1];
                loginid = where.Split('|')[2];
                dt = where.Split('|')[3];
                barcodeList = where.Split('|')[4];
                where = where.Split('|')[0];
            }

            if (where.Contains("|") && where.Split('|').Length == 2)
            {
                opname = where.Split('|')[1];
                where = where.Split('|')[0];
            }

            int result = base.Update(set, where);

            if(!string.IsNullOrEmpty(stepcode))
            {
                new TATRecordHelper().Record(barcodeList, stepcode, loginid, dt);
            }

            if (result > 0 && MainTable.ToLower() == "bc_patients" &&
                set.Trim().Contains("bc_status = '1', bc_status_cname = '已打印'"))
            {
                string sqlUpdateSub = @"update bc_patients 
set
bc_blood_date = bc_print_date,
bc_send_date = bc_print_date,
bc_reach_date = bc_print_date,
bc_receiver_date = bc_print_date
,bc_receiver_flag = '1'
,bc_status='5'
where bc_ori_id in ('110') and (bc_status = '0' or bc_status = '1') and " + where;

                dao.DoTran(new string[] { sqlUpdateSub });




                string sql = @"
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
where bc_patients." + where;
                DataTable table = new SqlHelper().GetTable(sql);

                if (table.Rows.Count > 0)
                {
                    string ori_id = table.Rows[0]["bc_ori_id"].ToString();

                    foreach (DataRow row in table.Rows)
                    {
                        if (row["bc_del"] != null && row["bc_del"].ToString() == "1")
                            continue;
                        string advice_id = row["bc_yz_id"].ToString();
                        if (advice_id != string.Empty)
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
                                dihelper.ExecuteNonQueryWithGroup("条码_门诊_打印后", list.ToArray());
                            }
                            else if (ori_id == "108")
                            {
                                dihelper.ExecuteNonQueryWithGroup("条码_住院_打印后", list.ToArray());
                            }
                            else if (ori_id == "109")
                            {
                                dihelper.ExecuteNonQueryWithGroup("条码_体检_打印后", list.ToArray());
                            }
                        }
                    }


                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_PDABarCode") == "是")
                    {
                        DataView barcode = new DataView(table);
                        DataTable dtBarCode = barcode.ToTable(true, new string[] { "bc_bar_code" });
                        CDRService cdr = new CDRService();
                        foreach (DataRow item in dtBarCode.Rows)
                        {
                            cdr.RegBarcode(item["bc_bar_code"].ToString());
                        }

                    }
                }
            }

            //12-5-10
            //更新回退信息
            if (result > 0 && MainTable.ToLower() == "bc_patients" && ((set.Trim() == "bc_del = 1") && where.Trim().Contains("bc_bar_no") || set.Trim().Contains("bc_status =")))
            {
                try
                {
                    string sql = @"select bc_bar_code from bc_patients where bc_status<>'9' and " + where;
                    SqlHelper helper = new SqlHelper();
                    DbCommandEx cmd = helper.CreateCommandEx(sql);
                    DataTable table = helper.GetTable(cmd);
                    table.TableName = "bc_patients";

                    string whereStr = "";
                    if (table != null)
                        if (table.Rows.Count > 0)
                        {
                            foreach (DataRow dr in table.Rows)
                            {
                                string item = dr["bc_bar_code"].ToString();
                                whereStr += string.Format(",'{0}'", item);
                            }
                        }
                    if (!string.IsNullOrEmpty(whereStr))
                    {
                        whereStr = whereStr.Substring(1);

                        string sqlUpdateSub = @"UPDATE bc_return_messages
   SET 
   bc_handle_flag =1
 WHERE bc_bar_no in (" + whereStr + ")";

                        dao.DoTran(new string[] { sqlUpdateSub });
                    }
                }
                catch (Exception dtex)
                {
                }
            }


            //调用院网接口
            if (result > 0 && MainTable.ToLower() == "bc_patients" && set.Trim() == "bc_del = 1")
            {
                string sql = @"
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
where bc_patients." + where;
                DataTable table = new SqlHelper().GetTable(sql);

                if (table.Rows.Count > 0)
                {
                    string ori_id = table.Rows[0]["bc_ori_id"].ToString();
                    string opcode = string.Empty;
                    if (!string.IsNullOrEmpty(opname))
                    {
                        string[] strarry = opname.Split('&');
                        DataTable doc = DictDoctorCache.Current.GetDoctors();
                        if (doc != null && doc.Rows.Count > 0 && strarry.Length == 2)
                        {
                            DataRow[] rows =
                                doc.Select(string.Format("doc_name='{0}' ", strarry[0]));
                            if (rows.Length > 0)
                            {
                                opcode = rows[0]["doc_code"] != null
                                              ? rows[0]["doc_code"].ToString()
                                              : opcode;
                                opname = strarry[1];
                            }
                        }
                    }


                    foreach (DataRow row in table.Rows)
                    {
                        if (row["bc_del"] != null && row["bc_del"].ToString() == "1")
                            continue;
                        string advice_id = row["bc_yz_id"].ToString();
                        if (advice_id != string.Empty)
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
                            list.Add(new InterfaceDataBindingItem("op_code", opcode));
                            list.Add(new InterfaceDataBindingItem("op_name", opname));


                            DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);

                            if (ori_id == "107")
                            {
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload") == "HL7V3")
                                {
                                    Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                    eyComfirm.bc_ori_id = "107";
                                    eyComfirm.bc_order_id = advice_id;
                                    eyComfirm.OrderStatus = "30";
                                    eyComfirm.doctor_id = "-1";
                                    eyComfirm.doctor_name = "-1";
                                    new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                }

                                string gn = "条码_门诊_删除后";
                                if (ConfigurationManager.AppSettings["HospitalType"] == "1")
                                    gn = "分院_" + gn;

                                dihelper.ExecuteNonQueryWithGroup(gn, list.ToArray());
                            }
                            else if (ori_id == "108")
                            {
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload") == "HL7V3")
                                {
                                    Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                    eyComfirm.bc_ori_id = "108";
                                    eyComfirm.bc_order_id = advice_id;
                                    eyComfirm.OrderStatus = "30";
                                    eyComfirm.doctor_id = "-1";
                                    eyComfirm.doctor_name = "-1";
                                    new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                }
                                dihelper.ExecuteNonQueryWithGroup("条码_住院_删除后", list.ToArray());
                            }
                            else if (ori_id == "109")
                            {
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirmWithDowload") == "HL7V3")
                                {
                                    Lis.EntityHl7v3.EntityComfirmHl7 eyComfirm = new Lis.EntityHl7v3.EntityComfirmHl7();
                                    eyComfirm.bc_ori_id = "109";
                                    eyComfirm.bc_order_id = advice_id;
                                    eyComfirm.OrderStatus = "30";
                                    eyComfirm.doctor_id = "-1";
                                    eyComfirm.doctor_name = "-1";
                                    new Lis.SendDataByHl7v3.DataSendHelper().LisSendComfirmInvoke(eyComfirm);
                                }
                                dihelper.ExecuteNonQueryWithGroup("条码_体检_删除后", list.ToArray());
                            }
                        }
                    }

                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_PDABarCode") == "是")
                    {
                        DataView barcode = new DataView(table);
                        DataTable dtBarCode = barcode.ToTable(true, new string[] { "bc_bar_code" });
                        CDRService cdr = new CDRService();

                        List<string> listBarCode = new List<string>();

                        foreach (DataRow item in dtBarCode.Rows)
                        {
                            listBarCode.Add(item["bc_bar_code"].ToString());
                        }

                        cdr.DeleteBarCode(listBarCode);
                    }
                }
            }

            return result;
        }

        public DataTable GetPatientsInfoByBcInNo(string bc_in_no)
        {
            //手工条码病人编号查询唯一字段
            string colName = CacheSysConfig.Current.GetSystemConfig("BCManual_select_colName");

            string sql = @"select top 1 * from bc_patients where bc_in_no = ? order by bc_date desc";

            if (!string.IsNullOrEmpty(colName))
            {
                if (colName == "bc_in_no or bc_social_no")
                {
                    if (string.IsNullOrEmpty(bc_in_no)) bc_in_no = "";
                    sql = "select top 1 * from bc_patients where (bc_social_no='" + bc_in_no + "' or bc_in_no= ?) order by bc_date desc";
                }
                else
                {
                    sql = "select top 1 * from bc_patients where " + colName + "= ? order by bc_date desc";
                }
            }

            SqlHelper helper = new SqlHelper();
            DbCommandEx cmd = helper.CreateCommandEx(sql);
            cmd.AddParameterValue(bc_in_no, DbType.AnsiString);
            DataTable table = helper.GetTable(cmd);
            table.TableName = "bc_patients";
            return table;
        }
        /// <summary>
        /// 将条码登记状态插入到bc_sign表中记录下来 20120920
        /// </summary>
        public void insertBC_Remark(string login_id, string bc_name, string barCode, string departDisplayValue, string bar_sequence, DateTime registTime)
        {
            string remark = string.Format(@"接收室：{0}；编号：{1}；登记日期：{2}；", departDisplayValue, bar_sequence, registTime.ToString());
            string sql = string.Format(@"insert into bc_sign(bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_remark)
                                         values(getdate(),'{0}','{1}',{2},'{3}','{4}','{5}')", login_id, bc_name, 20, barCode, barCode, remark);

            new DBHelper().ExecuteNonQuery(sql);


        }

    }


}