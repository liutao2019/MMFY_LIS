namespace dcl.client.frame
{
    using System;
    using System.Data;
    using System.Xml;
    using System.Text;
    using System.Collections.Generic;
    using entity;
    using System.Linq;
    using System.ComponentModel.Composition.Hosting;

    public class UserInfo
    {

        /// <summary>
        /// 存放MefContainer
        /// </summary>
        public static CompositionContainer Container { get; set; }
        /// <summary>
        /// 用户默认仪器ID
        /// </summary>
        public static string defaultItr;

        /// <summary>
        /// 用户默认仪器代码
        /// </summary>
        public static string defaultItrName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户默认物理组ID
        /// </summary>
        public static string defaultType;

        /// <summary>
        /// 用户默认物理组名称
        /// </summary>
        public static string defaultTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否跳过权限
        /// </summary>
        private static bool skipPower = false;

        public static bool SkipPower
        {
            get { return UserInfo.skipPower; }
            set { UserInfo.skipPower = value; }
        }

        /// <summary>
        /// 通过密钥
        /// </summary>
        public static bool userKey { get; set; }

        /// <summary>
        /// 用户科室
        /// </summary>
        public static string departName;


        /// <summary>
        /// 用户科室Code hi代码
        /// </summary>
        public static string departCode;


        /// <summary>
        /// 用户科室Id
        /// </summary>
        public static string departId;

        /// <summary>
        /// 科室类型
        /// </summary>
        public static string oriName;

        /// <summary>
        /// 用户可以仪器的ID列表
        /// </summary>
        public static string[] UserItrs { get; set; }

        /// <summary>
        /// 用户可用仪器ID的sql where语句
        /// </summary>
        public static string sqlUserItrs
        {
            get
            {
                if (UserItrs == null)
                {
                    return "-1";
                }

                string sql = string.Empty;

                StringBuilder sb = new StringBuilder();

                foreach (string s in UserItrs)
                {
                    sb.Append(string.Format(",'{0}'", s));
                }

                if (sb.Length > 0)
                {
                    sb = sb.Remove(0, 1);
                }

                //sql = "(" + sb.ToString() + ")";

                return sb.ToString();
            }
        }

        /// <summary>
        /// 用户可用物理组sql子查询语句
        /// </summary>
        public static string sqlUserTypesFilter
        {
            get
            {
                //初始化用户可操作物理组 
                string userTypes = string.Empty;

                if (UserInfo.listUserLab != null)
                {
                    foreach (EntityUserLab lab in UserInfo.listUserLab)
                    {
                        userTypes += string.Format("'{0}',", lab.LabId);
                    }
                }

                if (userTypes != string.Empty)
                {
                    userTypes = userTypes.TrimEnd(',');
                }
                return userTypes;
            }
        }

        public static EntityLoginUserInfo entityUserInfo = new EntityLoginUserInfo();
        public static List<EntityUserLab> listUserLab;
        public static List<EntityUserDept> listUserDepart;
        public static string ip;
        public static bool isAdmin = false;
        public static string loginID;
        public static string loginStatus = "0";
        public static string mac;
        public static string password;
        public static string types;
        public static string userName;
        public static string userInfoId;
        public static bool CASignMode = false;

        public static bool login()
        {
            DataTable table = CommonClient.CreateDT(new string[] { "funcID" }, "func");
            table.Rows.Add(new object[] { "FrmFuncManage.tsbSave" });
            table.Rows.Add(new object[] { "FrmMain.miFrmFuncManage" });
            //dsUserInfo.Tables.Add(table);
            return false;
        }

        /// <summary>
        /// 根据funcInfoId判断是否有权限
        /// </summary>
        /// <param name="funcInfoId"></param>
        /// <returns></returns>
        public static bool HaveFunction(int funcInfoId)
        {
            return HaveFunvtionBase(funcInfoId, string.Empty, string.Empty);
        }

        /// <summary>
        /// 根据funcCode和moduleName判断是否有权限
        /// </summary>
        /// <param name="funcCode"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static bool HaveFunction(string funcCode, string moduleName)
        {
            if (moduleName == "")
            {
                return true;
            }
            else
            {
                return HaveFunvtionBase(-1, funcCode, moduleName);
            }

        }

        /// <summary>
        /// 判断权限基础方法
        /// </summary>
        /// <param name="funcInfoId">功能编码-1则不判断</param>
        /// <param name="funcCode">功能代码 对应窗体的完整路径</param>
        /// <param name="moduleName">子功能名称</param>
        /// <returns></returns>
        private static bool HaveFunvtionBase(int funcInfoId, string funcCode, string moduleName)
        {
            //参数为空则不判断
            if (funcInfoId < -1 && string.IsNullOrEmpty(funcCode) && string.IsNullOrEmpty(moduleName))
                return true;
            if (isAdmin == false && skipPower == false)
            {
                List<EntitySysFunction> drAll = entityUserInfo.AllFunc;
                List<EntitySysFunction> drUser = entityUserInfo.Func;
                if (drAll == null || drUser == null) //突破权限
                    return false;
                if (funcInfoId > -1)
                {
                    drUser = drUser.Where(w => w.FuncId == funcInfoId).ToList();
                    drAll = drAll.Where(w => w.FuncId == funcInfoId ).ToList();
                }
                if (!string.IsNullOrEmpty(funcCode))
                {
                    drUser = drUser.Where(w => w.FuncCode == funcCode).ToList();
                    drAll = drAll.Where(w => w.FuncCode == funcCode).ToList();
                }
                if (!string.IsNullOrEmpty(moduleName))
                {
                    drUser = drUser.Where(w => w.FuncChildName == moduleName).ToList();
                    drAll = drAll.Where(w => w.FuncChildName == moduleName).ToList();
                }
                if (drAll.Count > 0 && (drAll[0].FuncType == "管理员窗体" || drAll[0].FuncType == "管理员功能"))
                {
                    return false;
                }

                if (drAll.Count > 0 && drUser.Count <= 0)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 根据FuncCode判断时候有权限 2011-4-8
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public static bool HaveFunctionByCode(string funcCode)
        {
            if (isAdmin == false && (skipPower == false || funcCode == "PatInput_SaveHistoryResult"))
            {
                List<EntitySysFunction> listUserFunc = entityUserInfo.Func;
                if (listUserFunc != null)
                {
                    List<EntitySysFunction> listUser = listUserFunc.Where(w => w.FuncCode == funcCode).ToList();
                    if (listUser.Count <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool HaveFunctionByCodeForAll(string funcCode)
        {
            List<EntitySysFunction> listUserFunc = entityUserInfo.Func;
            if (listUserFunc != null)
            {
                List<EntitySysFunction> listUser = listUserFunc.Where(w => w.FuncCode == funcCode).ToList();
                if (listUser.Count <= 0)
                {
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// 获得指定系统配置
        /// </summary>
        /// <param name="configId">参数代码</param>
        /// <returns>参数值</returns>
        public static string GetSysConfigValue(string configCode)
        {
            if (entityUserInfo == null)
            {
                return "";
            }

            string configItemValue = "";

            List<EntitySysParameter> listSysConfig = entityUserInfo.SysConfig;

            if (listSysConfig != null && listSysConfig.Count > 0)
            {
                List<EntitySysParameter> SysConfig = listSysConfig.Where(w => w.ParmCode == configCode).ToList();

                if (SysConfig != null && SysConfig.Count > 0)
                {
                    configItemValue = SysConfig[0].ParmFieldValue;
                }
            }

            return configItemValue;
        }

        /// <summary>
        /// 获得指定用户配置
        /// </summary>
        /// <param name="configId">参数代码</param>
        /// <returns>参数值</returns>
        public static string GetUserConfigValue(string configCode)
        {
            if (entityUserInfo == null)
            {
                return "";
            }

            string configItemValue = "";

            List<EntitySysParameter> listSysConfig = entityUserInfo.UserConfig;

            List<EntitySysParameter> SysConfig = listSysConfig.Where(w => w.ParmCode == configCode).ToList();

            if (SysConfig.Count > 0)
            {
                //取用户配置数据时使用了order by configtype,除非自增长已经超过11位数字,否则已经保证了以userInfoId的数字排在'userdefault'之前
                configItemValue = SysConfig[0].ParmFieldValue;
            }

            return configItemValue;
        }

        /// <summary>
        /// 更新系统配置缓存
        /// </summary>
        /// <param name="dt"></param>
        public static void SetSysConfig(List<EntitySysParameter> list)
        {
            List<EntitySysParameter> listSysConfig = entityUserInfo.SysConfig;
            listSysConfig = new List<EntitySysParameter>();
            foreach (EntitySysParameter item in list)
            {
                EntitySysParameter par = new EntitySysParameter(); ;
                par.ParmId = item.ParmId;
                par.ParmCode = item.ParmCode;
                par.ParmGroup = item.ParmGroup;
                par.ParmFieldName = item.ParmFieldName;
                par.ParmFieldType = item.ParmFieldType;
                par.ParmFieldValue = item.ParmFieldValue;
                par.ParmDictList = item.ParmDictList;
                par.ParmType = item.ParmType;
                par.ParmModule = item.ParmModule;
                listSysConfig.Add(par);
            }
        }

        /// <summary>
        /// 更新用户配置缓存
        /// </summary>
        /// <param name="dt"></param>
        public static void SetUserConfig(DataTable dt)
        {
            //DataTable dtSysConfig = dsUserInfo.Tables["userconfig"];
            //dtSysConfig.Rows.Clear();
            //DataRow[] drs = new DataRow[dt.Rows.Count];
            //dt.Rows.CopyTo(drs, 0);
            //foreach (DataRow dr in drs)
            //{
            //    dtSysConfig.Rows.Add(dr.ItemArray);
            //}
        }
        #region  使用过的培养架缓存

        private static Dictionary<string, string> BufferMicCuvShelf = new Dictionary<string, string>();

        //添加键值对，如果键存在则更新值
        public static void AddBufferMicCuvShelf(string key, string value)
        {
            if (BufferMicCuvShelf.Keys.Contains(key))
            {
                BufferMicCuvShelf[key] = value;
                return;
            }
            else
            {
                BufferMicCuvShelf.Add(key, value);
            }
        }

        public static string GetBufferMicCuvShelf(string key)
        {
            if (BufferMicCuvShelf.Keys.Contains(key))
            {
                return BufferMicCuvShelf[key];
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}

