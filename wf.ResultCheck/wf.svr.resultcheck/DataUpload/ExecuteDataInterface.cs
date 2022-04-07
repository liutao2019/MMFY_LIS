using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using dcl.root.logon;
using Lib.DataInterface.Implement;
using Lib.DAC;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    class ExecuteDataInterface
    {
        EntityPidReportMain _patInfo;

        public ExecuteDataInterface(EntityPidReportMain patInfo)
        {
            this._patInfo = patInfo;
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
                //TODO HB 2018-04-13
                return;

                string sqlSelectBarcode = @"
select bc_yz_id from bc_cname with(nolock)
where bc_bar_code = ? and (bc_del is null or bc_del = '0')
and bc_yz_id is not null and len(bc_yz_id) > 0
";

                SqlHelper helper = new SqlHelper();
                DbCommandEx cmd = helper.CreateCommandEx(sqlSelectBarcode);
                cmd.AddParameterValue(_patInfo.RepBarCode, System.Data.DbType.AnsiString);
                DataTable table = helper.GetTable(cmd);

                foreach (DataRow row in table.Rows)
                {
                    List<InterfaceDataBindingItem> list = new List<InterfaceDataBindingItem>();
                    list.Add(new InterfaceDataBindingItem("pat_id", _patInfo.RepId));
                    list.Add(new InterfaceDataBindingItem("pat_in_no", _patInfo.PidInNo));
                    list.Add(new InterfaceDataBindingItem("pat_pid", _patInfo.RepInputId));
                    list.Add(new InterfaceDataBindingItem("pat_name", _patInfo.PidName));
                    list.Add(new InterfaceDataBindingItem("pat_c_name", _patInfo.PidComName));
                    list.Add(new InterfaceDataBindingItem("pat_report_code", _patInfo.RepReportUserId));
                    list.Add(new InterfaceDataBindingItem("pat_report_date", _patInfo.RepReportDate));
                    list.Add(new InterfaceDataBindingItem("pat_bar_code", _patInfo.RepBarCode));
                    list.Add(new InterfaceDataBindingItem("op_time", DateTime.Now));
                    list.Add(new InterfaceDataBindingItem("pat_emp_id", _patInfo.PidExamNo));

                    DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);
                    string bc_yz_id = row["bc_yz_id"].ToString();
                    list.Add(new InterfaceDataBindingItem("bc_yz_id", bc_yz_id));

                    if (this._patInfo.PidSrcId == "107")
                    {
                        dihelper.ExecuteNonQueryWithGroup("检验_门诊_二审后", list.ToArray());
                    }
                    else if (this._patInfo.PidSrcId == "108")
                    {
                        dihelper.ExecuteNonQueryWithGroup("检验_住院_二审后", list.ToArray());
                    }
                    else if (this._patInfo.PidSrcId == "109")
                    {
                        dihelper.ExecuteNonQueryWithGroup("检验_体检_二审后", list.ToArray());
                    }
                    else
                    {
                        dihelper.ExecuteNonQueryWithGroup("检验_其他_二审后", list.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "执行DataInterface", ex.ToString());
            }
        }
    }
}
