using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml;
using dcl.client.wcf;

using dcl.entity;
using System.Linq;

namespace dcl.client.msgclient
{
    public partial class FrmSelectHistory : Form
    {
        /// <summary>
        /// 科室
        /// </summary>
        private string deptID;

        /// <summary>
        /// 忽略科室(或病区)
        /// </summary>
        public string patNeglectDep;

        /// <summary>
        /// 医生代码
        /// </summary>
        public string patDoctorcode;

        /// <summary>
        /// 病人来源
        /// </summary>
        public string patOriID;

        public FrmSelectHistory()
        {
            InitializeComponent();

        }
        public FrmSelectHistory(string dep_code)
            : this()
        {

            if (!string.IsNullOrEmpty(dep_code))
            {
                //ProxyMessage proxy = new ProxyMessage();
                ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                //加载科室信息
                //DataTable dept = proxy.Service.GetDeptInfo();
                List<EntityDicPubDept> dept = proxyObrMsg.Service.GetDeptInfo();

                if (!string.IsNullOrEmpty(dep_code) && dep_code.Contains(",") && dep_code.Contains("'"))
                {
                    //DataRow[] rows = dept.Select(string.Format("dep_code in({0})", dep_code));
                    List<EntityDicPubDept> listDeptSel = new List<EntityDicPubDept>();
                    //dept.Where(w => dep_code.Contains(w.DeptCode)).ToList();//这里筛选出来的值会变多,故不能用这个
                    string depCodeList = dep_code.Replace("'", "");
                    string[] strArray = depCodeList.Split(',');
                    foreach (var str in strArray)
                    {
                        foreach (var info in dept)
                        {
                            if (info.DeptCode.Equals(str))
                            {
                                listDeptSel.Add(info);
                            }
                        }
                    }

                    listDeptSel = listDeptSel.FindAll(w => w.DeptSource == patOriID);
                    if (listDeptSel.Count > 0 && listDeptSel[0].DeptName != null)
                    {
                        string strtempdepname = "";
                        for (int i = 0; i < listDeptSel.Count; i++)
                        {
                            strtempdepname += listDeptSel[i].DeptName + ",";
                        }
                        if (!string.IsNullOrEmpty(strtempdepname)) { strtempdepname = strtempdepname.TrimEnd(new char[] { ',' }); }
                        lblDepCode.Text = strtempdepname;
                    }
                }
                else
                {
                    //DataRow[] rows = dept.Select(string.Format("dep_code='{0}'", dep_code));
                    List<EntityDicPubDept> listDept = dept.Where(w => w.DeptCode == dep_code).ToList();
                    if (listDept.Count > 0 && listDept[0].DeptName != null)
                    {
                        lblDepCode.Text = listDept[0].DeptName;
                    }
                }

                deptID = dep_code;

            }
            else
            {
                lblDepCode.Text = "";
            }
        }

        #region 这个方法没用到，暂时先屏蔽
        //private DataTable dtTest()
        //{
        //    EntityPatients patients = new EntityPatients();
        //    patients.PidDeptName = "内科一区";
        //    patients.PidBedNo = "xxg22";
        //    patients.PidName = "梁森泉";
        //    patients.PidSex = "男";
        //    patients.PidAge = 87;
        //    patients.PidResult = "";
        //    patients.PidChkName = "刘首明";
        //    patients.RepAuditDate = Convert.ToDateTime("2012-05-05 09:57:28.720");
        //    patients.PatLookName = "";
        //    patients.RepReadDate = Convert.ToDateTime("2012-05-05 10:08:29.017");
        //    patients.PidDatediff = "";
        //    patients.AffirmMode = "";
        //    patients.PidRes = "";
        //    patients.RepId = "123456789123456789";

        //    DataTable dt = new DataTable("pat");

        //    dt.Columns.Add("dep_name");
        //    dt.Columns.Add("pat_bed_no");
        //    dt.Columns.Add("pat_name");
        //    dt.Columns.Add("pat_sex");
        //    dt.Columns.Add("pat_age");
        //    dt.Columns.Add("pat_result");
        //    dt.Columns.Add("pat_chk_name");
        //    dt.Columns.Add("pat_chk_date");
        //    dt.Columns.Add("pat_look_name");
        //    dt.Columns.Add("pat_look_date");
        //    dt.Columns.Add("pat_datediff");
        //    dt.Columns.Add("affirm_mode");
        //    dt.Columns.Add("pat_res");
        //    dt.Columns.Add("pat_id");

