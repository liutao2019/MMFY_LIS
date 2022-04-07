using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using dcl.servececontract;
using dcl.svr.cache;

namespace dcl.svr.dicbasic
{
    public class InstrmtComBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                object objCombine = dict["ItrComIn"];
                List<EntityDicItrCombine> combine = new List<EntityDicItrCombine>();
                if (objCombine != null)
                {
                    combine = objCombine as List<EntityDicItrCombine>;
                }

                IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
                int k = 0;
                LogDbLOG(dict);
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    bool idTrue = dao.Delete(combine[0]);//因为List数据的itr_id都是一样的所以只需要传一个就好
                    foreach (EntityDicItrCombine info in combine)
                    {
                        if (dao.Save(info))
                        {
                            k++;
                        }
                    }
                    if (k == combine.Count && idTrue)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(combine);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItrCombine combine = request.GetRequestValue<EntityDicItrCombine>();
                //sample.DelFlag = "1";
                IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    //response.Scusess = dao.Delete(combine);
                    response.Scusess = dao.Delete(combine);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItrCombine combine = request.GetRequestValue<EntityDicItrCombine>();
                IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(combine);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            string itrID = request.GetRequestValue<string>();
            IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {

                EntityDicItrCombine info = new EntityDicItrCombine();
                info.ItrId = itrID;
                List<EntityDicItrCombine> combineIn = dao.Search(info);
                info.IsNotIn = true;
                List<EntityDicItrCombine> combineNotIn = dao.Search(info);

                List<List<EntityDicItrCombine>> list = new List<List<EntityDicItrCombine>>();

                list.Add(combineIn);

                list.Add(combineNotIn);

                response.SetResult(list);

                response.Scusess = true;
            }
            return response;
        }
        public List<EntityDicItrCombine> GetItrCombine(string itrId, bool getMergeItrCombine)
        {
            List<EntityDicItrCombine> listCombine = new List<EntityDicItrCombine>();
            List<EntityDicInstrument> list = DictInstrmtCache.Current.DclCache;
            if (getMergeItrCombine)
            {
                //查找所有"存储仪器"为当前仪器的仪器
                List<EntityDicInstrument> listConItr = list.Where(w => w.ItrReportItrId == itrId).ToList();
                string[] sqlItrCombineIn = new string[listConItr.Count];
                if (listConItr.Count > 0)
                {
                    for (int i = 0; i < listConItr.Count; i++)
                    {
                        sqlItrCombineIn[i] = listConItr[i].ItrId;
                    }
                }

                List<EntityDicItrCombine> listItrCombine = new List<EntityDicItrCombine>();
                IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
                if (dao != null)
                {
                    listItrCombine = dao.Search("cache");
                }
                List<EntityDicItrCombine> drs = (from x in listItrCombine where sqlItrCombineIn.Contains(x.ItrId) select x).ToList();

                foreach (EntityDicItrCombine dr in drs)
                {
                    if (!Compare.IsEmpty(dr.ComId))
                    {
                        listCombine.Add(dr);
                    }
                }

            }
            return listCombine;
        }
        public EntityResponse Other(EntityRequest request)
        {
            return new InstrmtBIZ().Search(request);
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(null));//查询仪器包含组合数据
            }
            return response;
        }

        private void LogDbLOG(Dictionary<string, object> dict)
        {
            EntitySysOperationLog dtOperation = (EntitySysOperationLog)dict["Operation"];
            //DataTable _LOGINFO = dsDataPack.Tables["_LOGINFO"];

            List<EntityDicItrCombine> listNewDicItrCombine = (List<EntityDicItrCombine>)dict["ItrComIn"];

            EntityResponse response = new EntityResponse();
            string old_itr_id = listNewDicItrCombine[0].ItrId;
            //string new_com_name = listNewDicItrCombine[0].;
            //string new_com_ecd = listNewDicItrCombine[0].ComCode;

            string operationName = dtOperation.OperatAction;
            string loginID = dtOperation.OperatUserId;
            string userName = dtOperation.OperatUserName;
            string ipAdress = IPUtility.GetIP();

            if (operationName == "修改")
            {
                DateTime dtNow = ServerDateTime.GetDatabaseServerDateTime();

                #region 比较组合明细信息
                //比较组合明细信息 
                if (listNewDicItrCombine.Count > 0)
                {
                    List<EntityDicItrCombine> listOldDictItrCombine = new List<EntityDicItrCombine>();
                    EntityRequest req = new EntityRequest();
                    req.SetRequestValue("SearchAllData");

                    EntityResponse result = Search(req);

                    List<List<EntityDicItrCombine>> listAllCombine = new List<List<EntityDicItrCombine>>();
                    listAllCombine = result.GetResult() as List<List<EntityDicItrCombine>>;
                    listOldDictItrCombine = listAllCombine[0]; //包含组合(全部)
                    if (listOldDictItrCombine != null && listOldDictItrCombine.Count > 0)
                    {
                        listOldDictItrCombine = listOldDictItrCombine.Where(w => w.ItrId == old_itr_id).ToList();
                        string detailComId = string.Empty;
                        if (listOldDictItrCombine.Count > listNewDicItrCombine.Count)
                        {
                            List<EntityDicItrCombine> itrResult = listOldDictItrCombine.Where(p => !listNewDicItrCombine.Any(p2 => p2.ComId == p.ComId)).ToList();
                            if (itrResult.Count > 0)
                            {
                                foreach (EntityDicItrCombine detail in itrResult)
                                {
                                    detailComId += string.Format(",{0}", detail.ComId);
                                }
                                detailComId= detailComId.Remove(0, 1);
                                new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "仪器组合", "仪器组合明细", "删除", "已包含组合", "移除组合id" + detailComId, "");
                            }
                        }
                        else if (listOldDictItrCombine.Count < listNewDicItrCombine.Count)
                        {
                            List<EntityDicItrCombine> itrResult = listNewDicItrCombine.Where(p => !listOldDictItrCombine.Any(p2 => p2.ComId == p.ComId)).ToList();
                            if (itrResult.Count > 0)
                            {
                                foreach (EntityDicItrCombine detail in itrResult)
                                {
                                    detailComId += string.Format(",{0}", detail.ComId);
                                }
                                detailComId = detailComId.Remove(0, 1);
                                new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "仪器组合", "仪器组合明细", "新增", "已包含组合", "新增组合id" + detailComId, null);
                            }
                        }
                    }

                }

                #endregion

            }
        }

    }
}
