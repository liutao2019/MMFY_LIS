using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Threading;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    class RunScriptAfterAudit : AbstractAuditClass, IAuditUpdater
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info"></param>
        /// <param name="resulto"></param>
        /// <param name="auditType"></param>
        /// <param name="config"></param>
        public RunScriptAfterAudit(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            //if (this.auditType == EnumOperationCode.Audit)//一审
            //{
            //    Thread t1 = new Thread(RunScriptAfterFirstAudit);
            //    t1.Start();
            //}
            //else if (this.auditType == EnumOperationCode.Report)//二审
            //{
            //    Thread t2 = new Thread(RunScriptAfterSecondAudit);
            //    t2.Start();
            //}
            //else if (this.auditType == EnumOperationCode.UndoAudit)//一审
            //{
            //    Thread t3 = new Thread(RunScriptAfterUndoFirstAudit);
            //    t3.Start();
            //}
            //else if (this.auditType == EnumOperationCode.UndoReport)//二审
            //{
            //    Thread t4 = new Thread(RunScriptAfterUndoSecondAudit);
            //    t4.Start();
            //}
        }

        #endregion

        /// <summary>
        /// 运行一审后脚本
        /// </summary>
        void RunScriptAfterFirstAudit()
        {
            //string msg;
            //try
            //{
            //    //执行脚本
            //    dcl.common.DynamicCompilation.ClsDynamicCompilationMemory.Instance.m_objDynamicCompilationMemory("AfterFirstAudit", new object[] { this.pat_info, this.patients_mi, this.resulto }, out msg);

            //    if (!string.IsNullOrEmpty(msg))
            //    {
            //        Logger.WriteException(this.GetType().Name, "RunScriptAfterFirstAudit：脚本执行错误", msg);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteException(this.GetType().Name, "RunScriptAfterFirstAudit", ex.ToString());
            //}
        }

        /// <summary>
        /// 运行二审后脚本
        /// </summary>
        void RunScriptAfterSecondAudit()
        {
            string msg;
            try
            {
                ////执行脚本
                //dcl.common.DynamicCompilation.ClsDynamicCompilationMemory.Instance.m_objDynamicCompilationMemory("AfterSecondAudit", new object[] { this.pat_info, this.patients_mi, this.resulto }, out msg);

                //if (!string.IsNullOrEmpty(msg))
                //{
                //    Logger.WriteException(this.GetType().Name, "RunScriptAfterSecondAudit：脚本执行错误", msg);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "RunScriptAfterSecondAudit", ex.ToString());
            }
        }

        /// <summary>
        /// 运行一审后脚本
        /// </summary>
        void RunScriptAfterUndoFirstAudit()
        {
            string msg;
            try
            {
                ////执行脚本
                //dcl.common.DynamicCompilation.ClsDynamicCompilationMemory.Instance.m_objDynamicCompilationMemory("AfterUndoFirstAudit", new object[] { this.pat_info, this.patients_mi, this.resulto }, out msg);

                //if (!string.IsNullOrEmpty(msg))
                //{
                //    Logger.WriteException(this.GetType().Name, "RunScriptAfterUndoFirstAudit：脚本执行错误", msg);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "RunScriptAfterUndoFirstAudit", ex.ToString());
            }
        }

        /// <summary>
        /// 运行二审后脚本
        /// </summary>
        void RunScriptAfterUndoSecondAudit()
        {
            string msg;
            try
            {
                ////执行脚本
                //dcl.common.DynamicCompilation.ClsDynamicCompilationMemory.Instance.m_objDynamicCompilationMemory("AfterUndoSecondAudit", new object[] { this.pat_info, this.patients_mi, this.resulto }, out msg);

                //if (!string.IsNullOrEmpty(msg))
                //{
                //    Logger.WriteException(this.GetType().Name, "RunScriptAfterUndoSecondAudit：脚本执行错误", msg);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "RunScriptAfterUndoSecondAudit", ex.ToString());
            }
        }
    }
}
