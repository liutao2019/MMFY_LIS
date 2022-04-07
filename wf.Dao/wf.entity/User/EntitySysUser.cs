using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统用户表
    /// 旧表名:sys_power_user 新表名:Base_user
    /// </summary>
    [Serializable]
    public class EntitySysUser : EntityBase
    {
        public EntitySysUser()
        {
            Checked = false;
        }
        /// <summary>
        ///用户编码  业务主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///用户名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name")]
        public String UserName { get; set; }

        /// <summary>
        ///登录账号
        /// </summary>   
        [FieldMapAttribute(ClabName = "loginId", MedName = "user_loginid", WFName = "Buser_loginid")]
        public String UserLoginid { get; set; }

        /// <summary>
        ///登录密码
        /// </summary>   
        [FieldMapAttribute(ClabName = "password", MedName = "user_password", WFName = "Buser_password")]
        public String UserPassword { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_id", MedName = "itr_id", WFName = "Buser_Ditr_id")]
        public String ItrId { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "seq", MedName = "sort_no", WFName = "sort_no")]
        public String SortNo { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        ///输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "incode", MedName = "user_incode", WFName = "Buser_incode")]
        public String UserIncode { get; set; }

        /// <summary>
        ///来源ID(目前未使用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "source_id", MedName = "user_source_id", WFName = "Buser_source_id")]
        public String UserSourceId { get; set; }

        /// <summary>
        ///默认物理组
        /// </summary>   
        [FieldMapAttribute(ClabName = "default_type", MedName = "user_default_lab_id", WFName = "Buser_default_lab_id")]
        public String UserDefaultLabId { get; set; }

        /// <summary>
        ///所属科室
        /// </summary>   
        [FieldMapAttribute(ClabName = "depart_id", MedName = "user_depart_id", WFName = "Buser_Ddept_id")]
        public String UserDepartId { get; set; }

        /// <summary>
        ///用户类型（检验组/条码组）
        /// </summary>   
        [FieldMapAttribute(ClabName = "userType", MedName = "user_type", WFName = "Buser_type")]
        public String UserType { get; set; }

        /// <summary>
        ///图片
        /// </summary>   
        [FieldMapAttribute(ClabName = "signImage", MedName = "user_signinamge", WFName = "Buser_image")]
        public Byte[] UserSigninamge { get; set; }

        /// <summary>
        ///图片路径
        /// </summary>   
        [FieldMapAttribute(ClabName = "signFileName", MedName = "user_signname", WFName = "Buser_image_file")]
        public String UserSignname { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "cerid", MedName = "user_cerid", WFName = "Buser_cerid")]
        public String UserCerid { get; set; }

        /// <summary>
        ///CA唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_entity_id", MedName = "ca_entity_id", WFName = "Buser_caentity_id")]
        public String CaEntityId { get; set; }

        /// <summary>
        ///CA标志模式
        /// </summary>   
        [FieldMapAttribute(ClabName = "CASignMode", MedName = "user_ca_flag", WFName = "Buser_ca_mode")]
        public Boolean UserCaFlag { get; set; }

        /// <summary>
        ///所属医院
        /// </summary>   
        [FieldMapAttribute(ClabName = "user_hos_id", MedName = "user_org_id", WFName = "Buser_Dorg_id")]
        public String UserOrgId { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        [FieldMapAttribute(ClabName = "identity", MedName = "identity", WFName = "Buser_identity")]
        public string Identity { get; set; }

        #region 附加字段 用户登录账号
        /// <summary>
        /// 用户登录账号
        /// </summary>
        public String LoginId { get; set; }
        #endregion

        #region 附加字段 用户旧密码
        /// <summary>
        /// 用户旧密码
        /// </summary>
        public String OldPassword { get; set; }
        #endregion

        #region 附加字段 用户权限主键
        /// <summary>
        /// 用户权限主键
        /// </summary>
        public Int32 PowerUserKey
        {
            get
            {
                if (string.IsNullOrEmpty(LoginId))
                    return 1;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return LoginId;
            }
        }
        #endregion

        #region 附加字段 用户ID
        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldMapAttribute(ClabName = "userId", MedName = "userId", WFName = "userId", DBColumn = false)]
        public String UserIDMess { get; set; }
        #endregion

        #region 附加字段 组别编码
        /// <summary>
        /// 组别编码
        /// </summary>
        [FieldMapAttribute(ClabName = "type_id", MedName = "pro_id", WFName = "Dpro_id", DBColumn = false)]
        public String ProID { get; set; }
        #endregion

        #region 附加字段 组别名称
        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段  类别来源ID
        /// <summary>
        ///类别来源ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "typeSourceId", MedName = "user_org_id", WFName = "Buser_Dorg_id", DBColumn = false)]
        public String TypeSourceId { get; set; }
        #endregion

        #region 附加字段 默认实验组名称
        /// <summary>
        ///默认实验组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "defaule_type_name", MedName = "defaule_type_name", WFName = "defaule_pro_name", DBColumn = false)]
        public String DefaultLabName { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "itr_mid", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段 所属科室名称
        /// <summary>
        ///所属科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "depart_name", MedName = "user_depart_name", WFName = "user_depart_name", DBColumn = false)]
        public String UserDepartName { get; set; }

        [FieldMapAttribute(ClabName = "dep_code", MedName = "dept_code", WFName = "Ddept_code", DBColumn = false)]
        public string UserDepartCode { get; set; }
        #endregion

        #region 附加字段 所属医院名称
        /// <summary>
        ///所属医院名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ori_name", MedName = "user_org_name", WFName = "user_org_name", DBColumn = false)]
        public String UserOrgName { get; set; }
        #endregion

        #region 附加字段 模块名称
        /// <summary>
        /// 模块名称
        /// </summary>
        [FieldMapAttribute(ClabName = "funcName", MedName = "func_name", WFName = "Bfunc_name", DBColumn = false)]
        public String FuncName { get; set; }
        #endregion

        #region 附加字段 模块代码
        /// <summary>
        /// 模块代码
        /// </summary>
        [FieldMapAttribute(ClabName = "funcCode", MedName = "func_code", WFName = "Bfunc_code", DBColumn = false)]
        public String FuncCode { get; set; }
        #endregion
    }
}
