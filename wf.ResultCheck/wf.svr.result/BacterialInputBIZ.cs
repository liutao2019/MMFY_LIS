using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.svr.root.com;
using dcl.root.dto;
using System.Collections;
using lis.dto;
using System.Data.OleDb;
using System.Data.SqlClient;
using dcl.root.dac;
using dcl.common;
using dcl.pub.entities;
using dcl.svr.resultcheck.Updater;
using Lib.DAC;
using dcl.svr.cache;
using dcl.svr.resultcheck;
using System.Threading;
using dcl.entity;

namespace dcl.svr.result
{
    public class BacterialInputBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBiz成员
        public DataSet doDel(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtDict_an_sstd = ds.Tables["dict_an_sstd"];
                int id = dao.GetID("dict_an_sstd", dtDict_an_sstd.Rows.Count);
                foreach (DataRow dr in dtDict_an_sstd.Rows)
                {
                    dr["ss_flag"] = id - dtDict_an_sstd.Rows.Count + 1;
                    id++;
                }
                ArrayList arr = dao.GetInsertSql(dtDict_an_sstd);
                dao.DoTran(arr);
                result.Tables.Add(dtDict_an_sstd.Copy());
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("新增信息出错!", ex.ToString())); ;
            }

            return result;
        }

        public DataSet doNew(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPatients = ds.Tables["patients"];
                DataTable dtPatients_mi = ds.Tables["patients_mi"];
                DataTable dict_btype = ds.Tables["ba_rlts"];
                DataTable dtAntibio = ds.Tables["an_rlts"];
                DataTable dtCs_res = ds.Tables["cs_rlts"];
                //DataTable dtResulto = ds.Tables["resulto"];
                DataRow drPat = dtPatients.Rows[0];
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                string tiems = now.ToString("HH:mm:ss");
                //DateTime pat_date = DateTime.Now;//新增时:时间设为服务器时间(以服务器时间为准)

                DateTime pat_date = DateTime.Parse(drPat["pat_date"].ToString());
                drPat["pat_date"] = pat_date.ToString("yyyy-MM-dd") + " " + tiems;
                String pat_id = drPat["pat_itr_id"].ToString() + String.Format("{0:yyyyMMdd}", pat_date)
                   + drPat["pat_sid"].ToString();
                drPat["pat_id"] = pat_id;
                drPat["pat_flag"] = "0";

                StringBuilder sbcCode = new StringBuilder();
                bool needCmma = false;//是否添加逗号
                foreach (DataRow dr in dtPatients_mi.Rows)
                {
                    if (needCmma)
                        sbcCode.Append(",");

                    if (!Compare.IsEmpty(dr["pat_com_id"]))
                        sbcCode.Append(string.Format("'{0}'", dr["pat_com_id"].ToString()));

                    dr["pat_id"] = pat_id;

                    needCmma = true;
                }

                if (dtPatients.Columns.Contains("pat_key"))
                    dtPatients.Columns.Remove("pat_key");

                if (dtPatients_mi.Columns.Contains("pat_key"))
                {
                    dtPatients_mi.Columns.Remove("pat_key");
                    dtPatients_mi.AcceptChanges();
                }


                //时间计算方式
                string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
                if (Lab_BarcodeTimeCal == "计算签收时间")
                {
                    if (dtPatients.Rows.Count > 0)
                    {
                        if (dtPatients.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatients.Rows[0]["pat_apply_date"] == null)
                        {
                            dtPatients.Rows[0]["pat_apply_date"] = dtPatients.Rows[0]["pat_date"];
                        }
                        dtPatients.Rows[0]["pat_jy_date"] = dtPatients.Rows[0]["pat_apply_date"];
                    }
                }

                //dtPatients.PrimaryKey = new DataColumn[] { dtPatients.Columns["pat_key"] };
                ArrayList arr = dao.GetInsertSql(dtPatients);

                if (!Compare.IsEmpty(drPat["pat_bar_code"]))
                {
                    string pat_bar_code = drPat["pat_bar_code"].ToString();
                    if (sbcCode.Length > 0)
                    {
                        //允许细菌录入的条码能多次录入
                        arr.Add(string.Format("update bc_cname set bc_flag='1' where bc_lis_code in({0}) and bc_bar_code='{1}'", sbcCode.ToString(), drPat["pat_bar_code"].ToString()));

                    }

                    string pat_c_name = ds.Tables[1].Rows[0]["pat_c_name"].ToString();
                    string sid = ds.Tables[1].Rows[0]["pat_sid"].ToString();
                    string sqlBcSign = string.Format(@"insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status,bc_remark)
                                                      values('{0}','{1}','{2}','{3}','{4}','20','{5}')", pat_bar_code, pat_bar_code, now.ToString("yyyy-MM-dd HH:mm:ss"), string.Empty, string.Empty,
                                                                                                     string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", ds.Tables[1].Rows[0]["pat_itr_id"], ds.Tables[1].Rows[0]["pat_sid"], pat_c_name, ds.Tables[1].Rows[0]["pat_date"]));

                    arr.Add(sqlBcSign);
                }

                if (dict_btype != null)
                {
                    if (dict_btype.Rows.Count > 0)
                        arr.Add("delete ba_rlts where bar_id='" + pat_id + "'");
                    foreach (DataRow drType in dict_btype.Rows)
                    {
                        drType["bar_id"] = pat_id;
                    }
                    //delDict_Type(dict_btype);
                    arr.AddRange(dao.GetInsertSql(dict_btype));
                }
                if (dtAntibio != null)
                {
                    if (dtAntibio.Rows.Count > 0)
                        arr.Add("delete an_rlts where anr_id='" + pat_id + "'");
                    foreach (DataRow drAnti in dtAntibio.Rows)
                    {
                        drAnti["anr_id"] = pat_id;
                    }
                    //delAntibio(dtAntibio);
                    arr.AddRange(dao.GetInsertSql(dtAntibio));
                }
                if (dtCs_res != null)
                {
                    foreach (DataRow dtCs in dtCs_res.Rows)
                    {
                        dtCs["bsr_id"] = pat_id;
                    }
                    arr.AddRange(dao.GetInsertSql(dtCs_res));
                }
                arr.AddRange(dao.GetInsertSql(dtPatients_mi));
                dao.DoTran(arr);
                DataTable dt = new DataTable("patients");
                dt.Columns.Add("pat_id");
                dt.Rows.Add(new String[] { pat_id });
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("新增信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet delPat(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataRow drAction = ds.Tables["action"].Rows[0];
                String del_flag = drAction["del_flag"].ToString();
                DataTable dtPat = ds.Tables["patients"];
                //string pat_id = dtPat.Rows[0]["pat_id"].ToString();
                ArrayList arr = dao.getDeleteSQL(dtPat, new string[] { "pat_id" });

                foreach (DataRow drPat in dtPat.Rows)
                {
                    if (!Compare.IsEmpty(drPat["pat_bar_code"]))
                    {
                        arr.Add(string.Format("update bc_cname set bc_flag='0' where bc_lis_code in (select pat_com_id from dbo.patients_mi where pat_id='{0}') and bc_bar_code='{1}'", drPat["pat_id"], drPat["pat_bar_code"].ToString()));

                        string sqlBcSign = string.Format(@"insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status)
                                                      values('{0}','{1}',getdate(),'{3}','{4}','530')", drPat["pat_bar_code"], drPat["pat_bar_code"], "", string.Empty, string.Empty);
                        arr.Add(sqlBcSign);
                    }
                }

                dtPat.TableName = "patients_mi";
                arr.AddRange(dao.getDeleteSQL(dtPat, new string[] { "pat_id" })); 

                //                string pat_bar_code = dtPat.Rows[0]["pat_bar_code"].ToString();
                //                if (!string.IsNullOrEmpty(pat_bar_code))
                //                {
                //                    string sqlBcSign = string.Format(@"insert into bc_sign(bc_bar_code,bc_bar_no,bc_date,bc_login_id,bc_name,bc_status)
                //                                                      values('{0}','{1}','{2}','{3}','{4}','530','{5}')", pat_bar_code, pat_bar_code, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Empty, string.Empty);

                //                }

                //dtPat.TableName = "resulto";
                //dtPat.Columns.Add("res_id");
                //foreach (DataRow _dr in dtPat.Rows)
                //{
                //    _dr["res_id"] = _dr["pat_id"];
                //}
                if (del_flag.Trim() == "1")//是否删除结果
                {
                    foreach (DataRow dr in dtPat.Rows)
                    {
                        string pat_id = dr["pat_id"].ToString();
                        // dtPat.Rows[0]["pat_id"].ToString();
                        arr.Add(String.Format("delete from ba_rlts where bar_id='{0}'", pat_id));
                        arr.Add(String.Format("delete from an_rlts where anr_id='{0}'", pat_id));
                        arr.Add(String.Format("delete from cs_rlts where bsr_id='{0}'", pat_id));
                    }

                }
                dao.DoTran(arr);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("删除信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doOther(DataSet ds)
        {

            DataSet result = new DataSet();
            try
            {

                DataRow drAction = ds.Tables["action"].Rows[0];
                String action = drAction["action"].ToString();
                switch (action)
                {
                    case "del_pat": result = this.delPat(ds); break;//删除病人资料

                    case "audit_pat": result = this.auditPat(ds); break;//审核病人资料

                    case "preaudit_pat": result = this.preauditPat(ds); break;//审核病人资料

                    case "report_pat": result = this.reportPat(ds); break;//报告病人资料

                    case "getScheml": result = this.getScheml(ds); break;//获取病人资料录入，明细，结果的数据库表结构

                    case "getLayout": result = this.getLayout(ds); break;//获得设计面板

                    case "getMaxSID": result = this.getMaxSID(ds); break;//获取最大样本号

                    case "set_desig": result = this.viewResulto(ds); break;//设计版面
                    case "delLayout": result = this.delLayout(ds); break;//视窗

                    case "getOldPat": result = this.getOldPat(ds); break;//历史病人资料

                    case "getType": result = this.AntiType(ds); break;//查询报表类型

                    case "getPatByIn_no": result = this.getPatByIn_no(ds); break;//根据病人ID查找历史记录

                    case "setPatPrint": result = this.setPatPrint(ds); break;//打印并修改状态

                    case "copyOldRlts": result = this.copyOldRlts(ds); break;//复制历史结果
                    //case "pat_history": result = this.patHistory(ds); break;//历史
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;

        }

        /// <summary>
        /// 更新打印状态
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet setPatPrint(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dtPat = ds.Tables["sql"];
            try
            {
                ArrayList lisSql = new ArrayList();

                DataRow drPat = dtPat.Rows[0];

                string strWhere = drPat["pat_where"].ToString().Trim();

                string sql = @"update patients set pat_flag='{0}',Pat_prt_date=getdate() {1}";

                sql = string.Format(sql, "4", strWhere);

                lisSql.Add(sql);


                string updateBarcodeSql = string.Format(@"insert into bc_sign 
                                                        (bc_date, bc_login_id, bc_name,  bc_status, bc_bar_no, bc_bar_code, pat_id)
                                                        (
                                                        select 
                                                        getdate() bc_date,
                                                        '' bc_login_id,--pat_report_code bc_login_id,
                                                        '' bc_name,--PowerUserInfo.username bc_name,
                                                        '100' bc_status,
                                                        pat_bar_code bc_bar_no,
                                                        pat_bar_code bc_bar_code,
                                                        pat_id 
                                                        from 
                                                        patients 
                                                        --LEFT OUTER JOIN PowerUserInfo  on patients.pat_report_code=PowerUserInfo.loginId
                                                        {0}
                                                        )", strWhere);

                lisSql.Add(updateBarcodeSql);

                dao.DoTran(lisSql);

                result.Tables.Add(dtPat.Copy());
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString()));
            }
            return result;
        }

        /// <summary>
        /// 根据病人ID查找历史记录
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet getPatByIn_no(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dtPat = ds.Tables["patients"];
            try
            {
                DataRow drPat = dtPat.Rows[0];
                string sql = @"select top 1 pat_bed_no,pat_name,pat_age_exp,pat_sex,pat_date,
                            pat_sex_name = case when pat_sex = '1' then '男' when pat_sex = '2' then '女' else '' end 
                            from dbo.patients where pat_no_id='{0}' and pat_in_no='{1}' order by pat_date desc";
                sql = string.Format(sql, drPat["pat_no_id"], drPat["pat_in_no"]);
                DataTable Pat = dao.GetDataTable(sql);
                Pat.TableName = "patients";
                result.Tables.Add(Pat);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet AntiType(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dtOld = ds.Tables["antiType"];
            try
            {
                string sql = "select top 1 * from cs_rlts where bsr_id='" + dtOld.Rows[0]["pat_id"].ToString() + "'";
                DataTable oldPat = dao.GetDataTable(sql);
                oldPat.TableName = "type";
                result.Tables.Add(oldPat);

                string sqlYm = "select top 1 * from an_rlts where anr_id='" + dtOld.Rows[0]["pat_id"] + "'";
                DataTable dtAnr = dao.GetDataTable(sqlYm);
                dtAnr.TableName = "an_rlts";
                result.Tables.Add(dtAnr);

                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet getOldPat(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dtOld = ds.Tables["oldPat"];
            try
            {
                string samsql = string.Empty;

                int TopCount = 3;//默认只显示3条
                //系统配置：细菌病人历史记录显示数
                string strTemp = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("History_Bacteria_Top500");
                if (!string.IsNullOrEmpty(strTemp) && int.TryParse(strTemp, out TopCount) && TopCount > 3)
                {

                }
                else
                {
                    TopCount = 3;
                }

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ShowNewSid") == "是")
                {
                    if (dtOld.Columns.Contains("pat_sam_id") && dtOld.Rows[0]["pat_sam_id"] != null
                        && dtOld.Rows[0]["pat_sam_id"] != DBNull.Value && !string.IsNullOrEmpty(dtOld.Rows[0]["pat_sam_id"].ToString()))
                    {
                        samsql = string.Format(" and pat_sam_id='{0}' ", dtOld.Rows[0]["pat_sam_id"]);
                    }
                }
                //                string sql = @"select top {0} * from patients where pat_no_id='{1}' and pat_in_no='{2}' and pat_itr_id='{3}' and pat_date<'{4}'
                //                           and pat_name='{5}'  and pat_no_id is not null and pat_in_no is not null and pat_itr_id is not null and pat_name is not null and pat_date between '{7}' and '{8}' {6} ";
                string sql = @"select top {0} patients.*,dict_sample.sam_name from patients left join dict_sample on patients.pat_sam_id=dict_sample.sam_id where pat_no_id='{1}' and pat_in_no='{2}' and pat_itr_id='{3}' and pat_date<'{4}'
                           and pat_name='{5}'  and pat_no_id is not null and pat_in_no is not null and pat_itr_id is not null and pat_name is not null and pat_date between '{7}' and '{8}' {6} ";
                string sqlStr = string.Format(sql, TopCount, dtOld.Rows[0]["pat_no_id"], dtOld.Rows[0]["pat_in_no"], dtOld.Rows[0]["pat_itr_id"], dtOld.Rows[0]["pat_date"], dtOld.Rows[0]["pat_name"], samsql, dtOld.Rows[0]["datefrom"], dtOld.Rows[0]["dateto"]);
                DataTable oldPat = dao.GetDataTable(sqlStr);
                oldPat.TableName = "oldPat";

                if (oldPat.Rows.Count < TopCount)
                {
                    sqlStr = string.Format(sql, TopCount - oldPat.Rows.Count, dtOld.Rows[0]["pat_no_id"], dtOld.Rows[0]["pat_in_no"], dtOld.Rows[0]["pat_itr_id"], dtOld.Rows[0]["pat_date"], dtOld.Rows[0]["pat_name"], samsql, dtOld.Rows[0]["datefrom"], dtOld.Rows[0]["dateto"]);
                    DataTable dtPatHis = CRUD.LisHistoryResultController.GetPatHistoryResultData(sqlStr, "oldPat");

                    if (dtPatHis != null && dtPatHis.Rows.Count > 0)
                    {
                        oldPat.Merge(dtPatHis);
                    }
                }
                result.Tables.Add(oldPat);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 复制历史结果
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet copyOldRlts(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtCopyOld = ds.Tables["dtCopyOld"];

                string old_pat_id = dtCopyOld.Rows[0]["old_pat_id"].ToString();
                string pat_id = dtCopyOld.Rows[0]["pat_id"].ToString();
                string pat_itr_id = dtCopyOld.Rows[0]["pat_itr_id"].ToString();
                string pat_sid = dtCopyOld.Rows[0]["pat_sid"].ToString();

                string sqlDelRlts = string.Format(@"delete from dbo.an_rlts where anr_id='{0}';
delete from dbo.ba_rlts where bar_id='{0}';
delete from dbo.cs_rlts where bsr_id='{0}'", pat_id);

                string sqlCoypOld_an_rlts = string.Format(@"INSERT INTO an_rlts
           ([anr_id],[anr_bid],[anr_aid],[anr_dose],[anr_mic],[anr_smic1],[anr_smic2]
           ,[anr_ref],[anr_date],[anr_mid],[anr_sid],[anr_test_method],[anr_bcnt],[anr_unit]
           ,[anr_st_id],[anr_row_flag],anr_remark)
     select '{0}',anr_bid,anr_aid,anr_dose,anr_mic,anr_smic1,anr_smic2,anr_ref,getdate(),'{1}','{2}'
           ,anr_test_method,anr_bcnt,anr_unit,anr_st_id,anr_row_flag,anr_remark
     from an_rlts where anr_id='{3}'", pat_id, pat_itr_id, pat_sid, old_pat_id);

                string sqlCoypOld_ba_rlts = string.Format(@"INSERT INTO ba_rlts
           ([bar_id],[bar_mid],[bar_date],[bar_sid],[bar_bid],[bar_bcnt],[bar_scripe])
     select '{0}','{1}',getdate(),'{2}',bar_bid,bar_bcnt,bar_scripe
     from ba_rlts where bar_id='{3}'", pat_id, pat_itr_id, pat_sid, old_pat_id);

                string sqlCoypOld_cs_rlts = string.Format(@"INSERT INTO cs_rlts
           ([bsr_id],[bsr_mid],[bsr_date],[bsr_sid],[bsr_cname],[bsr_describe],[bsr_res_flag],[bsr_seq],[bsr_i_flag])
     select '{0}','{1}',getdate(),'{2}',bsr_cname,bsr_describe,bsr_res_flag,bsr_seq,bsr_i_flag
     from cs_rlts where bsr_id='{3}'", pat_id, pat_itr_id, pat_sid, old_pat_id);

                DBHelper helper = new DBHelper();
                int iRowAffact = helper.ExecuteNonQuery(sqlDelRlts);
                iRowAffact = helper.ExecuteNonQuery(sqlCoypOld_an_rlts);
                iRowAffact = helper.ExecuteNonQuery(sqlCoypOld_ba_rlts);
                iRowAffact = helper.ExecuteNonQuery(sqlCoypOld_cs_rlts);

                return result;
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "", ex.ToString());
                result.Tables.Add(CommonBIZ.createErrorInfo("复制信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet delLayout(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {

                DataTable dtPat = ds.Tables["sys_form_desig"];
                //string pat_id = dtPat.Rows[0]["pat_id"].ToString();
                ArrayList arr = dao.getDeleteSQL(dtPat, new string[] { "des_id", "des_type", "des_object" });
                dao.DoTran(arr);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("删除信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet getLayout(DataSet ds)
        {
            DataSet result = new DataSet();

            try
            {
                DataTable dt = ds.Tables["sys_form_desig"];
                string sql = "select * from sys_form_desig where des_id='{0}' and des_type='{1}' and des_object='{2}'";
                string sql1 = string.Format(sql, dt.Rows[0]["des_id"], dt.Rows[0]["des_type"], dt.Rows[0]["des_object"]);
                DataTable dtLayout = new DataTable();
                dtLayout = dao.GetDataTable(sql1);
                if (dtLayout.Rows.Count == 0)
                {
                    sql1 = string.Format(sql, dt.Rows[0]["specialityId"], "1", dt.Rows[0]["des_object"]);
                    dtLayout = dao.GetDataTable(sql1);
                    if (dtLayout.Rows.Count == 0)
                    {
                        sql1 = string.Format(sql, "0", "0", dt.Rows[0]["des_object"]);
                        dtLayout = dao.GetDataTable(sql1);
                    }
                }
                result.Tables.Add(dtLayout);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet viewResulto(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dt = ds.Tables["sys_form_desig"];
                byte[] bt = dt.Rows[0]["des_value"] as byte[];

                DBHelper helper = new DBHelper();

                SqlCommand cmd = new SqlCommand();
                string sql = "insert into sys_form_desig(des_id,des_type,des_object,des_value,des_flag) values(@des_id,@des_type,@des_object,@des_value,@des_flag)";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("des_id", dt.Rows[0]["des_id"]);
                cmd.Parameters.AddWithValue("des_type", dt.Rows[0]["des_type"]);
                cmd.Parameters.AddWithValue("des_object", dt.Rows[0]["des_object"]);
                cmd.Parameters.AddWithValue("des_value", bt);
                cmd.Parameters.AddWithValue("des_flag", dt.Rows[0]["des_flag"]);
                helper.ExecuteNonQuery(cmd);
                result.Tables.Add(dt.Copy());
                return result;

            }

            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet getScheml(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                result.Tables.Add(dao.GetDataTable(@"select top 0 patients.*,'' itr_ptype, '' itr_mid, '' dep_name, 
      '' sam_name, '' chk_cname, '' doc_name, '' no_name,'' ori_name,pat_sex_name = '',
      ''  pat_chk_name from patients", "patients"));
                result.Tables.Add(dao.GetDataTable(@"SELECT  top 0 dict_combine.com_name as pat_com_name,dict_combine.com_seq,patients_mi.* FROM patients_mi 
        INNER JOIN dict_combine ON patients_mi.pat_com_id = dict_combine.com_id", "patients_mi"));
                result.Tables.Add(dao.GetDataTable("select * from ba_rlts where bar_id='0'", "ba_rlts"));
                result.Tables.Add(dao.GetDataTable("select anr_bid as bt_id,anr_aid as anti_id,'' as anti_cname,anr_mic as anr_res,anr_smic1 as anr_res1,anr_st_id,anr_ref,'' ss_zone,anr_row_flag,anr_remark from an_rlts where anr_id='0'", "an_rlts"));
                result.Tables.Add(dao.GetDataTable("select * from cs_rlts where bsr_id='0'", "cs_rlts"));
                result.Tables.Add(dao.GetDataTable("select * from dict_nobact where nob_del='0' order by nob_seq", "dict_nobact"));
                //result.Tables.Add(dao.GetDataTable("select * from dbo.sys_form_desig", "sys_form_desig"));
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("删除信息出错!", ex.ToString())); ;
            }
            return result;
        }

        private DataSet getMaxSID(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataRow drgetMaxSID = ds.Tables["search"].Rows[0];


                DataTable dtWhereOP = ds.Tables["search"].Clone();
                DataRow drWhereOP = dtWhereOP.NewRow();
                drWhereOP["pat_date"] = UT_DateTimeTypes.opTheDay;
                drWhereOP["pat_itr_id"] = UT_StringTypes.opEqual;

                dtWhereOP.Rows.Add(drWhereOP);
                String where = CommonBIZ.GetWhereString(ds.Tables["search"], dtWhereOP, ds.Tables["search"].Clone());
                result.Tables.Add(
                  dao.GetDataTable(
                  String.Format(@"SELECT MAX(CAST(pat_sid AS bigint)) pat_sid FROM patients where {0} ", where)
                  , "result")
                  );
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("删除信息出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 报告
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet reportPat(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPat = ds.Tables["patients"];

                string ipAddress = "";

                if (dtPat.Columns.Contains("pat_remark"))
                {
                    ipAddress = dtPat.Rows[0]["pat_remark"].ToString();
                }

                string strChkCode = dtPat.Rows[0]["pat_chk_code"].ToString();
                bool blnInputerAndReporterMustDiff = CacheSysConfig.Current.GetSystemConfig("Audit_InputerAndReporterMustDiff") == "是" ? true : false;
                string addsql = string.Empty;
                if (dtPat.Columns.Contains("pat_i_code") && dtPat.Rows[0]["pat_i_code"] != null && !string.IsNullOrEmpty(dtPat.Rows[0]["pat_i_code"].ToString()))
                {
                    addsql = string.Format(",pat_i_code='{0}' ", dtPat.Rows[0]["pat_i_code"]);
                }

                dtPat.Columns.Remove("pat_chk_code");
                dtPat.Columns.Remove("pat_chk_date");
                string anditDate = ServerDateTime.GetDatabaseServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                ArrayList arr = new ArrayList();

                List<string> listPatIdToReport = new List<string>();//二审的病人id
                List<string> listPatIdToUndoReport = new List<string>();//取消二审的病人id
                DataTable dtbCritical = null;
                DataTable dtbCriticalBacResult = null;
                DataTable dtbCriticalAllergy = null;
                DataTable dtTemp = null;
                string strReportType = "";
                List<string> strBarCodeList = new List<string>();
                foreach (DataRow drReportPat in dtPat.Rows)
                {
                    string pat_id = drReportPat["pat_id"].ToString();
                    string strSql = @"select pat_id,
	                                    pat_itr_id,
	                                    pat_sid,
	                                    pat_name,
	                                    pat_sex,
	                                    pat_age,
	                                    pat_age_exp,
	                                    pat_age_unit,
	                                    pat_dep_id,
	                                    pat_no_id,
	                                    pat_in_no,
	                                    pat_admiss_times,
	                                    pat_bed_no,
	                                    pat_c_name,
	                                    pat_diag,
	                                    pat_rem,
	                                    pat_work,
	                                    pat_tel,
	                                    pat_email,
	                                    pat_unit,
	                                    pat_address,
	                                    pat_pre_week,
	                                    pat_height,
	                                    pat_weight,
	                                    pat_sam_id,
	                                    pat_chk_id,
	                                    pat_doc_id,
	                                    pat_i_code,
	                                    pat_chk_code,
	                                    pat_send_code,
	                                    pat_report_code,
	                                    pat_ctype,
	                                    pat_send_flag,
	                                    pat_prt_flag,
	                                    pat_flag,
	                                    pat_reg_flag,
	                                    pat_urgent_flag,
	                                    pat_drugfast,
	                                    pat_look_code,
	                                    pat_exp,
	                                    pat_pid,
	                                    pat_date,
	                                    pat_sdate,
	                                    pat_rec_date,
	                                    pat_chk_date,
	                                    pat_report_date,
	                                    pat_send_date,
	                                    pat_look_date,
	                                    pat_social_no,
	                                    pat_emp_id,
	                                    pat_bar_code,
	                                    pat_host_order,
	                                    Pat_etagere,
	                                    Pat_place,
	                                    pat_sample_date,
	                                    pat_apply_date,
	                                    pat_jy_date,
	                                    Pat_prt_date,
	                                    pat_sample_part,
	                                    pat_ori_id,
	                                    pat_mid_info,
	                                    pat_comment,
	                                    pat_hospital_id,
	                                    pat_modified_times,
	                                    pat_fee_type,
	                                    pat_sam_rem,
	                                    pat_key,
	                                    pat_sample_receive_date,
	                                    pat_dep_name,
	                                    pat_ward_id,
	                                    pat_ward_name,
	                                    pat_app_no,
	                                    pat_doc_name,
	                                    pat_recheck_flag,
	                                    pat_emp_company_name,
	                                    pat_reach_date,
	                                    pat_critical,
                                        pat_upid
                                    from patients
                                     where pat_id='" + pat_id + "'";

                    DataTable dtPatientInfo = dao.GetDataTable(strSql, "patients");
                    DataTable dtResult = new DataTable();

                    string strFlag = dtPatientInfo.Rows[0]["pat_flag"].ToString();

                    string strUpdate = string.Empty;

                    string strBarCode = string.Empty;

                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        if (dtPatientInfo.Rows[0]["pat_bar_code"] != null && dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim() != string.Empty)
                        {
                            strBarCode = dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim();
                            strBarCodeList.Add(strBarCode);
                        }

                        if (dtbCritical == null)
                        {
                            dtbCritical = dtPatientInfo.Clone();
                        }
                        if (dtTemp == null)
                        {
                            dtTemp = dtPatientInfo.Clone();
                        }
                        dtbCritical.Rows.Add(dtPatientInfo.Rows[0].ItemArray);
                        dtTemp.Rows.Add(dtPatientInfo.Rows[0].ItemArray);
                    }
                      if (strFlag == "0" || strFlag == "1")
                    {
                        //判断检验录入者与二审是否一致
                        if (blnInputerAndReporterMustDiff)
                        {
                            if (strChkCode == dtPatientInfo.Rows[0]["pat_i_code"].ToString())
                            {
                                if (result != null && result.Tables.Contains("InputerAndReporterMustDiff"))
                                {
                                    result.Tables["InputerAndReporterMustDiff"].Rows.Add(new string[] { "病人: " + dtPatientInfo.Rows[0]["pat_name"].ToString() + " 的检验报告审核者不能与检验录入者相同" });
                                }
                                else
                                {
                                    if (result == null)
                                    {
                                        result = new DataSet();
                                    }
                                    DataTable dtbInputerAndReporterMustDiff = new DataTable("InputerAndReporterMustDiff");
                                    dtbInputerAndReporterMustDiff.Columns.Add("content");
                                    dtbInputerAndReporterMustDiff.Rows.Add(new object[0]);
                                    dtbInputerAndReporterMustDiff.Rows[0]["content"] = "病人: " + dtPatientInfo.Rows[0]["pat_name"].ToString() + " 的检验报告审核者不能与检验录入者相同";
                                    result.Tables.Add(dtbInputerAndReporterMustDiff);
                                }
                                continue;
                            }
                        }



                        dtResult = this.GetBacResult(pat_id);

                        if (dtbCriticalBacResult == null)
                        {
                            dtbCriticalBacResult = dtResult.Clone();
                        }
                        if (dtResult.Rows.Count <= 0)
                        {
                            dtResult = this.GetAllergy(pat_id);

                            if (dtbCriticalAllergy == null)
                            {
                                dtbCriticalAllergy = dtResult.Clone();
                            }
                            foreach (DataRow row in dtResult.Rows)
                            {
                                dtbCriticalAllergy.Rows.Add(row.ItemArray);
                            }
                        }
                        else
                        {
                            foreach (DataRow row in dtResult.Rows)
                            {
                                dtbCriticalBacResult.Rows.Add(row.ItemArray);
                            }
                        }
                        //拼接CA电子签名所需的报告单内容
                        if (!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode")) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode") != "无")
                        {
                            //开始 李进 2013-09-18 添加判断分支,如果为网证通
                            if (String.Equals(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode"), "GDNETCA", StringComparison.CurrentCultureIgnoreCase))
                            {

                                if (result != null)
                                {
                                    if (!result.Tables.Contains(dtPatientInfo.TableName))
                                        result.Tables.Add(dtPatientInfo.Copy());
                                    else
                                        result.Tables[dtPatientInfo.TableName].Merge(dtPatientInfo);
                                    if (result.Tables.Contains(dtResult.TableName))
                                        result.Tables[dtResult.TableName].Merge(dtResult);
                                    else
                                        result.Tables.Add(dtResult.Copy());
                                }
                                else
                                {
                                    if (result == null)
                                    {
                                        result = new DataSet();
                                    }
                                    result.Tables.Add(dtPatientInfo.Copy());
                                    result.Tables.Add(dtResult.Copy());
                                }
                            }
                            //结束 2013-09-18 添加判断分支,如果为网证通
                            else
                            {
                                Lib.biz.CASign.ICheckCASign CheckCASign = new Lib.biz.CASign.AuditCheckCASign(dtPatientInfo, dtResult);
                                DataTable dtbCASignContent = CheckCASign.CASignContentSplice();

                                //在原文追加upid
                                if (dtbCASignContent != null && dtbCASignContent.Rows.Count > 0 && dtbCASignContent.Columns.Contains("SourceContent"))
                                {
                                    string temp_scontent = dtbCASignContent.Rows[0]["SourceContent"].ToString();
                                    if (!string.IsNullOrEmpty(temp_scontent) && dtPatientInfo != null && dtPatientInfo.Rows.Count > 0)
                                    {
                                        dtbCASignContent.Rows[0]["SourceContent"] = temp_scontent + dtPatientInfo.Rows[0]["pat_upid"].ToString() + ",";
                                    }
                                }

                                if (result != null && result.Tables.Contains(dtbCASignContent.TableName))
                                {
                                    result.Tables[dtbCASignContent.TableName].Merge(dtbCASignContent);
                                }
                                else
                                {
                                    if (result == null)
                                    {
                                        result = new DataSet();
                                    }
                                    result.Tables.Add(dtbCASignContent);
                                }
                            }
                        }


                        #region CA时间戳

                        if ((!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode")) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode") != "无"))
                        {
                            Lib.biz.CASign.ICheckCASign CheckCASign = new Lib.biz.CASign.AuditCheckCASign(dtPatientInfo, dtResult);
                            DataTable dtbCASignContent = CheckCASign.CASignContentSplice();

                            //在原文追加upid
                            if (dtbCASignContent != null && dtbCASignContent.Rows.Count > 0 && dtbCASignContent.Columns.Contains("SourceContent"))
                            {
                                string temp_scontent = dtbCASignContent.Rows[0]["SourceContent"].ToString();
                                if (!string.IsNullOrEmpty(temp_scontent) && dtPatientInfo != null && dtPatientInfo.Rows.Count > 0)
                                {
                                    dtbCASignContent.Rows[0]["SourceContent"] = temp_scontent + dtPatientInfo.Rows[0]["pat_upid"].ToString() + ",";
                                }
                            }

                            Lib.biz.CASign.ICASignature Timestamp = Lib.biz.CASign.CASignatureFactory.CreateCASignature(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode"));
                            DateTime dtTimestamp = DateTime.Now;
                            DataTable dtbCATimestamp = dtbCASignContent.Copy();
                            if (Timestamp.Sign(ref dtbCATimestamp, out dtTimestamp))
                            {
                                //时间戳时间
                                anditDate = dtTimestamp.ToString("yyyy-MM-dd HH:mm:ss");

                                //2014-6-12用新方法，用新字段保存时间戳内容
                                if (true)
                                {
                                    DataTable dttempCopy = dtbCATimestamp.DefaultView.ToTable();

                                    //把SourceContent改为SourceTimestamp保存时间戳原文
                                    if (dttempCopy != null && dttempCopy.Columns.Contains("SourceContent"))
                                    {
                                        dttempCopy.Columns["SourceContent"].ColumnName = "SourceTimestamp";
                                    }
                                    //把SignContent改为SignTimestamp保存时间戳密文
                                    if (dttempCopy != null && dttempCopy.Columns.Contains("SignContent"))
                                    {
                                        dttempCopy.Columns["SignContent"].ColumnName = "SignTimestamp";
                                    }

                                    new CASignature().InsertReportCASignature(dttempCopy);
                                }
                                else
                                {
                                    new CASignature().InsertReportCASignature(dtbCATimestamp);
                                }
                            }

                        }

                        #endregion


                        //}
                    }

                    strReportType = "60";
                    if (strFlag == "0")
                    {
                        strUpdate = string.Format(@"update patients set 
                                                                        pat_flag=2,pat_chk_code='{0}',pat_chk_date='{1}',
                                                                        pat_report_code='{0}',pat_report_date='{1}' {3} 
                                                                        where pat_id='{2}'", drReportPat["pat_report_code"], anditDate, pat_id, addsql);

                        listPatIdToReport.Add(pat_id);
                    }
                    else if (strFlag == "1")
                    {
                        strUpdate = string.Format(@"update patients set 
                                                                        pat_flag=2,
                                                                        pat_report_code='{0}',pat_report_date='{1}' {3}
                                                                        where pat_id='{2}'", drReportPat["pat_report_code"], anditDate, pat_id, addsql);

                        listPatIdToReport.Add(pat_id);
                    }
                    else
                    {
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("OneStepCancelReport") == "是")
                        {
                            strUpdate = string.Format(@"update patients set 
                                                                        pat_flag=0,
                                                                        pat_chk_code=null,pat_chk_date=null,
                                                                        pat_report_code=null,pat_report_date=null,
                                                                        pat_look_code=null,pat_look_date=null 
                                                                        where pat_id='{0}'", pat_id);
                            strReportType = "50";
                        }
                        else
                        {
                            strUpdate = string.Format(@"update patients set 
                                                                        pat_flag=1,
                                                                        pat_report_code=null,pat_report_date=null,
                                                                        pat_look_code=null,pat_look_date=null 
                                                                        where pat_id='{0}'", pat_id);
                            strReportType = "70";
                        }



                        listPatIdToUndoReport.Add(pat_id);

                    }
                    arr.Add(strUpdate);


                    if (strBarCode != string.Empty)
                    {
                        string sqlUpdateBcPatients = string.Format(@"
                        update bc_patients
                        set bc_status = '{0}' ,bc_lastaction_time = getdate()
                        where bc_bar_code = '{1}'", strReportType, strBarCode);
                        arr.Add(sqlUpdateBcPatients);
                    }

                    string sqlInsterBcSign = string.Empty;
                    if (strReportType == "60")
                    {
                        string sql = string.Format(@"select * from dict_instrmt where itr_id='{0}'", dtPatientInfo.Rows[0]["pat_itr_id"]);
                        DataTable dtInstrmt = dao.GetDataTable(sql, "dict_instrmt");
                        string reporDate = Convert.ToDateTime(anditDate).ToString("yyyy/MM/dd HH:mm:ss");
                        string strRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", dtInstrmt.Rows[0]["itr_mid"], dtPatientInfo.Rows[0]["pat_sid"], dtPatientInfo.Rows[0]["pat_c_name"], reporDate);
                        sqlInsterBcSign = string.Format(@"
                                insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,
                                bc_bar_no,bc_bar_code,bc_flow,bc_remark,pat_id)
                                values
                                (getdate(),'{0}','{1}',{2},'{3}','{3}','1','{4}','{5}')", drReportPat["pat_report_code"], drReportPat["pat_report_name"], strReportType, strBarCode, ipAddress + " " + strRemark  , pat_id);
                    }
                    else
                    {
                        string remark = string.Empty;
                        //插入反审原因
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_UndoReportRemark") == "是")
                        {
                            remark = "反审原因：" + drReportPat["pat_remark"].ToString();
                        }
                        sqlInsterBcSign = string.Format(@"
                                insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,
                                bc_bar_no,bc_bar_code,bc_flow,bc_remark,pat_id)
                                values
                                (getdate(),'{0}','{1}',{2},'{3}','{3}','1','{4}','{5}')", drReportPat["pat_report_code"], drReportPat["pat_report_name"], strReportType, strBarCode, ipAddress + " " + remark,pat_id);
                    }
                    arr.Add(sqlInsterBcSign);

                }
                dao.DoTran(arr);

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid") == "HL7")//使用HL7
                {
                    //发送细菌报告单HL7数据
                    if (listPatIdToReport != null && listPatIdToReport.Count > 0)
                    {
                        Lis.SendDataByHl7.TypeStandardDataSenderByHL7 SenderByHl7 = new Lis.SendDataByHl7.TypeStandardDataSenderByHL7();

                        SenderByHl7.SendBacteria(listPatIdToReport);
                    }
                    //发送细菌报告单HL7数据(删除报告)
                    if (listPatIdToUndoReport != null && listPatIdToUndoReport.Count > 0)
                    {
                        Lis.SendDataByHl7.TypeStandardDataSenderByHL7 SenderByHl7 = new Lis.SendDataByHl7.TypeStandardDataSenderByHL7();

                        SenderByHl7.SendBacteriaDelReport(listPatIdToUndoReport);
                    }

                    //return result;
                }
                else if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSendDataToMid") == "广州新海医院")
                {
                    //发送细菌报告单数据
                    if (listPatIdToReport != null && listPatIdToReport.Count > 0)
                    {
                        //上传到新海医院his的中间表
                        Lis.SendDataToMidXHYY.TypeStandardDataSender.Current.Send(listPatIdToReport);
                    }
                    //发送细菌报告单数据(删除报告)
                    if (listPatIdToUndoReport != null && listPatIdToUndoReport.Count > 0)
                    {
                        Lis.SendDataToMidXHYY.TypeStandardDataSender.Current.UndoSend(listPatIdToUndoReport);
                    }

                    //return result;
                }
                else
                {
                    new dcl.svr.resultcheck.SendDataToMid().Run(listPatIdToReport, EnumOperationCode.Report);
                    new dcl.svr.resultcheck.SendDataToMid().Run(listPatIdToUndoReport, EnumOperationCode.UndoReport);
                }

                //发送危急值消息
                SendCriticalMessage scm = new SendCriticalMessage();
                scm.UpdateByBacteria(dtbCritical, dtbCriticalBacResult, dtbCriticalAllergy);

                if (listPatIdToReport.Count > 0 && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BacLab_MICInterimReport") == "开")
                {
                    MicMediaBIZ micBiz = new MicMediaBIZ();
                    micBiz.DeleteMicInterimReport(listPatIdToReport);
                }

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_UploadProcessBarCode") == "是")
                {
                    //foreach (string barCode in strBarCodeList)
                    //{
                    //    Lis.SendDataToCDR.CDRService cds = new Lis.SendDataToCDR.CDRService();
                    //    cds.UploadProcessInvoke(barCode);
                    //}
                }

            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("报告信息出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet auditPat(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPat = ds.Tables["patients"];
                string ipAddress = "";

                if (dtPat.Columns.Contains("IPAddress"))
                {
                    ipAddress = dtPat.Rows[0]["IPAddress"].ToString();
                }
                string anditDate = ServerDateTime.GetDatabaseServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                ArrayList arr = new ArrayList();
                List<string> strBarCodeList = new List<string>();
                foreach (DataRow drAnditPat in dtPat.Rows)
                {
                    string sqlGetPatientInfo = string.Format(@"select pat_bar_code from patients where pat_id='{0}'", drAnditPat["pat_id"]);
                    DataTable dtPatientInfo = dao.GetDataTable(sqlGetPatientInfo);

                    string strBarCode = string.Empty;

                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        if (dtPatientInfo.Rows[0]["pat_bar_code"] != null && dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim() != string.Empty)
                        {
                            strBarCode = dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim();
                            strBarCodeList.Add(strBarCode);
                        }
                    }

                    string strAuditType = "50";

                    if (drAnditPat["pat_flag"].ToString() == "1")
                    {
                        drAnditPat["pat_chk_date"] = anditDate;
                        strAuditType = "40";
                    }
                    else
                    {
                        drAnditPat["pat_chk_date"] = DBNull.Value;
                        drAnditPat["pat_chk_code"] = DBNull.Value;
                    }
                    if (strBarCode != string.Empty)
                    {
                        string sqlUpdateBcPatients = string.Format(@"
                        update bc_patients
                        set bc_status = '{0}' ,bc_lastaction_time = getdate()
                        where bc_bar_code = '{1}'", strAuditType, strBarCode);
                        arr.Add(sqlUpdateBcPatients);
                        string sqlInsterBcSign = string.Format(@"
                                insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,
                                bc_bar_no,bc_bar_code,bc_flow,bc_remark)
                                values
                                (getdate(),'{0}','{1}',{2},'{3}','{3}','1','{4}')", drAnditPat["pat_chk_code"], drAnditPat["pat_chk_name"], strAuditType, strBarCode, ipAddress);
                        arr.Add(sqlInsterBcSign);
                    }
                }
                arr.AddRange(dao.GetUpdateSql(dtPat, new string[] { "pat_id" }));

                dao.DoTran(arr);

                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_UploadProcessBarCode") == "是")
                {
                    //foreach (string barCode in strBarCodeList)
                    //{
                    //    Lis.SendDataToCDR.CDRService cds = new Lis.SendDataToCDR.CDRService();
                    //    cds.UploadProcessInvoke(barCode);
                    //}
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("报告信息出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataSet preauditPat(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPat = ds.Tables["patients"];
                string ipAddress = "";

                if (dtPat.Columns.Contains("IPAddress"))
                {
                    ipAddress = dtPat.Rows[0]["IPAddress"].ToString();
                }
                string anditDate = ServerDateTime.GetDatabaseServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                ArrayList arr = new ArrayList();
                foreach (DataRow drAnditPat in dtPat.Rows)
                {
                    string sqlGetPatientInfo = string.Format(@"select pat_bar_code from patients where pat_id='{0}'", drAnditPat["pat_id"]);
                    DataTable dtPatientInfo = dao.GetDataTable(sqlGetPatientInfo);

                    string strBarCode = string.Empty;

                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        if (dtPatientInfo.Rows[0]["pat_bar_code"] != null && dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim() != string.Empty)
                            strBarCode = dtPatientInfo.Rows[0]["pat_bar_code"].ToString().Trim();
                    }

                    string strAuditType = "38";

                    if (drAnditPat["pat_pre_flag"].ToString() == "1")
                    {
                        drAnditPat["pat_pre_date"] = anditDate;
                        strAuditType = "39";
                    }
                    else
                    {
                        drAnditPat["pat_pre_date"] = DBNull.Value;
                        drAnditPat["pat_pre_code"] = DBNull.Value;
                    }
                    if (strBarCode != string.Empty)
                    {
                        //                        string sqlUpdateBcPatients = string.Format(@"
                        //                        update bc_patients
                        //                        set bc_status = '{0}' ,bc_lastaction_time = getdate()
                        //                        where bc_bar_code = '{1}'", strAuditType, strBarCode);
                        //                        arr.Add(sqlUpdateBcPatients);
                        string sqlInsterBcSign = string.Format(@"
                                insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,
                                bc_bar_no,bc_bar_code,bc_flow,bc_remark)
                                values
                                (getdate(),'{0}','{1}',{2},'{3}','{3}','1','{4}')", drAnditPat["pat_pre_code"], drAnditPat["pat_chk_name"], strAuditType, strBarCode, ipAddress);
                        arr.Add(sqlInsterBcSign);
                    }
                }
                arr.AddRange(dao.GetUpdateSql(dtPat, new string[] { "pat_id" }));

                dao.DoTran(arr);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("报告信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPa = ds.Tables["para"];
                DataTable dtPatBySid = ds.Tables["pati"];
                #region
                if (dtPa != null)
                {
                    string str_itr_id = dtPa.Rows[0]["pat_itr_id"].ToString().Trim();
                    string str_type_id = string.Empty;

                    if (dtPa.Columns.Contains("pat_type_id"))
                        str_type_id = dtPa.Rows[0]["pat_type_id"].ToString().Trim();

                    string sqlItrID_IN = string.Empty;
                    if (str_itr_id == string.Empty && str_type_id != string.Empty)
                    {
                        sqlItrID_IN = "'-1'";
                        string sqlSelect = string.Format("select itr_id from dict_instrmt where itr_type='{0}'", str_type_id);
                        DataTable dtItrID_List = dao.GetDataTable(sqlSelect);

                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow dr in dtItrID_List.Rows)
                        {
                            sb.Append(string.Format(",'{0}'", dr["itr_id"]));
                        }

                        sqlItrID_IN = sqlItrID_IN + sb.ToString();
                    }
                    else
                        sqlItrID_IN = string.Format("'{0}'", str_itr_id);



                    string edate = Convert.ToDateTime(dtPa.Rows[0]["pat_date"]).Date.ToString();
                    string sdate = Convert.ToDateTime(dtPa.Rows[0]["pat_end_date"]).Date.AddDays(1).AddMilliseconds(-1).ToString();

                    String sql = @"select 
cast(pat_sid as bigint) as pat_sid_int,
pat_flag_name='',
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end,
cast(isnull(patients.pat_age,0)/518400 as decimal(18,0)) pat_age,
dict_sample.sam_name as pat_sam_name,
patients.*,
(case patients.pat_host_order when '' then null when null then null else cast(pat_host_order as bigint) end) pat_host_order_int,
0 pat_select  
,user1.username as pat_check_name,
user2.username as pat_report_name,
dict_instrmt.itr_mid,0 as ba_rlts,
ISNULL(isnull((select top 1 1 from ba_rlts where ba_rlts.bar_id=patients.pat_id),(select top 1 1 from cs_rlts where cs_rlts.bsr_id=patients.pat_id)),0) hasresult,
ISNULL(isnull((select top 1 2 from an_rlts with(nolock) where an_rlts.anr_id=patients.pat_id),(select top 1 1 from ba_rlts with(nolock) where ba_rlts.bar_id=patients.pat_id)),0) hasresult2

from patients 
left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
left join dict_instrmt on dict_instrmt.itr_id=patients.pat_itr_id
LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
where pat_date between '{0}' and '{1}' and pat_itr_id in ({2}) ";
                    sql = string.Format(sql, edate, sdate, sqlItrID_IN);
                    string term = "";
                    if (dtPa.Columns.Count != 3)
                    {
                        if (dtPa.Rows[0]["pat_flag"] != null)
                        {
                            string str = dtPa.Rows[0]["pat_flag"].ToString();
                            if (str == "未审核")
                                term = " and pat_flag = '0'";
                            if (str == "未报告")
                            {
                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Bacterial_ShowFirstAuditButton") == "否")
                                    term = " and (pat_flag = '1' or pat_flag = '0')";
                                else
                                    term = " and pat_flag = '1'";
                            }
                            if (str == "未打印")
                                term = " and pat_flag = '2'";
                            if (str == "已打印")
                                term = " and pat_flag ='4'";
                            if (str == "危急值")
                                term = " and (pat_urgent_flag =1 or pat_urgent_flag =2)";
                            sql += term;
                        }
                        if (dtPa.Rows[0]["pat_sex"] != null)
                        {
                            string str = dtPa.Rows[0]["pat_sex"].ToString();
                            if (str == "男")
                                term = " and pat_sex='1'";
                            if (str == "女")
                                term = " and pat_sex='2'";
                            sql += term;
                        }
                        //if (dtPa.Rows[0]["pat_where"] != null)
                        //{
                        //    string condition = dtPa.Rows[0]["pat_con"].ToString();
                        //    if (condition == "姓名")
                        //    {
                        //        sql += " and pat_name like '%" + dtPa.Rows[0]["pat_where"] + "%'";
                        //    }
                        //    if (condition == "样本号")
                        //    {
                        //        sql += " and pat_sid='" + dtPa.Rows[0]["pat_where"] + "'";
                        //    }
                        //}
                    }
                    sql += " order by cast(pat_sid as bigint)";
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "patients";


                    foreach (DataRow drPat in dt.Rows)
                    {
                        if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                        {
                            string patage = drPat["pat_age_exp"].ToString();

                            patage = AgeConverter.TrimZeroValue(patage);
                            patage = AgeConverter.ValueToText(patage);
                            drPat["pat_age_exp"] = patage;
                        }

                        if (drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value)
                        {
                            string patflag = drPat["pat_flag"].ToString();
                            if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                            {
                                drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                            {
                                drPat["pat_flag_name"] = "已打印";
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                            {
                                drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                            {
                                drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                            }
                        }
                        else
                        {
                            drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                        }
                    }

                    //排序，序号升序，样本号升序
                    if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("pat_host_order_int") && dt.Columns.Contains("pat_sid_int"))
                    {
                        DataView dvtemp = dt.DefaultView.ToTable().DefaultView;
                        dvtemp.Sort = "pat_host_order_int asc,pat_sid_int asc";
                        dt = dvtemp.ToTable();
                        dt.AcceptChanges();
                    }

                    result.Tables.Add(dt);


                    return result;
                }
                #endregion
                if (dtPatBySid != null)
                {
                    string edate = Convert.ToDateTime(dtPa.Rows[0]["pat_date"]).Date.ToString();
                    string sdate = Convert.ToDateTime(dtPa.Rows[0]["pat_end_date"]).Date.AddDays(1).AddMilliseconds(-1).ToString();
                    String sql = @"select
pat_flag_name='',
cast(pat_sid as bigint) as pat_sid_int,
cast(isnull(patients.pat_age,0)/518400 as decimal(18,0)) pat_age,
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end
,
*,
(case patients.pat_host_order when '' then null when null then null else cast(pat_host_order as bigint) end) pat_host_order_int,
0 pat_select
,user1.username as pat_check_name,
user2.username as pat_report_name,patients.pat_bar_code
from patients 
LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code

where pat_date between '{0}' and '{1}' and pat_itr_id={2} and pat_sid='{3}'";
                    sql = string.Format(sql, edate, sdate, dtPatBySid.Rows[0]["pat_itr_id"], dtPatBySid.Rows[0]["pat_sid"]);
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "patients";

                    foreach (DataRow drPat in dt.Rows)
                    {
                        if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                        {
                            string patage = drPat["pat_age_exp"].ToString();

                            patage = AgeConverter.TrimZeroValue(patage);
                            patage = AgeConverter.ValueToText(patage);
                            drPat["pat_age_exp"] = patage;
                        }
                        if (drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value)
                        {
                            string patflag = drPat["pat_flag"].ToString();
                            if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                            {
                                drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                            {
                                drPat["pat_flag_name"] = "已打印";
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                            {
                                drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                            }
                            else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                            {
                                drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                            }
                        }
                        else
                        {
                            drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                        }
                    }
                    //排序，序号升序，样本号升序
                    if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("pat_host_order_int") && dt.Columns.Contains("pat_sid_int"))
                    {
                        DataView dvtemp = dt.DefaultView.ToTable().DefaultView;
                        dvtemp.Sort = "pat_host_order_int asc,pat_sid_int asc";
                        dt = dvtemp.ToTable();
                        dt.AcceptChanges();
                    }

                    result.Tables.Add(dt);
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPatients = ds.Tables["patients"];
                DataTable dtPatients_mi = ds.Tables["patients_mi"];
                DataTable dict_btype = ds.Tables["ba_rlts"];
                DataTable dtAntibio = ds.Tables["an_rlts"];
                DataTable dtCs_res = ds.Tables["cs_rlts"];
                DataRow drPat = dtPatients.Rows[0];
                String pat_id = drPat["pat_id"].ToString();

                string strPatKeySQL = string.Format("select pat_key from patients where pat_id='{0}'", pat_id);
                DataTable dtPatKey = dao.GetDataTable(strPatKeySQL);

                if (dtPatKey.Rows.Count == 0)
                    return result;

                string strPatKey = dtPatKey.Rows[0]["pat_key"].ToString();

                if (dtPatients_mi != null && dtPatients_mi.Columns.Contains("pat_key"))
                {
                    dtPatients_mi.Columns.Remove("pat_key");
                    dtPatients_mi.AcceptChanges();
                }

                DateTime pat_date = DateTime.Parse(drPat["pat_date"].ToString());
                string strNewPatId = drPat["pat_itr_id"].ToString() + String.Format("{0:yyyyMMdd}", pat_date)
                   + drPat["pat_sid"].ToString();

                drPat["pat_key"] = strPatKey;
                drPat["pat_id"] = strNewPatId;

                //if (dtPatients.Columns.Contains("pat_key"))
                //    dtPatients.Columns.Remove("pat_key");

                ArrayList arr = dao.GetUpdateSql(dtPatients, new string[] { "pat_key" }, true);
                arr.Add(String.Format("delete from Patients_mi where pat_id='{0}'", pat_id));
                arr.Add(String.Format("delete from ba_rlts where bar_id='{0}'", pat_id));
                arr.Add(String.Format("delete from an_rlts where anr_id='{0}'", pat_id));
                arr.Add(String.Format("delete from cs_rlts where bsr_id='{0}'", pat_id));

                if (dict_btype != null)
                {
                    foreach (DataRow drType in dict_btype.Rows)
                    {
                        drType["bar_id"] = strNewPatId;
                    }
                    arr.AddRange(dao.GetInsertSql(dict_btype));
                }
                if (dtAntibio != null)
                {
                    foreach (DataRow drAnti in dtAntibio.Rows)
                    {
                        drAnti["anr_id"] = strNewPatId;
                    }
                    arr.AddRange(dao.GetInsertSql(dtAntibio));
                }
                if (dtCs_res != null)
                {
                    foreach (DataRow dtCs in dtCs_res.Rows)
                    {
                        dtCs["bsr_id"] = strNewPatId;
                    }
                    arr.AddRange(dao.GetInsertSql(dtCs_res));
                }
                if (dtPatients_mi != null)
                {
                    foreach (DataRow dtPatMi in dtPatients_mi.Rows)
                    {
                        dtPatMi["pat_id"] = strNewPatId;
                    }
                    arr.AddRange(dao.GetInsertSql(dtPatients_mi));
                }
                dao.DoTran(arr);
                DataTable dt = new DataTable("patients");
                dt.Columns.Add("pat_id");
                dt.Rows.Add(new String[] { pat_id });
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("更新信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doView(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dtCount = ds.Tables["count"];
            DataTable dtPatients = ds.Tables["patients"];
            try
            {
                if (dtCount != null)
                {
                    string id = dtCount.Rows[0][0].ToString();
                    String sql = @"select e.*,anti_id,a.bt_id,d.anti_cname,'' anr_ref,ss_mstd ss_zone
                                    from dict_btype a,dict_bacteri b,
                                    dict_an_stype c,dict_antibio d,dict_an_sstd e
                                    where
                                    b.bac_bt_id=a.bt_id and a.bt_stid=c.st_id and c.st_id=e.ss_st_id
                                    and e.ss_anti_id=d.anti_id and e.ss_del=0
                                    and e.ss_flag='0'
                                    and b.bac_id='" + id + "' order by e.ss_seq";
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "dict_antibio";
                    result.Tables.Add(dt);
                    String sql2 = @"select bt_stid from dict_bacteri a,dict_btype b
                                    where a.bac_bt_id=b.bt_id and  bac_id='" + id + "'";
                    DataTable dtDB = dao.GetDataTable(sql2);
                    dtDB.TableName = "bt_stid";
                    result.Tables.Add(dtDB);
                    return result;
                }
                if (dtPatients != null)
                {
                    DataRow drAction = ds.Tables["patients"].Rows[0];
                    DateTime? patDate = null;
                    if (ds.Tables["patients"].Columns.Contains("pat_date") && !Compare.IsNullOrDBNull(drAction["pat_date"]))
                    {
                        patDate = DateTime.Parse(drAction["pat_date"].ToString());
                    }
                    SqlHelper helper = CRUD.LisHistoryResultController.GetSqlHelper(patDate);

                    String pat_id = drAction["pat_id"].ToString();
                    String where = " pat_id ='" + pat_id + "' ";
                    DataTable dtPat = helper.GetTable(
                   @"SELECT 
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end,
patients.*,dict_instrmt.itr_ptype, dict_instrmt.itr_mid,
--dict_depart.dep_name, 
      dict_sample.sam_name as pat_sam_name, dict_checkb.chk_cname, dict_doctor.doc_name, 
      PowerUserInfo.userName AS pat_chk_name, dict_no_type.no_name, 
      dict_origin.ori_name,user1.username as pat_check_name,
    user2.username as pat_report_name,dbo.getAge(patients.pat_age_exp) pat_age_txt,dict_sample.sam_name
FROM patients LEFT OUTER JOIN
      dict_origin ON patients.pat_ori_id = dict_origin.ori_id LEFT OUTER JOIN
      dict_no_type ON patients.pat_no_id = dict_no_type.no_id LEFT OUTER JOIN
      PowerUserInfo ON 
      patients.pat_chk_code = PowerUserInfo.userInfoId LEFT OUTER JOIN
      dict_doctor ON patients.pat_doc_id = dict_doctor.doc_id LEFT OUTER JOIN
      dict_checkb ON patients.pat_chk_id = dict_checkb.chk_id LEFT OUTER JOIN
      dict_sample ON patients.pat_sam_id = dict_sample.sam_id LEFT OUTER JOIN
      --dict_depart ON patients.pat_dep_id = dict_depart.dep_id LEFT OUTER JOIN
      dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
      where " + where, "patients"
                    );
                    result.Tables.Add(dtPat);

                    DataTable dtPatients_mi = helper.GetTable(//
            String.Format(
            @"SELECT dict_combine.com_name as pat_com_name,dict_combine.com_seq, patients_mi.*
              FROM patients_mi INNER JOIN
                    dict_combine ON patients_mi.pat_com_id = dict_combine.com_id
              WHERE (patients_mi.pat_id = '{0}')", pat_id), "patients_mi");

                    if (dtPatients_mi != null && dtPatients_mi.Columns.Contains("pat_key"))
                    {
                        dtPatients_mi.Columns.Remove("pat_key");
                        dtPatients_mi.AcceptChanges();
                    }
                    result.Tables.Add(dtPatients_mi);

                    DataTable dict_btype = helper.GetTable(//
            String.Format(
            @"select ba_rlts.*,dict_bacteri.bac_bt_id from ba_rlts left join dict_bacteri 
on ba_rlts.bar_bid=dict_bacteri.bac_id where bar_id='{0}'", pat_id), "ba_rlts");
                    result.Tables.Add(dict_btype);

                    if (dict_btype != null && dict_btype.Rows.Count > 0)
                    {
                        foreach (DataRow drtempbtype in dict_btype.Rows)
                        {
                            if (string.IsNullOrEmpty(drtempbtype["bar_seq"].ToString()))
                            {
                                drtempbtype["bar_seq"] = 1;
                            }
                        }
                        dict_btype.AcceptChanges();
                    }

                    string IsShowByDict_an_sstd = "";
                    //系统配置：根据药敏标准[字典]显示药敏结果
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Bacterial_ByDict_an_sstd_show") == "是")
                    {
                        IsShowByDict_an_sstd = " and b.ss_flag='0' ";
                    }

                    DataTable dtAntibio = helper.GetTable(//
            String.Format(
            @"select anr_bid as bt_id,a.anr_aid as anti_id,c.anti_cname,anr_mic as anr_res,anr_smic1 as anr_res1,
            anr_st_id,ss_hstd,ss_mstd,ss_lstd,ss_rzone,ss_izone,ss_szone,anr_ref,
(case when isnull(anr_ref,'MIC')='MIC' then ss_mstd else ss_izone end) ss_zone,
a.anr_row_flag,isnull(a.anr_seq,1) as anr_seq,b.ss_seq,a.anr_remark
                    from an_rlts a left join dict_an_sstd b on  
                (a.anr_aid=b.ss_anti_id
                and  a.anr_st_id=b.ss_st_id
                and b.ss_del=0)
                left join dict_antibio c on a.anr_aid=c.anti_id
                where a.anr_id='{0}' {1} order by b.ss_seq,a.anr_aid", pat_id, IsShowByDict_an_sstd), "an_rlts");

                    if (dtAntibio != null && dtAntibio.Rows.Count > 0)
                    {
                        DataView dvtempAntibio = dtAntibio.DefaultView.ToTable().DefaultView;
                        dvtempAntibio.Sort = "anr_seq asc,ss_seq asc,anti_id asc";
                        dtAntibio = dvtempAntibio.ToTable();
                        if (dtAntibio.Columns.Contains("ss_seq"))
                        {
                            dtAntibio.Columns.Remove("ss_seq");
                        }
                        dtAntibio.AcceptChanges();
                    }
                    //dtAntibio.Columns.Add("ss_hstd"); dtAntibio.Columns.Add("ss_mstd");
                    //dtAntibio.Columns.Add("ss_lstd"); dtAntibio.Columns.Add("ss_rzone");
                    //dtAntibio.Columns.Add("ss_izone"); dtAntibio.Columns.Add("ss_szone");
                    result.Tables.Add(dtAntibio);

                    DataTable dt_cs = helper.GetTable(//
            String.Format(
            @"select * from cs_rlts where bsr_id='{0}'", pat_id), "cs_rlts");
                    result.Tables.Add(dt_cs);
                }

            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 获取药敏结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        DataTable GetAllergy(string pat_id)
        {

            string sqlSelect = string.Format(@"
select
	bar_id  as anr_id, 
	anr_bid, 
    dict_bacteri.bac_cname,
    dict_bacteri.bac_bt_id,
	anr_aid, 
	dict_antibio.anti_cname,
	anr_mic ,isnull(ba_rlts.bar_scripe,'') bar_scripe,isnull(ba_rlts.bar_wjtext,'') bar_wjtext
FROM ba_rlts  with(nolock)
LEFT JOIN an_rlts  ON an_rlts.anr_id=ba_rlts.bar_id and an_rlts.anr_bid=ba_rlts.bar_bid
left join dict_bacteri on ba_rlts.bar_bid=dict_bacteri.bac_id
left join dict_antibio on an_rlts.anr_aid=dict_antibio.anti_id
where bar_id = '{0}'
", pat_id);

            DataTable tbAllergy = dao.GetDataTable(sqlSelect, "an_rlts");

            return tbAllergy;
        }

        /// <summary>
        /// 获取涂片结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        DataTable GetBacResult(string pat_id)
        {


            string sqlSelect = string.Format(@"
select 
	bsr_id,
	bsr_cname

from cs_rlts with(nolock)
where bsr_id = '{0}'", pat_id);

            DataTable tbBac = dao.GetDataTable(sqlSelect, "cs_rlts");

            return tbBac;
        }
        #endregion
    }
}
