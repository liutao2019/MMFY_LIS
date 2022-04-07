using System;
using System.Collections.Generic;
using System.Data;
using dcl.common;
using Lib.DAC;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.servececontract;
using System.Configuration;
using System.Linq;
using dcl.svr.cache;

namespace dcl.svr.dicbasic
{
    public class ItemBIZ : IDicCommon
    {

        private void LogDbLOG(Dictionary<string, object> dict)
        {
            EntitySysOperationLog dtOperation = (EntitySysOperationLog)dict["Operation"];
            //DataTable _LOGINFO = dsDataPack.Tables["_LOGINFO"];

            EntityDicItmItem dtNewDictItem = (EntityDicItmItem)dict["Item"];
            List<EntityDicItemSample> dtNewItemSam = dict["Sam"] as List<EntityDicItemSample>;
            List<EntityDicItmRefdetail> dtNewItemMi = dict["Detail"] as List<EntityDicItmRefdetail>;
            string ipAdress = (string)dict["IP"];

            EntityResponse response = new EntityResponse();
            string old_itm_id = dtNewDictItem.ItmId;
            string new_itm_name = dtNewDictItem.ItmName;
            string new_itm_ecd = dtNewDictItem.ItmEcode;

            string operationName = dtOperation.OperatAction;
            string loginID = dtOperation.OperatUserId;
            string userName = dtOperation.OperatUserName;

            if (operationName == "删除"
                || operationName == "新增"
                )
            {
                dtOperation.OperatServername = ipAdress;
                dtOperation.OperatKey = old_itm_id;
                dtOperation.OperatDate = ServerDateTime.GetDatabaseServerDateTime();
                dtOperation.OperatModule = "项目字典";
                dtOperation.OperatGroup = "项目基本信息";
                dtOperation.OperatObject = string.Format("{0}({1})", new_itm_ecd, old_itm_id);
                dtOperation.OperatContent = new_itm_name;
                IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.SaveSysOperationLog(dtOperation);
                    response.SetResult(dtOperation);
                }
            }
            else if (operationName == "修改")
            {
                DateTime dtNow =ServerDateTime.GetDatabaseServerDateTime();

                SqlHelper helper = new SqlHelper();

                #region 比较项目信息
                //比较项目信息
                List<EntityDicItmItem> dtOldDictItem = new List<EntityDicItmItem>();
                IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(dtNewDictItem.ItmId, ""));
                    dtOldDictItem = response.GetResult() as List<EntityDicItmItem>;
                }

                if (dtOldDictItem.Count > 0
                    && dtNewDictItem != null)
                {
                    EntityDicItmItem rowItemOld = dtOldDictItem[0];
                    EntityDicItmItem rowItemNew = dtNewDictItem;

                    string msgItem = string.Empty;
                    if (rowItemOld.ItmEcode != rowItemNew.ItmEcode)
                    {
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目基本信息", "修改", "项目代码(itm_ecd)", rowItemOld.ItmEcode, rowItemNew.ItmEcode);
                    }

                    if (rowItemOld.ItmName != rowItemNew.ItmName)
                    {
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目基本信息", "修改", "项目名称(itm_name)", rowItemOld.ItmName, rowItemNew.ItmName);
                    }

                    if (rowItemOld.ItmRepCode != rowItemNew.ItmRepCode)
                    {
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目基本信息", "修改", "项目报告代码(itm_rep_ecd)", rowItemOld.ItmRepCode, rowItemNew.ItmRepCode);
                    }

                    if (rowItemOld.ItmPriId != rowItemNew.ItmPriId)
                    {
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目基本信息", "修改", "专业组ID(itm_ptype)", rowItemOld.ItmPriId, rowItemNew.ItmPriId);
                    }
                    if (rowItemOld.ItmCaluFlag != rowItemNew.ItmCaluFlag)
                    {
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目基本信息计算项目", "修改", "计算项目(itm_cal_flag)", rowItemOld.ItmCaluFlag, rowItemNew.ItmCaluFlag);
                    }
                }
                #endregion

