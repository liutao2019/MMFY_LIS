using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Lib.DataInterface
{
    class BinDllDataInterfaceCommand : AbstractDataInterfaceCommand
    {
        public BinDllDataInterfaceCommand()
        {
            base.Parameters = (new BinDllDataInterfaceParameterCollection()) as IDataInterfaceParameterCollection;
        }

        public override IDataInterfaceParameter CreateParameter(string paramName)
        {
            BinDllDataInterfaceParameter p = new BinDllDataInterfaceParameter();
            p.Name = paramName;
            return p;
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

        object Execute()
        {
            object result = null;
            BinDllDataInterfaceParameter returnParma = null;

            int hashcode = this.GetHashCode();
            MethodInfo mi = BinDllLoader.Current.Get(hashcode);

            if (mi == null)
            {
                mi = BinDllLoader.CreateMethodCode(this);
                BinDllLoader.Current.Put(hashcode, mi);
            }

            object[] listParam = PrepareParameter(out returnParma);

            result = mi.Invoke(null, listParam);

            object[] objOutput = new object[0];
            //result = invoker.Invoke(this.Connection.Catelog, this.CommandText, listParam, out objOutput);

            //int pIndex = 0;
            foreach (BinDllDataInterfaceParameter p in this.Parameters)
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
                    //object objVal = objOutput[pIndex];
                    //Type destType = Type.GetType(DataTypeUtil.GetTypeName(p.DataType));
                    //if (objVal.GetType() == destType)
                    //{
                    //    p.Value = objVal;
                    //}
                    //else
                    //{
                    //    p.Value = Convert.ChangeType(result, destType);
                    //}
                    //pIndex++;
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

        object[] PrepareParameter(out BinDllDataInterfaceParameter returnParma)
        {
            List<object> listParam = new List<object>();
            returnParma = null;

            foreach (BinDllDataInterfaceParameter item in this.Parameters)
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

        public new BinDllDataInterfaceParameterCollection Parameters
        {
            get
            {
                return base.Parameters as BinDllDataInterfaceParameterCollection;
            }
            set
            {
                base.Parameters = value as BinDllDataInterfaceParameterCollection;
            }
        }

        //public  int GetHashCode1()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(this.CommandText + "|");
        //    sb.Append(this.CommandType + "|");
        //    sb.Append(this.Connection.Catelog + "|");
        //    sb.Append(this.Connection.DbDialet + "|");
        //    sb.Append(this.Connection.DbDriver + "|");
        //    sb.Append(this.Connection.LoginName + "|");
        //    sb.Append(this.Connection.LoginPassword + "|");
        //    sb.Append(this.Connection.ServerAddress + "|");

        //    foreach (BinDllDataInterfaceParameter item in this.Parameters)
        //    {
        //        sb.Append(item.DataType + "|");
        //        sb.Append(item.Direction + "|");
        //        sb.Append(item.Length + "|");
        //        sb.Append(item.Name + "|");
        //        sb.Append(item.ValueExpression + "|");
        //    }

        //    return sb.ToString().GetHashCode();
        //}
    }
}
