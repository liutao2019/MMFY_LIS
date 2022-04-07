using dcl.entity;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class S_StateBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSState state = request.GetRequestValue<EntityDicSState>();
                IDaoDic<EntityDicSState> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSState>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(state);
                    response.SetResult(state);
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
                EntityDicSState state = request.GetRequestValue<EntityDicSState>();
                state.DelFlag = "1";
                IDaoDic<EntityDicSState> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSState>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(state);
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
                EntityDicSState state = request.GetRequestValue<EntityDicSState>();
                IDaoDic<EntityDicSState> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSState>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(state);
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

            EntityDicSState state = request.GetRequestValue<EntityDicSState>();
            IDaoDic<EntityDicSState> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSState>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(state));
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
