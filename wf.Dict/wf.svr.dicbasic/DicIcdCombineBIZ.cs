using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class DicIcdCombineBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityDicPubIcdCombine icd = request.GetRequestValue<EntityDicPubIcdCombine>();
            IDaoDic<EntityDicPubIcdCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcdCombine>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = dao.Delete(icd);
            }
            return response;
        }

        public EntityResponse New(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Other(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityDicPubIcdCombine icd = request.GetRequestValue<EntityDicPubIcdCombine>();
            IDaoDic<EntityDicPubIcdCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcdCombine>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(icd.IcdId));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            List<EntityDicPubIcdCombine> icdList = request.GetRequestValue<List<EntityDicPubIcdCombine>>();
            IDaoDic<EntityDicPubIcdCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubIcdCombine>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                foreach (var item in icdList)
                {
                    response.Scusess = dao.Save(item);
                }
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
