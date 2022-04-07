using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class ConResAdjustBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                EntityDicResAdjust type = new EntityDicResAdjust();
                object objResAdjust = dict["ResAdjust"];
                if (objResAdjust != null)
                {
                    type = objResAdjust as EntityDicResAdjust;
                }
                IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    LogDbLOG(dict);
                    response.Scusess = dao.Delete(type);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        #region ICommonBIZ 成员

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                List<EntityDicResAdjust> type = new List<EntityDicResAdjust>();
                object objResAdjust = dict["ResAdjust"];
                if (objResAdjust != null)
                {
                    type = objResAdjust as List<EntityDicResAdjust>;
                }
                IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();

                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    LogDbLOG(dict);
                    foreach (var item in type)
                    {
                        // response.Scusess = dao.Delete(item);
                        response.Scusess = dao.Save(item);
                        response.SetResult(item);
                    }

                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicInstrument type = request.GetRequestValue<EntityDicInstrument>();
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type));
            }
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicResAdjust type = request.GetRequestValue<EntityDicResAdjust>();
            IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(type.ItrId));
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                List<EntityDicResAdjust> type = new List<EntityDicResAdjust>();
                object objResAdjust = dict["ResAdjust"];
                if (objResAdjust != null)
                {
                    type = objResAdjust as List<EntityDicResAdjust>;
                }
                EntityDicResAdjust Itr = new EntityDicResAdjust();
                IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    LogDbLOG(dict);
                    var i = 0;
                    foreach (var item in type)
                    {
                        if (i == 0)
                        {
                            Itr.ItrId = item.ItrId;
                            response.Scusess = dao.Delete(Itr);
                        }
                        response.Scusess = dao.Save(item);
                        response.SetResult(item);
                        i++;
                    }

                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        private void LogDbLOG(Dictionary<string, object> dict)
        {
            EntitySysOperationLog dtOperation = (EntitySysOperationLog)dict["Operation"];
            //DataTable _LOGINFO = dsDataPack.Tables["_LOGINFO"];

            List<EntityDicResAdjust> listNewDicResAdjust = dict["ResAdjust"] as List<EntityDicResAdjust>;

            EntityDicResAdjust deleoper = dict["ResAdjust"] as EntityDicResAdjust;

            EntityResponse response = new EntityResponse();

            string operationName = dtOperation.OperatAction;
            string old_itr_id = "";
            if (operationName == "删除")
                old_itr_id = deleoper?.ItrId;
            else
                old_itr_id = listNewDicResAdjust[0].ItrId;

            //string new_com_name = listNewDicItrCombine[0].;
            //string new_com_ecd = listNewDicItrCombine[0].ComCode;

            
            string loginID = dtOperation.OperatUserId;
            string userName = dtOperation.OperatUserName;
            string ipAdress = IPUtility.GetIP();
            DateTime dtNow = cache.ServerDateTime.GetDatabaseServerDateTime();
            if (operationName == "新增")
            {
                string mitcno = string.Empty;
                if (listNewDicResAdjust.Count > 0)
                {
                    foreach (EntityDicResAdjust just in listNewDicResAdjust)
                    {
                        mitcno += string.Format(",{0}", just.MitCno);
                    }
                }

                mitcno = mitcno.Remove(0, 1);
                new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "新增", "通道码", "新增通道码" + mitcno, null);
            }
            else if (operationName == "删除")
            {
                string mitcno = string.Empty;

                List<EntityDicResAdjust> listOldDicResAdjust = new List<EntityDicResAdjust>();

                IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
                if (dao != null)
                {
                    listOldDicResAdjust = dao.Search(old_itr_id);
                }
                if (listOldDicResAdjust.Count > 0)
                {
                    foreach (EntityDicResAdjust just in listOldDicResAdjust)
                    {
                        mitcno += string.Format(",{0}", just.MitCno);
                    }
                }

                mitcno = mitcno.Remove(0, 1);
                new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "删除", "通道码", "删除通道码" + mitcno, null);
            }
            else if (operationName == "修改")
            {
                //原结果
                List<EntityDicResAdjust> listOldDicResAdjust = new List<EntityDicResAdjust>();
                IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
                if (dao != null)
                {
                    listOldDicResAdjust = dao.Search(old_itr_id);
                }
                foreach (EntityDicResAdjust just in listNewDicResAdjust)
                {
                    List<EntityDicResAdjust> listEntityDicResAdjustTemp = listOldDicResAdjust.FindAll(w => w.ItrId == old_itr_id && w.MitCno == just.MitCno &&w.Adjkey==just.Adjkey);
                    if (listEntityDicResAdjustTemp != null && listEntityDicResAdjustTemp.Count > 0)
                    {
                        EntityDicResAdjust  temp = listEntityDicResAdjustTemp[0];
                        if (temp.SrcRes != just.SrcRes)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "原结果", temp.SrcRes, just.SrcRes);
                        }
                        if (temp.SrcSid != just.SrcSid)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "原样本号", temp.SrcSid, just.SrcSid);
                        }
                        if (temp.ResMultiple != just.ResMultiple)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "调整因子", temp.ResMultiple, just.ResMultiple);
                        }
                        if (temp.ResDecPlace != just.ResDecPlace)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "小数位数", temp.ResDecPlace, just.ResDecPlace);
                        }
                        if (temp.DstRes != just.DstRes)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "替换结果", temp.DstRes, just.DstRes);
                        }
                        if (temp.DstSid != just.DstSid)
                        {
                            new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "修改", "替换样本号", temp.DstSid, just.DstSid);
                        }
                        //}
                    }
                    else
                    {
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "新增", "通道码", "新增通道码" + just.MitCno, null);
                        string mitCno = string.Empty;
                        string adjustKey = string.Empty;
                        foreach (EntityDicResAdjust adjust in listOldDicResAdjust)
                        {
                            List<EntityDicResAdjust> listNewTemp2 = listNewDicResAdjust.FindAll(w => w.ItrId == old_itr_id && w.MitCno == adjust.MitCno &&  w.Adjkey == just.Adjkey);
                            if (listNewTemp2.Count <= 0)
                            {
                                mitCno = adjust.MitCno;
                                adjustKey = adjust.Adjkey.ToString();
                            }
                        }
                        new ItemBIZ().LogDBField(loginID, ipAdress, dtNow, old_itr_id, "结果调整", "结果调整", "删除", "通道码", "删除通道码" + mitCno+"主键"+ adjustKey, null);
                    }
                }
            }
        }

    }
}

#endregion

