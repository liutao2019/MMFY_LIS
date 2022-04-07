using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.result
{
    public class PatientRecheckBIZ : IPatientRecheck
    {
        public bool DeletePatientRecheck(EntityPatientRecheck recheck)
        {
            bool result = false;
            IDaoPatientRecheck recheckDao = DclDaoFactory.DaoHandler<IDaoPatientRecheck>();
            if (recheckDao != null)
            {
                result = recheckDao.DeletePatientRecheck(recheck);
            }
            return result;
        }

        public bool InsertPatientRecheck(EntityPatientRecheck recheck)
        {
            bool result = false;
            IDaoPatientRecheck recheckDao = DclDaoFactory.DaoHandler<IDaoPatientRecheck>();
            if (recheckDao != null)
            {
                result = recheckDao.InsertPatientRecheck(recheck);
            }
            return result;
        }

        public bool RecheckResultItem(string repId, List<EntityObrResult> listResult)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(repId))
            {
                #region 修改病人复查标志
                EntityPidReportMain patient = new EntityPidReportMain();
                patient.RepId = repId;
                patient.RepRecheckFlag = 1;
                result = new PidReportMainBIZ().UpdateRepRecheckFlag(patient);
                #endregion

                if (result)
                {
                    try
                    {
                        foreach (EntityObrResult obrResult in listResult)
                        {
                            #region 是否在病人复查表中插入数据
                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_SendItmResRecheck") == "是")
                            {
                                EntityPatientRecheck recheck = new EntityPatientRecheck();
                                recheck.ChkExp = string.Empty;
                                recheck.ChkItmId = obrResult.ItmId;
                                recheck.ChkPatId = repId;
                                recheck.ChkResChr = obrResult.ObrValue;
                                recheck.ChkFlag = 0;

                                result = DeletePatientRecheck(recheck);
                                if (result)
                                {
                                    result = InsertPatientRecheck(recheck);
                                }
                            }
                            #endregion

                            #region 更新Obr_Result的复查标志

                            if (obrResult.ObrSn != 0)
                                new ObrResultBIZ().UpdateRecheckFalgByObrSn(obrResult.ObrSn.ToString());
                            #endregion
                        }
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
