using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.root.com;
using System.Data;
using System.Collections;
using lis.dto;
using dcl.common;
using dcl.svr.cache;
using dcl.root.dto;

namespace dcl.svr.sample
{
    public class LabcodePrintBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        DataSet ICommonBIZ.doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        DataSet ICommonBIZ.doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取组合对应的工作单信息
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        DataSet ICommonBIZ.doOther(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables["dtCSW"] != null)
                    {
                        DataTable dt = ds.Tables["dtCSW"];

                        string comIn = "";//组合ID集

                        foreach (DataRow drCSW in dt.Rows)
                        {
                            if (string.IsNullOrEmpty(comIn))
                            {
                                comIn = string.Format("'{0}'", drCSW["bc_lis_code"].ToString());
                            }
                            else
                            {
                                comIn += string.Format(",'{0}'", drCSW["bc_lis_code"].ToString());
                            }
                        }

                        DataTable dtComSam = getComSamRefWorkSheet(dt.Rows[0]["bc_sam_id"].ToString(), comIn);

                        //result.Tables.Add(dtComSam, "Dict_ComSamRefWorkSheet");
                        result.Tables.Add(dtComSam);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取工作单信息", ex.ToString()));
            }

            return result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        DataSet ICommonBIZ.doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables["dtWhere"] != null)
                    {
                        DataTable dt = ds.Tables["dtWhere"];
                        if (dt.Rows.Count > 0)
                        {
                            string sqlWhere = dt.Rows[0]["sqlwhere"].ToString();
                            DataTable dtLC = getLabcodeBySQLwhere(sqlWhere);//查询实验码信息
                            result.Tables.Add(dtLC);
                        }
                    }
                    else
                    {
                        throw new Exception("参数有误");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("查询实验码信息错误", ex.ToString()));
            }

            return result;
        }

        /// <summary>
        /// 生成实验码
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        DataSet ICommonBIZ.doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtUpdateBC"] != null)
                {
                    DataTable dtGroup = GetdtGroup();//获取分组表
                    foreach (DataRow drUD in ds.Tables["dtUpdateBC"].Rows)
                    {
                        DataRow drObj = null;
                        //检查是否已经存在相同的工作单和类别,是则合并,使用相同的类别序号
                        if (IsSameSamAdnType(dtGroup, out drObj, drUD["sht_id"].ToString(), drUD["com_labcode_type"].ToString()))
                        {
                            drUD["lc_type_seq"] = drObj["lc_type_seq"].ToString();
                        }
                        else
                        {
                            drUD["lc_type_seq"] = Generate(drUD["com_labcode_type"].ToString());//获取此类型序号

                            DataRow drNwGroup = dtGroup.NewRow();
                            drNwGroup["bc_id"] = drUD["bc_id"].ToString();
                            drNwGroup["sht_id"] = drUD["sht_id"].ToString();
                            drNwGroup["com_labcode_type"] = drUD["com_labcode_type"].ToString();
                            drNwGroup["lc_type_seq"] = drUD["lc_type_seq"].ToString();
                            dtGroup.Rows.Add(drNwGroup);
                        }
                    }

                    if (ds.Tables["dtUpdateBC"].Rows.Count > 0)
                    {
                        ArrayList arr = new ArrayList();

                        string strYear = DateTime.Now.Year.ToString().Substring(2, 2);//获取年份后两位

                        foreach (DataRow drUD in ds.Tables["dtUpdateBC"].Rows)
                        {
                            string strSQL = string.Format(@"update bc_cname set bc_lc_code='{0}{1}{2}',bc_lc_work_id='{3}' where bc_id={4}  
", strYear, drUD["com_labcode_type"].ToString(), drUD["lc_type_seq"].ToString(), drUD["sht_id"].ToString(), drUD["bc_id"].ToString());

                            arr.Add(strSQL);
                        }

                        dao.DoTran(arr);
                    }
                }
                else
                {
                    throw new Exception("参数值为null,生成实验码失败");
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("生成实验码失败", ex.ToString()));
            }

            return result;
        }

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        DataSet ICommonBIZ.doView(DataSet ds)
        {
            DataSet result = new DataSet();

            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables["dtBC"] != null)
                    {
                        DataTable dt = ds.Tables["dtBC"];

                        DataTable dtBCPat = getBCPat(dt.Rows[0]["barcode"].ToString());

                        DataTable dtBCname = getBCnameByBarcode(dt.Rows[0]["barcode"].ToString());

                        //result.Tables.Add(dtBCPat, "bc_patients");
                        //result.Tables.Add(dtBCname, "bc_cname");

                        result.Tables.Add(dtBCPat);
                        result.Tables.Add(dtBCname);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取条码信息", ex.ToString()));
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 根据条码号获取bc_patients信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private DataTable getBCPat(string barcode)
        {
            DataTable dtBCPat = null;
            try
            {
                dtBCPat = dao.GetDataTable(
                    String.Format(@"select bc_bar_no,bc_bar_code,bc_sam_id,bc_date from bc_patients 
where bc_bar_no='{0}' and bc_del='0'", barcode), "bc_patients");

            }
            catch (Exception ex)
            {
                throw ex;
                //result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return dtBCPat;
        }

        /// <summary>
        /// 根据条码号获取bc_cname信息
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns></returns>
        private DataTable getBCnameByBarcode(string barcode)
        {
            DataTable dtBCname = null;
            try
            {
                dtBCname = dao.GetDataTable(
                    String.Format(@"select bc_id,bc_bar_no,bc_bar_code,bc_lis_code,
bc_lc_code,bc_lc_work_id from bc_cname
where bc_bar_no='{0}' and bc_del='0'", barcode), "bc_cname");

            }
            catch (Exception ex)
            {
                throw ex;
                //result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return dtBCname;
        }

        /// <summary>
        /// 根据条件查询实验码信息
        /// </summary>
        /// <param name="sqlWhere">条件</param>
        /// <returns></returns>
        private DataTable getLabcodeBySQLwhere(string sqlWhere)
        {
            DataTable dtLabcode = null;
            try
            {
                string strSQL = @"select p_bc_cname.bc_lc_code,bc_patients.bc_bar_no,bc_patients.bc_bar_code,
bc_patients.bc_name,bc_patients.bc_sex,bc_patients.bc_age,bc_patients.bc_in_no,bc_patients.bc_bed_no,
dict_type.type_name,bc_patients.bc_d_name,dict_cuvette.cuv_name,dict_sample.sam_name
,Dict_Worksheet.*
from
(select  bc_bar_no,bc_lc_code,bc_lc_work_id from bc_cname
where bc_del='0' and bc_lc_code is not null and bc_lc_work_id is not null
{0}
group by bc_bar_no,bc_lc_code,bc_lc_work_id) as p_bc_cname
inner join bc_patients on p_bc_cname.bc_bar_no=bc_patients.bc_bar_no
LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id
LEFT OUTER JOIN  dict_cuvette ON bc_patients.bc_cuv_code = dict_cuvette.cuv_code
LEFT OUTER JOIN dict_sample ON bc_patients.bc_sam_id = dict_sample.sam_id 
LEFT OUTER JOIN Dict_Worksheet on p_bc_cname.bc_lc_work_id=Dict_Worksheet.sht_id";

                strSQL = string.Format(strSQL, sqlWhere);

                dtLabcode = dao.GetDataTable(strSQL, "bc_cname_lc");

                if (dtLabcode != null && dtLabcode.Rows.Count > 0)
                {
                    dtLabcode.DefaultView.Sort = "bc_lc_code,bc_bar_code ASC";
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return dtLabcode;
        }

        /// <summary>
        /// 根据标本类型与组合得实验码工作单
        /// </summary>
        /// <param name="samID">标本ID</param>
        /// <param name="comIN">组合</param>
        /// <returns></returns>
        private DataTable getComSamRefWorkSheet(string samID, string comIN)
        {
            DataTable dtComSam = null;
            try
            {
                string strSQL = @"select Dict_ComSamRefWorkSheet.*,dict_combine.com_labcode_type,Dict_Worksheet.* from Dict_ComSamRefWorkSheet
left join dict_combine on
Dict_ComSamRefWorkSheet.csrw_com_id=dict_combine.com_id
left join Dict_Worksheet on
Dict_ComSamRefWorkSheet.csrw_work_id=Dict_Worksheet.sht_id
where Dict_ComSamRefWorkSheet.csrw_com_id in({0})--组合
and Dict_ComSamRefWorkSheet.csrw_sam_id='{1}'--样本";

                strSQL = string.Format(strSQL, comIN, samID);

                dtComSam = dao.GetDataTable(strSQL, "Dict_ComSamRefWorkSheet");

                if (dtComSam != null && dtComSam.Rows.Count > 0)
                {
                    dtComSam.DefaultView.Sort = "csrw_com_id ASC";
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return dtComSam;
        }

        /// <summary>
        /// 获取某一实验码类型的递增号
        /// </summary>
        /// <param name="lc_type"></param>
        /// <returns></returns>
        private string Generate(string lc_type)
        {
            string id_key = "";
            try
            {
                id_key = new Lib.DAC.GlobalSysTableIDGenerator().Generate("labcode_type", lc_type, "");
                for (int i = 0; id_key.Length < 6; i++)
                {
                    id_key = "0" + id_key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id_key;
        }

        /// <summary>
        /// 定义好的分组表
        /// </summary>
        /// <returns></returns>
        private DataTable GetdtGroup()
        {
            DataTable dtObj = new DataTable("dtGroup");
            dtObj.Columns.Add("bc_id", System.Type.GetType("System.String"));
            dtObj.Columns.Add("sht_id", System.Type.GetType("System.String"));
            dtObj.Columns.Add("com_labcode_type", System.Type.GetType("System.String"));
            dtObj.Columns.Add("lc_type_seq", System.Type.GetType("System.String"));//序号

            return dtObj;
        }

        /// <summary>
        /// 根据工作单ID与组合实验码类别判断是否已经存
        /// </summary>
        /// <param name="dtObj">数据表</param>
        /// <param name="drObj">返回行</param>
        /// <param name="sht_id">工作单ID</param>
        /// <param name="com_labcode_type">组合实验码类别</param>
        /// <returns>true表示存在</returns>
        private bool IsSameSamAdnType(DataTable dtObj, out DataRow drObj, string sht_id, string com_labcode_type)
        {
            bool bln = false;

            drObj = null;

            if (dtObj != null && dtObj.Rows.Count > 0)
            {
                DataRow[] drTeam = dtObj.Select(string.Format(@"sht_id='{0}' and com_labcode_type='{1}' ", sht_id, com_labcode_type));
                if (drTeam.Length > 0)
                {
                    bln = true;
                    drObj = drTeam[0];
                }
            }

            return bln;
        }
    }
}
