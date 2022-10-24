using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using System.Data;
using Lib.DataInterface;
using System.Configuration;
using dcl.common;
using dcl.dao.interfaces;
using dcl.svr.cache;
using System.Collections.Specialized;
using System.Xml;
using Lib.DataInterface.Implement;
using Lib.DAC;

namespace dcl.svr.interfaces
{
    internal partial class DCLExtInterface_MMFY : DCLExtInterfaceBase
    {
        #region SQL
        string SQLCheckPassword = @"select * from hisinterface.v_employee_lis where user_login_name='{0}' and user_pwd='{1}' and valid_flag=1";
        #endregion

        public override List<EntitySampMain> DownloadOrderData(EntityInterfaceExtParameter parameter)
        {
            EntitySysInterfaceLog log = new EntitySysInterfaceLog();
            List<EntitySampMain> result = new List<EntitySampMain>();
            string sqlMain = String.Empty;
            try
            {
                //log.SampBarId = sampMain.SampBarCode;

                #region 获取连接串

                CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
                List<EntityDictDataInterfaceConnection> list = biz.SelectAll("慧扬HIS");
                if (list == null || list.Count <= 0)
                {
                    log.OperationName = "获取HIS数据库连接失败！";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = "获取HIS数据库连接失败";
                    SaveSysInterfaceLog(log);
                    return result;
                }

                #endregion

                #region 测试连接串
                string msg;
                DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
                conn.TestConnection(out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    log.OperationName = "HIS数据库连接串测试连接操作失败！";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = "HIS数据库连接串测试连接操作失败,详细问题：" + msg;
                    SaveSysInterfaceLog(log);
                    return result;
                }
                #endregion

                string sql = "select * from hisinterface.v_patient_orderinfo_lis_all where 1 = 1 {0} ";
                //string sql = "select * from hyhis.v_patient_orderinfo_lis_all where 1 = 1 {0} ";
                string sqlwhere = "";
                SqlHelper helper = conn.GetSqlHelper();

                sqlwhere += string.Format(@" and APPLY_TIME BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND
                    TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')",
                    parameter.StartTime, parameter.EndTime);

                if (!string.IsNullOrEmpty(parameter.PatientName))
                    sqlwhere += " and patient_name = '" + parameter.PatientName + "'";

                if (!string.IsNullOrEmpty(parameter.PatientID))
                    sqlwhere += " and patient_id = '" + parameter.PatientID + "'";

                if (parameter.DownloadType == InterfaceType.MZDownload)
                    sqlwhere += " and patient_type = '1'";
                else if (parameter.DownloadType == InterfaceType.MZDownload)
                    sqlwhere += " and patient_type = '2'";

                sqlMain = string.Format(sql, sqlwhere);
                Lib.LogManager.Logger.LogInfo("医嘱查询SQL :" + sqlMain);
                DataTable dt = helper.GetTable(sqlMain);
                List<EntitySampMain> samList = new List<EntitySampMain>();
                EntitySampMain sam;
                foreach (DataRow dr in dt.Rows)
                {
                    //ir.inpatient_no as patient_no,--住院号,门诊号
                    sam = new EntitySampMain();
                    sam.PidInNo = dr["patient_id"].ToString();
                    sam.PidName = dr["patient_name"].ToString();
                    sam.SampOccDate = Convert.ToDateTime(dr["apply_time"]);
                    //ia.item_no as item_no,--项目码
                    sam.SampComName = dr["item_name"].ToString();//ia.item_name as item_name,--项目名称
                    //ia.QTY as qty,--数量
                    //ia.PRICE as item_price,--单价
                    //hyhis.fun_get_dictionary_name('ITEM_SYS_TYPE', ia.item_sys_type) as item_type, --系统类别
                    sam.PidInsuId = dr["item_fee_type"].ToString();//item_fee_type,--费用类别
                    sam.SampCapcityUnit = dr["spec"].ToString();//ia.specs as spec,--规格
                    sam.SampPrice = dr["tot_cost"].ToString();//ia.TOT_COST as tot_cost --总价

                    samList.Add(sam);
                }
                return samList;
            }
            catch (Exception ex)
            {
                log.OperationName = "医嘱查询！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = ex.Message;
                SaveSysInterfaceLog(log);
                Lib.LogManager.Logger.LogInfo("DownloadOrderDataSQL: " + sqlMain);
                return result;
            }
        }

        /// <summary>
        /// 获取医嘱数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override List<EntityInterfaceData> DownloadInterfaceData(EntityInterfaceExtParameter parameter)
        {
            return base.DownloadInterfaceData(parameter);
        }


