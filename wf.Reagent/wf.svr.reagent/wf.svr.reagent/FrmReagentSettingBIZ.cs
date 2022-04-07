using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wf.svr.reagent
{
    public class FrmReagentSettingBIZ : IReaSetting
    {
        public bool DeleteReaSetting(EntityReaSetting reaSetting)
        {
            bool isTrue = false;
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                isTrue = dao.DeleteReaSetting(reaSetting);
            }
            return isTrue;
        }

        public string GetReaBarcodeByID(string reaid)
        {
            string str = string.Empty;
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                str = dao.GetReaBarcodeByID(reaid);
            }
            return str;
        }

        public void LogLogin(string type, string module, string loginID, string ip, string mac, string message)
        {
            IDaoSysLoginLog daoPar = DclDaoFactory.DaoHandler<IDaoSysLoginLog>();
            if (daoPar != null)
            {
                daoPar.LogLogin(type, module, loginID, ip, mac, message);
            }
        }

        public EntityResponse SaveReaSetting(EntityReaSetting reaSetting)
        {
            EntityResponse result = new EntityResponse();
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                result = dao.SaveReaSetting(reaSetting);
                if (!string.IsNullOrEmpty(reaSetting.Barcode))
                {
                    dao.InsertBarcode(reaSetting.Barcode, reaSetting.Drea_id);
                }
            }
            return result;
        }

        public List<EntityReaSetting> SearchReaSettingAll()
        {
            List<EntityReaSetting> list = new List<EntityReaSetting>();
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                list = dao.SearchReaSettingAll();
            }
            return list;
        }

        public bool UpdateBarcode(EntityReaSetting reaSetting)
        {
            bool isTrue = false;
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                isTrue = dao.UpdateBarcode(reaSetting);
                if (!string.IsNullOrEmpty(reaSetting.Barcode))
                {
                    string barcode = dao.GetReaBarcodeByID(reaSetting.Drea_id);
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        dao.UpdateBarcode(reaSetting);
                    }
                    else
                    {
                        dao.InsertBarcode(reaSetting.Barcode, reaSetting.Drea_id);
                    }
                }
            }
            return isTrue;
        }

        public bool UpdateReaSetting(EntityReaSetting reaSetting)
        {
            bool isUpdate = false;
            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao != null)
            {
                isUpdate = dao.UpdateReaSetting(reaSetting);
            }
            return isUpdate;
        }
    }
}
