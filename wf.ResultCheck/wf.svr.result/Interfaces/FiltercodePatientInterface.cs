using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using dcl.svr.sample;
using System.Data;
using dcl.pub.entities;
using dcl.common;
using lis.dto;
using dcl.root.dac;
using dcl.svr.framedic;
using dcl.svr.cache;
using System.Text.RegularExpressions;

namespace dcl.svr.result
{
    /// <summary>
    /// 根据筛查号获取病人信息
    /// </summary>
    [System.ComponentModel.Description("根据筛查号获取病人信息")]
    public class FiltercodePatientInterface:IPatientInterface<InterfacePatientInfo>
    {
        #region IPatientInterface<InterfacePatientInfo> 成员

        public InterfacePatientInfo Get(string code, string interfaceID)
        {
            InterfacePatientInfo patInfo = null;

            //根据筛查号,查询病人信息
            try
            {
                DBHelper helper = new DBHelper();

                string sqlPat = string.Format(@"select top 1 patients_newborn.*,patients_ext.pat_birthday from patients_newborn 
left join patients_ext on patients_newborn.pat_id=patients_ext.pat_id
where patients_newborn.pat_upid='{0}' and patients_newborn.pat_flag in(2,4)", code);

                DataTable dtPat = helper.GetTable(sqlPat);

                if (dtPat != null && dtPat.Rows.Count > 0)
                {
                    patInfo = new InterfacePatientInfo();
                    DataRow drPat = dtPat.Rows[0];

                    #region 填充病人信息

                    //姓名
                    if (!Compare.IsNullOrDBNull(drPat["pat_name"]))
                    {
                        patInfo.Name = drPat["pat_name"].ToString();
                    }

                    //性别
                    if (!Compare.IsNullOrDBNull(drPat["pat_sex"]))
                    {
                        if (drPat["pat_sex"].ToString() == "男"
                            || drPat["pat_sex"].ToString() == "1")
                        {
                            patInfo.Sex = "1";
                        }
                        else if (drPat["pat_sex"].ToString() == "女"
                            || drPat["pat_sex"].ToString() == "2")
                        {
                            patInfo.Sex = "2";
                        }
                        else
                        {
                            //性别，1-男，2-女，0-未知
                            patInfo.Sex = "0";
                        }
                    }

                    //年龄
                    if (!Compare.IsNullOrDBNull(drPat["pat_age_exp"]))
                    {//目前只截取年
                        string age = drPat["pat_age_exp"].ToString();

                        patInfo.AgeValue = age;
                    }

                    //地址
                    if (!Compare.IsNullOrDBNull(drPat["pat_address"]))
                    {
                        patInfo.Address = drPat["pat_address"].ToString();
                    }

                    //联系电话
                    if (!Compare.IsNullOrDBNull(drPat["pat_tel"]))
                    {
                        patInfo.Tel = drPat["pat_tel"].ToString();
                    }

                    //标本类别
                    if (!Compare.IsEmpty(drPat["pat_sam_id"]))
                    {
                        patInfo.SampleID = drPat["pat_sam_id"].ToString();
                        //patInfo.SampleName = drBarcode[BarcodeTable.Patient.SampleName].ToString();
                    }

                    //病人来源id
                    if (!Compare.IsEmpty(drPat["pat_ori_id"]))
                    {
                        patInfo.Ori_id = drPat["pat_ori_id"].ToString();
                    }

                    //病人来源名称
                    //if (!Compare.IsEmpty(drBarcode["bc_ori_name"]))
                    //{
                    //    patInfo.Ori_name = drBarcode["bc_ori_name"].ToString();
                    //}

                    //病人id类型
                    if (!Compare.IsEmpty(drPat["pat_no_id"]))
                    {
                        patInfo.PatientsIDType = drPat["pat_no_id"].ToString();
                    }

                    //病人id
                    if (!Compare.IsEmpty(drPat["pat_in_no"]))
                    {
                        patInfo.PatientsID = drPat["pat_in_no"].ToString();
                    }

                    //病床号
                    if (!Compare.IsNullOrDBNull(drPat["pat_bed_no"]))
                    {
                        patInfo.BedNumber = drPat["pat_bed_no"].ToString();
                    }

                    //送检科室code
                    patInfo.SenderDeptCode = string.Empty;

                    if (Compare.IsNullOrDBNull(drPat["pat_dep_id"]))
                    {
                        patInfo.SenderDeptCode = drPat["pat_dep_id"].ToString();
                    }

                    //送检科室名称
                    if (!Compare.IsNullOrDBNull(drPat["pat_dep_name"]))
                    {
                        patInfo.SenderDeptName = drPat["pat_dep_name"].ToString();
                    }
                    else
                    {
                        patInfo.SenderDeptName = string.Empty;
                    }

                    //病区code
                    if (!Compare.IsNullOrDBNull(drPat["pat_ward_id"]))
                    {
                        patInfo.WardCode = drPat["pat_ward_id"].ToString();
                    }

                    patInfo.WardName = string.Empty;

                    //开单医生代码
                    if (!Compare.IsEmpty(drPat["pat_doc_id"]))//如果医生工号不为空
                    {
                        patInfo.SenderID = drPat["pat_doc_id"].ToString();
                    }

                    //开单医生姓名
                    if (!Compare.IsNullOrDBNull(drPat["pat_doc_name"]))
                    {
                        patInfo.SenderName = drPat["pat_doc_name"].ToString();
                    }

                    //临床诊断
                    if (!Compare.IsNullOrDBNull(drPat["pat_diag"]))
                    {
                        patInfo.DiagID = drPat["pat_diag"].ToString();
                    }

                    //标本备注
                    if (!Compare.IsEmpty(drPat["pat_sam_rem"]))
                    {
                        patInfo.SamRem = "新生儿筛查";

                        patInfo.SamRemName = "新生儿筛查";
                    }

                    //检查类型
                    if (!Compare.IsEmpty(drPat["pat_ctype"]))
                    {
                        patInfo.CheckType = drPat["pat_ctype"].ToString();
                    }
                    else
                    {
                        patInfo.CheckType = "1";
                    }

                    if (!Compare.IsNullOrDBNull(drPat["pat_social_no"]))
                    {
                        patInfo.SocialNo = drPat["pat_social_no"].ToString();
                    }

                    if (!Compare.IsNullOrDBNull(drPat["pat_pid"]))
                    {
                        patInfo.PatPid = drPat["pat_pid"].ToString();
                    }

                    //病人唯一号UPID
                    if (drPat.Table.Columns.Contains("pat_upid") && !Compare.IsNullOrDBNull(drPat["pat_upid"]))
                    {
                        patInfo.PatUPID = drPat["pat_upid"].ToString();
                    }

                    //出生日期
                    if (drPat.Table.Columns.Contains("pat_birthday") && (!Compare.IsNullOrDBNull(drPat["pat_birthday"])))
                    {
                        patInfo.birthday = (DateTime)drPat["pat_birthday"];
                    }

                    //就诊次数
                    if (!Compare.IsNullOrDBNull(drPat["pat_admiss_times"]))
                    {
                        int iAdmissTimes = 0;

                        if (int.TryParse(drPat["pat_admiss_times"].ToString(), out iAdmissTimes))
                        {
                            patInfo.AdmissTimes = iAdmissTimes;
                        }
                        else
                        {
                            patInfo.AdmissTimes = 0;
                        }
                    }
                    else
                    {
                        patInfo.AdmissTimes = 0;
                    }

                    #endregion

                    return patInfo;
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "筛查号获取病人信息", ex.ToString());
            }

            return null;
        }

        #endregion
    }
}
