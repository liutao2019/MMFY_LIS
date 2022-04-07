using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using dcl.svr.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.tools
{
    public class MergeResultBIZ : IMergeResult
    {
        public List<EntityObrResult> GetMergeResult(EntityMergeResultQC qc)
        {
            IDaoMergeResult dao = DclDaoFactory.DaoHandler<IDaoMergeResult>();

            List<EntityObrResult> listObrResult = new List<EntityObrResult>();
            if (dao != null)
            {
                listObrResult = dao.GetMergeResult(qc);
            }

            return listObrResult;
        }

        public List<EntityObrResult> GetSourceResult(EntityMergeResultQC qc)
        {
            IDaoMergeResult dao = DclDaoFactory.DaoHandler<IDaoMergeResult>();

            List<EntityObrResult> listObrResult = new List<EntityObrResult>();
            if (dao != null)
            {
                listObrResult = dao.GetSourceResult(qc);
            }

            return listObrResult;
        }

        public string MergeResult(EntityMergeResultQC source, EntityMergeResultQC target, bool BarcodeRelation, bool InNoRelation)
        {
            string strMsg = string.Empty;

            IDaoMergeResult dao = DclDaoFactory.DaoHandler<IDaoMergeResult>();

            if (dao != null)
            {
                List<EntityObrResult> listUpdate = new List<EntityObrResult>();
                List<EntityObrResult> listInsert = new List<EntityObrResult>();

                //查询出原数据
                //List<EntityObrResult> listSourceResult = dao.GetMergeResult(source);
                List<EntityObrResult> listSourceResult = dao.GetSourceResult(source);

                if (BarcodeRelation)//条码号匹配，暂不实现
                {
                    //查询出原病人资料
                    List<EntityPidReportMain> listSourcePatients = dao.GetMergePatients(source);
                }
                else
                {
                    //查询出目标数据
                    List<EntityObrResult> listTargetResult = dao.GetMergeResult(target);

                    //查询出目标病人资料
                    List<EntityPidReportMain> listTargetPatients = dao.GetMergePatients(target);

                    int index = 0;
                    for (long i = source.IdStart; i <= source.IdEnd; i++)
                    {
                        //计算目标索引
                        string TargetIdIndex = (target.IdStart + index).ToString();

                        //计算目标索引
                        string SourceIdIndex = (source.IdStart + index).ToString();

                        index += 1;

                        List<EntityObrResult> listResult = new List<EntityObrResult>();//本次需要合并的结果

                        int TargetPatintsIndex = -1;//目标病人资料索引

                        //拿出对应目标的结果
                        if (source.MatchMode == "0")
                        {
                            listResult = listSourceResult.FindAll(w => w.ObrSid == SourceIdIndex);
                        }
                        else
                        {
                            listResult = listSourceResult.FindAll(w => w.RepSerialNum == SourceIdIndex);
                        }
                        //拿出对应病人资料索引
                        if (target.MatchMode == "0")
                        {
                            TargetPatintsIndex = listTargetPatients.FindIndex(w => w.RepSid == TargetIdIndex);
                        }
                        else
                        {
                            TargetPatintsIndex = listTargetPatients.FindIndex(w => w.RepSerialNum == TargetIdIndex);
                        }
                        //存在病人资料，检查标本状态
                        if (TargetPatintsIndex > -1)
                        {
                            //已审核、报告，不允许合并
                            if (listTargetPatients[TargetPatintsIndex].RepStatus.Value > 0)
                            {
                                strMsg += string.Format("目标{1}[{0}]已审核\r\n", TargetIdIndex, source.MatchMode == "0" ? "样本号" : "序号");
                                continue;
                            }
                        }

                        if (listResult.Count > 0)
                        {
                            foreach (EntityObrResult item in listResult)
                            {
                                int TargetResultIndex = -1;//目标是否存在该项目结果

                                if (target.MatchMode == "0")
                                    TargetResultIndex = listTargetResult.FindIndex(w => w.ObrSid == TargetIdIndex && w.ItmId == item.ItmId);
                                else if (target.MatchMode == "1")
                                    TargetResultIndex = listTargetResult.FindIndex(w => w.RepSerialNum == TargetIdIndex && w.ItmId == item.ItmId);

                                //存在目标结果，更新
                                if (TargetResultIndex > -1)
                                {
                                    EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(listTargetResult[TargetResultIndex]);
                                    obrResult.ObrValue = item.ObrValue;
                                    obrResult.ObrValue2 = item.ObrValue2;
                                    listUpdate.Add(obrResult);
                                }
                                else//不存在则新增
                                {
                                    //样本号
                                    if (target.MatchMode == "0")
                                    {
                                        EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(item);
                                        obrResult.ObrSid = TargetIdIndex;
                                        obrResult.ObrItrId = target.RepItrId.Trim();
                                        obrResult.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                                        obrResult.ObrFlag = 1;
                                        obrResult.ObrId = string.Format("{0}{1}{2}", obrResult.ObrItrId, target.ObrDate.ToString("yyyyMMdd"), obrResult.ObrSid);
                                        listInsert.Add(obrResult);
                                    }
                                    //序号，必须存在病人资料才新增
                                    else if (TargetPatintsIndex > -1)
                                    {
                                        EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(item);
                                        obrResult.ObrSid = listTargetPatients[TargetPatintsIndex].RepSid;
                                        obrResult.ObrItrId = listTargetPatients[TargetPatintsIndex].RepItrId;
                                        obrResult.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                                        obrResult.ObrFlag = 1;
                                        obrResult.ObrId = listTargetPatients[TargetPatintsIndex].RepId;
                                        listInsert.Add(obrResult);
                                    }
                                }
                            }
                        }
                    }

                    #region 旧业务逻辑
                    ////排序
                    //if (source.MatchMode == "0")
                    //    listSourceResult = listSourceResult.OrderBy(w => Convert.ToInt64(w.ObrSid)).ToList();
                    //else
                    //    listSourceResult = listSourceResult.OrderBy(w => Convert.ToInt64(w.RepSerialNum)).ToList();

                    ////计算出原始ID起始号差异数量
                    //long DifferenceNum = Convert.ToInt64(target.IdStart) - Convert.ToInt64(source.IdStart);

                    //foreach (EntityObrResult item in listSourceResult)
                    //{
                    //    int resultIndex = -1;//目标结果索引
                    //    int patientIndex = -1;//目标病人资料索引

                    //    string targetId = "0";//目标号（样本号、序号）

                    //    //样本号匹配
                    //    if (target.MatchMode == "0")
                    //    {
                    //        targetId = (Convert.ToInt64(item.ObrSid) + DifferenceNum).ToString();
                    //        resultIndex = listTargetResult.FindIndex(w => w.ObrSid == targetId && w.ItmId == item.ItmId);
                    //    }
                    //    //序号匹配
                    //    else
                    //    {
                    //        targetId = (Convert.ToInt64(item.RepSerialNum) + DifferenceNum).ToString();
                    //        patientIndex = listTargetPatients.FindIndex(w => w.RepSerialNum == targetId);
                    //        resultIndex = listTargetResult.FindIndex(w => w.RepSerialNum == targetId && w.ItmId == item.ItmId);
                    //    }
                    //    //存在目标数据，则更新
                    //    if (resultIndex > -1)
                    //    {
                    //        EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(listTargetResult[resultIndex]);
                    //        if (obrResult.RepStatus == 0)
                    //        {
                    //            obrResult.ObrValue = item.ObrValue;
                    //            obrResult.ObrValue2 = item.ObrValue2;
                    //            listUpdate.Add(obrResult);
                    //        }
                    //        else
                    //        {
                    //            if (source.MatchMode == "0")
                    //                strMsg += string.Format("目标样本号[{0}]已审核\r\n", targetId);
                    //            if (source.MatchMode == "1")
                    //                strMsg += string.Format("目标序号[{0}]已审核\r\n", targetId);
                    //        }
                    //    }
                    //    //不存在则新增
                    //    else
                    //    {
                    //        //样本号
                    //        if (target.MatchMode == "0")
                    //        {
                    //            EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(item);
                    //            obrResult.ObrSid = (Convert.ToInt64(item.ObrSid) + DifferenceNum).ToString();
                    //            obrResult.ObrItrId = target.RepItrId.Trim();
                    //            obrResult.ObrDate = target.ObrDate;
                    //            obrResult.ObrFlag = 1;
                    //            obrResult.ObrId = string.Format("{0}{1}{2}", obrResult.ObrItrId, obrResult.ObrDate.ToString("yyyyMMdd"), obrResult.ObrSid);
                    //            listInsert.Add(obrResult);
                    //        }
                    //        //序号，必须存在病人资料才新增
                    //        else if (patientIndex > -1)
                    //        {
                    //            EntityObrResult obrResult = EntityManager<EntityObrResult>.EntityClone(item);
                    //            obrResult.ObrSid = listTargetPatients[patientIndex].RepSid;
                    //            obrResult.ObrItrId = listTargetPatients[patientIndex].RepItrId;
                    //            obrResult.ObrDate = target.ObrDate;
                    //            obrResult.ObrFlag = 1;
                    //            obrResult.ObrId = listTargetPatients[patientIndex].RepId;
                    //            listInsert.Add(obrResult);
                    //        }
                    //    }
                    //}
                    #endregion

                }


                ObrResultBIZ resultBIZ = new ObrResultBIZ();
                if (listInsert.Count > 0)
                {
                    foreach (var item in listInsert)
                    {
                        resultBIZ.InsertObrResult(item, false);
                    }
                }
                if (listUpdate.Count > 0)
                {
                    foreach (var item in listUpdate)
                    {
                        resultBIZ.UpdateObrResult(item);
                    }
                }
            }

            return strMsg;
        }


    }
}
