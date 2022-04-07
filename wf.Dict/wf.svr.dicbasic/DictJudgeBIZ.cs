using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class DictJudgeBIZ : IDicCommon
    {

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaCriter type = request.GetRequestValue<EntityDicElisaCriter>();
                IDaoDic<EntityDicElisaCriter> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCriter>>();
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
                EntityDicElisaCriter type = request.GetRequestValue<EntityDicElisaCriter>();
                IDaoDic<EntityDicElisaCriter> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCriter>>();
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
                List<EntityDicElisaCriter> listCriter = request.GetRequestValue<List<EntityDicElisaCriter>>();
                IDaoDic<EntityDicElisaCriter> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCriter>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0;
                    foreach (EntityDicElisaCriter criter in listCriter)
                    {
                        if (dao.Update(criter))
                        {
                            count++;
                        }
                    }
                    if (count == listCriter.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(listCriter);
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
                EntityDicElisaCriter type = request.GetRequestValue<EntityDicElisaCriter>();
                List<EntityDicElisaCriter> listCriter = new List<EntityDicElisaCriter>();
                IDaoDic<EntityDicElisaCriter> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCriter>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    listCriter = dao.Search(type);
                }

                List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
                listItem = new ItemBIZ().GetItemByItmId("");

                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("Criter", listCriter);
                d.Add("Item", listItem);
                response.SetResult(d);
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