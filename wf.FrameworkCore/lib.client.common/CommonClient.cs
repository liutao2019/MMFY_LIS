namespace dcl.client.frame
{
    using entity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class CommonClient 
    {
        public static DataSet CreateDS(string[] cols, string tableName)
        {
            DataSet set = new DataSet();
            set.Tables.Add(CreateDT(cols, tableName));
            return set;
        }

        public static DataTable CreateDT(string[] cols, string tableName)
        {
            DataTable table = new DataTable(tableName);
            foreach (string str in cols)
            {
                table.Columns.Add(str);
            }
            return table;
        }


        public static DataTable getUserLogInfo()
        {
            DataTable table = CreateDT(new string[] { "userid", "ip", "mac" }, "_LOGINFO");
            DataRow row = table.NewRow();
            row["userid"] = UserInfo.loginID;
            row["ip"] = UserInfo.ip;
            row["mac"] = UserInfo.mac;
            table.Rows.Add(row);
            return table;
        }



        public class setUserPower
        {
            private static List<EntitySysFunction> _dtFunc;

            public static void setPower(Control con, string frmName)
            {
                if (!UserInfo.isAdmin)
                {
                    string str = frmName + "." + con.Name;
                    if (((con.Tag != null) && ((con.Tag.ToString() == "1") || con.Tag.ToString().Contains("client"))) && (dtFunc.Where(w=>w.FuncCode==str).Count() == 0))
                    {
                        con.Enabled = false;
                    }
                    if (con is ToolStrip)
                    {
                        if (con is MenuStrip)
                        {
                            setPower(null, 0, (MenuStrip) con, frmName);
                        }
                        else
                        {
                            setPower((ToolStrip) con, frmName);
                        }
                    }
                    if (con.ContextMenuStrip != null)
                    {
                        setPower((ToolStrip) con.ContextMenuStrip, frmName);
                    }
                    foreach (Control control in con.Controls)
                    {
                        setPower(control, frmName);
                    }
                }
            }

            private static void setPower(ToolStrip con, string frmName)
            {
                foreach (ToolStripItem item in con.Items)
                {
                    string str = frmName + "." + item.Name;
                    if (((item.Tag != null) && ((item.Tag.ToString() == "1") || item.Tag.ToString().Contains("client"))) && (dtFunc.Where(w=>w.FuncCode==str).Count() == 0))
                    {
                        item.Enabled = false;
                    }
                }
            }

            private static void setPower(ToolStripMenuItem tsmi, int level, MenuStrip menuStrip1, string frmName)
            {
                int num;
                ToolStripMenuItem item;
                if (level == 0)
                {
                    for (num = 0; num < menuStrip1.Items.Count; num++)
                    {
                        item = (ToolStripMenuItem) menuStrip1.Items[num];
                        setPower(item, level + 1, menuStrip1, frmName);
                    }
                }
                else if ((tsmi != null) && (tsmi.DropDownItems.Count > 0))
                {
                    for (num = 0; num < tsmi.DropDownItems.Count; num++)
                    {
                        item = (ToolStripMenuItem) tsmi.DropDownItems[num];
                        setPower(item, level + 1, menuStrip1, frmName);
                    }
                }
                else
                {
                    string str = frmName + "." + tsmi.Name;
                    if (((tsmi.Tag != null) && ((tsmi.Tag.ToString() == "1") || tsmi.Tag.ToString().Contains("client"))) && (dtFunc.Where(w=>w.FuncCode==str).Count() == 0))
                    {
                        tsmi.Enabled = false;
                    }
                }
            }

            private static List<EntitySysFunction> dtFunc
            {
                get
                {
                    if (_dtFunc == null)
                    {
                        _dtFunc = UserInfo.entityUserInfo.Func;
                    }
                    return _dtFunc;
                }
            }
        }
    }
}

