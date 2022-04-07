using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DAC
{
    public class GlobalSysTableIDGenerator
    {

        ITransaction tran = null;
        public GlobalSysTableIDGenerator(ITransaction tran)
        {
            this.tran = tran;
        }

        public GlobalSysTableIDGenerator()
            : this(null)
        { }

        public string Generate(string tablename, string columnname, GlobalSysTableIDRule rule)
        {
            string ruleString = null;

            if (string.IsNullOrEmpty(tablename) || string.IsNullOrEmpty(columnname))
            {
                throw new ArgumentNullException();
            }

            //是否需要在本次操作后提交事务
            bool needCommit = false;
            SqlHelper helper;

            if (this.tran == null)//如果传入的事务对象为空
            {
                this.tran = DACHelper.BeginTransaction();
                helper = new SqlHelper(this.tran);
                needCommit = true;
            }
            else
            {
                helper = new SqlHelper(this.tran);
            }

            string newEntityID;
            string newSysID;
            try
            {
                string sqlSelect = "select sys_currid ,sys_rule from GlobalSysTableID where sys_table = ? and sys_column = ?";
                DbCommandEx cmd = helper.CreateCommandEx(sqlSelect);
                cmd.AddParameterValue(tablename);
                cmd.AddParameterValue(columnname);

                DataTable table = helper.GetTable(cmd);

                object objCurrID;
                if (table.Rows.Count > 0)
                {
                    objCurrID = table.Rows[0]["sys_currid"] == null ? DBNull.Value : table.Rows[0]["sys_currid"];
                    ruleString = table.Rows[0]["sys_rule"] == DBNull.Value ? null : table.Rows[0]["sys_rule"].ToString();
                    rule = GlobalSysTableIDRule.Parse(ruleString);
                }
                else
                {
                    objCurrID = null;
                }

                DbCommandEx cmd2;
                if (objCurrID == null)
                {
                    newEntityID = GenerateNextID(objCurrID, out newSysID, rule);
                    string sql = "insert into GlobalSysTableID(sys_table, sys_column, sys_currid, sys_rule) values(?, ?, ?, ?)";
                    cmd2 = helper.CreateCommandEx(sql);
                    cmd2.AddParameterValue(tablename, DbType.AnsiString);
                    cmd2.AddParameterValue(columnname, DbType.AnsiString);
                    cmd2.AddParameterValue(newSysID, DbType.AnsiString);
                    cmd2.AddParameterValue(rule.ToString(), DbType.AnsiString);
                    helper.ExecuteNonQuery(cmd2);
                }
                else if (objCurrID == DBNull.Value)
                {
                    newEntityID = GenerateNextID(objCurrID, out newSysID, rule);
                    string sql = "update GlobalSysTableID set sys_currid = ? where sys_table = ? and sys_column = ?";
                    cmd2 = helper.CreateCommandEx(sql);
                    cmd2.AddParameterValue(newSysID, DbType.AnsiString);
                    cmd2.AddParameterValue(tablename, DbType.AnsiString);
                    cmd2.AddParameterValue(columnname, DbType.AnsiString);
                    helper.ExecuteNonQuery(cmd2);
                }
                else
                {
                    //currID = Convert.ToInt64(objCurrID);
                    //if (currID == long.MaxValue)
                    //{
                    //    throw new Exception("标识已达到最大值");
                    //}
                    newEntityID = GenerateNextID(objCurrID, out newSysID, rule);
                    string sql = "update GlobalSysTableID set sys_currid = ? where sys_table = ? and sys_column = ?";
                    cmd2 = helper.CreateCommandEx(sql);

                    cmd2.AddParameterValue(newSysID, DbType.AnsiString);
                    cmd2.AddParameterValue(tablename, DbType.AnsiString);
                    cmd2.AddParameterValue(columnname, DbType.AnsiString);
                    helper.ExecuteNonQuery(cmd2);

                    //currID = Convert.ToInt64(helper.ExecuteScalar(cmd));
                }

                if (needCommit)
                {
                    this.tran.Commit();
                }
                else
                {
                }
            }
            catch
            {
                this.tran.Rollback();
                throw;
            }

            return newEntityID;
        }

        public string Generate(string tablename, string columnname, string ruleExpression)
        {
            return Generate(tablename, columnname, GlobalSysTableIDRule.Parse(ruleExpression));
        }


        /// <summary>
        /// 生成下一个id
        /// 如：currID = 1，规则为 32位整型，规定长度为4，那么：返回0002，newSysID=2
        /// </summary>
        /// <param name="currID">当前值(SysTable中记录的值)</param>
        /// <param name="newSysID">新ID,(SysTable中存储的值)</param>
        /// <param name="rule"></param>
        /// <returns>返回可使用的id</returns>
        private string GenerateNextID(object currID, out string newSysID, GlobalSysTableIDRule rule)
        {
            if (currID == null || currID == DBNull.Value)
            {
                if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int32
                    || rule.IDGenerateType == EnumSysTableIDGenerateType.Int64)
                {
                    newSysID = rule.Start.ToString();
                }
                else
                {
                    newSysID = "1";
                }

                string strNewEntityID = newSysID;
                if (rule.Length > strNewEntityID.Length)
                {
                    strNewEntityID = strNewEntityID.PadLeft(rule.Length, '0');
                }
                return strNewEntityID;
            }
            else
            {
                string strNewEntityID;
                if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int32
                    || rule.IDGenerateType == EnumSysTableIDGenerateType.Int64)
                {
                    if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int32)
                    {
                        newSysID = (Convert.ToInt32(currID) + rule.Step).ToString();
                        strNewEntityID = newSysID;
                    }
                    else
                    {
                        newSysID = (Convert.ToInt64(currID) + rule.Step).ToString();
                        strNewEntityID = newSysID;
                    }
                }
                else
                {
                    long id = Convert.ToInt64(currID) + 1;
                    newSysID = id.ToString();
                    strNewEntityID = newSysID;
                }

                if (rule.Length > strNewEntityID.Length)
                {
                    strNewEntityID = strNewEntityID.PadLeft(rule.Length, '0');
                }
                return strNewEntityID;
            }
        }

        /// <summary>
        /// 同步GlobalSysTableID表的主键id值
        /// </summary>
        public static void Sync()
        {
            SqlHelper helper = new SqlHelper();

            //获取整个GlobalSysTableID数据
            DataTable tableID = helper.GetTable("select * from GlobalSysTableID");

            //遍历每一个增长列
            foreach (DataRow rowTable in tableID.Rows)
            {
                string sys_table = rowTable["sys_table"].ToString();
                string sys_column = rowTable["sys_column"].ToString();
                string sys_rule = rowTable["sys_rule"].ToString();
                string sys_currid = rowTable["sys_currid"].ToString();

                try
                {
                    GlobalSysTableIDRule rule = GlobalSysTableIDRule.Parse(sys_rule);

                    //如果为自动增长型
                    if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int32
                        || rule.IDGenerateType == EnumSysTableIDGenerateType.Int64)
                    {
                        //查找对应表对应列的所有值，然后循环
                        //如果为正数增长，则找出业务表中的最大值
                        //如果为负数增长，则找出业务表中的最小值

                        //如果GlobalSysTableID的值比业务表中的最大值大(正增长)
                        //或GlobalSysTableID的值比业务表中的最小值小(负增长)
                        //则GlobalSysTableID的记录值sys_currid不变
                        string sqlSelectID = string.Format("select {0} from {1}", sys_column, sys_table);
                        DataTable tb = helper.GetTable(sqlSelectID);

                        string new_sys_currid = string.Empty;

                        //区分int32和int64类型
                        if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int32)
                        {
                            int tempID = rule.Start;
                            foreach (DataRow item in tb.Rows)
                            {
                                int currID;
                                if (int.TryParse(item[sys_column].ToString(), out currID))
                                {
                                    
                                    if (
                                        (currID > tempID && rule.Step > 0)
                                        || (currID < tempID && rule.Step < 0)
                                        )
                                    {
                                        tempID = currID;
                                    }
                                }
                            }

                            if (
                                 string.IsNullOrEmpty(sys_currid)
                                 || (rule.Step > 0 && Convert.ToInt32(sys_currid) < tempID)
                                 || (rule.Step < 0 && Convert.ToInt32(sys_currid) > tempID)
                                )
                            {
                                new_sys_currid = tempID.ToString();
                            }
                            else
                            {
                                new_sys_currid = sys_currid;
                            }
                        }
                        else if (rule.IDGenerateType == EnumSysTableIDGenerateType.Int64)
                        {
                            long tempID = (long)rule.Start;
                            foreach (DataRow item in tb.Rows)
                            {
                                long currID;
                                if (long.TryParse(item[sys_column].ToString(), out currID))
                                {
                                    if (currID > tempID)
                                    {
                                        tempID = currID;
                                    }
                                }
                            }
                            if (
                                 string.IsNullOrEmpty(sys_currid)
                                 || (rule.Step > 0 && Convert.ToInt64(sys_currid) < tempID)
                                 || (rule.Step < 0 && Convert.ToInt64(sys_currid) > tempID)
                                )
                            {
                                new_sys_currid = tempID.ToString();
                            }
                            else
                            {
                                new_sys_currid = sys_currid;
                            }
                        }

                        string sqlUpdate = "update GlobalSysTableID set sys_currid=? where sys_table=? and sys_column=?";
                        DbCommandEx cmd = helper.CreateCommandEx(sqlUpdate);

                        cmd.AddParameterValue(new_sys_currid, DbType.AnsiString);
                        cmd.AddParameterValue(sys_table, DbType.AnsiString);
                        cmd.AddParameterValue(sys_column, DbType.AnsiString);
                        helper.ExecuteNonQuery(cmd);
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
            }
        }
    }
}
