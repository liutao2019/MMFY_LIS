using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    abstract class AbstractDataInterfaceParameter : IDataInterfaceParameter
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public EnumDataInterfaceParameterDirection Direction { get; set; }
        public string DataType { get; set; }
        public object Value { get; set; }
        //public Lib.DataInterface.DataConverter.IInterfaceDataConverter Converter { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", this.Name, this.DataType);
        }

        public override int GetHashCode()
        {
            string str = string.Format("{0}_{1}_{2}_{ n3}"
                         , this.Name
                         , this.Length
                         , this.Direction
                         , this.DataType);

            return str.GetHashCode();
        }
    }
}
