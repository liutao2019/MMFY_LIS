using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.qc;
using dcl.svr.cache;
using dcl.svr.result;

namespace dcl.svr.elisa
{
    public class ElisaAnalyseBIZ : IElisaAnalyse
    {
        public List<EntityObrElisaResult> GetElisaResult(string ItrId, DateTime dtImmDate)
        {
            List<EntityObrElisaResult> ObrResult = new List<EntityObrElisaResult>();
            IDaoElisaAnalyse dao = DclDaoFactory.DaoHandler<IDaoElisaAnalyse>();
            if (dao != null)
            {
                ObrResult = dao.GetElisaResult(ItrId, dtImmDate);
            }
            return ObrResult;
        }

        public List<EntityDicQcConvert> GetQCConvert(string ItrId)
        {
            List<EntityDicQcConvert> QCConvert = new List<EntityDicQcConvert>();
            IDaoElisaAnalyse dao = DclDaoFactory.DaoHandler<IDaoElisaAnalyse>();
            if (dao != null)
            {
                QCConvert = dao.GetQCConvert(ItrId);
            }
            return QCConvert;
        }

        public List<EntityObrQcResult> GetQCResult(string ItrId)
        {
            List<EntityObrQcResult> QCResult = new List<EntityObrQcResult>();
            QcResultBIZ biz = new QcResultBIZ();
            QCResult = biz.GetQcResultByItrId(ItrId);
            return QCResult;
        }

        public bool UpdateResValue(string ResId, string ResValue)
        {
            bool issuccess = false;
            IDaoElisaAnalyse dao = DclDaoFactory.DaoHandler<IDaoElisaAnalyse>();
            if (dao != null)
            {
                issuccess = dao.UpdateResValue(ResId, ResValue);
            }
            return issuccess;
        }

