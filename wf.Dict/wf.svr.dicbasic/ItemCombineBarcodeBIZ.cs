using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.servececontract;

namespace dcl.svr.dicbasic
{
    public class ItemCombineBarcodeBIZ : IDicCommon
    {
        public EntityResponse Search(EntityRequest request)
        {
            //返回条码明细
            EntityResponse response = new EntityResponse();
            string conId = request.GetRequestValue<String>();
            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            //List<EntityDicCombineTimeRule> listRule = new List<EntityDicCombineTimeRule>();
            Dictionary<string, object> d = new Dictionary<string, object>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(conId));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = request.GetRequestValue<Dictionary<string, object>>();
                var listNew = dict["new"] as List<EntitySampMergeRule>;
                var listUpdate = dict["update"] as List<EntitySampMergeRule>;
                IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    try
                    {
                        foreach (var item in listNew)
                            dao.Save(item);

                        foreach (var item in listUpdate)
                            dao.Update(item);
                        response.Scusess = true;
                    }
                    catch (Exception ex)
                    {
                        response.Scusess = false;
                        response.ErroMsg = ex.Message;
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

        public EntityResponse View(EntityRequest request)
        {
            #region 返回组合信息
            EntityResponse response = new EntityResponse();
            //string hosID = ConfigurationManager.AppSettings["HospitalID"];
            EntityDicCombine type = request.GetRequestValue<EntityDicCombine>();
            IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>(); ;
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(""));
            }
            return response;
            #endregion
        }

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntitySampMergeRule mergeRule = request.GetRequestValue<EntitySampMergeRule>();
                IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(mergeRule);
                    response.SetResult(mergeRule);
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
                EntitySampMergeRule mergeRule = request.GetRequestValue<EntitySampMergeRule>();
                mergeRule.ComDelFlag = "1";
                IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(mergeRule);
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
                EntitySampMergeRule rule = request.GetRequestValue<EntitySampMergeRule>();
                IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(rule);
                    response.SetResult(rule);
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
        /// 根据hiscode查询出合并信息
        /// </summary>
        /// <param name="listHisCode"></param>
        /// <returns></returns>
        public List<EntitySampMergeRule> GetSampMergeRuleByHisCode(List<String> listHisCode, String strOriId)
        {
            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();

            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.SearchRuleByHisCode(listHisCode, strOriId);
            }

            return listResult;
        }

        /// <summary>
        /// 根据liscode查询出合并信息
        /// </summary>
        /// <param name="listHisCode"></param>
        /// <returns></returns>
        public List<EntitySampMergeRule> GetSampMergeRuleByLisCode(List<String> listLisCode, String strOriId)
        {
            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();

            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.SearchRuleByLisCode(listLisCode, strOriId);
            }

            return listResult;
        }


        /// <summary>
        /// 根据组合才分信息获取合并信息
        /// </summary>
        /// <param name="strComId"></param>
        /// <param name="strOriId"></param>
        /// <returns></returns>
        public List<EntitySampMergeRule> GetSampMergeRuleRuleBySplitComId(string strComId, String strOriId)
        {
            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();

            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.SearchRuleBySplitComId(strComId, strOriId);
            }

            return listResult;
        }

        /// <summary>
        /// 获取所有组合的拆分条码信息
        /// </summary>
        /// <returns></returns>
        public List<EntitySampMergeRule> GetAllCombineSplitBarCode()
        {
            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();

            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.GetAllCombineSplitBarCode();
            }

            return listResult;
        }
    }
}
