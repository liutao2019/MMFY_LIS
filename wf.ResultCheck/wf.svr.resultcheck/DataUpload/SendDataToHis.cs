using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Threading;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class SendDataToHis 
    {
        protected EntityPidReportMain pat_info = null;
        protected List<EntityObrResult> resulto = null;
        protected List<EntityPidReportDetail> patients_mi = null;
        public EnumOperationCode auditType { get; set; }
        protected AuditConfig config = null;



        public EntityRemoteCallClientInfo Caller { get; set; }

        public SendDataToHis()
        { }

        public SendDataToHis(EntityPidReportMain pat_info, AuditConfig config, EnumOperationCode auditType, EntityRemoteCallClientInfo caller)
        {
            _patid = pat_info.RepId;
            _LoginID = caller.LoginID;
            this.pat_info = pat_info;
            this.auditType = auditType;
            this.config = config;
            this.Caller = caller;
        }
        string _patid;
        string _LoginID;
        public SendDataToHis(string patid, string LoginID)
        {
            _patid = patid;
            _LoginID = LoginID;
        }

        public void Execute()
        {
            Thread t = new Thread(ThredExecute);
            t.Start();
        }

        void ThredExecute()
        {
            try
            {
                if (auditType == EnumOperationCode.Report)
                {
                    string strInterMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode");
                    if (strInterMode == "惠州六院")
                    {

                        Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();

                        //ser.ChangeOrdItemStatusByPatID(_patid, _LoginID, "R");
                        ser.ChangeOrdItemStatusByPatID(_patid, _LoginID, "E");
                        ser.LReport(_patid);
                    }


                    //if (auditType == EnumOperationCode.UndoReport)
                    //{
                    //    Lis.HZLYHis.Interface.HZLYService ser = new Lis.HZLYHis.Interface.HZLYService();

                    //    ser.ChangeOrdItemStatusByPatID(pat_info.RepId, Caller.LoginID, "D");
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "执行SendDataToHis", ex.ToString());
            }
        }
    }
}
