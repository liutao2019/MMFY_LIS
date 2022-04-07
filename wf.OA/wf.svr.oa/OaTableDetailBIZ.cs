using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.dicbasic;

namespace dcl.svr.oa
{
    public class OaTableDetailBIZ : IOaTabelDetail
    {
        public bool CopyOneDetailToNew(EntityOaTableDetail detail)
        {
            IDaoOaTableDetail dao = DclDaoFactory.DaoHandler<IDaoOaTableDetail>();
            List<EntityOaTableDetail> listDetail = new List<EntityOaTableDetail>();
            bool result = false;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {

                try
                {
                    listDetail = dao.GetTabDetailByTabCode(detail);
                    if (listDetail.Count > 0)
                    {
                        result = dao.InsertNewTabDetail(listDetail[0]);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }

            }
        }

        public bool DeleteTabetail(EntityOaTableDetail detail)
        {
            IDaoOaTableDetail dao = DclDaoFactory.DaoHandler<IDaoOaTableDetail>();
            bool result;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {

                try
                {
                    result = dao.DeleteTabDetail(detail);
                    return result;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }

            }
        }

        public List<EntityDicItmItem> GetItemList()
        {
            List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
            try
            {
                listItem = new ItemBIZ().GetItemByItmId("");
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listItem;
        }

        public List<EntityOaTableField> GetOrderTableField(string tabCode)
        {
            OrderTableBIZ tableBiz = new OrderTableBIZ();
            List<EntityOaTableField> fields = tableBiz.GetOrderTableField(tabCode).GetResult() as List<EntityOaTableField>;
            return fields;
        }

        public List<EntityOaTableDetail> GetTabDetailByTabCode(EntityOaTableDetail detail)
        {
            IDaoOaTableDetail dao = DclDaoFactory.DaoHandler<IDaoOaTableDetail>();
            List<EntityOaTableDetail> tabDetail = null;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return tabDetail;
            }
            else
            {

                try
                {
                    tabDetail = dao.GetTabDetailByTabCode(detail);
                    return tabDetail;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return tabDetail;
                }

            }
        }

        public bool InsertNewTabDetail(EntityOaTableDetail detail)
        {
            IDaoOaTableDetail dao = DclDaoFactory.DaoHandler<IDaoOaTableDetail>();
            bool result;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {

                try
                {
                    result = dao.InsertNewTabDetail(detail);
                    return result;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }

            }
        }

        public bool UpdateTabDetail(EntityOaTableDetail detail)
        {
            IDaoOaTableDetail dao = DclDaoFactory.DaoHandler<IDaoOaTableDetail>();
            bool result;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {

                try
                {
                    result = dao.UpdateTabDetail(detail);
                    return result;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }

            }
        }
    }
}
