using System;
using System.Collections.Generic;
using System.Linq;
using dcl.svr.cache;
using System.Data;
using System.Collections;
using dcl.common;
using dcl.root.logon;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 漏项/必录项检查
    /// </summary>
    public class CheckerLostItem : AbstractAuditClass, IAuditChecker
    {
        public CheckerLostItem(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report || this.auditType == EnumOperationCode.PreAudit)
            {
                var query = from pat_com in patients_mi
                            join com_mi in DictCombineMiCache2.Current.DclCache on pat_com.ComId equals com_mi.ComId
                            join dict_item in DictItemCache.Current.DclCache on com_mi.ComItmId equals dict_item.ItmId
                            where dict_item.ItmDelFlag != "1" && com_mi.ComMustItem != null && com_mi.ComMustItem == "1"
                            select dict_item;

                int count = 0;
                foreach (var item in query)
                {
                    //***********************************************************************************
                    //增加判断是否结果值为空格
                    if (!this.resulto.Any(i => i.ItmId == item.ItmId && !string.IsNullOrEmpty(i.ObrValue) && !string.IsNullOrEmpty(i.ObrValue.Trim())))
                    {
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_CalculateCalItem") == "是")
                        {
                            List<EntityDicItmCalu> dtCalItem = dcl.svr.cache.DictClItemCache.Current.GetAllCalu();
                            List<EntityDicItmCalu> drCalItemList = dtCalItem.Where(w => w.ItmId == item.ItmId).ToList();
                            if (drCalItemList.Count > 0)
                            {
                                count++;
                                continue;
                            }
                        }
                        chkResult.AddMessage(EnumOperationErrorCode.ItemNotNullLost, item.ItmEcode, EnumOperationErrorLevel.Warn);
                        count++;
                    }
                    //***********************************************************************************
                }

                if (count > 0 && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_CalculateCalItem") == "是")
                {
                    try
                    {
                        GenerateAutoCalItem(pat_info, dcl.svr.cache.DictSampleCache.Current.GetSamCNameByID(pat_info.PidSamId), pat_info.SampRemark);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, "计算项目没有结果的标本进行自动计算息出错,patID=" + pat_info.RepId, ex.ToString());
                    }
                }

            }
        }


        public void GenerateAutoCalItem(EntityPidReportMain entityPatient, string sampName, string sampRem)
        {
            //系统配置：关闭自动关联计算项目的功能
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_stopCalItem") == "是")
            {
                return;//不自动关联计算项目
            }

            List<EntityObrResult> listPatientResulto = new List<EntityObrResult>();
            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ListObrId.Add(entityPatient.RepId);
            //查找当前病人结果
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                listPatientResulto = resultDao.ObrResultQuery(resultQc);
            }


            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (EntityObrResult drSource in listPatientResulto)
            {
                if (!string.IsNullOrEmpty(drSource.ObrValue) && drSource.ObrValue.Trim(null) != string.Empty)
                {
                    string item_ecd = drSource.ItmEname;

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource.ObrValue);
                    }
                }
            }



            List<EntityDicItmCalu> dtCalItem = dcl.svr.cache.DictClItemCache.Current.GetAllCalu();

            DataSet dsResult = Variable(ht, dtCalItem, sampName, sampRem, entityPatient);
            DateTime now = ServerDateTime.GetDatabaseServerDateTime();
            DataTable dtCalResult = dsResult.Tables[0];

            if (dtCalResult.Rows.Count > 0)
            {
                List<EntityDicCombineDetail> listComItem = new List<EntityDicCombineDetail>();
                IDaoDicCombineDetail daoDetail = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                if (daoDetail != null)
                {
                    listComItem = daoDetail.GetDictCombineItem(entityPatient.RepId);
                }
                List<EntityObrResult> listInsert = EntityManager<EntityObrResult>.ListClone(listPatientResulto);
                List<EntityObrResult> listUpdate = EntityManager<EntityObrResult>.ListClone(listPatientResulto);
                foreach (DataRow drResult in dtCalResult.Rows)
                {

                    string itm_id = drResult["cal_item_ecd"].ToString();
                    string itm_ecd = drResult["itm_ecd"].ToString();
                    string value = drResult["retu"].ToString();
                    string valueINItmProp = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        decimal dec = 0;

                        if (decimal.TryParse(value, out dec))
                        {
                            dec = decimal.Round(dec, 2);

                            valueINItmProp = dec.ToString();
                        }
                    }
                    string strProp = dcl.svr.cache.DictItemPropCache.Current.GetItmProp(itm_id, valueINItmProp);

                    value = strProp == string.Empty ? value : strProp;

                    List<EntityObrResult> existItems = listPatientResulto.Where(w => w.ItmId == itm_id).ToList();

                    List<EntityDicCombineDetail> listComItems = listComItem.Where(w => w.ComItmId == itm_id).ToList();

                    if (listComItems.Count > 0 || listComItem.Count == 0)
                    {
                        if (existItems.Count == 0 && listInsert.Where(w => w.ItmId == itm_id).Count() == 0) //项目不存在：添加
                        {
                            EntityObrResult drInsert = new EntityObrResult(); ;

                            drInsert.ObrId = entityPatient.RepId;
                            drInsert.ItmId = itm_id;
                            drInsert.ItmEname = itm_ecd;
                            drInsert.ObrFlag = 1;
                            drInsert.ObrItrId = entityPatient.RepItrId;
                            drInsert.ObrSid = entityPatient.RepSid;
                            //drInsert["res_sam_id"] = pat_sam_id;d
                            drInsert.ObrDate = now;
                            drInsert.ObrValue = value;
                            drInsert.ObrType = 2;
                            drInsert.ItmReportCode = itm_ecd;

                            listInsert.Add(drInsert);
                        }
                        else//存在：更新结果
                        {

                            //如果结果为空才进行关联计算（只计算一次）
                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AllowEditCalItem") == "是" && !string.IsNullOrEmpty(existItems[0].ObrValue.ToString().Trim()))
                                continue;
                            string obr_convert_value = string.Empty;
                            if (Compare.IsEmpty(value))
                            {
                                obr_convert_value = string.Empty;
                            }
                            else
                            {
                                decimal decValue = 0;
                                if (decimal.TryParse(value, out decValue))
                                    obr_convert_value = value;
                                else
                                    obr_convert_value = string.Empty;
                            }
                            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                            if (resultDao != null)
                            {
                                dao.UpdateResultVauleByObrSn(value, obr_convert_value, existItems[0].ObrSn.ToString());
                            }

                        }
                    }
                }
                //更新结果
                IDaoObrResult obrDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                if (obrDao != null)
                {
                    foreach (EntityObrResult result in listInsert)
                    {
                        try
                        {
                            obrDao.UpdateObrResult(result);
                        }
                        catch (Exception ex)
                        {
                            Lib.LogManager.Logger.LogException(ex);
                        }

                    }
                }
            }
        }


        //公用效验方法
        private DataSet Variable(Hashtable ht, List<EntityDicItmCalu> dtCalItem, string sampName, string sampRem, EntityPidReportMain entityPatient)
        {
            string Pat_itr_id = entityPatient.RepId;
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("cal_sp_formula");
            pb.Columns.Add("retu");

            List<string> fmla = new List<string>();
            foreach (EntityDicItmCalu dr in dtCalItem)
            {
                string cal_item_ecd = string.Empty;
                if (!string.IsNullOrEmpty(dr.ItmId))
                {
                    cal_item_ecd = dr.ItmId;
                }
                if (!string.IsNullOrEmpty(dr.CalItrId) && !string.IsNullOrEmpty(Pat_itr_id)
                  && dr.CalItrId != Pat_itr_id)
                {
                    continue;
                }
                if (dr.CalExpression != null && !string.IsNullOrEmpty(dr.CalExpression) &&
                   fmla.Contains(dr.CalExpression + cal_item_ecd))
                {
                    continue;
                }
                if (dr.CalExpression != null && !string.IsNullOrEmpty(dr.CalExpression))
                {
                    fmla.Add(dr.CalExpression + cal_item_ecd);
                }
                if (dr.CalVariable != "")
                {
                    string[] varpr = dr.CalVariable.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr.CalExpression, dr.CalFlag, dr.ItmId, dr.ItmEcode, dr.CalSpFormula);

                    }
                }
            }

            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();
                string itmID = pb.Rows[i]["cal_item_ecd"].ToString();
                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";

                    double dValue = 0;
                    double.TryParse(value[j], out dValue);

                    string va = dValue.ToString("0.0000");

                    methAll = methAll.Replace(fam, va);
                }

                DataTable dt = new DataTable();
                try
                {
                    object objValue = dt.Compute(methAll, string.Empty);

                    decimal decVal = 0;

                    if (decimal.TryParse(objValue.ToString(), out decVal))
                    {
                        int? itm_max_digit = null;
                        EntityDicItemSample itemSam = dcl.svr.cache.DictItemSamCache.Current.DclCache.Find(k => k.ItmId == itmID && k.ItmSamId == sampName);
                        if (itemSam != null)
                        {
                            itm_max_digit = itemSam.ItmMaxDigit;
                        }
                        if (itm_max_digit == null || itm_max_digit < 0)
                        {
                            decVal = decimal.Round(decVal, 4);
                            pb.Rows[i]["retu"] = decVal.ToString("0.00");
                        }
                        else
                        {
                            decVal = decimal.Round(decVal, itm_max_digit.Value);

                            if (itm_max_digit == 0)
                            {
                                pb.Rows[i]["retu"] = decVal.ToString();
                            }
                            else
                            {

                                pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                            }


                        }
                    }
                }
                catch
                {
                    //当使用DataTable.Compute无法计算表达式的值时,比如带Math.Log()的表达式
                    //用动态编译后进行计算 
                    try
                    {
                        //2013年5月14日14:20:41 叶
                        if (methAll.Contains("[标本]"))
                        {

                            methAll = methAll.Replace("[标本]", string.Format("\"{0}\"", sampName));

                        }
                        if (methAll.Contains("[标本备注]"))
                        {
                            methAll = methAll.Replace("[标本备注]", string.Format("\"{0}\"", sampRem));

                        }
                        object objValue = ExpressionCompute.CalExpression(methAll);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {
                                int? itm_max_digit = null;
                                EntityDicItemSample itemSam = dcl.svr.cache.DictItemSamCache.Current.DclCache.Find(k => k.ItmId == itmID && k.ItmSamId == sampName);
                                if (itemSam != null)
                                {
                                    itm_max_digit = itemSam.ItmMaxDigit;
                                }
                                if (itm_max_digit == null || itm_max_digit < 0)
                                {
                                    decVal = decimal.Round(decVal, 4);
                                    pb.Rows[i]["retu"] = decVal.ToString("0.00");
                                }
                                else
                                {
                                    decVal = decimal.Round(decVal, itm_max_digit.Value);
                                    if (itm_max_digit == 0)
                                    {
                                        pb.Rows[i]["retu"] = decVal.ToString();
                                    }
                                    else
                                    {

                                        pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                    }

                                }
                            }
                        }
                        else
                        {


                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }


        #endregion
    }
}