                #region 比较项目标本信息
                //比较项目标本信息 
                List<EntityDicItemSample> dtOldDictItemSam = new List<EntityDicItemSample>();
                IDaoDic<EntityDicItemSample> daoSam = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItemSample>>();
                if (daoSam == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(daoSam.Search(old_itm_id));
                    dtOldDictItemSam = response.GetResult() as List<EntityDicItemSample>;
                }


                //dtItemSam

                //遍历旧数据
                foreach (EntityDicItemSample rowOldSam in dtOldDictItemSam)
                {
                    string old_sam_id = rowOldSam.ItmSamId;
                    string old_sam_name = rowOldSam.ItmSamName;

                    //在新数据里面查找是否有旧数据信息
                    List<EntityDicItemSample> rowsNewItemSam = dtNewItemSam.Where(w => w.ItmSamId.Contains(old_sam_id)).ToList();

                    if (rowsNewItemSam.Count == 0)//没有，则被删除
                    {
                        string samLog = string.Format("价格：{0} ，成本：{1} ，实验方法：{2} ，默认值：{3}"
                            , rowOldSam.ItmPrice
                            , rowOldSam.ItmCost
                            , rowOldSam.ItmMeaning
                            , rowOldSam.ItmDefault
                            );
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目标本信息", "删除", "标本：" + old_sam_name, samLog, null);
                    }
                    else//找到，则匹配改动的地方
                    {
                        EntityDicItemSample rowNewItemSam = rowsNewItemSam[0];

                        string samModifyMsg = string.Empty;

                        if (rowOldSam.ItmPrice.ToString() != rowNewItemSam.ItmPrice.ToString())
                        {
                            samModifyMsg += string.Format("价格：{0} -> {1} ，", rowOldSam.ItmPrice, rowNewItemSam.ItmPrice);
                        }

                        if (rowOldSam.ItmCost.ToString() != rowNewItemSam.ItmCost.ToString())
                        {
                            samModifyMsg += string.Format("成本：{0} -> {1} ，", rowOldSam.ItmCost, rowNewItemSam.ItmCost);
                        }

                        if (rowOldSam.ItmMeaning != rowNewItemSam.ItmMeaning)
                        {
                            samModifyMsg += string.Format("实验方法：{0} -> {1} ，", rowOldSam.ItmMeaning, rowNewItemSam.ItmMeaning);
                        }

                        if (rowOldSam.ItmDefault != rowNewItemSam.ItmDefault)
                        {
                            samModifyMsg += string.Format("默认值：{0} -> {1} ，", rowOldSam.ItmDefault, rowNewItemSam.ItmDefault);
                        }

                        if (samModifyMsg != string.Empty)
                        {
                            LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目标本信息", "删除", "标本：" + old_sam_name, samModifyMsg, null);
                        }
                    }
                }

                //遍历新数据
                foreach (EntityDicItemSample rowNewSam in dtNewItemSam)
                {
                    string new_sam_id = rowNewSam.ItmSamId.ToString();

                    //在旧数据里面查找是否有新数据信息
                    List<EntityDicItemSample> rowsOldItemSam = dtOldDictItemSam.Where(w => w.ItmSamId.Contains(new_sam_id)).ToList(); ;

                    if (rowsOldItemSam.Count == 0)//没有，则新增
                    {
                        string new_sam_name = dcl.svr.cache.DictSampleCache.Current.GetSamCNameByID(new_sam_id);
                        string samLog = string.Format("价格：{0} ，成本：{1} ，实验方法：{2} ，默认值：{3}"
                            , rowNewSam.ItmPrice
                            , rowNewSam.ItmCost
                            , rowNewSam.ItmCost
                            , rowNewSam.ItmDefault
                            );
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目标本信息", "新增", "标本：" + new_sam_name, samLog, null);
                    }
                }
                #endregion

                #region 比较参考值信息
                List<EntityDicItmRefdetail> dtOldDictItemMi = new List<EntityDicItmRefdetail>();
                IDaoDic<EntityDicItmRefdetail> daoMi = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmRefdetail>>();
                if (daoMi == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(daoMi.Search(old_itm_id));
                    dtOldDictItemMi = response.GetResult() as List<EntityDicItmRefdetail>;
                }

