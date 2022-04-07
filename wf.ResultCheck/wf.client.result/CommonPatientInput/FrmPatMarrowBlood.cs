using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.result.Interface;
using dcl.client.frame.runsetting;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.common;

using dcl.entity;
using dcl.client.cache;
using dcl.client.result.CommonPatientInput;
using dcl.common;
using System.Linq;
using System.IO;
using System.Drawing;
using DevExpress.XtraEditors;

namespace dcl.client.result
{
    public partial class FrmPatMarrowBlood : FrmPatInputBaseNew, IPatEnter
    {
        #region Propreties

        /// <summary>
        /// 结果表
        /// </summary>
        List<EntityObrResult> dtPatientResulto
        {
            get;
            set;
        }

        /// <summary>
        /// 结果表
        /// </summary>
        List<EntityObrResultImage> imageList
        {
            get;
            set;
        }

        string AuditWord = "审核";
        string ReportWord = "报告";

        #endregion

        #region Constructor && Load


        public FrmPatMarrowBlood()
        {
            controlPatList.ItrDataType = ItrDataType;
            InitializeComponent();
            this.PatEnter = this;

            this.Load += new System.EventHandler(this.FrmPatMarrowBlood_Load);

            this.ceCombine.CombineAdded += new CombineAddedEventHandler(CombineEditor_CombineAdded);
            this.ceCombine.CombineRemoved += new CombineRemovedEventHandler(CombineEditor_CombineRemoved);
            this.txtPatSampleState.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicSState>.ValueChangedEventHandler(this.txtPatSampleState_ValueChanged);

            controlPatList.ShowUpdateMenu();
        }

        #endregion

        private void FrmPatMarrowBlood_Load(object sender, EventArgs e)
        {
            AuditWord = LocalSetting.Current.Setting.AuditWord;
            ReportWord = LocalSetting.Current.Setting.ReportWord;
            controlPatList.ItrDataType = ItrDataType;
        }

        #region Event

        #region ceCombine套餐事件



        void CombineEditor_CombineRemoved(object sender, string com_id)
        {
        }

        void CombineEditor_CombineAdded(object sender, string com_id, int com_seq)
        {
            AddCombineItems(com_id, com_seq);
            BindResults();
        }

        private void BindResults()
        {
            BsResult.DataSource = this.dtPatientResulto;
            this.gridControlResult.RefreshDataSource();
        }

        private void AddCombineItems(string com_id, int com_seq)
        {
            this.gridViewResult.CloseEditor();

            //根据组合ID获取组合信息
            EntityDicCombine drCombine = DictCombine.Instance.GetCombinebyID(com_id);

            if (drCombine != null)
            {
                string samtypeid = this.txtPatSampleType.valueMember;
                int age = -1;
                if (textAgeInput1.AgeYear != null)
                {
                    age = textAgeInput1.AgeYear.Value;
                }
                string sex = txtPatSex.valueMember;
                string Pat_itr_id = this.controlPatList.ItrID;
                string com_name = drCombine.ComName;

                //获取组合包含的项目
                List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, sex);
                if (dtComItems.Count > 0)
                {
                    List<string> itemsID = new List<string>();

                    //遍历
                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {

                        if (!string.IsNullOrEmpty(drComItem.ComItmId))
                        {
                            string itm_id = drComItem.ComItmId;
                            if (this.dtPatientResulto != null && this.dtPatientResulto.FindAll(i => i.ItmId == itm_id).Count == 0)
                                itemsID.Add(itm_id);//添加到ID集合
                        }
                    }
                    if (itemsID.Count == 0)
                    {
                        return;
                    }


                    ////获取项目：项目信息、项目样本信息、参考值
                    List<EntityItmRefInfo> listItemRefInfo = new ProxyPatResult().Service.GetItemRefInfo(itemsID,
                                                                samtypeid,
                                                                GetConfigOnNullAge(age),
                                                                GetConfigOnNullSex(sex),
                                                                "", Pat_itr_id, "", "");
                    if (listItemRefInfo == null)
                    {
                        return;
                    }

                    foreach (string itm_id in itemsID)
                    {
                        List<EntityItmRefInfo> drItems = listItemRefInfo.FindAll(i => i.ItmId == itm_id);

                        if (drItems.Count > 0)
                        {
                            EntityItmRefInfo drItem = drItems[0];

                            string defValue = null;

                            List<EntityDicCombineDetail> drsCombineMi = dtComItems.FindAll(i => i.ComItmId == itm_id);

                            if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                            {
                                defValue = drItem.ItmDefault;
                            }

                            EntityDicCombineDetail drCombineMi = null;
                            if (drsCombineMi.Count > 0)
                            {
                                drCombineMi = drsCombineMi[0];
                            }
                            AddItem(drItem, drCombineMi, com_id, com_name, com_seq, defValue, null);
                        }
                    }

                }
            }
            NotNullItemCheck();
        }

