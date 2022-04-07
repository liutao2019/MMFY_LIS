using System;
using System.Collections.Generic;
using System.Text;
using Lib.EntityCore;
using System.Data;
using Lib.DAC;
using System.Reflection;
using Lib.DAC.DbDriver;

namespace Lib.EntityCore
{
    public sealed class EntityHelper
    {
        public bool UpdateWithChangeLog { get; set; }

        private bool _checkDbFields = false;
        public bool CheckDbFields
        {
            get
            {
                return this._checkDbFields;
            }
            set
            {
                this._checkDbFields = value;
            }
        }

        string _connectionString;

        IDbDriver _driver = null;
        IDialet _dialet = null;
        ITransaction tran = null;

        public EntityHelper()
            : this(DACHelper.CurrentConnectionString
            , DACHelper.CurrentDriver
            , DACHelper.CurrentDialet)
        {
            UpdateWithChangeLog = true;
        }

        public EntityHelper(ITransaction tran)
        {
            this.tran = tran;
            this._dialet = tran.Dialet;
            this._driver = tran.Driver;

            UpdateWithChangeLog = true;
        }

        public EntityHelper(string connStr, EnumDbDriver driverType, EnumDataBaseDialet dbType)
        {
            this._connectionString = connStr;
            this._driver = DbDriverHelper.GetDriver(driverType);
            this._dialet = DbDialetHelper.GetDialet(dbType);
            UpdateWithChangeLog = true;
        }

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间
        /// </summary>
        public int CommandTimeout { get; set; }

        private SqlHelper GetSqlHelper()
        {
            SqlHelper helper;
            if (this.tran != null)
            {
                helper = new SqlHelper(this.tran);
            }
            else
            {
                helper = new SqlHelper(this._connectionString, this._driver, this._dialet);
            }
            helper.CommandTimeout = this.CommandTimeout;
            return helper;
        }

        #region 删除
        /// <summary>
        /// 删除(数据库删除)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deleteType"></param>
        public int Delete(object entity)
        {
            //获取实体信息
            EntityInfo entityinfo = GetEntityInfo(entity.GetType());
            string sql = null;

            //sql语句模板
            sql = "delete from {0} where {1}";

            //获取设定的表明
            string tableName = entityinfo.TableAttribute.TableName;

            //where条件参数的值集合
            List<object> valuesWHERE = new List<object>();

            //where条件参数的数据类型
            List<DbType> datatypesWHERE = new List<DbType>();

            StringBuilder sbWhere = new StringBuilder();

            bool needAND = false;

            //遍历没一个字段
            foreach (EntityPropertyInfo item in entityinfo.Fields)
            {
                //如果为数据库设定的字段
                if (item.IsDBField)
                {
                    //如果为主键，则把此属性添加到where条件中
                    if (item.MapAttribute.IsPrimaryKey)
                    {
                        if (needAND)
                        {
                            sbWhere.Append(" and ");
                        }

                        //数据库列名
                        sbWhere.Append(item.MapAttribute.DBColumnName + " = ?");

                        //获取字段当前值
                        object value = item.GetValueWithConverter(entity, null);

                        //null值要转换为dbnull
                        valuesWHERE.Add(value == null ? DBNull.Value : value);

                        //获取数据类型
                        datatypesWHERE.Add(item.MapAttribute.DbType);

                        needAND = true;
                    }
                }
            }

            string SqlToExecute = null;

            //拼接sql语句
            SqlToExecute = string.Format(sql, tableName, sbWhere);

            SqlHelper helper = GetSqlHelper();
            DbCommandEx cmd = CreateCommandEx(SqlToExecute);

            //添加sql参数并赋值
            for (int i = 0; i < valuesWHERE.Count; i++)
            {
                cmd.AddParameterValue(valuesWHERE[i], datatypesWHERE[i]);
            }

            return helper.ExecuteNonQuery(cmd);
        }



