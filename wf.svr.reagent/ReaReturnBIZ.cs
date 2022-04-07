using dcl.entity;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace wf.svr.dicreagent
{
    public class ReaReturnBIZ : IDicCommon
    {
        //public override string MainTable
        //{
        //    get { return BarcodeTable.Message.TableName; }
        //}

        //public override string PrimaryKey
        //{
        //    get { return BarcodeTable.Message.ID; }
        //}

        //public override DataSet Search(string where)
        //{
        //    return doSearch(new DataSet(), SearchSQL + where);
        //}
        
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicReaReturn sampReturn = request.GetRequestValue<EntityDicReaReturn>();
                IDaoDic<EntityDicReaReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaReturn>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(sampReturn);
                    response.SetResult(sampReturn);
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
                EntityDicReaReturn sampReturn = request.GetRequestValue<EntityDicReaReturn>();
                //sampReturn.SamDelFlag = "1";
                IDaoDic<EntityDicReaReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaReturn>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    //response.Scusess = dao.Update(sampReturn);
                    response.Scusess = dao.Delete(sampReturn);
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
                EntityDicReaReturn sampReturn = request.GetRequestValue<EntityDicReaReturn>();
                IDaoDic<EntityDicReaReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaReturn>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(sampReturn);
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

            EntityDicReaReturn sampReturn = request.GetRequestValue<EntityDicReaReturn>();
            IDaoDic<EntityDicReaReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaReturn>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(sampReturn));
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
