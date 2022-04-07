using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using System.IO;
using System.Configuration;
using System.Web;
using dcl.client.repdesign;
using System.Collections;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using lis.client.control;
using dcl.common;
using Lib.DataInterface.Implement;
using Lib.LogManager;
using dcl.client.cache;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;

namespace dcl.client.report
{
    public partial class FrmReporMain : FrmCommon
    {
        public FrmReporMain()
        {
            InitializeComponent();
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            try
            {
                Directory.CreateDirectory(localPath);
            }
            catch (Exception)
            { }
        }

        string localPath = PathManager.ReportPath;
        ProxyReportMain proxy = new ProxyReportMain();
        private List<EntitySysReport> listReport = new List<EntitySysReport>();
        private List<EntitySysReportParameter> listPar = new List<EntitySysReportParameter>();
        #region CheckBoxOnGridHeader

        void gridViewPatientList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "gc_code")
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

            Point pt = this.gridView1.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridView1.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "gc_code")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridView1.InvalidateColumnHeader(this.gridView1.Columns["gc_code"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            for (int i = 0; i < this.listReport.Count; i++)
            {
                //this.gridView1.GetDataRow(i)["RepSelect"] = bGridHeaderCheckBoxState;
                listReport[i].RepSelect = bGridHeaderCheckBoxState.ToString();
                
            }
            bsReport.ResetBindings(true);
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                EntitySysReport report = (EntitySysReport) bsReport.Current;
                string path = localPath + @"\" + report.RepLocation.Trim();
                if (File.Exists(path) == false)
                {
                    lis.client.control.MessageDialog.Show("该设计报表不存在，请从服务器上获取在进行修改！", "提示");
                    return;
                }
                MainForm mf = new MainForm(path, report.RepId.ToString(), report.RepSql, report.RepConectCode);
                try
                {
                    mf.ShowDialog();
                }
                catch (Exception ex)
                {
                    mf.Close();
                    //Logger.WriteException("FrmReporMain", "报表设计错误", ex.ToString());
                    Logger.LogException("报表设计错误", ex);
                    lis.client.control.MessageDialog.Show("设计器异常关闭，请联系管理员！", "提示");
                }
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        private void FrmReporMain_Load(object sender, EventArgs e)
        {
            sysClien.SetToolButtonStyle(new string[] {sysClien.BtnGetVersion.Name,sysClien.BtnImport.Name,
                                        sysClien.BtnExport.Name,sysClien.BtnDesign.Name,sysClien.BtnUploadVersion.Name,
                                        sysClien.BtnPrintPreview2.Name});

            this.sysClien.OnBtnExportClicked += new System.EventHandler(this.btnEduce_Click);
            this.sysClien.OnBtnImportClicked += new System.EventHandler(this.btnImport_Click);
            this.sysClien.BtnDesignClick += new System.EventHandler(this.btnUpdate_Click);
            this.sysClien.BtnGetVersionClick += new System.EventHandler(this.btnGetServer_Click);
            this.sysClien.BtnUploadVersionClick += new System.EventHandler(this.sysClien_BtnUploadVersionClick);
            this.sysClien.OnPrintPreviewClicked += SysClien_OnPrintPreviewClicked;

            sysOperation.SetToolButtonStyle(new string[] {sysOperation.BtnAdd.Name,sysOperation.BtnModify.Name,sysOperation.BtnSaveDefault.Name,
                                        sysOperation.BtnSave.Name,sysOperation.BtnDelete.Name,sysOperation.BtnCancel.Name,sysOperation.BtnRevertDefault.Name });
            this.isEnable(true);
            txtSql.SetHighlighting("TSQL");
            txtSql.ShowEOLMarkers = false;
            txtSql.ShowSpaces = false;
            txtSql.ShowTabs = false;
            txtSql.ShowInvalidLines = false;
            txtSql.Dock = DockStyle.Fill;
            txtSql.VRulerRow = 200;
            getReport();
            this.txtConnection.Properties.DataSource = CacheDataInterfaceConnection.Current.GetAll();
        }


        public void getReport()
        {
            EntityRequest request = new EntityRequest();
            EntityResponse response = proxy.Service.GetReport(request);
            if (response.Scusess)
            {
                listReport = response.GetResult() as List<EntitySysReport>;
            }
            this.bsReport.DataSource = listReport;
        }

        //导入
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                EntitySysReport rep = (EntitySysReport)bsReport.Current;
                if (rep == null)
                {
                    lis.client.control.MessageDialog.Show("请选择要覆盖的报表！", "提示");
                    return;
                }
                //弹出选择文件框以及设置过滤条件
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "repx";
                ofd.Filter = "报表设计文件(*.repx)|*.repx";
                ofd.Title = "选择报表";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() != string.Empty)
                    {
                        //string path = Application.StartupPath.ToString() + @"\xtraReport";
                        //addFile(localPath);
                        string path = localPath + @"\" + rep.RepLocation.Trim();
                        if (readAndWrite(path, ofd.FileName.Trim(), "请检查文件格式是否正确或文件是否已经损坏!"))
                            lis.client.control.MessageDialog.Show("导入成功", "提示");
                    }
                }
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        //导出
        private void btnEduce_Click(object sender, EventArgs e)
        {
            try
            {
                EntitySysReport rep = (EntitySysReport)bsReport.Current;
                if (rep == null)
                {
                    lis.client.control.MessageDialog.Show("请选择要导出的报表！", "提示");
                    return;
                }
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "repx";
                ofd.Filter = "报表设计文件(*.repx)|*.repx";
                ofd.Title = "选择报表";
                ofd.FileName = rep.RepLocation.Trim();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() != string.Empty)
                    {
                        string path = localPath + @"\" + rep.RepLocation.Trim();
                        if (readAndWrite(ofd.FileName.Trim(), path, "该报表不存在，请从服务器上获取!"))
                            lis.client.control.MessageDialog.Show("导出成功", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        /// <summary>
        /// 读取写入文件
        /// </summary>
        /// <param name="readPath">读取路径</param>
        /// <param name="WritePath">写入路径</param>
        private bool readAndWrite(string readPath, string writePath, string erro)
        {
            try
            {
                File.Copy(writePath, readPath, true);
                return true;
            }
            catch
            {
                MessageBox.Show(erro, "程序出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

 

        //获取服务器版本
        private void btnGetServer_Click(object sender, EventArgs e)
        {
            try
            {
                sysClien.Focus();
                List<EntitySysReport> listRep = bsReport.DataSource as List<EntitySysReport>;
                listRep = listRep.Where(w=>w.RepSelect.Contains("True")).ToList();
                if (listRep.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("请选择要获取的报表！", "提示");
                    return;
                }
                DialogResult dir = lis.client.control.MessageDialog.Show("        是否获取服务器版本？\r\n(获取服务器版本本地版本将会被覆盖!)", "提示", MessageBoxButtons.YesNo);
                if (dir == DialogResult.No)
                    return;

                string str = "";
                foreach (EntitySysReport rep in listRep)
                {
                    if (rep.RepLocation != "")
                    {
                        string serverPath = ConfigurationManager.AppSettings["wcfAddr"].ToString();
                        string readPath = serverPath + @"xtraReport/" + rep.RepLocation.Trim();
                        string writePath = localPath + @"\" + rep.RepLocation.Trim();
                        System.Net.WebClient client = new System.Net.WebClient();
                        try
                        {
                            client.DownloadFile(readPath, writePath);
                        }
                        catch (Exception)
                        {
                            str += "服务器上无:" + rep.RepLocation.ToString() + "报表！\r\n";
                        }
                    }
                }
                if (str != "")
                    lis.client.control.MessageDialog.Show(str, "提示");
                else
                    lis.client.control.MessageDialog.Show("获取成功！", "提示");
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        public void isEnable(bool isTrue)
        {
            txtName.Properties.ReadOnly = isTrue;
            txtCode.Properties.ReadOnly = isTrue;
            txtConnection.Properties.ReadOnly = isTrue;
            grdReport.Enabled = isTrue;
            panelControl2.Enabled = isTrue;
            sysOperation.BtnSaveDefault.Enabled = isTrue;
            sysOperation.BtnRevertDefault.Enabled = isTrue;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.isEnable(false);
            txtSql.Text = "";
            bsReport.AddNew();
        }

        private void btnModified_Click(object sender, EventArgs e)
        {
            this.isEnable(false);
            txtCode.Properties.ReadOnly = true;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = txtSql.Text.ToString();
                bsReport.EndEdit();
                if (txtCode.EditValue.ToString() == "")
                {
                    lis.client.control.MessageDialog.Show("关联代码不能为空！", "提示");
                    return;
                }
                if (sql == "")
                {
                    lis.client.control.MessageDialog.Show("Sql不能为空！", "提示");
                    return;
                }
                List<EntitySysReport> dtRep = bsReport.DataSource as List<EntitySysReport>;
                int count = dtRep.Where(w => w.RepCode==(txtCode.EditValue.ToString().Trim())).Count();
                if (count > 1)
                {
                    lis.client.control.MessageDialog.Show("此关联代码已存在！", "提示");
                    return;
                }
                count = dtRep.Where(w => w.RepCode==(txtName.EditValue.ToString().Trim())).Count();
                if (count > 1)
                {
                    lis.client.control.MessageDialog.Show("名称不能重复！", "提示");
                    return;
                }
                EntitySysReport report = (EntitySysReport)bsReport.Current;
                report.RepSql= EncryptClass.Encrypt(sql);
                String itm_id =report.RepId.ToString();
                report.RepLocation = report.RepName.ToString().Trim() + ".repx";
                report.RepConectCode = this.txtConnection.EditValue == null ? string.Empty : this.txtConnection.EditValue.ToString();
                EntityRequest request = new EntityRequest();
                EntityResponse result = new EntityResponse();
                request.SetRequestValue(report);
                if (itm_id == "0")
                {
                    result = proxy.Service.NewReport(request);
                    string path = localPath + @"\";
                    try
                    {
                        File.Copy(path + "报表样本.repx", path + report.RepLocation, true);
                        System.IO.FileInfo file = new System.IO.FileInfo(path + report.RepLocation.ToString());
                        file.IsReadOnly = false;
                    }
                    catch (Exception)
                    {
                        lis.client.control.MessageDialog.Show("不存在样本标本！", "提示");
                    }

                }
                else
                {
                     result = proxy.Service.UpdateReport(request);

                }
                if (result.Scusess)
                {
                    isActionSuccess = true;
                    if (itm_id == "0")
                    {
                        getReport();
                        grdReport.Focus();
                    }
                    lis.client.control.MessageDialog.Show("保存成功", "提示");
                }
                this.isEnable(true);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        /// <summary>
        /// 报表删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                sysClien.Focus();
                List<EntitySysReport> listRep = bsReport.DataSource as List<EntitySysReport>;
                listRep = listRep.Where(w => w.RepSelect.ToString().Contains("True")).ToList();
                if (listRep.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("请选择你需要删除的数据！", "提示");
                    return;
                }
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗?(删除\r\n该报表将会删除对应的参数！） ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dresult == DialogResult.OK)
                {
                    EntitySysReport dr = (EntitySysReport)bsReport.Current;
                    String itm_id = dr.RepId.ToString();
                    List<EntitySysReportParameter> listParameter = new List<EntitySysReportParameter>();
                    if (listRep != null)
                    {
                        foreach (var listPar in listRep)
                        {
                            EntitySysReportParameter entitypar = new EntitySysReportParameter();
                            entitypar.RepId = listPar.RepId;
                            listParameter.Add(entitypar);
                        }

                    }
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Report", listRep);
                    dict.Add("Parameter", listParameter);
                    EntityRequest request = new EntityRequest();
                    request.SetRequestValue(dict);
                    EntityResponse reponse= proxy.Service.DeleteReport(request);
                    if (reponse.Scusess)
                    {
                        getReport();
                        lis.client.control.MessageDialog.Show("删除成功", "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        private void btnAbandon_Click(object sender, EventArgs e)
        {
            try
            {
                this.isEnable(true);
                this.bsReport.EndEdit();
                getReport();
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (bsReport.Current != null)
                {
                    EntitySysReport report = (EntitySysReport)bsReport.Current;
                    if (report != null)
                    {
                        txtSql.Refresh();
                        if (report.RepId.ToString() != "0")
                        {
                            getParameter(Convert.ToInt32(report.RepId));
                            txtSql.Text = "";
                            txtSql.Refresh();
                            string txt = EncryptClass.Decrypt(report.RepSql.ToString());
                            txt = IsHadLoneEnter(txt);
                            this.txtSql.Text = txt;//EncryptClass.Decrypt(report.RepSql.ToString());
                        }
                        else
                        {
                            bsParameter.DataSource = null;
                            txtSql.Refresh();
                            txtSql.Text = "";
                        }
                    }
                }
                else
                {
                    bsParameter.DataSource = null;
                    txtSql.Refresh();
                    txtSql.Text = "";
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("失败", "提示");
            }
        }

        /// <summary>
        /// 是否存在单独的\r
        /// </summary>
        /// <returns></returns>
        public string IsHadLoneEnter(string str)
        {
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == '\r'  && ( str[i + 1] != '\n' && str[i + 1] != '\t'))
                {
                    str = str.Insert(i + 1, "\n");
                    return IsHadLoneEnter(str);
                }

            }
            return str;
        }

        public void getParameter(int id)
        {
            EntityResponse response = proxy.Service.GetReportPar(id.ToString());
            if (response.Scusess)
            {
                listPar = response.GetResult() as List<EntitySysReportParameter>;
            }
            this.bsParameter.DataSource = listPar;
        }
        
        private void btnParAdd_Click(object sender, EventArgs e)
        {
            EntitySysReport rep = (EntitySysReport)bsReport.Current;
            if (rep == null)
            {
                lis.client.control.MessageDialog.Show("请选择要添加参数的报表！", "提示");
                return;
            }
            if (txtPara.EditValue == null || txtInitPar.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("参数或初始化参数不能为空！", "提示");
                return;
            }
            List<EntitySysReportParameter> dtPar = bsParameter.DataSource as List<EntitySysReportParameter>;
            if (dtPar != null)
            {
                int count = dtPar.Where(w =>w.RepParmType.Contains(txtPara.EditValue.ToString())).ToList().Count();
                if (count > 0)
                {
                    lis.client.control.MessageDialog.Show("参数已存在，请勿重复添加！", "提示");
                    return;
                }
            }
            EntitySysReportParameter par = new EntitySysReportParameter();
            par.RepId = rep.RepId;
            par.RepParmType = txtPara.EditValue.ToString().Trim();
            par.RepParmValue = txtInitPar.EditValue.ToString().Trim();
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(par);
            EntityResponse result = proxy.Service.NewReportParameter(request);
            if (result.Scusess)
            {
                getParameter(Convert.ToInt32(rep.RepId));
                txtPara.EditValue = txtInitPar.EditValue = null;
            }
        }

        private void btnParDel_Click(object sender, EventArgs e)
        {
            if (bsParameter.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择你要删除的数据！", "提示");
                return;
            }
            DialogResult dresult = MessageBox.Show("确定要删除该记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult == DialogResult.OK)
            {
                EntitySysReport drRep = (EntitySysReport)bsReport.Current;
                DataSet ds = new DataSet();
                EntitySysReportParameter dr = (EntitySysReportParameter)bsParameter.Current;
                String itm_id = dr.RepId.ToString();
                EntityRequest request = new EntityRequest();
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("Parameter", dr);
                request.SetRequestValue(dict);
                EntityResponse result=proxy.Service.DeleteReport(request);
                if (result.Scusess)
                {
                    getParameter(Convert.ToInt32(drRep.RepId));
                }
            }
        }

        //预览
        private void SysClien_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            EntitySysReport drRep = (EntitySysReport)bsReport.Current;
            if (drRep == null)
            {
                lis.client.control.MessageDialog.Show("请选择要预览的报表！", "提示");
                return;
            }

            if (string.IsNullOrEmpty(txtInitPar.Text))
            {
                lis.client.control.MessageDialog.Show("请填写数值（报告ID）！", "提示");
                return;
            }

            EntityDCLPrintParameter par = new EntityDCLPrintParameter();
            par.RepId = txtInitPar.Text;
            par.ReportCode = drRep.RepCode;
            XtraReport xr = DCLReportPrint.GetXtraReportByPrintData(par);
            if (xr != null)
                xr.ShowPreviewDialog();
        }

        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysClien_BtnUploadVersionClick(object sender, EventArgs e)
        {
            sysClien.Focus();
            List<EntitySysReport> listRep =bsReport.DataSource as List<EntitySysReport>;
            listRep = listRep.Where(w => w.RepSelect.ToString().Contains("True")).ToList();
            if (listRep.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请选择要上传的报表！", "提示");
                return;
            }
            DialogResult dir = lis.client.control.MessageDialog.Show("        是否上传到服务器？\r\n(上传到服务器服务器版本将会被覆盖!)", "提示", MessageBoxButtons.YesNo);
            if (dir == DialogResult.No)
                return;

            string str = "";
            foreach (EntitySysReport rep in listRep)
            {
                if (rep.RepLocation != "")
                {
                    string readPath = @"xtraReport/" + rep.RepLocation.Trim();
                    string writePath = localPath + @"\" + rep.RepLocation.Trim();
                    try
                    {
                        if(!UpLoadFile(writePath, readPath))
                        {
                            str += rep.RepName + "报表上传失败！\r\n";
                        }
                    }
                    catch (Exception)
                    {
                        str += "本地无:" + rep.RepName + "报表！\r\n";
                    }
                }
            }
            if (str != "")
                lis.client.control.MessageDialog.Show(str, "提示");
            else
                lis.client.control.MessageDialog.Show("上传成功！", "提示");
        }

        public bool UpLoadFile(string fileNameFullPath, string strUrlDirPath)
        {
            try
            {
                // 将要上传的文件打开读进文件流
                FileStream myFileStream = new FileStream(fileNameFullPath, FileMode.Open, FileAccess.Read);
                byte[] postArray = new byte[myFileStream.Length];
                myFileStream.Read(postArray, 0, postArray.Length);
                myFileStream.Close();
                myFileStream.Dispose();

                EntityRequest request = new EntityRequest();
                EntitySysReport report = new EntitySysReport();
                report.FlieName = strUrlDirPath;
                report.FlieBase64 = Convert.ToBase64String(postArray);
                request.SetRequestValue(report);
                bool result = proxy.Service.UpLoadReportFile(request);

            }
            catch (Exception ex)
            {
                Logger.LogException("上传失败！", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 备份SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysOperation_BtnSaveDefaultClick(object sender, EventArgs e)
        {
            EntitySysReport drReport = (EntitySysReport)bsReport.Current;
            if (drReport == null)
            {
                lis.client.control.MessageDialog.Show("请选择要备份SQL的数据", "提示");
                return;
            }
            DialogResult dlr = lis.client.control.MessageDialog.Show("是否备份?(请确定备份数据的正确性)", "提示", MessageBoxButtons.YesNo);
            if (dlr == DialogResult.Yes)
            {
                drReport.RepDefaultSql = drReport.RepSql;//把SQL放入备份字段
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(drReport);
                EntityResponse response= proxy.Service.UpdateReport(request);
                if (response.Scusess)
                {
                    getReport();
                    grdReport.Focus();
                    lis.client.control.MessageDialog.Show("备份成功", "提示");
                }
            }
        }

        /// <summary>
        /// 还原SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysOperation_BtnRevertDefaultClick(object sender, EventArgs e)
        {
            EntitySysReport drReport = (EntitySysReport)bsReport.Current;
            if (drReport == null)
            {
                lis.client.control.MessageDialog.Show("请选择要还原SQL的数据", "提示");
                return;
            }
            DialogResult dlr = lis.client.control.MessageDialog.Show("是否还原SQL?(还原SQL将会使你现在使用的SQL丢失！)", "提示", MessageBoxButtons.YesNo);
            if (dlr == DialogResult.Yes)
            {
                if (lis.client.control.MessageDialog.Show("真的确定还原?(还原后你现使用的SQL将会彻底消失！)", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    drReport.RepSql = drReport.RepDefaultSql;//把备份SQL放入使用字段
                    EntityRequest request = new EntityRequest();
                    request.SetRequestValue(drReport);
                    EntityResponse response = proxy.Service.UpdateReport(request);
                    if (response.Scusess)
                    {
                        getReport();
                        grdReport.Focus();
                    }
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (bsReport.Current != null)
            {
                EntitySysReport drReport = (EntitySysReport)bsReport.Current;
                FrmServerReport fsr = new FrmServerReport(drReport.RepLocation);
                fsr.ShowDialog();
            }
            else
                lis.client.control.MessageDialog.Show("请选择要还原的报表！", "提示");

        }

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchFilter_TextChanged(object sender, EventArgs e)
        {
          
            string filter = txtSearchFilter.Text.Trim();

            if (filter == string.Empty)
                bsReport.DataSource = listReport;
            else
            {
                bsReport.DataSource = listReport.Where(w => w.RepCode.Contains(filter) ||
                                                     w.RepName.Contains(filter)).ToList();
            }
            gridView1_FocusedRowChanged(null, null);
        }

    }
}
