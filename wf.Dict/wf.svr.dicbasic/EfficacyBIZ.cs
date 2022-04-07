using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using dcl.servececontract;

namespace dcl.svr.dicbasic
{

    public class EfficacyBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicItmCheck check = request.GetRequestValue<EntityDicItmCheck>();
                IDaoDic<EntityDicItmCheck> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheck>>();

                EntityDicItmCheckDetail checkDetail = request.GetRequestValue<EntityDicItmCheckDetail>();
                IDaoDic<EntityDicItmCheckDetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheckDetail>>();

                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    bool boolCheck = dao.Save(check);
                    bool boolCheckDetail = daoDetail.Save(checkDetail);
                    if (boolCheck && boolCheckDetail)
                    {
                        response.Scusess = true;
                    }

                    response.SetResult(check);//这里只能传主表数据
                    //response.SetResult(daoDetail);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictEffcacyItemCache.Current.Refresh();
            dcl.svr.cache.DictEfficacyGroupCache.Current.Refresh();
            return response;
        }

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                IDaoDic<EntityDicItmCheck> daoCheck = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheck>>();
                IDaoDic<EntityDicItmCheckDetail> daoCheckDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheckDetail>>();

                if (daoCheck == null || daoCheckDetail == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                    return response;
                }
                else
                {
                    string itrID = request.GetRequestValue<string>();

                    //删除本仪器所有校验公式
                    if (!string.IsNullOrEmpty(itrID))
                    {
                        EntityDicItmCheck info = new EntityDicItmCheck();
                        info.CheckItrId = itrID;
                        List<EntityDicItmCheck> check = daoCheck.Search(info);
                        List<EntityDicItmCheckDetail> checkDetail = daoCheckDetail.Search(info);

                        foreach (var checkInfo in check)
                        {
                            daoCheck.Delete(checkInfo);
                        }
                        foreach (var detail in checkDetail)
                        {
                            daoCheckDetail.Delete(detail);
                        }
                        response.Scusess = true;
                    }
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictEffcacyItemCache.Current.Refresh();
            dcl.svr.cache.DictEfficacyGroupCache.Current.Refresh();
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                List<Object> listAll = request.GetRequestValue() as List<Object>;

                EntityDicItmCheck itmCheck = listAll[0] as EntityDicItmCheck;
                List<EntityDicItmCheck> checkSav = listAll[1] as List<EntityDicItmCheck>;
                List<EntityDicItmCheckDetail> checkDetailSav = listAll[2] as List<EntityDicItmCheckDetail>;

                //List<EntityDicItmCheck> check = listAll[2] as List<EntityDicItmCheck>;
                //List<EntityDicItmCheckDetail> checkDetail = listAll[3] as List<EntityDicItmCheckDetail>;

                IDaoDic<EntityDicItmCheck> daoCheck = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheck>>();
                IDaoDic<EntityDicItmCheckDetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheckDetail>>();

                if (daoCheck == null || daoDetail == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    List<EntityDicItmCheck> check = daoCheck.Search(itmCheck);
                    List<EntityDicItmCheckDetail> checkDetail = daoDetail.Search(itmCheck);

                    foreach (var detail in checkDetail)
                    {
                        daoDetail.Delete(detail);
                    }

                    foreach (var checkInfo in check)
                    {
                        daoCheck.Delete(checkInfo);
                    }

                    if (checkSav.Count > 0)
                    {
                        foreach (EntityDicItmCheck savInfo in checkSav)
                        {
                            savInfo.DetailList = checkDetailSav.FindAll(a => a.CheckIdDetial == savInfo.CheckId);
                            daoCheck.Save(savInfo); //保存校验组数据
                        }
                    }
                    //if (checkDetailSav.Count > 0)
                    //{
                    //    foreach (EntityDicItmCheckDetail savDeInfo in checkDetailSav)
                    //    {
                    //        daoDetail.Save(savDeInfo); //保存校验明细数据
                    //    }
                    //}

                    response.Scusess = true;
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictEffcacyItemCache.Current.Refresh();
            dcl.svr.cache.DictEfficacyGroupCache.Current.Refresh();
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            string itrID = request.GetRequestValue<string>();

            IDaoDic<EntityDicItmCheck> daoCheck = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheck>>();
            IDaoDic<EntityDicItmCheckDetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheckDetail>>();

            if (daoCheck == null || daoDetail == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                EntityDicItmCheck info = new EntityDicItmCheck();
                info.CheckItrId = itrID;
                List<EntityDicItmCheck> check = daoCheck.Search(info);
                List<EntityDicItmCheckDetail> checkDetail = daoDetail.Search(info);
                List<Object> listAll = new List<object>();
                listAll.Add(check);
                listAll.Add(checkDetail);

                response.SetResult(listAll);

            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            string itrID = request.GetRequestValue<string>();
            IDaoDic<EntityDicItmCheckDetail> daoDetail = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItmCheckDetail>>();
            if (daoDetail == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                EntityDicItmCheck info = new EntityDicItmCheck();
                info.CheckItrId = itrID;
                List<EntityDicItmCheckDetail> checkDetail = daoDetail.Search(info);

                response.SetResult(checkDetail);

            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            List<Object> listAll = new List<Object>();

            IDaoDicInstrument daoInstr = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            List<EntityDicInstrument> instrument = daoInstr.Search(new Object());

            List<EntityDicItmItem> itmItem = new ItemBIZ().GetItemByItmId("");

            listAll.Add(instrument);
            listAll.Add(itmItem);

            response.SetResult(listAll);

            return response;
        }
    }

}
