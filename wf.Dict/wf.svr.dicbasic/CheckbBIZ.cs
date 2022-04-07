using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class CheckbBIZ : IDicCommon
    {
         public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicCheckPurpose check = request.GetRequestValue<EntityDicCheckPurpose>();
                IDaoDic<EntityDicCheckPurpose> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCheckPurpose>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(check);
                    response.SetResult(check);
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
                EntityDicCheckPurpose check = request.GetRequestValue<EntityDicCheckPurpose>();
                check.DelFlag = "1";
                IDaoDic<EntityDicCheckPurpose> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCheckPurpose>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(check);
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
                EntityDicCheckPurpose check = request.GetRequestValue<EntityDicCheckPurpose>();
                IDaoDic<EntityDicCheckPurpose> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCheckPurpose>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(check);
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

            EntityDicCheckPurpose check = request.GetRequestValue<EntityDicCheckPurpose>();
            IDaoDic<EntityDicCheckPurpose> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicCheckPurpose>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(check));
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
