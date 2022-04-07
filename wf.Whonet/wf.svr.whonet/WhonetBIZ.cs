using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.dicmic;
using dcl.svr.result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcl.svr.whonet
{
    public class WhonetBIZ: IWhonet
    {
        /// <summary>
        /// 获取记录数量(抗生素)只获取抗生素名称，用于分段获取防止超时
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<string> GetAntibosName(EntityAntiQc qc)
        {
            List<string> list = new List<string>();
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao != null)
            {
                list = dao.GetAntibosName(qc);
            }
            return list;
        }

        public List<EntityWhonet> GetAntiData(EntityAntiQc qc)
        {
            List<EntityWhonet> list = new List<EntityWhonet>();
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao != null)
            {
                list = dao.GetAntiData(qc);
            }
            return list;
            }
        /// <summary>
        /// 更新药敏信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UpdateAnti(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            response= new Dict_AntibioBIZ().Update(request);
            return response.Scusess;
        }
         /// <summary>
         /// 更新细菌信息
         /// </summary>
         /// <param name="request"></param>
         /// <returns></returns>
        public bool UpdateBac(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            response = new Dict_BacteriBIZ().Update(request);
            return response.Scusess;
        }
    }
    }

