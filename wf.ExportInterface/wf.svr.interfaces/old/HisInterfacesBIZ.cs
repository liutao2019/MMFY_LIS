using System;
using System.Collections.Generic;
using System.Data;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.interfaces
{
    public class HISInterfacesBIZ : ISysItfInterfaces
    {

        public EntityResponse DeleteSysInterface(string id)
        {
            EntityResponse response = new EntityResponse();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.DeleteSysInterface(id));
            }
            return response;
        }
        public List<EntitySysItfInterface> GetSysInterface()
        {
            List<EntitySysItfInterface> list = new List<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetSysInterface();
            }
            return list;
        }

        /// <summary>
        /// 根据接口类型获取接口
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public List<EntitySysItfInterface> GetSysInterface(string interfaceType)
        {
            List<EntitySysItfInterface> list = new List<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetSysInterface(interfaceType);
            }
            return list;
        }

        public EntityResponse SaveSysInterface(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntitySysItfInterface type = request.GetRequestValue<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.SaveSysInterface(type));
            }
            return response;
        }


        public EntityResponse UpdateSysInterface(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            EntitySysItfInterface type = request.GetRequestValue<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.UpdateSysInterface(type));
            }
            return response;
        }

        public DataSet TestConnection(EntitySysItfInterface inter)
        {
            //测试接口连接
            HospitalInterface interfaces = new HospitalInterface(
             inter.ItfaceServer,
             inter.ItfaceDatabase,
             inter.ItfaceLogid,
             inter.ItfacePassword,
             inter.ItfaceConnectType,
             inter.ItfaceName);
            bool result = interfaces.TestConnect();

            //返回结果
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable("return");
            dtResult.Columns.Add("value");
            dtResult.Rows.Add(result.ToString());
            dsResult.Tables.Add(dtResult);
            return dsResult;
        }

        public EntityOperationResult CardDataConvert(string cardData, string interfaceKey)
        {
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                throw new Exception("未找到IDaoSysItfInterface的数据访问！");
            }
            else
            {
                EntityOperationResult result = new EntityOperationResult();
                EntitySysItfInterface sysitf = dao.CardDataConvert(cardData,interfaceKey);
                if(sysitf == null)
                {
                    result.AddCustomMessage("CardDataConvert", "卡号转换", string.Format("没有设置名称为：{0} 的接口！", interfaceKey), EnumOperationErrorLevel.Error);
                }
                else
                {
                    string data = string.Empty;
                    HospitalInterface interfaces = new HospitalInterface(
                        sysitf.ItfaceServer, sysitf.ItfaceDatabase,sysitf.ItfaceLogid,sysitf.ItfacePassword,
                        sysitf.ItfaceConnectType,sysitf.ItfaceExecuteSql);

                    DataSet dataset = interfaces.Connecter.ExeInterface(cardData);
                    if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                    {
                        data = dataset.Tables[0].Rows[0][0].ToString();
                    }
                    result.OperationResultData = data;
                }
                return result;
            }
        }
    }
}
