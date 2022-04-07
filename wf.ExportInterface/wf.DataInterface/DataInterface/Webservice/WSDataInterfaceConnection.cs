using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Description;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.CodeDom;
using Microsoft.CSharp;
using Lib.DAC;

namespace Lib.DataInterface
{
    class WSDataInterfaceConnection : AbstractDataInterfaceConnection
    {
        public override IDataInterfaceCommand CreateCommand()
        {
            WSDataInterfaceCommand cmd = new WSDataInterfaceCommand();
            return cmd;
        }


        public override SqlHelper GetSqlHelper()
        {
            throw new NotImplementedException();
        }

        public override bool TestConnection(out string errorMessage)
        {
            bool success = false;
            errorMessage = null;

            try
            {
                CreateWSAssembly();
                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return success;
        }

        Type CreateWSAssembly()
        {
            WebClient client = new WebClient();
            String url = this.ServerAddress;
            Stream stream = client.OpenRead(url);
            ServiceDescription description = ServiceDescription.Read(stream);
            stream.Close();

            if (description.Services.Count == 0)
            {
                throw new Exception(string.Format("\"{0}\" 没有定义服务。", this.ServerAddress));
            }

            string svcName = description.Services[0].Name;
            string svcNamespace = "DynClientProxy";// description.TargetNamespace;

            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();//创建客户端代理代理类。

            importer.ProtocolName = "Soap"; //指定访问协议。
            importer.Style = ServiceDescriptionImportStyle.Client; //生成客户端代理。
            //importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.CodeGenerationOptions = CodeGenerationOptions.None;

            importer.AddServiceDescription(description, null, null); //添加WSDL文档。

            CodeNamespace nmspace = new CodeNamespace(); //命名空间
            nmspace.Name = svcNamespace;
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);

            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            //CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v3.5" } });

            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = true;
            //parameter.OutputAssembly = "MyTest.dll";//输出程序集的名称
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");

            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            if (result.Errors.HasErrors)
            {
                // 显示编译错误信息
                throw new Exception("生成" + this.ServerAddress + "服务代理时出错");
            }

            Assembly asm = result.CompiledAssembly;

            Type originType = asm.GetType(svcNamespace + "." + svcName);

            //originType.InvokeMember("HelloWorld", BindingFlags.CreateInstance, 

            //object o = Activator.CreateInstance(originType);

            //MethodInfo mi = originType.GetMethod("HelloWorld");
            //object r = mi.Invoke(o, null);

            return originType;
        }

        public object GetInvokeObject(out Type invokeType)
        {
            object obj = null;
            invokeType = null;
            obj = WSInvokerCache.Current.GetInvokeInstance(this.ServerAddress);
            if (obj == null)
            {
                invokeType = CreateWSAssembly();
                obj = Activator.CreateInstance(invokeType);
                WSInvokerCache.Current.Put(this.ServerAddress, invokeType, obj);
            }
            else
            {
                invokeType = WSInvokerCache.Current.GetInvokeType(this.ServerAddress);
            }
            return obj;
        }
    }
}
