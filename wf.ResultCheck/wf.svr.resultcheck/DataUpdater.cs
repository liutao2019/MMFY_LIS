using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.dao.core;

namespace dcl.svr.resultcheck
{
    public class DataUpdater
    {
        EntityPidReportMain patients;
        List<EntityPidReportDetail> patients_mi;
        List<EntityObrResult> resulto;

        public DataUpdater(EntityPidReportMain pat_info
                                    , List<EntityPidReportDetail> patients_mi
                                    , List<EntityObrResult> resulto
                                    , EnumOperationCode auditType
                                    , AuditConfig config)
        {
            this.patients = pat_info;
            this.patients_mi = patients_mi;
            this.resulto = resulto;
        }

        public void Update(ref EntityOperationResult chkResult)
        {
            DBManager helper = new DBManager();

            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                mainDao.Dbm = helper;
                helper.BeginTrans();
                try
                {
                    //更新病人资料
                    mainDao.UpdatePatientData(patients);

                    //更新结果
                    IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                    resultDao.Dbm = helper;
                    if (resultDao != null)
                    {
                        foreach (EntityObrResult res in this.resulto)
                        {
                            if (res.NeedDelete)
                            {
                                resultDao.DeleteObrResultByObrSn(res.ObrSn.ToString());
                            }
                            else
                            {
                                resultDao.UpdateObrResult(res);
                            }
                        }
                    }

                    //更新组合信息
                    IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                    detailDao.Dbm = helper;
                    if (detailDao != null)
                    {
                        foreach (EntityPidReportDetail pat_mi in this.patients_mi)
                        {
                            if (string.IsNullOrEmpty(pat_mi.OrderPrice))
                                pat_mi.OrderPrice = null;
                            detailDao.UpdatePidReportDetailInfo(pat_mi);
                        }
                    }

                    helper.CommitTrans();
                }
                catch (Exception ex)
                {
                    helper.RollbackTrans();
                    Lib.LogManager.Logger.LogException(string.Format("更新病人信息失败,病人id = {0}", patients.RepId), ex);
                    throw new Exception("更新病人信息失败");
                }
                finally
                {
                    //强制销毁
                    helper = null;
                }
            }
        }

        public void Update(List<EntityObrResult> obrResultList)
        {
            DBManager helper = new DBManager();

            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                mainDao.Dbm = helper;
                helper.BeginTrans();
                try
                {
                    //更新结果
                    IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                    resultDao.Dbm = helper;
                    if (resultDao != null)
                    {
                        foreach (EntityObrResult res in obrResultList)
                        {
                            if (res.NeedDelete)
                            {
                                resultDao.DeleteObrResultByObrSn(res.ObrSn.ToString());
                            }
                            else
                            {
                                resultDao.UpdateObrResult(res);
                            }
                        }
                    }
                    helper.CommitTrans();
                }
                catch (Exception ex)
                {
                    helper.RollbackTrans();
                    Lib.LogManager.Logger.LogException(string.Format("更新病人信息失败,病人id = {0}", patients.RepId), ex);
                    throw new Exception("更新病人信息失败");
                }
                finally
                {
                    //强制销毁
                    helper = null;
                }
            }
        }
    }
}
