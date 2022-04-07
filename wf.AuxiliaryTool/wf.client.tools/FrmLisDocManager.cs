using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using dcl.entity;
using dcl.client.wcf;
using System.Windows;
using DevExpress.XtraRichEdit;
using System.IO;
using lis.client.control;
using DevExpress.XtraNavBar;
using System.Reflection;
using DevExpress.XtraNavBar.ViewInfo;

namespace dcl.client.tools
{
    public partial class FrmLisDocManager : DevExpress.XtraEditors.XtraForm
    {
        public FrmLisDocManager()
        {
            InitializeComponent();

            Init();
        }

        /// <summary>
        /// 所有文档
        /// </summary>
        private List<EntityLisDoc> listDoc = null;

        /// <summary>
        /// 当前编辑文档
        /// </summary>
        private EntityLisDoc currentDoc = null;
        
        private void Init()
        {
            //设置默认的起始日期
            this.deBeginDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.deBeginDate.DateTime = DateTime.Now.AddMonths(-1);
            this.deEndDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.deEndDate.DateTime = DateTime.Now;

            this.cbCurrentDocType.Properties.Items.Add("模板文档");
            this.cbCurrentDocType.Properties.Items.Add("数据文档");

            this.RefreshForm();

            this.navBarGroupModel.Expanded = true;

            this.navBarGroupResult.Expanded = false;
        }


        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var item = (NavBarItem)sender;
            currentDoc = listDoc.Find(o => o.docId.ToString() == item.Tag.ToString());
            this.cbCurrentDocDate.Text = currentDoc.docDate.ToShortDateString();
            this.cbCurrentModelType.Text = currentDoc.docTitle;
            this.cbCurrentDocType.SelectedIndex = Convert.ToInt32(currentDoc.docType);
            this.richEditControl1.LoadDocument(new MemoryStream(Convert.FromBase64String(currentDoc.docContent)), DocumentFormat.Doc);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cbQueryModelType.Text))
            {
                MessageDialog.Show("请先选择模板类型！", "提示");
                this.cbQueryModelType.Focus();
                return;
            }

            this.navBarGroupModel.Expanded = false;

            this.navBarGroupResult.Expanded = true;

            this.navBarGroupResult.ItemLinks.Clear();

            //筛选条件：起止日期和类型
            DateTime beginDate = this.deBeginDate.DateTime;
            DateTime endDate = this.deEndDate.DateTime;
            string docTitle = this.cbQueryModelType.Text;

            ProxyLisDoc proxy = new ProxyLisDoc();

            var resultList = proxy.Service.Query(beginDate, endDate, docTitle);

            //listDoc.FindAll(o => o.docDate > beginDate && o.docDate < endDate && o.docTitle == docTitle && o.docType == "0");

            foreach (var result in resultList.OrderBy(o => o.docDate))
            {
                var item = this.navBarControl1.Items.Add();

                //this.navBarControl1.MouseDown += NavBarControl1_MouseDown;
                item.Caption = result.docDate.ToShortDateString() + "   " + result.docTitle;
                item.LinkClicked += Item_LinkClicked;
          
                item.Tag = result.docId;
                this.navBarGroupResult.ItemLinks.Add(item);
            }
        }

        /// <summary>
        /// item 右键单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavBarControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                return;
            }
            NavBarHitInfo hit = navBarControl1.CalcHitInfo(e.Location);
            if ((hit.InLink))
            {
                FieldInfo fi = typeof(NavBarControl).GetField("viewInfo", BindingFlags.NonPublic | BindingFlags.Instance);
                NavBarViewInfo vi = fi.GetValue(navBarControl1) as NavBarViewInfo;
                NavLinkInfoArgs arg = vi.GetLinkInfo(hit.Link);
                Point p = new Point(arg.Bounds.X, arg.Bounds.Bottom);
                popupMenu1.ShowPopup(navBarControl1.PointToScreen(p));
            }
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cbCurrentDocDate.Text))
            {
                MessageDialog.Show("请输入文档日期！", "提示");
                this.cbCurrentDocDate.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.cbCurrentModelType.Text))
            {
                MessageDialog.Show("请输入模板类型！", "提示");
                this.cbCurrentModelType.Focus();
                return;
            }

            EntityLisDoc doc = new EntityLisDoc();
            doc.docDate = this.cbCurrentDocDate.DateTime; //文档日期
            doc.docType = this.cbCurrentDocType.SelectedIndex.ToString();  //文档类型
            doc.docTitle = this.cbCurrentModelType.SelectedItem.ToString();  //文档标题

            this.richEditControl1.Document.SaveDocument("lis.doc", DocumentFormat.Doc);
            
            doc.docContent = Convert.ToBase64String(File.ReadAllBytes("lis.doc"));

            ProxyLisDoc proxy = new ProxyLisDoc();
            if (proxy.Service.Save(doc) < 0)
            {
                MessageDialog.Show("保存出错！");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存成功！");
            }
            this.RefreshForm();
        }


        private void cbCurrentDocType_TextChanged(object sender, EventArgs e)
        {
            if(this.cbCurrentDocType.Text == "数据文档")
            {
                this.cbCurrentDocDate.Properties.ReadOnly = false;
                this.cbCurrentDocDate.Text = DateTime.Now.ToShortDateString();
                if (!this.cbCurrentModelType.Properties.Items.Contains(this.cbCurrentModelType.Text))
                {
                    this.cbCurrentModelType.Text = this.cbCurrentModelType.Properties.Items[0].ToString();
                }
                this.cbCurrentModelType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            }
            else
            {
                this.cbCurrentModelType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                this.cbCurrentDocDate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                this.cbCurrentDocDate.Text = DateTime.Now.ToShortDateString();
                this.cbCurrentDocDate.Properties.ReadOnly = true;
            }
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        protected void RefreshForm()
        {
            ProxyLisDoc proxy = new ProxyLisDoc();
            listDoc = proxy.Service.QueryAll();

            if(listDoc.Count == 0)
            {
                MessageDialog.Show("当前没有文档，请先创建模板文档！");
                return;
            }
            List<String> modelType = listDoc.FindAll(o => o.docType == "0").Select(a => a.docTitle).ToList();

            this.cbQueryModelType.Properties.Items.Clear();
            this.cbQueryModelType.Properties.Items.AddRange(modelType);
            if(this.cbCurrentModelType.SelectedIndex < 0)
            {
                this.cbQueryModelType.SelectedIndex = 0;
            }
            this.cbCurrentModelType.Properties.Items.Clear();
            this.cbCurrentModelType.Properties.Items.AddRange(modelType);
            this.cbCurrentModelType.SelectedIndex = 0;

            this.navBarControl1.Items.Clear();
            foreach (var doc in listDoc.FindAll(o => o.docType == "0"))
            {
                var item = this.navBarControl1.Items.Add();
                item.Caption = doc.docTitle;
                item.LinkClicked -= Item_LinkClicked;
                item.LinkClicked += Item_LinkClicked;
                item.Tag = doc.docId;
                this.navBarGroupModel.ItemLinks.Add(item);
            }
        }

        private void deleteItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ProxyLisDoc proxy = new ProxyLisDoc();
            if (proxy.Service.Delete(currentDoc) >= 0)
            {
                MessageDialog.Show("删除成功！");
            }
        }
    }
}