using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.result.DictToolkit;
using dcl.common;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmResultTemplate : FrmCommon
    {
        //缓存当前选中的组合数据,用于检索项目是否有组合ID
        //private DataTable dtCombineMi = null;
        private List<EntityDicCombineDetail> listDetail = null;

        /// <summary>
        /// 缓存符合当前仪器和日期的病人信息(带流水号并未审核未报告的)
        /// </summary>
        private List<EntityPidReportMain> dtPatWithHost = null;

        //记录仪器的专业组,以便通过专业组对组合、项目进行过滤,方便录入
        private string instrmtPType = "";

        /// <summary>
        /// 记录仪器包含的组合ID
        /// </summary>
        private string instrmtComID = "";

        /// <summary>
        /// 记录仪器包含的ITEMID
        /// </summary>
        private string instrmtItemID = "";

        /// <summary>
        /// 记录仪器ID
        /// </summary>
        private string instrmtItrID = null;

        public FrmResultTemplate()
        {
            InitializeComponent();
            this.txtType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.txtType_ValueChanged);
            this.txtInstrmt.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtInstrmt_ValueChanged);
            this.txtSidTo.EditValueChanged += new System.EventHandler(this.txtSidFrom_EditValueChanged);
            this.txtSidFrom.EditValueChanged += new System.EventHandler(this.txtSidFrom_EditValueChanged);
            this.rgNumType.EditValueChanged += new System.EventHandler(this.rgNumType_EditValueChanged);
            this.txtItem.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicItmItem>.ValueChangedEventHandler(this.txtItem_ValueChanged);
            this.txtCombine.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicCombine>.ValueChangedEventHandler(this.txtCombine_ValueChanged);
            this.chkFilterItem.CheckedChanged += new System.EventHandler(this.chkFilterItem_CheckedChanged);

            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.barResult_OnBtnSaveClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.barResult_BtnResetClick);
        }

        ProxyResultTemp proxy = new ProxyResultTemp();

        bool firstLoad = true;

        string userTypes = string.Empty;

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmResultTemplate_Load(object sender, EventArgs e)
        {
            sysToolBar1.QuickOption = true;
            //显示按钮
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSave.Name, sysToolBar1.BtnReset.Name, sysToolBar1.BtnClose.Name }, new string[] { "F3", "F4", "F12" });

            //测定日期
            txtPatDate.EditValue = DateTime.Now;

            //避免界面关联事件在第一次载入时被重复调用
            firstLoad = false;

            //控制界面
            tabResult_SelectedPageChanged(null, null);

            //默认多标本项目列表
            SetMultiSampleResult();

            SetBacResult();

            //保证多标本项目特征中至少有一个空表结构
            txtItem_ValueChanged(null, null);

            foreach (EntityUserLab dr in UserInfo.listUserLab)
            {
                userTypes += "'" + dr.LabId + "',";
            }
            if (userTypes != "")
            {
                userTypes = userTypes.TrimEnd(',');
                userTypes = " (" + userTypes + ") ";
            }
            sysToolBar1.Height = sysToolBar1.Height + 10;

            //由权限控制物理组别
            if (this.userTypes != "")
            {
                txtType.SetFilter((from x in txtType.getDataSource() where userTypes.Contains(x.ProId) select x).ToList());
            }
            else
            {
                if (UserInfo.isAdmin == false)
                    txtType.SetFilter(new List<EntityDicPubProfession>());
            }
        }

        /// <summary>
        /// 设置多标本项目列表
        /// </summary>
        private void SetMultiSampleResult()
        {
            if (firstLoad == false)
            {
                int startSid = (int)txtSidFrom.Value;
                int endSid = (int)txtSidTo.Value;

                multiSampleResult.LoadGrid(startSid, endSid, 5);
            }
        }

        /// <summary>
        /// 设置细菌结果列表
        /// </summary>
        private void SetBacResult()
        {
            if (firstLoad == false)
            {
                int startSid = (int)txtSidFrom.Value;
                int endSid = (int)txtSidTo.Value;

                bacResult.LoadGrid(startSid, endSid, 5);
            }
        }

        /// <summary>
        /// 切换Tab页时对控件的控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabResult_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            chkFilterItem.Visible = false;
            if (tabResult.SelectedTabPage == tabSingleCombine)
            {
                //显示可切换流水号
                rgNumType.Visible = true;
                if (int.Parse(rgNumType.EditValue?.ToString())==1)
                {
                    lbStart.Text = "流水始号";
                    lbTypeName.Text = "流水终号";
                    cbAddNullPat.Checked = false;
                    cbAddNullPat.Visible = false;//同时添加病人信息复选框不可见
                }
                else
                {
                    lbStart.Text = "标本始号";
                    lbTypeName.Text = "标本终号";
                    cbAddNullPat.Checked = false;
                    cbAddNullPat.Visible = true;
                }

                //单标本组合
                txtItem.Readonly = true;
                txtSidTo.Properties.ReadOnly = true;
                txtCombine.Readonly = false;

                if (txtInstrmt.valueMember != null && txtInstrmt.valueMember != null)
                {
                    singleResultA.SetItem(instrmtPType);
                    singleResultA.QuickResult("");
                }
                else
                    singleResultA.SetItem("");
            }

            if (tabResult.SelectedTabPage == tabMultiCombine)
            {
                //显示可切换流水号
                rgNumType.Visible = true;
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1)
                {
                    lbStart.Text = "流水始号";
                    lbTypeName.Text = "流水终号";
                    cbAddNullPat.Checked = false;
                    cbAddNullPat.Visible = false;//同时添加病人信息复选框不可见
                }
                else
                {
                    lbStart.Text = "标本始号";
                    lbTypeName.Text = "标本终号";
                    cbAddNullPat.Checked = false;
                    cbAddNullPat.Visible = true;
                }

                //多标本组合
                txtItem.Readonly = true;
                txtSidTo.Properties.ReadOnly = false;
                txtCombine.Readonly = false;
                if (txtInstrmt.valueMember != null && txtInstrmt.valueMember != null)
                {
                    multiResultA.SetItem(instrmtPType);
                    multiResultA.QuickResult("");
                }
                else
                    multiResultA.SetItem("");
            }
            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                //显示可切换流水号
                rgNumType.Visible = true;
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1)
                {
                    lbTypeName.Text = "流水终号";
                    multiSampleResult.sidName = "流水号";
                }
                else
                {
                    lbTypeName.Text = "标本终号";
                    multiSampleResult.sidName = "标本号";
                }

                //同时添加空病人信息复选框-可见
                cbAddNullPat.Checked = false;
                cbAddNullPat.Visible = true;
                chkFilterItem.Visible = true;

                //多标本项目
                txtItem.Readonly = false;
                txtSidTo.Properties.ReadOnly = false;
                txtCombine.Readonly = true;
                checkBox_resultMerg.Enabled = true;
                textBox_MergCut.Enabled = true;
                if (checkBox_resultMerg.Checked)
                {
                    //增加结果录入合并内容
                    multiSampleResult.blnMergCut = true;
                    multiSampleResult.strMergCut = textBox_MergCut.Text.Trim();
                }
            }
            else
            {
                checkBox_resultMerg.Enabled = false;
                textBox_MergCut.Enabled = false;
                multiSampleResult.blnMergCut = false;
            }
            if (tabResult.SelectedTabPage == tabImmItem)
            {
                cbAddNullPat.Checked = false;
                chkFilterItem.Visible = false;

                //隐藏可切换流水号
                rgNumType.Visible = false;
                lbTypeName.Text = "标本终号";

                //酶标板项目
                txtItem.Readonly = false;
                txtSidTo.Properties.ReadOnly = false;
                txtCombine.Readonly = true;
            }

            if (tabResult.SelectedTabPage == tabBcRes)
            {
                //隐藏可切换流水号
                rgNumType.Visible = false;
                lbTypeName.Text = "标本终号";

                //同时添加空病人信息复选框-可见
                cbAddNullPat.Checked = false;
                cbAddNullPat.Visible = true;
                chkFilterItem.Visible = false;


                //多标本项目
                txtItem.Readonly = true;
                txtSidTo.Properties.ReadOnly = false;
                txtCombine.Readonly = true;
                checkBox_resultMerg.Enabled = true;
                textBox_MergCut.Enabled = true;
                if (checkBox_resultMerg.Checked)
                {
                    //增加结果录入合并内容
                    multiSampleResult.blnMergCut = true;
                    multiSampleResult.strMergCut = textBox_MergCut.Text.Trim();
                }
            }
        }

        private EntityObrResult GetNewResultEntity(string itrId)
        {
            EntityObrResult result = new EntityObrResult();
            //仪器代码
            result.ObrItrId = itrId;
            //有效标志 0 历史结果 1 生效结果
            result.ObrFlag = 1;
            //结果类型 0 手工输入 1 仪器传输
            result.ObrType = 0;
            //报告类型 0 普通 1 OD结果
            result.ObrReportType = 0;
            //默认一个服务器时间
            result.ObrDate = ServerDateTime.GetServerDateTime();
            return result;
        }

        /// <summary>
        /// 保存录入_仅保存当前Tab页数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barResult_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();

            #region 检查字段不能为空
            if (txtInstrmt.valueMember == null || txtInstrmt.valueMember == "")
            {
                lis.client.control.MessageDialog.Show("请选择测定仪器", "信息");
                txtInstrmt.Focus();
                return;
            }

            if (txtPatDate.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请选择标本录入日期", "信息");
                txtPatDate.Focus();
                return;
            }

            if (tabResult.SelectedTabPage == tabImmItem || tabResult.SelectedTabPage == tabMultiItem)
            {
                if (txtItem.valueMember == null || txtItem.valueMember == "")
                {
                    lis.client.control.MessageDialog.Show("请选择项目", "信息");
                    txtItem.Focus();
                    return;
                }
            }

            if (tabResult.SelectedTabPage != tabSingleCombine
                && txtSidTo.Value < txtSidFrom.Value)
            {
                lis.client.control.MessageDialog.Show("标本终号不能大于标本始号", "信息");
                return;
            }

            //if (txtSidTo.Value - txtSidFrom.Value > 1000)
            //{
            //    lis.client.control.MessageDialog.Show("标本终号和标本始号相差不能超过1000", "信息");
            //    txtSidTo.Focus();
            //    return;
            //}
            #endregion

            #region 保存数据中相同的部分
            string itrId = txtInstrmt.valueMember;
            DateTime res_date = Convert.ToDateTime(txtPatDate.EditValue);

            //用于实际保存数据的表结构
            List<EntityObrResult> listResult = new List<EntityObrResult>();

            string itmId = "";
            string itmEcd = "";
            string itmRepEcd = "";

            if (!string.IsNullOrEmpty(txtItem.valueMember))
            {
                //经过前面判断,后面有用到txtItem的保存情况时valueMember肯定不为空
                itmId = txtItem.valueMember;
                itmEcd = txtItem.displayMember;
                itmRepEcd = txtItem.selectRow.ItmRepCode;
            }
            #endregion

            #region 单标本组合或多标本组合可以合并处理
            if (tabResult.SelectedTabPage == tabSingleCombine || tabResult.SelectedTabPage == tabMultiCombine)
            {
                DataTable itemResult = new DataTable();

                if (tabResult.SelectedTabPage == tabSingleCombine)
                {
                    itemResult = singleResultA.DataSource.Copy();
                }
                if (tabResult.SelectedTabPage == tabMultiCombine)
                {
                    itemResult = multiResultA.DataSource.Copy();
                }

                int startSid = (int)txtSidFrom.Value;
                int endSid = (int)txtSidTo.Value;

                if (tabResult.SelectedTabPage == tabSingleCombine)
                {
                    endSid = startSid;
                }

                dtPatWithHost = new List<EntityPidReportMain>();//清空有流水号的病人信息缓存
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1)//如果使用的是流水号,则更新缓存信息
                {
                    if (!string.IsNullOrEmpty(itrId))
                    {
                        //获取病人列表(带流水号并未审核未报告的)
                        EntityPatientQC patientQc = new EntityPatientQC();
                        patientQc.DateStart = res_date;
                        patientQc.DateEnd = res_date.AddDays(1);
                        patientQc.ListItrId.Add(itrId);
                        patientQc.RepStatus = "0";

                        dtPatWithHost = new ProxyPidReportMain().Service.PatientQuery(patientQc).FindAll(i => !string.IsNullOrEmpty(i.RepSerialNum));
                    }
                }

                //用于保存的有效数据
                List<string> checkSame = new List<string>();
                for (int n = startSid; n <= endSid; n++)
                {
                    for (int j = 0; j < itemResult.Columns.Count; j = j + 2)
                    {
                        for (int i = 0; i < itemResult.Rows.Count; i++)
                        {
                            string res_itm_id = itemResult.Rows[i][j].ToString();
                            string res_chr = itemResult.Rows[i][j + 1].ToString();

                            if (res_itm_id == "" || res_chr == "")
                            {
                                continue;
                            }

                            //记录转化后的标本号
                            string strSid_Temp = getSidBySidType(n.ToString());//(标本号或流水号)转标本号

                            if (string.IsNullOrEmpty(strSid_Temp))
                            {
                                //如果流水号没有对应的标本号,则跳过
                                continue;
                            }

                            //检查是否有重复的项目,因为所有标本下项目相同,仅在最小标本号时判断以节省效率
                            if (n == startSid)
                            {
                                if (checkSame.IndexOf(res_itm_id) != -1)
                                {
                                    lis.client.control.MessageDialog.Show("列表中存在重复的项目", "信息");
                                    return;
                                }
                                checkSame.Add(res_itm_id);
                            }

                            EntityObrResult result = GetNewResultEntity(itrId);
                            result.ObrSid = strSid_Temp;
                            result.ItmId = res_itm_id;
                            result.ObrValue = res_chr;

                            //获得项目的代码和打印代码
                            //DataRow[] drsItem = txtItem.dtSource.Select("itm_id='" + res_itm_id + "'");
                            List<EntityDicItmItem> listItem = txtItem.dtSource.FindAll(w => w.ItmId == res_itm_id);
                            if (listItem.Count > 0)
                            {
                                result.ItmEname = listItem[0].ItmEcode.ToString();
                                result.ItmReportCode = listItem[0].ItmRepCode.ToString();
                            }

                            if (txtCombine.valueMember != null && txtCombine.valueMember != "" && listDetail != null)
                            {
                                string comId = txtCombine.valueMember;

                                //如果该项目在当前选中组合中包含,则取得组合值得
                                if (listDetail.FindAll(w => w.ComItmId == res_itm_id).Count > 0)
                                {
                                    result.ItmComId = comId;
                                }
                            }
                            //生成标识
                            result.ObrId = dcl.common.ResultoHelper.GenerateResID(itrId, res_date, strSid_Temp);
                            listResult.Add(result);
                        }
                    }
                }

                //使用流水号时,需要判断带有效流水号的病人信息数,若为零,则不保存
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1 && (listResult == null || listResult.Count <= 0))
                {
                    lis.client.control.MessageDialog.Show("保存失败,不存在有效的流水号", "信息");
                    return;
                }
            }
            #endregion

            #region 多标本项目处理
            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                DataTable itemResult = multiSampleResult.GetDataSource();

                string dType = GetItrDataType();

                dtPatWithHost = new List<EntityPidReportMain>();//清空有流水号的病人信息缓存
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1)//如果使用的是流水号,则更新缓存信息
                {
                    if (!string.IsNullOrEmpty(itrId))
                    {
                        //获取病人列表(带流水号并未审核未报告的)
                        EntityPatientQC patientQc = new EntityPatientQC();
                        patientQc.DateStart = res_date;
                        patientQc.DateEnd = res_date.AddDays(1);
                        patientQc.ListItrId.Add(itrId);
                        patientQc.RepStatus = "0";

                        dtPatWithHost = new ProxyPidReportMain().Service.PatientQuery(patientQc).FindAll(i => !string.IsNullOrEmpty(i.RepSerialNum));
                    }
                }



                //先列后行可以保证结果数据按照标本号排序
                for (int j = 0; j < itemResult.Columns.Count; j = j + 2)
                {
                    //遍历数据
                    for (int i = 0; i < itemResult.Rows.Count; i++)
                    {
                        string sid = itemResult.Rows[i][j].ToString();
                        string chr = itemResult.Rows[i][j + 1].ToString();

                        if (sid == "" || chr == "")
                        {
                            //无标本号或结果时继续执行下一条
                            continue;
                        }
                        //记录转化后的标本号
                        sid = getSidBySidType(sid);//(标本号或流水号)转标本号

                        if (string.IsNullOrEmpty(sid))
                        {
                            //如果流水号没有对应的标本号,则跳过
                            continue;
                        }
                        EntityObrResult result = GetNewResultEntity(itrId);
                        result.ObrSid = sid;
                        result.ItmId = itmId;
                        result.ItmEname = itmEcd;
                        result.ItmReportCode = itmRepEcd;
                        if (dType != LIS_Const.InstmtDataType.Eiasa)
                        {
                            result.ObrValue = chr;
                        }

                        else
                        {
                            float temp = 0;
                            if (float.TryParse(chr, out temp)) //如果是数值
                            {
                                result.ObrValue2 = chr;
                                string judgeValue = DictImmJudge.Instance.GetJudge(this.txtItem.valueMember, chr);
                                if (!string.IsNullOrEmpty(judgeValue))
                                {
                                    result.ObrValue = judgeValue;
                                }
                            }
                            else
                                result.ObrValue = chr;
                            if (result.ObrValue.ToString().IndexOf("阳") >= 0)
                                result.RefFlag = "3";

                            result.ObrReportType = 1;//1:OD结果
                        }

                        //生成标识
                        result.ObrId = dcl.common.ResultoHelper.GenerateResID(itrId, res_date, sid);
                        listResult.Add(result);
                    }
                }
            }
            #endregion

            #region 微生物结果处理
            if (tabResult.SelectedTabPage == tabBcRes)
            {
                DataTable itemResult = bacResult.GetDataSource();


                //先列后行可以保证结果数据按照标本号排序
                for (int j = 0; j < itemResult.Columns.Count; j = j + 2)
                {
                    //遍历数据
                    for (int i = 0; i < itemResult.Rows.Count; i++)
                    {
                        string sid = itemResult.Rows[i][j].ToString();
                        string chr = itemResult.Rows[i][j + 1].ToString();

                        if (sid == "" || chr == "")
                        {
                            //无标本号或结果时继续执行下一条
                            continue;
                        }

                        EntityObrResult result = GetNewResultEntity(itrId);
                        result.ObrSid = sid;
                        result.ObrValue = chr;

                        //生成标识
                        result.ObrId = dcl.common.ResultoHelper.GenerateResID(itrId, res_date, sid);
                        listResult.Add(result);
                    }
                }
            }
            #endregion

            #region 酶标板项目处理
            if (tabResult.SelectedTabPage == tabImmItem)
            {
                int sidFrom = Convert.ToInt32(txtSidFrom.Value);
                int sidTo = Convert.ToInt32(txtSidTo.Value);

                DataTable itemResult = immResult.GetDataSource();

                //遍历行
                for (int i = 0; i < itemResult.Rows.Count; i++)
                {
                    //遍历列_第1列为非数据列
                    for (int j = 1; j < itemResult.Columns.Count; j++)
                    {
                        string resChr = itemResult.Rows[i][j].ToString();
                        if (resChr == "")
                        {
                            //无结果时继续下一条
                            continue;
                        }

                        //计算标本号
                        //int sid = i * 12 + itemResult.Columns.Count - 1;
                        int sid = i * 12 + j - 1;
                        sid = sid + sidFrom;
                        if (sid > sidTo)
                            continue;

                        EntityObrResult result = GetNewResultEntity(itrId);
                        result.ObrSid = sid.ToString();
                        result.ItmId = itmId;
                        result.ObrValue = resChr;
                        result.ItmEname = itmEcd;
                        result.ItmReportCode = itmRepEcd;
                        //生成标识
                        result.ObrId = dcl.common.ResultoHelper.GenerateResID(itrId, res_date, sid.ToString());
                        listResult.Add(result);
                    }
                }
            }
            #endregion


            #region 添加空病人信息

            //获取病人信息表结构
            List<EntityPidReportMain> listNullPatData = new List<EntityPidReportMain>();
            //添加空病人信息--未开始影响数据库
            //如果‘同时添加空病人信息’的复选框--可见--并且被勾选--则执行下内容
            if (cbAddNullPat.Visible && cbAddNullPat.Checked && listResult != null && listResult.Count > 0)
            {
                //记录标本号
                List<string> strtemplist = new List<string>();
                foreach (EntityObrResult drtempAdd in listResult)
                {
                    if (!strtemplist.Contains(drtempAdd.ObrSid.ToString()))
                    {
                        strtemplist.Add(drtempAdd.ObrSid.ToString());

                        EntityPidReportMain patient = new EntityPidReportMain();
                        patient.RepItrId = txtInstrmt.valueMember;
                        patient.ItrName = txtInstrmt.displayMember;
                        patient.RepSid = drtempAdd.ObrSid.ToString();
                        patient.RepInDate = Convert.ToDateTime(txtPatDate.EditValue);
                        patient.RepCheckUserId = UserInfo.loginID;
                        patient.SampCheckDate = Convert.ToDateTime(txtPatDate.EditValue);
                        patient.SampReceiveDate = Convert.ToDateTime(txtPatDate.EditValue).AddSeconds(-4);
                        patient.SampCollectionDate = Convert.ToDateTime(txtPatDate.EditValue).AddSeconds(-3);
                        patient.SampSendDate = Convert.ToDateTime(txtPatDate.EditValue).AddSeconds(-2);
                        patient.SampApplyDate = Convert.ToDateTime(txtPatDate.EditValue).AddSeconds(-1);

                        listNullPatData.Add(patient);
                    }
                }
            }

            #endregion


            if (listResult.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有需要保存的数据", "信息");
                return;
            }

            //显示结果确认窗体
            FrmResultView frm = new FrmResultView();
            frm.DataSource = listResult;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ProxyPatTempInput proxyPat = new ProxyPatTempInput();
                //开始影响数据库--添加空病人信息
                if (cbAddNullPat.Visible && cbAddNullPat.Checked && listNullPatData.Count > 0)
                {
                    foreach (EntityPidReportMain drNullPat in listNullPatData)
                    {
                        EntityQcResultList qcResultList = new EntityQcResultList();
                        qcResultList.patient = drNullPat;
                        EntityOperateResult opResult = proxyPat.Service.InsertPatCommonResult(dcl.client.common.Util.ToCallerInfo(), qcResultList);
                    }
                }

                //如果为单标本组合的录入情况,保存成功后把当前标本号+1
                if (tabResult.SelectedTabPage == tabSingleCombine)
                {
                    txtSidFrom.Value += 1;
                    txtCombine_ValueChanged(sender, null);
                }

                if (tabResult.SelectedTabPage == tabMultiCombine)
                {
                    txtCombine_ValueChanged(sender, null);
                }
            }

        }

        /// <summary>
        /// 重置数据_仅重置当前Tab页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barResult_BtnResetClick(object sender, EventArgs e)
        {
            if (tabResult.SelectedTabPage == tabSingleCombine)
            {
                txtCombine_ValueChanged(sender, null);
            }

            if (tabResult.SelectedTabPage == tabMultiCombine)
            {
                txtCombine_ValueChanged(sender, null);
            }

            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                SetMultiSampleResult();
                SetMultiSampleResultDefultValue();
            }
            if (tabResult.SelectedTabPage == tabImmItem)
            {
                immResult.QuickResult("");
            }
            if (tabResult.SelectedTabPage == tabBcRes)
            {
                SetBacResult();
            }
        }


        public string GetItrDataType()
        {
            if (this.txtInstrmt.valueMember != null && this.txtInstrmt.valueMember.Trim(null) != string.Empty)
            {
                string datatype = DictInstrmt.Instance.GetItrRepFlag(this.txtInstrmt.valueMember);
                return datatype;
            }
            return "-1";
        }




        /// <summary>
        /// 设置多标本项目列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSidFrom_EditValueChanged(object sender, EventArgs e)
        {
            int startSid = (int)txtSidFrom.Value;
            int endSid = (int)txtSidTo.Value;
            SetMultiSampleResult();
            SetMultiSampleResultDefultValue();

            SetBacResult();
        }

        /// <summary>
        /// 改变项目时，设置多标本项目中可用参考值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>

        private void SetMultiSampleResultDefultValue()
        {
            if (!string.IsNullOrEmpty(txtItem.displayMember))
            {
                string input = GetInput();
                int startSid = (int)txtSidFrom.Value;
                int endSid = (int)txtSidTo.Value;
                multiSampleResult.QuickInput(startSid, endSid, 5, input);
            }
        }

        private string GetInput()
        {
            if (int.Parse(rgQXType.EditValue.ToString()) == 1)
            {
                string strYan = UserInfo.GetSysConfigValue("Result_Template_YanX");
                return strYan == string.Empty ? "阳性(+)" : strYan;
            }
            else if (int.Parse(rgQXType.EditValue.ToString())==0)
            {
                string strYin = UserInfo.GetSysConfigValue("Result_Template_YinX");
                return strYin == string.Empty ? "阴性(-)" : strYin;
            }
            else if (int.Parse(rgQXType.EditValue.ToString()) == 2)
                return txtOther.Text;

            return "";
        }

        /// <summary>
        /// 自动填充结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoMasculine_Click(object sender, EventArgs e)
        {
            if (rdoNegative.Checked)
            {
                immResult.QuickResult("-");
            }

            if (rdoMasculine.Checked)
            {
                immResult.QuickResult("+");
            }
        }


        private bool IsEiasa()
        {
            string dType = GetItrDataType();
            return dType == LIS_Const.InstmtDataType.Eiasa;
        }
        

        private void txtQuickResult_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode != Keys.Enter)
                return;
            if (txtSID.Text == "" || txtSID.Text.Contains("例"))
                return;
            txtQuickResult.Text = GetQuickResult(txtQuickResult.Text);
            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                if (!string.IsNullOrEmpty(txtItem.displayMember))
                {
                    string input = GetInput();
                    int startSid = (int)txtSidFrom.Value;
                    int endSid = (int)txtSidTo.Value;
                    multiSampleResult.QuickInput(startSid, endSid, 5, SampleIDRangeUtil.ToList('.', txtSID.Text), txtQuickResult.Text);
                }
            }
            if (tabResult.SelectedTabPage == tabBcRes)
            {
                int startSid = (int)txtSidFrom.Value;
                int endSid = (int)txtSidTo.Value;
                bacResult.QuickInput(startSid, endSid, 5, SampleIDRangeUtil.ToList('.', txtSID.Text), txtQuickResult.Text);
            }
            txtQuickResult.Text = "";
        }

        private string GetQuickResult(string inputStr)
        {
            if (inputStr == "-")
            {
                string strYin = UserInfo.GetSysConfigValue("Result_Template_YinX");
                return strYin == string.Empty ? "阴性(-)" : strYin;
            }
            else if (inputStr == "+")
            {
                string strYan = UserInfo.GetSysConfigValue("Result_Template_YanX");
                return strYan == string.Empty ? "阳性(+)" : strYan;
            }
            else if (inputStr == "*")
            {
                return "弱阳性";
            }

            return inputStr;

        }

       

        private void SetDefaultValue()
        {
            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                if (!string.IsNullOrEmpty(txtItem.displayMember))
                {
                    string input = GetInput();
                    int startSid = (int)txtSidFrom.Value;
                    int endSid = (int)txtSidTo.Value;
                    multiSampleResult.QuickInput(startSid, endSid, 5, input);
                }
            }

        }

     

        private void txtOther_KeyDown(object sender, KeyEventArgs e)
        {
            if (int.Parse(rgQXType.EditValue.ToString())!=2)
                return;
            if (e.KeyCode != Keys.Enter)
                return;
            SetDefaultValue();

            txtOther.Text = "";
        }

        private void checkBox_resultMerg_CheckedChanged(object sender, EventArgs e)
        {
            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                if (checkBox_resultMerg.Checked)
                {
                    multiSampleResult.blnMergCut = true;
                    multiSampleResult.strMergCut = textBox_MergCut.Text.Trim();
                }
                else
                {
                    multiSampleResult.blnMergCut = false;
                }

            }
        }

        private void textBox_MergCut_TextChanged(object sender, EventArgs e)
        {
            multiSampleResult.strMergCut = textBox_MergCut.Text.Trim();
        }

        /// <summary>
        /// 根据标本号类型获取标本号
        /// </summary>
        /// <param name="str_number"></param>
        /// <returns></returns>
        private string getSidBySidType(string str_number)
        {
            if (rgNumType.EditValue != null&&int.Parse(rgNumType.EditValue?.ToString())==0)//是否选择标本号
            {
                return str_number;
            }
            else
            {
                string tempSid = "";

                if (dtPatWithHost.Count > 0 && (!string.IsNullOrEmpty(str_number)))
                {
                    //流水号转标本号
                    int index = dtPatWithHost.FindIndex(i => i.RepSerialNum == str_number);
                    if (index > -1)
                    {
                        tempSid = dtPatWithHost[index].RepSid;
                    }
                }

                return tempSid;
            }
        }

        private void chkFilterItem_CheckedChanged(object sender, EventArgs e)
        {
            multiSampleResult.FilterItem = chkFilterItem.Checked;
        }

        private void txtType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            txtInstrmt.valueMember = string.Empty;
            txtInstrmt.displayMember = string.Empty;

            string strIns = "(" + UserInfo.sqlUserItrs + ")";
            if (this.txtType.valueMember == "" || this.txtType.valueMember == null)
            {
                if (UserInfo.sqlUserItrs != "" && UserInfo.sqlUserItrs != "-1")
                {
                    txtInstrmt.SetFilter((from x in txtInstrmt.getDataSource() where strIns.Contains(x.ItrId) select x).ToList());
                }
                else
                {
                    if (UserInfo.isAdmin == false)
                        txtInstrmt.SetFilter(new List<EntityDicInstrument>());
                }
            }
            else
            {
                if (UserInfo.sqlUserItrs == "-1")
                    txtInstrmt.SetFilter(txtInstrmt.getDataSource().FindAll(i => i.ItrLabId == this.txtType.valueMember));
                else
                    txtInstrmt.SetFilter((from x in txtInstrmt.getDataSource() where strIns.Contains(x.ItrId) && x.ItrLabId == this.txtType.valueMember select x).ToList());
            }
        }

        private void txtInstrmt_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (txtInstrmt.selectRow != null && !string.IsNullOrEmpty(txtInstrmt.selectRow.ItrProId))
            {
                instrmtPType = txtInstrmt.selectRow.ItrProId.ToString();

                string p_irt_id = txtInstrmt.selectRow.ItrId.ToString();//获取仪器ID

                //如果当前选中组合与仪器专业组不匹配,则清空组合选择内容
                if (txtCombine.selectRow != null)
                {
                    if (txtCombine.selectRow.ComPriId != instrmtPType)
                    {
                        txtCombine.valueMember = "";
                        txtCombine.displayMember = "";
                    }
                }

                #region 根据仪器筛选对应的组合ID

                //根据仪器ID,筛选对应的组合ID
                if (!string.IsNullOrEmpty(p_irt_id) && (p_irt_id != instrmtItrID))
                {
                    instrmtItrID = p_irt_id;

                    if (txtCombine.selectRow != null)
                    {
                        txtCombine.valueMember = "";
                        txtCombine.displayMember = "";
                    }

                    ////缓存当前选中的仪器的组合,用于组合过滤
                    List<EntityDicItrCombine> itrCombs = proxy.Service.GetItrCombine(null);

                    itrCombs = itrCombs.FindAll(i => i.ItrId == p_irt_id);
                    if (itrCombs.Count > 0)
                    {
                        string strComId = "";
                        List<string> listCombId = new List<string>();
                        foreach (EntityDicItrCombine crow in itrCombs)
                        {
                            strComId = strComId + ",'" + crow.ComId.ToString() + "'";
                            listCombId.Add(crow.ComId.ToString());
                        }
                        //把组合ID字符串赋值到instrmtComID
                        instrmtComID = strComId.Substring((strComId.IndexOf(",") + 1));

                        List<EntityDicCombineDetail> combDetails = new List<EntityDicCombineDetail>();
                        combDetails = proxy.Service.GetCombDetail(listCombId).Distinct().ToList();
                        string stritemId = "";
                        if (combDetails.Count > 0)
                        {
                            foreach (EntityDicCombineDetail crow in combDetails)
                            {
                                stritemId = stritemId + ",'" + crow.ComItmId.ToString() + "'";
                            }
                            //把组合ID字符串赋值到instrmtComID
                            instrmtItemID = stritemId.Substring((stritemId.IndexOf(",") + 1));

                        }
                        else
                        {
                            instrmtItemID = "";
                        }
                    }
                    else
                    {
                        instrmtComID = "''";
                        instrmtItemID = "";
                    }

                }
                else if (string.IsNullOrEmpty(p_irt_id))
                {
                    instrmtItrID = null;
                    instrmtComID = "";
                    instrmtItemID = "";
                }

                #endregion


                if (tabResult.SelectedTabPage == tabSingleCombine)
                {
                    singleResultA.SetItem(instrmtPType);
                    singleResultA.QuickResult("");
                }

                if (tabResult.SelectedTabPage == tabMultiCombine)
                {
                    multiResultA.SetItem(instrmtPType);
                    multiResultA.QuickResult("");
                }
            }
            else
            {
                instrmtPType = "";
                instrmtComID = "";
                instrmtItrID = null;
                instrmtItemID = "";
                if (tabResult.SelectedTabPage == tabSingleCombine)
                {
                    singleResultA.SetItem("");
                }

                if (tabResult.SelectedTabPage == tabMultiCombine)
                {
                    multiResultA.SetItem("");
                }

            }

            //选择缺省类型
            if (IsEiasa()) //酶标
            {
                rgQXType.EditValue = 0;
            }
            else
                rgQXType.EditValue = 2;

            //按照仪器过滤
            if (instrmtComID != "")
            {
                txtCombine.SetFilter((from x in txtCombine.getDataSource() where instrmtComID.Contains(x.ComId) select x).ToList());
            }

            if (txtInstrmt.valueMember != null && txtInstrmt.valueMember != "")
            {
                if (instrmtItemID != "")
                {
                    txtItem.SetFilter((from x in txtItem.getDataSource() where instrmtItemID.Contains(x.ItmId) select x).ToList());
                }
                else
                {
                    EntityDicInstrument drInstrmt = txtInstrmt.selectRow;
                    txtItem.SetFilter(txtItem.getDataSource().FindAll(i => i.ItmPriId == drInstrmt.ItrProId));
                }
            }
        }

        /// <summary>
        /// 自动列出组合下的项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCombine_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (tabResult.SelectedTabPage == tabSingleCombine)
            {
                singleResultA.QuickResult("");
                singleResultA.FocusCellAfterSave();
            }

            if (tabResult.SelectedTabPage == tabMultiCombine)
            {
                multiResultA.QuickResult("");
            }

            if (txtCombine.valueMember == null || txtCombine.valueMember == "")
            {
                listDetail = null;
                return;
            }

            string comId = txtCombine.valueMember;

            List<string> listWhere = new List<string>();
            listWhere.Add(txtInstrmt.valueMember);
            listWhere.Add(comId);
            listDetail = new List<EntityDicCombineDetail>();
            listDetail = proxy.Service.GetCombDetailByComIdAndItrId(listWhere);

            //把当前组合对应的项目在列表中自动列出
            if (tabResult.SelectedTabPage == tabSingleCombine)
            {
                singleResultA.SetDataSource(listDetail);//listDetail
            }

            if (tabResult.SelectedTabPage == tabMultiCombine)
            {
                multiResultA.SetDataSource(listDetail);//listDetail
            }
        }

        private void txtItem_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (firstLoad == false)
            {
                List<string> listStr = new List<string>();
                string itmId = string.Empty;

                if (txtItem.valueMember != null && txtItem.valueMember != "")
                {
                    itmId = txtItem.valueMember;
                    multiSampleResult.itm_id = txtItem.valueMember;
                }
                else
                {
                    multiSampleResult.itm_id = null;
                }
                listStr.Add(itmId);
                EntityAnanlyseQC query = null;
                if (!string.IsNullOrEmpty(txtItem.valueMember) && txtPatDate.EditValue != null && !string.IsNullOrEmpty(txtInstrmt.valueMember))
                {
                    DateTime date = txtPatDate.DateTime.Date;
                    query = new EntityAnanlyseQC();
                    query.DateStart = date.AddDays(-1);
                    query.DateEnd = date;
                    query.ItrId = txtInstrmt.valueMember;
                    query.ItmId = txtItem.valueMember;
                }
                List<EntityDefItmProperty> properties = proxy.Service.GetItemProp(listStr);

                List<string> listSid = null;
                //缓存当前选中的组合数据,用于检索项目是否有组合ID
                if (query != null)
                {
                    listSid = proxy.Service.GetPatientsSid(query);
                }

                multiSampleResult.listSid = listSid;

                SetMultiSampleResultDefultValue();
            }
        }

        private void rgNumType_EditValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(rgNumType.EditValue?.ToString()) == 1)
            {
                lbStart.Text = "流水始号";
                lbTypeName.Text = "流水终号";
                cbAddNullPat.Checked = false;
                cbAddNullPat.Visible = false;//同时添加病人信息复选框不可见

            }
            else
            {
                lbStart.Text = "标本始号";
                lbTypeName.Text = "标本终号";
                cbAddNullPat.Checked = false;
                cbAddNullPat.Visible = true;
            }

            if (tabResult.SelectedTabPage == tabMultiItem)
            {
                if (int.Parse(rgNumType.EditValue?.ToString()) == 1)
                {
                    multiSampleResult.sidName = "流水号";
                }
                else
                {
                    multiSampleResult.sidName = "标本号";
                }
            }
        }

        private void rgQXType_EditValueChanged(object sender, EventArgs e)
        {
            if(rgNumType.EditValue!=null)
                SetDefaultValue();
        }
    }
}
