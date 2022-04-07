using System;
using System.Data;
using dcl.svr.msg;

namespace dcl.svr.resultcheck.Updater
{
    public class CASignature
    {
        //加锁
        private static Object thisCALock = new Object();

        /// <summary>
        /// 将报告单电子签章写入数据库
        /// </summary>
        /// <param name="p_dtbCASigncontent"></param>
        /// <returns></returns>
        public bool InsertReportCASignature(DataTable p_dtbCASigncontent)
        {
            bool blnRes = false;
            try
            {
                p_dtbCASigncontent.TableName = "patients_ext";
                if (p_dtbCASigncontent != null && p_dtbCASigncontent.Rows.Count > 0)
                {
                    //加锁防止并发
                    lock (thisCALock)
                    {
                        foreach (DataRow dr in p_dtbCASigncontent.Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["RepId"].ToString()))
                            {
                                PidReportMainExtBIZ BIZ = new PidReportMainExtBIZ();
                                blnRes=BIZ.InsertReportCASignature(dr);
                            }
                        }
                    }
                    blnRes = true;
                }
            }
            catch (Exception ex)
            {
                blnRes = false;
                Lib.LogManager.Logger.LogException("电子签章写入数据库", ex);
            }

            return blnRes;
        }
    }
}
