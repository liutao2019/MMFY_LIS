using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;

using lis.client.control;
using DevExpress.XtraGrid.Columns;

namespace dcl.client.dicmic
{
    public partial class ConDic_Antibio_Type : ConDicCommon, IBarActionExt
    {
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        // 是否为新增事件
        private bool blIsNew = false;
        public bool isActionSuccess = true;
        //全局变量 抗生素大类字典
        List<EntityDicAntibioType> antibiotypeList = new List<EntityDicAntibioType>();

        public ConDic_Antibio_Type()
        {
            InitializeComponent();
            this.Load += ConDic_Antibio_Type_Load;
            this.Name = "ConDic_Antibio_Type";
        }
        #region IBarActionExt
        public void Add()
        {
            textEdit_tppy.Properties.ReadOnly = true;
            textEdit_tpwb.Properties.ReadOnly = true;
            blIsNew = true;
            foreach (GridColumn column in gridViewAntibiotype.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }
            this.textEdit_tpid.Focus();
            EntityDicAntibioType AntibioType = (EntityDicAntibioType)bsAntibioType.AddNew();
            //int id = IdentityHelper.GetMedIdentity("dic_mic_qa_bac");
            //textEdit_qabId.Text = id.ToString();
            textEdit_tpid.Enabled = false;
            AntibioType.TpID = string.Empty;
            AntibioType.DelFlag = Convert.ToInt32(LIS_Const.del_flag.OPEN);
            bsAntibioType.EndEdit();
            bsAntibioType.ResetCurrentItem();
            this.gridAntibioType.Enabled = false;
        }

        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridAntibioType.Enabled = true;
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            this.bsAntibioType.EndEdit();
            if (bsAntibioType == null)
            { return; }
            EntityRequest request = new EntityRequest();
            EntityDicAntibioType AntibioType = (EntityDicAntibioType)bsAntibioType.Current;
            string id = AntibioType.TpID;
            request.SetRequestValue(AntibioType);
            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dresult == DialogResult.OK)
            {
                base.Delete(request); //删除数据

                if (this.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    //DoRefresh(); //不刷新数据(Remove就好，目的:为了不跳到第一行,方便查看)
                    AntibioType.DelFlag = 1;
                    bsAntibioType.ResetCurrentItem();
                    gridAntibioType.RefreshDataSource();
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
        }

        public void DoRefresh()
        {
            EntityResponse response = base.Search(new EntityRequest());
            antibiotypeList = response.GetResult() as List<EntityDicAntibioType>;
            this.bsAntibioType.DataSource = antibiotypeList;
            Filter();
        }
        /// <summary>
        /// 数据过滤
        /// </summary>
        private void Filter()
        {
            List<EntityDicAntibioType> list = new List<EntityDicAntibioType>();
            list = antibiotypeList;
            string filter = txtFilter.Text.Trim();
            //选择项目停用、启用过滤
            string strDelFilter = string.Empty;
            if (radioGroup_Item.EditValue.ToString() == "0")
            {
                strDelFilter = "";

            }
            if (radioGroup_Item.EditValue.ToString() == "1")
            {
                strDelFilter = LIS_Const.del_flag.OPEN;

            }
            else if (radioGroup_Item.EditValue.ToString() == "2")
            {
                strDelFilter = LIS_Const.del_flag.DEL;
            }
            if (!string.IsNullOrEmpty(strDelFilter))
            {
                list = list.Where(a => a.DelFlag.ToString() == strDelFilter).ToList();
            }
            if (string.IsNullOrEmpty(filter))
            {
                this.bsAntibioType.DataSource = list;
            }
            else
            {
                bsAntibioType.DataSource = list.Where(a => a.TpID.Contains(filter)
                                                         || a.TpCName.Contains(filter)
                                                         || a.TpCode.Contains(filter)
                                                         || a.TpPY.Contains(filter.ToUpper())
                                                         || a.TpWB.Contains(filter.ToUpper())

                                                         || a.DelFlag.ToString() == filter).ToList();
            }
        }

        public void Edit()
        {
            //GridMicQaBac.Enabled = true;
            groupBox1.Enabled = true;
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add("gridAntibioType", true);
            return dlist;
        }

        public void MoveNext()
        {
            throw new NotImplementedException();
        }

        public void MovePrev()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {

            if (this.textEdit_tpcname.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请输入中文名称", "提示信息");
                this.ActiveControl = this.textEdit_tpcname;
                isActionSuccess = false;
                this.textEdit_tpcname.Focus();
                return;
            }
            this.bsAntibioType.EndEdit();
            if (bsAntibioType.Current == null)
            {
                return;
            }
            EntityRequest request = new EntityRequest();

            EntityDicAntibioType AntibioType = (EntityDicAntibioType)bsAntibioType.Current;
            String tp_id = AntibioType.TpID;

            request.SetRequestValue(AntibioType);

            EntityResponse result = new EntityResponse();
            if (tp_id == "")
            {
                result = base.New(request); ;//新增保存
            }
            else
            {
                result = base.Update(request); //修改更新
            }

            if (base.isActionSuccess)
            {
                if (tp_id == "")
                {
                    AntibioType.TpID = result.GetResult<EntityDicAntibioType>().TpID;
                }

                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridAntibioType.Enabled = true;
            }

            bsAntibioType.ResetCurrentItem();

        }

        #endregion

        private void ConDic_Antibio_Type_Load(object sender, EventArgs e)
        {
            InitData();
            SetGridControl();
        }
        private void SetGridControl()
        {
            //设置控件不可编辑
            for (int i = 0; i < this.gridViewAntibiotype.Columns.Count; i++)
            {
                this.gridViewAntibiotype.Columns[i].OptionsColumn.AllowEdit = false;
            }
            gridAntibioType.Enabled = false;
            // layoutControlGroup2.Enabled = false;
        }

        private void InitData()
        {
            this.DoRefresh();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void radioGroup_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void textEdit_tpcname_Leave(object sender, EventArgs e)
        {
            if (bsAntibioType != null)
            {
                EntityDicAntibioType AntibioType = bsAntibioType.Current as EntityDicAntibioType;
                // AntibioType.TpCode = tookit.GetSpellCode(this.textEdit_tpcname.Text);
                AntibioType.TpPY = tookit.GetSpellCode(this.textEdit_tpcname.Text);
                AntibioType.TpWB = tookit.GetWBCode(this.textEdit_tpcname.Text);
            }
        }
    }
}
