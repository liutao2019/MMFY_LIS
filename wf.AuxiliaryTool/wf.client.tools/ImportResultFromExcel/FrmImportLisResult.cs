using dcl.client.frame;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NPOI.XSSF.UserModel;
using dcl.entity;
using dcl.client.wcf;
using DevExpress.XtraGrid.Columns;
using dcl.client.cache;

namespace dcl.client.tools
{
    public partial class FrmImportLisResult : FrmCommon
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PatDate
        {
            get
            {
                return (DateTime)this.dtBegin.EditValue;
            }
            set
            {
                dtBegin.EditValue = value;
            }
        }

        public DateTime PatDateEnd
        {
            get
            {
                return (DateTime)this.dtEnd.EditValue;
            }
            set
            {
                dtEnd.EditValue = value;
            }
        }


        List<EntityDicCombine> filterList = new List<EntityDicCombine>();
        List<EntityDicCombine> listComb;
        List<EntityDicCombineDetail> listCombDetail;

        public FrmImportLisResult()
        {
            InitializeComponent();
            this.Load += FrmImportLisResult_Load;
        }

        private void FrmImportLisResult_Load(object sender, EventArgs e)
        {
            PatDate = DateTime.Now.Date;
            PatDateEnd = DateTime.Now.Date;

            dtBeginDate.EditValue = DateTime.Now.Date;
            dtEndDate.EditValue = DateTime.Now.Date;

            listComb = CacheClient.GetCache<EntityDicCombine>();
            listCombDetail = CacheClient.GetCache<EntityDicCombineDetail>();

            selectDicInstrument1.SetFilter(i => i.ItrLabId == "10062");

            selectDict_Instrmt1.onAfterSelected += SelectDict_Instrmt1_onAfterSelected;
            cbImportSource.SelectedIndexChanged += CbImportSource_SelectedIndexChanged;

            teCombineName.TextChanged += TeCombineName_TextChanged;
        }

        #region Page0

        /// <summary>
        /// 医院单位过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbImportSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empdept = this.cbImportSource.Text;
            if (string.IsNullOrEmpty(empdept))
            {
                string newFilter = string.Empty;
                gvData0.ActiveFilterString = newFilter;
            }
            else
            {
                string newFilter = string.Format("出生医院='{0}'", empdept);
                gvData0.ActiveFilterString = newFilter;
            }
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport0_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx|(*.csv)|*.csv";
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                gcData0.DataSource = null;
                cbImportSource.Properties.Items.Clear();
                DataTable result = ReadExcel(open.FileName);
                if (result == null || result.Rows.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("未成功读取Excel数据，请检查文件格式！");
                }
                gcData0.DataSource = result;
                gvData0.BestFitColumns();

                if (result != null && result.Columns.Contains("出生医院"))
                {
                    cbImportSource.Properties.Items.Add("");
                    foreach (DataRow dr in result.Rows)
                    {
                        string value = dr["出生医院"].ToString();
                        if (!cbImportSource.Properties.Items.Contains(value))
                        {
                            cbImportSource.Properties.Items.Add(value);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave0_Click(object sender, EventArgs e)
        {
            DataTable dt = gcData0.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("无可保存的数据，请先进行数据导入！");
                return;
            }

            DataRow[] rows = dt.Select(this.gvData0.RowFilter);
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in rows)
            {
                newdt.ImportRow(dr);
            }

            if (newdt.Rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("无可保存的数据，请检查！");
                return;
            }

            List<EntityDicItmItem> CacheItems = (new ProxyCacheData().Service.GetCacheData("EntityDicItmItem")).GetResult() as List<EntityDicItmItem>;//缓存中的项目
            List<EntityDicItmItem> ExcelItems = new List<EntityDicItmItem>();//存储Excel文件中含有的项目
            List<EntityObrResultTestSeqVer> SaveItems = new List<EntityObrResultTestSeqVer>();//最终需要保存的数据

            ExcelItems = GetFromExcel(CacheItems, newdt);//从Excel中取出有多少项目

            if (ExcelItems.Count == 0)
            {
                lis.client.control.MessageDialog.Show("未从Excel数据中找到检验项目列，请检查文件格式！");
                return;
            }

            try
            {

                foreach (DataRow dr in newdt.Rows)
                {
                    string TestSeq = dr["实验编号"].ToString();
                    EntityObrResultTestSeqVer qc = new EntityObrResultTestSeqVer();
                    qc.TestSeq = TestSeq;
                    qc.Obrs = new List<EntityObrResult>();
                    foreach (EntityDicItmItem item in ExcelItems)
                    {
                        string columnname = GetColumnName(item.ItmEcode, newdt);
                        string itemvalue = dr[columnname].ToString();
                        if (!string.IsNullOrEmpty(itemvalue))
                        {
                            //项目有结果才保存
                            EntityObrResult obr = new EntityObrResult();
                            obr.TestSeq = TestSeq;
                            obr.ItmId = item.ItmId;
                            obr.ItmEname = item.ItmEcode;
                            obr.ObrValue = itemvalue;
                            qc.Obrs.Add(obr);
                        }
                    }
                    if (newdt.Columns.Contains("ms_jgpd"))
                    {
                        qc.RepComment = dr["ms_jgpd"].ToString();//处理意见
                    }
                    if (newdt.Columns.Contains("ms_ms"))
                    {
                        qc.Remark = dr["ms_ms"].ToString();//备注
                    }
                    SaveItems.Add(qc);
                }
                if (SaveItems.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("未能获取可以保存的数据，请检查数据！");
                    return;
                }

                //现在获取的SaveItems仍包含不存在的实验序号，但剩余工作都交给服务器完成
                string ErrMsg;
                List<List<EntityObrResultTestSeqVer>> lst = GetBlockList<EntityObrResultTestSeqVer>(SaveItems, 30);
                ProxyObrResult result = new ProxyObrResult();
                foreach (List<EntityObrResultTestSeqVer> qc in lst)
                {
                    result.Service.SaveobrresultbyTestSeq(qc, out ErrMsg);
                }
                lis.client.control.MessageDialog.Show("保存成功！");

            }
            catch (Exception ex)
            {

            }
        }


        public List<List<T>> GetBlockList<T>(List<T> list, int blockSize = 10)
        {
            List<List<T>> result = new List<List<T>>();
            var temp = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                temp.Add(list[i]);
                if ((i + 1) % blockSize == 0 && i > 0)
                {
                    result.Add(temp);
                    temp = new List<T>();
                }
                if (i == list.Count - 1)
                {
                    result.Add(temp);
                }
            }
            return result;
        }

