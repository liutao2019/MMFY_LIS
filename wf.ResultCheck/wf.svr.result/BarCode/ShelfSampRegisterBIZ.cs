using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.svr.sample;
using dcl.svr.dicbasic;

namespace dcl.svr.result
{
    /// <summary>
    /// 试管条码病人登记
    /// </summary>
    public class ShelfSampRegisterBIZ : IShelfSampRegister
    {
        public EntityResponse SavePatients(EntityRemoteCallClientInfo caller, List<EntityShelfSampToReportMain> listEntity)
        {
            List<EntityOperateResult> ret = new List<EntityOperateResult>();
            PidReportMainBIZ reportMain = new PidReportMainBIZ();

            foreach (EntityShelfSampToReportMain entity in listEntity)
            {
                string barcode = entity.SampBarCode;
                EntityPidReportMain patient = reportMain.GetPatientsByBarCode(barcode);
                if (patient != null && !string.IsNullOrEmpty(patient.RepBarCode))
                {
                    patient.RepItrId = entity.ItrId;
                    patient.ItrName = entity.ItrName;
                    patient.RepSid = entity.RepSid;
                    patient.RepInDate = entity.RepInDate;
                    patient.SampCheckDate = entity.SampCheckDate;
                    patient.RepSerialNum = entity.RepSerialNum;
                    patient.RepCheckUserId = entity.RepCheckUserId;

                    List<EntityPidReportDetail> listCombine = patient.ListPidReportDetail;

                    if (listCombine != null && listCombine.Count > 0)
                    {
                        for (int i = listCombine.Count - 1; i >= 0; i--)
                        {
                            string pat_com_id = listCombine[i].ComId;

                            if (entity.ComIds.Find(item => item == pat_com_id) == null)
                            {
                                listCombine.RemoveAt(i);
                            }
                        }
                    }

                    EntityOperateResult opresult = new PidReportMainBIZ().SavePatient(caller, patient);
                    try
                    {
                        if (opresult.Success)
                        {
                            foreach (string bc_id in entity.DetSns)
                            {
                                new SampDetailBIZ().UpdateSampFlagByDetSn(bc_id);
                            }

                            ret.Add(opresult);
                        }
                        else
                        {
                            ret.Add(opresult);
                        }
                    }
                    catch (Exception ex)
                    {
                        opresult.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                    }
                }
            }
            EntityResponse respone = new EntityResponse();
            respone.SetResult(ret);
            return respone;
        }

        public List<EntitySampRegister> GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo)
        {
            List<EntitySampRegister> listSamp = new List<EntitySampRegister>();
            SampRegisterBIZ samp = new SampRegisterBIZ();
            listSamp = samp.GetCuvetteShelfInfo(receviceDeptID, regDateFrom, regDateTo, shelfNoFrom, shelfNoTo, seqFrom, seqTo);
            return listSamp;
        }

        public List<EntityDicItrCombine> GetItrCombine(string itrId, bool getMergeItrCombine)
        {
            List<EntityDicItrCombine> list = new List<EntityDicItrCombine>();
            list = new InstrmtComBIZ().GetItrCombine(itrId, getMergeItrCombine);
            return list;
        }
    }
}
