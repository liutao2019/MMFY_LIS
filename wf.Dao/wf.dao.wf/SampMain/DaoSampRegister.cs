using dcl.dao.core;
using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using System.Data;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampRegister))]
    public class DaoSampRegister : IDaoSampRegister
    {

        public int SaveShelfBarcode(EntitySampRegister data)
        {
            int ret = 1;
            try
            {
                if (ExistShelfBarcode(data.RegBarCode, data.RegDate))
                {
                    return -1;
                }

                if (ExistSeqNo(data.RegNumber, data.RegLabId))
                {
                    return -2;
                }
                if (Returned(data.RegBarCode))
                {
                    return -3;
                }

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Sreg_lab_id", data.RegLabId);
                values.Add("Sreg_date", data.RegDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Sreg_bar_code", data.RegBarCode);
                values.Add("Sreg_number", data.RegNumber);
                values.Add("Sreg_Dtrack_code", data.RegRackCode);
                values.Add("Sreg_rack_no", data.RegRackNo);
                values.Add("Sreg_x_place", data.RegXPlace);
                values.Add("Sreg_y_place", data.RegYPlace);
                values.Add("Sreg_Buser_id", data.RegUserId);
                if(data.RegComName == null)
                {
                    values.Add("Sreg_com_name", DBNull.Value);
                }
                else
                {
                    values.Add("Sreg_com_name", data.RegComName);
                }
                

                object obj = helper.InsertOperation("Sample_register", values);

                ret = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -3;
            }
            return ret;
        }

        /// <summary>
        /// 判断当天是否已录入指定的条码号
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private bool ExistShelfBarcode(string barcode, DateTime date)
        {
            string sqlSelect = string.Format(@"
                select
                Sreg_id
                from Sample_register
                where Sreg_bar_code = '{0}' and Sreg_date >= '{1}' and Sreg_date < '{2}'
                ", barcode, date.Date.ToString("yyyy-MM-dd HH:mm:ss"), date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(sqlSelect);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 顺序号是否存在
        /// </summary>
        /// <param name="seqno"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        private bool ExistSeqNo(int seqno, string deptid)
        {
            string sqlSelect = string.Format(@"
select
Sreg_id
from Sample_register
where Sreg_lab_id = '{0}' and Sreg_number = {1} and Sreg_date >= '{2}' and Sreg_date < '{3}'
", deptid, seqno, DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"), 
DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"));

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(sqlSelect);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断条码号是否已回退
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public bool Returned(string barCode)
        {
            EntitySampQC sampQC = new EntitySampQC();
            List<string> barcodeList = new List<string>();
            barcodeList.Add(barCode);
            sampQC.ListSampBarId = barcodeList;
            DaoSampleMain dao = new DaoSampleMain();
            List<EntitySampMain> dt = dao.GetSampMain(sampQC);
            if (dt != null && dt.Count > 0)
            {
                if (dt[0].SampStatusId.ToString() == "9")
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool DeleteShelfBarcode(long RegSn)
        {
            try
            {
                DBManager helper = new DBManager();

                string sql = string.Format(@"delete from Sample_register where Sreg_id = '{0}' ", RegSn);
                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySampRegister> GetSampRegister(long RegSn)
        {
            List<EntitySampRegister> list = new List<EntitySampRegister>();
            try
            {
                string sql = string.Format(@"Select
Sample_register.*,
Dict_profession.Dpro_name,
Sample_main.Sma_pat_src_id,
Dict_source.Dsorc_name,--病人来源

Sample_main.Sma_pat_name,
Sample_main.Sma_pat_in_no,--住院号
Sample_main.Sma_pat_unique_id,--住院号
Sample_main.Sma_pat_sex,
Sample_main.Sma_pat_age,
Sample_main.Sma_pat_dept_name,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_doctor_name,
Sample_main.Sma_pat_admiss_times, --住院次数
Sample_main.Sma_pat_diag, --临床诊断
Base_User.Buser_name,
Sample_main.Sma_Dsam_id,
Sample_main.Sma_Dsam_name,
st_order_occ_date = (select top 1 Sdet_order_occ_date from Sample_detail where Sample_register.Sreg_bar_code = Sample_detail.Sdet_bar_code)
From Sample_register
left join Dict_profession on Dict_profession.Dpro_id = Sample_register.Sreg_lab_id
inner join Sample_main on Sma_bar_code = Sample_register.Sreg_bar_code
left join Dict_source on Dict_source.Dsorc_id = Sample_main.Sma_pat_src_id
left join Base_user on Sample_register.Sreg_Buser_id = Base_user.Buser_id
WHERE Sample_register.Sreg_id = '{0}'", RegSn);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampRegister>.ConvertToList(dt).OrderBy(i => i.RegSn).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public List<EntitySampRegister> GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo)
        {
            List<EntitySampRegister> list = new List<EntitySampRegister>();
            try
            {
                string sql = string.Format(@"select
Sample_register.Sreg_id,--Samp_register的id
Sample_register.Sreg_lab_id,--接收科室
Sample_register.Sreg_bar_code,--条码号
Sample_register.Sreg_x_place,
Sample_register.Sreg_y_place,
Sample_register.Sreg_date,--登记日期
Sample_register.Sreg_number,--序号
Sample_register.Sreg_rack_no,--架子号
Sample_main.Sma_pat_name,--病人姓名
Sample_main.Sma_pat_in_no,--住院号
Sample_main.Sma_pat_unique_id,--唯一号
Dict_itm_combine.Dcom_name,
Sample_detail.Sdet_com_id,--检验组合lis中的id
Sample_main.Sma_pat_sex,--性别
Sample_main.Sma_pat_age,--年龄
Sample_main.Sma_pat_dept_name,--科室
Sample_main.Sma_pat_bed_no,--床号
Sample_main.Sma_pat_diag,--临床诊断
Sample_main.Sma_doctor_name,--开单医生
Sample_main.Sma_Dsam_id,
Sample_main.Sma_Dsam_name,
Sample_detail.Sdet_order_occ_date,--执行日期
Sample_main.Sma_pat_admiss_times,--住院次数
Sample_detail.Sdet_sn,
Sample_detail.Sdet_flag,
Sample_main.Sma_status_id

from Sample_register
inner join Sample_main on Sample_main.Sma_bar_code = Sample_register.Sreg_bar_code
left join Sample_detail on Sample_detail.Sdet_bar_code = Sample_register.Sreg_bar_code
left join Dict_itm_combine on Dict_itm_combine.Dcom_id = Sample_detail.Sdet_com_id
where Sample_detail.del_flag='0' and Sample_register.Sreg_lab_id = '{0}' and Sample_register.Sreg_date >=@regDateFrom and Sample_register.Sreg_date <@regDateTo
                                    ", receviceDeptID, regDateFrom.ToString("yyyy-MM-dd HH:mm:ss"), regDateTo.ToString("yyyy-MM-dd HH:mm:ss"));
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@regDateFrom", regDateFrom.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                paramHt.Add(new SqlParameter("@regDateTo", regDateTo.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss")));
                if (shelfNoFrom != null)
                {
                    string sqlPart = string.Format(" and Sample_register.Sreg_rack_no >= {0} ", shelfNoFrom.Value);
                    sql = sql + sqlPart;
                }

                if (shelfNoTo != null)
                {
                    string sqlPart = string.Format(" and Sample_register.Sreg_rack_no <= {0} ", shelfNoTo.Value);
                    sql = sql + sqlPart;
                }


                if (seqFrom != null)
                {
                    string sqlPart = string.Format(" and Sample_register.Sreg_number >= {0} ", seqFrom.Value);
                    sql = sql + sqlPart;
                }

                if (seqTo != null)
                {
                    string sqlPart = string.Format(" and Sample_register.Sreg_number <= {0} ", seqTo.Value);
                    sql = sql + sqlPart;
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql,paramHt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampRegister>.ConvertToList(dt).OrderBy(i => i.RegNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public List<EntitySampRegister> GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime)
        {
            List<EntitySampRegister> list = new List<EntitySampRegister>();
            try
            {
                string sql = string.Format(@"select 
cast(0 as bit) as  pat_select,
Sample_register.Sreg_id,
Sample_register.Sreg_number,
Sample_register.Sreg_com_name,
Sample_register.Sreg_bar_code,
Sample_main.Sma_pat_name
from Sample_register
inner join Sample_main on Sma_bar_code = Sample_register.Sreg_bar_code
where Sample_register.Sreg_lab_id = '{0}' and Sample_register.Sreg_date >= '{1}' and Sample_register.Sreg_date < '{2}'
                        ", deptid, depTime, depTime.AddDays(1).AddSeconds(-1));

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampRegister>.ConvertToList(dt).OrderBy(i => i.RegNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc)
        {
            EntityDCLPrintData printer = new EntityDCLPrintData();
            DataSet result = new DataSet();
            try
            {
                StatQc = GetWhere(StatQc);

                List<EntitySysReport> dtEx = GetSysReportByCode();
                if (dtEx.Count > 0)
                {
                    string DateEditEnd = Convert.ToDateTime(StatQc.DateEditEnd).ToString("yyyy年MM月dd日");
                    string DateEditStart = Convert.ToDateTime(StatQc.DateEditStart).ToString("yyyy年MM月dd日");
                    string sql = EncryptClass.Decrypt(dtEx[0].RepSql.ToString());
                    sql = sql.Replace("&number&", StatQc.EditYBStart);
                    sql = sql.Replace("&where&", StatQc.Where);
                    sql = sql.Replace("&stratTime&", DateEditStart);
                    sql = sql.Replace("&endTime&", DateEditEnd);

                    DataTable an = new DataTable();
                    DBManager helper = new DBManager();

                    an = helper.ExecuteDtSql(sql);
                    an.TableName = "可设计字段";
                    result.Tables.Add(an.Copy());
                    printer.ReportData = result;
                    printer.ReportName = dtEx[0].RepLocation.ToString().Replace(".repx", "");
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                DataTable dt = new DataTable("ErrorMessage");
                dt.Columns.Add("ErrorMessage");
                DataRow dr = dt.NewRow();
                dr["ErrorMessage"] = "报表查询语句有误，请联系管理员";
                result.Tables.Add(dt);
                printer.ReportData = result;
            }
            return printer;
        }

        EntityStatisticsQC GetWhere(EntityStatisticsQC statQC)
        {
            string where = "";

            if (!string.IsNullOrEmpty(statQC.BacilliType))
            {
                where += " and Sample_register.Sreg_lab_id='" + statQC.BacilliType + "'";
            }

            if (!string.IsNullOrEmpty(statQC.DateEditStart))
            {
                where += " and Sample_register.Sreg_date>='" + statQC.DateEditStart + "'";
            }
            if (!string.IsNullOrEmpty(statQC.DateEditEnd))
            {
                where += " and Sample_register.Sreg_date<='" + statQC.DateEditEnd + "'";
            }
            if (!string.IsNullOrEmpty(statQC.EditYBStart))
            {
                where += " and (" + dcl.common.SampleIDRangeUtil.IntSampleIDToSQL("Sample_register.Sreg_number", statQC.EditYBStart) + ")";
            }
            statQC.Where += where;
            return statQC;
        }

        List<EntitySysReport> GetSysReportByCode()
        {
            try
            {
                String sql = string.Format(@"select * from Base_report where Brep_code = 'specimenStat'");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySysReport> list = EntityManager<EntitySysReport>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysReport>();
            }
        }

        public int GetSampRegisterMaxId()
        {
            int ret = 0;
            try
            {
                String sql = string.Format(@"select MAX(Sreg_id) as Sreg_id from Sample_register");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySampRegister> list = EntityManager<EntitySampRegister>.ConvertToList(dt).ToList();

                ret = Convert.ToInt32(list[0].RegSn.ToString());
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return ret;
        }
    }
    
}