        #endregion

        #region  Page1

        ProxyResultTemp proxyResultTemp = new ProxyResultTemp();

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx|(*.csv)|*.csv";
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {

                gcData.DataSource = null;
                gvData.Columns.Clear();
                DataTable result = ReadExcel(open.FileName);
                if (result == null || result.Rows.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("无数据！");
                }
                gcData.DataSource = result;
                gvData.BestFitColumns();
            }
        }
  

        #region NPOI读取Excel

        private DataTable ReadExcel(string FilePath)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;

            IWorkbook workbook = null;
            FileStream fs = null;

            try
            {
                fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                if (FilePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);//XSSFWorkbook
                else if (FilePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                sheet = workbook.GetSheetAt(0);

                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        cell.SetCellType(CellType.String);
                        if (cell != null)
                        {                      
                            string cellValue = cell.StringCellValue;
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                    }
                    startRow = sheet.FirstRowNum + 1;

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("未成功读取Excel数据，请转换后再导入！" + ex.Message);
                return null;
            }
        }


        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 保存前判断

            if (selectDicCombine1.selectRow == null)
            {
                lis.client.control.MessageDialog.Show("检验组合不能为空");

                selectDicCombine1.Focus();
                return;
            }
            if (selectDicInstrument1.selectRow == null)
            {
                lis.client.control.MessageDialog.Show("仪器不能为空");

                selectInstrument.Focus();
                return;
            }
            #endregion

            try
            {
                bool operType = true;

                #region 获取选中数据
                int[] selectedRow = gvData.GetSelectedRows();
                DataTable dt = gcData.DataSource as DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("无可保存的数据，请先进行数据导入！");
                    return;
                }
                DataTable selectedTb = dt.Clone();
                foreach (int rn in selectedRow)
                {
                    selectedTb.ImportRow(gvData.GetDataRow(rn));
                }
                List<EntityDicItmItem> CacheItems = (new ProxyCacheData().Service.GetCacheData("EntityDicItmItem")).GetResult() as List<EntityDicItmItem>;//缓存中的项目
                List<EntityDicCombineDetail> cacheCombineDetail = CacheClient.GetCache<EntityDicCombineDetail>();
                List<EntityDicCombine> cacheCombines = CacheClient.GetCache<EntityDicCombine>(); // (new ProxyCacheData().Service.GetCacheData("EntityDicCombine")).GetResult() as List<EntityDicCombine>;  // 缓存组合

                //检验组合
                EntityDicCombine combine = cacheCombines.Find(o => o.ComId == selectDicCombine1.selectRow.ComId);
                //组合明细项目
                List<EntityDicCombineDetail> combineList = cacheCombineDetail.FindAll(o => o.ComId == selectDicCombine1.selectRow.ComId);
                List<EntityDicItmItem> combineItems = new List<EntityDicItmItem>();// cacheCombines.FindAll(o => o.ComId == combineList.

                combineList.ForEach(detail => { combineItems.Add(CacheItems.Find(o => o.ItmId == detail.ComItmId)); });


                EntityPidReportMain pidReportMain = new EntityPidReportMain();

                EntityRemoteCallClientInfo remoteCaller = new EntityRemoteCallClientInfo();

                EntityQcResultList resultList = new EntityQcResultList();

                string log = string.Empty; //导入结果日志

                //待导入的组合项目数据
                List<EntityDicItmItem> itemList = GetFromExcel(combineItems, selectedTb);

                if (itemList.Count == 0)
                {
                    DialogResult dialogResult = lis.client.control.MessageDialog.Show("未从Excel数据中匹配到检验项目列，[是]继续登记，[否]退出", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    operType = false;
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                #endregion

                //操作信息
                remoteCaller.IPAddress = UserInfo.ip;
                remoteCaller.LoginID = UserInfo.loginID;
                remoteCaller.Remarks = "新筛外院导入登记";



                //组合项目列表
                EntityPidReportDetail repDetail = new EntityPidReportDetail();
                repDetail.ComId = combine.ComId;
                repDetail.PatComName = combine.ComName;
                repDetail.OrderPrice = combine.ComPrice.ToString();
                repDetail.SortNo = 0;

                List<EntityPidReportDetail> listRepDetail = new List<EntityPidReportDetail>();
                listRepDetail.Add(repDetail);
                string strId = selectDicInstrument1.selectRow.ItrId;  //仪器ID
                DateTime dtToday = ServerDateTime.GetServerDateTime();

                //遍历选中每一行登记并导入结果
                foreach (DataRow dr in selectedTb.Rows)
                {
                    DateTime birthday = ParseDate(dr["出生日期"].ToString());
                    TimeSpan span = dtToday - birthday;
                    string year = (dtToday.Year - birthday.Year).ToString();
                    string month = (dtToday.Month - birthday.Month).ToString();
                    string day = (dtToday.Day - birthday.Day).ToString();

                    pidReportMain.RepItrId = strId;
                    pidReportMain.PidName = dr["母亲姓名"].ToString(); //姓名
                    pidReportMain.PidRemark = "合格";
                    pidReportMain.PidComName = combine.ComName;
                    pidReportMain.PidSamId = combine.ComSamId;  //标本ID
                    pidReportMain.SamName = combine.ComSamName; //标本名称
                    pidReportMain.RepSid = dr["实验编号"].ToString(); //样本号
                    pidReportMain.PidSrcId = "100009";  //来源：外院
                    pidReportMain.PidIdtId = "107";  //病人ID类型：门诊号
                    pidReportMain.PidInNo = "0000" + dr["采血卡号"].ToString(); //唯一号
                    pidReportMain.RepInputId = pidReportMain.PidInNo;   //输入ID
                    pidReportMain.PidSex = dr["性别"].ToString() == "男" ? "1" : "2";  //性别
                    pidReportMain.PidBirthday = birthday;   //出生日期
                    pidReportMain.PidAgeExp = string.Format("{0}Y{1}M{2}D{3}H{4}I", year, month, day, "0", "0");
                    pidReportMain.PidTel = dr["手机"].ToString();  //电话号码
                    pidReportMain.PidAddress = dr["地址"].ToString();
                    pidReportMain.PidEmail = dr["出生医院"].ToString();
                    pidReportMain.RepCheckUserId = UserInfo.loginID;  //录入者
                    pidReportMain.RepCtype = "0";   //报告类别
                    pidReportMain.RepSendFlag = 0;  //发送标志
                    pidReportMain.RepDiseaseFlag = 0; //传染病上传标志
                    pidReportMain.RepInDate = dtToday;
                    pidReportMain.SampApplyDate = dtToday;
                    pidReportMain.SampCheckDate = dtToday;
                    pidReportMain.SampCollectionDate = dtToday;
                    pidReportMain.SampCollectionDate = dtToday;
                    pidReportMain.SampReachDate = dtToday;
                    pidReportMain.SampReceiveDate = dtToday;
                    pidReportMain.SampSendDate = dtToday;
                    pidReportMain.UrgStatus = 0;
                    pidReportMain.UrgentCount = 0;
                    pidReportMain.UrgentMsgHandle = 0;
                    pidReportMain.MicReportFlag = 0;  //中期报告标志
                    pidReportMain.RepDrugfastFlag = 0;  //抗药标志
                    pidReportMain.RepInitialFlag = 0;   //初始标志
                    pidReportMain.PidIdentity = 0;   //身份
                    //pidReportMain.RepTempFlag = "0";  //备份标志
                    //pidReportMain.RepDangerFlag = false
                    pidReportMain.ListPidReportDetail = listRepDetail;

                    resultList.patient = pidReportMain;
                    resultList.listRepDetail = listRepDetail;
                    resultList.patient.ListPidReportDetail = listRepDetail;

                    if (itemList.Count > 0)
                    {
                        List<EntityObrResult> obrList = new List<EntityObrResult>();
                        //项目结果
                        foreach (EntityDicItmItem item in itemList)
                        {
                            string columnname = GetColumnName(item.ItmEcode, selectedTb);
                            string itemvalue = dr[columnname].ToString();
                            if (!string.IsNullOrEmpty(itemvalue))
                            {
                                //项目有结果才保存
                                EntityObrResult obr = new EntityObrResult();
                                obr.ItmId = item.ItmId;
                                obr.ItmEname = item.ItmEcode;
                                obr.ObrValue = itemvalue.StartsWith(".") ? "0" + itemvalue : itemvalue;
                                obrList.Add(obr);
                            }
                        }

                        resultList.listResulto = obrList;
                        resultList.listResultoNoFliter = obrList;
                    }

                    var opResult = new ProxyObrResult().Service.InsertPatCommonResult(remoteCaller, resultList);

                    if (opResult.Success)
                    {
                        if (operType)
                        {
                            log += pidReportMain.RepSid + " 导入成功\r\n";

                        }
                        else
                        {
                            log += pidReportMain.RepSid + " 登记成功\r\n";
                        }
                    }
                    else
                    {
                        log += pidReportMain.RepId + " 导入失败\r\n";
                    }
                }
                lis.client.control.MessageDialog.Show(log, "导入结果");
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("导入出错:" + ex.Message, "导入结果");
            }

            #region arch
            /*
            DataTable dt = gcData.DataSource as DataTable;

            if (dt == null || dt.Rows.Count == 0)   
            {
                lis.client.control.MessageDialog.Show("无可保存的数据，请先进行数据导入！");
                return;
            }

            DataRow[] rows = dt.Select(this.gvData.RowFilter);
            DataTable newdt = dt.Clone();
            foreach (DataRow dr in rows)
            {
                newdt.ImportRow(dr);
            }

            if (newdt.Rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("无可保存的数据，请检查！");
                return;
            }

            //List<EntityDicItmItem> CacheItems = (new ProxyCacheData().Service.GetCacheData("EntityDicItmItem")).GetResult() as List<EntityDicItmItem>;//缓存中的项目
            List<EntityDicItmItem> ExcelItems = new List<EntityDicItmItem>();//存储Excel文件中含有的项目
            List<EntityObrResultTestSeqVer> SaveItems = new List<EntityObrResultTestSeqVer>();//最终需要保存的数据

            ExcelItems = GetFromExcel(CacheItems, newdt);//从Excel中取出有多少项目

            if (ExcelItems.Count == 0)
            {
                lis.client.control.MessageDialog.Show("未从Excel数据中找到检验项目列，请检查文件格式！");
                return;
            }

            try
            {

                foreach (DataRow dr in newdt.Rows)
                {
                    string TestSeq = dr["实验编号"].ToString();
                    EntityObrResultTestSeqVer qc = new EntityObrResultTestSeqVer();
                    qc.TestSeq = TestSeq;
                    qc.Obrs = new List<EntityObrResult>();
                    foreach (EntityDicItmItem item in ExcelItems)
                    {
                        string columnname = GetColumnName(item.ItmEcode, newdt);
                        string itemvalue = dr[columnname].ToString();
                        if (!string.IsNullOrEmpty(itemvalue))
                        {
                            //项目有结果才保存
                            EntityObrResult obr = new EntityObrResult();
                            obr.TestSeq = TestSeq;
                            obr.ItmId = item.ItmId;
                            obr.ItmEname = item.ItmEcode;
                            obr.ObrValue = itemvalue;
                            qc.Obrs.Add(obr);
                        }
                    }
                    if (newdt.Columns.Contains("ms_jgpd"))
                    {
                        qc.RepComment = dr["ms_jgpd"].ToString();//处理意见
                    }
                    if (newdt.Columns.Contains("ms_ms"))
                    {
                        qc.Remark = dr["ms_ms"].ToString();//备注
                    }
                    SaveItems.Add(qc);
                }
                if (SaveItems.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("未能获取可以保存的数据，请检查数据！");
                    return;
                }

                //现在获取的SaveItems仍包含不存在的实验序号，但剩余工作都交给服务器完成
                string ErrMsg;
                List<List<EntityObrResultTestSeqVer>> lst = GetBlockList(SaveItems, 30);
                ProxyObrResult result = new ProxyObrResult();
                foreach (List<EntityObrResultTestSeqVer> qc in lst)
                {
                    result.Service.SaveobrresultbyTestSeq(qc, out ErrMsg);
                }
                lis.client.control.MessageDialog.Show("保存成功！");

            }
            catch (Exception ex)
            {

            }
            */
            #endregion
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private DateTime ParseDate(string date)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowLeadingWhite, out dateTime))
            {
                return dateTime;
            }
            else if (DateTime.TryParseExact(date, "MM-d-yy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowLeadingWhite, out dateTime))
            {
                return dateTime;
            }
            else if (DateTime.TryParseExact(date, "MM-dd-yy", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowLeadingWhite, out dateTime))
            {
                return dateTime;
            }
            else
            {
                DevExpress.XtraBars.Alerter.AlertControl alert = new DevExpress.XtraBars.Alerter.AlertControl();
                alert.Show(this, new DevExpress.XtraBars.Alerter.AlertInfo("导入出错", "日期格式转换出错"));
                return dateTime;
            }
        }

        private string GetColumnName(string ItmEcode, DataTable newdt)
        {
            string columnname = "";
            foreach (DataColumn co in newdt.Columns)
            {
                if (ItmEcode.ToLower().Contains("(c0+c2+c3+c16+c18:1)/ci"))
                {
                    return "(0+2+3+16+18:1)/Cit";
                }
                else if (ItmEcode.Contains("α-THAL"))
                {
                    return "α-THA";
                }
                else if (ItmEcode.Contains("β-THAL"))
                {
                    return "β-THA";
                }

                if (co.ColumnName.ToLower() == ItmEcode.ToLower())
                {
                    columnname = co.ColumnName;
                    break;
                }
            }
            return columnname;
        }

        /// <summary>
        /// 从Excel中取出检验项目的列集合
        /// </summary>
        /// <param name="cacheItems"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<EntityDicItmItem> GetFromExcel(List<EntityDicItmItem> cacheItems, DataTable dt)
        {
            List<EntityDicItmItem> result = new List<EntityDicItmItem>();
            foreach (DataColumn col in dt.Columns)
            {
                EntityDicItmItem item = null;
                if (col.ColumnName.Contains("(0+2+3+16+18:1)/Cit"))
                {
                    item = cacheItems.Find(w => w.ItmEcode.ToLower().Contains("(c0+c2+c3+c16+c18:1)/ci"));
                }
                else if (col.ColumnName.Contains("α-THA"))
                {
                    item = cacheItems.Find(w => w.ItmEcode.Contains("α-THAL"));
                }
                else if (col.ColumnName.Contains("β-THA"))
                {
                    item = cacheItems.Find(w => w.ItmEcode.Contains("β-THAL"));
                }
                else
                {
                    item = cacheItems.Find(w => w.ItmEcode.ToLower() == col.ColumnName.ToLower());
                }
                if (item != null)
                    result.Add(item);

            }
            return result;
        }

        /// <summary>
        /// 选择仪器事件-过滤检验组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void selectDicInstrument1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            string itrId = this.selectDicInstrument1.selectRow.ItrId;

            ////缓存当前选中的仪器的组合,用于组合过滤
            List<EntityDicItrCombine> itrCombs = CacheClient.GetCache<EntityDicItrCombine>().FindAll(o => o.ItrId == itrId);

            List<EntityDicCombine> cacheCombines = CacheClient.GetCache<EntityDicCombine>();

            List<EntityDicCombine> combines = new List<EntityDicCombine>();

            if (itrCombs.Count > 0)
            {
                foreach (EntityDicItrCombine crow in itrCombs)
                {
                    combines.Add(cacheCombines.Find(o => o.ComId == crow.ComId));
                }

                this.selectDicCombine1.ClearSelect();

                this.selectDicCombine1.SetFilter(combines);

            }
        }
        #endregion

        #region Page2

        List<string> Model1 = new List<string> { "日期","样本号","姓名","性别","年龄", "出生日期", "标本", "采集日期", "申请科室", "病人编号",
                "MCV","MCH","Hb A","Hb A2","Hb F","异常带","α-地贫","β-地贫"};

        List<string> Model2 = new List<string> { "日期", "样本号", "姓名", "性别", "年龄", "出生日期", "联系方式", "申请科室",
                "申请医生","诊断","结果","报告日期","报告人"};

        List<string> Model3 = new List<string> { "日期", "样本号", "姓名", "种类", "病人编号", "申请科室" , "床号",
                "标本","结果","备注"};

        List<string> Model4 = new List<string> { "日期","样本号","姓名","性别","年龄", "出生日期", "标本", "采集日期", "申请科室", "病人编号",
                "MCV","MCH","Hb A","Hb A2","Hb F","Hb","异常带","α-地贫","β-地贫"};

        private void GenItrRelateDicCombine(string itrID)
        {
            List<EntityDicInstrument> dtDictInstrmt = CacheClient.GetCache<EntityDicInstrument>();
            List<EntityDicItrCombine> dt_dict_instrmt_com = CacheClient.GetCache<EntityDicItrCombine>();

            //查找所有"存储仪器"为当前仪器的仪器
            List<EntityDicInstrument> drsConItr = dtDictInstrmt.FindAll(i => i.ItrId == itrID);

            string sqlItrCombineIn = itrID;

            if (drsConItr.Count > 0)
            {
                foreach (EntityDicInstrument drItr in drsConItr)
                {
                    string itrid = drItr.ItrId.ToString();
                    sqlItrCombineIn += "," + itrid;
                }
            }

            List<EntityDicItrCombine> drs = (from x in dt_dict_instrmt_com where sqlItrCombineIn.Contains(x.ItrId) select x).ToList();

            string sqlPart = " 1=2 ";
            if (drs.Count > 0)
            {
                bool bNeedComma = false;
                sqlPart = string.Empty;
                foreach (EntityDicItrCombine dr in drs)
                {
                    if (dr.ComId.ToString() != string.Empty)
                    {
                        if (bNeedComma)
                        {
                            sqlPart += "," + dr.ComId.ToString();
                        }
                        else
                        {
                            sqlPart += dr.ComId.ToString();
                        }

                        bNeedComma = true;
                    }
                }
            }
            filterList = (from x in listComb where sqlPart.Contains(x.ComId) select x).ToList();
            lueItem.SetFilter(filterList);
        }


        private void SelectDict_Instrmt1_onAfterSelected(object sender, EventArgs e)
        {
            GenItrRelateDicCombine(selectDict_Instrmt1.valueMember);
        }

        private void TeCombineName_TextChanged(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listPatient = listPatientTotal.FindAll(i => i.PidComName.Contains(teCombineName.Text.TrimEnd()));
            DataTable dt = BuildDataTable(listPatient);
            gcItrData.DataSource = dt;
            gvItrData.BestFitColumns();
        }

        List<EntityPidReportMain> listPatientTotal;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            gcItrData.DataSource = null;
            lstcolumns.Items.Clear();
            if (PatDate.Date > PatDateEnd.Date)
            {
                lis.client.control.MessageDialog.Show("检索起始日期不能大于结束日期！");
                return;
            }
            if ((PatDateEnd.Date - PatDate.Date).Days > 31)
            {
                lis.client.control.MessageDialog.Show("检索日期最长不能超过31天！");
                return;
            }

            if (string.IsNullOrEmpty(selectDict_Instrmt1.valueMember))
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }

            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.DateStart = DateTime.Parse(PatDate.ToString("yyyy-MM-dd 00:00:00"));
            patientQc.DateEnd = DateTime.Parse(PatDateEnd.ToString("yyyy-MM-dd 23:59:59"));
            patientQc.ListItrId = new List<string> { selectDict_Instrmt1.valueMember };
            try
            {
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                List<EntityPidReportMain> listPatient = proxy.Service.PatientQuery(patientQc);
                listPatientTotal = EntityManager<EntityPidReportMain>.ListClone(listPatient);

                if (!string.IsNullOrEmpty(teCombineName.Text))
                    listPatient = listPatient.FindAll(i => i.PidComName.Contains(teCombineName.Text.TrimEnd()));

                if (listPatient == null || listPatient.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("无数据！");
                    return;
                }
                DataTable dt = BuildDataTable(listPatient);
                gcItrData.DataSource = dt;
                gvItrData.BestFitColumns();
                Bindlstcolumns(dt);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("错误:" + ex.Message);
            }
        }

        private void Bindlstcolumns(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0)
                return;
            lstcolumns.Items.Add("全选");
            foreach (DataColumn col in dt.Columns)
            {
                lstcolumns.Items.Add(col.ColumnName);
            }
            for (int j = 0; j < lstcolumns.Items.Count; j++)
                lstcolumns.SetItemChecked(j, true);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsxExportOptionsEx op = new DevExpress.XtraPrinting.XlsxExportOptionsEx();
                    op.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gvItrData.OptionsPrint.PrintHeader = true;
                    gvItrData.OptionsPrint.AutoWidth = false;
                    gvItrData.ExportToXlsx(saveFileDialog.FileName, op);
                    lis.client.control.MessageDialog.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("导出Excel失败:" + ex.Message);
            }
        }

        private DataTable BuildDataTable(List<EntityPidReportMain> Patients)
        {
            List<string> ListPidInNo = new List<string>();
            foreach (EntityPidReportMain p in Patients)
            {
                ListPidInNo.Add(p.PidInNo);//PidInNo
            }

            List<string> ListDitmEcode = new List<string>();
            ListDitmEcode = new List<string> { "MCV","MCH","HB A","HB A2","HB", "HB F",
                    "Β", "A-A","HCG1"};


            DataTable dt = new DataTable();
            dt.Columns.Add("日期", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("性别", typeof(string));
            dt.Columns.Add("年龄", typeof(string));
            dt.Columns.Add("出生日期", typeof(string));
            dt.Columns.Add("标本", typeof(string));
            dt.Columns.Add("采集日期", typeof(string));
            dt.Columns.Add("申请科室", typeof(string));
            dt.Columns.Add("病人编号", typeof(string));
            dt.Columns.Add("MCV", typeof(string));
            dt.Columns.Add("MCH", typeof(string));
            dt.Columns.Add("Hb A", typeof(string));
            dt.Columns.Add("Hb A2", typeof(string));
            dt.Columns.Add("Hb F", typeof(string));
            dt.Columns.Add("Hb", typeof(string));
            dt.Columns.Add("异常带", typeof(string));
            dt.Columns.Add("α-地贫", typeof(string));
            dt.Columns.Add("β-地贫", typeof(string));
            dt.Columns.Add("样本号", typeof(string));
            dt.Columns.Add("种类", typeof(string));
            dt.Columns.Add("床号", typeof(string));
            dt.Columns.Add("结果", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            dt.Columns.Add("孕周", typeof(string));
            dt.Columns.Add("联系方式", typeof(string));
            dt.Columns.Add("申请医生", typeof(string));
            dt.Columns.Add("诊断", typeof(string));
            dt.Columns.Add("报告日期", typeof(string));
            dt.Columns.Add("报告人", typeof(string));
            dt.Columns.Add("检验组合", typeof(string));
            dt.Columns.Add("HCG", typeof(string));
            dt.Columns.Add("样本备注", typeof(string));

            string First = txtFirst.Text.Trim();
            try
            {
                ProxyObrResult proxyObrResult = new ProxyObrResult();
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListPidInNo = ListPidInNo;
                resultQc.ListDitmEcode = ListDitmEcode;
                //先一次查出所有需要查询的报告单结果
                List<EntityObrResult> listObrResult = proxyObrResult.Service.LisResultQuery(resultQc);
                foreach (EntityPidReportMain p in Patients)
                {
                    string pidInNo = p.PidInNo;
                    string MCV = "";
                    string MCH = "";
                    string Hb_A = "";
                    string Hb = "";
                    string Hb_A2 = "";
                    string Hb_F = "";
                    string β = "";
                    string α = "";
                    string HCG = "";
                    //由于需要多次匹配，先查出单个报告单的结果效率较高
                    List<EntityObrResult> results = listObrResult.FindAll(w => w.PidInNo == pidInNo);
                    if (results != null)
                    {
                        //匹配已有结果
                        MCV = results.Find(w => w.ItmEname == "MCV")?.ObrValue;
                        MCH = results.Find(w => w.ItmEname == "MCH")?.ObrValue;
                        Hb_A = results.Find(w => w.ItmEname == "HB A")?.ObrValue;
                        Hb_A2 = results.Find(w => w.ItmEname == "HB A2")?.ObrValue;
                        Hb = results.Find(w => w.ItmEname == "HGB")?.ObrValue;
                        Hb_F = results.Find(w => w.ItmEname == "HB F")?.ObrValue;
                        β = results.Find(w => w.ItmEname == "Β")?.ObrValue;
                        α = results.Find(w => w.ItmEname == "A-A")?.ObrValue;
                        HCG = results.Find(w => w.ItmEname == "HCG1")?.ObrValue;
                    }

                    DataRow dr = dt.NewRow();
                    dr["日期"] = p.RepInDate?.ToString("yyyy-MM-dd");
                    dr["姓名"] = p.PidName;
                    dr["性别"] = p.PidSexExp;
                    dr["年龄"] = AgeConvertRule(p.PidAgeExp);
                    dr["出生日期"] = p.PidBirthday?.ToString("yyyy-MM-dd");
                    dr["标本"] = p.SamName;
                    dr["采集日期"] = p.SampCollectionDate;
                    dr["申请科室"] = p.PidDeptName;
                    dr["病人编号"] = p.PidInNo;
                    dr["样本号"] = First + string.Format("{0:d4}", int.Parse(p.RepSid));
                    dr["种类"] = p.PidSrcId == "107" ? "门诊" : p.PidSrcId == "108" ? "住院" :
                        p.PidSrcId == "109" ? "体检" : "其它";
                    dr["床号"] = p.PidBedNo;
                    dr["孕周"] = p.PidPreWeek;
                    dr["联系方式"] = p.PidTel;
                    dr["申请医生"] = p.PidDocName;//DoctorName
                    dr["诊断"] = p.PidDiag;
                    dr["检验组合"] = p.PidComName;
                    dr["样本备注"] = p.SampRemark;


                    dr["MCV"] = MCV;
                    dr["MCH"] = MCH;
                    dr["Hb A"] = Hb_A;
                    dr["Hb"] = Hb;
                    dr["Hb A2"] = Hb_A2;
                    dr["Hb F"] = Hb_F;
                    dr["α-地贫"] = α;
                    dr["β-地贫"] = β;
                    dr["HCG"] = HCG;
                    //报告日期和报告人 遗传要求不导出值，只给出空白列给他们填写即可
                    //dr["报告日期"] = p.RepAuditDate?.ToString("yyyy-MM-dd HH:mm:ss");
                    //dr["报告人"] = p.BgName;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        private void lstcolumns_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            DataTable dt = gcItrData.DataSource as DataTable;
            if (lstcolumns.GetItemText(e.Index) == "全选")
            {
                if (lstcolumns.GetItemChecked(0))
                {
                    //设置索引为index的项为选中状态
                    for (int i = 1; i < lstcolumns.Items.Count; i++)
                    {
                        lstcolumns.SetItemChecked(i, true);
                        SetColumnsVisible(lstcolumns.Items[i].Value.ToString(), true);

                    }
                    return;
                }
                else
                {
                    //设置索引为index的项为选中状态
                    for (int i = 1; i < lstcolumns.Items.Count; i++)
                    {
                        lstcolumns.SetItemChecked(i, false);
                        SetColumnsVisible(lstcolumns.Items[i].Value.ToString(), false);
                    }
                    return;//return 解决了 CheckOnClick 检测单击或者双击延迟的问题
                }

            }
            else
            {
                string value = lstcolumns.GetItemText(e.Index);
                if (lstcolumns.GetItemCheckState(e.Index) == CheckState.Checked)
                    SetColumnsVisible(value, true);
                else
                    SetColumnsVisible(value, false);
            }
        }

        private void SetColumnsVisible(string txt, bool v2)
        {
            foreach (GridColumn col in gvItrData.Columns)
            {
                if (col.FieldName == txt)
                {
                    col.Visible = v2;
                    if (v2)
                        col.VisibleIndex = gvItrData.VisibleColumns.Count;
                }
            }
        }

        private void radioGroup1_Properties_EditValueChanged(object sender, EventArgs e)
        {
            int x = int.Parse(ModelChoosed.EditValue?.ToString());
            List<string> lst = new List<string>();

            if (x == 1)
            {
                lst = Model1;
            }
            else if (x == 2)
            {
                lst = Model2;
            }
            else if (x == 3)
            {
                lst = Model3;
            }
            else if (x == 4)
            {
                lst = Model4;
            }

            for (int i = 1; i < lstcolumns.Items.Count; i++)
            {
                if (lst.Count == 0 || lst.Contains(lstcolumns.Items[i].Value.ToString()))
                {
                    lstcolumns.SetItemChecked(i, true);
                    SetColumnsVisible(lstcolumns.Items[i].Value.ToString(), true);
                }
                else
                {
                    lstcolumns.SetItemChecked(i, false);
                    SetColumnsVisible(lstcolumns.Items[i].Value.ToString(), false);
                }
            }

        }

        #region 年龄格式
        private string AgeConvertRule(string pidAgeExp)
        {
            string strAge = pidAgeExp.ToLower();


            string strY = "0";
            string strM = "0";
            string strD = "0";
            string strH = "0";
            string strI = "0";

            if (strAge.IndexOf("y") > 0)
            {
                #region surfix y
                strY = strAge.Substring(0, strAge.IndexOf("y"));
                if (strAge.IndexOf("m") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (strAge.IndexOf("y") == 0 && strAge.Length > 0)
            {
                #region prefix y
                if (strAge.IndexOf("m") > 0)
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                    if (strAge.IndexOf("d") > 0)
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                        if (strAge.IndexOf("h") > 0)
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                            if (strAge.IndexOf("i") > 0)
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                            }
                            else
                            {
                                strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                            }
                        }
                        else
                        {
                            strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                        }
                    }
                    else
                    {
                        strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                    }
                }
                else
                {
                    strY = strAge.Substring(strAge.IndexOf("y") + 1, strAge.Length - strAge.IndexOf("y") - 1);
                }
                #endregion

            }
            else if (strAge.IndexOf("m") > 0)
            {
                strM = strAge.Substring(strAge.IndexOf("y") + 1, strAge.IndexOf("m") - strAge.IndexOf("y") - 1);
                if (strAge.IndexOf("d") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        }
                    }
                }
            }
            else if (strAge.IndexOf("m") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("d") > 0)
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                    if (strAge.IndexOf("h") > 0)
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                        if (strAge.IndexOf("i") > 0)
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                            strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                        }
                        else
                        {
                            strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                        }
                    }
                    else
                    {
                        strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                    }
                }
                else
                {
                    strM = strAge.Substring(strAge.IndexOf("m") + 1, strAge.Length - strAge.IndexOf("m") - 1);
                }
            }
            else if (strAge.IndexOf("d") > 0)
            {
                strD = strAge.Substring(strAge.IndexOf("m") + 1, strAge.IndexOf("d") - strAge.IndexOf("m") - 1);
                if (strAge.IndexOf("h") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    }
                }
            }
            else if (strAge.IndexOf("d") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("h") > 0)
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                    if (strAge.IndexOf("i") > 0)
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                        strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                    }
                    else
                    {
                        strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                    }
                }
                else
                {
                    strD = strAge.Substring(strAge.IndexOf("d") + 1, strAge.Length - strAge.IndexOf("d") - 1);
                }
            }
            else if (strAge.IndexOf("h") > 0)
            {
                strH = strAge.Substring(strAge.IndexOf("d") + 1, strAge.IndexOf("h") - strAge.IndexOf("d") - 1);
                if (strAge.IndexOf("i") > 0)
                {
                    strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("h") == 0 && strAge.Length > 0)
            {
                if (strAge.IndexOf("i") > 0)
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
                    strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
                }
                else
                {
                    strH = strAge.Substring(strAge.IndexOf("h") + 1, strAge.Length - strAge.IndexOf("h") - 1);
                }
            }
            else if (strAge.IndexOf("i") > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("h") + 1, strAge.IndexOf("i") - strAge.IndexOf("h") - 1);
            }
            else if (strAge.IndexOf("i") == 0 && strAge.Length > 0)
            {
                strI = strAge.Substring(strAge.IndexOf("i") + 1, strAge.Length - strAge.IndexOf("i") - 1);
            }
            else
            {
                return string.Empty;
            }

            string strOutAge = string.Empty;
            try
            {
                strY = Convert.ToInt32(Convert.ToDecimal(strY)).ToString();
                strM = Convert.ToInt32(Convert.ToDecimal(strM)).ToString();
                strD = Convert.ToInt32(Convert.ToDecimal(strD)).ToString();
                strH = Convert.ToInt32(Convert.ToDecimal(strH)).ToString();
                strI = Convert.ToInt32(Convert.ToDecimal(strI)).ToString();
                if (int.Parse(strY) > 0)
                    strOutAge += strY + "岁";
                if (int.Parse(strM) > 0)
                    strOutAge += strM + "月";
                if (int.Parse(strD) > 0)
                    strOutAge += strD + "天";
                if (int.Parse(strH) > 0)
                    strOutAge += strH + "小时";
                if (string.IsNullOrEmpty(strOutAge))
                    strOutAge += strI + "分钟";
                return strOutAge;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #endregion

        #region Page3

        /// <summary>
        /// 仪器选中后过滤项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectInstrument_onAfterSelected(object sender, EventArgs e)
        {
            FilterCombineByItrID(selectInstrument.valueMember);
        }

        /// <summary>
        /// 仪器选中过滤项目方法
        /// </summary>
        /// <param name="itrID"></param>
        private void FilterCombineByItrID(string itrID)
        {
            List<EntityDicInstrument> dtDictInstrmt = CacheClient.GetCache<EntityDicInstrument>();
            List<EntityDicItrCombine> dt_dict_instrmt_com = CacheClient.GetCache<EntityDicItrCombine>();

            //查找所有"存储仪器"为当前仪器的仪器
            List<EntityDicInstrument> drsConItr = dtDictInstrmt.FindAll(i => i.ItrId == itrID);

            string sqlItrCombineIn = itrID;

            if (drsConItr.Count > 0)
            {
                foreach (EntityDicInstrument drItr in drsConItr)
                {
                    string itrid = drItr.ItrId.ToString();
                    sqlItrCombineIn += "," + itrid;
                }
            }

            List<EntityDicItrCombine> drs = (from x in dt_dict_instrmt_com where sqlItrCombineIn.Contains(x.ItrId) select x).ToList();

            string sqlPart = " 1=2 ";
            if (drs.Count > 0)
            {
                bool bNeedComma = false;
                sqlPart = string.Empty;
                foreach (EntityDicItrCombine dr in drs)
                {
                    if (dr.ComId.ToString() != string.Empty)
                    {
                        if (bNeedComma)
                        {
                            sqlPart += "," + dr.ComId.ToString();
                        }
                        else
                        {
                            sqlPart += dr.ComId.ToString();
                        }

                        bNeedComma = true;
                    }
                }
            }
            filterList = (from x in listComb where sqlPart.Contains(x.ComId) select x).ToList();
            lueItem.SetFilter(filterList);
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            gcltrData1.DataSource = null;
            DateTime beginDate = ((DateTime)dtBeginDate.EditValue).Date;
            DateTime endDate = ((DateTime)dtEndDate.EditValue).Date;

            if (beginDate > endDate)
            {
                lis.client.control.MessageDialog.Show("检索起始日期不能大于结束日期！");
                return;
            }
            if ((endDate - beginDate).Days > 31)
            {
                lis.client.control.MessageDialog.Show("检索日期最长不能超过31天！");
                return;
            }

            if (string.IsNullOrEmpty(selectInstrument.valueMember) || string.IsNullOrEmpty(selectInstrument.valueMember))
            {
                lis.client.control.MessageDialog.Show("请选择仪器和组合项目！");
                return;
            }

            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.DateStart = DateTime.Parse(beginDate.ToString("yyyy-MM-dd 00:00:00"));
            patientQc.DateEnd = DateTime.Parse(endDate.ToString("yyyy-MM-dd 23:59:59"));
            patientQc.ListItrId = new List<string> { selectInstrument.valueMember };
            patientQc.ComId = selectCombine.valueMember;

            try
            {
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                List<EntityPidReportMain> listPatient = proxy.Service.PatientQuery(patientQc);
                listPatientTotal = EntityManager<EntityPidReportMain>.ListClone(listPatient);

                if (listPatient == null || listPatient.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("无数据！");
                    return;
                }
                DataTable dt = GenDataTable(listPatient);
                gridView1.Columns.Clear();
                gcltrData1.DataSource = dt;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("错误:" + ex.Message);
            }

        }


        /// <summary>
        /// 生成表格数据
        /// </summary>
        /// <param name="Patients"></param>
        /// <returns></returns>
        private DataTable GenDataTable(List<EntityPidReportMain> Patients)
        {
            List<string> ListPidInNo = new List<string>();
            foreach (EntityPidReportMain p in Patients)
            {
                ListPidInNo.Add(p.PidInNo);//PidInNo
            }

            List<string> listComIds = new List<string> { selectCombine.valueMember };
            List<string> ListDitmEcode = new List<string>();

            foreach (string comId in listComIds)
            {
                ListDitmEcode.AddRange(listCombDetail.FindAll(o => o.ComId == comId).Select(a => a.ComItmEname));
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("日期", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("性别", typeof(string));
            dt.Columns.Add("年龄", typeof(string));
            dt.Columns.Add("标本", typeof(string));
            dt.Columns.Add("申请科室", typeof(string));
            dt.Columns.Add("病人编号", typeof(string));
            dt.Columns.Add("样本号", typeof(string));
            dt.Columns.Add("种类", typeof(string));
            dt.Columns.Add("床号", typeof(string));
            dt.Columns.Add("结果", typeof(string));
            dt.Columns.Add("备注", typeof(string));
            dt.Columns.Add("孕周", typeof(string));
            dt.Columns.Add("联系方式", typeof(string));
            dt.Columns.Add("申请医生", typeof(string));
            dt.Columns.Add("诊断", typeof(string));
            dt.Columns.Add("报告日期", typeof(string));
            dt.Columns.Add("报告人", typeof(string));
            dt.Columns.Add("检验组合", typeof(string));
            dt.Columns.Add("样本备注", typeof(string));

            foreach (string itm in ListDitmEcode)
            {
                dt.Columns.Add(itm, typeof(string));
            }

            string First = txtFirst.Text.Trim();
            try
            {
                ProxyObrResult proxyObrResult = new ProxyObrResult();
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListPidInNo = ListPidInNo;
                resultQc.ListDitmEcode = ListDitmEcode;
                //先一次查出所有需要查询的报告单结果
                List<EntityObrResult> listObrResult = proxyObrResult.Service.LisResultQuery(resultQc);
                foreach (EntityPidReportMain p in Patients)
                {
                    string pidInNo = p.PidInNo;

                    //由于需要多次匹配，先查出单个报告单的结果效率较高
                    List<EntityObrResult> results = listObrResult.FindAll(w => w.PidInNo == pidInNo);

                    DataRow dr = dt.NewRow();
                    dr["日期"] = p.RepInDate?.ToString("yyyy-MM-dd");
                    dr["姓名"] = p.PidName;
                    dr["性别"] = p.PidSexExp;
                    dr["年龄"] = AgeConvertRule(p.PidAgeExp);
                    dr["标本"] = p.SamName;
                    dr["申请科室"] = p.PidDeptName;
                    dr["病人编号"] = p.PidInNo;
                    dr["样本号"] = First + string.Format("{0:d4}", int.Parse(p.RepSid));
                    dr["种类"] = p.PidSrcId == "107" ? "门诊" : p.PidSrcId == "108" ? "住院" :
                        p.PidSrcId == "109" ? "体检" : "其它";
                    dr["床号"] = p.PidBedNo;
                    dr["孕周"] = p.PidPreWeek;
                    dr["联系方式"] = p.PidTel;
                    dr["申请医生"] = p.PidDocName;//DoctorName
                    dr["诊断"] = p.PidDiag;
                    dr["检验组合"] = p.PidComName;
                    dr["样本备注"] = p.SampRemark;


                    //遍历每个结果项目
                    if (results != null)
                    {
                        foreach (string itm in ListDitmEcode)
                        {
                            dr[itm] = results.Find(w => w.ItmEname == itm)?.ObrValue;
                        }
                    }

                    //报告日期和报告人 遗传要求不导出值，只给出空白列给他们填写即可
                    //dr["报告日期"] = p.RepAuditDate?.ToString("yyyy-MM-dd HH:mm:ss");
                    //dr["报告人"] = p.BgName;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
                DialogResult dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsxExportOptionsEx op = new DevExpress.XtraPrinting.XlsxExportOptionsEx();
                    op.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gridView1.OptionsPrint.PrintHeader = true;
                    gridView1.OptionsPrint.AutoWidth = false;
                    gridView1.ExportToXlsx(saveFileDialog.FileName, op);
                    lis.client.control.MessageDialog.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("导出Excel失败:" + ex.Message);
            }
        }
        #endregion
    }
}
