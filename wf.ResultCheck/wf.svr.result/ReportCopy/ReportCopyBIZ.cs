using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    public class ReportCopyBIZ : IReportCopy
    {
        public string CopyPatientsInfoCustomSid(List<string> pat_id, DateTime dtTime, string strItrId, List<decimal> newSid)
        {
            StringBuilder sbPatId = new StringBuilder();
            foreach (string strPatId in pat_id)
            {
                sbPatId.Append(string.Format(",'{0}'", strPatId));
            }
            sbPatId.Remove(0, 1);

            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            listPat = new PidReportMainBIZ().SearchPatientForReportCopyUse(sbPatId.ToString()); //根据多个病人标识ID查询病人信息

            List<EntityPidReportDetail> listPatMi = new List<EntityPidReportDetail>();
            listPatMi = new PidReportDetailBIZ().SearchPidReportDetailByMulitRepId(sbPatId.ToString());//根据多个病人标识ID查询病人组合明细数据

            int sidIndex = 0;//样本LIST索引
            StringBuilder sbCopyPatId = new StringBuilder();
            List<EntitySampProcessDetail> listSaveDetail = new List<EntitySampProcessDetail>();
            foreach (var infoPat in listPat)
            {
                string strPatOldId = infoPat.RepId;
                string strPatItrOldId = infoPat.RepItrId;

                DateTime patolddate = ServerDateTime.GetDatabaseServerDateTime();//初始化一个服务器时间
                if (infoPat.RepInDate != null)
                {
                    patolddate = infoPat.RepInDate.Value;
                }

                string strPatSid = newSid.Count > sidIndex ? newSid[sidIndex].ToString() : infoPat.RepSid;
                string strPatId = strItrId + dtTime.ToString("yyyyMMdd") + strPatSid;
                infoPat.RepId = strPatId;
                infoPat.RepSid = strPatSid;//更新样本号
                infoPat.RepItrId = strItrId;//更新仪器代码
                infoPat.RepInDate = dtTime;

                //不复制一审与二审的时间与操作者信息
                if (true)
                {
                    infoPat.RepReportDate = null;
                    infoPat.RepStatus = 0;

                    //drPat["pat_send_flag"] = DBNull.Value;
                    infoPat.RepSendFlag = 0;

                    infoPat.RepReportUserId = null;
                    infoPat.RepAuditDate = null;
                    infoPat.RepAuditUserId = null;
                }

                sbCopyPatId.Append(string.Format(",'{0}'", strPatId));

                foreach (var infoPatMi in listPatMi.Where(w => w.RepId == strPatOldId).ToList())
                {
                    infoPatMi.RepId = strPatId;
                }
                if (!string.IsNullOrEmpty(infoPat.RepBarCode))
                {
                    EntitySampProcessDetail eySamProDetail = new EntitySampProcessDetail();
                    eySamProDetail.ProcBarcode = string.Empty;
                    eySamProDetail.ProcBarno = string.Empty;
                    eySamProDetail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                    eySamProDetail.ProcUsercode = string.Empty;
                    eySamProDetail.ProcUsername = string.Empty;
                    eySamProDetail.ProcStatus = "20";
                    eySamProDetail.RepId = strPatId;
                    eySamProDetail.ProcContent = string.Format("复制:原资料日期：{0} 仪器代码：{1}->{2}", patolddate.ToString("yyyy-MM-dd"), strPatItrOldId, strItrId);

                    listSaveDetail.Add(eySamProDetail);
                }
                sidIndex++;//递增索引
            }
            sbCopyPatId.Remove(0, 1);

            List<EntityPidReportMain> listCopyPat = new List<EntityPidReportMain>();
            listCopyPat = new PidReportMainBIZ().SearchPatientForReportCopyUse(sbCopyPatId.ToString()); //根据多个病人标识ID查询病人信息

            String strMes = string.Empty;

            if (listCopyPat.Count <= 0)
            {
                new PidReportMainBIZ().InsertNewPatient(listPat);//插入病人信息表(List)

                new PidReportDetailBIZ().InsertNewReportDetail(listPatMi);//插入病人组合明细(List)

                //插入bc_sign表数据（暂时先调用dao的方法，后面修改时再改为用BIZ的方法，并完善操作信息）
                IDaoSampProcessDetail dao = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
                bool result = false;
                if (dao != null)
                {
                    foreach (var infoDetail in listSaveDetail)
                    {
                        result = dao.SaveSampProcessDetail(infoDetail);
                    }
                }
            }
            else
            {
                strMes = "此批病人有资料已经存在";
            }
            return strMes;
        }

        /// <summary>
        /// //复制患者信息(不可自定义样本号)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId"></param>
        /// <returns></returns>
        public String CopyPatientsInfo(List<string> pat_id, DateTime dtTime, String strItrId)
        {
            StringBuilder sbPatId = new StringBuilder();
            foreach (string strPatId in pat_id)
            {
                sbPatId.Append(string.Format(",'{0}'", strPatId));
            }

            sbPatId.Remove(0, 1);

            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            listPat = new PidReportMainBIZ().SearchPatientForReportCopyUse(sbPatId.ToString());//根据多个病人标识ID查询病人信息

            List<EntityPidReportDetail> listPatMi = new List<EntityPidReportDetail>();
            listPatMi = new PidReportDetailBIZ().SearchPidReportDetailByMulitRepId(sbPatId.ToString());//根据多个病人标识ID查询病人组合明细数据

            StringBuilder sbCopyPatId = new StringBuilder();

            List<EntitySampProcessDetail> listSaveDetailAL = new List<EntitySampProcessDetail>();

            foreach (var infoPat in listPat)
            {
                string strPatOldId = infoPat.RepId;
                string strPatItrOldId = infoPat.RepItrId;
                string strPatSid = infoPat.RepSid;

                DateTime patolddate = ServerDateTime.GetDatabaseServerDateTime();//初始化一个服务器时间
                if (infoPat.RepInDate != null)
                    patolddate = infoPat.RepInDate.Value;

                string strPatId = strItrId + dtTime.ToString("yyyyMMdd") + strPatSid;
                infoPat.RepId = strPatId;
                infoPat.RepItrId = strItrId;//更新仪器代码
                infoPat.RepInDate = dtTime;

                //不复制一审与二审的时间与操作者信息
                if (true)
                {
                    infoPat.RepReportDate = null;
                    infoPat.RepStatus = 0;

                    //drPat["pat_send_flag"] = DBNull.Value;
                    infoPat.RepSendFlag = 0;

                    infoPat.RepReportUserId = null;
                    infoPat.RepAuditDate = null;
                    infoPat.RepAuditUserId = null;
                }

                sbCopyPatId.Append(string.Format(",'{0}'", strPatId));

                foreach (var infoPatMi in listPatMi.Where(w => w.RepId == strPatOldId).ToList())
                {
                    infoPatMi.RepId = strPatId;
                }
                if (!string.IsNullOrEmpty(infoPat.RepBarCode))
                {
                    //老版本的操作信息是没有的，到后面根据实际需求再进行完善(20171205)
                    EntitySampProcessDetail eySamProDetail = new EntitySampProcessDetail();
                    eySamProDetail.ProcBarcode = string.Empty;
                    eySamProDetail.ProcBarno = string.Empty;
                    eySamProDetail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                    eySamProDetail.ProcUsercode = string.Empty;
                    eySamProDetail.ProcUsername = string.Empty;
                    eySamProDetail.ProcStatus = "20";
                    eySamProDetail.RepId = strPatId;
                    eySamProDetail.ProcContent = string.Format("复制: 原资料日期：{0} 仪器代码：{1}->{2}", patolddate.ToString("yyyy-MM-dd"), strPatItrOldId, strItrId);

                    listSaveDetailAL.Add(eySamProDetail);//记录需要插入标本流转明细表的数据

                }
            }

            sbCopyPatId.Remove(0, 1);
            
            List<EntityPidReportMain> listCopyPat = new List<EntityPidReportMain>();
            listCopyPat = new PidReportMainBIZ().SearchPatientForReportCopyUse(sbCopyPatId.ToString());

            String strMes = string.Empty;

            if (listCopyPat.Count <= 0)
            {
                new PidReportMainBIZ().InsertNewPatient(listPat);//插入病人信息表(List)
                
                new PidReportDetailBIZ().InsertNewReportDetail(listPatMi);//插入病人组合明细(List)
                
                //插入bc_sign表数据（暂时先调用dao的方法，后面修改时用BIZ方法，并完善操作信息）
                IDaoSampProcessDetail dao = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
                bool result = false;
                if (dao != null)
                {
                    foreach (var infoAL in listSaveDetailAL)
                    {
                        result = dao.SaveSampProcessDetail(infoAL);
                    }
                }
            }
            else
            {
                strMes = "此批病人有资料已经存在";
            }
            return strMes;
        }

    }
}
