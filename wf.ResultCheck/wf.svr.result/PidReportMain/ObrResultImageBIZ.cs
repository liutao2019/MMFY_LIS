using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.root.logon;

namespace dcl.svr.result
{
    public class ObrResultImageBIZ : IObrResultImage
    {
        public EntityResponse DeletePatPhotoResult(EntitySysOperationLog userInfo, string pat_id, string itm_ecd, long pres_key)
        {
            EntityResponse opResult = new EntityResponse();

            if (pres_key > 0)
            {
                IDaoObrResultImage dao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                if (dao == null)
                {
                    opResult.Scusess = false;
                    opResult.ErroMsg = "未找到数据访问";
                }
                else
                {
                    OperationLogger oplog = new OperationLogger(userInfo.OperatUserId, userInfo.OperatServername, "病人资料", pat_id);
                    oplog.Add_DelLog("图像结果", itm_ecd, string.Empty);

                    opResult.Scusess = dao.DeletePatPhotoResult(pres_key);//删除图像结果

                    oplog.Log();
                }
            }
            else
            {
                opResult.Scusess = false;
                opResult.ErroMsg = "传入参数不对pres_key="+ pres_key;
            }

            return opResult;
        }

        /// <summary>
        /// 根据标识id删除图像结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        public bool DeletePatPhotoResultByObrId(string obrId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrId))
            {
                IDaoObrResultImage dao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                if (dao != null)
                {
                    result = dao.DeleteObrResultImageByObrId(obrId);
                }
            }
            return result;
        }

        public List<EntityObrResultImage> GetObrResultImage(string pat_id)
        {
            List<EntityObrResultImage> listObrResImg = new List<EntityObrResultImage>();
            if (!string.IsNullOrEmpty(pat_id))
            {
                IDaoObrResultImage dao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                if (dao != null)
                {
                    listObrResImg = dao.GetObrResultImage(pat_id);
                }
            }
            return listObrResImg;
        }

        public int SaveObrResultImage(EntityObrResultImage eyObrResImg)
        {
            int saveInt = 0;
            IDaoObrResultImage dao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
            if(dao!=null)
            {
                saveInt = dao.SaveObrResultImage(eyObrResImg);
            }
            return saveInt;
        }
    }
}
