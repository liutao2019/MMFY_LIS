using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DataInterface.DataConverter;
using System.IO;
using System.Xml;

namespace Lib.DataInterface.Implement
{
    public class DataInterfaceHelper
    {
        #region 记录条码命令操作失败的成员

        /// <summary>
        /// 是否启动条码失败记录
        /// </summary>
        private bool IsBarcodeLoseLog = false;

        /// <summary>
        /// 记录失败的bc_bar_code
        /// </summary>
        private string Log_bc_bar_code { get; set; }

        /// <summary>
        /// 记录失败的Log_bc_yz_id
        /// </summary>
        private string Log_bc_yz_id { get; set; }

        /// <summary>
        /// 记录失败的Log_pat_id
        /// </summary>
        private string Log_pat_id { get; set; }

        /// <summary>
        /// 记录失败的Log_bc_op_code
        /// </summary>
        private string Log_bc_op_code { get; set; }

        /// <summary>
        /// 记录失败的Log_bc_op_name
        /// </summary>
        private string Log_bc_op_name { get; set; }

        /// <summary>
        /// 记录失败的Log_cmd_group
        /// </summary>
        private string Log_cmd_group { get; set; }

        private List<string> _Log_list_group_name = new List<string>();

        private List<string> Log_list_group_name
        {
            get
            {
                if (_Log_list_group_name.Count <= 0)
                {
                    _Log_list_group_name.Add("条码_门诊_下载后");
                    _Log_list_group_name.Add("条码_住院_下载后");
                    _Log_list_group_name.Add("条码_体检_下载后");
                    _Log_list_group_name.Add("条码_其他_下载后");
                    _Log_list_group_name.Add("条码_门诊_签收后");
                    _Log_list_group_name.Add("条码_住院_签收后");
                    _Log_list_group_name.Add("条码_体检_签收后");
                    _Log_list_group_name.Add("条码_其他_签收后");
                    _Log_list_group_name.Add("条码_门诊_删除后");
                    _Log_list_group_name.Add("条码_住院_删除后");
                    _Log_list_group_name.Add("条码_体检_删除后");
                    _Log_list_group_name.Add("条码_其他_删除后");
                    _Log_list_group_name.Add("检验_门诊_二审后");
                    _Log_list_group_name.Add("检验_住院_二审后");
                    _Log_list_group_name.Add("检验_体检_二审后");
                    _Log_list_group_name.Add("检验_其他_二审后");
                    _Log_list_group_name.Add("检验_门诊_登记录入后");
                    _Log_list_group_name.Add("检验_住院_登记录入后");
                    _Log_list_group_name.Add("检验_体检_登记录入后");
                }
                return _Log_list_group_name;
            }
        }
        
        #endregion


        private DACManager dac = null;

        public DataInterfaceHelper(EnumDataAccessMode accessMode, bool useCache)
        {
            this.dac = new DACManager(accessMode, useCache);
            getIsBcLogByXml();
        }

        #region TestConnection
        public bool TestConnection(string conn_id, out string errMsg)
        {
            DACManager dacTemp = dac.Clone() as DACManager;
            dacTemp.UseCache = false;
            EntityDictDataInterfaceConnection dtoConn = dacTemp.GetConnectionByID(conn_id);
            return TestConnection(dtoConn, out errMsg);
        }

        public bool TestConnection(EntityDictDataInterfaceConnection dtoConn, out string errMsg)
        {
            errMsg = dac.TestConnection(dtoConn);
            return string.IsNullOrEmpty(errMsg);
        }
        #endregion

        #region ExecuteNonQuery


        public void ExecuteNonQueryWithGroup(string groupName, InterfaceDataBindingItem[] dataBindings)
        {
            setNoteDataForLog(groupName, dataBindings);
            List<EntityDictDataInterfaceCommand> dtoListCmd = dac.GetCommandByGroup(groupName);
            try
            {
                foreach (EntityDictDataInterfaceCommand dtoCmd in dtoListCmd)
                {
                    if (dtoCmd.cmd_enabled == 1)
                    {
                        ExecuteNonQuery(dtoCmd, dataBindings);
                    }
                }
            }
            catch (Exception ex)
            {
                AddbarcodeLoseLogToDB();//记录到数据库
                throw ex;
            }
        }


