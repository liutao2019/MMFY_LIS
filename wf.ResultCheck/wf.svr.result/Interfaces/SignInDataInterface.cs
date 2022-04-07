using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Lib.DAC;
using Lib.DataInterface.Implement;
using dcl.pub.entities;
using dcl.svr.cache;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.result
{
    class SignInDataInterface
    {
        EntityRemoteCallClientInfo _caller;
        private DataTable dtPatCombine;
        private DataTable dtPatInfo;
        private EntityOperationResult _result;
        public SignInDataInterface(EntityRemoteCallClientInfo caller, DataTable combine, DataTable patInfo, EntityOperationResult result)
        {
            this._caller = caller;
            dtPatCombine = combine;
            dtPatInfo = patInfo;
            _result = result;
        }

        public void Execute()
        {
            ThredExecute();
            //t.Start();
        }

        void ThredExecute()
        {
            try
            {
                string opcode = _caller.LoginID;
                string opname = _caller.LoginName;
                DataTable doc = DictDoctorCache.Current.GetDoctors();
                if (doc != null && doc.Rows.Count > 0)
                {
                    DataRow[] rows =
                        doc.Select(string.Format("doc_name='{0}' ", _caller.LoginName));
                    if (rows.Length > 0)
                    {
                        opcode = rows[0]["doc_code"] != null
                                      ? rows[0]["doc_code"].ToString()
                                      : opcode;
                        opname = rows[0]["doc_dep_id"] != null
                                        ? rows[0]["doc_dep_id"].ToString()
                                        : opname;
                    }
                }

              

                //需要时再加
                //list.Add(new InterfaceDataBindingItem("pat_pid", _patInfo.pat_pid));
                //list.Add(new InterfaceDataBindingItem("pat_name", _patInfo.pat_name));
                //list.Add(new InterfaceDataBindingItem("pat_c_name", _patInfo.pat_c_name));
                //list.Add(new InterfaceDataBindingItem("pat_report_code", _patInfo.pat_report_code));
                //list.Add(new InterfaceDataBindingItem("pat_report_date", _patInfo.pat_report_date));
                //list.Add(new InterfaceDataBindingItem("pat_bar_code", _patInfo.pat_bar_code));
                //list.Add(new InterfaceDataBindingItem("op_time", DateTime.Now));
                Dictionary<string,List<InterfaceDataBindingItem>> notSucclist=new Dictionary<string, List<InterfaceDataBindingItem>>();
                DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);

                string pat_ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();
                string groupName = string.Empty;
                if (pat_ori_id == "107")
                {
                    groupName="检验_门诊_登记录入后";
                }
                else if (pat_ori_id == "108")
                {
                    //Logger.WriteException(this.GetType().Name, "记录ID医嘱:", bc_yz_id);
                    groupName = "检验_住院_登记录入后";
                }
                else if (pat_ori_id == "109")
                {
                    groupName = "检验_体检_登记录入后";
                }

                if (string.IsNullOrEmpty(groupName)) return;
                string sql =
                string.Format(
                    "select top 1 cmd_id from dict_DataInterfaceCommand where cmd_group='{0}' and cmd_id<>'11'  and cmd_enabled=1 ",
                    groupName);

                SqlHelper helper = new SqlHelper();

                string cmdID = string.Empty;
                object cmdid = helper.ExecuteScalar(sql);

                if (cmdid != null && cmdid != DBNull.Value && !string.IsNullOrEmpty(cmdid.ToString()))
                {
                    cmdID = cmdid.ToString();
                }
                if (string.IsNullOrEmpty(cmdID)) return;
                foreach (DataRow row in dtPatCombine.Rows)
                {
                    List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                    list.Add(new InterfaceDataBindingItem("bc_in_no", dtPatInfo.Rows[0]["pat_in_no"]));
                    list.Add(new InterfaceDataBindingItem("op_code", opcode));
                    list.Add(new InterfaceDataBindingItem("op_name", _caller.Remarks));//科室
                    list.Add(new InterfaceDataBindingItem("bc_times", 1));
                    string bc_yz_id = row["pat_yz_id"].ToString();
                    list.Add(new InterfaceDataBindingItem("bc_yz_id", bc_yz_id));
                    try
                    {
                        if (pat_ori_id == "108")
                        {
                            dihelper.ExecuteScalar("11", list.ToArray());
                        }
                        object ret = dihelper.ExecuteScalar(cmdID, list.ToArray());
                        if (ret != null && ret != DBNull.Value && ret.ToString().ToUpper() != "OK")
                        {
                            _result.AddMessage(EnumOperationErrorCode.ChargeFall, bc_yz_id,
                                               EnumOperationErrorLevel.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _result.AddMessage(EnumOperationErrorCode.ChargeFall, bc_yz_id, EnumOperationErrorLevel.Error);
                        Logger.WriteException(this.GetType().Name, "执行DataInterface登记录入后", ex.ToString());
                        return;
                    }
                }
                //if (notSucclist.Count > 0)
                //{
                //    Thread.Sleep(2000);
                //}
                //foreach (string key in notSucclist.Keys)
                //{
                //    try
                //    {
                //        if (pat_ori_id == "107")
                //        {
                //            dihelper.ExecuteNonQueryWithGroup("检验_门诊_登记录入后", notSucclist[key].ToArray());
                //        }
                //        else if (pat_ori_id == "108")
                //        {
                //            Thread.Sleep(100);
                //            dihelper.ExecuteNonQueryWithGroup("检验_住院_登记录入后", notSucclist[key].ToArray());
                //        }
                //        else if (pat_ori_id == "109")
                //        {
                //            object ret = dihelper.ExecuteScalar("16", notSucclist[key].ToArray());
                //            if (ret != null && ret != DBNull.Value && ret.ToString().ToUpper() != "OK")
                //            {
                //                _result.AddMessage(EnumOperationErrorCode.ChargeFall, key, EnumOperationErrorLevel.Error);
                //                return;
                //            }
                //        }
                //    }
                //    catch (Exception ex1)
                //    {
                //        _result.AddMessage(EnumOperationErrorCode.ChargeFall, bc_yz_id, EnumOperationErrorLevel.Error);
                //        Logger.WriteException(this.GetType().Name, "执行SignInDataInterface登记录入后:出错医嘱ID:" + key + "，重传失败", ex1.ToString());
                //        return;

                //    }

                //}
                   

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "执行DataInterface登记录入后", ex.ToString());
            }
        }
    }
}
