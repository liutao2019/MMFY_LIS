using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器字典表
    /// 旧表名:Dic_itr_instrument 新表名:Dict_itr_instrument
    /// </summary>
    [Serializable]
    public class EntityDicInstrument : EntityBase
    {
        public EntityDicInstrument()
        {
            SortNo = 0;
            ItrCommType = 1; //默认为单项
            ItrPurchaseDate = DateTime.Now;
            ItrEnableDate = DateTime.Now;
            Checked = false;
        }
        /// <summary>
        /// 编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_id", MedName = "itr_id", WFName = "Ditr_id")]
        public String ItrId { get; set; }

        /// <summary>
        /// 仪器编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename")]
        public String ItrEname { get; set; }

        /// <summary>
        /// 工作站号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_client", MedName = "itr_client_no", WFName = "Ditr_client_no")]
        public String ItrClientNo { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_incode", MedName = "c_code", WFName = "Ditr_c_code")]
        public String CCode { get; set; }

        /// <summary>
        /// 物理组别
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_type", MedName = "itr_lab_id", WFName = "Ditr_lab_id")]
        public String ItrLabId { get; set; }

        /// <summary>
        /// 仪器名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name")]
        public String ItrName { get; set; }

        /// <summary>
        /// 报告类型编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_rep_id", MedName = "itr_report_id", WFName = "Ditr_report_id")]
        public String ItrReportId { get; set; }

        /// <summary>
        /// 数据类型 01-普通 02-酶标  03-细菌 04-描述 05-过敏源 06-新生儿筛查 07-骨髓
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_rep_flag", MedName = "itr_report_type", WFName = "Ditr_report_type")]
        public String ItrReportType { get; set; }

        /// <summary>
        /// 默认组合
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_com_id", MedName = "itr_com_id", WFName = "Ditr_Dcom_id")]
        public String ItrComId { get; set; }

        /// <summary>
        /// 默认标本
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_sam_id", MedName = "itr_sam_id", WFName = "Ditr_Dsam_id")]
        public String ItrSamId { get; set; }

        /// <summary>
        /// 专业组
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_ptype", MedName = "itr_pro_id", WFName = "Ditr_Dpro_id")]
        public String ItrProId { get; set; }

        /// <summary>
        /// 组合组别
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_ctype", MedName = "itr_com_pro_id", WFName = "Ditr_com_Dpro_id")]
        public String ItrComProId { get; set; }

        /// <summary>
        /// 子标题名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_stitle", MedName = "itr_sub_title", WFName = "Ditr_sub_title")]
        public String ItrSubTitle { get; set; }

        /// <summary>
        /// 标题模式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_stitle_mod", MedName = "itr_title_mod", WFName = "Ditr_title_mod")]
        public Int32? ItrTitleMod { get; set; }

        /// <summary>
        /// 自动计算标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_cal", MedName = "itr_autocalu_flag", WFName = "Ditr_autocalu_flag")]
        public Int32 ItrAutocaluFlag { get; set; }

        /// <summary>
        /// 通讯类型 1-单向 2-双向
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_host_flag", MedName = "itr_comm_type", WFName = "Ditr_comm_type")]
        public Int32 ItrCommType { get; set; }

        /// <summary>
        /// 双向用测试类型
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_test_type", MedName = "itr_test_type", WFName = "Ditr_test_type")]
        public String ItrTestType { get; set; }

        /// <summary>
        /// 双向方式
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_host_type", MedName = "itr_host_type", WFName = "Ditr_host_type")]
        public Int32 ItrHostType { get; set; }

        /// <summary>
        /// 取报告地点
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_rep_addres", MedName = "itr_report_address", WFName = "Ditr_report_address")]
        public String ItrReportAddress { get; set; }

        /// <summary>
        /// 质控标志 1-要求每天需审核质控
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_qc_flag", MedName = "itr_qc_check", WFName = "Ditr_qc_check")]
        public Int32 ItrQcCheck { get; set; }

        /// <summary>
        /// 默认医院
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_hospital_id", MedName = "itr_org_id", WFName = "Ditr_Dorg_id")]
        public String ItrOrgId { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32? SortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// 镜检标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_urt_flag", MedName = "itr_micro_flag", WFName = "Ditr_micro_flag")]
        public Int32 ItrMicroFlag { get; set; }

        /// <summary>
        /// 存储仪器代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_con_id", MedName = "itr_report_itr_id", WFName = "Ditr_report_Ditr_id")]
        public String ItrReportItrId { get; set; }

        /// <summary>
        /// 默认审核者
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_chk_id", MedName = "itr_report_chk_id", WFName = "Ditr_report_chk_id")]
        public String ItrReportChkId { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_manufacturers", MedName = "itr_manufacturers", WFName = "Ditr_manufacturers")]
        public String ItrManufacturers { get; set; }

        /// <summary>
        /// 厂商ID
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_outFactory_id", MedName = "itr_factory_id", WFName = "Ditr_factory_id")]
        public String ItrFactoryId { get; set; }

        /// <summary>
        /// 设备
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_spec", MedName = "itr_sepc", WFName = "Ditr_sepc")]
        public String ItrSepc { get; set; }

        /// <summary>
        /// 型号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_model", MedName = "itr_model", WFName = "Ditr_model")]
        public String ItrModel { get; set; }

        /// <summary>
        /// 生产地点
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_producing_area", MedName = "itr_producing_area", WFName = "Ditr_producing area")]
        public String ItrProducingArea { get; set; }

        /// <summary>
        /// 状态
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_status", MedName = "itr_status", WFName = "Ditr_status")]
        public String ItrStatus { get; set; }

        /// <summary>
        /// 是否存储仪器
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_rep_ins", MedName = "itr_report_ins", WFName = "Ditr_report_ins")]
        public Int32 ItrReportIns { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_enable_date", MedName = "itr_enable_date", WFName = "Ditr_enable_date")]
        public DateTime ItrEnableDate { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_contract_no", MedName = "itr_contract_no", WFName = "Ditr_contract_no")]
        public String ItrContractNo { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_purchase_date", MedName = "itr_purchase_date", WFName = "Ditr_purchase_date")]
        public DateTime ItrPurchaseDate { get; set; }

        /// <summary>
        /// 价格
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_price", MedName = "itr_price", WFName = "Ditr_price")]
        public Decimal ItrPrice { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_supplier", MedName = "itr_supplier", WFName = "Ditr_supplier")]
        public String ItrSupplier { get; set; }

        /// <summary>
        /// 折旧日期
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_depreciation_date", MedName = "itr_depreciation_date", WFName = "Ditr_depreciation_date")]
        public Int32 ItrDepreciationDate { get; set; }

        /// <summary>
        /// 折旧率
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_depreciation_rate", MedName = "itr_depreciation_rate", WFName = "Ditr_depreciation_rate")]
        public Decimal ItrDepreciationRate { get; set; }

        /// <summary>
        /// 商标
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_depreciation_rate", MedName = "itr_brand", WFName = "Ditr_brand")]
        public String ItrBrand { get; set; }

        /// <summary>
        /// 类型
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_mic_type", MedName = "itr_mic_type", WFName = "Ditr_mic_type")]
        public Int32 ItrMicType { get; set; }

        /// <summary>
        /// 图像标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_pic_flag", MedName = "itr_image_flag", WFName = "Ditr_image_flag")]
        public Int32 ItrImageFlag { get; set; }

        #region 附件字段 报表名称
        /// <summary>
        /// 报表名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "rep_name", MedName = "rep_name", WFName = "rep_name", DBColumn = false)]
        public String ItrRepName { get; set; }
        #endregion

        #region 附件字段 组合名称
        /// <summary>
        /// 组合名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ItrComName { get; set; }
        #endregion

        #region 附件字段 标本类别
        /// <summary>
        /// 标本类别
        /// </summary>                       
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public String ItrSamName { get; set; }
        #endregion

        #region 附件字段 医院名称
        /// <summary>
        /// 医院名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "hos_name", MedName = "org_name", WFName = "Dorg_name", DBColumn = false)]
        public String ItrOrgName { get; set; }
        #endregion

        #region 附件字段 组别名称C
        /// <summary>
        /// 组别名称C
        /// </summary>                       
        [FieldMapAttribute(ClabName = "type_name_c", MedName = "type_name_c", WFName = "type_name_c", DBColumn = false)]
        public String ItrTypeNameC { get; set; }
        #endregion

        #region 附加字段 组别名称
        /// <summary>
        /// 组别名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ItrTypeName { get; set; }
        #endregion

        #region 附加字段 组别名称S
        /// <summary>
        /// 组别名称S
        /// </summary>                       
        [FieldMapAttribute(ClabName = "type_name_s", MedName = "type_name_s", WFName = "type_name_s", DBColumn = false)]
        public String ItrTypeNameS { get; set; }
        #endregion

        #region 附加字段 报表删除标志
        /// <summary>
        /// 报表删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "rep_del", MedName = "rep_del", WFName = "rep_del", DBColumn = false)]
        public String ItrRepDel { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        /// 仪器代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_con_name", MedName = "itr_con_name", WFName = "itr_con_name", DBColumn = false)]
        public String ItrConName { get; set; }
        #endregion

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return ItrId;
            }
        }
        #endregion
    }
}
