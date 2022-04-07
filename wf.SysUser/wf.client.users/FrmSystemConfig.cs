using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using System.Collections;
using DevExpress.XtraEditors.Repository;
using Lib.LogManager;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;

namespace dcl.client.users
{
    public partial class FrmSystemConfig : FrmCommon
    {

        GridEditorCollection gridEditors;
        List<EntitySysParameter> parms = new List<EntitySysParameter>();
        //ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmSystemConfig");
        ProxySystemConfig proxy = new ProxySystemConfig();

        public FrmSystemConfig()
        {
            InitializeComponent();
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSystemConfig_Load(object sender, EventArgs e)
        {
            //显示工具条
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSave.Name, sysToolBar1.BtnReset.Name, sysToolBar1.BtnClose.Name });

            LoadData(string.Empty);
        }

        /// <summary>
        /// 显示列表
        /// </summary>
        private void LoadData(string filter)
        {
            //读取数据
            List<EntitySysParameter> parmeters = new List<EntitySysParameter>();
            List<string> list = new List<string>();

            if (string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(comboBoxEdit1.Text))
            {
                parms = proxy.Service.GetSysParaListByConfigType();
                parmeters = parms;

                foreach (EntitySysParameter item in parms)
                {
                    if (item.ParmGroup != null)
                    {
                        list.Add(item.ParmGroup);
                    }
                }
                if (comboBoxEdit1.Properties.Items.Count == 0)
                    comboBoxEdit1.Properties.Items.AddRange(list.Distinct().ToList());
            }
            else
            {
                if (!string.IsNullOrEmpty(comboBoxEdit1.Text))
                {
                    parmeters = parms.Where(w => w.ParmGroup == comboBoxEdit1.Text).ToList();
                }
                if (!string.IsNullOrEmpty(filter))
                {
                    if (parmeters.Count > 0)
                    {
                        parmeters = parmeters.Where(w => w.ParmGroup.Contains(filter)
                        || w.ParmFieldName.Contains(filter)
                        || w.ParmCode.Contains(filter)).ToList();
                    }
                    else
                    {
                        parmeters = parms.Where(w => w.ParmGroup.Contains(filter)
                        || w.ParmFieldName.Contains(filter)
                        || w.ParmCode.Contains(filter)).ToList();
                    }
                }
            }

            gridEditors = new GridEditorCollection();



            foreach (EntitySysParameter item in parmeters)
            {
                string id = item.ParmId.ToString();
                string group = item.ParmGroup;
                string name = item.ParmFieldName;
                string value = item.ParmFieldValue;

                string type = item.ParmFieldType;

                switch (type)
                {
                    case "枚举":
                        {
                            RepositoryItemComboBox txtEdit = new RepositoryItemComboBox();
                            txtEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                            string[] dict = item.ParmDictList.ToString().Split(',');

                            for (int i = 0; i < dict.Length; i++)
                            {
                                txtEdit.Items.Add(dict[i]);
                            }
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    case "数字":
                        {
                            RepositoryItemSpinEdit txtEdit = new RepositoryItemSpinEdit();
                            txtEdit.IsFloatValue = false;
                            try
                            {
                                string[] dict = item.ParmDictList.ToString().Split(',');
                                txtEdit.MinValue = int.Parse(dict[0]);
                                txtEdit.MaxValue = int.Parse(dict[1]);
                            }
                            catch
                            { }
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    case "小数":
                        {
                            RepositoryItemSpinEdit txtEdit = new RepositoryItemSpinEdit();
                            txtEdit.IsFloatValue = true;
                            try
                            {
                                string[] dict = item.ParmDictList.ToString().Split(',');
                                txtEdit.MinValue = decimal.Parse(dict[0]);
                                txtEdit.MaxValue = decimal.Parse(dict[1]);
                            }
                            catch
                            { }
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    case "日期":
                        {
                            RepositoryItemDateEdit txtEdit = new RepositoryItemDateEdit();
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    case "时间":
                        {
                            RepositoryItemTimeEdit txtEdit = new RepositoryItemTimeEdit();
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    case "密码":
                        {
                            RepositoryItemTextEdit txtEdit = new RepositoryItemTextEdit();
                            txtEdit.PasswordChar = '*';
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                    default:
                        {
                            RepositoryItemTextEdit txtEdit = new RepositoryItemTextEdit();
                            this.gridEditors.Add(txtEdit, id, group, name, value);
                            break;
                        }
                }
            }

            gdSystemConfig.DataSource = gridEditors;
            gvSystemConfig.ExpandAllGroups();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvSystemConfig_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column == colConfigItemValue)
            {
                GridEditorItem item = gvSystemConfig.GetRow(e.RowHandle) as GridEditorItem;
                if (item != null) e.RepositoryItem = item.RepositoryItem;
            }
        }

        public class GridEditorItem
        {
            string fId;
            string fGroup;
            string fName;
            object fValue;
            DevExpress.XtraEditors.Repository.RepositoryItem fRepositoryItem;

            public GridEditorItem(DevExpress.XtraEditors.Repository.RepositoryItem fRepositoryItem, string fId, string fGroup, string fName, object fValue)
            {
                this.fId = fId;
                this.fRepositoryItem = fRepositoryItem;
                this.fGroup = fGroup;
                this.fName = fName;
                this.fValue = fValue;
            }

            public string Id { get { return this.fId; } }
            public string Group { get { return this.fGroup; } }
            public string Name { get { return this.fName; } }
            public object Value { get { return this.fValue; } set { this.fValue = value; } }
            public DevExpress.XtraEditors.Repository.RepositoryItem RepositoryItem { get { return this.fRepositoryItem; } }
        }

        class GridEditorCollection : ArrayList
        {
            public GridEditorCollection()
            {
            }
            public new GridEditorItem this[int index] { get { return base[index] as GridEditorItem; } }
            public void Add(DevExpress.XtraEditors.Repository.RepositoryItem fRepositoryItem, string fId, string fGroup, string fName, object fValue)
            {
                base.Add(new GridEditorItem(fRepositoryItem, fId, fGroup, fName, fValue));
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();

            List<EntitySysParameter> updateList = new List<EntitySysParameter>();

            foreach (GridEditorItem item in this.gridEditors)
            {
                EntitySysParameter parm = new EntitySysParameter();
                parm.ParmId = int.Parse(item.Id);
                parm.ParmFieldValue = item.Value == null ? null : item.Value.ToString();
                updateList.Add(parm);
            }

            //保存数据
            try
            {


                bool result = proxy.Service.UpdateSysPara(updateList);

                //this.isActionSuccess = true;
                if (result)
                {
                    sysToolBar1.LogMessage = "保存系统配置";

                    //写入缓存
                    UserInfo.SetSysConfig(proxy.Service.GetSysParaListByConfigType());
                    dcl.client.cache.CacheSysconfig.Current.Refresh();

                    lis.client.control.MessageDialog.Show("保存成功,部分配置可能需要程序重新启动才能生效", PowerMessage.BASE_TITLE);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException("保存出错", ex);
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;

            LoadData(string.Empty);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void comboBoxEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Back)
            {
                comboBoxEdit1.EditValue = "";
            }
        }
    }
}