                //遍历旧数据
                foreach (EntityDicItmRefdetail rowOldMi in dtOldDictItemMi)
                {
                    string old_itm_mi_id = rowOldMi.ItmDetId.ToString();

                    //在新数据里面查找是否有旧数据信息
                    List<EntityDicItmRefdetail> rowsNewItemMi = dtNewItemMi.Where(w => w.ItmDetId.ToString().Contains(old_itm_mi_id)).ToList();

                    if (rowsNewItemMi.Count == 0)//没有，则被删除
                    {
                        string samLog = string.Format("参考值名称：{0} ，性别：{1} ，年龄下限：{2} ，年龄上限：{3} ，参考值下限：{4}，参考值上限：{5}，危急值下限：{6}，危急值上限：{7}，阈值下限：{8}，阈值上限：{9}"
                            , rowOldMi.ItmRefName
                            , rowOldMi.ItmSex
                            , rowOldMi.ItmAgeLowerLimit
                            , rowOldMi.ItmAgeUpperLimit
                            , rowOldMi.ItmLowerLimitValue
                            , rowOldMi.ItmUpperLimitValue
                            , rowOldMi.ItmDangerLowerLimit
                            , rowOldMi.ItmDangerUpperLimit
                            , rowOldMi.ItmMinValue
                            , rowOldMi.ItmMinValue
                            );
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目参考值信息", "删除", "标本：" + rowOldMi.ItmSamId, samLog, null);
                    }
                    else
                    {
                        EntityDicItmRefdetail rowNewItemMi = rowsNewItemMi[0];

                        string miModifyMsg = string.Empty;

                        if (rowOldMi.ItmRefName != rowNewItemMi.ItmRefName)
                        {
                            miModifyMsg += string.Format("参考值名称：{0}  ->  {1} ，", rowOldMi.ItmRefName, rowNewItemMi.ItmRefName);
                        }

                        if (rowOldMi.ItmSex != rowNewItemMi.ItmSex)
                        {
                            miModifyMsg += string.Format("性别：{0}  ->  {1} ，", rowOldMi.ItmSex, rowNewItemMi.ItmSex);
                        }

                        if (rowOldMi.ItmAgeLowerLimit.ToString() != rowNewItemMi.ItmAgeLowerLimit.ToString())
                        {
                            miModifyMsg += string.Format("年龄下限：{0}  ->  {1} ，", rowOldMi.ItmAgeLowerLimit, rowNewItemMi.ItmAgeLowerLimit);
                        }

                        if (rowOldMi.ItmAgeUpperLimit != rowNewItemMi.ItmAgeUpperLimit)
                        {
                            miModifyMsg += string.Format("年龄上限：{0}  ->  {1} ，", rowOldMi.ItmAgeUpperLimit, rowNewItemMi.ItmAgeUpperLimit);
                        }

                        if (rowOldMi.ItmLowerLimitValue != rowNewItemMi.ItmLowerLimitValue)
                        {
                            miModifyMsg += string.Format("参考值下限：{0}  ->  {1} ，", rowOldMi.ItmLowerLimitValue, rowNewItemMi.ItmLowerLimitValue);
                        }

                        if (rowOldMi.ItmUpperLimitValue != rowNewItemMi.ItmUpperLimitValue)
                        {
                            miModifyMsg += string.Format("参考值上限：{0}  ->  {1} ，", rowOldMi.ItmUpperLimitValue, rowNewItemMi.ItmUpperLimitValue);
                        }

                        if (rowOldMi.ItmDangerLowerLimit != rowNewItemMi.ItmDangerLowerLimit)
                        {
                            miModifyMsg += string.Format("危急值下限：{0}  ->  {1} ，", rowOldMi.ItmDangerLowerLimit, rowNewItemMi.ItmDangerLowerLimit);
                        }

                        if (rowOldMi.ItmDangerUpperLimit != rowNewItemMi.ItmDangerUpperLimit)
                        {
                            miModifyMsg += string.Format("危急值上限：{0}  ->  {1} ，", rowOldMi.ItmDangerUpperLimit, rowNewItemMi.ItmDangerUpperLimit);
                        }

                        if (rowOldMi.ItmMinValue != rowNewItemMi.ItmMinValue)
                        {
                            miModifyMsg += string.Format("阈值下限：{0}  ->  {1} ，", rowOldMi.ItmMinValue, rowNewItemMi.ItmMinValue);
                        }

                        if (rowOldMi.ItmMaxValue != rowNewItemMi.ItmMaxValue)
                        {
                            miModifyMsg += string.Format("阈值上限：{0}  ->  {1} ，", rowOldMi.ItmMaxValue, rowNewItemMi.ItmMaxValue);
                        }

                        if (miModifyMsg != string.Empty)
                            LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目参考值信息", "修改", "标本：" + rowOldMi.ItmSamId, miModifyMsg, null);
                    }
                }

