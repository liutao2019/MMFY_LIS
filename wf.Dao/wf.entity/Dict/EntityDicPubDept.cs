using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 科室类别
    /// 旧表名:Dic_pub_dept 新表名:Dict_dept
    /// </summary>
    [Serializable]
    public class EntityDicPubDept : EntityBase
    {
        public EntityDicPubDept()
        {
            DeptSortNo = 0;
            Checked = false;
        }

        /// <summary>
        /// 编码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_id", MedName = "dept_id", WFName = "Ddept_id")]
        public string DeptId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_name", MedName = "dept_name", WFName = "Ddept_name")]
        public string DeptName { get; set; }

        /// <summary>
        /// HIS编码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_code", MedName = "dept_code", WFName = "Ddept_code")]
        public string DeptCode { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_incode", MedName = "c_code", WFName = "Ddept_c_code")]
        public string DeptCCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_pswd", MedName = "acces_pssword",WFName = "Ddept_acces_pssword")]
        public string DeptAccesPassword { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_class", MedName = "acces_type",WFName = "Ddept_acces_type")]
        public string DeptAccesType { get; set; }

        /// <summary>
        /// 类型 MZ-门诊 ZY-住院 TJ-体检 WY-外院
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_ori_id", MedName = "dept_source",WFName = "Ddept_Dsorc_id")]
        public string DeptSource { get; set; }

        /// <summary>
        /// 简写
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_aen", MedName = "dept_shortname", WFName = "Dept_shortname")]
        public string DeptShortName { get; set; }

        /// <summary>
        /// 病区
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_ward", MedName = "parent_dept_id",WFName = "Ddept_parent_Ddept_id")]
        public string DeptParentId { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_py", MedName = "py_code", WFName = "py_code")]
        public string DeptPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_wb", MedName = "wb_code", WFName = "wb_code")]
        public string DeptWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_seq", MedName = "sort_no", WFName = "sort_no")]
        public int DeptSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_del", MedName = "del_flag", WFName = "del_flag")]
        public string DeptDelFlag { get; set; }

        /// <summary>
        /// 查询码
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_select_code", MedName = "org_id", WFName = "Ddept_select_code")]
        public string DeptOrgId { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_hospital", MedName = "dept_hospital", WFName = "Ddept_Dorg_id")]
        public string DeptHospital { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_tel", MedName = "dept_tel", WFName = "Ddept_tel")]
        public string DeptTel { get; set; }

        #region 附加码
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 来源名
        /// </summary>
        public string OriName { get; set; }
        #endregion

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion


        #region 附加字段  用户ID
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        #endregion

        #region 附加字段 用户编码
        /// <summary>
        /// 用户编码
        /// </summary>
        public Int32 UserInfoId { get; set; }
        #endregion

        #region 附加字段 用户名称
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserName{ get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return DeptCode;
            }
        }
        #endregion
    }
}
