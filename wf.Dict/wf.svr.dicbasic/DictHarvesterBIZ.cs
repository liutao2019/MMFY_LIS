using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class DictHarvesterBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDictHarvester harvester = request.GetRequestValue<EntityDictHarvester>();
                harvester.DelFlag = 1;
                IDaoDic<EntityDictHarvester> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDictHarvester>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(harvester);
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
                EntityDictHarvester harvester = request.GetRequestValue<EntityDictHarvester>();
                IDaoDic<EntityDictHarvester> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDictHarvester>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(harvester);
                    response.SetResult(harvester);
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
                EntityDictHarvester harvester = request.GetRequestValue<EntityDictHarvester>();
                IDaoDic<EntityDictHarvester> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDictHarvester>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(harvester));
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
                EntityDictHarvester harvester = request.GetRequestValue<EntityDictHarvester>();
                IDaoDic<EntityDictHarvester> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDictHarvester>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(harvester);
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
