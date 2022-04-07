using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class ClItemBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItmCalu type = request.GetRequestValue<EntityDicItmCalu>();
                IDaoDic<EntityDicItmCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCalu>>();

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

        #region ICommonBIZ 成员

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItmCalu type = request.GetRequestValue<EntityDicItmCalu>();
                IDaoDic<EntityDicItmCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCalu>>();
                if (!string.IsNullOrEmpty(type.ItmId))
                {
                    UpdateItem(type.ItmId);
                }

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

        void UpdateItem(string ItmId)
        {
            ItemBIZ itemBiz = new ItemBIZ();
            EntityDicItmItem itm = new EntityDicItmItem();
            itm.ItmId = ItmId;
            List<EntityDicItmItem> result = itemBiz.GetItemByItmId(ItmId);
            foreach (EntityDicItmItem item in result)
            {
                item.ItmCaluFlag = 1;
                itemBiz.UpdateItem(item);
            }
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItmItem type = request.GetRequestValue<EntityDicItmItem>();
                type.ItmCaluFlag = 1;
                try
                {
                    response.Scusess = true;
                    response.SetResult(new ItemBIZ().UpdateItem(type));
                }
                catch (Exception ex)
                {
                    response.Scusess = false;
                    response.ErroMsg = ex.ToString();
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
                EntityDicItmCalu type = request.GetRequestValue<EntityDicItmCalu>();
                IDaoDic<EntityDicItmCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCalu>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(type));
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
                EntityDicItmCalu type = request.GetRequestValue<EntityDicItmCalu>();
                IDaoDic<EntityDicItmCalu> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCalu>>();
                if (!string.IsNullOrEmpty(type.ItmId))
                {
                    UpdateItem(type.ItmId);
                }
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
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                try
                {
                    response.Scusess = true;
                    response.SetResult(new ItemBIZ().GetItemByItmId(""));
                }
                catch (Exception ex)
                {
                    response.Scusess = false;
                    response.ErroMsg = ex.ToString();
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        #endregion
    }
}
