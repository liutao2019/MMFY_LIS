using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.pub.entities;
//using dcl.svr.sample;
using lis.dto;
using dcl.common;

namespace dcl.svr.result
{
    /// <summary>
    /// 转换工具类
    /// </summary>
    public class ConvertUtil
    {
        public static InterfacePatientInfo ToPatients(DataSet dsSource)
        {
            if (dsSource == null) return null;

            DataTable dtPat = dsSource.Tables["PatInfo"];
            DataRow drPatInfo = dtPat.Rows[0];

            InterfacePatientInfo patInfo = new InterfacePatientInfo();

            //联系地址
            if (dtPat.Columns.Contains("pat_address") &&!Compare.IsNullOrDBNull(drPatInfo["pat_address"]))
            {
                patInfo.Address = drPatInfo["pat_address"].ToString();
            }

            ////年龄：分钟
            //if (!Compare.IsNullOrDBNull(drPatInfo["pat_age_int"]))
            //{
            //    patInfo.Age = Convert.ToInt32(drPatInfo["pat_age_int"]);
            //}

            //年龄：存储格式
            if (dtPat.Columns.Contains("pat_age_value") && !Compare.IsNullOrDBNull(drPatInfo["pat_age_value"]))
            {
                patInfo.AgeValue = drPatInfo["pat_age_value"].ToString();
            }

            //条码号
            if (!Compare.IsNullOrDBNull(drPatInfo["pat_bar_code"]))
            {
                patInfo.BarCode = drPatInfo["pat_bar_code"].ToString();
            }

            //检查目的ID
            if (dtPat.Columns.Contains("pat_chkpurpose_id") && !Compare.IsNullOrDBNull(drPatInfo["pat_chkpurpose_id"]))
            {
                patInfo.CheckPurposeID = drPatInfo["pat_chkpurpose_id"].ToString();
            }

            //检查目的名称
            if (dtPat.Columns.Contains("pat_chkpurpose_text") && !Compare.IsNullOrDBNull(drPatInfo["pat_chkpurpose_text"]))
            {
                patInfo.CheckPurposeText = drPatInfo["pat_chkpurpose_text"].ToString();
            }

            //病床号
            if (dtPat.Columns.Contains("pat_bedno") && !Compare.IsNullOrDBNull(drPatInfo["pat_bedno"]))
            {
                patInfo.BedNumber = drPatInfo["pat_bedno"].ToString();
            }

            //标本接收日期
            if (dtPat.Columns.Contains("date_sample_received") && !Compare.IsNullOrDBNull(drPatInfo["date_sample_received"]))
            {
                patInfo.DateReceived = (DateTime)drPatInfo["date_sample_received"];
            }

            //标本采集日期
            if (dtPat.Columns.Contains("date_sample_collect") && !Compare.IsNullOrDBNull(drPatInfo["date_sample_collect"]))
            {
                patInfo.DateSampleCollect = (DateTime)drPatInfo["date_sample_collect"];
            }

            //标本申请日期、医嘱时间
            if (drPatInfo.Table.Columns.Contains("pat_occ_date")
               && !Compare.IsNullOrDBNull(drPatInfo["pat_occ_date"]))
            {
                patInfo.DateApply = (DateTime)drPatInfo["pat_occ_date"];
            }

            //标本送检日期
            if (dtPat.Columns.Contains("date_sample_sended") && !Compare.IsNullOrDBNull(drPatInfo["date_sample_sended"]))
            {
                patInfo.DateSended = (DateTime)drPatInfo["date_sample_sended"];
            }

            //临床诊断ID
            if (dtPat.Columns.Contains("pat_diag_id") && !Compare.IsNullOrDBNull(drPatInfo["pat_diag_id"]))
            {
                patInfo.DiagID = drPatInfo["pat_diag_id"].ToString();
            }

            //临床诊断名称
            if (dtPat.Columns.Contains("pat_diag_text") && !Compare.IsNullOrDBNull(drPatInfo["pat_diag_text"]))
            {
                patInfo.DiagText = drPatInfo["pat_diag_text"].ToString();
            }

            //联系电子邮件
            if (dtPat.Columns.Contains("pat_email") && !Compare.IsNullOrDBNull(drPatInfo["pat_email"]))
            {
                patInfo.EMail = drPatInfo["pat_email"].ToString();
            }

            //病人身高
            if (dtPat.Columns.Contains("pat_height") && !Compare.IsNullOrDBNull(drPatInfo["pat_height"]))
            {
                patInfo.Height = drPatInfo["pat_height"].ToString();
            }

            //病人职业
            if (dtPat.Columns.Contains("pat_job") && !Compare.IsNullOrDBNull(drPatInfo["pat_job"]))
            {
                patInfo.Job = drPatInfo["pat_job"].ToString();
            }

            //病人姓名
            if (dtPat.Columns.Contains("pat_name") && !Compare.IsNullOrDBNull(drPatInfo["pat_name"]))
            {
                patInfo.Name = drPatInfo["pat_name"].ToString();
            }

            //病人工作单位
            if (dtPat.Columns.Contains("pat_workplace") && !Compare.IsNullOrDBNull(drPatInfo["pat_workplace"]))
            {
                patInfo.PlaceOfWork = drPatInfo["pat_workplace"].ToString();
            }

            //送检部门编号
            if (dtPat.Columns.Contains("sender_dept_id") && !Compare.IsNullOrDBNull(drPatInfo["sender_dept_id"]))
            {
                patInfo.SenderDeptCode = drPatInfo["sender_dept_id"].ToString();
            }

            //送检部门名称
            if (dtPat.Columns.Contains("sender_dept_name") && !Compare.IsNullOrDBNull(drPatInfo["sender_dept_name"]))
            {
                patInfo.SenderDeptName = drPatInfo["sender_dept_name"].ToString();
            }

            //送检医生编号
            if (dtPat.Columns.Contains("sender_id") && !Compare.IsNullOrDBNull(drPatInfo["sender_id"]))
            {
                patInfo.SenderID = drPatInfo["sender_id"].ToString();
            }

            //出生日期
            if (drPatInfo.Table.Columns.Contains("pat_birthday") && 
                !Compare.IsNullOrDBNull(drPatInfo["pat_birthday"]))
            {
                patInfo.birthday = Convert.ToDateTime(drPatInfo["pat_birthday"].ToString());
            }

            //送检医生名称
            if (dtPat.Columns.Contains("sender_name") && !Compare.IsNullOrDBNull(drPatInfo["sender_name"]))
            {
                patInfo.SenderName = drPatInfo["sender_name"].ToString();
            }

            //性别
            if (dtPat.Columns.Contains("pat_sex") && !Compare.IsNullOrDBNull(drPatInfo["pat_sex"]))
            {
                patInfo.Sex = drPatInfo["pat_sex"].ToString();
            }

            //病人联系电话
            if (dtPat.Columns.Contains("pat_tel") && !Compare.IsNullOrDBNull(drPatInfo["pat_tel"]))
            {
                patInfo.Tel = drPatInfo["pat_tel"].ToString();
            }

            //体重
            if (dtPat.Columns.Contains("pat_weight") && !Compare.IsNullOrDBNull(drPatInfo["pat_weight"]))
            {
                patInfo.Weight = drPatInfo["pat_weight"].ToString();
            }

            //费用类别
            if (dtPat.Columns.Contains("pat_fee_type") && !Compare.IsNullOrDBNull(drPatInfo["pat_fee_type"]))
            {
                patInfo.FeeType = drPatInfo["pat_fee_type"].ToString();
            }

            //体检号
            if (drPatInfo.Table.Columns.Contains("pat_emp_id")
                && !Compare.IsNullOrDBNull(drPatInfo["pat_emp_id"]))
            {
                patInfo.pat_emp_id = drPatInfo["pat_emp_id"].ToString();
            }

            //+++++++++edit by sink 2009-4-30 ++++++++++++
            //病人ID
            if (dtPat.Columns.Contains("pat_id") && !Compare.IsNullOrDBNull(drPatInfo["pat_id"]))
            {
                patInfo.PatientsID = drPatInfo["pat_id"].ToString();
            }

            if (dtPat.Columns.Contains("pat_social_no") && !Compare.IsNullOrDBNull(drPatInfo["pat_social_no"]))
            {
                patInfo.SocialNo = drPatInfo["pat_social_no"].ToString();
            }

            //体检号
            if (drPatInfo.Table.Columns.Contains("pat_app_no")
                && !Compare.IsNullOrDBNull(drPatInfo["pat_app_no"]))
            {
                patInfo.PatAppNo = drPatInfo["pat_app_no"].ToString();
            }
            //--------edit by sink 2009-4-30 end----------

            //+++++++++edit by sink 2010-6-17 ++++++++++++
            //自定义ID
            if (drPatInfo.Table.Columns.Contains("pat_pid") &&
                !Compare.IsNullOrDBNull(drPatInfo["pat_pid"]))
            {
                patInfo.PatPid = drPatInfo["pat_pid"].ToString();
            }

            //病人唯一号UPID
            if (drPatInfo.Table.Columns.Contains("pat_upid") && !Compare.IsNullOrDBNull(drPatInfo["pat_upid"]))
            {
                patInfo.PatUPID = drPatInfo["pat_upid"].ToString();
            }

            if (drPatInfo.Table.Columns.Contains("pat_com_his_code")
                && dsSource.Tables["PatInfo"].Rows.Count > 0
                && !Compare.IsNullOrDBNull(drPatInfo["pat_com_his_code"]))
            {
                List<EntityPatientsMi_4Barcode> combines = new List<EntityPatientsMi_4Barcode>();
                foreach (DataRow row in dsSource.Tables["PatInfo"].Rows)
                {
                    if (!Compare.IsNullOrDBNull(row["pat_com_his_code"]))
                    {
                        EntityPatientsMi_4Barcode combine = new EntityPatientsMi_4Barcode();
                        combine.pat_his_code = row["pat_com_his_code"].ToString();

                        if (drPatInfo.Table.Columns.Contains("advice_id")
                            && !Compare.IsEmpty("advice_id")
                            )//如果接口存在'医嘱号'这一列并且数据不为空
                        {
                            combine.pat_yz_id = row["advice_id"].ToString();
                        }

                        //如果接口存在'价格'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("com_price"))
                        {
                            decimal decPrice = 0;
                            if (decimal.TryParse(row["com_price"].ToString(), out decPrice))
                            {
                                combine.pat_com_price = decPrice;
                            }
                            else
                            {
                                combine.pat_com_price = null;
                            }
                        }

                        //如果接口存在'组合申请时间'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("pat_occ_date"))
                        {
                            DateTime dtOccDate;
                            if (DateTime.TryParse(row["pat_occ_date"].ToString(), out dtOccDate))
                            {
                                combine.com_occ_date = dtOccDate;
                            }
                            else
                            {
                                combine.com_occ_date = null;
                            }
                        }

                        //如果接口存在'临床诊断'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("pat_diag_text"))
                        {
                            combine.PatDiag = row["pat_diag_text"].ToString();
                        }

                        //如果接口存在'送检医生'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("sender_name"))
                        {
                            combine.DoctName = row["sender_name"].ToString();
                        }

                        //如果接口存在'送检医生ID'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("sender_id"))
                        {
                            combine.DoctID = row["sender_id"].ToString();
                        }

                        //如果接口存在'送检科室'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("sender_dept_name"))
                        {
                            combine.DeptName = row["sender_dept_name"].ToString();
                        }

                        //如果接口存在'送检科室'这一列并且数据不为空
                        if (drPatInfo.Table.Columns.Contains("sender_dept_id"))
                        {
                            combine.DeptCode = row["sender_dept_id"].ToString();
                        }

                        combines.Add(combine);
                    }
                }

                if (combines.Count > 0)
                    patInfo.PatientMi = combines;
            }
            return patInfo;
        }

        private static EntityPatientsMi_4Barcode ToAdvice(DataRow drAdvice)
        {
            throw new NotImplementedException();
        }
    }


}
