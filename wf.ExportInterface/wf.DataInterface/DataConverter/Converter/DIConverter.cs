using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.DataConverter
{
    /// <summary>
    /// 接口数据转换
    /// </summary>
    class DIConverter : IInterfaceDataConverter
    {
        public DataInterfaceCommand ConvertCommand { get; set; }

        public DIConverter()
        {
            this.ConvertCommand = null;
        }

        public object ConvertTo(object input)
        {
            if (this.ConvertCommand == null)
                return input;

            foreach (DataInterfaceParameter item in this.ConvertCommand.Parameters)
            {
                if (item.Direction == EnumDataInterfaceParameterDirection.Input
                    || item.Direction == EnumDataInterfaceParameterDirection.InputOutput)
                {
                    item.Value = input;
                    break;
                }
            }

            DataInterfaceParameter retparam = this.ConvertCommand.Parameters.GetReturnValueParameter();
            if (retparam != null)
            {
                object ret = this.ConvertCommand.ExecuteScalar();
                return ret;
            }
            else
            {
                this.ConvertCommand.ExecuteNonQuery();
                foreach (DataInterfaceParameter item in this.ConvertCommand.Parameters)
                {
                    if (item.Direction == EnumDataInterfaceParameterDirection.Output
                        || item.Direction == EnumDataInterfaceParameterDirection.InputOutput)
                    {
                        return item.Value;
                    }
                }
                throw new Exception("转换规则未指定Output属性或ReruenValue属性参数");
            }
        }
    }
}
