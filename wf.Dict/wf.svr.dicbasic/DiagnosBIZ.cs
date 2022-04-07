using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class DiagnosBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubIcd diagnos = request.GetRequestValue<EntityDicPubIcd>();
                diagnos.IcdDelFlag = "1";
                IDaoDic<EntityDicPubIcd> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcd>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(diagnos);
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
                EntityDicPubIcd diagnos = request.GetRequestValue<EntityDicPubIcd>();
                IDaoDic<EntityDicPubIcd> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcd>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(diagnos);
                    response.SetResult(diagnos);
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
            if (request != null)
            {
                EntityDicPubIcd diagnos = request.GetRequestValue<EntityDicPubIcd>();
                IDaoDic<EntityDicPubIcd> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcd>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(diagnos));
                }
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubIcd diagnos = request.GetRequestValue<EntityDicPubIcd>();
                IDaoDic<EntityDicPubIcd> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcd>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(diagnos);
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
