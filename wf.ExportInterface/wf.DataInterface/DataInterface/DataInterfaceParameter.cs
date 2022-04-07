using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataInterface.DataConverter;
using Lib.DataInterface.Implement;
using System.Xml.Serialization;

namespace Lib.DataInterface
{
    [Serializable]
    public class DataInterfaceParameter
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public EnumDataInterfaceParameterDirection Direction { get; set; }
        public string DataType { get; set; }
        public object Value { get; set; }

        [XmlIgnoreAttribute]
        public IInterfaceDataConverter Converter { get; set; }

        internal static DataInterfaceParameter FromDTO(EntityDictDataInterfaceCommandParameter dtoParam, DACManager dac)
        {
            DataInterfaceParameter parameter = new DataInterfaceParameter();
            parameter.DataType = dtoParam.param_datatype;
            parameter.Direction = (EnumDataInterfaceParameterDirection)Enum.Parse(typeof(EnumDataInterfaceParameterDirection), dtoParam.param_direction, false);

            if (dtoParam.param_length != null)
                parameter.Length = dtoParam.param_length.Value;

            parameter.Name = dtoParam.param_name;

            if (!string.IsNullOrEmpty(dtoParam.param_converter_id) && dac != null)
            {
                parameter.Converter = DataConverterFactory.FromDTO(
                    dac.GetConverterByID(dtoParam.param_converter_id)
                    , dac
                    );
            }
            else
            {
                parameter.Converter = new NoConverter();
            }

            if (dtoParam.param_isbound == 0
                && !string.IsNullOrEmpty(dtoParam.param_databind)
                )
            {
                if (dtoParam.param_direction == EnumDataInterfaceParameterDirection.Input.ToString())
                {
                    parameter.Value = parameter.Converter.ConvertTo(dtoParam.param_databind);
                }
                else
                {
                    parameter.Value = dtoParam.param_databind;
                }
            }

            return parameter;
        }

        public override string ToString()
        {
            return string.Format(string.Format("Name = {0}, DataType = {1}, Direction = {2}, Value = {3}"
                , this.Name
                , this.DataType
                , this.Direction
                , this.Value == null ? "<null>" : this.Value));
        }
    }
}
