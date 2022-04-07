using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.frame;
using dcl.client.common;
using DevExpress.XtraExport;
using dcl.entity;

namespace dcl.client.result.CommonPatientInput
{
    public partial class AuditCheckResultViwer : FrmCommon
    {
        #region CheckBoxOnGridHeader

        void gridViewPatientList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.FieldName == "Selected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {


            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;

            info = gridheadercheckbox.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = gridheadercheckbox.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }
        private bool bGridHeaderCheckBoxState = false;
        protected virtual void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {

            Point pt = this.gridViewPatList.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewPatList.CalcHitInfo(pt);
            if (info.InColumn && info.Column.FieldName == "Selected")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridViewPatList.InvalidateColumnHeader(this.gridViewPatList.Columns["Selected"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            for (int i = 0; i < this.gridViewPatList.RowCount; i++)
            {
                if (Convert.ToBoolean(this.gridViewPatList.GetDataRow(i)["CanContinue"]) == true)
                {
                    this.gridViewPatList.GetDataRow(i)["Selected"] = selectAll;
                }
            }
        }
        #endregion

        private EntityOperationResultList AuditCheckMessage;

        public AuditCheckResultViwer(EntityOperationResultList message)
            : this(message, EnumOperationCode.Audit)
        {

        }


        EnumOperationCode auditCheckType = EnumOperationCode.Audit;
        public AuditCheckResultViwer(EntityOperationResultList message, EnumOperationCode checkType)
        {
            InitializeComponent();
            gridheadercheckbox = new RepositoryItemCheckEdit();
            AuditCheckMessage = message;

            auditCheckType = checkType;
            if (checkType == EnumOperationCode.Audit)
            {
                gridColumn9.Visible = false;//列-是否发危急
                this.btnContinue.Text = "继续" + LocalSetting.Current.Setting.AuditWord;
            }
            else if (checkType == EnumOperationCode.Report)
            {
                //报告管理二审可选择是否发危急
                gridColumn9.Visible = UserInfo.GetSysConfigValue("AuditCheckResultViwer_showCol_UnSendUrg") == "是";//列-是否发危急
                this.btnContinue.Text = "继续" + LocalSetting.Current.Setting.ReportWord;
            }
            else if (checkType == EnumOperationCode.UndoReport)//如果是取消报告
            {
                gridColumn9.Visible = false;//列-是否发危急
                this.btnContinue.Text = "继续反审";
            }
            else if (checkType == EnumOperationCode.MidReport)//如果是中期报告
            {
                gridColumn9.Visible = false;//列-是否发危急
                this.btnContinue.Text = "继续";
            }
            this.btnExportExcel.Visible = UserInfo.GetSysConfigValue("Audit_InfoWindow_ShowExcelBut") == "否";
        }

        private void AuditResultViwer_Load(object sender, EventArgs e)
        {
            emptyEditor = new RepositoryItem();
            gridControlPatList.RepositoryItems.Add(emptyEditor);

            gridViewPatList.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewPatList.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewPatList.FocusRectStyle = DrawFocusRectStyle.RowFocus;

            DataTable dt = OperationMessageHelper.ExportToDataTable(AuditCheckMessage, auditCheckType);

            dt.DefaultView.Sort = "CheckSuccess asc,CanContinue asc";
            this.gridControlPatList.DataSource = dt;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            List<string> listPatId = GetSelectedID();
            if (listPatId.Count > 0)
            {
                foreach (string item in listPatId)
                {
                    foreach (EntityOperationResult operResult in AuditCheckMessage)
                    {
                        if (operResult.Data.Patient.RepId == item)
                            operResult.Message.Clear();
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                lis.client.control.MessageDialog.Show("请勾选需要继续操作的记录", "提示");
            }
        }

        public List<string> GetSelectedID()
        {
            List<string> list = new List<string>();

            this.gridViewPatList.CloseEditor();
            if (this.gridControlPatList.DataSource != null)
            {
                DataTable dt = this.gridControlPatList.DataSource as DataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToBoolean(dr["Selected"]) == true)
                    {
                        list.Add(dr["PatID"].ToString());
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取不发送危急提示的ID
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedunSendUrgID()
        {
            List<string> list = new List<string>();

            this.gridViewPatList.CloseEditor();
            if (this.gridControlPatList.DataSource != null)
            {
                DataTable dt = this.gridControlPatList.DataSource as DataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToBoolean(dr["unSendUrg"]) == true)
                    {
                        list.Add(dr["PatID"].ToString());
                    }
                }
            }
            return list;
        }

        RepositoryItem emptyEditor;
        private void gridViewPatList_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            GridView grid = sender as GridView;
            if (e.Column.FieldName == "Selected")
            {
                DataRow dr = grid.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    if (Convert.ToBoolean(dr["CanContinue"]) == false)
                    {
                        e.RepositoryItem = emptyEditor;
                    }
                }
            }
        }

        private void gridViewPatList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Message")
            {
                DataRow dr = this.gridViewPatList.GetDataRow(e.RowHandle);
                if (dr != null)
                {
                    if ((bool)dr["CheckSuccess"] == true)
                    {
                        e.Appearance.ForeColor = Color.Green;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void gridViewPatList_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = this.gridViewPatList.GetDataRow(this.gridViewPatList.FocusedRowHandle);
            if (dr != null)
            {
                string pat_id = dr["PatID"].ToString();
                string pat_sid = dr["PatSID"].ToString();

                OnPatSelected(pat_id, pat_sid);
            }
        }

        public delegate void PatSelectedEventHandler(object sender, string pat_id, string pat_sid);
        public event PatSelectedEventHandler PatSelected;

        public void OnPatSelected(string pat_id, string pat_sid)
        {
            if (PatSelected != null)
            {
                PatSelected(this, pat_id, pat_sid);
            }
        }

        private void gridViewPatList_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gridViewPatList.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.Lavender;
            }
        }

        /// <summary>
        /// 导出数据为Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportInputPublic export = new ExportInputPublic();
            export.gridViewExport = this.gridViewPatList;
            string fileName = export.ShowSaveFileDialog("OperationResultList");
            if (fileName != "")
            {
                export.ExportTo(new ExportXlsProvider(fileName));

                export.OpenFile(fileName);
            }
            export = null;
        }
    }
}
