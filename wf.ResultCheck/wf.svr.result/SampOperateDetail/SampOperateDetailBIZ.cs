using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    public class SampOperateDetailBIZ : ISampOperateDetail
    {
        public List<EntitySampOperateDetail> GetBarCodeExtend(string barCode)
        {
            List<EntitySampOperateDetail> listSampOperDetail = new List<EntitySampOperateDetail>();
            IDaoSampOperateDetail dao = DclDaoFactory.DaoHandler<IDaoSampOperateDetail>();
            if (dao != null)
            {
                listSampOperDetail = dao.GetBarCodeExtend(barCode);
            }

            return listSampOperDetail;
        }

        public List<EntitySampImage> GetSampImage(string barCode)
        {
            List<EntitySampImage> listSampImage = new List<EntitySampImage>();
            IDaoSampOperateDetail dao = DclDaoFactory.DaoHandler<IDaoSampOperateDetail>();
            if (dao != null)
            {
                listSampImage = dao.GetSampImage(barCode);
            }

            return listSampImage;
        }
    }
}
