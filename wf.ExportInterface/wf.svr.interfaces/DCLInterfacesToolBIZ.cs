using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace dcl.svr.interfaces
{
    public class DCLInterfacesToolBIZ : IDCLInterfacesTool
    {
        public EntityResponse UploadDCLReport(int Number)
        {
            EntityResponse response = new EntityResponse();

            try
            {
                IDaoSysInterfaceLog dao = DclDaoFactory.DaoHandler<IDaoSysInterfaceLog>();

                if (dao != null)
                {
                    EntitySysInterfaceLog interfaceQc = new EntitySysInterfaceLog();
                    interfaceQc.OperationName = "上传数据";
                    interfaceQc.OperationSuccess = 0;

                    List<EntitySysInterfaceLog> listSysInterfaceLog = dao.GetSysInterfaceLogInNumber(interfaceQc, Number);

                    List<string> listRepId = new List<string>();

                    foreach (EntitySysInterfaceLog log in listSysInterfaceLog)
                    {
                        if (!string.IsNullOrEmpty(log.RepId) &&
                             dao.DeleteSysInterfaceLog(log.OperationKey))
                        {
                            listRepId.Add(log.RepId);
                        }
                    }

                    NameValueCollection result = DCLExtInterfaceFactory.DCLExtInterface.UploadDCLReport(listRepId);
                    response.Scusess = true;
                    response.SetResult(result);
                }
            }
            catch (Exception ex)
            {
                response.Scusess = false;
                response.ErroMsg = ex.ToString();
                Logger.LogException(ex);
            }

            return response;
        }


        public EntityResponse ReUploadDCLReport(List<string> RepId)
        {
            EntityResponse response = new EntityResponse();

            try
            {
                DCLExtInterfaceFactory.DCLExtInterface.UploadDCLReportAsync(RepId);

                response.Scusess = true;
                response.SetResult(response);
            }
            catch (Exception ex)
            {
                response.Scusess = false;
                response.ErroMsg = ex.ToString();
                Logger.LogException(ex);
            }

            return response;
        }

        public EntityResponse ReChargeBarcode(List<EntitySampMain> Barids)
        {
            EntityResponse response = new EntityResponse();

            try
            {
                EntitySampOperation oper = new EntitySampOperation();
                oper.OperationStatus = "5";
                oper.OperationWorkId = "admin";
                oper.OperationName = "管理员";
                oper.OperationID = "admin";
                oper.OperationTime = cache.ServerDateTime.GetDatabaseServerDateTime();
                foreach (EntitySampMain bar in Barids)
                {
                    DCLExtInterfaceFactory.DCLExtInterface.ExecuteInterfaceAfter(oper, bar);
                }
                response.Scusess = true;
                response.SetResult(response);
            }
            catch (Exception ex)
            {
                response.Scusess = false;
                response.ErroMsg = ex.ToString();
                Logger.LogException(ex);
            }
            return response;
        }
    }
}
