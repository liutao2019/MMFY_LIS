using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 危急值消息表：msg_content(clab库):obr_message_content(med库)
    /// 旧表名:obr_message_content 新表名:Lis_message_content
    /// </summary>
    [Serializable]
    public class EntityDicObrMessageContent : EntityBase
    {
        public EntityDicObrMessageContent()
        {
            ListObrMessageReceiver = new ObrMessageReceiveCollection();
        }

        /// <summary>
        /// 信息ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_id", MedName = "obr_id", WFName = "Lmsg_id")]
        public String ObrId { get; set; }

        /// <summary>
        /// 危机值消息类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_type", MedName = "obr_type", WFName = "Lmsg_type")]
        public Int32 ObrType { get; set; }

        /// <summary>
        /// 消息发送者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_sender_id", MedName = "obr_send_user_id", WFName = "Lmsg_send_user_id")]
        public String ObrSendUserId { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_sender_name", MedName = "obr_send_user_name", WFName = "Lmsg_send_user_name")]
        public String ObrSendUserName { get; set; }

        /// <summary>
        /// 发送端IP
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_sender_ip", MedName = "obr_send_server", WFName = "Lmsg_send_server")]
        public String ObrSendServer { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_create_time", MedName = "obr_create_time", WFName = "Lmsg_create_time")]
        public DateTime ObrCreateTime { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_title", MedName = "obr_message_title", WFName = "Lmsg_title")]
        public String ObrMessageTitle { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_receiver", MedName = "obr_receive", WFName = "Lmsg_receive")]
        public String ObrReceive { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_content", MedName = "obr_content", WFName = "Lmsg_content")]
        public String ObrContent { get; set; }

        /// <summary>
        /// 父消息ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_parent_msgid", MedName = "obr_parent_id", WFName = "Lmsg_parent_Lmsg_id")]
        public String ObrParentId { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_del_flag", MedName = "del_flag", WFName = "del_flag")]
        public Boolean DelFlag { get; set; }

        /// <summary>
        /// 有效日期，为空则不限制
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_expiretime", MedName = "obr_expiration_date", WFName = "Lmsg_expiration_date")]
        public DateTime? ObrExpirationDate { get; set; }

        /// <summary>
        /// 扩展字段A
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_ext1", MedName = "obr_value_a", WFName = "Lmsg_extend_a")]
        public String ObrValueA { get; set; }

        /// <summary>
        /// 扩展字段B
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_ext2", MedName = "obr_value_b", WFName = "Lmsg_extend_b")]
        public String ObrValueB { get; set; }

        /// <summary>
        /// 扩展字段C:危急消息
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_ext3", MedName = "obr_value_c", WFName = "Lmsg_extend_c")]
        public String ObrValueC { get; set; }

        /// <summary>
        /// 扩展字段D
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_ext4", MedName = "obr_value_d", WFName = "Lmsg_extend_d")]
        public String ObrValueD { get; set; }

        /// <summary>
        /// 查看者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_checker_id", MedName = "obr_audit_user_id", WFName = "Lmsg_audit_user_id")]
        public String ObrAuditUserId { get; set; }

        /// <summary>
        /// 查看者
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_checker_name", MedName = "obr_audit_user_name", WFName = "Lmsg_audit_user_name")]
        public String ObrAuditUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_affirm_type", MedName = "obr_confirm_type", WFName = "Lmsg_confirm_type")]
        public String ObrConfirmType { get; set; }

        /// <summary>
        /// 病人名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "Lmsg_pid_name")]
        public String PidName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sex", MedName = "pid_sex", WFName = "Lmsg_pid_sex")]
        public String PidSex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age_str", MedName = "pid_age", WFName = "Lmsg_pid_age")]
        public String PidAge { get; set; }

        /// <summary>
        /// 床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_bed_no", MedName = "pid_bed_no", WFName = "Lmsg_pid_bed_no")]
        public String PidBedNo { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "Lmsg_pid_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        /// 病人来源
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ori_id", MedName = "pid_src_id", WFName = "Lmsg_pid_Dsorc_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        /// 医生代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_doc_code", MedName = "obr_doctor_code", WFName = "Lmsg_doctor_code")]
        public String ObrDoctorCode { get; set; }

        /// <summary>
        /// 开单医生
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_doc_name", MedName = "obr_doctor_name", WFName = "Lmsg_doctor_name")]
        public String ObrDoctorName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_pat_tel", MedName = "pid_tel", WFName = "Lmsg_tel")]
        public String PidTel { get; set; }

        /// <summary>
        /// 信息发送部门编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_send_depcode", MedName = "obr_send_dept_code", WFName = "Lmsg_send_dept_code")]
        public String ObrSendDeptCode { get; set; }

        /// <summary>
        /// 信号标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "msf_insgin_flag", MedName = "obr_inside_flag", WFName = "Lmsg_inside_flag")]
        public String ObrInsideFlag { get; set; }

        /// <summary>
        /// 护士确认标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_nurse_read_flag", MedName = "obr_nurse_read_flag", WFName = "Lmsg_nurse_read_flag")]
        public String ObrNurseReadFlag { get; set; }

        /// <summary>
        /// 医生确认标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_doctor_read_flag", MedName = "obr_doctor_read_flag", WFName = "Lmsg_doctor_read_flag")]
        public String ObrDoctorReadFlag { get; set; }

        #region 附加字段  信息查看状态
        /// <summary>
        /// 信息查看状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_status", MedName = "msg_status", WFName = "msg_status", DBColumn = false)]
        public String MsgStatus { get; set; }
        #endregion

        #region   附加字段 信号标志显示字符
        /// <summary>
        /// 信号标志字符串
        /// </summary>
        [FieldMapAttribute(ClabName = "msf_insgin_flag_str", MedName = "obr_inside_flag_str", WFName = "obr_inside_flag_str", DBColumn = false)]
        public String ObrInsideFlagStr { get; set; }
        #endregion

        #region  附加字段 性别显示字符
        /// <summary>
        /// 性别显示字符
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sex_str", MedName = "pid_sex_str", WFName = "pat_sex_str", DBColumn = false)]
        public String PidSexStr { get; set; }
        #endregion

        #region  附加字段 病人来源名称
        /// <summary>
        /// 病人来源名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name", DBColumn = false)]
        public String SrcName { get; set; }
        #endregion

        #region  附加字段 危急值处理ID
        /// <summary>
        /// 危急值处理ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_id", MedName = "obr_user_id", WFName = "Lmsgrec_user_id", DBColumn = false)]
        public String ObrUserId { get; set; }
        #endregion

        #region  附加字段  确认时间
        /// <summary>
        /// 确认时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_date_look", MedName = "obr_date_look", WFName = "obr_date_look", DBColumn = false)]
        public DateTime? ObrDateLook { get; set; }
        #endregion

        #region  附加字段  接收科室
        /// <summary>
        /// 接收科室
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_name", MedName = "obr_user_name", WFName = "Lmsgrec_user_name", DBColumn = false)]
        public String ObrUserName { get; set; }
        #endregion

        #region  附加字段  接收科室
        /// <summary>
        /// 接收科室
        /// </summary>   
        [FieldMapAttribute(ClabName = "receiver_Id", MedName = "obr_user_id", WFName = "Lmsgrec_user_id", DBColumn = false)]
        public String ObrUserCode { get; set; }
        #endregion

        #region 附加字段 开始日期
        public DateTime StartDate { get; set; }
        #endregion

        #region 附加字段 结束日期
        public DateTime EndDate { get; set; }
        #endregion

        #region 附加字段 危急值处理数据实体集合
        public ObrMessageReceiveCollection ListObrMessageReceiver { get; set; }
        #endregion

        #region  附加字段  组合名称
        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_c_name", MedName = "pid_com_name", WFName = "Pma_com_name", DBColumn = false)]
        public String PidComName { get; set; }
        #endregion

        #region  附加字段  历史结果
        /// <summary>
        ///历史结果
        /// </summary>   
        public String HistoryResult { get; set; }
        #endregion

        #region  附加字段  是否向医生发送危急值短信
        /// <summary>
        /// 是否向医生发送危急值短信
        /// </summary>   
        public String ObrSendFlag { get; set; }
        #endregion

    }
}
