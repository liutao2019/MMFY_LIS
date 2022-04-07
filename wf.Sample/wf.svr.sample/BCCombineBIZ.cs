using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.common.extensions;
using dcl.root.dac;
using dcl.common;
using dcl.svr.frame;
using dcl.common;
using dcl.svr.interfaces;
using dcl.svr.root.com;
using lis.common.extensions;

namespace dcl.svr.sample
{
    public class BCCombineBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return  "dict_combine"; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.Combine.ID; }
        }

        public override DataSet Search(string where)
        {
            return doSearch(new DataSet(), SearchSQL + where);
        }

        public override string SearchSQL
        {
            get
            {
                return string.Format(@"SELECT  dict_combine.*, dict_type.type_name AS ctype_name     
FROM dict_combine LEFT OUTER JOIN dict_type ON dict_combine.com_ptype = dict_type.type_id      
WHERE (dict_combine.com_del = '0') {0}
and dict_combine.com_id in
(select com_id from dict_combine_bar where com_split_flag='1' group by com_id)", ServerConfig.BuildTypeHospitalSqlWithAnd());
            }
        }

        /// <summary>
        /// 分析项目组合,并生成条码
        /// </summary>     
        public override DataSet doOther(DataSet dsData)
        {
            DataSet dsResult = new DataSet();
            if (dsData.IsEmpty())
                return dsResult;

            //获得自定义条码字典对照表
            ContrastHelper contrastHelper = new ContrastHelper("条码字典", new ContrastBizHelper());
            contrastHelper.GetData();
            string hisColumns = contrastHelper.HisColumns;
            string lisColumns = contrastHelper.LisColumns;

            Dictionary<string, string> sampleCodesAndBarcode = new Dictionary<string, string>();
            decimal capacitySum = 0;
            int combineSum = 0;
            string barcode = "";
            //组合ID
            List<String> combineIDs = DataTableHelper.ColumnValueToList(dsData.Tables["input"], "CombineID");

            //容器与项目
            DBHelper helper = new DBHelper();
            DataTable dtCombine = helper.GetTable(string.Format(@"select  * from dict_combine_bar, dict_cuvette where dict_combine_bar.com_his_code in {0} AND dict_combine_bar.com_cuv_code = dict_cuvette.cuv_code ", combineIDs.JoinString2(",")));
            if (dtCombine.IsEmpty())
                return dsResult;

            List<string> combineNames = new List<string>();
            for (int i = 0; i < dtCombine.Rows.Count; i++)
            {
                //His项目ID
                string combineID = dtCombine.Rows[i][BarcodeTable.Combine.HisID].ToString();
                combineNames.Add(dtCombine.Rows[i][BarcodeTable.Combine.HisName].ToString());
                //最大采集量
                string maxCapacity = dtCombine.Rows[i][BarcodeTable.Cuvette.MaxCapacity].ToString();
                if (maxCapacity.IsEmpty())
                    maxCapacity = "100000";
                //最大项目数
                string maxCombineCount = dtCombine.Rows[i][BarcodeTable.Cuvette.MaxCombineCount].ToString();
                if (maxCombineCount.IsEmpty())
                    maxCombineCount = "100";
                //项目最小采集量 
                string minCapacity = dtCombine.Rows[i][BarcodeTable.Combine.MinCapacity].ToString();
                if (minCapacity.IsEmpty())
                    minCapacity = "0";
                //标本分类码
                string classifiedCode = dtCombine.Rows[i][BarcodeTable.Combine.SampleCode].ToString();

                if (IsNotFirst(i) && ClassifiedCodeInclude(sampleCodesAndBarcode, classifiedCode) && LowThanMaxCapacity(capacitySum, Decimal.Parse(minCapacity), Decimal.Parse(maxCapacity)) && LowThanMaxCombineCount(combineSum, Int32.Parse(maxCombineCount)))
                {
                    barcode = sampleCodesAndBarcode[classifiedCode]; //获得原先已有的条码号,即与原先的项目同用一个试管
                }
                else
                {
                    barcode = new BCBarcodeBIZ().GetNewBarcode();
                    sampleCodesAndBarcode.Add(classifiedCode, barcode);
                    capacitySum += Decimal.Parse(minCapacity);
                    combineSum += 1;
                }

                //生成项目明细               
                string sql = string.Format(" INSERT INTO bc_cname ( bc_bar_no,bc_bar_code,{1} ) select {3},{3},{2} from dict_combine_bar where com_his_code = '{0}' ", combineID, lisColumns, hisColumns, barcode);
                helper.ExecuteNonQuery(sql);
            }

            DataTable dtPatient = dsData.Tables[BarcodeTable.Patient.TableName].Clone();
            DataRow row = dsData.Tables[BarcodeTable.Patient.TableName].Rows[0];
            for (int i = 0; i < sampleCodesAndBarcode.Keys.Count; i++)
            {
                row[BarcodeTable.Patient.BarcodeDisplayNumber] = row[BarcodeTable.Patient.BarcodeNumber] = sampleCodesAndBarcode.Values.ToList<string>()[i];
                row[BarcodeTable.Patient.Item] = combineNames[i];
                dtPatient.Rows.Add(row.ItemArray);
            }

            return AddPatient(dtPatient);
        }

        /// <summary>
        /// 添加条码资料
        /// </summary>
        private static DataSet AddPatient(DataTable dtPatient)
        {
            DataSet patient = new DataSet();
            patient.Tables.Add(dtPatient);
            return new BCPatientBIZ().doNew(patient);
        }

        /// <summary>
        /// 是否低于最大项目数
        /// </summary>
        /// <param name="combineSum">当前项目数</param>
        /// <param name="maxCombineCount">最大项目数</param>      
        private bool LowThanMaxCombineCount(int combineSum, int maxCombineCount)
        {
            return combineSum + 1 <= maxCombineCount;
        }

        /// <summary>
        /// 是否不是第一个,
        /// </summary>
        /// <param name="i"></param>  
        private bool IsNotFirst(int index)
        {
            return index > 0;
        }

        /// <summary>
        /// 低于最大容量
        /// </summary>
        /// <param name="capacitySum">当前容量</param>
        /// <param name="currentCapacity">当前项目的最小容量</param>
        /// <param name="maxCapacity">最大容量</param>
        private static bool LowThanMaxCapacity(decimal capacitySum, decimal currentCapacity, decimal maxCapacity)
        {
            return capacitySum + currentCapacity <= maxCapacity;
        }

        /// <summary>
        /// 是否分类码一样
        /// </summary>
        /// <param name="sampleCodesAndBarcode">分类码与条码表</param>
        /// <param name="classifiedCode">分类码</param>
        private bool ClassifiedCodeInclude(Dictionary<string, string> classifiedCodesAndBarcode, string classifiedCode)
        {
            return classifiedCodesAndBarcode.ContainsKey(classifiedCode);
        }

//        public override string SearchSQL
//        {
//            get
//            {
//                return @"SELECT   dict_combine_bar.com_id, dict_combine_bar.com_his_name, dict_combine_bar.com_his_code, 
//                dict_combine_bar.com_m_type, dict_combine_bar.com_cuv_code, dict_combine_bar.com_his_py, 
//                dict_combine_bar.com_his_wb, dict_combine_bar.com_cap_sum, dict_combine_bar.com_cap_unit, 
//                dict_combine_bar.com_sam_id, dict_combine_bar.com_ori_id, dict_combine_bar.com_price, 
//                dict_combine_bar.com_unit, dict_combine_bar.com_exec_code, dict_combine_bar.com_del_flag, 
//                dict_combine_bar.com_print_name, dict_combine_bar.com_amount, dict_combine_bar.com_out_time, 
//                dict_combine_bar.com_combine_source, dict_combine_bar.com_hospital_id, dict_combine_bar.com_bar_type, 
//                dict_combine_bar.com_split_flag, dict_combine_bar.com_blood_notice, dict_combine_bar.com_save_notice, 
//                dict_combine_bar.com_split_code, dict_combine_bar.com_lis_code, dict_origin.ori_name, 
//                dict_combine_bar.com_sample_remark_id, dict_Sample_Remarks.rem_cont, dict_combine_bar.com_rep_unit, 
//                dict_combine_bar.com_split_seq, dict_combine_bar.com_his_fee_code, dict_type.type_name
//FROM      dict_combine_bar LEFT OUTER JOIN
//                dict_origin ON dict_combine_bar.com_ori_id = dict_origin.ori_id LEFT OUTER JOIN
//                dict_Sample_Remarks ON dict_combine_bar.com_sample_remark_id = dict_Sample_Remarks.rem_id LEFT OUTER JOIN
//                dict_type ON dict_combine_bar.com_exec_code = dict_type.type_id";
//            }
//        }

        /// <summary>
        /// 拆分条码字典
        /// </summary>
        /// <param name="dsData"></param>
        public override DataSet doView(DataSet dsData)
        {
            DataSet result = new DataSet();
            try
            {
                String comId = dsData.Tables["dict_combine_split"].Rows[0]["com_id"].ToString();

                DataTable dtItemIn = dao.GetDataTable(
                    String.Format(
                                      @"select dict_combine.* from dict_combine LEFT OUTER JOIN dict_type ON dict_combine.com_ptype = dict_type.type_id  where (com_del = '0') {1} and com_id in ( select com_s_id from dict_combine_split where com_id = '{0}' )
", comId, ServerConfig.BuildTypeHospitalSqlWithAnd()), "CombinesIn");
                result.Tables.Add(dtItemIn);
                DataTable dtItemNotIn = dao.GetDataTable(
                    String.Format(
                    @"select dict_combine.* from dict_combine LEFT OUTER JOIN dict_type ON dict_combine.com_ptype = dict_type.type_id where  (com_del = '0') {1} and  com_id <> '{0}' and com_id not in ( select com_s_id from dict_combine_split where com_id = '{0}' )
", comId, ServerConfig.BuildTypeHospitalSqlWithAnd()), "CombinesNotIn");
                result.Tables.Add(dtItemNotIn);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return result;
        }
    }
}