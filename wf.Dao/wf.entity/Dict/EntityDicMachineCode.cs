using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器通道码
    /// 旧表名:Def_itr_machine_code 新表名:Rel_itr_channel_code
    /// </summary>
    [Serializable]
    public class EntityDicMachineCode : EntityBase
    {
        public EntityDicMachineCode()
        {
        }
        /// <summary>
        /// 编码
        /// </summary>
        [FieldMapAttribute(ClabName = "mit_id", MedName = "mac_id", WFName = "Ricc_id")]
        public String MacId { get; set; }

        /// <summary>
        /// 仪器代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_itr_id", MedName = "itr_id", WFName = "Ricc_Ditr_id")]
        public String ItrId { get; set; }

        /// <summary>
        /// 通道码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_cno", MedName = "mac_code", WFName = "Ricc_code")]
        public String MacCode { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_itm_id", MedName = "mac_itm_id", WFName = "Ricc_Ditm_id")]
        public String MacItmId { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_itm_ecd", MedName = "mac_itm_ecd", WFName = "Ricc_itm_ecd")]
        public String MacItmEcd { get; set; }

        /// <summary>
        /// 小数位
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_dec", MedName = "mac_dec_place", WFName = "Ricc_dec_place")]
        public Decimal? MacDecPlace { get; set; }

        /// <summary>
        /// 项目起始位
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_pos", MedName = "mac_position", WFName = "Ricc_position")]
        public Decimal? MacPosition { get; set; }

        /// <summary>
        /// 结束长度
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_rlen", MedName = "mac_res_len", WFName = "Ricc_res_len")]
        public Int32 MacResLen { get; set; }

        /// <summary>
        /// 结果类型
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_type", MedName = "mac_type", WFName = "Ricc_type")]
        public String MacType { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// 双向标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_flag", MedName = "mac_flag", WFName = "Ricc_flag")]
        public Int32 MacFlag { get; set; }

        /// <summary>
        ///  接收标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "mit_receive_flag", MedName = "mac_receive_flag", WFName = "Ricc_receive_flag")]
        public Int32 MacReceiveFlag { get; set; }

        /// <summary>
        ///  通道码类型
        /// </summary>                       
        //[FieldMapAttribute(ClabName = "mit_cno_type", MedName = "没有字段")]
        //public Int32 MitCnoType { get; set; }

        #region 附加字段

        /// <summary>
        /// 仪器代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename")]
        public string ItrEname { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_del", MedName = "instrument_del_flag", WFName = "Ditr_del")]
        public string ItrDel { get; set; }

        /// <summary>
        ///仪器名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name")]
        public string ItrName { get; set; }

        /// <summary>
        ///  项目名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name")]
        public string ItmName { get; set; }

        /// <summary>
        ///  删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_del", MedName = "item_del_flag", WFName = "Ditm_del")]
        public string ItmDelFlag { get; set; }

        /// <summary>
        ///  项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode")]
        public string ItmEcode { get; set; }
        #endregion
    }
}
