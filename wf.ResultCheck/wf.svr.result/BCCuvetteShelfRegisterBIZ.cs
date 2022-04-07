using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.root.dac;
using System.Data.SqlClient;
using System.Data;
using dcl.pub.entities;
using lis.dto.Entity;
using lis.dto;
using System.Diagnostics;
using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.result
{
    /// <summary>
    /// 试管条码病人登记
    /// </summary>
    public class BCCuvetteShelfRegisterBIZ
    {
        public System.Data.DataTable GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo)
        {
            string sqlSelect = string.Format(@"
select
bc_stand_temp.st_id,--bc_stand_temp的id
bc_stand_temp.st_type,--接收科室
bc_stand_temp.st_bar_code,--条码号
bc_stand_temp.st_place_x,
bc_stand_temp.st_place_y,
bc_stand_temp.st_date,--登记日期
bc_stand_temp.st_no,--序号
bc_stand_temp.st_etagere,--架子号
bc_patients.bc_name,--病人姓名
bc_patients.bc_in_no,--住院号
bc_patients.bc_upid,--唯一号
--bc_cname.bc_his_name,--检查项目
dict_combine.com_name as bc_his_name,
bc_cname.bc_lis_code,--检验组合lis中的id
bc_patients.bc_sex,--性别
bc_patients.bc_age,--年龄
bc_patients.bc_d_name,--科室
bc_patients.bc_bed_no,--床号
bc_patients.bc_diag,--临床诊断
bc_patients.bc_doct_name,--开单医生
bc_patients.bc_sam_id,
bc_patients.bc_sam_name,
bc_cname.bc_occ_date,--执行日期
bc_patients.bc_times,--住院次数
bc_cname.bc_id,
bc_cname.bc_flag,
bc_patients.bc_status

from bc_stand_temp
inner join bc_patients on bc_patients.bc_bar_code = bc_stand_temp.st_bar_code
left join bc_cname on bc_cname.bc_bar_code = bc_stand_temp.st_bar_code
left join dict_combine on dict_combine.com_id = bc_cname.bc_lis_code
where bc_cname.bc_del='0' and bc_stand_temp.st_type = '{0}' and bc_stand_temp.st_date >= @regDateFrom and bc_stand_temp.st_date < @regDateTo 
", receviceDeptID);

            if (shelfNoFrom != null)
            {
                string sqlPart = string.Format(" and bc_stand_temp.st_etagere >= {0} ", shelfNoFrom.Value);
                sqlSelect = sqlSelect + sqlPart;
            }

            if (shelfNoTo != null)
            {
                string sqlPart = string.Format(" and bc_stand_temp.st_etagere <= {0} ", shelfNoTo.Value);
                sqlSelect = sqlSelect + sqlPart;
            }


            if (seqFrom != null)
            {
                string sqlPart = string.Format(" and bc_stand_temp.st_no >= {0} ", seqFrom.Value);
                sqlSelect = sqlSelect + sqlPart;
            }

            if (seqTo != null)
            {
                string sqlPart = string.Format(" and bc_stand_temp.st_no <= {0} ", seqTo.Value);
                sqlSelect = sqlSelect + sqlPart;
            }


            sqlSelect = sqlSelect + " order by bc_stand_temp.st_no asc";

            DBHelper helper = new DBHelper();

            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.AddWithValue("regDateFrom", regDateFrom.Date);
            cmd.Parameters.AddWithValue("regDateTo", regDateTo.AddDays(1).Date);

            DataTable dt = helper.GetTable(cmd);

            dt.TableName = "GetCuvetteShelfInfo";

            return dt;
        }

        public List<EntityOperationResult> SavePatients(EntityRemoteCallClientInfo caller, List<EntityShelfSampleToPatients> listEntity)
        {
            List<EntityOperationResult> ret = new List<EntityOperationResult>();

            PatientEnterService bllPatients = new PatientEnterService();
            PatInsertBLL bllPatientInsert = new PatInsertBLL();

            foreach (EntityShelfSampleToPatients entity in listEntity)
            {


                string barcode = entity.bc_bar_code;
                DataSet ds = bllPatients.GetPatientsByBarCode(barcode);


                using (DBHelper transHelper = DBHelper.BeginTransaction())
                {
                    DataTable dtPatient = ds.Tables[PatientTable.PatientInfoTableName];

                    if (dtPatient != null)
                    {
                        dtPatient.Rows[0]["pat_itr_id"] = entity.itr_id;
                        dtPatient.Rows[0]["itr_name"] = entity.itr_name;
                        dtPatient.Rows[0]["pat_sid"] = entity.pat_sid;
                        //dtPatient.Rows[0]["pat_dep_id"] = entity.type_id;
                        dtPatient.Rows[0]["pat_date"] = entity.pat_date;
                        dtPatient.Rows[0]["pat_jy_date"] = entity.pat_jy_date;
                        //dtPatient.Rows[0]["pat_emp_id"] = ds.Tables[0].Rows[0]["bc_emp_id"];
                        dtPatient.Rows[0]["pat_host_order"] = entity.pat_host_order;
                        dtPatient.Rows[0]["pat_i_code"] = entity.pat_i_code;

                        DataTable dtCombine = ds.Tables[PatientTable.PatientCombineTableName];

                        if (dtCombine != null)
                        {
                            for (int i = dtCombine.Rows.Count - 1; i >= 0; i--)
                            {
                                string pat_com_id = dtCombine.Rows[i]["pat_com_id"].ToString();

                                if (entity.com_ids.Find(item => item == pat_com_id) == null)
                                {
                                    dtCombine.Rows.RemoveAt(i);
                                }
                            }
                        }

                        EntityOperationResult opresult = bllPatientInsert.InsertBarCodePatient(caller, ds, null);// helper);
                        try
                        {
                            if (opresult.Success)
                            {
                                string sqlUpdateBCCname = "update bc_cname set bc_flag = 1 where bc_id = {0} ";


                                //DBHelper helper2 = new DBHelper();
                                foreach (string bc_id in entity.bc_cname_ids)
                                {
                                    string sqlUpdate = string.Format(sqlUpdateBCCname, bc_id);
                                    //helper.ExecuteNonQuery(sqlUpdate);

                                    //把插入数据的事务与update bc_bcname分开防止死锁
                                    transHelper.ExecuteNonQuery(sqlUpdate);
                                }

                                if (opresult.Success)
                                {
                                    transHelper.Commit();
                                }
                                ret.Add(opresult);
                            }
                            else
                            {
                                ret.Add(opresult);
                            }
                        }
                        catch (Exception ex)
                        {
                            opresult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                        }
                    }
                }
            }

            return ret;
        }

        //public System.Data.DataTable GetInstrmtCom(string itr_id)
        //{
        //    string sql = "select * from dbo.dict_instrmt_com where itr_id='" + itr_id + "'";

        //    DBHelper helper = new DBHelper();

        //    DataTable dt = helper.GetTable(sql);

        //    dt.TableName = "dict_instrmt_com";

        //    return dt;
        //}


    }
}
