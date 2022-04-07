using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.entity;
using System.Linq;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.common;

namespace dcl.client.statistical
{
    public partial class FrmReagentGeneralQuery : FrmCommon
    {
        
        public FrmReagentGeneralQuery()
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

        public string ConvertFromTableName(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) return string.Empty;
            string StTableName = string.Empty;
            #region 表名转换
            switch (tablename)
            {
                case "dtReaSup":
                    StTableName = "dict_reasup";
                    break;
                case "dtReagent":
                    StTableName = "dict_reagent";
                    break;
                case "dtGroup":
                    StTableName = "dict_group";
                    break;
                case "dtPdt":
                    StTableName = "dict_pdt";
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
                    StTableName = "dict_pdt_remarks";
                    break;
            }
            #endregion
            return StTableName;
        }

        private void SaveTemplate()
        {
            dtStat = new List<EntityTpTemplate>();
            List<EntityDicReaSupplier> dvReaSup = ((List<EntityDicReaSupplier>)gcApparatus.DataSource);
            dvReaSup = dvReaSup.Where(i => i.Checked == true).ToList();
            string tablename = string.Empty;
            foreach (var item in dvReaSup)
            {
                tablename = ConvertFromTableName("dtReaSup");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityReaSetting> dvRea = ((List<EntityReaSetting>)gcReagent.DataSource);
            dvRea = dvRea.Where(i => i.Checked == true).ToList();
            foreach (var item in dvRea)
            {
                tablename = ConvertFromTableName("dtReagent");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicReaGroup> dvGroup = ((List<EntityDicReaGroup>)gcGroup.DataSource);
            dvGroup = dvGroup.Where(i => i.Checked == true).ToList();
            foreach (var item in dvGroup)
            {
                tablename = ConvertFromTableName("dtGroup");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicReaProduct> dvPdt = ((List<EntityDicReaProduct>)gcPdt.DataSource);
            dvPdt = dvPdt.Where(i => i.Checked == true).ToList();
            foreach (var item in dvPdt)
            {
                tablename = ConvertFromTableName("dtPdt");
                addRow(item.SpId, tablename, dtStat);
            }            
        }

        private void addRowItem(List<EntityObrResult> dv, List<EntityTpTemplate> dt)
        {
            List<EntityObrResult> dr = dv.Where(i => i.ItmEname != null || i.ItmEname != "").ToList();
            foreach (EntityObrResult drSt in dr)
            {
                EntityTpTemplate template = new EntityTpTemplate();
                template.StType = "ReagentStatistics";
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
                template.StType = "ReagentStatistics";
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
        /// 供货商
        /// </summary>
        List<EntityDicReaSupplier> dvReaSup = new List<EntityDicReaSupplier>();
        /// <summary>
        /// 试剂
        /// </summary>
        List<EntityReaSetting> dvRea = new List<EntityReaSetting>();
        /// <summary>
        /// 组别
        /// </summary>
        List<EntityDicReaGroup> dvGroup = new List<EntityDicReaGroup>();
        /// <summary>
        /// 生产厂家
        /// </summary>
        List<EntityDicReaProduct> dvPdt = new List<EntityDicReaProduct>();
        
        /// <summary>
        /// 检验者
        /// </summary>
        List<EntitySysUser> dvChkDoc = new List<EntitySysUser>();
        
        #endregion
        public void BindGridView(Dictionary<string, object> daAllIn)
        {
            daAll = daAllIn;
            dvReaSup = daAll["dtReaSup"] as List<EntityDicReaSupplier>;
            gcApparatus.DataSource = dvReaSup;

            dvRea = daAll["dtReagent"] as List<EntityReaSetting>;
            gcReagent.DataSource = dvRea;

            dvGroup = (daAll["dtGroup"]) as List<EntityDicReaGroup>;
            gcGroup.DataSource = dvGroup;

            dvPdt = (daAll["dtPdt"]) as List<EntityDicReaProduct>;
            gcPdt.DataSource = dvPdt;

            dvChkDoc = (daAll["dtUs"]) as List<EntitySysUser>;
            gcChkDoc.DataSource = dvChkDoc;

            #region 重置后取消勾选
            if (dtStat != null && dtStat.Count == 0)
            {
                foreach (var item in dvReaSup)
                {
                    item.Checked = false;
                }
                foreach (var item in dvRea)
                {
                    item.Checked = false;
                }
                foreach (var item in dvGroup)
                {
                    item.Checked = false;
                }
                
                foreach (var item in dvPdt)
                {
                    item.Checked = false;
                }
               
                foreach (var item in dvChkDoc)
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
                case "xtReaSup":
                    foreach(var item in dvReaSup)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcApparatus.DataSource = dvReaSup;
                    gcApparatus.RefreshDataSource();
                    break;
                case "xtReagent":
                    foreach (var item in dvRea)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcReagent.DataSource = dvRea;
                    gcReagent.RefreshDataSource();
                    break;
                case "xtGroup":
                    foreach (var item in dvGroup)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcGroup.DataSource = dvGroup;
                    gcGroup.RefreshDataSource();
                    break;
                case "xtPdt":
                    foreach (var item in dvPdt)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcPdt.DataSource = dvPdt;
                    gcPdt.RefreshDataSource();
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
                case "xtReaSup":
                    List<EntityDicReaSupplier> dvReaSup = ((List<EntityDicReaSupplier>)gcApparatus.DataSource);
                    i = dvReaSup.Where(w => w.Checked == true).ToList().Count;
                    count = dvReaSup.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtReagent":
                    List<EntityReaSetting> dvRea = ((List<EntityReaSetting>)gcReagent.DataSource);
                    i = dvRea.Where(w => w.Checked == true).ToList().Count;
                    count = dvRea.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtGroup":
                    List<EntityDicReaGroup> dvGroup = ((List<EntityDicReaGroup>)gcGroup.DataSource);
                    i = dvGroup.Where(w => w.Checked == true).ToList().Count;
                    count = dvGroup.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtPdt":
                    List<EntityDicReaProduct> dvPdt = ((List<EntityDicReaProduct>)gcPdt.DataSource);
                    i = dvPdt.Where(w => w.Checked == true).ToList().Count;
                    count = dvPdt.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtVerifier":
                    List<EntitySysUser> dvDoc = ((List<EntitySysUser>)gcChkDoc.DataSource);
                    i = dvDoc.Where(w => w.Checked == true).ToList().Count;
                    count = dvDoc.Count;
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
            count = dvReaSup.Count;
            dvReaSup = dvReaSup.Where(i => i.Checked == true).ToList();
            if (dvReaSup.Count > 0)
            {
                if(dvReaSup.Count == count)
                {
                    StatQC.ReaSupAllList = dvReaSup;
                }
                StatQC.ReaSupList = dvReaSup;
            }
            count = dvRea.Count;
            dvRea = dvRea.Where(i => i.Checked == true).ToList();
            if (dvRea.Count > 0)
            {
                if(count == dvRea.Count) { StatQC.ReagentAllList = dvRea; }
                StatQC.ReagentList = dvRea;
            }
            count = dvGroup.Count;
            dvGroup = dvGroup.Where(i => i.Checked == true).ToList();
            if (dvGroup.Count > 0)
            {
                if (count == dvGroup.Count) { StatQC.GroupAllList = dvGroup; }
                StatQC.GroupList = dvGroup;
            }
            count = dvPdt.Count;
            dvPdt = dvPdt.Where(i => i.Checked == true).ToList();
            if (dvPdt.Count > 0)
            {
                if (count == dvPdt.Count) { StatQC.PdtAllList = dvPdt; }
                StatQC.PdtList = dvPdt;
            }
            
            count = dvChkDoc.Count;
            dvChkDoc = dvChkDoc.Where(i => i.Checked == true).ToList();
            if (dvChkDoc.Count > 0)
            {
                if (count == dvChkDoc.Count) { StatQC.ChkDocAllList = dvChkDoc; }
                StatQC.ChkDocList = dvChkDoc;
            }
            
            #endregion     

            ProxyStatistical proxy = new ProxyStatistical();
            StatQC = proxy.Service.GetStatQC(dtitem,StatQC);
            return StatQC;
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

        private void txtApparatus_EditValueChanged(object sender, EventArgs e)
        {   ////输入框的编辑值时触发的事件
            gvApparatus.CloseEditor();
            List<EntityDicReaSupplier> dv = gcApparatus.DataSource as List<EntityDicReaSupplier>;
            if (txtApparatus.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtApparatus.EditValue.ToString()).ToUpper();
                dv = dvReaSup.Where(i => (i.Rsupplier_name != null && i.Rsupplier_name.Contains(where)) || i.Checked == true).ToList();
                gcApparatus.DataSource = dv;
            }
            else
            {
                gcApparatus.DataSource = dvReaSup;
            }
        }

        private void txtReagent_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityReaSetting> dv = gcReagent.DataSource as List<EntityReaSetting>;
            if (txtReagent.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtReagent.EditValue.ToString()).ToUpper();
                dv = dvRea.Where(i => (i.Drea_name != null && i.Drea_name.Contains(where)) || i.Checked == true).ToList();
                gcReagent.DataSource = dv;
            }
            else
            {
                gcReagent.DataSource = dvRea;
            }
        }
        private void txtGroup_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicReaGroup> dv = (List<EntityDicReaGroup>)gcGroup.DataSource;
            if (txtGroup.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtGroup.EditValue.ToString()).ToUpper();
                dv = dvGroup.Where(i => (i.Rea_group != null && i.Rea_group.Contains(where)) || i.Checked == true).ToList();
                gcGroup.DataSource = dv;
            }
            else
            {
                gcGroup.DataSource = dvGroup;
            }
        }

        private void txtPdt_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicReaProduct> dv = (List<EntityDicReaProduct>)gcPdt.DataSource;
            if (txtPdt.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtPdt.EditValue.ToString()).ToUpper();
                dv = dvPdt.Where(i => (i.Rpdt_name != null && i.Rpdt_name.Contains(where)) || i.Checked == true).ToList();
                gcPdt.DataSource = dv;
            }
            else
            {
                gcPdt.DataSource = dvPdt;
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
       
        private void txtMark_EditValueChanged(object sender, EventArgs e)
        {

        }
        
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            bsNull.AddNew();
        }

    }
}
