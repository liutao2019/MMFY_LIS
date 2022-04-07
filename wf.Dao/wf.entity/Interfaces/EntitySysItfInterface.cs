using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 院网接口表
    /// 旧表名:Sys_itf_interface  新表名:Base_itf_interface
    /// </summary>
    [Serializable()]
    public class EntitySysItfInterface : EntityBase
    {
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_id", MedName = "itface_id", WFName = "Bitf_id", DBIdentity = true)]
        public String ItfaceId { get; set; }

        /// <summary>
        ///接口名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_name", MedName = "itface_name", WFName = "Bitf_name")]
        public String ItfaceName { get; set; }

        /// <summary>
        ///数据库地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_db_address", MedName = "itface_server", WFName = "Bitf_server")]
        public String ItfaceServer { get; set; }

        /// <summary>
        ///数据库名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_db_name", MedName = "itface_database", WFName = "Bitf_database")]
        public String ItfaceDatabase { get; set; }

        /// <summary>
        ///用户名
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_db_username", MedName = "itface_logid", WFName = "Bitf_logid")]
        public String ItfaceLogid { get; set; }

        /// <summary>
        ///密码
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_db_password", MedName = "itface_password", WFName = "Bitf_password")]
        public String ItfacePassword { get; set; }

        /// <summary>
        ///连接类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_db_connect_type", MedName = "itface_connect_type", WFName = "Bitf_connect_type")]
        public String ItfaceConnectType { get; set; }

        /// <summary>
        ///接口类型(条码、院网、不使用接口)
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_interface_type", MedName = "itface_interface_type", WFName = "Bitf_type")]
        public String ItfaceInterfaceType { get; set; }

        /// <summary>
        ///调用语句
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_interface_sql", MedName = "itface_execute_sql", WFName = "Bitf_execute_sql")]
        public String ItfaceExecuteSql { get; set; }
        /// <summary>
        ///返回DataTable名
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_return_tablename", MedName = "itface_return_table", WFName = "Bitf_return_table")]
        public String ItfaceReturnTable { get; set; }

        /// <summary>
        ///接口取数据方式(视图、存储过程、sql语句)
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_interface_fetchtype", MedName = "itface_fetchtype", WFName = "Bitf_fetchtype")]
        public String ItfaceFetchtype { get; set; }

        /// <summary>
        ///医院ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_interface_hospital", MedName = "itface_org_id", WFName = "Bitf_org_id")]
        public String ItfaceOrgId { get; set; }

        #region 附加字段
        /// <summary>
        /// 医院名称
        /// </summary>
        [FieldMapAttribute(ClabName = "hos_name", MedName = "itface_org_id", WFName = "Dorg_name", DBColumn = false)]
        public string OrgName { get; set; }
        #endregion
    }
}
