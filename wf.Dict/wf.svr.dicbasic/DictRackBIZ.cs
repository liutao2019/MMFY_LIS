using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class DictRackBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                List<EntityDicSampTubeRack> tubes = request.GetRequestValue() as List<EntityDicSampTubeRack>;
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    try
                    {
                        foreach(EntityDicSampTubeRack rack in tubes)
                        {
                            dao.Delete(rack);
                        }
                        response.Scusess = true;
                    }
                    catch(Exception ex)
                    {
                        response.Scusess = false;
                        response.ErroMsg = ex.ToString();
                    }
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
                EntityDicSampTubeRack tube = request.GetRequestValue<EntityDicSampTubeRack>();
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
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
            EntityResponse response = new EntityResponse();

            EntitySampStoreRack storeRack = request.GetRequestValue<EntitySampStoreRack>();
            IDaoDic<EntitySampStoreRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntitySampStoreRack>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Save(storeRack));
            }
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            //Dictionary<string, object> dict = request.GetRequestValue() as Dictionary<string, object>;

            Object tube = request.GetRequestValue();
            //if (dict["datetime"] != null)
            //{
            //    tube = dict["datetime"];
            //}
            //if (dict["rackCode"] != null)
            //{
            //    tube = dict["rackCode"];
            //}
            IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
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
                EntityDicSampTubeRack tube = request.GetRequestValue<EntityDicSampTubeRack>();
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
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
            EntityResponse response = new EntityResponse();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            IDaoDic<EntityDicSampStoreStatus> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampStoreStatus>>();
            IDaoDic<EntityDicTubeRack> dao2 = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTubeRack>>();
            if (dao == null || dao2==null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                dict.Add("status", dao.Search(null));
                dict.Add("tubeRacks", dao2.Search(null));
                response.SetResult(dict);
            }
            return response;
        }
    }
}