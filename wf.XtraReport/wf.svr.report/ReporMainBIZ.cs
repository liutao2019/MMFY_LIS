using System;
using System.Collections.Generic;
using System.Data;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using Lib.DataInterface.Implement;
using Lib.DAC;

namespace dcl.svr.report
{
    public class ReporMainBIZ : IReportMain
    {
        public EntityResponse GetReport(EntityRequest request)
        {

            EntityResponse response = new EntityResponse();
            EntitySysReport type = request.GetRequestValue<EntitySysReport>();
            IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.GetReport());
            }
            return response;
        }

        public EntityResponse GetReportPar(string id)
        {
            EntityResponse response = new EntityResponse();
            IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.GetReportParameter(Convert.ToInt32(id)));
            }
            return response;
        }

        public EntityResponse NewReport(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntitySysReport report = request.GetRequestValue<EntitySysReport>();
                IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.SaveReport(report);
                    response.SetResult(report);

                }

            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse NewReportParameter(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                EntitySysReportParameter par = request.GetRequestValue<EntitySysReportParameter>();
                IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.SaveReportParameter(par);
                    response.SetResult(par);

                }

            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse UpdateReport(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntitySysReport report = request.GetRequestValue<EntitySysReport>();
            if (request != null)
            {

                IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    response.Scusess = dao.UpdateReport(report);
                    response.SetResult(report);

                }
            }
            else
            {
                response.Scusess = false;
                response.ErroMsg = "传入参数为Null";
            }
            return response;
        }

        public EntityResponse DeleteReport(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            if (request != null)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)request.GetRequestValue();
                List<EntitySysReport> listReport = new List<EntitySysReport>();
                EntitySysReportParameter Parameter = new EntitySysReportParameter();
                if (dict.ContainsKey("Report"))
                {
                    object objReport = dict["Report"];
                    if (objReport != null)
                    {
                        listReport = objReport as List<EntitySysReport>;
                    }
                    IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
                    if (dao == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        int count = 0; ;
                        foreach (EntitySysReport rep in listReport)
                        {
                            if (dao.DeleteReport(rep))
                            {
                                count++;
                            }
                        }
                        if (count == listReport.Count)
                        {
                            response.Scusess = true;
                        }
                        response.SetResult(listReport);

                    }
                }
                if (dict.ContainsKey("Parameter"))
                {
                    object objPar = dict["Parameter"];
                    if (objPar != null)
                    {
                        Parameter = objPar as EntitySysReportParameter;
                    }

                    IDaoReport daoPar = DclDaoFactory.DaoHandler<IDaoReport>();
                    if (daoPar == null)
                    {
                        response.Scusess = false;
                        response.ErroMsg = "未找到数据访问";
                    }
                    else
                    {
                        response.SetResult(daoPar.DeleteReportParameter(Parameter));
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


        public List<EntitySysReport> GetRepLocationByListCode(List<string> strCode)
        {
            List<EntitySysReport> listSysRep = new List<EntitySysReport>();
            IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
            if (dao != null)
            {
                listSysRep = dao.GetRepLocationByListCode(strCode);
            }
            return listSysRep;
        }

        public EntitySysReport GetReportByRepCode(string strCode)
        {
            EntitySysReport SysRep = new EntitySysReport();
            IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
            if (dao != null)
            {
                SysRep = dao.GetReportByRepCode(strCode);
            }
            return SysRep;
        }

        public DataTable GetSqlResult(DataTable dtWhere)
        {
            DataTable result = null;
            try
            {
                string sql = dtWhere.Rows[0]["id"].ToString();
                string conn_code = dtWhere.Rows[0]["conn_code"].ToString();

                SqlHelper helper;
                //没有指定数据连接串则使用检验系统
                if (string.IsNullOrEmpty(conn_code))
                {
                    helper = new SqlHelper();
                }
                else
                {
                    EntityDictDataInterfaceConnection ettConn = svr.cache.CacheDataInterfaceConnection.Current.GetConnectionByCode(conn_code);
                    if (ettConn != null)
                    {
                        helper = ettConn.GetSqlHelper();
                    }
                    else
                    {
                        helper = new SqlHelper();
                    }
                }

                result = helper.GetTable(sql);
                result.TableName = "dataTable";
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileNameFullPath"></param>
        /// <param name="strUrlDirPath"></param>
        /// <returns></returns>
        public bool UpLoadReportFile(EntityRequest request)
        {
            EntitySysReport report = request.GetRequestValue<EntitySysReport>();
            IDaoReport dao = DclDaoFactory.DaoHandler<IDaoReport>();
            if (dao != null)
            {
                return dao.UpLoadReportFile(report);
            }
            return false;
        }
    }
}
