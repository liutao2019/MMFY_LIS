using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.cache
{
    public class DictDoctorCache
    {
        #region 旧代码
        private static DictDoctorCache _instance = null;
        private static object padlock = new object();

        public static DictDoctorCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictDoctorCache();
                        }
                    }
                }
                return _instance;
            }
        }

        DataTable cache = null;
        public List<EntityDicDoctor> Dclcache = new List<EntityDicDoctor>();

        private DictDoctorCache()
        {
            Refresh();
        }

        public void Refresh()
        {
            this.cache = GetAllDoctors();
        }

        /// <summary>
        /// 获取所有医生
        /// </summary>
        /// <returns></returns>
        private DataTable GetAllDoctors()
        {
            //string sql = string.Format(@"select ROW_NUMBER() OVER(ORDER BY doc_seq DESC) doc_number,doc_id, doc_dep_id, doc_name, doc_code, doc_incode,
            //                doc_py, doc_wb, doc_del, doc_seq, doc_tel,dict_depart.dep_name,dict_depart.dep_CODE 
            //                      from dict_doctor 
            //                      LEFT OUTER JOIN dict_depart ON dict_doctor.doc_dep_id = dict_depart.dep_id 
            //                      where (doc_del ='0' or doc_del is null) {0}
            //                      order by doc_seq asc", ServerCacheConfig.BuildHospitalSqlWhere("dict_doctor.doc_hospital"));
            //DataTable table = new SqlHelper().GetTable(sql, "dict_doctor");


            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntityDicDoctor");
            Dclcache = response.GetResult<List<EntityDicDoctor>>();

            DataTable table = new DataTable();

            table.Columns.Add("doc_number");
            table.Columns.Add("doc_id");
            table.Columns.Add("doc_dep_id");
            table.Columns.Add("doc_name");
            table.Columns.Add("doc_code");
            table.Columns.Add("doc_incode");
            table.Columns.Add("doc_py");
            table.Columns.Add("doc_wb");
            table.Columns.Add("doc_del");
            table.Columns.Add("doc_seq");
            table.Columns.Add("doc_tel");
            table.Columns.Add("dep_name");
            table.Columns.Add("dep_code");


            int i = 0;
            if (Dclcache != null)
            {
                foreach (EntityDicDoctor item in Dclcache)
                {
                    DataRow row = table.NewRow();
                    row["doc_number"] = i;

                    row["doc_id"] = item.DoctorId;
                    row["doc_dep_id"] = item.DeptId;
                    row["doc_name"] = item.DoctorName;
                    row["doc_code"] = item.DoctorCode;
                    row["doc_incode"] = item.CCode;
                    row["doc_py"] = item.PyCode;
                    row["doc_wb"] = item.WbCode;
                    row["doc_del"] = item.DelFlag;
                    row["doc_seq"] = item.SortNo;
                    row["doc_tel"] = item.DoctorTel;
                    row["dep_name"] = item.DoctorDeptName;
                    row["dep_code"] = item.DeptId;

                    table.Rows.Add(row);
                    i++;
                }
            }


            return table;
        }

        public DataTable GetDoctors()
        {
            return this.cache.Copy(); ;
        }

        public string GetDocIDByName(string doc_name)
        {
            DataRow row = GetDocDataByName(doc_name);
            if (row != null)
            {
                return row["doc_id"].ToString();
            }
            return null;
        }

        public string GetDocCodeByName(string doc_name)
        {
            DataRow row = GetDocDataByName(doc_name);
            if (row != null)
            {
                return row["doc_code"].ToString();
            }
            return null;
        }

        /// <summary>
        /// 根据医生名称与(ID或code)获取code
        /// </summary>
        /// <param name="doc_name"></param>
        /// <param name="IDorCode"></param>
        /// <returns></returns>
        public string GetDocCodeByNameAndIDorCode(string doc_name, string IDorCode)
        {
            DataRow[] rows = this.cache.Select(string.Format("doc_name = '{0}' and (doc_id='{1}' or doc_code='{1}')", doc_name, IDorCode));
            if (rows != null && rows.Length > 0)
            {
                return rows[0]["doc_code"].ToString();
            }
            return null;
        }

        public string GetDocIDByCode(string doc_code)
        {
            DataRow row = GetDocDataByCode(doc_code);
            if (row != null)
            {
                return row["doc_id"].ToString();
            }
            return null;
        }

        private DataRow GetDocDataByName(string doc_name)
        {
            DataRow[] rows = this.cache.Select(string.Format("doc_name = '{0}'", doc_name));
            if (rows != null && rows.Length > 0)
            {
                return rows[0];
            }
            return null;
        }

        private DataRow GetDocDataByCode(string doc_code)
        {
            DataRow[] rows = this.cache.Select(string.Format("doc_code = '{0}'", doc_code));
            if (rows != null && rows.Length > 0)
            {
                return rows[0];
            }
            return null;
        }

        public string GetDocTelByID(string doc_id)
        {
            DataRow[] rows = this.cache.Select(string.Format("doc_id = '{0}'", doc_id));
            if (rows.Length > 0 && rows[0]["doc_tel"] != null && rows[0]["doc_tel"] != DBNull.Value)
            {
                return rows[0]["doc_tel"].ToString();
            }

            rows = cache.Select(string.Format("doc_code = '{0}'", doc_id));
            if (rows.Length > 0 && rows[0]["doc_tel"] != null && rows[0]["doc_tel"] != DBNull.Value)
            {
                return rows[0]["doc_tel"].ToString();
            }

            return string.Empty;
        }
        #endregion
    }
}
