using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using dcl.entity;

namespace dcl.client.instrmt
{
    public partial class ItrRegistraionLog : UserControl
    {
        public ItrRegistraionLog()
        {
            InitializeComponent();
        }

        //public void Init(string content, List<EntityInstrmtMaintainRegistration> lisRegisInfo) 
        public void Init(string content, List<EntityDicInstrmtMaintainRegistration> lisRegisInfo) 
        {
            this.labContent.Text = content;
            DataTable result = new DataTable();
            result.Columns.Add("日期", typeof(string));
            
            for (int i = 0; i < lisRegisInfo.Count; i++)
            {
                //string date = lisRegisInfo[i].reg_register_date.ToString("yyyy-MM-dd");
                //if (result.Columns.Contains(date))
                //{
                //    date = lisRegisInfo[i].reg_register_date.ToString("yyyy-MM-dd HH:mm");
                //}
                //if (result.Columns.Contains(date))
                //{
                //    date = lisRegisInfo[i].reg_register_date.ToString("yyyy-MM-dd HH:mm:ss");
                //}
                string date = lisRegisInfo[i].RegRegisterDate.Value.ToString("yyyy-MM-dd");
                if (result.Columns.Contains(date))
                {
                    date = lisRegisInfo[i].RegRegisterDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                if (result.Columns.Contains(date))
                {
                    date = lisRegisInfo[i].RegRegisterDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!result.Columns.Contains(date))
                {
                    result.Columns.Add(date, typeof(string));
                }
            }



            object[] values1 = new object[lisRegisInfo.Count + 1];
            object[] values2 = new object[lisRegisInfo.Count + 1];
            object[] values3 = new object[lisRegisInfo.Count + 1];
            object[] values4 = new object[lisRegisInfo.Count + 1];
            values1[0] = "操作信息";
            values2[0] = "保养备注";
            values3[0] = "登记人";
            values4[0] = "超出保养时间";

            for (int i = 0; i < lisRegisInfo.Count; i++)
            {
                //values1[i + 1] = lisRegisInfo[i].RegOperateContent;
                //values2[i + 1] = lisRegisInfo[i].reg_exp;
                //values3[i + 1] = lisRegisInfo[i].reg_register_code;
                
                values1[i + 1] = lisRegisInfo[i].RegOperateContent;
                values2[i + 1] = lisRegisInfo[i].RegExp;
                values3[i + 1] = lisRegisInfo[i].RegRegisterCode;
                values4[i + 1] = lisRegisInfo[i].OverrunIntervalTime;
            }

            result.Rows.Add(values1);
            result.Rows.Add(values2);
            result.Rows.Add(values3);
            result.Rows.Add(values4);
            this.gridControl1.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridControl1.DataSource = result;
            this.gridView1.BestFitColumns();
        }
    }
}
