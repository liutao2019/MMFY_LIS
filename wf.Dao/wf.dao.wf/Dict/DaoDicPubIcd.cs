using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Data;
using dcl.dao.core;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(interfaces.IDaoDic<EntityDicPubIcd>))]
    public class DaoDicPubIcd : IDaoDic<EntityDicPubIcd>
    {
        /// <summary>
        /// 把数据库表转成实体
        /// </summary>
        /// <param name="dtSour"></param>
        /// <returns></returns>
        public List<EntityDicPubIcd> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubIcd> list = new List<EntityDicPubIcd>();
            foreach(DataRow item in dtSour.Rows)
            {
                EntityDicPubIcd diagnos = new EntityDicPubIcd();

                diagnos.IcdId = item["Dicd_id"].ToString();
                diagnos.IcdName = item["Dicd_name"].ToString();
                diagnos.IcdGbCode = item["Dicd_gb_code"].ToString();
                diagnos.IcdCCode = item["Dicd_c_code"].ToString();
                diagnos.IcdPyCode = item["py_code"].ToString();
                diagnos.IcdWbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                diagnos.IcdSortNo = sort;
                diagnos.IcdContent = item["Dicd_content"].ToString();
                diagnos.IcdDelFlag = item["del_flag"].ToString();
                diagnos.Checked = false ;

                list.Add(diagnos);
            }
            return list.OrderBy(i => i.IcdSortNo).ToList();
        }

        public bool Delete(EntityDicPubIcd diagnos)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dicd_id", diagnos.IcdId);

                helper.UpdateOperation("Dict_icd", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicPubIcd diagnos)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_icd");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dicd_id", id);
                values.Add("Dicd_name", diagnos.IcdName);
                values.Add("Dicd_gb_code", diagnos.IcdGbCode);
                values.Add("Dicd_c_code", diagnos.IcdCCode);
                values.Add("py_code", diagnos.IcdPyCode);
                values.Add("wb_code", diagnos.IcdWbCode);
                values.Add("sort_no", diagnos.IcdSortNo);
                values.Add("del_flag", diagnos.IcdDelFlag);
                values.Add("Dicd_content", diagnos.IcdContent);

                helper.InsertOperation("Dict_icd", values);

                diagnos.IcdId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubIcd> Search(object obj)
        {
            try
            {
                String sql = @"SELECT *,0 sp_select FROM Dict_icd where del_flag =0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubIcd>();
            }
        }

        public bool Update(EntityDicPubIcd diagnos)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dicd_name", diagnos.IcdName);
                values.Add("Dicd_gb_code", diagnos.IcdGbCode);
                values.Add("Dicd_c_code", diagnos.IcdCCode);
                values.Add("py_code", diagnos.IcdPyCode);
                values.Add("wb_code", diagnos.IcdWbCode);
                values.Add("sort_no", diagnos.IcdSortNo);
                values.Add("del_flag", diagnos.IcdDelFlag);
                values.Add("Dicd_content", diagnos.IcdContent);
                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dicd_id", diagnos.IcdId);

                helper.UpdateOperation("Dict_icd", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
