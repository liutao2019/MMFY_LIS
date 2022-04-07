using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lib.DAC;

namespace dcl.svr.sample
{
    public class TATRecordHelper
    {
        string m_barcodes;
        string m_stepcode;
        string m_loginid;
        string m_dt;
        public void Record(string barcodes,string stepcode,string loginid,string dt)
        {
            m_barcodes = barcodes;
            m_stepcode = stepcode;
            m_loginid = loginid;
            m_dt = dt;
            Thread th = new Thread(TATRecode);
            th.Start();
        }

        public void Clear(string barcode)
        {
            m_barcodes = barcode;
            Thread th = new Thread(TATRecodeDelete);
            th.Start();
        }

        private void TATRecodeDelete()
        {
            if (m_barcodes == "")
            {
                return;
            }

            try
            {
                string sql = string.Format("delete from tat_pro_record  where tpr_bar_code='{0}' ", m_barcodes);

                SqlHelper heler = new SqlHelper();
                heler.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        private void TATRecode()
        {
            try
            {
                string col = GetColum(m_stepcode);

                if (col == ""|| m_barcodes=="")
                {
                    Lib.LogManager.Logger.LogInfo(m_stepcode + "暂时不支持");
                    return;
                }
                foreach (string m_barcode in m_barcodes.Split(','))
                {
                    string sql = string.Format("select count(1) from tat_pro_record with(nolock) where tpr_bar_code='{0}' ", m_barcode);

                    SqlHelper heler = new SqlHelper();
                    object ob = heler.ExecuteScalar(sql);

                    if (ob == null || Convert.ToInt32(ob) == 0)
                    {
                        sql = string.Format(@"INSERT INTO [tat_pro_record]
                       ([tpr_bar_code]
                       ,{0},[tpr_stauts]
                       ,[tpr_createdate])
                       VALUES
                       ('{1}'
                       ,'{2}'
                       ,'{3}'
                       ,'{2}')", col, m_barcode, m_dt,m_stepcode);

                    }
                    else
                    {
                        sql = string.Format(@"UPDATE [tat_pro_record] SET  {0} ='{1}',tpr_stauts='{3}'
                                         WHERE tpr_bar_code='{2}'", col, m_dt, m_barcode, m_stepcode);
                    }
                    heler.ExecuteNonQuery(sql);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        private string GetColum(string stepcode)
        {
            switch(stepcode)
            {
                case "2":
                    return "tpr_blood_date";
                case "3":
                    return "tpr_send_date";
                case "4":
                    return "tpr_reach_date";
                case "5":
                    return "tpr_receiver_date";
                case "9":
                    return "tpr_return_date";
                case "20":
                    return "tpr_jy_date";
                case "40":
                    return "tpr_check_date";
                case "60":
                    return "tpr_report_date";
            }
            return "";
        }
    }
}
