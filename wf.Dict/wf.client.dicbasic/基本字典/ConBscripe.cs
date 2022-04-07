using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using dcl.common;
using DevExpress.XtraGrid.Columns;
using dcl.client.wcf;

using lis.client.control;
using dcl.entity;
using System.Linq;

namespace dcl.client.dicbasic
{
    public partial class ConBscripe : ConDicCommon, IBarActionExt
    {

        #region 保存数据表临时过滤信息
        /// <summary>
        /// 过滤编码信息
        /// </summary>
        ColumnFilterInfo cfiId = new ColumnFilterInfo();
        /// <summary>
        /// 过滤诊断信息
        /// </summary>
        ColumnFilterInfo cfiScript = new ColumnFilterInfo();

        /// <summary>
        /// 是否为新增事件
        /// </summary>
        bool blIsNew = false;
        #endregion
        private List<EntityDicPubEvaluate> list = new List<EntityDicPubEvaluate>();
        #region IBarAction 成员

        public void Add()
        {
            //标记为新增事件
            this.blIsNew = true;
            #region 保存过滤信息到临时存储里
            cfiId = colbr_id.FilterInfo;
            cfiScript = colbr_scripe.FilterInfo;
            #endregion

            #region 清空过滤信息

            colbr_id.FilterInfo = new ColumnFilterInfo();
            colbr_scripe.FilterInfo = new ColumnFilterInfo();
            #endregion
            memoEdit1.Focus();
            EntityDicPubEvaluate dr = (EntityDicPubEvaluate)bsBscript.AddNew();
            dr.EvaId = string.Empty;
            memoEdit1.Focus();
            this.gridControl1.Enabled = false;
        }

        public void Save()
        {
            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }
            EntityDicPubEvaluate DicPubEvaluate = (EntityDicPubEvaluate)bsBscript.Current;
            EntityDicPubEvaluate dr = new EntityDicPubEvaluate();
            dr.EvaTitle = txtTitle.Text;
            dr.EvaId = textEdit1.Text;
            dr.EvaContent = memoEdit1.Text;
            dr.EvaUserId = DicPubEvaluate.EvaUserId;
            dr.EvaCcode = buttonEdit6.Text;
            //dr.EvaSamName = cboSamId.displayMember;
            if (string.IsNullOrEmpty(buttonEdit1.Text))
            {
                dr.EvaSortNo = 0;
            }
            else {
                dr.EvaSortNo = int.Parse(buttonEdit1.Text);
            }
            switch (this.txtTemplateType.Text)
            {
                case "":
                    dr.EvaFlag = string.Empty;
                    break;
                case "报告评价":
                    dr.EvaFlag = "0";
                    break;
                case "处理意见":
                    dr.EvaFlag = "1";
                    break;
                case "细菌备注":
                    dr.EvaFlag = "2";
                    break;
                case "描述报告":
                    dr.EvaFlag = "3";
                    break;
                case "菌落计数":
                    dr.EvaFlag = "4";
                    break;
                case "质控失控原因":
                    dr.EvaFlag = "5";
                    break;
                case "质控解决措施":
                    dr.EvaFlag = "6";
                    break;
                case "危急类型":
                    dr.EvaFlag = "7";
                    break;
                case "标本超时签收理由":
                    dr.EvaFlag = "8";
                    break;
                case "标本超时拒绝理由":
                    dr.EvaFlag = "9";
                    break;
                case "自编危急范文":
                    dr.EvaFlag = "10";
                    break;
                case "细菌临时报告结论":
                    dr.EvaFlag = "11";
                    break;
                case "危急值信息原因":
                    dr.EvaFlag = "12";
                    break;
                case "危急值反馈信息":
                    dr.EvaFlag = "13";
                    break;
                case "交接班字典信息":
                    dr.EvaFlag = "14";
                    break;
                case "危急值记录信息原因":
                    dr.EvaFlag = "15";
                    break;
                case "临床危急值备注":
                    dr.EvaFlag = "16";
                    break;
                case "处理结果":
                    dr.EvaFlag = "17";
                    break;
                case "审核结果":
                    dr.EvaFlag = "18";
                    break;
                case "院感监测对象":
                    dr.EvaFlag = "19";
                    break;
                case "复查意见":
                    dr.EvaFlag = "20";
                    break;
                case "质控结果分析与处理":
                    dr.EvaFlag = "21";
                    break;
                case "骨髓描述":
                    dr.EvaFlag = "22";
                    break;
                case "保养仪器描述":
                    dr.EvaFlag = "23";
                    break;
                case "报损原因":
                    dr.EvaFlag = "24";
                    break;
                case "到达温度":
                    dr.EvaFlag = "25";
                    break;
                case "外包装":
                    dr.EvaFlag = "26";
                    break;
                case "检验报告":
                    dr.EvaFlag = "27";
                    break;
            }

