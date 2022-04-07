using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.root.dac;
using System.Data.SqlClient;
using System.Data;
using dcl.svr.result;
using lis.dto.Entity;

using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.wcf
{
    public class CuvetteShelfRegisterService : WCFServiceBase, ICuvetteShelfRegister
    {
        #region ICuvetteShelfRegister 成员

        public System.Data.DataTable GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo)
        {
            BCCuvetteShelfRegisterBIZ biz = new BCCuvetteShelfRegisterBIZ();
            return biz.GetCuvetteShelfInfo(receviceDeptID, regDateFrom, regDateTo, shelfNoFrom, shelfNoTo, seqFrom, seqTo);
        }

        public List<EntityOperationResult> SavePatients(EntityRemoteCallClientInfo caller, List<dcl.pub.entities.EntityShelfSampleToPatients> listEntity)
        {
            BCCuvetteShelfRegisterBIZ biz = new BCCuvetteShelfRegisterBIZ();
            return biz.SavePatients(caller, listEntity);
        }


        //public DataTable GetInstrmtCom(string itr_id)
        //{
        //    BCCuvetteShelfRegisterBIZ biz = new BCCuvetteShelfRegisterBIZ();
        //    return biz.GetInstrmtCom(itr_id);
        //}

        #endregion
    }
}
