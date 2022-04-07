using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.root.dac;
using dcl.common;
using System.Data.SqlClient;
using dcl.svr.framedic;
using dcl.entity;

namespace dcl.svr.msg
{
    /// <summary>
    /// 仪器危急值数据
    /// </summary>
    public class InstrmtUrgentMsgCache
    {
        #region singleton
        private static object objLock = new object();

        private static InstrmtUrgentMsgCache _instance = null;

        /// <summary>
        /// 当时是否没在处理
        /// </summary>
        private static bool IsCurrNotDisposal { get; set; }

        public static InstrmtUrgentMsgCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InstrmtUrgentMsgCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 危机值信息缓存
        /// </summary>
        private DataTable cache = null;


        /// <summary>
        /// 仪器危急值数据
        /// </summary>
        private InstrmtUrgentMsgCache()
        {
            this.cache = new DataTable();
            this.cache.TableName = "ItrUrgentMsgCache";

            IsCurrNotDisposal = true;
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = this.GetInstrmtUrgentMsgToCache();
        }

        /// <summary>
        /// 获取仪器危急值数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstrmtUrgentMsgToCache()
        {
            try
            {
                //总开关--是否启动仪器危急值提醒
                bool UrgentMessage_ShowInstrmtMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Instrmt_IsNotify") == "是";

                if (!UrgentMessage_ShowInstrmtMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            DataTable dtbResult = null;

            DeleteItrUrgentMsgData();//删除24小时后的数据

            string strSQL = @"select patients.pat_id,
patients.pat_itr_id,
patients.pat_sid,
patients.pat_host_order,
patients.pat_name,
patients.pat_date,
patients.pat_flag,
dict_instrmt.itr_mid,
dict_instrmt.itr_type, --物理组 
dict_type.type_name
from msg_patients
inner join patients on patients.pat_id=msg_patients.msg_id and patients.pat_flag=0
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on dict_instrmt.itr_type=dict_type.type_id
where msg_patients.msg_create_date>=Dateadd(day,-1,getdate()) and msg_patients.msg_create_date<=getdate()";

            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "ItrUrgentMsgCache";

                //if (dtbResult != null && dtbResult.Rows.Count > 0)
                //{
                //    List<string> patIDList = new List<string>();
                //    for (int i = dtbResult.Rows.Count - 1; i >= 0; i--)
                //    {
                //        string pat_id = dtbResult.Rows[i]["pat_id"].ToString();
                //        if (patIDList.Contains(pat_id))//过滤重复的信息
                //        {
                //            dtbResult.Rows.Remove(dtbResult.Rows[i]);
                //        }
                //        else
                //        {
                //            patIDList.Add(pat_id);
                //            //   dtbResult.Rows[i]["pat_result"] = GetResult(pat_id);
                //        }
                //    }
                //}
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取仪器危急值数据", objEx);
            }

            return dtbResult;
        }

        /// <summary>
        /// 监控仪器危急值数据并生成
        /// </summary>
        public void InsertItrUrgentMsgData()
        {
            try
            {
                //总开关--是否启动仪器危急值提醒
                bool UrgentMessage_ShowInstrmtMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Instrmt_IsNotify") == "是";

                if (!UrgentMessage_ShowInstrmtMsg)
                {
                    //当不启动时
                    return;
                }
            }
            catch
            {
                return;
            }

            if (IsCurrNotDisposal)
            {
                IsCurrNotDisposal = false;
            }
            else
            {
                return;//如果在处理中,则跳出不继续执行
            }

            //获取仪器结果信息(18小时内)
            string sqlSel = @"SELECT     TOP (500) res_id
FROM         resulto_mid
WHERE     (res_date >= DATEADD(hour, - 18, GETDATE())) AND (res_date <= GETDATE()) AND (res_critical_flag IS NULL) AND (res_id IS NOT NULL)";


            //标记已查看过
            string sqlUpdate = @"UPDATE    resulto_mid
SET              res_critical_flag = '1'
WHERE     (res_id = '{0}') and res_critical_flag is null ";


            //添加危急值数据
            string sqlInsert = @"delete from msg_patients where msg_id='{0}'
INSERT INTO msg_patients
           ([msg_id]
           ,[msg_create_date])
     VALUES
           ('{0}',getdate())";


            try
            {
                DataTable dtbResult = null;
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(sqlSel);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "ItrUrgentMsgData";

                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    List<string> patIDList = new List<string>();
                    for (int i = dtbResult.Rows.Count - 1; i >= 0; i--)
                    {
                        string res_id = dtbResult.Rows[i]["res_id"].ToString();
                        if ((!patIDList.Contains(res_id)) && (!string.IsNullOrEmpty(res_id)))//过滤重复的信息
                        {
                            patIDList.Add(res_id);//记录此ID

                            try
                            {
                                #region 查看结果

                                //根据ID获取结果信息
                                //DataTable dtRes = lis.biz.lab.PatReadBLL.NewInstance.GetPatientCommonResult(res_id, false, out dtPatHistorRes);
                                DataTable dtRes = GetPatientCommonResultByID(res_id);

                                #endregion

                                bool IsAddUrgentData = false;//是否添加危急值信息

                                #region 分析危急值

                                if (dtRes != null && dtRes.Rows.Count > 0)
                                {
                                    foreach (DataRow drRes in dtRes.Rows)
                                    {
                                        if (CalcPatResultRow(drRes))//是否有危急值
                                        {
                                            IsAddUrgentData = true;
                                            break;
                                        }
                                    }
                                }

                                #endregion

                                #region 添加仪器危急值信息与更新状态

                                if (true)
                                {
                                    SqlCommand cmdInsertMsgData = new SqlCommand(string.Format(sqlInsert, res_id));

                                    SqlCommand cmdUpdateResMid = new SqlCommand(string.Format(sqlUpdate, res_id));

                                    using (DBHelper helper = DBHelper.BeginTransaction())
                                    {
                                        if (IsAddUrgentData)//是否添加仪器危急值信息
                                        {
                                            int m = helper.ExecuteNonQuery(cmdInsertMsgData);
                                        }

                                        int n = helper.ExecuteNonQuery(cmdUpdateResMid);

                                        helper.Commit();
                                    }
                                }

                                #endregion
                            }
                            catch (Exception itemEx)
                            {
                                Lib.LogManager.Logger.LogException("分析并添加仪器危急值数据", itemEx);
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取仪器数据", objEx);
            }

            IsCurrNotDisposal = true;
        }

        /// <summary>
        /// 根据ID删除指定的仪器危急值数据
        /// </summary>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        public bool DeleteItrUrgentMsgDataByID(string msg_id)
        {
            //如果ID为空返回失败
            if (string.IsNullOrEmpty(msg_id)) return false;

            try
            {
                string sqlDelete = @"delete from msg_patients where msg_id='{0}'";

                SqlCommand cmdDeleteMsgData = new SqlCommand(string.Format(sqlDelete, msg_id));

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int n = helper.ExecuteNonQuery(cmdDeleteMsgData);

                    helper.Commit();
                }

                //this.Refresh();//刷新缓存
                DeleteCacheDataByID(msg_id);
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("删除仪器危急值信息", objEx);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除缓存中指定的数据
        /// </summary>
        /// <param name="msg_id"></param>
        private void DeleteCacheDataByID(string msg_id)
        {
            if (string.IsNullOrEmpty(msg_id)) return;

            try
            {
                if (this.cache != null && this.cache.Rows.Count > 0)
                {
                    DataRow[] drArray = this.cache.Select(string.Format("pat_id='{0}'", msg_id));
                    if (drArray.Length > 0)
                    {
                        foreach (DataRow dr in drArray)
                        {
                            this.cache.Rows.Remove(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 删除24小时后的数据
        /// </summary>
        private void DeleteItrUrgentMsgData()
        {
            try
            {
                string sqlDelete = @"delete from msg_patients where msg_create_date<Dateadd(day,-1,getdate())";

                SqlCommand cmdDeleteMsgData = new SqlCommand(sqlDelete);

                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    int n = helper.ExecuteNonQuery(cmdDeleteMsgData);

                    helper.Commit();
                }
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("删除24小时后的仪器危急值信息", objEx);
            }
        }

        /// <summary>
        /// 根据筛选条件获取仪器危急值数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetItrUrgentMessage(string strWhere)
        {
            if (strWhere == null || string.IsNullOrEmpty(strWhere))
            {
                return this.cache;
            }
            else
            {
                try
                {
                    if (this.cache != null && this.cache.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cache.Clone();
                        DataRow[] drArray = this.cache.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "ItrUrgentMsgCache";
                        return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存仪器危急值数据", ex);
                }
                return this.cache;
            }
        }

        /// <summary>
        /// 对病人结果表中的某一行的参考值和是否超出;结果数据类型是否正确
        /// </summary>
        /// <param name="drResult"></param>
        private bool CalcPatResultRow(DataRow drResult)
        {
            bool hasCriticalValues = false;//是否有危急值

            if (drResult != null && !Compare.IsNullOrDBNull(drResult["res_chr"]))
            {
                #region 判断有没有大小于号结果 edit by zheng
                //结果
                string strValue = drResult["res_chr"].ToString();
                //结果符号，为>号或<号
                string strSymbol = "";
                if (strValue.Contains(">"))
                {
                    strSymbol = ">";
                    strValue = strValue.TrimStart('>');
                }
                else if (strValue.Contains("<"))
                {
                    strSymbol = "<";
                    strValue = strValue.TrimStart('<');
                }

                #endregion

               

                decimal decValue;
               
                
                //去掉指定的符号来计算参考值
                strValue = ResultRemoveSymbol(strValue);

                if (
                    !strValue.Contains("+")
                    &&
                     decimal.TryParse(strValue, out decValue))//是否数值型结果
                {
                    string strItmRef_l = string.Empty;//参考值下限
                    string strItmRef_h = string.Empty;//参考值上限
                    string strItmPan_l = string.Empty;//危急值下限
                    string strItmPan_h = string.Empty;//危急值上限
                    string strItm_min = string.Empty;//阈值下限
                    string strItm_max = string.Empty;//阈值上限

                    string strItmRef_lSymbol = string.Empty;//参考值下限
                    string strItmRef_hSymbol = string.Empty;//参考值上限
                    string strItmPan_lSymbol = string.Empty;//危急值下限
                    string strItmPan_hSymbol = string.Empty;//危急值上限
                    string strItm_minSymbol = string.Empty;//阈值下限
                    string strItm_maxSymbol = string.Empty;//阈值上限

                    //是否存在参考值
                    bool hasRef = false;

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_l_cal"]))
                    {
                        strItmRef_l = drResult["res_ref_l_cal"].ToString();
                        if (drResult["res_ref_l"].ToString().Contains(">"))
                        {
                            strItmRef_lSymbol = ">";
                        }
                        strItmRef_l = ResultRemoveSymbol(strItmRef_l);
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_h_cal"]))
                    {
                        strItmRef_h = drResult["res_ref_h_cal"].ToString();
                        if (drResult["res_ref_h"].ToString().Contains("<"))
                        {
                            strItmRef_hSymbol = "<";
                        }
                        strItmRef_h = ResultRemoveSymbol(strItmRef_h);
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_pan_l_cal"]))
                    {
                        strItmPan_l = drResult["res_pan_l_cal"].ToString();
                        if (drResult["res_pan_l"].ToString().Contains(">"))
                        {
                            strItmPan_lSymbol = ">";
                        }
                        strItmPan_l = ResultRemoveSymbol(strItmPan_l);
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_pan_h_cal"]))
                    {
                        strItmPan_h = drResult["res_pan_h_cal"].ToString();
                        if (drResult["res_pan_h"].ToString().Contains("<"))
                        {
                            strItmPan_hSymbol = "<";
                        }
                        strItmPan_h = ResultRemoveSymbol(strItmPan_h);
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_min_cal"]))
                    {
                        strItm_min = drResult["res_min_cal"].ToString();
                        if (drResult["res_min"].ToString().Contains(">"))
                        {
                            strItm_minSymbol = ">";
                        }
                        strItm_min = ResultRemoveSymbol(strItm_min);
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_max_cal"]))
                    {
                        strItm_max = drResult["res_max_cal"].ToString();
                        if (drResult["res_max"].ToString().Contains("<"))
                        {
                            strItm_maxSymbol = "<";
                        }
                        strItm_max = ResultRemoveSymbol(strItm_max);
                    }

                    decimal decItmRef_l;
                    decimal decItmRef_h;
                    decimal decItmMax;
                    decimal decItmMin;
                    decimal decItmPan_l;
                    decimal decItmpan_h;
                    bool refLowAndHighIsString = false; //参考值下限是否是字符串 edit by sink 2010-7-1

                    EnumResRefFlag enumResRefFlag = EnumResRefFlag.Normal;

                    string message = string.Empty;


                    if (decimal.TryParse(strItmRef_h, out decItmRef_h) == false && decimal.TryParse(strItmRef_l, out decItmRef_l) == false)
                    //参考值为字符 edit by sink 2010-7-22
                    {
                        refLowAndHighIsString = true;
                    }

                    //低于极小阈值
                    #region 低于极小阈值


                    if (decimal.TryParse(strItm_min, out decItmMin))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmMin) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                                message += string.Format("低于极小阈值[{0}];", decItmMin);
                            }
                        }
                        else
                        {
                            if (strItm_minSymbol.Contains(">"))
                            {
                                if (decValue <= decItmMin)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                    message += string.Format("低于极小阈值[{0}];", decItmMin);
                                }
                            }
                            else
                            {
                                if (decValue < decItmMin)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                                    message += string.Format("低于极小阈值[{0}];", decItmMin);
                                }
                            }

                        }
                    }
                    #endregion

                    //高于极大阈值
                    #region 高于极大阈值


                    if (decimal.TryParse(strItm_max, out decItmMax))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmMax) //edit by zheng && sink 2010-8-5
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                                message += string.Format("高于极大阈值[{0}];", decItmMax);
                            }
                        }
                        else
                        {
                            if (strItm_maxSymbol.Contains("<"))
                            {
                                if (decValue >= decItmMax)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                                    message += string.Format("高于极大阈值[{0}];", decItmMax);
                                }
                            }
                            else
                            {
                                if (decValue > decItmMax)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                                    message += string.Format("高于极大阈值[{0}];", decItmMax);
                                }
                            }

                        }

                    }
                    #endregion

                    //低于危机值下限
                    if (decimal.TryParse(strItmPan_l, out decItmPan_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmPan_l) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                            }
                        }
                        else
                        {

                            if (strItmPan_lSymbol.Contains(">"))
                            {
                                if (decValue <= decItmPan_l)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                    message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                                }
                            }
                            else
                            {

                                if (decValue < decItmPan_l)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                    message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                                }
                            }

                        }
                        
                    }

                    //高于危机值上限
                    if (decimal.TryParse(strItmPan_h, out decItmpan_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmpan_h) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                            }
                        }
                        else
                        {
                            if (strItmPan_hSymbol.Contains("<"))
                            {
                                if (decValue >= decItmpan_h)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                    message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                                }
                            }
                            else
                            {
                                if (decValue > decItmpan_h)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                    message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                                }
                            }

                        }

                    }

                    

