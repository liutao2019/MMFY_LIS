using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.cache;
using System.Data;

namespace dcl.svr.qc
{
    public class QcResultBIZ : IObrQcResult
    {
        public List<EntityObrQcResult> GetQcResultByItrId(string ItrId)
        {
            List<EntityObrQcResult> QcResult = new List<EntityObrQcResult>();
            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                EntityObrQcResultQC resultQc = new EntityObrQcResultQC();
                resultQc.StateTime = DateTime.Now.Date;
                resultQc.EndTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                resultQc.ItrId = ItrId;

                QcResult = dao.QcResultQuery(resultQc);
            }
            return QcResult;
        }

        public bool InsertQcResult(string ItrId, string ItmId, string QcValue, string NoType, string MatSn, DateTime QcDate)
        {
            bool QcResult = false;
            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                QcResult = dao.InsertQcResult(ItrId, ItmId, QcValue, NoType, MatSn, QcDate);
            }
            return QcResult;
        }

        /// <summary>
        /// 获取质控图表列表
        /// </summary>
        /// <param name="strItrId">仪器ID</param>
        /// <param name="dtSDate">开始时间</param>
        /// <param name="dtEDate">结束时间</param>
        /// <param name="viewType">显示类型</param>
        /// <param name="radarView">是否是雷达图</param>
        /// <returns></returns>
        public List<EntityQcTreeView> GetQcTreeView(string strItrId, DateTime dtSDate, DateTime dtEDate, QcTreeViewType viewType, bool radarView)
        {
            List<EntityQcTreeView> listView = new List<EntityQcTreeView>();

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                listView = dao.GetQcTreeView(strItrId, dtSDate, dtEDate, viewType, radarView);
            }

            return listView;
        }

        /// <summary>
        /// 获取质控数据（预审）
        /// </summary>
        /// <param name="listResultQc"></param>
        /// <returns></returns>
        public List<EntityObrQcResult> GetQcResult(List<EntityObrQcResultQC> listResultQc)
        {
            List<EntityObrQcResult> listResult = new List<EntityObrQcResult>();

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                foreach (EntityObrQcResultQC resultQc in listResultQc)
                {
                    List<EntityObrQcResult> listValue = dao.GetQcResult(resultQc);

                    if (listValue.Count > 0)
                    {
                        Dictionary<int, SIValue> siValue = GetListSIValue();
                        int count = 1;
                        string nextAuditHour = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("QCTwoAuditTime");

                        if (resultQc.IsCheckGrubbs)
                        {
                            foreach (EntityObrQcResult drQcValue in listValue)
                            {
                                if ((drQcValue.MatItmCcv != null))
                                {
                                    drQcValue.ValueSeq = count;
                                    double QcActualValue = 0;
                                    bool isErroData = false;
                                    if (drQcValue.ItmConvertValue == null)
                                    {
                                        isErroData = Double.TryParse(drQcValue.QresValue.ToString(), out QcActualValue);
                                        if (isErroData)
                                            drQcValue.FinalValue = QcActualValue;
                                    }
                                    else
                                    {
                                        isErroData = Double.TryParse(drQcValue.ItmConvertValue.ToString(), out QcActualValue);
                                        if (isErroData)
                                            drQcValue.FinalValue = QcActualValue;
                                    }


                                    if (!isErroData)
                                    {
                                        drQcValue.ValueSeq = -1;
                                        drQcValue.QresRunawayFlag = "2";
                                        drQcValue.QresRunawayRule = "ErroData";
                                        continue;
                                    }


                                    //string strFilter = string.Format("qc_count>0 and qc_count<={0}", count);
                                    //Decimal avgValue = Convert.ToDecimal(dtQcValue.Compute("avg(qcr_actual_value)", strFilter));

                                    List<EntityObrQcResult> listMath = listValue.FindAll(s => s.ValueSeq > 0 && s.ValueSeq <= count);

                                    double avgValue = listMath.Average(w => w.FinalValue);

                                    double sumValue = listMath.Sum(w => Math.Pow(w.FinalValue - avgValue, 2));

                                    double sdValue = Math.Sqrt(sumValue / listMath.Count());

                                    double num4 = Convert.ToDouble(drQcValue.MatItmCcv);
                                    double MaxAllowValue = avgValue + (num4 * avgValue);
                                    double MinAllowValue = avgValue - (num4 * avgValue);
                                    drQcValue.NSD = (sdValue == 0) ? 0 : ((QcActualValue - avgValue) / sdValue);
                                    drQcValue.MatItmX = avgValue;
                                    drQcValue.MatItmSd = sdValue;


                                    if (QcActualValue > MaxAllowValue)
                                    {
                                        drQcValue.ValueSeq = -1;
                                        drQcValue.QresRunawayFlag = "2";
                                        drQcValue.QresRunawayRule = "MaxAllow↑";
                                    }
                                    else if (QcActualValue < MinAllowValue)
                                    {
                                        drQcValue.ValueSeq = -1;
                                        drQcValue.QresRunawayFlag = "2";
                                        drQcValue.QresRunawayRule = "MinAllow↓";
                                    }
                                    else
                                    {
                                        drQcValue.ValueSeq = count;
                                        drQcValue.QresRunawayFlag = "0";

                                    }
                                }
                                else
                                {
                                    drQcValue.ValueSeq = -1;
                                    drQcValue.QresRunawayFlag = "2";
                                    drQcValue.QresRunawayRule = "字典未设定CCV";
                                }

                                count++;
                            }

                        }
                        else
                        {
                            string strQcinfo = string.Empty;
                            int i = 1;
                            foreach (EntityObrQcResult drQcValue in listValue)
                            {
                                drQcValue.ValueSeq = count;
                                count++;
                                //double dbC_x = (drQcValue.QresAuditFlag.ToString() == "0" || drQcValue["qcm_type"].ToString() == "1") ? Convert.ToDouble(drQcValue.MatItmX == null ? 0 : drQcValue.MatItmX) : Convert.ToDouble(drQcValue.QresItmX);
                                //double dbC_sd = (drQcValue.QresAuditFlag.ToString() == "0" || drQcValue["qcm_type"].ToString() == "1") ? Convert.ToDouble(drQcValue.MatItmSd == null ? 1 : drQcValue.MatItmSd) : Convert.ToDouble(drQcValue.QresItmSd);
                                double dbC_x = (drQcValue.QresAuditFlag.ToString() == "0") ? Convert.ToDouble(drQcValue.MatItmX == null ? 0 : drQcValue.MatItmX) : Convert.ToDouble(drQcValue.QresItmX == null ? 0 : drQcValue.QresItmX);
                                double dbC_sd = (drQcValue.QresAuditFlag.ToString() == "0") ? Convert.ToDouble(drQcValue.MatItmSd == null ? 1 : drQcValue.MatItmSd) : Convert.ToDouble(drQcValue.QresItmSd == null ? 1 : drQcValue.QresItmSd);

                                if (strQcinfo != string.Empty && strQcinfo != string.Format(i + "靶值:{0} SD:{1}", dbC_x, dbC_sd))
                                    i++;

                                strQcinfo = string.Format(i + "靶值:{0} SD:{1}", dbC_x, dbC_sd);
                                drQcValue.GroupName = strQcinfo;

                                if (string.IsNullOrEmpty(drQcValue.ItmConvertValue))
                                {
                                    try
                                    {
                                        double value = Convert.ToDouble(drQcValue.QresValue);
                                        drQcValue.FinalValue = value;
                                        drQcValue.NSD = dbC_sd == 0 ? 0 : (value - dbC_x) / dbC_sd;
                                    }
                                    catch (Exception)
                                    {
                                        drQcValue.ValueSeq = -1;
                                        drQcValue.QresRunawayFlag = "2";
                                        drQcValue.QresRunawayRule = "ErrorValue";
                                    }
                                }
                                else
                                {
                                    drQcValue.NSD = (dbC_sd == 0 ? 0 : (Convert.ToDouble(drQcValue.ItmConvertValue) - dbC_x) / dbC_sd);
                                    int qcm_flag = 0;
                                    if (!string.IsNullOrEmpty(drQcValue.QresConvertValue)
                                        && int.TryParse(drQcValue.QresAuditFlag.ToString(), out qcm_flag)
                                        && qcm_flag > 0)
                                    {
                                        drQcValue.FinalValue = Convert.ToDouble(drQcValue.QresConvertValue);
                                    }
                                    else
                                        drQcValue.FinalValue = Convert.ToDouble(drQcValue.ItmConvertValue);
                                }
                                if (drQcValue.QresAuditFlag.ToString() == "1" &&
                                    string.IsNullOrEmpty(drQcValue.QresSecondauditUserId))
                                {
                                    if (drQcValue.QresSecondauditDate != null && drQcValue.QresSecondauditDate != null && drQcValue.QresSecondauditDate.ToString() != string.Empty)
                                    {
                                        drQcValue.QresSecondauditInterval = Convert.ToInt32(DateTime.Now.Subtract(Convert.ToDateTime(drQcValue.QresSecondauditDate)).TotalHours);
                                        if (!string.IsNullOrEmpty(nextAuditHour))
                                        {
                                            drQcValue.NextAuditTime = Convert.ToDateTime(drQcValue.QresSecondauditDate).AddHours(Convert.ToDouble(nextAuditHour));
                                        }
                                        else
                                            drQcValue.NextAuditTime = Convert.ToDateTime(drQcValue.QresSecondauditDate).AddHours(24);
                                    }
                                    else
                                        drQcValue.QresSecondauditInterval = 0;
                                }
                            }
                        }


                        listResult.AddRange(listValue);
                    }
                }

            }

            return listResult;
        }

        /// <summary>
        /// 查询半定量字典
        /// </summary>
        /// <param name="strItrId"></param>
        /// <param name="strItmId"></param>
        /// <returns></returns>
        public List<EntityDicQcConvert> GetQcConvert(String strItrId, String strItmId)
        {
            List<EntityDicQcConvert> listView = new List<EntityDicQcConvert>();

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                listView = dao.GetQcConvert(strItrId, strItmId);
            }

            return listView;
        }


        public bool QcResultUndoAudit(List<String> listQresSn)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                result = dao.QcResultUndoAudit(listQresSn);
            }

            return result;

        }

        public bool QcResultAudit(List<EntityObrQcResult> listQcResult, string operatorID)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                foreach (EntityObrQcResult item in listQcResult)
                {
                    item.QresAuditUserId = operatorID;
                    item.QresAuditDate = now;
                    item.QresAuditFlag = 1;
                }

                result = dao.QcResultAudit(listQcResult);
            }

            return result;
        }

        /// <summary>
        /// 二次审核
        /// </summary>
        /// <param name="listQcResult"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        public bool QcResultSecondAudit(List<EntityObrQcResult> listQcResult, string operatorID)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                foreach (EntityObrQcResult item in listQcResult)
                {
                    if (item.QresSecondauditDate != null)
                    {
                        item.QresSecondauditInterval = Convert.ToInt32(DateTime.Now.Subtract(Convert.ToDateTime(item.QresSecondauditDate)).TotalHours);
                    }
                    else
                        item.QresSecondauditInterval = 0;

                    item.QresSecondauditUserId = operatorID;
                    item.QresSecondauditDate = now;
                }

                result = dao.QcResultSecondAudit(listQcResult);
            }

            return result;
        }


        public Boolean UpdateQresDisplay(string strQresSn, int display)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                result = dao.UpdateQresDisplay(strQresSn, display);
            }
            return result;
        }

        /// <summary>
        /// 基础查询，不关联字典等
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        public List<EntityObrQcResult> QcResultQuery(EntityObrQcResultQC resultQc)
        {
            List<EntityObrQcResult> listQcResult = new List<EntityObrQcResult>();

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                listQcResult = dao.QcResultQuery(resultQc);
            }

            return listQcResult;
        }

        public bool SaveQcResult(EntityObrQcResult qcResult)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                result = dao.SaveQcResult(qcResult);
            }

            return result;
        }

        public bool DeleteQcResult(List<string> listQresSn)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                result = dao.DeleteQcResult(listQresSn);
            }

            return result;
        }

        public bool UpdateQcResult(EntityObrQcResult qcResult)
        {
            bool result = false;

            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                result = dao.UpdateQcResult(qcResult);
            }

            return result;
        }

        private Dictionary<int, SIValue> GetListSIValue()
        {
            Dictionary<int, SIValue> siValue = new Dictionary<int, SIValue>();
            siValue.Add(3, GetSIValue(1.15, 1.15));
            siValue.Add(4, GetSIValue(1.46, 1.49));
            siValue.Add(5, GetSIValue(1.67, 1.75));
            siValue.Add(6, GetSIValue(1.82, 1.94));
            siValue.Add(7, GetSIValue(1.94, 2.10));
            siValue.Add(8, GetSIValue(2.03, 2.22));
            siValue.Add(9, GetSIValue(2.11, 2.32));
            siValue.Add(10, GetSIValue(2.18, 2.41));
            siValue.Add(11, GetSIValue(2.23, 2.48));
            siValue.Add(12, GetSIValue(2.29, 2.55));
            siValue.Add(13, GetSIValue(2.33, 2.61));
            siValue.Add(14, GetSIValue(2.37, 2.66));
            siValue.Add(15, GetSIValue(2.41, 2.70));
            siValue.Add(16, GetSIValue(2.44, 2.75));
            siValue.Add(17, GetSIValue(2.47, 2.79));
            siValue.Add(18, GetSIValue(2.50, 2.82));
            siValue.Add(19, GetSIValue(2.53, 2.85));
            siValue.Add(20, GetSIValue(2.56, 2.88));
            return siValue;
        }

        private SIValue GetSIValue(double n2s, double n3s)
        {
            SIValue value = new SIValue();
            value.n2s = n2s;
            value.n3s = n3s;
            return value;
        }

        public List<EntityQcStatistic> GetAnalyseData(List<EntityObrQcResultQC> lisItem)
        {
            List<EntityQcStatistic> listQcSatistic = new List<EntityQcStatistic>();
            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao != null)
            {
                listQcSatistic = dao.GetAnalyseData(lisItem);
            }
            return listQcSatistic;
        }

        public DataSet doNew(DataSet ds)
        {
            DataSet result = new DataSet();
            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao == null)
            {
                throw new Exception("未找到Dao导出！");
            }
            bool IsSeq = CacheSysConfig.Current.GetSystemConfig("QC_Item_Seq") == "是";
            result = dao.doNew(ds, IsSeq);
            return result;
            
        }

        
        public DataTable QcReagentsCompare(DataTable QcItem, DataTable QcCompareData)
        {
            IDaoQcResult dao = DclDaoFactory.DaoHandler<IDaoQcResult>();
            if (dao == null)
            {
                throw new Exception("未找到Dao导出！");
            }
            else
            {
                DataTable result = dao.QcReagentsCompare(QcItem, QcCompareData);
                return result;
            }
        }
    }

    [Serializable]
    public class SIValue
    {
        public SIValue()
        { }

        Double _n2s;
        Double _n3s;

        public Double n2s
        {
            get { return _n2s; }
            set { _n2s = value; }
        }

        public Double n3s
        {
            get { return _n3s; }
            set { _n3s = value; }
        }
    }
}
