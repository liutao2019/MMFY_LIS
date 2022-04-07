using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Lib.DataInterface
{
    class WSDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public WSDataInterfaceCommand()
        {
            base.Parameters = (new WSDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;

        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            WSDataInterfaceParameter p = new WSDataInterfaceParameter();
            p.Name = paramName;
            return p;
        }

        object Execute()
        {
            Type tp;
            WSDataInterfaceConnection _conn = this.Connection as WSDataInterfaceConnection;
            object objInstance = _conn.GetInvokeObject(out tp);
            MethodInfo mi = tp.GetMethod(this.CommandText);
            WSDataInterfaceParameter returnParma = null;

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

        object[] PrepareParameter(out WSDataInterfaceParameter returnParma)
        {
            List<object> listParam = new List<object>();
            returnParma = null;

            foreach (WSDataInterfaceParameter item in this.Parameters)
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

        public new WSDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as WSDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as WSDataInterfaceParameterCollection;
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
