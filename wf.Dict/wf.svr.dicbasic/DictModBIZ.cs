using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class DictModBIZ : IDicCommon
    {

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaMeaning type = request.GetRequestValue<EntityDicElisaMeaning>();
                IDaoDic<EntityDicElisaMeaning> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaMeaning>>();
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
                EntityDicElisaMeaning type = request.GetRequestValue<EntityDicElisaMeaning>();
                IDaoDic<EntityDicElisaMeaning> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaMeaning>>();
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
                List<EntityDicElisaMeaning> listMean = request.GetRequestValue<List<EntityDicElisaMeaning>>();
                IDaoDic<EntityDicElisaMeaning> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaMeaning>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0;
                    foreach (EntityDicElisaMeaning mean in listMean)
                    {
                        if (dao.Update(mean))
                        {
                            count++;
                        }
                    }
                    if (count == listMean.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(listMean);
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
            if (request != null)
            {
                EntityDicElisaMeaning type = request.GetRequestValue<EntityDicElisaMeaning>();
                IDaoDic<EntityDicElisaMeaning> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaMeaning>>();
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
    }
}