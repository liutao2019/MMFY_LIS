using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraEditors;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.PatControl;
using dcl.client.result.Interface;
using dcl.client.frame;
using lis.client.control;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    /// <summary>
    /// 添加组合的选择框
    /// </summary>
    public partial class FrmCombineManagerV2 : FrmCommon
    {
        //string ctype;
        //string ptype;
        //string strStr_id;
        List<EntityDicCombine> _dtCurrCombine = null;
        public List<EntityDicCombine> CurrentCombine
        {
            get
            {
                return _dtCurrCombine;
            }
            set
            {
                _dtCurrCombine = value;
                this.gridControlPatCombine.DataSource = _dtCurrCombine;
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

        //int combine_select_type = 0;


        /// <summary>
        /// .ctor；组合选择方式：物理组、专业组
        /// </summary>
        /// <param name="com_ctype">物理组ID</param>
        /// <param name="ctype_name">物理组名称</param>
        /// <param name="com_ptype">专业组ID</param>
        /// <param name="ptype_name">专业组名称</param>
        //public FrmCombineManagerV2(string com_ctype, string ctype_name, string com_ptype, string ptype_name)
        //{
        //    InitializeComponent();

        //    ctype = com_ctype;
        //    ptype = com_ptype;

        //    if (!DesignMode)
        //    {

        //        //设置标题
        //        string ctype_text = ctype_name;
        //        if (ctype_text == null || ctype_text == string.Empty)
        //        {
        //            ctype_text = "无";
        //        }

        //        string ptype_text = ptype_name;
        //        if (ptype_text == null || ptype_text == string.Empty)
        //        {
        //            ptype_text = "无";
        //        }


        //        combine_select_type = 0;

        //        this.Text = string.Format("组合选择   当前物理组：{0}   当前专业组：{1}", ctype_text, ptype_text);
        //    }
        //}

        public FrmCombineManagerV2()
        {
            InitializeComponent();
        }

        public void SetFilterParam(CombineFilterParam param)
        {
            Param = param;

            if (param != null)
            {
                //获取系统配置：组合过滤方式
                string configValue = UserInfo.GetSysConfigValue("CombineFilterMode");

                if (configValue == "仪器")
                {
                    param.currentFilterMode = 0;
                }
                else if (configValue == "物理、专业组")
                {
                    param.currentFilterMode = 1;
                }
                else
                {
                    param.currentFilterMode = 0;
                }

                if (param.currentFilterMode == 0)
                {
                    if (!string.IsNullOrEmpty(param.itr_name))
                    {
                        this.Text = string.Format("组合选择   当前仪器：{0}", param.itr_name);
                    }
                    else
                    {
                        this.Text = string.Format("组合选择   当前仪器：无");
                    }
                }
                else
                {
                    //设置标题
                    string ctype_text = param.ctype_name;
                    if (ctype_text == null || ctype_text == string.Empty)
                    {
                        ctype_text = "无";
                    }

                    string ptype_text = param.ptype_name;
                    if (ptype_text == null || ptype_text == string.Empty)
                    {
                        ptype_text = "无";
                    }

                    this.Text = string.Format("组合选择   当前物理组：{0}   当前专业组：{1}", ctype_text, ptype_text);
                }
            }
            ResetFilter();
        }

        public FrmCombineManagerV2(CombineFilterParam param)
        {
            InitializeComponent();

            SetFilterParam(param);

        }

        CombineFilterParam Param = null;

        ///// <summary>
        ///// .ctor组合选择方式：仪器
        ///// </summary>
        ///// <param name="itr_id">仪器ID</param>
        ///// <param name="itr_name">仪器名称</param>
        //public FrmCombineManagerV2(string itr_id,string itr_name)
        //{
        //    strStr_id = itr_id;

        //    combine_select_type = 1;

        //    this.Text = string.Format("组合选择   当前仪器：{0}", itr_name);
        //}

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCombineSelect_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                try
                {
                    string config_selectmode = dcl.client.frame.UserInfo.GetSysConfigValue("HopePopSelectMode");

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
                listComb = CacheClient.GetCache<EntityDicCombine>();
                if (listComb != null)//按组合序号排序
                {
                    listComb.OrderBy(i => i.ComSortNo);
                }
                ResetFilter();



                //this.bsCombine.DataSource = listComb;

                this.ActiveControl = this.textBox1;
                this.textBox1.Focus();
            }
        }

        List<EntityDicCombine> listComb = new List<EntityDicCombine>();
        /// <summary>
        /// 重置过滤条件
        /// </summary>
        private void ResetFilter()
        {
            if (this.Param != null)
            {
                List<EntityDicCombine> filterList = new List<EntityDicCombine>();
                if (this.Param.currentFilterMode == 1)//物理、专物组过滤方式
                {

                    if (!string.IsNullOrEmpty(Param.com_ctype))
                    {
                        filterList = listComb.FindAll(i => i.ComLabId == Param.com_ctype);
                    }

                    if (!string.IsNullOrEmpty(Param.ptype_id))
                    {
                        filterList = listComb.FindAll(i => i.ComPriId == Param.ptype_id);
                    }

                    this.bsCombine.DataSource = filterList;
                }
                else//按仪器过滤方式
                {
                    if (string.IsNullOrEmpty(Param.itr_id))
                    {
                        //MessageDialog.ShowAutoCloseDialog("请选择仪器！");
                        //this.Close();
                        //return;
                        this.bsCombine.DataSource = new List<EntityDicCombine>();
                    }
                    else
                    {
                        List<EntityDicInstrument> dtDictInstrmt = CacheClient.GetCache<EntityDicInstrument>();

                        List<EntityDicItrCombine> dt_dict_instrmt_com = CacheClient.GetCache<EntityDicItrCombine>();

                        //查找所有"存储仪器"为当前仪器的仪器
                        List<EntityDicInstrument> drsConItr = dtDictInstrmt.FindAll(i => i.ItrReportItrId == Param.itr_id);

                        string sqlItrCombineIn = Param.itr_id;

                        if (drsConItr.Count > 0)
                        {
                            foreach (EntityDicInstrument drItr in drsConItr)
                            {
                                string itrid = drItr.ItrId.ToString();
                                sqlItrCombineIn += "," + itrid;
                            }
                        }

                        List<EntityDicItrCombine> drs = (from x in dt_dict_instrmt_com where sqlItrCombineIn.Contains(x.ItrId) select x).ToList();

                        string sqlPart = " 1=2 ";
                        if (drs.Count > 0)
                        {
                            bool bNeedComma = false;
                            sqlPart = string.Empty;
                            foreach (EntityDicItrCombine dr in drs)
                            {
                                if (dr.ComId.ToString() != string.Empty)
                                {
                                    if (bNeedComma)
                                    {
                                        sqlPart += "," + dr.ComId.ToString();
                                    }
                                    else
                                    {
                                        sqlPart += dr.ComId.ToString();
                                    }

                                    bNeedComma = true;
                                }
                            }
                        }
                        filterList = (from x in listComb where sqlPart.Contains(x.ComId) select x).ToList();
                        this.bsCombine.DataSource = filterList;
                    }
                }
            }
            else
            {
                this.bsCombine.DataSource = new List<EntityDicCombine>();
            }
        }

        /// <summary>
        /// 双击组合选择列表其中一组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlCombineList_DoubleClick(object sender, EventArgs e)
        {
            if (this._selectmode == HopePopSelectMode.DoubleClick)
            {
                EntityDicCombine dr = this.gridViewCombineList.GetFocusedRow() as EntityDicCombine;
                if (dr != null)
                {
                    int combIndex = CurrentCombine.FindIndex(i => i.ComId == dr.ComId);
                    if (combIndex > -1)
                    {
                        CurrentCombine.Remove(dr);
                    }
                    else
                    {
                        CurrentCombine.Add(dr);
                    }
                    this.gridControlPatCombine.RefreshDataSource();
                    OnCombineSelected(dr.ComId.ToString(), CombineEditor.CombineEditorAction.Add);

                }
            }
        }

        /// <summary>
        /// 单击组合选择列表其中一组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCombineList_MouseUp(object sender, MouseEventArgs e)
        {
            //注释单击添加组合的功能
            if (this._selectmode == HopePopSelectMode.SingleClick && e.Button == MouseButtons.Left)
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = this.gridViewCombineList.CalcHitInfo(new Point(e.X, e.Y));

                if (hInfo.InRow || hInfo.InRowCell)
                {
                    EntityDicCombine dr = this.gridViewCombineList.GetFocusedRow() as EntityDicCombine;
                    if (dr != null)
                    {
                        int combIndex = CurrentCombine.FindIndex(i => i.ComId == dr.ComId);
                        if (combIndex > -1)
                        {
                            CurrentCombine.Remove(dr);
                        }
                        else
                        {
                            CurrentCombine.Add(dr);
                        }
                        this.gridControlPatCombine.RefreshDataSource();
                        OnCombineSelected(dr.ComId.ToString(), CombineEditor.CombineEditorAction.Add);
                    }
                }
            }
        }

        /// <summary>
        /// 双击当前病人组合列表中的组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlPatCombine_DoubleClick(object sender, EventArgs e)
        {
            EntityDicCombine dr = this.gridViewCurrent.GetFocusedRow() as EntityDicCombine;
            if (dr != null)
            {
                string com_id = dr.ComId.ToString();
                int combIndex = CurrentCombine.FindIndex(i => i.ComId == dr.ComId);
                if (combIndex > -1)
                {
                    CurrentCombine.Remove(dr);
                }
                this.gridControlPatCombine.RefreshDataSource();
                OnCombineSelected(com_id, CombineEditor.CombineEditorAction.Remove);
            }
        }

        //public delegate void ResreshCombineTextDemandedEventhandler(object sender, EventArgs args);
        //public event ResreshCombineTextDemandedEventhandler RefreshCombineTextDemanded;
        //public void OnResreshCombineTextDemanded()
        //{
        //    if (RefreshCombineTextDemanded != null)
        //    {
        //        RefreshCombineTextDemanded(this, EventArgs.Empty);
        //    }
        //}

        public delegate void CombineSelectedEventHandler(object sender, string com_id, dcl.client.result.PatControl.CombineEditor.CombineEditorAction action);
        public event CombineSelectedEventHandler CombineSelected;
        public void OnCombineSelected(string com_id, dcl.client.result.PatControl.CombineEditor.CombineEditorAction action)
        {
            if (CombineSelected != null)
            {

                CombineSelected(this, com_id, action);
            }
        }

        ///// <summary>
        ///// 选择组合
        ///// </summary>
        //private void CombineSelected(string com_id)
        //{
        //    if (ExistCombine(com_id))//当前组合已存在,移除
        //    {
        //        this.IComEdit.RemoveCombine(com_id);
        //    }
        //    else//不存在,增加
        //    {
        //        this.IComEdit.AddCombine(com_id);
        //    }
        //    OnResreshCombineTextDemanded();
        //}
        ///// <summary>
        ///// 当前病人组合中是否存在指定的组合
        ///// </summary>
        ///// <param name="com_id"></param>
        ///// <returns></returns>
        //private bool ExistCombine(string com_id)
        //{
        //    if (this.IComEdit.PatientsMi != null)
        //    {
        //        if (this.IComEdit.PatientsMi.Select(string.Format("pat_com_id='{0}'", com_id)).Length > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ResetFilter();
            string filter = this.textBox1.Text;
            List<EntityDicCombine> combList = bsCombine.DataSource as List<EntityDicCombine>;
            combList = combList.FindAll(i => i.ComCCode.Contains(filter) || i.ComId.Contains(filter) || i.ComName.Contains(filter) || i.ComPyCode.Contains(filter.ToUpper()) || i.ComWbCode.Contains(filter.ToUpper()));
            bsCombine.DataSource = combList.OrderByDescending(i => i.ComCCode);
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
                this.gridViewCombineList.Focus();
                gridControlCombineList_KeyDown(sender, e);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCombineSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)//Esc,关闭窗体
            {
                this.Close();
            }
            else if (e.KeyValue == 39)//向右
            {
                this.gridViewCombineList.Focus();
            }
            else if (e.KeyValue == 37)//向左
            {
                this.gridViewCurrent.Focus();
            }
        }

        private void gridControlPatCombine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)//回车
            {
                EntityDicCombine dr = this.gridViewCurrent.GetFocusedRow() as EntityDicCombine;
                if (dr != null)
                {
                    OnCombineSelected(dr.ComId.ToString(), CombineEditor.CombineEditorAction.Remove);
                }
            }
        }

        /// <summary>s
        /// 在组合列表按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlCombineList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)//回车
            {
                EntityDicCombine dr = this.gridViewCombineList.GetFocusedRow() as EntityDicCombine;
                if (dr != null)
                {
                    OnCombineSelected(dr.ComId.ToString(), CombineEditor.CombineEditorAction.Add);
                    textBox1.Text = string.Empty;
                    textBox1.Focus();
                }
            }
        }


        private void gridControlCombineList_KeyPress(object sender, KeyPressEventArgs e)
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

        private void FrmCombineManagerV2_Leave(object sender, EventArgs e)
        {

        }

        private void FrmCombineManagerV2_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Close();
        }

        private void FrmCombineManagerV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void FrmCombineManagerV2_VisibleChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Empty;
        }


    }

    /// <summary>
    /// 组合过滤参数
    /// </summary>
    public class CombineFilterParam
    {
        public string itr_id { get; set; }
        public string itr_name { get; set; }

        public string com_ctype { get; set; }
        public string ctype_name { get; set; }

        public string ptype_id { get; set; }
        public string ptype_name { get; set; }

        public int currentFilterMode;
    }
}
