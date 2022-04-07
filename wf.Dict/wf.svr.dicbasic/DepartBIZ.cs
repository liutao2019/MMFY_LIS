using System.Collections.Generic;
using dcl.svr.interfaces;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using System.Configuration;

namespace dcl.svr.dicbasic
{
    public class DepartBIZ : IDicCommon
    {
        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubDept depart = request.GetRequestValue<EntityDicPubDept>();
                depart.DeptDelFlag = "1";
                IDaoDic<EntityDicPubDept> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubDept>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(depart);
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
            if (request != null)
            {
                EntityDicPubDept depart = request.GetRequestValue<EntityDicPubDept>();
                IDaoDic<EntityDicPubDept> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubDept>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(depart);
                    response.SetResult(depart);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }
        /// <summary>
        /// 同步科室资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            List<EntityDicPubDept> listUpdateDept = new List<EntityDicPubDept>();//需要更新科室信息
            List<EntityDicPubDept> listInsertDept = new List<EntityDicPubDept>();//需要新增科室信息
            List<EntityDicPubDept> listInterfaceDoc = DCLExtInterfaceFactory.DCLExtInterface.GetDepartInfo();//接口获取的科室信息
            List<EntityDicPubDept> listLisDoc = Search(request).GetResult() as List<EntityDicPubDept>; //系统科室信息
            if (listInterfaceDoc != null && listInterfaceDoc.Count > 0)
            {
                foreach (EntityDicPubDept Dept in listInterfaceDoc)
                {
                    if (listLisDoc.FindIndex(w => w.DeptCode == Dept.DeptCode) > 0)
                    {
                        listUpdateDept.Add(Dept);
                    }
                    else
                    {
                        listInsertDept.Add(Dept);
                    }
                }
            }
            if (listUpdateDept != null && listUpdateDept.Count > 0)
            {
                foreach (EntityDicPubDept deptUpdate in listUpdateDept)
                {
                    EntityRequest requestUpdate = new EntityRequest();
                    requestUpdate.SetRequestValue(deptUpdate);
                    response = Update(requestUpdate);
                }
            }
            if (listInsertDept != null && listInsertDept.Count > 0)
            {
                foreach (EntityDicPubDept deptInsert in listInsertDept)
                {
                    EntityRequest requestInsert = new EntityRequest();
                    requestInsert.SetRequestValue(deptInsert);
                    response = New(requestInsert);
                }
            }
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicPubDept depart = request.GetRequestValue<EntityDicPubDept>();
            IDaoDic<EntityDicPubDept> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubDept>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];

                List<EntityDicPubDept> list = dao.Search(null);

                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    list = list.FindAll(w => w.DeptHospital == strHospitalId);
                }

                response.SetResult(list);
            }
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicPubDept depart = request.GetRequestValue<EntityDicPubDept>();
                IDaoDic<EntityDicPubDept> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubDept>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(depart);
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