        //    DataRow dr = dt.NewRow();
        //    dr[0] = "内科一区";
        //    dr[1] = "xxg22";
        //    dr[2] = "梁森泉";
        //    dr[3] = "男";
        //    dr[4] = "87岁";
        //    dr[5] = "";
        //    dr[6] = "刘首明";
        //    dr[7] = "2012-05-05 09:57:28.720";
        //    dr[8] = "";
        //    dr[9] = "2012-05-05 10:08:29.017";
        //    dr[10] = "";
        //    dr[11] = "";
        //    dr[12] = "";
        //    dr[13] = "123456789123456789";

        //    dt.Rows.Add(dr);

        //    return dt;
        //}
        #endregion

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(deptID)
                && (string.IsNullOrEmpty(patNeglectDep)
                || ((!string.IsNullOrEmpty(patNeglectDep)) && patNeglectDep != "1")))
            {
                MessageBox.Show("科室号不能为空！");
                return;
            }

            if (dtpLookStart.Value.Date > dtpLookEnd.Value.Date)
            {
                MessageBox.Show("请选择正确的日期范围！");
                return;
            }

            string p_msg_type = "1024";//消息类型,默认1024,危急信息
            string p_time_start = dtpLookStart.Value.Date.ToString();//开始时间
            string p_time_end = dtpLookEnd.Value.Date.AddDays(1).ToString();//结束时间
            string p_receiver_id = deptID;//科室ID

            //DataTable dtWhere = new DataTable("sqlWhere");
            //drWhere["msg_type"] = p_msg_type;
            //drWhere["create_time_start"] = p_time_start;
            //drWhere["create_time_end"] = p_time_end;
            //drWhere["receiver_id"] = p_receiver_id;
            //drWhere["is_neglect_dep"] = this.patNeglectDep;//忽略科室(或病区)
            //drWhere["pat_ori_config"] = this.patOriID;//病人来源配置
            //drWhere["pat_doc_id"] = this.patDoctorcode;//医生代码

            EntityUrgentHistoryUseParame eyUrgentParm = new EntityUrgentHistoryUseParame();
            eyUrgentParm.MsgType = p_msg_type;
            eyUrgentParm.CreateTimeStart = p_time_start;
            eyUrgentParm.CreateTimeEnd = p_time_end;
            eyUrgentParm.ReceiveID = p_receiver_id;
            eyUrgentParm.IsNeglectDep = this.patNeglectDep;//忽略科室(或病区)
            eyUrgentParm.PatOriConfig = this.patOriID;//病人来源配置
            eyUrgentParm.PatDocId = this.patDoctorcode;//医生代码

            //DataSet dsHistory = proxy.Service.GetUrgentHistoryMsg(dsWhere);//获取危急值历史信息
            ProxyUrgentObrMessage proxyUrgentObrMsg = new ProxyUrgentObrMessage();
            List<EntityPidReportMain> listHistory = proxyUrgentObrMsg.Service.GetUrgentHistoryMsgSqlWhere(eyUrgentParm);//获取危急值历史信息

