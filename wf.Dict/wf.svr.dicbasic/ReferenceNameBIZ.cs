using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.servececontract;

namespace dcl.svr.dicbasic
{
    public class ReferenceNameBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItmReftype type = request.GetRequestValue<EntityDicItmReftype>();
                IDaoDic<EntityDicItmReftype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmReftype>>();
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
                EntityDicItmReftype type = request.GetRequestValue<EntityDicItmReftype>();
                type.RefDelFlag = "1";
                IDaoDic<EntityDicItmReftype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmReftype>>();
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
                EntityDicItmReftype type = request.GetRequestValue<EntityDicItmReftype>();
                IDaoDic<EntityDicItmReftype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmReftype>>();
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

            EntityDicItmReftype type = request.GetRequestValue<EntityDicItmReftype>();
            IDaoDic<EntityDicItmReftype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmReftype>>();
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
