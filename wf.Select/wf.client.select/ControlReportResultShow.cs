using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.resultquery
{
    public partial class ControlReportResultShow : UserControl
    {
        private string words = string.Empty;
        public ControlReportResultShow()
        {
            InitializeComponent();
            xtraTabControl1.SelectedPageChanged += XtraTabControl1_SelectedPageChanged;
        }

        /// <summary>
        /// 普通报告备注信息
        /// </summary>
        private string Str_pat_exp { get; set; }

        /// <summary>
        /// 清空结果表数据
        /// </summary>
        private void clearResultoGv()
        {
            xtraTabControl1.SelectedTabPageIndex = 0;//普通

            bsObrResult.DataSource = null;

            bsMarrowResults.DataSource = null;

            bsBac.DataSource = null;
            bsAnti.DataSource = null;

            bsDesc.DataSource = null;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void Reset()
        {
            clearPat();
            clearResultoGv();
        }
        public void show_load_info(EntityPidReportMain patient, EntityQcResultList listQcResult)
        {
            if (patient == null)
            {
                clearPat();
                clearResultoGv();
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无此患者信息");
                return;
            }
            else
            {
                clearPat();
                //填充患者信息
                FillPatientInfo(patient);
            }

            if (listQcResult == null)
            {
                clearResultoGv();
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无报告信息");
                return;
            }

            clearResultoGv();
            FillResultInfo(listQcResult);


        }

        /// <summary>
        /// 填充患者信息
        /// </summary>
        /// <param name="dtPatData"></param>
        private void FillPatientInfo(EntityPidReportMain patient)
        {
            if (true)
            {
                bsPatient.DataSource = patient;
                Str_pat_exp = patient.RepRemark;
            }
            else
            {

            }
        }

        /// <summary>
        /// 填充结果信息
        /// </summary>
        /// <param name="dsResulto"></param>
        private void FillResultInfo(EntityQcResultList listQcResult)
        {
            if (bsPatient.Current == null)
            {
                return;
            }
            EntityPidReportMain report = bsPatient.Current as EntityPidReportMain;
            if (report != null && report.ItrReportType == "7") // 骨髓
            {
                xtraTabControl1.SelectedTabPageIndex = 3;//骨髓
                bsMarrowResults.DataSource = listQcResult.listResulto;
                rtbPatMarrow_exp.Text = report.RepRemark;
                rtbPatComment_exp.Text = report.RepComment;
                rtbPatSpecial_exp.Text = report.RepDiscribe;
            }
            else
            {

                if (listQcResult.listAnti.Count > 0 || listQcResult.listBact.Count > 0)
                {
                    xtraTabControl1.SelectedTabPageIndex = 1;//药敏

                    bsBac.DataSource = listQcResult.listBact;

                    if (listQcResult.listBact.Count > 0)
                    {
                        rtbBar_scripe.Text = listQcResult.listBact[0].ObrRemark;//备注信息
                    }

                    //过滤数据，应滨海需求，anr_row_flag为N时,则不显示
                    if (listQcResult.listAnti.Count > 0)
                    {
                        listQcResult.listAnti.RemoveAll(i => i.ObrCountFlag == "N");
                    }

                    bsAnti.DataSource = listQcResult.listAnti;

                    rtbBar_exp.Text = Str_pat_exp;//细菌-报告评价

                }
                else
                {
                    if (listQcResult.listDesc.Count > 0)
                    {
                        xtraTabControl1.SelectedTabPageIndex = 2;//描述
                        bsDesc.DataSource = listQcResult.listDesc;
                    }
                    else if (listQcResult.listResulto.Count > 0)
                    {
                        xtraTabControl1.SelectedTabPageIndex = 0;//普通

                        bsObrResult.DataSource = listQcResult.listResulto;

                        grColObrResult.Caption = "结果";
                        gvResulto.Columns[3].Visible = true;
                        gvResulto.Columns[4].Visible = true;

                        string chr2 = listQcResult.listResulto[0].ObrValue3;
                        if (!string.IsNullOrEmpty(chr2) && chr2.Trim() != string.Empty)
                        {
                            grColObrResult.Caption = "OD值";
                            grColTips.Visible = true;
                            grColUnit.Visible = true;
                        }
                        else
                        {
                            grColObrResult.Caption = "结果";
                            grColTips.Visible = false;
                            grColUnit.Visible = false;
                        }
                        if (true)//显示异常提示和单位
                        {
                            grColTips.Visible = true;
                            grColUnit.Visible = true;
                        }
                        rtbPat_exp.Text = Str_pat_exp;
                    }
                    else
                    {
                        clearResultoGv();

                        lis.client.control.MessageDialog.ShowAutoCloseDialog("无报告信息");
                    }
                }

            }
        }

        /// <summary>
        /// 清空患者信息
        /// </summary>
        private void clearPat()
        {
            bsPatient.DataSource = new List<EntityPidReportMain>();

            Str_pat_exp = "";
            rtbPat_exp.Text = "";

            rtbBar_scripe.Text = "";
            rtbBar_exp.Text = "";
        }


        /// <summary>
        /// 复制表格信息gvResulto
        /// </summary>
        private void Copy_gvResulto()
        {
            if (gvResulto.GetSelectedRows().Length > 0)
            {
                if (gvResulto.OptionsClipboard.AllowCopy != DevExpress.Utils.DefaultBoolean.False)
                {
                    try
                    {
                        gvResulto.CopyToClipboard();

                        string strtext = Clipboard.GetText();

                        if (!string.IsNullOrEmpty(strtext))
                        {
                            Clipboard.SetText(strtext);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 复制表格信息gvBac
        /// </summary>
        private void Copy_gvBac()
        {
            if (gvBac.GetSelectedRows().Length > 0)
            {
                if (gvBac.OptionsClipboard.AllowCopy != DevExpress.Utils.DefaultBoolean.False)
                {
                    try
                    {
                        gvBac.CopyToClipboard();

                        string strtext = Clipboard.GetText();

                        if (!string.IsNullOrEmpty(strtext))
                        {
                            Clipboard.SetText(strtext);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 复制表格信息gvAnti
        /// </summary>
        private void Copy_gvAnti()
        {
            if (gvAnti.GetSelectedRows().Length > 0)
            {
                if (gvAnti.OptionsClipboard.AllowCopy != DevExpress.Utils.DefaultBoolean.False)
                {
                    try
                    {
                        gvAnti.CopyToClipboard();

                        string strtext = Clipboard.GetText();

                        if (!string.IsNullOrEmpty(strtext))
                        {
                            Clipboard.SetText(strtext);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 复制表格信息gvSmear
        /// </summary>
        private void Copy_gvSmear()
        {
            if (gvSmear.GetSelectedRows().Length > 0)
            {
                if (gvSmear.OptionsClipboard.AllowCopy != DevExpress.Utils.DefaultBoolean.False)
                {
                    try
                    {
                        gvSmear.CopyToClipboard();

                        string strtext = Clipboard.GetText();

                        if (!string.IsNullOrEmpty(strtext))
                        {
                            Clipboard.SetText(strtext);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        private void gvResulto_KeyUp(object sender, KeyEventArgs e)
        {
            //按下ctrl+c或c键时复制表格信息
            if (e.Control && e.KeyCode == Keys.C || e.KeyCode == Keys.C)
            {
                Copy_gvResulto();
            }
        }

        private void gvBac_KeyUp(object sender, KeyEventArgs e)
        {
            //按下ctrl+c或c键时复制表格信息
            if (e.Control && e.KeyCode == Keys.C || e.KeyCode == Keys.C)
            {
                Copy_gvBac();
            }
        }

        private void gvAnti_KeyUp(object sender, KeyEventArgs e)
        {
            //按下ctrl+c或c键时复制表格信息
            if (e.Control && e.KeyCode == Keys.C || e.KeyCode == Keys.C)
            {
                Copy_gvAnti();
            }
        }

        private void gvSmear_KeyUp(object sender, KeyEventArgs e)
        {
            //按下ctrl+c或c键时复制表格信息
            if (e.Control && e.KeyCode == Keys.C || e.KeyCode == Keys.C)
            {
                Copy_gvSmear();
            }
        }

        private void tsmCopyA_Click(object sender, EventArgs e)
        {
            Copy_gvResulto();
        }

        private void tsmCopyB_Click(object sender, EventArgs e)
        {
            Copy_gvBac();
        }

        private void tsmCopyC_Click(object sender, EventArgs e)
        {
            Copy_gvAnti();
        }

        private void tsmCopyD_Click(object sender, EventArgs e)
        {
            Copy_gvSmear();
        }

        private void btn_copy_Click(object sender, EventArgs e)
        {
            string strResult = "";
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                if (bsObrResult.DataSource != null)
                {

                    List<EntityObrResult> results = bsObrResult.DataSource as List<EntityObrResult>;


                    foreach (EntityObrResult dr in results)
                    {
                        if (dr.IsSelected)
                        {
                            strResult += dr.ItmEname + "  " + dr.ObrValue + " " + dr.ObrUnit + ", ";
                        }
                    }
                    strResult = strResult.TrimEnd().TrimEnd(',');
                }

            }
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                if (bsDesc.DataSource != null)
                {

                    List<EntityObrResultDesc> listDesc = bsDesc.DataSource as List<EntityObrResultDesc>;

                    foreach (EntityObrResultDesc dr in listDesc)
                    {
                        if (dr.Selected)
                        {
                            strResult += dr.ObrValue + "  " + dr.PositiveFlag.ToString() + ", ";
                        }
                    }
                    strResult = strResult.TrimEnd().TrimEnd(',');
                }
            }

            Clipboard.SetDataObject(strResult);
        }


        private void XtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0 || xtraTabControl1.SelectedTabPageIndex == 2)
            {
                sbtnCopy.Visible = true;
            }
            else
            {
                sbtnCopy.Visible = false;
            }
        }


        private void ControlReportResultShow_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.sbtnCopy.Click += new System.EventHandler(this.btn_copy_Click);

            words = UserInfo.GetSysConfigValue("ResultNoticeWhenWordsAre");

            clearPat();
            clearResultoGv();
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Select_SerialNumber") == "是")
            {
                label18.Text = "检验者";
                label19.Visible = false;
                label21.Visible = false;
                label23.Visible = false;
                lblSH.Visible = false;
                lblSHdate.Visible = false;
                lblSQdate.Visible = false;
            }
        }

        private void gvResulto_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (gvResulto.RowCount != 0)
            {

                for (int i = 0; i < this.gvResulto.RowCount; i++)
                {
                    EntityObrResult dr = gvResulto.GetRow(i) as EntityObrResult;

                    string str = dr.ResPrompt.ToString();

                    if (!string.IsNullOrEmpty(words))
                    {
                        string[] wordsArray = words.Split(',');
                        foreach (string word in wordsArray)
                        {
                            if (dr.ObrValue.ToString().Replace(" ", "").Contains(word))
                            {
                                if (e.RowHandle == i)
                                    e.Appearance.ForeColor = Color.Red;
                            }
                        }
                    }

                    if (str.Contains("↑") || str.Contains("危") || str.Contains("*"))
                    {
                        if (e.RowHandle == i)
                            e.Appearance.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (str.Contains("↓"))
                    {
                        if (e.RowHandle == i)
                            e.Appearance.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {

                    }

                }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.gvResulto.RowCount; i++)
            {
                ((EntityObrResult)this.gvResulto.GetRow(i)).IsSelected = checkEdit1.Checked;
            }
            gcApparatus.RefreshDataSource();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.gvSmear.RowCount; i++)
            {
                ((EntityObrResultDesc)this.gvSmear.GetRow(i)).Selected = checkEdit2.Checked;
            }
            gridControl3.RefreshDataSource();
        }
    }
}
