using dcl.common;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using dcl.svr.interfaces;
using dcl.svr.sample;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace dcl.svr.result
{
    public class PidReportMainInterface : IPidReportMainInterface
    {
        public EntityPidReportMain GetPatientFromInterface(EntityInterfaceExtParameter parameter)
        {
            EntityPidReportMain patient = null;
            try
            {
                if (parameter.DownloadType == InterfaceType.BarcodePatient)
                {
                    PidReportMainBIZ reportMainBiz = new PidReportMainBIZ();
                    patient = reportMainBiz.GetPatientsByBarCode(parameter.PatientID);
                }
                else
                {
                    List<EntityInterfaceData> listInterfaceData = DCLExtInterfaceFactory.DCLExtInterface.DownloadPatientInfo(parameter);

                    if (listInterfaceData.Count > 0)
                    {
                        EntityInterfaceData interfaceData = listInterfaceData[0];
                        //拿到接口的对照信息
                        ContrastDefineBIZ contBiz = new ContrastDefineBIZ();
                        List<EntitySysItfContrast> listContrast = contBiz.GetSysContrast(interfaceData.InterfaceID);
                        if (interfaceData.InterfaceData != null &&
                            interfaceData.InterfaceData.Tables.Count > 0)
                        {
                            DataTable dtPatient = ConvertToLis(interfaceData.InterfaceData.Tables[0], listContrast);

                            if (dtPatient.Rows.Count > 0)
                            {
                                patient = EntityManager<EntityPidReportMain>.ConvertToEntityByMapName(dtPatient.Rows[0], "clab");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return patient;
        }
        private DataTable ConvertToLis(DataTable hisData, List<EntitySysItfContrast> listContrast)
        {
            //创建转换后的条码表
            DataTable dtSampMain = new DataTable();
            foreach (EntitySysItfContrast item in listContrast)
            {
                if (!string.IsNullOrEmpty(item.ContInterfaceColumn))
                {
                    dtSampMain.Columns.Add(item.ContSysColumn);
                }
            }
            EntityDicCombine combine = new EntityDicCombine();
            //数据转换到检验表中
            foreach (DataRow hisRow in hisData.Rows)
            {
                DataRow lisRow = dtSampMain.NewRow();
                foreach (EntitySysItfContrast item in listContrast)
                {
                    if (!string.IsNullOrEmpty(item.ContInterfaceColumn))
                    {
                        if (!string.IsNullOrEmpty(item.ContColumnRule))
                        {
                            //规则转换
                            IRule rule = IRule.CreateRule(item.ContColumnRule);
                            lisRow[item.ContSysColumn] = rule.ConvertRule(hisRow[item.ContInterfaceColumn].ToString());
                        }
                        else
                            lisRow[item.ContSysColumn] = hisRow[item.ContInterfaceColumn];
                    }
                }

                dtSampMain.Rows.Add(lisRow);

            }
            return dtSampMain;
        }

        #region 待清理
        /// <summary>
        /// 条码病人表资料转换为病人资料表
        /// </summary>
        /// <param name="sampMain"></param>
        /// <param name="dtLisPat"></param>
        //private EntityPidReportMain ConvertSampMainToPatient(EntitySampMain sampMain)
        //{
        //    EntityPidReportMain drPatInfo = new EntityPidReportMain();

        //    //姓名
        //    drPatInfo.PidName = sampMain.PidName;

        //    //性别
        //    string sex = "0";
        //    if (!string.IsNullOrEmpty(sampMain.PidSex))
        //    {
        //        if (sampMain.PidSex.ToString() == "男"
        //            || sampMain.PidSex.ToString() == "1")
        //        {
        //            sex = "1";
        //        }
        //        else if (sampMain.PidSex.ToString() == "女"
        //            || sampMain.PidSex.ToString() == "2"
        //            )
        //        {
        //            sex = "2";
        //        }
        //        else
        //        {
        //            sex = string.Empty;
        //        }
        //    }

        //    drPatInfo.PidSex = sex;

        //    //年龄
        //    if (sampMain.PidAge != null && !string.IsNullOrEmpty(sampMain.PidAge))
        //    {
        //        //目前只截取年
        //        string age = sampMain.PidAge.ToString();

        //        if (age.ToLower().Contains('y')
        //        && age.ToLower().Contains('m')
        //        && age.ToLower().Contains('d')
        //        && age.ToLower().Contains('h')
        //        && age.ToLower().Contains('i')
        //        )
        //        {
        //            drPatInfo.PidAgeExp = age;
        //        }
        //        else
        //        {
        //            int intAge;
        //            age = age.Trim().Split('.')[0];
        //            if (age != null && age.Length > 0)
        //            {
        //                if (
        //                    age.Contains("Y")
        //                    && age.Contains("M")
        //                    && age.Contains("D")
        //                    && age.Contains("H")
        //                    && age.Contains("I")
        //                    )
        //                {

        //                }
        //                else if (int.TryParse(age, out intAge))
        //                {
        //                    age = age + "Y0M0D0H0I";
        //                }
        //                else//老outlink
        //                {
        //                    age = age.ToUpper().Replace('年', 'Y')
        //                           .Replace('岁', 'Y')
        //                           .Replace("个月", "M")
        //                           .Replace('月', 'M')
        //                           .Replace('日', 'D')
        //                           .Replace('天', 'D')
        //                           .Replace("小时", "H")
        //                           .Replace('时', 'H')
        //                           .Replace("分钟", "I")
        //                           .Replace('分', 'I');

        //                    string patten = "(Y|D|M|H|I)";
        //                    string[] tmp = Regex.Split(age, patten);
        //                    string[] tmp2 = new string[tmp.Length];
        //                    int count = 0;
        //                    for (int i = 0; i < tmp.Length; i = i + 2)
        //                    {
        //                        if (i + 1 >= tmp.Length)
        //                            continue;
        //                        tmp2[count] = tmp[i] + tmp[i + 1];
        //                        count++;
        //                    }
        //                    string year = null;
        //                    string month = null;
        //                    string day = null;
        //                    string hour = null;
        //                    string minute = null;
        //                    foreach (string s in tmp2)
        //                    {
        //                        if (string.IsNullOrEmpty(s))
        //                            continue;

        //                        if (s.Contains("Y") && year == null)
        //                            year = s;

        //                        if (s.Contains("M") && month == null)
        //                            month = s;

        //                        if (s.Contains("D") && day == null)
        //                            day = s;

        //                        if (s.Contains("H") && hour == null)
        //                            hour = s;

        //                        if (s.Contains("I") && minute == null)
        //                            minute = s;
        //                    }
        //                    if (year == null) year = "0Y";
        //                    if (month == null) month = "0M";
        //                    if (day == null) day = "0D";
        //                    if (hour == null) hour = "0H";
        //                    if (minute == null) minute = "0I";
        //                    age = year + month + day + hour + minute;
        //                }
        //            }
        //            drPatInfo.PidAgeExp = age;
        //        }
        //    }

        //    //病人来源
        //    drPatInfo.SrcName = sampMain.PidSrcName;
        //    drPatInfo.PidSrcId = sampMain.PidSrcId;
        //    //所属院区
        //    if (!string.IsNullOrEmpty(sampMain.PidOrgId))
        //    {
        //        drPatInfo.PidOrgId = sampMain.PidOrgId;
        //    }

        //    if (!string.IsNullOrEmpty(drPatInfo.PidAgeExp))
        //        drPatInfo.PidAge = AgeConverter.AgeValueTextToMinute(drPatInfo.PidAgeExp.ToString());

        //    if (sampMain.ReachDate != null)//送达时间
        //    {
        //        drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
        //    }

        //    DateTime now = ServerDateTime.GetDatabaseServerDateTime();
        //    string Lab_BarcodeTimeCal = "佛山市一";
        //    Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");

        //    if (Lab_BarcodeTimeCal == "佛山市一")
        //    {
        //        #region 佛山市一
        //        //如果打印时间为空，就用条码生成日期作为打印时间
        //        if (sampMain.SampPrintDate == null)
        //        {
        //            sampMain.SampPrintDate = sampMain.SampDate;
        //        }

        //        //送检时间
        //        if (!Compare.IsEmpty(sampMain.ReachDate))//送达时间 不为空
        //        {
        //            //如果送达时间不为空，将送达时间赋值给送检时间
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReachDate);
        //        }
        //        else if (!Compare.IsEmpty(sampMain.ReceiverDate))//签收时间 不为空
        //        {
        //            //如果签收时间不为空，将签收时间赋值给送检时间
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReceiverDate);
        //        }
        //        else
        //        {
        //            drPatInfo.SampSendDate = now;
        //        }

        //        //采样时间
        //        if (!Compare.IsEmpty(drPatInfo.PidSrcId))
        //        {
        //            string ori_id = drPatInfo.PidSrcId.ToString();
        //            DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

        //            //采血(采样)时间
        //            if (ori_id == "108")//住院
        //            {
        //                if (Compare.IsEmpty(sampMain.CollectionDate))
        //                {
        //                    drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
        //                }
        //                else
        //                {
        //                    DateTime bc_blood_date = Convert.ToDateTime(sampMain.CollectionDate);
        //                    if (bc_blood_date < dtSended.AddMinutes(-90))
        //                    {
        //                        drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
        //                    }
        //                    else if (bc_blood_date > dtSended.AddMinutes(-20))
        //                    {
        //                        drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
        //                    }
        //                    else
        //                    {
        //                        drPatInfo.SampCollectionDate = bc_blood_date;
        //                    }
        //                }
        //            }
        //            else if (ori_id == "107" || ori_id == "109")
        //            {
        //                if (sampMain.CollectionDate != null)
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //                }
        //                else
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //                }
        //            }
        //            else
        //            {
        //                if (sampMain.CollectionDate != null)
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //                }
        //                else
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (sampMain.CollectionDate != null)
        //            {
        //                drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //            }
        //            else
        //            {
        //                drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //            }
        //        }
        //        //如果条码医嘱时间为空，将样本采集时间赋值给条码医嘱时间
        //        if (sampMain.SampOccDate == null)
        //        {
        //            sampMain.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
        //        }

        //        //医嘱执行时间(申请时间)
        //        drPatInfo.SampReceiveDate = sampMain.SampOccDate;
        //        if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
        //            && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
        //            )
        //        {
        //            drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
        //        }
        //        #endregion
        //    }
        //    else if (Lab_BarcodeTimeCal == "清远人医")
        //    {
        //        #region 清远人医
        //        //如果打印时间为空，就用条码生成日期作为打印时间
        //        if (sampMain.SampPrintDate == null)
        //        {
        //            sampMain.SampPrintDate = sampMain.SampDate;
        //        }

        //        //送检时间
        //        if (!Compare.IsEmpty(sampMain.ReachDate))//送达时间 不为空
        //        {
        //            //如果送达时间不为空，将送达时间赋值给送检时间
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReachDate);
        //        }
        //        else if (!Compare.IsEmpty(sampMain.ReceiverDate))//签收时间 不为空
        //        {
        //            //如果签收时间不为空，将签收时间赋值给送检时间
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReceiverDate);
        //        }
        //        else if (!Compare.IsEmpty(sampMain.SendDate))//送检时间 不为空
        //        {
        //            //如果送检时间不为空，将送检时间赋值给送检时间
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
        //        }
        //        else
        //        {
        //            drPatInfo.SampSendDate = now;
        //        }


        //        ////采样时间
        //        if (!Compare.IsEmpty(drPatInfo.PidSrcId))
        //        {
        //            string ori_id = drPatInfo.PidSrcId.ToString();
        //            DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

        //            //采血(采样)时间
        //            if (ori_id == "108")//住院
        //            {
        //                if (Compare.IsEmpty(sampMain.CollectionDate))
        //                {
        //                    drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
        //                }
        //                else
        //                {
        //                    DateTime bc_blood_date = Convert.ToDateTime(sampMain.CollectionDate);
        //                    if (bc_blood_date < dtSended.AddMinutes(-90))
        //                    {
        //                        drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
        //                    }
        //                    else if (bc_blood_date > dtSended.AddMinutes(-20))
        //                    {
        //                        drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
        //                    }
        //                    else
        //                    {
        //                        drPatInfo.SampCollectionDate = bc_blood_date;
        //                    }
        //                }
        //            }
        //            else if (ori_id == "107" || ori_id == "109")
        //            {
        //                if (sampMain.CollectionDate != null)
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //                }
        //                else
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //                }
        //            }
        //            else
        //            {
        //                if (sampMain.CollectionDate != null)
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //                }
        //                else
        //                {
        //                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (sampMain.CollectionDate != null)
        //            {
        //                drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //            }
        //            else
        //            {
        //                drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
        //            }
        //        }

        //        if (sampMain.SampOccDate == null)
        //        {
        //            sampMain.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
        //        }

        //        //医嘱执行时间(申请时间)
        //        drPatInfo.SampReceiveDate = sampMain.SampOccDate;
        //        if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
        //            && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
        //            )
        //        {
        //            drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
        //        }
        //        #endregion
        //    }
        //    else if (Lab_BarcodeTimeCal == "中山人医")
        //    {
        //        #region 中山人医
        //        //如果打印时间为空，就用条码生成日期作为打印时间
        //        if (sampMain.SampPrintDate == null)
        //        {
        //            sampMain.SampPrintDate = sampMain.SampDate;
        //        }

        //        if (sampMain.CollectionDate != null)//采样时间
        //        {
        //            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //        }

        //        //采集时间为空，则标本收取时间作为采集时间
        //        if (string.IsNullOrEmpty(sampMain.CollectionDate.ToString())
        //            && !string.IsNullOrEmpty(sampMain.SendDate.ToString()))
        //        {
        //            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SendDate).AddMinutes(-new Random().Next(3, 9));
        //        }

        //        if (sampMain.SendDate != null)//送检时间(收取)
        //        {
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
        //        }

        //        if (sampMain.ReceiverDate != null)//接收时间
        //        {
        //            drPatInfo.SampApplyDate = Convert.ToDateTime(sampMain.ReceiverDate);
        //        }

        //        if (sampMain.SampOccDate != null)//申请时间
        //        {
        //            drPatInfo.SampReceiveDate = Convert.ToDateTime(sampMain.SampOccDate);
        //        }

        //        if (sampMain.ReachDate != null)//送达时间
        //        {
        //            drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
        //        }

        //        #endregion
        //    }
        //    else
        //    {
        //        if (sampMain.CollectionDate != null)//采样时间
        //        {
        //            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
        //        }

        //        if (sampMain.SendDate != null)//送检时间(收取)
        //        {
        //            drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
        //        }

        //        if (sampMain.ReceiverDate != null)//接收时间
        //        {
        //            drPatInfo.SampApplyDate = Convert.ToDateTime(sampMain.ReceiverDate);
        //        }

        //        if (sampMain.SampOccDate != null)//申请时间
        //        {
        //            drPatInfo.SampReceiveDate = Convert.ToDateTime(sampMain.SampOccDate);
        //        }

        //        if (sampMain.ReachDate != null)//送达时间
        //        {
        //            drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
        //        }

        //    }
        //    int iAdmissTimes = 0;

        //    if (int.TryParse(sampMain.PidAdmissTimes.ToString(), out iAdmissTimes))
        //    {
        //        drPatInfo.PidAddmissTimes = iAdmissTimes;
        //    }
        //    else
        //    {
        //        drPatInfo.PidAddmissTimes = 0;
        //    }

        //    //标本备注
        //    drPatInfo.SampRemark = sampMain.SampRemark;

        //    //ID类型
        //    drPatInfo.PidIdtId = sampMain.PidIdtId;

        //    //接收时间
        //    drPatInfo.SampApplyDate = sampMain.ReceiverDate;

        //    drPatInfo.SampCheckDate = now;

        //    //条码
        //    drPatInfo.RepBarCode = sampMain.SampBarCode;

        //    //病床号
        //    drPatInfo.PidBedNo = sampMain.PidBedNo;

        //    //ID
        //    drPatInfo.PidInNo = sampMain.PidInNo;

        //    //病区code
        //    drPatInfo.PidWardId = sampMain.PidDeptCode;

        //    //病区名称
        //    drPatInfo.PidWardName = string.Empty;

        //    //送检科室名称
        //    drPatInfo.PidDeptName = sampMain.PidDeptName;

        //    drPatInfo.PidSocialNo = sampMain.PidSocialNo;

        //    //送检科室code
        //    drPatInfo.PidDeptId = string.Empty;

        //    if ((CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "通用"
        //        || CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "outlink")
        //        && !Compare.IsNullOrDBNull(sampMain.PidDeptCode))
        //    {
        //        drPatInfo.PidDeptId = sampMain.PidDeptCode.ToString();
        //    }
        //    //联系地址
        //    drPatInfo.PidAddress = sampMain.PidAddress;

        //    //联系电话
        //    drPatInfo.PidTel = sampMain.PidTel;

        //    if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
        //    {
        //        drPatInfo.PidDoctorCode = sampMain.PidDoctorCode.ToString();
        //    }
        //    else
        //    {
        //        if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生code
        //        {
        //            drPatInfo.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(sampMain.PidDoctorName.ToString());
        //            //new DictDoctor().GetDocByName(sampMain.PidDoctorName.ToString());
        //        }
        //    }

        //    //开单医生姓名
        //    drPatInfo.PidDocName = sampMain.PidDoctorName;
        //    drPatInfo.DoctorName = sampMain.PidDoctorName;

        //    //临床诊断
        //    drPatInfo.PidDiag = sampMain.PidDiag;

        //    if (sampMain.PidBirthday != null)//出生日期
        //    {
        //        drPatInfo.PidBirthday = Convert.ToDateTime(sampMain.PidBirthday);
        //    }

        //    //条码状态
        //    drPatInfo.BcStatus = sampMain.SampStatusId;

        //    drPatInfo.SampType = sampMain.SampType;
        //    //打印标志
        //    drPatInfo.SampPrintFlag = sampMain.SampPrintFlag.ToString();

        //    //标本类别
        //    drPatInfo.SamName = sampMain.SampSamName;
        //    drPatInfo.PidSamId = sampMain.SampSamId;
        //    //医院ID
        //    drPatInfo.PidOrgId = sampMain.PidOrgId;

        //    //检查类型
        //    if (sampMain.SampUrgentFlag)
        //    {
        //        drPatInfo.RepCtype = "2";
        //    }
        //    else
        //    {
        //        drPatInfo.RepCtype = "1";
        //    }

        //    //检查类型
        //    if (sampMain.SampUrgentStatus.ToString() == "2")
        //    {
        //        drPatInfo.RepCtype = "4";
        //    }
        //    //+++++++++ 2010-9-26 ++++++++++++
        //    //自定义ID
        //    if (!Compare.IsNullOrDBNull(sampMain.PidPatno))
        //    {
        //        drPatInfo.RepInputId = sampMain.PidPatno.ToString();
        //    }

        //    //唯一号UPID 目前滨海使用
        //    if (!Compare.IsNullOrDBNull(sampMain.PidUniqueId))
        //    {
        //        drPatInfo.PidUniqueId = sampMain.PidUniqueId.ToString();
        //    }

        //    //人员身份
        //    if (!Compare.IsNullOrDBNull(sampMain.PidIdentity))
        //    {
        //        drPatInfo.PidIdentity = sampMain.PidIdentity;
        //    }
        //    //身份证
        //    drPatInfo.PidIdentityCard = sampMain.PidIdentityCard;
        //    //病人身份
        //    if (!Compare.IsNullOrDBNull(sampMain.PidIdentityName))
        //    {
        //        drPatInfo.PidIdentityName = sampMain.PidIdentityName;
        //    }
        //    //保存拆分大组合(特殊合并)ID
        //    if (!Compare.IsNullOrDBNull(sampMain.SampMergeComId))
        //    {
        //        drPatInfo.BcMergeComid = sampMain.SampMergeComId.ToString();
        //    }

        //    //申请单号
        //    if (!Compare.IsNullOrDBNull(sampMain.SampApplyNo)
        //        )
        //    {
        //        drPatInfo.PidApplyNo = sampMain.SampApplyNo.ToString();
        //    }

        //    //费用类别
        //    drPatInfo.PidInsuId = sampMain.PidInsuId;

        //    //体检id
        //    drPatInfo.PidExamNo = sampMain.PidExamNo;

        //    //体检单位id
        //    if (!string.IsNullOrEmpty(sampMain.PidExamCompany))
        //    {
        //        drPatInfo.PidExamCompany = sampMain.PidExamCompany;
        //    }
        //    else
        //    {
        //        drPatInfo.PidExamCompany = sampMain.PidExamCompanyName;
        //    }
        //    //如果体检ID不为空，则更新病人来源为体检
        //    if (!Compare.IsEmpty(sampMain.PidExamNo))
        //    {
        //        drPatInfo.PidSrcId = "109";
        //        drPatInfo.SrcName = "体检";
        //    }
        //    int com_seq = 0;
        //    foreach (EntitySampDetail drBCCombine in sampMain.ListSampDetail)//条码检验组合转换为LIS中的病人检验组合
        //    {
        //        EntityPidReportDetail patCom = new EntityPidReportDetail();

        //        //组合ID
        //        if (!string.IsNullOrEmpty(drBCCombine.ComId))
        //        {
        //            patCom.ComId = drBCCombine.ComId.ToString();
        //        }

        //        //组合HIS编码
        //        if (!string.IsNullOrEmpty(drBCCombine.OrderCode))
        //        {
        //            patCom.OrderCode = drBCCombine.OrderCode.ToString();
        //        }

        //        //价格
        //        patCom.OrderPrice = drBCCombine.OrderPrice.ToString();


        //        //医嘱ID
        //        if (!string.IsNullOrEmpty(drBCCombine.OrderSn))
        //        {
        //            patCom.OrderSn = drBCCombine.OrderSn.ToString();
        //        }

        //        //组合名称
        //        if (!string.IsNullOrEmpty(drBCCombine.ComName))
        //        {
        //            patCom.PatComName = drBCCombine.ComName.ToString();
        //        }

        //        //条码登记信息
        //        patCom.SampFlag = Convert.ToInt32(drBCCombine.SampFlag);


        //        patCom.PidDeptCode = sampMain.PidDeptCode.ToString();
        //        //送检科室名称
        //        if (!string.IsNullOrEmpty(sampMain.PidDeptName))
        //        {
        //            patCom.PidDeptName = sampMain.PidDeptName.ToString();
        //        }
        //        if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
        //        {
        //            patCom.PidDoctorCode = sampMain.PidDoctorCode.ToString();
        //        }
        //        else
        //        {
        //            if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生ID
        //            {
        //                patCom.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(sampMain.PidDoctorName.ToString());
        //            }
        //        }

        //        //开单医生姓名
        //        if (!string.IsNullOrEmpty(sampMain.PidDoctorName))
        //        {
        //            patCom.PidDoctorName = sampMain.PidDoctorName.ToString();
        //        }

        //        //临床诊断
        //        if (!string.IsNullOrEmpty(sampMain.PidDiag))
        //        {
        //            patCom.PidDiag = sampMain.PidDiag.ToString();
        //        }
        //        patCom.ComSeq = com_seq.ToString();

        //        patCom.RepBarCode = sampMain.SampBarCode;
        //        drPatInfo.ListPidReportDetail.Add(patCom);
        //        com_seq++;
        //    }

        //    return drPatInfo;
        //}
        #endregion
    }
}
