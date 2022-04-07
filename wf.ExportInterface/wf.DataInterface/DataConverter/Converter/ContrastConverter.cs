using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface.DataConverter
{
    /// <summary>
    /// 对照转换
    /// </summary>
    class ContrastConverter : IInterfaceDataConverter
    {
        public List<DataConverterConstrstItem> ConstrstItems { get; set; }

        public string SourceDataType { get; set; }
        public string DestDataType { get; set; }

        public ContrastConverter()
        {
            this.ConstrstItems = new List<DataConverterConstrstItem>();
            this.SourceDataType = "System.String";
            this.DestDataType = "System.String";
        }

        public object ConvertTo(object input)
        {
            if (ConstrstItems == null
                || ConstrstItems.Count == 0
                )
                return input;


            if (input == null)//寻找空值对照
            {
                foreach (DataConverterConstrstItem item in ConstrstItems)
                {
                    if (item.SourceValue.ToLower() == ConverterSpecialValue.NULL.ToLower())
                    {
                        object ret = Convert.ChangeType(item.DestValue, Type.GetType(this.DestDataType));
                        return ret;
                    }
                }
            }

            if (input == DBNull.Value)//寻找dbnull值对照
            {
                foreach (DataConverterConstrstItem item in ConstrstItems)
                {
                    if (item.SourceValue.ToLower() == ConverterSpecialValue.DBNULL.ToLower())
                    {
                        object ret = Convert.ChangeType(item.DestValue, Type.GetType(this.DestDataType));
                        return ret;
                    }
                }
            }

            //从对照表中查找对照
            foreach (DataConverterConstrstItem item in ConstrstItems)
            {
                object objinput = Convert.ChangeType(input, Type.GetType(this.SourceDataType));
                object objSrc = Convert.ChangeType(item.SourceValue, Type.GetType(this.SourceDataType));

                if (objinput.Equals(objSrc))
                {
                    object objDest = Convert.ChangeType(item.DestValue, Type.GetType(this.DestDataType));
                    return objDest;
                }
            }

            //查找不到则查找默认对照
            foreach (DataConverterConstrstItem item in ConstrstItems)
            {
                if (item.SourceValue.ToLower() == ConverterSpecialValue.DEFAULT.ToLower())
                {
                    object ret = Convert.ChangeType(item.DestValue, Type.GetType(this.SourceDataType));
                    return ret;
                }
            }

            return input;
        }
    }
}
