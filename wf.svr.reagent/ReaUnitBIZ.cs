using dcl.entity;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace wf.svr.dicreagent
{
    public class ReaUnitBIZ : IDicCommon
    {
        #region ICommonBIZ 成员
        //private DbBase dao = DbBase.InConn();
        //public DataSet doDel(DataSet ds)
        //{
        //    DataSet result = new DataSet();
        //    try
        //    {
        //        string sqlstring = "update " + LIS_Const.tableName.ZD_HOSPITAL + " set hos_del='" + LIS_Const.del_flag.DEL + "' where hos_id='" + ds.Tables[LIS_Const.tableName.ZD_HOSPITAL].Rows[0]["hos_id"].ToString() + "'";
        //        ArrayList arrNew = new ArrayList();
        //        arrNew.Add(sqlstring);
        //        dao.DoTran(arrNew);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
        //    }
        //    return result;
        //}

        //public DataSet doNew(DataSet ds)
        //{
        //    DataSet result = new DataSet();
        //    try
        //    {
        //        DataTable dtNew = ds.Tables[LIS_Const.tableName.ZD_HOSPITAL];
        //        int id = dao.GetID(LIS_Const.tableName.ZD_HOSPITAL, dtNew.Rows.Count);

        //        foreach (DataRow dr in dtNew.Rows)
        //        {
        //            dr["hos_id"] = id - dtNew.Rows.Count + 1;
        //            id++;
        //        }

        //        ArrayList arrNew = dao.GetInsertSql(dtNew);
        //        dao.DoTran(arrNew);
        //        result.Tables.Add(dtNew.Copy());
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
        //    }
        //    return result;
        //}

        //public DataSet doOther(DataSet ds)
        //{
        //    throw new NotImplementedException();
        //}

        //public DataSet doSearch(DataSet ds)
        //{
        //    DataSet result = new DataSet();
        //    try
        //    {
        //        String sql = "select  * from " + LIS_Const.tableName.ZD_HOSPITAL + " where hos_del='"+LIS_Const.del_flag.OPEN+"'  order by hos_seq ";
        //        DataTable dt = dao.GetDataTable(sql);
        //        dt.TableName = LIS_Const.tableName.ZD_HOSPITAL;
        //        result.Tables.Add(dt);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
        //    }
        //    return result;
        //}

        //public DataSet doUpdate(DataSet ds)
        //{
        //    DataSet result = new DataSet();
        //    try
        //    {
        //        DataTable dtUpdate = ds.Tables["dict_hospital"];
        //        ArrayList arrUpdate = dao.GetUpdateSql(dtUpdate, new string[] { "hos_id" });
        //        dao.DoTran(arrUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
        //    }
        //    return result;
        //}

        //public DataSet doView(DataSet ds)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        public EntityResponse New(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntityDicReaUnit sample = request.GetRequestValue<EntityDicReaUnit>();
                IDaoDic<EntityDicReaUnit> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaUnit>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Save(sample);
                    response.SetResult(sample);
                }
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
                EntityDicReaUnit reaUnit = request.GetRequestValue<EntityDicReaUnit>();
                reaUnit.del_flag = "1";
                IDaoDic<EntityDicReaUnit> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaUnit>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(reaUnit);
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
                EntityDicReaUnit reaUnit = request.GetRequestValue<EntityDicReaUnit>();
                IDaoDic<EntityDicReaUnit> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaUnit>>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.Update(reaUnit);
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

            EntityDicReaUnit sample = request.GetRequestValue<EntityDicReaUnit>();
            IDaoDic<EntityDicReaUnit> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicReaUnit>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(sample));
            }
            return response;
        }

        public EntityResponse Other(EntityRequest request)
        {
            return new EntityResponse();
        }

        public EntityResponse View(EntityRequest request)
        {
            return new EntityResponse();
        }
    }
}
