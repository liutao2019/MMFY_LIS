using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using wf.client.reagent.ReaControl;
using System.Linq;
using dcl.client.frame;
using dcl.entity;
using dcl.client.cache;
using lis.client.control;

namespace wf.client.reagent
{
    /// <summary>
    /// 添加组合的选择框
    /// </summary>
    public partial class FrmReagentManagerV3 : FrmCommon
    {
        List<EntityReaSetting> _dtCurrReaSetting = null;
        public List<EntityReaSetting> CurrentReaSetting
        {
            get
            {
                return _dtCurrReaSetting;
            }
            set
            {
                _dtCurrReaSetting = value;
                this.gridControlReagent.DataSource = _dtCurrReaSetting;
            }
        }


        HopePopSelectMode _selectmode = HopePopSelectMode.SingleClick;
        public HopePopSelectMode SelectMode
        {
            get
            {
                return _selectmode;
            }
            set
            {
                _selectmode = value;
            }
        }


        public FrmReagentManagerV3()
        {
            InitializeComponent();
        }

        public void SetFilterParam(ReagentFilterParam3 param)
        {
            Param = param;

            if (param != null)
            {
                //获取系统配置：组合过滤方式
                string configValue = UserInfo.GetSysConfigValue("ReagentFilterMode");

                if (configValue == "试剂组别")
                {
                    param.currentFilterMode = 0;
                }
                else
                {
                    param.currentFilterMode = 0;
                }

                if (param.currentFilterMode == 0)
                {
                    if (!string.IsNullOrEmpty(param.group_name))
                    {
                        this.Text = string.Format("试剂选择   当前组别：{0}", param.group_name);
                    }
                    else
                    {
                        this.Text = string.Format("试剂选择   当前组别：无");
                    }
                }
            }
            ResetFilter();
        }

        public FrmReagentManagerV3(ReagentFilterParam3 param)
        {
            InitializeComponent();

            SetFilterParam(param);

        }

