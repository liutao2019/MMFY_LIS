using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraEditors;
using dcl.client.frame;
using lis.client.control;
using System.Configuration;
using System.IO;
using System.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.control;
using System.Linq;

namespace dcl.client.oa
{
    public partial class OrderDetail : UserControl
    {
        public string OrderTypeCode = "";
        public string S1 = "";
        public string S2 = "";
        public string S3 = "";
        public string S4 = "";
        public string S5 = "";
        public string S6 = "";

        string filterString = "1=1";

        public static string ZeroFormat = "00000000";

        public bool ShowSearch
        {
            get { return pnlSearch.Visible; }
            set { pnlSearch.Visible = value; }
        }

        public static char split = '┏';
        /// <summary>
        /// 判断点击保存按钮时是Insert还是Update的标志位
        /// </summary>
        public enum OptionStatus
        {
            Insert,
            Update
        }
        OptionStatus optionStatus = OptionStatus.Update;


        List<EntityOaTableField> listField = new List<EntityOaTableField>();
        DataView dvOrderDetail = new DataView();

        ProxyOaTableDetail proxyDetail = null;

        public OrderDetail()
        {
            InitializeComponent();

            this.barOrderDetail.OnBtnSearchClicked += new System.EventHandler(this.BarOrderDetail_OnBtnSearchClicked);
            this.barOrderDetail.OnBtnAddClicked += new System.EventHandler(this.barOrderDetail_OnBtnAddClicked);
            this.barOrderDetail.OnBtnModifyClicked += new System.EventHandler(this.barOrderDetail_OnBtnModifyClicked);
            this.barOrderDetail.OnBtnDeleteClicked += new System.EventHandler(this.barOrderDetail_OnBtnDeleteClicked);
            this.barOrderDetail.OnBtnSaveClicked += new System.EventHandler(this.barOrderDetail_OnBtnSaveClicked);
            this.barOrderDetail.OnBtnCancelClicked += new System.EventHandler(this.barOrderDetail_OnBtnCancelClicked);
            this.barOrderDetail.OnBtnRefreshClicked += new System.EventHandler(this.barOrderDetail_OnBtnRefreshClicked);
            this.barOrderDetail.OnBtnSearchClicked += new System.EventHandler(this.barOrderDetail_OnBtnSearchClicked);
            this.barOrderDetail.OnBtnExportClicked += new System.EventHandler(this.barOrderDetail_OnBtnExportClicked);
            this.barOrderDetail.BtnCopyClick += new System.EventHandler(this.barOrderDetail_BtnCopyClick);
        }

        public bool QuickOption
        {
            set { barOrderDetail.QuickOption = value; }
            get { return barOrderDetail.QuickOption; }
        }

        public delegate void FocusedChangedEventHandler(object sender, EventArgs arg);
        public event FocusedChangedEventHandler FocusedChanged;

        /// <summary>
        /// 取得当前选中项的OrderCode
        /// </summary>
        /// <returns></returns>
        public string GetFocusedOrderCode()
        {
            string result = "";
            if (tvOrderDetail.Selection.Count > 0)
            {
                result = tvOrderDetail.Selection[0].GetValue(tvOrderDetail.Columns["DetId"]).ToString();
            }
            return result;
        }

        public void OnFocusedChanged()
        {
            if (FocusedChanged != null)
            {
                FocusedChanged(this, new EventArgs());
            }
        }

        #region 控件载入
        /// <summary>
        /// 控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDetail_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            proxyDetail = new ProxyOaTableDetail();
            //显示按钮
            // if (UserInfo.HaveFunction("dcl.client.oa.FrmOfficePlan", "OfficeSelect"))
            barOrderDetail.SetToolButtonStyle(new string[] { barOrderDetail.BtnSearch.Name, barOrderDetail.BtnAdd.Name, barOrderDetail.BtnModify.Name, barOrderDetail.BtnDelete.Name, barOrderDetail.BtnSave.Name, barOrderDetail.BtnCancel.Name, barOrderDetail.BtnExport.Name, barOrderDetail.BtnRefresh.Name, barOrderDetail.BtnCopy.Name });
            //  else
            //     barOrderDetail.SetToolButtonStyle(new string[] { barOrderDetail.BtnExport.Name, barOrderDetail.BtnRefresh.Name, barOrderDetail.BtnCopy.Name });
            dateEditStart.EditValue = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            dateEditEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #endregion

        public void SetCanEdit(bool canEdit, bool canExport)
        {

            barOrderDetail.BtnAdd.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            barOrderDetail.BtnModify.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;
            barOrderDetail.BtnDelete.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;
            barOrderDetail.BtnCopy.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;
            barOrderDetail.BtnCancel.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;
            barOrderDetail.BtnSave.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;
            barOrderDetail.BtnExport.Visibility = canExport ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never; ;

        }

        #region 重新生成所有内容

        /// <summary>
        /// 重新生成所有内容
        /// </summary>
        public void LoadData()
        {
            //生成控件
            LoadEdit();

            //生成列表
            LoadList(true);

            //默认按钮状态
            barOrderDetail.EnableButton(false);
        }

        #endregion

        #region 生成列表页面
        /// <summary>
        /// 生成列表页面
        /// </summary>
        /// <param name="reload">是否清空过滤条件</param>
        public void LoadList(bool reload)
        {
            EntityOaTableDetail detail = new EntityOaTableDetail();

            if (OrderTypeCode != "")
            {
                detail.TabCode = OrderTypeCode;
            }

            if (S1 != "")
            {
                detail.DetCharA = S1;
            }

            if (S2 != "")
            {
                detail.DetCharB = S2;
            }

            if (S3 != "")
            {
                detail.DetCharC = S3;
            }

            if (S4 != "")
            {
                detail.DetCharD = S4;
            }


            DataTable dtOrderDetail = new DataTable();
            dtOrderDetail.Columns.Add("DetId");
            dtOrderDetail.Columns.Add("DetDate");
            dtOrderDetail.Columns.Add("DetContent");
            List<EntityOaTableDetail> tabDetail = proxyDetail.Service.GetTabDetailByTabCode(detail);
            foreach (var item in tabDetail)
            {
                DataRow dr = dtOrderDetail.NewRow();
                dr["DetId"] = item.DetId;
                dr["DetDate"] = item.DetDate;
                dr["DetContent"] = item.DetContent;
                dtOrderDetail.Rows.Add(dr);
            }

            foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in tvOrderDetail.Columns)
            {
                if (dtOrderDetail.Columns.Contains(col.FieldName) == false)
                {
                    dtOrderDetail.Columns.Add(col.FieldName);
                }
            }
            for (int j = 0; j < dtOrderDetail.Rows.Count; j++)
            {
                DataRow thisDr = dtOrderDetail.Rows[j];
                string[] orderDetail = tabDetail[j].DetContent.ToString().Split(split);

                for (int i = 1; i < orderDetail.Length - 1; i = i + 2)
                {
                    string tmpDetail = (int.Parse(orderDetail[i])).ToString(ZeroFormat);
                    if (dtOrderDetail.Columns.Contains("fld" + tmpDetail))
                    {
                        thisDr["fld" + tmpDetail] = orderDetail[i + 1]; 
                    }
                }
            }

