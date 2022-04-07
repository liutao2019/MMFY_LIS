using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.qc
{
    #region 新代码

    public class CriterionBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicQcRule rule = request.GetRequestValue<EntityDicQcRule>();
                IDaoDic<EntityDicQcRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(rule);
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

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicQcRule rule = request.GetRequestValue<EntityDicQcRule>();
                rule.DelFlag = "1";
                IDaoDic<EntityDicQcRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(rule);
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
                EntityDicQcRule rule = request.GetRequestValue<EntityDicQcRule>();
                IDaoDic<EntityDicQcRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRule>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(rule);
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

            EntityDicQcRule rule = request.GetRequestValue<EntityDicQcRule>();
            IDaoDic<EntityDicQcRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRule>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(rule));
            }
            return response;
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
    #endregion
}