            String br_id = dr.EvaId.ToString();

            EntityRequest request = new EntityRequest();
            StringBuilder instrmtInfo = new StringBuilder();
            foreach (TreeNode typenode in this.typeNodeDict.Values)
            {
                foreach (TreeNode node in typenode.Nodes)
                {
                    if (node.Checked)
                    {
                        if (instrmtInfo.Length > 0)
                        {
                            instrmtInfo.Append(",");
                        }
                        instrmtInfo.Append(node.Tag.ToString());
                    }
                }

            }
            dr.EvaItrId = instrmtInfo.ToString();

            if (string.IsNullOrEmpty(cboSamId.valueMember))//标本
            {
                dr.EvaSamId = null;
                dr.EvaSamName = null;
            }
            else
            {
                dr.EvaSamId = cboSamId.valueMember;
                dr.EvaSamName = cboSamId.displayMember;
            }
            if (br_id == "")
            {
                if (UserInfo.loginID != "admin")
                {
                    dr.EvaUserId = UserInfo.loginID;
                }
            }
            request.SetRequestValue(dr);
            EntityResponse result = new EntityResponse();
            if (br_id == "")
            {
                result = base.New(request);
            }
            else
            {
                result = base.Update(request);
            }
            if (base.isActionSuccess)
            {
                DoRefresh();

            }
            //新增事件处理
            if (blIsNew)
            {
                colbr_id.FilterInfo = cfiId;
                colbr_scripe.FilterInfo = cfiScript;
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Delete()
        {

            this.bsBscript.EndEdit();
            if (bsBscript.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDicPubEvaluate dr = (EntityDicPubEvaluate)bsBscript.Current;
            String eva_id = dr.EvaId;

            request.SetRequestValue(dr);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request);
                if (base.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    this.DoRefresh();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public void DoRefresh()
        {
            EntityRequest request = new EntityRequest();
            EntityDicPubEvaluate dr = new EntityDicPubEvaluate();
            request.SetRequestValue(dr);
            EntityResponse ds = base.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult() as List<EntityDicPubEvaluate>;
                foreach (EntityDicPubEvaluate eva in list)
                {
                    if (!Compare.IsNullOrDBNull(eva.EvaFlag)
                        && !string.IsNullOrEmpty(eva.EvaFlag)
                        )
                    {
                        int index = int.Parse(eva.EvaFlag);
                        if (index < this.txtTemplateType.Properties.Items.Count)
                        {
                            eva.EvaFlagText = this.txtTemplateType.Properties.Items[index].ToString();
                        }
                    }
                    else
                    {
                        eva.EvaFlagText = string.Empty;
                    }
                }
                this.bsBscript.DataSource = list;
            }
        }

        public Dictionary<string, Boolean> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridControl1", true);

            return dlist;
        }

        #endregion
       
        public ConBscripe()
        {
            InitializeComponent();

            this.treeView1.CheckBoxes = true;
            this.treeView1.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);
            this.bsBscript.CurrentChanged += new EventHandler(bsBscript_CurrentChanged);
        }
        bool isupdatting = false;
        void bsBscript_CurrentChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                node.Checked = false;
            }
            isupdatting = true;
            if (this.bsBscript.Current != null)
            {
                EntityDicPubEvaluate row = this.bsBscript.Current as EntityDicPubEvaluate;
                txtTitle.Text=row.EvaTitle;
                textEdit1.Text = row.EvaId;
                memoEdit1.Text = row.EvaContent;
                txtTemplateType.Text=row.EvaFlagName;
                buttonEdit6.Text=row.EvaCcode;
                buttonEdit1.Text = row.EvaSortNo.ToString();
                cboSamId.displayMember = row.EvaSamName;
                if ( !string.IsNullOrEmpty(row.EvaItrId))
                {
                    string br_InstrmtInfo = row.EvaItrId.ToString();
                    string[] instrmtArray = br_InstrmtInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> instrmtList = new List<string>(instrmtArray);
                    foreach (TreeNode typenode in this.typeNodeDict.Values)
                    {
                        int i = 0;
                        foreach (TreeNode node in typenode.Nodes)
                        {
                            if (instrmtList.Contains(node.Tag.ToString()))
                            {
                                node.Checked = true;
                                i++;
                            }
                        }
                        if (i == typenode.Nodes.Count)
                        {
                            typenode.Checked = true;
                        }
                    }
                }

                if (string.IsNullOrEmpty(row.EvaSamId))
                {
                    cboSamId.displayMember = row.EvaSamName;
                    cboSamId.valueMember = row.EvaSamId;
                }
            }
            isupdatting = false;
        }

        void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!isupdatting)
            {
                foreach (TreeNode item in e.Node.Nodes)
                {
                    item.Checked = e.Node.Checked;
                }
            }
        }
        Dictionary<string, TreeNode> typeNodeDict = new Dictionary<string, TreeNode>();
        void LoadInstrmtInfo()
        {

            typeNodeDict.Clear();
            List<EntityDicInstrument> list = new List<EntityDicInstrument>();
            EntityRequest request = new EntityRequest();
            ProxyCommonDic proxy = new ProxyCommonDic("svc.ConInstrmt");
            EntityResponse ds = proxy.Search(request);
            if (isActionSuccess)
            {
                list = ds.GetResult()as List<EntityDicInstrument>;
            }
            
            foreach (EntityDicInstrument item in list)
            {
                TreeNode typeNode = null;
                if (!typeNodeDict.ContainsKey(item.ItrTypeName))
                {
                    typeNode = new TreeNode(item.ItrTypeName);
                    typeNodeDict.Add(item.ItrTypeName, typeNode);
                    this.treeView1.Nodes.Add(typeNode);

                }
                else
                {
                    typeNode = typeNodeDict[item.ItrTypeName];
                }
                TreeNode instrmtNode = new TreeNode(item.ItrName);
                instrmtNode.Tag = item.ItrId;
                typeNode.Nodes.Add(instrmtNode);
            }
        }

        private void on_Load(object sender, EventArgs e)
        {
            this.initData();
        }

        private void initData()
        {
            LoadInstrmtInfo();
            this.DoRefresh();
        }

        private void txtSort_EditValueChanged(object sender, EventArgs e)
        {
            if (this.cmbSort.Text != "")
            {
                if (this.txtSort.Text == "")
                {
                    DoRefresh();
                }
                else
                {
                    string sortText = txtSort.Text;

                    if (this.cmbSort.Text == "编码")
                    {
                        bsBscript.DataSource = list.Where(w => w.EvaId.Contains(sortText)).ToList();
                    }
                    if (this.cmbSort.Text == "描述内容")
                    {
                        bsBscript.DataSource = list.Where(w => w.EvaContent!=null && w.EvaContent.Contains(sortText)).ToList();
                    }
                }
            }
        }

        #region IBarActionExt 成员

        public void Cancel()
        {
            //新增事件处理
            if (blIsNew)
            {
                colbr_id.FilterInfo = cfiId;
                colbr_scripe.FilterInfo = cfiScript;
                this.blIsNew = false;
                this.gridControl1.Enabled = true;
            }
        }

        public void Close()
        {

        }

        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        #endregion
    }
}
