using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class Mitm_NoBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMachineCode code = request.GetRequestValue<EntityDicMachineCode>();
                IDaoDic<EntityDicMachineCode> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMachineCode>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(code);
                    response.SetResult(code);
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
                EntityDicMachineCode code = request.GetRequestValue<EntityDicMachineCode>();
                // code.DelFlag = "1";
                IDaoDic<EntityDicMachineCode> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMachineCode>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    //response.Scusess = dao.Update(code);
                    response.Scusess = dao.Delete(code);
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
                EntityDicMachineCode code = request.GetRequestValue<EntityDicMachineCode>();
                IDaoDic<EntityDicMachineCode> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMachineCode>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(code);
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

            EntityDicMachineCode code = request.GetRequestValue<EntityDicMachineCode>();
            IDaoDic<EntityDicMachineCode> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMachineCode>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(code));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicInstrument code = request.GetRequestValue<EntityDicInstrument>();
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(code));
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            response.Scusess = true;
            response.SetResult(new ItemBIZ().GetItemByItmId(""));

            return response;
        }
    }
}