        #region 验证HIS账号密码
        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public override AuditInfo IdentityVerification(AuditInfo userInfo)
        {
            AuditInfo result = null;

            #region 获取连接串
            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            List<EntityDictDataInterfaceConnection> list = biz.SelectAll("慧扬HIS");
            if (list == null || list.Count <= 0)
            {
                Lib.LogManager.Logger.LogException(new Exception("获取HIS数据库连接信息失败，无法验证账号密码！"));
                return result;
            }
            #endregion

            #region 测试连接
            string msg;
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
            conn.TestConnection(out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return result;
            }
            #endregion
            string sql = string.Format(SQLCheckPassword,userInfo.UserId,userInfo.Password);
            try
            {
                SqlHelper helper = conn.GetSqlHelper();
                DataTable employee = helper.GetTable(sql);
                if (employee.Rows.Count == 0)
                    return result;
                result = new AuditInfo(); 
                result.UserId = employee.Rows[0]["user_login_name"].ToString();
                result.UserName = employee.Rows[0]["user_name"].ToString();
                result.UserStfId = employee.Rows[0]["user_id"].ToString();
    
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        #endregion

        #region 确费相关


        /// <summary>
        /// 条码签收前确认医嘱状态
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public override string ExecuteInterfaceBefore(EntitySampOperation operation, EntitySampMain sampMain)
        {
            string strMsg = string.Empty;

            if ((sampMain.PidSrcId == "107" && CacheSysConfig.Current.GetSystemConfig("Barcode_MZIsCharged") != "是")
                && sampMain.PidSrcId != "108")
                return strMsg;///只有住院病人和新门诊病人需要确认医嘱状态
            
            EntitySysInterfaceLog log = new EntitySysInterfaceLog();
            log.SampBarId = sampMain.SampBarCode;

            string SQL = @"select  dcFlag  from hisinterface.v_orderstatus_for_lis where dcFlag=1  {0}";
            if (operation.OperationStatus == "1" || operation.OperationStatus == "2"
                || operation.OperationStatus == "3" || operation.OperationStatus == "4"
                || operation.OperationStatus == "5" || operation.OperationStatus == "20")
            {
                try
                {
                    #region 获取连接串

                    CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
                    List<EntityDictDataInterfaceConnection> list = biz.SelectAll("慧扬HIS");
                    if (list == null || list.Count <= 0)
                    {
                        log.OperationName = "获取HIS数据库连接失败！";
                        log.OperationTime = DateTime.Now;
                        log.OperationSuccess = 0;
                        log.OperationContent = "获取HIS数据库连接失败";
                        SaveSysInterfaceLog(log);
                        return strMsg;
                    }

                    #endregion

                    #region 测试连接串
                    string msg;
                    DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
                    conn.TestConnection(out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        log.OperationName = "HIS数据库连接串测试连接操作失败！";
                        log.OperationTime = DateTime.Now;
                        log.OperationSuccess = 0;
                        log.OperationContent = "HIS数据库连接串测试连接操作失败,详细问题：" + msg;
                        SaveSysInterfaceLog(log);
                        return strMsg;
                    }
                    #endregion

                    string sqlwhere = "";
                    SqlHelper helper = conn.GetSqlHelper();

                    foreach (EntitySampDetail smadet in sampMain.ListSampDetail)
                    {
                        sqlwhere += string.Format(" and orderID='{0}' and registerID='{1}'", smadet.OrderSn, sampMain.SampPackNo);
                        object obj = helper.ExecuteScalar(string.Format(SQL, sqlwhere));
                        if(obj != null)
                        {
                            strMsg = string.Format("条码号【{0}】组合名称【{1}】的医嘱【{2}】已经停嘱!",
                                sampMain.SampBarCode,smadet.ComName,smadet.OrderSn);
                            log.OperationName = "确认医嘱状态！";
                            log.OperationTime = DateTime.Now;
                            log.OperationSuccess = 1;
                            log.OperationContent = strMsg;
                            SaveSysInterfaceLog(log);
                            return strMsg;
                        }
                    }

                    log.OperationName = "确认医嘱状态！";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 1;
                    log.OperationContent = "确认医嘱状态成功,医嘱未停嘱！";
                    SaveSysInterfaceLog(log);
                }
                catch(Exception ex)
                {
                    log.OperationName = "确认医嘱状态！";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = ex.Message;
                    SaveSysInterfaceLog(log);
                }
            }
            return strMsg;
            
        }

        /// <summary>
        /// 操作后执行接口，更新病人状态后执行
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        internal override void ExecuteInterfaceAfter(EntitySampOperation operation, EntitySampMain sampMain)
        {
            //1.获取接口地址
            string Url = ConfigurationManager.AppSettings["HIS_Charge_Addr"];//http://192.168.101.22:8087/services/PatientForLis
            string MethodName = ConfigurationManager.AppSettings["HIS_Charge_MethodName"]; //"AcceptMessage";
            if (string.IsNullOrEmpty(Url))
            {
                Lib.LogManager.Logger.LogInfo("HIS_Charge_Addr未配置！");
                return;
            }
            //2.操作判断: 107-门诊 & 5-签收 进行门诊收费操作
            if (sampMain.PidSrcId == "107" && operation.OperationStatus == "0" 
                && CacheSysConfig.Current.GetSystemConfig("Barcode_MZIsCharged") == "是")
            {
                string SendXML = BuildXML_Charge(sampMain, operation);
                string ErrorMsg;
                bool Result = InvokeHISCharge(Url, MethodName, SendXML, out ErrorMsg);

                //写入日志
                EntitySysInterfaceLog log = new EntitySysInterfaceLog();
                log.SampBarId = sampMain.SampBarId;
                log.OperationName = "门诊收费操作";
                log.OperationUserCode = operation.OperationWorkId;
                log.OperationUserName = operation.OperationName;
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = Result ? 1 : 0;
                log.OperationContent = ErrorMsg;
                SaveSysInterfaceLog(log);

            }
            //3.操作判断 108-住院 & 5-签收 进行住院收费操作
            else if (sampMain.PidSrcId == "108" && operation.OperationStatus == "5")
            {
                string SendXML = BuildXML_Charge(sampMain, operation);
                string ErrorMsg;
                bool Result = InvokeHISCharge(Url, MethodName, SendXML, out ErrorMsg);

                EntitySysInterfaceLog log = new EntitySysInterfaceLog();
                log.SampBarId = sampMain.SampBarId;
                log.OperationName = "住院收费操作";
                log.OperationUserCode = operation.OperationID;
                log.OperationUserName = operation.OperationName;
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = Result ? 1 : 0;
                log.OperationContent = ErrorMsg;
                SaveSysInterfaceLog(log);
            }
            //4.操作判断 107-门诊 ||108-住院 || 109-体检  &   9-回退 & 501-删除条码  进行退费 
            else if ((sampMain.PidSrcId == "107" && (operation.OperationStatus == "510") && CacheSysConfig.Current.GetSystemConfig("Barcode_MZIsCharged") == "是") ||
                      (sampMain.PidSrcId == "108" && (operation.OperationStatus == "9" || operation.OperationStatus == "510")) 
                      //||(sampMain.PidSrcId == "109" && (operation.OperationStatus == "9" || operation.OperationStatus == "510"))
                      )
            {
                string SendXML = BuildXML_CancelChaged(sampMain, operation);
                string ErrorMsg;
                bool Result = InvokeHISCharge(Url, MethodName, SendXML, out ErrorMsg);

                foreach (EntitySampDetail item in sampMain.ListSampDetail)
                {
                    EntitySysInterfaceLog log = new EntitySysInterfaceLog();
                    log.SampBarId = sampMain.SampBarId;
                    log.OrderSn = item.OrderSn;
                    log.OperationName = "退费";
                    log.OperationUserCode = operation.OperationWorkId;
                    log.OperationUserName = operation.OperationName;
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = Result ? 1 : 0;
                    log.OperationContent = ErrorMsg;
                    SaveSysInterfaceLog(log);
                }
            }
        }

        #region HIS确认费用接口相关
        /// <summary>
        /// 调用HIS确费接口
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="sendXML"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        private bool InvokeHISCharge(string URL, string MethodName, string sendXML, out string ErrorMsg)
        {
            ErrorMsg = "";
            try
            {
                string Response;
                WebServiceAgent factory = new WebServiceAgent();
                bool Result = factory.Invoke(URL, MethodName, sendXML, out Response, out ErrorMsg);
                if (!Result)
                {
                    Lib.LogManager.Logger.LogInfo("HIS费用接口收费失败:" + ErrorMsg);
                    return false;
                }
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Response);
                string msgresult = doc.SelectSingleNode("/Response/MessageHead/MessageResultValue").InnerText;
                string msgcontent = doc.SelectSingleNode("/Response/MessageHead/MessageReusltInfo").InnerText;
                if (msgresult == "-1")
                {
                    Lib.LogManager.Logger.LogInfo("HIS费用接口收费返回失败结果:" + Response);
                    ErrorMsg = msgcontent;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("HIS费用接口错误", ex);
                ErrorMsg = "错误信息:" + ex.Message;
                return false;
            }
        }

        private string BuildXML_Charge(EntitySampMain sma, EntitySampOperation oper)
        {
            #region SendString
            string BaseXML = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.connectors.com/"">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:AcceptMessage>
         <!--Optional:-->
         <arg0><![CDATA[
    <Request>
    <MessageHead>
        <MessageID></MessageID>
        <MessageSender>@MessageSender</MessageSender>
        <MessageType>@MessageType</MessageType>
    </MessageHead>
    <MessageBody>
        <NewDataSet>
            <PatientType>@PatientType</PatientType>
            <PatientID>@PatientID</PatientID>
            <PatientNO>@PatientNO</PatientNO>
            <RegisterID>@RegisterID</RegisterID>
            <ExecDeptID>@ExecDeptID</ExecDeptID>
            <ExecDoctID>@ExecDoctID</ExecDoctID>
            <ExecDate>@ExecDate</ExecDate>
            <ExecList>
            @ExecList
            </ExecList>
        </NewDataSet>
    </MessageBody>
</Request>
]]></arg0>
      </ws:AcceptMessage>
   </soapenv:Body>
</soapenv:Envelope>
";
            string BaseOrderXML = @" <ExecOrder>
                    <ApplyID>@ApplyID</ApplyID>
                    <OrderID>@OrderID</OrderID>
                    <ReportID>@ReportID</ReportID>
                    <Barcode>@Barcode</Barcode>
                    <TubeCode>@TubeCode</TubeCode>
                </ExecOrder>";
            #endregion
            IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
            List<EntityDicDoctor> lst = dao.Search(null);

            EntityDicDoctor doctor = lst.Find(w=>w.DoctorCode == oper.OperationID);


            string ExecOrder = "";
            foreach (EntitySampDetail smadet in sma.ListSampDetail)
            {
                ExecOrder += BaseOrderXML.Replace("@ApplyID", smadet.ApplyID)
                    .Replace("@OrderID", smadet.OrderSn)
                    .Replace("@ReportID", "")
                    .Replace("@Barcode", smadet.SampBarCode)
                    .Replace("@TubeCode", sma.TubChargeCode);
            }
            string MethodName = "LisExecInfo";

            return BaseXML.Replace("@MessageType", MethodName)
                .Replace("@MessageSender", sma.ReceiverUserId)
                .Replace("@PatientType", sma.PidSrcId == "108" ? "2" : "1")
                .Replace("@PatientID", sma.PidPatno)
                .Replace("@PatientNO", sma.PidInNo)
                .Replace("@RegisterID", sma.SampPackNo)
                .Replace("@ExecDeptID", sma.SampLastactionPlace)
                .Replace("@ExecDoctID", string.IsNullOrEmpty(doctor?.DoctorId)? oper.OperationID: doctor.DoctorId)
                .Replace("@ExecDate", oper.OperationTime.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("@ExecList", ExecOrder)
                ;
        }

        private string BuildXML_CancelChaged(EntitySampMain sma, EntitySampOperation oper)
        {
            #region SendString
            string BaseXMLCancel = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.connectors.com/"">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:AcceptMessage>
         <!--Optional:-->
         <arg0><![CDATA[
    <Request>
    <MessageHead>
        <MessageID></MessageID>
        <MessageSender>@MessageSender</MessageSender>
        <MessageType>@MessageType</MessageType>
    </MessageHead>
    <MessageBody>
        <NewDataSet>
            <PatientType>@PatientType</PatientType>
            <PatientID>@PatientID</PatientID>
            <PatientNO>@PatientNO</PatientNO>
            <RegisterID>@RegisterID</RegisterID>
            <CancelDeptID>@ExecDeptID</CancelDeptID>
            <CancelDoctID>@ExecDoctID</CancelDoctID>
            <CancelDate>@ExecDate</CancelDate>
            <CancelList>
            @ExecList
            </CancelList>
        </NewDataSet>
    </MessageBody>
</Request>
]]></arg0>
      </ws:AcceptMessage>
   </soapenv:Body>
</soapenv:Envelope>";

            string BaseOrderXMLCancel = @"<CancelOrder>
                    <ApplyID>@ApplyID</ApplyID>
                    <OrderID>@OrderID</OrderID>
                    <ReportID>@ReportID</ReportID>
                </CancelOrder>";
            #endregion

            IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
            List<EntityDicDoctor> lst = dao.Search(null);
            EntityDicDoctor doctor = lst.Find(w => w.DoctorCode == oper.OperationID);

            string ExecOrder = "";
            foreach (EntitySampDetail smadet in sma.ListSampDetail)
            {
                ExecOrder += BaseOrderXMLCancel.Replace("@ApplyID", smadet.ApplyID)
                    .Replace("@OrderID", smadet.OrderSn)
                    .Replace("@ReportID", "")
                    .Replace("@Barcode", smadet.SampBarCode);

            }
            string MethodName = "LisCancelExecInfo";

            return BaseXMLCancel.Replace("@MessageType", MethodName)
                .Replace("@MessageSender", sma.ReceiverUserId)
                .Replace("@PatientType", sma.PidSrcId == "108" ? "2" : "1")
                .Replace("@PatientID", sma.PidPatno)
                .Replace("@PatientNO", sma.PidInNo)
                .Replace("@RegisterID", sma.SampPackNo)
                .Replace("@ExecDeptID", sma.SampLastactionPlace)
                .Replace("@ExecDoctID", string.IsNullOrEmpty(doctor?.DoctorId) ? oper.OperationID : doctor.DoctorId)
                .Replace("@ExecDate", oper.OperationTime.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("@ExecList", ExecOrder)
                ;
        }

        #endregion


        #endregion

        #region 检验结果上传/撤回中间表

        /// <summary>
        /// 中期报告
        /// </summary>
        /// <param name="listRepId"></param>
        /// <returns></returns>
        internal override NameValueCollection UploadDCLMidReport(List<string> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            EntitySysInterfaceLog log = new EntitySysInterfaceLog();

            #region 获取连接串

            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            List<EntityDictDataInterfaceConnection> list = biz.SelectAll("lis中间库");
            if (list == null || list.Count <= 0)
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "获取中间表连接串失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "获取中间表连接串失败";
                SaveSysInterfaceLog(log);
                return result;
            }

            #endregion

            #region 测试连接串
            string msg;
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
            conn.TestConnection(out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "中间表连接串测试连接操作失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "中间表连接串测试连接操作失败,详细问题：" + msg;
                SaveSysInterfaceLog(log);
                return result;
            }

            #endregion

            string sqlDeleteReport = "delete from smd.smp_lis_report t where t.FREPORT_NO = '{0}'; ";
            string sqlDeleteResult = "delete from {0} where FREPORT_ID='{1}';";

            SqlHelper helper = conn.GetSqlHelper();
            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();

            List<EntityDCLReportParmeter> DCLParms = dao.GetDCLParmeter(listRepId);
            

            foreach (string strPatId in listRepId)
            {
                string sqlMain = "begin   ";
                try
                {
                    EntityQcResultList pidResult = base.GetPidResult(strPatId);

                    log = new EntitySysInterfaceLog();
                    log.SampBarId = pidResult?.patient?.RepBarCode;
                    log.RepId = strPatId;
                    log.OperationName = "上传中期报告到中间表";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = "";


                    if (pidResult.patient == null)
                    {
                        log.OperationSuccess = 0;
                        log.OperationContent = "上传中期报告到中间表-获取病人信息失败！";
                        SaveSysInterfaceLog(log);
                        continue;
                    }

                    #region 删除之前的报告（中期）

                    sqlMain += string.Format(sqlDeleteReport, strPatId);
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result", strPatId);//报告从表
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//细菌（微生物）报告
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_allergy", strPatId);//药敏报告
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//药敏报告

                    #endregion

                    EntityDCLReportParmeter dp = DCLParms.Find(w=>w.PmaRepID == strPatId);


                    Dictionary<String, OracleValue> parm = GetSmpLisReport(pidResult, dp);
                    sqlMain += helper.GetInsertSQL("smd.smp_lis_report", parm) + ";";//报告主表

                    int count = 0;//排序计数
                  
                    count = 0;
                    foreach (EntityObrResultBact item in pidResult.listBact)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultBact(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result_bact", parm) + ";";//鉴定药敏
                    }

                    count = 0;
                    foreach (EntityObrResultAnti item in pidResult.listAnti)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultAllerg(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result_allergy", parm) + ";";//抗生素
                    }

                    count = 0;
                    foreach (EntityObrResultDesc item in pidResult.listDesc)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultBact_Desc(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result_bact", parm) + ";";//无菌或涂片
                    }

                    sqlMain += "   end;";

                    helper.ExecuteNonQuery(sqlMain);
                    log.OperationSuccess = 1;
                    log.OperationContent = "上传中期报告到中间表成功！";

                }
                catch (Exception ex)
                {
                    log.OperationSuccess = 0;
                    log.OperationContent = "上传中期报告到中间表失败,详细信息： " + ex.Message;
                }
                result.Add(strPatId, log.OperationSuccess.Value == 0 ? "失败" : "成功");
                SaveSysInterfaceLog(log);
            }

            return result;
        }


        /// <summary>
        ///  报告（二审）
        /// </summary>
        /// <param name="listRepId"></param>
        /// <returns></returns>
        internal override NameValueCollection UploadDCLReport(List<string> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            EntitySysInterfaceLog log = new EntitySysInterfaceLog();

            #region 获取连接串

            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            List<EntityDictDataInterfaceConnection> list = biz.SelectAll("lis中间库");
            if (list == null || list.Count <= 0)
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "获取中间表连接串失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "获取中间表连接串失败";
                SaveSysInterfaceLog(log);
                return result;
            }

            #endregion

            #region 测试连接串
            string msg;
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
            conn.TestConnection(out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "中间表连接串测试连接操作失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "中间表连接串测试连接操作失败,详细问题：" + msg;
                SaveSysInterfaceLog(log);
                return result;
            }

            
            #endregion

            string sqlDeleteReport = "delete from smd.smp_lis_report t where t.FREPORT_NO = '{0}'; ";
            string sqlDeleteResult = "delete from {0} where FREPORT_ID='{1}';";

            SqlHelper helper = conn.GetSqlHelper();
            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();

            List<EntityDCLReportParmeter> DCLParms = dao.GetDCLParmeter(listRepId);


            foreach (string strPatId in listRepId)
            {
                string sqlMain = "begin   ";
                try
                {
                    EntityQcResultList pidResult = base.GetPidResult(strPatId);


                    log = new EntitySysInterfaceLog();
                    log.SampBarId = pidResult?.patient?.RepBarCode;
                    log.RepId = strPatId;
                    log.OperationName = "上传二审数据到中间表";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = "";


                    #region 删除之前的报告单
                    sqlMain += string.Format(sqlDeleteReport, strPatId);
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result", strPatId);//报告从表
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//细菌（微生物）报告
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_allergy", strPatId);//药敏报告
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//药敏报告
                    #endregion

                    
                    if (pidResult.patient == null)
                    {
                        log.OperationSuccess = 0;
                        log.OperationContent = "上传二审数据到中间表-获取病人信息失败！";
                        SaveSysInterfaceLog(log);
                        continue;
                    }

                    EntityDCLReportParmeter dp = DCLParms.Find(w => w.PmaRepID == strPatId);

                    Dictionary<String, OracleValue> parm = GetSmpLisReport(pidResult, dp);
                    sqlMain += helper.GetInsertSQL("smd.smp_lis_report", parm) +";";//报告主表


                    int count = 0;//排序计数
                    //重新排序
                    List<EntityObrResult> Res = pidResult.listResulto?.OrderBy(i => i.ResComSeq).ThenBy(i => i.ComMiSort).ToList();

                    foreach (EntityObrResult item in Res)
                    {

                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResult(item, pidResult.patient,count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result", parm) + ";";//报告从表
                    }


                    count = 0;
                    foreach (EntityObrResultBact item in pidResult.listBact)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultBact(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result_bact", parm) + ";";//鉴定药敏
                    }

                    count = 0;
                    foreach (EntityObrResultAnti item in pidResult.listAnti)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultAllerg(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain +=  helper.GetInsertSQL("smd.smp_lis_result_allergy", parm)+";";//抗生素
                    }

                    count = 0;
                    foreach (EntityObrResultDesc item in pidResult.listDesc)
                    {
                        parm.Clear();
                        count += 1;
                        parm = GetSmpLisResultBact_Desc(item, pidResult.patient, count);
                        if (parm.Count > 0)
                            sqlMain += helper.GetInsertSQL("smd.smp_lis_result_bact", parm) + ";";//无菌或涂片
                    }



                    sqlMain +=  "   end;";

                    helper.ExecuteNonQuery(sqlMain);
                    log.OperationSuccess = 1;
                    log.OperationContent = "上传二审数据到中间表成功！";

                }
                catch (Exception ex)
                {
                    log.OperationSuccess = 0;
                    log.OperationContent = "上传二审数据到中间表失败,详细信息： " + ex.Message;
                }
                result.Add(strPatId, log.OperationSuccess.Value == 0 ? "失败" : "成功");
                SaveSysInterfaceLog(log);
            }

            return result;
        }

        private EntitySampMain GetSampMainByBarcode(string repBarCode)
        {
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            EntitySampMain sam = dao?.GetSampMainSampleInfo(repBarCode);
            return sam;
        }


        /// <summary>
        /// 无菌或涂片结果插入中间表 smp_lis_result_bact 数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="patient"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Dictionary<string, OracleValue> GetSmpLisResultBact_Desc(EntityObrResultDesc item, EntityPidReportMain patient, int count)
        {
            Dictionary<String, OracleValue> parm = new Dictionary<string, OracleValue>();

            if (item == null)
                return parm;

            parm.Add("FRESULT_ID", new OracleValue(item.ObrId+"_"+count));//结果ID
            parm.Add("FREPORT_ID", new OracleValue(item.ObrId));//报告ID
            if(patient.MicReportFlag==1 && patient.RepStatus==0)//中期报告
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.MicReportSendUserID));//测试医生ID
                parm.Add("FTESTDR_NAME", new OracleValue(patient.MicReportSendUserName));//测试医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.MicReportChkUserID));//审核医生ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.MicReportChkUserName));//审核医生
                parm.Add("FLOGCDATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.MicReportDate, DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.MicReportDate, DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.MicReportDate, DataType.DateTime));//
            }
            else
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.RepAuditUserId));//测试医生ID  patient.RepCheckUserId
                parm.Add("FTESTDR_NAME", new OracleValue(patient.PidChkName));//测试医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.RepCheckUserId));//审核医生ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.BgName));//审核医生
                parm.Add("FLOGCDATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.RepInDate, DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.RepInDate, DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.RepInDate, DataType.DateTime));//
            }
            
