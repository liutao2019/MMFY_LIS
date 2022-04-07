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
using Lib.DAC;

namespace Lib.DataInterface
{
    class NetDllDataInterfaceConnection : AbstractDataInterfaceConnection
    {
        public override IDataInterfaceCommand CreateCommand()
        {
            NetDllDataInterfaceCommand cmd = new NetDllDataInterfaceCommand();
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
                if (string.IsNullOrEmpty(this.Catelog))
                {
                    throw new Exception(string.Format("没有指定类名，格式 命名空间.类名"));
                }
                else
                {

                    NetDllLoader loader = CreateDllLoader();
                    Type tp = loader.GetInvokeType(this.Catelog);
                    loader.Unload();

                    if (tp == null)
                        throw new Exception(string.Format("创建对象[{0}]失败", this.Catelog));
                }
                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return success;
        }

        public NetDllLoader CreateDllLoader()
        {
            NetDllLoader loader = new NetDllLoader(this.ServerAddress);
            return loader;
        }
    }
}
