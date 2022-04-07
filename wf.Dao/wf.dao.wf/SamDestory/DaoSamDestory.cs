using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using dcl.common;
using dcl.dao.core;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSamDestory))]
    public class DaoSamDestory : IDaoSamDestory
    {
        public int DestoryRackSam(List<string> barcodeList, string strSsid, string rackID, string operatorName,
                                           string operatorID, string opPlace,
                                           string iecID, string cupID)
        {
            int intRet = -1;
            try
            {
                DBManager helper = new DBManager();
                foreach (string barcode in barcodeList)
                {
                    string strSql = string.Format(@"update Sample_store_detail
                                set Ssdt_status = 20
                                where Ssdt_Ssr_id = '{0}' and Ssdt_bar_code='{1}' and Ssdt_status<>20   ",
                               strSsid, barcode);
                    intRet = helper.ExecCommand(strSql);
                    if (intRet == 1)
                    {
                        #region 获取服务器时间 dtNow
                        DateTime dtNow = DateTime.Now;
                        try
                        {
                            string strSQLSDT = @"SELECT GETDATE() ";
                            object objServiceTime = helper.SelOne(strSQLSDT, new List<DbParameter>());
                            dtNow = Convert.ToDateTime(Convert.ToDateTime(objServiceTime).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception ex)
                        {
                            Lib.LogManager.Logger.LogException(ex);
                        }
                        #endregion

                        EntitySampProcessDetail SampProcessDetial = new EntitySampProcessDetail();
                        SampProcessDetial.ProcDate = dtNow;
                        SampProcessDetial.ProcUsercode = operatorID;
                        SampProcessDetial.ProcUsername = operatorName;
                        SampProcessDetial.ProcStatus = "130";
                        SampProcessDetial.ProcBarno = barcode;
                        SampProcessDetial.ProcBarcode = barcode;
                        SampProcessDetial.ProcPlace = opPlace;
                        SampProcessDetial.ProcContent = "";
                        //调用bc_sign数据插入的方法
                        //还未迁入med的DaoSampProcessDetail文件,先注释
                        //bool IsExists = new DaoSampProcessDetail().SaveSampProcessDetail(SampProcessDetial);
                    }
                }

                string sql = string.Format(@"select ISNULL(count(1),0)  
                                              from Sample_store_detail  
                                                where  Ssdt_Ssr_id = @strSsid and Ssdt_status<>20  ", strSsid);
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@strSsid", strSsid));
                object obj = helper.SelOne(sql, paramHt);

                if (Convert.ToInt32(obj) == 0)
                {
                    string strSql = string.Format(@"update Sample_store_rack
                                set Ssr_status= 20
                                where Ssr_id = '{0}'  and Ssr_status<>20  ", strSsid);

                    intRet = helper.ExecCommand(strSql);

                    sql = @"Select ISNULL(count(1),0) from Sample_store_rack where Ssr_Drack_id=@sr_rack_id and Ssr_status<>20  ";
                    List<DbParameter> paramHt2 = new List<DbParameter>();
                    paramHt2.Add(new SqlParameter("@sr_rack_id", rackID));
                    obj = helper.SelOne(sql, paramHt2);

                    if (Convert.ToInt32(obj) == 0)
                    {
                        EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                        entityRack.RackId = rackID;
                        entityRack.RackStatus = 20;
                        ModifyRackStatus(entityRack);
                    }
                }
                else
                {
                    string strSql = string.Format(@"update Sample_store_rack  set Ssr_amount= {0}
                                where Ssr_id = '{1}'  and Ssr_status<>20 ", Convert.ToInt32(obj), strSsid);

                    intRet = helper.ExecCommand(strSql);
                }
                if (!string.IsNullOrEmpty(iecID))
                {
                    sql = string.Format(@"SELECT ISNULL(count(1),0) FROM Sample_store_rack ssr 
                                             WHERE ssr.Ssr_Dstore_id=@sr_store_id  AND ssr.Ssr_status<>20  ");
                    List<DbParameter> paramHt3 = new List<DbParameter>();
                    paramHt3.Add(new SqlParameter("@sr_store_id", iecID));
                    obj = helper.SelOne(sql, paramHt3);
                    if (Convert.ToInt32(obj) == 0)
                    {
                        UpdateIceBoxStatus(iecID, 0);
                    }
                }

                if (!string.IsNullOrEmpty(iecID) && !string.IsNullOrEmpty(cupID))
                {
                    sql = string.Format(@"SELECT ISNULL(count(1),0) FROM Sample_store_rack ssr 
                                  WHERE ssr.Ssr_Dstore_id=@sr_store_id AND ssr.Ssr_Dpos_id=@sr_place AND ssr.Ssr_status<>20  ");
                    List<DbParameter> paramHt4 = new List<DbParameter>();
                    paramHt4.Add(new SqlParameter("@sr_store_id", iecID));
                    paramHt4.Add(new SqlParameter("@sr_place", cupID));

                    obj = helper.SelOne(sql, paramHt4);
                    if (Convert.ToInt32(obj) == 0)
                    {
                        UpdateCupStatus(cupID, 0);
                    }
                }

                return intRet;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 DestoryRackSam", ex);
                return intRet;
            }
        }

        public bool CanRollBackDestory(string strSsid, string rackID)
        {
            try
            {
                DBManager helper = new DBManager();

                string sql = @"Select ISNULL(count(1),0) from Sample_store_rack 
                                    where Ssr_Drack_id=@sr_rack_id and Ssr_status<>20 and Ssr_id<>@sr_id 
                                      and isnull(Ssr_amount,0)>0  ";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@sr_rack_id", rackID));
                paramHt.Add(new SqlParameter("@sr_id", strSsid));
                object obj = helper.SelOne(sql, paramHt);
                return Convert.ToInt32(obj) == 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 CanRollBackDestory", ex);
            }
            return false;
        }

        public int RollBackDestory(List<string> barcodeList, string strSsid, string rackID)
        {
            int intRet = -1;
            try
            {
                DBManager helper = new DBManager();
                foreach (string barcode in barcodeList)
                {
                    string strSql = string.Format(@"update Sample_store_detail
                                set Ssdt_status = 10
                                where Ssdt_Ssr_id = '{0}' and Ssdt_bar_code='{1}' 
                                 and Ssdt_status=20  ", strSsid, barcode);

                    intRet = helper.ExecCommand(strSql);
                    if (intRet == 1)
                    {
                        DeleteBcSign(barcode, "130");
                    }
                }

                string sql = string.Format(@"select ISNULL(count(1),0)  from Sample_store_detail 
                                where  Ssdt_Ssr_id = @det_id and Ssdt_status<>20  ");
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@det_id", strSsid));

                object obj = helper.SelOne(sql, paramHt);

                if (Convert.ToInt32(obj) > 0)
                {
                    //更新孔数量
                    string strSql2 = string.Format(@"update Sample_store_rack  set  Ssr_amount={0}
                                             where Ssr_id = '{1}' ", Convert.ToInt32(obj), strSsid);

                    intRet = helper.ExecCommand(strSql2);

                    //回写状态
                    string strSql = string.Format(@"update Sample_store_rack
                                set Ssr_status= 10 
                                where Ssr_id = '{0}'  and Ssr_status=20 ", strSsid);

                    intRet = helper.ExecCommand(strSql);

                    sql = string.Format(@"delete from Sample_store_rack where Ssr_id <> '{0}' 
                                       and Ssr_status<>20 and Ssr_Drack_id='{1}' ", strSsid, rackID);

                    intRet = helper.ExecCommand(sql);

                    EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                    entityRack.RackId = rackID;
                    entityRack.RackStatus = 10;

                    ModifyRackStatus(entityRack);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 RollBackDestory", ex);
            }
            return intRet;
        }


        public string GetUserConfig(string confCode, string userID)
        {
            string sqlSelect = string.Format("select top 1 configItemValue from sysconfig where configType = '{0}' and configCode='{1}'", userID, confCode);
            DBManager helper = new DBManager();

            DataTable objValue = helper.ExecSel(sqlSelect);
            if (objValue == null || objValue.Rows.Count <= 0)
                return string.Empty;

            return objValue.Rows[0]["configItemValue"].ToString();
        }


        public List<EntitySampStoreRack> GetRackDataForDestory(DateTime? dateTimeFrom, DateTime? dateTimeTo, string rackCtype,
                                               string iceID, string cupID,
                                               string rackID, string barcode)
        {
            try
            {
                DBManager helper = new DBManager();

                //samp_store_rackDetail表需要替換
                string strSql = string.Format(@"select distinct 0 as isselected,Sample_store_rack.Ssr_id,
Sample_store_rack.Ssr_amount,Dict_sample_tube_rack.Drack_id,
Dict_sample_tube_rack.Drack_name,Dict_sample_tube_rack.Drack_Dtrack_code,
Dict_sample_tube_rack.Drack_code,Dict_sample_tube_rack.Drack_Dpro_id,
Sample_store_rack.Ssr_status,Dict_sample_tube_rack.Drack_barcode,                                     
Dict_tube_rack.Dtrack_name rack_name_new,Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_sample_store_position.Dpos_name,
Dict_sample_store.Dstore_name,
Dict_profession.Dpro_name,Sample_store_rack.Ssr_Dpos_id,Sample_store_rack.Ssr_Dstore_id,Sample_store_rack.Ssr_audit_date 
from Sample_store_rack  
inner join Dict_sample_tube_rack on Dict_sample_tube_rack.Drack_id=Sample_store_rack.Ssr_Drack_id

inner join Sample_store_detail on Sample_store_detail.Ssdt_Ssr_id=Sample_store_rack.Ssr_id
inner join Dict_tube_rack on Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id
left join Dict_sample_store on  Dict_sample_store.Dstore_id = Sample_store_rack.Ssr_Dstore_id
left join Dict_sample_store_position on  Dict_sample_store_position.Dpos_id = Sample_store_rack.Ssr_Dpos_id
where  Sample_store_rack.Ssr_status>5");

                if (dateTimeFrom.HasValue && dateTimeTo.HasValue)
                {
                    strSql += " and Sample_store_detail.Ssdt_date between @dateTimeFrom and @dateTimeTo  ";
                }
                else
                {
                    string str = GetUserConfig("Sam_SaveToDestoryDaysTip", "system");
                    if (string.IsNullOrEmpty(str))
                        str = "7";
                    strSql += string.Format(" and Sample_store_rack.Ssr_status <>20 and Sample_store_rack.Ssr_audit_date is not null and datediff(day,Sample_store_rack.Ssr_audit_date,getdate())>{0} ", str);
                }
                if (!string.IsNullOrEmpty(rackCtype))
                {
                    strSql += " and Dict_sample_tube_rack.Drack_Dpro_id = @rackCtype ";
                }
                if (!string.IsNullOrEmpty(iceID))
                {
                    strSql += " and Sample_store_rack.Ssr_Dstore_id = @iceID ";
                }
                if (!string.IsNullOrEmpty(cupID))
                {
                    strSql += " and Sample_store_rack.Ssr_Dpos_id = @cupID ";
                }
                if (!string.IsNullOrEmpty(rackID))
                {
                    strSql += " and Dict_sample_tube_rack.Drack_barcode like @rackID ";
                }
                if (!string.IsNullOrEmpty(barcode))
                {
                    strSql += " and Sample_store_detail.Ssdt_bar_code like @barcode ";
                }

                List<DbParameter> paramHt = new List<DbParameter>();

                if (dateTimeFrom.HasValue && dateTimeTo.HasValue)
                {
                    paramHt.Add(new SqlParameter("@dateTimeFrom", dateTimeFrom.Value.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                    paramHt.Add(new SqlParameter("@dateTimeTo", dateTimeTo.Value.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")));
                }

                if (!string.IsNullOrEmpty(rackCtype))
                {
                    paramHt.Add(new SqlParameter("@rackCtype", rackCtype));
                }
                if (!string.IsNullOrEmpty(iceID))
                {
                    paramHt.Add(new SqlParameter("@iceID", iceID));
                }
                if (!string.IsNullOrEmpty(cupID))
                {
                    paramHt.Add(new SqlParameter("@cupID", cupID));
                }
                if (!string.IsNullOrEmpty(rackID))
                {
                    paramHt.Add(new SqlParameter("@rackID", rackID));
                }
                if (!string.IsNullOrEmpty(barcode))
                {
                    paramHt.Add(new SqlParameter("@barcode", barcode));
                }

                DataTable dtSampStoreRack = helper.ExecuteDtSql(strSql, paramHt);

                List<EntitySampStoreRack> listSampStoreRack = EntityManager<EntitySampStoreRack>.ConvertToList(dtSampStoreRack).OrderBy(i => i.SrId).ToList();

                return listSampStoreRack;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetRackDataForDestory", ex);
                return new List<EntitySampStoreRack>();
            }
        }

        public List<EntitySampStoreDetail> GetRackDetailForDestory(string strSsid)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql = string.Format(@"select distinct Sample_store_detail.*,                                
Pat_lis_main.Pma_bar_code,                                   
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_pat_in_no,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '男*' end) Pma_pat_sex,
(case isnull(dbo.getAge(Pat_lis_main.Pma_pat_age_exp),'') when '' then '20*' else dbo.getAge(Pat_lis_main.Pma_pat_age_exp) end ) pid_age,
Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_pat_dept_name,
Pat_lis_main.Pma_Dsam_id,
Sample_store_rack.Ssr_Dstore_id,
Sample_store_rack.Ssr_Dpos_id,
Sample_store_rack.Ssr_status,
'1' as isselected
from   Sample_store_detail
left join Pat_lis_main on  Pat_lis_main.Pma_bar_code = Sample_store_detail.Ssdt_bar_code
left join Sample_store_rack on Sample_store_rack.Ssr_id = Sample_store_detail.Ssdt_Ssr_id
where  Sample_store_detail.Ssdt_Ssr_id = '{0}'  
and isnull(Sample_store_detail.Ssdt_bar_code,'')<>''   --预防空值导致与Pat_lis_main.Pma_bar_code关联之后查询很慢 
                           ", strSsid);

                DataTable dtSampStoreDetail = helper.ExecuteDtSql(strSql);

                List<EntitySampStoreDetail> listSampStoreRack = EntityManager<EntitySampStoreDetail>.ConvertToList(dtSampStoreDetail).OrderBy(i => i.DetId).ToList();

                return listSampStoreRack;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 GetRackDetailForDestory", ex);
                return new List<EntitySampStoreDetail>();
            }
        }

        /// <summary>
        /// 修改试管架的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyRackStatus(EntityDicSampTubeRack entity)
        {
            DBManager helper = new DBManager();
            int intRet = -1;
            string strSql = string.Format(@"update Dict_sample_tube_rack
                                set
                                    Drack_status = {0}
                                where Drack_id = '{1}' ", entity.RackStatus, entity.RackId);

            try
            {
                intRet = helper.ExecCommand(strSql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 ModifyRackStatus", ex);
            }
            return intRet;
        }

        /// <summary>
        /// 更新冰箱状态
        /// </summary>
        /// <param name="ice_id"></param>
        /// <param name="stautscode"></param>
        /// <returns></returns>
        public int UpdateIceBoxStatus(string ice_id, int stautscode)
        {
            DBManager helper = new DBManager();
            int intRet = -1;
            string strSql = string.Format(@"update Dict_sample_store
                                set
	                                Dstore_Dstau_id = {0}
                                where Dstore_id = '{1}' ", stautscode, ice_id);
            try
            {
                intRet = helper.ExecCommand(strSql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 UpdateIceBoxStatus", ex);
            }
            return intRet;
        }

        /// <summary>
        /// 更新柜子状态
        /// </summary>
        /// <param name="cup_id"></param>
        /// <param name="statuscode"></param>
        /// <returns></returns>
        public int UpdateCupStatus(string cup_id, int statuscode)
        {
            DBManager helper = new DBManager();
            int intRet = -1;
            string strSql = string.Format(@"update Dict_sample_store_position
                                set
	                                Dpos_status = {0}
	                               
                                where Dpos_id = '{1}' ", statuscode, cup_id);
            try
            {
                intRet = helper.ExecCommand(strSql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 UpdateCupStatus", ex);
            }
            return intRet;
        }

        /// <summary>
        /// 删除标本流转明细数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="bcStatus"></param>
        /// <returns></returns>
        public int DeleteBcSign(string barCode, string bcStatus)
        {
            int intRet = -1;
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.DeleteSampProcessDetail(barCode, bcStatus);
                if (IsExists)
                {
                    intRet = 1;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本管理 DeleteBcSign", ex);
            }
            return intRet;
        }

    }
}
