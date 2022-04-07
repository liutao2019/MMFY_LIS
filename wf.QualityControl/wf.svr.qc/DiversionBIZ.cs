using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.dicbasic;

namespace dcl.svr.qc
{
    public class DiversionBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicQcConvert convert = request.GetRequestValue<EntityDicQcConvert>();
                IDaoDic<EntityDicQcConvert> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcConvert>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(convert);
                    response.SetResult(convert);
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
                EntityDicQcConvert convert = request.GetRequestValue<EntityDicQcConvert>();

                IDaoDic<EntityDicQcConvert> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcConvert>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(convert);
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
                EntityDicQcConvert convert = request.GetRequestValue<EntityDicQcConvert>();
                IDaoDic<EntityDicQcConvert> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcConvert>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(convert);
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

            EntityDicQcConvert convert = request.GetRequestValue<EntityDicQcConvert>();
            IDaoDic<EntityDicQcConvert> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcConvert>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(convert));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            
            response.Scusess = true;
            response.SetResult(new ItemBIZ().GetItemByItmId(null));//参数为null表示查询全部项目字典数据

            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicInstrument instrumt = request.GetRequestValue<EntityDicInstrument>();
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(instrumt));
            }
            return response;
        }
    }
}
