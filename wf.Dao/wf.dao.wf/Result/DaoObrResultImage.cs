using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResultImage))]
    public class DaoObrResultImage : DclDaoBase, IDaoObrResultImage
    {
        public bool DeleteObrResultImageByObrId(string obrId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrId))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sqlDelete = string.Format("delete from Lis_result_image where Lri_Lresdesc_id='{0}'", obrId);
                    helper.ExecSql(sqlDelete);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("删除图像结果DeleteObrResultImageByObrId", ex);
                }
            }
            return result;
        }

        public bool DeletePatPhotoResult(long pres_key)
        {
            bool isDelete = false;
            if (pres_key > 0)
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sqlDelete = string.Format("delete from Lis_result_image where Lri_id={0}", pres_key);
                    helper.ExecSql(sqlDelete);
                    isDelete = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("删除图像结果DeletePatPhotoResult", ex);
                }
            }

            return isDelete;
        }

        public List<EntityObrResultImage> GetObrResultImage(string pat_id)
        {
            List<EntityObrResultImage> listObrMsgImage = new List<EntityObrResultImage>();
            if (!string.IsNullOrEmpty(pat_id))
            {
                try
                {
                    DBManager helper = new DBManager();

                    string sql = string.Format("select * from Lis_result_image where Lri_Lresdesc_id='{0}' and Lri_flag=1", pat_id);

                    DataTable dtObrResImage = helper.ExecuteDtSql(sql);
                    listObrMsgImage = EntityManager<EntityObrResultImage>.ConvertToList(dtObrResImage).OrderBy(i => i.ObrId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取病人图像结果出错：病人ID=" + pat_id, ex);
                }
            }
            return listObrMsgImage;
        }

        public int SaveObrResultImage(EntityObrResultImage eyObrResImg)
        {
            int saveInt = 0;
            if (eyObrResImg != null)
            {
                try
                {
                    DBManager helper = GetDbManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Lri_Lresdesc_id", eyObrResImg.ObrId);
                    values.Add("Lri_Ditm_ename", eyObrResImg.ObrItmEname);
                    values.Add("Lri_date", eyObrResImg.ObrDate);
                    values.Add("Lri_sid", eyObrResImg.ObrSid);
                    values.Add("Lri_Ditr_id", eyObrResImg.ObrItrId);
                    values.Add("Lri_image", eyObrResImg.ObrImage);
                    values.Add("Lri_flag", eyObrResImg.ObrFlag);
                    values.Add("Lri_base64", eyObrResImg.ObrBase64);
                    //values.Add("obr_sn", eyObrResImg.ObrSn); //自增字段，不需要插入
                    values.Add("Lri_path", eyObrResImg.ObrPath);

                    saveInt = helper.InsertOperation("Lis_result_image", values);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("图像保存失败SaveObrResultImage", ex);
                    throw ex;
                }
            }
            return saveInt;
        }
    }

}
