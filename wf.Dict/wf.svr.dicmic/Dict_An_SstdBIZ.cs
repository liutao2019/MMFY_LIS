using System;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Collections.Generic;

namespace dcl.svr.dicmic
{
    public class Dict_An_SstdBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntidetail anSstd = request.GetRequestValue<EntityDicMicAntidetail>();
                anSstd.AnsDelFlag = "1";
                IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(anSstd);
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
                EntityDicMicAntidetail anSstd = request.GetRequestValue<EntityDicMicAntidetail>();
                IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(anSstd);
                    response.SetResult(anSstd);
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
                var listNew = dict["new"] as List<EntityDicMicAntidetail>;
                var listUpdate = dict["update"] as List<EntityDicMicAntidetail>;
                IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
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

            EntityDicMicAntidetail anSstd = request.GetRequestValue<EntityDicMicAntidetail>();
            IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(anSstd));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntidetail anSstd = request.GetRequestValue<EntityDicMicAntidetail>();
                IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(anSstd);
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
