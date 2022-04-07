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
using dcl.client.statistical;

namespace dcl.client.ananlyse
{
    public partial class FrmAdvanBasicQuery : FrmCommon
    {
        FrmDateBasicAnalyse pForm;
        public FrmAdvanBasicQuery(FrmDateBasicAnalyse frm)
        {
            InitializeComponent();

            pForm = frm;
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

        private void SaveTemplate()
        {
            FrmAdvanGeneralQuery Advan = new FrmAdvanGeneralQuery();
            dtStat = new List<EntityTpTemplate>();
            List<EntityDicInstrument> dvIns = ((List<EntityDicInstrument>)gcApparatus.DataSource);
            dvIns = dvIns.Where(i => i.Checked == true).ToList();
            string tablename = string.Empty;
            foreach (var item in dvIns)
            {
                tablename = Advan.ConvertFromTableName("dtInst");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubDept> dvDep = ((List<EntityDicPubDept>)gcDivisions.DataSource);
            dvDep = dvDep.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDep)
            {
                tablename = Advan.ConvertFromTableName("dtDep");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubIcd> dvDiag = ((List<EntityDicPubIcd>)gridControlZD.DataSource);
            dvDiag = dvDiag.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDiag)
            {
                tablename = Advan.ConvertFromTableName("dtDiag");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
            dvSam = dvSam.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSam)
            {
                tablename = Advan.ConvertFromTableName("dtSam");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
            dvCom = dvCom.Where(i => i.Checked == true).ToList();
            foreach (var item in dvCom)
            {
                tablename = Advan.ConvertFromTableName("dtCombine");
                addRow(item.SpId, tablename, dtStat);
            }


            List<EntityObrResult> dvItem = (List<EntityObrResult>)bsNull.DataSource;
            addRowItem(dvItem, dtStat);

            List<EntityDicPubProfession> dvPhy = ((List<EntityDicPubProfession>)gridControlZuBie.DataSource);
            dvPhy = dvPhy.Where(i => i.Checked == true).ToList();
            foreach (var item in dvPhy)
            {
                tablename = Advan.ConvertFromTableName("dtPhyType");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubProfession> dvSep = ((List<EntityDicPubProfession>)gcSepGroups.DataSource);
            dvSep = dvSep.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSep)
            {
                tablename = Advan.ConvertFromTableName("dtSpeType");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntityDicOrigin> dvOri = ((List<EntityDicOrigin>)gcOriId.DataSource);
            dvOri = dvOri.Where(i => i.Checked == true).ToList();
            foreach (var item in dvOri)
            {
                tablename = Advan.ConvertFromTableName("dtOri");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicSState> listState = new List<EntityDicSState>();
            listState = dvState.Where(i => i.Checked == true).ToList();
            foreach (var item in listState)
            {
                tablename = Advan.ConvertFromTableName("dictSampStatus");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicSampRemark> listRemark = new List<EntityDicSampRemark>();
            listRemark = dvRemark.Where(i => i.Checked == true).ToList();
            foreach (var item in listRemark)
            {
                tablename = Advan.ConvertFromTableName("dictSampRemark");
                addRow(item.SpId, tablename, dtStat);
            }

        }

        private void addRowItem(List<EntityObrResult> dv, List<EntityTpTemplate> dt)
        {
            List<EntityObrResult> dr = dv.Where(i => i.ItmEname != null || i.ItmEname != "").ToList();
            foreach (EntityObrResult drSt in dr)
            {
                EntityTpTemplate template = new EntityTpTemplate();
                template.StType = "DateBasicAnalyse";
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

        private void addRow(string SpId, string tablename, List<EntityTpTemplate> dt)
        {
            EntityTpTemplate template = new EntityTpTemplate();
            template.StType = "DateBasicAnalyse";
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
        #region 全局变量
        private DataTable dtNull = new DataTable();
        List<EntityObrResult> dtitem = null;

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
        /// 病人来源
        /// </summary>
        List<EntityDicOrigin> dvOri = new List<EntityDicOrigin>();
        #endregion
        /// <summary>
        /// 项目
        /// </summary>
        List<EntityDicItmItem> dtItem = new List<EntityDicItmItem>();
        /// <summary>
        /// 标本状态
        /// </summary>
        List<EntityDicSState> dvState = new List<EntityDicSState>();

        /// <summary>
        /// 标本备注
        /// </summary>
        List<EntityDicSampRemark> dvRemark = new List<EntityDicSampRemark>();
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
            this.gridControlZuBie.DataSource = dvPhy;

            dvSep = (daAll["dtSpeType"]) as List<EntityDicPubProfession>;
            gcSepGroups.DataSource = dvSep;

            dvCom = (daAll["dtCombine"]) as List<EntityDicCombine>;
            gcCombine.DataSource = dvCom;

            dtitem = (daAll["dtNull"]) as List<EntityObrResult>;
            bsNull.DataSource = dtitem;

            dvState = (daAll["dictSampStatus"]) as List<EntityDicSState>;
            bsSampState.DataSource = dvState;

            dvRemark = (daAll["dictSampRemark"]) as List<EntityDicSampRemark>;
            bsSampRemark.DataSource = dvRemark;

            //项目加载时判断是否加载启用停用
            dvOri = (daAll["dtOri"]) as List<EntityDicOrigin>;
            gcOriId.DataSource = dvOri;

            dtItem = (daAll["dtItem"]) as List<EntityDicItmItem>;

            if (radioGroup_Item.SelectedIndex == 1)
            {
                dtItem = dtItem.Where(i => i.ItmDelFlag == "0").ToList();
            }
            else if (radioGroup_Item.SelectedIndex == 2)
            {
                dtItem = dtItem.Where(i => i.ItmDelFlag == "1").ToList();
            }
            this.bsItem.DataSource = dtItem;

            this.bsTiaojian.DataSource = CommonValue.GetTiaojian();

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
                foreach (var item in dvOri)
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
                    foreach (var item in dvIns)
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
                case "xtPhysicsGroup":
                    foreach (var item in dvPhy)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gridControlZuBie.DataSource = dvPhy;
                    gridControlZuBie.RefreshDataSource();
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
                case "xtProfessionalGroup":
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
                case "xtOri":
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
                case "xtSampState":
                    foreach (var item in dvState)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSampState.DataSource = dvState;
                    gcSampState.RefreshDataSource();
                    break;
                case "xtSampRemark":
                    foreach (var item in dvRemark)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcSampRemark.DataSource = dvRemark;
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
                case "xtPortfolio":
                    List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
                    i = dvCom.Where(w => w.Checked == true).ToList().Count;
                    count = dvCom.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtSpecimens":
                    List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
                    i = dvSam.Where(w => w.Checked == true).ToList().Count;
                    count = dvSam.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtPhysicsGroup":
                    List<EntityDicPubProfession> dvPhy = ((List<EntityDicPubProfession>)gridControlZuBie.DataSource);
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
                case "xtOri":
                    List<EntityDicOrigin> dvOri = ((List<EntityDicOrigin>)gcOriId.DataSource);
                    i = dvOri.Where(w => w.Checked == true).ToList().Count;
                    count = dvOri.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtItem":
                    List<EntityDicItmItem> ItemList = new List<EntityDicItmItem>();
                    if (radioGroup_Item.SelectedIndex == 1)
                    {
                        ItemList = dtItem.Where(r => r.ItmDelFlag == "0").ToList();
                    }
                    else if (radioGroup_Item.SelectedIndex == 2)
                    {
                        ItemList = dtItem.Where(r => r.ItmDelFlag == "1").ToList();
                    }
                    this.bsItem.DataSource = ItemList;
                    break;
                default:
                    break;
            }
        }


        private void chkAllCheck(int i, int count)
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
                if (dvIns.Count == count)
                {
                    StatQC.ItrAllList = dvIns;
                }
                StatQC.ItrList = dvIns;
            }
            count = dvDep.Count;
            dvDep = dvDep.Where(i => i.Checked == true).ToList();
            if (dvDep.Count > 0)
            {
                if (count == dvDep.Count) { StatQC.DeptAllList = dvDep; }
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

            count = dvState.Count;
            List<EntityDicSState> listState = dvState.Where(i => i.Checked == true).ToList();
            if (listState.Count > 0)
            {
                if (count == listState.Count) { StatQC.SampStateAllList = dvState; }
                StatQC.SampStateList = listState;
            }

            count = dvRemark.Count;
            List<EntityDicSampRemark> listRemark = dvRemark.Where(i => i.Checked == true).ToList();
            if (listRemark.Count > 0)
            {
                if (count == listRemark.Count) { StatQC.SampRemarkAllList = dvRemark; }
                StatQC.SampRemarkList = listRemark;
            }
            #endregion     

            ProxyStatistical proxy = new ProxyStatistical();
            StatQC = proxy.Service.GetStatQC(dtitem, StatQC);
            return StatQC;
        }

        private void radioGroup_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            //项目加载时判断是否加载启用停用

            List<EntityDicItmItem> ItemList = new List<EntityDicItmItem>();

            if (radioGroup_Item.SelectedIndex == 1)
            {
                ItemList = dtItem.Where(i => i.ItmDelFlag == "0").ToList();
            }
            else if (radioGroup_Item.SelectedIndex == 2)
            {
                ItemList = dtItem.Where(i => i.ItmDelFlag == "1").ToList();
            }
            else
            {
                ItemList = dtItem;
            }

            this.bsItem.DataSource = ItemList;

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
        {
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
            List<EntityDicPubProfession> dv = (List<EntityDicPubProfession>)gridControlZuBie.DataSource;
            if (txtGroups.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtGroups.EditValue.ToString()).ToUpper();
                dv = dvPhy.Where(i => (i.ProName != null && i.ProName.Contains(where)) || (i.ProPyCode != null && i.ProPyCode.Contains(where)) || (i.ProWbCode != null && i.ProWbCode.Contains(where)) || i.Checked == true).ToList();
                gridControlZuBie.DataSource = dv;
            }
            else
            {
                gridControlZuBie.DataSource = dvPhy;
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
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            bsNull.AddNew();
        }

        private void txtSampState_EditValueChanged(object sender, EventArgs e)
        {
            //  List<EntityDicSState> dv = (List<EntityDicSState>)gvSampState.DataSource;
            if (txtSampState.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSampState.EditValue.ToString()).ToUpper();
                List<EntityDicSState> dv = dvState.Where(i => (i.StauName != null && i.StauName.Contains(where)) || (i.PyCode != null && i.PyCode.Contains(where)) || (i.WbCode != null && i.WbCode.Contains(where)) || i.Checked == true).ToList();
                gcSampState.DataSource = dv;
            }
            else
            {
                gcSampState.DataSource = dvState;
            }
        }

        private void txtSampRemark_EditValueChanged(object sender, EventArgs e)
        {
            if (txtSampRemark.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSampRemark.EditValue.ToString()).ToUpper();
                List<EntityDicSampRemark> dv = dvRemark.Where(i => (i.RemContent != null && i.RemContent.Contains(where))
                 || (i.RemPyCode != null && i.RemPyCode.Contains(where)) || (i.RemWbCode != null && i.RemWbCode.Contains(where)) || i.Checked == true).ToList();
                gcSampRemark.DataSource = dv;
            }
            else
            {
                gcSampRemark.DataSource = dvRemark;
            }
        }
    }
}
