using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class DicItemHoleBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaItem type = request.GetRequestValue<EntityDicElisaItem>();
                IDaoDic<EntityDicElisaItem> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaItem>>();
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
                EntityDicElisaItem type = request.GetRequestValue<EntityDicElisaItem>();
                type.PlateId = "1";
                IDaoDic<EntityDicElisaItem> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaItem>>();
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicElisaItem type = request.GetRequestValue<EntityDicElisaItem>();
            IDaoDic<EntityDicElisaItem> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaItem>>();
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
        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaItem type = request.GetRequestValue<EntityDicElisaItem>();
                IDaoDic<EntityDicElisaItem> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaItem>>();
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

            EntityDicElisaSort type = request.GetRequestValue<EntityDicElisaSort>();
            IDaoDic<EntityDicElisaSort> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaSort>>();
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
        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicElisaStatus type = request.GetRequestValue<EntityDicElisaStatus>();
            IDaoDic<EntityDicElisaStatus> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaStatus>>();
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

    }
}
