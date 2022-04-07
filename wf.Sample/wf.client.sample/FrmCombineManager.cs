using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;
using dcl.client.cache;

namespace dcl.client.sample
{
    /// <summary>
    /// 添加组合的选择框
    /// </summary>
    public partial class FrmCombineManager : FrmCommonExt
    {
        public bool ShowUrgentSelect
        {
            get
            {
                return this.gridColumn7.Visible;
            }
            set
            {
                this.gridColumn7.Visible = value;
            }
        }

        string ctype;
        //string ptype;
        // PatResult CurrentPatResult;
        public List<EntityDicCombine> dtCombine
        {
            get
            {
                return _dtCurrCombine;
            }
            set
            {
                _dtCurrCombine = value;

            }
        }
        List<EntityDicCombine> _dtCurrCombine = null;

        private bool isFilter = false;
        private bool isYgManual = false;

        /// <summary>
        /// 院感条码是否过滤项目物理组ID
        /// </summary>
        public bool IsYgManual
        {
            get { return isYgManual; }
            set { isYgManual = value; }
        }

        public bool IsFilter
        {
            get { return isFilter; }
            set { isFilter = value; }
        }

        List<EntityUserRole> listUserRole;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptype">专业组的组别ID</param>
        public FrmCombineManager(List<EntityDicCombine> combine, /*PatResult ctrlPatResult,*/ string com_ctype, string ctype_name/*, string com_ptype, string ptype_name*/)
        {
            InitializeComponent();
            this.Load += FrmCombineSelect_Load;
            this.dtCombine = combine;
            ctype = com_ctype;

            //设置标题
            string ctype_text = ctype_name;
            if (ctype_text == null || ctype_text == string.Empty)
            {
                ctype_text = "无";
            }

            this.Text = string.Format("组合选择   当前物理组：{0} " /*  当前专业组：{1}"*/, ctype_text /*, ptype_text*/);
        }
        List<EntityDicCombine> listType = new List<EntityDicCombine>();
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCombineSelect_Load(object sender, EventArgs e)
        {
            ResetFilter();
            List<EntityDicCombine> listSource = CacheClient.GetCache<EntityDicCombine>();


            string strCTypeFilter = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CTypeFilter");
            string strYgCTypeFilter = ConfigHelper.GetSysConfigValueWithoutLogin("BCPrint_YGFilterItemByType");//过滤院感物理组


            //判断是否有过滤权限
            if (isFilter && strCTypeFilter != string.Empty)
            {
                listType = (from x in listSource where !strCTypeFilter.Contains(x.ComLabId) select x).ToList();
            }
            else
                listType = listSource;

            if (isYgManual && !string.IsNullOrEmpty(strYgCTypeFilter))
            {
                string[] strCtypeId = strYgCTypeFilter.Split(',');

                string strTypeFilter = string.Empty;
                foreach (string item in strCtypeId)
                {
                    strTypeFilter += String.Format(",'{0}'", item);
                }
                strTypeFilter = strTypeFilter.Remove(0, 1);

                listType = listType.FindAll(w => strTypeFilter.Contains(w.ComLabId)).ToList();
            }

            foreach (EntityDicCombine rowCombine in listType)
            {
                rowCombine.Urgent = false;
            }

            //获取用户角色对应表
            ProxyRoleManagePro roleProxy = new ProxyRoleManagePro();

            //暂时判断住院条码组角色时，限制两个免费项目才能开手工条码选择
            List<EntityUserRole> list = roleProxy.Service.GetRoleUserAndFunc("10021").listUser;
            listUserRole = list.FindAll(i => i.UserId == UserInfo.userInfoId);
            //获取系统过滤组合ID

            string strConfig = UserInfo.GetSysConfigValue("ZYManalInputCombine");

            //******************************************************************
            //此处加了若strConfig不为空才进行过滤
            if (!string.IsNullOrEmpty(strConfig))
            {
                //拼接ID字符串
                string[] strConfigArr = strConfig.Split(';');
                string strComId = string.Empty;
                foreach (string id in strConfigArr)
                {
                    strComId += "'" + id + "',";

                }
                strComId = strComId.TrimEnd(',');

                if (listUserRole.Count > 0)
                {
                    listType = (from x in listType where strComId.Contains(x.ComId) select x).ToList();
                }
            }
            //******************************************************************
            this.bsType.DataSource = listType;
            this.gridControlPatCombine.DataSource = dtCombine;

            textBox1.Text = strFilter;

            this.ActiveControl = this.textBox1;
            this.textBox1.Focus();


        }

