using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class FeesTypeBIZ : IDicCommon
  {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubInsurance feesType = request.GetRequestValue<EntityDicPubInsurance>();
                feesType.FeesTypeDelFlag = "1";
                IDaoDic<EntityDicPubInsurance> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubInsurance>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(feesType);
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
                EntityDicPubInsurance feesType = request.GetRequestValue<EntityDicPubInsurance>();
                IDaoDic<EntityDicPubInsurance> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubInsurance>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(feesType);
                    response.SetResult(feesType);
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

            EntityDicPubInsurance feesType = request.GetRequestValue<EntityDicPubInsurance>();
            IDaoDic<EntityDicPubInsurance> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubInsurance>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(feesType));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubInsurance feesType = request.GetRequestValue<EntityDicPubInsurance>();
                IDaoDic<EntityDicPubInsurance> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubInsurance>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(feesType);
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
