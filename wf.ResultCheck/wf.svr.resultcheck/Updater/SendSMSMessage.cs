using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.ProxyFactory;
using dcl.pub.entities;
using dcl.pub.entities.Message;
using System.Threading;
using dcl.root.logon;
using Lib.DAC;
using System.Data;
using Lib.DataInterface;
using Lib.DataInterface.Implement;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    public class SendSMSMessage : AbstractAuditClass, IAuditUpdater
    {

        public SendSMSMessage(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        public SendSMSMessage()
            : base(null, null, null, EnumOperationCode.Report, null)
        {

        }
        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report)//只有报告(二审)时才操作
            {
                 if (!string.IsNullOrEmpty(CacheSysConfig.Current.GetSystemConfig("ZSLY_HandMessageAddress")))
                 {
                     try
                     {
                         string wsdlAddress = CacheSysConfig.Current.GetSystemConfig("ZSLY_HandMessageAddress");
                         if (!wsdlAddress.Contains("?wsdl"))
                         {
                             wsdlAddress = wsdlAddress + "?wsdl";
                         }

                         string oriID = "1";

                         if (pat_info.PidSrcId == "107") oriID = "1";
                         if (pat_info.PidSrcId == "108") oriID = "2";
                         if (pat_info.PidSrcId == "109") oriID = "3";


                         StringBuilder sb = new StringBuilder();
                         sb.AppendLine("报告提醒");
                         sb.AppendLine(string.Format("{0}   中山大学附属第六医院", DateTime.Now.ToString("yyyy-MM-dd")));
                         sb.AppendLine(string.Format("病历号:{0}", pat_info.PidInNo));
                         sb.AppendLine(string.Format("就诊人姓名:{0}", pat_info.PidName));
                         sb.AppendLine("您的报告已经出来了,请到 [综合楼一楼服务大厅服务台] 打印");

                         TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime("1970-01-01 00:00:00"));

                         WSProxyFactory factory = new WSProxyFactory(wsdlAddress);
                         object retString = factory.Invoke("SendMessage",
                             new object[]
                            {
                                sb.ToString(),"中山六院检验科"
                                , pat_info.PidInNo??""
                                ,oriID
                              ,ts.TotalMilliseconds
                            });

                         if (retString != null && !string.IsNullOrEmpty(retString.ToString()))
                         {
                             Logger.WriteException(this.GetType().Name, string.Format("掌上医院消息推送接口"), retString.ToString());
                         }
                     }
                     catch (Exception ex)
                     {
                         Logger.WriteException(this.GetType().Name, string.Format("掌上医院消息推送接口"), ex.ToString());
                     }
                     
                 }


                if (CacheSysConfig.Current.GetSystemConfig("HospitalName") == "深圳沙井人民医院")
                {

                    if (CacheSysConfig.Current.GetSystemConfig("Audit_SendReportSMS") == "是")
                    {
                        //门诊病人发短息
                        if (pat_info.PidSrcId == "107" && !string.IsNullOrEmpty(pat_info.PidTel))
                        {
                            string strSex = "";
                            if (pat_info.PidSex == "1")
                            {
                                strSex = "先生";
                            }
                            else if (pat_info.PidSex == "2")
                            {
                                strSex = "女士";
                            }
                            string strSmsCon = "尊敬的" + pat_info.PidName + strSex + ",您的 " + pat_info.PidComName + " 检验报告已出,您可以到检验科自助打印处打印此报告。";
                            SjyyAppInterfaceLib.clsSMSInterface sms = new SjyyAppInterfaceLib.clsSMSInterface();
                            string strRes = sms.SendSMS(pat_info.PidTel, strSmsCon, "0", pat_info.RepReportUserId, "LIS");
                            if (!string.IsNullOrEmpty(strRes))
                            {
                                Logger.WriteException(this.GetType().Name, "SendSMSMessage", strRes);
                            }
                        }

                    }

                }

            }
        }

        #endregion

        public void UpdateByBacteria(DataTable dtbCriticalPat)
        {
            if (CacheSysConfig.Current.GetSystemConfig("HospitalName") == "深圳沙井人民医院")
            {

                if (CacheSysConfig.Current.GetSystemConfig("Audit_SendReportSMS") == "是")
                {

                    foreach (DataRow dr in dtbCriticalPat.Rows)
                    {
                        //门诊病人发短息
                        if (dr["pat_ori_id"].ToString() == "107" && !string.IsNullOrEmpty(dr["pat_tel"].ToString()))
                        {
                            string strSex = "";
                            if (dr["pat_sex"].ToString() == "1")
                            {
                                strSex = "先生";
                            }
                            else if (dr["pat_sex"].ToString() == "2")
                            {
                                strSex = "女士";
                            }
                            string strSmsCon = "尊敬的" + dr["pat_name"].ToString() + strSex + ",您的" + dr["pat_c_name"].ToString() + " 的检验报告已出,您可以到检验科自助打印处打印此报告。";


                            SjyyAppInterfaceLib.clsSMSInterface sms = new SjyyAppInterfaceLib.clsSMSInterface();
                            string strRes = sms.SendSMS(dr["pat_tel"].ToString(), strSmsCon, "0", dr["pat_report_code"].ToString(), "LIS");
                            if (!string.IsNullOrEmpty(strRes))
                            {
                                Logger.WriteException(this.GetType().Name, "SendSMSMessage", strRes);
                            }
                        }
                    }



                }

            }
        }
    }
}
