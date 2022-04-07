using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class TemperatureBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTemperature temp = request.GetRequestValue<EntityDicTemperature>();
                temp.DelFlag = "1";
                IDaoDic<EntityDicTemperature> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTemperature>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(temp);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入数据为null";
            }
            return response;
        }

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTemperature temp = request.GetRequestValue<EntityDicTemperature>();
                IDaoDic<EntityDicTemperature> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTemperature>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(temp);
                    response.SetResult(temp);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入数据为null";
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTemperature temp = request.GetRequestValue<EntityDicTemperature>();
                IDaoDic<EntityDicTemperature> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTemperature>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(temp));
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入数据为null";
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicTemperature temp = request.GetRequestValue<EntityDicTemperature>();
                IDaoDic<EntityDicTemperature> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTemperature>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(temp);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入数据为null";
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
