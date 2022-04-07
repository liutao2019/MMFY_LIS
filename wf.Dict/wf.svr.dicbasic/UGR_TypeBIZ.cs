using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class UGR_TypeBIZ : IDicCommon
    {

        #region ICommonBIZ 成员

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicroscope type = request.GetRequestValue<EntityDicMicroscope>();
                type.MicroDelFlag = "1";
                IDaoDic<EntityDicMicroscope> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicroscope>>();
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

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicroscope type = request.GetRequestValue<EntityDicMicroscope>();
                IDaoDic<EntityDicMicroscope> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicroscope>>();
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

            EntityDicMicroscope type = request.GetRequestValue<EntityDicMicroscope>();
            IDaoDic<EntityDicMicroscope> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicroscope>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type.MicroName));
            }
            return response;
        }
        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicroscope type = request.GetRequestValue<EntityDicMicroscope>();
                IDaoDic<EntityDicMicroscope> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicroscope>>();
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
            return new EntityResponse();
        }
        public EntityResponse View(EntityRequest request)
        {
            return new EntityResponse();
        }
        #endregion
    }
}
