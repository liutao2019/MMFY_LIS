using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;

namespace dcl.client.qc
{
    public partial class FrmRoom : FrmCommon
    {
        public FrmRoom()
        {
            InitializeComponent();
            login();
            base.ShowSucessMessage = false;

            this.lueType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.lueType_ValueChanged);
            this.lue_Instrmt.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.lue_Instrmt_ValueChanged);

        }

        private void login()
        {
            deStart.EditValue = DateTime.Now.Date.AddMonths(-1);
            deEnd.EditValue = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
        }

        string userTypes = string.Empty;

        string userQcTypes = string.Empty;
        private bool enableEiasaMath = false;
        private List<EntityObrResult> resultoTB = new List<EntityObrResult>();

        //全局变量，存储测定仪器下拉控件过滤之后的数据
        List<EntityDicInstrument> listLueInstrmt = new List<EntityDicInstrument>();
        List<EntityDicPubProfession> listLueType = new List<EntityDicPubProfession>();

        private void FrmRoom_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            enableEiasaMath = ConfigHelper.GetSysConfigValueWithoutLogin("QualityRoom_EnableEiasaMath") == "是";
            if (enableEiasaMath)
            {
                sysToolBar1.SetToolButtonStyle(new string[] { "BtnSearch", "BtnImport", "btnCalculation" });
            }
            else
            {
                sysToolBar1.SetToolButtonStyle(new string[] { "BtnSearch", "BtnImport" });
            }
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserItrsQc.Count > 0)
                {
                    listLueInstrmt = lue_Instrmt.getDataSource().FindAll(i => UserInfo.entityUserInfo.UserItrsQc.FindIndex(w => w.ItrId == i.ItrId) > -1);
                    //（测定仪器）
                    lue_Instrmt.SetFilter(listLueInstrmt);
                }
                else
                    lue_Instrmt.SetFilter(new List<EntityDicInstrument>());
            }
            else
            {
                listLueInstrmt = lue_Instrmt.getDataSource();
            }

            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserQcLab.Count > 0)
                {
                    listLueType = lueType.getDataSource().FindAll(i => UserInfo.entityUserInfo.UserQcLab.FindIndex(w => w.LabId == i.ProId) > -1);
                    //(实验组)
                    lueType.SetFilter(listLueType);
                }
                else
                    lueType.SetFilter(new List<EntityDicPubProfession>());
            }
            else
            {
                listLueType = lueType.getDataSource();
            }

        }

        private void lueType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            lue_Instrmt.displayMember = "";
            lue_Instrmt.valueMember = "";

            //过滤条件
            if (!string.IsNullOrEmpty(lueType.valueMember))
            {
                if (listLueInstrmt.Count > 0)
                    lue_Instrmt.SetFilter(listLueInstrmt.Where(w => w.ItrLabId == lueType.valueMember).ToList());
                else
                    lue_Instrmt.SetFilter(new List<EntityDicInstrument>());
            }
        }

        private void lue_Instrmt_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            if (lue_Instrmt.valueMember != string.Empty)
            {
                List<EntityDicQcMateria> listQcMateria = new List<EntityDicQcMateria>();

                ProxyQcMateria proxyQcMateria = new ProxyQcMateria();
                listQcMateria = proxyQcMateria.Service.SearchQcMateriaByMatId(lue_Instrmt.valueMember, null);

                gcDate.DataSource = listQcMateria;
            }
        }

        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            string sqlWhere = string.Empty;

            if (lue_Instrmt.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }

            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ObrFlag = "1"; //报告有效标志

            resultQc.StartObrDate = deStart.DateTime.ToString();
            resultQc.EndObrDate = deEnd.DateTime.Date.AddDays(1).AddSeconds(-1).ToString();//查询语句中没有等号，故先加一天再减一秒

            string strItrId = lue_Instrmt.valueMember;
            if (lue_Instrmt.selectRow != null && lue_Instrmt.selectRow.ItrReportItrId != null && lue_Instrmt.selectRow.ItrReportItrId.Trim() != string.Empty)
                strItrId = lue_Instrmt.selectRow.ItrReportItrId.Trim();
            resultQc.ItrId = strItrId;

            if (lue_Item.valueMember != string.Empty && lue_Item.valueMember != null)
            {
                resultQc.ItmId = lue_Item.valueMember;
            }
            if (txtSid.Text.Trim() != string.Empty)
            {
                resultQc.RepSid = txtSid.Text.Trim();//样本号
            }

            List<EntityObrResult> listObrResult = new List<EntityObrResult>();

            ProxyObrResult proxyObrResult = new ProxyObrResult();
            listObrResult = proxyObrResult.Service.GetObrResultQuery(resultQc, false);

            gcPatients.DataSource = listObrResult;
            resultoTB = listObrResult;
        }

        private void sysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {
            if (lue_Instrmt.valueMember == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }

            if (gvDate.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请选择要导入的批号！");
                return;
            }
            #region 查询检验结果数据

            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ObrFlag = "1"; //报告有效标志

            resultQc.StartObrDate = deStart.DateTime.ToString();
            resultQc.EndObrDate = deEnd.DateTime.Date.AddDays(1).AddSeconds(-1).ToString();//查询语句中没有等号，故先加一天再减一秒

            string strItrId = lue_Instrmt.valueMember;
            if (lue_Instrmt.selectRow != null && lue_Instrmt.selectRow.ItrReportItrId != null && lue_Instrmt.selectRow.ItrReportItrId.Trim() != string.Empty)
                strItrId = lue_Instrmt.selectRow.ItrReportItrId.Trim();
            resultQc.ItrId = strItrId;

            if (lue_Item.valueMember != string.Empty && lue_Item.valueMember != null)
            {
                resultQc.ItmId = lue_Item.valueMember;
            }
            if (txtSid.Text.Trim() != string.Empty)
            {
                //resultQc.listItmIds.Add(txtSid.Text.Trim());//样本号
                resultQc.RepSid = txtSid.Text.Trim(); //样本号
            }

            if (ckMB.Checked) //是否勾选酶标
            {
                resultQc.IsCheckMB = true;
            }
            else
            {
                resultQc.ResChrIsNull = true;
            }

            List<EntityObrResult> listResultData = new List<EntityObrResult>();
            ProxyObrResult proxyObrResult = new ProxyObrResult();

            listResultData = proxyObrResult.Service.GetObrResultQuery(resultQc, false);//查询检验结果数据
            #endregion

            EntityDicQcMateria eyQcMateriaDate = gvDate.GetFocusedRow() as EntityDicQcMateria;

            List<EntityObrResult> patiens = gcPatients.DataSource as List<EntityObrResult>;

            if (lis.client.control.MessageDialog.Show("是否要导入质控数据？", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProxyObrQcResult proxyObrQcRes = new ProxyObrQcResult();
                bool isSave = true;
                if (enableEiasaMath && gcPatients.DataSource != null && patiens != null && patiens.Count > 0)
                {
                    //插入数据到质控结果表（从控件中获取数据后插入到质控结果表中）
                    foreach (var info in patiens)
                    {
                        if (string.IsNullOrEmpty(info.ObrValue))
                            continue;
                        bool isSaveDetail = false;
                        isSaveDetail = proxyObrQcRes.Service.InsertQcResult(lue_Instrmt.valueMember, info.ItmId, info.ObrValue,
                                                             eyQcMateriaDate.MatLevel, eyQcMateriaDate.MatSn, info.ObrDate);
                        if (isSaveDetail == false)
                            isSave = false;
                    }
                }
                else
                {
                    //插入数据到质控结果表(查询出检验结果表数据后插入到质控结果表)
                    foreach (var eyObrResult in listResultData)
                    {
                        bool isSaveDetail = false;
                        isSaveDetail = proxyObrQcRes.Service.InsertQcResult(eyObrResult.ObrItrId, eyObrResult.ItmId, eyObrResult.ObrValue,
                                                      eyQcMateriaDate.MatLevel, eyQcMateriaDate.MatSn, eyObrResult.ObrDate);
                        if (isSaveDetail == false)
                            isSave = false;
                    }
                }

                if (isSave)
                {
                    lis.client.control.MessageDialog.Show("操作成功！");
                    return;
                }
            }
        }

        private void sysToolBar1_BtnCalculationClick(object sender, EventArgs e)
        {
            try
            {
                if (resultoTB != null && resultoTB.Count > 0)
                {
                    List<EntityObrResult> resCopy = new List<EntityObrResult>();
                    resCopy = resultoTB;
                    Hashtable hs = new Hashtable();
                    foreach (var info in resCopy)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(info.ObrValue))
                            {
                                hs.Add(info.ItmEname, info.ObrValue);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    gcPatients.DataSource = resCopy;
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }
        
    }
}
