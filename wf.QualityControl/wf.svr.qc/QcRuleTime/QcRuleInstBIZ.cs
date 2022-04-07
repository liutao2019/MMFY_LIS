using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.qc
{
    #region 新代码
    public class QcRuleInstBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            throw new Exception();
        }

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                List<EntityDicQcRuleTime> rule = request.GetRequestValue<List<EntityDicQcRuleTime>>();

                IDaoDic<EntityDicQcRuleTime> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRuleTime>>();

                IDaoDic<EntityDicQcInstrmtChannel> daoChannel = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcInstrmtChannel>>();

                IDaoQcRuleMes daoMes = DclDaoFactory.DaoHandler<IDaoQcRuleMes>();

                if (dao == null || daoChannel == null || daoMes == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    EntityDicQcInstrmtChannel channel = new EntityDicQcInstrmtChannel();
                    channel.ItrId = rule[0].QrtItrId;
                    channel.MatTimeId = rule[0].QrtId.ToString();

                    EntityDicQcRuleMes ruleMes = new EntityDicQcRuleMes();
                    ruleMes.QcmItrId = rule[0].QrtItrId;
                    ruleMes.QrmRootNodeId = rule[0].QrtId;

                    dao.Delete(rule[0]);
                    daoChannel.Delete(channel);
                    daoMes.Delete(ruleMes);
                    response.Scusess = true;
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
                List<Object> obj = request.GetRequestValue() as List<Object>;

                List<EntityDicQcRuleTime> ruleTime = obj[0] as List<EntityDicQcRuleTime>;
                List<EntityDicQcInstrmtChannel> instrChannel = obj[1] as List<EntityDicQcInstrmtChannel>;

                IDaoDic<EntityDicQcRuleTime> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRuleTime>>();
                IDaoDic<EntityDicQcInstrmtChannel> daoChannel = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcInstrmtChannel>>();

                if (dao == null || daoChannel == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    string  matTimeID = string.Empty;
                   
                    if (ruleTime[0].QrtId == 0)
                    {
                        dao.Save(ruleTime[0]);
                        bool isTrue = false;
                        List<EntityDicQcRuleTime> listOne= dao.Search(isTrue);//获取新插入数据的主键值
                        matTimeID= listOne[0].QrtId.ToString();
                    }
                    else
                    {
                        dao.Update(ruleTime[0]);

                        matTimeID=ruleTime[0].QrtId.ToString();
                    }

                    EntityDicQcInstrmtChannel channel = new EntityDicQcInstrmtChannel();
                    channel.ItrId = ruleTime[0].QrtItrId;
                    channel.MatTimeId = ruleTime[0].QrtId.ToString();

                    daoChannel.Delete(channel);

                    foreach (var info in instrChannel)
                    {
                        info.MatTimeId = matTimeID;
                        daoChannel.Save(info);
                    }

                    response.Scusess = true;
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

            EntityDicQcInstrmtChannel channel = request.GetRequestValue<EntityDicQcInstrmtChannel>();
            IDaoDic<EntityDicQcInstrmtChannel> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcInstrmtChannel>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(channel));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicQcRuleTime ruleTime = request.GetRequestValue<EntityDicQcRuleTime>();
            IDaoDic<EntityDicQcRuleTime> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRuleTime>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(ruleTime));
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntityDicQcInstrmtChannel channel = new EntityDicQcInstrmtChannel();
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(channel));
            }
            return response;
        }
    }
    #endregion
}
