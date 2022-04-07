using dcl.client.cache;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;
using Lib.DAC;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dcl.client.whonet
{
    public partial class FrmWhonet : Form
    {
        public FrmWhonet()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportExeclClicked);
            this.sysToolBar1.BtnGetVersionClick += new System.EventHandler(this.sysToolBar1_UpdateWho_Clicked);
            this.sysToolBar1.BtnAnswerClick += new System.EventHandler(this.sysToolBar1_OnBtnExportOrigionExeclClicked);
            this.sysToolBar1.BtnRevertDefaultClick += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnBtnCloseClicked);
        }
        string dictFileNameDrug = "WhoDict.xls";
        DataTable tableAntibio = null;
        DataTable tableBac = null;
        public static List<string> addColumn = new List<string>();
        private void FrmWhonet_Load(object sender, EventArgs e)
        {
            sysToolBar1.BtnRevertDefault.Caption = "导出";
            sysToolBar1.BtnExport.Caption = "导出execl";
            sysToolBar1.BtnAnswer.Caption = "导出原始数据execl";
            sysToolBar1.BtnGetVersion.Caption = "更新检验who码";
            sysToolBar1.SetToolButtonStyle(new string[] {
                                                                    sysToolBar1.BtnSearch.Name,
                                                                    sysToolBar1.BtnAnswer.Name,
                                                                    sysToolBar1.BtnExport.Name,
                                                                    sysToolBar1.BtnGetVersion.Name,
                                                                    sysToolBar1.BtnRevertDefault.Name
                                                              });
            sysToolBar1.BtnAnswer.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            List<EntityDicInstrument> listItr = CacheClient.GetCache<EntityDicInstrument>().FindAll(w => w.ItrReportType == "3" && w.DelFlag == "0");

            this.pnlItr.Controls.Clear();
            foreach (EntityDicInstrument itr in listItr)
            {
                CheckBox chk = new CheckBox();
                chk.Text = itr.ItrEname;
                chk.Tag = itr.ItrId;
                chk.Checked = true;
                pnlItr.Controls.Add(chk);
            }
            addColumn = new List<string>() { "MRCNS", "MDRPAE", "MDRABA", "HLAR" };
            this.saveFileDialog1.InitialDirectory = @"c:\";

            //设置打开界面时的默认时间分为为当前月的第一天到当前月的最后一天

            DateTime dtFirstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dtBegin.EditValue = dtFirstDayOfMonth;

            DateTime dtLastDayOfMonth = dtFirstDayOfMonth.AddMonths(1).AddDays(-1);
            this.dtEnd.EditValue = dtLastDayOfMonth;

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            DateTime dtBegin = this.dtBegin.DateTime;
            DateTime dtEnd = this.dtEnd.DateTime.AddDays(1);
            int dayInterval = (dtEnd - dtBegin).Days;
            if (dayInterval < 0)
            {
                MessageBox.Show("开始日期不能大于结束日期", "错误");
                return;
            }
            EntityAntiQc qc = new EntityAntiQc();
            qc.DateStart = dtBegin;
            qc.DateEnd = dtEnd;
            qc.ExportDataType = this.rgExportType.EditValue?.ToString() == "0" ? 0 : 1;
            foreach (Control ctrl in this.pnlItr.Controls)
            {
                if (ctrl is CheckBox)
                {
                    CheckBox chk = ctrl as CheckBox;
                    if (chk.Checked)
                    {
                        qc.ListItrId.Add(chk.Tag.ToString());
                    }
                }
            }
            //获取日期范围内的所有抗生素whonet码
            List<string> antibios = new ProxyWhonet().Service.GetAntibosName(qc);
            //生成数据表结构
            DataTable tableData;
            if (checkBox1.Checked)
            {
                tableData = CreateWhoStruct(antibios);

                //每次获取的天数
                int fetchPerDays = (int)this.numericUpDown1.Value;

                //获取的次数
                int fetchCount = (dayInterval + 1) / fetchPerDays;

                DateTime dateBegin = dtBegin;
  
                for (int i = 1; i <= fetchCount; i++)
                {
                    DateTime dateEnd = dateBegin.AddDays(fetchPerDays);
                    if (dateEnd > dtEnd)
                    {
                        dateEnd = dtEnd;
                    }
                    qc.DateStart = dateBegin;
                    qc.DateEnd = dateEnd;
                    DataTable tableTemp = CreateWhoData(qc, this.txtLabName.Text, this.chkRemoveLogical.Checked);
                    tableData.Merge(tableTemp);

                    dateBegin = dateEnd;
                }
            }
            else
            {
                tableData = CreateWhoData(qc, this.txtLabName.Text, this.chkRemoveLogical.Checked);
            }
            
            this.gcData.DataSource = tableData;
            if(gvData.Columns.Count>0)
            {
                gvData.Columns[0].SummaryItem.DisplayFormat = "记录总数:{0:0.##}";
                gvData.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            }

        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            Export(EnumExportType.DBF, false);
        }
        /// <summary>
        /// 导出execl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportExeclClicked(object sender, EventArgs e)
        {
            Export(EnumExportType.Excel, false);
        }
        /// <summary>
        /// 导出原始数据execl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportOrigionExeclClicked(object sender, EventArgs e)
        {
            Export(EnumExportType.Excel, true);
        }
        /// <summary>
        /// 更新检验who码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_UpdateWho_Clicked(object sender, EventArgs e)
        {
            ExtractWhoDict();
            ReadDictToMemory();
            UpdateBac();
            UpdateAnti();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="isOriDataExport">是否</param>
        private void Export(EnumExportType exportType, bool isOriDataExport)
        {
            if (this.gcData.DataSource == null || ((DataTable)this.gcData.DataSource).Rows.Count == 0)
            {
                MessageBox.Show("没有可导出的数据", "提示");
                return;
            }

            DataTable dtSource = (DataTable)this.gcData.DataSource;
            DataTable data = dtSource.Copy();

            DataTable dtTemp = null;
            if (this.txtLabName.Text.Length == 0 && exportType == EnumExportType.DBF)
            {
                MessageBox.Show("请输入实验室名称");
                return;
            }
            string lab_name = this.txtLabName.Text;

            if (exportType == EnumExportType.DBF)
            {
                this.saveFileDialog1.Filter = string.Format("*.{0}|*.{0}", "dbf");
            }
            else if (exportType == EnumExportType.Excel)
            {
                if (data != null && data.Rows.Count > 0 && chkRemoveLogicalExport.Checked)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                            if (!data.Columns[j].ColumnName.Contains("COUNTRY_A") && !data.Columns[j].ColumnName.Contains("LABORATORY")
                               && !data.Columns[j].ColumnName.Contains("PATIENT_ID") && !data.Columns[j].ColumnName.Contains("FIRST_NAME")
                                && !data.Columns[j].ColumnName.Contains("LAST_NAME") && !data.Columns[j].ColumnName.Contains("SEX")
                                && !data.Columns[j].ColumnName.Contains("AGE") && !data.Columns[j].ColumnName.Contains("DATE_BIRTH")
                                && !data.Columns[j].ColumnName.Contains("WARD") && !data.Columns[j].ColumnName.Contains("INSTITUT")
                                && !data.Columns[j].ColumnName.Contains("DEPARTMENT") && !data.Columns[j].ColumnName.Contains("WARD_TYPE")
                                && !data.Columns[j].ColumnName.Contains("PAT_TYPE") && !data.Columns[j].ColumnName.Contains("SPEC_NUM")
                                && !data.Columns[j].ColumnName.Contains("SPEC_DATE") && !data.Columns[j].ColumnName.Contains("SPEC_TYPE")
                                && !data.Columns[j].ColumnName.Contains("SPEC_CODE") && !data.Columns[j].ColumnName.Contains("SPEC_REAS")
                                && !data.Columns[j].ColumnName.Contains("DATE_DATA") && !data.Columns[j].ColumnName.Contains("ORGANISM")
                                && !data.Columns[j].ColumnName.Contains("ORG_TYPE"))
                            {
                                if (!string.IsNullOrEmpty(data.Rows[i][j].ToString()))
                                {
                                    data.Rows[i][j] = data.Rows[i][j].ToString().Replace(">", "").Replace("<", "").Replace("=", "");
                                }
                            }
                        }
                    }

                    data.AcceptChanges();
                }

                this.saveFileDialog1.Filter = string.Format("*.{0}|*.{0}", "xls");
            }

            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (DataRow row in data.Rows)
                {
                    row["LABORATORY"] = lab_name;
                }

                Lib.Utils.ExcelApp.TableToExcelExporter exporter = new Lib.Utils.ExcelApp.TableToExcelExporter();

                Application.DoEvents();

                if (exportType == EnumExportType.DBF)
                {
                    DataTable datacopy = data.Copy();
                    long tempIntTe = 0;
                    if (datacopy.Columns.Contains("PATIENT_ID"))
                    {
                        foreach (DataRow drcopy in datacopy.Rows)
                        {
                            if (drcopy["PATIENT_ID"].ToString().Length > 0
                                && long.TryParse(drcopy["PATIENT_ID"].ToString(), out tempIntTe))
                            {
                                drcopy["PATIENT_ID"] = "'" + drcopy["PATIENT_ID"].ToString();
                            }
                        }
                        datacopy.AcceptChanges();
                    }
                    if (datacopy.Columns.Contains("AGE")
                        && long.TryParse(datacopy.Rows[0]["AGE"].ToString(), out tempIntTe))
                    {
                        datacopy.Rows[0]["AGE"] = "'" + datacopy.Rows[0]["AGE"].ToString();
                    }
                    exporter.Export(datacopy, true, this.saveFileDialog1.FileName, Lib.Utils.ExcelApp.EnumExcelFileFormat.DBF4);
                }
                else if (exportType == EnumExportType.Excel)
                {
                    DataTable datacopy = data.Copy();
                    long tempIntTe = 0;
                    if (datacopy.Columns.Contains("PATIENT_ID"))
                    {
                        foreach (DataRow drcopy in datacopy.Rows)
                        {
                            if (drcopy["PATIENT_ID"].ToString().Length > 0
                                && long.TryParse(drcopy["PATIENT_ID"].ToString(), out tempIntTe))
                            {
                                drcopy["PATIENT_ID"] = "'" + drcopy["PATIENT_ID"].ToString();
                            }
                        }
                        datacopy.AcceptChanges();
                    }
                    if (datacopy.Columns.Contains("AGE")
                        && long.TryParse(datacopy.Rows[0]["AGE"].ToString(), out tempIntTe))
                    {
                        datacopy.Rows[0]["AGE"] = "'" + datacopy.Rows[0]["AGE"].ToString();
                    }
                    InsertDataToExcel2(this.saveFileDialog1.FileName, datacopy);
                }
                MessageBox.Show("导出完成");
            }
        }

        private void InsertDataToExcel2(string decPath, DataTable dtExcalInfo)
        {
            try
            {
                #region 导出方法

                if (true)
                {
                    //======= 1. 生成模板文件 ==========
                    HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");


                    List<string> lsColNms = new List<string>();//字段名称
                    List<string> lsCaptions = new List<string>();//显示名称

                    //sw.Stop();

                    int noteTempStartIndex = 0;//记录开始添加行

                    for (int j = 0; j < dtExcalInfo.Columns.Count; j++)
                    {

                        lsColNms.Add(dtExcalInfo.Columns[j].ColumnName);
                        lsCaptions.Add(dtExcalInfo.Columns[j].ColumnName);

                    }

                    //添加显示的列名
                    if (lsCaptions.Count > 0)
                    {
                        int tempRowrum = 0;//从第几行开始添加

                        if (noteTempStartIndex > 0)//如果不是第一行，则相隔2行再添加
                        {
                            tempRowrum = noteTempStartIndex + 2;
                            noteTempStartIndex = tempRowrum;
                        }

                        IRow row = sheet1.CreateRow(tempRowrum);
                        for (int j = 0; j < lsCaptions.Count; j++)
                        {
                            row.CreateCell(j).SetCellValue(lsCaptions[j]);
                        }
                    }

                    double D;
                    for (int j = 0; j < dtExcalInfo.Rows.Count; j++)
                    {
                        noteTempStartIndex++;
                        IRow row = sheet1.CreateRow(noteTempStartIndex);
                        for (int k = 0; k < lsColNms.Count; k++)
                        {

                            if (lsColNms[k] == "SPEC_CODE" || lsColNms[k].Contains("_NM") || lsColNms[k].Contains("_ND"))
                            {
                                if (double.TryParse(dtExcalInfo.Rows[j][lsColNms[k]].ToString().Replace("'", ""), out D))
                                {
                                    row.CreateCell(k).SetCellValue(D);
                                }
                                else
                                {
                                    row.CreateCell(k).SetCellValue(dtExcalInfo.Rows[j][lsColNms[k]].ToString().Replace("'", ""));
                                }
                            }
                            else
                            {
                                row.CreateCell(k).SetCellValue(dtExcalInfo.Rows[j][lsColNms[k]].ToString());
                            }
                        }
                    }

                    //======= 4. 写入文件 ==========
                    FileStream file = new FileStream(decPath, FileMode.Create);
                    book.Write(file);
                    file.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(GetType().FullName, ex);
                throw ex;
            }
        }


        enum EnumExportType
        {
            Excel,
            DBF
        }
        /// <summary>
        /// 生成who结构
        /// </summary>
        /// <param name="whocode"></param>
        /// <returns></returns>
        public DataTable CreateWhoStruct(List<string> whocode)
        {
            //特殊功能模式代码
            string strHopModelCode = ConfigHelper.GetSysConfigValueWithoutLogin("HopModelCode");

            //select 'table.Columns.Add("'+column_name+'");' from information_schema.columns where table_name = 'who_demo'
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(strHopModelCode) && strHopModelCode == "bhyy")
            {
                #region bhyy模式排序

                table.Columns.Add("COUNTRY_A");//国家简写 中国：CHN
                table.Columns.Add("LABORATORY");//实验室简写（最长3位）
                table.Columns.Add("ORIGIN");
                //table.Columns.Add("PATIENT_ID");//病人id
                table.Columns.Add("UPID");//病人upid
                table.Columns.Add("LAST_NAME");//姓
                table.Columns.Add("FIRST_NAME");//名
                table.Columns.Add("SEX");//性别
                table.Columns.Add("DATE_BIRTH");//出生日期
                table.Columns.Add("AGE");//年龄
                table.Columns.Add("PAT_TYPE");//病人类型
                table.Columns.Add("WARD");//病区
                table.Columns.Add("WARD_TYPE");//病区类型
                table.Columns.Add("WARDNAME");//病区
                table.Columns.Add("INSTITUT");//机构
                table.Columns.Add("DEPARTMENT");//科室
                table.Columns.Add("SPEC_NUM");//标本号
                table.Columns.Add("SPEC_DATE");//标本日期
                table.Columns.Add("SPEC_TYPE");//标本类型
                table.Columns.Add("SPEC_CODE");//标本代码
                table.Columns.Add("SPEC_REAS");
                table.Columns.Add("ORGANISM");//菌类
                table.Columns.Add("ORG_TYPE");
                table.Columns.Add("BETA_LACT");//Beta-内酰胺酶
                table.Columns.Add("ESBL");//超广谱Beta-内酰胺酶
                table.Columns.Add("SEROTYPE");//SEROTYPE
                table.Columns.Add("COMMENT");
                table.Columns.Add("DATE_DATA");//数据日期

                //table.Columns.Add("MRSA");//MRSA
                //table.Columns.Add("VRE");//VRE
                //table.Columns.Add("MLS_DTEST");//MLS_DTEST 

                #endregion
            }
            else if (!string.IsNullOrEmpty(strHopModelCode) && strHopModelCode == "gysy")
            {
                #region gysy模式排序

                table.Columns.Add("COUNTRY_A");//国家简写 中国：CHN
                table.Columns.Add("LABORATORY");//实验室简写（最长3位）
                table.Columns.Add("PATIENT_ID");//病人id
                table.Columns.Add("FIRST_NAME");//名
                table.Columns.Add("LAST_NAME");//姓
                table.Columns.Add("SEX");//性别
                table.Columns.Add("AGE");//年龄
                table.Columns.Add("DATE_BIRTH");//出生日期
                table.Columns.Add("WARD");//病区
                table.Columns.Add("INSTITUT");//机构
                table.Columns.Add("DEPARTMENT");//科室
                table.Columns.Add("WARD_TYPE");//病区类型
                table.Columns.Add("PAT_TYPE");//病人类型
                table.Columns.Add("SPEC_NUM");//标本号
                table.Columns.Add("SPEC_DATE");//标本日期
                table.Columns.Add("SPEC_CODE");//标本代码
                table.Columns.Add("SPEC_TYPE");//标本类型
                table.Columns.Add("SPEC_REAS");
                table.Columns.Add("DATE_DATA");//数据日期
                table.Columns.Add("ORGANISM");//菌类
                table.Columns.Add("ORG_TYPE");
                table.Columns.Add("BETA_LACT");//Beta-内酰胺酶
                table.Columns.Add("COMMENT");
                table.Columns.Add("ORIGIN");
                table.Columns.Add("ESBL");//超广谱Beta-内酰胺酶
                table.Columns.Add("MRSA");//MRSA
                table.Columns.Add("VRE");//VRE
                table.Columns.Add("MLS_DTEST");//MLS_DTEST 
                table.Columns.Add("HLAR");//HLAR


                //table.Columns.Add("WARDNAME");//病区
                //table.Columns.Add("SEROTYPE");//SEROTYPE


                #endregion
            }
            else if (!string.IsNullOrEmpty(strHopModelCode) && strHopModelCode == "hnetyy")
            {
                #region hnetyy模式排序

                table.Columns.Add("COUNTRY_A");//国家简写 中国：CHN
                table.Columns.Add("LABORATORY");//实验室简写（最长3位）
                table.Columns.Add("ORIGIN");
                table.Columns.Add("PATIENT_ID");//病人id
                table.Columns.Add("LAST_NAME");//姓
                table.Columns.Add("FIRST_NAME");//名
                table.Columns.Add("SEX");//性别
                table.Columns.Add("DATE_BIRTH");//出生日期
                table.Columns.Add("AGE");//年龄
                table.Columns.Add("PAT_TYPE");//病人类型
                table.Columns.Add("WARDNAME");//病区
                table.Columns.Add("WARD_TYPE");//病区类型
                table.Columns.Add("WARD");//病区
                table.Columns.Add("INSTITUT");//机构
                table.Columns.Add("DEPARTMENT");//科室
                table.Columns.Add("SPEC_NUM");//标本号
                table.Columns.Add("SPEC_DATE");//标本日期
                table.Columns.Add("SPEC_TYPE");//标本类型
                table.Columns.Add("SPEC_CODE");//标本代码
                table.Columns.Add("SPEC_REAS");
                table.Columns.Add("ORGANISM");//菌类
                table.Columns.Add("ORG_TYPE");
                table.Columns.Add("BETA_LACT");//Beta-内酰胺酶
                table.Columns.Add("ESBL");//超广谱Beta-内酰胺酶
                table.Columns.Add("SEROTYPE");//SEROTYPE
                table.Columns.Add("COMMENT");
                table.Columns.Add("DATE_DATA");//数据日期
                table.Columns.Add("MRSA");//MRSA
                //table.Columns.Add("VRE");//VRE
                //table.Columns.Add("MLS_DTEST");//MLS_DTEST 

                #endregion
            }
            else
            {
                #region 通用

                table.Columns.Add("COUNTRY_A");//国家简写 中国：CHN
                table.Columns.Add("LABORATORY");//实验室简写（最长3位）
                table.Columns.Add("PATIENT_ID");//病人id
                table.Columns.Add("FIRST_NAME");//名
                table.Columns.Add("LAST_NAME");//姓
                table.Columns.Add("SEX");//性别
                table.Columns.Add("AGE");//年龄
                table.Columns.Add("DATE_BIRTH");//出生日期
                table.Columns.Add("WARD");//病区
                table.Columns.Add("INSTITUT");//机构
                table.Columns.Add("DEPARTMENT");//科室
                table.Columns.Add("WARD_TYPE");//病区类型
                table.Columns.Add("PAT_TYPE");//病人类型
                table.Columns.Add("SPEC_NUM");//标本号
                table.Columns.Add("SPEC_DATE");//标本日期
                table.Columns.Add("SPEC_TYPE");//标本类型
                table.Columns.Add("SPEC_CODE");//标本代码
                table.Columns.Add("SPEC_REAS");
                table.Columns.Add("DATE_DATA");//数据日期
                table.Columns.Add("ORGANISM");//菌类
                table.Columns.Add("ORG_TYPE");
                table.Columns.Add("BETA_LACT");//Beta-内酰胺酶
                table.Columns.Add("COMMENT");
                table.Columns.Add("ORIGIN");
                table.Columns.Add("ESBL");//超广谱Beta-内酰胺酶
                table.Columns.Add("MRSA");//MRSA
                table.Columns.Add("VRE");//VRE
                table.Columns.Add("MLS_DTEST");//MLS_DTEST 
                #endregion
            }
            foreach (string who_code in whocode)
            {
                if (who_code.ToLower() == "beta_lact"
                    || who_code.ToLower() == "esbl"
                    || who_code.ToLower() == "icr"
                    || who_code.ToLower() == "mrsa"
                    )
                {
                    continue;
                }
                table.Columns.Add(who_code);
            }

            return table;
        }

        /// <summary>
        /// 生成who数据
        /// </summary>
        /// <param name="qc"></param>
        /// <param name="lab_name">实验室简称</param>
        /// <param name="removeLogical">是否去除大于号小于号</param>
        /// <returns></returns>
        public DataTable CreateWhoData(EntityAntiQc qc, string lab_name, bool removeLogical)
        {
            List<EntityWhonet> listOrigin = new ProxyWhonet().Service.GetAntiData(qc);//获取药敏数据

            DataTable result = CreateWhoData(listOrigin, lab_name, qc.ExportDataType, removeLogical);

            foreach (string item in addColumn)
            {
                if (!result.Columns.Contains(item))
                {
                    result.Columns.Add(item);//MRCNS

                }
            }

            foreach (DataRow item in result.Rows)
            {
                foreach (string add in addColumn)
                {
                    string columnName = add + "_ND";
                    if (item.Table.Columns.Contains(columnName)
                        && item[columnName] != null
                        && !string.IsNullOrEmpty(item[columnName].ToString()))
                    {
                        item[add] = item[columnName];
                    }

                    string columnName2 = add + "_NM";
                    if (item.Table.Columns.Contains(columnName2)
                        && item[columnName2] != null
                        && !string.IsNullOrEmpty(item[columnName2].ToString()))
                    {
                        item[add] = item[columnName2];
                    }
                }

            }

            foreach (string item in addColumn)
            {
                string columnName = item + "_ND";
                if (result.Columns.Contains(columnName))
                {
                    result.Columns.Remove(columnName);
                }
                string columnName2 = item + "_NM";
                if (result.Columns.Contains(columnName2))
                {
                    result.Columns.Remove(columnName2);
                }

            }

            return result;
        }

        /// <summary>
        /// 生成whonet数据
        /// </summary>
        /// <param name="dataOrigin"></param>
        /// <param name="lab_name"></param>
        /// <returns></returns>
        public DataTable CreateWhoData(List<EntityWhonet> listOrigin, string lab_name, int exportDataType, bool removeLogical)
        {
            //特殊功能模式代码
            string strHopModelCode = ConfigHelper.GetSysConfigValueWithoutLogin("HopModelCode");

            //  string COMMENTValue = ConfigurationManager.AppSettings["COMMENTValue"];

            if (string.IsNullOrEmpty(strHopModelCode))
            {
                strHopModelCode = "";
            }
            //if (string.IsNullOrEmpty(COMMENTValue))
            //{
            //    COMMENTValue = "";
            //}

            DataTable whodata = CreateWhoStruct(GetWhoCode(listOrigin));//生成who结构
            DateTime dtToday = DateTime.Now;


            string curr_pat_bacwho = "-1";
            DataRow curr_datarow = null;

            int currRowIndex = -1;
            //遍历每一条药敏结果记录,相同的病人id要拼成一行
            foreach (EntityWhonet rowData in listOrigin)
            {
                currRowIndex++;

                //病人id
                string anr_id = rowData.ObrId;
                string bac_who_no = rowData.BacWhoNo;

                if ((anr_id + bac_who_no) != curr_pat_bacwho)//另一个pat_id
                {
                    if (curr_datarow != null)
                    {
                        whodata.Rows.Add(curr_datarow);
                    }

                    curr_pat_bacwho = (anr_id + bac_who_no);
                    curr_datarow = whodata.NewRow();
                    curr_datarow["COUNTRY_A"] = "CHN";//实验室名称
                    curr_datarow["LABORATORY"] = lab_name.Length > 3 ? lab_name.Substring(0, 3) : lab_name;//实验室简称
                    if (curr_datarow.Table.Columns.Contains("PATIENT_ID"))
                    {
                        curr_datarow["PATIENT_ID"] = rowData.PidInNo;//病人id
                    }

                    if (curr_datarow.Table.Columns.Contains("UPID")
                        && !string.IsNullOrEmpty(rowData.PidUniqueId))
                    {
                        curr_datarow["UPID"] = rowData.PidUniqueId;//病人upid
                    }
                    curr_datarow["FIRST_NAME"] = rowData.PidName;
                    curr_datarow["LAST_NAME"] = string.Empty;

                    //性别
                    string sex = rowData.PidSex;
                    if (sex == "1")
                    {
                        sex = "m";
                    }
                    else if (sex == "2")
                    {
                        sex = "f";
                    }
                    else
                    {
                        sex = string.Empty;
                    }
                    curr_datarow["SEX"] = sex;

                    #region 年龄
                    string age_exp = rowData.PidAgeExp;
                    string age_display = TrimZeroValue(age_exp).ToUpper();

                    if (age_display.Contains("Y"))
                    {
                        age_display = age_display.Split('Y')[0];
                    }
                    else if (age_display.Contains("M"))
                    {
                        age_display = age_display.Split('M')[0] + "m";
                    }
                    else if (age_display.Contains("D"))
                    {
                        age_display = age_display.Split('D')[0] + "d";
                    }
                    else if (age_display.Contains("H"))
                    {
                        age_display = age_display.Split('H')[0] + "h";
                        //if (strHopModelCode == "gysy")
                        //age_display = "1d";
                    }

                    curr_datarow["AGE"] = age_display;
                    #endregion

                    //出生日期
                    curr_datarow["DATE_BIRTH"] = string.Empty;
                    if (curr_datarow.Table.Columns.Contains("WARD"))
                    {
                        curr_datarow["WARD"] = rowData.DeptCode;
                    }

                    if (curr_datarow.Table.Columns.Contains("WARDNAME"))
                    {
                        curr_datarow["WARDNAME"] = rowData.PidDeptName;
                    }


                    curr_datarow["INSTITUT"] = string.Empty;
                    if (curr_datarow.Table.Columns.Contains("DEPARTMENT"))
                    {
                        curr_datarow["DEPARTMENT"] = rowData.PidDeptName;
                    }

                    string ward_type = rowData.SrcName;
                    if (ward_type == "门诊")
                    {
                        ward_type = "out";
                    }
                    else if (ward_type == "住院")
                    {
                        if (strHopModelCode == "hnetyy")
                        {
                            ward_type = "in";
                        }
                        else if (rowData.PidDeptName.Replace(" ", "").ToLower().Contains("icu"))
                        {
                            ward_type = "icu";
                        }
                        else
                        {
                            ward_type = "inx";
                        }
                    }
                    else
                    {
                        ward_type = "oth";
                    }

                    if (strHopModelCode == "gysy")
                    {
                        curr_datarow["WARD_TYPE"] = rowData.OrgId;
                    }
                    else
                    {
                        curr_datarow["WARD_TYPE"] = ward_type;
                    }


                    if (age_display.Contains("d") || age_display.Contains("h"))
                    {
                        curr_datarow["PAT_TYPE"] = "new";
                    }
                    double aged;
                    if (age_display.Contains("m") || double.TryParse(age_display, out aged) && aged < 15)
                    {
                        curr_datarow["PAT_TYPE"] = "ped";
                    }

                    if (double.TryParse(age_display, out aged) && aged > 14 && aged < 66)
                    {
                        curr_datarow["PAT_TYPE"] = "adu";
                    }

                    if (double.TryParse(age_display, out aged) && aged > 65)
                    {
                        curr_datarow["PAT_TYPE"] = "ger";
                    }
                    //curr_datarow["PAT_TYPE"] = string.Empty;



                    if (rowData.RepSid.Trim().Length >= 10)
                    {
                        curr_datarow["SPEC_NUM"] = string.Format("a{0}", rowData.RepSid); ;
                    }
                    else
                    {
                        curr_datarow["SPEC_NUM"] = rowData.RepSid;
                    }

                    curr_datarow["SPEC_DATE"] = rowData.SampSendDate;
                    curr_datarow["SPEC_TYPE"] = rowData.SamName;
                    curr_datarow["SPEC_CODE"] = rowData.SamCode;




                    curr_datarow["SPEC_REAS"] = string.Empty;
                    curr_datarow["DATE_DATA"] = dtToday.ToString("yyyy-MM-dd");


                    curr_datarow["ORGANISM"] = bac_who_no;

                    string ORG_TYPE;
                    if (WhonetDict.Instance.ORG_GRAM.TryGetValue(bac_who_no.Trim(), out ORG_TYPE))
                    {
                        curr_datarow["ORG_TYPE"] = ORG_TYPE;
                    }

                    //curr_datarow["ORG_TYPE"] = string.Empty;

                    curr_datarow["BETA_LACT"] = string.Empty;
                //    curr_datarow["ORGANISM"] = rowData.BacCname;
                    if (!string.IsNullOrEmpty(rowData.ObrRemark))
                    {
                        string sdripe = rowData.ObrRemark;
                        if (sdripe.ToUpper().StartsWith("MDR"))
                        {
                            curr_datarow["COMMENT"] = "MDR";
                        }
                        if (sdripe.ToUpper().StartsWith("XDR"))
                        {
                            curr_datarow["COMMENT"] = "XDR";
                        }
                        if (sdripe.ToUpper().StartsWith("MRS"))
                        {
                            curr_datarow["COMMENT"] = "MRS";
                        }
                        if (sdripe.ToUpper().StartsWith("MRSA"))
                        {
                            curr_datarow["COMMENT"] = "MRSA";
                        }
                        if (sdripe.ToUpper().StartsWith("CRE"))
                        {
                            curr_datarow["COMMENT"] = "CRE";
                        }
                        if (sdripe.ToUpper().StartsWith("CRAB"))
                        {
                            curr_datarow["COMMENT"] = "CRAB";
                        }
                        if (sdripe.ToUpper().StartsWith("D+"))
                        {
                            curr_datarow["COMMENT"] = "D+";
                        }
                        if (sdripe.ToUpper().StartsWith("HLAR"))
                        {
                            curr_datarow["COMMENT"] = "HLAR";
                        }
                        if (sdripe.ToUpper().StartsWith("VRE"))
                        {
                            curr_datarow["COMMENT"] = "VRE";
                        }

                        curr_datarow["COMMENT"] = string.Empty;
                        curr_datarow["ORIGIN"] = string.Empty;
                    }
                }
                //抗生素whonet码
                string who_code = rowData.AntWhoNo;

                string result;

                if (exportDataType == 1)
                {
                    if (who_code.ToLower() == "esbl"
                        || who_code.ToLower() == "beta_lact"
                        || who_code.ToLower() == "icr"
                        || who_code.ToLower() == "mrsa"
                        )
                    {
                        result = rowData.ObrValue.ToLower();

                        //转换为whonet标准结果
                        if (result == "pos" || result == "positive" || result.Contains("阳"))
                        {
                            result = "+";
                        }
                        else if (result == "neg" || result == "negtive" || result.Contains("阴"))
                        {
                            result = "-";
                        }
                        else if (result == "敏感" || result == "s")
                        {
                            result = "S";
                        }
                        else if (result == "耐药" || result == "r")
                        {
                            result = "R";
                        }
                        else if (result == "中介" || result == "i")
                        {
                            result = "I";
                        }

                        if (result == "R")
                        {
                            result = "+";
                        }

                        //if (who_code.ToLower() == "oxa" || who_code.ToLower() == "fox")//对 苯唑西林 或 头孢西丁 鉴定为耐药则判定MRSA为阳性
                        //{
                        //    if (result == "R")
                        //    {
                        //        curr_datarow["MRSA"] = "+";
                        //    }
                        //    else
                        //    {
                        //        curr_datarow["MRSA"] = "-";
                        //    }
                        //}
                        //else if (who_code.ToLower() == "van")//对万古霉素鉴定为中介或耐药则判定VRE为阳性
                        //{
                        //    if (result == "R" || result == "I")
                        //    {
                        //        curr_datarow["VRE"] = "+";
                        //    }
                        //    else
                        //    {
                        //        curr_datarow["VRE"] = "-";
                        //    }
                        //}
                    }
                    else
                    {
                        //数值结果 mic值/kb值
                        string digitResult = rowData.ObrValue2;
                        if (string.IsNullOrEmpty(digitResult))
                        {
                            digitResult = rowData.ObrValue3 != null ? rowData.ObrValue3.ToString() : string.Empty;
                        }
                        if (digitResult.ToLower() == "pos" || digitResult.ToLower() == "positive")
                        {
                            digitResult = "+";
                        }
                        else if (digitResult.ToLower() == "neg" || digitResult.ToLower() == "negitive")
                        {
                            digitResult = "-";
                        }

                        decimal decRes = 0;
                        if (strHopModelCode == "gysy"
                            && decimal.TryParse(digitResult, out decRes))
                        {
                            digitResult = String.Format("{0:N3}", decRes);
                        }

                        //去除大于、小于、等号

                        result = digitResult;

                        if (removeLogical)//是否去除大于、小于、等号
                        {
                            if (who_code.ToLower() == "sxt_nd")//如果是SXT抗生素,则转化'除以40'
                            {
                                result = result.Replace(">", "").Replace("<", "").Replace("=", "");
                                double temp_nm = 0;
                                if (double.TryParse(result, out temp_nm))
                                {
                                    result = (temp_nm / 40).ToString();
                                }
                                result = "'" + result;
                            }
                            if (who_code.ToLower() == "sxt_nm")//如果是SXT抗生素,则转化'除以40'
                            {
                                result = result.Replace(">", "").Replace("<", "").Replace("=", "");
                                double temp_nm = 0;
                                if (double.TryParse(result, out temp_nm))
                                {
                                    result = (temp_nm / 20).ToString();
                                }
                                result = "'" + result;
                            }
                            else
                            {
                                result = "'" + result.Replace(">", "").Replace("<", "").Replace("=", "").Replace("≥", "").Replace("≤", "");
                            }
                        }
                        else
                        {
                            if (who_code.ToLower() == "sxt_nd" || who_code.ToLower() == "sxt_nm")//如果是SXT抗生素,则转化'除以40'
                            {
                                string temp_sign = "";//记录符号
                                if (string.IsNullOrEmpty(result))
                                {
                                }
                                else if (result.StartsWith("<="))//开始位置否是有此符号
                                {
                                    temp_sign = "<=";
                                }
                                else if (result.StartsWith(">="))
                                {
                                    temp_sign = ">=";
                                }
                                else if (result.StartsWith(">"))
                                {
                                    temp_sign = ">";
                                }
                                else if (result.StartsWith("<"))
                                {
                                    temp_sign = "<";
                                }
                                else if (result.StartsWith("="))
                                {
                                    temp_sign = "=";
                                }

                                if (temp_sign != "")//如果开始位置有符号,则去除
                                {
                                    result = result.TrimStart(temp_sign.ToCharArray());
                                }
                                int num = 20;
                                if (who_code.ToLower() == "sxt_nd")
                                    num = 40;
                                double temp_nm = 0;
                                if (double.TryParse(result, out temp_nm))
                                {
                                    result = (temp_nm / num).ToString();
                                }

                                result = "'" + temp_sign + result;
                            }
                            else
                            {
                                result = "'" + result;
                            }
                        }
                    }
                }
                else
                {
                    result = rowData.ObrValue.ToString().Trim().ToLower();

                    //转换为whonet标准结果
                    if (result == "pos" || result == "positive" || result.Contains("阳"))
                    {
                        result = "+";
                    }
                    else if (result == "neg" || result == "negtive" || result.Contains("阴"))
                    {
                        result = "-";
                    }
                    else if (result == "敏感" || result == "s")
                    {
                        result = "S";
                    }
                    else if (result == "耐药" || result == "r")
                    {
                        result = "R";
                    }
                    else if (result == "中介" || result == "i")
                    {
                        result = "I";
                    }


                    if (who_code.ToLower() == "mrsa")//不推导mrsa
                    {
                        if (curr_datarow.Table.Columns.Contains("MRSA"))
                        {
                            curr_datarow["MRSA"] = result;
                        }
                    }
                    //else if (
                    //            (ConfigurationManager.AppSettings["mrsa_deduction"] == "y"
                    //            ||
                    //             ConfigurationManager.AppSettings["mrsa_deduction"] == null
                    //            ||
                    //             ConfigurationManager.AppSettings["mrsa_deduction"] == string.Empty
                    //            )
                    //        && (who_code.ToLower() == "oxa" || who_code.ToLower() == "fox")
                    //    )//对 苯唑西林 或 头孢西丁 鉴定为耐药则判定MRSA为阳性
                    //{
                    //    if (result == "R")
                    //    {
                    //        if (curr_datarow.Table.Columns.Contains("MRSA"))
                    //        {
                    //            curr_datarow["MRSA"] = "+";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (curr_datarow.Table.Columns.Contains("MRSA"))
                    //        {
                    //            curr_datarow["MRSA"] = "-";
                    //        }
                    //    }
                    //}
                    else if (who_code.ToLower() == "van")//对万古霉素鉴定为中介或耐药则判定VRE为阳性
                    {
                        if (result == "R" || result == "I")
                        {
                            if (curr_datarow.Table.Columns.Contains("VRE"))
                            {
                                curr_datarow["VRE"] = "+";
                            }
                        }
                        else
                        {
                            if (curr_datarow.Table.Columns.Contains("VRE"))
                            {
                                curr_datarow["VRE"] = "-";
                            }
                        }
                    }
                }

                if (who_code.ToLower() == "esbl"
                    || who_code.ToLower() == "beta_lact"
                    || who_code.ToLower() == "mrsa")
                {
                    curr_datarow[who_code] = result;
                }
                else if (who_code.ToLower() == "icr")
                {
                    if (curr_datarow.Table.Columns.Contains("MLS_DTEST"))
                    {
                        curr_datarow["MLS_DTEST"] = result;
                    }
                }
                else if (whodata.Columns.Contains(who_code))
                {
                    if (!string.IsNullOrEmpty(who_code) && who_code == "GEH_NM")
                    {
                        if (!string.IsNullOrEmpty(result) && result == "'SYN-S")
                        {
                            curr_datarow[who_code] = "-";
                        }
                        else if (!string.IsNullOrEmpty(result) && result == "'SYN-R")
                        {
                            curr_datarow[who_code] = "+";
                        }
                        else
                        {
                            curr_datarow[who_code] = result;
                        }
                    }
                    else
                    {
                        curr_datarow[who_code] = result;
                    }
                }
                //}
                //else
                //{
                //if (who_code.ToLower() == "esbl"
                //     || who_code.ToLower() == "beta_lact"
                //     || who_code.ToLower() == "mrsa")
                //{
                //    curr_datarow[who_code] = result;
                //}
                //else if (whodata.Columns.Contains(who_code))
                //{
                //    curr_datarow[who_code] = result;
                //}
                //}

                if (currRowIndex == listOrigin.Count - 1)
                {
                    whodata.Rows.Add(curr_datarow);
                }
            }
            if (whodata.Rows.Count > 0)
            {
                whodata.Rows[0]["AGE"] = string.Format("{0}", whodata.Rows[0]["AGE"]);
                whodata.Rows[0]["SPEC_NUM"] = string.Format("{0}", whodata.Rows[0]["SPEC_NUM"]);
            }

            return whodata;
        }

        /// <summary>
        /// 去掉0单位
        /// 20Y0M0D0H0m -> 20Y
        /// </summary>
        /// <param name="age_value"></param>
        /// <returns></returns>
        public static string TrimZeroValue(string ageValue)
        {
            if (ageValue == null || ageValue == string.Empty)
            {
                return ageValue;
            }

            if (ageValue == "0Y0M0D0H0m" || ageValue == "0Y0M0D0H0I")
            {
                return "0Y";
            }

            string newValue = string.Empty;
            string ageValue2 = ageValue;

            ageValue2 = ageValue2.Trim(null);

            string strYear = ageValue2.Split('Y')[0];
            string right = ageValue2.Split('Y')[1];

            string strMonth = right.Split('M')[0];
            right = ageValue2.Split('M')[1];

            string strDay = right.Split('D')[0];
            right = ageValue2.Split('D')[1];

            string strHour = right.Split('H')[0];
            right = ageValue2.Split('H')[1];

            //string strMinute = right.Split('m')[0];

            string strMinute = string.Empty;

            if (right.Contains("m"))
            {
                strMinute = right.Split('m')[0];
            }
            else
            {
                strMinute = right.Split('I')[0];
            }

            if (strYear != "0")
            {
                newValue = strYear + "Y";
            }

            if (strMonth != "0")
            {
                newValue += strMonth + "M";
            }

            if (strDay != "0")
            {
                newValue += strDay + "D";
            }

            if (strHour != "0")
            {
                newValue += strHour + "H";
            }

            if (strMinute != "0")
            {
                newValue += strMinute + "I";
            }

            return newValue;
        }

        /// <summary>
        /// 根据原始数据获取数据中有多少个抗生素
        /// </summary>
        /// <param name="tableOrigin"></param>
        /// <returns></returns>
        List<string> GetWhoCode(List<EntityWhonet> listOrigin)
        {
            List<string> list = new List<string>();
            foreach (EntityWhonet whonet in listOrigin)
            {
                string anti_who_no = whonet.AntWhoNo;
                if (!list.Contains(anti_who_no))
                {
                    list.Add(anti_who_no);
                }
            }
            return list;
        }
        
  
        private void chkRemoveLogical_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemoveLogical.Checked)
            {
                chkRemoveLogicalExport.Checked = true;
            }
            else
            {
                chkRemoveLogicalExport.Checked = false;
            }
        }
              /// <summary>
        /// 把资源中的who字典文件释放到磁盘中
        /// </summary>
        private void ExtractWhoDict()
        {

            FileInfo fi1 = new FileInfo(dictFileNameDrug);
            if (fi1.Exists)
            {
                fi1.Attributes = FileAttributes.Normal;
                fi1.Delete();
            }


            using (FileStream fs = new FileStream(dictFileNameDrug, FileMode.Create))
            {
                fs.Write(Resource1.WhoDict, 0, Resource1.WhoDict.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// 读取字典到内存
        /// </summary>
        private void ReadDictToMemory()
        {
            string connStr1 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + Application.StartupPath + "\\" + dictFileNameDrug + "';Persist Security Info=False;Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

            SqlHelper helper = new SqlHelper(connStr1, EnumDbDriver.Oledb);
            tableAntibio = helper.GetTable("select * from [DRGLSTU$]", "DRGLSTU");
            tableBac = helper.GetTable("select * from [ORGLISTI$]", "ORGLISTI");
        }
        /// <summary>
        /// 更新细菌信息
        /// </summary>
        private void UpdateBac()
        {
            //获取没有设置who码的药敏信息
            List<EntityDicMicBacteria> listBac = CacheClient.GetCache<EntityDicMicBacteria>().FindAll(w => string.IsNullOrEmpty(w.BacWhoNo));

            int countBacUpdated = 0;
            foreach (EntityDicMicBacteria rowNullBac in listBac)
            {
                string bac_ename = rowNullBac.BacEname;
                if (bac_ename.Trim() != string.Empty)
                {
                    DataRow[] drs = tableBac.Select(string.Format("ORG_CLEAN = '{0}' or ORGANISM = '{0}'", bac_ename));
                    if (drs.Length > 0)
                    {
                        string bac_who_no = drs[0]["ORG"].ToString();
                        rowNullBac.BacWhoNo = bac_who_no;
                        EntityRequest requet = new EntityRequest();
                        requet.SetRequestValue(rowNullBac);
                        if (new ProxyWhonet().Service.UpdateBac(requet))
                        {
                            countBacUpdated += 1;
                        }
                    }
                }
            }

            MessageBox.Show(string.Format("本次更新细菌who码{0}个，还有{1}不能设置，请手工设置", countBacUpdated, listBac.Count - countBacUpdated));
        }
        /// <summary>
        /// 更新药敏信息
        /// </summary>
        private void UpdateAnti()
        {
            //获取没有设置who码的药敏信息
            List<EntityDicMicAntibio> listAnti = CacheClient.GetCache<EntityDicMicAntibio>().FindAll(w => string.IsNullOrEmpty(w.AntWhoNo));
            int countAllergyUpdated = 0;
            foreach (EntityDicMicAntibio antiNoWho in listAnti)
            {
                string anti_ename = antiNoWho.AntEname;
                if (anti_ename.Trim() != string.Empty)
                {
                    DataRow[] drs = tableAntibio.Select(string.Format("ANTIBIOTIC = '{0}'", anti_ename));
                    if (drs.Length > 0)
                    {
                        string anti_who_no = drs[0]["WHON5_CODE"].ToString();
                        antiNoWho.AntWhoNo = anti_who_no;
                        EntityRequest requet = new EntityRequest();
                        requet.SetRequestValue(antiNoWho);
                        if (new ProxyWhonet().Service.UpdateAnti(requet))
                        {
                            countAllergyUpdated += 1;
                        }
                    }
                }
            }

            MessageBox.Show(string.Format("本次更新抗生素who码{0}个，还有{1}不能设置，请手工设置", countAllergyUpdated, listAnti.Count - countAllergyUpdated));
        }

        private void rgExportType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.rgExportType.EditValue?.ToString() == "1")
            {
                chkRemoveLogical.Enabled = true;
            }
            else
            {
                chkRemoveLogical.Enabled = false;
            }
        }
    }
}
