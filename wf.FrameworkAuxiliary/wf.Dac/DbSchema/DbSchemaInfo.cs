using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    /// <summary>
    /// 数据库表结构信息
    /// </summary>
    public class DbSchemaInfo
    {
        IDbSchemaProvider provider;

        /// <summary>
        /// .ctor
        /// </summary>
        public DbSchemaInfo()
            : this(DACConfig.Current.ConnectionString
                  , DACConfig.Current.DriverType
                  , DACConfig.Current.DataBaseDialet)
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="enumDriver">数据库驱动方式</param>
        /// <param name="enumDbDialet">数据库类型</param>
        public DbSchemaInfo(string connectionString
                                , EnumDbDriver enumDriver
                                , EnumDataBaseDialet enumDbDialet)
        {
            provider = DbSchemaHelper.GetProvider(connectionString, enumDriver, enumDbDialet);
        }

        #region 表

        private bool _tablesInited = false;
        private List<string> _tablesName = null;
        List<DbSchemaTable> _tablesSchema = null;

        /// <summary>
        /// 初始化表信息
        /// </summary>
        private void InitTable()
        {
            if (_tablesInited == false)
            {
                _tablesSchema = provider.GetTables();

                _tablesName = new List<string>();

                foreach (DbSchemaTable item in _tablesSchema)
                {
                    _tablesName.Add(item.TableName);
                }

                _tablesInited = true;
            }
        }

        /// <summary>
        /// 获取单个表信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DbSchemaTable GetTable(string tableName)
        {
            InitTable();
            DbSchemaTable table = _tablesSchema.Find(i => i.TableName == tableName);
            if (table != null && table.Columns == null)
            {
                table.Columns = provider.GetTableColumns(tableName);
            }
            return table;
        }

        public List<DbSchemaTable> GetTables()
        {
            InitTable();
            foreach (DbSchemaTable table in this._tablesSchema)
            {
                if (table.Columns == null)
                {
                    table.Columns = provider.GetTableColumns(table.TableName);
                }
            }
            return this._tablesSchema;
        }

        public string[] GetTablesName()
        {
            InitTable();
            return this._tablesName.ToArray();
        }
        #endregion

        #region 视图

        private bool _viewsInited = false;
        private List<string> _viewsName = null;
        List<DbSchemaView> _viewsSchema = null;

        /// <summary>
        /// 初始化表信息
        /// </summary>
        private void InitView()
        {
            if (_viewsInited == false)
            {
                _viewsSchema = provider.GetViews();

                _viewsName = new List<string>();

                foreach (DbSchemaView item in _viewsSchema)
                {
                    _viewsName.Add(item.ViewName);
                }

                _viewsInited = true;
            }
        }

        /// <summary>
        /// 获取单个表信息
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public DbSchemaView GetView(string viewName)
        {
            InitView();
            DbSchemaView view = _viewsSchema.Find(i => i.ViewName == viewName);
            if (view != null && view.Columns == null)
            {
                view.Columns = provider.GetViewColumns(viewName);
            }
            return view;
        }

        public List<DbSchemaView> GetViews()
        {
            InitView();
            foreach (DbSchemaView view in this._viewsSchema)
            {
                if (view.Columns == null)
                {
                    view.Columns = provider.GetViewColumns(view.ViewName);
                }
            }
            return this._viewsSchema;
        }

        public string[] GetViewsName()
        {
            InitView();
            return this._viewsName.ToArray();
        }
        #endregion

        public void Refresh()
        {
            _tablesName = null;
            _tablesSchema = null;
            _tablesInited = false;

            _viewsName = null;
            _viewsSchema = null;
            _viewsInited = false;
        }
    }
}
