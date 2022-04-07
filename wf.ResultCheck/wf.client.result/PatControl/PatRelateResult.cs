using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.client.frame.runsetting;
using dcl.client.frame;
using dcl.entity;
using System.Linq;
using DevExpress.Utils;
using System.Threading.Tasks;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace dcl.client.result.PatControl
{
    public partial class PatRelateResult : UserControl
    {
        EntityPidReportMain patient;
        public PatRelateResult()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 配置类
        /// </summary>
        PatInputPatResultSettingItem Config;

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="dtRelateResult"></param>
        public void LoadRelateResult(List<EntityObrResult> listRelateResult)
        {
            if (UserInfo.GetSysConfigValue("Lab_RelateResult_Group") == "样本号")
            {
                this.gridViewRelateResult.Columns["ObrSid"].GroupIndex = 0;
                this.gridViewRelateResult.Columns["ComName"].GroupIndex = 1;
            }
            else
            {
                //this.gridViewRelateResult.Columns["ComName"].GroupIndex = 0;
                this.gridViewRelateResult.Columns["RepReportDate"].GroupIndex = 0;
                this.gridViewRelateResult.Columns["RepReportDate"].GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DisplayText;
                this.gridViewRelateResult.Columns["RepReportDate"].GroupFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                this.gridViewRelateResult.Columns["RepReportDate"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }

            //*******************************************************************
            //加载数据时不展开，需要时再展开
            //this.gridViewRelateResult.ExpandAllGroups();

            //*******************************************************************

            if (Config != null && Config.OrderByItmSeq)
            {
                listRelateResult = listRelateResult.OrderBy(w => w.ItmSeq).ThenBy(w => w.ItmEname).ToList();

            }
            else
            {
                listRelateResult = listRelateResult.OrderByDescending(w => w.RepReportDate).ThenByDescending(w => w.ResComSeq).ThenByDescending(w => w.ItmSeq).ThenByDescending(w => w.ItmEname).ToList();
            }

            this.gridControlRelateResult.DataSource = listRelateResult;

        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_no_id"></param>
        /// <param name="pat_in_no"></param>
        /// <param name="dtPatDate"></param>
        public void LoadRelateResult(EntityPidReportMain patient)
        {
            //Thread combineItemThread = new Thread(new ThreadStart(delegate ()
            //{
            //    LoadRelateResultThread(patient);
            //}));
            //combineItemThread.IsBackground = true;
            //combineItemThread.Start();

            this.patient = patient;
            radioGroup1.SelectedIndex = 1;
        }

        private void DoSearch()
        {
            WaitDialogForm waitForm = new WaitDialogForm("正在加载中...", "请稍候");
            this.gridControlRelateResult.DataSource = null;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    List<EntityObrResult> lstResult = GetRelateResult(this.patient);
                    this.Invoke((MethodInvoker)(() =>
                    {
                        LoadResult(lstResult);
                    }));

                }
                finally
                {
                    waitForm.Close();
                }
            });
        }


        //private void GetReportOverTimeThread()
        //{
        //    Thread reportOverTimeThread = new Thread(new ThreadStart(delegate ()
        //    {
        //        ReportOverTimeThread();
        //    }));
        //    reportOverTimeThread.IsBackground = true;
        //    reportOverTimeThread.Start();
        //}

        private List<EntityObrResult> GetRelateResult(EntityPidReportMain patient)
        {
            ProxyRelateResult proxy = new ProxyRelateResult();
            return proxy.Service.GetRelateResult(patient);
        }

        public void LoadResult(List<EntityObrResult> listResult)
        {
            try
            {
                LoadRelateResult(listResult);
            }
            catch (Exception ex)
            {
            }
        }


        public void Reset()
        {
            this.gridControlRelateResult.DataSource = null;
        }

        private void gridViewRelateResult_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            List <EntityObrResult> source = gv.DataSource as List<EntityObrResult>;
            if (source == null || source.Count == 0)
            {
                Font f = new Font("宋体", 10.5f);
                Rectangle r = new Rectangle(5, gv.ColumnPanelRowHeight + 15, e.Bounds.Right - 5, e.Bounds.Height - 5);
                e.Graphics.DrawString("没有查询到数据!", f, Brushes.Blue, r);
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDate.Visible = radioGroup1.SelectedIndex == 3;
            DateTime? startDate = null;
            if (radioGroup1.SelectedIndex == 1)
            {
                //半年
                startDate = DateTime.Now.Date.AddMonths(-6);
            }
            else if (radioGroup1.SelectedIndex == 2)
            {
                //一年
                startDate = DateTime.Now.Date.AddYears(-1);
            }
            else if(radioGroup1.SelectedIndex == 3)
            {
                //自定义
                deBegin.DateTime = DateTime.Now;
                deEnd.DateTime = DateTime.Now;
            }

            if (radioGroup1.SelectedIndex != 3)
            {
                this.patient.StartDate = startDate;
                this.patient.EndDate = null;
                DoSearch();
                gridControlRelateResult.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(deBegin.Text))
            {
                this.patient.StartDate = deBegin.DateTime.Date;
            }
            else
            {
                this.patient.StartDate = null;
            }
            if (!string.IsNullOrEmpty(deEnd.Text))
            {
                this.patient.EndDate = deEnd.DateTime.Date.AddDays(1).AddMilliseconds(-1);
            }
            else
            {
                this.patient.EndDate = null;
            }
            DoSearch();
        }

        private void gridViewRelateResult_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            List<EntityObrResult> lstSource = this.gridControlRelateResult.DataSource as List<EntityObrResult>;
            if(lstSource == null)
            {
                return;
            }
            
            try
            {
                GridGroupRowInfo groupRow = e.Info as GridGroupRowInfo;
                string groupText = groupRow.Column.GetTextCaption() + ":" + groupRow.GroupValueText;
                lstSource.FindAll(t => t.RepReportDate == Convert.ToDateTime(groupRow.GroupValueText))
                         .Select(t => t.ComName).Distinct().ToList()
                         .ForEach(t => groupText += " " + t + "+");
                groupRow.GroupText = groupText.TrimEnd('+').Trim();
            }
            catch
            {
            }
        }
    }
}
