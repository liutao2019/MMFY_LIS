using System;
using System.Collections.Generic;
using System.Data;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Configuration;
using dcl.servececontract;
using dcl.svr.cache;
using System.Linq;

namespace dcl.svr.dicbasic
{
    public class ItemCombineBIZ : IDicCommon
    {
        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            EntityDicCombine type = request.GetRequestValue<EntityDicCombine>();
            IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(hosID));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityDicCombine type = request.GetRequestValue<EntityDicCombine>();
            //IDaoDic<EntityDicCombineTimeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCombineTimeRule>>();
            List<EntityDicCombineTimeRule> listRule = new List<EntityDicCombineTimeRule>();
            Dictionary<string, object> d = new Dictionary<string, object>();
            //if (dao == null)
            //{
            //    response.Scusess = false;
            //    response.ErroMsg = "未找到数据访问";
            //}
            //else
            //{
            //    response.Scusess = true;
            //    listRule = dao.Search(type);
            //}
            listRule = new CombineTimeruleBIZ().Search(request).GetResult() as List<EntityDicCombineTimeRule>;
            IDaoDic<EntityDicCombineTimeruleRelated> daoRelated = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCombineTimeruleRelated>>();
            List<EntityDicCombineTimeruleRelated> listRelated = new List<EntityDicCombineTimeruleRelated>();
            if (daoRelated == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                listRelated = daoRelated.Search(type.ComId);
            }
            d.Add("Rule", listRule);
            d.Add("Related", listRelated);
            response.SetResult(d);
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicCombine type = request.GetRequestValue<EntityDicCombine>();
                IDaoDicCombineDetail dao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                List<EntityDicCombineDetail> listDetail = new List<EntityDicCombineDetail>();
                Dictionary<string, object> d = new Dictionary<string, object>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    listDetail = dao.Search(type.ComId);
                }
                IDaoDic<EntityDicItmCombine> daoItmCombine = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCombine>>();
                List<EntityDicItmCombine> lisItmCombine = new List<EntityDicItmCombine>();
                if (daoItmCombine == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    lisItmCombine = daoItmCombine.Search(type);
                }
                d.Add("Detail", listDetail);
                d.Add("ItmCombine", lisItmCombine);
                //List<EntityDicItmCombine> listItemIn = lisItmCombine.Where(p => listDetail.Where(g => g.ComItmId == p.ItmId).Any()).ToList();
                //d.Add("ItmIn", lisItmCombine);
                response.SetResult(d);
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {

                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicCombine combine = new EntityDicCombine();
                string oldComId = "";
                object objCombine = dict["Combine"];
                if (objCombine != null)
                {
                    combine = (EntityDicCombine)objCombine;
                    oldComId = combine.ComId;
                }
                IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(combine);
                    response.SetResult(combine);
                }
                LogDbLOG(dict);
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
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicCombine combine = new EntityDicCombine();
                EntityDicCombineDetail detail = new EntityDicCombineDetail();
                if (dict.ContainsKey("Combine"))
                {
                    object com = dict["Combine"];
                    if (com != null)
                    {
                        combine = (EntityDicCombine)com;
                    }
                    combine.ComDelFlag = "1";
                    IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
                    if (dao == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        response.Scusess = dao.Delete(combine);
                    }
                }
                if (dict.ContainsKey("ItmDetail"))
                {
                    object dt = dict["ItmDetail"];
                    if (detail != null)
                    {
                        detail = (EntityDicCombineDetail)dt;
                    }

                    IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                    if (daoDetail == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        response.Scusess = daoDetail.Delete(detail);
                    }
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
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicCombine combine = new EntityDicCombine();
                string oldComId = "";
                List<EntityDicCombineTimeruleRelated> listRelatd = new List<EntityDicCombineTimeruleRelated>();
                List<EntityDicCombineDetail> listItmDetail = new List<EntityDicCombineDetail>();
                object objCombine = dict["Combine"];
                if (objCombine != null)
                {
                    combine = (EntityDicCombine)objCombine;
                    oldComId = combine.ComId;
                }
                if (!dict.ContainsKey("Related"))
                {
                    LogDbLOG(dict);
                }
                if (dict.ContainsKey("Related"))
                {
                    object objcomRelatd = dict["Related"];
                    if (objcomRelatd != null)
                    {
                        listRelatd = objcomRelatd as List<EntityDicCombineTimeruleRelated>;
                    }
                    IDaoDic<EntityDicCombineTimeruleRelated> daoRelated = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCombineTimeruleRelated>>();
                    if (daoRelated == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        EntityDicCombineTimeruleRelated entity = new EntityDicCombineTimeruleRelated();
                        entity.ComId = oldComId;
                        bool isDelete = daoRelated.Delete(entity); //daoRelated.Delete(listRelatd[0]);

                        int count = 0;
                        foreach (EntityDicCombineTimeruleRelated comRelatd in listRelatd)
                        {
                            if (daoRelated.Save(comRelatd))
                            {
                                count++;
                            }
                        }
                        if (isDelete && count == listRelatd.Count)
                        {
                            response.Scusess = true;
                        }
                        response.SetResult(listRelatd);
                    }
                }
                if (dict.ContainsKey("ComDetail"))
                {
                    object objItmDetail = dict["ComDetail"];
                    if (objItmDetail != null)
                    {
                        listItmDetail = objItmDetail as List<EntityDicCombineDetail>;
                    }
                    IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                    if (daoDetail == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        int count = 0;
                        bool isDelete = false;
                        if (listItmDetail.Count > 0)
                        {
                            isDelete = daoDetail.Delete(listItmDetail[0]);
                        }
                        foreach (EntityDicCombineDetail detail in listItmDetail)
                        {
                            if (daoDetail.Save(detail))
                            {
                                count++;
                            }
                        }
                        if (isDelete && count == listItmDetail.Count)
                        {
                            response.Scusess = true;
                        }
                        response.SetResult(listItmDetail);
                    }
                }
                IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(combine);
                    response.SetResult(combine);

                }

            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictCombineCache.Current.Refresh();
            return response;
        }
        /// <summary>
        /// 根据 仪器ID和组合ID 获取具有缺省值的项目
        /// </summary>
        /// <param name="itrId">仪器ID</param>
        /// <param name="comId">组合ID</param>
        /// <returns></returns>
        public List<EntityDicCombineDetail> GetCombineDefData(string itrId, string comId)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (daoDetail != null)
            {
                list = daoDetail.GetCombineDefData(itrId, comId);
            }
            return list;
        }
        /// <summary>
        /// 根据 组合ID和标本 仪器ID获取具有缺省值的项目
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="patSamId"></param>
        /// <param name="itrId"></param>
        /// <returns></returns>
        public List<EntityDicCombineDetail> GetCombineMiWdthDefault(string comId, string patSamId, string itrId)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (daoDetail != null)
            {
                list = daoDetail.GetCombineMiWdthDefault(comId, patSamId, itrId);
            }
            return list;
        }

        /// <summary>
        /// 根据报告ID获取项目名称
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="patSamId"></param>
        /// <param name="itrId"></param>
        /// <returns></returns>
        public List<EntityDicCombineDetail> GetItmNameByRepId(string RepId, bool addSql)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (daoDetail != null)
            {
                list = daoDetail.GetItmNameByRepId(RepId, addSql);
            }
            return list;
        }

        private void LogDbLOG(Dictionary<string, object> dict)
        {
            EntitySysOperationLog dtOperation = (EntitySysOperationLog)dict["Operation"];
            //DataTable _LOGINFO = dsDataPack.Tables["_LOGINFO"];

            EntityDicCombine NewDicCombine = (EntityDicCombine)dict["Combine"];
            List<EntityDicCombineDetail> listNewComDetail = new List<EntityDicCombineDetail>();
            if (dict.Keys.Contains("ComDetail"))
            {
               listNewComDetail = dict["ComDetail"] as List<EntityDicCombineDetail>;
            }

            //List<EntityDicItmRefdetail> dtNewItemMi = dict["Detail"] as List<EntityDicItmRefdetail>;
            EntityResponse response = new EntityResponse();
            string old_com_id = NewDicCombine.ComId;
            string new_com_name = NewDicCombine.ComName;
            string new_com_ecd = NewDicCombine.ComCode;

            string operationName = dtOperation.OperatAction;
            string loginID = dtOperation.OperatUserId;
            string userName = dtOperation.OperatUserName;
            string ipAdress = IPUtility.GetIP();


            if (operationName == "删除"
                || operationName == "新增"
                )
            {
                dtOperation.OperatServername = ipAdress;
                dtOperation.OperatKey = old_com_id;
                dtOperation.OperatDate = cache.ServerDateTime.GetDatabaseServerDateTime();
                dtOperation.OperatModule = "组合字典";
                dtOperation.OperatGroup = "组合基本信息";
                dtOperation.OperatObject = string.Format("{0}({1})", new_com_name, new_com_ecd);
                dtOperation.OperatContent = new_com_name;
                IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.SaveSysOperationLog(dtOperation);
                    response.SetResult(dtOperation);
                }
            }
            else if (operationName == "修改")
            {
                DateTime dtNow = ServerDateTime.GetDatabaseServerDateTime();
                #region 比较组合信息
                //比较组合信息
                List<EntityDicCombine> listOldDictCombine = new List<EntityDicCombine>();
                EntityRequest request = new EntityRequest();
                EntityResponse responseCom = new EntityResponse();
                responseCom=Search(request);
                listOldDictCombine = (responseCom.GetResult() as List<EntityDicCombine>);
                if (listOldDictCombine.Count > 0
                    && NewDicCombine != null)
                {
                    EntityDicCombine rowComOld = listOldDictCombine.Find(w=>w.ComId==old_com_id);
                    EntityDicCombine rowComNew = NewDicCombine;

                    string msgItem = string.Empty;
                    if (rowComOld.ComCode != rowComNew.ComCode)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "组合代码", rowComOld.ComCode , rowComNew.ComCode);
                    }

                    if (rowComOld.ComName != rowComNew.ComName)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "组合名称", rowComOld.ComName, rowComNew.ComName);
                    }

                    if (rowComOld.ComItrId != rowComNew.ComItrId)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "默认仪器", rowComOld.ComItrId, rowComNew.ComItrId);
                    }
                    if (rowComOld.ComSamId != rowComNew.ComSamId)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "默认标本", rowComOld.ComSamId, rowComNew.ComSamId);
                    }
                    if (rowComOld.ComPriId != rowComNew.ComPriId)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "专业组", rowComOld.ComPriId, rowComNew.ComPriId);
                    }
                    if (rowComOld.ComLabId != rowComNew.ComLabId)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "物理组", rowComOld.ComLabId, rowComNew.ComLabId);
                    }
                    if (rowComOld.ComPrice != rowComNew.ComPrice)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "价格", rowComOld.ComPrice, rowComNew.ComPrice);
                    }
                    if (rowComOld.ComQcFlag != rowComNew.ComQcFlag)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "质控标志", rowComOld.ComQcFlag, rowComNew.ComQcFlag);
                    }
                    if (rowComOld.ComUrgentFlag != rowComNew.ComUrgentFlag)
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合基本信息", "修改", "加急标志", rowComOld.ComUrgentFlag, rowComNew.ComUrgentFlag);
                    }
                }
                #endregion

                #region 比较组合明细信息
                //比较组合明细信息 
                if (listNewComDetail.Count > 0)
                {
                    List<EntityDicCombineDetail> listOldDictComDetail = new List<EntityDicCombineDetail>();
                    List<EntityDicItmCombine> listCom = new List<EntityDicItmCombine>();
                    List<EntityDicCombineDetail> listComDetail = new List<EntityDicCombineDetail>();
                    List<EntityDicItmCombine> listItmCombine = new List<EntityDicItmCombine>();
                    request.SetRequestValue(NewDicCombine);
                    EntityResponse ds = View(request);
                    Dictionary<string, object> dictDetail = ds.GetResult() as Dictionary<string, object>;
                    object objDetail = dictDetail["Detail"];
                    object objItm = dictDetail["ItmCombine"];
                    //  object objItemIn = dict["ItmIn"];
                    if (objDetail != null)
                    {
                        listComDetail = objDetail as List<EntityDicCombineDetail>;
                    }
                    if (objItm != null)
                    {
                        listItmCombine = objItm as List<EntityDicItmCombine>;
                    }

                    listCom = listItmCombine.Where(p => listComDetail.Where(g => g.ComItmId == p.ItmId).Any()).ToList();
                    foreach (var listItmCom in listCom)
                    {
                        EntityDicCombineDetail entityDetail = new EntityDicCombineDetail();
                        entityDetail.ComId = listItmCom.ItmComId;
                        entityDetail.ComItmId = listItmCom.ItmId;
                        entityDetail.ComItmEname = listItmCom.ItmEname;
                        entityDetail.ComMustItem = listItmCom.ItmMustItem.ToString();
                        entityDetail.ComFlag = listItmCom.ItmComFlag;
                        entityDetail.ComPrintFlag = listItmCom.ItmPrintFlag;
                        entityDetail.ComSortNo = listItmCom.ItmSort;
                        listOldDictComDetail.Add(entityDetail);
                    }
                    string detailItmId = string.Empty;
                    if (listOldDictComDetail.Count > listNewComDetail.Count)
                    {
                        List<EntityDicCombineDetail> result = listOldDictComDetail.Where(p => !listNewComDetail.Any(p2 => p2.ComItmId == p.ComItmId)).ToList();
                        if (result.Count > 0)
                        {
                            foreach (EntityDicCombineDetail detail in result)
                            {
                                detailItmId += string.Format(",{0}", detail.ComItmId);
                            }
                            detailItmId.Remove(0, 1);
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合明细", "删除", "已包含项目", "移除项目id" + detailItmId, "");
                        }
                    }
                    else if (listOldDictComDetail.Count < listNewComDetail.Count)
                    {
                        List<EntityDicCombineDetail> result = listNewComDetail.Where(p => !listOldDictComDetail.Any(p2 => p2.ComItmId == p.ComItmId)).ToList();
                        if (result.Count > 0)
                        {
                            foreach (EntityDicCombineDetail detail in result)
                            {
                                detailItmId += string.Format(",{0}", detail.ComItmId);
                            }
                            detailItmId=detailItmId.Remove(0, 1);
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_com_id, "组合字典", "组合明细", "新增", "已包含项目", "新增项目id" + detailItmId,null);
                        }
                    }
                }

                #endregion

            }
        }
     
    }
}
