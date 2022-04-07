using System.Collections.Generic;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.servececontract;

namespace dcl.svr.dicbasic
{
    public class BscripeBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubEvaluate type = request.GetRequestValue<EntityDicPubEvaluate>();
                IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
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

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubEvaluate type = request.GetRequestValue<EntityDicPubEvaluate>();
                IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
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

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubEvaluate type = request.GetRequestValue<EntityDicPubEvaluate>();
                IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicPubEvaluate type = request.GetRequestValue<EntityDicPubEvaluate>();
            IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type));
            }
            return response;
        }
        /// <summary>
        /// 查询描述评价  插入缺省值使用
        /// </summary>
        /// <returns></returns>
        public List<EntityDicPubEvaluate> GetContent()
        {
            List<EntityDicPubEvaluate> list = new List<EntityDicPubEvaluate>();
            IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
            if (dao != null)
            {
                list= dao.SearchContent();
            }
            return list;
        }
        public EntityResponse Other(EntityRequest request)
        {
            return new EntityResponse();
        }

        public EntityResponse View(EntityRequest request)
        {
            return new EntityResponse();
        }
    }
}
