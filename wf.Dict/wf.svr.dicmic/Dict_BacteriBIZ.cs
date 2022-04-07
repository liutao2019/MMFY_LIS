using System;
using dcl.entity;
using dcl.servececontract;

using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicmic
{
    public class Dict_BacteriBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicBacteria bacteria = request.GetRequestValue<EntityDicMicBacteria>();
                bacteria.BacDelFlag = "1";
                IDaoDic<EntityDicMicBacteria> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacteria>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(bacteria);
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
                EntityDicMicBacteria bacteria = request.GetRequestValue<EntityDicMicBacteria>();
                IDaoDic<EntityDicMicBacteria> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacteria>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(bacteria);
                    response.SetResult(bacteria);
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

            EntityDicMicBacteria bacteria = request.GetRequestValue<EntityDicMicBacteria>();
            IDaoDic<EntityDicMicBacteria> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacteria>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(bacteria));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicBacteria bacteria = request.GetRequestValue<EntityDicMicBacteria>();
                IDaoDic<EntityDicMicBacteria> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicBacteria>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(bacteria);
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
