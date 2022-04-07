using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using System.Reflection;
using dcl.root.dac;

namespace dcl.svr.result
{
    /// <summary>
    /// 院网接口固定表
    /// </summary>
    public class InterfaceDataStruct
    {
        private static InterfaceDataStruct _instance = null;
        public static InterfaceDataStruct Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InterfaceDataStruct();
                }
                return _instance;
            }
        }

        private DataTable fields;

        private DataTable dtPatInfo;
        
        private InterfaceDataStruct()
        {
            Init();
        }

        private void Init()
        {
            DBHelper helper = new DBHelper();
            fields = helper.GetTable("select * from dict_contrast where con_type =1");

            GenTable_PatInfo();
        }

        private void GenTable_PatInfo()
        {
            string tbName = "PatInfo";
            DataRow[] drs = fields.Select(string.Format("con_tablename='{0}'",tbName));

            dtPatInfo = new DataTable(tbName);

            foreach (DataRow drField in drs)
            {
                string datatype = drField["con_datatype"].ToString();
                string filedName = drField["con_lis_columns"].ToString();
                dtPatInfo.Columns.Add(filedName,Type.GetType(datatype));
            }
        }

        public DataTable GetStruct_Patients()
        {
            return dtPatInfo.Clone();
        }
    }
}
