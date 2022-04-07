using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class CombineSplitBIZ : IDicCommon
    {
        #region ICommonBIZ 成员

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampDivergeRule type = request.GetRequestValue<EntityDicSampDivergeRule>();
                IDaoDic<EntityDicSampDivergeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(type);
                }
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
                List<EntityDicSampDivergeRule> typeList = request.GetRequestValue<List<EntityDicSampDivergeRule>>();
                IDaoDic<EntityDicSampDivergeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int i = 0;
                    foreach (var type in typeList)
                    {
                        if (i == 0)
                        {
                            response.Scusess = dao.Delete(type);
                        }
                        response.Scusess = dao.Save(type);
                        response.SetResult(type);
                        i++;
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicSampDivergeRule type = request.GetRequestValue<EntityDicSampDivergeRule>();

            List<string> List = new List<string>();
            List.Add(type.ComId);
            List.Add("Search");
            IDaoDic<EntityDicSampDivergeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(List));
            }
            return response;
        }
        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampDivergeRule type = request.GetRequestValue<EntityDicSampDivergeRule>();
                IDaoDic<EntityDicSampDivergeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(type);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }
        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicSampDivergeRule type = request.GetRequestValue<EntityDicSampDivergeRule>();

            List<string> List = new List<string>();
            List.Add(type.ComId);
            List.Add("Other");
            IDaoDic<EntityDicSampDivergeRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(List));
            }
            return response;
        }
        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

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
                response.SetResult(dao.GetCombineSplit());
            }
            return response;
        }
        #endregion
    }
}
