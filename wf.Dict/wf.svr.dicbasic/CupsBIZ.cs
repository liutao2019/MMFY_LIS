using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class CupsBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampStoreArea area = request.GetRequestValue<EntityDicSampStoreArea>();
                IDaoDic<EntityDicSampStoreArea> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreArea>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(area);
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
                EntityDicSampStoreArea area = request.GetRequestValue<EntityDicSampStoreArea>();
                IDaoDic<EntityDicSampStoreArea> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreArea>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(area);
                    response.SetResult(area);
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

            EntityDicSampStoreArea area = request.GetRequestValue<EntityDicSampStoreArea>();
            IDaoDic<EntityDicSampStoreArea> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreArea>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(area));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSampStoreArea area = request.GetRequestValue<EntityDicSampStoreArea>();
                IDaoDic<EntityDicSampStoreArea> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreArea>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(area);
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
            EntityResponse response = new EntityResponse();

            IDaoDic<EntityDicSampStore> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStore>>();
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
    }
}