        /// <summary>
        /// 重置过滤条件
        /// </summary>
        private void ResetFilter()
        {
            if (!string.IsNullOrEmpty(ctype))
            {
                List<EntityDicCombine> combList = bsType.DataSource as List<EntityDicCombine>;
                if (combList != null && combList.Count > 0)
                {
                    bsType.DataSource = combList.FindAll(i => i.ComLabId == ctype);
                }
            }
        }

        /// <summary>
        /// 双击组合选择列表其中一组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlCombineList_DoubleClick(object sender, EventArgs e)
        {
            EntityDicCombine dr = this.gridViewCombineList.GetFocusedRow() as EntityDicCombine;
            if (dr != null)
            {
                string com_id = dr.ComId.ToString();
                CombineSelected(com_id);
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
                CombineSelected(com_id);
            }
        }

        public delegate void ResreshCombineTextDemandedEventhandler(object sender, EventArgs args);
        public event ResreshCombineTextDemandedEventhandler RefreshCombineTextDemanded;

        public void OnResreshCombineTextDemanded()
        {
            if (RefreshCombineTextDemanded != null)
            {
                RefreshCombineTextDemanded(this, EventArgs.Empty);
            }
        }

        public event ResreshCombineTextDemandedEventhandler SaveBarCode;

        public void OnSaveBarCode()
        {
            if (SaveBarCode != null)
            {
                SaveBarCode(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 选择组合
        /// </summary>
        private void CombineSelected(string com_id)
        {
            if (ExistCombine(com_id))//当前组合已存在,移除
            {
                EntityDicCombine rows = Find(com_id);
                dtCombine.Remove(rows);
            }
            else//不存在,增加
            {
                EntityDicCombine row = gridViewCombineList.GetFocusedRow() as EntityDicCombine;

                dtCombine.Add(row);
            }
            this.gridControlPatCombine.DataSource = dtCombine;
            this.gridControlPatCombine.RefreshDataSource();
            OnResreshCombineTextDemanded();
        }
        /// <summary>
        /// 当前病人组合中是否存在指定的组合
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        private bool ExistCombine(string com_id)
        {
            EntityDicCombine rows = Find(com_id);
            return rows != null;
        }

        private EntityDicCombine Find(string com_id)
        {
            return dtCombine.Find(i => i.ComId == com_id);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                this.bsType.DataSource = listType;
                ResetFilter();
                return;
            }

            ResetFilter();

            string filterStr = this.textBox1.Text;
            //List<EntityDicCombine> listComb = bsType.DataSource as List<EntityDicCombine>;
            bsType.DataSource = listType.FindAll(i => i.ComId.Contains(filterStr)
                                                    || i.ComName.Contains(filterStr)
                                                    || i.ComPyCode.Contains(filterStr.ToUpper())
                                                    || i.ComWbCode.Contains(filterStr.ToUpper())
                                                    || i.ComHisCode.Contains(filterStr)
                                                    || i.ComCCode.Contains(filterStr));
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
                if (e.KeyCode == Keys.Enter)
                {
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        OnSaveBarCode();
                    }
                    else
                    {
                        gridControlCombineList_KeyDown(sender, e);
                        textBox1.Text = string.Empty;
                    }
                }
                else
                    gridControlCombineList.Focus();
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
                    CombineSelected(dr.ComId.ToString());
                }
            }
        }

        /// <summary>
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
                    CombineSelected(dr.ComId.ToString());
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



        private string strFilter;

        public string StrFilter
        {
            get { return strFilter; }
            set
            {
                strFilter = value;
                textBox1.Text = strFilter;
                if (strFilter.Trim() != string.Empty)
                {
                    textBox1_TextChanged(null, null);
                    EntityDicCombine dr = this.gridViewCombineList.GetFocusedRow() as EntityDicCombine;
                    if (dr != null)
                        CombineSelected(dr.ComId.ToString());
                    textBox1.Text = string.Empty;
                }
                textBox1.Focus();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            base.OnClosing(e);
        }

        private void FrmCombineManager_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
