using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Lib.DAC;
using Lib.ProxyFactory;
using dcl.pub.entities;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    public class SendChangedResultDataToHis
    {
        public SendChangedResultDataToHis(EntityPidReportMain patInfo)
        {
            _patInfo = patInfo;
        }

        private EntityPidReportMain _patInfo { get; set; }

        public void Execute()
        {
            Thread t = new Thread(SendData);
            t.Start();
        }

        private void SendData()
        {
            try
            {
                SqlHelper helper = new SqlHelper();
                DateTime lastUnReportDate;
                string sql = @"Select top 1 bc_date from bc_sign
                    where pat_id =? and bc_status='70' order by bc_date desc ";

                DbCommandEx commandEx = helper.CreateCommandEx(sql);
                commandEx.AddParameterValue(_patInfo.RepId, DbType.AnsiString);
                object obj = helper.ExecuteScalar(commandEx);
                if (obj == null || obj == DBNull.Value || !DateTime.TryParse(obj.ToString(), out lastUnReportDate))
                {
                    return;
                }

                sql = @"Select isnull(OperationTime,getdate()) OperationTime,isnull([Group],'') [Group],isnull(Description,'') Description,
                        isnull(OperationType,'') OperationType,isnull(OperationContent,'') OperationContent from sysOperationLog
                    where OperationKey =? and Module='病人资料' and OperationTime>? ";

                DbCommandEx commandEx2 = helper.CreateCommandEx(sql);
                commandEx2.AddParameterValue(_patInfo.RepId, DbType.AnsiString);
                commandEx2.AddParameterValue(lastUnReportDate, DbType.DateTime);
                DataTable opLogDt = helper.GetTable(commandEx2, "opLog");

                if (opLogDt.Rows.Count == 0) return;

                StringBuilder sb = new StringBuilder();

                DataRow[] patRows = opLogDt.Select("Group='病人基本信息' and (OperationContent='组合名称' or OperationContent='病人ID' or OperationContent='姓名')  ");

                string operationType;
                string operationTime;
                string operationContent;
                string description;
                //for (int i = 0; i < patRows.Length; i++)
                //{

                //    if (i == 0)
                //    {
                //        sb.Append("病人基本信息:");
                //        sb.Append("\r\n");
                //    }
                //    operationType = patRows[i]["OperationType"].ToString();
                //    operationTime = Convert.ToDateTime(patRows[i]["OperationTime"].ToString()).ToString("yyyy-MM-dd hh:mm");
                //    operationContent = patRows[i]["OperationContent"].ToString();
                //    description = patRows[i]["Description"].ToString();
                //    sb.Append(string.Format("操作时间:{0},类型:{1},字段:{2},数据:{3}", operationTime, operationType, operationContent,
                //                            description));
                //    sb.Append("\r\n");
                //}

                patRows = opLogDt.Select("Group='病人结果'");

                for (int i = 0; i < patRows.Length; i++)
                {

                    if (i == 0)
                    {
                        sb.Append("检验结果更改通知");
                        sb.Append("\r\n");
                        sb.Append(string.Format("病历号为{0}的{1}的检验结果发生变更:", (_patInfo.PidInNo ?? ""), (_patInfo.PidName ?? "")));
                        sb.Append("\r\n");
                    }
                    operationType = patRows[i]["OperationType"].ToString();
                    operationTime = Convert.ToDateTime(patRows[i]["OperationTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                    operationContent = patRows[i]["OperationContent"].ToString();
                    description = patRows[i]["Description"].ToString();
                    sb.Append(string.Format("变更时间为:{0},类型为:{1},修改内容为:{2}结果从:{3};", operationTime, operationType, operationContent,
                                            description));
                    sb.Append("\r\n");
                }

                if (sb.Length > 0)
                {
                    string wsdlAddress = ConfigurationManager.AppSettings["ZLYYUnReportHisServiceAddress"];

                    Logger.WriteInfo(this.GetType().Name, "肿瘤医院发送反审后改动结果接口发送日志", sb.ToString() + "\r\nhis接口地址为：" + wsdlAddress);
                    if (string.IsNullOrEmpty(wsdlAddress)) return;
                    WSProxyFactory factory = new WSProxyFactory(wsdlAddress);
                    object retString=factory.Invoke("SaveLISFSRecord",
                        new object[]
                            {
                                _patInfo.PidSrcId == "108" ? "1" : "2"
                                , _patInfo.PidDoctorCode??""
                                , _patInfo.PidInNo??""
                                , _patInfo.PidName??""
                                , sb.ToString()
                            });

                    if (retString != null && !string.IsNullOrEmpty(retString.ToString()))
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("肿瘤医院发送反审后改动结果接口错误返回信息"),retString.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "肿瘤医院发送反审后改动结果接口报错", ex.ToString());
            }
            

        }

    }
}
