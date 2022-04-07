using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using System.Data;
using dcl.svr.framedic;
using dcl.root.dac;
using dcl.root.logon;
using dcl.svr.dicbasic;
using dcl.pub.entities.dict;

namespace dcl.svr.wcf
{
    public class DictService : WCFServiceBase, IDictService
    {
        public System.Data.DataTable GetItemsWithSamRef(IEnumerable<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itr_id, string pat_depcode)
        {
            DataTable dt = new DictItemBLL().GetItemsWithSamRef(itemsID, sam_id, age_minutes, sex, sam_rem, itr_id, pat_depcode);
            return dt;
        }
        public DataTable GetCombineDefData()
        {

            try
            {
                string sql = @"
select com_id,isnull(itm_defa,'') as def_data,dict_item_sam.itm_itr_id from dict_combine_mi 
left join dict_item on dict_combine_mi.com_itm_id=dict_item.itm_id 
left join dict_item_sam on dict_item_sam.itm_id=dict_item.itm_id
where isnull(itm_defa,'')<>''
group by com_id,isnull(itm_defa,''),dict_item_sam.itm_itr_id,dict_combine_mi.com_sort
order by dict_combine_mi.com_sort

";

                DBHelper helper = new DBHelper();
                DataTable dt = helper.GetTable(sql);
                dt.TableName = "CombineDefData";

                foreach (DataRow item in dt.Rows)
                {
                    string defData = item["def_data"].ToString();


                    if (defData.StartsWith("[")
                        && defData.EndsWith("]"))
                    {
                        StringBuilder itemDefData = new StringBuilder();
                        string[] codeList = defData.Replace("[", "").Replace("]", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DataTable desInfo = helper.GetTable(@"select br_id,isnull(br_scripe,'') as br_scripe from dict_bscripe");
                        foreach (string code in codeList)
                        {
                            DataRow[] rows = desInfo.Select(string.Format("br_id='{0}'", code));
                            if (rows.Length > 0 && !string.IsNullOrEmpty(rows[0]["br_scripe"].ToString()))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0]["br_scripe"].ToString());

                            }

                        }
                        item["def_data"] = itemDefData.ToString();
                    }
                    else if (defData.StartsWith("^")
                        && defData.EndsWith("^"))
                    {

                        StringBuilder itemDefData = new StringBuilder();


                        string[] codeList = defData.Replace("^", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        DataTable desInfo = helper.GetTable(@"select nob_id,nob_cname from dict_nobact where nob_del<>'1'");
                        foreach (string code in codeList)
                        {
                            DataRow[] rows = desInfo.Select(string.Format("nob_id='{0}'", code));
                            if (rows.Length > 0 && !string.IsNullOrEmpty(rows[0]["nob_cname"].ToString()))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0]["nob_cname"].ToString());

                            }

                        }
                        item["def_data"] = itemDefData.ToString();
                    }
                }


                return dt;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetCombineDefData", ex.ToString());

                DataTable dt = new DataTable();
                dt.TableName = "CombineDefData";
                return dt;
            }
        }

        public DataTable GetCombineView()
        {
            try
            {
                string sql = @"
SELECT     dc.com_id AS 组合id, dc.com_name 组合名称, dc.com_his_name AS 收费名称, dcb.com_his_fee_code AS 收费代码, ISNULL(dcb.com_print_name, dc.com_name) AS 打印名称, 
                      dcb.com_split_code AS 拆分码, dbo.dict_type.type_name AS 物理组别, dbo.dict_cuvette.cuv_name AS 试管, dbo.dict_sample.sam_name AS 标本类别, 
                      dcb.com_split_seq AS 序号, dcb.com_blood_notice AS 采样注意事项, dcb.com_save_notice AS 保存注意事项, ori.ori_name AS 组合来源, 
                      dc.com_reptimes AS 出报告时间
FROM         dbo.dict_combine AS dc inner JOIN
                      dbo.dict_combine_bar AS dcb ON dc.com_id = dcb.com_id LEFT OUTER JOIN
                      dbo.dict_sample ON dbo.dict_sample.sam_id = dcb.com_sam_id LEFT OUTER JOIN
                      dbo.dict_type ON dcb.com_exec_code = dbo.dict_type.type_id LEFT OUTER JOIN
                      dbo.dict_cuvette ON dbo.dict_cuvette.cuv_code = dcb.com_cuv_code left join
                      dbo.dict_origin as ori  on ori.ori_id=dcb.com_ori_id
                      
WHERE     (dc.com_del <> 1)
";

                DBHelper helper = new DBHelper();
                DataTable dt = helper.GetTable(sql);
                dt.TableName = "GetCombineView";
                return dt;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetCombineView", ex.ToString());

                DataTable dt = new DataTable();
                dt.TableName = "GetCombineView";
                return dt;
            }

        }

        #region IDictService 成员

        public void ItemUrgentValue_Delete(dcl.pub.entities.dict.EntityDictItemUrgentValue obj)
        {
            new DictItemUrgentValueBIZ().Deletes(obj);
        }

        public void ItemUrgentValue_Save(List<EntityDictItemUrgentValue> obj)
        {
            new DictItemUrgentValueBIZ().Save(obj);
        }

        public List<dcl.pub.entities.dict.EntityDictItemUrgentValue> ItemUrgentValue_SelectAll()
        {
            return new DictItemUrgentValueBIZ().SelectAll();
        }

        #endregion

        #region IDictService 成员


        public void ReflashServerDictCache()
        {
            dcl.common.ConvertHelper.ReflashCache();
        }

        #endregion

        #region IDictService 成员


        public DataTable GetSysconfig()
        {
            Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
            string sqlSelect = "select * from sysconfig";

            DataTable table= helper.GetTable(sqlSelect);
            table.TableName = "sysconfig";
            return table;
        }

        /// <summary>
        /// 获取指定PatID的组合排序号
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetCombineSeqForPatID(string pat_id)
        {
            try
            {
                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                string sqlSelect = string.Format(@"select patients_mi.pat_id,patients_mi.pat_com_id,
isnull(dict_combine.com_seq,99999) as com_seq
from patients_mi
left join dict_combine on patients_mi.pat_com_id=dict_combine.com_id
where patients_mi.pat_id='{0}'
order by com_seq asc", pat_id);

                DataTable table = helper.GetTable(sqlSelect);
                table.TableName = "patients_seq";
                return table;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetCombineSeqForPatID", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 根据仪器ID获取其专业组的项目字典
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public DataTable GetItemDataForItrID(string itr_id)
        {
            try
            {
                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                string sqlSelect = string.Format(@"select itm_id,itm_name,itm_ecd,itm_py,itm_wb 
from dict_item with(nolock)
where itm_ptype=(select top 1 itr_ptype from dict_instrmt where itr_id='{0}')", itr_id);

                DataTable table = helper.GetTable(sqlSelect);
                table.TableName = "dtDictitem";
                return table;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetItemDataForItrID", ex.ToString());
                return null;
            }
        }

        #endregion
    }
}