        public void ExecuteNonQuery(EntityDictDataInterfaceCommand dtoCmd, InterfaceDataBindingItem[] dataBindings)
        {
            List<EntityDictDataInterfaceCommandParameter> dtoParams;
            DataInterfaceCommand executeCmd = CreateExecuteCommand(dtoCmd, out dtoParams);
            PrepareBindingValue(dtoCmd, dtoParams, executeCmd, dataBindings);
            int rvExec= executeCmd.ExecuteNonQuery();//记录影响行数

            if (rvExec <= 0)
            {
                AddbarcodeLoseLogToDB();
            }

            AfterExecute(executeCmd, dataBindings);
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(string cmd_id, InterfaceDataBindingItem[] dataBindings)
        {
            EntityDictDataInterfaceCommand dtoCmd = dac.GetCommandByID(cmd_id);
            if (dtoCmd == null)
                throw new Exception("找不到指定的cmd_id=" + cmd_id);

            return ExecuteScalar(dtoCmd, dataBindings);
        }

        public object ExecuteScalar(EntityDictDataInterfaceCommand dtoCmd, InterfaceDataBindingItem[] dataBindings)
        {
            List<EntityDictDataInterfaceCommandParameter> dtoParams;
            DataInterfaceCommand executeCmd = CreateExecuteCommand(dtoCmd, out dtoParams);
            PrepareBindingValue(dtoCmd, dtoParams, executeCmd, dataBindings);
            object ret = executeCmd.ExecuteScalar();

            AfterExecute(executeCmd, dataBindings);
            return ret;
        }
        #endregion

        private DataInterfaceCommand CreateExecuteCommand(EntityDictDataInterfaceCommand dtoCommand, out List<EntityDictDataInterfaceCommandParameter> dtoParams)
        {
            DataInterfaceCommand cmd = DataInterfaceCommand.FromDTO(dtoCommand);

            EntityDictDataInterfaceConnection dtoConn = dac.GetConnectionByID(dtoCommand.conn_id);
            dtoParams = dac.GetParametersByCmdID(dtoCommand.cmd_id);

            cmd.Connection = DataInterfaceConnection.FromDTO(dtoConn);
            cmd.Parameters = DataInterfaceParameterCollection.FromDTO(dtoParams, dac);

            return cmd;
        }

        private void PrepareBindingValue(EntityDictDataInterfaceCommand dtoCommand
            , List<EntityDictDataInterfaceCommandParameter> dtoListParameter
            , DataInterfaceCommand executeCmd
            , InterfaceDataBindingItem[] dataBindings)
        {
            Dictionary<string, object> dictNameValue = new Dictionary<string, object>();

            //遍历所有的参数配置
            foreach (EntityDictDataInterfaceCommandParameter dtoParam in dtoListParameter)
            {
                if (dtoParam.param_enabled == 0)
                    continue;

                //名称
                string parName = dtoParam.param_name;

                //找到对应的执行参数
                DataInterfaceParameter executeParam = executeCmd.Parameters[parName];

                if (executeParam == null)
                    continue;

                if (dtoParam.param_isbound == 1)//使用了绑定
                {
                    string bindingName;
                    if (!string.IsNullOrEmpty(dtoParam.param_databind))
                    {
                        //找出绑定名称
                        bindingName = dtoParam.param_databind;
                    }
                    else//没有则使用参数名
                    {
                        bindingName = dtoParam.param_name;
                    }

                    foreach (InterfaceDataBindingItem bindingValue in dataBindings)
                    {
                        if (bindingName.ToLower() == bindingValue.BindingName.ToLower())
                        {
                            executeParam.Value = bindingValue.Value;
                            bindingValue.Parameter = executeParam;
                            break;
                        }
                    }
                }
                else//没有使用绑定，检查是否有设置默认值
                {
                    if (string.IsNullOrEmpty(dtoParam.param_databind))
                    {
                    }
                    else
                    {
                        executeParam.Value = dtoParam.param_databind;
                    }
                }
            }
        }

        private void AfterExecute(DataInterfaceCommand cmd, InterfaceDataBindingItem[] dataBindings)
        {
            foreach (InterfaceDataBindingItem binding in dataBindings)
            {
                if (binding.Parameter == null)
                    continue;

                if (binding.Parameter.Direction == EnumDataInterfaceParameterDirection.InputOutput
                    || binding.Parameter.Direction == EnumDataInterfaceParameterDirection.Output
                    || binding.Parameter.Direction == EnumDataInterfaceParameterDirection.Reference
                    || binding.Parameter.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                {
                    binding.Value = binding.Parameter.Value;
                }
            }
        }

        #region 记录条码命令操作失败的方法

        /// <summary>
        ///  读取XML配置文件,获取是否启动日志功能
        /// </summary>
        /// <returns></returns>
        private void getIsBcLogByXml()
        {
            IsBarcodeLoseLog = false;

            string str_temp = readXml("ISLOG");

            if (!string.IsNullOrEmpty(str_temp))
            {
                if (str_temp.ToUpper() == "Y")
                {
                    IsBarcodeLoseLog = true;
                }
            }
        }

        /// <summary>
        /// 读取xml关于某键值
        /// </summary>
        /// <param name="tKey"></param>
        private string readXml(string tKey)
        {
            string tValue = "";

            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Lib.DataInterface.Setting.XML";

            try
            {
                if (!File.Exists(filepath))
                {
                    saveXml();
                    return tValue;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(tKey))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tValue = row[tKey].ToString();
                        ds.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(filepath))//有误,重新生成
                {
                    File.Delete(filepath);
                    saveXml();
                }
            }
            return tValue;
        }

        /// <summary>
        /// 保存xmlMSG配置文件
        /// </summary>
        private void saveXml()
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Lib.DataInterface.Setting.XML";
            try
            {
                string strXml = null;

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                strXml = createStrXml();
                if (!string.IsNullOrEmpty(strXml))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXml);

                    doc.Save(filepath);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <returns></returns>
        private string createStrXml()
        {
            //添加了：Lib.DataInterface.Setting.XML作为配置文件,控制功能启动
            string strXml = null;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xtw.WriteStartDocument();
                //根节点
                xtw.WriteStartElement("DATAINTERFACE");

                //子节点
                xtw.WriteStartElement("SETTING");

                xtw.WriteComment("ISLOG:N-不启动失败日志;Y-启动");

                //ORIID
                xtw.WriteStartElement("ISLOG");
                xtw.WriteString("N");
                xtw.WriteEndElement();

                xtw.WriteEndElement();//SETTING
                xtw.WriteEndElement();//DATAINTERFACE
                xtw.WriteEndDocument();

                strXml = sw.ToString();
            }
            return strXml;
        }

