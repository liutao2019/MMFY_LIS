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
using dcl.common.extensions;

using lis.client.control;
using dcl.client.report;
using System.IO;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.elisa
{
    /// <summary>
    /// //TO_DO:保存时样本号和保存时间怎么取
    /// </summary>
    public partial class FrmEiasaAnalyse : FrmCommonExt
    {
        List<EntityObrElisaResult> tableItemDataSource;
        public FrmEiasaAnalyse()
        {
            InitializeComponent();
            complexSample.SetDisplayMode(ControlType.Complex);
            tableItemDataSource = new List<EntityObrElisaResult>();
        }

        List<EntityDicElisaItem> dtItemHole = new List<EntityDicElisaItem>();
        List<EntityDicElisaSort> dtHoleMode = new List<EntityDicElisaSort>();
        List<EntityDicElisaStatus> dtHoleStatus = new List<EntityDicElisaStatus>();

        /// <summary>
        /// 当前仪器当天的数据
        /// </summary>
        List<EntityObrElisaResult> dtResults = new List<EntityObrElisaResult>();


        List<EntityDicElisaCriter> dtJudge = new List<EntityDicElisaCriter>();
        List<EntityDicElisaCalu> dtCalc = new List<EntityDicElisaCalu>();
        String itemID = "";
        string calcExpression = "";
        Judgor judgor = null;
        ODCalc odClac = null;
        int calcAgainID = 223;
        bool canCalcAgain = false;
        bool isFirst = true;
        /// <summary>
        /// 仪器是否自动传项目ID
        /// </summary>
        bool blnIsSendItemID = true;
        /// <summary>
        /// 需要保存结果的类型(0-保存所有结果值，1-只保存定性结果值，2-只保存OD值结果)
        /// </summary>
        int intResultType = 0;
        /// <summary>
        ///  酶标质控根据半定量设置字典自动转换结果
        /// </summary>
        private bool qcAutoChangeQcValueSet = false;

        /// <summary>
        /// 获取项目ID（有两种情况获取，所以需集中处理）
        /// </summary>
        /// <returns></returns>
        internal string m_strGetItemId()
        {
            if (blnIsSendItemID)
            {
                if (!string.IsNullOrEmpty(lookUpEdit1.EditValue.ToString()))
                {
                    return lookUpEdit1.EditValue.ToString();
                }
                else
                {
                    return null;
                }
            }
            else if (!string.IsNullOrEmpty(selectDict_Item1.valueMember))
            { return selectDict_Item1.valueMember; }
            else {
                return null;
            }

        }

        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        internal string m_strGetItemName()
        {
            if (blnIsSendItemID)
                return lookUpEdit1.EditValue.ToString();
            else
                return selectDict_Item1.displayMember;

        }


        private void FrmEiasaAnalyse_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[]
            { sysToolBar1.BtnResultJudge.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.btnSaveTemplateForEiasa.Name,
                sysToolBar1.BtnPrint.Name,
                sysToolBar1.BtnSampleMonitor.Name,
                sysToolBar1.BtnClose.Name });

            sysToolBar1.BtnSave.Enabled = false;
            sysToolBar1.BtnResultJudge.Enabled = false;
            sysToolBar1.btnSaveTemplateForEiasa.Enabled = false;
            sysToolBar1.BtnSampleMonitor.Enabled = false;
            sysToolBar1.BtnSampleMonitor.Caption = "更新原始值";

            if (DesignMode)
                return;

            this.dateEdit1.EditValue = DateTime.Today;
            try
            {
                this.bsHoleMode.DataSource = dtHoleMode = CacheClient.GetCache<EntityDicElisaSort>();
                this.bsHoleStatus.DataSource = dtHoleStatus = CacheClient.GetCache<EntityDicElisaStatus>();
                this.bsItemHole.DataSource = dtItemHole = CacheClient.GetCache<EntityDicElisaItem>();
            }
            catch (Exception ex)
            {
                string i = ex.ToString();
            }

            selectDict_Instrmt1_onBeforeFilter();
            this.selectDict_Instrmt1.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.selectDict_Instrmt1_ValueChanged);

            //旧的项目测定关联，现已用新的
            selectDict_Item1_onBeforeFilter();
            this.selectDict_Item1.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicItmItem>.ValueChangedEventHandler(this.selectDict_Item1_ValueChanged_1);

            canCalcAgain = UserInfo.HaveFunction(calcAgainID);
            qcAutoChangeQcValueSet = UserInfo.GetSysConfigValue("QC_AutoChangeQcValueSet") == "是";
            #region 系统参数加载
            //测定项目是否为仪器传ID
            if (UserInfo.GetSysConfigValue("EiasaInstrumentsSendItemID") == "否")
            {
                blnIsSendItemID = false;
                layitemC2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layitemC1.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                blnIsSendItemID = true;
                layitemC2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layitemC1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            // 酶标孔位序号、孔位状态在界面打开后默认取第一个(惠州3院)
            lookUpHoleMode.ItemIndex = 0;
            lookUpHoleStatus.ItemIndex = 0;
            #endregion
        }

        /// <summary>
        /// 改变项目时
        /// </summary>     
        void selectDict_Item1_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            //改为测定序号时用此方法显示结果判定控件
            //ShowHoleWithItem();
            ReadShiJInfo();
        }

        /// <summary>
        /// 改变仪器时
        /// </summary>
        void selectDict_Instrmt1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            OnInstrmtOrDateChange();
            selectDict_Item1_onBeforeFilter();
        }


        /// <summary>
        /// 仪器或日期改变
        /// </summary>
        private void OnInstrmtOrDateChange()
        {

            if (Extensions.IsEmpty(selectDict_Instrmt1.displayMember) || Extensions.IsEmpty(dateEdit1.Text))
                return;

            dtResults.Clear();
            tableItemDataSource = new List<EntityObrElisaResult>();

            DateTime dtImmDate = dateEdit1.DateTime.Date;
            ProxyElisaAnalyse proxy = new ProxyElisaAnalyse();
            dtResults = proxy.Service.GetElisaResult(selectDict_Instrmt1.valueMember,dtImmDate).OrderBy(i => i.ResElsaplate.Length).ThenBy(i => i.ResElsaplate).ToList();
            if (dtResults.Count > 0)
            {
                if (UserInfo.GetSysConfigValue("EiasaInstrumentsSendItemID") == "是")
                {

                    //tableItemDataSource.Rows.Add(new object[] { });

                    //构造当前仪器当天所做项目下拉列表数据源
                    foreach (EntityObrElisaResult row in dtResults)
                    {
                        string itm_id = row.ResItmId.ToString();

                        if (itm_id != string.Empty
                            && tableItemDataSource.Where(i => i.ResItmId == itm_id).ToList().Count == 0
                            )
                        {
                            string itm_ecd = row.ItmEcd.ToString();
                            string itm_name = row.ItmName.ToString();
                            string itm_rep_ecd = row.ItmRepEcd.ToString();
                            EntityObrElisaResult rowItem = new EntityObrElisaResult();
                            rowItem.ResItmId = itm_id;
                            rowItem.ItmEcd = itm_ecd;
                            rowItem.ItmName = itm_name;
                            rowItem.ItmRepEcd = itm_rep_ecd;

                            tableItemDataSource.Add(rowItem);
                        }
                    }

                    this.lookUpEdit1.Properties.DataSource = tableItemDataSource.OrderBy(i => i.ResElsaplate.Length).ThenBy(i => i.ResElsaplate).ToList();
                    bsItemSequence.DataSource = null;
                }
                else
                {
                    this.lookUpEdit1.Properties.DataSource = null;
                    //仪器没有项目传进来的时候处理(直接加载测试序号事件)
                    this.m_mthEiasaInstrumentsNoSendItemID();

                }
            }
            else
            {
                this.lookUpEdit1.Properties.DataSource = null;
                bsItemSequence.DataSource = null;
            }
            //注释清空所选数据操作
            //this.lookUpEdit1.EditValue = null;

            this.complexSample.Clear();
        }

        /// <summary>
        /// 当测试项目有变化时，显示相应孔位状态与序号
        /// </summary>       
        private void ShowHoleWithItem(string formatNesPosValues)
        {

            sysToolBar1.btnSaveTemplateForEiasa.Enabled = true;
            sysToolBar1.BtnSampleMonitor.Enabled = true;

            ClearItemCache();
            string itemID = string.Empty;
            if (UserInfo.GetSysConfigValue("EiasaInstrumentsSendItemID") == "是")
            {
                itemID = lookUpEdit1.EditValue.ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(selectDict_Item1.valueMember))
                {
                    MessageDialog.Show("请选择测定项目！");
                    return;
                }
                itemID = selectDict_Item1.valueMember;
            }

            List<EntityDicElisaItem> rows = dtItemHole.Where(i => i.IplateItmId == itemID).ToList();
            if (rows != null && rows.Count > 0)
            {
                GetJudgesCacheTable();
                FindHoleMode(rows, formatNesPosValues);
                FindHoleStatus(rows);



                //获取该项目需要保存的结果类型
                if (!string.IsNullOrEmpty(rows[0].IplateResulttype))
                {
                    this.intResultType = Convert.ToInt32(rows[0].IplateResulttype);
                }
                else
                {
                    this.intResultType = 0;
                }

                bool findODJudge = FindODJudgeExpression(itemID);
                if (!findODJudge)
                {
                    lis.client.control.MessageDialog.Show("找不到该项目的OD值判断公式！");
                    return;
                }
                sysToolBar1.BtnResultJudge.Enabled = true;

                bool findNegOrPosJudge = FindNegOrPosJudgeExpression(itemID);

                if (findNegOrPosJudge == false)
                {
                    lis.client.control.MessageDialog.Show("找不到该项目的定性判断公式！");
                    return;
                }

            }
            else
            {
                lis.client.control.MessageDialog.Show("该项目没有指定孔位序号与状态，请在“项目字典－酶标字典”里配置。\r\n或将当前孔位序号与状态保存为模板。");
                lookUpHoleMode.Text = "";
                lookUpHoleStatus.Text = "";
                lookUpHoleMode.Focus();
                return;
            }

        }

        /// <summary>
        /// 清除项目Cache
        /// </summary>
        private void ClearItemCache()
        {
            itemID = calcExpression = "";
            judgor = null;
            odClac = null;
        }

        /// <summary>
        /// 结果判断表
        /// </summary>
        /// <param name="item_id"></param>
        private void GetJudgesCacheTable()
        {
            dtJudge = CacheClient.GetCache<EntityDicElisaCriter>();
            dtCalc = CacheClient.GetCache<EntityDicElisaCalu>();
        }

        /// <summary>
        /// 根据项目ID找到OD值判定公式
        /// </summary>
        /// <param name="item_id">项目ID</param>
        private bool FindODJudgeExpression(string itemID)
        {
            if (dtCalc.Count <= 0)
                return false;
            List<EntityDicElisaCalu> rows = dtCalc.Where(i => i.CalItmId == itemID).ToList(); 
            if (rows == null || rows.Count <= 0)
                return false;

            calcExpression = rows[0].CalExpression.ToString();
            return true;
        }


        /// <summary>
        /// 根据项目ID找到阴阳性判定公式
        /// </summary>
        /// <param name="item_id">项目ID</param>
        private bool FindNegOrPosJudgeExpression(string itemID)
        {
            if (dtJudge.Count <= 0)
                return false;
            List<EntityDicElisaCriter> rows = dtJudge.Where(i => i.CriItmId == itemID).ToList(); 
            if (rows == null || rows.Count <= 0)
                return false;

            string judgeValue = rows[0].CriValue.ToString();
            string strJudgeValueExpress = rows[0].CriExpression.ToString();
            string judgeExpress = rows[0].CriJudge.ToString();
            string judgeRes = rows[0].CriResult.ToString();

            string strFeebPosMin = rows[0].CriWposLowerLimit.ToString();
            string strFeebPosMax = rows[0].CriWposUpperLimit.ToString();
            bool blnMinusnull = false;
            if (rows[0].CriIgnoreNullSam == "0")
                blnMinusnull = true;



            //判断阴阳性判定值是否为表达式需自己计算
            if (!string.IsNullOrEmpty(strJudgeValueExpress) && strJudgeValueExpress.IndexOf("[") >= 0)
            {
                ODCalc JudClac = new ODCalc(strJudgeValueExpress, rows[0].CriNegLowerLimit.ToString(), rows[0].CriNegUpperLimit.ToString(), rows[0].CriPosLowerlimit.ToString(), rows[0].CriPosUpperlimit.ToString(), blnMinusnull);
                judgeValue = JudClac.GetExpressJudgePosNegVlaue(complexSample.HoleStatusList, complexSample.mainControls).ToString();

            }

            decimal dectest;
            if (string.IsNullOrEmpty(judgeValue) && decimal.TryParse(strJudgeValueExpress, out dectest))
            {
                judgeValue = strJudgeValueExpress;
            }

            //判断弱阳性判定值是否为表达式需自己计算
            if (!string.IsNullOrEmpty(strFeebPosMin) && strFeebPosMin.IndexOf("[") >= 0)
            {
                ODCalc JudClac = new ODCalc(strFeebPosMin, rows[0].CriNegLowerLimit.ToString(), rows[0].CriNegUpperLimit.ToString(), rows[0].CriPosLowerlimit.ToString(), rows[0].CriPosUpperlimit.ToString(), blnMinusnull);
                strFeebPosMin = JudClac.GetExpressJudgePosNegVlaue(complexSample.HoleStatusList, complexSample.mainControls).ToString();

            }

            if (!string.IsNullOrEmpty(strFeebPosMax) && strFeebPosMax.IndexOf("[") >= 0)
            {
                ODCalc JudClac = new ODCalc(strFeebPosMax, rows[0].CriNegLowerLimit.ToString(), rows[0].CriNegUpperLimit.ToString(), rows[0].CriPosLowerlimit.ToString(), rows[0].CriPosUpperlimit.ToString(), blnMinusnull);
                strFeebPosMax = JudClac.GetExpressJudgePosNegVlaue(complexSample.HoleStatusList, complexSample.mainControls).ToString();

            }

            judgor = new Judgor(judgeExpress, judgeValue, judgeRes, strFeebPosMin, strFeebPosMax);
            odClac = new ODCalc(calcExpression, rows[0].CriNegLowerLimit.ToString(), rows[0].CriNegUpperLimit.ToString(), rows[0].CriPosLowerlimit.ToString(), rows[0].CriPosUpperlimit.ToString(), blnMinusnull);
            return true;
        }

        /// <summary>
        /// 寻找孔位状态
        /// </summary>
        private void FindHoleStatus(List<EntityDicElisaItem> rows)
        {
            string statusID = rows[0].IplateStaId.ToString();
            List<EntityDicElisaStatus> holeStatusRows = dtHoleStatus.Where(i => i.StaId == statusID).ToList(); 
            if (holeStatusRows != null && holeStatusRows.Count > 0)
            {
                lookUpHoleStatus.Text = holeStatusRows[0].StaName.ToString();
                sampleHoleStatus.FormatHoleValues = complexSample.HoleStatus = holeStatusRows[0].StaHoleStaus.ToString();
                lookUpHoleStatus.ClosePopup();
            }
        }

        /// <summary>
        /// 寻找孔位模式
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="formatNesPosValues"></param>
        private void FindHoleMode(List<EntityDicElisaItem> rows, string formatNesPosValues)
        {
            string modeID = rows[0].IplateSortId.ToString();

            List<EntityDicElisaSort> holeModeRows = dtHoleMode.Where(i => i.SortId == modeID).ToList();
            if (holeModeRows != null && holeModeRows.Count > 0)
            {
                lookUpHoleMode.Text = holeModeRows[0].SortName.ToString();
                sampleHoleMode.StrNesPosValues = formatNesPosValues;
                sampleHoleMode.FormatHoleValues = complexSample.HoleMode = holeModeRows[0].SortHoleSorting.ToString();
                lookUpHoleMode.ClosePopup();
            }
        }

        void selectDict_Instrmt1_onBeforeFilter()
        {
            List<EntityDicInstrument> dtItr = selectDict_Instrmt1.dtSource;
            dtItr = dtItr.Where(i => i.ItrReportType == LIS_Const.InstmtDataType.Eiasa || i.ItrReportType == LIS_Const.InstmtDataType.BabyFilter).ToList();
            selectDict_Instrmt1.dtSource = dtItr;
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            OnInstrmtOrDateChange();
        }

        /// <summary> 结果判定 </summary>      
        private void sysToolBar1_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            if (!isFirst && !canCalcAgain)
            {
                lis.client.control.MessageDialog.Show("您没有重新计算的权限,请联系管理员！");
                return;
            }

            if (string.IsNullOrEmpty(lookUpItemID.Text))
            {
                lis.client.control.MessageDialog.Show("请选择测定项目！");
                return;
            }

            if (string.IsNullOrEmpty(calcExpression))
            {
                lis.client.control.MessageDialog.Show("该项目没有指定计算公式！");
                return;
            }

            if (complexSample.NotAllHolesHaveValues)
            {
                lis.client.control.MessageDialog.Show("请确保所有仪器原始值存在！");
                return;
            }

            sysToolBar1.BtnSave.Enabled = true;
            //加入样板范围
            complexSample.SampleRange = new SampleRange((int)seStartSample.Value, (int)seSampleCount.Value);
            //结果判定
            complexSample.JudgeOD(odClac);//OD值
            complexSample.JudgeNegOrPos(judgor);//定性
            isFirst = false;
        }

        /// <summary>
        /// 结果保存
        /// </summary>
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (lookUpItemID.Properties.GetDataSourceValue("ResFlag", lookUpItemID.ItemIndex).ToString() != "0")
            {
                if (lis.client.control.MessageDialog.Show("该酶标板面数据已计算保存，是否需要重新计算?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            //获取结果表结构
            List<EntityObrResult> dsResulto = new List<EntityObrResult>();

            List<EntityObrResult> dtResulto = dsResulto;
            if (dtResulto == null)
                return;

            #region 保存数据中相同的部分

            DateTime res_date = Convert.ToDateTime(dateEdit1.DateTime);

            //用于实际保存数据的表结构
            string itrId = selectDict_Instrmt1.valueMember;
            //仪器代码
            //dtResulto.Columns["res_itr_id"].DefaultValue = selectDict_Instrmt1.valueMember;
            ////测定日期在服务端生成,样本日期res_date作为关键ID的生成依据
            ////dtSave.Columns["res_date"].DefaultValue = res_date;
            ////有效标志 0 历史结果 1 生效结果
            //dtResulto.Columns["res_flag"].DefaultValue = 1;
            ////结果类型 0 手工输入 1 仪器传输
            //dtResulto.Columns["res_type"].DefaultValue = 0;
            ////报告类型 0 普通 1 OD结果
            //dtResulto.Columns["res_rep_type"].DefaultValue = 1;

            string itmId = "";
            string itmEcd = "";
            string itmRepEcd = "";
            //是否仪器自动传项目ID处理
            if (blnIsSendItemID)
            {
                if (lookUpEdit1.EditValue.ToString() != null)
                {
                    //经过前面判断,后面有用到txtItem的保存情况时valueMember肯定不为空
                    itmId = lookUpEdit1.EditValue.ToString();
                    itmEcd = lookUpEdit1.Text.ToString();
                    if (((List<EntityObrElisaResult>)lookUpEdit1.Properties.DataSource).Where(i => i.ResItmId == itmId).ToList().Count > 0)
                    {
                        itmRepEcd = ((List<EntityObrElisaResult>)lookUpEdit1.Properties.DataSource).Where(i => i.ResItmId == itmId).ToList()[0].ItmRepEcd.ToString();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(selectDict_Item1.valueMember))
                {
                    itmId = selectDict_Item1.valueMember;
                    itmEcd = selectDict_Item1.displayMember;
                    if (selectDict_Item1.dtSource.Where(i => i.ItmId == itmId).ToList().Count > 0)
                    {
                        itmRepEcd = selectDict_Item1.dtSource.Where(i => i.ItmId == itmId).ToList()[0].ItmRepCode.ToString();
                    }
                }
            }

            #endregion


            List<EntityObrResult> dtAdd = dtResulto;
            //获得酶标板结果
            List<string> listODResult = complexSample.GetODList();

            List<string> listNegOrPosResult = complexSample.GetNegOrPosList();
            //获得酶标板原始结果
            List<string> listValuesResult = complexSample.GetValuesList();

            List<string> listHoleMode = complexSample.HoleModeList;

            List<string> listHoleStatus = complexSample.HoleStatusList;

            List<EntityElisaQc> dtEiasaQc = new List<EntityElisaQc>();

            ProxyElisaAnalyse proxy = new ProxyElisaAnalyse();
            List<EntityDicQcConvert> qcSet = new List<EntityDicQcConvert>();
            List<EntityObrQcResult> qc_value = new List<EntityObrQcResult>();
            if (qcAutoChangeQcValueSet)
            {
                qcSet = proxy.Service.GetQCConvert(itrId);
                qc_value = proxy.Service.GetQCResult(itrId);
            }


            for (int i = 0; i < listHoleStatus.Count; i++)
            {
                string strStatus = listHoleStatus[i].ToString();
                if (strStatus == "5" || strStatus == "6" || strStatus == "7")
                {
                    string c_no = string.Empty;
                    switch (strStatus)
                    {
                        case "5": c_no = "H";
                            break;
                        case "6": c_no = "M";
                            break;
                        case "7": c_no = "L";
                            break;
                    }
                    if (qcAutoChangeQcValueSet)
                    {
                        List<EntityObrQcResult> rows =
                            qc_value.Where(w => w.QresItrId == itrId && w.QresItmId == itmId && w.QresLevel == c_no).ToList();

                        if (rows.Count > 0) continue;
                    }
                    EntityElisaQc drEiasaQc = new EntityElisaQc();
                    drEiasaQc.QcItrId = itrId;
                    drEiasaQc.QcItmId = itmId;
                    drEiasaQc.QcNoType = c_no;
                    drEiasaQc.QcDate = dateEdit1.DateTime;

                    drEiasaQc.QcValue = GetOdValue(listODResult[i], itmId, qcSet, listNegOrPosResult[i]);
                    dtEiasaQc.Add(drEiasaQc);
                }
            }

            //原始结果与阴阳结果拼接字符串
            string strImm_ResultOd = string.Empty;
            //如果原始结果值在数据库里前面有，号那么在记录OD值时也同时记录
            if (lookUpItemID.Properties.GetDataSourceValue("ResValue", lookUpItemID.ItemIndex).ToString()[0] == ',')
            {
                strImm_ResultOd = ",";
            }
            string strImm_ResultChr = string.Empty;

            #region 拼接孔位原始值与阴阳值
            for (int i2 = 0; i2 < listValuesResult.Count; i2++)
            {

                strImm_ResultOd += listODResult[i2] + ",";
                strImm_ResultChr += listNegOrPosResult[i2] + ",";


            }
            strImm_ResultOd = strImm_ResultOd.TrimEnd(',');
            strImm_ResultChr = strImm_ResultChr.TrimEnd(',');
            #endregion


            if (listHoleMode == null || listHoleMode.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("保存失败,请检查孔位序号是否正确！", "提示");
                return;
            }

            if (listHoleStatus == null || listHoleStatus.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("保存失败,请检查孔位状态是否正确！", "提示");
                return;
            }

            //由样本起始号-样本结束号录入，改为样本起始号录入，与样本录入个数。废除样本结束号的操作计算

            //complexSample.SampleRange = new SampleRange((int)seStartSample.Value, (int)seSampleCount.Value);
            //int count = complexSample.SampleRange.End - complexSample.SampleRange.Start + 1;
            complexSample.SampleRange = new SampleRange((int)seStartSample.Value, (int)(seStartSample.Value + seSampleCount.Value - 1));
            int count = (int)seSampleCount.Value;
            if (count <= 0)
            {
                lis.client.control.MessageDialog.Show("起始样本号输入有误！", "提示");
                return;
            }
            //样本号的规则：找最小的有效孔位序号（样本孔位），它就算开始样本号，第二个找到的就是开始样本号+1，由此累加。
            //***比如开始样本号为101，结束样本号为120，要找20个样本出来，找到最小的有效孔位序号为6（有可能前面的序号为阳性或阴性等对照，不能用）
            //*** 那样本号101的结果就是序号为6的孔位的结果，从此类推


            string strodValueCast = UserInfo.GetSysConfigValue("Eiasa_ItemOdCast");

            decimal dtry;
            decimal dtry2;
            string mesShow = string.Empty;
            int startSampleID = complexSample.SampleRange.Start;
            int startHoleModeID = 1;//从孔位序号1开始查找样本
            for (int i = 0; i < count; i++)
            {

                //孔位有效，是放样本的孔位
                while ((listHoleMode.IndexOf(startHoleModeID.ToString()) < 0 || listHoleStatus[listHoleMode.IndexOf(startHoleModeID.ToString())].Trim() != "1")
                    && startHoleModeID <= complexSample.Columns * complexSample.Rows)
                {
                    startHoleModeID++;
                }
                if (startHoleModeID > complexSample.Columns * complexSample.Rows)
                    continue;

                int index = listHoleMode.IndexOf(startHoleModeID.ToString());

                string sid = (startSampleID + i).ToString(); //样本号

                if (sid.Trim() == "0" || string.IsNullOrEmpty(sid)) //如果样本号不大于0
                    continue;

                //如果OD值为空的结果，直接跳空
                if (string.IsNullOrEmpty(listODResult[index]))
                {
                    startHoleModeID++;//孔位跳下一个
                    continue;
                }
                EntityObrResult drSave = new EntityObrResult();
                drSave.ObrFlag = 1;
                drSave.ObrSid = sid;
                drSave.ObrItrId = itrId;
                drSave.ItmId = itmId;
                drSave.ObrDate = res_date;
                drSave.ObrType = 1;
                drSave.ObrReportType = 1;

                //保存结果的时候，判断一下需要保存的结果有哪些，哪些不需要保存（0为所有都保存，1为只保存定性结果，2为只保存数值OD结果）
                if (this.intResultType == 0 || this.intResultType == 2)
                {
                    drSave.ObrValue2 = listODResult[index]; //OD值
                }

                //将原始结果保存
                drSave.ObrConvertValue = Convert.ToDecimal(listValuesResult[index]);

                if (this.intResultType == 0 || this.intResultType == 1)
                {
                    //陈星海特性项目转换用
                    if (!string.IsNullOrEmpty(strodValueCast) && strodValueCast.Contains(itmId)
                        && !string.IsNullOrEmpty(listODResult[index]) && decimal.TryParse(listODResult[index], out dtry))
                    {
                        try
                        {
                            string[] strs = strodValueCast.Split(';');

                            foreach (string str in strs)
                            {
                                if (str.Split(',')[0] == itmId)
                                {
                                    if (decimal.TryParse(str.Split(',')[1], out dtry2) && dtry2 >= dtry)
                                    {
                                        drSave.ObrValue = ForamtResult2(listNegOrPosResult[index]) + string.Format("(<{0})", dtry2);//阴阳结果
                                    }
                                    else if (decimal.TryParse(str.Split(',')[1], out dtry2) && dtry2 < dtry)
                                    {
                                        drSave.ObrValue = ForamtResult2(listNegOrPosResult[index]) + string.Format("(>{0})", dtry2);//阴阳结果
                                    }
                                    else
                                    {
                                        drSave.ObrValue = ForamtResult(listNegOrPosResult[index]);//阴阳结果
                                    }
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            mesShow = ex.Message;
                            drSave.ObrValue = ForamtResult(listNegOrPosResult[index]);//阴阳结果
                        }
                    }
                    else
                    {
                        drSave.ObrValue = ForamtResult(listNegOrPosResult[index]);//阴阳结果
                    }

                    if (listNegOrPosResult[index] == "-")
                        drSave.RefFlag = "0"; //阴性为0
                    else if (listNegOrPosResult[index] == "+")
                        drSave.RefFlag = "3"; //阳性为3
                    else if (listNegOrPosResult[index] == "±")
                        drSave.RefFlag = "3"; //弱阳性为2
                    else
                        drSave.RefFlag = "-1"; //未知为-1
                }
                else
                {
                    drSave.RefFlag = "-1"; //未知为-1
                }


                drSave.ItmEname = itmEcd;
                drSave.ItmReportCode = itmRepEcd;

                //生成标识
                drSave.ObrId = dcl.common.ResultoHelper.GenerateResID(itrId, res_date, sid);
                dtAdd.Add(drSave);

                //下次从找到的序号下一个找起
                startHoleModeID++;
            }

            if (GetBabyFilterFlag(itrId))
            {
                //dsAdd.Tables.Add(new DataTable("BabyFilter"));
            }


            if (!string.IsNullOrEmpty(mesShow))
                lis.client.control.MessageDialog.ShowAutoCloseDialog(mesShow);
            //保存
            string notsave = string.Empty;
            try
            {
                notsave = proxy.Service.SaveBatchObrResult(dtAdd);

                #region 更新酶标原始值更新

                EntityObrElisaResult drUpdate = ((EntityObrElisaResult)lookUpItemID.Properties.GetDataSourceRowByDisplayValue(lookUpItemID.Text));

                drUpdate.ResResultoOd = strImm_ResultOd;
                drUpdate.ResResultoChr = strImm_ResultChr;
                drUpdate.ResFlag = 1;
                drUpdate.ResItmId = this.selectDict_Item2.valueMember;
                drUpdate.ResStartSid = this.seStartSample.Text;
                drUpdate.ResEndSid = this.seEndSample.Text;

                try
                {
                    drUpdate.ResReagId = this.txtPiHao.Text;
                    drUpdate.ResReagDate = (DateTime)this.txtYouxiaoqi.EditValue;
                    drUpdate.ResReagManu = this.txtShijS.Text;
                    drUpdate.ResPosFmlu = this.txtYGongshi.Text;
                    drUpdate.ResManuDate = (DateTime)this.txtShengChangRiQi.EditValue;
                }
                catch (Exception ex)
                {

                }

                //((DataTable)bsItemSequence.DataSource).AcceptChanges();
                //((DataTable)lookUpItemID.Properties.DataSource).AcceptChanges();

                List<EntityObrElisaResult> dtbUpdate =new List<EntityObrElisaResult>();
                dtbUpdate.Add(drUpdate);

                foreach(var item in dtbUpdate)
                {
                    EntityRequest request = new EntityRequest();
                    request.SetRequestValue(item);
                    EntityResponse result = new EntityResponse();
                    result = proxy.Service.UpdateElisaResult(request);
                }

                #endregion
                textEdit1.Text = "已计算";



                string message = "保存成功";
                if (notsave != "")
                {
                    message += "\r\n" + notsave;
                }
                message += "\r\n" + "如有新板请重新输入起始结束样本号！";
                lis.client.control.MessageDialog.Show(message, "信息");

                proxy.Service.SaveQcValue(dtEiasaQc);

            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("保存失败,请检查数据的有效性" + ex.ToString(), "信息");
                return;
            }
        }

        private string GetOdValue(string odValue, string itmID, List<EntityDicQcConvert> table, string negOrPosValue)
        {
            if (qcAutoChangeQcValueSet && table.Count > 0)
            {
                List<EntityDicQcConvert> rows =
                    table.Where(i => i.ItmId == itmID && i.ItmValue == negOrPosValue).ToList(); 
                if (rows.Count > 0 && rows[0].ItmConvertValue != null &&
                    !string.IsNullOrEmpty(rows[0].ItmConvertValue.ToString()))
                {
                    odValue = rows[0].ItmConvertValue.ToString();
                }
            }
            return odValue;
        }

        private string ForamtResult(string result)
        {
            if (result == "-")
            {
                string strYin = UserInfo.GetSysConfigValue("Result_Template_YinX");
                return strYin == string.Empty ? "阴性(-)" : strYin;
            }
            else if (result == "+")
            {
                string strYan = UserInfo.GetSysConfigValue("Result_Template_YanX");
                return strYan == string.Empty ? "阳性(+)" : strYan;
            }
            else if (result == "±")
                return "弱阳性(±)";
            else
                return result;
        }

        private string ForamtResult2(string result)
        {
            if (result == "-")
            {
                string strYin = UserInfo.GetSysConfigValue("Result_Template_YinX");
                return strYin == string.Empty ? "阴性" : strYin;
            }
            else if (result == "+")
            {
                string strYan = UserInfo.GetSysConfigValue("Result_Template_YanX");
                return strYan == string.Empty ? "阳性" : strYan;
            }
            else if (result == "±")
                return "弱阳性";
            else
                return result;
        }

        private bool GetBabyFilterFlag(string itrid)
        {
            bool isBabyFilter = false;
            if (!string.IsNullOrEmpty(itrid))
            {
                List<EntityDicInstrument> itrTable = CacheClient.GetCache<EntityDicInstrument>();

                List<EntityDicInstrument> rows = itrTable.Where(i => i.ItrId == itrid).ToList();

                if (rows.Count > 0 && rows[0].ItrReportType != null &&
                    rows[0].ItrReportType.ToString() == LIS_Const.InstmtDataType.BabyFilter)
                {
                    isBabyFilter = true;
                }
            }
            return isBabyFilter;
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 序号更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpItemID_EditValueChanged(object sender, EventArgs e)
        {
            string formatNesPosValues = string.Empty;
            if (!String.IsNullOrEmpty(lookUpItemID.Text))
            {
                complexSample.FormatHoleValues = lookUpItemID.Properties.GetDataSourceValue("ResValue", lookUpItemID.ItemIndex).ToString();
                complexSample.ImmID = lookUpItemID.Properties.GetDataSourceValue("ResId", lookUpItemID.ItemIndex).ToString();

                //加载计算状态
                if (lookUpItemID.Properties.GetDataSourceValue("ResFlag", lookUpItemID.ItemIndex).ToString() == "0")
                {
                    textEdit1.Text = "未计算";
                }
                else
                {
                    textEdit1.Text = "已计算";

                    #region 加载已计算结果到酶标板里



                    //加载OD值数据
                    this.complexSample.FormatODValues = lookUpItemID.Properties.GetDataSourceValue("ResResultoOd", lookUpItemID.ItemIndex).ToString();
                    //加载阴阳定性数据
                    formatNesPosValues =
                        lookUpItemID.Properties.GetDataSourceValue("ResResultoChr", lookUpItemID.ItemIndex)
                                    .ToString();
                    this.complexSample.FormatNesPosValues = formatNesPosValues;


                    #endregion
                }
                if (lookUpItemID.ItemIndex >= 0)
                {
                    string itemID = lookUpItemID.Properties.GetDataSourceValue("ResItmId", lookUpItemID.ItemIndex).ToString();

                    if (!ChanggingItem)
                    {
                        if (!string.IsNullOrEmpty(itemID))
                        {
                            this.selectDict_Item2.SelectByID(itemID);
                            ChanggingItem = true;
                            this.selectDict_Item1.SelectByID(itemID);
                            ChanggingItem = false;
                        }
                        //else
                        //{
                        //    this.selectDict_Item2.valueMember = string.Empty;
                        //    this.selectDict_Item2.displayMember = string.Empty;
                        //    ChanggingItem = true;
                        //    this.selectDict_Item1.valueMember = string.Empty;
                        //    this.selectDict_Item1.displayMember = string.Empty;
                        //    ChanggingItem = false;
                        //}
                    }
                    else if (!string.IsNullOrEmpty(this.selectDict_Item1.valueMember))
                    {
                        this.selectDict_Item2.SelectByID(this.selectDict_Item1.valueMember);

                    }
                    else
                    {
                        this.selectDict_Item2.valueMember = string.Empty;
                        this.selectDict_Item2.displayMember = string.Empty;
                    }

                    //if (!string.IsNullOrEmpty(itemID) && !ChanggingItem)
                    //{
                    //    this.selectDict_Item2.SelectByID(itemID);
                    //    ChanggingItem = true;
                    //    this.selectDict_Item1.SelectByID(itemID);
                    //    ChanggingItem = false;
                    //}
                    //else if (!string.IsNullOrEmpty(this.selectDict_Item1.valueMember))
                    //{
                    //    this.selectDict_Item2.SelectByID(this.selectDict_Item1.valueMember);

                    //}
                    //else
                    //{
                    //    this.selectDict_Item2.valueMember = string.Empty;
                    //    this.selectDict_Item2.displayMember = string.Empty;
                    //}

                    object start_sid = lookUpItemID.Properties.GetDataSourceValue("ResStartSid", lookUpItemID.ItemIndex);
                    object end_sid = lookUpItemID.Properties.GetDataSourceValue("ResEndSid", lookUpItemID.ItemIndex);

                    if (start_sid != null && start_sid.ToString() != "null" && !string.IsNullOrEmpty(start_sid.ToString()))
                    {
                        this.seStartSample.Value = decimal.Parse(start_sid.ToString());
                    }
                    //更新前的程序在录入了上一个板的数据后，下一个板会继承上一个板的开始样本号和结束样本号。
                    //else
                    //{
                    //    this.seStartSample.Value = 1;
                    //}
                    if (end_sid != null && end_sid.ToString() != "null" && !string.IsNullOrEmpty(end_sid.ToString()))
                    {
                        this.seEndSample.Value = decimal.Parse(end_sid.ToString());
                    }
                    //else
                    //{
                    //    this.seEndSample.Value = 92;

                    //}

                }
                else
                {
                    this.selectDict_Item2.valueMember = string.Empty;
                    this.selectDict_Item2.displayMember = string.Empty;

                }

                try
                {
                    txtPiHao.EditValue = lookUpItemID.Properties.GetDataSourceValue("ResReagId", lookUpItemID.ItemIndex);
                    txtYouxiaoqi.EditValue = lookUpItemID.Properties.GetDataSourceValue("ResReagDate", lookUpItemID.ItemIndex);
                    txtShengChangRiQi.EditValue = lookUpItemID.Properties.GetDataSourceValue("ResManuDate", lookUpItemID.ItemIndex);
                    txtYGongshi.EditValue = lookUpItemID.Properties.GetDataSourceValue("ResPosFmlu", lookUpItemID.ItemIndex);
                    txtShijS.EditValue = lookUpItemID.Properties.GetDataSourceValue("ResReagManu", lookUpItemID.ItemIndex);
                }
                catch (Exception ex)
                {

                }
                //string itemID = lookUpItemID.Properties.GetDataSourceValue(EiasaTable.Med.ID, lookUpItemID.ItemIndex).ToString();
                ShowHoleWithItem(formatNesPosValues);
            }

        }

        /// <summary>
        /// 保存模板
        /// </summary>
        private void sysToolBar1_BtnSaveTemplateClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lookUpItemID.Text))
            {
                lis.client.control.MessageDialog.Show("没有有效项目！");
                lookUpItemID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(lookUpHoleMode.Text))
            {
                lis.client.control.MessageDialog.Show("没有对应孔位序号！");
                lookUpHoleMode.Focus();
                return;
            }


            if (string.IsNullOrEmpty(lookUpHoleStatus.Text))
            {
                lis.client.control.MessageDialog.Show("没有对应孔位状态！");
                lookUpHoleStatus.Focus();
                return;
            }

            if (ExistsItemHole(this.m_strGetItemId()))
            {
                lis.client.control.MessageDialog.Show("该项目已指定孔位序号与状态，不能再增加此模板！");
                lookUpHoleStatus.Focus();
                return;
            }

            //保存模板
            EntityDicElisaItem dr = new EntityDicElisaItem();
            dr.IplateItmId = selectDict_Item1.valueMember;
            if (lookUpHoleMode.EditValue != DBNull.Value)
            {
                dr.IplateSortId = lookUpHoleMode.EditValue.ToString();
            }
            if (lookUpHoleStatus.EditValue != DBNull.Value)
            {
                dr.IplateStaId = lookUpHoleStatus.EditValue.ToString();
            }
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            ProxyCommonDic proxy = new ProxyCommonDic("svc.ConElisaItemHole");
            proxy.New(request);
            result = proxy.Search(request);
            dtItemHole = result.GetResult() as List<EntityDicElisaItem>;
            if (IsActionSuccess)
            {
                lis.client.control.MessageDialog.Show("操作成功！", "提示");
            }
            else
            {
                lis.client.control.MessageDialog.Show("操作失败！", "提示");
            }
        }

        /// <summary>
        /// 是否存在项目对应孔位
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        private bool ExistsItemHole(string itemID)
        {
            List<EntityDicElisaItem> rows = dtItemHole.Where(i => i.IplateItmId == itemID).ToList();
            if(rows != null && rows.Count > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 孔位序号更改
        /// </summary>
        private void lookUpHoleMode_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicElisaSort> holeModeRows = dtHoleMode.Where(i => i.SortId == lookUpHoleMode.EditValue.ToString()).ToList();
            if (holeModeRows != null && holeModeRows.Count > 0)
            {
                sampleHoleMode.FormatHoleValues = complexSample.HoleMode = holeModeRows[0].SortHoleSorting.ToString();
                lookUpHoleMode.ClosePopup();
            }
        }

        /// <summary>
        /// 孔位状态更改
        /// </summary>
        private void lookUpHoleStatus_EditValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (lookUpHoleStatus.EditValue == null || string.IsNullOrEmpty(lookUpHoleStatus.EditValue.ToString())) return;
                List<EntityDicElisaStatus> holeStatusRows = dtHoleStatus.Where(i => i.StaId == lookUpHoleStatus.EditValue.ToString()).ToList();
                if (holeStatusRows != null && holeStatusRows.Count > 0)
                {
                    sampleHoleStatus.FormatHoleValues = complexSample.HoleStatus = holeStatusRows[0].StaHoleStaus.ToString();
                    lookUpHoleStatus.ClosePopup();
                    //张：项目ID改为从m_strGetItemId()方法中取
                    this.FindNegOrPosJudgeExpression(this.m_strGetItemId());
                }
                if(lookUpHoleStatus.ItemIndex==-1)
                {
                    lookUpHoleStatus.ItemIndex = 0;
                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 项目下拉改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            this.complexSample.Clear();
            if (this.lookUpEdit1.EditValue != null
                && this.lookUpEdit1.EditValue.ToString().Trim() != string.Empty
                && dtResults != null
                && dtResults.Count > 0
                )
            {
                string itm_id = this.m_strGetItemId();

                List<EntityObrElisaResult> rowsItem = this.dtResults.Where(i => i.ResItmId == itm_id).ToList();

                if (rowsItem.Count > 0)
                {
                    List<EntityObrElisaResult> tableResultTemp =new List<EntityObrElisaResult>();
                    foreach (EntityObrElisaResult row in rowsItem)
                    {
                        tableResultTemp.Add(row);
                    }
                    this.bsItemSequence.DataSource = tableResultTemp.OrderBy(i => i.ResElsaplate.Length).ThenBy(i => i.ResElsaplate).ToList();

                }
                else
                {
                    this.bsItemSequence.DataSource = null;
                }
            }
            else
            {
                this.bsItemSequence.DataSource = null;
            }
        }


        /// <summary>
        /// 当仪器没有传项目ID时直接进行序号加载处理
        /// </summary>
        internal void m_mthEiasaInstrumentsNoSendItemID()
        {
            this.complexSample.Clear();
            if (dtResults != null
                && dtResults.Count > 0
                )
            {
                List<EntityObrElisaResult> tableResultTemp = new List<EntityObrElisaResult>();
                foreach (EntityObrElisaResult row in this.dtResults)
                {
                    tableResultTemp.Add(row);
                }
                this.bsItemSequence.DataSource = tableResultTemp.OrderBy(i => i.ResElsaplate.Length).ThenBy(i => i.ResElsaplate).ToList();


            }
            else
            {
                this.bsItemSequence.DataSource = null;
            }
        }

        private void selectDict_Item1_ValueChanged_1(object sender, control.ValueChangeEventArgs args)
        {
            if (!ChanggingItem)
            {
                ChanggingItem = true;
                lookUpItemID_EditValueChanged(null, null);
                ChanggingItem = false;
            }
        }

        bool ChanggingItem = false;

        void WriteShiJInfoXml()
        {
            if (!string.IsNullOrEmpty(this.selectDict_Item1.valueMember)
            && !string.IsNullOrEmpty(this.txtShijS.Text)
             && !string.IsNullOrEmpty(this.txtPiHao.Text)
             && this.txtYouxiaoqi.EditValue != null
                )
            {
                try
                {


                    DataTable dt = new DataTable("试剂信息");
                    if (!File.Exists(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml"))
                    {

                        dt.Columns.Add("项目");
                        dt.Columns.Add("试剂批号");
                        dt.Columns.Add("有效期");
                        dt.Columns.Add("试剂商");
                        dt.Columns.Add("阳性公式");
                        dt.WriteXml(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml");

                    }
                    dt.ReadXml(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml");
                    DataRow[] rows = dt.Select(string.Format("项目='{0}'", this.selectDict_Item1.valueMember));
                    if (rows.Length > 0)
                    {
                        rows[0]["阳性公式"] = this.txtYGongshi.Text;
                        rows[0]["试剂商"] = this.txtShijS.Text;
                        rows[0]["试剂批号"] = this.txtPiHao.Text;
                        rows[0]["有效期"] = ((DateTime)this.txtYouxiaoqi.EditValue).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dt.Rows.Add(new object[] { this.selectDict_Item1.valueMember, this.txtPiHao.Text, ((DateTime)this.txtYouxiaoqi.EditValue).ToString("yyyy-MM-dd"), this.txtShijS.Text });
                        dt.WriteXml(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml");
                    }
                }
                catch (Exception)
                {

                    // throw;
                }
            }

        }
        void ReadShiJInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.selectDict_Item1.valueMember))
                {
                    if (File.Exists(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml"))
                    {
                        DataTable dt = new DataTable("试剂信息");
                        dt.Columns.Add("项目");
                        dt.Columns.Add("试剂批号");
                        dt.Columns.Add("有效期");
                        dt.Columns.Add("试剂商");
                        dt.Columns.Add("阳性公式");
                        dt.ReadXml(Application.StartupPath + "\\EiasaAnalyseSJInfo.xml");
                        DataRow[] rows = dt.Select(string.Format("项目='{0}'", this.selectDict_Item1.valueMember));
                        if (rows.Length > 0)
                        {
                            this.txtYGongshi.Text = rows[0]["阳性公式"] != null ? rows[0]["阳性公式"].ToString() : string.Empty;
                            this.txtShijS.Text = rows[0]["试剂商"].ToString();
                            this.txtPiHao.Text = rows[0]["试剂批号"].ToString();
                            this.txtYouxiaoqi.EditValue = DateTime.Parse(rows[0]["有效期"].ToString());
                        }
                        else
                        {
                            this.txtShijS.Text = string.Empty;
                            this.txtPiHao.Text = string.Empty;
                            this.txtYouxiaoqi.EditValue = null;
                        }
                    }

                }
            }
            catch (Exception)
            {


            }

        }



        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {

            clsDrawSamleResult objDrawing = new clsDrawSamleResult(complexSample);
            byte[] buff = objDrawing.m_ByteDrawingAllSample();
            string judgorValue;
            //this.panel1.BackgroundImage = img;
            string youxiaoqi = string.Empty;
            if (this.txtYouxiaoqi.EditValue != null && this.txtYouxiaoqi.EditValue.ToString() != "")
            {
                youxiaoqi = ((DateTime)this.txtYouxiaoqi.EditValue).ToString("yyyy/MM/dd");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("图像", typeof(byte[]));
            dt.Columns.Add("仪器");
            dt.Columns.Add("日期");
            dt.Columns.Add("项目");
            dt.Columns.Add("序号");
            dt.Columns.Add("试剂批号");
            dt.Columns.Add("有效期");
            dt.Columns.Add("试剂商");
            dt.Columns.Add("打印时间");
            dt.Columns.Add("检验者");

            dt.Columns.Add("空白对照");
            dt.Columns.Add("阴性对照");
            dt.Columns.Add("阳性对照");
            dt.Columns.Add("阳性公式");
            dt.Columns.Add("CutOff");
            //张：判断结果判断judgor是否为空，若为空且没有为所选项目设置结果判断则给它赋string.Empty
            if (judgor == null)
            {
                judgorValue = string.Empty;
                GetJudgesCacheTable();
                if (dtJudge.Count > 0)
                {
                    List<EntityDicElisaCriter> rows = dtJudge.Where(i => i.CriItmId == itemID).ToList();
                    if (rows != null)
                    {
                        judgorValue = rows[0].CriValue.ToString();
                    }
                }
            }
            else
            {
                judgorValue = judgor.Value.ToString();
            }
            //张：通过m_strGetItemId()获取项目ID
            //2014-01-08 edit 把this.m_strGetItemId()改为m_strGetItemName() 从编码改为名称
            dt.Rows.Add(new object[] { buff, this.selectDict_Instrmt1.displayMember, this.dateEdit1.Text, this.m_strGetItemName(), this.lookUpItemID.Text, this.txtPiHao.Text, youxiaoqi, this.txtShijS.Text
            ,DateTime.Now.ToString("yyyy/MM/dd HH:mm")
            ,UserInfo.userName
           , objDrawing.GetResult("2")
           ,objDrawing.GetResult("3")
              ,objDrawing.GetResult("4")
            ,  this.txtYGongshi.Text
            ,judgorValue
            });
            WriteShiJInfoXml();
            EntityDCLPrintData printData = new EntityDCLPrintData();
            printData.ReportName = "酶标板打印";
            printData.ReportData.Tables.Add(dt);
            DCLReportPrint.PrintPreviewByData(printData);
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit txtItem = ((TextEdit)sender);

            if (txtItem.EditValue.ToString().Trim() == "0")
            {
                txtItem.Text = "未计算";
            }
            else
            {
                txtItem.Text = "已计算";
            }
        }

        private void seEndSample_EditValueChanged(object sender, EventArgs e)
        {
            seSampleCount.Value = seEndSample.Value - seStartSample.Value + 1;
        }

        private void seSampleCount_EditValueChanged(object sender, EventArgs e)
        {
            seEndSample.Value = seSampleCount.Value + seStartSample.Value - 1;
        }

        private void selectDict_Item1_onBeforeFilter()
        {
            EntityDicInstrument row = selectDict_Instrmt1.selectRow;
            if (row != null)
            {
                string itr_id = row.ItrId.ToString();
                if(itr_id.Trim() != string.Empty)
                {
                    List<EntityDicCombineDetail> dtComDetail = CacheClient.GetCache<EntityDicCombineDetail>();
                    List<EntityDicItrCombine> dtItrCom = CacheClient.GetCache<EntityDicItrCombine>();

                    List<EntityDicItrCombine> drItrComs = dtItrCom.Where(i => i.ItrId == itr_id).ToList();

                    if (drItrComs.Count == 0)
                    {
                        selectDict_Item1.dtSource = new List<EntityDicItmItem>();
                        selectDict_Item2.dtSource = new List<EntityDicItmItem>();
                        return;
                    }

                    StringBuilder sbInsCom = new StringBuilder();

                    foreach (EntityDicItrCombine drInsCom in drItrComs)
                    {
                        sbInsCom.Append(string.Format(",'{0}'", drInsCom.ComId.ToString()));
                    }

                    sbInsCom.Remove(0, 1);

                    List<EntityDicCombineDetail> drComItms = dtComDetail.Where(i => sbInsCom.ToString().Contains(i.ComId)).ToList();

                    if (drComItms.Count == 0)
                    {
                        selectDict_Item1.dtSource = new List<EntityDicItmItem>();
                        selectDict_Item2.dtSource = new List<EntityDicItmItem>();
                        return;
                    }

                    StringBuilder sbComItm = new StringBuilder();

                    foreach (EntityDicCombineDetail drComItm in drComItms)
                    {
                        sbComItm.Append(string.Format(",'{0}'", drComItm.ComItmId.ToString()));
                    }

                    sbComItm.Remove(0, 1);
                    List<EntityDicItmItem> dtItem = selectDict_Item1.dtSource;
                    selectDict_Item1.dtSource = dtItem.Where(i =>i.ItmId != null && sbComItm.ToString().Contains(i.ItmId)).ToList();
                    selectDict_Item2.dtSource = dtItem.Where(i => i.ItmId != null && sbComItm.ToString().Contains(i.ItmId)).ToList();
                }
            }
        }

        /// <summary>
        /// 更新原始结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnSampleMonitorClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(complexSample.ImmID))
            {

                StringBuilder sb = new StringBuilder();
                foreach (string item in complexSample.GetValuesList())
                {
                    sb.Append("," + item);
                }

                string imm_id = complexSample.ImmID;

                ProxyElisaAnalyse proxy = new ProxyElisaAnalyse();
                bool isSuccess = proxy.Service.UpdateResValue(imm_id, sb.ToString());

                if (isSuccess)
                {
                    if (this.bsItemSequence.DataSource != null && this.bsItemSequence.DataSource is List<EntityObrElisaResult>)
                    {
                        List<EntityObrElisaResult> drs = (this.bsItemSequence.DataSource as List<EntityObrElisaResult>).Where(i => i.ResId == imm_id).ToList();

                        if (drs.Count > 0)
                        {
                            drs[0].ResValue = sb.ToString();
                        }
                    }
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("更新成功", 1);
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("更新失败", 1);
                }
            }
        }

        private void barButtonItem1_ItemPress(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnInstrmtOrDateChange();//刷新测定序号
        }

        private void lookUpItemID_MouseUp(object sender, MouseEventArgs e)
        {
            OnInstrmtOrDateChange();
        }
    }
}