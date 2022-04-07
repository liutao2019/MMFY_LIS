using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class IceBoxBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampStore store = request.GetRequestValue<EntityDicSampStore>();
                IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(store);
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
                EntityDicSampStore store = request.GetRequestValue<EntityDicSampStore>();
                IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(store);
                    response.SetResult(store);
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

            IDaoDic<EntityDicSampStoreStatus> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreStatus>>();
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicSampStore store = request.GetRequestValue<EntityDicSampStore>();
            IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(store));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampStore store = request.GetRequestValue<EntityDicSampStore>();
                IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(store);
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
