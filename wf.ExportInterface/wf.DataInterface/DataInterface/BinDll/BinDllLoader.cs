using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Lib.DataInterface
{
    class BinDllLoader
    {
        public static MethodInfo CreateMethodCode(BinDllDataInterfaceCommand cmd)
        {
            string strCode = @"
    class DynBinDllLoader
    {
        [System.Runtime.InteropServices.DllImport(""" + cmd.Connection.ServerAddress + @""", EntryPoint = """ + cmd.CommandText + @""", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
";


            strCode += "        public extern static ";
            BinDllDataInterfaceParameter pReturn = cmd.Parameters.GetReturnValueParameter() as BinDllDataInterfaceParameter;
            if (pReturn == null)
            {
                strCode += "void ";
            }
            else
            {
                strCode += pReturn.DataType + " "; ;
            }

            strCode += cmd.CommandText + "(";

            bool needCommd = false;
            foreach (BinDllDataInterfaceParameter p in cmd.Parameters)
            {
                if (p.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                    continue;

                if (needCommd)
                    strCode += ", ";

                if (p.Direction == EnumDataInterfaceParameterDirection.InputOutput
                    || p.Direction == EnumDataInterfaceParameterDirection.Output)
                    strCode += "out ";

                if (p.Direction == EnumDataInterfaceParameterDirection.Reference)
                    strCode += "ref ";

                strCode += p.DataType + " " + p.Name;
                needCommd = true;
            }
            strCode += ");\r\n    }";
            //    public extern static int MessageBox(int hwnd, string text, string caption, int utype);
            //}";


            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");

            CompilerResults cResult = provider.CompileAssemblyFromSource(cp, strCode);

            if (cResult.Errors.HasErrors)
            {
                throw new Exception(cResult.Errors[0].ToString());
            }
            else
            {
                MethodInfo mi = cResult.CompiledAssembly.GetTypes()[0].GetMethods()[0];
                return mi;
                //mi.Invoke(null, new object[] { 0, "2", "3", 0 });
            }
        }

        static object padlock = new object();
        static BinDllLoader _instance = null;

        public static BinDllLoader Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BinDllLoader();
                        }
                    }
                }
                return _instance;
            }
        }

        BinDllLoader()
        {
            this._cache = new Dictionary<int, MethodInfo>();
        }

        Dictionary<int, MethodInfo> _cache = null;

        public MethodInfo Get(int hashcode)
        {
            if (this._cache.ContainsKey(hashcode))
            {
                return this._cache[hashcode];
            }
            else
            {
                return null;
            }
        }

        public void Put(int hashcode, MethodInfo mi)
        {
            if (this._cache.ContainsKey(hashcode))
            {
                this._cache[hashcode] = mi;
            }
            else
            {
                this._cache.Add(hashcode, mi);
            }
        }
    }
}