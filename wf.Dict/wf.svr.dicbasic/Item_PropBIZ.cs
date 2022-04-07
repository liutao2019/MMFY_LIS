using System;
using System.Collections.Generic;
using System.Linq;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class Item_PropBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDefItmProperty depart = request.GetRequestValue<EntityDefItmProperty>();
                IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(depart);
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
                EntityDefItmProperty depart = request.GetRequestValue<EntityDefItmProperty>();
                IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(depart);
                    response.SetResult(depart);
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
            if (request != null)
            {
                Dictionary<string, object> dict = request.GetRequestValue<Dictionary<string, object>>();
                var listNew = dict["new"] as List<EntityDefItmProperty>;
                var listUpdate = dict["update"] as List<EntityDefItmProperty>;
                IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    try
                    {
                        foreach (var item in listNew)
                            dao.Save(item);

                        foreach (var item in listUpdate)
                            dao.Update(item);
                        response.Scusess = true;
                    }
                    catch (Exception ex)
                    {
                        response.Scusess = false;
                        response.ErroMsg = ex.Message;
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            string itmId = request.GetRequestValue<string>();
            IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(itmId));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDefItmProperty depart = request.GetRequestValue<EntityDefItmProperty>();
                IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(depart);
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

            try
            {
                response.Scusess = true;
                response.SetResult(new ItemBIZ().GetItemByItmId(""));
            }
            catch
            {
                response.Scusess = false;
            }
            return response;
        }
    }
}
