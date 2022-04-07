using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.svr.cache;
using System.Collections;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 效验规则检查
    /// </summary>
    public class CheckerClItem2 : AbstractAuditClass, IAuditChecker
    {
        public CheckerClItem2(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            Hashtable ht = new Hashtable();
            foreach (EntityObrResult dr in resulto)
            {
                if (!ht.ContainsKey(dr.ItmEname))
                {
                    ht.Add(dr.ItmEname, dr.ObrValue);
                }

            }
            DataSet result = GetEfficacy(ht);
            //String error = "";
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataRow drVali in dt.Rows)
                {
                    if (drVali["retu"].ToString() == "False")
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.Others, "经公式校验结果有异常：应该为" + drVali["cal_fmla"].ToString() + "\n\r", EnumOperationErrorLevel.Message);

                    }
                }
            }

        }

        //效验方法
        public DataSet GetEfficacy(Hashtable ht)
        {
            List<EntityDicItmCalu> list = new List<EntityDicItmCalu>();
            list =EntityManager< EntityDicItmCalu >.ListClone( DictClItemCache2.Current.GetAllData());
            return Variable(ht, list, 1);
        }

        //公用效验方法
        private  DataSet Variable(Hashtable ht, List<EntityDicItmCalu> listCalu, int cot)
        {
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("retu");
            foreach (EntityDicItmCalu ItmCalu in listCalu)
            {
                if (ItmCalu.CalVariable != "")
                {
                    string[] varpr = ItmCalu.CalVariable.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(ItmCalu.CalExpression, ItmCalu.CalFlag, ItmCalu.ItmId, ItmCalu.ItmEcode);

                    }
                }
            }

            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";

                    //string va = Convert.ToDouble(value[j]).ToString("0.0000");

                    double dValue = 0;
                    string va;
                    bool isSucc=double.TryParse(value[j], out dValue);

                    if (config.CheckerClItemDealNumOnly)
                    {
                        if (isSucc)
                        {
                            va = dValue.ToString("0.0000");
                        }
                        else
                        {
                            va = "0";
                        }
                    }
                    else
                    {
                        if (isSucc)
                        {
                            va = dValue.ToString("0.0000");
                        }
                        else
                        {
                            va = "\"" + value[j] + "\"";
                        }
                    }

                    methAll = methAll.Replace(fam, va);
                }


                if (cot == 1)
                {

                    CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

                    ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

                    CompilerParameters objCompilerParameters = new CompilerParameters();
                    objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                    objCompilerParameters.GenerateExecutable = false;
                    objCompilerParameters.GenerateInMemory = true;
                    CompilerResults cr;

                    cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, Efficacy(methAll));

                    if (cr.Errors.HasErrors)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        Assembly objAssembly = cr.CompiledAssembly;
                        object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                        MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
                        pb.Rows[i]["retu"] = objMI.Invoke(objHelloWorld, null).ToString();
                    }
                }
                else
                {

                    //DataTable dt = new DataTable();

                    //object objValue = dt.Compute(methAll, string.Empty);

                    //pb.Rows[i]["retu"] = .ToString();
                    //cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(methAll));


                    DataTable dt = new DataTable();

                    try
                    {
                        object objValue = dt.Compute(methAll, string.Empty);

                        decimal decVal = 0;

                        if (decimal.TryParse(objValue.ToString(), out decVal))
                        {
                            decVal = decimal.Round(decVal, 4);
                            pb.Rows[i]["retu"] = decVal.ToString();
                        }

                        //pb.Rows[i]["retu"] = .ToString();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }

            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }
        //效验动态编译
        private static string Efficacy(string meth)
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
            sb.Append("        public bool OutPut()");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            sb.Append("             if(");
            sb.Append(meth);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("     return true;");
            sb.Append(Environment.NewLine);
            sb.Append("   else");
            sb.Append(Environment.NewLine);
            sb.Append("     return false;");
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }
        #endregion
    }
}