            parm.Add("FBACT_ID", new OracleValue(""));//细菌ID
            parm.Add("FBACT_ENNAME", new OracleValue(""));//细菌英文名称
            parm.Add("FBACT_CNNAME", new OracleValue(""));//细菌中文名称
            parm.Add("FTEST_RESULT", new OracleValue(item.ObrValue?.Trim()));//检测结果
            parm.Add("FBACT_QUANTITY", new OracleValue(""));//菌落记数
            parm.Add("FTEST_BOARD_NAME", new OracleValue(""));//测试板
            parm.Add("FCULTURE_MEDIUM", new OracleValue(""));//培养基
            parm.Add("FCULTURE_DATE", new OracleValue(""));//培养日期
            parm.Add("FCULTURE_CONDITION", new OracleValue(""));//培养条件
            parm.Add("FFIND_METHOD", new OracleValue(""));//查找方法
            parm.Add("FSAMPLE_NO", new OracleValue(item.ObrSid));//样本号
            parm.Add("FTEST_ORDER", new OracleValue(""));//测试顺序
            parm.Add("FTESTBOARD", new OracleValue(""));//测量[测试]台
            parm.Add("FMACHINE", new OracleValue(item.ItrName));//测试仪器
            parm.Add("FADV_ID", new OracleValue(""));//
            
