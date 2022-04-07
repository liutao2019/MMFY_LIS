using System;
using dcl.client.frame;

namespace dcl.client.notifyclient
{
    /// <summary>
    /// 报告超时提醒
    /// </summary>
    public class OverTimeWarningHelper
    {
        /// <summary>
        ///  检验报告超时提醒
        /// </summary>
        private FrmOverTimeWarning frmOverTim = null;

        #region Instance
        private static OverTimeWarningHelper _instance = null;

        private static object instanceLock2 = new object();

        public static OverTimeWarningHelper Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock2)
                    {
                        if (_instance == null)
                        {
                            _instance = new OverTimeWarningHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 检验报告超时提醒信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="pat_itr_ids">仪器IDs</param>
        public void startCheckOverTime(DateTime date, string pat_itr_ids)
        {
            try
            {
                //Lab_ShowOvertimeForm  是否报告超时提醒信息
                //BC_EnableOvertimeMessage  是否开启时间限定功能
                if (UserInfo.GetSysConfigValue("Lab_ShowOvertimeForm") == "是"
                    && UserInfo.GetSysConfigValue("BC_EnableOvertimeMessage") == "是")
                {
                    //DataTable dtOverTime = dcl.client.wcf.PatientCRUDClient.NewInstance.GetPatientStatusForOverTime(date, pat_itr_ids);

                    //if (dtOverTime != null && dtOverTime.Rows.Count > 0)
                    //{
                    //    FrmOverTimeWarning frmOverTim = new FrmOverTimeWarning();
                    //    frmOverTim.Show();
                    //    frmOverTim.LoadData(dtOverTime);
                    //}

                    //报告超时提醒信息查询间隔时间(分钟)
                    string configvalue = UserInfo.GetSysConfigValue("Lab_ShowOvertimeQueryInter");

                    int i;

                    if (string.IsNullOrEmpty(configvalue) || !int.TryParse(configvalue, out i))
                    {
                        i = 1;
                    }

                    if (frmOverTim == null)
                    {
                        frmOverTim = new dcl.client.notifyclient.FrmOverTimeWarning(date, pat_itr_ids);
                        frmOverTim.OvertimeQueryInter = i;
                        frmOverTim.startShowFrm();//开启
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void stop()
        {
            if (frmOverTim != null)
            {
                frmOverTim.StriogClose();//关闭并释放资源
                frmOverTim = null;//置空
            }
        }
    }
}
