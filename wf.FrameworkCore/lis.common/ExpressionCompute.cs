using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using Lib.LogManager;

namespace dcl.common
{
    /// <summary>
    /// 表达式计算
    /// </summary>
    public class ExpressionCompute
    {
        /// <summary>
        /// 生成可以动态编译的类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        static string GenerateCode(string expression)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public double OutPut()");


            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            if (expression.Contains("return"))
            {
                sb.Append(expression);
            }
            else
            {
                sb.Append("            double med= ");
                sb.Append(expression);
                sb.Append("; return med;");
            }
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object CalExpression(string expression)
        {
            try
            {

                CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
                ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

                CompilerParameters objCompilerParameters = new CompilerParameters();
                objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;
                CompilerResults cr;

                cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(expression));


                if (!cr.Errors.HasErrors)
                {
                    Type type = cr.CompiledAssembly.GetType("DynamicCodeGenerate.HelloWorld");
                    object obj = Activator.CreateInstance(type);
                    MethodInfo meth = type.GetMethod("OutPut");
                    return meth.Invoke(obj, null);

                }
            }
            catch (Exception ex)
            {
                //lib.Logger.Logger.WriteException("dcl.common.ExpressionCompute", "计算项目计算错误", expression + Environment.NewLine + ex.Message);
                Logger.LogException("计算项目计算错误", ex);

            }
            return null;
        }
    }
}
