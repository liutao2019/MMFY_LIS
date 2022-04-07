using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using Lib.LogManager;
using System.Configuration;

namespace dcl.svr.sample
{
    //肿瘤医院条码签收时进行费用判断
    public class ZLYYBarCheckHISStatus
    {
        static bool WriteLog = false;
        public static string ZLYYCheckHISStatus(string barcode, string ori_ID)
        {

            Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
            DataTable t_bc_cname = helper.GetTable(string.Format(@"
select bc_cname.bc_yz_id
from bc_cname  with(nolock) 
left join bc_patients with(nolock) on  bc_cname.bc_bar_code=bc_patients.bc_bar_code
where bc_cname.bc_bar_code='{0}' and bc_patients.bc_ori_id in ('107','108') and  bc_cname.bc_del <> '1' and bc_yz_id is not null", barcode));

            if (t_bc_cname.Rows.Count > 0)
            {
                //if (ori_ID == "107")
                //{
                return MZCheck(t_bc_cname);
                //}
                //else
                //{
                //    return ZYCheck(t_bc_cname);
                //}
            }

            return "1";
        }

        public static string ZYCheck(DataTable t_bc_cname)
        {
            StringBuilder recipe_codeSql = new StringBuilder();
            foreach (DataRow item in t_bc_cname.Rows)
            {
                string yzID = item["bc_yz_id"].ToString();
                if (!string.IsNullOrEmpty(yzID))
                {
                    string[] array = yzID.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length > 1)
                    {
                        if (recipe_codeSql.Length > 0)
                        {
                            recipe_codeSql.Append(" or ");
                        }
                        recipe_codeSql.Append(string.Format("(ORDER_CODE = '{0}' and order_exec_code='{1}')", array[0], array[1]));
                    }



                }

            }

            if (recipe_codeSql.Length > 0)
            {
                string sqlSelect1 = string.Format(@"

select
IS_VALID
from NEUSOFT.VIEW_LIS_PATIENT_ITEM
where {0}
", recipe_codeSql.ToString());
                try
                {


                    //string connStr = "DSN=HIS_DB2_B;UID=lis;Pwd=lis";
                    //string connStr = "DSN=HIS_DB2_B;UID=lis;Pwd=lis";
                    string connStr = ConfigurationSettings.AppSettings["ZLYYConnectionString"];

                    string ZLYYHISDBTYPE = ConfigurationSettings.AppSettings["ZLYYHISDBTYPE"];
                    SqlHelper helper;
                    //1.根据医嘱号和his项目编号得到所有的明细
                    if (ZLYYHISDBTYPE == "DB2")
                        helper = new SqlHelper(connStr, EnumDbDriver.ODBC, EnumDataBaseDialet.DB2);
                    else
                        helper = new SqlHelper(connStr, EnumDbDriver.Oracle, EnumDataBaseDialet.Oracle11g);
                    DataTable resulttable = helper.GetTable(sqlSelect1);

                    //IS_VALID='1' 表示已经收费
                    //IS_VALID='0' 表示未收费
                    foreach (DataRow item in resulttable.Rows)
                    {
                        if (item["IS_VALID"] != null && item["IS_VALID"].ToString() == "0")
                        {
                            return "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("肿瘤医院ZY条码签收时进行费用判断", "获取判断数据："+ex.Message + sqlSelect1);

                }

            }
            return "1";
        }
        public static string MZCheck(DataTable t_bc_cname)
        {
            #region MyRegion


            /*
//            StringBuilder recipe_codeSql = new StringBuilder();
//            foreach (DataRow item in t_bc_cname.Rows)
//            {
//                string yzID = item["bc_yz_id"].ToString();
//                if (!string.IsNullOrEmpty(yzID))
//                {


//                    if (recipe_codeSql.Length > 0)
//                    {
//                        recipe_codeSql.Append(",");
//                    }
//                    recipe_codeSql.Append(string.Format("'{0}'", yzID));
//                }

//            }
//            if (recipe_codeSql.Length > 0)
//            {

//                string sqlSelect1 = string.Format(@"
//select
//IS_VALID
//from NEUSOFT.VIEW_LIS_PATIENT_ITEM
//where recipe_code in (0) ", recipe_codeSql.ToString());

//                try
//                {


//                    string connStr = "DSN=HIS_DB2_B;UID=lis;Pwd=lis";
//                    //1.根据医嘱号和his项目编号得到所有的明细
//                    SqlHelper helper = new SqlHelper(connStr, EnumDbDriver.ODBC, EnumDataBaseDialet.SQL2005);
//                    DataTable resulttable = helper.GetTable(sqlSelect1);

//                    //IS_VALID='1' 表示已经收费
//                    //IS_VALID='0' 表示未收费
//                    foreach (DataRow item in resulttable.Rows)
//                    {
//                        if (item["IS_VALID"] != null && item["IS_VALID"].ToString() == "0")
//                        {
//                            return "0";
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {

//                    lib.Logger.Logger.WriteException("ZLYYBarCheckHISStatus", "肿瘤医院MZ条码签收时进行费用判断", "获取判断数据：" + sqlSelect1);
//                }


//            }
//            return "1";
             */
            #endregion

            StringBuilder recipe_codeSql = new StringBuilder();
            foreach (DataRow item in t_bc_cname.Rows)
            {
                string yzID = item["bc_yz_id"].ToString();
                if (!string.IsNullOrEmpty(yzID))
                {
                    string[] array = yzID.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length > 1)
                    {
                        if (recipe_codeSql.Length > 0)
                        {
                            recipe_codeSql.Append(" or ");
                        }
                        recipe_codeSql.Append(string.Format("(ORDER_CODE = '{0}' and order_exec_code='{1}')", array[0], array[1]));
                    }



                }

            }

            if (recipe_codeSql.Length > 0)
            {
                string sqlSelect1 = string.Format(@"

select
IS_VALID
from NEUSOFT.VIEW_LIS_PATIENT_ITEM
where {0}
", recipe_codeSql.ToString());
                try
                {


                    //string connStr = "DSN=HIS_DB2_B;UID=lis;Pwd=lis";
                    string connStr = ConfigurationSettings.AppSettings["ZLYYConnectionString"];

                    string ZLYYHISDBTYPE = ConfigurationSettings.AppSettings["ZLYYHISDBTYPE"];
                    SqlHelper helper;
                    //1.根据医嘱号和his项目编号得到所有的明细
                    if (ZLYYHISDBTYPE == "DB2")
                        helper = new SqlHelper(connStr, EnumDbDriver.ODBC, EnumDataBaseDialet.DB2);
                    else
                        helper = new SqlHelper(connStr, EnumDbDriver.Oracle, EnumDataBaseDialet.Oracle11g);
                    DataTable resulttable = helper.GetTable(sqlSelect1);

                    //IS_VALID='1' 表示已经收费
                    //IS_VALID='0' 表示未收费
                    foreach (DataRow item in resulttable.Rows)
                    {
                        if (item["IS_VALID"] != null && item["IS_VALID"].ToString() == "0")
                        {
                            return "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("肿瘤医院MZ条码签收时进行费用判断", "获取判断数据：" + sqlSelect1+ex.Message);

                }

            }
            return "1";
        }
    }
}