                //遍历新数据
                foreach (EntityDicItmRefdetail rowNewMi in dtNewItemMi)
                {
                    string itm_mi_id = rowNewMi.ItmDetId.ToString();

                    bool isAddNew = false;

                    List<EntityDicItmRefdetail> rowsOldItemMi;
                    if (string.IsNullOrEmpty(itm_mi_id))
                    {
                        isAddNew = true;
                    }
                    else
                    {
                        //在旧数据里面查找是否有新数据信息
                        rowsOldItemMi = dtOldDictItemMi.Where(w => w.ItmDetId.ToString().Contains(itm_mi_id)).ToList();
                        if (rowsOldItemMi.Count == 0)
                        {
                            isAddNew = true;
                        }
                    }

                    if (isAddNew)
                    //没有，则新增
                    {

                        string new_sam_name = dcl.svr.cache.DictSampleCache.Current.GetSamCNameByID(rowNewMi.ItmSamId.ToString());

                        string samLog = string.Format("参考值名称：{0} ，性别：{1} ，年龄下限：{2} ，年龄上限：{3} ，参考值下限：{4}，参考值上限：{5}，危急值下限：{6}，危急值上限：{7}，阈值下限：{8}，阈值上限：{9}"
                            , rowNewMi.ItmRefName
                            , rowNewMi.ItmSex
                            , rowNewMi.ItmAgeLowerLimit
                            , rowNewMi.ItmAgeUpperLimit
                            , rowNewMi.ItmLowerLimitValue
                            , rowNewMi.ItmUpperLimitValue
                            , rowNewMi.ItmDangerLowerLimit
                            , rowNewMi.ItmDangerUpperLimit
                            , rowNewMi.ItmMinValue
                            , rowNewMi.ItmMaxValue
                            );
                        LogDBField(loginID, ipAdress, dtNow, old_itm_id, "项目字典", "项目参考值信息", "新增", "标本：" + new_sam_name, samLog, null);
                    }
                }

