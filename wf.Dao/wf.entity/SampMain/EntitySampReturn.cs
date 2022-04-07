using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 回退条码信息表
    /// 旧表名:Samp_return 新表名:Sample_return
    /// </summary>
    [Serializable()]
    public class EntitySampReturn : EntityBase
    {
        public EntitySampReturn()
        {

        }

        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "return_sn", WFName = "Sret_id")]
        public Decimal ReturnSn { get; set; }

        /// <summary>
        ///回退条码内部关联ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_no", MedName = "samp_bar_id", WFName = "Sret_Smain_bar_id")]
        public String SampBarId { get; set; }

        /// <summary>
        ///回退条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sret_Smain_bar_code")]
        public String SampBarCode { get; set; }

        /// <summary>
        ///回退信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_message", MedName = "return_reasons", WFName = "Sret_return_reasons")]
        public String ReturnReasons { get; set; }

        /// <summary>
        ///科室代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_code", MedName = "return_dept_code", WFName = "Sret_dept_code")]
        public String ReturnDeptCode { get; set; }

        /// <summary>
        ///科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_name", MedName = "return_dept_name", WFName = "Sret_dept_name")]
        public String ReturnDeptName { get; set; }

        /// <summary>
        ///回退人工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sender_code", MedName = "return_user_id", WFName = "Sret_user_id")]
        public String ReturnUserId { get; set; }

        /// <summary>
        ///回退人名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sender_name", MedName = "return_user_name", WFName = "Sret_user_name")]
        public String ReturnUserName { get; set; }

        /// <summary>
        ///查看标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_read_flag", MedName = "return_read_flag", WFName = "Sret_read_flag")]
        public Boolean ReturnReadFlag { get; set; }

        /// <summary>
        /// 回退时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_time", MedName = "return_date", WFName = "Sret_date")]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// 条码主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_barcode_id", MedName = "return_samp_sn", WFName = "Sret_Smain_id")]
        public Int32 ReturnSampSn { get; set; }

        /// <summary>
        ///处理标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_handle_flag", MedName = "return_proc_flag", WFName = "Sret_proc_flag")]
        public Boolean ReturnProcFlag { get; set; }

        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_del", MedName = "del_flag", WFName = "del_flag")]
        public Boolean DelFlag { get; set; }

        /// <summary>
        ///病人来源
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ori_id", MedName = "pid_src_id", WFName = "Sret_pat_Dsorc_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver", MedName = "return_receiver", WFName = "Sret_receiver")]
        public String ReturnReceiver { get; set; }

        #region 附加信息

        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "pid_name", WFName = "Sma_pat_name", DBColumn = false)]
        public String PidName { get; set; }

        /// <summary>
        ///组合累加显示名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_name", MedName = "samp_com_name", WFName = "Sma_com_name", DBColumn = false)]
        public String SampComName { get; set; }

        /// <summary>
        /// 病人标识(住院号)
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_in_no", MedName = "pid_in_no", WFName = "Sma_pat_in_no", DBColumn = false)]
        public String PidInNo { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_bed_no", MedName = "pid_bed_no", WFName = "Sma_pat_bed_no", DBColumn = false)]
        public String PidBedNo { get; set; }

        #endregion

        /// <summary>
        /// 回退类型
        /// </summary>
        public Int32? MessageType { get; set; }
    }
}
