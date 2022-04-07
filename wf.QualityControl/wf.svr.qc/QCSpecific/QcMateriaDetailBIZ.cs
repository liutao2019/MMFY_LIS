using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.qc
{
    public class QcMateriaDetailBIZ : IDicQcMateriaDetail
    {
        public bool DeleteQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            bool isTrue = false;
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                isTrue = dao.DeleteQcMateriaDetail(QMDetail);
            }
            return isTrue;
        }

        public EntityResponse SaveQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            EntityResponse result = new EntityResponse();
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                result = dao.SaveQcMateriaDetail(QMDetail);
            }
            return result;
        }

        public List<EntityDicQcMateriaDetail> SearchQcMateriaDetail(string mat_id)
        {
            List<EntityDicQcMateriaDetail> list = new List<EntityDicQcMateriaDetail>();
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                list = dao.SearchQcMateriaDetail(mat_id);
            }
            return list;
        }

        public bool UpdateQcMateriaDetail(EntityDicQcMateriaDetail QMDetail)
        {
            bool isTrue = false;
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                isTrue = dao.UpdateQcMateriaDetail(QMDetail);
            }
            return isTrue;
        }

        public List<EntityDicQcMateriaDetail> GetQcMateriaDetailItmId(string strItrId)
        {
            List<EntityDicQcMateriaDetail> list = new List<EntityDicQcMateriaDetail>();
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                list = dao.GetQcMateriaDetailItmId(strItrId);
            }
            return list;
        }
        /// <summary>
        /// 批量更新质控项目参数
        /// </summary>
        /// <param name="listQMDetail"></param>
        /// <returns></returns>
        public bool UpdateQcMateriaDetailCondition(List<EntityDicQcMateriaDetail> listQMDetail)
        {
            bool isTrue = false;
            IDaoQcMateriaDetail dao = DclDaoFactory.DaoHandler<IDaoQcMateriaDetail>();
            if (dao != null)
            {
                if (listQMDetail.Count > 0)
                {
                    foreach (EntityDicQcMateriaDetail qMDetail in listQMDetail)
                    {
                        isTrue = dao.UpdateQcMateriaDetailCondition(qMDetail);
                    }
                }
         
            }
            return isTrue;
        }
    }
}
