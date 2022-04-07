using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.sample
{
    public class SampReturnBIZ : ISampReturn
    {
        /// <summary>
        /// 处理回退信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean HandleSampReturn(String sampBarId)
        {
            bool result = false;
            IDaoSampReturn daoDetail = DclDaoFactory.DaoHandler<IDaoSampReturn>();
            if (daoDetail != null)
                result = daoDetail.HandleSampReturn(sampBarId);

            return result;
        }

        /// <summary>
        /// 新增回退信息
        /// </summary>
        /// <param name="sampReturn"></param>
        /// <returns></returns>
        public Boolean SaveSampReturn(EntitySampReturn sampReturn)
        {
            bool result = false;
            IDaoSampReturn daoDetail = DclDaoFactory.DaoHandler<IDaoSampReturn>();
            if (daoDetail != null)
                result = daoDetail.SaveSampReturn(sampReturn);

            return result;
        }

        public List<EntitySampReturn> GetSampReturn(EntitySampQC sampQc)
        {
            List<EntitySampReturn> listReturn = new List<EntitySampReturn>();
          
            listReturn = SampReturnCache.Current.DclCache.FindAll(w=> w.ReturnDate > Convert.ToDateTime(sampQc.StartDate)
                                                                                                   && w.ReturnDate < Convert.ToDateTime(sampQc.EndDate));

            if (!string.IsNullOrEmpty(sampQc.PidDeptCode))
            {
                if (sampQc.PidDeptCode.Contains("&"))
                {
                    string[] dept = sampQc.PidDeptCode.Split('&');

                    List<EntitySampReturn> sum = new List<EntitySampReturn>();
                    List<EntitySampReturn> temp = new List<EntitySampReturn>();
                    foreach (var item in dept)
                    {
                        temp = listReturn.FindAll(w => w.ReturnDeptCode == item);
                        sum.AddRange(temp);
                    }
                    listReturn = sum;
                }
                else
                {
                    listReturn = listReturn.FindAll(w => w.ReturnDeptCode == sampQc.PidDeptCode);
                }
            }


            if (sampQc.HandleProc != ReturnProc.全部)
            {
                bool handle = false;
                if (sampQc.HandleProc == ReturnProc.已处理)
                    handle =true;
                listReturn=listReturn.FindAll(w => w.ReturnProcFlag == handle);
            }
            //只查询住院
            if (sampQc.SearchHospital)
            {
                listReturn=listReturn.FindAll(w => w.PidSrcId =="108");
            }
            return listReturn;
        }

        public bool UpdateSampReturnFlag(EntitySampMain sampMain)
        {
            bool result = false;
            result = new SampMainBIZ().UpdateSampReturnFlag(sampMain);
            return result;
        }

        public bool UpdateReturnMessage(EntitySampReturn sampMain)
        {
            bool result = false;
            IDaoSampReturn daoDetail = DclDaoFactory.DaoHandler<IDaoSampReturn>();
            if (daoDetail != null)
            {
                result = daoDetail.UpdateReturnMessage(sampMain);
        
            }
            return result;
        }

        public void RefereshReturnMessage()
        {
            dcl.svr.cache.SampReturnCache.Current.Refresh();
        }
    }
}
