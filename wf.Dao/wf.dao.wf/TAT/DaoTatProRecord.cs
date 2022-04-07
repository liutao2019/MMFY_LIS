using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoTatProRecord))]
    public class DaoTatProRecord : IDaoTatProRecord
    {
        public int CountRecordByBarCode(string barCord)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(barCord))
            {
                DBManager helper = new DBManager();
                string sql = string.Format("select count(1) from tat_pro_record with(nolock) where tpr_bar_code='{0}' ", barCord);
                DataTable dt = helper.ExecuteDtSql(sql);
                result = Convert.ToInt32(dt.Rows[0][0]);
            }
            return result;
        }

        public bool DeleteRecordByBarCode(string barCode)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(barCode))
            {
                DBManager helper = new DBManager();
                try
                {
                    string sql = string.Format("delete from tat_pro_record  where tpr_bar_code='{0}' ", barCode);
                    helper.ExecCommand(sql);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool InsertTATProRecord(string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(barCode))
            {
                DBManager helper = new DBManager();
                try
                {
                    string strInsertValue = GetColum(stepCode);
                    if (!string.IsNullOrEmpty(strInsertValue))
                    {
                        string sql = string.Format(@"INSERT INTO [tat_pro_record]
                       ([tpr_bar_code]
                       ,{0},[tpr_stauts]
                       ,[tpr_createdate])
                       VALUES
                       ('{1}'
                       ,'{2}'
                       ,'{3}'
                       ,'{2}')", strInsertValue, barCode, dtToday, stepCode);
                        helper.ExecCommand(sql);

                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
        private string GetColum(string stepcode)
        {
            string strUpdateValue = string.Empty;
            switch (stepcode)
            {
                case "2":
                    strUpdateValue = "tpr_blood_date";
                    break;
                case "3":
                    strUpdateValue = "tpr_send_date";
                    break;
                case "4":
                    strUpdateValue = "tpr_reach_date";
                    break;
                case "5":
                    strUpdateValue = "tpr_receiver_date";
                    break;
                case "9":
                    strUpdateValue = "tpr_return_date";
                    break;
                case "20":
                    strUpdateValue = "tpr_jy_date";
                    break;
                case "40":
                    strUpdateValue = "tpr_check_date";
                    break;
                case "60":
                    strUpdateValue = "tpr_report_date";
                    break;
                default:
                    break;
            }
            return strUpdateValue;
        }
        public bool UpdateTATProRecord( string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(barCode))
            {
                DBManager helper = new DBManager();
                try
                {
                    string strUpdateValue =GetColum(stepCode);
                    if (!string.IsNullOrEmpty(strUpdateValue))
                    {
                        string sql = string.Format(@"UPDATE [tat_pro_record] SET  {0} ='{1}',tpr_stauts='{3}'
                                         WHERE tpr_bar_code='{2}'", strUpdateValue, dtToday, barCode, stepCode);
                        helper.ExecCommand(sql);

                        result = true;
                    }
    
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public EntityTatProRecord GetTatRecord(string barCode)
        {
            EntityTatProRecord tat = new EntityTatProRecord();
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"select tpr_bar_code, tpr_apply_date, tpr_blood_date, tpr_send_date,tpr_reach_date, tpr_receiver_date, tpr_reg_date, 
                                                          tpr_test_date, tpr_jy_date, tpr_check_date, tpr_report_date, tpr_return_date, tpr_target_date, tpr_remark, tpr_flag,
                                                           tpr_stauts, tpr_createdate from tat_pro_record where tpr_bar_code='{0}'", barCode);
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityTatProRecord> list = EntityManager<EntityTatProRecord>.ConvertToList(dt);
                if (list != null && list.Count > 0)
                {
                    tat = list[0];
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return tat;
        }
    }
}
