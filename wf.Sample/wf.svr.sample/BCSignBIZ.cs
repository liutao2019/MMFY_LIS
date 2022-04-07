using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.root.dac;
using dcl.svr.root.com;
using dcl.svr.frame;
using dcl.common;

namespace dcl.svr.sample
{
    public class BCSignBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.Sign.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.Sign.ID; }
        }

        public override DataSet Search(string where)
        {
            return doSearch(new DataSet(), SearchSQL + where);
        }

        public override int Update(string set, string where)
        {
            return base.Update(set, where);
        }

        public override DataSet doOther(DataSet dsData)
        {
            DataSet result = new DataSet();
            if (dsData != null && dsData.Tables.Count > 0)
            {
                try
                {
                    string strBarCode = dsData.Tables["bc_sign"].Rows[0]["bc_bar_code"].ToString();
                    string strDeleteBcSignSql = string.Format("delete bc_sign where bc_bar_no='{0}'", strBarCode);
                    DBHelper helper = new DBHelper();
                    helper.ExecuteNonQuery(strDeleteBcSignSql);
                }
                catch (Exception ex)
                {
                    result.Tables.Add(CommonBIZ.createErrorInfo("删除数据出错!", ex.ToString())); ;
                }

            }
            return result;
        }
        /// <summary>
        /// 根据条码号检索签名信息
        /// </summary>
        /// <param name="bc_bar_code"></param>
        /// <returns></returns>
        public DataTable SearchByBarcode(string bc_bar_code)
        {
            string input = SQLFormater.Format(bc_bar_code);
            string sqlSelect = string.Format(@"
select
bc_sign.*
from bc_sign 
where
bc_sign.bc_bar_code = '{0}'
and bc_status is not null and bc_status <> ''
order by bc_sign.bc_date asc
", input);

            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);
            dt.TableName = "bc_sign";
            return dt;
        }

        //public override DataSet doNew(DataSet dsData)
        //{
        //    return base.doNew(dsData);
        //}
    }
}