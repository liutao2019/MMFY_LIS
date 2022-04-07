using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Lib.EntityCore
{
    /// <summary>
    /// 实体转换工具
    /// </summary>
    public class EntityConverter
    {
        /// <summary>
        /// DataTable转实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        public static List<TEntity> DataTableToEntityList<TEntity>(DataTable dtSource) where TEntity : class
        {
            List<TEntity> list = new List<TEntity>();

            EntityAnaliser ea = new EntityAnaliser();

            //获取实体信息
            EntityInfo ei = ea.GetEntityInfo(typeof(TEntity));

            if (ei == null)
            {
                return list;
            }

            foreach (DataRow dr in dtSource.Rows)
            {
                TEntity entity = DataRowToEntity<TEntity>(null, ei, dr);
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// DataRow转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ei"></param>
        /// <param name="drSource"></param>
        /// <returns></returns>
        public static T DataRowToEntity<T>(T sourceEntity, EntityInfo ei, DataRow drSource) where T : class
        {
            //创建实体实例
            if (sourceEntity == null)
            {
                sourceEntity = Activator.CreateInstance(typeof(T)) as T;
            }

            if (sourceEntity is BaseEntity)
            {
                (sourceEntity as BaseEntity).EnablePropertiesValueChangedLog = false;
            }

            foreach (EntityPropertyInfo entityProp in ei.Fields)
            {
                //PropertyInfo prop = entityProp.Property;
                if (entityProp.Property.CanWrite)
                {
                    string propColName;
                    if (entityProp.IsDBField)//如果为数据库实体，则用字段指定的映射数据库列明
                    {
                        propColName = entityProp.MapAttribute.DBColumnName;
                    }
                    else//否则使用属性名
                    {
                        propColName = entityProp.Property.Name;
                    }

                    if (drSource.Table.Columns.Contains(propColName))
                    {

                        object objValue = ValueConverter.ConvertValue(entityProp, drSource[propColName]);
                        try
                        {

                            
                            entityProp.Property.SetValue(sourceEntity, objValue, null);
                            //赋值
                            //AssignFieldValue(sourceEntity, entityProp, drSource.Table.Columns[propColName].DataType, drSource[propColName]);
                        }
                        catch (Exception)
                        {
                            //System.IO.File.AppendAllText(@"c:\temp.txt", string.Format("colname={0},datatype={1},valueToAssign={2},valuetype={3},sourcevalue={4},sourcevaluetype={5}"
                            //                                                        , propColName, entityProp.Property.PropertyType, objValue, objValue.GetType(), drSource[propColName], drSource.Table.Columns[propColName].DataType));
                            throw;
                        }
                    }
                }
            }

            if (sourceEntity is BaseEntity)
            {
                (sourceEntity as BaseEntity).EnablePropertiesValueChangedLog = true;
            }

            return sourceEntity;
        }

        /// <summary>
        /// 字段赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="prop"></param>
        /// <param name="col"></param>
        /// <param name="objValue"></param>
        static void AssignFieldValue(object entity, EntityPropertyInfo entityProp, Type sourceDataType, object objValue)
        {
            //可空类型与非可空类型分开处理
            //判断此字段数据类型是否为可空类型(泛型并且泛型属性为System.Nullable<>)
            if (entityProp.Property.PropertyType.IsGenericType
                && entityProp.Property.PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
            {
                if (objValue == null || objValue == DBNull.Value)
                {
                    //当前值为空值
                    entityProp.SetValueWithConverter(entity, null, null);
                }
                else
                {
                    //获取泛型的数据类型 例如：int?为int； DateTime?为DateTime
                    Type propDataType = entityProp.Property.PropertyType.GetGenericArguments()[0];

                    if (propDataType == sourceDataType)//如果数据类型与datatable对应的列一致则直接填充
                    {
                        entityProp.SetValueWithConverter(entity, objValue, null);
                    }
                    else
                    {
                        //否则，数据转换后再填充
                        object objContertedValue = DataConverter.ConvertData(objValue, sourceDataType, propDataType);
                        entityProp.SetValueWithConverter(entity, objContertedValue, null);
                    }
                }
            }
            else
            {
                //此属性的类型
                Type propDataType = entityProp.Property.PropertyType;

                object objValueConvertered = objValue;
                if (propDataType != sourceDataType)//如果数据类型与datatable对应的列数据类型一致
                {
                    objValueConvertered = DataConverter.ConvertData(objValue, sourceDataType, propDataType);
                }

                if (propDataType.IsValueType)//如果此类型为值类型
                {
                    if (objValueConvertered != null && objValueConvertered != DBNull.Value)//并且将要填充的值不为空
                    {
                        entityProp.SetValueWithConverter(entity, objValueConvertered, null);
                    }
                    else
                    {
                        //否则不能填充
                    }
                }
                else//引用类型
                {
                    if (objValueConvertered == DBNull.Value)
                    {
                        entityProp.SetValueWithConverter(entity, null, null);//为DBNull.Value则填充null
                    }
                    else
                    {
                        //否则：直接填充
                        entityProp.SetValueWithConverter(entity, objValueConvertered, null);
                    }
                }


                ////此属性的类型
                //Type propDataType = prop.PropertyType;
                //if (propDataType == col.DataType)//如果数据类型与datatable对应的列数据类型一致
                //{
                //    if (propDataType.IsValueType)//如果此类型为值类型
                //    {
                //        if (objValue != null && objValue != DBNull.Value)//并且将要填充的值不为空
                //        {
                //            prop.SetValue(entity, objValue, null);
                //        }
                //        else
                //        {
                //            //否则不能填充
                //        }
                //    }
                //    else//引用类型
                //    {
                //        if (objValue == DBNull.Value)
                //        {
                //            prop.SetValue(entity, null, null);//为DBNull.Value则填充null
                //        }
                //        else
                //        {
                //            //否则：直接填充
                //            prop.SetValue(entity, objValue, null);
                //        }
                //    }
                //}
                //else//数据类型不一致，转换后再填充
                //{
                //    object objContertedValue = DataConverter.ConvertData(objValue, col.DataType, propDataType);
                //    prop.SetValue(entity, objContertedValue, null);
                //}
            }
        }

        /// <summary>
        /// 单实体转DataTable
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DataTable EntityToDataTable<TEntity>(TEntity entity) where TEntity : class
        {
            //if (entity == null)
            //{
            //    return null;
            //}

            //EntityAnaliser ea = new EntityAnaliser();

            ////获取实体信息
            //EntityInfo ei = ea.GetEntityInfo(typeof(TEntity));

            //if (ei == null)
            //{
            //    return null;
            //}

            //DataTable dt = ei.DataTableSchema;

            //DataRow row = EntityToDataRow(entity, dt);

            //dt.Rows.Add(row);

            //return dt;

            return EntityListToDataTable<TEntity>(new List<TEntity>() { entity });
        }

        public static DataTable EntityListToDataTable<TEntity>(IEnumerable<TEntity> list) where TEntity : class
        {
            EntityAnaliser ea = new EntityAnaliser();

            //获取实体信息
            EntityInfo ei = ea.GetEntityInfo(typeof(TEntity));

            DataTable table = ei.DataTableSchema;

            foreach (TEntity entity in list)
            {
                DataRow row = EntityToDataRow<TEntity>(entity, table, ei);
                table.Rows.Add(row);
            }

            return table;
        }

        private static DataRow EntityToDataRow<TEntity>(TEntity entity, DataTable table, EntityInfo entityInfo) where TEntity : class
        {
            DataRow row = table.NewRow();
            foreach (EntityPropertyInfo field in entityInfo.Fields)
            {
                string colName;

                if (field.IsDBField)
                {
                    colName = field.MapAttribute.DBColumnName;
                    //colName = field.MapAttribute.DBColumnName;
                }
                else
                {
                    colName = field.Property.Name;
                }

                object val = field.GetValueWithConverter(entity, null);

                if (val == null)
                {
                    val = DBNull.Value;
                }
                row[colName] = val;

            }
            return row;
        }
    }
}
