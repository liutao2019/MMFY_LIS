using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.svr.dicbasic;
using dcl.dao.interfaces;
using dcl.common;
using System.Data.OracleClient;
using System.Data;
using dcl.svr.users;
using dcl.svr.cache;
using dcl.svr.sample;

namespace dcl.svr.msg
{
    /// <summary>
    /// 危急值消息
    /// </summary>
    public class ObrMessageBIZ : IDicObrMessage
    {
        private ObrMessageReceiveCollection cache = null;

        public ObrMessageBIZ()
        {
            cache = new ObrMessageReceiveCollection();
        }

        /// <summary>
        /// 根据科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetDeptMessageByDeptCode(string dept_code)
        {
            if (dept_code == null)
            {
                return cache;
            }
            else
            {
                int Int_AddDays = -1;//默认24小时内

                //系统配置：[旧危急值]提醒几小时内的消息
                string Str_AddDays = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Old_CacheHour");

                if (!string.IsNullOrEmpty(Str_AddDays))
                {
                    if (Str_AddDays == "48")
                    {
                        Int_AddDays = -2;
                    }
                    else if (Str_AddDays == "72")
                    {
                        Int_AddDays = -3;
                    }
                }

                //                string sql = string.Format(@"select * from dict_depart where dep_ward =
                //(select top 1 dep_ward from dbo.dict_depart where dep_code='{0}' )", dept_code);

                //                DataTable dtDep = db.GetTable(sql);

                #region 根据科室代码查询科室数据
                EntityResponse deptRep = new EntityResponse();
                deptRep = new DepartBIZ().Search(new EntityRequest());
                List<EntityDicPubDept> listPubDept = deptRep.GetResult() as List<EntityDicPubDept>;

                List<EntityDicPubDept> listIn = listPubDept.Where(w => w.DeptCode == dept_code).ToList();
                string deptParentId = string.Empty;
                if (listIn.Count > 0)
                {
                    deptParentId = listIn[0].DeptParentId;
                }
                List<EntityDicPubDept> listDep = listPubDept.Where(w => w.DeptParentId == deptParentId).ToList();
                #endregion

                List<string> depIDs = new List<string>();

                IEnumerable<EntityDicObrMessageReceive> query;
                if (listDep.Count > 0)
                {
                    foreach (var infoDep in listDep)
                    {
                        depIDs.Add(infoDep.DeptCode);
                    }

                    query = from item in this.cache
                            where depIDs.Contains(item.ObrUserId.ToString().Trim()) && item.DelFlag == false
                                && item.ObrMessageContent.ObrCreateTime >= DateTime.Now.AddDays(Int_AddDays)
                                //&& item.ObrMessageContent.ObrType != EnumMessageType.URGENT_MESSAGE
                                && item.ObrMessageContent.ObrType != 4096
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;
                }
                else
                {
                    query = from item in this.cache
                            where item.ObrUserId.ToString().Trim() == dept_code && item.DelFlag == false
                            && item.ObrMessageContent.ObrCreateTime >= DateTime.Now.AddDays(Int_AddDays)
                             //&& item.ObrMessageContent.ObrType != EnumMessageType.URGENT_MESSAGE
                             && item.ObrMessageContent.ObrType != 4096
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;
                }
                ObrMessageReceiveCollection list = new ObrMessageReceiveCollection(query);
                return list;
            }
        }

