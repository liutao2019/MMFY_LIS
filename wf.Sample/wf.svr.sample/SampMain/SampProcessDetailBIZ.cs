using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dcl.svr.sample
{
    /// <summary>
    /// 条码流程
    /// </summary>
    public class SampProcessDetailBIZ : DclBizBase, ISampProcessDetail
    {
        public List<EntitySampProcessDetail> GetSampProcessDetail(String sampBarId)
        {
            List<EntitySampProcessDetail> listProcessDetail = new List<EntitySampProcessDetail>();

            IDaoSampProcessDetail daoProcess = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (daoProcess != null)
                listProcessDetail = daoProcess.GetSampProcessDetail(sampBarId).OrderBy(o => o.ProcNo).ToList();

            return listProcessDetail;
        }

        /// <summary>
        /// 保存流程信息
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public Boolean SaveSampProcessDetail(EntitySampOperation operation, EntitySampMain sampMain)
        {
            bool result = false;

            EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();

            sampProcessDetial.ProcDate = operation.OperationTime;
            sampProcessDetial.ProcUsercode = operation.OperationID;
            sampProcessDetial.ProcUsername = operation.OperationName;
            sampProcessDetial.ProcStatus = operation.OperationStatus;
            sampProcessDetial.ProcBarno = sampMain.SampBarId;
            sampProcessDetial.ProcBarcode = sampMain.SampBarCode;
            sampProcessDetial.ProcPlace = operation.OperationPlace;
            sampProcessDetial.ProcTimes = sampMain.SampReturnTimes + 1;
            sampProcessDetial.ProcContent = operation.Remark;
            sampProcessDetial.RepId = operation.RepId;

            IDaoSampProcessDetail daoProcess = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (daoProcess != null)
            {
                daoProcess.Dbm = Dbm;
                result = daoProcess.SaveSampProcessDetail(sampProcessDetial);
            }
            if (sampMain.ListSampDetail.Count == 0)
                sampMain.ListSampDetail = new SampDetailBIZ().GetSampDetail(sampMain.SampBarId);

            //(电视机TAT监控)对表tat_pro_record进行数据插入或更新
            new TatProRecordNewBIZ().TatRecode(operation, sampMain);
            //手工条码不执行接口
            if (string.IsNullOrEmpty(sampMain.SampInfo) || sampMain.SampInfo != "122")
                DCLExtInterfaceFactory.DCLExtInterface.ExecuteInterfaceAfterAsync(operation, sampMain);

            return result;
        }


        /// <summary>
        /// 细菌调用
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Boolean SaveSampProcessDetail(EntitySampOperation operation, string barcode)
        {
            bool result = false;

            EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();

            sampProcessDetial.ProcDate = operation.OperationTime;
            sampProcessDetial.ProcUsercode = operation.OperationID;
            sampProcessDetial.ProcUsername = operation.OperationName;
            sampProcessDetial.ProcStatus = operation.OperationStatus;
            sampProcessDetial.ProcBarno = barcode;
            sampProcessDetial.ProcBarcode = barcode;
            sampProcessDetial.ProcPlace = operation.OperationPlace;
            sampProcessDetial.ProcTimes = 1;
            sampProcessDetial.ProcContent = operation.Remark;
            sampProcessDetial.RepId = operation.RepId;

            IDaoSampProcessDetail daoProcess = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (daoProcess != null)
            {
                daoProcess.Dbm = this.Dbm;
                result = daoProcess.SaveSampProcessDetail(sampProcessDetial);
            }
            //   new SampMainConfirmInterface().ExecuteInterface(operation, sampMain);
            return result;
        }




        /// <summary>
        /// 直接记录流程信息，不执行院网接口
        /// </summary>
        /// <param name="sampProcessDetail"></param>
        /// <returns></returns>
        public Boolean SaveSampProcessDetailWithoutInterface(EntitySampProcessDetail sampProcessDetail)
        {
            bool result = false;
            IDaoSampProcessDetail daoProcess = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (daoProcess != null)
                result = daoProcess.SaveSampProcessDetail(sampProcessDetail);

            return result;
        }



        public EntitySampProcessDetail GetLastSampProcessDetail(String sampBarId)
        {
            EntitySampProcessDetail processDetail = new EntitySampProcessDetail();
            List<EntitySampProcessDetail> listProcessDetail = GetSampProcessDetail(sampBarId);
            if (listProcessDetail != null && listProcessDetail.Count > 0)
            {
                processDetail = listProcessDetail[listProcessDetail.Count - 1];
            }

            return processDetail;
        }

        public string GetDeletePatId(string patId, string patName, string timeFrom, string timeTo)
        {
            string deletePatId = string.Empty;
            IDaoSampProcessDetail daoProcess = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (daoProcess != null)
                deletePatId = daoProcess.GetDeletePatId(patId, patName, timeFrom, timeTo);
            return deletePatId;
        }
    }
}
