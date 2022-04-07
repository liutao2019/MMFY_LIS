using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class No_TypeBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubIdent noType = request.GetRequestValue<EntityDicPubIdent>();
                noType.IdtDelFlag = "1";
                IDaoDic<EntityDicPubIdent> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIdent>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(noType);
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
                EntityDicPubIdent noType = request.GetRequestValue<EntityDicPubIdent>();
                IDaoDic<EntityDicPubIdent> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIdent>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(noType);
                    response.SetResult(noType);
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

            EntityDicPubIdent noType = request.GetRequestValue<EntityDicPubIdent>();
            IDaoDic<EntityDicPubIdent> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIdent>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(noType));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubIdent noType = request.GetRequestValue<EntityDicPubIdent>();
                IDaoDic<EntityDicPubIdent> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIdent>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(noType);
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
