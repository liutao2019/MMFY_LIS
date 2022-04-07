using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto.BarCodeEntity;
using dcl.common;

namespace dcl.svr.sample
{
    public class SplitCodeInfo
    {
        public string NewBarcode { get; set; }
        public string GroupID { get; set; }
        public string SplitCode { get; set; }
        public string Count { get; set; }
        public string OrderTime { get; set; }
        public string PatID { get; set; } //病人ID
        public string PatName { get; set; }//病人姓名
        public string HisFeeCode { get; set; } //HIS组合代码
        private List<string> includeHisFeeCodes = new List<string>();
        public List<string> IncludeHisFeeCodes { get { return includeHisFeeCodes; } set { includeHisFeeCodes = value; } } //已经合并的HIS收费码
        public string OriID { get; set; }
        public SplitCodeInfo(string splitCode)
        {
            this.SplitCode = splitCode;
        }

        /*
        internal void GetMoreInfo(DataRow dataRow)
        {
            //if (dataRow.Table.Columns.Contains("GrpID"))
            //    GroupID = dataRow["GrpID"].ToString();
            //if (dataRow.Table.Columns.Contains("Qty"))
            //    Count = dataRow["Qty"].ToString();
            //if (dataRow.Table.Columns.Contains("chktm"))
            //    OrderTime = dataRow["chktm"].ToString();
            //if (dataRow.Table.Columns.Contains("MzNo"))
            //    PatID = dataRow["MzNo"].ToString();
            //if (dataRow.Table.Columns.Contains("FEEID"))
            //    HisFeeCode = dataRow["FEEID"].ToString(); 
       

            string groupColumn = ConvertHelper.GetHISColumn("bc_group_id");
            if (dataRow.Table.Columns.Contains(groupColumn)
                && dataRow[groupColumn] != null)
                GroupID = dataRow[groupColumn].ToString();

            string groupColumn2 = ConvertHelper.GetHISColumn("bc_gourp_id");
            if (dataRow.Table.Columns.Contains(groupColumn2)
                && dataRow[groupColumn2] != null)
                GroupID = dataRow[groupColumn2].ToString();

            if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_qty")))
                Count = dataRow[ConvertHelper.GetHISColumn("bc_qty")].ToString();

            if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_occ_date")))
                OrderTime = dataRow[ConvertHelper.GetHISColumn("bc_occ_date")].ToString();

            if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_in_no")))
                PatID = dataRow[ConvertHelper.GetHISColumn("bc_in_no")].ToString();

            if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_name")))
                PatID = dataRow[ConvertHelper.GetHISColumn("bc_name")].ToString().Trim();

            if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_his_code")))
                HisFeeCode = dataRow[ConvertHelper.GetHISColumn("bc_his_code")].ToString().Trim();

            if (dataRow.Table.Columns.Contains("bc_ori_id") && dataRow["bc_ori_id"] != null)
                OriID = dataRow["bc_ori_id"].ToString();
        }
        */


        private bool IsNull(object p)
        {
            return p == null;
        }



        internal void GetMoreInfo(DataRow dataRow, BarcodeDownloadInfo downloadInfo)
        {
            //2014年2月21日16:51:21 ye
            dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);

            //string orderTimeColumn = "chktm";
            //string numberColumn = "MzNo";
            //string itemIDColumn = "FEEID";

            //if (downloadInfo.FetchDataType == FetchDataType.Normal)
            //{
            //    orderTimeColumn = "医嘱时间";
            //    numberColumn = "体检号";

            //    itemIDColumn = "项目编码";
            //}

            //if (dataRow.Table.Columns.Contains("GrpID"))
            //    GroupID = dataRow["GrpID"].ToString();
            //if (dataRow.Table.Columns.Contains("Qty"))
            //    Count = dataRow["Qty"].ToString();
            //if (dataRow.Table.Columns.Contains(orderTimeColumn))
            //    OrderTime = dataRow[orderTimeColumn].ToString();
            //if (dataRow.Table.Columns.Contains(numberColumn))
            //    PatID = dataRow[numberColumn].ToString();
            //if (dataRow.Table.Columns.Contains(itemIDColumn))
            //    HisFeeCode = dataRow[itemIDColumn].ToString();

            string groupColumn = columnConvertHelper.GetHISColumn("bc_group_id");
            if (dataRow.Table.Columns.Contains(groupColumn)
                && dataRow[groupColumn] != null)
                GroupID = dataRow[groupColumn].ToString();

            string groupColumn2 = columnConvertHelper.GetHISColumn("bc_gourp_id");
            if (dataRow.Table.Columns.Contains(groupColumn2)
                && dataRow[groupColumn2] != null)
                GroupID = dataRow[groupColumn2].ToString();

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_qty")))
                Count = dataRow[columnConvertHelper.GetHISColumn("bc_qty")].ToString();

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_occ_date")))
                OrderTime = dataRow[columnConvertHelper.GetHISColumn("bc_occ_date")].ToString();

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_in_no")))
                PatID = dataRow[columnConvertHelper.GetHISColumn("bc_in_no")].ToString();

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_name")))
                PatName = dataRow[columnConvertHelper.GetHISColumn("bc_name")].ToString();

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_his_code")))
                HisFeeCode = dataRow[columnConvertHelper.GetHISColumn("bc_his_code")].ToString();

            if (dataRow.Table.Columns.Contains("bc_ori_id") && dataRow["bc_ori_id"] != null)
                OriID = dataRow["bc_ori_id"].ToString();
        }
    }
}
