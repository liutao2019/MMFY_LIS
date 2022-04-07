using System;
using System.Collections.Generic;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.svr.sample;
using dcl.svr.cache;
using dcl.svr.resultcheck;
using dcl.dao.interfaces;
using dcl.svr.dicbasic;

namespace dcl.svr.result
{
    public class PatEnterNewBIZ : IPatEnterNew
    {
        public List<EntityDicItmItem> GetItmByComID(string comId)
        {
            return new ItemBIZ().GetLisSubItemsByComId(comId);
        }

        /// <summary>
        /// 报告解读
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityPidReportMain GetReportInterpretation(string pat_id)
        {
            LabAuditBiz biz = new LabAuditBiz();
            return biz.GetReportInterpretation(pat_id);
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public List<EntityReportComment> GetReportComment(string pat_id)
        {
            List<EntityReportComment> listObrMsgImage = new List<EntityReportComment>();
            if (!string.IsNullOrEmpty(pat_id))
            {

                IDaoReportComment mainDao = DclDaoFactory.DaoHandler<IDaoReportComment>();
                return mainDao.GetReportComment(pat_id);
            }
            return listObrMsgImage;
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="rcKey"></param>
        /// <returns></returns>
        public bool DeleteReportComment(string rcKey)
        {
            IDaoReportComment mainDao = DclDaoFactory.DaoHandler<IDaoReportComment>();
            return mainDao.DeleteReportComment(rcKey);
        }
        /// <summary>
        /// 保存评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SaveReportComment(EntityReportComment model)
        {
            IDaoReportComment mainDao = DclDaoFactory.DaoHandler<IDaoReportComment>();
            return mainDao.SaveReportComment(model);
        }

        /// <summary>
        /// 追加条码
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_date"></param>
        /// <param name="pat_bar_code">原条码</param>
        /// <param name="barcode_to_append">新条码</param>
        /// <param name="pat_itr_name"></param>
        /// <param name="pat_sid"></param>
        /// <param name="loginId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public bool PatientAdditionalBarcode(string pat_id, DateTime pat_date, string pat_bar_code, string barcode_to_append, string pat_itr_name, string pat_sid, string loginId, string loginName)
        {
            bool result = false;
            SampProcessDetailBIZ detailBiz = new SampProcessDetailBIZ();

            DateTime dt = ServerDateTime.GetDatabaseServerDateTime();//数据库时间

            #region 添加新条码操作记录
            EntitySampProcessDetail detail = new EntitySampProcessDetail();
            detail.ProcUsercode = loginId;
            detail.ProcDate = dt;
            detail.ProcUsername = loginName;
            detail.ProcStatus = EnumBarcodeOperationCode.AppendBarcode.ToString();
            detail.ProcBarno = barcode_to_append;
            detail.ProcBarcode = barcode_to_append;
            detail.ProcPlace = string.Empty;
            detail.ProcContent = string.Format("条码信息追加到 资料日期：{0} 仪器：{1}，样本号：{2}", pat_date.ToString("yyyy-MM-dd"), pat_itr_name, pat_sid);
            detail.RepId = pat_id;

            result = detailBiz.SaveSampProcessDetailWithoutInterface(detail);
            #endregion

            #region 更新条码主表标志
            if (result)
            {
                EntitySampOperation operation = new EntitySampOperation();
                operation.OperationStatus = "560";
                operation.OperationTime = dt;
                operation.OperationID = loginId;
                operation.OperationName = loginName;

                result = new SampMainBIZ().UpdateSampMainStatusByBarId(operation, barcode_to_append);
            }
            #endregion

            #region 更新原条码信息与报告单信息的日志
            EntitySampProcessDetail detailOri = new EntitySampProcessDetail();
            detailOri.ProcUsercode = loginId;
            detailOri.ProcDate = dt;
            detailOri.ProcUsername = loginName;
            detailOri.ProcStatus = string.Empty;
            detailOri.ProcBarno = pat_bar_code;
            detailOri.ProcBarcode = pat_bar_code;
            detailOri.ProcPlace = string.Empty;
            detailOri.ProcContent = string.Format("条码追加，条码号：{0}", barcode_to_append);
            detailOri.RepId = pat_id;

            result = detailBiz.SaveSampProcessDetailWithoutInterface(detailOri);
            #endregion

            return result;
        }
    }
}
