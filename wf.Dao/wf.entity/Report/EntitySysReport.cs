using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报表
    /// 旧表名:Sys_report 新表名:Base_report
    /// </summary>
    [Serializable]
    public class EntitySysReport : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "repid", MedName = "rep_id", WFName = "Brep_id")]
        public Int32 RepId { get; set; }

        /// <summary>
        ///报表名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "repname", MedName = "rep_name", WFName = "Brep_name")]
        public String RepName { get; set; }

        /// <summary>
        ///报表代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "repcode", MedName = "rep_code", WFName = "Brep_code")]
        public String RepCode { get; set; }

        /// <summary>
        ///报表地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "repAddress", MedName = "rep_location", WFName = "Brep_location")]
        public String RepLocation { get; set; }

        /// <summary>
        ///报表sql语句
        /// </summary>   
        [FieldMapAttribute(ClabName = "repSql", MedName = "rep_sql", WFName = "Brep_sql")]
        public String RepSql { get; set; }

        /// <summary>
        ///备份sql
        /// </summary>   
        [FieldMapAttribute(ClabName = "repDefaultSql", MedName = "rep_default_sql", WFName = "Brep_default_sql")]
        public String RepDefaultSql { get; set; }

        /// <summary>
        ///数据库连接
        /// </summary>   
        [FieldMapAttribute(ClabName = "repconncode", MedName = "rep_conect_code", WFName = "Breap_Dconn_code")]
        public String RepConectCode { get; set; }

        #region 附加字段 
        /// <summary>
        ///是否选中
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_select", MedName = "rep_select", WFName = "rep_select", DBColumn = false)]
        public String RepSelect { get; set; }

        /// <summary>
        ///数据库名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_name", MedName = "conn_name", WFName = "conn_name", DBColumn = false)]
        public String RepConnName { get; set; }

        /// <summary>
        /// 上传的文件名称
        /// </summary>
        [FieldMapAttribute(ClabName = "rep_flieName", MedName = "rep_flieName", WFName = "rep_flieName", DBColumn = false)]
        public String FlieName { get; set; }

        /// <summary>
        /// 上传的报表文件Base64格式
        /// </summary>
        [FieldMapAttribute(ClabName = "rep_fliebase64", MedName = "rep_fliebase64", WFName = "rep_fliebase64", DBColumn = false)]
        public String FlieBase64 { get; set; }


        #endregion

        #region 附加字段 报表语句条件
        /// <summary>
        /// 报表语句条件
        /// </summary>
        [FieldMapAttribute(ClabName = "repWhere", MedName = "repWhere", WFName = "repWhere", DBColumn = false)]
        public String RepWhere { get; set; }
        #endregion

    }
}