        private int GetConfigOnNullAge(int age)
        {
            if (age < 0)
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");

                int calage = -1;

                if (!string.IsNullOrEmpty(configCalAge)
                    && configCalAge != "不计算参考值")
                {
                    calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                    if (age >= 0)
                    {
                        calage = age;
                    }
                }
                return calage;
            }
            else
            {
                return age;
            }
        }

        private string GetConfigOnNullSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)

                || (sex != "1"
                && sex != "2"
                && sex != "0"))
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                if (configCalSex.Contains("男"))
                {
                    return "1";
                }
                else if (configCalSex.Contains("女"))
                {
                    return "2";
                }

                return "0";
            }
            else
            {
                return sex;
            }
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="drItem"></param>
        public EntityObrResult AddItem(EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, int com_seq, string res_chr, string res_od_chr)
        {
            if (drItem != null)
            {
                string itm_id = drItem.ItmId;

                //项目编号
                string itm_ecd = string.Empty;
                if (!string.IsNullOrEmpty(drItem.ItmEcode))
                {
                    itm_ecd = drItem.ItmEcode;
                }

                string strEcd = SQLFormater.Format(itm_ecd.Trim());

                //查找当前病人结果表中的项目是否已存在
                List<EntityObrResult> drsResultItems = this.dtPatientResulto.FindAll(i => i.ItmId == itm_id || i.ItmEname == strEcd);

                EntityObrResult drResultItem = null;
                if (drsResultItems.Count == 0)
                {
                    drResultItem = new EntityObrResult();
                    FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                    drResultItem.IsNew = 1;
                    drResultItem.ResComSeq = com_seq;
                    this.dtPatientResulto.Add(drResultItem);
                }
                else
                {
                    EntityObrResult drResultExistItem = drsResultItems[0];
                    int row_state = drResultExistItem.IsNew;


                    if (row_state == 2)//需要添加的项目为已被删除的项目
                    {
                        this.dtPatientResulto.Remove(drResultExistItem);//先把被删除(隐藏)的项目移除，再添加

                        drResultItem = new EntityObrResult();
                        FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                        drResultItem.ResComSeq = com_seq;
                        drResultItem.IsNew = 0;
                        this.dtPatientResulto.Add(drResultItem);
                    }
                    else
                    {
                        drResultItem = drsResultItems[0];
                        if (drResultItem.ObrSn == 0)
                        {
                            drResultItem.IsNew = 1;
                        }
                        else
                        {
                            drResultItem.IsNew = 0;
                        }
                    }
                }

                return drResultItem;
            }
            return null;
        }

        /// <summary>
        /// 将参考值，组合信息等放入结果数据中
        /// </summary>
        /// <param name="drResultItem">待完善的结果数据</param>
        /// <param name="drItem">参考值对象</param>
        /// <param name="drComMi">组合明细</param>
        /// <param name="com_id">组合ID</param>
        /// <param name="com_name">组合名</param>
        /// <param name="itm_id">项目ID</param>
        /// <param name="itm_ecd"></param>
        /// <param name="res_chr"></param>
        /// <param name="res_od_chr"></param>
        private void FillItemToResult(EntityObrResult drResultItem, EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, string itm_id, string itm_ecd, string res_chr, string res_od_chr)
        {
            if (bsPat.Current == null)
            {
                return;
            }

            EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;

            drResultItem.ObrFlag = 1;

            //项目名称
            string itm_name = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmName))
            {
                itm_name = drItem.ItmName;
            }

            //单位
            string itm_unit = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmUnit))
            {
                itm_unit = drItem.ItmUnit;
            }

            string itm_rep_ecd = string.Empty;
            if (!Compare.IsNullOrDBNull(drItem.ItmRepCode) && drItem.ItmRepCode.Trim() != string.Empty)
            {
                itm_rep_ecd = drItem.ItmRepCode;
            }
            else
            {
                itm_rep_ecd = itm_ecd;
            }
            drResultItem.ObrDate = ServerDateTime.GetServerDateTime();
            drResultItem.ObrId = CurrentPatInfo.RepId;
            drResultItem.ItmId = itm_id;
            drResultItem.ObrItmMethod = drItem.ItmMethod;
            drResultItem.ItmEname = itm_ecd.Trim();
            drResultItem.ItmReportCode = itm_rep_ecd;
            drResultItem.ItmName = itm_name;
            drResultItem.ObrUnit = itm_unit;
            drResultItem.ResComName = com_name;
            //drResultItem["pat_com_seq"] = com_seq;
            drResultItem.ItmDtype = drItem.ItmResType;
            drResultItem.ItmMaxDigit = drItem.ItmMaxDigit;

            if (drItem.ItmCaluFlag == 1)
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Cal);
            }
            else if (!string.IsNullOrEmpty(res_chr))
            {
                drResultItem.ObrType = 3;
            }
            else
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Normal);
            }

            drResultItem.ItmSeq = drItem.ItmSortNo;
            drResultItem.ItmPrice = drItem.ItmPrice;
            drResultItem.ItmComId = com_id;

            if (!string.IsNullOrEmpty(drItem.ItmPositiveRes))
                drResultItem.ResPositiveResult = drItem.ItmPositiveRes;

            if (!string.IsNullOrEmpty(drItem.ItmUrgentRes))
                drResultItem.ResCustomCriticalResult = drItem.ItmUrgentRes;

            if (!string.IsNullOrEmpty(drItem.ItmResultAllow))
                drResultItem.ResAllowValues = drItem.ItmResultAllow;

            drResultItem.ResMax = drItem.ItmMaxValue;
            drResultItem.ResMin = drItem.ItmMinValue;

            drResultItem.ResPanH = drItem.ItmDangerUpperLimit;
            drResultItem.ResPanL = drItem.ItmDangerLowerLimit;

            drResultItem.RefUpperLimit = drItem.ItmUpperLimitValue;
            drResultItem.RefLowerLimit = drItem.ItmLowerLimitValue;

            drResultItem.ResMaxCal = drItem.ItmMaxValueCal;
            drResultItem.ResMinCal = drItem.ItmMinValueCal;

            drResultItem.ResPanHCal = drItem.ItmDangerUpperLimitCal;
            drResultItem.ResPanLCal = drItem.ItmDangerLowerLimitCal;

            drResultItem.ResRefHCal = drItem.ItmUpperLimitValueCal;
            if (drResultItem.ResRefHCal == null) drResultItem.ResRefHCal = string.Empty;

            drResultItem.ResRefLCal = drItem.ItmLowerLimitValueCal;
            if (drResultItem.ResRefLCal == null) drResultItem.ResRefLCal = string.Empty;

            if (drResultItem.ResRefLCal.Trim() != string.Empty
                && drResultItem.ResRefHCal.Trim() != string.Empty)
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + " - " + drResultItem.ResRefHCal.ToString().Trim();
            }
            else
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + drResultItem.ResRefHCal.ToString().Trim();
            }

            if (drComMi != null)
            {
                drResultItem.IsNotEmpty = drComMi.ComMustItem;
                drResultItem.ComMiSort = drComMi.ComSortNo;
            }
            drResultItem.ObrValue = res_chr;

        }

        /// <summary>
        /// 判断必须和非空项目数目
        /// </summary>
        public void NotNullItemCheck()
        {
            List<EntityPidReportDetail> patients_mi = null;
            if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
            {
                patients_mi = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);
            }
            if (patients_mi != null && patients_mi.Count > 0)
            {
                string sex = txtPatSex.valueMember;
                //必录项目
                List<string> listNotNullItem = new List<string>();

                List<string> listNotNullItemHasResult = new List<string>();

                //遍历当前病人检验组合
                foreach (EntityPidReportDetail drPatComMi in patients_mi)
                {
                    string com_id = drPatComMi.ComId.ToString();

                    //查找组合所有检验项目
                    List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, sex);

                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        string com_itm_id = drComItem.ComItmId;
                        string com_itm_ecd = SQLFormater.Format(drComItem.ComItmEname);

                        if (!listNotNullItem.Exists(i => i == com_itm_id))
                        {
                            if (!string.IsNullOrEmpty(drComItem.ComMustItem))
                            {
                                if (Convert.ToInt32(drComItem.ComMustItem) == 1)
                                {
                                    listNotNullItem.Add(com_itm_id);

                                    if (this.dtPatientResulto != null &&
                                        this.dtPatientResulto.FindAll(i => i.ItmId == com_itm_id &&
                                        (!string.IsNullOrEmpty(i.ObrBldValue) || !string.IsNullOrEmpty(i.ObrBoneValue))).Count > 0)
                                    {
                                        listNotNullItemHasResult.Add(com_itm_id);
                                    }
                                }
                            }
                        }
                    }
                }

                this.lblNotEmptyItem.Text = string.Format("项目数：{0}/{1}", listNotNullItemHasResult.Count, listNotNullItem.Count);
            }
            else
            {
                this.lblNotEmptyItem.Text = string.Format("项目数：{0}/{0}", 0);
            }
        }


        #endregion

        #region 标本状态事件
    
        private void txtPatSampleState_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //样本状态字体颜色大小变化
            string sampleStatus = txtPatSampleState.valueMember;
            if (!string.IsNullOrEmpty(sampleStatus))
            {
                if (!sampleStatus.Equals("合格"))
                {
                    this.txtPatSampleState.PFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
                    this.txtPatSampleState.popupContainerEdit1.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.txtPatSampleState.PFont = new System.Drawing.Font("Tahoma", 9F);
                    this.txtPatSampleState.popupContainerEdit1.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
        #endregion

        #region 镜检

        //镜检
        private void SysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
            if (CurrentPatInfo == null)
            {
                MessageDialog.Show("请选择需要镜检的记录");
                return;
            }
            if (CurrentPatInfo.RepStatus.ToString() != "0" || CurrentPatInfo.RepInitialFlag.ToString() != "0")
            {
                MessageDialog.Show(string.Format("该记录已{0}或已{1}，无法镜检！", this.AuditWord, this.ReportWord));
                return;
            }
            if (this.txtPatSampleType.valueMember == "" || this.txtPatSampleType.valueMember == null)
            {
                MessageDialog.Show("请输入标本类别！", "提示");
                txtPatSampleType.Focus();
                return;
            }
            FrmMarrowMark mark = new FrmMarrowMark();
            mark.SampleType = this.txtPatSampleType.displayMember;
            mark.RepId = CurrentPatInfo.RepId;
            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0)
            {

                mark.dtPatientResulto = this.dtPatientResulto;
                if (this.imageList != null && this.imageList.Count > 0)
                {
                    mark.result_images = this.imageList;
                }
            }
            mark.ShowDialog();
            if (mark.IsSave && mark.cellMarkList != null && mark.cellMarkList.Count > 0)
            {
                List<EntityObrResultImage> result_images = mark.resultImageList;
                List<EntityObrResult> obr_results = SetBsResult(mark.cellMarkList);
                UpdateResults(obr_results, result_images);
                //Save_Click(null, null);
            }
        }

        private List<EntityObrResult> SetBsResult(List<EntityObrCellsMark> cellmarklist)
        {
            List<EntityObrResult> entityObrResult = new List<EntityObrResult>();
            List<string> ItemId = new List<string>();
            foreach (EntityObrCellsMark cellmark in cellmarklist)
            {
                ItemId.Add(cellmark.ItemId);
            }
            HashSet<string> hs = new HashSet<string>(ItemId);
            foreach (string Id in hs)
            {
                EntityObrResult obrResult = new EntityObrResult();
                var celllist = cellmarklist.FindAll(x => x.ItemId == Id);
                if (celllist != null && celllist.Count > 0)
                {
                    var cellmarkBone = cellmarklist.FindAll(x => x.ItemId == Id);
                    if (cellmarkBone != null && cellmarkBone.Count > 0 && cellmarkBone.Find(x => !string.IsNullOrEmpty(x.ObrBoneValue)) != null)
                    {
                        string ObrBoneValue = cellmarkBone.Find(x => !string.IsNullOrEmpty(x.ObrBoneValue)).ObrBoneValue.ToString();
                        if (!string.IsNullOrEmpty(ObrBoneValue))
                            obrResult.ObrBoneValue = ObrBoneValue;
                    }
                    var cellmarkBld = cellmarklist.FindAll(x => x.ItemId == Id);
                    if (cellmarkBld != null && cellmarkBld.Count > 0 && cellmarkBld.Find(x => !string.IsNullOrEmpty(x.ObrBldValue)) != null)
                    {
                        string ObrBldValue = cellmarkBld.Find(x => !string.IsNullOrEmpty(x.ObrBldValue)).ObrBldValue.ToString();
                        if (!string.IsNullOrEmpty(ObrBldValue))
                            obrResult.ObrBldValue = ObrBldValue;
                    }
                    obrResult.ItmId = Id;
                    entityObrResult.Add(obrResult);
                }
            }
            return entityObrResult;
        }

        /// <summary>
        /// 将镜检界面结果返回给检验项目
        /// </summary>
        /// <param name="obr_results"></param>
        /// <param name="result_images"></param>
        private void UpdateResults(List<EntityObrResult> obr_results, List<EntityObrResultImage> result_images)
        {
            if (this.dtPatientResulto != null)
            {
                foreach (EntityObrResult result in obr_results)
                {
                    EntityObrResult cur_result = this.dtPatientResulto.FirstOrDefault(a => a.ItmId == result.ItmId);
                    if (cur_result != null)
                    {
                        cur_result.ObrBldValue = result.ObrBldValue;
                        cur_result.ObrBoneValue = result.ObrBoneValue;
                    }
                }
            }

            this.imageList = result_images;
            UpdatePictures(this.imageList);
            BindResults();

        }

        /// <summary>
        ///  更新图片显示
        /// </summary>
        /// <param name="list_result_img"></param>
        private void UpdatePictures(List<EntityObrResultImage> list_result_img)
        {
            flowLayoutPanel1.Controls.Clear();
            if (list_result_img == null || list_result_img.Count == 0)
            {
                return;
            }
            foreach (EntityObrResultImage img in list_result_img)
            {
                MemoryStream ms = new MemoryStream(img.ObrImage);
                Image img_show = Image.FromStream(ms);
                PictureEdit pic = new PictureEdit();
                ContextMenu emptyMenu = new ContextMenu();
                pic.Properties.ContextMenu = emptyMenu;
                pic.MouseClick += new MouseEventHandler(ImageOperation);
                pic.Size = new Size(81, 62);
                pic.Image = img_show;
                pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                flowLayoutPanel1.Controls.Add(pic);
            }
        }
        private void ImageOperation(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureEdit pic = (PictureEdit)sender;
                FrmImagView imagview = new FrmImagView();
                imagview.imagelist = GetAllImage(this.flowLayoutPanel1);
                imagview.currentImage = pic.Image;
                imagview.ShowDialog();
            }
            else if (e.Button == MouseButtons.Right)
            {
                List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
                listPats = this.controlPatList.GetCheckedPatients();
                EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                if (CurrentPatInfo.RepStatus != 0)
                {
                    string show = string.Format("该记录已{0}或{1}，不能删除", this.AuditWord, this.ReportWord);
                    MessageDialog.Show(show);
                    return;
                }

                if (lis.client.control.MessageDialog.Show("确定删除该图像吗？", "确认", MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
                {
                    PictureEdit pic = (PictureEdit)sender;
                    int index = flowLayoutPanel1.Controls.IndexOf(pic);
                    this.flowLayoutPanel1.Controls.Remove(pic);
                    if (this.imageList != null && this.imageList.Count > 0)
                    {
                        this.imageList.RemoveAt(index);
                        //Save_Click(null, null);
                    }
                }
                else
                    return;

            }
        }

        private List<Image> GetAllImage(Control control)
        {
            List<Image> imaglist = new List<Image>();
            foreach (Control con in control.Controls)
            {
                if (con is PictureEdit)
                {
                    PictureEdit im = con as PictureEdit;
                    imaglist.Add(im.Image);
                }
            }
            return imaglist;
        }

        #endregion

        #endregion


        public void PatDiagChanged(string patDiag)
        {
        }

        public string Save(EntityPidReportMain patient)
        {
            EntityQcResultList resultlist = new EntityQcResultList();

            string pat_id = string.Empty;

            List<EntityObrResultDesc> dtDescResult = null;//controlPatDescResult1.GetResult();
            dtDescResult[0].ObrItrId = this.txtPatInstructment.valueMember;

            List<EntityObrResultDesc> list = new List<EntityObrResultDesc>();

            foreach (EntityObrResultDesc item in dtDescResult)
            {
                EntityObrResultDesc desc = item;
                desc.ObrDate = ServerDateTime.GetServerDateTime();
                desc.ObrId = patient.RepId;
                desc.ObrItrId = patient.RepItrId;
                desc.ObrSid = Convert.ToDecimal(patient.RepSid);
                list.Add(desc);
            }

            EntityRemoteCallClientInfo remoteCaller = new EntityRemoteCallClientInfo();
            remoteCaller.IPAddress = UserInfo.ip;
            remoteCaller.LoginID = UserInfo.loginID;

            resultlist.patient = patient;
            resultlist.patient.ListPidReportDetail = PatEnter.CombineEditor.listRepDetail;
            resultlist.listRepDetail = PatEnter.CombineEditor.listRepDetail;
            resultlist.listDesc = list;

            if (IsNew)
            {
                //无条可录入
                if (Lab_NoBarcodeNeedAuditCheek
                    && (string.IsNullOrEmpty(Lab_NoBarCodeAuditCheckItrExList) ||
                    !Lab_NoBarCodeAuditCheckItrExList.Contains(txtPatInstructment.valueMember))
                     &&
                    (string.IsNullOrEmpty(patient.RepBarCode)))
                {

                    FrmCheckPassword frmCheck = new FrmCheckPassword("NoBarcode_CanInput");
                    frmCheck.Text = "无条码报告录入确认";
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig != DialogResult.OK)
                    {
                        return "";
                    }
                    remoteCaller.Remarks = string.Format("由{0}医生确认录入", frmCheck.OperatorName);
                    remoteCaller.LoginID = frmCheck.OperatorID;
                    remoteCaller.OperationName = frmCheck.OperatorName;
                }

                EntityOperationResult opResult = new ProxyObrResult().Service.InsertPatCommonResult(remoteCaller, resultlist);

                if (opResult.Success)
                {
                    this.SearchPatients(false, pat_id);
                }
                else
                {
                    if (opResult.HasExcaptionError)
                    {
                        lis.client.control.MessageDialog.Show("保存失败", "提示");
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show(OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                    }
                }
            }
            else
            {
                EntityOperationResult opResult = new ProxyObrResult().Service.UpdatePatCommonResult(remoteCaller, resultlist);

                if (opResult.Success)
                {
                }
                else
                {
                    if (opResult.HasExcaptionError)
                    {
                        lis.client.control.MessageDialog.Show("保存失败", "提示");
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show(OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                    }
                }
            }
            return pat_id;

        }

        public void LoadPatientData(string patID, ref EntityPidReportMain patient, ref List<EntityPidReportDetail> listPatCombine)
        {
            ProxyPatResult proxy = new ProxyPatResult();
            //获取病人资料（病人基本信息、病人检验组合、病人普通结果）
            EntityQcResultList resultList = proxy.Service.GetPatientCommonResult(patID, false);

            if (resultList.patient != null)
            {
                //病人资料表
                patient = resultList.patient;
            }
            else
            {
                patient = new EntityPidReportMain();
            }

            if (patient != null)
            {
                //病人组合表
                listPatCombine = patient.ListPidReportDetail;
            }
            else
            {
                listPatCombine = new List<EntityPidReportDetail>();
            }

            ProxyObrResultDesc proxyResultDesc = new ProxyObrResultDesc();
            List<EntityObrResultDesc> dtParDescResult = proxyResultDesc.Service.GetDescResultById(patID);//描述结果
            //this.controlPatDescResult1.LoadDescResult(dtParDescResult);

            //判断是否有权修改检验报告管理信息
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManager") == "是")
            {
                if (!isCanModify)
                {
                    if (!IsNew)
                    {
                        //系统配置：不能修改报告管理信息[模式]
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
                        {
                            if (!string.IsNullOrEmpty(this.txtPatBarCode.Text))
                            {
                                base.setIsModify(false);
                            }
                        }
                        else
                        {
                            base.setIsModify(false);
                            this.ceCombine.Enabled = false;
                        }
                    }
                    else
                    {
                        base.setIsModify(true);
                        this.ceCombine.Enabled = true;
                    }
                }
                else
                {
                    base.setIsModify(true);
                    this.ceCombine.Enabled = true;
                }
            }
            else
            {
                base.setIsModify(true);
                this.ceCombine.Enabled = true;
            }
        }


        #region IPatEnter 成员

        public string[] ToolBarStyle
        {
            get
            {
                string btnCalculationName = this.Lab_DisplaySamReturnButton ? sysToolBar1.btnCalculation.Name : "";

                string btnAuditAndPrint = UserInfo.GetSysConfigValue("Lab_ReportAndPrintCusName");
                string BtnSinglePrint = "";
                if (!string.IsNullOrEmpty(btnAuditAndPrint))
                {
                    BtnSinglePrint = "BtnSinglePrint";
                    sysToolBar1.BtnSinglePrint.Caption = btnAuditAndPrint;
                }

                string btnDeRef = string.Empty;
                if (UserInfo.GetSysConfigValue("Open_HisFeeView") == "是")
                {
                    btnDeRef = "BtnDeRef";
                    sysToolBar1.BtnDeRef.Caption = "费用清单";
                }
                string BtnQualityAudit = string.Empty;

                if (UserInfo.GetSysConfigValue("Lab_EnableNoBarCodeCheck") == "是")
                {
                    BtnQualityAudit = "BtnQualityAudit";
                    sysToolBar1.BtnQualityAudit.Caption = "打印确认";
                }
                string BtnUndoReport = string.Empty;
                if (UserInfo.GetSysConfigValue("lab_Undo_Report_button") == "是")
                {
                    BtnUndoReport = "BtnUndo";
                    string word = LocalSetting.Current.Setting.ReportWord == string.Empty ? "报告" : LocalSetting.Current.Setting.ReportWord;
                    sysToolBar1.BtnUndo.Caption = "取消" + word;
                    sysToolBar1.BtnUndo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    string word1 = LocalSetting.Current.Setting.ReportWord == string.Empty ? "审核" : LocalSetting.Current.Setting.AuditWord;
                    sysToolBar1.BtnUndo2.Caption = "取消" + word1;
                    sysToolBar1.BtnUndo2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                string BtnMirror = "";
                if (UserInfo.GetSysConfigValue("Lab_QuickRegister") == "是")//以后增加配置是否可见，门诊导入暂用于茂名妇幼
                {
                    BtnMirror = sysToolBar1.BtnExport.Name;
                    sysToolBar1.BtnExport.Caption = "镜检";
                    sysToolBar1.OnBtnExportClicked += SysToolBar1_OnBtnExportClicked;
                }

                return new string[] {
                                    "BtnAdd",
                                    "BtnSave",
                                    "BtnDelete",
                                    "BtnRefresh",
                                    "BtnAudit",
                                    "BtnUndo2",
                                    "BtnReport",
                                    "BtnUndo",
                                    "BtnPrint",
                                    this.sysToolBar1.BtnPrintPreview2.Name,
                                    BtnMirror,
                                    "BtnClose",
                                };
            }
        }



        public void ApplyCustomSetting(PatInputRuntimeSetting UserCustomSetting)
        {

        }

        public string ItrDataType
        {
            get { return LIS_Const.InstmtDataType.Marrow; }
        }

        public void TypeChanged(string typeID)
        {

        }

        public void InstructmentChanged(string itr_id)
        {

        }

        public void Reset()
        {
            //this.controlPatDescResult1.Reset();
        }

        public void ResReset()
        {
        }

        public void AddNew()
        {
            //this.controlPatDescResult1.Reset();

            //新增时设计所有属性均可编辑
            setIsModify(true);
            this.ceCombine.Enabled = true;
        }

        public void SetItrDefaultCombine(string itr_id)
        {

        }

        /// <summary>
        /// 性别改变
        /// </summary>
        /// <param name="pat_sex"></param>
        public void SexChanged(string pat_sex)
        {

        }
        #endregion

        #region IPatEnter 成员


        public void PatIDChanged(string PatIDType, string PatID)
        {

        }

        public void PatIDTypeChanged(string PatIDType)
        {

        }

        public void DepChanged(string depid)
        {
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="listPat"></param>
        public List<EntityOperationResult> DeleteBatch(EntityRemoteCallClientInfo caller, List<string> listPat)
        {
            DialogResult delDiaResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNoCancel);

            List<EntityOperationResult> listOpResult = null;
            if (delDiaResult != DialogResult.Cancel)
            {
                bool bDelResult = (delDiaResult == DialogResult.Yes);

                ProxyObrResultDesc proxy = new ProxyObrResultDesc();
                listOpResult = proxy.Service.BatchDelPatDescResult(caller, listPat, bDelResult);
            }
            else
            {
                listOpResult = new List<EntityOperationResult>();
            }
            return listOpResult;
        }


        #endregion

        #region IPatEnter 成员


        public void ResultView(DateTime date, string itr_id)
        {

        }

        #endregion

        #region IPatEnter 成员


        public ICombineEditor CombineEditor
        {
            get { return this.ceCombine; }
        }

        #endregion

        #region IPatEnter 成员

        public void PatAgeChanged(int ageMinute)
        {

        }

        public void SampleChanged(string sam_id)
        {

        }

        #endregion

        #region IPatEnter 成员


        public void QualityImageView(DateTime date, string itr_id, string itr_mid)
        {
        }

        #endregion

        #region IPatEnter 成员

        public void SetColumnFocus()
        {
        }

        #endregion

        #region IPatEnter 成员


        public void PatDateChanged(DateTime dt)
        {

        }

        #endregion

        #region IPatEnter 成员


        public bool HasNotManualResult()
        {
            return false;
        }

        public bool ShouldCheckWhenPatSidLeave
        {
            get { return false; }
        }


        public void PatSIDChanged(string pat_id, bool merge)
        {
            // IsDataChange = true; 
        }

        public bool CheckResultBeforeAction(string pat_id, bool isAudit)
        {
            try
            {
                ProxyPatResult proxy = new ProxyPatResult();
                //获取病人资料（病人基本信息、病人检验组合、病人普通结果）
                EntityQcResultList resultList = proxy.Service.GetPatientCommonResult(pat_id, false);

                //病人资料表
                EntityPidReportMain patient = resultList.patient;

                //病人组合表
                listPatCombine = patient.ListPidReportDetail;

                ProxyObrResultDesc proxyResultDesc = new ProxyObrResultDesc();
                List<EntityObrResultDesc> dtParDescResult = proxyResultDesc.Service.GetDescResultById(pat_id);//描述结果

                if (patient != null)
                {
                    if (patient.RepStatus != null && (patient.RepStatus.ToString() == "1" || patient.RepStatus.ToString() == "2" || patient.RepStatus.ToString() == "4"))
                        return true;
                }
                if (dtParDescResult != null && dtParDescResult.Count > 0 && !string.IsNullOrEmpty(dtParDescResult[0].ObrDescribe))
                {
                    //if (dtParDescResult[0].ObrDescribe != controlPatDescResult1.GetBsr_describe())
                    //{
                    //    return false;
                    //}
                    return false;
                }

                //if (string.IsNullOrEmpty(txtPatName.Text)
                //    && string.IsNullOrEmpty(txtPatID.Text)
                //    && string.IsNullOrEmpty(controlPatDescResult1.GetBsr_describe()))
                //    return true;

                return !IsDataChange;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("操作失败" + ex.Message, 3m);
                return false;
            }
        }

        #endregion

    }
}
