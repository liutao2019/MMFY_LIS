using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.msg
{
    /// <summary>
    /// 危急值数据
    /// </summary>
    public class UrgentObrMessageBIZ : IDicUrgentObrMessage
    {
        public List<EntityPidReportMain> GetUrgentMsgToCache()
        {
            bool UrgentMessage_ShowPrintedRepMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("UrgentMessage_ShowPrintedRepMsg") != "否";

            //系统配置：允许服务端缓存危急值信息
            bool UrgentMessage_AllowWCFCache = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_AllowWCFCache") != "否";

            if (!UrgentMessage_AllowWCFCache)
            {
                //如果不缓存,则返回空
                return null;
            }

            string pat_flag_sql = string.Empty;
            if (UrgentMessage_ShowPrintedRepMsg)
            {
                pat_flag_sql = "2,4";
            }
            else
            {
                pat_flag_sql = "2";
            }

            List<EntityPidReportMain> listPatsAll = new List<EntityPidReportMain>();//存放所有信息
            try
            {
                List<EntityPidReportMain> listPatFlag = GetUrgentMsgByPatFlag(pat_flag_sql);//获取危急与急查信息
                listPatsAll = listPatFlag;

                #region 中山三院 住院的病人ID取pid_social_no
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("HospitalName") == "中山三院")
                {
                    foreach (var info in listPatsAll)
                    {
                        string pidSocialNo = info.PidSocialNo;
                        if ((info.PidSrcId == "108" || info.PidIdtId == "106") && !string.IsNullOrEmpty(pidSocialNo))
                        {
                            info.PidInNo = pidSocialNo;
                        }
                    }
                }
                #endregion


                //当二审时可以不发送危急，则不查询
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditCheckResultViwer_showCol_UnSendUrg") != "是")//内部提醒危急值(msg_content无数据时,来源为住院)
                {
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CannotSendClinic") != "是")
                    {
                        //DataTable dtbResultInner = objHelper.GetTable(strSQLInner);

                        //查询内部提醒危急值(msg_content无数据时,来源为住院)
                        List<EntityPidReportMain> listPatInner = GetUrgentMsgToA();

                        if (listPatInner != null && listPatInner.Count > 0)
                        {
                            System.Diagnostics.Debug.WriteLine("危急值缓存，内部提醒危急信息：" + listPatInner.Count.ToString());
                            //dtbResult.Merge(dtbResultInner);
                            foreach (var infoInner in listPatInner)
                            {
                                listPatsAll.Add(infoInner);
                            }
                        }
                    }

                }

                //排序
                if (listPatsAll != null && listPatsAll.Count > 0)
                {
                    listPatsAll = listPatsAll.OrderByDescending(i => i.RepAuditDate).ThenBy(i => i.ObrType).ToList();
                }

                if (true)//自编危急
                {
                    //DataTable dtbResultDIY = objHelper.GetTable(strSQLDIY);
                    List<EntityPidReportMain> listPatsDIY = GetUrgentMsgToB();//查询自编危急
                    if (listPatsDIY != null && listPatsDIY.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("危急值缓存，自编危急信息：" + listPatsDIY.Count.ToString());
                        //dtbResult.Merge(dtbResultDIY);
                        listPatsAll.AddRange(listPatsDIY);
                    }
                }

                if (listPatsAll != null && listPatsAll.Count > 0)
                {
                    string temppatInNoZy = "";//住院病人住院号
                    List<string> temppatInNoZyList = new List<string>();//住院病人住院号集
                    List<string> patIDList = new List<string>();
                    for (int i = listPatsAll.Count - 1; i >= 0; i--)
                    {
                        string pat_id = listPatsAll[i].RepId;
                        if (patIDList.Contains(pat_id))//过滤重复的信息(如,既是急查又是危急值的信息)
                        {
                            listPatsAll.Remove(listPatsAll[i]);
                        }
                        else
                        {
                            patIDList.Add(pat_id);
                        }

                        if (listPatsAll[i].PidSrcId == "108" && listPatsAll[i].PidInNo.ToString().Length > 0)
                        {
                            if (!temppatInNoZyList.Contains(listPatsAll[i].PidInNo))
                            {
                                temppatInNoZyList.Add(listPatsAll[i].PidInNo);
                                temppatInNoZy += "'" + listPatsAll[i].PidInNo + "',";
                            }
                        }
                    }

                    #region 陈星海his连接字符串,检查转科室信息  (暂时先屏蔽,需要用时再修改)
                    ////陈星海his连接字符串,检查转科室信息
                    //string CXHYYhisConnectionString = System.Configuration.ConfigurationManager.AppSettings["CXHYYhisConnectionString"];
                    //if (!string.IsNullOrEmpty(CXHYYhisConnectionString) && temppatInNoZyList.Count > 0)
                    //{
                    //    try
                    //    {
                    //        temppatInNoZy = temppatInNoZy.TrimEnd(new char[] { ',' });
                    //        DBHelper objHelper2 = new DBHelper(CXHYYhisConnectionString);
                    //        string sqlhisview = string.Format("select BAHM,BRKS,KSMC from Lis_brks where BAHM in({0})", temppatInNoZy);
                    //        //BAHM  --住院号
                    //        //BRKS  --科室代码
                    //        //KSMC  --科室名称
                    //        DataTable dthisview = objHelper2.GetTable(sqlhisview);

                    //        if (dthisview != null && dthisview.Rows.Count > 0)
                    //        {
                    //            //pat_dep_id dep_name
                    //            for (int j = 0; j < dthisview.Rows.Count; j++)
                    //            {
                    //                //DataRow[] drtempsel = dtbResult.Select("pat_ori_id='108' and pat_in_no='" + dthisview.Rows[j]["BAHM"].ToString() + "' and pat_dep_id<>'" + dthisview.Rows[j]["BRKS"].ToString() + "'");
                    //                List<EntityPatients> listTempSel = listPatsAll.Where(w => w.PidSrcId == "108" &&
                    //                                                                      w.PidInNo == dthisview.Rows[j]["BAHM"].ToString() &&
                    //                                                                      w.PidDeptId == dthisview.Rows[j]["BRKS"].ToString()).ToList();
                    //                if (listTempSel.Count > 0)
                    //                {
                    //                    for (int k = 0; k < listTempSel.Count; k++)
                    //                    {
                    //                        listTempSel[k].PidDeptId = dthisview.Rows[j]["BRKS"].ToString();
                    //                        listTempSel[k].DeptName = dthisview.Rows[j]["KSMC"].ToString();
                    //                    }
                    //                }
                    //            }
                    //            //dtbResult.AcceptChanges();
                    //        }

                    //    }
                    //    catch (Exception objEx2)
                    //    {
                    //        Lib.LogManager.Logger.LogException("获取陈星海HIS库数据", objEx2);
                    //    }
                    //}
                    #endregion

                }
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReturnMessages_IsNotify") == "是")
                {
                    //DataTable dtbResultReturn = objHelper.GetTable(strReturn);
                    List<EntityPidReportMain> listPatsReturn = GetUrgentMsgToC();
                    if (listPatsReturn != null && listPatsReturn.Count > 0)
                    {
                        //dtbResult.Merge(dtbResultReturn);
                        listPatsAll.AddRange(listPatsReturn);
                    }
                }

            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取危急值数据", objEx);
            }

            return listPatsAll;
        }

        public List<EntityPidReportMain> GetUrgentMsgByPatFlag(string pat_flag)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentMsgByPatFlag(pat_flag);
            }
            return listPats;
        }

        public List<EntityPidReportMain> GetUrgentMsgToA()
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentMsgToA();
            }
            return listPats;
        }

        public List<EntityPidReportMain> GetUrgentMsgToB()
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentMsgToB();
            }
            return listPats;
        }

        public List<EntityPidReportMain> GetUrgentMsgToC()
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentMsgToC();
            }
            return listPats;
        }

        public List<EntityPidReportMain> GetUrgentHistoryMsgByDep(EntityUrgentHistoryUseParame entityParame)
        {
            string p_receiver_id = entityParame.ReceiveID;//科室ID

            //获取一个病区的代码
            p_receiver_id = new ObrMessageBIZ().GetDepInwhereStr(p_receiver_id);

            if (string.IsNullOrEmpty(p_receiver_id))//如果为空,转空字符串
                p_receiver_id = "''";


            List<EntityPidReportMain> listUrgAll = new List<EntityPidReportMain>();
            /************下面这个是为了测试才加上的，测试完要删除*********/
            UrgentObrMessageCache.Current.Refresh();//测试完后记得删除
            /*****************/
            List<EntityPidReportMain> listUrgAllCache = UrgentObrMessageCache.Current.cache;

            if (listUrgAllCache != null && listUrgAllCache.Count > 0)
            {
                //if (ds.Tables["getCacheByDep"].Columns.Contains("is_neglect_dep"))//是否忽略科室(或病区)
                string p_neg_dep = entityParame.IsNeglectDep;//忽略科室(或病区)

                if ((!string.IsNullOrEmpty(p_neg_dep)) && p_neg_dep == "1")//1代表忽略
                {
                    //strWhere = " 1=1 and (pat_dep_id is not null and pat_dep_id<>'') ";
                    listUrgAll = listUrgAllCache.Where(w => !string.IsNullOrEmpty(w.PidDeptId)).ToList();
                }
                else   //不忽略科室
                {
                    //string strWhere = string.Format(" pat_dep_id in ({0}) ", p_receiver_id);//筛选条件
                    string[] strArray = p_receiver_id.Replace("'", "").Split(',');
                    foreach (var str in strArray)
                    {
                        foreach (var info in listUrgAllCache)
                        {
                            if (info.PidDeptId.Equals(str))
                                listUrgAll.Add(info);
                        }
                    }
                }
            }
            listUrgAll = listUrgAll.Distinct().ToList();
            //if (ds.Tables["getCacheByDep"].Columns.Contains("pat_doc_id"))//医生代码
            string p_doc_code = entityParame.PatDocId;//医生代码
            if ((!string.IsNullOrEmpty(p_doc_code)))
            {
                //strWhere += string.Format(" and pat_doc_id='{0}' ", p_doc_code);
                listUrgAll = listUrgAll.Where(w => w.PidDoctorCode == p_doc_code).ToList();
            }

            //if (ds.Tables["getCacheByDep"].Columns.Contains("pat_ori_config"))//是否有病人来源配置
            string p_ori_id = entityParame.PatOriConfig;//病人来源配置
            if (!string.IsNullOrEmpty(p_ori_id))
            {
                //值为1111 分别代表 住院,门诊,体检,其他
                char[] chr = p_ori_id.ToCharArray();

                for (int i = 0; i < chr.Length; i++)
                {
                    if (i == 0)
                    {
                        if (chr[i] == '0')
                        {
                            //strWhere += " and pat_ori_id<>'108' ";
                            listUrgAll = listUrgAll.Where(w => w.PidSrcId != "108").ToList();
                        }
                    }
                    if (i == 1)
                    {
                        if (chr[i] == '0')
                        {
                            //strWhere += " and pat_ori_id<>'107' ";
                            listUrgAll = listUrgAll.Where(w => w.PidSrcId != "107").ToList();
                        }
                    }
                    if (i == 2)
                    {
                        if (chr[i] == '0')
                        {
                            //strWhere += " and pat_ori_id<>'109' ";
                            listUrgAll = listUrgAll.Where(w => w.PidSrcId != "109").ToList();
                        }
                    }
                    if (i == 3)
                    {
                        if (chr[i] == '0')
                        {
                            //strWhere += " and (pat_ori_id='107' or pat_ori_id='108' or pat_ori_id='109') ";
                            listUrgAll = listUrgAll.Where(w => w.PidSrcId == "107" || w.PidSrcId == "108" || w.PidSrcId == "109").ToList();
                        }
                    }
                }
            }

            //if (ds.Tables["getCacheByDep"].Columns.Contains("msg_type"))//消息类型
            string msg_type = entityParame.MsgType;
            if ((!string.IsNullOrEmpty(msg_type)))
            {
                //strWhere += string.Format(" and msg_type={0} ", msg_type);
                listUrgAll = listUrgAll.Where(w => w.ObrType.ToString() == msg_type).ToList();
            }
            else
            {
                //strWhere += string.Format(" and msg_type<>3024 ");
                listUrgAll = listUrgAll.Where(w => w.ObrType != 3024).ToList();
            }

            return listUrgAll;
        }

        public List<EntityPidReportMain> GetUrgentHistoryMsgSqlWhere(EntityUrgentHistoryUseParame entityParame)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentHistoryMsgSqlWhere(entityParame);
            }
            return listPats;
        }

        public List<EntityPidReportMain> GetUrgentHistoryMsgGetAll(EntityUrgentHistoryUseParame entityParame)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            try
            {
                int temp_fMinutes = 0;
                int temp_fMinutes2 = 0;

                if (entityParame != null)
                {
                    if (int.TryParse(entityParame.FilterTime.ToString(), out temp_fMinutes))
                    {
                        // temp_fMinutes = temp_fMinutes > 0 ? temp_fMinutes : -temp_fMinutes;
                    }
                }
                if (entityParame != null)
                {
                    if (int.TryParse(entityParame.FilterTime2.ToString(), out temp_fMinutes2))
                    {
                        //temp_fMinutes2 = temp_fMinutes2 > 0 ? temp_fMinutes2 : -temp_fMinutes2;
                    }
                }

                bool isDIYCritical = false;//是否只接收自编危急信息 2024

                if (!string.IsNullOrEmpty(entityParame.IsOnlyDIY))
                {
                    if (entityParame.IsOnlyDIY.ToString() == "1")
                    {
                        isDIYCritical = true;
                    }
                }
                DateTime filterTime = DateTime.Now.AddMinutes(-temp_fMinutes);//获取推迟多少分钟的危急值信息

                DateTime filterTime2 = DateTime.Now.AddMinutes(-temp_fMinutes2);//获取推迟多少分钟的急查信息

                //读取缓存数据
                //DataTable dtUrgAll = UrgentMessageCache.Current.GetDTUrgentMessage(strWhere);
                List<EntityPidReportMain> listUrgAll = GetUrgentMsgToCache();//也可以用UrgentObrMessageCache.Current.cache;

                List<EntityPidReportMain> listCopy = new List<EntityPidReportMain>();
                listCopy.AddRange(listUrgAll);

                string strWhere = string.Empty;
                if (temp_fMinutes > 0)
                {
                    if (isDIYCritical)
                    {
                        //strWhere = string.Format(" ((msg_type=2024) and msg_create_time<='{0}') ", filterTime);
                        strWhere = "temp_fMinutes > 0";//随意赋值，用来判断
                        listUrgAll = listUrgAll.Where(a => a.ObrType == 2024 && a.ObrCreateTime <= filterTime).ToList();
                    }
                    else
                    {
                        //strWhere = string.Format(" ((msg_type=1024 or msg_type=3024) and msg_create_time<='{0}') ", filterTime);
                        strWhere = "isDIYCritical-is-false";
                        listUrgAll = listUrgAll.Where(w => (w.ObrType == 1024 || w.ObrType == 3024) && w.ObrCreateTime <= filterTime).ToList();
                    }
                }

                List<EntityPidReportMain> list = new List<EntityPidReportMain>();

                if (temp_fMinutes2 > 0)
                {
                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += " or ";
                    }
                    //strWhere += string.Format("(msg_type=4096 and msg_create_time<='{0}')", filterTime2);
                    strWhere += "is-have-or";
                    list = listCopy.Where(a => a.ObrType == 4096 && a.ObrCreateTime <= filterTime2).ToList();

                    listUrgAll.AddRange(list);//此时查询出的是or条件的所有值集合

                    listUrgAll = listUrgAll.Distinct().ToList();//去除相同的元素
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    // strWhere = string.Format("({0})", strWhere);
                }
                else
                {
                    //strWhere = "msg_type=0";
                    strWhere = "MsgTypeIs0";
                    listUrgAll = listUrgAll.Where(w => w.ObrType == 0).ToList();
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    //strWhere += " and (pat_dep_id is not null and pat_dep_id<>'') ";
                    strWhere += "PatDepIDAnd";
                    listUrgAll = listUrgAll.Where(w => !string.IsNullOrEmpty(w.PidDeptId)).ToList();
                }
                else
                {
                    //strWhere = " (pat_dep_id is not null and pat_dep_id<>'') ";
                    strWhere = "PatDepIDNotAnd";
                    listUrgAll = listUrgAll.Where(w => !string.IsNullOrEmpty(w.PidDeptId)).ToList();
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    //strWhere += " and (msf_insgin_flag is null or msf_insgin_flag='' or msf_insgin_flag<>'1') ";
                    strWhere += "MsfInsginFlag";
                    listUrgAll = listUrgAll.Where(w => string.IsNullOrEmpty(w.ObrInsideFlag) || w.ObrInsideFlag != "1").ToList();
                }
                else
                {
                    //strWhere = " (msf_insgin_flag is null or msf_insgin_flag='' or msf_insgin_flag<>'1') ";
                    strWhere = "MsfInsginFlagNoAnd";
                    listUrgAll = listUrgAll.Where(w => string.IsNullOrEmpty(w.ObrInsideFlag) || w.ObrInsideFlag != "1").ToList();
                }

                if (!string.IsNullOrEmpty(entityParame.DepIDs))
                {
                    string depids = entityParame.DepIDs;
                    string DepStr = "";

                    string[] darry = depids.Split(',');

                    foreach (string drDep in darry)
                    {
                        if (string.IsNullOrEmpty(DepStr))
                        {
                            DepStr = "'" + drDep + "'";
                        }
                        else
                        {
                            DepStr += ",'" + drDep + "'";
                        }
                    }

                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        //strWhere += string.Format(" and (msg_send_depcode in ({0})) ", DepStr);
                        strWhere += "MsgSendDepcodeHaveIn";
                        listUrgAll = listUrgAll.Where(w => DepStr.Contains(w.ObrSendDeptCode)).ToList();
                    }
                    else
                    {
                        //strWhere = string.Format(" and (msg_send_depcode in ({0})) ", DepStr);
                        strWhere = "MsgSendDepcodeHaveIn";
                        listUrgAll = listUrgAll.Where(w => DepStr.Contains(w.ObrSendDeptCode)).ToList();
                    }
                }

                if (listUrgAll != null && listUrgAll.Count > 0)
                {
                    //dtUrgAll.TableName = "UrgentMsg";
                    listPats = listUrgAll;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listPats;
        }

        public void RefreshUrgentMessage()
        {
            UrgentObrMessageCache.Current.Refresh();
        }
    }
}
