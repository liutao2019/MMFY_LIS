using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 申请医生字典
    /// 旧表名:Dic_Pub_doctor 新表名:Dict_doctor
    /// </summary>
    [Serializable]
    public class EntityDicDoctor : EntityBase
    {
        public EntityDicDoctor()
        {
            SortNo = 0;
            Checked = false;
        }

        /// <summary>
        /// 医生编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_id", MedName = "doctor_id" ,WFName = "Ddoctor_id")]
        public String DoctorId { get; set; }

        /// <summary>
        /// 所属科室
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_dep_id", MedName = "dept_id", WFName = "Ddoctor_Ddept_id")]
        public String DeptId { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_name", MedName = "doctor_name", WFName = "Ddoctor_name")]
        public String DoctorName { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_incode", MedName = "c_code", WFName = "Ddoctor_c_code")]
        public String CCode { get; set; }

        /// <summary>
        /// 工号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_code", MedName = "doctor_code", WFName = "Ddoctor_code")]
        public String DoctorCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_tel", MedName = "doctor_tel", WFName = "Ddoctor_tel")]
        public String DoctorTel { get; set; }

        /// <summary>
        /// 医院ID（关联医院字典表）
        /// </summary>                       
        [FieldMapAttribute(ClabName = "doc_hospital", MedName = "doctor_hospital", WFName = "Ddoctor_hospital")]
        public String DoctorHospital { get; set; }

        #region 附加字段
        /// <summary>
        /// 医院名称
        /// </summary>                       
        public String DoctorOrgName { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DoctorDeptName { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public string DoctorDeptDelFlag { get; set; }
        #endregion

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return DoctorCode;
            }
        }
    }
}
