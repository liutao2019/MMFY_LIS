using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicInstrument))]
    public class DaoDicInstrument : IDaoDicInstrument
    {
        public bool Save(EntityDicInstrument instrmt)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itr_instrument");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ditr_id", id);

                values.Add("Ditr_ename", instrmt.ItrEname);
                values.Add("Ditr_client_no", instrmt.ItrClientNo);
                values.Add("Ditr_c_code", instrmt.CCode);
                values.Add("Ditr_lab_id", instrmt.ItrLabId);
                values.Add("Ditr_name", instrmt.ItrName);
                values.Add("Ditr_report_id", instrmt.ItrReportId);
                values.Add("Ditr_report_type", instrmt.ItrReportType);
                values.Add("Ditr_Dcom_id", instrmt.ItrComId);
                values.Add("Ditr_Dsam_id", instrmt.ItrSamId);
                values.Add("Ditr_Dpro_id", instrmt.ItrProId);

                values.Add("Ditr_com_Dpro_id", instrmt.ItrComProId);
                values.Add("Ditr_sub_title", instrmt.ItrSubTitle);
                values.Add("Ditr_title_mod", instrmt.ItrTitleMod);
                values.Add("Ditr_autocalu_flag", instrmt.ItrAutocaluFlag);
                values.Add("Ditr_comm_type", instrmt.ItrCommType);
                values.Add("Ditr_test_type", instrmt.ItrTestType);
                values.Add("Ditr_host_type", instrmt.ItrHostType);
                values.Add("del_flag", instrmt.DelFlag); //删除标志
                values.Add("py_code", instrmt.PyCode);
                values.Add("wb_code", instrmt.WbCode);

                values.Add("sort_no", instrmt.SortNo);
                values.Add("Ditr_report_address", instrmt.ItrReportAddress);
                values.Add("Ditr_qc_check", instrmt.ItrQcCheck);
                values.Add("Ditr_Dorg_id", instrmt.ItrOrgId);
                values.Add("Ditr_report_Ditr_id", instrmt.ItrReportItrId);
                values.Add("Ditr_micro_flag", instrmt.ItrMicroFlag);
                values.Add("Ditr_report_chk_id", instrmt.ItrReportChkId);
                values.Add("Ditr_manufacturers", instrmt.ItrManufacturers);
                values.Add("Ditr_factory_id", instrmt.ItrFactoryId);
                values.Add("Ditr_sepc", instrmt.ItrSepc);

                values.Add("Ditr_model", instrmt.ItrModel);
                values.Add("Ditr_producing_area", instrmt.ItrProducingArea);
                values.Add("Ditr_status", instrmt.ItrStatus);
                values.Add("Ditr_report_ins", instrmt.ItrReportIns);
                values.Add("Ditr_enable_date", instrmt.ItrEnableDate);
                values.Add("Ditr_contract_no", instrmt.ItrContractNo);
                values.Add("Ditr_purchase_date", instrmt.ItrPurchaseDate);
                values.Add("Ditr_price", instrmt.ItrPrice);
                values.Add("Ditr_supplier", instrmt.ItrSupplier);
                values.Add("Ditr_depreciation_date", instrmt.ItrDepreciationDate);

                values.Add("Ditr_depreciation_rate", instrmt.ItrDepreciationRate);
                values.Add("Ditr_brand", instrmt.ItrBrand);
                values.Add("Ditr_mic_type", instrmt.ItrMicType);
                values.Add("Ditr_image_flag", instrmt.ItrImageFlag);

                helper.InsertOperation("Dict_itr_instrument", values);

                instrmt.ItrId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicInstrument instrmt)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ditr_ename", instrmt.ItrEname);
                values.Add("Ditr_client_no", instrmt.ItrClientNo);
                values.Add("Ditr_c_code", instrmt.CCode);
                values.Add("Ditr_lab_id", instrmt.ItrLabId);
                values.Add("Ditr_name", instrmt.ItrName);
                values.Add("Ditr_report_id", instrmt.ItrReportId);
                values.Add("Ditr_report_type", instrmt.ItrReportType);
                values.Add("Ditr_Dcom_id", instrmt.ItrComId);
                values.Add("Ditr_Dsam_id", instrmt.ItrSamId);
                values.Add("Ditr_Dpro_id", instrmt.ItrProId);

                values.Add("Ditr_com_Dpro_id", instrmt.ItrComProId);
                values.Add("Ditr_sub_title", instrmt.ItrSubTitle);
                values.Add("Ditr_title_mod", instrmt.ItrTitleMod);
                values.Add("Ditr_autocalu_flag", instrmt.ItrAutocaluFlag);
                values.Add("Ditr_comm_type", instrmt.ItrCommType);
                values.Add("Ditr_test_type", instrmt.ItrTestType);
                values.Add("Ditr_host_type", instrmt.ItrHostType);
                values.Add("del_flag", instrmt.DelFlag); //删除标志
                values.Add("py_code", instrmt.PyCode);
                values.Add("wb_code", instrmt.WbCode);

                values.Add("sort_no", instrmt.SortNo);
                values.Add("Ditr_report_address", instrmt.ItrReportAddress);
                values.Add("Ditr_qc_check", instrmt.ItrQcCheck);
                values.Add("Ditr_Dorg_id", instrmt.ItrOrgId);
                values.Add("Ditr_report_Ditr_id", instrmt.ItrReportItrId);
                values.Add("Ditr_micro_flag", instrmt.ItrMicroFlag);
                values.Add("Ditr_report_chk_id", instrmt.ItrReportChkId);
                values.Add("Ditr_manufacturers", instrmt.ItrManufacturers);
                values.Add("Ditr_factory_id", instrmt.ItrFactoryId);
                values.Add("Ditr_sepc", instrmt.ItrSepc);

                values.Add("Ditr_model", instrmt.ItrModel);
                values.Add("Ditr_producing_area", instrmt.ItrProducingArea);
                values.Add("Ditr_status", instrmt.ItrStatus);
                values.Add("Ditr_report_ins", instrmt.ItrReportIns);
                values.Add("Ditr_enable_date", instrmt.ItrEnableDate);
                values.Add("Ditr_contract_no", instrmt.ItrContractNo);
                values.Add("Ditr_purchase_date", instrmt.ItrPurchaseDate);
                values.Add("Ditr_price", instrmt.ItrPrice);
                values.Add("Ditr_supplier", instrmt.ItrSupplier);
                values.Add("Ditr_depreciation_date", instrmt.ItrDepreciationDate);

                values.Add("Ditr_depreciation_rate", instrmt.ItrDepreciationRate);
                values.Add("Ditr_brand", instrmt.ItrBrand);
                values.Add("Ditr_mic_type", instrmt.ItrMicType);
                values.Add("Ditr_image_flag", instrmt.ItrImageFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ditr_id", instrmt.ItrId);

                helper.UpdateOperation("Dict_itr_instrument", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicInstrument instrumt)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ditr_id", instrumt.ItrId);

                helper.UpdateOperation("Dict_itr_instrument", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicInstrument> Search(Object obj)
        {
            try
            {
                String sql = @" SELECT  Dict_itr_instrument.*,0 sp_select,
Dict_itm_combine.Dcom_name, Dict_organize.Dorg_name,Dict_sample.Dsam_name,Dic_itr_instrument2.Ditr_ename itr_con_name,
dict_type_c.Dpro_name AS type_name_c,Dict_profession.Dpro_name,dict_type_s.Dpro_name AS type_name_s,
dict_report.rep_del,dict_report.rep_name
FROM  Dict_itr_instrument 
LEFT OUTER JOIN Dict_itm_combine ON Dict_itr_instrument.Ditr_Dcom_id = Dict_itm_combine.Dcom_id AND Dict_itm_combine.del_flag = '0' 
LEFT OUTER JOIN Dict_profession AS dict_type_s ON Dict_itr_instrument.Ditr_Dpro_id = dict_type_s.Dpro_id AND dict_type_s.del_flag = '0' 
LEFT OUTER JOIN Dict_profession AS dict_type_c ON Dict_itr_instrument.Ditr_Dpro_id = dict_type_c.Dpro_id AND dict_type_c.del_flag = '0' 
LEFT OUTER JOIN dict_report ON Dict_itr_instrument.Ditr_report_id = dict_report.rep_id AND dict_report.rep_del = '0' 
LEFT OUTER JOIN Dict_organize ON Dict_itr_instrument.Ditr_Dorg_id = Dict_organize.Dorg_id AND Dict_organize.del_flag = '0' 
LEFT OUTER JOIN Dict_profession ON Dict_itr_instrument.Ditr_lab_id = Dict_profession.Dpro_id AND Dict_profession.del_flag = '0' 
LEFT OUTER JOIN Dict_sample ON Dict_itr_instrument.Ditr_Dsam_id = Dict_sample.Dsam_id AND Dict_sample.del_flag = '0' 
LEFT OUTER JOIN Dict_itr_instrument Dic_itr_instrument2 on Dict_itr_instrument.Ditr_report_Ditr_id=Dic_itr_instrument2.Ditr_id and Dic_itr_instrument2.del_flag='0'
WHERE Dict_itr_instrument.del_flag = '0' and  Dict_itr_instrument.Ditr_id<>'-1' ";

                bool isTrue = obj is EntityDicQcInstrmtChannel;

                if (isTrue)
                {
                    sql += string.Format(@"and Dict_itr_instrument.Ditr_id in (
                                    select distinct Dmat_Ditr_id from Dict_qc_materia where Dmat_date_end > getdate()
                                    ) ");
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicInstrument>();
            }
        }
        public List<EntityDicInstrument> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicInstrument> list = new List<EntityDicInstrument>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicInstrument instrmt = new EntityDicInstrument();

                instrmt.ItrId = item["Ditr_id"].ToString();
                instrmt.ItrEname = item["Ditr_ename"].ToString();
                instrmt.ItrClientNo = item["Ditr_client_no"].ToString();
                instrmt.CCode = item["Ditr_c_code"].ToString();
                instrmt.ItrLabId = item["Ditr_lab_id"].ToString();
                instrmt.ItrName = item["Ditr_name"].ToString();
                instrmt.ItrReportId = item["Ditr_report_id"].ToString();
                instrmt.ItrReportType = item["Ditr_report_type"].ToString();

                instrmt.ItrComId = item["Ditr_Dcom_id"].ToString();
                instrmt.ItrSamId = item["Ditr_Dsam_id"].ToString();
                instrmt.ItrProId = item["Ditr_Dpro_id"].ToString();
                instrmt.ItrComProId = item["Ditr_com_Dpro_id"].ToString();
                instrmt.ItrSubTitle = item["Ditr_sub_title"].ToString();
                if (!(item["Ditr_title_mod"].ToString()??"").Equals(""))
                {
                    instrmt.ItrTitleMod = int.Parse(item["Ditr_title_mod"].ToString());
                }
                if (!(item["Ditr_autocalu_flag"].ToString()??"").Equals(""))
                {
                    instrmt.ItrAutocaluFlag = int.Parse(item["Ditr_autocalu_flag"].ToString());
                }
                if (!(item["Ditr_comm_type"].ToString()??"").Equals(""))
                {
                    instrmt.ItrCommType = int.Parse(item["Ditr_comm_type"].ToString());
                }

                instrmt.ItrTestType = item["Ditr_test_type"].ToString();
                if (!(item["Ditr_host_type"].ToString()??"").Equals(""))
                {
                    instrmt.ItrHostType = int.Parse(item["Ditr_host_type"].ToString());
                }

                instrmt.DelFlag = item["del_flag"].ToString(); //删除标志
                instrmt.PyCode = item["py_code"].ToString();
                instrmt.WbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                instrmt.SortNo = sort;   //序号

                instrmt.ItrReportAddress = item["Ditr_report_address"].ToString();
                if (!(item["Ditr_qc_check"].ToString()??"").Equals(""))
                {
                    instrmt.ItrQcCheck = int.Parse(item["Ditr_qc_check"].ToString());
                }

                instrmt.ItrOrgId = item["Ditr_Dorg_id"].ToString();
                instrmt.ItrReportItrId = item["Ditr_report_Ditr_id"].ToString();
                if (!(item["Ditr_micro_flag"].ToString()??"").Equals(""))
                {
                    instrmt.ItrMicroFlag = int.Parse(item["Ditr_micro_flag"].ToString());
                }

                instrmt.ItrReportChkId = item["Ditr_report_chk_id"].ToString();

                instrmt.ItrManufacturers = item["Ditr_manufacturers"].ToString();
                instrmt.ItrFactoryId = item["Ditr_factory_id"].ToString();
                instrmt.ItrSepc = item["Ditr_sepc"].ToString();

                instrmt.ItrModel = item["Ditr_model"].ToString();

                instrmt.ItrProducingArea = item["Ditr_producing_area"].ToString();
                instrmt.ItrStatus = item["Ditr_status"].ToString();

                if (!(item["Ditr_report_ins"].ToString()??"").Equals(""))
                {
                    instrmt.ItrReportIns = int.Parse(item["Ditr_report_ins"].ToString());
                }

                if (!(item["Ditr_enable_date"].ToString()??"").Equals(""))
                {
                    instrmt.ItrEnableDate = DateTime.Parse(item["Ditr_enable_date"].ToString());
                }
                instrmt.ItrContractNo = item["Ditr_contract_no"].ToString();
                if (!string.IsNullOrEmpty(item["Ditr_price"].ToString()))
                {
                    instrmt.ItrPrice = decimal.Parse(item["Ditr_price"].ToString());
                }

                instrmt.ItrSupplier = item["Ditr_supplier"].ToString();

                if (!(item["Ditr_depreciation_date"].ToString()??"").Equals(""))
                {
                    instrmt.ItrDepreciationDate = int.Parse(item["Ditr_depreciation_date"].ToString());
                }
                if (!(item["Ditr_depreciation_rate"].ToString()??"").Equals(""))
                {
                    instrmt.ItrDepreciationRate = decimal.Parse(item["Ditr_depreciation_rate"].ToString());
                }
                instrmt.ItrBrand = item["Ditr_brand"].ToString();
                //instrmt.ItrMicType = (Int32)item["itr_mic_type"];
                if (!(item["Ditr_mic_type"].ToString()??"").Equals(""))
                {
                    instrmt.ItrMicType = int.Parse(item["Ditr_mic_type"].ToString());
                }

                if (!(item["Ditr_image_flag"].ToString()??"").Equals(""))
                {
                    instrmt.ItrImageFlag = int.Parse(item["Ditr_image_flag"].ToString());
                }

                #region 附件字段赋值
                instrmt.ItrRepName = item["rep_name"].ToString();
                instrmt.ItrComName = item["Dcom_name"].ToString();
                instrmt.ItrSamName = item["Dsam_name"].ToString();
                instrmt.ItrOrgName = item["Dorg_name"].ToString();
                instrmt.ItrTypeNameC = item["type_name_c"].ToString();

                instrmt.ItrTypeName = item["Dpro_name"].ToString();
                instrmt.ItrTypeNameS = item["type_name_s"].ToString();
                instrmt.ItrRepDel = item["rep_del"].ToString();
                instrmt.ItrConName = item["itr_con_name"].ToString();
                instrmt.Checked = false;
                #endregion

                list.Add(instrmt);
            }
            return list.OrderBy(i => i.SortNo).ToList();
        }

        public List<EntityDicInstrument> GetInstrumentByComIds(List<string> ComIdList)
        {
            string ComIdListString = string.Empty;
            foreach (var ComId in ComIdList)
            {
                ComIdListString = string.Format(",'{0}'", ComId);
            }
            ComIdListString = ComIdListString.Remove(0, 1);
            try
            {
                string sql = string.Format(@"SELECT Dict_itr_instrument.Ditr_id,Dict_itr_instrument.Ditr_ename,Dict_itr_instrument.Ditr_client_no,Dict_itr_instrument.Ditr_c_code,
Dict_itr_instrument.Ditr_lab_id,Dict_itr_instrument.Ditr_name,Dict_itr_instrument.Ditr_report_id,Dict_itr_instrument.Ditr_report_type,Dict_itr_instrument.Ditr_Dcom_id,Dict_itr_instrument.Ditr_Dsam_id,
Dict_itr_instrument.Ditr_Dpro_id,Dict_itr_instrument.Ditr_com_Dpro_id,Dict_itr_instrument.Ditr_sub_title,Dict_itr_instrument.Ditr_title_mod,Dict_itr_instrument.Ditr_autocalu_flag,Dict_itr_instrument.Ditr_comm_type,
Dict_itr_instrument.Ditr_test_type,Dict_itr_instrument.Ditr_host_type,Dict_itr_instrument.Ditr_report_address,Dict_itr_instrument.Ditr_qc_check,
Dict_itr_instrument.Ditr_Dorg_id,Dict_itr_instrument.wb_code,Dict_itr_instrument.py_code,
Dict_itr_instrument.sort_no,Dict_itr_instrument.del_flag,Dict_itr_instrument.Ditr_micro_flag,Dict_itr_instrument.Ditr_report_Ditr_id
FROM Dict_itr_instrument
where Ditr_id in
(select
distinct id = (case Dict_itr_instrument.Ditr_report_Ditr_id when null then Dict_itr_instrument.Ditr_id
when '' then Dict_itr_instrument.Ditr_id
else Dict_itr_instrument.Ditr_report_Ditr_id end)
from Dict_itr_instrument
right OUTER JOIN Rel_itr_combine ON Dict_itr_instrument.Ditr_id = Rel_itr_combine.Ric_Ditr_id
WHERE Dict_itr_instrument.del_flag='0' and
Ric_Dcom_id IN ({0}))", ComIdListString);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicInstrument> list = EntityManager<EntityDicInstrument>.ConvertToList(dt).OrderBy(i => i.ItrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicInstrument>();
            }
        }

        public int GetItrHostFlag(string itr_id)
        {
            int result = 1;
            try
            {
                string sql = string.Format("select top 1 Ditr_comm_type from Dict_itr_instrument where Ditr_id = '{0}'", itr_id);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && !string.IsNullOrEmpty(dt.Rows[0]["Ditr_comm_type"].ToString()))
                {
                    result = Convert.ToInt32(dt.Rows[0]["Ditr_comm_type"]);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityDicInstrument> GetInstrumentByItridOrItrType(string itrId = "", string itrType = "")
        {
            List<EntityDicInstrument> listInstrmt = new List<EntityDicInstrument>();

            string strWhere = string.Empty;
            if (!string.IsNullOrEmpty(itrId))
            {
                strWhere += string.Format(" and Ditr_id = '{0}'", itrId);
            }
            if (!string.IsNullOrEmpty(itrType))
            {
                strWhere += string.Format(" and Ditr_lab_id = '{0}'", itrType);
            }
            if (strWhere.Length < 1)
            {
                string sql = string.Format("select Ditr_id,Ditr_report_type,Ditr_report_type from Dict_itr_instrument where 1=1 {0}", strWhere);
                try
                {
                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sql);
                    listInstrmt = EntityManager<EntityDicInstrument>.ConvertToList(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listInstrmt;
        }

        public List<EntityDicInstrument> GetHistoryReletedInstrumentByRepId(string repId)
        {
            List<EntityDicInstrument> listItr = new List<EntityDicInstrument>();
            if (!string.IsNullOrEmpty(repId))
            {
                try
                {
                    string sql = string.Format(@"select bins.Ditr_id from Pat_lis_main as pat,
Dict_itr_instrument as ains,Dict_itr_instrument as bins
where bins.Ditr_Dpro_id=ains.Ditr_Dpro_id
and ains.Ditr_id=pat.Pma_Ditr_id
and pat.Pma_rep_id='{0}'", repId);
                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sql);

                    listItr = EntityManager<EntityDicInstrument>.ConvertToList(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listItr;
        }

        public EntityDicInstrument GetInstrumentByItrid(string Itrid)
        {
            List<EntityDicInstrument> lst = new List<EntityDicInstrument>();
            EntityDicInstrument result = null;
            if (!string.IsNullOrEmpty(Itrid))
            {
                try
                {
                    string sql = string.Format(@"select * from Dict_itr_instrument
                    where Ditr_id = '{0}'", Itrid);
                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sql);
                    lst = EntityManager<EntityDicInstrument>.ConvertToList(dt);
                    if (lst.Count > 0)
                        result = lst[0];
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<EntityDicInstrument> GetInstrumentByBarcode(string Barcode)
        {
            List<EntityDicInstrument> result = null;
            string sql = string.Format( @"select  Dict_itr_instrument.*
from Dict_itr_instrument
left join Rel_itr_combine on Rel_itr_combine.Ric_Ditr_id = Dict_itr_instrument.Ditr_id
left join Sample_detail on Sample_detail.Sdet_com_id = Rel_itr_combine.Ric_Dcom_id
where Sample_detail.Sdet_bar_code='{0}'",Barcode);
            try
            {
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                result = EntityManager<EntityDicInstrument>.ConvertToList(dt);
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