                    if (!hasRef)//如果没有参考值
                    {
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                    }
                    else
                    {
                        if (refLowAndHighIsString)//参考值上下限是否是字符串 edit by sink 2010-7-22
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                        else
                        {
                            drResult["res_ref_flag"] = ((int)enumResRefFlag).ToString();
                            
                        }
                        
                    }
                    
                }
                else
                {
                    string res_positive_result = drResult["res_positive_result"].ToString().Trim();
                    string res_custom_critical_result = drResult["res_custom_critical_result"].ToString().Trim();
                    string res_allow_values = drResult["res_allow_values"].ToString().Trim();

                    bool isCustomCritical = false;
                    if (res_custom_critical_result.Trim() != string.Empty && strValue != null && strValue.Trim() != string.Empty)
                    {
                        foreach (string pos_res in res_custom_critical_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strValue == pos_res)
                            {
                                drResult["res_ref_flag"] = ((int)EnumResRefFlag.CustomCriticalValue).ToString();
                                isCustomCritical = true;
                                break;
                            }
                        }
                    }

                    bool hasNotAllowValue = true;
                    if (res_allow_values.Trim() != string.Empty && strValue != null && strValue.Trim() != string.Empty)
                    {
                        foreach (string res_allow in res_allow_values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strValue == res_allow)
                            {
                                hasNotAllowValue = false;
                                break;

                            }
                        }

                        if (hasNotAllowValue)
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                    }
                    else
                    {
                        hasNotAllowValue = false;
                    }

                    if (res_positive_result != string.Empty && !isCustomCritical && !hasNotAllowValue)
                    {
                        if (strValue != null)
                        {
                            if (strValue.Trim().Contains("弱阳性") || strValue.Trim() == "±")
                            {
                                drResult["res_ref_flag"] = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                            }
                            else
                            {
                                bool is_pos = false;
                                foreach (string pos_res in res_positive_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if (strValue == pos_res)
                                    {
                                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.Positive).ToString();
                                        is_pos = true;
                                        break;
                                    }
                                }

                                if (!is_pos)
                                {
                                    drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                                }
                            }
                        }
                        else
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                    }
                    else if (!isCustomCritical && !hasNotAllowValue)
                    {
                        if (strValue != null
                            &&
                                (
                                    strValue.Trim() == "+"
                                    || strValue.StartsWith("+")
                                    || strValue.EndsWith("+")
                                    || strValue.IndexOf("阳性") >= 0
                                    || strValue.ToLower() == "pos"
                                )
                            && !strValue.Trim().Contains("弱阳性")
                            && !(strValue.Trim() == "±")
                            && !(strValue.Length > 1 && strValue.Replace("+", "").Trim() == string.Empty)
                            )
                        {
                            
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Positive).ToString();
                        }
                        else if (strValue.Trim().Contains("弱阳性") || strValue.Trim() == "±")
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                            
                        }
                        else
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                    }

                   
                }

                 int int_res_ref_flag;
                 if (int.TryParse(drResult["res_ref_flag"].ToString(), out int_res_ref_flag))
                 {
                     EnumResRefFlag res_ref_flag = (EnumResRefFlag)int_res_ref_flag;

                     if (res_ref_flag != EnumResRefFlag.Unknow)
                     {
                         //根据res_ref_flag标志判断
                         if (
                             (res_ref_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2//高于危急值上限
                             ||
                             (res_ref_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2//低于危急值下限
                             ||
                             (res_ref_flag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3//高于阈值上限
                             ||
                             (res_ref_flag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3//低于阈值下限
                             ||
                             (res_ref_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue//低于危急值下限
                            )
                         {
                             hasCriticalValues = true;//标记出现危急值
                         }
                     }
                 }
            }

            return hasCriticalValues;
        }


        /// <summary>
        /// 去掉指定的符号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private string ResultRemoveSymbol(string strValue)
        {

            strValue = strValue.TrimStart(new char[] { '=', '>', '<' });
            if (strValue.Contains(":"))
            {
                string[] splited = strValue.Split(':');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse(splited[0], out decLeft)
                        && decimal.TryParse(splited[1], out decRight))
                    {

                        return splited[1];
                    }
                }
            }
            else if (strValue.Contains("："))
            {
                string[] splited = strValue.Split('：');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse(splited[0], out decLeft)
                        && decimal.TryParse(splited[1], out decRight))
                    {

                        return splited[1];
                    }
                }
            }
            return strValue;
        }

        /// <summary>
        /// 获取病人普通结果
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        private DataTable GetPatientCommonResultByID(string patID)
        {
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                DBHelper helper = new DBHelper();

                //string selectPatState = string.Format("select top 1 pat_id,pat_sid,pat_flag,pat_sam_id,pat_itr_id,pat_sam_rem from patients where pat_id = '{0}'", patID);

                //DataTable dtPat = helper.GetTable(selectPatState);

                //EntityPatient2 entityPatient = new EntityPatient2();

                //if (dtPat.Rows.Count > 0)
                //{
                //    entityPatient.pat_id = dtPat.Rows[0]["pat_id"].ToString();
                //    entityPatient.pat_sid = dtPat.Rows[0]["pat_sid"].ToString();
                //    entityPatient.pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                //    entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();

                //    if (!Compare.IsEmpty(dtPat.Rows[0]["pat_flag"]))
                //    {
                //        entityPatient.pat_flag = Convert.ToInt32(dtPat.Rows[0]["pat_flag"]);
                //    }
                //    else
                //    {
                //        entityPatient.pat_flag = 0;
                //    }


                //    if (entityPatient.pat_flag == 0)
                //    {
                //        string sampRem = string.Empty;
                //        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_sam_rem"]))
                //        {
                //            sampRem = dtPat.Rows[0]["pat_sam_rem"].ToString();
                //        }
                //        GenerateAutoCalItem(entityPatient, entityPatient.pat_sam_id, sampRem);
                //        //FillNotNullResult(entityPatient);
                //        UpdateResultNotCombineItem(patID);
                //    }
                //}

                //if (Compare.IsNullOrDBNull(objState) || objState.ToString() == "0")
                //{
                //    GenerateAutoCalItem(patID);
                //    FillNotNullResult(patID);
                //    UpdateResultNotCombineItem(patID);
                //}

                string sql = string.Format(@"
select
resulto.res_key,--结果主键标识
resulto.res_id,--结果ID(病人ID)
resulto.res_itr_id,--仪器ID
resulto.res_sid,--样本号
resulto.res_itm_id,--项目ID
dict_item.itm_ecd as res_itm_ecd,--项目代码
dict_item.itm_name as res_itm_name,--项目名称
resulto.res_itm_rep_ecd,--项目报告编号
resulto.res_chr,--结果
resulto.res_od_chr,--OD结果
resulto.res_cast_chr,--数值结果
resulto.res_price,--价格
resulto.res_ref_exp,--阳性标志 （提示）
resulto.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
resulto.res_meams,--实验方法
resulto.res_date,--结果日期
resulto.res_flag,--有效标志
dict_instrmt_warningsigns.itr_warn_trandate,--项目状态
resulto.res_itr_ori_id,--原始仪器id
resulto.res_ref_type,--参考值类型
res_type = case when dict_item.itm_cal_flag = 1 then 2
                else resulto.res_type end, --结果类型
resulto.res_rep_type,--报告类型
resulto.res_com_id,--组合ID
res_com_name = case when (resulto.res_com_id is null or resulto.res_com_id ='') then '无组合'
                    else  dict_combine.com_name
                    end,--组合名称
--res_com_seq = isnull(dict_combine.com_seq,99999),
res_com_seq = isnull(dict_combine.com_seq,isnull(patients_mi.pat_seq,99999)),
--res_com_seq = case when patients_mi.pat_seq is null then 9999
--                   else patients_mi.pat_seq
--                   end,
resulto.res_unit,--单位
itm_dtype = '',--项目数据类型
itm_max_digit = 0,--小数位数
res_max = '',--极大阈值
res_min = '',--极小阈值
res_pan_h = '',--危急值上限
res_pan_l = '',--危急值下限
res_ref_h = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_h else '' end),--参考值上限
res_ref_l = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_l else '' end),--参考值下限
res_max_cal = '',--极大阈值
res_min_cal = '',--极小阈值
res_pan_h_cal = '',--危急值上限
res_pan_l_cal = '',--危急值下限
res_ref_h_cal = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_h else '' end),--参考值上限
res_ref_l_cal = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_l else '' end),--参考值下限
res_ref_range = '',--参考值范围
res_allow_values = '',--允许出现的结果
res_positive_result = '',--阳性提示结果
res_custom_critical_result = '',--自定义危急值
history_result1='',--历史结果,
history_date1 = '',--历史结果时间
history_result2='',
history_date2 = '',
history_result3='',
history_date3 = '',
patients.pat_sam_id,
patients.pat_sex,
patients.pat_dep_id,
patients.pat_age,
patients.pat_sam_rem,
patients.pat_urgent_flag,
patients.pat_flag,--审核状态
resulto.res_exp,
isnew=0,
resulto.res_recheck_flag,
res_ref_flag_h1=0,
res_ref_flag_h2=0,
res_ref_flag_h3=0,
res_origin_record = 1,
dict_combine_mi.com_popedom as isnotempty,--是否为必录项目
calculate_source_itm_id = '',
calculate_dest_itm_id = '',
res_itm_seq = dict_combine_mi.com_sort,
isnull(dict_item.itm_seq,0) as itm_seq
,resulto.res_chr2,--结果2
resulto.res_chr3 --结果3
from resulto
left join patients on patients.pat_id = resulto.res_id--join结果表 edit by sink 2010-6-9 inner join 改为left join
left join dict_item on dict_item.itm_id = resulto.res_itm_id--join项目表
left join dict_combine on dict_combine.com_id = resulto.res_com_id--join项目组合表
left join patients_mi on (patients_mi.pat_com_id = resulto.res_com_id and patients_mi.pat_id = resulto.res_id)--join病人检验组合
left join dict_combine_mi on  patients_mi.pat_com_id = dict_combine_mi.com_id and resulto.res_itm_id = dict_combine_mi.com_itm_id
left join dict_instrmt_warningsigns on dict_instrmt_warningsigns.itr_warn_origdate=resulto.res_exp and patients.pat_itr_id=dict_instrmt_warningsigns.itr_id
where resulto.res_id = '{0}' and res_flag = 1
order by dict_combine.com_seq asc, dict_combine_mi.com_sort asc,dict_item.itm_seq ,resulto.res_itm_ecd asc
", patID); //加上项目排序码排序 edit by sink 2010-6-8


                DataTable dtResult = helper.GetTable(sql);
                dtResult.TableName = lis.dto.PatientTable.PatientResultTableName;

                if (dtResult.Rows.Count > 0)
                {
                    List<string> itemsID = new List<string>();
                    foreach (DataRow drResult in dtResult.Rows)
                    {
                        if (!Compare.IsEmpty(drResult["res_itm_id"]))
                        {
                            itemsID.Add(drResult["res_itm_id"].ToString());
                        }
                    }

                    //int? pat_age = null;

                    //if (!Compare.IsEmpty(dtResult.Rows[0]["pat_age"]))
                    //{
                    //    pat_age = Convert.ToInt32(dtResult.Rows[0]["pat_age"]);
                    //}
                    //else
                    //{
                    //    pat_age = PatCommonBll.GetConfigOnNullAge();
                    //}

                    ////if (pat_age == null)
                    ////{
                    ////    pat_age = AgeConverter.YearToMinute(20);
                    ////}

                    //string pat_sex = PatCommonBll.GetConfigOnNullSex(dtResult.Rows[0]["pat_sex"].ToString());

                    string pat_depcode = string.Empty;
                    if (!Compare.IsNullOrDBNull(dtResult.Rows[0]["pat_dep_id"]))
                    {
                        pat_depcode = dtResult.Rows[0]["pat_dep_id"].ToString();
                    }
                    string pat_sam_id = dtResult.Rows[0]["pat_sam_id"].ToString();

                    string sam_rem = dtResult.Rows[0]["pat_sam_rem"].ToString();

                    string res_itr_id = dtResult.Rows[0]["res_itr_id"].ToString();

                    int pat_age = MsgCommon.GetConfigAge(dtResult.Rows[0]["pat_age"]);
                    string pat_sex = MsgCommon.GetConfigSex(dtResult.Rows[0]["pat_sex"]);

                    DataTable dtItmRef = DictItemBLL.NewInstance.GetItemsWithSamRef(itemsID, pat_sam_id, pat_age, pat_sex, sam_rem, res_itr_id, pat_depcode);

                    if (dtItmRef.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)//循环结果表
                        {
                            if (!Compare.IsEmpty(drResult["res_itm_id"]))
                            {
                                DataRow[] drsItmRef = dtItmRef.Select(string.Format("itm_id = '{0}'", drResult["res_itm_id"].ToString()));

                                if (drsItmRef.Length > 0)
                                {
                                    //res_max = '',--极大阈值
                                    //res_min = '',--极小阈值
                                    //res_pan_h = '',--危急值上限
                                    //res_pan_l = '',--危急值下限
                                    //res_ref_h = '',--参考值上限
                                    //res_ref_l = '',--参考值下限

                                    //res_max_cal = '',--极大阈值
                                    //res_min_cal = '',--极小阈值
                                    //res_pan_h_cal = '',--危急值上限
                                    //res_pan_l_cal = '',--危急值下限
                                    //res_ref_h_cal = '',--参考值上限
                                    //res_ref_l_cal = '',--参考值下限

                                    if (drResult.Table.Columns.Contains("pat_flag") && (drResult["pat_flag"].ToString() == "2" || drResult["pat_flag"].ToString() == "4"))
                                    {
                                        //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                        //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                    }
                                    else
                                    {
                                        drResult["res_ref_l"] = drsItmRef[0]["itm_ref_l"];
                                        drResult["res_ref_h"] = drsItmRef[0]["itm_ref_h"];
                                    }

                                    drResult["res_pan_l"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max"] = drsItmRef[0]["itm_max"];

                                    //允许出现的结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_allow_result"))
                                    {
                                        drResult["res_allow_values"] = drsItmRef[0]["itm_allow_result"];
                                    }

                                    //阳性提示结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_positive_result"))
                                    {
                                        drResult["res_positive_result"] = drsItmRef[0]["itm_positive_result"];
                                    }

                                    //自定义危急值
                                    if (drsItmRef[0].Table.Columns.Contains("itm_urgent_result"))
                                    {
                                        drResult["res_custom_critical_result"] = drsItmRef[0]["itm_urgent_result"];
                                    }

                                    if (drResult.Table.Columns.Contains("pat_flag") && (drResult["pat_flag"].ToString() == "2" || drResult["pat_flag"].ToString() == "4"))
                                    {
                                        //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                        //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                    }
                                    else
                                    {
                                        drResult["res_ref_l_cal"] = drsItmRef[0]["itm_ref_l"];
                                        drResult["res_ref_h_cal"] = drsItmRef[0]["itm_ref_h"];
                                    }

                                    if (
                                       !string.IsNullOrEmpty(drResult["res_ref_l_cal"].ToString().Trim())
                                       && !string.IsNullOrEmpty(drResult["res_ref_h_cal"].ToString().Trim()))
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + " - " + drResult["res_ref_h_cal"].ToString();
                                    }
                                    else
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + drResult["res_ref_h_cal"].ToString();
                                    }

                                    drResult["res_pan_l_cal"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h_cal"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min_cal"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max_cal"] = drsItmRef[0]["itm_max"];


                                    drResult["res_meams"] = drsItmRef[0]["itm_meams"];
                                    drResult["itm_dtype"] = drsItmRef[0]["itm_dtype"];
                                    drResult["itm_max_digit"] = drsItmRef[0]["itm_max_digit"];
                                    drResult["res_unit"] = drsItmRef[0]["itm_unit"];
                                    //resulto.res_unit,--单位
                                    //itm_dtype = '',--项目数据类型
                                    //itm_max_digit = 0,--小数位数
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drResult in dtResult.Rows)
                {
                    ItemRefFormatter.Format(drResult, "res_ref_l_cal", "res_ref_h_cal", "res_min_cal", "res_max_cal", "res_pan_l_cal", "res_pan_h_cal");
                }

                sw.Stop();
               //Lib.LogManager.Logger.Debug(string.Format("数据库:GetPatientResult,获取病人结果表,耗时 {0}ms", sw.ElapsedMilliseconds));

                return dtResult;
            }
            catch (Exception ex)
            {
               dcl.root.logon.Logger.WriteException(this.GetType().Name, "获取病人结果出错,patID=" + patID, ex.ToString());
                throw;
            }
        }
    }
}
