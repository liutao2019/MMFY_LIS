﻿using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class BCCuvetteBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTestTube tube = request.GetRequestValue<EntityDicTestTube>();
                tube.TubDelFlag = "1";
                IDaoDic<EntityDicTestTube> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTestTube>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(tube);
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
                EntityDicTestTube tube = request.GetRequestValue<EntityDicTestTube>();
                IDaoDic<EntityDicTestTube> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTestTube>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(tube);
                    response.SetResult(tube);
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

            EntityDicTestTube tube = request.GetRequestValue<EntityDicTestTube>();
            IDaoDic<EntityDicTestTube> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTestTube>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(tube));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTestTube tube = request.GetRequestValue<EntityDicTestTube>();
                IDaoDic<EntityDicTestTube> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTestTube>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(tube);
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