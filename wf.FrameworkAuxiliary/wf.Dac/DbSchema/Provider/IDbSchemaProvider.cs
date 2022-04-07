using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    /// <summary>
    /// 数据库信息类
    /// </summary>
    internal interface IDbSchemaProvider
    {
        /// <summary>
        /// 获取所有表信息
        /// </summary>
        /// <returns></returns>
        List<DbSchemaTable> GetTables();

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DbSchemaTable GetTable(string tableName);

        /// <summary>
        /// 获取所有视图
        /// </summary>
        /// <returns></returns>
        List<DbSchemaView> GetViews();

        /// <summary>
        /// 获取视图信息
        /// </summary>
        /// <returns></returns>
        DbSchemaView GetView(string viewName);

        /// <summary>
        /// 获取表列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DbSchemaColumnCollection GetTableColumns(string tableName);

        /// <summary>
        /// 获取表列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        DbSchemaColumn GetTableColumn(string tableName, string columnName);


        /// <summary>
        /// 获取视图列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DbSchemaColumnCollection GetViewColumns(string viewName);

        /// <summary>
        /// 获取视图列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        DbSchemaColumn GetViewColumn(string viewName, string columnName);

        /// <summary>
        /// 获取数据库基本信息
        /// </summary>
        /// <returns></returns>
        DbSchemaInfo GetDataBaseInfo();
    }
}
