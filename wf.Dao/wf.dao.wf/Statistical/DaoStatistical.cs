using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using Lib.DAC;
using Lib.DataInterface;
using Lib.DataInterface.Implement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoStatistical))]
    public class DaoStatistical : IDaoStatistical
    {
        List<EntitySysReport> GetSysReportByCode(string repCode)
        {
            try
            {
                String sql = string.Format(@"select * from Base_report where Brep_code = '{0}'", repCode);
                if (repCode == "tpAnalyse")
                {
                    sql = "select * from Base_report where Brep_code='dataanalysis'";
                }
                if (repCode == "tpExamine")
                {
                    sql = "select * from Base_report where Brep_code='examine'";
                }
                if (repCode == "tpData")
                {
                    sql = "select * from Base_report where Brep_code='dataBrowse'";
                }
                if (repCode == "tpParData")
                {
                    sql = "select * from Base_report where Brep_code='sickAnalysis'";
                }

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

        public List<EntityTpTemplate> GetTemplate(string stName, string stType)
        {
            try
            {
                String sql = string.Format(@"select * from tp_template where 1 = 1");

                if (!string.IsNullOrEmpty(stName))
                {
                    sql += string.Format(@"and tp_template.st_name='{0}'", stName);
                }
                if (!string.IsNullOrEmpty(stType))
                {
                    sql += string.Format(@"and dbo.tp_template.st_type='{0}'", stType);
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityTpTemplate> list = EntityManager<EntityTpTemplate>.ConvertToList(dt).OrderBy(i => i.StId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityTpTemplate>();
            }
        }

        EntityStatisticsQC GetWhere(EntityStatisticsQC statQC)
        {
            string where = "";
            string subWhere = string.Empty;

            string strTime = "Pat_lis_main.Pma_in_date";


            if (statQC.TimeType == "报告时间")
                strTime = "Pat_lis_main.Pma_report_date";
            if (statQC.TimeType == "签收时间")
                strTime = "Pat_lis_main.Pma_apply_date";

            if (!string.IsNullOrEmpty(statQC.DateEditStart))
            {
                where += " and " + strTime + " >= '" + statQC.DateEditStart + "'";
            }
            if (!string.IsNullOrEmpty(statQC.DateEditEnd))
            {
                where += " and " + strTime + " <'" + Convert.ToDateTime(statQC.DateEditEnd).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }

            string strNo = statQC.SelectedIndex == "0" ? "Pat_lis_main.Pma_sid" : "Pat_lis_main.Pma_serial_num";


            if (!string.IsNullOrEmpty(statQC.EditYBStart))
            {
                if (Convert.ToInt32(statQC.EditYBStart) > 0)
                {
                    where += string.Format(" and cast({0} as bigint)>={1}", strNo, statQC.EditYBStart);
                    subWhere += string.Format(" and cast({0} as bigint)>={1}", strNo, statQC.EditYBStart);
                }
            }
            if (!string.IsNullOrEmpty(statQC.EditYBEnd))
            {
                if (Convert.ToInt32(statQC.EditYBEnd) > 0)
                {
                    where += string.Format(" and cast({0} as bigint)<={1}", strNo, statQC.EditYBEnd);
                    subWhere += string.Format(" and cast({0} as bigint)<={1}", strNo, statQC.EditYBEnd);
                }
            }
            if (!string.IsNullOrEmpty(statQC.EditAgeStart))
            {
                if (Convert.ToInt32(statQC.EditAgeStart) > 0)
                {
                    where += " and Pat_lis_main.Pma_pat_age>=" + Convert.ToInt32(statQC.EditAgeStart) * 365 * 24 * 60;
                    subWhere += " and Pat_lis_main.Pma_pat_age>=" + Convert.ToInt32(statQC.EditAgeStart) * 365 * 24 * 60;
                }
            }
            if (!string.IsNullOrEmpty(statQC.EditAgeEnd))
            {
                if (Convert.ToInt32(statQC.EditAgeEnd) > 0)
                {
                    where += " and Pat_lis_main.Pma_pat_age<=" + Convert.ToInt32(statQC.EditAgeEnd) * 365 * 24 * 60;
                    subWhere += " and Pat_lis_main.Pma_pat_age<=" + Convert.ToInt32(statQC.EditAgeEnd) * 365 * 24 * 60;
                }
            }
            if (!string.IsNullOrEmpty(statQC.Sex))
            {
                where += " and Pat_lis_main.Pma_pat_sex =" + statQC.Sex;
                subWhere += " and Pat_lis_main.Pma_pat_sex=" + statQC.Sex;
            }
            if (statQC.CmbResults != "全部" && (statQC.BacilliType == "4" || statQC.BacilliType == "5"))
            {
                where += " and Lis_result_anti.Lanti_value = '" + statQC.CmbResults + "'";
            }
            if (!string.IsNullOrEmpty(statQC.OrgId))
            {
                where += "and Dict_organize.Dorg_id='" + statQC.OrgId + "'";
            }
            statQC.Where += where;
            statQC.SubWhere += subWhere;
            return statQC;
        }

        EntityStatisticsQC GetReagentWhere(EntityStatisticsQC statQC)
        {
            string where = "";
            string subWhere = string.Empty;
            string strTime = "Rea_storage.Rsr_date";

            string strNo = statQC.SelectedIndex == "0" ? "Pat_lis_main.Pma_sid" : "Pat_lis_main.Pma_serial_num";

            if (statQC.ReagentType == "申领")
            {
                strTime = "Rea_apply.Ray_date";
                strNo = "Rea_apply.Ray_no";
            }
            else if (statQC.ReagentType == "申购")
            {
                strTime = "Rea_subscribe.Rsb_date";
                strNo = "Rea_subscribe.Rsb_no";
            }
            else if (statQC.ReagentType == "采购")
            {
                strTime = "Rea_purchase.Rpc_date";
                strNo = "Rea_purchase.Rpc_no";
            }
            else if (statQC.ReagentType == "入库")
            {
                strTime = "Rea_storage.Rsr_date";
                strNo = statQC.SelectedIndex == "0" ? "Rea_storage_detail.Rsd_no" :
                    statQC.SelectedIndex == "1"? "Rea_storage_detail.Rsd_batchno" : "Rea_storage_detail.Rsd_barcode";
            }
            else if (statQC.ReagentType == "库存")
            {
                strTime = "Rea_storage.Rsr_date";
                strNo = statQC.SelectedIndex == "0" ? "Rea_storage_detail.Rsd_no" :
                    statQC.SelectedIndex == "1" ? "Rea_storage_detail.Rsd_batchno" : "Rea_storage_detail.Rsd_barcode";
            }
            else if (statQC.ReagentType == "出库")
            {
                strTime = "Rea_delivery.Rdl_date";
                strNo = "Rea_delivery_detail.Rdvd_no";
            }
            else if (statQC.ReagentType == "报损")
            {
                strTime = "Rea_lossreport.Rlr_date";
                strNo = "Rea_lossreport_detail.Rld_no";
            }
            if (!statQC.WithoutTime)
            {
                if (!string.IsNullOrEmpty(statQC.DateEditStart))
                {
                    where += " and " + strTime + " >= '" + statQC.DateEditStart + "'";
                }
                if (!string.IsNullOrEmpty(statQC.DateEditEnd))
                {
                    where += " and " + strTime + " <'" + Convert.ToDateTime(statQC.DateEditEnd).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }    

            if (!string.IsNullOrEmpty(statQC.EditYBStart))
            {
                where += string.Format(" and {0} = '{1}'", strNo, statQC.EditYBStart);
                subWhere += string.Format(" and {0} = '{1}'", strNo, statQC.EditYBStart);
            }
            statQC.Where += where;
            statQC.SubWhere += subWhere;
            return statQC;
        }
        public EntityDCLPrintData GetReagentData(EntityStatisticsQC StatQc)
        {
            EntityDCLPrintData printer = new EntityDCLPrintData();
            DataSet result = new DataSet();
            string sql = string.Empty;

            try
            {
                StatQc = GetReagentWhere(StatQc);
                string code = StatQc.ReportCode.ToString();

                List<EntitySysReport> dtEx = GetSysReportByCode(code);
                if (dtEx.Count > 0)
                {
                    sql = EncryptClass.Decrypt(dtEx[0].RepSql.ToString());

                    sql = sql.Replace("&subWhere&", StatQc.SubWhere);
                    sql = sql.Replace("&itemWhere&", StatQc.ItemWhere);
                    sql = sql.Replace("&where&", StatQc.Where);
                    sql = sql.Replace("&sDate&", StatQc.DateEditStart);
                    sql = sql.Replace("&eDate&", StatQc.DateEditEnd);

                    sql = sql.Replace("&qcWhere&", StatQc.QcWhere); //新增 质控

                    DataTable an = new DataTable();


                    if (!string.IsNullOrEmpty(dtEx[0].RepConectCode))
                    {
                        EntityDictDataInterfaceConnection list = CacheDirectDBDataInterface.Current.GetConnectionByCode(dtEx[0].RepConectCode);
                        DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list);
                        SqlHelper helper = conn.GetSqlHelper();
                        an = helper.GetTable(sql);
                    }
                    else
                    {
                        DBManager helper = new DBManager();
                        an = helper.ExecuteDtSql(sql);
                    }
                    an.TableName = "可设计字段";
                    result.Tables.Add(an.Copy());
                    printer.ReportData = result;
                    printer.ReportName = dtEx[0].RepLocation.ToString().Replace(".repx", "");

                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(new Exception(ex.ToString() + "出错SQL语句：" + sql));
                DataTable dt = new DataTable("ErrorMessage");
                dt.Columns.Add("ErrorMessage");
                DataRow dr = dt.NewRow();
                dr["ErrorMessage"] = "报表查询语句有误，请联系管理员";
                dt.Rows.Add(dr);
                result.Tables.Add(dt);
                printer.ReportData = result;
            }
            return printer;
        }

        public EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc)
        {
            EntityDCLPrintData printer = new EntityDCLPrintData();
            DataSet result = new DataSet();
            string sql = string.Empty;
            try
            {
                StatQc = GetWhere(StatQc);
                string code = StatQc.ReportCode.ToString();

                List<EntitySysReport> dtEx = GetSysReportByCode(code);
                if (dtEx.Count > 0)
                {
                    sql = EncryptClass.Decrypt(dtEx[0].RepSql.ToString());

                    sql = sql.Replace("&subWhere&", StatQc.SubWhere);
                    sql = sql.Replace("&itemWhere&", StatQc.ItemWhere);
                    sql = sql.Replace("&where&", StatQc.Where);
                    sql = sql.Replace("&sDate&", StatQc.DateEditStart);
                    sql = sql.Replace("&eDate&", StatQc.DateEditEnd);

                    sql = sql.Replace("&qcWhere&", StatQc.QcWhere); //新增 质控

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
                Lib.LogManager.Logger.LogException( new Exception(ex.ToString() + "出错SQL语句：" + sql));
                DataTable dt = new DataTable("ErrorMessage");
                dt.Columns.Add("ErrorMessage");
                DataRow dr = dt.NewRow();
                dr["ErrorMessage"] = "报表查询语句有误，请联系管理员";
                dt.Rows.Add(dr);
                result.Tables.Add(dt);
                printer.ReportData = result;
            }
            return printer;
        }

        public EntityStatisticsQC GetStatQC(List<EntityObrResult> dtResult, EntityStatisticsQC StatQC)
        {

            string where = "";
            string subWhere = string.Empty;

            string qcWhere = "";//质控仪器与项目查询条件

            if (!string.IsNullOrEmpty(StatQC.ItrIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_Ditr_id in ({0})", StatQC.ItrIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_Ditr_id in ({0})", StatQC.ItrIdString);
                qcWhere += string.Format(@" and Lis_qc_result.Lres_Ditr_id in ({0})", StatQC.ItrIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.DeptIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_pat_dept_id in ({0})", StatQC.DeptIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_pat_dept_id in ({0})", StatQC.DeptIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.DiagIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_pat_diag in ({0})", StatQC.DiagIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_pat_diag in ({0})", StatQC.DiagIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.SamIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_Dsam_id in ({0})", StatQC.SamIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_Dsam_id in ({0})", StatQC.SamIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.PhyIdString))
            {
                where += string.Format(@" and Dict_itr_instrument.Ditr_lab_id in ({0})", StatQC.PhyIdString);
                subWhere += string.Format(@" and Dict_itr_instrument.Ditr_lab_id in ({0})", StatQC.PhyIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.SepIdString))
            {
                where += string.Format(@" and Dict_itr_instrument.Ditr_Dpro_id in ({0})", StatQC.SepIdString);
                subWhere += string.Format(@" and Dict_itr_instrument.Ditr_Dpro_id in ({0})", StatQC.SepIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.ChkDocIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_audit_Buser_id in ({0})", StatQC.ChkDocIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_audit_Buser_id in ({0})", StatQC.ChkDocIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.AuditIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_report_Buser_id in ({0})", StatQC.AuditIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_report_Buser_id in ({0})", StatQC.AuditIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.SendDocIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_Ddoctor_id in ({0})", StatQC.SendDocIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_Ddoctor_id in ({0})", StatQC.SendDocIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.ComIdString))
            {
                where += string.Format(@" and Dict_itm_combine.Dcom_id in ({0})", StatQC.ComIdString);
                subWhere += string.Format(@" and Dict_itm_combine.Dcom_id in ({0})", StatQC.ComIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.OriIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_pat_Dsorc_id in ({0})", StatQC.OriIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_pat_Dsorc_id in ({0})", StatQC.OriIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.MarkIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_ctype in ({0})", StatQC.MarkIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_ctype in ({0})", StatQC.MarkIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.SexIdString))
            {
                where += string.Format(@" and Pat_lis_main.Pma_pat_sex in ({0})", StatQC.SexIdString);
                subWhere += string.Format(@" and Pat_lis_main.Pma_pat_sex in ({0})", StatQC.SexIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.TipIdString))
            {
                where += string.Format(@" and Lis_result.Lres_ref_flag in ({0})", StatQC.TipIdString);
                subWhere += string.Format(@" and Lis_result.Lres_ref_flag in ({0})", StatQC.TipIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.BactypeIdString))
            {
                where += string.Format(@" and Dict_mic_bacttype.Dbactt_id in ({0})", StatQC.BactypeIdString);
                subWhere += string.Format(@" and Dict_mic_bacttype.Dbactt_id in ({0})", StatQC.BactypeIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.AntiIdString))
            {
                where += string.Format(@" and Dict_mic_antibio.Dant_id in ({0})", StatQC.AntiIdString);
                subWhere += string.Format(@" and Dict_mic_antibio.Dant_id in ({0})", StatQC.AntiIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.BacIdString))
            {
                where += string.Format(@" and Dict_mic_bacteria.Dbact_id in ({0})", StatQC.BacIdString);
                subWhere += string.Format(@" and Dict_mic_bacteria.Dbact_id in ({0})", StatQC.BacIdString);
            }
            //院区
            if (!string.IsNullOrEmpty(StatQC.OrgId))
            {
                where += string.Format(@" and Dict_organize.Dorg_id = '{0}'", StatQC.OrgId);
                subWhere += string.Format(@" and Dict_organize.Dorg_id = '{0}'", StatQC.OrgId);
            }
            //标本状态
            if (!string.IsNullOrEmpty(StatQC.SampStateIdString))
            {
                where += string.Format(@" and Dict_sample_status.Dstau_id in ({0})", StatQC.SampStateIdString);
                subWhere += string.Format(@" and Dict_sample_status.Dstau_id in ({0})", StatQC.SampStateIdString);
            }

            //标本备注
            if (!string.IsNullOrEmpty(StatQC.SampRemarkIdString))
            {
                where += string.Format(@" and Dict_sample_remark.Dsamr_id in ({0})", StatQC.SampRemarkIdString);
                subWhere += string.Format(@" and Dict_sample_remark.Dsamr_id in ({0})", StatQC.SampRemarkIdString);
            }

            if (!string.IsNullOrEmpty(StatQC.ReaSupIdString))
            {
                where += string.Format(@" and Dict_rea_setting.Drea_supplier in ({0})", StatQC.ReaSupIdString);
                subWhere += string.Format(@" and Dict_rea_setting.Drea_supplier in ({0})", StatQC.ReaSupIdString);
                qcWhere += string.Format(@" and Dict_rea_setting.Drea_supplier in ({0})", StatQC.ReaSupIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.ReagentIdString))
            {
                where += string.Format(@" and Dict_rea_setting.Drea_id in ({0})", StatQC.ReagentIdString);
                subWhere += string.Format(@" and Dict_rea_setting.Drea_id in ({0})", StatQC.ReagentIdString);
                qcWhere += string.Format(@" and Dict_rea_setting.Drea_id in ({0})", StatQC.ReagentIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.GroupIdString))
            {
                where += string.Format(@" and Dict_rea_setting.Drea_group in ({0})", StatQC.GroupIdString);
                subWhere += string.Format(@" and Dict_rea_setting.Drea_group in ({0})", StatQC.GroupIdString);
                qcWhere += string.Format(@" and Dict_rea_setting.Drea_group in ({0})", StatQC.GroupIdString);
            }
            if (!string.IsNullOrEmpty(StatQC.PdtIdString))
            {
                where += string.Format(@" and Dict_rea_setting.Drea_product in ({0})", StatQC.PdtIdString);
                subWhere += string.Format(@" and Dict_rea_setting.Drea_product in ({0})", StatQC.PdtIdString);
                qcWhere += string.Format(@" and Dict_rea_setting.Drea_product in ({0})", StatQC.PdtIdString);
            }
            string strItemSubWhere = string.Empty;

            if (dtResult != null)
            {
                if (dtResult.Count > 0)
                {
                    if (dtResult.Where(i => i.ItmEname != null || i.ItmEname != "").ToList().Count > 0)
                    {
                        string strItemWhere = getItemWhere(ref strItemSubWhere, dtResult);
                        where += strItemWhere;
                        subWhere += strItemWhere;

                        #region 添加质控查询条件 2018-05-18
                        string strTemp = " and Lis_qc_result.Lres_Ditm_id in ('";
                        for (int i = 0; i < dtResult.Count; i++)
                        {
                            if (i != (dtResult.Count - 1))
                            {
                                strTemp += dtResult[i].ItmEname + "','";
                            }
                            else
                            {
                                strTemp += dtResult[i].ItmEname + "')";
                            }

                        }
                        qcWhere += strTemp;
                        #endregion
                    }
                }
            }
            StatQC.Where = where + strItemSubWhere;
            StatQC.SubWhere = subWhere + strItemSubWhere;
            StatQC.ItemWhere = strItemSubWhere;
            StatQC.QcWhere = qcWhere;  //新增 质控
            return StatQC;
        }

        string getItemWhere(ref string strSubWhere, List<EntityObrResult> dtResult)
        {
            string where = " and (";

            if (dtResult != null)
            {
                string strOr = "";
                if (dtResult.Count > 0)
                {
                    List<EntityObrResult> drItem = dtResult.Where(i => i.ItmEname != null || i.ItmEname != "").OrderBy(e => e.ObrId).ToList();

                    string strtempItmIn = "";//添加只挑选查询的项目

                    string strtempItmResIn = "";//添加挑选只满足条件的项目

                    for (int i = 0; i < drItem.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(drItem[i].ItmEname.ToString()))
                        {
                            if (string.IsNullOrEmpty(strtempItmIn))
                            {
                                strtempItmIn = "'" + drItem[i].ItmEname.ToString() + "'";
                            }
                            else
                            {
                                strtempItmIn += ",'" + drItem[i].ItmEname.ToString() + "'";
                            }
                        }

                        where += "(exists(select top 1 1 from Lis_result as r with(nolock) where r.Lres_Pma_rep_id=Pat_lis_main.Pma_rep_id and r.Lres_flag=1 and r.Lres_Ditm_id='" + drItem[i].ItmEname.ToString() + "' ";
                        string parWh = drItem[i].ObrValue2.ToString().Trim();
                        if (parWh != "" && drItem[i].ObrValue.ToString().Trim() != "")
                        {
                            if (parWh != "like" && parWh != "not like")
                            {
                                where += " and convert(FLOAT, r.Lres_value)" + parWh + drItem[i].ObrValue.ToString().Trim();

                                if (string.IsNullOrEmpty(strtempItmResIn))
                                {
                                    strtempItmResIn = "Lis_result.Lres_Ditm_id='" + drItem[i].ItmEname.ToString() + "' and convert(FLOAT, Lis_result.Lres_value)" + parWh + drItem[i].ObrValue.ToString().Trim();
                                }
                                else
                                {
                                    strtempItmResIn += " or Lis_result.Lres_Ditm_id='" + drItem[i].ItmEname.ToString() + "' and convert(FLOAT, Lis_result.Lres_value)" + parWh + drItem[i].ObrValue.ToString().Trim();
                                }
                            }
                            else
                            {
                                where += " and r.Lres_value " + parWh + " '%" + drItem[i].ObrValue.ToString().Trim() + "%'";

                                if (string.IsNullOrEmpty(strtempItmResIn))
                                {
                                    strtempItmResIn = "Lis_result.Lres_Ditm_id='" + drItem[i].ItmEname.ToString() + "' and Lis_result.Lres_value " + parWh + " '%" + drItem[i].ObrValue.ToString().Trim() + "%'";
                                }
                                else
                                {
                                    strtempItmResIn += " or Lis_result.Lres_Ditm_id='" + drItem[i].ItmEname.ToString() + "' and Lis_result.Lres_value " + parWh + " '%" + drItem[i].ObrValue.ToString().Trim() + "%'";
                                }
                            }
                        }
                        if (i != (drItem.Count - 1))
                        {
                            if (drItem[i].ResOr.ToString() == "并且")
                                strOr = " and ";
                            else
                                strOr = " or ";
                            where += "))" + strOr;
                        }
                        else
                            where += ")))";


                        strSubWhere += string.Format(",'{0}'", drItem[i].ItmEname.ToString());
                    }

                    if (strSubWhere.Length > 0)
                    {
                        strSubWhere = strSubWhere.Remove(0, 1);
                        strSubWhere = string.Format(" and Lis_result.Lres_Ditm_id in ({0})", strSubWhere);
                    }

                    if (!string.IsNullOrEmpty(strtempItmResIn))
                    {
                        strSubWhere += string.Format(" and ({0}) ", strtempItmResIn);
                    }
                }
            }
            return where;
        }

        public DataSet GetAnalyseData(EntityStatisticsQC StatQc)
        {
            DBManager helper = new DBManager();

            StatQc = GetWhere(StatQc);

            string where = "where 1=1 " + StatQc.Where;

            string sql_an = string.Format(@"select Dant_cname from Lis_result_anti
inner JOIN Pat_lis_main  ON Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
left JOIN Dict_mic_antibio ON Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id 
left join Dict_sample on Pat_lis_main.Pma_Dsam_id=Dict_sample.Dsam_id
left join Dict_mic_bacteria on Lis_result_anti.Lanti_Dbact_id=Dict_mic_bacteria.Dbact_id
left join Dict_mic_bacttype on Dict_mic_bacteria.Dbact_Dbactt_id=Dict_mic_bacttype.Dbactt_id
{0}
group by Dant_cname", where);

            DataTable ds_An = helper.ExecuteDtSql(sql_an);

            string sql_bar = string.Format(@"select Dbact_cname from Lis_result_bact 
inner JOIN Pat_lis_main  ON Pat_lis_main.Pma_rep_id = Lis_result_bact.Lbac_id
left JOIN Dict_mic_bacteria ON Lis_result_bact.Lbac_Dbact_id = Dict_mic_bacteria.Dbact_id
left join Dict_mic_bacttype on Dict_mic_bacteria.Dbact_Dbactt_id=Dict_mic_bacttype.Dbactt_id
left join Dict_sample on Pat_lis_main.Pma_Dsam_id=Dict_sample.Dsam_id
{0}
group by Dbact_cname", where);

            DataTable ds_Bar = helper.ExecuteDtSql(sql_bar);

            DataTable dtPatients = new DataTable();

            dtPatients.Columns.Add("pat_id");
            dtPatients.Columns.Add("日期");
            dtPatients.Columns.Add("时间");
            dtPatients.Columns.Add("申请医生");
            dtPatients.Columns.Add("费别");
            dtPatients.Columns.Add("病例号");
            dtPatients.Columns.Add("检查目的");
            dtPatients.Columns.Add("标本类别");
            dtPatients.Columns.Add("姓名");
            dtPatients.Columns.Add("年龄");
            dtPatients.Columns.Add("性别");
            dtPatients.Columns.Add("科室");
            dtPatients.Columns.Add("样本号");
            dtPatients.Columns.Add("条码号");
            dtPatients.Columns.Add("签收时间");
            dtPatients.Columns.Add("审核时间");
            dtPatients.Columns.Add("菌类");
            dtPatients.Columns.Add("检出菌株");
            dtPatients.Columns.Add("菌落计数");
            dtPatients.Columns.Add("菌株备注");

            foreach (DataRow drBar in ds_Bar.Rows)
            {
                dtPatients.Columns.Add(drBar["Dbact_cname"].ToString());
            }

            foreach (DataRow drAn in ds_An.Rows)
            {
                dtPatients.Columns.Add(drAn["Dant_cname"].ToString());
            }

            string sql_an_resulto = string.Format(@"select Lis_result_anti.Lanti_id,
Lanti_value,
Dant_cname,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) 年龄, 
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '未知' end) 性别, 
Pat_lis_main.Pma_pat_name 姓名,
Pat_lis_main.Pma_pat_dept_name 科室,
Pma_in_date 时间,
Dict_doctor.Ddoctor_name 申请医生,
Pat_lis_main.Pma_pat_Dinsu_id 费别,
Pat_lis_main.Pma_pat_in_no 病例号,
Dict_check_purpose.Dpurp_name 检查目的,
Dict_sample.Dsam_name 标本类别,
Pat_lis_main.Pma_sid 样本号,
Pma_bar_code 条码号,
Sample_main.Sma_receiver_date 签收时间,
Pat_lis_main.Pma_report_date 审核时间
from Lis_result_anti
inner JOIN Pat_lis_main  ON Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
left JOIN Dict_mic_antibio ON Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id 
left join Dict_doctor on Pat_lis_main.Pma_Ddoctor_id=Dict_doctor.Ddoctor_id
left join Dict_check_purpose on Pat_lis_main.Pma_Dpurp_id=Dict_check_purpose.Dpurp_id
left join Dict_sample on Pat_lis_main.Pma_Dsam_id=Dict_sample.Dsam_id
left join Dict_mic_bacteria on Lis_result_anti.Lanti_Dbact_id=Dict_mic_bacteria.Dbact_id
left join Dict_mic_bacttype on Dict_mic_bacteria.Dbact_Dbactt_id=Dict_mic_bacttype.Dbactt_id
left join Sample_main on Pat_lis_main.Pma_bar_code=Sample_main.Sma_bar_code
{0}  and Pat_lis_main.Pma_status in ('2','4')", where);

            DataTable dtAnRes = helper.ExecuteDtSql(sql_an_resulto);

            foreach (DataRow drAnRes in dtAnRes.Rows)
            {
                if (drAnRes["Dant_cname"].ToString().Trim() != "")
                {
                    DataRow[] drPats = dtPatients.Select("pat_id='" + drAnRes["Lanti_id"] + "'");
                    if (drPats.Length > 0)
                    {
                        drPats[0][drAnRes["Dant_cname"].ToString()] = drAnRes["Lanti_value"];
                    }
                    else
                    {
                        DataRow drPat = dtPatients.NewRow();
                        drPat["年龄"] = drAnRes["年龄"];
                        drPat["日期"] = Convert.ToDateTime(drAnRes["时间"]).ToString("yyyy-MM-dd");
                        drPat["姓名"] = drAnRes["姓名"];
                        drPat["性别"] = drAnRes["性别"];
                        drPat["科室"] = drAnRes["科室"];
                        drPat["时间"] = Convert.ToDateTime(drAnRes["时间"]).ToString("hh:mm:ss");
                        drPat["申请医生"] = drAnRes["申请医生"];
                        drPat["费别"] = drAnRes["费别"];
                        drPat["病例号"] = drAnRes["病例号"];
                        drPat["检查目的"] = drAnRes["检查目的"];
                        drPat["标本类别"] = drAnRes["标本类别"];
                        drPat["pat_id"] = drAnRes["Lanti_id"];
                        drPat["样本号"] = drAnRes["样本号"];
                        drPat["条码号"] = drAnRes["条码号"];
                        drPat["签收时间"] = drAnRes["签收时间"];
                        drPat["审核时间"] = drAnRes["审核时间"];
                        drPat[drAnRes["Dant_cname"].ToString()] = drAnRes["Lanti_value"];
                        dtPatients.Rows.Add(drPat);
                    }
                }
            }

            string sql_bar_resulto = string.Format(@"select Lbac_id,
Dbact_cname,
Lbac_remark,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) 年龄, 
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '未知' end) 性别, 
Pat_lis_main.Pma_pat_name 姓名,
Pat_lis_main.Pma_pat_dept_name 科室,
Pma_in_date 时间,
Dict_doctor.Ddoctor_name 申请医生,
Lbac_colony_count 菌落记数,
Pat_lis_main.Pma_pat_Dinsu_id 费别,
Pat_lis_main.Pma_pat_in_no 病例号,
Dict_check_purpose.Dpurp_name 检查目的,
Dict_sample.Dsam_name 标本类别,
Pat_lis_main.Pma_report_date 审核时间,
Dict_mic_bacttype.Dbactt_ename 菌类,
Dict_mic_bacteria.Dbact_cname 检出菌株,
Lbac_colony_count 菌落计数,
Lis_result_bact.Lbac_remark 菌株备注,
Pat_lis_main.Pma_sid 样本号,
Pma_bar_code 条码号,
Sample_main.Sma_receiver_date 签收时间
from Lis_result_bact 
inner JOIN Pat_lis_main  ON Pat_lis_main.Pma_rep_id = Lis_result_bact.Lbac_id
left JOIN Dict_mic_bacteria ON Lis_result_bact.Lbac_Dbact_id = Dict_mic_bacteria.Dbact_id
left join Dict_doctor on Pat_lis_main.Pma_Ddoctor_id=Dict_doctor.Ddoctor_id
left join Dict_check_purpose on Pat_lis_main.Pma_Dpurp_id=Dict_check_purpose.Dpurp_id
left join Dict_sample on Pat_lis_main.Pma_Dsam_id=Dict_sample.Dsam_id
left join Dict_mic_bacttype on Dict_mic_bacteria.Dbact_Dbactt_id=Dict_mic_bacttype.Dbactt_id
left join Sample_main on Pat_lis_main.Pma_bar_code=Sample_main.Sma_bar_code
{0} and Pat_lis_main.Pma_status in ('2','4')", where);

            DataTable dtBarRes = helper.ExecuteDtSql(sql_bar_resulto);

            foreach (DataRow drBarRes in dtBarRes.Rows)
            {
                if (drBarRes["Dbact_cname"].ToString().Trim() != "")
                {
                    DataRow[] drPats = dtPatients.Select("pat_id='" + drBarRes["Lbac_id"] + "'");
                    if (drPats.Length > 0)
                    {

                        if (drBarRes["菌落记数"] != DBNull.Value && drBarRes["菌落记数"] != null && drBarRes["菌落记数"].ToString().Trim() != string.Empty)
                            drPats[0][drBarRes["Dbact_cname"].ToString()] = drBarRes["Lbac_remark"] + "(" + drBarRes["菌落记数"].ToString() + ")";
                        else
                            drPats[0][drBarRes["Dbact_cname"].ToString()] = drBarRes["Lbac_remark"];

                        drPats[0]["菌类"] = drBarRes["菌类"];
                        drPats[0]["检出菌株"] = drBarRes["检出菌株"];
                        drPats[0]["菌落计数"] = drBarRes["菌落计数"];
                        drPats[0]["菌株备注"] = drBarRes["菌株备注"];

                    }
                    else
                    {
                        DataRow drPat = dtPatients.NewRow();
                        drPat["日期"] = Convert.ToDateTime(drBarRes["时间"]).ToString("yyyy-MM-dd");
                        drPat["年龄"] = drBarRes["年龄"];
                        drPat["姓名"] = drBarRes["姓名"];
                        drPat["性别"] = drBarRes["性别"];
                        drPat["科室"] = drBarRes["科室"];
                        drPat["时间"] = Convert.ToDateTime(drBarRes["时间"]).ToString("hh:mm:ss");
                        drPat["申请医生"] = drBarRes["申请医生"];
                        drPat["费别"] = drBarRes["费别"];
                        drPat["病例号"] = drBarRes["病例号"];
                        drPat["检查目的"] = drBarRes["检查目的"];
                        drPat["标本类别"] = drBarRes["标本类别"];
                        drPat["pat_id"] = drBarRes["Lbac_id"];
                        drPat["样本号"] = drBarRes["样本号"];
                        drPat["条码号"] = drBarRes["条码号"];
                        drPat["签收时间"] = drBarRes["签收时间"];
                        drPat["审核时间"] = drBarRes["审核时间"];
                        drPat[drBarRes["Dbact_cname"].ToString()] = drBarRes["Lbac_remark"];
                        dtPatients.Rows.Add(drPat);
                    }
                }
            }


            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(dtPatients);
            return dsResult;
        }

    }
}
