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

namespace dcl.client.ananlyse
{
    public partial class FrmAdvanBacQuery : FrmCommon
    {
        public FrmAdvanBacQuery()
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
                case "dtDep":
                    StTableName = "dict_depart";
                    break;
                case "dtSam":
                    StTableName = "dict_sample";
                    break;
                case "dtOri":
                    StTableName = "Dict_origin";
                    break;
                case "dictBacteri":
                    StTableName = "dict_bacteri";
                    break;
                case "dictAntibio":
                    StTableName = "dict_antibio";
                    break;
                case "dictBtype":
                    StTableName = "dict_btype";
                    break;
            }
            #endregion
            return StTableName;
        }

        private void SaveTemplate()
        {

            dtStat = new List<EntityTpTemplate>();

            List<EntityDicMicBacteria> dvBacteri = ((List<EntityDicMicBacteria>)gcApparatus.DataSource);
            dvBacteri = dvBacteri.Where(i => i.Checked == true).ToList();
            string tablename = string.Empty;
            foreach (var item in dvBacteri)
            {
                tablename = ConvertFromTableName("dictBacteri");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicPubDept> dvDep = ((List<EntityDicPubDept>)gcDivisions.DataSource);
            dvDep = dvDep.Where(i => i.Checked == true).ToList();
            foreach (var item in dvDep)
            {
                tablename = ConvertFromTableName("dtDep");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicMicAntibio> dvAntibio = ((List<EntityDicMicAntibio>)gridControlZD.DataSource);
            dvAntibio = dvAntibio.Where(i => i.Checked == true).ToList();
            foreach (var item in dvAntibio)
            {
                tablename = ConvertFromTableName("dictAntibio");
                addRow(item.SpId, tablename, dtStat);
            }
            List<EntityDicMicBacttype> dvBtype = ((List<EntityDicMicBacttype>)gcBtype.DataSource);
            dvBtype = dvBtype.Where(i => i.Checked == true).ToList();
            foreach (var item in dvBtype)
            {
                tablename = ConvertFromTableName("dictBtype");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicSample> dvSam = ((List<EntityDicSample>)gridControlBB.DataSource);
            dvSam = dvSam.Where(i => i.Checked == true).ToList();
            foreach (var item in dvSam)
            {
                tablename = ConvertFromTableName("dtSam");
                addRow(item.SpId, tablename, dtStat);
            }

            List<EntityDicOrigin> dvOri = (List<EntityDicOrigin>)gcOriId.DataSource;
            dvOri = dvOri.Where(i => i.Checked == true).ToList(); ;
            foreach (var item in dvOri)
            {
                tablename = ConvertFromTableName("dtOri");
                addRow(item.SpId, tablename, dtStat);
            }

        }

        private void addRow(string SpId,string tablename, List<EntityTpTemplate> dt)
        {
            EntityTpTemplate template = new EntityTpTemplate();
            template.StType = "BacilliBasicAnalyse";
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
        /// <summary>
        /// 细菌名称
        /// </summary>
        List<EntityDicMicBacteria> dvBacteri = new List<EntityDicMicBacteria>();
        /// <summary>
        /// 科室
        /// </summary>
        List<EntityDicPubDept> dvDep = new List<EntityDicPubDept>();
        /// <summary>
        /// 抗生素
        /// </summary>
        List<EntityDicMicAntibio> dvAntibio = new List<EntityDicMicAntibio>();
        /// <summary>
        /// 标本
        /// </summary>
        List<EntityDicSample> dvSam = new List<EntityDicSample>();
        /// <summary>
        /// 细菌菌类
        /// </summary>
        List<EntityDicMicBacttype> dvBtype = new List<EntityDicMicBacttype>();
        /// <summary>
        /// 病人来源
        /// </summary>
        List<EntityDicOrigin> dvOri = new List<EntityDicOrigin>();
        #endregion
        public void BindGridView(Dictionary<string, object> daAllIn)
        {
            daAll = daAllIn;
            dvBacteri = daAll["dictBacteri"] as List<EntityDicMicBacteria>;
            gcApparatus.DataSource = dvBacteri;

            dvDep = daAll["dtDep"] as List<EntityDicPubDept>;
            gcDivisions.DataSource = dvDep;

            dvAntibio = (daAll["dictAntibio"]) as List<EntityDicMicAntibio>;
            gridControlZD.DataSource = dvAntibio;

            dvSam = (daAll["dtSam"]) as List<EntityDicSample>;
            gridControlBB.DataSource = dvSam;

            dvBtype = (daAll["dictBtype"]) as List<EntityDicMicBacttype>;
            gcBtype.DataSource = dvBtype;

            dvOri = (daAll["dtOri"]) as List<EntityDicOrigin>;
            gcOriId.DataSource = dvOri;
            #region 重置后取消勾选
            if (dtStat != null && dtStat.Count == 0)
            {
                foreach(var item in dvBacteri)
                {
                    item.Checked = false;
                }
                foreach (var item in dvDep)
                {
                    item.Checked = false;
                }
                foreach (var item in dvAntibio)
                {
                    item.Checked = false;
                }
                foreach (var item in dvSam)
                {
                    item.Checked = false;
                }
                foreach (var item in dvBtype)
                {
                    item.Checked = false;
                }
                foreach (var item in dvOri)
                {
                    item.Checked = false;
                }
                #endregion
            }

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
                case "xtFungi":
                    foreach (var item in dvBtype)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcBtype.DataSource = dvBtype;
                    gcBtype.RefreshDataSource();
                    break;
                case "xtBacteria":
                    foreach (var item in dvBacteri)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gcApparatus.DataSource = dvBacteri;
                    gcApparatus.RefreshDataSource();
                    break;
                case "xtAntibiotics":
                    foreach (var item in dvAntibio)
                    {
                        if (item.Checked != i)
                        {
                            item.Checked = i;
                        }
                    }
                    gridControlZD.DataSource = dvAntibio;
                    gridControlZD.RefreshDataSource();
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
                case "xtFungi":
                    List<EntityDicMicBacttype> dvBtype = ((List<EntityDicMicBacttype>)gcBtype.DataSource);
                    i = dvBtype.Where(w => w.Checked == true).ToList().Count;
                    count = dvBtype.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtBacteria":
                    List<EntityDicMicBacteria> dvBacteria = ((List<EntityDicMicBacteria>)gcApparatus.DataSource);
                    i = dvBacteria.Where(w => w.Checked == true).ToList().Count;
                    count = dvBacteria.Count;
                    chkAllCheck(i, count);
                    break;
                case "xtAntibiotics":
                    List<EntityDicMicAntibio> dvAnti = ((List<EntityDicMicAntibio>)gridControlZD.DataSource);
                    i = dvAnti.Where(w => w.Checked == true).ToList().Count;
                    count = dvAnti.Count;
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
                case "xtOri":
                    List<EntityDicOrigin> dvOri = ((List<EntityDicOrigin>)gcOriId.DataSource);
                    i = dvOri.Where(w => w.Checked == true).ToList().Count;
                    count = dvOri.Count;
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


        public EntityStatisticsQC getWhere(string type)
        {
            EntityStatisticsQC StatQC = new EntityStatisticsQC();
            StatQC.BacilliType = type;
            int count = 0;
            #region 获取被选中的所有ID
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

            count = dvOri.Count;
            dvOri = dvOri.Where(i => i.Checked == true).ToList();
            if (dvOri.Count > 0)
            {
                if (count == dvOri.Count) { StatQC.OriginAllList = dvOri; }
                StatQC.OriginList = dvOri;
            }

            count = dvAntibio.Count;
            dvAntibio = dvAntibio.Where(i => i.Checked == true).ToList();
            if (dvAntibio.Count > 0)
            {
                if (count == dvAntibio.Count) { StatQC.AntibioAllList = dvAntibio; }
                StatQC.AntibioList = dvAntibio;
            }
            count = dvBacteri.Count;
            dvBacteri = dvBacteri.Where(i => i.Checked == true).ToList();
            if (dvBacteri.Count > 0)
            {
                if (count == dvBacteri.Count) { StatQC.BacteriaAllList = dvBacteri; }
                StatQC.BacteriaList = dvBacteri;
            }
            count = dvBtype.Count;
            dvBtype = dvBtype.Where(i => i.Checked == true).ToList();
            if (dvBtype.Count > 0)
            {
                if (count == dvBtype.Count) { StatQC.BacttypeAllList = dvBtype; }
                StatQC.BacttypeList = dvBtype;
            }
            #endregion     
            List<EntityObrResult> dtitem = new List<EntityObrResult>();
            ProxyStatistical proxy = new ProxyStatistical();
            StatQC = proxy.Service.GetStatQC(dtitem, StatQC);
            return StatQC;
        }


        private void txtBtype_EditValueChanged(object sender, EventArgs e)
        {
            if (txtBtype.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtBtype.EditValue.ToString()).ToUpper();
                gcBtype.DataSource = dvBtype.Where(i => (i.BtypeCname != null && i.BtypeCname.Contains(where)) || (i.BtypeEname != null && i.BtypeEname.Contains(where)) || (i.BtypePyCode != null && i.BtypePyCode.Contains(where)) || (i.BtypeWbCode != null && i.BtypeWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gcBtype.DataSource = dvBtype;
            }
        }

        private void txtAnti_EditValueChanged(object sender, EventArgs e)
        {
            if (txtAnti.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtAnti.EditValue.ToString()).ToUpper();
                gridControlZD.DataSource = dvAntibio.Where(i => (i.AntCname != null && i.AntCname.Contains(where)) || (i.AntEname != null && i.AntEname.Contains(where)) || (i.AntPyCode != null && i.AntPyCode.Contains(where)) || (i.AntWbCode != null && i.AntWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gridControlZD.DataSource = dvAntibio;
            }
        }

      

        private void txtApparatus_EditValueChanged(object sender, EventArgs e)
        {
            if (txtApparatus.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtApparatus.EditValue.ToString()).ToUpper();
                gcApparatus.DataSource = dvBacteri.Where(i => (i.BacCname != null && i.BacCname.Contains(where)) || (i.BacEname != null && i.BacEname.Contains(where)) || (i.BacPyCode != null && i.BacPyCode.Contains(where)) || (i.BacWbCode != null && i.BacWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gcApparatus.DataSource = dvBacteri;
            }
        }

        private void txtSpecimens_EditValueChanged(object sender, EventArgs e)
        {
            if (txtSpecimens.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtSpecimens.EditValue.ToString()).ToUpper();
                gridControlBB.DataSource = dvSam.Where(i => (i.SamName != null && i.SamName.Contains(where)) || (i.SamPyCode != null && i.SamPyCode.Contains(where)) || (i.SamWbCode != null && i.SamWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gridControlBB.DataSource = dvSam;
            }
        }

        private void txtDivisions_EditValueChanged(object sender, EventArgs e)
        {
            if (txtDivisions.EditValue.ToString() != "")
            {
                string where = SQLFormater.Format(txtDivisions.EditValue.ToString()).ToUpper();
                gcDivisions.DataSource = dvDep.Where(i => (i.DeptName != null && i.DeptName.Contains(where)) || (i.DeptPyCode != null && i.DeptPyCode.Contains(where)) || (i.DeptWbCode != null && i.DeptWbCode.Contains(where)) || i.Checked == true).ToList();
            }
            else
            {
                gcDivisions.DataSource = dvDep;
            }
        }


    }
}
