using System;
using System.Collections.Generic;
using dcl.common;
using dcl.root.logon;
using dcl.servececontract;
using dcl.entity;

namespace dcl.svr.result
{
    public class ObrResultMergeBIZ: IObrResultMerge
    {



        public List<EntityOperateResult> Merge(List<EntityPidReportMain> listPat,bool IsCopy)
        {
            List<EntityOperateResult> listResult = new List<EntityOperateResult>();

            foreach (EntityPidReportMain entityData in listPat)
            {
                if (!Compare.IsEmpty(entityData.DestRepSid))
                {
                    //源结果样本号
                    string src_pat_sid = entityData.DestRepSid;

                    //源结果时间
                    DateTime src_res_date = Convert.ToDateTime(entityData.DestRepInDate);

                    //源仪器ID
                    string src_itr_id = entityData.DestRepItrId;

                    //源病人ID
                    string src_pat_id = entityData.DestRepId;

                    //目标病人样本号
                    string dest_res_sid = entityData.RepSid;

                    //目标病人ID
                    string dest_pat_id = entityData.RepId;

                    //目标仪器ID
                    string dest_pat_itr_id = entityData.RepItrId;

                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.RepSid = src_pat_sid;
                    if (!string.IsNullOrEmpty(src_pat_id))
                        resultQc.ListObrId.Add(src_pat_id);
                    resultQc.ItrId = src_itr_id;
                    resultQc.StartObrDate = src_res_date.Date.ToString("yyyy-MM-dd 00:00:00");
                    resultQc.EndObrDate = src_res_date.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00");
                    //获取源结果数据
                    List<EntityObrResult> listSource = new ObrResultBIZ().ObrResultQuery(resultQc);


                    //过滤只合并勾选了的项目id
                    if (listSource != null && listSource.Count > 0 
                        &&!entityData.DestAllItem)
                    {
                        List<string> dest_itm_ids = entityData.DestItmIds;
                        if (dest_itm_ids.Count <= 0)
                            continue;

                        List<EntityObrResult> listSourceCopy = new List<EntityObrResult>();
                        listSourceCopy = listSource;

                        listSource = new List<EntityObrResult>();

                        foreach (EntityObrResult entitySourceCopy in listSourceCopy)
                        {
                            if (dest_itm_ids.Contains(entitySourceCopy.ItmId))
                            {
                                listSource.Add(entitySourceCopy);
                            }
                        }

                        if (listSource.Count <= 0)//如果没有选择要合并的项目,则不往下执行
                            continue;
                    }

                    //删除目标结果数据中的重复结果
                    List<string> listItmIds = new List<string>();
                    foreach (EntityObrResult entitySource in listSource)
                    {
                        listItmIds.Add(entitySource.ItmId);
                        entitySource.ObrId = dest_pat_id;
                        entitySource.ObrSid= dest_res_sid;
                        entitySource.ObrItrId= dest_pat_itr_id;
                    }
                    EntityResultQC qc = new EntityResultQC();
                    qc.ListObrId.Add(dest_pat_id);
                    qc.listItmIds = listItmIds;
                    try
                    {
                        //更新目标结果数据中的重复结果为无效结果
                        bool updateResult=new ObrResultBIZ().UpdateObrFlagByCondition(qc);
                        if (updateResult)
                        {
                            if (IsCopy)
                            {
                                //复制
                                foreach (EntityObrResult result in listSource)
                                {
                                    new ObrResultBIZ().InsertObrResult(result);
                                }
                            }
                            else
                            {
                                //合并
                                foreach (EntityObrResult result in listSource)
                                {
                                    new ObrResultBIZ().UpdateObrResult(result);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, "Merge", ex.ToString());
                        //throw;
                    }

                }
            }

            return listResult;
        }

        /// <summary>
        /// 获取仪器未审核纪录的病人列表
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        List<EntityPidReportMain> IObrResultMerge.GetCurrentItrPatientList(EntityPatientQC patientQc)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            listPat = new PidReportMainBIZ().PatientQuery(patientQc);
            return listPat;
              
        }

        List<EntityObrResult> IObrResultMerge.GetNonePatInfoResult(EntityResultQC resultQc)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            listResult = new ObrResultBIZ().ObrResultQuery(resultQc);
            return listResult;
        }
    }
}
