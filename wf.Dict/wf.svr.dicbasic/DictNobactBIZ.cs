using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.dicbasic
{
    public class DictNobactBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicMicSmear smear = request.GetRequestValue<EntityDicMicSmear>();
                smear.SmeDelFlag = "1";
                IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
                IDaoDic<EntityDicNobactCom> dao2 = DclDaoFactory.DaoHandler<IDaoDic<EntityDicNobactCom>>();
                EntityDicNobactCom noCom = new EntityDicNobactCom() { NobId = smear.SmeId };
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Delete(smear);
                    if (response.Scusess)
                    {
                        dao2.Delete(noCom);
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

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            Dictionary<string, object> dict = request.GetRequestValue<Dictionary<string, object>>();
            if (dict != null)
            {
                EntityDicMicSmear smear = dict["newSmear"] as EntityDicMicSmear;
                List<EntityDicCombine> changeCombineList = dict["combines"] as List<EntityDicCombine>;
                IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
                IDaoDic<EntityDicNobactCom> dao2 = DclDaoFactory.DaoHandler<IDaoDic<EntityDicNobactCom>>();
                if (dao == null || dao2 == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(smear);
                    response.SetResult(smear);
                }
                if (response.Scusess && changeCombineList.Count > 0)
                {
                    string nobId = smear.SmeId;
                    EntityDicNobactCom nobactCom = new EntityDicNobactCom() { NobId = nobId };
                    dao2.Delete(nobactCom);
                    foreach (EntityDicCombine combine in changeCombineList)
                    {
                        EntityDicNobactCom noCom = new EntityDicNobactCom();
                        noCom.NobId = nobId;
                        noCom.ComId = combine.ComId;
                        dao2.Save(noCom);
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
            string i = request.GetRequestValue<string>();

            if (i == "GetCombine")
            {
                IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.GetNoBactCombine());
                }
            }
            else
            {
                IDaoDic<EntityDicNobactCom> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicNobactCom>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = true;
                    response.SetResult(dao.Search(i));
                }
            }


            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicMicSmear smear = request.GetRequestValue<EntityDicMicSmear>();
            IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(smear));
            }
            return response;
        }
        public List<EntityDicMicSmear> GetMicSmear()
        {
            List<EntityDicMicSmear> list = new List<EntityDicMicSmear>();
            IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
            if (dao != null)
            {
                list = dao.SearchMicSmear();
            }
            return list;
        }
        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            Dictionary<string, object> dict = request.GetRequestValue<Dictionary<string, object>>();
            if (dict != null)
            {
                EntityDicMicSmear smear = dict["newSmear"] as EntityDicMicSmear;
                List<EntityDicCombine> changeCombineList = dict["combines"] as List<EntityDicCombine>;
                IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
                IDaoDic<EntityDicNobactCom> dao2 = DclDaoFactory.DaoHandler<IDaoDic<EntityDicNobactCom>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(smear);
                }
                if (response.Scusess)
                {
                    string nobId = smear.SmeId;
                    EntityDicNobactCom nobactCom = new EntityDicNobactCom() { NobId = nobId };
                    dao2.Delete(nobactCom);
                    foreach (EntityDicCombine combine in changeCombineList)
                    {
                        EntityDicNobactCom noCom = new EntityDicNobactCom();
                        noCom.NobId = nobId;
                        noCom.ComId = combine.ComId;
                        dao2.Save(noCom);
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
            return new EntityResponse();
        }
    }
}
