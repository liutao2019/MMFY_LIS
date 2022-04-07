using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class SampReturnBIZ : IDicCommon
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
                EntityDicSampReturn sampReturn = request.GetRequestValue<EntityDicSampReturn>();
                IDaoDic<EntityDicSampReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampReturn>>();
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
                EntityDicSampReturn sampReturn = request.GetRequestValue<EntityDicSampReturn>();
                //sampReturn.SamDelFlag = "1";
                IDaoDic<EntityDicSampReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampReturn>>();
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
                EntityDicSampReturn sampReturn = request.GetRequestValue<EntityDicSampReturn>();
                IDaoDic<EntityDicSampReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampReturn>>();
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

            EntityDicSampReturn sampReturn = request.GetRequestValue<EntityDicSampReturn>();
            IDaoDic<EntityDicSampReturn> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampReturn>>();
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
