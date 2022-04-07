using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;
using System.Diagnostics;
using dcl.client.common;
using dcl.client.frame.runsetting;

using DevExpress.XtraGrid.Views.Grid;
using dcl.client.frame;
using dcl.common;
using lis.client.control;
using dcl.client.result.CommonPatientInput;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using dcl.root.logon;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using dcl.entity;
using System.Linq;

using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class PatResultDoubleColumn : UserControl
    {
        public PatResultDoubleColumn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 配置类
        /// </summary>
        PatInputPatResultSettingItem Config;

        /// <summary>
        /// 是否修改过结果信息
        /// </summary>
        public static bool IsPatResultDoubleColVChange { get; private set; }

        public static bool showDoubleItrWarningMsg { get; set; }

        private int note_history_col = 0;//记录历史结果-列标识
        private string note_history_rowValue = "";//记录历史结-果行内容

        public void Reset()
        {
            gridControlSingle.DataSource = null;
            lblItemCount.Text = string.Empty;
            this.gridViewSingle.Columns["history_result1"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result2"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result3"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result12"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result22"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result32"].Caption = "历史结果";
            lblCombine.Text = string.Empty;
        }

        private string PatID;
        private string PatInNo;
        List<EntityObrResult> dtPatientResulto;
        EntityPidReportMain dtPatient;
        public event ClaItemInfoEventHandler ClaItemInfo = null;
        List<EntityPidReportDetail> patients_mi;
        List<EntitySysOperationLog> listSysOprLog = new List<EntitySysOperationLog>();
        private bool Lab_MarkModifyResultWithColor = false;
        private string samtypeid;
        private int patage;
        private string patsex;
        /// <summary>
        /// 仪器
        /// </summary>
        public string Pat_itr_id { get; set; }

        /// <summary>
        /// 项目特征选择窗体
        /// </summary>
        FrmItmPropLst prop = null;

        /// <summary>
        /// 加载病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        public void LoadResult(string pat_id)
        {
            ProxyPatResult resultProxy = new ProxyPatResult();
            EntityQcResultList qcResult = resultProxy.Service.GetPatientCommonResult(pat_id, true);
            EntityPidReportMain patient = qcResult.patient;
            List<EntityPidReportDetail> dtComMi = new List<EntityPidReportDetail>();
            if (patient != null)
            {
                dtComMi = patient.ListPidReportDetail;
            }

            List<EntityObrResult> dtPatResulto = qcResult.listResulto;

            LoadResult(patient, dtPatResulto, true);

            if (dtComMi != null && dtComMi.Count > 0)
            {
                patients_mi = dtComMi;
            }
            else
            {
                patients_mi = null;
            }

            NotNullItemCheck();
        }

        public void NotNullItemCheck()
        {
            if (this.patients_mi != null && this.patients_mi.Count > 0)
            {
                //必录项目
                List<string> listNotNullItem = new List<string>();

                List<string> listNotNullItemHasResult = new List<string>();

                //遍历当前病人检验组合
                foreach (EntityPidReportDetail drPatComMi in this.patients_mi)
                {
                    string com_id = drPatComMi.ComId;

                    //查找组合所有检验项目
                    List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);

                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        string com_itm_id = drComItem.ComItmId;
                        string com_itm_ecd = SQLFormater.Format(drComItem.ComItmEname);

                        if (!listNotNullItem.Exists(i => i == com_itm_id))
                        {
                            if (!string.IsNullOrEmpty(drComItem.ComMustItem))
                            {
                                if (Convert.ToInt32(drComItem.ComMustItem) == 1)
                                {
                                    listNotNullItem.Add(com_itm_id);

                                    if (this.dtPatientResulto != null && this.dtPatientResulto.FindAll(i => i.ItmId == com_itm_id && !string.IsNullOrEmpty(i.ObrValue)).Count > 0)
                                    {
                                        listNotNullItemHasResult.Add(com_itm_id);
                                    }
                                }
                            }
                        }
                    }
                }

                this.lblNotEmptyItem.Text = string.Format("必录项目数：{0}/{1}", listNotNullItemHasResult.Count, listNotNullItem.Count);
            }
            else
            {
                this.lblNotEmptyItem.Text = string.Format("必录项目数：{0}/{0}", 0);
            }
        }

        private string repStatus;
        public void LoadResult(EntityPidReportMain dtPatInfo, List<EntityObrResult> dtPatResult, bool loadDefaVal)
        {
            IsPatResultDoubleColVChange = false;
            listSysOprLog = null;
            Lab_MarkModifyResultWithColor = UserInfo.GetSysConfigValue("Lab_MarkModifyResultWithColor") == "是";
            if (dtPatInfo != null)
            {
                lblCombine.Text = dtPatInfo.PidComName;
                PatID = dtPatInfo.RepId;
                PatInNo = dtPatInfo.PidInNo;
                Pat_itr_id = dtPatInfo.RepItrId;
                samtypeid = dtPatInfo.PidSamId;
                if (!Compare.IsNullOrDBNull(dtPatInfo.PidAge))
                    patage = Convert.ToInt32(dtPatInfo.PidAge);
                else
                    patage = -1;
                patsex = dtPatInfo.PidSex;
                repStatus = dtPatInfo.RepStatus.ToString();
                if (dtPatInfo.RepRecheckFlag.ToString() == "1")
                    menuResultCheckConfirm.Enabled = true;
                else
                    menuResultCheckConfirm.Enabled = false;
            }
            else
                PatID = string.Empty;

            if (Config != null && Config.OrderByItmSeq)
                dtPatResult = dtPatResult.OrderBy(i => i.ItmSeq).ThenBy(i => i.ItmEname.TrimEnd()).ToList();
            else
                dtPatResult = dtPatResult.OrderBy(i => i.ResComSeq).ThenBy(i => i.ComMiSort).ThenBy(i => i.ItmSeq).ThenBy(i => i.ItmEname.TrimEnd()).ToList();

            dtPatientResulto = dtPatResult;
            dtPatient = dtPatInfo;
            this.gridViewSingle.Columns["history_result1"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result2"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result3"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result12"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result22"].Caption = "历史结果";
            this.gridViewSingle.Columns["history_result32"].Caption = "历史结果";
            DataTable dtPatDoubleRes = new DataTable();
            dtPatDoubleRes.Columns.Add("res_key");
            dtPatDoubleRes.Columns.Add("res_itm_id");
            dtPatDoubleRes.Columns.Add("res_itm_seq");
            dtPatDoubleRes.Columns.Add("res_itm_name");
            dtPatDoubleRes.Columns.Add("res_itm_ecd");
            dtPatDoubleRes.Columns.Add("res_chr");
            dtPatDoubleRes.Columns.Add("res_ref_flag");
            dtPatDoubleRes.Columns.Add("res_type");
            dtPatDoubleRes.Columns.Add("res_ref_range");
            dtPatDoubleRes.Columns.Add("history_result1");
            dtPatDoubleRes.Columns.Add("history_result2");
            dtPatDoubleRes.Columns.Add("history_result3");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h1");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h2");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h3");
            dtPatDoubleRes.Columns.Add("calculate_dest_itm_id");
            dtPatDoubleRes.Columns.Add("res_recheck_flag");
            dtPatDoubleRes.Columns.Add("res_allow_values");
            dtPatDoubleRes.Columns.Add("res_ref_l_cal");
            dtPatDoubleRes.Columns.Add("res_ref_h_cal");
            dtPatDoubleRes.Columns.Add("res_pan_l_cal");
            dtPatDoubleRes.Columns.Add("res_pan_h_cal");
            dtPatDoubleRes.Columns.Add("res_min_cal");
            dtPatDoubleRes.Columns.Add("res_max_cal");
            dtPatDoubleRes.Columns.Add("history_date1");
            dtPatDoubleRes.Columns.Add("history_date2");
            dtPatDoubleRes.Columns.Add("history_date3");

            dtPatDoubleRes.Columns.Add("res_key2");
            dtPatDoubleRes.Columns.Add("res_itm_id2");
            dtPatDoubleRes.Columns.Add("res_itm_seq2");
            dtPatDoubleRes.Columns.Add("res_itm_name2");
            dtPatDoubleRes.Columns.Add("res_itm_ecd2");
            dtPatDoubleRes.Columns.Add("res_chr2");
            dtPatDoubleRes.Columns.Add("res_ref_flag2");
            dtPatDoubleRes.Columns.Add("res_type2");
            dtPatDoubleRes.Columns.Add("res_ref_range2");
            dtPatDoubleRes.Columns.Add("history_result12");
            dtPatDoubleRes.Columns.Add("history_result22");
            dtPatDoubleRes.Columns.Add("history_result32");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h12");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h22");
            dtPatDoubleRes.Columns.Add("res_ref_flag_h32");
            dtPatDoubleRes.Columns.Add("calculate_dest_itm_id2");
            dtPatDoubleRes.Columns.Add("res_recheck_flag2");
            dtPatDoubleRes.Columns.Add("res_allow_values2");
            dtPatDoubleRes.Columns.Add("res_ref_l_cal2");
            dtPatDoubleRes.Columns.Add("res_ref_h_cal2");
            dtPatDoubleRes.Columns.Add("res_pan_l_cal2");
            dtPatDoubleRes.Columns.Add("res_pan_h_cal2");
            dtPatDoubleRes.Columns.Add("res_min_cal2");
            dtPatDoubleRes.Columns.Add("res_max_cal2");
            dtPatDoubleRes.Columns.Add("history_date12");
            dtPatDoubleRes.Columns.Add("history_date22");
            dtPatDoubleRes.Columns.Add("history_date32");

            int RowCount = dtPatResult.Count;

            int DoubleColumnsCount = 19;

            //系统配置：报告管理[双列浏览]默认行数
            string str_DoubleColumnRowCount = UserInfo.GetSysConfigValue("Lab_DoubleColumnRowCount");
            int temp_intDoubleColumnRowCount = 0;
            if (!string.IsNullOrEmpty(str_DoubleColumnRowCount)
                && int.TryParse(str_DoubleColumnRowCount, out temp_intDoubleColumnRowCount)
                && temp_intDoubleColumnRowCount > 1)
            {
                DoubleColumnsCount = temp_intDoubleColumnRowCount;
            }

            if (RowCount > (DoubleColumnsCount * 2))
            {
                if (RowCount % 2 == 0)
                    DoubleColumnsCount = RowCount / 2;
                else
                    DoubleColumnsCount = (RowCount + 1) / 2;
            }

            lblItemCount.Text = RowCount == 0 ? string.Empty : RowCount.ToString();

            for (int i = 0; i < RowCount; i++)
            {
                CalcObrResult(dtPatResult[i], true);

                string seq = (i + 1).ToString();
                if (!string.IsNullOrEmpty(dtPatResult[i].IsNotEmpty) &&
                    dtPatResult[i].IsNotEmpty == "1")
                    seq += "*";

                if (i < DoubleColumnsCount)
                {
                    DataRow drPatDoubleRes = dtPatDoubleRes.NewRow();
                    drPatDoubleRes["res_key"] = dtPatResult[i].ObrSn;
                    drPatDoubleRes["res_itm_id"] = dtPatResult[i].ItmId;
                    drPatDoubleRes["res_itm_seq"] = seq;
                    drPatDoubleRes["res_itm_name"] = dtPatResult[i].ItmName;
                    drPatDoubleRes["res_itm_ecd"] = dtPatResult[i].ItmEname.TrimEnd();
                    drPatDoubleRes["res_chr"] = dtPatResult[i].ObrValue;
                    drPatDoubleRes["res_ref_flag"] = dtPatResult[i].RefFlag;
                    drPatDoubleRes["res_ref_range"] = dtPatResult[i].ResRefRange;
                    drPatDoubleRes["history_result1"] = dtPatResult[i].HistoryResult1;
                    drPatDoubleRes["history_result2"] = dtPatResult[i].HistoryResult2;
                    drPatDoubleRes["history_result3"] = dtPatResult[i].HistoryResult3;
                    drPatDoubleRes["res_ref_flag_h1"] = dtPatResult[i].RefFlagHistory1;
                    drPatDoubleRes["res_ref_flag_h2"] = dtPatResult[i].RefFlagHistory2;
                    drPatDoubleRes["res_ref_flag_h3"] = dtPatResult[i].RefFlagHistory3;
                    drPatDoubleRes["res_type"] = dtPatResult[i].ObrType;
                    drPatDoubleRes["calculate_dest_itm_id"] = dtPatResult[i].CalculateDestItmId;
                    drPatDoubleRes["res_recheck_flag"] = dtPatResult[i].ObrRecheckFlag;
                    drPatDoubleRes["res_allow_values"] = dtPatResult[i].ResAllowValues;
                    drPatDoubleRes["res_ref_l_cal"] = dtPatResult[i].ResRefLCal;
                    drPatDoubleRes["res_ref_h_cal"] = dtPatResult[i].ResRefHCal;
                    drPatDoubleRes["res_pan_l_cal"] = dtPatResult[i].ResPanLCal;
                    drPatDoubleRes["res_pan_h_cal"] = dtPatResult[i].ResPanHCal;
                    drPatDoubleRes["res_min_cal"] = dtPatResult[i].ResMinCal;
                    drPatDoubleRes["res_max_cal"] = dtPatResult[i].ResMaxCal;
                    drPatDoubleRes["history_date1"] = dtPatResult[i].HistoryDate1;
                    drPatDoubleRes["history_date2"] = dtPatResult[i].HistoryDate2;
                    drPatDoubleRes["history_date3"] = dtPatResult[i].HistoryDate3;

                    dtPatDoubleRes.Rows.Add(drPatDoubleRes);
                }
                else
                {
                    DataRow drPatDoubleRes = dtPatDoubleRes.Rows[i - DoubleColumnsCount];
                    drPatDoubleRes["res_key2"] = dtPatResult[i].ObrSn;
                    drPatDoubleRes["res_itm_id2"] = dtPatResult[i].ItmId;
                    drPatDoubleRes["res_itm_seq2"] = seq;
                    drPatDoubleRes["res_itm_name2"] = dtPatResult[i].ItmName;
                    drPatDoubleRes["res_itm_ecd2"] = dtPatResult[i].ItmEname.TrimEnd();
                    drPatDoubleRes["res_chr2"] = dtPatResult[i].ObrValue;
                    drPatDoubleRes["res_ref_flag2"] = dtPatResult[i].RefFlag;
                    drPatDoubleRes["res_ref_range2"] = dtPatResult[i].ResRefRange;
                    drPatDoubleRes["history_result12"] = dtPatResult[i].HistoryResult1;
                    drPatDoubleRes["history_result22"] = dtPatResult[i].HistoryResult2;
                    drPatDoubleRes["history_result32"] = dtPatResult[i].HistoryResult3;
                    drPatDoubleRes["res_ref_flag_h12"] = dtPatResult[i].RefFlagHistory1;
                    drPatDoubleRes["res_ref_flag_h22"] = dtPatResult[i].RefFlagHistory2;
                    drPatDoubleRes["res_ref_flag_h32"] = dtPatResult[i].RefFlagHistory3;
                    drPatDoubleRes["res_type2"] = dtPatResult[i].ObrType;
                    drPatDoubleRes["calculate_dest_itm_id2"] = dtPatResult[i].CalculateDestItmId;
                    drPatDoubleRes["res_recheck_flag2"] = dtPatResult[i].ObrRecheckFlag;
                    drPatDoubleRes["res_allow_values2"] = dtPatResult[i].ResAllowValues;
                    drPatDoubleRes["res_ref_l_cal2"] = dtPatResult[i].ResRefLCal;
                    drPatDoubleRes["res_ref_h_cal2"] = dtPatResult[i].ResRefHCal;
                    drPatDoubleRes["res_pan_l_cal2"] = dtPatResult[i].ResPanLCal;
                    drPatDoubleRes["res_pan_h_cal2"] = dtPatResult[i].ResPanHCal;
                    drPatDoubleRes["res_min_cal2"] = dtPatResult[i].ResMinCal;
                    drPatDoubleRes["res_max_cal2"] = dtPatResult[i].ResMaxCal;
                    drPatDoubleRes["history_date12"] = dtPatResult[i].HistoryDate1;
                    drPatDoubleRes["history_date22"] = dtPatResult[i].HistoryDate2;
                    drPatDoubleRes["history_date32"] = dtPatResult[i].HistoryDate3;
                }
            }
            if (Lab_MarkModifyResultWithColor)
            {
                SetModifyFlag(dtPatDoubleRes);
            }
            gridControlSingle.DataSource = dtPatDoubleRes;

            for (int i = 1; i <= 3; i++)
            {
                List<EntityObrResult> rows = new List<EntityObrResult>();
                string strTime = string.Empty;
                if (i == 1)
                {
                    rows = dtPatResult.FindAll(w => !string.IsNullOrEmpty(w.HistoryResult1) && !string.IsNullOrEmpty(w.HistoryDate1));

                }
                else if (i == 2)
                {
                    rows = dtPatResult.FindAll(w => !string.IsNullOrEmpty(w.HistoryResult2) && !string.IsNullOrEmpty(w.HistoryDate2));
                }
                else
                {
                    rows = dtPatResult.FindAll(w => !string.IsNullOrEmpty(w.HistoryResult3) && !string.IsNullOrEmpty(w.HistoryDate3));
                }
                if (rows.Count > 0)
                {
                    if (i == 1) strTime = DateTime.Parse(rows[0].HistoryDate1).ToString("yy/MM/dd");
                    if (i == 2) strTime = DateTime.Parse(rows[0].HistoryDate2).ToString("yy/MM/dd");
                    if (i == 3) strTime = DateTime.Parse(rows[0].HistoryDate3).ToString("yy/MM/dd");

                    this.gridViewSingle.Columns["history_result" + i.ToString()].Caption = "历史结果" + "\r\n" + strTime;
                    this.gridViewSingle.Columns["history_result" + i.ToString() + "2"].Caption = "历史结果" + "\r\n" + strTime;
                }
            }

            Thread t = new Thread(ThreadLoadHistory);
            t.Start();
        }

        private void ThreadLoadHistory()
        {
            try
            {
                if (!string.IsNullOrEmpty(PatID))
                {
                    List<EntityObrResult> listResult = new List<EntityObrResult>();
                    listResult = new ProxyPatResult().Service.GetPatCommonResultHistoryWithRef(PatID, 10, Convert.ToDateTime("2100-01-01"));
                    if (listResult == null || listResult.Count == 0 || listResult[0].PidInNo != PatInNo || this.gridControlSingle==null) return;
                    SetHistoryRes(this.gridControlSingle, listResult);
                }

            }
            catch (Exception EX)
            {
                Lib.LogManager.Logger.LogException(EX);
            }
        }

        private delegate void DelegateSetPatStatus(GridControl grid, List<EntityObrResult> data);
        private void SetHistoryRes(GridControl grid, List<EntityObrResult> data)
        {
            if (grid.InvokeRequired)
            {
                DelegateSetPatStatus del = new DelegateSetPatStatus(SetHistoryRes);
                this.Invoke(del, new object[] { grid, data });
            }
            else
            {
                try
                {
                    if (grid.DataSource != null && grid.DataSource is DataTable)
                    {
                        DataTable source = grid.DataSource as DataTable;

                        if (source.Rows.Count == 0) return;

                        StringBuilder sbResultId = new StringBuilder();
                        foreach (EntityObrResult drResult in dtPatientResulto)
                        {
                            sbResultId.Append(string.Format(",'{0}'", drResult.ItmId));
                        }
                        sbResultId.Remove(0, 1);

                        //选出包含于项目列表的病人结果并根据病人ID去重
                        List<EntityObrResult> dtTimes = (from x in data where sbResultId.ToString().Contains(x.ItmId) select x)
                            .GroupBy(i => i.ObrId).Select(i => i.First()).OrderByDescending(i => i.ObrDate).ToList();


                        try
                        {
                            if (dtTimes.Count > 0)
                            {
                                for (int j = 0; j < dtTimes.Count; j++)
                                {
                                    if (j >= 3)
                                    {
                                        break;
                                    }
                                    EntityObrResult drTime = dtTimes[j];
                                    foreach (DataRow drResult in source.Rows)
                                    {
                                        if (drResult["res_itm_id"] != DBNull.Value && !string.IsNullOrEmpty(drResult["res_itm_id"].ToString()))
                                        {
                                            string itm_id = drResult["res_itm_id"].ToString();

                                            int resultIndex = data.FindIndex(i => i.ItmId == itm_id && i.ObrId == drTime.ObrId);

                                            if (resultIndex > -1)
                                            {
                                                EntityObrResult result = data[resultIndex];
                                                string strTime = result.ObrDate.ToString("yy/MM/dd");
                                                drResult["history_result" + (j + 1).ToString()] = result.ObrValue;
                                                this.gridViewSingle.Columns["history_result" + (j + 1).ToString()].Caption = "历史结果" + "\r\n" + strTime;
                                                this.gridViewSingle.Columns["history_result" + (j + 1).ToString() + "2"].Caption = "历史结果" + "\r\n" + strTime;
                                                drResult["history_date" + (j + 1).ToString()] = result.ObrDate.ToString();
                                                drResult["history_date" + (j + 1).ToString()+"2"] = result.ObrDate.ToString();
                                                //drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                //source.Rows[0]["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                        }
                                        if (drResult["res_itm_id2"] != DBNull.Value && !string.IsNullOrEmpty(drResult["res_itm_id2"].ToString()))
                                        {
                                            string itm_id = drResult["res_itm_id2"].ToString();

                                            int resultIndex = data.FindIndex(i => i.ItmId == itm_id && i.ObrId == drTime.ObrId);

                                            if (resultIndex > -1)
                                            {
                                                EntityObrResult result = data[resultIndex];
                                                string strTime = result.ObrDate.ToString("yy/MM/dd");
                                                drResult["history_result" + (j + 1).ToString() + "2"] = result.ObrValue;
                                                this.gridViewSingle.Columns["history_result" + (j + 1).ToString() + "2"].Caption = "历史结果" + "\r\n" + strTime;
                                                this.gridViewSingle.Columns["history_result" + (j + 1).ToString()].Caption = "历史结果" + "\r\n" + strTime;
                                                drResult["history_date" + (j + 1).ToString()] = result.ObrDate.ToString();
                                                drResult["history_date" + (j + 1).ToString() + "2"] = result.ObrDate.ToString();
                                                //drResult["history_date" + (j + 1).ToString() + "2"] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                //source.Rows[0]["history_date" + (j + 1).ToString() + "2"] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            }
                                        }
                                        CalcPatHistoryResultRow(drResult, true);
                                    }
                                }
                                #region 过滤"必录+历史结果1
                                if (UserInfo.GetSysConfigValue("Lab_PopedomFilter") == "必录+历史结果" && (repStatus != "2" && repStatus != "4"))
                                {
                                    List<EntityObrResult>dataFilter= PatientHistoryViewFilter(data);
                                    if (dtPatientResulto == null || dtPatientResulto.Count == 0) return;
                                    List<string> listItmId = new List<string>();
                                    if (dataFilter != null && dataFilter.Count > 0)
                                    {
                                        foreach (EntityObrResult item in dataFilter)
                                        {
                                            //找出没有显示历史结果1的项目
                                            if (dtPatientResulto.FindAll(w => w.ItmId == item.ItmId).Count <= 0)
                                                listItmId.Add(item.ItmId);
                                        }
                                    }
                                    //}

                                    int count = 0;
                                    int seq = dtPatientResulto.Count;
                                    if (dtPatientResulto.Count % 36 == 0)
                                    {
                                        if (listItmId.Count % 2 == 0)
                                        {
                                            for (int i = 0; i < listItmId.Count / 2; i++)
                                            {
                                                DataRow dr = source.NewRow();
                                                dr["res_com_seq"] = 999;
                                                source.Rows.Add(dr);
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < listItmId.Count / 2 + 1; i++)
                                            {
                                                DataRow dr = source.NewRow();
                                                dr["res_com_seq"] = 999;
                                                source.Rows.Add(dr);
                                            }
                                        }

                                    }

                                    foreach (DataRow drPatDoubleRes in source.Rows)
                                    {
                                        //防止重复添加
                                        if (count >= listItmId.Count || source.Select("res_itm_id='" + listItmId[count] + "'").Length > 0 || source.Select("res_itm_id2='" + listItmId[count] + "'").Length > 0) continue;
                                        if (drPatDoubleRes["res_itm_id"] == DBNull.Value || string.IsNullOrEmpty(drPatDoubleRes["res_itm_id"].ToString()))
                                        {
                                            List<EntityObrResult> dtPatResult = dataFilter.FindAll(w => w.ItmId == listItmId[count]);
                                            EntityObrResult dr = new EntityObrResult();
                                            drPatDoubleRes["res_itm_id"] = dtPatResult[0].ItmId;
                                            drPatDoubleRes["res_itm_seq"] = seq + 1;
                                            drPatDoubleRes["res_itm_name"] = dtPatResult[0].ItmName;
                                            drPatDoubleRes["res_ref_flag"] = dtPatResult[0].RefFlag;
                                            if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range"] = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                                            }
                                            else if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range"] = dtPatResult[0].RefLowerLimit;
                                            }
                                            else if (string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range"] = dtPatResult[0].RefUpperLimit;
                                            }
                                            drPatDoubleRes["res_ref_l_cal"] = dtPatResult[0].RefLowerLimit;
                                            drPatDoubleRes["res_ref_h_cal"] = dtPatResult[0].RefUpperLimit;
                                            drPatDoubleRes["history_result1"] = dtPatResult[0].ObrValue;
                                            drPatDoubleRes["res_type"] = dtPatResult[0].ObrType;
                                            drPatDoubleRes["res_recheck_flag"] = dtPatResult[0].ObrRecheckFlag;
                                            dr.ItmId = dtPatResult[0].ItmId;
                                            dr.ComMiSort = seq + 1;
                                            dr.ItmName = dtPatResult[0].ItmName;
                                            dr.ItmEname = dtPatResult[0].ItmEname.TrimEnd();
                                            dr.ResRefRange = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                                            dr.ResRefLCal = dtPatResult[0].RefLowerLimit;
                                            dr.ResRefHCal = dtPatResult[0].RefUpperLimit;
                                            dr.HistoryResult1 = dtPatResult[0].ObrValue;
                                            dr.ObrType = dtPatResult[0].ObrType;
                                            dr.ObrRecheckFlag= dtPatResult[0].ObrRecheckFlag;
                                            dr.IsNew= 1;
                                            dr.ItmComId = "-1";
                                            dr.ObrDate = ServerDateTime.GetServerDateTime();
                                            dtPatientResulto.Add(dr);
                                            count++;
                                            seq++;
                                        }
                                        if (drPatDoubleRes["res_itm_id2"] == DBNull.Value || string.IsNullOrEmpty(drPatDoubleRes["res_itm_id2"].ToString()))
                                        {
                                            EntityObrResult dr = new EntityObrResult();
                                            List<EntityObrResult> dtPatResult = dataFilter.FindAll(w => w.ItmId == listItmId[count]);
                                            drPatDoubleRes["res_itm_id2"] = dtPatResult[0].ItmId;
                                            drPatDoubleRes["res_itm_seq2"] = seq + 1;
                                            drPatDoubleRes["res_itm_name2"] = dtPatResult[0].ItmName;
                                            drPatDoubleRes["res_ref_flag2"] = dtPatResult[0].RefFlag;
                                            if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range2"] = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                                            }
                                            else if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range2"] = dtPatResult[0].RefLowerLimit;
                                            }
                                            else if (string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                            {
                                                drPatDoubleRes["res_ref_range2"] = dtPatResult[0].RefUpperLimit;
                                            }
                                            drPatDoubleRes["res_ref_l_cal2"] = dtPatResult[0].RefLowerLimit;
                                            drPatDoubleRes["res_ref_h_cal2"] = dtPatResult[0].RefUpperLimit;
                                            drPatDoubleRes["history_result12"] = dtPatResult[0].ObrValue;
                                            drPatDoubleRes["res_type2"] = dtPatResult[0].ObrType;
                                            drPatDoubleRes["res_recheck_flag2"] = dtPatResult[0].ObrRecheckFlag;
                                            dr.ItmId = dtPatResult[0].ItmId;
                                            dr.ComMiSort = seq + 1;
                                            dr.ItmName = dtPatResult[0].ItmName;
                                            dr.ItmEname = dtPatResult[0].ItmEname.TrimEnd();
                                            dr.ResRefRange = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                                            dr.ResRefLCal = dtPatResult[0].RefLowerLimit;
                                            dr.ResRefHCal = dtPatResult[0].RefUpperLimit;
                                            dr.HistoryResult1 = dtPatResult[0].ObrValue;
                                            dr.ObrType = dtPatResult[0].ObrType;
                                            dr.ObrRecheckFlag = dtPatResult[0].ObrRecheckFlag;
                                            dr.IsNew = 1;
                                            dr.ItmComId = "-1";
                                            dr.ObrDate = ServerDateTime.GetServerDateTime();
                                            dtPatientResulto.Add(dr);
                                            count++;
                                            seq++;
                                        }

                                    }
                                    foreach (DataRow drPatDoubleRes in source.Rows)
                                    {
                                        CalcPatHistoryResultRow(drPatDoubleRes, true);
                                    }
                                }

                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException(this.GetType().Name, "reshistroyfill", ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
                finally
                {

                }
            }
        }
        /// <summary>
        /// 历史结果过滤不包含此组合的项目
        /// </summary>
        /// <param name="listPatientHistory"></param>
        private List<EntityObrResult> PatientHistoryViewFilter(List<EntityObrResult> listPatientHistory)
        {
            List<EntityObrResult> listHistroyResult = new List<EntityObrResult>();
            string comIds = string.Empty;
            if (patients_mi != null && patients_mi.Count > 0)
            {
                foreach (EntityPidReportDetail detail in this.patients_mi)
                {
                    string com_id = detail.ComId;
                    if (!comIds.Contains(com_id))
                    {
                        comIds += string.Format(",'{0}'", com_id);
                    }
                }
            }
            if (string.IsNullOrEmpty(comIds))
                return listHistroyResult;
            comIds = comIds.Remove(0, 1);
            string sbItmId = string.Empty;
            List<EntityDicCombineDetail> listComDetailCache = CacheClient.GetCache("EntityDicCombineDetail") as List<EntityDicCombineDetail>;
            List<EntityDicCombineDetail> listComDetail = (from x in listComDetailCache where comIds.Contains(x.ComId) select x).ToList();
            if (listComDetail != null && listComDetail.Count > 0)
            {
                foreach (EntityDicCombineDetail detail in listComDetail)
                {
                    sbItmId += string.Format(",'{0}'", detail.ComItmId);
                }
            }
            if (string.IsNullOrEmpty(sbItmId))
                return listHistroyResult;
            sbItmId = sbItmId.Remove(0, 1);

            listHistroyResult = (from x in listPatientHistory where sbItmId.Contains(x.ItmId) select x).OrderByDescending(w => w.ObrDate).ToList();
            return listHistroyResult;
        }
        /// <summary>
        /// 对病人结果表中的某一行的参考值和是否超出;结果数据类型是否正确
        /// </summary>
        /// <param name="drResult"></param>
        public void CalcObrResult(EntityObrResult drResult, bool bShowHint)
        {
            if (drResult != null && !Compare.IsNullOrDBNull(drResult.ObrValue))
            {
                #region 判断有没有大小于号结果 edit by zheng
                //结果
                string strValue = drResult.ObrValue;
                //结果符号，为>号或<号
                string strSymbol = "";
                if (strValue.Contains(">"))
                {
                    strSymbol = ">";
                    strValue = strValue.TrimStart('>');
                }
                else if (strValue.Contains("<"))
                {
                    strSymbol = "<";
                    strValue = strValue.TrimStart('<');
                }

                #endregion

                #region 判断历史结果有没有大小于号 edit by zheng

                string strValue1 = "";
                string strValue2 = "";
                string strValue3 = "";
                //结果符号，为>号或<号
                string strSymbol1 = "";
                string strSymbol2 = "";
                string strSymbol3 = "";



                if (!Compare.IsNullOrDBNull(drResult.HistoryResult1))
                {
                    //结果
                    strValue1 = drResult.HistoryResult1;


                    if (strValue1.Contains(">"))
                    {
                        strSymbol1 = ">";
                        strValue1 = strValue1.TrimStart('>');
                    }
                    else if (strValue1.Contains("<"))
                    {
                        strSymbol1 = "<";
                        strValue1 = strValue1.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult.HistoryResult2))
                {
                    //结果
                    strValue2 = drResult.HistoryResult2;

                    if (strValue2.Contains(">"))
                    {
                        strSymbol2 = ">";
                        strValue2 = strValue2.TrimStart('>');
                    }
                    else if (strValue2.Contains("<"))
                    {
                        strSymbol2 = "<";
                        strValue2 = strValue2.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult.HistoryResult3))
                {
                    //结果
                    strValue3 = drResult.HistoryResult3;

                    if (strValue1.Contains(">"))
                    {
                        strSymbol3 = ">";
                        strValue3 = strValue3.TrimStart('>');
                    }
                    else if (strValue3.Contains("<"))
                    {
                        strSymbol3 = "<";
                        strValue3 = strValue3.TrimStart('<');
                    }
                }

                #endregion

                decimal decValue;
                #region 历史结果decValue
                decimal decValue1;
                decimal decValue2;
                decimal decValue3;
                #endregion

                if (strValue.Contains("/"))
                {
                    string[] splited = strValue.Split('/');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = (Math.Round((decLeft / decRight), 8)).ToString();
                        }
                    }
                }
                else if (strValue.Contains(":"))
                {
                    string[] splited = strValue.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = splited[1];
                        }
                    }
                }
                else if (strValue.Contains("e") || strValue.Contains("E"))
                {
                    double d;

                    if (double.TryParse(strValue.Replace(" ", ""), out d))//科学计数法可转换为double
                    {
                        strValue = Convert.ToDecimal(d).ToString();
                    }
                }

                if (
                    !strValue.Contains("+")
                    &&
                    decimal.TryParse(strValue, out decValue)
                    //&& drResult.Table.Columns.Contains("res_allow_values")
                    && drResult.ResAllowValues.Replace(" ", "").Length == 0
                    )//是否数值型结果
                {
                    string strItmRef_l = string.Empty;//参考值下限
                    string strItmRef_h = string.Empty;//参考值上限
                    string strItmPan_l = string.Empty;//危急值下限
                    string strItmPan_h = string.Empty;//危急值上限
                    string strItm_min = string.Empty;//阈值下限
                    string strItm_max = string.Empty;//阈值上限

                    //是否存在参考值
                    bool hasRef = false;

                    if (!Compare.IsNullOrDBNull(drResult.ResRefLCal))
                    {
                        strItmRef_l = drResult.ResRefLCal.ToString();
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult.ResRefHCal))
                    {
                        strItmRef_h = drResult.ResRefHCal;
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult.ResPanLCal))
                    {
                        strItmPan_l = drResult.ResPanLCal;
                    }

                    if (!Compare.IsNullOrDBNull(drResult.ResPanHCal))
                    {
                        strItmPan_h = drResult.ResPanHCal;
                    }

                    if (!Compare.IsNullOrDBNull(drResult.ResMinCal))
                    {
                        strItm_min = drResult.ResMinCal;
                    }

                    if (!Compare.IsNullOrDBNull(drResult.ResMaxCal))
                    {
                        strItm_max = drResult.ResMaxCal;
                    }

                    decimal decItmRef_l;
                    decimal decItmRef_h;
                    decimal decItmMax;
                    decimal decItmMin;
                    decimal decItmPan_l;
                    decimal decItmpan_h;
                    bool refLowAndHighIsString = false; //参考值下限是否是字符串  2010-7-1

                    EnumResRefFlag enumResRefFlag = EnumResRefFlag.Normal;

                    //历史结果的
                    EnumResRefFlag enumResRefFlag1 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag2 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag3 = EnumResRefFlag.Normal;
                    string message = string.Empty;

                    //低于参考值下限
                    if (decimal.TryParse(strItmRef_l, out decItmRef_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol)) //edit by zheng
                        {
                            if (strSymbol == "<" && decValue <= decItmRef_l)
                            {
                                //drResult.RefFlag = "2";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                            }
                        }
                        else if (decValue < decItmRef_l)
                        {
                            //drResult.RefFlag = "2";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                            message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmRef_l)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue1 < decItmRef_l)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmRef_l)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue2 < decItmRef_l)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmRef_l)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue3 < decItmRef_l)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion
                    }


                    //高于参考值上限
                    if (decimal.TryParse(strItmRef_h, out decItmRef_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmRef_h) // && zheng
                            {
                                //drResult.RefFlag = "1";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                            }
                        }
                        else if (decValue > decItmRef_h)
                        {
                            //drResult.RefFlag = "1";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                            message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmRef_h)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue1 > decItmRef_h)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmRef_h)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue2 > decItmRef_h)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmRef_h)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue3 > decItmRef_h)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion
                    }

                    if (decimal.TryParse(strItmRef_h, out decItmRef_h) == false && decimal.TryParse(strItmRef_l, out decItmRef_l) == false)
                    //参考值为字符  2010-7-22
                    {
                        refLowAndHighIsString = true;
                    }

                    //低于极小阈值
                    if (decimal.TryParse(strItm_min, out decItmMin))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmMin) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                                message += string.Format("低于极小阈值[{0}];", decItmMin);
                            }
                        }
                        else if (decValue < decItmMin)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                            //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                            message += string.Format("低于极小阈值[{0}];", decItmMin);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmMin)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue1 < decItmMin)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmMin)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue2 < decItmMin)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmMin)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue3 < decItmMin)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion
                    }

                    //高于极大阈值
                    if (decimal.TryParse(strItm_max, out decItmMax))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmMax) //edit by zheng && sink 2010-8-5
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                                message += string.Format("高于极大阈值[{0}];", decItmMax);
                            }
                        }
                        else if (decValue > decItmMax)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                            //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                            message += string.Format("高于极大阈值[{0}];", decItmMax);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmMax)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue1 > decItmMax)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmMax)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue2 > decItmMax)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmMax)
                                {

                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue3 > decItmMax)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion
                    }

                    //低于危机值下限
                    if (decimal.TryParse(strItmPan_l, out decItmPan_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmPan_l) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                            }
                        }
                        else if (decValue < decItmPan_l)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                            message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                        }
                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmPan_l)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue1 < decItmPan_l)
                            {


                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;

                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmPan_l)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue2 < decItmPan_l)
                            {


                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmPan_l)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue3 < decItmPan_l)
                            {


                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;


                            }
                        }
                        #endregion
                    }

                    //高于危机值上限
                    if (decimal.TryParse(strItmPan_h, out decItmpan_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmpan_h) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                            }
                        }
                        else if (decValue > decItmpan_h)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                            message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmpan_h)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue1 > decItmpan_h)
                            {


                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmpan_h)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue2 > decItmpan_h)
                            {


                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmpan_h)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue3 > decItmpan_h)
                            {


                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion
                    }



                    if (!hasRef)//如果没有参考值
                    {
                        drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                    }
                    else
                    {
                        if (refLowAndHighIsString)//参考值上下限是否是字符串  2010-7-22
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                        else// if (enumResRefFlag == EnumResRefFlag.Normal)
                        {
                            drResult.RefFlag = ((int)enumResRefFlag).ToString();
                            //drResult.RefFlag = ((int)EnumResRefFlag.Normal).ToString();

                            drResult.RefFlagHistory1 = ((int)enumResRefFlag1);

                            drResult.RefFlagHistory2 = ((int)enumResRefFlag2);

                            drResult.RefFlagHistory3 = ((int)enumResRefFlag3);
                        }
                        //else
                        //{
                        //drResult.RefFlag = ((int)enumResRefFlag).ToString();
                        //历史结果
                        //drResult.RefFlagHistory1 = ((int)enumResRefFlag1).ToString();

                        //drResult.RefFlagHistory2 = ((int)enumResRefFlag2).ToString();

                        //drResult.RefFlagHistory3 = ((int)enumResRefFlag3).ToString();
                        //}
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(drResult.ResAllowValues)
                        && drResult.ResAllowValues.Trim().Length > 0)
                    {
                        string[] allowValues = drResult.ResAllowValues.Trim().Split(',');

                        bool value_not_allow = true;
                        foreach (string allow_value in allowValues)
                        {
                            if (allow_value.IndexOf("$") > -1)
                            {
                                //string allow_value_check= allow_value.Split("$")[1];
                                value_not_allow = false;
                                break;
                            }
                            else
                            {

                                if (strValue == allow_value.Trim())
                                {
                                    value_not_allow = false;
                                    break;
                                }
                                else
                                {
                                    value_not_allow = value_not_allow & true;
                                }
                            }
                        }

                        if (value_not_allow)
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                        }
                        else
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Normal).ToString();
                        }
                    }
                    else if (strValue != null
                        &&
                            (
                                strValue.Trim() == "+"
                                || strValue.StartsWith("+")
                                || strValue.EndsWith("+")
                                || strValue.IndexOf("阳性") >= 0
                                || strValue.ToLower() == "pos"
                            )
                        && !strValue.Trim().Contains("弱阳性")
                        )
                    {
                        drResult.RefFlag = ((int)EnumResRefFlag.Positive).ToString();
                    }
                    else if (strValue.Trim().Contains("弱阳性"))
                    {
                        drResult.RefFlag = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                        //grid.SetRowCellValue(rowHandle, "res_ref_flag", ((int)EnumResRefFlag.WeaklyPositive).ToString());
                    }
                    else if (strValue.Trim().Contains("#"))
                        drResult.RefFlag = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                    else
                    {
                        drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                    }


                }
            }

        }

        /// <summary>
        /// 对病人结果表中的某一行的参考值和是否超出;结果数据类型是否正确
        /// </summary>
        /// <param name="drResult"></param>
        public void CalcPatResultRow(DataRow drResult, bool bShowHint)
        {
            if (drResult != null && !Compare.IsNullOrDBNull(drResult["res_chr"]))
            {
                #region 判断有没有大小于号结果 edit by zheng
                //结果
                string strValue = drResult["res_chr"].ToString();
                //结果符号，为>号或<号
                string strSymbol = "";
                if (strValue.Contains(">"))
                {
                    strSymbol = ">";
                    strValue = strValue.TrimStart('>');
                }
                else if (strValue.Contains("<"))
                {
                    strSymbol = "<";
                    strValue = strValue.TrimStart('<');
                }

                #endregion

                #region 判断历史结果有没有大小于号 edit by zheng

                string strValue1 = "";
                string strValue2 = "";
                string strValue3 = "";
                //结果符号，为>号或<号
                string strSymbol1 = "";
                string strSymbol2 = "";
                string strSymbol3 = "";



                if (!Compare.IsNullOrDBNull(drResult["history_result1"]))
                {
                    //结果
                    strValue1 = drResult["history_result1"].ToString();


                    if (strValue1.Contains(">"))
                    {
                        strSymbol1 = ">";
                        strValue1 = strValue1.TrimStart('>');
                    }
                    else if (strValue1.Contains("<"))
                    {
                        strSymbol1 = "<";
                        strValue1 = strValue1.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result2"]))
                {
                    //结果
                    strValue2 = drResult["history_result2"].ToString();

                    if (strValue2.Contains(">"))
                    {
                        strSymbol2 = ">";
                        strValue2 = strValue2.TrimStart('>');
                    }
                    else if (strValue2.Contains("<"))
                    {
                        strSymbol2 = "<";
                        strValue2 = strValue2.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result3"]))
                {
                    //结果
                    strValue3 = drResult["history_result3"].ToString();

                    if (strValue1.Contains(">"))
                    {
                        strSymbol3 = ">";
                        strValue3 = strValue3.TrimStart('>');
                    }
                    else if (strValue3.Contains("<"))
                    {
                        strSymbol3 = "<";
                        strValue3 = strValue3.TrimStart('<');
                    }
                }

                #endregion

                decimal decValue;
                #region 历史结果decValue
                decimal decValue1;
                decimal decValue2;
                decimal decValue3;
                #endregion

                if (strValue.Contains("/"))
                {
                    string[] splited = strValue.Split('/');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = (Math.Round((decLeft / decRight), 8)).ToString();
                        }
                    }
                }
                else if (strValue.Contains(":"))
                {
                    string[] splited = strValue.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = splited[1];
                        }
                    }
                }
                else if (strValue.Contains("e") || strValue.Contains("E"))
                {
                    double d;

                    if (double.TryParse(strValue.Replace(" ", ""), out d))//科学计数法可转换为double
                    {
                        strValue = Convert.ToDecimal(d).ToString();
                    }
                }

                if (
                    !strValue.Contains("+")
                    &&
                    decimal.TryParse(strValue, out decValue)
                    //&& drResult.Table.Columns.Contains("res_allow_values")
                    && drResult["res_allow_values"].ToString().Replace(" ", "").Length == 0
                    )//是否数值型结果
                {
                    string strItmRef_l = string.Empty;//参考值下限
                    string strItmRef_h = string.Empty;//参考值上限
                    string strItmPan_l = string.Empty;//危急值下限
                    string strItmPan_h = string.Empty;//危急值上限
                    string strItm_min = string.Empty;//阈值下限
                    string strItm_max = string.Empty;//阈值上限

                    //是否存在参考值
                    bool hasRef = false;

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_l_cal"]))
                    {
                        strItmRef_l = drResult["res_ref_l_cal"].ToString();
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_h_cal"]))
                    {
                        strItmRef_h = drResult["res_ref_h_cal"].ToString();
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_pan_l_cal"]))
                    {
                        strItmPan_l = drResult["res_pan_l_cal"].ToString();
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_pan_h_cal"]))
                    {
                        strItmPan_h = drResult["res_pan_h_cal"].ToString();
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_min_cal"]))
                    {
                        strItm_min = drResult["res_min_cal"].ToString();
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_max_cal"]))
                    {
                        strItm_max = drResult["res_max_cal"].ToString();
                    }

                    decimal decItmRef_l;
                    decimal decItmRef_h;
                    decimal decItmMax;
                    decimal decItmMin;
                    decimal decItmPan_l;
                    decimal decItmpan_h;
                    bool refLowAndHighIsString = false; //参考值下限是否是字符串  2010-7-1

                    EnumResRefFlag enumResRefFlag = EnumResRefFlag.Normal;

                    //历史结果的
                    EnumResRefFlag enumResRefFlag1 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag2 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag3 = EnumResRefFlag.Normal;
                    string message = string.Empty;

                    //低于参考值下限
                    if (decimal.TryParse(strItmRef_l, out decItmRef_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol)) //edit by zheng
                        {
                            if (strSymbol == "<" && decValue <= decItmRef_l)
                            {
                                //drResult["res_ref_flag"] = "2";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                            }
                        }
                        else if (decValue < decItmRef_l)
                        {
                            //drResult["res_ref_flag"] = "2";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                            message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmRef_l)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue1 < decItmRef_l)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmRef_l)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue2 < decItmRef_l)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmRef_l)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue3 < decItmRef_l)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion
                    }


                    //高于参考值上限
                    if (decimal.TryParse(strItmRef_h, out decItmRef_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmRef_h) // && zheng
                            {
                                //drResult["res_ref_flag"] = "1";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                            }
                        }
                        else if (decValue > decItmRef_h)
                        {
                            //drResult["res_ref_flag"] = "1";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                            message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmRef_h)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue1 > decItmRef_h)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmRef_h)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue2 > decItmRef_h)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmRef_h)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue3 > decItmRef_h)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion
                    }

                    if (decimal.TryParse(strItmRef_h, out decItmRef_h) == false && decimal.TryParse(strItmRef_l, out decItmRef_l) == false)
                    //参考值为字符  2010-7-22
                    {
                        refLowAndHighIsString = true;
                    }

                    //低于极小阈值
                    if (decimal.TryParse(strItm_min, out decItmMin))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmMin) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                                message += string.Format("低于极小阈值[{0}];", decItmMin);
                            }
                        }
                        else if (decValue < decItmMin)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                            //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult["res_itm_ecd"]));
                            message += string.Format("低于极小阈值[{0}];", decItmMin);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmMin)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue1 < decItmMin)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmMin)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue2 < decItmMin)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmMin)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                                }
                            }
                            else if (decValue3 < decItmMin)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                            }
                        }
                        #endregion
                    }

                    //高于极大阈值
                    if (decimal.TryParse(strItm_max, out decItmMax))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmMax) //edit by zheng && sink 2010-8-5
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                                message += string.Format("高于极大阈值[{0}];", decItmMax);
                            }
                        }
                        else if (decValue > decItmMax)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                            //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult["res_itm_ecd"]));
                            message += string.Format("高于极大阈值[{0}];", decItmMax);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmMax)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue1 > decItmMax)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmMax)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue2 > decItmMax)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmMax)
                                {

                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                                }
                            }
                            else if (decValue3 > decItmMax)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                            }
                        }
                        #endregion
                    }

                    //低于危机值下限
                    if (decimal.TryParse(strItmPan_l, out decItmPan_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == "<" && decValue <= decItmPan_l) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                            }
                        }
                        else if (decValue < decItmPan_l)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                            message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                        }
                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmPan_l)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue1 < decItmPan_l)
                            {


                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;

                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmPan_l)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue2 < decItmPan_l)
                            {


                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmPan_l)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;


                                }
                            }
                            else if (decValue3 < decItmPan_l)
                            {


                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;


                            }
                        }
                        #endregion
                    }

                    //高于危机值上限
                    if (decimal.TryParse(strItmPan_h, out decItmpan_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmpan_h) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                            }
                        }
                        else if (decValue > decItmpan_h)
                        {
                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                            message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmpan_h)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue1 > decItmpan_h)
                            {


                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmpan_h)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue2 > decItmpan_h)
                            {


                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmpan_h)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;


                                }
                            }
                            else if (decValue3 > decItmpan_h)
                            {


                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;


                            }
                        }
                        #endregion
                    }



                    if (!hasRef)//如果没有参考值
                    {
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                    }
                    else
                    {
                        if (refLowAndHighIsString)//参考值上下限是否是字符串  2010-7-22
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                        else// if (enumResRefFlag == EnumResRefFlag.Normal)
                        {
                            drResult["res_ref_flag"] = ((int)enumResRefFlag).ToString();
                            //drResult["res_ref_flag"] = ((int)EnumResRefFlag.Normal).ToString();

                            drResult["res_ref_flag_h1"] = ((int)enumResRefFlag1).ToString();

                            drResult["res_ref_flag_h2"] = ((int)enumResRefFlag2).ToString();

                            drResult["res_ref_flag_h3"] = ((int)enumResRefFlag3).ToString();
                        }
                        //else
                        //{
                        //drResult["res_ref_flag"] = ((int)enumResRefFlag).ToString();
                        //历史结果
                        //drResult["res_ref_flag_h1"] = ((int)enumResRefFlag1).ToString();

                        //drResult["res_ref_flag_h2"] = ((int)enumResRefFlag2).ToString();

                        //drResult["res_ref_flag_h3"] = ((int)enumResRefFlag3).ToString();
                        //}
                    }

                }
                else
                {
                    if (
                        drResult.Table.Columns.Contains("res_allow_values")
                        && drResult["res_allow_values"].ToString().Trim().Length > 0
                        )
                    {
                        string[] allowValues = drResult["res_allow_values"].ToString().Trim().Split(',');

                        bool value_not_allow = true;
                        foreach (string allow_value in allowValues)
                        {
                            if (allow_value.IndexOf("$") > -1)
                            {
                                //string allow_value_check= allow_value.Split("$")[1];
                                value_not_allow = false;
                                break;
                            }
                            else
                            {

                                if (strValue == allow_value.Trim())
                                {
                                    value_not_allow = false;
                                    break;
                                }
                                else
                                {
                                    value_not_allow = value_not_allow & true;
                                }
                            }
                        }

                        if (value_not_allow)
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                        }
                        else
                        {
                            drResult["res_ref_flag"] = ((int)EnumResRefFlag.Normal).ToString();
                        }
                    }
                    else if (strValue != null
                        &&
                            (
                                strValue.Trim() == "+"
                                || strValue.StartsWith("+")
                                || strValue.EndsWith("+")
                                || strValue.IndexOf("阳性") >= 0
                                || strValue.ToLower() == "pos"
                            )
                        && !strValue.Trim().Contains("弱阳性")
                        )
                    {
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.Positive).ToString();
                    }
                    else if (strValue.Trim().Contains("弱阳性"))
                    {
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                        //grid.SetRowCellValue(rowHandle, "res_ref_flag", ((int)EnumResRefFlag.WeaklyPositive).ToString());
                    }
                    else if (strValue.Trim().Contains("#"))
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                    else
                    {
                        drResult["res_ref_flag"] = ((int)EnumResRefFlag.Unknow).ToString();
                    }


                }
            }

        }
        /// <summary>
        /// 对病人结果表中的历史结果参考值和是否超出;结果数据类型是否正确
        /// </summary>
        /// <param name="drResult"></param>
        /// <param name="bShowHint"></param>
        public void CalcPatHistoryResultRow(DataRow drResult, bool bShowHint)
        {
            if (drResult != null && !Compare.IsNullOrDBNull(drResult["res_chr"]))
            {
                #region 判断有没有大小于号结果 edit by zheng
                //结果
                string strValue = drResult["res_chr"].ToString();
                string strValueB = drResult["res_chr2"].ToString();
                //当双列右边结果没有值时 会跳过判断历史结果偏高偏低 因此需要给一个值进入判断
                if (string.IsNullOrEmpty(strValueB))
                {
                    strValueB = "0";
                }
                //结果符号，为>号或<号
                string strSymbol = "";
                if (strValue.Contains(">"))
                {
                    strSymbol = ">";
                    strValue = strValue.TrimStart('>');
                }
                else if (strValue.Contains("<"))
                {
                    strSymbol = "<";
                    strValue = strValue.TrimStart('<');
                }
                //if (drResult["res_itm_ecd"].ToString() == "TBA")
                //{
                //    string I = "1";
                //}
                #endregion
                #region 判断历史结果有没有大小于号 edit by zheng

                string strValue1 = "";
                string strValue2 = "";
                string strValue3 = "";
                //结果符号，为>号或<号
                string strSymbol1 = "";
                string strSymbol2 = "";
                string strSymbol3 = "";
                string strValue12 = "";
                string strValue22 = "";
                string strValue32 = "";
                //结果符号，为>号或<号
                string strSymbol12 = "";
                string strSymbol22 = "";
                string strSymbol32 = "";

                if (!Compare.IsNullOrDBNull(drResult["history_result1"]))
                {
                    //结果
                    strValue1 = drResult["history_result1"].ToString();

                    if (strValue1.Contains(">"))
                    {
                        strSymbol1 = ">";
                        strValue1 = strValue1.TrimStart('>');
                    }
                    else if (strValue1.Contains("<"))
                    {
                        strSymbol1 = "<";
                        strValue1 = strValue1.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result2"]))
                {
                    //结果
                    strValue2 = drResult["history_result2"].ToString();

                    if (strValue2.Contains(">"))
                    {
                        strSymbol2 = ">";
                        strValue2 = strValue2.TrimStart('>');
                    }
                    else if (strValue2.Contains("<"))
                    {
                        strSymbol2 = "<";
                        strValue2 = strValue2.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result3"]))
                {
                    //结果
                    strValue3 = drResult["history_result3"].ToString();

                    if (strValue1.Contains(">"))
                    {
                        strSymbol3 = ">";
                        strValue3 = strValue3.TrimStart('>');
                    }
                    else if (strValue3.Contains("<"))
                    {
                        strSymbol3 = "<";
                        strValue3 = strValue3.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result12"]))
                {
                    //结果
                    strValue12 = drResult["history_result12"].ToString();

                    if (strValue12.Contains(">"))
                    {
                        strSymbol12 = ">";
                        strValue12 = strValue12.TrimStart('>');
                    }
                    else if (strValue12.Contains("<"))
                    {
                        strSymbol12 = "<";
                        strValue12 = strValue12.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result22"]))
                {
                    //结果
                    strValue22 = drResult["history_result22"].ToString();

                    if (strValue22.Contains(">"))
                    {
                        strSymbol22 = ">";
                        strValue22 = strValue22.TrimStart('>');
                    }
                    else if (strValue22.Contains("<"))
                    {
                        strSymbol22 = "<";
                        strValue22 = strValue22.TrimStart('<');
                    }
                }
                if (!Compare.IsNullOrDBNull(drResult["history_result32"]))
                {
                    //结果
                    strValue32 = drResult["history_result32"].ToString();

                    if (strValue12.Contains(">"))
                    {
                        strSymbol32 = ">";
                        strValue32 = strValue32.TrimStart('>');
                    }
                    else if (strValue32.Contains("<"))
                    {
                        strSymbol32 = "<";
                        strValue32 = strValue32.TrimStart('<');
                    }
                }
                #endregion

                decimal decValue;
                decimal decValueB;
                #region 历史结果decValue
                decimal decValue1;
                decimal decValue2;
                decimal decValue3;
                decimal decValue12;
                decimal decValue22;
                decimal decValue32;
                #endregion

                if (strValue.Contains("/"))
                {
                    string[] splited = strValue.Split('/');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = (Math.Round((decLeft / decRight), 8)).ToString();
                        }
                    }
                }
                else if (strValue.Contains(":"))
                {
                    string[] splited = strValue.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValue = splited[1];
                        }
                    }
                }
                else if (strValue.Contains("e") || strValue.Contains("E"))
                {
                    double d;

                    if (double.TryParse(strValue.Replace(" ", ""), out d))//科学计数法可转换为double
                    {
                        strValue = Convert.ToDecimal(d).ToString();
                    }
                }

                if (strValueB.Contains("/"))
                {
                    string[] splited = strValueB.Split('/');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValueB = (Math.Round((decLeft / decRight), 8)).ToString();
                        }
                    }
                }
                else if (strValueB.Contains(":"))
                {
                    string[] splited = strValueB.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight))
                        {
                            strValueB = splited[1];
                        }
                    }
                }
                else if (strValueB.Contains("e") || strValueB.Contains("E"))
                {
                    double d;

                    if (double.TryParse(strValueB.Replace(" ", ""), out d))//科学计数法可转换为double
                    {
                        strValueB = Convert.ToDecimal(d).ToString();
                    }
                }
                if (
                    !strValue.Contains("+") && !strValueB.Contains("+")
                    &&
                    decimal.TryParse(strValue, out decValue)
                    && decimal.TryParse(strValueB, out decValueB)
                    //&& drResult.Table.Columns.Contains("res_allow_values")
                    && drResult["res_allow_values"].ToString().Replace(" ", "").Length == 0
                       && drResult["res_allow_values2"].ToString().Replace(" ", "").Length == 0
                    )//是否数值型结果
                {
                    string strItmRef_l = string.Empty;//参考值下限
                    string strItmRef_h = string.Empty;//参考值上限
                    string strItmRef_l2 = string.Empty;//参考值下限
                    string strItmRef_h2 = string.Empty;//参考值上限

                    //是否存在参考值
                    bool hasRef = false;

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_l_cal"]))
                    {
                        strItmRef_l = drResult["res_ref_l_cal"].ToString();
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_h_cal"]))
                    {
                        strItmRef_h = drResult["res_ref_h_cal"].ToString();
                        hasRef = true;
                    }
                    if (!Compare.IsNullOrDBNull(drResult["res_ref_l_cal2"]))
                    {
                        strItmRef_l2 = drResult["res_ref_l_cal2"].ToString();
                        hasRef = true;
                    }

                    if (!Compare.IsNullOrDBNull(drResult["res_ref_h_cal2"]))
                    {
                        strItmRef_h2 = drResult["res_ref_h_cal2"].ToString();
                        hasRef = true;
                    }
                    decimal decItmRef_l;
                    decimal decItmRef_h;
                    decimal decItmRef_l2;
                    decimal decItmRef_h2;

                    EnumResRefFlag enumResRefFlag = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlagB = EnumResRefFlag.Normal;

                    //历史结果的
                    EnumResRefFlag enumResRefFlag1 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag2 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag3 = EnumResRefFlag.Normal;

                    EnumResRefFlag enumResRefFlag12 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag22 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag32 = EnumResRefFlag.Normal;
                    string message = string.Empty;

                    #region 左边低于参考值下限
                    if (decimal.TryParse(strItmRef_l, out decItmRef_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol)) //edit by zheng
                        {
                            if (strSymbol == "<" && decValue <= decItmRef_l)
                            {
                                //drResult["res_ref_flag"] = "2";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                            }
                        }
                        else if (decValue < decItmRef_l)
                        {
                            //drResult["res_ref_flag"] = "2";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                            message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmRef_l)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue1 < decItmRef_l)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmRef_l)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue2 < decItmRef_l)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmRef_l)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue3 < decItmRef_l)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region 右边低于参考值
                    if (decimal.TryParse(strItmRef_l2, out decItmRef_l2))
                    {
                        if (!string.IsNullOrEmpty(strSymbol2)) //edit by zheng
                        {
                            if (strSymbol2 == "<" && decValueB <= decItmRef_l2)
                            {
                                //drResult["res_ref_flag"] = "2";

                                enumResRefFlagB = enumResRefFlag | EnumResRefFlag.Lower1;

                                message += string.Format("低于参考值下限[{0}];", decItmRef_l2);
                            }
                        }
                        else if (decValueB < decItmRef_l2)
                        {
                            //drResult["res_ref_flag"] = "2";

                            enumResRefFlagB = enumResRefFlag | EnumResRefFlag.Lower1;

                            message += string.Format("低于参考值下限[{0}];", decItmRef_l2);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue12, out decValue12))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol12))
                            {
                                if (strSymbol12 == "<" && decValue12 <= decItmRef_l2)
                                {
                                    enumResRefFlag12 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue12 < decItmRef_l2)
                            {
                                enumResRefFlag12 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue22, out decValue22))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol22))
                            {
                                if (strSymbol22 == "<" && decValue22 <= decItmRef_l2)
                                {
                                    enumResRefFlag22 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue22 < decItmRef_l2)
                            {
                                enumResRefFlag22 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue32, out decValue32))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol32))
                            {
                                if (strSymbol32 == "<" && decValue32 <= decItmRef_l2)
                                {
                                    enumResRefFlag32 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                }
                            }
                            else if (decValue32 < decItmRef_l2)
                            {
                                enumResRefFlag32 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region 左边 高于参考值上限
                    if (decimal.TryParse(strItmRef_h, out decItmRef_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol))
                        {
                            if (strSymbol == ">" && decValue >= decItmRef_h) // && zheng
                            {
                                //drResult["res_ref_flag"] = "1";

                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                            }
                        }
                        else if (decValue > decItmRef_h)
                        {
                            //drResult["res_ref_flag"] = "1";

                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                            message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1))
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmRef_h)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue1 > decItmRef_h)
                            {
                                enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2))
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmRef_h)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue2 > decItmRef_h)
                            {
                                enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3))
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmRef_h)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue3 > decItmRef_h)
                            {
                                enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region 右边高于参考值上限
                    if (decimal.TryParse(strItmRef_h2, out decItmRef_h2))
                    {
                        if (!string.IsNullOrEmpty(strSymbol2))
                        {
                            if (strSymbol2 == ">" && decValueB >= decItmRef_h2) // && zheng
                            {
                                //drResult["res_ref_flag"] = "1";

                                enumResRefFlagB = enumResRefFlag | EnumResRefFlag.Greater1;

                                message += string.Format("高于参考值上限[{0}];", decItmRef_h2);
                            }
                        }
                        else if (decValueB > decItmRef_h2)
                        {
                            //drResult["res_ref_flag"] = "1";

                            enumResRefFlagB = enumResRefFlag | EnumResRefFlag.Greater1;

                            message += string.Format("高于参考值上限[{0}];", decItmRef_h2);
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue12, out decValue12))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol12))
                            {
                                if (strSymbol12 == ">" && decValue12 >= decItmRef_h2)
                                {
                                    enumResRefFlag12 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue12 > decItmRef_h2)
                            {
                                enumResRefFlag12 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue22, out decValue22))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol22))
                            {
                                if (strSymbol22 == ">" && decValue22 >= decItmRef_h)
                                {
                                    enumResRefFlag22 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue22 > decItmRef_h2)
                            {
                                enumResRefFlag22 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue32, out decValue32))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol32))
                            {
                                if (strSymbol32 == ">" && decValue32 >= decItmRef_h2)
                                {
                                    enumResRefFlag32 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                }
                            }
                            else if (decValue32 > decItmRef_h2)
                            {
                                enumResRefFlag32 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    //drResult["res_ref_flag"] = ((int)enumResRefFlag).ToString();
                    //drResult["res_ref_flag2"] = ((int)enumResRefFlagB).ToString();
                    //drResult["res_ref_flag"] = ((int)EnumResRefFlag.Normal).ToString();

                    drResult["res_ref_flag_h1"] = ((int)enumResRefFlag1).ToString();

                    drResult["res_ref_flag_h2"] = ((int)enumResRefFlag2).ToString();

                    drResult["res_ref_flag_h3"] = ((int)enumResRefFlag3).ToString();

                    drResult["res_ref_flag_h12"] = ((int)enumResRefFlag12).ToString();

                    drResult["res_ref_flag_h22"] = ((int)enumResRefFlag22).ToString();

                    drResult["res_ref_flag_h32"] = ((int)enumResRefFlag32).ToString();

                }
            }
        }
        private void PatResultDoubleColumn_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                patResultProxy = new ProxyPatResult();
                mainProxy = new ProxyPidReportMain();
                List<EntityDefItmProperty> listItmProp = CacheClient.GetCache<EntityDefItmProperty>();
                List<EntityDicResultTips> listTips = CacheClient.GetCache<EntityDicResultTips>();
                this.repositoryItemLookUpEdit2.DataSource = listTips;

                if (LocalSetting.Current.Setting.LabResultShowType == "1")
                {
                    colitm_ecd.Visible = false;
                    colitm_ecd2.Visible = false;
                }
                else
                {
                    colitm_name.Visible = false;
                    colitm_name2.Visible = false;
                }
            }
        }


        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="config"></param>
        public void ApplyConfig(PatInputPatResultSettingItem conf)
        {
            Config = conf;
        }

        private void gridViewSingle_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gridViewSingle.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.Lavender;
                e.Appearance.Options.UseBackColor = true;
            }
        }

        private void gridViewSingle_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "res_itm_seq" && e.Column.FieldName != "res_itm_seq2")
                e.Appearance.BackColor = Color.Transparent;
            GridView grid = sender as GridView;

            DataRow dr = grid.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                ChangeRowStyle(dr, e, "res_ref_flag", "res_chr", "res_itm_ecd", "res_itm_name", "");

                ChangeRowStyle(dr, e, "res_ref_flag2", "res_chr2", "res_itm_ecd2", "res_itm_name2", "2");

                #region 历史结果颜色控制

                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        string column = string.Empty;
                        if (j > 0)
                            column = "2";
                        //历史结果1
                        if (dr["res_ref_flag_h" + i.ToString() + column] != null && dr["res_ref_flag_h" + i.ToString() + column] != DBNull.Value &&
                            dr["res_ref_flag_h" + i.ToString() + column].ToString().Trim(null) != string.Empty)
                        {
                            EnumResRefFlag enumResRefFlag1 = (EnumResRefFlag)Convert.ToInt32(dr["res_ref_flag_h" + i.ToString() + column]);
                            if (FlagEquals(enumResRefFlag1, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag1, EnumResRefFlag.Greater1))
                            {
                                //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                                if (e.Column.FieldName == "history_result" + i.ToString() + column)
                                {
                                    e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                                    e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                                }

                            }
                            if ((enumResRefFlag1 & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
                            {
                                if (e.Column.FieldName == "history_result" + i.ToString() + column)
                                {
                                    e.Appearance.BackColor = Config.BackColorLowerThanRef;
                                    e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                                }
                            }
                        }
                    }

                }

                ////历史结果2
                //if (dr.RefFlagHistory2 != null && dr.RefFlagHistory2 != DBNull.Value && dr.RefFlagHistory2.ToString().Trim(null) != string.Empty)
                //{
                //    EnumResRefFlag enumResRefFlag2 = (EnumResRefFlag)Convert.ToInt32(dr.RefFlagHistory2);
                //    if (FlagEquals(enumResRefFlag2, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag2, EnumResRefFlag.Greater1))
                //    {
                //        //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                //        if (e.Column.FieldName == "history_result2")
                //        {
                //            e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                //            e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                //        }

                //    }
                //    if ((enumResRefFlag2 & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
                //    {
                //        if (e.Column.FieldName == "history_result2")
                //        {
                //            e.Appearance.BackColor = Config.BackColorLowerThanRef;
                //            e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                //        }
                //    }
                //}
                #endregion


                if (e.Column.FieldName == "res_itm_seq" && dr["res_itm_seq"].ToString().Contains("*"))
                    e.Appearance.ForeColor = Color.Red;

                if (e.Column.FieldName == "res_itm_seq2" && dr["res_itm_seq2"].ToString().Contains("*"))
                    e.Appearance.ForeColor = Color.Red;


                if (e.Column.FieldName == "res_itm_ecd")
                {
                    if (dr["res_recheck_flag"].ToString() == "1")
                        e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                    if (dr["res_recheck_flag"].ToString() == "2")
                        e.Appearance.BackColor = Color.FromArgb(227, 239, 255);
                }

                if (e.Column.FieldName == "res_itm_ecd2")
                {
                    if (dr["res_recheck_flag2"].ToString() == "1")
                        e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                    if (dr["res_recheck_flag2"].ToString() == "2")
                        e.Appearance.BackColor = Color.FromArgb(227, 239, 255);
                }
                //修改标识颜色提醒
                if (dr.Table.Columns.Contains("ismodify") && dr["ismodify"] != null && dr["ismodify"].ToString() == "1" &&
                    dr["res_chr"] != null && dr["res_chr"].ToString().Trim() != string.Empty)
                {
                    if (e.Column.FieldName == "res_itm_ecd")
                    {
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                    }
                }
                if (dr.Table.Columns.Contains("ismodify2") && dr["ismodify2"] != null && dr["ismodify2"].ToString() == "1" &&
              dr["res_chr2"] != null && dr["res_chr2"].ToString().Trim() != string.Empty)
                {
                    if (e.Column.FieldName == "res_itm_ecd2")
                    {
                        e.Appearance.BackColor = Color.DeepSkyBlue;
                    }
                }
                //双列异常结果是否加粗显示
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("IsResultDoubleColBold") == "否")
                {
                }
                else
                {
                    #region 异常结果加粗显示

                    //if (dr.RefFlag.ToString().Trim() != string.Empty && e.Column.FieldName == "res_chr")
                    //{
                    //    EnumResRefFlag enumResRefFlag = (EnumResRefFlag)Convert.ToInt32(dr.RefFlag);
                    //    if (
                    //        enumResRefFlag != EnumResRefFlag.Unknow
                    //        &&
                    //        ((enumResRefFlag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1
                    //            || (enumResRefFlag & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1

                    //            || (enumResRefFlag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2
                    //            || (enumResRefFlag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2

                    //            || (enumResRefFlag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3
                    //            || (enumResRefFlag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3

                    //            || (enumResRefFlag & EnumResRefFlag.Positive) == EnumResRefFlag.Positive
                    //            || (enumResRefFlag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                    //            || (enumResRefFlag & EnumResRefFlag.ExistedNotAllowValues) == EnumResRefFlag.ExistedNotAllowValues
                    //            || (enumResRefFlag & EnumResRefFlag.WeaklyPositive) == EnumResRefFlag.WeaklyPositive
                    //        )
                    //        )
                    //    {
                    //        Font oriFont = e.Appearance.Font;
                    //        Font f = new Font(oriFont.FontFamily, oriFont.Size + 2.5f, FontStyle.Bold);
                    //        e.Appearance.Font = f;
                    //    }
                    //}
                    //else if (dr["res_ref_flag2"].ToString().Trim() != string.Empty && e.Column.FieldName == "res_chr2")
                    //{
                    //    EnumResRefFlag enumResRefFlag = (EnumResRefFlag)Convert.ToInt32(dr["res_ref_flag2"]);
                    //    if (
                    //        enumResRefFlag != EnumResRefFlag.Unknow
                    //        &&
                    //        ((enumResRefFlag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1
                    //        || (enumResRefFlag & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1

                    //        || (enumResRefFlag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2
                    //        || (enumResRefFlag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2

                    //        || (enumResRefFlag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3
                    //        || (enumResRefFlag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3

                    //        || (enumResRefFlag & EnumResRefFlag.Positive) == EnumResRefFlag.Positive
                    //        || (enumResRefFlag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue
                    //        || (enumResRefFlag & EnumResRefFlag.ExistedNotAllowValues) == EnumResRefFlag.ExistedNotAllowValues
                    //        || (enumResRefFlag & EnumResRefFlag.WeaklyPositive) == EnumResRefFlag.WeaklyPositive
                    //        )
                    //        )
                    //    {
                    //        Font oriFont = e.Appearance.Font;
                    //        Font f = new Font(oriFont.FontFamily, oriFont.Size + 2.5f, FontStyle.Bold);
                    //        e.Appearance.Font = f;
                    //    }
                    //}

                    #endregion
                }
            }
        }

        private void ChangeRowStyle(DataRow dr, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, string res_ref_flag, string res_chr, string res_itm_ecd, string res_itm_name, string strResColumnCount)
        {
            if (dr[res_ref_flag]
                    != null && dr[res_ref_flag] != DBNull.Value && dr[res_ref_flag].ToString().Trim(null) != string.Empty)
            {
                EnumResRefFlag enumResRefFlag = (EnumResRefFlag)Convert.ToInt32(dr[res_ref_flag]);
                string strRes = dr[res_chr].ToString();

                if (enumResRefFlag != EnumResRefFlag.Unknow)
                {
                    if ((enumResRefFlag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3)
                    {
                        if (e.Column.FieldName == res_itm_ecd)
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanMax;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanMax;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanMax;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                    {
                        if (e.Column.FieldName == res_itm_name)
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanPan;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                        if (e.Column.FieldName == res_itm_ecd)
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanPan;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                    }

                    if (FlagEquals(enumResRefFlag, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag, EnumResRefFlag.Greater1))
                    {
                        //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                        if (e.Column.FieldName == res_chr)//|| e.Column.FieldName == "history_result1" || e.Column.FieldName == "history_result2" || e.Column.FieldName == "history_result3")
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3)
                    {
                        if (e.Column.FieldName == res_itm_ecd)
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanMin;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanMin;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanMin;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)
                    {
                        if (e.Column.FieldName == res_itm_name)
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanPan;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                        if (e.Column.FieldName == res_itm_ecd)
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanPan;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
                    {
                        //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                        if (e.Column.FieldName == res_chr)//|| e.Column.FieldName == "history_result1" || e.Column.FieldName == "history_result2" || e.Column.FieldName == "history_result3")
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanRef;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                        }
                        else if (e.Column.FieldName == res_ref_flag)
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                        }
                    }

                    if (enumResRefFlag == EnumResRefFlag.Normal)
                    {

                    }
                }

                if (strRes != null && strRes != "")// 2010-7-1
                {
                    string words = UserInfo.GetSysConfigValue("ResultNoticeWhenWordsAre");
                    if (!string.IsNullOrEmpty(words))
                    {
                        string[] wordsArray = words.Split(',');
                        foreach (string word in wordsArray)
                        {
                            if (strRes.Replace(" ", "").Contains(word))
                            {
                                if (e.Column.FieldName == res_itm_ecd || e.Column.FieldName == res_itm_ecd || e.Column.FieldName == res_chr || e.Column.FieldName == res_ref_flag
                                    || e.Column.FieldName == "res_ref_range" + strResColumnCount || e.Column.FieldName == "history_result1" + strResColumnCount ||
                                    e.Column.FieldName == "history_result2" + strResColumnCount || e.Column.FieldName == "history_result3" + strResColumnCount)
                                {
                                    e.Appearance.ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                    if (enumResRefFlag == EnumResRefFlag.Positive
                        || enumResRefFlag == EnumResRefFlag.WeaklyPositive
                        || enumResRefFlag == EnumResRefFlag.ExistedNotAllowValues
                        )
                    {
                        if (e.Column.FieldName == res_itm_ecd || e.Column.FieldName == res_itm_ecd || e.Column.FieldName == res_chr || e.Column.FieldName == res_ref_flag
                                     || e.Column.FieldName == "res_ref_range" + strResColumnCount || e.Column.FieldName == "history_result1" + strResColumnCount ||
                                     e.Column.FieldName == "history_result2" + strResColumnCount || e.Column.FieldName == "history_result3" + strResColumnCount)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 包含Flag状态
        /// </summary>
        /// <returns></returns>
        private bool FlagEquals(EnumResRefFlag flag1, EnumResRefFlag flag2)
        {
            return (flag1 & flag2) == flag2;
        }

        private void repositoryItemTextEdit3_DoubleClick(object sender, EventArgs e)
        {
            showProp(sender, string.Empty);
        }

        private void repositoryItemTextEdit4_DoubleClick(object sender, EventArgs e)
        {
            showProp(sender, "2");
        }


        private void showProp(object sender, string strColumnCount)
        {
            //显示项目特征
            if (sender == null)
                return;

            TextEdit textEdit = (sender as TextEdit);
            if (textEdit == null || textEdit.Parent == null)
                return;

            GridControl mainControl = (textEdit.Parent as GridControl);
            if (mainControl == null)
                return;

            if (mainControl.MainView is GridView)
                this.ItemProp(mainControl.MainView as GridView, strColumnCount);
        }

        /// <summary>
        /// 项目特征
        /// </summary>
        /// <param name="sourceGrid"></param>
        public void ItemProp(GridView sourceGrid, string strColumnCount)
        {
            DataRow dr = sourceGrid.GetFocusedDataRow();

            if (dr == null) return;

            string itm_id = dr["res_itm_id" + strColumnCount].ToString();
            string itm_ecd = dr["res_itm_ecd" + strColumnCount].ToString();

            if (prop == null)
            {
                prop = new FrmItmPropLst();
                //Point p = this.PointToClient(this.PointToClient(new Point(0, 0)));

                prop.Left = this.PointToScreen(new Point(0, 0)).X + 300;
                prop.Top = this.PointToScreen(new Point(0, 0)).Y + 100;
            }

            prop.Visible = true;
            prop.strResColumnCount = strColumnCount;
            //prop.parentControl = this;
            prop.parent_grid = sourceGrid;

            List<EntityDefItmProperty> dtProp = DictItem.Instance.GetItmProp(itm_id);
            if (dtProp.Count == 0)
            {
                EntityDefItmProperty drProp = new EntityDefItmProperty();
                drProp.PtyItmEname = itm_ecd;
                drProp.PtyItmProperty = string.Empty;
                dtProp.Add(drProp);
            }

            prop.listSource = dtProp;
            prop.Text = "当前项目：" + itm_ecd;
            try
            {
                prop.Show();
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ShowProp", ex.ToString());
            }
            finally
            {

            }
        }

        bool bEnableCellValueChange = true;
        private void gridViewSingle_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (bEnableCellValueChange)
            {
                bEnableCellValueChange = false;
                GridView grid = sender as GridView;


                if (e.RowHandle >= 0 && e.Column.FieldName == "res_chr" || e.Column.FieldName == "res_chr2")
                {
                    string strColumnCount = string.Empty;
                    if (e.Column.FieldName == "res_chr2")
                        strColumnCount = "2";

                    DataRow drResulto = grid.GetFocusedDataRow();

                    if (drResulto != null)
                    {
                        if (!Compare.IsEmpty(drResulto["res_itm_id" + strColumnCount]))
                        {
                            string itemprops = DictItem.Instance.GetItmProp(drResulto["res_itm_id" + strColumnCount].ToString(), e.Value.ToString());

                            if (itemprops != string.Empty)
                            {
                                string[] props = itemprops.Split(';');
                                int rowIndex = grid.FocusedRowHandle;
                                for (int i = 0; i < props.Length; i++)
                                {
                                    if (grid.RowCount >= (rowIndex + i) + 1)
                                    {
                                        grid.SetRowCellValue(rowIndex + i, e.Column.FieldName, props[i]);
                                    }
                                }
                            }

                            //DataRow[] drPatientResultos = dtPatientResulto.Select("res_itm_id='" + drResulto["res_itm_id" + strColumnCount].ToString() + "'");
                            int resultIndex = dtPatientResulto.FindIndex(i => i.ItmId == drResulto["res_itm_id" + strColumnCount].ToString());
                            if (resultIndex > -1)
                            {
                                dtPatientResulto[resultIndex].ObrValue = drResulto["res_chr" + strColumnCount].ToString();
                                CalcObrResult(dtPatientResulto[resultIndex], true);
                                drResulto["res_ref_flag" + strColumnCount] = dtPatientResulto[resultIndex].RefFlag;
                            }

                        }
                        if (drResulto["calculate_dest_itm_id" + strColumnCount].ToString() != string.Empty)
                        {
                            ItemCalc();
                        }




                    }
                }

                if ((e.Column.FieldName == "res_chr" || e.Column.FieldName == "res_chr2")
                    && (UserInfo.GetSysConfigValue("PromptSaveAfterResValueChanged") == "是"
                    || UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是"))
                {

                    if (!string.IsNullOrEmpty(this.PatID))
                    {
                        string pat_flag = mainProxy.Service.GetPatientState(this.PatID);
                        if (string.IsNullOrEmpty(pat_flag) || pat_flag == LIS_Const.PATIENT_FLAG.Natural)
                        {
                            if (UserInfo.GetSysConfigValue("PromptSaveAfterResValueChanged") == "是")
                            {
                                if (MessageDialog.Show("数据已改变，是否保存？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    DataRow drResulto = grid.GetFocusedDataRow();

                                    if (!SaveChangedValue(drResulto, e.Column.FieldName))
                                    {
                                        MessageDialog.ShowAutoCloseDialog("修改失败！");
                                        LoadResult(PatID);
                                    }

                                    this.gridViewSingle.Focus();
                                }
                                else
                                {
                                    LoadResult(PatID);
                                }
                            }
                            else if (UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是")
                            {
                                IsPatResultDoubleColVChange = true;
                            }
                        }
                        else
                        {
                            MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，保存失败！", "提示");
                        }
                    }
                }


            }
            bEnableCellValueChange = true;
        }

        private bool SaveChangedValue(DataRow drResulto, string column)
        {
            bool success = true;

            string columnCount = "";

            string resKey = "res_key";
            if (column == "res_chr2")
            {
                resKey = "res_key2";
                columnCount = "2";
            }

            if (!Compare.IsEmpty(drResulto[resKey].ToString()) && drResulto[resKey].ToString()!="0")
            {
                List<EntityObrResult> dtResultToUpdate = new List<EntityObrResult>();

                EntityObrResult drResult = new EntityObrResult();

                int index = dtPatientResulto.FindIndex(i => i.ObrSn == Convert.ToInt32(drResulto[resKey].ToString()));

                if (index > -1)
                {
                    EntityObrResult drSourRes = dtPatientResulto[index];

                    drSourRes.ObrValue = drResulto[column].ToString();

                    dtResultToUpdate.Add(drSourRes);

                    EntityLogLogin logLogin = new EntityLogLogin();
                    logLogin.LogLoginID = UserInfo.loginID;
                    logLogin.LogIP = UserInfo.ip;

                    success = new ProxyPatResult().Service.UpdatePatientResultByResKey(logLogin, drSourRes);

                }
                else
                    success = false;
            }
            else if (!string.IsNullOrEmpty(this.PatID))
            {
                int index = dtPatientResulto.FindIndex(i => i.ItmId == drResulto["res_itm_id" + columnCount].ToString());
                if (index > -1)
                {

                    //DataTable dtResultoInsert = drRes[0].Table.Clone();
                    //dtResultoInsert.TableName = "resulto";

                    //dtResultoInsert.Rows.Add(drRes[0].ItemArray);
                    EntityObrResult dtResultoInsert = dtPatientResulto[index];
                    EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(this.PatID, false);
                    if (patient != null)
                    {
                        dtResultoInsert.ObrId = patient.RepId;
                        dtResultoInsert.ObrItrId = patient.RepItrId;
                        dtResultoInsert.ObrSid = patient.RepSid;
                        dtResultoInsert.ObrFlag = 1;
                    }

                    ProxyObrResult resultProxy = new ProxyObrResult();
                    bool result = resultProxy.Service.InsertObrResult(dtResultoInsert);

                    if (!result)
                    {
                        success = false;
                    }
                    else
                    {
                        //查出插入的检验信息，更新显示的病人信息
                        EntityResultQC resultQc = new EntityResultQC();
                        string obrId = string.Empty;
                        string itmId = string.Empty;
                        if (column == "res_chr2")
                        {
                            obrId = drResulto["res_chr2"].ToString();
                            itmId = drResulto["res_itm_id2"].ToString();
                        }
                        else
                        {
                            obrId = drResulto["res_chr"].ToString();
                            itmId = drResulto["res_itm_id"].ToString();
                        }
                        resultQc.ListObrId.Add(patient.RepId);
                        resultQc.ItmId = itmId;
                        EntityObrResult newResult = resultProxy.Service.ObrResultQuery(resultQc, false).First();
                        long result_key = newResult.ObrSn;
                        dtResultoInsert.ObrSn = result_key;

                        drResulto["res_key" + columnCount] = result_key;

                    }
                }

            }



            return success;
        }

        private string ItemProColumn = string.Empty;

        private void gridViewSingle_DoubleClick(object sender, EventArgs e)
        {
            GridView sourceGrid = sender as GridView;
            //点击结果时显示参考值
            GridHitInfo info;
            Point pt = sourceGrid.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = sourceGrid.CalcHitInfo(pt);
            if (info == null || info.Column == null)
                return;

            //******************************************************
            //若当前为readonly属性的话，则不允许弹出
            if (info.Column.ReadOnly == true)
            {
                return;
            }
            //*****************************************************

            if (info.InRow)
            {
                if (info.Column.FieldName == "res_chr")
                {
                    ItemProColumn = string.Empty;
                    this.ItemProp(sourceGrid, ItemProColumn);
                }
                if (info.Column.FieldName == "res_chr2")
                {
                    ItemProColumn = "2";
                    this.ItemProp(sourceGrid, ItemProColumn);
                }

            }
        }

        private void gridViewSingle_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView grid = sender as GridView;
            if (grid.FocusedColumn.FieldName == "res_chr" || grid.FocusedColumn.FieldName == "res_chr2")
            {
                string strColumnName = "res_type";
                if (grid.FocusedColumn.FieldName == "res_chr2")
                    strColumnName = "res_type2";

                DataRow dr = grid.GetDataRow(grid.FocusedRowHandle);
                if (dr[strColumnName] != null && dr[strColumnName] != DBNull.Value)
                {
                    if (UserInfo.GetSysConfigValue("Lab_AllowEditCalItem") != "是" && dr[strColumnName].ToString() == LIS_Const.PatResultType.Cal)
                    {
                        toolTipController1.ShowHint("当前为关联计算结果，不能编辑");
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// 项目关联计算
        /// </summary>
        public void ItemCalc()
        {
            DataTable dtPatRes = (DataTable)gridControlSingle.DataSource;
            string pat_flag = mainProxy.Service.GetPatientState(this.PatID);
            if (dtPatRes != null && dtPatRes.Rows.Count > 0 &&
               (pat_flag == LIS_Const.PATIENT_FLAG.Natural || pat_flag == string.Empty))
            {
                //this.CloseEditor();
                //this.dtPatientResulto.AcceptChanges();

                //生成关联计算参数表
                Hashtable ht = new Hashtable();
                foreach (DataRow drSource in dtPatRes.Rows)
                {
                    if (drSource["res_chr"] != null && drSource["res_chr"] != DBNull.Value && drSource["res_chr"].ToString().Trim(null) != string.Empty)
                    {
                        string item_ecd = drSource["res_itm_ecd"].ToString();

                        if (!ht.Contains(item_ecd))
                        {
                            ht.Add(item_ecd, drSource["res_chr"]);
                        }
                    }
                    if (drSource["res_chr2"] != null && drSource["res_chr2"] != DBNull.Value && drSource["res_chr2"].ToString().Trim(null) != string.Empty)
                    {
                        string item_ecd2 = drSource["res_itm_ecd2"].ToString();

                        if (!ht.Contains(item_ecd2))
                        {
                            ht.Add(item_ecd2, drSource["res_chr2"]);
                        }
                    }
                }

                try
                {

                    //关联计算
                    DataSet dsResult = Variable(ht);

                    DataTable dtCalResult = dsResult.Tables[0];
                    if (dtCalResult.Rows.Count > 0)
                    {
                        //DataTable dtComItem = new FuncLibClient().getDictFromCache(new String[] { "itm_com_mi" }).Tables[0];

                        foreach (DataRow drResult in dtCalResult.Rows)
                        {
                            string itm_id = drResult["cal_item_ecd"].ToString();
                            string itm_ecd = drResult["itm_ecd"].ToString();
                            string value = drResult["retu"].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    decimal dec = 0;

                                    if (decimal.TryParse(value, out dec))
                                    {
                                        dec = decimal.Round(dec, 2);

                                        value = dec.ToString();
                                    }

                                    string itmprop = DictItem.Instance.GetItmProp(itm_id, value);
                                    if (!string.IsNullOrEmpty(itmprop))
                                        value = itmprop;
                                }


                                foreach (DataRow drPatRes in dtPatRes.Rows)
                                {
                                    if (drPatRes["res_itm_id"].ToString() == itm_id || drPatRes["res_itm_ecd"].ToString() == itm_ecd)
                                    {
                                        drPatRes["res_chr"] = value;
                                        drPatRes["res_type"] = LIS_Const.PatResultType.Cal;
                                    }
                                    if (drPatRes["res_itm_id2"].ToString() == itm_id || drPatRes["res_itm_ecd2"].ToString() == itm_ecd)
                                    {
                                        drPatRes["res_chr2"] = value;
                                        drPatRes["res_type2"] = LIS_Const.PatResultType.Cal;
                                    }
                                }

                            }
                        }
                        dtPatRes.AcceptChanges();
                        DataTable dtNew = dtPatRes.Copy();
                        gridControlSingle.DataSource = dtNew;
                        gridViewSingle.UpdateCurrentRow();
                        gridViewSingle.CloseEditor();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "ItemCalc", ex.ToString());
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再关联计算", "提示");
            }
        }





        /// <summary>
        /// 关闭编辑单元格并更新数据
        /// </summary>
        private void CloseEditor()
        {
            this.gridViewSingle.CloseEditor();

            if (this.dtPatientResulto != null)
            {
                //this.dtPatientResulto.AcceptChanges();
            }
        }

        private DataSet Variable(Hashtable ht)
        {
            List<EntityDicItmCalu> dci = CacheClient.GetCache<EntityDicItmCalu>();
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("cal_sp_formula");//存ECD
            pb.Columns.Add("retu");
            List<string> fmla = new List<string>();
            foreach (EntityDicItmCalu dr in dci)
            {
                if (!string.IsNullOrEmpty(dr.CalItrId) && !string.IsNullOrEmpty(Pat_itr_id)
                    && dr.CalItrId != Pat_itr_id)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression) &&
                   fmla.Contains(dr.CalExpression))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression))
                {
                    fmla.Add(dr.CalExpression);
                }
                if (!string.IsNullOrEmpty(dr.CalVariable))
                {
                    string[] varpr = dr.CalVariable.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr.CalExpression, dr.CalFlag, dr.ItmId, dr.ItmEcode, dr.CalSpFormula);

                    }
                }
            }
            CalInfoEventArgs eArgs = null;
            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();
                string itmID = pb.Rows[i]["cal_item_ecd"].ToString();

                if (pb.Rows[i]["cal_sp_formula"] != null &&
                    !string.IsNullOrEmpty(pb.Rows[i]["cal_sp_formula"].ToString()))
                {
                    if (eArgs == null)
                    {
                        eArgs = new CalInfoEventArgs();
                        //if (ClaItemInfo != null)
                        //{
                        eArgs.SampName = dtPatient.SamName;
                        eArgs.SampRem = dtPatient.SampRemark;
                        eArgs.SampID = dtPatient.PidSamId;
                        eArgs.itm_itr_id = dtPatient.RepItrId;
                        eArgs.Sex = dtPatient.PidSex;
                        string[] age = dtPatient.PidAgeExp.Split('Y');
                        eArgs.Age = Convert.ToDouble(age[0]);
                        //ClaItemInfo(this, eArgs);
                        //}
                    }
                    pb.Rows[i]["retu"] = new CalcItemResHelper().GetCalcRes(pb.Rows[i]["cal_sp_formula"].ToString(), ht, eArgs);
                    continue;
                }
                // string methAll = pb.Rows[i]["cal_fmla"].ToString();
                //检验系统中的计算项目：如果参与计算项目结果有一项不为数值的，不用计算
                //tom 2012年9月11日11:13:27
                bool can_cal = true;

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";
                    if (methAll.Contains(fam))
                    {
                        double dValue = 0;
                        can_cal = can_cal && double.TryParse(value[j], out dValue);
                        if (can_cal)
                        {
                            string va = dValue.ToString("0.0000");

                            methAll = methAll.Replace(fam, va);
                        }
                    }
                }
                if (can_cal)
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        object objValue = dt.Compute(methAll, string.Empty);

                        decimal decVal = 0;

                        if (decimal.TryParse(objValue.ToString(), out decVal))
                        {
                            CalInfoEventArgs args = new CalInfoEventArgs();
                            if (ClaItemInfo != null)
                            {
                                ClaItemInfo(this, args);
                            }
                            int? itm_max_digit = null;
                            EntityDicItemSample itemSam = dcl.client.cache.CacheItemSam.Current.SelectAll().Find(k => k.ItmId == itmID && k.ItmSamId == dtPatient.PidSamId);
                            if (itemSam != null)
                            {
                                itm_max_digit = itemSam.ItmMaxDigit;
                            }
                            if (itm_max_digit == null || itm_max_digit < 0)
                            {
                                decVal = decimal.Round(decVal, 4);
                                pb.Rows[i]["retu"] = decVal.ToString("0.00");
                            }
                            else
                            {
                                decVal = decimal.Round(decVal, itm_max_digit.Value);
                                if (itm_max_digit == 0)
                                {
                                    pb.Rows[i]["retu"] = decVal.ToString();
                                }
                                else
                                {

                                    pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                }

                            }
                        }

                        //pb.Rows[i]["retu"] = .ToString();
                    }
                    catch
                    {
                        //2013年2月28日16:45:32 叶
                        //当使用DataTable.Compute无法计算表达式的值时,比如带Math.Log()的表达式
                        //用动态编译后进行计算
                        try
                        {
                            //2013年5月14日14:20:41 叶
                            CalInfoEventArgs args = new CalInfoEventArgs();
                            if (ClaItemInfo != null)
                            {
                                ClaItemInfo(this, args);
                            }
                            if (methAll.Contains("[标本]") || methAll.Contains("[标本备注]"))
                            {

                                methAll = methAll.Replace("[标本]", string.Format("\"{0}\"", args.SampName));
                                methAll = methAll.Replace("[标本备注]", string.Format("\"{0}\"", args.SampRem));

                            }


                            object objValue = ExpressionCompute.CalExpression(methAll);
                            if (objValue != null)
                            {
                                decimal decVal = 0;

                                if (decimal.TryParse(objValue.ToString(), out decVal))
                                {
                                    int? itm_max_digit = null;
                                    EntityDicItemSample itemSam = dcl.client.cache.CacheItemSam.Current.SelectAll().Find(k => k.ItmId == itmID && k.ItmSamId == args.SampID);
                                    if (itemSam != null)
                                    {
                                        itm_max_digit = itemSam.ItmMaxDigit;
                                    }
                                    if (itm_max_digit == null || itm_max_digit < 0)
                                    {
                                        decVal = decimal.Round(decVal, 4);
                                        pb.Rows[i]["retu"] = decVal.ToString("0.00");
                                    }
                                    else
                                    {
                                        decVal = decimal.Round(decVal, itm_max_digit.Value);
                                        if (itm_max_digit == 0)
                                        {
                                            pb.Rows[i]["retu"] = decVal.ToString();
                                        }
                                        else
                                        {

                                            pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                        }

                                    }
                                }
                            }
                            else
                            {

                                pb.Rows[i]["retu"] = string.Empty;
                            }
                        }
                        catch (Exception ex)
                        {

                            pb.Rows[i]["retu"] = string.Empty;
                        }
                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        private int GetConfigOnNullAge(int age)
        {
            if (age < 0)
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");

                int calage = -1;

                if (!string.IsNullOrEmpty(configCalAge)
                    && configCalAge != "不计算参考值")
                {
                    calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                    if (age >= 0)
                    {
                        calage = age;
                    }
                }
                return calage;
            }
            else
            {
                return age;
            }
        }

        private string GetConfigOnNullSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)

                || (sex != "1"
                && sex != "2"
                && sex != "0"))
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                if (configCalSex.Contains("男"))
                {
                    return "1";
                }
                else if (configCalSex.Contains("女"))
                {
                    return "2";
                }

                return "0";
            }
            else
            {
                return sex;
            }
        }

        private void contextMenuLeft_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.CloseEditor();

            ContextMenuStrip menu = (sender as ContextMenuStrip);

            GridControl grid = menu.SourceControl as GridControl;
            GridView sourceGrid = null;

            if (grid == this.gridControlSingle)
            {
                sourceGrid = this.gridViewSingle;
            }

            menu.Visible = false;

            if (sourceGrid != null)
            {
                switch (e.ClickedItem.Name)
                {
                    case "menuCalc":
                        ItemCalc();
                        break;
                    case "menuAddItem":
                        MenuAddItem(sourceGrid);
                        break;
                    case "menuDelItem":
                        MenuDelItem(sourceGrid, true);
                        break;
                    case "menuResultCheck":
                        MenuResultCheck(sourceGrid);
                        break;
                    case "menuResultCheckConfirm":
                        MenuResultCheckConfirm(sourceGrid);
                        break;
                    case "menuSingleView":
                        MenuSingleView();
                        break;
                    case "menuLookOldResult":
                        LookOldResultView();
                        break;
                    case "menuMedicalRecord"://查看电子病历
                        BrowseMedicalRecord(dtPatient != null ? dtPatient.PidInNo : "");
                        break;

                }
            }
        }
        public void BrowseMedicalRecord(string pidInNo)
        {
            string url = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_MedicalRecordUrl");
            url = url.Replace("@regno", pidInNo);
            System.Diagnostics.Process.Start(url);
        }
        /// <summary>
        /// 查看复查原结果
        /// </summary>
        FrmOldResultView frmORV = null;

        /// <summary>
        /// 查看复查原结果
        /// </summary>
        public void LookOldResultView()
        {
            DataRow drFocused = this.gridViewSingle.GetFocusedDataRow();

            if (drFocused == null)
            {
                return;
            }

            //选中列是否包含第一列项目
            bool isResultSingle = false;
            //选中列是否包含第二列项目
            bool isResultDouble = false;

            GridColumn[] SelectedColumn = gridViewSingle.GetSelectedCells(gridViewSingle.GetFocusedDataSourceRowIndex());

            foreach (GridColumn item in SelectedColumn)
            {
                if (!isResultSingle && (!item.FieldName.Contains("2") || item.FieldName == "history_result2"))
                    isResultSingle = true;

                if (!isResultDouble && item.FieldName.Contains("2") && item.FieldName != "history_result2")
                    isResultDouble = true;
            }

            EntityObrResult dr = new EntityObrResult();

            string itmId = string.Empty;

            if (isResultSingle)
                itmId = drFocused["res_itm_id"].ToString();
            else
                itmId = drFocused["res_itm_id2"].ToString();


            int index = dtPatientResulto.FindIndex(i => i.ItmId == itmId);
            if (index > -1)
            {
                dr = dtPatientResulto[index];

                DateTime p_dtime = DateTime.Now;
                string sqlSelItr = "";
                string p_sid = "";

                if (!DateTime.TryParse(dr.ObrDate.ToString(), out p_dtime))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:010)");
                    return;//如果日期为空,则跳出
                }


                if (string.IsNullOrEmpty(dr.ObrItrId))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:020)");
                    return;
                }
                else
                {
                    sqlSelItr = dr.ObrItrId;
                }

                if (string.IsNullOrEmpty(dr.ObrSid))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:030)");
                    return;
                }
                else
                {
                    p_sid = dr.ObrSid;
                }

                string itm_id = dr.ItmId.ToString();
                PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();
                List<EntityDicObrResultOriginal> dtResult = proxy.Service.GetInstructmentResult2(p_dtime, sqlSelItr, 0, p_sid);
                dtResult = dtResult.FindAll(i => i.ItmID == itm_id);
                try
                {
                    if (dtResult != null && dtResult.Count > 0)
                    {
                        #region 排序

                        for (int i = 0; i < dtPatientResulto.Count; i++)
                        {
                            List<EntityDicObrResultOriginal> listOriginal = dtResult.FindAll(w => w.ItmID == dtPatientResulto[i].ItmId);
                            if (listOriginal != null && listOriginal.Count > 0)
                            {
                                foreach (EntityDicObrResultOriginal p_dr in listOriginal)
                                {
                                    p_dr.RowSer = i;
                                }
                            }
                        }

                        foreach (EntityDicObrResultOriginal p_dr in dtResult)
                        {
                            if (p_dr.RowSer == null)
                            {
                                p_dr.RowSer = 999;
                            }
                        }

                        dtResult = dtResult.OrderBy(i => i.RowSer).ToList();

                        #endregion

                        if (frmORV == null)
                        {
                            frmORV = new FrmOldResultView();
                        }
                        frmORV.dtSource = dtResult;
                        frmORV.Show();
                    }
                    else
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果");
                    }
                }
                catch (Exception ex)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("遇到错误");
                    Logger.WriteException(this.GetType().Name, "查看复查原结果出错", ex.ToString());
                }
            }
        }

        /// <summary>
        /// 仪器专业组
        /// </summary>
        public string Itr_ptype { get; set; }

        /// <summary>
        /// 标本备注
        /// </summary>
        public string Samrem { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string Pat_dep_id { get; set; }

        /// <summary>
        /// 病人组合明细
        /// </summary>
        public List<EntityPidReportDetail> PatientsMi { get; set; }
        ProxyPatResult patResultProxy = null;//new ProxyPatResult();
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuAddItem(GridView sourceGrid)
        {
            //获取病人记录状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                FrmItemSelect frm = new FrmItemSelect();
                frm.itm_ptype = this.Itr_ptype;
                frm.itr_id = dtPatient.RepItrId;
                frm.ResetFilter();
                frm.Location = this.Location;
                frm.Left = Control.MousePosition.X;
                frm.Top = Control.MousePosition.Y;
                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.OK)
                {
                    string itm_id = frm.ReturnItemID;
                    string itm_ecd = frm.ReturnItemECD;

                    //添加项目
                    //AddItem(itm_id);

                    List<EntityItmRefInfo> dtItems = patResultProxy.Service.GetItemRefInfo(new List<string> { itm_id }, this.samtypeid, GetConfigOnNullAge(this.patage), GetConfigOnNullSex(this.patsex), Samrem, this.Pat_itr_id, Pat_dep_id, "");
                    if (dtItems.Count > 0)
                    {
                        EntityItmRefInfo drItem = dtItems[0];

                        string defValue = string.Empty;
                        if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                        {
                            defValue = drItem.ItmDefault;
                        }

                        //在病人组合中是否有包含次项目
                        bool bItemInCombine = false;
                        EntityObrResult drAddedItem = null;

                        foreach (EntityPidReportDetail drCom in this.PatientsMi)
                        {
                            string com_id = drCom.ComId;
                            string com_name = drCom.PatComName;
                            int com_seq = Convert.ToInt32(drCom.SortNo);

                            List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);

                            foreach (EntityDicCombineDetail drComItem in dtComItems)
                            {
                                if (drComItem.ComItmId == itm_id)
                                {
                                    bItemInCombine = true;
                                    drAddedItem = AddItem(drItem, drComItem, com_id, com_name, com_seq, defValue, null);
                                    break;
                                }

                                if (bItemInCombine == true)
                                {
                                    break;
                                }
                            }
                        }

                        if (!bItemInCombine)
                            drAddedItem = AddItem(drItem, null, string.Empty, string.Empty, 999, defValue, null);

                        LoadResult(dtPatient, dtPatientResulto, true);
                    }
                }
            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再添加项目", "提示");
            }
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="drItem"></param>
        public EntityObrResult AddItem(EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, int com_seq, string res_chr, string res_od_chr)
        {
            if (drItem != null)
            {
                string itm_id = drItem.ItmId;

                //项目编号
                string itm_ecd = string.Empty;
                if (!string.IsNullOrEmpty(drItem.ItmEcode))
                {
                    itm_ecd = drItem.ItmEcode;
                }

                string strEcd = SQLFormater.Format(itm_ecd.Trim());

                //查找当前病人结果表中的项目是否已存在
                int resultIndex = dtPatientResulto.FindIndex(i => i.ItmId == itm_id || i.ItmEname.TrimEnd() == strEcd);

                EntityObrResult drResultItem = null;
                if (resultIndex == -1)
                {
                    drResultItem = new EntityObrResult();
                    FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                    drResultItem.IsNew = 1;

                    drResultItem.ResComSeq = com_seq;
                    this.dtPatientResulto.Add(drResultItem);
                }
                else
                {
                    EntityObrResult drResultExistItem = dtPatientResulto[resultIndex];
                    int row_state = drResultExistItem.IsNew;

                    if (row_state == 2)//需要添加的项目为已被删除的项目
                    {
                        //drResultExistItem.Delete();
                        this.dtPatientResulto.Remove(drResultExistItem);//先把被删除(隐藏)的项目移除，再添加

                        drResultItem = new EntityObrResult();
                        FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                        drResultItem.ResComSeq = com_seq;
                        drResultItem.IsNew = 0;
                        this.dtPatientResulto.Add(drResultItem);
                    }
                    else
                    {
                        drResultItem = dtPatientResulto[resultIndex];
                        if (drResultItem.ObrSn == 0)
                        {
                            drResultItem.IsNew = 1;
                        }
                        else
                        {
                            drResultItem.IsNew = 0;
                        }
                    }
                }


                //this.dtPatientResulto.AcceptChanges();
                //BindGrid();
                //SetAutoCalParam();
                return drResultItem;
            }
            return null;
        }

        private void FillItemToResult(EntityObrResult drResultItem, EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, string itm_id, string itm_ecd, string res_chr, string res_od_chr)
        {
            drResultItem.ObrFlag = 1;

            //项目名称
            string itm_name = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmName))
            {
                itm_name = drItem.ItmName;
            }

            //单位
            string itm_unit = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmUnit))
            {
                itm_unit = drItem.ItmUnit;
            }

            string itm_rep_ecd = string.Empty;
            if (!Compare.IsNullOrDBNull(drItem.ItmRepCode) && drItem.ItmRepCode.Trim() != string.Empty)
            {
                itm_rep_ecd = drItem.ItmRepCode;
            }
            else
            {
                itm_rep_ecd = itm_ecd;
            }

            drResultItem.ObrDate = ServerDateTime.GetServerDateTime();
            drResultItem.ObrId = this.PatID;
            drResultItem.ItmId = itm_id;
            drResultItem.ObrItmMethod = drItem.ItmMethod;
            drResultItem.ItmEname = itm_ecd.TrimEnd();
            drResultItem.ItmReportCode = itm_rep_ecd;
            drResultItem.ItmName = itm_name;
            drResultItem.ObrUnit = itm_unit;
            drResultItem.ResComName = com_name;
            drResultItem.ItmDtype = drItem.ItmResType;
            drResultItem.ItmMaxDigit = drItem.ItmMaxDigit;

            if (drItem.ItmCaluFlag == 1)
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Cal);
            }
            else if (!string.IsNullOrEmpty(res_chr))
            {
                drResultItem.ObrType = 3;
            }
            else
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Normal);
            }

            drResultItem.ItmSeq = drItem.ItmSortNo;
            drResultItem.ItmPrice = drItem.ItmPrice;
            drResultItem.ItmComId = com_id;

            if (!string.IsNullOrEmpty(drItem.ItmPositiveRes))
                drResultItem.ResPositiveResult = drItem.ItmPositiveRes;

            if (!string.IsNullOrEmpty(drItem.ItmUrgentRes))
                drResultItem.ResCustomCriticalResult = drItem.ItmUrgentRes;

            if (!string.IsNullOrEmpty(drItem.ItmResultAllow))
                drResultItem.ResAllowValues = drItem.ItmResultAllow;

            drResultItem.ResMax = drItem.ItmMaxValue;
            drResultItem.ResMin = drItem.ItmMinValue;

            drResultItem.ResPanH = drItem.ItmDangerUpperLimit;
            drResultItem.ResPanL = drItem.ItmDangerLowerLimit;

            drResultItem.RefUpperLimit = drItem.ItmUpperLimitValue;
            drResultItem.RefLowerLimit = drItem.ItmLowerLimitValue;



            drResultItem.ResMaxCal = drItem.ItmMaxValueCal;
            drResultItem.ResMinCal = drItem.ItmMinValueCal;

            drResultItem.ResPanHCal = drItem.ItmDangerUpperLimitCal;
            drResultItem.ResPanLCal = drItem.ItmDangerLowerLimitCal;

            drResultItem.ResRefHCal = drItem.ItmUpperLimitValueCal;
            if (drResultItem.ResRefHCal == null) drResultItem.ResRefHCal = string.Empty;

            drResultItem.ResRefLCal = drItem.ItmLowerLimitValueCal;
            if (drResultItem.ResRefLCal == null) drResultItem.ResRefLCal = string.Empty;

            if (drResultItem.ResRefLCal.Trim() != string.Empty
                && drResultItem.ResRefHCal.Trim() != string.Empty)
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + " - " + drResultItem.ResRefHCal.ToString().Trim();
            }
            else
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + drResultItem.ResRefHCal.ToString().Trim();
            }

            if (drComMi != null)
            {
                drResultItem.IsNotEmpty = drComMi.ComMustItem;
                drResultItem.ComMiSort = drComMi.ComSortNo;
            }

        }

        /// <summary>
        /// 菜单删除项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        /// <param name="bNeedConfirm">是否需要提示确认</param>
        private void MenuDelItem(GridView sourceGrid, bool bNeedConfirm)
        {
            //获取当前记录的审核状态
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }

                List<EntityObrResult> selectDataRows = new List<EntityObrResult>();

                string tipItemsText = string.Empty;
                foreach (int rowHandler in selectedRowHandler)
                {
                    DataRow row = sourceGrid.GetDataRow(rowHandler);

                    if (row != null)
                    {
                        //选中列是否包含第一列项目
                        bool isResultSingle = false;
                        //选中列是否包含第二列项目
                        bool isResultDouble = false;

                        GridColumn[] SelectedColumn = sourceGrid.GetSelectedCells(rowHandler);

                        foreach (GridColumn item in SelectedColumn)
                        {
                            if (!isResultSingle && (!item.FieldName.Contains("2") || item.FieldName == "history_result2"))
                                isResultSingle = true;

                            if (!isResultDouble && item.FieldName.Contains("2") && item.FieldName != "history_result2")
                                isResultDouble = true;
                        }


                        if (isResultSingle)
                        {
                            int resultIndex = dtPatientResulto.FindIndex(i => i.ItmId == row["res_itm_id"].ToString());
                            if (resultIndex > -1)
                            {
                                selectDataRows.Add(dtPatientResulto[resultIndex]);
                                tipItemsText += string.Format(", [{0}]", row["res_itm_ecd"].ToString());
                            }
                        }

                        if (isResultDouble)
                        {
                            int resultIndex = dtPatientResulto.FindIndex(i => i.ItmId == row["res_itm_id2"].ToString());
                            if (resultIndex > -1)
                            {
                                selectDataRows.Add(dtPatientResulto[resultIndex]);
                                tipItemsText += string.Format(", [{0}]", row["res_itm_ecd2"].ToString());
                            }
                        }

                        if (tipItemsText.Length > 0)
                            tipItemsText = tipItemsText.Remove(0, 1);
                    }
                }

                string messagetips = "是否要移除项目{0}？\r\n{1}";

                if (dcl.client.frame.UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除") //是否立刻从数据库删除
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后立刻从数据库删除");
                }
                else
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后需要点击[保存]才从数据库中删除");
                }

                if (!bNeedConfirm
                    || lis.client.control.MessageDialog.Show(messagetips, "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RemoveItem(selectDataRows, true);
                    //this.dtPatientResulto.AcceptChanges();
                    this.LoadResult(dtPatient, dtPatientResulto, true);
                }

            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能删除", "提示");
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        public void RemoveItem(List<EntityObrResult> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (dcl.client.frame.UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }

            bool statusCheck = false;
            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityObrResult drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = false;
                if (!string.IsNullOrEmpty(drPatResultItem.ObrValue)
                    && drPatResultItem.ObrValue.Trim(null) != string.Empty)
                {
                    hasResult = true;
                }

                //数据库是否存在结果
                bool recordInDataBase = false;
                if (drPatResultItem.ObrSn != 0)
                {
                    recordInDataBase = true;
                }
                if (!Compare.IsEmpty(drPatResultItem.ObrId) && !statusCheck)
                {
                    string currPatFlag = mainProxy.Service.GetPatientState(drPatResultItem.ObrId);

                    if (currPatFlag != LIS_Const.PATIENT_FLAG.Natural && currPatFlag != string.Empty)
                    {
                        MessageDialog.Show(string.Format("当前记录已{1}或已{0}，不能删除组合", "审核", LocalSetting.Current.Setting.ReportWord), "提示");
                        break;
                    }
                    statusCheck = true;
                }

                if (hasResult)
                {
                    if (removeHasResult && recordInDataBase)
                    {
                        if (!Compare.IsEmpty(drPatResultItem.ObrId) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {
                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string resid = drPatResultItem.ObrId;
                                string res_itm_ecd = drPatResultItem.ItmEname.TrimEnd();
                                string res_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.ItmId))
                                {
                                    res_itm_id = drPatResultItem.ItmId;
                                }

                                long reskey = -1;

                                bool opResult = false;
                                if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                                {
                                    reskey = drPatResultItem.ObrSn;
                                }

                                if (res_itm_id == string.Empty)
                                {

                                }
                                else if (reskey != -1)
                                {
                                    opResult = patResultProxy.Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    this.dtPatientResulto.Remove(drPatResultItem);
                                }

                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtPatientResulto.RemoveAt(deleteIndex);
                                }
                            }
                            else
                            {
                                drPatResultItem.IsNew = 2;
                            }
                        }
                        else
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!Compare.IsEmpty(drPatResultItem.ObrId) && drPatResultItem.ObrSn != 0)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.ObrId;
                            string res_itm_ecd = drPatResultItem.ItmEname.TrimEnd();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.ItmId))
                            {
                                res_itm_id = drPatResultItem.ItmId;
                            }

                            long reskey = -1;

                            bool opResult = false;
                            if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                            {
                                reskey = Convert.ToInt64(drPatResultItem.ObrSn);
                            }

                            if (res_itm_id == string.Empty)
                            {

                            }
                            else if (reskey != -1)
                            {
                                opResult = patResultProxy.Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);
                                i--;
                            }

                            if (!opResult)
                            {
                                MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);
                                i--;
                                int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtPatientResulto.RemoveAt(deleteIndex);
                            }
                        }
                        else
                        {
                            drPatResultItem.IsNew = 2;
                        }
                    }
                    else
                    {
                        rowsPatResultItem.Remove(drPatResultItem);
                        this.dtPatientResulto.Remove(drPatResultItem);
                        i--;
                    }
                }
            }
            this.gridControlSingle.RefreshDataSource();
        }


        /// <summary>
        /// 报告复查
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuResultCheck(GridView sourceGrid)
        {
            //获取当前记录的审核状态
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);


            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }
                List<EntityObrResult> dtResulto = new List<EntityObrResult>();
                List<EntityObrResult> dtResulto2 = new List<EntityObrResult>();
                foreach (int rowHandler in selectedRowHandler)
                {
                    DataRow row = sourceGrid.GetDataRow(rowHandler);

                    if (row != null)
                    {
                        //选中列是否包含第一列项目
                        bool isResultSingle = false;
                        //选中列是否包含第二列项目
                        bool isResultDouble = false;

                        GridColumn[] SelectedColumn = sourceGrid.GetSelectedCells(rowHandler);
              
                        foreach (GridColumn item in SelectedColumn)
                        {
                            if (!isResultSingle && (!item.FieldName.Contains("2") || item.FieldName == "history_result2"))
                                isResultSingle = true;

                            if (!isResultDouble && item.FieldName.Contains("2") && item.FieldName != "history_result2")
                                isResultDouble = true;
                        }

                        if (isResultSingle)
                        {
                            int index = dtPatientResulto.FindIndex(i => i.ItmId == row["res_itm_id"].ToString());
                            if (index > -1)
                            {
                                if (!string.IsNullOrEmpty(dtPatientResulto[index].ObrValue))
                                {
                                    dtResulto.Add(dtPatientResulto[index]);
                                }
                                else {
                                    MessageBox.Show("该项目无结果,不进行复查");
                                    return;
                                }
                            }
                                
                        }

                        if (isResultDouble)
                        {
                            int index = dtPatientResulto.FindIndex(i => i.ItmId == row["res_itm_id2"].ToString());
                            if (index > -1)
                            {
                                if (!string.IsNullOrEmpty(dtPatientResulto[index].ObrValue))
                                {
                                    dtResulto2.Add(dtPatientResulto[index]);
                                }
                                else
                                {
                                    MessageBox.Show("该项目无结果,不进行复查");
                                    return;
                                }
                            }
                        }

                    }
                }
                if (dtResulto.Count > 0)
                {
                    ProxyPatientRecheck recheckProxy = new ProxyPatientRecheck();
                    if (recheckProxy.Service.RecheckResultItem(PatID, dtResulto))
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("已标记复查！");

                        foreach (int rowHandler in selectedRowHandler)
                        {
                            DataRow rowResult = this.gridViewSingle.GetDataRow(rowHandler);
                            rowResult["res_recheck_flag"] = "1";
                        }
                        if (dtPatient != null)
                        {
                            dtPatient.RepRecheckFlag = 1;
                        }
                    }
                    // LoadResult(dtPatient, dtPatientResulto, true);
                    gridViewSingle.RefreshData();
                    menuResultCheckConfirm.Enabled = true;
                }
                if (dtResulto2.Count > 0)
                {
                    ProxyPatientRecheck recheckProxy = new ProxyPatientRecheck();
                    if (recheckProxy.Service.RecheckResultItem(PatID, dtResulto2))
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("已标记复查！");

                        foreach (int rowHandler in selectedRowHandler)
                        {
                            DataRow rowResult = this.gridViewSingle.GetDataRow(rowHandler);
                            rowResult["res_recheck_flag2"] = "1";
                        }
                        if (dtPatient != null)
                        {
                            dtPatient.RepRecheckFlag = 1;
                        }
                    }
                    //LoadResult(dtPatient, dtPatientResulto, true);
                    gridViewSingle.RefreshData();
                    menuResultCheckConfirm.Enabled = true;
                }
            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不允许复查", "提示");
            }
        }

        ProxyPidReportMain mainProxy = null;// new ProxyPidReportMain();
        /// <summary>
        /// 复查确认
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuResultCheckConfirm(GridView sourceGrid)
        {
            //获取当前记录的审核状态
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }

                string patExp = string.Empty;
                bool ok = false;
                if (showDoubleItrWarningMsg || !ConfigHelper.IsNotOutlink())
                {
                    FrmRecheckConfirm frm = new FrmRecheckConfirm();
                    frm.Init(PatID, Pat_itr_id);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (patExp == null || patExp.Trim() == string.Empty)
                            patExp = frm.RemarkMsg;
                        else
                            patExp += "," + frm.RemarkMsg;
                        ok = true;
                    }
                }
                //DataTable dtItr = new FuncLibClient().getDictFromCache(new String[] { "dict_instrmt" }).Tables[0];

                //DataRow[] dritr = dtItr.Select("itr_id='" + Pat_itr_id + "'");
                else
                {
                    ok = true;
                    List<EntityDicInstrument> listInst = CacheClient.GetCache<EntityDicInstrument>();
                    int index = listInst.FindIndex(i => i.ItrId == Pat_itr_id);

                    string strUrt = string.Empty;

                    if (index > -1 && listInst[index].ItrMicroFlag == 1)
                        strUrt = "镜检";

                    if (patExp == null || patExp.Trim() == string.Empty ||
                        patExp.Trim() == ("标本已" + strUrt + "复查"))
                        patExp = "标本已" + strUrt + "复查";
                    else
                        patExp += ",标本已" + strUrt + "复查";


                }
                if (ok)
                {
                    // EntityPatients updatePat = new EntityPatients();
                    //dtPatient.RepId = PatID;
                    dtPatient.RepRemark = patExp;
                    dtPatient.RepRecheckFlag = 2; ;

                    //更新复查标志为2
                    if (new ProxyPidReportMain().Service.UpdatePatientData(new List<EntityPidReportMain>() { dtPatient }))
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("复查完毕！");
                        if (dtPatient != null)
                        {
                            dtPatient.RepRecheckFlag = 2;
                        }
                        menuResultCheckConfirm.Enabled = false;
                    }
                }

            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不允许复查确认", "提示");
            }
        }

        /// <summary>
        /// 获取当前结果
        /// </summary>
        /// <returns></returns>
        public List<EntityObrResult> GetResultTable()
        {
            try
            {
                CloseEditor();
                if (gridControlSingle.DataSource == null) return new List<EntityObrResult>();
                DataTable dtPatRes = (DataTable)gridControlSingle.DataSource;

                if (dtPatRes == null) return new List<EntityObrResult>();
                foreach (DataRow drPatRes in dtPatRes.Rows)
                {
                    int index = dtPatientResulto.FindIndex(i => i.ItmId == drPatRes["res_itm_id"].ToString());
                    if (index > -1)
                    {
                        dtPatientResulto[index].ObrValue = drPatRes["res_chr"].ToString();
                        dtPatientResulto[index].ObrType = Convert.ToInt32(drPatRes["res_type"]);
                    }
                    if (drPatRes["res_itm_id2"].ToString().Trim() != string.Empty)
                    {
                        int resultIndex = dtPatientResulto.FindIndex(i => i.ItmId == drPatRes["res_itm_id2"].ToString());
                        if (resultIndex > -1)
                        {
                            dtPatientResulto[resultIndex].ObrValue = drPatRes["res_chr2"].ToString();
                            dtPatientResulto[resultIndex].ObrType = Convert.ToInt32(drPatRes["res_type2"]);
                        }
                    }
                }
                //dtPatientResulto.AcceptChanges();
                return dtPatientResulto;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetResultTable", ex.ToString());
                throw;
            }
        }

        private void repositoryItemTextEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gridViewSingle.GetFocusedDataSourceRowIndex() == (gridViewSingle.RowCount - 1))
                {
                    gridViewSingle.FocusedRowHandle = 0;
                    gridViewSingle.FocusedColumn = colres_chr2;
                    colres_chr2.OptionsColumn.AllowFocus = true;
                    colres_chr.OptionsColumn.AllowFocus = false;
                    gridControlSingle.Refresh();
                }
            }
        }

        private void repositoryItemTextEdit3_Enter(object sender, EventArgs e)
        {
            if (colres_chr2.OptionsColumn.AllowFocus)
            {
                colres_chr2.OptionsColumn.AllowFocus = false;
            }
        }

        private void repositoryItemTextEdit4_Enter(object sender, EventArgs e)
        {
            if (colres_chr.OptionsColumn.AllowFocus)
            {
                colres_chr.OptionsColumn.AllowFocus = false;
            }

        }

        private void gridControlSingle_Click(object sender, EventArgs e)
        {
            colres_chr.OptionsColumn.AllowFocus = true;
            colres_chr2.OptionsColumn.AllowFocus = true;
        }

        public delegate void ColumnTypeChange();

        public event ColumnTypeChange ColumnChange;

        private void MenuSingleView()
        {
            if (ColumnChange != null)
            {
                ColumnChange();
            }
        }

        private void gridViewSingle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.prop != null && this.prop.Visible)
            {
                GridView sourceGrid = sender as GridView;
                this.ItemProp(sourceGrid, ItemProColumn);
            }
        }
        private void SetModifyFlag(DataTable dtPatResult)
        {
            if (dtPatResult != null && dtPatResult.Rows.Count > 0)
            {
                GetSysoperationlog();
                if ((listSysOprLog != null && (listSysOprLog.FindAll(i => i.OperatAction == "修改").Count > 0)))
                {
                    if (!dtPatResult.Columns.Contains("ismodify"))
                        dtPatResult.Columns.Add("ismodify");
                    if (!dtPatResult.Columns.Contains("ismodify2"))
                        dtPatResult.Columns.Add("ismodify2");
                    for (int i = 0; i < dtPatResult.Rows.Count; i++)
                    {
                        try
                        {
                            DataRow row = dtPatResult.Rows[i];

                            string ecd = row["res_itm_ecd"] != null ? row["res_itm_ecd"].ToString() : "";
                            string ecd2 = row["res_itm_ecd2"] != null ? row["res_itm_ecd2"].ToString() : "";

                            if (!string.IsNullOrEmpty(ecd) &&
                                 listSysOprLog.FindAll(w => w.OperatAction == "修改" && w.OperatObject == ecd).Count > 0)
                            {
                                row["ismodify"] = 1;
                            }

                            if (!string.IsNullOrEmpty(ecd2) &&
                           listSysOprLog.FindAll(w => w.OperatAction == "修改" && w.OperatObject == ecd2).Count > 0)
                            {
                                row["ismodify2"] = 1;
                            }
                        }
                        catch
                        {
                        }

                    }
                }
            }
        }
        private void GetSysoperationlog()
        {
            if (!string.IsNullOrEmpty(this.PatID) && listSysOprLog == null)
            {
                ProxySysOperationLog proxy = new ProxySysOperationLog();
                EntityLogQc qc = new EntityLogQc();
                qc.Operatkey = PatID;
                qc.OperatModule = "病人资料";
                listSysOprLog = proxy.Service.GetOperationLog(qc);
            }
        }

        private void gridViewSingle_MouseMove(object sender, MouseEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView == null)
                return;

            GridHitInfo info;
            Point pt = gridView.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = gridView.CalcHitInfo(pt);
            if (info == null)
                return;


            if (info.InRow && info.Column != null
                && (info.Column.FieldName == "history_result1"
                || info.Column.FieldName == "history_result2"
                || info.Column.FieldName == "history_result3"
                || info.Column.FieldName == "history_result12"
                || info.Column.FieldName == "history_result22"
                || info.Column.FieldName == "history_result32"))
            {

                if (info.InRow && info.Column != null && info.Column.FieldName == "history_result1")
                {
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date1"].ToString()))
                    {
                        if (note_history_col == 1 && note_history_rowValue == dr["res_itm_ecd"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 1;
                            note_history_rowValue = dr["res_itm_ecd"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("1 项目:" + dr["res_itm_ecd"].ToString() + "\r\n" + dr["history_date1"].ToString());
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "history_result2")
                {
                    //DataRow dr = gridView.GetFocusedDataRow();
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date2"].ToString()))
                    {
                        if (note_history_col == 2 && note_history_rowValue == dr["res_itm_ecd"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 2;
                            note_history_rowValue = dr["res_itm_ecd"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("2 项目:" + dr["res_itm_ecd"].ToString() + "\r\n" + dr["history_date2"].ToString());
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "history_result3")
                {
                    //DataRow dr = gridView.GetFocusedDataRow();
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date3"].ToString()))
                    {
                        if (note_history_col == 3 && note_history_rowValue == dr["res_itm_ecd"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 3;
                            note_history_rowValue = dr["res_itm_ecd"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("3 项目:" + dr["res_itm_ecd"].ToString() + "\r\n" + dr["history_date3"].ToString());
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "history_result12")
                {
                    //DataRow dr = gridView.GetFocusedDataRow();
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date12"].ToString()))
                    {
                        if (note_history_col == 3 && note_history_rowValue == dr["res_itm_ecd2"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 3;
                            note_history_rowValue = dr["res_itm_ecd2"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("1 项目:" + dr["res_itm_ecd2"].ToString() + "\r\n" + dr["history_date12"].ToString());
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "history_result22")
                {
                    //DataRow dr = gridView.GetFocusedDataRow();
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date22"].ToString()))
                    {
                        if (note_history_col == 3 && note_history_rowValue == dr["res_itm_ecd2"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 3;
                            note_history_rowValue = dr["res_itm_ecd2"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("2 项目:" + dr["res_itm_ecd2"].ToString() + "\r\n" + dr["history_date22"].ToString());
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "history_result32")
                {
                    //DataRow dr = gridView.GetFocusedDataRow();
                    DataRow dr = gridView.GetDataRow(info.RowHandle);
                    if (dr != null && !Compare.IsNullOrDBNull(dr["history_date32"].ToString()))
                    {
                        if (note_history_col == 3 && note_history_rowValue == dr["res_itm_ecd2"].ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 3;
                            note_history_rowValue = dr["res_itm_ecd2"].ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("3 项目:" + dr["res_itm_ecd2"].ToString() + "\r\n" + dr["history_date32"].ToString());
                    }
                }
            }
            else if (info.InRow && info.Column != null && info.Column.FieldName == "res_ref_flag")
            {
                //如果为特定列，不消除，防止跟点击事件冲突
                note_history_col = 0;//非历史结果列,标识为0
            }
            else
            {
                note_history_col = 0;//非历史结果列,标识为0

                if (toolTipController1.Active)
                    toolTipController1.HideHint();
            }
        }
    }
}
