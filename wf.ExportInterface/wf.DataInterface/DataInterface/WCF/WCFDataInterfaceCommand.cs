using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Lib.DataInterface
{
    class WCFDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public WCFDataInterfaceCommand()
        {
            base.Parameters = (new WCFDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;

        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            WCFDataInterfaceParameter p = new WCFDataInterfaceParameter();
            p.Name = paramName;
            return p;
        }

        object Execute()
        {
            Type tp;
            WCFDataInterfaceConnection _conn = this.Connection as WCFDataInterfaceConnection;
            object objInstance = _conn.GetInvokeObject(out tp);
            MethodInfo mi = tp.GetMethod(this.CommandText);
            WCFDataInterfaceParameter returnParma = null;

            object[] listParam = PrepareParameter(out returnParma);
            object result = mi.Invoke(objInstance, listParam);

            if (returnParma != null)
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
            Execute();
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
        //    throw new NotImplementedException();
        //}

        object[] PrepareParameter(out WCFDataInterfaceParameter returnParma)
        {
            List<object> listParam = new List<object>();
            returnParma = null;

            foreach (WCFDataInterfaceParameter item in this.Parameters)
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

        public new WCFDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as WCFDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as WCFDataInterfaceParameterCollection;
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