            //if (dsHistory != null && dsHistory.Tables.Count > 0 && dsHistory.Tables["UrgentHistory"] != null
            //    && dsHistory.Tables["UrgentHistory"].Rows.Count > 0)
            if (listHistory != null && listHistory.Count > 0)
            {
                //foreach (DataRow drU in dsHistory.Tables["UrgentHistory"].Rows)
                foreach (var infoU in listHistory)
                {
                    string cType = "h";
                    int cValue = 2;
                    readXml(out cType, out cValue);
                    //EntityTimeSpan etSpan = DIYdatediff(drU["pat_report_date"].ToString(), drU["pat_look_date"].ToString(), cType, cValue);
                    string pat_report_date = infoU.RepReportDate != null ? infoU.RepReportDate.ToString() : null;
                    string pat_look_date = infoU.RepReadDate != null ? infoU.RepReadDate.ToString() : null;

                    EntityTimeSpan etSpan = DIYdatediff(pat_report_date, pat_look_date, cType, cValue);

                    infoU.PidDatediff = etSpan.strSpan;
                }

                gcLookData.DataSource = listHistory;
                gvLookData.BestFitColumns();
            }
            else
            {
                gcLookData.DataSource = null;
                MessageBox.Show("查不到相关数据！");
            }
        }

        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="cType">时差单位</param>
        /// <param name="cValue">指定对比值</param>
        /// <returns></returns>
        private EntityTimeSpan DIYdatediff(string startdate, string enddate, string cType, int cValue)
        {
            DateTime p_dtimeStart = DateTime.Now;
            DateTime p_dtimeEnd = DateTime.Now;

            EntityTimeSpan strRv = new EntityTimeSpan();

            if (DateTime.TryParse(startdate, out p_dtimeStart) && DateTime.TryParse(enddate, out p_dtimeEnd))
            {
                #region 计算时差

                decimal t_day = 0;
                decimal t_hout = 0;
                decimal t_minute = 0;
                decimal t_second = 0;

                TimeSpan tspan = (p_dtimeEnd - p_dtimeStart).Duration();
                //strRv.t_day = t_day = Convert.ToDecimal(tspan.Days);
                //strRv.t_hout = t_hout = Convert.ToDecimal(tspan.Hours) - t_day * 24;
                //strRv.t_minute = t_minute = Convert.ToDecimal(tspan.Minutes) - Convert.ToDecimal(tspan.Hours) * 60;
                //strRv.t_second = t_second = Convert.ToDecimal(tspan.Seconds) - Convert.ToDecimal(tspan.Minutes) * 60;

                strRv.t_day = t_day = Convert.ToDecimal(tspan.Days);
                strRv.t_hout = t_hout = Convert.ToDecimal(tspan.Hours);
                strRv.t_minute = t_minute = Convert.ToDecimal(tspan.Minutes);
                strRv.t_second = t_second = Convert.ToDecimal(tspan.Seconds);

                if (t_day > 0)
                {
                    strRv.strSpan = string.Format("{0}天{1}时{2}分{3}秒", t_day.ToString(), t_hout.ToString(), t_minute.ToString(), t_second.ToString());
                }
                else if (t_hout > 0)
                {
                    strRv.strSpan = string.Format("{0}时{1}分{2}秒", t_hout.ToString(), t_minute.ToString(), t_second.ToString());
                }
                else if (t_minute > 0)
                {
                    strRv.strSpan = string.Format("{0}分{1}秒", t_minute.ToString(), t_second.ToString());
                }
                else if (t_second > 0)
                {
                    strRv.strSpan = string.Format("{0}秒", t_second.ToString());
                }
                else
                {
                    strRv.strSpan = "时间相同";
                }

                #endregion

                #region 时差差额对比

                if (string.IsNullOrEmpty(cType))//如果时差单位为空,默认小于
                {
                    strRv.Isgreater = false;
                }
                else if (cValue < 0)
                {
                    strRv.Isgreater = false;
                }
                else
                {
                    switch (cType)
                    {
                        case "d":
                            strRv.Isgreater = t_day > Convert.ToDecimal(cValue);
                            break;
                        case "h":
                            strRv.Isgreater = (t_hout + t_day * 24) > Convert.ToDecimal(cValue);
                            break;
                        case "m":
                            strRv.Isgreater = (t_minute + (t_hout + t_day * 24) * 60) > Convert.ToDecimal(cValue);
                            break;
                        case "s":
                            strRv.Isgreater = (t_second + (t_minute + (t_hout + t_day * 24) * 60) * 60) > Convert.ToDecimal(cValue);
                            break;
                        case "day":
                            strRv.Isgreater = t_day > Convert.ToDecimal(cValue);
                            break;
                        case "hout":
                            strRv.Isgreater = (t_hout + t_day * 24) > Convert.ToDecimal(cValue);
                            break;
                        case "minute":
                            strRv.Isgreater = (t_minute + (t_hout + t_day * 24) * 60) > Convert.ToDecimal(cValue);
                            break;
                        case "second":
                            strRv.Isgreater = (t_second + (t_minute + (t_hout + t_day * 24) * 60) * 60) > Convert.ToDecimal(cValue);
                            break;
                        default:
                            strRv.Isgreater = t_day > Convert.ToDecimal(cValue);
                            break;
                    }
                }
                #endregion
            }

            return strRv;
        }

        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="gcExcel"></param>
        private void setExcel(DevExpress.XtraGrid.GridControl gcExcel)
        {
            if (gcExcel.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        MessageBox.Show("文件名不能为空！");
                        return;
                    }
                    try
                    {
                        //gcExcel.ExportToExcelOld(ofd.FileName.Trim());
                        gcExcel.ExportToXls(ofd.FileName.Trim());
                        MessageBox.Show("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        /// <summary>
        /// 点击导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (gcLookData.DataSource != null)
            {
                //如果细菌组,则显示 MDR预警报告
                //如果病理组与血库组,则显示 其他预警报告
                string xijun_TypeIDs = ConfigurationManager.AppSettings["xijun_typeids"];
                string bingli_TypeIDs = ConfigurationManager.AppSettings["bingli_typeids"];

                List<EntityPidReportMain> listNoteData = gcLookData.DataSource as List<EntityPidReportMain>;

                if ((!string.IsNullOrEmpty(xijun_TypeIDs) && xijun_TypeIDs.ToLower() != "null"
                    || !string.IsNullOrEmpty(bingli_TypeIDs) && bingli_TypeIDs.ToLower() != "null"))
                {
                    FrmExportType frmety = new FrmExportType();
                    frmety.TopMost = true;
                    frmety.ShowDialog();

                    if (string.IsNullOrEmpty(frmety.strType))
                    {
                        return;
                    }
                    else
                    {
                        List<EntityPidReportMain> listNoteDataClone = new List<EntityPidReportMain>();
                        for (int i = 0; i < listNoteData.Count; i++)
                        {
                            EntityPidReportMain eyTempSel = listNoteData[i];

                            //如果细菌组,则显示 MDR预警报告
                            if (!string.IsNullOrEmpty(xijun_TypeIDs) && !string.IsNullOrEmpty(eyTempSel.ItrLabId)
                                && xijun_TypeIDs.Contains(eyTempSel.ItrLabId)
                                && frmety.strType == "1")
                            {
                                listNoteDataClone.Add(eyTempSel);
                            }

                            else if (!string.IsNullOrEmpty(bingli_TypeIDs) && !string.IsNullOrEmpty(eyTempSel.ItrLabId)
                                && bingli_TypeIDs.Contains(eyTempSel.ItrLabId)
                                && frmety.strType == "2")
                            {
                                //如果病理组与血库组,则显示 其他预警报告
                                listNoteDataClone.Add(eyTempSel);
                            }
                            else if (frmety.strType == "0")
                            {
                                listNoteDataClone.Add(eyTempSel);
                            }
                        }

                        if (listNoteDataClone.Count > 0)
                        {
                            gcLookData.DataSource = listNoteDataClone;
                        }
                        else
                        {
                            MessageBox.Show("没有可导出的数据！");
                            return;
                        }
                    }
                }

                setExcel(gcLookData);

                gcLookData.DataSource = listNoteData;
            }
            else
            {
                MessageBox.Show("没有可导出的数据");
            }
        }

        private void FrmSelectHistory_Load(object sender, EventArgs e)
        {
            //nw危急值标题
            string NwAppTile = ConfigurationManager.AppSettings["NwAppTile"];

            if (!string.IsNullOrEmpty(NwAppTile) && NwAppTile != "危急值报告提示")
            {
                this.Text = "查看历史数据-" + NwAppTile;

                colpat_result.Caption = "危急值,MDR及其他预警报告";
            }

            //若忽略科室(或病区),则不显示当前科室(或病区)
            //if ((!string.IsNullOrEmpty(patNeglectDep)) && patNeglectDep == "1")
            //{
            //    deptID = "";
            //    lblDepCode.Text = "";
            //}
            //lblDepCode.Text = deptID;
            dtpLookStart.Value = DateTime.Now.Date;
            dtpLookEnd.Value = DateTime.Now.Date;
            saveXml();
        }


        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <returns></returns>
        private string createStrXml()
        {
            string strXml = null;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xtw.WriteStartDocument();
                //根节点
                xtw.WriteStartElement("TIMEDIFFSETTING");

                //子节点
                xtw.WriteStartElement("TIMEDIFF");

                xtw.WriteComment("'UNIT'为d时,表示以'天'为单位");
                xtw.WriteComment("'UNIT'为h时,表示以'小时'为单位");
                xtw.WriteComment("'UNIT'为m时,表示以'分钟'为单位");
                xtw.WriteComment("'UNIT'为s时,表示以'秒'为单位");

                //UNIT
                xtw.WriteStartElement("UNIT");
                xtw.WriteString("h");
                xtw.WriteEndElement();

                xtw.WriteComment("'QUANTITY'为时差额数");
                //quantity
                xtw.WriteStartElement("QUANTITY");
                xtw.WriteString("2");
                xtw.WriteEndElement();


                xtw.WriteEndElement();//TIMEDIFF
                xtw.WriteEndElement();//TIMEDIFFSETTING
                xtw.WriteEndDocument();

                strXml = sw.ToString();
            }
            return strXml;
        }

        /// <summary>
        /// 保存xml时差配置文件
        /// </summary>
        private void saveXml()
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "TIMEDIFFSETTING.XML";
            try
            {
                string strXml = null;

                if (File.Exists(filepath))
                {
                    return;
                }

                strXml = this.createStrXml();
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
        /// 读取xml关于时间差的配置
        /// </summary>
        /// <param name="cType"></param>
        /// <param name="cValue"></param>
        private void readXml(out string cType, out int cValue)
        {
            cType = "h";
            cValue = 2;

            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "TIMEDIFFSETTING.XML";

            try
            {
                if (!File.Exists(filepath))
                {
                    saveXml();
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int p = 0;
                        DataRow row = ds.Tables[0].Rows[0];

                        switch (row["UNIT"].ToString())
                        {
                            case "d":
                                cType = "d";
                                break;
                            case "h":
                                cType = "h";
                                break;
                            case "m":
                                cType = "m";
                                break;
                            case "s":
                                cType = "s";
                                break;
                            case "day":
                                cType = "d";
                                break;
                            case "hout":
                                cType = "h";
                                break;
                            case "minute":
                                cType = "m";
                                break;
                            case "second":
                                cType = "s";
                                break;
                            default:
                                cType = "h";
                                if (File.Exists(filepath))//有误,重新生成
                                {
                                    File.Delete(filepath);
                                    saveXml();
                                }
                                break;
                        }


                        if (int.TryParse(row["QUANTITY"].ToString(), out p))
                        {
                            cValue = p;
                        }
                        else
                        {
                            if (File.Exists(filepath))//有误,重新生成
                            {
                                File.Delete(filepath);
                                saveXml();
                            }
                        }

                        ds.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                    saveXml();
                }
            }

        }

        /// <summary>
        /// 双击修改处理意见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            string strKey_PatientCRUD = ConfigurationManager.AppSettings["svc.PatientCRUD"];

            if (string.IsNullOrEmpty(strKey_PatientCRUD))
            {
                return;
            }


            if (gvLookData.GetFocusedDataRow() == null) return;
            int rowIndex = gvLookData.FocusedRowHandle;

            EntityPidReportMain eyPatData = gvLookData.GetFocusedRow() as EntityPidReportMain;

            if (eyPatData != null)
            {
                FrmUpdatePatRes frmuppatres = new FrmUpdatePatRes(eyPatData);
                if (frmuppatres.ShowDialog() == DialogResult.Yes)
                {
                    btnSel_Click(null, null);
                }
            }
        }

    }

    /// <summary>
    /// 时间差实体
    /// </summary>
    internal class EntityTimeSpan
    {
        /// <summary>
        /// 时间差结果
        /// </summary>
        public string strSpan { get; set; }

        /// <summary>
        /// 相差天数
        /// </summary>
        public decimal t_day { get; set; }

        /// <summary>
        /// 相差小时
        /// </summary>
        public decimal t_hout { get; set; }

        /// <summary>
        /// 相差分钟
        /// </summary>
        public decimal t_minute { get; set; }

        /// <summary>
        /// 相差秒
        /// </summary>
        public decimal t_second { get; set; }

        /// <summary>
        /// 是否大于指定值
        /// </summary>
        public bool Isgreater { get; set; }

        public EntityTimeSpan()
        {
            strSpan = "";
            t_day = 0;
            t_hout = 0;
            t_minute = 0;
            t_second = 0;
            Isgreater = false;
        }
    }
}
