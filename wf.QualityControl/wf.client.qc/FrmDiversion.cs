using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using System.Linq;
using lis.client.control;

namespace dcl.client.qc
{
    public partial class FrmDiversion : ConCommon
    {
        #region 全局变量
        ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmDiversion");
        
        List<EntityDicQcConvert> listQcValue = new List<EntityDicQcConvert>();
        #endregion
        public FrmDiversion()
        {
            InitializeComponent();
            login();
        }

        private void login()
        {
            EntityResponse dt = proxy.Search(new EntityRequest());
            listQcValue = dt.GetResult() as List<EntityDicQcConvert>;
            this.bdqcvalueset.DataSource = listQcValue;
            
            EntityResponse dtInstrmt = proxy.View(new EntityRequest());
            List<EntityDicInstrument> dtdi = dtInstrmt.GetResult() as List<EntityDicInstrument>;
            this.bddict_instrmt.DataSource = dtdi;
            
            EntityResponse dtqcItm = proxy.Other(new EntityRequest());
            List<EntityDicItmItem> dtqcValue = dtqcItm.GetResult() as List<EntityDicItmItem>;
            dtqcValue = dtqcValue.Where(w => w.ItmDelFlag == "0").OrderBy(i => i.ItmId).ToList();
            this.ctlRepositoryItemLookUpEdit2.DataSource = dtqcValue;

        }

        #region 关闭按钮
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //  this.Close();
        }
        #endregion

        #region 新增按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.bdqcvalueset.AddNew();

            BtnShowDisplay(false);
        }
        #endregion

        private void BtnShowDisplay(bool isShow)
        {
            if (isShow)
            {
                this.sysToolBar1.BtnAdd.Enabled = true;
                //this.sysToolBar1.BtnDelete.Enabled = true;
            }
            else
            {
                this.sysToolBar1.BtnAdd.Enabled = false;
                //this.sysToolBar1.BtnDelete.Enabled = false;
            }

        }

        #region 删除按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.bdqcvalueset.EndEdit();
            if (bdqcvalueset.Current == null)
            {
                lis.client.control.MessageDialog.Show("请选择要删除的数据！", "提示");
                return;
            }
            
            EntityDicQcConvert dr = bdqcvalueset.Current as EntityDicQcConvert;
            string qcr_id = dr.CovSn;
            
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dr);

            if (qcr_id == "")
            {
                this.listQcValue.Remove(dr);
            }
            else
            {
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        proxy.Delete(request);
                        break;
                    case DialogResult.Cancel:
                        return;

                }
            }
            if (base.isActionSuccess)
            {
                if (qcr_id != "")
                {
                    bdqcvalueset.Remove(dr);
                }
            }
            //login(); //刷新数据

            BtnShowDisplay(true);
        }
        #endregion

        #region 保存按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            if (bdqcvalueset.Current == null)//这里永远都不会为空，因为是实体，所以需要可行的判断语句
            {
                lis.client.control.MessageDialog.Show("请选择要保存的数据！", "提示");
                return;
            }

            EntityDicQcConvert eyQcconvert = bdqcvalueset.Current as EntityDicQcConvert;

            bdqcvalueset.EndEdit();

            if (string.IsNullOrEmpty(eyQcconvert.ItrId))
            {
                lis.client.control.MessageDialog.Show("请输入仪器代码！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(eyQcconvert.ItmId))
            {
                lis.client.control.MessageDialog.Show("请输入项目！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(eyQcconvert.ItmValue))
            {
                lis.client.control.MessageDialog.Show("请输入原始值！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(eyQcconvert.ItmConvertValue))
            {
                lis.client.control.MessageDialog.Show("请输入表示值！", "提示");
                return;
            }
            try
            {
                double cast = Convert.ToDouble(eyQcconvert.ItmConvertValue);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("表示值请输入数字！", "提示");
                return;
            }
            sysToolBar1.Focus();
            
            EntityRequest request = new EntityRequest();

            request.SetRequestValue(eyQcconvert);

            EntityResponse result = new EntityResponse();
            
            if (eyQcconvert.CovSn == null)
                result = proxy.New(request);
            else
                result = proxy.Update(request);
            if (base.isActionSuccess)
            {
                if (eyQcconvert.CovSn == null)
                {
                    eyQcconvert.CovSn = result.GetResult<EntityDicQcConvert>().CovSn;
                }

                MessageDialog.ShowAutoCloseDialog("操作成功!");
            }

            this.bdqcvalueset.ResetCurrentItem();
            //login();  //刷新数据

            BtnShowDisplay(true);
        }
        #endregion

        private void FrmDiversion_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnDelete", "BtnSave", "BtnClose" });
        }

    }
}
