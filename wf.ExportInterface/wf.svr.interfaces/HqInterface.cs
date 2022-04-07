using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.cache;
using dcl.svr.report;
using Lib.ProxyFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace dcl.svr.interfaces
{
    public class HqInterface
    {

        /// <summary>
        /// 获取门诊医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="CardNo"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <param name="execDept"></param>
        /// <returns></returns>
        public string GetMzApplyInfo(string PatientId, string CardNo, DateTime begin_date, DateTime end_date, string execDept, out string sendXml)
        {
            string rvStr = string.Empty;
            string result = string.Empty;
            try
            {
                result = CreateMzPatInfoXml("lis_getmzapplyinfo", PatientId, CardNo, begin_date, end_date, execDept);
                rvStr = CreatePostHttpResponse(result);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            sendXml = result;
            Lib.LogManager.Logger.LogInfo("获取门诊医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("获取门诊医嘱返回xml：" + rvStr);
            return rvStr;
        }

        /// <summary>
        /// 获取门诊病人信息
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public string GetMzPatientsInfo(string PatientId, DateTime begin_date, DateTime end_date, string dept)
        {
            string rvStr = string.Empty;
            try
            {
                string result = CreateMzPatInfoXml("LIS_GetMzPatInfo", PatientId, "", begin_date, end_date, dept);
                rvStr = CreatePostHttpResponse(result);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return rvStr;
        }


        /// <summary>
        /// 获取住院医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public string GetZyApplyInfo(string PatientId, DateTime begin_date, DateTime end_date, string dept, out string sendXml)
        {
            string rvStr = string.Empty;
            string result = string.Empty;
            try
            {
                result = CreateZyPatInfoXml("LIS_GetZyApplyInfo", PatientId, begin_date, end_date, dept);
                rvStr = CreatePostHttpResponse(result);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            sendXml = result;
            Lib.LogManager.Logger.LogInfo("获取住院医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("获取住院医嘱返回xml：" + rvStr);
            return rvStr;
        }

        /// <summary>
        /// 确认门诊医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string ConfirmMzApplyInfo(string ordeSn, string Operator, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateMzOrderXML("lis_confirmmzapplyinfo", ordeSn, Operator);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("确认门诊医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("确认门诊医嘱返回xml：" + rvStr);
            return rvStr;
        }


        /// <summary>
        /// 确认住院医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string ConfirmZyApplyInfo(string ordeSn, string OperatorDate, string Operator, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateZyOrderXML("LIS_ConfirmZyApplyInfo", ordeSn, OperatorDate, Operator);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("确认住院医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("确认住院医嘱返回xml：" + rvStr);
            return rvStr;
        }


        /// <summary>
        /// 取消门诊医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string CancelMzApplyInfo(string ordeSn, string Operator, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateMzOrderXML("lis_cancelmzapplyinfo", ordeSn, Operator);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("取消门诊医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("取消门诊医嘱返回xml：" + rvStr);
            return rvStr;
        }


        /// <summary>
        /// 取消住院医嘱
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public string CancelZyApplyInfo(string ordeSn, string Operator, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateZyOrderXML("LIS_CancelZyApplyInfo", ordeSn, "", Operator);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("取消住院医嘱发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("取消住院医嘱返回xml：" + rvStr);
            return rvStr;
        }

        /// <summary>
        /// 确认住院试管费用
        /// </summary>
        /// <param name="patInNo">病人id</param>
        /// <param name="admissTimes">就诊次数</param>
        /// <param name="ordeSn">医嘱号</param>
        /// <param name="Operator">操作工号</param>
        /// <param name="chargeCode">收费代码</param>
        /// <param name="chargeCount">打印次数</param>
        /// <param name="comment">备注</param>
        /// <param name="sendXml">返回xml</param>
        /// <returns></returns>
        public string ConfirmZyCuvCharge(string patInNo, string admissTimes, string ordeSn, string Operator,
                                                                 string chargeCode, string chargeCount, string comment, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateZyCuvChargeXml("LIS_ConfirmZyCuvCharge", patInNo, admissTimes, ordeSn, Operator, chargeCode, chargeCount, comment);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("确认住院试管费用发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("确认住院试管费用返回xml：" + rvStr);
            return rvStr;
        }


        /// <summary>
        /// 取消住院试管费用
        /// </summary>
        /// <param name="patInNo">病人id</param>
        /// <param name="admissTimes">就诊次数</param>
        /// <param name="ordeSn">医嘱号</param>
        /// <param name="Operator">操作工号</param>
        /// <param name="chargeCode">收费代码</param>
        /// <param name="chargeCount">打印次数</param>
        /// <param name="comment">备注</param>
        /// <param name="sendXml">返回xml</param>
        /// <returns></returns>
        public string CancelZyCuvCharge(string patInNo, string admissTimes, string ordeSn, string Operator,
                                                                 string chargeCode, string chargeCount, string comment, out string sendXml)
        {
            string rvStr = string.Empty;
            sendXml = "";
            try
            {
                sendXml = CreateZyCuvChargeXml("LIS_CancelZyCuvCharge", patInNo, admissTimes, ordeSn, Operator, chargeCode, chargeCount, comment);
                rvStr = CreatePostHttpResponse(sendXml);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            Lib.LogManager.Logger.LogInfo("取消住院试管费用发送xml：" + sendXml);
            Lib.LogManager.Logger.LogInfo("取消住院试管费用返回xml：" + rvStr);
            return rvStr;
        }
        private string CreateMzPatInfoXml(string model, string patientId, string cardNo, DateTime beginDate, DateTime endDate, string execDept)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);
            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);


            XmlElement xePat = xmldoc.CreateElement("patient_id");
            xePat.InnerText = patientId;
            xe1.AppendChild(xePat);

            XmlElement xeCarNo = xmldoc.CreateElement("card_no");
            xeCarNo.InnerText = cardNo;
            xe1.AppendChild(xeCarNo);

            XmlElement xeBDate = xmldoc.CreateElement("begin_date");
            xeBDate.InnerText = beginDate.ToString("yyy-MM-dd");
            xe1.AppendChild(xeBDate);

            XmlElement xeReceiptSn = xmldoc.CreateElement("ReceiptSN");
            xeReceiptSn.InnerText = "";
            xe1.AppendChild(xeReceiptSn);

            XmlElement xeEmpUnitCode = xmldoc.CreateElement("emp_unit_code");
            xeEmpUnitCode.InnerText = "";
            xe1.AppendChild(xeEmpUnitCode);

            XmlElement xeEDate = xmldoc.CreateElement("end_date");
            xeEDate.InnerText = endDate.ToString("yyy-MM-dd");
            xe1.AppendChild(xeEDate);

            XmlElement xeInvNo = xmldoc.CreateElement("exec_dept");
            xeInvNo.InnerText = execDept;
            xe1.AppendChild(xeInvNo);

            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);


            return XMLDocumentToString(xmldoc);
        }

        private string CreateZyPatInfoXml(string model, string patientId, DateTime beginDate, DateTime endDate, string execDept)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);
            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);


            XmlElement xePat = xmldoc.CreateElement("inpatient_no");
            xePat.InnerText = patientId;
            xe1.AppendChild(xePat);

            XmlElement xeBDate = xmldoc.CreateElement("begin_date");
            xeBDate.InnerText = beginDate.ToString("yyy-MM-dd");
            xe1.AppendChild(xeBDate);

            XmlElement xeEDate = xmldoc.CreateElement("end_date");
            xeEDate.InnerText = endDate.ToString("yyy-MM-dd");
            xe1.AppendChild(xeEDate);

            XmlElement xeInvNo = xmldoc.CreateElement("exec_dept");
            xeInvNo.InnerText = execDept;
            xe1.AppendChild(xeInvNo);

            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);


            return XMLDocumentToString(xmldoc);
        }


        /// <summary>
        /// 生成调用门诊医嘱接口xml
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private string CreateMzOrderXML(string model, string OrderSn, string Operator)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);
            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);
            XmlElement xeSn = xmldoc.CreateElement("Item_Sn");
            xeSn.InnerText = OrderSn;
            xe1.AppendChild(xeSn);

            //XmlElement xeCo = xmldoc.CreateElement("ChargeOperator");
            //xeCo.InnerText = Operator;
            //xe1.AppendChild(xeCo);

            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            return XMLDocumentToString(xmldoc);
        }


        /// <summary>
        /// 生成调用住院医嘱接口xml
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        private string CreateZyOrderXML(string model, string OrderSn, string operatorDate, string Operator)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);
            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);
            XmlElement xeSn = xmldoc.CreateElement("Item_Sn");
            xeSn.InnerText = OrderSn;
            xe1.AppendChild(xeSn);

            if (model == "LIS_ConfirmZyApplyInfo")
            {
                XmlElement xeDate = xmldoc.CreateElement("charge_date");
                xeDate.InnerText = operatorDate;
                xe1.AppendChild(xeDate);

                XmlElement xeCo = xmldoc.CreateElement("charge_operator");
                xeCo.InnerText = Operator;
                xe1.AppendChild(xeCo);
            }
            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            return XMLDocumentToString(xmldoc);
        }

        /// <summary>
        /// 确认/取消住院试管费生成xml
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patInNo"></param>
        /// <param name="admissTimes"></param>
        /// <param name="orderSn"></param>
        /// <param name="Operator"></param>
        /// <param name="chargeCode"></param>
        /// <param name="chargeAmount"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private string CreateZyCuvChargeXml(string model, string patInNo, string admissTimes, string orderSn, string Operator,
                                                                 string chargeCode, string chargeAmount, string comment)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);
            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);


            XmlElement xePat = xmldoc.CreateElement("pat_id");
            xePat.InnerText = patInNo;
            xe1.AppendChild(xePat);

            XmlElement xeTimes = xmldoc.CreateElement("admiss_times");
            xeTimes.InnerText = admissTimes;
            xe1.AppendChild(xeTimes);

            XmlElement xeOrderSn = xmldoc.CreateElement("order_sn");
            xeOrderSn.InnerText = orderSn;
            xe1.AppendChild(xeOrderSn);

            XmlElement xeOper = xmldoc.CreateElement("opera");
            xeOper.InnerText = Operator;
            xe1.AppendChild(xeOper);

            XmlElement xeChargeCode = xmldoc.CreateElement("charge_code");
            xeChargeCode.InnerText = chargeCode;
            xe1.AppendChild(xeChargeCode);


            XmlElement xeChargeAmount = xmldoc.CreateElement("charge_amount");
            xeChargeAmount.InnerText = chargeAmount;
            xe1.AppendChild(xeChargeAmount);

            XmlElement xeComment = xmldoc.CreateElement("comment");
            xeComment.InnerText = comment;
            xe1.AppendChild(xeComment);

            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);


            return XMLDocumentToString(xmldoc);
        }




        public string GetDangerousValuesXML(EntityQcResultList qcResult, string Method, List<EntityDicObrMessageContent> listExt)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = Method;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);

            #region 病人信息
            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            XmlElement xeSysIdValue = xmldoc.CreateElement("data_sys_id_value");
            xeSysIdValue.InnerText = qcResult.patient.RepInputId;
            xe1.AppendChild(xeSysIdValue);

            XmlElement xeDataVer = xmldoc.CreateElement("data_ver");
            xeDataVer.InnerText = "1";
            xe1.AppendChild(xeDataVer);

            XmlElement xeDataTime = xmldoc.CreateElement("data_effectivetime");
            xeDataTime.InnerText = cache.ServerDateTime.GetDatabaseServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            xe1.AppendChild(xeDataTime);

            XmlElement xeDataAutor = xmldoc.CreateElement("data_author");
            xeDataAutor.InnerText = "Lis";
            xe1.AppendChild(xeDataAutor);

            XmlElement xeDataValidFlag = xmldoc.CreateElement("data_valid_flag");
            xeDataValidFlag.InnerText = "0";
            xe1.AppendChild(xeDataValidFlag);


            XmlElement xeEmpiId = xmldoc.CreateElement("pid_empi_id");
            xeEmpiId.InnerText = "";
            xe1.AppendChild(xeEmpiId);

            XmlElement xePidName = xmldoc.CreateElement("pid_name");
            xePidName.InnerText = qcResult.patient.PidName;
            xe1.AppendChild(xePidName);

            XmlElement xePidSex = xmldoc.CreateElement("pid_sex");
            xePidSex.InnerText = qcResult.patient.PidSexExp;
            xe1.AppendChild(xePidSex);

            XmlElement xePidAge = xmldoc.CreateElement("pid_age");
            string age = qcResult.patient.PidAgeExp.Split('Y')[0];
            string month = qcResult.patient.PidAgeExp.Split('M')[0].Split('Y')[1];
            if (age != "0")
            {
                xePidAge.InnerText = age + "岁";
            }
            else
            {
                xePidAge.InnerText = month + "月";
            }
            xe1.AppendChild(xePidAge);

            XmlElement xePidTel = xmldoc.CreateElement("pid_tel");
            xePidTel.InnerText = qcResult.patient.PidTel;
            xe1.AppendChild(xePidTel);

            XmlElement xePidSocialNo = xmldoc.CreateElement("pid_social_no");
            xePidSocialNo.InnerText = qcResult.patient.PidSocialNo;
            xe1.AppendChild(xePidSocialNo);

            XmlElement xePidInsuranceNo = xmldoc.CreateElement("pid_insurance_no");
            xePidInsuranceNo.InnerText = "";
            xe1.AppendChild(xePidInsuranceNo);

            XmlElement xePidAddresss = xmldoc.CreateElement("pid_address");
            xePidAddresss.InnerText = qcResult.patient.PidAddress;
            xe1.AppendChild(xePidAddresss);


            XmlElement xePidGuardian = xmldoc.CreateElement("pid_guardian");
            xePidGuardian.InnerText = "";
            xe1.AppendChild(xePidGuardian);

            XmlElement xePidHospital = xmldoc.CreateElement("pid_hospitalno");
            xePidHospital.InnerText = qcResult.patient.PidInNo;
            xe1.AppendChild(xePidHospital);

            XmlElement xePidInpationNo = xmldoc.CreateElement("pid_inpatient_no");
            if (qcResult.patient.PidSrcId == "108")
            {
                xePidInpationNo.InnerText = qcResult.patient.PidInNo;
            }
            else
            {
                xePidInpationNo.InnerText = "";
            }
            xe1.AppendChild(xePidInpationNo);

            XmlElement xePidAdmissTimes = xmldoc.CreateElement("pid_admiss_times");
            xePidAdmissTimes.InnerText = qcResult.patient.PidAddmissTimes.ToString();
            xe1.AppendChild(xePidAdmissTimes);

            XmlElement xePidEx = xmldoc.CreateElement("pid_ex_no");
            xePidEx.InnerText = qcResult.patient.PidExamNo.ToString();
            xe1.AppendChild(xePidEx);

            XmlElement xePidPacsId = xmldoc.CreateElement("pid_pacs_id");
            xePidPacsId.InnerText = "";
            xe1.AppendChild(xePidEx);

            XmlElement xePv1Pattype = xmldoc.CreateElement("pv1_pattype");
            if (qcResult.patient.PidSrcId == "108")
            {
                xePv1Pattype.InnerText = "I";
            }
            else if (qcResult.patient.PidSrcId == "107")
            {
                xePv1Pattype.InnerText = "O";
            }
            else if (qcResult.patient.PidSrcId == "109")
            {
                xePv1Pattype.InnerText = "T";
            }
            else
            {
                xePv1Pattype.InnerText = "U";

            }
            xe1.AppendChild(xePv1Pattype);

            XmlElement xePv1Regid = xmldoc.CreateElement("pv1_regid");
            xePv1Regid.InnerText = qcResult.patient.PidAddmissTimes.ToString();
            xe1.AppendChild(xePv1Regid);

            XmlElement xePv1DeptCode = xmldoc.CreateElement("pv1_dept_code");
            xePv1DeptCode.InnerText = qcResult.patient.PidDeptId;
            xe1.AppendChild(xePv1DeptCode);


            XmlElement xePv1DeptName = xmldoc.CreateElement("pv1_dept_name");
            xePv1DeptName.InnerText = qcResult.patient.DeptName;
            xe1.AppendChild(xePv1DeptName);

            XmlElement xePv1BedNo = xmldoc.CreateElement("pv1_bedno");
            xePv1BedNo.InnerText = qcResult.patient.PidBedNo;
            xe1.AppendChild(xePv1BedNo);

            XmlElement xeDg1Diag = xmldoc.CreateElement("dg1_diag");
            xeDg1Diag.InnerText = qcResult.patient.PidDiag;
            xe1.AppendChild(xeDg1Diag);

            XmlElement xeDg1DiagTime = xmldoc.CreateElement("dg1_diag_time");
            xeDg1DiagTime.InnerText = "";
            xe1.AppendChild(xeDg1DiagTime);

            XmlElement xeDg1DiagType = xmldoc.CreateElement("dg1_diag_type");
            xeDg1DiagType.InnerText = "";
            xe1.AppendChild(xeDg1DiagType);

            XmlElement xeDg1DiagDoctor = xmldoc.CreateElement("dg1_diag_doctor");
            xeDg1DiagDoctor.InnerText = "";
            xe1.AppendChild(xeDg1DiagDoctor);

            XmlElement xeOrcReqno = xmldoc.CreateElement("orc_reqno");
            xeOrcReqno.InnerText = qcResult.patient.RepInputId;
            xe1.AppendChild(xeOrcReqno);

            XmlElement xeOrcAppDeptCode = xmldoc.CreateElement("orc_appdept_code");
            xeOrcAppDeptCode.InnerText = qcResult.patient.PidDeptId;
            xe1.AppendChild(xeOrcAppDeptCode);

            XmlElement xeOrcAppDepName = xmldoc.CreateElement("orc_appdept_name");
            xeOrcAppDepName.InnerText = qcResult.patient.DeptName;
            xe1.AppendChild(xeOrcAppDepName);

            XmlElement xeOrcAppdrCode = xmldoc.CreateElement("orc_appdr_code");
            xeOrcAppdrCode.InnerText = qcResult.patient.PidDoctorCode;
            xe1.AppendChild(xeOrcAppdrCode);

            XmlElement xeOrcAppdrName = xmldoc.CreateElement("orc_appdr_name");
            xeOrcAppdrName.InnerText = qcResult.patient.DoctorName;
            xe1.AppendChild(xeOrcAppdrName);

            XmlElement xeOrcAppdrTime = xmldoc.CreateElement("orc_app_time");
            xeOrcAppdrTime.InnerText = qcResult.patient.SampApplyDate!=null?qcResult.patient.SampApplyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeOrcAppdrTime);

            XmlElement xeOrcBarcode = xmldoc.CreateElement("orc_bar_code");
            xeOrcBarcode.InnerText = qcResult.patient.RepBarCode;
            xe1.AppendChild(xeOrcBarcode);

            XmlElement xeOrcSampleName = xmldoc.CreateElement("orc_sample_name");
            xeOrcSampleName.InnerText = qcResult.patient.SamName;
            xe1.AppendChild(xeOrcSampleName);


            XmlElement xeOrcSampleNo = xmldoc.CreateElement("orc_sample_no");
            xeOrcSampleNo.InnerText = qcResult.patient.RepSid;
            xe1.AppendChild(xeOrcSampleNo);

            XmlElement xeObrRepNo = xmldoc.CreateElement("obr_repno");
            xeObrRepNo.InnerText = qcResult.patient.RepId;
            xe1.AppendChild(xeObrRepNo);

            XmlElement xeObrReType = xmldoc.CreateElement("obr_reptype");
            xeObrReType.InnerText = "0";
            xe1.AppendChild(xeObrReType);

            XmlElement xeObrRepName = xmldoc.CreateElement("obr_rep_name");
            xeObrRepName.InnerText = qcResult.patient.PidComName;
            xe1.AppendChild(xeObrRepName);

            XmlElement xeObrRepDesc = xmldoc.CreateElement("obr_rep_desc");
            xeObrRepDesc.InnerText = "";
            xe1.AppendChild(xeObrRepDesc);


            EntityResponse responseItr = new CacheDataBIZ().GetCacheData("EntityDicInstrument");
            List<EntityDicInstrument> listItrCache = responseItr.GetResult() as List<EntityDicInstrument>;
            List<EntityDicInstrument> listitr = listItrCache.FindAll(w => w.ItrId == qcResult.patient.RepItrId);
            XmlElement xeObrCtype = xmldoc.CreateElement("obr_ctype");
            if (listitr.Count > 0)
            {
                xeObrCtype.InnerText = listitr[0].ItrTypeName;
            }
            else
            {
                xeObrCtype.InnerText = "";
            }
            xe1.AppendChild(xeObrCtype);

            XmlElement xeObrInstrumentid = xmldoc.CreateElement("obr_instrument_id");
            xeObrInstrumentid.InnerText = qcResult.patient.RepItrId;
            xe1.AppendChild(xeObrInstrumentid);

            XmlElement xeObrInstrumentName = xmldoc.CreateElement("obr_instrumant_name");
            if (listitr.Count > 0)
            {
                xeObrInstrumentName.InnerText = listitr[0].ItrName;
            }
            xe1.AppendChild(xeObrInstrumentName);

            XmlElement xeObrExedoctor = xmldoc.CreateElement("obr_exedoctor_code");
            xeObrExedoctor.InnerText = listExt[0].ObrAuditUserId;
            xe1.AppendChild(xeObrExedoctor);

            List<EntitySysUser> listExeDoctor = DictSysUserCache.Current.Dclcache.FindAll(w => w.UserLoginid == listExt[0].ObrAuditUserId);
            string ExeName = string.Empty;
            if (listExeDoctor.Count > 0)
                ExeName = listExeDoctor[0].UserName;
            XmlElement xeObrExedoctorName = xmldoc.CreateElement("obr_exedoctor_name");
            xeObrExedoctorName.InnerText = ExeName;
            xe1.AppendChild(xeObrExedoctorName);

            XmlElement xeObrExeTime = xmldoc.CreateElement("obr_exe_time");
            xeObrExeTime.InnerText = listExt[0].ObrCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            xe1.AppendChild(xeObrExeTime);


            XmlElement xeObrItem = xmldoc.CreateElement("obr_item");
            xeObrItem.InnerText = "";
            xe1.AppendChild(xeObrItem);

            XmlElement xeObrItemName = xmldoc.CreateElement("obr_item_name");
            xeObrItemName.InnerText = "";
            xe1.AppendChild(xeObrItemName);

            XmlElement xeObrResult = xmldoc.CreateElement("obr_result");
            xeObrResult.InnerText = listExt[0].ObrValueC;
            xe1.AppendChild(xeObrResult);

            XmlElement xeObrReference = xmldoc.CreateElement("obr_reference");
            xeObrReference.InnerText = "";
            xe1.AppendChild(xeObrReference);

            XmlElement xeObrDesc = xmldoc.CreateElement("obr_desc");
            xeObrDesc.InnerText = "";
            xe1.AppendChild(xeObrDesc);

            XmlElement xeObrFirstAuditTime = xmldoc.CreateElement("obr_firstaudit_time");
            xeObrFirstAuditTime.InnerText = qcResult.patient.RepAuditDate!=null?qcResult.patient.RepAuditDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrFirstAuditTime);

            XmlElement xeObrFirstAuditDrCode = xmldoc.CreateElement("obr_firstaudit_drcode");
            xeObrFirstAuditDrCode.InnerText = qcResult.patient.RepAuditUserId;
            xe1.AppendChild(xeObrFirstAuditDrCode);

            List<EntitySysUser> listAudit = DictSysUserCache.Current.Dclcache.FindAll(w => w.UserLoginid == qcResult.patient.RepAuditUserId);
            string firstAuditName = string.Empty;
            if (listAudit.Count > 0)
                firstAuditName = listAudit[0].UserName;
            XmlElement xeObrFirstAuditDrName = xmldoc.CreateElement("obr_firstaudit_drname");
            xeObrFirstAuditDrName.InnerText = firstAuditName;
            xe1.AppendChild(xeObrFirstAuditDrName);

            XmlElement xeObrSecondAuditTime = xmldoc.CreateElement("obr_secondaudit_time");
            xeObrSecondAuditTime.InnerText = qcResult.patient.RepReportDate!=null? qcResult.patient.RepReportDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrSecondAuditTime);

            XmlElement xeObrSecondAuditDrCode = xmldoc.CreateElement("obr_secondaudit_drcode");
            xeObrSecondAuditDrCode.InnerText = qcResult.patient.RepReportUserId;
            xe1.AppendChild(xeObrSecondAuditDrCode);

            List<EntitySysUser> listReport = DictSysUserCache.Current.Dclcache.FindAll(w => w.UserLoginid == qcResult.patient.RepReportUserId);
            string secondAuditName = string.Empty;
            if (listReport.Count > 0)
                secondAuditName = listAudit[0].UserName;
            XmlElement xeObrSecondAuditDrName = xmldoc.CreateElement("obr_secondaudit_drname");
            xeObrSecondAuditDrName.InnerText = secondAuditName;
            xe1.AppendChild(xeObrSecondAuditDrName);

            XmlElement xeObrReadFlag = xmldoc.CreateElement("obr_read_flag");
            xeObrReadFlag.InnerText = "0";
            xe1.AppendChild(xeObrReadFlag);

            XmlElement xeObrPrintFlag = xmldoc.CreateElement("obr_print_flag");
            xeObrPrintFlag.InnerText = qcResult.patient.RepStatus.ToString();
            xe1.AppendChild(xeObrPrintFlag);

            XmlElement xeObrPrintTime = xmldoc.CreateElement("obr_print_time");
            xeObrPrintTime.InnerText = qcResult.patient.RepPrintDate!=null? qcResult.patient.RepPrintDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrPrintTime);
            #endregion
            Lib.LogManager.Logger.LogInfo("GetDangerousValuesXML：" + XMLDocumentToString(xmldoc));
            return XMLDocumentToString(xmldoc);
        }
        /// <summary>
        /// 上传结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        public string UploadResult(EntityQcResultList qcResult)
        {
            string str = string.Empty;
            string rvStr = string.Empty;
            try
            {
                List<EntityObrResultImage> listObrResImg = new List<EntityObrResultImage>();
                IDaoObrResultImage dao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                if (dao != null)
                {
                    listObrResImg = dao.GetObrResultImage(qcResult.patient.RepId);
                }
                str = GetPatientsXML(qcResult, "LIS_UploadResult", listObrResImg);
                rvStr = CreatePostHttpResponse(str);
                Lib.LogManager.Logger.LogInfo("上传结果发送Xml：" + str);
                Lib.LogManager.Logger.LogInfo("上传结果返回Xml：" + rvStr);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return rvStr;
        }
        /// <summary>
        /// 上传PDF结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        public string UploadPDFResult(EntityQcResultList qcResult)
        {
            string str = string.Empty;
            string rvStr = string.Empty;
            try
            {
                DCLReportPrintBIZ printBiz = new DCLReportPrintBIZ();
                EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                printParameter.RepId = qcResult.patient.RepId;
                List<EntityDicInstrument> listInstr = DictInstrmtCache.Current.DclCache.FindAll(w => w.ItrId == qcResult.patient.RepItrId);
                if (listInstr.Count > 0)
                    printParameter.ReportCode = listInstr[0].ItrReportId;
                string PDF = printBiz.GetReportPDF(printParameter);
                str = GetPatientsPDFXML(qcResult, "LIS_UploadPdfResult", PDF);
                rvStr = CreatePostHttpResponse(str);
                Lib.LogManager.Logger.LogInfo("上传PDF结果发送Xml：" + str);
                Lib.LogManager.Logger.LogInfo("上传PDF结果返回Xml：" + rvStr);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return rvStr;
        }

        /// <summary>
        /// 上传危急值结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        public string UploadDangerousValues(EntityQcResultList qcResult)
        {
            string str = string.Empty;
            string rvStr = string.Empty;
            try
            {
                List<EntityDicObrMessageContent> listMsgContent = new List<EntityDicObrMessageContent>();
                EntityDicObrMessageContent content = new EntityDicObrMessageContent();
                content.ObrValueA = qcResult.patient.RepId;
                IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
                if (dao != null)
                {
                    listMsgContent = dao.GetMessageByCondition(content) ;
                }
                if (listMsgContent.Count > 0)
                {
                    str = GetDangerousValuesXML(qcResult, "LIS_DangerousValues", listMsgContent);
                    rvStr = CreatePostHttpResponse(str);
                    Lib.LogManager.Logger.LogInfo("上传危急值结果发送Xml：" + str);
                    Lib.LogManager.Logger.LogInfo("上传危急值结果返回Xml：" + rvStr);
                } 
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return rvStr;
        }


        /// <summary>
        /// <summary>
        /// 检验危急值发送短信
        /// </summary>
        /// <param name="callee">手机号码</param>
        /// <param name="dept_name">科室名称</param>
        /// <param name="doctor_name">医生姓名</param>
        /// <param name="patient_name">患者姓名</param>
        /// <param name="patient_id">门诊ID或住院号</param>
        /// <param name="bed_no">床号</param>
        /// <param name="item_name">项目名称</param>
        /// <param name="item_result">项目结果</param>
        /// <param name="type">结果类型</param>
        /// <param name="paramsXml">参数</param>
        /// <returns></returns>
        public string LISSmsSendLis(string callee, string dept_name, string doctor_name, string patient_name, string patient_id, string bed_no, string item_name, string item_result, string type, string paramsXml)
        {
            string result =LISSmsSendLisXml("SmsSendLis", callee, dept_name, doctor_name, patient_name, patient_id, bed_no, item_name, item_result, type, paramsXml);
            string rvStr = CreatePostHttpResponse(result);
            Lib.LogManager.Logger.LogInfo("检验危急值发送短信发送XML:" + result);
            Lib.LogManager.Logger.LogInfo("检验危急值发送短信返回XML:" + result);
            return rvStr;
        }

        /// <summary>
        /// 获取医生手机号码
        /// </summary>
        /// <param name="doctor_code">工号</param>
        /// <returns></returns>
        public string LISGetDoctorPhone(string doctor_code)
        {
            string result = LISGetDoctorPhoneXml("GetDoctorPhone", doctor_code);
            string rvStr = CreatePostHttpResponse(result);
            Lib.LogManager.Logger.LogInfo("获取医生手机号码发送XML:" + result);
            Lib.LogManager.Logger.LogInfo("获取医生手机号码返回XML:" + result);
            return rvStr;
        }


        private string GetPatientsXML(EntityQcResultList qcResult, string Method, List<EntityObrResultImage> listImage)
        {
            string strResult = string.Empty;
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = Method;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");//创建一个节点 
            xmlelem.AppendChild(xe1);
            CreatePatientXml(qcResult, xmldoc, xe1);

            XmlElement xePdfBlob = xmldoc.CreateElement("pdf_blob");
            xePdfBlob.InnerText = "";
            xe1.AppendChild(xePdfBlob);
            //EntitySampMain sampMain = new EntitySampMain();
            //EntitySampQC sampQC = new EntitySampQC();
            //sampQC.ListSampBarId.Add(qcResult.patient.RepBarCode);
            //sampQC.SearchDeleteSampMain = false;
            //IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            //if (dao != null)
            //{
            //    List<EntitySampMain> listSampMain = dao.GetSampMain(sampQC);

            //    if (listSampMain.Count > 0)
            //    {
            //        sampMain = listSampMain[0];
            //    }
            //}
            XmlElement xeRes = xmldoc.CreateElement("lis_result");//创建结果节点 
            xe1.AppendChild(xeRes);

            XmlElement xeResDE = xmldoc.CreateElement("DocumentElement");//创建结果节点 
            xeRes.AppendChild(xeResDE);
            if (qcResult.listResulto.Count > 0)
            {
                #region 普通
                int orderSeq = 0;
                foreach (EntityObrResult result in qcResult.listResulto)
                {
                    orderSeq ++;
                    XmlElement xeResDT = xmldoc.CreateElement("DataTable");//创建一个节点 
                    xeResDE.AppendChild(xeResDT);

                    XmlElement xeResObrRepNo = xmldoc.CreateElement("obr_repno");
                    xeResObrRepNo.InnerText = result.ObrId;
                    xeResDT.AppendChild(xeResObrRepNo);

                    XmlElement xeResObrReptype= xmldoc.CreateElement("obr_reptype");
                    xeResObrReptype.InnerText = "";
                    xeResDT.AppendChild(xeResObrReptype);
                    
                    //List<EntityDicCombineDetail> listCache = DictCombineMiCache2.Current.DclCache.FindAll(w => w.ComId == result.ItmComId
                    //                                                                                                                                               && w.ComItmId == result.ItmId);
                    XmlElement xeObrSeq = xmldoc.CreateElement("obr_seq");
                    //if (listCache.Count > 0)
                    //{
                    //    xeObrSeq.InnerText = listCache[0].ComSortNo.ToString();
                    //}
                    xeObrSeq.InnerText = orderSeq.ToString();
                    xeResDT.AppendChild(xeObrSeq);

                    XmlElement xeResObrComCode = xmldoc.CreateElement("obr_com_code");
                    xeResObrComCode.InnerText ="";
                    xeResDT.AppendChild(xeResObrComCode);

                    XmlElement xeResObrComName= xmldoc.CreateElement("obr_com_name");
                    xeResObrComName.InnerText = "";
                    xeResDT.AppendChild(xeResObrComName);

                    XmlElement xeResObrItemId = xmldoc.CreateElement("obr_item_id");
                    xeResObrItemId.InnerText = result.ItmId;
                    xeResDT.AppendChild(xeResObrItemId);

                    XmlElement xeResObrItemCname = xmldoc.CreateElement("obr_item_cname");
                    xeResObrItemCname.InnerText = result.ItmName;
                    xeResDT.AppendChild(xeResObrItemCname);

                    XmlElement xeResObrItemEname = xmldoc.CreateElement("obr_item_ename");
                    xeResObrItemEname.InnerText = result.ItmEname;
                    xeResDT.AppendChild(xeResObrItemEname);

                    XmlElement xeResObrItemAlias = xmldoc.CreateElement("obr_item_alias");
                    xeResObrItemAlias.InnerText = result.ItmEname;
                    xeResDT.AppendChild(xeResObrItemAlias);

                    XmlElement xeResObrAntiId = xmldoc.CreateElement("obr_ant_id");
                    xeResObrAntiId.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntiId);

                    XmlElement xeResObrAntCname = xmldoc.CreateElement("obr_ant_cname");
                    xeResObrAntCname.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntCname);

                    XmlElement xeResObrAntEname = xmldoc.CreateElement("obr_ant_ename");
                    xeResObrAntEname.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntEname);

                    XmlElement xeResObrResult = xmldoc.CreateElement("obr_result");
                    xeResObrResult.InnerText = ConvertXml(result.ObrValue.Replace(">", "大于").Replace("<", "小于").Replace("=", "等于").Replace("+", @"%2B"));
                    xeResDT.AppendChild(xeResObrResult);

                    XmlElement xeResObrDesc = xmldoc.CreateElement("obr_desc");
                    xeResObrDesc.InnerText = "";
                    xeResDT.AppendChild(xeResObrDesc);

                    XmlElement xeResObrOdResult = xmldoc.CreateElement("obr_od_result");
                    xeResObrOdResult.InnerText = result.ObrValue2;
                    xeResDT.AppendChild(xeResObrOdResult);

                    XmlElement xeResObrColonyCnt = xmldoc.CreateElement("obr_colony_cnt");
                    xeResObrColonyCnt.InnerText = "";
                    xeResDT.AppendChild(xeResObrColonyCnt);

                    XmlElement xeResObrRefRange = xmldoc.CreateElement("obr_ref_range");
                    if (string.IsNullOrEmpty(result.RefLowerLimit) && !string.IsNullOrEmpty(result.RefUpperLimit))
                    {
                        xeResObrRefRange.InnerText = result.RefUpperLimit;
                    }
                    else if (!string.IsNullOrEmpty(result.RefLowerLimit) && string.IsNullOrEmpty(result.RefUpperLimit))
                    {
                        xeResObrRefRange.InnerText = result.RefLowerLimit;
                    }
                    else
                    {
                        xeResObrRefRange.InnerText = result.RefLowerLimit + "-" + result.RefUpperLimit;
                    }
                    xeResDT.AppendChild(xeResObrRefRange);

                    XmlElement xeResObrUnit = xmldoc.CreateElement("obr_unit");
                    xeResObrUnit.InnerText = result.ObrUnit;
                    xeResDT.AppendChild(xeResObrUnit);

                    XmlElement xeResObrPrompt = xmldoc.CreateElement("obr_prompt");
                    xeResObrPrompt.InnerText = result.ResPrompt;
                    xeResDT.AppendChild(xeResObrPrompt);

                    XmlElement xeResObrUrgentFlag = xmldoc.CreateElement("obr_urgent_flag");
                    xeResObrUrgentFlag.InnerText = "";
                    xeResDT.AppendChild(xeResObrUrgentFlag);

                    XmlElement xeResObrPosFlag = xmldoc.CreateElement("obr_pos_flag");
                    xeResObrPosFlag.InnerText = result.RefFlag;
                    xeResDT.AppendChild(xeResObrPosFlag);

                    XmlElement xeResObrTime = xmldoc.CreateElement("obr_time");
                    xeResObrTime.InnerText = result.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"); ;
                    xeResDT.AppendChild(xeResObrTime);

                    XmlElement xeResObrRefMax = xmldoc.CreateElement("obr_ref_max");
                    xeResObrRefMax.InnerText = result.RefUpperLimit;
                    xeResDT.AppendChild(xeResObrRefMax);

                    XmlElement xeResObrRefMin = xmldoc.CreateElement("obr_ref_min");
                    xeResObrRefMin.InnerText = result.RefLowerLimit;
                    xeResDT.AppendChild(xeResObrRefMin);

                    XmlElement xeResObrNote = xmldoc.CreateElement("obr_note");
                    xeResObrNote.InnerText = result.RefLowerLimit;
                    xeResDT.AppendChild(xeResObrNote);

                    XmlElement xeResObrMethod = xmldoc.CreateElement("obr_method");
                    xeResObrMethod.InnerText = result.ObrItmMethod;
                    xeResDT.AppendChild(xeResObrMethod);
                }
                #endregion
            }
            else if (qcResult.listDesc != null && qcResult.listDesc.Count > 0)
            {
                #region 描述报告
                int descSeq = 0;
                foreach (EntityObrResultDesc desc in qcResult.listDesc)
                {
                    descSeq++;
                    XmlElement xeResDT = xmldoc.CreateElement("DataTable");//创建一个节点 
                    xeResDE.AppendChild(xeResDT);

                    XmlElement xeResObrRepNo = xmldoc.CreateElement("obr_repno");
                    xeResObrRepNo.InnerText = desc.ObrId;
                    xeResDT.AppendChild(xeResObrRepNo);

                    XmlElement xeResObrReptype = xmldoc.CreateElement("obr_reptype");
                    xeResObrReptype.InnerText = "";
                    xeResDT.AppendChild(xeResObrReptype);

                    XmlElement xeResObrSeq = xmldoc.CreateElement("obr_seq");
                    xeResObrSeq.InnerText = descSeq.ToString();
                    xeResDT.AppendChild(xeResObrSeq);

                    XmlElement xeResObrComCode = xmldoc.CreateElement("obr_com_code");
                    xeResObrComCode.InnerText = "";
                    xeResDT.AppendChild(xeResObrComCode);

                    XmlElement xeResObrComName = xmldoc.CreateElement("obr_com_name");
                    xeResObrComName.InnerText = "";
                    xeResDT.AppendChild(xeResObrComName);

                    XmlElement xeResObrItemId = xmldoc.CreateElement("obr_item_id");
                    xeResObrItemId.InnerText = "";
                    xeResDT.AppendChild(xeResObrItemId);

                    XmlElement xeResObrItemCname = xmldoc.CreateElement("obr_item_cname");
                    xeResObrItemCname.InnerText = "";
                    xeResDT.AppendChild(xeResObrItemCname);

                    XmlElement xeResObrItemEname = xmldoc.CreateElement("obr_item_ename");
                    xeResObrItemEname.InnerText = "";
                    xeResDT.AppendChild(xeResObrItemEname);

                    XmlElement xeResObrItemAlias = xmldoc.CreateElement("obr_item_alias");
                    xeResObrItemAlias.InnerText = "";
                    xeResDT.AppendChild(xeResObrItemAlias);

                    XmlElement xeResObrAntiId = xmldoc.CreateElement("obr_ant_id");
                    xeResObrAntiId.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntiId);

                    XmlElement xeResObrAntCname = xmldoc.CreateElement("obr_ant_cname");
                    xeResObrAntCname.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntCname);

                    XmlElement xeResObrAntEname = xmldoc.CreateElement("obr_ant_ename");
                    xeResObrAntEname.InnerText = "";
                    xeResDT.AppendChild(xeResObrAntEname);

                    XmlElement xeResObrResult = xmldoc.CreateElement("obr_result");
                    if (desc.ObrFlag.ToString() == "0")
                    {
                        xeResObrResult.InnerText = desc.ObrValue;
                    }
                    else
                    {
                        xeResObrResult.InnerText = desc.ObrDescribe;
                    }
                    xeResDT.AppendChild(xeResObrResult);

                    XmlElement xeResObrDesc = xmldoc.CreateElement("obr_desc");
                    xeResObrDesc.InnerText = "";
                    xeResDT.AppendChild(xeResObrDesc);

                    XmlElement xeResObrOdResult = xmldoc.CreateElement("obr_od_result");
                    xeResObrOdResult.InnerText = "";
                    xeResDT.AppendChild(xeResObrOdResult);


                    XmlElement xeResObrColonyCnt = xmldoc.CreateElement("obr_colony_cnt");
                    xeResObrColonyCnt.InnerText = "";
                    xeResDT.AppendChild(xeResObrColonyCnt);

                    XmlElement xeResObrRefRange = xmldoc.CreateElement("obr_ref_range");
                    xeResObrRefRange.InnerText = "";
                    xeResDT.AppendChild(xeResObrRefRange);

                    XmlElement xeResObrUnit = xmldoc.CreateElement("obr_unit");
                    xeResObrUnit.InnerText = "";
                    xeResDT.AppendChild(xeResObrUnit);

                    XmlElement xeResObrPrompt = xmldoc.CreateElement("obr_prompt");
                    xeResObrPrompt.InnerText = "";
                    xeResDT.AppendChild(xeResObrUnit);

                    XmlElement xeResObrUrgentFlag = xmldoc.CreateElement("obr_urgent_flag");
                    xeResObrUrgentFlag.InnerText = "";
                    xeResDT.AppendChild(xeResObrUrgentFlag);

                    XmlElement xeResObrPosFlag = xmldoc.CreateElement("obr_pos_flag");
                    xeResObrPosFlag.InnerText = "";
                    xeResDT.AppendChild(xeResObrPosFlag);

                    XmlElement xeResObrTime = xmldoc.CreateElement("obr_time");
                    xeResObrTime.InnerText = "";
                    xeResDT.AppendChild(xeResObrTime);

                    XmlElement xeResObrRefMax = xmldoc.CreateElement("obr_ref_max");
                    xeResObrRefMax.InnerText = "";
                    xeResDT.AppendChild(xeResObrRefMax);

                    XmlElement xeResObrRefMin = xmldoc.CreateElement("obr_ref_min");
                    xeResObrRefMin.InnerText = "";
                    xeResDT.AppendChild(xeResObrRefMin);

                    XmlElement xeResObrNote = xmldoc.CreateElement("obr_note");
                    xeResObrNote.InnerText = "";
                    xeResDT.AppendChild(xeResObrNote);

                    XmlElement xeResObrMethod = xmldoc.CreateElement("obr_method");
                    xeResObrMethod.InnerText = "";
                    xeResDT.AppendChild(xeResObrMethod);
                }
                #endregion

            }
            else if (qcResult.listBact != null && qcResult.listBact.Count > 0)
            {
                int antiSeq = 0;
                #region 细菌报告
                foreach (EntityObrResultBact bact in qcResult.listBact)
                {
                    foreach (EntityObrResultAnti anti in qcResult.listAnti)
                    {
                        antiSeq++;
                        XmlElement xeResDT = xmldoc.CreateElement("DataTable");//创建一个节点 
                        xeResDE.AppendChild(xeResDT);

                        XmlElement xeResObrRepNo = xmldoc.CreateElement("obr_repno");
                        xeResObrRepNo.InnerText = bact.ObrId;
                        xeResDT.AppendChild(xeResObrRepNo);

                        XmlElement xeResObrReptype = xmldoc.CreateElement("obr_reptype");
                        xeResObrReptype.InnerText = "";
                        xeResDT.AppendChild(xeResObrReptype);

                        XmlElement xeResObrSeq = xmldoc.CreateElement("obr_seq");
                        xeResObrSeq.InnerText = antiSeq.ToString();
                        xeResDT.AppendChild(xeResObrSeq);

                        XmlElement xeResObrComCode = xmldoc.CreateElement("obr_com_code");
                        xeResObrComCode.InnerText = "";
                        xeResDT.AppendChild(xeResObrComCode);

                        XmlElement xeResObrComName = xmldoc.CreateElement("obr_com_name");
                        xeResObrComName.InnerText = "";
                        xeResDT.AppendChild(xeResObrComName);

                        XmlElement xeResObrItemId = xmldoc.CreateElement("obr_item_id");
                        xeResObrItemId.InnerText = bact.ObrBacId.ToString();
                        xeResDT.AppendChild(xeResObrItemId);

                        XmlElement xeResObrItemCname = xmldoc.CreateElement("obr_item_cname");
                        xeResObrItemCname.InnerText = bact.BacCname;
                        xeResDT.AppendChild(xeResObrItemCname);

                        XmlElement xeResObrItemEname = xmldoc.CreateElement("obr_item_ename");
                        xeResObrItemEname.InnerText = bact.BacEname;
                        xeResDT.AppendChild(xeResObrItemEname);

                        XmlElement xeResObrItemAlias = xmldoc.CreateElement("obr_item_alias");
                        xeResObrItemAlias.InnerText = "";
                        xeResDT.AppendChild(xeResObrItemAlias);

                        XmlElement xeResObrAntiId = xmldoc.CreateElement("obr_ant_id");
                        xeResObrAntiId.InnerText = anti.ObrAntId;
                        xeResDT.AppendChild(xeResObrAntiId);

                        XmlElement xeResObrAntCname = xmldoc.CreateElement("obr_ant_cname");
                        xeResObrAntCname.InnerText = anti.AntCname;
                        xeResDT.AppendChild(xeResObrAntCname);

                        XmlElement xeResObrAntEname = xmldoc.CreateElement("obr_ant_ename");
                        xeResObrAntEname.InnerText = anti.AntEname;
                        xeResDT.AppendChild(xeResObrAntEname);

                        XmlElement xeResObrResult = xmldoc.CreateElement("obr_result");
                        xeResObrResult.InnerText = anti.ObrValue;
                        xeResDT.AppendChild(xeResObrResult);

                        XmlElement xeResObrDesc = xmldoc.CreateElement("obr_desc");
                        xeResObrDesc.InnerText = "";
                        xeResDT.AppendChild(xeResObrDesc);

                        XmlElement xeResObrOdResult = xmldoc.CreateElement("obr_od_result");
                        xeResObrOdResult.InnerText = "";
                        xeResDT.AppendChild(xeResObrOdResult);

                        XmlElement xeResObrColonyCnt = xmldoc.CreateElement("obr_colony_cnt");
                        xeResObrColonyCnt.InnerText = "";
                        xeResDT.AppendChild(xeResObrColonyCnt);

                        XmlElement xeResObrRefRange = xmldoc.CreateElement("obr_ref_range");
                        xeResObrRefRange.InnerText = anti.ObrRef;
                        xeResDT.AppendChild(xeResObrRefRange);

                        XmlElement xeResObrUnit = xmldoc.CreateElement("obr_unit");
                        xeResObrUnit.InnerText = anti.ObrUnit;
                        xeResDT.AppendChild(xeResObrUnit);

                        XmlElement xeResObrPrompt = xmldoc.CreateElement("obr_prompt");
                        xeResObrPrompt.InnerText = "";
                        xeResDT.AppendChild(xeResObrPrompt);

                        XmlElement xeResObrUrgentFlag = xmldoc.CreateElement("obr_urgent_flag");
                        xeResObrUrgentFlag.InnerText = "";
                        xeResDT.AppendChild(xeResObrUrgentFlag);

                        XmlElement xeResObrPosFlag = xmldoc.CreateElement("obr_pos_flag");
                        xeResObrPosFlag.InnerText = "";
                        xeResDT.AppendChild(xeResObrPosFlag);

                        XmlElement xeResObrTime = xmldoc.CreateElement("obr_time");
                        xeResObrTime.InnerText = anti.ObrDate.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        xeResDT.AppendChild(xeResObrTime);

                        XmlElement xeResObrRefMax = xmldoc.CreateElement("obr_ref_max");
                        xeResObrRefMax.InnerText = "";
                        xeResDT.AppendChild(xeResObrRefMax);

                        XmlElement xeResObrRefMin = xmldoc.CreateElement("obr_ref_min");
                        xeResObrRefMin.InnerText = "";
                        xeResDT.AppendChild(xeResObrRefMin);

                        XmlElement xeResObrNote = xmldoc.CreateElement("obr_note");
                        xeResObrNote.InnerText = bact.ObrRemark;
                        xeResDT.AppendChild(xeResObrNote);

                        XmlElement xeResObrMethod = xmldoc.CreateElement("obr_method");
                        xeResObrMethod.InnerText = anti.ObrMethod;
                        xeResDT.AppendChild(xeResObrMethod);
                    }
                }
                #endregion

            }
            XmlElement xelisResultplus = xmldoc.CreateElement("lis_resultplus");//创建结果节点 
            xe1.AppendChild(xelisResultplus);

            XmlElement xeResDEPlus = xmldoc.CreateElement("DocumentElement");//创建结果节点 
            xelisResultplus.AppendChild(xeResDEPlus);
            #region 图片结果
            if (listImage.Count > 0)
            {
                int imageSeq = 0;
                foreach (EntityObrResultImage image in listImage)
                {
                    imageSeq++;
                    XmlElement xeResDT = xmldoc.CreateElement("DataTable");//创建一个节点 
                    xeResDE.AppendChild(xeResDT);
                    XmlElement xeResObrRepNo = xmldoc.CreateElement("obr_repno");
                    xeResObrRepNo.InnerText = image.ObrId;
                    xeResDT.AppendChild(xeResObrRepNo);

                    XmlElement xeResObrRepType = xmldoc.CreateElement("obr_reptype");
                    xeResObrRepType.InnerText = "";
                    xeResDT.AppendChild(xeResObrRepType);

                    XmlElement xeResObrSeq = xmldoc.CreateElement("obr_seq");
                    xeResObrSeq.InnerText = imageSeq.ToString();
                    xeResDT.AppendChild(xeResObrSeq);

                    XmlElement xeResObrResCode = xmldoc.CreateElement("obr_res_code");
                    xeResObrResCode.InnerText = image.ObrItmEname;
                    xeResDT.AppendChild(xeResObrResCode);

                    EntityResponse responseItme = new CacheDataBIZ().GetCacheData("EntityDicItmItem");
                    List<EntityDicItmItem> listItemCache = responseItme.GetResult() as List<EntityDicItmItem>;
                    List<EntityDicItmItem> listItem = listItemCache.FindAll(w => w.ItmEcode == image.ObrItmEname);
                    XmlElement xeResObrResName = xmldoc.CreateElement("obr_res_name");
                    if (listItem.Count > 0)
                    {
                        xeResObrResCode.InnerText = listItem[0].ItmName;
                    }
                    else
                    {
                        xeResObrResCode.InnerText = "";
                    }

                    XmlElement xeResObrText = xmldoc.CreateElement("obr_text");
                    xeResObrText.InnerText = "";
                    xeResDT.AppendChild(xeResObrText);

                    XmlElement xeResObrImage = xmldoc.CreateElement("obr_image");
                    xeResObrImage.InnerText = Convert.ToBase64String(image.ObrImage).Replace("+", @"%2B").Replace("/", @"%2F").Replace("=", @"%3D");
                    xeResDT.AppendChild(xeResObrImage);
                }
                #endregion
            }
            strResult = XMLDocumentToString(xmldoc);
            return strResult;
        }

        private string GetPatientsPDFXML(EntityQcResultList qcResult, string Method, string PDF)
        {
            string strResult = string.Empty;
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = Method;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");//创建一个节点 
            xmlelem.AppendChild(xe1);
            CreatePatientXml(qcResult, xmldoc, xe1);

            XmlElement xePdfBlob = xmldoc.CreateElement("pdf_blob");
            xePdfBlob.InnerText = PDF;
            xe1.AppendChild(xePdfBlob);

            strResult = XMLDocumentToString(xmldoc);
            return strResult;
        }
        private void CreatePatientXml(EntityQcResultList qcResult, XmlDocument xmldoc, XmlElement xe1)
        {
            #region 病人信息
            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            XmlElement xeEmpiId = xmldoc.CreateElement("data_empi_id");
            xeEmpiId.InnerText = "";
            xe1.AppendChild(xeEmpiId);

            XmlElement xeDataVer = xmldoc.CreateElement("data_ver");
            xeDataVer.InnerText = "1";
            xe1.AppendChild(xeDataVer);

            XmlElement xeDataTime = xmldoc.CreateElement("data_effectivetime");
            xeDataTime.InnerText = cache.ServerDateTime.GetDatabaseServerDateTime().ToString("yyyy-MM-dd HH:mm:ss"); ;
            xe1.AppendChild(xeDataTime);

            XmlElement xeSysIdValue = xmldoc.CreateElement("data_sys_id_value");
            xeSysIdValue.InnerText = qcResult.patient.RepInputId;
            xe1.AppendChild(xeSysIdValue);

            XmlElement xeDataAutor = xmldoc.CreateElement("data_author");
            xeDataAutor.InnerText = "Lis";
            xe1.AppendChild(xeDataAutor);

            XmlElement xeDataValidFlag = xmldoc.CreateElement("data_valid_flag");
            xeDataValidFlag.InnerText = "0";
            xe1.AppendChild(xeDataValidFlag);

            XmlElement xeObrRepNo = xmldoc.CreateElement("obr_repno");
            xeObrRepNo.InnerText = qcResult.patient.RepId;
            xe1.AppendChild(xeObrRepNo);

            XmlElement xeObrReType = xmldoc.CreateElement("obr_reptype");
            xeObrReType.InnerText = "0";
            xe1.AppendChild(xeObrReType);

            XmlElement xePv1Regid = xmldoc.CreateElement("pv1_regid");
            xePv1Regid.InnerText = qcResult.patient.PidAddmissTimes.ToString();
            xe1.AppendChild(xePv1Regid);

            XmlElement xePv1Pattype = xmldoc.CreateElement("pv1_pattype");
            if (qcResult.patient.PidSrcId == "108")
            {
                xePv1Pattype.InnerText = "I";
            }
            else if (qcResult.patient.PidSrcId == "107")
            {
                xePv1Pattype.InnerText = "O";
            }
            else if (qcResult.patient.PidSrcId == "109")
            {
                xePv1Pattype.InnerText = "T";
            }
            else
            {
                xePv1Pattype.InnerText = "U";

            }
            xe1.AppendChild(xePv1Pattype);

            XmlElement xePidHospital = xmldoc.CreateElement("pid_hospitalno");
            xePidHospital.InnerText = qcResult.patient.PidInNo;
            xe1.AppendChild(xePidHospital);

            XmlElement xePidName = xmldoc.CreateElement("pid_name");
            xePidName.InnerText = qcResult.patient.PidName;
            xe1.AppendChild(xePidName);

            XmlElement xePidSex = xmldoc.CreateElement("pid_sex");
            xePidSex.InnerText = qcResult.patient.PidSexExp;
            xe1.AppendChild(xePidSex);

            XmlElement xePidAge = xmldoc.CreateElement("pid_age");
            string age = qcResult.patient.PidAgeExp.Split('Y')[0];
            string month = qcResult.patient.PidAgeExp.Split('M')[0].Split('Y')[1];
            if (age != "0")
            {
                xePidAge.InnerText = age + "岁";
            }
            else
            {
                xePidAge.InnerText = month + "月";
            }
            xe1.AppendChild(xePidAge);

            XmlElement xePidAdmissTimes = xmldoc.CreateElement("pid_admiss_times");
            xePidAdmissTimes.InnerText = qcResult.patient.PidAddmissTimes.ToString();
            xe1.AppendChild(xePidAdmissTimes);

            XmlElement xePidInpationNo = xmldoc.CreateElement("pid_inpatient_no");
            if (qcResult.patient.PidSrcId == "108")
            {
                xePidInpationNo.InnerText = qcResult.patient.PidInNo;
            }
            else
            {
                xePidInpationNo.InnerText = "";
            }
            xe1.AppendChild(xePidInpationNo);

            XmlElement xePv1BedNo = xmldoc.CreateElement("pv1_bedno");
            xePv1BedNo.InnerText = qcResult.patient.PidBedNo;
            xe1.AppendChild(xePv1BedNo);

            XmlElement xePidInsuranceNo = xmldoc.CreateElement("pid_insurance_no");
            xePidInsuranceNo.InnerText = "";
            xe1.AppendChild(xePidInsuranceNo);

            XmlElement xePidSocialNo = xmldoc.CreateElement("pid_social_no");
            xePidSocialNo.InnerText = qcResult.patient.PidSocialNo;
            xe1.AppendChild(xePidSocialNo);

            XmlElement xePidEx = xmldoc.CreateElement("pid_ex_no");
            xePidEx.InnerText = qcResult.patient.PidExamNo.ToString();
            xe1.AppendChild(xePidEx);

            XmlElement xeDg1Diag = xmldoc.CreateElement("dg1_diag");
            xeDg1Diag.InnerText = qcResult.patient.PidDiag;
            xe1.AppendChild(xeDg1Diag);

            XmlElement xePidAddresss = xmldoc.CreateElement("pid_address");
            xePidAddresss.InnerText = qcResult.patient.PidAddress;
            xe1.AppendChild(xePidAddresss);

            XmlElement xePidTel = xmldoc.CreateElement("pid_tel");
            xePidTel.InnerText = qcResult.patient.PidTel;
            xe1.AppendChild(xePidTel);

            XmlElement xePdfRepName= xmldoc.CreateElement("pdf_rep_name");
            xePdfRepName.InnerText = qcResult.patient.PidComName;
            xe1.AppendChild(xePdfRepName);

            XmlElement xePdfRepDesc = xmldoc.CreateElement("pdf_rep_desc");
            xePdfRepDesc.InnerText = qcResult.patient.RepRemark;
            xe1.AppendChild(xePdfRepDesc);

            XmlElement xeOrcAppDeptCode = xmldoc.CreateElement("orc_appdept_code");
            xeOrcAppDeptCode.InnerText = qcResult.patient.PidDeptId;
            xe1.AppendChild(xeOrcAppDeptCode);

            XmlElement xeOrcAppDepName = xmldoc.CreateElement("orc_appdept_name");
            xeOrcAppDepName.InnerText = qcResult.patient.PidDeptName;
            xe1.AppendChild(xeOrcAppDepName);

            XmlElement xeOrcAppdrCode = xmldoc.CreateElement("orc_appdr_code");
            xeOrcAppdrCode.InnerText = qcResult.patient.PidDoctorCode;
            xe1.AppendChild(xeOrcAppdrCode);

            XmlElement xeOrcAppdrName = xmldoc.CreateElement("orc_appdr_name");
            xeOrcAppdrName.InnerText = qcResult.patient.DoctorName;
            xe1.AppendChild(xeOrcAppdrName);

            XmlElement xeOrcAppdrTime = xmldoc.CreateElement("orc_app_time");
            xeOrcAppdrTime.InnerText = qcResult.patient.SampApplyDate!=null? qcResult.patient.SampApplyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):""; 
            xe1.AppendChild(xeOrcAppdrTime);

            EntityResponse responseItr = new CacheDataBIZ().GetCacheData("EntityDicInstrument");
            List<EntityDicInstrument> listItrCache = responseItr.GetResult() as List<EntityDicInstrument>;
            List<EntityDicInstrument> listitr = listItrCache.FindAll(w => w.ItrId == qcResult.patient.RepItrId);
            XmlElement xeObrCtype = xmldoc.CreateElement("obr_ctype");
            if (listitr.Count > 0)
            {
                xeObrCtype.InnerText = listitr[0].ItrTypeName;
            }
            else
            {
                xeObrCtype.InnerText = "";
            }
            xe1.AppendChild(xeObrCtype);

            XmlElement xeOrcSampleName = xmldoc.CreateElement("orc_sample_name");
            xeOrcSampleName.InnerText = qcResult.patient.SamName;
            xe1.AppendChild(xeOrcSampleName);

            XmlElement xeOrcSampleStatus = xmldoc.CreateElement("orc_sample_status");
            xeOrcSampleStatus.InnerText = qcResult.patient.BcStatus;
            xe1.AppendChild(xeOrcSampleStatus);

            XmlElement xeOrcGesweek = xmldoc.CreateElement("orc_gesweek");
            xeOrcGesweek.InnerText = qcResult.patient.PidPreWeek;
            xe1.AppendChild(xeOrcGesweek);

            XmlElement xeObrInstrumentid = xmldoc.CreateElement("obr_instrument_id");
            xeObrInstrumentid.InnerText = qcResult.patient.RepItrId;
            xe1.AppendChild(xeObrInstrumentid);


            XmlElement xeObrInstrumentName = xmldoc.CreateElement("obr_instrumant_name");
            if(listitr.Count > 0)
            {
                xeObrInstrumentName.InnerText = listitr[0].ItrName;
            }
            else
            {
                xeObrCtype.InnerText = "";
            }
            xe1.AppendChild(xeObrInstrumentName);

            XmlElement xeOrcBarcode = xmldoc.CreateElement("orc_bar_code");
            xeOrcBarcode.InnerText = qcResult.patient.RepBarCode;
            xe1.AppendChild(xeOrcBarcode);

            XmlElement xeOrcSampleNo = xmldoc.CreateElement("orc_sample_no");
            xeOrcSampleNo.InnerText = qcResult.patient.RepSid;
            xe1.AppendChild(xeOrcSampleNo);

            XmlElement xeOrcSampleTime = xmldoc.CreateElement("orc_sample_time");
            xeOrcSampleTime.InnerText = qcResult.patient.SampCollectionDate!=null? qcResult.patient.SampCollectionDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeOrcSampleTime);

            XmlElement xeOrcSampleOper = xmldoc.CreateElement("orc_sample_oper");
            xeOrcSampleOper.InnerText = "";
            xe1.AppendChild(xeOrcSampleOper);

            XmlElement xeOrcSampleOperName = xmldoc.CreateElement("orc_sample_opername");
            xeOrcSampleOperName.InnerText = "";
            xe1.AppendChild(xeOrcSampleOperName);

            XmlElement xeOrcSendTime = xmldoc.CreateElement("orc_send_time");
            xeOrcSendTime.InnerText = qcResult.patient.SampSendDate!=null? qcResult.patient.SampSendDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeOrcSendTime);

            XmlElement xeOrcSendOper = xmldoc.CreateElement("orc_send_oper");
            xeOrcSendOper.InnerText = "";
            xe1.AppendChild(xeOrcSendOper);

            XmlElement xeOrcSendOperName = xmldoc.CreateElement("orc_send_opername");
            xeOrcSendOperName.InnerText = "";
            xe1.AppendChild(xeOrcSendOperName);

            XmlElement xeOrcReceiveOper = xmldoc.CreateElement("orc_receive_oper");
            xeOrcReceiveOper.InnerText = "";
            xe1.AppendChild(xeOrcReceiveOper);

            XmlElement xeOrcReceiveOperName = xmldoc.CreateElement("orc_receive_opername");
            xeOrcReceiveOperName.InnerText = "";
            xe1.AppendChild(xeOrcReceiveOperName);

            XmlElement xeOrcReceiveTime = xmldoc.CreateElement("orc_receive_time");
            xeOrcReceiveTime.InnerText = qcResult.patient.SampReceiveDate!=null? qcResult.patient.SampReceiveDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            xe1.AppendChild(xeOrcReceiveTime);

            XmlElement xeObrFirstAuditTime = xmldoc.CreateElement("obr_firstaudit_time");
            xeObrFirstAuditTime.InnerText = qcResult.patient.RepAuditDate!=null? qcResult.patient.RepAuditDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrFirstAuditTime);

            XmlElement xeObrFirstAuditDrCode = xmldoc.CreateElement("obr_firstaudit_drcode");
            xeObrFirstAuditDrCode.InnerText = qcResult.patient.RepAuditUserId;
            xe1.AppendChild(xeObrFirstAuditDrCode);

            List<EntitySysUser> listAudit = DictSysUserCache.Current.Dclcache.FindAll(w => w.UserLoginid == qcResult.patient.RepAuditUserId);
            string firstAuditName = string.Empty;
            if (listAudit.Count > 0)
                firstAuditName = listAudit[0].UserName;
            XmlElement xeObrFirstAuditDrName = xmldoc.CreateElement("obr_firstaudit_drname");
            xeObrFirstAuditDrName.InnerText = firstAuditName;
            xe1.AppendChild(xeObrFirstAuditDrName);

            XmlElement xeObrSecondAuditTime = xmldoc.CreateElement("obr_secondaudit_time");
            xeObrSecondAuditTime.InnerText = qcResult.patient.RepReportDate!=null? qcResult.patient.RepReportDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrSecondAuditTime);

            XmlElement xeObrSecondAuditDrCode = xmldoc.CreateElement("obr_secondaudit_drcode");
            xeObrSecondAuditDrCode.InnerText = qcResult.patient.RepReportUserId;
            xe1.AppendChild(xeObrSecondAuditDrCode);

            List<EntitySysUser> listReport = DictSysUserCache.Current.Dclcache.FindAll(w => w.UserLoginid == qcResult.patient.RepReportUserId);
            string secondAuditName = string.Empty;
            if (listReport.Count > 0)
                secondAuditName = listAudit[0].UserName;
            XmlElement xeObrSecondAuditDrName = xmldoc.CreateElement("obr_secondaudit_drname");
            xeObrSecondAuditDrName.InnerText = secondAuditName;
            xe1.AppendChild(xeObrSecondAuditDrName);


            XmlElement xeObrDangerFlag = xmldoc.CreateElement("obr_danger_flag");
            xeObrDangerFlag.InnerText = qcResult.patient.RepUrgentFlag.ToString();
            xe1.AppendChild(xeObrDangerFlag);

            XmlElement xeObrUrgentFlag = xmldoc.CreateElement("obr_urgent_falg");
            xeObrUrgentFlag.InnerText = qcResult.patient.RepCtype;
            xe1.AppendChild(xeObrUrgentFlag);

            XmlElement xeObrReadFlag = xmldoc.CreateElement("obr_read_flag");
            xeObrReadFlag.InnerText = "0";
            xe1.AppendChild(xeObrReadFlag);

            XmlElement xeObrPrintFlag = xmldoc.CreateElement("obr_print_flag");
            xeObrPrintFlag.InnerText = qcResult.patient.RepStatus.ToString();
            xe1.AppendChild(xeObrPrintFlag);

            XmlElement xeObrPrintTime = xmldoc.CreateElement("obr_print_time");
            xeObrPrintTime.InnerText = qcResult.patient.RepPrintDate!=null? qcResult.patient.RepPrintDate.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            xe1.AppendChild(xeObrPrintTime);

            XmlElement xePidBackOutSigin = xmldoc.CreateElement("pid_backout_sign");
            xePidBackOutSigin.InnerText ="";
            xe1.AppendChild(xePidBackOutSigin);
            #endregion
        }



        /// <summary>
        /// <summary>
        /// 检验危急值发送短信
        /// </summary>
        /// <param name="callee">手机号码</param>
        /// <param name="dept_name">科室名称</param>
        /// <param name="doctor_name">医生姓名</param>
        /// <param name="patient_name">患者姓名</param>
        /// <param name="patient_id">门诊ID或住院号</param>
        /// <param name="bed_no">床号</param>
        /// <param name="item_name">项目名称</param>
        /// <param name="item_result">项目结果</param>
        /// <param name="type">结果类型</param>
        /// <param name="paramsXml">参数</param>
        /// <returns></returns>
        public string LISSmsSendLisXml(string model, string callee, string dept_name, string doctor_name, string patient_name, string patient_id, string bed_no, string item_name, string item_result, string type, string paramsXml)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);


            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            XmlElement xeCol = xmldoc.CreateElement("callee");
            xeCol.InnerText = callee;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("dept_name");
            xeCol.InnerText = dept_name;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("doctor_name");
            xeCol.InnerText = doctor_name;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("patient_name");
            xeCol.InnerText = patient_name;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("patient_id");
            xeCol.InnerText = patient_id;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("bed_no");
            xeCol.InnerText = bed_no;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("item_name");
            xeCol.InnerText = item_name;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("item_result");
            xeCol.InnerText = item_result;
            xe1.AppendChild(xeCol);

            xeCol = xmldoc.CreateElement("type");
            xeCol.InnerText = type;
            xe1.AppendChild(xeCol);

            string patTel = string.Empty;
            if (!string.IsNullOrEmpty(paramsXml))
            {
                XmlDocument xmldocRequest = new XmlDocument();
                xmldocRequest.LoadXml(paramsXml);
                XmlNode telNode = xmldocRequest.SelectSingleNode("/root/patTel");
                patTel = telNode.InnerText;//病人电话号码 
            }

            xeCol = xmldoc.CreateElement("patient_tel");
            xeCol.InnerText = patTel;
            xe1.AppendChild(xeCol);

            return XMLDocumentToString(xmldoc);
        }



        /// <summary>
        /// 获取医生手机号码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="doctor_code">工号</param>
        /// <returns></returns>
        public string LISGetDoctorPhoneXml(string model, string doctor_code)
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("DocumentElement");
            xmldoc.AppendChild(xmlelem);

            XmlElement xeAK = xmldoc.CreateElement("AccessKey");
            xeAK.InnerText = ConfigurationManager.AppSettings["AccessKey"];
            xmlelem.AppendChild(xeAK);

            XmlElement xeMN = xmldoc.CreateElement("MethodName");
            xeMN.InnerText = model;
            xmlelem.AppendChild(xeMN);

            XmlElement xe1 = xmldoc.CreateElement("DataTable");
            xmlelem.AppendChild(xe1);

            XmlElement xeOrgId = xmldoc.CreateElement("data_org_id");
            xeOrgId.InnerText = ConfigurationManager.AppSettings["OrgId"];
            xe1.AppendChild(xeOrgId);

            XmlElement xeSysId = xmldoc.CreateElement("data_sys_id");
            xeSysId.InnerText = ConfigurationManager.AppSettings["SysId"];
            xe1.AppendChild(xeSysId);

            XmlElement xeDoctorCode = xmldoc.CreateElement("doctor_code");
            xeDoctorCode.InnerText = doctor_code;
            xe1.AppendChild(xeDoctorCode);

            return XMLDocumentToString(xmldoc);
        }


        /// <summary>

        /// XML转义字符处理

        /// </summary>

        public string ConvertXml(string xml)
        {
            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀

            for (int intNext = 0; true;)
            {
                int intIndexOf = xml.IndexOf("&", intNext);

                intNext = intIndexOf + 1;  //避免&被重复替换

                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true;)
            {
                int intIndexOf = xml.IndexOf("<");

                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true;)
            {
                int intIndexOf = xml.IndexOf(">");

                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true;)
            {
                int intIndexOf = xml.IndexOf("\"");

                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);
                }
            }

            return xml.Replace(((char)1).ToString(), "");
        }
        private string XMLDocumentToString(XmlDocument xmldoc)
        {
            MemoryStream stream = new MemoryStream();
            string strEncoding = "gb2312";
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.GetEncoding(strEncoding));

            writer.Formatting = Formatting.Indented;

            xmldoc.Save(writer);

            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("gb2312"));

            stream.Position = 0;

            string XMLstring = reader.ReadToEnd();

            reader.Close();
            stream.Close();

            return XMLstring;

        }


        public string CreatePostHttpResponse(string xmlResult)
        {
            try
            {
                string wsdlAddress = ConfigurationManager.AppSettings["HQInterfaceUrl"];
                if (string.IsNullOrEmpty(wsdlAddress))
                    return "未配置HQInterfaceUrl地址";
                Lib.LogManager.Logger.LogInfo("地址:" + wsdlAddress);
                HttpWebRequest request = WebRequest.Create(wsdlAddress) as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";


                byte[] data = Encoding.Default.GetBytes(xmlResult);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse result = request.GetResponse() as HttpWebResponse;

                using (Stream s = result.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.GetEncoding("gb2312"));
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        /// <summary>
        /// 把xml内容转成指定Datatable
        /// </summary>
        /// <param name="strXml">XML原文</param>
        /// <param name="tableName">table名</param>
        /// <param name="dt">转成功的数据表</param>
        /// <returns>true成功</returns>
        [System.ComponentModel.Description("把xml内容转成指定Datatable")]
        public static bool ConvertXmlToDatatable(string strXml, out DataTable dt)
        {
            bool bln = false;
            dt = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXml);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables["DataTable"];
                    bln = true;
                }
                else {
                    dt = ds.Tables[0];
                    bln = true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return bln;
        }


        /// <summary>
        /// 把xml内容转成指定Datatable
        /// </summary>
        /// <param name="strXml">XML原文</param>
        /// <param name="tableName">table名</param>
        /// <param name="dt">转成功的数据表</param>
        /// <returns>true成功</returns>
        [System.ComponentModel.Description("把xml内容转成指定Datatable")]
        public bool ConvertXmlToDatatable(string strXml, string tableName, out DataTable dt)
        {
            bool bln = false;
            dt = null;
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(strXml);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains(tableName) && ds.Tables[tableName] != null)
                    {
                        dt = ds.Tables[tableName].Copy();

                        bln = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("ConvertXmlToDatatable", new Exception("把xml内容转成指定Datatable,遇到错误！\r\n" + ex.ToString()));
            }

            return bln;
        }
    }
}
