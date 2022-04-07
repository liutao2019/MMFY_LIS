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
    //抗生素大类字典
    public class Dic_Antibio_TypeBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {

            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicAntibioType AntibioType = request.GetRequestValue<EntityDicAntibioType>();
                AntibioType.DelFlag = 1;
                IDaoDic<EntityDicAntibioType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicAntibioType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(AntibioType);
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
                EntityDicAntibioType AntibioType = request.GetRequestValue<EntityDicAntibioType>();

                IDaoDic<EntityDicAntibioType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicAntibioType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(AntibioType);
                    response.SetResult(AntibioType);
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

            EntityDicAntibioType AntibioType = request.GetRequestValue<EntityDicAntibioType>();
            IDaoDic<EntityDicAntibioType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicAntibioType>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(null));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicAntibioType AntibioType = request.GetRequestValue<EntityDicAntibioType>();
                IDaoDic<EntityDicAntibioType> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicAntibioType>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(AntibioType);
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
