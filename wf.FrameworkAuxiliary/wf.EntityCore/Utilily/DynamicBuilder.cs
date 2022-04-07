using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection.Emit;
using System.Reflection;

namespace Lib.EntityCore
{
    public class DynamicBuilder<T>
    {
        private delegate T Load(DataRow row);

        private Load handler;//最终执行动态方法的一个委托 参数是DataRow接口

        private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(string) });

        private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });


        private static readonly MethodInfo getDataTypeConverterMethod = typeof(EntityPropertyInfo).GetMethod("get_DataTypeConverter", new Type[] { });

        public T Build(DataRow row)
        {
            return handler(row);//执行CreateBuilder里创建的DynamicCreate动态方法的委托
        }

        public static DynamicBuilder<T> CreateBuilder(DataRow row, EntityInfo ei)
        {
            DataTable parentTable = row.Table;

            DynamicBuilder<T> dynamicBuilder = new DynamicBuilder<T>();

            //定义一个名为DynamicCreate的动态方法，返回值typof(T)，参数typeof(DataRow)
            DynamicMethod method = new DynamicMethod("DynamicCreate", typeof(T), new Type[] { typeof(DataRow) }, typeof(T), true);

            ILGenerator generator = method.GetILGenerator();//创建一个MSIL生成器，为动态方法生成代码

            LocalBuilder result = generator.DeclareLocal(typeof(T));//声明指定类型的局部变量 可以T t;这么理解
            //The next piece of code instantiates the requested type of object and stores it in the local variable. 可以t=new T();这么理解
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < parentTable.Columns.Count; i++)
            {
                EntityPropertyInfo epi = ei.GetFieldInfoByPropertyDbColumnName(parentTable.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel();

                if (epi != null && epi.Property.GetSetMethod() != null)//实体存在该属性 且该属性有SetMethod方法
                {
                    //generator.Emit(OpCodes.Ldarg_1);
                    //generator.Emit(OpCodes.


                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);//调用IsDBNull方法 如果IsDBNull==true contine
                    generator.Emit(OpCodes.Brtrue, endIfLabel);

                    /*If the value in the data reader is not null, the code sets the value on the object.*/
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldstr, epi.MapAttribute.DBColumnName);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);//调用get_Item方法
                    generator.Emit(OpCodes.Unbox_Any, epi.Property.PropertyType);
                    generator.Emit(OpCodes.Callvirt, epi.Property.GetSetMethod());//给该属性设置对应值

                    generator.MarkLabel(endIfLabel);
                }
            }

            /*The last part of the code returns the value of the local variable*/
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);//方法结束，返回

            //完成动态方法的创建，并且创建执行该动态方法的委托，赋值到全局变量handler,handler在Build方法里Invoke
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}
