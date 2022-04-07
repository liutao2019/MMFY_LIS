using System;
using dcl.entity;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicmic
{
    public class Dict_AntibioBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntibio antibio = request.GetRequestValue<EntityDicMicAntibio>();
                antibio.AntDelFlag = "1";
                IDaoDic<EntityDicMicAntibio> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntibio>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(antibio);
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
                EntityDicMicAntibio antibio = request.GetRequestValue<EntityDicMicAntibio>();
                IDaoDic<EntityDicMicAntibio> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntibio>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(antibio);
                    response.SetResult(antibio);
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

            EntityDicMicAntibio antibio = request.GetRequestValue<EntityDicMicAntibio>();
            IDaoDic<EntityDicMicAntibio> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntibio>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(antibio));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntibio antibio = request.GetRequestValue<EntityDicMicAntibio>();
                IDaoDic<EntityDicMicAntibio> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntibio>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(antibio);
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
