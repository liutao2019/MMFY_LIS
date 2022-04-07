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
    /// <summary>
    /// 标本信息扩展表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoSampOperateDetail))]
    public class DaoSampOperateDetail : IDaoSampOperateDetail
    {
        public List<EntitySampOperateDetail> GetBarCodeExtend(string barCode)
        {
            List<EntitySampOperateDetail> listSampOperDetail = new List<EntitySampOperateDetail>();

            if (!string.IsNullOrEmpty(barCode))
            {
                try
                {
                    string sqlSelect = string.Format(@"select *
                                                       from Sample_operate_detail
                                                         where Soper_Sma_bar_id = '{0}' ", barCode);

                    DBManager helper = new DBManager();
                    DataTable dtSampOperDetail = helper.ExecuteDtSql(sqlSelect);

                    listSampOperDetail = EntityManager<EntitySampOperateDetail>.ConvertToList(dtSampOperDetail).OrderBy(i => i.SampBarCode).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return listSampOperDetail;
        }

        public List<EntitySampImage> GetSampImage(string barCode)
        {
            List<EntitySampImage> listSampImage = new List<EntitySampImage>();

            if (!string.IsNullOrEmpty(barCode))
            {
                try
                {
                    string sqlSelect = string.Format(@"select *
                                                       from Sample_image
                                                         where Sima_Smain_bar_id = '{0}' ", barCode);

                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sqlSelect);

                    listSampImage = EntityManager<EntitySampImage>.ConvertToList(dt).OrderBy(i => i.SampBarCode).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            } 

            return listSampImage;
        }
    }
}
