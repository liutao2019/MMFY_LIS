using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using lis.dto.Entity;
using System.Data;
using dcl.root.dac;
using lis.dto;
using dcl.root.logon;
using System.Data.SqlClient;
using lis.dto.FieldsCorrespandance;
using dcl.svr.framedic;
using dcl.common;
using dcl.svr.result.CRUD;
using lis.dto.BarCodeEntity;

using dcl.pub.entities;
using Lib.DataInterface.Implement;
using dcl.entity;

namespace dcl.svr.result
{

    public class PatInsertBLL
    {
        public void UpdatePatCName(DataTable dtPat, DataTable dtCombine)
        {
            if (dtPat.Columns.Contains("pat_c_name") && dtCombine != null)
            {
                //***************************************************************************//
                //将所选组合排序
                int[] a = new int[dtCombine.Rows.Count];
                for (int i = 0; i < a.Length; i++)
                {
                    if ((dtCombine.Rows[i]["com_seq"] == null) || (dtCombine.Rows[i]["com_seq"].ToString() == ""))
                        a[i] = 99999;
                    else
                        a[i] = Convert.ToInt32(dtCombine.Rows[i]["com_seq"].ToString());
                }
                a = SortCombine(a);

                string pat_c_name = string.Empty;

                bool needPlus = false;
                for (int i = 0; i < dtCombine.Rows.Count; i++)
                {
                    if (needPlus)
                    {
                        pat_c_name += "+";
                    }
                    //根据项目组合中的顺序加入
                    pat_c_name += dtCombine.Rows[a[i]]["pat_com_name"].ToString();

                    needPlus = true;
                }
                //}

                dtPat.Rows[0]["pat_c_name"] = pat_c_name;

                //***************************************************************************//
                #region 旧代码
                //string pat_c_name = string.Empty;

                ////if (dtCombine != null)
                ////{
                //bool needPlus = false;
                //foreach (DataRow drCombine in dtCombine.Rows)
                //{
                //    if (needPlus)
                //    {
                //        pat_c_name += "+";
                //    }

                //    pat_c_name += drCombine["pat_com_name"].ToString();

                //    needPlus = true;
                //}
                ////}

                //dtPat.Rows[0]["pat_c_name"] = pat_c_name;
                #endregion
            }
        }
        /// <summary>
        /// 保存条码病人信息
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult InsertBarCodePatient(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            //if (caller != null && caller.BabyFilterFlag)
            //{
            //    return InsertBarCodePatientForBf(caller, dsData, null);
            //}
            return InsertBarCodePatient(caller, dsData, null);
        }

        /// <summary>
        /// 新增或更新病人扩展表patients_ext信息
        /// </summary>
        /// <param name="colName">指定的列名</param>
        /// <param name="colValue">对应的列值</param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult AddOrUpdatePatientExt(string[] colName, string[] colValue, string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();

