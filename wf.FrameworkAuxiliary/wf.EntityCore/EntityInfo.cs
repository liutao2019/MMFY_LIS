using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;

namespace Lib.EntityCore
{
    /// <summary>
    /// 实体信息
    /// </summary>
    public class EntityInfo
    {
        #region props&fields
        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type TypeEntity { get; set; }

        /// <summary>
        /// 是否为数据库映射实体
        /// </summary>
        public bool IsDbEntity
        {
            get
            {
                return this.TableAttribute != null;
            }
        }

        /// <summary>
        /// 删除时更新删除标志，不物理删除
        /// </summary>
        public bool IsDeleteWithFlag
        {
            get
            {
                return this.DelFlagAttribute != null;
            }
        }

        /// <summary>
        /// 表影射属性
        /// </summary>
        public EntityTableAttribute TableAttribute
        {
            get
            {
                return this.GetAttribute<EntityTableAttribute>();
            }
        }

        /// <summary>
        /// 删除标志属性
        /// </summary>
        public EntityDeleteWithFlagAttribute DelFlagAttribute
        {
            get
            {
                return this.GetAttribute<EntityDeleteWithFlagAttribute>();
            }
        }

        /// <summary>
        /// 字段信息集合
        /// </summary>
        public List<EntityPropertyInfo> Fields { get; set; }


        private List<IEntityAttribute> _attributes = null;
        #endregion

        #region 添加与获取字段自定义属性
        public void AddAttribute(IEntityAttribute attribute)
        {
            this._attributes.Add(attribute);
        }

        public void AddAttributes(IEntityAttribute[] attributes)
        {
            if (attributes != null && attributes.Length > 0)
            {
                foreach (IEntityAttribute attr in attributes)
                {
                    AddAttribute(attr);
                }
            }
        }

        /// <summary>
        /// 获取所有自定义属性
        /// </summary>
        /// <returns></returns>
        public IEntityAttribute[] GetAttributes()
        {
            return this._attributes.ToArray();
        }

        public T[] GetAttributes<T>() where T : IEntityAttribute
        {
            List<T> ret = new List<T>();
            foreach (IEntityAttribute item in this._attributes)
            {
                if (item is T)
                {
                    ret.Add(item as T);
                }
            }
            return ret.ToArray();
        }

