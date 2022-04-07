using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using dcl.svr.interfaces.LisDataInterface;
using dcl.pub.entities;
using dcl.pub.entities.dict;
using dcl.svr.cache;
using dcl.root.logon;

namespace dcl.svr.result
{
    class BarcodeDirectConfirmInterface
    {
        EntityRemoteCallClientInfo _caller;
        private DataTable dtPatCombine;
        private DataTable dtPatInfo;
        public BarcodeDirectConfirmInterface(EntityRemoteCallClientInfo caller, DataTable combine, DataTable patInfo)
        {
            _caller = caller;
            dtPatCombine = combine;
            dtPatInfo = patInfo;
            //_result = result;
        }

        public void Execute()
        {
            Thread t = new Thread(ThredExecute);
            t.Start();
            //ThredExecute();
            //t.Start();
        }

        void ThredExecute()
        {
            try
            {
                string opcode = _caller.LoginID;
                string opname = _caller.LoginName;

                string ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();

                foreach (DataRow row in dtPatCombine.Rows)
                {
                    if( row["pat_yz_id"]==null||string.IsNullOrEmpty( row["pat_yz_id"].ToString()))continue;
                    EntityDataInterfaceAdviceConfirmParameter p = new EntityDataInterfaceAdviceConfirmParameter();
                    p.bc_in_no = dtPatInfo.Rows[0]["pat_in_no"].ToString();
                    p.bc_pid = dtPatInfo.Rows[0]["pat_pid"] == null ? "" : dtPatInfo.Rows[0]["pat_pid"].ToString();
                    p.bc_yz_id = row["pat_yz_id"].ToString();
                    p.bc_yz_id2 = row["pat_yz_id"].ToString();
                    p.op_code = opcode;
                    p.op_name = opname;
                    p.bc_app_no = dtPatInfo.Rows[0]["pat_app_no"]==null?"":dtPatInfo.Rows[0]["pat_app_no"].ToString();
                    p.bc_his_code = row["pat_his_code"] == null ? "" : row["pat_his_code"].ToString();
                    p.bc_his_name = row["pat_his_code"] == null ? "" : row["pat_his_code"].ToString();
                    p.bc_social_no = dtPatInfo.Rows[0]["pat_social_no"] == null ? "" : dtPatInfo.Rows[0]["pat_social_no"].ToString();
                    p.bc_emp_id = dtPatInfo.Rows[0]["pat_emp_id"] == null ? "" : dtPatInfo.Rows[0]["pat_emp_id"].ToString(); ;
                    p.bc_cuv_code ="";
                    p.bc_cuv_count = 1;
                    p.bc_barcode_id = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                    if (dtPatInfo.Rows[0]["pat_admiss_times"]==null||dtPatInfo.Rows[0]["pat_admiss_times"]==DBNull.Value
                        || string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_admiss_times"].ToString()))
                        p.bc_times = "0";
                    else
                        p.bc_times = dtPatInfo.Rows[0]["pat_admiss_times"].ToString();


                    if (ori_id == "107")//门诊
                    {
                        string stepProc = CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirm");

                        if (stepProc == "标本签收")
                        {
                            new BarcodeConfirmInterface(p).mz_ExecuteAfterSignIn();
                        }
                    }
                    else if (ori_id == "108")//住院
                    {
                        string stepProc = CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirm");

                        if (stepProc == "标本签收")
                        {
                            new BarcodeConfirmInterface(p).zy_ExecuteAfterSignIn();
                        }
                    }
                    else if (ori_id == "109" || ori_id == "10002")//体检包括第二体检
                    {
                        string stepProc = CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirm");

                        if (stepProc == "标本签收")
                        {
                            new BarcodeConfirmInterface(p).tj_ExecuteAfterSignIn();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "执行DataInterface签收后", ex.ToString());
            }
        }
    }
}