            try
            {
                if (colName == null || colValue == null || (string.IsNullOrEmpty(pat_id)))
                {
                    throw new Exception("参数值不能为NULL");
                }
                if (colName.Length == 0)
                {
                    throw new Exception("colName的数量不能为0");
                }
                if (colValue.Length == 0)
                {
                    throw new Exception("colValue的数量不能为0");
                }
                if (colName.Length != colValue.Length)
                {
                    throw new Exception("colName与colValue的数量不一致");
                }

                DataTable dtPatientExt = new PatReadBLL().GetPatientExtData(pat_id);//获取当前pat_id的信息
                if (dtPatientExt != null && dtPatientExt.Columns.Count > 0)
                {
                    string updateSetStr = "";//更新SQL语句
                    string insertColNameStr = "";//新增SQL语句key
                    string insertColValueStr = "";//新增SQL语句value

                    for (int i = 0; i < colName.Length; i++)
                    {
                        if (!dtPatientExt.Columns.Contains(colName[i]))
                        {
                            throw new Exception(string.Format("表patients_ext找不到字段{0}", colName[i]));
                        }

                        #region 生成更新SQL语句

                        if (string.IsNullOrEmpty(updateSetStr))
                        {
                            updateSetStr = colName[i] + "=" + colValue[i];
                        }
                        else
                        {
                            updateSetStr += "," + colName[i] + "=" + colValue[i];
                        }
                        #endregion

                        #region 生成新增SQL语句

                        if (string.IsNullOrEmpty(insertColNameStr))
                        {
                            insertColNameStr = colName[i];
                        }
                        else
                        {
                            insertColNameStr += "," + colName[i];
                        }

                        if (string.IsNullOrEmpty(insertColValueStr))
                        {
                            insertColValueStr = colValue[i];
                        }
                        else
                        {
                            insertColValueStr += "," + colValue[i];
                        }
                        #endregion

                    }

                    SqlCommand cmdAddOrUpdate = null;
                    if (dtPatientExt.Rows.Count > 0)//如果大于零,则已经存在此条记录,则update
                    {
                        //获取更新病人扩展表patients_ext的语句
                        cmdAddOrUpdate = GetUpdatePatientExtInfoCMD(updateSetStr, pat_id);
                    }
                    else
                    {
                        //获取新增病人扩展表patients_ext的语句
                        cmdAddOrUpdate = GetInsertPatientExtInfoCMD(insertColNameStr, insertColValueStr, pat_id);
                    }

                    using (DBHelper helper = DBHelper.BeginTransaction())//事务
                    {
                        helper.ExecuteNonQuery(cmdAddOrUpdate);//新增或更新病人扩展表信息
                        helper.Commit();//提交事务
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "AddOrUpdatePatientExt", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }

            return result;
        }

        public EntityOperationResult InsertBarCodePatient(EntityRemoteCallClientInfo caller, DataSet dsData, DBHelper transHelper)
        {
            EntityOperationResult result = new EntityOperationResult();//.GetNew("保存条码病人信息");
            result.ReturnResult = dsData;
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];


            string strSql = string.Format("select pat_id  from patients where pat_itr_id='{0}' and pat_bar_code='{1}' and pat_sid='{2}' ",
                                            dtPatInfo.Rows[0]["pat_itr_id"].ToString(), dtPatInfo.Rows[0]["pat_bar_code"].ToString(), dtPatInfo.Rows[0]["pat_sid"].ToString());

            DBHelper pathelper = new DBHelper();
            DataTable dtPat = pathelper.GetTable(strSql);
            if (dtPat.Rows.Count > 0)
            {
                //result.Success = false;
                return result;
            }




            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }
            if (dtPatInfo.Rows.Count > 0 && dtPatInfo.Columns.Contains("pat_rem"))//取默认标本状态
            {
                dtPatInfo.Rows[0]["pat_rem"] = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");
            }

            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (dtPatInfo.Rows.Count > 0
                    && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
                {
                    DateTime pat_jy_date = (DateTime)dtPatInfo.Rows[0]["pat_jy_date"];
                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                    if (pat_jy_date > now)
                    {
                        dtPatInfo.Rows[0]["pat_apply_date"] = now;
                    }
                    else
                    {
                        dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_jy_date"];
                    }


                }

            }

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "HIS代码关联")
            {
                dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(dtPatInfo.Rows[0]["pat_doc_id"].ToString());

                if (
                    (dtPatInfo.Rows[0]["pat_doc_id"] == DBNull.Value || dtPatInfo.Rows[0]["pat_doc_id"] == null)
                    && dtPatInfo.Rows[0]["pat_doc_name"] != DBNull.Value && dtPatInfo.Rows[0]["pat_doc_name"] != null
                    )
                {
                    dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(dtPatInfo.Rows[0]["pat_doc_name"].ToString());
                }
            }

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMD(dtPatInfo, result);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }



            try
            {
                DateTime pat_date = (DateTime)dtPatInfo.Rows[0]["pat_date"];
                string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                result.Data.Patient.RepSid = pat_sid;
                result.Data.Patient.PidName = dtPatInfo.Rows[0]["pat_name"].ToString();

                if (transHelper != null)
                {
                    //判断是否已回退
                    if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                    {
                        result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                        result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                    }
                    //判断样本号是否存在
                    else if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                    {
                        result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        int? host_order = null;
                        if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                        {
                            host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                        }

                        DictInstructmentBLL bllItr = new DictInstructmentBLL();

                        //判断序号是否存在
                        if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                        {
                            result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                            {
                                dtPatInfo.Rows[0]["pat_jy_date"] = ServerDateTime.GetDatabaseServerDateTime();
                            }

                            //插入病人资料
                            foreach (SqlCommand cmd in cmdInsertInfo)
                            {
                                transHelper.ExecuteNonQuery(cmd);
                            }

                            //获取插入组合cmd
                            List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, result);

                            //插入组合
                            foreach (SqlCommand cmd in cmdInsertCombine)
                            {
                                transHelper.ExecuteNonQuery(cmd);
                            }

                            new PatCommonBll().InsertDefaultResult(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                     , dtPatCombine
                                                     , transHelper);

                            if (!string.IsNullOrEmpty(barcode))
                            {
                                string barcodeRemark = string.Empty;

                                //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                //{
                                //*************************************************************************************
                                //将序号写入备注中
                                //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                //}

                                if (host_order.HasValue)
                                {
                                    barcodeRemark = string.Format("仪器：{0}，标本号：{1}, 序号：{2}，登记组合：{3}", dtPatInfo.Rows[0]["itr_name"], pat_sid, host_order.Value, dtPatInfo.Rows[0]["pat_c_name"]);

                                }
                                else
                                {

                                    barcodeRemark = string.Format("仪器：{0}，标本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);

                                }

                                //************************************************************************************

                                new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, transHelper, dtPatInfo.Rows[0]["pat_id"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        using (DBHelper helper = DBHelper.BeginTransaction())
                        {
                            //判断是否已回退
                            if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                            {
                                result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                                result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                            }
                            else if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                            {
                                result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                            }
                            else
                            {
                                int? host_order = null;

                                if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                                {
                                    host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                                }

                                DictInstructmentBLL bllItr = new DictInstructmentBLL();
                                if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                                {
                                    result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                                }
                                else
                                {

                                    if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                                    {
                                        dtPatInfo.Rows[0]["pat_jy_date"] = ServerDateTime.GetDatabaseServerDateTime();
                                    }

                                    //插入病人资料
                                    foreach (SqlCommand cmd in cmdInsertInfo)
                                    {
                                        helper.ExecuteNonQuery(cmd);
                                    }

                                    //获取插入组合cmd
                                    List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, result);

                                    //插入组合
                                    foreach (SqlCommand cmd in cmdInsertCombine)
                                    {
                                        helper.ExecuteNonQuery(cmd);
                                    }

                                    new PatCommonBll().InsertDefaultResult(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                        , dtPatCombine
                                                        , null);


                                    if (!string.IsNullOrEmpty(barcode))
                                    {
                                        string barcodeRemark = string.Empty;

                                        //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                        //{

                                        //*************************************************************************************
                                        //将序号写入备注中
                                        //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                        //}
                                        if (host_order.HasValue)
                                        {
                                            barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}", dtPatInfo.Rows[0]["itr_name"], pat_sid, host_order.Value, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                                        }
                                        else
                                        {
                                            barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                                        }

                                        //************************************************************************************

                                        new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, null, dtPatInfo.Rows[0]["pat_id"].ToString());
                                    }

                                    if (result.Success)
                                    {
                                        helper.Commit();
                                        string pat_ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();
                                        if (pat_ori_id == "107")//门诊
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                                            }
                                        }
                                        else if (pat_ori_id == "108")//住院
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                                            }

                                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_RegisterExecuteYZ") == "启用")
                                            {
                                                ExecuteYZ(barcode);
                                            }

                                        }
                                        else if (pat_ori_id == "109")//体检
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirm").Contains("标本登记") || CacheSysConfig.Current.GetSystemConfig("SP_InterfaceForHN") == "是")
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(barcode) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_UploadProcessBarCode") == "是")
                                        {
                                            //Lis.SendDataToCDR.CDRService cds = new Lis.SendDataToCDR.CDRService();
                                            //cds.UploadProcessInvoke(barcode);
                                        }

                                    }
                                    else
                                    {
                                        helper.RollBack();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatient", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }


        private void ExecuteYZ(string strBarCode)
        {
            DBHelper helper = new DBHelper();
            string strSql = string.Format(@"select 
                                bc_patients.bc_in_no,
                                bc_cname.bc_yz_id,
                                bc_patients.bc_bar_code,
                                bc_cname.bc_his_code,
                                bc_cname.bc_his_name,
                                bc_patients.bc_pid,
                                bc_patients.bc_times,
                                bc_patients.bc_name,
                                bc_patients.bc_social_no
                                from 
                                bc_patients  WITH (NOLOCK)
                                left join bc_cname on bc_cname.bc_bar_no=bc_patients.bc_bar_no
                                where bc_patients.bc_bar_no='{0}'", strBarCode);

            DataTable dtBcPat = helper.GetTable(strSql);

            foreach (DataRow item in dtBcPat.Rows)
            {
                if (item["bc_yz_id"] != null && item["bc_yz_id"].ToString() != string.Empty)
                {
                    List<InterfaceDataBindingItem> listPart = new List<InterfaceDataBindingItem>();
                    listPart.Add(new InterfaceDataBindingItem("bc_in_no", item["bc_in_no"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_yz_id", item["bc_yz_id"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_bar_code", item["bc_bar_code"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_his_code", item["bc_his_code"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_his_name", item["bc_his_name"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_pid", item["bc_pid"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_times", item["bc_times"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("bc_name", item["bc_name"].ToString()));
                    listPart.Add(new InterfaceDataBindingItem("op_time", DateTime.Now));
                    listPart.Add(new InterfaceDataBindingItem("bc_social_no", item["bc_social_no"].ToString()));

                    DataInterfaceHelper dihelper = new DataInterfaceHelper(EnumDataAccessMode.DirectToDB, true);

                    dihelper.ExecuteNonQueryWithGroup("住院_条码_登记后", listPart.ToArray());
                }
            }
        }

        public EntityOperationResult InsertBarCodePatientForBf(EntityRemoteCallClientInfo caller, DataSet dsData, DBHelper transHelper)
        {
            EntityOperationResult result = new EntityOperationResult();//.GetNew("保存条码病人信息");
            result.ReturnResult = dsData;
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];
            DateTime now = ServerDateTime.GetDatabaseServerDateTime();

            string strSql = string.Format("select pat_id  from patients_newborn where pat_itr_id='{0}' and pat_bar_code='{1}' and pat_sid='{2}' ",
                                            dtPatInfo.Rows[0]["pat_itr_id"].ToString(), dtPatInfo.Rows[0]["pat_bar_code"].ToString(), dtPatInfo.Rows[0]["pat_sid"].ToString());

            DBHelper pathelper = new DBHelper();
            DataTable dtPat = pathelper.GetTable(strSql);
            if (dtPat.Rows.Count > 0)
            {
                //result.Success = false;
                return result;
            }

            string pat_upid = "isnewfiltercode";

            if (dtPatInfo.Columns.Contains("pat_upid") && dtPatInfo.Rows[0]["pat_upid"] != null
                && dtPatInfo.Rows[0]["pat_upid"] != DBNull.Value && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_upid"].ToString()))
            {
                pat_upid = dtPatInfo.Rows[0]["pat_upid"].ToString();
            }



            strSql = string.Format("select top 1 pat_upid from patients_newborn where pat_upid='{0}' ", pat_upid);
            DataTable dtPatOld = pathelper.GetTable(strSql);

            if (dtPatOld.Rows.Count > 0)
            {
                dtPatInfo.Rows[0]["pat_prt_flag"] = 3;
            }
            else
            {
                dtPatInfo.Rows[0]["pat_prt_flag"] = 1;

                if (pat_upid == "isnewfiltercode")
                {
                    dtPatInfo.Rows[0]["pat_upid"] = GetPatFiltercodeNewNo();
                }
            }



            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }
            if (dtPatInfo.Rows.Count > 0 && dtPatInfo.Columns.Contains("pat_rem"))//取默认标本状态
            {
                dtPatInfo.Rows[0]["pat_rem"] = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");
            }

            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (dtPatInfo.Rows.Count > 0
                    && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
                {
                    DateTime pat_jy_date = (DateTime)dtPatInfo.Rows[0]["pat_jy_date"];


                    if (pat_jy_date > now)
                    {
                        dtPatInfo.Rows[0]["pat_apply_date"] = now;
                    }
                    else
                    {
                        dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_jy_date"];
                    }


                }

            }

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "HIS代码关联")
            {
                dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(dtPatInfo.Rows[0]["pat_doc_id"].ToString());

                if (
                    (dtPatInfo.Rows[0]["pat_doc_id"] == DBNull.Value || dtPatInfo.Rows[0]["pat_doc_id"] == null)
                    && dtPatInfo.Rows[0]["pat_doc_name"] != DBNull.Value && dtPatInfo.Rows[0]["pat_doc_name"] != null
                    )
                {
                    dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(dtPatInfo.Rows[0]["pat_doc_name"].ToString());
                }
            }

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMDForBf(dtPatInfo, result);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }



            try
            {
                DateTime pat_date = (DateTime)dtPatInfo.Rows[0]["pat_date"];
                string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                result.Data.Patient.RepSid = pat_sid;
                result.Data.Patient.PidName = dtPatInfo.Rows[0]["pat_name"].ToString();

                if (transHelper != null)
                {
                    //判断是否已回退
                    if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                    {
                        result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                        result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                    }
                    //判断样本号是否存在
                    else if (PatCommonBll.ExistSIDForBabyFilter(pat_sid, pat_itr_id, pat_date))
                    {
                        result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        int? host_order = null;
                        if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                        {
                            host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                        }

                        DictInstructmentBLL bllItr = new DictInstructmentBLL();

                        //判断序号是否存在
                        if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrderForBabyFilter(host_order.Value, pat_itr_id, pat_date))
                        {
                            result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                            {
                                dtPatInfo.Rows[0]["pat_jy_date"] = ServerDateTime.GetDatabaseServerDateTime();
                            }

                            //插入病人资料
                            foreach (SqlCommand cmd in cmdInsertInfo)
                            {
                                transHelper.ExecuteNonQuery(cmd);
                            }

                            //获取插入组合cmd
                            List<SqlCommand> cmdInsertCombine = GetCombineInsertCMDForBf(dtPatCombine, barcode, result);

                            //插入组合
                            foreach (SqlCommand cmd in cmdInsertCombine)
                            {
                                transHelper.ExecuteNonQuery(cmd);
                            }

                            new PatCommonBll().InsertDefaultResultForBf(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                     , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                     , dtPatCombine
                                                     , transHelper);

                            if (!string.IsNullOrEmpty(barcode))
                            {
                                string barcodeRemark = string.Empty;

                                //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                //{
                                //*************************************************************************************
                                //将序号写入备注中
                                //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                //}

                                if (host_order.HasValue)
                                {
                                    barcodeRemark = string.Format("仪器：{0}，标本号：{1}, 序号：{2}，登记组合：{3}", dtPatInfo.Rows[0]["itr_name"], pat_sid, host_order.Value, dtPatInfo.Rows[0]["pat_c_name"]);

                                }
                                else
                                {

                                    barcodeRemark = string.Format("仪器：{0}，标本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);

                                }

                                //************************************************************************************

                                new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, now, transHelper, dtPatInfo.Rows[0]["pat_id"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        using (DBHelper helper = DBHelper.BeginTransaction())
                        {
                            //判断是否已回退
                            if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                            {
                                result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                                result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                            }
                            else if (PatCommonBll.ExistSIDForBabyFilter(pat_sid, pat_itr_id, pat_date))
                            {
                                result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                            }
                            else
                            {
                                int? host_order = null;

                                if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                                {
                                    host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                                }

                                DictInstructmentBLL bllItr = new DictInstructmentBLL();
                                if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrderForBabyFilter(host_order.Value, pat_itr_id, pat_date))
                                {
                                    result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                                }
                                else
                                {

                                    if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                                    {
                                        dtPatInfo.Rows[0]["pat_jy_date"] = now;
                                    }

                                    //插入病人资料
                                    foreach (SqlCommand cmd in cmdInsertInfo)
                                    {
                                        helper.ExecuteNonQuery(cmd);
                                    }

                                    //获取插入组合cmd
                                    List<SqlCommand> cmdInsertCombine = GetCombineInsertCMDForBf(dtPatCombine, barcode, result);

                                    //插入组合
                                    foreach (SqlCommand cmd in cmdInsertCombine)
                                    {
                                        helper.ExecuteNonQuery(cmd);
                                    }

                                    new PatCommonBll().InsertDefaultResultForBf(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                        , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                        , dtPatCombine
                                                        , null);


                                    if (!string.IsNullOrEmpty(barcode))
                                    {
                                        string barcodeRemark = string.Empty;

                                        //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                        //{

                                        //*************************************************************************************
                                        //将序号写入备注中
                                        //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                        //}
                                        if (host_order.HasValue)
                                        {
                                            barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}", dtPatInfo.Rows[0]["itr_name"], pat_sid, host_order.Value, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                                        }
                                        else
                                        {
                                            barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                                        }

                                        //************************************************************************************

                                        new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, now, null, dtPatInfo.Rows[0]["pat_id"].ToString());
                                    }

                                    if (result.Success)
                                    {
                                        helper.Commit();
                                        string pat_ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();
                                        if (pat_ori_id == "107")//门诊
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirm").Contains("标本登记"))
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                                            }
                                        }
                                        else if (pat_ori_id == "108")//住院
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirm").Contains("标本登记"))
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                                            }
                                        }
                                        else if (pat_ori_id == "109")//体检
                                        {
                                            if (CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirm").Contains("标本登记"))
                                            {
                                                new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute(); ;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        helper.RollBack();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatient", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }

        /// <summary>
        /// 保存病人、普通结果信息
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, DataSet dsData)
        {

            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("保存病人、普通结果信息");
            opResult.ReturnResult = dsData;


            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];//病人表
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];//组合表
            DataTable dtPatResult = dsData.Tables[PatientTable.PatientResultTableName];//结果表
            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (dtPatInfo.Rows.Count > 0
                    && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_date"];
                }
            }

            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }

            //有权限操作保存结果的则可以不对比历史结果
            if (caller.OperationName.Trim() != string.Empty && !Convert.ToBoolean(caller.OperationName))
            {
                #region 对比历史结果


                dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare checkHistoryRes = new dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare(null, null, EnumOperationCode.Unspecified, null);
                //if (!checkHistoryRes.Savecheck(dtPatInfo, dtPatResult, ref opResult))
                //{
                //    return opResult;
                //}

                #endregion
            }
            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMD(dtPatInfo, opResult);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }

            //获取插入组合cmd
            List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, opResult);


            List<SqlCommand> cmdInsertPatResult = GetCommonResultInsertCMD(dtPatResult, opResult);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {

                    DateTime pat_date = Convert.ToDateTime(dtPatInfo.Rows[0]["pat_date"]);
                    string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                    string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                    if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                    {
                        opResult.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        int? host_order = null;
                        if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                        {
                            host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                        }

                        DictInstructmentBLL bllItr = new DictInstructmentBLL();
                        if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                        {
                            opResult.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            //插入病人资料
                            foreach (SqlCommand cmd in cmdInsertInfo)
                            {
                                helper.ExecuteNonQuery(cmd);
                            }

                            //插入组合
                            //DBHelper helper2 = new DBHelper();
                            foreach (SqlCommand cmd in cmdInsertCombine)
                            {
                                //helper2.ExecuteNonQuery(cmd);
                                helper.ExecuteNonQuery(cmd);
                            }

                            //插入结果
                            foreach (SqlCommand cmd in cmdInsertPatResult)
                            {
                                helper.ExecuteNonQuery(cmd);
                            }

                            if (!string.IsNullOrEmpty(barcode) && dtPatInfo.Columns.Contains("itr_name"))
                            {
                                string barcodeRemark = string.Empty;

                                //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                //{
                                barcodeRemark = string.Format("仪器：{0}，标本号：{1}，登记组合：{2},日期：{3}", dtPatInfo.Rows[0]["itr_name"],
                                                              pat_sid, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                                //}

                                //###########################################################################################################
                                //此处增加了pat_id号
                                new PatCommonBll().UpdateBarcodeStatus(caller,
                                                                       EnumBarcodeOperationCode.SampleRegister.ToString(),
                                                                       barcode, barcodeRemark, null, helper,
                                                                       dtPatInfo.Rows[0]["pat_id"].ToString());
                                //new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, helper);

                                //#############################################################################################################
                            }
                            //假如没有条码号，按照没有条码号的插入法插入
                            else
                            {
                                new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, caller.Remarks, null, helper, dtPatInfo.Rows[0]["pat_id"].ToString());
                            }

                            //**********************************************************************************************//

                            if (opResult.Success)
                            {
                                helper.Commit();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SavePatCommonResult", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }


        /// <summary>
        /// 保存病人、普通结果信息
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult InsertPatCommonResultForBf(EntityRemoteCallClientInfo caller, DataSet dsData)
        {

            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("保存病人、普通结果信息");
            opResult.ReturnResult = dsData;


            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];//病人表
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];//组合表
            DataTable dtPatResult = dsData.Tables[PatientTable.PatientResultTableName];//结果表
            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (dtPatInfo.Rows.Count > 0
                    && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_date"];
                }
            }


            string pat_upid = "isnewfiltercode";

            if (dtPatInfo.Columns.Contains("pat_upid") && dtPatInfo.Rows[0]["pat_upid"] != null
                && dtPatInfo.Rows[0]["pat_upid"] != DBNull.Value && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_upid"].ToString()))
            {
                pat_upid = dtPatInfo.Rows[0]["pat_upid"].ToString();
            }



            string strSql = string.Format("select top 1 pat_upid from patients_newborn where pat_upid='{0}' ", pat_upid);
            DBHelper pathelper = new DBHelper();
            DataTable dtPatOld = pathelper.GetTable(strSql);

            if (dtPatOld.Rows.Count > 0)
            {
                dtPatInfo.Rows[0]["pat_prt_flag"] = 3;
            }
            else
            {
                dtPatInfo.Rows[0]["pat_prt_flag"] = 1;

                if (pat_upid == "isnewfiltercode")
                {
                    dtPatInfo.Rows[0]["pat_upid"] = GetPatFiltercodeNewNo();
                }
            }

            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMDForBf(dtPatInfo, opResult);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }

            //获取插入组合cmd
            List<SqlCommand> cmdInsertCombine = GetCombineInsertCMDForBf(dtPatCombine, barcode, opResult);


            List<SqlCommand> cmdInsertPatResult = GetCommonResultInsertCMDForBf(dtPatResult, opResult);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {

                    DateTime pat_date = Convert.ToDateTime(dtPatInfo.Rows[0]["pat_date"]);
                    string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                    string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                    if (PatCommonBll.ExistSIDForBabyFilter(pat_sid, pat_itr_id, pat_date))
                    {
                        opResult.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        int? host_order = null;
                        if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                        {
                            host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                        }

                        DictInstructmentBLL bllItr = new DictInstructmentBLL();
                        if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 && PatCommonBll.ExistHostOrderForBabyFilter(host_order.Value, pat_itr_id, pat_date))
                        {
                            opResult.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            //插入病人资料
                            foreach (SqlCommand cmd in cmdInsertInfo)
                            {
                                helper.ExecuteNonQuery(cmd);
                            }

                            //插入组合
                            //DBHelper helper2 = new DBHelper();
                            foreach (SqlCommand cmd in cmdInsertCombine)
                            {
                                //helper2.ExecuteNonQuery(cmd);
                                helper.ExecuteNonQuery(cmd);
                            }

                            //插入结果
                            foreach (SqlCommand cmd in cmdInsertPatResult)
                            {
                                helper.ExecuteNonQuery(cmd);
                            }

                            if (!string.IsNullOrEmpty(barcode) && dtPatInfo.Columns.Contains("itr_name"))
                            {
                                string barcodeRemark = string.Empty;

                                //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                //{
                                barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                //}

                                //###########################################################################################################
                                //此处增加了pat_id号
                                new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, helper, dtPatInfo.Rows[0]["pat_id"].ToString());
                                //new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, helper);

                                //#############################################################################################################
                            }
                            //假如没有条码号，按照没有条码号的插入法插入
                            else
                                new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, "", null, helper, dtPatInfo.Rows[0]["pat_id"].ToString());

                            //**********************************************************************************************//

                            if (opResult.Success)
                            {
                                helper.Commit();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SavePatCommonResult", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }


        /// <summary>
        /// 获取一个新的筛查号
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.Description("获取一个新的筛查号")]
        public string GetPatFiltercodeNewNo()
        {
            string newFiltercode = "";
            try
            {
                DBHelper helper = new DBHelper();

                DataSet dsResult = helper.ExecuteSql("sp_get_filtercode");
                if (dsResult != null)
                {
                    DataTable dtResult = dsResult.Tables[0];
                    newFiltercode = dtResult.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatFiltercodeNewNo", ex.ToString());
            }
            return newFiltercode;
        }

        public EntityOperationResult InsertPatCommonResultItems(string pat_id, DataTable dtResulto)
        {

            EntityOperationResult opResult = new EntityOperationResult();
            string sqlSelect = string.Format(@"
select top 1
pat_sam_id,
pat_itr_id,
pat_sid
from patients
where patients.pat_id = '{0}'
", pat_id);
            DBHelper helper = new DBHelper();
            DataTable dtpatient = helper.GetTable(sqlSelect);

            if (dtpatient.Rows.Count > 0)
            {
                DateTime dtToday = ServerDateTime.GetDatabaseServerDateTime();
                foreach (DataRow drResulto in dtResulto.Rows)
                {
                    drResulto["res_id"] = pat_id;
                    drResulto["res_itr_id"] = dtpatient.Rows[0]["pat_itr_id"];
                    drResulto["res_sid"] = dtpatient.Rows[0]["pat_sid"];
                    drResulto["res_date"] = dtToday;
                    drResulto["res_flag"] = 1;
                }

                List<SqlCommand> cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtResulto);

                try
                {

                    using (DBHelper trandHelper = DBHelper.BeginTransaction())
                    {
                        foreach (SqlCommand cmd in cmdsResult)
                        {
                            trandHelper.ExecuteNonQuery(cmd);

                        }
                        trandHelper.Commit();
                    }


                    string sqlSelectKey = "select top 1 res_key from resulto where res_id = @res_id and res_itm_id = @res_itm_id";

                    foreach (DataRow drResulto in dtResulto.Rows)
                    {
                        string itm_id = drResulto["res_itm_id"].ToString();


                        SqlCommand cmd = new SqlCommand(sqlSelectKey);
                        SqlParameter p1 = cmd.Parameters.AddWithValue("res_id", pat_id);
                        p1.DbType = DbType.AnsiString;

                        SqlParameter p2 = cmd.Parameters.AddWithValue("res_itm_id", itm_id);
                        p2.DbType = DbType.AnsiString;

                        object objResKey = helper.ExecuteScalar(cmd);

                        DataRow[] drsResulto = dtResulto.Select(string.Format("res_itm_id = '{0}'", itm_id));

                        if (drsResulto.Length > 0)
                        {
                            drsResulto[0]["res_key"] = objResKey;
                        }
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtResulto);
                    opResult.ReturnResult = ds;
                }
                catch (Exception ex)
                {

                    Logger.WriteException(this.GetType().Name, "InsertPatCommonResultItems", ex.ToString());
                    opResult.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
                }

                //List<SqlCommand> cmdInsertPatResult = GetCommonResultInsertCMD(dtResulto, opResult);
            }

            //                drsResult["res_id"] = pat_id;
            //    drsResult["res_sid"] = pat_sid;
            //    drsResult["res_type"] = 0;
            //    drsResult["res_date"] = today;
            //    drsResult["res_flag"] = 1;
            //    drsResult["res_rep_type"] = 0;
            //}

            //cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtPatientResult);
            return opResult;
        }

        public IEnumerable<EntityOperationResult> BatchInsertPatCommonResult(IEnumerable<PatientCommonResultEntity> data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存病人、描述结果信息
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public EntityOperationResult InsertPatDescResult(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("保存病人、描述结果信息");
            opResult.ReturnResult = dsData;
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];
            DataTable dtPatDescResult = dsData.Tables[PatientTable.PatientDescResultTableName];


            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (dtPatInfo.Rows.Count > 0
                    && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_date"];
                }
            }

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMD(dtPatInfo, opResult);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }

            //获取插入组合cmd
            List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, opResult);

            //获取插入描述结果cmd
            List<SqlCommand> cmdInsertDescResult = GetDescResultInsertCMD(dtPatDescResult, opResult);

            try
            {
                using (DBHelper helper = DBHelper.BeginTransaction())
                {
                    DateTime pat_date = (DateTime)dtPatInfo.Rows[0]["pat_date"];
                    string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                    string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                    if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                    {
                        opResult.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                    }
                    else
                    {
                        //插入病人资料
                        foreach (SqlCommand cmd in cmdInsertInfo)
                        {
                            helper.ExecuteNonQuery(cmd);
                        }

                        //插入组合
                        //DBHelper helper2 = new DBHelper();
                        foreach (SqlCommand cmd in cmdInsertCombine)
                        {
                            //helper2.ExecuteNonQuery(cmd);
                            helper.ExecuteNonQuery(cmd);
                        }

                        //插入描述结果
                        foreach (SqlCommand cmd in cmdInsertDescResult)
                        {
                            helper.ExecuteNonQuery(cmd);
                        }


                        if (!string.IsNullOrEmpty(barcode))
                        {
                            //string barcodeRemark = string.Empty;
                            string barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"], dtPatInfo.Rows[0]["pat_date"].ToString());
                            //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                            //{
                            // barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                            //}

                            new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, barcodeRemark, null, helper);
                        }
                        if (string.IsNullOrEmpty(barcode) && caller != null && !string.IsNullOrEmpty(caller.Remarks))
                        {
                            new PatCommonBll().UpdateBarcodeStatus(caller, EnumBarcodeOperationCode.SampleRegister.ToString(), barcode, caller.Remarks, null, helper, dtPatInfo.Rows[0]["pat_id"].ToString());
                        }
                        if (opResult.Success)
                        {
                            helper.Commit();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "SavePatCommonResult", ex.ToString());
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }

        /// <summary>
        /// 保存病人信息
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private List<SqlCommand> GetPatientInfoInsertCMD(DataTable dtPatientsInfo, EntityOperationResult result)
        {
            //new PatCommonBll().AdjustSampleDate(dtPatientsInfo);

            DataRow drPatient = dtPatientsInfo.Rows[0];

            ////样本号
            string pat_sid = drPatient["pat_sid"].ToString();

            DateTime pat_date = Convert.ToDateTime(drPatient["pat_date"]);
            string nowTime = DateTime.Now.ToString(" HH:mm:ss");

            //            //检查样本号是否已存在
            //            string sqlSelect = string.Format(@"
            //                    select top 1 pat_sid from 
            //                    patients where pat_sid='{0}' 
            //                    and pat_itr_id ='{1}' 
            //                    and pat_date >= '{2}' 
            //                    and pat_date<'{3}'", pat_sid, drPatient["pat_itr_id"].ToString(), pat_date.Date, pat_date.AddDays(1).Date);
            //            object objSID = transHelper.ExecuteScalar(sqlSelect);
            //            if (objSID != null && objSID != DBNull.Value)
            //            {
            //                result.AddError(EnumOperationErrorCode.SIDExist);
            //            }
            //            else
            //            {
            string pat_id = drPatient["pat_itr_id"].ToString() + pat_date.ToString("yyyyMMdd") + drPatient["pat_sid"].ToString();
            drPatient["pat_date"] = pat_date.ToString("yyyy-MM-dd") + nowTime;
            drPatient["pat_id"] = pat_id;
            drPatient["pat_flag"] = "0";
            drPatient["pat_modified_times"] = 0;//修改次数

            if (!Compare.IsEmpty(drPatient["pat_age_exp"]))
            {
                drPatient["pat_age"] = AgeConverter.AgeValueTextToMinute(drPatient["pat_age_exp"].ToString());
            }

            //根据仪器报警内容更新复查标记
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM instrmt_warning_msg WHERE WARN_PAT_ID='{0}'", pat_id);
                using (DBHelper helper = new DBHelper())
                {
                    object obj = helper.ExecuteScalar(sql);
                    if (obj != null && obj != DBNull.Value && Convert.ToInt32(obj) > 0)
                    {
                        drPatient["pat_recheck_flag"] = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetPatientInfoInsertCMD", ex.ToString());
            }


            List<SqlCommand> cmdsPatient = DBTableHelper.GenerateInsertCommand(PatientTable.PatientInfoTableName, new string[] { "pat_key" }, dtPatientsInfo);



            //try
            //{
            //    transHelper.ExecuteNonQuery(cmdsPatient[0]);
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteException(this.GetType().Name, "SavePatientInfo", ex.ToString());
            //    result.AddError(EnumOperationErrorCode.Exception, ex.ToString());
            //}
            //}

            return cmdsPatient;
        }



        /// <summary>
        /// 获取保存组合sqlcommand
        /// </summary>
        /// <param name="dtPatientCombine"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        //private List<SqlCommand> GetCombineInsertCMD(DataTable dtPatientCombine, EntityOperationResult opResult)
        //{
        //    return GetCombineInsertCMD(dtPatientCombine, string.Empty, opResult);
        //}

        private List<SqlCommand> GetCombineInsertCMD(DataTable dtPatientCombine, string barcode, EntityOperationResult opResult)
        {
            List<SqlCommand> cmdsPatientMi;
            if (opResult.Success && dtPatientCombine != null)
            {
                string pat_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();

                StringBuilder sbCom_id = new StringBuilder();
                bool needComma = false;

                dtPatientCombine.DefaultView.Sort = "com_seq";

                int i = 0;
                foreach (DataRowView drCombine in dtPatientCombine.DefaultView)
                {
                    drCombine.Row["pat_seq"] = i;
                    if (!Compare.IsEmpty(drCombine.Row["pat_com_id"].ToString()))
                    {
                        drCombine.Row["pat_id"] = pat_id;

                        if (needComma)
                        {
                            sbCom_id.Append(",");
                        }

                        sbCom_id.Append(string.Format(" '{0}' ", drCombine.Row["pat_com_id"].ToString()));

                        needComma = true;
                    }
                    i++;
                }

                ////如果有使用条码，并根据配置是否更新patients_mi.pat_yz_id为bc_cname.bc_yz_id
                //if (!string.IsNullOrEmpty(barcode)
                //    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AutoFillOrderIdWhenSave") == "是"
                //    && sbCom_id.Length > 0
                //    )
                //{
                //    DBHelper helperGetYzID = new DBHelper();

                //    //根据条码号与组合id查找出对应的医嘱(一次性取出该条码对应的医嘱)
                //    string sqlSelectYzId = string.Format("select bc_yz_id,bc_lis_code from bc_cname with (nolock) where bc_bar_code = '{0}' and bc_lis_code in ({1})", barcode, sbCom_id.ToString());

                //    DataTable tableYzId = helperGetYzID.GetTable(sqlSelectYzId);

                //    foreach (DataRow rowCombine in dtPatientCombine.Rows)
                //    {
                //        if (Compare.IsEmpty(rowCombine["pat_yz_id"]) || rowCombine["pat_yz_id"].ToString().Trim() != string.Empty)
                //        {
                //            string com_id = rowCombine["pat_com_id"].ToString();

                //            DataRow[] drsYZ = tableYzId.Select(string.Format("bc_lis_code = '{0}'", com_id));

                //            if (drsYZ.Length > 0 && !Compare.IsEmpty(drsYZ[0]["bc_yz_id"]))
                //            {
                //                rowCombine["pat_yz_id"] = drsYZ[0]["bc_yz_id"];
                //            }
                //        }
                //    }
                //}

                cmdsPatientMi = DBTableHelper.GenerateInsertCommand(PatientTable.PatientCombineTableName, null, dtPatientCombine);

                string sqlDeletePatientMi = string.Format("delete patients_mi where pat_id ='{0}'", pat_id);

                DBHelper helper_bccname = new DBHelper();
                helper_bccname.ExecuteNonQuery(sqlDeletePatientMi);

                //如果有条码号则更新bc_cname标志，
                if (!string.IsNullOrEmpty(barcode) && sbCom_id.Length > 0)
                {
                    string sqlUpdateBcCname = string.Format(@"update bc_cname set bc_flag = 1 where bc_bar_code = '{0}' and (bc_lis_code in ({1}) or 
exists(select top 1 1 from bc_patients where bc_bar_code='{0}' and bc_merge_comid is not null and bc_merge_comid<>''))", barcode, sbCom_id.ToString());
                    SqlCommand cmUpdateBcCname = new SqlCommand(sqlUpdateBcCname);
                    //DBHelper helper_bccname = new DBHelper();
                    //helper_bccname.ExecuteNonQuery(cmUpdateBcCname);
                    cmdsPatientMi.Add(cmUpdateBcCname);
                    //
                }
            }
            else
            {
                cmdsPatientMi = new List<SqlCommand>();
            }
            return cmdsPatientMi;
        }

        //private void SetBcFlag(

        /// <summary>
        /// 保存普通结果
        /// </summary>
        /// <param name="dtPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private List<SqlCommand> GetCommonResultInsertCMD(DataTable dtPatientResultInput, EntityOperationResult opResult)
        {
            List<SqlCommand> cmdsResult = new List<SqlCommand>();
            if (opResult.Success && dtPatientResultInput != null)
            {
                DataTable dtPatientResult = dtPatientResultInput.Clone();
                DataTable dtPatientResultUpdate = dtPatientResultInput.Clone();
                string pat_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();
                string pat_sid = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();
                string pat_itr_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_itr_id"].ToString();

                DBHelper helper = new DBHelper();
                string sqlSelectCurrentResulto = string.Format("select * from  resulto  where res_id = '{0}' and res_flag = 1 ", pat_id);
                SqlCommand cmdSelectCurrentResulto = new SqlCommand(sqlSelectCurrentResulto);
                DataTable dtResultInDB = helper.GetTable(cmdSelectCurrentResulto);

                foreach (DataRow row in dtPatientResultInput.Rows)
                {
                    DataRow[] rows = dtResultInDB.Select(string.Format("res_itm_id = '{0}'", row["res_itm_id"].ToString()));
                    if (dtResultInDB != null && dtResultInDB.Rows.Count > 0 && rows != null && rows.Length > 0) //如果数据库有仪器结果
                    {
                        if (row["res_chr"] == null || row["res_chr"] == DBNull.Value || string.IsNullOrEmpty(row["res_chr"].ToString()) || rows[0]["res_type"].ToString() == "1") //当前项目结果或为仪器结果
                        {

                        }
                        else
                        {
                            dtPatientResultUpdate.Rows.Add(row.ItemArray);
                        }
                    }
                    else
                        dtPatientResult.Rows.Add(row.ItemArray);
                }

                //if (dtResultInDB != null && dtResultInDB.Rows.Count > 0) //如果数据库有仪器结果
                //{
                //    foreach (DataRow row in dtResultInDB)
                //    {
                //        DataRow[] rows = dtPatientResult.Select(string.Format("res_itm_id = '{0}'", row["res_itm_id"].ToString()));
                //        if (rows != null && rows.Length > 0 && string.IsNullOrEmpty(rows[0]["res_chr"].ToString())) //保存的项目没有结果
                //        {
                //            rows[0]["res_chr"] = row["res_chr"];
                //        }
                //    }

                //}
                ////插入结果前先更新原有结果为历史结果
                //string sqlUpdateCurrentResulto = string.Format("update resulto set res_flag = 0 where res_id = '{0}'", pat_id);
                //SqlCommand cmdUpdateCurrentResulto = new SqlCommand(sqlUpdateCurrentResulto);
                //cmdsResult.Add(cmdUpdateCurrentResulto);

                DateTime today = (DateTime)opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_date"];

                bool isVerify = ResultExistVerify();
                if (isVerify && !dtPatientResult.Columns.Contains("res_verify"))
                    dtPatientResult.Columns.Add("res_verify");

                foreach (DataRow drsResult in dtPatientResult.Rows)
                {
                    drsResult["res_id"] = pat_id;
                    drsResult["res_sid"] = pat_sid;
                    drsResult["res_itr_id"] = pat_itr_id;
                    drsResult["res_type"] = 0;
                    drsResult["res_date"] = today;
                    drsResult["res_flag"] = 1;
                    drsResult["res_rep_type"] = 0;
                    //if (isVerify)
                    //    drsResult["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(drsResult["res_itm_id"].ToString() + ";" + drsResult["res_chr"].ToString());
                }

                foreach (DataRow drsResult in dtPatientResultUpdate.Rows)
                {
                    drsResult["res_id"] = pat_id;
                    drsResult["res_sid"] = pat_sid;
                    drsResult["res_itr_id"] = pat_itr_id;
                    drsResult["res_type"] = 0;
                    drsResult["res_date"] = today;
                    drsResult["res_flag"] = 1;
                    drsResult["res_rep_type"] = 0;
                    //if (isVerify)
                    //    drsResult["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(drsResult["res_itm_id"].ToString() + ";" + drsResult["res_chr"].ToString());
                }
                cmdsResult.AddRange(DBTableHelper.GenerateUpdateCommand(PatientTable.PatientResultTableName, new string[] { "res_id", "res_itm_id" }, new string[] { "res_key" }, dtPatientResultUpdate));
                cmdsResult.AddRange(DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtPatientResult));
            }
            else
            {
                //cmdsResult = new List<SqlCommand>();
            }
            return cmdsResult;
        }

        /// <summary>
        /// 获取新增病人扩展表patients_ext的语句
        /// </summary>
        /// <param name="ColSql">列名字符串</param>
        /// <param name="valueSql">列值字符串</param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        private SqlCommand GetInsertPatientExtInfoCMD(string ColSql, string valueSql, string pat_id)
        {
            string sqlInsertPatientExtInfo = string.Format("INSERT INTO patients_ext ({0},pat_id) VALUES ({1},'{2}') ", ColSql, valueSql, pat_id);
            SqlCommand cmdInsert = new SqlCommand(sqlInsertPatientExtInfo);
            return cmdInsert;
        }
        /// <summary>
        /// 获取更新病人扩展表patients_ext的语句
        /// </summary>
        /// <param name="setSql">set内容</param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        private SqlCommand GetUpdatePatientExtInfoCMD(string setSql, string pat_id)
        {
            string sqlUpdatePatientExtInfo = string.Format("UPDATE patients_ext SET {0} WHERE pat_id='{1}' ", setSql, pat_id);
            SqlCommand cmdUpdate = new SqlCommand(sqlUpdatePatientExtInfo);
            return cmdUpdate;
        }


        /// <summary>
        /// 保存描述结果
        /// </summary>
        /// <param name="dtPatientDescResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private List<SqlCommand> GetDescResultInsertCMD(DataTable dtPatientDescResult, EntityOperationResult opResult)
        {
            List<SqlCommand> cmdsResult = null;
            if (opResult.Success)
            {
                string pat_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();
                string pat_sid = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();

                DateTime today = ServerDateTime.GetDatabaseServerDateTime();
                foreach (DataRow drDescResult in dtPatientDescResult.Rows)
                {
                    drDescResult["bsr_sid"] = pat_sid;
                    drDescResult["bsr_id"] = pat_id;
                    drDescResult["bsr_date"] = today;
                    drDescResult["bsr_res_flag"] = 1;
                }

                cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientDescResultTableName, null, dtPatientDescResult);

                //try
                //{
                //    foreach (SqlCommand cmdR in cmdsResult)
                //    {
                //        transHelper.ExecuteNonQuery(cmdR);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Logger.WriteException(this.GetType().Name, "SaveNewDescResult", ex.ToString());
                //    result.AddError(EnumOperationErrorCode.Exception, ex.ToString());
                //}
            }
            else
            {
                cmdsResult = new List<SqlCommand>();
            }
            return cmdsResult;
        }


        /// <summary>
        /// 保存病人信息
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private List<SqlCommand> GetPatientInfoInsertCMDForBf(DataTable dtPatientsInfo, EntityOperationResult result)
        {
            //new PatCommonBll().AdjustSampleDate(dtPatientsInfo);

            DataRow drPatient = dtPatientsInfo.Rows[0];
            ////样本号
            string pat_sid = drPatient["pat_sid"].ToString();

            DateTime pat_date = Convert.ToDateTime(drPatient["pat_date"]);
            string nowTime = DateTime.Now.ToString(" HH:mm:ss");

            //            //检查样本号是否已存在
            //            string sqlSelect = string.Format(@"
            //                    select top 1 pat_sid from 
            //                    patients where pat_sid='{0}' 
            //                    and pat_itr_id ='{1}' 
            //                    and pat_date >= '{2}' 
            //                    and pat_date<'{3}'", pat_sid, drPatient["pat_itr_id"].ToString(), pat_date.Date, pat_date.AddDays(1).Date);
            //            object objSID = transHelper.ExecuteScalar(sqlSelect);
            //            if (objSID != null && objSID != DBNull.Value)
            //            {
            //                result.AddError(EnumOperationErrorCode.SIDExist);
            //            }
            //            else
            //            {
            string pat_id = drPatient["pat_itr_id"].ToString() + pat_date.ToString("yyyyMMdd") + drPatient["pat_sid"].ToString();
            drPatient["pat_date"] = pat_date.ToString("yyyy-MM-dd") + nowTime;
            drPatient["pat_id"] = pat_id;
            drPatient["pat_flag"] = "0";
            drPatient["pat_modified_times"] = 0;//修改次数

            if (!Compare.IsEmpty(drPatient["pat_age_exp"]))
            {
                drPatient["pat_age"] = AgeConverter.AgeValueTextToMinute(drPatient["pat_age_exp"].ToString());
            }



            List<SqlCommand> cmdsPatient = DBTableHelper.GenerateInsertCommand("patients_newborn", new string[] { "pat_key" }, dtPatientsInfo);



            return cmdsPatient;
        }



        /// <summary>
        /// 获取保存组合sqlcommand
        /// </summary>
        /// <param name="dtPatientCombine"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        //private List<SqlCommand> GetCombineInsertCMD(DataTable dtPatientCombine, EntityOperationResult opResult)
        //{
        //    return GetCombineInsertCMD(dtPatientCombine, string.Empty, opResult);
        //}

        private List<SqlCommand> GetCombineInsertCMDForBf(DataTable dtPatientCombine, string barcode, EntityOperationResult opResult)
        {
            List<SqlCommand> cmdsPatientMi;
            if (opResult.Success && dtPatientCombine != null)
            {
                string pat_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();

                StringBuilder sbCom_id = new StringBuilder();
                bool needComma = false;

                dtPatientCombine.DefaultView.Sort = "com_seq";

                int i = 0;
                foreach (DataRowView drCombine in dtPatientCombine.DefaultView)
                {
                    drCombine.Row["pat_seq"] = i;
                    if (!Compare.IsEmpty(drCombine.Row["pat_com_id"].ToString()))
                    {
                        drCombine.Row["pat_id"] = pat_id;

                        if (needComma)
                        {
                            sbCom_id.Append(",");
                        }

                        sbCom_id.Append(string.Format(" '{0}' ", drCombine.Row["pat_com_id"].ToString()));

                        needComma = true;
                    }
                    i++;
                }

                ////如果有使用条码，并根据配置是否更新patients_mi.pat_yz_id为bc_cname.bc_yz_id
                //if (!string.IsNullOrEmpty(barcode)
                //    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AutoFillOrderIdWhenSave") == "是"
                //    && sbCom_id.Length > 0
                //    )
                //{
                //    DBHelper helperGetYzID = new DBHelper();

                //    //根据条码号与组合id查找出对应的医嘱(一次性取出该条码对应的医嘱)
                //    string sqlSelectYzId = string.Format("select bc_yz_id,bc_lis_code from bc_cname with (nolock) where bc_bar_code = '{0}' and bc_lis_code in ({1})", barcode, sbCom_id.ToString());

                //    DataTable tableYzId = helperGetYzID.GetTable(sqlSelectYzId);

                //    foreach (DataRow rowCombine in dtPatientCombine.Rows)
                //    {
                //        if (Compare.IsEmpty(rowCombine["pat_yz_id"]) || rowCombine["pat_yz_id"].ToString().Trim() != string.Empty)
                //        {
                //            string com_id = rowCombine["pat_com_id"].ToString();

                //            DataRow[] drsYZ = tableYzId.Select(string.Format("bc_lis_code = '{0}'", com_id));

                //            if (drsYZ.Length > 0 && !Compare.IsEmpty(drsYZ[0]["bc_yz_id"]))
                //            {
                //                rowCombine["pat_yz_id"] = drsYZ[0]["bc_yz_id"];
                //            }
                //        }
                //    }
                //}
                cmdsPatientMi = DBTableHelper.GenerateInsertCommand("patients_mi_newborn", null, dtPatientCombine);

                string sqlDeletePatientMi = string.Format("delete patients_mi_newborn where pat_id ='{0}'", pat_id);

                DBHelper helper_bccname = new DBHelper();
                helper_bccname.ExecuteNonQuery(sqlDeletePatientMi);

                //如果有条码号则更新bc_cname标志，
                if (!string.IsNullOrEmpty(barcode) && sbCom_id.Length > 0)
                {
                    string sqlUpdateBcCname = string.Format(@"update bc_cname set bc_flag = 1 where bc_bar_code = '{0}' and (bc_lis_code in ({1}) or 
exists(select top 1 1 from bc_patients where bc_bar_code='{0}' and bc_merge_comid is not null and bc_merge_comid<>''))", barcode, sbCom_id.ToString());
                    SqlCommand cmUpdateBcCname = new SqlCommand(sqlUpdateBcCname);
                    //DBHelper helper_bccname = new DBHelper();
                    //helper_bccname.ExecuteNonQuery(cmUpdateBcCname);
                    cmdsPatientMi.Add(cmUpdateBcCname);
                    //
                }
            }
            else
            {
                cmdsPatientMi = new List<SqlCommand>();
            }
            return cmdsPatientMi;
        }

        //private void SetBcFlag(

        /// <summary>
        /// 保存普通结果
        /// </summary>
        /// <param name="dtPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        private List<SqlCommand> GetCommonResultInsertCMDForBf(DataTable dtPatientResultInput, EntityOperationResult opResult)
        {
            List<SqlCommand> cmdsResult = new List<SqlCommand>();
            if (opResult.Success && dtPatientResultInput != null)
            {
                DataTable dtPatientResult = dtPatientResultInput.Clone();
                DataTable dtPatientResultUpdate = dtPatientResultInput.Clone();
                string pat_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_id"].ToString();
                string pat_sid = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_sid"].ToString();
                string pat_itr_id = opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_itr_id"].ToString();

                DBHelper helper = new DBHelper();
                string sqlSelectCurrentResulto = string.Format("select * from  resulto_newborn  where res_id = '{0}' and res_flag = 1 ", pat_id);
                SqlCommand cmdSelectCurrentResulto = new SqlCommand(sqlSelectCurrentResulto);
                DataTable dtResultInDB = helper.GetTable(cmdSelectCurrentResulto);

                foreach (DataRow row in dtPatientResultInput.Rows)
                {
                    DataRow[] rows = dtResultInDB.Select(string.Format("res_itm_id = '{0}'", row["res_itm_id"].ToString()));
                    if (dtResultInDB != null && dtResultInDB.Rows.Count > 0 && rows != null && rows.Length > 0) //如果数据库有仪器结果
                    {
                        if (row["res_chr"] == null || row["res_chr"] == DBNull.Value || string.IsNullOrEmpty(row["res_chr"].ToString()) || rows[0]["res_type"].ToString() == "1") //当前项目结果或为仪器结果
                        {

                        }
                        else
                        {
                            dtPatientResultUpdate.Rows.Add(row.ItemArray);
                        }
                    }
                    else
                        dtPatientResult.Rows.Add(row.ItemArray);
                }


                DateTime today = (DateTime)opResult.ReturnResult.Tables[PatientTable.PatientInfoTableName].Rows[0]["pat_date"];

                foreach (DataRow drsResult in dtPatientResult.Rows)
                {
                    drsResult["res_id"] = pat_id;
                    drsResult["res_sid"] = pat_sid;
                    drsResult["res_itr_id"] = pat_itr_id;
                    drsResult["res_type"] = 0;
                    drsResult["res_date"] = today;
                    drsResult["res_flag"] = 1;
                    drsResult["res_rep_type"] = 0;
                }

                foreach (DataRow drsResult in dtPatientResultUpdate.Rows)
                {
                    drsResult["res_id"] = pat_id;
                    drsResult["res_sid"] = pat_sid;
                    drsResult["res_itr_id"] = pat_itr_id;
                    drsResult["res_type"] = 0;
                    drsResult["res_date"] = today;
                    drsResult["res_flag"] = 1;
                    drsResult["res_rep_type"] = 0;
                }
                cmdsResult.AddRange(DBTableHelper.GenerateUpdateCommand("resulto_newborn", new string[] { "res_id", "res_itm_id" }, new string[] { "res_key" }, dtPatientResultUpdate));
                cmdsResult.AddRange(DBTableHelper.GenerateInsertCommand("resulto_newborn", new string[] { "res_key" }, dtPatientResult));
            }
            else
            {
                //cmdsResult = new List<SqlCommand>();
            }
            return cmdsResult;
        }

        /// <summary>
        /// 插入病人资料时更新条码表条码状态、插入条码日志记录
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="barcode"></param>
        /// <param name="dtCombine"></param>
        /// <param name="transHelper"></param>
        public void UpdateBarcodeInfoOnInsert(EntityRemoteCallClientInfo caller, string barcode, DataTable dtCombine, DBHelper transHelper)
        {

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

        //********************************************************************************//

        //*************************************************************************************//
        //新增检验报告记录登记到bc_sign表中
        public void insertBcSignNewCheckReport(EntityRemoteCallClientInfo caller, System.Data.DataSet dsData, string BarcodeOperation, string pat_id)
        {
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];//病人表

            string barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            new PatCommonBll().insertCheckReportToBcSign(caller, BarcodeOperation, barcode, pat_id);
        }

        public EntityOperationResult InsertBarCodePatientForSZ(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            EntityOperationResult result = new EntityOperationResult();//.GetNew("保存条码病人信息");
            result.ReturnResult = dsData;
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];


            string strSql = string.Format("select pat_id  from patients where pat_itr_id='{0}' and pat_bar_code='{1}' and pat_sid='{2}' ",
                                            dtPatInfo.Rows[0]["pat_itr_id"].ToString(), dtPatInfo.Rows[0]["pat_bar_code"].ToString(), dtPatInfo.Rows[0]["pat_sid"].ToString());

            DBHelper pathelper = new DBHelper();
            DataTable dtPat = pathelper.GetTable(strSql);
            if (dtPat.Rows.Count > 0)
            {
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatientForSZ", "已存在该样本号");
                //result.Success = false;
                return result;
            }




            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }
            if (dtPatInfo.Rows.Count > 0 && dtPatInfo.Columns.Contains("pat_rem"))//取默认标本状态
            {
                dtPatInfo.Rows[0]["pat_rem"] = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");
            }

            if (dtPatInfo.Rows.Count > 0
               && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                if (dtPatInfo.Rows[0]["pat_jy_date"] == null || dtPatInfo.Rows[0]["pat_jy_date"] == DBNull.Value)
                    dtPatInfo.Rows[0]["pat_jy_date"] = now;
                DateTime pat_jy_date = (DateTime)dtPatInfo.Rows[0]["pat_jy_date"];

                if (pat_jy_date > now)
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = now;
                }
                else
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_jy_date"];
                }
            }

            //if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "HIS代码关联")
            //{
            //    dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(dtPatInfo.Rows[0]["pat_doc_id"].ToString());

            //    if (
            //        (dtPatInfo.Rows[0]["pat_doc_id"] == DBNull.Value || dtPatInfo.Rows[0]["pat_doc_id"] == null)
            //        && dtPatInfo.Rows[0]["pat_doc_name"] != DBNull.Value && dtPatInfo.Rows[0]["pat_doc_name"] != null
            //        )
            //    {
            //        dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(dtPatInfo.Rows[0]["pat_doc_name"].ToString());
            //    }
            //}

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMD(dtPatInfo, result);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }



            try
            {
                DateTime pat_date = (DateTime)dtPatInfo.Rows[0]["pat_date"];
                string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                result.Data.Patient.RepSid = pat_sid;
                result.Data.Patient.PidName = dtPatInfo.Rows[0]["pat_name"].ToString();


                try
                {
                    using (DBHelper helper = DBHelper.BeginTransaction())
                    {
                        //判断是否已回退
                        if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                        {
                            result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                            result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                        }
                        else if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                        {
                            result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            int? host_order = null;

                            if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                            {
                                host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                            }

                            DictInstructmentBLL bllItr = new DictInstructmentBLL();
                            if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 &&
                                PatCommonBll.ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                            {
                                result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                            }
                            else
                            {

                                if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                                {
                                    dtPatInfo.Rows[0]["pat_jy_date"] = ServerDateTime.GetDatabaseServerDateTime();
                                }

                                //插入病人资料
                                foreach (SqlCommand cmd in cmdInsertInfo)
                                {
                                    helper.ExecuteNonQuery(cmd);
                                }

                                //获取插入组合cmd
                                List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, result);

                                //插入组合
                                foreach (SqlCommand cmd in cmdInsertCombine)
                                {
                                    helper.ExecuteNonQuery(cmd);
                                }

                                new PatCommonBll().InsertDefaultResult(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                                       , dtPatCombine
                                                                       , null);


                                if (!string.IsNullOrEmpty(barcode))
                                {
                                    string barcodeRemark = string.Empty;

                                    //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                    //{

                                    //*************************************************************************************
                                    //将序号写入备注中
                                    //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                    //}
                                    if (host_order.HasValue)
                                    {
                                        barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}",
                                                                      dtPatInfo.Rows[0]["itr_name"], pat_sid,
                                                                      host_order.Value, dtPatInfo.Rows[0]["pat_c_name"],
                                                                      dtPatInfo.Rows[0]["pat_date"].ToString());
                                    }
                                    else
                                    {
                                        barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}",
                                                                      dtPatInfo.Rows[0]["itr_name"], pat_sid,
                                                                      dtPatInfo.Rows[0]["pat_c_name"],
                                                                      dtPatInfo.Rows[0]["pat_date"].ToString());
                                    }

                                    //************************************************************************************

                                    new PatCommonBll().UpdateBarcodeStatusForSZ(caller,
                                                                           EnumBarcodeOperationCode.SampleRegister
                                                                                                   .ToString(), barcode,
                                                                           barcodeRemark, null, null,
                                                                           dtPatInfo.Rows[0]["pat_id"].ToString());
                                }

                                if (result.Success)
                                {
                                    helper.Commit();
                                    string pat_ori_id = dtPatInfo.Rows[0]["pat_ori_id"].ToString();
                                    if (pat_ori_id == "107") //门诊
                                    {
                                        if (CacheSysConfig.Current.GetSystemConfig("MZAdivceConfirm").Contains("标本登记"))
                                        {
                                            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                                            ;
                                        }
                                    }
                                    else if (pat_ori_id == "108") //住院
                                    {
                                        if (CacheSysConfig.Current.GetSystemConfig("ZYAdivceConfirm").Contains("标本登记"))
                                        {
                                            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                                        }
                                    }
                                    else if (pat_ori_id == "109") //体检
                                    {
                                        if (CacheSysConfig.Current.GetSystemConfig("TJAdivceConfirm").Contains("标本登记"))
                                        {
                                            new SignInDataInterface(caller, dtPatCombine, dtPatInfo, result).Execute();
                                            ;
                                        }
                                    }

                                }
                                else
                                {
                                    helper.RollBack();
                                }
                            }
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
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatientForSZ", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }

        public EntityOperationResult InsertBarCodePatientWithSignIn(EntityRemoteCallClientInfo caller, DataSet dsData)
        {
            EntityOperationResult result = new EntityOperationResult();//.GetNew("保存条码病人信息");
            result.ReturnResult = dsData;
            DataTable dtPatInfo = dsData.Tables[PatientTable.PatientInfoTableName];
            DataTable dtPatCombine = dsData.Tables[PatientTable.PatientCombineTableName];


            string strSql = string.Format("select pat_id  from patients where pat_itr_id='{0}' and pat_bar_code='{1}' and pat_sid='{2}' ",
                                            dtPatInfo.Rows[0]["pat_itr_id"].ToString(), dtPatInfo.Rows[0]["pat_bar_code"].ToString(), dtPatInfo.Rows[0]["pat_sid"].ToString());

            DBHelper pathelper = new DBHelper();
            DataTable dtPat = pathelper.GetTable(strSql);
            if (dtPat.Rows.Count > 0)
            {
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatientForSZ", "已存在该样本号");
                //result.Success = false;
                return result;
            }




            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            if ((dtPatInfo.Rows[0]["pat_age"] == null || dtPatInfo.Rows[0]["pat_age"].ToString() == "-1")
                && (dtPatInfo.Rows[0]["pat_age_exp"] != null && !string.IsNullOrEmpty(dtPatInfo.Rows[0]["pat_age_exp"].ToString())))
            {
                try
                {
                    dtPatInfo.Rows[0]["pat_age"] = AgeConverter.AgeValueTextToMinute(dtPatInfo.Rows[0]["pat_age_exp"].ToString());

                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", dtPatInfo.Rows[0]["pat_id"].ToString(), dtPatInfo.Rows[0]["pat_age_exp"].ToString()));

                }

            }
            if (dtPatInfo.Rows.Count > 0 && dtPatInfo.Columns.Contains("pat_rem"))//取默认标本状态
            {
                dtPatInfo.Rows[0]["pat_rem"] = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");
            }

            //时间计算方式

            if (dtPatInfo.Rows.Count > 0
                && (dtPatInfo.Rows[0]["pat_apply_date"] == DBNull.Value || dtPatInfo.Rows[0]["pat_apply_date"] == null))
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                if (dtPatInfo.Rows[0]["pat_jy_date"] == null || dtPatInfo.Rows[0]["pat_jy_date"] == DBNull.Value)
                    dtPatInfo.Rows[0]["pat_jy_date"] = now;
                DateTime pat_jy_date = (DateTime)dtPatInfo.Rows[0]["pat_jy_date"];

                if (pat_jy_date > now)
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = now;
                }
                else
                {
                    dtPatInfo.Rows[0]["pat_apply_date"] = dtPatInfo.Rows[0]["pat_jy_date"];
                }
            }


            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "HIS代码关联")
            {
                dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(dtPatInfo.Rows[0]["pat_doc_id"].ToString());

                if (
                    (dtPatInfo.Rows[0]["pat_doc_id"] == DBNull.Value || dtPatInfo.Rows[0]["pat_doc_id"] == null)
                    && dtPatInfo.Rows[0]["pat_doc_name"] != DBNull.Value && dtPatInfo.Rows[0]["pat_doc_name"] != null
                    )
                {
                    dtPatInfo.Rows[0]["pat_doc_id"] = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(dtPatInfo.Rows[0]["pat_doc_name"].ToString());
                }
            }

            UpdatePatCName(dtPatInfo, dtPatCombine);

            List<SqlCommand> cmdInsertInfo = GetPatientInfoInsertCMD(dtPatInfo, result);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(dtPatInfo.Rows[0]["pat_bar_code"]))
            {
                barcode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
            }



            try
            {
                DateTime pat_date = (DateTime)dtPatInfo.Rows[0]["pat_date"];
                string pat_sid = dtPatInfo.Rows[0]["pat_sid"].ToString();
                string pat_itr_id = dtPatInfo.Rows[0]["pat_itr_id"].ToString();

                result.Data.Patient.RepSid = pat_sid;
                result.Data.Patient.PidName = dtPatInfo.Rows[0]["pat_name"].ToString();


                try
                {
                    using (DBHelper helper = DBHelper.BeginTransaction())
                    {
                        //判断是否已回退
                        if (new PatCommonBll().Returned(dtPatInfo.Rows[0]["pat_bar_code"].ToString()))
                        {
                            result.Data.Patient.RepBarCode = dtPatInfo.Rows[0]["pat_bar_code"].ToString();
                            result.AddMessage(EnumOperationErrorCode.HaveReturned, EnumOperationErrorLevel.Error);
                        }
                        else if (PatCommonBll.ExistSID(pat_sid, pat_itr_id, pat_date))
                        {
                            result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            int? host_order = null;

                            if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_host_order"]))
                            {
                                host_order = Convert.ToInt32(dtPatInfo.Rows[0]["pat_host_order"]);
                            }

                            DictInstructmentBLL bllItr = new DictInstructmentBLL();
                            if (host_order != null && bllItr.GetItrHostFlag(pat_itr_id) == 2 &&
                                PatCommonBll.ExistHostOrder(host_order.Value, pat_itr_id, pat_date))
                            {
                                result.AddMessage(EnumOperationErrorCode.HostOrderExist, EnumOperationErrorLevel.Error);
                            }
                            else
                            {

                                if (Compare.IsEmpty(dtPatInfo.Rows[0]["pat_jy_date"]))
                                {
                                    dtPatInfo.Rows[0]["pat_jy_date"] = ServerDateTime.GetDatabaseServerDateTime();
                                }

                                //插入病人资料
                                foreach (SqlCommand cmd in cmdInsertInfo)
                                {
                                    helper.ExecuteNonQuery(cmd);
                                }

                                //获取插入组合cmd
                                List<SqlCommand> cmdInsertCombine = GetCombineInsertCMD(dtPatCombine, barcode, result);

                                //插入组合
                                foreach (SqlCommand cmd in cmdInsertCombine)
                                {
                                    helper.ExecuteNonQuery(cmd);
                                }

                                new PatCommonBll().InsertDefaultResult(dtPatInfo.Rows[0]["pat_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_sam_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_itr_id"].ToString()
                                                                       , dtPatInfo.Rows[0]["pat_sid"].ToString()
                                                                       , dtPatCombine
                                                                       , null);


                                if (!string.IsNullOrEmpty(barcode))
                                {
                                    string barcodeRemark = string.Empty;

                                    //if (!Compare.IsEmpty(dtPatInfo.Rows[0]["pat_c_name"]))
                                    //{

                                    //*************************************************************************************
                                    //将序号写入备注中
                                    //barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2}", dtPatInfo.Rows[0]["itr_name"], pat_sid, dtPatInfo.Rows[0]["pat_c_name"]);
                                    //}
                                    if (host_order.HasValue)
                                    {
                                        barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}",
                                                                      dtPatInfo.Rows[0]["itr_name"], pat_sid,
                                                                      host_order.Value, dtPatInfo.Rows[0]["pat_c_name"],
                                                                      dtPatInfo.Rows[0]["pat_date"].ToString());
                                    }
                                    else
                                    {
                                        barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}",
                                                                      dtPatInfo.Rows[0]["itr_name"], pat_sid,
                                                                      dtPatInfo.Rows[0]["pat_c_name"],
                                                                      dtPatInfo.Rows[0]["pat_date"].ToString());
                                    }

                                    //************************************************************************************

                                    new PatCommonBll().UpdateBarcodeStatusForSZ(caller,
                                                                           EnumBarcodeOperationCode.SampleRegister
                                                                                                   .ToString(), barcode,
                                                                           barcodeRemark, null, null,
                                                                           dtPatInfo.Rows[0]["pat_id"].ToString());
                                }

                                if (result.Success)
                                {
                                    helper.Commit();

                                    new BarcodeDirectConfirmInterface(caller, dtPatCombine, dtPatInfo).Execute();
                                }
                                else
                                {
                                    helper.RollBack();
                                }
                            }
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
                Logger.WriteException(this.GetType().Name, "SaveBarCodePatientForSZ", ex.ToString());
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }

        public bool ResultExistVerify()
        {
            //string strSql = "select * from resulto where 1!=1";
            //DBHelper db = new DBHelper();
            //DataTable dtResult = db.GetTable(strSql);
            return false;// dtResult.Columns.Contains("res_verify");
        }
    }
}
