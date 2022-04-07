using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Data;
using dcl.svr.sample;
using System.Text.RegularExpressions;
using dcl.svr.cache;
using dcl.pub.entities;
using dcl.root.dac;
using dcl.svr.dicbasic;
using dcl.root.logon;

namespace dcl.svr.result
{
    public class PidReportMainBIZ : IPidReportMain
    {
        public List<EntityPatients> GetPatientsDetail(DateTime dtFrom, DateTime dtTo, string itrId, string sid = "")
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPatients> dtPat = new List<EntityPatients>();
            if (mainDao != null)
            {
                dtPat = mainDao.GetPatientsDetail(dtFrom, dtTo, itrId, sid);
            }
            return dtPat;
        }

        public bool ExsitSid(string pat_sid, string itr_id, DateTime pat_date)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSid(pat_sid, itr_id, pat_date);
            }
            return result;
        }

        public bool ExsitPatHostOrder(string pat_host_order, string itr_id, DateTime pat_date)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSid(pat_host_order, itr_id, pat_date, "1");
            }
            return result;
        }

        public bool InsertNewPatient(List<EntityPatients> patients)
        {

            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                foreach (EntityPatients item in patients)
                {
                    result = mainDao.InsertNewPatient(item);
                    //插入病人信息成功后插入病人组合信息
                    if (result && item.ListPidReportDetail.Count>0)
                    {
                        result= new PidReportDetailBIZ().InsertNewReportDetail(item.ListPidReportDetail);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据条码号获取病人资料
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public EntityResponse GetPatientsByBarCode(string barCode)
        {
            ////获取lis病人资料表结构
            Dictionary<string, object> patientsInfo = new Dictionary<string, object>();
            EntityResponse respone = new EntityResponse();
            List<EntityPatients> patients = new List<EntityPatients>();

            //获取条码病人资料
            SampMainBIZ sampMain = new SampMainBIZ();
            EntitySampMain dsBarCodePatient = sampMain.SampMainQueryByBarId(barCode);

            List<EntityPidReportDetail> dtLISCombineToUnRegister = new List<EntityPidReportDetail>();
            List<EntityPidReportDetail> dtLisCombineAll = new List<EntityPidReportDetail>();
            List<EntityPatients> dtPatMergeComid = new List<EntityPatients>();

            if (dsBarCodePatient != null)
            {
                EntitySampMain dtBarCodePat = dsBarCodePatient;

                //填充条码病人资料到lis病人资料
                patients = ConvertBarCodePatientToLisPatient(dtBarCodePat, patients);

                //根据条码获取条码明细
                List<EntitySampDetail> dtBCCombine = new SampDetailBIZ().GetSampDetail(barCode);

                #region 特殊项目小组合转大组合上机

                //如果是特殊项目合并组合
                if (!string.IsNullOrEmpty(dtBarCodePat.SampMergeComId.ToString())
                    && dtBCCombine != null && dtBCCombine.Count > 0)
                {
                    string tempStrcomid = "";//组合id
                    string tempStroriid = dtBarCodePat.PidOrgId.ToString();//来源 bc_ori_id

                    //bc_merge_comid的值等于：递增号+,+组合id
                    if (dtBarCodePat.SampMergeComId.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        tempStrcomid = dtBarCodePat.SampMergeComId.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    }

                    dcl.pub.entities.dict.EntityDictCombineBar eCombar = dcl.svr.cache.DictCombineBarCache.Current.GetCombineBarWithComID(tempStrcomid, tempStroriid);

                    if (eCombar != null)
                    {
                        if (dtBCCombine.Count > 1)
                        {
                            //只保留一条，清除其他
                            EntitySampDetail tempkeepone = dtBCCombine[0];
                            dtBCCombine.Clear();
                            dtBCCombine.Add(tempkeepone);
                        }

                        dtBCCombine[0].ComId = eCombar.com_id;
                        dtBCCombine[0].OrderCode = eCombar.com_his_fee_code;
                        dtBCCombine[0].CombineName = eCombar.com_print_name;

                        //如果eCombar.com_print_name没有维护字典时,再获取dict_combine.com_name as bc_lis_code_name
                        if (string.IsNullOrEmpty(eCombar.com_print_name) && !string.IsNullOrEmpty(eCombar.com_id))
                        {
                            dcl.pub.entities.dict.EntityDictCombine temp_eCom = dcl.svr.cache.DictCombineCache.Current.GetCombineByID(eCombar.com_id, true);
                            if (temp_eCom != null && !string.IsNullOrEmpty(temp_eCom.com_name))
                            {
                                dtBCCombine[0].CombineName = temp_eCom.com_name;
                            }
                        }

                        dtPatMergeComid = GetPatientByMergeComid(dtBarCodePat.SampMergeComId.ToString());
                    }
                }
                #endregion

                //项目序号
                int com_seq = 0;
                foreach (EntitySampDetail drBCCombine in dtBCCombine)//条码检验组合转换为LIS中的病人检验组合
                {
                    if (Compare.IsEmpty(drBCCombine.SampFlag) || drBCCombine.SampFlag.ToString() != "1")
                    {
                        EntityPidReportDetail drLisCombine = new EntityPidReportDetail();

                        drLisCombine.ComId = drBCCombine.ComId;//项目ID
                        drLisCombine.OrderCode = drBCCombine.OrderCode;//组合HIS编码
                        drLisCombine.OrderPrice = drBCCombine.OrderPrice.ToString();//价格
                        drLisCombine.OrderSn = drBCCombine.OrderSn;//医嘱ID
                        drLisCombine.SortNo = com_seq;//顺序号
                        drLisCombine.PatComName = drBCCombine.CombineName;//组合名称
                        drLisCombine.ComSeq = drBCCombine.SortNo;
                        dtLISCombineToUnRegister.Add(drLisCombine);
                        com_seq++;
                    }

                    EntityPidReportDetail drLisAllCombine = new EntityPidReportDetail();

                    drLisAllCombine.ComId = drBCCombine.ComId;//项目ID
                    drLisAllCombine.OrderCode = drBCCombine.OrderCode;//组合HIS编码
                    drLisAllCombine.OrderPrice = drBCCombine.OrderPrice.ToString();//价格
                    drLisAllCombine.OrderSn = drBCCombine.OrderSn;//医嘱ID
                    drLisAllCombine.SortNo = com_seq;//顺序号
                    drLisAllCombine.PatComName = drBCCombine.CombineName;//组合名称

                    dtLisCombineAll.Add(drLisAllCombine);
                }

            }
            patientsInfo.Add("patients", patients);
            patientsInfo.Add("dtLISCombineToUnRegister", dtLISCombineToUnRegister);
            patientsInfo.Add("dtLisCombineAll", dtLisCombineAll);

            if (dtPatMergeComid != null && dtPatMergeComid.Count > 0)
            {
                patientsInfo.Add("dtPatMergeComid", dtPatMergeComid);
            }

            respone.SetResult(patientsInfo);

            return respone;
        }

        public List<EntityPatients> GetPatientByMergeComid(string BcMergeComid)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPatients> dtPat = new List<EntityPatients>();
            if (mainDao != null)
            {
                dtPat = mainDao.GetPatientByMergeComid(BcMergeComid);
            }
            return dtPat;
        }
        /// <summary>
        /// 获取标识Id
        /// </summary>
        /// <param name="patItrId">仪器ID</param>
        /// <param name="patBarcode">条码号</param>
        /// <param name="patSid">样本号</param>
        /// <returns></returns>
        public string GetPatientPatId(string patItrId, string patBarcode, string patSid)
        {
            string patId = string.Empty;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPatients> dtPat = new List<EntityPatients>();
            if (mainDao != null)
            {
                patId = mainDao.GetPatientPatId(patItrId, patBarcode, patSid);
            }
            return patId;
        }

        public bool ExistHostOrder(int pat_host_order, string itr_id, DateTime pat_date)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPatients> dtPat = new List<EntityPatients>();
            if (mainDao != null)
            {
                result = mainDao.ExistHostOrder(pat_host_order, itr_id, pat_date);
            }
            return result;
        }
        /// <summary>
        /// 条码病人表资料转换为lis病人资料表
        /// </summary>
        /// <param name="dtBarCodePat"></param>
        /// <param name="dtLisPat"></param>
        private List<EntityPatients> ConvertBarCodePatientToLisPatient(EntitySampMain dtBarCodePat, List<EntityPatients> dtLisPat)
        {
            EntitySampMain drBarcode = dtBarCodePat;
            EntityPatients drPatInfo = new EntityPatients();

            //姓名
            drPatInfo.PidName = drBarcode.PidName;

            //性别
            string sex = "0";
            if (!string.IsNullOrEmpty(drBarcode.PidSex))
            {
                if (drBarcode.PidSex.ToString() == "男"
                    || drBarcode.PidSex.ToString() == "1")
                {
                    sex = "1";
                }
                else if (drBarcode.PidSex.ToString() == "女"
                    || drBarcode.PidSex.ToString() == "2"
                    )
                {
                    sex = "2";
                }
                else
                {
                    sex = string.Empty;
                }
            }

            drPatInfo.PidSex = sex;

            //年龄
            if (drBarcode.PidAge != null && !string.IsNullOrEmpty(drBarcode.PidAge))
            {
                //目前只截取年
                string age = drBarcode.PidAge.ToString();

                if (age.ToLower().Contains('y')
                && age.ToLower().Contains('m')
                && age.ToLower().Contains('d')
                && age.ToLower().Contains('h')
                && age.ToLower().Contains('i')
                )
                {
                    drPatInfo.PidAgeExp = age;
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
                    drPatInfo.PidAgeExp = age;
                }
            }

            //病人来源
            drPatInfo.SrcName = drBarcode.PidSrcName;
            drPatInfo.PidSrcId = drBarcode.PidSrcId;
            //所属院区
            if (!string.IsNullOrEmpty(drBarcode.PidOrgId))
            {
                drPatInfo.PidOrgId = drBarcode.PidOrgId;
            }
            drPatInfo.PidAge = AgeConverter.AgeValueTextToMinute(drPatInfo.PidAgeExp.ToString());

            if (drBarcode.ReachDate != null)//送达时间
            {
                drPatInfo.SampReachDate = Convert.ToDateTime(drBarcode.ReachDate);
            }

            DateTime now = ServerDateTime.GetDatabaseServerDateTime();
            string Lab_BarcodeTimeCal = "佛山市一";
            Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");

            if (Lab_BarcodeTimeCal == "佛山市一")
            {
                #region 佛山市一
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (drBarcode.SampPrintDate == null)
                {
                    drBarcode.SampPrintDate = drBarcode.SampDate;
                }

                //送检时间
                if (!Compare.IsEmpty(drBarcode.ReachDate))//送达时间 不为空
                {
                    //如果送达时间不为空，将送达时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.ReachDate);
                }
                else if (!Compare.IsEmpty(drBarcode.ReceiverDate))//签收时间 不为空
                {
                    //如果签收时间不为空，将签收时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.ReceiverDate);
                }
                else
                {
                    drPatInfo.SampSendDate = now;
                }

                ////采样时间
                if (!Compare.IsEmpty(drPatInfo.PidSrcId))
                {
                    string ori_id = drPatInfo.PidSrcId.ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(drBarcode.CollectionDate))
                        {
                            drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(drBarcode.CollectionDate);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo.SampCollectionDate = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (drBarcode.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                        }
                    }
                    else
                    {
                        if (drBarcode.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                        }
                    }
                }
                else
                {
                    if (drBarcode.CollectionDate != null)
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                    }
                    else
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                    }
                }
                //如果条码医嘱时间为空，将样本采集时间赋值给条码医嘱时间
                if (drBarcode.SampOccDate == null)
                {
                    drBarcode.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
                }

                //医嘱执行时间(申请时间)
                drPatInfo.SampReceiveDate = drBarcode.SampOccDate;
                if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
                    && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
                    )
                {
                    drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "清远人医")
            {
                #region 清远人医
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (drBarcode.SampPrintDate == null)
                {
                    drBarcode.SampPrintDate = drBarcode.SampDate;
                }

                //送检时间
                if (!Compare.IsEmpty(drBarcode.ReachDate))//送达时间 不为空
                {
                    //如果送达时间不为空，将送达时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.ReachDate);
                }
                else if (!Compare.IsEmpty(drBarcode.ReceiverDate))//签收时间 不为空
                {
                    //如果签收时间不为空，将签收时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.ReceiverDate);
                }
                else if (!Compare.IsEmpty(drBarcode.SendDate))//送检时间 不为空
                {
                    //如果送检时间不为空，将送检时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.SendDate);
                }
                else
                {
                    drPatInfo.SampSendDate = now;
                }


                ////采样时间
                if (!Compare.IsEmpty(drPatInfo.PidSrcId))
                {
                    string ori_id = drPatInfo.PidSrcId.ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(drBarcode.CollectionDate))
                        {
                            drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(drBarcode.CollectionDate);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo.SampCollectionDate = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (drBarcode.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                        }
                    }
                    else
                    {
                        if (drBarcode.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                        }
                    }
                }
                else
                {
                    if (drBarcode.CollectionDate != null)
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                    }
                    else
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SampPrintDate);
                    }
                }

                if (drBarcode.SampOccDate == null)
                {
                    drBarcode.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
                }

                //医嘱执行时间(申请时间)
                drPatInfo.SampReceiveDate = drBarcode.SampOccDate;
                if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
                    && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
                    )
                {
                    drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "中山人医")
            {
                #region 中山人医
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (drBarcode.SampPrintDate == null)
                {
                    drBarcode.SampPrintDate = drBarcode.SampDate;
                }

                if (drBarcode.CollectionDate != null)//采样时间
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                }

                //采集时间为空，则标本收取时间作为采集时间
                if (string.IsNullOrEmpty(drBarcode.CollectionDate.ToString())
                    && !string.IsNullOrEmpty(drBarcode.SendDate.ToString()))
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.SendDate).AddMinutes(-new Random().Next(3, 9));
                }

                if (drBarcode.SendDate != null)//送检时间(收取)
                {
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.SendDate);
                }

                if (drBarcode.ReceiverDate != null)//接收时间
                {
                    drPatInfo.SampApplyDate = Convert.ToDateTime(drBarcode.ReceiverDate);
                }

                if (drBarcode.SampOccDate != null)//申请时间
                {
                    drPatInfo.SampReceiveDate = Convert.ToDateTime(drBarcode.SampOccDate);
                }

                if (drBarcode.ReachDate != null)//送达时间
                {
                    drPatInfo.SampReachDate = Convert.ToDateTime(drBarcode.ReachDate);
                }

                #endregion
            }
            else
            {
                if (drBarcode.CollectionDate != null)//采样时间
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(drBarcode.CollectionDate);
                }

                if (drBarcode.SendDate != null)//送检时间(收取)
                {
                    drPatInfo.SampSendDate = Convert.ToDateTime(drBarcode.SendDate);
                }

                if (drBarcode.ReceiverDate != null)//接收时间
                {
                    drPatInfo.SampApplyDate = Convert.ToDateTime(drBarcode.ReceiverDate);
                }

                if (drBarcode.SampOccDate != null)//申请时间
                {
                    drPatInfo.SampReceiveDate = Convert.ToDateTime(drBarcode.SampOccDate);
                }

                if (drBarcode.ReachDate != null)//送达时间
                {
                    drPatInfo.SampReachDate = Convert.ToDateTime(drBarcode.ReachDate);
                }

            }
            int iAdmissTimes = 0;

            if (int.TryParse(drBarcode.PidAdmissTimes.ToString(), out iAdmissTimes))
            {
                drPatInfo.PidAddmissTimes = iAdmissTimes;
            }
            else
            {
                drPatInfo.PidAddmissTimes = 0;
            }

            //标本备注
            drPatInfo.SampRemark = drBarcode.SampRemContent;

            //ID类型
            drPatInfo.PidIdtId = drBarcode.PidIdtId;

            //接收时间
            drPatInfo.SampApplyDate = drBarcode.ReceiverDate;

            drPatInfo.SampCheckDate = now;

            //条码
            drPatInfo.RepBarCode = drBarcode.SampBarCode;

            //病床号
            drPatInfo.PidBedNo = drBarcode.PidBedNo;

            //ID
            drPatInfo.PidInNo = drBarcode.PidInNo;

            //病区code
            drPatInfo.PidWardId = drBarcode.PidDeptCode;

            //病区名称
            drPatInfo.PidWardName = string.Empty;

            //送检科室名称
            drPatInfo.PidDeptName = drBarcode.PidDeptName;

            drPatInfo.PidSocialNo = drBarcode.PidSocialNo;

            //送检科室code
            drPatInfo.PidDeptId = string.Empty;

            if ((CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "通用"
                || CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "outlink")
                && !Compare.IsNullOrDBNull(drBarcode.PidDeptCode))
            {
                drPatInfo.PidDeptId = drBarcode.PidDeptCode.ToString();
            }
            //联系地址
            drPatInfo.PidAddress = drBarcode.PidAddress;

            //联系电话
            drPatInfo.PidTel = drBarcode.PidTel;

            if (!Compare.IsEmpty(drBarcode.PidDoctorCode))//如果医生工号不为空
            {
                drPatInfo.PidDoctorCode = drBarcode.PidDoctorCode.ToString();
            }
            else
            {
                if (!Compare.IsEmpty(drBarcode.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生code
                {
                    drPatInfo.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(drBarcode.PidDoctorName.ToString());
                    //new DictDoctor().GetDocByName(drBarcode.PidDoctorName.ToString());
                }
            }

            //开单医生姓名
            drPatInfo.PidDocName = drBarcode.PidDoctorName;
            drPatInfo.DoctorName = drBarcode.PidDoctorName;

            //临床诊断
            drPatInfo.PidDiag = drBarcode.PidDiag;

            if (drBarcode.PidBirthday != null)//出生日期
            {
                drPatInfo.PidBirthday = Convert.ToDateTime(drBarcode.PidBirthday);
            }

            //条码状态
            drPatInfo.BcStatus = drBarcode.SampStatusId;

            drPatInfo.SampType = drBarcode.SampType;
            //打印标志
            drPatInfo.SampPrintFlag = drBarcode.SampPrintFlag.ToString();

            //标本类别
            drPatInfo.SamName = drBarcode.SampSamName;
            drPatInfo.PidSamId = drBarcode.SampSamId;

            //检查类型
            if (drBarcode.SampUrgentFlag)
            {
                drPatInfo.RepCtype = "2";
            }
            else
            {
                drPatInfo.RepCtype = "1";
            }

            //检查类型
            if (drBarcode.SampUrgentStatus.ToString() == "2")
            {
                drPatInfo.RepCtype = "4";
            }
            //+++++++++edit by sink 2010-9-26 ++++++++++++
            //自定义ID
            if (!Compare.IsNullOrDBNull(drBarcode.PidPatno))
            {
                drPatInfo.RepInputId = drBarcode.PidPatno.ToString();
            }

            //唯一号UPID 目前滨海使用
            if (!Compare.IsNullOrDBNull(drBarcode.PidUniqueId))
            {
                drPatInfo.PidUniqueId = drBarcode.PidUniqueId.ToString();
            }

            //人员身份
            if (!Compare.IsNullOrDBNull(drBarcode.PidIdentity))
            {
                drPatInfo.PidIdentity = drBarcode.PidIdentity;
            }

            //保存拆分大组合(特殊合并)ID
            if (!Compare.IsNullOrDBNull(drBarcode.SampMergeComId))
            {
                drPatInfo.BcMergeComid = drBarcode.SampMergeComId.ToString();
            }

            //申请单号
            if (!Compare.IsNullOrDBNull(drBarcode.SampApplyNo)
                )
            {
                drPatInfo.PidApplyNo = drBarcode.SampApplyNo.ToString();
            }

            //费用类别
            drPatInfo.PidInsuId = drBarcode.PidInsuId;

            //体检id
            drPatInfo.PidExamNo = drBarcode.PidExamNo;

            //体检id
            drPatInfo.PidExamCompany = drBarcode.PidExamCompanyName;

            //如果体检ID不为空，则更新病人来源为体检
            if (!Compare.IsEmpty(drBarcode.PidExamNo))
            {
                drPatInfo.PidSrcId = "109";
                drPatInfo.SrcName = "体检";
            }


            dtLisPat.Add(drPatInfo);
            return dtLisPat;
        }


        /// <summary>
        /// 将条码插入patients
        /// </summary>
        /// <param name="caller">操作记录</param>
        /// <param name="listDict">病人信息和明细集合</param>
        /// <returns></returns>
        public EntityOperateResult InsertBarCodePatient(EntityRemoteCallClientInfo caller, Dictionary<string, object> listDict)
        {
            EntityOperateResult result = new EntityOperateResult();//.GetNew("保存条码病人信息");
            result.ReturnResult = listDict;
            List<EntityPatients> listPatInfo = new List<EntityPatients>();
            List<EntityPidReportDetail> listPatCombine = new List<EntityPidReportDetail>();
            if (listDict.ContainsKey("patients") && listDict["patients"] != null)
            {
                listPatInfo = listDict["patients"] as List<EntityPatients>;
            }
            if (listDict.ContainsKey("dtLISCombineToUnRegister") && listDict["dtLISCombineToUnRegister"] != null)
            {
                listPatCombine = listDict["dtLISCombineToUnRegister"] as List<EntityPidReportDetail>;
            }
            EntityPatients patient = new EntityPatients();
            if (listPatInfo.Count > 0)
            {
                patient = listPatInfo[0];
            }
            string patId = GetPatientPatId(patient.RepItrId, patient.RepBarCode, patient.RepSid);

            if (!string.IsNullOrEmpty(patId))
            {
                //result.Success = false;
                return result;
            }
            //如果pat_age = -1 而pat_age_exp不为空则进行更新
            if ((patient.PidAge.ToString() == null || patient.PidAge.ToString() == "-1")
                && (patient.PidAgeExp != null && !string.IsNullOrEmpty(patient.PidAgeExp)))
            {
                try
                {
                    patient.PidAge = AgeConverter.AgeValueTextToMinute(patient.PidAgeExp);
                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp));

                }

            }
            if (listPatInfo.Count > 0)//取默认标本状态
            {
                patient.PidRemark = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");
            }

            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (listPatInfo.Count > 0
                    && patient.SampApplyDate == null)
                {
                    DateTime pat_jy_date = (DateTime)patient.SampCheckDate;
                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                    if (pat_jy_date > now)
                    {
                        patient.SampApplyDate = now;
                    }
                    else
                    {
                        patient.SampApplyDate = patient.SampCheckDate;
                    }
                }
            }

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "HIS代码关联")
            {
                patient.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(patient.PidDoctorCode);

                if (
                    (patient.PidDoctorCode == null)
                    && patient.PidDocName != null
                    )
                {
                    patient.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(patient.PidDocName);
                }
            }

            UpdatePatCName(listPatInfo, listPatCombine);



            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(patient.RepBarCode))
            {
                barcode = patient.RepBarCode;
            }
            try
            {
                DateTime pat_date = (DateTime)patient.RepInDate;
                string pat_sid = patient.RepSid;
                string pat_itr_id = patient.RepItrId;

                result.Data.Patient.RepSid = pat_sid;
                result.Data.Patient.PidName = patient.PidName;
                try
                {

                    //判断是否已回退
                    if (new SampMainBIZ().Returned(patient.RepBarCode))
                    {
                        result.Data.Patient.RepBarCode = patient.RepBarCode;
                        result.AddMessage(EnumOperateErrorCode.HaveReturned, EnumOperateErrorLevel.Error);
                    }
                    //判断是否存在样本号
                    else if (ExsitSid(pat_sid, pat_itr_id, pat_date))
                    {
                        result.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                    }
                    else
                    {
                        int? host_order = null;

                        if (!Compare.IsEmpty(patient.RepSerialNum))
                        {
                            host_order = Convert.ToInt32(patient.RepSerialNum);
                        }

                        InstrmtBIZ bllItr = new InstrmtBIZ();
                        if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                        {
                            result.AddMessage(EnumOperateErrorCode.HostOrderExist, EnumOperateErrorLevel.Error);
                        }
                        else
                        {

                            if (Compare.IsEmpty(listPatInfo[0].SampCheckDate))
                            {
                                patient.SampCheckDate = ServerDateTime.GetDatabaseServerDateTime();
                            }

                            //获取插入组合
                            List<EntityPidReportDetail> listReportDetail = GetCombineInsertList(listPatCombine, barcode, result);
                            patient.ListPidReportDetail = listReportDetail;
                            //获取插入病人信息
                            EntityPatients entityInsertInfo = GetPatientInfoInsertEntity(patient);
                            //插入缺省值结果
                            new ObrResultBIZ().InsertDefaultResult(patient.RepId
                                                 , patient.PidSamId
                                                 , patient.RepItrId
                                                 , patient.RepSid
                                                 , listPatCombine);
                            List<EntityPatients> listPatients = new List<EntityPatients>();
                            listPatients.Add(patient);
                            //插入病人资料
                            InsertNewPatient(listPatients);


                            if (!string.IsNullOrEmpty(barcode))
                            {
                                string barcodeRemark = string.Empty;
                                //*************************************************************************************
                                //将序号写入备注中
                                if (host_order.HasValue)
                                {
                                    barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}", listPatInfo[0].ItrName, pat_sid, host_order.Value, listPatInfo[0].PidComName, listPatInfo[0].RepInDate);
                                }
                                else
                                {
                                    barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", listPatInfo[0].ItrName, pat_sid, listPatInfo[0].PidComName, listPatInfo[0].RepInDate);
                                }

                                //************************************************************************************
                                EntitySampOperation operation = new EntitySampOperation();
                                operation.OperationID = caller.LoginID;
                                operation.OperationName = caller.OperationName;
                                operation.OperationIP = caller.IPAddress;
                                operation.OperationTime = caller.Time;
                                operation.OperationWorkId = caller.UserID;
                               new SampMainBIZ().UpdateSampMainStatusByBarId(operation, barcode);
                            }
                            #region 院网接口
                            //if (result.Success)
                            //{
                            //    string pat_ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();
                            //    if (pat_ori_id == "107")//门诊
                            //    {
                            //        if (CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                            //        {
                            //            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                            //        }
                            //    }
                            //    else if (pat_ori_id == "108")//住院
                            //    {
                            //        if (CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                            //        {
                            //            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                            //        }

                            //        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_RegisterExecuteYZ") == "启用")
                            //        {
                            //            ExecuteYZ(barcode);
                            //        }

                            //    }
                            //    else if (pat_ori_id == "109")//体检
                            //    {
                            //        if (CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                            //        {
                            //            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                            //        }
                            //    }
                            //    if (!string.IsNullOrEmpty(barcode) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_UploadProcessBarCode") == "是")
                            //    {
                            //        //Lis.SendDataToCDR.CDRService cds = new Lis.SendDataToCDR.CDRService();
                            //        //cds.UploadProcessInvoke(barcode);
                            //    }

                            //}
                            #endregion
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatient", ex.ToString());
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }
            return result;
        }

        public void UpdatePatCName(List<EntityPatients> listPat, List<EntityPidReportDetail> listCombine)
        {

            //***************************************************************************//
            //将所选组合排序
            int[] a = new int[listCombine.Count];
            for (int i = 0; i < a.Length; i++)
            {
                if ((listCombine[i].ComSeq == null) || (listCombine[i].ComSeq == ""))
                    a[i] = 99999;
                else
                    a[i] = Convert.ToInt32(listCombine[i].ComSeq);
            }
            a = SortCombine(a);

            string pat_c_name = string.Empty;

            bool needPlus = false;
            for (int i = 0; i < listCombine.Count; i++)
            {
                if (needPlus)
                {
                    pat_c_name += "+";
                }
                //根据项目组合中的顺序加入
                pat_c_name += listCombine[a[i]].PatComName;

                needPlus = true;
            }

            listPat[0].PidComName = pat_c_name;
        }

        //*******************************************************************************//
        //冒泡排序
        private int[] SortCombine(int[] a)
        {
            int[] tempArry = new int[a.Length];
            for (int i = 0; i < tempArry.Length; i++)
            {
                tempArry[i] = i;
            }
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Length - 1; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;

                        int temp1 = tempArry[j];
                        tempArry[j] = tempArry[j + 1];
                        tempArry[j + 1] = temp1;
                    }
                }
            }

            return tempArry;
        }

        /// <summary>
        /// 保存病人信息
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private EntityPatients GetPatientInfoInsertEntity(EntityPatients entityPatient)
        {
            ////样本号
            string pat_sid = entityPatient.RepSid;

            DateTime pat_date = Convert.ToDateTime(entityPatient.RepInDate);
            string nowTime = DateTime.Now.ToString(" HH:mm:ss");
            string pat_id = entityPatient.RepItrId + pat_date.ToString("yyyyMMdd") + entityPatient.RepSid;
            entityPatient.RepInDate = Convert.ToDateTime(pat_date.ToString("yyyy-MM-dd") + nowTime);
            entityPatient.RepId = pat_id;
            entityPatient.RepStatus = 0;
            entityPatient.RepModifyFrequency = 0;//修改次数

            if (!Compare.IsEmpty(entityPatient.PidAgeExp))
            {
                entityPatient.PidAge = AgeConverter.AgeValueTextToMinute(entityPatient.PidAgeExp);
            }

            //根据仪器报警内容更新复查标记
            try
            {

                InstrmtWardingMsgBIZ msgBiz = new InstrmtWardingMsgBIZ();

                object obj = msgBiz.CheckHasInstrmtWardMsgByPatItrId(pat_id);
                if (obj != null && obj != DBNull.Value && Convert.ToInt32(obj) > 0)
                {
                    entityPatient.RepRecheckFlag = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatientInfoInsertCMD", ex.ToString());
            }
            return entityPatient;
        }

        /// <summary>
        /// 获取插入病人组合的信息
        /// </summary>
        /// <param name="listPatCombine">病人组合集合</param>
        /// <param name="barcode">条码号</param>
        /// <param name="opResult">操作记录</param>
        /// <returns></returns>
        private List<EntityPidReportDetail> GetCombineInsertList(List<EntityPidReportDetail> listPatCombine, string barcode, EntityOperateResult opResult)
        {
            List<EntityPidReportDetail> listReportDdetail = new List<EntityPidReportDetail>();
            if (opResult.Success && listPatCombine.Count > 0)
            {
                string pat_id = string.Empty;
                if (opResult.ReturnResult["patients"] != null)
                {
                    List<EntityPatients> listPat = opResult.ReturnResult["patients"] as List<EntityPatients>;
                    pat_id = listPat[0].RepId;
                }
                StringBuilder sbCom_id = new StringBuilder();
                bool needComma = false;
                listPatCombine = listPatCombine.OrderBy(w => w.ComSeq).ToList();
                int i = 0;
                foreach (EntityPidReportDetail entityCombine in listPatCombine)
                {
                    entityCombine.SortNo = i;
                    if (!Compare.IsEmpty(entityCombine.ComId))
                    {
                        entityCombine.RepId = pat_id;

                        if (needComma)
                        {
                            sbCom_id.Append(",");
                        }

                        sbCom_id.Append(string.Format(" '{0}' ", entityCombine.ComId));
                        listReportDdetail.Add(entityCombine);
                        needComma = true;
                    }
                    i++;
                }

                //删除该标识ID病人组合明细
                PidReportDetailBIZ detailBIZ = new PidReportDetailBIZ();
                detailBIZ.DeleteReportDetail(pat_id);

                //如果有条码号则更新bc_cname标志
                if (!string.IsNullOrEmpty(barcode) && sbCom_id.Length > 0)
                {
                    SampDetailBIZ sampDetailBIZ = new SampDetailBIZ();
                    sampDetailBIZ.UpdateSampDetailSampFlagByComId(barcode, sbCom_id.ToString());
                }
            }
            return listReportDdetail;
        }
        public List<EntityPatients> GetPatientsByCustomCondition(EntityAnanlyseQC ananlyse)
        {
            List<EntityPatients> listPat = new List<EntityPatients>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                listPat = mainDao.GetPatientsByCustomCondition(ananlyse);
            }
            return listPat;
        }

        public bool SearchPatientByRepId(string repId)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.SearchPatientByRepId(repId);
            }
            return result;
        }

        public bool UpdatePatientData(List<EntityPatients> listPat)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityPatients patient in listPat)
                    {
                        mainDao.UpdatePatientData(patient);
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

    }
}
