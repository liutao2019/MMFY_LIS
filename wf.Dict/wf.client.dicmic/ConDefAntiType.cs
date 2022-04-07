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

using DevExpress.XtraGrid.Columns;
using lis.client.control;

namespace dcl.client.dicmic
{
    public partial class ConDefAntiType : ConDicCommon, IBarActionExt
    {
        public ConDefAntiType()
        {
            InitializeComponent();
            this.Name = "ConDefAntiType";
        }

        private void ConDefAntiType_Load(object sender, EventArgs e)
        {
            InitComboBox();
            this.DoRefresh();
        }

        private void InitComboBox()
        {
            //List<string> listDuoNai = new List<string>();
            //listDuoNai.Add("单种抗生素大类判断多耐");
            //listDuoNai.Add("多种抗生素大类判断多耐");
            //cbbDuoNai.Properties.Items.AddRange(listDuoNai);
            //cbbDuoNai.SelectedIndex = 0;
        }

        // 是否为新增事件
        private bool blIsNew = false;
        //全局变量 状态颜色数据
        List<EntityDefAntiType> listMicGachan = new List<EntityDefAntiType>();

        /// <summary>
        /// 新增
        /// </summary>
        public void Add()
        {
            blIsNew = true;

            foreach (GridColumn column in gridView1.Columns)
            {
                column.FilterInfo = new ColumnFilterInfo();
            }

            EntityDefAntiType staColor = (EntityDefAntiType)micStaGachan.AddNew();
            staColor.DtID = string.Empty;
            staColor.DelFlag = Convert.ToInt32(LIS_Const.del_flag.OPEN);

            micStaGachan.EndEdit();
            micStaGachan.ResetCurrentItem();
            this.gridControl1.Enabled = false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        public void Edit()
        {
            this.gridControl1.Enabled = false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            this.micStaGachan.EndEdit();
            if (micStaGachan.Current == null)
            {
                return;
            }

            EntityRequest request = new EntityRequest();
            EntityDefAntiType eyMicStaCor = (EntityDefAntiType)micStaGachan.Current;

            request.SetRequestValue(eyMicStaCor);

            DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (dresult == DialogResult.OK)
            {
                base.Delete(request); //删除数据

                if (this.isActionSuccess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    //DoRefresh(); //不刷新数据(Remove就好，目的:为了不跳到第一行,方便查看)
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("删除失败");
                }
            }
            DoRefresh();
        }

        private void ChangeData(EntityDefAntiType antiType)
        {
            if (cbbDuoNai.EditValue.ToString() == "单种抗生素大类判断多耐")
            {
                antiType.DtFlag = "0";
            }
            else
            {
                antiType.DtFlag = "1";
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.micStaGachan.EndEdit();
            if (micStaGachan.Current == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(selectDicMicAntibio1.valueMember))
            {
                MessageDialog.Show("请选择抗生素", "提示");
                isActionSuccess = false;

                return;
            }
            if (string.IsNullOrEmpty(selectDicMicBacteria1.valueMember))
            {
                MessageDialog.Show("请选择细菌", "提示");
                isActionSuccess = false;

                return;
            }
            if (string.IsNullOrEmpty(cbbDuoNai.Text))
            {
                MessageDialog.Show("请选择判断多耐", "提示");
                isActionSuccess = false;
                return;
            }

            EntityRequest request = new EntityRequest();

            EntityDefAntiType tubeRack = (EntityDefAntiType)micStaGachan.Current;
            String sco_id = tubeRack.DtID;
            ChangeData(tubeRack);

            request.SetRequestValue(tubeRack);

            EntityResponse result = new EntityResponse();
            if (sco_id == "")
            {
                result = base.New(request); ;//新增保存
            }
            else
            {
                result = base.Update(request); ;//修改更新
            }
            if (base.isActionSuccess)
            {
                if (sco_id == "")
                {
                    tubeRack.DtID = result.GetResult<EntityDefAntiType>().DtID;
                }
                //micStaCor.DoctorDeptName = selectDicPubDept1.displayMember;

                MessageDialog.ShowAutoCloseDialog("保存成功");
                //DoRefresh();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败");
            }
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridControl1.Enabled = true;
            }

            //this.gridControl1.Enabled = true;

            micStaGachan.ResetCurrentItem();
            DoRefresh();
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            //只有新增事件放弃时才对过滤信息重新过滤
            if (blIsNew)
            {
                blIsNew = false;//取消新增事物
                this.gridControl1.Enabled = true;
            }
            DoRefresh();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void DoRefresh()
        {
            EntityResponse ds = base.Search(new EntityRequest());
            if (isActionSuccess)
            {
                listMicGachan = ds.GetResult() as List<EntityDefAntiType>;

                if (radioGroup1.SelectedIndex == 1) //已启用
                {
                    micStaGachan.DataSource = listMicGachan.Where(w => w.DelFlag == 0).ToList();
                }
                else if (this.radioGroup1.SelectedIndex == 2) //未启用
                {
                    micStaGachan.DataSource = listMicGachan.Where(w => w.DelFlag == 1).ToList();
                }
                else  //this.radioGroup1.SelectedIndex == 0 //全部
                {
                    micStaGachan.DataSource = listMicGachan;
                }

            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {

        }


        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            dlist.Add("gridControl1", true);
            dlist.Add(this.splitContainerControl1.Panel1.Name, true);
            dlist.Add(this.radioGroup1.Name, true);
            return dlist;
        }

        public void MoveNext()
        {

        }

        public void MovePrev()
        {

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = txtFilter.Text.Trim();

            if (filter == string.Empty)
                micStaGachan.DataSource = listMicGachan;
            else
            {
                List<EntityDefAntiType> micDataSource = listMicGachan.Where(w => w.DtID.Contains(filter) ||
                                                        w.DtAntiID.ToUpper().Contains(filter.ToUpper()) ||
                                                        w.DtBtID.ToUpper().Contains(filter.ToUpper()) ||
                                                        w.AntiName.ToUpper().Contains(filter.ToUpper()) ||
                                                        w.BtName.ToUpper().Contains(filter.ToUpper())).ToList();

                micStaGachan.DataSource = micDataSource;
            }
        }
        //生成五笔和拼音码的工具
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DoRefresh();
        }
    }
}
