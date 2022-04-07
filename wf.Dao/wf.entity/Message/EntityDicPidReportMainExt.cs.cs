using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人信息扩展表:med库:Pid_report_main_ext;clab库:patients_ext
    /// </summary>
    [Serializable]
    public class EntityDicPidReportMainExt : EntityBase
    {
        public EntityDicPidReportMainExt()
        {
        }
        /// <summary>
        ///病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id",WFName = "rep_id")]
        public String RepId { get; set; }

        /// <summary>
        ///（资源内容）
        /// </summary>   
        [FieldMapAttribute(ClabName = "SourceContent", MedName = "SourceContent", WFName = "SourceContent")]
        public String SourceContent { get; set; }

        /// <summary>
        ///（标志内容）
        /// </summary>   
        [FieldMapAttribute(ClabName = "SignContent", MedName = "SignContent", WFName = "SignContent")]
        public String SignContent { get; set; }

        /// <summary>
        ///（暂时未知）
        /// </summary>   
        [FieldMapAttribute(ClabName = "SignerImage", MedName = "SignerImage", WFName = "SignerImage")]
        public Byte[] SignerImage { get; set; }

        /// <summary>
        ///信息内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_content", MedName = "msg_content", WFName = "msg_content")]
        public String MsgContent { get; set; }

        /// <summary>
        ///检查者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_checker_id", MedName = "msg_checker_id", WFName = "msg_checker_id")]
        public String MsgCheckerId { get; set; }

        /// <summary>
        ///检查这姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_checker_name", MedName = "msg_checker_name", WFName = "msg_checker_name")]
        public String MsgCheckerName { get; set; }

        /// <summary>
        ///（签名类型）
        /// </summary>   
        [FieldMapAttribute(ClabName = "SignType", MedName = "SignType", WFName = "SignType")]
        public String SignType { get; set; }

        /// <summary>
        ///（通知日期）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_inform_date", MedName = "pat_inform_date", WFName = "pat_inform_date")]
        public DateTime? PatInformDate { get; set; }

        /// <summary>
        ///临床工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_doc_num", MedName = "msg_doc_num", WFName = "msg_doc_num")]
        public String MsgDocNum { get; set; }

        /// <summary>
        ///临床名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_doc_name", MedName = "msg_doc_name", WFName = "msg_doc_name")]
        public String MsgDocName { get; set; }

        /// <summary>
        ///临床电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_dep_tel", MedName = "msg_dep_tel", WFName = "msg_dep_tel")]
        public String MsgDepTel { get; set; }

        /// <summary>
        ///记录时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_date", MedName = "msg_date", WFName = "msg_date")]
        public DateTime MsgDate { get; set; }

        /// <summary>
        ///(暂时未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_cure_flag", MedName = "pat_cure_flag", WFName = "pat_cure_flag")]
        public Boolean PatCureFlag { get; set; }

        /// <summary>
        ///(暂时未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_release_flag", MedName = "pat_release_flag", WFName = "pat_release_flag")]
        public Boolean PatReleaseFlag { get; set; }

        /// <summary>
        ///出生日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pid_birthday", MedName = "pid_birthday", WFName = "pid_birthday")]
        public DateTime? PidBirthday { get; set; }

        /// <summary>
        ///(暂时未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "SourceTimestamp", MedName = "SourceTimestamp", WFName = "SourceTimestamp")]
        public String SourceTimestamp { get; set; }

        /// <summary>
        ///(暂时未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "SignTimestamp", MedName = "SignTimestamp", WFName = "SignTimestamp")]
        public String SignTimestamp { get; set; }

        /// <summary>
        ///登记者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_register_loginId", MedName = "msg_register_loginId", WFName = "msg_register_loginId")]
        public String MsgRegisterLoginId { get; set; }

        /// <summary>
        ///登记者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_register_userName", MedName = "msg_register_userName", WFName = "msg_register_userName")]
        public String MsgRegisterUserName { get; set; }

        /// <summary>
        ///(未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_lmp_weeks", MedName = "pat_lmp_weeks", WFName = "pat_lmp_weeks")]
        public String PatLmpWeeks { get; set; }

        /// <summary>
        ///信息内容2
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_content2", MedName = "msg_content2", WFName = "msg_content2")]
        public String MsgContent2 { get; set; }

        /// <summary>
        ///(暂时未知)
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_againlog_id", MedName = "msg_againlog_id", WFName = "msg_againlog_id")]
        public String MsgAgainlogId { get; set; }

        /// <summary>
        ///危急值内部提示处理人
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_insgin_id", MedName = "msg_insgin_id", WFName = "msg_insgin_id")]
        public String MsgInsginId { get; set; }

        /// <summary>
        ///危急值内部提示处理时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_insgin_date", MedName = "msg_insgin_date", WFName = "msg_insgin_date")]
        public DateTime? MsgInsginDate { get; set; }


        /// <summary>
        ///病人民族
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_race", MedName = "pat_race", WFName = "pat_race")]
        public string PatRace { get; set; }


        /// <summary>
        ///胎数
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_birth_number", MedName = "pat_birth_number", WFName = "pat_birth_number")]
        public string PatBirthNumber { get; set; }

        /// <summary>
        ///通知时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "", WFName = "pat_notice_date")]
        public DateTime? PatNoticeDate { get; set; }

        /// <summary>
        ///糖尿病史
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_diabetes_mellitus", MedName = "", WFName = "")]
        public string PatDiabetesMellitus { get; set; }

        
        /// <summary>
        ///吸烟史
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_smoke", MedName = "", WFName = "")]
        public string PatSmoke { get; set; }

        
        /// <summary>
        ///末次月经
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_last_menstrual_period", MedName = "", WFName = "")]
        public string PatLastMenstrualPeriod { get; set; }

        
        /// <summary>
        ///B超日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_BCRQ", MedName = "", WFName = "")]
        public string PatBCRQ { get; set; }


        /// <summary>
        ///预产期母龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_YCQML", MedName = "", WFName = "")]
        public string PatYCQML { get; set; }

        
        /// <summary>
        ///末次月经孕周
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_lmp_weeks", MedName = "", WFName = "")]
        public string PatlmpWeeks { get; set; }
    }
}
