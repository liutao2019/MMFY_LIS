using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.report;
using dcl.svr.result;
using dcl.svr.sample;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace dcl.outside.service
{
    public class WFServiceNew
    {
        public string WFInterface(string MethodName, string XMLData)
        {
            string strResultXml = string.Empty;

            switch (MethodName)
            {
                case "GetPatientList":
                    strResultXml = GetPatientList(XMLData);
                    break;
                case "GetRoutineResult":
                    strResultXml = GetRoutineResult(XMLData);
                    break;
                case "PidOperationRecord":
                    strResultXml = PidOperationRecord(XMLData);
                    break;
                case "OperationRecord":
                    strResultXml = OperationRecord(XMLData);
                    break;
                case "GetBarcodeInfo":
                    strResultXml = GetBarcodeInfo(XMLData);
                    break;
                case "GetPatientReport":
                    strResultXml = GetPatientReport(XMLData);
                    break;
                case "GetPrintRepId":
                    strResultXml = GetPrintRepId(XMLData);
                    break;
                default:
                    strResultXml = ErroMessage("非法方法");
                    break;
            }

            return strResultXml;
        }

        /// <summary>
        /// 根据唯一号获取未打印的所有报告单号
        /// </summary>
        /// <param name="xMLData"></param>
        /// <returns></returns>
        private string GetPrintRepId(string xMLData)
        {
            try
            {
                NameValueCollection parameter = ConvertToParameter(xMLData);

                if (parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                XmlDocument doc = ResponseTemplate;
                IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                EntityPidReportMain patient = new EntityPidReportMain();
                string printStatus = string.Empty;
                XmlElement elementValue = null;
                string notFoundRepId = string.Empty;
                string strPid = parameter["PidInNo"]; //唯一号

                XmlNode nodeResult = doc.SelectSingleNode("Response/Result");

                if (string.IsNullOrEmpty(strPid))
                    return ErroMessage("参数 PidInNo 不能为空");

                var patientList = dao.PatientQuery(new EntityPatientQC { PidInNo = strPid });

                if(patientList.Count > 0)
                {
                    elementValue = doc.CreateElement("PidInNo");
                    elementValue.InnerText = strPid;
                    nodeResult.AppendChild(elementValue);

                    string repId = string.Join(",", patientList.FindAll(a => a.RepStatus == 2).Select(a => a.RepId).ToArray());

                    elementValue = doc.CreateElement("RepId");
                    elementValue.InnerText = repId;
                    nodeResult.AppendChild(elementValue);

                    return ConvertXmlToString(doc);
                }                
                else
                {
                    return ErroMessage("唯一号不存在或暂无可打印报告");
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 获取病人PDF
        /// </summary>
        /// <param name="MLData"></param>
        /// <returns></returns>
        private string GetPatientReport(string XMLData)
        {
            NameValueCollection Parameter = ConvertToParameter(XMLData);
            if (Parameter.Count == 0)
                return ErroMessage("调用参数不能为空");

            //报告ID
            string strRepId = Parameter["RepId"];
            if (string.IsNullOrEmpty(strRepId))
                return ErroMessage("参数RepId不能为空");
            List<EntityDicInstrument> listInstrmt = dcl.svr.cache.DictInstrmtCache.Current.DclCache;

            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            EntityPidReportMain patient = dao.GetPatientInfo(strRepId);

            if (patient != null && !string.IsNullOrEmpty(patient.RepItrId))
            {
                int i = listInstrmt.FindIndex(w => w.ItrId == patient.RepItrId);
                if (i >= 0)
                {
                    EntityDCLPrintParameter par = new EntityDCLPrintParameter();
                    par.RepId = strRepId;
                    par.ReportCode = listInstrmt[i].ItrReportId;

                    DCLReportPrintBIZ reportBiz = new DCLReportPrintBIZ();
                    string pdf = reportBiz.GetReportPDF(par);
                    XmlDocument doc = ResponseTemplate;

                    XmlNode nodeResult = doc.SelectSingleNode("Response/Result");

                    XmlElement elementValue = null;

                    XmlElement obrResult = doc.CreateElement("Patient");
                    nodeResult.AppendChild(obrResult);

                    elementValue = doc.CreateElement("RepId");
                    elementValue.InnerText = strRepId;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PatientReport");
                    elementValue.InnerText = pdf;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PrintStatus");
                    elementValue.InnerText = patient.RepStatus == 4 ? "1" : "0";
                    obrResult.AppendChild(elementValue);

                    return ConvertXmlToString(doc);
                
                }
                else
                    return ErroMessage("该报告仪器已被删除，请联系工程师");
            }
            else
                return ErroMessage("未查到该病人报告");
        }

        /// <summary>
        /// 获取病人列表
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private string GetPatientList(string XMLData)
        {
            try
            {
                NameValueCollection Parameter = ConvertToParameter(XMLData);

                if (Parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                string strMsg = string.Empty;
                EntityPatientQC patientCondition = CreateQC(Parameter, ref strMsg);

                if (strMsg != string.Empty)
                    return ErroMessage(strMsg);

                PidReportMainBIZ biz = new PidReportMainBIZ();

                List<EntityPidReportMain> listPidReportMain = biz.PatientQuery(patientCondition);

                XmlDocument doc = ResponseTemplate;

                XmlNode nodeResult = doc.SelectSingleNode("Response/Result");

                XmlElement elementValue = null;

                foreach (EntityPidReportMain pidReportMain in listPidReportMain)
                {
                    XmlElement resultCode = doc.CreateElement("PidReportMain");
                    nodeResult.AppendChild(resultCode);

                    elementValue = doc.CreateElement("PidInNo");
                    elementValue.InnerText = pidReportMain.PidInNo;
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PidBarCode");
                    elementValue.InnerText = pidReportMain.RepBarCode;
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PidAddmissTimes");
                    elementValue.InnerText = pidReportMain.PidAddmissTimes.ToString();
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RepId");
                    elementValue.InnerText = pidReportMain.RepId;
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PidComName");
                    elementValue.InnerText = pidReportMain.PidComName;
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RepInDate");
                    elementValue.InnerText = pidReportMain.RepInDate.ToString();
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RepReportUserName");
                    elementValue.InnerText = pidReportMain.RepReportUserName;
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RepReportDate");
                    elementValue.InnerText = pidReportMain.RepReportDate.ToString();
                    resultCode.AppendChild(elementValue);

                    elementValue = doc.CreateElement("PidDocName");
                    elementValue.InnerText = pidReportMain.PidDocName;
                    resultCode.AppendChild(elementValue);
                }

                return ConvertXmlToString(doc);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 生成查询条件
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private EntityPatientQC CreateQC(NameValueCollection Parameter, ref string strMsg)
        {
            EntityPatientQC patientCondition = new EntityPatientQC();

            strMsg = string.Empty;

            //就诊号
            string strPidInNo = Parameter["PidInNo"];
            //住院次数
            string strPidAddmissTimes = Parameter["PidAddmissTimes"];
            //病人来源
            string strPidSrcId = Parameter["PidSrcId"];
            //报告开始时间
            string strRepReportStartDate = Parameter["StartDate"];
            //报告结束时间
            string strRepReportEndDate = Parameter["EndDate"];
            //就诊卡号
            string strPidSocialNo = Parameter["PidSocialNo"];
            //长号
            string strRepInputId = Parameter["RepInputId"];

            patientCondition.PidInNo = strPidInNo;
            patientCondition.RepStatus = "'2','4'";

            if (!string.IsNullOrEmpty(strRepReportStartDate))
            {
                DateTime dt = DateTime.Now;
                if (!DateTime.TryParse(strRepReportStartDate, out dt))
                    strMsg = "开始时间格式错误";
                else
                    patientCondition.DateStart = dt;
            }

            if (!string.IsNullOrEmpty(strRepReportEndDate))
            {
                DateTime dt = DateTime.Now;
                if (!DateTime.TryParse(strRepReportEndDate, out dt))
                    strMsg = "结束时间格式错误";
                else
                    patientCondition.DateEnd = dt;
            }

            if (!string.IsNullOrEmpty(strPidSrcId))
            {
                patientCondition.RepSrcId = strPidSrcId;
            }

            return patientCondition;
        }


        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private string GetRoutineResult(string XMLData)
        {
            try
            {
                NameValueCollection Parameter = ConvertToParameter(XMLData);

                if (Parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                string strRepId = Parameter["RepId"];

                if (string.IsNullOrEmpty(strRepId))
                    return ErroMessage("参数RepId不能为空");

                ObrResultBIZ biz = new ObrResultBIZ();
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId.Add(strRepId);
                List<EntityObrResult> listResult = biz.ObrResultQuery(resultQc);

                XmlDocument doc = ResponseTemplate;

                XmlNode nodeResult = doc.SelectSingleNode("Response/Result");

                XmlElement elementValue = null;

                foreach (EntityObrResult pidReportMain in listResult)
                {
                    XmlElement obrResult = doc.CreateElement("ObrResult");
                    nodeResult.AppendChild(obrResult);

                    elementValue = doc.CreateElement("RepId");
                    elementValue.InnerText = pidReportMain.ObrId;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("ItmId");
                    elementValue.InnerText = pidReportMain.ItmId;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("ItmName");
                    elementValue.InnerText = pidReportMain.ItmName;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("ObrValue");
                    elementValue.InnerText = pidReportMain.ObrValue;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("ResTips");
                    elementValue.InnerText = pidReportMain.ResTips;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RefLowerLimit");
                    elementValue.InnerText = pidReportMain.RefLowerLimit;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("RefUpperLimit");
                    elementValue.InnerText = pidReportMain.RefUpperLimit;
                    obrResult.AppendChild(elementValue);

                    elementValue = doc.CreateElement("ObrUnit");
                    elementValue.InnerText = pidReportMain.ObrUnit;
                    obrResult.AppendChild(elementValue);
                }

                return ConvertXmlToString(doc);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 回写病人打印操作记录
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private string PidOperationRecord(string XMLData)
        {
            try
            {
                NameValueCollection Parameter = ConvertToParameter(XMLData);

                if (Parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                if (string.IsNullOrEmpty(Parameter["RepId"]))
                    return ErroMessage("参数RepId不能为空");

                if (string.IsNullOrEmpty(Parameter["OperationStatus"]))
                    return ErroMessage("参数OperationStatus不能为空");


                int operationStatus = -1;
                if (!Int32.TryParse(Parameter["OperationStatus"], out operationStatus))
                    return ErroMessage("参数OperationStatus格式错误");

                if(operationStatus != 4)
                    return ErroMessage("参数OperationStatus数值错误");

                string RepId = Parameter["RepId"];

                //PidReportMainBIZ rep = new PidReportMainBIZ();
                //rep.UpdatePrintState(new List<string> { RepId }, operationStatus.ToString());

                var pidReportMainBIZ = new PidReportMainBIZ();
                pidReportMainBIZ.UpdatePrintState_whitOperator(new List<string> { RepId }, "4", "-1", "新自助机", "自助打印程序打印");

                XmlDocument doc = ResponseTemplate;
                return ConvertXmlToString(doc);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }


        /// <summary>
        /// 回写操作记录
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private string OperationRecord(string XMLData)
        {
            try
            {
                NameValueCollection Parameter = ConvertToParameter(XMLData);

                if (Parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                if (string.IsNullOrEmpty(Parameter["Barcode"]))
                    return ErroMessage("参数Barcode不能为空");

                if (string.IsNullOrEmpty(Parameter["OperationStatus"]))
                    return ErroMessage("参数OperationStatus不能为空");

                if (string.IsNullOrEmpty(Parameter["OperationTime"]))
                    return ErroMessage("参数OperationTime不能为空");

                if (string.IsNullOrEmpty(Parameter["OperationWorkId"]))
                    return ErroMessage("参数OperationWorkId不能为空");

                if (string.IsNullOrEmpty(Parameter["OperationName"]))
                    return ErroMessage("参数OperationName不能为空");

                int operationStatus = -1;
                if (!Int32.TryParse(Parameter["OperationStatus"], out operationStatus))
                    return ErroMessage("参数OperationStatus格式错误");

                DateTime operationTime = DateTime.Now;

                if (!DateTime.TryParse(Parameter["OperationTime"], out operationTime))
                    return ErroMessage("参数OperationTime格式错误");

                List<int> listAllowStatus = new List<int>();
                listAllowStatus.Add(2);
                listAllowStatus.Add(3);
                listAllowStatus.Add(4);
                listAllowStatus.Add(5);

                string strBarcode = Parameter["Barcode"];

                if (!listAllowStatus.Contains(operationStatus))
                    return ErroMessage("非法操作状态，请检查OperationStatus数据");

                SampProcessDetailBIZ detailBiz = new SampProcessDetailBIZ();
                EntitySampProcessDetail sampProcessDetail = detailBiz.GetLastSampProcessDetail(strBarcode);

                if (sampProcessDetail == null || string.IsNullOrEmpty(sampProcessDetail.ProcBarcode))
                    return ErroMessage("未找到该条码信息");

                int status = Convert.ToInt32(sampProcessDetail.ProcStatus);

                if (operationStatus <= status)
                    return ErroMessage("此条码已[" + sampProcessDetail.ProcStatusName + "]，无法执行此操作");

                string strOperationRemark = Parameter["OperationRemark"];

                EntitySampOperation operation = new EntitySampOperation();
                operation.OperationTime = operationTime;
                operation.OperationID = Parameter["OperationWorkId"];
                operation.OperationName = Parameter["OperationName"];
                operation.OperationStatus = operationStatus.ToString();
                operation.Remark = string.IsNullOrEmpty(strOperationRemark) ? "" : strOperationRemark + "[外部接口确认]";

                SampMainBIZ smBiz = new SampMainBIZ();
                EntitySampMain sm = smBiz.SampMainQueryByBarId(strBarcode);
                List<EntitySampMain> listSm = new List<EntitySampMain>();
                listSm.Add(sm);

                if (smBiz.UpdateSampMainStatus(operation, listSm))
                {
                    XmlDocument doc = ResponseTemplate;
                    return ConvertXmlToString(doc);
                }
                else
                {
                    return ErroMessage("操作失败，请联系管理员");
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private string GetBarcodeInfo(string XMLData)
        {
            try
            {
                NameValueCollection Parameter = ConvertToParameter(XMLData);

                if (Parameter.Count == 0)
                    return ErroMessage("调用参数不能为空");

                if (string.IsNullOrEmpty(Parameter["Barcode"]))
                    return ErroMessage("参数Barcode不能为空");

                string strBarcode = Parameter["Barcode"];

                SampMainBIZ biz = new SampMainBIZ();
                EntitySampMain sm = biz.SampMainQueryByBarId(strBarcode);

                if (sm == null || string.IsNullOrEmpty(sm.SampBarId))
                    return ErroMessage("未找到该条码信息");

                XmlDocument doc = ResponseTemplate;

                XmlNode nodeResult = doc.SelectSingleNode("Response/Result");

                XmlElement elementValue = null;

                XmlElement obrResult = doc.CreateElement("Barcode");
                nodeResult.AppendChild(obrResult);

                elementValue = doc.CreateElement("SampBarCode");
                elementValue.InnerText = sm.SampBarCode;
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("PidPatNo");
                elementValue.InnerText = sm.PidPatno;
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("PidAdmissTimes");
                elementValue.InnerText = sm.PidAdmissTimes.ToString();
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("PidInNo");
                elementValue.InnerText = sm.PidInNo;
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("SampComName");
                elementValue.InnerText = sm.SampComName;
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("SampStatusId");
                elementValue.InnerText = sm.SampStatusId;
                obrResult.AppendChild(elementValue);

                elementValue = doc.CreateElement("SampStatusName");
                elementValue.InnerText = sm.SampStatusName;
                obrResult.AppendChild(elementValue);

                XmlElement elementSampDetail = doc.CreateElement("ListSampDetail");
                obrResult.AppendChild(elementSampDetail);
                XmlElement elementDetailValue = null;
                foreach (EntitySampDetail sampDetail in sm.ListSampDetail)
                {
                    XmlElement xeSampDetail = doc.CreateElement("SampDetail");
                    elementSampDetail.AppendChild(xeSampDetail);

                    elementDetailValue = doc.CreateElement("OrderCode");
                    elementDetailValue.InnerText = sampDetail.OrderCode;
                    xeSampDetail.AppendChild(elementDetailValue);

                    elementDetailValue = doc.CreateElement("OrderName");
                    elementDetailValue.InnerText = sampDetail.OrderName;
                    xeSampDetail.AppendChild(elementDetailValue);

                    elementDetailValue = doc.CreateElement("OrderSn");
                    elementDetailValue.InnerText = sampDetail.OrderSn;
                    xeSampDetail.AppendChild(elementDetailValue);
                }

                XmlElement elementSampProcessDetail = doc.CreateElement("ListSampProcessDetail");
                obrResult.AppendChild(elementSampProcessDetail);
                XmlElement elementProcessValue = null;
                foreach (EntitySampProcessDetail sampProcess in sm.ListSampProcessDetail)
                {
                    XmlElement xeSampProcessDetail = doc.CreateElement("SampProcessDetail");
                    elementSampProcessDetail.AppendChild(xeSampProcessDetail);

                    elementProcessValue = doc.CreateElement("ProcDate");
                    elementProcessValue.InnerText = sampProcess.ProcDate.ToString();
                    xeSampProcessDetail.AppendChild(elementProcessValue);

                    elementProcessValue = doc.CreateElement("ProcStatus");
                    elementProcessValue.InnerText = sampProcess.ProcStatus;
                    xeSampProcessDetail.AppendChild(elementProcessValue);

                    elementProcessValue = doc.CreateElement("ProcStatusName");
                    elementProcessValue.InnerText = sampProcess.ProcStatusName;
                    xeSampProcessDetail.AppendChild(elementProcessValue);

                    elementProcessValue = doc.CreateElement("ProcUsercode");
                    elementProcessValue.InnerText = sampProcess.ProcUsercode;
                    xeSampProcessDetail.AppendChild(elementProcessValue);

                    elementProcessValue = doc.CreateElement("ProcUsername");
                    elementProcessValue.InnerText = sampProcess.ProcUsername;
                    xeSampProcessDetail.AppendChild(elementProcessValue);
                }

                return ConvertXmlToString(doc);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErroMessage(ex.ToString());
            }
        }

        /// <summary>
        /// 返回模板
        /// </summary>
        private XmlDocument ResponseTemplate
        {
            get
            {
                XmlDocument doc = new XmlDocument();

                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);

                XmlElement root = doc.CreateElement("Response");
                doc.AppendChild(root);

                XmlElement resultCode = doc.CreateElement("ResultCode");
                resultCode.InnerText = "0";
                root.AppendChild(resultCode);

                XmlElement resultContent = doc.CreateElement("ResultContent");
                resultContent.InnerText = "成功";
                root.AppendChild(resultContent);

                XmlElement result = doc.CreateElement("Result");
                root.AppendChild(result);

                return doc;
            }
        }

        /// <summary>
        /// 格式化入参
        /// </summary>
        /// <param name="XMLData"></param>
        /// <returns></returns>
        private NameValueCollection ConvertToParameter(string XMLData)
        {
            NameValueCollection Parameter = new NameValueCollection();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XMLData);

            XmlNode nodeRequest = doc.SelectSingleNode("Request");
            if (nodeRequest != null)
            {
                foreach (XmlNode item in nodeRequest.ChildNodes)
                {
                    Parameter.Add(item.Name, item.InnerText);
                }
            }

            return Parameter;
        }

        /// <summary>
        /// 获取错误消息格式
        /// </summary>
        /// <param name="erroInfo"></param>
        /// <returns></returns>
        private string ErroMessage(string erroInfo)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("Response");
            doc.AppendChild(root);

            XmlElement resultCode = doc.CreateElement("ResultCode");
            resultCode.InnerText = "1";
            root.AppendChild(resultCode);

            XmlElement resultContent = doc.CreateElement("ResultContent");
            resultContent.InnerText = erroInfo;
            root.AppendChild(resultContent);

            return ConvertXmlToString(doc);
        }

        /// <summary>
        /// XML转换为文本数据
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }
    }
}