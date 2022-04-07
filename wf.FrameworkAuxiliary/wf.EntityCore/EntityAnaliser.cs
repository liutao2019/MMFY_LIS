using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Lib.EntityCore
{
    /// <summary>
    /// 实体分析器
    /// </summary>
    public class EntityAnaliser
    {
        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public EntityInfo GetEntityInfo(Type type)
        {
            EntityInfo entityInfo = null;
            if (!EntityInfoCache.Current.Exist(type))
            {
                entityInfo = Analise(type);
                EntityInfoCache.Current.Put(type, entityInfo);
            }
            else
            {
                entityInfo = EntityInfoCache.Current.Get(type);
            }
            return entityInfo;
        }

        /// <summary>
        /// 分析实体
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private EntityInfo Analise(Type type)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return null;
            }

            //获取"实体-表"影射属性
            IEntityAttribute[] tableattributes = type.GetCustomAttributes(typeof(IEntityAttribute), false) as IEntityAttribute[];

            EntityInfo entityInfo = new EntityInfo();

            if (tableattributes.Length == 0)
            {
                EntityTableAttribute tbAttr = new EntityTableAttribute();
                tbAttr.DisplayName = tbAttr.TableName = type.Name;
                entityInfo.AddAttribute(tbAttr);
            }
            else
            {
                entityInfo.AddAttributes(tableattributes);
            }
            entityInfo.TypeEntity = type;

            bool isDeleteFlagEntity = entityInfo.GetAttribute<EntityDeleteWithFlagAttribute>() != null;
            bool hasDeleteFlagProp = false;

            //遍历实体每一个属性Property
            foreach (PropertyInfo prop in type.GetProperties())
            {
                //获取字段定义的属性
                IEntityPropertyAttribute[] fieldAttrs = prop.GetCustomAttributes(typeof(IEntityPropertyAttribute), true) as IEntityPropertyAttribute[];
                EntityPropertyInfo fi = new EntityPropertyInfo();
                fi.Property = prop;
                fi.AddAttributes(fieldAttrs);

                ////获取映射属性
                //FieldMapAttribute filedMap = fi.GetAttribute<FieldMapAttribute>();
                //if (filedMap != null)
                //{
                //    fi.MapAttribute = filedMap;
                //}

                //检查各个attribute是否符合规范
                foreach (IEntityPropertyAttribute item in fieldAttrs)
                {
                    #region 检查字段类型是否符合要求
                    if (item.AllowPropertyTypes != null)
                    {
                        bool typeInDefTypes = false;
                        string allowTypes = string.Empty;
                        bool needComma = false;
                        foreach (Type t in item.AllowPropertyTypes)
                        {
                            if (needComma)
                            {
                                allowTypes += ",";
                            }
                            allowTypes += t;

                            if (t == prop.PropertyType
                                ||
                                (prop.PropertyType.IsGenericType
                                 && prop.PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>)
                                 && prop.PropertyType.GetGenericArguments()[0] == t
                                 )
                                )
                            {
                                typeInDefTypes = true;
                                continue;
                            }

                            needComma = true;
                        }

                        if (typeInDefTypes == false)
                        {
                            throw new Exception(string.Format("属性[{0}]允许的类型为{1}", prop.Name, allowTypes));
                        }
                    }

                    #endregion

                    #region 检查标签并存情况
                    if (item.NotAllowExistAttributeTypes != null)
                    {
                        foreach (IEntityPropertyAttribute item2 in fieldAttrs)
                        {
                            if (item.NotAllowExistAttributeTypes.Contains(item2.GetType()))
                            {
                                throw new Exception(string.Format("属性[{0}]不允许{1}与{2}并存", prop.Name, item.GetType().Name, item2.GetType().Name));
                            }
                        }
                    }

                    if (item.AttributeTypesNeeded != null)
                    {
                        bool b = false;
                        StringBuilder sb = new StringBuilder();

                        foreach (IEntityPropertyAttribute item2 in fieldAttrs)
                        {
                            if (item.AttributeTypesNeeded.Contains(item2.GetType()))
                            {
                                b = true;
                                break;
                            }
                        }

                        if (b == false)
                        {
                            bool needComma = false;
                            foreach (Type typeNeeded in item.AttributeTypesNeeded)
                            {
                                if (needComma)
                                {
                                    sb.Append(",");
                                }
                                sb.Append(string.Format("[{0}]", typeNeeded.Name));
                                needComma = true;
                            }

                            throw new Exception(string.Format("属性[{0}]当使用[{1}]标签时必须与{2}同时使用", prop.Name, item.GetType().Name, sb));
                        }
                    }
                    #endregion

                    #region 检查是否有删除字段标志
                    //检查是否有删除字段标志
                    if (isDeleteFlagEntity && hasDeleteFlagProp == false)
                    {
                        if (item is FieldDeleteFlagAttribute)
                        {
                            hasDeleteFlagProp = true;
                        }
                    }
                    #endregion

                    //FieldMapAttribute.IsDBGenerate == true不能跟FieldDataCustomGenerateAttribute并存
                    //if (fi.MapAttribute != null && fi.MapAttribute.IsDBGenerate == true
                    //    && item is FieldDataCustomGenerateAttribute)
                    //{
                    //    throw new Exception(string.Format("当[{0}.IsDBGenerate]为true时，不能与[{1}]并存", typeof(FieldMapAttribute).Name, typeof(FieldDataCustomGenerateAttribute).Name));
                    //}
                }
                entityInfo.Fields.Add(fi);
            }

            //如果实体指定为删除时置删除标志，判断是否有删除标志字段
            if (isDeleteFlagEntity == true && hasDeleteFlagProp == false)
            {
                throw new Exception("当实体指定为删除时置删除标志，请指定删除标志字段");
            }

            return entityInfo;
        }
    }
}
