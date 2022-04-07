using Lib.DataInterface.DataConverter;

namespace Lib.DataInterface
{
    public class DataInterfaceDataTableMappingContrast
    {
        public IInterfaceDataConverter Converter { get; set; }

        public string SourceDataType { get; set; }
        public string SourceField { get; set; }

        public string DestDataType { get; set; }
        public string DestField { get; set; }

        public DataInterfaceDataTableMappingContrast()
        {
            this.SourceDataType = "System.String";
            this.DestDataType = "System.String";
        }
    }
}
