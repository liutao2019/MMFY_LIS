using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.common
{
    public class LocalSetting
    {
        #region Instance
        private static LocalSetting _instance = null;

        private static object instanceLock = new object();

        public static LocalSetting Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LocalSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        private EntityLocalSetting currentSetting = null;

        public EntityLocalSetting Setting
        {
            get
            {
                return currentSetting;
            }
        }

        private LocalSetting()
        {
            Refresh();
        }

        public void Refresh()
        {
            currentSetting = LoadFromXML();

            if (currentSetting == null)
            {
                currentSetting = new EntityLocalSetting();
            }


            currentSetting.AuditWord = UserInfo.GetSysConfigValue("AuditWord");

            if (string.IsNullOrEmpty(currentSetting.AuditWord))
            {
                currentSetting.AuditWord = "审核";
            }


            currentSetting.ReportWord = UserInfo.GetSysConfigValue("ReportWord");

            if (string.IsNullOrEmpty(currentSetting.ReportWord))
            {
                currentSetting.ReportWord = "报告";
            }
        }


        private EntityLocalSetting LoadFromXML()
        {
            EntityLocalSetting setting = null;
            if (File.Exists(XMLPath + "\\" + XMLFileName))
            {
                DataSet data = new DataSet();
                data.ReadXml(XMLPath + "\\" + XMLFileName);

                if (data != null && data.Tables.Count > 0)
                {
                    setting = this.FromDataTable(data.Tables[0]);
                }
            }
            return setting;
        }

        private DataTable ToDataTable(EntityLocalSetting setting)
        {
            DataTable table = new DataTable();
            table.Columns.Add("CType_id");
            table.Columns.Add("CType_name");
            table.Columns.Add("Itr_id");
            table.Columns.Add("LocalItrID");
            table.Columns.Add("Barcode_Dep_id");
            table.Columns.Add("Barcode_Dep_name");
            table.Columns.Add("Description");
            table.Columns.Add("HospitalID");
            table.Columns.Add("HospitalName");
            table.Columns.Add("IsUrgentNotity");
            table.Columns.Add("IsItrUrgentNotity");
            table.Columns.Add("IsQCNotify");
            table.Columns.Add("IsTATNotify");
            table.Columns.Add("IsBcTATNotify");
            table.Columns.Add("ItrIDList");//仪器ID集合
            table.Columns.Add("TypeIDList");//物理组ID集合
            table.Columns.Add("IDTypeFlag");
            table.Columns.Add("QsTypeIDList");//物理组ID集合(签收地点)
            table.Columns.Add("SdTypeIDList");//物理组ID集合(送达地点)
            table.Columns.Add("LabResultShowType");//物理组ID集合(送达地点)
            table.Columns.Add("BloodWindow");//采血窗口
            table.Columns.Add("BloodArea");//采血区域
            table.Columns.Add("MzDefaultCombines");//门诊默认组合
            table.Columns.Add("CachePwTime");//审核密码缓存时间

            DataRow dr = table.NewRow();
            dr["CType_id"] = setting.CType_id;
            dr["CType_name"] = setting.CType_name;
            dr["Itr_id"] = setting.Itr_id;
            dr["LocalItrID"] = setting.LocalItrID;
            dr["Barcode_Dep_id"] = setting.Barcode_Dep_id;
            dr["Barcode_Dep_name"] = setting.Barcode_Dep_name;
            dr["Description"] = setting.Description;
            dr["HospitalID"] = setting.HospitalID;
            dr["HospitalName"] = setting.HospitalName;
            dr["IsUrgentNotity"] = string.IsNullOrEmpty(setting.IsUrgentNotity) ? "1" : setting.IsUrgentNotity;
            dr["IsItrUrgentNotity"] = string.IsNullOrEmpty(setting.IsItrUrgentNotity) ? "0" : setting.IsItrUrgentNotity;
            dr["IsQCNotify"] = string.IsNullOrEmpty(setting.IsQCNotify) ? "0" : setting.IsQCNotify;
            dr["IsTATNotify"] = string.IsNullOrEmpty(setting.IsTATNotify) ? "0" : setting.IsTATNotify;
            dr["IsBcTATNotify"] = string.IsNullOrEmpty(setting.IsBcTATNotify) ? "0" : setting.IsBcTATNotify;
            dr["ItrIDList"] = setting.ItrIDList;
            dr["TypeIDList"] = setting.TypeIDList;
            dr["IDTypeFlag"] = setting.IDTypeFlag;
            dr["QsTypeIDList"] = setting.QsTypeIDList;
            dr["SdTypeIDList"] = setting.SdTypeIDList;
            dr["LabResultShowType"] = setting.LabResultShowType;
            dr["BloodWindow"] = setting.BloodWindow;
            dr["BloodArea"] = setting.BloodArea;
            dr["MzDefaultCombines"] = setting.MzDefaultSam;
            dr["CachePwTime"] = setting.CachePwTime;
            table.Rows.Add(dr);

            return table;
        }

        private EntityLocalSetting FromDataTable(DataTable table)
        {
            EntityLocalSetting entity = null;

            if (table != null && table.Rows.Count > 0)
            {
                entity = new EntityLocalSetting();

                DataRow row = table.Rows[0];

                if (table.Columns.Contains("CType_id"))
                {
                    entity.CType_id = row["CType_id"].ToString();
                }

                if (table.Columns.Contains("CType_name"))
                {
                    entity.CType_name = row["CType_name"].ToString();
                }

                if (table.Columns.Contains("Itr_id"))
                {
                    entity.Itr_id = row["Itr_id"].ToString();
                }

                if (table.Columns.Contains("LocalItrID"))
                {
                    entity.LocalItrID = row["LocalItrID"].ToString();
                }

                if (table.Columns.Contains("Barcode_Dep_id"))
                {
                    entity.Barcode_Dep_id = row["Barcode_Dep_id"].ToString();
                }

                if (table.Columns.Contains("Barcode_Dep_name"))
                {
                    entity.Barcode_Dep_name = row["Barcode_Dep_name"].ToString();
                }

                if (table.Columns.Contains("Description"))
                {
                    entity.Description = row["Description"].ToString();
                }

                if (table.Columns.Contains("HospitalID"))
                {
                    entity.HospitalID = row["HospitalID"].ToString();
                }

                if (table.Columns.Contains("HospitalName"))
                {
                    entity.HospitalName = row["HospitalName"].ToString();
                }

                if (table.Columns.Contains("IsUrgentNotity"))
                {
                    entity.IsUrgentNotity = row["IsUrgentNotity"].ToString();

                    if (string.IsNullOrEmpty(entity.IsUrgentNotity))
                        entity.IsUrgentNotity = "1";//空时,默认为0
                }

                if (table.Columns.Contains("IsItrUrgentNotity"))
                {
                    entity.IsItrUrgentNotity = row["IsItrUrgentNotity"].ToString();

                    if (string.IsNullOrEmpty(entity.IsItrUrgentNotity))
                        entity.IsItrUrgentNotity = "0";//空时,默认为0
                }

                if (table.Columns.Contains("IsQCNotify"))
                {
                    entity.IsQCNotify = row["IsQCNotify"].ToString();

                    if (string.IsNullOrEmpty(entity.IsQCNotify))
                        entity.IsQCNotify = "0";//空时,默认为0
                }

                if (table.Columns.Contains("IsTATNotify"))
                {
                    entity.IsTATNotify = row["IsTATNotify"].ToString();

                    if (string.IsNullOrEmpty(entity.IsTATNotify))
                        entity.IsTATNotify = "0";//空时,默认为0
                }

                if (table.Columns.Contains("IsBcTATNotify"))
                {
                    entity.IsBcTATNotify = row["IsBcTATNotify"].ToString();

                    if (string.IsNullOrEmpty(entity.IsBcTATNotify))
                        entity.IsBcTATNotify = "0";//空时,默认为0
                }

                if (table.Columns.Contains("ItrIDList"))
                {
                    entity.ItrIDList = row["ItrIDList"].ToString();
                }

                if (table.Columns.Contains("TypeIDList"))
                {
                    entity.TypeIDList = row["TypeIDList"].ToString();
                }
                if (table.Columns.Contains("IDTypeFlag"))
                {
                    entity.IDTypeFlag = row["IDTypeFlag"].ToString();
                }

                if (table.Columns.Contains("QsTypeIDList"))
                {
                    entity.QsTypeIDList = row["QsTypeIDList"].ToString();
                }

                if (table.Columns.Contains("SdTypeIDList"))
                {
                    entity.SdTypeIDList = row["SdTypeIDList"].ToString();
                }

                if (table.Columns.Contains("LabResultShowType"))
                {
                    entity.LabResultShowType = row["LabResultShowType"].ToString();
                }
                if (table.Columns.Contains("BloodWindow"))
                {
                    entity.BloodWindow = row["BloodWindow"].ToString();
                }

                if (table.Columns.Contains("BloodArea"))
                {
                    entity.BloodArea = row["BloodArea"].ToString();
                }

                if(table.Columns.Contains("MzDefaultCombines"))
                {
                    entity.MzDefaultSam = row["MzDefaultCombines"].ToString();
                }

                if (table.Columns.Contains("CachePwTime"))
                {
                    entity.CachePwTime = row["CachePwTime"].ToString();
                }
            }
            return entity;
        }

        /// <summary>
        /// 配置文件地址
        /// </summary>
        private static string XMLPath
        {
            get
            {
                // return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis";
                return PathManager.SettingLisPath;
            }
        }

        private static string XMLFileName
        {
            get
            {
                return "clientsetting.xml";
            }
        }

        public void Save()
        {
            DataSet data = new DataSet();

            try
            {
                if (!Directory.Exists(XMLPath))
                {
                    Directory.CreateDirectory(XMLPath);
                }

                if (this.currentSetting == null)
                {
                    this.currentSetting = new EntityLocalSetting();
                }

                DataTable table = ToDataTable(this.currentSetting);
                data.Tables.Add(table);

                data.WriteXml(XMLPath + "\\" + XMLFileName, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
