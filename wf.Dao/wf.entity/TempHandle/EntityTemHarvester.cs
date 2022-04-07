using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcl.entity
{
    [Serializable]
    public class EntityTemHarvester : EntityBase
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        [FieldMapAttribute(ClabName = "th_id", MedName = "th_id", WFName = "th_id")]
        public String ThId { get; set; }

        /// <summary>
        /// 采集器id 关联dict_harvester.dh_id
        /// </summary>
        [FieldMapAttribute(ClabName = "th_h_id", MedName = "th_h_id", WFName = "th_h_id")]
        public String ThHId { get; set; }

        /// <summary>
        /// 采集温度
        /// </summary>
        [FieldMapAttribute(ClabName = "th_temperature", MedName = "th_temperature", WFName = "th_temperature")]
        public String ThTemperature { get; set; }

        /// <summary>
        /// 采集湿度
        /// </summary>
        [FieldMapAttribute(ClabName = "th_humidity", MedName = "th_humidity", WFName = "th_humidity")]
        public String ThHumidity { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        [FieldMapAttribute(ClabName = "th_date", MedName = "th_date", WFName = "th_date")]
        public DateTime ThDate { get; set; }


        #region 附加字段 采集器名称
        /// <summary>
        /// 采集器名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_name", MedName = "dh_name", WFName = "dh_name")]
        public String DhName { get; set; }
        #endregion

        #region 附加字段 组别名称
        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 最高温
        /// <summary>
        /// 最高温
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_h_limit", MedName = "dt_h_limit", WFName = "dt_h_limit")]
        public String DtHLimit { get; set; }
        #endregion

        #region 附加字段 最低温
        /// <summary>
        /// 最低温
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_l_limit", MedName = "dt_l_limit", WFName = "dt_l_limit")]
        public String DtLLimit { get; set; }
        #endregion

        #region 附加字段 冰箱名称
        /// <summary>
        /// 最低温
        /// </summary>
        [FieldMapAttribute(ClabName = "ice_name", MedName = "store_name", WFName = "Dstore_name")]
        public String StoreName { get; set; }
        #endregion

        #region 附加字段  采集器编码
        /// <summary>
        /// 采集器编码
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_code", MedName = "dh_code", WFName = "dh_code")]
        public string DhCode { get; set; }
        #endregion
    }
}