            dvOrderDetail = new DataView(dtOrderDetail);

            if (reload)
            {
                filterString = "1=1";
            }

            tvOrderDetail.DataSource = dvOrderDetail;
            dvOrderDetail.RowFilter = this.setFilterStr(filterString);

            tvOrderDetail.BestFitColumns(false);
            
        }
        #endregion

        public DataTable DataSource
        {
            get
            {
                return dvOrderDetail.Table;
            }
        }

        #region 生成详细页面
        /// <summary>
        /// 生成控件编辑框和列
        /// </summary>
        public void LoadEdit()
        {
            pnlDetail.Visible = false;
            pnlDetail.Controls.Clear();

            string tabCode = OrderTypeCode;
            listField = proxyDetail.Service.GetOrderTableField(tabCode);

            for (int i = 0; i < listField.Count; i++)
            {
                EntityOaTableField drItem = listField[i];

                Panel panel = new Panel();
                panel.Name = "pnl" + i.ToString(ZeroFormat);
                panel.Dock = DockStyle.Top;
                panel.Height = 26;

                Panel panelA = new Panel();
                panelA.Name = "pna" + i.ToString(ZeroFormat);
                //panelA.Dock = DockStyle.Left;
                panelA.Width = 250;
                panelA.Height = 26;
                panelA.Location = new Point(0, 0);


                Panel panelB = new Panel();
                panelB.Name = "pnb" + i.ToString(ZeroFormat);
                //panelB.Dock = DockStyle.Fill;
                panelB.Height = 26;
                panelB.Width = 250;
                panelB.Location = new Point(250, 0);

                CreatePanel(drItem, ref panelA);
                if (listField.Count - 1 > i && drItem.FieldType.ToString() != "文本" && listField[i + 1].FieldType.ToString() != "文本")
                {
                    CreatePanel(listField[i + 1], ref panelB);
                    i++;
                }

                if (panelA.Height > 26)
                {
                    panel.Height = panelA.Height;
                    panelB.Visible = false;
                }
                panel.Controls.Add(panelA);
                panel.Controls.Add(panelB);
                panelA.SendToBack();
                pnlDetail.Controls.Add(panel);
                panel.BringToFront();

                Panel panelSpace = new Panel();
                panelSpace.Name = "pns" + i.ToString(ZeroFormat);
                panelSpace.Dock = DockStyle.Top;
                panelSpace.Height = 5;
                pnlDetail.Controls.Add(panelSpace);
                panelSpace.BringToFront();
            }
            pnlDetail.Visible = true;
            EnterEditingState(false);

            //生成列数据
            tvOrderDetail.Columns.Clear();
            DevExpress.XtraTreeList.Columns.TreeListColumn colOrderCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colOrderCode.Caption = "OrderCode";
            colOrderCode.FieldName = "DetId";
            colOrderCode.Name = "colOrderCode";
            colOrderCode.VisibleIndex = 0;
            colOrderCode.OptionsColumn.AllowEdit = false;
            tvOrderDetail.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colOrderCode });
            colOrderCode.Visible = false;

            //添加创建时间列
            DevExpress.XtraTreeList.Columns.TreeListColumn colOrderDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            colOrderDate.Caption = "创建时间";
            colOrderDate.FieldName = "DetDate";
            colOrderDate.Name = "OrderDate";
            colOrderDate.Format.FormatString = "yyyy-MM-dd HH:mm";
            //colOrderDate.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            //colOrderDate.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime;
            colOrderDate.VisibleIndex = 0;
            colOrderDate.Width = 139;
            colOrderDate.OptionsColumn.AllowEdit = false;
            tvOrderDetail.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] { colOrderDate });
            colOrderDate.Visible = true;//显示

            foreach (EntityOaTableField thisDr in listField)
            {
                DevExpress.XtraTreeList.Columns.TreeListColumn col = new DevExpress.XtraTreeList.Columns.TreeListColumn();
                col.Caption = thisDr.FieldName.ToString();

                col.FieldName = "fld" + int.Parse(thisDr.FieldCode.ToString()).ToString(ZeroFormat);
                col.Name = "col" + int.Parse(thisDr.FieldCode.ToString()).ToString(ZeroFormat);
                col.VisibleIndex = int.Parse(thisDr.FieldIndex.ToString()) + 1;
                col.OptionsColumn.AllowEdit = false;
                //col.OptionsColumn.FixedWidth = true;
                //col.Width = 100;
                tvOrderDetail.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] { col });
                if (thisDr.FieldListDisplay.ToString() == "1")
                {
                    col.Visible = true;
                }
                else
                {
                    col.Visible = false;
                }
            }
        }

        /// <summary>
        /// 创建一个单证字段输入
        /// </summary>
        /// <param name="drItem"></param>
        /// <param name="panel"></param>
        private void CreatePanel(EntityOaTableField drItem, ref Panel panel)
        {
            LabelControl lab = new LabelControl();
            lab.Name = "lab" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
            lab.Text = drItem.FieldName.ToString();
            lab.AutoSizeMode = LabelAutoSizeMode.None;
            lab.Height = 20;
            lab.Width = 70;
            lab.Location = new Point(0, 0);
            lab.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lab.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            panel.Controls.Add(lab);


            Panel spaceA = new Panel();
            spaceA.Name = "spa" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
            spaceA.Dock = DockStyle.Left;
            spaceA.Width = 5;
            panel.Controls.Add(spaceA);


            if (drItem.FieldType.ToString() == "数字")
            {
                SpinEdit txtEdit = new SpinEdit();
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                txtEdit.Properties.MaxValue = 99999999;
                txtEdit.Properties.IsFloatValue = true;
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Properties.ReadOnly = true;
                //txtEdit.Dock = DockStyle.Left;
                txtEdit.Location = new Point(75, 0);
                panel.Controls.Add(txtEdit);
            }

            if (drItem.FieldType.ToString() == "字符串")
            {
                TextEdit txtEdit = new TextEdit();
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Properties.ReadOnly = true;
                txtEdit.Location = new Point(75, 0);
                panel.Controls.Add(txtEdit);
            }

            if (drItem.FieldType.ToString() == "日期")
            {
                DateEdit txtEdit = new DateEdit();
                //txtEdit.EditValue = DateTime.Now;
                txtEdit.Text = "";
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Properties.ReadOnly = true;
                txtEdit.Location = new Point(75, 0);
                panel.Controls.Add(txtEdit);
            }

            if (drItem.FieldType.ToString() == "文件")
            {
                ButtonEdit txtEdit = new ButtonEdit();
                txtEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down,"下载"),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK,"查看")});
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                //txtEdit.Click += new EventHandler(txtEdit_Click);
                txtEdit.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtEdit_ButtonPressed);
                txtEdit.Width = 140;
                txtEdit.Text = "未上传";
                txtEdit.Properties.ReadOnly = true;
                txtEdit.Location = new Point(75, 0);
                panel.Controls.Add(txtEdit);
            }

            if (drItem.FieldType.ToString() == "时间")
            {
                TimeEdit txtEdit = new TimeEdit();
                txtEdit.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
                //txtEdit.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:00:00"));
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                txtEdit.EnterMoveNextControl = true;
                txtEdit.Width = 140;
                txtEdit.Properties.ReadOnly = true;
                txtEdit.Location = new Point(75, 0);
                panel.Controls.Add(txtEdit);
            }

            if (drItem.FieldType.ToString() == "枚举")
            {
                if (drItem.FieldDictList.ToString() != "" && (drItem.FieldDictSql.ToString() != "" || drItem.FieldDictList.ToString().IndexOf(",") != -1))
                {
                    ComboBoxEdit txtEdit = new ComboBoxEdit();
                    txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                    txtEdit.EnterMoveNextControl = true;
                    txtEdit.Width = 140;
                    txtEdit.Properties.ReadOnly = true;
                    txtEdit.Location = new Point(75, 0);
                    txtEdit.Properties.DropDownRows = 10;
                    txtEdit.Properties.ImmediatePopup = true;

                    string dictName = drItem.FieldDictList.ToString();

                    try
                    {
                        if (dictName.IndexOf(",") != -1)
                        {
                            //文本枚举
                            string[] items = dictName.Split(',');
                            txtEdit.Properties.Items.AddRange(items);
                        }
                        else
                        {
                            //数据字典
                            List<EntityDicItmItem> listItem = proxyDetail.Service.GetItemList();

                            foreach (EntityDicItmItem drDict in listItem)
                            {
                                txtEdit.Properties.Items.Add(drDict.ItmName);
                            }

                        }
                    }
                    catch
                    {
                        txtEdit.Text = "字典绑定出错";
                    }

                    panel.Controls.Add(txtEdit);
                }
                else
                {
                    //使用系统自定义控件
                    string dictName = drItem.FieldDictList.ToString();
                    UserControl txtEdit = new UserControl();

                    if (dictName == "SelectDict_Type")
                    {
                        txtEdit = new SelectDicLabProfession();
                    }

                    if (dictName == "SelectDict_Instrmt")
                    {
                        txtEdit = new SelectDicInstrument();
                    }

                    if (dictName == "SelectDict_inspect")
                    {
                        txtEdit = new SelectDictSysUser();
                    }

                    txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                    txtEdit.Width = 140;
                    txtEdit.Height = 30;
                    //txtEdit.Readonly = true;
                    txtEdit.Location = new Point(75, 0);
                    //txtEdit.EnterMoveNext = true;
                    //txtEdit.Readonly = false;
                    //txtEdit.SaveSourceID = true;
                    //txtEdit.SelectOnly = true;

                    panel.Controls.Add(txtEdit);
                }
            }

            if (drItem.FieldType.ToString() == "文本")
            {
                MemoEdit txtEdit = new MemoEdit();
                txtEdit.Name = "txt" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
                txtEdit.Width = 390;
                txtEdit.Height = 80;
                txtEdit.Properties.ReadOnly = true;
                txtEdit.Location = new Point(75, 0);
                panel.Height = 80;
                panel.Width = 500;
                panel.Controls.Add(txtEdit);
            }

            Panel spaceB = new Panel();
            spaceB.Name = "spb" + int.Parse(drItem.FieldCode.ToString()).ToString(ZeroFormat);
            spaceB.Dock = DockStyle.Left;
            spaceB.Width = 10;
            panel.Controls.Add(spaceB);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtEdit_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit txtEdit = (ButtonEdit)sender;

            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.OK)
            {
                if (txtEdit.Text == "未上传")
                {
                    return;
                }

                try
                {
                    string serverPath = System.Configuration.ConfigurationManager.AppSettings["wcfAddr"].ToString();
                    string readPath = serverPath + @"uploadFile/" + txtEdit.Text;

                    string extendName = txtEdit.Text.Substring(txtEdit.Text.LastIndexOf("."));

                    string file = Application.StartupPath + "\\Temp";
                    addFile(file);

                    string fileName = file + "\\" + txtEdit.Text.Substring(0, txtEdit.Text.LastIndexOf("-")) + "-" + DateTime.Now.Ticks.ToString() + extendName;

                    WebClient client = new WebClient();
                    client.DownloadFile(readPath, fileName);

                    System.Diagnostics.Process.Start(fileName);
                }
                catch
                {
                    lis.client.control.MessageDialog.Show("服务器上不存在此文件", "信息提示");
                }

                return;
            }
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
            {
                if (txtEdit.Text == "未上传")
                {
                    return;
                }

                try
                {
                    string serverPath = ConfigurationManager.AppSettings["wcfAddr"].ToString();
                    string readPath = serverPath + @"uploadFile/" + txtEdit.Text;

                    string extendName = txtEdit.Text.Substring(txtEdit.Text.LastIndexOf("."));

                    SaveFileDialog sfd = new SaveFileDialog();

                    string fileName = txtEdit.Text.Substring(0, txtEdit.Text.LastIndexOf("-")) + extendName;
                    sfd.FileName = fileName;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (sfd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                            return;
                        }
                        WebClient client = new WebClient();
                        client.DownloadFile(readPath, sfd.FileName.Trim());

                        MessageDialog.ShowAutoCloseDialog("下载成功！");
                    }
                }
                catch
                {
                    lis.client.control.MessageDialog.Show("服务器上不存在此文件", "信息提示");
                }

                return;
            }
            if (txtEdit.BackColor != Color.White)
            {
                return;
            }

            txtEdit.Enabled = false;
            //弹出选择文件框以及设置过滤条件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "doc";
            ofd.Filter = "附件 (*.doc,*.xls,*.pdf,*.jpg,*.ppt)|*.doc;*.xls;*.pdf;*.jpg;*.ppt";
            ofd.Title = "选择附件";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\") + 1, ofd.FileName.LastIndexOf(".") - ofd.FileName.LastIndexOf("\\") - 1);
                string extendName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("."));
                string newName = fileName + "-" + DateTime.Now.Ticks.ToString() + extendName;
                string urlName = "uploadFile\\" + newName;
                if (UpLoadFile(ofd.FileName, urlName))
                {
                    lis.client.control.MessageDialog.Show(OfficeMessage.BASE_UPLOAD_SUCCESS, OfficeMessage.BASE_TITLE);
                    txtEdit.Text = newName;
                }
                else
                {
                    lis.client.control.MessageDialog.Show(OfficeMessage.BASE_UPLOAD_ERROR, OfficeMessage.BASE_TITLE);
                }
            }

            txtEdit.Enabled = true;
        }

        public static bool UpLoadFile(string fileNameFullPath, string strUrlDirPath)
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

                ProxyReportMain proxy = new ProxyReportMain();
                bool result = proxy.Service.UpLoadReportFile(request);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("上传失败!" ,ex);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 浏览文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtEdit_Click(object sender, EventArgs e)
        {
            ButtonEdit txtEdit = (ButtonEdit)sender;
            if (txtEdit.Text == "未上传")
            {
                return;
            }

            try
            {
                string serverPath = System.Configuration.ConfigurationManager.AppSettings["wcfAddr"].ToString();
                string readPath = serverPath + @"uploadFile/" + txtEdit.Text;

                string extendName = txtEdit.Text.Substring(txtEdit.Text.LastIndexOf("."));

                string file = Application.StartupPath + "\\Temp";
                addFile(file);

                string fileName = file + "\\" + txtEdit.Text.Substring(0, txtEdit.Text.LastIndexOf("-")) + "-" + DateTime.Now.Ticks.ToString() + extendName;

                WebClient client = new WebClient();
                client.DownloadFile(readPath, fileName);

                System.Diagnostics.Process.Start(fileName);
            }
            catch
            {
                lis.client.control.MessageDialog.Show("服务器上不存在此文件", "信息提示");
            }
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">文件夹的路径</param>
        private void addFile(string path)
        {
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
        }


        #endregion

        private void barOrderDetail_OnBtnAddClicked(object sender, EventArgs e)
        {
            optionStatus = OptionStatus.Insert;

            EnterEditingState(true);

            //新增时清空文本框
            SetEditingNull();

            SetFocus();
        }


        #region 设置焦点


        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <param name="enable"></param>
        private void SetFocus()
        {
            focus = false;
            SetFocus(pnlDetail);
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void SetFocus(Control control)
        {
            if (focus)
            {
                return;
            }

            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                Control c = control.Controls[i];

                if (c is TextEdit || c is HopePopSelect)
                {
                    c.Focus();
                    focus = true;
                    break;
                }
                else
                {
                    SetFocus(c);
                }
            }
        }
        #endregion

        bool focus = false;


        /// <summary>
        /// 设置输入框只读
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(bool enable)
        {
            SetEnable(pnlDetail, enable);
        }

        #region 设置只读

        /// <summary>
        /// 设置只读
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void SetEnable(Control control, bool enable)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit)
                {
                    ((TextEdit)c).Properties.ReadOnly = !enable;
                }

                if (c is ButtonEdit && !(c is DateEdit) && !(c is ComboBoxEdit) && !(c is SpinEdit) && !(c is TimeEdit))
                {
                    //不允许手工录入
                    ((ButtonEdit)c).Properties.ReadOnly = true;

                    if (enable)
                    {
                        c.BackColor = Color.White;
                    }
                    else
                    {
                        c.BackColor = this.BackColor;
                    }
                }

                if (c is UserControl)
                {
                    if (c is SelectDicInstrument)
                        ((SelectDicInstrument)c).Readonly = !enable;
                    if (c is SelectDicLabProfession)
                        ((SelectDicLabProfession)c).Readonly = !enable;
                    if (c is SelectDictSysUser)
                        ((SelectDictSysUser)c).Readonly = !enable;
                }

                if (!(c is TextEdit || c is UserControl))
                {
                    //如果是需要的底层控件不再继续往下
                    SetEnable(c, enable);
                }
            }
        }

        #endregion

        #region 清空文本框

        /// <summary>
        /// 清空文本框
        /// </summary>
        public void SetEditingNull()
        {
            SetNull(pnlDetail);
        }

        #endregion

        #region 清空文本框

        /// <summary>
        /// 清空文本框
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void SetNull(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit && !(c is TimeEdit))
                {
                    ((TextEdit)c).Text = "";
                }

                if (c is TimeEdit)
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                    ((TimeEdit)c).EditValue = Convert.ToDateTime(date);
                }

                if (c is DateEdit)
                {
                    c.Text = "";
                    //((DateEdit)c).EditValue = DateTime.Now;
                }

                if (c is ButtonEdit && !(c is DateEdit) && !(c is ComboBoxEdit) && !(c is SpinEdit) && !(c is TimeEdit))
                {
                    ((ButtonEdit)c).Text = "未上传";
                    if (optionStatus == OptionStatus.Insert)
                    {
                        c.BackColor = Color.White;
                    }
                    else
                    {
                        c.BackColor = this.BackColor;
                    }
                }

                if (c is HopePopSelect)
                {
                    ((HopePopSelect)c).displayMember = "";
                    ((HopePopSelect)c).valueMember = "";
                }

                if (!(c is TextEdit || c is HopePopSelect))
                {
                    //如果是需要的底层控件不再继续往下
                    SetNull(c);
                }
            }
        }

        #endregion

        #region 修改按钮

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (tvOrderDetail.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);

                if (this.ParentForm is FrmCommon)
                {
                    ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
                }

                return;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(true);

            SetFocus();
        }

        #endregion

        #region 取消按钮

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnCancelClicked(object sender, EventArgs e)
        {
            tvOrderDetail_FocusedNodeChanged(null, null);
        }

        #endregion

        #region 选中列表时显示详细信息

        /// <summary>
        /// 选中列表时显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvOrderDetail_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            barOrderDetail.EnableButton(false);
            optionStatus = OptionStatus.Update;
            EnterEditingState(false);

            SetNull(pnlDetail);
            if (tvOrderDetail.Selection.Count > 0)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvOrderDetail.Selection[0];
                SetValue(pnlDetail, tn);
            }

            OnFocusedChanged();
        }

        #endregion

        #region 设置值

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void SetValue(Control control, DevExpress.XtraTreeList.Nodes.TreeListNode tn)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit && !(c is TimeEdit))
                {
                    c.Text = tn.GetValue(tvOrderDetail.Columns["fld" + c.Name.Substring(3)]).ToString();
                }
                if (c is TimeEdit)
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd") + " " + tn.GetValue(tvOrderDetail.Columns["fld" + c.Name.Substring(3)]).ToString();
                    ((TimeEdit)c).EditValue = Convert.ToDateTime(date);
                }
                if (c is UserControl)
                {
                    if (c is SelectDicLabProfession)
                    {
                        SelectDicLabProfession hopePopSelect = (SelectDicLabProfession)c;
                        hopePopSelect.displayMember = tn.GetValue(tvOrderDetail.Columns["fld" + c.Name.Substring(3)]).ToString();

                        //DataRow[] drs = hopePopSelect.dtSource.Select("type_name = '" + hopePopSelect.displayMember + "'");
                        List<EntityDicPubProfession> professions = hopePopSelect.dtSource.FindAll(i => i.ProName == hopePopSelect.displayMember);
                        if (professions.Count > 0)
                        {
                            hopePopSelect.valueMember = professions[0].ProId.ToString();
                            hopePopSelect.displayMember = professions[0].ProName.ToString();
                        }
                    }

                    if (c is SelectDicInstrument)
                    {
                        SelectDicInstrument hopePopSelect = (SelectDicInstrument)c;
                        hopePopSelect.displayMember = tn.GetValue(tvOrderDetail.Columns["fld" + c.Name.Substring(3)]).ToString();

                        //DataRow[] drs = hopePopSelect.dtSource.Select("itr_mid = '" + hopePopSelect.displayMember + "'");
                        List<EntityDicInstrument> instrmts = hopePopSelect.dtSource.FindAll(i => i.ItrEname == hopePopSelect.displayMember);
                        if (instrmts.Count > 0)
                        {
                            hopePopSelect.valueMember = instrmts[0].ItrId.ToString();
                            hopePopSelect.displayMember = instrmts[0].ItrEname.ToString();
                        }
                    }
                }
                else
                {
                    //如果是自定义控件HopePopSelect则不再继续找下一级
                    SetValue(c, tn);
                }
            }
        }

        #endregion

        #region 保存数据

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (dvOrderDetail.Table == null)
            {
                MessageDialog.Show("请先选择物理组!");
                return;
            }
            gcDetail.Focus();

            if (this.ParentForm is FrmCommon)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
            }

            List<EntityOaTableDetail> tableDetail = new List<EntityOaTableDetail>();
            EntityOaTableDetail detail = new EntityOaTableDetail();
            detail.TabCode = this.OrderTypeCode;
            detail.DetCharA = this.S1;
            detail.DetCharB = this.S2;
            detail.DetCharC = this.S3;
            detail.DetCharD = this.S4;
            detail.DetDate = DateTime.Now;
            detail.DetUserId = UserInfo.loginID;
            detail.DetContent = GetItemDetail();

            if (optionStatus == OptionStatus.Update)
            {
                detail.DetId = tvOrderDetail.Selection[0].GetValue(tvOrderDetail.Columns["DetId"]).ToString();
            }

            tableDetail.Add(detail);

            int focusIndex = -1;
            if (optionStatus == OptionStatus.Insert)
            {
                //Insert
                bool newResult = proxyDetail.Service.InsertNewTabDetail(detail);
                #region 论文情况_新增时_同时添加_论文发表

                //系统配置：人员资料[加]论文情况同时[加]论文发表
                //数据格式：┏00000041┏&00000170&┏00000040┏&00000166&┏00000039┏&00000167&┏00000038┏&00000168&┏00000037┏&00000106&┏00000036┏&递增序号&
                string StaffMg_Addlwfb_OrderDetail = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("StaffMg_Addlwfb_OrderDetail");

                //论文情况 23,新增时,同时添加 论文发表 6
                if (this.OrderTypeCode != null && this.OrderTypeCode == "23"
                    && detail != null && detail.DetCharA.ToString().Length > 0
                    && !string.IsNullOrEmpty(StaffMg_Addlwfb_OrderDetail))
                {
                    EntityOaTableDetail tabDetail = new EntityOaTableDetail();
                    tabDetail.DetId = detail.DetCharA;
                    //字段名称
                    List<string> ltOrderDetailColNm = new List<string>();
                    //字段值
                    List<string> ltOrderDetailColVe = new List<string>();

                    //获取 论文情况 对应的 个人资料信息
                    List<EntityOaTableDetail> listDetail = proxyDetail.Service.GetTabDetailByTabCode(tabDetail);

                    if (listDetail != null && listDetail.Count > 0)
                    {
                        foreach (EntityOaTableDetail thisDr in listDetail)
                        {
                            string[] orderDetail = thisDr.DetContent.Split(split);

                            for (int i = 1; i < orderDetail.Length - 1; i = i + 2)
                            {
                                string tmpDetail = (int.Parse(orderDetail[i])).ToString(ZeroFormat);
                                ltOrderDetailColNm.Add("&" + tmpDetail + "&");
                                ltOrderDetailColVe.Add(orderDetail[i + 1]);
                            }
                        }
                    }

                    //论文情况 23 内容
                    foreach (EntityOaTableDetail thisDr in tableDetail)
                    {
                        string[] orderDetail = thisDr.DetContent.Split(split);

                        for (int i = 1; i < orderDetail.Length - 1; i = i + 2)
                        {
                            string tmpDetail = (int.Parse(orderDetail[i])).ToString(ZeroFormat);
                            ltOrderDetailColNm.Add("&" + tmpDetail + "&");
                            ltOrderDetailColVe.Add(orderDetail[i + 1]);
                        }
                    }

                    //获取 所有 论文发表 6 内容
                    tabDetail.DetId = string.Empty;
                    tabDetail.TabCode = "6";
                    listDetail = proxyDetail.Service.GetTabDetailByTabCode(tabDetail);

                    //获取 论文发表 数量,从而得出递增序号
                    if (listDetail != null && listDetail.Count > 0)
                    {
                        ltOrderDetailColNm.Add("&递增序号&");
                        ltOrderDetailColVe.Add((listDetail.Count + 1).ToString());
                    }
                    else
                    {
                        ltOrderDetailColNm.Add("&递增序号&");
                        ltOrderDetailColVe.Add("1");
                    }

                    //论文发表 6 内容
                    //┏00000041┏d┏00000040┏测试2┏00000039┏d┏00000038┏2015/7/10┏00000037┏first┏00000036┏1
                    //个人资料 15 内容
                    //┏00000120┏┏00000118┏认为┏00000119┏┏00000117┏┏00000116┏┏00000115┏┏00000114┏┏00000113┏┏00000112┏353┏00000111┏353┏00000110┏35┏00000109┏53┏00000108┏2015-06-07┏00000252┏ICU2区┏00000107┏男┏00000106┏452┏00000105┏1

                    //替换内容
                    for (int i = 0; i < ltOrderDetailColNm.Count; i++)
                    {
                        StaffMg_Addlwfb_OrderDetail = StaffMg_Addlwfb_OrderDetail.Replace(ltOrderDetailColNm[i], ltOrderDetailColVe[i]);
                    }

                    detail.TabCode = "6";
                    detail.DetCharA = string.Empty;
                    detail.DetCharB = string.Empty;
                    detail.DetCharC = string.Empty;
                    detail.DetCharC = string.Empty;
                    detail.DetContent = StaffMg_Addlwfb_OrderDetail;

                    bool result = proxyDetail.Service.InsertNewTabDetail(detail);
                }

                #endregion
                //lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SAVE_SUCCESS, OfficeMessage.BASE_TITLE);
            }
            else
            {
                //Update
                focusIndex = tvOrderDetail.GetVisibleIndexByNode(tvOrderDetail.FocusedNode);

                bool result = proxyDetail.Service.UpdateTabDetail(detail);
                //lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SAVE_SUCCESS, OfficeMessage.BASE_TITLE);
            }

            if (this.ParentForm is FrmCommon)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = true;
                barOrderDetail.LogMessage = "保存成功";
            }

            LoadList(false);

            if (optionStatus == OptionStatus.Update && tvOrderDetail.GetNodeByVisibleIndex(focusIndex) != null)
            {
                //重新设置选中编辑行_和判断是否可切换行一样,使用了BtnSave.Enabled作为标志,所以需要提前更改
                barOrderDetail.BtnSave.Enabled = false;
                tvOrderDetail.SetFocusedNode(tvOrderDetail.GetNodeByVisibleIndex(focusIndex));
                tvOrderDetail.Focus();
            }
        }

        #endregion

        #region 获取表单数据

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <returns></returns>
        private string GetItemDetail()
        {
            string result = "";
            GetValue(pnlDetail, ref result);
            return result;
        }
        #endregion

        #region 获得文本数据

        /// <summary>
        /// 获得文本数据
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enable"></param>
        private void GetValue(Control control, ref string result)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit || c is UserControl)
                {
                    result += (split + c.Name.Substring(3));
                }

                //这几个控件居然有继承关系……
                if (c is TextEdit)
                {
                    result += (split + ((TextEdit)c).Text.Trim().Replace(split.ToString(), ""));
                }

                if (c is UserControl)
                {
                    if (c is SelectDicInstrument)
                    {
                        result += (split + ((SelectDicInstrument)c).displayMember.Trim().Replace(split.ToString(), ""));
                    }
                    if (c is SelectDicLabProfession)
                    {
                        result += (split + ((SelectDicLabProfession)c).displayMember.Trim().Replace(split.ToString(), ""));
                    }
                    if (c is SelectDictSysUser)
                    {
                        result += (split + ((SelectDictSysUser)c).displayMember.Trim().Replace(split.ToString(), ""));
                    }

                }

                if (!(c is TextEdit || c is UserControl))
                {
                    //如果是需要的底层控件则不再继续往下
                    GetValue(c, ref result);
                }
            }
        }

        #endregion

        #region 删除按钮

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (tvOrderDetail.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }

            if (this.ParentForm is FrmCommon)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
            }

            EntityOaTableDetail detail = new EntityOaTableDetail();
            detail.DetId = tvOrderDetail.Selection[0].GetValue(tvOrderDetail.Columns["DetId"]).ToString();

            DialogResult dresult = MessageBox.Show(OfficeMessage.BASE_DELETE_CONFIRM, OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    {
                        proxyDetail.Service.DeleteTabetail(detail);
                        break;
                    }
                case DialogResult.Cancel:
                    return;
            }

            if (this.ParentForm is FrmCommon)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = true;
                barOrderDetail.LogMessage = "删除单证[" + detail.DetId + "]成功";
            }

            //删除记录后重新加载列表
            LoadList(false);

            if (tvOrderDetail.AllNodesCount == 0)
            {
                tvOrderDetail_FocusedNodeChanged(null, null);
            }
        }

        #endregion

        #region 查询按钮_暂未使用

        /// <summary>
        /// 查询按钮_暂未使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnSearchClicked(object sender, EventArgs e)
        {
            if (tvOrderDetail.DataSource != null)
            {
                DataTable dt = new DataTable();

                for (int i = 0; i < listField.Count; i++)
                {
                    string colName = listField[i].FieldCode.ToString();
                    string caption = listField[i].FieldName.ToString();
                    string typeString = listField[i].FieldType.ToString();

                    Type type = System.Type.GetType("System.String");

                    switch (typeString)
                    {
                        case "日期":
                            {
                                type = System.Type.GetType("System.DateTime");
                                break;
                            }
                        case "数字":
                            {
                                type = System.Type.GetType("System.Decimal");
                                break;
                            }
                        default: break;
                    }

                    DataColumn col = new DataColumn("fld" + int.Parse(colName).ToString(OrderDetail.ZeroFormat), type);
                    col.Caption = caption;
                    dt.Columns.Add(col);
                }

                FrmFilter frmFilter = new FrmFilter(dt, filterString);

                if (frmFilter.ShowDialog() == DialogResult.OK)
                {
                    dvOrderDetail.RowFilter = frmFilter.filterString;
                }
            }
        }

        #endregion

        #region 导出列表

        /// <summary>
        /// 导出列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (tvOrderDetail.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", OfficeMessage.BASE_TITLE);
                        return;
                    }

                    try
                    {
                        if (true)
                        {
                            //如果是新路径,则生成新文件导出
                            if (File.Exists(ofd.FileName))
                            {
                                InsertDataToExcel(ofd.FileName, ofd.FileName);
                            }
                            else
                            {
                                InsertDataToExcel2(ofd.FileName);
                            }

                            lis.client.control.MessageDialog.Show("导出成功！", OfficeMessage.BASE_TITLE);

                        }
                        else
                        {
                            tvOrderDetail.ExportToXls(ofd.FileName);
                            lis.client.control.MessageDialog.Show("导出成功！", OfficeMessage.BASE_TITLE);
                        }
                    }
                    catch (Exception ex)
                    {
                        lis.client.control.MessageDialog.Show("导出失败,遇到错误,请查看日志");
                    }
                }

            }

        }

        /// <summary>
        /// 导出excel(导到指定文件中,后面追加内容)
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="decPath"></param>
        private void InsertDataToExcel(string srcPath, string decPath)
        {
            try
            {
                #region 导出方法

                using (FileStream fs = new FileStream(srcPath, FileMode.Open, FileAccess.Read))
                {
                    //======= 1. 读取模板文件 ==========
                    //FileStream fs = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                    HSSFWorkbook book = new HSSFWorkbook(fs);

                    ISheet sheet1 = book.GetSheetAt(0);
                    //sw.Start();
                    DataTable dtExcalInfo = new DataTable();

                    if (tvOrderDetail.DataSource != null && tvOrderDetail.DataSource is DataView)
                    {
                        dtExcalInfo = ((DataView)tvOrderDetail.DataSource).ToTable();
                    }

                    List<string> lsColNms = new List<string>();//字段名称
                    List<string> lsCaptions = new List<string>();//显示名称

                    //sw.Stop();

                    int noteTempStartIndex = 0;//记录开始添加行

                    int NullRowCount = 0;

                    for (int i = 0; i < 10000; i++)//遍历没excel表格，寻找空白行
                    {
                        noteTempStartIndex = i;
                        IRow row = sheet1.GetRow(i);
                        if (row == null)
                        {
                            NullRowCount++;

                            if (NullRowCount > 5)//空格大于5个才确认
                            {
                                noteTempStartIndex = (noteTempStartIndex + 1 - NullRowCount);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (string.IsNullOrEmpty(row.GetCell(0).StringCellValue))
                        {
                            NullRowCount++;
                            if (NullRowCount > 5)//空格大于5个才确认
                            {
                                noteTempStartIndex = (noteTempStartIndex + 1 - NullRowCount);
                                break;
                            }
                        }
                    }

                    for (int j = 0; j < tvOrderDetail.Columns.Count; j++)
                    {
                        if (tvOrderDetail.Columns[j].Visible)
                        {
                            lsColNms.Add(tvOrderDetail.Columns[j].FieldName);
                            lsCaptions.Add(tvOrderDetail.Columns[j].Caption);
                        }
                    }

                    //添加显示的列名
                    if (lsCaptions.Count > 0)
                    {
                        int tempRowrum = 0;//从第几行开始添加

                        if (noteTempStartIndex > 0)//如果不是第一行，则相隔2行再添加
                        {
                            tempRowrum = noteTempStartIndex + 2;
                            noteTempStartIndex = tempRowrum;
                        }

                        IRow row = sheet1.CreateRow(tempRowrum);
                        for (int j = 0; j < lsCaptions.Count; j++)
                        {
                            row.CreateCell(j).SetCellValue(lsCaptions[j]);
                        }
                    }

                    for (int j = 0; j < dtExcalInfo.Rows.Count; j++)
                    {
                        noteTempStartIndex++;
                        IRow row = sheet1.CreateRow(noteTempStartIndex);
                        for (int k = 0; k < lsColNms.Count; k++)
                        {
                            row.CreateCell(k).SetCellValue(dtExcalInfo.Rows[j][lsColNms[k]].ToString());
                        }
                    }

                    //======= 4. 写入文件 ==========
                    FileStream file = new FileStream(decPath, FileMode.Create);
                    book.Write(file);
                    file.Close();
                    fs.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(GetType().FullName, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 导出excel(生成新文件)
        /// </summary>
        /// <param name="decPath"></param>
        private void InsertDataToExcel2(string decPath)
        {
            try
            {
                #region 导出方法

                if (true)
                {
                    //======= 1. 生成模板文件 ==========
                    HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

                    DataTable dtExcalInfo = new DataTable();

                    if (tvOrderDetail.DataSource != null && tvOrderDetail.DataSource is DataView)
                    {
                        dtExcalInfo = ((DataView)tvOrderDetail.DataSource).ToTable();
                    }

                    List<string> lsColNms = new List<string>();//字段名称
                    List<string> lsCaptions = new List<string>();//显示名称

                    //sw.Stop();

                    int noteTempStartIndex = 0;//记录开始添加行

                    for (int j = 0; j < tvOrderDetail.Columns.Count; j++)
                    {
                        if (tvOrderDetail.Columns[j].Visible)
                        {
                            lsColNms.Add(tvOrderDetail.Columns[j].FieldName);
                            lsCaptions.Add(tvOrderDetail.Columns[j].Caption);
                        }
                    }

                    //添加显示的列名
                    if (lsCaptions.Count > 0)
                    {
                        int tempRowrum = 0;//从第几行开始添加

                        if (noteTempStartIndex > 0)//如果不是第一行，则相隔2行再添加
                        {
                            tempRowrum = noteTempStartIndex + 2;
                            noteTempStartIndex = tempRowrum;
                        }

                        IRow row = sheet1.CreateRow(tempRowrum);
                        for (int j = 0; j < lsCaptions.Count; j++)
                        {
                            row.CreateCell(j).SetCellValue(lsCaptions[j]);
                        }
                    }

                    for (int j = 0; j < dtExcalInfo.Rows.Count; j++)
                    {
                        noteTempStartIndex++;
                        IRow row = sheet1.CreateRow(noteTempStartIndex);
                        for (int k = 0; k < lsColNms.Count; k++)
                        {
                            row.CreateCell(k).SetCellValue(dtExcalInfo.Rows[j][lsColNms[k]].ToString());
                        }
                    }

                    //======= 4. 写入文件 ==========
                    FileStream file = new FileStream(decPath, FileMode.Create);
                    book.Write(file);
                    file.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(GetType().FullName, ex);
                throw ex;
            }
        }

        #endregion

        #region 重置过滤条件

        /// <summary>
        /// 重置过滤条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            LoadList(false);
        }

        #endregion

        #region 在选择节点时判断是否处于新增或编辑状态,避免移动

        /// <summary>
        /// 在选择节点时判断是否处于新增或编辑状态,避免移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvOrderDetail_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            if (barOrderDetail.BtnSave.Enabled == true)
            {
                e.CanFocus = false;
            }
        }
        #endregion

        #region 复制单证

        /// <summary>
        /// 复制单证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barOrderDetail_BtnCopyClick(object sender, EventArgs e)
        {
            if (tvOrderDetail.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }

            if (this.ParentForm is FrmCommon)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = false;
            }

            EntityOaTableDetail detail = new EntityOaTableDetail();
            detail.DetId = tvOrderDetail.Selection[0].GetValue(tvOrderDetail.Columns["DetId"]).ToString();
            bool copyResult = proxyDetail.Service.CopyOneDetailToNew(detail);


            if (this.ParentForm is FrmCommon && copyResult)
            {
                ((FrmCommon)(this.ParentForm)).isActionSuccess = true;
                barOrderDetail.LogMessage = "复制单证[" + detail.DetId + "]成功";
            }

            //复制记录后重新加载列表
            LoadList(false);

            if (tvOrderDetail.AllNodesCount == 0)
            {
                tvOrderDetail_FocusedNodeChanged(null, null);
            }
        }

        #endregion

        #region 通过日期范围筛选

        /// <summary>
        /// 通过日期范围筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarOrderDetail_OnBtnSearchClicked(object sender, EventArgs e)
        {
            DateTime dtiemStart = Convert.ToDateTime(this.dateEditStart.EditValue);//开始日期
            DateTime dtiemEnd = Convert.ToDateTime(this.dateEditEnd.EditValue);//结束日期

            if (dtiemStart == null || dtiemEnd == null || dtiemStart == DateTime.MinValue || dtiemEnd == DateTime.MinValue)
            {
                return;
            }

            if (dtiemStart.Date >= dtiemEnd.AddDays(1).Date)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择正确的日期范围");
                return;
            }


            string filter = " DetDate>='" + dtiemStart.Date.ToString() + "' and DetDate<'" + dtiemEnd.AddDays(1).Date.ToString() + "'";
            filterString = this.setFilterStr(filter);

            dvOrderDetail.RowFilter = filterString;

            tvOrderDetail.BestFitColumns(false);
        }


        #endregion

        /// <summary>
        /// 过滤人员资料信息-只能查看自己的
        /// </summary>
        /// <param name="oldFilertStr"></param>
        /// <returns></returns>
        private string setFilterStr(string oldFilertStr)
        {
            //人员资料的，权限-只能查看自己的
            if (!string.IsNullOrEmpty(this.OrderTypeCode) && this.OrderTypeCode == "15" && !dcl.client.frame.UserInfo.isAdmin)
            {
                if (!string.IsNullOrEmpty(this.FindForm().Name) && !string.IsNullOrEmpty(this.Name)
                    && this.FindForm().Name == "FrmStaffManage" && this.Name == "orderMain")
                {
                    bool blnIs = false;
                    if (UserInfo.entityUserInfo.Func!= null
                        && UserInfo.entityUserInfo.Func.Where(w=>w.FuncCode== "fun_office_StaffManage_Look_Self").ToList().Count > 0)
                    {
                        blnIs = true;
                    }

                    if (tvOrderDetail != null && tvOrderDetail.Columns.Count > 0 && blnIs)
                    {
                        foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in tvOrderDetail.Columns)
                        {
                            if (col.Caption != null && col.Caption == "姓名" && !string.IsNullOrEmpty(col.FieldName))
                            {
                                if (dvOrderDetail.Table.Columns != null
                                    && dvOrderDetail.Table.Columns.Contains(col.FieldName))
                                {
                                    if (!string.IsNullOrEmpty(oldFilertStr) && oldFilertStr.Trim().Length > 1)
                                    {
                                        oldFilertStr += " and " + col.FieldName + "='" + dcl.client.frame.UserInfo.userName + "'";
                                    }
                                    else
                                    {
                                        oldFilertStr = " " + col.FieldName + "='" + dcl.client.frame.UserInfo.userName + "'";
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return oldFilertStr;
        }

        #region DesignMode


        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }


        #endregion
        
    }

}