            parm.Add("FISAVAILABLE", new OracleValue("Y"));//是否有效的
            parm.Add("FORDERNUM", new OracleValue(count));//排序编码
            parm.Add("FRESULT_DESC", new OracleValue(item.ObrDescribe));//结果描述
            parm.Add("FVERSION", new OracleValue("1.00"));//版本号
            parm.Add("FLOGCBY", new OracleValue(patient.LrName));//
            parm.Add("FLOGLUBY", new OracleValue(patient.LrName));//
            
            return parm;
        }


        /// <summary>
        /// 检验报告插入中间表 smp_lis_report 数据
        /// </summary>
        /// <param name="pidResult"></param>
        /// <returns></returns>
        Dictionary<String, OracleValue> GetSmpLisReport(EntityQcResultList pidResult,EntityDCLReportParmeter dp)
        {
            Dictionary<String, OracleValue> parm = new Dictionary<string, OracleValue>();

            if (pidResult.patient == null || string.IsNullOrEmpty(pidResult.patient.RepId))
                return parm;
            List<EntitySampMain> sampMain = base.GetPidSample(pidResult.patient.RepBarCode);

            parm.Add("FREPORT_ID",new OracleValue(pidResult.patient.RepId));//报告ID
            parm.Add("FREPORT_NO", new OracleValue(pidResult.patient.RepId));//报告ID
            
            parm.Add("FPAT_ID", new OracleValue(pidResult.patient.HISPatientID));//病人ID
            parm.Add("FPAT_NO", new OracleValue(pidResult.patient.PidInNo));//病人ID
            parm.Add("FPAT_TYPE", new OracleValue(pidResult.patient.PidSrcId=="107"?"OP":
                pidResult.patient.PidSrcId == "108"?"IP":"TJ"));//病人类型（OP：门诊 IP: 住院）
            parm.Add("FPAT_HOSNO", new OracleValue(pidResult.patient.PidInNo));//03/27改传住院号
            parm.Add("FMEDCARD_NO", new OracleValue(pidResult.patient.PidSocialNo));// 就诊卡号（医疗卡号）

            string FVISIT_ID = pidResult.patient.PidSrcId == "108" ? "I" + pidResult.patient.HISSerialnum
                : "O" + pidResult.patient.HISSerialnum;
            
            parm.Add("FVISIT_ID", new OracleValue(FVISIT_ID));//诊疗卡号
            parm.Add("FVISIT_NO", new OracleValue(pidResult.patient.HISSerialnum));//诊疗卡号
            parm.Add("FPAT_NAME", new OracleValue(pidResult.patient.PidName));//病人姓名
            parm.Add("FPAT_SEX_ID", new OracleValue(pidResult.patient.PidSex));//年龄,1-男，2女
            parm.Add("FPAT_BDATE", new OracleValue(pidResult.patient.PidBirthday?.ToString("yyyy-MM-dd HH:mm:ss"),DataType.DateTime));//出生日期
            parm.Add("FIDTYPE_ID", new OracleValue(GetTJChargeCode(pidResult.patient.PidSrcId, pidResult.patient.RepBarCode)));//传入TJ合并收费代码
            parm.Add("FIDTYPE_NAME", new OracleValue(""));//ID类型名称
            parm.Add("FPAT_IDCARDNO", new OracleValue(pidResult.patient.PidIdentityCard));//身份证号

            
            string age = AgeConvertRule(pidResult.patient.PidAgeExp);
            //parm.Add("FPAT_AGE", new OracleValue( ));//年龄
            //parm.Add("FAGE_UNIT", new OracleValue( ));//年龄单位
            parm.Add("FFULL_AGE ", new OracleValue(age));//年龄（包含单位）

            parm.Add("FAPPNOTE_ID", new OracleValue(pidResult.patient.RepBarCode));//条码号
            parm.Add("FAPPNOTE_NO ", new OracleValue(""));

            //无条码时，茂名妇幼根据此字段收费
            parm.Add("FADV_ID", new OracleValue(dp?.PmaApplyID));


            parm.Add("FDIAGNOSIS", new OracleValue(pidResult.patient.PidDiag));//临床诊断
            parm.Add("FAPPDR_ID ", new OracleValue(pidResult.patient.PidDoctorCode));//申请医生工号
            parm.Add("FAPPDR_NAME", new OracleValue(pidResult.patient.PidDocName));//申请医生姓名
            parm.Add("FAPPDEPT_ID", new OracleValue(pidResult.patient.PidDeptId));//申请科室ID
            parm.Add("FAPPDEPT_NAME", new OracleValue(pidResult.patient.PidDeptName));//申请科室名称
            parm.Add("FWARD_ID", new OracleValue(pidResult.patient.PidDeptId));//申请科室ID
            parm.Add("FWARD_NAME", new OracleValue(pidResult.patient.PidDeptName));//申请科室名称
            parm.Add("FBED_NO", new OracleValue(pidResult.patient.PidBedNo));//床号
            parm.Add("FAPP_DATE", new OracleValue(pidResult.patient.SampReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),DataType.DateTime));//申请时间
            parm.Add("FCOLLECTION_DATE", new OracleValue(pidResult.patient.SampCollectionDate?.ToString("yyyy-MM-dd HH:mm:ss"),DataType.DateTime));//采集时间
            parm.Add("FSPECIMEN_ID", new OracleValue(pidResult.patient.PidSamId));//标本类型ID
            parm.Add("FSPECIMEN_NAME", new OracleValue(pidResult.patient.SamName));//标本类型名
            if (sampMain!= null && sampMain.Count > 0)
            {
                parm.Add("TUBE_ID", new OracleValue(sampMain[0].SampTubCode));//试管类型ID
                parm.Add("TUBE_NAME", new OracleValue(sampMain[0].TubName));//试管类型名
            }
            parm.Add("FIS_URGENCY", new OracleValue((pidResult.patient.RepUrgentFlag ?? 0).ToString() != "0" ?"Y":"N"));//危急值标志 -否，-是
            parm.Add("FSAMPLE_NO", new OracleValue(pidResult.patient.RepSid));//样本号
            
