using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class SamRemarksBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampRemark samRemark = request.GetRequestValue<EntityDicSampRemark>();
                IDaoDic<EntityDicSampRemark> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampRemark>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(samRemark);
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
                EntityDicSampRemark samRemark = request.GetRequestValue<EntityDicSampRemark>();
                IDaoDic<EntityDicSampRemark> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampRemark>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(samRemark);
                    response.SetResult(samRemark);
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
            return new EntityResponse();
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicSampRemark samRemark = request.GetRequestValue<EntityDicSampRemark>();
            IDaoDic<EntityDicSampRemark> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampRemark>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(samRemark));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampRemark samRemark = request.GetRequestValue<EntityDicSampRemark>();
                IDaoDic<EntityDicSampRemark> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampRemark>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(samRemark);
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
            return new EntityResponse();
        }
    }
}
