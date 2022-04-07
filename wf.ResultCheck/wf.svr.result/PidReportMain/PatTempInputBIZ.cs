using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;


using System.Data;
using dcl.svr.cache;
using dcl.svr.dicbasic;

namespace dcl.svr.result
{
    public class PatTempInputBIZ : IPatTempInput
    {
        public List<entity.EntityPidReportMain> GetPatientsDetail(DateTime dtFrom, DateTime dtTo, string type_id, string itr_id, string sid)
        {
            string sqlItrID_IN = string.Empty;
            DataTable dtPat = new DataTable();
            List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
            //ananlyseQc.NumRange = sid;     1-5,7,9-15
            List<EntitySid> listSid = ConvertStringSidToListSid.GetListSid(sid);
            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.DateStart = dtFrom;
            patientQc.DateEnd = dtTo;
            patientQc.ListSidRange = listSid;
            if (itr_id == string.Empty || itr_id == null)
            {
                List<EntityDicInstrument> listInst = new List<EntityDicInstrument>();
                listInst = new InstrmtBIZ().GetInstrumentByItridOrItrType(itrType: type_id);
                StringBuilder sb = new StringBuilder();
                foreach (EntityDicInstrument dr in listInst)
                {
                    patientQc.ListItrId.Add(dr.ItrId);
                }
            }
            else
            {
                patientQc.ListItrId.Add(itr_id);
            }

            listPat = new PidReportMainBIZ().PatientQuery(patientQc);

            foreach (EntityPidReportMain drPat in listPat)
            {
                if (drPat.PidAgeExp != null)
                {
                    string patage = drPat.PidAgeExp.ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    drPat.PatAgeTxt = patage;
                }

                if (drPat.RepStatus != null && !string.IsNullOrEmpty(drPat.RepStatus.Value.ToString()))
                {
                    string patflag = drPat.RepStatus.Value.ToString();
                    if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        drPat.RepStatusName = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        drPat.RepStatusName = "已打印";
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        drPat.RepStatusName = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                    {
                        drPat.RepStatusName = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                }
                else
                {
                    drPat.RepStatusName = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                }

                //判断是否有检验结果
                List<EntityObrResult> listResult = new ObrResultBIZ().ObrResultQueryByObrId(drPat.RepId);
                if (listResult.Count > 0)
                {
                    drPat.HasResult = "1";
                }
                else
                {
                    drPat.HasResult = "0";
                }

                if (drPat.HasResult == "1" && drPat.RepStatus.Value.ToString() == "0")
                {
                    IDaoDicCombineDetail comDetailDao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                    List<EntityDicCombineDetail> listComDetail = new List<EntityDicCombineDetail>();
                    if (comDetailDao != null)
                    {
                        listComDetail = comDetailDao.Search(new List<string> { drPat.RepId.ToString() });
                    }
                    if (listComDetail.Count > 0)
                    {
                        drPat.HasResult = "2";
                    }
                }
            }
            return listPat;
        }

        public EntityOperateResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, EntityQcResultList resultList)
        {
            EntityOperateResult opResult = new EntityOperateResult();//.GetNew("保存病人、普通结果信息");

            EntityPidReportMain patient = resultList.patient;//病人资料
            //List<EntityPidReportDetail> listRepDetail = resultList.listRepDetail;//病人组合明细
            List<EntityObrResult> listResult = resultList.listResulto;//病人结果


            Dictionary<string, List<EntityObrResult>> dsResult = GetCommonResultInsertCMD(listResult, opResult, patient);
            opResult = new PidReportMainBIZ().SavePatient(caller, patient);
            //插入检验结果
            if (dsResult != null && opResult.Success)
            {
                try
                {
                    ObrResultBIZ resultBiz = new ObrResultBIZ();
                    //更新检验结果列表
                    List<EntityObrResult> updateList = dsResult["updateList"] as List<EntityObrResult>;
                    foreach (EntityObrResult item in updateList)
                    {
                        resultBiz.UpdateObrResultByObrIdAndObrItmId(item);
                    }
                    //插入检验结果列表
                    List<EntityObrResult> insertList = dsResult["insertList"] as List<EntityObrResult>;
                    foreach (EntityObrResult item in insertList)
                    {
                        resultBiz.InsertObrResult(item);
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            DateTime dtToday = ServerDateTime.GetDatabaseServerDateTime();
            string barcode = patient.RepBarCode;
            new TatProRecordBIZ().TatRecode("20", barcode, dtToday.ToString("yyyy-MM-dd HH:mm:ss"));

            return opResult;
        }

        /// <summary>
        /// 保存普通结果
        /// </summary>
        /// <param name="dtPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private Dictionary<string, List<EntityObrResult>> GetCommonResultInsertCMD(List<EntityObrResult> dtPatientResultInput, EntityOperateResult opResult, entity.EntityPidReportMain patient)
        {
            Dictionary<string, List<EntityObrResult>> ds = new Dictionary<string, List<EntityObrResult>>();
            List<EntityObrResult> dtPatientResult = new List<EntityObrResult>();
            List<EntityObrResult> dtPatientResultUpdate = new List<EntityObrResult>();

            if (opResult.Success && dtPatientResultInput != null)
            {
                string pat_id = patient.RepId;
                string pat_sid = patient.RepSid;
                string pat_itr_id = patient.RepItrId;

                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId.Add(pat_id);

                List<EntityObrResult> dtResultInDB = new ObrResultBIZ().ObrResultQuery(resultQc);
                DateTime today = patient.RepInDate.Value;
                foreach (EntityObrResult row in dtPatientResultInput)
                {
                    //给检验结果统一赋值
                    row.ObrId = pat_id;
                    row.ObrSid = pat_sid;
                    row.ObrItrId = pat_itr_id;
                    row.ObrType = 0;
                    row.ObrDate = today;
                    row.ObrFlag = 1;
                    row.ObrReportType = 0;
                    //if (isVerify)
                    //    drsResult["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(drsResult["res_itm_id"].ToString() + ";" + drsResult["res_chr"].ToString());
                    List<EntityObrResult> rows = dtResultInDB.FindAll(i => i.ItmId == row.ItmId);
                    if (dtResultInDB != null && dtResultInDB.Count > 0 && rows != null && rows.Count > 0) //如果数据库有仪器结果
                    {
                        if (row.ObrValue == null || string.IsNullOrEmpty(row.ObrValue) || rows[0].ObrType.ToString() == "1") //当前项目结果或为仪器结果
                        {

                        }
                        else
                        {
                            dtPatientResultUpdate.Add(row);
                        }
                    }
                    else
                        dtPatientResult.Add(row);
                }

                ds.Add("updateList", dtPatientResultUpdate);
                ds.Add("insertList", dtPatientResult);
            }
            else
            {
                ds.Add("updateList", new List<EntityObrResult>());
                ds.Add("insertList", new List<EntityObrResult>());
            }
            return ds;
        }

        public List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId)
        {
            return new PidReportDetailBIZ().GetPidReportDetailByRepId(repId);
        }
    }
}
