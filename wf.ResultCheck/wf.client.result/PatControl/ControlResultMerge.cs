using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;

using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using lis.client.control;
using dcl.common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.result.CommonPatientInput;
using System.Diagnostics;
using dcl.root.logon;
using dcl.entity;
using System.Linq;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class ControlResultMerge : UserControl
    {
        public ControlResultMerge()
        {
            InitializeComponent();

            ////系统配置：[结果合并]可选择合并的项目
            //if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ResultMerge_IsSelItm") != "是")
            //{
            //    treeListColumn10.Visible = false;
            //}
        }

        private string _itr_id = string.Empty;
        public string ItrID
        {
            get
            {
                return _itr_id;
            }
            set
            {
                if (_itr_id != value)
                {
                    _itr_id = value;

                    if (string.IsNullOrEmpty(_itr_id))
                    {
                        this.label5.Text = "目标仪器：";
                    }
                    else
                    {
                        this.label5.Text = "目标仪器：" + DictInstrmt.Instance.GetItrMid(_itr_id);
                    }

                    this.bsPatients.DataSource = null;
                }
            }
        }

        private DateTime _pat_date = DateTime.Now;
        public DateTime PatDate
        {
            get
            {
                return _pat_date;
            }
            set
            {
                if (_pat_date.Date != value.Date)
                {
                    _pat_date = value;
                    //SearchLeftpatients();

                    this.label4.Text = string.Format("日    期：{0}", _pat_date.ToString("yyyy-MM-dd"));

                    this.bsPatients.DataSource = null;
                }
            }
        }



        /// <summary>
        /// 物理组改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatType_ValueChanged(object sender,control.ValueChangeEventArgs args)
        {
            treeRight.DataSource = null;
            this.txtPatInstructment.displayMember = null;
            this.txtPatInstructment.valueMember = null;

            //如果修改过物理组,则解除锁定当前选项
            if (cbKeepOpt.Checked)
            {
                string temp_typeName = txtPatType.displayMember;
                string temp_typeValue = txtPatType.valueMember;
                if (temp_typeName == null || temp_typeValue == null || temp_typeName != note_typeName || temp_typeValue != note_typeValue)
                {
                    cbKeepOpt.Checked = false;
                }
            }
            if (!string.IsNullOrEmpty(txtPatType.valueMember))
            {
                txtPatInstructment.SetFilter(txtPatInstructment.getDataSource().FindAll(w => w.ItrLabId == txtPatType.valueMember));
            }
            else
            {
                txtPatInstructment.SetFilter(txtPatInstructment.getDataSource());
            }
        }
     

        /// <summary>
        /// 仪器改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatInstructment_ValueChanged(object sender,control.ValueChangeEventArgs args)
        {
            //如果修改过仪器,则解除锁定当前选项
            if (cbKeepOpt.Checked)
            {
                string temp_interName = txtPatInstructment.displayMember;
                string temp_interValue = txtPatInstructment.valueMember;
                if (temp_interName == null || temp_interValue == null || temp_interName != note_interName || temp_interValue != note_interValue)
                {
                    cbKeepOpt.Checked = false;
                }
            }
        }

        private void txtPatDate_Leave(object sender, EventArgs e)
        {
            //SearchRightPatients();
        }


        //合并刷新
        private void btnSearch_Click(object sender, EventArgs e)
        {
            simpleButton1.Enabled = true;
            btnCopy.Enabled = false;
            if (!string.IsNullOrEmpty(this.txtPatInstructment.valueMember))
            {
                if (this.chkGetNonePatInfo.Checked)
                {
                    //只获取无病人资料信息 时 隐藏患者信息的列
                    this.treeListColumn6.Visible = false;//病人ID
                    this.treeListColumn7.Visible = false;//部门
                    this.treeListColumn8.Visible = false;//姓名
                }
                else
                {
                    this.treeListColumn6.Visible = true;//病人ID
                    this.treeListColumn7.Visible = true;//部门
                    this.treeListColumn8.Visible = true;//姓名
                }

                ProxyResultoMerge proxy = new ProxyResultoMerge();
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ItrId = this.txtPatInstructment.valueMember;
                resultQc.StartObrDate = Convert.ToDateTime(txtPatDate.EditValue).ToString("yyyy-MM-dd 00:00:00");
                resultQc.EndObrDate = Convert.ToDateTime(this.txtPatDate.EditValue).AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                resultQc.OnlyGetNonePatInfoResult = this.chkGetNonePatInfo.Checked;
                resultQc.IsCheck = false;
                List<EntityObrResult> listResult = (proxy.Service.GetNonePatInfoResult(resultQc)).OrderBy(w=>w.ObrSid.Length).ThenBy(w=>w.ObrSid).ToList();
                this.treeRight.DataSource = null;
                this.treeRight.Nodes.Clear();

                this.treeRight.BeginUpdate();

                Dictionary<string, TreeListNode> listNodeParent = new Dictionary<string, TreeListNode>();
                foreach (EntityObrResult result in listResult)
                {
                    string res_sid = result.ObrSid;
                    string res_itm_ecd = result.ItmEname;
                    TreeListNode nodeParent = null;
                    listNodeParent.TryGetValue(res_sid, out nodeParent);
                    if (nodeParent == null)
                    {
                        nodeParent = this.treeRight.AppendNode(res_sid, null);
                        nodeParent.SetValue("res_sid", res_sid);
                        nodeParent.SetValue("pat_date", this.PatDate);
                        nodeParent.SetValue("res_itr_id", result.ObrItrId);
                        nodeParent.SetValue("pat_name", result.PidName);
                        nodeParent.SetValue("pat_in_no", result.PidInNo);
                        nodeParent.SetValue("pat_dep_name", result.PidDeptName);
                        nodeParent.SetValue("res_id", result.ObrId);
                        nodeParent.SetValue("select_res",true);
                        listNodeParent.Add(res_sid, nodeParent);
                    }

                    TreeListNode nodeChild = this.treeRight.AppendNode(res_itm_ecd, nodeParent);
                    nodeChild.SetValue("res_sid", res_sid);
                    nodeChild.SetValue("res_itm_ecd", res_itm_ecd);
                    nodeChild.SetValue("res_chr", result.ObrValue);
                    nodeChild.SetValue("pat_date", this.PatDate);
                    nodeChild.SetValue("res_itr_id", result.ObrItrId);
                    nodeParent.SetValue("res_id", result.ObrId);
                    nodeChild.SetValue("pat_name", result.PidName);
                    nodeChild.SetValue("pat_in_no", result.PidInNo);
                    nodeChild.SetValue("res_itm_id", result.ItmId);
                    nodeChild.SetValue("select_res", true);
                    nodeChild.SetValue("pat_dep_name", result.PidDeptName);
                }
                //this.treeRight.ResumeLayout();
                //this.treeRight.DataSource = dt;

                this.treeRight.EndUpdate();
               
              //  this.treeRight.ExpandAll();
            }
            else
            {
                this.treeRight.DataSource = null;
            }
        }

        /// <summary>
        /// 点击左侧刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshLeft_Click(object sender, EventArgs e)
        {
            SearchLeftpatients();
        }

        /// <summary>
        /// 查询左侧数据
        /// </summary>
        private void SearchLeftpatients()
        {
            if (!string.IsNullOrEmpty(this.ItrID))
            {
                ProxyResultoMerge proxy = new ProxyResultoMerge();
                EntityPatientQC patientQc = new EntityPatientQC();
                patientQc.ListItrId.Add(this.ItrID);
                patientQc.DateStart = this.PatDate;
                patientQc.DateEnd = PatDate.AddDays(1);
                patientQc.RepStatus = "0";
                List<EntityPidReportMain> listPat = proxy.Service.GetCurrentItrPatientList(patientQc);
                this.bsPatients.DataSource = listPat;
            }
            else
            {
                this.bsPatients.DataSource = null;
            }
        }

        private void treeRight_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hInfo = this.treeRight.CalcHitInfo(new Point(e.X, e.Y));
            if (hInfo != null && hInfo.Node != null)
            {
                if (this.bsPatients.DataSource != null)
                {
                    string res_sid = hInfo.Node.GetValue("res_sid").ToString();

                    EntityPidReportMain entityLeft = (EntityPidReportMain)this.bsPatients.Current;

                    if (entityLeft != null)
                    {
                        List<EntityPidReportMain> listLeft = this.bsPatients.DataSource as List<EntityPidReportMain>;

                        if (entityLeft.DestRepSid != res_sid)
                        {
                            foreach (EntityPidReportMain patient in listLeft)
                            {
                                if (!Compare.IsEmpty(patient.DestRepSid))
                                {
                                    if (patient.DestRepSid == res_sid)
                                    {
                                        string src_sid = patient.RepSid;

                                        MessageDialog.Show(string.Format("目标样本号[{0}]已存在于左侧样本号[{1}]中", res_sid, src_sid));
                                        return;
                                    }
                                }
                            }
                        }

                        DateTime pat_date = Convert.ToDateTime(hInfo.Node.GetValue("pat_date"));
                        string res_itr_id = hInfo.Node.GetValue("res_itr_id").ToString();
                        string res_id = string.Empty;
                        if(hInfo.Node.GetValue("res_id")!=null)
                         res_id = hInfo.Node.GetValue("res_id").ToString();

                        entityLeft.DestRepSid = res_sid;
                        entityLeft.DestRepInDate = Convert.ToDateTime(txtPatDate.EditValue); //pat_date;
                        entityLeft.DestRepItrId = res_itr_id;
                        entityLeft.DestRepId = res_id;

                        this.gridView1.MoveNext();
                    }
                }
            }
        }

        //合并
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.bsPatients.DataSource != null)
            {
                List<EntityPidReportMain> listPat = this.bsPatients.DataSource as List<EntityPidReportMain>;
                List<EntityPidReportMain> listData = new List<EntityPidReportMain>();
                foreach (EntityPidReportMain patient in listPat)
                {
                    if (!Compare.IsEmpty(patient.DestRepSid) && !Compare.IsEmpty(patient.DestRepInDate))
                    {
                        listData.Add(patient);
                        //检查有什么选中的项目
                        if (treeListColumn10.Visible && treeRight.Nodes != null && treeRight.Nodes.Count > 0)
                        {
                            #region 遍历树,检查勾选了的项目id

                            for (int node_i = 0; node_i < treeRight.Nodes.Count; node_i++)
                            {
                                if (patient.DestRepSid == treeRight.Nodes[node_i].GetValue("res_sid").ToString())
                                {
                                    if (treeRight.Nodes[node_i].GetValue("select_res").ToString() == true.ToString())
                                    {
                                        patient.DestAllItem=true;//全部项目都合并
                                        break;
                                    }
                                    else if (treeRight.Nodes[node_i].Nodes != null && treeRight.Nodes[node_i].Nodes.Count>0)//遍历子节点
                                    {
                                        patient.DestAllItem = false;//不全部项目都合并
                                        #region 遍历子节点

                                        for (int node_j = 0; node_j < treeRight.Nodes[node_i].Nodes.Count; node_j++)
                                        {
                                            if (treeRight.Nodes[node_i].Nodes[node_j].GetValue("select_res").ToString() == true.ToString())
                                            {
                                               // dr["dest_itm_ids"] = dr["dest_itm_ids"].ToString() + "," + treeRight.Nodes[node_i].Nodes[node_j].GetValue("res_itm_id").ToString();//赋值项目id
                                                patient.DestItmIds.Add(treeRight.Nodes[node_i].Nodes[node_j].GetValue("res_itm_id").ToString());
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            patient.DestAllItem = true;//默认全部项目都合并
                        }
                    }
                }

                if (listData.Count > 0)
                {
                    if (MessageDialog.Show(string.Format("您将要合并{0}条记录，是否继续？", listData.Count), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ProxyResultoMerge proxy = new ProxyResultoMerge();
                        proxy.Service.Merge(listData,false);

                        this.bsPatients.DataSource = null;
                        this.treeRight.DataSource = null;
                        this.treeRight.Nodes.Clear();
                        //this.SearchLeftpatients();
                        //this.btnSearch_Click(null, EventArgs.Empty);

                        MessageDialog.Show("合并完成！", "提示");
                    }

                }
                else
                {
                    MessageDialog.Show("没有可以合并的数据", "提示");
                }
            }
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = this.gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if (hInfo.InRow || hInfo.InRowCell)
            {
                EntityPidReportMain patient = (EntityPidReportMain)this.bsPatients.Current;
                if (patient != null)
                {
                    if (string.IsNullOrEmpty(patient.DestRepId))
                    {
                        this.gridView1.Columns[3].OptionsColumn.AllowEdit = true;// 判断如果为空的则可以进行编缉

                    }
                    patient.DestRepSid = null;
                }
            }

        }


        /// <summary>
        /// 输入目标样本ID，双击回车后对数据的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.bsPatients.DataSource != null)
                {
                    EntityPidReportMain entityLeft =(EntityPidReportMain)bsPatients.Current;
                    string res_sid = entityLeft.DestRepSid;
                    if (entityLeft != null)
                    {
                        List<EntityPidReportMain> listLeft = this.bsPatients.DataSource as List<EntityPidReportMain>;
                        foreach (EntityPidReportMain patient in listLeft)
                        {
                            if (entityLeft.RepId != patient.RepId)
                            {
                                if (!Compare.IsEmpty(patient.DestRepSid))
                                {
                                    if (patient.DestRepSid == res_sid)
                                    {
                                        string src_sid = patient.RepSid;

                                        MessageDialog.Show(string.Format("目标样本号[{0}]已存在于左侧样本号[{1}]中", res_sid, src_sid));
                                        entityLeft.DestRepSid= "";
                                        return;
                                    }
                                }
                            }
                        }

                        //DataRow dr = this.gridView1.GetFocusedDataRow();
                        foreach (TreeListNode tn in this.treeRight.Nodes)
                        {
                            if (tn["res_sid"].ToString() == entityLeft.DestRepSid)
                            {
                                DateTime pat_date = Convert.ToDateTime(tn["pat_date"]);
                                string res_itr_id = tn["res_itr_id"].ToString();

                                entityLeft.DestRepSid = res_sid;
                                entityLeft.DestRepInDate = Convert.ToDateTime(txtPatDate.EditValue);//pat_date;
                                entityLeft.DestRepItrId= res_itr_id;
                                break;
                            }
                        }
                        this.gridView1.MoveNext();
                    }
                }

            }
        }


        /// <summary>
        /// 焦点离开控件表后对单元格设置为不可编缉状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Leave(object sender, EventArgs e)
        {
            this.gridView1.Columns[3].OptionsColumn.AllowEdit = false;
        }

        public void LoadData(string patDate, string typeName, string typeValue, string interName, string interValue)
        {
            if (cbKeepOpt.Checked)
            {
                txtPatDate.Text = note_patDate;
                txtPatType.displayMember = note_typeName;
                txtPatType.valueMember = note_typeValue;
                txtPatInstructment.displayMember = note_interName;
                txtPatInstructment.valueMember = note_interValue;
            }
            else
            {
                txtPatDate.Text = patDate;
                txtPatType.displayMember = typeName;
                txtPatType.valueMember = typeValue;
                txtPatInstructment.displayMember = interName;
                txtPatInstructment.valueMember = interValue;
            }
        }

        //锁定当前选项
        private string note_patDate { get; set; }
        private string note_typeName { get; set; }
        private string note_typeValue { get; set; }
        private string note_interName { get; set; }
        private string note_interValue { get; set; }

        private void cbKeepOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbKeepOpt.Checked)
            {
                #region MyRegion
                if (string.IsNullOrEmpty(txtPatDate.Text))
                {
                    MessageBox.Show("日期不能为空");
                    cbKeepOpt.Checked = false;
                    return;
                }

                note_patDate = txtPatDate.Text;


                if (string.IsNullOrEmpty(txtPatType.displayMember) || string.IsNullOrEmpty(txtPatType.valueMember))
                {
                    MessageBox.Show("物理组别不能为空");
                    cbKeepOpt.Checked = false;
                    return;
                }

                note_typeName = txtPatType.displayMember;
                note_typeValue = txtPatType.valueMember;

                if (string.IsNullOrEmpty(txtPatInstructment.displayMember) || string.IsNullOrEmpty(txtPatInstructment.valueMember))
                {
                    MessageBox.Show("源仪器不能为空");
                    cbKeepOpt.Checked = false;
                    return;
                }

                note_interName = txtPatInstructment.displayMember;
                note_interValue = txtPatInstructment.valueMember;
                #endregion
            }
        }

        private void txtPatDate_EditValueChanged(object sender, EventArgs e)
        {
            //如果修改过日期,则解除锁定当前选项
            if (cbKeepOpt.Checked)
            {
                string temp_patDate = txtPatDate.Text;
                if (temp_patDate == null || temp_patDate != note_patDate)
                {
                    cbKeepOpt.Checked = false;
                }
            }
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null &&sender is DevExpress.XtraEditors.CheckEdit)
            {
                DevExpress.XtraEditors.CheckEdit temp_checkedit = sender as DevExpress.XtraEditors.CheckEdit;

                if (this.treeRight.FocusedNode != null)
                {
                    TreeListNode temp_tr_node = this.treeRight.FocusedNode;
                    //是否主节点,如果是则，更新子节点状态
                    if (temp_tr_node.Nodes != null && temp_tr_node.Nodes.Count > 0)
                    {
                        bool allcheckTrue = true;//默认全选
                        if (temp_checkedit.EditValue.ToString() == true.ToString())
                        {
                            allcheckTrue = true;
                        }
                        else
                        {
                            allcheckTrue = false;
                        }

                        for (int i = 0; i < temp_tr_node.Nodes.Count; i++)
                        {
                            temp_tr_node.Nodes[i].SetValue("select_res", allcheckTrue);
                        }

                        temp_tr_node.SetValue("select_res", allcheckTrue);
                    }
                    else if (temp_tr_node.ParentNode != null && temp_tr_node.ParentNode.Nodes != null && temp_tr_node.ParentNode.Nodes.Count > 0)
                    {
                        bool allcheckTrue = true;//默认全选
                        //遍历子节点
                        for (int i = 0; i < temp_tr_node.ParentNode.Nodes.Count; i++)
                        {
                            if (temp_tr_node.ParentNode.Nodes[i].GetValue("select_res").ToString() != true.ToString()
                                && temp_tr_node.ParentNode.Nodes[i] != temp_tr_node)
                            {
                                allcheckTrue = false;//有一条子节点没有勾选,则不全选
                                break;
                            }
                            else if (temp_tr_node.ParentNode.Nodes[i] == temp_tr_node
                                && temp_checkedit.EditValue.ToString() != true.ToString())
                            {
                                allcheckTrue = false;//有一条子节点没有勾选,则不全选
                                break;
                            }
                        }

                        temp_tr_node.ParentNode.SetValue("select_res", allcheckTrue);

                        if (temp_checkedit.EditValue.ToString() == true.ToString())
                        {
                            temp_tr_node.SetValue("select_res", true);
                        }
                        else
                        {
                            temp_tr_node.SetValue("select_res", false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 复制刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCopy_Click(object sender, EventArgs e)
        {
            simpleButton1.Enabled = false;
            btnCopy.Enabled = true;
            if (!string.IsNullOrEmpty(this.txtPatInstructment.valueMember))
            {
                if (this.chkGetNonePatInfo.Checked)
                {
                    //只获取无病人资料信息 时 隐藏患者信息的列
                    this.treeListColumn6.Visible = false;//病人ID
                    this.treeListColumn7.Visible = false;//部门
                    this.treeListColumn8.Visible = false;//姓名
                }
                else
                {
                    this.treeListColumn6.Visible = true;//病人ID
                    this.treeListColumn7.Visible = true;//部门
                    this.treeListColumn8.Visible = true;//姓名
                }

                ProxyResultoMerge proxy = new ProxyResultoMerge();
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ItrId = this.txtPatInstructment.valueMember;
                resultQc.StartObrDate = Convert.ToDateTime(txtPatDate.EditValue).ToString("yyyy-MM-dd 00:00:00");
                resultQc.EndObrDate = Convert.ToDateTime(this.txtPatDate.EditValue).AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                resultQc.OnlyGetNonePatInfoResult = this.chkGetNonePatInfo.Checked;
                resultQc.IsCopy = true;
                List<EntityObrResult> listResult = (proxy.Service.GetNonePatInfoResult(resultQc)).OrderBy(w => w.ObrSid.Length).ThenBy(w => w.ObrSid).ToList();
                this.treeRight.DataSource = null;
                this.treeRight.Nodes.Clear();

                this.treeRight.BeginUpdate();

                Dictionary<string, TreeListNode> listNodeParent = new Dictionary<string, TreeListNode>();

                foreach (EntityObrResult result in listResult)
                {
                    string res_sid = result.ObrSid;
                    string res_itm_ecd = result.ItmEname;

                    TreeListNode nodeParent = null;
                    listNodeParent.TryGetValue(res_sid, out nodeParent);
                    if (nodeParent == null)
                    {
                        nodeParent = this.treeRight.AppendNode(res_sid, null);
                        nodeParent.SetValue("res_sid", res_sid);
                        nodeParent.SetValue("pat_date", this.PatDate);
                        nodeParent.SetValue("res_itr_id", result.ObrItrId);
                        nodeParent.SetValue("pat_name", result.PidName);
                        nodeParent.SetValue("pat_in_no", result.PidInNo);
                        nodeParent.SetValue("pat_dep_name", result.PidDeptName);
                        nodeParent.SetValue("res_id", result.ObrId);
                        nodeParent.SetValue("select_res", true);
                        listNodeParent.Add(res_sid, nodeParent);
                    }

                    TreeListNode nodeChild = this.treeRight.AppendNode(res_itm_ecd, nodeParent);
                    nodeChild.SetValue("res_sid", res_sid);
                    nodeChild.SetValue("res_itm_ecd", res_itm_ecd);
                    nodeChild.SetValue("res_chr", result.ObrValue);
                    nodeChild.SetValue("pat_date", this.PatDate);
                    nodeChild.SetValue("res_itr_id", result.ObrItrId);
                    nodeParent.SetValue("res_id", result.ObrId);
                    nodeChild.SetValue("pat_name", result.PidName);
                    nodeChild.SetValue("pat_in_no", result.PidInNo);
                    nodeChild.SetValue("res_itm_id", result.ItmId);
                    nodeChild.SetValue("select_res", true);
                    nodeChild.SetValue("pat_dep_name", result.PidDeptName);  
                }  
                this.treeRight.EndUpdate();
            }
            else
            {
                this.treeRight.DataSource = null;
            }
        }

        //复制
        private void btnCopy_Click(object sender, EventArgs e)
        {
            
            if (this.bsPatients.DataSource != null)
            {
                List<EntityPidReportMain> listPat = this.bsPatients.DataSource as List<EntityPidReportMain>;

                List<EntityPidReportMain> listData = new List<EntityPidReportMain>();
                foreach (EntityPidReportMain pat in listPat)
                {
                    if (!Compare.IsEmpty(pat.DestRepSid) && !Compare.IsEmpty(pat.DestRepInDate))
                    {
                        listData.Add(pat);

                        //检查有什么选中的项目
                        if (treeListColumn10.Visible && treeRight.Nodes != null && treeRight.Nodes.Count > 0)
                        {
                            #region 遍历树,检查勾选了的项目id

                            for (int node_i = 0; node_i < treeRight.Nodes.Count; node_i++)
                            {
                                if (pat.DestRepSid== treeRight.Nodes[node_i].GetValue("res_sid").ToString())
                                {
                                    if (treeRight.Nodes[node_i].GetValue("select_res").ToString() == true.ToString())
                                    {
                                        pat.DestAllItem= true;//全部项目都合并
                                        break;
                                    }
                                    else if (treeRight.Nodes[node_i].Nodes != null && treeRight.Nodes[node_i].Nodes.Count > 0)//遍历子节点
                                    {
                                        pat.DestAllItem = false;//不全部项目都合并
                                        #region 遍历子节点

                                        for (int node_j = 0; node_j < treeRight.Nodes[node_i].Nodes.Count; node_j++)
                                        {
                                            if (treeRight.Nodes[node_i].Nodes[node_j].GetValue("select_res").ToString() == true.ToString())
                                            {
                                                //dr["dest_itm_ids"] = dr["dest_itm_ids"].ToString() + "," + treeRight.Nodes[node_i].Nodes[node_j].GetValue("res_itm_id").ToString();//赋值项目id
                                                pat.DestItmIds.Add(treeRight.Nodes[node_i].Nodes[node_j].GetValue("res_itm_id").ToString());// 赋值项目id
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            pat.DestAllItem= true;//默认全部项目都合并
                        }
                    }
                }

                if (listData.Count > 0)
                {
                    if (MessageDialog.Show(string.Format("您将要复制{0}条记录，是否继续？", listData.Count), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ProxyResultoMerge proxy = new ProxyResultoMerge();
                       // dt.Columns.Add("IsCopy");
                       proxy.Service.Merge(listPat,true);

                        this.bsPatients.DataSource = null;
                        this.treeRight.DataSource = null;
                        this.treeRight.Nodes.Clear();
                        //this.SearchLeftpatients();
                        //this.btnSearch_Click(null, EventArgs.Empty);

                        MessageDialog.Show("复制完成！", "提示");
                    }

                }
                else
                {
                    MessageDialog.Show("没有可以复制的数据", "提示");
                }
            }
        }
    }
}
