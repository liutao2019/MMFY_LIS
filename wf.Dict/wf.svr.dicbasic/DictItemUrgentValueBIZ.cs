using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    /// <summary>
    /// 项目危急值
    /// </summary>
    public class DictItemUrgentValueBIZ: IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicUtgentValue type = request.GetRequestValue<EntityDicUtgentValue>();
                IDaoDic<EntityDicUtgentValue> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicUtgentValue>>();
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
                EntityDicUtgentValue type = request.GetRequestValue<EntityDicUtgentValue>();
                IDaoDic<EntityDicUtgentValue> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicUtgentValue>>();
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
                EntityDicUtgentValue type = request.GetRequestValue<EntityDicUtgentValue>();
                IDaoDic<EntityDicUtgentValue> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicUtgentValue>>();
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

            EntityDicUtgentValue type = request.GetRequestValue<EntityDicUtgentValue>();
            type.UgtItmId = "0";
            IDaoDic<EntityDicUtgentValue> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicUtgentValue>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type.UgtItmId));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicUtgentValue type = request.GetRequestValue<EntityDicUtgentValue>();
            IDaoDic<EntityDicUtgentValue> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicUtgentValue>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type.UgtKey));
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
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
    }
}