        /// <summary>
        /// 根据多个科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetMessageByDepts(string dept_codes)
        {
            if (dept_codes == null)
            {
                return cache;
            }
            else
            {
                int Int_AddDays = -1;//默认24小时内

                //系统配置：[旧危急值]提醒几小时内的消息
                string Str_AddDays = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Old_CacheHour");

                if (!string.IsNullOrEmpty(Str_AddDays))
                {
                    if (Str_AddDays == "48")
                    {
                        Int_AddDays = -2;
                    }
                    else if (Str_AddDays == "72")
                    {
                        Int_AddDays = -3;
                    }
                }

                string dept_code = "";//如果没病区,默认用此科室查询

                string dept_codeIn = "";//多病区条件

                if (dept_codes.Contains(","))
                {
                    foreach (string strTemp in dept_codes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        //随机默认取第一个为默认病区
                        if (string.IsNullOrEmpty(dept_code))
                        {
                            dept_code = strTemp;
                        }

                        if (string.IsNullOrEmpty(dept_codeIn))
                        {
                            dept_codeIn = "'" + strTemp + "'";
                        }
                        else
                        {
                            dept_codeIn += ",'" + strTemp + "'";
                        }
                    }
                }
                else
                {
                    dept_code = dept_codes;
                }

                //                string sql = string.Format(@"select * from dict_depart where dep_ward in
                //(select dep_ward from dbo.dict_depart where dep_code in({0}) )", dept_codeIn);

                //                DataTable dtDep = db.GetTable(sql);
                #region 根据多个科室代码查询科室数据
                EntityResponse deptRep = new EntityResponse();
                deptRep = new DepartBIZ().Search(new EntityRequest());
                List<EntityDicPubDept> listPubDept = deptRep.GetResult() as List<EntityDicPubDept>;
                //筛选出符合条件的病区集合（dep_ward）
                List<EntityDicPubDept> listIn = listPubDept.Where(w => dept_codeIn.Contains(w.DeptCode)).ToList();
                string listStr = "(";
                foreach (var info in listIn)
                {
                    listStr += ",'" + info.DeptParentId + "'";
                }
                listStr += ")";
                listStr = listStr.Remove(1, 1);//去除第一个逗号
                List<EntityDicPubDept> listFind = listPubDept.Where(w => listStr.Contains(w.DeptParentId)).ToList();
                #endregion

                List<string> depIDs = new List<string>();

                IEnumerable<EntityDicObrMessageReceive> query;
                if (listFind.Count > 0)
                {
                    foreach (var infoDep in listFind)
                    {
                        depIDs.Add(infoDep.DeptCode);
                    }

                    query = from item in this.cache
                            where depIDs.Contains(item.ObrUserId.ToString().Trim()) && item.DelFlag == false
                                        && item.ObrMessageContent.ObrCreateTime >= DateTime.Now.AddDays(Int_AddDays)
                                        && item.ObrMessageContent.ObrType != 4096 //EnumMessageType.URGENT_MESSAGE 急查标志消息
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;
                }
                else
                {
                    query = from item in this.cache
                            where item.ObrUserId.ToString().Trim() == dept_code && item.DelFlag == false
                               && item.ObrMessageContent.ObrCreateTime >= DateTime.Now.AddDays(Int_AddDays)
                               //&& item.ObrMessageContent.ObrType != EnumMessageType.URGENT_MESSAGE
                               && item.ObrMessageContent.ObrType != 4096
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;
                }
                ObrMessageReceiveCollection list = new ObrMessageReceiveCollection(query);
                return list;
            }
        }

        public void RefreshDeptMessage()
        {
            this.cache = new ObrMessageReceiveBIZ().GetMessageByReceiverID(null, EnumObrMessageReceiveType.Dept, false, true, false);
        }

        public bool DeleteMessageByIDPatId(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id)
        {
            bool isDelByID = false;
            IDaoDeptMessage dao = DclDaoFactory.DaoHandler<IDaoDeptMessage>();
            if (dao != null)
            {
                isDelByID = dao.DeleteMessageByIDPatId(objAuditInfo, messageID, bPhiDelete, pat_id);
            }
            return isDelByID;
        }

        //public bool DeleteMessageByID(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete)
        //{
        //    bool isDelByID = false;
        //    IDaoDeptMessage dao = DclDaoFactory.DaoHandler<IDaoDeptMessage>();
        //    if (dao != null)
        //    {
        //        isDelByID = dao.DeleteMessageByID(objAuditInfo, messageID, bPhiDelete);
        //    }
        //    return isDelByID;