        /// <summary>
        /// 记录当前参数
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="dataBindings"></param>
        private void setNoteDataForLog(string groupName, InterfaceDataBindingItem[] dataBindings)
        {
            if (!IsBarcodeLoseLog) return;

            if (!string.IsNullOrEmpty(groupName) && Log_list_group_name.Contains(groupName)
                && dataBindings != null && dataBindings.Length > 0)
            {
                Log_cmd_group = groupName;

                for (int i = 0; i < dataBindings.Length; i++)
                {
                    if (dataBindings[i] != null && (!string.IsNullOrEmpty(dataBindings[i].BindingName))
                        && dataBindings[i].BindingName == "bc_bar_code")
                    {
                        if (dataBindings[i].Value != null)
                        {
                            Log_bc_bar_code = dataBindings[i].Value.ToString();
                        } 
                    }

                    if (dataBindings[i] != null && (!string.IsNullOrEmpty(dataBindings[i].BindingName))
                        && dataBindings[i].BindingName == "bc_yz_id")
                    {
                        if (dataBindings[i].Value != null)
                        {
                            Log_bc_yz_id = dataBindings[i].Value.ToString();
                        }
                    }

                    //Log_pat_id
                    if (dataBindings[i] != null && (!string.IsNullOrEmpty(dataBindings[i].BindingName))
                        && dataBindings[i].BindingName == "pat_id")
                    {
                        if (dataBindings[i].Value != null)
                        {
                            Log_pat_id = dataBindings[i].Value.ToString();
                        }
                    }

                    //Log_bc_op_code
                    if (dataBindings[i] != null && (!string.IsNullOrEmpty(dataBindings[i].BindingName))
                        && dataBindings[i].BindingName == "op_code")
                    {
                        if (dataBindings[i].Value != null)
                        {
                            Log_bc_op_code = dataBindings[i].Value.ToString();
                        }
                    }

                    //Log_bc_op_name
                    if (dataBindings[i] != null && (!string.IsNullOrEmpty(dataBindings[i].BindingName))
                        && dataBindings[i].BindingName == "op_name")
                    {
                        if (dataBindings[i].Value != null)
                        {
                            Log_bc_op_name = dataBindings[i].Value.ToString();
                        }
                    }
                }

                if (string.IsNullOrEmpty(Log_bc_bar_code)) Log_bc_bar_code = "";
                if (string.IsNullOrEmpty(Log_bc_yz_id)) Log_bc_yz_id = "";
                if (string.IsNullOrEmpty(Log_pat_id)) Log_pat_id = "";
                if (string.IsNullOrEmpty(Log_bc_op_code)) Log_bc_op_code = "";
                if (string.IsNullOrEmpty(Log_bc_op_name)) Log_bc_op_name = "";
            }
        }