                #endregion
            }
        }

        public void LogDBField(string loginID, string ipAdress, DateTime opDate, string itm_id, string module, string group, string operationName, string fieldName, object oldVal, object newVal)
        {
            EntitySysOperationLog dtOperation = new EntitySysOperationLog();
            EntityResponse response = new EntityResponse();
            dtOperation.OperatUserId = loginID;
            dtOperation.OperatServername = ipAdress;
            dtOperation.OperatKey = itm_id;
            dtOperation.OperatDate = ServerDateTime.GetDatabaseServerDateTime();
            dtOperation.OperatModule = module;
            dtOperation.OperatGroup = group;
            dtOperation.OperatAction = operationName;
            dtOperation.OperatObject = fieldName;

            if (newVal == null)
            {
                dtOperation.OperatContent = string.Format("{0}", oldVal);
            }
            else
            {
                dtOperation.OperatContent = string.Format("{0} -> {1}", oldVal, newVal);
            }
            IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = dao.SaveSysOperationLog(dtOperation);
                response.SetResult(dtOperation);
            }
        }

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicItmItem Itm = new EntityDicItmItem();
                List<EntityDicItemSample> ItmSamp = new List<EntityDicItemSample>();
                List<EntityDicItmRefdetail> ItmDetail = new List<EntityDicItmRefdetail>();
                string id = "";
                object objItm = dict["Item"];
                if (objItm != null)
                {
                    Itm = (EntityDicItmItem)objItm;

                }
                object objSam = dict["Sam"];
                if (objSam != null)
                {
                    ItmSamp = objSam as List<EntityDicItemSample>;

                }
                object objDetail = dict["Detail"];
                if (objDetail != null)
                {
                    ItmDetail = objDetail as List<EntityDicItmRefdetail>;

                }
                IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(Itm);
                    response.SetResult(Itm);
                    if (response.Scusess)
                    {
                        id = Itm.ItmId;
                        for (int i = 0; i < ItmSamp.Count; i++)
                        {
                            ItmSamp[i].ItmId = id.ToString();
                        }
                        for (int i = 0; i < ItmDetail.Count; i++)
                        {
                            ItmDetail[i].ItmId = id.ToString();
                        }
                    }
                    else
                    {
                        return response;
                    }
                }
                IDaoDic<EntityDicItemSample> daoSam = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItemSample>>();
                if (daoSam == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0;
                    foreach (EntityDicItemSample Samp in ItmSamp)
                    {
                        if (daoSam.Save(Samp))
                        {
                            count++;
                        }
                    }
                    if (count == ItmSamp.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(ItmSamp);
                }
                IDaoDic<EntityDicItmRefdetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmRefdetail>>();
                if (daoDetail == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0; ;
                    foreach (EntityDicItmRefdetail detail in ItmDetail)
                    {
                        if (daoDetail.Save(detail))
                        {
                            count++;
                        }
                    }
                    if (count == ItmSamp.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(ItmDetail);
                }
                LogDbLOG(dict);
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
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicItmItem Itm = new EntityDicItmItem();
                List<EntityDicItemSample> ItmSamp = new List<EntityDicItemSample>();
                List<EntityDicItmRefdetail> ItmDetail = new List<EntityDicItmRefdetail>();
                object objItm = dict["Item"];
                if (objItm != null)
                {
                    Itm = (EntityDicItmItem)objItm;
                }
                object objSam = dict["Sam"];
                if (objSam != null)
                {
                    ItmSamp = objSam as List<EntityDicItemSample>;
                }
                object objDetail = dict["Detail"];
                if (objDetail != null)
                {
                    ItmDetail = objDetail as List<EntityDicItmRefdetail>;
                }
                IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(Itm);
                    response.SetResult(Itm);
                }
                IDaoDic<EntityDicItemSample> daoSam = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItemSample>>();
                if (daoSam == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0; ;
                    foreach (EntityDicItemSample Samp in ItmSamp)
                    {
                        if (daoSam.Delete(Samp))
                        {
                            count++;
                        }
                    }
                    if (count == ItmSamp.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(ItmSamp);
                }
                IDaoDic<EntityDicItmRefdetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmRefdetail>>();
                if (daoDetail == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    int count = 0; ;
                    foreach (EntityDicItmRefdetail detail in ItmDetail)
                    {
                        if (daoDetail.Delete(detail))
                        {
                            count++;
                        }
                    }
                    if (count == ItmSamp.Count)
                    {
                        response.Scusess = true;
                    }
                    response.SetResult(ItmDetail);
                }
                dcl.svr.cache.DictCombineMiCache2.Current.Refresh();
                dcl.svr.cache.DictItemCache.Current.Refresh();
                dcl.svr.cache.DictItemMiCache.Current.Refresh();
                dcl.svr.cache.DictItemSamCache.Current.Refresh();

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload") == "HL7V3"
                    || dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload") == "HL7V3")
                {
                    new Lis.SendDataByHl7v3.DataSendHelper().GetXmlForItemInvoke(Itm.ItmId);
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
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicItmItem Itm = new EntityDicItmItem();
                List<EntityDicItemSample> ItmSamp = new List<EntityDicItemSample>();
                List<EntityDicItmRefdetail> ItmDetail = new List<EntityDicItmRefdetail>();
                object objItm = dict["Item"];
                if (objItm != null)
                {
                    Itm = (EntityDicItmItem)objItm;
                }
                object objSam = dict["Sam"];
                if (objSam != null)
                {
                    ItmSamp = objSam as List<EntityDicItemSample>;
                }
                object objDetail = dict["Detail"];
                if (objDetail != null)
                {
                    ItmDetail = objDetail as List<EntityDicItmRefdetail>;
                }
                LogDbLOG(dict);
                IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(Itm);
                    response.SetResult(Itm);
                }
                IDaoDic<EntityDicItemSample> daoSam = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItemSample>>();
                if (daoSam == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    //删除原有的标本信息
                    EntityDicItemSample sample = new EntityDicItemSample();
                    sample.ItmId = Itm.ItmId;
                    response.Scusess = daoSam.Delete(sample);
                    if (ItmSamp.Count > 0)
                    {
                        int count = 0;
                        foreach (EntityDicItemSample Samp in ItmSamp)
                        {
                            if (daoSam.Save(Samp))
                            {
                                count++;
                            }
                        }
                        if (count == ItmSamp.Count)
                        {
                            response.Scusess = true;
                        }
                        response.SetResult(ItmSamp);
                    }
                }
                IDaoDic<EntityDicItmRefdetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmRefdetail>>();
                if (daoDetail == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    EntityDicItmRefdetail refDetail = new EntityDicItmRefdetail();
                    refDetail.ItmId = Itm.ItmId;
                    response.Scusess = daoDetail.Delete(refDetail);
                    if (ItmDetail.Count > 0)
                    {
                        int count = 0;
                        foreach (EntityDicItmRefdetail detail in ItmDetail)
                        {
                            if (daoDetail.Save(detail))
                            {
                                count++;
                            }
                        }
                        if (count == ItmSamp.Count)
                        {
                            response.Scusess = true;
                        }
                        response.SetResult(ItmDetail);
                    }
                }
                dcl.svr.cache.DictCombineMiCache2.Current.Refresh();
                dcl.svr.cache.DictItemCache.Current.Refresh();
                dcl.svr.cache.DictItemMiCache.Current.Refresh();
                dcl.svr.cache.DictItemSamCache.Current.Refresh();
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirmWithDowload") == "HL7V3"
                 || dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirmWithDowload") == "HL7V3")
                {
                    new Lis.SendDataByHl7v3.DataSendHelper().GetXmlForItemInvoke(Itm.ItmId);
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
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search("", hosID));
            }
            return response;
        }

        public List<EntityDicItmItem> GetItemByItmId(string itmId)
        {
            List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
            IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (dao != null)
            {
                listItem = dao.Search(itmId, "");
            }
            return listItem;
        }

        public bool UpdateItem(EntityDicItmItem item)
        {
            EntityResponse response = new EntityResponse();
            IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                if (item != null)
                {
                    response.Scusess = true;
                    response.SetResult(dao.Update(item));
                }
            }
            return response.Scusess;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicItmItem type = request.GetRequestValue<EntityDicItmItem>();
            IDaoDic<EntityDicItemSample> daoSam = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItemSample>>();
            if (daoSam == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                if (type != null)
                {
                    response.Scusess = true;
                    response.SetResult(daoSam.Search(type.ItmId));
                }
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityDicItmItem type = request.GetRequestValue<EntityDicItmItem>();
            IDaoDic<EntityDicItmRefdetail> daodetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmRefdetail>>();
            if (daodetail == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                if (type != null)
                {
                    response.Scusess = true;
                    response.SetResult(daodetail.Search(type.ItmId));
                }
            }
            return response;
        }

        public List<EntityItmRefInfo> GetItemRefInfo(List<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itm_itr_id, string pat_depcode,string patDiag)
        {
            List<EntityItmRefInfo> listItmRefInfo = new List<EntityItmRefInfo>();
            IDaoDicItmItem itemDao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (itemDao != null)
            {
                listItmRefInfo = itemDao.GetItemRefInfo(itemsID, sam_id, age_minutes, sex, sam_rem, itm_itr_id, pat_depcode);

                foreach (EntityItmRefInfo info in listItmRefInfo)
                {
                    try
                    {
                        EntityDicUtgentValue utgentVal = DictItemUrgentValueCache.Current.GetDclValue(info.ItmId, sam_id, pat_depcode, sex, age_minutes, patDiag);
                        if (utgentVal != null)
                        {
                            info.ItmDangerUpperLimit = utgentVal.UgtPanH;
                            info.ItmDangerUpperLimitCal = utgentVal.UgtPanH;
                            info.ItmDangerLowerLimit = utgentVal.UgtPanL;
                            info.ItmDangerLowerLimitCal = utgentVal.UgtPanL;

                            info.ItmMaxValue = utgentVal.UgtMax;
                            info.ItmMaxValueCal = utgentVal.UgtMax;
                            info.ItmMinValue = utgentVal.UgtMin;
                            info.ItmMinValueCal = utgentVal.UgtMin;
                            info.ItmUrgentRes = utgentVal.UgtDesc;
                        }
                    }
                    catch
                    {

                    }
                    #region 格式化参考值
                    string str_itm_ref_l = string.Empty;
                    string str_itm_ref_h = string.Empty;

                    if (!string.IsNullOrEmpty(info.ItmLowerLimitValueCal) && info.ItmLowerLimitValueCal.Trim(null) != string.Empty)
                    {
                        str_itm_ref_l = info.ItmLowerLimitValueCal.Trim(null);

                        if (str_itm_ref_l.StartsWith(">") || str_itm_ref_l.StartsWith(">=") || str_itm_ref_h.StartsWith("≥"))
                        {
                            str_itm_ref_l = str_itm_ref_l.Replace(">=", "").Replace(">", "").Replace("≥", "");

                            info.ItmLowerLimitValueCal = str_itm_ref_l; // 2010-7-20
                                                                        //drRef[field_refh] = string.Empty;
                        }
                        else if (str_itm_ref_l.StartsWith("<") || str_itm_ref_l.StartsWith("<=") || str_itm_ref_l.StartsWith("≤"))
                        {
                            str_itm_ref_h = str_itm_ref_l.Replace("<=", "").Replace("<", "").Replace("≤", "");

                            info.ItmLowerLimitValueCal = string.Empty;// 2010-7-20
                            info.ItmUpperLimitValueCal = str_itm_ref_h;
                        }
                    }

                    if (!string.IsNullOrEmpty(info.ItmUpperLimitValueCal) && info.ItmUpperLimitValueCal.Trim(null) != string.Empty)
                    {
                        str_itm_ref_h = info.ItmUpperLimitValueCal.Trim(null);


                        if (str_itm_ref_h.StartsWith(">") || str_itm_ref_h.StartsWith(">=") || str_itm_ref_h.StartsWith("≥"))
                        {
                            str_itm_ref_l = str_itm_ref_l.Replace(">=", "").Replace(">", "").Replace("≥", "");
                            info.ItmLowerLimitValueCal = str_itm_ref_l; // 2010-7-20
                            info.ItmUpperLimitValueCal = string.Empty;
                        }
                        else if (str_itm_ref_h.StartsWith("<") || str_itm_ref_h.StartsWith("<=") || str_itm_ref_l.StartsWith("≤"))
                        {
                            str_itm_ref_h = str_itm_ref_h.Replace("<=", "").Replace("<", "").Replace("≤", "");
                            // drRef[field_refl] = string.Empty;// 2010-7-20
                            info.ItmUpperLimitValueCal = str_itm_ref_h;
                        }
                    }
                    #endregion

                    #region 格式化阈值
                    string str_itm_min = string.Empty;
                    string str_itm_max = string.Empty;

                    if (!string.IsNullOrEmpty(info.ItmMinValueCal))
                    {
                        if (info.ItmMinValueCal.Trim(null) != string.Empty)
                        {
                            str_itm_min = info.ItmMinValueCal.Trim(null);

                            if (str_itm_min.StartsWith(">") || str_itm_min.StartsWith(">=") || str_itm_max.StartsWith("≥"))
                            {
                                str_itm_min = str_itm_min.Replace(">=", "").Replace(">", "").Replace("≥", "");
                                info.ItmMinValueCal = str_itm_min;
                            }
                            else if (str_itm_min.StartsWith("<") || str_itm_min.StartsWith("<=") || str_itm_min.StartsWith("≤"))
                            {
                                str_itm_max = str_itm_min.Replace("<=", "").Replace("<", "").Replace("≤", "");
                                info.ItmMaxValueCal = str_itm_max;
                                info.ItmMinValueCal = string.Empty;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(info.ItmMaxValueCal))
                    {
                        if (info.ItmMaxValueCal.Trim(null) != string.Empty)
                        {
                            str_itm_max = info.ItmMaxValueCal.Trim(null);


                            if (str_itm_max.StartsWith(">") || str_itm_max.StartsWith(">=") || str_itm_max.StartsWith("≥"))
                            {
                                str_itm_min = str_itm_min.Replace(">=", "").Replace(">", "").Replace("≥", "");
                                info.ItmMinValueCal = str_itm_min;
                                info.ItmMaxValueCal = string.Empty;
                            }
                            else if (str_itm_max.StartsWith("<") || str_itm_max.StartsWith("<=") || str_itm_min.StartsWith("≤"))
                            {
                                str_itm_max = str_itm_max.Replace("<=", "").Replace("<", "").Replace("≤", "");
                                info.ItmMaxValueCal = str_itm_max;
                            }
                        }
                    }
                    #endregion

                    #region 格式化危急值
                    string str_pan_l = string.Empty;
                    string str_pan_h = string.Empty;

                    if (!string.IsNullOrEmpty(info.ItmDangerLowerLimitCal) && info.ItmDangerLowerLimitCal.Trim(null) != string.Empty)
                    {
                        str_pan_l = info.ItmDangerLowerLimitCal.Trim(null);

                        if (str_pan_l.StartsWith(">") || str_pan_l.StartsWith(">=") || str_pan_h.StartsWith("≥"))
                        {
                            str_pan_l = str_pan_l.Replace(">=", "").Replace(">", "").Replace("≥", "");
                            info.ItmDangerLowerLimitCal = str_pan_l;
                        }
                        else if (str_pan_l.StartsWith("<") || str_pan_l.StartsWith("<=") || str_pan_l.StartsWith("≤"))
                        {
                            str_pan_h = str_pan_l.Replace("<=", "").Replace("<", "").Replace("≤", "");
                            info.ItmDangerUpperLimitCal = str_pan_h;
                            info.ItmDangerLowerLimitCal = string.Empty;
                        }
                    }

                    if (!string.IsNullOrEmpty(info.ItmDangerUpperLimitCal) && info.ItmDangerUpperLimitCal.Trim(null) != string.Empty)
                    {
                        str_pan_h = info.ItmDangerUpperLimitCal.Trim(null);


                        if (str_pan_h.StartsWith(">") || str_pan_h.StartsWith(">=") || str_pan_h.StartsWith("≥"))
                        {
                            str_pan_l = str_pan_h.Replace(">=", "").Replace(">", "").Replace("≥", "");
                            info.ItmDangerLowerLimitCal = str_pan_l;
                            info.ItmDangerUpperLimitCal = string.Empty;
                        }
                        else if (str_pan_h.StartsWith("<") || str_pan_h.StartsWith("<=") || str_pan_l.StartsWith("≤"))
                        {
                            str_pan_h = str_pan_h.Replace("<=", "").Replace("<", "").Replace("≤", "");
                            info.ItmDangerUpperLimitCal = str_pan_h;
                        }
                    }
                    #endregion
                }
            }
            return listItmRefInfo;
        }
        /// <summary>
        /// 根据组合id获取项目信息(外送接口用)
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public List<EntityDicItmItem> GetLisSubItemsByComId(string comId)
        {
            List<EntityDicItmItem> list = new List<EntityDicItmItem>();
            IDaoDicItmItem itemDao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (itemDao != null)
            {
                list = itemDao.GetLisSubItemsByComId(comId);
            }
            return list;
        }

        /// <summary>
        /// 从Lis数据库获取项目数据(外送接口用)
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public List<EntityDicItmItem> GetLisSubItems()
        {
            List<EntityDicItmItem> list = new List<EntityDicItmItem>();
            IDaoDicItmItem itemDao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (itemDao != null)
            {
                list = itemDao.GetLisSubItems();
            }
            return list;
        }
    }
}
