using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 试管存储信息表
    /// 旧表名:samp_store_detail 新表名:Sample_store_detail
    /// </summary>
    [Serializable]
    public class EntitySampStoreDetail : EntityBase
    {
        public EntitySampStoreDetail()
        {
            Checked = false;
        }
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_id", MedName = "det_id", WFName = "Ssdt_Ssr_id")]
        public String DetId { get; set; }

        /// <summary>
        ///试管条码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_bar_code", MedName = "det_bar_code", WFName = "Ssdt_bar_code")]
        public String DetBarCode { get; set; }

        /// <summary>
        ///试管横向孔位
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_num_x", MedName = "det_x", WFName = "Ssdt_x")]
        public Int32 DetX { get; set; }

        /// <summary>
        ///试管纵向孔位
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_num_y", MedName = "det_y", WFName = "Ssdt_y")]
        public Int32 DetY { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_seq", MedName = "det_seqno", WFName = "Ssdt_seqno")]
        public Int32? DetSeqno { get; set; }

        /// <summary>
        ///存储状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_satus", MedName = "det_status", WFName = "Ssdt_status")]
        public Int32 DetStatus { get; set; }

        /// <summary>
        ///存储日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "ssd_createtime", MedName = "det_date", WFName = "Ssdt_date")]
        public DateTime? DetDate { get; set; }

        #region 附加字段1 病人条码号
        /// <summary>
        ///病人条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_bar_code", MedName = "rep_bar_code", WFName = "Pma_bar_code", DBColumn = false)]
        public String RepBarCode { get; set; }
        #endregion

        #region 附加字段2 病人姓名
        /// <summary>
        ///病人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "Pma_pat_name", DBColumn = false)]
        public String PidName { get; set; }
        #endregion

        #region 附加字段3 组合名称
        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_c_name", MedName = "pid_com_name", WFName = "Pma_com_name", DBColumn = false)]
        public String PidComName { get; set; }
        #endregion

        #region 附加字段4 性别
        /// <summary>
        ///性别
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sex", MedName = "pid_sex", WFName = "Pma_pat_sex", DBColumn = false)]
        public String PidSex { get; set; }
        #endregion

        #region 附加字段 性别中文名称
        /// <summary>
        ///性别中文名称
        /// </summary>   
        public String PidSexName
        {
            get
            {
                if (PidSex == "1")
                    return "男";
                else
                    return "女";
            }
        }
        #endregion

        #region 附加字段5 年龄
        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age", MedName = "pid_age", WFName = "pid_age", DBColumn = false)]
        public String PidAge { get; set; }
        #endregion

        #region 附加字段6 科室编码
        /// <summary>
        ///科室编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_id", MedName = "pid_dept_id", WFName = "Pma_pat_dept_id", DBColumn = false)]
        public String PidDeptId { get; set; }
        #endregion

        #region 附加字段7 科室名称
        /// <summary>
        ///科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_name", MedName = "pid_dept_name", WFName = "Pma_pat_dept_name", DBColumn = false)]
        public String PidDeptName { get; set; }
        #endregion

        #region 附加字段8 标本编码
        /// <summary>
        ///标本编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sam_id", MedName = "pid_sam_id", WFName = "Pma_Dsam_id", DBColumn = false)]
        public String PidSamId { get; set; }
        #endregion

        #region 附加字段9 状态 0-未审核 1-已审核 2-已报告
        /// <summary>
        ///状态 0-未审核 1-已审核 2-已报告
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_flag", MedName = "rep_status", WFName = "Pma_status", DBColumn = false)]
        public String RepStatus { get; set; }
        #endregion

        #region 附加字段10 样本号
        /// <summary>
        ///样本号
        /// </summary>  
        [FieldMapAttribute(ClabName = "pat_sid", MedName = "rep_sid", WFName = "Pma_sid", DBColumn = false)]
        public String PatSid { get; set; }
        #endregion

        #region 附加字段11 状态中文
        /// <summary>
        /// 状态中文
        /// </summary>  
        public String PatFlagStatus
        {
            get
            {
                if (RepStatus == "2") return "已报告";
                else if (RepStatus == "1") return "已审核";
                else if (RepStatus == "4") return "已打印";
                else { return "未审核"; }
            }
        }
        #endregion

        #region 附加字段12 冰箱ID
        /// <summary>
        /// 冰箱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_icebox", MedName = "sr_store_id", WFName = "Ssr_Dstore_id", DBColumn = false)]
        public String SrStoreId { get; set; }
        #endregion


        #region 附加字段13 位置
        /// <summary>
        /// 位置
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_place", MedName = "sr_place", WFName = "Ssr_Dpos_id", DBColumn = false)]
        public String SrPlace { get; set; }
        #endregion

        #region 附加字段14 架子状态
        /// <summary>
        /// 架子状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_status", MedName = "sr_status", WFName = "Ssr_status", DBColumn = false)]
        public String SrStatus { get; set; }
        #endregion

        #region 附加字段15 架子编码
        /// <summary>
        /// 架子编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_rack_id", MedName = "sr_rack_id", WFName = "Ssr_Drack_id", DBColumn = false)]
        public String SrRackId { get; set; }
        #endregion

        #region 附加字段16 存储名称
        /// <summary>
        /// 存储名称
        /// </summary>   
        public String DetNumXY
        {
            get
            {
                if (_DetNumXY != null)
                {
                    return _DetNumXY;
                }
                return DetX.ToString() + "*" + DetY.ToString();
            }
        }
        public String _DetNumXY { get; set; }
        #endregion

        #region 附加字段17 是否选中
        /// <summary>
        /// 是否选中  
        /// </summary>   
        public bool Checked { get; set; }
        #endregion

        #region 附加字段18 序号(双向)
        /// <summary>
        /// 序号(双向)
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_host_order", MedName = "rep_serial_num", WFName = "Pma_serial_num", DBColumn = false)]
        public String RepSerialNum { get; set; }
        #endregion  

        #region 附加字段19 病人ID
        /// <summary>
        /// 病人ID
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "Pma_pat_in_no", DBColumn = false)]
        public String PidInNo { get; set; }
        #endregion

        #region 附加字段20  试管横向纵向孔位数
        /// <summary>
        /// 试管横向纵向孔位数
        /// </summary>
        [FieldMapAttribute(ClabName = "ssd_num_xy", MedName = "det_xy", WFName = "det_xy", DBColumn = false)]
        public String DetXY { get; set; }
        #endregion

        #region 附加字段21   组别名称
        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段22   标本名称
        /// <summary>
        /// 标本名称
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public string SamName { get; set; }
        #endregion

        #region 附加字段23   架子名称 
        /// <summary>
        /// 架子名称
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_name", MedName = "rack_name", WFName = "Drack_name", DBColumn = false)]
        public String RackName { get; set; }
        #endregion

        #region 附加字段24   架子条码
        /// <summary>
        /// 架子条码
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_barcode", MedName = "rack_barcode", WFName = "Drack_barcode", DBColumn = false)]
        public String RackBarcode { get; set; }
        #endregion

        #region 附加字段25   规格
        /// <summary>
        /// 规格
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_spec", MedName = "rack_spec", WFName = "Drack_Dtrack_code", DBColumn = false)]
        public String RackSpec { get; set; }
        #endregion

        #region 附加字段26   架子颜色 
        /// <summary>
        /// 架子颜色
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_color", MedName = "rack_colour", WFName = "Drack_colour", DBColumn = false)]
        public String RackColour { get; set; }
        #endregion

        #region 附加字段27   冰箱名称
        /// <summary>
        /// 冰箱名称
        /// </summary>
        [FieldMapAttribute(ClabName = "ice_name", MedName = "store_name", WFName = "Dstore_name", DBColumn = false)]
        public String StoreName { get; set; }
        #endregion

        #region 附加字段28   柜子名称
        /// <summary>
        /// 柜子名称
        /// </summary>
        [FieldMapAttribute(ClabName = "cup_name", MedName = "area_name", WFName = "Dpos_name", DBColumn = false)]
        public String AreaName { get; set; }
        #endregion

        #region 附加字段29  仪器代码，对应dict_instrmt表中itr_id
        /// <summary>
        /// 仪器代码，对应dict_instrmt表中itr_id
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_itr_id", MedName = "rep_itr_id", WFName = "Pma_Ditr_id", DBColumn = false)]
        public String RepItrId { get; set; }
        #endregion

        #region 附加字段30   样本号
        /// <summary>
        /// 样本号
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_sid", MedName = "rep_sid", WFName = "Pma_sid", DBColumn = false)]
        public String RepSid { get; set; }
        #endregion

        #region 附加字段31   检验时间
        /// <summary>
        /// 检验时间
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_jy_date", MedName = "samp_check_date", WFName = "Pma_check_date", DBColumn = false)]
        public DateTime SampCheckDate { get; set; }
        #endregion

        #region 附加字段32  病人来源类型
        /// <summary>
        /// 病人来源类型
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_ori_id", MedName = "pid_src_id", WFName = "Pma_pat_Dsorc_id", DBColumn = false)]
        public String PidSrcId { get; set; }
        #endregion

        #region 附加字段33 是否选中(勾选时要用到)
        /// <summary>
        /// 是否选中  
        /// </summary>   
        [FieldMapAttribute(ClabName = "isselected", MedName = "isselected", WFName = "isselected", DBColumn = false)]
        public String IsSelected { get; set; }
        #endregion

    }
}
