using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;
using System.Data;
using System.Collections.Specialized;
using Lib.DataInterface.Implement;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReportPrint))]
    public class DaoReportPrint : IDaoReportPrint
    {
        public string GetBacReportCode(string repId)
        {
            string strReportCode = "asepsis";

            string strGetCs = string.Format("select top 1 1 from Lis_result_desc where Lrd_id='{0}'", repId);
            string strGetAn = string.Format("select top 1 1 from Lis_result_anti where Lanti_id='{0}'", repId);

            DBManager helper = new DBManager();
            DataTable dtCs = helper.ExecuteDtSql(strGetCs);
            if (dtCs != null && dtCs.Rows.Count > 0)
            {
                strReportCode = "smear";
            }
            else
            {
                DataTable dtAn = helper.ExecuteDtSql(strGetAn);
                if (dtAn != null && dtAn.Rows.Count > 0)
                {
                    strReportCode = "bacilli";
                }
            }

            return strReportCode;
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public EntityDCLPrintData GetReportData(EntityDCLPrintParameter parameter)
        {
            EntityDCLPrintData printData = new EntityDCLPrintData();

            DaoSysReport daoReport = new DaoSysReport();

            //获取报表信息
            EntitySysReport sysReport = daoReport.GetReportByRepCode(parameter.ReportCode);

            if (!string.IsNullOrEmpty(sysReport.RepCode))
            {
                //获取打印条件
                NameValueCollection printWhere = GetWhere(parameter);
                if (printWhere.Count > 0)
                {
                    string strSql = sysReport.RepSql;

                    //合成查询语句
                    foreach (string key in printWhere.AllKeys)
                    {
                        strSql = strSql.Replace(key, printWhere[key]);
                    }
                    DBManager helper ;
                    if (string.IsNullOrEmpty(sysReport.RepConectCode))
                    {
                        helper = new DBManager();
                    }
                    else
                    {
                       
                        EntityDictDataInterfaceConnection ettConn = CacheDirectDBDataInterface.Current.GetConnectionByCode(sysReport.RepConectCode);
                        if (ettConn != null)
                        {
                            string connStr = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}"
                           , ettConn.conn_address, ettConn.conn_db_catelog, ettConn.conn_login, ettConn.conn_pass);
                            helper = new DBManager(connStr);
                        }
                        else
                        {
                            helper = new DBManager();
                        }
                    }


                    //DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(strSql);
                    dt.TableName = "可设计字段";

                    printData.ReportData.Tables.Add(dt);
                    printData.ReportName = sysReport.RepName;
                    printData.ReportCode = sysReport.RepCode;
                }
            }

            return printData;


        }

        /// <summary>
        /// 获取报表打印需要的条件
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private NameValueCollection GetWhere(EntityDCLPrintParameter parameter)
        {
            NameValueCollection printWhere = new NameValueCollection();

            string strWhere = string.Empty;

            string strSubWhere = string.Empty;

            //病人标识
            if (!string.IsNullOrEmpty(parameter.RepId))
                strWhere += string.Format(" and Pat_lis_main.Pma_rep_id ='{0}'", parameter.RepId);

            //条码号
            if (parameter.ListBarId != null && parameter.ListBarId.Count > 0)
            {
                if (parameter.ListBarId.Count == 1)
                {
                    strWhere += string.Format(" and Sample_main.Sma_bar_id ='{0}'", parameter.ListBarId[0]);
                }
                else
                {
                    string strBarId = string.Empty;
                    foreach (string barId in parameter.ListBarId)
                    {
                        strBarId += string.Format(",'{0}'", barId);
                    }
                    strBarId = strBarId.Remove(0, 1);

                    strWhere += string.Format(" and Sample_main.Sma_bar_id in ({0})", strBarId);
                }
            }
            //条码号
            if (parameter.ListReagentBarId != null && parameter.ListReagentBarId.Count > 0)
            {
                if (parameter.ListReagentBarId.Count == 1)
                {
                    strWhere += string.Format(" and Rea_storage_detail.Rsd_barcode ='{0}'", parameter.ListReagentBarId[0]);
                }
                else
                {
                    string strBarId = string.Empty;
                    foreach (string barId in parameter.ListReagentBarId)
                    {
                        strBarId += string.Format(",'{0}'", barId);
                    }
                    strBarId = strBarId.Remove(0, 1);

                    strWhere += string.Format(" and Rea_storage_detail.Rsd_barcode  in ({0})", strBarId);
                }
            }
            //架子条码号
            if (parameter.ListRackBarCode != null && parameter.ListRackBarCode.Count > 0)
            {
                if (parameter.ListRackBarCode.Count == 1)
                {
                    strWhere += string.Format(" and Dict_sample_tube_rack.Drack_barcode ='{0}'", parameter.ListRackBarCode[0]);
                }
                else
                {
                    string strBarId = string.Empty;
                    foreach (string barId in parameter.ListRackBarCode)
                    {
                        strBarId += string.Format(",'{0}'", barId);
                    }
                    strBarId = strBarId.Remove(0, 1);

                    strWhere += string.Format(" and Dict_sample_tube_rack.Drack_barcode in ({0})", strBarId);
                }
            }
            //条码主键
            if (parameter.listSampSn != null && parameter.listSampSn.Count > 0)
            {
                if (parameter.listSampSn.Count == 1)
                {
                    strWhere += string.Format(" and Sample_main.Sma_id ='{0}'", parameter.listSampSn[0]);
                }
                else
                {
                    string strBarId = string.Empty;
                    foreach (string barId in parameter.listSampSn)
                    {
                        strBarId += string.Format(",'{0}'", barId);
                    }
                    strBarId = strBarId.Remove(0, 1);

                    strWhere += string.Format(" and Sample_main.Sma_id in ({0})", strBarId);
                }
            }

            if (parameter.ListRepId != null && parameter.ListRepId.Count > 0)
            {
                string strRepId = string.Empty;
                foreach (string repId in parameter.ListRepId)
                {
                    strRepId += string.Format(",'{0}'", repId);
                }
                strRepId = strRepId.Remove(0, 1);

                strWhere += string.Format(" and Pat_lis_main.Pma_rep_id in ({0}) ", strRepId);
            }

            foreach (var item in parameter.CustomParameter)
            {
                //入库单号
                if (item.Key == "StorageId")
                {
                    strWhere += string.Format(" and Rea_storage.Rsr_no = '{0}'", item.Value.ToString());
                }
                //采购单号
                if (item.Key == "PurchaseId")
                {
                    strWhere += string.Format(" and Rea_storage.Rpc_no = '{0}'", item.Value.ToString());
                }
                //申购单号
                if (item.Key == "SubscribeId")
                {
                    strWhere += string.Format(" and Rea_subscribe.Rsb_no = '{0}'", item.Value.ToString());
                }
                //出库单号
                if (item.Key == "DeliveryId")
                {
                    strWhere += string.Format(" and Rea_delivery.Rdl_no = '{0}'", item.Value.ToString());
                }
                //申领单号
                if (item.Key == "ApplyId")
                {
                    strWhere += string.Format(" and Rea_apply.Ray_no = '{0}'", item.Value.ToString());
                }
                //报损单号
                if (item.Key == "LossReportId")
                {
                    strWhere += string.Format(" and Rea_lossreport.Rlr_no = '{0}'", item.Value.ToString());
                }
                //病历号
                if (item.Key == "PidInNo")
                {
                    strWhere += string.Format(" and Sample_main.Sma_pat_in_no = '{0}'", item.Value.ToString());
                }
                //试管条码(归档查询)
                if (item.Key == "SamBarcode")
                {
                    strWhere += string.Format(" and Sample_store_detail.Ssdt_bar_code in  ({0})", item.Value.ToString());
                }
                //危急值报告提示
                if (item.Key == "ItrRepFlag")
                {
                    if (item.Value.ToString() == "4")
                    {
                        strWhere += " and Lis_result_desc.Lrd_flag='1'";
                    }
                }
                if (item.Key == "F_HospSampleID")
                {
                    strWhere += string.Format(" and F_HospSampleID='{0}'", item.Value.ToString());
                }
                if (item.Key == "KM_LIS_ExtReport")
                {
                    strWhere += string.Format(" and kingmed.dbo.KM_LIS_ExtReport.F_HospSampleID='{0}'", item.Value.ToString());
                }
                if (item.Key == "ReportInfoId")
                {
                    strWhere += string.Format(" and lis_report_pic.report_info_id='{0}'", item.Value.ToString());
                }
                if (item.Key == "LismainRepno")
                {
                    strWhere += string.Format(" and cdr_lis_main.lismain_repno='{0}'", item.Value.ToString());
                }
                if (item.Key == "ParameReport") //质控参数报表
                {
                    strWhere += string.Format(" and Dict_qc_materia.Dmat_id = '{0}' ", item.Value.ToString());
                }
                if (item.Key == "ctypeid")
                {
                    printWhere.Add("&ctypeid&", item.Value.ToString());
                }
                if (item.Key == "ctypename")
                {
                    printWhere.Add("&ctypename&", item.Value.ToString());
                }
                if (item.Key == "remark")
                {
                    printWhere.Add("&remark&", item.Value.ToString());
                }
                if (item.Key == "packcount")
                {
                    printWhere.Add("&packcount&", item.Value.ToString());
                }
                if (item.Key == "opname")
                {
                    printWhere.Add("&opname&", item.Value.ToString());
                }
                if (item.Key == "optime")
                {
                    printWhere.Add("&optime&", item.Value.ToString());
                }
                if (item.Key == "queue_date")
                {
                    printWhere.Add("&queue_date&", item.Value.ToString());
                }
                if (item.Key == "pid_in_no")
                {
                    printWhere.Add("&pid_in_no&", item.Value.ToString());
                }
                if (item.Key == "queue_no")
                {
                    printWhere.Add("&queue_no&", item.Value.ToString());
                }

                #region 微生物院感打印相关

                if (item.Key == "YGDept") //微生物院感科室
                {
                    strWhere += string.Format(" and yg_main.yg_dep_code = '{0}' ", item.Value.ToString());
                }
                if (item.Key == "YGSid") //微生物院感条码
                {
                    strWhere += string.Format(" and yg_main.yg_sid = '{0}' ", item.Value.ToString());
                }
                if (item.Key == "YGKey") //微生物院感key
                {
                    strWhere += string.Format(" and yg_main.yg_key = '{0}' ", item.Value.ToString());
                }
                if (item.Key == "YGStartTime") //微生物院感开始时间
                {
                    strWhere += string.Format(" and yg_main.yg_in_date >= '{0}' ", item.Value.ToString());
                }
                if (item.Key == "YGEndTime") //微生物院感开始时间
                {
                    strWhere += string.Format(" and yg_main.yg_in_date <= '{0}' ", item.Value.ToString());
                }
                #endregion

                #region 微生物质控打印相关

                if (item.Key == "MicQCType")
                {
                    string qctype = item.Value.ToString();

                    //试剂质控
                    if (qctype == "ReagentQC")
                    {
                        if (item.Key == "QCStartTime") //开始时间
                        {
                            strWhere += string.Format(" and obr_mic_reagent_qa.rqa_indate >= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCEndTime") //开始时间
                        {
                            strWhere += string.Format(" and obr_mic_reagent_qa.rqa_indate <= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCItemId") //质控试剂ID
                        {
                            strWhere += string.Format(" and rqa_reg_id = '{0}' ", item.Value.ToString());
                        }
                    }

                    if (qctype == "MediaQC")
                    {
                        if (item.Key == "QCStartTime") //开始时间
                        {
                            strWhere += string.Format(" and oqa_indate >= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCEndTime") //开始时间
                        {
                            strWhere += string.Format(" and oqa_indate <= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCItemId") //培养基字典ID
                        {
                            strWhere += string.Format(" and oqa_media_id = '{0}' ", item.Value.ToString());
                        }
                    }
                    //生化实验质控
                    if (qctype == "PhyChemQC")
                    {
                        if (item.Key == "QCStartTime") //开始时间
                        {
                            strWhere += string.Format(" and pqa_indate >= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCEndTime") //开始时间
                        {
                            strWhere += string.Format(" and pqa_indate <= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCItemId") //生化实验质控项目字典表ID
                        {
                            strWhere += string.Format(" and pqa_pqoit_id = '{0}' ", item.Value.ToString());
                        }
                    }

                    if (qctype == "DrugSensitivityQC")
                    {
                        if (item.Key == "QCStartTime") //开始时间
                        {
                            strWhere += string.Format(" and drugs_indate >= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCEndTime") //开始时间
                        {
                            strWhere += string.Format(" and drugs_indate <= '{0}' ", item.Value.ToString());
                        }
                    }

                    //培养基质控
                    if (qctype == "MediaConfig")
                    {
                        if (item.Key == "QCStartTime") //开始时间
                        {
                            strWhere += string.Format(" and cfg_date >= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCEndTime") //开始时间
                        {
                            strWhere += string.Format(" and cfg_date <= '{0}' ", item.Value.ToString());
                        }
                        if (item.Key == "QCItemId") //生化实验质控项目字典表ID
                        {
                            strWhere += string.Format(" and cfg_media_id = '{0}' ", item.Value.ToString());
                        }
                    }
                }

                #endregion

                #region 培养基配置条码打印

                if (item.Key == "BatchNum")//批号
                {
                    printWhere.Add("&BatchNum&", item.Value.ToString());
                }

                if (item.Key == "PackNum")//分装数
                {
                    printWhere.Add("&PackNum&", item.Value.ToString());
                }

                #endregion

                #region 危急值导出相关
                if (item.Key == "MsgStartTime") //开始时间
                {
                    strWhere += string.Format(" and Lis_message_content.Lmsg_create_time >= '{0}' ", item.Value.ToString());
                }
                if (item.Key == "MsgEndTime") //开始时间
                {
                    strWhere += string.Format(" and Lis_message_content.Lmsg_create_time <= '{0}' ", item.Value.ToString());
                }
                if (item.Key == "MsgDepart") //科室
                {
                    strWhere += string.Format(" and Lis_message_content.Lmsg_receive in ({0}) ", item.Value.ToString());
                }
                #endregion

                #region 保养登记
                if (item.Key == "ItrId") //保养登记仪器id
                {
                    strWhere += string.Format(" and Dict_itr_instrument.Ditr_id = '{0}' ", item.Value.ToString());
                }
                #endregion

                #region 质控评价
                if (item.Key == "ItmId") //质控评价项目id
                {
                    strWhere += string.Format(" and Dict_itm.Ditm_id = '{0}' ", item.Value.ToString());
                }
                if (item.Key == "QanLevel") //质控评价质控水平
                {
                    strWhere += string.Format(" and Lis_qc_analysis.Lqa_level = '{0}' ", item.Value.ToString());
                }
                #endregion
            }

            #region 微生物报表相关

            //微生物病人主报告ID
            if (!string.IsNullOrEmpty(parameter.MicRepId))
                strWhere += string.Format(" and Pid_mic_report_main.rep_id ='{0}'", parameter.MicRepId);

            //微生物病人主报告ID
            if (!string.IsNullOrEmpty(parameter.MicRepPatKey))
                strWhere += string.Format(" and Pid_mic_report_main.rep_pat_key ='{0}'", parameter.MicRepPatKey);

            //微生物病人主报告ID
            if (!string.IsNullOrEmpty(parameter.MicRepSid))
                strWhere += string.Format(" and Pid_mic_report_main.rep_sid ='{0}'", parameter.MicRepSid);

            //微生物病人科室ID
            if (!string.IsNullOrEmpty(parameter.MicDept))
                strWhere += string.Format(" and Pid_mic_report_main.pid_dept_id ='{0}'", parameter.MicDept);



            #endregion

            //不允许加空条件
            if (strWhere != string.Empty)
                printWhere.Add("&where&", strWhere);

            return printWhere;
        }
    }
}
