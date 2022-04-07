using dcl.entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace dcl.entity
{
    public static class EntityManager<T>
        where T : new()
    {
        private static string DbName = ConfigurationManager.AppSettings["DCL.ExtDataInterface"];

        /// <summary>
        /// 数据转换成实体集合
        /// </summary>
        /// <param name="dtSour">数据集</param>
        /// <returns></returns>
        public static List<T> ConvertToList(DataTable dtSour)
        {
            List<T> list = new List<T>();
            if (dtSour != null && dtSour.Rows.Count > 0)
            {
                foreach (DataRow item in dtSour.Rows)
                {
                    list.Add(ConvertToEntity(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 数据转换成实体集合
        /// </summary>
        /// <param name="dtSour">原始数据</param>
        /// <param name="listContrast">数据对照信息</param>
        /// <returns></returns>
        public static List<T> ConvertToList(DataTable dtSour, List<EntitySysItfContrast> listContrast)
        {
            //数据转换到检验表中
            DataTable dtResult = new DataTable();
            foreach (EntitySysItfContrast item in listContrast)
            {
                if (!string.IsNullOrEmpty(item.ContInterfaceColumn))
                {
                    dtResult.Columns.Add(item.ContSysColumn);
                }
            }

            //检验数据转换成实体
            List<T> list = new List<T>();
            foreach (DataRow hisRow in dtSour.Rows)
            {
                DataRow lisRow = dtResult.NewRow();

                foreach (EntitySysItfContrast item in listContrast)
                {
                    if (!string.IsNullOrEmpty(item.ContInterfaceColumn) &&
                        dtSour.Columns.Contains(item.ContInterfaceColumn))
                    {
                        lisRow[item.ContSysColumn] = hisRow[item.ContInterfaceColumn];
                    }
                }

                list.Add(ConvertToEntityByMapName(lisRow, "clab"));

            }

            return list;
        }

        /// <summary>
        /// 数据转换成实体
        /// </summary>
        /// <param name="drSour">原始数据</param>
        /// <returns></returns>
        public static T ConvertToEntity(DataRow drSour)
        {
            return ConvertToEntityByMapName(drSour, DbName);
        }

        /// <summary>
        /// 指定列名转换成实体
        /// </summary>
        /// <param name="drSour">原始数据</param>
        /// <param name="dbName">列名</param>
        /// <returns></returns>
        public static T ConvertToEntityByMapName(DataRow drSour, String dbName)
        {
            T entity = new T();

            PropertyInfo[] propertys = entity.GetType().GetProperties();

            foreach (PropertyInfo item in propertys)
            {
                var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    FieldMapAttribute map = (FieldMapAttribute)attribute;

                    string columnName = string.Empty;
                    if (dbName.Contains("clab"))
                        columnName = map.ClabName;
                    if (dbName.Contains("med"))
                        columnName = map.MedName;
                    if (dbName.Contains("wf"))
                        columnName = map.WFName;

                    if (!string.IsNullOrEmpty(columnName))
                    {
                        if (drSour.Table.Columns.Contains(columnName) &&
                            drSour[columnName] != DBNull.Value &&
                           !string.IsNullOrEmpty(drSour[columnName].ToString())
                            )
                        {
                            string value = drSour[columnName].ToString();
                            if (item.PropertyType == typeof(DateTime) || item.PropertyType == typeof(DateTime?))
                                item.SetValue(entity, Convert.ToDateTime(value), null);
                            else if (item.PropertyType == typeof(Int32) || item.PropertyType == typeof(Int32?))
                                item.SetValue(entity, Convert.ToInt32(value), null);
                            else if (item.PropertyType == typeof(Int64) || item.PropertyType == typeof(Int64?))
                                item.SetValue(entity, Convert.ToInt64(value), null);
                            else if (item.PropertyType == typeof(Decimal) || item.PropertyType == typeof(Decimal?))
                                item.SetValue(entity, Convert.ToDecimal(value), null);
                            else if (item.PropertyType == typeof(Double) || item.PropertyType == typeof(Double?))
                                item.SetValue(entity, Convert.ToDouble(value), null);
                            else if (item.PropertyType == typeof(Single))
                                item.SetValue(entity, Convert.ToSingle(value), null);
                            else if (item.PropertyType == typeof(Boolean))
                            {
                                if (value == "1")
                                    item.SetValue(entity, true, null);
                                else if (value.ToLower() == "true")
                                    item.SetValue(entity, true, null);
                                else
                                    item.SetValue(entity, false, null);
                            }
                            else if (item.PropertyType == typeof(Byte[]))
                                item.SetValue(entity, (byte[])drSour[columnName], null);
                            else
                                item.SetValue(entity, value, null);
                        }
                        else
                        {
                            //给予String类型初始值，不让null值存在。
                            if (item.PropertyType == typeof(String))
                                item.SetValue(entity, string.Empty, null);
                        }
                    }


                }
            }


            return entity;
        }

        /// <summary>
        /// 实体转换成数据
        /// </summary>
        /// <param name="listSour">原始数据</param>
        /// <param name="mapName">转换的列名</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(List<T> listSour, String mapName = "")
        {
            DataTable dtResult = new DataTable("result");

            if (listSour.Count > 0)
            {
                PropertyInfo[] propertys = listSour[0].GetType().GetProperties();

                //生成列
                foreach (PropertyInfo item in propertys)
                {
                    var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                    if (attribute != null)
                    {
                        FieldMapAttribute map = (FieldMapAttribute)attribute;

                        string columnName = string.Empty;
                        if (mapName == string.Empty)
                            columnName = map.ClabName;
                        else
                        {
                            if (mapName.Contains("med"))
                                columnName = map.MedName;
                            else if (DbName.Contains("wf"))
                                columnName = map.WFName;
                            else
                                columnName = map.ClabName;
                        }

                        if (!dtResult.Columns.Contains(columnName))
                            dtResult.Columns.Add(columnName);
                    }
                }

                //添加值
                foreach (T entity in listSour)
                {
                    DataRow drResult = dtResult.NewRow();
                    foreach (PropertyInfo item in propertys)
                    {
                        var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                        if (attribute != null)
                        {
                            object value = item.GetValue(entity, null);
                            if (value != null)
                            {
                                FieldMapAttribute map = (FieldMapAttribute)attribute;

                                string columnName = string.Empty;
                                if (mapName == string.Empty)
                                    columnName = map.ClabName;
                                else
                                {
                                    if (mapName.Contains("med"))
                                        columnName = map.MedName;
                                    else if (DbName.Contains("wf"))
                                        columnName = map.WFName;
                                    else
                                        columnName = map.ClabName;
                                }

                                drResult[columnName] = value.ToString();
                            }
                        }
                    }

                    dtResult.Rows.Add(drResult);
                }

            }

            return dtResult;
        }

        /// <summary>
        /// 深度克隆实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T EntityClone(object entity)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, entity);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }


        /// <summary>
        /// 深度克隆实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ListClone(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }


        public static List<T> ToList(DataTable dt)
        {
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count == 0) return list;
            DataTableEntityBuilder<T> eblist = DataTableEntityBuilder<T>.CreateBuilder(dt.Rows[0]);

            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (DataRow info in dt.Rows)
            {
                var ET = eblist.Build(info);

                //这段影响速度
                foreach (PropertyInfo item in propertys)
                {
                    if (item.PropertyType == typeof(String) && item.GetValue(ET, null) == null)
                        item.SetValue(ET, string.Empty, null);
                }

                list.Add(ET);
            }
            dt.Dispose();
            dt = null;
            return list;
        }


    }

    public class DataTableEntityBuilder<T>
    {
        private static string DbName = ConfigurationManager.AppSettings["DCL.ExtDataInterface"];
        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
        private delegate T Load(DataRow dataRecord);

        private Load handler;
        private DataTableEntityBuilder() { }

        public T Build(DataRow dataRecord)
        {
            return handler(dataRecord);
        }

        public static DataTableEntityBuilder<T> CreateBuilder(DataRow dataRow)
        {
            DataTableEntityBuilder<T> dynamicBuilder = new DataTableEntityBuilder<T>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(T), new Type[] { typeof(DataRow) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            //var propertys = entity.GetType().GetProperties();

            for (int index = 0; index < dataRow.ItemArray.Length; index++)
            {
                string colname = GetEntityName(dataRow.Table.Columns[index].ColumnName, dataRow.Table.Columns[index].DataType);
                if (string.IsNullOrEmpty(colname)) continue;
                PropertyInfo propertyInfo = typeof(T).GetProperty(colname);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, index);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, index);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
        static Dictionary<string, PropertyInfo[]> Cache = new Dictionary<string, PropertyInfo[]>();
        private static string GetEntityName(string columnName, Type datatype)
        {
            PropertyInfo[] propertys;


            if (!Cache.Keys.Contains(typeof(T).FullName))
            {
                propertys = typeof(T).GetProperties();
                Cache.Add(typeof(T).FullName, propertys);
            }
            else
            {
                propertys = Cache[typeof(T).FullName];
            }

            foreach (PropertyInfo propertyInfo in propertys)
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();

                if (attribute == null) continue;

                FieldMapAttribute map = (FieldMapAttribute)attribute;

                string entityColName = string.Empty;

                if (DbName.Contains("med"))
                    entityColName = map.MedName;
                else if (DbName.Contains("wf"))
                    entityColName = map.WFName;
                else
                    entityColName = map.ClabName;


                //实体跟数据库数据类型会导致无法获取结果,请知悉
                if (entityColName.ToLower() == columnName.ToLower()
                    && (propertyInfo.PropertyType == datatype
                    || propertyInfo.PropertyType.FullName.Contains(datatype.Name)
                    || (propertyInfo.PropertyType.FullName.ToLower().Contains("int")) && datatype.FullName.ToLower().Contains("int")))
                    return propertyInfo.Name;
            }

            return string.Empty;
        }
    }
}