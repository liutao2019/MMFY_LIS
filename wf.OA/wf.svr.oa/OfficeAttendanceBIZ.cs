using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.oa
{
    class OfficeAttendanceBIZ : IOfficeAttendance
    {


        public string GetAttdRecordByUID(string userIDandIsWork)
        {

            string[] strings = userIDandIsWork.Split(';');
            string attdRecordID = string.Empty;
            if (strings.Length != 2)
            {
                return string.Empty;
            }
            IDaoOaWorkAttendance dao = DclDaoFactory.DaoHandler<IDaoOaWorkAttendance>();
            if (dao == null)
            {
                return string.Empty;
            }
            else
            {
                attdRecordID = dao.GetAttdRecordByUID(strings);
            }
            return attdRecordID;
        }

        public List<EntityOaWorkAttendance> GetAttRecordByUID(DateTime sDTime, DateTime eDTime)
        {
            List<EntityOaWorkAttendance> list = new List<EntityOaWorkAttendance>();
            IDaoOaWorkAttendance dao = DclDaoFactory.DaoHandler<IDaoOaWorkAttendance>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetAttRecordByUID(sDTime, eDTime);
            }
            return list;
        }


        public string GetMaxAttdRecordID()
        {
            string strMaxID = "";
            IDaoOaWorkAttendance dao = DclDaoFactory.DaoHandler<IDaoOaWorkAttendance>();
            if (dao == null)
            {
                return strMaxID;
            }
            else
            {
                strMaxID = dao.GetMaxAttdRecordID();
            }
            return strMaxID;
        }


        public List<EntityDicPubProfession> GetPhyic()
        {
            throw new NotImplementedException();
        }

        public int InsertAttdRecord(EntityOaWorkAttendance sample)
        {
            IDaoOaWorkAttendance dao = DclDaoFactory.DaoHandler<IDaoOaWorkAttendance>();
            if (dao == null)
            {
                return 0;
            }
            else
            {
                return dao.InsertAttdRecord(sample);
            }

        }

        public int ModifyAttdRecord(EntityOaWorkAttendance sample)
        {

            int intRet = -1;
            IDaoOaWorkAttendance dao = DclDaoFactory.DaoHandler<IDaoOaWorkAttendance>();
            if (dao == null)
            {
                return intRet;
            }
            else
            {
                return dao.ModifyAttdRecord(sample);
            }
        }

    }
}