        //}
        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID"></param>
        /// <param name="bPhiDelete"></param>
        /// <returns></returns>
        public bool DeleteMessageByID(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete)
        {
            bool isDelMsgByID = false;
            string sqlDeleteContent = string.Empty;
            string sqlDeleteReceiver = string.Empty;
            try
            {
                //作为危急值处理更新操作的实体参数
                EntityDicObrMessageContent obrMsgContent = new EntityDicObrMessageContent();
                obrMsgContent.ObrAuditUserId = objAuditInfo.UserId;
                obrMsgContent.ObrAuditUserName = objAuditInfo.UserName;
                obrMsgContent.ObrConfirmType = objAuditInfo.MsgAffirmType;
                obrMsgContent.ObrId = messageID;

                if (bPhiDelete)//物理删除
                {
                    EntityDicObrMessageContent eyMsgContent = new EntityDicObrMessageContent();
                    eyMsgContent.ObrId = messageID;
                    isDelMsgByID = new ObrMessageContentBIZ().DeleteObrMessageContent(eyMsgContent);
                    EntityDicObrMessageReceive eyMsgReceive = new EntityDicObrMessageReceive();
                    eyMsgReceive.ObrId = messageID;
                    isDelMsgByID = new ObrMessageReceiveBIZ().DeleteObrMessageReceive(eyMsgReceive);
                }
                else
                {
                    //只标记内部提醒已处理
                    if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                    {
                        isDelMsgByID = new ObrMessageContentBIZ().UpdateObrMsgConToInsignByID(obrMsgContent);
                        EntityDicObrMessageReceive eyObrMsgReceive = new EntityDicObrMessageReceive();
                        eyObrMsgReceive.ObrId = messageID;
                        isDelMsgByID = new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateByID(eyObrMsgReceive);
                    }
                    else
                    {
                        isDelMsgByID = new ObrMessageContentBIZ().UpdateObrMsgConToDelFlagByID(obrMsgContent);
                        EntityDicObrMessageReceive eyObrMsgReceive = new EntityDicObrMessageReceive();
                        eyObrMsgReceive.ObrId = messageID;
                        isDelMsgByID = new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateDelFlagByID(eyObrMsgReceive);
                    }
                }
                return isDelMsgByID;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public bool DeleteMessageByIDAndUpdateCriticalChecker(EntityAuditInfo objAuditInfo, string messageID, string pat_id)
        {
            bool isDelByIDAndUpdate = false;
            EntityPidReportMain pidReportMain = new EntityPidReportMain();
            //根据pat_id获取病人信息
            IDaoPidReportMain mianDao= DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if(mianDao!=null)
            {
                pidReportMain = mianDao.GetPatientInfo(pat_id);
            }
            string confirmType = objAuditInfo.MsgContent;
            IDaoDeptMessage dao = DclDaoFactory.DaoHandler<IDaoDeptMessage>();
            if (dao != null)
            {
                #region 梧州人医 当医生和护士都确认危急值 才更新危急值删除标志  以及查看标志

                if (!string.IsNullOrEmpty(objAuditInfo.UserRole))
                {
                    if (objAuditInfo.UserRole == "nurse")
                    {
                        isDelByIDAndUpdate = new ObrMessageContentBIZ().UpdateReadFlag(objAuditInfo.UserRole, messageID);
                        confirmType = "护士确认_"+objAuditInfo.MsgContent;
                    }
                    else if (objAuditInfo.UserRole == "doctor")
                    {
                        isDelByIDAndUpdate = new ObrMessageContentBIZ().UpdateReadFlag(objAuditInfo.UserRole, messageID);
                        confirmType = "医生确认_"+objAuditInfo.MsgContent;
                    }
                    if (string.IsNullOrEmpty(objAuditInfo.UserName))
                    {
                        objAuditInfo.UserName = objAuditInfo.UserId;
                    }
                    EntityDicObrMessageContent content = new EntityDicObrMessageContent();
                    content.ObrValueA = pat_id;
                    List<EntityDicObrMessageContent> listContent = new ObrMessageContentBIZ().GetMessageByCondition(content);
                    if (listContent != null && listContent.Count > 0)
                    {
                        if (listContent[0].ObrDoctorReadFlag == "1" && listContent[0].ObrNurseReadFlag == "1")
                        {
                            isDelByIDAndUpdate = DeleteMessageByIDAndUpdateCriticalCheckerBase(objAuditInfo, messageID, pat_id);
                        }
                    }
                    #endregion
                }
                else
                {
                    isDelByIDAndUpdate = DeleteMessageByIDAndUpdateCriticalCheckerBase(objAuditInfo, messageID, pat_id);
                }
            }
            //记录操作流程
            if (!string.IsNullOrEmpty(pidReportMain.RepBarCode))
            {
                EntitySampProcessDetail detail = new EntitySampProcessDetail();
                detail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                detail.ProcUsercode = objAuditInfo.UserId;
                detail.ProcUsername = objAuditInfo.UserName;
                detail.ProcBarcode = pidReportMain.RepBarCode;
                detail.ProcStatus = "1000";
                detail.ProcContent = confirmType;
                detail.ProcBarno = pidReportMain.RepBarCode;
                detail.ProcPlace = objAuditInfo.Place;
                detail.ProcTimes = 1;
                detail.RepId = pat_id;
                isDelByIDAndUpdate = new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);
            }
            return isDelByIDAndUpdate;
        }

        public bool DeleteMessageByIDPatIdBase(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id)
        {
            bool isDelMsgByID = false;
            bool isSecond = false;
            try
            {
                string sqlDeleteContent = string.Empty;
                string sqlDeleteReceiver = string.Empty;

                //作为危急值处理更新操作的实体参数
                EntityDicObrMessageContent obrMsgContent = new EntityDicObrMessageContent();
                obrMsgContent.ObrAuditUserId = objAuditInfo.UserId;
                obrMsgContent.ObrAuditUserName = objAuditInfo.UserName;
                obrMsgContent.ObrConfirmType = objAuditInfo.MsgAffirmType;
                obrMsgContent.ObrValueA = pat_id;

                bool isThree = false;
                if (bPhiDelete)//物理删除
                {
                    EntityDicObrMessageContent eyMsgContent = new EntityDicObrMessageContent();
                    eyMsgContent.ObrId = messageID;
                    //删除危急值数据
                    isDelMsgByID = new ObrMessageContentBIZ().DeleteObrMessageContent(eyMsgContent);
                    EntityDicObrMessageReceive eyMsgReceive = new EntityDicObrMessageReceive();
                    eyMsgReceive.ObrId = messageID;
                    //删除危急值处理数据
                    isSecond = new ObrMessageReceiveBIZ().DeleteObrMessageReceive(eyMsgReceive);

                }
                else
                {
                    //不取消其他提醒,只取消内部提醒
                    if (objAuditInfo != null && objAuditInfo.IsOnlyInsgin)
                    {
                        isDelMsgByID = new ObrMessageContentBIZ().UpdateObrMessageContentHaveIn(obrMsgContent);
                        EntityDicObrMessageReceive obrMsgReceive = new EntityDicObrMessageReceive();
                        obrMsgReceive.ObrId = messageID;
                        isSecond = new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateByID(obrMsgReceive);
                    }
                    else
                    {
                        isDelMsgByID = new ObrMessageContentBIZ().UpdateObrMessageContentHaveInDelFlag(obrMsgContent);
                        EntityDicObrMessageReceive obrMsgReceive = new EntityDicObrMessageReceive();
                        obrMsgReceive.ObrId = messageID;
                        isSecond = new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateDelFlagByID(obrMsgReceive);
                    }
                    isThree = AddPatExtEditInfo(objAuditInfo, pat_id);//在扩展表[保存]危急值编辑内容
                }
                if (isSecond && isDelMsgByID && isThree)
                    return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }
        /// <summary>
        /// 在扩展表[保存]危急值编辑内容
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public bool AddPatExtEditInfo(EntityAuditInfo objAuditInfo, string pat_id)
        {
            bool isPatExt = false;
            if (string.IsNullOrEmpty(objAuditInfo.MsgContent))
            {
                objAuditInfo.MsgContent = string.Format("{0}", objAuditInfo.Place);

            }
            else if (!string.IsNullOrEmpty(objAuditInfo.Place))
            {
                objAuditInfo.MsgContent = objAuditInfo.MsgContent + "\r\n" + objAuditInfo.Place;
            }

            try
            {
                bool patExtIsExist = new PidReportMainExtBIZ().SearchPatExtExistByID(pat_id);////记录扩展表是否存已存在此ID

                if (patExtIsExist)//若扩展表存在此ID，则Update
                {
                    isPatExt = new PidReportMainExtBIZ().UpdatePidReportMainExt(objAuditInfo, pat_id);
                }
                else
                {
                    isPatExt = new PidReportMainExtBIZ().SavePidReportMainExt(objAuditInfo, pat_id);
                }
                return isPatExt;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public bool DeleteMessageByIDAndUpdateCriticalCheckerBase(EntityAuditInfo objAuditInfo, string messageID, string pat_id)
        {
            bool IsTrue = false;
            try
            {
                string msg = objAuditInfo.ExetMsg;
                if (objAuditInfo.IsSaveMsg)//是否保存危急值编辑内容
                {
                    //删除消息-并保持危急值编辑内容
                    IsTrue = DeleteMessageByIDPatIdBase(objAuditInfo, messageID, false, pat_id);
                }
                else
                {
                    //删除消息
                    IsTrue = DeleteMessageByID(objAuditInfo, messageID, false);
                }

                try
                {
                    //更新病人表危急值查看标志，查看人
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    if (mainDao != null)
                    {
                        IsTrue = mainDao.UpdateReadUserIdAndUrgentFlag(false, objAuditInfo.UserName, pat_id);
                        if (msg == "急查")//暂时用来急查用
                        {
                            IsTrue = mainDao.UpdateReadUserIdAndUrgentFlag(true, objAuditInfo.UserName, pat_id);
                        }
                        mainDao.UpdateDrugfastFlag(pat_id);
                    }

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

                try
                {
                    if (objAuditInfo.SendMsgFlag)
                    {
                        //之后需要修改，现在还是引用的旧的(暂时先屏蔽)
                        //new PatientEnterService().SendCriticalMsg(pat_id);
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

                return IsTrue;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return IsTrue;
        }
        public string GetConfigValue(string confiigcode)
        {
            return dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig(confiigcode);
        }

        /// <summary>
        /// 沙井HIS账号验证
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet SJHisCheckPassWord(DataSet ds)
        {
            DataSet result = new DataSet();
            OracleConnection conn = new OracleConnection("Data Source=HISRUN;user=bslis;password=bslis;");
            try
            {
                string sql = "";

                if (ds.Tables["Dtnhyy"] != null && ds.Tables["Dtnhyy"].Rows.Count > 0)//添加了南海oracle调用验证
                {
                    #region HIS提供的函数

                    /**
                 
create or replace function nhyy_login_chk(s1 varchar2,s2 varchar2) return varchar2 is
  Result varchar2(20);
  m varchar2(256);
  xm varchar2(20);
  d varchar2(50);
  c varchar2(256);
  s varchar2(256);
  i Integer;
  t Integer;
begin
  select p0193_3,p0193_4 into m,xm from hospital.p0193 where p0193_1=s1 and left(p0193_2,1)<>'*' and rownum<=1;
  d:= '67531824098463720159';
  c:= '';
  s:= '';
  for i in 1..len(m) loop
    c:=c||substr(d,to_number(Chr(ascii(substr(m,i,1))+32))+1,1);
  end loop;
  
  for i in 1..trunc(Len(c)/3) loop
    t:=to_number(substr(c,(i-1)*3+1,3));
    if t>255 then
      t:=0; 
    end if;
    s:=s||chr(t);
  end loop;
  if s2=s then
    Result:=xm;
  else
    Result:=null;
  end if;
    
  return(Result);
end nhyy_login_chk;


--插入参数是医生工号和密码，返回的是姓名

                ***/

                    #endregion

                    conn = new OracleConnection("Data Source=EHISSERVER;user=system;password=manager;");

                    sql = string.Format("select nhyy_login_chk('{0}','{1}') FROM dual", ds.Tables["Dtnhyy"].Rows[0]["userid"].ToString(), ds.Tables["Dtnhyy"].Rows[0]["pw"].ToString());
                }
                else
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["pw"].ToString()))
                    {
                        sql = string.Format("select yhdm, yhkl,yhmc from v_hszyh where   yhkl IS null and yhdm='{0}'", ds.Tables[0].Rows[0]["userid"].ToString());
                    }
                    else
                    {
                        sql = string.Format("select yhdm, yhkl,yhmc from v_hszyh where  yhdm='{0}' and yhkl='{1}'", ds.Tables[0].Rows[0]["userid"].ToString(), ds.Tables[0].Rows[0]["pw"].ToString());
                    }
                }

                conn.Open();
                OracleCommand cmd = conn.CreateCommand();

                cmd.CommandText = sql;
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(result, "PowerUserInfo");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public List<EntitySysUser> LisCheckPassWord(EntitySysUser user)
        {
            string par = "All";//查询所有用户
            List<EntitySysUser> listSysUser = new List<EntitySysUser>();
            listSysUser = new SysUserInfoBIZ().GetAllUsers(par);

            //加密
            string p_strPwd = dcl.common.EncryptClass.Encrypt(user.UserPassword);
            string p_strUserId = user.UserLoginid;
            List<EntitySysUser> returnUser = listSysUser.Where(w => w.UserLoginid == p_strUserId && w.UserPassword == p_strPwd).ToList();

            return returnUser;
        }

        public List<EntityDicPubDept> GetDeptInfo()
        {
            List<EntityDicPubDept> listDept = new List<EntityDicPubDept>();
            EntityResponse result = new DepartBIZ().Search(new EntityRequest());
            listDept = result.GetResult() as List<EntityDicPubDept>;
            return listDept;
        }



        /// <summary>
        /// 获取一个病区的科室代码
        /// </summary>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public string GetDepInwhereStr(string dept_code)
        {
            string DepStr = "";

            if (dept_code == null)
                return DepStr;  //空科室代码,返回空    
            try
            {
                List<EntityDicPubDept> listPubDept = new List<EntityDicPubDept>();

                if (!string.IsNullOrEmpty(dept_code) && dept_code.Contains(",") && dept_code.Contains("'"))
                {
                    //  sql = string.Format(@"select dep_code from dict_depart where dep_ward in
                    //     (select dep_ward from dbo.dict_depart where dep_code in({0}) )", dept_code);
                    #region 根据多个科室代码查询科室数据
                    EntityResponse deptRep = new EntityResponse();
                    deptRep = new DepartBIZ().Search(new EntityRequest());
                    List<EntityDicPubDept> listPubDeptLS = deptRep.GetResult() as List<EntityDicPubDept>;
                    //筛选出符合条件的病区集合（dep_ward）
                    //List<EntityDicPubDept> listParentID = listPubDept.Where(w => dept_code.Contains(w.DeptCode)).ToList();//这里筛选不正确，需要处理成单个再逐个筛选
                    List<EntityDicPubDept> listDeptCode = new List<EntityDicPubDept>();
                    string depCodeList = dept_code.Replace("'", "");
                    string[] strArray = depCodeList.Split(',');
                    foreach (var str in strArray)
                    {
                        foreach (var infoPubDept in listPubDeptLS)
                        {
                            if (infoPubDept.DeptCode.Equals(str))
                                listDeptCode.Add(infoPubDept);
                        }
                    }

                    foreach (var infoDeptCode in listDeptCode)
                    {
                        foreach (var info in listPubDeptLS)
                        {
                            if (info.DeptParentId.Equals(infoDeptCode.DeptParentId))
                                listPubDept.Add(infoDeptCode);
                        }
                    }
                    #endregion
                    listPubDept = listPubDept.Distinct().ToList();
                }
                else
                {
                    // sql = string.Format(@"select dep_code from dict_depart where dep_ward =
                    //   (select top 1 dep_ward from dbo.dict_depart where dep_code='{0}' )", dept_code);
                    #region 根据科室代码查询科室数据
                    EntityResponse deptRep = new EntityResponse();
                    deptRep = new DepartBIZ().Search(new EntityRequest());
                    listPubDept = deptRep.GetResult() as List<EntityDicPubDept>;

                    List<EntityDicPubDept> listIn = listPubDept.Where(w => w.DeptCode == dept_code).ToList();
                    string deptParentId = string.Empty;
                    if (listIn.Count > 0)
                    {
                        deptParentId = listIn[0].DeptParentId;
                    }
                    listPubDept = listPubDept.Where(w => w.DeptParentId == deptParentId).ToList();
                    #endregion
                }

                if (listPubDept != null && listPubDept.Count > 0)
                {
                    foreach (var infoDept in listPubDept)
                    {
                        if (string.IsNullOrEmpty(DepStr))
                        {
                            DepStr = "'" + infoDept.DeptCode + "'";
                        }
                        else
                        {
                            DepStr += ",'" + infoDept.DeptCode + "'";
                        }
                    }
                }
                else
                {
                    DepStr = string.Format("'{0}'", dept_code);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取病区信息", ex);
            }
            return DepStr;
        }

        public List<EntityPidReportMain> GetUrgentflagAndPatlookcodeByPatid(string pat_id)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            IDaoUrgentObrMessage dao = DclDaoFactory.DaoHandler<IDaoUrgentObrMessage>();
            if (dao != null)
            {
                listPats = dao.GetUrgentflagAndPatlookcodeByPatid(pat_id);
            }
            return listPats;
        }

        public void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark)
        {
            IDaoDeptMessage dao = DclDaoFactory.DaoHandler<IDaoDeptMessage>();
            if (dao != null)
                dao.HandleReturnMessage(barcode, strOperatorID, strOperatorName, currentServerTime, bc_remark);
        }

        public List<EntityDicQcRuleMes> GetItrQcMessage(string itr_type)
        {
            List<EntityDicQcRuleMes> listQcRuleMes = new List<EntityDicQcRuleMes>();
            IDaoQcRuleMes dao = DclDaoFactory.DaoHandler<IDaoQcRuleMes>();
            if (dao != null)
            {
                string days = CacheSysConfig.Current.GetSystemConfig("QC_ItrQcMessageQueryDays");
                int day;
                if (string.IsNullOrEmpty(days) || !int.TryParse(days, out day))
                {
                    days = "90";
                }
                //listQcRuleMes = dao.Search(itr_type);
                listQcRuleMes = dao.Search(days);
            }
            return listQcRuleMes;
        }
    }
}
