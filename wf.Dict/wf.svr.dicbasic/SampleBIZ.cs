using dcl.servececontract;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class SampleBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicSample sample = request.GetRequestValue<EntityDicSample>();
                IDaoDic<EntityDicSample> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSample>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(sample);
                    response.SetResult(sample);
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
                EntityDicSample sample = request.GetRequestValue<EntityDicSample>();
                sample.SamDelFlag = "1";
                IDaoDic<EntityDicSample> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSample>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(sample);
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
                EntityDicSample sample = request.GetRequestValue<EntityDicSample>();
                IDaoDic<EntityDicSample> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSample>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(sample);
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

            EntityDicSample sample = request.GetRequestValue<EntityDicSample>();
            IDaoDic<EntityDicSample> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSample>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(sample));
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