        /// <summary>
        /// 添加失败记录到数据库
        /// </summary>
        private void AddbarcodeLoseLogToDB()
        {
            if (!IsBarcodeLoseLog) return;

            if ((!string.IsNullOrEmpty(Log_cmd_group)) && (!string.IsNullOrEmpty(Log_bc_yz_id)))
            {
                try
                {
                    string LisConnectionString_temp =new dcl.common.MD5Helper().DecryptString( System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                    using (Lib.DAC.ITransaction tran = Lib.DAC.DACHelper.BeginTransaction(LisConnectionString_temp, Lib.DAC.EnumDbDriver.MSSql, Lib.DAC.EnumDataBaseDialet.SQL2005))
                    {
                        Lib.DAC.SqlHelper sqlHelper = new Lib.DAC.SqlHelper(tran);
                        string sql = string.Format(@" DELETE FROM LogDataInterfaceBC WHERE bc_bar_code='{0}' and bc_yz_id='{1}' and cmd_group='{2}' and pat_id='{3}' ;
                INSERT INTO LogDataInterfaceBC ([bc_bar_code],[bc_yz_id],[cmd_group],[pat_id],[bc_op_code],[bc_op_name],[op_time]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',getdate())", Log_bc_bar_code, Log_bc_yz_id, Log_cmd_group, Log_pat_id, Log_bc_op_code, Log_bc_op_name);
                        sqlHelper.ExecuteNonQuery(sql);
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.LogInfo(this.GetType().FullName, ex.ToString());
                }
            }
        }
        
        #endregion
    }

    [Serializable]
    public class InterfaceDataBindingItem
    {
        public bool IsBindingMode { get; set; }
        public string BindingName { get; set; }
        public object Value { get; set; }
        internal DataInterfaceParameter Parameter { get; set; }

        public InterfaceDataBindingItem(string name, object value)
        {
            this.BindingName = name;
            this.Value = value;
            this.IsBindingMode = true;
        }

        public override string ToString()
        {
            return string.Format("Name={0}，Value={1}", BindingName, ObjectDisplayHelper.ToString(Value));
        }
    }
}
