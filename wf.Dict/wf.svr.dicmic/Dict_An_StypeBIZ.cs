using System;
using dcl.entity;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicmic
{
    public class Dict_An_StypeBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntitype antiType = request.GetRequestValue<EntityDicMicAntitype>();
                antiType.ADelFlag = 1;
                IDaoDic<EntityDicMicAntitype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntitype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(antiType);
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
                EntityDicMicAntitype antiType = request.GetRequestValue<EntityDicMicAntitype>();
                IDaoDic<EntityDicMicAntitype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntitype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(antiType);
                    response.SetResult(antiType);
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

            EntityDicMicAntitype antiType = request.GetRequestValue<EntityDicMicAntitype>();
            IDaoDic<EntityDicMicAntitype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntitype>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(antiType));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicAntitype antiType = request.GetRequestValue<EntityDicMicAntitype>();
                IDaoDic<EntityDicMicAntitype> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntitype>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(antiType);
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