            parm.Add("FPRINT_DATE", new OracleValue(pidResult.patient.RepPrintDate?.ToString("yyyy-MM-dd HH:mm:ss"),DataType.DateTime));//打印日期

            string DitrSubTitle = "";
            if (pidResult.listResulto != null && pidResult.listResulto.Count > 0)
                DitrSubTitle = pidResult.listResulto[0].DitrSubTitle;
            else if (pidResult.listBact != null && pidResult.listBact.Count > 0)
                DitrSubTitle = pidResult.listBact[0].DitrSubTitle;
            else if (pidResult.listDesc != null && pidResult.listDesc.Count > 0)
                DitrSubTitle = pidResult.listDesc[0].DitrSubTitle;

            parm.Add("FREPORT_TYPE", new OracleValue(DitrSubTitle));//03/27改传子标题
            parm.Add("FREPORT_NAME", new OracleValue(pidResult.patient.PidComName));//03/27改传组合名称
            parm.Add("FHAVE_CRITICAL", new OracleValue("N"));// 是否存在临界值 ，-无，-有
            parm.Add("FCRITICAL_VALUES", new OracleValue(""));//临界值
            parm.Add("FISAVAILABLE", new OracleValue("Y"));//是可用的（有效）
            
            if (pidResult.patient.MicReportFlag == 1 && pidResult.patient.RepStatus == 0)//中期报告单

