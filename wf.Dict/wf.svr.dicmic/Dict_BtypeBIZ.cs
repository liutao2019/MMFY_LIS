using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicmic
{
    public class Dict_BtypeBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicBacttype bacttype = request.GetRequestValue<EntityDicMicBacttype>();
                bacttype.BtypeDelFlag = "1";
                IDaoDic<EntityDicMicBacttype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacttype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(bacttype);
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
                EntityDicMicBacttype bacttype = request.GetRequestValue<EntityDicMicBacttype>();
                IDaoDic<EntityDicMicBacttype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacttype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(bacttype);
                    response.SetResult(bacttype);
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

            EntityDicMicBacttype bacttype = request.GetRequestValue<EntityDicMicBacttype>();
            IDaoDic<EntityDicMicBacttype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacttype>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(bacttype));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicBacttype bacttype = request.GetRequestValue<EntityDicMicBacttype>();
                IDaoDic<EntityDicMicBacttype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacttype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(bacttype);
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

            EntityDicMicAntitype antitype = request.GetRequestValue<EntityDicMicAntitype>();
            IDaoDic<EntityDicMicAntitype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntitype>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(antitype));
            }
            return response;
        }
    }
}
