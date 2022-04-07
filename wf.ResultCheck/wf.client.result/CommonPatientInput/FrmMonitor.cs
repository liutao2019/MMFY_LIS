using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;
using dcl.common;
using dcl.entity;
using dcl.client.report;
using dcl.client.cache;

namespace lis.client.result
{
    public partial class FrmMonitor : FrmCommon
    {
        public List<EntityPidReportMain> listNormal = new List<EntityPidReportMain>();
        public List<EntityPidReportMain> listNoResult = new List<EntityPidReportMain>();
        public List<EntityObrResult> listNoPat = new List<EntityObrResult>();
        public DataTable dtPatFlag = new DataTable();
        public DataTable dtDepId = new DataTable();
        public List<EntityPidReportMain> listTypepat = new List<EntityPidReportMain>();

        string itr_id;
        string itr_name;
        string ctype_id;
        string ctype_name;
        DateTime dtPat_date;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="itr_name"></param>
        /// <param name="type_id"></param>
        /// <param name="type_name"></param>
        /// <param name="dtPat_date"></param>
        public FrmMonitor(string itr_id, string itr_name, string type_id, string type_name, DateTime dtPat_date)
        {
            InitializeComponent();

            this.itr_id = itr_id;
            this.itr_name = itr_name;
            this.ctype_id = type_id;
            this.ctype_name = type_name;
            this.dtPat_date = dtPat_date;
        }


        /// <summary>
        /// .load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMonitor_Load(object sender, EventArgs e)
        {
            string t_name = "无";
            if (!string.IsNullOrEmpty(itr_name))
            {
                t_name = itr_name;
            }

            this.Text = string.Format("样本进程    当前物理组：{0}   当前仪器：{1}", ctype_name, t_name);
            BindData();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EntityResponse response = new ProxyMonitor().Service.GetPatMonitor(this.ctype_id, this.itr_id, this.dtPat_date);
            Dictionary<string, object> dict = response.GetResult() as Dictionary<string, object>;
            this.listNoPat = dict["nopat"] as List<EntityObrResult>;
            this.listNoResult = dict["noresult"] as List<EntityPidReportMain>;
            this.listNormal = dict["normal"] as List<EntityPidReportMain>;
            this.listTypepat = dict["AllTypepat"] as List<EntityPidReportMain>;
            //总数
            int countTotal = listNormal.Count;

            //未审核个数
            int countUnAudit = 0;

            //未报告个数
            int countUnReport = 0;

            //未打印个数
            int countUnPrint = 0;

            foreach (EntityPidReportMain row in listNormal)
            {

                if (row.RepStateName == "未审核")
                {
                    row.RepStateName = "未" + LocalSetting.Current.Setting.AuditWord;
                    countUnAudit++;
                }

                if (row.RepStateName == "未报告")
                {
                    row.RepStateName = "未" + LocalSetting.Current.Setting.ReportWord;
                    countUnReport++;
                }

                if (row.RepStateName == "未打印")
                {
                    row.RepStateName = "未打印";
                    countUnPrint++;
                }

                if (row.RepStatusName == "未审核")
                {
                    row.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                }
                else if (row.RepStatusName == "已审核")
                {
                    row.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                }
                else if (row.RepStatusName == "已报告")
                {
                    row.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                }
                else if (row.RepStatusName == "已打印")
                {
                    row.RepStatusName = "已打印";
                }
            }

            this.lblMsgHasResult.Text = string.Format("总数{0}  未{4}{1}  未{5}{2}  未打印{3}", countTotal, countUnAudit, countUnReport, countUnPrint, LocalSetting.Current.Setting.AuditWord, LocalSetting.Current.Setting.ReportWord);

            #region MyRegion
            //总数
            int countTypeTotal = listTypepat.Count;

            //未审核个数
            int countTypeUnAudit = 0;

            //已签收未登记个数
            int countTypeUnRegister = 0;

            foreach (EntityPidReportMain row in listTypepat)
            {
                if (row.RepStateName == "未审核")
                {
                    row.RepStateName = "未" + LocalSetting.Current.Setting.AuditWord;
                    countTypeUnAudit++;
                }

                if (row.RepStateName == "未报告")
                {
                    row.RepStateName = "未" + LocalSetting.Current.Setting.ReportWord;

                }

                if (row.RepStateName.ToString() == "已签收未登记")
                {
                    row.RepStateName = "已签收未登记";
                    countTypeUnRegister++;
                }

                if (row.RepStatusName == "未审核")
                {
                    row.RepStatusName = "未" + LocalSetting.Current.Setting.AuditWord;
                }
                else if (row.RepStatusName == "已审核")
                {
                    row.RepStatusName = "已" + LocalSetting.Current.Setting.AuditWord;
                }
                else if (row.RepStatusName == "已报告")
                {
                    row.RepStatusName = "已" + LocalSetting.Current.Setting.ReportWord;
                }
                else if (row.RepStatusName == "已打印")
                {
                    row.RepStatusName = "已打印";
                }
            }

            this.label1.Text = string.Format("总数{0}  未{3}{1}  已签收未登记{2}", countTypeTotal, countTypeUnAudit, countTypeUnRegister, LocalSetting.Current.Setting.AuditWord);

            #endregion

            this.bsPatList.DataSource = listNormal;
            this.gcNoPat.DataSource = listNoPat;
            this.gcNoResult.DataSource = listNoResult;
            this.bsPatDepId.DataSource = this.dtDepId;
            this.gvNoPat.ExpandGroupLevel(0);

            gdAllType.DataSource = listTypepat;

            this.gvNormal.ClearSelection();
            if (gvNormal.RowCount > 0)
            {
                gvNormal.UnselectRow(0);
            }
        }


        private void gvNormal_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                EntityPidReportMain dr = this.gvNormal.GetRow(e.RowHandle) as EntityPidReportMain;

                if (dr != null && !Compare.IsEmpty(dr.RepStatus))
                {
                    string pat_flag = dr.RepStatus.ToString();


                    if (pat_flag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        e.Appearance.ForeColor = Color.Green;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Natural || pat_flag == string.Empty)
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        e.Appearance.ForeColor = Color.Blue; ;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void cmbGridFilterPatientState_EditValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void gvAllType_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                EntityPidReportMain dr = this.gvAllType.GetRow(e.RowHandle) as EntityPidReportMain;

                if (dr != null && !Compare.IsEmpty(dr.RepStatus))
                {
                    string pat_flag = dr.RepStatus.ToString();


                    if (pat_flag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        e.Appearance.ForeColor = Color.Green;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Natural || pat_flag == string.Empty)
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        e.Appearance.ForeColor = Color.Blue; ;
                    }
                    else if (pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gcNoResult.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcNoResult.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        /// <summary>
        /// 打印按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repPrintButton_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = EntityManager<EntityPidReportMain>.ConvertToDataTable(listNoResult, "wf");
            dt.TableName = "可设计字段";
            //新增一列打印日期
            dt.Columns.Add("报告日期", typeof(string));
            DateTime date = ServerDateTime.GetServerDateTime();
            for (int i = 0; i < dt.Rows.Count; i++)
                dt.Rows[i]["报告日期"] = date;
            
            ds.Tables.Add(dt);

            EntityDCLPrintData printData = new EntityDCLPrintData();
            printData.ReportName = "未完成项目报表";
            printData.ReportData = ds;

            DCLReportPrint.PrintByData(printData);
        }
    }
}
