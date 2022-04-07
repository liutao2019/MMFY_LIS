using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.servececontract;
using System.Collections.Generic;
using System.Configuration;

namespace dcl.svr.dicbasic
{
    public class TypeBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubProfession type = request.GetRequestValue<EntityDicPubProfession>();
                IDaoDic<EntityDicPubProfession> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubProfession>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                    if (!string.IsNullOrEmpty(strHospitalId))
                    {
                        type.ProOrgId = strHospitalId;
                    }

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
                EntityDicPubProfession type = request.GetRequestValue<EntityDicPubProfession>();
                type.ProDelFlag = "1";
                IDaoDic<EntityDicPubProfession> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubProfession>>();
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
                EntityDicPubProfession type = request.GetRequestValue<EntityDicPubProfession>();
                IDaoDic<EntityDicPubProfession> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubProfession>>();
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

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            IDaoDic<EntityDicPubProfession> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubProfession>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                List<EntityDicPubProfession> list = dao.Search(null);

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    list = list.FindAll(w => w.ProOrgId == strHospitalId);
                }

                response.SetResult(list);
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
