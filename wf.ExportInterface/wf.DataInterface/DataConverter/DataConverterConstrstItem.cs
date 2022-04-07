using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataInterface.Implement;

namespace Lib.DataInterface
{
    public class DataConverterConstrstItem
    {
        public string SourceValue { get; set; }

        public string DestValue { get; set; }

        public DataConverterConstrstItem()
            : this(typeof(string).FullName, null, typeof(string).FullName, null)
        {
        }

        public DataConverterConstrstItem(string srcDataType, string srcValue, string destDataType, string destValue)
        {
            this.SourceValue = srcValue;
            this.DestValue = destValue;
        }

        internal static DataConverterConstrstItem FromDTO(EntityDictDataConvertContrast dtoContrasts)
        {
            DataConverterConstrstItem item = new DataConverterConstrstItem();
            item.DestValue = dtoContrasts.con_dest_value;
            item.SourceValue = dtoContrasts.con_src_value;

            return item;
        }

        internal static List<DataConverterConstrstItem> FromDTOCollection(List<EntityDictDataConvertContrast> dtoCollection)
        {
            List<DataConverterConstrstItem> ret = new List<DataConverterConstrstItem>();
            foreach (EntityDictDataConvertContrast item in dtoCollection)
            {
                ret.Add(DataConverterConstrstItem.FromDTO(item));
            }
            return ret;
        }
    }
}