        ReagentFilterParam3 Param = null;


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmReagentSelect_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                try
                {
                    string config_selectmode = UserInfo.GetSysConfigValue("HopePopSelectMode");

                    if (config_selectmode == "单击")
                    {
                        this._selectmode = HopePopSelectMode.SingleClick;
                    }
                    else if (config_selectmode == "双击")
                    {
                        this._selectmode = HopePopSelectMode.DoubleClick;
                    }
                    else
                    {
                        this._selectmode = HopePopSelectMode.SingleClick;
                    }
                }
                catch (Exception)
                {

                }
                listReaSetting = CacheClient.GetCache<EntityReaSetting>();
                if (listReaSetting != null)//按组合序号排序
                {
                    listReaSetting.OrderBy(i => i.Drea_id);
                }
                ResetFilter();
                this.ActiveControl = this.textBox1;
                this.textBox1.Focus();
            }
        }

        List<EntityReaSetting> listReaSetting = new List<EntityReaSetting>();
        /// <summary>
        /// 重置过滤条件
        /// </summary>
        private void ResetFilter()
        {
            if (this.Param != null)
            {
                List<EntityReaSetting> filterList = new List<EntityReaSetting>();
                if (this.Param.currentFilterMode == 0)//物理、专物组过滤方式
                {

                    if (!string.IsNullOrEmpty(Param.group_id))
                    {
                        filterList = listReaSetting.FindAll(i => i.Drea_group == Param.group_id);
                    }
                    else if (!string.IsNullOrEmpty(Param.sup_id))
                    {
                        filterList = listReaSetting.FindAll(i => i.Drea_supplier == Param.sup_id);
                    }
                    else
                    {
                        filterList = listReaSetting;
                    }

                    this.bsReagent.DataSource = filterList;
                }
            }
            else
            {
                this.bsReagent.DataSource = listReaSetting;
            }
        }

        /// <summary>
        /// 双击组合选择列表其中一组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlReagentList_DoubleClick(object sender, EventArgs e)
        {
            if (this._selectmode == HopePopSelectMode.DoubleClick)
            {
                EntityReaSetting dr = this.gridViewReagentList.GetFocusedRow() as EntityReaSetting;
                if (dr != null)
                {
                    int combIndex = CurrentReaSetting.FindIndex(i => i.Drea_id == dr.Drea_id);
                    if (combIndex > -1)
                    {
                        CurrentReaSetting.Remove(dr);
                        OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Remove);

                    }
                    else
                    {
                        CurrentReaSetting.Add(dr);
                        OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Add);

                    }
                    this.gridControlReagent.RefreshDataSource();

                }
            }
        }

        /// <summary>
        /// 单击组合选择列表其中一组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewReagentList_MouseUp(object sender, MouseEventArgs e)
        {
            //注释单击添加组合的功能
            if (this._selectmode == HopePopSelectMode.SingleClick && e.Button == MouseButtons.Left)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = this.gridViewReagentList.CalcHitInfo(new Point(e.X, e.Y));

                if (hInfo.InRow || hInfo.InRowCell)
                {
                    EntityReaSetting dr = this.gridViewReagentList.GetFocusedRow() as EntityReaSetting;
                    if (dr != null)
                    {
                        int combIndex = CurrentReaSetting.FindIndex(i => i.Drea_id == dr.Drea_id);
                        if (combIndex > -1)
                        {
                            CurrentReaSetting.Remove(dr);
                            OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Remove);
                        }
                        else
                        {
                            //if (CurrentReaSetting != null && CurrentReaSetting.Count > 0)
                            //{
                            //    foreach (var item in CurrentReaSetting)
                            //    {
                            //        if (item.Drea_supplier != dr.Drea_supplier)
                            //        {
                            //            wf.auxiliary.control.MessageDialog.Show("供货商不同！", "提示");
                            //            return;
                            //        }
                            //    }
                            //}
                            
                            CurrentReaSetting.Add(dr);
                            OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Add);
                        }
                        this.gridControlReagent.RefreshDataSource();
                       
                    }
                }
            }
        }

        /// <summary>
        /// 双击当前病人组合列表中的组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlPatReagent_DoubleClick(object sender, EventArgs e)
        {
            EntityReaSetting dr = this.gridViewReagent.GetFocusedRow() as EntityReaSetting;
            if (dr != null)
            {
                string com_id = dr.Drea_id.ToString();
                int combIndex = CurrentReaSetting.FindIndex(i => i.Drea_id == dr.Drea_id);
                if (combIndex > -1)
                {
                    CurrentReaSetting.Remove(dr);
                }
                this.gridControlReagent.RefreshDataSource();
                OnReagentSelected(com_id, EnumAction.ReagentEditorAction.Remove);
            }
        }


        public delegate void ReagentSelectedEventHandler(object sender, string com_id, EnumAction.ReagentEditorAction action);
        public event ReagentSelectedEventHandler ReagentSelected;
        public void OnReagentSelected(string com_id, EnumAction.ReagentEditorAction action)
        {
            if (ReagentSelected != null)
            {

                ReagentSelected(this, com_id, action);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ResetFilter();
            string filter = this.textBox1.Text;
            List<EntityReaSetting> combList = bsReagent.DataSource as List<EntityReaSetting>;
            combList = combList.FindAll(i => i.Drea_id.Contains(filter) || i.Drea_name.Contains(filter) || i.py_code.Contains(filter.ToUpper()) || i.wb_code.Contains(filter.ToUpper()));
            bsReagent.DataSource = combList.OrderByDescending(i => i.Drea_id);
        }


        /// <summary>
        /// 搜索框按下回车焦点定位到组合选择列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 || e.KeyValue == 40)
            {
                this.gridViewReagentList.Focus();
                gridControlReagentList_KeyDown(sender, e);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmReagentSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)//Esc,关闭窗体
            {
                this.Close();
            }
            else if (e.KeyValue == 39)//向右
            {
                this.gridViewReagentList.Focus();
            }
            else if (e.KeyValue == 37)//向左
            {
                this.gridViewReagent.Focus();
            }
        }

        private void gridControlPatReagent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)//回车
            {
                EntityReaSetting dr = this.gridViewReagent.GetFocusedRow() as EntityReaSetting;
                if (dr != null)
                {
                    OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Remove);
                }
            }
        }

        /// <summary>s
        /// 在组合列表按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlReagentList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)//回车
            {
                EntityReaSetting dr = this.gridViewReagentList.GetFocusedRow() as EntityReaSetting;
                if (dr != null)
                {
                    OnReagentSelected(dr.Drea_id.ToString(), EnumAction.ReagentEditorAction.Add);
                    textBox1.Text = string.Empty;
                    textBox1.Focus();
                }
            }
        }


        private void gridControlReagentList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar >= 65 && (int)e.KeyChar <= 90) //a-z
                || ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 122)//A-Z
                || ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57)//0-9
                || ((int)e.KeyChar >= 96 && (int)e.KeyChar <= 105)//0-9
                )
            {
                this.textBox1.Focus();
                this.textBox1.Text += (char)e.KeyChar;
                this.textBox1.SelectionStart = this.textBox1.Text.Length;
            }
            else if ((int)e.KeyChar == 8)
            {
                if (this.textBox1.Text.Length > 1)
                {
                    this.textBox1.Focus();
                    this.textBox1.Text = this.textBox1.Text.Substring(0, this.textBox1.Text.Length - 2);
                    this.textBox1.SelectionStart = this.textBox1.Text.Length;
                }
            }
        }

        private void FrmReagentManagerV3_Leave(object sender, EventArgs e)
        {

        }

        private void FrmReagentManagerV3_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Close();
        }

        private void FrmReagentManagerV3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void FrmReagentManagerV3_VisibleChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;
        }

    }

    /// <summary>
    /// 组合过滤参数
    /// </summary>
    public class ReagentFilterParam3
    {
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string sup_id { get; set; }
        public string sup_name { get; set; }
        public int currentFilterMode;
    }
}
