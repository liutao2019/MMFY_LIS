using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using dcl.root.logon;
using Lib.DAC;
using System.Data;
using Lib.DataInterface.Implement;
using dcl.svr.cache;
using dcl.entity;
using dcl.svr.msg;
using dcl.svr.sample;
using System.Xml;
using System.IO;
using dcl.svr.interfaces;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// 发送危急值消息
    /// </summary>
    public class SendCriticalMessage : AbstractAuditClass, IAuditUpdater
    {

        protected DataTable m_dtbCriticalPatient = new DataTable();
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info"></param>
        /// <param name="resulto"></param>
        /// <param name="auditType"></param>
        /// <param name="config"></param>
        public SendCriticalMessage(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        {

        }

        public SendCriticalMessage()
            : base(null, null, null, EnumOperationCode.Report, null)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report)//只有报告(二审)时才操作
            {
                Thread t = new Thread(ThreadSendCriticalMessage);
                t.Start();
            }
            else if (this.auditType == EnumOperationCode.UndoReport)
            {
                Thread t = new Thread(ThreadClearCriticalMessage);
                t.Start();
            }
        }

        public void UpdateByBacteriaForDcl(EntityQcResultList res)
        {
            try
            {
                Thread t = new Thread(ThreadUpdateBacterialCriticalMessageForDCL);
                t.Start((object)res);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

        }

        #endregion
        //危急值结果
        List<EntityObrResult> listUrgentResult = new List<EntityObrResult>();
        void ThreadSendCriticalMessage()
        {
            try
            {
                bool hasCriticalValues = false;

                bool hasTJCriticalValues = false;
                //是否有传染病项目超出危急值
                bool hasItmForillflag = false;

                string itm_ecd = null;
                string res_chr = null;

                //危急值 项目，值，单位
                //格式：项目1:值+单位,项目2:值+单位
                StringBuilder item_value_unit = new StringBuilder();
                StringBuilder item_value_unit2 = new StringBuilder();
                bool needComma = false;


                StringBuilder ex_item_value_unit = new StringBuilder();
                StringBuilder ex_item_value_unit2 = new StringBuilder();
                bool extneedComma = false;

                //系统配置：判断危急值时只报超出阈值(不报超出危急)
                bool BlnUrgent_OnlyReportThreshold = CacheSysConfig.Current.GetSystemConfig("Urgent_OnlyReportThreshold") == "是";

                //系统配置：[门诊]允许报告危急值
                bool BlnUrgent_AllowReport_MZ = CacheSysConfig.Current.GetSystemConfig("Urgent_AllowReport_MZ") != "否";

                bool useWardID = CacheSysConfig.Current.GetSystemConfig("Urgent_Dept_Column") == "pat_ward_id";

                bool isSendTJMsg = CacheSysConfig.Current.GetSystemConfig("Audit_SendTjCriticalToMsg") == "是";

                if (pat_info.PidSrcId == "107") //门诊
                {
                    if (!BlnUrgent_AllowReport_MZ) return;//如果[门诊]不允许报告危急值,则跳过
                }

                //获取项目字典缓存信息
                List<EntityDicItmItem> listDicItem = dcl.svr.cache.DictItemCache.Current.DclCache;

                foreach (EntityObrResult res in this.resulto)
                {
                    int int_res_ref_flag;
                    if (int.TryParse(res.RefFlag, out int_res_ref_flag))
                    {
                        EnumResRefFlag res_ref_flag = (EnumResRefFlag)int_res_ref_flag;

                        if (res_ref_flag != EnumResRefFlag.Unknow)
                        {
                            //根据res_ref_flag标志判断
                            bool sendMsg = false;
                            //是否判断阈值
                            bool Lab_CriticalMsgJudgeThreshold = CacheSysConfig.Current.GetSystemConfig("Lab_CriticalMsgJudgeThreshold") != "否";
                            if (Lab_CriticalMsgJudgeThreshold)
                            {
                                sendMsg = (
                                ((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                                )//自定义危急值
                                ||
                                ((res_ref_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2
                                && (!BlnUrgent_OnlyReportThreshold))//高于危急值上限
                                ||
                                ((res_ref_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2
                                && (!BlnUrgent_OnlyReportThreshold))//低于危急值下限
                                ||
                                (res_ref_flag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3//高于阈值上限
                                ||
                                (res_ref_flag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3//低于阈值下限
                                ||
                                (((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue)
                                && (!BlnUrgent_OnlyReportThreshold))//低于危急值下限
                               );
                            }
                            else
                            {
                                sendMsg = (
                                ((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                                )//自定义危急值
                                ||
                                ((res_ref_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2
                                && (!BlnUrgent_OnlyReportThreshold))//高于危急值上限
                                ||
                                ((res_ref_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2
                                && (!BlnUrgent_OnlyReportThreshold))//低于危急值下限
                                ||
                                ((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                                && (!BlnUrgent_OnlyReportThreshold))//低于危急值下限
                               );
                            }
                            if (sendMsg)
                            {
                                itm_ecd = res.ItmEname;
                                res_chr = res.ObrValue;
                                hasCriticalValues = true;

                                //当前项目是否标识为‘传染病’
                                if (listDicItem != null && listDicItem.Count > 0
                                    && listDicItem.FindAll(tempItem => (tempItem.ItmId == res.ItmId && tempItem.ItmInfectionFlag == 1)).Count > 0)
                                {
                                    hasItmForillflag = true;
                                    res.hasItmForillflag = true;
                                }

                                string temp_item_value_unit = res.ItmEname + ":" + res.ObrValue + " " + res.ObrUnit;

                                if (needComma)
                                    item_value_unit.Append(",");

                                item_value_unit.Append(temp_item_value_unit);
                                //break;

                                needComma = true;

                                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("HospitalName") == "广东医学院附属医院")
                                {
                                    item_value_unit2.AppendLine(string.Format("{0}  {1} {2}  {3}", res.ItmEname, res.ObrValue, "参考值范围(" + res.ResRefRange + ")", dict_res_ref_flag(res.RefFlag)));
                                }
                                else
                                {
                                    item_value_unit2.AppendLine(string.Format("{0}  {1} {2} {3}  {4}", res.ItmEname, res.ObrValue, res.ObrUnit, "参考值范围(" + res.ResRefRange + ")", dict_res_ref_flag(res.RefFlag)));
                                }

                                listUrgentResult.Add(res);
                            }
                        }
                    }


                    if (isSendTJMsg && pat_info.PidSrcId == "109")
                    {
                        int int_ext_res_ref_flag;
                        if (int.TryParse(res.ExtResRefFlag, out int_ext_res_ref_flag))
                        {
                            EnumResRefFlag res_ref_flag = (EnumResRefFlag)int_ext_res_ref_flag;

                            if (res_ref_flag != EnumResRefFlag.Unknow)
                            {
                                //根据res_ref_flag标志判断

                                bool sendMsg = (
                                    ((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                                    )//自定义危急值
                                    ||
                                    ((res_ref_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2
                                   )//高于危急值上限
                                    ||
                                    ((res_ref_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2
                                   )//低于危急值下限
                                    ||
                                    ((res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                                    )//低于危急值下限
                                   );

                                if (sendMsg)
                                {
                                    itm_ecd = res.ItmEname;
                                    res_chr = res.ObrValue;
                                    hasTJCriticalValues = true;

                                    string temp_item_value_unit = res.ItmEname + ":" + res.ObrValue + " " + res.ObrUnit;

                                    if (extneedComma)
                                        ex_item_value_unit.Append(",");

                                    ex_item_value_unit.Append(temp_item_value_unit);

                                    extneedComma = true;

                                    ex_item_value_unit2.AppendLine(string.Format("{0}  {1} {2} {3}  {4}", res.ItmEname, res.ObrValue, res.ObrUnit, "参考值范围(" + res.ResRefRange + ")", dict_res_ref_flag(res.RefFlag)));
                                    listUrgentResult.Add(res);

                                }
                            }
                        }
                    }
                }

                //removed:目前只判断只有有一个项目出现危急值提示便发出危急值消息，并未判断是哪一个项目超出危急值
                if (hasCriticalValues)
                {
                    try
                    {
                        List<string> listOrderIDs = new SampDetailBIZ().GetPatOrderIDs(pat_info.RepId);//获取医嘱ID

                        foreach (string itemOrderID in listOrderIDs)
                        {
                            #region 向命令接口传入信息

                            List<InterfaceDataBindingItem> binding = new List<InterfaceDataBindingItem>();

                            string temp_doc_code = "";//医生代码
                            if (!string.IsNullOrEmpty(pat_info.PidDocName) && !string.IsNullOrEmpty(pat_info.PidDocName))
                            {
                                temp_doc_code = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByNameAndIDorCode(pat_info.PidDocName, pat_info.PidDoctorCode);
                            }
                            else if (!string.IsNullOrEmpty(pat_info.PidDocName))
                            {
                                temp_doc_code = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(pat_info.PidDocName);
                            }

                            binding.Add(new InterfaceDataBindingItem("pat_doc_code", temp_doc_code));
                            binding.Add(new InterfaceDataBindingItem("pat_doc_name", pat_info.PidDocName));

                            binding.Add(new InterfaceDataBindingItem("pat_dep_name", pat_info.PidDeptName));
                            binding.Add(new InterfaceDataBindingItem("pat_dep_code", pat_info.PidDeptId));

                            binding.Add(new InterfaceDataBindingItem("pat_id", pat_info.RepId));
                            binding.Add(new InterfaceDataBindingItem("pat_name", pat_info.PidName));
                            binding.Add(new InterfaceDataBindingItem("pat_bed_no", pat_info.PidBedNo));
                            binding.Add(new InterfaceDataBindingItem("pat_c_name", pat_info.PidComName));
                            binding.Add(new InterfaceDataBindingItem("pat_age_YMDHI", pat_info.PidAgeExp));
                            binding.Add(new InterfaceDataBindingItem("item_value_unit", item_value_unit.ToString()));
                            binding.Add(new InterfaceDataBindingItem("pat_pid", pat_info.RepInputId));
                            binding.Add(new InterfaceDataBindingItem("pat_social_no", pat_info.PidSocialNo));
                            binding.Add(new InterfaceDataBindingItem("pat_in_no", pat_info.PidInNo));
                            binding.Add(new InterfaceDataBindingItem("pat_report_name", CacheUserInfo.Current.GetUserNameByLoginID(pat_info.RepReportUserId)));//报告者名称
                            binding.Add(new InterfaceDataBindingItem("pat_yz_id", itemOrderID));//医嘱号
                            binding.Add(new InterfaceDataBindingItem("pat_report_code", pat_info.RepReportUserId));

                            DataInterfaceHelper iHelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);

                            iHelper.ExecuteNonQueryWithGroup("二审后_危急值", binding.ToArray());
                            //执行调用院网接口操作危机值的模块
                            if (pat_info.PidSrcId == "107") //门诊
                            {
                                iHelper.ExecuteNonQueryWithGroup("二审后_门诊_危急值", binding.ToArray());
                            }
                            else if (pat_info.PidSrcId == "108") //住院
                            {
                                iHelper.ExecuteNonQueryWithGroup("二审后_住院_危急值", binding.ToArray());
                            }
                            else if (pat_info.PidSrcId == "109") //体检
                            {
                                iHelper.ExecuteNonQueryWithGroup("二审后_体检_危急值", binding.ToArray());
                            }
                            else
                            {
                                iHelper.ExecuteNonQueryWithGroup("二审后_其他_危急值", binding.ToArray());
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                    }

                    #region 将危急值信息插入到危急值表中 2017-12-13 SJC
                    //插入到用户消息表
                    EntityDicObrMessageContent msg_content = new EntityDicObrMessageContent();
                    msg_content.ObrCreateTime = ServerDateTime.GetDatabaseServerDateTime();
                    msg_content.DelFlag = false;
                    msg_content.ObrValueA = this.pat_info.RepId;
                    msg_content.ObrValueB = this.pat_info.PidName;
                    msg_content.ObrValueC = item_value_unit2.ToString();
                    if (hasItmForillflag)
                    {
                        msg_content.ObrValueD = "传染病";
                    }
                    msg_content.ObrExpirationDate = null;
                    msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.CRITICAL_MESSAGE);
                    msg_content.ObrReceive = this.pat_info.PidDeptName;
                    msg_content.ObrMessageTitle = "危急值报告";

                    //床号
                    string temp_pat_bedNo = string.IsNullOrEmpty(this.pat_info.PidBedNo)
                                                ? ""
                                                : this.pat_info.PidBedNo;

                    //床号
                    string temp_pat_Sex = string.IsNullOrEmpty(this.pat_info.PidSex)
                                                ? ""
                                                : this.pat_info.PidSex == "1" ? "男" : "女";
                    string temp_pat_age = "";
                    try
                    {
                        temp_pat_age = string.IsNullOrEmpty(this.pat_info.PidAgeExp)
                                               ? ""
                                               : dcl.common.AgeConverter.ValueToText(dcl.common.AgeConverter.TrimZeroValue(this.pat_info.PidAgeExp));
                    }
                    catch
                    {

                        throw;
                    }


                    string message = string.Format("危急值报告\r\n病区：{0}  病人号：{1} 姓名：{2} 性别：{3} 年龄：{4} 床号：{5} 开单医生：{6}", this.pat_info.PidDeptName
                                                   , this.pat_info.PidInNo
                                                   , this.pat_info.PidName, temp_pat_Sex, temp_pat_age, temp_pat_bedNo, pat_info.PidDocName);

                    if (config.bSecondAuditUrgentContainCom) //发送危急值含[组合]
                    {
                        //组合
                        string temp_pat_comName = string.IsNullOrEmpty(this.pat_info.PidComName)
                                                      ? "NULL"
                                                      : this.pat_info.PidComName;
                        //开单医生
                        string temp_pat_docName = string.IsNullOrEmpty(this.pat_info.PidDocName)
                                                      ? "NULL"
                                                      : this.pat_info.PidDocName;

                        //报告时间
                        string temp_pat_repDate = string.IsNullOrEmpty(this.pat_info.RepReportDate.ToString())
                                                      ? "NULL"
                                                      : this.pat_info.RepReportDate.ToString();

                        //去掉物理组 13-4-19
                        //message = string.Format("危急值报告 病区：{0} 住院号：{1} 姓名：{2} 组合：{3} 物理组：{4}", this.pat_info.PidDeptName
                        //                                                , this.pat_info.PidInNo
                        //                                                , this.pat_info.PidName
                        //                                                , temp_pat_comName
                        //                                                , GetPatInstrmtType(this.pat_info.RepItrId));

                        message = string.Format("(危急)开单医生：{3} 姓名：{2} 组合：{5} 病区：{0} 住院号：{1} 床号：{4} 报告时间：{6} ",
                                                this.pat_info.PidDeptName
                                                , this.pat_info.PidInNo
                                                , this.pat_info.PidName
                                                , temp_pat_docName
                                                , temp_pat_bedNo
                                                , temp_pat_comName
                                                , temp_pat_repDate);
                    }

                    msg_content.ObrContent = message;

                    EntityDicObrMessageReceive rec = new EntityDicObrMessageReceive();
                    rec.DelFlag = false;
                    rec.ObrType = Convert.ToInt32(EnumObrMessageReceiveType.Dept);

                    if (useWardID && !string.IsNullOrEmpty(pat_info.PidWardId))
                    {
                        rec.ObrUserId = pat_info.PidWardId;
                    }
                    else
                    {
                        rec.ObrUserId = string.IsNullOrEmpty(this.pat_info.PidDeptId) &&
                                     !string.IsNullOrEmpty(this.pat_info.PidWardId)
                                         ? this.pat_info.PidWardId
                                         : this.pat_info.PidDeptId;
                    }

                    rec.ObrUserName = this.pat_info.PidDeptName;

                    msg_content.ListObrMessageReceiver.Add(rec);

                    new UserMessageBIZ().InsertMessage(msg_content);

                    #endregion

                    #region 清远市人民医院危值短信通知（单独判断是描述报告时发送【传染病消息】）

                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("IsUseUpdateInfectiousDisease") == "是"
                        && pat_info.RepRemark.ToString() != null && pat_info.RepRemark == "传染病")
                    {
                        EntityObrResult result = new EntityObrResult();
                        if (string.IsNullOrEmpty(itm_ecd))
                        {
                            result.ItmEname = "描述报告";
                        }
                        else {
                            result.ItmEname = itm_ecd;
                        }
                        if (string.IsNullOrEmpty(res_chr))
                        {
                            result.ObrValue = "描述报告";
                        }
                        else {
                            result.ObrValue = res_chr;
                        }
                        result.hasItmForillflag = true;
                        listUrgentResult.Add(result);
                    }
                    #endregion

                    //危急值发送短信
                    if (listUrgentResult.Count > 0)
                        DCLExtInterfaceFactory.DCLExtInterface.SendUrgentMessage(pat_info, listUrgentResult);
                }

                if (hasTJCriticalValues)
                {
                    //插入到用户消息表
                    EntityDicObrMessageContent msg_content = new EntityDicObrMessageContent();
                    msg_content.ObrCreateTime = ServerDateTime.GetDatabaseServerDateTime();
                    msg_content.DelFlag = false;
                    msg_content.ObrValueA = this.pat_info.RepId;
                    msg_content.ObrValueB = this.pat_info.PidName;
                    msg_content.ObrValueC = ex_item_value_unit2.ToString();
                    msg_content.ObrExpirationDate = null;
                    msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.TJ_CRITICAL_MESSAGE);
                    msg_content.ObrReceive = this.pat_info.PidDeptName;
                    msg_content.ObrMessageTitle = "危急值报告";

                    //床号
                    string temp_pat_bedNo = string.IsNullOrEmpty(this.pat_info.PidBedNo)
                                                ? ""
                                                : this.pat_info.PidBedNo;



                    string message = string.Format("危急值报告\r\n病区：{0}  病人号：{1} 姓名：{2} ", this.pat_info.PidDeptName
                                                   , this.pat_info.PidInNo
                                                   , this.pat_info.PidName);

                    if (config.bSecondAuditUrgentContainCom) //发送危急值含[组合]
                    {
                        //组合
                        string temp_pat_comName = string.IsNullOrEmpty(this.pat_info.PidComName)
                                                      ? "NULL"
                                                      : this.pat_info.PidComName;
                        //开单医生
                        string temp_pat_docName = string.IsNullOrEmpty(this.pat_info.PidDocName)
                                                      ? "NULL"
                                                      : this.pat_info.PidDocName;

                        //报告时间
                        string temp_pat_repDate = string.IsNullOrEmpty(this.pat_info.RepReportDate.ToString())
                                                      ? "NULL"
                                                      : this.pat_info.RepReportDate.ToString();


                        message = string.Format("(危急)开单医生：{3} 姓名：{2} 组合：{5} 病区：{0} 病人号：{1} 床号：{4} 报告时间：{6} ",
                                                this.pat_info.PidDeptName
                                                , this.pat_info.PidInNo
                                                , this.pat_info.PidName
                                                , temp_pat_docName
                                                , temp_pat_bedNo
                                                , temp_pat_comName
                                                , temp_pat_repDate);
                    }

                    msg_content.ObrContent = message;

                    EntityDicObrMessageReceive rec = new EntityDicObrMessageReceive();
                    rec.DelFlag = false;
                    rec.ObrType = Convert.ToInt32(EnumObrMessageReceiveType.Dept);

                    if (useWardID && !string.IsNullOrEmpty(pat_info.PidWardId))
                    {
                        rec.ObrUserId = pat_info.PidWardId;
                    }
                    else
                    {
                        rec.ObrUserId = string.IsNullOrEmpty(this.pat_info.PidDeptId) &&
                                     !string.IsNullOrEmpty(this.pat_info.PidWardId)
                                         ? this.pat_info.PidWardId
                                         : this.pat_info.PidDeptId;
                    }

                    rec.ObrUserName = this.pat_info.PidDeptName;

                    msg_content.ListObrMessageReceiver.Add(rec);

                    new UserMessageBIZ().InsertMessage(msg_content);//改造后
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(GetType().Name, "ThreadSendCriticalMessage", ex.ToString());
            }
        }



        void ThreadClearCriticalMessage(string p_strPat_id)
        {
            try
            {
                #region 更新危机值处理表数据删除标志,根据其信息ID包含于危急值信息表信息ID中

                List<EntityDicObrMessageContent> listMsgCon = new List<EntityDicObrMessageContent>();
                EntityDicObrMessageContent eyObrMsgCon = new EntityDicObrMessageContent();
                eyObrMsgCon.ObrValueA = p_strPat_id;
                eyObrMsgCon.DelFlag = true; //查询删除标志为0的数据
                listMsgCon = new ObrMessageContentBIZ().GetMessageByCondition(eyObrMsgCon);
                foreach (var info in listMsgCon)
                {
                    EntityDicObrMessageReceive eyObrMsgReceive = new EntityDicObrMessageReceive();
                    eyObrMsgReceive.ObrId = info.ObrId;
                    //new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateDelFlagByID(eyObrMsgReceive);
                    new ObrMessageReceiveBIZ().DeleteObrMessageReceive(eyObrMsgReceive);
                }
                #endregion

                //删除危急值处理表删除标志
                new ObrMessageContentBIZ().UPdateMessageDelFlagByObrValueA(p_strPat_id);
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ThreadClearCriticalMessage", ex.ToString());

            }
        }


        void ThreadClearCriticalMessage()
        {
            ThreadClearCriticalMessage(this.pat_info.RepId);
        }

        private void ThreadUpdateBacterialCriticalMessageForDCL(Object res)
        {
            try
            {
                EntityQcResultList ds = (EntityQcResultList)res;
                if (ds == null || ds.listPatients.Count == 0)
                {
                    return;
                }
                var dtbCriticalPat = ds.listPatients;
                var descList = ds.listDesc;
                var antiList = ds.listAnti;
                var bactlist = ds.listBact;
                //系统配置：细菌报告自定义危急值判断规则(关键字判断 分号为一组，逗号分开是一组的关键字)
                string Lab_DescBloodUrgRuleKeyWord = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_DescBloodUrgRuleKeyWord");

                //系统配置：细菌危急值判断模式
                string StrBacUrgenMode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_Sencond_BacUrgenMode");
                //系统配置：细菌危急值细菌与抗生素显示名称
                bool IsUseCName = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_Sencond_BacUrgenUseCName") == "是";

                //系统配置：药敏危急值显示细菌名称
                bool IsMicShowBaCName = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MicUrgen_ShowBacName") == "是";

                //系统配置：药敏危急值显示备注信息
                bool IsMicShowPatExp = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("MicUrgen_ShowPatExp") == "是";

                string strDrugfast_type = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Drugfast_DefaultType");
                if (StrBacUrgenMode == null) { StrBacUrgenMode = ""; }

                bool IsBacUrgenModeBySTZX = false;//汕头中心医院模式,是否有危急值
                if (dtbCriticalPat.Count > 0)
                {
                    foreach (var patinfo in dtbCriticalPat)
                    {
                        List<EntityObrResultAnti> listResult = new List<EntityObrResultAnti>();
                        IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
                        if (dao != null)
                        { 
                             listResult = dao.GetAntiResultById(patinfo.RepId);
                        }
                        bool isNeedUpdateUrgentFlag = false;
                        bool isDrugfastFlag = false;//只是多重耐药菌
                        string strFlag = patinfo.RepStatus.ToString();
                        string strUrgent_flag = patinfo.RepUrgentFlag.HasValue ? patinfo.RepUrgentFlag.ToString() : "0";
                        string strDrugfast_flag = patinfo.RepDrugfastFlag.ToString();//多重耐药标志 1-多重耐药 2-临床已确认
                        //传染病标志
                        bool isItmForillflag = ds.listBact == null ? false : ds.listBact.Exists(w => w.ObrIddFlag == 1 && w.ObrId == patinfo.RepId);
                        bool isSendMsg = false;
                        EntityObrResult result = new EntityObrResult();
                        if (patinfo.SampRemark == "传染病")
                        {
                            result.hasItmForillflag = true;
                            isSendMsg = true;
                        }
                        else
                        {
                            result.hasItmForillflag = false;
                        }
                        if ((strFlag == "0" || strFlag == "1") && descList.Count > 0
                            && !string.IsNullOrEmpty(Lab_DescBloodUrgRuleKeyWord))
                        {
                            var rows = descList.FindAll(a => a.ObrId == patinfo.RepId);
                            if (rows.Count > 0)
                            {
                                string samName = DictSampleCache.Current.GetSamCNameByID(patinfo.PidSamId);
                                if ((!string.IsNullOrEmpty(samName) && samName.Contains("血")))
                                {
                                    string[] keyWords = Lab_DescBloodUrgRuleKeyWord.Split(';');
                                    bool isFind = false;
                                    foreach (var row in rows)
                                    {
                                        if (!string.IsNullOrEmpty(row.ObrValue))
                                        {
                                            string bsr_cname = row.ObrValue;

                                            if (isFind) break;
                                            foreach (string words in keyWords)
                                            {
                                                foreach (string word in words.Split(','))
                                                {
                                                    if (bsr_cname.Contains(word))
                                                    {
                                                        isFind = true;
                                                    }
                                                    if (!bsr_cname.Contains(word))
                                                    {
                                                        isFind = false;
                                                        break;
                                                    }
                                                }
                                                if (isFind)
                                                {
                                                    strUrgent_flag = "1";
                                                    isNeedUpdateUrgentFlag = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if ((strFlag == "0" || strFlag == "1") && (strUrgent_flag == "1" || strUrgent_flag == "2" || StrBacUrgenMode == "汕头中心医院" || strDrugfast_flag == "1" || strDrugfast_flag == "2" || isItmForillflag))
                        {
                            //发送危急值数据
                            try
                            {
                                StringBuilder item_value_unit2 = new StringBuilder();

                                string bar_wjtext_str = "";//危急类型

                                if (descList.Count > 0 && (strUrgent_flag == "1" || strUrgent_flag == "2"))
                                {
                                    var rows = descList.FindAll(a => a.ObrId == patinfo.RepId);
                                    if (rows.Count > 0)
                                    {
                                        foreach (var row in rows)
                                        {
                                            if (row.ObrValue != null && row.ObrValue != "")
                                                item_value_unit2.AppendLine(row.ObrValue);
                                            IsBacUrgenModeBySTZX = true;
                                        }
                                    }
                                }

                                if (item_value_unit2.Length == 0 && antiList.Count > 0 && (strUrgent_flag == "1" || strUrgent_flag == "2" || StrBacUrgenMode == "汕头中心医院"))
                                {
                                    var rows = antiList.FindAll(a => a.ObrId == patinfo.RepId);
                                    //.Select("anr_id='" + patinfo["pat_id"].ToString() + "'");
                                    if (rows.Count > 0)
                                    {
                                        if (StrBacUrgenMode == "汕头中心医院")
                                        {

                                        }
                                        else
                                        {
                                            #region 通用
                                            if (!string.IsNullOrEmpty(rows[0].ObrRemark))
                                            {
                                                if (!string.IsNullOrEmpty(rows[0].ObrSterile))
                                                {
                                                    if (!bar_wjtext_str.Contains(rows[0].ObrSterile))
                                                    {
                                                        bar_wjtext_str += "[" + rows[0].ObrSterile + "]";
                                                    }

                                                    if (IsMicShowBaCName)
                                                    {
                                                        item_value_unit2.AppendLine("细菌名:" + rows[0].BacName + "," + rows[0].ObrRemark + "[" + rows[0].ObrSterile + "]");
                                                    }
                                                    else
                                                    {
                                                        item_value_unit2.AppendLine(rows[0].ObrRemark + "[" + rows[0].ObrSterile + "]");
                                                    }
                                                }
                                                else
                                                {
                                                    if (IsMicShowBaCName)
                                                    {
                                                        item_value_unit2.AppendLine("细菌名:" + rows[0].BacName + "," + rows[0].ObrRemark);
                                                    }
                                                    else
                                                    {
                                                        item_value_unit2.AppendLine(rows[0].ObrRemark);
                                                    }
                                                }
                                                IsBacUrgenModeBySTZX = true;
                                            }
                                            else
                                            {
                                                foreach (var row in rows)
                                                {
                                                    if (!string.IsNullOrEmpty(row.ObrValue) &&
                                                        !string.IsNullOrEmpty(row.ObrBacId))
                                                    {
                                                        if (IsUseCName)
                                                        {
                                                            if (!string.IsNullOrEmpty(row.ObrSterile))
                                                            {
                                                                if (!bar_wjtext_str.Contains(row.ObrSterile))
                                                                {
                                                                    bar_wjtext_str += "[" + row.ObrSterile + "]";
                                                                }
                                                                item_value_unit2.AppendLine(
                                                                    string.Format("细菌名:{0},抗生素名:{1},敏感度:{2},危急类型:{3}"
                                                                                  , row.BacName, row.AntCname, row.ObrValue, row.ObrSterile));
                                                            }
                                                            else
                                                            {
                                                                item_value_unit2.AppendLine(
                                                                    string.Format("细菌名:{0},抗生素名:{1},敏感度:{2}"
                                                                                  , row.BacName, row.AntCname, row.ObrValue));
                                                            }
                                                        }
                                                        else
                                                        {

                                                            if (!string.IsNullOrEmpty(row.ObrSterile))
                                                            {
                                                                if (!bar_wjtext_str.Contains(row.ObrSterile))
                                                                {
                                                                    bar_wjtext_str += "[" + row.ObrSterile + "]";
                                                                }

                                                                item_value_unit2.AppendLine(
                                                                    string.Format("细菌编码:{0},抗生素编码:{1},敏感度:{2},危急类型:{3}"
                                                                                  , row.ObrBacId, row.ObrAntId, row.ObrValue, row.ObrSterile));
                                                            }
                                                            else
                                                            {
                                                                item_value_unit2.AppendLine(
                                                                    string.Format("细菌编码:{0},抗生素编码:{1},敏感度:{2}"
                                                                                  , row.ObrBacId, row.ObrAntId, row.ObrValue));
                                                            }
                                                        }
                                                    }
                                                    else if (IsUseCName)
                                                    {
                                                        if (!string.IsNullOrEmpty(row.ObrSterile))
                                                        {
                                                            if (!bar_wjtext_str.Contains(row.ObrSterile))
                                                            {
                                                                bar_wjtext_str += "[" + row.ObrSterile + "]";
                                                            }
                                                            item_value_unit2.AppendLine(
                                                                string.Format("细菌名:{0},危急类型:{1}"
                                                                              , row.BacName, row.ObrSterile));
                                                        }
                                                        else
                                                        {
                                                            item_value_unit2.AppendLine(
                                                                string.Format("细菌名:{0}", row.BacName));
                                                        }
                                                    }
                                                    IsBacUrgenModeBySTZX = true;
                                                }
                                            }

                                            if (IsMicShowPatExp && !string.IsNullOrEmpty(patinfo.RepRemark))
                                            {
                                                //药敏危急值添加备注信息
                                                item_value_unit2.AppendLine(patinfo.RepRemark);
                                            }

                                            #endregion
                                        }
                                    }

                                }
                                else if (item_value_unit2.Length == 0 && antiList.Count > 0 && listResult.Count>0 && (strDrugfast_flag == "1" || strDrugfast_flag == "2"))//多重耐药
                                {
                                    #region 多重耐药标志

                                    //pat_drugfast   说明：多重耐药标志 1-多重耐药 2-临床已确认
                                    string bcName = string.Empty;
                                   List<EntityObrResultBact>  bactlistTemp = bactlist.FindAll(w => w.ObrMrbFlag == 1 && w.ObrId== patinfo.RepId);
                                    if (bactlistTemp.Count > 0)
                                    {
                                        foreach (EntityObrResultBact bac in bactlistTemp)
                                        {
                                            bcName +=string.Format(",{0}", bac.BacCname) ;
                                        }
                                        bcName = bcName.Remove(0, 1);
                                    }
                                    item_value_unit2.AppendLine("多重耐药菌" + "["+ bcName + "]");
                                    isDrugfastFlag = true;//标识为多重耐药菌

                                    #endregion
                                }
                                else if (item_value_unit2.Length == 0 && listResult.Count > 0 && isItmForillflag)//传染病
                                {
                                    #region 传染病标志
                                    string bcName = string.Empty;
                                    List<EntityObrResultBact> bactlistTemp = bactlist.FindAll(w => w.ObrIddFlag == 1 && w.ObrId == patinfo.RepId);
                                    if (bactlistTemp.Count > 0)
                                    {
                                        foreach (EntityObrResultBact bac in bactlistTemp)
                                        {
                                            bcName += string.Format(",{0}", bac.BacCname);
                                        }
                                        bcName = bcName.Remove(0, 1);
                                    }
                                    item_value_unit2.AppendLine("传染病菌" + "[" + bcName + "]");
                                    #endregion
                                }

                                if (StrBacUrgenMode == "汕头中心医院" && !IsBacUrgenModeBySTZX)
                                {
                                    continue;//不往下执行
                                }

                                if (item_value_unit2.Length > 130)
                                {
                                    string newvalue = item_value_unit2.ToString().Substring(0, 130) + ".....";
                                    item_value_unit2 = new StringBuilder();
                                    item_value_unit2.Append(newvalue);
                                }

                                /*******************20171211 SJC*****************************/
                                //插入到用户消息表
                                EntityDicObrMessageContent msg_content = new EntityDicObrMessageContent();
                                msg_content.ObrCreateTime = ServerDateTime.GetDatabaseServerDateTime();
                                msg_content.DelFlag = false;
                                msg_content.ObrValueA = patinfo.RepId;
                                msg_content.ObrValueB = patinfo.PidName;
                                msg_content.ObrValueC = item_value_unit2.ToString();
                                msg_content.ObrExpirationDate = null;
                                msg_content.ObrReceive = patinfo.PidDeptName;
                                if (isDrugfastFlag)
                                {
                                    msg_content.ObrValueD = "多重耐药菌";
                                    if (string.IsNullOrEmpty(strDrugfast_type) || strDrugfast_type == "急查")
                                    {
                                        msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.URGENT_MESSAGE);
                                        msg_content.ObrMessageTitle = "急查报告";
                                    }
                                    else {
                                        msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.CRITICAL_MESSAGE);
                                        msg_content.ObrMessageTitle = "危急值报告";
                                    }
                                }
                                else if (isItmForillflag)
                                {
                                    msg_content.ObrValueD = "传染病菌";
                                    msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.CRITICAL_MESSAGE);
                                    msg_content.ObrMessageTitle = "危急值报告";
                                }
                                else
                                {
                                    msg_content.ObrValueD = "微生物危急值";
                                    msg_content.ObrType = Convert.ToInt32(EnumObrMessageType.CRITICAL_MESSAGE);
                                    msg_content.ObrMessageTitle = "危急值报告";
                                }

                                string message = "";

                                if (!string.IsNullOrEmpty(bar_wjtext_str))
                                {
                                    message = string.Format("危急值报告\r\n病区：{0} 住院号：{1} 姓名：{2} 危急类型:{3}", patinfo.PidDeptName
                                                                                    , patinfo.PidInNo
                                                                                    , patinfo.PidName
                                                                                    , bar_wjtext_str);
                                }
                                else
                                {

                                    message = string.Format("危急值报告\r\n病区：{0} 住院号：{1} 姓名：{2}", patinfo.PidDeptName
                                                                                        , patinfo.PidInNo
                                                                                        , patinfo.PidName);
                                }

                                if (isDrugfastFlag)
                                {
                                    if (string.IsNullOrEmpty(strDrugfast_type) || strDrugfast_type == "急查")
                                    {
                                        message = string.Format("急查报告\r\n病区：{0} 住院号：{1} 姓名：{2}", patinfo.PidDeptName
                                                                         , patinfo.PidInNo
                                                                         , patinfo.PidName);
                                    }
                                    else
                                    {
                                        message = string.Format("危急值报告\r\n病区：{0} 住院号：{1} 姓名：{2}", patinfo.PidDeptName
                                                                                  , patinfo.PidInNo
                                                                                  , patinfo.PidName);
                                    }
                     
                                }
                                else if (isItmForillflag)
                                {
                                    message = string.Format("危急值报告\r\n病区：{0} 住院号：{1} 姓名：{2}", patinfo.PidDeptName
                                                                                  , patinfo.PidInNo
                                                                                  , patinfo.PidName);
                                }

                                msg_content.ObrContent = message;

                                bool useWardID = CacheSysConfig.Current.GetSystemConfig("Urgent_Dept_Column") == "pat_ward_id";

                                EntityDicObrMessageReceive rec = new EntityDicObrMessageReceive();
                                rec.DelFlag = false;
                                rec.ObrType = Convert.ToInt32(EnumObrMessageReceiveType.Dept);

                                if (useWardID && !string.IsNullOrEmpty(patinfo.PidWardId))
                                {
                                    rec.ObrUserId = patinfo.PidWardId;
                                }
                                else
                                {
                                    rec.ObrUserId = patinfo.PidDeptId;
                                }
                                rec.ObrUserName = patinfo.PidDeptName.ToString();

                                msg_content.ListObrMessageReceiver.Add(rec);

                                //new dcl.svr.msg.MessageBiz().SendMessage(msg_content);//改造前
                                new UserMessageBIZ().InsertMessage(msg_content);//改造后

                            }
                            catch (Exception ex)
                            {

                                Logger.WriteException(this.GetType().Name, "ThreadSendCriticalMessage", ex.ToString());
                            }


                        }
                        else
                        {
                            //清除危急值数据
                            ThreadClearCriticalMessage(patinfo.RepId.ToString());
                        }
                        //判断是否发短信，再在里面判断是发危急值短信还是传染病短信   清远人医发送传染病短信
                        if (isSendMsg && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("IsUseUpdateInfectiousDisease") == "是" && patinfo.RepRemark != null && patinfo.RepRemark == "传染病")
                        {
                            result.ObrValue = "";
                            result.ItmEname = "";
                            listUrgentResult.Add(result);
                            DCLExtInterfaceFactory.DCLExtInterface.SendUrgentMessage(pat_info, listUrgentResult);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ThreadUpdateBacterialCriticalMessageForDCL", ex.ToString());
            }
        }

       
        #region 获取危急值提示
        /// <summary>
        /// 判断是否异常或危机
        /// </summary>
        /// <param name="res_ref_flag"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private string dict_res_ref_flag(string res_ref_flag)
        {
            bool Isref = false;//是否异常或危急
            string description = "";

            switch (res_ref_flag)
            {
                case "-1":
                    description = "无判定提示";
                    Isref = false;
                    break;
                case "0":
                    description = "正常";
                    Isref = false;
                    break;
                case "1":
                    description = "↑";
                    Isref = false;
                    break;
                case "2":
                    description = "↓";
                    Isref = false;
                    break;
                case "3":
                    description = "阳性";
                    Isref = true;
                    break;
                case "4":
                    description = "弱阳性";
                    Isref = true;
                    break;
                case "6":
                    description = "超出异常值";//危急值
                    Isref = true;
                    break;
                case "8":
                    description = "高于参考值";
                    Isref = false;
                    break;
                case "16":
                    description = "超出异常值";//高于危急值
                    Isref = true;
                    break;
                case "24":
                    description = "超出异常值";//高于危急值
                    Isref = true;
                    break;
                case "32":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "40":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "48":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "56":
                    description = "超出危急值";//高于阈值
                    Isref = true;
                    break;
                case "128":
                    description = "低于参考值";
                    Isref = false;
                    break;
                case "256":
                    description = "超出异常值";//低于危急值
                    Isref = true;
                    break;
                case "384":
                    description = "超出异常值";//低于危急值
                    Isref = true;
                    break;
                case "512":
                    description = "超出危急值";//低于阈值下限
                    Isref = true;
                    break;
                case "640":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                case "768":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                case "896":
                    description = "超出危急值";//低于阈值
                    Isref = true;
                    break;
                default:
                    description = "";
                    Isref = false;
                    break;
            }

            return description;

        }

        #endregion
    }
}
