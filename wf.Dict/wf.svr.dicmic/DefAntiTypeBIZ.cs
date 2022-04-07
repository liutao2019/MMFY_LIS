using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicmic
{
    public class DefAntiTypeBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDefAntiType micGechan = request.GetRequestValue<EntityDefAntiType>();
                IDaoDic<EntityDefAntiType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefAntiType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    try
                    {
                        response.Scusess = dao.Delete(micGechan);
                    }
                    catch (Exception ex)
                    {
                        response.Scusess = false;
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
                EntityDefAntiType micGechan = request.GetRequestValue<EntityDefAntiType>();
                IDaoDic<EntityDefAntiType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefAntiType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(micGechan);
                    response.SetResult(micGechan);
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
            throw new NotImplementedException();
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDefAntiType micGechan = request.GetRequestValue<EntityDefAntiType>();
            IDaoDic<EntityDefAntiType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefAntiType>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(micGechan));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDefAntiType micGechan = request.GetRequestValue<EntityDefAntiType>();
                IDaoDic<EntityDefAntiType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefAntiType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(micGechan);
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
            throw new NotImplementedException();
        }
    }
}
