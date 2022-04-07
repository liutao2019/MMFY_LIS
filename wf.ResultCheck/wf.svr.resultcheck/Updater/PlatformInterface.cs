using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Threading;
using dcl.root.logon;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    class PlatformInterface : AbstractAuditClass, IAuditUpdater
    {
    /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info"></param>
        /// <param name="resulto"></param>
        /// <param name="auditType"></param>
        /// <param name="config"></param>
        public PlatformInterface(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report)//只有报告(二审)时才操作
            {
                Thread t = new Thread(ThreadDoEvent);
                t.Start();
            }
        }

        #endregion

        void ThreadDoEvent()
        {
            try
            {
                string pat_bar_code = pat_info.RepBarCode;

                string platformconstr = ConfigurationManager.AppSettings["PlatformConnStr"];

                if (!string.IsNullOrEmpty(pat_bar_code) && !string.IsNullOrEmpty(platformconstr))
                {
                    //更新标本状态到省妇幼检验平台
                    SqlHelper helper = new SqlHelper(platformconstr, EnumDbDriver.MSSql, EnumDataBaseDialet.SQL2005);

                    string sql = string.Format(@"
declare @ret int
exec @ret = sp_update_barcode_status '{0}','60','{1}','{2}','{3}',0
select @ret
"
 , pat_bar_code
 , pat_info.RepReportUserId
 , pat_info.RepReportUserId
 , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
 );

                    object obj = helper.ExecuteScalar(sql);

                    Logger.WriteException(this.GetType().Name, "PlatformInterface", string.Format("调用平台更新报告状态，条码号={0}，返回状态={1}", pat_bar_code, obj));

                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "PlatformInterface", ex.ToString());
            }
        }
    }
}
