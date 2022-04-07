using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Lib.DAC;
using Lib.ProxyFactory;
using dcl.svr.cache;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    public class SendAuditNotifyToKindem
    {
        public SendAuditNotifyToKindem(EntityPidReportMain patInfo,string address)
        {
            _patInfo = patInfo;
            _address = address;
        }
        string _address { get; set; }
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
                string column = CacheSysConfig.Current.GetSystemConfig("Audit_KindemCarDataBaseColumn");

                string sql = string.Format("select top 1 {0} from patients where pat_id='{1}'", column, _patInfo.RepId);

                SqlHelper helper=new SqlHelper();

                string cardNo = string.Empty;
                object obj = helper.ExecuteScalar(sql);

                if (obj != null && obj != DBNull.Value) cardNo = obj.ToString();

                if (string.IsNullOrEmpty(cardNo))
                {
                    throw new Exception(string.Format("健康卡号不存在:{0}", column));
                }

                WSProxyFactory factory = new WSProxyFactory(_address);
                object retString = factory.Invoke("InspectionNotify",
                                                  new object[]
                                                      {
                                                          cardNo
                                                          , _patInfo.RepId,
                                                         DateTime.Now.ToString("yyyy-MM-dd")
                                                      });

                if (retString != null && !string.IsNullOrEmpty(retString.ToString()) && retString.ToString()!="0")
                {
                    Logger.WriteException(this.GetType().Name, string.Format("传递给移动医疗平台接口返回信息"),
                                          retString.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "报告完成通知消息传递给移动医疗平台接口报错", ex.ToString());
            }


        }

    }
}

