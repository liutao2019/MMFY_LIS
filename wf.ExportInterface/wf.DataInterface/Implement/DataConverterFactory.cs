using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataInterface.Implement;

namespace Lib.DataInterface.DataConverter
{
    public class DataConverterFactory
    {
        internal static IInterfaceDataConverter FromDTO(EntityDictDataConverter dtoConverter, DACManager dac)
        {
            if (dtoConverter == null)
            {
                return new NoConverter();
            }
            else
            {
                EnumDataInterfaceConverterType ruleType = (EnumDataInterfaceConverterType)Enum.Parse(typeof(EnumDataInterfaceConverterType), dtoConverter.rule_type, true);

                if (ruleType == EnumDataInterfaceConverterType.None)
                {
                    NoConverter converter = new NoConverter();
                    return converter;
                }
                else if (ruleType == EnumDataInterfaceConverterType.Contrast)
                {
                    ContrastConverter converter = new ContrastConverter();

                    converter.SourceDataType = dtoConverter.rule_src_datatype;
                    converter.DestDataType = dtoConverter.rule_dest_datatype;

                    List<EntityDictDataConvertContrast> listContrast = dac.GetConverterContrastsByConverterID(dtoConverter.rule_id);

                    foreach (EntityDictDataConvertContrast item in listContrast)
                    {
                        if (item.con_enabled)
                            converter.ConstrstItems.Add(DataConverterConstrstItem.FromDTO(item));
                    }

                    return converter;
                }
                else if (ruleType == EnumDataInterfaceConverterType.System)
                {
                    throw new NotSupportedException();
                }
                else if (ruleType == EnumDataInterfaceConverterType.DataInterface)
                {
                    DIConverter converter = new DIConverter();
                    EntityDictDataInterfaceCommand dtoCommand = dac.GetCommandByID(dtoConverter.rule_ref_id);
                    if (dtoCommand == null)
                    {
                        throw new Exception(string.Format("转换规则[{0}]对应的接口命令不存在，dict_DataInterfaceCommand.cmd_id={1}", dtoConverter.rule_name, dtoConverter.rule_ref_id));
                    }

                    EntityDictDataInterfaceConnection dtoConn = dac.GetConnectionByID(dtoCommand.conn_id);
                    if (dtoConn == null)
                    {
                        throw new Exception(string.Format("转换规则[{0}]对应的连接不存在，dict_DataInterfaceConnection.conn_id={1}", dtoConverter.rule_name, dtoCommand.conn_id));
                    }

                    List<EntityDictDataInterfaceCommandParameter> dtoListParam = dac.GetParametersByCmdID(dtoCommand.cmd_id);

                    DataInterfaceCommand cmd = DataInterfaceCommand.FromDTO(dtoCommand);
                    cmd.Connection = DataInterfaceConnection.FromDTO(dtoConn);
                    cmd.Parameters = DataInterfaceParameterCollection.FromDTO(dtoListParam, dac);

                    converter.ConvertCommand = cmd;
                    return converter;
                }
                else if (ruleType == EnumDataInterfaceConverterType.PluginDll)
                {
                    throw new NotSupportedException();
                }
                else if (ruleType == EnumDataInterfaceConverterType.Script)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}
