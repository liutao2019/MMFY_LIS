using System.Collections.Generic;
using dcl.svr.interfaces;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using System.Configuration;

namespace dcl.svr.dicbasic
{
    public class DoctorBIZ : IDicCommon
    {
        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicDoctor doctor = request.GetRequestValue<EntityDicDoctor>();
                IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(doctor);
                    response.SetResult(doctor);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictDoctorCache.Current.Refresh();
            return response;
        }

        public EntityResponse Delete(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicDoctor doctor = request.GetRequestValue<EntityDicDoctor>();
                doctor.DelFlag = "1";
                IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(doctor);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictDoctorCache.Current.Refresh();
            return response;
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicDoctor doctor = request.GetRequestValue<EntityDicDoctor>();
                IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(doctor);
                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            dcl.svr.cache.DictDoctorCache.Current.Refresh();
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicDoctor doctor = request.GetRequestValue<EntityDicDoctor>();
            IDaoDic<EntityDicDoctor> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicDoctor>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            { 
                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];

                List<EntityDicDoctor> list = dao.Search(doctor);

                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    list = list.FindAll(w => w.DoctorHospital == strHospitalId);
                }

                response.Scusess = true;
                response.SetResult(list);
            }
            return response;
        }
        /// <summary>
        /// 同步医生资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            List<EntityDicDoctor> listUpdateDoc = new List<EntityDicDoctor>();//需要更新医生信息
            List<EntityDicDoctor> listInsertDoc = new List<EntityDicDoctor>();//需要新增医生信息
            List<EntityDicDoctor> listInterfaceDoc = DCLExtInterfaceFactory.DCLExtInterface.GetDoctorInfo();//接口获取的医生信息
            List<EntityDicDoctor> listLisDoc = Search(request).GetResult() as List<EntityDicDoctor>; //系统医生信息
            if (listInterfaceDoc != null && listInterfaceDoc.Count > 0)
            {
                foreach (EntityDicDoctor doc in listInterfaceDoc)
                {
                    if (listLisDoc.FindIndex(w => w.DoctorCode == doc.DoctorCode) > 0)
                    {
                        listUpdateDoc.Add(doc);
                    }
                    else
                    {
                        listInsertDoc.Add(doc);
                    }
                }
            }
            if (listUpdateDoc != null && listUpdateDoc.Count > 0)
            {
                foreach (EntityDicDoctor docUpdate in listUpdateDoc)
                {
                    EntityRequest requestUpdate = new EntityRequest();
                    requestUpdate.SetRequestValue(docUpdate);
                    response = Update(requestUpdate);
                }
            }
            if (listInsertDoc != null && listInsertDoc.Count > 0)
            {
                foreach (EntityDicDoctor docInsert in listInsertDoc)
                {
                    EntityRequest requestInsert = new EntityRequest();
                    requestInsert.SetRequestValue(docInsert);
                    response = New(requestInsert);
                }
            }
            dcl.svr.cache.DictDoctorCache.Current.Refresh();
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            return new EntityResponse();
        }
    }
}