        public T GetAttribute<T>() where T : IEntityAttribute
        {
            foreach (IEntityAttribute item in this._attributes)
            {
                if (item is T)
                {
                    return item as T;
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 根据字段名获取实体字段信息
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public EntityPropertyInfo GetFieldInfoByPropertyName(string propName)
        {
            EntityPropertyInfo info = Fields.Find(i => i.Property.Name == propName);
            return info;
        }

        /// <summary>
        /// 根据字段对应数据库列名获取字段信息
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public EntityPropertyInfo GetFieldInfoByPropertyDbColumnName(string colName)
        {
            foreach (EntityPropertyInfo item in Fields)
            {
                FieldMapAttribute[] dbFieldMapAttrs = item.GetAttributes<FieldMapAttribute>();
                if (dbFieldMapAttrs.Length > 0)
                {
                    foreach (FieldMapAttribute dbFieldMap in dbFieldMapAttrs)
                    {
                        if (dbFieldMap.DBColumnName == colName)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public EntityInfo()
        {
            this.Fields = new List<EntityPropertyInfo>();
            this._attributes = new List<IEntityAttribute>();
        }

        private DataTable _datatableSchema = null;

        /// <summary>
        /// 此实体的表结构
        /// </summary>
        public DataTable DataTableSchema
        {
            get
            {
                if (_datatableSchema == null)
                {
                    _datatableSchema = GetDataTableSchema();
                }
                return _datatableSchema.Clone();
            }
        }

        /// <summary>
        /// 获取实体对应的表结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTableSchema()
        {
            DataTable table = new DataTable();

            //是否为数据库对应的实体
            if (this.IsDbEntity)
            {
                table.TableName = this.TableAttribute.TableName;//是的话用实体指定的表名作为结构表的表名
            }
            else
            {
                table.TableName = this.TypeEntity.FullName;//否则使用实体名作为表名
            }

            //遍历每一个字段
            foreach (EntityPropertyInfo field in this.Fields)
            {
                Type dataType;
                if (field.Property.PropertyType.IsGenericType && field.Property.PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                {
                    dataType = field.Property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    dataType = field.Property.PropertyType;
                }

                if (field.IsDBField)
                {
                    table.Columns.Add(field.MapAttribute.DBColumnName, dataType);
                }
                else
                {
                    table.Columns.Add(field.Property.Name, dataType);
                }
            }

            return table;
        }
    }

    /// <summary>
    /// 字段属性信息
    /// </summary>
    public class EntityPropertyInfo
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EntityPropertyInfo()
        {
            this._attributes = new List<IEntityPropertyAttribute>();
            this.MapAttribute = null;
        }

        #region Properties&Fields
        /// <summary>
        /// 是否为数据库字段
        /// </summary>
        public bool IsDBField
        {
            get
            {
                return this.MapAttribute != null;
            }
        }

        /// <summary>
        /// 数据类型转换器
        /// </summary>
        internal DataTypeConverterAttribute DataTypeConverter = null;

        /// <summary>
        /// 字段影射属性
        /// </summary>
        public FieldMapAttribute MapAttribute { get; set; }

        /// <summary>
        /// 属性集合
        /// </summary>
        private List<IEntityPropertyAttribute> _attributes = null;

        /// <summary>
        /// 字段属性
        /// </summary>
        public PropertyInfo Property { get; set; }
        #endregion

        #region 添加与获取字段自定义属性
        public void AddAttribute(IEntityPropertyAttribute attribute)
        {
            if (attribute is FieldMapAttribute)
            {
                this.MapAttribute = (FieldMapAttribute)attribute;
            }
            else if (attribute is DataTypeConverterAttribute)
            {
                this.DataTypeConverter = (DataTypeConverterAttribute)attribute;
            }

            this._attributes.Add(attribute);
        }

        public void AddAttributes(IEntityPropertyAttribute[] attributes)
        {
            if (attributes != null && attributes.Length > 0)
            {
                foreach (IEntityPropertyAttribute attr in attributes)
                {
                    AddAttribute(attr);
                }
                //this._attributes.AddRange(attributes);
            }
        }

        /// <summary>
        /// 获取所有自定义属性
        /// </summary>
        /// <returns></returns>
        public IEntityPropertyAttribute[] GetAttributes()
        {
            return this._attributes.ToArray();
        }

        public T[] GetAttributes<T>() where T : IEntityPropertyAttribute
        {
            List<T> ret = new List<T>();
            foreach (IEntityPropertyAttribute item in this._attributes)
            {
                if (item is T)
                {
                    ret.Add(item as T);
                }
            }
            return ret.ToArray();
        }

        public T GetAttribute<T>() where T : IEntityPropertyAttribute
        {
            foreach (IEntityPropertyAttribute item in this._attributes)
            {
                if (item is T)
                {
                    return item as T;
                }
            }
            return null;
        }
        #endregion

        #region 通过数据类型转换器获取或设置字段值
        public void SetValueWithConverter(object obj, object value, object[] index)
        {
            if (DataTypeConverter != null)
            {
                object objValDest = DataTypeConverter.converter.ConvertFrom(value);
                this.Property.SetValue(obj, objValDest, index);
            }
            else
            {
                this.Property.SetValue(obj, value, index);
            }
        }

        public object GetValueWithConverter(object obj, object[] index)
        {
            object objValueSrc = this.Property.GetValue(obj, index);

            if (DataTypeConverter != null)
            {
                object objValueDest = DataTypeConverter.converter.ConvertTo(objValueSrc);
                return objValueDest;
            }
            else
            {
                return objValueSrc;
            }
        }
        #endregion

        public override string ToString()
        {
            return this.Property.Name;
        }
    }
}
