using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;

namespace dcl.svr.sample
{
    /// <summary>
    /// 条码项目
    /// </summary>
    public class SampDetailBIZ : DclBizBase, ISampDetail
    {
        /// <summary>
        /// 获取条码组合明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public List<EntitySampDetail> GetSampDetail(String sampBarId)
        {
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetail(sampBarId);

            return listSampDetail;
        }
        /// <summary>
        /// 获取条码组合明细  标本上机查询使用
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public List<EntitySampDetail> GetSampDetailByBarCode(String sampBarId)
        {
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetailByBarCode(sampBarId);

            return listSampDetail;
        }

        /// <summary>
        /// 删除所有条码明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean DeleteSampDetailAll(String sampBarId)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
            {
                daoDetail.Dbm = Dbm;
                result = daoDetail.DeleteSampDetailAll(sampBarId);
            }

            return result;

        }


        public Boolean DeleteSampDetail(List<EntitySampDetail> listSampDetail)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.DeleteSampDetail(listSampDetail);

            return result;

        }

        /// <summary>
        /// 根据病人标识ID和条码号更新上机标志
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="sampBarCode"></param>
        /// <returns></returns>
        //public Boolean UpdateSampDetailSampFlag(String repId, string sampBarCode)
        //{
        //    bool result = false;
        //    IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
        //    if (daoDetail != null)
        //        result = daoDetail.UpdateSampDetailSampFlag(repId, sampBarCode);
        //    return result;
        //}

        /// <summary>
        /// 根据条码号和病人组合ID更新标志
        /// </summary>
        /// <param name="sampBarCode">条码号</param>
        /// <param name="comId">组合ID</param>
        /// <returns></returns>
        public bool UpdateSampDetailSampFlagByComId(string sampBarCode, List<string> listComId, string flag)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.UpdateSampDetailSampFlagByComId(sampBarCode, listComId, flag);
            return result;
        }

        /// <summary>
        /// 获取条码组合明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public List<EntitySampDetail> GetSampDetailByYzId(List<String> listYzId, string srcName)
        {
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetailByYzId(listYzId, srcName);

            return listSampDetail;
        }

        /// <summary>
        /// 根据条码号和仪器ID获取条码上机信息（项目、组合、通道码）
        /// </summary>
        /// <param name="sampBarId">条码号</param>
        /// <param name="itrId">仪器ID</param>
        /// <returns></returns>
        public List<EntitySampDetailMachineCode> GetSampDetailMachineCodeByItrId(string sampBarId, string itrId)
        {
            List<EntitySampDetailMachineCode> listSampDetail = new List<EntitySampDetailMachineCode>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetailMachineCodeByItrId(sampBarId, itrId);

            return listSampDetail;
        }

        /// <summary>
        /// 根据条码号和lis组合编码获取条码组合信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="listComId"></param>
        /// <returns></returns>
        public List<EntitySampDetail> GetSampDetailByBarCodeAndComId(String sampBarId, List<string> listComId)
        {
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetailByBarCodeAndComId(sampBarId, listComId);

            return listSampDetail;
        }

        /// <summary>
        /// 根据所选上机标志和条码号更新上机标志
        /// </summary>
        /// <param name="commflag">上机标志</param>
        /// <param name="sampBarCode">条码号</param>
        /// <returns></returns>
        public Boolean UpdateSampDetailCommFlag(string commflag, string sampBarCode)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.UpdateSampDetailCommFlag(commflag, sampBarCode, null);
            return result;
        }

        /// <summary>
        /// 根据所选上机标志和条码号和his项目代码来更新上机标志
        /// </summary>
        /// <param name="commflag">上机标志</param>
        /// <param name="sampBarCode">条码号</param>
        /// <param name="orderCode">his项目代码</param>
        /// <returns></returns>
        public Boolean UpdateSampDetailCommFlagByCode(string commflag, string sampBarCode, string orderCode)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.UpdateSampDetailCommFlag(commflag, sampBarCode, orderCode);
            return result;
        }


        /// <summary>
        /// 根据自增ID更新标志
        /// </summary>
        /// <param name="detSn"></param>
        /// <returns></returns>
        public bool UpdateSampFlagByDetSn(string detSn)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.UpdateSampFlagByDetSn(detSn);
            return result;
        }

        /// <summary>
        /// 根据条码号更新标志
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public bool UpdateSampFlagByBarCode(string barCode)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.UpdateSampFlagByBarCode(barCode);
            return result;
        }

        /// <summary>
        /// 根据条码号和组合id查询是否存在条码
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="listComIds"></param>
        /// <returns></returns>
        public Int32 GetSampDetailCount(string barCode, List<string> listComIds)
        {
            Int32 count = 0;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                count = daoDetail.GetSampDetailCount(barCode, listComIds);
            return count;
        }

        /// <summary>
        /// 保存条码组合明细
        /// </summary>
        /// <param name="listSampDetail"></param>
        /// <returns></returns>
        public Boolean SaveSampDetail(List<EntitySampDetail> listSampDetail)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.SaveSampDetail(listSampDetail);
            return result;
        }

        /// <summary>
        /// 判断是否存在不同天的医嘱信息
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        public Boolean ExistDifferentOCCDate(List<String> listSampBarId)
        {
            bool result = false;
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                result = daoDetail.ExistDifferentOCCDate(listSampBarId);
            return result;
        }

        /// <summary>
        /// 根据barId批量获取条码明细
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        public List<EntitySampDetail> GetSampDetailByListBarId(List<string> listSampBarId)
        {
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();

            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listSampDetail = daoDetail.GetSampDetailByListBarId(listSampBarId);

            return listSampDetail;
        }

        /// <summary>
        ///  获取报告对应的医嘱id
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        public List<string> GetPatOrderIDs(string RepId)
        {
            List<string> listOrderIds = new List<string>();
            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
            if (daoDetail != null)
                listOrderIds = daoDetail.GetPatOrderIDs(RepId);
            return listOrderIds;
        }
    }
}