        public EntityResponse UpdateElisaResult(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityObrElisaResult ElisaResult = request.GetRequestValue<EntityObrElisaResult>();
                IDaoElisaAnalyse dao = DclDaoFactory.DaoHandler<IDaoElisaAnalyse>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    if (ElisaResult.ResManuDate <= Convert.ToDateTime("1900-01-01 0:00:00"))
                        ElisaResult.ResManuDate = ServerDateTime.GetDatabaseServerDateTime();
                    if (ElisaResult.ResReagDate <= Convert.ToDateTime("1900-01-01 0:00:00"))
                        ElisaResult.ResReagDate = ServerDateTime.GetDatabaseServerDateTime();

                    response.Scusess = dao.UpdateElisaResult(ElisaResult);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        /// <summary>
        /// 保存批量录入的数据结果
        /// </summary>
        /// <returns></returns>
        public string SaveBatchObrResult(List<EntityObrResult> dtObrResult)
        {
            //保存因已审核而未能保存的样本号序列用于提示
            string notSave = "";
            try
            {
                //移除自增长列
                //dt.Remove("res_key");

                //此处不判断是否审核,并取出项目明细数据,是为了对样本已有记录的项目执行update，否则可能丢失原项目对应的组合信息

                //缓存用于判断是否被审核、数据是Insert或Update的条件表
                ObrResultBIZ resultBIZ = new ObrResultBIZ();

                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId = dtObrResult.Select(i => i.ObrId).ToList();

                List<EntityObrResult> dtPatId = resultBIZ.ObrResultQuery(resultQc);
                CacheDataBIZ CacheBIZ = new CacheDataBIZ();
                List<EntityDicInstrument> itrType = new List<EntityDicInstrument>();
                EntityResponse response = CacheBIZ.GetCacheData("EntityDicInstrument");
                itrType = response.GetResult() as List<EntityDicInstrument>;
                itrType = itrType.Where(i => i.ItrId == dtObrResult[0].ObrItrId).ToList();

                if (itrType[0].ItrReportType.ToString() == "3")
                {
                    foreach (EntityObrResult item in dtObrResult)
                    {
                        string thisResId = item.ObrId.ToString();
                        if (dtPatId.Where(i => i.RepId == thisResId && i.RepStatus > 0).ToList().Count < 1)
                        {
                            EntityObrResultDesc ResultDesc = new EntityObrResultDesc();
                            ResultDesc.ObrId = thisResId;
                            ResultDesc.ObrItrId = item.ObrItrId;
                            ResultDesc.ObrDate = DateTime.Now;
                            ResultDesc.ObrSid = Convert.ToDecimal(item.ObrSid);
                            ResultDesc.ObrValue = item.ObrValue;
                            ResultDesc.SortNo = 1;
                            ResultDesc.ObrPositiveFlag = 0;
                            ObrResultDescBIZ ResultDescBIZ = new ObrResultDescBIZ();
                            List<EntityObrResultDesc> listResultDesc = new List<EntityObrResultDesc>();
                            listResultDesc.Add(ResultDesc);
                            ResultDescBIZ.InsertObrResultDesc(listResultDesc);
                        }
                        else
                        {
                            //报告已被审核,记录并不做任何数据操作
                            string sid = item.ObrSid.ToString();
                            if (notSave.IndexOf("," + sid + " ") == -1)
                            {
                                notSave += "," + sid + " ";
                            }
                        }
                    }
                }
                else
                {
                    //生成Insert语句备用,部分会被手工的Update替换
                    //ArrayList arrNew = dao.GetInsertSql(dt);

                    for (int i = 0; i < dtObrResult.Count; i++)
                    {
                        //取得res_id来判断报告单是否已被审核,arrNew的Index与dt是对应的
                        EntityObrResult result = dtObrResult[i];
                        string thisResId = result.ObrId.ToString();

                        string thisItmId = string.Empty;
                        if (!string.IsNullOrEmpty(result.ItmId))
                            thisItmId = result.ItmId;

                        string thisChar = string.Empty;
                        if (result.ObrValue != null)
                            thisChar = result.ObrValue.ToString().Replace("'", "''");

                        string thisCastChar = string.Empty;
                        if (result.ObrConvertValue != null)
                            thisCastChar = result.ObrConvertValue.Value.ToString();
                        else
                        {
                            //数值结果 不可为空
                            foreach (var info in dtPatId)
                            {
                                if (info.ObrId == result.ObrId && info.ItmId == result.ItmId && info.ItmComId == result.ItmComId && info.ObrConvertValue != null)
                                    result.ObrConvertValue = info.ObrConvertValue;
                            }
                        }

                        string thisODChar = result.ObrValue2;
                        string thisDate = result.ObrDate.ToString();
                        string thisRepType = result.ObrReportType.ToString();
                        string thisRepFlag = result.RefFlag;
                        if (thisCastChar == "")
                        {
                            thisCastChar = "null";
                        }

                        //pat_id为DBNull的数据也适用于此条件
                        if (dtPatId.Where(w => w.RepId == thisResId && w.RepStatus > 0).ToList().Count < 1)
                        {
                            //未被审核或报告的处理
                            if (dtPatId.Where(w => w.ObrId == thisResId && w.ItmId == thisItmId).ToList().Count < 1)
                            {
                                //Insert数据
                                resultBIZ.InsertObrResult(result);
                            }
                            else
                            {
                                //Update数据
                                resultBIZ.UpdateObrResultByObrIdAndObrItmId(result);
                            }
                        }
                        else
                        {
                            //报告已被审核,记录并不做任何数据操作
                            string sid = result.ObrSid.ToString();
                            if (notSave.IndexOf("," + sid + " ") == -1)
                            {
                                notSave += "," + sid + " ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("保存数据出错!", ex.ToString());
                return ex.ToString();
            }

            //返回因审核未保存的结果
            if (notSave != "")
            {
                notSave = "以下样本号已经审核，不能更新测定结果:\r\n" + notSave.Substring(1); ;
            }

            return notSave;
        }


        public bool SaveQcValue(List<EntityElisaQc> dtEiasa)
        {
            DataSet result = new DataSet();
            try
            {
                bool blQcData = CacheSysConfig.Current.GetSystemConfig("Eiasa_QcData") == "是";

                foreach (EntityElisaQc drEiasa in dtEiasa)
                {
                    QcMateriaBIZ materiaBiz = new QcMateriaBIZ();

                    List<EntityDicQcMateria> dtQcInfo = materiaBiz.GetMatSn(drEiasa.QcItrId, drEiasa.QcNoType, drEiasa.QcItmId);
                    foreach (EntityDicQcMateria drQcInfo in dtQcInfo)
                    {
                        QcResultBIZ QcResultBIZ = new QcResultBIZ();
                        QcResultBIZ.InsertQcResult(drEiasa.QcItrId, drEiasa.QcItmId, drEiasa.QcValue,
                                                           drEiasa.QcNoType, drQcInfo.MatSn,
                                                           blQcData ? drEiasa.QcDate : DateTime.Now);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
                return false;
            }
        }
    }
}
