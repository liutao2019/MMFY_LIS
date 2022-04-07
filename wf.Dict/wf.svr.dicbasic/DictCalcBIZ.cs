using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class DictCalcBIZ : IDicCommon
    {

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaCalu type = request.GetRequestValue<EntityDicElisaCalu>();
                IDaoDic<EntityDicElisaCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCalu>>();
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
            dcl.svr.cache.DictClItemCache.Current.Refresh();
            return response;
        }

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaCalu type = request.GetRequestValue<EntityDicElisaCalu>();
                IDaoDic<EntityDicElisaCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCalu>>();
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
                EntityDicElisaCalu type = request.GetRequestValue<EntityDicElisaCalu>();
                IDaoDic<EntityDicElisaCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCalu>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(type);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictClItemCache.Current.Refresh();
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicElisaCalu type = request.GetRequestValue<EntityDicElisaCalu>();
                List<EntityDicElisaCalu> listCalu = new List<EntityDicElisaCalu>();
                IDaoDic<EntityDicElisaCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicElisaCalu>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    listCalu = dao.Search(type);
                }
                List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();

                listItem = new ItemBIZ().GetItemByItmId("");
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("Calu", listCalu);
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