using dcl.common;
using dcl.dao.interfaces;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    public class TatProRecordBIZ : ITatProRecord
    {
        public int CountRecordByBarCode(string barCord)
        {
            int result = 0;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.CountRecordByBarCode(barCord);
            }
            return result;
        }

        public bool DeleteRecordByBarCode(string barCode)
        {
            bool result = false;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.DeleteRecordByBarCode(barCode);
            }
            return result;
        }

        public bool InsertTATProRecord( string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.InsertTATProRecord(stepCode, barCode, dtToday);
            }
            return result;
        }

        public bool TatRecode(string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            try
            {
                int ob = CountRecordByBarCode(barCode);
                if (ob == 0)
                {
                    InsertTATProRecord(stepCode, barCode, dtToday);
                }
                else
                {
                    UpdateTATProRecord(stepCode, barCode, dtToday);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateTATProRecord( string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.UpdateTATProRecord(stepCode, barCode, dtToday);
            }
            return result;
        }

    }
}
