using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.oa
{
    public class OfficeHoRecordBIZ : IOfficeHoRecord
    {


        public bool UpdateHandoverInfo(EntityHoRecord info)
        {
            bool result = false;
            IDaoOaHoRecord dao = DclDaoFactory.DaoHandler<IDaoOaHoRecord>();
            if (dao != null)
            {
                if (info.IsNew)
                {
                    result= dao.SaveHandoverInfo(info);
                }
                //else
                //{
                //  //  return (dao.UpdateHandoverInfo(info));
                //}
            }
            else
            {
                result= false;
            }
            return result;
        }

        public EntityHoRecord GetHandoverStat(DateTime dtFrom, DateTime dtTo, string ctypeID)
        {
            IDaoOaHoRecord dao = DclDaoFactory.DaoHandler<IDaoOaHoRecord>();
            if (dao != null)
            {
                return (dao.GetHandoverStat(dtFrom, dtTo, ctypeID));
            }
            else
            {
                return new EntityHoRecord();
            }
        }

        public List<EntityHoRecord> GetHandoverList(DateTime dtFrom, DateTime dtTo)
        {
            IDaoOaHoRecord dao = DclDaoFactory.DaoHandler<IDaoOaHoRecord>();
            if (dao != null)
            {
                return (dao.GetHandoverList(dtFrom, dtTo));
            }
            else
            {
                return new List<EntityHoRecord>();
            }
        }


        public bool DeleteHandover(string ho_id)
        {
            IDaoOaHoRecord dao = DclDaoFactory.DaoHandler<IDaoOaHoRecord>();
            if (dao != null)
            {
                return (dao.DeleteHandover(ho_id));
            }
            else
            {
                return false;
            }
        }


        public List<EntityHoRecord> GetDtNullResData(string ctypeID)
        {
            IDaoOaHoRecord dao = DclDaoFactory.DaoHandler<IDaoOaHoRecord>();
            if (dao != null)
            {
                return (dao.GetDtNullResData(ctypeID));
            }
            else
            {
                return new List<EntityHoRecord>();
            }
        }
    }
}
