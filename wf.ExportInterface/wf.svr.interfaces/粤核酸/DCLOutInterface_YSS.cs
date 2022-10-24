using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.svr.interfaces
{
    internal class YssResponse
    {
        public string statusCode { get; set; }
        public string error { get; set; }
        public string requestId { get; set; }
        public ResultData[] resultData { get; set; }
    }

    internal class ResultData
    {
        public string personName { get; set; }//姓名
        public string age { get; set; }//年龄
        public string phoneNumber { get; set; }//电话号码
        public string sampleOrhName { get; set; }//采样机构名称
        public string sampleDate { get; set; }//采样日期
        public string detectionOrgName { get; set; }//检测机构名称
        public string detectionDate { get; set; }//检测日期
        public string detectionResult { get; set; }//检测结果
        public string detectionApplicantDate { get; set; }//检测结果填报时间
        public string gender { get; set; }//性别
        public string nationality { get; set; }//国家或地区
        public string householdDivision { get; set; }//户籍地
        public string livingAddress { get; set; }//居住地
        public string identityType { get; set; }//证件类型名称
        public string identityNumber { get; set; }//证件号码
        public string personSource { get; set; }//人员来源
        public string personIdentity { get; set; }//人员身份
        public string overseasPersonnel { get; set; }//14 天内境外入境人员
        public string insuranceType { get; set; }//参保类型
        public string insuranceLocation { get; set; }//参保地
        public string insuranceCategory { get; set; }//险种
        public string sampleBarcode { get; set; }//样本条形码
        public string sampleType { get; set; }//样本类型
        public string detectionItem { get; set; }//检测项目
        public string paymentType { get; set; }//结算类型
        public string sampleLocationDivision { get; set; }//采样点行政区划
        public string sampleLocation { get; set; }//采用地点
        public string sampleLocationUnit { get; set; }//所在学校或单位名称
        public string createUserCn { get; set; }//创建人姓名
        public string remark1 { get; set; }//备注1
        public string remark2 { get; set; }//备注2
        public string collectType { get; set; }//采集类型
        public string birthDate { get; set; }//出生日期
        public string otherTypeDesc { get; set; }//未提供有效证件原因
        public string detectionType { get; set; }//检测人群分类
        public string shouldDetectionType { get; set; }//应检尽检类别

    }

    internal partial class DCLExtInterface_MMFY : DCLExtInterfaceBase
    {
        string authUrl = ConfigurationManager.AppSettings["YSS_AuthUrl"];
        string validateUrl = ConfigurationManager.AppSettings["YSS_ValidateAuthUrl"];
        string password = ConfigurationManager.AppSettings["YSS_PassWord"];
        string username = ConfigurationManager.AppSettings["YSS_UserName"];
        string resultUrl = ConfigurationManager.AppSettings["YSS_UploadResultUrl"];
        string getsampleinfo = ConfigurationManager.AppSettings["YSS_SampleInfoUrl"];
        string ticket = CacheSysConfig.Current.GetSystemConfig("YSS_Ticket");

        string doctorCode = CacheSysConfig.Current.GetSystemConfig("YSS_Doctor");//开单医生

        public bool CheckAuth()
        {
            try
            {
                var checkTicket = new
                {
                    Authorization = ticket
                };
                string url = validateUrl;
                var postData = Newtonsoft.Json.JsonConvert.SerializeObject(checkTicket);
                YssResponse response = new WebApiAgent().Invoke(url, postData, true, out ticket, false, ticket);
                if ("000000".Equals(response.statusCode))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public string Authorization()
        {
            try
            {
                var user = new
                {
                    username = username,
                    password = password
                };
                string url = authUrl;
                var postData = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                YssResponse response = new WebApiAgent().Invoke(url, postData, false, out ticket, true, ticket);
                if ("000000".Equals(response.statusCode) && !string.IsNullOrWhiteSpace(ticket))
                {
                    UpdateTicket(ticket);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return ticket;
        }

        public bool UpdateTicket(string ticket)
        {
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                try
                {
                    EntitySysParameter item = null;
                    item = (dao.GetSysParaByConfigCode("YSS_Ticket")).FirstOrDefault();
                    if (item != null)
                    {
                        item.ParmFieldValue = ticket;
                        dao.UpdateSysParaByConfigId(item);
                        dcl.svr.cache.CacheSysConfig.Current.Refresh();//刷新系统配置表服务器缓存 SJC 2017-12-21
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return false;
        }

        internal override bool UploadYssReport(List<string> listRepId)
        {
            //if (!CheckAuth())
            //{
            //    Authorization();
            //}
            bool res = false;
            EntitySysInterfaceLog log;
            string url = resultUrl;
            var daoPidMain = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            var daoSamDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            foreach (string strPatId in listRepId)
            {
                Lib.LogManager.Logger.LogInfo("发布粤省事检验报告：报告单号：" + strPatId);
                var patMain = daoPidMain.GetPatientInfo(strPatId);
                var pidDetails = daoSamDetail.GetSampDetail(patMain.RepBarCode);
                if (patMain.RepBarCode.Length > 14)
                {
                    Lib.LogManager.Logger.LogInfo("发布粤省事检验报告失败：条码为粤核酸子条码无需上传，条码号为：" + patMain.RepBarCode);
                    continue;
                }
                if (pidDetails.Count == 0)
                {
                    Lib.LogManager.Logger.LogInfo("发布粤省事检验报告失败：查不到该报告条码明细信息，报告单号：" + strPatId);
                    continue;
                }
                log = new EntitySysInterfaceLog();
                log.SampBarId = pidDetails[0].SampBarId;
                log.RepId = strPatId;
                log.OperationTime = DateTime.Now;
                log.OperationName = "发布粤省事检验报告";
                bool positive = false;
                EntityQcResultList pidResult = base.GetPidResult(strPatId);
                foreach (var result in pidResult.listResulto)
                {
                    if (result.ObrValue.Contains("+") || result.ObrValue.Contains("阳"))
                    {
                        positive = true;
                        break;
                    }
                }
                var report = new
                {
                    sampleBarcode = patMain.RepBarCode,
                    detectionDate = patMain.RepReportDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    detectionResult = positive ? 3 : 1
                };
                var postData = Newtonsoft.Json.JsonConvert.SerializeObject(report);
                res = Invoke(postData, url, log);
                //Thread.Sleep(1000*60*5+1000); 
            }
            return res;
        }

        internal override bool UndoUploadYssReport(List<string> listRepId)
        {
            bool res = false;
            //EntitySysInterfaceLog log;
            //string url = resultUrl;
            //var daoPidMain = DaoFactory.DaoHandler<IDaoPidReportMain>();
            //var daoSamDetail = DaoFactory.DaoHandler<IDaoSampDetail>();
            //foreach (string strPatId in listRepId)
            //{
            //    var patMain = daoPidMain.GetPatientInfo(strPatId);
            //    log = new EntitySysInterfaceLog();
            //    log.SampBarId = patMain.RepBarCode;
            //    log.RepId = strPatId;
            //    log.OperationTime = DateTime.Now;
            //    log.OperationName = "发布HIS检验报告";
            //    var report = new
            //    {
            //        sampleBarcode = patMain.RepBarCode,
            //        detectionDate = patMain.RepInDate,
            //        detectionResult = 4
            //    };
            //    var postData = Newtonsoft.Json.JsonConvert.SerializeObject(report);
            //    res = Invoke(postData, url, log);
            //}
            return res;
        }

        internal override List<EntitySampMain> GetYssPatientInfoBase(List<string> SampBarId)
        {
            bool res = false;

            List<EntitySampMain> listSampMain = new List<EntitySampMain>();

            EntitySampQC sampQC = new EntitySampQC();
            sampQC.ListSampBarId = SampBarId;
            //sampQC.SampYhsBarCode = SampBarId.FirstOrDefault();
            var daoSamMain = DclDaoFactory.DaoHandler<IDaoSampMain>();
            //通过身份证、姓名、项目、条码状态查询患者在lis中对应的条码信息
            listSampMain = daoSamMain.GetSampMain(sampQC);

            if (listSampMain != null && listSampMain.Count > 0)
            {
                return listSampMain;
            }

            EntitySysInterfaceLog log;
            string url = getsampleinfo;
            foreach (string sampBarId in SampBarId)
            {            
                Lib.LogManager.Logger.LogInfo("获取采样人员信息，总条码：：" + sampBarId);
                log = new EntitySysInterfaceLog();
                log.SampBarId = sampBarId;
                log.RepId = "";
                log.OperationTime = DateTime.Now;
                log.OperationName = "获取粤核酸总码的人员信息";
                var SampBarInfo = new
                {
                    sampleBarcode = sampBarId,
                    identityNumber = "",
                    detectionOrgName = "茂名市妇幼保健院"
                };
                var postData = Newtonsoft.Json.JsonConvert.SerializeObject(SampBarInfo);

                listSampMain = InvokePatientInfo(postData, url, sampBarId, log);
            }
            return listSampMain;
        }

        internal bool Invoke(string postData, string url, EntitySysInterfaceLog log)
        {
            bool res;
            try
            {
                YssResponse response = new WebApiAgent().Invoke(url, postData, true, out ticket, false, ticket);
                if (!"000000".Equals(response.statusCode))
                {
                    if ("000102".Equals(response.statusCode))
                    {
                        Authorization();
                        response = new WebApiAgent().sendDatePost(url, postData, null, ticket);
                        if (!"000000".Equals(response.statusCode))
                        {
                            throw new Exception(response.requestId + response.error + postData);
                        }
                    }
                    else
                    {
                        throw new Exception(response.requestId + response.error + postData);
                    }
                }
                log.OperationSuccess = 1;
                log.OperationContent = "上传粤省事检验报告成功:" + postData;
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Lib.LogManager.Logger.LogException(ex);
                log.OperationSuccess = 0;
                log.OperationContent = "上传粤省事检验报告出错:" + ex.Message + postData;
            }
            finally
            {
                Lib.LogManager.Logger.LogInfo(log.OperationContent);
                SaveSysInterfaceLog(log);
            }
            return res;
        }

        internal List<EntitySampMain> InvokePatientInfo(string postData, string url, string SampBarId, EntitySysInterfaceLog log)
        {
            bool res;

            List<EntitySampMain> listSampMain = new List<EntitySampMain>();

            EntitySampMain entitySampMain = new EntitySampMain();
            EntitySampDetail entitySampDetail = new EntitySampDetail();
            EntitySampProcessDetail entitySampProcessDetail = new EntitySampProcessDetail();

            EntitySampQC sampQC = new EntitySampQC();

            var daoSamMain = DclDaoFactory.DaoHandler<IDaoSampMain>();

            ticket = CacheSysConfig.Current.GetSystemConfig("YSS_Ticket");

            try
            {

                YssResponse response = new WebApiAgent().Invoke(url, postData, true, out ticket, false, ticket);
                if (!"000000".Equals(response.statusCode))
                {
                    if ("000102".Equals(response.statusCode))
                    {
                        Authorization();
                        response = new WebApiAgent().GetYssPratintInfo(url, postData, null, ticket);
                        if (!"000000".Equals(response.statusCode))
                        {
                            throw new Exception(response.requestId + response.error + postData);
                        }
                    }
                    else
                    {
                        throw new Exception(response.requestId + "||" + response.statusCode + "||" + response.error + postData);
                    }
                }


                //生成总条码
                entitySampMain.PidOrgId = ConfigurationManager.AppSettings["HospitalId"];
                entitySampMain.SampBarId = SampBarId;
                entitySampMain.SampBarCode = SampBarId;
                entitySampMain.PidName = SampBarId;
                entitySampMain.PidInNo = SampBarId;
                entitySampMain.PidAge = "01";
                entitySampMain.PidTel = "";
                //entitySampMain.CollectionDate = System.DateTime.Now;
                //entitySampMain.ReceiverDate = System.DateTime.Now;
                entitySampMain.PidSex = "1";
                entitySampMain.PidIdtId = "111";
                entitySampMain.PidSrcId = "107";
                entitySampMain.PidSrcName = "门诊";
                entitySampMain.PidDoctorCode = "0434";
                entitySampMain.PidIdentityName = "居民身份证";
                entitySampMain.PidIdentityCard = "SampBarId";
                entitySampMain.SampSamId = "4";
                entitySampMain.SampSamName = "咽拭子";
                entitySampMain.PidDeptCode = "1901";
                entitySampMain.PidDeptName = "河东门诊部(妇)";
                entitySampMain.SampComName = "新型冠状病毒RNA测定（混检）";
                entitySampMain.SampPrintTime = 1;
                entitySampMain.SampBarType = 0;
                entitySampMain.SampYhsBarCode = SampBarId;
                entitySampMain.CollectionDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                entitySampMain.SendDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                entitySampMain.ReceiverDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                entitySampMain.ReachDate = cache.ServerDateTime.GetDatabaseServerDateTime();

                entitySampDetail.SampBarId = SampBarId;
                entitySampDetail.SampBarCode = SampBarId;
                entitySampDetail.OrderName = "新型冠状病毒RNA测定（混合检测）";
                entitySampDetail.OrderCode = "250403089S-2/3";
                entitySampDetail.ComId = "100224";
                entitySampDetail.ComName = "新型冠状病毒RNA测定（混检）";
                entitySampDetail.SampDate = cache.ServerDateTime.GetDatabaseServerDateTime();              

                entitySampProcessDetail.ProcBarcode = SampBarId;
                entitySampProcessDetail.ProcBarno = SampBarId;
                entitySampProcessDetail.ProcDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                entitySampProcessDetail.ProcUsercode = "0434";
                entitySampProcessDetail.ProcUsername = "温嫦莉";
                entitySampProcessDetail.ProcStatus = "5";
                entitySampProcessDetail.ProcTimes = 1;
                entitySampProcessDetail.ProcContent = "签收";
                entitySampProcessDetail.ProcPlace = "无";

                entitySampMain.ListSampDetail.Add(entitySampDetail);
                entitySampMain.ListSampProcessDetail.Add(entitySampProcessDetail);

                listSampMain.Add(entitySampMain);

                //子条码
                if (response.resultData.Length > 0)
                {
                    int i = 1;
                    foreach (var patient in response.resultData)
                    {
                        entitySampMain = new EntitySampMain();
                        entitySampDetail = new EntitySampDetail();
                        entitySampProcessDetail = new EntitySampProcessDetail();

                        //sampQC.PidIdentityCard = patient.identityNumber;
                        //sampQC.PidName = patient.personName;
                        //sampQC.ComId = "100224";
                        //sampQC.ListSampStatusId.Add("1");//打印状态

                        ////通过身份证、姓名、项目、条码状态查询患者在lis中对应的条码信息
                        //entitySampMain = daoSamMain.GetSampMain(sampQC).FirstOrDefault();

                        string ii = i.ToString("00");
                        entitySampMain.SampBarId = SampBarId + ii;
                        entitySampMain.SampBarCode = SampBarId + ii;
                        entitySampMain.PidOrgId = ConfigurationManager.AppSettings["HospitalId"];
                        entitySampMain.PidInNo = patient.identityNumber;
                        entitySampMain.PidName = patient.personName;
                        if (patient.age == "0")
                        {
                            entitySampMain.PidAge = "";
                        }
                        else
                        {
                            entitySampMain.PidAge = patient.age;
                        }
                        entitySampMain.PidTel = patient.phoneNumber;
                        //entitySampMain.CollectionDate = Convert.ToDateTime(patient.sampleDate);
                        //entitySampMain.ReceiverDate = Convert.ToDateTime(patient.detectionDate);
                        if (patient.gender == "男")
                            entitySampMain.PidSex = "1";
                        else if (patient.gender == "女")
                            entitySampMain.PidSex = "2";
                        else
                            entitySampMain.PidSex = "0";
                        entitySampMain.SampRegistyAddress = patient.householdDivision;
                        entitySampMain.PidAddress = patient.livingAddress;

                        if(patient.identityNumber.Contains("H"))
                        {
                            entitySampMain.PidIdentityName = "港澳通行证";
                        }
                        else if (patient.identityNumber.Contains("M") || patient.identityNumber.Contains("F") || patient.identityNumber.Contains("m"))
                        {
                            entitySampMain.PidIdentityName = "其他";
                        }
                        else
                        {
                            entitySampMain.PidIdentityName = patient.identityType;
                        }
                        if (patient.identityType.Contains("通行证"))
                        {
                            entitySampMain.PidIdentityName = "港澳台通行证";
                        }
                        entitySampMain.PidIdentityCard = patient.identityNumber;
                        if (!string.IsNullOrEmpty(patient.birthDate))
                        {
                            entitySampMain.PidBirthday = Convert.ToDateTime(patient.birthDate);
                        }
                        entitySampMain.SampSamId = "4";
                        entitySampMain.SampSamName = "咽拭子";
                        entitySampMain.PidDeptCode = "1901";
                        entitySampMain.PidIdtId = "111";
                        entitySampMain.PidSrcId = "107";
                        entitySampMain.PidSrcName = "门诊";
                        entitySampMain.PidDoctorCode = "0434";
                        entitySampMain.PidDeptCode = "1901";
                        entitySampMain.PidDeptName = "河东门诊部(妇)";
                        entitySampMain.SampYhsBarCode = SampBarId;
                        if (patient.collectType.Contains("单采"))
                            entitySampMain.SampComName = "新型冠状病毒RNA测定（单样检测）";
                        else
                            entitySampMain.SampComName = "新型冠状病毒RNA测定（混检）";
                        entitySampMain.SampPrintTime = 1;
                        entitySampMain.SampBarType = 0;
                        entitySampMain.SampOccDate = Convert.ToDateTime(patient.sampleDate);
                        entitySampMain.CollectionDate = Convert.ToDateTime(patient.sampleDate);
                        entitySampMain.SendDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                        entitySampMain.ReceiverDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                        entitySampMain.ReachDate = cache.ServerDateTime.GetDatabaseServerDateTime();

                        entitySampMain.PidDiag = "体检";
                        entitySampMain.SendUserId = "0434";
                        entitySampMain.SendUserName = "温嫦莉";

                        entitySampDetail.SampBarId = SampBarId + ii;
                        entitySampDetail.SampBarCode = SampBarId + ii;
                        if (patient.collectType.Contains("单采"))
                        {
                            entitySampDetail.OrderName = "新型冠状病毒RNA测定（单样检测）";
                            entitySampDetail.OrderCode = "250403089S-2/3";
                            entitySampDetail.ComId = "100223";
                            entitySampDetail.ComName = "新型冠状病毒RNA测定（单样检测）";
                        }
                        else
                        {
                            entitySampDetail.OrderName = "新型冠状病毒RNA测定（混合检测）";
                            entitySampDetail.OrderCode = "250403089S-2/3";
                            entitySampDetail.ComId = "100224";
                            entitySampDetail.ComName = "新型冠状病毒RNA测定（混检）";
                        }
                           
                        entitySampDetail.SampDate = Convert.ToDateTime(patient.sampleDate);

                        entitySampProcessDetail.ProcBarcode = SampBarId + ii;
                        entitySampProcessDetail.ProcBarno = SampBarId + ii;
                        entitySampProcessDetail.ProcDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                        entitySampProcessDetail.ProcUsercode = "0434";
                        entitySampProcessDetail.ProcUsername = "温嫦莉";
                        entitySampProcessDetail.ProcStatus = "5";
                        entitySampProcessDetail.ProcTimes = 1;
                        entitySampProcessDetail.ProcContent = "签收";
                        entitySampProcessDetail.ProcPlace = "无";

                        entitySampMain.ListSampDetail.Add(entitySampDetail);
                        entitySampMain.ListSampProcessDetail.Add(entitySampProcessDetail);

                        listSampMain.Add(entitySampMain);

                        i++;                     
                    }
                }
                else
                {
                    res = false;
                    log.OperationTime = DateTime.Now;
                    log.OperationName = "获取采样人员信息";
                    log.OperationSuccess = 1;
                    log.OperationContent = "获取采样信息后总码明细信息缺少:" + postData;
                }

                if (!daoSamMain.CreateSampMain(listSampMain))
                {
                    res = false;
                    log.OperationTime = DateTime.Now;
                    log.OperationName = "生成总条码信息";
                    log.OperationSuccess = 0;
                    log.OperationContent = "获取采样信息后生成条码信息失败！总条码：" + SampBarId;
                }

                log.OperationTime = DateTime.Now;
                log.OperationName = "获取采样人员信息";
                log.OperationSuccess = 1;
                log.OperationContent = "获取采样人员信息成功:" + postData;
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Lib.LogManager.Logger.LogException(ex);
                log.OperationTime = DateTime.Now;
                log.OperationName = "获取采样人员信息";
                log.OperationSuccess = 0;
                log.OperationContent = "获取采样人员信息失败:" + ex.Message + "||" + ticket;

            }
            finally
            {
                Lib.LogManager.Logger.LogInfo(log.OperationContent);
                SaveSysInterfaceLog(log);
            }

            return listSampMain;
        }
    }
}
