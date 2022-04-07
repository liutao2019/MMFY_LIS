using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using lis.client.control;
using System.Collections;
using dcl.client.report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using dcl.common;
using System.IO;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace dcl.client.statistical
{
    public partial class FrmAdvanGeneralQuery : FrmCommon
    {
        
        public FrmAdvanGeneralQuery()
        {
            InitializeComponent();
        }
        public List<EntityTpTemplate> dtStat;
        Dictionary<string, object> daAll;
        private void FrmAdvanBacQuery_Load(object sender, EventArgs e)
        {
        }

        private void btnConfrim_Click(object sender, EventArgs e)
        {
            gvItem.CloseEditor();
            dtStat.Clear();
            SaveTemplate();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        public string ConvertFromTableName(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) return string.Empty;
            string StTableName = string.Empty;
            #region 表名转换
            switch (tablename)
            {
                case "dtInst":
                    StTableName = "dict_instrmt";
                    break;
                case "dtDep":
                    StTableName = "dict_depart";
                    break;
                case "dtDiag":
                    StTableName = "dict_diagnos";
                    break;
                case "dtSam":
                    StTableName = "dict_sample";
                    break;
                case "dtPhyType":
                    StTableName = "dict_PhyType";
                    break;
                case "dtSpeType":
                    StTableName = "dict_SepType";
                    break;
                case "dtCombine":
                    StTableName = "dict_combine";
                    break;
                case "dtUs":
                    StTableName = "poweruserinfo";
                    break;
                case "dtAndit":
                    StTableName = "poweruserinfo1";
                    break;
                case "dtDoc":
                    StTableName = "dict_doctor";
                    break;
                case "dictResRefFlag":
                    StTableName = "dict_res_ref_flag";
                    break;
                case "dtOri":
                    StTableName = "Dict_origin";
                    break;
                case "dictUrgent":
                    StTableName = "Dict_bscripe";
                    break;
                case "dictSampStatus":
                    StTableName = "Dict_s_state";
                    break;
                case "dictSampRemark":
                    StTableName = "Dict_sample_remarks";
                    break;
            }
            #endregion
            return StTableName;
        }

        private void SaveTemplate()
        {
            dtStat = new List<EntityTpTemplate>();
            List<EntityDicInstrument> dvIns = ((List<EntityDicInstrument>)gcApparatus.DataSource);
            dvIns = dvIns.Where(i => i.Checked == true).ToList();
            string tablename = string.Empty;
            foreach (var item in dvIns)
            {
                tablename = ConvertFromTableName("dtInst");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubDept> dvDep = ((List<EntityDicPubDept>)gcDivisions.DataSource);
            dvDep = dvDep.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDep)
            {
                tablename = ConvertFromTableName("dtDep");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubIcd> dvDiag = ((List<EntityDicPubIcd>)gridControlZD.DataSource);
            dvDiag = dvDiag.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDiag)
            {
                tablename = ConvertFromTableName("dtDiag");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
            dvSam = dvSam.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSam)
            {
                tablename = ConvertFromTableName("dtSam");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
            dvCom = dvCom.Where(i => i.Checked == true).ToList();
            foreach (var item in dvCom)
            {
                tablename = ConvertFromTableName("dtCombine");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityMark> dvMar = ((List<EntityMark>)gcMark.DataSource);
            dvMar = dvMar.Where(i => i.Checked == true).ToList();
            foreach (var item in dvMar)
            {
                addRow(item.SpId, "dtMark", dtStat);
            }

            List<EntityObrResult> dvItem = (List<EntityObrResult>)bsNull.DataSource;
            addRowItem(dvItem, dtStat);

            List<EntityDicPubProfession> dvPhy = ((List<EntityDicPubProfession>)gcZuBie.DataSource);
            dvPhy = dvPhy.Where(i => i.Checked == true).ToList();
            foreach (var item in dvPhy)
            {
                tablename = ConvertFromTableName("dtPhyType");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubProfession> dvSep = ((List<EntityDicPubProfession>)gcSepGroups.DataSource);
            dvSep = dvSep.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSep)
            {
                tablename = ConvertFromTableName("dtSpeType");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntitySysUser> dvDoc = ((List<EntitySysUser>)gcChkDoc.DataSource);
            dvDoc = dvDoc.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDoc)
            {
                tablename = ConvertFromTableName("dtUs");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntitySysUser> dvAndit = ((List<EntitySysUser>)gcNo2Andi.DataSource);
            dvAndit = dvAndit.Where(i => i.Checked == true).ToList();
            foreach (var item in dvAndit)
            {
                tablename = ConvertFromTableName("dtAndit");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicDoctor> dvDicDoc = ((List<EntityDicDoctor>)gcSendDoc.DataSource);
            dvDicDoc = dvDicDoc.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDicDoc)
            {
                tablename = ConvertFromTableName("dtDoc");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicResultTips> dvRef = ((List<EntityDicResultTips>)gcRefflag.DataSource);
            dvRef = dvRef.Where(i => i.Checked == true).ToList();
            foreach (var item in dvRef)
            {
                tablename = ConvertFromTableName("dictResRefFlag");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntityDicOrigin> dvOri = (List<EntityDicOrigin>)gcOriId.DataSource;
            dvOri = dvOri.Where(i => i.Checked == true).ToList(); ;
            foreach (var item in dvOri)
            {
                tablename = ConvertFromTableName("dtOri");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntityDicPubEvaluate> dvUrgent = (List<EntityDicPubEvaluate>)gcUrgent.DataSource;
            dvUrgent = dvUrgent.Where(i => i.Checked == true).ToList(); ;
            foreach (var item in dvUrgent)
            {
                tablename = ConvertFromTableName("dictUrgent");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntitySex> dvSex = ((List<EntitySex>)gcSex.DataSource);
            dvSex = dvSex.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSex)
            {
                addRow(item.SpId, "dtSex", dtStat);
            }

            List<EntityDicSState> dvSampState = ((List<EntityDicSState>)gcSampStatus.DataSource);
            dvSampState = dvSampState.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSampState)
            {
                tablename = ConvertFromTableName("dictSampStatus");
                addRow(item.SpId, "tablename", dtStat);
            }

            List<EntityDicSampRemark>dvSampRemark = ((List<EntityDicSampRemark>)gcSampRemark.DataSource);
            dvSampRemark = dvSampRemark.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSampRemark)
            {
                tablename = ConvertFromTableName("dictSampRemark");
                addRow(item.SpId, "tablename", dtStat);
            }
        }

        private void addRowItem(List<EntityObrResult> dv, List<EntityTpTemplate> dt)
        {
            List<EntityObrResult> dr = dv.Where(i => i.ItmEname != null || i.ItmEname != "").ToList();
            foreach (EntityObrResult drSt in dr)
            {
                EntityTpTemplate template = new EntityTpTemplate();
                template.StType = "GeneralStatistics";
                template.StName = "";
                template.StTableName = "null";
                template.ResItmEcd = drSt.ItmEname.ToString();
                template.ResChr = drSt.ObrValue.ToString();
                template.ResOdChr = drSt.ObrValue2.ToString();
                template.ResUnit = drSt.ObrUnit;
                template.ResOr = drSt.ResOr;
                dt.Add(template);
            }
        }

        private void addRow(string SpId,string tablename, List<EntityTpTemplate> dt)
        {
                EntityTpTemplate template = new EntityTpTemplate();
                template.StType = "GeneralStatistics";
                template.StName = "";
                template.StTableId = SpId;
                template.StTableName = tablename;
                dt.Add(template);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            Close();
        }
        private DataTable dtNull = new DataTable();
        List<EntityObrResult> dtitem = null;

        #region 全局变量
        /// <summary>
        /// 性别
        /// </summary>
        List<EntitySex> dvSex = new List<EntitySex>();
        /// <summary>
        /// 急查
        /// </summary>
        List<EntityMark> dvMark = new List<EntityMark>();
        /// <summary>
        /// 仪器
        /// </summary>
        List<EntityDicInstrument> dvIns = new List<EntityDicInstrument>();
        /// <summary>
        /// 科室
        /// </summary>
        List<EntityDicPubDept> dvDep = new List<EntityDicPubDept>();
        /// <summary>
        /// 诊断
        /// </summary>
        List<EntityDicPubIcd> dvDiag = new List<EntityDicPubIcd>();
        /// <summary>
        /// 标本
        /// </summary>
        List<EntityDicSample> dvSam = new List<EntityDicSample>();
        /// <summary>
        /// 实验组
        /// </summary>
        List<EntityDicPubProfession> dvPhy = new List<EntityDicPubProfession>();
        /// <summary>
        /// 专业组
        /// </summary>
        List<EntityDicPubProfession> dvSep = new List<EntityDicPubProfession>();
        /// <summary>
        /// 组合
        /// </summary>
        List<EntityDicCombine> dvCom = new List<EntityDicCombine>();
        /// <summary>
        /// 检验者
        /// </summary>
        List<EntitySysUser> dvChkDoc = new List<EntitySysUser>();
        /// <summary>
        /// 报告者
        /// </summary>
        List<EntitySysUser> dvNo2Andi = new List<EntitySysUser>();
        /// <summary>
        /// 申请者
        /// </summary>
        List<EntityDicDoctor> dvSendDoc = new List<EntityDicDoctor>();
        /// <summary>
        /// 病人来源
        /// </summary>
        List<EntityDicOrigin> dvOri = new List<EntityDicOrigin>();
        /// <summary>
        /// 危机类型
        /// </summary>
        List<EntityDicPubEvaluate> dvUrgent = new List<EntityDicPubEvaluate>();
        /// <summary>
        /// 项目
        /// </summary>
        List<EntityDicItmItem> dtItem = new List<EntityDicItmItem>();
        /// <summary>
        /// 结果提示
        /// </summary>
        List<EntityDicResultTips> dvResRefFlag = new List<EntityDicResultTips>();

        /// <summary>
        /// 标本状态
        /// </summary>
        List<EntityDicSState> dvSampStatus= new List<EntityDicSState>();

        /// <summary>
        /// 标本状态
        /// </summary>
        List<EntityDicSampRemark> dvSampRemark = new List<EntityDicSampRemark>();
        #endregion
        public void BindGridView(Dictionary<string, object> daAllIn)
        {
            daAll = daAllIn;
            dvIns = daAll["dtInst"] as List<EntityDicInstrument>;
            gcApparatus.DataSource = dvIns;

            dvDep = daAll["dtDep"] as List<EntityDicPubDept>;
            gcDivisions.DataSource = dvDep;

            dvDiag = (daAll["dtDiag"]) as List<EntityDicPubIcd>;
            gridControlZD.DataSource = dvDiag;

            dvSam = (daAll["dtSam"]) as List<EntityDicSample>;
            gridControlBB.DataSource = dvSam;

            dvPhy = (daAll["dtPhyType"]) as List<EntityDicPubProfession>;
            gcZuBie.DataSource = dvPhy;

            dvSep = (daAll["dtSpeType"]) as List<EntityDicPubProfession>;
            gcSepGroups.DataSource = dvSep;

            dvCom = (daAll["dtCombine"]) as List<EntityDicCombine>;
            gcCombine.DataSource = dvCom;

            dvChkDoc = (daAll["dtUs"]) as List<EntitySysUser>;
            gcChkDoc.DataSource = dvChkDoc;

            dvNo2Andi = (daAll["dtAndit"]) as List<EntitySysUser>;
            gcNo2Andi.DataSource = dvNo2Andi;

            dvSendDoc = (daAll["dtDoc"]) as List<EntityDicDoctor>;
            gcSendDoc.DataSource = dvSendDoc;

            dvOri = (daAll["dtOri"]) as List<EntityDicOrigin>;
            gcOriId.DataSource = dvOri;

            dvUrgent = (daAll["dictUrgent"]) as List<EntityDicPubEvaluate>;
            gcUrgent.DataSource = dvUrgent;

            
            dtitem = (daAll["dtNull"]) as List<EntityObrResult>;
            bsNull.DataSource = dtitem;

            dtItem =  (daAll["dtItem"]) as List<EntityDicItmItem>;
            this.bsItem.DataSource = dtItem.Where(i => i.ItmDelFlag == "0").ToList();

            this.bsTiaojian.DataSource = CommonValue.GetTiaojian(); 

            dvResRefFlag = (daAll["dictResRefFlag"]) as List<EntityDicResultTips>;
            gcRefflag.DataSource = dvResRefFlag;

            dvSampStatus = (daAll["dictSampStatus"]) as List<EntityDicSState>;
            gcSampStatus.DataSource = dvSampStatus;

            dvSampRemark = (daAll["dictSampRemark"]) as List<EntityDicSampRemark>;
            gcSampRemark.DataSource = dvSampRemark;

            #region 性别
            EntitySex dtSex = new EntitySex();
            dtSex.Checked = false;
            dtSex.SexName = "男";
            dtSex.SexPy = "N";
            dtSex.SexWb = "L";
            dtSex.SpId = "1";
            EntitySex dtSex1 = new EntitySex();
            dtSex1.Checked = false;
            dtSex1.SexName = "女";
            dtSex1.SexPy = "N";
            dtSex1.SexWb = "V";
            dtSex1.SpId = "2";
            #endregion

            #region 急查
            EntityMark dtMark = new EntityMark();
            dtMark.Checked = false;
            dtMark.MarkName = "常规";
            dtMark.MarkPy = "CG";
            dtMark.MarkWb = "IF";
            dtMark.SpId = "1";
            EntityMark dtMark1 = new EntityMark();
            dtMark1.Checked = false;
            dtMark1.MarkName = "急查";
            dtMark1.MarkPy = "JC";
            dtMark1.MarkWb = "QS";
            dtMark1.SpId = "2";
            #endregion

            #region 急查和性别的勾选
            if(dtStat.Count > 0)
            {
                foreach(var item in dtStat)
                {
                    if(item.StTableName == "dtSex")
                    {
                        if(item.StTableId == dtSex.SpId)
                        {
                            dtSex.Checked = true;
                        }
                        if (item.StTableId == dtSex1.SpId)
                        {
                            dtSex1.Checked = true;
                        }
                    }
                    if (item.StTableName == "dtMark")
                    {
                        if (item.StTableId == dtMark.SpId)
                        {
                            dtMark.Checked = true;
                        }
                        if (item.StTableId == dtMark1.SpId)
                        {
                            dtMark1.Checked = true;
                        }
                    }
                }
            }
            #endregion
            dvSex.Add(dtSex);
            dvSex.Add(dtSex1);
            gcSex.DataSource = dvSex;
            dvMark.Add(dtMark);
            dvMark.Add(dtMark1);
            gcMark.DataSource = dvMark;


            #region 重置后取消勾选
            if (dtStat != null && dtStat.Count == 0)
            {
                foreach (var item in dvIns)
                {
                    item.Checked = false;
                }
                foreach (var item in dvDep)
                {
                    item.Checked = false;
                }
                foreach (var item in dvDiag)
                {
                    item.Checked = false;
                }
                foreach (var item in dvResRefFlag)
                {
                    item.Checked = false;
                }
                foreach (var item in dvSendDoc)
                {
                    item.Checked = false;
                }
                foreach (var item in dvSam)
                {
                    item.Checked = false;
                }
                foreach (var item in dvPhy)
                {
                    item.Checked = false;
                }
                foreach (var item in dvSep)
                {
                    item.Checked = false;
                }
                foreach (var item in dvCom)
                {
                    item.Checked = false;
                }
                foreach (var item in dvChkDoc)
                {
                    item.Checked = false;
                }
                foreach (var item in dvMark)
                {
                    item.Checked = false;
                }

                foreach (var item in dvSex)
                {
                    item.Checked = false;
                }
                foreach (var item in dvUrgent)
                {
                    item.Checked = false;
                }
                foreach (var item in dvOri)
                {
                    item.Checked = false;
                }
                foreach (var item in dvNo2Andi)
                {
                    item.Checked = false;
                }
                bsNull.Clear();
            }
            #endregion

        }


        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            string ckName = xtcConditions.SelectedTabPage.Name;
            bool i = false;
            if (checkAll.Checked)
                i = true;
            else
                i = false;
            switch (ckName)
            {
                case "xtInstrument":
                    foreach(var item in dvIns)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcApparatus.DataSource = dvIns;
                    gcApparatus.RefreshDataSource();
                    break;
                case "xtDivisions":
                    foreach (var item in dvDep)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcDivisions.DataSource = dvDep;
                    gcDivisions.RefreshDataSource();
                    break;
                case "xtDiagnosis":
                    foreach (var item in dvDiag)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gridControlZD.DataSource = dvDiag;
                    gridControlZD.RefreshDataSource();
                    break;
                case "xtSpecimens":
                    foreach (var item in dvSam)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gridControlBB.DataSource = dvSam;
                    gridControlBB.RefreshDataSource();
                    break;
                case "xtPortfolio":
                    foreach (var item in dvCom)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcCombine.DataSource = dvCom;
                    gcCombine.RefreshDataSource();
                    break;
                case "xtLogo":
                    foreach (var item in dvMark)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcMark.DataSource = dvMark;
                    gcMark.RefreshDataSource();
                    break;
                case "xtPhysicsGroup":
                    foreach (var item in dvPhy)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcZuBie.DataSource = dvPhy;
                    gcZuBie.RefreshDataSource();
                    break;
                case "xtProfessionalGroup":;
                    foreach (var item in dvSep)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSepGroups.DataSource = dvSep;
                    gcSepGroups.RefreshDataSource();
                    break;
                case "xtVerifier":
                    foreach (var item in dvChkDoc)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcChkDoc.DataSource = dvChkDoc;
                    gcChkDoc.RefreshDataSource();
                    break;
                case "xtCensorshipby":
                    foreach (var item in dvSendDoc)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSendDoc.DataSource = dvSendDoc;
                    gcSendDoc.RefreshDataSource();
                    break;
                case "xtNo2Andi":
                    foreach (var item in dvNo2Andi)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcNo2Andi.DataSource = dvNo2Andi;
                    gcNo2Andi.RefreshDataSource();
                    break;
                case "xtOriId":
                    foreach (var item in dvOri)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcOriId.DataSource = dvOri;
                    gcOriId.RefreshDataSource();
                    break;
                case "xtSex":
                    foreach (var item in dvSex)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSex.DataSource = dvSex;
                    gcSex.RefreshDataSource();
                    break;
                case "xtRef_flag":
                    foreach (var item in dvResRefFlag)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcRefflag.DataSource = dvResRefFlag;
                    gcRefflag.RefreshDataSource();
                    break;
                case "xtUrgentType":
                    foreach (var item in dvUrgent)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcUrgent.DataSource = dvUrgent;
                    gcUrgent.RefreshDataSource();
                    break;
                case "xtSampStatus":
                    foreach (var item in dvSampStatus)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSampStatus.DataSource = dvSampStatus;
                    gcSampStatus.RefreshDataSource();
                    break;
                case "xtSampRemark":
                    foreach (var item in dvSampRemark)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSampRemark.DataSource = dvSampRemark;
                    gcSampRemark.RefreshDataSource();
                    break;
                default:
                    break;
            }
        }


        private void xtcConditions_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            string ckName = xtcConditions.SelectedTabPage.Name;
            int i = 0; 
            int count = 0;
            switch (ckName)
            {
                case "xtInstrument":
                    List<EntityDicInstrument> dvIns = ((List<EntityDicInstrument>)gcApparatus.DataSource);
                    i = dvIns.Where(w => w.Checked == true).ToList().Count;
                    count = dvIns.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtDivisions":
                    List<EntityDicPubDept> dvDep = ((List<EntityDicPubDept>)gcDivisions.DataSource);
                    i = dvDep.Where(w => w.Checked == true).ToList().Count;
                    count = dvDep.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtDiagnosis":
                    List<EntityDicPubIcd> dvDiag = ((List<EntityDicPubIcd>)gridControlZD.DataSource);
                    i = dvDiag.Where(w => w.Checked == true).ToList().Count;
                    count = dvDiag.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtSpecimens":
                    List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
                    i = dvSam.Where(w => w.Checked == true).ToList().Count;
                    count = dvSam.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtPortfolio":
                    List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
                    i = dvCom.Where(w => w.Checked == true).ToList().Count;
                    count = dvCom.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtLogo":
                    List<EntityMark> dvMar = ((List<EntityMark>)gcMark.DataSource);
                    i = dvMar.Where(w => w.Checked == true).ToList().Count;
                    count = dvMar.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtPhysicsGroup":
                    List<EntityDicPubProfession> dvPhy = ((List<EntityDicPubProfession>)gcZuBie.DataSource);
                    i = dvPhy.Where(w => w.Checked == true).ToList().Count;
                    count = dvPhy.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtProfessionalGroup":
                    List<EntityDicPubProfession> dvSep = ((List<EntityDicPubProfession>)gcSepGroups.DataSource);
                    i = dvSep.Where(w => w.Checked == true).ToList().Count;
                    count = dvSep.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtVerifier":
                    List<EntitySysUser> dvDoc = ((List<EntitySysUser>)gcChkDoc.DataSource);
                    i = dvDoc.Where(w => w.Checked == true).ToList().Count;
                    count = dvDoc.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtCensorshipby":
                    List<EntityDicDoctor> dvSendDoc = ((List<EntityDicDoctor>)gcSendDoc.DataSource);
                    i = dvSendDoc.Where(w => w.Checked == true).ToList().Count;
                    count = dvSendDoc.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtOriId":
                    List<EntityDicOrigin> dvOri = ((List<EntityDicOrigin>)gcOriId.DataSource);
                    i = dvOri.Where(w => w.Checked == true).ToList().Count;
                    count = dvOri.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtNo2Andi":
                    List<EntitySysUser> dvAndit = ((List<EntitySysUser>)gcNo2Andi.DataSource);
                    i = dvAndit.Where(w => w.Checked == true).ToList().Count;
                    count = dvAndit.Count;
                    chkAllCheck(i, count);
                    break;
                default:
                    break;
            }
        }


        private void chkAllCheck(int i,int count)
        {
            checkAll.CheckedChanged -= this.checkAll_CheckedChanged;
            if (count == i)
                checkAll.Checked = true;
            else
                checkAll.Checked = false;
            checkAll.CheckedChanged += this.checkAll_CheckedChanged;
        }


        public EntityStatisticsQC getWhere()
        {

            EntityStatisticsQC StatQC = new EntityStatisticsQC();
            int count = 0;
            #region 获取被选中的所有ID
            count = dvIns.Count;
            dvIns = dvIns.Where(i => i.Checked == true).ToList();
            if (dvIns.Count > 0)
            {
                if(dvIns.Count == count)
                {
                    StatQC.ItrAllList = dvIns;
                }
                StatQC.ItrList = dvIns;
            }
            count = dvDep.Count;
            dvDep = dvDep.Where(i => i.Checked == true).ToList();
            if (dvDep.Count > 0)
            {
                if(count == dvDep.Count) { StatQC.DeptAllList = dvDep; }
                StatQC.DeptList = dvDep;
            }
            count = dvDiag.Count;
            dvDiag = dvDiag.Where(i => i.Checked == true).ToList();
            if (dvDiag.Count > 0)
            {
                if (count == dvDiag.Count) { StatQC.DiagAllList = dvDiag; }
                StatQC.DiagList = dvDiag;
            }
            count = dvSam.Count;
            dvSam = dvSam.Where(i => i.Checked == true).ToList();
            if (dvSam.Count > 0)
            {
                if (count == dvSam.Count) { StatQC.SampleAllList = dvSam; }
                StatQC.SampleList = dvSam;
            }
            count = dvPhy.Count;
            dvPhy = dvPhy.Where(i => i.Checked == true).ToList();
            if (dvPhy.Count > 0)
            {
                if (count == dvPhy.Count) { StatQC.PhyAllList = dvPhy; }
                StatQC.PhyList = dvPhy;
            }
            count = dvSep.Count;
            dvSep = dvSep.Where(i => i.Checked == true).ToList();
            if (dvSep.Count > 0)
            {
                if (count == dvSep.Count) { StatQC.SepAllList = dvSep; }
                StatQC.SepList = dvSep;
            }
            count = dvChkDoc.Count;
            dvChkDoc = dvChkDoc.Where(i => i.Checked == true).ToList();
            if (dvChkDoc.Count > 0)
            {
                if (count == dvChkDoc.Count) { StatQC.ChkDocAllList = dvChkDoc; }
                StatQC.ChkDocList = dvChkDoc;
            }
            count = dvNo2Andi.Count;
            dvNo2Andi = dvNo2Andi.Where(i => i.Checked == true).ToList();
            if (dvNo2Andi.Count > 0)
            {
                if (count == dvNo2Andi.Count) { StatQC.AuditAllList = dvNo2Andi; }
                StatQC.AuditList = dvNo2Andi;
            }
            count = dvSendDoc.Count;
            dvSendDoc = dvSendDoc.Where(i => i.Checked == true).ToList();
            if (dvSendDoc.Count > 0)
            {
                if (count == dvSendDoc.Count) { StatQC.SendDocAllList = dvSendDoc; }
                StatQC.SendDocList = dvSendDoc;
            }
            count = dvCom.Count;
            dvCom = dvCom.Where(i => i.Checked == true).ToList();
            if (dvCom.Count > 0)
            {
                if (count == dvCom.Count) { StatQC.CombineAllList = dvCom; }
                StatQC.CombineList = dvCom;
            }
            count = dvOri.Count;
            dvOri = dvOri.Where(i => i.Checked == true).ToList();
            if (dvOri.Count > 0)
            {
                if (count == dvOri.Count) { StatQC.OriginAllList = dvOri; }
                StatQC.OriginList = dvOri;
            }
            dvMark = dvMark.Where(i => i.Checked == true).ToList();
            if (dvMark.Count > 0)
            {
                StatQC.MarkList = dvMark;
            }
            dvSex = dvSex.Where(i => i.Checked == true).ToList();
            if (dvSex.Count > 0)
            {
                StatQC.SexList = dvSex;
            }
            count = dvResRefFlag.Count;
            dvResRefFlag = dvResRefFlag.Where(i => i.Checked == true).ToList();
            if (dvResRefFlag.Count > 0)
            {
                if (count == dvResRefFlag.Count) { StatQC.ResultTipsAllList = dvResRefFlag; }
                StatQC.ResultTipsList = dvResRefFlag;
            }

            count = dvSampStatus.Count;
            dvSampStatus = dvSampStatus.Where(i => i.Checked == true).ToList();
            if (dvSampStatus.Count > 0)
            {
                if (count == dvSampStatus.Count) { StatQC.SampStateAllList = dvSampStatus; }
                StatQC.SampStateList = dvSampStatus;
            }

            count = dvSampRemark.Count;
            dvSampRemark = dvSampRemark.Where(i => i.Checked == true).ToList();
            if (dvSampRemark.Count > 0)
            {
                if (count == dvSampRemark.Count) { StatQC.SampRemarkAllList = dvSampRemark; }
                StatQC.SampRemarkList = dvSampRemark;
            }
            #endregion     

            ProxyStatistical proxy = new ProxyStatistical();
            StatQC = proxy.Service.GetStatQC(dtitem,StatQC);
            return StatQC;
        }


        private void txtCombine_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicCombine> dv = (List<EntityDicCombine>)gcCombine.DataSource;
            if (txtCombine.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtCombine.EditValue.ToString()).ToUpper();
                gcCombine.DataSource = dvCom.Where(i => (i.ComName != null && i.ComName.Contains(where)) || (i.ComPyCode != null && i.ComPyCode.Contains(where)) || (i.ComWbCode != null && i.ComWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gcCombine.DataSource = dvCom;
            }
        }

        private void txtOriId_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicOrigin> dv = gcOriId.DataSource as List<EntityDicOrigin>;
            if (txtOriId.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtOriId.EditValue.ToString()).ToUpper();
                dv = dvOri.Where(i => (i.SrcName != null && i.SrcName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcOriId.DataSource = dv;
             }
            else
            {
                gcOriId.DataSource = dvOri;
            }
        }

        private void txtApparatus_EditValueChanged(object sender, EventArgs e)
        {   ////输入框的编辑值时触发的事件
            gvApparatus.CloseEditor();
            List<EntityDicInstrument> dv = gcApparatus.DataSource as List<EntityDicInstrument>;
            if (txtApparatus.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtApparatus.EditValue.ToString()).ToUpper();
                dv = dvIns.Where(i => (i.ItrName != null && i.ItrName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcApparatus.DataSource = dv;
            }
            else
            {
                gcApparatus.DataSource = dvIns;
            }
        }

        private void txtDivisions_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicPubDept> dv = gcDivisions.DataSource as List<EntityDicPubDept>;
            if (txtDivisions.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtDivisions.EditValue.ToString()).ToUpper();
                dv = dvDep.Where(i => (i.DeptName != null && i.DeptName.Contains(where)) || (i.DeptPyCode != null && i.DeptPyCode.Contains(where)) || (i.DeptWbCode != null && i.DeptWbCode.Contains(where)) || i.Checked == true).ToList();
                gcDivisions.DataSource = dv;
            }
            else
            {
                gcDivisions.DataSource = dvDep;
            }
        }
        private void txtDiagnosis_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicPubIcd> dv = (List<EntityDicPubIcd>)gridControlZD.DataSource;
            if (txtDiagnosis.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtDiagnosis.EditValue.ToString()).ToUpper();
                dv = dvDiag.Where(i => (i.IcdName != null && i.IcdName.Contains(where)) || (i.IcdPyCode != null && i.IcdPyCode.Contains(where)) || (i.IcdWbCode != null && i.IcdWbCode.Contains(where)) || i.Checked == true).ToList();
                gridControlZD.DataSource = dv;
            }
            else
            {
                gridControlZD.DataSource = dvDiag;
            }
        }

        private void txtSpecimens_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicSample> dv = (List<EntityDicSample>)gridControlBB.DataSource;
            if (txtSpecimens.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSpecimens.EditValue.ToString()).ToUpper();
                dv = dvSam.Where(i => (i.SamName != null && i.SamName.Contains(where)) || (i.SamPyCode != null && i.SamPyCode.Contains(where)) || (i.SamWbCode != null && i.SamWbCode.Contains(where)) || i.Checked == true).ToList();
                gridControlBB.DataSource = dv;
            }
            else
            {
                gridControlBB.DataSource = dvSam;
            }
        }

        private void txtGroups_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicPubProfession> dv = (List<EntityDicPubProfession>)gcZuBie.DataSource;
            if (txtGroups.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtGroups.EditValue.ToString()).ToUpper();
                dv = dvPhy.Where(i => (i.ProName != null && i.ProName.Contains(where)) || (i.ProPyCode != null && i.ProPyCode.Contains(where)) || (i.ProWbCode != null && i.ProWbCode.Contains(where)) || i.Checked == true).ToList();
                gcZuBie.DataSource = dv;
            }
            else
            {
                gcZuBie.DataSource = dvPhy;
            }
        }

        private void txtSpeGroups_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicPubProfession> dv = (List<EntityDicPubProfession>)gcSepGroups.DataSource;
            if (txtSpeGroups.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSpeGroups.EditValue.ToString()).ToUpper();
                dv = dvSep.Where(i => (i.ProName != null && i.ProName.Contains(where)) || (i.ProPyCode != null && i.ProPyCode.Contains(where)) || (i.ProWbCode != null && i.ProWbCode.Contains(where)) || i.Checked == true).ToList();
                gcSepGroups.DataSource = dv;
            }
            else
            {
                gcSepGroups.DataSource = dvSep;
            }
        }

        private void btnDelItem_Click(object sender, EventArgs e)
        {
            bsNull.EndEdit();
            if (bsNull.Current != null)
            {
                try
                {
                    EntityObrResult dr = ((EntityObrResult)bsNull.Current);
                    dtitem.Remove(dr);
                    bsNull.ResetBindings(true);
                }
                catch (Exception)
                {
                }
            }
        }


        private void txtChkDoc_EditValueChanged(object sender, EventArgs e)
        {
            List<EntitySysUser> dv = (List<EntitySysUser>)gcChkDoc.DataSource;
            if (txtChkDoc.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtChkDoc.EditValue.ToString()).ToUpper();
                dv = dvChkDoc.Where(i => (i.UserName != null && i.UserName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcChkDoc.DataSource = dv;
            }
            else
            {
                gcChkDoc.DataSource = dvChkDoc;
            }
        }

        private void txtSendDoc_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicDoctor> dv = gcSendDoc.DataSource as List<EntityDicDoctor>;
            if (txtSendDoc.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSendDoc.EditValue.ToString()).ToUpper();
                dv = dvSendDoc.Where(i => (i.DoctorName != null && i.DoctorName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcSendDoc.DataSource = dv;
            }
            else
            {
                gcSendDoc.DataSource = dvSendDoc;
            }
        }
        private void txtMark_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void txtNo2Andi_EditValueChanged(object sender, EventArgs e)
        {
            List<EntitySysUser> dv = (List<EntitySysUser>)gcNo2Andi.DataSource;
            if (txtNo2Andi.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtNo2Andi.EditValue.ToString()).ToUpper();
                dv = dvNo2Andi.Where(i => (i.UserName != null && i.UserName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcNo2Andi.DataSource = dv;
            }
            else
            {
                gcNo2Andi.DataSource = dvNo2Andi;
            }
            
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            bsNull.AddNew();
        }


        #region 张力 2013-5-23


        //仪器---->专业组，物理组，项目组合
        private void gvApparatus_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_instrmt"];  
            List<EntityDicInstrument> dtApp = gcApparatus.DataSource as List<EntityDicInstrument>;
            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);



            List<EntityDicInstrument> drApp = dtApp.Where(i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {
                #region 物理组
                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ItrLabId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ItrLabId);
                    }
                }

                List<EntityDicPubProfession> drPhy = daAll["dtPhyType"] as List<EntityDicPubProfession>;
                drPhy = drPhy.Where(i => i.ProId.Contains(sb.ToString())).ToList();
                List<EntityDicPubProfession> dtResult = daAll["dtPhyType"] as List<EntityDicPubProfession>;
                foreach (EntityDicPubProfession dr in drPhy)
                {
                    dtResult.Add(dr);
                }
                this.gcZuBie.DataSource = dtResult;

                #endregion

                #region 专业组
                sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ItrProId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ItrProId);
                    }
                }
                List<EntityDicPubProfession> drSpe = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                drSpe = drSpe.Where(i => i.ProId.Contains(sb.ToString())).ToList();
                List<EntityDicPubProfession> dsResult = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                foreach (var dr in drSpe)
                {
                    dsResult.Add(dr);
                }
                this.gcSepGroups.DataSource = dsResult;

                #endregion

                #region 项目组合
                sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ItrId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ItrId);
                    }
                }
                List<EntityDicItrCombine> drCom = daAll["dtInsCom"] as List<EntityDicItrCombine>;
                drCom = drCom.Where(i => i.ItrId.Contains(sb.ToString())).ToList();

                sb = new StringBuilder();
                foreach (var item in drCom)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ComId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ComId);
                    }
                }
                List<EntityDicCombine> dtCom = daAll["dtCombine"] as List<EntityDicCombine>;
                dtCom = dtCom.Where(i => i.ComId.Contains(sb.ToString())).ToList();
                List<EntityDicCombine> dtCombine = dtCom;
                foreach (var dr in dtCom)
                {
                    dtCombine.Add(dr);
                }
                this.gcCombine.DataSource = dtCombine;
                #endregion
            }
            else
            {
                List<EntityDicPubProfession> dvPhy = daAll["dtPhyType"] as List<EntityDicPubProfession>;
                gcZuBie.DataSource = dvPhy;

                gcSepGroups.DataSource = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                gcCombine.DataSource = daAll["dtCombine"] as List<EntityDicCombine>;
            }
        }

        //项目组合---->专业组，物理组,项目
        private void gvCombine_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_combine"];
            List<EntityDicCombine> dtApp = gcCombine.DataSource as List<EntityDicCombine>;

            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);
            List<EntityDicCombine> drApp = dtApp.Where( i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {
                #region 物理组

                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ComLabId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ComLabId);
                    }
                }

                List<EntityDicPubProfession> drPhy = daAll["dtPhyType"] as List<EntityDicPubProfession>;
                drPhy = drPhy.Where(i => i.ProId.Contains(sb.ToString())).ToList();
                List<EntityDicPubProfession> dtResult = daAll["dtPhyType"] as List<EntityDicPubProfession>;

                foreach (var dr in drPhy)
                {
                    dtResult.Add(dr);
                }
                this.gcZuBie.DataSource = dtResult;
                #endregion

                #region 专业组
                sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ComPriId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ComPriId);
                    }
                }
                List<EntityDicPubProfession> drSpe = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                drSpe = drSpe.Where(i => i.ProId.Contains(sb.ToString())).ToList();
                List<EntityDicPubProfession> dsResult = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                foreach (var dr in drSpe)
                {
                    dtResult.Add(dr);
                }
                this.gcSepGroups.DataSource = dsResult;
                #endregion

                #region 项目
                sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ComId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ComId);
                    }
                }
                List<EntityDicItmCombine> drComItem = daAll["dtComItem"] as List<EntityDicItmCombine>;
                drComItem = drComItem.Where(i => i.ItmComId.Contains(sb.ToString())).ToList();
                List<EntityDicItmItem> dtItem = daAll["dtItem"] as List<EntityDicItmItem>;
                if (drComItem.Count > 0)
                {
                    sb = new StringBuilder();
                    foreach (var item in drComItem)
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append(item.ItmComId);
                        }
                        else
                        {
                            sb.Append(",");
                            sb.Append(item.ItmComId);
                        }
                    }
                    dtItem = dtItem.Where(i => i.ItmId.Contains(sb.ToString())).ToList();
                }
                else
                {
                    dtItem = new List<EntityDicItmItem>();//如果组合不包含项目,则不查询项目
                }
                
                List<EntityDicItmItem> dsItem = daAll["dtItem"] as List<EntityDicItmItem>;
                foreach (var dr in dtItem)
                {
                    dsItem.Add(dr);
                }
                //this.gcCombine.DataSource = new DataView(dtResult);
                this.repositoryItemLookUpEdit3.DataSource = dsItem;
                #endregion

            }
            else
            {
                List<EntityDicPubProfession> dvPhy = daAll["dtPhyType"] as List<EntityDicPubProfession>;
                gcZuBie.DataSource = dvPhy;

                gcSepGroups.DataSource = daAll["dtSpeType"] as List<EntityDicPubProfession>;
                this.repositoryItemLookUpEdit3.DataSource = bsItem;
            }
        }

        //科室---->医生（申请者）
        private void gvDivisions_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_depart"];
            List<EntityDicPubDept> dtApp = gcDivisions.DataSource as List<EntityDicPubDept>;
            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);
            List<EntityDicPubDept> drApp = dtApp.Where(i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {       
                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.DeptCode);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.DeptCode);
                    }
                }

                List<EntityDicDoctor> drPhy = daAll["dtDoc"] as List<EntityDicDoctor>;
                drPhy = drPhy.Where(i => i.DeptId.Contains(sb.ToString())).ToList();
                List<EntityDicDoctor> dtResult = daAll["dtDoc"] as List<EntityDicDoctor>;
                foreach (var dr in drPhy)
                {
                    dtResult.Add(dr);
                }
                this.gcSendDoc.DataSource = dtResult;

            }
            else
            {
                List<EntityDicDoctor> dvPhy = daAll["dtDoc"] as List<EntityDicDoctor>;
                gcSendDoc.DataSource = dvPhy;
            }
        }

        //样本---->项目
        private void gridViewSample_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_sample"];
            List<EntityDicSample> dtApp = gridControlBB.DataSource as List<EntityDicSample>;

            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);
            List<EntityDicSample> drApp = dtApp.Where(i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {
                #region 项目
                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.SamId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.SamId);
                    }
                }
                List<EntityDicItemSample> drSamItem = daAll["dtItemSam"] as List<EntityDicItemSample>;
                drSamItem = drSamItem.Where(i => i.ItmSamId.Contains(sb.ToString())).ToList();
                if (drSamItem.Count > 0)
                {
                    sb = new StringBuilder();
                    foreach (var item in drSamItem)
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append(item.ItmId);
                        }
                        else
                        {
                            sb.Append(",");
                            sb.Append(item.ItmId);
                        }
                    }
                    List<EntityDicItmItem> drItem = daAll["dtItem"] as List<EntityDicItmItem>;
                    drItem = drItem.Where(i => i.ItmId.Contains(sb.ToString())).ToList();
                    List<EntityDicItmItem> dtResult = daAll["dtItem"] as List<EntityDicItmItem>;
                    foreach (var dr in drItem)
                    {
                        dtResult.Add(dr);
                    }
                    this.repositoryItemLookUpEdit3.DataSource = dtResult;
                }
                else
                {
                    this.repositoryItemLookUpEdit3.DataSource = null;
                }
                #endregion
            }
            else
            {
                this.repositoryItemLookUpEdit3.DataSource = bsItem;
            }

        }

        //物理组---->仪器
        private void gridViewType_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_PhyType"];
            List<EntityDicPubProfession> dtApp = gcZuBie.DataSource as List<EntityDicPubProfession>;

            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);
            List<EntityDicPubProfession> drApp = dtApp.Where(i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {
                #region 项目
                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ProId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ProId);
                    }
                }
                List<EntityDicInstrument> drSamItem = daAll["dtInst"] as List<EntityDicInstrument>;
                drSamItem = drSamItem.Where(i => i.ItrProId.Contains(sb.ToString())).ToList();
                List<EntityDicInstrument> dtResult = daAll["dtInst"] as List<EntityDicInstrument>;
                foreach (var dr in drSamItem)
                {
                    dtResult.Add(dr);
                }
                this.gcApparatus.DataSource =dtResult;

                #endregion

            }
            else
            {
                this.gcApparatus.DataSource = daAll["dtInst"] as List<EntityDicInstrument>;
            }
        }

        //专业组---->仪器
        private void gvSepGroups_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //DataTable dtApp = daAll.Tables["dict_SepType"];
            List<EntityDicPubProfession> dtApp = gcSepGroups.DataSource as List<EntityDicPubProfession>;

            dtApp[e.RowHandle].Checked = Convert.ToBoolean(e.Value);
            List<EntityDicPubProfession> drApp = dtApp.Where(i => i.Checked == true).ToList();
            if (drApp.Count > 0)
            {
                #region 项目
                StringBuilder sb = new StringBuilder();
                foreach (var item in drApp)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(item.ProId);
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(item.ProId);
                    }
                }
                List<EntityDicInstrument> drItr = daAll["dtInst"] as List<EntityDicInstrument>;
                drItr = drItr.Where(i => i.ItrProId.Contains(sb.ToString())).ToList();
                List<EntityDicInstrument> dtResult = daAll["dtInst"] as List<EntityDicInstrument>;
                foreach (var dr in drItr)
                {
                    dtResult.Add(dr);
                }
                this.gcApparatus.DataSource = dtResult;

                #endregion

            }
            else
            {
                this.gcApparatus.DataSource = daAll["dtInst"] as List<EntityDicInstrument>;
            }
        }




        #endregion

        private void txtSampStatus_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicSState> dv = (List<EntityDicSState>)gcSampStatus.DataSource;
            if (txtSampStatus.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSampStatus.EditValue.ToString()).ToUpper();
                dv = dvSampStatus.Where(i => (i.StauName != null && i.StauName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcSampStatus.DataSource = dv;
            }
            else
            {
                gcSampStatus.DataSource = dvSampStatus;
            }
        }

        private void txtSampRemark_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicSampRemark> dv = (List<EntityDicSampRemark>)gcSampRemark.DataSource;
            if (txtSampRemark.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSampRemark.EditValue.ToString()).ToUpper();
                dv = dvSampRemark.Where(i => (i.RemContent != null && i.RemContent.Contains(where))   
                || (i.RemPyCode != null && i.RemPyCode.Contains(where)) || (i.RemWbCode != null && i.RemWbCode.Contains(where)) || i.Checked == true).ToList();
                gcSampRemark.DataSource = dv;
            }
            else
            {
                gcSampRemark.DataSource = dvSampRemark;
            }
        }
    }
}
