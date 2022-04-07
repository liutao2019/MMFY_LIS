using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 连接参数实体类
    /// 旧表名:dict_DataInterfaceConnection 新表名:dict_DataInterfaceConnection
    /// </summary>
    [Serializable]
    public class EntityDicDataInterfaceConnection : EntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_id", MedName = "conn_id", WFName = "conn_id")]
        public String ConnId { get; set; }

        /// <summary>
        /// 连接名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_name", MedName = "conn_name", WFName = "conn_name")]
        public String ConnName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_type", MedName = "conn_type", WFName = "conn_type")]
        public String ConnType { get; set; }

        /// <summary>
        /// 数据库地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_address", MedName = "conn_address", WFName = "conn_address")]
        public String ConnAddress { get; set; }

        /// <summary>
        /// 数据驱动
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_db_driver", MedName = "conn_db_driver", WFName = "conn_db_driver")]
        public String ConnDbDriver { get; set; }

        /// <summary>
        /// 数据库类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_db_dialet", MedName = "conn_db_dialet", WFName = "conn_db_dialet")]
        public String ConnDbDialet { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_db_catelog", MedName = "conn_db_catelog", WFName = "conn_db_catelog")]
        public String ConnDbCatelog { get; set; }

        /// <summary>
        /// 备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_desc", MedName = "conn_desc", WFName = "conn_desc")]
        public String ConnDesc { get; set; }

        /// <summary>
        /// 登入名
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_login", MedName = "conn_login", WFName = "conn_login")]
        public String ConnLogin { get; set; }

        /// <summary>
        /// 密码
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_pass", MedName = "conn_pass", WFName = "conn_pass")]
        public String ConnPass { get; set; }

        /// <summary>
        /// 模块
        /// </summary>   
        [FieldMapAttribute(ClabName = "sys_module", MedName = "sys_module", WFName = "sys_module")]
        public String SysModule { get; set; }

        /// <summary>
        /// (系统预置)
        /// </summary>   
        [FieldMapAttribute(ClabName = "sys_default", MedName = "sys_default", WFName = "sys_default")]
        public Int32 SysDefault { get; set; }

        /// <summary>
        /// 运行端
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_running_side", MedName = "conn_running_side", WFName = "conn_running_side")]
        public String ConnRunningSide { get; set; }

        /// <summary>
        /// 编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_code", MedName = "conn_code", WFName = "conn_code")]
        public String ConnCode { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public String DisPlayName { get; set; }
    }
}