        #endregion

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(object entity)
        {
            try
            {
                EntityInfo entityinfo = GetEntityInfo(entity.GetType());

                //表名
                string tableName = entityinfo.TableAttribute.TableName;

                string sql = "insert into {0}({1}) values({2})";

                //要插入字段的值
                List<object> valuesInsert = new List<object>();
                //要插入字段数据类型
                List<DbType> datatypesInsert = new List<DbType>();

                //sql语句：要插入的字段
                StringBuilder sbFieldsInsert = new StringBuilder();

                //sql语句：要插入的值，问号标示，如：insert into table(id,name,value) values(?,?,?)
                StringBuilder sbFieldsValueInsert = new StringBuilder();

                //sql语句：插入后要查询出来的字段，如自增列
                StringBuilder sbFieldsSelectAfterInsert = new StringBuilder();

                //sql语句：插入后要查询的where条件
                StringBuilder sbSelectWhere = new StringBuilder();

                //数据插入后需要更新值的字段
                Dictionary<EntityPropertyInfo, object> dictFieldNeedUpdateValueWithSysIDAfterInsert = new Dictionary<EntityPropertyInfo, object>();

                //要查询字段的值
                List<object> valuesSelectWhere = new List<object>();
                //要查询字段的数据类型
                List<DbType> datatypesSelectWhere = new List<DbType>();

                //是否需要查询自增字段（sql数据库专有）
                bool needSelectIdentity = false;

                bool needComma = false;
                foreach (EntityPropertyInfo item in entityinfo.Fields)//遍历实体信息的每一个字段
                {
                    if (item.IsDBField)//如果为数据库存在的字段
                    {
                        SysTableIDGenerateAttribute attrSysID = item.GetAttribute<SysTableIDGenerateAttribute>();
                        if (item.MapAttribute.IsDBGenerate == true)//如果为数据库生成的字段
                        {
                            if (item.MapAttribute.IsPrimaryKey && this._dialet.SupportIdentitySelect)//如果该字段为主键
                            {
                                needSelectIdentity = true;

                                if (sbSelectWhere.Length > 0)
                                {
                                    sbSelectWhere.Append(" and ");
                                }
                                sbSelectWhere.Append(item.MapAttribute.DBColumnName + " = ");
                            }
                            sbFieldsSelectAfterInsert.Append("," + item.MapAttribute.DBColumnName);
                        }
                        else
                        {
                            if (needComma)
                            {
                                sbFieldsInsert.Append(",");
                                sbFieldsValueInsert.Append(",");
                            }

                            sbFieldsInsert.Append(item.MapAttribute.DBColumnName);
                            sbFieldsValueInsert.Append("?");

                            datatypesInsert.Add(item.MapAttribute.DbType);

                            object value;

                            if (attrSysID != null)
                            {
                                GlobalSysTableIDGenerator generator = new GlobalSysTableIDGenerator();
                                value = generator.Generate(tableName, item.MapAttribute.DBColumnName, attrSysID.Rule);

                                dictFieldNeedUpdateValueWithSysIDAfterInsert.Add(item, value);
                            }
                            else
                            {
                                value = item.GetValueWithConverter(entity, null);//反射取的实体的值
                            }

                            if (value == null)//null值转dbnull
                            {
                                value = DBNull.Value;
                            }

                            valuesInsert.Add(value);

                            if (item.MapAttribute.IsPrimaryKey)
                            {
                                if (sbSelectWhere.Length > 0)
                                {
                                    sbSelectWhere.Append(" and ");
                                }
                                sbSelectWhere.Append(string.Format("{0} = ? ", item.MapAttribute.DBColumnName));

                                valuesSelectWhere.Add(value);
                                datatypesSelectWhere.Add(item.MapAttribute.DbType);
                            }
                            needComma = true;
                        }
                    }
                }

                string sqlToInsert = string.Format(sql, tableName, sbFieldsInsert, sbFieldsValueInsert);

                string sqlSelect = null;
                if (sbFieldsSelectAfterInsert.Length > 0
                    || (needSelectIdentity && sbSelectWhere.Length > 0 && this._dialet.SupportIdentitySelect)
                    )
                {
                    string sqlWhere;

                    if (!needSelectIdentity)
                    {
                        sqlWhere = sbSelectWhere.ToString();
                    }
                    else
                    {
                        sqlWhere = sbSelectWhere + "(" + this._dialet.IdentitySelectString + ")";
                    }


                    sqlSelect = string.Format("select {0} from {1} where {2}", sbFieldsSelectAfterInsert.Remove(0, 1)
                                          , tableName
                                          , sqlWhere
                                          );
                }

                SqlHelper helper = GetSqlHelper();

                if (sqlSelect != null && this._dialet.SupportMultiQueries)
                {
                    sqlToInsert += this._dialet.MultiQueriesSperator + sqlSelect;
                }

                DbCommandEx cmdInsert = CreateCommandEx(sqlToInsert);

                for (int i = 0; i < valuesInsert.Count; i++)
                {
                    cmdInsert.AddParameterValue(valuesInsert[i], datatypesInsert[i]);
                }

                if (sqlSelect != null && this._dialet.SupportMultiQueries)
                {
                    for (int i = 0; i < valuesSelectWhere.Count; i++)
                    {
                        cmdInsert.AddParameterValue(valuesSelectWhere[i], datatypesSelectWhere[i]);
                    }
                }

                if (sqlSelect == null)
                {
                    helper.ExecuteNonQuery(cmdInsert);
                }
                else
                {
                    DataTable table;
                    if (this._dialet.SupportMultiQueries)
                    {
                        table = helper.GetTable(cmdInsert);
                    }
                    else
                    {
                        DbCommandEx cmdSelect = CreateCommandEx(sqlSelect);
                        for (int i = 0; i < valuesSelectWhere.Count; i++)
                        {
                            cmdSelect.AddParameterValue(valuesSelectWhere[i], datatypesSelectWhere[i]);
                        }
                        table = helper.GetLastCmdTable(new IDbCommand[] { cmdInsert, cmdSelect });
                    }

                    Lib.EntityCore.EntityConverter.DataRowToEntity<object>(entity, entityinfo, table.Rows[0]);
                }

                foreach (EntityPropertyInfo propInfo in dictFieldNeedUpdateValueWithSysIDAfterInsert.Keys)
                {
                    object objVal = dictFieldNeedUpdateValueWithSysIDAfterInsert[propInfo];
                    if (propInfo.Property.PropertyType == objVal.GetType())
                    {
                        propInfo.Property.SetValue(entity, objVal, null);
                    }
                    else
                    {
                        object objNewVal = Convert.ChangeType(objVal, propInfo.Property.PropertyType);
                        propInfo.Property.SetValue(entity, objNewVal, null);
                    }
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
        }


        #region Select


        public TEntity SelectSingle<TEntity>(IDbCommand cmd) where TEntity : class
        {
            SqlHelper helper = this.GetSqlHelper();
            DataTable table = helper.GetTable(cmd);

            if (table.Rows.Count > 0)
            {
                TEntity entity = Lib.EntityCore.EntityConverter.DataRowToEntity<TEntity>(null, GetEntityInfo(typeof(TEntity)), table.Rows[0]);
                return entity;
            }
            else
            {
                return null;
            }
        }

        public List<TEntity> SelectMany<TEntity>(string sql) where TEntity : class
        {
            DbCommandEx cmd = CreateCommandEx(sql);
            return SelectMany<TEntity>(cmd);
        }

        public List<TEntity> SelectMany<TEntity>(IDbCommand cmd) where TEntity : class
        {
            SqlHelper helper = this.GetSqlHelper();
            DataTable table = helper.GetTable(cmd);

            List<TEntity> list = Lib.EntityCore.EntityConverter.DataTableToEntityList<TEntity>(table);
            return list;
        }


        public DbCommandEx CreateCommandEx(string sql)
        {
            DbCommandEx cmd = new DbCommandEx(this._driver, this._dialet, sql);
            cmd.CommandTimeout = this.CommandTimeout;
            return cmd;
        }

        #endregion

        #region 更新
        public int Update(object entity)
        {
            if (entity is BaseEntity)
            {
                if (this.UpdateWithChangeLog && (entity as BaseEntity).PropertiesValueChanedLog.Count == 0)
                    return 0;
            }

            EntityInfo entityinfo = GetEntityInfo(entity.GetType());

            //表名
            string tableName = entityinfo.TableAttribute.TableName;

            string sql = "update {0} set {1} where {2}";

            List<object> valuesWHERE = new List<object>();
            List<DbType> datatypesWHERE = new List<DbType>();

            List<object> valuesSET = new List<object>();
            List<DbType> datatypesSET = new List<DbType>();

            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbSet = new StringBuilder();

            bool needAND = false;
            bool needCOMMA = false;
            foreach (EntityPropertyInfo item in entityinfo.Fields)
            {
                if (item.IsDBField)
                {
                    if (item.MapAttribute.IsPrimaryKey)
                    {
                        if (needAND)
                        {
                            sbWhere.Append(" and ");
                        }
                        sbWhere.Append(item.MapAttribute.DBColumnName + " = ?");

                        object value = item.GetValueWithConverter(entity, null);

                        valuesWHERE.Add(value == null ? DBNull.Value : value);
                        datatypesWHERE.Add(item.MapAttribute.DbType);

                        needAND = true;
                    }
                    else
                    {
                        if (entity is BaseEntity)
                        {
                            if (!(entity as BaseEntity).PropertiesValueChanedLog.ExistProperty(item.Property.Name)
                                && this.UpdateWithChangeLog
                                )
                            {
                                continue;
                            }
                        }

                        if (item.MapAttribute.IsDBGenerate)
                        {
                            continue;
                        }

                        if (needCOMMA)
                        {
                            sbSet.Append(",");
                        }

                        sbSet.Append(item.MapAttribute.DBColumnName + " = ? ");
                        object value = item.GetValueWithConverter(entity, null);
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }
                        valuesSET.Add(value);

                        datatypesSET.Add(item.MapAttribute.DbType);

                        needCOMMA = true;
                    }
                }
            }

            string sqlToExecute = string.Format(sql, tableName, sbSet, sbWhere);


            SqlHelper helper = GetSqlHelper();
            DbCommandEx cmd = CreateCommandEx(sqlToExecute);

            for (int i = 0; i < valuesSET.Count; i++)
            {
                cmd.AddParameterValue(valuesSET[i], datatypesSET[i]);
            }

            for (int i = 0; i < valuesWHERE.Count; i++)
            {
                cmd.AddParameterValue(valuesWHERE[i], datatypesWHERE[i]);
            }

            int rowsAffect = helper.ExecuteNonQuery(cmd);
            return rowsAffect;
        }

        #endregion

        /// <summary>
        /// 根据实体类型获取实体信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private EntityInfo GetEntityInfo(Type entityType)
        {
            EntityAnaliser ea = new EntityAnaliser();
            EntityInfo entityinfo = ea.GetEntityInfo(entityType);
            return entityinfo;
        }
    }
}
