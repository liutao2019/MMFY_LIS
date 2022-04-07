using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Diagnostics;

namespace Lib.DataInterface
{
    class DOSCmdDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public DOSCmdDataInterfaceCommand()
        {
            base.Parameters = (new DOSCmdDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;
        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            DOSCmdDataInterfaceParameter p = new DOSCmdDataInterfaceParameter();
            p.Name = paramName;
            return p;
        }

        public override int ExecuteNonQuery()
        {
            if (string.IsNullOrEmpty(this.CommandText))
                return -1;

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            string executeString = this.CommandText;

            foreach (DOSCmdDataInterfaceParameter par in this.Parameters)
            {
                string val;
                if (par.Value == null)
                    val = string.Empty;
                else
                    val = par.Value.ToString();

                executeString = executeString.Replace(par.Name, val);
            }

            string[] str = executeString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in str)
            {
                p.StandardInput.WriteLine(item);
            }
            //p.StandardInput.WriteLine(@"D:\DEV\Lib.DataInterface\bin\DemoWinApp.exe");

            p.StandardInput.WriteLine("exit");

            return -1;
        }

        public override DataTable ExecuteGetDataTable()
        {
            throw new NotSupportedException();
        }

        public new DOSCmdDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as DOSCmdDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as DOSCmdDataInterfaceParameterCollection;
            }
        }

        public override DataSet ExecuteGetDataSet()
        {
            throw new NotSupportedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotSupportedException();
        }
    }
}
