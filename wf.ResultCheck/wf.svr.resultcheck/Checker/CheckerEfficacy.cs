using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using dcl.svr.cache;
using Microsoft.CSharp;
using dcl.entity;
using dcl.common;

namespace dcl.svr.resultcheck.Checker
{
    public class CheckerEfficacy : AbstractAuditClass, IAuditChecker
    {
        public CheckerEfficacy(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config, EntityRemoteCallClientInfo caller)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {
            Caller = caller;
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
            try
            {
                DataSet result = GetEfficacy(ht, pat_info);
                foreach (DataTable dt in result.Tables)
                {
                    foreach (DataRow drVali in dt.Rows)
                    {
                        if (drVali["retu"].ToString() == "True")
                        {
                            if (drVali["itm_ecd"] != null && drVali["itm_ecd"] != DBNull.Value &&
                                !string.IsNullOrEmpty(drVali["itm_ecd"].ToString()))
                            {

                                if (drVali["ei_name"] != null && !string.IsNullOrEmpty(drVali["ei_name"].ToString()))
                                {
                                    chkResult.AddMessage(EnumOperationErrorCode.Others,
                                                       string.Format("项目{0}关联判断结果有异常，内容为:{1}\n\r", drVali["itm_ecd"], drVali["ei_name"]),
                                                        EnumOperationErrorLevel.Message);
                                }
                                else
                                {
                                    chkResult.AddMessage(EnumOperationErrorCode.Others,
                                                       string.Format("项目{0}关联判断结果有异常，公式为:{1}\n\r", drVali["itm_ecd"], drVali["ei_fmla"]),
                                                        EnumOperationErrorLevel.Message);
                                }

                            }
                            else
                            {
                                if (drVali["ei_name"] != null && !string.IsNullOrEmpty(drVali["ei_name"].ToString()))
                                {
                                    chkResult.AddMessage(EnumOperationErrorCode.Others,
                                                         "经公式效验结果有异常,内容为:" + drVali["ei_name"].ToString() + "\n\r",
                                                         EnumOperationErrorLevel.Message);
                                }
                                else
                                {
                                    chkResult.AddMessage(EnumOperationErrorCode.Others,
                                                         "经公式效验结果有异常：公式为:" + drVali["ei_fmla"].ToString() + "\n\r",
                                                         EnumOperationErrorLevel.Message);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
               // Lib.LogManager.Logger.LogException(ex);
            }
        }

        //效验方法
        public DataSet GetEfficacy(Hashtable ht, EntityPidReportMain patInfo)
        {
            DataSet ds = new DataSet();
            List<EntityDicItmCheck> dtGroup =DictEfficacyGroupCache.Current.GetCacheData(patInfo.RepItrId);

            List<EntityDicItmCheckDetail> dtEfficacy = DictEffcacyItemCache.Current.GetCacheData(patInfo.RepItrId, Caller.UseAuditRule);

            if (dtGroup.Count == 0||dtEfficacy.Count == 0) return ds;
            Stopwatch sw = new Stopwatch();
            sw.Start();
             DataSet dsRe=  Variable(ht, patInfo, dtGroup, dtEfficacy);
             sw.Stop();
            return dsRe;
        }

        //公用效验方法
        private static DataSet Variable(Hashtable ht, EntityPidReportMain patInfo, List<EntityDicItmCheck> dtGroup, List<EntityDicItmCheckDetail> dtEfficacy)
        {
            DataTable dtPatColumn = CommonValue.GetEfficacyPatients();
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("ei_fmla");
            pb.Columns.Add("ei_name");
            pb.Columns.Add("ei_type");
            pb.Columns.Add("ei_variable");
            pb.Columns.Add("itm_ecd");
            pb.Columns.Add("retu");

            bool isGroupPass = false;
        
            foreach (EntityDicItmCheck row in dtGroup)
            {
                bool isFirstSet = true;
                List<EntityDicItmCheckDetail> rows = dtEfficacy.Where(w=>w.CheckIdDetial==row.CheckId).ToList();
                foreach (EntityDicItmCheckDetail dr in rows)
                {
                    if (dr.CheckVariable != "")
                    {
                        string[] varpr = dr.CheckVariable.Split(',');
                        if (dr.CheckTypeDetail == "1")
                        {
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
                                if (isFirstSet)
                                {
                                    isGroupPass = true;
                                    isFirstSet = false;
                                }
                                SetReturnValue(ht, patInfo, pb, dr, parm, value, dtPatColumn, ref isGroupPass);
                            }
                        }
                        if (dr.CheckTypeDetail == "2")
                        {
                            if (isFirstSet)
                            {
                                isGroupPass = true;
                                isFirstSet = false;
                            }
                            SetReturnValue(ht, patInfo, pb, dr, parm, value, dtPatColumn, ref isGroupPass);
                        }
                    }
                }

                //仪器下不同组的关系为：或 有一个组未通过，则不再效验其它组规则
                if (!isGroupPass)
                {
                    DataSet dss = new DataSet();
                    dss.Tables.Add(pb);
                    return dss;
                }
            }
            return new DataSet();
        }

        private static bool SetReturnValue(Hashtable ht, EntityPidReportMain patInfo, DataTable pb, EntityDicItmCheckDetail dr, string[] parm,
                                           string[] value, DataTable dtPatColumn, ref bool isGroupPass)
        {
            if (!string.IsNullOrEmpty(dr.ItmEcode)  &&
                  !string.IsNullOrEmpty(dr.ItmEcode) && !ht.ContainsKey(dr.ItmEcode))
            {
                return false;
            }
            pb.Rows.Add(dr.CheckExpression, dr.CheckNameDetail, dr.CheckTypeDetail, dr.CheckVariable, dr.ItmEcode, "");
            string retValue = GetEfficacyValue(ht, patInfo, pb.Rows[pb.Rows.Count - 1], parm, value, dtPatColumn);
            if (retValue == "True")
            {
                isGroupPass = false;
                pb.Rows[pb.Rows.Count - 1]["retu"] = "True";
                return true;
            }
            return false;
        }

        private static string GetEfficacyValue(Hashtable ht, EntityPidReportMain patInfo, DataRow pbRow, string[] parm,
                                             string[] value, DataTable dtPatColumn)
        {
            string methAll = pbRow["ei_fmla"].ToString();
            string eiType = pbRow["ei_type"].ToString();
            if (eiType == "1")
            {
                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";
                    double dValue = 0;
                    string va;
                    bool isSucc = double.TryParse(value[j], out dValue);

                    if (isSucc)
                    {
                        va = dValue.ToString("0.0000");
                    }
                    else
                    {
                        va = "\"" + value[j] + "\"";
                    }

                    methAll = methAll.Replace(fam, va);
                }
            }
            if (eiType == "2")
            {
                string[] varpr = pbRow["ei_variable"].ToString().Split(',');
                Type t = typeof (EntityPidReportMain);
                for (int j = 0; j < varpr.Length; j++)
                {
                    DataRow[] rows = dtPatColumn.Select(string.Format("column_id='{0}'", varpr[j]));
                    if (rows.Length == 0) continue;
                    object obj = t.GetProperty(varpr[j]).GetValue(patInfo, null);
                    string va = obj == null ? "" : obj.ToString();

                    double dValue = 0;
                    bool isSucc = double.TryParse(va, out dValue);

                    if (isSucc)
                    {
                        va = dValue.ToString("0.0000");
                    }
                    else
                    {
                        va = "\"" + va + "\"";
                    }

                    string fam = "[" + rows[0]["column_name"] + "]";
                    methAll = methAll.Replace(fam, va);
                }
            }

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
            Assembly objAssembly = cr.CompiledAssembly;
            object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
            MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
            return objMI.Invoke(objHelloWorld, null).ToString();
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
