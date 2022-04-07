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

namespace dcl.svr.result
{
    public class CheckBloodTypeBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doOther(DataSet ds)
        {
            {
                DataSet dsResult = null;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtWhere"] != null)
                {
                    DataTable dtMeabo = null;
                    DataTable dtMehr = null;
                    for (int i = 0; i < ds.Tables["dtWhere"].Rows.Count; i++)
                    {
                        #region select

                        string strSqlWhere = ds.Tables["dtWhere"].Rows[i]["colWhere"].ToString();//添加
                        string colABO = ds.Tables["dtWhere"].Rows[i]["colABO"].ToString();//添加
                        string colRh = ds.Tables["dtWhere"].Rows[i]["colRh"].ToString();//添加

                        string strSQL1 = string.Format(@"select patients.pat_id,patients.pat_itr_id,dict_instrmt.itr_mid,
patients.pat_name,patients.pat_sid,patients.pat_bar_code,
patients.pat_host_order,resulto.res_chr as chr_abo_old,
resulto_plus.res_chr as chr_abo_nw,'' as chr_rh_old,'' as chr_rh_nw,
poweruserinfo1.userName as pat_report_name,
patients.pat_report_date,
poweruserinfo2.userName as blood_chk_name,
patients_ext.blood_chk_date
from resulto WITH (NOLOCK)
inner join patients on patients.pat_id=resulto.res_id
left join dict_instrmt on dict_instrmt.itr_id=patients.pat_itr_id
left join patients_ext on patients_ext.pat_id=patients.pat_id
left join resulto_plus on resulto_plus.res_id=resulto.res_id and resulto_plus.res_itm_id=resulto.res_itm_id
left join poweruserinfo as poweruserinfo1 on poweruserinfo1.loginId=patients.pat_report_code
left join poweruserinfo as poweruserinfo2 on poweruserinfo2.loginId=patients_ext.blood_chk_code
where 1=1 
{0} 
and resulto.res_itm_id='{1}' ", strSqlWhere, colABO);

                        string strSQL2 = string.Format(@"select  patients.pat_id,patients.pat_itr_id,
patients.pat_name,patients.pat_sid,patients.pat_bar_code,
patients.pat_host_order,resulto.res_chr as chr_rh_old,
resulto_plus.res_chr as chr_rh_nw
from resulto WITH (NOLOCK)
inner join patients on patients.pat_id=resulto.res_id
left join patients_ext on patients_ext.pat_id=patients.pat_id
left join resulto_plus on resulto_plus.res_id=resulto.res_id and resulto_plus.res_itm_id=resulto.res_itm_id
where 1=1 
{0} 
and resulto.res_itm_id='{1}' ", strSqlWhere, colRh);

                        try
                        {
                            DataTable dtrv1 = dao.GetDataTable(strSQL1, "dtblood");
                            DataTable dtrv2 = dao.GetDataTable(strSQL2, "dtRh");

                            if (dtMeabo == null) { dtMeabo = dtrv1.Clone(); }
                            if (dtMehr == null) { dtMehr = dtrv2.Clone(); }

                            if (dtrv1.Rows.Count > 0)
                            {
                                dtMeabo.Merge(dtrv1.Copy());
                            }

                            if (dtrv2.Rows.Count > 0)
                            {
                                dtMehr.Merge(dtrv2.Copy());
                            }
                        }
                        catch (Exception objEx)
                        {
                            dcl.root.logon.Logger.WriteException(this.GetType().Name, "查询复核血型信息", objEx.ToString());
                        }
                        #endregion
                    }

                    if (dtMeabo != null && dtMehr != null)
                    {
                        dsResult = new DataSet();
                        dsResult.Tables.Add(dtMeabo.Copy());
                        dsResult.Tables.Add(dtMehr.Copy());
                    }
                }

                return dsResult;
            }
        }

        public DataSet doSearch(DataSet ds)
        {
            DataSet dsResult = null;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtWhere"] != null)
            {
                DataTable dtMe = null;
                for (int i = 0; i < ds.Tables["dtWhere"].Rows.Count; i++)
                {
                    string strSqlWhere = ds.Tables["dtWhere"].Rows[i]["colWhere"].ToString();//添加
                    string colABO = ds.Tables["dtWhere"].Rows[i]["colABO"].ToString();//添加
                    string colRh = ds.Tables["dtWhere"].Rows[i]["colRh"].ToString();//添加

                    string strSQL = string.Format(@"select distinct patients.pat_id,patients.pat_itr_id,dict_instrmt.itr_mid,
patients.pat_name,patients.pat_sid,patients.pat_bar_code,patients.pat_in_no,patients.pat_date,
patients.pat_host_order,'' as chr_abo_nw,'' as chr_rh_nw
,(select top 1 res_chr from resulto as resABO where resABO.res_id=resulto.res_id and resABO.res_itm_id='{1}' and resABO.res_flag=1) as chr_abo_old
,(select top 1 res_chr from resulto as resABO where resABO.res_id=resulto.res_id and resABO.res_itm_id='{2}' and resABO.res_flag=1) as chr_rh_old
,'{1}' as itm_id_abo
,'{2}' as itm_id_rh
from resulto WITH (NOLOCK)
inner join patients on patients.pat_id=resulto.res_id
left join dict_instrmt on dict_instrmt.itr_id=patients.pat_itr_id
left join patients_ext on patients_ext.pat_id=patients.pat_id
where 1=1
{0} ", strSqlWhere, colABO, colRh);

                    try
                    {
                        DataSet dsResultTemp = dao.GetDataSet(strSQL);
                        dsResultTemp.Tables[0].TableName = "dtblood";

                        if (dtMe == null) { dtMe = dsResultTemp.Tables[0].Clone(); }

                        if (dsResultTemp.Tables[0].Rows.Count > 0)
                        {
                            dtMe.Merge(dsResultTemp.Tables[0].Copy());
                        }
                    }
                    catch (Exception objEx)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().Name, "获取要复核的血型信息", objEx.ToString());
                    }
                }

                if (dtMe != null)
                {
                    dsResult = new DataSet("ds");
                    dsResult.Tables.Add(dtMe.Copy());
                }
            }

            return dsResult;
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtSaveOKData = ds.Tables["dtsave"];
                DataTable dtuserinfo = ds.Tables["action"];

                DateTime blood_chk_date = DateTime.Now;
                string blood_chk_code = dtuserinfo.Rows[0]["action"].ToString();
                bool cbRh = dtuserinfo.Rows[0]["cb"].ToString() == "all";

                DBHelper helper = new DBHelper();
                foreach (DataRow drSave in dtSaveOKData.Rows)
                {
                    string itm_id_abo = drSave["itm_id_abo"].ToString();
                    string itm_id_rh = drSave["itm_id_rh"].ToString();
                    string chr_abo_nw = drSave["chr_abo_nw"].ToString();
                    string chr_rh_nw = drSave["chr_rh_nw"].ToString();
                    string pat_id = drSave["pat_id"].ToString();

                    string sqlDel = string.Format("delete from resulto_plus where res_id='{0}'", pat_id);
                    string sqlAdd = "";

                    if (cbRh)
                    {
                        sqlAdd = string.Format(@"INSERT INTO resulto_plus ([res_id],[res_itm_id],[res_chr]) VALUES('{0}','{1}','{2}');
INSERT INTO resulto_plus ([res_id],[res_itm_id],[res_chr]) VALUES('{0}','{3}','{4}')", pat_id, itm_id_abo, chr_abo_nw, itm_id_rh, chr_rh_nw);
                    }
                    else
                    {
                        sqlAdd = string.Format(@"INSERT INTO resulto_plus ([res_id],[res_itm_id],[res_chr]) VALUES('{0}','{1}','{2}')", pat_id, itm_id_abo, chr_abo_nw);
                    }

                    int iRowAffact = helper.ExecuteNonQuery(sqlDel);
                    iRowAffact = helper.ExecuteNonQuery(sqlAdd);

                    string[] patExtColName = new string[2];//列名
                    string[] patExtColValue = new string[2];//列值

                    patExtColName[0] = "blood_chk_code";//复核者
                    patExtColName[1] = "blood_chk_date";//复核时间
                    patExtColValue[0] = "'"+blood_chk_code+"'";//列值
                    patExtColValue[1] = "'"+blood_chk_date.ToString()+"'";//列值

                    PatInsertBLL bll = new PatInsertBLL();
                    bll.AddOrUpdatePatientExt(patExtColName, patExtColValue, pat_id);
                }
                return result;
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "", ex.ToString());
                result.Tables.Add(CommonBIZ.createErrorInfo("保存信息时出错!", ex.ToString())); ;
            }
            return result;
        }

