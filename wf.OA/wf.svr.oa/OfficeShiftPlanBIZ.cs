using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.oa
{
    class OfficeShiftPlanBIZ : IOfficeShiftDictDetail
    {
        /// <summary>
        /// 获得排班计划表
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public List<EntityOaDicShiftDetail> GetShiftPlan(DateTime sDate, DateTime eDate, string strType)
        {
            List<EntityOaDicShiftDetail> list = new List<EntityOaDicShiftDetail>();
            IDaoOaDicShiftDetail dao = DclDaoFactory.DaoHandler<IDaoOaDicShiftDetail>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetShiftPlan(sDate, eDate, strType);
            }
            return list;
        }

        /// <summary>
        /// 登记时插入排班计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertShiftPlan(EntityOaDicShiftDetail entity)
        {
            IDaoOaDicShiftDetail dao = DclDaoFactory.DaoHandler<IDaoOaDicShiftDetail>();
            if (dao == null)
            {
                return -1;
            }
            else
            {
                return dao.InsertShiftPlan(entity);
            }
        }



        /// <summary>
        /// 更新排班计划
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public bool UpdateShiftPlan(EntityRequest request)
        {
            bool isSuccess = false;
            if (request != null)
            {
                string[] stringPar = new string[4];
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                List<EntityOaDicShiftDetail> listShiftDetail = new List<EntityOaDicShiftDetail>();
                object objPar = dict["par"];
                object objList = dict["listShiftDetail"];
                if (objPar != null)
                {
                    stringPar = objPar as string[];
                }
                if (objList != null)
                {
                    listShiftDetail = objList as List<EntityOaDicShiftDetail>;
                }
                IDaoOaDicShiftDetail dao = DclDaoFactory.DaoHandler<IDaoOaDicShiftDetail>();
                if (dao != null)
                {
                    isSuccess = dao.DeleteShiftPlan(stringPar[0], stringPar[1], stringPar[2], stringPar[3]);
                    foreach (EntityOaDicShiftDetail detail in listShiftDetail)
                    {
                        dao.InsertShiftPlan(detail);
                        isSuccess = true;
                    }
                }
                else
                {
                    isSuccess= false;
                }
            }
            else {
                isSuccess= false;
            }
            return isSuccess;
        }

        public bool CopyShiftPlan(string sFrom, string sTo, string timeFrom, string timeTo)
        {
            bool isSuccess = false;
            IDaoOaDicShiftDetail dao = DclDaoFactory.DaoHandler<IDaoOaDicShiftDetail>();
            if (dao != null)
            {
                isSuccess = dao.DeleteShiftPlan(null, timeFrom, timeTo, null);
                for (DateTime tmp =Convert.ToDateTime(sFrom); tmp <= Convert.ToDateTime(sTo); tmp = tmp.AddDays(1))
                {
                    isSuccess = dao.CopyShiftPlan(timeFrom, tmp.ToString());
                    timeFrom =Convert.ToDateTime( timeFrom).AddDays(1).ToString();
                }
                return isSuccess;
            }
            else
            {
                return false;
            }
        }
    }
}
