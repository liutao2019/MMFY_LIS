using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Lib.DataInterface
{
    class NetDllDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public NetDllDataInterfaceCommand()
        {
            base.Parameters = (new NetDllDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;
        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            NetDllDataInterfaceParameter p = new NetDllDataInterfaceParameter();
            p.Name = paramName;
            return p;
        }

        object Execute()
        {
            object result = null;
            NetDllDataInterfaceParameter returnParma = null;
            using (NetDllLoader invoker = new NetDllLoader(this.Connection.ServerAddress))
            {
                object[] listParam = PrepareParameter(out returnParma);

                object[] objOutput = new object[0];
                result = invoker.Invoke(this.Connection.Catelog, this.CommandText, listParam, out objOutput);

                int pIndex = 0;
                foreach (NetDllDataInterfaceParameter p in this.Parameters)
                {
                    if (p.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                    {
                        if (result == null)
                        {
                            returnParma.Value = null;
                        }
                        else
                        {
                            Type destType = Type.GetType(DataTypeUtil.GetNetTypeFullName(returnParma.DataType));
                            if (result.GetType() == destType)
                            {
                                returnParma.Value = result;
                            }
                            else
                            {
                                returnParma.Value = Convert.ChangeType(result, destType);
                            }
                        }
                    }
                    else if (p.Direction != EnumDataInterfaceParameterDirection.Input)
                    {
                        object objVal = objOutput[pIndex];
                        Type destType = Type.GetType(DataTypeUtil.GetNetTypeFullName(p.DataType));
                        if (objVal.GetType() == destType)
                        {
                            p.Value = objVal;
                        }
                        else
                        {
                            p.Value = Convert.ChangeType(result, destType);
                        }
                        pIndex++;
                    }
                }
            }

            if (returnParma != null)
            {
                return returnParma.Value;
            }
            else
            {
                return result;
            }
        }

        public override int ExecuteNonQuery()
        {
            object result = Execute();
            return -1;
        }

        public override DataTable ExecuteGetDataTable()
        {
            object result = Execute();
            if (result is DataTable)
            {
                return (DataTable)result;
            }
            else
            {
                string value = result == null ? "<null>" : result.ToString();
                throw new Exception(string.Format("[{0}]转换为System.DataTable失败", value));
            }
        }

        //private DataTable ConvertToDataTable(object input, string converter)
        //{
        //    Type tConverter = Type.GetType(converter);
        //    object objConverter = Activator.CreateInstance(tConverter);
        //    MethodInfo mi = tConverter.GetMethod("ConvertFrom");
        //    object ret = mi.Invoke(objConverter, new object[] { input });
        //    DataTable table = (DataTable)ret;
        //    return table;
        //}

        object[] PrepareParameter(out NetDllDataInterfaceParameter returnParma)
        {
            List<object> listParam = new List<object>();
            returnParma = null;

            foreach (NetDllDataInterfaceParameter item in this.Parameters)
            {
                if (item.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                {
                    returnParma = item;
                }
                else
                {
                    object objP = DataTypeUtil.ChangeType(item.Value, item.DataType);
                    listParam.Add(objP);
                }
            }
            return listParam.ToArray();
        }

        public new NetDllDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as NetDllDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as NetDllDataInterfaceParameterCollection;
            }
        }

        public override DataSet ExecuteGetDataSet()
        {
            object result = Execute();
            if (result is DataSet)
            {
                return (DataSet)result;
            }
            else if (result is DataTable)
            {
                DataTable table = (DataTable)result;
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                return ds;
            }
            else
            {
                string value = result == null ? "<null>" : result.ToString();
                throw new Exception(string.Format("[{0}]转换为System.DataTable失败", value));
            }
        }

        public override object ExecuteScalar()
        {
            object result = Execute();
            return result;
        }
    }
}