            {
                parm.Add("FREPORTDR_ID", new OracleValue(pidResult.patient.MicReportSendUserID));
                parm.Add("FREPORTDR_NAME", new OracleValue(pidResult.patient.MicReportSendUserName));
                parm.Add("FAUDITDR_ID", new OracleValue(pidResult.patient.MicReportChkUserID));
                parm.Add("FAUDITDR_NAME", new OracleValue(pidResult.patient.MicReportChkUserName));
                parm.Add("FREPORT_DESC", new OracleValue("此报告单为中期报告！"));
                parm.Add("FREPORT_DATE", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期

                parm.Add("FLOGCDATE", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FTEST_DATE", new OracleValue(pidResult.patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//检验日期
            }
            else
            {
                parm.Add("FREPORTDR_ID", new OracleValue(pidResult.patient.RepAuditUserId));
                parm.Add("FREPORTDR_NAME", new OracleValue(pidResult.patient.PidChkName));
                parm.Add("FAUDITDR_ID", new OracleValue(pidResult.patient.RepReportUserId));
                parm.Add("FAUDITDR_NAME", new OracleValue(pidResult.patient.BgName));
                parm.Add("FREPORT_DESC", new OracleValue(""));
                parm.Add("FREPORT_DATE", new OracleValue(pidResult.patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期

                parm.Add("FLOGCDATE", new OracleValue(pidResult.patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(pidResult.patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(pidResult.patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(pidResult.patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FTEST_DATE", new OracleValue(pidResult.patient.SampCheckDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//检验日期
            }
            
            parm.Add("FVERSION", new OracleValue("1.00"));//版本号
            parm.Add("FLOGCBY", new OracleValue(pidResult.patient.LrName));//
            
            parm.Add("FLOGLUBY", new OracleValue(pidResult.patient.LrName));//
            
            parm.Add("CHECK_FLAG", new OracleValue("A"));//
            parm.Add("FRECEIVE_DATE", new OracleValue(pidResult.patient.SampApplyDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//接收时间
            return parm;
        }

        private string AgeConvertRule(string pidAgeExp)
        {
            string strAge = pidAgeExp.ToLower();


            string strY = "0";
            string strM = "0";
            string strD = "0";
            string strH = "0";
            string strI = "0";

            if (strAge.IndexOf("y") > 0)
            {
                #region surfix y
                strY = strAge.Substring(0, strAge.IndexOf("y"));
                if (strAge.IndexOf("m") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (strAge.IndexOf("y") == 0 && strAge.Length > 0)
            {
                #region prefix y
                if (strAge.IndexOf("m") > 0)
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                            }
                            else
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                            }
                        }
                        else
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                        }
                    }
                    else
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                    }
                }
                else
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.Length - strAge.IndexOf("y") - 1);
                }
                #endregion

            }
            else if (strAge.IndexOf("m") > 0)
            {
                strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                if (strAge.IndexOf("d") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        }
                    }
                }
            }
            else if (strAge.IndexOf("m") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("d") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                        }
                        else
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                        }
                    }
                    else
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                    }
                }
                else
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                }
            }
            else if (strAge.IndexOf("d") > 0)
            {
                strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                if (strAge.IndexOf("h") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    }
                }
            }
            else if (strAge.IndexOf("d") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("h") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                    }
                    else
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                    }
                }
                else
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                }
            }
            else if (strAge.IndexOf("h") > 0)
            {
                strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                if (strAge.IndexOf("i") > 0)
                {
                    strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("h") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("i") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                }
                else
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("i") > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
            }
            else if (strAge.IndexOf("i") == 0 && strAge.Length > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
            }
            else
            {
                return string.Empty;
            }

            string strOutAge = string.Empty;
            try
            {
                strY = Convert.ToInt32(Convert.ToDecimal(strY)).ToString();
                strM = Convert.ToInt32(Convert.ToDecimal(strM)).ToString();
                strD = Convert.ToInt32(Convert.ToDecimal(strD)).ToString();
                strH = Convert.ToInt32(Convert.ToDecimal(strH)).ToString();
                strI = Convert.ToInt32(Convert.ToDecimal(strI)).ToString();
                if (int.Parse(strY) > 0)
                    return strY + "Y";
                else if (int.Parse(strM) > 0)
                    return strM + "M";
                else if (int.Parse(strD) > 0)
                    return strD + "D";
                else
                    return strH + "H" + strI + "I";
            }
            catch
            {
                return "";
            }
        }

        private object GetTJChargeCode(string PidSrcId,string repBarCode)
        {
            if(PidSrcId == "109")
            {
                EntitySampMain Sma = GetSampMainByBarcode(repBarCode);
                return Sma?.DcomHisCodes;
            }
            return "";
        }

        Dictionary<String, OracleValue> GetSmpLisResult(EntityObrResult obrResult, EntityPidReportMain patient,int count)
        {
            Dictionary<String, OracleValue> parm = new Dictionary<string, OracleValue>();

            if (obrResult == null)
                return parm;

            parm.Add("FRESULT_ID", new OracleValue(obrResult.ObrSn+"_"+count));//结果ID
            parm.Add("FREPORT_ID", new OracleValue(obrResult.ObrId));//报告ID
            parm.Add("FREPORT_DATE", new OracleValue(obrResult.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"),DataType.DateTime));//报告日期
            parm.Add("FLISITEM_ID", new OracleValue(obrResult.ItmId?.Trim()));//检验项目英文名称
            parm.Add("FLISITEM_ENNAME", new OracleValue(obrResult.ItmEname?.Trim()));//检验项目英文名称
            parm.Add("FLISITEM_CNNAME", new OracleValue(obrResult.ItmName.Trim()));//检验项目中文名称
            parm.Add("FTEST_RESULT", new OracleValue(obrResult.ObrValue));//检验结果
            parm.Add("FLISITEM_REFERENCE", new OracleValue(obrResult.RefMore.Length > 0 ? obrResult.RefMore.Replace("*", " ") : obrResult.ResRefRange));//检验项目参考范围
            parm.Add("FLISTITEM_UNIT", new OracleValue(obrResult.ObrUnit));//结果单位
            parm.Add("FRESULT_NOTE", new OracleValue(obrResult.ResTips));//结果提示
            parm.Add("FHAVE_CRITICAL", new OracleValue("N"));//是否存在临界值
            parm.Add("FCRITICAL_VALUES", new OracleValue(""));//临界值
            parm.Add("FRESULT_TYPE", new OracleValue(obrResult.ObrConvertValue==null?"QN":"NAR"));//结果类型（NAR：精确  QN ：非精确）
            parm.Add("FSAMPLE_NO", new OracleValue(obrResult.ObrSid));//样本号
            parm.Add("FMACHINE", new OracleValue(obrResult.ItrEname));//测试仪器
            parm.Add("FTEST_METHOD", new OracleValue(obrResult.ObrItmMethod));//测试方法
            parm.Add("FADV_ID", new OracleValue(""));//
            parm.Add("FTESTDR_ID", new OracleValue(patient.RepCheckUserId));//检验者工号
            parm.Add("FTESTDR_NAME", new OracleValue(patient.LrName));//检验者名称
            parm.Add("FAUDITDR_ID", new OracleValue(patient.RepAuditUserId));//审核医生ID
            parm.Add("FAUDITDR_NAME", new OracleValue(patient.PidChkName));//审核医生
            parm.Add("FISAVAILABLE", new OracleValue("Y"));//是否有效的
            parm.Add("FORDERNUM", new OracleValue(count));//排序号
            parm.Add("FRESULT_DESC", new OracleValue(patient.RepDiscribe));//结果描述
            parm.Add("FVERSION", new OracleValue("1.00"));//版本号
            parm.Add("FLOGCBY", new OracleValue(patient.LrName));//
            parm.Add("FLOGCDATE", new OracleValue(obrResult.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
            parm.Add("FLOGLUBY", new OracleValue(patient.LrName));//
            parm.Add("FLOGLUDATE", new OracleValue(patient.RepInDate,DataType.DateTime));//
            parm.Add("FLOGLABY", new OracleValue(patient.RepInDate,DataType.DateTime));//
            parm.Add("FLOGLADATE", new OracleValue(patient.RepInDate,DataType.DateTime));//
            return parm;
        }

        Dictionary<String, OracleValue> GetSmpLisResultBact(EntityObrResultBact obrResultBact ,EntityPidReportMain patient,int count)
        {
            Dictionary<String, OracleValue> parm = new Dictionary<string, OracleValue>();

            IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();

            string title = string.Empty;
            var eva = dao.Search(null).Find(a => a.EvaContent == obrResultBact.ObrRemark);
            if(eva != null)
            {
                title = eva.EvaTitle;
            }

            if (obrResultBact == null)
                return parm;

            parm.Add("FRESULT_ID", new OracleValue(obrResultBact.ObrId +"_"+count));//结果ID
            parm.Add("FREPORT_ID", new OracleValue(obrResultBact.ObrId));//报告ID
            if(patient.MicReportFlag==1 && patient.RepStatus==0) //中期报告
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.MicReportSendUserID));//测试医生ID
                parm.Add("FTESTDR_NAME", new OracleValue(patient.MicReportSendUserName));//测试医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.MicReportChkUserID));//审核医生ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.MicReportChkUserName));//审核医生
                parm.Add("FLOGCDATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
            }
            else
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.RepCheckUserId));//测试医生ID
                parm.Add("FTESTDR_NAME", new OracleValue(patient.UserName));//测试医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.RepAuditUserId));//审核医生ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.PidChkName));//审核医生
                parm.Add("FLOGCDATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
            }
            
            parm.Add("FBACT_ID", new OracleValue(obrResultBact.ObrBacId));//细菌ID
            parm.Add("FBACT_ENNAME", new OracleValue(obrResultBact.BacEname));//细菌英文名称
            parm.Add("FBACT_CNNAME", new OracleValue(obrResultBact.BacCname));//细菌中文名称
            parm.Add("FTEST_RESULT", new OracleValue(obrResultBact.ObrRemark));//检测结果
            parm.Add("FTEST_RESULT_TITLE", new OracleValue(title)); //检测结果标题 
            parm.Add("FBACT_QUANTITY", new OracleValue(obrResultBact.ObrColonyCount));//菌落记数
            parm.Add("FTEST_BOARD_NAME", new OracleValue(""));//测试板
            parm.Add("FCULTURE_MEDIUM", new OracleValue(""));//培养基
            parm.Add("FCULTURE_DATE", new OracleValue(""));//培养日期
            parm.Add("FCULTURE_CONDITION", new OracleValue(""));//培养条件
            parm.Add("FFIND_METHOD", new OracleValue(""));//查找方法
            parm.Add("FSAMPLE_NO", new OracleValue(obrResultBact.ObrSid));//样本号
            parm.Add("FTEST_ORDER", new OracleValue(""));//测试顺序
            parm.Add("FTESTBOARD", new OracleValue(""));//测量[测试]台
            parm.Add("FMACHINE", new OracleValue(obrResultBact.ItrName));//测试仪器
            parm.Add("FADV_ID", new OracleValue(""));//
            
            parm.Add("FISAVAILABLE", new OracleValue("Y"));//是否有效的
            parm.Add("FORDERNUM", new OracleValue(count));//排序编码
            parm.Add("FRESULT_DESC", new OracleValue(patient.RepDiscribe));//结果描述
            parm.Add("FVERSION", new OracleValue("1.00"));//版本号
            parm.Add("FLOGCBY", new OracleValue(patient.LrName));//
            
            parm.Add("FLOGLUBY", new OracleValue(patient.LrName));//
            
            return parm;
        }

        Dictionary<String, OracleValue> GetSmpLisResultAllerg(EntityObrResultAnti obrResultAnti, EntityPidReportMain patient,int count)
        {
            Dictionary<String, OracleValue> parm = new Dictionary<string, OracleValue>();
            if (obrResultAnti == null)
                return parm;

            parm.Add("FRESULT_ID", new OracleValue(obrResultAnti.ObrId+"_"+count));//结果ID
            parm.Add("FREPORT_ID", new OracleValue(obrResultAnti.ObrId));//报告ID
            if(patient.MicReportFlag==1 && patient.RepStatus==0)
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.MicReportSendUserID));//检验医生ID
                parm.Add("FTESTDR_NAME", new OracleValue(patient.MicReportSendUserName));//检验医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.MicReportChkUserID));//审核人ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.MicReportChkUserName));//审核人
                parm.Add("FLOGCDATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.MicReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
            }
            else
            {
                parm.Add("FREPORT_DATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//报告日期
                parm.Add("FTESTDR_ID", new OracleValue(patient.RepCheckUserId));//检验医生ID
                parm.Add("FTESTDR_NAME", new OracleValue(patient.UserName));//检验医生
                parm.Add("FAUDITDR_ID", new OracleValue(patient.RepAuditUserId));//审核人ID
                parm.Add("FAUDITDR_NAME", new OracleValue(patient.PidChkName));//审核人
                parm.Add("FLOGCDATE", new OracleValue(patient.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLUDATE", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLABY", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
                parm.Add("FLOGLADATE", new OracleValue(patient.RepInDate?.ToString("yyyy-MM-dd HH:mm:ss"), DataType.DateTime));//
            }
            
            parm.Add("FBACT_ID", new OracleValue(obrResultAnti.ObrBacId));//细菌ID
            parm.Add("FALLERGY_ID", new OracleValue(obrResultAnti.ObrAntId));//抗生素ID
            parm.Add("FALLERGY_ENNAME", new OracleValue(obrResultAnti.AntEname));//抗生素英文名称
            parm.Add("FALLERGY_CNNAME", new OracleValue(obrResultAnti.AntCname));//抗生素中文名称
            parm.Add("FTEST_RESULT", new OracleValue(obrResultAnti.ObrValue));//测试结果
            parm.Add("FYJND", new OracleValue(obrResultAnti.ObrValue2));//MIC值
            parm.Add("FYJHZJ", new OracleValue(""));//
            parm.Add("FZPHYL", new OracleValue(""));//
            parm.Add("FSAMPLE_NO", new OracleValue(obrResultAnti.ObrSid));//样本号
            parm.Add("FMACHINE", new OracleValue(obrResultAnti.ItrName));//检测仪器
            parm.Add("FTEST_METHOD", new OracleValue(obrResultAnti.ObrAntMethod));//检测方法
            parm.Add("FADV_ID", new OracleValue(""));//
            
            parm.Add("FISAVAILABLE", new OracleValue("Y"));//是有效吗
            parm.Add("FORDERNUM", new OracleValue(count));//排序号
            parm.Add("FRESULT_DESC", new OracleValue(patient.RepDiscribe));//结果描述
            parm.Add("FVERSION", new OracleValue("1.00"));//版本号
            parm.Add("FLOGCBY", new OracleValue(patient.LrName));//
            
            parm.Add("FLOGLUBY", new OracleValue(patient.LrName));//
            
            return parm;
        }

        /// <summary>
        /// 取消报告（取消二审）
        /// </summary>
        /// <param name="listRepId"></param>
        /// <returns></returns>
        internal override NameValueCollection UndoUploadDCLReport(List<string> listRepId)
        {
            NameValueCollection result = new NameValueCollection();
            EntitySysInterfaceLog log = new EntitySysInterfaceLog();

            #region 获取连接串

            CRUDDataInterfaceConnection biz = new CRUDDataInterfaceConnection();
            List<EntityDictDataInterfaceConnection> list = biz.SelectAll("lis中间库");
            if (list == null || list.Count <= 0)
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "获取中间表连接串失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "获取中间表连接串失败";
                SaveSysInterfaceLog(log);
                return result;
            }

            #endregion

            #region 测试连接串

            string msg;
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(list[0]);
            conn.TestConnection(out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                log = new EntitySysInterfaceLog();
                log.OperationName = "中间表连接串测试连接操作失败！";
                log.OperationTime = DateTime.Now;
                log.OperationSuccess = 0;
                log.OperationContent = "中间表连接串测试连接操作失败";
                SaveSysInterfaceLog(log);
                return result;
            }

            #endregion

            SqlHelper helper = conn.GetSqlHelper();
            //IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            string sqlDeleteReport = "delete from smd.smp_lis_report t where t.FREPORT_NO = '{0}'; ";
            string sqlDeleteResult = "delete from {0} where FREPORT_ID='{1}';";

            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();

            List<EntityDCLReportParmeter> DCLParms = dao.GetDCLParmeter(listRepId);

            string sqlMain = string.Empty;
            foreach (string strPatId in listRepId)
            {
                try
                {
                    EntityDCLReportParmeter _pp = DCLParms.Find(w=>w.PmaRepID==strPatId);

                    log = new EntitySysInterfaceLog();
                    log.SampBarId = _pp?.BarCode;
                    log.RepId = strPatId;
                    log.OperationName = "反审删除中间表数据";
                    log.OperationTime = DateTime.Now;
                    log.OperationSuccess = 0;
                    log.OperationContent = "";

                    sqlMain = "begin   ";
                    sqlMain += string.Format(sqlDeleteReport, strPatId);

                    //EntityQcResultList pidResult = base.GetPidResult(strPatId);

                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result", strPatId);//报告从表

                    
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//细菌（微生物）报告

                   
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_allergy", strPatId);//药敏报告

                    
                    sqlMain += string.Format(sqlDeleteResult, "smd.smp_lis_result_bact", strPatId);//药敏报告

                    sqlMain += "    end;";

                    int excueFlag = helper.ExecuteNonQuery(sqlMain);
                    log.OperationSuccess = 1;
                    log.OperationContent = "反审删除中间表数据成功！";
                }
                catch (Exception ex)
                {
                    log.OperationSuccess = 0;
                    log.OperationContent = "反审删除中间表数据失败,详细信息： " + ex.Message;
                }
                SaveSysInterfaceLog(log);
                result.Add(strPatId, log.OperationSuccess.Value == 0 ? "失败" : "成功");
            }
            return result;
        }

        #endregion

        #region 根据门诊ID生成报告单信息
        public override List<EntityPidReportMain> GetMzPatients(EntityInterfaceExtParameter Parameter)
        {
            MZReportCreater Helper = new MZReportCreater();
            List<EntityPidReportMain> result = new List<EntityPidReportMain>();

            try
            {
                //先获取条码信息
                List<EntityInterfaceData> listInterfaceData = DownloadInterfaceData(Parameter);
                List<EntitySampMain> Barcodes = Helper.GetMzBarcodeInfo(listInterfaceData, Parameter);
                if (Barcodes.Count > 0)
                {
                    //对每个条码信息进行提取，生成报告信息
                    foreach (EntitySampMain Barcode in Barcodes)
                    {
                        EntityPidReportMain Report = Helper.CreateReport(Barcode);
                        result.Add(Report);
                    }

                }
                return result;
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException("生成门诊报告信息失败",ex);
                result = new List<EntityPidReportMain>();
            }
            return result;
        }

        private void FiltByOrder(List<EntitySampMain> barcodes)
        {
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();

            for (int i = 0; i < barcodes.Count; i++)
            {
                EntitySampMain ri = barcodes[i];
                for (int y = 0; y < ri.ListSampDetail.Count; y++)
                {
                    EntitySampDetail dy = ri.ListSampDetail[y];
                    if(detailDao.IsExistedOrder(dy.OrderSn))
                    {
                        ri.ListSampDetail.Remove(dy);
                    }
                }
                if (ri.ListSampDetail.Count == 0)
                {
                    barcodes.Remove(ri);
                }
            }
        }

       
        #endregion

    }
}
