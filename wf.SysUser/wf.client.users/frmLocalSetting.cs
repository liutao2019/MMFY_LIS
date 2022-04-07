using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using lis.client.control;
using dcl.client.frame;
using dcl.client.common;
using Lib.LogManager;
using System.Collections;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.users
{
    public partial class frmLocalSetting : FrmCommon
    {
        /// <summary>
        /// 仪器ID集
        /// </summary>
        [Description("仪器ID集")]
        string list_itr_id { get; set; }

        /// <summary>
        /// 物理组ID集
        /// </summary>
        [Description("物理组ID集")]
        string list_type_id { get; set; }

        public frmLocalSetting()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnSaveClicked += new EventHandler(sysToolBar1_OnBtnSaveClicked);
        }

        void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();

            LocalSetting.Current.Setting.HospitalID = this.txtHospital.valueMember;
            LocalSetting.Current.Setting.HospitalName = this.txtHospital.displayMember;

            LocalSetting.Current.Setting.Barcode_Dep_id = this.txtBarcodeDept.valueMember;
            LocalSetting.Current.Setting.Barcode_Dep_name = this.txtBarcodeDept.displayMember;

            LocalSetting.Current.Setting.CType_id = this.txtType.valueMember;
            LocalSetting.Current.Setting.CType_name = this.txtType.displayMember;

            LocalSetting.Current.Setting.Description = this.txtDescription.Text;

            LocalSetting.Current.Setting.CachePwTime = this.CachePwTime.Text;

            LocalSetting.Current.Setting.IsUrgentNotity = this.cbIsUNotify.Checked ? "1" : "0";

            LocalSetting.Current.Setting.IsItrUrgentNotity = this.cbIsItrNotify.Checked ? "1" : "0";

            LocalSetting.Current.Setting.IsQCNotify = this.cbIsQCNotify.Checked ? "1" : "0";

            LocalSetting.Current.Setting.IsTATNotify = this.cbIsTATNotify.Checked ? "1" : "0";

            LocalSetting.Current.Setting.IsBcTATNotify = this.cbIsBcTATNotify.Checked ? "1" : "0";

            LocalSetting.Current.Setting.ItrIDList = list_itr_id;//仪器ID集合

            LocalSetting.Current.Setting.TypeIDList = list_type_id;//物理组ID集合

            LocalSetting.Current.Setting.LocalItrID = lue_itr_id.valueMember;

            LocalSetting.Current.Setting.LabResultShowType = ckShowName.Checked ? "1" : "0";

            LocalSetting.Current.Setting.BloodWindow = txtBloodWindow.Text;

            LocalSetting.Current.Setting.BloodArea = txtBloodArea.Text;

           
            string strcombines = "";
            foreach (object item in lstSamples.CheckedItems)
            {
                EntityDicSample rv = item as EntityDicSample; //数据源是DataTable,对应的Item是DataRowView类型。 

                strcombines += rv.SamId + ",";
            }
            LocalSetting.Current.Setting.MzDefaultSam = strcombines.TrimEnd(',');

            if (lueTypes.valueMember != null && lueTypes.valueMember.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < lueTypes.valueMember.Count; i++)
                {
                    sb.Append("," + lueTypes.valueMember[i].ToString());
                }
                sb.Remove(0, 1);

                LocalSetting.Current.Setting.IDTypeFlag = sb + "|" + lueTypes.displayMember;
            }
            else
            {
                LocalSetting.Current.Setting.IDTypeFlag = "";
            }


            try
            {
                LocalSetting.Current.Save();
                MessageDialog.ShowAutoCloseDialog("保存成功");
            }
            catch (Exception ex)
            {
                MessageDialog.Show("保存失败！", "错误");
                Logger.LogException("保存配置", ex);
            }
        }


        private void frmLocalSetting_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSave.Name });

            List<EntityDicInstrument> dtTempInstrmt = CacheClient.GetCacheCopy<EntityDicInstrument>();//读取缓存数据，就不会在每次打开时还残余上次操作的记录
            List<EntityDicPubProfession> dtTempPhyType = CacheClient.GetCacheCopy<EntityDicPubProfession>();//跟上面一样
            List<EntityDicSample> dtSam = CacheClient.GetCacheCopy<EntityDicSample>();                                                                            //实验组(过滤掉专业组)

            lstSamples.DataSource = dtSam;


            dtTempPhyType = dtTempPhyType.Where(w => w.ProType == 1).ToList();
            foreach (EntityDicPubProfession type in dtTempPhyType)
            {
                if (string.IsNullOrEmpty(type.ProSortNo.ToString()))
                {
                    type.ProSortNo = 999;
                }
            }


            if (dtTempInstrmt != null && dtTempInstrmt.Count > 0
                && dtTempPhyType != null && dtTempPhyType.Count > 0)
            {
                for (int j = 0; j < dtTempInstrmt.Count; j++)
                {
                    List<EntityDicPubProfession> drTempPhyType = dtTempPhyType.Where(i => i.ProId == dtTempInstrmt[j].ItrLabId).ToList();
                    if (drTempPhyType != null && drTempPhyType.Count > 0)
                    {
                        dtTempInstrmt[j].ItrTypeName = drTempPhyType[0].ProName;
                    }
                }
            }

            bindingSourceItr.DataSource = dtTempInstrmt;
            bindingSourceType.DataSource = dtTempPhyType;

            this.txtBarcodeDept.valueMember = LocalSetting.Current.Setting.Barcode_Dep_id;
            this.txtBarcodeDept.displayMember = LocalSetting.Current.Setting.Barcode_Dep_name;

            this.txtType.valueMember = LocalSetting.Current.Setting.CType_id;
            this.txtType.displayMember = LocalSetting.Current.Setting.CType_name;

            this.txtDescription.Text = LocalSetting.Current.Setting.Description;

            this.CachePwTime.Text = LocalSetting.Current.Setting.CachePwTime;

            this.txtHospital.valueMember = LocalSetting.Current.Setting.HospitalID;
            this.txtHospital.displayMember = LocalSetting.Current.Setting.HospitalName;


            txtBloodWindow.Text = LocalSetting.Current.Setting.BloodWindow;

            txtBloodArea.Text = LocalSetting.Current.Setting.BloodArea;

            if (LocalSetting.Current.Setting.IsUrgentNotity == "0")
            {
                this.cbIsUNotify.Checked = false;
            }
            else if (LocalSetting.Current.Setting.IsUrgentNotity == "1")
            {
                this.cbIsUNotify.Checked = true;
            }
            else
            {
                this.cbIsUNotify.Checked = ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_LocalSetting") == "是";
            }

            if (LocalSetting.Current.Setting.IsItrUrgentNotity == "1")
            {
                this.cbIsItrNotify.Checked = true;
            }
            else
            {
                this.cbIsItrNotify.Checked = false;
            }

            if (LocalSetting.Current.Setting.IsQCNotify == "1")
            {
                this.cbIsQCNotify.Checked = true;
            }
            else
            {
                this.cbIsQCNotify.Checked = false;
            }

            if (LocalSetting.Current.Setting.IsTATNotify == "1")
            {
                this.cbIsTATNotify.Checked = true;
            }
            else
            {
                this.cbIsTATNotify.Checked = false;
            }

            if (LocalSetting.Current.Setting.IsBcTATNotify == "1")
            {
                this.cbIsBcTATNotify.Checked = true;
            }
            else
            {
                this.cbIsBcTATNotify.Checked = false;
            }

            if (LocalSetting.Current.Setting.LabResultShowType == "1")
                ckShowName.Checked = true;
            else
                ckShowCode.Checked = true;

            lue_itr_id.valueMember = LocalSetting.Current.Setting.LocalItrID;
            lue_itr_id.SelectByID(LocalSetting.Current.Setting.LocalItrID);


            if (!string.IsNullOrEmpty(LocalSetting.Current.Setting.IDTypeFlag) && LocalSetting.Current.Setting.IDTypeFlag.Length > 1)
            {
                string[] ids = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[0].Split(',');

                ArrayList alType = new ArrayList();
                foreach (string id in ids)
                {
                    alType.Add(id);
                }

                lueTypes.valueMember = alType;
                lueTypes.displayMember = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[1];
            }

            
            string MzCombines = LocalSetting.Current.Setting.MzDefaultSam;
            if(!string.IsNullOrEmpty(MzCombines))
            {
                List<string> str = new List<string>(MzCombines.Replace("，",",").Split(','));
                for (int i = 0; i < lstSamples.ItemCount; i++)
                {
                    if(str.Contains(lstSamples.GetItemValue(i)))
                    {
                        lstSamples.SetItemChecked(i,true);
                    }
                    else
                        lstSamples.SetItemChecked(i, false);
                }
            }
            

            #region 加载仪器ID集合信息

            list_itr_id = LocalSetting.Current.Setting.ItrIDList;
            if ((!string.IsNullOrEmpty(list_itr_id)) && list_itr_id.Contains("'"))
            {
                try
                {
                    if (dtTempInstrmt != null && dtTempInstrmt.Count > 0)
                    {
                        //这里是有in来过滤，所以用Contains可能会有点问题
                        List<EntityDicInstrument> drpaItrID = dtTempInstrmt.Where(i => list_itr_id.Contains(i.ItrId)).ToList();

                        if (drpaItrID.Count > 0)
                        {
                            foreach (EntityDicInstrument dr in drpaItrID)
                            {
                                dr.Checked = true;
                            }
                            //gvApparatus.CloseEditor();
                            bindingSourceItr.EndEdit();
                            txtBingdingItrID.Text = "已设置" + drpaItrID.Count + "台";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 加载仪器ID错误时,不报错,采用默认值
                    LocalSetting.Current.Setting.ItrIDList = list_itr_id = "";
                }
            }

            #endregion

            #region 加载物理组ID集合信息

            list_type_id = LocalSetting.Current.Setting.TypeIDList;
            if ((!string.IsNullOrEmpty(list_type_id)) && list_type_id.Contains("'"))
            {
                try
                {
                    if (dtTempPhyType != null && dtTempPhyType.Count > 0)
                    {
                        //这里过滤出来的东西可能会有点问题
                        List<EntityDicPubProfession> drpaphttype = dtTempPhyType.Where(i => list_type_id.Contains(i.ProId)).ToList();
                        if (drpaphttype.Count > 0)
                        {
                            foreach (EntityDicPubProfession dr in drpaphttype)
                            {
                                dr.Checked = true;
                            }

                            gvDictPhyType.CloseEditor();
                            bindingSourceType.EndEdit();

                            txtBingdingTypeID.Text = "已设置" + drpaphttype.Count + "个";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 加载物理组ID错误时,不报错,采用默认值
                    LocalSetting.Current.Setting.TypeIDList = list_type_id = "";
                }
            }

            #endregion

            //根据实验组过滤仪器
            List<EntityDicInstrument> instrmList = this.lue_itr_id.getDataSource() as List<EntityDicInstrument>;
            if (!string.IsNullOrEmpty(this.txtType.valueMember))
            {
                this.lue_itr_id.SetFilter(instrmList.FindAll(w => w.ItrLabId == this.txtType.valueMember));
            }
        }

        private void txtBingdingItrID_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            gvApparatus.CloseEditor();
            bindingSourceItr.EndEdit();
            if (bindingSourceItr.DataSource != null && bindingSourceItr.DataSource is List<EntityDicInstrument>)
            {
                List<EntityDicInstrument> dvItr = bindingSourceItr.DataSource as List<EntityDicInstrument>;
                if (dvItr != null && dvItr.Count > 0)
                {
                    List<EntityDicInstrument> drpaItrID = dvItr.Where(w => w.Checked == true).ToList();
                    int kcount = drpaItrID.Count;
                    if (kcount > 0)
                    {
                        txtBingdingItrID.Text = "已设置" + kcount.ToString() + "台";
                        list_itr_id = "";
                        foreach (var info in drpaItrID)
                        {
                            if (string.IsNullOrEmpty(list_itr_id))
                            {
                                list_itr_id = string.Format("'{0}'", info.ItrId);
                            }
                            else
                            {
                                list_itr_id += string.Format(",'{0}'", info.ItrId);
                            }
                        }
                    }
                    else
                    {
                        txtBingdingItrID.Text = "未设置";
                        list_itr_id = "";
                    }
                }
            }
        }


        private void txtBingdingTypeID_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {

            gvDictPhyType.CloseEditor();  //实现实时刷新勾选的数据
            bindingSourceType.EndEdit();

            if (bindingSourceType.DataSource != null && bindingSourceType.DataSource is List<EntityDicPubProfession>)
            {
                List<EntityDicPubProfession> dvphytype = bindingSourceType.DataSource as List<EntityDicPubProfession>;
                if (dvphytype != null && dvphytype.Count > 0)
                {
                    List<EntityDicPubProfession> drpaTypeID = dvphytype.Where(w => w.Checked == true).ToList();
                    int kcount = drpaTypeID.Count;
                    if (kcount > 0)
                    {
                        txtBingdingTypeID.Text = "已设置" + kcount.ToString() + "个";

                        list_type_id = "";
                        foreach (var info in drpaTypeID)
                        {
                            if (string.IsNullOrEmpty(list_type_id))
                            {
                                list_type_id = string.Format("'{0}'", info.ProId);
                            }
                            else
                            {
                                list_type_id += string.Format(",'{0}'", info.ProId);
                            }
                        }
                    }
                    else
                    {
                        txtBingdingTypeID.Text = "未设置";
                        list_type_id = "";
                    }
                }
            }
        }

        private void ckLabShowType_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            if (ck.Checked)
            {
                if (ck.Name == "ckShowName")
                    ckShowCode.Checked = !ck.Checked;
                else
                    ckShowName.Checked = !ck.Checked;

            }
        }

        bool isChange = true;
        private void txtType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (isChange)
            {
                List<EntityDicInstrument> instruList = this.lue_itr_id.getDataSource() as List<EntityDicInstrument>;

                if (!string.IsNullOrEmpty(this.lue_itr_id.valueMember))
                {
                    this.lue_itr_id.valueMember = "";
                    this.lue_itr_id.displayMember = "";
                }

                if (!string.IsNullOrEmpty(this.txtType.valueMember))
                {
                    instruList = instruList.FindAll(w => w.ItrLabId == this.txtType.valueMember);
                }

                this.lue_itr_id.SetFilter(instruList);
            }
        }

        private void lueItrId_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (string.IsNullOrEmpty(this.txtType.valueMember))
            {
                string ctype_id = DictInstrmt.Instance.GetItrCTypeID(this.lue_itr_id.valueMember);

                if (!string.IsNullOrEmpty(ctype_id))
                {
                    EntityDicPubProfession rowCType = DictType.Instance.GetCType(ctype_id);

                    if (rowCType != null)
                    {
                        isChange = false;
                        this.txtType.valueMember = ctype_id;
                        this.txtType.displayMember = rowCType.ProName;
                        isChange = true;
                    }
                }
            }
        }
    }
}
