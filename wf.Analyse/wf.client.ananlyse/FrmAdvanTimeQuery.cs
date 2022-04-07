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
    public partial class FrmAdvanTimeQuery : FrmCommon
    {
        
        public FrmAdvanTimeQuery()
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
            dtStat.Clear();
            SaveTemplate();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveTemplate()
        {

            dtStat = new List<EntityTpTemplate>();
            string tablename = string.Empty;
            FrmAdvanGeneralQuery Advan = new FrmAdvanGeneralQuery();


            List<EntityDicInstrument> dvIns = ((List<EntityDicInstrument>)gcApparatus.DataSource);
            dvIns = dvIns.Where(i => i.Checked == true).ToList();
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

            List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
            dvSam = dvSam.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSam)
            {
                tablename = Advan.ConvertFromTableName("dtSam");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityMark> dvMar = ((List<EntityMark>)gcMark.DataSource);
            dvMar = dvMar.Where(i => i.Checked == true).ToList();
            foreach (var item in dvMar)
            {
                addRow(item.SpId, "dtMark", dtStat);
            }

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

            List<EntitySysUser> dvDoc = ((List<EntitySysUser>)gcChkDoc.DataSource);
            dvDoc = dvDoc.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDoc)
            {
                tablename = Advan.ConvertFromTableName("dtUs");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicDoctor> dvDicDoc = ((List<EntityDicDoctor>)gcSendDoc.DataSource);
            dvDicDoc = dvDicDoc.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDicDoc)
            {
                tablename = Advan.ConvertFromTableName("dtDoc");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
            dvCom = dvCom.Where(i => i.Checked == true).ToList();
            foreach (var item in dvCom)
            {
                tablename = Advan.ConvertFromTableName("dtCombine");
                addRow(item.SpId, tablename, dtStat);
            }
        }

        private void addRow(string SpId, string tablename, List<EntityTpTemplate> dt)
        {
            EntityTpTemplate template = new EntityTpTemplate();
            template.StType = "TimeAnalyse";
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
        /// 申请者
        /// </summary>
        List<EntityDicDoctor> dvSendDoc = new List<EntityDicDoctor>();
        #endregion
        public void BindGridView(Dictionary<string, object> daAllIn)
        {
            daAll = daAllIn;

            daAll = daAllIn;
            dvIns = daAll["dtInst"] as List<EntityDicInstrument>;
            gcApparatus.DataSource = dvIns;

            dvDep = daAll["dtDep"] as List<EntityDicPubDept>;
            gcDivisions.DataSource = dvDep;

            dvSam = (daAll["dtSam"]) as List<EntityDicSample>;
            gridControlBB.DataSource = dvSam;

            dvPhy = (daAll["dtPhyType"]) as List<EntityDicPubProfession>;
            gridControlZuBie.DataSource = dvPhy;

            dvSep = (daAll["dtSpeType"]) as List<EntityDicPubProfession>;
            gcSepGroups.DataSource = dvSep;

            dvSendDoc = (daAll["dtDoc"]) as List<EntityDicDoctor>;
            gcSendDoc.DataSource = dvSendDoc;

            dvChkDoc = (daAll["dtUs"]) as List<EntitySysUser>;
            gcChkDoc.DataSource = dvChkDoc;

            dvCom = (daAll["dtCombine"]) as List<EntityDicCombine>;
            gcCombine.DataSource = dvCom;

            dtitem = (daAll["dtNull"]) as List<EntityObrResult>;
            bsNull.DataSource = dtitem;

            this.bsTiaojian.DataSource = CommonValue.GetTiaojian();

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

            #region 急查的勾选
            if (dtStat.Count > 0)
            {
                foreach (var item in dtStat)
                {
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
                    gridControlZuBie.DataSource = dvPhy;
                    gridControlZuBie.RefreshDataSource();
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
                default:
                    break;
            }
            // txtWhere.Text = getConditions();
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
                case "xtSpecimens":
                    List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
                    i = dvSam.Where(w => w.Checked == true).ToList().Count;
                    count = dvSam.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtLogo":
                    List<EntityMark> dvMar = ((List<EntityMark>)gcMark.DataSource);
                    i = dvMar.Where(w => w.Checked == true).ToList().Count;
                    count = dvMar.Count;
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
                case "xtPortfolio":
                    List<EntityDicCombine> dvCom = ((List<EntityDicCombine>)gcCombine.DataSource);
                    i = dvCom.Where(w => w.Checked == true).ToList().Count;
                    count = dvCom.Count;
                    chkAllCheck(i, count);
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
            dvMark = dvMark.Where(i => i.Checked == true).ToList();
            if (dvMark.Count > 0)
            {
                StatQC.MarkList = dvMark;
            }
            #endregion     

            ProxyStatistical proxy = new ProxyStatistical();
            StatQC = proxy.Service.GetStatQC(dtitem, StatQC);
            return StatQC;
        }


        //public string getConditions()
        //{
        //    string where = "";


        //    DataView dtApp = (DataView)gcApparatus.DataSource;
        //    DataRow[] drApp = dtApp.Table.Select("sp_select=1");
        //    if (drApp.Length > 0)
        //    {
        //        if (drApp.Length == dtApp.Table.Rows.Count)
        //            where += " 仪器:全部 ";
        //        else
        //            where += getString(drApp, " 仪器:('", "itr_name");
        //    }
        //    DataView dtBB = (DataView)gridControlBB.DataSource;
        //    DataRow[] drBB = dtBB.Table.Select("sp_select=1");
        //    if (drBB.Length > 0)
        //    {
        //        if (drBB.Length == dtBB.Table.Rows.Count)
        //            where += " 标本:全部 ";
        //        else
        //            where += getString(drBB, " 标本:('", "sam_name");
        //    }
        //    DataView dtDiv = (DataView)gcDivisions.DataSource;
        //    DataRow[] drDiv = dtDiv.Table.Select("sp_select=1");
        //    if (drDiv.Length > 0)
        //    {
        //        if (drDiv.Length == dtDiv.Table.Rows.Count)
        //            where += " 科室:全部 ";
        //        else
        //            where += getString(drDiv, " 科室:('", "dep_name");
        //    }
        //    DataView dtZB = (DataView)gridControlZuBie.DataSource;
        //    DataRow[] drZB = dtZB.Table.Select("sp_select=1");
        //    if (drZB.Length > 0)
        //    {
        //        if (drZB.Length == dtZB.Table.Rows.Count)
        //            where += " 实验组:全部 ";
        //        else
        //            where += getString(drZB, " 实验组:('", "type_name");
        //    }
        //    DataView dtSZB = (DataView)gcSepGroups.DataSource;
        //    DataRow[] drSZB = dtSZB.Table.Select("sp_select=1");
        //    if (drSZB.Length > 0)
        //    {
        //        if (drSZB.Length == dtSZB.Table.Rows.Count)
        //            where += " 实验组:全部 ";
        //        else
        //            where += getString(drSZB, " 专业组:('", "type_name");
        //    }
        //    DataView dtChkDoc = (DataView)gcChkDoc.DataSource;
        //    DataRow[] drChkDoc = dtChkDoc.Table.Select("sp_select=1");
        //    if (drChkDoc.Length > 0)
        //    {
        //        where += getString(drChkDoc, " and 检验者 in('");
        //    }
        //    DataView dtSendDoc = (DataView)gcSendDoc.DataSource;
        //    DataRow[] drSendDoc = dtSendDoc.Table.Select("sp_select=1");
        //    if (drSendDoc.Length > 0)
        //    {
        //        where += getString(drSendDoc, " and 开单医生 in('");
        //    }
        //    DataView dtMark = (DataView)gcMark.DataSource;
        //    DataRow[] drMark = dtMark.Table.Select("sp_select=1");
        //    if (drMark.Length > 0)
        //    {
        //        where += getString(drMark, " and 标识 in('");
        //    }

        //    //组合
        //    DataView dtComb = (DataView)gcCombine.DataSource;
        //    DataRow[] drComb = dtComb.Table.Select("sp_select=1");
        //    if (drComb.Length > 0)
        //    {
        //        string str_temp_comID = "";

        //        foreach (DataRow drtemp in drComb)
        //        {
        //            if (string.IsNullOrEmpty(str_temp_comID))
        //            {
        //                str_temp_comID = "'" + drtemp["sp_id"].ToString() + "'";
        //            }
        //            else
        //            {
        //                str_temp_comID += ",'" + drtemp["sp_id"].ToString() + "'";
        //            }
        //        }

        //        where += string.Format(" and 组合ID in({0})) ", str_temp_comID);
        //    }

        //    if (dtitem != null)
        //    {
        //        if (dtitem.Rows.Count > 0)
        //        {
        //            if (dtitem.Select("res_itm_ecd <>''").Length > 0)
        //            {
        //                where += getItemWhere();
        //            }

        //        }
        //    }



        //    return where;
        //}

        private string getString(DataRow[] dr, string str, string columns)
        {
            for (int i = 0; i < dr.Length; i++)
            {
                if (i != (dr.Length - 1))
                {
                    str += dr[i][columns].ToString() + "','";
                }
                else
                {
                    str += dr[i][columns].ToString() + "')";
                }
            }
            return str;
        }

        private string getItemWhere()
        {
            string where = " and (";
            if (dtitem != null)
            {
                if (dtitem.Count > 0)
                {
                    List<EntityObrResult> drItem = dtitem.Where(i => i.ItmEname != null && i.ItmEname != "").ToList();
                    for (int i = 0; i < drItem.Count; i++)
                    {
                        if (drItem[i].ItmEname.ToString().Trim() != "")
                        {
                            where += "(res_itm_ecd='" + drItem[i].ItmEname.ToString() + "' ";
                            string parWh = drItem[i].ObrValue2.ToString().Trim();
                            if (parWh != "" && drItem[i].ObrValue.ToString().Trim() != "")
                            {
                                if (parWh != "like" && parWh != "not like")
                                {
                                    where += " and res_cast_chr" + parWh + drItem[i].ObrValue.ToString().Trim();
                                }
                                else
                                {
                                    where += " and res_chr " + parWh + " '%" + drItem[i].ObrValue.ToString().Trim() + "%'";
                                }
                            }
                            if (i != (drItem.Count - 1))
                                where += ") or ";
                            else
                                where += "))";
                        }
                    }
                }
            }
            return where;
        }



        private string getString(DataRow[] dr, string str)
        {
            for (int i = 0; i < dr.Length; i++)
            {
                if (i != (dr.Length - 1))
                {
                    str += dr[i]["sp_id"].ToString() + "','";
                }
                else
                {
                    str += dr[i]["sp_id"].ToString() + "')";
                }
            }
            return str;
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
        private void btnDelItem_Click(object sender, EventArgs e)
        {
            bsNull.EndEdit();
            if (bsNull.Current != null)
            {
                try
                {
                    EntityObrResult dr = (EntityObrResult)bsNull.Current;
                    dtitem.Remove(dr);
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
                dv = dvDep.Where(i => (i.DeptName != null && i.DeptName.Contains(where)) || (i.DeptPyCode != null && i.DeptPyCode.Contains(where.ToUpper())) || (i.DeptWbCode != null && i.DeptWbCode.Contains(where.ToUpper())) || i.Checked == true).ToList();
                gcDivisions.DataSource = dv;
            }
            else
            {
                gcDivisions.DataSource = dvDep;
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
    }
}
