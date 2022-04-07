using System;
using System.Collections.Generic;
using System.Linq;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using Lib.LogManager;
using System.Configuration;

namespace dcl.svr.dicbasic
{
    public class InstrmtBIZ : IDicCommon
    {
        /// <summary>
        /// 是否为新增事件
        /// </summary>
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicInstrument instrumt = request.GetRequestValue<EntityDicInstrument>();
                IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(instrumt);
                    response.SetResult(instrumt);
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
                EntityDicInstrument instrumt = request.GetRequestValue<EntityDicInstrument>();
                instrumt.DelFlag = "1";
                IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(instrumt);
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
                EntityDicInstrument instrumt = request.GetRequestValue<EntityDicInstrument>();
                IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(instrumt);
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
                List<EntityDicInstrument> list = dao.Search(instrumt);

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    EntityResponse type = new TypeBIZ().Search(request);
                    List<EntityDicPubProfession> listRv = type.GetResult() as List<EntityDicPubProfession>;
                    list = list.FindAll(w => listRv.FindIndex(z => z.ProId == w.ItrLabId && z.ProType == 1) > -1);
                }

                response.SetResult(list);
            }
            return response;
        }


        public List<EntityDicInstrument> GetInstrumentByComIds(List<string> ComIdList)
        {
            try
            {

                IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
                List<EntityDicInstrument> list = new List<EntityDicInstrument>();
                if (dao != null)
                {
                    list = dao.GetInstrumentByComIds(ComIdList);
                }
                return list;

            }
            catch (Exception ex)
            {
                Logger.LogException("GetInstrumentByComIds", ex);
                return new List<EntityDicInstrument>();
            }
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityReport instrumt = request.GetRequestValue<EntityReport>();
            instrumt = request.GetRequestValue<EntityReport>();
            IDaoDic<EntityReport> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityReport>>();
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

        public EntityResponse View(EntityRequest request)
        {
            return new EntityResponse();
        }
        public int GetItrHostFlag(string itr_id)
        {
            int result = 1;
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            List<EntityDicInstrument> list = new List<EntityDicInstrument>();
            if (dao != null)
            {
                result = dao.GetItrHostFlag(itr_id);
            }
            return result;
        }

        /// <summary>
        /// 根据仪器ID或者仪器物理组别获取仪器
        /// </summary>
        /// <param name="itrId"></param>
        /// <param name="itrType"></param>
        /// <returns></returns>
        public List<EntityDicInstrument> GetInstrumentByItridOrItrType(string itrId = "", string itrType = "")
        {
            List<EntityDicInstrument> listInstrmt = new List<EntityDicInstrument>();
            IDaoDicInstrument instDao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (instDao != null)
            {
                listInstrmt = instDao.GetInstrumentByItridOrItrType(itrId, itrType);
            }
            return listInstrmt;
        }

        public List<EntityDicInstrument> GetHistoryReletedInstrumentByRepId(string repId)
        {
            List<EntityDicInstrument> listItr = new List<EntityDicInstrument>();
            IDaoDicInstrument itrDao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (itrDao != null)
            {
                listItr = itrDao.GetHistoryReletedInstrumentByRepId(repId);
            }
            return listItr;
        }
    }
}
