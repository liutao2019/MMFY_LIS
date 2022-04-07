using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class CombineTimeruleBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicCombineTimeRule type = request.GetRequestValue<EntityDicCombineTimeRule>();
                type.DelFlag = "1";
                IDaoDicCombineTimeRule dao = DclDaoFactory.DaoHandler<IDaoDicCombineTimeRule>();
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
        #region ICommonBIZ 成员

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicCombineTimeRule type = request.GetRequestValue<EntityDicCombineTimeRule>();
                type.DelFlag = "0";
                IDaoDicCombineTimeRule dao = DclDaoFactory.DaoHandler<IDaoDicCombineTimeRule>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(type);
                    response.SetResult(type);
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
            throw new NotImplementedException();
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            IDaoDicCombineTimeRule dao = DclDaoFactory.DaoHandler<IDaoDicCombineTimeRule>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(request));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicCombineTimeRule type = request.GetRequestValue<EntityDicCombineTimeRule>();
                IDaoDicCombineTimeRule dao = DclDaoFactory.DaoHandler<IDaoDicCombineTimeRule>();
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

        public EntityResponse View(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicCombineTimeRule> GetTATComTimeByRepId(List<string> listRepId)
        {
            List<EntityDicCombineTimeRule> list = new List<EntityDicCombineTimeRule>();
            IDaoDicCombineTimeRule dao = DclDaoFactory.DaoHandler<IDaoDicCombineTimeRule>();
            if (dao != null)
            {
                list = dao.GetTATComTimeByRepId(listRepId);
            }
            return list;
        }
        #endregion
    }
}