        /// <summary>
        /// 获取历史结果
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet doView(DataSet ds)
        {
            DataSet dsResult = new DataSet("ds");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtblood"] != null)
            {
                List<string> strABOitmIDs = new List<string>();
                List<string> strRhitmIDs = new List<string>();

                string whereABoItmIDsIn = "";

                //系统配置：血型复核项目ID(ABO,Rh)
                string Lab_CheckBloodTypeItmIDs = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_CheckBloodTypeItmIDs");
                string[] strSplit = Lab_CheckBloodTypeItmIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (strSplit.Length > 0)
                {
                    foreach (string strItem in strSplit)
                    {
                        if (!string.IsNullOrEmpty(strItem))
                        {
                            string[] strSplit2 = strItem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            //大于1才记录
                            if (strSplit2.Length > 1 && !string.IsNullOrEmpty(strSplit2[0]) && !string.IsNullOrEmpty(strSplit2[1]))
                            {
                                strABOitmIDs.Add(strSplit2[0]);
                                strRhitmIDs.Add(strSplit2[1]);
                            }
                        }
                    }
                }

                //如果没有ABO项目则,不往下执行
                if (strABOitmIDs.Count <= 0)
                {
                    return dsResult;
                }

                for (int i = 0; i < strABOitmIDs.Count; i++)
                {
                    whereABoItmIDsIn += "'" + strABOitmIDs[i] + "',";
                }

                if (whereABoItmIDsIn.EndsWith(","))
                {
                    whereABoItmIDsIn = whereABoItmIDsIn.TrimEnd(new char[] { ',' });
                }

                DataTable dtbloodclone = ds.Tables["dtblood"].Clone();//克隆结构
                dtbloodclone.TableName = "dtbloodclone";

                for (int i = 0; i < ds.Tables["dtblood"].Rows.Count; i++)
                {
                    string str_patid = ds.Tables["dtblood"].Rows[i]["pat_id"].ToString();//
                    string str_in_no = ds.Tables["dtblood"].Rows[i]["pat_in_no"].ToString();//病人唯一号
                    string str_patDate = ds.Tables["dtblood"].Rows[i]["pat_date"].ToString();//报告日期
                    string str_chr_abo_old = ds.Tables["dtblood"].Rows[i]["chr_abo_old"].ToString();//现在结果

                    if (string.IsNullOrEmpty(str_patid) || string.IsNullOrEmpty(str_in_no)
                        || string.IsNullOrEmpty(str_patDate) || string.IsNullOrEmpty(str_chr_abo_old))
                    {
                        continue;//如果有个值为空,则跳出
                    }

                    string strSQL = string.Format(@"select top 1 resulto.res_key from patients
inner join resulto on resulto.res_id=patients.pat_id
where patients.pat_date<'{0}'
and patients.pat_in_no='{1}'
and patients.pat_id<>'{2}'
and resulto.res_flag=1
and resulto.res_chr='{3}'
and resulto.res_itm_id in({4})
 ", str_patDate, str_in_no, str_patid, str_chr_abo_old, whereABoItmIDsIn);

                    try
                    {
                        object ResultTemp = dao.DoScalar(strSQL);
                        //如果有历史结果,则添加此信息
                        if (ResultTemp != null && ResultTemp.ToString().Length > 0)
                        {
                            dtbloodclone.Rows.Add(ds.Tables["dtblood"].Rows[i].ItemArray);
                        }
                    }
                    catch (Exception objEx)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().Name, "获取历史结果", objEx.ToString());
                    }
                }

                if (dtbloodclone != null)
                {
                    dsResult.Tables.Add(dtbloodclone.Copy());
                }
            }

            return dsResult;
        }

        #endregion
    }
}
