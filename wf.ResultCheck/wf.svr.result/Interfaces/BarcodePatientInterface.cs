using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using dcl.svr.sample;
using System.Data;
using dcl.pub.entities;
using dcl.common;
using lis.dto;
using dcl.svr.sample;
using dcl.svr.framedic;
using dcl.svr.cache;
using System.Text.RegularExpressions;
using dcl.entity;

namespace dcl.svr.result
{
    public class BarcodePatientInterface : IPatientInterface<InterfacePatientInfo>
    {
        #region IPatientInterface 成员

        public InterfacePatientInfo Get(string code, string interfaceID)
        {
            SampMainBIZ sampBiz = new SampMainBIZ();
            EntitySampMain sampMain = sampBiz.SampMainQueryByBarId(code);


            //获取条码病人资料
            //BCPatientBIZ bllBCPatient = new BCPatientBIZ();
            //DataSet dsBarCodePatient = bllBCPatient.SearchByBarcode(code);

            //DataTable dtLISCombine = this.GetPatientCombineStruct();

            InterfacePatientInfo patInfo = null;

            if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarId))
            {
                patInfo = new InterfacePatientInfo();
                //DataRow sampMain = dsBarCodePatient.Tables[0].Rows[0];

                //姓名
                if (!string.IsNullOrEmpty(sampMain.PidName))
                {
                    patInfo.Name = sampMain.PidName;
                }

                //性别
                if (!string.IsNullOrEmpty(sampMain.PidSex))
                {
                    if (sampMain.PidSex == "男"
                        || sampMain.PidSex == "1")
                    {
                        patInfo.Sex = "1";
                    }
                    else if (sampMain.PidSex == "女"
                        || sampMain.PidSex == "2")
                    {
                        patInfo.Sex = "2";
                    }
                    else
                    {
                        //性别，1-男，2-女，0-未知
                        //patInfo.Sex = sampMain.PidSex.ToString();// edit 13-3-18 by ojf
                        patInfo.Sex = "0";
                    }
                }

                //年龄
                if (!string.IsNullOrEmpty(sampMain.PidAge))
                {//目前只截取年
                    string age = sampMain.PidAge.ToString();

                    if (age.ToLower().Contains('y')
                    && age.ToLower().Contains('m')
                    && age.ToLower().Contains('d')
                    && age.ToLower().Contains('h')
                    && age.ToLower().Contains('i')
                    )
                    {
                        patInfo.AgeValue = age;
                    }
                    else
                    {
                        int intAge;
                        age = age.Trim().Split('.')[0];
                        if (age != null && age.Length > 0)
                        {
                            if (
                                age.Contains("Y")
                                && age.Contains("M")
                                && age.Contains("D")
                                && age.Contains("H")
                                && age.Contains("I")
                                )
                            {

                            }
                            else if (int.TryParse(age, out intAge))
                            {
                                age = age + "Y0M0D0H0I";
                            }
                            else//老outlink
                            {
                                age = age.ToUpper().Replace('年', 'Y')
                                       .Replace('岁', 'Y')
                                       .Replace("个月", "M")
                                       .Replace('月', 'M')
                                       .Replace('日', 'D')
                                       .Replace('天', 'D')
                                       .Replace("小时", "H")
                                       .Replace('时', 'H')
                                       .Replace("分钟", "I")
                                       .Replace('分', 'I');

                                string patten = "(Y|D|M|H|I)";
                                string[] tmp = Regex.Split(age, patten);
                                string[] tmp2 = new string[tmp.Length];
                                int count = 0;
                                for (int i = 0; i < tmp.Length; i = i + 2)
                                {
                                    if (i + 1 >= tmp.Length)
                                        continue;
                                    tmp2[count] = tmp[i] + tmp[i + 1];
                                    count++;
                                }
                                string year = null;
                                string month = null;
                                string day = null;
                                string hour = null;
                                string minute = null;
                                foreach (string s in tmp2)
                                {
                                    if (string.IsNullOrEmpty(s))
                                        continue;

                                    if (s.Contains("Y") && year == null)
                                        year = s;

                                    if (s.Contains("M") && month == null)
                                        month = s;

                                    if (s.Contains("D") && day == null)
                                        day = s;

                                    if (s.Contains("H") && hour == null)
                                        hour = s;

                                    if (s.Contains("I") && minute == null)
                                        minute = s;
                                }
                                if (year == null) year = "0Y";
                                if (month == null) month = "0M";
                                if (day == null) day = "0D";
                                if (hour == null) hour = "0H";
                                if (minute == null) minute = "0I";
                                age = year + month + day + hour + minute;
                            }
                        }
                        patInfo.AgeValue = age;
                    }

                    #region 旧方法
                    ////目前只截取年
                    //string age = sampMain.PidAge.ToString();
                    //age = age.Trim().Split('.')[0];
                    //if (age != null && age.Length > 0)
                    //{
                    //    if (
                    //        age.Contains("Y")
                    //        && age.Contains("M")
                    //        && age.Contains("D")
                    //        && age.Contains("H")
                    //        && age.Contains("I") 
                    //        )
                    //    {

                    //    }
                    //    else if (age.IndexOf("年") >= 0)
                    //    {
                    //        if (age.EndsWith("年") || age.EndsWith("岁"))
                    //        {
                    //            age = age.Replace('年', 'Y').Replace('岁', 'Y') + "0M0D0H0I";
                    //        }
                    //        else
                    //        {
                    //            age = age.Replace('年', 'Y').Replace('月', 'M').Replace('日', 'D').Replace('时', 'H');//新outlink修改了返回年龄格式    2010-6-18 by li
                    //            age = age + "0I";
                    //        }
                    //    }
                    //    else//老outlink
                    //    {
                    //        if (age.EndsWith("年") || age.EndsWith("岁"))
                    //        {
                    //            age = age.Replace('年', 'Y').Replace('岁', 'Y') + "0M0D0H0I";
                    //        }
                    //        else if (age.EndsWith("月"))
                    //        {
                    //            age = "0Y" + age.Replace('月', 'M') + "0D0H0I";
                    //        }
                    //        else if (age.EndsWith("天") || age.EndsWith("日"))
                    //        {
                    //            age = "0Y0M" + age.Replace('天', 'D').Replace('日', 'D') + "0H0I";
                    //        }
                    //        else
                    //        {
                    //            age = age + "Y0M0D0H0I";
                    //        }
                    //    }
                    //}
                    //patInfo.AgeValue = age; 
                    #endregion
                }

                //地址
                if (sampMain.PidBirthday != null)
                {
                    patInfo.birthday = sampMain.PidBirthday.Value;
                }

                //地址
                if (!string.IsNullOrEmpty(sampMain.PidAddress))
                {
                    patInfo.Address = sampMain.PidAddress.ToString();
                }

                //联系电话
                if (!string.IsNullOrEmpty(sampMain.PidTel))
                {
                    patInfo.Tel = sampMain.PidTel.ToString();
                }

                //标本类别
                if (!Compare.IsEmpty(sampMain.SampSamId))
                {
                    patInfo.SampleID = sampMain.SampSamId.ToString();
                    patInfo.SampleName = sampMain.SampSamName.ToString();
                }

                //病人来源id
                if (!Compare.IsEmpty(sampMain.PidSrcId))
                {
                    patInfo.Ori_id = sampMain.PidSrcId.ToString();
                }

                //病人来源名称
                if (!Compare.IsEmpty(sampMain.PidSrcName))
                {
                    patInfo.Ori_name = sampMain.PidSrcName.ToString();
                }

                //病人id类型
                if (!Compare.IsEmpty(sampMain.PidIdtId))
                {
                    patInfo.PatientsIDType = sampMain.PidIdtId.ToString();
                }

                //病人id
                if (!Compare.IsEmpty(sampMain.PidInNo))
                {
                    patInfo.PatientsID = sampMain.PidInNo.ToString();
                }

                if (sampMain.SampPrintDate == null)
                {
                    sampMain.SampPrintDate = sampMain.SampDate;
                }

                //接收时间
                if (sampMain.ReceiverDate != null)
                {
                    patInfo.DateReceived = sampMain.ReceiverDate.Value;
                }

                string Lab_BarcodeTimeCal = "佛山市一";
                Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");

                if (Lab_BarcodeTimeCal == "佛山市一")
                {
                    //送检时间(收取时间)
                    if (sampMain.ReachDate != null)//送达时间 不为空
                    {
                        patInfo.DateSended = sampMain.ReachDate.Value;
                    }
                    else if (sampMain.ReceiverDate != null)//签收时间 不为空
                    {
                        patInfo.DateSended = sampMain.ReceiverDate.Value;
                    }
                    else
                    {
                        patInfo.DateSended = ServerDateTime.GetDatabaseServerDateTime();
                    }

                    //采血(采样)时间
                    //DateTime bc_blood_date = sampMain.CollectionDate.Value;
                    //patInfo.DateSampleCollect = bc_blood_date;
                    if (patInfo.Ori_id == "108")//住院
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = sampMain.CollectionDate.Value;
                            if (bc_blood_date < patInfo.DateSended.Value.AddMinutes(-90))
                            {
                                patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-90);
                            }
                            else if (bc_blood_date > patInfo.DateSended.Value.AddMinutes(-20))
                            {
                                patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-20);
                            }
                            else
                            {
                                patInfo.DateSampleCollect = bc_blood_date;
                            }
                        }
                    }
                    else if (patInfo.Ori_id == "107" || patInfo.Ori_id == "109")
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                        }
                        else
                        {
                            patInfo.DateSampleCollect = sampMain.SampPrintDate.Value;
                        }
                    }
                    else
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                        }
                        else
                        {
                            patInfo.DateSampleCollect = sampMain.SampPrintDate.Value;
                        }
                    }

                    //医嘱执行时间(申请时间)
                    if (sampMain.SampOccDate != null)
                    {
                        patInfo.DateApply = sampMain.SampOccDate;
                    }
                    else
                    {
                        patInfo.DateApply = patInfo.DateSampleCollect.Value;
                    }

                    if (patInfo.DateSampleCollect.Value < patInfo.DateApply.Value
                        && patInfo.DateApply.Value <= patInfo.DateSended.Value)
                    {
                        patInfo.DateSampleCollect = patInfo.DateApply.Value;
                    }
                }
                else if (Lab_BarcodeTimeCal == "清远人医")
                {
                    #region 清远人医-模式-时间计算
                    //送检时间(收取时间)
                    if (sampMain.SendDate != null)//送检时间 不为空
                    {
                        patInfo.DateSended = sampMain.SendDate.Value;
                    }
                    else if (sampMain.ReachDate != null)//送达时间 不为空
                    {
                        patInfo.DateSended = sampMain.ReachDate.Value;
                    }
                    else if (sampMain.ReceiverDate != null)//签收时间 不为空
                    {
                        patInfo.DateSended = sampMain.ReceiverDate.Value;
                    }
                    else
                    {
                        patInfo.DateSended = ServerDateTime.GetDatabaseServerDateTime();
                    }

                    //采血(采样)时间
                    //DateTime bc_blood_date = sampMain.CollectionDate.Value;
                    //patInfo.DateSampleCollect = bc_blood_date;
                    if (patInfo.Ori_id == "108")//住院
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = sampMain.CollectionDate.Value;
                            if (bc_blood_date < patInfo.DateSended.Value.AddMinutes(-90))
                            {
                                patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-90);
                            }
                            else if (bc_blood_date > patInfo.DateSended.Value.AddMinutes(-20))
                            {
                                patInfo.DateSampleCollect = patInfo.DateSended.Value.AddMinutes(-20);
                            }
                            else
                            {
                                patInfo.DateSampleCollect = bc_blood_date;
                            }
                        }
                    }
                    else if (patInfo.Ori_id == "107" || patInfo.Ori_id == "109")
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                        }
                        else
                        {
                            patInfo.DateSampleCollect = sampMain.SampPrintDate.Value;
                        }
                    }
                    else
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                        }
                        else
                        {
                            patInfo.DateSampleCollect = sampMain.SampPrintDate.Value;
                        }
                    }

                    //医嘱执行时间(申请时间)
                    if (sampMain.SampOccDate != null)
                    {
                        patInfo.DateApply = sampMain.SampOccDate;
                    }
                    else
                    {
                        patInfo.DateApply = patInfo.DateSampleCollect.Value;
                    }

                    if (patInfo.DateSampleCollect.Value < patInfo.DateApply.Value
                        && patInfo.DateApply.Value <= patInfo.DateSended.Value)
                    {
                        patInfo.DateSampleCollect = patInfo.DateApply.Value;
                    }
                    #endregion
                }
                else if (Lab_BarcodeTimeCal == "中山人医")
                {
                    #region 中山人医-模式-时间计算

                    if (sampMain.CollectionDate != null)//采样时间
                    {
                        patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                    }

                    //采集时间为空，则标本收取时间作为采集时间
                    if (string.IsNullOrEmpty(sampMain.CollectionDate.ToString())
                        && !string.IsNullOrEmpty(sampMain.SendDate.ToString()))
                    {
                        patInfo.DateSampleCollect = sampMain.SendDate.Value.AddMinutes(-new Random().Next(3, 9));
                    }

                    if (sampMain.SendDate != null)//送检时间(收取)
                    {
                        patInfo.DateSended = sampMain.SendDate.Value;
                    }

                    if (sampMain.ReceiverDate != null)//接收时间
                    {
                        patInfo.DateReceived = sampMain.ReceiverDate.Value;
                    }

                    if (sampMain.SampOccDate != null)//申请时间
                    {
                        patInfo.DateApply = sampMain.SampOccDate;
                    }

                    if (sampMain.ReachDate != null)//送达时间
                    {
                        patInfo.DateReach = sampMain.ReachDate.Value;
                    }

                    #endregion
                }
                else
                {
                    if (sampMain.CollectionDate != null)//采样时间
                    {
                        patInfo.DateSampleCollect = sampMain.CollectionDate.Value;
                    }

                    if (sampMain.SendDate != null)//送检时间
                    {
                        patInfo.DateSended = sampMain.SendDate.Value;
                    }

                    if (sampMain.ReceiverDate != null)//接收时间
                    {
                        patInfo.DateReceived = sampMain.ReceiverDate.Value;
                    }

                    if (sampMain.SampOccDate != null)//申请时间
                    {
                        patInfo.DateApply = sampMain.SampOccDate;
                    }

                    if (sampMain.ReachDate != null)//送达时间
                    {
                        patInfo.DateReach = sampMain.ReachDate.Value;
                    }
                }



                ////送检时间
                //if (!string.IsNullOrEmpty(sampMain[BarcodeTable.Patient.SampleSendDate]))
                //{
                //    patInfo.DateSended = (DateTime)sampMain[BarcodeTable.Patient.SampleSendDate];
                //}
                //else
                //{
                //    patInfo.DateSended = DateTime.Now;
                //}

                //条码
                if (!string.IsNullOrEmpty(sampMain.SampBarCode))
                {
                    patInfo.BarCode = sampMain.SampBarCode.ToString();
                }

                //病床号
                if (!string.IsNullOrEmpty(sampMain.PidBedNo))
                {
                    patInfo.BedNumber = sampMain.PidBedNo.ToString();
                }

                //送检科室code
                patInfo.SenderDeptCode = string.Empty;

                if (CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "通用"//)// SystemConfiguration.GetSystemConfig(""))
                                                                                         //&& SystemConfiguration.GetSystemConfig("GetPatientsInfoType") =="通用"
                    && !string.IsNullOrEmpty(sampMain.PidDeptCode))
                {
                    patInfo.SenderDeptCode = sampMain.PidDeptCode.ToString();
                }

                //送检科室名称
                if (!string.IsNullOrEmpty(sampMain.PidDeptName))
                {
                    patInfo.SenderDeptName = sampMain.PidDeptName.ToString();
                }
                else
                {
                    patInfo.SenderDeptName = string.Empty;
                }

                //病区code
                if (!string.IsNullOrEmpty(sampMain.PidDeptCode))
                {
                    patInfo.WardCode = sampMain.PidDeptCode.ToString();
                }
                patInfo.WardName = string.Empty;


                ////开单医生ID
                //if (!string.IsNullOrEmpty(sampMain.PidDoctorCode))
                //{
                //    patInfo.SenderID = new DictDoctor().GetDocIDByCode(sampMain.PidDoctorCode.ToString());// sampMain.PidDoctorCode.ToString();
                //}

                //if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
                //{
                //    //用医生工号获取医生ID
                //    patInfo.SenderID = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(sampMain.PidDoctorCode.ToString());
                //    // new DictDoctor().GetDocIDByCode(sampMain.PidDoctorCode.ToString());
                //}
                //else
                //{
                //    if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生ID
                //    {
                //        patInfo.SenderID = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(sampMain.PidDoctorName.ToString());
                //        // new DictDoctor().GetDocByName(sampMain.PidDoctorName.ToString());
                //    }
                //}

                if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
                {
                    //用医生工号获取医生ID
                    //patInfo.SenderID = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(sampMain.PidDoctorCode.ToString());
                    // new DictDoctor().GetDocIDByCode(sampMain.PidDoctorCode.ToString());

                    patInfo.SenderID = sampMain.PidDoctorCode.ToString();
                }
                else
                {
                    if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生ID
                    {
                        patInfo.SenderID = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(sampMain.PidDoctorName.ToString());
                        // new DictDoctor().GetDocByName(sampMain.PidDoctorName.ToString());
                    }
                }

                //开单医生姓名
                if (!string.IsNullOrEmpty(sampMain.PidDoctorName))
                {
                    patInfo.SenderName = sampMain.PidDoctorName.ToString();
                }

                //临床诊断
                if (!string.IsNullOrEmpty(sampMain.PidDiag))
                {
                    patInfo.DiagID = sampMain.PidDiag.ToString();
                }

                ///标本备注
                if (!Compare.IsEmpty(sampMain.SampRemark))
                {
                    patInfo.SamRem = sampMain.SampRemark.ToString();

                    patInfo.SamRemName = sampMain.SampRemark.ToString();
                }


                //条码状态
                if (!Compare.IsEmpty(sampMain.SampStatusId))
                {
                    patInfo.BarcodeStatus = sampMain.SampStatusId.ToString();
                }

                if (!Compare.IsEmpty(sampMain.SampPrintFlag))
                {
                    patInfo.bc_print_flag = Convert.ToInt32(sampMain.SampPrintFlag);
                }

                //检查类型
                if (sampMain.SampUrgentFlag != null && Convert.ToBoolean(sampMain.SampUrgentFlag) == true)
                {
                    patInfo.CheckType = "2";
                }
                else
                {
                    patInfo.CheckType = "1";
                }

                if (!string.IsNullOrEmpty(sampMain.PidSocialNo))
                {
                    patInfo.SocialNo = sampMain.PidSocialNo.ToString();
                }

                if (!string.IsNullOrEmpty(sampMain.SampApplyNo))
                {
                    patInfo.PatAppNo = sampMain.SampApplyNo.ToString();
                }


                //+++++++++edit by sink 2010-9-13 ++++++++++++
                //自定义ID
                if (!string.IsNullOrEmpty(sampMain.PidPatno))
                {
                    patInfo.PatPid = sampMain.PidPatno.ToString();
                }

                //病人唯一号UPID
                if (!string.IsNullOrEmpty(sampMain.PidUniqueId))
                {
                    patInfo.PatUPID = sampMain.PidUniqueId.ToString();
                }

                //人员身份
                patInfo.PatIdentity = sampMain.PidIdentity.ToString();


                //体检id
                patInfo.pat_emp_id = sampMain.PidExamNo.ToString();


                //体检单位名称
                patInfo.pat_emp_company_name = sampMain.PidExamCompany;


                //就诊次数
                int iAdmissTimes = 0;

                if (int.TryParse(sampMain.PidAdmissTimes.ToString(), out iAdmissTimes))
                {
                    patInfo.AdmissTimes = iAdmissTimes;
                }
                else
                {
                    patInfo.AdmissTimes = 0;
                }


                //如果体检ID不为空，则更新病人来源为体检
                if (!Compare.IsEmpty(sampMain.PidExamNo))
                {
                    patInfo.Ori_id = "109";
                    patInfo.Ori_name = "体检";
                }


                int com_seq = 0;
                foreach (EntitySampDetail drBCCombine in sampMain.ListSampDetail)//条码检验组合转换为LIS中的病人检验组合
                {
                    EntityPatientsMi_4Barcode patCom = new EntityPatientsMi_4Barcode();

                    //组合ID
                    if (!string.IsNullOrEmpty(drBCCombine.ComId))
                    {
                        patCom.pat_com_id = drBCCombine.ComId.ToString();
                    }

                    //组合HIS编码
                    if (!string.IsNullOrEmpty(drBCCombine.OrderCode))
                    {
                        patCom.pat_his_code = drBCCombine.OrderCode.ToString();
                    }

                    //价格
                    patCom.pat_com_price = drBCCombine.OrderPrice;


                    //医嘱ID
                    if (!string.IsNullOrEmpty(drBCCombine.OrderSn))
                    {
                        patCom.pat_yz_id = drBCCombine.OrderSn.ToString();
                    }

                    //组合名称
                    if (!string.IsNullOrEmpty(drBCCombine.ComName))
                    {
                        patCom.pat_com_name = drBCCombine.ComName.ToString();
                    }

                    //条码登记信息
                    patCom.bc_flag = Convert.ToInt32(drBCCombine.SampFlag);


                    //费用类别
                    if (!Compare.IsEmpty(sampMain.PidInsuId))
                    {
                        patInfo.FeeType = sampMain.PidInsuId.ToString();
                    }

                    patCom.DeptCode = sampMain.PidDeptCode.ToString();
                    //送检科室名称
                    if (!string.IsNullOrEmpty(sampMain.PidDeptName))
                    {
                        patCom.DeptName = sampMain.PidDeptName.ToString();
                    }
                    if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
                    {
                        patCom.DoctID = sampMain.PidDoctorCode.ToString();
                    }
                    else
                    {
                        if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生ID
                        {
                            patCom.DoctID = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(sampMain.PidDoctorName.ToString());
                        }
                    }

                    //开单医生姓名
                    if (!string.IsNullOrEmpty(sampMain.PidDoctorName))
                    {
                        patCom.DoctName = sampMain.PidDoctorName.ToString();
                    }

                    //临床诊断
                    if (!string.IsNullOrEmpty(sampMain.PidDiag))
                    {
                        patCom.PatDiag = sampMain.PidDiag.ToString();
                    }
                    patCom.pat_seq = com_seq;

                    patInfo.PatientMi.Add(patCom);

                    patCom.pat_bar_code = code;

                    com_seq++;
                }
                return patInfo;
            }

            return null;
        }

        #endregion

        public string GetBarcodeBySamIdAndItrId(string SamId, string ItrId)
        {
            string strbarcode = "";
            string strSQL = @"SELECT TOP 1 pat_bar_code FROM patients WHERE   pat_sid=? AND pat_itr_id=? ORDER BY pat_date desc";
            try
            {

                Lib.DAC.SqlHelper helper = new Lib.DAC.SqlHelper();
                Lib.DAC.DbCommandEx cmd = helper.CreateCommandEx(strSQL);
                cmd.AddParameterValue(SamId);
                cmd.AddParameterValue(ItrId);
                DataTable dtbResult = helper.GetTable(cmd);
                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    strbarcode = dtbResult.Rows[0]["pat_bar_code"].ToString();
                }

            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, string.Format("Search,sql= {0}", strSQL), ex.StackTrace);
                throw;
            }

            return strbarcode;
        }
    }
}
